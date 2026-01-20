﻿﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_VendorRejYearly : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    public static string exe = "0";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";

        if (Session["PartyCode"].ToString() != "")
        {
            Control c = this.Master.FindControl("Dashboard");
            c.Visible = false;

            Control c1 = this.Master.FindControl("Masters");
            c1.Visible = false;

            Control c2 = this.Master.FindControl("Purchase");
            c2.Visible = false;

            Control c3 = this.Master.FindControl("Production");
            c3.Visible = false;

            Control c4 = this.Master.FindControl("Sale");
            c4.Visible = false;

            Control c5 = this.Master.FindControl("Excise");
            c5.Visible = false;

            Control c6 = this.Master.FindControl("Utility");
            c6.Visible = false;

            Control c7 = this.Master.FindControl("IRN");
            c7.Visible = false;

            //Mobile View Menu Hide
            Control c8 = this.Master.FindControl("Dashboard1MV");
            c8.Visible = false;

            Control c9 = this.Master.FindControl("Masters1MV");
            c9.Visible = false;

            Control c10 = this.Master.FindControl("Purchase1MV");
            c10.Visible = false;

            Control c11 = this.Master.FindControl("Production1MV");
            c11.Visible = false;

            Control c12 = this.Master.FindControl("Sale1MV");
            c12.Visible = false;

            Control c13 = this.Master.FindControl("Excise1MV");
            c13.Visible = false;

            Control c14 = this.Master.FindControl("Utility1MV");
            c14.Visible = false;
        }
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
            string Type = Request.QueryString[3].ToString();
            string Cond = Request.QueryString[4].ToString();
            string PartyCode = Request.QueryString[5].ToString();
            if (PartyCode == "ALL")
            {
                GenerateReport1(Title, From, Todt, Type, Cond, PartyCode);
            }
            else
            {
                GenerateReport(Title, From, Todt, Type, Cond, PartyCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string Type, string Cond, string PartyCode)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            string Query1 = "";
            string Query2 = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtLine = new DataTable();

            if (Type == "0")
            {
                DataTable dtFilter = new DataTable();

                #region MyRegion
                dtFilter.Columns.Add(new System.Data.DataColumn("1", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("2", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("3", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("4", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("5", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("6", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("7", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("8", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("9", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("10", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("11", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("12", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("13", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("14", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("15", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("16", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("17", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("18", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("19", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("20", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("21", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("22", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("23", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("24", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("25", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("26", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("27", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("28", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("29", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("30", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("31", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("TYPES", typeof(string)));
                #endregion

                Query = " (select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], I_CODE,I_CODENO,I_NAME ,P_CODE,P_NAME ,'1'     AS TYPES from (SELECT I_CODE,I_CODENO,I_NAME,ISNULL(SUM(IWD_SQTY),0) AS IWD_CH_QTY,P_CODE,P_NAME ,DATEPART(dd, IWM_DATE) AS IWM_DATE_DAY,IWM_DATE FROM INWARD_MASTER,INWARD_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=P_CODE  AND IWD_I_CODE=I_CODE AND  IWM_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IWM_P_CODE='" + PartyCode + "' AND " + Cond + "  IWM_TYPE='OUTCUSTINV' AND I_CAT_CODE=-2147483648 GROUP BY  I_CODE,I_CODENO,I_NAME,P_CODE,DATEPART(dd, IWM_DATE),IWM_DATE,P_CODE,P_NAME    )   AS SOURCETABLE PIVOT (sum(IWD_CH_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY I_CODE,I_CODENO,I_NAME  ,P_CODE,P_NAME    )   ORDER BY P_NAME,I_CODENO  ,TYPES     ";

                Query1 = " (select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], I_CODE,I_CODENO,I_NAME ,P_CODE,P_NAME,'2' AS TYPES  from ( SELECT I_CODE,I_CODENO,I_NAME ,ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY, P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) AS IWM_DATE_DAY,   IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0  AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND   IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRND_P_CODE='" + PartyCode + "' AND " + Cond + "  IRND_TYPE=1  AND IRND_RSM_CODE=-2147483645 GROUP BY I_CODE,I_CODENO,I_NAME ,P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) ,   IRN_DATE   )   AS aSOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY I_CODE,I_CODENO,I_NAME  ,P_CODE,P_NAME )   ORDER BY P_NAME,I_CODENO  ,TYPES     ";

                Query2 = " (select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31], I_CODE,I_CODENO,I_NAME ,P_CODE,P_NAME,'3' AS TYPES  from (         SELECT I_CODE,I_CODENO,I_NAME ,ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY, P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) AS IWM_DATE_DAY,   IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0  AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND   IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRND_P_CODE='" + PartyCode + "' AND " + Cond + " IRND_TYPE=1 AND IRND_RSM_CODE=-2147483643  GROUP BY I_CODE,I_CODENO,I_NAME ,P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) ,   IRN_DATE   )   AS aSOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY I_CODE,I_CODENO,I_NAME  ,P_CODE,P_NAME    )   ORDER BY P_NAME,I_CODENO  ,TYPES     ";

                dt = CommonClasses.Execute(Query);
                dt1 = CommonClasses.Execute(Query1);
                dt2 = CommonClasses.Execute(Query2);
                string strRes1 = "";

                #region MyRegion
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtFilter.Rows.Add(dt.Rows[i]["1"].ToString(), dt.Rows[i]["2"].ToString(), dt.Rows[i]["3"].ToString(), dt.Rows[i]["4"].ToString(),
                                              dt.Rows[i]["5"].ToString(), dt.Rows[i]["6"].ToString(), dt.Rows[i]["7"].ToString(), dt.Rows[i]["8"].ToString(), dt.Rows[i]["9"].ToString(), dt.Rows[i]["10"].ToString(),
                                              dt.Rows[i]["11"].ToString(), dt.Rows[i]["12"].ToString(), dt.Rows[i]["13"].ToString(), dt.Rows[i]["14"].ToString(), dt.Rows[i]["15"].ToString(), dt.Rows[i]["16"].ToString(),
                                              dt.Rows[i]["17"].ToString(), dt.Rows[i]["18"].ToString(), dt.Rows[i]["19"].ToString(), dt.Rows[i]["20"].ToString(), dt.Rows[i]["21"].ToString(), dt.Rows[i]["22"].ToString(),
                                              dt.Rows[i]["23"].ToString(), dt.Rows[i]["24"].ToString(), dt.Rows[i]["25"].ToString(), dt.Rows[i]["26"].ToString(), dt.Rows[i]["27"].ToString(), dt.Rows[i]["28"].ToString(),
                                              dt.Rows[i]["29"].ToString(), dt.Rows[i]["30"].ToString(), dt.Rows[i]["31"].ToString(), dt.Rows[i]["I_CODE"].ToString(), dt.Rows[i]["I_CODENO"].ToString(), dt.Rows[i]["I_NAME"].ToString(),
                                              dt.Rows[i]["P_CODE"].ToString(), dt.Rows[i]["P_NAME"].ToString(), dt.Rows[i]["TYPES"].ToString());
                }
                #endregion
                #region MyRegion
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["I_CODE"].ToString() == dt1.Rows[j]["I_CODE"].ToString())
                        {
                            dtFilter.Rows.Add(dt1.Rows[j]["1"].ToString(), dt1.Rows[j]["2"].ToString(), dt1.Rows[j]["3"].ToString(), dt1.Rows[j]["4"].ToString(),
                                                      dt1.Rows[j]["5"].ToString(), dt1.Rows[j]["6"].ToString(), dt1.Rows[j]["7"].ToString(), dt1.Rows[j]["8"].ToString(), dt1.Rows[j]["9"].ToString(), dt1.Rows[j]["10"].ToString(),
                                                      dt1.Rows[j]["11"].ToString(), dt1.Rows[j]["12"].ToString(), dt1.Rows[j]["13"].ToString(), dt1.Rows[j]["14"].ToString(), dt1.Rows[j]["15"].ToString(), dt1.Rows[j]["16"].ToString(),
                                                      dt1.Rows[j]["17"].ToString(), dt1.Rows[j]["18"].ToString(), dt1.Rows[j]["19"].ToString(), dt1.Rows[j]["20"].ToString(), dt1.Rows[j]["21"].ToString(), dt1.Rows[j]["22"].ToString(),
                                                      dt1.Rows[j]["23"].ToString(), dt1.Rows[j]["24"].ToString(), dt1.Rows[j]["25"].ToString(), dt1.Rows[j]["26"].ToString(), dt1.Rows[j]["27"].ToString(), dt1.Rows[j]["28"].ToString(),
                                                      dt1.Rows[j]["29"].ToString(), dt1.Rows[j]["30"].ToString(), dt1.Rows[j]["31"].ToString(), dt1.Rows[j]["I_CODE"].ToString(), dt1.Rows[j]["I_CODENO"].ToString(), dt1.Rows[j]["I_NAME"].ToString(),
                                                      dt1.Rows[j]["P_CODE"].ToString(), dt1.Rows[j]["P_NAME"].ToString(), dt1.Rows[j]["TYPES"].ToString());
                            strRes1 = "";
                            break;
                        }
                        else
                        {
                            strRes1 = "1";
                        }
                    }
                    if (strRes1 == "1")
                    {
                        dtFilter.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dt.Rows[i]["I_CODE"].ToString(), dt.Rows[i]["I_CODENO"].ToString(), dt.Rows[i]["I_NAME"].ToString(), dt.Rows[i]["P_CODE"].ToString(), dt.Rows[i]["P_NAME"].ToString(), 2);
                    }
                }
                #endregion
                #region MyRegion
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["I_CODE"].ToString() == dt2.Rows[j]["I_CODE"].ToString())
                        {
                            dtFilter.Rows.Add(dt2.Rows[j]["1"].ToString(), dt2.Rows[j]["2"].ToString(), dt2.Rows[j]["3"].ToString(), dt2.Rows[j]["4"].ToString(),
                                                dt2.Rows[j]["5"].ToString(), dt2.Rows[j]["6"].ToString(), dt2.Rows[j]["7"].ToString(), dt2.Rows[j]["8"].ToString(), dt2.Rows[j]["9"].ToString(), dt2.Rows[j]["10"].ToString(),
                                                dt2.Rows[j]["11"].ToString(), dt2.Rows[j]["12"].ToString(), dt2.Rows[j]["13"].ToString(), dt2.Rows[j]["14"].ToString(), dt2.Rows[j]["15"].ToString(), dt2.Rows[j]["16"].ToString(),
                                                dt2.Rows[j]["17"].ToString(), dt2.Rows[j]["18"].ToString(), dt2.Rows[j]["19"].ToString(), dt2.Rows[j]["20"].ToString(), dt2.Rows[j]["21"].ToString(), dt2.Rows[j]["22"].ToString(),
                                                dt2.Rows[j]["23"].ToString(), dt2.Rows[j]["24"].ToString(), dt2.Rows[j]["25"].ToString(), dt2.Rows[j]["26"].ToString(), dt2.Rows[j]["27"].ToString(), dt2.Rows[j]["28"].ToString(),
                                                dt2.Rows[j]["29"].ToString(), dt2.Rows[j]["30"].ToString(), dt2.Rows[j]["31"].ToString(), dt2.Rows[j]["I_CODE"].ToString(), dt2.Rows[j]["I_CODENO"].ToString(), dt2.Rows[j]["I_NAME"].ToString(),
                                                dt2.Rows[j]["P_CODE"].ToString(), dt2.Rows[j]["P_NAME"].ToString(), dt2.Rows[j]["TYPES"].ToString());
                            strRes1 = "";
                            break;
                        }
                        else
                        {
                            strRes1 = "1";
                        }
                    }
                    if (strRes1 == "1")
                    {
                        dtFilter.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dt.Rows[i]["I_CODE"].ToString(), dt.Rows[i]["I_CODENO"].ToString(), dt.Rows[i]["I_NAME"].ToString(), dt.Rows[i]["P_CODE"].ToString(), dt.Rows[i]["P_NAME"].ToString(), 3);
                    }
                }
                #endregion
                if (dtFilter.Rows.Count > 0)
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    rptname.Load(Server.MapPath("~/Reports/rptVendorRejYearly1.rpt")); //foundaryRejYearly
                    rptname.FileName = Server.MapPath("~/Reports/rptVendorRejYearly1.rpt"); //foundaryRejYearly
                    rptname.Refresh();
                    rptname.SetDataSource(dtFilter);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "Vendor Wise Rejection Yearly From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
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
            else
            {
                Query = "  select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], P_CODE,P_NAME ,'1'     AS TYPES from ( SELECT  P_CODE,P_NAME ,DATEPART(MM, IWM_DATE) AS IWM_DATE_DAY,DATEPART(YYYY, IWM_DATE) AS IWM_DATE_YEAR,CASE WHEN ISNULL(SUM(IWD_SQTY),0)=0 then 0 else round((  ISNULL((SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND  DATEPART(MM, IRN_DATE)=DATEPART(MM, IWM_DATE) AND  DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IWM_DATE)  AND IRND_TYPE=1 AND IRND_RSM_CODE IN (-2147483643 ,-2147483645)  AND IRND_P_CODE=P_CODE),0)  / ISNULL(SUM(IWD_SQTY),0)  *100),2) END AS IRND_REJ_QTY  FROM INWARD_MASTER,INWARD_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=P_CODE  AND IWD_I_CODE=I_CODE    AND   " + Cond + "   IWM_TYPE='OUTCUSTINV' AND I_CAT_CODE=-2147483648  AND IWM_P_CODE='" + PartyCode + "' GROUP BY P_CODE,DATEPART(MM, IWM_DATE),DATEPART(YYYY, IWM_DATE),P_CODE,P_NAME    )   AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12] )) AS PivotTable    GROUP BY  P_CODE,P_NAME    ";

                dt = CommonClasses.Execute(Query);
                if (dt.Rows.Count > 0)
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    rptname.Load(Server.MapPath("~/Reports/rptVendorRejYearly2.rpt")); //foundaryRejYearly
                    rptname.FileName = Server.MapPath("~/Reports/rptVendorRejYearly2.rpt"); //foundaryRejYearly
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "Vendor Wise Rejection Yearly From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Found";
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region GenerateReport1
    private void GenerateReport1(string Title, string From, string Todt, string Type, string Cond, string PartyCode)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            string Query1 = "";
            string Query2 = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dtLine = new DataTable();

            if (Type == "0")
            {
                DataTable dtFilter = new DataTable();

                #region MyRegion
                dtFilter.Columns.Add(new System.Data.DataColumn("1", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("2", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("3", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("4", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("5", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("6", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("7", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("8", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("9", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("10", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("11", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("12", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("13", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("14", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("15", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("16", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("17", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("18", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("19", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("20", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("21", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("22", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("23", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("24", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("25", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("26", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("27", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("28", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("29", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("30", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("31", typeof(float)));
                dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                dtFilter.Columns.Add(new System.Data.DataColumn("TYPES", typeof(string)));
                #endregion
                Query = " (select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31] ,P_CODE,P_NAME ,'1'     AS TYPES from (SELECT  ISNULL(SUM(IWD_SQTY),0) AS IWD_CH_QTY,P_CODE,P_NAME ,DATEPART(dd, IWM_DATE) AS IWM_DATE_DAY,IWM_DATE FROM INWARD_MASTER,INWARD_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=P_CODE  AND IWD_I_CODE=I_CODE AND  IWM_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'    AND " + Cond + "  IWM_TYPE='OUTCUSTINV' AND I_CAT_CODE=-2147483648 GROUP BY   P_CODE,DATEPART(dd, IWM_DATE),IWM_DATE,P_CODE,P_NAME    )   AS SOURCETABLE PIVOT (sum(IWD_CH_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY  P_CODE,P_NAME )   ORDER BY P_NAME, TYPES     ";

                Query1 = " (select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31]  ,P_CODE,P_NAME,'2' AS TYPES  from ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY, P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) AS IWM_DATE_DAY,   IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0  AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND   IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND " + Cond + "  IRND_TYPE=1  AND IRND_RSM_CODE=-2147483645 GROUP BY  P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) ,   IRN_DATE   )   AS aSOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY  P_CODE,P_NAME )   ORDER BY P_NAME,TYPES     ";

                Query2 = " (select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12],ISNULL(SUM([13]),0) AS [13],ISNULL(SUM([14]),0) AS [14],ISNULL(SUM([15]),0) AS [15],ISNULL(SUM([16]),0) AS [16],ISNULL(SUM([17]),0) AS [17],ISNULL(SUM([18]),0) AS [18],ISNULL(SUM([19]),0) AS [19],ISNULL(SUM([20]),0) AS [20],ISNULL(SUM([21]),0) AS [21],ISNULL(SUM([22]),0) AS [22],ISNULL(SUM([23]),0) AS [23],ISNULL(SUM([24]),0) AS [24],ISNULL(SUM([25]),0) AS [25],ISNULL(SUM([26]),0) AS [26],ISNULL(SUM([27]),0) AS [27],ISNULL(SUM([28]),0) AS [28],ISNULL(SUM([29]),0) AS [29],ISNULL(SUM([30]),0) AS [30],ISNULL(SUM([31]),0) AS [31]  ,P_CODE,P_NAME,'3' AS TYPES  from ( SELECT  ISNULL(SUM(IRND_REJ_QTY),0) AS IRND_REJ_QTY, P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) AS IWM_DATE_DAY,   IRN_DATE FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER,PARTY_MASTER where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0  AND IRND_P_CODE=P_CODE AND IRND_I_CODE=I_CODE AND   IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'    AND " + Cond + " IRND_TYPE=1 AND IRND_RSM_CODE=-2147483643  GROUP BY  P_CODE,P_NAME ,  DATEPART(DD, IRN_DATE) ,   IRN_DATE   )   AS aSOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IWM_DATE_DAY IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16], [17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])) AS PivotTable    GROUP BY  P_CODE,P_NAME )   ORDER BY P_NAME,TYPES     ";

                dt = CommonClasses.Execute(Query);
                dt1 = CommonClasses.Execute(Query1);
                dt2 = CommonClasses.Execute(Query2);

                string strRes1 = "";

                #region MyRegion
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtFilter.Rows.Add(dt.Rows[i]["1"].ToString(), dt.Rows[i]["2"].ToString(), dt.Rows[i]["3"].ToString(), dt.Rows[i]["4"].ToString(),
                                              dt.Rows[i]["5"].ToString(), dt.Rows[i]["6"].ToString(), dt.Rows[i]["7"].ToString(), dt.Rows[i]["8"].ToString(), dt.Rows[i]["9"].ToString(), dt.Rows[i]["10"].ToString(),
                                              dt.Rows[i]["11"].ToString(), dt.Rows[i]["12"].ToString(), dt.Rows[i]["13"].ToString(), dt.Rows[i]["14"].ToString(), dt.Rows[i]["15"].ToString(), dt.Rows[i]["16"].ToString(),
                                              dt.Rows[i]["17"].ToString(), dt.Rows[i]["18"].ToString(), dt.Rows[i]["19"].ToString(), dt.Rows[i]["20"].ToString(), dt.Rows[i]["21"].ToString(), dt.Rows[i]["22"].ToString(),
                                              dt.Rows[i]["23"].ToString(), dt.Rows[i]["24"].ToString(), dt.Rows[i]["25"].ToString(), dt.Rows[i]["26"].ToString(), dt.Rows[i]["27"].ToString(), dt.Rows[i]["28"].ToString(),
                                              dt.Rows[i]["29"].ToString(), dt.Rows[i]["30"].ToString(), dt.Rows[i]["31"].ToString(),
                                              dt.Rows[i]["P_CODE"].ToString(), dt.Rows[i]["P_NAME"].ToString(), dt.Rows[i]["TYPES"].ToString());
                }
                #endregion
                #region MyRegion
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["P_CODE"].ToString() == dt1.Rows[j]["P_CODE"].ToString())
                        {
                            dtFilter.Rows.Add(dt1.Rows[j]["1"].ToString(), dt1.Rows[j]["2"].ToString(), dt1.Rows[j]["3"].ToString(), dt1.Rows[j]["4"].ToString(),
                                                      dt1.Rows[j]["5"].ToString(), dt1.Rows[j]["6"].ToString(), dt1.Rows[j]["7"].ToString(), dt1.Rows[j]["8"].ToString(), dt1.Rows[j]["9"].ToString(), dt1.Rows[j]["10"].ToString(),
                                                      dt1.Rows[j]["11"].ToString(), dt1.Rows[j]["12"].ToString(), dt1.Rows[j]["13"].ToString(), dt1.Rows[j]["14"].ToString(), dt1.Rows[j]["15"].ToString(), dt1.Rows[j]["16"].ToString(),
                                                      dt1.Rows[j]["17"].ToString(), dt1.Rows[j]["18"].ToString(), dt1.Rows[j]["19"].ToString(), dt1.Rows[j]["20"].ToString(), dt1.Rows[j]["21"].ToString(), dt1.Rows[j]["22"].ToString(),
                                                      dt1.Rows[j]["23"].ToString(), dt1.Rows[j]["24"].ToString(), dt1.Rows[j]["25"].ToString(), dt1.Rows[j]["26"].ToString(), dt1.Rows[j]["27"].ToString(), dt1.Rows[j]["28"].ToString(),
                                                      dt1.Rows[j]["29"].ToString(), dt1.Rows[j]["30"].ToString(), dt1.Rows[j]["31"].ToString(),
                                                      dt1.Rows[j]["P_CODE"].ToString(), dt1.Rows[j]["P_NAME"].ToString(), dt1.Rows[j]["TYPES"].ToString());
                            strRes1 = "";
                            break;
                        }
                        else
                        {
                            strRes1 = "1";
                        }
                    }
                    if (strRes1 == "1")
                    {
                        dtFilter.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dt.Rows[i]["P_CODE"].ToString(), dt.Rows[i]["P_NAME"].ToString(), 2);
                    }
                }
                #endregion
                #region MyRegion
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["P_CODE"].ToString() == dt2.Rows[j]["P_CODE"].ToString())
                        {
                            dtFilter.Rows.Add(dt2.Rows[j]["1"].ToString(), dt2.Rows[j]["2"].ToString(), dt2.Rows[j]["3"].ToString(), dt2.Rows[j]["4"].ToString(),
                                                dt2.Rows[j]["5"].ToString(), dt2.Rows[j]["6"].ToString(), dt2.Rows[j]["7"].ToString(), dt2.Rows[j]["8"].ToString(), dt2.Rows[j]["9"].ToString(), dt2.Rows[j]["10"].ToString(),
                                                dt2.Rows[j]["11"].ToString(), dt2.Rows[j]["12"].ToString(), dt2.Rows[j]["13"].ToString(), dt2.Rows[j]["14"].ToString(), dt2.Rows[j]["15"].ToString(), dt2.Rows[j]["16"].ToString(),
                                                dt2.Rows[j]["17"].ToString(), dt2.Rows[j]["18"].ToString(), dt2.Rows[j]["19"].ToString(), dt2.Rows[j]["20"].ToString(), dt2.Rows[j]["21"].ToString(), dt2.Rows[j]["22"].ToString(),
                                                dt2.Rows[j]["23"].ToString(), dt2.Rows[j]["24"].ToString(), dt2.Rows[j]["25"].ToString(), dt2.Rows[j]["26"].ToString(), dt2.Rows[j]["27"].ToString(), dt2.Rows[j]["28"].ToString(),
                                                dt2.Rows[j]["29"].ToString(), dt2.Rows[j]["30"].ToString(), dt2.Rows[j]["31"].ToString(),
                                                dt2.Rows[j]["P_CODE"].ToString(), dt2.Rows[j]["P_NAME"].ToString(), dt2.Rows[j]["TYPES"].ToString());
                            strRes1 = "";
                            break;
                        }
                        else
                        {
                            strRes1 = "1";
                        }
                    }
                    if (strRes1 == "1")
                    {
                        dtFilter.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dt.Rows[i]["P_CODE"].ToString(), dt.Rows[i]["P_NAME"].ToString(), 3);
                    }
                }
                #endregion
                if (dtFilter.Rows.Count > 0)
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    rptname.Load(Server.MapPath("~/Reports/rptVendorRejYearly3.rpt")); //foundaryRejYearly
                    rptname.FileName = Server.MapPath("~/Reports/rptVendorRejYearly3.rpt"); //foundaryRejYearly
                    rptname.Refresh();
                    rptname.SetDataSource(dtFilter);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "Vendor Wise Rejection Yearly From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
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
            else
            {
                Query = "SELECT  P_CODE,P_NAME ,DATEPART(MM, IWM_DATE) AS IWM_DATE_DAY,DATEPART(YYYY, IWM_DATE) AS IWM_DATE_YEAR,CASE WHEN ISNULL(SUM(IWD_SQTY),0)=0 then 0 else round((  ISNULL((SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRND_IRN_CODE=IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND  DATEPART(MM, IRN_DATE)=DATEPART(MM, IWM_DATE) AND  DATEPART(YYYY, IRN_DATE)=DATEPART(YYYY, IWM_DATE)  AND IRND_TYPE=1 AND IRND_RSM_CODE IN (-2147483643 ,-2147483645)  AND IRND_P_CODE=P_CODE),0)  / ISNULL(SUM(IWD_SQTY),0)  *100),2) END AS IRND_REJ_QTY  FROM INWARD_MASTER,INWARD_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=P_CODE  AND IWD_I_CODE=I_CODE    AND   " + Cond + "   IWM_TYPE='OUTCUSTINV' AND I_CAT_CODE=-2147483648    AND IWM_DATE BETWEEN '01/NOV/2017'  AND '30/NOV/2017' GROUP BY P_CODE,DATEPART(MM, IWM_DATE),DATEPART(YYYY, IWM_DATE),P_CODE,P_NAME        ";

                dt = CommonClasses.Execute(Query);
                if (dt.Rows.Count > 0)
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/rptVendorRejYearly4.rpt")); //foundaryRejYearly
                    rptname.FileName = Server.MapPath("~/Reports/rptVendorRejYearly4.rpt"); //foundaryRejYearly
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "Vendor Wise Rejection Yearly For " + Convert.ToDateTime(From).ToString("MMM yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Found";
                    return;
                }
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/ViewVendorRej.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
