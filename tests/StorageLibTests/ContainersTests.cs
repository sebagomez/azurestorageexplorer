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
		[ClassInitialize]
		public static void Initialize(TestContext ctx)
		{
			MockUtils.Reintialize();
		}

		[TestMethod]
		public async Task GetContainerBlobs()
		{
			string containerName = "one";
			List<string> expected = new List<string>
			{
				$"{MockUtils.FAKE_URL}/{containerName}/fromOne:1",
				$"{MockUtils.FAKE_URL}/{containerName}/fromOne:2",
				$"{MockUtils.FAKE_URL}/{containerName}/fromOne:3"
			};

			StorageFactory factory = new StorageFactory();
			List<BlobItemWrapper> blobs = await factory.Containers.ListBlobsAsync(containerName, string.Empty);

			Assert.HasCount(expected.Count, blobs, $"Different amount returned. {string.Join(",", blobs)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(blobs[i].Url, expected[i], $"Different objecte returned. Expected '{expected[i]}' got '{blobs[i].Url}'");
		}

		/// <summary>
		/// The wrapper must hand back exactly the name the provider reported, split into a
		/// parent path and a leaf. This used to be derived by parsing the URL, which broke
		/// on path-style endpoints and on names needing percent-encoding.
		/// </summary>
		[TestMethod]
		public async Task BlobNamesRoundTrip()
		{
			string containerName = "with-many-folders";

			StorageFactory factory = new StorageFactory();
			List<BlobItemWrapper> blobs = await factory.Containers.ListBlobsAsync(containerName, "folder1/");

			// folder1/file1 and folder1/folder11/
			Assert.HasCount(2, blobs, $"Unexpected listing for 'folder1/'. {string.Join(",", blobs)}");

			foreach (BlobItemWrapper blob in blobs)
			{
				Assert.AreEqual(containerName, blob.Container, $"Wrong container for '{blob.FullName}'");
				Assert.AreEqual(blob.FullName, $"{blob.Path}{blob.Name}", $"Path+Name does not reproduce FullName for '{blob.FullName}'");
				Assert.AreEqual("folder1/", blob.Path, $"Wrong path for '{blob.FullName}'");
				Assert.AreEqual(blob.IsFile, !blob.Name.EndsWith("/"), $"Wrong kind for '{blob.FullName}'");
			}

			BlobItemWrapper folder = blobs.Find(b => !b.IsFile);
			Assert.IsNotNull(folder, "Expected a nested folder under 'folder1/'");
			Assert.AreEqual("folder1/folder11/", folder.FullName, "A prefix must keep its trailing slash");
		}

		/// <summary>
		/// A blob at the root of a container has no parent path. This is the case that used to
		/// throw 'length must be a non-negative value' when the account was reached through a
		/// host-style URL while the azurite flag was set.
		/// </summary>
		[TestMethod]
		public async Task RootBlobHasNoPath()
		{
			StorageFactory factory = new StorageFactory();
			List<BlobItemWrapper> blobs = await factory.Containers.ListBlobsAsync("with-many-folders", string.Empty);

			BlobItemWrapper root = blobs.Find(b => b.IsFile);
			Assert.IsNotNull(root, "Expected a file at the container root");
			Assert.AreEqual(string.Empty, root.Path, "A root blob must have an empty path");
			Assert.AreEqual("file-at-root", root.FullName);
			Assert.AreEqual("file-at-root", root.Name);
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

			Assert.HasCount(expected.Count, containers, $"Different amount returned. {string.Join(",", containers)}");
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

			Assert.HasCount(expected.Count, containers, $"Different amount returned. {string.Join(",", containers)}");
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

			Assert.HasCount(expected.Count, containers, $"Different amount returned. {string.Join(",", containers)}");
			for	(int i = 0; i < expected.Count; i++)
				Assert.AreEqual(containers[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{containers[i].Name}'");
		}
	}
}
