using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;
using StorageLibrary.Common;

namespace StorageLibTests
{
    [TestClass]
	public class FileShareTests : BaseTests
	{
		[TestMethod]
		public async Task GetFileShares()
		{
			List<FileShareWrapper> files = await File.ListFileSharesAsync(ACCOUNT, KEY);

			Assert.IsTrue(files.Count > 0, "No files found");

		}

		[TestMethod]
		public async Task GetFileAndDirs()
		{
			string share = "test/Russia2018";
			List<FileShareItemWrapper> files = await File.ListFilesAndDirsAsync(ACCOUNT, KEY, share);

			Assert.IsTrue(files.Count > 0, "No files found");

		}
	}
}
