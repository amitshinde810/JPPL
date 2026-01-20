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

public partial class RoportForms_ADD_MaterialRequisitionPrint : System.Web.UI.Page
{
    #region Variable
    DatabaseAccessLayer DL_DBAccess = null;
    string MatReq_code = "";
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
            MatReq_code = Request.QueryString[0];
            type = Request.QueryString[1];
            GenerateReport(MatReq_code, type);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Print", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string code, string printType)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            DataTable dtfinal = new DataTable();
            if (printType == "REQUSITION")
            {
                dtfinal = CommonClasses.Execute("select  MR_CODE,MR_BATCH_NO,MR_DATE,MR_TYPE,MR_CPOM_CODE,MR_I_CODE,MRD_I_CODE,isnull(MRD_ADD_IN,0) as MRD_ADD_IN,isnull(MRD_REQ_QTY,0) as MRD_REQ_QTY,MRD_ISSUE_QTY,MRD_PROD_QTY,MRD_PURC_REQ_QTY,(I1.I_NAME) as FINISH_I_NAME,I1.I_CODENO as FINISH_I_CODENO,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,MR_DEPT_NAME,BT_NO,SHM_FORMULA_CODE as MR_FORMULA from MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,BATCH_MASTER,SHADE_MASTER,ITEM_MASTER as I1,ITEM_MASTER I2 where BT_SHM_CODE=SHM_CODE and MR_BATCH_CODE=BT_CODE and MR_CODE=MRD_MR_CODE and MR_I_CODE=I1.I_CODE and MRD_I_CODE=I2.I_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0  and MR_CODE='" + code + "'");
            }
            else if (printType.Trim() == "ISSUE")
            {
                string type = DL_DBAccess.GetColumn("select IM_TYPE from ISSUE_MASTER where IM_CODE='" + code + "'");
                if (type == "1")
                    dtfinal = CommonClasses.Execute("SELECT   dbo.ISSUE_MASTER.IM_CODE AS MR_CODE, dbo.MATERIAL_REQUISITION_MASTER.MR_BATCH_NO as MR_BATCH_NO, dbo.ISSUE_MASTER.IM_DATE AS MR_DATE,dbo.ISSUE_MASTER.IM_TYPE AS MR_TYPE, dbo.MATERIAL_REQUISITION_MASTER.MR_I_CODE, dbo.ISSUE_MASTER_DETAIL.IMD_I_CODE AS MRD_I_CODE, dbo.ISSUE_MASTER_DETAIL.IMD_REQ_QTY AS MRD_REQ_QTY,dbo.ISSUE_MASTER_DETAIL.IMD_ISSUE_QTY AS MRD_ISSUE_QTY, I1.I_NAME AS FINISH_I_NAME, I1.I_CODENO AS FINISH_I_CODENO, I2.I_NAME AS RAW_I_NAME, I2.I_CODENO AS RAW_I_CODENO, dbo.SHADE_MASTER.SHM_FORMULA_CODE as MR_FORMULA, dbo.BATCH_MASTER.BT_NO,'Store' as MR_DEPT_NAME,dbo.ISSUE_MASTER_DETAIL.IMD_REQ_QTY AS MRD_ADD_IN FROM dbo.SHADE_MASTER INNER JOIN  dbo.BATCH_MASTER ON dbo.SHADE_MASTER.SHM_CODE = dbo.BATCH_MASTER.BT_SHM_CODE INNER JOIN  dbo.ITEM_MASTER AS I2 INNER JOIN  dbo.ISSUE_MASTER INNER JOIN dbo.ISSUE_MASTER_DETAIL ON dbo.ISSUE_MASTER.IM_CODE = dbo.ISSUE_MASTER_DETAIL.IM_CODE ON I2.I_CODE = dbo.ISSUE_MASTER_DETAIL.IMD_I_CODE INNER JOIN  dbo.ITEM_MASTER AS I1 INNER JOIN  dbo.MATERIAL_REQUISITION_MASTER ON I1.I_CODE = dbo.MATERIAL_REQUISITION_MASTER.MR_I_CODE ON  dbo.ISSUE_MASTER.IM_MATERIAL_REQ = dbo.MATERIAL_REQUISITION_MASTER.MR_CODE ON  dbo.BATCH_MASTER.BT_CODE = dbo.MATERIAL_REQUISITION_MASTER.MR_BATCH_CODE WHERE (dbo.MATERIAL_REQUISITION_MASTER.ES_DELETE = 0) AND (dbo.ISSUE_MASTER.IM_CODE = '" + code + "')");
                else
                    dtfinal = CommonClasses.Execute("select ISSUE_MASTER.IM_CODE as MR_CODE,IM_NO as MR_BATCH_NO,IM_DATE as MR_DATE,IM_TYPE as MR_TYPE,IMD_I_CODE as MRD_I_CODE,IMD_REQ_QTY as MRD_REQ_QTY,IMD_ISSUE_QTY as MRD_ISSUE_QTY,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER where ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE and ISSUE_MASTER.IM_CODE='" + code + "'  AND IM_UM_CODE=UM_CODE ");
            }
            else
            {
                dtfinal = CommonClasses.Execute("select ISSUE_MASTER.IM_CODE as MR_CODE,IM_NO as MR_BATCH_NO,IM_DATE as MR_DATE,IM_TYPE as MR_TYPE,IMD_I_CODE as MRD_I_CODE,IMD_REQ_QTY as MRD_REQ_QTY,IMD_ISSUE_QTY as MRD_ISSUE_QTY,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME,ISNULL(E_TARIFF_NO,'') AS E_TARIFF_NO ,ISNULL((SELECT STORE_ADDRESS FROM STORE_MASTER WHERE STORE_CODE='-2147483647' AND ES_DELETE='0'),'') AS FROM_STORE_ADDRESS ,ISNULL((SELECT STORE_ADDRESS FROM STORE_MASTER WHERE STORE_CODE='-2147483646' AND ES_DELETE='0'),'') AS TO_STORE_ADDRESS ,round(E_BASIC *IMD_AMOUNT/100,2) AS CGST,round(E_EDU_CESS*IMD_AMOUNT/100,2) AS SGST  from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER,EXCISE_TARIFF_MASTER  where ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE and ISSUE_MASTER.IM_CODE='" + code + "' AND IM_UM_CODE=UM_CODE AND I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE='0'");
            }
            DataTable dtTaxInvoice = new DataTable();
            DataTable dtTemp = new DataTable();
            DataSet dsTaxInvoiceGST = new DataSet();

            if (dtfinal.Rows.Count > 0)
            {
                try
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    if (printType == "ISSUE")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptMaterialRequisitionPrint.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptMaterialRequisitionPrint.rpt");

                        rptname.Refresh();
                        rptname.SetDataSource(dtfinal);
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        if (printType == "REQUSITION")
                        {
                            rptname.SetParameterValue("txtTitle", "Material Requisition");
                        }
                        else if (printType == "ISSUE")
                        {
                            rptname.SetParameterValue("txtTitle", "Issue To Production");
                        }
                        else
                        {
                            rptname.SetParameterValue("txtTitle", "Stock Transfer Challan");
                        }
                        DataTable gst = CommonClasses.Execute("SELECT CM_GST_NO  FROM COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"].ToString() + "' ");
                        rptname.SetParameterValue("txtGST", gst.Rows[0]["CM_GST_NO"].ToString());
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    else
                    { 

                        dtTemp.Columns.Add("MR_CODE", typeof(int));
                        dtTemp.Columns.Add("INM_NO", typeof(int));
                        dtTemp.Columns.Add("INM_NAME", typeof(string));
                        dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                         
                            DataRow dr = dtTemp.NewRow();
                            dr["MR_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Original for Consignee";
                            dr["INM_SEQNO"] = 1;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["MR_CODE"] = code;
                            dr["INM_NO"] = 1;
                            dr["INM_NAME"] = "Duplicate for Transporter";
                            dr["INM_SEQNO"] = 2;
                            dtTemp.Rows.Add(dr);

                            dr = dtTemp.NewRow();
                            dr["MR_CODE"] = code;
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


                        rptname.Load(Server.MapPath("~/Reports/rptMaterialRequisitionMultiStorePrint.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptMaterialRequisitionMultiStorePrint.rpt");

                        rptname.Refresh();
                        rptname.SetDataSource(dsTaxInvoiceGST);
                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                        rptname.SetParameterValue("txtTitle", "Stock Transfer Challan");

                        DataTable gst = CommonClasses.Execute("SELECT CM_GST_NO  FROM COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"].ToString() + "' ");
                        rptname.SetParameterValue("txtGST", gst.Rows[0]["CM_GST_NO"].ToString());
                        CrystalReportViewer1.ReportSource = rptname;
                    }



                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Material Requisition Print", "GenerateReport", Ex.Message);

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
            CommonClasses.SendError("Material Requisition Print", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (type == "REQUSITION")
            {
                Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
            }
            else
            {
                Response.Redirect("~/Transactions/VIEW/ViewIssueToProduction.aspx", false);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
