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
		public string? AzureAccount { get; set; } = "sebgomez";
		public string? AzureKey { get; set; } = "LOT6kY6ik0pqDqzPXacwZLLG0psx2aCvpDAkdxSYEQyb2emB0XvxuW1VnZbYPhl+M5dziMlEQRpn+AStUoma/g==";
		public string AzureUrl { get; set; } = "core.windows.net";

		[Inject]
		NavigationManager? NavManager { get; set;}

		[Inject]
		ProtectedSessionStorage? SessionStorage {get; set;}
	
		private void NavigateOnEnter(KeyboardEventArgs args)
		{
			if (args.Code == "Enter")
			{
				Task.Run(() => SignIn());
			}
		}

		private async Task SignIn()
		{
			ShowError = false;
			try
			{
				if (string.IsNullOrWhiteSpace(AzureAccount) || string.IsNullOrWhiteSpace(AzureKey) || string.IsNullOrWhiteSpace(AzureUrl))
					return;

				Credentials cred = new Credentials
				{
					Account = AzureAccount,
					Key = AzureKey,
					Endpoint = AzureUrl
				};

				StorageFactory factory = Util.GetStorageFactory(cred);
				var containers = await factory.Containers.ListContainersAsync();

				await cred.SaveAsync(SessionStorage!);

				NavManager!.NavigateTo("/");
			}
			catch (Exception ex)
			{
				ShowError = true;
				ErrorMessage = ex.Message;
			}
		}
	}
}