using System;
using System.Web.UI;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_ItemSaleMonthWise : System.Web.UI.Page
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
                GenerateReport(Title, "All", "ONE", From, To, i_name);
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

        Query = "SELECT DATENAME(MONTH, INM_DATE) + '/' + DATENAME(YEAR, INM_DATE) as INM_DATE,I_CODENO,I_NAME+'('+ I_CODENO +')' as I_NAME,SUM(IND_INQTY) as IND_INQTY,sum(IND_AMT) as IND_AMT FROM INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER WHERE INM_CODE=IND_INM_CODE AND IND_I_CODE=I_CODE AND INVOICE_MASTER.ES_DELETE=0 AND I_CAT_CODE=-2147483648 AND INM_INVOICE_TYPE=0 AND INM_TYPE='TAXINV' AND INM_IS_SUPPLIMENT=0 ";

        string FMonthName = Convert.ToDateTime(From).ToString("MMMM");
        string Fyear = Convert.ToDateTime(From).Year.ToString();
        From = FMonthName + '/' + Fyear;

        string TMonthName = Convert.ToDateTime(To).ToString("MMMM");
        string Tyear = Convert.ToDateTime(To).Year.ToString();
        To = TMonthName + '/' + Tyear;

        if (chkdate != "All")
        {
            Query = Query + " and DATENAME(MONTH, INM_DATE) + '/' + DATENAME(YEAR, INM_DATE) between '" + From + "' and '" + To + "'";
        }

        if (chkComp != "All")
        {
            Query = Query + " and ITEM_MASTER.I_CODE='" + i_name + "' ";
        }

        Query = Query + "group by DATENAME(MONTH, INM_DATE) + '/' + DATENAME(YEAR, INM_DATE),I_NAME,I_CODENO order by  I_NAME";

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptItemSaleMonthWise.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptItemSaleMonthWise.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            rptname.SetParameterValue("txtPeriod", "Item Sale Month Wise Report From " + From + " To " + To);
            CrystalReportViewer1.ReportSource = rptname;
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
            CommonClasses.SendError("Customer Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewItemSaleMonthWise.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ItemSaleMonthWise", "btnCancel_Click", Ex.Message);
        }
    }
}
