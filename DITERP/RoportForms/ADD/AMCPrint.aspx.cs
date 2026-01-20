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
public partial class RoportForms_ADD_AMCPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Code = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Page_Init(object sender, EventArgs e)
    {
        Code = Request.QueryString[0];
        GenerateReport(Code);
    }

    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();

        DataTable dtfinal = CommonClasses.Execute("select WO_AMC_CODE,WO_AMC_NO,CONVERT(varchar,WO_PO_DATE,103) as WO_PO_DATE,WO_P_CODE,P_NAME,P_ADD1,WO_TYPE,WO_REF,WO_CONTACT_PER,WO_PHONE_NO,WO_DELIVERY_SCHD,WO_PAY_TERM,WO_DELIVER_TO,WO_FRIEGHT_TERM,WO_GARUNTEE_TERM,WO_TRANSPORTOR,WO_NOTE,cast(isnull(WO_SER_TAX_PER,0) as numeric(20,2)) as WO_SER_TAX_PER,WO_TAX_APPLICABLE,cast(WO_GRAND_TOT as numeric(20,2)) as WO_GRAND_TOT,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,cast(WOD_QTY as numeric(20,3)) as WOD_QTY,cast(WOD_RATE as numeric(20,2)) as WOD_RATE,cast(WOD_TOT_AMT as numeric(20,2)) as WOD_TOT_AMT,CM_EXCISE_RANGE,CM_EXCISE_DIVISION,CM_COMMISONERATE,CM_PAN_NO,CM_CST_NO,CM_VAT_TIN_NO,CM_VAT_WEF,CM_CST_WEF,CM_BANK_NAME,CM_BANK_ACC_NO,CM_OWNER,UM_NAME,I_UOM_NAME from WORK_AMC_ORDER_MASTER,WORK_AMC_ORDER_DETAIL,PARTY_MASTER,USER_MASTER,COMPANY_MASTER,ITEM_UNIT_MASTER where WOD_WO_AMC_CODE=WO_AMC_CODE and I_UOM_CODE=WOD_UOM_CODE and WO_P_CODE=P_CODE and WO_CM_CODE=CM_CODE and WO_UM_CODE=UM_CODE and WORK_AMC_ORDER_MASTER.ES_DELETE=0 and WO_AMC_CODE='" + Code + "' and WO_TYPE='AMC' ");
        if (dtfinal.Rows.Count > 0)
        {
        }
        try
        {


            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptAMCPrint.rpt"));
            rptname.FileName = Server.MapPath("~/Reports/rptAMCPrint.rpt");
            rptname.Refresh();
            rptname.SetDataSource(dtfinal);
            string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,WORK_AMC_ORDER_MASTER where WO_AMC_CODE='" + code + "' and ISO_SCREEN_NO=85 and ISO_WEF_DATE<=WO_PO_DATE order by ISO_WEF_DATE DESC");
            rptname.SetParameterValue("txtIsoNo", IsoNo);
            if (IsoNo == "")
            {
                rptname.SetParameterValue("txtIsoNo", "1");
            }
            else
            {
                rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
            }
            CrystalReportViewer1.ReportSource = rptname;


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);
    }
    #endregion
}
