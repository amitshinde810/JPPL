using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_PurchaseRequirement : System.Web.UI.Page
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
            string StrCond = Request.QueryString[3].ToString();
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
            /* Hardcode check = STL_STORE_TYPE from STOCK_LEDGER FOR Actual Stock(SUM(STL_DOC_QTY) for RFI Store */
            //Query = "SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,ISNULL(CS_SCHEDULE_QTY,0) AS CS_SCHEDULE_QTY ,ISNULL(SIM_RFI_STORE,0) AS Tot_Stnd_Stock ,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483643' AND STL_I_CODE= I.I_CODE),0) AS ActualStockQty FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE WHERE " + StrCond + " S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND G.GP_CODE='" + GroupCode + "' AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO, I_NAME";
            //Query = "SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL(C.CS_SCHEDULE_QTY, 0),0) AS BODQTY_SCHEDULE,isnull(I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE),0) as ActualStockQty,isnull(I_INV_RATE,0) as I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) as I_CODE_BOUGHT_CODE FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE WHERE (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' order by I.I_CODENO + ' - ' + I.I_NAME,BOD_I_BOUGHT_CODE";
           // Query = "SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I_INV_RATE,0) as I_INV_RATE1,(SELECT I1.I_INV_RATE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) as I_CODE_BOUGHT_CODE into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE WHERE " + StrCond + " (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'  select DISTINCT I_CODE,I_CODENO,I_NAME,ICODE_INAME,BOM_CODE,BOD_CODE,BOD_I_BOUGHT_CODE,BOD_QTY,CS_SCHEDULE_QTY,case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE  from #Temp drop table #Temp";
            

            //AS per New Logic suggested by Deepak Survase Sir
            //Query = "SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) - ISNULL((select SUM(ISNULL(CL_CQTY,0)) AS STL_DOC_QTY from CHALLAN_STOCK_LEDGER WHERE CL_I_CODE= I.I_CODE and CL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) ), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I_INV_RATE,0) as I_INV_RATE1,(SELECT I1.I_INV_RATE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) as I_CODE_BOUGHT_CODE into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE WHERE " + StrCond + " (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'  select DISTINCT I_CODE,I_CODENO,I_NAME,ICODE_INAME,BOM_CODE,BOD_CODE,BOD_I_BOUGHT_CODE,BOD_QTY,CS_SCHEDULE_QTY,case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE  from #Temp drop table #Temp";



            //New by removing sub quries
            Query = "SELECT DISTINCT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) - ISNULL((select SUM(ISNULL(CL_CQTY,0)) AS STL_DOC_QTY from CHALLAN_STOCK_LEDGER WHERE CL_I_CODE= I.I_CODE and CL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) ), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(BROUGHOUT.I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I.I_INV_RATE,0) as I_INV_RATE1,   BROUGHOUT.I_INV_RATE AS I_INV_RATE, BROUGHOUT.I_CODE As I_CODE1, BROUGHOUT.I_NAME As I_CODE_BOUGHT_NAME,  BROUGHOUT.I_CODENO As I_CODE_BOUGHT_CODE     into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE  INNER JOIN  ITEM_MASTER AS BROUGHOUT  ON BROUGHOUT.I_CODE=BOD.BOD_I_BOUGHT_CODE WHERE " + StrCond + " (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'  select DISTINCT I_CODE,I_CODENO,I_NAME,ICODE_INAME,BOM_CODE,BOD_CODE,BOD_I_BOUGHT_CODE,BOD_QTY,CS_SCHEDULE_QTY,case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE  from #Temp drop table #Temp";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/PurchaseRequirement.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/PurchaseRequirement.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                //rptname.SetParameterValue("Title", Title);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", Date);
                double LossPer = 0; /* Bind Dynamically*/

                rptname.SetParameterValue("txtLossPer", LossPer);
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
            CommonClasses.SendError("Purchase Requirement Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewPurchaseRequirement.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requirement Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}