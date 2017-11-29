using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageLibrary;

namespace AngularWebStorageExplorer.Controllers
{
    [Produces("application/json")]
    [Route("api/Tables")]
    public class TablesController : Controller
    {
        [HttpGet("[action]")]
        public async Task<IEnumerable<string>> GetTables()
        {
			return await Table.ListTablesAsync(Settings.Instance.Account, Settings.Instance.Key);
        }

		[HttpGet("[action]")]
		public async Task<IEnumerable<TableEntity>> QueryTable(string table, string query)
		{
			return await Table.QueryAsync(Settings.Instance.Account, Settings.Instance.Key, table, query);
		}

        // GET: api/Tables/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        //// POST: api/Tables
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}
        
        //// PUT: api/Tables/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
