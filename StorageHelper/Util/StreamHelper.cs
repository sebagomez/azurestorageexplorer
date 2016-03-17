using System.IO;

namespace StorageHelper.Util
{
    public static class StreamHelper
	{
		public static void CopyTo(this Stream source, Stream target)
		{
			byte[] copyBuf = new byte[0x1000];
			int bytesRead = 0;
			int bufSize = copyBuf.Length;

			while ((bytesRead = source.Read(copyBuf, 0, bufSize)) > 0)
				target.Write(copyBuf, 0, bytesRead);
		}

		public static void CopyTo(this Stream source, string filePath)
		{
			using (FileStream writeStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
				source.CopyTo(writeStream);
		}
	}
}