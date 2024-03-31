
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
		: this(string.Empty, string.Empty, string.Empty, string.Empty, true)
		{ }

		public StorageFactory(string account, string key, string endpoint, string connectionString, bool mock = false)
		{
			Queues = mock ? new MockQueue() : new AzureQueue(account, key, endpoint, connectionString);
			Containers = mock ? new MockContainer() : new AzureContainer(account, key, endpoint, connectionString);
			Tables = mock ? new MockTable() : new AzureTable(account, key, endpoint, connectionString);
			Files = mock ? new MockFile() : new AzureFile(account, key, endpoint, connectionString);
		}
	}
}