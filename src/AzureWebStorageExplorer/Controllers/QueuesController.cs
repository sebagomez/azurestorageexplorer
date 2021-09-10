using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using StorageLibrary;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
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
        public async Task<IEnumerable<KeyValuePair<string, string>>> GetMessages(string account, string key, string queue)
        {
            List<PeekedMessageWrapper> messages = await Queue.GetAllMessagesAsync(account, key, queue);

            return messages.Select(m => new KeyValuePair<string, string>(m.Id, m.Message));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DequeueMessage(string account, string key, string queue)
        {
            if (string.IsNullOrEmpty(queue))
                return BadRequest();

            await Queue.DequeueMessage(account, key, queue);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NewQueue(string account, string key, string queue)
        {
            if (string.IsNullOrEmpty(queue))
                return BadRequest();

            await Queue.CreateAsync(account, key, queue);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NewQueueMessage(string account, string key, string queue, string message)
        {
            if (string.IsNullOrEmpty(queue))
                return BadRequest();

            await Queue.CreateMessageAsync(account, key, queue, message);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteQueue(string account, string key, string queue)
        {
            if (string.IsNullOrEmpty(queue))
                return BadRequest();

            await Queue.DeleteAsync(account, key, queue);

            return Ok();
        }
    }
}
