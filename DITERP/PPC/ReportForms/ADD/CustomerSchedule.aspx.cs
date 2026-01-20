using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_CustomerSchedule : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Date = Request.QueryString[1].ToString();
            string StrCond = Request.QueryString[2].ToString();
            GenerateReport(Title, Date, StrCond);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Date, string StrCond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DateTime dtMonth = new DateTime();
            dtMonth = Convert.ToDateTime(Date);
            // Check Amendment count and sum(schedule qty) revision wise
            Query = "SELECT DISTINCT I.I_CODE,I.I_CODENO , I.I_NAME, ISNULL(C.CS_SCHEDULE_QTY, 0) AS CS_SCHEDULE_QTY,I_INV_RATE, isnull((select ISNULL(AM.AM_CS_SCHEDULE_QTY, 0) from CUSTOMER_SCHEDULE_AMENDMENT AM where " + StrCond + "  AM.AM_CS_CODE=C.CS_CODE AND AM.AM_AMEND_COUNT='1' and ES_DELETE = 0 and C.ES_DELETE = 0 and AM_CS_DATE= '" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) AS Rev1,isnull((select ISNULL(AM.AM_CS_SCHEDULE_QTY, 0) from CUSTOMER_SCHEDULE_AMENDMENT AM where " + StrCond + "  AM.AM_CS_CODE=C.CS_CODE AND AM.AM_AMEND_COUNT='2' and ES_DELETE = 0 and C.ES_DELETE = 0 AND AM_CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) AS Rev2,case when C.CS_AMEND_COUNT ='3' then isnull((select ISNULL(AM.AM_CS_SCHEDULE_QTY, 0) from CUSTOMER_SCHEDULE_AMENDMENT AM where " + StrCond + " AM.AM_CS_CODE=C.CS_CODE AND AM.AM_AMEND_COUNT=3 AND AM_CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) when C.CS_AMEND_COUNT>3 then isnull((select ISNULL(AM.AM_CS_SCHEDULE_QTY, 0) from CUSTOMER_SCHEDULE_AMENDMENT AM where " + StrCond + " AM.AM_CS_CODE=C.CS_CODE AND AM.AM_AMEND_COUNT =(select max(ISNULL(AM_AMEND_COUNT,0)) from CUSTOMER_SCHEDULE_AMENDMENT where " + StrCond + " AM_CS_CODE=AM.AM_CS_CODE and AM_CS_CODE=C.CS_CODE AND AM_CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "')),0) else 0 end AS Rev3,ISNULL(C.CS_WEEK1, 0) AS CS_WEEK1,  ISNULL(C.CS_WEEK2, 0) AS CS_WEEK2, ISNULL(C.CS_WEEK3, 0) AS CS_WEEK3, ISNULL(C.CS_WEEK4, 0) AS CS_WEEK4,P_NAME,ISNULL(CS_AMEND_COUNT,0) AS CS_AMEND_COUNT  into #temp   FROM CUSTOMER_SCHEDULE AS C INNER JOIN ITEM_MASTER AS I ON C.CS_I_CODE = I.I_CODE INNER JOIN PARTY_MASTER P ON C.CS_P_CODE=P.P_CODE WHERE " + StrCond + " (C.ES_DELETE = 0) and (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND C.CS_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'      SELECT I_CODE,I_CODENO,I_NAME,CS_SCHEDULE_QTY,I_INV_RATE,ROUND((CS_SCHEDULE_QTY*I_INV_RATE)/100000,2) AS AMT,Rev1 , ROUND((Rev1*I_INV_RATE/100000),2) AS AMT1, Rev2 ,ROUND((Rev2*I_INV_RATE)/100000,2) AS AMT2,Rev3,ROUND((Rev3*I_INV_RATE)/100000,2) AS AMT3,CS_WEEK1,CS_WEEK2,CS_WEEK3,CS_WEEK4,P_NAME,CS_AMEND_COUNT FROM #temp DROP TABLE #temp";
            
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/CustomerSchedule.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/CustomerSchedule.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Title", Title);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", Date);
                CrystalReportViewer1.ReportSource = rptname;
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
            CommonClasses.SendError("Customer Schedule Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewCustomerSchedule.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}