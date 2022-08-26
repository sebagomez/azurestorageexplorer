using Azure;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;
using StorageLibrary.Common;
using web.Utils;

namespace web.Pages
{
	public partial class Containers : BaseComponent
	{
		public string? NewContainerName { get; set; }

		public bool PublicAccess { get; set; }

		public string? SelectedContainer { get; set; }

		List<CloudBlobContainerWrapper> AzureContainers = new List<CloudBlobContainerWrapper>();

		protected override async Task OnInitializedAsync()
		{	
			await base.OnInitializedAsync();

			AzureContainers = await AzureStorage!.Containers.ListContainersAsync();
		}

		public async Task NewContainer()
		{
			if (string.IsNullOrWhiteSpace(NewContainerName))
				return;
			try
			{
				await AzureStorage!.Containers.CreateAsync(NewContainerName, PublicAccess);
			}
			catch (RequestFailedException rfex)
			{
				HasError = true;
				ErrorMessage = rfex.Message;
			} 
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
			finally
			{
				StateHasChanged();
			}
		}

		public void SelectedChanged(MouseEventArgs e, string selectedContainer)
		{
			SelectedContainer = selectedContainer;
		}
	}
}