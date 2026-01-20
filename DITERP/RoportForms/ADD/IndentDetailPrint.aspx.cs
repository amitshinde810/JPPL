using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_IndentDetailPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Type = "";
    string PrintType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Code = Request.QueryString[1];


            PrintType = Request.QueryString[2].ToString();

            GenerateReport(Title, Code, PrintType);
        }
        catch (Exception Ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = Ex.Message;
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Code, string PrintType1)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataTable dtindent = new DataTable();

            dtindent = CommonClasses.Execute("SELECT INM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS IN_TYPE,convert(varchar,IN_DATE,106) as IN_DATE,IN_TNO,  ISNULL((SELECT PROCM_NAME  FROM PROJECT_CODE_MASTER where ES_DELETE=0 AND PROCM_CODE=IN_PROJECT),'' ) AS IN_PRO_CODE,DM_NAME,IN_SUPP_NAME,IN_CONTACT,IN_ADDRESS,IN_EMAIL,IN_MOB,IND_DESC,IND_RATE,IND_QTY,IND_AMT,IN_PAYMENT,UM_NAME AS  UM_USERNAME,IN_APPROVE, CASE WHEN ISNULL(IN_APPROVE,0)=1 then 'TRUE' ELSE 'FALSE' END as IN_APPROVE, (SELECT UM_NAME FROM USER_MASTER where UM_CODE=IN_APPROVEBY)  As IN_APPROVEBY, CASE WHEN ISNULL(IN_AUTHORIZE,0)=1 then 'TRUE' ELSE 'FALSE' END as IN_AUTHORIZE, (SELECT UM_NAME FROM USER_MASTER where UM_CODE=IN_AUTHORIZEBY)  As IN_AUTHORIZEBY ,CASE WHEN ISNULL(IN_AUTHORIZE1,0)=1 then 'TRUE' ELSE 'FALSE' END as IN_AUTHORIZE1, (SELECT UM_NAME FROM USER_MASTER where UM_CODE=IN_AUTHORIZEBY1)  As IN_AUTHORIZEBY1   FROM  INDENT_MASTER,INDENT_TYPE_MASTER,DEPARTMENT_MASTER,USER_MASTER,INDENT_DETAIL WHERE INDENT_MASTER.ES_DELETE=0 AND IN_TYPE=IM_CODE AND IN_DEPT=DM_CODE and IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND IN_USER=UM_CODE AND INM_CODE=IND_INM_CODE and INM_CODE='" + Code + "'");

            ReportDocument rptname = null;
            rptname = new ReportDocument();

            #region Count
            if (dtindent.Rows.Count > 0)
            {
                rptname.Load(Server.MapPath("~/Reports/IndentDetailPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/IndentDetailPrint.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtindent);
                rptname.SetParameterValue("txttitle", Session["CompanyName"].ToString());
                rptname.SetParameterValue("CompAdd", Session["CompanyAdd"].ToString());
                CrystalReportViewer1.ReportSource = rptname;

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail Print", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region Cancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
        }
        catch (Exception ex)
        {

            CommonClasses.SendError("Indent Detail Print", "btnCancel_Click", ex.Message);
        }
    }
    #endregion
}
