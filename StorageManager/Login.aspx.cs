using System;
using System.Web;

using Microsoft.WindowsAzure.Storage.Blob;
using StorageHelper;
using StorageManager.Helpers;

namespace StorageManager
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				txtAccount.Focus();
				lblError.Text = string.Empty;
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(txtAccount.Text) || string.IsNullOrEmpty(txtKey.Text))
					return;

				CheckSecurity();

				HttpCookie account = new HttpCookie(SiteHelper.ACCOUNT, txtAccount.Text);
				HttpCookie key = new HttpCookie(SiteHelper.KEY, txtKey.Text);

				Response.Cookies.Add(account);
				Response.Cookies.Add(key);

				Response.Redirect("Default.aspx");
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
		}

		void CheckSecurity()
		{
			CloudBlobClient client = Client.GetBlobClient(txtAccount.Text, txtKey.Text);
			client.GetServiceProperties();
		}
	}
}
