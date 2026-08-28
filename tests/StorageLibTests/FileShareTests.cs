using System;
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

			CompareFileShares(expected, shares);
		}

		[TestMethod]
		public async Task GetShareFiles()
		{
			string fileShareName = "one";
			List<string> expected = new List<string>
			{
				$"{MockUtils.FAKE_URL}/{fileShareName}/fromOne:1",
				$"{MockUtils.FAKE_URL}/{fileShareName}/fromOne:2",
				$"{MockUtils.FAKE_URL}/{fileShareName}/fromOne:3"
			};

			StorageFactory factory = new StorageFactory();
			List<FileShareItemWrapper> files = await factory.Files.ListFilesAndDirsAsync(fileShareName, string.Empty);

			Assert.HasCount(expected.Count, files, $"Different amount returned. {string.Join(",", files)}");
			for (int i = 0; i < expected.Count; i++)
				Assert.AreEqual(files[i].Url, expected[i], $"Different objecte returned. Expected '{expected[i]}' got '{files[i].Url}'");
		}

		/// <summary>
		/// Same round-trip guarantee as for blobs: Path and Name must reproduce the name the
		/// provider gave us. DownloadFile and DeleteFile pass Path as the directory, so a
		/// wrong split silently reads from the wrong folder.
		/// </summary>
		[TestMethod]
		public async Task FileNamesRoundTrip()
		{
			StorageFactory factory = new StorageFactory();
			List<FileShareItemWrapper> files = await factory.Files.ListFilesAndDirsAsync("brothers", "seba/");

			Assert.HasCount(3, files, $"Unexpected listing for 'seba/'. {string.Join(",", files)}");

			foreach (FileShareItemWrapper file in files)
			{
				Assert.AreEqual("brothers", file.FileShare, $"Wrong share for '{file.FullName}'");
				Assert.AreEqual(file.FullName, $"{file.Path}{file.Name}", $"Path+Name does not reproduce FullName for '{file.FullName}'");
				Assert.AreEqual("seba/", file.Path, $"Wrong path for '{file.FullName}'");
			}
		}

		[TestMethod]
		public async Task GetOneFiles()
		{
			string expected = "seba/juan";

			StorageFactory factory = new StorageFactory();
			string filePath = await factory.Files.GetFileAsync("brothers", "juan", "seba/");

			Assert.AreEqual(expected, filePath, $"Different path returned. Expected '{expected}' got '{filePath}'");
		}

		[TestMethod]
		public async Task GetWrongPath()
		{
			StorageFactory factory = new StorageFactory();
			try
			{
				string filePath = await factory.Files.GetFileAsync("brothers", "alfo", "seba/");
				Assert.Fail($"Returned file path not expected: {filePath}");
			}
			catch (Exception ex)
			{
				Assert.AreEqual("File 'seba/alfo' does not exist in Share 'brothers'", ex.Message, ex.Message);
			}
		}

		[TestMethod]
		public async Task CreateFileShare()
		{
			string fileShare = "four";
			List<FileShareWrapper> expected = new List<FileShareWrapper>
			{
				new FileShareWrapper { Name = "one"},
				new FileShareWrapper { Name =  "two"},
				new FileShareWrapper { Name =  "three"},
				new FileShareWrapper { Name =  "empty"},
				new FileShareWrapper { Name =  "with-folder"},
				new FileShareWrapper { Name =  "with-many-folders"},
				new FileShareWrapper { Name =  "brothers"},
				new FileShareWrapper { Name =  fileShare}
			};

			StorageFactory factory = new StorageFactory();
			await factory.Files.CreateFileShareAsync(fileShare, "optimized");

			List<FileShareWrapper> shares = await factory.Files.ListFileSharesAsync();

			CompareFileShares(expected, shares);
		}

		[TestMethod]
		public async Task DeleteFileShare()
		{
			string fileShare = "one";
			List<FileShareWrapper> expected = new List<FileShareWrapper>
			{
				new FileShareWrapper { Name =  "two"},
				new FileShareWrapper { Name =  "three"},
				new FileShareWrapper { Name =  "empty"},
				new FileShareWrapper { Name =  "with-folder"},
				new FileShareWrapper { Name =  "with-many-folders"},
				new FileShareWrapper { Name =  "brothers"},
				new FileShareWrapper { Name =  "four"}
			};

			StorageFactory factory = new StorageFactory();
			await factory.Files.DeleteFileShareAsync(fileShare);

			List<FileShareWrapper> shares = await factory.Files.ListFileSharesAsync();

			CompareFileShares(expected, shares);
		}

		private void CompareFileShares(List<FileShareWrapper> expected, List<FileShareWrapper> returned)
		{
			Assert.HasCount(expected.Count, returned, $"Different amount returned. {string.Join(",", returned)}");
			for (int i = 0; i < expected.Count; i++)
				Assert.AreEqual(returned[i].Name, expected[i].Name, $"Different objecte returned. Expected '{expected[i].Name}' got '{returned[i].Name}'");
		}
	}
}
