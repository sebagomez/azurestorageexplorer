using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageHelper;
using Microsoft.WindowsAzure.StorageClient;
using System.ComponentModel;

namespace StorageManager.Helpers
{
    public class MessageHelper
    {
		public string Id { get; set; }
        public string Content { get; set; }

		public static List<MessageHelper> GetAll(string queueName, string account, string key)
        {
			List<MessageHelper> list = new List<MessageHelper>();
			foreach (CloudQueueMessage message in Queue.GetAllMessages(account, key, queueName))
			{
				MessageHelper msgHelper = new MessageHelper()
				{
					Id = message.Id,
					Content = message.AsString
				};
				list.Add(msgHelper);
			}

			return list;
        }
    
        public static void Delete(string account, string key, string queueName, string messageId)
        {
			StorageHelper.Queue.DeleteMessage(account, key, queueName, messageId);
        }
    }
}
