using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_PerformanceSummaryReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ViewState["strUser"] = strUser;
            if (!IsPostBack)
            {
                CommonClasses.Execute("DELETE FROM IRN_DETAILS_REPORT where UM_CODE='" + Session["UserCode"].ToString() + "' ");
                ViewState["strUser"] = "0";
            }
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string L_Name = Request.QueryString[3].ToString();
            string Type = Request.QueryString[4].ToString();

            GenerateReport(Title, From, Todt, L_Name, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Performance Summary Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string L_Name, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            string cond = "";
            if (L_Name != "0")
            {
                cond = "  AND  LM_CODE ='" + L_Name + "' ";
            }
            if (Type == "0")
            {
                Query = "SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], LM_CODE, LM_NAME,'ONDATE' AS ONDATE FROM (SELECT LINE_MASTER.LM_CODE, LINE_MASTER.LM_NAME, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, datepart (dd,IE.IRN_DATE) AS IRN_DAY, datepart (MM,IE.IRN_DATE) AS MONTH,datepart (yyyy,IE.IRN_DATE) AS YEAR , ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_CODE = IRND_IRN_CODE WHERE IRN_ENTRY.ES_DELETE = 0 AND IRN_CM_ID = 1 AND IRN_TRANS_TYPE =1 and IRND_TYPE=1 AND IRND_I_CODE=ITEM_MASTER.I_CODE AND datepart (dd,IRN_DATE)=datepart (dd,IE.IRN_DATE) AND datepart (MM,IRN_DATE)=datepart (MM,IE.IRN_DATE)    AND datepart (YYYY,IRN_DATE)=datepart (YYYY,IE.IRN_DATE)),0) AS PROD  FROM LINE_CHANGE INNER JOIN  ITEM_MASTER ON LINE_CHANGE.LC_I_CODE = ITEM_MASTER.I_CODE INNER JOIN IRN_ENTRY AS IE INNER JOIN IRN_DETAIL AS ID ON IE.IRN_CODE = ID.IRND_IRN_CODE ON ITEM_MASTER.I_CODE = ID.IRND_I_CODE INNER JOIN LINE_MASTER ON LINE_CHANGE.LC_LM_CODE = LINE_MASTER.LM_CODE WHERE (LINE_CHANGE.LC_ACTIVE = 1) AND IE.IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' " + cond + " ) AS SOURCETABLE PIVOT (sum(PROD) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable GROUP BY  LM_CODE, LM_NAME  ";
            }
            else
            {
                Query = "SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], LM_CODE, LM_NAME,'ONDATE' AS ONDATE FROM (SELECT LINE_MASTER.LM_CODE, LINE_MASTER.LM_NAME, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, datepart (dd,IE.IRN_DATE) AS IRN_DAY, datepart (MM,IE.IRN_DATE) AS MONTH,datepart (yyyy,IE.IRN_DATE) AS YEAR , ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_CODE = IRND_IRN_CODE WHERE IRN_ENTRY.ES_DELETE = 0 AND IRN_CM_ID = 1 AND IRN_TRANS_TYPE =1 and IRND_TYPE=1 AND IRND_I_CODE=ITEM_MASTER.I_CODE AND datepart (dd,IRN_DATE)=datepart (dd,IE.IRN_DATE) AND datepart (MM,IRN_DATE)=datepart (MM,IE.IRN_DATE)    AND datepart (YYYY,IRN_DATE)=datepart (YYYY,IE.IRN_DATE)),0) AS PROD  FROM LINE_CHANGE INNER JOIN  ITEM_MASTER ON LINE_CHANGE.LC_I_CODE = ITEM_MASTER.I_CODE INNER JOIN IRN_ENTRY AS IE INNER JOIN IRN_DETAIL AS ID ON IE.IRN_CODE = ID.IRND_IRN_CODE ON ITEM_MASTER.I_CODE = ID.IRND_I_CODE INNER JOIN LINE_MASTER ON LINE_CHANGE.LC_LM_CODE = LINE_MASTER.LM_CODE WHERE (LINE_CHANGE.LC_ACTIVE = 1) AND IE.IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' " + cond + ") AS SOURCETABLE PIVOT (sum(PROD) FOR MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY  LM_CODE, LM_NAME ";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (Type == "0")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptIRNPerformanceDays.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIRNPerformanceDays.rpt");
                }
                else
                {
                    rptname.Load(Server.MapPath("~/Reports/rptIRNPerformanceYearly.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIRNPerformanceYearly.rpt");
                }
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                if (Type == "0")
                {
                    rptname.SetParameterValue("txtPeriod", "MONTHLY DAY WISE PRODUCTION EFFICIENCY From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
                else
                {
                    rptname.SetParameterValue("txtPeriod", "YEARLY MONTH WISE PRODUCTION EFFICIENCY From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
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
            CommonClasses.SendError("Performance Summary Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/ViewPerformanceSummaryReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Performance Summary Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
