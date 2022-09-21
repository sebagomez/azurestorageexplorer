using Microsoft.AspNetCore.Components.Web;
using StorageLibrary.Common;

namespace web.Pages
{
	public partial class Tables : BaseComponent
	{
		public string? SelectedTable { get; set; }
		public string? NewTableName { get; set; }

		private Dictionary<string, Dictionary<string, object>> TablesAtts = new Dictionary<string, Dictionary<string, object>>();
		List<TableWrapper> AzureTables = new List<TableWrapper>();

		protected override async Task OnInitializedAsync()
		{	
			await base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			await LoadTables();
		}

		private async Task LoadTables()
		{
			AzureTables.Clear();
			TablesAtts.Clear();
			foreach(TableWrapper table in (await AzureStorage!.Tables.ListTablesAsync()).OrderBy(t => t.Name))
			{
				TablesAtts[table.Name] = new Dictionary<string, object>();
				AzureTables.Add(table);
			}
		}

		public async Task NewTable()
		{

		}

		public void SelectedChanged(MouseEventArgs e, string table)
		{
			if (SelectedTable == table)
				return;

			if ((!string.IsNullOrEmpty(SelectedTable)) && TablesAtts[SelectedTable].ContainsKey("style"))
			{
				TablesAtts[SelectedTable].Remove("style");
				TablesAtts[SelectedTable]["class"] = "item";
			}
			SelectedTable = table;

			TablesAtts[SelectedTable]["class"] = "item active";
			TablesAtts[SelectedTable]["style"] = "font-weight: bold;";

			StateHasChanged();
		}

		public override async Task SelectionDeletedAsync()
		{
			SelectedTable = null;
			await LoadTables();

			await base.SelectionDeletedAsync();
		}
	}
}