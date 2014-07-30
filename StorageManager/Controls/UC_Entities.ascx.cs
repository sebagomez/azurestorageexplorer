using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StorageManager.Helpers;
using System.Drawing;
using System.Data;
using System.IO;
using System.Xml;
using StorageHelper;

namespace StorageManager.Controls
{
	public partial class UC_Entities : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				btnExecute.Enabled = Request.Params["table"] != null;
				btnInsert.Enabled = Request.Params["table"] != null;

				if (Request.Params["table"] != null)
				{
					string tableName = Request.Params["table"];
					btnInsert.Text = string.Format("Insert on '{0}'", tableName);
					btnExecute.Text = string.Format("Execute query on '{0}'", tableName);
				}
			}

		}

		void grdEntities_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				string originalData = e.Row.Cells[3].Text;
				try
				{
					string decodedText = HttpUtility.HtmlDecode(originalData);
					e.Row.Cells[3].Text = decodedText;
				}
				catch (Exception)
				{
					e.Row.Cells[3].Text = originalData;
				}
			}
		}

		protected void btnExecute_Click(object sender, EventArgs e)
		{
			try
			{
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				grdEntities.DataSource = null;
				grdEntities.DataBind();

				//IEnumerable<TableEntity> list = StorageHelper.Table.Query(Request.Cookies[SiteHelper.ACCOUNT].Value,
				//                                        Request.Cookies[SiteHelper.KEY].Value,
				//                                        Request.Params["table"],
				//                                        txtQuery.Text);
				
				//grdEntities.RowDataBound += new GridViewRowEventHandler(grdEntities_RowDataBound);

				DSEntities ds = TableHelper.Query(Request.Cookies[SiteHelper.ACCOUNT].Value,
												Request.Cookies[SiteHelper.KEY].Value,
												Request.Params["table"],
												txtQuery.Text);

				//grdEntities.DataSource = list;
				grdEntities.DataSource = ds;
				grdEntities.DataMember = "TableEntity";
				grdEntities.DataBind();
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
		}

		protected void btnInsert_Click(object sender, EventArgs e)
		{
			try
			{
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				StorageHelper.Table.Insert(Request.Cookies[SiteHelper.ACCOUNT].Value,
										Request.Cookies[SiteHelper.KEY].Value,
										Request.Params["table"],
										txtQuery.Text);

				lblMessage.Text = "Entity saved";
				txtQuery.Text = string.Empty;

				grdEntities.DataSource = null;
				grdEntities.DataBind();
				
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
		}

		protected void grdEntities_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				UCHelper.CleanUpLabels(new Label[] { lblMessage, lblError });

				if (e.CommandName == "DeleteEntity")
				{
					int index = int.Parse(e.CommandArgument.ToString());
					string partitionKey = grdEntities.DataKeys[index][0].ToString();
					string rowKey = grdEntities.DataKeys[index][1].ToString();

					StorageHelper.Table.DeleteEntity(Request.Cookies[SiteHelper.ACCOUNT].Value,
											Request.Cookies[SiteHelper.KEY].Value,
											Request.Params["table"],
											partitionKey,
											rowKey);

					lblMessage.Text = "Entity deleted";
					
					grdEntities.DataSource = null;
					grdEntities.DataBind();
				}
			}
			catch (Exception ex)
			{
				lblError.Text = ex.Message;
			}
		}
	}
}