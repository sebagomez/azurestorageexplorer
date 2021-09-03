using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
{
    [Produces("application/json")]
    [Route("api/Files")]
    public class FilesController : Controller
    {
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetShares(string account, string key)
		{
			IEnumerable<FileShareWrapper> shares = await StorageLibrary.File.ListFileSharesAsync(account, key);

			return shares.Select(s => s.Name);
		}

        [HttpGet("[action]")]
        public async Task<IEnumerable<FileShareItemWrapper>> GetFilesAndDirectories(string account, string key, string share, string folder)
        {
            return await StorageLibrary.File.ListFilesAndDirsAsync(account, key, share, folder);
        }

        [HttpGet("[action]")]
        public async Task<FileResult> GetFile(string account, string key, string share, string file, string folder= null)
        {
            if (string.IsNullOrEmpty(share))
                return null;

            string filePath = await StorageLibrary.File.GetFile(account, key, share, file, folder);

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            Response.Headers.Add("Content-Disposition", $"inline; filename={HttpUtility.UrlEncode(file)}");

            return File(fileBytes, "application/octet-stream", file);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteFile(string account, string key, string share, string file, string folder = null)
        {
            if (string.IsNullOrEmpty(file))
                return BadRequest();

            await StorageLibrary.File.DeleteFileAsync(account, key, share, file,folder);

            return Ok();
        }
    }
}