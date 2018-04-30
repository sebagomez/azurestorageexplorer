using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.File;

namespace StorageLibrary
{
    public class File
    {
		public static async Task<List<CloudFileShare>> ListFileSharesAsync(string account, string key)
		{
			CloudFileClient fileClient = Client.GetFileShareClient(account, key);
			FileContinuationToken continuationToken = null;
			List<CloudFileShare> results = new List<CloudFileShare>();
			do
			{
				var response = await fileClient.ListSharesSegmentedAsync(continuationToken);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);

			return results;
		}

		public static async Task<List<IListFileItem>> ListFilesAndDirsAsync(string account, string key, string share)
		{
			CloudFileShare shareRef = Get(account, key, share);
			FileContinuationToken continuationToken = null;
			List<IListFileItem> results = new List<IListFileItem>();
			do
			{
				var response = await shareRef.GetRootDirectoryReference().ListFilesAndDirectoriesSegmentedAsync(continuationToken);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);

			return results;
		}

		public static CloudFileShare Get(string account, string key, string share)
		{
			if (string.IsNullOrEmpty(share))
				return null;

			CloudFileClient fileClient = Client.GetFileShareClient(account, key);

			return fileClient.GetShareReference(share);
		}
	}
}
