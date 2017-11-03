using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageHelper
{
	public class Table
	{
		static XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";
		static XNamespace AstoriaDataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices";
		static XNamespace AstoriaMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

		public static IEnumerable<string> GetAll(string account, string key)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);

			foreach (CloudTable table in tableClient.ListTables())
				yield return table.Name;
		}

		public static CloudTable Get(string account, string key, string tableName)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);

			return tableClient.GetTableReference(tableName);
		}

		public static void Create(string account, string key, string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				return;

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			tableClient.GetTableReference(tableName).CreateIfNotExists();

		}

		public static void Delete(string account, string key, string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				return;

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			tableClient.GetTableReference(tableName).Delete();
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

		public static void Insert(string account, string key, string tableName, string data)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);
			CloudTable table = tableClient.GetTableReference(tableName);
			TableEntity entity = TableEntity.Get(data);
			TableOperation insert = TableOperation.Insert(entity);
			table.Execute(insert);
		}

		public static IEnumerable<TableEntity> Query(string account, string key, string tableName, string query)
		{
			//http://gauravmantri.com/2012/11/17/storage-client-library-2-0-migrating-table-storage-code/

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			CloudTable table = tableClient.GetTableReference(tableName);
			return table.ExecuteQuery<TableEntity>(new TableQuery<TableEntity> { FilterString = query });
		}

		public static void DeleteEntity(string account, string key, string tableName, string partitionKey, string rowKey)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);
			CloudTable table = tableClient.GetTableReference(tableName);
			TableEntity entity = new TableEntity() { PartitionKey = partitionKey, RowKey = rowKey, ETag = "*" };
			TableOperation delete = TableOperation.Delete(entity);
			table.Execute(delete);
		}
	}
}
