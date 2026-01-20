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

public partial class ToolRoom_ADD_BreakdownRegister : System.Web.UI.Page
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

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string Condition = Request.QueryString[3].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            GenerateReport(Title, From, To, Condition);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Title, string From, string To, string Condition)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        //dt = CommonClasses.Execute(" DECLARE @CM_STATE INT = (SELECT CM_STATE FROM COMPANY_MASTER where CM_CODE=" + Session["CompanyCode"].ToString() + ")  SELECT CL_CH_NO ,convert(varchar ,CL_DATE,105) AS CL_DATE ,CL_P_CODE ,P_NAME ,CL_I_CODE ,I_CODENO ,I_NAME ,CL_DOC_NO ,CL_DOC_DATE ,CL_CQTY ,CL_DOC_TYPE , case when P_LBT_IND=0 then '' else P_GST_NO end AS P_LBT_NO, SM_STATE_CODE+'-'+ SM_NAME AS SM_STATE_CODE, STATE_MASTER.SM_NAME,E_TARIFF_NO ,CASE WHEN @CM_STATE=P_SM_CODE THEN E_BASIC ELSE 0 END AS E_BASIC,CASE WHEN @CM_STATE=P_SM_CODE THEN E_EDU_CESS ELSE 0 END AS E_EDU_CESS,CASE WHEN @CM_STATE<>P_SM_CODE THEN E_H_EDU ELSE 0 END AS E_H_EDU,E_SPECIAL ,  CASE when I_CAT_CODE=-2147483648 then ROUND(I_INV_RATE*60/100,2) ELSE    ROUND(I_INV_RATE ,2)  END AS IND_RATE,I_INV_RATE,CASE WHEN I_UOM_NAME='NOS' THEN 'NUMBERS' WHEN   I_UOM_NAME='KGS' THEN 'KILOGRAMS'  ELSE  I_UOM_NAME END AS I_UOM_NAME      FROM CHALLAN_STOCK_LEDGER,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,INWARD_DETAIL,INWARD_MASTER,STATE_MASTER,ITEM_UNIT_MASTER where   " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0  AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='IWIAP' and ITEM_MASTER.I_E_CODE=E_CODE and IWD_IWM_CODE=IWM_CODE AND INWARD_MASTER.ES_DELETE=0  AND IWM_TYPE='OUTCUSTINV'   AND IWM_NO=CL_DOC_NO AND IWD_I_CODE=CL_I_CODE and P_SM_CODE=STATE_MASTER.SM_CODE   AND CL_DATE  BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ORDER BY convert(int,CL_CH_NO)   ");
        dt = CommonClasses.Execute(" SELECT B_CODE,case when B_STATUS=0 then 'Open' else 'Close' end as B_STATUS,B_T_CODE,B_NO as B_SLIPNO, B_NO,  B_DATE , B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE, T_NAME, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE, I_CODENO +' - '+ I_NAME AS I_CODENO FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE AND BREAKDOWN_ENTRY.B_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + Condition + "  B_CM_CODE=" + Session["CompanyCode"].ToString() + " AND (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=0 ORDER BY B_NO,T_NAME");

        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptBreakdown.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptBreakdown.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            //rptname.SetParameterValue("txtCompGST", Session["CompanyGST"].ToString());
            rptname.SetParameterValue("txtPeriod", "BREAKDOWN REGISTER FROM " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " TO " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
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
            Response.Redirect("~/ToolRoom/VIEW/ViewBreakdownRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("GST ITC-04", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
