using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageLibrary;
using StorageLibrary.Common;

namespace StorageLibTests
{

	//[TestClass]
	public class ContainersTests : BaseTests
	{
		[TestMethod]
		public async Task GetContainerBlobs()
		{
			string containerName = "claritagx27";
			List<BlobItemWrapper> blobs = await Container.ListBlobsAsync(ACCOUNT, KEY, containerName, string.Empty);

			Assert.IsTrue(blobs.Count > 0, "No blobs in container");

		}

		//[TestMethod]
		//public async Task CreatePublicContainer()
		//{
		//	string containerName = GetStringGuid();
		//	await Container.CreateAsync(ACCOUNT, KEY, containerName, true);
		//	CloudBlobContainer container = Container.CreateAsync( .Get(ACCOUNT, KEY, containerName);

		//	Assert.IsTrue(await container.ExistsAsync(), "Missing container");

		//	await container.FetchAttributesAsync();

		//	Assert.IsTrue(container.Properties.PublicAccess == BlobContainerPublicAccessType.Container, "Invalid access");
		//}

		//[TestMethod]
		//public async Task CreatePrivateContainer()
		//{
		//	string containerName = GetStringGuid();
		//	await Container.CreateAsync(ACCOUNT, KEY, containerName, false);
		//	CloudBlobContainer container = Container.Get(ACCOUNT, KEY, containerName);

		//	Assert.IsTrue(await container.ExistsAsync(), "Missing container");

		//	await container.FetchAttributesAsync();

		//	Assert.IsTrue(container.Properties.PublicAccess == BlobContainerPublicAccessType.Off, "Invalid access");
		//}

		[TestMethod]
		public async Task DeleteContainers()
		{
			foreach (CloudBlobContainerWrapper container in await Container.ListContainersAsync(ACCOUNT, KEY))
				await Container.DeleteAsync(ACCOUNT, KEY, container.Name);
		}
	}
}
