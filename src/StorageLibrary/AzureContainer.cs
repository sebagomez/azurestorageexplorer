using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary
{
	internal class AzureContainer : StorageObject, IContainer
	{
		public AzureContainer(string account, string key, string endpoint)
		: base(account, key, endpoint) { }

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);

			List<CloudBlobContainerWrapper> results = new List<CloudBlobContainerWrapper>();
			await foreach (var container in blobServiceClient.GetBlobContainersAsync())
			{
				results.Add(new CloudBlobContainerWrapper
				{
					Name = container.Name
				});
			}

			return results;
		}

		public async Task<List<BlobItemWrapper>> ListBlobsAsync(string containerName, string path)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
			BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

			List<BlobItemWrapper> results = new List<BlobItemWrapper>();

			string localPath = "/";
			if (!string.IsNullOrEmpty(path))
				localPath = path;

			await foreach (BlobHierarchyItem blobItem in container.GetBlobsByHierarchyAsync(BlobTraits.None, BlobStates.None, "/", path, CancellationToken.None))
			{
				BlobItemWrapper wrapper = null;
				if (blobItem.IsBlob)
				{
					BlobClient blobClient = container.GetBlobClient(blobItem.Blob.Name);

					wrapper = new BlobItemWrapper(blobClient.Uri.AbsoluteUri, blobItem.Blob.Properties.ContentLength);
				}
				else if (blobItem.IsPrefix)
				{
					wrapper = new BlobItemWrapper($"{container.Uri}{localPath}{blobItem.Prefix}",0);
				}

				if (wrapper != null && !results.Contains(wrapper))
					results.Add(wrapper);
			}

			return results;
		}

		public async Task DeleteAsync(string containerName)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
			BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

			await container.DeleteAsync();
		}

		public async Task CreateAsync(string containerName, bool publicAccess)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
			BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

			PublicAccessType accessType = publicAccess ? PublicAccessType.BlobContainer : PublicAccessType.None;
			await container.CreateAsync(accessType);
		}

		public async Task DeleteBlobAsync(string containerName, string blobName)
		{

			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
			BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

			BlobClient blob = container.GetBlobClient(blobName);

			await blob.DeleteAsync();
		}

		public async Task CreateBlobAsync(string containerName, string blobName, Stream fileContent)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
			BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

			await container.UploadBlobAsync(blobName, fileContent);
		}

		public async Task<string> GetBlobAsync(string containerName, string blobName)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
			BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

			BlobClient blob = container.GetBlobClient(blobName);

			string tmpPath = Path.GetTempFileName();
			await blob.DownloadToAsync(tmpPath);

			return tmpPath;
		}
	}
}
