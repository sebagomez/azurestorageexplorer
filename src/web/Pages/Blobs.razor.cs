using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary;
using StorageLibrary.Common;
using web.Utils;

namespace web.Pages
{
	public partial class Blobs : BaseComponent
	{
		[Parameter]
		public string? CurrentContainer { get; set; }

		public string CurrentPath { get; set; } = "";

		public string? SelectedBlob { get; set; }

		public bool ShowTable { get; set; } = false;

		public bool RemoveContainerFlag { get; set; } = false;

		public string Plural { get => AzureContainerBlobs.Count + AzureContainerFolders.Count == 1 ? "blob" : "blobs"; }

		List<BlobItemWrapper> AzureContainerBlobs = new List<BlobItemWrapper>();
		List<BlobItemWrapper> AzureContainerFolders = new List<BlobItemWrapper>();

		protected override async Task OnParametersSetAsync()
		{
			await LoadBlobs();
		}

		private async Task LoadBlobs()
		{
			if (string.IsNullOrEmpty(CurrentContainer))
				return;

			try
			{
				Loading= true;
				ShowTable = false;
				AzureContainerBlobs.Clear();
				AzureContainerFolders.Clear();

				foreach(var blob in await AzureStorage!.Containers.ListBlobsAsync(CurrentContainer!, CurrentPath))
				{
					if (blob.IsFolder)
						AzureContainerFolders.Add(blob);
					else
						AzureContainerBlobs.Add(blob);
				}

				AzureContainerFolders.OrderBy( b => b.Name);
				AzureContainerBlobs.OrderBy( b => b.Name);
				
				ShowTable = true;
				Loading = false;
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void FlagContainerToDelete()
		{

		}

		public void DeleteContainer()
		{

		}

		public void CancelDeleteContainer()
		{

		}

		public void FlagBlobToDelete(EventArgs args, string blobUrl)
		{

		}

		public void MoveUp()
		{

		}

		public async Task EnterFolder(EventArgs args, string blobUrl)
		{
			CurrentPath = blobUrl;
			await LoadBlobs();
		}

		public void DownloadBlob()
		{

		}
	}
}