using System;

namespace StorageLibrary.Common
{
	public class BlobItemWrapper : IEquatable<BlobItemWrapper>, IComparable<BlobItemWrapper>
	{
		Uri m_internalUri;
		public string Name { get; private set; }
		public string Path { get; private set; }
		public string Container { get; private set; }

		/// <summary>
		/// The blob's name relative to its container, exactly as the provider reported it.
		/// This is what every read/delete call expects back.
		/// </summary>
		public string FullName { get; private set; }
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

		/// <summary>
		/// The URL is kept only for display and equality; it is never parsed. Container,
		/// name and kind come from the caller, which already has them - deriving them from
		/// the URL cannot be done reliably, since the account name sits in the host for
		/// Azure and in the path for Azurite, and names may be percent-encoded.
		/// </summary>
		public BlobItemWrapper(string url, string container, string fullName, bool isFile, long size, CloudProvider provider)
		{
			Url = url;
			Container = container;
			FullName = fullName;
			IsFile = isFile;
			Size = size;
			Provider = provider;

			(Path, Name) = StorageItemName.Split(fullName);
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
