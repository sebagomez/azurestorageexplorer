using StorageLibrary;
using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace SkyFsTests;

/// <summary>
/// An in-memory IContainer that keeps the real bytes, lists by hierarchy the way Azure
/// does, and follows the library's overwrite contract.
/// </summary>
internal sealed class FakeContainer(CloudProvider provider) : IContainer
{
	readonly Dictionary<string, SortedDictionary<string, byte[]>> _containers = [];

	public FakeContainer WithContainer(string name)
	{
		_containers[name] = new(StringComparer.Ordinal);
		return this;
	}

	public FakeContainer WithBlob(string container, string name, string content)
	{
		_containers[container][name] = System.Text.Encoding.UTF8.GetBytes(content);
		return this;
	}

	public bool Has(string container, string name) => _containers[container].ContainsKey(name);

	public string Read(string container, string name) => System.Text.Encoding.UTF8.GetString(_containers[container][name]);

	public IReadOnlyCollection<string> Names(string container) => _containers[container].Keys;

	SortedDictionary<string, byte[]> Get(string container) =>
		_containers.TryGetValue(container, out var blobs) ? blobs : throw new InvalidOperationException($"Container '{container}' does not exist");

	public Task<List<CloudBlobContainerWrapper>> ListContainersAsync() =>
		Task.FromResult(_containers.Keys.Order().Select(n => new CloudBlobContainerWrapper { Name = n }).ToList());

	public Task<List<BlobItemWrapper>> ListBlobsAsync(string containerName, string path)
	{
		string prefix = path ?? string.Empty;
		List<BlobItemWrapper> results = [];
		HashSet<string> folders = [];

		foreach ((string name, byte[] content) in Get(containerName))
		{
			if (!name.StartsWith(prefix, StringComparison.Ordinal))
				continue;

			int slash = name.IndexOf('/', prefix.Length);
			if (slash < 0)
				results.Add(new BlobItemWrapper($"https://fake/{containerName}/{name}", containerName, name, true, content.Length, provider));
			else if (folders.Add(name[..(slash + 1)]))
				results.Add(new BlobItemWrapper($"https://fake/{containerName}/{name[..(slash + 1)]}", containerName, name[..(slash + 1)], false, 0, provider));
		}

		return Task.FromResult(results);
	}

	public async Task CreateBlobAsync(string containerName, string blobName, Stream fileContent, bool overwrite = false)
	{
		SortedDictionary<string, byte[]> blobs = Get(containerName);
		if (!overwrite && blobs.ContainsKey(blobName))
			throw new BlobAlreadyExistsException(containerName, blobName);

		using MemoryStream buffer = new();
		await fileContent.CopyToAsync(buffer);
		blobs[blobName] = buffer.ToArray();
	}

	public async Task<string> GetBlobAsync(string containerName, string blobName)
	{
		if (!Get(containerName).TryGetValue(blobName, out byte[]? content))
			throw new InvalidOperationException($"Blob '{blobName}' does not exist");

		string path = StorageLibrary.Util.File.GetTempFileName();
		await File.WriteAllBytesAsync(path, content);
		return path;
	}

	public Task DeleteBlobAsync(string containerName, string blobName)
	{
		if (!Get(containerName).Remove(blobName))
			throw new InvalidOperationException($"Blob '{blobName}' does not exist");
		return Task.CompletedTask;
	}

	public Task DeleteAsync(string containerName) => Task.FromResult(_containers.Remove(containerName));

	public Task CreateAsync(string containerName, bool publicAccess)
	{
		WithContainer(containerName);
		return Task.CompletedTask;
	}

	public Task<string> GetBlobUploadUrlAsync(string containerName, string blobName, TimeSpan validFor) =>
		Task.FromResult<string>(null!);
}
