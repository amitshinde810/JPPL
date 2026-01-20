using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_DailyIRNReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string i_name = Request.QueryString[2].ToString();
            GenerateReport(Title, From, i_name);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string i_name)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (i_name == "0")
            {
                Query = " declare @IRN_DATE datetime='" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'     SELECT DISTINCT I_CODE,I_CODENO,I_NAME    ,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483648 AND IRND_I_CODE=I_CODE AND IRN_DATE=@IRN_DATE),0) AS R1 ,      ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483647 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R2       ,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483647 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R3      ,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483646 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R4A_CASTING,      ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483646 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R4A_MECHINING,      ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R4_CASTING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R4_MECHINING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483644 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R5_CASTING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483644 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R5_MECHINING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R6_CASTING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R6_MECHINING FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND I_CODE=IRND_I_CODE  AND IRN_DATE=@IRN_DATE ORDER BY I_CODENO ";
            }
            else
            {
                Query = " declare @IRN_DATE datetime='" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'    SELECT DISTINCT I_CODE,I_CODENO,I_NAME    ,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483648 AND IRND_I_CODE=I_CODE AND IRN_DATE=@IRN_DATE),0) AS R1 ,      ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483647 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R2       ,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483647 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R3      ,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483646 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R4A_CASTING,      ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483646 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R4A_MECHINING,      ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R4_CASTING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R4_MECHINING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483644 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R5_CASTING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483644 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R5_MECHINING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND IRND_I_CODE=I_CODE AND IRND_TYPE=0 AND IRN_DATE=@IRN_DATE),0) AS R6_CASTING,ISNULL((SELECT SUM(IRND_REJ_QTY) AS IRND_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=@IRN_DATE),0) AS R6_MECHINING FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND I_CODE=IRND_I_CODE  AND IRN_DATE=@IRN_DATE AND IRND_I_CODE='" + i_name + "'  ORDER BY I_CODENO ";
            }
         
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/IRN/Reports/rptDailyIRNReport.rpt"));
                rptname.FileName = Server.MapPath("~/IRN/Reports/rptDailyIRNReport.rpt"); 
                rptname.Refresh();
                rptname.SetDataSource(dt);  
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "DAILY REJECTION REPORT On " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy"));
                
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
            Response.Redirect("~/IRN/VIEW/ViewDailyIRNReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
