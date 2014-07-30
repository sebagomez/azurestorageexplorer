using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StorageManager.Helpers;

namespace StorageManager.Controls
{
	public partial class UC_Tables : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			try
			{
				string name = txtName.Text.Trim();
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				if (string.IsNullOrEmpty(name))
					return;

				StorageHelper.Table.Create(Request.Cookies[SiteHelper.ACCOUNT].Value, Request.Cookies[SiteHelper.KEY].Value, name);
				lblMessage.Text = string.Format("Table '{0}' has been created.", name);
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
			finally
			{
				ReloadTables();
			}
		}

		private void ReloadTables()
		{
			DataList1.DataBind();
		}

		public void ItemCommand(object source, DataListCommandEventArgs args)
		{
			try
			{
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });
				if (args.CommandName == "TableClick")
					Response.Redirect("default.aspx?type=table&table=" + args.CommandArgument.ToString());
				else if (args.CommandName == "DeleteTable")
					DeleteTable(args.CommandArgument.ToString());
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
		}

		private void DeleteTable(string tableName)
		{
			StorageHelper.Table.Delete(Request.Cookies[SiteHelper.ACCOUNT].Value, 
										Request.Cookies[SiteHelper.KEY].Value, 
										tableName);
			Response.Redirect("default.aspx?type=table");
		}
	}
}