using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
			Uri uri = new Uri($"{blobClient.BaseUri}{containerName}");
			CloudBlobContainer cont = new CloudBlobContainer(uri, blobClient.Credentials);
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
			Uri uri = new Uri($"{blobClient.BaseUri}{containerName}");
			CloudBlobContainer cont = new CloudBlobContainer(uri, blobClient.Credentials);

			AsyncCallback callback = new AsyncCallback(ResponseReceived);

			IAsyncResult result = cont.BeginCreate(callback, containerName);
		}

		static void ResponseReceived(IAsyncResult result)
		{
			ContainerCreated?.Invoke(null, new EventArgs());
		}

		public static void DeleteBlob(string account, string key, string blobUrl)
		{
			if (string.IsNullOrEmpty(blobUrl))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			ICloudBlob blobRef = blobClient.GetBlobReferenceFromServer(new Uri(blobUrl));

			blobRef.Delete();
		}

		public static void CreateBlob(string account, string key, string containerName, string fileName, Stream fileContent)
		{
			if (string.IsNullOrEmpty(containerName))
				return;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			CloudBlobContainer container = blobClient.GetContainerReference(containerName);
			CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

			if (fileContent.Length < (64 * 1024 * 1024)) // 64 MB
				blob.UploadFromStream(fileContent);
			else
			{
				string tmpPath = Path.GetRandomFileName();
				fileContent.CopyTo(tmpPath);
				blob.UploadFromFile(tmpPath, AccessCondition.GenerateEmptyCondition());
			}
		}

		public static string GetBlob(string account, string key, string blobUrl)
		{
			if (string.IsNullOrEmpty(blobUrl))
				return null;

			CloudBlobClient blobClient = Client.GetBlobClient(account, key);
			ICloudBlob blob = blobClient.GetBlobReferenceFromServer(new Uri(blobUrl));

			string tmpPath = Path.GetTempFileName();
			blob.DownloadToFile(tmpPath, FileMode.Create);

			return tmpPath;
		}
	}
}
