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
		Dictionary<string, List<string>> files = new Dictionary<string, List<string>>()
		{
			{ "shareOne", new List<string> {"fromOne:1", "fromOne:2", "fromOne:3"}},
			{ "shareTwo", new List<string> {"fromTwo:1", "fromTwo:2"}},
			{ "shareThree", new List<string> {"fromThree:1"}}
		};

		public async Task CreateFileAsync(string share, string fileName, Stream fileContent, string folder = null)
		{
			await Task.Run(() => {
				if (!files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (files[share].Contains(fileName))
					throw new InvalidOperationException($"File '{fileName}' already exists in Sare '{share}'");

				files[share].Add(fileName);
			});
		}

		public async Task CreateSubDirectory(string share, string folder, string subDir)
		{
			await Task.Run(() => {
				if (!files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (files[share].Contains($"{folder}/{subDir}"))
					throw new InvalidOperationException($"Dir '{subDir}' already exists in Sare '{share}'");

				files[share].Add($"{folder}/{subDir}");
			});
		}

		public async Task DeleteFileAsync(string share, string file, string folder = null)
		{
			await Task.Run(() => {
				if (!files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!files[share].Contains(file))
					throw new InvalidOperationException($"File '{file}' does not exist in share '{share}'");

				files[share].Remove(file);
			});
		}

		public async Task<string> GetFile(string share, string file, string folder = null)
		{
			return await Task.Run(() => {
				if (!files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!files[share].Contains(file))
					throw new NullReferenceException($"File '{file}' does not exist in Share '{share}'");

				return file;
			});
		}

		public async Task<List<FileShareItemWrapper>> ListFilesAndDirsAsync(string share, string folder = null)
		{
			return await Task.Run(() => {
				if (!files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");

				List<FileShareItemWrapper> results = new List<FileShareItemWrapper>();
				var rand = new Random();
				foreach(string val in files[share])
					results.Add(new FileShareItemWrapper(val, false, rand.NextInt64(512, 5* 1024 * 1024)));

				return results;
			});
		}

		public async Task<List<FileShareWrapper>> ListFileSharesAsync()
		{
			return await Task.Run(() => {
				List<FileShareWrapper> retList = new List<FileShareWrapper>();
				foreach(string key in files.Keys)
					retList.Add(new FileShareWrapper { Name = key });
				
				return retList;
			});
		}
	}
}
