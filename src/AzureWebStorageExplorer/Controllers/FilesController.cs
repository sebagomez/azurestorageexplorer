using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureWebStorageExplorer.Controllers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureWebStorageExplorer.Controllers
{
	[Produces("application/json")]
    [Route("api/Files")]
    public class FilesController : Controller
    {
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetShares(string account, string key)
		{
			IEnumerable<CloudFileShare> shares = await StorageLibrary.File.ListFileSharesAsync(account, key);

			return shares.Select(s => s.Name);
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<FileShareItem>> GetFilesAndDirectories(string account, string key, string share, string folder)
		{
			IEnumerable<IListFileItem> files = await StorageLibrary.File.ListFilesAndDirsAsync(account, key, share, folder);

			List<FileShareItem> list = new List<FileShareItem>();

			files.ToList().ForEach(f => {

				FileShareItem item = new FileShareItem
				{
					Name = f is CloudFileDirectory ? (f as CloudFileDirectory).Name : (f is CloudFile) ? (f as CloudFile).Name : "N/A",
					Parent = f.Parent.Name,
					ParentUrl = f.Parent.Uri.AbsoluteUri,
					Url = f.StorageUri.PrimaryUri.AbsoluteUri,
					IsDirectory = f is CloudFileDirectory

				};

				list.Add(item);
			});

			list.Sort();
			return list;
		}
	}
}