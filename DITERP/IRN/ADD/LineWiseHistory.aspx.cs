﻿﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_LineWiseHistory : System.Web.UI.Page
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
            string i_name = Request.QueryString[3].ToString();
            string L_name = Request.QueryString[4].ToString();
            GenerateReport(Title, From, Todt, i_name, L_name);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string i_name, string L_name)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = " SELECT DISTINCT I_CODE,I_CODENO,I_NAME,LM_CODE AS STG_CODE,LM_NAME AS STG_NAME,LC_DATE AS SI_DATE,LC_CODE AS SI_CODE,LC_NO AS SI_NO    ,LC_ACTIVE AS SI_ACTIVE FROM LINE_MASTER INNER JOIN  LINE_CHANGE ON LINE_MASTER.LM_CODE = LINE_CHANGE.LC_LM_CODE INNER JOIN  ITEM_MASTER ON LINE_CHANGE.LC_I_CODE = ITEM_MASTER.I_CODE WHERE     (LINE_MASTER.ES_DELETE = 0) AND LC_DATE  BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO, LC_ACTIVE DESC,LC_DATE DESC,LC_CODE desc";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtLine = new DataTable();
            dt = CommonClasses.Execute(Query);

            #region Apply Filter On DataTable
            if (i_name == "0")
            {
                if (L_name == "0")
                {
                    dtLine = dt;
                }
                else
                {
                    DataView dv1 = dt.DefaultView;
                    dv1.RowFilter = "I_CODE in(" + i_name + ")";
                    dtLine = dv1.ToTable();
                }
            }
            else if (i_name != "0")
            {
                if (L_name == "0")
                {
                    DataView dv1 = dt.DefaultView;
                    dv1.RowFilter = "I_CODE in(" + i_name + ")";
                    dtLine = dv1.ToTable();
                }
                else
                {
                    DataView dv1 = dt.DefaultView;
                    dv1.RowFilter = "I_CODE in(" + i_name + ") AND STG_CODE in (" + L_name + ")";
                    dtLine = dv1.ToTable();
                }
            }
            #endregion Apply Filter On DataTable

            if (dtLine.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/rptLineChange.rpt"));   // rptStageChange //rpt8020CompSpecRejPer
                rptname.FileName = Server.MapPath("~/Reports/rptLineChange.rpt"); // rptStageChange  //rpt8020CompSpecRejPer
                rptname.Refresh();
                rptname.SetDataSource(dtLine);
                rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
                rptname.SetParameterValue("Title", "Line Wise History Report From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "  To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
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
            Response.Redirect("~/IRN/VIEW/ViewLineWiseHistory.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Line Wise History Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

