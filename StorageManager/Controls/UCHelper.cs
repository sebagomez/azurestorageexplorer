using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace StorageManager.Controls
{
	public class UCHelper
	{
		public static void CleanUpLabels(Label[] labels)
		{
			foreach (Label label in labels)
				label.Text = string.Empty;
		}
	}
}
