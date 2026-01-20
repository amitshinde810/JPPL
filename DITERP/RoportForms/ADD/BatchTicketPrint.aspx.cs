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

public partial class RoportForms_ADD_BatchTicketPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cpom_code = "";
    string print_type = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        cpom_code = Request.QueryString[0];
        GenerateReport(cpom_code);
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewBatchTicket.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion


    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        try
        {
            string BatchType = DL_DBAccess.GetColumn("select isnull(BT_TYPE,0) as BT_TYPE from BATCH_MASTER where BT_CODE='"+code+"'");
            if (BatchType == "1")
            {
                Query = "select BT_NO,BT_DATE,SHM_FORMULA_CODE,SHM_FORMULA_NAME,P_NAME,BTD_PROCESS_CODE as PROCESS_CODE,PROCESS_NAME, I_CODENO,I_CODE,BTD_STEP_NO as STEP_NO,BTD_QTY as QTY_IN_LTR,BTD_WGT as WEIGHT_IN_KG,BTD_QTY_IN as QtyinKG from BATCH_MASTER,BATCH_DETAIL,PROCESS_MASTER,ITEM_MASTER ,SHADE_MASTER,WORK_ORDER_MASTER,PARTY_MASTER where BT_CODE=BTD_BT_CODE and BTD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and BTD_I_CODE=I_CODE and BT_SHM_CODE=SHM_CODE and SHADE_MASTER.ES_DELETE=0 and BT_WO_CODE=WO_CODE and WORK_ORDER_MASTER.ES_DELETE=0 and WO_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and BT_CODE='" + code + "' order by BTD_STEP_NO,I_CODENO";
            }
            else
            {
                Query = "select BT_NO,BT_DATE,SHM_FORMULA_CODE,SHM_FORMULA_NAME,'' as P_NAME,BTD_PROCESS_CODE as PROCESS_CODE,PROCESS_NAME, I_CODENO,I_CODE,BTD_STEP_NO as STEP_NO,BTD_QTY as QTY_IN_LTR,BTD_WGT as WEIGHT_IN_KG,BTD_QTY_IN as QtyinKG from BATCH_MASTER,BATCH_DETAIL,PROCESS_MASTER,ITEM_MASTER ,SHADE_MASTER where BT_CODE=BTD_BT_CODE and BTD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and BTD_I_CODE=I_CODE and BT_SHM_CODE=SHM_CODE and SHADE_MASTER.ES_DELETE=0  and BT_CODE='" + code + "' order by BTD_STEP_NO,I_CODENO";
            }
            DataTable dtfinal = CommonClasses.Execute(Query);

            if (dtfinal.Rows.Count > 0)
            {

                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptBatchTicketPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptBatchTicketPrint.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                //rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                //string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,ENQUIRY_MASTER where INQ_CODE='" + code + "' and ISO_SCREEN_NO=94 and ISO_WEF_DATE<=INQ_REQ_DATE order by ISO_WEF_DATE DESC");
                //if (IsoNo == "")
                //{
                //    rptname.SetParameterValue("txtIsoNo", "1");
                //}
                //else
                //{
                //    rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
                //}
                CrystalReportViewer1.ReportSource = rptname;

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
