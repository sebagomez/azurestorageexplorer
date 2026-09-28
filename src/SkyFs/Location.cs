using StorageLibrary;

namespace SkyFs;

/// <summary>
/// A place a command reads from or writes to: a local path, or a container plus a
/// path inside it on one cloud provider.
/// </summary>
internal sealed record Location(CloudProvider? Provider, string Container, string Path)
{
	internal static readonly IReadOnlyList<(string Scheme, CloudProvider Provider)> Schemes =
	[
		("az://", CloudProvider.Azure),
		("s3://", CloudProvider.AWS),
		("gs://", CloudProvider.GCP),
	];

	public bool IsCloud => Provider.HasValue;

	public static Location Parse(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			throw new SkyFsException("A location is required");

		foreach ((string scheme, CloudProvider provider) in Schemes)
		{
			if (!text.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
				continue;

			string rest = text[scheme.Length..];
			int slash = rest.IndexOf('/');
			return slash < 0
				? new Location(provider, rest, string.Empty)
				: new Location(provider, rest[..slash], rest[(slash + 1)..]);
		}

		return new Location(null, string.Empty, text);
	}

	public static string SchemeOf(CloudProvider provider) =>
		Schemes.First(s => s.Provider == provider).Scheme;
}
