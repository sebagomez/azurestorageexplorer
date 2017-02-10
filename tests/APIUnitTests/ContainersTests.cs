using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageHelper;

namespace APIUnitTests
{

	[TestClass]
	public class ContainersTests : BaseTests
	{
		[TestMethod]
		public void CreatePublicContainer()
		{
			string containerName = GetStringGuid();
			Container.Create(ACCOUNT, KEY, containerName, true);
			CloudBlobContainer container = Container.Get(ACCOUNT, KEY, containerName);

			Assert.IsTrue(container.Exists(), "Missing container");

			container.FetchAttributes();

			Assert.IsTrue(container.Properties.PublicAccess == BlobContainerPublicAccessType.Container, "Invalid access");
		}

		[TestMethod]
		public void CreatePrivateContainer()
		{
			string containerName = GetStringGuid();
			Container.Create(ACCOUNT, KEY, containerName, false);
			CloudBlobContainer container = Container.Get(ACCOUNT, KEY, containerName);

			Assert.IsTrue(container.Exists(), "Missing container");

			container.FetchAttributes();

			Assert.IsTrue(container.Properties.PublicAccess == BlobContainerPublicAccessType.Off, "Invalid access");
		}

		[TestMethod]
		public void DeleteContainers()
		{
			foreach (CloudBlobContainer container in Container.GetAll(ACCOUNT, KEY))
				container.Delete();
		}
	}
}
