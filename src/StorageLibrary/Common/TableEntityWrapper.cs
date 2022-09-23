using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Azure;
using Azure.Data.Tables;

namespace StorageLibrary.Common
{
	[DataServiceKey("PartitionKey", "RowKey")]
	public class TableEntityWrapper : ITableEntity
	{
		static string PROP_SEPARATOR = Environment.NewLine;
		static string PROP_VAL_SEPARATOR = "=";
		readonly string[] KEYWORDS = { "PartitionKey", "RowKey", "Timestamp", "odata.etag" };

		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		Dictionary<string, string> m_properties = new Dictionary<string, string>();

		public TableEntityWrapper()
		{ }

		public TableEntityWrapper(TableEntity entity)
		{
			PartitionKey = entity.PartitionKey;
			RowKey = entity.RowKey;
			Timestamp = entity.Timestamp;
			ETag = entity.ETag;

			foreach (string key in entity.Keys)
			{
				if (KEYWORDS.Contains(key))
					continue;

				this[key] = entity[key].ToString();
			}
		}

		public string this[string key]
		{
			get
			{
				if (this.m_properties.ContainsKey(key))
					return this.m_properties[key];
				return "";
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

		public List<string> GetKeys()
		{
			return m_properties.Keys.ToList();
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

			string[] lines = data.Split(new string[] { PROP_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

			if (lines.Count() < 1)
				throw new ApplicationException($"'{data}' is not a valid input");

			foreach (string line in lines)
			{
				string[] values = line.Split(new string[] { PROP_VAL_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
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
	}
}
