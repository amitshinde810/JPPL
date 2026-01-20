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

public partial class RoportForms_ADD_SalesPendingOrderStatus : System.Web.UI.Page
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

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }

            string i_name = Request.QueryString[5].ToString();
            if (ChkAll == true && ChkComp == true)
            {
                GenerateReport(Title, "All", "All", From, To, i_name);
            }
            if (ChkAll != true && ChkComp == true)
            {
                GenerateReport(Title, "ONE", "All", From, To, i_name);
            }
            if (ChkAll == true && ChkComp != true)
            {
                GenerateReport(Title, "All", "ONE", From, To, i_name);
            }
            if (ChkAll != true && ChkComp != true)
            {
                GenerateReport(Title, "ONE", "ONE", From, To, i_name);
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string chkdate, string chkComp, string From, string To, string i_name)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        //Query = "SELECT Distinct CPOM_DOC_NO,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ITEM_UNIT_MASTER.I_UOM_NAME,CPOM_PONO,CPOM_DATE,CPOD_ORD_QTY,CPOD_RATE,CPOD_DISPACH,CPOM_PO_DATE FROM PARTY_MASTER ,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER ,ITEM_UNIT_MASTER WHERE CPOM_CODE = CPOD_CPOM_CODE AND  P_CODE = CPOM_P_CODE AND CPOD_I_CODE = I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=CUSTPO_DETAIL.CPOD_UOM_CODE and CUSTPO_MASTER.ES_DELETE=0 ";
        Query = "SELECT Distinct P_NAME,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CPOM_PONO,CPOM_DATE,CPOD_ORD_QTY,CPOD_DISPACH,CPOM_PO_DATE FROM PARTY_MASTER ,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER  WHERE CPOM_CODE = CPOD_CPOM_CODE AND  P_CODE = CPOM_P_CODE AND CPOD_I_CODE = I_CODE and CUSTPO_MASTER.ES_DELETE=0 ";
        if (chkdate == "All" && chkComp == "All")
        {
            Query = Query;
        }
        if (chkdate != "All" && chkComp == "All")
        {
            Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'";
        }
        if (chkdate == "All" && chkComp != "All")
        {
            Query = Query + " and CPOM_P_CODE='" + i_name + "' ";
        }
        if (chkdate != "All" && chkComp != "All")
        {
            Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' and CPOM_P_CODE='" + i_name + "' ";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptSalesOrderPendingStatus.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptSalesOrderPendingStatus.rpt");
            rptname.Refresh();
            //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER where ISO_SCREEN_NO=90 and ISO_WEF_DATE<='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' order by ISO_WEF_DATE DESC");
            string CustName = "";

             CustName = DL_DBAccess.GetColumn("select P_NAME from PARTY_MASTER where P_CODE='" + i_name + "'");
            if (IsoNo == "")
            {
                rptname.SetParameterValue("txtIsoNo", "1");
            }
            else
            {
                rptname.SetParameterValue("txtIsoNo", IsoNo);
            }
            if (CustName == "")
            {
                rptname.SetParameterValue("txtCustName", "1");
            }
            else
            {
                rptname.SetParameterValue("txtCustName", CustName);
            }
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
            CommonClasses.SendError("Sales Order Rport", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSalesOrderReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}