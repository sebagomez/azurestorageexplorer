using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Mocks
{
	internal class MockQueue : IQueue
	{
		static Dictionary<string, List<string>> s_queues = new Dictionary<string, List<string>>()
		{
			{ "one", new List<string> {"fromOne:1", "fromOne:2", "fromOne:3"}},
			{ "two", new List<string> {"fromTwo:1", "fromTwo:2"}},
			{ "three", new List<string> {"fromThree:1"}}
		};

		public async Task<List<string>> ListQueuesAsync()
		{
			return await Task.Run(() => {
				List<string> retList = new List<string>();
				foreach(string key in s_queues.Keys)
					retList.Add(key);
				
				return retList;
			});
		}

		public async Task<List<PeekedMessageWrapper>> GetAllMessagesAsync(string queueName)
		{
			return await Task.Run(() => {
				if (!s_queues.ContainsKey(queueName))
					throw new NullReferenceException($"Queue '{queueName}' does not exist");

				List<PeekedMessageWrapper> results = new List<PeekedMessageWrapper>();
				foreach(string val in s_queues[queueName])
					results.Add(new PeekedMessageWrapper { Id = val, Message = val });

				return results;
			});
		}

		public async Task DequeueMessage(string queueName)
		{
			await Task.Run(() => {
				if (!s_queues.ContainsKey(queueName))
					throw new NullReferenceException($"Queue '{queueName}' does not exist");
				
				int count = s_queues[queueName].Count;
				if (count == 0)
					throw new NullReferenceException($"Queue '{queueName}' is empty");

				s_queues[queueName].RemoveAt(0);
			});
		}

		public async Task CreateAsync(string queueName)
		{
			await Task.Run(() => {
				if (s_queues.ContainsKey(queueName))
					throw new InvalidOperationException($"Queue '{queueName}' already exists");
				
				s_queues.Add(queueName, new List<string>());
			});
		}

		public async Task DeleteAsync(string queueName)
		{
			await Task.Run(() => {
				if (!s_queues.ContainsKey(queueName))
					throw new NullReferenceException($"Queue '{queueName}' does not exist");
				
				s_queues.Remove(queueName);
			});
		}

		public async Task CreateMessageAsync(string queueName, string message)
		{
			await Task.Run(() => {
				if (!s_queues.ContainsKey(queueName))
					throw new NullReferenceException($"Queue '{queueName}' does not exist");
				
				s_queues[queueName].Add(message);
			});
		}
	}
}
