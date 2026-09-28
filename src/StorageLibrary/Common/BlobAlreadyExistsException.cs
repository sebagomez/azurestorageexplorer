using System;

namespace StorageLibrary.Common
{
	/// <summary>
	/// Thrown by <see cref="Interfaces.IContainer.CreateBlobAsync"/> when the blob exists
	/// and the caller did not ask to overwrite it. Each provider reports this condition
	/// with a different exception, so the library converts them all to this one.
	/// </summary>
	public class BlobAlreadyExistsException : InvalidOperationException
	{
		public string Container { get; }
		public string BlobName { get; }

		public BlobAlreadyExistsException(string container, string blobName, Exception inner = null)
		: base($"Blob '{blobName}' already exists in '{container}'", inner)
		{
			Container = container;
			BlobName = blobName;
		}
	}
}
