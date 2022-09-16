using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StorageLibrary.Common;
using web.Utils;

namespace web.Pages
{
	public partial class Files : BaseComponent
	{
		[Parameter]
		public string? CurrentFileShare { get; set; }

		[Parameter]
		public string CurrentPath { get; set; } = "";

		public IBrowserFile? FileToUpload { get; set; }

		public bool ShowTable { get; set; } = false;

		public string Plural { get => FileCount == 1 ? "object" : "objects"; }

		public int FileCount { get => AzureFileShareFiles.Count + AzureFileShareDirectories.Count; }

		List<FileShareItemWrapper> AzureFileShareFiles = new List<FileShareItemWrapper>();
		List<FileShareItemWrapper> AzureFileShareDirectories = new List<FileShareItemWrapper>();

		protected override async Task OnParametersSetAsync()
		{
			await LoadFiles();
		}

		private async Task LoadFiles()
		{
			if (string.IsNullOrEmpty(CurrentFileShare))
				return;

			try
			{
				Loading = true;
				ShowTable = false;
				AzureFileShareFiles.Clear();
				AzureFileShareDirectories.Clear();

				foreach (var file in await AzureStorage!.Files.ListFilesAndDirsAsync(CurrentFileShare, CurrentPath))
				{
					if (file.IsFile)
						AzureFileShareFiles.Add(file);
					else
						AzureFileShareDirectories.Add(file);
				}

				AzureFileShareDirectories = AzureFileShareDirectories.OrderBy(b => b.Name).ToList();
				AzureFileShareFiles = AzureFileShareFiles.OrderBy(b => b.Name).ToList();

				ShowTable = true;
				Loading = false;
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task UploadFile()
		{
			try
			{
				await AzureStorage!.Files.CreateFileAsync(CurrentFileShare, FileToUpload!.Name, FileToUpload.OpenReadStream(Util.MAX_UPLOAD_SIZE), CurrentPath );
				await LoadFiles();
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

		public async Task DeleteFileShare()
		{
			try
			{
				//Delete FileSHare here
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
				CurrentPath = CurrentPath.Substring(0, parentSlash + 1);

			//UploadFolder = CurrentPath;

			StateHasChanged();
			await LoadFiles();
		}

		public async Task EnterFolder(EventArgs args, string fileUrl)
		{
			FileShareItemWrapper file = new FileShareItemWrapper(fileUrl, false, null);
			CurrentPath = file.FullName;

			StateHasChanged();
			await LoadFiles();
		}

		public async Task DownloadFile(EventArgs args, string url)
		{
			

		}

		public async Task DeleteFile(EventArgs args, string url)
		{
			

		}

	}
}