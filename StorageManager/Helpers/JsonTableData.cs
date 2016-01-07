using StorageHelper;

namespace StorageManager.Helpers
{
	public class JsonTableData
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public string Payload { get; set; }

		public static JsonTableData FromTableEntity(TableEntity table)
		{
			JsonTableData json = new JsonTableData();
			json.PartitionKey = table.PartitionKey;
			json.RowKey = table.RowKey;
			json.Payload = table.Values;
			return json;
		}
	}
}