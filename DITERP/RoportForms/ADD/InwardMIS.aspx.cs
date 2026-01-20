using System;
using System.Web.UI;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_InwardMIS : System.Web.UI.Page
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
            string Cond = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();
            string POType = Request.QueryString[7].ToString();
            #region MyRegion
            #endregion

            GenerateReport(Title, From, To, group, way, Cond, Type.Trim().ToString(), POType);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string condition, string Type, string POType)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            //Query = "SELECT PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_DETAIL.IWD_SQTY, INWARD_DETAIL.IWD_CH_QTY, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY, INWARD_DETAIL.IWD_CON_REJ_QTY, INWARD_DETAIL.IWD_INSP_FLG, INWARD_DETAIL.IWD_INSP_NO,  CASE WHEN IWD_HOLD=1 then'YES' ELSE 'NO' END aS IWD_HOLD,  (SELECT INSP_RDESC  FROM INSPECTION_REASONMASTER where INSP_RCODE=IWD_HOLDR) AS IWD_HOLDR, INWARD_DETAIL.IWD_HDATE ,CASE WHEN IWD_INSP_FLG =0 then DATEDIFF(hh,IWM_DATE,GETDATE())ELSE '0' END AS HRDIFF FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE WHERE  " + condition + "   (INWARD_MASTER.ES_DELETE = 0) ";
            //Query = "SELECT PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_DETAIL.IWD_SQTY, INWARD_DETAIL.IWD_CH_QTY, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY, INWARD_DETAIL.IWD_CON_REJ_QTY, INWARD_DETAIL.IWD_INSP_FLG, INWARD_DETAIL.IWD_INSP_NO,  CASE WHEN IWD_HOLD=1 then'YES' ELSE 'NO' END aS IWD_HOLD,  (SELECT INSP_RDESC  FROM INSPECTION_REASONMASTER where INSP_RCODE=IWD_HOLDR) AS IWD_HOLDR, INWARD_DETAIL.IWD_HDATE ,CASE WHEN IWD_INSP_FLG =0 then DATEDIFF(hh,IWM_DATE,GETDATE())ELSE '0' END AS HRDIFF ,case when (IWD_INSP_FLG='0' and IWD_HOLD='0') then 'Pending' when (IWD_INSP_FLG='0' and IWD_HOLD='1') then  'Hold' when (IWD_INSP_FLG='1' and IWD_HOLD='1') then  'Inspected' when (IWD_INSP_FLG='1' and IWD_HOLD='0') then 'Inspected' end as [Status] FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE WHERE  " + condition + "   (INWARD_MASTER.ES_DELETE = 0) ";
            Query = "SELECT PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, INWARD_MASTER.IWM_NO, INWARD_MASTER.IWM_DATE, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_DETAIL.IWD_SQTY, INWARD_DETAIL.IWD_CH_QTY, INWARD_DETAIL.IWD_REV_QTY, INWARD_DETAIL.IWD_CON_OK_QTY, INWARD_DETAIL.IWD_CON_REJ_QTY, INWARD_DETAIL.IWD_INSP_FLG, INWARD_DETAIL.IWD_INSP_NO,  CASE WHEN (IWD_INSP_FLG='0' and IWD_HOLD='1') then'YES' ELSE 'NO' END aS IWD_HOLD,  (SELECT INSP_RDESC  FROM INSPECTION_REASONMASTER where INSP_RCODE=IWD_HOLDR) AS IWD_HOLDR, INWARD_DETAIL.IWD_HDATE ,CASE WHEN IWD_INSP_FLG =0 then DATEDIFF(hh,IWM_DATE,GETDATE())ELSE '0' END AS HRDIFF ,case when  (IWD_INSP_FLG='0' and IWD_HOLD='1') then 'HOLD' WHEN (IWD_INSP_FLG='1') then 'Inspected'  else 'Pending' end as[Status] FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE WHERE  " + condition + "   (INWARD_MASTER.ES_DELETE = 0) ";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();  
                rptname.Load(Server.MapPath("~/Reports/rptInwardMIS.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptInwardMIS.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtType", "Supplierwise " + Type + " Summery Report");
                rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("txtReportType", "Inward MIS Report From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                CrystalReportViewer1.ReportSource = rptname;

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Inward Supp Wise", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/RoportForms/VIEW/ViewInwardMIS.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supplierwise", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
