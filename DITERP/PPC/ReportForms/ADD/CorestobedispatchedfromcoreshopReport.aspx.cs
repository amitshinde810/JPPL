using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class PPC_ReportForms_ADD_CorestobedispatchedfromcoreshopReport : System.Web.UI.Page
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
            /*Check INHOUSE/BOTH from Product Master*/
            /*Check Hardcoded Machine Code from Machine Master with Machine Booking Master*/
            // Hardcode check = STL_STORE_TYPE from STOCK_LEDGER FOR Actual Stock(SUM(STL_DOC_QTY) for FOUNDRY(castingtocast Total)and coretobemade
            //Query = "SELECT * into #Temp from(SELECT DISTINCT I.I_CODE,I_CODENO,I_NAME ,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ROUND((ROUND(isnull((sum(ISNULL(CS_SCHEDULE_QTY,0)) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0)) *(CAST(2 as float)/100 +1),0),0) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0))/1000,0)  AS CorestobeMade ,isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483648),0) as [800x600],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483647),0) as [600x450],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483646),0) as [600x300],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483645),0) as [500x350] FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE  S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by I.I_CODE,I_CODENO,I_NAME,PROD_CAST_WT,PROD_CORE_WT ) as Temp select I_CODE,I_CODENO,I_NAME,PROD_CORE_WT,CorestobeMade,[800x600],[600x450],[600x300],[500x350],case when [800x600]=0 then 0 else round(CorestobeMade/[800x600],2) end as [800x600_Booking] ,case when [600x450]=0 then 0 else round(CorestobeMade/[600x450],2) end as [600x450_Booking] ,case when [600x300]=0 then 0 else round(CorestobeMade/[600x300],2) end as [600x300_Booking] ,case when [500x350]=0 then 0 else round(CorestobeMade/[500x350],2) end as [500x350_Booking] from #Temp ORDER BY #Temp.I_CODENO, I_NAME drop table #Temp";
            double NoofDaysAvailable = 25; /* Bind Dynamically*/
            Query = "select * into #Temp1 from ( SELECT DISTINCT G.GP_NAME ,I.I_CODE, I_CODENO , I_NAME ,PROD_MACHINE_LOC,ISNULL((ISNULL((ISNULL((ISNULL((select SUM(ISNULL(CPOD_ORD_QTY,0)) from CUSTPO_MASTER CPOM INNER JOIN CUSTPO_DETAIL CPOD ON CPOM.CPOM_CODE=CPOD.CPOD_CPOM_CODE WHERE CPOD.CPOD_I_CODE=I.I_CODE  AND CPOM.ES_DELETE=0 AND datepart(MONTH,CPOM.CPOM_PO_DATE)='" + dtMonth.ToString("MM") + "' and datepart(year,CPOM.CPOM_PO_DATE)='" + dtMonth.ToString("yyyy") + "'),0) +ISNULL(SIM_CUST_STOCK,0)-ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE),0)),0) +isnull(SIM_FINISH_GOODS,0) +ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE),0)),0) + isnull(SIM_RFI_STORE,0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483643' AND STL_I_CODE= I.I_CODE),0)),0) as Dispatch_Plan_Of_Machine,isnull(SIM_RFM_STORE,0) as Std_Stock_RFI,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483645' AND STL_I_CODE= I.I_CODE),0) as Actual_Stock , case when CWP_WEEK='1' then 'Week :1' when CWP_WEEK='2' then 'Week :2' when CWP_WEEK='3' then 'Week :3' when CWP_WEEK='4' then 'Week :4'  else '0' end As  CWP_WEEK , right(convert(varchar, CWP.CWP_DATE, 106), 8) as [Month] ,ISNULL(SIM_FOUNDRY,0) AS Stnd_Stock_Core ,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0) AS ActualStockCore  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join CUSTOMER_WEEKLY_PLAN CWP ON I.I_CODE=CWP.CWP_I_CODE WHERE  S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "') as TempTable1 select GP_NAME,I_CODE,I_CODENO,I_NAME,CWP_WEEK,[Month],PROD_MACHINE_LOC,case when (PROD_MACHINE_LOC='INHOUSE' OR PROD_MACHINE_LOC='BOTH') then Isnull((isnull(Dispatch_Plan_Of_Machine,0)+ isnull(Std_Stock_RFI,0)-isnull(Actual_Stock,0)),0) else 0 end  as CastReqInRFMforMachine ,case when (PROD_MACHINE_LOC='OFFLOADED' OR PROD_MACHINE_LOC='BOTH') then Isnull((isnull(Dispatch_Plan_Of_Machine,0)+ isnull(Std_Stock_RFI,0)-isnull(Actual_Stock,0)),0) else 0 end  as CastReqInRFMforVendor  ,Stnd_Stock_Core,ActualStockCore from #Temp1 order by GP_NAME,I_NAME Drop Table #Temp1";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/CorestobeDispatchFromCoreShop.rpt"));  //rptCoreToBeDispfromCoreShop
                rptname.FileName = Server.MapPath("~/PPC/Reports/CorestobeDispatchFromCoreShop.rpt");
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
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewCorestobedispatchedfromcoreshopReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

