using SkyFs.Stores;

using StorageLibrary;
using StorageLibrary.Interfaces;

namespace SkyFs;

/// <summary>A provider the user can reach, and how to build its client on first use.</summary>
internal sealed record ProviderSetup(string Description, Func<IContainer> Create);

/// <summary>
/// Holds one StorageLibrary client for each configured provider, so one command can use
/// more than one cloud. A client is built only when a command first needs it, so a bad
/// setting for one provider does not stop commands that use the others.
/// </summary>
internal sealed class ProviderRegistry : IDisposable
{
	static readonly LocalStore s_local = new();

	readonly IReadOnlyDictionary<CloudProvider, ProviderSetup> _setups;
	readonly Dictionary<CloudProvider, IContainer> _clients = [];

	public ProviderRegistry(IReadOnlyDictionary<CloudProvider, ProviderSetup> setups)
	{
		_setups = setups;
	}

	public IEnumerable<CloudProvider> Configured => Location.Schemes.Select(s => s.Provider).Where(_setups.ContainsKey);

	public string? Describe(CloudProvider provider) =>
		_setups.TryGetValue(provider, out ProviderSetup? setup) ? setup.Description : null;

	public IContainer Get(CloudProvider provider)
	{
		if (_clients.TryGetValue(provider, out IContainer? client))
			return client;

		if (!_setups.TryGetValue(provider, out ProviderSetup? setup))
			throw new SkyFsException($"{provider} is not configured. Set {Requirements(provider)}.");

		return _clients[provider] = setup.Create();
	}

	/// <summary>Gets the store for a location. A cloud location must name a container.</summary>
	public IStore Resolve(Location location)
	{
		if (!location.IsCloud)
			return s_local;

		CloudProvider provider = location.Provider!.Value;
		if (location.Container.Length == 0)
			throw new SkyFsException($"A container is required, for example {Location.SchemeOf(provider)}<container>/<path>");

		return new CloudStore(provider, Get(provider), location.Container);
	}

	public void Dispose()
	{
		foreach (IContainer client in _clients.Values)
			(client as IDisposable)?.Dispose();
	}

	public static string Requirements(CloudProvider provider) => provider switch
	{
		CloudProvider.Azure => "AZURE_STORAGE_CONNECTIONSTRING, or AZURE_STORAGE_ACCOUNT and AZURE_STORAGE_KEY",
		CloudProvider.AWS => "AWS_ACCESS_KEY, AWS_SECRET_KEY and AWS_REGION",
		CloudProvider.GCP => "GCP_CREDENTIALS_FILE",
		_ => throw new ArgumentOutOfRangeException(nameof(provider)),
	};

	/// <summary>
	/// Reads the same variables as the web app. The standard AWS and Google names are
	/// also accepted, because most machines with those SDKs already have them set.
	/// </summary>
	public static ProviderRegistry FromEnvironment(Func<string, string?> env)
	{
		string? Read(params string[] names) =>
			names.Select(env).FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));

		Dictionary<CloudProvider, ProviderSetup> setups = [];
		bool azurite = string.Equals(Read("AZURITE"), "true", StringComparison.OrdinalIgnoreCase);

		string? connectionString = Read("AZURE_STORAGE_CONNECTIONSTRING");
		string? account = Read("AZURE_STORAGE_ACCOUNT");
		string? key = Read("AZURE_STORAGE_KEY");
		if (connectionString is not null || (account is not null && key is not null))
		{
			StorageFactoryConfig config = new()
			{
				Provider = CloudProvider.Azure,
				AzureConnectionString = connectionString,
				AzureAccount = account,
				AzureKey = key,
				AzureEndpoint = Read("AZURE_STORAGE_ENDPOINT") ?? "core.windows.net",
				IsAzurite = azurite,
			};
			string how = connectionString is not null ? "connection string" : $"account {account}";
			setups[CloudProvider.Azure] = new(how, () => new StorageFactory(config).Containers);
		}

		string? awsKey = Read("AWS_ACCESS_KEY", "AWS_ACCESS_KEY_ID");
		string? awsSecret = Read("AWS_SECRET_KEY", "AWS_SECRET_ACCESS_KEY");
		string? awsRegion = Read("AWS_REGION", "AWS_DEFAULT_REGION");
		if (awsKey is not null && awsSecret is not null && awsRegion is not null)
		{
			StorageFactoryConfig config = new()
			{
				Provider = CloudProvider.AWS,
				AwsKey = awsKey,
				AwsSecret = awsSecret,
				AwsRegion = awsRegion,
			};
			setups[CloudProvider.AWS] = new($"region {awsRegion}", () => new StorageFactory(config).Containers);
		}

		string? gcpFile = Read("GCP_CREDENTIALS_FILE", "GOOGLE_APPLICATION_CREDENTIALS");
		if (gcpFile is not null)
		{
			StorageFactoryConfig config = new()
			{
				Provider = CloudProvider.GCP,
				GcpCredentialsFile = gcpFile,
			};
			setups[CloudProvider.GCP] = new($"credentials {gcpFile}", () => new StorageFactory(config).Containers);
		}

		return new ProviderRegistry(setups);
	}
}
