using System.Collections.Generic;
using System.Threading.Tasks;

using StorageLibrary.Common;

namespace StorageLibrary.Interfaces
{
	public interface IQueue 
	{
		Task<List<QueueWrapper>> ListQueuesAsync();
		Task<List<PeekedMessageWrapper>> GetAllMessagesAsync(string queueName);
		Task DequeueMessage(string queueName);
		Task CreateAsync(string queueName);
		Task DeleteAsync(string queueName);
		Task CreateMessageAsync(string queueName, string message);
	}
}