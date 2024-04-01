using StorageLibrary;

namespace web.Utils
{
	public class Util
	{
		const long MAX_MEGS = 10;
		public const long MAX_UPLOAD_SIZE = 1024 * 1024 * MAX_MEGS;
		public const string AZURE_STORAGE_CONNECTIONSTRING = "AZURE_STORAGE_CONNECTIONSTRING";
		public const string AZURE_STORAGE_ACCOUNT = "AZURE_STORAGE_ACCOUNT";
		public const string AZURE_STORAGE_KEY = "AZURE_STORAGE_KEY";
		public const string AZURE_STORAGE_ENDPOINT = "AZURE_STORAGE_ENDPOINT";

		public static StorageFactory GetStorageFactory(Credentials cred)
		{
			string? mock = Environment.GetEnvironmentVariable("MOCK");
			if (mock is not null && mock.ToLower() == bool.TrueString.ToLower())
				return new StorageFactory();

			return new StorageFactory(cred.Account, cred.Key, cred.Endpoint, cred.ConnectionString);
		}
	}
}
