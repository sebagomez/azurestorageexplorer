namespace SkyFsTests;

[TestClass]
public class RemoveTests : TestBase
{
	[TestMethod]
	public async Task DeletesACloudFile()
	{
		Azure.WithBlob("box", "a.txt", "a").WithBlob("box", "b.txt", "b");

		int code = await Commands.RemoveAsync("az://box/a.txt", recursive: false, yes: false);

		Assert.AreEqual(0, code, Error.ToString());
		CollectionAssert.AreEqual(new[] { "b.txt" }, Azure.Names("box").ToArray());
	}

	[TestMethod]
	public async Task FolderNeedsRecursive()
	{
		Azure.WithBlob("box", "dir/a.txt", "a");

		int code = await Commands.RemoveAsync("az://box/dir/", recursive: false, yes: true);

		Assert.AreEqual(1, code);
		Assert.IsTrue(Azure.Has("box", "dir/a.txt"));
	}

	[TestMethod]
	public async Task RecursiveDeleteAsksFirst()
	{
		Azure.WithBlob("box", "dir/a.txt", "a");
		ConfirmAnswer = false;

		int code = await Commands.RemoveAsync("az://box/dir", recursive: true, yes: false);

		Assert.AreEqual(1, code);
		Assert.IsTrue(Azure.Has("box", "dir/a.txt"));
	}

	[TestMethod]
	public async Task RecursiveDeleteWithYesDeletesOnlyThatFolder()
	{
		Aws.WithBlob("bucket", "dir/a.txt", "a").WithBlob("bucket", "dir/sub/b.txt", "b").WithBlob("bucket", "dirt.txt", "keep");
		ConfirmAnswer = false;

		int code = await Commands.RemoveAsync("s3://bucket/dir", recursive: true, yes: true);

		Assert.AreEqual(0, code, Error.ToString());
		CollectionAssert.AreEqual(new[] { "dirt.txt" }, Aws.Names("bucket").ToArray());
	}

	[TestMethod]
	public async Task RecursiveLocalDeleteRemovesTheFolder()
	{
		LocalFile("gone/a.txt", "a");
		LocalFile("gone/sub/b.txt", "b");

		int code = await Commands.RemoveAsync(Local("gone"), recursive: true, yes: true);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.IsFalse(Directory.Exists(Local("gone")));
	}
}

[TestClass]
public class ListTests : TestBase
{
	[TestMethod]
	public async Task NoLocationListsContainersOfEveryProvider()
	{
		int code = await Commands.ListAsync(null, recursive: false);

		Assert.AreEqual(0, code, Error.ToString());
		CollectionAssert.AreEqual(new[] { "az://box/", "s3://bucket/" }, Lines());
	}

	[TestMethod]
	public async Task ListsOneLevel()
	{
		Azure.WithBlob("box", "a.txt", "12345").WithBlob("box", "dir/b.txt", "b");

		await Commands.ListAsync("az://box", recursive: false);

		CollectionAssert.AreEqual(new[] { "5 B  a.txt", "<DIR>  dir/" }, Lines());
	}

	[TestMethod]
	public async Task ListsRecursively()
	{
		Azure.WithBlob("box", "dir/a.txt", "a").WithBlob("box", "dir/sub/b.txt", "b").WithBlob("box", "other.txt", "o");

		await Commands.ListAsync("az://box/dir/", recursive: true);

		CollectionAssert.AreEqual(new[] { "1 B  a.txt", "1 B  sub/b.txt" }, Lines());
	}

	[TestMethod]
	public async Task ListsASingleFile()
	{
		Azure.WithBlob("box", "a.txt", "12345").WithBlob("box", "a.txt.bak", "1");

		await Commands.ListAsync("az://box/a.txt", recursive: false);

		CollectionAssert.AreEqual(new[] { "5 B  a.txt" }, Lines());
	}

	[TestMethod]
	public async Task ListsALocalFolder()
	{
		LocalFile("l/a.txt", "a");
		LocalFile("l/sub/b.txt", "b");

		await Commands.ListAsync(Local("l"), recursive: false);

		CollectionAssert.AreEqual(new[] { "1 B  a.txt", "<DIR>  sub/" }, Lines());
	}

	[TestMethod]
	[DataRow(0L, "0 B")]
	[DataRow(1023L, "1023 B")]
	[DataRow(1536L, "1.5 KB")]
	[DataRow(5L * 1024 * 1024 * 1024, "5.0 GB")]
	public void FormatsSizes(long bytes, string expected) =>
		Assert.AreEqual(expected, SkyFs.Commands.FormatSize(bytes));

	string[] Lines() =>
		Output.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).ToArray();
}
