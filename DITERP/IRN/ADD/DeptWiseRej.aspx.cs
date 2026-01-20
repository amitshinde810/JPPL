using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_DeptWiseRej : System.Web.UI.Page
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
                ViewState["strUser"] = "0";
            }
            string Title = Request.QueryString[0];

            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string i_name = Request.QueryString[3].ToString();
            string Type = Request.QueryString[4].ToString();

            GenerateReport(Title, From, Todt, i_name, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string i_name, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (Type == "0")
            {
                Query = "   select * into #remp FROM   ( SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  'MACHINING IN HOUSE' AS types,'AMT' AS ABC  FROM (    SELECT  CASE WHEN  SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000 END AS IRND_REJ_QTY   ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY ,IRN_DETAIL WHERE IRN_TRANS_TYPE=0  AND IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_TYPE=1 AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND  IRND_P_CODE=-2147483210    GROUP BY DATEPART(MM, IRN_DATE) )   AS SOURCETABLE PIVOT (SUM(IRND_REJ_QTY) FOR IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE  UNION SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  'MACHINING VENDOR' AS types,'AMT' AS ABC  FROM (    SELECT  CASE WHEN  SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000 END AS IRND_REJ_QTY   ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY ,IRN_DETAIL WHERE IRN_TRANS_TYPE=0  AND IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_TYPE=1 AND IRN_DATE BETWEEN         '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'     AND  IRND_P_CODE!=-2147483210    GROUP BY DATEPART(MM, IRN_DATE) )   AS SOURCETABLE PIVOT (SUM(IRND_REJ_QTY) FOR IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE UNION SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], 'CASTING' AS types ,'AMT' AS ABC  FROM (    SELECT  CASE WHEN  SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000 END AS IRND_REJ_QTY ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY,IRN_DETAIL WHERE IRN_TRANS_TYPE=0  AND IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_TYPE=0     AND IRN_DATE BETWEEN       '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'    GROUP BY DATEPART(MM, IRN_DATE)  )   AS SOURCETABLE PIVOT (SUM(IRND_REJ_QTY) FOR IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  'MACHINING IN HOUSE' AS types,'PER' AS ABC    from ( SELECT    CASE WHEN (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=1  AND IRND_TYPE=1     AND DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) )  =0 then 0 else    (ISNULL(( SELECT ISNULL(SUM(IRND_REJ_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=0  AND IRND_TYPE=1  AND  IRND_P_CODE=-2147483210  AND   DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) )  /(ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=1  AND IRND_TYPE=1    AND DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND   DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) ) *100 END   AS IRND_REJ_QTY ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY IE,IRN_DETAIL ID where IE.IRN_TRANS_TYPE=0  AND IE.ES_DELETE=0 AND IE.IRN_CODE=ID.IRND_IRN_CODE   AND  IE.IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   GROUP BY DATEPART(MM, IE.IRN_DATE), DATEPART(YYYY, IE.IRN_DATE) )   AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  'MACHINING VENDOR' AS types,'PER' AS ABC    from ( SELECT    CASE WHEN (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=1     AND IRND_TYPE=1     AND DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) )  =0 then 0 else    (ISNULL(( SELECT ISNULL(SUM(IRND_REJ_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=0  AND IRND_TYPE=1   AND  IRND_P_CODE!=-2147483210  AND   DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) )  /(ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=1  AND IRND_TYPE=1     AND DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND   DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) ) *100 END   AS IRND_REJ_QTY ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY IE,IRN_DETAIL ID where IE.IRN_TRANS_TYPE=0  AND IE.ES_DELETE=0 AND IE.IRN_CODE=ID.IRND_IRN_CODE    AND  ID.IRND_P_CODE!=-2147483210 AND  IE.IRN_DATE BETWEEN        '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'     GROUP BY DATEPART(MM, IE.IRN_DATE), DATEPART(YYYY, IE.IRN_DATE) )   AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable UNION   select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  'CASTING' AS types ,'PER' AS ABC from (  SELECT   CASE WHEN (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=1  AND IRND_TYPE=1  AND DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) )  =0 then 0 else    (ISNULL(( SELECT ISNULL(SUM(IRND_REJ_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=0  AND IRND_TYPE=0   AND  DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) )  /(ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND  ES_DELETE=0   AND IRN_TRANS_TYPE=1  AND IRND_TYPE=1  AND DATEPART(MM, IRN_DATE)=DATEPART(MM, IE.IRN_DATE) AND  DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IE.IRN_DATE)),0) ) *100 END   AS IRND_REJ_QTY ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY IE,IRN_DETAIL ID where IE.IRN_TRANS_TYPE=0  AND IE.ES_DELETE=0 AND IE.IRN_CODE=ID.IRND_IRN_CODE   AND  IE.IRN_DATE BETWEEN      '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'    GROUP BY DATEPART(MM, IE.IRN_DATE), DATEPART(YYYY, IE.IRN_DATE)    )   AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable      ) AS tr SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], types,ABC FROM #remp GROUP BY  types,ABC ORDER BY types,ABC DROP TABLE #remp  ";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptDeptWiseRej.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptDeptWiseRej.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "COMPANY REJECTION TREND PROCESS WISE From " + Convert.ToDateTime(From).ToString("MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("MMM/yyyy"));
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/ViewDeptWiseRej.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
