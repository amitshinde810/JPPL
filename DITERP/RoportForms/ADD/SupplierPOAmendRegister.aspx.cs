using System;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;


public partial class RoportForms_ADD_SupplierPOAmendRegister : System.Web.UI.Page
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
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();

            string Cond = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, group, way, Cond);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            Query = "SELECT MAX(SPOM_AM_AM_COUNT) AS AMD_NO,I_CODENO,I_NAME,P_NAME,P_CODE, SPOM_AM_DATE, SPOM_PONO,SPOD_AMD_SPOM_AM_CODE,SPOD_AMD_I_CODE into #po FROM SUPP_PO_AM_MASTER,SUPP_PO_AMD_DETAILS,ITEM_MASTER,PARTY_MASTER  where AM_CODE=AMD_AM_CODE AND SPOD_AMD_I_CODE=I_CODE AND P_CODE=SPOM_AM_P_CODE  GROUP BY    SPOM_PONO,SPOD_AMD_SPOM_AM_CODE,SPOD_AMD_I_CODE,I_CODENO,I_NAME,P_NAME,P_CODE,SPOM_AM_DATE  SELECT  #po.*,SPOM_AM_AM_DATE,SPOD_AMD_ORDER_QTY,SPOD_AMD_RATE ,SPOD_ORDER_QTY,SPOD_RATE FROM #po ,SUPP_PO_AM_MASTER,SUPP_PO_AMD_DETAILS,SUPP_PO_MASTER,SUPP_PO_DETAILS  where AM_CODE=AMD_AM_CODE AND SUPP_PO_AMD_DETAILS.SPOD_AMD_SPOM_AM_CODE=#po.SPOD_AMD_SPOM_AM_CODE AND P_CODE=SPOM_AM_P_CODE  AND SUPP_PO_AMD_DETAILS.SPOD_AMD_I_CODE =#po.SPOD_AMD_I_CODE AND SPOM_AM_AM_COUNT=#PO.AMD_NO    AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_AM_MASTER.SPOM_AM_CODE=SPOM_CODE  AND SPOD_I_CODE=SUPP_PO_AMD_DETAILS.SPOD_AMD_I_CODE  AND SPOM_P_CODE=SPOM_AM_P_CODE " + Cond + " DROP TABLE #PO";

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
                    rptname.Load(Server.MapPath("~/Reports/rptPOAmendDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptPOAmendDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Pending")
                    {
                        rptname.SetParameterValue("txtType", group + " PO Pending Report");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", group + " PO All Report ");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }

                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptPOAmendSuppWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptPOAmendSuppWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", " SupplierWIse PO Pending Report");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "SupplierWIse PO All Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
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
        catch (Exception Ex)
        {
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSupplierPoAmendRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Amendment Register", "btnCancel_Click", Ex.Message);
        }
    }
}
