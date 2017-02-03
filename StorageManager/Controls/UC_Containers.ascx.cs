using System;
using System.Web.UI.WebControls;

using StorageHelper;
using StorageManager.Helpers;

namespace StorageManager.Controls
{
	public partial class UC_Containers : System.Web.UI.UserControl
    {

        public void ItemCommand(object source, DataListCommandEventArgs args)
        {
            try
            {
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

                if (args.CommandName == "ContainerClick")
                    Response.Redirect("Default.aspx?container=" + args.CommandArgument.ToString());
                else if (args.CommandName == "DeleteContainer")
                    DeleteContainer(args.CommandArgument.ToString());
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void DeleteContainer(string containerName)
        {
            Container.Delete(Request.Cookies[SiteHelper.ACCOUNT].Value, Request.Cookies[SiteHelper.KEY].Value, containerName);
            ReloadContainers();
            lblMessage.Text = $"Container '{containerName}' has been deleted.";
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });
                string name = txtName.Text.Trim();

                if (string.IsNullOrEmpty(name))
                    return;

                Container.Create(Request.Cookies[SiteHelper.ACCOUNT].Value, 
								Request.Cookies[SiteHelper.KEY].Value, 
								name,
								chkPublic.Checked);

                lblMessage.Text = $"Container '{name}' has been created.";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                ReloadContainers();
            }

        }

        private void ReloadContainers()
        {
            DataList1.DataBind();
        }
    }
}