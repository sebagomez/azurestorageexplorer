using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;
using web.Utils;

namespace web.Pages
{
	public partial class BaseComponent
	{
		public string? Account { get; set; }
		public string? ErrorMessage { get; set; }
		public bool HasError { get; set; } = false;
		public bool Loading { get; set; } = false;
		public StorageFactory? AzureStorage { get; set; }

		public Credentials? AzureCredentials { get; set; }

		[Inject]
		NavigationManager? NavManager { get; set; }

		[Inject]
		ProtectedSessionStorage? SessionStorage {get; set;}

		[Parameter]
		public EventCallback RefreshNeeded { get; set; }

		[Parameter]
		public string? Selected { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (AzureCredentials is null)
				AzureCredentials = await Credentials.LoadCredentialsAsync(SessionStorage!);

			if (AzureCredentials is null || string.IsNullOrEmpty(AzureCredentials.Account))
			{
				NavManager!.NavigateTo("login");
				return;
			}

			if (AzureStorage is null)
				AzureStorage = Util.GetStorageFactory(AzureCredentials!);

			Account = AzureCredentials!.Account;
		}

		protected void RefreshParent()
		{
			StateHasChanged();
		}
	}
}