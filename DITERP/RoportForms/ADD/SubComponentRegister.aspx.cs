using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_SubComponentRegister : System.Web.UI.Page
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
            bool ChkCodeNoAll = Convert.ToBoolean(Request.QueryString[3].ToString());

            if (ChkAll == true && ChkCodeNoAll == true)
            {
                GenerateReport(Title, "All", "All");
            }
            if (ChkAll == false && ChkCodeNoAll == true)
            {
                string Comp = Request.QueryString[2];
                GenerateReport(Title, Comp, "All");
            }
            if (ChkAll == true && ChkCodeNoAll == false)
            {
                string Comp1 = Request.QueryString[2];
                GenerateReport(Title, "All", Comp1);
            }
            if (ChkAll == false && ChkCodeNoAll == false)
            {
                string Comp = Request.QueryString[2];
                GenerateReport(Title, Comp, Comp);
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Comp, string Comp1)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        if (Comp == "All" && Comp1 == "All")
        {
            Query = "SELECT ITEM_CATEGORY_MASTER.I_CAT_NAME,(case when I_INV_CAT=0 then 'A' when I_INV_CAT=1 then 'B' else 'C' end)as I_INV_CAT,I_NAME,I_CODENO,ITEM_UNIT_MASTER.I_UOM_NAME,I_MATERIAL,I_SPECIFICATION,I_CURRENT_BAL AS I_OP_BAL,I_STORE_LOC,I_INV_RATE,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE LEFT OUTER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE WHERE  ITEM_MASTER.ES_DELETE=0 and ITEM_CATEGORY_MASTER.I_CAT_CODE='-2147483648'";
        }
        if (Comp != "All" && Comp1 == "All")
        {
            Query = "SELECT ITEM_CATEGORY_MASTER.I_CAT_NAME,(case when I_INV_CAT=0 then 'A' when I_INV_CAT=1 then 'B' else 'C' end)as I_INV_CAT,I_NAME,I_CODENO,ITEM_UNIT_MASTER.I_UOM_NAME,I_MATERIAL,I_SPECIFICATION,I_CURRENT_BAL AS I_OP_BAL,I_STORE_LOC,I_INV_RATE,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE LEFT OUTER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE WHERE  ITEM_MASTER.ES_DELETE=0  and ITEM_MASTER.I_CODE=" + Comp + " and ITEM_CATEGORY_MASTER.I_CAT_CODE='-2147483648'";
        }
        if (Comp == "All" && Comp1 != "All")
        {
            Query = "SELECT ITEM_CATEGORY_MASTER.I_CAT_NAME,(case when I_INV_CAT=0 then 'A' when I_INV_CAT=1 then 'B' else 'C' end)as I_INV_CAT,I_NAME,I_CODENO,ITEM_UNIT_MASTER.I_UOM_NAME,I_MATERIAL,I_SPECIFICATION,I_CURRENT_BAL AS I_OP_BAL,I_STORE_LOC,I_INV_RATE,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE LEFT OUTER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE WHERE ITEM_MASTER.ES_DELETE=0  and ITEM_MASTER.I_CODE=" + Comp1 + " and ITEM_CATEGORY_MASTER.I_CAT_CODE='-2147483648'";
        }
        if (Comp != "All" && Comp1 != "All")
        {
            Query = "SELECT ITEM_CATEGORY_MASTER.I_CAT_NAME,(case when I_INV_CAT=0 then 'A' when I_INV_CAT=1 then 'B' else 'C' end)as I_INV_CAT,I_NAME,I_CODENO,ITEM_UNIT_MASTER.I_UOM_NAME,I_MATERIAL,I_SPECIFICATION,I_CURRENT_BAL AS I_OP_BAL,I_STORE_LOC,I_INV_RATE,I_LENGTH,I_HEIGHT,I_WIDTH,I_ROWS,I_STORIED,I_MAX_LEVEL,I_MIN_LEVEL,I_REORDER_LEVEL FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE LEFT OUTER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE WHERE ITEM_MASTER.ES_DELETE=0  and ITEM_MASTER.I_CODE=" + Comp + " and ITEM_CATEGORY_MASTER.I_CAT_CODE='-2147483648'";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptSubComponentRegister.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptSubComponentRegister.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

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
            CommonClasses.SendError("Bill Of Component View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSubComponentReg.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sub Component Register", "btnCancel_Click", Ex.Message);
        }
    } 
    #endregion
}
