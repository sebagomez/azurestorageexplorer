using System;

namespace StorageLibrary.Common
{
	public class FileShareItemWrapper : IEquatable<FileShareItemWrapper>, IComparable<FileShareItemWrapper>
	{
		Uri m_internalUri;
		public string Name { get; private set; }
		public string Path { get; private set; }
		public string FileShare { get; private set; }

		/// <summary>
		/// The item's name relative to its share, exactly as the provider reported it.
		/// </summary>
		public string FullName { get; private set; }
		public string Url
		{
			get { return m_internalUri.OriginalString; }
			private set { m_internalUri = new Uri(value); }
		}
		public bool IsFile { get; set; }
		public long? Size {get;set;}
		public decimal SizeInKBs { get => (decimal)Size / 1024; }
		public decimal SizeInMBs { get => (decimal)Size / 1024 / 1024; }

		/// <summary>
		/// The URL is kept only for display and equality; it is never parsed. See
		/// <see cref="BlobItemWrapper"/> for why the caller supplies the identity instead.
		/// </summary>
		public FileShareItemWrapper(string url, string fileShare, string fullName, bool isFile, long? size)
		{
			Url = url;
			FileShare = fileShare;
			FullName = fullName;
			IsFile = isFile;
			Size = size.HasValue ? size.Value : 0;

			(Path, Name) = StorageItemName.Split(fullName);
		}

		public int CompareTo(FileShareItemWrapper other)
		{
			if (other == null)
				return -1;

			if (IsFile && !other.IsFile)
				return -1;

			if (!IsFile && other.IsFile)
				return 1;

			return Url.CompareTo(other.Url);
		}

		public bool Equals(FileShareItemWrapper other)
		{
			if (other == null)
				return false;

			return Url == other.Url;
		}
	}
}
