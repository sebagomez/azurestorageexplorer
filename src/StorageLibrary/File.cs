using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using StorageLibrary.Common;
//using Microsoft.WindowsAzure.Storage.File;

namespace StorageLibrary
{
    public class File
    {


		public static async Task<List<FileShareWrapper>> ListFileSharesAsync(string account, string key)
		{
            System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
			ShareServiceClient client = new ShareServiceClient(Client.GetConnectionString(account, key));

			List<FileShareWrapper> items = new List<FileShareWrapper>();

			var shares = client.GetSharesAsync(ShareTraits.None, ShareStates.None, null, cancellationToken);
			await foreach (var share in shares)
			{
				items.Add(new FileShareWrapper { Name = share.Name });	
			}

            return items;

		}

        public static async Task<List<FileShareItemWrapper>> ListFilesAndDirsAsync(string account, string key, string share, string folder = null)
        {
            ShareClient client = new ShareClient(Client.GetConnectionString(account, key), share);

            ShareDirectoryClient dir = GetShareDirectoryClient(account, key, share, folder);

            List<FileShareItemWrapper> items = new List<FileShareItemWrapper>();

            var files = dir.GetFilesAndDirectoriesAsync();
            await foreach(var file in files)
            {
                items.Add(new FileShareItemWrapper { Name = file.Name,
                                                    IsDirectory = file.IsDirectory,
                                                    Parent = dir.Name,
                                                    ParentUrl = dir.Uri.AbsoluteUri,
                                                    Url = $"{dir.Uri.AbsoluteUri}/{file.Name}" });
            }

            return items;

        }

        public static async Task<string> GetFile(string account, string key, string share, string file, string folder = null)
        {
            ShareFileClient fileClient = GetShareFileClient(account, key, share, file, folder);

            string tmpPath = Path.GetTempFileName();
            ShareFileDownloadInfo download = await fileClient.DownloadAsync();
            using (FileStream stream = System.IO.File.OpenWrite(tmpPath))
            {
                download.Content.CopyTo(stream);
            }

            return tmpPath;
        }

        public static async Task DeleteFileAsync(string account, string key, string share, string file, string folder = null)
        {
            ShareDirectoryClient parentDir = GetShareDirectoryClient(account, key, share, folder);

            ShareFileClient fileClient = parentDir.GetFileClient(file);
            bool deleted = await fileClient.DeleteIfExistsAsync();

            if (deleted)
                return;

            ShareDirectoryClient childDir = parentDir.GetSubdirectoryClient(file);
            await childDir.DeleteIfExistsAsync();
        }


        private static ShareDirectoryClient GetShareDirectoryClient(string account, string key, string share, string folder)
        {
            ShareClient client = new ShareClient(Client.GetConnectionString(account, key), share);
            return string.IsNullOrWhiteSpace(folder) ? client.GetRootDirectoryClient() : client.GetDirectoryClient(folder);
        }

        private static ShareFileClient GetShareFileClient(string account, string key, string share, string file, string folder = null)
        {
            ShareDirectoryClient dirClient = GetShareDirectoryClient(account, key, share, folder);
            return dirClient.GetFileClient(file);
        }
    }
}
