using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;
using StorageLibrary.Common;

namespace wasm.Pages
{
	public partial class Containers : BaseComponent
	{
		public string? NewContainerName { get; set; }

		public bool PublicAccess { get; set; }

		List<CloudBlobContainerWrapper> AzureContainers = new List<CloudBlobContainerWrapper>();

		protected override Task OnInitializedAsync()
		{
			Loading = true;

			// StorageFactory factory = Util.GetStorageFactory(account, key);
			// List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

			
			return base.OnInitializedAsync();
		}
		public void NewContainer()
		{

		}
	}
}