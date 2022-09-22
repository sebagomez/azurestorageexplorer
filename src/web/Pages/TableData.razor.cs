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
		public string QueryMode { get; set; } = "q";
		public long HeadersCount { get => Headers.LongCount() + (HideKeysCols ? 1 : 3); }
		List<TableEntityWrapper> AzureTableData = new List<TableEntityWrapper>();
		HashSet<string> Headers = new HashSet<string>();
		private Dictionary<string, object> ColAtts = new Dictionary<string, object>();
		public bool HideKeysCols { get; set; } = false;
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

		private async Task InsertData()
		{
			try
			{
				await AzureStorage!.Tables.InsertAsync(CurrentTable, InputQuery);
				InputQuery = string.Empty;
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task QueryData()
		{
			if (QueryMode == "q")
				await LoadData();
			else
				await InsertData();
		}

		public void ShowKeysChanged(ChangeEventArgs arg)
		{
			HideKeysCols = (bool)arg!.Value!;

			if (HideKeysCols)
				ColAtts["style"] = "display: none;";
			else
				ColAtts.Remove("style");

			StateHasChanged();
		}

		public async Task DeleteTable()
		{
			try
			{
				await AzureStorage!.Tables.Delete(CurrentTable);
				await Parent!.SelectionDeletedAsync();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage = ex.Message;
			}
		}

		public async Task DeleteRow(string partitionKey, string rowKey)
		{
			try
			{
				await AzureStorage!.Tables.DeleteEntityAsync(CurrentTable, partitionKey, rowKey);
				await LoadData();
			}
			catch (Exception ex)
			{
				HasError = true;
				ErrorMessage= ex.Message;
			}
		}
	}
}