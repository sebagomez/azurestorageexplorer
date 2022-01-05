
using StorageLibrary.Interfaces;
using StorageLibrary.Mocks;

namespace StorageLibrary
{
	public class StorageFactory
	{
		public IQueue Queues { get; private set; }
		public IContainer Containers { get; set; }

		public StorageFactory()
		: this(string.Empty, string.Empty, string.Empty, true)
		{ }

		public StorageFactory(string account, string key, string endpoint = "core.windows.net", bool mock = false)
		{
			if (mock)
			{
				Queues = new MockQueue();
				Containers = new MockContainer();
			}
			else
			{
				Queues = new AzureQueue(account, key, endpoint);
				Containers = new AzureContainer(account, key, endpoint);
			}
		}
	}
}