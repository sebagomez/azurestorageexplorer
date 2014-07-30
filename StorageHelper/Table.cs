using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Data.Services.Client;
using System.Xml.Linq;
using System.Xml;

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

			foreach (string table in tableClient.ListTables())
				yield return table;
		}

		public static void Create(string account, string key, string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				return;

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			tableClient.CreateTable(tableName);
		}

		public static void Delete(string account, string key, string tableName)
		{
			if (string.IsNullOrEmpty(tableName))
				return;

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			tableClient.DeleteTable(tableName);
		}

		public static string QueryREST(string account, string key, string query)
		{
			//http://msdn.microsoft.com/en-us/library/dd179421.aspx Query Entities
			//http://msdn.microsoft.com/en-us/library/dd179428.aspx Authentication Schemes
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}.table.core.windows.net/{1}()", account,query));
			request.UserAgent = "this is me!";
			request.KeepAlive = true;
			request.Method = "GET";
			request.ContentType = "text/plain; charset=UTF-8";
			request.Headers.Add("x-ms-version", "2009-09-19");
			request.Headers.Add("x-ms-date", string.Format("{0} GMT", DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss"))); //Thu, 01 Oct 2009 15:25:14 GMT

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

		public static void Insert(string account, string key, string table, string data)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);
			TableServiceContext context = tableClient.GetDataServiceContext();
			context.WritingEntity += new EventHandler<ReadingWritingEntityEventArgs>(context_WritingEntity);
			TableEntity entity = TableEntity.Get(data);

			context.AddObject(table, entity);

			context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate);
		}

		static void context_WritingEntity(object sender, ReadingWritingEntityEventArgs e)
		{
			TableEntity entity = e.Entity as TableEntity;
			if (entity == null)
				return;

			XElement properties = e.Data.Element(AtomNamespace + "content").Element(AstoriaMetadataNamespace + "properties");

			properties.Element(AstoriaDataNamespace + "Values").Remove();

			foreach (KeyValuePair<string,string> pair in entity.GetProperties())
				properties.Add(new XElement(AstoriaDataNamespace + pair.Key, pair.Value));
		}

		public static IEnumerable<TableEntity> Query(string account, string key, string table, string query)
		{
			//http://social.msdn.microsoft.com/Forums/en-US/windowsazure/thread/481afa1b-03a9-42d9-ae79-9d5dc33b9297/
			//http://stackoverflow.com/questions/2363399/problem-accesing-azure-table-entities/2370814#2370814
			//http://azuretablequery.codeplex.com/

			CloudTableClient tableClient = Client.GetTableClient(account, key);
			TableServiceContext context = tableClient.GetDataServiceContext();
			context.ReadingEntity += new EventHandler<ReadingWritingEntityEventArgs>(context_ReadingEntity);

			StringBuilder builder = new StringBuilder();

			Uri url = new Uri(string.Format("{0}{1}{2}", tableClient.BaseUri, table, string.IsNullOrEmpty(query) ? string.Empty : "?$filter=" + query));
			return context.Execute<TableEntity>(url);
		}

		static void context_ReadingEntity(object sender, ReadingWritingEntityEventArgs args)
		{
			TableEntity entity = args.Entity as TableEntity;
			if (entity == null)
				return;

			// read each property, type and value in the payload   
			var properties = args.Entity.GetType().GetProperties();
			var q = from p in args.Data.Element(AtomNamespace + "content")
									.Element(AstoriaMetadataNamespace + "properties")
									.Elements()
					where properties.All(pp => pp.Name != p.Name.LocalName)
					select new
					{
						Name = p.Name.LocalName,
						p.Value
					};

			foreach (var dp in q)
				entity[dp.Name] = dp.Value;
		}

		public static void DeleteEntity(string account, string key, string table, string partitionKey, string rowKey)
		{
			CloudTableClient tableClient = Client.GetTableClient(account, key);
			TableServiceContext context = tableClient.GetDataServiceContext();
			
			TableEntity entity = new TableEntity() { PartitionKey = partitionKey, RowKey = rowKey };

			context.AttachTo(table, entity, "*");
			context.DeleteObject(entity);
			context.SaveChangesWithRetries(SaveChangesOptions.None);
		}
	}
}
