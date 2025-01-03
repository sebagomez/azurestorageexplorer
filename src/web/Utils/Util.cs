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
		public const string MOCK = "MOCK";
		public const string AZURITE = "AZURITE";

		public static string Provider { get; set; } = "Azure";
		public static bool Azurite { get; set; } = false;

		public static StorageFactory GetStorageFactory(Credentials cred)
		{
			Provider = cred.Provider!;

			string? mock = Environment.GetEnvironmentVariable(MOCK);
			bool mockEnabled = mock is not null && mock.ToLower() == bool.TrueString.ToLower();

			string? azurite = Environment.GetEnvironmentVariable(AZURITE);
			Azurite = azurite is not null && azurite.ToLower() == bool.TrueString.ToLower();

			return new StorageFactory(new StorageFactoryConfig 
			{ 
				Provider = Enum.Parse<CloudProvider>(cred.Provider!),
				AzureAccount = cred.Account, 
				AzureKey = cred.Key, 
				AzureEndpoint = cred.Endpoint, 
				AzureConnectionString = cred.ConnectionString, 
				AwsKey = cred.AwsKey,
				AwsSecret = cred.AwsSecret,
				AwsRegion = cred.AwsRegion,
				GcpCredentialsFile = cred.GcpCredFile,
				IsAzurite = Azurite, 
				Mock = mockEnabled 
			});
		}
	}
}
