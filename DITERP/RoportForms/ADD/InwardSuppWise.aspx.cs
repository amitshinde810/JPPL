using System;
using System.Web.UI;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_InwardSuppWise : System.Web.UI.Page
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
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Cond = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();
            string POType = Request.QueryString[7].ToString();
            #region MyRegion
            #endregion

            GenerateReport(Title, From, To, group, way, Cond, Type.Trim().ToString(), POType);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string condition, string Type, string POType)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            if (POType == "SUPP")
            {
                Query = "SELECT DISTINCT    INWARD_MASTER.IWM_INWARD_TYPE,INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, ROUND(IWD_CH_QTY,2) AS  SPOD_ORDER_QTY,  ROUND(IWD_REV_QTY,2) AS IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, SPOM_PO_NO, PO_TYPE_MASTER.PO_T_SHORT_NAME, PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = SUPP_PO_MASTER.SPOM_CODE INNER JOIN PO_TYPE_MASTER ON SUPP_PO_MASTER.SPOM_TYPE = PO_TYPE_MASTER.PO_T_CODE INNER JOIN PROJECT_CODE_MASTER ON SUPP_PO_MASTER.SPOM_PROJECT = PROJECT_CODE_MASTER.PROCM_CODE WHERE " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (SUPP_PO_MASTER.ES_DELETE = 0) and (PO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0)";
  
            }
            else
            {
                Query = "SELECT DISTINCT   INWARD_MASTER.IWM_INWARD_TYPE,INWARD_MASTER.IWM_NO, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_DATE, PARTY_MASTER.P_NAME AS LM_NAME,ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, INWARD_MASTER.IWM_CHALLAN_NO, INWARD_MASTER.IWM_CHAL_DATE, ROUND(IWD_CH_QTY,2) AS  SPOD_ORDER_QTY,  ROUND(IWD_REV_QTY,2) AS IWD_REV_QTY,ROUND(IWD_RATE,2) AS IWD_RATE, CPOM_PONO AS SPOM_PO_NO, SO_T_SHORT_NAME AS PO_T_SHORT_NAME, PROJECT_CODE_MASTER.PROCM_NAME as PRO_CODE  FROM ITEM_UNIT_MASTER INNER JOIN INWARD_DETAIL INNER JOIN INWARD_MASTER ON INWARD_DETAIL.IWD_IWM_CODE = INWARD_MASTER.IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = INWARD_DETAIL.IWD_UOM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN CUSTPO_MASTER ON INWARD_DETAIL.IWD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE INNER JOIN SO_TYPE_MASTER ON SO_TYPE_MASTER.SO_T_CODE = CUSTPO_MASTER.CPOM_TYPE INNER JOIN PROJECT_CODE_MASTER ON CUSTPO_MASTER.CPOM_PROJECT_CODE = PROJECT_CODE_MASTER.PROCM_CODE WHERE     " + condition + " (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) and (SO_TYPE_MASTER.ES_DELETE=0) and (PROJECT_CODE_MASTER.ES_DELETE=0)";
    
            }
            #region MyRegion

            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                #region Datewise
                if (group == "Datewise")
                {

                    if (way == "Summary")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "DateWise "+Type+" Summary Report");
                        rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                        rptname.SetParameterValue("txtReportType", "Summary Report");

                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailDateWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "DateWise " + Type + " Detail Report");
                        rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                        rptname.SetParameterValue("txtReportType", "Detail Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
                #endregion

                #region ItemWise
                if (group == "ItemWise")
                {
                    if (way == "Summary")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "Itemwise " + Type + " Summery Report");
                        rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                        rptname.SetParameterValue("txtReportType", "Summary Report");

                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailItemWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "Itemwise " + Type + " Detail Report");
                        rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                        rptname.SetParameterValue("txtReportType", "Detail Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
                #endregion

                #region SupplierWise
                if (group == "CustWise")
                {

                    if (way == "Summary")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "Supplierwise " + Type + " Summery Report");
                        rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                        rptname.SetParameterValue("txtReportType", "Summary Report");
                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptInwardDetailSuppWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);
                        rptname.SetParameterValue("txtType", "Supplierwise " + Type + " Detail Report");
                        rptname.SetParameterValue("txtDate", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                        rptname.SetParameterValue("txtReportType", "Detail Report");
                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
                #endregion
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Inward Supp Wise", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/RoportForms/VIEW/ViewInwardSuppWise.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supplierwise", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
