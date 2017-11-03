using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageLibrary;

namespace AngularWebStorageExplorer.Controllers
{
    [Produces("application/json")]
    [Route("api/Blobs")]
    public class BlobsController : Controller
    {
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetBlobs(string container)
		{
			if (string.IsNullOrEmpty(container))
				return new List<string>();

			List<IListBlobItem> blobs = await Container.ListBlobsAsync(Settings.Instance.Account, Settings.Instance.Key, container);

			return blobs.Select(b => b.Uri.ToString());
		}

		//[HttpDelete("{blobUri}")]
		//public async Task<IActionResult> DeleteBlob([FromRoute] string blobUri)
		//{
		//	if (string.IsNullOrEmpty(blobUri))
		//		return BadRequest();

		//	await Container.DeleteBlobAsync(Settings.Instance.Account, Settings.Instance.Key, blobUri);

		//	return Ok();
		//}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteBlob(string blobUri)
		{
			if (string.IsNullOrEmpty(blobUri))
				return BadRequest();

			await Container.DeleteBlobAsync(Settings.Instance.Account, Settings.Instance.Key, blobUri);

			return Ok();
		}
	}
}