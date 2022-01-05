using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using StorageLibrary;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
{
	[Produces("application/json")]
	[Route("api/Files")]
	public class FilesController : CounterController
	{
		private static readonly Counter FilesCounter = Metrics.CreateCounter("filescontroller_counter_total", "Keep FilesController access count");

		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetShares(string account, string key)
		{
			Increment(FilesCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			IEnumerable<FileShareWrapper> shares = await factory.Files.ListFileSharesAsync();

			return shares.Select(s => s.Name);
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<FileShareItemWrapper>> GetFilesAndDirectories(string account, string key, string share, string folder)
		{
			Increment(FilesCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			return await factory.Files.ListFilesAndDirsAsync(share, folder);
		}

		[HttpGet("[action]")]
		public async Task<FileResult> GetFile(string account, string key, string share, string file, string folder= null)
		{
			Increment(FilesCounter);

			if (string.IsNullOrEmpty(share))
				return null;

			StorageFactory factory = Util.GetStorageFactory(account, key);
			string filePath = await factory.Files.GetFile(share, file, folder);

			byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

			Response.Headers.Add("Content-Disposition", $"inline; filename={HttpUtility.UrlEncode(file)}");

			return File(fileBytes, "application/octet-stream", file);

		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteFile(string account, string key, string share, string file, string folder = null)
		{
			Increment(FilesCounter);

			if (string.IsNullOrEmpty(file))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Files.DeleteFileAsync(share, file,folder);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> UploadFile(string account, string key, string share, string folder, string fileName, List<IFormFile> files)
		{
			Increment(FilesCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			foreach (IFormFile file in files)
				await factory.Files.CreateFileAsync(share, fileName, file.OpenReadStream(), folder);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> CreateSubDir(string account, string key, string share, string subDir, string folder = null)
		{
			Increment(FilesCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Files.CreateSubDirectory(share, subDir, folder);

			return Ok();
		}
	}
}