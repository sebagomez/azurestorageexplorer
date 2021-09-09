using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageLibrary;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
{
    [Produces("application/json")]
    [Route("api/Blobs")]
    public class BlobsController : Controller
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetBlobs(string account, string key, string container, string path)
        {
            if (string.IsNullOrEmpty(container))
                return Ok(new List<string>());

            List<BlobItemWrapper> blobs = await Container.ListBlobsAsync(account, key, container, path);

            return Ok(blobs.Select(b => b.Url));
        }

        [HttpGet("[action]")]
        public async Task<FileResult> GetBlob(string account, string key, string container, string blobUri)
        {
            if (string.IsNullOrEmpty(blobUri))
                return null;

            string fileName = GetFileName(container, blobUri);
            string blobPath = await Container.GetBlob(account, key, container, fileName);

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(blobPath);

            return File(fileBytes, "application/octet-stream", fileName.Substring(fileName.LastIndexOf("/") + 1));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteBlob(string account, string key, string container, string blobUri)
        {
            if (string.IsNullOrEmpty(blobUri))
                return BadRequest();

            string fileName = GetFileName(container, blobUri);
            await Container.DeleteBlobAsync(account, key, container, fileName);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadBlob(string account, string key, string container, string path, List<IFormFile> files)
        {
            foreach (IFormFile file in files)
                await Container.CreateBlobAsync(account, key, container, path + file.FileName, file.OpenReadStream());

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