using System;

namespace StorageLibrary.Common
{
	/// <summary>
	/// Splits the name a storage provider gives us into a parent path and a leaf, so the
	/// item wrappers never have to reconstruct identity by parsing a URL. URLs differ per
	/// provider and per endpoint style (the account name lives in the host on Azure but in
	/// the path on Azurite), while the name is always the same string we need to hand back
	/// when reading, downloading or deleting the item.
	/// </summary>
	internal static class StorageItemName
	{
		/// <summary>
		/// "a/b/c.txt" -> ("a/b/", "c.txt"), "a/b/" -> ("a/", "b/"), "c.txt" -> ("", "c.txt").
		/// Concatenating the two halves always reproduces <paramref name="fullName"/>.
		/// </summary>
		internal static (string Path, string Name) Split(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
				return (string.Empty, string.Empty);

			// Length-2 so a trailing slash stays attached to the leaf: blob prefixes are named
			// "folder/", and both the folder list and the prefix query depend on keeping it.
			int slash = fullName.LastIndexOf('/', Math.Max(fullName.Length - 2, 0));

			return slash < 0
				? (string.Empty, fullName)
				: (fullName.Substring(0, slash + 1), fullName.Substring(slash + 1));
		}
	}
}
