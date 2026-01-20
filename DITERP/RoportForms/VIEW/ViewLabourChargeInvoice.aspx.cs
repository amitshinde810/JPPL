using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class RoportForms_VIEW_ViewLabourChargeInvoice : System.Web.UI.Page
{
    #region Variables
    string Title = "";
    string Cond = "";
    string invoice_code = "";
    string reportType = "";
    string chkPrint1 = "";
    #endregion Variables

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        Title = Request.QueryString[0];
        Cond = Request.QueryString[1];
        invoice_code = Request.QueryString[3];
        reportType = Request.QueryString[4];
        chkPrint1 = Convert.ToInt32(Request.QueryString[2]).ToString();
    }
    #endregion #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM INVOICE_MASTER where INM_CODE='" + Request.QueryString[3].ToString() + "'");
            txtFrom.Text = "1";
            txtToNo.Text = dt.Rows[0]["INM_NO"].ToString();
            if (Request.QueryString[4].ToString() != "Mult")
            {
                txtFrom.Enabled = false;
                txtToNo.Enabled = false;
                txtFrom.Text = dt.Rows[0]["INM_NO"].ToString();
                // Response.Redirect("~/RoportForms/ADD/LabourChargeInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint4=" + chkPrintCopy4.Text + "&code=" + Code + "&type=" + type + "&Supp=" + chlSupp.Checked + "", false);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        //string inv_code = e.CommandArgument.ToString();
        string inv_code = "0";
        string type = "Mult";

        string FromNo = txtFrom.Text.Trim();
        string toNo = txtToNo.Text.Trim();
        //Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?inv_code=" + FromNo + "&type=" + toNo, false);
        Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + Cond + "&chkPrint1=" + chkPrint1 + "&code=" + FromNo + "&type=" + type + "&toNo=" + toNo + "&Supp=" + Request.QueryString[5].ToString() + "", false);
       // Response.Redirect("~/RoportForms/ADD/LabourChargeInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint4=" + chkPrintCopy4.Text + "&code=" + Code + "&type=" + type + "&Supp=" + chlSupp.Checked + "", false);
    }
}
