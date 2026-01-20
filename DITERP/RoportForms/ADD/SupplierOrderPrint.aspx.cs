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

public partial class RoportForms_ADD_SupplierOrderPrint : System.Web.UI.Page
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
            Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
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
            //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
            //if (Po_Type == "Export PO")
            //{
            //    dtfinal = CommonClasses.Execute("SELECT  dbo.SUPP_PO_DETAILS.SPOD_DISC_AMT, dbo.SUPP_PO_MASTER.SPOM_CODE, dbo.PARTY_MASTER.P_NAME, dbo.PARTY_MASTER.P_ADD1, dbo.PARTY_MASTER.P_CONTACT, dbo.PARTY_MASTER.P_PHONE, dbo.COMPANY_MASTER.CM_PAN_NO as P_PAN, dbo.COMPANY_MASTER.CM_CST_NO as P_CST, dbo.COMPANY_MASTER.CM_VAT_TIN_NO as P_VAT, dbo.COMPANY_MASTER.CM_ECC_NO as P_ECC_NO, dbo.COMPANY_MASTER.CM_EXCISE_DIVISION as P_EXC_DIV, dbo.COMPANY_MASTER.CM_EXCISE_RANGE as P_EXC_RANGE, dbo.COMPANY_MASTER.CM_COMMISONERATE as P_EXC_COLLECTORATE, dbo.SUPP_PO_MASTER.SPOM_PO_NO, dbo.SUPP_PO_MASTER.SPOM_DATE, dbo.ITEM_MASTER.I_CODE, CASE WHEN SPOD_ITEM_NAME = '' THEN I_NAME ELSE SPOD_ITEM_NAME END AS I_NAME, dbo.ITEM_MASTER.I_MATERIAL, dbo.SUPP_PO_DETAILS.SPOD_ORDER_QTY, dbo.SUPP_PO_DETAILS.SPOD_RATE, dbo.SUPP_PO_DETAILS.SPOD_ORDER_QTY * dbo.SUPP_PO_DETAILS.SPOD_RATE AS SPOD_AMOUNT, dbo.SUPP_PO_DETAILS.SPOD_EXC_Y_N, dbo.SALES_TAX_MASTER.ST_TAX_NAME, dbo.SALES_TAX_MASTER.ST_SALES_TAX, dbo.ITEM_MASTER.I_E_CODE, dbo.EXCISE_TARIFF_MASTER.E_BASIC, dbo.EXCISE_TARIFF_MASTER.E_EDU_CESS, dbo.EXCISE_TARIFF_MASTER.E_H_EDU, dbo.SUPP_PO_MASTER.SPOM_DELIVERED_TO, dbo.SUPP_PO_MASTER.SPOM_TRANSPOTER, dbo.SUPP_PO_MASTER.SOM_FREIGHT_TERM, dbo.SUPP_PO_MASTER.SPOM_PAY_TERM1, dbo.SUPP_PO_MASTER.SPOM_DEL_SHCEDULE, dbo.SUPP_PO_MASTER.SPOM_NOTES, dbo.SUPP_PO_MASTER.SPOM_GUARNTY, dbo.SUPP_PO_MASTER.SPOM_SUP_REF_DATE, dbo.PARTY_MASTER.P_MOB, dbo.SUPP_PO_DETAILS.SPOD_SPECIFICATION, dbo.ITEM_UNIT_MASTER.I_UOM_NAME, dbo.SUPP_PO_MASTER.SPOM_CURR_CODE, dbo.CURRANCY_MASTER.CURR_SHORT_NAME AS CURR_NAME, dbo.CURRANCY_MASTER.CURR_RATE, dbo.COMPANY_MASTER.CM_BANK_NAME, dbo.COMPANY_MASTER.CM_BANK_ACC_NO, dbo.SUPP_PO_MASTER.SPOM_CIF_NO, dbo.SUPP_PO_MASTER.SPOM_PACKING AS SPOM_PAKING, dbo.COMPANY_MASTER.CM_BANK_NAME AS Expr1, dbo.COMPANY_MASTER.CM_BANK_ACC_NO AS Expr2, ISNULL(dbo.SUPP_PO_MASTER.SPOM_AM_COUNT, 0) AS SPOM_AM_COUNT, dbo.SUPP_PO_MASTER.SPOM_PACKING, dbo.SUPP_PO_MASTER.SPOM_SUP_REF, dbo.ITEM_MASTER.I_CAT_CODE, dbo.SUPP_PO_DETAILS.SPOD_ITEM_NAME, dbo.COMPANY_MASTER.CM_ADDRESS1, dbo.COMPANY_MASTER.CM_ADDRESS2, dbo.COMPANY_MASTER.CM_PHONENO1, dbo.COMPANY_MASTER.CM_PHONENO2, dbo.COMPANY_MASTER.CM_FAXNO, dbo.COMPANY_MASTER.CM_EMAILID, dbo.COMPANY_MASTER.CM_WEBSITE FROM         dbo.ITEM_UNIT_MASTER INNER JOIN  dbo.SUPP_PO_DETAILS ON dbo.ITEM_UNIT_MASTER.I_UOM_CODE = dbo.SUPP_PO_DETAILS.SPOD_UOM_CODE INNER JOIN  dbo.SUPP_PO_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_SPOM_CODE = dbo.SUPP_PO_MASTER.SPOM_CODE INNER JOIN  dbo.PARTY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_P_CODE = dbo.PARTY_MASTER.P_CODE INNER JOIN dbo.ITEM_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_I_CODE = dbo.ITEM_MASTER.I_CODE INNER JOIN  dbo.SALES_TAX_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_T_CODE = dbo.SALES_TAX_MASTER.ST_CODE INNER JOIN dbo.EXCISE_TARIFF_MASTER ON dbo.ITEM_MASTER.I_E_CODE = dbo.EXCISE_TARIFF_MASTER.E_CODE INNER JOIN dbo.CURRANCY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_CURR_CODE = dbo.CURRANCY_MASTER.CURR_CODE INNER JOIN dbo.COMPANY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_CM_CODE = dbo.COMPANY_MASTER.CM_CODE WHERE     (dbo.SUPP_PO_MASTER.ES_DELETE = 0) AND (dbo.SUPP_PO_MASTER.SPOM_CM_CODE = '" + Session["CompanyCode"] + "') AND (dbo.SUPP_PO_MASTER.SPOM_CODE = '" + code + "')");
            //    ReportDocument rptname = null;
            //     rptname = new ReportDocument();

            //     rptname.Load(Server.MapPath("~/Reports/rptImportSupplierPo.rpt"));
            //     rptname.FileName = Server.MapPath("~/Reports/rptImportSupplierPo.rpt");
            //     rptname.Refresh();
            //     rptname.SetDataSource(dtfinal);
            //     rptname.SetParameterValue("txtUserName", Session["Username"].ToString());
            //     rptname.SetParameterValue("txtCM_VAT_WEF",Convert.ToDateTime(Session["CompanyVatWef"]).ToString("dd/MM/yyyy").ToString());
            //     rptname.SetParameterValue("txtCM_CST_WEF", Convert.ToDateTime(Session["CompanyCstWef"]).ToString("dd/MM/yyyy").ToString());
            //     if (Autho_Flag == "true")
            //     {
            //         rptname.SetParameterValue("txtAuthFalg", "0");

            //     }
            //     else
            //     {
            //         rptname.SetParameterValue("txtAuthFalg", "1");

            //     }

            //     string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,SUPP_PO_MASTER where SPOM_CODE='" + code + "' and ISO_SCREEN_NO=42 and ISO_WEF_DATE<=SPOM_DATE order by ISO_WEF_DATE DESC");
            //     rptname.SetParameterValue("txtIsoNo", IsoNo);
            //     if (IsoNo == "")
            //     {
            //         rptname.SetParameterValue("txtIsoNo", "1");
            //     }
            //     else
            //     {
            //         rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
            //     }
            //     CrystalReportViewer1.ReportSource = rptname;
            //}
            //else
            {
                dtfinal = CommonClasses.Execute("SELECT  'PC-'+''+SUBSTRING(P_NAME,1,1)+''+ CONVERT(VARCHAR,P_PARTY_CODE) AS P_PARTY_CODE,   ISNULL(SPOM_PLANT,-2147483648) AS SPOM_PLANT, SPOM_USER_CODE,UM_NAME,UM_USERNAME , dbo.SUPP_PO_DETAILS.SPOD_DISC_AMT, dbo.SUPP_PO_MASTER.SPOM_CODE, dbo.PARTY_MASTER.P_NAME, dbo.PARTY_MASTER.P_ADD1, dbo.PARTY_MASTER.P_CONTACT, dbo.PARTY_MASTER.P_PHONE,  P_PAN, P_CST,  P_VAT,  P_ECC_NO,  P_EXC_DIV, P_EXC_RANGE, P_EXC_COLLECTORATE, dbo.SUPP_PO_MASTER.SPOM_PO_NO, dbo.SUPP_PO_MASTER.SPOM_DATE, dbo.ITEM_MASTER.I_CODE,  I_NAME, dbo.ITEM_MASTER.I_MATERIAL, dbo.SUPP_PO_DETAILS.SPOD_ORDER_QTY, dbo.SUPP_PO_DETAILS.SPOD_RATE, dbo.SUPP_PO_DETAILS.SPOD_ORDER_QTY * dbo.SUPP_PO_DETAILS.SPOD_RATE AS SPOD_AMOUNT, dbo.SUPP_PO_DETAILS.SPOD_EXC_Y_N, dbo.SALES_TAX_MASTER.ST_TAX_NAME, dbo.SALES_TAX_MASTER.ST_SALES_TAX, dbo.ITEM_MASTER.I_E_CODE, SPOD_EXC_PER AS E_BASIC, dbo.EXCISE_TARIFF_MASTER.E_EDU_CESS, dbo.EXCISE_TARIFF_MASTER.E_H_EDU, dbo.SUPP_PO_MASTER.SPOM_DELIVERED_TO, dbo.SUPP_PO_MASTER.SPOM_TRANSPOTER, dbo.SUPP_PO_MASTER.SOM_FREIGHT_TERM, dbo.SUPP_PO_MASTER.SPOM_PAY_TERM1, dbo.SUPP_PO_MASTER.SPOM_DEL_SHCEDULE, dbo.SUPP_PO_MASTER.SPOM_NOTES, dbo.SUPP_PO_MASTER.SPOM_GUARNTY, dbo.SUPP_PO_MASTER.SPOM_SUP_REF_DATE, dbo.PARTY_MASTER.P_MOB, dbo.SUPP_PO_DETAILS.SPOD_SPECIFICATION, dbo.ITEM_UNIT_MASTER.I_UOM_NAME, (CASE WHEN (LEN(I_NAME) / 30) = 0 THEN 1 ELSE (LEN(I_NAME) / 30) END) AS I_LENGTH, (CASE WHEN (LEN(I_MATERIAL) / 30) = 0 THEN 1 ELSE (LEN(I_MATERIAL) / 30) END) AS I_MATERIAL_LENGTH, (CASE WHEN (LEN(SPOD_SPECIFICATION) / 30) = 0 THEN 1 ELSE (LEN(SPOD_SPECIFICATION) / 30) END) AS SPOD_SPECIFICATION_LENGTH, ISNULL(dbo.SUPP_PO_MASTER.SPOM_AM_COUNT, 0) AS SPOM_AM_COUNT, dbo.SUPP_PO_MASTER.SPOM_PACKING AS SPOM_PAKING, dbo.SUPP_PO_MASTER.SPOM_SUP_REF, dbo.ITEM_MASTER.I_CAT_CODE, dbo.SUPP_PO_DETAILS.SPOD_ITEM_NAME, dbo.COMPANY_MASTER.CM_ADDRESS1, dbo.COMPANY_MASTER.CM_ADDRESS2, dbo.COMPANY_MASTER.CM_PHONENO1, dbo.COMPANY_MASTER.CM_FAXNO, dbo.COMPANY_MASTER.CM_EMAILID, dbo.COMPANY_MASTER.CM_PHONENO2, dbo.COMPANY_MASTER.CM_WEBSITE,dbo.COMPANY_MASTER.CM_BANK_NAME,dbo.COMPANY_MASTER.CM_BANK_ACC_NO,I_CODENO,E_COMMODITY,E_TARIFF_NO,P_EMAIL,SPOM_CIF_NO,SPOM_AM_DATE,PROCM_NAME AS  SPOM_PROJECT,SPOM_VALID_DATE ,SPOD_DISC_PER,SPOM_PONO,SPOM_PROJ_NAME,PO_T_SHORT_NAME,PO_T_FIRST_LETTER,PO_T_DESC,SPOM_AM_DATE  ,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT, ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,SPOD_E_CODE,SPOD_E_TARIFF_NO  ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO      FROM dbo.ITEM_UNIT_MASTER INNER JOIN  dbo.SUPP_PO_DETAILS ON dbo.ITEM_UNIT_MASTER.I_UOM_CODE = dbo.SUPP_PO_DETAILS.SPOD_UOM_CODE INNER JOIN  dbo.SUPP_PO_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_SPOM_CODE = dbo.SUPP_PO_MASTER.SPOM_CODE INNER JOIN  dbo.PARTY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_P_CODE = dbo.PARTY_MASTER.P_CODE INNER JOIN dbo.ITEM_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_I_CODE = dbo.ITEM_MASTER.I_CODE INNER JOIN  dbo.SALES_TAX_MASTER ON dbo.SUPP_PO_DETAILS.SPOD_T_CODE = dbo.SALES_TAX_MASTER.ST_CODE INNER JOIN  dbo.EXCISE_TARIFF_MASTER ON dbo.ITEM_MASTER.I_E_CODE = dbo.EXCISE_TARIFF_MASTER.E_CODE INNER JOIN  dbo.COMPANY_MASTER ON dbo.SUPP_PO_MASTER.SPOM_CM_CODE = dbo.COMPANY_MASTER.CM_CODE   INNER JOIN USER_MASTER ON USER_MASTER.UM_CODE=SPOM_USER_CODE   INNER JOIN PO_TYPE_MASTER ON SPOM_TYPE=PO_T_CODE  INNER JOIN PROJECT_CODE_MASTER ON SPOM_PROJECT=PROJECT_CODE_MASTER.PROCM_CODE   WHERE     (dbo.SUPP_PO_MASTER.ES_DELETE = 0) AND (dbo.SUPP_PO_MASTER.SPOM_CM_CODE = '" + Session["CompanyCode"] + "') AND (dbo.SUPP_PO_MASTER.SPOM_CODE = '" + code + "')");
                if (dtfinal.Rows.Count > 0)
                {


                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptSupplierPoPrint1.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSupplierPoPrint1.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtUserName", dtfinal.Rows[0]["UM_NAME"].ToString());


                    DataTable dtComp = new DataTable();
                    dtComp = CommonClasses.Execute(" SELECT CM_GST_NO,  CM_NAME, CM_ID, CM_ADDRESS1,CM_ADDRESS3, CM_PHONENO1, CM_FAXNO, CM_EMAILID, CM_WEBSITE, CM_VAT_TIN_NO, CM_CST_NO, CM_PAN_NO, CM_ECC_NO,CM_VAT_WEF, CM_CST_WEF,CM_EXCISE_RANGE ,CM_EXCISE_DIVISION,CM_COMMISONERATE FROM COMPANY_MASTER where CM_ID='" + Session["CompanyId"].ToString() + "'");
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion

}
