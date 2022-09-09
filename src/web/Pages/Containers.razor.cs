using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Containers : BaseComponent
	{
		public string? NewContainerName { get; set; }

		public bool PublicAccess { get; set; }

		[Parameter]
		public string? SelectedContainer { get; set; }

		List<CloudBlobContainerWrapper> AzureContainers = new List<CloudBlobContainerWrapper>();

		protected override async Task OnInitializedAsync()
		{	
			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			await LoadContainers();
		}

		private async Task LoadContainers()
		{
			AzureContainers = (await AzureStorage!.Containers.ListContainersAsync()).OrderBy(c => c.Name ).ToList();
		}

		public async Task NewContainer()
		{
			if (string.IsNullOrWhiteSpace(NewContainerName))
				return;

			try
			{
				await AzureStorage!.Containers.CreateAsync(NewContainerName, PublicAccess);
				NewContainerName = string.Empty;
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
		}

		public void SelectedChanged(MouseEventArgs e, string selectedContainer)
		{
			SelectedContainer = selectedContainer;
		}

		public override async Task SelectionDeletedAsync()
		{
			SelectedContainer = null;
			await LoadContainers();

			await base.SelectionDeletedAsync();
		}
	}
}