﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_Rejectiondetails : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ViewState["strUser"] = strUser;
            if (!IsPostBack)
            {
                CommonClasses.Execute("DELETE FROM IRN_DETAILS_REPORT where UM_CODE='" + Session["UserCode"].ToString() + "' ");
                ViewState["strUser"] = "0";
            }
            string Title = Request.QueryString[0];

            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string i_name = Request.QueryString[3].ToString();

            GenerateReport(Title, From, Todt, i_name);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string i_name)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (i_name == "0")
            {
                Query = " SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_CM_ID=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO";
            }
            else
            {
                Query = " SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRN_CM_ID=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND  I_CODE='" + i_name + "' ORDER BY I_CODENO";
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                DataTable dtReport = new DataTable();
                DateTime dtfromdate = new DateTime();
                DateTime dtTodate = new DateTime();

                if (dtReport.Columns.Count == 0)
                {
                    dtReport.Columns.Add("I_CODE");
                    dtReport.Columns.Add("I_CODENO");
                    dtReport.Columns.Add("I_NAME");
                    dtReport.Columns.Add("PROD_QTY");
                    dtReport.Columns.Add("PROD_AMT");
                    dtReport.Columns.Add("R1_REJ_QTY");
                    dtReport.Columns.Add("R1_REJ_AMT");
                    dtReport.Columns.Add("R2_REJ_QTY");
                    dtReport.Columns.Add("R2_REJ_AMT");
                    dtReport.Columns.Add("R3_REJ_QTY");
                    dtReport.Columns.Add("R3_REJ_AMT");
                    dtReport.Columns.Add("R4A_REJ_QTY");
                    dtReport.Columns.Add("R4A_REJ_AMT");
                    dtReport.Columns.Add("R4_REJ_QTY");
                    dtReport.Columns.Add("R4_REJ_AMT");
                    dtReport.Columns.Add("R5_REJ_QTY");
                    dtReport.Columns.Add("R5_REJ_AMT");
                    dtReport.Columns.Add("R6_REJ_QTY");
                    dtReport.Columns.Add("R6_REJ_AMT");
                    dtReport.Columns.Add("IRN_DATE");
                    if (ViewState["strUser"] == "0")
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dtfromdate = Convert.ToDateTime(From);
                            dtTodate = Convert.ToDateTime(Todt);
                            for (; dtfromdate <= dtTodate; )
                            {
                                DataTable dtResult = new DataTable();
                                dtResult = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME,I_INV_RATE,convert(varchar,IRN_DATE,106) as IRN_DATE,  (SELECT ISNULL(SUM(IRND_PROD_QTY),0)  AS PROD_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1  )  AS PROD_QTY,(SELECT ISNULL(SUM(IRND_AMT),0)  AS PROD_AMT FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1  ) AS PROD_AMT, (SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R1_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE   AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483648) AS R1_REJ_QTY,(SELECT ISNULL(SUM(IRND_AMT),0)  AS R1_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE   AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-2147483648 ) AS R1_REJ_AMT, (SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R2_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE  AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483647 ) AS R2_REJ_QTY ,(SELECT ISNULL(SUM(IRND_AMT),0)  AS R2_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-2147483647) AS R2_REJ_AMT,(SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R3_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483646) AS R3_REJ_QTY , (SELECT ISNULL(SUM(IRND_AMT),0)  AS R3_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-2147483646) AS R3_REJ_AMT, (SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R4A_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483645) AS R4A_REJ_QTY, (SELECT ISNULL(SUM(IRND_AMT),0)  AS R4A_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-2147483645) AS R4A_REJ_AMT, (SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R4_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483644)  AS R4_REJ_QTY, (SELECT ISNULL(SUM(IRND_AMT),0)  AS R4_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-2147483644)  AS R4_REJ_AMT , (SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R5_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483643) AS R5_REJ_QTY, (SELECT ISNULL(SUM(IRND_AMT),0)  AS R5_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-2147483643)   AS R5_REJ_AMT,(SELECT ISNULL(SUM(IRND_REJ_QTY),0)  AS R6_REJ_QTY FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_RSM_CODE=-2147483642)    AS R6_REJ_QTY,(SELECT ISNULL(SUM(IRND_AMT),0)  AS R6_REJ_AMT FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_RSM_CODE=-21474836442)   AS R6_REJ_AMT  FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER  where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0   AND IRND_I_CODE=I_CODE AND IRN_DATE =  '" + dtfromdate.ToString("dd/MMM/yyyy") + "' AND I_CODE='" + dt.Rows[i]["I_CODE"].ToString() + "' ORDER BY I_CODENO");
                                if (dtResult.Rows.Count > 0)
                                {
                                    CommonClasses.Execute("INSERT INTO IRN_DETAILS_REPORT (I_CODE,I_CODENO,I_NAME,PROD_QTY,PROD_AMT,R1_REJ_QTY, R1_REJ_AMT, R2_REJ_QTY, R2_REJ_AMT, R3_REJ_QTY, R3_REJ_AMT, R4A_REJ_QTY, R4A_REJ_AMT, R4_REJ_QTY, R4_REJ_AMT, R5_REJ_QTY, R5_REJ_AMT, R6_REJ_QTY, R6_REJ_AMT,IRN_DATE,UM_CODE) VALUES('" + dtResult.Rows[0]["I_CODE"].ToString() + "','" + dtResult.Rows[0]["I_CODENO"].ToString() + "','" + dtResult.Rows[0]["I_NAME"].ToString() + "','" + dtResult.Rows[0]["PROD_QTY"].ToString() + "','" + dtResult.Rows[0]["PROD_AMT"].ToString() + "','" + dtResult.Rows[0]["R1_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R1_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["R2_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R2_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["R3_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R3_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["R4A_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R4A_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["R4_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R4_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["R5_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R5_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["R6_REJ_QTY"].ToString() + "','" + dtResult.Rows[0]["R6_REJ_AMT"].ToString() + "','" + dtResult.Rows[0]["IRN_DATE"].ToString() + "' ,'" + Session["UserCode"].ToString() + "' ) ");
                                }
                                else
                                {
                                    CommonClasses.Execute("INSERT INTO IRN_DETAILS_REPORT (I_CODE,I_CODENO,I_NAME,PROD_QTY,PROD_AMT,R1_REJ_QTY, R1_REJ_AMT, R2_REJ_QTY, R2_REJ_AMT, R3_REJ_QTY, R3_REJ_AMT, R4A_REJ_QTY, R4A_REJ_AMT, R4_REJ_QTY, R4_REJ_AMT, R5_REJ_QTY, R5_REJ_AMT, R6_REJ_QTY, R6_REJ_AMT,IRN_DATE,UM_CODE) VALUES('" + dt.Rows[i]["I_CODE"].ToString() + "', '" + dt.Rows[i]["I_CODENO"].ToString() + "', '" + dt.Rows[i]["I_NAME"].ToString() + "','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','0','" + dtfromdate.ToString("dd/MMM/yyyy") + "' ,'" + Session["UserCode"].ToString() + "' )");
                                }
                                dtfromdate = dtfromdate.AddDays(1);
                            }
                        }
                        ViewState["strUser"] = "1";
                    }
                    dtReport = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME,IRN_DATE,PROD_QTY,PROD_AMT,R1_REJ_QTY,R1_REJ_AMT,R2_REJ_QTY,R2_REJ_AMT,R3_REJ_QTY,R3_REJ_AMT,R4A_REJ_QTY,R4A_REJ_AMT,R4_REJ_QTY,R4_REJ_AMT,R5_REJ_QTY,R5_REJ_AMT,R6_REJ_QTY,R6_REJ_AMT FROM IRN_DETAILS_REPORT WHERE UM_CODE='" + Session["UserCode"].ToString() + "' ORDER BY I_CODENO,IRN_DATE");
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    rptname.Load(Server.MapPath("~/Reports/rptIRNRejDetail.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptIRNRejDetail.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtReport);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "STAGE WISE REJECTION DETAILS From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
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
            Response.Redirect("~/IRN/VIEW/ViewRejectiondetails.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
