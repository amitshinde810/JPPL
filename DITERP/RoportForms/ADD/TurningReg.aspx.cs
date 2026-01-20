using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_TurningReg : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Menu highlight code
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
            Query = " SELECT SM_CODE,convert(varchar,SM_IWM_NO)as SM_IWM_NO ,CONVERT(VARCHAR,SM_IWM_DATE,106)SM_IWM_DATE,SM_P_CODE,P_NAME ,SM_CH_NO,CONVERT(VARCHAR,SM_CH_DATE,106)AS SM_CH_DATE,I_CODENO,I_NAME,SD_REV_QTY FROM SCRAP_MASTER,SCRAP_DETAIL,ITEM_MASTER,PARTY_MASTER where SM_CODE=SD_SM_CODE AND SCRAP_MASTER.ES_DELETE=0 AND SD_I_CODE=I_CODE AND SM_P_CODE=P_CODE AND SM_CM_CODE=" + Session["CompanyCode"].ToString() + " " + cond + "  ORDER BY convert(int, SM_IWM_NO)";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                 
                    rptname.Load(Server.MapPath("~/Reports/rptTurningReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptTurningReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtTitle", "Turning Report " + From + " to " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                
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
            Response.Redirect("~/RoportForms/VIEW/ViewTurningReg.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
