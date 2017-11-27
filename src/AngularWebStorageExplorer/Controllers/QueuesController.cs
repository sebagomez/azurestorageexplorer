using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<IEnumerable<string>> GetQueues()
        {
			return await Queue.ListQueuesAsync(Settings.Instance.Account, Settings.Instance.Key);
        }

		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetMessages(string queue)
		{
			List<CloudQueueMessage> messages = await Queue.GetAllMessagesAsync(Settings.Instance.Account, Settings.Instance.Key, queue);

			return messages.Select(m => m.AsString);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewQueue(string queue)
		{
			if (string.IsNullOrEmpty(queue))
				return BadRequest();

			await Queue.CreateAsync(Settings.Instance.Account, Settings.Instance.Key, queue);

			return Ok();
		}

		// GET: api/Queues/5
		[HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Queues
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Queues/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
