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
    [Route("api/Containers")]
    public class ContainersController : Controller
    {
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetContainers()
		{
			List<CloudBlobContainer> containers = await Container.ListContainersAsync(Settings.Instance.Account, Settings.Instance.Key);

			return containers.Select(c => c.Name);
		}
	}
}