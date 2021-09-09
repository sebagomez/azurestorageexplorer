using System;

namespace StorageLibrary.Common
{
	public class FileShareItemWrapper : IEquatable<FileShareItemWrapper>, IComparable<FileShareItemWrapper>
	{
		public string Name { get; set; }
		public string Parent { get; set; }
		public string ParentUrl { get; set; }
		public string Url { get; set; }
		public bool IsDirectory { get; set; }

		public int CompareTo(FileShareItemWrapper other)
		{
			if (other == null)
				return -1;

			if (IsDirectory && !other.IsDirectory)
				return -1;

			if (!IsDirectory && other.IsDirectory)
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
