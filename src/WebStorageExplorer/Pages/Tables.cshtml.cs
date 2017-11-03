using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WindowsAzure.Storage.Table;
using StorageLibrary;

namespace WebStorageExplorer.Pages
{
	public class TablesModel : PageModel
	{
		public List<string> Tables { get; set; }
		public async Task OnGetAsync()
		{
			Tables = await Table.ListTablesAsync(Settings.Instance.Account, Settings.Instance.Key);
		}
	}
}