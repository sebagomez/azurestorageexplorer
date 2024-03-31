using System.Runtime.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace web.Utils
{
	public class Credentials
	{
		const string ACCOUNT = "wasm_account";
		const string KEY = "wasm_key";
		const string ENDPOINT = "wasm_endpoint";
		const string CONNECTION_STRING = "wasm_connectionString";

		[DataMember]
		public string? Account { get; set; }
		[DataMember]
		public string? Key { get; set; }
		[DataMember]
		public string? Endpoint { get; set; }
		[DataMember]
		public string? ConnectionString { get; set; }

		public Credentials()
		{ }

		public Credentials(string azure_account, string azure_key, string azure_endpoint, string azure_connectionString)
		{
			Account = azure_account;
			Key = azure_key;
			Endpoint = azure_endpoint;
			ConnectionString = azure_connectionString;
		}

		public async Task SaveAsync(ProtectedBrowserStorage sessionStorage)
		{
			if (string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(ConnectionString))
				Account = GetAccountName();

			await sessionStorage.SetAsync(ACCOUNT, Account!);
			await sessionStorage.SetAsync(KEY, Key!);
			await sessionStorage.SetAsync(ENDPOINT, Endpoint!);
			await sessionStorage.SetAsync(CONNECTION_STRING, ConnectionString!);
		}

		private string GetAccountName()
		{
			var connStringArray = ConnectionString!.Split(';');
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
				Account = (await sessionStorage.GetAsync<string>(ACCOUNT)).Value,
				Key = (await sessionStorage.GetAsync<string>(KEY)).Value,
				Endpoint = (await sessionStorage.GetAsync<string>(ENDPOINT)).Value,
				ConnectionString = (await sessionStorage.GetAsync<string>(CONNECTION_STRING)).Value
			};
		}

		public static async Task ClearAsync(ProtectedBrowserStorage sessionStorage)
		{
			await sessionStorage.DeleteAsync(ACCOUNT);
			await sessionStorage.DeleteAsync(KEY);
			await sessionStorage.DeleteAsync(ENDPOINT);
			await sessionStorage.DeleteAsync(CONNECTION_STRING);
		}
	}
}