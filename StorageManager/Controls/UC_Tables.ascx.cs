using System;
using System.Web.UI.WebControls;
using StorageManager.Helpers;

namespace StorageManager.Controls
{
	public partial class UC_Tables : System.Web.UI.UserControl
	{
		protected void btnCreate_Click(object sender, EventArgs e)
		{
			try
			{
				string name = txtName.Text.Trim();
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				if (string.IsNullOrEmpty(name))
					return;

				StorageHelper.Table.Create(Request.Cookies[SiteHelper.ACCOUNT].Value, Request.Cookies[SiteHelper.KEY].Value, name);
				lblMessage.Text = $"Table '{name}' has been created.";
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
					Response.Redirect("Default.aspx?type=table&table=" + args.CommandArgument.ToString());
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
			Response.Redirect("Default.aspx?type=table");
		}
	}
}