using Microsoft.AspNetCore.Components;
using StorageLibrary;

namespace wasm.Utils
{
	public class Util
	{
		const long MAX_MEGS = 100;
		public const long MAX_UPLOAD_SIZE = 1024 * 1024 * MAX_MEGS;

		
		public static async Task<StorageFactory> GetStorageFactory()
		{
			string? mock = Environment.GetEnvironmentVariable("MOCK");
			if (!(mock is null) && mock.ToLower() == bool.TrueString.ToLower())
				return new StorageFactory();

			Credentials cred = await Credentials.LoadCredentials();

			return new StorageFactory(cred.Account, cred.Key, cred.Endpoint);
		}
	}
}
