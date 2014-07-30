using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
			System.Diagnostics.Trace.TraceWarning(string.Format("Warning:{0}", message));
		}

		public static void TraceError(string message)
		{
			System.Diagnostics.Trace.TraceError(string.Format("Error:{0}", message));
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
