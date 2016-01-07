using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using StorageManager.Helpers;

namespace StorageManager.API
{
	[ServiceContract]
	public class Rest
	{
		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/containers?account={account}&key={key}")]
		public List<ContainerHelper> GetContainers(string account, string key)
		{
			return ContainerHelper.GetAll(account, key);
		}

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/blobs?account={account}&key={key}&container={container}")]
		public List<BlobHelper> GetContainerBlobs(string account, string key, string container)
		{
			return BlobHelper.GetAll(container, account, key);
		}

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/tables?account={account}&key={key}")]
		public List<TableHelper> GetTables(string account, string key)
		{
			return TableHelper.GetAll(account, key);
		}

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/query?account={account}&key={key}&table={table}&query={query}")]
		public List<JsonTableData> QueryTable(string account, string key, string table, string query)
		{
			return TableHelper.QueryEntities(account, key, table, query);
		}

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/queues?account={account}&key={key}")]
		public List<QueueHelper> GetQueues(string account, string key)
		{
			return QueueHelper.GetAll(account, key);
		}

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/messages?account={account}&key={key}&queue={queue}")]
		public List<MessageHelper> GetMessages(string account, string key, string queue)
		{
			return MessageHelper.GetAll(queue, account, key);
		}
	}
}
