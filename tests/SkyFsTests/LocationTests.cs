using SkyFs;

using StorageLibrary;

namespace SkyFsTests;

[TestClass]
public class LocationTests
{
	[TestMethod]
	[DataRow("az://box/a/b.txt", CloudProvider.Azure, "box", "a/b.txt")]
	[DataRow("s3://bucket/dir/", CloudProvider.AWS, "bucket", "dir/")]
	[DataRow("gs://bucket", CloudProvider.GCP, "bucket", "")]
	[DataRow("GS://bucket/", CloudProvider.GCP, "bucket", "")]
	[DataRow("az://", CloudProvider.Azure, "", "")]
	public void ParsesCloudLocations(string text, CloudProvider provider, string container, string path)
	{
		Location location = Location.Parse(text);

		Assert.AreEqual(provider, location.Provider);
		Assert.AreEqual(container, location.Container);
		Assert.AreEqual(path, location.Path);
	}

	[TestMethod]
	[DataRow("./file.txt")]
	[DataRow("/tmp/dir/")]
	[DataRow("relative/path")]
	[DataRow(@"C:\Users\me\file.txt")]
	public void AnythingElseIsLocal(string text)
	{
		Location location = Location.Parse(text);

		Assert.IsFalse(location.IsCloud);
		Assert.AreEqual(text, location.Path);
	}

	[TestMethod]
	public void RedactsSignatures()
	{
		string redacted = Commands.RedactSignatures("failed: https://a.blob.core.windows.net/c/b?sv=1&sig=abc%2F123&se=2");

		Assert.AreEqual("failed: https://a.blob.core.windows.net/c/b?sv=1&sig=REDACTED&se=2", redacted);
	}
}
