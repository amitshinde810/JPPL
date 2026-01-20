using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewDispToSubReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM INVOICE_MASTER where INM_CODE='" + Request.QueryString[0].ToString() + "'");
            txtToNo.Text = dt.Rows[0]["INM_NO"].ToString();
            if (Request.QueryString[1].ToString() != "Mult")
            {
                txtFrom.Enabled = false;
                txtToNo.Enabled = false;
                txtFrom.Text = dt.Rows[0]["INM_NO"].ToString();
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        string inv_code = "0";
        string type = "Mult";
        string FromNo = txtFrom.Text;
        string toNo = txtToNo.Text;
        Response.Redirect("~/RoportForms/ADD/DispToSubContPrint.aspx?inv_code=" + FromNo + "&type=" + toNo + "&pTYPE=" + ddlPrintOpt.SelectedValue, false);
    }

}
