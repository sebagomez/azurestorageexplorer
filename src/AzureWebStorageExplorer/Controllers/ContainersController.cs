using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageLibrary;

namespace AzureWebStorageExplorer.Controllers
{
	[Produces("application/json")]
	[Route("api/Containers")]
	public class ContainersController : Controller
	{
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetContainers(string account, string key)
		{
			List<CloudBlobContainer> containers = await Container.ListContainersAsync(account, key);

			return containers.Select(c => c.Name);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteContainer(string account, string key, string container)
		{
			if (string.IsNullOrEmpty(container))
				return BadRequest();

			await Container.DeleteAsync(account, key, container);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewContainer(string account, string key, string container, bool publicAccess)
		{
			if (string.IsNullOrEmpty(container))
				return BadRequest();

			await Container.CreateAsync(account, key, container, publicAccess);

			return Ok();
		}
	}
}