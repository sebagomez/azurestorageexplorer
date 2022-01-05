
using StorageLibrary.Interfaces;
using StorageLibrary.Mocks;

namespace StorageLibrary
{
	public class StorageFactory
	{
		public IQueue Queue { get; private set; }

		public StorageFactory()
		: this(string.Empty, string.Empty, string.Empty, true)
		{ }

		public StorageFactory(string account, string key, string endpoint = "core.windows.net", bool mock = false)
		{
			if (mock)
			{
				Queue = new MockQueue();
			}
			else
			{
				Queue = new AzureQueue(account, key, endpoint);
			}
		}
	}
}