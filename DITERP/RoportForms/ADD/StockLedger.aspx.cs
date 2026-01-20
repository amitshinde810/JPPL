using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Web; 
using ZXing;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using System.Collections.Generic;
using System.Linq; 
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Configuration;


public partial class RoportForms_ADD_StockLedger : System.Web.UI.Page
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

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }

            string i_name = Request.QueryString[5].ToString();
            bool ChkCatAll = Convert.ToBoolean(Request.QueryString[7].ToString());
            string category = Request.QueryString[8].ToString();
            string detail = Request.QueryString[6].ToString();
            string PlantCode = Request.QueryString[9].ToString();
            if (ChkAll == true && ChkComp == true && ChkCatAll == true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name, detail, category, "All", PlantCode);
            }
            if (ChkAll != true && ChkComp == true && ChkCatAll == true)
            {
                GenerateReport(Title, "ONE", "All", From, To, i_name, detail, category, "All", PlantCode);
            }
            if (ChkAll == true && ChkComp != true && ChkCatAll == true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail, category, "All", PlantCode);
            }
            if (ChkAll == true && ChkComp != true && ChkCatAll != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail, category, "ONE", PlantCode);
            }
            if (ChkAll != true && ChkComp != true && ChkCatAll == true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail, category, "All", PlantCode);
            }
            if (ChkAll != true && ChkComp == true && ChkCatAll != true)
            {
                GenerateReport(Title, "ONE", "ALL", From, To, i_name, detail, category, "ONE", PlantCode);
            }
            if (ChkAll == true && ChkComp != true && ChkCatAll != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name, detail, category, "ONE", PlantCode);
            }
            if (ChkAll == true && ChkComp == true && ChkCatAll != true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name, detail, category, "ONE", PlantCode);
            }

            if (ChkAll != true && ChkComp != true && ChkCatAll != true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name, detail, category, "ONE", PlantCode);
            }

        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string chkComp, string From, string To, string i_name, string detail, string category, string chkCat, string plantCode)
    {
        string pdfReport = Request.QueryString[10].ToString();
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        //Hiten Stock Query Added 

        //Query = "select ITEM_MASTER.I_CODENO,ISNULL( STL.STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,(STL.STL_DOC_DATE) AS STL_DOC_DATE,STL.STL_CODE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS numeric(10, 3)), 0) AS REM_QTY,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD' WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD' WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.' WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'Stock Transfer' THEN 'STOCK TRANSFER' WHEN 'STCADJ1' THEN 'STOCK ERROR CORRECTION' ELSE STL_DOC_TYPE END) AS STL_DOC_TYPE,ITEM_MASTER.I_CODE AS STL_I_CODE,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)) AS numeric(10, 3)), 0) AS OpenStoc FROM STOCK_LEDGER AS STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE WHERE  ITEM_MASTER.ES_DELETE=0 AND (ITEM_MASTER.I_ACTIVE_IND = 1)";
        // new Query after plant wise
        //Query = "select ITEM_MASTER.I_CODENO,ISNULL( STL.STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,(STL.STL_DOC_DATE) AS STL_DOC_DATE,STL.STL_CODE,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AND SOL.STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "') ) AS numeric(10, 3)), 0) AS ADD_QTY,ISNULL(CAST((SELECT ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_I_CODE = ITEM_MASTER.I_CODE) AND SOL.STL_CODE=STL.STL_CODE AND (STL_DOC_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')   AND SOL.STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')) AS numeric(10, 3)), 0) AS REM_QTY,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD' WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD' WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.' WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'Stock Transfer' THEN 'STOCK TRANSFER' WHEN 'STCADJ1' THEN 'STOCK ERROR CORRECTION' ELSE STL_DOC_TYPE END) AS STL_DOC_TYPE,ITEM_MASTER.I_CODE AS STL_I_CODE,ITEM_MASTER.I_NAME,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (STL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (STL_I_CODE = ITEM_MASTER.I_CODE)   AND SOL.STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')) AS numeric(10, 3)), 0) AS OpenStoc FROM STOCK_LEDGER AS STL RIGHT OUTER JOIN ITEM_MASTER ON STL.STL_I_CODE = ITEM_MASTER.I_CODE WHERE  ITEM_MASTER.ES_DELETE=0 AND (ITEM_MASTER.I_ACTIVE_IND = 1)   AND STL.STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')";


        //optimized Query 
        //Query = "SELECT STL_CODE,ISNULL(ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)),0) AS ADD_QTY , ISNULL(ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)),0) AS REM_QTY,STL_I_CODE ,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD' WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD' WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.' WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'Stock Transfer' THEN 'STOCK TRANSFER' WHEN 'STCADJ1' THEN 'STOCK ERROR CORRECTION' ELSE STL_DOC_TYPE END) AS STL_DOC_TYPE ,ISNULL( STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,(STL_DOC_DATE) AS STL_DOC_DATE,ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (SOL.STL_DOC_DATE <  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' ) AND (SOL.STL_I_CODE = STOCK_LEDGER.STL_I_CODE)   AND SOL.STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')) AS numeric(10, 3)), 0) AS OpenStoc   into #temp    FROM STOCK_LEDGER WHERE     (STL_DOC_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  and  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AND STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')  GROUP BY STL_CODE, STL_I_CODE,STL_DOC_TYPE,STL_DOC_NUMBER,STL_DOC_DATE   select  I_CODENO,	ISNULL(STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,	ISNULL(STL_DOC_DATE,0) AS STL_DOC_DATE,	ISNULL(STL_CODE,0) AS STL_CODE, ISNULL(ADD_QTY,0) AS  ADD_QTY, ISNULL(REM_QTY,0) AS 	REM_QTY,ISNULL(STL_DOC_TYPE,0) AS  	STL_DOC_TYPE,ISNULL(STL_I_CODE,0) AS 	STL_I_CODE,	I_NAME	, ISNULL(OpenStoc,0) AS OpenStoc  from #temp RIGHT OUTER JOIN ITEM_MASTER ON  STL_I_CODE = ITEM_MASTER.I_CODE WHERE  ITEM_MASTER.ES_DELETE=0 AND (ITEM_MASTER.I_ACTIVE_IND = 1)";

        Query = "SELECT STL_CODE,ISNULL(ABS(SUM(CASE WHEN STL_DOC_QTY > 0 THEN STL_DOC_QTY END)),0) AS ADD_QTY , ISNULL(ABS(SUM(CASE WHEN STL_DOC_QTY < 0 THEN STL_DOC_QTY END)),0) AS REM_QTY,STL_I_CODE ,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD' WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE'  WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD' WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE'  WHEN 'IWIC' THEN 'CUSTOMER REJ.' WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'Stock Transfer' THEN 'STOCK TRANSFER' WHEN 'STCADJ1' THEN 'STOCK ERROR CORRECTION' ELSE STL_DOC_TYPE END) AS STL_DOC_TYPE ,ISNULL( STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,(STL_DOC_DATE) AS STL_DOC_DATE   into #temp    FROM STOCK_LEDGER WHERE     (STL_DOC_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  and  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AND STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')  GROUP BY STL_CODE, STL_I_CODE,STL_DOC_TYPE,STL_DOC_NUMBER,STL_DOC_DATE   select  I_CODENO,	ISNULL(STL_DOC_NUMBER,0) AS STL_DOC_NUMBER,	ISNULL(STL_DOC_DATE,0) AS STL_DOC_DATE,	ISNULL(STL_CODE,0) AS STL_CODE, ISNULL(ADD_QTY,0) AS  ADD_QTY, ISNULL(REM_QTY,0) AS 	REM_QTY,ISNULL(STL_DOC_TYPE,0) AS  	STL_DOC_TYPE,ISNULL(STL_I_CODE,0) AS 	STL_I_CODE,	I_NAME	, ISNULL(CAST((SELECT SUM(STL_DOC_QTY) AS Expr1 FROM STOCK_LEDGER AS SOL WHERE (SOL.STL_DOC_DATE <  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' ) AND (SOL.STL_I_CODE = I_CODE)   AND SOL.STL_STORE_TYPE IN (SELECT STORE_CODE FROM STORE_MASTER where STORE_PLANT='" + plantCode + "')) AS numeric(10, 3)), 0)  AS OpenStoc  from #temp RIGHT OUTER JOIN ITEM_MASTER ON  STL_I_CODE = ITEM_MASTER.I_CODE WHERE  ITEM_MASTER.ES_DELETE=0 AND (ITEM_MASTER.I_ACTIVE_IND = 1)";


        if (chkCat != "All")
        {
            Query = Query + " AND I_CAT_CODE='" + category + "'";
        }


        if (i_name.Trim() != "")
        {
            Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
        }

        //Query = Query + "GROUP BY I_CODENO, STL_DOC_NUMBER ,STL_DOC_DATE,I_CODE, STL_CODE, STL_DOC_TYPE, STL_I_CODE, I_NAME,(CASE STL_DOC_TYPE WHEN 'IWIM' THEN 'MATERIAL INWARD' WHEN 'ISSPROD' THEN 'ISSUE TO PRODUCTION' WHEN 'PRODTOSTORE' THEN 'PRODUCTION TO STORE' WHEN 'EXPINV' THEN 'EXPORT INVOICE' WHEN 'TAXINV' THEN 'TAX INVOICE' WHEN 'STCADJ' THEN 'STOCK ADJUSTMENT' WHEN 'OutSUBINM' THEN 'ISSUE TO SUBCONTRACTOR' WHEN 'IWIAP' THEN 'SUBCONTRACTOR INWARD' WHEN 'DCTOUT' THEN 'TRAY DELIVARY CHALLAN ISSUE' WHEN 'DCIN' THEN 'DELIVARY CHALLAN INWARD' WHEN 'OutJWINM' THEN 'LABOUR CHARGE INVOICE' WHEN 'IWIC' THEN 'CUSTOMER REJ.' WHEN 'IWIFP' THEN 'FOR PROCESS INWARD' WHEN 'DCOUT' THEN 'DELIVARY CHALLAN ISSUE'  WHEN 'DCTIN' THEN 'TRAY DELIVARY CHALLAN INWARD' WHEN 'SubContractorRejection' THEN 'SUBCONTRACTOR REJECTION' WHEN 'Stock Transfer' THEN 'STOCK TRANSFER' WHEN 'STCADJ1' THEN 'STOCK ERROR CORRECTION' ELSE STL_DOC_TYPE END)  ";

        //Query = Query + "GROUP BY I_CODENO, STL_DOC_NUMBER ,STL_DOC_DATE,I_CODE, STL_CODE, STL_DOC_TYPE, STL_I_CODE, I_NAME  ";

        Query = Query + " drop table #temp";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptStockLedger.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptStockLedger.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            if (plantCode == "-2147483648")
            {
                rptname.SetParameterValue("txtPeriod", "Plant 1 From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            }
            else
            {
                rptname.SetParameterValue("txtPeriod", "Plant 2 From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            }
            CrystalReportViewer1.ReportSource = rptname;

            if (pdfReport.ToUpper() == "TRUE")
            {


                DateTime now = DateTime.Now;
                string invoiceName = Server.MapPath("~/UpLoadPath/STOCK/" + now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second + now.Millisecond + "STOCK.pdf");
                rptname.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, invoiceName);

                string path = invoiceName;
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(path);
                if (buffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", buffer.Length.ToString());
                    Response.BinaryWrite(buffer);
                }


                try
                {
                    //SendEmail(dsTaxInvoiceGST.Tables[0].Rows[0]["P_EMAIL"].ToString(), invoiceName, dsTaxInvoiceGST.Tables[0].Rows[0]["P_NAME"].ToString(), dtComp.Rows[0]["CM_NAME"].ToString(), invoicenumber, dtComp.Rows[0]["CM_BANK_NAME"].ToString(), dtComp.Rows[0]["CM_NAME"].ToString(), dtComp.Rows[0]["CM_BANK_ACC_NO"].ToString(), dtComp.Rows[0]["CM_IFSC_CODE"].ToString(), dtComp.Rows[0]["CM_B_SWIFT_CODE"].ToString(), dtComp.Rows[0]["CM_ACC_TYPE"].ToString());

                }
                catch (Exception ex)
                {


                }
            }           

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
            Response.Redirect("~/RoportForms/VIEW/ViewStockLedger.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
