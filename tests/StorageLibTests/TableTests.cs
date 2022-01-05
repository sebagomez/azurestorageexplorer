using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;

namespace StorageLibTests
{
	[TestClass]
	public class TableTests : BaseTests
	{
		[TestMethod]
		public async Task GetTables()
		{
			List<string> expected = new List<string>() { "tableOne", "tableTwo", "tableThree" };

			StorageFactory factory = new StorageFactory();
			List<string> tables = await factory.Tables.ListTablesAsync();

			CollectionAssert.AreEqual(expected, tables);
		}

		[TestMethod]
		public async Task CreateTable()
		{
			string table = "four";
			List<string> expected = new List<string>() { "tableOne", "tableTwo", "tableThree", table };

			StorageFactory factory = new StorageFactory();
			await factory.Tables.Create(table);

			List<string> queues = await factory.Tables.ListTablesAsync();

			CollectionAssert.AreEqual(expected, queues);
		}

		[TestMethod]
		public async Task DeleteTable()
		{
			string table = "tableTwo";
			List<string> expected = new List<string>() { "tableOne", "tableThree" };

			StorageFactory factory = new StorageFactory();
			await factory.Tables.Delete(table);

			List<string> queues = await factory.Tables.ListTablesAsync();

			CollectionAssert.AreEqual(expected, queues);
		}
	}
}
