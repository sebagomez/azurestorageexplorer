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

		public CloudProvider Provider { get; private set; }

		public decimal SizeInKBs { get => (decimal)Size / 1024; }

		public decimal SizeInMBs { get => (decimal)Size / 1024 / 1024; }

		public BlobItemWrapper(string url) : this(url, 0, CloudProvider.Azure) { }

		public BlobItemWrapper(string url, long size, CloudProvider provider, bool fromAzurite = false)
		{
			Url = url;
			Size = size;
			Provider = provider;
			m_isAzurite = fromAzurite;
			IsFile = !m_internalUri.Segments[m_internalUri.Segments.Length - 1].EndsWith(System.IO.Path.AltDirectorySeparatorChar);
			Name = HttpUtility.UrlDecode(m_internalUri.Segments[m_internalUri.Segments.Length - 1]);

			switch (provider)
			{
				case CloudProvider.Azure:
					Container = m_isAzurite ? m_internalUri.Segments[2] : m_internalUri.Segments[1];
					int containerPos = m_internalUri.LocalPath.IndexOf(Container) + Container.Length;
					Path = m_internalUri.LocalPath.Substring(containerPos, (m_internalUri.LocalPath.Length) - (containerPos) - Name.Length);
					break;
				case CloudProvider.AWS:
					Container = m_internalUri.Host.Split('.')[0];
					Path = m_internalUri.LocalPath.Substring(1, m_internalUri.LocalPath.Length - 1 - Name.Length);
					break;
				case CloudProvider.GCP:
					Container = m_internalUri.Segments[1];
					int bucketPos = m_internalUri.LocalPath.IndexOf(Container) + Container.Length;
					Path = m_internalUri.LocalPath.Substring(bucketPos, (m_internalUri.LocalPath.Length) - (bucketPos) - Name.Length);
					break;
				default:
					throw new ApplicationException($"Invalid provider: {provider}");
			}

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
