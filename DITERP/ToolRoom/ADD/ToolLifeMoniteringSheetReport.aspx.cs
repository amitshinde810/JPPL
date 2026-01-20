using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class ToolRoom_ADD_ToolLifeMoniteringSheetReport : System.Web.UI.Page
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
            string Cond = Request.QueryString[1].ToString();
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();

            GenerateReport(Title, Cond, From, To);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Cond, string From, string To)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DataTable dt2 = new DataTable();

            DataTable dtFilter = new DataTable();

            DateTime dtEDate = Convert.ToDateTime(To);

            // Query = "SELECT TOOL_MASTER.T_CODE,TOOL_MASTER.T_NAME,TOOL_MASTER.T_TYPE,TOOL_MASTER.T_STDLIFE,TOOL_MASTER.T_PENDTOOLLIFE,TOOL_MASTER.T_PENDTOOLLIFEMONTH,CASE WHEN T_OWNER=0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER,TOOL_MASTER.T_PMFREQ,TOOL_MASTER.T_REVNO,CONVERT(varchar,TOOL_MASTER.T_REV_DATE,106) AS T_REV_DATE, TOOL_MASTER.T_REF_NO,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,PARTY_MASTER.P_NAME,CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, SUM(IRND_PROD_QTY) AS IRND_PROD_QTY,LEFT(DATENAME(MONTH, IRN_DATE),3) AS MONTH,DATEPART(YYYY, IRN_DATE) AS Year FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE INNER JOIN TOOL_MASTER INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE ON IRN_DETAIL.IRND_T_CODE = TOOL_MASTER.T_CODE WHERE " + Cond + " (TOOL_MASTER.ES_DELETE = 0) and T_CM_COMP_ID='" + Session["CompanyID"] + "' AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (TOOL_MASTER.T_STATUS = 1) AND (TOOL_MASTER.T_CM_COMP_ID = 1) AND (IRN_ENTRY.ES_DELETE = 0) GROUP BY TOOL_MASTER.T_CODE, TOOL_MASTER.T_NAME, TOOL_MASTER.T_TYPE,TOOL_MASTER.T_STDLIFE, TOOL_MASTER.T_PENDTOOLLIFE,TOOL_MASTER.T_PENDTOOLLIFEMONTH, TOOL_MASTER.T_OWNER, TOOL_MASTER.T_PMFREQ, TOOL_MASTER.T_REVNO,T_REV_DATE, TOOL_MASTER.T_REF_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, T_TYPE, DATENAME(MM,IRN_DATE),DATEPART(YYYY,IRN_DATE)";
            //Query = "SELECT TOOL_MASTER.T_CODE, TOOL_MASTER.T_NAME, TOOL_MASTER.T_TYPE,TOOL_MASTER.T_STDLIFE, TOOL_MASTER.T_PENDTOOLLIFE, TOOL_MASTER.T_PENDTOOLLIFEMONTH, CASE WHEN T_OWNER = 0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER, TOOL_MASTER.T_PMFREQ, TOOL_MASTER.T_REVNO, CONVERT(varchar, TOOL_MASTER.T_REV_DATE, 106) AS T_REV_DATE, TOOL_MASTER.T_REF_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, GETDATE() AS dtfDate,0 As opTool ,0 as prod,0 As PtoolLife FROM TOOL_MASTER INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE WHERE  " + Cond + "   (TOOL_MASTER.ES_DELETE = 0) and T_CM_COMP_ID='" + Session["CompanyID"] + "' AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (TOOL_MASTER.T_STATUS = 1) AND (TOOL_MASTER.T_CM_COMP_ID = '" + Session["CompanyID"] + "') ";

            /*Refurbish Life add in Pending Life And show Refurbish number on report*/
            //Query = "SELECT T.T_CODE, T.T_NAME, T.T_TYPE,T.T_STDLIFE,  isnull((T.T_PENDTOOLLIFE) + ISNULL((select ISNULL(TRR_STD_PROD,0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0),0) as  T_PENDTOOLLIFE,ISNULL((select ISNULL(TRR_REF_REV_NO,0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0) AS T_REF_NO, T.T_PENDTOOLLIFEMONTH, CASE WHEN T_OWNER = 0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER, T.T_PMFREQ, T.T_REVNO, CONVERT(varchar, T.T_REV_DATE, 106) AS T_REV_DATE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, GETDATE() AS dtfDate,0 As opTool ,0 as prod,0 As PtoolLife FROM TOOL_MASTER T INNER JOIN ITEM_MASTER ON T.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON T.T_P_CODE = PARTY_MASTER.P_CODE WHERE " + Cond + " (T.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (T.T_STATUS = 1) AND (T.T_CM_COMP_ID = '" + Session["CompanyID"] + "') ";

            /*Logic By Sanket Sir (22082018):- adding Pening life of tool in months as per logic of every prod month*/
            // Query = "SELECT T.T_CODE, T.T_NAME, T.T_TYPE,T.T_STDLIFE,  isnull((T.T_PENDTOOLLIFE) + ISNULL((select ISNULL(SUM(TRR_STD_PROD),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0),0) as  T_PENDTOOLLIFE,ISNULL((select ISNULL(MAX(TRR_REF_REV_NO),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0) AS T_REF_NO, T.T_PENDTOOLLIFEMONTH as T_PENDTOOLLIFEMONTH, CASE WHEN T_OWNER = 0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER, T.T_PMFREQ, T.T_REVNO, CONVERT(varchar, T.T_REV_DATE, 106) AS T_REV_DATE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, GETDATE() AS dtfDate,0 As opTool ,0 as prod,0 As PtoolLife   ,ISNULL((SELECT COUNT(DISTINCT (  datename(m,IRN_DATE)+' '+cast(datepart(yyyy,IRN_DATE) as varchar) )) as MonthYear  FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_T_CODE= T.T_CODE AND  IRN_DATE <='" + (dtEDate.AddMonths(1).AddDays(-1)).ToString("dd/MMM/yyyy") + "'),0) AS PEND_MONTH FROM TOOL_MASTER T INNER JOIN ITEM_MASTER ON T.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON T.T_P_CODE = PARTY_MASTER.P_CODE WHERE " + Cond + " (T.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (T.T_STATUS = 1) AND (T.T_CM_COMP_ID = '" + Session["CompanyID"] + "') ";

            //Query = "SELECT T.T_CODE, T.T_NAME, T.T_TYPE,T.T_STDLIFE,  isnull((T.T_PENDTOOLLIFE) + ISNULL((select ISNULL(SUM(TRR_STD_PROD),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0),0) as  T_PENDTOOLLIFE,ISNULL((select ISNULL(MAX(TRR_REF_REV_NO),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0) AS T_REF_NO, T.T_PENDTOOLLIFEMONTH as T_PENDTOOLLIFEMONTH, CASE WHEN T_OWNER = 0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER, T.T_PMFREQ, T.T_REVNO, CONVERT(varchar, T.T_REV_DATE, 106) AS T_REV_DATE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, GETDATE() AS dtfDate,0 As opTool ,0 as prod,0 As PtoolLife   , ISNULL(DATEDIFF(MM,T_CRE_DATE,getdate()),0)    AS PEND_MONTH FROM TOOL_MASTER T INNER JOIN ITEM_MASTER ON T.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON T.T_P_CODE = PARTY_MASTER.P_CODE WHERE " + Cond + " (T.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (T.T_STATUS = 1) AND (T.T_CM_COMP_ID = '" + Session["CompanyID"] + "') ";

            //Query = "SELECT T.T_CODE, T.T_NAME, T.T_TYPE,T.T_STDLIFE,  isnull((T.T_PENDTOOLLIFE),0) as  T_PENDTOOLLIFE,ISNULL((select ISNULL(MAX(TRR_REF_REV_NO),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0) AS T_REF_NO, T.T_PENDTOOLLIFEMONTH as T_PENDTOOLLIFEMONTH, CASE WHEN T_OWNER = 0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER, T.T_PMFREQ, T.T_REVNO, CONVERT(varchar, T.T_REV_DATE, 106) AS T_REV_DATE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, GETDATE() AS dtfDate,0 As opTool ,0 as prod,0 As PtoolLife   , ISNULL(DATEDIFF(MM,T_CRE_DATE,getdate()),0)    AS PEND_MONTH FROM TOOL_MASTER T INNER JOIN ITEM_MASTER ON T.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON T.T_P_CODE = PARTY_MASTER.P_CODE WHERE " + Cond + " (T.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (T.T_STATUS = 1) AND (T.T_CM_COMP_ID = '" + Session["CompanyID"] + "') ";

            Query = "SELECT T.T_CODE, T.T_NAME, T.T_TYPE,T.T_STDLIFE,  isnull((T.T_PENDTOOLLIFE),0) as  T_PENDTOOLLIFE,    ISNULL((select ISNULL(SUM(TRR_STD_PROD),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE AND TRR_DATE<='" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'),0) AS  T_REFUBLLIFE , ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) AS IRND_PROD_QTY    FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_TYPE=0 AND  IRN_DATE <='" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'   AND IRND_T_CODE =T.T_CODE AND IRND_TYPE=1),0) AS T_PROD,  ISNULL((select ISNULL(MAX(TRR_REF_REV_NO),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T.T_CODE),0) AS T_REF_NO, T.T_PENDTOOLLIFEMONTH as T_PENDTOOLLIFEMONTH, CASE WHEN T_OWNER = 0 THEN 'PCPL' ELSE 'Customer' END AS T_OWNER, T.T_PMFREQ, T.T_REVNO, CONVERT(varchar, T.T_REV_DATE, 106) AS T_REV_DATE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME, CASE WHEN T_TYPE = 0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPENM, GETDATE() AS dtfDate,0 As opTool ,0 as prod,0 As PtoolLife   , ISNULL(DATEDIFF(MM,T_CRE_DATE,getdate()),0)    AS PEND_MONTH     into #temp  FROM TOOL_MASTER T INNER JOIN ITEM_MASTER ON T.T_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON T.T_P_CODE = PARTY_MASTER.P_CODE WHERE " + Cond + " (T.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (T.T_STATUS = 1) AND (T.T_CM_COMP_ID = '" + Session["CompanyID"] + "')  SELECT T_CODE,	T_NAME,	T_TYPE,	T_STDLIFE,	T_PENDTOOLLIFE,      (T_PENDTOOLLIFE+	T_REFUBLLIFE-	T_PROD)	 AS T_PLIFE,  T_REF_NO,	T_PENDTOOLLIFEMONTH,	T_OWNER,	T_PMFREQ,	T_REVNO,	T_REV_DATE,	I_CODENO,	I_NAME,	P_NAME	,T_TYPENM	,dtfDate,	opTool,	prod,	PtoolLife,	PEND_MONTH FROM #temp DROP TABLE #temp";

            dt2 = CommonClasses.Execute(Query);

            if (dt2.Rows.Count > 0)
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_STDLIFE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_PENDTOOLLIFE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_PENDTOOLLIFEMONTH", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_OWNER", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_PMFREQ", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_REVNO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_REV_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_REF_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPENM", typeof(String)));

                    dtFilter.Columns.Add(new System.Data.DataColumn("MONTH", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Year", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("opTool", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("prod", typeof(String)));

                    dtFilter.Columns.Add(new System.Data.DataColumn("PtoolLife", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PEND_MONTH", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRefubrish", typeof(String)));
                }

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    //double PtoolLife = Convert.ToDouble(dt2.Rows[i]["T_PENDTOOLLIFE"].ToString());
                    double PtoolLife = 0;

                    DataTable dttool = new DataTable();
                    DateTime dttooldate = new DateTime();
                    dttool = CommonClasses.Execute("SELECT  ISNULL(MAX(TRR_DATE),GETDATE())  AS TRR_DATE FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0  AND TRR_T_CODE ='" + dt2.Rows[i]["T_CODE"].ToString() + "'  AND TRR_DATE<='" + Convert.ToDateTime(Convert.ToDateTime(From).ToString("dd/MMM/yyyy")).ToString("dd/MMM/yyyy") + "' ");
                    if (dttool.Rows.Count > 0)
                    {
                        dttooldate = Convert.ToDateTime(dttool.Rows[0]["TRR_DATE"].ToString());
                        if (dttooldate <= Convert.ToDateTime(Convert.ToDateTime(From).ToString("dd/MMM/yyyy")))
                        {
                              DataTable dtprod = CommonClasses.Execute(" SELECT  T_CODE , ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) AS IRND_PROD_QTY    FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_TYPE=0 AND  IRN_DATE >='" + Convert.ToDateTime(dttool.Rows[0]["TRR_DATE"].ToString()).ToString("dd/MMM/yyyy") + "'   AND DATEPART(MM,IRN_DATE)='" + Convert.ToDateTime(dttool.Rows[0]["TRR_DATE"].ToString()).Month + "' AND DATEPART(YYYY,IRN_DATE)='" + Convert.ToDateTime(dttool.Rows[0]["TRR_DATE"].ToString()).Year + "'   AND IRND_T_CODE =T_CODE AND IRND_TYPE=1 ),0) AS  IRND_PROD_QTY,  ISNULL((select  top 1  ISNULL((TRR_STD_PROD),0) from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 and TRR_T_CODE=T_CODE   AND CONVERT(VARCHAR, TRR_DATE,106) <='" + Convert.ToDateTime(dttool.Rows[0]["TRR_DATE"].ToString()).ToString("dd MMM yyyy") + "'  ORDER BY TRR_REF_REV_NO DESC ),0)   AS  TRR_STD_PROD  FROM TOOL_MASTER  where T_CODE='" + dt2.Rows[i]["T_CODE"].ToString() + "'  ");
                            if (dtprod.Rows.Count > 0)
                            {
                                PtoolLife = Convert.ToDouble(dtprod.Rows[0]["TRR_STD_PROD"].ToString()) - Convert.ToDouble(dtprod.Rows[0]["IRND_PROD_QTY"].ToString());
                            }
                        }
                        else
                        {
                            PtoolLife = Convert.ToDouble(dt2.Rows[i]["T_PLIFE"].ToString());
                        }
                    }
                    else
                    {
                        PtoolLife = Convert.ToDouble(dt2.Rows[i]["T_PLIFE"].ToString());
                    }


                    double opTool = 0;
                    double Prefub = 0;
                    DateTime dtfDate = Convert.ToDateTime(From);


                    for (int j = 0; dtfDate <= dtEDate; j++)
                    {
                        DataTable dtRefub = CommonClasses.Execute("SELECT ISNULL(SUM(TRR_STD_PROD),0) AS TRR_STD_PROD ,TRR_REF_REV_NO , TRR_DATE FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND   DATEPART(mm,TRR_DATE)='" + dtfDate.Month + "' AND DATEPART(YYYY,TRR_DATE)='" + dtfDate.Year + "' AND TRR_T_CODE ='" + dt2.Rows[i]["T_CODE"].ToString() + "' GROUP BY TRR_DATE,TRR_REF_REV_NO  ");
                        double refub = 0;
                        double refNo = 0;
                        if (dtRefub.Rows.Count > 0)
                        {
                            refub = Convert.ToDouble(dtRefub.Rows[0]["TRR_STD_PROD"].ToString());
                            refNo = Convert.ToDouble(dtRefub.Rows[0]["TRR_REF_REV_NO"].ToString());
                        }
                        DataTable dt = new DataTable();
                        DataTable dt1 = new DataTable();
                        if (refub > 0)
                        {
                            dt = CommonClasses.Execute("SELECT ISNULL(SUM(IRND_PROD_QTY),0) AS IRND_PROD_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_TYPE=0 AND DATEPART(mm,IRN_DATE)='" + dtfDate.Month + "' AND DATEPART(YYYY,IRN_DATE)='" + dtfDate.Year + "' AND    IRN_DATE<'" + Convert.ToDateTime(dtRefub.Rows[0]["TRR_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND IRND_T_CODE ='" + dt2.Rows[i]["T_CODE"].ToString() + "' AND IRND_TYPE=1");
                            dt1 = CommonClasses.Execute("SELECT ISNULL(SUM(IRND_PROD_QTY),0) AS IRND_PROD_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_TYPE=0 AND DATEPART(mm,IRN_DATE)='" + dtfDate.Month + "' AND DATEPART(YYYY,IRN_DATE)='" + dtfDate.Year + "' AND    IRN_DATE>='" + Convert.ToDateTime(dtRefub.Rows[0]["TRR_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND IRND_T_CODE ='" + dt2.Rows[i]["T_CODE"].ToString() + "' AND IRND_TYPE=1");

                        }
                        else
                        {
                            dt = CommonClasses.Execute("SELECT ISNULL(SUM(IRND_PROD_QTY),0) AS IRND_PROD_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_TYPE=0 AND DATEPART(mm,IRN_DATE)='" + dtfDate.Month + "' AND DATEPART(YYYY,IRN_DATE)='" + dtfDate.Year + "' AND IRND_T_CODE ='" + dt2.Rows[i]["T_CODE"].ToString() + "' AND IRND_TYPE=1");

                        }
                        double prod = 0;
                        if (dt.Rows.Count > 0)
                        {
                            prod = Convert.ToDouble(dt.Rows[0]["IRND_PROD_QTY"].ToString());
                        }
                        double prod1 = 0;
                        if (dt1.Rows.Count > 0)
                        {
                            prod1 = Convert.ToDouble(dt1.Rows[0]["IRND_PROD_QTY"].ToString());
                        }

                        if (j != 0)
                        {
                            PtoolLife = PtoolLife - prod;
                            if (PtoolLife < 0)
                            {
                                PtoolLife = 0;
                            }
                            if (refub > 0)
                            {
                                PtoolLife = PtoolLife + refub - prod1;
                            }
                            if (PtoolLife < 0)
                            {
                                PtoolLife = 0;
                            }

                            //opTool = PtoolLife - prod;
                        }
                        else
                        {
                            PtoolLife = PtoolLife - prod;
                            if (PtoolLife < 0)
                            {
                                PtoolLife = 0;
                            }
                            if (refub > 0)
                            {
                                PtoolLife = PtoolLife + refub - prod1;
                            }
                            if (PtoolLife < 0)
                            {
                                PtoolLife = 0;
                            }
                        }
                        dtFilter.Rows.Add(dt2.Rows[i]["T_CODE"],
                        dt2.Rows[i]["T_NAME"],
                        dt2.Rows[i]["T_TYPE"],
                        dt2.Rows[i]["T_STDLIFE"],
                       // dt2.Rows[i]["T_PENDTOOLLIFE"],
                       PtoolLife,
                        dt2.Rows[i]["T_PENDTOOLLIFEMONTH"],
                        dt2.Rows[i]["T_OWNER"],
                        dt2.Rows[i]["T_PMFREQ"],
                        dt2.Rows[i]["T_REVNO"],
                        dt2.Rows[i]["T_REV_DATE"],
                        dt2.Rows[i]["T_REF_NO"],
                        dt2.Rows[i]["I_CODENO"],
                        dt2.Rows[i]["I_NAME"],
                        dt2.Rows[i]["P_NAME"],
                        dt2.Rows[i]["T_TYPENM"],
                        dtfDate.ToString("MMM"), dtfDate.Year.ToString().Substring(2, 2), opTool, prod + prod1, PtoolLife, dt2.Rows[i]["PEND_MONTH"], refNo);

                        opTool = PtoolLife;
                        Prefub = refub;

                        dtfDate = dtfDate.AddMonths(1);
                    }
                }

                DataTable dtAll = new DataTable();

                dtAll = dtFilter.Copy();

                //dtAll.Merge(dttest,true);
                // dtAll.Merge(dt2, true, MissingSchemaAction.Ignore);

                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptToolLifeMonitoringSheet.rpt")); //foundaryRejYearly
                rptname.FileName = Server.MapPath("~/Reports/rptToolLifeMonitoringSheet.rpt"); //foundaryRejYearly
                rptname.Refresh();
                rptname.SetDataSource(dtAll);
                rptname.SetParameterValue("txtcompany", Session["CompanyName"].ToString());// From" + From + "To " + To + "
                rptname.SetParameterValue("txtPeriod", "TOOL LIFE MONITORING SHEET");// From" + From + "To " + To + "
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
            Response.Redirect("~/ToolRoom/VIEW/ViewToolLifeMoniteringSheetReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

