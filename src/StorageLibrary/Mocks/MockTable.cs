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

		Dictionary<string, List<TableEntityWrapper>> tables = new Dictionary<string, List<TableEntityWrapper>>()
		{
			{ "tableOne", new List<TableEntityWrapper> { new TableEntityWrapper() { ["Number"]=1, ["Bool"]=true,["String"]="foo" }, new TableEntityWrapper() { ["Number"]=2, ["Bool"]=true,["String"]="bar" }}},
			{ "tableTwo", new List<TableEntityWrapper> { new TableEntityWrapper() { ["Number"]=2, ["Bool"]=true,["String"]="bar" }}},
			{ "tableThree", new List<TableEntityWrapper> { new TableEntityWrapper() { ["Number"]=3, ["Bool"]=false,["String"]="choo" }}}
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

				tables.Add(tableName, new List<TableEntityWrapper>());
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

				tables[tableName].Add(new TableEntityWrapper(TableEntityWrapper.Get(data)));
			});
		}


		public async Task<IEnumerable<TableEntityWrapper>> QueryAsync(string tableName, string query)
		{
			return await Task.Run(() =>
			{
				if (!tables.ContainsKey(tableName))
					throw new NullReferenceException($"Table '{tableName}' does not exist");

				List<TableEntityWrapper> results = new List<TableEntityWrapper>();
				if (query is null)
					return tables[tableName];

				string[] tokens = query.Split("eq");
				if (tokens.Length != 2)
					throw new Exception($"{query} is not a valid 'field = value' query.");

				string field = tokens[0].Trim();
				object value;
				switch (field)
				{
					case "Number":
						value = int.Parse(tokens[1].Trim());
						break;
					case "Bool":
						value = bool.Parse(tokens[1].Trim());
						break;
					case "String":
						value = tokens[1].Trim();
						break;
					default:
						throw new Exception($"Only Number, Bool and String are testable querable fields");
				};


				foreach (TableEntityWrapper data in tables[tableName])
				{
					if (object.Equals(data[field],value))
						results.Add(data);
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
