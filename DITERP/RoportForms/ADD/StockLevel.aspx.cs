using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;
public partial class RoportForms_ADD_StockLevel : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }
    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];

            string Type = Request.QueryString[1].ToString();
            string cond = Request.QueryString[2].ToString();

            GenerateReport(Title, Type, cond);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Type, string cond)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        string repType = "";
        string repType1 = "";
        if (Type == "MIN")
        {
            //Query = "SELECT I_CODE, I_CODENO,I_NAME,ITEM_MASTER.I_CAT_CODE,I_CAT_NAME,ITEM_MASTER.I_UOM_CODE,I_UOM_NAME,I_MIN_LEVEL,I_MAX_LEVEL,I_REORDER_LEVEL,isnull((SELECT round(SUM(STL_DOC_QTY),2) FROM STOCK_LEDGER where STL_I_CODE=I_CODE),0) AS CURREN_BAL  into #temp FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER where ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE   AND     " + cond + "  ITEM_MASTER.ES_DELETE=0      SELECT I_CODE,	I_CODENO,	I_NAME	,I_CAT_CODE,	I_CAT_NAME,	I_UOM_CODE	,I_UOM_NAME,	I_MIN_LEVEL,	I_MAX_LEVEL,	I_REORDER_LEVEL,	CURREN_BAL,CURREN_BAL-I_MIN_LEVEL AS ORDER_QTY FROM #temp where    (CURREN_BAL <=I_MIN_LEVEL)   drop table  #temp ";
            Query = "SELECT I_CODE, I_CODENO,I_NAME,ITEM_MASTER.I_CAT_CODE,I_CAT_NAME,ITEM_MASTER.I_UOM_CODE,I_UOM_NAME,I_MIN_LEVEL,I_MAX_LEVEL,I_REORDER_LEVEL,isnull((SELECT round(SUM(STL_DOC_QTY),2) FROM STOCK_LEDGER where STL_I_CODE=I_CODE),0) AS CURREN_BAL  into #temp FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER where ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND " + cond + "     ITEM_MASTER.ES_DELETE=0 AND ITEM_CATEGORY_MASTER.I_CAT_CODE!='" + -2147483648 + "' SELECT I_CODE,I_CODENO,I_NAME,I_CAT_CODE,I_CAT_NAME,I_UOM_CODE,I_UOM_NAME,I_MIN_LEVEL,I_MAX_LEVEL,I_REORDER_LEVEL,CURREN_BAL,CURREN_BAL-I_MIN_LEVEL AS ORDER_QTY FROM #temp where I_MIN_LEVEL!=0  AND CURREN_BAL<= I_MIN_LEVEL ORDER BY CURREN_BAL-I_MIN_LEVEL drop table #temp ";
            repType = "Items below min level Report";
            repType1 = "Min level";
        }
        else
        {
            Query = "SELECT I_CODE, I_CODENO,I_NAME,ITEM_MASTER.I_CAT_CODE,I_CAT_NAME,ITEM_MASTER.I_UOM_CODE,I_UOM_NAME, I_MAX_LEVEL AS I_MIN_LEVEL,I_MAX_LEVEL,I_REORDER_LEVEL,isnull((SELECT round(SUM(STL_DOC_QTY),2) FROM STOCK_LEDGER where STL_I_CODE=I_CODE),0) AS CURREN_BAL  into #temp FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER where ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND " + cond + "  ITEM_MASTER.ES_DELETE=0 AND ITEM_CATEGORY_MASTER.I_CAT_CODE!='" + -2147483648 + "' SELECT I_CODE,I_CODENO,I_NAME,I_CAT_CODE,I_CAT_NAME,I_UOM_CODE,I_UOM_NAME,I_MIN_LEVEL,I_MAX_LEVEL,I_REORDER_LEVEL,CURREN_BAL,CURREN_BAL-I_MAX_LEVEL AS ORDER_QTY FROM #temp where I_MAX_LEVEL!=0 AND CURREN_BAL>= I_MAX_LEVEL ORDER BY CURREN_BAL-I_MAX_LEVEL desc drop table #temp";
            repType = "Item above maximum level Report";
            repType1 = "Max level";
        }

        // Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ";

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptStockLevel.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptStockLevel.rpt");
            rptname.Refresh();
            rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", repType);
            rptname.SetParameterValue("txtName", repType1);
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
            CommonClasses.SendError("Stock Ledger Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockLevel.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
}
