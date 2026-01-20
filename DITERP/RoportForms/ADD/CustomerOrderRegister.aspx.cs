using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
public partial class RoportForms_ADD_CustomerOrderRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewCustomerOrderRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order Register", "btnCancel_Click", Ex.Message);
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
            string cond = Request.QueryString[5].ToString();
            string withVal = Request.QueryString[6].ToString();
            // string OutwardType = Request.QueryString[7].ToString();

            GenerateReport(Title, From, To, group, way, cond, withVal);

            // i_name = i_name.Replace("'", "''");

            #region Detail
            if (way == "Detail")
            {
            }
            if (way == "Summary")
            {

            }
            if (group == "Datewise")
            {

            }
            if (group == "ItemWise")
            {

            }
            if (group == "CustWise")
            {

            }
            #endregion
        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string cond, string withVal)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "SELECT Distinct CPOD_I_CODE,CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT, CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT FROM PARTY_MASTER INNER JOIN CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN LOG_MASTER ON CUSTPO_MASTER.CPOM_CODE = LOG_MASTER.LG_DOC_CODE ON PARTY_MASTER.P_CODE = CUSTPO_MASTER.CPOM_P_CODE INNER JOIN ITEM_MASTER ON CUSTPO_DETAIL.CPOD_I_CODE = ITEM_MASTER.I_CODE where CUSTPO_MASTER.ES_DELETE=0 and  LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";
            // Query = "SELECT Distinct CPOD_I_CODE,ITEM_MASTER.I_CODENO,CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT,CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_CODE,ST_SALES_TAX,cast(((CPOD_AMT*E_BASIC/100)+((CPOD_AMT*E_BASIC/100)*E_EDU_CESS/100)+((CPOD_AMT*E_BASIC/100)*E_H_EDU/100)) as numeric(10,3)) as EXC_AMT,cast((CPOD_AMT*ST_SALES_TAX/100) as numeric(10,3)) as TAX_AMT FROM  INVOICE_MASTER,PARTY_MASTER ,CUSTPO_MASTER,CUSTPO_DETAIL,LOG_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,SALES_TAX_MASTER WHERE " + cond + " I_E_CODE=E_CODE and CPOM_CODE = CPOD_CPOM_CODE AND CPOM_CODE = LG_DOC_CODE AND CPOM_P_CODE=INM_P_CODE and P_CODE = CPOM_P_CODE AND CPOD_I_CODE = I_CODE AND CPOD_ST_CODE=ST_CODE AND CUSTPO_MASTER.ES_DELETE=0 and LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";
            Query = "SELECT Distinct INM_NO,I_UOM_NAME,INM_TYPE,CPOM_T_PER,CPOM_T_AMT,isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT,CPOM_BASIC_AMT,CPOM_DISCOUNT_PER,CPOM_DISCOUNT_AMT,CPOM_DISCOUNT_REASON,	CPOM_DEVIATION_AMT,CPOM_DEVIATION_REASON,CPOM_PACKING_AMT,CPOM_EXC_PER,CPOM_EXC_EDU_PER,CPOM_EXC_HEDU_PER,CPOM_EXC_AMT,CPOM_ROUNDING,CPOM_GRAND_TOT,CPOD_I_CODE,ITEM_MASTER.I_CODENO,CPOM_CODE,CPOM_PONO,CPOM_DATE,CPOM_CR_DAYS,CPOM_T_AMT,CPOM_BASIC_AMT,CPOM_DISCOUNT_AMT,CPOM_DEVIATION_AMT,CPOM_PACKING_AMT,CPOM_EXC_AMT,CPOM_GRAND_TOT,PARTY_MASTER.P_NAME,ITEM_MASTER.I_NAME,LOG_MASTER.LG_U_NAME,CPOM_P_CODE,CPOD_I_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOD_AMT,E_BASIC,E_SPECIAL,E_EDU_CESS,E_H_EDU,ST_CODE,ST_SALES_TAX,cast(((CPOD_AMT*E_BASIC/100)+((CPOD_AMT*E_BASIC/100)*E_EDU_CESS/100)+((CPOD_AMT*E_BASIC/100)*E_H_EDU/100)) as numeric(10,3)) as EXC_AMT,cast((CPOD_AMT*ST_SALES_TAX/100) as numeric(10,3)) as TAX_AMT FROM ITEM_UNIT_MASTER,USER_MASTER,INVOICE_MASTER,INVOICE_DETAIL,PARTY_MASTER ,CUSTPO_MASTER,CUSTPO_DETAIL,LOG_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,SALES_TAX_MASTER WHERE " + cond + " I_E_CODE=E_CODE and CPOD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and CPOM_CODE = CPOD_CPOM_CODE AND LG_U_CODE=UM_CODE and INVOICE_DETAIL.IND_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE and CPOM_CODE = LG_DOC_CODE AND INM_CODE=IND_INM_CODE and ITEM_MASTER.I_CODE=INVOICE_DETAIL.IND_I_CODE and P_CODE = CPOM_P_CODE AND CPOD_I_CODE = I_CODE AND CPOD_ST_CODE=ST_CODE and INVOICE_MASTER.ES_DELETE=0 AND CUSTPO_MASTER.ES_DELETE=0 and LOG_MASTER.LG_DOC_NAME='Customer Order' and LOG_MASTER.LG_EVENT='save'";
            #region Detail
            if (way == "Detail")
            {
            }
            #endregion

            #region Summary
            if (way == "Summary")
            {
            }
            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustOrderDatewiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustOrderDatewiseReg.rpt");
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
                    rptname.SetParameterValue("txtIsSupplimentaryFlag", dt.Rows[0]["INM_IS_SUPPLIMENT"].ToString());
                    rptname.SetParameterValue("txtWithVal", withVal);
                    //rptname.SetParameterValue("txtItemCode", dt.Rows[0]["I_CODENO"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustOrderItemwiseRegnew.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustOrderItemwiseRegnew.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

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
                    rptname.SetParameterValue("txtIsSupplimentaryFlag", dt.Rows[0]["INM_IS_SUPPLIMENT"].ToString());
                    rptname.SetParameterValue("txtWithVal", withVal);
                    // rptname.SetParameterValue("txtItemCode", dt.Rows[0]["I_CODENO"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCustOrderCustmerwiseRegnew.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCustOrderCustmerwiseRegnew.rpt");
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
                    rptname.SetParameterValue("txtIsSupplimentaryFlag", dt.Rows[0]["INM_IS_SUPPLIMENT"].ToString());
                    rptname.SetParameterValue("txtWithVal", withVal);
                    // rptname.SetParameterValue("txtItemCode", dt.Rows[0]["I_CODENO"].ToString());

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
            CommonClasses.SendError("Customer Order Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
