using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Mocks
{
	internal class MockFile : IFile
	{
		public async Task CreateFileAsync(string share, string fileName, Stream fileContent, string folder = null)
		{
			await Task.Run(() => {
				if (!MockUtils.FolderStructure.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (MockUtils.FolderStructure[share].Contains(fileName))
					throw new InvalidOperationException($"File '{fileName}' already exists in Sare '{share}'");

				if (!string.IsNullOrEmpty(folder))
					fileName = $"{folder}{fileName}";

				MockUtils.FolderStructure[share].Add(fileName);
			});
		}

		public async Task CreateSubDirectory(string share, string folder, string subDir)
		{
			await Task.Run(() => {
				if (!MockUtils.FolderStructure.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!subDir.EndsWith("/"))
					subDir += "/";

				if (MockUtils.FolderStructure[share].Contains($"{folder}/{subDir}"))
					throw new InvalidOperationException($"Dir '{subDir}' already exists in Sare '{share}'");

				MockUtils.FolderStructure[share].Add($"{folder}/{subDir}");
			});
		}

		public async Task DeleteFileAsync(string share, string file, string folder = null)
		{
			await Task.Run(() => {
				if (!MockUtils.FolderStructure.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!MockUtils.FolderStructure[share].Contains(file))
					throw new InvalidOperationException($"File '{file}' does not exist in share '{share}'");

				MockUtils.FolderStructure[share].Remove(file);
			});
		}

		public async Task<string> GetFileAsync(string share, string file, string folder = null)
		{
			return await Task.Run(() => {
				if (!MockUtils.FolderStructure.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!MockUtils.FolderStructure[share].Contains(file))
					throw new NullReferenceException($"File '{file}' does not exist in Share '{share}'");

				return file;
			});
		}

		public async Task<List<FileShareItemWrapper>> ListFilesAndDirsAsync(string share, string folder = null)
		{
			return await Task.Run(() => {
				if (!MockUtils.FolderStructure.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");

				List<FileShareItemWrapper> results = new List<FileShareItemWrapper>();
				foreach(string url in MockUtils.GetItems(share, folder))
					results.Add(new FileShareItemWrapper(url, !url.EndsWith("/"), MockUtils.NewRandomSize));

				return results;
			});
		}

		public async Task<List<FileShareWrapper>> ListFileSharesAsync()
		{
			return await Task.Run(() => {
				List<FileShareWrapper> retList = new List<FileShareWrapper>();
				foreach(string key in MockUtils.FolderStructure.Keys)
					retList.Add(new FileShareWrapper { Name = key });
				
				return retList;
			});
		}
	}
}
