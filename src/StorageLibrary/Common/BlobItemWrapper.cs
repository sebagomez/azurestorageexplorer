﻿using System;
using System.Web;

namespace StorageLibrary.Common
{
	public class BlobItemWrapper : IEquatable<BlobItemWrapper>, IComparable<BlobItemWrapper>
	{
		Uri m_internalUri;
		public string Name { get => HttpUtility.UrlDecode(m_internalUri.Segments[m_internalUri.Segments.Length - 1]); }
		public string Path { get => m_internalUri.LocalPath.Substring(Container.Length + 1, m_internalUri.LocalPath.Length - Container.Length - Name.Length - 1); }
		public string Container { get => m_internalUri.Segments[1]; }
		public string FullName { get => $"{Path}{Name}"; }
		public bool IsFile { get => !m_internalUri.Segments[m_internalUri.Segments.Length - 1].EndsWith("/"); }
		public string Url
		{
			get { return m_internalUri.OriginalString; }
			private set { m_internalUri = new Uri(value); }
		}

		public long Size { get; private set; }

		public decimal SizeInKBs { get => (decimal)Size / 1024; }

		public decimal SizeInMBs { get => (decimal)Size / 1024 / 1024; }

		public BlobItemWrapper(string url) : this(url, 0) { }

		public BlobItemWrapper(string url, long? size)
		{
			Url = url;
			Size = size.HasValue ? size.Value : 0;
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
