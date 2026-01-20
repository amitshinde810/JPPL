using System;
using System.Web.UI;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_InhouseVendorStock : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
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

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            string i_name = Request.QueryString[5].ToString();
            string detail = Request.QueryString[6].ToString();
            if (ChkAll == true && ChkComp == true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name, detail);
            }
            if (ChkAll != true && ChkComp == true)
            {
                GenerateReport(Title, "ONE", "All", From, To, i_name, detail);
            }
            if (ChkAll == true && ChkComp != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail);
            }
            if (ChkAll == true && ChkComp != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail);
            }
            if (ChkAll != true && ChkComp != true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail);
            }
            if (ChkAll != true && ChkComp == true)
            {
                GenerateReport(Title, "ONE", "ALL", From, To, i_name, detail);
            }
            if (ChkAll == true && ChkComp != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail);
            }
            if (ChkAll == true && ChkComp == true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name, detail);
            }
            if (ChkAll != true && ChkComp != true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail);
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string chkComp, string From, string To, string i_name, string detail)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        string str = "";
        if (chkComp != "All")
        {
            str = str + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
        }
        Query = "DROP table Vendor SELECT  ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,((SELECT ISNULL(SUM(CL_CQTY), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER WHERE   (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE IN ('OutSUBINM', 'IWIAP')) AND  (CL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'))) AS OP_QTY, ((SELECT ISNULL(SUM(CL_CQTY), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER AS CHALLAN_STOCK_LEDGER_2 WHERE   (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE = 'OutSUBINM') AND (CL_DOC_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'))) AS OUT_QTY, ((SELECT ISNULL(ABS(SUM(CL_CQTY)), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER AS CHALLAN_STOCK_LEDGER_1  WHERE  (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE = 'IWIAP') AND (CL_DOC_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'))) AS IN_QTY,I_INV_RATE,I_UWEIGHT   into Vendor FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN    ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN  PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN  ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE WHERE    ITEM_MASTER.I_CAT_CODE= '-2147483648'  AND (INVOICE_MASTER.INM_TYPE = 'OutSUBINM') AND (INVOICE_MASTER.ES_DELETE = 0) GROUP BY  ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,  ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,I_INV_RATE,I_UWEIGHT   SELECT A.I_CODE,A.I_CODENO,A.I_NAME ,(A.ADD_QTY+A.OpenStoc-A.REM_QTY) AS BAL,ISNULL(OP_QTY+OUT_QTY- IN_QTY,0) AS VBAL,A.I_UOM_NAME,A.I_UOM_CODE FROM (select I_CODE, ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME, ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc,I_INV_RATE,ISNULL(I_UWEIGHT,0) AS I_UWEIGHT from STOCK_LEDGER STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE  INNER JOIN  ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE  where  ITEM_MASTER.ES_DELETE=0  " + str + " AND (ITEM_MASTER.I_ACTIVE_IND = 1) AND I_CAT_CODE='-2147483648' ) as A LEFT OUTER JOIN Vendor ON A.I_CODE = Vendor.I_CODE GROUP BY A.I_CODE,A.I_CODENO,A.I_NAME,A.I_INV_RATE,A.I_UWEIGHT  ,A.ADD_QTY,A.OpenStoc,A.REM_QTY,Vendor.OP_QTY+Vendor.OUT_QTY- Vendor.IN_QTY   ,Vendor.I_UOM_CODE,Vendor.I_UOM_NAME,A.I_UOM_NAME,A.I_UOM_CODE ORDER BY A.I_CODENO    ";

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptInhouseVendor.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptInhouseVendor.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", Title + " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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
            Response.Redirect("~/RoportForms/VIEW/StockReportInHouseVendor.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
