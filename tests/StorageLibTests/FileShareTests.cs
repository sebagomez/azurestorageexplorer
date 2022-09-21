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
		[ClassInitialize]
		public static void Initialize(TestContext ctx)
		{
			MockUtils.Reintialize();
		}

		[TestMethod]
		public async Task GetFileShares()
		{
			List<FileShareWrapper> expected = new List<FileShareWrapper>
			{
				new FileShareWrapper(){ Name = "one"},
				new FileShareWrapper(){ Name = "two"},
				new FileShareWrapper(){ Name = "three"},
				new FileShareWrapper(){ Name = "empty"},
				new FileShareWrapper(){ Name = "with-folder"},
				new FileShareWrapper(){ Name = "with-many-folders"},
				new FileShareWrapper(){ Name = "brothers"},
			};

			StorageFactory factory = new StorageFactory();
			List<FileShareWrapper> shares = await factory.Files.ListFileSharesAsync();

			Assert.IsTrue(expected.Count == shares.Count, $"Different amount returned. {string.Join(",", shares)}");
			for (int i = 0; i < expected.Count; i++)
				Assert.AreEqual(shares[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{shares[i].Name}'");
		}

		[TestMethod]
		public async Task GetShareFiles()
		{
			string fileShareName = "one";
			List<FileShareItemWrapper> expected = new List<FileShareItemWrapper> 
			{ 
				new FileShareItemWrapper($"{MockUtils.FAKE_URL}/{fileShareName}/fromOne:1", true, MockUtils.NewRandomSize), 
				new FileShareItemWrapper($"{MockUtils.FAKE_URL}/{fileShareName}/fromOne:2", true, MockUtils.NewRandomSize),
				new FileShareItemWrapper($"{MockUtils.FAKE_URL}/{fileShareName}/fromOne:3", true, MockUtils.NewRandomSize)
			};

			StorageFactory factory = new StorageFactory();
			List<FileShareItemWrapper> files = await factory.Files.ListFilesAndDirsAsync(fileShareName, string.Empty);

			Assert.IsTrue(expected.Count == files.Count, $"Different amount returned. {string.Join(",", files)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(files[i].Url, expected[i].Url, $"Different objecte returned. Expected '{expected[i].Url}' got '{files[i].Url}'");
		}

		// [TestMethod]
		// public async Task CreateFileShare()
		// {
		// 	string fileShare = "four";
		// 	List<FileShareWrapper> expected = new List<FileShareWrapper> 
		// 	{ 
		// 		new FileShareWrapper { Name = "one"},
		// 		new FileShareWrapper { Name =  "two"},
		// 		new FileShareWrapper { Name =  "three"},
		// 		new FileShareWrapper { Name =  "empty"},
		// 		new FileShareWrapper { Name =  "with-folder"},
		// 		new FileShareWrapper { Name =  "with-many-folders"},
		// 		new FileShareWrapper { Name =  "brothers"},
		// 		new FileShareWrapper { Name =  fileShare}
		// 	};

		// 	StorageFactory factory = new StorageFactory();
		// 	await factory.Containers.CreateAsync(container, true);

		// 	List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

		// 	Assert.IsTrue(expected.Count == containers.Count, $"Different amount returned. {string.Join(",", containers)}");
		// 	for	(int i = 0; i < expected.Count; i++)
		// 		Assert.AreEqual(containers[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{containers[i].Name}'");
		// }

		// [TestMethod]
		// public async Task DeleteFileShare()
		// {
		// 	string container = "one";
		// 	List<CloudBlobContainerWrapper> expected = new List<CloudBlobContainerWrapper> 
		// 	{
		// 		new CloudBlobContainerWrapper { Name =  "two"},
		// 		new CloudBlobContainerWrapper { Name =  "three"},
		// 		new CloudBlobContainerWrapper { Name =  "empty"},
		// 		new CloudBlobContainerWrapper { Name =  "with-folder"},
		// 		new CloudBlobContainerWrapper { Name =  "with-many-folders"},
		// 		new CloudBlobContainerWrapper { Name =  "brothers"},
		// 		new CloudBlobContainerWrapper { Name =  "four"},
		// 	};

		// 	StorageFactory factory = new StorageFactory();
		// 	await factory.Containers.DeleteAsync(container);

		// 	List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

		// 	Assert.IsTrue(expected.Count == containers.Count, $"Different amount returned. {string.Join(",", containers)}");
		// 	for	(int i = 0; i < expected.Count; i++)
		// 		Assert.AreEqual(containers[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{containers[i].Name}'");
		// }
	}
}
