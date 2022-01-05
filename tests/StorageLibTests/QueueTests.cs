using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;
using StorageLibrary.Common;

namespace StorageLibTests
{
    [TestClass]
    public class QueueTests : BaseTests
    {
        [TestMethod]
        public async Task GetQueues()
        {
            List<string> expected = new List<string>() { "one", "two", "three" };

            StorageFactory factory = new StorageFactory();
            List<string> queues = await factory.Queue.ListQueuesAsync();

            CollectionAssert.AreEqual(expected, queues);
        }

        [TestMethod]
        public async Task GetMessages()
        {
            List<string> expected = new List<string> { "fromOne:1", "fromOne:2", "fromOne:3" };
            string queue = "one";

            StorageFactory factory = new StorageFactory();
            List<PeekedMessageWrapper> messages = await factory.Queue.GetAllMessagesAsync(queue);

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
            await factory.Queue.DequeueMessage(queue);
            List<PeekedMessageWrapper> messages = await factory.Queue.GetAllMessagesAsync(queue);

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
            List<string> expected = new List<string>() { "one", "two", "three", queue };

            StorageFactory factory = new StorageFactory();
            await factory.Queue.CreateAsync(queue);

            List<string> queues = await factory.Queue.ListQueuesAsync();

            CollectionAssert.AreEqual(expected, queues);
        }

        [TestMethod]
        public async Task FailCreateQueue()
        {
            string queue = "two";
            StorageFactory factory = new StorageFactory();
            try
            {
                await factory.Queue.CreateAsync(queue);
            }
            catch (InvalidOperationException ioe)
            {
                Assert.IsTrue(ioe.Message == "Queue named 'two' already exists", ioe.Message);
                return;
            }

            Assert.Fail("An InvalidOperationException should have been thrown.");
        }

        [TestMethod]
        public async Task DeleteQueue()
        {
            string queue = "one";
            List<string> expected = new List<string>() { "two", "three" };

            StorageFactory factory = new StorageFactory();
            await factory.Queue.DeleteAsync(queue);

            List<string> queues = await factory.Queue.ListQueuesAsync();

            CollectionAssert.AreEqual(expected, queues);
        }

		 [TestMethod]
        public async Task FailDeleteQueue()
        {
            string queue = "four";
            StorageFactory factory = new StorageFactory();
			try
			{
            await factory.Queue.DeleteAsync(queue);
			}
			catch (NullReferenceException nre)
			{
                Assert.IsTrue(nre.Message == "Queue named 'four' does not exist", nre.Message);
                return;
			}

            Assert.Fail("An NullReferenceException should have been thrown.");
        }

		[TestMethod]
        public async Task AddMessage()
        {
			string message = "newMessage";
            List<string> expected = new List<string> { "fromOne:1", "fromOne:2", "fromOne:3", message };
            string queue = "one";

            StorageFactory factory = new StorageFactory();
			await factory.Queue.CreateMessageAsync(queue, message);
            List<PeekedMessageWrapper> messages = await factory.Queue.GetAllMessagesAsync(queue);

            foreach (PeekedMessageWrapper msg in messages)
            {
                Assert.IsTrue(expected.Contains(msg.Message), $"An unexpected message arrived: {msg.Message}");
                expected.Remove(msg.Message);
            }

            Assert.IsTrue(expected.Count == 0, $"More messages were expected: {string.Join(",", expected)}");
        }
    }
}
