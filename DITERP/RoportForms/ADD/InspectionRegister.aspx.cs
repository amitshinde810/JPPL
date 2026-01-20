using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_InspectionRegister : System.Web.UI.Page
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
            string showStatus = Request.QueryString[5].ToString();
            string Cond = Request.QueryString[6].ToString();
            GenerateReport(Title, From, To, group, way, showStatus, Cond);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string showStatus, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "SELECT distinct INSM_CODE,(case when INSM_PDR_CHECK=0 then 'Pending' when INSM_PDR_CHECK=1 then 'Done' end) as INSM_PDR_CHECK,(case when INSM_PDR_NO='' then '0' else INSM_PDR_NO end)as INSM_PDR_NO,convert(varchar,INSM_DATE,106) as INSM_DATE,INSM_NO,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,IWM_NO,IWM_TYPE,convert(varchar,IWM_DATE,106) as IWM_DATE,P_NAME as LM_NAME,ITEM_UNIT_MASTER.I_UOM_CODE as UOM_CODE,I_UOM_NAME as UOM_NAME,I_CODENO,I_NAME,ITEM_UNIT_MASTER.I_UOM_CODE as UOM_CODE ,I_UOM_NAME as UOM_NAME FROM INSPECTION_S_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,INWARD_MASTER,PARTY_MASTER,INWARD_DETAIL where " + Cond + " INSM_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND INSM_I_CODE = I_CODE AND INSM_IWM_CODE = IWM_CODE AND INSM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " AND IWM_P_CODE= P_CODE and IWM_CODE=IWD_IWM_CODE and INSPECTION_S_MASTER.ES_DELETE=0 and ITEM_UNIT_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and INWARD_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {

                    if (way == "pending")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInspectionPendingDatewise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInspectionPendingDatewise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", group + "Pending Inspection Report");
                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInspectionInspectedDatewise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInspectionInspectedDatewise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", group + " Inspected Inspection Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {

                    if (way == "pending")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInspectionPendingItemwise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInspectionPendingItemwise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", group + " Pending Inspection Report");
                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInspectionInspectedItemwise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInspectionInspectedItemwise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", group + " Inspected Inspection Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    if (way == "pending")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInspectionPendingPartywise .rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInspectionPendingPartywise .rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "Supplierwise Pending Inspection Report");
                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInspectionInspectedPartywise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInspectionInspectedPartywise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "Supplierwise Inspected Inspection Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found ! ";
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
            CommonClasses.SendError("Inspection Register View Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/View/ViewInspectionRegisterReport.aspx", false);
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion btnCancel_Click

}
