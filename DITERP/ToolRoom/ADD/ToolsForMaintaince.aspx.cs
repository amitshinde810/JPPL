using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class ToolRoom_ADD_ToolsForMaintaince : System.Web.UI.Page
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

            GenerateReport();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        //dt = CommonClasses.Execute("DECLARE @CM_STATE INT = (SELECT CM_STATE FROM COMPANY_MASTER where CM_CODE=" + Session["CompanyCode"].ToString() + ")  SELECT CL_CH_NO ,convert(varchar ,CL_DATE,105) AS CL_DATE ,CL_P_CODE ,P_NAME ,CL_I_CODE ,I_CODENO ,I_NAME ,CL_DOC_NO ,CL_DOC_DATE ,CL_CQTY ,CL_DOC_TYPE , case when P_LBT_IND=0 then '' else P_GST_NO end AS P_LBT_NO, SM_STATE_CODE+'-'+ SM_NAME AS SM_STATE_CODE, STATE_MASTER.SM_NAME,E_TARIFF_NO ,CASE WHEN @CM_STATE=P_SM_CODE THEN E_BASIC ELSE 0 END AS E_BASIC,CASE WHEN @CM_STATE=P_SM_CODE THEN E_EDU_CESS ELSE 0 END AS E_EDU_CESS,CASE WHEN @CM_STATE<>P_SM_CODE THEN E_H_EDU ELSE 0 END AS E_H_EDU,E_SPECIAL ,  CASE when I_CAT_CODE=-2147483648 then ROUND(I_INV_RATE*60/100,2) ELSE    ROUND(I_INV_RATE ,2)  END AS IND_RATE,I_INV_RATE,CASE WHEN I_UOM_NAME='NOS' THEN 'NUMBERS' WHEN   I_UOM_NAME='KGS' THEN 'KILOGRAMS'  ELSE  I_UOM_NAME END AS I_UOM_NAME      FROM CHALLAN_STOCK_LEDGER,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,INWARD_DETAIL,INWARD_MASTER,STATE_MASTER,ITEM_UNIT_MASTER where   " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0  AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='IWIAP' and ITEM_MASTER.I_E_CODE=E_CODE and IWD_IWM_CODE=IWM_CODE AND INWARD_MASTER.ES_DELETE=0  AND IWM_TYPE='OUTCUSTINV'   AND IWM_NO=CL_DOC_NO AND IWD_I_CODE=CL_I_CODE and P_SM_CODE=STATE_MASTER.SM_CODE   AND CL_DATE  BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ORDER BY convert(int,CL_CH_NO)   ");
        //dt = CommonClasses.Execute("SELECT T_CODE,CASE WHEN T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END As T_TYPE,T_NAME,MAX(WPM_TOOL_COMPLETED_DATE ) AS LASTDATE , (SELECT isnull(sum(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_TYPE=0 AND IRND_TYPE=1 and IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_T_CODE= T_CODE  and IRN_DATE between MAX(WPM_TOOL_COMPLETED_DATE ) AND GETDATE()) AS PROD_QTY,(SELECT isnull(sum(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_TYPE=0 AND IRND_TYPE=1 and IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_T_CODE= T_CODE  and IRN_DATE between  DATEADD(MM,-2, MAX(WPM_TOOL_COMPLETED_DATE)) AND MAX(WPM_TOOL_COMPLETED_DATE )) AS LAST2, T_PMFREQ,I_CODE,I_CODENO,I_NAME,DATEADD(MM,-2, MAX(WPM_TOOL_COMPLETED_DATE)) AS LAST2Date into #temp FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where WPM_T_CODE=T_CODE AND T_I_CODE=I_CODE AND WPM_STATUS=1 and TOOL_MASTER.ES_DELETE=0 and WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 GROUP BY T_CODE,T_NAME,T_PMFREQ,T_TYPE,I_CODE,I_CODENO,I_NAME SELECT T_CODE,T_TYPE,T_NAME,LASTDATE,T_PMFREQ-PROD_QTY AS PM_DUE,PROD_QTY,LAST2,T_PMFREQ,LAST2Date,I_CODE,I_CODENO,I_NAME from #temp DROP TABLE #temp");
        dt = CommonClasses.Execute("select T_CODE,CASE WHEN T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END As T_TYPE,T_NAME,isnull((select MAX(WPM_TOOL_COMPLETED_DATE ) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=T_CODE),'') as LASTDATE,isnull(T_PMFREQ,0) as T_PMFREQ, (SELECT isnull(sum(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_TYPE=0 AND IRND_TYPE=1 and IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_T_CODE= T_CODE and IRN_DATE between isnull((select MAX(WPM_TOOL_COMPLETED_DATE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=T_CODE),'') AND GETDATE())  AS PROD_QTY,isnull((isnull(T_PMFREQ,0)-(SELECT isnull(sum(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_TYPE=0 AND IRND_TYPE=1 and IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_T_CODE= T_CODE and IRN_DATE between isnull((select MAX(WPM_TOOL_COMPLETED_DATE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=T_CODE),'') AND GETDATE())),0) as PM_DUE,(SELECT isnull(sum(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_TYPE=0 AND IRND_TYPE=1 and IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_T_CODE= T_CODE  and IRN_DATE between DATEADD(MM,-2, isnull((select MAX(WPM_TOOL_COMPLETED_DATE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=T_CODE),'')) AND isnull((select MAX(WPM_TOOL_COMPLETED_DATE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=T_CODE),'')) AS LAST2,isnull((DATEADD(MM,-2, isnull((select MAX(WPM_TOOL_COMPLETED_DATE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_T_CODE=T_CODE),''))),'') as LAST2Date,I_CODE,I_CODENO,I_NAME from TOOL_MASTER,ITEM_MASTER where  T_STATUS=1 AND T_I_CODE=I_CODE  and ITEM_MASTER.ES_DELETE=0 and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 order by PM_DUE asc");
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptToolsForPreventiveMain.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptToolsForPreventiveMain.rpt");

            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            //rptname.SetParameterValue("txtCompGST", Session["CompanyGST"].ToString());
            rptname.SetParameterValue("txtPeriod", "TRIGGER FOR PREVENTIVE MAINTENANCE");
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
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("GST ITC-04", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
