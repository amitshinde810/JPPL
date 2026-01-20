﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_DefectWiseRej : System.Web.UI.Page
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
            string cond = "";
            if (i_name != "0")
            {
                cond = "  AND  IRND_I_CODE='" + i_name + "' ";
            }
            if (Type == "0")
            {
                Query = "  select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE  from (   SELECT IRN.IRN_DATE,I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE, SUM(IRND_REJ_QTY) AS IRND_REJ_QTY  ,DATEPART(dd, IRN.IRN_DATE) AS IRN_DAY  FROM IRN_ENTRY IRN,IRN_DETAIL,ITEM_MASTER,REASON_MASTER WHERE IRN.IRN_CODE=IRND_IRN_CODE AND IRN.ES_DELETE=0  AND IRN.IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRND_RM_CODE=RM_CODE  AND IRN.IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'    " + cond + "  GROUP BY IRN.IRN_DATE,I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE )   AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT ,RM_TYPE   ORDER BY I_CODE,I_CODENO,RM_TYPE DESC,RM_DEFECT";
            }
            else if (Type == "1")
            {
                Query = "  select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE  from (   SELECT IRN.IRN_DATE,I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE, (SUM(IRND_AMT)/100000) AS IRND_AMT  ,DATEPART(dd, IRN.IRN_DATE) AS IRN_DAY  FROM IRN_ENTRY IRN,IRN_DETAIL,ITEM_MASTER,REASON_MASTER WHERE IRN.IRN_CODE=IRND_IRN_CODE AND IRN.ES_DELETE=0  AND IRN.IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRND_RM_CODE=RM_CODE  AND IRN.IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'     " + cond + "     GROUP BY IRN.IRN_DATE,I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE )   AS SOURCETABLE PIVOT (sum(IRND_AMT) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT ,RM_TYPE   ORDER BY I_CODE,I_CODENO,RM_TYPE DESC,RM_DEFECT";
            }
            else
            {
                Query = "  select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE  from ( SELECT IRN.IRN_DATE,I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE,((case when (ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY IE,IRN_DETAIL ID where IE.IRN_CODE=ID.IRND_IRN_CODE AND  IE.ES_DELETE=0  AND IE.IRN_TRANS_TYPE=1 AND ID.IRND_I_CODE=I_CODE AND IE.IRN_DATE=IRN.IRN_DATE),0) ) =0 then 0 else  (SUM(IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0)  FROM IRN_ENTRY IE,IRN_DETAIL ID where IE.IRN_CODE=ID.IRND_IRN_CODE AND  IE.ES_DELETE=0  AND IE.IRN_TRANS_TYPE=1 AND ID.IRND_I_CODE=I_CODE AND IE.IRN_DATE=IRN.IRN_DATE),0) ) END ) *100)  AS IRND_REJ_QTY ,DATEPART(dd, IRN.IRN_DATE) AS IRN_DAY  FROM IRN_ENTRY IRN,IRN_DETAIL,ITEM_MASTER,REASON_MASTER WHERE IRN.IRN_CODE=IRND_IRN_CODE AND IRN.ES_DELETE=0  AND IRN.IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRND_RM_CODE=RM_CODE AND IRN.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'    " + cond + "  GROUP BY IRN.IRN_DATE,I_CODE,I_CODENO,I_NAME,RM_DEFECT ,RM_TYPE )   AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY I_CODE,I_CODENO,I_NAME,RM_DEFECT,RM_TYPE    ORDER BY I_CODE,I_CODENO,RM_TYPE DESC,RM_DEFECT  ";
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
                    rptname.Load(Server.MapPath("~/Reports/rptIRNDefectDetail.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIRNDefectDetail.rpt");
                }
                else if (Type == "1")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptIRNDefectDetail1.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIRNDefectDetail1.rpt");
                }
                else
                {
                    rptname.Load(Server.MapPath("~/Reports/rptIRNDefectDetail2.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIRNDefectDetail2.rpt");
                }
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                if (Type == "0")
                {
                    rptname.SetParameterValue("txtPeriod", "Monthly defect specific rejection details From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
                else if (Type == "1")
                {
                    rptname.SetParameterValue("txtPeriod", "Monthly defect specific rejection details From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
                else
                {
                    rptname.SetParameterValue("txtPeriod", "Monthly defect specific rejection details From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
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
            Response.Redirect("~/IRN/VIEW/ViewDefectWiseRej.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
