using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Data.Tables;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.Mocks
{
	internal class MockTable : ITable
	{
		Dictionary<string, List<string>> tables = new Dictionary<string, List<string>>()
		{
			{ "tableOne", new List<string> {"fromTableOne:1", "fromTableOne:2", "fromTableOne:3"}},
			{ "tableTwo", new List<string> {"fromTableTwo:1", "fromTableTwo:2"}},
			{ "tableThree", new List<string> {"fromTableThree:1"}}
		};

		public async Task<List<TableWrapper>> ListTablesAsync()
		{
			return await Task.Run(() =>
			{
				List<TableWrapper> retList = new List<TableWrapper>();
				foreach (string key in tables.Keys)
					retList.Add(new TableWrapper { Name = key });

				return retList;
			});
		}

		public async Task Create(string tableName)
		{
			await Task.Run(() =>
			{
				if (tables.ContainsKey(tableName))
					throw new InvalidOperationException($"Table '{tableName}' already exists");

				tables.Add(tableName, new List<string>());
			});
		}

		public async Task Delete(string tableName)
		{
			await Task.Run(() =>
			{
				if (!tables.ContainsKey(tableName))
					throw new NullReferenceException($"Table '{tableName}' does not exist");

				tables.Remove(tableName);
			});
		}

		public async Task InsertAsync(string tableName, string data)
		{
			await Task.Run(() =>
			{
				if (!tables.ContainsKey(tableName))
					throw new NullReferenceException($"Table '{tableName}' does not exist");

				tables[tableName].Add(data);
			});
		}


		public async Task<IEnumerable<TableEntityWrapper>> QueryAsync(string tableName, string query)
		{
			return await Task.Run(() =>
			{
				if (!tables.ContainsKey(tableName))
					throw new NullReferenceException($"Table '{tableName}' does not exist");

				List<TableEntityWrapper> results = new List<TableEntityWrapper>();

				foreach (string val in tables[tableName])
				{
					if (query is null || val.Contains(query))
					{
						TableEntity entity = TableEntityWrapper.Get($"Name={val}");
						results.Add(new TableEntityWrapper(entity));
					}
				}

				return results;
			});
		}

		public async Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey)
		{
			await Task.Run(() =>
			{
				if (!tables.ContainsKey(tableName))
					throw new NullReferenceException($"Table '{tableName}' does not exist");
			});
		}
	}
}
