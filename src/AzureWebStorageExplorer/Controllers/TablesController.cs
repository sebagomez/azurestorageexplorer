using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using StorageLibrary;
using StorageLibrary.Common;

namespace AzureWebStorageExplorer.Controllers
{
    [Produces("application/json")]
    [Route("api/Tables")]
    public class TablesController : CounterController
    {
        private static readonly Counter TablesCounter = Metrics.CreateCounter("tablescontroller_counter_total", "Keep TablesController access count");

        [HttpGet("[action]")]
        public async Task<IEnumerable<string>> GetTables(string account, string key)
        {
            Increment(TablesCounter);

            return await Table.ListTablesAsync(account, key);
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<TableEntityWrapper>> QueryTable(string account, string key, string table, string query)
        {
            Increment(TablesCounter);

            return await Table.QueryAsync(account, key, table, query);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> InsertData(string account, string key, string table, string data)
        {
            Increment(TablesCounter);

            if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(data))
                return BadRequest();

            await Table.InsertAsync(account, key, table, data);

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteData(string account, string key, string table, string partitionKey, string rowKey)
        {
            Increment(TablesCounter);

            if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey))
                return BadRequest();

            await Table.DeleteEntityAsync(account, key, table, partitionKey, rowKey);

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteTable(string account, string key, string table)
        {
            Increment(TablesCounter);

            if (string.IsNullOrEmpty(table))
                return BadRequest();

            await Table.Delete(account, key, table);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NewTable(string account, string key, string table)
        {
            Increment(TablesCounter);

            if (string.IsNullOrEmpty(table))
                return BadRequest();

            await Table.Create(account, key, table);

            return Ok();
        }
    }
}
