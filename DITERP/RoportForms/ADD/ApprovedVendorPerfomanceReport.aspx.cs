using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class IRN_ADD_ApprovedVendorPerfomanceReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    public static string exe = "0";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";

        //if (Session["PartyCode"].ToString() != "")
        //{
        //    Control c = this.Master.FindControl("Dashboard");
        //    c.Visible = false;

        //    Control c1 = this.Master.FindControl("Masters");
        //    c1.Visible = false;

        //    Control c2 = this.Master.FindControl("Purchase");
        //    c2.Visible = false;

        //    Control c3 = this.Master.FindControl("Production");
        //    c3.Visible = false;

        //    Control c4 = this.Master.FindControl("Sale");
        //    c4.Visible = false;

        //    Control c5 = this.Master.FindControl("Excise");
        //    c5.Visible = false;

        //    Control c6 = this.Master.FindControl("Utility");
        //    c6.Visible = false;

        //    Control c7 = this.Master.FindControl("IRN");
        //    c7.Visible = false;

        //    //Mobile View Menu Hide
        //    Control c8 = this.Master.FindControl("Dashboard1MV");
        //    c8.Visible = false;

        //    Control c9 = this.Master.FindControl("Masters1MV");
        //    c9.Visible = false;

        //    Control c10 = this.Master.FindControl("Purchase1MV");
        //    c10.Visible = false;

        //    Control c11 = this.Master.FindControl("Production1MV");
        //    c11.Visible = false;

        //    Control c12 = this.Master.FindControl("Sale1MV");
        //    c12.Visible = false;

        //    Control c13 = this.Master.FindControl("Excise1MV");
        //    c13.Visible = false;

        //    Control c14 = this.Master.FindControl("Utility1MV");
        //    c14.Visible = false;
        //}
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ViewState["strUser"] = strUser;
            if (!IsPostBack)
            {
                ViewState["strUser"] = "0";
            }
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string Cond = Request.QueryString[3].ToString();
            string PartyCode = Request.QueryString[4].ToString();
            //if (PartyCode == "ALL")
            //{
            //    GenerateReport1(Title, From, Todt, Cond, PartyCode);
            //}
            //else
            //{
            GenerateReport(Title, From, Todt, Cond, PartyCode);
            // }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Approved Vendor Performance Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string Cond, string PartyCode)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            string Query1 = "";
            string Query2 = "";
            string Query3 = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dtLine = new DataTable();
            DataTable dtFilter = new DataTable();

            Query = "SELECT I_CODE,I_CODENO,I_NAME,ISNULL((select ISNULL(SUM(IWD_SQTY),0) from INWARD_MASTER inner join INWARD_DETAIL on IWD_IWM_CODE=IWM_CODE inner join ITEM_MASTER on IWD_I_CODE=I_CODE where INWARD_MASTER.IWM_CODE=IWM.IWM_CODE AND INWARD_DETAIL.IWD_CODE=IWD.IWD_CODE AND IWM_TYPE='OUTCUSTINV' AND I_CAT_CODE=-2147483648 ),0) AS IWD_CH_QTY,P_CODE,P_NAME,DATEPART(dd,IWM_DATE) AS IWM_DATE_DAY,IWM_DATE,isnull((SELECT ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY_R4A FROM IRN_ENTRY,IRN_DETAIL where IRN_DETAIL.IRND_I_CODE= IWD.IWD_I_CODE and IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_DATE BETWEEN '" + From + "' AND '" + Todt + "' AND IRND_TYPE=1 AND IRND_RSM_CODE=-2147483645),0) as IRND_REJ_QTY_R4A,isnull((SELECT ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY_R5 FROM IRN_ENTRY,IRN_DETAIL where IRN_DETAIL.IRND_I_CODE= IWD.IWD_I_CODE and IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_DATE BETWEEN '" + From + "' AND '" + Todt + "' AND IRND_TYPE=1 AND IRND_RSM_CODE=-2147483643),0) as IRND_REJ_QTY_R5,isnull((SELECT ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY_R4 FROM IRN_ENTRY,IRN_DETAIL where IRN_DETAIL.IRND_I_CODE= IWD.IWD_I_CODE and IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_DATE BETWEEN '" + From + "' AND '" + Todt + "' AND IRND_TYPE=1 AND IRND_RSM_CODE=-2147483644),0) as IRND_REJ_QTY_R4 FROM INWARD_MASTER IWM,INWARD_DETAIL IWD,PARTY_MASTER P,ITEM_MASTER I WHERE " + Cond + " IWM.IWM_CODE=IWD_IWM_CODE AND IWM.ES_DELETE=0 and IWM.ES_DELETE=0 AND IWM_P_CODE=P_CODE AND IWD.IWD_I_CODE=I_CODE AND CONVERT(DATE,IWM.IWM_DATE) BETWEEN '" + From + "' AND '" + Todt + "' and P_INHOUSE_IND=1";
            //Inward Qty
            // Query = "SELECT I_CODENO,I_NAME,ISNULL(SUM(IWD_SQTY),0) AS IWD_CH_QTY,P_CODE,P_NAME ,DATEPART(dd, IWM_DATE) AS IWM_DATE_DAY,IWM_DATE FROM INWARD_MASTER,INWARD_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND IWM_P_CODE=P_CODE  AND IWD_I_CODE=I_CODE AND IWM_DATE BETWEEN '01-Jun-2018' AND '30-Jun-2018' AND IWM_TYPE='OUTCUSTINV' AND I_CAT_CODE=-2147483648 GROUP BY P_CODE,DATEPART(dd, IWM_DATE),IWM_DATE,P_CODE,P_NAME,I_CODENO,I_NAME";

            ////inward inspection vendor --R4 A --2147483645 REJECTIONSTAGE_MASTER Code
            //Query1 = "SELECT I_CODENO,I_NAME,ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY_R4A, P_CODE,P_NAME ,DATEPART(DD,IRN_DATE) AS IWM_DATE_DAY,IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '01-Jun-2018' AND '30-Jun-2018' AND IRND_TYPE=1 AND IRND_RSM_CODE=-2147483645 GROUP BY P_CODE,P_NAME,DATEPART(DD,IRN_DATE),IRN_DATE,I_CODENO,I_NAME";

            ////final inspection --R5--2147483643 REJECTIONSTAGE_MASTER Code
            //Query2 = "SELECT I_CODENO,I_NAME,ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY_R5, P_CODE,P_NAME ,  DATEPART(DD,IRN_DATE) AS IWM_DATE_DAY,IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '01-Jun-2018' AND '30-Jun-2018' AND IRND_TYPE=1 AND IRND_RSM_CODE=-2147483643 GROUP BY P_CODE,P_NAME,DATEPART(DD,IRN_DATE),IRN_DATE,I_CODENO,I_NAME";

            ////machine shop  --R4 --2147483644 REJECTIONSTAGE_MASTER Code
            //Query3 = "SELECT I_CODENO,I_NAME,ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY_R4, P_CODE,P_NAME,DATEPART(DD,IRN_DATE) AS IWM_DATE_DAY,IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '01-Jun-2018' AND '30-Jun-2018' AND IRND_TYPE=1 AND IRND_RSM_CODE=-2147483644 GROUP BY P_CODE,P_NAME,DATEPART(DD,IRN_DATE),IRN_DATE,I_CODENO,I_NAME";

            dt = CommonClasses.Execute(Query);
            //dt1 = CommonClasses.Execute(Query1);
            //dt2 = CommonClasses.Execute(Query2);
            //dt3 = CommonClasses.Execute(Query3);

            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptApprovedVendorPerformance.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptApprovedVendorPerformance.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "Approved Vendor Performance Report From " + Convert.ToDateTime(From).ToString("dd MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("dd MMM yyyy"));
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
            CommonClasses.SendError("Approved Vendor Performance Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewApprovedVendorPerfomanceReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Approved Vendor Performance Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
