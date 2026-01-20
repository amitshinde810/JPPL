﻿﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;

public partial class IRN_ADD_InternalCAPAReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];

            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string cond = "";
            GenerateReport(Title, From, Todt, cond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Internal CAPA Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = " SELECT DISTINCT I_CODE,I_CODENO,I_NAME,LM_CODE AS STG_CODE,LM_NAME AS STG_NAME,LC_DATE AS SI_DATE,LC_CODE AS SI_CODE,LC_NO AS SI_NO ,LC_ACTIVE AS SI_ACTIVE FROM LINE_MASTER INNER JOIN  LINE_CHANGE ON LINE_MASTER.LM_CODE = LINE_CHANGE.LC_LM_CODE INNER JOIN  ITEM_MASTER ON LINE_CHANGE.LC_I_CODE = ITEM_MASTER.I_CODE WHERE     (LINE_MASTER.ES_DELETE = 0) AND LC_DATE  BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO, LC_ACTIVE DESC,LC_DATE DESC,LC_CODE desc";

            DL_DBAccess = new DatabaseAccessLayer();
            DataSet ds = new DataSet("MonthyData");
            SqlParameter[] Params = 
			    { 
                    new SqlParameter("@StartDate",null),
				    new SqlParameter("@EndDate",null),
               
                 };
            ds = DL_DBAccess.SelectDataDataset("GetMonthyData", Params);




            for (int i = 1; i < ds.Tables.Count; i++)
            {
                ds.Tables[i].TableName = ds.Tables[0].Rows[i - 1].ItemArray[0].ToString();
            }
            DataTable dt = new DataTable();

            DataTable dtInsertnalReject = new DataTable();
            dtInsertnalReject = CommonClasses.Execute("SELECT   isnull(jan,'') as jan,isnull( feb,'') as  feb,isnull( mar,'') as  mar,isnull( apr,'') as  apr,isnull( may,'') as  may,isnull( jun,'') as  jun,isnull( jul,'') as  jul,isnull( aug,'') as  aug,isnull( sep,'') as  sep,isnull( oct,'') as  oct,isnull( nov,'') as  nov,isnull( dec,'') as  dec FROM ( SELECT DISTINCT ISNULL( LEFT(DATENAME(month, INTERNAL_REJECTION_CAPA_MASTER.IRCM_DATE), 3),'') AS month, ISNULL( I_NAME,'') as I_CODE,  I_CODENO+' - '+I_NAME AS  I_CODENO   FROM INTERNAL_REJECTION_CAPA_DETAIL INNER JOIN INTERNAL_REJECTION_CAPA_MASTER ON INTERNAL_REJECTION_CAPA_DETAIL.IRCD_IRCM_CODE = INTERNAL_REJECTION_CAPA_MASTER.IRCM_CODE INNER JOIN ITEM_MASTER ON INTERNAL_REJECTION_CAPA_DETAIL.IRCD_I_CODE = ITEM_MASTER.I_CODE WHERE (INTERNAL_REJECTION_CAPA_MASTER.ES_DELETE = 0) AND (INTERNAL_REJECTION_CAPA_MASTER.IRCM_DATE BETWEEN '01/APR/2018' AND '31/MAR/2019') ) as s PIVOT ( MAX(I_CODENO) FOR [month] IN (jan, feb, mar, apr,may, jun, jul, aug, sep, oct, nov, dec) )AS pvt ORDER BY jan DESC, feb DESC, mar DESC, apr DESC,may DESC, jun DESC, jul DESC, aug DESC, sep DESC, oct DESC, nov DESC, dec DESC");

            for (int i = 0; i < dtInsertnalReject.Rows.Count; i++)
            {

            }
            if (dtInsertnalReject.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/internalrejectyr.rpt"));   //InternalRej // rptStageChange //rpt8020CompSpecRejPer
                rptname.FileName = Server.MapPath("~/Reports/internalrejectyr.rpt"); //InternalRej // rptStageChange  //rpt8020CompSpecRejPer
                rptname.Refresh();
                rptname.SetDataSource(dtInsertnalReject);
                //rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
                //rptname.SetParameterValue("Title", "Internal CAPA Report From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "  To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                CrystalReportViewer1.ReportSource = rptname;
                //CrystalReportViewer1.ID = Title.Replace(" ", "_");
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
            CommonClasses.SendError("Internal CAPA Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/ViewInternalCAPAReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Internal CAPA Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

