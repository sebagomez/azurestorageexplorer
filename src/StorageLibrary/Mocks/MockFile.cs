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
		static Dictionary<string, List<string>> s_files = new Dictionary<string, List<string>>()
		{
			{ "shareOne", new List<string> {"fromOne:1", "fromOne:2", "fromOne:3"}},
			{ "shareTwo", new List<string> {"fromTwo:1", "fromTwo:2"}},
			{ "shareThree", new List<string> {"fromThree:1"}},
			{ "shareEmpty", new List<string> {}},
			{ "with-folder", new List<string> {"root-file", "folder1/", "folder1/file"}},
		};

		public async Task CreateFileAsync(string share, string fileName, Stream fileContent, string folder = null)
		{
			await Task.Run(() => {
				if (!s_files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (s_files[share].Contains(fileName))
					throw new InvalidOperationException($"File '{fileName}' already exists in Sare '{share}'");

				s_files[share].Add(fileName);
			});
		}

		public async Task CreateSubDirectory(string share, string folder, string subDir)
		{
			await Task.Run(() => {
				if (!s_files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (s_files[share].Contains($"{folder}/{subDir}"))
					throw new InvalidOperationException($"Dir '{subDir}' already exists in Sare '{share}'");

				s_files[share].Add($"{folder}/{subDir}");
			});
		}

		public async Task DeleteFileAsync(string share, string file, string folder = null)
		{
			await Task.Run(() => {
				if (!s_files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!s_files[share].Contains(file))
					throw new InvalidOperationException($"File '{file}' does not exist in share '{share}'");

				s_files[share].Remove(file);
			});
		}

		public async Task<string> GetFile(string share, string file, string folder = null)
		{
			return await Task.Run(() => {
				if (!s_files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");
				
				if (!s_files[share].Contains(file))
					throw new NullReferenceException($"File '{file}' does not exist in Share '{share}'");

				return file;
			});
		}

		public async Task<List<FileShareItemWrapper>> ListFilesAndDirsAsync(string share, string folder = null)
		{
			return await Task.Run(() => {
				if (!s_files.ContainsKey(share))
					throw new NullReferenceException($"Share '{share}' does not exist");

				List<FileShareItemWrapper> results = new List<FileShareItemWrapper>();
				foreach (string val in s_files[share])
				{
					int slash = val.LastIndexOf("/");

					string[] deep = folder.Split("/", StringSplitOptions.RemoveEmptyEntries);
					string[] dirs = val.Split("/", StringSplitOptions.RemoveEmptyEntries);

					if (!string.IsNullOrEmpty(folder))
					{
						if (val == folder)
							continue;

						if (dirs.Length > (deep.Length + 1) || dirs.Length <= deep.Length)
							continue;

						bool inCurrentDir = true;
						if (dirs.Length >= deep.Length)
						{
							for(int i = 0; i < dirs.Length -1; i++)
							{
								if (dirs[i] != deep[i])
								{
									inCurrentDir &= false;
									break;
								}
							}
						}

						if (inCurrentDir)
							results.Add(new FileShareItemWrapper($"{MockUtils.FAKE_URL}/{share}/{val}", !val.EndsWith("/"), MockUtils.NewRandomSize));
					}
					else
					{
						if (dirs.Length > 1)
							continue;

						results.Add(new FileShareItemWrapper($"{MockUtils.FAKE_URL}/{share}/{val}", !val.EndsWith("/"), MockUtils.NewRandomSize ));
					}
				}

				return results;
			});
		}

		public async Task<List<FileShareWrapper>> ListFileSharesAsync()
		{
			return await Task.Run(() => {
				List<FileShareWrapper> retList = new List<FileShareWrapper>();
				foreach(string key in s_files.Keys)
					retList.Add(new FileShareWrapper { Name = key });
				
				return retList;
			});
		}
	}
}
