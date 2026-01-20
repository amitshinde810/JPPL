using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_BillPassingRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();
            int p_name = Convert.ToInt32(Request.QueryString[5].ToString());
            string group = Request.QueryString[6].ToString();

            if (group == "Datewise")
            {
                if (ChkAllDate == true && chkAllCust == true)
                {
                    GenerateReport(Title, "All", "All", From, To, p_name, group);
                }
                if (ChkAllDate != true && chkAllCust != true)
                {
                    GenerateReport(Title, "ONE", "ONE", From, To, p_name, group);
                }
                if (ChkAllDate != true && chkAllCust == true)
                {
                    GenerateReport(Title, "ONE", "All", From, To, p_name, group);
                }
                if (ChkAllDate == true && chkAllCust != true)
                {
                    GenerateReport(Title, "All", "ONE", From, To, p_name, group);
                }
            }
            if (group == "CustWise")
            {
                if (ChkAllDate == true && chkAllCust == true)
                {
                    GenerateReport(Title, "All", "All", From, To, p_name, group);
                }
                if (ChkAllDate != true && chkAllCust != true)
                {
                    GenerateReport(Title, "ONE", "ONE", From, To, p_name, group);
                }
                if (ChkAllDate != true && chkAllCust == true)
                {
                    GenerateReport(Title, "ONE", "All", From, To, p_name, group);
                }
                if (ChkAllDate == true && chkAllCust != true)
                {
                    GenerateReport(Title, "All", "ONE", From, To, p_name, group);
                }
            }
        }
        catch (Exception Ex) { }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string date1, string cust, string From, string To, int p_name, string group)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "   SELECT BPM_INV_NO,BPM_INV_DATE,BPM_BASIC_AMT,BPM_EXCIES_AMT, BPM_ECESS_AMT,BPM_HECESS_AMT,BPM_TAX_AMT,BPM_DISCOUNT_AMT,BPM_DISC_AMT,BPM_FREIGHT,BPM_PACKING_AMT,BPM_OCTRO_AMT,BPM_G_AMT, P_NAME ,BPM_ADD_DUTY, cast(isnull(BPM_BASIC_AMT,0) as numeric(20,2)) as BPM_BASIC_AMT, cast(isnull(BPM_DISC_PER,0) as numeric(20,2)) as BPM_DISC_PER, cast(isnull(BPM_DISC_AMT,0) as numeric(20,2)) as BPM_DISC_AMT, cast(isnull(BPM_PACKING_AMT,0) as numeric(20,2)) as BPM_PACKING_AMT, cast(isnull(BPM_ACCESS_AMT,0) as numeric(20,2)) as BPM_ACCESS_AMT, cast(isnull(BPM_GEXCISE,0) as numeric(20,2)) as INM_BE_AMT, cast(isnull(BPM_GCESS,0) as numeric(20,2)) as BPM_GCESS, cast(isnull(BPM_GSHCESS,0) as numeric(20,2)) as BPM_GSHCESS, cast(isnull(BPM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT, cast(isnull(BPM_TAX_PER,0) as numeric(20,2)) as BPM_TAX_PER,cast(isnull(BPM_TAX_AMT,0) as numeric(20,2)) as BPM_TAX_AMT,cast(isnull(BPM_OTHER_AMT,0) as numeric(20,2)) as BPM_OTHER_AMT,cast(isnull(BPM_FREIGHT,0) as numeric(20,2)) as BPM_FREIGHT,cast(isnull(BPM_INSURRANCE,0) as numeric(20,2)) as BPM_INSURRANCE,cast(isnull(BPM_TRANSPORT,0) as numeric(20,2)) as BPM_TRANSPORT,cast(isnull(BPM_OCTRO_AMT,0) as numeric(20,2)) as BPM_OCTRO_AMT, cast(isnull(BPM_ROUND_OFF,0) as numeric(20,2)) as BPM_ROUND_OFF,cast(isnull(BPM_G_AMT,0) as numeric(20,2)) as BPM_G_AMT FROM BILL_PASSING_MASTER, PARTY_MASTER WHERE  BPM_P_CODE=P_CODE AND BPM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + "   AND BILL_PASSING_MASTER.ES_DELETE=0";

            #region Datewise
            if (group == "Datewise")
            {
                if (date1 == "All" && cust == "All")
                {
                    Query = Query;
                }
                if (date1 != "All" && cust != "All")
                {
                    Query = Query + " and BPM_INV_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_CODE='" + p_name + "' ";
                }
                if (date1 != "All" && cust == "All")
                {
                    Query = Query + " and BPM_INV_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                }
                if (date1 == "All" && cust != "All")
                {
                    Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";
                }
            }
            #endregion

            #region CustWise
            if (group == "CustWise")
            {
                if (date1 == "All" && cust == "All")
                {
                    Query = Query;
                }
                if (date1 != "All" && cust != "All")
                {
                    Query = Query + " and BPM_INV_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_CODE='" + p_name + "' ";
                }
                if (date1 != "All" && cust == "All")
                {
                    Query = Query + " and BPM_INV_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
                }
                if (date1 == "All" && cust != "All")
                {
                    Query = Query + " and PARTY_MASTER.P_CODE='" + p_name + "' ";
                }
            }
            #endregion
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);

            #region Count
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/Bill Passing Register DateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/Bill Passing Register DateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtReportTitle", group + " Bill Passing Report");
                    rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/Bill Passing Register SupplierWIse.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/Bill Passing Register SupplierWIse.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtReportTitle", "SupplierWise Bill Passing Report");
                    rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";
            }
            #endregion
        }
        catch (Exception Ex) { }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/View/ViewBillPassingRegister.aspx", false);
        }
        catch (Exception Ex)
        {
        }
    }
}
