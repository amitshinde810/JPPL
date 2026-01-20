using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_SalesSummary : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
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
            string Condition = Request.QueryString[5];

            GenerateReport(Title, From, To, group, way, Condition);
        }
        catch (Exception Ex)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string strCondition)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            Query = "SELECT  DISTINCT(I_CODE),I_CODENO,INM_NET_AMT,I_TARGET_WEIGHT AS INTFINISHWEIGHT,I_UWEIGHT AS INTCASTWEIGHT,I_DENSITY AS BDFINISHWEIGH,I_TARGET_WEIGHT*IND_INQTY AS INTCASTWT,I_TARGET_WEIGHT*IND_INQTY AS INTFINWT ,I_DENSITY*IND_INQTY AS BDWT,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,CONVERT(VARCHAR(11),CPOM_DATE,113) as CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,cast(isnull(IND_PAK_QTY,0) as numeric(20,3)) as IND_PAK_QTY,  CASE when INM_IS_SUPPLIMENT =1 then 0 else  cast(isnull(IND_INQTY,0) as numeric(20,3)) END as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT ,I_UOM_NAME,CASE WHEN INM_TYPE='TAXINV' then 'Sales'  WHEN INM_TYPE='OutJWINM' then 'Labour Charge' END AS INM_TYPE,INM_TNO FROM INVOICE_MASTER,INVOICE_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER WHERE INM_CODE=IND_INM_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND IND_CPOM_CODE =CPOM_CODE AND IND_I_CODE=CPOD_I_CODE AND I_CODE=IND_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE  " + strCondition + "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            Query = Query + " GROUP BY INM_LR_DATE,I_CODENO,I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,INM_VEH_NO,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_IS_SUPPLIMENT,INM_TNO,I_UOM_NAME,INM_TYPE,I_TARGET_WEIGHT,I_UWEIGHT,I_DENSITY";
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSalessummaryDatewise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSalessummaryDatewise.rpt");
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
                    rptname.SetParameterValue("txtDatePeriod", "From " + From + " To " + To);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSalessummaryItemwise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSalessummaryItemwise.rpt");

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
                    rptname.SetParameterValue("txtDatePeriod", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSalessummaryPartywise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSalessummaryPartywise.rpt");
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
                    rptname.SetParameterValue("txtDatePeriod", "From " + From + " To " + To);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptSalessummaryPartyItemwise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptSalessummaryPartyItemwise.rpt");
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
                    rptname.SetParameterValue("txtDatePeriod", "From " + From + " To " + To);
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
            CommonClasses.SendError("Tax Invoice Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewSalesSummary.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
