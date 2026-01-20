using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_TurningRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        string Title = Request.QueryString[0];
        string From = Request.QueryString[1].ToString();
        string To = Request.QueryString[2].ToString();
        string group = Request.QueryString[3].ToString();
        string way = Request.QueryString[4].ToString();
        string cond = Request.QueryString[5].ToString();
        string Type = Request.QueryString[6].ToString();

        GenerateReport(Title, From, To, group, cond, Type);

    }
    #endregion

    private void GenerateReport(string Title, string From, string To, string group, string cond, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            if (Type == "Detail")
            {
                if (cond == "")
                {
                    Query = "SELECT IWD_I_CODE,IWM_NO,IWM_DATE,P_NAME,I_CODENO,I_NAME,IWD_REV_QTY,IWD_TUR_WEIGHT,IWD_TUR_QTY,IWD_REC_TUR_QTY,ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND INWARD_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0";
                }
                else
                {
                    Query = "SELECT IWD_I_CODE,IWM_NO,IWM_DATE,P_NAME,I_CODENO,I_NAME,IWD_REV_QTY,IWD_TUR_WEIGHT,IWD_TUR_QTY,IWD_REC_TUR_QTY,ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE " + cond + " IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND INWARD_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0";
                }
            }
            else
            {
                if (cond == "")
                {
                    Query = "SELECT IWM_DATE,P_NAME,sum(IWD_REV_QTY) as IWD_REV_QTY,sum(IWD_TUR_QTY) as IWD_TUR_QTY,SUM(IWD_REC_TUR_QTY) AS IWD_REC_TUR_QTY,(SELECT (Round(sum(IWD_TUR_QTY),3)-Round(SUM(IWD_REC_TUR_QTY),3)) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWM_DATE < IWM.IWM_DATE AND IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 and P_CODE=IWM.IWM_P_CODE) as OPENING,(((SELECT (Round(sum(IWD_TUR_QTY),3)-Round(SUM(IWD_REC_TUR_QTY),3)) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWM_DATE < IWM.IWM_DATE AND IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 and P_CODE=IWM.IWM_P_CODE))+sum(IWD_TUR_QTY)-SUM(IWD_REC_TUR_QTY)) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER as IWM,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND IWM.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 group by P_NAME,IWM_P_CODE,IWM_DATE";
                }
                else
                {
                    Query = "SELECT IWM_DATE,P_NAME,sum(IWD_REV_QTY) as IWD_REV_QTY,sum(IWD_TUR_QTY) as IWD_TUR_QTY,SUM(IWD_REC_TUR_QTY) AS IWD_REC_TUR_QTY,(SELECT (Round(sum(IWD_TUR_QTY),3)-Round(SUM(IWD_REC_TUR_QTY),3)) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWM_DATE < IWM.IWM_DATE AND IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 and P_CODE=IWM.IWM_P_CODE) as OPENING,(((SELECT (Round(sum(IWD_TUR_QTY),3)-Round(SUM(IWD_REC_TUR_QTY),3)) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWM_DATE < IWM.IWM_DATE AND IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 and P_CODE=IWM.IWM_P_CODE))+sum(IWD_TUR_QTY)-SUM(IWD_REC_TUR_QTY)) AS TUR_BAL FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER as IWM,PARTY_MASTER WHERE " + cond + " IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND IWM.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 group by P_NAME,IWM_P_CODE,IWM_DATE";
                }
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    if (Type == "Detail")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptDateWiseTurningreport.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptDateWiseTurningreport.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);

                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "");
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    else if (Type == "Summary")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptTurningdateWiseSummary.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptTurningdateWiseSummary.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);

                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "");
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                }
                if (group == "SubContWise")
                {
                    if (Type == "Detail")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptSubContWiseTurningreport.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptSubContWiseTurningreport.rpt");

                        rptname.Refresh();
                        rptname.SetDataSource(dt);

                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "");

                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    else if (Type == "Summary")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptTurningSubContWiseSummary.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptTurningSubContWiseSummary.rpt");

                        rptname.Refresh();
                        rptname.SetDataSource(dt);

                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtTitle", "From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "");
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RoportForms/VIEW/ViewTurningRegisterReport.aspx", false);
    }
}
