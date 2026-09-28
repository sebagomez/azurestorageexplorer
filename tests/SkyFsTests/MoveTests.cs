namespace SkyFsTests;

[TestClass]
public class MoveTests : TestBase
{
	[TestMethod]
	public async Task MoveBetweenCloudsDeletesTheSource()
	{
		Azure.WithBlob("box", "a.txt", "a");

		int code = await Commands.CopyAsync("az://box/a.txt", "s3://bucket/", false, false, move: true);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("a", Aws.Read("bucket", "a.txt"));
		Assert.IsFalse(Azure.Has("box", "a.txt"));
	}

	[TestMethod]
	public async Task FailedMoveKeepsTheSource()
	{
		Azure.WithBlob("box", "a.txt", "new");
		Aws.WithBlob("bucket", "a.txt", "old");

		int code = await Commands.CopyAsync("az://box/a.txt", "s3://bucket/a.txt", false, force: false, move: true);

		Assert.AreEqual(1, code);
		Assert.IsTrue(Azure.Has("box", "a.txt"));
		Assert.AreEqual("old", Aws.Read("bucket", "a.txt"));
	}

	[TestMethod]
	public async Task MoveLocalFolderToCloudRemovesTheFolder()
	{
		LocalFile("src/a.txt", "a");
		LocalFile("src/sub/b.txt", "b");

		int code = await Commands.CopyAsync(Local("src"), "az://box/moved/", recursive: true, false, move: true);

		Assert.AreEqual(0, code, Error.ToString());
		Assert.AreEqual("b", Azure.Read("box", "moved/sub/b.txt"));
		Assert.IsFalse(Directory.Exists(Local("src")));
	}

	[TestMethod]
	public async Task MoveLocalFolderKeepsFilesThatFailed()
	{
		LocalFile("src/a.txt", "a");
		LocalFile("src/b.txt", "b");
		Azure.WithBlob("box", "b.txt", "taken");

		int code = await Commands.CopyAsync(Local("src"), "az://box/", recursive: true, false, move: true);

		Assert.AreEqual(1, code);
		Assert.IsFalse(File.Exists(Local("src/a.txt")));
		Assert.IsTrue(File.Exists(Local("src/b.txt")));
	}
}
