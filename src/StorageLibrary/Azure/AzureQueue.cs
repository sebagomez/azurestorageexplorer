using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Azure
{
	internal class AzureQueue : StorageObject, IQueue
	{
		public AzureQueue(StorageFactoryConfig config)
		: base(config) { }

		QueueClientOptions ClientOptions => IsAzurite
			? new QueueClientOptions(QueueClientOptions.ServiceVersion.V2024_11_04)
			: new QueueClientOptions();

		QueueServiceClient ServiceClient => new QueueServiceClient(ConnectionString, ClientOptions);
		QueueClient GetQueueClient(string queueName) => new QueueClient(ConnectionString, queueName, ClientOptions);

		public async Task<List<QueueWrapper>> ListQueuesAsync()
		{
			List<QueueWrapper> results = new List<QueueWrapper>();
			await foreach (var q in ServiceClient.GetQueuesAsync())
				results.Add(new QueueWrapper { Name = q.Name });

			return results;
		}

		public async Task<List<PeekedMessageWrapper>> GetAllMessagesAsync(string queueName)
		{
			QueueClient queueClient = GetQueueClient(queueName);
			PeekedMessage[] msgs = await queueClient.PeekMessagesAsync(queueClient.MaxPeekableMessages);

			List<PeekedMessageWrapper> results = new List<PeekedMessageWrapper>();

			foreach (PeekedMessage m in msgs)
				results.Add(new PeekedMessageWrapper { Id = m.MessageId, Message = m.MessageText });

			return results;
		}

		public async Task DequeueMessage(string queueName)
		{
			QueueClient queueClient = GetQueueClient(queueName);
			QueueMessage msg = await queueClient.ReceiveMessageAsync();
			await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
		}

		public async Task CreateAsync(string queueName)
		{
			await ServiceClient.CreateQueueAsync(queueName);
		}

		public async Task DeleteAsync(string queueName)
		{
			await GetQueueClient(queueName).DeleteAsync();
		}

		public async Task CreateMessageAsync(string queueName, string message)
		{
			await GetQueueClient(queueName).SendMessageAsync(message);
		}
	}
}
