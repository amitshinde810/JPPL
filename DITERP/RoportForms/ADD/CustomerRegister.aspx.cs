using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_CustomerRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            // bool chkAllSector = Convert.ToBoolean(Request.QueryString[3].ToString());

            if (ChkAll == true)
            {
                GenerateReport(Title, "All");
            }
            if (ChkAll == false)
            {
                string val = Request.QueryString[2];
                GenerateReport(Title, val);
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string val/*, string val1*/)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        if (val == "All")
        {
            Query = "SELECT P_NAME,P_ADD1,P_MOB,P_PHONE,P_EMAIL,P_NOTE FROM PARTY_MASTER where PARTY_MASTER.ES_DELETE=0 ";
        }
        if (val != "All")
        {
            Query = "SELECT P_NAME,P_ADD1,P_MOB,P_PHONE,P_EMAIL,P_NOTE FROM PARTY_MASTER where PARTY_MASTER.ES_DELETE=0 and P_CODE='" + val + "'";
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptCustomerMaster.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptCustomerMaster.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

            CrystalReportViewer1.ReportSource = rptname;
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
            CommonClasses.SendError("Customer Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master Register", "btnCancel_Click", Ex.Message);
        }
    }
}
