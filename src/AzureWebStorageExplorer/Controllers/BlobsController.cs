using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using StorageLibrary;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
{
	[Produces("application/json")]
	[Route("api/Blobs")]
	public class BlobsController : CounterController
	{
		private static readonly Counter BlobCounter = Metrics.CreateCounter("blobscontroller_counter_total", "Keep BlobsController access count");

		[HttpGet("[action]")]
		public async Task<IActionResult> GetBlobs(string account, string key, string container, string path)
		{
			Increment(BlobCounter);

			if (string.IsNullOrEmpty(container))
				return Ok(new List<string>());

			StorageFactory factory = Util.GetStorageFactory(account, key);
			List<BlobItemWrapper> blobs = await factory.Containers.ListBlobsAsync(container, path);

			return Ok(blobs.Select(b => b.Url));
		}

		[HttpGet("[action]")]
		public async Task<FileResult> GetBlob(string account, string key, string container, string blobUri)
		{
			Increment(BlobCounter);

			if (string.IsNullOrEmpty(blobUri))
				return null;

			string fileName = GetFileName(container, blobUri);
			StorageFactory factory = Util.GetStorageFactory(account, key);
			string blobPath = await factory.Containers.GetBlob(container, fileName);

			byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(blobPath);

			return File(fileBytes, "application/octet-stream", fileName.Substring(fileName.LastIndexOf("/") + 1));
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteBlob(string account, string key, string container, string blobUri)
		{
			Increment(BlobCounter);

			if (string.IsNullOrEmpty(blobUri))
				return BadRequest();

			string fileName = GetFileName(container, blobUri);
			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Containers.DeleteBlobAsync(container, fileName);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> UploadBlob(string account, string key, string container, string path, List<IFormFile> files)
		{
			Increment(BlobCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			foreach (IFormFile file in files)
				await factory.Containers.CreateBlobAsync(container, path + file.FileName, file.OpenReadStream());

			return Ok();
		}

		private string GetFileName(string containerName, string blobUri)
		{
			int containerIndex = blobUri.IndexOf(containerName);
			int slash = blobUri.IndexOf("/", containerIndex);

			return blobUri.Substring(slash + 1);
		}
	}
}