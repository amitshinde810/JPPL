using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;

public partial class Admin_MasterRegister_ADD_AddUserMaster : System.Web.UI.Page
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
            bool ChkUserNameAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            string User_Name = Request.QueryString[2].ToString();

            if (ChkUserNameAll == true)
            {
                GenerateReport(Title, User_Name, "All");
            }
            if (ChkUserNameAll == false)
            {
                GenerateReport(Title, User_Name, "NotAll");
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string User_Name, string Comp1)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";

        if (Comp1 == "All")
        {
            Query = "SELECT UM_CODE,UM_NAME,UM_EMAIL,UM_LEVEL,UM_USERNAME FROM USER_MASTER WHERE ES_DELETE=0";
        }
        else
        {
            Query = "SELECT UM_CODE,UM_NAME,UM_EMAIL,UM_LEVEL,UM_USERNAME FROM USER_MASTER WHERE  ES_DELETE=0 AND UM_CODE='" + User_Name + "'";
        }
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            rptname.Load(Server.MapPath("~/Reports/rptUserMaster.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptUserMaster.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dt);
            rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
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
            CommonClasses.SendError("User Master View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/MasterRegister/VIEW/ViewUserMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
        }
    }
}
