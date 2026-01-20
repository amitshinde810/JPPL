using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;

public partial class Masters_ADD_Dashboard : System.Web.UI.Page
{
    #region general

    DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();

    DataTable dtFilter = new DataTable();
    DataTable dtStoreFilter = new DataTable();
    public static string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Dashboard1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Dashboard");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE='139'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            ViewState["right"] = right;

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (Session["DASHBOARD"].ToString() == "1")
                {
                    LoadActivity();
                }

                if (CommonClasses.ValidRights(int.Parse(((string)ViewState["right"]).Substring(5, 1)), this, "For Print"))
                {
                    DateTime dtOpenDate = Convert.ToDateTime(Session["CompanyOpeningDate"]);
                    DateTime dtCloseDate = Convert.ToDateTime(Session["CompanyClosingDate"]);
                    txtFromDate.Text = dtOpenDate.ToString("dd MMM yyyy");
                    txtToDate.Text = dtCloseDate.ToString("dd MMM yyyy");
                    txtFromDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");
                    LoadFilter();
                    LoadData();
                    LoadFilterStore();
                    LoadStoreData();
                    LoadTotSale();
                    LoadTotPurchase();
                    LoadTotSubContractor();
                    LoadRawMaterial();
                }
                else
                {
                    StockValuation.Visible = false;
                }
            }
        }
    }
    #endregion Page_Load

    #region LoadFilter
    public void LoadFilter()
    {
        dtFilter.Clear();

        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("VALUE", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgDashboard.DataSource = dtFilter;
            dgDashboard.DataBind();
            dgDashboard.Enabled = false;
        }
    }
    #endregion LoadFilter

    #region LoadStoreData
    public void LoadStoreData()
    {
        try
        {
            //DataTable dtLoadStock = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME ,I_UWEIGHT,I_INV_RATE ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER where STL_STORE_TYPE=-2147483648 AND STL_I_CODE= I_CODE),0) AS INWARD,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER where STL_STORE_TYPE=-2147483647 AND STL_I_CODE= I_CODE),0) AS MAIN,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483646 AND STL_I_CODE= I_CODE),0) AS FOUNDRY  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483645 AND STL_I_CODE= I_CODE),0) AS RFM,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483644 AND STL_I_CODE= I_CODE),0) AS MACHINESHOP  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483643 AND STL_I_CODE= I_CODE),0) AS RFI  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483642 AND STL_I_CODE= I_CODE),0) AS RFD  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483641 AND STL_I_CODE= I_CODE),0) AS Rejection   ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483640 AND STL_I_CODE= I_CODE),0) AS CastingRework  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483639 AND STL_I_CODE= I_CODE),0) AS MACHINERework into #temp  FROM ITEM_MASTER where I_CAT_CODE=-2147483648 AND ES_DELETE=0  SELECT I_CODE,I_CODENO,I_NAME,INWARD,MAIN,FOUNDRY,RFM,MACHINESHOP,RFI,RFD,Rejection,CastingRework,MACHINERework, I_UWEIGHT *INWARD AS INWARD_WT, I_INV_RATE  *INWARD AS INWARD_AMT,I_UWEIGHT*MAIN AS MAIN_WT,I_INV_RATE*MAIN AS MAIN_AMT,I_UWEIGHT*FOUNDRY AS FOUNDRY_WT,I_INV_RATE*FOUNDRY AS FOUNDRY_AMT,I_UWEIGHT*RFM AS RFM_WT,I_INV_RATE*RFM AS RFM_AMT,I_UWEIGHT*MACHINESHOP AS MACHINESHOP_WT,I_INV_RATE*MACHINESHOP AS MACHINESHOP_AMT,I_UWEIGHT*RFI AS RFI_WT,I_INV_RATE*RFI AS RFI_AMT,I_UWEIGHT*RFD AS RFD_WT,I_INV_RATE*RFD AS RFD_AMT,I_UWEIGHT*Rejection AS Rejection_WT,I_INV_RATE*Rejection AS Rejection_AMT,I_UWEIGHT*CastingRework AS CastingRework_WT,I_INV_RATE*CastingRework AS CastingRework_AMT,I_UWEIGHT*MACHINERework AS MACHINERework_WT, I_INV_RATE*MACHINERework AS MACHINERework_AMT  into #temp1  FROM  #temp DROP TABLE #temp     SELECT  'Inward Store' AS STORE_NAME, ROUND(SUM(INWARD),2) AS QTY, ISNULL(ROUND(SUM(INWARD_WT)/1000,2),0) AS INWARD_WT,ISNULL(ROUND(SUM(INWARD_AMT)/100000,2),0) AS INWARD_AMT  FROM  #temp1  UNION   SELECT  'Main Store' AS STORE_NAME, ROUND(SUM(MAIN),2) AS QTY,  ISNULL(ROUND(SUM(MAIN_WT)/1000,2),0) AS MAIN_WT,ISNULL(ROUND(SUM(MAIN_AMT)/100000,2),0) AS MAIN_AMT  FROM  #temp1  UNION  SELECT 'Foundry Store' AS STORE_NAME,ROUND(SUM(FOUNDRY),2) AS QTY ,ISNULL(ROUND(SUM(FOUNDRY_WT)/1000,2),0) AS FOUNDRY_WT,ISNULL(ROUND(SUM(FOUNDRY_AMT)/100000,2),0) AS FOUNDRY_AMT  FROM  #temp1  UNION  SELECT 'RFM Store' AS STORE_NAME,ROUND(SUM(RFM),2) AS QTY , ISNULL(ROUND(SUM(RFM_WT)/1000,2),0) AS RFM_WT,ISNULL(ROUND(SUM(RFM_AMT)/100000,2),0) AS RFM_AMT  FROM  #temp1  UNION  SELECT 'MACHINE SHOP Store' AS STORE_NAME,ROUND(SUM(MACHINESHOP),2) AS QTY ,  ISNULL(ROUND(SUM(MACHINESHOP_WT)/1000,2),0) AS MACHINESHOP_WT,ISNULL(ROUND(SUM(MACHINESHOP_AMT)/100000,2),0) AS MACHINESHOP_AMT  FROM  #temp1  UNION   SELECT 'RFI Store' AS STORE_NAME,ROUND(SUM(RFI),2) AS QTY,ISNULL(ROUND(SUM(RFI_WT)/1000,2),0) AS RFI_WT,ISNULL(ROUND(SUM(RFI_AMT)/100000,2),0) AS RFI_AMT  FROM  #temp1  UNION   SELECT 'RFD Store' AS STORE_NAME,ROUND(SUM(RFD),2) AS QTY,ISNULL(ROUND(SUM(RFD_WT)/1000,2),0) AS RFD_WT,ISNULL(ROUND(SUM(RFD_AMT)/100000,2),0) AS RFD_AMT  FROM  #temp1  UNION   SELECT 'Rejection Store' AS STORE_NAME,ROUND(SUM(Rejection),2) AS QTY,ISNULL(ROUND(SUM(Rejection_WT)/1000,2),0) AS Rejection_WT,ISNULL(ROUND(SUM(Rejection_AMT)/100000,2),0) AS Rejection_AMT  FROM  #temp1  UNION    SELECT 'Casting Rework' AS STORE_NAME,ROUND(SUM(CastingRework),2) AS QTY,ISNULL(ROUND(SUM(CastingRework_WT)/1000,2),0) AS CastingRework_WT,ISNULL(ROUND(SUM(CastingRework_AMT)/100000,2),0) AS CastingRework_AMT  FROM  #temp1  UNION  SELECT 'MACHINE Rework' AS STORE_NAME,ROUND(SUM(MACHINERework),2) AS QTY,ISNULL(ROUND(SUM(MACHINERework_WT)/1000,2),0) AS MACHINERework_WT,ISNULL(ROUND(SUM(MACHINERework_AMT)/100000,2),0) AS MACHINERework_AMT   FROM  #temp1   DROP TABLE #temp1   ");
            DataTable dtLoadStock = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME ,I_UWEIGHT,I_INV_RATE ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER where STL_STORE_TYPE=-2147483648 AND STL_I_CODE= I_CODE),0) AS INWARD,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER where STL_STORE_TYPE=-2147483647 AND STL_I_CODE= I_CODE),0) AS MAIN,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483646 AND STL_I_CODE= I_CODE),0) AS FOUNDRY  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483645 AND STL_I_CODE= I_CODE),0) AS RFM,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483644 AND STL_I_CODE= I_CODE),0) AS MACHINESHOP  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483643 AND STL_I_CODE= I_CODE),0) AS RFI  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483642 AND STL_I_CODE= I_CODE),0) AS RFD  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483641 AND STL_I_CODE= I_CODE),0) AS Rejection   ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483640 AND STL_I_CODE= I_CODE),0) AS CastingRework  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483639 AND STL_I_CODE= I_CODE),0) AS MACHINERework into #temp  FROM ITEM_MASTER where I_CAT_CODE=-2147483648 AND ES_DELETE=0  SELECT I_CODE,I_CODENO,I_NAME,INWARD,MAIN,FOUNDRY,RFM,MACHINESHOP,RFI,RFD,Rejection,CastingRework,MACHINERework, I_UWEIGHT *INWARD AS INWARD_WT, I_INV_RATE  *INWARD AS INWARD_AMT,I_UWEIGHT*MAIN AS MAIN_WT,I_INV_RATE*MAIN AS MAIN_AMT,I_UWEIGHT*FOUNDRY AS FOUNDRY_WT,I_INV_RATE*FOUNDRY AS FOUNDRY_AMT,I_UWEIGHT*RFM AS RFM_WT,I_INV_RATE*RFM AS RFM_AMT,I_UWEIGHT*MACHINESHOP AS MACHINESHOP_WT,I_INV_RATE*MACHINESHOP AS MACHINESHOP_AMT,I_UWEIGHT*RFI AS RFI_WT,I_INV_RATE*RFI AS RFI_AMT,I_UWEIGHT*RFD AS RFD_WT,I_INV_RATE*RFD AS RFD_AMT,I_UWEIGHT*Rejection AS Rejection_WT,I_INV_RATE*Rejection AS Rejection_AMT,I_UWEIGHT*CastingRework AS CastingRework_WT,I_INV_RATE*CastingRework AS CastingRework_AMT,I_UWEIGHT*MACHINERework AS MACHINERework_WT, I_INV_RATE*MACHINERework AS MACHINERework_AMT  into #temp1  FROM  #temp DROP TABLE #temp        SELECT   1 As SRNO,'Inward Store' AS STORE_NAME, ROUND(SUM(INWARD),2) AS QTY, ISNULL(ROUND(SUM(INWARD_WT)/1000,2),0) AS INWARD_WT,ISNULL(ROUND(SUM(INWARD_AMT)/100000,2),0) AS INWARD_AMT  FROM  #temp1  UNION   SELECT  2 As SRNO,'Main Store' AS STORE_NAME, ROUND(SUM(MAIN),2) AS QTY,  ISNULL(ROUND(SUM(MAIN_WT)/1000,2),0) AS MAIN_WT,ISNULL(ROUND(SUM(MAIN_AMT)/100000,2),0) AS MAIN_AMT  FROM  #temp1  UNION  SELECT   3 As SRNO, 'Foundry Store' AS STORE_NAME,ROUND(SUM(FOUNDRY),2) AS QTY ,ISNULL(ROUND(SUM(FOUNDRY_WT)/1000,2),0) AS FOUNDRY_WT,ISNULL(ROUND(SUM(FOUNDRY_AMT)/100000,2),0) AS FOUNDRY_AMT  FROM  #temp1  UNION    SELECT 5 As SRNO,'RFM Store' AS STORE_NAME,ROUND(SUM(RFM),2) AS QTY , ISNULL(ROUND(SUM(RFM_WT)/1000,2),0) AS RFM_WT,ISNULL(ROUND(SUM(RFM_AMT)/100000,2),0) AS RFM_AMT  FROM  #temp1  UNION  SELECT   6 As SRNO,'MACHINE SHOP Store' AS STORE_NAME,ROUND(SUM(MACHINESHOP),2) AS QTY ,  ISNULL(ROUND(SUM(MACHINESHOP_WT)/1000,2),0) AS MACHINESHOP_WT,ISNULL(ROUND(SUM(MACHINESHOP_AMT)/100000,2),0) AS MACHINESHOP_AMT  FROM  #temp1  UNION   SELECT 8 As SRNO, 'RFI Store' AS STORE_NAME,ROUND(SUM(RFI),2) AS QTY,ISNULL(ROUND(SUM(RFI_WT)/1000,2),0) AS RFI_WT,ISNULL(ROUND(SUM(RFI_AMT)/100000,2),0) AS RFI_AMT  FROM  #temp1  UNION   SELECT  10 As SRNO, 'RFD Store' AS STORE_NAME,ROUND(SUM(RFD),2) AS QTY,ISNULL(ROUND(SUM(RFD_WT)/1000,2),0) AS RFD_WT,ISNULL(ROUND(SUM(RFD_AMT)/100000,2),0) AS RFD_AMT  FROM  #temp1  UNION   SELECT  9 As SRNO,'Rejection Store' AS STORE_NAME,ROUND(SUM(Rejection),2) AS QTY,ISNULL(ROUND(SUM(Rejection_WT)/1000,2),0) AS Rejection_WT,ISNULL(ROUND(SUM(Rejection_AMT)/100000,2),0) AS Rejection_AMT  FROM  #temp1  UNION    SELECT  4 As SRNO,'Casting Rework' AS STORE_NAME,ROUND(SUM(CastingRework),2) AS QTY,ISNULL(ROUND(SUM(CastingRework_WT)/1000,2),0) AS CastingRework_WT,ISNULL(ROUND(SUM(CastingRework_AMT)/100000,2),0) AS CastingRework_AMT  FROM  #temp1  UNION  SELECT  7 As SRNO,'MACHINE Rework' AS STORE_NAME,ROUND(SUM(MACHINERework),2) AS QTY,ISNULL(ROUND(SUM(MACHINERework_WT)/1000,2),0) AS MACHINERework_WT,ISNULL(ROUND(SUM(MACHINERework_AMT)/100000,2),0) AS MACHINERework_AMT   FROM  #temp1   DROP TABLE #temp1 ");

            DataTable dtLoadStockVendorF = CommonClasses.Execute(" SELECT * into #temp1 FROM (  SELECT  ISNULL(SUM(CL_CQTY), 0) AS Q,  ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE AS A,CL_I_CODE,I_CODENO , ISNULL(SUM(CL_CQTY), 0) * I_UWEIGHT   AS TONS  FROM CHALLAN_STOCK_LEDGER ,ITEM_MASTER WHERE  CL_I_CODE=I_CODE  AND (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP')) AND (CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy") + "'  and  '" + Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy") + "' AND I_CAT_CODE=-2147483648   )    GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO,I_UWEIGHT UNION  SELECT  ISNULL(SUM(CL_CQTY), 0) aS Q, ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE AS A,CL_I_CODE,I_CODENO , ISNULL(SUM(CL_CQTY), 0) * I_UWEIGHT   AS TONS   FROM CHALLAN_STOCK_LEDGER,ITEM_MASTER WHERE  CL_I_CODE=I_CODE   AND  (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP'))    AND   CL_DOC_DATE <  '" + Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy") + "'     AND I_CAT_CODE=-2147483648      GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO,I_CODENO   ,I_UWEIGHT  ) AS addd   SELECT  11 As SRNO,'Vendor Finish' AS STORE_NAME,  round(SUM(Q),2) as QTY, round(SUM(TONS)/1000,2)  As TONS , round(SUM(A)/100000,2)   As AMT   FROM #temp1   DROP TABLE #temp1 ");

            DataTable dtLoadStockVendorR = CommonClasses.Execute(" SELECT * into #temp1 FROM (  SELECT  ISNULL(SUM(CL_CQTY), 0) AS Q,  ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE AS A,CL_I_CODE,I_CODENO , ISNULL(SUM(CL_CQTY), 0) * I_UWEIGHT   AS TONS  FROM CHALLAN_STOCK_LEDGER ,ITEM_MASTER WHERE  CL_I_CODE=I_CODE  AND (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP')) AND (CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy") + "'  and  '" + Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy") + "' AND I_CAT_CODE=-2147483635   )    GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO,I_UWEIGHT UNION  SELECT  ISNULL(SUM(CL_CQTY), 0) aS Q, ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE AS A,CL_I_CODE,I_CODENO , ISNULL(SUM(CL_CQTY), 0) * I_UWEIGHT   AS TONS   FROM CHALLAN_STOCK_LEDGER,ITEM_MASTER WHERE  CL_I_CODE=I_CODE   AND  (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP'))    AND   CL_DOC_DATE <  '" + Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy") + "'     AND I_CAT_CODE=-2147483635      GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO,I_CODENO   ,I_UWEIGHT  ) AS addd   SELECT  12 As SRNO,'Vendor Raw' AS STORE_NAME,  0 as QTY, round(SUM(Q)/1000,2)  As TONS, round(SUM(A)/100000,2)   As AMT    FROM #temp1   DROP TABLE #temp1 ");

            DataTable dtRawMaterial = CommonClasses.Execute("SELECT * into #temp1 FROM (SELECT I_CODE, ROUND(SUM(STL_DOC_QTY)/1000,2) aS TONS ,  round((SUM(STL_DOC_QTY)*I_INV_RATE)/100000,2) As AMT FROM ITEM_MASTER,STOCK_LEDGER where I_CAT_CODE=-2147483635 AND STL_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 GROUP BY I_CODE,I_INV_RATE)  AS addd SELECT 13 As SRNO,'Raw Material' AS STORE_NAME,  0 as QTY, round(SUM(TONS),2)  As TONS, round(SUM(AMT),2) As AMT FROM #temp1 DROP TABLE #temp1 ");

            DataRow dr = dtLoadStock.NewRow();
            dr.ItemArray = dtLoadStockVendorF.Rows[0].ItemArray;
            dtLoadStock.Rows.Add(dr);



            DataRow dr1 = dtLoadStock.NewRow();
            dr1.ItemArray = dtLoadStockVendorR.Rows[0].ItemArray;
            dtLoadStock.Rows.Add(dr1);

            DataRow dr2 = dtLoadStock.NewRow();
            dr2.ItemArray = dtRawMaterial.Rows[0].ItemArray;
            dtLoadStock.Rows.Add(dr2);

            dgStoreWiseStock.DataSource = dtLoadStock;
            dgStoreWiseStock.DataBind();
            dgStoreWiseStock.Enabled = false;
            Double QTY = dtLoadStock.AsEnumerable().Sum(row => row.Field<Double>("QTY"));
            Double Weight = dtLoadStock.AsEnumerable().Sum(row => row.Field<Double>("INWARD_WT"));
            Double total = dtLoadStock.AsEnumerable().Sum(row => row.Field<Double>("INWARD_AMT"));
        }
        catch
        {
        }
    }
    #endregion LoadStoreData

    #region LoadRawMaterial
    public void LoadRawMaterial()
    {
        try
        {
            //DataTable dtLoadStock = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME ,I_UWEIGHT,I_INV_RATE ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER where STL_STORE_TYPE=-2147483648 AND STL_I_CODE= I_CODE),0) AS INWARD,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER where STL_STORE_TYPE=-2147483647 AND STL_I_CODE= I_CODE),0) AS MAIN,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483646 AND STL_I_CODE= I_CODE),0) AS FOUNDRY  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483645 AND STL_I_CODE= I_CODE),0) AS RFM,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483644 AND STL_I_CODE= I_CODE),0) AS MACHINESHOP  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483643 AND STL_I_CODE= I_CODE),0) AS RFI  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483642 AND STL_I_CODE= I_CODE),0) AS RFD  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483641 AND STL_I_CODE= I_CODE),0) AS Rejection   ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483640 AND STL_I_CODE= I_CODE),0) AS CastingRework  ,ISNULL((SELECT SUM(STL_DOC_QTY) AS STL_DOC_QTY FROM STOCK_LEDGER   where STL_STORE_TYPE=-2147483639 AND STL_I_CODE= I_CODE),0) AS MACHINERework into #temp  FROM ITEM_MASTER where I_CAT_CODE=-2147483648 AND ES_DELETE=0  SELECT I_CODE,I_CODENO,I_NAME,INWARD,MAIN,FOUNDRY,RFM,MACHINESHOP,RFI,RFD,Rejection,CastingRework,MACHINERework, I_UWEIGHT *INWARD AS INWARD_WT, I_INV_RATE  *INWARD AS INWARD_AMT,I_UWEIGHT*MAIN AS MAIN_WT,I_INV_RATE*MAIN AS MAIN_AMT,I_UWEIGHT*FOUNDRY AS FOUNDRY_WT,I_INV_RATE*FOUNDRY AS FOUNDRY_AMT,I_UWEIGHT*RFM AS RFM_WT,I_INV_RATE*RFM AS RFM_AMT,I_UWEIGHT*MACHINESHOP AS MACHINESHOP_WT,I_INV_RATE*MACHINESHOP AS MACHINESHOP_AMT,I_UWEIGHT*RFI AS RFI_WT,I_INV_RATE*RFI AS RFI_AMT,I_UWEIGHT*RFD AS RFD_WT,I_INV_RATE*RFD AS RFD_AMT,I_UWEIGHT*Rejection AS Rejection_WT,I_INV_RATE*Rejection AS Rejection_AMT,I_UWEIGHT*CastingRework AS CastingRework_WT,I_INV_RATE*CastingRework AS CastingRework_AMT,I_UWEIGHT*MACHINERework AS MACHINERework_WT, I_INV_RATE*MACHINERework AS MACHINERework_AMT  into #temp1  FROM  #temp DROP TABLE #temp     SELECT  'Inward Store' AS STORE_NAME, ROUND(SUM(INWARD),2) AS QTY, ISNULL(ROUND(SUM(INWARD_WT)/1000,2),0) AS INWARD_WT,ISNULL(ROUND(SUM(INWARD_AMT)/100000,2),0) AS INWARD_AMT  FROM  #temp1  UNION   SELECT  'Main Store' AS STORE_NAME, ROUND(SUM(MAIN),2) AS QTY,  ISNULL(ROUND(SUM(MAIN_WT)/1000,2),0) AS MAIN_WT,ISNULL(ROUND(SUM(MAIN_AMT)/100000,2),0) AS MAIN_AMT  FROM  #temp1  UNION  SELECT 'Foundry Store' AS STORE_NAME,ROUND(SUM(FOUNDRY),2) AS QTY ,ISNULL(ROUND(SUM(FOUNDRY_WT)/1000,2),0) AS FOUNDRY_WT,ISNULL(ROUND(SUM(FOUNDRY_AMT)/100000,2),0) AS FOUNDRY_AMT  FROM  #temp1  UNION  SELECT 'RFM Store' AS STORE_NAME,ROUND(SUM(RFM),2) AS QTY , ISNULL(ROUND(SUM(RFM_WT)/1000,2),0) AS RFM_WT,ISNULL(ROUND(SUM(RFM_AMT)/100000,2),0) AS RFM_AMT  FROM  #temp1  UNION  SELECT 'MACHINE SHOP Store' AS STORE_NAME,ROUND(SUM(MACHINESHOP),2) AS QTY ,  ISNULL(ROUND(SUM(MACHINESHOP_WT)/1000,2),0) AS MACHINESHOP_WT,ISNULL(ROUND(SUM(MACHINESHOP_AMT)/100000,2),0) AS MACHINESHOP_AMT  FROM  #temp1  UNION   SELECT 'RFI Store' AS STORE_NAME,ROUND(SUM(RFI),2) AS QTY,ISNULL(ROUND(SUM(RFI_WT)/1000,2),0) AS RFI_WT,ISNULL(ROUND(SUM(RFI_AMT)/100000,2),0) AS RFI_AMT  FROM  #temp1  UNION   SELECT 'RFD Store' AS STORE_NAME,ROUND(SUM(RFD),2) AS QTY,ISNULL(ROUND(SUM(RFD_WT)/1000,2),0) AS RFD_WT,ISNULL(ROUND(SUM(RFD_AMT)/100000,2),0) AS RFD_AMT  FROM  #temp1  UNION   SELECT 'Rejection Store' AS STORE_NAME,ROUND(SUM(Rejection),2) AS QTY,ISNULL(ROUND(SUM(Rejection_WT)/1000,2),0) AS Rejection_WT,ISNULL(ROUND(SUM(Rejection_AMT)/100000,2),0) AS Rejection_AMT  FROM  #temp1  UNION    SELECT 'Casting Rework' AS STORE_NAME,ROUND(SUM(CastingRework),2) AS QTY,ISNULL(ROUND(SUM(CastingRework_WT)/1000,2),0) AS CastingRework_WT,ISNULL(ROUND(SUM(CastingRework_AMT)/100000,2),0) AS CastingRework_AMT  FROM  #temp1  UNION  SELECT 'MACHINE Rework' AS STORE_NAME,ROUND(SUM(MACHINERework),2) AS QTY,ISNULL(ROUND(SUM(MACHINERework_WT)/1000,2),0) AS MACHINERework_WT,ISNULL(ROUND(SUM(MACHINERework_AMT)/100000,2),0) AS MACHINERework_AMT   FROM  #temp1   DROP TABLE #temp1   ");
            DataTable dtLoadStock = CommonClasses.Execute("SELECT ROUND(SUM(STL_DOC_QTY)/1000,2) aS STOCK FROM ITEM_MASTER,STOCK_LEDGER where I_CAT_CODE=-2147483635 AND STL_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 ");
            if (dtLoadStock.Rows.Count > 0)
            {
                Double SubConNetAmtYearly = Convert.ToDouble(dtLoadStock.Rows[0]["STOCK"]);

                // lblrawMaterial.Text = SubConNetAmtYearly.ToString();
            }

            //Double QTY = dtLoadStock.AsEnumerable().Sum(row => row.Field<Double>("QTY"));
            //Double Weight = dtLoadStock.AsEnumerable().Sum(row => row.Field<Double>("INWARD_WT"));
            //Double total = dtLoadStock.AsEnumerable().Sum(row => row.Field<Double>("INWARD_AMT"));
        }
        catch
        {
        }
    }
    #endregion LoadStoreData

    #region LoadFilterStore
    public void LoadFilterStore()
    {
        dtStoreFilter.Clear();

        if (dtStoreFilter.Columns.Count == 0)
        {
            dtStoreFilter.Columns.Add(new System.Data.DataColumn("STORE_NAME", typeof(String)));
            dtStoreFilter.Columns.Add(new System.Data.DataColumn("QTY", typeof(String)));
            dtStoreFilter.Columns.Add(new System.Data.DataColumn("INWARD_WT", typeof(String)));
            dtStoreFilter.Columns.Add(new System.Data.DataColumn("INWARD_AMT", typeof(String)));
            dtStoreFilter.Rows.Add("Total", 0, 0, 0);
            dgStoreWiseStock.DataSource = dtStoreFilter;
            dgStoreWiseStock.DataBind();
            dgStoreWiseStock.Enabled = false;
        }
    }
    #endregion LoadFilter

    Double Qty = 0, wt = 0, amt = 0;
    #region GridView1_RowDataBound
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblQty = (Label)e.Row.FindControl("lblQTY");
            Label lblwt = (Label)e.Row.FindControl("lblINWARD_WT");
            Label lblamt = (Label)e.Row.FindControl("lblINWARD_AMT");

            double price = double.Parse(lblQty.Text);
            double stock = double.Parse(lblwt.Text);
            double stock1 = double.Parse(lblamt.Text);

            Qty += price;
            wt += stock;
            amt += stock1;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTOTQTY = (Label)e.Row.FindControl("lblTOTQTY");
            Label lblTOTWT = (Label)e.Row.FindControl("lblTOTWT");
            Label lblTOTAMT = (Label)e.Row.FindControl("lblTOTAMT");

            lblTOTQTY.Text = Qty.ToString();
            lblTOTWT.Text = wt.ToString();
            lblTOTAMT.Text = amt.ToString();
        }
    }
    #endregion

    #region LoadData
    public void LoadData()
    {
        try
        {
            DateTime dtfrom = new DateTime();
            DataTable dtLoadStock = CommonClasses.Execute("SELECT * into #Temp FROM ( SELECT SUM(STL_DOC_QTY) AS QTY,I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME    FROM STOCK_LEDGER,ITEM_MASTER,ITEM_CATEGORY_MASTER where STL_I_CODE=I_CODE  AND ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE  AND  (STL_DOC_DATE between   '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  and   '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "')  GROUP BY I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME   UNION SELECT SUM(STL_DOC_QTY) AS QTY,I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME FROM STOCK_LEDGER,ITEM_MASTER,ITEM_CATEGORY_MASTER where STL_I_CODE=I_CODE  AND ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE  AND  (STL_DOC_DATE <   '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' )  GROUP BY I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME ) AS aa SELECT ROUND((SUM(QTY*I_INV_RATE))/100000,2) AS VALUE,I_CAT_NAME,I_CAT_CODE  FROM #Temp   where I_CAT_CODE   IN  (  -2147483647,-2147483646,-2147483645,-2147483648,-2147483644,-2147483643,-2147483642,-2147483641,-2147483638,-2147483637,-2147483619,-2147483636,-2147483621,-2147483628,-2147483635,-2147483634,-2147483631 ) GROUP BY I_CAT_CODE,	I_CAT_NAME  DROP TABLE #Temp   ");
            DataTable dtSum = CommonClasses.Execute("SELECT * into #Temp FROM ( SELECT SUM(STL_DOC_QTY) AS QTY,I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME FROM STOCK_LEDGER,ITEM_MASTER,ITEM_CATEGORY_MASTER where STL_I_CODE=I_CODE  AND ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE  AND  (STL_DOC_DATE between   '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  and   '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "')  GROUP BY I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME   UNION SELECT SUM(STL_DOC_QTY) AS QTY,I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME FROM STOCK_LEDGER,ITEM_MASTER,ITEM_CATEGORY_MASTER where STL_I_CODE=I_CODE  AND ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE  AND  (STL_DOC_DATE <   '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' )  GROUP BY I_INV_RATE,I_CODE,ITEM_CATEGORY_MASTER.I_CAT_CODE,I_CAT_NAME ) AS aa SELECT ROUND((SUM(QTY*I_INV_RATE))/100000,2) AS VALUE, 'Inhouse Total :' AS I_CAT_CODE   FROM #Temp    where I_CAT_CODE   IN  (   -2147483647,-2147483646,-2147483645,-2147483648,-2147483644,-2147483643,-2147483642,-2147483641,-2147483638,-2147483637,-2147483619,-2147483636,-2147483621,-2147483628,-2147483635,-2147483634,-2147483631 )   DROP TABLE #Temp    ");
            DataTable dtVendorSum = CommonClasses.Execute("SELECT * into #TempVendor FROM   (SELECT  ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE AS A,CL_I_CODE,I_CODENO FROM CHALLAN_STOCK_LEDGER ,ITEM_MASTER WHERE  CL_I_CODE=I_CODE AND (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP')) AND (CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  and '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND I_CAT_CODE=-2147483648) GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO UNION  SELECT  ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE  AS A,CL_I_CODE ,I_CODENO   FROM CHALLAN_STOCK_LEDGER,ITEM_MASTER WHERE CL_I_CODE=I_CODE AND  (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP')) AND (CL_DOC_DATE <  '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'    AND I_CAT_CODE=-2147483648 )    GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO,I_CODENO ) AS aa SELECT 'Total :' AS I_CAT_CODE, ISNULL(ROUND(SUM(#TempVendor.A)/100000,2),0) AS VALUE FROM #TempVendor  DROP TABLE #TempVendor   ");

            DataTable dtVendorSumRaw = CommonClasses.Execute("SELECT * into #TempVendor FROM (SELECT ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE AS A,CL_I_CODE,I_CODENO FROM CHALLAN_STOCK_LEDGER ,ITEM_MASTER WHERE CL_I_CODE=I_CODE AND (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP')) AND (CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  and '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND I_CAT_CODE=-2147483635) GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO UNION  SELECT  ISNULL(SUM(CL_CQTY), 0)*I_INV_RATE  AS A,CL_I_CODE ,I_CODENO FROM CHALLAN_STOCK_LEDGER,ITEM_MASTER WHERE  CL_I_CODE=I_CODE AND  (CL_DOC_TYPE IN ( 'OutSUBINM','IWIAP')) AND (CL_DOC_DATE <  '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'    AND I_CAT_CODE=-2147483635 )    GROUP BY CL_I_CODE,I_INV_RATE,I_CODENO,I_CODENO ) AS aa SELECT 'Total :' AS I_CAT_CODE, ISNULL(ROUND(SUM(#TempVendor.A)/100000,2),0) AS VALUE FROM #TempVendor DROP TABLE #TempVendor");

            double Sum = 0; double Sum1 = 0; double Sum2 = 0;
            StockValuation.Visible = true;
            if (dtLoadStock.Rows.Count > 0)
            {
                DataRow dr = dtLoadStock.NewRow();
                dr["I_CAT_NAME"] = dtSum.Rows[0]["I_CAT_CODE"].ToString();
                dr["VALUE"] = dtSum.Rows[0]["VALUE"].ToString();
                dtLoadStock.Rows.Add(dr);
            }
            else
            {
                DataRow dr = dtLoadStock.NewRow();
                dr["I_CAT_NAME"] = "";
                dr["VALUE"] = "0";
                dtLoadStock.Rows.Add(dr);
            }
            if (dtVendorSum.Rows.Count > 0)
            {
                DataRow drVendor = dtLoadStock.NewRow();
                drVendor["I_CAT_NAME"] = "Vendor Finished Goods :";
                drVendor["VALUE"] = dtVendorSum.Rows[0]["VALUE"].ToString();
                dtLoadStock.Rows.Add(drVendor);
            }
            else
            {
                DataRow drVendor = dtLoadStock.NewRow();
                drVendor["I_CAT_NAME"] = "Vendor Finished Goods :";
                drVendor["VALUE"] = "0";
                dtLoadStock.Rows.Add(drVendor);
            }
            if (dtVendorSumRaw.Rows.Count > 0)
            {
                DataRow drVendor = dtLoadStock.NewRow();
                drVendor["I_CAT_NAME"] = "Vendor Raw material:";
                drVendor["VALUE"] = dtVendorSumRaw.Rows[0]["VALUE"].ToString();
                dtLoadStock.Rows.Add(drVendor);
            }
            else
            {
                DataRow drVendor = dtLoadStock.NewRow();
                drVendor["I_CAT_NAME"] = "Vendor Raw material:";
                drVendor["VALUE"] = "0";
                dtLoadStock.Rows.Add(drVendor);
            }
            if (dtSum.Rows.Count > 0)
            {
                Sum1 = Convert.ToDouble(dtSum.Rows[0]["VALUE"].ToString());
                //Sum2 = Convert.ToDouble(dtVendorSum.Rows[0]["VALUE"].ToString());
            }

            Sum2 = Convert.ToDouble(dtVendorSum.Rows[0]["VALUE"].ToString());
            Sum = Sum1 + Sum2 + Convert.ToDouble(dtVendorSumRaw.Rows[0]["VALUE"].ToString());
            DataRow drSum = dtLoadStock.NewRow();
            drSum["I_CAT_NAME"] = "Amount :";
            drSum["VALUE"] = Sum;
            dtLoadStock.Rows.Add(drSum);

            dgDashboard.DataSource = dtLoadStock;
            dgDashboard.DataBind();
            dgDashboard.Enabled = false;
        }
        catch
        {

        }
    }
    #endregion LoadData

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtFromDate.Focus();
                return;
            }
            LoadData();
        }
    }
    #endregion txtFromDate_TextChanged

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtToDate.Focus();
                return;
            }
            LoadData();
        }
    }
    #endregion txtToDate_TextChanged

    #region VerifyRenderingInServerForm
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    #endregion

    #region btnSHow_Click
    protected void btnSHow_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "AS" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            dgDashboard.GridLines = GridLines.Both;
            dgDashboard.HeaderStyle.Font.Bold = true;
            dgDashboard.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion

    #region checkRights
    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }
    #endregion

    #region btnConfirm_Click
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            checkRights(148);
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                Response.Redirect("~/Transactions/VIEW/ViewMaterialAcceptance.aspx", false);
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region LoadActivity
    private void LoadActivity()
    {
        try
        {
            string str = "";

            DataTable checkExist = CommonClasses.Execute(" SELECT  DISTINCT ISSUE_MASTER.IM_CODE, 'ACCEPT FROM '+ CONVERT(VARCHAR,STORE_MASTER.STORE_NAME)+' - '+  CONVERT(VARCHAR,TOStore.STORE_NAME) AS TYPE, I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO  ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE  ISSUE_MASTER.ES_DELETE=0 AND  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1      AND ISSUE_MASTER.IM_CODE   IN  (SELECT STL_DOC_NO FROM STOCK_LEDGER where STL_DOC_NO= ISSUE_MASTER.IM_CODE  AND STL_DOC_NUMBER= IM_NO AND STL_STORE_TYPE= TOStore.STORE_CODE  AND STL_DOC_TYPE ='ACCEPT FROM '+ CONVERT(VARCHAR,STORE_MASTER.STORE_NAME)+' - '+  CONVERT(VARCHAR,TOStore.STORE_NAME)  AND STL_I_CODE=I_CODE)  ORDER BY IM_CODE DESC ");
            if (checkExist.Rows.Count > 0)
            {
                for (int i = 0; i < checkExist.Rows.Count; i++)
                {
                    CommonClasses.Execute(" UPDATE ISSUE_MASTER_DETAIL SET IMD_STORE_TYPE=1 where IM_CODE='" + checkExist.Rows[i]["IM_CODE"].ToString() + "'  AND IMD_I_CODE='" + checkExist.Rows[i]["I_CODE"].ToString() + "'  AND IMD_To_STORE='" + checkExist.Rows[i]["TO_STORE_CODE"].ToString() + "'");
                }
            }

            DataTable checkExistRej = CommonClasses.Execute("  SELECT RTF_CODE,RTF_DOC_NO,RTF_I_CODE   FROM REJECTION_TO_FOUNDRY_MASTER where RTF_ISUSED=0 AND RTF_CODE IN (SELECT STL_DOC_NO FROM STOCK_LEDGER where  STL_DOC_NO=RTF_CODE AND STL_DOC_NUMBER=RTF_DOC_NO  AND STL_I_CODE=RTF_I_CODE  AND STL_DOC_TYPE='ACCEPT FROM Rejection Store - Main Store' AND STL_STORE_TYPE=-2147483647)");
            if (checkExistRej.Rows.Count > 0)
            {
                for (int i = 0; i < checkExistRej.Rows.Count; i++)
                {
                    CommonClasses.Execute(" UPDATE REJECTION_TO_FOUNDRY_MASTER SET RTF_ISUSED=1 where  RTF_DOC_NO='" + checkExistRej.Rows[i]["RTF_DOC_NO"].ToString() + "'  AND  RTF_CODE='" + checkExistRej.Rows[i]["RTF_CODE"].ToString() + "'  AND RTF_I_CODE='" + checkExistRej.Rows[i]["RTF_I_CODE"].ToString() + "'   ");
                }
            }


            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("   SELECT  STORE_SRNO, STORE_NAME , COUNT(IMD_CODE) AS RECOUNT FROM ISSUE_MASTER,ISSUE_MASTER_DETAIL,USER_STORE_DETAIL,STORE_MASTER where ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE AND ISSUE_MASTER.ES_DELETE=0 AND ISSUE_MASTER_DETAIL.IMD_STORE_TYPE=0 AND IM_TRANS_TYPE=1 AND USER_STORE_DETAIL.STORE_CODE=IMD_To_STORE  AND USER_STORE_DETAIL.STORE_CODE=STORE_MASTER.STORE_CODE  AND UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' GROUP BY STORE_NAME,STORE_SRNO ORDER BY STORE_SRNO");
            if (dt.Rows.Count == 0)
            {
                dgActivity_Task.Enabled = false;
                dt.Rows.Add(dt.NewRow());
                dgActivity_Task.DataSource = dt;
                dgActivity_Task.DataBind();
                dgActivity_Task.Enabled = false;
                popUpPanel5.Visible = false;
                ModalCancleConfirmation.Hide();
            }
            else
            {
                dgActivity_Task.Enabled = true;
                dgActivity_Task.DataSource = dt;
                dgActivity_Task.DataBind();
                dgActivity_Task.Enabled = true;
                popUpPanel5.Visible = true;
                ModalCancleConfirmation.Show();
            }
            Session["DASHBOARD"] = "0";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Activity Transaction - View", "LoadActivity", Ex.Message);
        }
    }
    #endregion

    #region dgActivity_Task_PageIndexChanging
    protected void dgActivity_Task_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgActivity_Task.PageIndex = e.NewPageIndex;
            popUpPanel5.Visible = true;
            ModalCancleConfirmation.Show();
            LoadActivity();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region LoadTotSale
    private void LoadTotSale()
    {
        DateTime dtOpenDate = Convert.ToDateTime(Session["CompanyOpeningDate"]);
        DateTime CurrDate = DateTime.Now.Date;
        var firstDayOfMonth = new DateTime(CurrDate.Year, CurrDate.Month, 1);


        DL_DBAccess = new DatabaseAccessLayer();

        SqlParameter[] par1 = new SqlParameter[3];
        par1[0] = new SqlParameter("@CompanyId", Session["CompanyId"].ToString());
        par1[1] = new SqlParameter("@OpeningDate", Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy"));
        par1[2] = new SqlParameter("@firtstdate", Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy"));
        DataTable dtTotSale = DL_DBAccess.SelectData("LoadSales", par1);



        if (dtTotSale.Rows.Count > 0)
        {

            lblTillDateNetAmt.Text = dtTotSale.Rows[0]["INM_NET_AMTYearly"].ToString() + " Lacs";
            lblCurrMonthNetAmt.Text = dtTotSale.Rows[0]["INM_NET_AMTMonthly"].ToString() + " Lacs";
            lblOnDateNetAmt.Text = dtTotSale.Rows[0]["INM_NET_AMTDaily"].ToString() + " Lacs";
        }


        //DataTable dtTotSaleMonthly = CommonClasses.Execute("select isnull(round(sum(INM_G_AMT),2),0) as INM_G_AMT, isnull(round(sum(INM_NET_AMT),2),0) AS INM_NET_AMT from INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE and INM_TYPE in ('TAXINV','OutJWINM') and INM_DATE between '" + dtOpenDate.ToString("dd MMM yyyy") + "'  and  '" + CurrDate.ToString("dd MMM yyyy") + "' and INVOICE_MASTER.ES_DELETE=0 AND ISNULL(INM_INV_TYPE,0)=0");
        //if (dtTotSaleMonthly.Rows.Count > 0)
        //{
        //    Double SaleNetAmtMonthly = Convert.ToDouble(dtTotSaleMonthly.Rows[0]["INM_NET_AMT"]);
        //    lblTillDateNetAmt.Text = Math.Round((SaleNetAmtMonthly / 100000), 2).ToString() + " Lacs";
        //}
        //DataTable dtTotSaleYearly = CommonClasses.Execute("select isnull(round(sum(INM_G_AMT),2),0) as INM_G_AMT, isnull(round(sum(INM_NET_AMT),2),0) AS INM_NET_AMT from INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE and INM_TYPE in ('TAXINV','OutJWINM') and INM_DATE between '" + firstDayOfMonth.ToString("dd MMM yyyy") + "'  and  '" + CurrDate.ToString("dd MMM yyyy") + "' and INVOICE_MASTER.ES_DELETE=0  AND ISNULL(INM_INV_TYPE,0)=0");
        //if (dtTotSaleYearly.Rows.Count > 0)
        //{
        //    Double SaleNetAmtYearly = Convert.ToDouble(dtTotSaleYearly.Rows[0]["INM_NET_AMT"]);
        //    lblCurrMonthNetAmt.Text = Math.Round((SaleNetAmtYearly / 100000), 2).ToString() + " Lacs";
        //}
        //DataTable dtTotSaleDaily = CommonClasses.Execute("select isnull(round(sum(INM_G_AMT),2),0) as INM_G_AMT, isnull(round(sum(INM_NET_AMT),2),0) AS INM_NET_AMT from INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE and INM_TYPE in ('TAXINV','OutJWINM') and INM_DATE between '" + CurrDate.ToString("dd MMM yyyy") + "'  and  '" + CurrDate.ToString("dd MMM yyyy") + "' and INVOICE_MASTER.ES_DELETE=0  AND ISNULL(INM_INV_TYPE,0)=0");
        //if (dtTotSaleDaily.Rows.Count > 0)
        //{
        //    Double SaleNetAmtDaily = Convert.ToDouble(dtTotSaleDaily.Rows[0]["INM_NET_AMT"]);
        //    lblOnDateNetAmt.Text = Math.Round((SaleNetAmtDaily / 100000), 2).ToString() + " Lacs";
        //}
    }
    #endregion

    #region dgTotSubCon_RowDataBound
    protected void dgTotSubCon_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    #endregion

    #region LoadTotSubContractor
    private void LoadTotSubContractor()
    {
        DateTime dtOpenDate = Convert.ToDateTime(Session["CompanyOpeningDate"]);
        DateTime CurrDate = DateTime.Now.Date;
        var firstDayOfMonth = new DateTime(CurrDate.Year, CurrDate.Month, 1);

        DataTable dtTotSUbContractor = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME,ISNULL((SELECT isnull(ROUND(SUM((IWD_RATE * IWD_REV_QTY)),2),0) AS AMT  FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='OUTCUSTINV' AND IWD_I_CODE=I_CODE and IWM_DATE between '" + firstDayOfMonth.ToString("dd MMM yyyy") + "'  and  '" + CurrDate.ToString("dd MMM yyyy") + "' ),0) AS MONTHLY,ISNULL((SELECT isnull(ROUND(SUM((IWD_RATE * IWD_REV_QTY)),2),0) AS AMT FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='OUTCUSTINV'AND IWD_I_CODE=I_CODE and IWM_DATE between '" + CurrDate.ToString("dd MMM yyyy") + "'  and  '" + CurrDate.ToString("dd MMM yyyy") + "' ),0) AS Daily,ISNULL((SELECT isnull(ROUND(SUM((IWD_RATE * IWD_REV_QTY)),2),0) AS AMT  FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='OUTCUSTINV' AND IWD_I_CODE=I_CODE and IWM_DATE between '" + dtOpenDate.ToString("dd MMM yyyy") + "'  and  '" + CurrDate.ToString("dd MMM yyyy") + "' ),0) AS YEALY into #temp FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE AND IWM_TYPE='OUTCUSTINV' SELECT ISNULL(SUM(MONTHLY),0) As MONTHLY,ISNULL(SUM(Daily),0) AS Daily,ISNULL(SUM(YEALY),0) AS YEALY FROM #temp DROP TABLE #temp");

        if (dtTotSUbContractor.Rows.Count > 0)
        {
            Double SubConNetAmtYearly = Convert.ToDouble(dtTotSUbContractor.Rows[0]["YEALY"]);
            Double SubConNetAmtDaily = Convert.ToDouble(dtTotSUbContractor.Rows[0]["Daily"]);
            Double SubConNetAmtMonthly = Convert.ToDouble(dtTotSUbContractor.Rows[0]["MONTHLY"]);
            lblTillDateNetAmtSubCon.Text = SubConNetAmtYearly.ToString();
            lblCurrMonthNetAmtSubCon.Text = SubConNetAmtMonthly.ToString();
            lblOnDateNetAmtSubCon.Text = SubConNetAmtDaily.ToString();
            lblTillDateNetAmtSubCon.Text = Math.Round((SubConNetAmtYearly / 100000), 2).ToString() + " Lacs";
            lblCurrMonthNetAmtSubCon.Text = Math.Round((SubConNetAmtMonthly / 100000), 2).ToString() + " Lacs";
            lblOnDateNetAmtSubCon.Text = Math.Round((SubConNetAmtDaily / 100000), 2).ToString() + " Lacs";
        }
    }
    #endregion

    #region LoadTotPurchase
    private void LoadTotPurchase()
    {
        DateTime dtOpenDate = Convert.ToDateTime(Session["CompanyOpeningDate"]);
        DateTime CurrDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MMM/yyyy"));
        var firstDayOfMonth = new DateTime(CurrDate.Year, CurrDate.Month, 1);
        DateTime LastDate = Convert.ToDateTime(firstDayOfMonth).AddMonths(1).AddDays(-1);

        DataTable dtTotPurchase = CommonClasses.Execute("SELECT * INTO #temp FROM (SELECT 0 as I_CODE ,'Raw Material' as I_CODENO ,'AS' as I_NAME , ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS IWD_REV_QTY,ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) AS AMT , 0 AS IWD_REV_QTY_MONTH , 0 AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND IWM_TYPE='IWIM' AND I_CAT_CODE=-2147483635  AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE and IWM_DATE between '" + Convert.ToDateTime(dtOpenDate).ToString("dd/MMM/yyyy") + "'   and   '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'   union SELECT 0 as I_CODE ,'Raw Material' as I_CODENO ,'AS' as I_NAME , 0 AS IWD_REV_QTY ,0 AS AMT , ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS IWD_REV_QTY_MONTH , ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND IWM_TYPE='IWIM' AND I_CAT_CODE=-2147483635   AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE and IWM_DATE between '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' and  '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'  union SELECT 0 as I_CODE ,'Raw Material' as I_CODENO ,'AS' as I_NAME , 0 AS IWD_REV_QTY ,0 AS AMT , 0 AS IWD_REV_QTY_MONTH , 0 AS MONTHLYAMT,ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS DAILYQTY, ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND IWM_TYPE='IWIM' AND I_CAT_CODE=-2147483635 AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE and IWM_DATE between  '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'   and   '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'  ) as RawMaterial select * INTO #TempPurchaseAssets from(SELECT 1 as I_CODE ,'Asset Purchase' as I_CODENO ,'AS' as I_NAME , ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS IWD_REV_QTY ,ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) AS AMT , 0 AS IWD_REV_QTY_MONTH , 0 AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND I_CAT_CODE=-2147483629  AND IWM_TYPE='IWIM' AND IWD_I_CODE=I_CODE and IWM_DATE between '" + Convert.ToDateTime(dtOpenDate).ToString("dd/MMM/yyyy") + "'   and   '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'  UNION SELECT 1 as I_CODE ,'Asset Purchase' as I_CODENO ,'AS' as I_NAME , 0 AS IWD_REV_QTY ,0 AS AMT , ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS IWD_REV_QTY_MONTH , ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT  FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND   I_CAT_CODE=-2147483629  AND IWM_TYPE='IWIM' AND IWD_I_CODE=I_CODE and IWM_DATE between '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' and  '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'  UNION SELECT 1 as I_CODE ,'Asset Purchase' as I_CODENO ,'AS' as I_NAME , 0 AS IWD_REV_QTY ,0 AS AMT , 0 AS IWD_REV_QTY_MONTH , 0 AS MONTHLYAMT,ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS DAILYQTY, ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) As DAILYAMT  FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0  AND   I_CAT_CODE=-2147483629  AND IWM_TYPE='IWIM' AND IWD_I_CODE=I_CODE and IWM_DATE between  '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'   and   '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "' ) as PurchaseAsstes SELECT * INTO #TempOther from (   SELECT 2 as I_CODE ,'Other Purchase' as I_CODENO ,'AS' as I_NAME , ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS IWD_REV_QTY , ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) AS AMT , 0 AS IWD_REV_QTY_MONTH , 0 AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND IWM_TYPE='IWIM' AND I_CAT_CODE NOT IN  ( -2147483629,-2147483635) AND  IWD_I_CODE=I_CODE  AND I_CM_COMP_ID = 1 AND      INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE AND IWM_TYPE='IWIM' and IWM_DATE between '" + Convert.ToDateTime(dtOpenDate).ToString("dd/MMM/yyyy") + "'   and   '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'     UNION SELECT 2 as I_CODE ,'Other Purchase' as I_CODENO ,'AS' as I_NAME , 0 AS IWD_REV_QTY ,0 AS AMT , ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS IWD_REV_QTY_MONTH , ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND IWM_TYPE='IWIM' AND I_CAT_CODE NOT IN  ( -2147483629,-2147483635) AND I_CM_COMP_ID = 1  AND  INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE and IWM_DATE between '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' and  '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'      UNION SELECT 2 as I_CODE ,'Other Purchase' as I_CODENO ,'AS' as I_NAME , 0 AS IWD_REV_QTY ,0 AS AMT , 0 AS IWD_REV_QTY_MONTH , 0 AS MONTHLYAMT,ISNULL(ROUND(SUM(IWD_REV_QTY)/1000,2),0) AS DAILYQTY, ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY))/100000,2),0) As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND IWM_TYPE='IWIM' AND I_CAT_CODE NOT IN   ( -2147483629,-2147483635) AND I_CM_COMP_ID = 1  AND  INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE and IWM_DATE between  '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'   and   '" + Convert.ToDateTime(CurrDate).ToString("dd/MMM/yyyy") + "'   ) as Other  select * into #Total from (SELECT 0 as I_CODE,'Raw Material' as I_CODENO,'AS' as I_NAME,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0) AS IWD_REV_QTY, ISNULL(ROUND(SUM(AMT),2),0) AS AMT,ISNULL(ROUND(SUM(IWD_REV_QTY_MONTH),2),0) AS IWD_REV_QTY_MONTH,ISNULL(ROUND(SUM(MONTHLYAMT),2),0) As MONTHLYAMT,Isnull(Round(SUM(DAILYQTY),2),0) As DAILYQTY,ISNULL(ROUND(SUM(DAILYAMT),2),0) As DAILYAMT FROM #Temp UNION SELECT 1 as I_CODE,'Asset Purchase' as I_CODENO,'AS' as I_NAME,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0) AS IWD_REV_QTY, ISNULL(ROUND(SUM(AMT),2),0) AS AMT,ISNULL(ROUND(SUM(IWD_REV_QTY_MONTH),2),0) AS IWD_REV_QTY_MONTH,ISNULL(ROUND(SUM(MONTHLYAMT),2),0) As MONTHLYAMT,Isnull(Round(SUM(DAILYQTY),2),0) As DAILYQTY,ISNULL(ROUND(SUM(DAILYAMT),2),0) As DAILYAMT   from #TempPurchaseAssets  UNION SELECT 2 as I_CODE,'Other Purchase' as I_CODENO,'AS' as I_NAME,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0) AS IWD_REV_QTY, ISNULL(ROUND(SUM(AMT),2),0) AS AMT,ISNULL(ROUND(SUM(IWD_REV_QTY_MONTH),2),0) AS IWD_REV_QTY_MONTH,ISNULL(ROUND(SUM(MONTHLYAMT),2),0) As MONTHLYAMT,Isnull(Round(SUM(DAILYQTY),2),0) As DAILYQTY,ISNULL(ROUND(SUM(DAILYAMT),2),0) As DAILYAMT  FROM #TempOther) as Total  select * from #Total union select 3 as I_CODE,'Total' as I_CODENO,'AS' as I_NAME,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0) AS IWD_REV_QTY, ISNULL(ROUND(SUM(AMT),2),0) AS AMT,ISNULL(ROUND(SUM(IWD_REV_QTY_MONTH),2),0) AS IWD_REV_QTY_MONTH,ISNULL(ROUND(SUM(MONTHLYAMT),2),0) As MONTHLYAMT,Isnull(Round(SUM(DAILYQTY),2),0) As DAILYQTY,ISNULL(ROUND(SUM(DAILYAMT),2),0) As DAILYAMT from #Total DROP TABLE #Temp DROP TABLE #TempPurchaseAssets DROP TABLE  #TempOther DROP TABLE #Total ");
        if (dtTotPurchase.Rows.Count > 0)
        {
            dgTotPurchase.DataSource = dtTotPurchase;
            dgTotPurchase.DataBind();
            dgTotPurchase.Enabled = false;
        }
    }
    #endregion

    protected void dgTotPurchase_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DateTime dtOpenDate = Convert.ToDateTime(Session["CompanyOpeningDate"]);
        DateTime CurrDate = Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MMM/yyyy"));
        var firstDayOfMonth = new DateTime(CurrDate.Year, CurrDate.Month, 1);
        DateTime LastDate = Convert.ToDateTime(firstDayOfMonth).AddMonths(1).AddDays(-1);
        DataTable dtMaterial = new DataTable();
        int Icode = 0;

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grid1 = (GridView)sender;
                int row = e.Row.RowIndex;
                Icode = Convert.ToInt32(grid1.DataKeys[e.Row.RowIndex].Values["I_CODE"].ToString());
                Image ImgP = e.Row.FindControl("ImgP") as Image;

                if (Icode == 0)
                {
                    dtMaterial = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME,ROUND(IWD_REV_QTY/1000,2) AS IWD_REV_QTY,ROUND(AMT/100000,2) AS AMT,ROUND(IWD_REV_QTY_MONTH/1000,2) AS IWD_REV_QTY_MONTH,ROUND(MONTHLYAMT/100000,2) AS MONTHLYAMT,ROUND(DAILYQTY/1000,2) AS DAILYQTY,ROUND(DAILYAMT/100000,2) AS DAILYAMT into #temp1 from (SELECT I_CODE,I_CODENO,I_NAME,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0) AS IWD_REV_QTY ,ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY)),2),0)AS AMT,0 AS IWD_REV_QTY_MONTH ,0 AS MONTHLYAMT,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE AND IWM_TYPE='IWIM' AND I_CODE IN (-2147481794,-2147479681,-2147481791,-2147481792,-2147481790) and IWM_DATE between '" + dtOpenDate.ToString("dd/MMM/yyyy") + "' and '" + CurrDate.ToString("dd/MMM/yyyy") + "' GROUP BY I_CODE,I_CODENO,I_NAME UNION SELECT I_CODE,I_CODENO,I_NAME, 0 AS IWD_REV_QTY, 0 AS AMT,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0) AS IWD_REV_QTY_MONTH ,ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY)),2),0) AS MONTHLYAMT ,0 AS DAILYQTY, 0 As DAILYAMT FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE AND IWM_TYPE='IWIM' AND I_CODE IN (-2147481794,-2147479681,-2147481791,-2147481792,-2147481790) and IWM_DATE between '" + firstDayOfMonth.ToString("dd/MMM/yyyy") + "' and '" + CurrDate.ToString("dd/MMM/yyyy") + "' GROUP BY I_CODE,I_CODENO,I_NAME UNION SELECT I_CODE,I_CODENO,I_NAME, 0 AS IWD_REV_QTY, 0 AS AMT,0 AS IWD_REV_QTY_MONTH ,0 AS MONTHLYAMT,ISNULL(ROUND(SUM(IWD_REV_QTY),2),0)AS DAILYQTY ,ISNULL(ROUND(SUM((IWD_RATE * IWD_REV_QTY)),2),0) AS DAILYAMT  FROM INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWD_I_CODE=I_CODE AND IWM_TYPE='IWIM' AND I_CODE IN (-2147481794,-2147479681,-2147481791,-2147481792,-2147481790) and IWM_DATE between '" + CurrDate.ToString("dd/MMM/yyyy") + "' and '" + CurrDate.ToString("dd/MMM/yyyy") + "' GROUP BY I_CODE,I_CODENO,I_NAME) AS AAA SELECT I_CODE,I_CODENO,I_NAME,SUM(IWD_REV_QTY) AS IWD_REV_QTY,SUM(AMT) AS AMT,SUM(IWD_REV_QTY_MONTH) AS IWD_REV_QTY_MONTH,SUM(MONTHLYAMT) AS MONTHLYAMT,SUM(DAILYQTY) AS DAILYQTY,SUM(DAILYAMT) AS DAILYAMT FROM #temp1  GROUP BY  I_CODE,I_CODENO,I_NAME DROP TABLE #temp1");
                }
                else
                {
                    dtMaterial.Rows.Clear();
                }

                GridView dgMaterial = e.Row.FindControl("dgMaterial") as GridView;

                if (dtMaterial.Rows.Count > 0)
                {
                    dgMaterial.DataSource = dtMaterial;
                    dgMaterial.DataBind();
                }
                else
                {
                    dgMaterial.DataSource = null;
                    dgMaterial.DataBind();
                    ImgP.Visible = false;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dgMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void dgMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
}
