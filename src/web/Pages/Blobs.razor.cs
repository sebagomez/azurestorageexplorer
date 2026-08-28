using System.IO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
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

		// Needed to hand the underlying <input type="file"> to JS so the browser can
		// upload the bytes directly instead of streaming them through the circuit.
		protected InputFile? BlobFileInput { get; set; }

		static readonly TimeSpan SAS_LIFETIME = TimeSpan.FromMinutes(15);

		public bool Uploading { get; set; } = false;

		/// <summary>Percent uploaded, or -1 when the transfer cannot report progress.</summary>
		public int UploadPercent { get; set; } = -1;

		public string UploadStatus
		{
			get
			{
				string name = FileToUpload?.Name ?? "file";
				return UploadPercent < 0
					? $"Uploading {name} through the server..."
					: $"Uploading {name}... {UploadPercent}%";
			}
		}

		public bool ShowTable { get; set; } = false;

		public string Plural { get => FileCount == 1 ? "object" : "objects"; }

		public int FileCount { get => AzureContainerBlobs.Count + AzureContainerFolders.Count; }

		List<BlobItemWrapper> AzureContainerBlobs = new List<BlobItemWrapper>();
		List<BlobItemWrapper> AzureContainerFolders = new List<BlobItemWrapper>();

		[Inject]
		IJSRuntime? JS { get; set; }

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
				CurrentPath = CurrentPath.Substring(0, parentSlash + 1);

			UploadFolder = CurrentPath;

			StateHasChanged();
			await LoadBlobs();
		}

		public async Task EnterFolder(EventArgs args, BlobItemWrapper blob)
		{
			if (blob.IsFile)
				return;

			CurrentPath = blob.FullName;
			UploadFolder = CurrentPath;

			StateHasChanged();
			await LoadBlobs();
		}

		public async Task DownloadBlob(EventArgs args, BlobItemWrapper blob)
		{
			string path = "";
			try
			{
				path = await AzureStorage!.Containers.GetBlobAsync(CurrentContainer, blob.FullName);

				FileStream fileStream = File.OpenRead(path);

				using var streamRef = new DotNetStreamReference(stream: fileStream);

				await JS!.InvokeVoidAsync("downloadFileFromStream", blob.Name, streamRef);

			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
			finally
			{
				if (File.Exists(path))
					File.Delete(path);
			}
		}

		public async Task DeleteBlob(EventArgs args, BlobItemWrapper blob)
		{
			try
			{
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
			if (Uploading)
				return;

			if (FileToUpload is null)
			{
				HasError = true;
				ErrorMessage = "No file selected";
				return;
			}

			try
			{
				if (!string.IsNullOrEmpty(UploadFolder) && !UploadFolder.EndsWith("/"))
					UploadFolder += "/";

				string blobName = $"{UploadFolder}{FileToUpload!.Name}";

				Uploading = true;
				UploadPercent = -1;
				StateHasChanged();

				if (!await TryDirectUploadAsync(blobName))
					await ProxyUploadAsync(blobName);

				UploadFolder = string.Empty;
				Uploading = false; // the refresh below has its own indicator
				await LoadBlobs();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = Util.RedactSignatures(ex.Message);
			}
			finally
			{
				Uploading = false;
				UploadPercent = -1;
			}
		}

		/// <summary>
		/// Called from JS as the upload advances. Marshalled onto the renderer's
		/// synchronization context so the progress bar actually repaints.
		/// </summary>
		[JSInvokable]
		public async Task ReportUploadProgress(int percent)
		{
			UploadPercent = percent;
			await InvokeAsync(StateHasChanged);
		}

		/// <summary>
		/// Uploads straight from the browser to storage using a short lived SAS, keeping
		/// the bytes off the Blazor circuit. Returns false when the upload could not be
		/// attempted or never reached storage, meaning the caller should proxy instead.
		/// </summary>
		private async Task<bool> TryDirectUploadAsync(string blobName)
		{
			if (BlobFileInput?.Element is null)
				return false;

			string url = await AzureStorage!.Containers.GetBlobUploadUrlAsync(CurrentContainer!, blobName, SAS_LIFETIME);
			if (string.IsNullOrEmpty(url))
				return false; // these credentials cannot sign an upload URL

			UploadPercent = 0;
			using DotNetObjectReference<Blobs> progress = DotNetObjectReference.Create(this);
			UploadResult result = await JS!.InvokeAsync<UploadResult>("uploadBlobToSasUrl", BlobFileInput.Element, url, progress);

			if (result.Ok)
				return true;

			if (result.Retryable)
				return false; // never reached storage (CORS, offline) - safe to proxy

			// Storage answered and refused; proxying would fail the same way.
			throw new InvalidOperationException(DescribeFailure(result, blobName));
		}

		private async Task ProxyUploadAsync(string blobName)
		{
			// No per-byte progress available on this path.
			UploadPercent = -1;
			await InvokeAsync(StateHasChanged);

			if (FileToUpload!.Size > Util.MAX_UPLOAD_SIZE)
				throw new InvalidOperationException(
					$"Direct upload to storage is unavailable (the account most likely has no CORS rule for this site) and '{FileToUpload.Name}' is larger than the {Util.MAX_UPLOAD_SIZE / (1024 * 1024)} MB limit for uploads routed through the server.");

			using Stream fileStream = FileToUpload.OpenReadStream(Util.MAX_UPLOAD_SIZE);
			await AzureStorage!.Containers.CreateBlobAsync(CurrentContainer, blobName, fileStream);
		}

		private static string DescribeFailure(UploadResult result, string blobName) => result.Status switch
		{
			409 or 412 => $"Blob '{blobName}' already exists",
			403 => "Storage rejected the upload as unauthorized; the signature may have expired",
			_ => $"Upload failed with status {result.Status}{(string.IsNullOrEmpty(result.Error) ? "" : $" ({Util.RedactSignatures(result.Error)})")}"
		};

		private sealed record UploadResult(bool Ok, bool Retryable, int Status, string? Error);

		public Task LoadFile(InputFileChangeEventArgs args)
		{
			FileToUpload = args.File;
			return Task.CompletedTask;
		}
	}
}