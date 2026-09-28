namespace SkyFs.Stores;

internal enum ItemKind { None, File, Folder }

/// <summary>
/// One entry of a listing. <see cref="Name"/> is relative to the listed folder, uses '/'
/// as the separator, and ends with '/' for a folder.
/// </summary>
internal sealed record StoreEntry(string Name, bool IsFolder, long Size);

/// <summary>
/// The same operations over the local disk and over one cloud container, so copy, move,
/// and delete do not need to know where the bytes are. A key is a path inside the store.
/// </summary>
internal interface IStore
{
	/// <summary>Two keys point to the same item only when the identities are equal.</summary>
	string Identity { get; }

	string Display(string key);
	string NormalizeKey(string key);
	string Combine(string folderKey, string relativeName);
	string FileName(string key);

	Task<ItemKind> GetKindAsync(string key);

	Task<long> GetFileSizeAsync(string key);

	/// <summary>True when a single file copied to <paramref name="key"/> must go inside it.</summary>
	Task<bool> IsFolderTargetAsync(string key);

	/// <summary>Lists one level, or with <paramref name="recursive"/> all files below the folder.</summary>
	Task<List<StoreEntry>> ListAsync(string folderKey, bool recursive);

	Task<Stream> OpenReadAsync(string key);

	/// <summary>Throws <see cref="TargetExistsException"/> when the file exists and <paramref name="overwrite"/> is false.</summary>
	Task WriteAsync(string key, Stream content, bool overwrite);

	Task DeleteFileAsync(string key);

	/// <summary>Removes the folder and its subfolders when they hold no files. Cloud stores have no real folders.</summary>
	Task RemoveEmptyFoldersAsync(string folderKey);
}
