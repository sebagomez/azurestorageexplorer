
using System;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;
using StorageLibrary.Mocks;
using StorageLibrary.Azure;
using StorageLibrary.AWS;
using StorageLibrary.Google;

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
			if (config.Mock)
			{
				Queues = new MockQueue();
				Containers = new MockContainer();
				Tables = new MockTable();
				Files = new MockFile();
			}
			else
			{
				switch (config.Provider)
				{
					case CloudProvider.Azure:
						Queues = new AzureQueue(config);
						Containers = new AzureContainer(config);
						Tables = new AzureTable(config);
						Files = new AzureFile(config);
						break;
					case CloudProvider.AWS:
						Containers = new AWSBucket(config);
						break;
					case CloudProvider.GCP:
						Containers = new GCPBucket(config);
						break;
					default:
						throw new ApplicationException($"Invalid provider: {config.Provider}");
				}
			}

			Instance = this;
		}

		public static BlobItemWrapper GetBlobItemWrapper(string url, long size = 0)
		{
			return new BlobItemWrapper(url, size, Instance.m_currentConfig.Provider, Instance.m_currentConfig.IsAzurite);
		}
	}

	public class StorageFactoryConfig
	{
		public string AzureAccount { get; set; }
		public string AzureKey { get; set; }
		public string AzureEndpoint { get; set; } = "core.windows.net";
		public string AzureConnectionString { get; set; }
		public bool IsAzurite { get; set; }
		public bool Mock { get; set; }
		public string AwsKey { get; set; }
		public string AwsSecret { get; set; }
		public string AwsRegion { get; set; }
		public string GcpCredentialsFile { get; set; }
		public CloudProvider Provider { get; set; } = CloudProvider.Azure;
	}

	public enum CloudProvider
	{
		Azure,
		AWS,
		GCP
	}
}