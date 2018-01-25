using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Queue;
using StorageLibrary;

namespace AngularWebStorageExplorer.Controllers
{
	[Produces("application/json")]
	[Route("api/Queues")]
	public class QueuesController : Controller
	{
		// GET: api/Queues
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetQueues(string account, string key)
		{
			return await Queue.ListQueuesAsync(account, key);
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetMessages(string account, string key, string queue)
		{
			List<CloudQueueMessage> messages = await Queue.GetAllMessagesAsync(account, key, queue);

			return messages.Select(m => m.AsString);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewQueue(string account, string key, string queue)
		{
			if (string.IsNullOrEmpty(queue))
				return BadRequest();

			await Queue.CreateAsync(account, key, queue);

			return Ok();
		}
	}
}
