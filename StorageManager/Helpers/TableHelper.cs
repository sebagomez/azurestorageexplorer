using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageHelper;
using System.Data;

namespace StorageManager.Helpers
{
    public class TableHelper
    {
        public string Name { get; set; }

		public static List<TableHelper> GetAll(string account, string key)
        {
			List<TableHelper> list = new List<TableHelper>();
            foreach (string tableName in Table.GetAll(account,key))
            {
				TableHelper table = new TableHelper() { Name = tableName};
                list.Add(table);
            }

            return list;
        }

		public static DSEntities Query(string account, string key, string table, string query)
		{
			return GetDataSet(StorageHelper.Table.Query(account, key, table, query));
		}

		public static List<JsonTableData> QueryEntities(string account, string key, string table, string query)
		{
			List<JsonTableData> list = new List<JsonTableData>();
			foreach (TableEntity data in StorageHelper.Table.Query(account, key, table, query))
				list.Add(JsonTableData.FromTableEntity(data));

			return list;
		}

		static DSEntities GetDataSet(IEnumerable<TableEntity> list)
		{
			DSEntities ds = new DSEntities();

			foreach (TableEntity entity in list)
			{
				DSEntities.TableEntityRow row = ds.TableEntity.NewTableEntityRow();
				row.PartitionKey = entity.PartitionKey;
				row.RowKey = entity.RowKey;

				foreach (KeyValuePair<string,string> property in entity.GetProperties())
				{
					if (!ds.TableEntity.Columns.Contains(property.Key))
						ds.TableEntity.Columns.Add(new DataColumn(property.Key, typeof(string)));
					
					row[property.Key] = property.Value;
				}

				ds.TableEntity.Rows.Add(row);
			}

			return ds;
		}

    }
}
