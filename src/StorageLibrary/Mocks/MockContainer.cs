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
		Dictionary<string, List<string>> containers = new Dictionary<string, List<string>>()
		{
			// { "one", new List<string> {"fromOne:1", "fromOne:2", "fromOne:3"}},
			// { "two", new List<string> {"fromTwo:1", "fromTwo:2"}},
			// { "three", new List<string> {"fromThree:1"}},
			// { "empty", new List<string> {}},
			{ "with-folders", new List<string> {"root-file", "folder1/", "folder1/file"}},
		};

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			return await Task.Run(() => {
				List<CloudBlobContainerWrapper> retList = new List<CloudBlobContainerWrapper>();
				foreach(string key in containers.Keys)
					retList.Add(new CloudBlobContainerWrapper { Name = key });
				
				return retList;
			});
		}

		public async Task<List<BlobItemWrapper>> ListBlobsAsync(string containerName, string path)
		{
			return await Task.Run(() => {
				if (!containers.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");

				List<BlobItemWrapper> results = new List<BlobItemWrapper>();
				foreach(string val in containers[containerName])
				{
					int slash = val.LastIndexOf("/");
					if (!string.IsNullOrEmpty(path))
					{
						if (val == path)
							continue;

						if (slash >= 0 && val.Substring(0,slash + 1) == path)
						{
							results.Add(new BlobItemWrapper { Name = val, Url = val });		
						}
					}
					else 
					{
						if (slash < 0 || slash == val.Length -1)
							results.Add(new BlobItemWrapper { Name = val, Url = val });		
					}
				}

				return results;
			});
		}

		public async Task CreateAsync(string containerName, bool publicAccess)
		{
			await Task.Run(() => {
				if (containers.ContainsKey(containerName))
					throw new InvalidOperationException($"Container '{containerName}' already exists");
				
				containers.Add(containerName, new List<string>());
			});
		}

		public async Task DeleteBlobAsync(string containerName, string blobName)
		{
			await Task.Run(() => {
				if (!containers.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");
				
				if (!containers[containerName].Contains(blobName))
					throw new NullReferenceException($"Blob '{blobName}' does not exist in Container '{containerName}'");

				containers[containerName].Remove(containerName);
			});
		}

		public async Task CreateBlobAsync(string containerName, string blobName, Stream fileContent)
		{
			await Task.Run(() => {
				if (!containers.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");
				
				if (containers[containerName].Contains(blobName))
					throw new InvalidOperationException($"Blob '{blobName}' already exists in Container '{containerName}'");

				containers[containerName].Add(blobName);
			});
		}

		public async Task<string> GetBlob(string containerName, string blobName)
		{
			return await Task.Run(() => {
				if (!containers.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");
				
				if (!containers[containerName].Contains(blobName))
					throw new InvalidOperationException($"Blob '{blobName}' does not exist in Container '{containerName}'");

				return blobName;
			});
		}

		public async Task DeleteAsync(string containerName)
		{
			 await Task.Run(() => {
				if (!containers.ContainsKey(containerName))
					throw new NullReferenceException($"Container '{containerName}' does not exist");
				
				containers.Remove(containerName);
			});
		}
	}
}
