using System;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using ZXing;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

public partial class RoportForms_ADD_TaxInvoicePrint : System.Web.UI.Page
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
    ReportDocument rptname = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        string Title = Request.QueryString[0];
        Cond = Request.QueryString[1];
        chkPrint1 = Convert.ToInt32(Request.QueryString[2]).ToString();
        invoice_code = Request.QueryString[3];
        reportType = Request.QueryString[4];

        if (reportType == "Mult")
        {
            toNo = Request.QueryString[5];
            ptype = Request.QueryString[6];
            rptType = Request.QueryString[6];
        }
        else
        {
            rptType = Request.QueryString[5];
            ptype = Request.QueryString[5];
        }

        if (Title == "Labour Charge Invoice/Delivery Challan")
        {
            t2.Visible = true;
            this.Title = "Labour Charge Invoice/Delivery Challan";
        }
        else
        {
            t1.Visible = true;
        }
        GenerateReport(invoice_code);
    }
    #endregion Page_Init

    private void GenerateCode(string name)
    {
        var writer = new BarcodeWriter();
        writer.Format = BarcodeFormat.QR_CODE;
        var result = writer.Write(name);
        string path = Server.MapPath("~/images/QRImage.jpg");
        var barcodeBitmap = new Bitmap(result);


        using (MemoryStream memory = new MemoryStream())
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                byte[] bytes = memory.ToArray();
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }

    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtTaxInvoice = new DataTable();
        DataTable dtTemp = new DataTable();
        DataSet dsTaxInvoiceGST = new DataSet();

        rptname = new ReportDocument();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            if (reportType == "Single")
            {
                dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE), ISNULL(INM_LC_NO,0) AS INM_LC_NO,ISNULL(INM_LC_DATE,GETDATE()) AS INM_LC_DATE  ,P_REVERSE_CHARGE,ISNULL(P_QR,0) AS P_QR,P_CODE,IND_E_TARIFF_NO AS E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_E_TARIFF_NO,INM_BUYER_NAME,INM_BUYTER_ADD ,INM_PREPARE_BY,case when INM_IS_SUPPLIMENT=1 then 1 else 0 END As INM_IS_SUPPLIMENT,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE, ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT  ,CPOD_DISC_PER,CPOD_GROSS_RATE,Rtrim(ltrim(CPOM_PONO))+',10,'+convert(varchar,cast(isnull(IND_INQTY,0) AS NUMERIC(20,3)))+','+INM_TNO+','+ CONVERT(VARCHAR, INM_DATE, 104)+','+convert(varchar,cast(isnull(CPOD_GROSS_RATE,CPOD_RATE) AS NUMERIC(20,2)))+','+convert(varchar,cast(isnull(IND_RATE,0) AS NUMERIC(20,2)))+','+P_VEND_CODE+','+I_CODENO+','+CONVERT(VARCHAR,cast(isnull(INM_BE_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_EDUC_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_AMT,0) AS NUMERIC(20,2)))+',0.00,'+ CONVERT(VARCHAR,cast(isnull(INM_BEXCISE,0) AS NUMERIC(20,2)))+','+  CONVERT(VARCHAR,cast(isnull(INM_EDUC_CESS,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_CESS,0) AS NUMERIC(20,2)))+',0.00,0.00,'+  CONVERT(VARCHAR,cast(isnull(INM_G_AMT,0) AS NUMERIC(20,2))) +','+IND_E_TARIFF_NO AS QR_CODEPrint,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT   ,AckNo,AckDate,InvValue,ReciptGSTIn,EInvStatus,IRN,ISNULL(QRCode,0) AS QRCodeEinvPrint,ISNULL(EwayBill,0) AS EwayBill   FROM STATE_MASTER,EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INM_TYPE='TAXINV' AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND P_SM_CODE=SM_CODE Order by INM_NO");
            }
            else
            {
                if (ptype.ToUpper() == "TRUE")
                {
                    dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),ISNULL(INM_LC_NO,0) AS INM_LC_NO,ISNULL(INM_LC_DATE,GETDATE()) AS INM_LC_DATE  ,ISNULL(P_QR,0) AS P_QR,P_CODE,IND_E_TARIFF_NO AS E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_E_TARIFF_NO,INM_BUYER_NAME,INM_BUYTER_ADD ,INM_PREPARE_BY,case when INM_IS_SUPPLIMENT=1 then 1 else 0 END As INM_IS_SUPPLIMENT,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE, ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT  ,CPOD_DISC_PER,CPOD_GROSS_RATE,Rtrim(ltrim(CPOM_PONO))+',10,'+convert(varchar,cast(isnull(IND_INQTY,0) AS NUMERIC(20,3)))+ ','+INM_TNO+','+ CONVERT(VARCHAR, INM_DATE, 104)+','+convert(varchar,cast(isnull(CPOD_GROSS_RATE,CPOD_RATE) AS NUMERIC(20,2)))+','+convert(varchar,cast(isnull(IND_RATE,0) AS NUMERIC(20,2)))+','+P_VEND_CODE+','+I_CODENO+','+CONVERT(VARCHAR,cast(isnull(INM_BE_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_EDUC_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_AMT,0) AS NUMERIC(20,2)))+',0.00,'+ CONVERT(VARCHAR,cast(isnull(INM_BEXCISE,0) AS NUMERIC(20,2)))+','+  CONVERT(VARCHAR,cast(isnull(INM_EDUC_CESS,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_CESS,0) AS NUMERIC(20,2)))+',0.00,0.00,'+  CONVERT(VARCHAR,cast(isnull(INM_G_AMT,0) AS NUMERIC(20,2))) +','+IND_E_TARIFF_NO AS QR_CODEPrint,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT    ,AckNo,AckDate,InvValue,ReciptGSTIn,EInvStatus,IRN,ISNULL(QRCode,0) AS QRCodeEinvPrint,ISNULL(EwayBill,0) AS EwayBill   FROM STATE_MASTER,EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND INM_TYPE='TAXINV' AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and INM_NO BETWEEN '" + invoice_code + "' AND '" + toNo + "' AND INM_SUPPLEMENTORY=1 AND   INM_IS_SUPPLIMENT=1 AND P_SM_CODE=SM_CODE Order by INM_NO");
                }
                else
                {
                    dtTaxInvoice = CommonClasses.Execute("SELECT DISTINCT(I_CODE),ISNULL(INM_LC_NO,0) AS INM_LC_NO,ISNULL(INM_LC_DATE,GETDATE()) AS INM_LC_DATE  ,ISNULL(P_QR,0) AS P_QR,P_CODE,IND_E_TARIFF_NO AS E_TARIFF_NO ,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_TNO,INM_NO,CONVERT(VARCHAR, INM_DATE,103) AS INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,CONVERT(VARCHAR,INM_LR_DATE,103) AS INM_LR_DATE,I_NAME,I_CODENO AS CPOD_CUST_I_CODE,IND_PACK_DESC as CPOD_CUST_I_NAME,CPOM_PONO,ST_TAX_NAME,CONVERT(VARCHAR, CPOM_PO_DATE,103) AS CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) AS IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(20,3)) AS IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) AS IND_RATE,cast(isnull(IND_AMT,0) AS NUMERIC(20,2)) AS IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,CONVERT(VARCHAR,INM_ISSUE_DATE,103) AS INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,EXCISE_TARIFF_MASTER.E_COMMODITY,ITEM_UNIT_MASTER.I_UOM_NAME,INM_TAX_TCS  ,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE ,0 AS [HSN_NO] ,INM_ELECTRREFNUM,'' as INM_ADDRESS,'' as INM_STATE ,ISNULL(P_LBT_NO,'') AS P_GST_NO ,ISNULL( IND_EX_AMT,0) AS IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS IND_E_CESS_AMT,ISNULL(IND_SH_CESS_AMT,0) AS IND_SH_CESS_AMT ,INM_CODE,P_VEND_CODE,SM_NAME,SM_STATE_CODE,IND_E_TARIFF_NO,INM_BUYER_NAME,INM_BUYTER_ADD ,INM_PREPARE_BY,case when INM_IS_SUPPLIMENT=1 then 1 else 0 END As INM_IS_SUPPLIMENT,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE, ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT  ,CPOD_DISC_PER,CPOD_GROSS_RATE,Rtrim(ltrim(CPOM_PONO))+',10,'+convert(varchar,cast(isnull(IND_INQTY,0) AS NUMERIC(20,3)))+','+ INM_TNO+','+ CONVERT(VARCHAR, INM_DATE, 104)+','+convert(varchar,cast(isnull(CPOD_GROSS_RATE,CPOD_RATE) AS NUMERIC(20,2)))+','+convert(varchar,cast(isnull(IND_RATE,0) AS NUMERIC(20,2)))+','+P_VEND_CODE+','+I_CODENO+','+CONVERT(VARCHAR,cast(isnull(INM_BE_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_EDUC_AMT,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_AMT,0) AS NUMERIC(20,2)))+',0.00,'+ CONVERT(VARCHAR,cast(isnull(INM_BEXCISE,0) AS NUMERIC(20,2)))+','+  CONVERT(VARCHAR,cast(isnull(INM_EDUC_CESS,0) AS NUMERIC(20,2)))+','+ CONVERT(VARCHAR,cast(isnull(INM_H_EDUC_CESS,0) AS NUMERIC(20,2)))+',0.00,0.00,'+  CONVERT(VARCHAR,cast(isnull(INM_G_AMT,0) AS NUMERIC(20,2))) +','+IND_E_TARIFF_NO AS QR_CODEPrint ,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT  ,AckNo,AckDate,InvValue,ReciptGSTIn,EInvStatus,IRN,ISNULL(QRCode,0) AS QRCodeEinvPrint,ISNULL(EwayBill,0) AS EwayBill  FROM STATE_MASTER,EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND INM_TYPE='TAXINV' AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and INM_NO BETWEEN '" + invoice_code + "' AND '" + toNo + "' AND INM_SUPPLEMENTORY=0 AND   INM_IS_SUPPLIMENT=0  AND P_SM_CODE=SM_CODE Order by INM_NO");
                }
            }
            DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER,STATE_MASTER where CM_STATE=SM_CODE AND CM_ID='" + Session["CompanyId"] + "' AND  CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");
            if (dtComp.Rows.Count > 0)
            {
                if (dtTaxInvoice.Rows.Count > 0)
                {
                    dtTaxInvoice.Columns.Add("QR_CODE", typeof(byte[]));
                    dtTaxInvoice.Columns.Add("QRCodeEinv", typeof(byte[])); 

                    foreach (DataRow row in dtTaxInvoice.Rows)
                    {
                        string strQR = "";
                        strQR = row["QR_CODEPrint"].ToString();
                        var writer = new BarcodeWriter();
                        writer.Format = BarcodeFormat.QR_CODE;
                        var result = writer.Write(strQR);
                        string path = Server.MapPath("~/UpLoadPath/QRImage.Jpeg");
                        var barcodeBitmap = new Bitmap(result);

                        byte[] bytes;
                        using (MemoryStream memory = new MemoryStream())
                        {
                            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                            {
                                // barcodeBitmap.GetPixel(25, 25);
                                barcodeBitmap.Save(memory, ImageFormat.Png);

                                bytes = memory.ToArray();
                                fs.Write(bytes, 0, bytes.Length);
                            }
                        }
                        row["QR_CODE"] = (byte[])bytes;





                        //------------E INV 


                        string strEQR = row["QRCodeEinvPrint"].ToString();
                        var Ewriter = new BarcodeWriter();
                        Ewriter.Format = BarcodeFormat.QR_CODE;
                        var Eresult = Ewriter.Write(strEQR);
                        string Epath = Server.MapPath("~/UpLoadPath/QR/QRCodeEinv.Jpeg");
                        var EbarcodeBitmap = new Bitmap(Eresult);

                        byte[] Ebytes;
                        using (MemoryStream Ememory = new MemoryStream())
                        {
                            using (FileStream efs = new FileStream(Epath, FileMode.Create, FileAccess.ReadWrite))
                            {
                                EbarcodeBitmap.Save(Ememory, System.Drawing.Imaging.ImageFormat.Png);

                                Ebytes = Ememory.ToArray();
                                efs.Write(Ebytes, 0, Ebytes.Length);
                            }
                        }
                        row["QRCodeEinv"] = (byte[])Ebytes; 
                    }
                    if (Cond == "1")
                    {
                        #region Cond_1_Printed_Material
                        // Create a DataRow, add Name and Age data, and add to the DataTable
                        rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice_1X.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice_1X.rpt");

                        rptname.Refresh();
                        //rptname.SetDataSource(dtfinal);
                        rptname.SetDataSource(dtTaxInvoice);
                        rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                        rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                        rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                        rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                        rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                        rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                        rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                        rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                        rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                        rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                        rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                        rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                        rptname.SetParameterValue("txtCINNo", dtComp.Rows[0]["SM_NAME"].ToString());
                        rptname.SetParameterValue("txtSAC", " ");
                        CrystalReportViewer1.ReportSource = rptname;

                        #endregion Printed_Material
                    }

                    #region Cond_2_Plain Print
                    if (Cond == "2")
                    {
                        DataTable dtCompGSTNo = CommonClasses.Execute("SELECT CM_GST_NO,SM_NAME FROM COMPANY_MASTER,STATE_MASTER where SM_CODE=CM_STATE AND  CM_ID=" + Session["CompanyId"].ToString() + "");
                        //txtCompGSTNo 

                        DataTable dtTaxInvoiceGST = new DataTable();
                        dtTaxInvoiceGST = dtTaxInvoice;

                        dtTemp.Columns.Add("INM_CODE", typeof(int));
                        dtTemp.Columns.Add("INM_NO", typeof(int));
                        dtTemp.Columns.Add("INM_NAME", typeof(string));
                        dtTemp.Columns.Add("INM_SEQNO", typeof(int));



                        if (chkPrint1 == "1")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                        }
                        if (chkPrint1 == "2")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);
                        }

                        if (chkPrint1 == "3")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";

                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate for Supplier";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);
                        }
                        if (chkPrint1 == "4")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate for Supplier";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Quaradruplicate";
                            dr["INM_SEQNO"] = 4;
                            dtTemp.Rows.Add(dr);
                        }
                        dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                        dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                        DataTable dta = new DataTable();
                        dta = (DataTable)dtTemp;
                        dsTaxInvoiceGST.Tables.Add(dta);
                        dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                        // Create a DataRow, add Name and Age data, and add to the DataTable
                        if (rptType.ToUpper() == "TRUE")
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceGSTX.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceGSTX.rpt");
                        }
                        else
                        {
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt");
                        }

                        rptname.Refresh();
                        //rptname.SetDataSource(dtfinal);
                        rptname.SetDataSource(dsTaxInvoiceGST);
                        rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                        rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                        rptname.SetParameterValue("txtCompGSTNo", dtCompGSTNo.Rows[0][0].ToString());
                        rptname.SetParameterValue("txtCompCity", dtCompGSTNo.Rows[0]["SM_NAME"].ToString());
                        CrystalReportViewer1.ReportSource = rptname;
                    }

                    #region MyRegion
                    /*
                        if (chkPrint1 == "2")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                            rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                            rptname.SetParameterValue("txtCompGSTNo", dtCompGSTNo.Rows[0][0].ToString());
                            rptname.SetParameterValue("txtCompCity", dtCompGSTNo.Rows[0]["SM_NAME"].ToString());
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                        if (chkPrint1 == "3")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";

                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate for Supplier";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                            rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                            rptname.SetParameterValue("txtCompGSTNo", dtCompGSTNo.Rows[0][0].ToString());
                            rptname.SetParameterValue("txtCompCity", dtCompGSTNo.Rows[0]["SM_NAME"].ToString());
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                        if (chkPrint1 == "4")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate for Supplier";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Quaradruplicate";
                            dr["INM_SEQNO"] = 4;
                            dtTemp.Rows.Add(dr);

                            dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                            dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                            DataTable dta = new DataTable();
                            dta = (DataTable)dtTemp;
                            dsTaxInvoiceGST.Tables.Add(dta);
                            dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                            // Create a DataRow, add Name and Age data, and add to the DataTable
                            rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                            rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceGST.rpt");

                            rptname.Refresh();
                            //rptname.SetDataSource(dtfinal);
                            rptname.SetDataSource(dsTaxInvoiceGST);
                            rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                            rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                            rptname.SetParameterValue("txtCompGSTNo", dtCompGSTNo.Rows[0][0].ToString());
                            rptname.SetParameterValue("txtCompCity", dtCompGSTNo.Rows[0]["SM_NAME"].ToString());
                            CrystalReportViewer1.ReportSource = rptname;
                        }
                    
                    }
                     *  * */

                    #endregion
                    #endregion Cond_2_Plain Print

                    if (Cond == "3")
                    {
                        #region Cond_1_Printed_Excise
                        rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice.rpt");
                        // Create a DataRow, add Name and Age data, and add to the DataTable
                        //rptname.Load(Server.MapPath("~/Reports/rptTaxInvoice_1.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                        //rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoice_1.rpt");

                        rptname.Refresh();
                        //rptname.SetDataSource(dtfinal);
                        rptname.SetDataSource(dtTaxInvoice);
                        rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                        rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                        rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                        rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                        rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                        rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                        rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                        rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                        rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                        rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                        rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                        rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                        rptname.SetParameterValue("txtCINNo", dtComp.Rows[0]["SM_NAME"].ToString());
                        rptname.SetParameterValue("txtSAC", " ");
                        CrystalReportViewer1.ReportSource = rptname;
                        #endregion Printed_Material
                    }

                    #region Cond_4_E Invoice
                    if (Cond == "4")
                    {
                        DataTable dtCompGSTNo = CommonClasses.Execute("SELECT CM_GST_NO,SM_NAME FROM COMPANY_MASTER,STATE_MASTER where SM_CODE=CM_STATE AND  CM_ID=" + Session["CompanyId"].ToString() + "");
                        //txtCompGSTNo 

                        DataTable dtTaxInvoiceGST = new DataTable();
                        dtTaxInvoiceGST = dtTaxInvoice;

                        dtTemp.Columns.Add("INM_CODE", typeof(int));
                        dtTemp.Columns.Add("INM_NO", typeof(int));
                        dtTemp.Columns.Add("INM_NAME", typeof(string));
                        dtTemp.Columns.Add("INM_SEQNO", typeof(int));



                        if (chkPrint1 == "1")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                        }
                        if (chkPrint1 == "2")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);
                        }

                        if (chkPrint1 == "3")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";

                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate for Supplier";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);
                        }
                        if (chkPrint1 == "4")
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original For Recipient";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate For Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Triplicate for Supplier";
                            dr["INM_SEQNO"] = 3;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["INM_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Quaradruplicate";
                            dr["INM_SEQNO"] = 4;
                            dtTemp.Rows.Add(dr);
                        }
                        dsTaxInvoiceGST.Tables.Add(dtTaxInvoiceGST);
                        dsTaxInvoiceGST.Tables[0].TableName = "dtTaxInvoiceGST";
                        DataTable dta = new DataTable();
                        dta = (DataTable)dtTemp;
                        dsTaxInvoiceGST.Tables.Add(dta);
                        dsTaxInvoiceGST.Tables[1].TableName = "NoOfCopyTaxInvoice";

                        // Create a DataRow, add Name and Age data, and add to the DataTable

                        rptname.Load(Server.MapPath("~/Reports/rptTaxEInvoice.rpt"));   //rptTaxInvoice  rptTaxInvoiceGST
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxEInvoice.rpt");
                       

                        rptname.Refresh();
                        //rptname.SetDataSource(dtfinal);
                        rptname.SetDataSource(dsTaxInvoiceGST);
                        rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                        rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                        rptname.SetParameterValue("txtCompGSTNo", dtCompGSTNo.Rows[0][0].ToString());
                        rptname.SetParameterValue("txtCompCity", dtCompGSTNo.Rows[0]["SM_NAME"].ToString());
                        CrystalReportViewer1.ReportSource = rptname;
                    }
 
                    #endregion Cond_2_Plain Print
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "GenerateReport", Ex.Message);
        }
    }

    private Color Color(int p, int p_2, int p_3, int p_4)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string title1 = Request.QueryString[0];
        if (title1 == "Labour Charge Invoice/Delivery Challan")
        {
            Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);
        }
        else
        {
            Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
        }
    }
    #endregion
}
