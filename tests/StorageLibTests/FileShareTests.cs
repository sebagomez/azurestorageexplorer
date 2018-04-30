using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.File;
using StorageLibrary;

namespace StorageLibTests
{
	[TestClass]
	public class FileShareTests : BaseTests
	{
		[TestMethod]
		public async Task GetFileShares()
		{
			List<CloudFileShare> files = await File.ListFileSharesAsync(ACCOUNT, KEY);

			Assert.IsTrue(files.Count > 0, "No files found");

		}

		[TestMethod]
		public async Task GetFileAndDirs()
		{
			string share = "test/Russia2018";
			List<IListFileItem> files = await File.ListFilesAndDirsAsync(ACCOUNT, KEY, share);

			Assert.IsTrue(files.Count > 0, "No files found");

		}
	}
}
