using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

using StorageLibrary.Common;
using StorageLibrary.Util;

namespace StorageLibrary
{
    public class Table
	{
		public static async Task<List<string>> ListTablesAsync(string account, string key)
		{
			TableServiceClient client = new TableServiceClient(Client.GetConnectionString(account, key));
			List<string> results = new List<string>();
			await foreach(var table in client.QueryAsync())
				results.Add(table.Name);

			return results;
		}

        public static async Task Create(string account, string key, string tableName)
        {
            TableClient table = new TableClient(Client.GetConnectionString(account, key), tableName);

            await table.CreateAsync();
        }

        public static async Task Delete(string account, string key, string tableName)
        {
            TableClient table = new TableClient(Client.GetConnectionString(account, key), tableName);
            await table.DeleteAsync();
        }

        public static async Task InsertAsync(string account, string key, string tableName, string data)
        {
            TableClient table = new TableClient(Client.GetConnectionString(account, key), tableName);
            TableEntity entity = TableEntityWrapper.Get(data);
            await table.AddEntityAsync<TableEntity>(entity);
        }


        public static async Task<IEnumerable<TableEntityWrapper>> QueryAsync(string account, string key, string tableName, string query)
        {
            TableClient table = new TableClient(Client.GetConnectionString(account, key), tableName);
            AsyncPageable<TableEntity> queryResultsFilter = table.QueryAsync<TableEntity>(query);

            List<TableEntityWrapper> results = new List<TableEntityWrapper>();

            await foreach (TableEntity entity in queryResultsFilter)
                results.Add(new TableEntityWrapper(entity));

            return results;
        }

        public static async Task DeleteEntityAsync(string account, string key, string tableName, string partitionKey, string rowKey)
        {
            TableClient table = new TableClient(Client.GetConnectionString(account, key), tableName);
            await table.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
