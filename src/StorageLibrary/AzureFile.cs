using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary
{
	internal class AzureFile : StorageObject, IFile
	{
		public AzureFile(StorageFactoryConfig config)
		: base(config) { }

		public async Task<List<FileShareWrapper>> ListFileSharesAsync()
		{
			System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
			ShareServiceClient client = new ShareServiceClient(ConnectionString);

			List<FileShareWrapper> items = new List<FileShareWrapper>();

			var shares = client.GetSharesAsync(ShareTraits.None, ShareStates.None, null, cancellationToken);
			await foreach (var share in shares)
				items.Add(new FileShareWrapper { Name = share.Name });

			return items;
		}

		public async Task<List<FileShareItemWrapper>> ListFilesAndDirsAsync(string share, string folder = null)
		{
			ShareClient client = new ShareClient(ConnectionString, share);

			ShareDirectoryClient dir = GetShareDirectoryClient(share, folder);

			List<FileShareItemWrapper> items = new List<FileShareItemWrapper>();

			var files = dir.GetFilesAndDirectoriesAsync();
			await foreach (var file in files)
			{
				var uriBuilder = new UriBuilder(dir.Uri);
				uriBuilder.Path += $"/{file.Name}";
				var uri = uriBuilder.Uri;
				items.Add(new FileShareItemWrapper(uri.AbsoluteUri, !file.IsDirectory, file.FileSize));
			}

			return items;
		}

		public async Task<string> GetFileAsync(string share, string file, string folder = null)
		{
			ShareFileClient fileClient = GetShareFileClient(share, file, folder);

			string tmpPath = Util.File.GetTempFileName();
			ShareFileDownloadInfo download = await fileClient.DownloadAsync();
			using (FileStream stream = System.IO.File.OpenWrite(tmpPath))
			{
				download.Content.CopyTo(stream);
			}

			return tmpPath;
		}

		public async Task DeleteFileAsync(string share, string file, string folder = null)
		{
			ShareDirectoryClient parentDir = GetShareDirectoryClient(share, folder);

			ShareFileClient fileClient = parentDir.GetFileClient(file);
			bool deleted = await fileClient.DeleteIfExistsAsync();

			if (deleted)
				return;

			ShareDirectoryClient childDir = parentDir.GetSubdirectoryClient(file);
			await childDir.DeleteIfExistsAsync();
		}

		public async Task CreateSubDirectory(string share, string folder, string subDir)
		{
			ShareDirectoryClient parentDir = GetShareDirectoryClient(share, folder);
			await parentDir.CreateSubdirectoryAsync(subDir);
		}

		public async Task CreateFileAsync(string share, string fileName, Stream fileContent, string folder = null)
		{
			ShareFileClient file = GetShareFileClient(share, fileName, folder);
			file.Create(fileContent.Length);
			await file.UploadAsync(fileContent);
		}

		public async Task CreateFileShareAsync(string share, string accessTier = "optimized")
		{
			ShareCreateOptions options = new ShareCreateOptions();
			switch (accessTier.ToLower())
			{
				case "cool":
					options.AccessTier = ShareAccessTier.Cool;
					break;
				case "hot":
					options.AccessTier = ShareAccessTier.Hot;
					break;
				case "optimized":
					options.AccessTier = ShareAccessTier.TransactionOptimized;
					break;
				default:
					throw new Exception($"{accessTier} is not a valid access tier");

			}

			ShareServiceClient client = new ShareServiceClient(ConnectionString);
			await client.CreateShareAsync(share, options);
		}

		public async Task DeleteFileShareAsync(string share)
		{
			ShareServiceClient client = new ShareServiceClient(ConnectionString);
			await client.DeleteShareAsync(share);
		}

		private ShareDirectoryClient GetShareDirectoryClient(string share, string folder)
		{
			ShareClient client = new ShareClient(ConnectionString, share);
			return string.IsNullOrWhiteSpace(folder) ? client.GetRootDirectoryClient() : client.GetDirectoryClient(folder);
		}

		private ShareFileClient GetShareFileClient(string share, string file, string folder = null)
		{
			ShareDirectoryClient dirClient = GetShareDirectoryClient(share, folder);
			return dirClient.GetFileClient(file);
		}
	}
}
