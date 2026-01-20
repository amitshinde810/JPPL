using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class Transactions_ADD_RejectionToFoundryConReg : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            string i_name = Request.QueryString[4].ToString();
            string StrCond = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, StrCond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Store To Foundry Conversion Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string strcond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "  SELECT distinct REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_NO, convert(varchar,REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_DATE,106) as RTF_DOC_DATE,REJECTION_TO_FOUNDRY_DETAIL.RTFD_REJ_ITEMCODE, REJECTION_TO_FOUNDRY_MASTER.RTF_CQTY,REJECTION_TO_FOUNDRY_DETAIL.RTFD_CON_ITEMCODE, ISNULL(REJECTION_TO_FOUNDRY_DETAIL.RTFD_STK_REJ_QTY, 0) AS RTFD_STK_REJ_QTY,ROUND(ITEM_MASTER.I_UWEIGHT * ISNULL(SUM(REJECTION_TO_FOUNDRY_DETAIL.RTFD_STK_REJ_QTY), 0), 2) AS TOTALWEIGHT,ISNULL(REJECTION_TO_FOUNDRY_DETAIL.RTFD_STAND_WEIGHT, 0) AS RTFD_STAND_WEIGHT, ISNULL(REJECTION_TO_FOUNDRY_DETAIL.RTFD_CON_QTY, 0) AS RTFD_CON_QTY, ISNULL(REJECTION_TO_FOUNDRY_MASTER.RTF_QTY, 0) AS RTF_QTY,ITEM_MASTER_1.I_CODENO, ITEM_MASTER_1.I_NAME, ITEM_MASTER.I_CODENO AS I_CODENO_DETAIL, ITEM_MASTER.I_NAME AS I_NAME_DETAIL,ITEM_UNIT_MASTER.I_UOM_NAME, ITEM_UNIT_MASTER_1.I_UOM_NAME AS I_Unit_Name_Detail,STOCK_LEDGER.STL_DOC_QTY,ITEM_MASTER.I_UWEIGHT FROM REJECTION_TO_FOUNDRY_MASTER INNER JOIN REJECTION_TO_FOUNDRY_DETAIL ON REJECTION_TO_FOUNDRY_MASTER.RTF_CODE = REJECTION_TO_FOUNDRY_DETAIL.RTFD_RTF_CODE INNER JOIN ITEM_MASTER ON REJECTION_TO_FOUNDRY_DETAIL.RTFD_REJ_ITEMCODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_MASTER AS ITEM_MASTER_1 ON REJECTION_TO_FOUNDRY_MASTER.RTF_I_CODE = ITEM_MASTER_1.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER_1.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN ITEM_UNIT_MASTER AS ITEM_UNIT_MASTER_1 ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER_1.I_UOM_CODE INNER JOIN STOCK_LEDGER ON ITEM_MASTER.I_CODE = STOCK_LEDGER.STL_I_CODE    WHERE " + strcond + " STL_STORE_TYPE =-2147483641  and (REJECTION_TO_FOUNDRY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_UNIT_MASTER_1.ES_DELETE = 0)  and (REJECTION_TO_FOUNDRY_MASTER.RTF_CM_CODE = " + Session["CompanyCode"] + ") AND (ITEM_UNIT_MASTER.ES_DELETE = 0) AND (ITEM_MASTER_1.ES_DELETE = 0) and STL_DOC_NO=RTF_CODE GROUP BY REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_NO, REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_DATE,REJECTION_TO_FOUNDRY_DETAIL.RTFD_REJ_ITEMCODE, REJECTION_TO_FOUNDRY_MASTER.RTF_CQTY,REJECTION_TO_FOUNDRY_DETAIL.RTFD_CON_ITEMCODE, RTFD_STK_REJ_QTY,RTFD_STAND_WEIGHT,RTFD_CON_QTY,RTF_QTY,ITEM_MASTER_1.I_CODENO, ITEM_MASTER_1.I_NAME,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,ITEM_UNIT_MASTER.I_UOM_NAME,ITEM_UNIT_MASTER_1.I_UOM_NAME, STOCK_LEDGER.STL_DOC_QTY,ITEM_MASTER.I_UWEIGHT";

            #region MyRegion

            #endregion
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/RptCastingConversionReg.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/RptCastingConversionReg.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "Casting Conversion Register From " + From + " to " + To);
                CrystalReportViewer1.ReportSource = rptname;
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../../RoportForms/VIEW/RejectionToFoundryConReg.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
