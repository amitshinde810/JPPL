using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_CoreInventoryRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Date = Request.QueryString[1].ToString();
            //string GroupCode = Request.QueryString[2].ToString();
            string StrCond = Request.QueryString[2].ToString();
            GenerateReport(Title, Date, StrCond);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Date, string StrCond)
    {
        try
        {
            //dsCastingtobeInspected dataset is used
            DL_DBAccess = new DatabaseAccessLayer();
            DateTime dtMonth = new DateTime();
            dtMonth = Convert.ToDateTime(Date);
            string Query = "";
            // Hardcode check = STL_STORE_TYPE from STOCK_LEDGER FOR Actual Stock(SUM(STL_DOC_QTY) for Foundry Store
            Query = "SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,sum(ISNULL(CS_SCHEDULE_QTY,0)) AS CS_SCHEDULE_QTY,sum(ISNULL(SIM_FOUNDRY,0)) AS Tot_Stnd_Stock ,ISNULL((select SUM(ISNULL(STL_DOC_QTY,0)) AS STL_DOC_QTY from STOCK_LEDGER WHERE STL_STORE_TYPE = '-2147483646' AND STL_I_CODE= I.I_CODE),0) AS ActualStockQty ,PROD_RAW_TYPE FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON SIM_I_CODE=C.CS_I_CODE INNER JOIN PRODUCT_MASTER PROD ON PROD.PROD_I_CODE=I.I_CODE WHERE " + StrCond + " S.ES_DELETE=0 AND I.ES_DELETE=0 AND PROD.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' group by  I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO , I_NAME ,PROD_RAW_TYPE  ORDER BY I_CODENO, I_NAME";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/CastingToBeCast.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/CastingToBeCast.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Title", Title);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtDate", Date);
                DataTable dtLossPer = CommonClasses.Execute("SELECT ISNULL((CAST(RM_CASTING_REJECTION AS FLOAT)/100)+1,0) AS RM_CASTING_REJECTION FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_DATE=(SELECT MAX(RM_DATE) FROM REJECTION_MASTER WHERE ES_DELETE=0 AND RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "')");
                double LossPer = 0; // 1.005
                if (dtLossPer.Rows.Count == 0)
                { }
                else
                { LossPer = Convert.ToDouble(dtLossPer.Rows[0]["RM_CASTING_REJECTION"].ToString()); }

                rptname.SetParameterValue("txtLossPer", LossPer);
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
            CommonClasses.SendError("Casting ToBe Cast Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/PPC/ReportForms/VIEW/ViewCastingtobeCast.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}