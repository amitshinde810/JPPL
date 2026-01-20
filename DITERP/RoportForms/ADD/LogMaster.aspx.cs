using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_LogMaster : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool ChkAllUser = Convert.ToBoolean(Request.QueryString[2].ToString());
            bool chkAllMod = Convert.ToBoolean(Request.QueryString[3].ToString());
            string From = Request.QueryString[4].ToString();
            string To = Request.QueryString[5].ToString();
            int U_name = Convert.ToInt32(Request.QueryString[6].ToString());
            string M_name = Request.QueryString[7].ToString();
            string Type = Request.QueryString[8].ToString();

            if (Type == "DateWise")
            {
                if (ChkAllDate == true && ChkAllUser == true && chkAllMod == true)
                {
                    GenerateReport(Title, "All", "All", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser != true && chkAllMod != true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser != true && chkAllMod == true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser == true && chkAllMod != true)
                {
                    GenerateReport(Title, "ONE", "All", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser == true && chkAllMod == true)
                {
                    GenerateReport(Title, "ONE", "All", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser != true && chkAllMod != true)
                {
                    GenerateReport(Title, "ALL", "ONE", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser != true && chkAllMod == true)
                {
                    GenerateReport(Title, "ALL", "ONE", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser == true && chkAllMod != true)
                {
                    GenerateReport(Title, "All", "All", "ONE", From, To, U_name, M_name, Type);
                }
            }
            if (Type == "ModuleWise")
            {
                if (ChkAllDate == true && ChkAllUser == true && chkAllMod == true)
                {
                    GenerateReport(Title, "All", "All", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser != true && chkAllMod != true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser != true && chkAllMod == true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser == true && chkAllMod != true)
                {
                    GenerateReport(Title, "ONE", "All", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser == true && chkAllMod == true)
                {
                    GenerateReport(Title, "ONE", "All", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser != true && chkAllMod != true)
                {
                    GenerateReport(Title, "ALL", "ONE", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser != true && chkAllMod == true)
                {
                    GenerateReport(Title, "ALL", "ONE", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser == true && chkAllMod != true)
                {
                    GenerateReport(Title, "All", "All", "ONE", From, To, U_name, M_name, Type);
                }
            }
            if (Type == "UserWise")
            {
                if (ChkAllDate == true && ChkAllUser == true && chkAllMod == true)
                {
                    GenerateReport(Title, "All", "All", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser != true && chkAllMod != true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser != true && chkAllMod == true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser == true && chkAllMod != true)
                {
                    GenerateReport(Title, "ONE", "All", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate != true && ChkAllUser == true && chkAllMod == true)
                {
                    GenerateReport(Title, "ONE", "All", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser != true && chkAllMod != true)
                {
                    GenerateReport(Title, "ALL", "ONE", "ONE", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser != true && chkAllMod == true)
                {
                    GenerateReport(Title, "ALL", "ONE", "ALL", From, To, U_name, M_name, Type);
                }
                if (ChkAllDate == true && ChkAllUser == true && chkAllMod != true)
                {
                    GenerateReport(Title, "All", "All", "ONE", From, To, U_name, M_name, Type);
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string date1, string User, string Module, string From, string To, int U_name, string M_name, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "Select LG_CODE,LG_DATE,UPPER(LG_SOURCE)AS LG_SOURCE,(UM_NAME)as LG_U_NAME ,LG_EVENT ,LG_COMP_NAME,UPPER(LG_DOC_NAME)AS LG_DOC_NAME,LG_U_NAME,LG_IP_ADDRESS from LOG_MASTER,USER_MASTER where LG_CM_COMP_ID=" + (string)Session["CompanyId"] + " and  USER_MASTER.UM_CODE=LOG_MASTER.LG_U_CODE ";

            #region All
            if (Type == "DateWise")
            {
                if (date1 == "All" && User == "All" && Module == "ALL")
                {
                    Query = Query;
                }
                if (date1 != "All" && User != "All" && Module != "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and LG_U_CODE='" + U_name + "' and LG_SOURCE='" + M_name + "'  ";
                }
                if (date1 != "All" && User != "All" && Module == "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),106) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LG_U_CODE='" + U_name + "'";
                }
                if (date1 != "All" && User == "All" && Module != "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'   and LG_SOURCE='" + M_name + "'  ";
                }
                if (date1 != "All" && User == "All" && Module == "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  ";
                }
                if (date1 == "All" && User != "All" && Module != "ALL")
                {
                    Query = Query + " and LG_U_CODE='" + U_name + "' and LG_SOURCE='" + M_name + "' ";
                }
                if (date1 == "All" && User != "All" && Module == "ALL")
                {
                    Query = Query + " and LG_U_CODE='" + U_name + "' ";
                }
                if (date1 == "All" && User == "All" && Module != "ALL")
                {
                    Query = Query + " and LG_SOURCE='" + M_name + "' ";
                }
            }
            if (Type == "ModuleWise")
            {
                if (date1 == "All" && User == "All" && Module == "ALL")
                {
                    Query = Query;
                }
                if (date1 != "All" && User != "All" && Module != "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and LG_U_CODE='" + U_name + "' and LG_SOURCE='" + M_name + "'  ";
                }
                if (date1 != "All" && User != "All" && Module == "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),106) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LG_U_CODE='" + U_name + "'";
                }
                if (date1 != "All" && User == "All" && Module != "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'   and LG_SOURCE='" + M_name + "'  ";
                }

                if (date1 != "All" && User == "All" && Module == "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  ";
                }
                if (date1 == "All" && User != "All" && Module != "ALL")
                {
                    Query = Query + " and LG_U_CODE='" + U_name + "' and LG_SOURCE='" + M_name + "' ";
                }
                if (date1 == "All" && User != "All" && Module == "ALL")
                {
                    Query = Query + " and LG_U_CODE='" + U_name + "' ";
                }
                if (date1 == "All" && User == "All" && Module != "ALL")
                {
                    Query = Query + " and LG_SOURCE='" + M_name + "' ";
                }
            }
            if (Type == "UserWise")
            {
                if (date1 == "All" && User == "All" && Module == "ALL")
                {
                    Query = Query;
                }
                if (date1 != "All" && User != "All" && Module != "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  and LG_U_CODE='" + U_name + "' and LG_SOURCE='" + M_name + "'  ";
                }
                if (date1 != "All" && User != "All" && Module == "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),106) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and LG_U_CODE='" + U_name + "'";
                }
                if (date1 != "All" && User == "All" && Module != "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'   and LG_SOURCE='" + M_name + "'  ";
                }
                if (date1 != "All" && User == "All" && Module == "ALL")
                {
                    Query = Query + " and CONVERT(date,(LG_DATE),103) between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'  ";
                }
                if (date1 == "All" && User != "All" && Module != "ALL")
                {
                    Query = Query + " and LG_U_CODE='" + U_name + "' and LG_SOURCE='" + M_name + "' ";
                }
                if (date1 == "All" && User != "All" && Module == "ALL")
                {
                    Query = Query + " and LG_U_CODE='" + U_name + "' ";
                }
                if (date1 == "All" && User == "All" && Module != "ALL")
                {
                    Query = Query + " and LG_SOURCE='" + M_name + "' ";
                }
            }
            #endregion
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (Type == "DateWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptLogMasterDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptLogMasterDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (Type == "ModuleWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptLogMasterModuleWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptLogMasterModuleWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (Type == "UserWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptLogMasterUserWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptLogMasterUserWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
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
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewLogMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
