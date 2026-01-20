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
public partial class RoportForms_ADD_StoreToStorePrint : System.Web.UI.Page
{
    #region Variable
    DatabaseAccessLayer DL_DBAccess = null;
    string Store_code = "";
    string type = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            Store_code = Request.QueryString[0];
            type = Request.QueryString[1];
            GenerateReport(Store_code, type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion Page_Init

    #region GenerateReport
    private void GenerateReport(string Store_code, string printType)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataTable dtfinal = new DataTable();
            //dtfinal = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE as MR_CODE,IM_NO as MR_BATCH_NO,IM_DATE as MR_DATE,IM_TYPE as MR_TYPE,IMD_I_CODE as MRD_I_CODE,IMD_REQ_QTY as MRD_REQ_QTY,IMD_ISSUE_QTY as MRD_ISSUE_QTY,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER where ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE and ISSUE_MASTER.IM_CODE='" + Store_code + "'  AND IM_UM_CODE=UM_CODE ");
            
            if (printType == "STORE")
                dtfinal = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE as MR_CODE,STORE_MASTER.STORE_PREFIX + + CONVERT(varchar,IM_NO) as MR_BATCH_NO,IM_DATE as MR_DATE,IM_TYPE as MR_TYPE,IMD_I_CODE as MRD_I_CODE,IMD_REQ_QTY as MRD_REQ_QTY,IMD_ISSUE_QTY as MRD_ISSUE_QTY,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME ,STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME],STORE_MASTER1.STORE_NAME AS [TO_STORE_NAME],ISNULL(E_TARIFF_NO,'') AS E_TARIFF_NO  FROM ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER ,STORE_MASTER ,STORE_MASTER AS STORE_MASTER1,EXCISE_TARIFF_MASTER WHERE ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE and ISSUE_MASTER.IM_CODE='" + Store_code + "'  AND IM_UM_CODE = UM_CODE AND I_E_CODE=E_CODE  AND ISSUE_MASTER.IM_FROM_STORE=STORE_MASTER.STORE_CODE AND STORE_MASTER1.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE AND STORE_MASTER.ES_DELETE = '0' AND STORE_MASTER1.ES_DELETE = '0'");
            else // round((ISNULL(IMD_RATE,0) * 60)/100,2) for casting (only casting items are used in movement
                dtfinal = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE as MR_CODE,STORE_MASTER.STORE_PREFIX + + CONVERT(varchar,IM_NO) as MR_BATCH_NO,IM_DATE as MR_DATE,IM_TYPE as MR_TYPE,IMD_I_CODE as MRD_I_CODE,IMD_REQ_QTY as MRD_REQ_QTY,IMD_ISSUE_QTY as MRD_ISSUE_QTY,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,round((ISNULL(IMD_RATE,0) * 60)/100,2) AS IMD_RATE,round(round((ISNULL(IMD_RATE,0) * 60)/100,2) * isnull(IMD_ISSUE_QTY,0) ,2) as IMD_AMOUNT,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT1 ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME ,STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME],STORE_MASTER1.STORE_NAME AS [TO_STORE_NAME] ,ISNULL(E_TARIFF_NO,'') AS E_TARIFF_NO ,STORE_MASTER.STORE_ADDRESS AS [FROM_STORE_ADDRESS],STORE_MASTER1.STORE_ADDRESS AS [TO_STORE_ADDRESS] FROM ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER,STORE_MASTER,STORE_MASTER AS STORE_MASTER1,EXCISE_TARIFF_MASTER WHERE ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE and ISSUE_MASTER.IM_CODE='" + Store_code + "'  AND IM_UM_CODE = UM_CODE AND ISSUE_MASTER.IM_FROM_STORE=STORE_MASTER.STORE_CODE AND STORE_MASTER1.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE AND STORE_MASTER.ES_DELETE = '0' AND STORE_MASTER1.ES_DELETE = '0' AND I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE='0'");
            
            if (dtfinal.Rows.Count > 0)
            {

                DataTable dtTaxInvoice = new DataTable();
                DataTable dtTemp = new DataTable();
                DataSet dsTaxInvoiceGST = new DataSet();

                try
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    string Title = "";
                    if (printType == "STORE")
                    {//For Store to store print print
                        Title = "Store To Store Movement";
                        rptname.Load(Server.MapPath("~/Reports/rptStoreToStorePrint.rpt"));  //rptMaterialRequisitionPrint
                        rptname.FileName = Server.MapPath("~/Reports/rptStoreToStorePrint.rpt");

                        rptname.Refresh();
                        rptname.SetDataSource(dtfinal);
                        DateTime Datetime = DateTime.Now;
                        DataTable gst = CommonClasses.Execute("SELECT CM_GST_NO  FROM COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"].ToString() + "' ");
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtDateTime", Datetime.ToString("dd/MMM/yyyy hh:mm"));
                        rptname.SetParameterValue("txtTitle", Title);
                        rptname.SetParameterValue("txtGST", gst.Rows[0]["CM_GST_NO"].ToString());
                        CrystalReportViewer1.ReportSource = rptname;

                    }
                    else
                    {//For Multi Store print(Internal Stock Transfer)



                        dtTemp.Columns.Add("MR_CODE", typeof(int));
                        dtTemp.Columns.Add("INM_NO", typeof(int));
                        dtTemp.Columns.Add("INM_NAME", typeof(string));
                        dtTemp.Columns.Add("INM_SEQNO", typeof(int));


                        DataRow dr = dtTemp.NewRow();
                        dr["MR_CODE"] = Store_code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Original for Consignee";
                        dr["INM_SEQNO"] = 1;
                        dtTemp.Rows.Add(dr);

                        dr = dtTemp.NewRow();
                        dr["MR_CODE"] = Store_code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Duplicate for Transporter";
                        dr["INM_SEQNO"] = 2;
                        dtTemp.Rows.Add(dr);

                        dr = dtTemp.NewRow();
                        dr["MR_CODE"] = Store_code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Triplicate for consignor";
                        dr["INM_SEQNO"] = 3;
                        dtTemp.Rows.Add(dr);



                        dsTaxInvoiceGST.Tables.Add(dtfinal);
                        dsTaxInvoiceGST.Tables[0].TableName = "dtMaterialRequisitionPrint";
                        DataTable dta = new DataTable();
                        dta = (DataTable)dtTemp;
                        dsTaxInvoiceGST.Tables.Add(dta);
                        dsTaxInvoiceGST.Tables[1].TableName = "dtPrint";

                        Title = "Stock Transfer Challan";
                        rptname.Load(Server.MapPath("~/Reports/rptStoreToStoreMultiStorePrint.rpt"));  //rptMaterialRequisitionPrint
                        rptname.FileName = Server.MapPath("~/Reports/rptStoreToStoreMultiStorePrint.rpt");


                        rptname.Refresh();
                        rptname.SetDataSource(dsTaxInvoiceGST);
                        DateTime Datetime = DateTime.Now;
                        DataTable gst = CommonClasses.Execute("SELECT CM_GST_NO  FROM COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"].ToString() + "' ");
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtDateTime", Datetime.ToString("dd/MMM/yyyy hh:mm"));
                        rptname.SetParameterValue("txtTitle", Title);
                        rptname.SetParameterValue("txtGST", gst.Rows[0]["CM_GST_NO"].ToString());
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                   
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("StoreToStore Print","GenerateReport",Ex.Message);
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
    #endregion GenerateReport

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
            CommonClasses.SendError("StoreToStore Print", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewIssue.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("StoreToStore Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
