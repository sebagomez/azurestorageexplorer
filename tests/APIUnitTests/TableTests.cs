using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using StorageHelper;

namespace APIUnitTests
{
	[TestClass]
	public class TableTests : BaseTests
	{
		[TestMethod]
		public void AddData()
		{
			string tableName = "Clarita"; 

			Table.Insert(ACCOUNT, KEY, tableName, "test:test");
		}
	}
}
