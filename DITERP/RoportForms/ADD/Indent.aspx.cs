using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_Indent : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string reportType = "";
    string rrreportType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        string type = Request.QueryString[1];
        string from = Request.QueryString[2];
        string to = Request.QueryString[3];
        GenerateReport(type, from, to);
    }


    #region GenerateReport
    private void GenerateReport(string type, string from, string to)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            DataTable dtfinal = new DataTable();

            dtfinal = CommonClasses.Execute(" SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,IN_PRO_CODE,DM_NAME ,CASE when (IN_STATUS=2) then  'Cancel' when (IN_AUTHORIZE=1 AND IN_AUTHORIZE1=1) then  'Authorize'  when (IN_AUTHORIZE=1 AND ISNULL(IN_AUTHORIZE1,0)=0) then 'Ready For Authorize'   WHEN IN_APPROVE=1 then 'Approve'    WHEN IN_APPROVE=2 then 'Not Approve' ELSE 'Pending' END   AS IN_APPROVE ,IN_SUPP_NAME,UM_NAME,IN_AUTHORIZE , ISNULL((select SUM(IND_AMT)  from INDENT_DETAIL where IND_INM_CODE=INM_CODE),0) AS IND_AMT,ISNULL(IN_USEDIN,0) AS IN_USEDIN, ISNULL(IN_REASON,'') AS IN_REASON  FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER WHERE IN_USER=UM_CODE    AND  INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE and IN_CM_CODE='" + Session["CompanyCode"] + "' AND IN_DATE between '" + Convert.ToDateTime(from).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(to).ToString("dd/MMM/yyyy") + "'  AND IN_STATUS='" + type + "'   ");




            ReportDocument rptname = null;
            rptname = new ReportDocument();


            rptname.Load(Server.MapPath("~/Reports/rptIndentReport.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptIndentReport.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtfinal);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtTitle", " From  ");
            CrystalReportViewer1.ReportSource = rptname;





        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "GenerateReport", Ex.Message);

        }
    }
    #endregion


    protected void btnCancel_Click(object sender, EventArgs e)
    {



        Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
    }
}