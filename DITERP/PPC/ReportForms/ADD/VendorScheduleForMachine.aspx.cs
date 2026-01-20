using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_VendorScheduleForMachine : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Date = Request.QueryString[1].ToString();
            string StrCond = Request.QueryString[2].ToString();
            GenerateReport(Title, Date, StrCond);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Date, string StrCond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DateTime dtMonth = new DateTime();
            dtMonth = Convert.ToDateTime(Date);
            Query = "SELECT DISTINCT I.I_CODE,I.I_CODENO , I.I_NAME, ISNULL(V.VS_CASTING_OFFLOADED, 0) AS VS_CASTING_OFFLOADED, ISNULL(V.VS_WEEK1, 0) AS VS_WEEK1,  ISNULL(V.VS_WEEK2, 0) AS VS_WEEK2, ISNULL(V.VS_WEEK3, 0) AS VS_WEEK3, ISNULL(V.VS_WEEK4, 0) AS VS_WEEK4,P_NAME FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE INNER JOIN PARTY_MASTER P ON V.VS_P_CODE=P.P_CODE inner join PRODUCT_MASTER PROD on I.I_CODE=PROD.PROD_I_CODE WHERE " + StrCond + " PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND (V.ES_DELETE = 0) and (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and V.VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "'";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/VendorSchedule.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/VendorSchedule.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Title", Title);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", Date);
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
            CommonClasses.SendError("Vendor Schedule Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewVendorScheduleForMachine.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}