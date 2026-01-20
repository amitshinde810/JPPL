using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class PPC_ReportForms_ADD_CastingrequiredfrominspectionReport : System.Web.UI.Page
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
            string WeekCond = Request.QueryString[3].ToString();
            GenerateReport(Title, Date, StrCond, WeekCond);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Date, string StrCond, string WeekCond)
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
            //Query = "SELECT * into #Temp from(SELECT DISTINCT I.I_CODE,I_CODENO,I_NAME ,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ROUND((ROUND(isnull((sum(ISNULL(CS_SCHEDULE_QTY,0)) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0)) *(CAST(2 as float)/100 +1),0),0) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0))/1000,0) AS CorestobeMade ,isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483648),0) as [800x600],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483647),0) as [600x450],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483646),0) as [600x300],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483645),0) as [500x350] FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by I.I_CODE,I_CODENO,I_NAME,PROD_CAST_WT,PROD_CORE_WT ) as Temp select I_CODE,I_CODENO,I_NAME,PROD_CORE_WT,CorestobeMade,[800x600],[600x450],[600x300],[500x350],case when [800x600]=0 then 0 else round(CorestobeMade/[800x600],2) end as [800x600_Booking] ,case when [600x450]=0 then 0 else round(CorestobeMade/[600x450],2) end as [600x450_Booking] ,case when [600x300]=0 then 0 else round(CorestobeMade/[600x300],2) end as [600x300_Booking] ,case when [500x350]=0 then 0 else round(CorestobeMade/[500x350],2) end as [500x350_Booking] from #Temp ORDER BY #Temp.I_CODENO, I_NAME drop table #Temp";
            double NoofDaysAvailable = 25; /* Bind Dynamically*/
            //Query = "SELECT DISTINCT G.GP_NAME as [Group],I.I_CODE, I_CODENO as Part_No, I_NAME as [Part_Name],ISNULL((ISNULL((select SUM(ISNULL(CPOD_ORD_QTY,0)) from CUSTPO_MASTER CPOM INNER JOIN CUSTPO_DETAIL CPOD ON CPOM.CPOM_CODE=CPOD.CPOD_CPOM_CODE WHERE CPOD.CPOD_I_CODE=I.I_CODE AND CPOM.ES_DELETE=0 AND datepart(MONTH,CPOM.CPOM_PO_DATE)='" + dtMonth.ToString("MM") + "' and datepart(year,CPOM.CPOM_PO_DATE)='" + dtMonth.ToString("yyyy") + "'),0) +ISNULL(SIM_CUST_STOCK,0)-ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE),0)),0) AS Sale_Plan,isnull(SIM_FINISH_GOODS,0) as STD_StockInFG,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE),0) as Actual_Stock_InFG,case when CWP_WEEK='1' then 'Week :1' when CWP_WEEK='2' then 'Week :2' when CWP_WEEK='3' then 'Week :3' when CWP_WEEK='4' then 'Week :4' else '0' end As [Week] , right(convert(varchar, CWP.CWP_DATE, 106), 8) as [Month] FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join CUSTOMER_WEEKLY_PLAN CWP ON I.I_CODE=CWP.CWP_I_CODE WHERE " + StrCond + " S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' order by GP_NAME,I_NAME";
            //Query = "SELECT DISTINCT G.GP_NAME as [Group],I.I_CODE, I_CODENO as Part_No, I_NAME as [Part_Name], ISNULL((isnull((select sum(isnull(CWP_SALE_QTY,0)) from CUSTOMER_WEEKLY_PLAN where CWP_I_CODE=I.I_CODE and CUSTOMER_WEEKLY_PLAN.ES_DELETE=0 and CWP_P_CODE=CWP.CWP_P_CODE and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) + ISNULL(SIM_CUST_STOCK,0)-isnull((select isnull(sum(CWP_CUSTOMER_STOCK),0) from CUSTOMER_WEEKLY_PLAN where CWP_I_CODE=I.I_CODE and CUSTOMER_WEEKLY_PLAN.ES_DELETE=0 and CWP_P_CODE=CWP.CWP_P_CODE and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)),0) AS Sale_Plan ,isnull(SIM_FINISH_GOODS,0) as STD_StockInFG,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE),0) as Actual_Stock_InFG,case when CWP_WEEK='1' then 'Week :1' when CWP_WEEK='2' then 'Week :2' when CWP_WEEK='3' then 'Week :3' when CWP_WEEK='4' then 'Week :4' else '0' end As [Week] , right(convert(varchar, CWP.CWP_DATE, 106), 8) as [Month] into #Temp FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join CUSTOMER_WEEKLY_PLAN CWP ON I.I_CODE=CWP.CWP_I_CODE WHERE " + StrCond + " S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' order by GP_NAME,I_NAME select [Group],	I_CODE,	Part_No,	Part_Name,	case when Sale_Plan<0 then 0 else Sale_Plan end as Sale_Plan,	STD_StockInFG,	Actual_Stock_InFG,	Week,	[Month] from #Temp drop table #Temp";
            /* Firm Customer Weekly plan Union Firm Vendor Weekly plan*/
            //Query = "SELECT * INTO #Temp from (SELECT DISTINCT G.GP_NAME,I.I_CODE, I_CODENO , I_NAME ,isnull((select sum(isnull(CWP_SALE_QTY,0)) from CUSTOMER_WEEKLY_PLAN where CWP_I_CODE=I.I_CODE AND CUSTOMER_WEEKLY_PLAN.ES_DELETE=0 and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND CWP_WEEK=CWP.CWP_WEEK),0) as SALE_PLAN_QTY , ISNULL(SIM_CUST_STOCK,0) as Tot_Stnd_Stock,isnull((select isnull(sum(CWP_CUSTOMER_STOCK),0) from CUSTOMER_WEEKLY_PLAN where CWP_I_CODE=I.I_CODE and CUSTOMER_WEEKLY_PLAN.ES_DELETE=0 and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND CWP_WEEK=CWP.CWP_WEEK),0) AS ActualStockQty ,isnull(SIM_FINISH_GOODS,0) as STD_StockInFG,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as Actual_Stock_InFG,case when CWP_WEEK='1' then 'Week :1' when CWP_WEEK='2' then 'Week :2' when CWP_WEEK='3' then 'Week :3' when CWP_WEEK='4' then 'Week :4' else '0' end As CWP_WEEK , right(convert(varchar, CWP.CWP_DATE, 106), 8) as CWP_DATE FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join CUSTOMER_WEEKLY_PLAN CWP ON I.I_CODE=CWP.CWP_I_CODE WHERE " + StrCond + " CWP_WEEK='" + WeekCond + "' and  PROD.PROD_MACHINE_LOC in('INHOUSE','BOTH') and PROD.ES_DELETE=0 and S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by G.GP_NAME,I.I_CODE, I_CODENO ,CWP_P_CODE, I_NAME,CWP_WEEK,SIM_CUST_STOCK,SIM_FINISH_GOODS,right(convert(varchar, CWP.CWP_DATE, 106), 8)) AS tEMP select * into #Temp1 from( SELECT DISTINCT G.GP_NAME,I.I_CODE, I_CODENO,I_NAME ,isnull((select sum(isnull(VWP_QTY,0)) from VENDOR_WEEKLY_PLAN where VWP_I_CODE=I.I_CODE and VENDOR_WEEKLY_PLAN.ES_DELETE=0 and VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VWP_WEEK=CWP.VWP_WEEK),0) as SALE_PLAN_QTY,ISNULL(SIM_CUST_STOCK,0) as Tot_Stnd_Stock,isnull((select isnull(sum(VWP_VENDOR_STOCK),0) from VENDOR_WEEKLY_PLAN where VWP_I_CODE=I.I_CODE and VENDOR_WEEKLY_PLAN.ES_DELETE=0 and VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VWP_WEEK=CWP.VWP_WEEK),0) AS ActualStockQty ,isnull(SIM_FINISH_GOODS,0) as STD_StockInFG,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as Actual_Stock_InFG,case when VWP_WEEK='1' then 'Week :1' when VWP_WEEK='2' then 'Week :2' when VWP_WEEK='3' then 'Week :3' when VWP_WEEK='4' then 'Week :4' else '0' end As CWP_WEEK,right(convert(varchar, CWP.VWP_DATE, 106), 8) as CWP_DATE FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join VENDOR_WEEKLY_PLAN CWP ON I.I_CODE=CWP.VWP_I_CODE WHERE " + StrCond + " VWP_WEEK='" + WeekCond + "' and PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') and PROD.ES_DELETE=0 and S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CWP.VWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by G.GP_NAME,I.I_CODE, I_CODENO,I_NAME,VWP_WEEK,SIM_CUST_STOCK,right(convert(varchar, CWP.VWP_DATE, 106), 8),SIM_FINISH_GOODS ) as Temp1 select GP_NAME,I_CODE,I_CODENO,I_NAME,case when SALE_PLAN_QTY<0 then 0 else SALE_PLAN_QTY end as SALE_PLAN_QTY,Tot_Stnd_Stock,case when ActualStockQty<0 THEN 0 ELSE ActualStockQty END AS ActualStockQty,CWP_WEEK,CWP_DATE,STD_StockInFG,CASE WHEN Actual_Stock_InFG<0 THEN 0 ELSE Actual_Stock_InFG END AS Actual_Stock_InFG into #Temp2 from #Temp select GP_NAME,I_CODE,I_CODENO,I_NAME,case when SALE_PLAN_QTY<0 then 0 else SALE_PLAN_QTY end as SALE_PLAN_QTY,Tot_Stnd_Stock,case when ActualStockQty<0 THEN 0 ELSE ActualStockQty END AS ActualStockQty,CWP_WEEK,CWP_DATE,STD_StockInFG,CASE WHEN Actual_Stock_InFG<0 THEN 0 ELSE Actual_Stock_InFG END AS Actual_Stock_InFG into #Temp3 from #Temp1 select GP_NAME as [Group],I_CODE,I_CODENO as Part_No ,I_NAME as Part_Name,round(isnull(SALE_PLAN_QTY ,0)-ISNULL(ActualStockQty,0),0) as Sale_Plan,Tot_Stnd_Stock,ActualStockQty,CWP_WEEK as Week,CWP_DATE as [Month],STD_StockInFG,Actual_Stock_InFG from #Temp2 union select GP_NAME as [Group],I_CODE,I_CODENO as Part_No ,I_NAME as Part_Name,round(isnull(SALE_PLAN_QTY ,0) -ISNULL(ActualStockQty,0),0) as Sale_Plan,Tot_Stnd_Stock,ActualStockQty,CWP_WEEK as Week,CWP_DATE as [Month],STD_StockInFG,Actual_Stock_InFG from #Temp3 drop table #Temp drop table #Temp1 drop table #Temp2 drop table #Temp3";
            /* Change Query as Per Deshpande Sir Logic :- Fetch data from Firm Customer Weekly plan and remove round(isnull(SALE_PLAN_QTY ,0)-ISNULL(ActualStockQty,0),0) as Sale_Plan  28072018*/
            Query = "SELECT * INTO #Temp from (SELECT DISTINCT G.GP_NAME,I.I_CODE, I_CODENO , I_NAME ,isnull((select sum(isnull(CWP_SALE_QTY,0)) from CUSTOMER_WEEKLY_PLAN where CWP_I_CODE=I.I_CODE AND CUSTOMER_WEEKLY_PLAN.ES_DELETE=0 and CWP_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' AND CWP_WEEK=CWP.CWP_WEEK),0) as SALE_PLAN_QTY , ISNULL(SIM_CUST_STOCK,0) as Tot_Stnd_Stock,isnull((select isnull(sum(CWP_CUSTOMER_STOCK),0) from CUSTOMER_WEEKLY_PLAN where CWP_I_CODE=I.I_CODE and CUSTOMER_WEEKLY_PLAN.ES_DELETE=0 and CWP_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND CWP_WEEK=CWP.CWP_WEEK),0) AS ActualStockQty ,isnull(SIM_FINISH_GOODS,0) as STD_StockInFG,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483642' AND STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as Actual_Stock_InFG,case when CWP_WEEK='1' then 'Week :1' when CWP_WEEK='2' then 'Week :2' when CWP_WEEK='3' then 'Week :3' when CWP_WEEK='4' then 'Week :4' else '0' end As CWP_WEEK , right(convert(varchar, CWP.CWP_DATE, 106), 8) as CWP_DATE FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE inner join CUSTOMER_WEEKLY_PLAN CWP ON I.I_CODE=CWP.CWP_I_CODE WHERE " + StrCond + " CWP_WEEK='" + WeekCond + "' and PROD.ES_DELETE=0 and S.ES_DELETE=0 AND CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and DATEPART(mm,CWP.CWP_DATE)='" + dtMonth.Month + "'  and DATEPART(yyyy,CWP.CWP_DATE)='" + dtMonth.Year + "' group by G.GP_NAME,I.I_CODE, I_CODENO ,CWP_P_CODE, I_NAME,CWP_WEEK,SIM_CUST_STOCK,SIM_FINISH_GOODS,right(convert(varchar, CWP.CWP_DATE, 106), 8)) AS tEMP select GP_NAME,I_CODE,I_CODENO,I_NAME,case when SALE_PLAN_QTY<0 then 0 else SALE_PLAN_QTY end as SALE_PLAN_QTY,Tot_Stnd_Stock,case when ActualStockQty<0 THEN 0 ELSE ActualStockQty END AS ActualStockQty,CWP_WEEK,CWP_DATE,STD_StockInFG,CASE WHEN Actual_Stock_InFG<0 THEN 0 ELSE Actual_Stock_InFG END AS Actual_Stock_InFG into #Temp2 from #Temp select GP_NAME as [Group],I_CODE,I_CODENO as Part_No ,I_NAME as Part_Name,round(isnull(SALE_PLAN_QTY ,0),0) as Sale_Plan,Tot_Stnd_Stock,ActualStockQty,CWP_WEEK as Week,CWP_DATE as [Month],STD_StockInFG,Actual_Stock_InFG from #Temp2 drop table #Temp  drop table #Temp2 ";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/CastingsReqFromInspection.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/CastingsReqFromInspection.rpt");
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
            Response.Redirect("~/PPC/ReportForms/VIEW/VewCastingrequiredfrominspectionReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
