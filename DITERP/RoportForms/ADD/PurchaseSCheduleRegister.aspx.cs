using System;
using System.Data;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_PurchaseSCheduleRegister : System.Web.UI.Page
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
            string Type = Request.QueryString[6].ToString();
            GenerateReport(Title, From, To, group, way, Cond, Type);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string condition, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "SELECT DISTINCT PURCHASE_SCHEDULE_MASTER.PSM_CODE,IWD_REV_QTY,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_DATE,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_MONTH,ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY,0.00) AS PSD_W1_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY,0.00) AS PSD_W2_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY, 0.00) AS PSD_W3_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY, 0.00) AS PSD_W4_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_MONTH_QTY ,0.00) AS PSD_MONTH_QTY,PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME, SUPP_PO_MASTER.SPOM_PO_NO, ITEM_UNIT_MASTER.I_UOM_NAME FROM SUPP_PO_DETAILS INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE INNER JOIN PARTY_MASTER ON PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE = ITEM_MASTER.I_CODE ON SUPP_PO_MASTER.SPOM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE  INNER JOIN  INWARD_DETAIL ON INWARD_DETAIL.IWD_IWM_CODE=PURCHASE_SCHEDULE_MASTER.PSM_CODE  WHERE " + condition + " (PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID = 1) AND (PARTY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.SPOM_CM_CODE =" + Session["CompanyCode"] + ") AND (ITEM_UNIT_MASTER.ES_DELETE=0) ORDER BY PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO";
            // Query = "SELECT DISTINCT PURCHASE_SCHEDULE_MASTER.PSM_CODE,ITEM_MASTER.I_CODE,ISNULL((SELECT SUM(IWD_REV_QTY) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND  IWM_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND IWD_I_CODE=ITEM_MASTER.I_CODE and IWM_P_CODE=PARTY_MASTER.P_CODE  ),0) AS IWD_REV_QTY,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_DATE,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_MONTH,ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY,0.00) AS PSD_W1_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY,0.00) AS PSD_W2_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY, 0.00) AS PSD_W3_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY, 0.00) AS PSD_W4_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_MONTH_QTY ,0.00) AS PSD_MONTH_QTY,PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME, SUPP_PO_MASTER.SPOM_PO_NO, ITEM_UNIT_MASTER.I_UOM_NAME FROM SUPP_PO_DETAILS INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE INNER JOIN PARTY_MASTER ON PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE = ITEM_MASTER.I_CODE ON SUPP_PO_MASTER.SPOM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE   WHERE " + condition + " (PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID = 1) AND (PARTY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.SPOM_CM_CODE =" + Session["CompanyCode"] + ") AND (ITEM_UNIT_MASTER.ES_DELETE=0) ORDER BY PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO";
            string type = "";
            if (Type == "0")
                type = type + " and SPOM_POTYPE=0  "; //-2147483646 for Both and -2147483648 - Supplier
            else
                type = type + " and SPOM_POTYPE=1 "; //-2147483646 for Both and -2147483647  - Subcontractor

            Query = "SELECT DISTINCT PURCHASE_SCHEDULE_MASTER.PSM_CODE,ITEM_MASTER.I_CODE,ISNULL((SELECT SUM(IWD_REV_QTY) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND  IWM_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND IWD_I_CODE=ITEM_MASTER.I_CODE and IWM_P_CODE=PARTY_MASTER.P_CODE  ),0) AS IWD_REV_QTY   ,ISNULL(( SELECT SUM(IND_INQTY) FROM INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND IND_I_CODE=ITEM_MASTER.I_CODE and INM_P_CODE=PARTY_MASTER.P_CODE  ),0) AS IND_INQTY ,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_DATE,PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_MONTH,ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY,0.00) AS PSD_W1_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY,0.00) AS PSD_W2_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY, 0.00) AS PSD_W3_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY, 0.00) AS PSD_W4_QTY, ISNULL(PURCHASE_SCHEDULE_DETAIL.PSD_MONTH_QTY ,0.00) AS PSD_MONTH_QTY,PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME, SUPP_PO_MASTER.SPOM_PO_NO, ITEM_UNIT_MASTER.I_UOM_NAME FROM SUPP_PO_DETAILS INNER JOIN SUPP_PO_MASTER ON SUPP_PO_DETAILS.SPOD_SPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE INNER JOIN PARTY_MASTER ON PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON PURCHASE_SCHEDULE_DETAIL.PSD_I_CODE = ITEM_MASTER.I_CODE ON SUPP_PO_MASTER.SPOM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PO_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE   WHERE " + condition + " (PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID = 1) AND (PARTY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.SPOM_CM_CODE =" + Session["CompanyCode"] + ") AND (ITEM_UNIT_MASTER.ES_DELETE=0) " + type + " ORDER BY PURCHASE_SCHEDULE_MASTER.PSM_SCHEDULE_NO";

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
                    rptname.Load(Server.MapPath("~/Reports/PurchaseScheduleRegister.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/PurchaseScheduleRegister.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", "  Purchase Schedule Register Date Wise For the Month of " + Convert.ToDateTime(From).ToString("MMM/yyyy") + " ");
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/ItemwisePurScheduleReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/ItemwisePurScheduleReg.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    //  rptname.SetParameterValue("txtTitle", Title);
                    rptname.SetParameterValue("txtDate", " Purchase Schedule Register Item Wise For the Month of " + Convert.ToDateTime(From).ToString("MMM/yyyy") + " ");
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/SupplierwisePurScheduleReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/SupplierwisePurScheduleReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " Purchase Schedule Register Supplier Wise For the Month of " + Convert.ToDateTime(From).ToString("MMM/yyyy") + "");
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
            Response.Redirect("~/RoportForms/VIEW/ViewPurchaseScheduleRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Register", "btnCancel_Click", Ex.Message);
        }
    }
}

