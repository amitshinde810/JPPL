using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_SalePlanningReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
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
            string cond = Request.QueryString[5].ToString();

            GenerateReport(Title, From, To, group, way, cond);

            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            DateTime dtDate = Convert.ToDateTime(From);
            //Query = "SELECT ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,SO_TYPE_MASTER.SO_T_SHORT_NAME,CUSTPO_DETAIL.CPOD_ORD_QTY,CUSTPO_DETAIL.CPOD_RATE, CUSTPO_DETAIL.CPOD_AMT,CUSTPO_DETAIL.CPOD_CUST_I_NAME,CUSTPO_DETAIL.CPOD_CUST_I_CODE,CUSTPO_DETAIL.CPOD_AMORTRATE,CUSTPO_DETAIL.CPOD_DISPACH,ITEM_UNIT_MASTER.I_UOM_NAME,CUSTPO_MASTER.CPOM_PONO,CUSTPO_MASTER.CPOM_DOC_NO,CUSTPO_MASTER.CPOM_DATE,CUSTPO_MASTER.CPOM_BASIC_AMT,PROJECT_CODE_MASTER.PROCM_NAME,PARTY_MASTER.P_NAME FROM PARTY_MASTER INNER JOIN SO_TYPE_MASTER INNER JOIN CUSTPO_MASTER ON SO_TYPE_MASTER.SO_T_CODE = CUSTPO_MASTER.CPOM_TYPE INNER JOIN PROJECT_CODE_MASTER ON CUSTPO_MASTER.CPOM_PROJECT_CODE = PROJECT_CODE_MASTER.PROCM_CODE ON  PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER INNER JOIN CUSTPO_DETAIL ON ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON  CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE WHERE " + cond + " (ITEM_UNIT_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (CUSTPO_MASTER.ES_DELETE = 0) AND (PROJECT_CODE_MASTER.ES_DELETE = 0) AND (SO_TYPE_MASTER.ES_DELETE = 0) and (PARTY_MASTER.ES_DELETE=0)";
            //Query = "SELECT ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO ,sum(ISNULL(CS_SCHEDULE_QTY,0)) as CS_SCHEDULE_QTY,CUSTPO_DETAIL.CPOD_RATE,ISNULL(I_TARGET_WEIGHT,0) as PROD_FINISH_WT ,GROUP_MASTER.GP_NAME FROM CUSTPO_MASTER INNER JOIN ITEM_MASTER INNER JOIN PRODUCT_MASTER INNER JOIN GROUP_MASTER ON PRODUCT_MASTER.PROD_GP_CODE = GROUP_MASTER.GP_CODE ON ITEM_MASTER.I_CODE = PRODUCT_MASTER.PROD_I_CODE INNER JOIN CUSTPO_DETAIL ON ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN CUSTOMER_SCHEDULE C ON PRODUCT_MASTER.PROD_I_CODE=C.CS_I_CODE WHERE " + cond + " C.ES_DELETE =0 AND (CUSTPO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CM_COMP_ID = '" + Session["CompanyId"] + "') AND (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE = '-2147483648') and PRODUCT_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0 group by ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO,CPOD_RATE,I_TARGET_WEIGHT,GP_NAME ORDER BY ITEM_MASTER.I_CODENO";
            // Query = "SELECT ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO ,sum(ISNULL(CS_SCHEDULE_QTY,0)) as CS_SCHEDULE_QTY,CUSTPO_DETAIL.CPOD_RATE,ISNULL(I_TARGET_WEIGHT,0) as PROD_FINISH_WT ,GROUP_MASTER.GP_NAME FROM CUSTPO_MASTER INNER JOIN ITEM_MASTER INNER JOIN PRODUCT_MASTER INNER JOIN GROUP_MASTER ON PRODUCT_MASTER.PROD_GP_CODE = GROUP_MASTER.GP_CODE ON ITEM_MASTER.I_CODE = PRODUCT_MASTER.PROD_I_CODE INNER JOIN CUSTPO_DETAIL ON ITEM_MASTER.I_CODE = CUSTPO_DETAIL.CPOD_I_CODE ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN CUSTOMER_SCHEDULE C ON PRODUCT_MASTER.PROD_I_CODE=C.CS_I_CODE WHERE " + cond + " CPOM_DATE = (select max(CPOM_DATE)  from CUSTPO_MASTER inner join CUSTPO_DETAIL  ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE where CPOM_DATE<'" + To.ToString() + "') AND  DATEPART(MM,C.CS_DATE)='" + dtDate.ToString("MM") + "' AND  DATEPART(YYYY,C.CS_DATE)='" + dtDate.ToString("yyyy") + "' AND  C.ES_DELETE =0 AND (CUSTPO_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CM_COMP_ID = '" + Session["CompanyId"] + "') AND (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE = '-2147483648') and PRODUCT_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0 group by ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO,CPOD_RATE,I_TARGET_WEIGHT,GP_NAME ORDER BY ITEM_MASTER.I_CODENO";
            //Query = "SELECT ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO ,isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where " + cond + " CUSTOMER_SCHEDULE.CS_I_CODE=I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0),0) as CS_SCHEDULE_QTY,ISNULL(I_TARGET_WEIGHT,0) as PROD_FINISH_WT ,GROUP_MASTER.GP_NAME ,I_INV_RATE AS CPOD_RATE FROM ITEM_MASTER INNER JOIN PRODUCT_MASTER INNER JOIN GROUP_MASTER ON PRODUCT_MASTER.PROD_GP_CODE = GROUP_MASTER.GP_CODE ON ITEM_MASTER.I_CODE = PRODUCT_MASTER.PROD_I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON PRODUCT_MASTER.PROD_I_CODE=C.CS_I_CODE WHERE " + cond + " C.ES_DELETE =0 AND (ITEM_MASTER.I_CM_COMP_ID = '" + Session["CompanyId"] + "') AND (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE = '-2147483648') and PRODUCT_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0  group by ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO,I_TARGET_WEIGHT,GP_NAME,I_INV_RATE ORDER BY ITEM_MASTER.I_CODENO";
            Query = "SELECT ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO ,isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where " + cond + " CUSTOMER_SCHEDULE.CS_I_CODE=I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0),0) as CS_SCHEDULE_QTY,isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where " + cond + " CUSTOMER_SCHEDULE.CS_I_CODE=I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0),0)* ISNULL(I_TARGET_WEIGHT,0)),0)/1000 as PROD_FINISH_WT ,GROUP_MASTER.GP_NAME ,isnull((isnull((select SUM(ISNULL(CS_SCHEDULE_QTY,0)) FROM CUSTOMER_SCHEDULE where " + cond + " CUSTOMER_SCHEDULE.CS_I_CODE=I_CODE and CUSTOMER_SCHEDULE.ES_DELETE=0),0) * isnull(I_INV_RATE,0)),0)/100000 AS CPOD_RATE FROM ITEM_MASTER INNER JOIN PRODUCT_MASTER INNER JOIN GROUP_MASTER ON PRODUCT_MASTER.PROD_GP_CODE = GROUP_MASTER.GP_CODE ON ITEM_MASTER.I_CODE = PRODUCT_MASTER.PROD_I_CODE INNER JOIN CUSTOMER_SCHEDULE C ON PRODUCT_MASTER.PROD_I_CODE=C.CS_I_CODE inner join PARTY_MASTER P ON CS_P_CODE=P.P_CODE WHERE " + cond + " C.ES_DELETE =0 AND P.ES_DELETE =0 AND (ITEM_MASTER.I_CM_COMP_ID = '" + Session["CompanyId"] + "') AND (ITEM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CAT_CODE = '-2147483648') and PRODUCT_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0  group by ITEM_MASTER.I_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO,I_TARGET_WEIGHT,GP_NAME,I_INV_RATE ORDER BY ITEM_MASTER.I_CODENO";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/PPC/Reports/rptSalePlanningReport.rpt"));
                rptname.FileName = Server.MapPath("~/PPC/Reports/rptSalePlanningReport.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("title", Title);
                //rptname.SetParameterValue("txtDate","Date From : " + From + "To " + To);

                CrystalReportViewer1.ReportSource = rptname;

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
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "','" + Message + "','" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Rport", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../../ReportForms/VIEW/ViewSalePlanningReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}