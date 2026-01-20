using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_PrevmainPerformanceReport : System.Web.UI.Page
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
            string FromD = Request.QueryString[1].ToString();
            string ToD = Request.QueryString[2].ToString();
            string Cond = Request.QueryString[3].ToString();

            GenerateReport(Title, FromD, ToD, Cond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string FromD, string ToD, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt2 = new DataTable();

            DataTable dtFilter = new DataTable();

            //Query = "SELECT PM_TOOL_NO ,T_NAME,I_CODENO,I_NAME,COUNT(PM_CODE) AS [PLAN],(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=0) AS ACTUAL,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE<>0)  AS FAILUER, (SELECT COUNT(PM_CODE) FROM MP_PREVENTIVE_MAINTENANCE_MASTER WHERE MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_STATUS<>1 AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_TOOL_NO NOT IN (SELECT WPM_T_CODE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER WHERE WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0)) AS PENDING,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483644)  AS[FOUNDRY]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483648)  AS[TOOL ROOM]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483647)  AS [PURCHASE] ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483646)  AS [PLANNING]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND WPM_TOOL_RECEIVED_DATE BETWEEN  '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483645)  AS [MISC]  FROM MP_PREVENTIVE_MAINTENANCE_MASTER PM ,TOOL_MASTER,ITEM_MASTER where " + Cond + "  PM.ES_DELETE=0 AND T_CODE=PM.PM_TOOL_NO AND T_CM_COMP_ID='" + Session["CompanyID"] + "' AND T_I_CODE=I_CODE  AND PM_STATUS=0 GROUP BY PM_TOOL_NO,T_NAME,I_CODENO,I_NAME";

           // Query = "SELECT PM_TOOL_NO ,T_NAME,I_CODENO,I_NAME,COUNT(PM_CODE) AS [PLAN],(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND ISNULL(WPM_REASON_CODE,0)=0) AS ACTUAL,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE<>0) + (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' ) AS FAILUER, (SELECT COUNT(PM_CODE) FROM MP_PREVENTIVE_MAINTENANCE_MASTER WHERE MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_STATUS<>1 AND convert(date,PM_MONTH) BETWEEN'" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_TOOL_NO NOT IN (SELECT WPM_T_CODE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER WHERE WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0)) AS PENDING,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483644) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483644) AS[FOUNDRY]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483648) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483648)  AS[TOOL ROOM]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483647) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483647) AS [PURCHASE] ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483646)+  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483646)  AS [PLANNING]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483645) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483645) AS [MISC]  FROM MP_PREVENTIVE_MAINTENANCE_MASTER PM ,TOOL_MASTER,ITEM_MASTER where " + Cond + "  PM.ES_DELETE=0 AND T_CODE=PM.PM_TOOL_NO AND T_CM_COMP_ID= '" + Session["CompanyID"] + "' AND T_I_CODE=I_CODE GROUP BY PM_TOOL_NO,T_NAME,I_CODENO,I_NAME ";

            //added actul qty should be calculated when wpm status is close otherwise it not show in report
            Query = "SELECT PM_TOOL_NO ,T_NAME,I_CODENO,I_NAME,COUNT(PM_CODE) AS [PLAN],(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_STATUS=1 AND ISNULL(WPM_REASON_CODE,0)=0) AS ACTUAL,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE<>0) + (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' ) AS FAILUER, (SELECT COUNT(PM_CODE) FROM MP_PREVENTIVE_MAINTENANCE_MASTER WHERE MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_STATUS<>1 AND convert(date,PM_MONTH) BETWEEN'" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_TOOL_NO NOT IN (SELECT WPM_T_CODE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER WHERE WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0)) AS PENDING,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483644) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483644) AS[FOUNDRY]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483648) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483648)  AS[TOOL ROOM]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483647) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483647) AS [PURCHASE] ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483646)+  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483646)  AS [PLANNING]  ,(SELECT COUNT( WPM_CODE) FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=PM.PM_TOOL_NO AND ES_DELETE=0 AND CONVERT(DATE, WPM_TOOL_RECEIVED_DATE) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND WPM_REASON_CODE=-2147483645) +  (  SELECT COUNT(PM_CODE)  FROM MP_PREVENTIVE_MAINTENANCE_MASTER where  PM_STATUS=1 AND ES_DELETE=0  AND PM_TOOL_NO=PM.PM_TOOL_NO AND convert(date,PM_MONTH) BETWEEN '" + Convert.ToDateTime(FromD).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(ToD).ToString("dd/MMM/yyyy") + "' AND PM_REASON_CODE =-2147483645) AS [MISC]  FROM MP_PREVENTIVE_MAINTENANCE_MASTER PM ,TOOL_MASTER,ITEM_MASTER where " + Cond + "  PM.ES_DELETE=0 AND T_CODE=PM.PM_TOOL_NO AND T_CM_COMP_ID= '" + Session["CompanyID"] + "' AND T_I_CODE=I_CODE GROUP BY PM_TOOL_NO,T_NAME,I_CODENO,I_NAME ";
            dt2 = CommonClasses.Execute(Query);

            if (dt2.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptPreMainPerformanceReport.rpt")); //foundaryRejYearly
                rptname.FileName = Server.MapPath("~/Reports/rptPreMainPerformanceReport.rpt"); //foundaryRejYearly
                rptname.Refresh();
                rptname.SetDataSource(dt2);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "PREVENTIVE MAINTENANCE PERFORMANCE");
                string Fmonth = Convert.ToDateTime(FromD).ToString("MMM/yyyy");
                string Tmonth = Convert.ToDateTime(ToD).ToString("MMM/yyyy");
                rptname.SetParameterValue("txtMonth", "MONTH :" + Fmonth);
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
            Response.Redirect("~/ToolRoom/VIEW/ViewPrevmainPerformanceReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

