using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using StorageLibrary.Common;

namespace StorageLibrary.Interfaces
{
	public interface IContainer
	{
		Task<List<CloudBlobContainerWrapper>> ListContainersAsync();
		Task<List<BlobItemWrapper>> ListBlobsAsync(string containerName, string path);
		Task DeleteAsync(string containerName);
		Task CreateAsync(string containerName, bool publicAccess);
		Task DeleteBlobAsync(string containerName, string blobName);
		Task CreateBlobAsync(string containerName, string blobName, Stream fileContent);
		Task<string> GetBlobAsync(string containerName, string blobName);
		/// <summary>
		/// Gets a pre-signed URL the browser can upload a single blob to directly,
		/// bypassing the server. Returns null when the provider or the current
		/// credentials cannot sign one, in which case the caller must upload through
		/// <see cref="CreateBlobAsync"/> instead.
		/// </summary>
		Task<string> GetBlobUploadUrlAsync(string containerName, string blobName, TimeSpan validFor);
	}
}
