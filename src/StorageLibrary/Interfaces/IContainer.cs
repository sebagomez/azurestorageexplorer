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
        Task<string> GetBlob(string containerName, string blobName);
    }
}
