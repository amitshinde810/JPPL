using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class ToolRoom_ADD_BreakdownSlipReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Type = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Condition = "B_CODE=" + Request.QueryString[0].ToString() + " AND ";
            Type = Request.QueryString[1].ToString();

            if (Type == "0")
            {
                lblTitle.Text = "TOOLING BREAKDOWN SLIP";
                Page.Header.Title = "TOOLING BREAKDOWN SLIP";
            }
            else
            {
                lblTitle.Text = "INDENT FOR TOOLING IMPROVEMENT";
                Page.Header.Title = "INDENT FOR TOOLING IMPROVEMENT";
            }
            GenerateReport(Condition, Type);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Cond, string Type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        if (Type == "0")
        {
            dt = CommonClasses.Execute("SELECT B_CODE,B_T_CODE,B_SLIPNO, B_NO, B_DATE ,case when T_OWNER=0 then 'PCPL' else 'CUSTOMER' end as T_OWNER, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE, T_NAME, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE, I_CODENO,I_NAME FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON BREAKDOWN_ENTRY.B_I_CODE = ITEM_MASTER.I_CODE WHERE " + Cond + " B_CM_CODE=" + Session["CompanyCode"].ToString() + " AND (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=0");
        }
        else
        {
            dt = CommonClasses.Execute("SELECT B_CODE,B_T_CODE,B_SLIPNO, B_NO, B_DATE ,case when T_OWNER=0 then 'PCPL' else 'CUSTOMER' end as T_OWNER, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE, T_NAME, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE, I_CODENO,I_NAME FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON BREAKDOWN_ENTRY.B_I_CODE = ITEM_MASTER.I_CODE WHERE " + Cond + " B_CM_CODE=" + Session["CompanyCode"].ToString() + " AND (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=1");
        }

        if (Type == "0")
        {
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptBreakdownSlip.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptBreakdownSlip.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dt);
                //rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "TOOLING BREAKDOWN SLIP");
                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        else
        {
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptImprovementSlip.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptImprovementSlip.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dt);
                //rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "INDENT FOR TOOLING IMPROVEMENT");
                CrystalReportViewer1.ReportSource = rptname;
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
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
            CommonClasses.SendError("GST ITC-04 Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Type == "0")
            {
                Response.Redirect("~/ToolRoom/VIEW/ViewBreakdown.aspx", false);
            }
            else
            {
                Response.Redirect("~/ToolRoom/VIEW/ViewImprovement.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("GST ITC-04", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
