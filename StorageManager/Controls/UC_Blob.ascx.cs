using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using StorageHelper;
using StorageManager.Helpers;

namespace StorageManager.Controls
{
	public partial class UC_Blob : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string container = Request.QueryString["container"];

                FileUpload.Visible = !string.IsNullOrEmpty(container);
                btnUpload.Visible = !string.IsNullOrEmpty(container);

                if (grdBlobs.Rows.Count == 0 && !string.IsNullOrEmpty(container))
                    lblMessage.Text = $"Container '{container}' is empty.";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

                if (e.CommandName == "DeleteBlob")
                    DeleteBlob(grdBlobs.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());
                else if (e.CommandName == "DownloadBlob")
                {
                    string blobUrl = grdBlobs.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString();
                    string fileName = blobUrl.Substring(blobUrl.LastIndexOf("/") + 1);
                    DownloadBlob(blobUrl,fileName);
                }
            }
            catch   (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void DownloadBlob(string blobUrl, string fileName)
        {
            string path = Container.GetBlob(Request.Cookies[SiteHelper.ACCOUNT].Value, Request.Cookies[SiteHelper.KEY].Value, blobUrl);

            FileInfo info = new FileInfo(path);

            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName );
            Response.AddHeader("Content-Length", info.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(path);
            Response.Flush();
        }

        public void DeleteBlob(string blobUrl)
        {
            BlobHelper.Delete(Request.Cookies[SiteHelper.ACCOUNT].Value, Request.Cookies[SiteHelper.KEY].Value, blobUrl);
            grdBlobs.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
			try
			{
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				if (FileUpload.HasFile)
				{
					string container = Request.QueryString["container"];
					Container.CreateBlob(Request.Cookies[SiteHelper.ACCOUNT].Value,
										Request.Cookies[SiteHelper.KEY].Value,
										container,
										FileUpload.FileName,
										FileUpload.FileContent);

					grdBlobs.DataBind();
				}
			}
			catch (StorageException sex)
			{
				lblError.Text = sex.RequestInformation.HttpStatusMessage;
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
        }

		protected void grdBlobs_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				string originalData = e.Row.Cells[2].Text;
				try
				{
					Uri url = new Uri(originalData);
					string newUrl = $"<a href='{url}'>{url}</a>";

					string decodedText = HttpUtility.HtmlDecode(newUrl);
					e.Row.Cells[2].Text = decodedText;
				}
				catch (Exception)
				{
					e.Row.Cells[2].Text = originalData;
				}
			}
		}
    }
}