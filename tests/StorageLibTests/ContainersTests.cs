using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using StorageLibrary;
using StorageLibrary.Common;
using StorageLibrary.Mocks;

namespace StorageLibTests
{
	[TestClass]
	public class ContainersTests : BaseTests
	{
		[TestMethod]
		public async Task GetContainerBlobs()
		{
			string containerName = "one";
			List<BlobItemWrapper> expected = new List<BlobItemWrapper> 
			{ 
				new BlobItemWrapper($"{MockUtils.FAKE_URL}/{containerName}/fromOne:1"), 
				new BlobItemWrapper($"{MockUtils.FAKE_URL}/{containerName}/fromOne:2"),
				new BlobItemWrapper($"{MockUtils.FAKE_URL}/{containerName}/fromOne:3")
			};

			StorageFactory factory = new StorageFactory();
			List<BlobItemWrapper> blobs = await factory.Containers.ListBlobsAsync(containerName, string.Empty);

			Assert.IsTrue(expected.Count == blobs.Count, $"Different amount returned. {string.Join(",", blobs)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(blobs[i].Url, expected[i].Url, $"Different objecte returned. Expected '{expected[i].Url}' got '{blobs[i].Url}'");
		}

		[TestMethod]
		public async Task GetAllContainer()
		{
			List<CloudBlobContainerWrapper> expected = new List<CloudBlobContainerWrapper> 
			{ 
				new CloudBlobContainerWrapper { Name = "one"},
				new CloudBlobContainerWrapper { Name =  "two"},
				new CloudBlobContainerWrapper { Name =  "three"},
				new CloudBlobContainerWrapper { Name =  "empty"},
				new CloudBlobContainerWrapper { Name =  "with-folder"},
				new CloudBlobContainerWrapper { Name =  "with-many-folders"},
				new CloudBlobContainerWrapper { Name =  "brothers"}
			};

			StorageFactory factory = new StorageFactory();
			List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

			Assert.IsTrue(expected.Count == containers.Count, $"Different amount returned. {string.Join(",", containers)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(containers[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{containers[i].Name}'");
		}

		[TestMethod]
		public async Task CreatePublicContainer()
		{
			string container = "four";
			List<CloudBlobContainerWrapper> expected = new List<CloudBlobContainerWrapper> 
			{ 
				new CloudBlobContainerWrapper { Name = "one"},
				new CloudBlobContainerWrapper { Name =  "two"},
				new CloudBlobContainerWrapper { Name =  "three"},
				new CloudBlobContainerWrapper { Name =  "empty"},
				new CloudBlobContainerWrapper { Name =  "with-folder"},
				new CloudBlobContainerWrapper { Name =  "with-many-folders"},
				new CloudBlobContainerWrapper { Name =  "brothers"},
				new CloudBlobContainerWrapper { Name =  container}
			};

			StorageFactory factory = new StorageFactory();
			await factory.Containers.CreateAsync(container, true);

			List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

			Assert.IsTrue(expected.Count == containers.Count, $"Different amount returned. {string.Join(",", containers)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(containers[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{containers[i].Name}'");
		}

		[TestMethod]
		public async Task DeleteContainer()
		{
			string container = "one";
			List<CloudBlobContainerWrapper> expected = new List<CloudBlobContainerWrapper> 
			{
				new CloudBlobContainerWrapper { Name =  "two"},
				new CloudBlobContainerWrapper { Name =  "three"},
				new CloudBlobContainerWrapper { Name =  "empty"},
				new CloudBlobContainerWrapper { Name =  "with-folder"},
				new CloudBlobContainerWrapper { Name =  "with-many-folders"},
				new CloudBlobContainerWrapper { Name =  "brothers"},
				new CloudBlobContainerWrapper { Name =  "four"},
			};

			StorageFactory factory = new StorageFactory();
			await factory.Containers.DeleteAsync(container);

			List<CloudBlobContainerWrapper> containers = await factory.Containers.ListContainersAsync();

			Assert.IsTrue(expected.Count == containers.Count, $"Different amount returned. {string.Join(",", containers)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(containers[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{containers[i].Name}'");
		}
	}
}
