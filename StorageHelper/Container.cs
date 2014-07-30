using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using StorageHelper.Util;

namespace StorageHelper
{
	public class Container
	{
		public static event EventHandler ContainerCreated;

		public static IEnumerable<CloudBlobContainer> GetAll(string account, string key)
		{
			CloudBlobClient blobClient = Client.GetBlobClient(account, key);

			foreach (CloudBlobContainer container in blobClient.ListContainers())
				yield return container;
		}

		public static CloudBlobContainer Get(string account, string key, string containerName)
		{
			if (string.IsNullOrEmpty(containerName))
				return null;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);

			return blobClient.GetContainerReference(containerName);
		}

		public static void Delete(string account, string key, string containerName)
		{
			if (string.IsNullOrEmpty(containerName))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			foreach (CloudBlobContainer container in blobClient.ListContainers())
			{
				if (container.Name == containerName)
				{
					container.Delete();
					break;
				}
			}
		}

		public static void Create(string account, string key, string containerName, bool publicAccess)
		{
			if (string.IsNullOrEmpty(containerName))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			CloudBlobContainer cont = new CloudBlobContainer(containerName, blobClient);
			cont.Create();

			if (publicAccess)
			{
				BlobContainerPermissions permissions = new BlobContainerPermissions();
				permissions.PublicAccess = BlobContainerPublicAccessType.Container;
				cont.SetPermissions(permissions);
			}
		}

		public static void CreateAsync(string account, string key, string containerName)
		{
			if (string.IsNullOrEmpty(containerName))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			CloudBlobContainer cont = new CloudBlobContainer(containerName, blobClient);

			AsyncCallback callback = new AsyncCallback(ResponseReceived);

			IAsyncResult result = cont.BeginCreate(callback, containerName);
		}

		static void ResponseReceived(IAsyncResult result)
		{
			if (ContainerCreated != null)
				ContainerCreated(null, new EventArgs());
		}

		public static void DeleteBlob(string account, string key, string blobUrl)
		{
			if (string.IsNullOrEmpty(blobUrl))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			CloudBlockBlob blockBlob = blobClient.GetBlockBlobReference(blobUrl);

			blockBlob.Delete();
		}

		public static void CreateBlob(string account, string key, string container, string fileName, Stream fileContent)
		{
			if (string.IsNullOrEmpty(container))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			CloudBlockBlob blockBlob = new CloudBlockBlob(container + "\\" + fileName, blobClient);

			if (fileContent.Length < (64 * 1024 * 1024)) // 64 MB
				blockBlob.UploadFromStream(fileContent);
			else
			{
				string tmpPath = Path.GetRandomFileName();
				fileContent.CopyTo(tmpPath);
				blockBlob.UploadFile(tmpPath);
			}
		}

		public static string GetBlob(string account, string key, string blobUrl)
		{
			if (string.IsNullOrEmpty(blobUrl))
				return null;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			CloudBlockBlob blockBlob = blobClient.GetBlockBlobReference(blobUrl);

			string tmpPath = Path.GetTempFileName();
			blockBlob.DownloadToFile(tmpPath);
			return tmpPath;
		}
	}
}
