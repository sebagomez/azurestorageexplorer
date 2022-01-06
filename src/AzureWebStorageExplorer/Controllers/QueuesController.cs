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
	[Route("api/Queues")]
	public class QueuesController : CounterController
	{
		private static readonly Counter QueueCounter = Metrics.CreateCounter("queuescontroller_counter_total", "Keep QueuesController access count");

		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetQueues(string account, string key)
		{
			Increment(QueueCounter);
			StorageFactory factory = Util.GetStorageFactory(account, key);
			return await factory.Queues.ListQueuesAsync();
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<KeyValuePair<string, string>>> GetMessages(string account, string key, string queue)
		{
			Increment(QueueCounter);

			StorageFactory factory = Util.GetStorageFactory(account, key);
			List<PeekedMessageWrapper> messages = await factory.Queues.GetAllMessagesAsync(queue);

			return messages.Select(m => new KeyValuePair<string, string>(m.Id, m.Message));
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DequeueMessage(string account, string key, string queue)
		{
			Increment(QueueCounter);

			if (string.IsNullOrEmpty(queue))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Queues.DequeueMessage(queue);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewQueue(string account, string key, string queue)
		{
			Increment(QueueCounter);

			if (string.IsNullOrEmpty(queue))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Queues.CreateAsync(queue);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewQueueMessage(string account, string key, string queue, string message)
		{
			Increment(QueueCounter);

			if (string.IsNullOrEmpty(queue))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Queues.CreateMessageAsync(queue, message);

			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> DeleteQueue(string account, string key, string queue)
		{
			Increment(QueueCounter);

			if (string.IsNullOrEmpty(queue))
				return BadRequest();

			StorageFactory factory = Util.GetStorageFactory(account, key);
			await factory.Queues.DeleteAsync(queue);

			return Ok();
		}
	}
}
