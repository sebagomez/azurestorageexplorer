using SkyFs;

using StorageLibrary;

namespace SkyFsTests;

/// <summary>Gives each test fake Azure and AWS accounts, a temp folder, and captured output.</summary>
public abstract class TestBase
{
	internal FakeContainer Azure = null!;
	internal FakeContainer Aws = null!;
	internal ProviderRegistry Providers = null!;
	internal StringWriter Output = null!;
	internal StringWriter Error = null!;
	internal string Temp = null!;
	internal bool ConfirmAnswer = true;
	internal Commands Commands = null!;

	[TestInitialize]
	public void Setup()
	{
		Azure = new FakeContainer(CloudProvider.Azure).WithContainer("box");
		Aws = new FakeContainer(CloudProvider.AWS).WithContainer("bucket");
		Providers = new ProviderRegistry(new Dictionary<CloudProvider, ProviderSetup>
		{
			[CloudProvider.Azure] = new("fake", () => Azure),
			[CloudProvider.AWS] = new("fake", () => Aws),
		});
		Output = new StringWriter();
		Error = new StringWriter();
		Temp = Directory.CreateTempSubdirectory("skyfs-tests-").FullName;
		Commands = new Commands(Providers, Output, Error, _ => ConfirmAnswer);
	}

	[TestCleanup]
	public void Cleanup()
	{
		Providers.Dispose();
		Directory.Delete(Temp, recursive: true);
	}

	internal string LocalFile(string relative, string content)
	{
		string path = Path.Combine(Temp, relative);
		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		File.WriteAllText(path, content);
		return path;
	}

	internal string Local(string relative) => Path.Combine(Temp, relative);
}
