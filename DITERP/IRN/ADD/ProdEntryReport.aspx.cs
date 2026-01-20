using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_ProdEntryReport : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
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
        }
        catch (Exception Ex)
        {

        }
    }
    private void GenerateReport(string Title, string From, string To, string group, string way, string cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = ""; //select IRN_NO,CONVERT(VARCHAR,IRN_DATE,106) AS IRN_DATE,I_CODENO,I_NAME,IRND_PROD_QTY,IRND_RATE,IRND_AMT,IRND_I_CODE,STORE_NAME,I_UOM_NAME FROM IRN_ENTRY IE,IRN_DETAIL ID,STORE_MASTER SM,ITEM_MASTER IM,ITEM_UNIT_MASTER IUM
            // Query = "SELECT IE.IRN_NO, CONVERT(VARCHAR, IE.IRN_DATE, 106) AS IRN_DATE, IM.I_CODENO, IM.I_NAME, ID.IRND_PROD_QTY, ID.IRND_RATE, ID.IRND_AMT, ID.IRND_I_CODE, SM.STORE_NAME, IUM.I_UOM_NAME, isnull(ID.IRN_ALLOY_WGHT,0) as IRN_ALLOY_WGHT, isnull(ID.IRN_ALLOY_TOTAL,0) as IRN_ALLOY_TOTAL,isnull(AC.Alloy_CAT_NAME,0) as Alloy_CAT_NAME FROM IRN_ENTRY AS IE INNER JOIN IRN_DETAIL AS ID ON IE.IRN_CODE = ID.IRND_IRN_CODE INNER JOIN STORE_MASTER AS SM ON ID.IRN_STORE_CODE = SM.STORE_CODE AND ID.IRN_STORE_CODE = SM.STORE_CODE INNER JOIN ITEM_MASTER AS IM ON ID.IRND_I_CODE = IM.I_CODE INNER JOIN ITEM_UNIT_MASTER AS IUM ON ID.IRND_UOM = IUM.I_UOM_CODE LEFT OUTER JOIN Alloy_category AS AC ON ID.IRN_ALLOY_NAME = AC.Alloy_I_CAT_CODE WHERE " + cond + " IE.IRN_CODE=ID.IRND_IRN_CODE AND IE.ES_DELETE=0 AND SM.STORE_CODE=ID.IRN_STORE_CODE and ID.IRND_I_CODE=IM.I_CODE AND IUM.I_UOM_CODE=ID.IRND_UOM AND ID.IRN_STORE_CODE=SM.STORE_CODE and IE.IRN_CM_CODE='" + Session["CompanyCode"] + "' and IE.IRN_CM_ID='" + Session["CompanyId"] + "'";
            Query = "SELECT IE.IRN_NO, CONVERT(VARCHAR,IE.IRN_DATE,106) AS IRN_DATE,IM.I_CODENO,IM.I_NAME,ID.IRND_PROD_QTY,ID.IRND_RATE,ID.IRND_AMT, ID.IRND_I_CODE,IUM.I_UOM_NAME FROM IRN_ENTRY AS IE INNER JOIN IRN_DETAIL AS ID ON IE.IRN_CODE = ID.IRND_IRN_CODE AND IE.IRN_CODE = ID.IRND_IRN_CODE INNER JOIN ITEM_MASTER AS IM ON ID.IRND_I_CODE = IM.I_CODE AND ID.IRND_I_CODE = IM.I_CODE INNER JOIN ITEM_UNIT_MASTER AS IUM ON ID.IRND_UOM = IUM.I_UOM_CODE AND ID.IRND_UOM = IUM.I_UOM_CODE WHERE " + cond + " (IE.ES_DELETE = 0) AND  IRN_TRANS_TYPE=1 AND (IE.IRN_CM_ID = '" + Session["CompanyId"] + "')";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);

            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/ProdEntryDatewiseReport.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/ProdEntryDatewiseReport.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtTitle", "Production Entry Datewise " + From + " to " + To);
                    rptname.SetParameterValue("type", way);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/ProdEntryItemwiseReport.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/ProdEntryItemwiseReport.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtTitle", "Production Entry Itemwise " + From + " to " + To);
                    rptname.SetParameterValue("type", way);
                    CrystalReportViewer1.ReportSource = rptname;
                }
                if (group == "StoreWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/ProdEntryStoreWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/ProdEntryStoreWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompanyName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtTitle", "Production Entry Storewise " + From + " to " + To);
                    rptname.SetParameterValue("type", way);
                    CrystalReportViewer1.ReportSource = rptname;
                }

                if (way == "Summary")
                {

                }
            }


        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/ViewProdEntryReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production Entry Register", "btnCancel_Click", Ex.Message);
        }
    }
}
