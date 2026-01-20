using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_GST_ITC_04 : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise1MV");
        home1.Attributes["class"] = "active"; 
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Condition = Request.QueryString[1].ToString();
            string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            string Type = Request.QueryString[4].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }
            GenerateReport(Title, Condition, From, To, Type);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Title, string Condition, string From, string To, string Type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();
        if (Type == "0")
        {

            dt = CommonClasses.Execute(" DECLARE @CM_STATE INT = (SELECT CM_STATE FROM COMPANY_MASTER where CM_CODE=" + Session["CompanyCode"].ToString() + ")  SELECT CL_CH_NO ,convert(varchar ,CL_DATE,105) AS CL_DATE ,CL_P_CODE ,P_NAME ,CL_I_CODE ,I_CODENO ,I_NAME ,CL_DOC_NO ,CL_DOC_DATE ,CL_CQTY ,CL_DOC_TYPE , case when P_LBT_IND=0 then '' else P_GST_NO end AS P_LBT_NO, STATE_MASTER.SM_STATE_CODE, SM_STATE_CODE+'-'+ SM_NAME AS SM_NAME, CASE   WHEN SUBSTRING(E_TARIFF_NO,1,2) IN ('82','84','85','90') then 'CAPITAL GOODS' ELSE 'INPUT' END AS  [Types of Goods] ,E_TARIFF_NO ,CASE WHEN @CM_STATE=P_SM_CODE THEN E_BASIC ELSE 0 END AS E_BASIC,CASE WHEN @CM_STATE=P_SM_CODE THEN E_EDU_CESS ELSE 0 END AS E_EDU_CESS,CASE WHEN @CM_STATE<>P_SM_CODE THEN E_H_EDU ELSE 0 END AS E_H_EDU,E_SPECIAL ,  CASE when I_CAT_CODE=-2147483648 then ROUND(I_INV_RATE*60/100,2) ELSE    ROUND(I_INV_RATE ,2)  END AS IND_RATE,I_INV_RATE,CASE WHEN I_UOM_NAME='NOS' THEN 'NUMBERS' WHEN   I_UOM_NAME='KGS' THEN 'KILOGRAMS'  ELSE  I_UOM_NAME END AS I_UOM_NAME   FROM CHALLAN_STOCK_LEDGER,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,INVOICE_DETAIL,INVOICE_MASTER,STATE_MASTER,ITEM_UNIT_MASTER where  " + Condition + "  P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0  AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='OutSUBINM' and ITEM_MASTER.I_E_CODE=E_CODE and IND_INM_CODE=INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutSUBINM' AND INM_NO=CL_DOC_NO AND IND_I_CODE=CL_I_CODE and P_SM_CODE=STATE_MASTER.SM_CODE   AND CL_DATE  BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  ORDER BY convert(int,CL_CH_NO)  ");

        }
        else if (Type == "1")
        {
            dt = CommonClasses.Execute(" DECLARE @CM_STATE INT = (SELECT CM_STATE FROM COMPANY_MASTER where CM_CODE=" + Session["CompanyCode"].ToString() + ")  SELECT  DISTINCT  convert(int,CL_CH_NO)    AS  CL_CH_NO ,convert(varchar ,CL_DATE,105) AS CL_DATE ,CL_P_CODE ,P_NAME ,CL_I_CODE ,I_CODENO ,I_NAME ,CL_DOC_NO ,CL_DOC_DATE ,CL_CQTY ,CL_DOC_TYPE , case when P_LBT_IND=0 then '' else P_GST_NO end AS P_LBT_NO, SM_STATE_CODE+'-'+ SM_NAME AS SM_STATE_CODE, STATE_MASTER.SM_NAME, CASE   WHEN SUBSTRING(E_TARIFF_NO,1,2) IN ('82','84','85','90') then 'CAPITAL GOODS' ELSE 'INPUT' END AS  [Types of Goods] ,E_TARIFF_NO ,CASE WHEN @CM_STATE=P_SM_CODE THEN E_BASIC ELSE 0 END AS E_BASIC,CASE WHEN @CM_STATE=P_SM_CODE THEN E_EDU_CESS ELSE 0 END AS E_EDU_CESS,CASE WHEN @CM_STATE<>P_SM_CODE THEN E_H_EDU ELSE 0 END AS E_H_EDU,E_SPECIAL ,  CASE when I_CAT_CODE=-2147483648 then ROUND(I_INV_RATE*60/100,2) ELSE    ROUND(I_INV_RATE ,2)  END AS IND_RATE,I_INV_RATE,CASE WHEN I_UOM_NAME='NOS' THEN 'NUMBERS' WHEN   I_UOM_NAME='KGS' THEN 'KILOGRAMS'  ELSE  I_UOM_NAME END AS I_UOM_NAME      FROM CHALLAN_STOCK_LEDGER,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,INWARD_DETAIL,INWARD_MASTER,STATE_MASTER,ITEM_UNIT_MASTER where   " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0  AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='IWIAP' and ITEM_MASTER.I_E_CODE=E_CODE and IWD_IWM_CODE=IWM_CODE AND INWARD_MASTER.ES_DELETE=0  AND IWM_TYPE='OUTCUSTINV'   AND IWM_NO=CL_DOC_NO AND IWD_I_CODE=CL_I_CODE and P_SM_CODE=STATE_MASTER.SM_CODE   AND CL_DATE  BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   ORDER BY convert(int,CL_CH_NO)   ");

        }
        else
        {
            dt = CommonClasses.Execute(" DECLARE @CM_STATE INT = (SELECT CM_STATE FROM COMPANY_MASTER where CM_CODE=" + Session["CompanyCode"].ToString() + ")  SELECT CL_CH_NO ,convert(varchar ,CL_DATE,105) AS CL_DATE ,CL_P_CODE ,P_NAME ,CL_I_CODE ,I_CODENO ,I_NAME ,CL_DOC_NO ,CL_DOC_DATE ,CL_CQTY ,CL_DOC_TYPE , case when P_LBT_IND=0 then '' else P_GST_NO end AS P_LBT_NO, STATE_MASTER.SM_STATE_CODE, SM_STATE_CODE+'-'+ SM_NAME AS SM_NAME, CASE   WHEN SUBSTRING(E_TARIFF_NO,1,2) IN ('82','84','85','90') then 'CAPITAL GOODS' ELSE 'INPUT' END AS  [Types of Goods] ,E_TARIFF_NO ,CASE WHEN @CM_STATE=P_SM_CODE THEN E_BASIC ELSE 0 END AS E_BASIC,CASE WHEN @CM_STATE=P_SM_CODE THEN E_EDU_CESS ELSE 0 END AS E_EDU_CESS,CASE WHEN @CM_STATE<>P_SM_CODE THEN E_H_EDU ELSE 0 END AS E_H_EDU,E_SPECIAL ,  CASE when I_CAT_CODE=-2147483648 then ROUND(I_INV_RATE*60/100,2) ELSE    ROUND(I_INV_RATE ,2)  END AS IND_RATE,I_INV_RATE,CASE WHEN I_UOM_NAME='NOS' THEN 'NUMBERS' WHEN   I_UOM_NAME='KGS' THEN 'KILOGRAMS'  ELSE  I_UOM_NAME END AS I_UOM_NAME,DATEDIFF(dd,CL_DATE,GETDATE()) AS pendingdays,(CL_CQTY-CL_CON_QTY)as PendQTy  into #temp    FROM CHALLAN_STOCK_LEDGER,PARTY_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER,INVOICE_DETAIL,INVOICE_MASTER,STATE_MASTER,ITEM_UNIT_MASTER where  " + Condition + "  P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0  AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='OutSUBINM' and ITEM_MASTER.I_E_CODE=E_CODE and IND_INM_CODE=INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutSUBINM' AND INM_NO=CL_DOC_NO AND IND_I_CODE=CL_I_CODE and P_SM_CODE=STATE_MASTER.SM_CODE    AND (CL_CQTY-CL_CON_QTY)>0   AND  DATEDIFF(dd,CL_DATE,GETDATE())>180     SELECT * FROM #temp order BY pendingdays DESC   drop TABLE #temp ");

        }
        if (dt.Rows.Count > 0)
        {
            //if (true)
            //{
            //    ReportDocument rptname = null;
            //    rptname = new ReportDocument();
            //    if (Type == "0")
            //    {
            //        rptname.Load(Server.MapPath("~/Reports/rptITC04Outward.rpt"));
            //        rptname.FileName = Server.MapPath("~/Reports/rptITC04Outward.rpt");
            //    }
            //    else
            //    {
            //        rptname.Load(Server.MapPath("~/Reports/rptITC04INWARD.rpt"));
            //        rptname.FileName = Server.MapPath("~/Reports/rptITC04INWARD.rpt");
            //    }

            //    rptname.Refresh();
            //    rptname.SetDataSource(dt);
            //    //rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
            //    //rptname.SetParameterValue("txtCompGST", Session["CompanyGST"].ToString());
            //    //rptname.SetParameterValue("txtPeriod", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            //    CrystalReportViewer1.ReportSource = rptname;
            //}
            //else
            //{
            #region Export_Excel
            DataTable dtResult = new DataTable();
            dtResult = dt;
            DataTable dtExport = new DataTable();
            if (dt.Rows.Count > 0)
            {
                if (Type == "0")
                {
                    dtExport.Columns.Add("GSTIN of Job Worker (JW)");
                    dtExport.Columns.Add("State (in case of unregistered JW)");
                    dtExport.Columns.Add("Job Worker's Type");
                    dtExport.Columns.Add("Challan Number");
                    dtExport.Columns.Add("Challan Date");
                    dtExport.Columns.Add("PARTY NAME");
                    dtExport.Columns.Add("Types of Goods");
                    dtExport.Columns.Add("HSN");
                    dtExport.Columns.Add("Description of Goods");
                    dtExport.Columns.Add("Unique Quantity Code (UQC)");
                    dtExport.Columns.Add("Quantity");
                    dtExport.Columns.Add("Taxable Value");
                    dtExport.Columns.Add("Integrated Tax Rate in (%)");
                    dtExport.Columns.Add("Central Tax Rate in (%) ");
                    dtExport.Columns.Add("State/UT Tax Rate in (%)");
                    dtExport.Columns.Add("Cess");

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtExport.Rows.Add(dt.Rows[i]["P_LBT_NO"].ToString(),
                                          dt.Rows[i]["SM_NAME"].ToString(),
                                          "Non SEZ",
                                          dt.Rows[i]["CL_CH_NO"].ToString(),
                                          dt.Rows[i]["CL_DATE"].ToString(),
                                           dt.Rows[i]["P_NAME"].ToString(),
                                           dt.Rows[i]["Types of Goods"].ToString(),
                                           dt.Rows[i]["E_TARIFF_NO"].ToString(),
                                          dt.Rows[i]["I_CODENO"].ToString() + " - " + dt.Rows[i]["I_NAME"].ToString(),
                                          dt.Rows[i]["I_UOM_NAME"].ToString(),
                                          dt.Rows[i]["CL_CQTY"].ToString(),
                                        Math.Round(Convert.ToDouble(dt.Rows[i]["CL_CQTY"].ToString()) * Convert.ToDouble(dt.Rows[i]["IND_RATE"].ToString()), 2),
                                          dt.Rows[i]["E_H_EDU"].ToString(),
                                          dt.Rows[i]["E_BASIC"].ToString(),
                                          dt.Rows[i]["E_EDU_CESS"].ToString(),
                                          dt.Rows[i]["E_SPECIAL"].ToString()
                                         );
                    }
                    //}
                }
                else if (Type == "1")
                {
                    dtExport.Columns.Add("GSTIN of Job Worker (JW)");
                    dtExport.Columns.Add("State (in case of unregistered JW)");
                    dtExport.Columns.Add("PARTY NAME");
                    dtExport.Columns.Add("Original Challan Number");
                    dtExport.Columns.Add("Original Challan Date");
                    dtExport.Columns.Add("Nature of Transaction");
                    dtExport.Columns.Add("Description of Goods");
                    dtExport.Columns.Add("Unique Quantity Code (UQC)");
                    dtExport.Columns.Add("Quantity");
                    dtExport.Columns.Add("Taxable Value");

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtExport.Rows.Add(dt.Rows[i]["P_LBT_NO"].ToString(),
                                          dt.Rows[i]["SM_NAME"].ToString(),
                                            dt.Rows[i]["P_NAME"].ToString(),
                                          dt.Rows[i]["CL_CH_NO"].ToString(),
                                          dt.Rows[i]["CL_DATE"].ToString(),
                                          "Goods Received back from JW",
                                          dt.Rows[i]["I_CODENO"].ToString() + " - " + dt.Rows[i]["I_NAME"].ToString(),
                                          dt.Rows[i]["I_UOM_NAME"].ToString(),
                                          Math.Abs(Convert.ToDouble(dt.Rows[i]["CL_CQTY"].ToString())),
                                        Math.Abs(Math.Round(Convert.ToDouble(dt.Rows[i]["CL_CQTY"].ToString()) * Convert.ToDouble(dt.Rows[i]["IND_RATE"].ToString()), 2))
                                         );
                    }
                }
                else
                {
                    dtExport.Columns.Add("GSTIN of Job Worker (JW)");
                    dtExport.Columns.Add("State (in case of unregistered JW)");
                    dtExport.Columns.Add("PARTY NAME");
                    dtExport.Columns.Add("Challan Number");
                    dtExport.Columns.Add("Challan Date");
                    dtExport.Columns.Add("Types of Goods");
                    dtExport.Columns.Add("Description of Goods");
                    dtExport.Columns.Add("Unique Quantity Code (UQC)");
                    dtExport.Columns.Add("Quantity");
                    dtExport.Columns.Add("Taxable Value");
                    dtExport.Columns.Add("PendingQty");
                    dtExport.Columns.Add("PendingDays");
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        dtExport.Rows.Add(dt.Rows[i]["P_LBT_NO"].ToString(),
                                          dt.Rows[i]["SM_NAME"].ToString(),
                                            dt.Rows[i]["P_NAME"].ToString(),
                                          dt.Rows[i]["CL_CH_NO"].ToString(),
                                          dt.Rows[i]["CL_DATE"].ToString(),
                                         dt.Rows[i]["Types of Goods"].ToString(),
                                          dt.Rows[i]["I_CODENO"].ToString() + " - " + dt.Rows[i]["I_NAME"].ToString(),
                                          dt.Rows[i]["I_UOM_NAME"].ToString(),
                                          Math.Abs(Convert.ToDouble(dt.Rows[i]["CL_CQTY"].ToString())),
                                        Math.Abs(Math.Round(Convert.ToDouble(dt.Rows[i]["CL_CQTY"].ToString()) * Convert.ToDouble(dt.Rows[i]["IND_RATE"].ToString()), 2)),
                                        Math.Abs(Convert.ToDouble(dt.Rows[i]["PendQTy"].ToString())), dt.Rows[i]["pendingdays"].ToString());
                    }
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=ITC04.xls");
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
                            //if (i == 4)
                            //{
                            //    if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                            //    {
                            //        HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            //    }
                            //    else
                            //    {
                            //        HttpContext.Current.Response.Write("<Td>");
                            //    }
                            //}
                            //else
                            //{
                            HttpContext.Current.Response.Write("<Td>");
                            //}
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
            #endregion Export_Excel
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
            CommonClasses.SendError("GST ITC-04 Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewGST_ITC_04.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("GST ITC-04", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
