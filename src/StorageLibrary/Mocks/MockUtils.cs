using System;

namespace StorageLibrary.Mocks
{
	public class MockUtils
	{
		public static readonly string FAKE_URL = "https://this.is.fake";

		static Random s_rand = new Random();

		public static long NewRandomSize { get => s_rand.NextInt64(512, 5 * 1024 * 1024); } // between 512 bytes and 5 Megs
	}
}