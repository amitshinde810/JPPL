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
using CrystalDecisions.CrystalReports.Engine;


public partial class RoportForms_ADD_CustomerRejectionRPTForm : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cr_code = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        cr_code = Request.QueryString[0];

        GenerateReport(cr_code);
    }
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer PO Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");

//        DataTable dtfinal = CommonClasses.Execute("select E_H_EDU,E_EDU_CESS,E_SPECIAL,E_BASIC,CPOM_CODE,CPOM_P_CODE,P_NAME,P_ADD1,CPOM_PONO,CPOM_TYPE,CPOM_DATE,CPOM_CR_DAYS,CPOD_CPOM_CODE,CPOD_I_CODE,I_NAME,I_CODENO,I_SPECIFICATION,I_MATERIAL,I_SIZE,I_UOM_NAME,CPOD_ORD_QTY,CPOD_RATE,CPOM_T_NAME,CPOM_T_PER,CPOM_EXC_PER,CPOM_EXC_EDU_PER,CPOM_EXC_HEDU_PER,CPOD_DESC,CPOM_BASIC_AMT,CPOM_DISCOUNT_PER,CPOM_DISCOUNT_AMT,CPOM_DISCOUNT_REASON,CPOM_DEVIATION_AMT,CPOM_DEVIATION_REASON,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_T_AMT,CPOM_ROUNDING,CPOM_GRAND_TOT,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,ST_SALES_TAX,ST_TAX_NAME from EXCISE_TARIFF_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,PARTY_MASTER,SALES_TAX_MASTER where  ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE and ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE and CPOD_CPOM_CODE=CPOM_CODE and CPOM_P_CODE=P_CODE and CPOD_I_CODE=I_CODE and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and CUSTPO_MASTER.ES_DELETE=0 and CPOD_ST_CODE=ST_CODE and  CPOM_CODE='" + code + "'");

        DataTable dtfinal = CommonClasses.Execute("select I_CODENO,P_NAME,P_ADD1, CR_GIN_NO,Convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_CHALLAN_NO, Convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, I_NAME, CD_CHALLAN_QTY, CD_RECEIVED_QTY, CD_ORIGIONAL_QTY, CD_RATE, CD_AMOUNT from PARTY_MASTER, CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL, ITEM_MASTER where P_CODE=CR_P_CODE and CR_CODE=CD_CR_CODE and CD_I_CODE=I_CODE and CUSTREJECTION_MASTER.ES_DELETE=0 and CR_CODE='" + code + "'");
        if (dtfinal.Rows.Count > 0)
        {
        }
        try
        {
            //if (p_type == "saleorder")
            //{


                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptCustomerRejection.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptCustomerRejection.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd"].ToString());
                //rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                //rptname.SetParameterValue("txtCompfax", Session["Companyfax"].ToString());

                CrystalReportViewer1.ReportSource = rptname;
            //}

            //else
            //{

            //    ReportDocument rptname = null;
            //    rptname = new ReportDocument();

            //    rptname.Load(Server.MapPath("~/Reports/rptWorkOrderPrint.rpt"));
            //    rptname.FileName = Server.MapPath("~/Reports/rptWorkOrderPrint.rpt");
            //    rptname.Refresh();
            //    rptname.SetDataSource(dtfinal);
            //    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            //    rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd"].ToString());
            //    rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
            //    rptname.SetParameterValue("txtCompfax", Session["Companyfax"].ToString());

            //    CrystalReportViewer1.ReportSource = rptname;
            //}

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
