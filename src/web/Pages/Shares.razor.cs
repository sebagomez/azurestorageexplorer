using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Shares : BaseComponent
	{
		private Dictionary<string, Dictionary<string, object>> FileShareAtts = new Dictionary<string, Dictionary<string, object>>();

		[Parameter]
		public string? SelectedFileShare { get; set; }

		List<FileShareWrapper> AzureFileShares = new List<FileShareWrapper>();

		public string NewFileShareName { get; set; }

		protected override async Task OnInitializedAsync()
		{	
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

		}

		public void SelectedChanged(MouseEventArgs e, string selectedFileShare)
		{

		}

	}
}
