using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_TaxInvoiceRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

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
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Condition = Request.QueryString[5];
            string Type = Request.QueryString[6];

            GenerateReport(Title, From, To, group, way, Condition, Type);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string strCondition, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Suja: 20-03-2019 in where cond. add ISNULL(INM_INV_TYPE,0)!=1 and Union for New labour Charge invoice 
            //Old: Query = "SELECT DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,CASE when INM_IS_SUPPLIMENT=1 then 0 else cast(isnull(IND_INQTY,0) as numeric(20,3)) END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales'  WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,INM_TNO, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE,ISNULL(IND_AMORTAMT,0)  AS IND_AMORTAMT ,ISNULL(IND_E_TARIFF_NO,'') AS E_TARIFF_NO,ISNULL(IND_EX_AMT,0) + isnull(IND_TEX_AMT,0) AS  IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) + isnull(IND_TE_CESS_AMT,0) AS  IND_E_CESS_AMT,	ISNULL(IND_SH_CESS_AMT,0) + isnull(IND_TSH_CESS_AMT,0)  AS IND_SH_CESS_AMT ,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE,ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER ,ITEM_UNIT_MASTER  WHERE INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE AND I_CODE=IND_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND ISNULL(INM_INV_TYPE,0)!=1 AND INM_P_CODE=P_CODE " + strCondition + "     UNION SELECT DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,LIM_NO AS INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,  CASE when INM_IS_SUPPLIMENT =1 then 0 else  cast(isnull(IND_INQTY,0) as numeric(20,3)) END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales' WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,CONVERT(varchar(50),LIM_NO) AS INM_TNO, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT ,ISNULL(IND_E_TARIFF_NO,'') AS E_TARIFF_NO,ISNULL(IND_EX_AMT,0) + isnull(IND_TEX_AMT,0)AS  IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) + isnull(IND_TE_CESS_AMT,0) AS  IND_E_CESS_AMT,	ISNULL(IND_SH_CESS_AMT,0) + isnull(IND_TSH_CESS_AMT,0)AS IND_SH_CESS_AMT ,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE,ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,LABOR_INVOICE_MASTER,LABOR_INVOICE_DETAIL WHERE INM_P_CODE=LIM_P_CODE and LIM_CODE=LID_LIM_CODE AND INM_CODE=LID_INM_CODE AND INM_CODE=IND_INM_CODE AND INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE AND I_CODE=IND_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND ISNULL(INM_INV_TYPE,0)=0  AND INM_P_CODE=P_CODE " + strCondition + "";
            //Change Flag ISNULL(INM_INV_TYPE,0)=0 To ISNULL(INM_INV_TYPE,0)=1 IN second query After Union
            Query = "SELECT DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,P_CODE,P_NAME,  P_LBT_NO, P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,CASE when INM_IS_SUPPLIMENT=1 then 0 else cast(isnull(IND_INQTY,0) as numeric(20,3)) END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales'  WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,INM_TNO, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE,ISNULL(IND_AMORTAMT,0)  AS IND_AMORTAMT ,ISNULL(IND_E_TARIFF_NO,'') AS E_TARIFF_NO,ISNULL(IND_EX_AMT,0) + isnull(IND_TEX_AMT,0) AS  IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) + isnull(IND_TE_CESS_AMT,0) AS  IND_E_CESS_AMT,	ISNULL(IND_SH_CESS_AMT,0) + isnull(IND_TSH_CESS_AMT,0)  AS IND_SH_CESS_AMT ,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE,ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER ,ITEM_UNIT_MASTER  WHERE INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE AND I_CODE=IND_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND ISNULL(INM_INV_TYPE,0)!=1 AND INM_P_CODE=P_CODE " + strCondition + "     UNION SELECT DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,P_CODE,P_NAME, P_LBT_NO,P_ADD1,P_CST,P_VAT,P_ECC_NO,LIM_NO AS INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,  CASE when INM_IS_SUPPLIMENT =1 then 0 else  cast(isnull(IND_INQTY,0) as numeric(20,3)) END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales' WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,CONVERT(varchar(50),LIM_NO) AS INM_TNO, ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE,ISNULL(IND_AMORTAMT,0) AS IND_AMORTAMT ,ISNULL(IND_E_TARIFF_NO,'') AS E_TARIFF_NO,ISNULL(IND_EX_AMT,0) + isnull(IND_TEX_AMT,0)AS  IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) + isnull(IND_TE_CESS_AMT,0) AS  IND_E_CESS_AMT,	ISNULL(IND_SH_CESS_AMT,0) + isnull(IND_TSH_CESS_AMT,0)AS IND_SH_CESS_AMT ,ISNULL(IND_TRASPORT_RATE,0) AS IND_TRASPORT_RATE,ISNULL(IND_TRASPORT_AMT,0) AS IND_TRASPORT_AMT FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,LABOR_INVOICE_MASTER,LABOR_INVOICE_DETAIL WHERE INM_P_CODE=LIM_P_CODE and LIM_CODE=LID_LIM_CODE AND INM_CODE=LID_INM_CODE  AND INM_CODE=IND_INM_CODE AND INM_CODE=IND_INM_CODE  AND LID_I_CODE=IND_I_CODE  AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE AND I_CODE=IND_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE AND  LABOR_INVOICE_MASTER.ES_DELETE=0 AND   LIM_CM_CODE='" + Session["CompanyCode"] + "'  " + strCondition + "";

            // Query = "SELECT  DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,  CASE when INM_IS_SUPPLIMENT =1 then 0 else cast(isnull(IND_INQTY,0) as numeric(20,3)) END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales'  WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,INM_TNO, ISNULL(IND_AMORTRATE,0)  AS IND_AMORTRATE,ISNULL(IND_AMORTAMT,0)  AS IND_AMORTAMT ,ISNULL(IND_E_TARIFF_NO,'') AS IND_E_TARIFF_NO ,ISNULL(E_BASIC,0) AS    E_BASIC_CentralT,ISNULL(E_EDU_CESS,0) AS E_EDU_CESS_State, ISNULL(E_H_EDU,0) AS  E_H_EDU_Integrated,ISNULL(E_TARIFF_NO,0) AS  IND_HSN_CODE,ISNULL(IND_EX_AMT,0) AS  IND_EX_AMT,ISNULL(IND_E_CESS_AMT,0) AS  IND_E_CESS_AMT,	ISNULL(IND_SH_CESS_AMT,0) AS   IND_SH_CESS_AMT FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER ,ITEM_UNIT_MASTER  WHERE INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE AND I_CODE=IND_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INVOICE_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE  " + strCondition + "";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            Query = Query + " GROUP BY INM_LR_DATE,I_CODENO,I_CODE,INM_NO,P_CODE,P_NAME, P_LBT_NO,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,INM_VEH_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_IS_SUPPLIMENT,INM_TNO,I_UOM_NAME,INM_TYPE,IND_AMORTRATE,IND_AMORTAMT ,IND_E_TARIFF_NO ,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT  ,IND_TRASPORT_RATE,IND_TRASPORT_AMT ,isnull(IND_TEX_AMT,0),isnull(IND_TE_CESS_AMT,0),isnull(IND_TSH_CESS_AMT,0),LIM_NO ORDER BY INM_TNO";
            //Query = Query + "  GROUP BY INM_LR_DATE,I_CODENO,I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,INM_VEH_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_IS_SUPPLIMENT,INM_TNO,I_UOM_NAME,INM_TYPE,IND_AMORTRATE,IND_AMORTAMT ,IND_E_TARIFF_NO ,E_BASIC,E_EDU_CESS,E_H_EDU,E_TARIFF_NO,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT   ORDER BY INM_TNO";

            dt = CommonClasses.Execute(Query);

            if (dt.Rows.Count > 0)
            {
                if (Type == "SHOW")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    string report = "";
                    if (group == "Datewise")
                    {
                        report = "TaxInvRegDatewise";
                    }
                    if (group == "ItemWise")
                    {
                        report = "TaxInvRegItemwise";
                    }
                    if (group == "CustWise")
                    {
                        report = "TaxInvRegCustwise";
                    }
                    if (group == "HSNWise")
                    {
                        report = "TaxInvRegHSNwise";
                    }
                    rptname.Load(Server.MapPath("~/Reports/" + report + ".rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/" + report + ".rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", Convert.ToDateTime(From).ToString("dd-MMM-yyyy") + " - " + Convert.ToDateTime(To).ToString("dd-MMM-yyyy"));

                    CrystalReportViewer1.ReportSource = rptname;

                    #region old code
                    /*
                    #region Datewise
                    if (group == "Datewise")
                    {
                        //rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceDatewiseRegPCPL.rpt"));
                        //rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceDatewiseRegPCPL.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        if (way == "Summary")
                        {
                            rptname.SetParameterValue("txtType", "1");
                        }
                        else
                        {
                            rptname.SetParameterValue("txtType", "0");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "Datewise " + Title + " From " + From + " To " + To);
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    #endregion Datewise

                    #region ItemWise
                    else if (group == "ItemWise")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceItemwiseRegPCPL.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceItemwiseRegPCPL.rpt");

                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        if (way == "Summary")
                        {
                            rptname.SetParameterValue("txtType", "1");
                        }
                        else
                        {
                            rptname.SetParameterValue("txtType", "0");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "Itemwise " + Title + " From " + From + " To " + To);
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    #endregion ItemWise

                    #region CustWise
                    else if (group == "CustWise")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceCustwiseRegPCPL.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceCustwiseRegPCPL.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        if (way == "Summary")
                        {
                            rptname.SetParameterValue("txtType", "1");
                        }
                        else
                        {
                            rptname.SetParameterValue("txtType", "0");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "Customer wise " + Title + " From " + From + " To " + To);
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    #endregion CustWise

                    #region HSNWise
                    else if (group == "HSNWise")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptTaxInvoiceHSNNoPCPL.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptTaxInvoiceHSNNoPCPL.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        if (way == "Summary")
                        {
                            rptname.SetParameterValue("txtType", "1");
                        }
                        else
                        {
                            rptname.SetParameterValue("txtType", "0");
                        }
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "HSN Number wise " + Title + " From " + From + " To " + To);
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    #endregion HSNWise
                     * */

                    #endregion
                }
                else
                {
                    #region Export_Excel
                    DataTable dtResult = new DataTable();
                    dtResult = dt;
                    DataTable dtExport = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtExport.Columns.Add("Inv Date");
                        dtExport.Columns.Add("Inv No.");
                        dtExport.Columns.Add("Party Name");
                        dtExport.Columns.Add("GST No");
                        dtExport.Columns.Add("Item Name");
                        dtExport.Columns.Add("Item Code");
                        dtExport.Columns.Add("HSN No.");
                        dtExport.Columns.Add("Qty");
                        dtExport.Columns.Add("Rate");
                        dtExport.Columns.Add("Basic Amt");
                        dtExport.Columns.Add("Transport Amt");
                        dtExport.Columns.Add("CGST");
                        dtExport.Columns.Add("SGST");
                        dtExport.Columns.Add("IGST");
                        dtExport.Columns.Add("Tax Amt");
                        dtExport.Columns.Add("Total Amt");

                        for (int i = 0; i < dtResult.Rows.Count; i++)
                        {
                            dtExport.Rows.Add(Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                              dtResult.Rows[i]["INM_TNO"].ToString(),
                                              dtResult.Rows[i]["P_NAME"].ToString(),
                                               dtResult.Rows[i]["P_LBT_NO"].ToString(),
                                              
                                              dtResult.Rows[i]["I_NAME"].ToString(),
                                              dtResult.Rows[i]["I_CODENO"].ToString(),
                                              dtResult.Rows[i]["E_TARIFF_NO"].ToString(),
                                              dtResult.Rows[i]["IND_INQTY"].ToString(),
                                              dtResult.Rows[i]["IND_RATE"].ToString(),
                                              dtResult.Rows[i]["IND_AMT"].ToString(),
                                               dtResult.Rows[i]["IND_TRASPORT_AMT"].ToString(),
                                              dtResult.Rows[i]["IND_EX_AMT"].ToString(),
                                              dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
                                              dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
                                              dtResult.Rows[i]["INM_S_TAX_AMT"].ToString(),
                                               dtResult.Rows[i]["INM_G_AMT"].ToString()
                                             );
                        }
                    }

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/ms-excel";
                    HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=TaxInvoiceSummary.xls");
                    HttpContext.Current.Response.Charset = "utf-8";
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                    //sets font
                    HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                    HttpContext.Current.Response.Write("<BR><BR><BR>");
                    HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                    "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                    "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                    //am getting my grid's column headers
                    int columnscount = dtExport.Columns.Count;
                    for (int j = 0; j < columnscount; j++)
                    {      //write in new column
                        HttpContext.Current.Response.Write("<Td>");
                        //Get column headers  and make it as bold in excel columns
                        HttpContext.Current.Response.Write("<B>");
                        HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                        HttpContext.Current.Response.Write("</B>");
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    HttpContext.Current.Response.Write("</TR>");
                    for (int k = 0; k < dtExport.Rows.Count; k++)
                    {//write in new row
                        HttpContext.Current.Response.Write("<TR>");
                        for (int i = 0; i < dtExport.Columns.Count; i++)
                        {
                            if (i == dtExport.Columns.Count - 1)
                            {
                                HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                                HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                HttpContext.Current.Response.Write("</Td>");
                            }
                            else
                            {
                                if (i == 5)
                                {
                                    if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                                    {
                                        HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                                    }
                                    else
                                    {
                                        HttpContext.Current.Response.Write("<Td>");
                                    }
                                }
                                else
                                {
                                    HttpContext.Current.Response.Write("<Td>");
                                }
                                HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                HttpContext.Current.Response.Write("</Td>");
                            }
                        }
                        HttpContext.Current.Response.Write("</TR>");
                    }
                    HttpContext.Current.Response.Write("</Table>");
                    HttpContext.Current.Response.Write("</font>");
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                    #endregion Export_Excel
                }
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
            throw Ex;
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
            CommonClasses.SendError("Tax Invoice Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewTaxInvoiceRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
