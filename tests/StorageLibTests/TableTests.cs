using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;

namespace StorageLibTests
{
	[TestClass]
	public class TableTests : BaseTests
	{
		[TestMethod]
		public async Task AddData()
		{
			string tableName = "Clarita"; 

			await Table.InsertAsync(ACCOUNT, KEY, tableName, "test:test");
		}
	}
}
