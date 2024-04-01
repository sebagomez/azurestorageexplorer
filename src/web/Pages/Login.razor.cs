using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;
using web.Utils;

namespace web.Pages
{
	public partial class Login
	{
		public bool Loading { get; set; }
		public bool ShowError { get; set; }
		public string? ErrorMessage { get; set; }
		public string? AzureAccount { get; set; } = "";
		public string? AzureKey { get; set; } = "";
		public string AzureUrl { get; set; } = "core.windows.net";
		public string? ConnectionString { get; set; }

		[Inject]
		NavigationManager? NavManager { get; set; }

		[Inject]
		ProtectedSessionStorage? SessionStorage { get; set; }

		private void NavigateOnEnter(KeyboardEventArgs args)
		{
			if (args.Code == "Enter")
			{
				Task.Run(() => SignIn());
			}
		}

		protected override async Task OnInitializedAsync()
		{
			string? connString = Environment.GetEnvironmentVariable(Util.AZURE_STORAGE_CONNECTIONSTRING);
			if (!string.IsNullOrEmpty(connString))
			{
				Credentials cred = new Credentials { ConnectionString = connString };
				if (await cred.IsAuthenticated(SessionStorage!))
					NavManager!.NavigateTo("home");
			}
			else
			{
				string? account = Environment.GetEnvironmentVariable(Util.AZURE_STORAGE_ACCOUNT);
				if (!string.IsNullOrEmpty(account))
				{
					string? key = Environment.GetEnvironmentVariable(Util.AZURE_STORAGE_KEY);
					if (!string.IsNullOrEmpty(key))
					{
						string? endpoint = Environment.GetEnvironmentVariable(Util.AZURE_STORAGE_ENDPOINT);
						if (!string.IsNullOrEmpty(endpoint))
						{
							Credentials cred = new Credentials { Account = account, Key = key, Endpoint = endpoint };
							if (await cred.IsAuthenticated(SessionStorage!))
								NavManager!.NavigateTo("home");
						}
					}
				}
			}
		}

		private async Task SignIn()
		{
			ShowError = false;
			try
			{
				if (string.IsNullOrWhiteSpace(ConnectionString) &&
					(string.IsNullOrWhiteSpace(AzureAccount) ||
					string.IsNullOrWhiteSpace(AzureKey) ||
					string.IsNullOrWhiteSpace(AzureUrl)))
					return;

				Credentials cred = new Credentials
				{
					Account = AzureAccount,
					Key = AzureKey,
					Endpoint = AzureUrl,
					ConnectionString = ConnectionString
				};

				if (await cred.IsAuthenticated(SessionStorage!))
					NavManager!.NavigateTo("home");
			}
			catch (Exception ex)
			{
				ShowError = true;
				ErrorMessage = ex.Message;
			}
		}
	}
}