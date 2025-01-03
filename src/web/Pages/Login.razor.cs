using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using web.Utils;

namespace web.Pages
{
	public partial class Login
	{
		public bool Loading { get; set; }
		public bool ShowError { get; set; }
		public string? ErrorMessage { get; set; }
		public string? AzureAccount { get; set; } = "";
		public string? AccountErrorMessage { get; set; } = "";
		public string? AccountClass { get; set; } = "";
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

		private void ValidateAccount(FocusEventArgs args)
		{
			string validpattern = "^[a-z0-9]{3,24}$";
			if (!Regex.IsMatch(AzureAccount!, validpattern))
			{
				ShowError = true;
				AccountErrorMessage = $"Account is not a valid name";
				AccountClass = "error";
			}
			else
			{
				ShowError = false;
				AccountErrorMessage = string.Empty;
				AccountClass = "";
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
				else //check if it's AWS or GCP
				{
					string? cloudProvider = Environment.GetEnvironmentVariable("CLOUD_PROVIDER");
					if (!string.IsNullOrEmpty(cloudProvider))
					{
						if (cloudProvider.Equals("AWS", StringComparison.OrdinalIgnoreCase))
						{
							string? awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
							string? awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
							string? awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
							if (!string.IsNullOrEmpty(awsAccessKey) && !string.IsNullOrEmpty(awsSecretKey) && !string.IsNullOrEmpty(awsRegion))
							{
								Credentials awsCredentials = new Credentials
								{
									Provider = "AWS",
									AwsKey = awsAccessKey,
									AwsSecret = awsSecretKey,
									AwsRegion = awsRegion
								};
								if (await awsCredentials.IsAuthenticated(SessionStorage!))
									NavManager!.NavigateTo("home");
							}
						}
						else if (cloudProvider.Equals("GCP", StringComparison.OrdinalIgnoreCase))
						{
							string? gcpCredentials = Environment.GetEnvironmentVariable("GCP_CREDENTIALS_FILE");
							if (!string.IsNullOrEmpty(gcpCredentials))
							{
								Credentials gcpCred = new Credentials { Provider = "GCP", GcpCredFile = gcpCredentials };
								if (await gcpCred.IsAuthenticated(SessionStorage!))
									NavManager!.NavigateTo("home");
							}
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
				AccountErrorMessage = "Invalid account or account key";
				ErrorMessage = ex.Message;
			}
		}
	}
}