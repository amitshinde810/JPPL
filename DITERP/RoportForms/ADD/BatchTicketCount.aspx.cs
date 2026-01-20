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

public partial class RoportForms_ADD_BatchTicketCount : System.Web.UI.Page
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
            Query = "select COUNT(BT_NO) BT_NO,convert(varchar,BT_DATE,106) As BT_DATE from BATCH_MASTER where BT_CM_CODE=" + cpom_code + " and ES_DELETE=0 group by BT_DATE order by BT_DATE asc";

           DataTable dtfinal = CommonClasses.Execute(Query);

            if (dtfinal.Rows.Count > 0)
            {

                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/BatchTicketCount.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/BatchTicketCount.rpt");
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
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                CrystalReportViewer1.ReportSource = rptname;

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket Count", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
