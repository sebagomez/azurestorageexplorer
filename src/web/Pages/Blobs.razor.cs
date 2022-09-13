using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Blobs : BaseComponent
	{
		const long MAX_UPLOAD_SIZE = 5 * 1024 * 1024; // 5 Megs

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

		[Inject]
		IJSRuntime? JS {get;set;}

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
			BlobItemWrapper blob = new BlobItemWrapper(blobUrl, 0);
			if (blob.IsFile)
				return;

			CurrentPath = blob.Name;
			await LoadBlobs();
		}

		public  async Task DownloadBlob(EventArgs args, string blobUrl)
		{
			try
			{
				BlobItemWrapper blob = new BlobItemWrapper(blobUrl, 0);
				string path = await AzureStorage!.Containers.GetBlobAsync(CurrentContainer, blob.FullName);

				FileStream fileStream = File.OpenRead(path);

				using var streamRef = new DotNetStreamReference(stream: fileStream);

				await JS!.InvokeVoidAsync("downloadFileFromStream", blob.Name, streamRef);

			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task DeleteBlob(EventArgs args, string blobUrl)
		{
			try
			{
				BlobItemWrapper blob = new BlobItemWrapper(blobUrl, 0);
				await AzureStorage!.Containers.DeleteBlobAsync(CurrentContainer, blob.FullName);
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

				await AzureStorage!.Containers.CreateBlobAsync(CurrentContainer, $"{CurrentPath}{FileToUpload!.Name}", FileToUpload.OpenReadStream(MAX_UPLOAD_SIZE));
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