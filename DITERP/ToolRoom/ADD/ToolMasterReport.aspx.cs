using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class ToolRoom_ADD_ToolMasterReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    public static string exe = "0";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
        home1.Attributes["class"] = "active";
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
            // string Type = Request.QueryString[1].ToString();
            string Cond = Request.QueryString[1].ToString();
            // string PartyCode = Request.QueryString[5].ToString();

            GenerateReport(Title, Cond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt2 = new DataTable();

            DataTable dtFilter = new DataTable();

           //  Query = "select T_CODE,T_NAME,T_P_CODE,T_I_CODE,T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,T_PENDTOOLLIFEMONTH,case when T_OWNER=0 then 'PCPL' else 'Customer' end as T_OWNER,T_PMFREQ,T_3D	T_3D_PATH,T_REVNO,convert(varchar,T_REV_DATE,106) as T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,case when T_TYPE=0 then 'DIE' else 'CORE BOX' end as T_TYPE from TOOL_MASTER,ITEM_MASTER,PARTY_MASTER where " + Cond + " I_CODE=TOOL_MASTER.T_I_CODE and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and P_CODE=TOOL_MASTER.T_P_CODE and TOOL_MASTER.T_CM_COMP_ID=1 and TOOL_MASTER.T_STATUS=1 and T_CM_COMP_ID='" + Session["CompanyID"] + "'";


           // Query = " select distinct  I_CODE, T_CODE,T_NAME,T_P_CODE,T_I_CODE,T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,T_PENDTOOLLIFEMONTH,case when T_OWNER=0 then 'PCPL' else 'Customer' end as T_OWNER,T_PMFREQ,T_3D,	T_3D_PATH,T_REVNO,convert(varchar,T_REV_DATE,106) as T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,case when T_TYPE=0 then 'DIE' else 'CORE BOX' end as T_TYPE ,  ISNULL((SELECT CONVERT(varchar, MAX(TRR_DATE),106) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),'') AS ttt , T_PENDTOOLLIFEMONTH-(  DATEDIFF(MM,T_CRE_DATE,getdate())) AS Mlife into #temp from TOOL_MASTER ,ITEM_MASTER,PARTY_MASTER  where  " + Cond + "  TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_P_CODE=P_CODE  and TOOL_MASTER.T_STATUS=1 and T_CM_COMP_ID='" + Session["CompanyID"] + "'    SELECT T_CODE,T_P_CODE,T_I_CODE,T_OWNER,T_PMFREQ,T_3D,T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,T_TYPE,	T_3D_PATH,T_REVNO, T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,  CASE WHEN Mlife<0 then 0 else Mlife END  AS Mlife,T_PENDTOOLLIFEMONTH,I_CODE,T_NAME,ttt ,CASE when ttt='' then (select ISNULL(SUM(IRND_PROD_QTY),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) ELSE  (select ISNULL(SUM(IRND_PROD_QTY),0) from IRN_ENTRY inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE AND IRN_DATE>= ttt) END  AS IRND_PROD_QTY,  CASE when ttt='' then T_PENDTOOLLIFE else    (ISNULL((SELECT ISNULL(SUM(TRR_STD_PROD),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE AND  CONVERT(varchar, (TRR_DATE),106)=ttt),0)) END AS TRR_STD_PROD        into #temp1   FROM #temp  drop table #temp                 SELECT T_CODE,T_P_CODE,T_I_CODE,T_OWNER,T_PMFREQ,T_3D,T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,T_TYPE,T_3D_PATH,T_REVNO,	T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,Mlife AS T_PLIFEMONTH,T_PENDTOOLLIFEMONTH,I_CODE,T_NAME,ttt,	CASE when ( TRR_STD_PROD-IRND_PROD_QTY)<0 then 0 else ( TRR_STD_PROD-IRND_PROD_QTY) END   AS T_PLIFE  FROM  #temp1        DROP TABLE #temp1      ";


            Query = " select distinct  I_CODE, T_CODE,T_NAME,T_P_CODE,T_I_CODE,T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,T_PENDTOOLLIFEMONTH,case when T_OWNER=0 then 'PCPL' else 'Customer' end as T_OWNER,T_PMFREQ,T_3D,	T_3D_PATH,T_REVNO,convert(varchar,T_REV_DATE,106) as T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,case when T_TYPE=0 then 'DIE' else 'CORE BOX' end as T_TYPE ,  ISNULL((SELECT CONVERT(varchar, MAX(TRR_DATE),106) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),'') AS ttt , T_PENDTOOLLIFEMONTH-(  DATEDIFF(MM,T_CRE_DATE,getdate())) AS Mlife , CASE WHEN T_STATUS=1 then 'Active' else 'Inactive' END AS T_TYPENM   into #temp from TOOL_MASTER ,ITEM_MASTER,PARTY_MASTER  where  " + Cond + "  TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_P_CODE=P_CODE   and T_CM_COMP_ID='" + Session["CompanyID"] + "'    SELECT T_CODE,T_P_CODE,T_I_CODE,T_OWNER,T_PMFREQ,T_3D,T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,T_TYPE,	T_3D_PATH,T_REVNO, T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,  CASE WHEN Mlife<0 then 0 else Mlife END  AS Mlife,T_PENDTOOLLIFEMONTH,I_CODE,T_NAME,ttt ,CASE when ttt='' then (select ISNULL(SUM(IRND_PROD_QTY),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) ELSE  (select ISNULL(SUM(IRND_PROD_QTY),0) from IRN_ENTRY inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE AND IRN_DATE>= ttt) END  AS IRND_PROD_QTY,  CASE when ttt='' then T_PENDTOOLLIFE else    (ISNULL((SELECT ISNULL(SUM(TRR_STD_PROD),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE AND  CONVERT(varchar, (TRR_DATE),106)=ttt),0)) END AS TRR_STD_PROD  ,T_TYPENM      into #temp1   FROM #temp  drop table #temp                 SELECT T_CODE,T_P_CODE,T_I_CODE,T_OWNER,T_PMFREQ,T_3D,T_REV_DATE,T_REF_NO,I_CODENO,I_NAME,P_NAME,T_TYPE,T_3D_PATH,T_REVNO,	T_PHOTO_PATH,T_PHOTO,T_TOOLNO,T_STDLIFE,T_PENDTOOLLIFE,Mlife AS T_PLIFEMONTH,T_PENDTOOLLIFEMONTH,I_CODE,T_NAME,ttt,	CASE when ( TRR_STD_PROD-IRND_PROD_QTY)<0 then 0 else ( TRR_STD_PROD-IRND_PROD_QTY) END   AS T_PLIFE,T_TYPENM  FROM  #temp1        DROP TABLE #temp1      ";

            dt2 = CommonClasses.Execute(Query);

            if (dt2.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptToolmasterReport.rpt")); //foundaryRejYearly
                rptname.FileName = Server.MapPath("~/Reports/rptToolmasterReport.rpt"); //foundaryRejYearly
                rptname.Refresh();
                rptname.SetDataSource(dt2);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "Tooling Master Report");
                CrystalReportViewer1.ReportSource = rptname;
                CrystalReportViewer1.ID = Title.Replace(" ", "_");
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
            CommonClasses.SendError("Tooling Master Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/ToolRoom/VIEW/ViewToolMasterReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

