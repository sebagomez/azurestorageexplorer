using System;

namespace StorageLibrary.Common
{
    public class BlobItemWrapper : IEquatable<BlobItemWrapper>, IComparable<BlobItemWrapper>
    {
        public string Name { get; set; }
        public string Url { get; set; }

		public bool IsFolder { get => Url.EndsWith("/"); }

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
