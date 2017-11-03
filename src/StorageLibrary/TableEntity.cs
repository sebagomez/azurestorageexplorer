using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageLibrary
{
	[DataServiceKey("PartitionKey", "RowKey")]
	public class TableEntity : ITableEntity
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public string ETag { get; set; }

		Dictionary<string, string> m_properties = new Dictionary<string, string>();

		internal string this[string key]
		{
			get
			{
				return this.m_properties[key];
			}

			set
			{
				this.m_properties[key] = value;
			}
		}

		public Dictionary<string, string> GetProperties()
		{
			return m_properties;
		}

		public string Values
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				foreach (string key in m_properties.Keys)
					builder.AppendFormat("{0}={1};", key, m_properties[key]);

				return builder.ToString();
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (string key in m_properties.Keys)
				builder.AppendFormat("<b>{0}:</b>{1}<br/>", key, m_properties[key]);

			return builder.ToString();
		}

		public static TableEntity Get(string data)
		{
			TableEntity entity = new TableEntity();

			string[] lines = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			if (lines.Count() < 1)
				throw new Exception($"'{data}' is not a valid input");

			foreach (string line in lines)
			{
				string[] values = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
				Debug.Assert(values.Length == 2);

				if (values.Count() != 2)
					continue;

				if (values[0] == "PartitionKey")
					entity.PartitionKey = values[1];
				else if (values[0] == "RowKey")
					entity.RowKey = values[1];
				else
					entity[values[0]] = values[1];
			}

			if (string.IsNullOrEmpty(entity.PartitionKey))
				entity.PartitionKey = "1";
			if (string.IsNullOrEmpty(entity.RowKey))
				entity.RowKey = DateTime.Now.Ticks.ToString();// Guid.NewGuid().ToString();

			return entity;
		}

		public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
		{
			foreach (var p in properties)
				this[p.Key] = p.Value.PropertyAsObject.ToString();
		}

		public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
		{
			IDictionary<string, EntityProperty> dictionary = new Dictionary<string, EntityProperty>();

			foreach (var p in GetProperties())
				dictionary[p.Key] = new EntityProperty(p.Value);

			return dictionary;
		}
	}
}
