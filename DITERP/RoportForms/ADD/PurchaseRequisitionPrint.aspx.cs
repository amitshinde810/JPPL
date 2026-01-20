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

public partial class RoportForms_ADD_PurchaseRequisitionPrint : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            int Code =Convert.ToInt32( Request.QueryString[0].ToString());
           
            GenerateReport(Code);




        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(int Code)
    {

        try
        {


            string Query1 = "";
             Query1 = "Select PRM_TYPE from PRUCHASE_REQUISITION_MASTER where PRM_CODE="+Code+" ";
            DataTable dt1 = new DataTable();
            dt1 = CommonClasses.Execute(Query1);
            string Query = "";
            if (dt1.Rows[0]["PRM_TYPE"].ToString() == "2")
            {
                Query = "Select PRM_CODE,PRM_DATE,UM_NAME as UM_USERNAME,PRM_DEPARTMENT,I1.I_CODENO+' '+(I1.I_NAME+')' as RequiredFor,PRM_NO,PRD_REQ_QTY,I2.I_NAME,PRD_ORD_QTY,PRD_REMARK from PRUCHASE_REQUISITION_MASTER,ITEM_MASTER I1,ITEM_MASTER I2,USER_MASTER,PURCHASE_REQUISION_DETAIL where PRM_UM_CODE=UM_CODE and PRM_I_CODE=I1.I_CODE  and PRM_CODE=PRD_PRM_CODE and I2.I_CODE=PRD_I_CODE  and  PRM_CODE=" + Code + " ";

            }
            else
            {


               // Query = "Select LG_CODE,LG_DATE,LG_SOURCE,UM_USERNAME,LG_EVENT ,LG_COMP_NAME,LG_DOC_NAME,LG_U_NAME,LG_IP_ADDRESS from LOG_MASTER,USER_MASTER where LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " and  USER_MASTER.UM_CODE=LOG_MASTER.LG_U_CODE ";
                Query = "Select PRM_CODE,PRM_DATE,UM_NAME as UM_USERNAME,PRM_DEPARTMENT,I1.I_CODENO+' ('+I1.I_NAME+')' as RequiredFor,MR_BATCH_NO,PRM_NO,PRD_REQ_QTY,I2.I_NAME,PRD_ORD_QTY,PRD_REMARK from PRUCHASE_REQUISITION_MASTER,ITEM_MASTER I1,ITEM_MASTER I2,USER_MASTER,MATERIAL_REQUISITION_MASTER,PURCHASE_REQUISION_DETAIL where PRM_UM_CODE=UM_CODE and PRM_I_CODE=I1.I_CODE and PRM_MR_CODE=MR_CODE and PRM_CODE=PRD_PRM_CODE and I2.I_CODE=PRD_I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 and  PRM_CODE=" + Code + " ";
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();



                rptname.Load(Server.MapPath("~/Reports/rptPurchaseRequisitionPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptPurchaseRequisitionPrint.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    //rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    //rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

               

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
            #endregion
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRequisition.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
