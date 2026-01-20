﻿﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_AddReasonReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    public static string exe = "0";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPP");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPPMV");
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
            string Todt = Request.QueryString[2].ToString();
            string Type = Request.QueryString[3].ToString();
            string Cond = Request.QueryString[4].ToString();
            string PartyCode = Request.QueryString[5].ToString();

            GenerateReport(Title, From, Todt, Type, Cond, PartyCode);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string Type, string Cond, string PartyCode)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt = new DataTable();

            if (Type == "0")
            {
                Query = "select ISNULL([1],0) AS [1],ISNULL([2],0) AS [2],ISNULL([3],0) AS [3],ISNULL([4],0) AS [4],ISNULL([5],0) AS [5],ISNULL([6],0) AS [6],ISNULL([7],0) AS [7],ISNULL([8],0) AS [8],ISNULL([9],0) AS [9],ISNULL([10],0) AS [10],ISNULL([11],0) AS [11],ISNULL([12],0) AS [12],ISNULL([13],0) AS [13],ISNULL([14],0) AS [14],ISNULL([15],0) AS [15],ISNULL([16],0) AS [16],ISNULL([17],0) AS [17],ISNULL([18],0) AS [18],ISNULL([19],0) AS [19],ISNULL([20],0) AS [20],ISNULL([21],0) AS [21],ISNULL([22],0) AS [22],ISNULL([23],0) AS [23],ISNULL([24],0) AS [24],ISNULL([25],0) AS [25],ISNULL([26],0) AS [26],ISNULL([27],0) AS [27],ISNULL([28],0) AS [28],ISNULL([29],0) AS [29],ISNULL([30],0) AS [30],ISNULL([31],0) AS [31],SPR_CODE,SPR_DESC from(select distinct SHORTPROD_REASON.SPR_CODE,SHORTPROD_REASON.SPR_DESC,sum(iSNULL(SHORT_QTY,0)) AS SHORT_QTY,left(datename(day,IRN_DATE),3)as [month] from SHORTPROD_REASON,IRN_SPDETAIL,IRN_ENTRY where " + Cond + " IRN_SPDETAIL.SPR_CODE=SHORTPROD_REASON.SPR_CODE and SHORTPROD_REASON.ES_DELETE=0 and IRN_ENTRY.ES_DELETE=0 and IRN_SPDETAIL.IRN_CODE=IRN_ENTRY.IRN_CODE group by SHORTPROD_REASON.SPR_CODE,SHORTPROD_REASON.SPR_DESC,left(datename(day,IRN_DATE),3))as s PIVOT(SUM(SHORT_QTY) FOR [month] IN ([1], [2],[3], [4],[5], [6], [7], [8], [9], [10], [11], [12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31]))AS pvt";
            }
            else
            {
                Query = "select ISNULL([JAN],0) AS [JAN],ISNULL([FEB],0) AS [FEB],ISNULL([MAR],0) AS [MAR],ISNULL([APR],0) AS [APR],ISNULL([MAY],0) AS [MAY],ISNULL([JUN],0) AS [JUN],ISNULL([JUL],0) AS [JUL],ISNULL([AUG],0) AS [AUG],ISNULL([SEP],0) AS [SEP],ISNULL([OCT],0) AS [OCT],ISNULL([NOV],0) AS [NOV],ISNULL([DEC],0) AS [DEC],SPR_CODE,SPR_DESC from(select distinct SHORTPROD_REASON.SPR_CODE,SHORTPROD_REASON.SPR_DESC,sum(iSNULL(SHORT_QTY,0)) AS SHORT_QTY,left(datename(month,IRN_DATE),3)as [month] from SHORTPROD_REASON,IRN_SPDETAIL,IRN_ENTRY where " + Cond + " IRN_SPDETAIL.SPR_CODE=SHORTPROD_REASON.SPR_CODE and SHORTPROD_REASON.ES_DELETE=0 and IRN_ENTRY.ES_DELETE=0 and IRN_SPDETAIL.IRN_CODE=IRN_ENTRY.IRN_CODE group by SHORTPROD_REASON.SPR_CODE,SHORTPROD_REASON.SPR_DESC,left(datename(month,IRN_DATE),3))as s PIVOT(SUM(SHORT_QTY) FOR [month] IN (JAN,FEB,MAR,APR,MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC)) AS pvt";
            }
            dt = CommonClasses.Execute(Query);

            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (Type == "0")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptreasonWiseMonthlyShortProduction.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptreasonWiseMonthlyShortProduction.rpt");
                }
                else    
                {
                    rptname.Load(Server.MapPath("~/Reports/rptreasonWiseYearlyShortProduction.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptreasonWiseYearlyShortProduction.rpt");
                }
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                if (Type == "0")
                {
                    rptname.SetParameterValue("txtPeriod", "REASON WISE MONTHLY SHORT PRODUCTION FROM " + Convert.ToDateTime(From).ToString("dd MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("dd MMM yyyy"));
                }
                else
                {
                    rptname.SetParameterValue("txtPeriod", "REASON WISE YEARLY SHORT PRODUCTION FROM " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
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
            Response.Redirect("~/IRN/VIEW/ViewReasonReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}