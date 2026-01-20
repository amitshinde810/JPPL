using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class ToolRoom_ADD_MonthlyPMPlanReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    public static string exe = "0";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
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
            string Cond = Request.QueryString[1].ToString();
            string From = Request.QueryString[2].ToString();
            //string To = Request.QueryString[3].ToString();

            GenerateReport(Title, Cond, From);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Cond, string From)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt2 = new DataTable();

            DataTable dtFilter = new DataTable();

            Query = "select T_NAME,I_CODENO,I_NAME,PM_MONTH,PM_YEAR,PM_TYPE,ISNULL([WEEK 1],0) as [WEEK 1],ISNULL([WEEK 2],0) as [WEEK 2],ISNULL([WEEK 3],0) as [WEEK 3],ISNULL([WEEK 4],0) as [WEEK 4] from (select isnull(T_CODE,0) as T_CODE,T_NAME,I_CODENO,I_NAME,LEFT(DATENAME(MONTH, PM_MONTH),3) AS PM_MONTH,year(PM_MONTH) as PM_YEAR,case when isnull(CONVERT(varchar,PM_WEEK ),0)='0' then 'WEEK 1' when CONVERT(varchar,PM_WEEK )='1' then 'WEEK 2' when CONVERT(varchar,PM_WEEK )='2' then 'WEEK 3' when CONVERT(varchar,PM_WEEK )='3' then 'WEEK 4' end as PM_WEEK ,case when PM_TYPE=0 then 'DIE' else 'CORE BOX' end as PM_TYPE from MP_PREVENTIVE_MAINTENANCE_MASTER INNER JOIN TOOL_MASTER ON MP_PREVENTIVE_MAINTENANCE_MASTER.PM_TOOL_NO = TOOL_MASTER.T_CODE AND MP_PREVENTIVE_MAINTENANCE_MASTER.PM_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE where " + Cond + " MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and T_CM_COMP_ID=1 and T_STATUS=1) DataTable PIVOT (MAX(T_CODE) FOR PM_WEEK IN ([WEEK 1],[WEEK 2],[WEEK 3],[WEEK 4])) PivotTable";
            dt2 = CommonClasses.Execute(Query);

            if (dt2.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptMonthlyPMPlan.rpt")); //foundaryRejYearly
                rptname.FileName = Server.MapPath("~/Reports/rptMonthlyPMPlan.rpt"); //foundaryRejYearly
                rptname.Refresh();
                rptname.SetDataSource(dt2);
                rptname.SetParameterValue("txtPeriod", "MONTHLY PREVENTIVE MAINTENANCE PLAN");// From" + From + "To " + To + "
                CrystalReportViewer1.ReportSource = rptname;
                CrystalReportViewer1.ID = Title.Replace(" ", "_");
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
            Response.Redirect("~/ToolRoom/VIEW/MonthlyPMPlanReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}


