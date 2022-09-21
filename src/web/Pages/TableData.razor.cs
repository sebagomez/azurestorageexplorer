using Microsoft.AspNetCore.Components;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class TableData : BaseComponent
	{
		[Parameter]
		public string? CurrentTable { get; set; }

		public bool ShowTable { get; set; }

		public string? InputQuery { get; set; }
		public string? QueryMode { get; set; }

		List<TableEntityWrapper> AzureTableData = new List<TableEntityWrapper>();
		HashSet<string> Headers = new HashSet<string>();

		List<Dictionary<string, object>> Values = new List<Dictionary<string, object>>();
		private async Task LoadData()
		{
			if (string.IsNullOrEmpty(CurrentTable))
				return;

			try
			{
				Loading = true;
				ShowTable = false;
				AzureTableData.Clear();
				Headers.Clear();
				foreach (TableEntityWrapper data in await AzureStorage!.Tables.QueryAsync(CurrentTable, InputQuery))
				{
					Headers.UnionWith(data.GetKeys());
					AzureTableData.Add(data);
				}

				ShowTable = true;
				Loading = false;
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public void ModeChanged()
		{}

		public async Task QueryData()
		{
			await LoadData();
		}
	}
}