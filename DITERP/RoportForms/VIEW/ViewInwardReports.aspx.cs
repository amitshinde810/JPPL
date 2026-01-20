using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_ViewInwardReports : System.Web.UI.Page
{
    static string Type = "";
    static string PrintType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT MAX(IWM_NO) AS  IWM_NO FROM INWARD_MASTER where IWM_TYPE='IWIM' AND IWM_CM_CODE='" + Session["CompanyCode"] + "'  AND ES_DELETE=0");
            txtToNo.Text = dt.Rows[0]["IWM_NO"].ToString();
            txtFrom.Text = "0";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        Type = "IWIM";
        PrintType = "Mult";
        //string inv_code = e.CommandArgument.ToString();
        string inv_code = "0";
        //string type = "Mult";
        string FromNo = txtFrom.Text;
        string toNo = txtToNo.Text;
        Response.Redirect("~/RoportForms/ADD/RptInspectionGIN_IWIM.aspx?inv_code=" + FromNo + "&type=" + toNo + " &Type123 = " + Type+"&PType=" + PrintType, false);
    }
}
