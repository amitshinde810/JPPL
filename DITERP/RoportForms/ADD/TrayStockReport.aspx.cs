using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_TrayStockReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/TrayStockReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery challan Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Cond = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();

            GenerateReport(Title, From, To, group, way, Cond, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "SELECT   (SELECT ISNULL(SUM(STL_DOC_QTY),0) AS STOCK FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE<'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AS OPENING_QTY,  ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME ,(SELECT ISNULL(SUM(ABS(STL_DOC_QTY)),0) AS STOCK FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND STL_DOC_TYPE='DCTOUT') AS ISSUE_QTY,    (SELECT ISNULL(SUM(STL_DOC_QTY),0) AS STOCK FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND STL_DOC_TYPE='DCTIN') AS IN_QTY, ( SELECT ISNULL(SUM(DND_SCRAP_QTY),0) AS DND_SCRAP_QTY  FROM DC_RETURN_DETAIL,DC_RETURN_MASTER where DNM_CODE=DND_DNM_CODE AND DND_I_CODE=I_CODE AND DNM_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AS SCRAP FROM ITEM_MASTER INNER JOIN  ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE WHERE  " + Condition + "   (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE = - 2147483633)";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptTrayMIS.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptTrayMIS.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
