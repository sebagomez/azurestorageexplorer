using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

		public string? FileInput { get; set; }

		public string? UploadFolder { get; set; }

		public IBrowserFile? FileToUpload { get; set; }

		public bool ShowTable { get; set; } = false;

		public string Plural { get => FileCount == 1 ? "blob" : "blobs"; }

		public int FileCount { get => AzureContainerBlobs.Count; }

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
				Loading = true;
				ShowTable = false;
				AzureContainerBlobs.Clear();
				AzureContainerFolders.Clear();

				foreach (var blob in await AzureStorage!.Containers.ListBlobsAsync(CurrentContainer!, CurrentPath))
				{
					if (blob.IsFile)
						AzureContainerBlobs.Add(blob);
					else
						AzureContainerFolders.Add(blob);
				}

				AzureContainerFolders = AzureContainerFolders.OrderBy(b => b.Name).ToList();
				AzureContainerBlobs = AzureContainerBlobs.OrderBy(b => b.Name).ToList();

				ShowTable = true;
				Loading = false;
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task DeleteContainer()
		{
			try
			{
				await AzureStorage!.Containers.DeleteAsync(CurrentContainer!);
				await Parent!.SelectionDeletedAsync();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task MoveUp()
		{
			int parentSlash = CurrentPath.LastIndexOf("/", CurrentPath.Length - 2);
			if (parentSlash < 0)
				CurrentPath = "";
			else
				CurrentPath = CurrentPath.Substring(0, parentSlash);

			UploadFolder = CurrentPath;

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

		public async Task DeleteBlob(EventArgs args, string blobUrl)
		{
			try
			{
				Uri uri = new Uri(blobUrl);
				await AzureStorage!.Containers.DeleteBlobAsync(CurrentContainer, uri.LocalPath.Replace($"/{CurrentContainer}/"!, ""));
				await LoadBlobs();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task UploadBlob()
		{
			try
			{
				if (!string.IsNullOrEmpty(CurrentPath) && !CurrentPath.EndsWith("/"))
					CurrentPath += "/";

				await AzureStorage!.Containers.CreateBlobAsync(CurrentContainer, $"{CurrentPath}{FileToUpload!.Name}", FileToUpload.OpenReadStream());
				await LoadBlobs();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void LoadFile(InputFileChangeEventArgs args)
		{
			FileToUpload = args.File;
		}
	}
}