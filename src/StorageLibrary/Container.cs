using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using StorageLibrary.Common;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageLibrary.Util;
using Azure.Storage.Blobs.Models;
using System.Threading;

namespace StorageLibrary
{
	public class Container
	{
		public static async Task<List<CloudBlobContainerWrapper>> ListContainersAsync(string account, string key)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));

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

        public static async Task<List<BlobItemWrapper>> ListBlobsAsync(string account, string key, string containerName, string path)
        {

            BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));

            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            List<BlobItemWrapper> results = new List<BlobItemWrapper>();
            await foreach (BlobItem blobItem in container.GetBlobsAsync(BlobTraits.None, BlobStates.None, path, CancellationToken.None))
            {
                BlobClient blobClient = container.GetBlobClient(blobItem.Name);


                results.Add(new BlobItemWrapper
                {
                    Name = blobClient.Name,
                    Uri = blobClient.Uri
                });
            }

            return results;

            //CloudBlobContainer container = Get(account, key, containerName);

                //BlobContinuationToken continuationToken = null;
                //List<IListBlobItem> results = new List<IListBlobItem>();
                //do
                //{
                //    var response = await container.ListBlobsSegmentedAsync(path, false, BlobListingDetails.Metadata, null, continuationToken, new BlobRequestOptions(), new Microsoft.WindowsAzure.Storage.OperationContext());
                //    continuationToken = response.ContinuationToken;
                //    results.AddRange(response.Results);
                //}
                //while (continuationToken != null);

                //return results;
        }

        //public static CloudBlobContainer Get(string account, string key, string containerName)
        //{
        //	if (string.IsNullOrEmpty(containerName))
        //		return null;

        //	CloudBlobClient blobClient = Client.GetBlobClient(account, key);
        //	return blobClient.GetContainerReference(containerName);
        //}

        //public static async Task DeleteAsync(string account, string key, string containerName)
        //{
        //	if (string.IsNullOrEmpty(containerName))
        //		return;

        //	List<CloudBlobContainer> containers = await ListContainersAsync(account, key);
        //	foreach (CloudBlobContainer container in containers)
        //	{
        //		if (container.Name == containerName)
        //		{
        //			await container.DeleteAsync();
        //			break;
        //		}
        //	}
        //}

        //public static async Task CreateAsync(string account, string key, string containerName, bool publicAccess)
        //{
        //	if (string.IsNullOrEmpty(containerName))
        //		return;

        //	CloudBlobClient blobClient = Client.GetBlobClient(account, key);
        //	Uri uri = new Uri($"{blobClient.BaseUri}{containerName}");
        //	CloudBlobContainer cont = new CloudBlobContainer(uri, blobClient.Credentials);
        //	await cont.CreateAsync();

        //	if (publicAccess)
        //	{
        //		BlobContainerPermissions permissions = new BlobContainerPermissions
        //		{
        //			PublicAccess = BlobContainerPublicAccessType.Container
        //		};
        //		await cont.SetPermissionsAsync(permissions);
        //	}
        //}

        //public static async Task DeleteBlobAsync(string account, string key, string blobUrl)
        //{
        //	if (string.IsNullOrEmpty(blobUrl))
        //		return;

        //	CloudBlobClient blobClient = Client.GetBlobClient(account, key);
        //	ICloudBlob blobRef = await blobClient.GetBlobReferenceFromServerAsync(new Uri(blobUrl));

        //	await blobRef.DeleteAsync();
        //}

        //public static async Task CreateBlobAsync(string account, string key, string containerName, string fileName, Stream fileContent)
        //{
        //	if (string.IsNullOrEmpty(containerName))
        //		return;

        //	CloudBlobClient blobClient = Client.GetBlobClient(account, key);
        //	CloudBlobContainer container = blobClient.GetContainerReference(containerName);
        //	CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

        //	if (fileContent.Length < (64 * 1024 * 1024)) // 64 MB
        //		await blob.UploadFromStreamAsync(fileContent);
        //	else
        //	{
        //		string tmpPath = Path.GetRandomFileName();
        //		fileContent.CopyTo(tmpPath);
        //		await blob.UploadFromFileAsync(tmpPath);
        //	}
        //}

        //public static async Task<string> GetBlob(string account, string key, string blobUrl)
        //{
        //	if (string.IsNullOrEmpty(blobUrl))
        //		return null;

        //	CloudBlobClient blobClient = Client.GetBlobClient(account, key);
        //	ICloudBlob blob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(blobUrl));

        //	string tmpPath = Path.GetTempFileName();
        //	await blob.DownloadToFileAsync(tmpPath, FileMode.Create);

        //	return tmpPath;
        //}
    }
}
