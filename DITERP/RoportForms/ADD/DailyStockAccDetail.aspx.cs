using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_DailyStockAccDetail : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise1MV");
        home1.Attributes["class"] = "active";
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewDailyStockAccDetail.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery challan Register", "btnCancel_Click", Ex.Message);
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
            string Cond = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();

            GenerateReport(Title, From, To, group, way, Cond, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            Query = "SELECT INM_TYPE, EXCISE_TARIFF_MASTER.E_TARIFF_NO, EXCISE_TARIFF_MASTER.E_COMMODITY, EXCISE_TARIFF_MASTER.E_BASIC,INVOICE_MASTER.INM_DATE,INVOICE_MASTER.INM_NO, INVOICE_MASTER.INM_TNO, INVOICE_DETAIL.IND_INQTY, INVOICE_DETAIL.IND_RATE,INM_BE_AMT AS IND_EX_AMT,INVOICE_DETAIL.IND_E_CESS_AMT, INVOICE_DETAIL.IND_SH_CESS_AMT,ISNULL(IND_AMT,0) AS  IND_AMT,ISNULL(IND_AMORTAMT,0) IND_AMORTAMT,ISNULL(IND_AMORTRATE,0) AS IND_AMORTRATE, INVOICE_MASTER.INM_SUPPLEMENTORY,INVOICE_MASTER.INM_NET_AMT, INVOICE_MASTER.INM_S_TAX,INVOICE_MASTER.INM_S_TAX_AMT, INVOICE_MASTER.INM_BEXCISE, INVOICE_MASTER.INM_BE_AMT, INVOICE_MASTER.INM_EDUC_CESS,INVOICE_MASTER.INM_EDUC_AMT,INVOICE_MASTER.INM_H_EDUC_CESS, INVOICE_MASTER.INM_H_EDUC_AMT, INVOICE_MASTER.INM_DISC,INVOICE_MASTER.INM_DISC_AMT,I_CODENO,I_NAME,I_CODE FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN EXCISE_TARIFF_MASTER ON INVOICE_DETAIL.IND_E_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE   " + Condition + "   (INVOICE_MASTER.ES_DELETE = 0) AND INM_TYPE='TAXINV' ORDER BY E_COMMODITY,INM_DATE";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (Type == "SHOW")
                {
                    DataTable DTcompInfo = CommonClasses.Execute("select CM_CODE,CM_ID,CM_NAME,CM_ADDRESS1,CM_ECC_NO from COMPANY_MASTER where CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and CM_ACTIVE_IND=1");
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    if (group == "TariffWise")
                    {
                        rptname.Load(Server.MapPath("~/Reports/RptTeriffExciseRptnew.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/RptTeriffExciseRptnew.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);

                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                        rptname.SetParameterValue("txtCompAddress", DTcompInfo.Rows[0]["CM_ADDRESS1"]);
                        rptname.SetParameterValue("txtECCNO", DTcompInfo.Rows[0]["CM_ECC_NO"]);
                        rptname.SetParameterValue("type", way);
                        CrystalReportViewer1.ReportSource = rptname;
                    }
                    if (group == "ItemWise")
                    {
                        rptname.Load(Server.MapPath("~/Reports/RptTeriffExciseItemWise.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/RptTeriffExciseItemWise.rpt");
                        rptname.Refresh();
                        rptname.SetDataSource(dt);

                        rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                        rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                        rptname.SetParameterValue("txtCompAddress", DTcompInfo.Rows[0]["CM_ADDRESS1"]);
                        rptname.SetParameterValue("txtECCNO", DTcompInfo.Rows[0]["CM_ECC_NO"]);
                        rptname.SetParameterValue("type", way);

                        CrystalReportViewer1.ReportSource = rptname;
                    }
                }
                else
                {
                    DataTable dtTrafiic = dt.DefaultView.ToTable("E_TARIFF_NO", true, "E_TARIFF_NO");
                    DataTable dtResult = new DataTable();
                    dtResult = dt;
                    DataTable dtExport = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtExport.Columns.Add("DATE");
                        dtExport.Columns.Add("OPENING BALANCE");
                        dtExport.Columns.Add("QTY MFGD");
                        dtExport.Columns.Add("QTY CLRD");
                        dtExport.Columns.Add("CLOSING BALANCE");
                        dtExport.Columns.Add("ASS. VALUE");
                        dtExport.Columns.Add("RATE OF DUTY");
                        dtExport.Columns.Add("EXCISE DUTY");
                        dtExport.Columns.Add("EDU. CESS");
                        dtExport.Columns.Add("S&H. CESS");
                        dtExport.Columns.Add("INV NO.");

                        for (int t = 0; t < dtTrafiic.Rows.Count; t++)
                        {
                            double qty = 0, ass_value = 0, excise = 0, edu = 0, sh = 0;
                            DataTable dtexcise = new DataTable();
                            dtexcise = CommonClasses.Execute("SELECT * FROM EXCISE_TARIFF_MASTER  where E_TARIFF_NO='" + dtTrafiic.Rows[t]["E_TARIFF_NO"].ToString() + "'");
                            dtExport.Rows.Add("Tarrif No:", dtexcise.Rows[0]["E_TARIFF_NO"].ToString(),
                                                      dtexcise.Rows[0]["E_COMMODITY"].ToString(), "", "", "", "", "", "", "", ""
                                                     );
                            try
                            {
                                for (int i = 0; i < dtResult.Rows.Count; i++)
                                {
                                    if (dtTrafiic.Rows[t]["E_TARIFF_NO"].ToString() == dtResult.Rows[i]["E_TARIFF_NO"].ToString())
                                    {
                                        dtExport.Rows.Add(
                                                         Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd.MM.yyyy"),
                                                         0.00,
                                                          dtResult.Rows[i]["IND_INQTY"].ToString(),
                                                          dtResult.Rows[i]["IND_INQTY"].ToString(),
                                                          0.00,
                                                          dtResult.Rows[i]["IND_AMT"].ToString(),
                                                          dtResult.Rows[i]["E_BASIC"].ToString(),
                                                          dtResult.Rows[i]["IND_EX_AMT"].ToString(),
                                                           dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
                                                            dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
                                                             dtResult.Rows[i]["INM_NO"].ToString()
                                                         );
                                        qty = qty + Convert.ToDouble(dtResult.Rows[i]["IND_INQTY"].ToString());
                                        ass_value = ass_value + Convert.ToDouble(dtResult.Rows[i]["IND_AMT"].ToString());
                                        excise = excise + Convert.ToDouble(dtResult.Rows[i]["IND_EX_AMT"].ToString());
                                        edu = edu + Convert.ToDouble(dtResult.Rows[i]["IND_E_CESS_AMT"].ToString());
                                        sh = sh + Convert.ToDouble(dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }

                            dtExport.Rows.Add("TOTAL", 0.00, qty, qty, 0.00, ass_value, "", excise, edu, sh, "");
                            dtExport.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
                        }
                    }
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.ContentType = "application/ms-excel";
                    HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=DSA.xls");
                    HttpContext.Current.Response.Charset = "utf-8";
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                    //sets font
                    HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                    HttpContext.Current.Response.Write("<BR><BR><BR>");
                    HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                    "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                    "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                    //am getting my grid's column headers
                    int columnscount = dtExport.Columns.Count;
                    for (int j = 0; j < columnscount; j++)
                    {      //write in new column
                        HttpContext.Current.Response.Write("<Td>");
                        //Get column headers  and make it as bold in excel columns
                        HttpContext.Current.Response.Write("<B>");
                        HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                        HttpContext.Current.Response.Write("</B>");
                        HttpContext.Current.Response.Write("</Td>");
                    }

                    HttpContext.Current.Response.Write("</TR>");
                    for (int k = 0; k < dtExport.Rows.Count; k++)
                    {//write in new row
                        HttpContext.Current.Response.Write("<TR>");
                        for (int i = 0; i < dtExport.Columns.Count; i++)
                        {
                            if (i == dtExport.Columns.Count - 1)
                            {
                                HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                                HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                HttpContext.Current.Response.Write("</Td>");
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<Td>");
                                HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                HttpContext.Current.Response.Write("</Td>");
                            }
                        }
                        HttpContext.Current.Response.Write("</TR>");
                    }
                    HttpContext.Current.Response.Write("</Table>");
                    HttpContext.Current.Response.Write("</font>");
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}