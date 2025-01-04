using System.Runtime.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using StorageLibrary;

namespace web.Utils
{
	public class Credentials
	{
		const string PROVIDER = "wasm_provider";
		const string ACCOUNT = "wasm_account";
		const string KEY = "wasm_key";
		const string ENDPOINT = "wasm_endpoint";
		const string CONNECTION_STRING = "wasm_connectionString";
		const string AWS_KEY = "wasm_aws_key";
		const string AWS_SECRET = "wasm_aws_secret";
		const string AWS_REGION = "wasm_aws_region";
		const string GCP_CREDENTIALS_FILE = "wasm_gcp_credfile";

		[DataMember]
		public string? Provider { get; set; } = "Azure";

		[DataMember]
		public string? Account { get; set; }
		[DataMember]
		public string? Key { get; set; }
		[DataMember]
		public string? Endpoint { get; set; }
		[DataMember]
		public string? ConnectionString { get; set; }
		[DataMember]
		public string? AwsKey { get; set; }
		[DataMember]
		public string? AwsSecret { get; set; }
		[DataMember]
		public string? AwsRegion { get; set; }
		[DataMember]
		public string? GcpCredFile { get; set; }

		public Credentials()
		{ }

		public async Task SaveAsync(ProtectedBrowserStorage sessionStorage)
		{
			if (string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(ConnectionString))
				Account = GetAccountName();

			if (string.IsNullOrEmpty(Account) && Provider != "Azure")
				Account = Provider;

			await sessionStorage.SetAsync(PROVIDER, Provider!);
			await sessionStorage.SetAsync(ACCOUNT, Account!);
			await sessionStorage.SetAsync(KEY, Key!);
			await sessionStorage.SetAsync(ENDPOINT, Endpoint!);
			await sessionStorage.SetAsync(CONNECTION_STRING, ConnectionString!);
			await sessionStorage.SetAsync(AWS_KEY, AwsKey!);
			await sessionStorage.SetAsync(AWS_SECRET, AwsSecret!);
			await sessionStorage.SetAsync(AWS_REGION, AwsRegion!);
			await sessionStorage.SetAsync(GCP_CREDENTIALS_FILE, GcpCredFile!);
		}

		private string GetAccountName()
		{
			if (Provider != "Azure")
				return Provider!;

			var connStringArray = ConnectionString!.Split(';', StringSplitOptions.RemoveEmptyEntries);
			var dictionary = new Dictionary<string, string>();
			foreach (var item in connStringArray)
			{
				var itemKeyValue = item.Split('=');
				dictionary.Add(itemKeyValue[0], itemKeyValue[1]);
			}

			List<string> endpoints = new List<string> { "BlobEndpoint", "QueueEndpoint", "TableEndpoint", "FileEndpoint" };
			string account = string.Empty;
			foreach (string endpoint in endpoints)
			{
				if (string.IsNullOrEmpty(account) && dictionary.ContainsKey(endpoint))
				{
					Uri uri = new Uri(dictionary[endpoint]);
					var hosts = uri.Host.Split('.');
					if (hosts.Length > 0)
					{
						account = hosts[0];
						break;
					}
				}
			}

			return account;
		}

		public static async Task<Credentials> LoadCredentialsAsync(ProtectedBrowserStorage sessionStorage)
		{
			return new Credentials
			{
				Provider = (await sessionStorage.GetAsync<string>(PROVIDER)).Value,
				Account = (await sessionStorage.GetAsync<string>(ACCOUNT)).Value,
				Key = (await sessionStorage.GetAsync<string>(KEY)).Value,
				Endpoint = (await sessionStorage.GetAsync<string>(ENDPOINT)).Value,
				ConnectionString = (await sessionStorage.GetAsync<string>(CONNECTION_STRING)).Value,
				AwsKey = (await sessionStorage.GetAsync<string>(AWS_KEY)).Value,
				AwsSecret = (await sessionStorage.GetAsync<string>(AWS_SECRET)).Value,
				AwsRegion = (await sessionStorage.GetAsync<string>(AWS_REGION)).Value,
				GcpCredFile = (await sessionStorage.GetAsync<string>(GCP_CREDENTIALS_FILE)).Value
			};
		}

		public static async Task ClearAsync(ProtectedBrowserStorage sessionStorage)
		{
			await sessionStorage.DeleteAsync(PROVIDER);
			await sessionStorage.DeleteAsync(ACCOUNT);
			await sessionStorage.DeleteAsync(KEY);
			await sessionStorage.DeleteAsync(ENDPOINT);
			await sessionStorage.DeleteAsync(CONNECTION_STRING);
			await sessionStorage.DeleteAsync(AWS_KEY);
			await sessionStorage.DeleteAsync(AWS_SECRET);
			await sessionStorage.DeleteAsync(AWS_REGION);
			await sessionStorage.DeleteAsync(GCP_CREDENTIALS_FILE);
		}

		public async Task<bool> IsAuthenticated(ProtectedBrowserStorage sessionStorage)
		{
			StorageFactory factory = Util.GetStorageFactory(this);
			var containers = await factory.Containers.ListContainersAsync(); //This authentication method is dangerous with SaS and ConnectionStrings

			await SaveAsync(sessionStorage!);

			return true;
		}
	}
}