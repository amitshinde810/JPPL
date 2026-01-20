using System;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_SubContractorPOPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cpom_code = "";
    string autho_flag = "";
    string po_type = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        cpom_code = Request.QueryString[0];
        autho_flag = Request.QueryString[1];
        po_type = Request.QueryString[2];
        GenerateReport(cpom_code, autho_flag, po_type);
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewSubContractPO.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer PO Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string code, string Autho_Flag, string Po_Type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dtfinal = new DataTable();
        try
        {
            {
               // dtfinal = CommonClasses.Execute("SELECT 'PC-'+''+SUBSTRING(P_NAME,1,1)+''+ CONVERT(VARCHAR,P_PARTY_CODE) AS P_PARTY_CODE,SPOM_USER_CODE,UM_NAME,UM_USERNAME , dbo.SUPP_PO_DETAILS.SPOD_DISC_AMT, dbo.SUPP_PO_MASTER.SPOM_CODE, dbo.PARTY_MASTER.P_NAME, dbo.PARTY_MASTER.P_ADD1, dbo.PARTY_MASTER.P_CONTACT, dbo.PARTY_MASTER.P_PHONE,    P_PAN,    P_CST,   P_VAT,    P_ECC_NO,    P_EXC_DIV,  P_EXC_RANGE,  P_EXC_COLLECTORATE, dbo.SUPP_PO_MASTER.SPOM_PO_NO, dbo.SUPP_PO_MASTER.SPOM_DATE, dbo.ITEM_MASTER.I_CODE,     I_NAME, dbo.ITEM_MASTER.I_MATERIAL, dbo.SUPP_PO_DETAILS.SPOD_ORDER_QTY, dbo.SUPP_PO_DETAILS.SPOD_RATE, dbo.SUPP_PO_DETAILS.SPOD_ORDER_QTY * dbo.SUPP_PO_DETAILS.SPOD_RATE AS SPOD_AMOUNT, dbo.SUPP_PO_DETAILS.SPOD_EXC_Y_N, dbo.SALES_TAX_MASTER.ST_TAX_NAME, dbo.SALES_TAX_MASTER.ST_SALES_TAX, dbo.ITEM_MASTER.I_E_CODE,SPOD_EXC_PER AS E_BASIC, SPOD_EDU_CESS_PER AS  E_EDU_CESS, SPOD_H_EDU_CESS AS E_H_EDU, dbo.SUPP_PO_MASTER.SPOM_DELIVERED_TO, dbo.SUPP_PO_MASTER.SPOM_TRANSPOTER, dbo.SUPP_PO_MASTER.SOM_FREIGHT_TERM, dbo.SUPP_PO_MASTER.SPOM_PAY_TERM1, dbo.SUPP_PO_MASTER.SPOM_DEL_SHCEDULE, dbo.SUPP_PO_MASTER.SPOM_NOTES, dbo.SUPP_PO_MASTER.SPOM_GUARNTY, dbo.SUPP_PO_MASTER.SPOM_SUP_REF_DATE, dbo.PARTY_MASTER.P_MOB, dbo.SUPP_PO_DETAILS.SPOD_SPECIFICATION, dbo.ITEM_UNIT_MASTER.I_UOM_NAME, (CASE WHEN (LEN(I_NAME) / 30) = 0 THEN 1 ELSE (LEN(I_NAME) / 30) END) AS I_LENGTH, (CASE WHEN (LEN(I_MATERIAL) / 30) = 0 THEN 1 ELSE (LEN(I_MATERIAL) / 30) END) AS I_MATERIAL_LENGTH, (CASE WHEN (LEN(SPOD_SPECIFICATION) / 30) = 0 THEN 1 ELSE (LEN(SPOD_SPECIFICATION) / 30) END) AS SPOD_SPECIFICATION_LENGTH, ISNULL(dbo.SUPP_PO_MASTER.SPOM_AM_COUNT, 0) AS SPOM_AM_COUNT, dbo.SUPP_PO_MASTER.SPOM_PACKING AS SPOM_PAKING, dbo.SUPP_PO_MASTER.SPOM_SUP_REF, dbo.ITEM_MASTER.I_CAT_CODE, dbo.SUPP_PO_DETAILS.SPOD_ITEM_NAME, dbo.COMPANY_MASTER.CM_ADDRESS1, dbo.COMPANY_MASTER.CM_ADDRESS2, dbo.COMPANY_MASTER.CM_PHONENO1, dbo.COMPANY_MASTER.CM_FAXNO, dbo.COMPANY_MASTER.CM_EMAILID, dbo.COMPANY_MASTER.CM_PHONENO2, dbo.COMPANY_MASTER.CM_WEBSITE,dbo.COMPANY_MASTER.CM_BANK_NAME,dbo.COMPANY_MASTER.CM_BANK_ACC_NO,I_CODENO,E_COMMODITY,E_TARIFF_NO,P_EMAIL,SPOM_CIF_NO,SPOM_AM_DATE,PROCM_NAME AS  SPOM_PROJECT,SPOM_VALID_DATE,PROCESS_NAME,SPOD_DISC_PER,SPOM_PONO,SPOM_PROJ_NAME,PO_T_SHORT_NAME,PO_T_FIRST_LETTER,PO_T_DESC  ,SPOM_AM_DATE ,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT, ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,SPOD_E_CODE,SPOD_E_TARIFF_NO  ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO   FROM dbo.ITEM_UNIT_MASTER INNER JOIN  dbo.SUPP_PO_DETAILS ON dbo.ITEM_UNIT_MASTER.I_UOM_CODE = dbo.SUPP_PO_DETAILS.SPOD_UOM_CODE INNER JOIN  dbo.SUPP_PO_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_SPOM_CODE = dbo.SUPP_PO_MASTER.SPOM_CODE INNER JOIN  dbo.PARTY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_P_CODE = dbo.PARTY_MASTER.P_CODE INNER JOIN dbo.ITEM_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_I_CODE = dbo.ITEM_MASTER.I_CODE INNER JOIN  dbo.SALES_TAX_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_T_CODE = dbo.SALES_TAX_MASTER.ST_CODE INNER JOIN  dbo.EXCISE_TARIFF_MASTER ON dbo.ITEM_MASTER.I_SCAT_CODE = dbo.EXCISE_TARIFF_MASTER.E_CODE INNER JOIN  dbo.COMPANY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_CM_CODE = dbo.COMPANY_MASTER.CM_CODE   INNER JOIN USER_MASTER ON USER_MASTER.UM_CODE=SPOM_USER_CODE   INNER JOIN PROCESS_MASTER ON PROCESS_CODE=SPOD_PROCESS_CODE    INNER JOIN PO_TYPE_MASTER ON SPOM_TYPE=PO_T_CODE  INNER JOIN PROJECT_CODE_MASTER ON PROCM_CODE=SPOM_PROJECT   WHERE     (dbo.SUPP_PO_MASTER.ES_DELETE = 0) AND (dbo.SUPP_PO_MASTER.SPOM_CM_CODE = '" + Session["CompanyCode"] + "') AND (dbo.SUPP_PO_MASTER.SPOM_CODE = '" + code + "')");

                dtfinal = CommonClasses.Execute("SELECT 'PC-' + '' + SUBSTRING(PARTY_MASTER.P_NAME, 1, 1) + '' + CONVERT(VARCHAR, PARTY_MASTER.P_PARTY_CODE) AS P_PARTY_CODE, ISNULL(SPOM_PLANT,-2147483648) AS SPOM_PLANT ,SUPP_PO_MASTER.SPOM_USER_CODE, USER_MASTER.UM_NAME, USER_MASTER.UM_USERNAME, SUPP_PO_DETAILS.SPOD_DISC_AMT,SUPP_PO_MASTER.SPOM_CODE, PARTY_MASTER.P_NAME, PARTY_MASTER.P_ADD1, PARTY_MASTER.P_CONTACT, PARTY_MASTER.P_PHONE,PARTY_MASTER.P_PAN, PARTY_MASTER.P_CST, PARTY_MASTER.P_VAT, PARTY_MASTER.P_ECC_NO, PARTY_MASTER.P_EXC_DIV,PARTY_MASTER.P_EXC_RANGE, PARTY_MASTER.P_EXC_COLLECTORATE, SUPP_PO_MASTER.SPOM_PO_NO, SUPP_PO_MASTER.SPOM_DATE,  ITEM_MASTER.I_CODE, ITEM_MASTER.I_NAME, ITEM_MASTER.I_MATERIAL, SUPP_PO_DETAILS.SPOD_ORDER_QTY, SUPP_PO_DETAILS.SPOD_RATE,  SUPP_PO_DETAILS.SPOD_ORDER_QTY * SUPP_PO_DETAILS.SPOD_RATE AS SPOD_AMOUNT, SUPP_PO_DETAILS.SPOD_EXC_Y_N, SALES_TAX_MASTER.ST_TAX_NAME, SALES_TAX_MASTER.ST_SALES_TAX, ITEM_MASTER.I_E_CODE, SUPP_PO_DETAILS.SPOD_EXC_PER AS E_BASIC,  SUPP_PO_DETAILS.SPOD_EDU_CESS_PER AS E_EDU_CESS, SUPP_PO_DETAILS.SPOD_H_EDU_CESS AS E_H_EDU, SUPP_PO_MASTER.SPOM_DELIVERED_TO,  SUPP_PO_MASTER.SPOM_TRANSPOTER, SUPP_PO_MASTER.SOM_FREIGHT_TERM, SUPP_PO_MASTER.SPOM_PAY_TERM1,  SUPP_PO_MASTER.SPOM_DEL_SHCEDULE, SUPP_PO_MASTER.SPOM_NOTES, SUPP_PO_MASTER.SPOM_GUARNTY, SUPP_PO_MASTER.SPOM_SUP_REF_DATE,  PARTY_MASTER.P_MOB, SUPP_PO_DETAILS.SPOD_SPECIFICATION, ITEM_UNIT_MASTER.I_UOM_NAME, (CASE WHEN (LEN(I_NAME) / 30)  = 0 THEN 1 ELSE (LEN(I_NAME) / 30) END) AS I_LENGTH, (CASE WHEN (LEN(I_MATERIAL) / 30) = 0 THEN 1 ELSE (LEN(I_MATERIAL) / 30) END)  AS I_MATERIAL_LENGTH, (CASE WHEN (LEN(SPOD_SPECIFICATION) / 30) = 0 THEN 1 ELSE (LEN(SPOD_SPECIFICATION) / 30) END)  AS SPOD_SPECIFICATION_LENGTH, ISNULL(SUPP_PO_MASTER.SPOM_AM_COUNT, 0) AS SPOM_AM_COUNT,  SUPP_PO_MASTER.SPOM_PACKING AS SPOM_PAKING, SUPP_PO_MASTER.SPOM_SUP_REF, ITEM_MASTER.I_CAT_CODE, SUPP_PO_DETAILS.SPOD_ITEM_NAME,  COMPANY_MASTER.CM_ADDRESS1, COMPANY_MASTER.CM_ADDRESS2, COMPANY_MASTER.CM_PHONENO1, COMPANY_MASTER.CM_FAXNO,  COMPANY_MASTER.CM_EMAILID, COMPANY_MASTER.CM_PHONENO2, COMPANY_MASTER.CM_WEBSITE, COMPANY_MASTER.CM_BANK_NAME,  COMPANY_MASTER.CM_BANK_ACC_NO, ITEM_MASTER.I_CODENO, EXCISE_TARIFF_MASTER.E_COMMODITY, EXCISE_TARIFF_MASTER.E_TARIFF_NO,  PARTY_MASTER.P_EMAIL, SUPP_PO_MASTER.SPOM_CIF_NO, SUPP_PO_MASTER.SPOM_AM_DATE, PROJECT_CODE_MASTER.PROCM_NAME AS SPOM_PROJECT,  SUPP_PO_MASTER.SPOM_VALID_DATE, PROCESS_MASTER.PROCESS_NAME, SUPP_PO_DETAILS.SPOD_DISC_PER, SUPP_PO_MASTER.SPOM_PONO,  SUPP_PO_MASTER.SPOM_PROJ_NAME, PO_TYPE_MASTER.PO_T_SHORT_NAME, PO_TYPE_MASTER.PO_T_FIRST_LETTER, PO_TYPE_MASTER.PO_T_DESC,  SUPP_PO_MASTER.SPOM_AM_DATE AS Expr1, ISNULL(SUPP_PO_DETAILS.SPOD_EXC_AMT, 0) AS SPOD_EXC_AMT,  ISNULL(SUPP_PO_DETAILS.SPOD_EXC_E_AMT, 0) AS SPOD_EXC_E_AMT, ISNULL(SUPP_PO_DETAILS.SPOD_EXC_HE_AMT, 0) AS SPOD_EXC_HE_AMT,  SUPP_PO_DETAILS.SPOD_E_CODE, SUPP_PO_DETAILS.SPOD_E_TARIFF_NO, CASE WHEN P_LBT_IND = 1 THEN P_LBT_NO ELSE 'NA' END AS P_GST_NO,  EXCISE_TARIFF_MASTER_1.E_TARIFF_NO AS HSNNO, EXCISE_TARIFF_MASTER_1.E_COMMODITY AS HSN_E_COMMODITY FROM ITEM_UNIT_MASTER INNER JOIN SUPP_PO_DETAILS ON ITEM_UNIT_MASTER.I_UOM_CODE = SUPP_PO_DETAILS.SPOD_UOM_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PARTY_MASTER ON SUPP_PO_MASTER.SPOM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON SUPP_PO_DETAILS.SPOD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN SALES_TAX_MASTER ON SUPP_PO_DETAILS.SPOD_T_CODE = SALES_TAX_MASTER.ST_CODE INNER JOIN EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_SCAT_CODE = EXCISE_TARIFF_MASTER.E_CODE INNER JOIN COMPANY_MASTER ON SUPP_PO_MASTER.SPOM_CM_CODE = COMPANY_MASTER.CM_CODE INNER JOIN USER_MASTER ON USER_MASTER.UM_CODE = SUPP_PO_MASTER.SPOM_USER_CODE INNER JOIN PROCESS_MASTER ON PROCESS_MASTER.PROCESS_CODE = SUPP_PO_DETAILS.SPOD_PROCESS_CODE INNER JOIN PO_TYPE_MASTER ON SUPP_PO_MASTER.SPOM_TYPE = PO_TYPE_MASTER.PO_T_CODE INNER JOIN PROJECT_CODE_MASTER ON PROJECT_CODE_MASTER.PROCM_CODE = SUPP_PO_MASTER.SPOM_PROJECT INNER JOIN EXCISE_TARIFF_MASTER AS EXCISE_TARIFF_MASTER_1 ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER_1.E_CODE  WHERE     (SUPP_PO_MASTER.ES_DELETE = 0)  AND (dbo.SUPP_PO_MASTER.SPOM_CM_CODE = '" + Session["CompanyCode"] + "') AND (dbo.SUPP_PO_MASTER.SPOM_CODE = '" + code + "')");
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptSuConPoPrint1.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptSuConPoPrint1.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtUserName", dtfinal.Rows[0]["UM_NAME"].ToString());

                DataTable dtComp = new DataTable();
                dtComp = CommonClasses.Execute(" SELECT  CM_GST_NO, CM_NAME, CM_ID, CM_ADDRESS1,CM_ADDRESS3, CM_PHONENO1, CM_FAXNO, CM_EMAILID, CM_WEBSITE, CM_VAT_TIN_NO, CM_CST_NO, CM_PAN_NO, CM_ECC_NO,CM_VAT_WEF, CM_CST_WEF,CM_EXCISE_RANGE ,CM_EXCISE_DIVISION,CM_COMMISONERATE FROM COMPANY_MASTER where CM_ID='" + Session["CompanyId"].ToString() + "'");
                rptname.SetParameterValue("txtComp", dtComp.Rows[0]["CM_NAME"].ToString().ToUpper());
                rptname.SetParameterValue("txtAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString().ToUpper());
                if (dtfinal.Rows[0]["SPOM_PLANT"].ToString() == "-2147483648")
                {
                    rptname.SetParameterValue("txtDivision", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                }
                else
                {
                    rptname.SetParameterValue("txtDivision", dtComp.Rows[0]["CM_ADDRESS3"].ToString());
                }

                rptname.SetParameterValue("txtPhone", dtComp.Rows[0]["CM_PHONENO1"].ToString());
                rptname.SetParameterValue("txtFax", dtComp.Rows[0]["CM_FAXNO"].ToString());
                rptname.SetParameterValue("txtEmail", dtComp.Rows[0]["CM_EMAILID"].ToString());
                rptname.SetParameterValue("txtVAT", dtComp.Rows[0]["CM_VAT_TIN_NO"].ToString());
                rptname.SetParameterValue("txtCST", dtComp.Rows[0]["CM_CST_NO"].ToString());
                rptname.SetParameterValue("txtECC", dtComp.Rows[0]["CM_ECC_NO"].ToString());
                rptname.SetParameterValue("txtRange", dtComp.Rows[0]["CM_EXCISE_RANGE"].ToString()); 
                rptname.SetParameterValue("txtComm", dtComp.Rows[0]["CM_COMMISONERATE"].ToString());
                rptname.SetParameterValue("txtP_CODE", dtfinal.Rows[0]["P_PARTY_CODE"].ToString());
                rptname.SetParameterValue("txtGST", dtComp.Rows[0]["CM_GST_NO"].ToString());

                rptname.SetParameterValue("txtCM_VAT_WEF", Convert.ToDateTime(Session["CompanyVatWef"]).ToString("dd/MM/yyyy").ToString());
                rptname.SetParameterValue("txtCM_CST_WEF", Convert.ToDateTime(Session["CompanyCstWef"]).ToString("dd/MM/yyyy").ToString());
                if (Autho_Flag == "true")
                {
                    rptname.SetParameterValue("txtAuthFalg", "0");
                }
                else
                {
                    rptname.SetParameterValue("txtAuthFalg", "1");
                }

                string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,SUPP_PO_MASTER where SPOM_CODE='" + code + "' and ISO_SCREEN_NO=42 and ISO_WEF_DATE<=SPOM_DATE order by ISO_WEF_DATE DESC");
                rptname.SetParameterValue("txtIsoNo", IsoNo);
                if (IsoNo == "")
                {
                    rptname.SetParameterValue("txtIsoNo", "1");
                }
                else
                {
                    rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
                }
                CrystalReportViewer1.ReportSource = rptname;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Print", "GenerateReport", Ex.Message);
        }
    }
    #endregion
}
