using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class PPC_ReportForms_ADD_WeeklyOffloadingPlanning : System.Web.UI.Page
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
            //string GroupCode = Request.QueryString[2].ToString();
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
            DateTime dtMonth = new DateTime();
            dtMonth = Convert.ToDateTime(Date);
            string Query = "";
            /*Check ActualStockQty from VENDOR_WEEKLY_PLAN*/
            double NoofDaysAvailable = 25; /* Bind Dynamically*/
            //Query = "SELECT DISTINCT G.GP_NAME,I.I_CODE, I_CODENO,I_NAME ,isnull(VWP_SALE_QTY,0) as SALE_PLAN_QTY,ISNULL((select SUM(ISNULL(CPOD_ORD_QTY,0)) from CUSTPO_MASTER CPOM INNER JOIN CUSTPO_DETAIL CPOD ON CPOM.CPOM_CODE=CPOD.CPOD_CPOM_CODE WHERE CPOD.CPOD_I_CODE=I.I_CODE AND CPOM.ES_DELETE=0 AND datepart(MONTH,CPOM.CPOM_PO_DATE)='" + dtMonth.ToString("MM") + "' and datepart(year,CPOM.CPOM_PO_DATE)='" + dtMonth.ToString("yyyy") + "'),0) AS SALE_PLAN_QTY_From_ERP,ISNULL(SIM_CUST_STOCK,0) AS Tot_Stnd_Stock ,ISNULL((VWP_CUSTOMER_STOCK),0) AS ActualStockQty ,case when VWP_WEEK='1' then 'Week :1' when VWP_WEEK='2' then 'Week :2' when VWP_WEEK='3' then 'Week :3' when VWP_WEEK='4' then 'Week :4' else '0' end As VWP_WEEK,right(convert(varchar, CWP.VWP_DATE, 106), 8) as VWP_DATE FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join VENDOR_WEEKLY_PLAN CWP ON I.I_CODE=CWP.VWP_I_CODE WHERE " + StrCond + " S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'";
            Query = "SELECT DISTINCT G.GP_NAME,I.I_CODE, I_CODENO,I_NAME ,isnull((select sum(isnull(VWP_QTY,0)) from VENDOR_WEEKLY_PLAN where VWP_I_CODE=I.I_CODE and VENDOR_WEEKLY_PLAN.ES_DELETE=0 and VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VWP_WEEK=CWP.VWP_WEEK),0) as SALE_PLAN_QTY,ISNULL(SIM_CUST_STOCK,0) as Tot_Stnd_Stock,isnull((select isnull(sum(VWP_VENDOR_STOCK),0) from VENDOR_WEEKLY_PLAN where VWP_I_CODE=I.I_CODE and VENDOR_WEEKLY_PLAN.ES_DELETE=0 and VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VWP_WEEK=CWP.VWP_WEEK),0) AS ActualStockQty ,case when VWP_WEEK='1' then 'Week :1' when VWP_WEEK='2' then 'Week :2' when VWP_WEEK='3' then 'Week :3' when VWP_WEEK='4' then 'Week :4' else '0' end As CWP_WEEK,right(convert(varchar, CWP.VWP_DATE, 106), 8) as CWP_DATE FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join VENDOR_WEEKLY_PLAN CWP ON I.I_CODE=CWP.VWP_I_CODE WHERE " + StrCond + " PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') and PROD.ES_DELETE=0 and S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by G.GP_NAME,I.I_CODE, I_CODENO,I_NAME,VWP_WEEK,SIM_CUST_STOCK,right(convert(varchar, CWP.VWP_DATE, 106), 8) order by G.GP_NAME, I_CODENO,I_NAME ";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/WeeklySalePlanning.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/WeeklySalePlanning.rpt");
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
            CommonClasses.SendError("Foundry Capacity Booking Summary", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewWeeklyOffloadingPlanning.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
