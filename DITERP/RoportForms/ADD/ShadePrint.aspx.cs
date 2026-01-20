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

public partial class RoportForms_ADD_ShadePrint : System.Web.UI.Page
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
            Response.Redirect("~/Transactions/VIEW/ViewShadeCreation.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion


    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();

        try
        {
            DataTable dtfinal = CommonClasses.Execute("select SHM_CODE,SHM_FORMULA_CODE,SHM_FORMULA_NAME,SHM_REMARK,SHM_PROCESS_CODE ,PROCESS_NAME,SHM_PROCESS_STEPS,I_NAME,I_CODENO,SHM_QTY_KG,SHM_QTY_LTR ,SHM_RATE,isnull(SHM_VOL_SOLID,0) as SHM_VOL_SOLID,avg(isnull(I_DENSITY,0)) as I_DENSITY,avg(isnull(I_PIGMENT,0)) as I_PIGMENT,avg(isnull(I_SOLIDS,0)) as I_SOLIDS,avg(isnull(I_VOLATILE,0)) as I_VOLATILE From SHADE_DETAIL,SHADE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,PROCESS_MASTER where SHM_ITEM_CODE=I_CODE AND SHM_CODE=SHD_SHM_CODE AND ITEM_MASTER.I_UOM_CODE =ITEM_UNIT_MASTER.I_UOM_CODE AND SHM_PROCESS_CODE=PROCESS_CODE AND SHADE_MASTER .ES_DELETE=0 and SHM_CODE='" + code + "' group by SHM_CODE,SHM_FORMULA_CODE,SHM_FORMULA_NAME,SHM_REMARK,SHM_PROCESS_CODE ,PROCESS_NAME,SHM_PROCESS_STEPS,I_NAME,I_CODENO,SHM_QTY_KG,SHM_QTY_LTR ,SHM_RATE,SHM_VOL_SOLID order by SHM_PROCESS_STEPS,I_NAME");

            if (dtfinal.Rows.Count > 0)
            {

                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptShadePrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptShadePrint.rpt");
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
            CommonClasses.SendError("Shade Creation Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
