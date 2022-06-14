using StorageLibrary;
using System;

namespace AzureWebStorageExplorer
{
	public class Util
	{
		const long MAX_MEGS = 100;
		public const long MAX_UPLOAD_SIZE = 1024 * 1024 * MAX_MEGS;
		public static StorageFactory GetStorageFactory(string account, string key, string endpoint = "core.windows.net")
		{
			string? mock = Environment.GetEnvironmentVariable("MOCK");
			if (!(mock is null) && mock.ToLower() == bool.TrueString.ToLower())
				return new StorageFactory();

			return new StorageFactory(account, key, endpoint);
		}

	}
}
