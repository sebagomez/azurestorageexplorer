using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;
using StorageLibrary.Common;
using StorageLibrary.Mocks;

namespace StorageLibTests
{
	[TestClass]
	public class TableTests : BaseTests
	{
		[ClassInitialize]
		public static void Initialize(TestContext ctx)
		{
			MockUtils.Reintialize();
		}

		[TestMethod]
		public async Task GetTables()
		{
			List<TableWrapper> expected = new List<TableWrapper>() 
			{
				new TableWrapper{ Name = "tableOne"}, 
				new TableWrapper{ Name = "tableTwo"},
				new TableWrapper{ Name = "tableThree"} 
			};

			StorageFactory factory = new StorageFactory();
			List<TableWrapper> tables = await factory.Tables.ListTablesAsync();

			CompareTables(expected, tables);
		}

		[TestMethod]
		public async Task CreateTable()
		{
			string table = "four";
			List<TableWrapper> expected = new List<TableWrapper>() 
			{ 
				new TableWrapper{ Name = "tableOne"},
				new TableWrapper{ Name =  "tableTwo"}, 
				new TableWrapper{ Name = "tableThree"}, 
				new TableWrapper{ Name = table }
			};

			StorageFactory factory = new StorageFactory();
			await factory.Tables.Create(table);

			List<TableWrapper> tables = await factory.Tables.ListTablesAsync();

			CompareTables(expected, tables);
		}

		[TestMethod]
		public async Task FailCreateTable()
		{
			string table = "tableOne";

			StorageFactory factory = new StorageFactory();
			try
			{
				await factory.Tables.Create(table);
			}
			catch (InvalidOperationException ioe)
			{
				Assert.IsTrue(ioe.Message == $"Table '{table}' already exists", ioe.Message);
				return;
			}

			Assert.Fail("An InvalidOperationException should have been thrown.");
			
		}

		[TestMethod]
		public async Task DeleteTable()
		{
			string table = "tableTwo";
			List<TableWrapper> expected = new List<TableWrapper>() 
			{ 
				new TableWrapper{ Name = "tableOne"}, 
				new TableWrapper{ Name = "tableThree" }
			};

			StorageFactory factory = new StorageFactory();
			await factory.Tables.Delete(table);

			List<TableWrapper> tables = await factory.Tables.ListTablesAsync();

			CompareTables(expected, tables);
		}

		[TestMethod]
		public async Task FailDeleteTable()
		{
			string table = "tableFour";

			StorageFactory factory = new StorageFactory();
			try
			{
				await factory.Tables.Delete(table);
			}
			catch(NullReferenceException nre)
			{
				Assert.IsTrue(nre.Message == $"Table '{table}' does not exist", nre.Message);
				return;
			}

			Assert.Fail("An NullReferenceException should have been thrown.");
		}

		private void CompareTables(List<TableWrapper> expected, List<TableWrapper> returned)
		{
			Assert.IsTrue(expected.Count == returned.Count, $"Different amount returned. {string.Join(",", returned)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(returned[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{returned[i].Name}'");
		}
	}
}
