using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
public partial class PPC_ReportForms_ADD_UnplanRegister : System.Web.UI.Page
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
            string FDate = Request.QueryString[1].ToString();
            string TDate = Request.QueryString[2].ToString();
            string StrCond = Request.QueryString[3].ToString();
            GenerateReport(Title, FDate, TDate, StrCond);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string FDate, string TDate, string StrCond)
    {
        try
        {
            //dsCastingtobeInspected dataset is used
            DL_DBAccess = new DatabaseAccessLayer();
            DateTime dtMonth = new DateTime();
            dtMonth = Convert.ToDateTime(FDate);
            string Query = "";
            Query = " SELECT UNPLAN_SALE_SCHEDULE.US_I_CODE, UNPLAN_SALE_SCHEDULE.US_DATE, UNPLAN_SALE_SCHEDULE.US_QTY, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,ISNULL((SELECT SUM(IND_INQTY) FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE WHERE     (INVOICE_MASTER.INM_TYPE = 'TAXINV ') AND (INVOICE_MASTER.ES_DELETE = 0) AND INM_DATE=US_DATE AND IND_I_CODE=US_I_CODE  ),0) AS IND_INQTY ,ISNULL((SELECT MAX(IND_RATE) FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE WHERE (INVOICE_MASTER.INM_TYPE = 'TAXINV ') AND (INVOICE_MASTER.ES_DELETE = 0) AND INM_DATE=US_DATE AND IND_I_CODE=US_I_CODE  ),0) AS IND_RATE   FROM UNPLAN_SALE_SCHEDULE INNER JOIN ITEM_MASTER ON UNPLAN_SALE_SCHEDULE.US_I_CODE = ITEM_MASTER.I_CODE WHERE     (UNPLAN_SALE_SCHEDULE.ES_DELETE = 0) AND (UNPLAN_SALE_SCHEDULE.US_DATE BETWEEN '" + Convert.ToDateTime(FDate).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(FDate).ToString("dd/MMM/yyyy") + "'   AND US_CM_CODE='" + Session["CompanyCode"].ToString() + "' " + StrCond + "    )";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/UnplanSale.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/UnplanSale.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Title", Title);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", FDate);
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
            CommonClasses.SendError("Casting Inventory Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewCastingInventory.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
