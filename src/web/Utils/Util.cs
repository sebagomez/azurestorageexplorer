using StorageLibrary;

namespace web.Utils
{
	public class Util
	{
		const long MAX_MEGS = 10;
		public const long MAX_UPLOAD_SIZE = 1024 * 1024 * MAX_MEGS;

		public static StorageFactory GetStorageFactory(Credentials cred)
		{
			string? mock = Environment.GetEnvironmentVariable("MOCK");
			if (!(mock is null) && mock.ToLower() == bool.TrueString.ToLower())
				return new StorageFactory();

			return new StorageFactory(cred.Account, cred.Key, cred.Endpoint);
		}
	}
}
