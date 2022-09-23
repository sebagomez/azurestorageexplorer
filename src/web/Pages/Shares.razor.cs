using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Prometheus;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Shares : BaseComponent
	{
		private Dictionary<string, Dictionary<string, object>> FileShareAtts = new Dictionary<string, Dictionary<string, object>>();

		[Parameter]
		public string? SelectedFileShare { get; set; }

		List<FileShareWrapper> AzureFileShares = new List<FileShareWrapper>();

		public string? NewFileShareName { get; set; }

		private static readonly Counter FilesCounter = Metrics.CreateCounter("filescontroller_counter_total", "Keep FilesController access count");

		public string? AccessTier { get; set; } = "optimized";

		protected override async Task OnInitializedAsync()
		{	
			Increment(FilesCounter);
			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			await LoadFileShares();
		}

		private async Task LoadFileShares()
		{
			AzureFileShares.Clear();
			FileShareAtts.Clear();
			foreach(FileShareWrapper fileShare in (await AzureStorage!.Files.ListFileSharesAsync()).OrderBy(f => f.Name))
			{
				FileShareAtts[fileShare.Name] = new Dictionary<string, object>();
				AzureFileShares.Add(fileShare);
			}
		}

		public async Task NewFileShare()
		{
			if (string.IsNullOrWhiteSpace(NewFileShareName))
				return;

			try
			{
				await AzureStorage!.Files.CreateFileShareAsync(NewFileShareName, AccessTier);
				AccessTier = "optimized";
				NewFileShareName = string.Empty;
				await LoadFileShares();

			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void SelectedChanged(MouseEventArgs e, string selectedFileShare)
		{
			if (SelectedFileShare == selectedFileShare)
				return;

			if ((!string.IsNullOrEmpty(SelectedFileShare)) && FileShareAtts[SelectedFileShare].ContainsKey("style"))
			{
				FileShareAtts[SelectedFileShare].Remove("style");
				FileShareAtts[SelectedFileShare]["class"] = "item";
			}
			SelectedFileShare = selectedFileShare;

			FileShareAtts[SelectedFileShare]["class"] = "item active";
			FileShareAtts[SelectedFileShare]["style"] = "font-weight: bold;";

			StateHasChanged();
		}

		public override async Task SelectionDeletedAsync()
		{
			SelectedFileShare = null;
			await LoadFileShares();

			await base.SelectionDeletedAsync();
		}
	}
}
