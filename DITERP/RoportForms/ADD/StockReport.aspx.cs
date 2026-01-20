using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_StockReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool ChkComp = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();
            string i_name = Request.QueryString[5].ToString();
            string detail = Request.QueryString[6].ToString();
            string stl_code = Request.QueryString[7].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail, stl_code);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string chkComp, string From, string To, string i_name, string detail, string stl_code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        DataTable dtStore = new DataTable();
        dtStore = CommonClasses.Execute("SELECT STORE_NAME  FROM STORE_MASTER where  STORE_CODE='" + stl_code + "'");

        if (detail == "Detail")
        {
            Query = "SELECT  DISTINCT  STL.STL_CODE, STL.STL_I_CODE, STL.STL_DOC_DATE, I_CODENO,isnull(I_UWEIGHT,0) as I_UWEIGHT,I_NAME,ISNULL( STL.STL_DOC_NUMBER,0) AS STL_DOC_NUMBER, CASE WHEN STL_DOC_TYPE='OutSUBINM' then 'DISPATCH TO SUB CONTRACTOR' WHEN STL_DOC_TYPE='TAXINV' then 'TAX INVOICE' WHEN STL_DOC_TYPE='STCADJ' then 'STOCK ADJUSTMENT' WHEN STL_DOC_TYPE='IWIAP' then 'SUB CONTRACTOR INWARD' WHEN STL_DOC_TYPE='DCIN' then 'Delivary Challan Inward'  WHEN STL_DOC_TYPE='Stock Transfer' THEN 'STOCK TRANSFER' WHEN STL_DOC_TYPE='STCADJ1' THEN 'STOCK ERROR CORRECTION' ELSE STL_DOC_TYPE END AS STL_DOC_TYPE,ISNULL((SELECT SUM(STL_DOC_QTY) FROM STOCK_LEDGER where STL_I_CODE=I_CODE AND STL_DOC_DATE<'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL_STORE_TYPE='" + stl_code + "' ),0) AS OpenStoc,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (convert(Date,STL_DOC_DATE) between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AND STL_STORE_TYPE='" + stl_code + "') AS numeric(10, 3)), 0) AS ADD_QTY ,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (convert(Date,STL_DOC_DATE) between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AND STL_STORE_TYPE='" + stl_code + "') AS numeric(10, 3)), 0) AS REM_QTY,I_INV_RATE,I_CURRENT_BAL FROM STOCK_LEDGER  AS STL ,ITEM_MASTER,STORE_MASTER where ITEM_MASTER.ES_DELETE=0 AND STORE_MASTER.STORE_CODE=STL_STORE_TYPE AND I_CM_COMP_ID='" + Session["CompanyId"] + "' and STL_I_CODE=I_CODE and I_CAT_CODE=-2147483648  AND STL_STORE_TYPE='" + stl_code + "' AND convert(date,STL_DOC_DATE) BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO ";
        }
        else
        {
            Query = " SELECT  DISTINCT STL.STL_I_CODE,  isnull(I_UWEIGHT,0) as I_UWEIGHT, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ISNULL((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER WHERE (STL_I_CODE = STL.STL_I_CODE) AND (STL_DOC_DATE  <'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_STORE_TYPE= '" + stl_code + "')), 0) AS OpenStoc, ISNULL(CAST ((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER   WHERE (STL_I_CODE = STL.STL_I_CODE) AND (convert(Date,STL_DOC_DATE) BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AND  (STL_STORE_TYPE = '" + stl_code + "')) AS numeric(10, 3)), 0) AS ADD_QTY, ISNULL(CAST ((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER  WHERE (STL_I_CODE = STL.STL_I_CODE)  AND (convert(Date,STL_DOC_DATE) BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AND(STL_STORE_TYPE = '" + stl_code + "')) AS numeric(10, 3)), 0) AS REM_QTY, ITEM_MASTER.I_INV_RATE FROM STOCK_LEDGER AS STL INNER JOIN STORE_MASTER ON STL.STL_STORE_TYPE = STORE_MASTER.STORE_CODE INNER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE WHERE (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CM_COMP_ID = '" + Session["CompanyId"] + "') AND (STL.STL_STORE_TYPE ='" + stl_code + "') AND (ITEM_MASTER.I_CAT_CODE = - 2147483648)   ORDER BY ITEM_MASTER.I_CODENO";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (i_name != "0")
        {
            DataView dv1 = dt.DefaultView;
            dv1.RowFilter = "STL_I_CODE in(" + i_name + ")";
            dt = dv1.ToTable();
        }
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            if (detail == "Detail")
            {
                rptname.Load(Server.MapPath("~/Reports/StockReport.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/StockReport.rpt");
            }
            else
            {
                rptname.Load(Server.MapPath("~/Reports/StockReport1.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/StockReport1.rpt");
            }
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", dtStore.Rows[0]["STORE_NAME"].ToString() + "  Stock Report From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/StockReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
