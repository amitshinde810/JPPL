using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_CustomerRejectionRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerRejectionRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Condition = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, group, way, Condition);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            Query = "SELECT CR_GIN_NO, CR_GIN_DATE,CR_CHALLAN_DATE,CR_CHALLAN_NO,CR_INV_DATE,CR_INV_NO,CR_TYPE,CR_P_CODE,P_NAME,CR_NET_AMT,CD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CD_UOM,CD_PO_CODE,CPOM_PONO,cast(CD_RATE as numeric(20,2)) as Rate,cast(CD_ORIGIONAL_QTY as numeric(10,3)) as  OriginalQty,cast(CD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(CD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(CD_AMOUNT as numeric(20,2)) as  Amount FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER,PARTY_MASTER where  " + Condition + " CUSTREJECTION_MASTER.CR_P_CODE=P_CODE and CUSTREJECTION_DETAIL.CD_CR_CODE=CR_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=CD_UOM and CPOM_CODE=CD_PO_CODE and I_CODE=CD_I_CODE and CUSTREJECTION_MASTER.ES_DELETE=0 and CUSTPO_MASTER.ES_DELETE=0 and ITEM_UNIT_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND ISNULL(CR_TRANS_TYPE,0)=0 and PARTY_MASTER.ES_DELETE=0";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustRejectDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustRejectDateWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtdate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustRejectItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustRejectItemWise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtdate", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustRejectCustWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustRejectCustWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtdate","From "+ From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
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
}
