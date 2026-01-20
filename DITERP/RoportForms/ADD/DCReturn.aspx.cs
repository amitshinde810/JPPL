using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

public partial class RoportForms_ADD_DCReturn : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cpom_code = "";
    string print_type = "";
    string type = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        cpom_code = Request.QueryString[0];
        print_type = Request.QueryString[1];
        type = Request.QueryString[2];
        GenerateReport(cpom_code, print_type, type);
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewTrayDCReturn.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer PO Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string code, string p_type, string type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            DataTable dtfinal = new DataTable();


            dtfinal = CommonClasses.Execute(" SELECT DNM_NO AS DCM_NO,DNM_DATE AS DCM_DATE, '' AS DCM_IS_RETURNABLE,0 AS DCM_ORDER_NO,  DND_REC_QTY   AS DCD_ORD_QTY ,I_NAME,P_NAME,P_ADD1,I_CODENO,DND_IREMARK AS DCD_REMARK FROM DC_RETURN_MASTER,DC_RETURN_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE DNM_CODE=DND_DNM_CODE AND DC_RETURN_MASTER.ES_DELETE=0  AND P_CODE=DNM_P_CODE AND I_CODE=DND_I_CODE AND DNM_CODE='" + code + "'");

            //  dtfinal = CommonClasses.Execute("SELECT DCM_NO,DCM_DATE,DCM_VEH_NO,case when DCM_IS_RETURNABLE =1 then 'Returnable' else 'NotReturnable' end as DCM_IS_RETURNABLE,DCM_ORDER_NO,DCD_ORD_QTY,I_NAME,P_NAME,P_ADD1,I_CODENO,DCD_REMARK FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE DCM_CODE=DCD_DCM_CODE AND P_CODE=DCM_P_CODE AND I_CODE=DCD_I_CODE AND DCM_CODE='" + code + "'");


            if (dtfinal.Rows.Count > 0)
            {
                if (p_type == "saleorder")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptDelChallanReturn.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDelChallanReturn.rpt"); rptname.Refresh();
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd"].ToString());
                    rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                    rptname.SetParameterValue("txtCompfax", Session["Companyfax"].ToString());
                    //string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,CUSTPO_MASTER where CPOM_CODE='" + code + "' and ISO_SCREEN_NO=81 and ISO_WEF_DATE<=CPOM_DATE");
                    //if (IsoNo == "")
                    //{
                    rptname.SetParameterValue("txtIsoNo", " ");
                    //}
                    //else
                    //{
                    //    rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
                    //}
                    CrystalReportViewer1.ReportSource = rptname;
                }

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Report", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
