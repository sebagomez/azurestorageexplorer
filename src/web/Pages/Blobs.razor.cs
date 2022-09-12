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

		[Parameter]
		public string CurrentPath { get; set; } = "";

		public string? SelectedBlob { get; set; }

		public bool ShowTable { get; set; } = false;

		public bool RemoveContainerFlag { get; set; } = false;

		public int FolderSpan { get => SelectedBlob != "" ? 3: 2; }

		public string Plural { get => FileCount == 1 ? "blob" : "blobs"; }

		public int FileCount { get => AzureContainerBlobs.Count + AzureContainerFolders.Count; }

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

				AzureContainerFolders = AzureContainerFolders.OrderBy( b => b.Name).ToList();
				AzureContainerBlobs = AzureContainerBlobs.OrderBy( b => b.Name).ToList();
				
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
			RemoveContainerFlag = true;
		}

		public async Task DeleteContainer()
		{
			try
			{
				await AzureStorage!.Containers.DeleteAsync(CurrentContainer!);
				RemoveContainerFlag = false;
				await Parent!.SelectionDeletedAsync();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void CancelDeleteContainer()
		{
			RemoveContainerFlag = false;
		}

		public void FlagBlobToDelete(EventArgs args, string blobUrl)
		{
			SelectedBlob = blobUrl;
		}

		public void CancelDeleteBlob()
		{
			SelectedBlob = "";
		}

		public async Task MoveUp()
		{
			int parentSlash = CurrentPath.LastIndexOf("/", CurrentPath.Length - 2);
			if (parentSlash < 0)
				CurrentPath = "";
			else
				CurrentPath = CurrentPath.Substring(0,parentSlash);
			
			await LoadBlobs();
		}

		public async Task EnterFolder(EventArgs args, string blobUrl)
		{
			Uri uri = new Uri(blobUrl);
			CurrentPath = uri.LocalPath.Replace($"/{CurrentContainer}/"!, "");
			await LoadBlobs();
		}

		public void DownloadBlob()
		{

		}
	}
}