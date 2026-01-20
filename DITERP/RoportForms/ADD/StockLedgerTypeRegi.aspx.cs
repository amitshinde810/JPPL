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


public partial class RoportForms_ADD_StockLedgerTypeRegi : System.Web.UI.Page
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
            bool ChkComp = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();
            string i_code = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();
            bool chkItem =Convert.ToBoolean(Request.QueryString[7].ToString());
            string Cat_code = Request.QueryString[8].ToString();

            
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            GenerateReport(Title,ChkAll ,ChkComp ,From,To,i_code,Type,  chkItem,Cat_code);
           
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, bool  chkdate, bool chkComp, string From, string To, string i_code, string Type, bool  chkItem,string Cat_code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";      
        if (Type == "Detail")
        {
            //Query = "select I_CODENO,STL_DOC_NUMBER,STL_DOC_DATE,isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0) as ADD_QTY,isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0) as REM_QTY,STL_CODE,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' END) AS STL_DOC_TYPE,STL_I_CODE,I_NAME,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER where  STL_I_CODE=I_CODE";
            Query = "select I_CODENO,STL_DOC_NUMBER,STL_DOC_DATE,isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0) as ADD_QTY,isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0) as REM_QTY,STL_CODE,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' END) AS STL_DOC_TYPE,STL_I_CODE,I_NAME,I_CAT_NAME,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER,ITEM_CATEGORY_MASTER where  STL_I_CODE=I_CODE and  ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE ";
            if (chkdate == true  && chkComp == true && chkItem ==true  )
            {
                Query = Query;
            }
            if (chkdate !=true  && chkComp == true && chkItem ==true )
            {
                Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
            }
            if (chkdate == true && chkItem != true && chkComp == true)
            {
                Query = Query + "and ITEM_CATEGORY_MASTER.I_CAT_CODE="+Cat_code +"";
            }
            if (chkdate == true  && chkItem==true && chkComp != true )
            {
                Query = Query + " and ITEM_MASTER.I_CODE='" + i_code + "' ";
            }
            if (chkdate != true  && chkComp != true && chkItem !=true )
            {
                Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ITEM_MASTER.I_CODE='" + i_code + "' and  ITEM_CATEGORY_MASTER.I_CAT_CODE='"+Cat_code +"'";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptStockLedgerTypeRegister.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptStockLedgerTypeRegister.rpt");
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
        else
        {
            Query = "select I_CODENO,ITEM_CATEGORY_MASTER.I_CAT_CODE,STL_DOC_NUMBER,STL_DOC_DATE,isnull(cast((case when STL_DOC_QTY > 0 THEN STL_DOC_QTY end) as numeric(10,3)),0) as ADD_QTY,isnull(ABS(cast((case when STL_DOC_QTY < 0 THEN STL_DOC_QTY end) as numeric(10,3))),0) as REM_QTY,STL_CODE,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' END) AS STL_DOC_TYPE,STL_I_CODE,I_NAME,I_CAT_NAME,isnull(cast((select SUM(STL_DOC_QTY) from STOCK_LEDGER SOL where STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND STL.STL_I_CODE=SOL.STL_I_CODE) as numeric(10,3)),0) As OpenStoc from STOCK_LEDGER STL,ITEM_MASTER,ITEM_CATEGORY_MASTER where  STL_I_CODE=I_CODE and  ITEM_MASTER.I_CAT_CODE=ITEM_CATEGORY_MASTER.I_CAT_CODE";
            if (chkdate == true && chkComp == true && chkItem == true)
            {
                Query = Query;
            }
            if (chkdate != true && chkComp == true && chkItem == true)
            {
                Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
            }
            if (chkdate == true && chkItem != true && chkComp == true)
            {
                Query = Query + "ITEM_CATEGORY_MASTER.I_CAT_CODE=" + Cat_code + "";
            }
            if (chkdate == true && chkItem == true && chkComp != true)
            {
                Query = Query + " and ITEM_MASTER.I_CODE=" + i_code + "' ";
            }
            if (chkdate != true && chkComp != true && chkItem != true)
            {
                Query = Query + " and STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and ITEM_MASTER.I_CODE='" + i_code + "' and  ITEM_CATEGORY_MASTER.I_CAT_CODE='" + Cat_code + "'";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptStockLedgerTypeDetailRegister.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptStockLedgerTypeDetailRegister.rpt");
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
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedgerRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
}
