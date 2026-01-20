using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_AllMasterReport : System.Web.UI.Page
{
    static string right = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='117'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
        }
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckValid())
            {
                popUpPanel5.Visible = true;
                ModalPopupPrintSelection.Show();
            }
            else
            {
                CancelRecord();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private void CancelRecord()
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Country Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
    protected void btnOk_Click(object sender, EventArgs e)
    {
    }

    #region ItemMaster
    public void ItemMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("SELECT ITEM_MASTER.I_CODE, ITEM_CATEGORY_MASTER.I_CAT_NAME, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_DRAW_NO, ITEM_MASTER.I_NAME,  EXCISE_TARIFF_MASTER.E_TARIFF_NO, TALLY_MASTER_1.TALLY_NAME, TALLY_MASTER.TALLY_NAME AS TALLY_NAME_P, ITEM_MASTER.I_CURRENT_BAL, ITEM_MASTER.I_ACTIVE_IND, ITEM_MASTER.I_COSTING_HEAD, ITEM_MASTER.I_TARGET_WEIGHT, ITEM_MASTER.I_WEIGHT, ITEM_MASTER.I_DENSITY,  ITEM_MASTER.I_MAX_LEVEL, ITEM_MASTER.I_MIN_LEVEL, ITEM_MASTER.I_REORDER_LEVEL, ITEM_MASTER.I_INV_RATE, ITEM_MASTER.I_UWEIGHT,  ITEM_UNIT_MASTER.I_UOM_NAME, ITEM_MASTER.I_INV_CAT ,EXCISE_TARIFF_MASTER_1.E_TARIFF_NO AS SAC  FROM ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE INNER JOIN ITEM_CATEGORY_MASTER ON ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE INNER JOIN EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE INNER JOIN TALLY_MASTER AS TALLY_MASTER_1 ON ITEM_MASTER.I_ACCOUNT_SALES = TALLY_MASTER_1.TALLY_CODE INNER JOIN TALLY_MASTER ON ITEM_MASTER.I_ACCOUNT_PURCHASE = TALLY_MASTER.TALLY_CODE INNER JOIN EXCISE_TARIFF_MASTER AS EXCISE_TARIFF_MASTER_1 ON ITEM_MASTER.I_SCAT_CODE = EXCISE_TARIFF_MASTER_1.E_CODE WHERE     (ITEM_MASTER.ES_DELETE = 0) ORDER BY ITEM_CATEGORY_MASTER.I_CAT_NAME, ITEM_MASTER.I_CODENO");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Item Code");
        dtExport.Columns.Add("Item Name");
        dtExport.Columns.Add("Item Category");
        dtExport.Columns.Add("Item Unit");
        dtExport.Columns.Add("Cost head");
        dtExport.Columns.Add("Tally Acct(P)");
        dtExport.Columns.Add("Tally Acct(S)");
        dtExport.Columns.Add("HSN No");
        dtExport.Columns.Add("Max Level");
        dtExport.Columns.Add("Min Level");
        dtExport.Columns.Add("Current Bal");
        dtExport.Columns.Add("Rate");
        dtExport.Columns.Add("Internal cast weight");
        dtExport.Columns.Add("Internal finish weight");
        dtExport.Columns.Add("BD finish weigh");
        dtExport.Columns.Add("SAC No");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["I_CODENO"].ToString(),
                              dtResult.Rows[i]["I_NAME"].ToString(),
                               dtResult.Rows[i]["I_CAT_NAME"].ToString(),
                              dtResult.Rows[i]["I_UOM_NAME"].ToString(),
                              dtResult.Rows[i]["I_COSTING_HEAD"].ToString(),
                               dtResult.Rows[i]["TALLY_NAME_P"].ToString(),
                                dtResult.Rows[i]["TALLY_NAME"].ToString(),
                              dtResult.Rows[i]["E_TARIFF_NO"].ToString(),
                              dtResult.Rows[i]["I_MAX_LEVEL"].ToString(),
                              dtResult.Rows[i]["I_MIN_LEVEL"].ToString(),
                               dtResult.Rows[i]["I_CURRENT_BAL"].ToString(),
                                dtResult.Rows[i]["I_INV_RATE"].ToString(),
                                dtResult.Rows[i]["I_UWEIGHT"].ToString(),
                                dtResult.Rows[i]["I_TARGET_WEIGHT"].ToString(),
                                 dtResult.Rows[i]["I_DENSITY"].ToString(),
                                   dtResult.Rows[i]["SAC"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Item Master.xls");

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

    #region UnitMaster
    public void UnitMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("  SELECT I_UOM_NAME ,I_UOM_DESC FROM ITEM_UNIT_MASTER where ES_DELETE=0  ORDER BY I_UOM_NAME");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Unit Name");
        dtExport.Columns.Add("Item Desc");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["I_UOM_NAME"].ToString(),
                              dtResult.Rows[i]["I_UOM_DESC"].ToString()

                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Item Unit Master.xls");

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
                        if (dtExport.Rows[k]["Unit Name"].ToString().Length > 0)
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

    #region TallyMaster
    public void TallyMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("SELECT TALLY_NAME  FROM TALLY_MASTER where ES_DELETE=0 ORDER BY TALLY_NAME");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Tally Name");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["TALLY_NAME"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Tally Master.xls");

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
                        if (dtExport.Rows[k]["Tally Name"].ToString().Length > 0)
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

    #region SuppierMaster
    public void SuppierMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute(" SELECT SUPPLIER_TYPE_MASTER.STM_TYPE_CODE, PARTY_MASTER.P_NAME, PARTY_MASTER.P_CONTACT, PARTY_MASTER.P_PARTY_CODE,  PARTY_MASTER.P_VEND_CODE, PARTY_MASTER.P_ADD1, PARTY_MASTER.P_PIN_CODE, PARTY_MASTER.P_PHONE, PARTY_MASTER.P_MOB,  PARTY_MASTER.P_EMAIL, PARTY_MASTER.P_PAN, PARTY_MASTER.P_CST, PARTY_MASTER.P_VAT, PARTY_MASTER.P_SER_TAX_NO,  PARTY_MASTER.P_ECC_NO, PARTY_MASTER.P_CATEGORY, PARTY_MASTER.P_EXC_DIV, PARTY_MASTER.P_EXC_RANGE,  PARTY_MASTER.P_EXC_COLLECTORATE, PARTY_MASTER.P_TALLY, PARTY_MASTER.P_LBT_NO, PARTY_MASTER.P_GST_NO FROM PARTY_MASTER INNER JOIN SUPPLIER_TYPE_MASTER ON PARTY_MASTER.P_STM_CODE = SUPPLIER_TYPE_MASTER.STM_CODE WHERE (PARTY_MASTER.ES_DELETE = 0) ORDER BY STM_TYPE_CODE,P_NAME");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Party Name");
        dtExport.Columns.Add("Suppiler Type");
        dtExport.Columns.Add("Contact Person");
        dtExport.Columns.Add("Party Code");
        dtExport.Columns.Add("Vendor Code");
        dtExport.Columns.Add("Address");
        dtExport.Columns.Add("PIN Code");
        dtExport.Columns.Add("Phone No");
        dtExport.Columns.Add("Mobile No");
        dtExport.Columns.Add("Email ID");
        dtExport.Columns.Add("PAN No");
        dtExport.Columns.Add("VAT No");
        dtExport.Columns.Add("CST No");
        dtExport.Columns.Add("GST No");
        dtExport.Columns.Add("Service Tax No");
        dtExport.Columns.Add("ECC No");

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["P_NAME"].ToString(),
                               dtResult.Rows[i]["STM_TYPE_CODE"].ToString(),
                               dtResult.Rows[i]["P_CONTACT"].ToString(),
                              dtResult.Rows[i]["P_PARTY_CODE"].ToString(),
                              dtResult.Rows[i]["P_VEND_CODE"].ToString(),
                               dtResult.Rows[i]["P_ADD1"].ToString(),
                                dtResult.Rows[i]["P_PIN_CODE"].ToString(),
                              dtResult.Rows[i]["P_PHONE"].ToString(),
                               dtResult.Rows[i]["P_MOB"].ToString(),
                                dtResult.Rows[i]["P_EMAIL"].ToString(),
                                dtResult.Rows[i]["P_PAN"].ToString(),
                                dtResult.Rows[i]["P_CST"].ToString(),
                                 dtResult.Rows[i]["P_VAT"].ToString(),
                                  dtResult.Rows[i]["P_GST_NO"].ToString(),
                                dtResult.Rows[i]["P_SER_TAX_NO"].ToString(),
                                dtResult.Rows[i]["P_ECC_NO"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Suppiler Master.xls");

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

                    if (i == 8)
                    {
                        if (dtExport.Rows[k]["Mobile No"].ToString().Length > 0)
                        {
                            HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<Td>");
                        }
                    }
                    else if (i == 9)
                    {
                        if (dtExport.Rows[k]["Phone No"].ToString().Length > 0)
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

    #region customerMaster
    public void customerMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute(" SELECT PARTY_MASTER.P_TYPE, PARTY_MASTER.P_NAME, PARTY_MASTER.P_CONTACT, PARTY_MASTER.P_PARTY_CODE, PARTY_MASTER.P_VEND_CODE, PARTY_MASTER.P_ADD1, PARTY_MASTER.P_PIN_CODE, PARTY_MASTER.P_PHONE, PARTY_MASTER.P_MOB, PARTY_MASTER.P_EMAIL, PARTY_MASTER.P_PAN,   PARTY_MASTER.P_CST, PARTY_MASTER.P_VAT, PARTY_MASTER.P_SER_TAX_NO, PARTY_MASTER.P_ECC_NO, PARTY_MASTER.P_CATEGORY, PARTY_MASTER.P_EXC_DIV, PARTY_MASTER.P_EXC_RANGE, PARTY_MASTER.P_EXC_COLLECTORATE, PARTY_MASTER.P_TALLY, PARTY_MASTER.P_LBT_NO, PARTY_MASTER.P_GST_NO, CUSTOMER_TYPE_MASTER.CTM_TYPE_CODE  FROM PARTY_MASTER INNER JOIN CUSTOMER_TYPE_MASTER ON PARTY_MASTER.P_CUST_TYPE = CUSTOMER_TYPE_MASTER.CTM_CODE  WHERE (PARTY_MASTER.ES_DELETE = 0) ORDER BY PARTY_MASTER.P_NAME ");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Customer Name");
        dtExport.Columns.Add("Customer Type");
        dtExport.Columns.Add("Contact Person");
        dtExport.Columns.Add("Customer Code");
        dtExport.Columns.Add("Vendor Code");
        dtExport.Columns.Add("Address");
        dtExport.Columns.Add("PIN Code");
        dtExport.Columns.Add("Phone No");
        dtExport.Columns.Add("Mobile No");
        dtExport.Columns.Add("Email ID");
        dtExport.Columns.Add("PAN No");
        dtExport.Columns.Add("VAT No");
        dtExport.Columns.Add("CST No");
        dtExport.Columns.Add("Service Tax No");
        dtExport.Columns.Add("ECC No");
        dtExport.Columns.Add("GST No");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["P_NAME"].ToString(),
                                dtResult.Rows[i]["CTM_TYPE_CODE"].ToString(),
                               dtResult.Rows[i]["P_CONTACT"].ToString(),
                              dtResult.Rows[i]["P_PARTY_CODE"].ToString(),
                              dtResult.Rows[i]["P_VEND_CODE"].ToString(),
                               dtResult.Rows[i]["P_ADD1"].ToString(),
                                dtResult.Rows[i]["P_PIN_CODE"].ToString(),
                              dtResult.Rows[i]["P_PHONE"].ToString(),
                               dtResult.Rows[i]["P_MOB"].ToString(),
                                dtResult.Rows[i]["P_EMAIL"].ToString(),
                                dtResult.Rows[i]["P_PAN"].ToString(),
                                dtResult.Rows[i]["P_CST"].ToString(),
                                 dtResult.Rows[i]["P_VAT"].ToString(),
                                dtResult.Rows[i]["P_SER_TAX_NO"].ToString(),
                                dtResult.Rows[i]["P_ECC_NO"].ToString(),
                                dtResult.Rows[i]["P_LBT_NO"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Customer Master.xls");

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

                    if (i == 8)
                    {
                        if (dtExport.Rows[k]["Mobile No"].ToString().Length > 0)
                        {
                            HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<Td>");
                        }
                    }
                    else if (i == 9)
                    {
                        if (dtExport.Rows[k]["Phone No"].ToString().Length > 0)
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

    #region ExciseMaster
    public void ExciseMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("  SELECT E_TARIFF_NO,E_COMMODITY,E_BASIC FROM EXCISE_TARIFF_MASTER where ES_DELETE=0  AND E_TALLY_GST_EXCISE=0 ORDER BY E_TARIFF_NO");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Tariff No");
        dtExport.Columns.Add("commondity Name");
        dtExport.Columns.Add("Excise %");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["E_TARIFF_NO"].ToString(),
                              dtResult.Rows[i]["E_COMMODITY"].ToString(),
                               dtResult.Rows[i]["E_BASIC"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Excise Master.xls");

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
                        if (dtExport.Rows[k]["Tariff No"].ToString().Length > 0)
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

    #region CategoryMaster
    public void CategoryMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("SELECT I_CAT_NAME,I_CAT_SHORTCLOSE FROM ITEM_CATEGORY_MASTER WHERE ES_DELETE=0 ORDER By I_CAT_NAME");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Category Name");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["I_CAT_NAME"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Item Category Master.xls");

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
                        if (dtExport.Rows[k]["Category Name"].ToString().Length > 0)
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

    #region ProjectCodeMaster
    public void ProjectCodeMaster()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute(" SELECT  PROCM_NAME,PROCM_CODE  FROM PROJECT_CODE_MASTER  WHERE ES_DELETE=0 ORDER BY PROCM_CODE");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("Name");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["PROCM_NAME"].ToString()

                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Project Code Master.xls");

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
                        if (dtExport.Rows[k]["Name"].ToString().Length > 0)
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

    #region GSTMASTER
    public void GSTMASTER()
    {
        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("  SELECT E_TARIFF_NO,E_COMMODITY,E_BASIC ,E_SPECIAL,E_EDU_CESS,E_H_EDU,CASE WHEN E_EX_TYPE=0 THEN 'HSN No' ELSE 'SAC No' END AS E_EX_TYPE FROM EXCISE_TARIFF_MASTER where ES_DELETE=0 AND E_TALLY_GST_EXCISE=1 ORDER BY E_TARIFF_NO");

        dtExport.Columns.Add("Sr No");
        dtExport.Columns.Add("HSN/SAC No");
        dtExport.Columns.Add("commondity Name");
        dtExport.Columns.Add("CGST %");
        dtExport.Columns.Add("SGST %");
        dtExport.Columns.Add("IGST %");
        dtExport.Columns.Add("CESS %");
        dtExport.Columns.Add("Type");
        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["E_TARIFF_NO"].ToString(),
                              dtResult.Rows[i]["E_COMMODITY"].ToString(),
                              dtResult.Rows[i]["E_BASIC"].ToString(),
                              dtResult.Rows[i]["E_EDU_CESS"].ToString(),
                              dtResult.Rows[i]["E_H_EDU"].ToString(),
                              dtResult.Rows[i]["E_SPECIAL"].ToString(),
                              dtResult.Rows[i]["E_EX_TYPE"].ToString()
                             );
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=GST HSN/SAC Master.xls");

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
                        if (dtExport.Rows[k]["HSN/SAC No"].ToString().Length > 0)
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

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlMasterForms.SelectedIndex == 0)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Currency Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

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
            CommonClasses.SendError("Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void checkRights(int sm_code)
    {
        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE=" + sm_code);
        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
            {
                if (ddlMasterForms.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Please Select Master Form", CommonClasses.MSG_Warning);
                }
                if (ddlMasterForms.SelectedValue == "1")
                {
                    ItemMaster();
                }
                if (ddlMasterForms.SelectedValue == "2")
                {
                    UnitMaster();
                }
                if (ddlMasterForms.SelectedValue == "3")
                {
                    customerMaster();
                }
                if (ddlMasterForms.SelectedValue == "4")
                {
                    SuppierMaster();
                }
                if (ddlMasterForms.SelectedValue == "5")
                {
                    CategoryMaster();
                }
                if (ddlMasterForms.SelectedValue == "6")
                {
                    TallyMaster();
                }
                if (ddlMasterForms.SelectedValue == "7")
                {
                    ExciseMaster();
                }
                if (ddlMasterForms.SelectedValue == "8")
                {
                    ProjectCodeMaster();
                }
                if (ddlMasterForms.SelectedValue == "9")
                {
                    GSTMASTER();
                }
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../VIEW/ViewMasterReport.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    private void ShowRecord()
    {
        string master_index = "";

        if (ddlMasterForms.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Master Form", CommonClasses.MSG_Warning);
        }
        else
        {
            if (ddlMasterForms.SelectedIndex == 1)
            {
                checkRights(28);
                if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                {
                    Response.Redirect("~/RoportForms/VIEW/ViewSubComponentReg.aspx", false);
                }
                else
                {
                    Response.Write("<script> alert('You Have No Rights To View.');</script>");
                }
            }
            if (ddlMasterForms.SelectedIndex == 2)
            {
                checkRights(30);
                if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                {
                    master_index = "3";
                }
                else
                {
                    Response.Write("<script> alert('You Have No Rights To View.');</script>");
                }
            }
            if (ddlMasterForms.SelectedIndex == 3)
            {
                checkRights(29);
                if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                {

                    Response.Redirect("~/RoportForms/VIEW/ViewCustomerRegister.aspx", false);
                }
                else
                {
                    Response.Write("<script> alert('You Have No Rights To View.');</script>");
                }
            }
            if (ddlMasterForms.SelectedIndex == 4)
            {
                checkRights(31);
                if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                {
                    master_index = "5";
                }
                else
                {
                    Response.Write("<script> alert('You Have No Rights To View.');</script>");
                }
            }
            if (ddlMasterForms.SelectedIndex == 6)
            {
                master_index = "6";
            }
        }
        if (master_index != "")
        {
            Response.Redirect("~/RoportForms/ADD/MasterRegister.aspx?Title=" + Title + "&master_index=" + master_index + "", false);
        }
    }
}
