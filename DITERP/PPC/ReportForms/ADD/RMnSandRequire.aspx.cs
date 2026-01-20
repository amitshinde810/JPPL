using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_RMnSandRequire : System.Web.UI.Page
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
            // Hardcode check = STL_STORE_TYPE from STOCK_LEDGER FOR Actual Stock(SUM(STL_DOC_QTY) for FOUNDRY(castingtocast Total)and coretobemade
            //Query = "SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,sum(ISNULL(CS_SCHEDULE_QTY,0)) AS CS_SCHEDULE_QTY,sum(ISNULL(SIM_FOUNDRY,0)) AS CoretobMade ,ISNULL((select ISNULL(PROD_CAST_WT,0) from PRODUCT_MASTER WHERE PROD_I_CODE=I.I_CODE AND PROD_RAW_TYPE='AC4B' and ES_DELETE=0) * sum(ISNULL(CS_SCHEDULE_QTY,0)),0) as AC4BQty,ISNULL((select ISNULL(PROD_CAST_WT,0) from PRODUCT_MASTER WHERE PROD_I_CODE=I.I_CODE AND PROD_RAW_TYPE='LM25' and ES_DELETE=0) * sum(ISNULL(CS_SCHEDULE_QTY,0)),0) as LM25Qty,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0) AS ActualStockQty  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE WHERE " + StrCond + " S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND G.GP_CODE='" + GroupCode + "' AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by  I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ORDER BY I_CODENO, I_NAME";
            //Query = "SELECT DISTINCT I.I_CODE,PROD.PROD_RAW_TYPE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE),0)) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0),0)  AS CastingTobeCast ,ROUND((ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE),0)) *isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0),0) + ISNULL(SIM_FOUNDRY,0) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0)),0) AS CorestobeMade FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE  S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO, I_NAME,PROD.PROD_RAW_TYPE ";
            //Query = "SELECT DISTINCT I.I_CODE,PROD.PROD_RAW_TYPE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0),0)  AS CastingTobeCast ,0 AS CorestobeMade FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE  S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO, I_NAME,PROD.PROD_RAW_TYPE ";

            //As per Discussion with Deepak Surevase added Vendor Stock as 02-01-19
            Query = "SELECT DISTINCT I.I_CODE,PROD.PROD_RAW_TYPE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) -ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)- ISNULL((select SUM(ISNULL(CL_CQTY,0)) AS STL_DOC_QTY from CHALLAN_STOCK_LEDGER WHERE CL_I_CODE= I.I_CODE and CL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0),0)  AS CastingTobeCast ,0 AS CorestobeMade FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE  S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO, I_NAME,PROD.PROD_RAW_TYPE ";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/RMnSandRequire.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/RMnSandRequire.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Title", Title);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", Date);
                double LossPer = 1.005; /* Bind Dynamically*/
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
            CommonClasses.SendError("Casting ToBe Machined Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewCastingtobeMachined.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}