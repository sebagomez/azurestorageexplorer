using System.Collections.Generic;
using System.Threading.Tasks;

using StorageLibrary.Common;

namespace StorageLibrary.Interfaces
{
	public interface ITable 
	{
		Task<List<TableWrapper>> ListTablesAsync();
		Task Create(string tableName);
		Task Delete(string tableName);
		Task InsertAsync(string tableName, string data);
		Task<IEnumerable<TableEntityWrapper>> QueryAsync(string tableName, string query);
		Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey);
	}
}