using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary
{
	internal class AzureQueue : StorageObject, IQueue
	{
		public AzureQueue(string account, string key, string endpoint)
		: base(account, key, endpoint) { }
		public async Task<List<string>> ListQueuesAsync()
		{
			QueueServiceClient client = new QueueServiceClient(ConnectionString);
			List<string> results = new List<string>();
			await foreach (var q in client.GetQueuesAsync())
				results.Add(q.Name);

			return results;
		}

		public async Task<List<PeekedMessageWrapper>> GetAllMessagesAsync(string queueName)
		{
			QueueClient queueClient = new QueueClient(ConnectionString, queueName);
			PeekedMessage[] msgs = await queueClient.PeekMessagesAsync(queueClient.MaxPeekableMessages);

			List<PeekedMessageWrapper> results = new List<PeekedMessageWrapper>();

			foreach (PeekedMessage m in msgs)
				results.Add(new PeekedMessageWrapper { Id = m.MessageId, Message = m.MessageText });

			return results;
		}

		public async Task DequeueMessage(string queueName)
		{
			QueueClient queueClient = new QueueClient(ConnectionString, queueName);

			QueueMessage msg = await queueClient.ReceiveMessageAsync();
			await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
		}

		public async Task CreateAsync(string queueName)
		{
			QueueServiceClient client = new QueueServiceClient(ConnectionString);
			await client.CreateQueueAsync(queueName);
		}

		public async Task DeleteAsync(string queueName)
		{
			QueueClient queueClient = new QueueClient(ConnectionString, queueName);
			await queueClient.DeleteAsync();
		}

		public async Task CreateMessageAsync(string queueName, string message)
		{
			QueueClient queueClient = new QueueClient(ConnectionString, queueName);
			await queueClient.SendMessageAsync(message);
		}
	}
}
