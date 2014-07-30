using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StorageManager.Helpers;

namespace StorageManager
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.Cookies[SiteHelper.ACCOUNT] == null || Request.Cookies[SiteHelper.KEY] == null)
				Response.Redirect("login.aspx");

			if (!IsPostBack)
			{
				string mode = Request.QueryString["type"];
				TraceManager.TraceInformation(string.Format("Setting {0} mode", mode));

				if (mode == "blob")
					SetBlobMode();
				else if (mode == "table")
					SetTableMode();
				else if (mode == "queue")
					SetQueueMode();
				else
					SetBlobMode(); // default

			}
		}

		private void SetQueueMode()
		{
			UC_Containers1.Visible = false;
			UC_Blob1.Visible = false;
			UC_Tables1.Visible = false;
			UC_Entities1.Visible = false;
			UC_Queues1.Visible = true;
			UC_Messages1.Visible = true;
		}

		private void SetTableMode()
		{
			UC_Containers1.Visible = false;
			UC_Blob1.Visible = false;
			UC_Tables1.Visible = true;
			UC_Entities1.Visible = true;
			UC_Queues1.Visible = false;
			UC_Messages1.Visible = false;
		}

		private void SetBlobMode()
		{
			UC_Containers1.Visible = true;
			UC_Blob1.Visible = true;
			UC_Tables1.Visible = false;
			UC_Entities1.Visible = false;
			UC_Queues1.Visible = false;
			UC_Messages1.Visible = false;
		}
	}
}
