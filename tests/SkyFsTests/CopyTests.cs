namespace SkyFsTests;

[TestClass]
public class CopyTests : TestBase
{
	[TestMethod]
	public async Task LocalFileIntoCloudFolderKeepsItsName()
	{
		string file = LocalFile("photo.jpg", "pixels");

		int code = await Commands.CopyAsync(file, "az://box/images/", recursive: false, force: false, move: false);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("pixels", Azure.Read("box", "images/photo.jpg"));
	}

	[TestMethod]
	public async Task LocalFileToExactCloudName()
	{
		string file = LocalFile("photo.jpg", "pixels");

		await Commands.CopyAsync(file, "az://box/renamed.jpg", false, false, false);

		Assert.AreEqual("pixels", Azure.Read("box", "renamed.jpg"));
	}

	[TestMethod]
	public async Task CloudFolderWithoutSlashIsAFolderTarget()
	{
		Azure.WithBlob("box", "images/old.jpg", "x");
		string file = LocalFile("photo.jpg", "pixels");

		await Commands.CopyAsync(file, "az://box/images", false, false, false);

		Assert.AreEqual("pixels", Azure.Read("box", "images/photo.jpg"));
	}

	[TestMethod]
	public async Task CloudFileToLocalFolder()
	{
		Azure.WithBlob("box", "docs/readme.md", "hello");
		Directory.CreateDirectory(Local("down"));

		int code = await Commands.CopyAsync("az://box/docs/readme.md", Local("down"), false, false, false);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("hello", File.ReadAllText(Local("down/readme.md")));
		Assert.IsEmpty(Directory.GetFiles(Local("down"), "*.tmp"), "the temp file of the write must be gone");
	}

	[TestMethod]
	public async Task BetweenCloudProviders()
	{
		Azure.WithBlob("box", "a.txt", "from azure");

		int code = await Commands.CopyAsync("az://box/a.txt", "s3://bucket/copies/", false, false, false);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("from azure", Aws.Read("bucket", "copies/a.txt"));
		Assert.IsTrue(Azure.Has("box", "a.txt"), "copy must keep the source");
	}

	[TestMethod]
	public async Task RefusesToOverwriteCloudFileWithoutForce()
	{
		Aws.WithBlob("bucket", "a.txt", "old");
		string file = LocalFile("a.txt", "new");

		int code = await Commands.CopyAsync(file, "s3://bucket/a.txt", false, force: false, move: false);

		Assert.AreEqual(1, code);
		Assert.AreEqual("old", Aws.Read("bucket", "a.txt"));
		StringAssert.Contains(Error.ToString(), "--force");
	}

	[TestMethod]
	public async Task OverwritesCloudFileWithForce()
	{
		Aws.WithBlob("bucket", "a.txt", "old");
		string file = LocalFile("a.txt", "new");

		int code = await Commands.CopyAsync(file, "s3://bucket/a.txt", false, force: true, move: false);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("new", Aws.Read("bucket", "a.txt"));
	}

	[TestMethod]
	public async Task RefusesToOverwriteLocalFileWithoutForce()
	{
		Azure.WithBlob("box", "a.txt", "new");
		string target = LocalFile("a.txt", "old");

		int code = await Commands.CopyAsync("az://box/a.txt", target, false, force: false, move: false);

		Assert.AreEqual(1, code);
		Assert.AreEqual("old", File.ReadAllText(target));
	}

	[TestMethod]
	public async Task OverwritesLocalFileWithForce()
	{
		Azure.WithBlob("box", "a.txt", "new");
		string target = LocalFile("a.txt", "old");

		int code = await Commands.CopyAsync("az://box/a.txt", target, false, force: true, move: false);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("new", File.ReadAllText(target));
	}

	[TestMethod]
	public async Task FolderNeedsRecursive()
	{
		LocalFile("src/a.txt", "a");

		int code = await Commands.CopyAsync(Local("src"), "az://box/dst/", recursive: false, false, false);

		Assert.AreEqual(1, code);
		StringAssert.Contains(Error.ToString(), "--recursive");
		Assert.IsEmpty(Azure.Names("box"));
	}

	[TestMethod]
	public async Task RecursiveLocalToCloudCopiesTheContents()
	{
		LocalFile("src/a.txt", "a");
		LocalFile("src/sub/b.txt", "b");
		LocalFile("src/sub/deeper/c.txt", "c");

		int code = await Commands.CopyAsync(Local("src"), "az://box/dst", recursive: true, false, false);

		Assert.AreEqual(0, code, Error.ToString());
		CollectionAssert.AreEquivalent(new[] { "dst/a.txt", "dst/sub/b.txt", "dst/sub/deeper/c.txt" }, Azure.Names("box").ToArray());
		Assert.AreEqual("c", Azure.Read("box", "dst/sub/deeper/c.txt"));
	}

	[TestMethod]
	public async Task RecursiveCloudToLocalCopiesTheContents()
	{
		Aws.WithBlob("bucket", "root.txt", "r").WithBlob("bucket", "x/y/z.txt", "z");

		int code = await Commands.CopyAsync("s3://bucket", Local("out"), recursive: true, false, false);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("r", File.ReadAllText(Local("out/root.txt")));
		Assert.AreEqual("z", File.ReadAllText(Local("out/x/y/z.txt")));
	}

	[TestMethod]
	public async Task RecursiveCopyKeepsGoingAfterAConflict()
	{
		Azure.WithBlob("box", "src/a.txt", "a").WithBlob("box", "src/b.txt", "b");
		Aws.WithBlob("bucket", "a.txt", "old");

		int code = await Commands.CopyAsync("az://box/src/", "s3://bucket/", recursive: true, false, false);

		Assert.AreEqual(1, code);
		Assert.AreEqual("old", Aws.Read("bucket", "a.txt"));
		Assert.AreEqual("b", Aws.Read("bucket", "b.txt"));
		StringAssert.Contains(Output.ToString(), "1 copied, 1 failed");
	}

	[TestMethod]
	public async Task SameSourceAndDestinationIsRefused()
	{
		Azure.WithBlob("box", "a.txt", "a");

		int code = await Commands.CopyAsync("az://box/a.txt", "az://box/a.txt", false, force: true, move: true);

		Assert.AreEqual(1, code);
		Assert.AreEqual("a", Azure.Read("box", "a.txt"));
	}

	[TestMethod]
	public async Task MissingSourceFails()
	{
		int code = await Commands.CopyAsync("az://box/nope.txt", Local("x"), false, false, false);

		Assert.AreEqual(1, code);
		StringAssert.Contains(Error.ToString(), "does not exist");
	}

	[TestMethod]
	public async Task UnconfiguredProviderFailsWithHelp()
	{
		int code = await Commands.CopyAsync("gs://bucket/a.txt", Local("x"), false, false, false);

		Assert.AreEqual(1, code);
		StringAssert.Contains(Error.ToString(), "GCP_CREDENTIALS_FILE");
	}

	[TestMethod]
	public async Task CloudLocationNeedsAContainer()
	{
		string file = LocalFile("a.txt", "a");

		int code = await Commands.CopyAsync(file, "az://", false, false, false);

		Assert.AreEqual(1, code);
		StringAssert.Contains(Error.ToString(), "container is required");
	}
}
