using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using StorageLibrary.Common;

namespace StorageLibrary.Interfaces
{
	public interface IFile
	{
		Task<List<FileShareWrapper>> ListFileSharesAsync();
		Task<List<FileShareItemWrapper>> ListFilesAndDirsAsync(string share, string folder = null);
		Task<string> GetFileAsync(string share, string file, string folder = null);
		Task DeleteFileAsync(string share, string file, string folder = null);
		Task CreateSubDirectory(string share, string folder, string subDir);
		Task CreateFileAsync(string share, string fileName, Stream fileContent, string folder = null);
		Task CreateFileShareAsync(string share, string accessTier);
	}
}