using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_RejSummaryStageWise : System.Web.UI.Page
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
            string temtitle = "";
            if (Type == "0")
            {
                Query = "SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  STG_CODE,STG_NAME  FROM (   SELECT DISTINCT STG_CODE,STG_NAME,I_CODE,I_NAME,I_CODENO,DATEPART(MM,IR.IRN_DATE) AS IRN_MONTH, DATEPART(YYYY,IR.IRN_DATE) AS IRN_YEAR ,   ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,    ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0)  AS IRND_REJ ,     (CASE WHEN   ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) =0 THEN 0 ELSE  ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0)/100000  END )AS IRND_AMT,     ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE)),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) ),0)) END )*100)   AS CUMMDATEREJ         FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER ,STGAEWISE_ITEM,STAGE_MASTER where  I_CODE=SI_I_CODE  AND STG_CODE=SI_STG_CODE AND SI_ACTIVE=1  AND IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,DATEPART(MM,IRN_DATE) ,DATEPART(YYYY,IRN_DATE)  ,STG_CODE,STG_NAME               ) SOURCETABLE PIVOT (SUM(IRND_REJ) FOR IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE    GROUP BY STG_CODE,STG_NAME";
                temtitle = " ( NOS ) ";
            }
            else if (Type == "1")
            {
                Query = "   SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  STG_CODE,STG_NAME  FROM (   SELECT DISTINCT STG_CODE,STG_NAME,I_CODE,I_NAME,I_CODENO,DATEPART(MM,IR.IRN_DATE) AS IRN_MONTH, DATEPART(YYYY,IR.IRN_DATE) AS IRN_YEAR ,   ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,    ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0)  AS IRND_REJ ,    ROUND( (CASE WHEN   ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) =0 THEN 0 ELSE  ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0)/100000  END ),2) AS IRND_AMT,     ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE)),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) ),0)) END )*100)   AS CUMMDATEREJ         FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER ,STGAEWISE_ITEM,STAGE_MASTER where  I_CODE=SI_I_CODE  AND STG_CODE=SI_STG_CODE AND SI_ACTIVE=1  AND IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,DATEPART(MM,IRN_DATE) ,DATEPART(YYYY,IRN_DATE)  ,STG_CODE,STG_NAME               ) SOURCETABLE PIVOT (SUM(IRND_AMT) FOR IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE    GROUP BY STG_CODE,STG_NAME";
                temtitle = "  ( AMOUNT  )  ";
            }
            else
            {
                Query = "    SELECT ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],  STG_CODE,STG_NAME  FROM (   SELECT DISTINCT STG_CODE,STG_NAME,DATEPART(MM,IR.IRN_DATE) AS IRN_MONTH, DATEPART(YYYY,IR.IRN_DATE) AS IRN_YEAR ,  ROUND(((case when (ISNULL((  SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL,STGAEWISE_ITEM  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=SI_I_CODE  AND    DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND SI_STG_CODE=STG_CODE ),0) ) =0 then 0 else ( (ISNULL((  SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL,STGAEWISE_ITEM  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0   AND IRND_I_CODE=SI_I_CODE  AND    DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND SI_STG_CODE=STG_CODE ),0)) )/(ISNULL((  SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL,STGAEWISE_ITEM  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=SI_I_CODE  AND    DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND SI_STG_CODE=STG_CODE ),0)) END )*100),2 )  AS CUMMDATEREJ   FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER ,STGAEWISE_ITEM,STAGE_MASTER where  I_CODE=SI_I_CODE  AND STG_CODE=SI_STG_CODE AND SI_ACTIVE=1  AND IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE   IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN      '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  ) AND IR.IRN_DATE BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,DATEPART(MM,IRN_DATE) ,DATEPART(YYYY,IRN_DATE)  ,STG_CODE,STG_NAME       ) SOURCETABLE PIVOT (SUM(CUMMDATEREJ) FOR IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PIVOTTABLE    GROUP BY STG_CODE,STG_NAME";
                temtitle = "   ( %  )   ";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (i_name != "0")
            {
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = " STG_CODE in(" + i_name + ")";
                dt = dv1.ToTable();
            }

            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptRejSummStage.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptRejSummStage.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtRptType", Type);

                rptname.SetParameterValue("txtPeriod", "REJECTION SUMMARY STAGE WISE " + temtitle + " From " + Convert.ToDateTime(From).ToString("MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("MMM/yyyy"));
                if (i_name != "0")
                {
                    rptname.SetParameterValue("txtType", "1");
                }
                else
                {
                    rptname.SetParameterValue("txtType", "0");
                }
                rptname.SetParameterValue("txtFilter", temtitle);
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
            Response.Redirect("~/IRN/VIEW/ViewRejSummaryStageWise.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
