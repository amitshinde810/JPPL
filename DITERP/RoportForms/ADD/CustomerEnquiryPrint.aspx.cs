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

public partial class RoportForms_ADD_CustomerEnquiryPrint : System.Web.UI.Page
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
            Response.Redirect("~/Transactions/VIEW/ViewCustomerEnquiry.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry Print", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion


    #region GenerateReport
    private void GenerateReport(string code)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        
        try
        {
            DataTable dtfinal = CommonClasses.Execute("select INQ_CODE,INQ_CM_COMP_ID,INQ_NO,INQ_TYPE,INQ_GLOSS,INQ_SHADE_NAME,P_NAME as INQ_CUST_NAME,INQ_SHADE_CODE,INQ_SHADE_NO,INQ_REQ_DATE,INQ_REQ_BY,INQ_DES_DUE_DATE,INQ_END_USE,INQ_SUBRTATE,INQ_PRIMER_DFT,INQ_VOL_SOLID,INQ_DFT,INQ_PMT,INQ_CUST_SPEC,INQ_NOTE,INQ_COMP_DATE from ENQUIRY_MASTER,PARTY_MASTER where INQ_CUST_NAME=P_CODE and INQ_CODE='" + code + "' and ENQUIRY_MASTER.ES_DELETE=0");

            if (dtfinal.Rows.Count > 0)
            {               
               
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptCustomerEnquiryPrint.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptCustomerEnquiryPrint.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dtfinal);
                rptname.SetParameterValue("txtCompIso", Session["CompanyIso"].ToString());
                string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,ENQUIRY_MASTER where INQ_CODE='" + code + "' and ISO_SCREEN_NO=95 and ISO_WEF_DATE<=INQ_REQ_DATE order by ISO_WEF_DATE DESC");
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry Print", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
