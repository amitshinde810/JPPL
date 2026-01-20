using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_ProductionToStoreRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string type = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
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
            string group = Request.QueryString[3].ToString();
            type = Request.QueryString[4].ToString();
            string Condition = Request.QueryString[5].ToString();
            string Rtype = Request.QueryString[6].ToString();
            GenerateReport(Title, From, To, group, Condition, Rtype);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "Page_Init", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string vcond, string RType)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (type == "0")
            {
                Query = "select PS_CODE,PS_GIN_NO,Convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,PS_PERSON_NAME,(CASE PS_TYPE WHEN 1 THEN 'As per requirement' WHEN 2 THEN 'Direct'  END) AS PS_TYPE,I_NAME As ITEM_NAME,PSD_QTY,PSD_REMARK,I_UOM_NAME,I_CODENO,ROUND(I_INV_RATE,2) AS I_INV_RATE ,I_UWEIGHT,I_TARGET_WEIGHT,I_DENSITY   from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE " + vcond + "   ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PSD_PS_CODE=PS_CODE and PSD_I_CODE=I_CODE and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0 and PS_TYPE=2 and PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE='" + Session["CompanyCode"] + "'";
            }
            else if (type == "1")
            {
                Query = "select PS_CODE,PS_GIN_NO,Convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,PS_PERSON_NAME,(CASE PS_TYPE WHEN 1 THEN 'As per requirement' WHEN 2 THEN 'Direct'  END) AS PS_TYPE,I_NAME As ITEM_NAME,PSD_QTY,PSD_REMARK,I_UOM_NAME,I_CODENO,ROUND(I_INV_RATE,2) AS I_INV_RATE ,I_UWEIGHT,I_TARGET_WEIGHT,I_DENSITY    from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE " + vcond + "    ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PSD_PS_CODE=PS_CODE and PSD_I_CODE=I_CODE and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0 and PS_TYPE=1 and PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE='" + Session["CompanyCode"] + "'";
            }
            else
            {
                Query = "select PS_CODE,PS_GIN_NO,PS_GIN_DATE AS PS_GIN_DATE,PS_PERSON_NAME,(CASE PS_TYPE WHEN 1 THEN 'As per requirement' WHEN 2 THEN 'Direct'  END) AS PS_TYPE,I_NAME As ITEM_NAME,PSD_QTY,PSD_REMARK,I_UOM_NAME,I_CODENO,ROUND(I_INV_RATE,2) AS I_INV_RATE ,I_UWEIGHT,I_TARGET_WEIGHT,I_DENSITY    from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE  " + vcond + "   ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PSD_PS_CODE=PS_CODE and PSD_I_CODE=I_CODE and PRODUCTION_TO_STORE_MASTER.ES_DELETE=0 and PRODUCTION_TO_STORE_MASTER.PS_CM_COMP_CODE='" + Session["CompanyCode"] + "'";
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (group == "Datewise")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptProdToStoreDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptProdToStoreDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtType", RType);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "Itemwise")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptProdToStoreItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptProdToStoreItemWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtType", RType);
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
            CommonClasses.SendError("Production To Store Register", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewProdcutionToStoreRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Production To Store Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
