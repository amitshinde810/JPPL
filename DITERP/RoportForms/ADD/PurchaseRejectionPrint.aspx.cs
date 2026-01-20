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

public partial class RoportForms_ADD_PurchaseRejectionPrint : System.Web.UI.Page
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
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejection.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();

        DataTable dtfinal = CommonClasses.Execute("Select PR_CODE,P_NAME,P_ADD1, PR_NO,Convert(varchar,PR_DATE,106) as PR_DATE,PR_CHALLAN_NO,Convert(varchar,PR_CHALLAN_DATE,106) as PR_CHALLAN_DATE,I_CODENO,I_NAME,PRD_CHALLAN_QTY,PRD_RECEIVED_QTY,PRD_ORIGINAL_QTY,PRD_RATE,PRD_AMOUNT from PARTY_MASTER,PURCHASE_REJECTION_MASTER,ITEM_MASTER,PURCHASE_REJECTION_DETAIL where P_CODE=PR_P_CODE and PR_CODE=PRD_PR_CODE and PRD_I_CODE=I_CODE and PURCHASE_REJECTION_MASTER.ES_DELETE=0 and PR_CODE=" + code + "");
        if (dtfinal.Rows.Count > 0)
        {
        }
        try
        {
            //if (p_type == "saleorder")
            //{


            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptPurchaseRejectionPrint.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptPurchaseRejectionPrint.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtfinal);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd1"].ToString());
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
