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

public partial class RoportForms_ADD_ExiseTariffMaster : System.Web.UI.Page
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
            bool ChkTariffNoAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            string TariffNo = Request.QueryString[2].ToString();

            if (ChkTariffNoAll == true)
            {
                GenerateReport(Title, TariffNo, "All");
            }
            if (ChkTariffNoAll == false)
            {
                //string Comp = Request.QueryString[2];
                GenerateReport(Title, TariffNo, "NotAll");
            }


        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string TariffNo, string Comp1)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        if (Comp1 == "All")
        {
            Query = "SELECT E_CODE,E_TARIFF_NO,E_COMMODITY,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,E_EX_TYPE FROM EXCISE_TARIFF_MASTER WHERE ES_DELETE=0";
        }
        else
        {
            Query = "SELECT E_CODE,E_TARIFF_NO,E_COMMODITY,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,E_EX_TYPE FROM EXCISE_TARIFF_MASTER WHERE  ES_DELETE=0 AND E_CODE='" + TariffNo + "'";
        }

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {


            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/prtExiseTariffMaster.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/prtExiseTariffMaster.rpt");
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
            CommonClasses.SendError("User Master View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion



}
