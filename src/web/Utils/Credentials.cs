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

		[DataMember]
		public string? Account { get; set; }
		[DataMember]
		public string? Key { get; set; }
		[DataMember]
		public string? Endpoint { get; set; }

		public Credentials()
		{}

		public Credentials(string azure_account, string azure_key, string azure_endpoint)
		{
			Account = azure_account;
			Key = azure_key;
			Endpoint = azure_endpoint;
		}

		public async Task SaveAsync(ProtectedBrowserStorage sessionStorage)
		{
			await sessionStorage.SetAsync(ACCOUNT, Account!);
			await sessionStorage.SetAsync(KEY, Key!);
			await sessionStorage.SetAsync(ENDPOINT, Endpoint!);
		}

		public static async Task<Credentials> LoadCredentialsAsync(ProtectedBrowserStorage sessionStorage)
		{
			return new Credentials
			{
				Account = (await sessionStorage.GetAsync<string>(ACCOUNT)).Value,
				Key = (await sessionStorage.GetAsync<string>(KEY)).Value,
				Endpoint = (await sessionStorage.GetAsync<string>(ENDPOINT)).Value
			};
		}

		public static async Task ClearAsync(ProtectedBrowserStorage sessionStorage)
		{
			await sessionStorage.DeleteAsync(ACCOUNT);
			await sessionStorage.DeleteAsync(KEY);
			await sessionStorage.DeleteAsync(ENDPOINT);
		}
	}
}