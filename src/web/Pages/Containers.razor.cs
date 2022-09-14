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

		private Dictionary<string, Dictionary<string, object>> ContainerAtts = new Dictionary<string, Dictionary<string, object>>();

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
			AzureContainers.Clear();
			ContainerAtts.Clear();
			foreach(CloudBlobContainerWrapper container in (await AzureStorage!.Containers.ListContainersAsync()).OrderBy(c => c.Name))
			{
				ContainerAtts[container.Name] = new Dictionary<string, object>();
				AzureContainers.Add(container);
			}
		}

		public async Task NewContainer()
		{
			if (string.IsNullOrWhiteSpace(NewContainerName))
				return;

			try
			{
				await AzureStorage!.Containers.CreateAsync(NewContainerName, PublicAccess);
				NewContainerName = string.Empty;
				await LoadContainers();

			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void SelectedChanged(MouseEventArgs e, string selectedContainer)
		{
			if (SelectedContainer == selectedContainer)
				return;

			if ((!string.IsNullOrEmpty(SelectedContainer)) && ContainerAtts[SelectedContainer].ContainsKey("style"))
			{
				ContainerAtts[SelectedContainer].Remove("style");
				ContainerAtts[SelectedContainer]["class"] = "item";
			}
			SelectedContainer = selectedContainer;

			ContainerAtts[SelectedContainer]["class"] = "item active";
			ContainerAtts[SelectedContainer]["style"] = "font-weight: bold;";

			StateHasChanged();
		}

		public override async Task SelectionDeletedAsync()
		{
			SelectedContainer = null;
			await LoadContainers();

			await base.SelectionDeletedAsync();
		}
	}
}