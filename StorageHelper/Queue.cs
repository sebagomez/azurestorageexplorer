using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;

namespace StorageHelper
{
    public class Queue
    {
        public static IEnumerable<string> GetAll(string account, string key)
        {
            CloudQueueClient queueClient = Client.GetQueueClient(account, key);

            foreach (CloudQueue queue in queueClient.ListQueues())
                yield return queue.Name;
        }

        public static IEnumerable<CloudQueueMessage> GetAllMessages(string account, string key, string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
                yield break;

            CloudQueueClient queueClient = Client.GetQueueClient(account, key);
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.FetchAttributes();

            if (queue.ApproximateMessageCount.HasValue && queue.ApproximateMessageCount.Value > 0)
            {
                foreach (CloudQueueMessage message in queue.PeekMessages(queue.ApproximateMessageCount.Value))
                    yield return message;
            }
            else
                yield break;
        }

        public static void DeleteMessage(string account, string key, string queueName, string messageId)
        {
            CloudQueueClient queueClient = Client.GetQueueClient(account, key);
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.FetchAttributes();

            if (queue.ApproximateMessageCount.HasValue && queue.ApproximateMessageCount.Value > 0)
            {
                bool found = false;
                foreach (CloudQueueMessage message in queue.GetMessages(queue.ApproximateMessageCount.Value, new TimeSpan(0, 0, 0, 0, 250)))
                {
                    if (message.Id == messageId)
                    {
                        found = true;
                        queue.DeleteMessage(message);
                        break;
                    }
                }

                if (!found)
                    throw new Exception("No message found");
            }
        }

        public static void Create(string account, string key, string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
                return;

            CloudQueueClient queueClient = Client.GetQueueClient(account, key);
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
        }

        public static void Delete(string account, string key, string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
                return;

            CloudQueueClient queueClient = Client.GetQueueClient(account, key);
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.Delete();
        }

        public static void CreateMessage(string account, string key, string queueName, string message)
        {
            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
                return;

            CloudQueueClient queueClient = Client.GetQueueClient(account, key);
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            queue.AddMessage(new CloudQueueMessage(message.Trim()));
        }
    }
}
