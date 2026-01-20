using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;


public partial class ToolRoom_ADD_WEEKLYPMPRINT : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

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
            string Title = Request.QueryString[0];
            GenerateReport(Title);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Title)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();

        dt = CommonClasses.Execute("select WPM_CODE,WPM_SLIP_NO,T_NAME,LEFT(DATENAME(MONTH, WPM_MONTH), 3) + ' ' + DATENAME(YEAR, WPM_MONTH) as WPM_MONTH,(CASE WPM_WEEK when '0' then '1 WEEK' when '1' then '2 WEEK' when '2' then '3 WEEK' when '3' then '4 WEEK' else 'PM_WEEK' END) as WPM_WEEK,WPM_UPLOAD_FILE,CASE WHEN WPM_STATUS=1 then '1' Else '0' END  AS WPM_STATUS,I_CODENO,I_NAME,WPM_TOOL_EXPCOMP_DATE,WPM_TOOL_RECEIVED_DATE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where   T_I_CODE=I_CODE AND  WPM_CM_COMP_ID=" + Session["CompanyId"].ToString() + "  AND   TOOL_MASTER.ES_DELETE=0 and WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and TOOL_MASTER.T_CODE=WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.WPM_T_CODE AND WPM_CODE='" + Title + "' ORDER BY WPM_CODE desc");
        if (dt.Rows.Count > 0)
        {

            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptWeeklyPMPrint.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptWeeklyPMPrint.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rptname;
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            return;
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
            CommonClasses.SendError("weekly PM Print Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/ToolRoom/VIEW/WEEKLYPREVMAINDIE.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("weekly PM Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
