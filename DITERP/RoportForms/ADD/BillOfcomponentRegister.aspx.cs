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
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_BillOfcomponentRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

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
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Comp)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        //DataTable dtMain = new DataTable();
        //dtMain.Columns.Add("EMCode");
        //dtMain.Columns.Add("EMName");
        //dtMain.Columns.Add("Date");
        //dtMain.Columns.Add("Particulars");
        //dtMain.Columns.Add("LoanGiven");
        //dtMain.Columns.Add("Installment");
        //dtMain.Columns.Add("Balance");

        if (Comp == "All")
        {
            Query = " with t1 as ( select BM_CODE,BM_I_CODE,I_NAME from BOC_DETAIL,BOC_MASTER,ITEM_MASTER where BOC_MASTER.BM_I_CODE=I_CODE), t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,BD_VQTY,BD_SQTY from BOC_DETAIL,BOC_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOC_MASTER.ES_DELETE=0) select t1.I_NAME as FINISH_NAME,t2.I_NAME as SUB_NAME,t2.BD_VQTY,t2.BD_SQTY from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE  group by t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t2.BD_SQTY";
        }
        else
        {
            Query = " with t1 as ( select BM_CODE,BM_I_CODE,I_NAME from BOC_DETAIL,BOC_MASTER,ITEM_MASTER where BOC_MASTER.BM_I_CODE=I_CODE), t2 as(select BD_BM_CODE,BD_I_CODE,I_NAME,BD_VQTY,BD_SQTY from BOC_DETAIL,BOC_MASTER,ITEM_MASTER where BD_BM_CODE=BM_CODE and BD_I_CODE=I_CODE and BOC_MASTER.ES_DELETE=0) select t1.I_NAME as FINISH_NAME,t2.I_NAME as SUB_NAME,t2.BD_VQTY,t2.BD_SQTY from t1,t2 where t2.BD_BM_CODE=t1.BM_CODE and t1.BM_I_CODE='" + Comp + "' group by t1.I_NAME,t2.I_NAME,t2.BD_VQTY,t2.BD_SQTY";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {


            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptBOCRegister.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptBOCRegister.rpt");
            rptname.Refresh();
            //rptname.SetParameterValue("txtName", Session["CompanyName"].ToString());
            rptname.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rptname;

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
            Response.Redirect("~/RoportForms/VIEW/ViewMasterReport.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master Register", "btnCancel_Click", Ex.Message);
        }

    }
}
