﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class IRN_ADD_DefectWiseFoundryRejYearly : System.Web.UI.Page
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
            if (i_name == "0")
            {
                Query = "(select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,RM_DEFECT ,'NOS.' AS Types from (SELECT I_CODE,I_CODENO,I_NAME,SUM(IRND_REJ_QTY) AS IRND_REJ_QTY,DATEPART(MM,IRN_DATE) AS IRN_MONTH,RM_DEFECT FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,REASON_MASTER where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_TYPE=0 AND IRN_TRANS_TYPE = 0 AND IRND_I_CODE=I_CODE AND IRND_RM_CODE=RM_CODE and IRN_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   GROUP BY DATEPART(MM,IRN_DATE),RM_DEFECT,I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,RM_DEFECT ,'RS' AS Types  from (SELECT I_CODE,I_CODENO,I_NAME,CASE WHEN SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000 END    AS IRND_REJ_QTY,DATEPART(MM, IRN_DATE) AS IRN_MONTH,RM_DEFECT FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,REASON_MASTER where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE = 0 AND IRND_TYPE=0 AND IRND_I_CODE=I_CODE  AND IRND_RM_CODE=RM_CODE and IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IRN_DATE),RM_DEFECT,I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,RM_DEFECT,'t PER(%)' AS Types from (   SELECT I_CODE,I_CODENO,I_NAME,RM_DEFECT,   CASE WHEN ( ISNULL(( SELECT SUM(IRD.IRND_PROD_QTY) FROM IRN_ENTRY AS IRN INNER JOIN IRN_DETAIL AS IRD ON IRN.IRN_CODE = IRD.IRND_IRN_CODE WHERE (IRN.ES_DELETE = 0)  AND IRD.IRND_TYPE=1 AND (IRN.IRN_TRANS_TYPE = 1) AND IRD.IRND_I_CODE=I_CODE AND MONTH(IRN.IRN_DATE)=MONTH(IE.IRN_DATE ) AND YEAR(IRN.IRN_DATE)=YEAR(IE.IRN_DATE )),0))=0  then 0 else (SUM(ID.IRND_REJ_QTY)/( SELECT SUM(IRD.IRND_PROD_QTY) FROM IRN_ENTRY AS IRN INNER JOIN IRN_DETAIL AS IRD ON IRN.IRN_CODE = IRD.IRND_IRN_CODE WHERE     (IRN.ES_DELETE = 0) AND (IRN.IRN_TRANS_TYPE = 1) AND IRD.IRND_TYPE=1 AND IRD.IRND_I_CODE=I_CODE AND MONTH(IRN.IRN_DATE)=MONTH(IE.IRN_DATE ) AND YEAR(IRN.IRN_DATE)=YEAR(IE.IRN_DATE ))*100) END  AS IRND_REJ_QTY ,DATEPART(MM,IRN_DATE) AS IRN_MONTH FROM IRN_ENTRY IE,IRN_DETAIL ID,ITEM_MASTER,REASON_MASTER where    IE.ES_DELETE=0 AND IE.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_TYPE=0 AND ID.IRND_I_CODE=I_CODE AND ID.IRND_RM_CODE=RM_CODE   and IE.IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME,RM_DEFECT,DATEPART(YYYY, IE.IRN_DATE) ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME ,RM_DEFECT) ORDER BY I_CODE,I_CODENO,RM_DEFECT,Types";
            }
            else
            {
                Query = "(select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,RM_DEFECT ,'NOS.' AS Types from (SELECT I_CODE,I_CODENO,I_NAME,SUM(IRND_REJ_QTY) AS IRND_REJ_QTY,DATEPART(MM,IRN_DATE) AS IRN_MONTH,RM_DEFECT FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,REASON_MASTER where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_TYPE=0 AND IRN_TRANS_TYPE = 0 AND IRND_I_CODE=I_CODE AND IRND_RM_CODE=RM_CODE and I_CODE=" + i_name + "    and IRN_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM,IRN_DATE),RM_DEFECT,I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,RM_DEFECT ,'RS' AS Types  from (SELECT I_CODE,I_CODENO,I_NAME, CASE WHEN SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000 END  AS IRND_REJ_QTY,DATEPART(MM, IRN_DATE) AS IRN_MONTH,RM_DEFECT FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,REASON_MASTER where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE = 0  AND IRND_TYPE=0 AND IRND_I_CODE=I_CODE  AND IRND_RM_CODE=RM_CODE and I_CODE=" + i_name + "    and IRN_DATE  between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' GROUP BY DATEPART(MM, IRN_DATE),RM_DEFECT,I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,RM_DEFECT,'t PER(%)' AS Types from (   SELECT I_CODE,I_CODENO,I_NAME,RM_DEFECT,   CASE WHEN ( ISNULL(( SELECT SUM(IRD.IRND_PROD_QTY) FROM IRN_ENTRY AS IRN INNER JOIN IRN_DETAIL AS IRD ON IRN.IRN_CODE = IRD.IRND_IRN_CODE WHERE (IRN.ES_DELETE = 0)  AND IRD.IRND_TYPE=1 AND (IRN.IRN_TRANS_TYPE = 1) AND IRD.IRND_I_CODE=I_CODE AND MONTH(IRN.IRN_DATE)=MONTH(IE.IRN_DATE ) AND YEAR(IRN.IRN_DATE)=YEAR(IE.IRN_DATE )),0))=0  then 0 else (SUM(ID.IRND_REJ_QTY)/( SELECT SUM(IRD.IRND_PROD_QTY) FROM IRN_ENTRY AS IRN INNER JOIN IRN_DETAIL AS IRD ON IRN.IRN_CODE = IRD.IRND_IRN_CODE WHERE     (IRN.ES_DELETE = 0) AND (IRN.IRN_TRANS_TYPE = 1) AND IRD.IRND_TYPE=1 AND IRD.IRND_I_CODE=I_CODE AND MONTH(IRN.IRN_DATE)=MONTH(IE.IRN_DATE ) AND YEAR(IRN.IRN_DATE)=YEAR(IE.IRN_DATE ))*100) END  AS IRND_REJ_QTY ,DATEPART(MM,IRN_DATE) AS IRN_MONTH FROM IRN_ENTRY IE,IRN_DETAIL ID,ITEM_MASTER,REASON_MASTER where    IE.ES_DELETE=0 AND IE.IRN_CODE=ID.IRND_IRN_CODE AND IE.IRN_TRANS_TYPE = 0  AND ID.IRND_TYPE=0 AND ID.IRND_I_CODE=I_CODE AND ID.IRND_RM_CODE=RM_CODE   and IE.IRN_DATE  between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   and I_CODE=" + i_name + "    GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME,RM_DEFECT,DATEPART(YYYY, IE.IRN_DATE)  ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME ,RM_DEFECT) ORDER BY I_CODE,I_CODENO,RM_DEFECT,Types";
            }

            DataSet ds = new DataSet();
            DataTable dtNew = new DataTable();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (Type == "0")
            {
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = " Types='NOS.'";
                dtNew = dv1.ToTable();
            }
            if (Type == "1")
            {
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = " Types='RS'";
                dtNew = dv1.ToTable();
            }
            if (Type == "2")
            {
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = " Types='t PER(%)'";
                dtNew = dv1.ToTable();
            }
            if (dtNew.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/DefectWisefoundaryRejYearly.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/DefectWisefoundaryRejYearly.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dtNew);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                if (Type == "0")
                {
                    rptname.SetParameterValue("txtPeriod", "COMPONENT SPECIFIC DEFECT SPECIFIC MONTH WISE FOUNDRY REJECTION DETAILS From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
                }
                if (Type == "1")
                {
                    rptname.SetParameterValue("txtPeriod", "COMPONENT SPECIFIC DEFECT SPECIFIC MONTH WISE FOUNDRY REJECTION DETAILS From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
                }
                if (Type == "2")
                {
                    rptname.SetParameterValue("txtPeriod", "COMPONENT SPECIFIC DEFECT SPECIFIC MONTH WISE FOUNDRY REJECTION DETAILS From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
                }
                CrystalReportViewer1.ReportSource = rptname;
                CrystalReportViewer1.ID = Title.Replace(" ","_");
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
            Response.Redirect("~/IRN/VIEW/ViewDefectWiseFoundryRejYearly.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
