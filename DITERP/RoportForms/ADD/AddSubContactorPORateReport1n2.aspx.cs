using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_AddSubContactorPORateReport1n2 : System.Web.UI.Page
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
        string Title = Request.QueryString[0];
        string From = Request.QueryString[1].ToString();
        string To = Request.QueryString[2].ToString();
        string cond = Request.QueryString[3].ToString();

        GenerateReport(Title, From, To, cond);
    }
    #endregion

    private void GenerateReport(string Title, string From, string To, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            //Query = "SELECT DISTINCT SUPP_PO_MASTER.SPOM_CODE,PARTY_MASTER.P_NAME,ITEM_CATEGORY_MASTER.I_CAT_NAME,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO, SUPP_PO_MASTER.SPOM_PO_NO, SUPP_PO_MASTER.SPOM_PONO,SUPP_PO_DETAILS.SPOD_ORDER_QTY,SUPP_PO_DETAILS.SPOD_RATE FROM PARTY_MASTER INNER JOIN SUPP_PO_MASTER INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE INNER JOIN ITEM_MASTER INNER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE ON SUPP_PO_DETAILS.SPOD_I_CODE = ITEM_MASTER.I_CODE ON PARTY_MASTER.P_CODE = SUPP_PO_MASTER.SPOM_P_CODE where " + cond + " SUPP_PO_MASTER.ES_DELETE = 0 AND SUPP_PO_MASTER.SPOM_CM_COMP_ID = " + Session["CompanyId"] + " AND ITEM_CATEGORY_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND SPOD_RATE in(1,2) ORDER BY P_NAME";
            /*Add Inward Qty:- 13082018 SPOD_INW_QTY from SUPP_PO_DETAILS table*/
            Query = "SELECT DISTINCT SUPP_PO_MASTER.SPOM_CODE,PARTY_MASTER.P_NAME,ITEM_CATEGORY_MASTER.I_CAT_NAME,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO, SUPP_PO_MASTER.SPOM_PO_NO, SUPP_PO_MASTER.SPOM_PONO,SUPP_PO_DETAILS.SPOD_ORDER_QTY,SUPP_PO_DETAILS.SPOD_RATE,SPOD_INW_QTY FROM PARTY_MASTER INNER JOIN SUPP_PO_MASTER INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE INNER JOIN ITEM_MASTER INNER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE ON SUPP_PO_DETAILS.SPOD_I_CODE = ITEM_MASTER.I_CODE ON PARTY_MASTER.P_CODE = SUPP_PO_MASTER.SPOM_P_CODE where " + cond + " SUPP_PO_MASTER.ES_DELETE = 0 AND SUPP_PO_MASTER.SPOM_CM_COMP_ID = " + Session["CompanyId"] + " AND ITEM_CATEGORY_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND SPOD_RATE in(1,2) ORDER BY P_NAME";

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/SubContractorPoRate1n2Report.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/SubContractorPoRate1n2Report.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                if (cond == "")
                {
                    rptname.SetParameterValue("txtTitle", "Sub Contractor PO Rate Report from " + From + " To " + To + "");
                }
                else
                {
                    if (From == "")
                    {
                        rptname.SetParameterValue("txtTitle", "");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtTitle", "Sub Contractor PO Rate Report From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "");
                    }
                }
                CrystalReportViewer1.ReportSource = rptname;
                CrystalReportViewer1.ID = Title.Replace(" ", "_");
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RoportForms/VIEW/ViewSubContactorPORateReport1n2.aspx", false);
    }
}

