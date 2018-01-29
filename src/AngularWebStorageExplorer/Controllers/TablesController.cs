using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageLibrary;

namespace AngularWebStorageExplorer.Controllers
{
	[Produces("application/json")]
	[Route("api/Tables")]
	public class TablesController : Controller
	{
		[HttpGet("[action]")]
		public async Task<IEnumerable<string>> GetTables(string account, string key)
		{
			return await Table.ListTablesAsync(account, key);
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<TableEntity>> QueryTable(string account, string key, string table, string query)
		{
			return await Table.QueryAsync(account, key, table, query);
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> NewTable(string account, string key, string table)
		{
			if (string.IsNullOrEmpty(table))
				return BadRequest();

			await Table.Create(account, key, table);

			return Ok();
		}
	}
}
