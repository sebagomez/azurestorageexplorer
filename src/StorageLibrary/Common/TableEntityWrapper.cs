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

		Dictionary<string, object> m_properties = new Dictionary<string, object>();

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

		public object this[string key]
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

		public Dictionary<string, object> GetProperties()
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

			Dictionary<string, Type> types = new Dictionary<string, Type>();
			Dictionary<string, object> values = new Dictionary<string, object>();
			foreach (string line in lines)
			{
				string[] tokens = line.Split(new string[] { PROP_VAL_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
				if (tokens.Length != 2)
					throw new ApplicationException($"'{line}' is an invalid field");

				int odata = tokens[0].IndexOf("@odata.type");
				if (odata > 0)
				{
					string field = tokens[0].Substring(0,odata);
					switch (tokens[1].ToLower())
					{
						case "edm.int64":
							types[field] = typeof(long);
							break;
						case "edm.int32":
							types[field] = typeof(int);
							break;
						case "edm.boolean":
							types[field] = typeof(bool);
							break;
						case "edm.datetime":
							types[field] = typeof(DateTime);
							break;
						case "edm.double":
							types[field] = typeof(float);
							break;
						case "edm.guid":
							types[field] = typeof(Guid);
							break;
						default:
						break;
					}
				}
				else
				{
					values[tokens[0]] = tokens[1];
				}
			}

			foreach (string field in values.Keys)
			{
				if (field == "PartitionKey")
					entity.PartitionKey = values[field].ToString();
				else if (field == "RowKey")
					entity.RowKey = values[field].ToString();
				else
				{
					if (types.ContainsKey(field) && types[field] == typeof(Guid))
						entity[field] = Guid.Parse(values[field].ToString());
					else if (types.ContainsKey(field) && types[field] == typeof(DateTime))
						entity[field] = DateTime.SpecifyKind(DateTime.Parse(values[field].ToString()), DateTimeKind.Utc);
					else
						entity[field] = types.ContainsKey(field) ? Convert.ChangeType(values[field], types[field]) : values[field];	
				}
			}

			if (string.IsNullOrEmpty(entity.PartitionKey))
				entity.PartitionKey = "1";
			if (string.IsNullOrEmpty(entity.RowKey))
				entity.RowKey = DateTime.Now.Ticks.ToString();// Guid.NewGuid().ToString();

			return entity;
		}
	}
}
