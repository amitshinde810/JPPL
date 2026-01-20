using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
public partial class RoportForms_ADD_BiilOfMaterialRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
        home1.Attributes["class"] = "active";
    }
    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            if (ChkAll == true)
            {
                GenerateReport(Title, "All");
            }
            else
            {
                string Comp = Request.QueryString[2];
                GenerateReport(Title, Comp);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material Register", "Page_Init", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Comp)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        if (Comp == "All")
        {
            Query = "with t1 as ( select BM_CODE,BM_I_CODE,I_NAME,I_CODENO from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BOM_MASTER.BM_I_CODE=I_CODE), t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,I_CODENO,BD_VQTY,BD_SCRAPQTY from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0) select t1.I_NAME as FINISH_NAME,t1.I_CODENO as FinishCode,t2.I_NAME as SUB_NAME,t2.BD_VQTY,t2.I_CODENO as I_CODENO ,t2.BD_SCRAPQTY from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE  group by t1.I_NAME,t1.I_CODENO,t2.I_NAME,t2.BD_VQTY,t2.I_CODENO,t2.BD_SCRAPQTY ";
        }
        else
        {
            Query = "with t1 as ( select BM_CODE,BM_I_CODE,I_NAME,I_CODENO from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BOM_MASTER.BM_I_CODE=I_CODE), t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,I_CODENO,BD_VQTY,BD_SCRAPQTY from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0) select t1.I_NAME as FINISH_NAME,t1.I_CODENO as FinishCode,t2.I_NAME as SUB_NAME,t2.BD_VQTY,t2.I_CODENO as I_CODENO ,t2.BD_SCRAPQTY from t1,t2  where t2.BD_BM_CODE=t1.BM_CODE and t1.BM_I_CODE='" + Comp + "' group by t1.I_NAME,t1.I_CODENO,t2.I_NAME,t2.BD_VQTY,t2.I_CODENO,t2.BD_SCRAPQTY";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptBOMRegister.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptBOMRegister.rpt");
            rptname.Refresh();

            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            CrystalReportViewer1.ReportSource = rptname;
        }
        else
        {
            lblmsg.Text = "No Records Found";
            PanelMsg.Visible = true;
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
            CommonClasses.SendError("Bill Of Component View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewBillOfMaterialRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material Register", "btnCancel_Click", Ex.Message);
        }
    }
}
