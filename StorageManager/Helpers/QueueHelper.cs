using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageHelper;
using Microsoft.WindowsAzure.StorageClient;
using System.Data;

namespace StorageManager.Helpers
{
    public class QueueHelper
    {
        public string Name { get; set; }

		public static List<QueueHelper> GetAll(string account, string key)
        {
			List<QueueHelper> list = new List<QueueHelper>();
            foreach (string tableName in Queue.GetAll(account,key))
            {
				QueueHelper table = new QueueHelper() { Name = tableName };
                list.Add(table);
            }

            return list;
        }

		public static void Delete(string account, string key, string queue)
		{
			Queue.Delete(account, key, queue);
		}

    }
}
