using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_IRNCustomerRejectionPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Type = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Condition = "CRM_CODE=" + Request.QueryString[0].ToString() + " AND ";
            Type = Request.QueryString[1].ToString();

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
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        //dt = CommonClasses.Execute("SELECT B_CODE,B_T_CODE,B_SLIPNO, B_NO, B_DATE ,case when T_OWNER=0 then 'PCPL' else 'CUSTOMER' end as T_OWNER, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE, T_NAME, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE, I_CODENO,I_NAME FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON BREAKDOWN_ENTRY.B_I_CODE = ITEM_MASTER.I_CODE WHERE " + Cond + " B_CM_CODE=" + Session["CompanyCode"].ToString() + " AND (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=0");
        dt = CommonClasses.Execute("select CRM_CODE,CRM_NO,CRM_COMPLAINT_NO,CRD_QTY,CRD_REASON,convert(Varchar,CRM_DATE,106) as CRM_DATE,I_CODENO,I_NAME,P_NAME from IRN_CUSTOMER_REJECTION_DETAIL,IRN_CUSTOMER_REJECTION_MASTER,ITEM_MASTER,PARTY_MASTER where " + Cond + " IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 and P_ACTIVE_IND=1 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and CRD_I_CODE=I_CODE and CRM_P_CODE=P_CODE and CRM_CODE=CRD_CRM_CODE and CRM_CM_COMP_CODE=" + Session["CompanyCode"].ToString() + "");

        if (Type == "0")
        {
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/IRNCustomerRejPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/IRNCustomerRejPrint.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtPeriod", "CUSTOMER COMPLAINT ");
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
            CommonClasses.SendError("CUSTOMER COMPLAINT", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("CUSTOMER COMPLAINT", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
