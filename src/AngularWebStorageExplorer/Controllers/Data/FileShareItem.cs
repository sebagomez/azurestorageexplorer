using System;

namespace AngularWebStorageExplorer.Controllers.Data
{
	public class FileShareItem : IEquatable<FileShareItem>, IComparable<FileShareItem>
	{
		public string Name { get; set; }
		public string Parent { get; set; }
		public string ParentUrl { get; set; }
		public string Url { get; set; }
		public bool IsDirectory { get; set; }

		public int CompareTo(FileShareItem other)
		{
			if (other == null)
				return -1;

			if (IsDirectory && !other.IsDirectory)
				return -1;

			if (!IsDirectory && other.IsDirectory)
				return 1;

			return Url.CompareTo(other.Url);
		}

		public bool Equals(FileShareItem other)
		{
			if (other == null)
				return false;

			return Url == other.Url;
		}
	}
}
