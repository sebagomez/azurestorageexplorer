using System;

namespace StorageManager.Helpers
{
	public class TraceManager
	{
		public static void TraceInformation(string message)
		{
			System.Diagnostics.Trace.TraceInformation(message);
		}

		public static void TraceWarning(string message)
		{
			System.Diagnostics.Trace.TraceWarning($"Warning:{message}");
		}

		public static void TraceError(string message)
		{
			System.Diagnostics.Trace.TraceError($"Error:{message}");
		}

		public static void TraceError(Exception ex)
		{
			TraceError(ex.Message);
			Exception inner = ex.InnerException;
			while (inner != null)
			{
				TraceError(inner);
				inner = inner.InnerException;
			}
		}

		public static void TraceError(string message, Exception ex)
		{
			TraceError(message);
			TraceError(ex);
		}


	}
}
