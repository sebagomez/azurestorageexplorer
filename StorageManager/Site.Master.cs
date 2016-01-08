using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using StorageManager.Helpers;
using StorageManager.Resources;

namespace StorageManager
{
	public partial class Site : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				btnExit.Visible = Menu1.Visible = Request.Cookies[SiteHelper.ACCOUNT] != null;

			if (!IsPostBack)
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				AssemblyName name = assembly.GetName();
				Version ver = name.Version;

				Page.Title = string.Format(Messages.TitleVersion, string.Format("{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build), ver.Revision);
				lblVersion.Text = ver.ToString();
			}
		}

		protected void btnExit_Click(object sender, EventArgs e)
		{
			if (Request.Cookies[SiteHelper.ACCOUNT] != null || Request.Cookies[SiteHelper.KEY] != null)
			{
				HttpCookie account = new HttpCookie(SiteHelper.ACCOUNT);
				account.Expires = DateTime.Now.AddHours(-1d);
				HttpCookie key = new HttpCookie(SiteHelper.KEY);
				key.Expires = DateTime.Now.AddHours(-1d);
				Response.Cookies.Add(account);
				Response.Cookies.Add(key);
			}

			Response.Redirect("Default.aspx");
		}
	}
}
