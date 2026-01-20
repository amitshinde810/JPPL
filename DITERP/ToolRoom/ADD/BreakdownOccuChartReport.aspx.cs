using System;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_ADD_BreakdownOccuChartReport : System.Web.UI.Page
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
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string Cond = Request.QueryString[3].ToString();
            string Type = Request.QueryString[4].ToString();

            GenerateReport(Title, Cond, From, To, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Cond, string From, string To, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt2 = new DataTable();

            DataTable dtFilter = new DataTable();

            if (Type == "0")
            {
                // Query = "select BCODE,T_NAME,I_NAME,I_CODENO,isnull([1],0) as [1],isnull([2],0) as [2],isnull([3],0) as [3],isnull([4],0) as [4],isnull([5],0) as [5],isnull([6],0) as [6],isnull([7],0) as [7],isnull([8],0) as [8],isnull([9],0) as [9],isnull([10],0) as [10],isnull([11],0) as [11],isnull([12],0) as [12],isnull([13],0) as [13],isnull([14],0) as [14],isnull([15],0) as [15],isnull([16],0) as [16],isnull([17],0) as [17],isnull([18],0) as [18],isnull([19],0) as [19],isnull([20],0) as [20],isnull([21],0) as [21],isnull([22],0) as [22],isnull([23],0) as [23],isnull([24],0) as [24],isnull([25],0) as [25],isnull([26],0) as [26],isnull([27],0) as [27],isnull([28],0) as [28],isnull([29],0) as [29],isnull([30],0) as [30],isnull([31],0) as [31] from (select distinct B_CODE as BCODE,(case when B_CODE IS null then 0 else 1 end) as B_CODE,T_NAME,I_NAME,I_CODENO,datepart(dd,B_DATE) as B_DATE from BREAKDOWN_ENTRY,TOOL_MASTER,ITEM_MASTER where " + Cond + " B_TYPE=0 and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND TOOL_MASTER.T_CODE=BREAKDOWN_ENTRY.B_T_CODE and B_CM_CODE='" + Session["CompanyCode"].ToString() + "' and T_I_CODE=I_CODE and T_I_CODE=B_I_CODE) as src pivot (max(B_CODE) for B_DATE in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) as piv";
                Query = "select T_NAME,I_NAME,I_CODENO,isnull(SUM([1]),0) as [1],isnull(SUM([2]),0) as [2],isnull(SUM([3]),0) as [3],isnull(SUM([4]),0) as [4],isnull(SUM([5]),0) as [5],isnull(SUM([6]),0) as [6],isnull(SUM([7]),0) as [7],isnull(SUM([8]),0) as [8],isnull(SUM([9]),0) as [9],isnull(SUM([10]),0) as [10],isnull(SUM([11]),0) as [11],isnull(SUM([12]),0) as [12],isnull(SUM([13]),0) as [13],isnull(SUM([14]),0) as [14],isnull(SUM([15]),0) as [15],isnull(SUM([16]),0) as [16],isnull(SUM([17]),0) as [17],isnull(SUM([18]),0) as [18],isnull(SUM([19]),0) as [19],isnull(SUM([20]),0) as [20],isnull(SUM([21]),0) as [21],isnull(SUM([22]),0) as [22],isnull(SUM([23]),0) as [23],isnull(SUM([24]),0) as [24],isnull(SUM([25]),0) as [25],isnull(SUM([26]),0) as [26],isnull(SUM([27]),0) as [27],isnull(SUM([28]),0) as [28],isnull(SUM([29]),0) as [29],isnull(SUM([30]),0) as [30],isnull(SUM([31]),0) as [31] from (select distinct B_CODE as BCODE,(case when B_CODE IS null then 0 else 1 end) as B_CODE,T_NAME,I_NAME,I_CODENO,datepart(dd,B_DATE) as B_DATE from BREAKDOWN_ENTRY,TOOL_MASTER,ITEM_MASTER where " + Cond + "  B_TYPE=0 and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND TOOL_MASTER.T_CODE=BREAKDOWN_ENTRY.B_T_CODE and B_CM_CODE='" + Session["CompanyCode"].ToString() + "' and T_I_CODE=I_CODE and BREAKDOWN_ENTRY.ES_DELETE=0) as src pivot (SUM(B_CODE) for B_DATE in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])  ) as piv  GROUP BY  T_NAME,I_NAME,I_CODENO ORDER BY T_NAME";
            }
            else
            {
                Query = "select I_NAME,I_CODENO,ISNULL([1],0) as Jan,ISNULL([2],0) as Feb,ISNULL([3],0) as Mar,ISNULL([4],0) as Apr,ISNULL([5],0) as May,ISNULL([6],0) as Jun,ISNULL([7],0) as Jul,ISNULL([8],0) as Aug,ISNULL([9],0) as Sep,ISNULL([10],0) as Oct,ISNULL([11],0) as Nov,ISNULL([12],0) as Dec from (select COUNT(B_NO) as B_CODE,I_NAME,I_CODENO,MONTH(B_DATE) as B_DATE from BREAKDOWN_ENTRY,TOOL_MASTER,ITEM_MASTER where " + Cond + "  B_TYPE=0 and BREAKDOWN_ENTRY.ES_DELETE=0 and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND TOOL_MASTER.T_CODE=BREAKDOWN_ENTRY.B_T_CODE and B_CM_CODE='" + Session["CompanyCode"].ToString() + "' and T_I_CODE=I_CODE and T_I_CODE=B_I_CODE group by B_DATE,I_NAME,I_CODENO) as src pivot (SUM(B_CODE) for B_DATE in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) as piv";
            }
            dt2 = CommonClasses.Execute(Query);

            if (dt2.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (Type == "0")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptBreakdownOccuChart.rpt")); //foundaryRejYearly
                    rptname.FileName = Server.MapPath("~/Reports/rptBreakdownOccuChart.rpt"); //foundaryRejYearly
                    rptname.Refresh();
                    rptname.SetDataSource(dt2);
                }
                else
                {
                    rptname.Load(Server.MapPath("~/Reports/rptBreakdownOccuChartYr.rpt")); //foundaryRejYearly
                    rptname.FileName = Server.MapPath("~/Reports/rptBreakdownOccuChartYr.rpt"); //foundaryRejYearly
                    rptname.Refresh();
                    rptname.SetDataSource(dt2);
                }
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                if (Type == "0")
                {
                    rptname.SetParameterValue("txtPeriod", "BREAKDOWN OCCURANCE CHART For " + Convert.ToDateTime(From).ToString("MMM/yyyy"));
                }
                else
                {
                    rptname.SetParameterValue("txtPeriod", "BREAKDOWN OCCURANCE CHART For " + Convert.ToDateTime(From).ToString("MMM/yyyy"));
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
            Response.Redirect("~/ToolRoom/VIEW/ViewBreakdownOccuChartReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

