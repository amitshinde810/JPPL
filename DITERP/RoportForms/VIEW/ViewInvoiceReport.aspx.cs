using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_ViewInvoiceReport : System.Web.UI.Page
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

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            if ( Request.QueryString[5].ToString().ToUpper()=="FALSE")
            {
                dt = CommonClasses.Execute("SELECT MAX(INM_NO) AS  INM_NO FROM INVOICE_MASTER where INM_TYPE='TAXINV'  AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INM_SUPPLEMENTORY=0 AND   INM_IS_SUPPLIMENT=0 AND INVOICE_MASTER.ES_DELETE=0");

            }
            else
            {
                dt = CommonClasses.Execute("SELECT MAX(INM_NO) AS  INM_NO FROM INVOICE_MASTER where INM_TYPE='TAXINV'  AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND INM_SUPPLEMENTORY=1 AND   INM_IS_SUPPLIMENT=1 AND INVOICE_MASTER.ES_DELETE=0");

            }
            txtToNo.Text = dt.Rows[0]["INM_NO"].ToString();
            txtFrom.Text = "1";
        }
    }
    #endregion Page_Load

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
    }
    #endregion btnCancel_Click

 
    #region btnShow_Click
 
    protected void btnShow_Click(object sender, EventArgs e)
    {
        //string inv_code = e.CommandArgument.ToString();
        string inv_code = "0";
        string type = "Mult";
        string FromNo = txtFrom.Text.Trim();
        string toNo = txtToNo.Text.Trim();
        //Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?inv_code=" + FromNo + "&type=" + toNo, false);
        Response.Redirect("~/RoportForms/ADD/TaxInvoicePrint.aspx?Title=" + Title + "&Cond=" + Cond + "&chkPrint1=" + chkPrint1 + "&code=" + FromNo + "&type=" + type + "&toNo=" + toNo + "&Supp=" + Request.QueryString[1].ToString() + "", false);
    }
    #endregion btnShow_Click
}
