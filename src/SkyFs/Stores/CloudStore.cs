using StorageLibrary;
using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace SkyFs.Stores;

/// <summary>One container (Azure) or bucket (S3, GCS), reached through StorageLibrary.</summary>
internal sealed class CloudStore(CloudProvider provider, IContainer containers, string container) : IStore
{
	public string Identity => $"{Location.SchemeOf(provider)}{container}";

	public string Display(string key) => $"{Identity}/{key}";

	public string NormalizeKey(string key) => key;

	public string Combine(string folderKey, string relativeName) =>
		FolderPrefix(folderKey) + relativeName;

	public string FileName(string key)
	{
		string trimmed = key.TrimEnd('/');
		return trimmed[(trimmed.LastIndexOf('/') + 1)..];
	}

	public async Task<ItemKind> GetKindAsync(string key)
	{
		if (key.Length == 0)
			return ItemKind.Folder;

		if (key.EndsWith('/'))
			return (await containers.ListBlobsAsync(container, key)).Count > 0 ? ItemKind.Folder : ItemKind.None;

		// A prefix query for the exact name returns the file itself and the folder with
		// the same name, when they exist.
		List<BlobItemWrapper> items = await containers.ListBlobsAsync(container, key);
		if (items.Any(i => i.IsFile && i.FullName == key))
			return ItemKind.File;

		if (items.Any(i => !i.IsFile && i.FullName == key + "/"))
			return ItemKind.Folder;

		return ItemKind.None;
	}

	public async Task<long> GetFileSizeAsync(string key) =>
		(await containers.ListBlobsAsync(container, key)).FirstOrDefault(i => i.IsFile && i.FullName == key)?.Size ?? 0;

	public async Task<bool> IsFolderTargetAsync(string key) =>
		key.Length == 0 || key.EndsWith('/') || await GetKindAsync(key) == ItemKind.Folder;

	public async Task<List<StoreEntry>> ListAsync(string folderKey, bool recursive)
	{
		string root = FolderPrefix(folderKey);
		List<StoreEntry> entries = [];
		Queue<string> pending = new([root]);

		while (pending.Count > 0)
		{
			foreach (BlobItemWrapper item in await containers.ListBlobsAsync(container, pending.Dequeue()))
			{
				string relative = item.FullName[root.Length..];
				if (item.IsFile)
					entries.Add(new StoreEntry(relative, false, item.Size));
				else if (recursive)
					pending.Enqueue(item.FullName);
				else
					entries.Add(new StoreEntry(relative, true, 0));
			}
		}

		entries.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
		return entries;
	}

	public async Task<Stream> OpenReadAsync(string key)
	{
		// The library downloads to a temp file and does not delete it. DeleteOnClose
		// removes it when the copy closes the stream.
		string tempPath = await containers.GetBlobAsync(container, key);
		return new FileStream(tempPath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete,
			81920, FileOptions.DeleteOnClose | FileOptions.Asynchronous);
	}

	public async Task WriteAsync(string key, Stream content, bool overwrite)
	{
		try
		{
			await containers.CreateBlobAsync(container, key, content, overwrite);
		}
		catch (BlobAlreadyExistsException)
		{
			throw new TargetExistsException(Display(key));
		}
	}

	public Task DeleteFileAsync(string key) => containers.DeleteBlobAsync(container, key);

	public Task RemoveEmptyFoldersAsync(string folderKey) => Task.CompletedTask;

	static string FolderPrefix(string key) =>
		key.Length == 0 || key.EndsWith('/') ? key : key + "/";
}
