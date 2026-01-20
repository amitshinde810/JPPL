using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_DispatchToSubcontractor : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }
    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Condition = Request.QueryString[1].ToString();
            string Type = Request.QueryString[2].ToString();
            string From = Request.QueryString[3].ToString();
            string To = Request.QueryString[4].ToString();
            string Group = Request.QueryString[5].ToString();
            string RType = Request.QueryString[6].ToString();
            GenerateReport(Title, Condition, Type, From, To, Group, RType);
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Condition, string Type, string From, string To, string Group, string RType)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        Query = " SELECT INVOICE_MASTER.INM_CODE, INVOICE_MASTER.INM_NO, INVOICE_MASTER.INM_DATE,CASE WHEN  INM_INVOICE_TYPE='0' then 'As Per BOM' WHEN  INM_INVOICE_TYPE='1' then 'One To One' WHEN  INM_INVOICE_TYPE='2' then 'Online Rejection' ELSE '' END  AS INM_INVOICE_TYPE, INVOICE_MASTER.INM_TYPE, INVOICE_MASTER.INM_P_CODE, ITEM_MASTER.I_CODE, ITEM_MASTER.I_NAME, ITEM_MASTER.I_CODENO, ITEM_CATEGORY_MASTER.I_CAT_CODE, ITEM_CATEGORY_MASTER.I_CAT_NAME, ITEM_UNIT_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME, SUPP_PO_MASTER.SPOM_CODE, SUPP_PO_MASTER.SPOM_PO_NO, SUPP_PO_MASTER.SPOM_DATE, SUPP_PO_MASTER.SPOM_PONO, SUPP_PO_MASTER.SPOM_PROJECT, SUPP_PO_MASTER.SPOM_PROJ_NAME, PARTY_MASTER.P_NAME, PARTY_MASTER.P_VEND_CODE, PARTY_MASTER.P_PARTY_CODE, PARTY_MASTER.P_ADD1,  IND_INQTY AS IND_CON_QTY, INVOICE_DETAIL.IND_RATE , ISNULL(EXCISE_TARIFF_MASTER.E_TARIFF_NO,'') AS IND_E_TARIFF_NO ,ISNULL(E.E_TARIFF_NO,'') AS E_SAC_NO FROM SUPP_PO_MASTER INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE INNER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE INNER JOIN INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE ON ITEM_MASTER.I_CODE = INVOICE_DETAIL.IND_I_CODE ON SUPP_PO_MASTER.SPOM_CODE = INVOICE_DETAIL.IND_CPOM_CODE INNER JOIN EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE INNER JOIN EXCISE_TARIFF_MASTER AS E ON ITEM_MASTER.I_SCAT_CODE = E.E_CODE WHERE (INVOICE_MASTER.INM_TYPE = 'OutSUBINM') AND (INVOICE_MASTER.ES_DELETE = 0) AND " + Condition + "  AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND E.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND ITEM_CATEGORY_MASTER.ES_DELETE=0 AND SUPP_PO_MASTER.ES_DELETE=0 AND ITEM_UNIT_MASTER.ES_DELETE=0 ORDER BY INM_NO,P_NAME,I_CODENO";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(Query);

        if (dt.Rows.Count > 0)
        {
            #region EXPORT
            if (Type == "EXPORT")
            {
                DataTable dtResult = new DataTable();
                dtResult = dt;
                DataTable dtExport = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    dtExport.Columns.Add("Sr No");
                    dtExport.Columns.Add("Challan No");
                    dtExport.Columns.Add("Challan Date");
                    dtExport.Columns.Add("Challan Type");
                    dtExport.Columns.Add("Party Name");
                    dtExport.Columns.Add("Item Code");
                    dtExport.Columns.Add("Item Name");
                    dtExport.Columns.Add("HSN No.");
                    dtExport.Columns.Add("SAC No.");
                    dtExport.Columns.Add("Challan Qty");
                    dtExport.Columns.Add("Unit");

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtExport.Rows.Add(i + 1,
                                          dtResult.Rows[i]["INM_NO"].ToString(),
                                          Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                          dtResult.Rows[i]["INM_INVOICE_TYPE"].ToString(),
                                          dtResult.Rows[i]["P_NAME"].ToString(),
                                          dtResult.Rows[i]["I_CODENO"].ToString(),
                                          dtResult.Rows[i]["I_NAME"].ToString(),
                                          dtResult.Rows[i]["IND_E_TARIFF_NO"].ToString(),
                                          dtResult.Rows[i]["E_SAC_NO"].ToString(),
                                          dtResult.Rows[i]["IND_CON_QTY"].ToString(),
                                          dtResult.Rows[i]["I_UOM_NAME"].ToString()
                                         );
                    }
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=DispatchToSubContractor.xls");

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
                            if (i == 5)
                            {
                                if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                                {
                                    HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                                }
                                else
                                {
                                    HttpContext.Current.Response.Write("<Td>");
                                }
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<Td>");
                            }
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
            #endregion

            else
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                #region DateWise
                if (Group == "DateWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDispatchRegister.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDispatchRegister.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtTitle", "Dispatch To Subcontractor Date wise Report ");
                    rptname.SetParameterValue("txtType", RType.Trim());
                    CrystalReportViewer1.ReportSource = rptname;
                }
                #endregion DateWise

                #region ItemWise
                if (Group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDispatchRegisterItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDispatchRegisterItemWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtTitle", "Dispatch To Subcontractor Item wise Report ");
                    rptname.SetParameterValue("txtType", RType.Trim());
                    CrystalReportViewer1.ReportSource = rptname;
                }
                #endregion ItemWise

                #region PartyWise
                if (Group == "PartyWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDispatchRegisterCustWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDispatchRegisterCustWise.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtPeriod", "From " + From + " to " + To);
                    rptname.SetParameterValue("txtTitle", "Dispatch To Subcontractor SubContractor wise Report ");
                    rptname.SetParameterValue("txtType", RType.Trim());
                    CrystalReportViewer1.ReportSource = rptname;
                }
                #endregion PartyWise
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            return;
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
            CommonClasses.SendError("Sales Order Rport", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewDispatchtoSubcontractor.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
