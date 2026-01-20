using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_StockAdjustmentRegister : System.Web.UI.Page
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
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string cond = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, group, cond);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    private void GenerateReport(string Title, string From, string To, string group, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "SELECT SAM_DATE,SAM_DOC_NO,isnull(cast((case when SAD_ADJUSTMENT_QTY > 0 THEN SAD_ADJUSTMENT_QTY end) as numeric(10,3)),0) as STOCK_IN,isnull(ABS(cast((case when SAD_ADJUSTMENT_QTY < 0 THEN SAD_ADJUSTMENT_QTY end) as numeric(10,3))),0) as STOCK_OUT,SAD_REMARK,SAD_I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,STOCK_ADJUSTMENT_DETAIL,STOCK_ADJUSTMENT_MASTER WHERE  " + cond + "  SAM_CODE=SAD_SAM_CODE AND SAD_I_CODE=I_CODE and STOCK_ADJUSTMENT_MASTER.ES_DELETE=0 order by SAM_DATE";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptStockAdjustmentDatewiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptStockAdjustmentDatewiseReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtTitle", "Stock Adjustment Date wise From " + From + " to " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptStockAdjustmentItemwiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptStockAdjustmentItemwiseReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtTitle", "Stock Adjustment Item Wise From " + From + " to " + To);

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
            CommonClasses.SendError("Stock Adjustment Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewStockAdjustmentRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
