using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using StorageLibrary;
using StorageLibrary.Common;
using StorageLibrary.Mocks;

namespace StorageLibTests
{
	[TestClass]
	public class QueueTests : BaseTests
	{
		[ClassInitialize]
		public static void Initialize(TestContext ctx)
		{
			MockUtils.Reintialize();
		}

		[TestMethod]
		public async Task GetQueues()
		{
			List<QueueWrapper> expected = new List<QueueWrapper>()
			{
				new QueueWrapper {Name = "one"},
				new QueueWrapper {Name = "two"},
				new QueueWrapper {Name = "three" }
			};

			StorageFactory factory = new StorageFactory();
			List<QueueWrapper> queues = await factory.Queues.ListQueuesAsync();

			CompareQueues(expected, queues);
		}

		[TestMethod]
		public async Task GetMessages()
		{
			List<string> expected = new List<string> { "fromOne:1", "fromOne:2", "fromOne:3" };
			string queue = "one";

			StorageFactory factory = new StorageFactory();
			List<PeekedMessageWrapper> messages = await factory.Queues.GetAllMessagesAsync(queue);

			foreach (PeekedMessageWrapper msg in messages)
			{
				Assert.IsTrue(expected.Contains(msg.Message), $"An unexpected message arrived: {msg.Message}");
				expected.Remove(msg.Message);
			}

			Assert.IsTrue(expected.Count == 0, $"More messages were expected: {string.Join(",", expected)}");
		}

		[TestMethod]
		public async Task DequeMessages()
		{
			List<string> expected = new List<string> { "fromOne:2", "fromOne:3" };
			string queue = "one";

			StorageFactory factory = new StorageFactory();
			await factory.Queues.DequeueMessage(queue);
			List<PeekedMessageWrapper> messages = await factory.Queues.GetAllMessagesAsync(queue);

			foreach (PeekedMessageWrapper msg in messages)
			{
				Assert.IsTrue(expected.Contains(msg.Message), $"An unexpected message arrived: {msg.Message}");
				expected.Remove(msg.Message);
			}

			Assert.IsTrue(expected.Count == 0, $"More messages were expected: {string.Join(",", expected)}");
		}

		[TestMethod]
		public async Task CreateQueue()
		{
			string queue = "four";
			List<QueueWrapper> expected = new List<QueueWrapper>()
			{
				new QueueWrapper {Name = "one"},
				new QueueWrapper {Name ="two"},
				new QueueWrapper {Name = "three" },
				new QueueWrapper {Name = queue }
			};

			StorageFactory factory = new StorageFactory();
			await factory.Queues.CreateAsync(queue);

			List<QueueWrapper> queues = await factory.Queues.ListQueuesAsync();

			CompareQueues(expected, queues);
		}

		[TestMethod]
		public async Task FailCreateQueue()
		{
			string queue = "two";
			StorageFactory factory = new StorageFactory();
			try
			{
				await factory.Queues.CreateAsync(queue);
			}
			catch (InvalidOperationException ioe)
			{
				Assert.IsTrue(ioe.Message == $"Queue '{queue}' already exists", ioe.Message);
				return;
			}

			Assert.Fail("An InvalidOperationException should have been thrown.");
		}

		[TestMethod]
		public async Task DeleteQueue()
		{
			string queue = "one";
			List<QueueWrapper> expected = new List<QueueWrapper>()
			{
				new QueueWrapper {Name ="two"},
				new QueueWrapper {Name = "three" },
				new QueueWrapper {Name = "four" },
			};

			StorageFactory factory = new StorageFactory();
			await factory.Queues.DeleteAsync(queue);

			List<QueueWrapper> queues = await factory.Queues.ListQueuesAsync();

			CompareQueues(expected, queues);
		}

		[TestMethod]
		public async Task FailDeleteQueue()
		{
			string queue = "five";
			StorageFactory factory = new StorageFactory();
			try
			{
				await factory.Queues.DeleteAsync(queue);
			}
			catch (NullReferenceException nre)
			{
				Assert.IsTrue(nre.Message == $"Queue '{queue}' does not exist", nre.Message);
				return;
			}

			Assert.Fail("An NullReferenceException should have been thrown.");
		}

		[TestMethod]
		public async Task AddMessage()
		{
			string message = "newMessage";
			List<string> expected = new List<string> { "fromTwo:1", "fromTwo:2", message };
			string queue = "two";

			StorageFactory factory = new StorageFactory();
			await factory.Queues.CreateMessageAsync(queue, message);
			List<PeekedMessageWrapper> messages = await factory.Queues.GetAllMessagesAsync(queue);

			foreach (PeekedMessageWrapper msg in messages)
			{
				Assert.IsTrue(expected.Contains(msg.Message), $"An unexpected message arrived: {msg.Message}");
				expected.Remove(msg.Message);
			}

			Assert.IsTrue(expected.Count == 0, $"More messages were expected: {string.Join(",", expected)}");
		}

		private void CompareQueues(List<QueueWrapper> expected, List<QueueWrapper> returned)
		{
			Assert.IsTrue(expected.Count == returned.Count, $"Different amount returned. {string.Join(",", returned)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(returned[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{returned[i].Name}'");
		}
	}
}
