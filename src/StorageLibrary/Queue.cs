using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageLibrary
{
	public class Queue
	{
		public static async Task<List<string>> ListQueuesAsync(string account, string key)
		{
			CloudQueueClient queueClient = Client.GetQueueClient(account, key);

			QueueContinuationToken continuationToken = null;
			List<CloudQueue> results = new List<CloudQueue>();
			do
			{
				var response = await queueClient.ListQueuesSegmentedAsync(continuationToken);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);

			return results.Select(t => t.Name).ToList();
		}

		
		public static async Task<List<CloudQueueMessage>> GetAllMessagesAsync(string account, string key, string queueName)
		{
			List<CloudQueueMessage> result = new List<CloudQueueMessage>();

			if (string.IsNullOrEmpty(queueName))
				return result;

			CloudQueueClient queueClient = Client.GetQueueClient(account, key);
			CloudQueue queue = queueClient.GetQueueReference(queueName);
			await queue.FetchAttributesAsync();

			if (queue.ApproximateMessageCount.HasValue && queue.ApproximateMessageCount.Value > 0)
			{
				foreach (CloudQueueMessage message in await queue.PeekMessagesAsync(queue.ApproximateMessageCount.Value))
					result.Add(message);
			}

			return result;
		}

		public static async Task DeleteMessage(string account, string key, string queueName, string messageId)
		{
			CloudQueueClient queueClient = Client.GetQueueClient(account, key);
			CloudQueue queue = queueClient.GetQueueReference(queueName);
			await queue.FetchAttributesAsync();

			if (queue.ApproximateMessageCount.HasValue && queue.ApproximateMessageCount.Value > 0)
			{
				bool found = false;
				foreach (CloudQueueMessage message in await queue.GetMessagesAsync(queue.ApproximateMessageCount.Value, TimeSpan.FromMilliseconds(100), null, null))
				{
					if (message.Id == messageId)
					{
						found = true;
						await queue.DeleteMessageAsync(message.Id, message.PopReceipt);
						break;
					}
				}

				if (!found)
					throw new Exception("No message found");
			}
		}

		public static async Task CreateAsync(string account, string key, string queueName)
		{
			if (string.IsNullOrEmpty(queueName))
				return;

			CloudQueueClient queueClient = Client.GetQueueClient(account, key);
			CloudQueue queue = queueClient.GetQueueReference(queueName);
			await queue.CreateIfNotExistsAsync();
		}

		public static async Task DeleteAsync(string account, string key, string queueName)
		{
			if (string.IsNullOrEmpty(queueName))
				return;

			CloudQueueClient queueClient = Client.GetQueueClient(account, key);
			CloudQueue queue = queueClient.GetQueueReference(queueName);
			await queue.DeleteIfExistsAsync();
		}

		public static async Task CreateMessageAsync(string account, string key, string queueName, string message)
		{
			if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
				return;

			CloudQueueClient queueClient = Client.GetQueueClient(account, key);
			CloudQueue queue = queueClient.GetQueueReference(queueName);

			await queue.AddMessageAsync(new CloudQueueMessage(message.Trim()));
		}
	}
}
