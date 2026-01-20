using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class ToolRoom_ADD_WeeklyPM : System.Web.UI.Page
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
            string week = Request.QueryString[3].ToString();
            string WeekNM = Request.QueryString[4].ToString();

            GenerateReport(Title, Cond, From, week, WeekNM);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Cond, string From, string week, string WeekNM)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt2 = new DataTable();
            DataTable dt = new DataTable();

            DataTable dtFilter = new DataTable();

            //                 Query = " select T_NAME,I_CODENO,I_NAME,PM_MONTH,PM_YEAR,PM_TYPE,ISNULL([WEEK 1],0) as [WEEK 1],ISNULL([WEEK 2],0) as [WEEK 2],ISNULL([WEEK 3],0) as [WEEK 3],ISNULL([WEEK 4],0) as [WEEK 4] from (select isnull(T_CODE,0) as T_CODE,T_NAME,I_CODENO,I_NAME,LEFT(DATENAME(MONTH, PM_MONTH),3) AS PM_MONTH,year(PM_MONTH) as PM_YEAR,case when isnull(CONVERT(varchar,PM_WEEK ),0)='0' then 'WEEK 1' when CONVERT(varchar,PM_WEEK )='1' then 'WEEK 2' when CONVERT(varchar,PM_WEEK )='2' then 'WEEK 3' when CONVERT(varchar,PM_WEEK )='3' then 'WEEK 4' end as PM_WEEK ,case when PM_TYPE=0 then 'DIE' else 'CORE BOX' end as PM_TYPE from TOOL_MASTER,ITEM_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER where " + Cond + " I_CODE=PM_I_CODE and PM_TOOL_NO=T_CODE and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and T_CM_COMP_ID=1 and T_STATUS=1) DataTable PIVOT (SUM(T_CODE) FOR PM_WEEK IN ([WEEK 1],[WEEK 2],[WEEK 3],[WEEK 4])) PivotTable";

          //  dt = CommonClasses.Execute(" select T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME , 'A' As TYPES , ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31]  from ( select T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME ,WPM_TOOL_RECEIVED_DATE  ,DATEPART(dd, WPM_TOOL_RECEIVED_DATE) AS IRN_DAY  from dbo.WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where " + Cond + " DATEPART(MM,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Month + "' AND DATEPART(YYYY,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Year + "' AND WPM_STATUS=1 AND WPM_T_CODE=T_CODE AND T_I_CODE=I_CODE   )   AS SOURCETABLE PIVOT (COUNT(WPM_TOOL_RECEIVED_DATE) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME         UNION       select  T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME , 'B' As TYPES  ,ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31]   from ( select T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME ,WPM_TOOL_COMPLETED_DATE  ,DATEPART(dd, WPM_TOOL_COMPLETED_DATE) AS IRN_DAY  from dbo.WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where " + Cond + "   DATEPART(MM,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Month + "' AND DATEPART(YYYY,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Year + "' AND WPM_STATUS=1 AND WPM_T_CODE=T_CODE AND T_I_CODE=I_CODE   )   AS SOURCETABLE PIVOT (COUNT(WPM_TOOL_COMPLETED_DATE) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME ");


            dt = CommonClasses.Execute(" select T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME , 'A' As TYPES , ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31]  from ( select T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME ,WPM_TOOL_RECEIVED_DATE  ,DATEPART(dd, WPM_TOOL_RECEIVED_DATE) AS IRN_DAY  from dbo.WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where " + Cond + " DATEPART(MM,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Month + "' AND DATEPART(YYYY,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Year + "'  AND WPM_T_CODE=T_CODE AND T_I_CODE=I_CODE   )   AS SOURCETABLE PIVOT (COUNT(WPM_TOOL_RECEIVED_DATE) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME         UNION       select  T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME , 'B' As TYPES  ,ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31]   from ( select T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME ,WPM_TOOL_COMPLETED_DATE  ,DATEPART(dd, WPM_TOOL_COMPLETED_DATE) AS IRN_DAY  from dbo.WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where " + Cond + "   DATEPART(MM,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Month + "' AND DATEPART(YYYY,WPM_TOOL_RECEIVED_DATE )='" + Convert.ToDateTime(From).Year + "'    AND WPM_T_CODE=T_CODE AND T_I_CODE=I_CODE   )   AS SOURCETABLE PIVOT (COUNT(WPM_TOOL_COMPLETED_DATE) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY T_TYPE,T_NAME,I_CODE,T_CODE,I_CODENO,I_NAME ");


            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("TYPES", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("1", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("2", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("3", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("4", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("5", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("6", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("7", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("8", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("9", typeof(String)));
            }
            DateTime dtdate = new DateTime();
            if (dt.Rows.Count > 0)
            {
                if (week == "0")
                {
                    dt.Columns.Remove("8");
                    dt.Columns.Remove("9");
                    dt.Columns.Remove("10");
                    dt.Columns.Remove("11");
                    dt.Columns.Remove("12");
                    dt.Columns.Remove("13");
                    dt.Columns.Remove("14");
                    dt.Columns.Remove("15");
                    dt.Columns.Remove("16");
                    dt.Columns.Remove("17");
                    dt.Columns.Remove("18");
                    dt.Columns.Remove("19");
                    dt.Columns.Remove("20");
                    dt.Columns.Remove("21");
                    dt.Columns.Remove("22");
                    dt.Columns.Remove("23");
                    dt.Columns.Remove("24");
                    dt.Columns.Remove("25");
                    dt.Columns.Remove("26");
                    dt.Columns.Remove("27");
                    dt.Columns.Remove("28");
                    dt.Columns.Remove("29");
                    dt.Columns.Remove("30");
                    dt.Columns.Remove("31");
                    dtdate = Convert.ToDateTime(Convert.ToDateTime(From).ToString("01/MMM/yyyy"));
                }

                if (week == "1")
                {
                    dt.Columns.Remove("1");
                    dt.Columns.Remove("2");
                    dt.Columns.Remove("3");
                    dt.Columns.Remove("4");
                    dt.Columns.Remove("5");
                    dt.Columns.Remove("6");
                    dt.Columns.Remove("7");


                    dt.Columns.Remove("16");
                    dt.Columns.Remove("17");
                    dt.Columns.Remove("18");
                    dt.Columns.Remove("19");
                    dt.Columns.Remove("20");
                    dt.Columns.Remove("21");
                    dt.Columns.Remove("22");
                    dt.Columns.Remove("23");
                    dt.Columns.Remove("24");
                    dt.Columns.Remove("25");
                    dt.Columns.Remove("26");
                    dt.Columns.Remove("27");
                    dt.Columns.Remove("28");
                    dt.Columns.Remove("29");
                    dt.Columns.Remove("30");
                    dt.Columns.Remove("31");
                    dtdate = Convert.ToDateTime(Convert.ToDateTime(From).ToString("08/MMM/yyyy"));
                }
                if (week == "2")
                {
                    dt.Columns.Remove("1");
                    dt.Columns.Remove("2");
                    dt.Columns.Remove("3");
                    dt.Columns.Remove("4");
                    dt.Columns.Remove("5");
                    dt.Columns.Remove("6");
                    dt.Columns.Remove("7");
                    dt.Columns.Remove("8");
                    dt.Columns.Remove("9");
                    dt.Columns.Remove("10");
                    dt.Columns.Remove("11");
                    dt.Columns.Remove("12");
                    dt.Columns.Remove("13");
                    dt.Columns.Remove("14");
                    dt.Columns.Remove("15");
                    //dt.Columns.Remove("16");

                    dt.Columns.Remove("24");
                    dt.Columns.Remove("25");
                    dt.Columns.Remove("26");
                    dt.Columns.Remove("27");
                    dt.Columns.Remove("28");
                    dt.Columns.Remove("29");
                    dt.Columns.Remove("30");
                    dt.Columns.Remove("31");
                    dtdate = Convert.ToDateTime(Convert.ToDateTime(From).ToString("16/MMM/yyyy"));
                }
                if (week == "3")
                {
                    dt.Columns.Remove("1");
                    dt.Columns.Remove("2");
                    dt.Columns.Remove("3");
                    dt.Columns.Remove("4");
                    dt.Columns.Remove("5");
                    dt.Columns.Remove("6");
                    dt.Columns.Remove("7");
                    dt.Columns.Remove("8");
                    dt.Columns.Remove("9");
                    dt.Columns.Remove("10");
                    dt.Columns.Remove("11");
                    dt.Columns.Remove("12");
                    dt.Columns.Remove("13");
                    dt.Columns.Remove("14");
                    dt.Columns.Remove("15");
                    //dt.Columns.Remove("16");
                    dt.Columns.Remove("16");
                    dt.Columns.Remove("17");
                    dt.Columns.Remove("18");
                    dt.Columns.Remove("19");
                    dt.Columns.Remove("20");
                    dt.Columns.Remove("21");
                    dt.Columns.Remove("22");
                    dt.Columns.Remove("23");
                    dtdate = Convert.ToDateTime(Convert.ToDateTime(From).ToString("24/MMM/yyyy"));
                }
                foreach (DataRow dr in dt.Rows)
                {

                    dtFilter.Rows.Add(dr.ItemArray);
                }
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptWPMreport.rpt")); //foundaryRejYearly
                rptname.FileName = Server.MapPath("~/Reports/rptWPMreport.rpt"); //foundaryRejYearly
                rptname.Refresh();
                rptname.SetDataSource(dtFilter);
                rptname.SetParameterValue("1", dtdate.ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("2", (dtdate.AddDays(1)).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("3", dtdate.AddDays(2).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("4", dtdate.AddDays(3).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("5", dtdate.AddDays(4).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("6", dtdate.AddDays(5).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("7", dtdate.AddDays(6).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("8", dtdate.AddDays(7).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("9", dtdate.AddDays(8).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("txtPeriod", "WEEKLY PREVENTIVE MAINTENANCE");// From" + From + "To " + To + "
                rptname.SetParameterValue("WeekNM", "" + WeekNM + "");
                rptname.SetParameterValue("Month", "" + From + "");
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
            Response.Redirect("~/ToolRoom/VIEW/ViewWeeklyPM.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}


