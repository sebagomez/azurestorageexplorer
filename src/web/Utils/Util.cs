using System.Text.RegularExpressions;

using StorageLibrary;

namespace web.Utils
{
	public class Util
	{
		const long DEFAULT_MAX_MEGS = 10;

		/// <summary>
		/// Cap on uploads routed through the server, in bytes. This only bounds what a
		/// Blazor circuit will buffer - direct browser-to-storage uploads are unaffected.
		/// Read once at startup from <see cref="MAX_SERVER_UPLOAD_MEGS"/>; anything that
		/// is not a positive integer falls back to <see cref="DEFAULT_MAX_MEGS"/>.
		/// </summary>
		public static readonly long MAX_UPLOAD_SIZE = 1024 * 1024 * GetMaxUploadMegs();

		public const string MAX_SERVER_UPLOAD_MEGS = "MAX_SERVER_UPLOAD_MEGS";
		public const string AZURE_STORAGE_CONNECTIONSTRING = "AZURE_STORAGE_CONNECTIONSTRING";
		public const string AZURE_STORAGE_ACCOUNT = "AZURE_STORAGE_ACCOUNT";
		public const string AZURE_STORAGE_KEY = "AZURE_STORAGE_KEY";
		public const string AZURE_STORAGE_ENDPOINT = "AZURE_STORAGE_ENDPOINT";
		public const string MOCK = "MOCK";
		public const string AZURITE = "AZURITE";

		public static string Provider { get; set; } = "Azure";
		public static bool Azurite { get; set; } = false;

		static long GetMaxUploadMegs()
		{
			string? configured = Environment.GetEnvironmentVariable(MAX_SERVER_UPLOAD_MEGS);

			return long.TryParse(configured, out long megs) && megs > 0 ? megs : DEFAULT_MAX_MEGS;
		}

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

		/// <summary>
		/// Removes shared access signatures from text that is about to be shown to the
		/// user or logged. A SAS in a URL *is* the credential, and storage exceptions
		/// often embed the request URI.
		/// </summary>
		public static string RedactSignatures(string text)
		{
			if (string.IsNullOrEmpty(text))
				return text;

			return Regex.Replace(text, @"(?<=[?&]sig=)[^&\s""']+", "REDACTED", RegexOptions.IgnoreCase);
		}
	}
}
