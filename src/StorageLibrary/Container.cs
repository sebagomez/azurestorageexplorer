using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StorageLibrary.Common;
using StorageLibrary.Util;

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

            string localPath = "/";
            if (!string.IsNullOrEmpty(path))
                localPath = path;

            await foreach (BlobHierarchyItem blobItem in container.GetBlobsByHierarchyAsync(BlobTraits.None, BlobStates.None, "/", path, CancellationToken.None))
            {
                BlobItemWrapper wrapper = null;
                if (blobItem.IsBlob)
                {
                    BlobClient blobClient = container.GetBlobClient(blobItem.Blob.Name);

                    wrapper = new BlobItemWrapper
                    {
                        Name = blobClient.Name,
                        Url = blobClient.Uri.AbsoluteUri
                    };

                }
                else if (blobItem.IsPrefix)
                {
                    wrapper = new BlobItemWrapper
                    {
                        Name = blobItem.Prefix,
                        Url = $"{container.Uri}{localPath}{blobItem.Prefix}"
                    };
                }

                if (wrapper != null && !results.Contains(wrapper))
                    results.Add(wrapper);
            }

            return results;
        }

        public static async Task DeleteAsync(string account, string key, string containerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            await container.DeleteAsync();
        }

        public static async Task CreateAsync(string account, string key, string containerName, bool publicAccess)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            PublicAccessType accessType = publicAccess ? PublicAccessType.BlobContainer : PublicAccessType.None;
            await container.CreateAsync(accessType);
        }

        public static async Task DeleteBlobAsync(string account, string key, string containerName, string blobName)
        {

            BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blob = container.GetBlobClient(blobName);

            await blob.DeleteAsync();
        }

        public static async Task CreateBlobAsync(string account, string key, string containerName, string blobName, Stream fileContent)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            await container.UploadBlobAsync(blobName, fileContent);
        }

        public static async Task<string> GetBlob(string account, string key, string containerName, string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(Client.GetConnectionString(account, key));
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blob = container.GetBlobClient(blobName);

            string tmpPath = Path.GetTempFileName();
            await blob.DownloadToAsync(tmpPath);

            return tmpPath;
        }
    }
}
