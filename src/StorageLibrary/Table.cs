using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageLibrary
{
	public class Table
	{
		static XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";
		static XNamespace AstoriaDataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
		static XNamespace AstoriaMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

		public static async Task<List<string>> ListTablesAsync(string account, string key)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);

			TableContinuationToken continuationToken = null;
			List<CloudTable> results = new List<CloudTable>();
			do
			{
				var response = await tableClient.ListTablesSegmentedAsync(continuationToken);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);
			return results.Select( t => t.Name).ToList();
		}

		public static CloudTable Get(string account, string key, string tableName)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);

			return tableClient.GetTableReference(tableName);
		}

		public static async Task Create(string account, string key, string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				return;

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			await tableClient.GetTableReference(tableName).CreateIfNotExistsAsync();

		}

		public static async Task Delete(string account, string key, string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				return;

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			await tableClient.GetTableReference(tableName).DeleteAsync();
		}

		public static string QueryREST(string account, string key, string query)
		{
			//http://msdn.microsoft.com/en-us/library/dd179421.aspx Query Entities
			//http://msdn.microsoft.com/en-us/library/dd179428.aspx Authentication Schemes
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{account}.table.core.windows.net/{query}()");
			request.UserAgent = "this is me!";
			request.KeepAlive = true;
			request.Method = "GET";
			request.ContentType = "text/plain; charset=UTF-8";
			request.Headers.Add("x-ms-version", "2009-09-19");
			request.Headers.Add("x-ms-date", $"{DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss")} GMT"); //Thu, 01 Oct 2009 15:25:14 GMT

			string autorizationHeader = Client.GetSignedKey(request.Method, request.ContentType, request.Headers["x-ms-date"], request.RequestUri, account, key);

			request.Headers.Add("Authorization", autorizationHeader);
			request.Accept = "application/atom+xml,application/xml";
			request.Headers.Add("Accept-Charset", "UTF-8");
			request.Headers.Add("DataServiceVersion", "1.0;NetFx");
			request.Headers.Add("MaxDataServiceVersion", "1.0;NetFx");
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			string data = string.Empty;
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				data = reader.ReadToEnd();

			return data;
		}

		public static async Task InsertAsync(string account, string key, string tableName, string data)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);
			CloudTable table = tableClient.GetTableReference(tableName);
			TableEntity entity = TableEntity.Get(data);
			TableOperation insert = TableOperation.Insert(entity);
			await table.ExecuteAsync(insert);
		}


		public static async Task<IEnumerable<TableEntity>> QueryAsync(string account, string key, string tableName, string query)
		{
			//http://gauravmantri.com/2012/11/17/storage-client-library-2-0-migrating-table-storage-code/

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			CloudTable table = tableClient.GetTableReference(tableName);

			TableContinuationToken continuationToken = null;
			List<TableEntity> results = new List<TableEntity>();

			do
			{
				var response = await table.ExecuteQuerySegmentedAsync<TableEntity>(new TableQuery<TableEntity> { FilterString = query }, continuationToken);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);

			return results;
		}

		public static async Task DeleteEntityAsync(string account, string key, string tableName, string partitionKey, string rowKey)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);
			CloudTable table = tableClient.GetTableReference(tableName);
			TableEntity entity = new TableEntity() { PartitionKey = partitionKey, RowKey = rowKey, ETag = "*" };
			TableOperation delete = TableOperation.Delete(entity);
			await table.ExecuteAsync(delete);
		}
	}
}
