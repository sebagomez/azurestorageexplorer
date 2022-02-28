using System;

namespace StorageLibrary.Util
{
    public class Client
    {
        public static string GetConnectionString(string account, string key, string endpoint = "core.windows.net")
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(key))
                throw new NullReferenceException("Account and/or Key are empty. The class has not been properly initialized");

            return $"DefaultEndpointsProtocol=https;AccountName={account};AccountKey={key};EndpointSuffix={endpoint}";
        }
    }
}
