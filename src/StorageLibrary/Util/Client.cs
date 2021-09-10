namespace StorageLibrary.Util
{
    public class Client
	{
		public static string GetConnectionString(string account, string key)
		{
			return $"DefaultEndpointsProtocol=https;AccountName={account};AccountKey={key};EndpointSuffix=core.windows.net";
		}
	}
}
