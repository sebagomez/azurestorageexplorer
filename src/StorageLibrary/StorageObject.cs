
namespace StorageLibrary
{
	internal class StorageObject
	{
		public string Account { get; private set; }
		public string Key { get; private set; }
		public string Endpoint { get; private set; }
		public string ConnectionString { get => $"DefaultEndpointsProtocol=https;AccountName={Account};AccountKey={Key};EndpointSuffix={Endpoint}"; }

		public StorageObject(string account, string key, string endpoint = "core.windows.net")
		{
			Account = account;
			Key = key;
			Endpoint = endpoint;
		}
	}
}