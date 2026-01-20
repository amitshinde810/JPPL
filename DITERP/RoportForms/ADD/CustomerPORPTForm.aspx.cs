using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_CustomerPORPTForm : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cpom_code = "";
    string print_type = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        cpom_code = Request.QueryString[0];
        print_type = Request.QueryString[1];
        GenerateReport(cpom_code,print_type);
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer PO Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string code,string p_type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            DataTable dtfinal=new DataTable();
            if (p_type == "PrintWork")
            {
                dtfinal = CommonClasses.Execute("select E_H_EDU,E_EDU_CESS,E_SPECIAL,E_BASIC,CPOM_CODE,CPOM_P_CODE,P_NAME,P_ADD1,CPOM_PONO,CPOM_TYPE,WO_DATE as CPOM_DATE,CPOM_CR_DAYS,CPOD_CPOM_CODE,CPOD_I_CODE,I_NAME,I_CODENO,I_SPECIFICATION,I_MATERIAL,I_SIZE,I_UOM_NAME,WOD_WORK_ORDER_QTY as CPOD_ORD_QTY,CPOD_RATE,CPOM_T_NAME,CPOM_T_PER,CPOM_EXC_PER,CPOM_EXC_EDU_PER,CPOM_EXC_HEDU_PER,CPOD_DESC,CPOM_BASIC_AMT,CPOM_DISCOUNT_PER,CPOM_DISCOUNT_AMT,CPOM_DISCOUNT_REASON,CPOM_DEVIATION_AMT,CPOM_DEVIATION_REASON,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_T_AMT,CPOM_ROUNDING,CPOM_GRAND_TOT,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,ST_SALES_TAX,ST_TAX_NAME,WO_NO as CPOM_WORK_ODR_NO,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CM_ADDRESS1,CM_ADDRESS2, CM_FAXNO, CM_PHONENO1,CM_PHONENO2, CM_EMAILID, CM_WEBSITE,CPOM_PO_DATE from EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,PARTY_MASTER,SALES_TAX_MASTER,COMPANY_MASTER,WORK_ORDER_MASTER, WORK_ORDER_DETAIL where  ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE and ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE and CPOD_CPOM_CODE=CPOM_CODE and CPOM_P_CODE=P_CODE and  CUSTPO_DETAIL.CPOD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and CPOD_ST_CODE=ST_CODE and CPOM_CODE=WOD_CPOM_CODE and WO_CODE=WOD_WO_CODE and WO_CODE='" + code + "' and CPOM_CM_COMP_ID=CM_ID and CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' and WOD_I_CODE=CPOD_I_CODE and CM_CODE='" + Session["CompanyCode"] + "'");
            }
            else
            {
                dtfinal = CommonClasses.Execute("select E_H_EDU,E_EDU_CESS,E_SPECIAL,E_BASIC,CPOM_CODE,CPOM_P_CODE,P_NAME,P_ADD1,CPOM_PONO,CPOM_TYPE,CPOM_DATE,CPOM_CR_DAYS,CPOD_CPOM_CODE,CPOD_I_CODE,I_NAME,I_CODENO,I_SPECIFICATION,I_MATERIAL,I_SIZE,I_UOM_NAME,CPOD_ORD_QTY,CPOD_RATE,CPOM_T_NAME,CPOM_T_PER,CPOM_EXC_PER,CPOM_EXC_EDU_PER,CPOM_EXC_HEDU_PER,CPOD_DESC,CPOM_BASIC_AMT,CPOM_DISCOUNT_PER,CPOM_DISCOUNT_AMT,CPOM_DISCOUNT_REASON,CPOM_DEVIATION_AMT,CPOM_DEVIATION_REASON,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_T_AMT,CPOM_ROUNDING,CPOM_GRAND_TOT,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,ST_SALES_TAX,ST_TAX_NAME,CPOM_WORK_ODR_NO,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CM_ADDRESS1,CM_ADDRESS2, CM_FAXNO, CM_PHONENO1,CM_PHONENO2, CM_EMAILID, CM_WEBSITE,CPOM_PO_DATE from EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,PARTY_MASTER,SALES_TAX_MASTER,COMPANY_MASTER where  ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE and ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE and CPOD_CPOM_CODE=CPOM_CODE and CPOM_P_CODE=P_CODE and  CUSTPO_DETAIL.CPOD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and CPOD_ST_CODE=ST_CODE and  CPOM_CODE='" + code + "' and CPOM_CM_COMP_ID=CM_ID and CPOM_CM_COMP_ID='" + Session["CompanyId"] + "' and CM_CODE='" + Session["CompanyCode"] + "'");
            }
           
        
        if (dtfinal.Rows.Count > 0)
        {          
            if (p_type == "saleorder")
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptCustomerPO.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptCustomerPO.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd1"].ToString());
                rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                rptname.SetParameterValue("txtCompfax", Session["Companyfax"].ToString());
                string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,CUSTPO_MASTER where CPOM_CODE='" + code + "' and ISO_SCREEN_NO=81 and ISO_WEF_DATE<=CPOM_DATE");
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

            else
            {

                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptWorkOrderPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptWorkOrderPrint.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd1"].ToString());
                rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                rptname.SetParameterValue("txtCompfax", Session["Companyfax"].ToString());
                string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,CUSTPO_MASTER where CPOM_CODE='" + code + "' and ISO_SCREEN_NO=86 and ISO_WEF_DATE<=CPOM_DATE order by ISO_WEF_DATE DESC");
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
            CommonClasses.SendError("Customer PO Register", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
