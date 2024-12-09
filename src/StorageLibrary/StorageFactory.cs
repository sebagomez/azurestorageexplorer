
using StorageLibrary.Common;
using StorageLibrary.Interfaces;
using StorageLibrary.Mocks;

namespace StorageLibrary
{
	public class StorageFactory
	{
		static StorageFactory Instance;

		public IQueue Queues { get; private set; }
		public IContainer Containers { get; set; }
		public ITable Tables { get; set; }
		public IFile Files { get; set; }

		StorageFactoryConfig m_currentConfig;

		public StorageFactory()
		: this(new StorageFactoryConfig { Mock = true })
		{ }

		public StorageFactory(StorageFactoryConfig config)
		{
			m_currentConfig = config;
			Queues = config.Mock ? new MockQueue() : new AzureQueue(config);
			Containers = config.Mock ? new MockContainer() : new AzureContainer(config);
			Tables = config.Mock ? new MockTable() : new AzureTable(config);
			Files = config.Mock ? new MockFile() : new AzureFile(config);

			Instance = this;
		}

		public static BlobItemWrapper GetBlobItemWrapper(string url, long size = 0)
		{
			return new BlobItemWrapper(url, size, Instance.m_currentConfig.IsAzurite);
		}
	}

	public class StorageFactoryConfig
	{
		public string Account { get; set; }
		public string Key { get; set; }
		public string Endpoint { get; set; } = "core.windows.net";
		public string ConnectionString { get; set; }
		public bool IsAzurite { get; set; }
		public bool Mock { get; set; }
	}
}