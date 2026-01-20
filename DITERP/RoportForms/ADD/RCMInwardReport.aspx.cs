using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_RCMInwardReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Cond = "", Todt = "", Fromdt = "", Title = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise1MV");
        home1.Attributes["class"] = "active"; 
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Title = Request.QueryString[0];
        Fromdt = Request.QueryString[1];
        Todt = Request.QueryString[2];
        Cond = Request.QueryString[3];

        GenerateReport(Title, Fromdt, Todt, Cond);
    }

    #region GenerateReport
    private void GenerateReport(string Title, string Fromdt, string Todt, string Cond)
    {
        DL_DBAccess = new DatabaseAccessLayer();

        DataTable dtfinal = CommonClasses.Execute("SELECT distinct I_CODE,I_NAME,I_CODENO,P_NAME,IWD_SQTY,IWD_RATE,IWM_NO,CPOM_PONO,convert(varchar,IWM_DATE,106) as IWM_DATE,P_GST_NO,E_BASIC,E_EDU_CESS,E_H_EDU,E_TARIFF_NO FROM PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL,CUSTPO_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER where P_LBT_IND=0 and INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_TYPE='IWIM' and PARTY_MASTER.P_CODE=INWARD_MASTER.IWM_P_CODE and PARTY_MASTER.ES_DELETE=0 and CUSTPO_MASTER.CPOM_CODE=INWARD_DETAIL.IWD_CPOM_CODE and IWM_CODE=IWD_IWM_CODE and I_CODE=IWD_I_CODE and E_CODE=I_E_CODE and ITEM_MASTER.ES_DELETE=0");
        if (dtfinal.Rows.Count > 0)
        {
        }
        try
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            rptname.Load(Server.MapPath("~/Reports/rptRCMInward.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptRCMInward.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtfinal);
            rptname.SetParameterValue("txtTitle", Title.ToString());
            rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
            CrystalReportViewer1.ReportSource = rptname;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("RCM Print", "GenerateReport", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RoportForms/VIEW/RCMInwardReport.aspx", false);
    }
    #endregion
}
