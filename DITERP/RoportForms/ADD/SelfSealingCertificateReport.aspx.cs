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

public partial class RoportForms_ADD_SelfSealingCertificateReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
           
            string FromNo = Request.QueryString[0];
            string ToNo = Request.QueryString[1];
            string FromDate = Request.QueryString[2].ToString();
            GenerateReport(FromNo, ToNo, FromDate);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string FromNo, string ToNo, string FromDate)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        Query = "select INM_NO,INM_TRANSPORT_OWNER,INM_TRANSPORT_BY,INM_TRANSPORT_ADDRESS,CM_AUT_SPEC_SIGN,INM_AUTHO_SIGN from INVOICE_MASTER,COMPANY_MASTER where INM_NO='" + FromNo + "' and  INM_CM_CODE=CM_CODE and INVOICE_MASTER.ES_DELETE=0 and INM_INVOICE_TYPE<>1";

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptSelfSealingCertificate.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptSelfSealingCertificate.rpt");
            rptname.Refresh();

            rptname.SetDataSource(dt);

            string fy = Session["CompanyFinancialYr"].ToString();
            string un = Session["Username"].ToString();
            rptname.SetParameterValue("txtCompFinancialYr", fy);
            rptname.SetParameterValue("txtUserName", un);
            rptname.SetParameterValue("txtfrom", FromDate);

            DataTable dtNo = CommonClasses.Execute("select distinct INM_NO from INVOICE_MASTER where INM_NO<='" + ToNo + "' and INM_NO>='" + FromNo + "'   and ES_DELETE=0 and   INM_INVOICE_TYPE<>1 order by INM_NO");
            string InvocieNo = "";
            for (int i = 0; i < dtNo.Rows.Count; i++)
            {
                InvocieNo = InvocieNo + dtNo.Rows[i]["INM_NO"].ToString() + " , ";
            }
            rptname.SetParameterValue("txtInvoiceNo", InvocieNo);
            CrystalReportViewer1.ReportSource = rptname;
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            return;
        }
    }
    #endregion


    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Self Sealing Certificate Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSelfSealingCertificateReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Under Drawback Report", "btnCancel_Click", Ex.Message);
        }
    }
}
