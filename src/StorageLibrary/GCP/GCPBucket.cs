using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Storage.v1;
using Google.Apis.Storage.v1.Data;
using GoogleStorageObject = Google.Apis.Storage.v1.Data.Object;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Google
{
	internal class GCPBucket : StorageObject, IContainer, IDisposable
	{
		const string AppName = "Sebagomez Cloud Storage Explorer";
		protected StorageService _storageService;
		protected string _projectId;

		public GCPBucket(StorageFactoryConfig config) : base(config)
		{
			string serviceAccountPath = config.GcpCredentialsFile;
			GoogleCredential credential;
			using (var stream = new FileStream(serviceAccountPath, FileMode.Open, FileAccess.Read))
			{
				var serviceAccountCredential = CredentialFactory.FromStream<ServiceAccountCredential>(stream);
				_projectId = serviceAccountCredential.ProjectId;
				credential = serviceAccountCredential.ToGoogleCredential().CreateScoped(StorageService.Scope.DevstorageFullControl);
			}
			_storageService = new StorageService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = AppName,
			});
		}

		public async Task CreateAsync(string bucket, bool publicAccess)
		{
			var newBucket = new Bucket
			{
				Name = bucket
			};

			var insertRequest = _storageService.Buckets.Insert(newBucket, _projectId);
			await insertRequest.ExecuteAsync();

			if (publicAccess)
			{
				await SetBucketPolicyAsync(bucket);
			}
		}

		private async Task SetBucketPolicyAsync(string bucket)
		{
			var bucketIamPolicy = new Policy
			{
				Bindings = new List<Policy.BindingsData>
					{
						new Policy.BindingsData
						{
							Role = "roles/storage.objectViewer",
							Members = new List<string> { "allUsers" }
						}
					}
			};

			var setIamPolicyRequest = new BucketsResource.SetIamPolicyRequest(_storageService, bucketIamPolicy, bucket);
			await setIamPolicyRequest.ExecuteAsync();
		}

		public async Task CreateBlobAsync(string bucket, string objectName, Stream fileContent, bool overwrite = false)
		{
			var uploadRequest = new GoogleStorageObject()
			{
				Bucket = bucket,
				Name = objectName
			};

			var mediaUpload = new ObjectsResource.InsertMediaUpload(_storageService, uploadRequest, bucket, fileContent, "application/octet-stream");

			// Generation 0 means "no live object": the upload fails with 412 when it exists.
			if (!overwrite)
				mediaUpload.IfGenerationMatch = 0;

			// UploadAsync does not throw. It reports a failure in the returned progress.
			IUploadProgress progress = await mediaUpload.UploadAsync();
			if (progress.Status == UploadStatus.Failed)
			{
				if (!overwrite && progress.Exception is GoogleApiException api && api.HttpStatusCode == HttpStatusCode.PreconditionFailed)
					throw new BlobAlreadyExistsException(bucket, objectName, api);

				throw progress.Exception ?? new IOException($"Upload of '{objectName}' to '{bucket}' failed");
			}
		}

		public async Task DeleteAsync(string bucket)
		{
			var deleteRequest = _storageService.Buckets.Delete(bucket);
			await deleteRequest.ExecuteAsync();
		}

		public async Task DeleteBlobAsync(string bucket, string objectName)
		{
			var deleteRequest = _storageService.Objects.Delete(bucket, objectName);
			await deleteRequest.ExecuteAsync();
		}

		public async Task<string> GetBlobAsync(string bucket, string objectName)
		{
			string tmpPath = Util.File.GetTempFileName();
			using (var outputFile = new FileStream(tmpPath, FileMode.Create, FileAccess.Write))
			{
				var getRequest = _storageService.Objects.Get(bucket, objectName);
				await getRequest.DownloadAsync(outputFile);
			}

			return tmpPath;
		}

		public async Task<List<BlobItemWrapper>> ListBlobsAsync(string bucket, string path)
		{
			var listRequest = _storageService.Objects.List(bucket);
			listRequest.Prefix = path;
			listRequest.Delimiter = "/";

			List<BlobItemWrapper> blobs = new List<BlobItemWrapper>();
			string uriTemplate = $"https://storage.cloud.google.com/{bucket}/";

			// GCS returns at most 1,000 items for each request.
			Objects listObjects;
			do
			{
				listObjects = await listRequest.ExecuteAsync();

				if (listObjects.Items != null)
				{
					foreach (var obj in listObjects.Items)
					{
						// Skip only the folder placeholder object ("folder/"). A prefix that is
						// the full name of a file must still return that file.
						if (obj.Name == path && path.EndsWith("/"))
							continue;

						blobs.Add(new BlobItemWrapper($"{uriTemplate}{obj.Name}", bucket, obj.Name, true, (long)(obj.Size ?? 0), CloudProvider.GCP));
					}
				}

				if (listObjects.Prefixes != null)
					foreach (string commonPrefix in listObjects.Prefixes)
						blobs.Add(new BlobItemWrapper($"{uriTemplate}{commonPrefix}", bucket, commonPrefix, false, 0, CloudProvider.GCP));

				listRequest.PageToken = listObjects.NextPageToken;
			}
			while (!string.IsNullOrEmpty(listObjects.NextPageToken));

			return blobs;
		}

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			List<CloudBlobContainerWrapper> containers = new List<CloudBlobContainerWrapper>();
			var listRequest = _storageService.Buckets.List(_projectId);

			Buckets buckets;
			do
			{
				buckets = await listRequest.ExecuteAsync();

				foreach (var bucket in buckets.Items ?? [])
					containers.Add(new CloudBlobContainerWrapper() { Name = bucket.Name });

				listRequest.PageToken = buckets.NextPageToken;
			}
			while (!string.IsNullOrEmpty(buckets.NextPageToken));

			return containers;
		}

		// Signed PUT URLs (UrlSigner) are not wired up yet; callers fall back to
		// uploading through the server.
		public Task<string> GetBlobUploadUrlAsync(string bucket, string objectName, TimeSpan validFor)
		{
			return Task.FromResult<string>(null);
		}

		public void Dispose()
		{
			_storageService?.Dispose();
		}
	}
}