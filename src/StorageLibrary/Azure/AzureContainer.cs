using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Azure
{
	internal class AzureContainer : StorageObject, IContainer
	{
		public AzureContainer(StorageFactoryConfig config)
		: base(config) { }

		BlobClientOptions ClientOptions => IsAzurite
			? new BlobClientOptions(BlobClientOptions.ServiceVersion.V2024_11_04)
			: new BlobClientOptions();

		BlobServiceClient ServiceClient => new BlobServiceClient(ConnectionString, ClientOptions);

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			List<CloudBlobContainerWrapper> results = new List<CloudBlobContainerWrapper>();
			await foreach (var container in ServiceClient.GetBlobContainersAsync())
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
			BlobContainerClient container = ServiceClient.GetBlobContainerClient(containerName);

			List<BlobItemWrapper> results = new List<BlobItemWrapper>();
			await foreach (BlobHierarchyItem blobItem in container.GetBlobsByHierarchyAsync(BlobTraits.None, BlobStates.None, "/", path, CancellationToken.None))
			{
				BlobItemWrapper wrapper = null;
				if (blobItem.IsBlob)
				{
					BlobClient blobClient = container.GetBlobClient(blobItem.Blob.Name);

					wrapper = new BlobItemWrapper(blobClient.Uri.AbsoluteUri, blobItem.Blob.Properties.ContentLength.HasValue ? blobItem.Blob.Properties.ContentLength.Value : 0, CloudProvider.Azure, IsAzurite);
				}
				else if (blobItem.IsPrefix)
				{
					wrapper = new BlobItemWrapper($"{container.Uri}/{blobItem.Prefix}", 0, CloudProvider.Azure, IsAzurite);
				}

				if (wrapper != null && !results.Contains(wrapper))
					results.Add(wrapper);
			}

			return results;
		}

		public async Task DeleteAsync(string containerName)
		{
			BlobContainerClient container = ServiceClient.GetBlobContainerClient(containerName);
			await container.DeleteAsync();
		}

		public async Task CreateAsync(string containerName, bool publicAccess)
		{
			BlobContainerClient container = ServiceClient.GetBlobContainerClient(containerName);
			PublicAccessType accessType = publicAccess ? PublicAccessType.BlobContainer : PublicAccessType.None;
			await container.CreateAsync(accessType);
		}

		public async Task DeleteBlobAsync(string containerName, string blobName)
		{
			BlobContainerClient container = ServiceClient.GetBlobContainerClient(containerName);
			BlobClient blob = container.GetBlobClient(blobName);
			await blob.DeleteAsync();
		}

		public async Task CreateBlobAsync(string containerName, string blobName, Stream fileContent)
		{
			BlobContainerClient container = ServiceClient.GetBlobContainerClient(containerName);
			await container.UploadBlobAsync(blobName, fileContent);
		}

		public async Task<string> GetBlobAsync(string containerName, string blobName)
		{
			BlobContainerClient container = ServiceClient.GetBlobContainerClient(containerName);
			BlobClient blob = container.GetBlobClient(blobName);
			string tmpPath = Util.File.GetTempFileName();
			await blob.DownloadToAsync(tmpPath);
			return tmpPath;
		}
	}
}
