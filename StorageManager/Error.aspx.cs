using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StorageManager.Helpers;

namespace StorageManager
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Server.GetLastError() != null && Server.GetLastError().GetBaseException() != null)
                lblError.Text = Server.GetLastError().GetBaseException().Message;
            else
                lblError.Text = string.Empty;

            Response.Cookies.Remove(SiteHelper.ACCOUNT);
            Response.Cookies.Remove(SiteHelper.KEY);

			
        }
    }
}
