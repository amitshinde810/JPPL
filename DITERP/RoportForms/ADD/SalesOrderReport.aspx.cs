using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_SalesOrderReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";
    }

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
            string cond = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, group, way, cond);

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "SELECT ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, SO_TYPE_MASTER.SO_T_SHORT_NAME, CUSTPO_DETAIL.CPOD_ORD_QTY, CUSTPO_DETAIL.CPOD_RATE,  CUSTPO_DETAIL.CPOD_AMT, CUSTPO_DETAIL.CPOD_CUST_I_NAME, CUSTPO_DETAIL.CPOD_CUST_I_CODE, CUSTPO_DETAIL.CPOD_AMORTRATE,CUSTPO_DETAIL.CPOD_DISPACH,ITEM_UNIT_MASTER.I_UOM_NAME,CUSTPO_MASTER.CPOM_PONO,CUSTPO_MASTER.CPOM_DOC_NO,CUSTPO_MASTER.CPOM_DATE, CUSTPO_MASTER.CPOM_BASIC_AMT, PROJECT_CODE_MASTER.PROCM_NAME, PARTY_MASTER.P_NAME FROM PARTY_MASTER INNER JOIN SO_TYPE_MASTER INNER JOIN CUSTPO_MASTER ON SO_TYPE_MASTER.SO_T_CODE = CUSTPO_MASTER.CPOM_TYPE INNER JOIN PROJECT_CODE_MASTER ON CUSTPO_MASTER.CPOM_PROJECT_CODE = PROJECT_CODE_MASTER.PROCM_CODE ON  PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER INNER JOIN CUSTPO_DETAIL ON ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON  CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE WHERE " + cond + " (ITEM_UNIT_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) AND (PROJECT_CODE_MASTER.ES_DELETE = 0) AND (SO_TYPE_MASTER.ES_DELETE = 0) and (PARTY_MASTER.ES_DELETE=0)";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/RptSalesOrderRptNewDatewise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/RptSalesOrderRptNewDatewise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "All")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("title", "Customer PO Report Datewise " + "(" + way + ") From :" + From + " To " + To);
                   // rptname.SetParameterValue("txtDate", "Date From : " + From + "To " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/RptSalesOrderNewItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/RptSalesOrderNewItemWise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "All")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("title", "Customer PO Report Itemwise " + "(" + way + ") From :" + From + " To " + To);
                   // rptname.SetParameterValue("txtDate", "Date From : " + From + "To " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/RptSalesOrderNewCustWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/RptSalesOrderNewCustWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "All")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("title", "Customer PO Report Customerwise " + "(" + way + ") From :" + From + " To " + To);
                   // rptname.SetParameterValue("txtDate", "Date From : " + From + "To " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                }
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