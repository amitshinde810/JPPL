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

public partial class Transactions_ADD_Export : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Export_Excel
        DataTable dtResult = new DataTable();
        dtResult = (DataTable)Session["dtTostore"];
        DataTable dtExport = new DataTable();
        if (dtResult.Rows.Count > 0)
        {
            dtExport.Columns.Add("Sr.No");
            dtExport.Columns.Add("ITEM CODE");
            dtExport.Columns.Add("ITEM_NAME");
            dtExport.Columns.Add("Store Name");
            dtExport.Columns.Add("Store Stock Qty");
            dtExport.Columns.Add("Physical Qty");
            dtExport.Columns.Add("Diff Qty");
            dtExport.Columns.Add("Weight");
            dtExport.Columns.Add("Tonnage");
            dtExport.Columns.Add("Rate");
            dtExport.Columns.Add("Amount");

            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                dtExport.Rows.Add(i + 1, dtResult.Rows[i]["I_CODENO"].ToString(),
                                  dtResult.Rows[i]["I_NAME"].ToString(),
                                  dtResult.Rows[i]["STORE_NAME"].ToString(),
                                  dtResult.Rows[i]["STL_DOC_QTY"].ToString(),
                                  dtResult.Rows[i]["UPLOAD_QTY"].ToString(),
                                  dtResult.Rows[i]["DIFF_QTY"].ToString(),

                                  dtResult.Rows[i]["I_UWEIGHT"].ToString(),
                                  dtResult.Rows[i]["TOUNNGE"].ToString(),
                                  dtResult.Rows[i]["I_INV_RATE"].ToString(),
                                  dtResult.Rows[i]["AMT"].ToString()
                                 );
            }
        }

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Stock.xls");

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
                    if (i == 1)
                    {
                        if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                        {
                            HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                        }
                        else if (dtExport.Rows[k]["ITEM_NAME"].ToString().Length > 0)
                        {
                            //HttpContext.Current.Response.Write("<Td>");
                            HttpContext.Current.Response.Write(@"<Td style='white-space:nowrap:\@'>");
                            //HttpContext.Current.Response.Write("<Td>");
                            //HttpContext.Current.Response.Write(dtExport.Rows[i].ToString());
                            //HttpContext.Current.Response.Write("</Td>");
                        }

                    }
                    else if (i == 2)
                    {
                        HttpContext.Current.Response.Write(@"<Td style='white-space:nowrap'>");
                        //HttpContext.Current.Response.Write("'" + dtExport.Rows[k]["ITEM_NAME"].ToString());
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
        #endregion Export_Excel
    }
}
