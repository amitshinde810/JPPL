using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;

public partial class ADD_SuppAuditPlan : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    public static string exe = "0";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ViewState["strUser"] = strUser;
            if (!IsPostBack)
            {
                ViewState["strUser"] = "0";
            }
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string Cond = Request.QueryString[3].ToString();

            GenerateReport(Title, From, To, Cond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataTable dtSuppAuditPlan = new DataTable();
            dtSuppAuditPlan = CommonClasses.Execute("select 'A' AS [Plan_Actual],P_CODE,P_NAME  ,isnull(sum([1]),0) as [1],isnull(sum([2]),0) as [2],isnull(sum([3]),0) as [3],isnull(sum([4]),0) as [4],isnull(sum([5]),0) as [5],isnull(sum([6]),0) as [6],isnull(sum([7]),0) as [7],isnull(sum([8]),0) as [8],isnull(sum([9]),0) as [9],isnull(sum([10]),0) as [10],isnull(sum([11]),0) as [11],isnull(sum([12]),0) as [12] into #Temp from( select isnull(datepart(mm,SAP_AUDIT_DATE),0) AS [Month],SAP_DOC_DATE,P.P_CODE,P.P_NAME  from SUPPLIER_AUDIT_PLAN SAP inner join PARTY_MASTER P on P.P_CODE=SAP_P_CODE WHERE SAP.ES_DELETE=0 AND P.ES_DELETE=0 and SAP_AUDIT_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') as sourcetable pivot (count(SAP_DOC_DATE) for [Month] in([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) as pvttable GROUP BY P_CODE,P_NAME select 'B' AS [Plan_Actual],P_CODE,P_NAME  ,isnull(sum([1]),0) as [1],isnull(sum([2]),0) as [2],isnull(sum([3]),0) as [3],isnull(sum([4]),0) as [4],isnull(sum([5]),0) as [5],isnull(sum([6]),0) as [6],isnull(sum([7]),0) as [7],isnull(sum([8]),0) as [8],isnull(sum([9]),0) as [9],isnull(sum([10]),0) as [10],isnull(sum([11]),0) as [11],isnull(sum([12]),0) as [12] into #Temp1 from(select isnull(datepart(mm,SAP_COMPLETED_DATE),0) AS [Month],SAP_COMPLETED_DATE,P.P_CODE,P.P_NAME  from SUPPLIER_AUDIT_PLAN SAP inner join PARTY_MASTER P on P.P_CODE=SAP_P_CODE WHERE SAP.ES_DELETE=0 AND P.ES_DELETE=0 and SAP_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') as sourcetable pivot (count(SAP_COMPLETED_DATE) for [Month] in([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) as pvttable GROUP BY P_CODE,P_NAME select * from #Temp  union select * from #Temp1  order by P_NAME drop table #Temp drop table #Temp1");
            //dtSuppAuditPlan = CommonClasses.Execute("select 'A' AS [Plan_Actual],P_CODE,P_NAME  ,isnull(sum([1]),0) as [Jan],isnull(sum([2]),0) as [Feb],isnull(sum([3]),0) as [Mar],isnull(sum([4]),0) as [Apr],isnull(sum([5]),0) as [May],isnull(sum([6]),0) as [Jun],isnull(sum([7]),0) as [Jul],isnull(sum([8]),0) as [Aug],isnull(sum([9]),0) as [Sep],isnull(sum([10]),0) as [Oct],isnull(sum([11]),0) as [Nov],isnull(sum([12]),0) as [Dec] into #Temp from( select isnull(datepart(mm,SAP_DOC_DATE),0) AS [Month],SAP_DOC_DATE,P.P_CODE,P.P_NAME  from SUPPLIER_AUDIT_PLAN SAP inner join PARTY_MASTER P on P.P_CODE=SAP_P_CODE WHERE SAP.ES_DELETE=0 AND P.ES_DELETE=0 and SAP_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') as sourcetable pivot (count(SAP_DOC_DATE) for [Month] in([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) as pvttable GROUP BY P_CODE,P_NAME select 'B' AS [Plan_Actual],P_CODE,P_NAME  ,isnull(sum([1]),0) as [Jan],isnull(sum([2]),0) as [Feb],isnull(sum([3]),0) as [Mar],isnull(sum([4]),0) as [Apr],isnull(sum([5]),0) as [May],isnull(sum([6]),0) as [Jun],isnull(sum([7]),0) as [Jul],isnull(sum([8]),0) as [Aug],isnull(sum([9]),0) as [Sep],isnull(sum([10]),0) as [Oct],isnull(sum([11]),0) as [Nov],isnull(sum([12]),0) as [Dec] into #Temp1 from(select isnull(datepart(mm,SAP_COMPLETED_DATE),0) AS [Month],SAP_COMPLETED_DATE,P.P_CODE,P.P_NAME  from SUPPLIER_AUDIT_PLAN SAP inner join PARTY_MASTER P on P.P_CODE=SAP_P_CODE WHERE SAP.ES_DELETE=0 AND P.ES_DELETE=0 and SAP_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') as sourcetable pivot (count(SAP_COMPLETED_DATE) for [Month] in([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) as pvttable GROUP BY P_CODE,P_NAME select * from #Temp  union select * from #Temp1  order by P_NAME drop table #Temp drop table #Temp1");
            if (dtSuppAuditPlan.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/SupplierAuditPlan.rpt"));      //SupplierAuditPlan SuppAuditPlanNew suppapl
                rptname.FileName = Server.MapPath("~/Reports/SupplierAuditPlan.rpt"); //SupplierAuditPlan
                rptname.Refresh();
                rptname.SetDataSource(dtSuppAuditPlan);
                rptname.SetParameterValue("txtPeriod", "Supplier Audit Plan Report");
                try
                {
                    rptname.SetParameterValue("Month", " FromDate " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " ToDate " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + " ");
                }
                catch (Exception Ex) { }
                CrystalReportViewer1.PrintMode = PrintMode.ActiveX;
                CrystalReportViewer1.ReportSource = rptname;
                CrystalReportViewer1.ID = Title.Replace(" ", "_");

                #region Using IFrame 
                string path = "";
                rptname.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath("~/UpLoadPath/report.pdf"));
                path = ("../../UpLoadPath/report.pdf");
                iframe1.Attributes["src"] = path;
                #endregion Using IFrame
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierAuditRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}


