using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StorageManager.Helpers;
using StorageHelper;
using System.IO;

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
                    lblMessage.Text = string.Format("Container '{0}' is empty.", container);
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
					string newUrl = string.Format("<a href='{0}'>{0}</a>", url);

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