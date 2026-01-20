using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_ProductionTOStorePrint : System.Web.UI.Page
{
    #region Variable
    DatabaseAccessLayer DL_DBAccess = null;
    string MatReq_code = "";
    string type = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            MatReq_code = Request.QueryString[0];
            type = Request.QueryString[1];
            GenerateReport(MatReq_code, type);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Print", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string code, string printType)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();

            DataTable dtfinal = new DataTable();

            dtfinal = CommonClasses.Execute("SELECT PS_GIN_NO, PS_GIN_DATE, PS_PERSON_NAME,I_UOM_NAME, I_CAT_NAME, I_NAME, I_INV_RATE,PS_P_CODE,I_CODENO,PSD_QTY,PSD_REMARK  FROM PRODUCTION_TO_STORE_MASTER INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE INNER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE INNER JOIN PRODUCTION_TO_STORE_DETAIL ON ITEM_MASTER.I_CODE = PRODUCTION_TO_STORE_DETAIL.PSD_I_CODE ON PRODUCTION_TO_STORE_MASTER.PS_CODE = PRODUCTION_TO_STORE_DETAIL.PSD_PS_CODE  WHERE     (PRODUCTION_TO_STORE_MASTER.ES_DELETE = 0)  AND  PS_CODE='" + code + "'");


            if (dtfinal.Rows.Count > 0)
            {
                try
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/RptProd2Store.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/RptProd2Store.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    rptname.SetParameterValue("txtTitle", "Production To Store");
                    DataTable dtuser = CommonClasses.Execute("SELECT * FROM USER_MASTER WHERE UM_CODE='" + Session["UserCode"].ToString() + "'");
                    rptname.SetParameterValue("txtUser", dtuser.Rows[0]["UM_NAME"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;


                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Material Requisition Print", "GenerateReport", Ex.Message);

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
            CommonClasses.SendError("Material Requisition Print", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (type == "REQUSITION")
            {
                Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
            }
            else
            {
                Response.Redirect("~/Transactions/VIEW/ViewProductionToStore.aspx", false);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
