using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_MasterRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string MasterIndex = Request.QueryString[1];
            GenerateReport(Title, MasterIndex);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string MasterIndex)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        
        if (MasterIndex == "2")
        {
        }
        
        #region Unit Master
        if (MasterIndex == "3")
        {
            DL_DBAccess = new DatabaseAccessLayer();

            DataTable dtfinal = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME,I_UOM_DESC from ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.ES_DELETE=0 ");

            if (dtfinal.Rows.Count > 0)
            {
            }
            try
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptUnitMaster.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptUnitMaster.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                CrystalReportViewer1.ReportSource = rptname;
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Masters Register", "GenerateReport", Ex.Message);
            }
        } 
        #endregion

        #region Sector Master
        if (MasterIndex == "5")
        {
            DL_DBAccess = new DatabaseAccessLayer();

            DataTable dtfinal = CommonClasses.Execute("select PROCESS_CODE,PROCESS_NAME from PROCESS_MASTER where ES_DELETE=0 and PROCESS_CM_COMP_ID='" + Session["CompanyId"] + "' order by PROCESS_NAME ");

            try
            {
                if (dtfinal.Rows.Count > 0)
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptProcessMaster.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptProcessMaster.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Masters Register", "GenerateReport", Ex.Message);
            }
        }
        #endregion

        #region User Master
        if (MasterIndex == "6")
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataTable dtfinal = CommonClasses.Execute("select SHM_CODE,SHM_FORMULA_CODE,SHM_FORMULA_NAME from SHADE_MASTER where SHADE_MASTER.ES_DELETE=0 ");

            try
            {
                if (dtfinal.Rows.Count > 0)
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptShadeMaster.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptShadeMaster.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Masters Register", "GenerateReport", Ex.Message);
            }
        }
        #endregion
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
            CommonClasses.SendError("Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[1] == "5" || Request.QueryString[1] == "6")
            {
                Response.Redirect("~/RoportForms/VIEW/ViewRNDMasterReport.aspx", false);
            }
            else
            {
                Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master Register", "btnCancel_Click", Ex.Message);
        }
    }
}
