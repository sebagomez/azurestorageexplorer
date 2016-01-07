using System;
using StorageManager.Resources;

namespace StorageManager
{
	public partial class _04 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Format(Messages.Message404, Request.FilePath);
        }
    }
}
