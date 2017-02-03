using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using StorageManager.Helpers;

namespace StorageManager.Controls
{
	public partial class UC_Queues : UserControl
	{
		protected void btnCreate_Click(object sender, EventArgs e)
		{
			try
			{
				string name = txtName.Text.Trim();
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				if (string.IsNullOrEmpty(name))
					return;

				StorageHelper.Queue.Create(Request.Cookies[SiteHelper.ACCOUNT].Value, Request.Cookies[SiteHelper.KEY].Value, name);
				lblMessage.Text = $"Queue '{name}' has been created.";
				txtName.Text = string.Empty;
			}
			catch (StorageException sex)
			{
				lblError.Text = sex.RequestInformation.HttpStatusMessage;
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
			finally
			{
				ReloadQueues();
			}
		}

		public void ItemCommand(object source, DataListCommandEventArgs args)
		{
			try
			{
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				if (args.CommandName == "QueueClick")
					Response.Redirect("Default.aspx?type=queue&queue=" + args.CommandArgument.ToString());
				else if (args.CommandName == "DeleteQueue")
					DeleteQueue(args.CommandArgument.ToString());
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
		}

		private void DeleteQueue(string queueName)
		{
			try
			{
				QueueHelper.Delete(Request.Cookies[SiteHelper.ACCOUNT].Value,
									Request.Cookies[SiteHelper.KEY].Value,
									queueName);
			}
			finally
			{
				ReloadQueues();
			}
		}

		private void ReloadQueues()
		{
			DataList1.DataBind();
		}
	}
}