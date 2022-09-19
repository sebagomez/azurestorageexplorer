using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;
using StorageLibrary.Common;
using StorageLibrary.Mocks;

namespace StorageLibTests
{
	[TestClass]
	public class FileShareTests : BaseTests
	{
		[TestMethod]
		public async Task GetFileShares()
		{
			List<FileShareWrapper> expected = new List<FileShareWrapper>
			{
				new FileShareWrapper(){ Name = "two"},
				new FileShareWrapper(){ Name = "three"},
				new FileShareWrapper(){ Name = "empty"},
				new FileShareWrapper(){ Name = "with-folder"},
				new FileShareWrapper(){ Name = "with-many-folders"},
				new FileShareWrapper(){ Name = "brothers"},
				new FileShareWrapper(){ Name = "four"},
			};

			StorageFactory factory = new StorageFactory();
			List<FileShareWrapper> shares = await factory.Files.ListFileSharesAsync();

			Assert.IsTrue(expected.Count == shares.Count, $"Different amount returned. {string.Join(",", shares)}");
			for (int i = 0; i < expected.Count; i++)
				Assert.AreEqual(shares[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{shares[i].Name}'");
		}

		// [TestMethod]
		// public async Task GetFileAndDirs()
		// {
		// 	string share = "test/Russia2018";
		// 	List<FileShareItemWrapper> files = await File.ListFilesAndDirsAsync(ACCOUNT, KEY, share);

		// 	Assert.IsTrue(files.Count > 0, "No files found");

		// }
	}
}
