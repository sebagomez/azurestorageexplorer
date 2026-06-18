
using System;
using Azure;

namespace StorageLibrary
{
	internal class StorageObject
	{
		public string Account { get; private set; }
		public string Key { get; private set; }
		public string Endpoint { get; private set; } = "core.windows.net";
		public string ConnectionString { get; private set; }

		public bool IsAzurite { get; private set; }

		const string CONNSTRING_TEMPLATE = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};EndpointSuffix={2}";

		public StorageObject(StorageFactoryConfig config)
		{
			if (config.Provider != CloudProvider.Azure)
				return;

			if (!string.IsNullOrEmpty(config.AzureConnectionString))
			{
				ConnectionString = config.AzureConnectionString;
				IsAzurite = config.IsAzurite || IsAzuriteConnectionString(config.AzureConnectionString);
			}
			else if (config.AzureKey.Contains("SharedAccessSignature="))
			{
				ConnectionString = config.AzureKey;
			}
			else
			{
				IsAzurite = config.IsAzurite;
				Account = config.AzureAccount;
				Key = config.AzureKey;
				if (!string.IsNullOrEmpty(config.AzureEndpoint))
					Endpoint = config.AzureEndpoint;

				ConnectionString = string.Format(CONNSTRING_TEMPLATE, Account, Key, Endpoint);
			}
		}

		static bool IsAzuriteConnectionString(string connectionString) =>
			connectionString.Contains("devstoreaccount1", StringComparison.OrdinalIgnoreCase) ||
			connectionString.Contains("UseDevelopmentStorage=true", StringComparison.OrdinalIgnoreCase);
	}
}