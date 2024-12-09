using System;
using System.Web;

namespace StorageLibrary.Common
{
	public class BlobItemWrapper : IEquatable<BlobItemWrapper>, IComparable<BlobItemWrapper>
	{
		Uri m_internalUri;
		bool m_isAzurite = false;
		public string Name { get; private set; }
		public string Path { get; private set; }
		public string Container { get; private set; }
		public string FullName { get => $"{Path}{Name}"; }
		public bool IsFile { get; private set; }
		public string Url
		{
			get { return m_internalUri.OriginalString; }
			private set { m_internalUri = new Uri(value); }
		}

		public long Size { get; private set; }

		public decimal SizeInKBs { get => (decimal)Size / 1024; }

		public decimal SizeInMBs { get => (decimal)Size / 1024 / 1024; }

		public BlobItemWrapper(string url) : this(url, 0) { }

		public BlobItemWrapper(string url, long size, bool fromAzurite = false)
		{
			Url = url;
			Size = size;
			m_isAzurite = fromAzurite;
			IsFile = !m_internalUri.Segments[m_internalUri.Segments.Length - 1].EndsWith(System.IO.Path.AltDirectorySeparatorChar);
			Container = m_isAzurite ? m_internalUri.Segments[2] : m_internalUri.Segments[1];
			Name = HttpUtility.UrlDecode(m_internalUri.Segments[m_internalUri.Segments.Length - 1]);
			int containerPos = m_internalUri.LocalPath.IndexOf(Container) + Container.Length;
			Path = m_internalUri.LocalPath.Substring(containerPos, (m_internalUri.LocalPath.Length) - (containerPos) - Name.Length);
		}

		public int CompareTo(BlobItemWrapper other)
		{
			if (other == null)
				return -1;

			return Url.CompareTo(other.Url);
		}

		public bool Equals(BlobItemWrapper other)
		{
			if (other == null)
				return false;

			return Url == other.Url;
		}

		public override string ToString()
		{
			return Url;
		}
	}
}
