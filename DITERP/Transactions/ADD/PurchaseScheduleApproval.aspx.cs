using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Transactions_ADD_PurchaseScheduleApproval : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int IM_No = 0;
    DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();


    MaterialInspection_BL inspect_BL = new MaterialInspection_BL();
    static DataTable inspectiondt = new DataTable();
    static DataTable dt = new DataTable();
    static string type = "";
    public int icode;
    static int To_storeCode = 0;
    static int TRANS_TYPE = 0;
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {

                    ViewState["type"] = type;
                    ViewState["type"] = Request.QueryString[0].ToString();
                    txtInspDate.Attributes.Add("readonly", "readonly");
                    txtInspDate.Text = System.DateTime.Now.ToString("MMM/yyyy");
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["type"] = Request.QueryString[1].ToString();
                        txtInspDate.Text = Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy");
                        txtInspDate.Enabled = false;
                        dgMaterialAcceptance.Enabled = false;
                        txtTotal.Enabled = false;
                        getScheduleDetail();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["type"] = Request.QueryString[1].ToString();
                        txtInspDate.Text = Convert.ToDateTime(ViewState["type"]).ToString("MMM/yyyy");
                        txtInspDate.Enabled = true;
                        getScheduleDetail();
                        CommonClasses.Execute1("UPDATE PURCHASESCHEDULE_APPROVAL  SET MODIFY=1 where PA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        getSchedule();
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-ADD", "Page_Load", Ex.Message);
        }
    }
    #endregion

    public void getScheduleDetail()
    {
        DateTime dtMonth = new DateTime();
        dtMonth = Convert.ToDateTime(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy"));
        DataTable dtraw = new DataTable();
        //dtraw = CommonClasses.Execute(" SELECT PA_QTY AS PA_QTY_PPC,PA_QTY_PPC  AS QTY ,PA_RATE AS I_INV_RATE,PA_AMT AS AMT,I_CODE,I_CODENO,I_NAME   FROM PURCHASESCHEDULE_APPROVAL ,ITEM_MASTER where PA_I_CODE=I_CODE AND PURCHASESCHEDULE_APPROVAL.ES_DELETE=0 AND PA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "'");

        //dtraw = CommonClasses.Execute(" SELECT PA_QTY AS PA_QTY_PPC,PA_QTY_PPC  AS QTY ,PA_RATE AS I_INV_RATE,PA_AMT AS AMT,I_CODE,I_CODENO,I_NAME ,ISNULL(( SELECT  SUM(IWD_SQTY) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND DATEPART(MM,IWM_DATE)='" + dtMonth.Month + "' AND  DATEPART(YYYY,IWM_DATE)='" + dtMonth.Year + "'  AND IWD_I_CODE=I_CODE),2)  AS IWD_QTY   FROM PURCHASESCHEDULE_APPROVAL ,ITEM_MASTER where PA_I_CODE=I_CODE AND PURCHASESCHEDULE_APPROVAL.ES_DELETE=0 AND PA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "'");
        dtraw = CommonClasses.Execute("SELECT round(PA_QTY,2) AS PA_QTY_PPC,round(PA_QTY_PPC,2)  AS QTY ,PA_RATE AS I_INV_RATE,PA_AMT AS AMT,I_CODE,I_CODENO,I_NAME ,round(ISNULL(( SELECT  SUM(IWD_SQTY) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND DATEPART(MM,IWM_DATE)='" + dtMonth.Month + "' AND  DATEPART(YYYY,IWM_DATE)='" + dtMonth.Year + "'  AND IWD_I_CODE=I_CODE),0),2)  AS IWD_QTY  INTO #PA FROM PURCHASESCHEDULE_APPROVAL ,ITEM_MASTER where PA_I_CODE=I_CODE AND PURCHASESCHEDULE_APPROVAL.ES_DELETE=0 AND PA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "' SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I_INV_RATE,0) as I_INV_RATE1,(SELECT I1.I_INV_RATE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) as I_CODE_BOUGHT_CODE into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE WHERE (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'   select DISTINCT  case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE   into #temp1 from #Temp SELECT ((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) as PA_QTY_PPC,((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty )	AS QTY,I_INV_RATE,(((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) *I_INV_RATE) AS AMT,	I_CODE1 AS I_CODE,	I_CODE_BOUGHT_CODE  AS I_CODENO ,I_CODE_BOUGHT_NAME AS I_NAME,	0 AS IWD_QTY INTO #PANOTIN FROM #temp1 where #temp1.I_CODE1 not in(select PA_I_CODE from PURCHASESCHEDULE_APPROVAL where PURCHASESCHEDULE_APPROVAL.ES_DELETE=0 AND PA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "') 	 GROUP BY Tot_Stnd_Stock,	ActualStockQty	,I_INV_RATE,	I_CODE1	,I_CODE_BOUGHT_NAME,	I_CODE_BOUGHT_CODE   HAVING  (((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ))>0 SELECT * FROM #PA UNION SELECT * FROM #PANOTIN drop table #Temp drop table #Temp1 DROP TABLE #PANOTIN DROP TABLE #PA");
        // DataTable dtnotunion = CommonClasses.Execute("SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I_INV_RATE,0) as I_INV_RATE1,(SELECT I1.I_INV_RATE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) as I_CODE_BOUGHT_CODE into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE WHERE (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'   select DISTINCT  case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE   into #temp1 from #Temp SELECT ((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) as PA_QTY_PPC,((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty )	AS QTY,I_INV_RATE,(((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) *I_INV_RATE) AS AMT,	I_CODE1 AS I_CODE,	I_CODE_BOUGHT_CODE  AS I_CODENO ,I_CODE_BOUGHT_NAME AS I_NAME,	0 AS IWD_QTY  FROM #temp1 where #temp1.I_CODE1 not in(select PA_I_CODE from PURCHASESCHEDULE_APPROVAL where PURCHASESCHEDULE_APPROVAL.ES_DELETE=0 AND PA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "') 	 GROUP BY Tot_Stnd_Stock,	ActualStockQty	,I_INV_RATE,	I_CODE1	,I_CODE_BOUGHT_NAME,	I_CODE_BOUGHT_CODE   HAVING  (((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ))>0  drop table #Temp drop table #Temp1");
        if (dtraw.Rows.Count > 0)
        {
            dgMaterialAcceptance.DataSource = dtraw;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;
            double sum = 0;
            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {
                sum += Convert.ToDouble(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text);
            }
            txtTotal.Text = Convert.ToDouble(sum).ToString();
        }
        else
        {
            DataTable dtfill = new DataTable();
            dtfill.Columns.Add("I_CODE");
            dtfill.Columns.Add("I_CODENO");//change Binding Value Name From ItemCode1 to ItemCodeNo
            dtfill.Columns.Add("I_NAME");
            dtfill.Columns.Add("QTY");
            dtfill.Columns.Add("PA_QTY_PPC");//change Binding Value Name From StockUOM1 to UOM_CODE
            dtfill.Columns.Add("I_INV_RATE");
            dtfill.Columns.Add("AMT");
            dtfill.Columns.Add("IWD_QTY");
            dtfill.Rows.Add(dtfill.NewRow());

            dgMaterialAcceptance.DataSource = dtfill;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;
        }
    }

    protected void txtInspDate_TextChanged(object sender, EventArgs e)
    {

        getSchedule();
    }

    public void getSchedule()
    {
        string StrCond = "";
        DateTime dtMonth = new DateTime();
        DataTable dtraw = new DataTable(); DataTable dtAll = new DataTable();
        dtMonth = Convert.ToDateTime(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy"));
       // inspectiondt = CommonClasses.Execute("SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I_INV_RATE,0) as I_INV_RATE1,(SELECT I1.I_INV_RATE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = 1)) as I_CODE_BOUGHT_CODE into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE WHERE " + StrCond + " (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'   select DISTINCT  case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE   into #temp1 from #Temp SELECT ((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty )	AS QTY,((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) as PA_QTY_PPC,I_INV_RATE,(((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) *I_INV_RATE) AS AMT,	I_CODE1 AS I_CODE,	I_CODE_BOUGHT_NAME AS I_NAME,	I_CODE_BOUGHT_CODE  AS I_CODENO ,0 AS IWD_QTY  FROM #temp1  	 GROUP BY Tot_Stnd_Stock,	ActualStockQty	,I_INV_RATE,	I_CODE1	,I_CODE_BOUGHT_NAME,	I_CODE_BOUGHT_CODE   HAVING  (((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ))>0  drop table #Temp drop table #Temp1");

        //added std inv of raw material
        inspectiondt = CommonClasses.Execute("SELECT  DISTINCT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)  - ISNULL((select SUM(ISNULL(CL_CQTY,0)) AS STL_DOC_QTY from CHALLAN_STOCK_LEDGER WHERE CL_I_CODE= I.I_CODE and CL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)), 0)* isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='1')),0),0) AS BODQTY_SCHEDULE,isnull(BOUGHTOUT.I_MIN_LEVEL,0) as Tot_Stnd_Stock, isnull((select SUM(isnull(STL_DOC_QTY,0)) from STOCK_LEDGER where STL_I_CODE=BOD.BOD_I_BOUGHT_CODE and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0) as ActualStockQty,isnull(I.I_INV_RATE,0) as I_INV_RATE1,BOUGHTOUT.I_INV_RATE, BOUGHTOUT.I_CODE AS I_CODE1,  BOUGHTOUT.I_NAME AS I_CODE_BOUGHT_NAME, BOUGHTOUT.I_CODENO as I_CODE_BOUGHT_CODE into #Temp FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE inner join STANDARD_INVENTARY_MASTER S  on C.CS_I_CODE=S.SIM_I_CODE   INNER JOIN ITEM_MASTER AS BOUGHTOUT ON BOD.BOD_I_BOUGHT_CODE = BOUGHTOUT.I_CODE  WHERE " + StrCond + " (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and C.CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'   select DISTINCT  case when BODQTY_SCHEDULE<0 then 0 else round(BODQTY_SCHEDULE,0) end as BODQTY_SCHEDULE,Tot_Stnd_Stock,ActualStockQty,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE   into #temp1 from #Temp SELECT ((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty )	AS QTY,((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) as PA_QTY_PPC,I_INV_RATE,(((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ) *I_INV_RATE) AS AMT,	I_CODE1 AS I_CODE,	I_CODE_BOUGHT_NAME AS I_NAME,	I_CODE_BOUGHT_CODE  AS I_CODENO ,0 AS IWD_QTY  FROM #temp1  	 GROUP BY Tot_Stnd_Stock,	ActualStockQty	,I_INV_RATE,	I_CODE1	,I_CODE_BOUGHT_NAME,	I_CODE_BOUGHT_CODE   HAVING  (((SUM(BODQTY_SCHEDULE)  +	Tot_Stnd_Stock)-ActualStockQty ))>0  drop table #Temp drop table #Temp1");


        DateTime dtOpenDate = Convert.ToDateTime(Session["CompanyOpeningDate"]);
        DateTime CurrDate = DateTime.Now.Date;
        var firstDayOfMonth = new DateTime(CurrDate.Year, CurrDate.Month, 1);


        DL_DBAccess = new DatabaseAccessLayer();

        SqlParameter[] par1 = new SqlParameter[4];
        par1[0] = new SqlParameter("@CompanyId", Session["CompanyId"].ToString());
        par1[1] = new SqlParameter("@OpeningDate", Convert.ToDateTime(dtMonth).ToString("01/MMM/yyyy"));
        par1[2] = new SqlParameter("@month", (dtMonth).ToString("MM"));
        par1[3] = new SqlParameter("@year",  (dtMonth).ToString("yyyy"));
        dtraw = DL_DBAccess.SelectData("PurchaseSchedule", par1);


        //dtraw = CommonClasses.Execute("select * into #TempSRSM_AC4B from(select round(ISNULL(SRSM_AC4B,0),0) as SRSM_AC4B from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0) AS Std_Stock1 select * into #TempTurning_Ingot from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Turning_Ingot FROM STOCK_LEDGER WHERE STL_I_CODE='-2147481793' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "') AS Turning_Ingot select * into #TempIngots from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Ingots FROM STOCK_LEDGER WHERE STL_I_CODE='-2147479674' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Ingots select * into #TempRunner_Riser from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Runner_Riser FROM STOCK_LEDGER  WHERE STL_I_CODE='-2147479679' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' )AS Runner_Riser select * into #TempSRSM_LM25 from(select ISNULL(SRSM_LM25,0) as SRSM_LM25 from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0) AS Std_Stock2 select * into #TempTurning_IngotLM from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Turning_IngotLM FROM STOCK_LEDGER  WHERE STL_I_CODE='-2147481791' and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Turning_Ingot select * into #TempIngotsLM from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS IngotsLM FROM STOCK_LEDGER WHERE STL_I_CODE='-2147479673' and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Ingots select * into #TempRunner_RiserLM from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Runner_RiserLM FROM STOCK_LEDGER WHERE STL_I_CODE='-2147479680' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Runner_Riser select * into #TempLossPer from(select ISNULL(RM_MELTING_LOSS,0) as LossPer from REJECTION_MASTER R WHERE RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND R.ES_DELETE=0 AND RM_DATE<='" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS LossPer  select * into #Temp from(SELECT  PROD.PROD_RAW_TYPE,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)  -ISNULL((select SUM(ISNULL(CL_CQTY,0)) AS STL_DOC_QTY from CHALLAN_STOCK_LEDGER WHERE CL_I_CODE= I.I_CODE and CL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0),0) AS ProdPlan,ROUND(ISNULL((select ISNULL(SRSM_AC4B,0) from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0),0),2) AS Std_Stock,ROUND(ISNULL((SELECT SUM(ISNULL(Turning_Ingot,0)) AS Turning_Ingot FROM #TempTurning_Ingot )/1000,0),2) AS Turning_Ingot,ROUND(ISNULL((SELECT SUM(ISNULL(Ingots,0)) AS Ingots FROM #TempIngots)/1000,0),2) AS Ingots,ROUND(ISNULL((SELECT SUM(ISNULL(Runner_Riser,0)) AS Runner_Riser FROM #TempRunner_Riser )/1000,0),2) AS Runner_Riser,0 AS SAND ,round(ISNULL((select ISNULL(RM_MELTING_LOSS,0) from REJECTION_MASTER R WHERE RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND R.ES_DELETE=0 AND RM_DATE<='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0),2) AS LossPer FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' and PROD.PROD_RAW_TYPE='AC4B' ) as Temp1 select * into #Temp2 from(SELECT DISTINCT  PROD.PROD_RAW_TYPE,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE),0)) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0),0) AS ProdPlan,round(ISNULL((select ISNULL(SRSM_LM25,0) from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0)/1000,0),2) AS Std_Stock,ROUND(ISNULL((SELECT SUM(ISNULL(Turning_IngotLM,0)) AS Turning_IngotLM FROM #TempTurning_IngotLM)/1000,0),2) AS Turning_Ingot,ROUND(ISNULL((SELECT SUM(ISNULL(IngotsLM,0)) AS STL_DOC_QTY FROM #TempIngotsLM)/1000,0),2) AS Ingots,ROUND(ISNULL((SELECT SUM(ISNULL(Runner_RiserLM,0)) AS Runner_RiserLM FROM #TempRunner_RiserLM)/1000,0),2) AS Runner_Riser,0 AS SAND,ROUND(ISNULL((select ISNULL(RM_MELTING_LOSS,0) from REJECTION_MASTER R WHERE RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND R.ES_DELETE=0 AND RM_DATE<='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0),2) AS LossPer FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' and PROD.PROD_RAW_TYPE='LM25' ) as Temp2 select DISTINCT PROD_RAW_TYPE,	PROD_CORE_WT,	PROD_CAST_WT,	case when ProdPlan<0 then 0 else round(isnull(ProdPlan,0)*isnull(PROD_CAST_WT,0)/1000,2) end as ProdPlan,	Std_Stock,	 case when Turning_Ingot<0 then 0 else Turning_Ingot end as Turning_Ingot,case when Ingots<0 then 0 else Ingots end as Ingots,case when Runner_Riser<0 then 0 else Runner_Riser end as Runner_Riser,case when	SAND<0 then 0 else SAND end as SAND,case when LossPer<0 then 0 else LossPer end as LossPer into #Temp3 from #Temp select DISTINCT PROD_RAW_TYPE,	PROD_CORE_WT,	PROD_CAST_WT,	case when ProdPlan<0 then 0 else round(isnull(ProdPlan,0)*isnull(PROD_CAST_WT,0)/1000,2) end as ProdPlan,	Std_Stock,	 case when Turning_Ingot<0 then 0 else Turning_Ingot end as Turning_Ingot,case when Ingots<0 then 0 else Ingots end as Ingots,case when Runner_Riser<0 then 0 else Runner_Riser end as Runner_Riser,case when	SAND<0 then 0 else SAND end as SAND,case when LossPer<0 then 0 else LossPer end as LossPer into #Temp4 from #Temp2   select PROD_RAW_TYPE,round(SUM(ProdPlan),0) as ProdPlan, isnull((select isnull(sum(SRSM_AC4B),0) from #TempSRSM_AC4B),0) AS Std_Stock,case when (round(isnull((select isnull(Turning_Ingot,0) from #TempTurning_Ingot),0)/1000,2))<0 then 0 else  round(isnull((select isnull(Turning_Ingot,0) from #TempTurning_Ingot),0)/1000,2) end AS Turning_Ingot,case when (round(isnull((select isnull(Ingots,0) from #TempIngots),0)/1000,2))<0 then 0 else round(isnull((select isnull(Ingots,0) from #TempIngots),0)/1000,2) end AS Ingots,sum(isnull(SAND,0)) AS SAND,case when (round(isnull((select isnull(Runner_Riser,0) from #TempRunner_Riser),0)/1000,2)) <0 then 0 else round(isnull((select isnull(Runner_Riser,0) from #TempRunner_Riser),0)/1000,2) end AS Runner_Riser,0 as Total,0 as With_Melting_Loss ,0 as Material_Purchase,LossPer  into #TempB from #Temp3 group by PROD_RAW_TYPE,LossPer   select PROD_RAW_TYPE,round(SUM(ProdPlan),0) as ProdPlan,isnull((select isnull(sum(SRSM_LM25),0) from #TempSRSM_LM25),0) AS Std_Stock,case when ( round(isnull((select isnull(sum(Turning_IngotLM),0) from #TempTurning_IngotLM),0)/1000,2)) <0 then 0 else round(isnull((select isnull(sum(Turning_IngotLM),0) from #TempTurning_IngotLM),0)/1000,2) end AS Turning_Ingot,case when (round(isnull((select isnull(sum(IngotsLM),0) from #TempIngotsLM),0)/1000,2)) <0 then 0 else round(isnull((select isnull(sum(IngotsLM),0) from #TempIngotsLM),0)/1000,2) end AS Ingots,sum(isnull(SAND,0)) AS SAND,case when (round(isnull((select isnull(sum(Runner_RiserLM),0) from #TempRunner_RiserLM),0)/1000,2))<0 then 0 else round(isnull((select isnull(sum(Runner_RiserLM),0) from #TempRunner_RiserLM),0)/1000,2) end AS Runner_Riser,sum(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)) as Total,round(sum(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)*LossPer/100),2) as With_Melting_Loss ,sum(isnull(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)+ round(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)*LossPer/100,2),0)) as Material_Purchase,LossPer    into #TempA  from #Temp4   group by PROD_RAW_TYPE,LossPer   SELECT           '-2147481794' AS I_CODE,PROD_RAW_TYPE,	(ProdPlan+	Std_Stock-	Turning_Ingot-	Ingots	-SAND-	Runner_Riser)+((ProdPlan+	Std_Stock-	Turning_Ingot-	Ingots	-SAND-	Runner_Riser)*			LossPer/100) AS QTY into #TempC FROM   #TempB    SELECT   '-2147481791' AS I_CODE, PROD_RAW_TYPE,	(ProdPlan+	Std_Stock-	Turning_Ingot-	Ingots	-SAND-	Runner_Riser)+((ProdPlan+	Std_Stock-	Turning_Ingot-	Ingots	-SAND-	Runner_Riser)* LossPer/100) AS QTY  into #TempD FROM   #TempA   SELECT ITEM_MASTER.I_CODE,I_CODENO,I_NAME,CASE WHEN QTY<0 then 0 ELSE QTY*1000 END AS QTY,CASE WHEN QTY<0 then 0 ELSE QTY*1000 END AS PA_QTY_PPC,I_INV_RATE ,ROUND((CASE WHEN QTY<0 then 0 ELSE QTY*1000 END)*I_INV_RATE,2) AS AMT , 0 AS IWD_QTY FROM #TempC,ITEM_MASTER where #TempC.I_CODE=ITEM_MASTER.I_CODE  UNION SELECT ITEM_MASTER.I_CODE,I_CODENO,I_NAME, CASE WHEN QTY<0 then 0 ELSE QTY*1000 END AS QTY,CASE WHEN QTY<0 then 0 ELSE QTY*1000 END AS PA_QTY_PPC,I_INV_RATE ,ROUND((CASE WHEN QTY<0 then 0 ELSE QTY*1000 END)*I_INV_RATE,2) AS AMT  ,0 AS IWD_QTY  FROM #TempD,ITEM_MASTER where #TempD.I_CODE=ITEM_MASTER.I_CODE        UNION SELECT I_CODE,I_CODENO,I_NAME,0 AS QTY,0 AS  PA_QTY_PPC,I_INV_RATE,0 AS AMT,0 AS IWD_QTY  FROM ITEM_MASTER WHERE   ES_DELETE=0 AND I_CODE  IN  (-2147482503,-2147481792,-2147481790,-2147479682,-2147479681,-2147477100,-2147473273) DROP table #TempC DROP table #TempD DROP table #TempA DROP table #TempB     drop table #Temp drop table #Temp2 drop table #Temp3 drop table #Temp4 drop table #TempSRSM_AC4B drop table #TempSRSM_LM25 drop table #TempTurning_Ingot drop table #TempTurning_IngotLm drop table #TempIngots drop table #TempIngotsLm drop table #TempRunner_Riser drop table #TempRunner_RiserLM drop table #TempLossPer");
        
        //dtraw = CommonClasses.Execute("select * into #TempSRSM_AC4B from(select round(ISNULL(SRSM_AC4B,0),0) as SRSM_AC4B from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0) AS Std_Stock1 select * into #TempTurning_Ingot from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Turning_Ingot FROM STOCK_LEDGER WHERE STL_I_CODE='-2147481793' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "') AS Turning_Ingot select * into #TempIngots from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Ingots FROM STOCK_LEDGER WHERE STL_I_CODE='-2147479674' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Ingots select * into #TempRunner_Riser from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Runner_Riser FROM STOCK_LEDGER  WHERE STL_I_CODE='-2147479679' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' )AS Runner_Riser select * into #TempSRSM_LM25 from(select ISNULL(SRSM_LM25,0) as SRSM_LM25 from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0) AS Std_Stock2 select * into #TempTurning_IngotLM from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Turning_IngotLM FROM STOCK_LEDGER  WHERE STL_I_CODE='-2147481791' and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Turning_Ingot select * into #TempIngotsLM from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS IngotsLM FROM STOCK_LEDGER WHERE STL_I_CODE='-2147479673' and STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Ingots select * into #TempRunner_RiserLM from(SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS Runner_RiserLM FROM STOCK_LEDGER WHERE STL_I_CODE='-2147479680' AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS Runner_Riser select * into #TempLossPer from(select ISNULL(RM_MELTING_LOSS,0) as LossPer from REJECTION_MASTER R WHERE RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND R.ES_DELETE=0 AND RM_DATE<='" + dtMonth.ToString("dd/MMM/yyyy") + "' ) AS LossPer  select * into #Temp from(SELECT  PROD.PROD_RAW_TYPE,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE AND STL_DOC_DATE<'" + dtMonth.ToString("dd/MMM/yyyy") + "'),0)   ) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0),0) AS ProdPlan,ROUND(ISNULL((select ISNULL(SRSM_AC4B,0) from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0),0),2) AS Std_Stock,ROUND(ISNULL((SELECT SUM(ISNULL(Turning_Ingot,0)) AS Turning_Ingot FROM #TempTurning_Ingot )/1000,0),2) AS Turning_Ingot,ROUND(ISNULL((SELECT SUM(ISNULL(Ingots,0)) AS Ingots FROM #TempIngots)/1000,0),2) AS Ingots,ROUND(ISNULL((SELECT SUM(ISNULL(Runner_Riser,0)) AS Runner_Riser FROM #TempRunner_Riser )/1000,0),2) AS Runner_Riser,0 AS SAND ,round(ISNULL((select ISNULL(RM_MELTING_LOSS,0) from REJECTION_MASTER R WHERE RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND R.ES_DELETE=0 AND RM_DATE<='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0),2) AS LossPer FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' and PROD.PROD_RAW_TYPE='AC4B' ) as Temp1 select * into #Temp2 from(SELECT DISTINCT  PROD.PROD_RAW_TYPE,ISNULL(PROD_CORE_WT,0) as PROD_CORE_WT,ISNULL(PROD_CAST_WT,0) AS PROD_CAST_WT,ROUND(isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0 and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0)),0) - ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE),0)) * isnull((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')),0),0),0) AS ProdPlan,round(ISNULL((select ISNULL(SRSM_LM25,0) from SAND_RAW_STANDARD_MASTER where SAND_RAW_STANDARD_MASTER.ES_DELETE=0)/1000,0),2) AS Std_Stock,ROUND(ISNULL((SELECT SUM(ISNULL(Turning_IngotLM,0)) AS Turning_IngotLM FROM #TempTurning_IngotLM)/1000,0),2) AS Turning_Ingot,ROUND(ISNULL((SELECT SUM(ISNULL(IngotsLM,0)) AS STL_DOC_QTY FROM #TempIngotsLM)/1000,0),2) AS Ingots,ROUND(ISNULL((SELECT SUM(ISNULL(Runner_RiserLM,0)) AS Runner_RiserLM FROM #TempRunner_RiserLM)/1000,0),2) AS Runner_Riser,0 AS SAND,ROUND(ISNULL((select ISNULL(RM_MELTING_LOSS,0) from REJECTION_MASTER R WHERE RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND R.ES_DELETE=0 AND RM_DATE<='" + dtMonth.ToString("dd/MMM/yyyy") + "' ),0),2) AS LossPer FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND PROD.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and datepart(mm,CS_DATE)='" + dtMonth.ToString("MM") + "' and datepart(yyyy,CS_DATE)='" + dtMonth.ToString("yyyy") + "' and PROD.PROD_RAW_TYPE='LM25' ) as Temp2 select DISTINCT PROD_RAW_TYPE,	PROD_CORE_WT,	PROD_CAST_WT,	case when ProdPlan<0 then 0 else round(isnull(ProdPlan,0)*isnull(PROD_CAST_WT,0)/1000,2) end as ProdPlan,	Std_Stock,	 case when Turning_Ingot<0 then 0 else Turning_Ingot end as Turning_Ingot,case when Ingots<0 then 0 else Ingots end as Ingots,case when Runner_Riser<0 then 0 else Runner_Riser end as Runner_Riser,case when	SAND<0 then 0 else SAND end as SAND,case when LossPer<0 then 0 else LossPer end as LossPer into #Temp3 from #Temp select DISTINCT PROD_RAW_TYPE,	PROD_CORE_WT,	PROD_CAST_WT,	case when ProdPlan<0 then 0 else round(isnull(ProdPlan,0)*isnull(PROD_CAST_WT,0)/1000,2) end as ProdPlan,	Std_Stock,	 case when Turning_Ingot<0 then 0 else Turning_Ingot end as Turning_Ingot,case when Ingots<0 then 0 else Ingots end as Ingots,case when Runner_Riser<0 then 0 else Runner_Riser end as Runner_Riser,case when	SAND<0 then 0 else SAND end as SAND,case when LossPer<0 then 0 else LossPer end as LossPer into #Temp4 from #Temp2   select PROD_RAW_TYPE,round(SUM(ProdPlan),0) as ProdPlan, isnull((select isnull(sum(SRSM_AC4B),0) from #TempSRSM_AC4B),0) AS Std_Stock,case when (round(isnull((select isnull(Turning_Ingot,0) from #TempTurning_Ingot),0)/1000,2))<0 then 0 else  round(isnull((select isnull(Turning_Ingot,0) from #TempTurning_Ingot),0)/1000,2) end AS Turning_Ingot,case when (round(isnull((select isnull(Ingots,0) from #TempIngots),0)/1000,2))<0 then 0 else round(isnull((select isnull(Ingots,0) from #TempIngots),0)/1000,2) end AS Ingots,sum(isnull(SAND,0)) AS SAND,case when (round(isnull((select isnull(Runner_Riser,0) from #TempRunner_Riser),0)/1000,2)) <0 then 0 else round(isnull((select isnull(Runner_Riser,0) from #TempRunner_Riser),0)/1000,2) end AS Runner_Riser,0 as Total,0 as With_Melting_Loss ,0 as Material_Purchase,LossPer from #Temp3 group by PROD_RAW_TYPE,LossPer union select PROD_RAW_TYPE,round(SUM(ProdPlan),0) as ProdPlan,isnull((select isnull(sum(SRSM_LM25),0) from #TempSRSM_LM25),0) AS Std_Stock,case when ( round(isnull((select isnull(sum(Turning_IngotLM),0) from #TempTurning_IngotLM),0)/1000,2)) <0 then 0 else round(isnull((select isnull(sum(Turning_IngotLM),0) from #TempTurning_IngotLM),0)/1000,2) end AS Turning_Ingot,case when (round(isnull((select isnull(sum(IngotsLM),0) from #TempIngotsLM),0)/1000,2)) <0 then 0 else round(isnull((select isnull(sum(IngotsLM),0) from #TempIngotsLM),0)/1000,2) end AS Ingots,sum(isnull(SAND,0)) AS SAND,case when (round(isnull((select isnull(sum(Runner_RiserLM),0) from #TempRunner_RiserLM),0)/1000,2))<0 then 0 else round(isnull((select isnull(sum(Runner_RiserLM),0) from #TempRunner_RiserLM),0)/1000,2) end AS Runner_Riser,sum(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)) as Total,round(sum(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)*LossPer/100),2) as With_Melting_Loss ,sum(isnull(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)+ round(isnull((ProdPlan+Std_Stock)-(Turning_Ingot+Ingots+Runner_Riser+SAND) ,0)*LossPer/100,2),0)) as Material_Purchase,LossPer from #Temp4 group by PROD_RAW_TYPE,LossPer drop table #Temp drop table #Temp2 drop table #Temp3 drop table #Temp4 drop table #TempSRSM_AC4B drop table #TempSRSM_LM25 drop table #TempTurning_Ingot drop table #TempTurning_IngotLm drop table #TempIngots drop table #TempIngotsLm drop table #TempRunner_Riser drop table #TempRunner_RiserLM drop table #TempLossPer");

        //inspectiondt = CommonClasses.Execute(" SELECT I_CODENO	AS [Item Code],I_NAME AS [Item Name],RTF_DOC_NO AS [Document No.]	,RTF_DOC_DATE AS [Document Date]  ,  'Rejection Store' AS  [From Store] , 'Main Store' AS [To Store],  RTF_QTY AS [Received qty.]    FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_CODE=  '" + IM_No + "' ");
        dtAll = inspectiondt.Copy();
        //DataTable dtotherRaw = new DataTable();
        //dtotherRaw = CommonClasses.Execute("SELECT I_CODE	,I_CODENO	,I_NAME	,0 AS QTY	,0 AS  PA_QTY_PPC,	I_INV_RATE	,0 AS AMT	,0 AS IWD_QTY  FROM ITEM_MASTER where   ES_DELETE=0 AND I_CODE  IN  (-2147482503,-2147481792,-2147481790,-2147479682,-2147479681,-2147477100,-2147473273)");
        //dtAll.Merge(dtotherRaw);

        dtAll.Merge(dtraw);

        if (dtAll.Rows.Count > 0)
        {

            dgMaterialAcceptance.DataSource = dtAll;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;

            double sum = 0;
            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {

                sum += Convert.ToDouble(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text);
            }
            txtTotal.Text = Convert.ToDouble(sum).ToString();
        }
        else
        {
            DataTable dtfill = new DataTable();


            dtfill.Columns.Add("I_CODE");
            dtfill.Columns.Add("I_CODENO");//change Binding Value Name From ItemCode1 to ItemCodeNo
            dtfill.Columns.Add("I_NAME");
            dtfill.Columns.Add("QTY");//change Binding Value Name From StockUOM1 to UOM_CODE
            dtfill.Columns.Add("PA_QTY_PPC");
            dtfill.Columns.Add("I_INV_RATE");
            dtfill.Columns.Add("AMT");
            dtfill.Columns.Add("IWD_QTY");
            dtfill.Rows.Add(dtfill.NewRow());

            dgMaterialAcceptance.DataSource = dtfill;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;
        }

    }

    #region lblOK_Qty_TextChanged
    protected void lblOK_Qty_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion lblOK_Qty_TextChanged



    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtInspDate.Attributes.Add("readonly", "readonly");

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region getInfo
    private bool getInfo(string str)
    {
        bool flag = false;
        try
        {
            if (str == "VIEW" || str == "MOD")
            {
                dt = CommonClasses.Execute("select I_CODENO,IWM_TYPE,I_CURRENT_BAL,INSM_CODE,INSM_REMARK,convert(varchar,INSM_DATE,113) as INSM_DATE,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME,ISNULL(INSM_PDR_CHECK,0) AS INSM_PDR_CHECK,INSM_PDR_NO,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER,INSPECTION_S_MASTER where INSPECTION_S_MASTER.ES_DELETE=0 AND INSM_IWM_CODE=IWM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 AND IWD_I_CODE=INSM_I_CODE  and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND IWD_I_CODE='" + icode + "' and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + IM_No + "' ");
                if (dt.Rows.Count > 0)
                {
                }
            }
            else if (str == "ADD")
            {
                dt = CommonClasses.Execute("select I_CODENO,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE  from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE  AND IWD_I_CODE='" + icode + "' AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + IM_No + "' ");
                if (dt.Rows.Count > 0)
                {

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "getInfo", Ex.Message);
        }
        return flag;
    }
    #endregion

    public bool validation()
    {
        try
        {

            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {
                string ReceivedQty = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIWD_QTY")).Text;
                string OkQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtPA_QTY_PPC")).Text;

                ReceivedQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(ReceivedQty), 3));
                OkQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(OkQty), 3));
                if (Convert.ToDouble(OkQty) < Convert.ToDouble(ReceivedQty))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Purchase Quantity Not Less Than Inward Qty " + ReceivedQty;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtPA_QTY_PPC")).Text = "0";
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtPA_QTY_PPC")).Focus();
                    return false;
                }

            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    #region txtPA_QTY_PPC_TextChanged
    protected void txtPA_QTY_PPC_TextChanged(object sender, EventArgs e)
    {

        bool str = validation();
        if (!str)
        {
            return;
        }
        Calculate();

    }
    #endregion txtPA_QTY_PPC_TextChanged


    #region Calculate
    public void Calculate()
    {
        double sum = 0;
        string totalStr = "";
        string PurchaseQty = "";
        string rate = "";
        string Amount = "";
        for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
        {
            GridViewRow gRow = dgMaterialAcceptance.Rows[i];
            TextBox txtQty = (TextBox)gRow.FindControl("txtPA_QTY_PPC");
            Label lbrate = (Label)gRow.FindControl("lblI_INV_RATE");
            Label lblamt = (Label)gRow.FindControl("lblAMT");
            PurchaseQty = txtQty.Text.Trim();
            rate = lbrate.Text.Trim();
            Amount = lblamt.Text.Trim();

            if (PurchaseQty == "")
            {
                PurchaseQty = "0.00";
            }
            if (rate == "")
            {
                rate = "0.00";
            }
            if (Amount == "")
            {
                Amount = "0.00";
            }
            totalStr = DecimalMasking(PurchaseQty);
            PurchaseQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

            totalStr = DecimalMasking(rate);
            rate = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            Amount = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(PurchaseQty) * Convert.ToDouble(rate)), 2));

            ((Label)gRow.FindControl("lblAMT")).Text = Amount;


            sum += Convert.ToDouble(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text);
            txtTotal.Text = Convert.ToDouble(sum).ToString();
        }



    }
    #endregion


    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (type == "ADD")
            {

            }
            else if (type == "MOD")
            {

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "SaveRec", Ex.Message);
        }
        return result;
    }
    #endregion

    #region ClearFunction
    private void ClearTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Text = String.Empty;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.SelectedIndex = 0;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t = ctrl as CheckBox;

                    if (t != null)
                    {
                        t.Checked = false;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval -ADD", "ClearTextBoxes", Ex.Message);
        }
    }
    #endregion

    #region ViewInspection
    private void ViewInspection(string str, string item_code)
    {
        try
        {
            if (str == "VIEW")
            {

            }
            else if (str == "MOD")
            {

            }
            else if (str == "ADD")
            {

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region Numbering
    int Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(INSM_NO) as INSM_NO from INSPECTION_S_MASTER where ES_DELETE=0 and INSM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
        if (dt.Rows[0]["INSM_NO"] == null || dt.Rows[0]["INSM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["INSM_NO"]) + 1;
        }
        return GenGINNO;
    }
    #endregion

    #region Event

    #region GridEvent

    #region dgMainPO_Deleting
    protected void dgMainPO_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMainPO_SelectedIndexChanged
    protected void dgMainPO_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region dgMainPO_RowCommand
    protected void dgMainPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion


    #region dgMaterialAcceptance_RowDeleting
    protected void dgMaterialAcceptance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMaterialAcceptance_PageIndexChanging
    protected void dgMaterialAcceptance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgMaterialAcceptance.PageIndex = e.NewPageIndex;
        }
        catch (Exception)
        {
        }
    }
    #endregion
    #endregion

    #region ButtonEvent
    #region btnSave_Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            btnSubmit.Attributes.Add("onclick", "javascript:" + btnSubmit.ClientID + ".disabled=true;" + ClientScript.GetPostBackEventReference(btnSubmit, ""));

            bool result = false;
            DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
            string msg = "";

            bool str = validation();
            if (!str)
            {
                return;
            }

            if (Request.QueryString[0].Equals("INSERT"))
            {
                dt = CommonClasses.Execute("Select * FROM PURCHASESCHEDULE_APPROVAL WHERE PA_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and  PA_MONTH='" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "'");
                if (dt.Rows.Count == 0)
                {
                    for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                    {
                        CommonClasses.Execute1(" INSERT INTO PURCHASESCHEDULE_APPROVAL( PA_MONTH, PA_I_CODE, PA_QTY,PA_QTY_PPC, PA_RATE, PA_AMT,PA_CM_CODE)VALUES ( '" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtPA_QTY_PPC")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblQTY")).Text + "' ,'" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_INV_RATE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text + "','" + (string)Session["CompanyCode"].ToString() + "')");

                    }
                    CommonClasses.WriteLog("PURCHASESCHEDULE_APPROVAL", "Save", "PURCHASESCHEDULE_APPROVAL", Convert.ToString(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy")), Convert.ToInt32(0), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Transactions/VIEW/ViewPurchaseScheduleApproval.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Already Exist for Selected Month";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInspDate.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                result = CommonClasses.Execute1("UPDATE PURCHASESCHEDULE_APPROVAL SET ES_DELETE=1 where PA_MONTH='" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "' AND PA_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");//Insert From Store here


                for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                {
                    CommonClasses.Execute1(" INSERT INTO PURCHASESCHEDULE_APPROVAL( PA_MONTH, PA_I_CODE, PA_QTY,PA_QTY_PPC, PA_RATE, PA_AMT,PA_CM_CODE)VALUES ( '" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtPA_QTY_PPC")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblQTY")).Text + "' ,'" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_INV_RATE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text + "','" + (string)Session["CompanyCode"].ToString() + "')");

                }
                CommonClasses.Execute1("UPDATE PURCHASESCHEDULE_APPROVAL  SET MODIFY=0 where PA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

                CommonClasses.WriteLog("PURCHASESCHEDULE_APPROVAL", "Update", "PURCHASESCHEDULE_APPROVAL", Convert.ToString(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy")), Convert.ToInt32(0), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/Transactions/VIEW/ViewPurchaseScheduleApproval.aspx", false);
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "btnSave_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CommonClasses.Execute1("UPDATE PURCHASESCHEDULE_APPROVAL  SET MODIFY=0 where PA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

                Response.Redirect("~/Transactions/View/ViewPurchaseScheduleApproval.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnInspCancel_Click
    protected void btnInspCancel_Click(object sender, EventArgs e)
    {
        //CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
        CommonClasses.Execute1("UPDATE PURCHASESCHEDULE_APPROVAL  SET MODIFY=0 where PA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

        Response.Redirect("~/Transactions/View/ViewPurchaseScheduleApproval.aspx", false);
    }
    #endregion

    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {
                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {

            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion


    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                CommonClasses.Execute1("UPDATE PURCHASESCHEDULE_APPROVAL  SET MODIFY=0 where PA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

            }

            Response.Redirect("~/Transactions/View/ViewPurchaseScheduleApproval.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {

            flag = false;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion





}
