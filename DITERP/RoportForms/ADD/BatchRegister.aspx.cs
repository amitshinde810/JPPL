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

public partial class RoportForms_ADD_BatchRegister : System.Web.UI.Page
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
            string Title = Request.QueryString[0];
            bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            string Type = Request.QueryString[4].ToString();

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            if (ChkAll == true)
            {
                GenerateReport(Title, "All", Type, From, To);
            }
            if (ChkAll != true)
            {
                GenerateReport(Title, "ONE", Type, From, To);
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string Type, string From, string To)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        if (Type == "0")
        {
            Query = "select BT_CODE,BT_NO,BT_DATE,SHM_FORMULA_CODE,SHM_FORMULA_NAME from BATCH_MASTER,SHADE_MASTER where SHM_CODE=BT_SHM_CODE and BATCH_MASTER.ES_DELETE=0 and SHADE_MASTER.ES_DELETE=0  and BT_CM_CODE='" + Session["CompanyCode"] + "' and BT_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' order by BT_DATE DESC";
        }
        else
        {
            Query = "select BT_CODE,BT_NO,BT_DATE,SHM_FORMULA_CODE,SHM_FORMULA_NAME from BATCH_MASTER,SHADE_MASTER where SHM_CODE=BT_SHM_CODE and BATCH_MASTER.ES_DELETE=0 and SHADE_MASTER.ES_DELETE=0  and BT_CM_CODE='" + Session["CompanyCode"] + "' and BT_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' order by BT_DATE DESC";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptBatchRegister.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptBatchRegister.rpt");
            rptname.Refresh();
            //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));

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
            CommonClasses.SendError("Batch Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBatchRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
