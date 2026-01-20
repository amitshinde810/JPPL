using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_DCRetrunRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/DCReturnRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery challan Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

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

            GenerateReport(Title, From, To, group, way, Cond, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition, string Type)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "  Select DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME as I_UOM_NAME,DCM_TYPE,DCD_ORD_QTY ,SUM(DND_REC_QTY) AS DND_REC_QTY ,SUM(DND_SCRAP_QTY) AS  DND_SCRAP_QTY from DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER , DC_RETURN_MASTER,DC_RETURN_DETAIL where " + Condition + " DCM_TYPE='DLC' AND  DCM_CODE=DCD_DCM_CODE and DCD_I_CODE=I_CODE and DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.ES_DELETE=0 and DCD_UM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND DNM_CODE=DND_DNM_CODE AND DC_RETURN_MASTER.ES_DELETE=0 AND DNM_PARTY_DC_NO =DCM_CODE AND DCD_I_CODE=DND_I_CODE  GROUP BY DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME ,DCM_TYPE,DCD_ORD_QTY";

            if (Type == "1")
            {
                Query = Query + " HAVING ISNULL(DCD_ORD_QTY,0) -(ISNULL(SUM(DND_REC_QTY),0)+ISNULL(SUM(DND_SCRAP_QTY),0))>0 ";
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDCRetrunRegisterDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDCRetrunRegisterDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "SuppWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDCRetrunRegisterSuppWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDCRetrunRegisterSuppWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
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
}
