using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class Masters_ADD_DeliveryPerformanceReport : System.Web.UI.Page
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
            string Cond = Request.QueryString[3].ToString();

            GenerateReport(Title, From, To, Cond);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            Query = "SELECT PURCHASE_SCHEDULE_MASTER.PSM_CODE, PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO, PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_DATE,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_MONTH, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY, 0) + ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY, 0)+ ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY, 0) + ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY, 0) AS PSD_TOTSCHEDULE_QTY,SUPP_PO_MASTER.SPOM_PONO as SPOM_PO_NO, ISNULL(INWARD_DETAIL.IWD_REV_QTY,0) as IWD_REV_QTY FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE RIGHT OUTER JOIN PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE INNER JOIN PARTY_MASTER ON PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN SUPP_PO_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN SUPP_PO_DETAILS ON SUPP_PO_MASTER.SPOM_CODE = SUPP_PO_DETAILS.SPOD_SPOM_CODE ON  INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE WHERE " + Cond + " (PARTY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.P_INHOUSE_IND = 1) AND (PARTY_MASTER.P_TYPE = 2) AND (ITEM_MASTER.I_ACTIVE_IND = 1) AND (PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID = '" + Session["CompanyId"] + "') AND (SUPP_PO_MASTER.ES_DELETE = 0)ORDER BY PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptDeliveryPerformance.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptDeliveryPerformance.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", "" + Title + " From " + From + " To " + To);
                CrystalReportViewer1.ReportSource = rptname;
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
            Response.Redirect("~/RoportForms/VIEW/ViewDeliveryPerformanceReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Amendment Register", "btnCancel_Click", Ex.Message);
        }
    }
}

