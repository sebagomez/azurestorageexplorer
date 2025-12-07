using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
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
				credential = GoogleCredential.FromStream(stream).CreateScoped(StorageService.Scope.DevstorageFullControl);
			}
			// Create the Storage service. 
			_storageService = new StorageService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = AppName,
			});

			var underlyingCredential = credential.UnderlyingCredential as ServiceAccountCredential;
			if (underlyingCredential != null)
				_projectId = underlyingCredential.ProjectId;
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

		public async Task CreateBlobAsync(string bucket, string objectName, Stream fileContent)
		{
			var uploadRequest = new GoogleStorageObject()
			{
				Bucket = bucket,
				Name = objectName
			};

			var mediaUpload = new ObjectsResource.InsertMediaUpload(_storageService, uploadRequest, bucket, fileContent, "application/octet-stream");

			await mediaUpload.UploadAsync();
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

			var listObjects = await listRequest.ExecuteAsync();

			List<BlobItemWrapper> blobs = new List<BlobItemWrapper>();
			string uriTemplate = $"https://storage.cloud.google.com/sebagomeztestbucket/";
			if (listObjects.Items != null)
			{
				foreach (var obj in listObjects.Items)
				{
					if (obj.Name == path)
						continue;

					blobs.Add(new BlobItemWrapper($"{uriTemplate}{obj.Name}", (long)obj.Size, CloudProvider.GCP));
				}
			}

			if (listObjects.Prefixes != null)
				foreach (string commonPrefix in listObjects.Prefixes)
					blobs.Add(new BlobItemWrapper($"{uriTemplate}{commonPrefix}", 0, CloudProvider.GCP));

			return blobs;
		}

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			var buckets = await _storageService.Buckets.List(_projectId).ExecuteAsync();

			List<CloudBlobContainerWrapper> containers = new List<CloudBlobContainerWrapper>();
			foreach (var bucket in buckets.Items)
			{
				containers.Add(new CloudBlobContainerWrapper() { Name = bucket.Name });
			}

			return containers;
		}

		public void Dispose()
		{
			_storageService?.Dispose();
		}
	}
}