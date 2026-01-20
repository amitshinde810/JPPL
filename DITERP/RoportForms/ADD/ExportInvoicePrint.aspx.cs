using System;
using System.Collections;
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

public partial class RoportForms_ADD_ExportInvoicePrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string printtype = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        invoice_code = Request.QueryString[0];
        printtype = Request.QueryString[1];

        GenerateReport(invoice_code, printtype);
    }

    #region GenerateReport
    private void GenerateReport(string code, string printtype)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        try
            {
        if (printtype == "ExpInv")
        {
            #region Export Invoice Print
            DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),ITEM_UNIT_MASTER.I_UOM_NAME as I_UOM_NAME,P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_TAX_NAME,ST_SALES_TAX,INM_BOND_NO,INM_BOND_DATE,INM_AREA_FORM_NO,INM_FORM_DATE,P_PHONE,INM_PRE_CARRIAGE,INM_PORT_OF_DISCH,INM_PORT_OF_LOAD,INM_PLACE_OF_DEL,IND_CONTAINER_NO,IND_NET_WEIGHT,IND_GROSS_WEIGHT,IND_NO_OF_BARRELS,I_CODENO,INM_FREIGHT,INM_INSURANCE,INM_FINAL_DEST,INM_TOD,INM_TOP,INM_VOY_NO,INM_PLACE_REC,INM_M_NO,INM_NOS_PACK,INM_UN_NO,INM_HAZ,INM_HS_CODENO,INM_CONTA_NO,INM_SEAL_NO,INM_OTS_NO,INM_CURR_RATE,Counrty2.COUNTRY_NAME as INM_CONTRY_ORIGIN,Counrty1.COUNTRY_NAME as INM_COUNTRY_DEST,INM_LC_NO,INM_LC_DATE,INM_UN_PAK_GRP,INM_UN_PAK_CODE,INM_UT1_FILE_NO,INM_FILE_NO,INM_PER_E_NO FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,COUNTRY_MASTER Counrty1,COUNTRY_MASTER Counrty2,ITEM_UNIT_MASTER  WHERE ITEM_UNIT_MASTER.I_UOM_CODE=IND_UOM_CODE and INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE  and INM_COUNTRY_DEST= Counrty1.COUNTRY_CODE and INM_CONTRY_ORIGIN=Counrty2.COUNTRY_CODE and INM_CODE='" + code + "' ");

            if (dtfinal.Rows.Count > 0)
            {
           
                DataTable dtLinceDet = CommonClasses.Execute("select CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO from COMPANY_MASTER where CM_ID='"+Session["CompanyID"]+"'");

                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptExportInvoice.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptExportInvoice.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd1"].ToString());
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                rptname.SetParameterValue("txtCompFax", Session["Companyfax"].ToString());
                rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                rptname.SetParameterValue("txtCompEXPLicen", dtLinceDet.Rows[0]["CM_EXP_LICEN_NO"]);
                rptname.SetParameterValue("txtCompEXPPermission", dtLinceDet.Rows[0]["CM_EXP_PERMISSOIN_NO"]);
                string COUNTRY_NAME = DL_DBAccess.GetColumn("select COUNTRY_NAME from INVOICE_MASTER,COUNTRY_MASTER where INM_COUNTRY_DEST=COUNTRY_CODE and INM_CODE='" + code + "'");
                int InvType = 0;
                if (COUNTRY_NAME == "RUSSIA")
                {
                    InvType = 1;
                }
                else
                {
                    InvType = 0;
                }
                rptname.SetParameterValue("txtInvType", InvType);  
                CrystalReportViewer1.ReportSource = rptname;
                 }
            

            
            #endregion          

         }
        else if (printtype == "PackList")
        {
            #region Export Packing List
            DataTable dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),ITEM_UNIT_MASTER.I_UOM_NAME as I_UOM_NAME,P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_TAX_NAME,ST_SALES_TAX,INM_BOND_NO,INM_BOND_DATE,INM_AREA_FORM_NO,INM_FORM_DATE,P_PHONE,INM_PRE_CARRIAGE,INM_PORT_OF_DISCH,INM_PORT_OF_LOAD,INM_PLACE_OF_DEL,IND_CONTAINER_NO,IND_NET_WEIGHT,IND_GROSS_WEIGHT,IND_NO_OF_BARRELS,I_CODENO,INM_FREIGHT,INM_INSURANCE,INM_FINAL_DEST,INM_TOD,INM_TOP,INM_VOY_NO,INM_PLACE_REC,INM_M_NO,INM_NOS_PACK,INM_UN_NO,INM_HAZ,INM_HS_CODENO,INM_CONTA_NO,INM_SEAL_NO,INM_OTS_NO,INM_CURR_RATE,Counrty2.COUNTRY_NAME as INM_CONTRY_ORIGIN,Counrty1.COUNTRY_NAME as INM_COUNTRY_DEST,INM_LC_NO,INM_LC_DATE,INM_UN_PAK_GRP,INM_UN_PAK_CODE,INM_UT1_FILE_NO,INM_FILE_NO,INM_PER_E_NO FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER,COUNTRY_MASTER Counrty1,COUNTRY_MASTER Counrty2,ITEM_UNIT_MASTER  WHERE ITEM_UNIT_MASTER.I_UOM_CODE=IND_UOM_CODE and INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE  and INM_COUNTRY_DEST= Counrty1.COUNTRY_CODE and INM_CONTRY_ORIGIN=Counrty2.COUNTRY_CODE and INM_CODE='" + code + "'  group by I_CODE,I_UOM_NAME,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO ,INM_DATE,INM_TRANSPORT,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME ,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE ,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,INM_VEH_NO,ST_TAX_NAME,ST_SALES_TAX ,E_TARIFF_NO,INM_BOND_NO,INM_BOND_DATE,INM_AREA_FORM_NO,INM_FORM_DATE,P_PHONE,INM_PRE_CARRIAGE,INM_PORT_OF_DISCH,INM_PORT_OF_LOAD,INM_PLACE_OF_DEL,IND_CONTAINER_NO,IND_NET_WEIGHT,IND_GROSS_WEIGHT,IND_NO_OF_BARRELS,I_CODENO,INM_FREIGHT,INM_INSURANCE,INM_FINAL_DEST,INM_TOD,INM_TOP,INM_VOY_NO,INM_PLACE_REC,INM_M_NO,INM_NOS_PACK,INM_UN_NO,INM_HAZ,INM_HS_CODENO,INM_CONTA_NO,INM_SEAL_NO,INM_OTS_NO,INM_CURR_RATE,Counrty2.COUNTRY_NAME,Counrty1.COUNTRY_NAME,INM_LC_NO,INM_LC_DATE,INM_UN_PAK_GRP,INM_UN_PAK_CODE,INM_UT1_FILE_NO,INM_FILE_NO,INM_PER_E_NO");

            if (dtfinal.Rows.Count > 0)
            {
                     
                DataTable dtLinceDet = CommonClasses.Execute("select CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO from COMPANY_MASTER where CM_ID='" + Session["CompanyID"] + "'");

                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptExportInvoicePackingList.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptExportInvoicePackingList.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd1"].ToString());
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                rptname.SetParameterValue("txtCompFax", Session["Companyfax"].ToString());
                rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                rptname.SetParameterValue("txtCompEXPLicen", dtLinceDet.Rows[0]["CM_EXP_LICEN_NO"]);
                rptname.SetParameterValue("txtCompEXPPermission", dtLinceDet.Rows[0]["CM_EXP_PERMISSOIN_NO"]);
                string COUNTRY_NAME = DL_DBAccess.GetColumn("select COUNTRY_NAME from INVOICE_MASTER,COUNTRY_MASTER where INM_COUNTRY_DEST=COUNTRY_CODE and INM_CODE='" + code + "'");
                int InvType = 0;
                if (COUNTRY_NAME == "RUSSIA")
                {
                    InvType = 1;
                }
                else
                {
                    InvType = 0;
                }
                rptname.SetParameterValue("txtInvType", InvType);  
                CrystalReportViewer1.ReportSource = rptname;


            }
           
            #endregion          
        }
        else if (printtype == "ExpInvDom")
        {
            #region Export Invoice Domestic Print
            DataTable dtfinal1 = CommonClasses.Execute("SELECT DISTINCT(I_CODE),INM_CODE,P_CODE,E_TARIFF_NO,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT_BY as INM_TRANSPORT,INM_VEH_NO,INM_LR_NO,INM_LR_DATE,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,INM_BOND_NO,INM_BOND_DATE,INM_AREA_FORM_NO,INM_FORM_DATE,P_PHONE,cast(isnull(IND_NO_PACK,0) as numeric(10,0)) as IND_PAK_QTY,cast(isnull(IND_INQTY,0) as numeric(10,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(10,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(10,2)) as INM_AMT,cast(isnull(INM_NET_AMT,0) as numeric(10,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(10,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(10,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(10,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(10,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(10,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(10,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(10,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(10,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(10,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(10,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(10,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(10,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(10,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(10,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(10,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(10,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(10,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(10,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(10,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(10,2)) as INM_G_AMT,cast(isnull(INM_ADV_DUTY,0) as numeric(20,2)) as INM_ADV_DUTY,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,ST_TAX_NAME ,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_ISSU_TIME,INM_REMOVEL_TIME,P_ECC_NO,INM_LC_NO,INM_LC_DATE,INM_UN_PAK_GRP,INM_UN_PAK_CODE,INM_UT1_FILE_NO,INM_FILE_NO,INM_PER_E_NO,IND_PACK_DESC  FROM EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,SALES_TAX_MASTER,PARTY_MASTER WHERE INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and I_E_CODE=E_CODE and IND_I_CODE=CPOD_I_CODE  AND CPOD_ST_CODE=ST_CODE and INM_CODE='" + code + "'");

            if (dtfinal1.Rows.Count > 0)
            {
           
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptExpDomInvoice.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptExpDomInvoice.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal1);
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
                string CINNo = DL_DBAccess.GetColumn("select CM_CIN_NO from COMPANY_MASTER where CM_ID='" + Session["CompanyId"] + "'");
                rptname.SetParameterValue("txtCINNo", CINNo);
                string COUNTRY_NAME = DL_DBAccess.GetColumn("select COUNTRY_NAME from INVOICE_MASTER,COUNTRY_MASTER where INM_COUNTRY_DEST=COUNTRY_CODE and INM_CODE='" + code + "'");
                int InvType = 0;
                if (COUNTRY_NAME == "RUSSIA")
                {
                    InvType = 1;
                }
                else
                {
                    InvType = 0;
                }
                rptname.SetParameterValue("txtInvType", InvType);  
                CrystalReportViewer1.ReportSource = rptname;


            }
           
            #endregion

        }
        else if (printtype == "EximinationAdditionalReport")
        {
            #region Eximination Additional Report
            DataTable dtfinal1 = CommonClasses.Execute("SELECT INM_CARRIER_NAME, INM_CONTA_NO, CM_ADDRESS1,CM_ADDRESS2, CM_FAXNO, CM_PHONENO1,CM_PHONENO2, CM_EMAILID, CM_WEBSITE FROM INVOICE_MASTER,COMPANY_MASTER where INVOICE_MASTER.INM_CM_CODE = COMPANY_MASTER.CM_CODE and INM_CODE='" + code + "'");

            if (dtfinal1.Rows.Count > 0)
            {
         
           
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptEximinationAdditionalReport.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptEximinationAdditionalReport.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal1);
                //rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                //rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                //rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                //rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                //rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                //rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                rptname.SetParameterValue("txtCompIso", "("+Session["CompanyIso"].ToString()+")");
                //rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                //rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                //rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                //rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                CrystalReportViewer1.ReportSource = rptname;


            }
           
            #endregion

        }
        else if (printtype == "DangerousGoodsDeclaration")
        {
            #region Export Invoice Domestic Print
            DataTable dtfinal1 = CommonClasses.Execute("SELECT INM_SHIPMENT,P_MOB, P_CONTACT, INM_EXA_BOXES, P_NAME, P_CODE, INM_CODE, INM_NO, P_ADD1, P_PHONE, INM_DATE, INM_PORT_OF_DISCH, INM_PORT_OF_LOAD, INM_PLACE_OF_DEL, sum(IND_GROSS_WEIGHT) as IND_GROSS_WEIGHT, sum(IND_NET_WEIGHT) as IND_NET_WEIGHT, sum(IND_NO_OF_BARRELS) as IND_NO_OF_BARRELS, INM_FINAL_DEST, INM_UN_NO, INM_VOY_NO, INM_NOS_PACK, INM_HAZ, INM_CONTA_NO, INM_CARRIER_NAME, INM_CARRIER_BOOK_NO, INM_SHIP_NAME, INM_TECH_NAME, INM_OUT_PAKGS, INM_INR_PAKG, INM_SUB_CLASS, INM_SHIP_DECLAR, INM_MARINE_POLLT, INM_FLASH_POINT, INM_EMS_NO, INM_UN_PAK_CODE, INM_UN_PAK_GRP FROM INVOICE_DETAIL,INVOICE_MASTER,PARTY_MASTER WHERE IND_INM_CODE = INM_CODE AND INM_P_CODE = P_CODE and inm_code=" + code + " group by P_NAME, P_CODE, INM_CODE, INM_NO, P_ADD1, P_PHONE, INM_DATE, INM_PORT_OF_DISCH, INM_PORT_OF_LOAD, INM_PLACE_OF_DEL, INM_FINAL_DEST, INM_UN_NO, INM_VOY_NO, INM_NOS_PACK, INM_HAZ, INM_CONTA_NO, INM_CARRIER_NAME, INM_CARRIER_BOOK_NO, INM_SHIP_NAME, INM_TECH_NAME, INM_OUT_PAKGS, INM_INR_PAKG, INM_SUB_CLASS, INM_SHIP_DECLAR, INM_MARINE_POLLT, INM_FLASH_POINT, INM_EMS_NO, INM_UN_PAK_CODE, INM_UN_PAK_GRP,INM_EXA_BOXES,P_CONTACT,P_MOB,INM_SHIPMENT ");

            if (dtfinal1.Rows.Count > 0)
            {
          
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptDangerousGoodsDeclaration.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptDangerousGoodsDeclaration.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal1);
                rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd"].ToString());
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                string Boxes = dtfinal1.Rows[0]["INM_EXA_BOXES"].ToString();
                rptname.SetParameterValue("txtBoxes",Boxes);
                string P_Mob = dtfinal1.Rows[0]["P_MOB"].ToString();
                rptname.SetParameterValue("txtP_Mob", P_Mob);
                string P_CONTACT = dtfinal1.Rows[0]["P_CONTACT"].ToString();
                rptname.SetParameterValue("txtP_CONTACT", P_CONTACT);
                string INM_SHIPMENT = dtfinal1.Rows[0]["INM_SHIPMENT"].ToString();
                rptname.SetParameterValue("txtINM_SHIPMENT", INM_SHIPMENT);


                //rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                //rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                //rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                //rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                //rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                //rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                //rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                //rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                CrystalReportViewer1.ReportSource = rptname;


            }
          
            #endregion
        
            }

        else if (printtype == "FormSDF")
        {
            #region Export Invoice Domestic Print
            DataTable dtfinal1 = CommonClasses.Execute("SELECT INM_NO, INM_DATE,INM_G_AMT,CM_BANK_NAME+' '+CM_BANK_ADDRESS as BankDetails from INVOICE_MASTER,COMPANY_MASTER WHERE CM_CODE=INM_CM_CODE and INM_CODE =" + code);

            if (dtfinal1.Rows.Count > 0)
            {
         
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptAppendixIFormSDF.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptAppendixIFormSDF.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal1);
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd"].ToString());
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                //rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                //rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                //rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                //rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                //rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                //rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                //rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                //rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                CrystalReportViewer1.ReportSource = rptname;


            }
           
            #endregion

        }

        else if (printtype == "AreInv")
        {
            #region Export ARE Form 1
            //DataTable dtfinal1 = CommonClasses.Execute("SELECT CM_NAME,CM_ADDRESS1,CM_ADDRESS2,CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO,CM_EXCISE_DIVISION, CM_EXCISE_RANGE,CM_COMMISONERATE,CM_EXC_SUPRE_DETAILS,INM_NO,INM_CODE,INM_DATE,INM_COUNTRY_DEST, INM_UT1_FILE_NO,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,COUNTRY_NAME as COUNTORY_NAME FROM COMPANY_MASTER INNER JOIN INVOICE_MASTER ON COMPANY_MASTER.CM_CODE = INVOICE_MASTER.INM_CM_CODE LEFT OUTER JOIN  COUNTRY_MASTER ON INVOICE_MASTER.INM_COUNTRY_DEST = COUNTRY_MASTER.COUNTRY_CODE WHERE INM_CODE='" + code + "'");
            DataTable dtfinal1 = CommonClasses.Execute("SELECT CM_NAME,CM_ADDRESS1,CM_ADDRESS2+' '+CM_REGD_NO as CM_ADDRESS2,CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO,CM_EXCISE_DIVISION,CM_EXCISE_RANGE,CM_COMMISONERATE,CM_EXC_SUPRE_DETAILS,INM_NO,INM_CODE,INM_DATE,INM_COUNTRY_DEST,INM_UT1_FILE_NO,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,COUNTRY_NAME AS COUNTORY_NAME,SUM(IND_INQTY) AS IND_INQTY,SUM(IND_INQTY * IND_RATE) AS AMT,SUM(IND_EX_AMT) AS IND_EX_AMT,SUM(IND_E_CESS_AMT) AS IND_E_CESS_AMT,SUM(IND_SH_CESS_AMT) AS IND_SH_CESS_AMT,sum(IND_GROSS_WEIGHT) as IND_GROSS_WEIGHT,sum(IND_NET_WEIGHT) as IND_NET_WEIGHT,IND_CPOM_CODE,E_BASIC,E_EDU_CESS,E_H_EDU,INM_TRANSPORT_BY,INM_ARE_REMARK,INM_REMOVAL_DATE,INM_REMOVEL_TIME  FROM COMPANY_MASTER INNER JOIN INVOICE_MASTER ON COMPANY_MASTER.CM_CODE = INVOICE_MASTER.INM_CM_CODE INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE LEFT OUTER JOIN COUNTRY_MASTER ON INVOICE_MASTER.INM_COUNTRY_DEST = COUNTRY_MASTER.COUNTRY_CODE WHERE (INVOICE_MASTER.INM_CODE = '" + code + "') GROUP BY CM_NAME,CM_ADDRESS1,CM_ADDRESS2,CM_REGD_NO,CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO,CM_EXCISE_DIVISION,CM_EXCISE_RANGE,CM_COMMISONERATE,CM_EXC_SUPRE_DETAILS,INM_NO,INM_CODE,INM_DATE, INM_COUNTRY_DEST,INM_UT1_FILE_NO,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,COUNTRY_NAME,IND_CPOM_CODE,E_BASIC,E_EDU_CESS,E_H_EDU,INM_TRANSPORT_BY,INM_ARE_REMARK,INM_REMOVAL_DATE,INM_REMOVEL_TIME");

            //DataTable dtfinal1 = CommonClasses.Execute("SELECT CM_NAME,CM_ADDRESS1,CM_ADDRESS2,CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO,CM_EXCISE_RANGE,CM_EXCISE_DIVISION,CM_COMMISONERATE,CM_EXC_SUPRE_DETAILS,INM_NO,INM_CODE,INM_DATE,INM_COUNTRY_DEST,INM_UT1_FILE_NO,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,COUNTRY_NAME,I_NAME,IND_INQTY,IND_RATE,IND_INQTY*IND_RATE as AMT,IND_PACK_DESC FROM INVOICE_DETAIL INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN COMPANY_MASTER ON INVOICE_MASTER.INM_CM_CODE = COMPANY_MASTER.CM_CODE LEFT OUTER JOIN COUNTRY_MASTER ON INVOICE_MASTER.INM_COUNTRY_DEST = COUNTRY_MASTER.COUNTRY_CODE where INM_CODE='" + code + "' group by CM_NAME,CM_ADDRESS1,CM_ADDRESS2,CM_EXP_LICEN_NO,CM_EXP_PERMISSOIN_NO,CM_EXCISE_RANGE,CM_EXCISE_DIVISION,CM_COMMISONERATE,CM_EXC_SUPRE_DETAILS,INM_NO,INM_CODE,INM_DATE,INM_COUNTRY_DEST,INM_UT1_FILE_NO,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,COUNTRY_NAME,I_NAME,IND_INQTY,IND_RATE,IND_PACK_DESC");

            DataTable dtAre1Det = CommonClasses.Execute("SELECT I_NAME,cast(IND_INQTY as numeric(20,3)) as IND_INQTY,Cast(IND_RATE as numeric(20,2)) AS IND_RATE, cast(IND_INQTY*IND_RATE as numeric(20,2)) AS AMT,IND_PACK_DESC as IND_PACK_DESC,INM_CODE FROM INVOICE_DETAIL INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE AND INM_CODE='" + code + "'");

            DataTable dtExpInvoiceDet = new DataTable();

            if (dtExpInvoiceDet.Columns.Count == 0)
            {
                dtExpInvoiceDet.Columns.Add("I_NAME");
                dtExpInvoiceDet.Columns.Add("IND_INQTY");
                dtExpInvoiceDet.Columns.Add("IND_RATE");
                dtExpInvoiceDet.Columns.Add("AMT");
                dtExpInvoiceDet.Columns.Add("IND_PACK_DESC");
                dtExpInvoiceDet.Columns.Add("INM_CODE");

            }

            for (int n = 0; n < dtAre1Det.Rows.Count; n++)
            {
                dtExpInvoiceDet.Rows.Add(dtAre1Det.Rows[n]["I_NAME"].ToString(), dtAre1Det.Rows[n]["IND_INQTY"].ToString(), dtAre1Det.Rows[n]["IND_RATE"].ToString(), dtAre1Det.Rows[n]["AMT"].ToString(), dtAre1Det.Rows[n]["IND_PACK_DESC"].ToString(), dtAre1Det.Rows[n]["INM_CODE"].ToString());
            }

            if (dtfinal1.Rows.Count > 0)
            {
           
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                //string path = Server.MapPath("~/Reports/rptExportInvoice.rpt");

                rptname.Load(Server.MapPath("~/Reports/rptARE1Invoice.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptARE1Invoice.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal1);
                rptname.OpenSubreport("rptAreDet").SetDataSource(dtExpInvoiceDet);

                //rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                //rptname.SetParameterValue("txtCompFaxNo", Session["Companyfax"].ToString());
                //rptname.SetParameterValue("txtCompVatTin", Session["CompanyVatTin"].ToString());
                //rptname.SetParameterValue("txtCompCstTin", Session["CompanyCstTin"].ToString());
                //rptname.SetParameterValue("txtCompEcc", Session["CompanyEccNo"].ToString());
                //rptname.SetParameterValue("txtCompVatWef", Convert.ToDateTime(Session["CompanyVatWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompCstWef", Convert.ToDateTime(Session["CompanyCstWef"].ToString()).ToShortDateString());
                //rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                //rptname.SetParameterValue("txtCompRegd", Session["CompanyRegd"].ToString());
                //rptname.SetParameterValue("txtCompWebsite", Session["CompanyWebsite"].ToString());
                //rptname.SetParameterValue("txtCompEmail", Session["CompanyEmail"].ToString());
                //rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
                CrystalReportViewer1.ReportSource = rptname;


            }
          
            #endregion

        }
        else if (printtype == "ExpInvIndA")
        {
            #region ExpInvIndA
            DataTable dtfinal1 = CommonClasses.Execute("SELECT  COMPANY_MASTER.CM_CODE, COMPANY_MASTER.CM_NAME, COMPANY_MASTER.CM_ADDRESS1, COMPANY_MASTER.CM_ADDRESS2, COMPANY_MASTER.CM_PHONENO1, COMPANY_MASTER.CM_FAXNO, COMPANY_MASTER.CM_EMAILID, COMPANY_MASTER.CM_WEBSITE, COMPANY_MASTER.CM_ISO_NUMBER, INVOICE_MASTER.INM_CODE, INVOICE_MASTER.INM_NO,  INVOICE_MASTER.INM_DATE, INVOICE_MASTER.INM_TOD, INVOICE_MASTER.INM_TOP, INVOICE_MASTER.INM_AUTHO_SIGN FROM  COMPANY_MASTER CROSS JOIN INVOICE_MASTER where INM_CODE=" + code + "");

            if (dtfinal1.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();


                rptname.Load(Server.MapPath("~/Reports/rptExpInvIndA.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptExpInvIndA.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal1);

                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
            }


            #endregion
        }
        else if (printtype == "ARE1Declaration")
        {
            #region ARE1Declaration
            DataTable dtfinal1 = CommonClasses.Execute("SELECT  INM_UT1_FILE_NO,INM_FILE_NO as INM_ARE_REMARK from INVOICE_MASTER where INM_CODE=" + code + "");

            if (dtfinal1.Rows.Count > 0)
            {
            ReportDocument rptname = null;
            rptname = new ReportDocument();


            rptname.Load(Server.MapPath("~/Reports/rptARE1Declaration.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptARE1Declaration.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtfinal1);

            CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
            }


            #endregion
        }
        else if (printtype == "Eximination")
        {
            #region Eximination
            DataTable dtfinal1 = CommonClasses.Execute("SELECT INVOICE_MASTER.INM_PER_E_NO, INVOICE_MASTER.INM_CUST_SEAL_NO, COMPANY_MASTER.CM_NAME, COMPANY_MASTER.CM_CODE,COMPANY_MASTER.CM_ADDRESS2 as CM_ADDRESS1, INVOICE_MASTER.INM_CODE, INVOICE_MASTER.INM_DATE, INVOICE_MASTER.INM_AREA_FORM_NO,  INVOICE_MASTER.INM_SEAL_NO, INVOICE_MASTER.INM_CONTA_NO FROM  COMPANY_MASTER CROSS JOIN INVOICE_MASTER where INM_CODE=" + code + "");

            if (dtfinal1.Rows.Count > 0)
            {
            ReportDocument rptname = null;
            rptname = new ReportDocument();


            rptname.Load(Server.MapPath("~/Reports/rptEximinationReport.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptEximinationReport.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtfinal1);

            CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
            }


            #endregion
        }
        else if (printtype == "ANNEXURE C-1")
        {
            #region ANNEXUREC-1
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dtfinal1 = CommonClasses.Execute(" SELECT distinct( INM_CODE) as INM_CODE,INM_EXA_BOXES, INVOICE_MASTER.INM_CONTA_NO,INVOICE_MASTER.INM_SHIPP_BILL_NO, INVOICE_MASTER.INM_NONCARGO_NOPAKG,INVOICE_MASTER.INM_DATE, COMPANY_MASTER.CM_CODE, COMPANY_MASTER.CM_NAME, INVOICE_MASTER.INM_IEC_NO, INVOICE_MASTER.INM_CEN_EXC_REG, INVOICE_MASTER.INM_BOND_NO, COMPANY_MASTER.CM_ADDRESS1, INVOICE_MASTER.INM_DATE_OF_EXAM, INVOICE_MASTER.INM_SUP_C_EXC_NAME,INVOICE_MASTER.INM_INSP_C_EXC_NAME, COMPANY_MASTER.CM_COMMISONERATE, COMPANY_MASTER.CM_EXCISE_DIVISION, COMPANY_MASTER.CM_EXCISE_RANGE, INVOICE_MASTER.INM_NO, INVOICE_MASTER.INM_PACK_AMT, PARTY_MASTER.P_NAME, PARTY_MASTER.P_ADD1, INVOICE_MASTER.INM_M_NO, INVOICE_MASTER.INM_CUST_SEAL_NO, INVOICE_MASTER.INM_OTS_NO, INVOICE_MASTER.INM_CODE, INVOICE_MASTER.INM_P_CODE, COMPANY_MASTER.CM_PAN_NO, INVOICE_MASTER.INM_SEAL_NO FROM   INVOICE_DETAIL INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE CROSS JOIN COMPANY_MASTER  where  INM_CODE=" + code + "");
          
            DataTable dtitem = CommonClasses.Execute("  SELECT  SUM(INVOICE_DETAIL.IND_INQTY)as IND_INQTY, ITEM_MASTER.I_NAME FROM  INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE AND INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE LEFT OUTER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE WHERE(INVOICE_MASTER.INM_CODE=" + code + ")GROUP BY INVOICE_DETAIL.IND_INQTY, INVOICE_DETAIL.IND_I_CODE, INVOICE_MASTER.INM_CODE, ITEM_MASTER.I_NAME");

            DataTable dtQty = CommonClasses.Execute("  SELECT  SUM(INVOICE_DETAIL.IND_INQTY)as IND_INQTY,sum(IND_NET_WEIGHT ) as IND_NET_WEIGHT,SUM(IND_NO_OF_BARRELS) as IND_NO_OF_BARRELS FROM  INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE AND INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE LEFT OUTER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE WHERE(INVOICE_MASTER.INM_CODE=" + code + ")");

           
            if (dtfinal1.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();


                rptname.Load(Server.MapPath("~/Reports/rptExpInvAnnexureC1.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptExpInvAnnexureC1.rpt");
                rptname.Refresh();
               // rptname.SetDataSource(ds);
                string s="";
                double Qty =Convert.ToDouble( dtQty.Rows[0]["IND_INQTY"].ToString());
                string PalletNo = dtfinal1.Rows[0]["INM_EXA_BOXES"].ToString();
                string Netweight = dtQty.Rows[0]["IND_NET_WEIGHT"].ToString();
                string NoofBar = dtQty.Rows[0]["IND_NO_OF_BARRELS"].ToString();


                for (int i = 0; i < dtitem.Rows.Count;i++ )
                {
                    s = s + dtitem.Rows[i]["I_NAME"].ToString() + " ,& ";
                }
                s = s.Remove(s.Length - 2, 2);
                s = s + "(As per Invoice & Packing List)";
               
                rptname.Database.Tables[0].SetDataSource(dtfinal1);
               // rptname.Database.Tables[1].SetDataSource(dtitem);
                rptname.SetParameterValue("txtItem",s);
                rptname.SetParameterValue("txtQty",Qty);
                rptname.SetParameterValue("txtPalletNo", PalletNo);
                rptname.SetParameterValue("txtNetweight", Netweight);
                rptname.SetParameterValue("txtNoofBar", NoofBar);

                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
            }


            #endregion
        }
        else if (printtype == "AUTHORITYLETTER")
        {
            #region Authority Letter
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dtfinal1 = CommonClasses.Execute(" SELECT distinct( INM_CODE) as INM_CODE,INM_DATE,INM_NO,INM_LC_NO,INM_LC_DATE,INM_TRANSPORT_BY as INM_TRANSPORT,CM_COMM_CUSTOM,CM_AUT_SPEC_SIGN from  INVOICE_MASTER,COMPANY_MASTER  where CM_CODE=INM_CM_CODE and INM_CODE=" + code + "");
                      
            if (dtfinal1.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();


                rptname.Load(Server.MapPath("~/Reports/rptAuthorityLetter.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptAuthorityLetter.rpt");
                rptname.Refresh();             

                rptname.Database.Tables[0].SetDataSource(dtfinal1);
                // rptname.Database.Tables[1].SetDataSource(dtitem);
                rptname.SetParameterValue("txtCompFinancialYr", Session["CompanyFinancialYr"].ToString());
              

                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
            }


            #endregion
        }
          else
        {
            #region PROFORMA INVOICE
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dtfinal1 = CommonClasses.Execute(" SELECT  INM_PORT_OF_LOAD,INM_PORT_OF_DISCH,INM_CODE,COMPANY_MASTER.CM_NAME, COMPANY_MASTER.CM_ADDRESS1, COMPANY_MASTER.CM_PHONENO1, COMPANY_MASTER.CM_FAXNO,  COMPANY_MASTER.CM_EMAILID, COMPANY_MASTER.CM_WEBSITE, COMPANY_MASTER.CM_ISO_NUMBER, COMPANY_MASTER.CM_BANK_NAME,  COMPANY_MASTER.CM_BANK_ADDRESS, COMPANY_MASTER.CM_BANK_ACC_NO, COMPANY_MASTER.CM_ACC_TYPE, COMPANY_MASTER.CM_B_SWIFT_CODE, COMPANY_MASTER.CM_IFSC_CODE, COMPANY_MASTER.CM_ADDRESS2, COMPANY_MASTER.CM_CODE, COUNTRY_MASTER1.COUNTRY_NAME as INM_COUNTRY_ORIGIN, COUNTRY_MASTER2.COUNTRY_NAME as INM_COUNTRY_DEST, PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, PARTY_MASTER.P_CONTACT, PARTY_MASTER.P_ADD1, PARTY_MASTER.P_MOB, PARTY_MASTER.P_FAX,  INVOICE_MASTER.INM_NO, INVOICE_MASTER.INM_DATE, INVOICE_MASTER.INM_IEC_NO, INVOICE_MASTER.INM_PRE_CARRIAGE, INVOICE_MASTER.INM_PLACE_REC, INVOICE_MASTER.INM_CONTRY_ORIGIN as INM_CONTRY_ORIGIN1, INVOICE_MASTER.INM_COUNTRY_DEST, INVOICE_MASTER.INM_VOY_NO, INVOICE_MASTER.INM_FINAL_DEST as INM_FINAL_DEST1, INVOICE_MASTER.INM_TOD, INVOICE_MASTER.INM_TOP, INVOICE_MASTER.INM_M_NO, INVOICE_MASTER.INM_NOS_PACK FROM    COUNTRY_MASTER COUNTRY_MASTER1,COUNTRY_MASTER COUNTRY_MASTER2, INVOICE_MASTER ,  COMPANY_MASTER , PARTY_MASTER  where  COUNTRY_MASTER1.COUNTRY_CODE = INVOICE_MASTER.INM_CONTRY_ORIGIN and INVOICE_MASTER.INM_COUNTRY_DEST= COUNTRY_MASTER2.COUNTRY_CODE and INM_P_CODE=P_CODE and INM_CODE=" + code + "  ");

            DataTable dtitem = CommonClasses.Execute(" SELECT   IND_GROSS_WEIGHT,IND_NET_WEIGHT,  INVOICE_DETAIL.IND_INM_CODE, INVOICE_DETAIL.IND_I_CODE, INVOICE_DETAIL.IND_INQTY, INVOICE_DETAIL.IND_RATE, INVOICE_DETAIL.IND_AMT, INVOICE_DETAIL.IND_GROSS_WEIGHT, INVOICE_DETAIL.IND_NET_WEIGHT,  ITEM_UNIT_MASTER.I_UOM_NAME, INVOICE_MASTER.INM_CODE,I_NAME FROM   ITEM_UNIT_MASTER , INVOICE_DETAIL , INVOICE_MASTER ,ITEM_MASTER where INM_CODE=IND_INM_CODE and I_CODE=IND_I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INM_CODE=" + code + " ");


           
            if (dtfinal1.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();


                rptname.Load(Server.MapPath("~/Reports/rptProformaInvoice.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptProformaInvoice.rpt");
                rptname.Refresh();

                //string Load = dtfinal1.Rows[0]["INM_PORT_OF_LOAD"].ToString();
               // string Disch = dtfinal1.Rows[0]["INM_PORT_OF_DISCH"].ToString();
              
                rptname.SetDataSource(dtfinal1);
               rptname.Database.Tables[1].SetDataSource(dtitem);
               string CINNo = DL_DBAccess.GetColumn("select CM_CIN_NO from COMPANY_MASTER where CM_ID='" + Session["CompanyId"] + "'");
               rptname.SetParameterValue("txtCINNo", CINNo);  
               // rptname.SetParameterValue("txtportl", Load);
                //rptname.SetParameterValue("txtport", Disch);
              
                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
            }


            #endregion
        }
            }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "GenerateReport", Ex.Message);

        }

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewExportInvoice.aspx", false);
    }
    #endregion

}
