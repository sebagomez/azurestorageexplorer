
using StorageLibrary.Interfaces;
using StorageLibrary.Mocks;

namespace StorageLibrary
{
	public class StorageFactory
	{
		public IQueue Queues { get; private set; }
		public IContainer Containers { get; set; }

		public ITable Tables { get; set; }
		public IFile Files { get; set; }

		public StorageFactory()
		: this(string.Empty, string.Empty, string.Empty, true)
		{ }

		public StorageFactory(string account, string key, string endpoint = "core.windows.net", bool mock = false)
		{
			if (mock)
			{
				Queues = new MockQueue();
				Containers = new MockContainer();
				Tables = new MockTable();
				Files = new MockFile();
			}
			else
			{
				Queues = new AzureQueue(account, key, endpoint);
				Containers = new AzureContainer(account, key, endpoint);
				Tables = new AzureTable(account, key, endpoint);
				Files = new AzureFile(account,key, endpoint);
			}
		}
	}
}