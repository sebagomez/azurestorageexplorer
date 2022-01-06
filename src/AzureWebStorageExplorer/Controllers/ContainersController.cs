using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prometheus;

using StorageLibrary;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
{
	[Produces("application/json")]
	[Route("api/Containers")]
	public class ContainersController : CounterController
	{
		private static readonly Counter ContainerCounter = Metrics.CreateCounter("containerscontroller_counter_total", "Keep ContainersController access count");

		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetContainers(string account, string key)
		{
			Increment(ContainerCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

			return containers.Select(c => c.Name);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteContainer(string account, string key, string container)
		{
			Increment(ContainerCounter);

			if (string.IsNullOrEmpty(container))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Containers.DeleteAsync(container);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewContainer(string account, string key, string container, bool publicAccess)
		{
			Increment(ContainerCounter);

			if (string.IsNullOrEmpty(container))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Containers.CreateAsync(container, publicAccess);

			return Ok();
		}
	}
}