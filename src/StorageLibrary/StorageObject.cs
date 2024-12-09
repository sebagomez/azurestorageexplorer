
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
		: this(config.Account, config.Key, config.Endpoint, config.ConnectionString, config.IsAzurite){ }

		public StorageObject(string account, string key, string endpoint, string connectionString, bool azurite)
		{
			IsAzurite = azurite;
			if (!string.IsNullOrEmpty(connectionString))
			{
				ConnectionString = connectionString;
			}
			else if (key.Contains("SharedAccessSignature="))
			{
				ConnectionString = key;
			}
			else
			{
				Account = account;
				Key = key;
				if (!string.IsNullOrEmpty(endpoint))
					Endpoint = endpoint;

				ConnectionString = string.Format(CONNSTRING_TEMPLATE, Account, Key, Endpoint);
			}
		}
	}
}