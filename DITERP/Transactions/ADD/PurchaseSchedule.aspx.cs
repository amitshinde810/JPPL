using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

public partial class Transactions_ADD_PurchaseSchedule : System.Web.UI.Page
{

    #region General Declaration
    static int mlCode = 0;
    static string right = "";
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    public static string str = "";
    public static int Index = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();
    #endregion
    public string Msg = "";

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='240'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    ViewState["dt2"] = dt2;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    ViewState["ItemUpdateIndex"] = "-1"; // ItemUpdateIndex;
                    ViewState["str"] = str;
                    ViewState["Index"] = Index;
                    ViewState["mlCode"] = mlCode;

                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["mlCode"] = mlCode;
                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["mlCode"] = mlCode;
                        ViewRec("MOD");
                        txtScheduleDate.Attributes.Add("readonly", "readonly");
                        txtScheduleMonth.Attributes.Add("readonly", "readonly");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtScheduleDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtScheduleMonth.Text = System.DateTime.Now.ToString("MMM yyyy");

                        BlankGrid();
                        LoadCombos();

                        dtFilter.Rows.Clear();
                        ViewState["str"] = "";
                        txtScheduleDate.Attributes.Add("readonly", "readonly");
                        txtScheduleMonth.Attributes.Add("readonly", "readonly");
                    }
                    //ddlSupplier.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Purchase Schedule", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region BlankGrid
    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_PSM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_PO_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_MONTH_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_W1_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_W2_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_W4_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PSD_W3_QTY", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInwardMaster.DataSource = dtFilter;
            dgInwardMaster.DataBind();
            ((DataTable)ViewState["dt2"]).Rows.Clear();
        }
    }
    #endregion BlankGrid

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillSupplier
        try
        {
            DataTable dtParty = new DataTable();
            string str = "";
            if (txtScheduleMonth.Text != "")
            { //add condition for show perticular Supplier in schedule month only once
                str = str + "PSM_SCHEDULE_MONTH='" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("MMM yyyy") + "' AND";
            }
            if (Convert.ToInt32(ViewState["mlCode"]) == 0)
            {
                // Load PO And SubCOn PO :- Remove SPOM_POTYPE condition on 20062018 
                //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0  AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY) AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1  AND   SPOM_DATE<'" + Convert.ToDateTime(txtScheduleMonth.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'    AND SPOD_I_CODE NOT IN(select PSD_I_CODE from PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PSM_CODE=PSD_PSM_CODE where " + str + " PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND MONTH(PSM_SCHEDULE_MONTH)=MONTH('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND YEAR(PSM_SCHEDULE_MONTH)=YEAR('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND PSM_P_CODE=P_CODE) ORDER BY P_NAME");
                /*Remove Short Close and Inward-Order lock*/
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 AND SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1 AND  SPOD_I_CODE NOT IN(select PSD_I_CODE from PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PSM_CODE=PSD_PSM_CODE where " + str + " PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND MONTH(PSM_SCHEDULE_MONTH)=MONTH('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND YEAR(PSM_SCHEDULE_MONTH)=YEAR('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND PSM_P_CODE=P_CODE) ORDER BY P_NAME");
            }
            else
            {
                //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0  AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1  AND   SPOM_DATE<'" + Convert.ToDateTime(txtScheduleMonth.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  ORDER BY P_NAME ");
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 AND SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1    ORDER BY P_NAME ");
            }
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillItem
        try
        {
            string str = "";
            string str1 = "";
            if (txtScheduleMonth.Text != "")
            {
                //add condition for show perticular Item in schedule month only once
                str1 = str1 + "PSM_SCHEDULE_MONTH='" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("MMM yyyy") + "' AND";
            }
            if (ddlSupplier.SelectedValue != "0")
            {
                str = str + "SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND";
            }
            if (Convert.ToInt32(ViewState["mlCode"]) == 0)
            {
                dt = CommonClasses.Execute("SELECT distinct ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO FROM ITEM_MASTER INNER JOIN SUPP_PO_DETAILS ON ITEM_MASTER.I_CODE = SUPP_PO_DETAILS.SPOD_I_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE WHERE " + str + " (ITEM_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "'  AND I_CODE NOT IN(select PSD_I_CODE from PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where " + str1 + " ES_DELETE=0 AND MONTH(PSM_SCHEDULE_MONTH)=MONTH(getdate()) AND YEAR(PSM_SCHEDULE_MONTH)=YEAR(getdate()) and PSM_CODE=PSD_PSM_CODE) ORDER BY I_NAME)");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT distinct ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO FROM ITEM_MASTER INNER JOIN SUPP_PO_DETAILS ON ITEM_MASTER.I_CODE = SUPP_PO_DETAILS.SPOD_I_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE WHERE " + str + " (ITEM_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "')");
            }
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
            ddlItemName.Items.Insert(0, new ListItem("Please Select Item", "0"));
            LoadPO();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "LoadCombos", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        #region PO
        try
        {
            string str = "";
            if (ddlSupplier.SelectedValue != "0")
            {
                str = str + "SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND";
            }
            //dt = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER,COMPANY_MASTER,SUPP_PO_DETAILS where " + str + " ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Session["CompanyId"] + "'  AND SPOD_SPOM_CODE=SPOM_CODE  AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOM_CM_CODE=CM_CODE AND CM_CLOSING_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' order by SPOM_CODE desc");
            dt = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO   from SUPP_PO_MASTER,COMPANY_MASTER,SUPP_PO_DETAILS   where " + str + " ES_DELETE=0 and SPOM_POST=1 AND SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Session["CompanyId"] + "' AND SPOD_SPOM_CODE=SPOM_CODE     AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOM_CM_CODE=CM_CODE   order by SPOM_CODE desc");
            ddlPoNumber.DataSource = dt;
            ddlPoNumber.DataTextField = "SPOM_PO_NO";
            ddlPoNumber.DataValueField = "SPOM_CODE";
            ddlPoNumber.DataBind();
            ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));


            if (dt.Rows.Count > 0)
            {
                txtReqQty.Text = "NA";

                //comit below function for remove schedule approval lock
                LoadPurchaseRequ();
                ddlPoNumber.SelectedIndex = 1;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "LoadCombos", Ex.Message);
        }
        #endregion
    }
    #endregion LoadPO


    #region LoadPurchaseRequ
    private void LoadPurchaseRequ()
    {
        try
        {

            DataTable dtBroughtout = new DataTable();
            //For Broughout Reqiurment 
            bool falg = false;
            string Query = "";
            double req_qty = 0, Schedule_Qty = 0;
            DataTable dtitem = new DataTable();
            dtitem = CommonClasses.Execute("SELECT * FROM ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");

            if (dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483635")
            {
                //Query = "SELECT I.I_CODE, I.I_CODENO,I.I_NAME,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME, BOM.BOM_CODE, BOD.BOD_CODE, BOD.BOD_I_BOUGHT_CODE, ISNULL(BOD.BOD_QTY, 0) AS BOD_QTY, ISNULL((SELECT SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE WHERE CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE AND CUSTOMER_SCHEDULE.ES_DELETE=0 AND CS_DATE='" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "' ),0) AS CS_SCHEDULE_QTY,ISNULL(ISNULL(BOD.BOD_QTY,0) * ISNULL((ISNULL((SELECT SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE WHERE CUSTOMER_SCHEDULE.CS_I_CODE=I.I_CODE AND CUSTOMER_SCHEDULE.ES_DELETE=0 AND CS_DATE='" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "' ),0) + ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0) +ISNULL(SIM_RFM_STORE,0) +ISNULL(SIM_FOUNDRY,0) +ISNULL(SIM_MAIN_STORE,0) ),0) - ISNULL((SELECT SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY FROM STOCK_LEDGER WHERE STL_I_CODE= I.I_CODE AND STL_DOC_DATE<'" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "'),0)), 0)* ISNULL((SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Session["CompanyId"] + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Session["CompanyId"].ToString() + "')),0),0) AS BODQTY_SCHEDULE,ISNULL(I_MIN_LEVEL,0) AS TOT_STND_STOCK, ISNULL((SELECT SUM(ISNULL(STL_DOC_QTY,0)) FROM STOCK_LEDGER WHERE STL_I_CODE=BOD.BOD_I_BOUGHT_CODE AND STL_DOC_DATE<'" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "'),0) AS ACTUALSTOCKQTY,ISNULL(I_INV_RATE,0) AS I_INV_RATE1,(SELECT I1.I_INV_RATE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = '" + Session["CompanyId"] + "')) AS I_INV_RATE,(SELECT I1.I_CODE FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = '" + Session["CompanyId"] + "')) AS I_CODE1, (SELECT I1.I_NAME FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = '" + Session["CompanyId"] + "')) AS I_CODE_BOUGHT_NAME,(SELECT I1.I_CODENO FROM BOUGHTOUT_DETAIL AS BOD1 INNER JOIN ITEM_MASTER AS I1 ON BOD1.BOD_I_BOUGHT_CODE = I1.I_CODE WHERE (BOD1.BOD_CODE = BOD.BOD_CODE) AND (BOD.BOD_I_BOUGHT_CODE = BOD1.BOD_I_BOUGHT_CODE) AND (BOD1.ES_DELETE = 0) AND (I1.ES_DELETE = 0) AND (I1.I_CM_COMP_ID = '" + Session["CompanyId"] + "')) AS I_CODE_BOUGHT_CODE  INTO #TEMP FROM BOUGHTOUT_MASTER AS BOM INNER JOIN BOUGHTOUT_DETAIL AS BOD ON BOM.BOM_CODE = BOD.BOD_BOM_CODE INNER JOIN ITEM_MASTER AS I ON BOM.BOM_I_CODE = I.I_CODE INNER JOIN CUSTOMER_SCHEDULE AS C ON I.I_CODE =C.CS_I_CODE INNER JOIN STANDARD_INVENTARY_MASTER S  ON C.CS_I_CODE=S.SIM_I_CODE WHERE  (S.ES_DELETE = 0) AND (BOM.ES_DELETE = 0) AND (BOD.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND BOM.BOM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND C.CS_DATE='" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "'  SELECT I_CODE,I_CODENO,I_NAME,ICODE_INAME,BOM_CODE,BOD_CODE,BOD_I_BOUGHT_CODE,BOD_QTY,CS_SCHEDULE_QTY,CASE WHEN BODQTY_SCHEDULE<0 THEN 0 ELSE ROUND(BODQTY_SCHEDULE,0) END AS BODQTY_SCHEDULE,TOT_STND_STOCK,ACTUALSTOCKQTY,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE  INTO #TEMP1  FROM #TEMP  where I_CODE1='" + ddlItemCode.SelectedValue + "'   SELECT  CASE WHEN (SUM(BODQTY_SCHEDULE+TOT_STND_STOCK )-ACTUALSTOCKQTY)>0 then  (SUM(BODQTY_SCHEDULE+TOT_STND_STOCK )-ACTUALSTOCKQTY)  ELSE 0 END     AS REQ_QTY,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE  FROM #TEMP1 GROUP BY   TOT_STND_STOCK,ACTUALSTOCKQTY,I_INV_RATE,I_CODE1,I_CODE_BOUGHT_NAME,I_CODE_BOUGHT_CODE DROP TABLE #TEMP1 DROP TABLE #TEMP";
                Query = "SELECT PA_MONTH, PA_I_CODE, PA_QTY AS REQ_QTY, PA_RATE, PA_AMT FROM PURCHASESCHEDULE_APPROVAL  WHERE (DATEPART(MM, PA_MONTH) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Month + "') AND (DATEPART(YYYY, PA_MONTH) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Year + "') AND (ES_DELETE = 0) AND PA_I_CODE='" + ddlItemCode.SelectedValue + "'";
            }
            //For Casting Reqiurment
            else if (dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483648")
            {
                 
                //Query = " SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + Convert.ToDateTime(txtScheduleDate.Text).Month + "'   and DATEPART(YYYY,VS_DATE)='" + Convert.ToDateTime(txtScheduleDate.Text).Year + "' ),0) AS CS_SCHEDULE_QTY,ISNULL((ISNULL(SIM_RFI_STORE,0)+ ISNULL(SIM_FINISH_GOODS,0)+ ISNULL(SIM_MACHINE_SHOP,0)+ISNULL(SIM_VENDOR,0)),0) AS Tot_Stnd_Stock ,ISNULL((select ISNULL(SUM(ISNULL(CL_CQTY,0)),0) as STL_DOC_QTY from challan_stock_ledger where CL_I_CODE = I.I_CODE and CL_DOC_DATE<'" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "'),0) AS ActualStockQty INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'     AND VS_I_CODE='" + ddlItemCode.SelectedValue + "'  and VS_DATE='" + Convert.ToDateTime(txtScheduleDate.Text).ToString("01/MMM/yyyy") + "' SELECT I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	+Tot_Stnd_Stock	-ActualStockQty )* " + LossPer + ",0)  AS REQ_QTY FROM #temp DROP TABLE #temp ";

                //Query = "SELECT PA_MONTH, PA_I_CODE, PA_QTY AS REQ_QTY, PA_RATE, PA_AMT FROM PURCHASESCHEDULE_APPROVAL  WHERE (DATEPART(MM, PA_MONTH) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Month + "') AND (DATEPART(YYYY, PA_MONTH) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Year + "') AND (ES_DELETE = 0) AND PA_I_CODE='" + ddlItemCode.SelectedValue + "'";

                Query = "SELECT VSA_MONTH, VSA_I_CODE, VSA_QTY_PPC AS REQ_QTY, VSA_RATE , VSA_AMT  FROM VENDORSCHEDULE_APPROVAL  WHERE (DATEPART(MM, VSA_MONTH) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Month + "') AND (DATEPART(YYYY, VSA_MONTH) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Year + "') AND (ES_DELETE = 0) AND VSA_I_CODE='" + ddlItemCode.SelectedValue + "'";

            } 
            else
            {
                falg = true;
            }
            dtBroughtout = CommonClasses.Execute(Query);
            if (dtBroughtout.Rows.Count > 0)
            {
                req_qty = Convert.ToDouble(dtBroughtout.Rows[0]["REQ_QTY"].ToString());
            }



            DataTable dtshedule = new DataTable();
            dtshedule = CommonClasses.Execute(" SELECT ISNULL(SUM(PSD_MONTH_QTY),0) AS PSD_MONTH_QTY  FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE  AND PSD_I_CODE='" + ddlItemCode.SelectedValue + "'  AND DATEPART(MM,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(txtScheduleDate.Text).Month + "' AND DATEPART(YYYY,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(txtScheduleDate.Text).Year + "'  AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0");
            if (dtshedule.Rows.Count > 0)
            {
                Schedule_Qty = Convert.ToDouble(dtshedule.Rows[0]["PSD_MONTH_QTY"].ToString());
            }
            if (falg == false)
            {
                txtReqQty.Text = (req_qty - Schedule_Qty).ToString();
            }
            else
            {
                txtReqQty.Text = "NA";
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "LoadPurchaseRequ", Ex.Message);
        }

    }

    #endregion LoadPurchaseRequ

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            {

                if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Supplier Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemCode.Focus();
                    return;
                }

                if (Convert.ToDateTime(txtScheduleDate.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Date in current financial year ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtScheduleDate.Focus();
                    return;
                }

                if (Convert.ToDateTime(txtScheduleMonth.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Date in current financial year ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtScheduleMonth.Focus();
                    return;
                }
                if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
                {
                    SaveRec();
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Insert Item ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                    return;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Current Year Date ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("SELECT PSM_CODE,PSM_SCHEDULE_NO,PSM_SCHEDULE_DATE,PSM_SCHEDULE_MONTH,PSM_P_CODE,PSM_CM_COMP_ID FROM PURCHASE_SCHEDULE_MASTER where ES_DELETE=0 and PSM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]) + " and PSM_CODE=" + ViewState["mlCode"] + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["PSM_CODE"]);
                string id = dt.Rows[0]["PSM_P_CODE"].ToString();
                txtScheduleNo.Text = Convert.ToInt32(dt.Rows[0]["PSM_SCHEDULE_NO"]).ToString();
                txtScheduleDate.Text = Convert.ToDateTime(dt.Rows[0]["PSM_SCHEDULE_DATE"]).ToString("dd MMM yyyy");
                txtScheduleMonth.Text = Convert.ToDateTime(dt.Rows[0]["PSM_SCHEDULE_MONTH"]).ToString(" MMM yyyy");

                // LoadCombos();

                #region FillSupplier
                try
                {
                    DataTable dtParty = new DataTable();
                    string str1 = "";
                    if (txtScheduleMonth.Text != "")
                    { //add condition for show perticular Supplier in schedule month only once
                        str1 = str1 + "PSM_SCHEDULE_MONTH='" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("MMM yyyy") + "' AND";
                    }
                    if (Convert.ToInt32(ViewState["mlCode"]) == 0)
                    {
                        // Load PO And SubCOn PO :- Remove SPOM_POTYPE condition on 20062018 
                        //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0  AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY) AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1  AND   SPOM_DATE<'" + Convert.ToDateTime(txtScheduleMonth.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'    AND SPOD_I_CODE NOT IN(select PSD_I_CODE from PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PSM_CODE=PSD_PSM_CODE where " + str1 + " PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND MONTH(PSM_SCHEDULE_MONTH)=MONTH('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND YEAR(PSM_SCHEDULE_MONTH)=YEAR('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND PSM_P_CODE=P_CODE) ORDER BY P_NAME");
                        /* Remove Short close and inward-Order lock*/
                        dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1  AND   SPOM_DATE<'" + Convert.ToDateTime(txtScheduleMonth.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'    AND SPOD_I_CODE NOT IN(select PSD_I_CODE from PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PSM_CODE=PSD_PSM_CODE where " + str1 + " PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND MONTH(PSM_SCHEDULE_MONTH)=MONTH('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND YEAR(PSM_SCHEDULE_MONTH)=YEAR('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND PSM_P_CODE=P_CODE) ORDER BY P_NAME");
                    }
                    else
                    {
                        //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0  AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1  AND   SPOM_DATE<'" + Convert.ToDateTime(txtScheduleMonth.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  ORDER BY P_NAME ");
                        dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1 AND P_INHOUSE_IND=1 and SPOM_CANCEL_PO=0  AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "' and P_TYPE='2' and P_ACTIVE_IND=1 AND SPOM_DATE<'" + Convert.ToDateTime(txtScheduleMonth.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'  ORDER BY P_NAME ");
                    }
                    ddlSupplier.DataSource = dt;
                    ddlSupplier.DataTextField = "P_NAME";
                    ddlSupplier.DataValueField = "P_CODE";
                    ddlSupplier.DataBind();
                    ddlSupplier.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Purchase Schedule", "LoadCombos", Ex.Message);
                }
                #endregion
                ddlSupplier.SelectedValue = id;
                ddlSupplier_SelectedIndexChanged(null, null);
                dtInwardDetail = CommonClasses.Execute("SELECT PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, CONVERT(varchar, SUPP_PO_MASTER.SPOM_PO_NO) + '/' + RIGHT(CONVERT(varchar, YEAR(COMPANY_MASTER.CM_OPENING_DATE)), 2) + '-' + RIGHT(CONVERT(varchar,YEAR(COMPANY_MASTER.CM_CLOSING_DATE)), 2) AS SPOM_PO_NO, ITEM_MASTER.I_CODE, PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE,PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY, PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY, PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY,PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY,PURCHASE_SCHEDULE_DETAIL.PSD_MONTH_QTY, SUPP_PO_MASTER.SPOM_CM_CODE FROM ITEM_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON ITEM_MASTER.I_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE INNER JOIN SUPP_PO_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN COMPANY_MASTER ON SUPP_PO_MASTER.SPOM_CM_CODE = COMPANY_MASTER.CM_CODE INNER JOIN PURCHASE_SCHEDULE_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE = PURCHASE_SCHEDULE_MASTER.PSM_CODE where PSM_CM_COMP_ID=1 and ITEM_MASTER.ES_DELETE=0 and PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND SUPP_PO_MASTER.ES_DELETE=0 and PSD_PSM_CODE='" + ViewState["mlCode"] + "'");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                }
            }
            if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;

                ddlItemName.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlPoNumber.Enabled = false;
                txtweek4qty.Enabled = false;
                txtWeek3Qty.Enabled = false;
                txtWeek1Qty.Enabled = false;
                txtWeek2Qty.Enabled = false;
                dgInwardMaster.Enabled = false;
                txtMonthQty.Enabled = false;

            }
            if (str == "MOD")
            {
                txtScheduleMonth.Enabled = false;
                ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("PURCHASE_SCHEDULE_MASTER", "ES_MODIFY", "PSM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
        string msg = "";
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Po_Doc_no = 0;
                DataTable dt = new DataTable();
                /*Change By Mahesh :- Change Query*/
                dt = CommonClasses.Execute("Select isnull(max(cAST(PSM_SCHEDULE_NO AS INT)),0) as PSM_SCHEDULE_NO FROM PURCHASE_SCHEDULE_MASTER WHERE PSM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["PSM_SCHEDULE_NO"]);
                    Po_Doc_no = Po_Doc_no + 1;
                }
                if (CommonClasses.Execute1("INSERT INTO PURCHASE_SCHEDULE_MASTER (PSM_SCHEDULE_NO,PSM_SCHEDULE_DATE,PSM_SCHEDULE_MONTH,PSM_P_CODE,PSM_CM_COMP_ID) values('" + Po_Doc_no + "','" + Convert.ToDateTime(txtScheduleDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("01/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + Convert.ToInt32(Session["CompanyId"]) + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(PSM_CODE) from PURCHASE_SCHEDULE_MASTER");

                    for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                    {
                        result = CommonClasses.Execute1("INSERT INTO PURCHASE_SCHEDULE_DETAIL (PSD_PSM_CODE,PSD_I_CODE,PSD_PO_CODE,PSD_MONTH_QTY,PSD_W1_QTY,PSD_W2_QTY,PSD_W3_QTY,PSD_W4_QTY) VALUES('" + Code + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_I_CODE")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_PO_CODE")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_MONTH_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W1_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W2_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W3_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W4_QTY")).Text + "')");
                        //result = CommonClasses.Execute1("INSERT INTO PURCHASE_SCHEDULE_DETAIL(IMD_COMP_ID,PSM_CODE,IMD_I_CODE,IMD_UOM,IMD_CURR_STOCK,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK,IMD_RATE,IMD_AMOUNT,IMD_To_STORE) values ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Code + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblUOM_CODE")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblCurrStock")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblQtyRequirment")).Text + "','" + PTS_QTY + "','" + ((TextBox)dgInwardMaster.Rows[i].FindControl("txtRemark")).Text.Replace("'", "\''") + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblIMD_RATE")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblIMD_AMOUNT")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblIMD_To_STORE_CODE")).Text + "')");
                    }
                    CommonClasses.WriteLog("Purchase Schedule", "Save", "Purchase Schedule", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewPurchaseSchedule.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtScheduleDate.Focus();
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE PURCHASE_SCHEDULE_MASTER SET PSM_SCHEDULE_DATE='" + Convert.ToDateTime(txtScheduleDate.Text).ToString("dd/MMM/yyyy") + "',PSM_SCHEDULE_MONTH='" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "',PSM_P_CODE='" + ddlSupplier.SelectedValue + "' where PSM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and PSM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyCode"]) + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM PURCHASE_SCHEDULE_DETAIL WHERE PSD_PSM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO PURCHASE_SCHEDULE_DETAIL (PSD_PSM_CODE,PSD_I_CODE,PSD_PO_CODE,PSD_MONTH_QTY,PSD_W1_QTY,PSD_W2_QTY,PSD_W3_QTY,PSD_W4_QTY) VALUES('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_I_CODE")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_PO_CODE")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_MONTH_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W1_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W2_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W3_QTY")).Text + "','" + ((Label)dgInwardMaster.Rows[i].FindControl("lblPSD_W4_QTY")).Text + "')");
                            // Inserting Into PURCHASE_SCHEDULE_DETAIL
                        }
                        CommonClasses.RemoveModifyLock("PURCHASE_SCHEDULE_MASTER", "MODIFY", "PSM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Purchase Schedule", "Update", "Purchase Schedule", txtScheduleNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtScheduleNo.Focus();
                    }
                }
                Response.Redirect("~/Transactions/VIEW/ViewPurchaseSchedule.aspx", false);
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Schedule", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Purchase Schedule", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void dgInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    #region dgInwardMaster_RowCommand
    protected void dgInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["Index"] = Index;
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"])];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                int I_CODE = Convert.ToInt32(((Label)(row.FindControl("lblPSD_I_CODE"))).Text);
                int CPOM_CODE = Convert.ToInt32(((Label)(row.FindControl("lblPSD_PO_CODE"))).Text);

                //Changed by Sujata if schedule qty used in Inward
                DataTable dtInward = CommonClasses.Execute("SELECT DISTINCT ISNULL(SUM(INWARD_DETAIL.IWD_CH_QTY), 0) AS IWD_CH_QTY,ISNULL(SUM(INWARD_DETAIL.IWD_REV_QTY), 0) AS IWD_REV_QTY FROM INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE WHERE (INWARD_MASTER.ES_DELETE = 0) AND datepart(MM,IWM_DATE) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Month + "' AND datepart(YYYY,IWM_DATE) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Year + "' AND IWD_CPOM_CODE='" + CPOM_CODE + "' AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "' AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ");
                if (dtInward.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record, it is used in Inward";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else
                {
                    dgInwardMaster.DeleteRow(rowindex);
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                    dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                    dgInwardMaster.DataBind();
                    if (dgInwardMaster.Rows.Count == 0)
                        BlankGrid();
                }
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                string s = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                ddlPoNumber.SelectedValue = ((Label)(row.FindControl("lblPSD_PO_CODE"))).Text;

                DataTable dtInward = CommonClasses.Execute("SELECT DISTINCT  ISNULL(SUM(INWARD_DETAIL.IWD_CH_QTY), 0) AS IWD_CH_QTY,ISNULL(SUM(INWARD_DETAIL.IWD_REV_QTY), 0) AS IWD_REV_QTY FROM INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE WHERE (INWARD_MASTER.ES_DELETE = 0) AND datepart(MM,IWM_DATE) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Month + "' AND datepart(YYYY,IWM_DATE) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Year + "' AND IWD_CPOM_CODE='" + ddlPoNumber.SelectedValue + "'  AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "' AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ");
                if (dtInward.Rows.Count > 0)
                {
                    txtPoPendingQty.Text = (Convert.ToDouble(txtPoPendingQty.Text) + Convert.ToDouble(dtInward.Rows[0]["IWD_CH_QTY"].ToString())).ToString();
                }

                txtMonthQty.Text = ((Label)(row.FindControl("lblPSD_MONTH_QTY"))).Text;
                if (txtReqQty.Text.Trim().ToUpper()!="NA")
                {
                    txtReqQty.Text = (Convert.ToDouble(txtReqQty.Text) + Convert.ToDouble(txtMonthQty.Text)).ToString(); 
                }
                txtWeek1Qty.Text = ((Label)(row.FindControl("lblPSD_W1_QTY"))).Text;
                if (txtWeek1Qty.Text == "")
                {
                    txtWeek1Qty.Text = "0";
                }
                txtWeek2Qty.Text = ((Label)(row.FindControl("lblPSD_W2_QTY"))).Text;
                txtWeek3Qty.Text = ((Label)(row.FindControl("lblPSD_W3_QTY"))).Text;
                txtweek4qty.Text = ((Label)(row.FindControl("lblPSD_W4_QTY"))).Text;

                //  dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,SPOD_RATE,SPOD_UOM_CODE FROM SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");

                // For Enabled Delete Button when record is modify.
                foreach (GridViewRow gvr in dgInwardMaster.Rows)
                {
                    LinkButton lnkButton = ((LinkButton)gvr.FindControl("lnkDelete"));
                    lnkButton.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedValue = "0";
            ddlItemCode.SelectedValue = "0";
            ddlPoNumber.SelectedValue = "0";
            txtweek4qty.Text = "0.00";
            txtWeek1Qty.Text = "0.00";
            txtWeek2Qty.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Material Inward ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Supplier Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (Convert.ToInt32(ddlItemName.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (txtWeek1Qty.Text == "" || Convert.ToDouble(txtWeek1Qty.Text) == 0)
            {
                if (txtWeek2Qty.Text == "" || Convert.ToDouble(txtWeek2Qty.Text) == 0)
                {
                    if (txtWeek3Qty.Text == "" || Convert.ToDouble(txtWeek3Qty.Text) == 0)
                    {
                        if (txtweek4qty.Text == "" || Convert.ToDouble(txtweek4qty.Text) == 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Please Enter Week Qty";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtWeek2Qty.Focus();
                            return;
                        }
                    }
                }
            }
            if (txtMonthQty.Text == "" || Convert.ToDouble(txtMonthQty.Text) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Monthly Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtMonthQty.Focus();
                return;
            }
            double WeekTotQty = Convert.ToDouble(txtWeek1Qty.Text) + Convert.ToDouble(txtWeek2Qty.Text) + Convert.ToDouble(txtWeek3Qty.Text) + Convert.ToDouble(txtweek4qty.Text);
            if (Convert.ToDouble(txtMonthQty.Text) != WeekTotQty)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Monthly Qty And Week Total Qty is Not Match...";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtWeek1Qty.Text = "0.000"; txtWeek2Qty.Text = "0.000"; txtWeek3Qty.Text = "0.000"; txtweek4qty.Text = "0.000";
                txtMonthQty.Focus();
                return;
            }
            string strMlcode = "";
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                strMlcode = " and PSD_PSM_CODE<>" + ViewState["mlCode"].ToString();
            }
            else
            {
                strMlcode = "";
            }
            PanelMsg.Visible = false;

            //Changed by Sujata for schedule qty always greater than IwdChQty
            double IwdChQty, SchQty;
            DataTable dtIwdQty = CommonClasses.Execute("SELECT DISTINCT ISNULL(SUM(INWARD_DETAIL.IWD_CH_QTY), 0) AS IWD_CH_QTY,ISNULL(SUM(INWARD_DETAIL.IWD_REV_QTY), 0) AS IWD_REV_QTY FROM INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE WHERE (INWARD_MASTER.ES_DELETE = 0) AND (DATEPART(MM, INWARD_MASTER.IWM_DATE) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Month + "') AND (DATEPART(YYYY, INWARD_MASTER.IWM_DATE) = '" + Convert.ToDateTime(txtScheduleMonth.Text).Year + "') aND IWD_CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "' AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' GROUP BY INWARD_MASTER.IWM_DATE");
            if (dtIwdQty.Rows.Count > 0)
            {
                IwdChQty = Convert.ToDouble(dtIwdQty.Rows[0]["IWD_CH_QTY"]);
                SchQty = Convert.ToDouble(txtMonthQty.Text);
                ViewState["IwdChQty"] = IwdChQty;
                if (SchQty < Convert.ToDouble(ViewState["IwdChQty"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Schedule Qty should not be less than Inward Qty : '" + ViewState["IwdChQty"] + "'";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (txtReqQty.Text.Trim() != "NA")
            { 
                if (Convert.ToDouble(txtMonthQty.Text) > Convert.ToDouble(txtReqQty.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Schedule Qty should not be Grater than  Req. Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            DataTable dtexist = CommonClasses.Execute("SELECT PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE FROM SUPP_PO_DETAILS INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE ON SUPP_PO_DETAILS.SPOD_I_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE AND SUPP_PO_DETAILS.SPOD_SPOM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE where PSD_I_CODE=" + ddlItemCode.SelectedValue + " and PSD_PO_CODE=" + ddlPoNumber.SelectedValue + " and  Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year('" + Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy") + "')),2)+ '-' + Right(Convert(varchar,Year('" + Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy") + "')),2)='" + ddlPoNumber.SelectedItem + "' and  SUPP_PO_MASTER.SPOM_CM_CODE=" + Session["CompanyCode"] + " and PURCHASE_SCHEDULE_MASTER.ES_DELETE=0  AND MONTH(PSM_SCHEDULE_MONTH)=MONTH('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "') AND YEAR(PSM_SCHEDULE_MONTH)=YEAR('" + Convert.ToDateTime(txtScheduleMonth.Text).ToString("dd/MMM/yyyy") + "')" + strMlcode);
            if (dtexist.Rows.Count > 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record already exist for this Schedule";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtWeek2Qty.Focus();
                return;
            }
            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblPSD_I_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_PO_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("SPOM_PO_NO", typeof(string));
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_MONTH_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_W1_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_W2_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_W3_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PSD_W4_QTY");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["PSD_I_CODE"] = ddlItemName.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem;
            dr["I_NAME"] = ddlItemCode.SelectedItem;
            dr["PSD_PO_CODE"] = ddlPoNumber.SelectedValue;
            dr["SPOM_PO_NO"] = ddlPoNumber.SelectedItem.Text;
            dr["PSD_MONTH_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtMonthQty.Text)));
            dr["PSD_W1_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtWeek1Qty.Text)));
            dr["PSD_W2_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtWeek2Qty.Text)));
            dr["PSD_W3_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtWeek3Qty.Text)));
            dr["PSD_W4_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtweek4qty.Text)));
            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"]));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"]));

                    ddlItemName.SelectedIndex = 0;
                    ddlItemCode.SelectedIndex = 0;
                    txtMonthQty.Text = "";
                    txtWeek1Qty.Text = "";
                    txtWeek2Qty.Text = "";
                    txtWeek3Qty.Text = "";
                    txtweek4qty.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtMonthQty.Text = "";
                txtWeek1Qty.Text = "";
                txtWeek2Qty.Text = "";
                txtWeek3Qty.Text = "";
                txtweek4qty.Text = "";
                ddlItemName.SelectedIndex = 0;
                ddlItemCode.SelectedIndex = 0;
                ddlPoNumber.SelectedIndex = 0;
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            #endregion
            //To avoid same item insert in Grid
            ViewState["ItemUpdateIndex"] = "-1";
            ViewState["IwdChQty"] = 0;
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCode.SelectedIndex != 0)
        {
            ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
            LoadPO();
        }
        POQty();
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        if (ddlItemName.SelectedIndex != 0)
        {
            LoadPO();
        }
        POQty();
    }
    #endregion

    #region ddlPoNumber_SelectedIndexChanged
    protected void ddlPoNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            POQty();
            dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
                {
                    txtweek4qty.Enabled = false;
                    PanelMsg.Visible = true;
                }
                else
                {
                    PanelMsg.Visible = false;
                    txtweek4qty.Enabled = true;
                    txtweek4qty.Enabled = true;
                }
            }
            else
            {
                txtweek4qty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            int id = Convert.ToInt32(ddlSupplier.SelectedValue);
            ddlItemName.Items.Clear();
            ddlItemCode.Items.Clear();
            DataTable dtItem = new DataTable();
            //dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_CODE=SPOD_SPOM_CODE   and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND SPOM_CM_CODE='" + (string)Session["CompanyCode"] + "' AND  SPOM_POST=1 AND SPOM_P_CODE=" + id + " ORDER BY I_NAME");
            dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1 AND SPOM_CODE=SPOD_SPOM_CODE and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND SPOM_POST=1 AND SPOM_P_CODE=" + id + " ORDER BY I_NAME");
            ddlItemName.DataSource = dtItem;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemCode.DataSource = dtItem;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
            ddlItemCode.Items.Insert(0, new ListItem("Please Select Item", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "ddlSupplier_SelectedIndexChanged", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region POQty
    private void POQty()
    {
        if (ddlPoNumber.SelectedValue != "0")
        {
            //DataTable dt = CommonClasses.Execute("select SPOM_CODE,isnull(SPOD_ORDER_QTY,0) as SPOD_ORDER_QTY,isnull(SPOD_INW_QTY,0) as SPOD_INW_QTY from SUPP_PO_MASTER,SUPP_PO_DETAILS,COMPANY_MASTER where ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='1' AND SPOM_CODE=SPOD_SPOM_CODE and SPOM_CM_CODE=CM_CODE and SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "' order by SPOM_CODE desc");
            DataTable dt = CommonClasses.Execute("select SPOM_CODE,isnull(SPOD_ORDER_QTY,0) as SPOD_ORDER_QTY,isnull(SPOD_INW_QTY,0) as SPOD_INW_QTY from SUPP_PO_MASTER,SUPP_PO_DETAILS,COMPANY_MASTER where ES_DELETE=0 and SPOM_POST=1 AND SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='1' AND SPOM_CODE=SPOD_SPOM_CODE and SPOM_CM_CODE=CM_CODE and SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "' order by SPOM_CODE desc");
            if (dt.Rows.Count > 0)
            {
                Double Qty = Math.Round(Convert.ToDouble(dt.Rows[0]["SPOD_ORDER_QTY"]), 2);
                Double INWQty = Math.Round(Convert.ToDouble(dt.Rows[0]["SPOD_INW_QTY"]), 2);
                //Double fQty = Qty - INWQty;
                Double fQty = Qty;
                txtPoPendingQty.Text = fQty.ToString();
            }
        }
        else
        {
            txtPoPendingQty.Text = "0.00";
            txtWeek1Qty.Text = "0.00"; txtWeek2Qty.Text = "0.00"; txtWeek3Qty.Text = "0.00"; txtweek4qty.Text = "0.00";

        }
    }
    #endregion POQty

    #region Qty
    private void Qty()
    {
        #region Qty
        try
        {
            string str = "";
            if (ddlSupplier.SelectedIndex != 0)
            {
                str = str + "SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND ";
            }
            if (ddlItemCode.SelectedIndex != 0)
            {
                str = str + "SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND ";
            }
            if (ddlPoNumber.SelectedIndex != 0)
            {
                str = str + "SPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND ";
            }
            else
            {
                // PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select PO No.";
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlPoNumber.SelectedIndex = 0;
                txtWeek1Qty.Text = "0.000";
                txtWeek2Qty.Text = "0.000";
                txtWeek3Qty.Text = "0.000";
                txtweek4qty.Text = "0.000";
                return;
            }
            //DataTable dt = CommonClasses.Execute("select SPOM_CODE,SPOD_ORDER_QTY,SPOD_INW_QTY from SUPP_PO_MASTER,SUPP_PO_DETAILS,COMPANY_MASTER where " + str + " ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='1' AND SPOM_CODE=SPOD_SPOM_CODE and SPOM_CM_CODE=CM_CODE order by SPOM_CODE desc");
            DataTable dt = CommonClasses.Execute("select SPOM_CODE,SPOD_ORDER_QTY,SPOD_INW_QTY from SUPP_PO_MASTER,SUPP_PO_DETAILS,COMPANY_MASTER where " + str + " ES_DELETE=0 and SPOM_POST=1 AND SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='1' AND SPOM_CODE=SPOD_SPOM_CODE and SPOM_CM_CODE=CM_CODE order by SPOM_CODE desc");
            if (dt.Rows.Count > 0)
            {
                Double Qty = Math.Round(Convert.ToDouble(dt.Rows[0]["SPOD_ORDER_QTY"]), 2);
                Double INWQty = Math.Round(Convert.ToDouble(dt.Rows[0]["SPOD_INW_QTY"]), 2);
                //Double fQty = Qty - INWQty;
                Double fQty = Qty;
                // txtPoPendingQty.Text = fQty.ToString();
                Double TotQty = 0.00;
                if (txtWeek1Qty.Text == "")
                {
                    txtWeek1Qty.Text = "0.000";
                }
                if (txtWeek2Qty.Text == "")
                {
                    txtWeek2Qty.Text = "0.000";
                }
                if (txtWeek3Qty.Text == "")
                {
                    txtWeek3Qty.Text = "0.000";
                }
                if (txtweek4qty.Text == "")
                {
                    txtweek4qty.Text = "0.000";
                }

                if (txtweek4qty.Text != "" && txtWeek2Qty.Text != "" && txtWeek3Qty.Text != "" && txtweek4qty.Text != "")
                {
                    TotQty = Convert.ToDouble(txtWeek1Qty.Text) + Convert.ToDouble(txtWeek2Qty.Text) + Convert.ToDouble(txtWeek3Qty.Text) + Convert.ToDouble(txtweek4qty.Text);
                }

                if (fQty >= TotQty)
                {

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Qty can not be greater than Balance Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtWeek1Qty.Focus();
                    txtWeek1Qty.Text = "0.000";
                    txtWeek2Qty.Text = "0.000";
                    txtWeek3Qty.Text = "0.000";
                    txtweek4qty.Text = "0.000";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "PendingQty", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region CtlDisable
    public void CtlDisable()
    {
        ddlSupplier.Enabled = false;
        txtScheduleDate.Enabled = false;
        txtScheduleMonth.Enabled = false;
        BtnInsert.Visible = false;
        btnSubmit.Visible = false;
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect("~/Transactions/VIEW/ViewPurchaseSchedule.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
                    // ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "btnCancel_Click", Ex.Message);
        }
    }
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
            CommonClasses.SendError("Purchase Schedule", "btnOk_Click", Ex.Message);
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
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && ViewState["mlCode"] != null)
            {
                CommonClasses.RemoveModifyLock("PURCHASE_SCHEDULE_MASTER", "ES_MODIFY", "PSM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseSchedule.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Schedule", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlSupplier.Text == "")
            {
                flag = false;
            }
            else if (txtScheduleDate.Text == "")
            {
                flag = false;
            }
            else if (txtScheduleMonth.Text == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region txtMonthQty_TextChanged
    protected void txtMonthQty_TextChanged(object sender, EventArgs e)
    {

        if (txtReqQty.Text != "NA")
        {
            if (Convert.ToDouble(txtMonthQty.Text) > Convert.ToDouble(txtReqQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Monthly Qty can not be greater than Req Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtMonthQty.Text = "0.00";
                txtMonthQty.Focus();
                return;
            }
        }
        string totalStr = DecimalMasking(txtMonthQty.Text);
        txtMonthQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }
    #endregion txtMonthQty_TextChanged

    #region txtWeek1Qty_TextChanged
    protected void txtWeek1Qty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek1Qty.Text);
        txtWeek1Qty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        Qty();
    }
    #endregion txtWeek1Qty_TextChanged

    #region txtWeek2Qty_TextChanged
    protected void txtWeek2Qty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek2Qty.Text);
        txtWeek2Qty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        Qty();
    }
    #endregion txtWeek2Qty_TextChanged

    #region txtWeek3Qty_TextChanged
    protected void txtWeek3Qty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtWeek3Qty.Text);
        txtWeek3Qty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        Qty();
    }
    #endregion txtWeek3Qty_TextChanged

    #region txtweek4qty_TextChanged
    protected void txtweek4qty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtweek4qty.Text);
        txtweek4qty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        Qty();
    }
    #endregion txtweek4qty_TextChanged

    #region txtScheduleMonth_TextChanged
    protected void txtScheduleMonth_TextChanged(object sender, EventArgs e)
    {
        //LoadCombos();
        txtWeek1Qty.Text = "0";
        txtWeek2Qty.Text = "0";
        txtWeek3Qty.Text = "0";
        txtweek4qty.Text = "0";
        txtPoPendingQty.Text = "0";
        /*Change By Mahesh :- Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtScheduleMonth.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtScheduleMonth.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date

            PanelMsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtScheduleMonth.Focus();
            return;
        }
    }
    #endregion txtScheduleMonth_TextChanged

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
}

