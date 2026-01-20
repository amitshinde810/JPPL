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

public partial class RoportForms_ADD_dsTinterSheetPrint : System.Web.UI.Page
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
            Response.Redirect("~/Transactions/VIEW/ViewTinterSheet.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet Print", "btnCancel_Click", Ex.Message);
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
            //string BatchType = DL_DBAccess.GetColumn("select isnull(BT_TYPE,0) as BT_TYPE from BATCH_MASTER where BT_CODE='" + code + "'");
            //if (BatchType == "1")
            //{
            //    Query = "SELECT TSM_CODE,TSM_FORMULA,TSM_DATE,BT_NO,P_NAME,TSM_LITERS,TSM_KG,TSM_TINTER_NAME,TSM_INITIAL_DL,TSM_INITIAL_DA,TSM_INITIAL_DB,TSM_INITIAL_GLOSS,TSM_INITIAL_VISCOSITY,TSM_CUP,TSM_INITIAL_SG,TSM_FINAL_DL,TSM_FINAL_DA,TSM_FINAL_DB,TSM_FINAL_GLOSS,TSM_FINAL_VISCOSITY,TSM_FINAL_SG,TSM_CHECKED_BY,TSM_APPROVED_BY,I_CODE,I_CODENO,TSD_LOT,TSD_QTY,TSD_ADD_BY,TSD_DL,TSD_DA,TSD_DB,TSD_GLOSS,TSD_VISCOSITY FROM    TINTER_SHEET_MASTER,TINTER_SHEET_DETAIL,ITEM_MASTER,PARTY_MASTER,BATCH_MASTER WHERE TSM_CODE=TSD_TSM_CODE AND TSM_P_CODE=P_CODE AND TSD_I_CODE=I_CODE AND TSM_BT_CODE=BT_CODE AND TSM_CODE='" + code + "' ";
            //}
            //else
            //{
            //    Query = "select BT_NO,SHM_FORMULA_CODE,SHM_FORMULA_NAME,'' as P_NAME,BTD_PROCESS_CODE as PROCESS_CODE,PROCESS_NAME, I_CODENO,I_CODE,BTD_STEP_NO as STEP_NO,BTD_QTY as QTY_IN_LTR,BTD_WGT as WEIGHT_IN_KG,BTD_QTY_IN as QtyinKG from BATCH_MASTER,BATCH_DETAIL,PROCESS_MASTER,ITEM_MASTER ,SHADE_MASTER where BT_CODE=BTD_BT_CODE and BTD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and BTD_I_CODE=I_CODE and BT_SHM_CODE=SHM_CODE and SHADE_MASTER.ES_DELETE=0  and BT_CODE='" + code + "' order by PROCESS_NAME,I_CODENO";
            //}
            //Query = "SELECT TSM_CODE,TSM_FORMULA,CONVERT(VARCHAR,TSM_DATE,106) as TSM_DATE,BT_NO,P_NAME,TSM_LITERS,TSM_KG,TSM_TINTER_NAME,TSM_INITIAL_DL,TSM_INITIAL_DA,TSM_INITIAL_DB,TSM_INITIAL_GLOSS,TSM_INITIAL_VISCOSITY,TSM_CUP,TSM_INITIAL_SG,TSM_FINAL_DL,TSM_FINAL_DA,TSM_FINAL_DB,TSM_FINAL_GLOSS,TSM_FINAL_VISCOSITY,TSM_FINAL_SG,TSM_CHECKED_BY,TSM_APPROVED_BY,I_CODE,I_CODENO,TSD_LOT,TSD_QTY,TSD_ADD_BY,TSD_DL,TSD_DA,TSD_DB,TSD_GLOSS,TSD_VISCOSITY ,TSM_FINAL_DE FROM    TINTER_SHEET_MASTER,TINTER_SHEET_DETAIL,ITEM_MASTER,PARTY_MASTER,BATCH_MASTER WHERE TSM_CODE=TSD_TSM_CODE AND TSM_P_CODE=P_CODE AND TSD_I_CODE=I_CODE AND TSM_BT_CODE=BT_CODE AND TSM_CODE='" + code + "' ";
            Query = "DECLARE @totalPerPage as decimal(8,4)DECLARE @mtotal as INT DECLARE @total as INT DECLARE @intFlag INT = 1 DECLARE @t table (Flag varchar(255),Flag1 varchar(255),Flag2 varchar(255),Flag3 varchar(255),Flag4 varchar(255),Flag5 varchar(255),Flag6 varchar(255),Flag7 varchar(255),Flag8 varchar(255),Flag9 varchar(255),Flag10 varchar(255),Flag11 varchar(255),Flag12 varchar(255),Flag13 varchar(255),Flag14 varchar(255),Flag15 varchar(255),Flag16 varchar(255),Flag17 varchar(255),Flag18 varchar(255),Flag19 varchar(255),Flag20 varchar(255),Flag21 varchar(255),Flag22 varchar(255),Flag23 varchar(255),Flag24 varchar(255),Flag25 varchar(255),Flag26 varchar(255),Flag27 varchar(255),Flag28 varchar(255),Flag29 varchar(255),Flag30 varchar(255),Flag31 varchar(255),Flag32 varchar(255),Flag33 varchar(255)) Set @totalPerPage = 28  Set @mtotal = (SELECT COUNT(TSD_CODE) FROM    TINTER_SHEET_MASTER,TINTER_SHEET_DETAIL,ITEM_MASTER,PARTY_MASTER,BATCH_MASTER WHERE TSM_CODE=TSD_TSM_CODE AND TSM_P_CODE=P_CODE AND TSD_I_CODE=I_CODE AND TSM_BT_CODE=BT_CODE AND TSM_CODE='-2147483648') Set @total = Cast((Ceiling((@mtotal / @totalPerpage)) * @totalPerPage)- @mtotal as int) WHILE (@intFlag <=@total) BEGIN INSERT @t values (152,null,null,null,null,null,null,null,null,null ,null,null,null,null,null,null,null,null,null,null ,null,null,null,null,null,null,null,null,null,null ,null,null,null,null)  SET @intFlag = @intFlag + 1 END SELECT TSM_CODE,TSM_FORMULA,CONVERT(VARCHAR,TSM_DATE,106) as TSM_DATE,BT_NO,P_NAME,TSM_LITERS,TSM_KG,TSM_TINTER_NAME,TSM_INITIAL_DL,TSM_INITIAL_DA,TSM_INITIAL_DB,TSM_INITIAL_GLOSS,TSM_INITIAL_VISCOSITY,TSM_CUP,TSM_INITIAL_SG,TSM_FINAL_DL,TSM_FINAL_DA,TSM_FINAL_DB,TSM_FINAL_GLOSS,TSM_FINAL_VISCOSITY,TSM_FINAL_SG,TSM_CHECKED_BY,TSM_APPROVED_BY,I_CODE,I_CODENO,TSD_LOT,TSD_QTY,TSD_ADD_BY,TSD_DL,TSD_DA,TSD_DB,TSD_GLOSS,TSD_VISCOSITY ,TSM_FINAL_DE FROM    TINTER_SHEET_MASTER,TINTER_SHEET_DETAIL,ITEM_MASTER,PARTY_MASTER,BATCH_MASTER WHERE TSM_CODE=TSD_TSM_CODE AND TSM_P_CODE=P_CODE AND TSD_I_CODE=I_CODE AND TSM_BT_CODE=BT_CODE AND TSM_CODE='"+code+"' Union ALL SELECT * FROM @t";
            DataTable dtfinal = CommonClasses.Execute(Query);

            if (dtfinal.Rows.Count > 0)
            {

                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptTinterSheetPrint_Blank.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptTinterSheetPrint_Blank.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                //rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                //string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,ENQUIRY_MASTER where INQ_CODE='" + code + "' and ISO_SCREEN_NO=94 and ISO_WEF_DATE<=INQ_REQ_DATE order by ISO_WEF_DATE DESC");
                //if (IsoNo == "")
                //{
                //    rptname.SetParameterValue("txtIsoNo", "1");
                //}
                //else
                //{
                //    rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
                //}
                CrystalReportViewer1.ReportSource = rptname;

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
