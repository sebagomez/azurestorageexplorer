using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;

namespace wasm.Pages
{
	public partial class Login
	{
		public bool Loading { get; set; }
		public bool ShowError { get; set; }
		public string ErrorMessage { get; set; }
		public string? AzureAccount { get; set; }
		public string? AzureKey { get; set; }
		public string? AzureUrl { get; set; } = "core.windows.net";

		private void NavigateOnEnter(KeyboardEventArgs args)
		{
			if (args.Code == "Enter")
			{
				SignIn();
			}
		}

		private void SignIn()
		{
			ShowError = false;
			try{
				StorageFactory factory = new StorageFactory(AzureAccount, AzureKey, AzureUrl);
				factory.Containers.ListContainersAsync().GetAwaiter().GetResult();
			}
			catch (Exception ex)
			{
				ShowError = true;
				ErrorMessage = ex.Message;
			}
		}
	}
}