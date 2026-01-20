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

public partial class RoportForms_ADD_FillOffSheetPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cpom_code = "";
    string print_type = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        cpom_code = Request.QueryString[0];
        GenerateReport(cpom_code);
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewFillOffSheet.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Fill Off Sheet Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion


    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        try
        {

            string BatchType = DL_DBAccess.GetColumn("select isnull(BT_TYPE,0) as BT_TYPE from BATCH_MASTER,FILL_OFF_SHEET where FOS_BT_CODE=BT_CODE and BATCH_MASTER.ES_DELETE=0 and FOS_CODE='" + code + "'");
            if (BatchType == "1")
            {
                Query = "select BT_NO,SHM_FORMULA_CODE as SHM_FORMULA_NAME,P_NAME,FOS_FINAL_YIELD,FOS_NOTES,FOS_FILTER_USED,FOSD_TYPE,FOSD_QTY,FOSD_WGT,I_UOM_NAME as UOM_NAME,FOS_DATE,FOS_FILL_DATE,sum(isnull(BTD_QTY,0)) as QTY_IN_LTR,sum(isnull(BTD_WGT,0)) as WEIGHT_IN_KG,(CASE FOSD_TYPE WHEN 1 THEN 'Package' WHEN 2 THEN 'Extra' WHEN 3 THEN 'Sample' END) AS Type_Name from BATCH_MASTER,BATCH_DETAIL,SHADE_MASTER,WORK_ORDER_MASTER,PARTY_MASTER,FILL_OFF_SHEET,FILL_OFF_SHEET_DETAIL,ITEM_UNIT_MASTER where BT_CODE=BTD_BT_CODE and  BT_SHM_CODE=SHM_CODE and FOS_BT_CODE=BT_CODE and FOS_CODE=FOSD_FOS_CODE and SHADE_MASTER.ES_DELETE=0 and BT_WO_CODE=WO_CODE and WORK_ORDER_MASTER.ES_DELETE=0 and WO_P_CODE=P_CODE and PARTY_MASTER.ES_DELETE=0 and FOSD_UOM_CODE=I_UOM_CODE and  ITEM_UNIT_MASTER.ES_DELETE=0 and FOS_CODE='" + code + "' group by BT_NO,SHM_FORMULA_CODE,P_NAME,FOS_APPROVED_BY,FOS_SPECIAL_INSTR,FOS_FILL_BY,FOS_FINAL_YIELD,FOS_NOTES,FOS_FILTER_USED,FOSD_TYPE,FOSD_QTY,FOSD_WGT,I_UOM_NAME,FOS_DATE,FOS_FILL_DATE order by FOSD_TYPE";
            }
            else
            {
                Query = "select BT_NO,SHM_FORMULA_CODE as SHM_FORMULA_NAME,'' as P_NAME,FOS_FINAL_YIELD,FOS_NOTES,FOS_FILTER_USED,FOSD_TYPE,FOSD_QTY,FOSD_WGT,I_UOM_NAME as UOM_NAME,FOS_DATE,FOS_FILL_DATE,sum(isnull(BTD_QTY,0)) as QTY_IN_LTR,sum(isnull(BTD_WGT,0)) as WEIGHT_IN_KG,(CASE FOSD_TYPE WHEN 1 THEN 'Package' WHEN 2 THEN 'Extra' WHEN 3 THEN 'Sample' END) AS Type_Name from BATCH_MASTER,BATCH_DETAIL,SHADE_MASTER,FILL_OFF_SHEET,FILL_OFF_SHEET_DETAIL,ITEM_UNIT_MASTER where BT_CODE=BTD_BT_CODE and  BT_SHM_CODE=SHM_CODE and FOS_BT_CODE=BT_CODE and FOS_CODE=FOSD_FOS_CODE and SHADE_MASTER.ES_DELETE=0 and  FOSD_UOM_CODE=I_UOM_CODE and  ITEM_UNIT_MASTER.ES_DELETE=0 and FOS_CODE='" + code + "' group by BT_NO,SHM_FORMULA_CODE,FOS_APPROVED_BY,FOS_SPECIAL_INSTR,FOS_FILL_BY,FOS_FINAL_YIELD,FOS_NOTES,FOS_FILTER_USED,FOSD_TYPE,FOSD_QTY,FOSD_WGT,I_UOM_NAME,FOS_DATE,FOS_FILL_DATE order by FOSD_TYPE";
            }
            DataTable dtfinal = CommonClasses.Execute(Query);

            if (dtfinal.Rows.Count > 0)
            {

                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptFillOffSheetPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptFillOffSheetPrint.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);             
                CrystalReportViewer1.ReportSource = rptname;

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Fill Off Sheet Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
