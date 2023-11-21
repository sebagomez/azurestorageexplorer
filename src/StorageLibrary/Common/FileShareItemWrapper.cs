using System;
using System.Web;

namespace StorageLibrary.Common
{
	public class FileShareItemWrapper : IEquatable<FileShareItemWrapper>, IComparable<FileShareItemWrapper>
	{
		Uri m_internalUri;
		public string Name { get => HttpUtility.UrlDecode(m_internalUri.Segments[m_internalUri.Segments.Length - 1]); }
		public string Path { get => m_internalUri.LocalPath.Substring(FileShare.Length + 1, m_internalUri.LocalPath.Length - FileShare.Length - Name.Length - 1); }
		public string FileShare { get => m_internalUri.Segments[1]; }
		public string FullName { get => $"{Path}{Name}"; }
		public string Url
		{
			get { return m_internalUri.OriginalString; }
			private set { m_internalUri = new Uri(value); }
		}
		public bool IsFile { get; set; }
		public long? Size {get;set;}
		public decimal SizeInKBs { get => (decimal)Size / 1024; }
		public decimal SizeInMBs { get => (decimal)Size / 1024 / 1024; }

		public FileShareItemWrapper(string url, bool isFile, long? size)
		{
			Url = url;
			Size = size.HasValue ? size.Value : 0;
			IsFile = isFile;
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
