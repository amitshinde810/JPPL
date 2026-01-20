using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine; 
using ZXing;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;
public partial class RoportForms_ADD_Default2 : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string reportType = "";
    string type = "";
    string title = "";
    string Cond = "";
    string chkPrint1 = "";
    string rptType = "";
    string toNo = "";
    string ptype = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";
        GenerateReport();
    }
    #endregion Page_Load

    //#region Page_Init
    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    string Title = "HI";
    //    Cond = Request.QueryString[1];
    //    invoice_code = Request.QueryString[3];
    //    reportType = Request.QueryString[4];

    //    chkPrint1 = Convert.ToInt32(Request.QueryString[2]).ToString();
    //    //}
    //    if (reportType == "Mult")
    //    {
    //        toNo = Request.QueryString[5];
    //        ptype = Request.QueryString[6];
    //        rptType = Request.QueryString[7];
    //    }
    //    else
    //    {
    //        rptType = Request.QueryString[5];
    //    }
    //    GenerateReport(invoice_code);
    //}
    //#endregion Page_Init

    #region GenerateReport
    private void GenerateReport()
    {
        int code = -2147438992;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtTaxInvoice = new DataTable();
        DataTable dtTemp = new DataTable();
        DataSet dsTaxInvoiceGST = new DataSet();
        ReportDocument rptname = null;
        rptname = new ReportDocument();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            //if (reportType == "Single")
            //{
            dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE), ISNULL(INM_LC_NO,0) AS INM_LC_NO,ISNULL(INM_LC_DATE,GETDATE()) AS INM_LC_DATE  ,P_CODE,IND_E_TARIFF_NO AS E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_E_TARIFF_NO,INM_BUYER_NAME,INM_BUYTER_ADD ,INM_PREPARE_BY,case when INM_IS_SUPPLIMENT=1 then 1 else 0 END As INM_IS_SUPPLIMENT,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE, ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT  ,CPOD_DISC_PER,CPOD_GROSS_RATE,Rtrim(ltrim(CPOM_PONO))+',10,'+convert(varchar,cast(isnull(IND_INQTY,0) AS NUMERIC(20,3)))+','+INM_TNO+','+ CONVERT(VARCHAR, INM_DATE, 104)+','+convert(varchar,cast(isnull(CPOD_GROSS_RATE,CPOD_RATE) AS NUMERIC(20,2)))+','+convert(varchar,cast(isnull(IND_RATE,0) AS NUMERIC(20,2)))+','+P_VEND_CODE+','+I_CODENO+','+CONVERT(VARCHAR,cast(isnull(INM_BE_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_EDUC_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_AMT,0) AS NUMERIC(20,2)))+',0.00,'+ CONVERT(VARCHAR,cast(isnull(INM_BEXCISE,0) AS NUMERIC(20,2)))+','+  CONVERT(VARCHAR,cast(isnull(INM_EDUC_CESS,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_CESS,0) AS NUMERIC(20,2)))+',0.00,0.00,'+  CONVERT(VARCHAR,cast(isnull(INM_G_AMT,0) AS NUMERIC(20,2))) +','+IND_E_TARIFF_NO AS QR_CODEPrint,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT    FROM STATE_MASTER,EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INM_TYPE='TAXINV' AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND P_SM_CODE=SM_CODE Order by INM_NO");
            //}
            //else
            //{
            //    if (ptype.ToUpper() == "TRUE")
            //    {
            //        dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),ISNULL(INM_LC_NO,0) AS INM_LC_NO,ISNULL(INM_LC_DATE,GETDATE()) AS INM_LC_DATE  ,P_CODE,IND_E_TARIFF_NO AS E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_E_TARIFF_NO,INM_BUYER_NAME,INM_BUYTER_ADD ,INM_PREPARE_BY,case when INM_IS_SUPPLIMENT=1 then 1 else 0 END As INM_IS_SUPPLIMENT,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE, ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT  ,CPOD_DISC_PER,CPOD_GROSS_RATE,Rtrim(ltrim(CPOM_PONO))+',10,'+convert(varchar,cast(isnull(IND_INQTY,0) AS NUMERIC(20,3)))+ ','+INM_TNO+','+ CONVERT(VARCHAR, INM_DATE, 104)+','+convert(varchar,cast(isnull(CPOD_GROSS_RATE,CPOD_RATE) AS NUMERIC(20,2)))+','+convert(varchar,cast(isnull(IND_RATE,0) AS NUMERIC(20,2)))+','+P_VEND_CODE+','+I_CODENO+','+CONVERT(VARCHAR,cast(isnull(INM_BE_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_EDUC_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_AMT,0) AS NUMERIC(20,2)))+',0.00,'+ CONVERT(VARCHAR,cast(isnull(INM_BEXCISE,0) AS NUMERIC(20,2)))+','+  CONVERT(VARCHAR,cast(isnull(INM_EDUC_CESS,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_CESS,0) AS NUMERIC(20,2)))+',0.00,0.00,'+  CONVERT(VARCHAR,cast(isnull(INM_G_AMT,0) AS NUMERIC(20,2))) +','+IND_E_TARIFF_NO AS QR_CODEPrint,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT     FROM STATE_MASTER,EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND INM_TYPE='TAXINV' AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and INM_NO BETWEEN '" + invoice_code + "' AND '" + toNo + "' AND INM_SUPPLEMENTORY=1 AND   INM_IS_SUPPLIMENT=1 AND P_SM_CODE=SM_CODE Order by INM_NO");
            //    }
            //    else
            //    {
            //        dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),ISNULL(INM_LC_NO,0) AS INM_LC_NO,ISNULL(INM_LC_DATE,GETDATE()) AS INM_LC_DATE  ,P_CODE,IND_E_TARIFF_NO AS E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_E_TARIFF_NO,INM_BUYER_NAME,INM_BUYTER_ADD ,INM_PREPARE_BY,case when INM_IS_SUPPLIMENT=1 then 1 else 0 END As INM_IS_SUPPLIMENT,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE, ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT  ,CPOD_DISC_PER,CPOD_GROSS_RATE,Rtrim(ltrim(CPOM_PONO))+',10,'+convert(varchar,cast(isnull(IND_INQTY,0) AS NUMERIC(20,3)))+','+ INM_TNO+','+ CONVERT(VARCHAR, INM_DATE, 104)+','+convert(varchar,cast(isnull(CPOD_GROSS_RATE,CPOD_RATE) AS NUMERIC(20,2)))+','+convert(varchar,cast(isnull(IND_RATE,0) AS NUMERIC(20,2)))+','+P_VEND_CODE+','+I_CODENO+','+CONVERT(VARCHAR,cast(isnull(INM_BE_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_EDUC_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_AMT,0) AS NUMERIC(20,2)))+',0.00,'+ CONVERT(VARCHAR,cast(isnull(INM_BEXCISE,0) AS NUMERIC(20,2)))+','+  CONVERT(VARCHAR,cast(isnull(INM_EDUC_CESS,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_CESS,0) AS NUMERIC(20,2)))+',0.00,0.00,'+  CONVERT(VARCHAR,cast(isnull(INM_G_AMT,0) AS NUMERIC(20,2))) +','+IND_E_TARIFF_NO AS QR_CODEPrint ,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT  FROM STATE_MASTER,EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND INM_TYPE='TAXINV' AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and INM_NO BETWEEN '" + invoice_code + "' AND '" + toNo + "' AND INM_SUPPLEMENTORY=0 AND   INM_IS_SUPPLIMENT=0  AND P_SM_CODE=SM_CODE Order by INM_NO");
            //    }
            //}
            DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER,STATE_MASTER where CM_STATE=SM_CODE AND CM_ID='" + Session["CompanyId"] + "' AND  CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");
            if (dtComp.Rows.Count > 0)
            {
                if (dtTaxInvoice.Rows.Count > 0)
                {

                    dtTaxInvoice.Columns.Add("QR_CODE", typeof(byte[]));

                    foreach (DataRow row in dtTaxInvoice.Rows)
                    {
                        string strQR = "";
                        strQR = row["QR_CODEPrint"].ToString();
                        //BarCode barcode = new BarCode();
                        //barcode.SymbologyType = SymbologyType.Code128;
                        //DataMatrix datamatrix = new DataMatrix();
                        //datamatrix.CodeText = strQR.ToString().Trim();
                        //datamatrix.X = 3;
                        //byte[] imageData = datamatrix.drawBarcodeAsBytes();
                        //row["QR_CODE"] = (byte[])imageData;

                        //CODE FOR QR CODE GENERATION
                        var QCwriter = new BarcodeWriter();
                        QCwriter.Format = BarcodeFormat.QR_CODE;
                        var result = QCwriter.Write(strQR);
                        string path = Server.MapPath("~/UpLoadPath/MyQRImage.jpg");
                        var barcodeBitmap = new Bitmap(result);
                        byte[] bytes;
                        using (MemoryStream memory = new MemoryStream())
                        {
                            //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                            //{
                                barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                                bytes = memory.ToArray();
                                var base64Data = Convert.ToBase64String(memory.ToArray());
                                imgQR.ImageUrl = "data:image/gif;base64," + base64Data;
                            //}
                        }
                        //imgQR.ImageUrl = path;
                        row["QR_CODE"] = (byte[])bytes;

                        //BarcodeLib.Barcode.QRCode barcode = new BarcodeLib.Barcode.QRCode();
                        //barcode.Data = strQR;

                        //barcode.ModuleSize = 3;
                        //barcode.LeftMargin = 0;
                        //barcode.RightMargin = 0;
                        //barcode.TopMargin = 0;
                        //barcode.BottomMargin = 0;

                        //barcode.Encoding = BarcodeLib.Barcode.QRCodeEncoding.Auto;
                        //barcode.Version = BarcodeLib.Barcode.QRCodeVersion.V1;
                        //barcode.ECL = BarcodeLib.Barcode.QRCodeErrorCorrectionLevel.L;
                        //barcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;


                        //byte[] barcodeInBytes = barcode.drawBarcodeAsBytes();
                        //row["QR_CODE"] = (byte[])barcodeInBytes;
                    }



                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
    }
    #endregion
}
