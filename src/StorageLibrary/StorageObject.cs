
using Azure;

namespace StorageLibrary
{
    internal class StorageObject
    {
        public string Account { get; private set; }
        public string Key { get; private set; }
        public string Endpoint { get; private set; }
        public string ConnectionString { get; private set; }

        const string CONNSTRING_TEMPLATE = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};EndpointSuffix={2}";
        public StorageObject(string account, string key, string endpoint = "core.windows.net")
        {
            if (key.Contains("SharedAccessSignature="))
            {
                ConnectionString = key;
            }
            else
            {
                Account = account;
                Key = key;
                Endpoint = endpoint;

                ConnectionString = string.Format(CONNSTRING_TEMPLATE, Account, Key, Endpoint);
            }
        }
    }
}