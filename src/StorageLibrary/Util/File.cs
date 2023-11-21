using System;
using System.IO;

namespace StorageLibrary.Util
{
	public class File
	{
		static string TempFolder { get; } = Path.GetTempPath();

		public static string GetTempFileName()
		{
			return $"{TempFolder}/ASE-{Guid.NewGuid()}.tmp";
		}
	}
}
