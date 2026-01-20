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
public partial class RoportForms_ADD_StoreToStoreCastingPrint : System.Web.UI.Page
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
            else
            {
                string type = DL_DBAccess.GetColumn("select IM_TYPE from ISSUE_MASTER where IM_CODE='" + code + "'");
                if (type == "1")
                {
                    dtfinal = CommonClasses.Execute("SELECT   dbo.ISSUE_MASTER.IM_CODE AS MR_CODE, dbo.MATERIAL_REQUISITION_MASTER.MR_BATCH_NO as MR_BATCH_NO, dbo.ISSUE_MASTER.IM_DATE AS MR_DATE,dbo.ISSUE_MASTER.IM_TYPE AS MR_TYPE, dbo.MATERIAL_REQUISITION_MASTER.MR_I_CODE, dbo.ISSUE_MASTER_DETAIL.IMD_I_CODE AS MRD_I_CODE, dbo.ISSUE_MASTER_DETAIL.IMD_REQ_QTY AS MRD_REQ_QTY,dbo.ISSUE_MASTER_DETAIL.IMD_ISSUE_QTY AS MRD_ISSUE_QTY, I1.I_NAME AS FINISH_I_NAME, I1.I_CODENO AS FINISH_I_CODENO, I2.I_NAME AS RAW_I_NAME, I2.I_CODENO AS RAW_I_CODENO, dbo.SHADE_MASTER.SHM_FORMULA_CODE as MR_FORMULA, dbo.BATCH_MASTER.BT_NO,'Store' as MR_DEPT_NAME,dbo.ISSUE_MASTER_DETAIL.IMD_REQ_QTY AS MRD_ADD_IN FROM dbo.SHADE_MASTER INNER JOIN  dbo.BATCH_MASTER ON dbo.SHADE_MASTER.SHM_CODE = dbo.BATCH_MASTER.BT_SHM_CODE INNER JOIN  dbo.ITEM_MASTER AS I2 INNER JOIN  dbo.ISSUE_MASTER INNER JOIN dbo.ISSUE_MASTER_DETAIL ON dbo.ISSUE_MASTER.IM_CODE = dbo.ISSUE_MASTER_DETAIL.IM_CODE ON I2.I_CODE = dbo.ISSUE_MASTER_DETAIL.IMD_I_CODE INNER JOIN  dbo.ITEM_MASTER AS I1 INNER JOIN  dbo.MATERIAL_REQUISITION_MASTER ON I1.I_CODE = dbo.MATERIAL_REQUISITION_MASTER.MR_I_CODE ON  dbo.ISSUE_MASTER.IM_MATERIAL_REQ = dbo.MATERIAL_REQUISITION_MASTER.MR_CODE ON  dbo.BATCH_MASTER.BT_CODE = dbo.MATERIAL_REQUISITION_MASTER.MR_BATCH_CODE WHERE (dbo.MATERIAL_REQUISITION_MASTER.ES_DELETE = 0) AND (dbo.ISSUE_MASTER.IM_CODE = '" + code + "')");
                }
                else
                {
                    //dtfinal = CommonClasses.Execute("select ISSUE_MASTER.IM_CODE as MR_CODE,IM_NO as MR_BATCH_NO,IM_DATE as MR_DATE,IM_TYPE as MR_TYPE,IMD_I_CODE as MRD_I_CODE,IMD_REQ_QTY as MRD_REQ_QTY,IMD_ISSUE_QTY as MRD_ISSUE_QTY,I2.I_NAME as RAW_I_NAME,I2.I_CODENO as RAW_I_CODENO,ISNULL(IMD_RATE,0) AS IMD_RATE,ISNULL(IMD_AMOUNT,0) AS IMD_AMOUNT ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,UM_NAME,UM_USERNAME from ISSUE_MASTER,ISSUE_MASTER_DETAIL,ITEM_MASTER I2,USER_MASTER where ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE  and IMD_I_CODE=I2.I_CODE and ISSUE_MASTER.IM_CODE='" + code + "'  AND IM_UM_CODE=UM_CODE ");
                    dtfinal = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE AS MR_CODE, ISSUE_MASTER.IM_NO AS MR_BATCH_NO, ISSUE_MASTER.IM_DATE AS MR_DATE, ISSUE_MASTER.IM_TYPE AS MR_TYPE, ISSUE_MASTER_DETAIL.IMD_I_CODE AS MRD_I_CODE, ISSUE_MASTER_DETAIL.IMD_REQ_QTY AS MRD_REQ_QTY, ISSUE_MASTER_DETAIL.IMD_ISSUE_QTY AS MRD_ISSUE_QTY, I2.I_NAME AS RAW_I_NAME, I2.I_CODENO AS RAW_I_CODENO, ISNULL(ISSUE_MASTER_DETAIL.IMD_RATE, 0) AS IMD_RATE, ISNULL(ISSUE_MASTER_DETAIL.IMD_AMOUNT, 0) AS IMD_AMOUNT, ISSUE_MASTER.IM_ISSUEBY, ISSUE_MASTER.IM_REQBY, ISSUE_MASTER.IM_UM_CODE, USER_MASTER.UM_NAME, USER_MASTER.UM_USERNAME, STORE_MASTER.STORE_NAME as FromStore, STORE_MASTER_1.STORE_NAME AS ToStore FROM STORE_MASTER INNER JOIN ISSUE_MASTER INNER JOIN ISSUE_MASTER_DETAIL ON ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE INNER JOIN ITEM_MASTER AS I2 ON ISSUE_MASTER_DETAIL.IMD_I_CODE = I2.I_CODE INNER JOIN USER_MASTER ON ISSUE_MASTER.IM_UM_CODE = USER_MASTER.UM_CODE ON STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE INNER JOIN STORE_MASTER AS STORE_MASTER_1 ON ISSUE_MASTER_DETAIL.IMD_To_STORE = STORE_MASTER_1.STORE_CODE WHERE (ISSUE_MASTER.IM_CODE = '" + code + "')");
                }
            }

            if (dtfinal.Rows.Count > 0)
            {
                try
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/StoreToStoreCastingPrint.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/StoreToStoreCastingPrint.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    if (printType == "REQUSITION")
                    {
                        rptname.SetParameterValue("txtTitle", "Material Requisition");
                    }
                    else
                    {
                        rptname.SetParameterValue("txtTitle", "Store To Store Casting");
                    }
                    CrystalReportViewer1.ReportSource = rptname;


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
