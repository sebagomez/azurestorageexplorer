using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

using StorageLibrary.Common;
using StorageLibrary.Util;

namespace StorageLibrary
{
    internal class Queue
    {
        public static async Task<List<string>> ListQueuesAsync(string account, string key)
        {
            QueueServiceClient client = new QueueServiceClient(Client.GetConnectionString(account, key));
            List<string> results = new List<string>();
            await foreach (var q in client.GetQueuesAsync())
                results.Add(q.Name);

            return results;
        }

        public static async Task<List<PeekedMessageWrapper>> GetAllMessagesAsync(string account, string key, string queueName)
        {
            QueueClient queueClient = new QueueClient(Client.GetConnectionString(account, key), queueName);
            PeekedMessage[] msgs = await queueClient.PeekMessagesAsync(queueClient.MaxPeekableMessages);

            List<PeekedMessageWrapper> results = new List<PeekedMessageWrapper>();

            foreach (PeekedMessage m in msgs)
                results.Add(new PeekedMessageWrapper { Id = m.MessageId, Message = m.MessageText });

            return results;
        }

        public static async Task DequeueMessage(string account, string key, string queueName)
        {
            QueueClient queueClient = new QueueClient(Client.GetConnectionString(account, key), queueName);

            QueueMessage msg = await queueClient.ReceiveMessageAsync();
            await queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
        }

        public static async Task CreateAsync(string account, string key, string queueName)
        {
            QueueServiceClient client = new QueueServiceClient(Client.GetConnectionString(account, key));
            await client.CreateQueueAsync(queueName);
        }

        public static async Task DeleteAsync(string account, string key, string queueName)
        {
            QueueClient queueClient = new QueueClient(Client.GetConnectionString(account, key), queueName);
            await queueClient.DeleteAsync();
        }

        public static async Task CreateMessageAsync(string account, string key, string queueName, string message)
        {
            QueueClient queueClient = new QueueClient(Client.GetConnectionString(account, key), queueName);
            await queueClient.SendMessageAsync(message);
        }
    }
}
