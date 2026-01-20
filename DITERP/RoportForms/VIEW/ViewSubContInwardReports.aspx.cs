using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RoportForms_VIEW_ViewSubContInwardReports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT MAX(IWM_NO) AS  IWM_NO FROM INWARD_MASTER where IWM_TYPE='OUTCUSTINV' AND IWM_CM_CODE='" + Session["CompanyCode"] + "'  AND ES_DELETE=0");
            txtToNo.Text = dt.Rows[0]["IWM_NO"].ToString();
            txtFrom.Text = "0";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        string Type = "OUTCUSTINV";
        string PrintType = "Mult";
        //string inv_code = e.CommandArgument.ToString();
        string inv_code = "0";
        //string type = "Mult";
        string FromNo = txtFrom.Text;
        string toNo = txtToNo.Text;
        Response.Redirect("~/RoportForms/ADD/RptInspectionGIN_IWIM.aspx?inv_code=" + FromNo + "&type=" + toNo + "&Type123=" + Type + "&PType=" + PrintType, false);
    }
}
