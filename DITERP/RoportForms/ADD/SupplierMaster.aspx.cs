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

public partial class RoportForms_ADD_SupplierMaster : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
           
            string cond = Request.QueryString[1].ToString();

            GenerateReport(Title,  cond);
        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT P_CODE,P_NAME,P_CONTACT,P_PARTY_CODE,P_VEND_CODE,P_ADD1	,P_CITY,P_PIN_CODE ,P_PHONE,P_MOB, P_EMAIL,P_PAN	,P_CST,P_LBT_NO ,P_CITY,P_COORDINATOR,	P_COORDINATOR_EMAIL FROM PARTY_MASTER  where " + cond + "  ES_DELETE=0 AND P_ACTIVE_IND=1 AND P_INHOUSE_IND=1 ");
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptSuppMaster.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptSuppMaster.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtTitle", "Approved Supplier Master");

                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion GenerateReport

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
            CommonClasses.SendError("Supplier Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
