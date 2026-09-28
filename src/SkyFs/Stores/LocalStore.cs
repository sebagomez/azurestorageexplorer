namespace SkyFs.Stores;

/// <summary>The local file system. A key is a path, relative to the current folder or absolute.</summary>
internal sealed class LocalStore : IStore
{
	public string Identity => "local";

	public string Display(string key) => key;

	public string NormalizeKey(string key) => Path.GetFullPath(key);

	public string Combine(string folderKey, string relativeName) =>
		Path.Combine(folderKey, relativeName.Replace('/', Path.DirectorySeparatorChar));

	public string FileName(string key) => Path.GetFileName(Path.TrimEndingDirectorySeparator(key));

	public Task<ItemKind> GetKindAsync(string key) => Task.FromResult(
		File.Exists(key) ? ItemKind.File :
		Directory.Exists(key) ? ItemKind.Folder :
		ItemKind.None);

	public Task<long> GetFileSizeAsync(string key) => Task.FromResult(new FileInfo(key).Length);

	public Task<bool> IsFolderTargetAsync(string key) => Task.FromResult(
		Path.EndsInDirectorySeparator(key) || key.EndsWith('/') || Directory.Exists(key));

	public Task<List<StoreEntry>> ListAsync(string folderKey, bool recursive)
	{
		DirectoryInfo root = new(folderKey);
		List<StoreEntry> entries = [];

		if (recursive)
		{
			foreach (FileInfo file in root.EnumerateFiles("*", SearchOption.AllDirectories))
				entries.Add(new StoreEntry(Relative(root, file.FullName), false, file.Length));
		}
		else
		{
			foreach (DirectoryInfo dir in root.EnumerateDirectories())
				entries.Add(new StoreEntry(dir.Name + "/", true, 0));
			foreach (FileInfo file in root.EnumerateFiles())
				entries.Add(new StoreEntry(file.Name, false, file.Length));
		}

		entries.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
		return Task.FromResult(entries);
	}

	public Task<Stream> OpenReadAsync(string key) =>
		Task.FromResult<Stream>(new FileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, FileOptions.Asynchronous));

	public async Task WriteAsync(string key, Stream content, bool overwrite)
	{
		if (!overwrite && File.Exists(key))
			throw new TargetExistsException(key);

		string fullPath = Path.GetFullPath(key);
		Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

		// Write next to the target, then rename. A failed download never leaves a
		// partial file under the real name.
		string tempPath = $"{fullPath}.skyfs-{Guid.NewGuid():N}.tmp";
		try
		{
			await using (FileStream target = new(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, FileOptions.Asynchronous))
				await content.CopyToAsync(target);

			try
			{
				File.Move(tempPath, fullPath, overwrite);
			}
			catch (IOException) when (!overwrite && File.Exists(fullPath))
			{
				throw new TargetExistsException(key);
			}
		}
		finally
		{
			File.Delete(tempPath);
		}
	}

	public Task DeleteFileAsync(string key)
	{
		File.Delete(key);
		return Task.CompletedTask;
	}

	public Task RemoveEmptyFoldersAsync(string folderKey)
	{
		RemoveIfEmpty(new DirectoryInfo(folderKey));
		return Task.CompletedTask;
	}

	static void RemoveIfEmpty(DirectoryInfo dir)
	{
		if (!dir.Exists)
			return;

		foreach (DirectoryInfo child in dir.EnumerateDirectories())
			RemoveIfEmpty(child);

		if (!dir.EnumerateFileSystemInfos().Any())
			dir.Delete();
	}

	static string Relative(DirectoryInfo root, string fullName) =>
		Path.GetRelativePath(root.FullName, fullName).Replace(Path.DirectorySeparatorChar, '/');
}
