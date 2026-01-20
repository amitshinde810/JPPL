using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_AddSubConReg : System.Web.UI.Page
{

    DatabaseAccessLayer DL_DBAccess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        string Title = Request.QueryString[0];
        string From = Request.QueryString[1].ToString();
        string To = Request.QueryString[2].ToString();
        string cond = Request.QueryString[3].ToString();

        GenerateReport(Title, From, To, cond);
    }
    #endregion

    private void GenerateReport(string Title, string From, string To, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            Query = "select distinct P_NAME,I_CODENO,I_NAME,SPOM_PO_NO,convert(varchar,SPOM_DATE,106) as SPOM_DATE,isnull(SPOM_AMOUNT,0) as SPOM_AMOUNT,isnull(SPOD_ORDER_QTY,0) as SPOD_ORDER_QTY,isnull(SPOD_RATE,0) as SPOD_RATE,isnull(SPOD_DISC_PER,0) as SPOD_DISC_PER,isnull(SPOD_DISC_AMT,0) as SPOD_DISC_AMT,isnull(SPOD_TOTAL_AMT,0) as SPOD_TOTAL_AMT,isnull(SPOD_INW_QTY,0) as SPOD_INW_QTY,isnull(SPOM_PONO,0) as SPOM_PONO,isnull(SPOD_COSTWEIGHT,0) as SPOD_COSTWEIGHT,isnull(SPOD_FINISHWEIGHT,0) as SPOD_FINISHWEIGHT,isnull(SPOD_TURNINGWEIGHT,0) as SPOD_TURNINGWEIGHT from SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,PARTY_MASTER where " + cond + " SPOM_POTYPE=1 and SUPP_PO_MASTER.ES_DELETE=0 and SUPP_PO_DETAILS.SPOD_SPOM_CODE=SUPP_PO_MASTER.SPOM_CODE and SPOM_CM_COMP_ID=" + Session["CompanyId"] + " and SPOD_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and PARTY_MASTER.ES_DELETE=0 and PARTY_MASTER.P_CODE=SUPP_PO_MASTER.SPOM_P_CODE order by SPOM_PO_NO";

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptSubConReg.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptSubConReg.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                if (cond == "")
                {
                    rptname.SetParameterValue("txtTitle", "Sub Contractor PO Register from " + From + " To " + To + "");
                }
                else
                {
                    if (From == "")
                    {
                        rptname.SetParameterValue("txtTitle", "");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtTitle", "From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "");
                    }
                }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/RoportForms/VIEW/ViewSubConReg.aspx", false);
    }
}

