using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Mocks
{
	internal class MockContainer : IContainer
	{
		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			return await Task.Run(() =>
			{
				List<CloudBlobContainerWrapper> retList = new List<CloudBlobContainerWrapper>();
				foreach (string key in MockUtils.FolderStructure.Keys)
					retList.Add(new CloudBlobContainerWrapper { Name = key });

				return retList;
			});
		}

		public async Task<List<BlobItemWrapper>> ListBlobsAsync(string containerName, string path)
		{
			return await Task.Run(() =>
			{
				if (!MockUtils.FolderStructure.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");

				List<BlobItemWrapper> results = new List<BlobItemWrapper>();
				foreach(string url in MockUtils.GetItems(containerName, path))
					results.Add(new BlobItemWrapper(url, MockUtils.NewRandomSize));

				return results;
			});
		}

		public async Task CreateAsync(string containerName, bool publicAccess)
		{
			await Task.Run(() =>
			{
				if (MockUtils.FolderStructure.ContainsKey(containerName))
					throw new InvalidOperationException($"Container '{containerName}' already exists");

				MockUtils.FolderStructure.Add(containerName, new List<string>());
			});
		}

		public async Task DeleteBlobAsync(string containerName, string blobName)
		{
			await Task.Run(() =>
			{
				if (!MockUtils.FolderStructure.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");

				if (!MockUtils.FolderStructure[containerName].Contains(blobName))
					throw new NullReferenceException($"Blob '{blobName}' does not exist in Container '{containerName}'");

				MockUtils.FolderStructure[containerName].Remove(blobName);
			});
		}

		public async Task CreateBlobAsync(string containerName, string blobName, Stream fileContent)
		{
			await Task.Run(() =>
			{
				if (!MockUtils.FolderStructure.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");

				if (MockUtils.FolderStructure[containerName].Contains(blobName))
					throw new InvalidOperationException($"Blob '{blobName}' already exists in Container '{containerName}'");

				BlobItemWrapper blob = new BlobItemWrapper($"{MockUtils.FAKE_URL}/{containerName}/{blobName}");
				if (!MockUtils.FolderStructure[containerName].Contains(blob.Path))
					MockUtils.FolderStructure[containerName].Add(blob.Path);

				MockUtils.FolderStructure[containerName].Add(blob.FullName);
			});
		}

		public async Task<string> GetBlobAsync(string containerName, string blobName)
		{
			return await Task.Run(() =>
			{
				if (!MockUtils.FolderStructure.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");

				if (!MockUtils.FolderStructure[containerName].Contains(blobName))
					throw new InvalidOperationException($"Blob '{blobName}' does not exist in Container '{containerName}'");

				return blobName;
			});
		}

		public async Task DeleteAsync(string containerName)
		{
			await Task.Run(() =>
			{
				if (!MockUtils.FolderStructure.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");

				MockUtils.FolderStructure.Remove(containerName);
			});
		}
	}
}
