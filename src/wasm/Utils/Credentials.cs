using Microsoft.AspNetCore.Components;

namespace wasm.Utils
{
	public class Credentials
	{
		const string ACCOUNT = "wasm_account";
		const string KEY = "wasm_key";
		const string ENDPOINT = "wasm_endpoint";

		public string Account { get; set; }
		public string Key { get; set; }
		public string Endpoint { get; set; }

		[Inject]
		static Blazored.SessionStorage.ISessionStorageService SessionStorage { get; set;}

		public Credentials()
		{}

		public Credentials(string azure_account, string azure_key, string azure_endpoint)
		{
			Account = azure_account;
			Key = azure_key;
			Endpoint = azure_endpoint;
		}

		public async Task Save()
		{
			await SessionStorage.SetItemAsync(ACCOUNT, Account);
			await SessionStorage.SetItemAsync(KEY, Key);
			await SessionStorage.SetItemAsync(ENDPOINT, Endpoint);
		}

		public static async Task<Credentials> LoadCredentials()
		{
			return new Credentials
			{
				Account = await SessionStorage.GetItemAsStringAsync(ACCOUNT),
				Key = await SessionStorage.GetItemAsStringAsync(KEY),
				Endpoint = await SessionStorage.GetItemAsStringAsync(ENDPOINT)
			};
		}
	}
}