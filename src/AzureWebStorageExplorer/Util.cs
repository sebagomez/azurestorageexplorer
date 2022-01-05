using StorageLibrary;
using System;

namespace AzureWebStorageExplorer
{
    public class Util
	{
		public static StorageFactory GetStorageFactory(string account, string key, string endpoint = "core.windows.net")
		{
			string mock =  Environment.GetEnvironmentVariable("MOCK");
			if (!string.IsNullOrWhiteSpace(mock) && mock == bool.TrueString)
				return new StorageFactory();

			return new StorageFactory(account, key, endpoint);
		}

	}
}
