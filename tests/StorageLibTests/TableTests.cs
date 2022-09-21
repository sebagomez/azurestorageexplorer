using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;
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
			List<string> expected = new List<string>() { "tableOne", "tableThree" };

			StorageFactory factory = new StorageFactory();
			await factory.Tables.Delete(table);

			List<string> queues = await factory.Tables.ListTablesAsync();

			CollectionAssert.AreEqual(expected, queues);
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
	}
}
