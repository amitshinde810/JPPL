using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class PPC_ReportForms_ADD_CastingsrequriedfromrfmforvendorReport : System.Web.UI.Page
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
            Query = "SELECT * into #Temp from(SELECT DISTINCT I.I_CODE,I_CODENO,I_NAME ,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ROUND((ROUND(isnull((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0)) *(CAST(2 as float)/100 +1),0),0) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0))/1000,0)  AS CorestobeMade ,isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483648 AND MBM.MBM_PMN_CODE=-2147483647),0) as [800x600],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483647 AND MBM.MBM_PMN_CODE=-2147483647),0) as [600x450],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483646 AND MBM.MBM_PMN_CODE=-2147483647),0) as [600x300],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483645 AND MBM.MBM_PMN_CODE=-2147483647),0) as [500x350],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483647 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483648),0) AS [800x600_Available],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483647 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483647),0) AS [600x450_Available],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483647 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483646),0) AS [600x300_Available],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483647 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483645),0) AS [500x350_Available] ,ISNULL(PROD_CAST_WT,0) as PROD_CAST_WT,ROUND(isnull((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + sum(ISNULL(SIM_FOUNDRY,0)) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0)) *(CAST(2 as float)/100 +1),0)/1000,4)  AS CastingTobeCast,isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483648 AND MBM.MBM_PMN_CODE=-2147483648),0) as [800x600_Cast],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483644 AND MBM.MBM_PMN_CODE=-2147483648),0) as [1000x800_Cast],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483643 AND MBM.MBM_PMN_CODE=-2147483648),0) as [STAND_ALONE],isnull((select ISNULL(MBD_NO_OF_UNITS,0) AS MBD_NO_OF_UNITS from MACHINE_BOOKING_MASTER MBM inner join MACHINE_BOOKING_DETAIL MBD on MBM.MBM_CODE=MBD.MBD_MBM_CODE WHERE MBM.MBM_I_CODE=I.I_CODE AND MBM.ES_DELETE=0 and MBD_M_CODE=-2147483642 AND MBM.MBM_PMN_CODE=-2147483648),0) as [HORIZONTAL],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483648 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483648),0) AS [800x600_Cast_Available],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483648 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483644),0) AS [1000x800_Cast_Available],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483648 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483643),0) AS [STAND_ALONE_Available],ISNULL((SELECT isnull(PMM_NO_MACHINES,0) as PMM_NO_MACHINES FROM PROCESS_MACHINE_MASTER PMM where PMM_P_ID=-2147483648 and PMM.ES_DELETE=0 and PMM_M_ID=-2147483642),0) AS [HORIZONTAL_Available] FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE  S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by I.I_CODE,I_CODENO,I_NAME,PROD_CAST_WT,PROD_CORE_WT) as Temp select I_CODE,I_CODENO,I_NAME,PROD_CORE_WT,CorestobeMade,case when [800x600]=0 then 0 else round(CorestobeMade/[800x600],2) end as [800x600_Booking] ,case when [600x450]=0 then 0 else round(CorestobeMade/[600x450],2) end as [600x450_Booking] ,case when [600x300]=0 then 0 else round(CorestobeMade/[600x300],2) end as [600x300_Booking] ,case when [500x350]=0 then 0 else round(CorestobeMade/[500x350],2) end as [500x350_Booking],[800x600_Available],[600x450_Available],[600x300_Available],[500x350_Available] ,case when [800x600_Cast]=0 then 0 else round(CastingTobeCast/[800x600_Cast],2) end as [800x600_Cast_Booking] ,case when [1000x800_Cast]=0 then 0 else round(CastingTobeCast/[1000x800_Cast],2) end as [1000x800_Cast_Booking] ,case when [STAND_ALONE]=0 then 0 else round(CastingTobeCast/[STAND_ALONE],2) end as [STAND_ALONE_Booking] ,case when [HORIZONTAL]=0 then 0 else round(CastingTobeCast/[HORIZONTAL],2) end as [HORIZONTAL_Booking],[800x600_Cast_Available],[1000x800_Cast_Available],[STAND_ALONE_Available],[HORIZONTAL_Available] INTO #Temp1 from #Temp drop table #Temp select sum((isnull([800x600_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [800x600_Machine_Req] ,sum((isnull([600x450_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [600x450_Machine_Req] ,sum((isnull([600x300_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [600x300_Machine_Req] ,sum((isnull([500x350_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [500x350_Machine_Req] ,sum(isnull([800x600_Available],0)) as [800x600_Available],sum(isnull([600x450_Available] ,0)) as [600x450_Available],sum(isnull([600x300_Available],0))as [600x300_Available],sum(isnull([500x350_Available],0)) as [500x350_Available],(sum((isnull([800x600_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([800x600_Available],0))) as [800x600_Short_Excess] ,(sum((isnull([600x450_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([600x450_Available] ,0))) as [600x450_Short_Excess] ,(sum((isnull([600x300_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([600x300_Available],0))) as [600x300_Short_Excess] ,(sum((isnull([500x350_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([500x350_Available],0))) as [500x350_Short_Excess] ,sum((isnull([800x600_Cast_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [800x600_Cast_Machine_Req] ,sum((isnull([1000x800_Cast_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [1000x800_Cast_Machine_Req] ,sum((isnull([STAND_ALONE_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [STAND_ALONE_Machine_Req] ,sum((isnull([HORIZONTAL_Booking],0)) *1.15)/" + NoofDaysAvailable + " as [HORIZONTAL_Machine_Req] ,sum(isnull([800x600_Cast_Available],0)) as [800x600_Cast_Available],sum(isnull([1000x800_Cast_Available] ,0)) as [1000x800_Cast_Available],sum(isnull([STAND_ALONE_Available],0))as [STAND_ALONE_Available],sum(isnull([HORIZONTAL_Available],0)) as [HORIZONTAL_Available],(sum((isnull([800x600_Cast_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([800x600_Cast_Available],0))) as [800x600_Cast_Short_Excess] ,(sum((isnull([1000x800_Cast_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([1000x800_Cast_Available] ,0))) as [1000x800_Cast_Short_Excess] ,(sum((isnull([STAND_ALONE_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([STAND_ALONE_Available],0))) as [STAND_ALONE_Short_Excess] ,(sum((isnull([HORIZONTAL_Booking],0)) *1.15)/" + NoofDaysAvailable + " /sum(isnull([HORIZONTAL_Available],0))) as [HORIZONTAL_Short_Excess] from #Temp1 drop table #Temp1";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/rptCastReqFromRFMForVendor.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/rptCastReqFromRFMForVendor.rpt");
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
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewCastingsrequriedfromrfmforvendorReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

