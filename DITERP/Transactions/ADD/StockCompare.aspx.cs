using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Transactions_ADD_StockCompare : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {

                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='148'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    btnSubmit.Visible = false;
                    dgMaterialAcceptance.Enabled = false;
                    if (dgMaterialAcceptance.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("STL_STORE_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("STORE_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("STL_DOC_QTY", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("UPLOAD_QTY", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("DIFF_QTY", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_INV_RATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UWEIGHT", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TONUGE", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgMaterialAcceptance.DataSource = dtFilter;
                            dgMaterialAcceptance.DataBind();
                        }
                        else
                        {
                            dgMaterialAcceptance.Enabled = true;
                        }
                    }
                    FillCombo();
                    string script = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });";
                    ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Acceptance", "btnShow_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Acceptance", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region UserDefiendMethod

    #region FillCombo
    private void FillCombo()
    {
        try
        {
            DataTable dtUser = CommonClasses.Execute("SELECT * FROM USER_STORE_DETAIL WHERE UM_CODE ='" + Session["UserCode"] + "' ");
            if (dtUser.Rows.Count == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "User have no rights of store...";
                return;
            }
            string Codes = "";
            for (int i = 0; i < dtUser.Rows.Count; i++)
            {
                Codes = Codes + "'" + dtUser.Rows[i]["STORE_CODE"].ToString() + "'" + ",";
            }
            Codes = Codes.TrimEnd(',');
            CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (" + Codes + ") ORDER BY STORE_NAME", ddlToStore);
            ddlToStore.Items.Insert(0, new ListItem("Select To Store Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "FillCombo", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from INWARD_MASTER where IWM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Acceptance -View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion

    #endregion

    #region ddlToStore_SelectedIndexChanged
    public void ddlToStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTostore = CommonClasses.Execute(" SELECT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,I_INV_RATE,I_UWEIGHT ,  STOCK_LEDGER.STL_STORE_TYPE, STORE_MASTER.STORE_NAME, SUM(STOCK_LEDGER.STL_DOC_QTY) AS STL_DOC_QTY ,   ISNULL((SELECT ISNULL(SUM(STORE_TEMP_TRANSACTION.TRAN_QTY),0) FROM  STORE_TEMP_TRANSACTION where TRAN_ITEM_CODE= I_CODE AND TRAN_STORE_CODE= '" + ddlToStore.SelectedValue + "' ),0 ) AS UPLOAD_QTY INTO #temp  FROM STOCK_LEDGER INNER JOIN ITEM_MASTER ON STOCK_LEDGER.STL_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN STORE_MASTER ON STOCK_LEDGER.STL_STORE_TYPE = STORE_MASTER.STORE_CODE  WHERE     (ITEM_MASTER.ES_DELETE = 0) AND I_CAT_CODE=-2147483648 AND (STOCK_LEDGER.STL_STORE_TYPE = '" + ddlToStore.SelectedValue + "')  GROUP BY ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO,I_INV_RATE,I_UWEIGHT , ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,STOCK_LEDGER.STL_STORE_TYPE, STORE_MASTER.STORE_NAME  SELECT I_CODE,I_CODENO,I_NAME,I_UOM_CODE,I_UOM_NAME,STL_STORE_TYPE,STORE_NAME,STL_DOC_QTY,   UPLOAD_QTY,  ROUND(ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0),2)	 AS  DIFF_QTY,I_INV_RATE,I_UWEIGHT ,   ABS(ROUND((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0))	* I_INV_RATE,2)) AS AMT,   ABS(ROUND((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0))	 * I_UWEIGHT,2)) AS TONUGE FROM #temp  ORDER BY DIFF_QTY    DROP TABLE #temp   ");
            if (dtTostore.Rows.Count > 0)
            {
                dgMaterialAcceptance.DataSource = dtTostore;
                dgMaterialAcceptance.DataBind();
                dgMaterialAcceptance.Enabled = true;

                double amt = 0, tonnuge = 0, nos = 0, UPLOAD_QTY = 0;
                for (int i = 0; i < dtTostore.Rows.Count; i++)
                {
                    amt = amt + Convert.ToDouble(dtTostore.Rows[i]["AMT"].ToString());
                    UPLOAD_QTY = UPLOAD_QTY + Convert.ToDouble(dtTostore.Rows[i]["UPLOAD_QTY"].ToString());
                    tonnuge = tonnuge + Convert.ToDouble(dtTostore.Rows[i]["TONUGE"].ToString());
                    nos = nos + Math.Abs(Convert.ToDouble(dtTostore.Rows[i]["DIFF_QTY"].ToString()));
                }
                lblAMT.Text = amt.ToString();
                lbltonnuge.Text = tonnuge.ToString();
                lblNOS.Text = nos.ToString();

                if (UPLOAD_QTY == 0)
                {
                    btnSubmit.Visible = false;
                }
                else
                {
                    btnSubmit.Visible = true;
                }
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STL_STORE_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STORE_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STL_DOC_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UPLOAD_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("DIFF_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_INV_RATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UWEIGHT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TONUGE", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMaterialAcceptance.DataSource = dtFilter;
                    dgMaterialAcceptance.DataBind();
                    dgMaterialAcceptance.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    #endregion ddlToStore_SelectedIndexChanged

    #region GetExcelSheetNames
    private String[] GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            // Connection String. Change the excel file to the file you
            // will search.
            String connString = "";
            try
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
            }
            catch (Exception ex)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\";";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
            }
            // Get the data table containg the schema guid.
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }

            String[] excelSheets = new String[dt.Rows.Count];
            int i = 0;

            // Add the sheet name to the string array.
            foreach (DataRow row in dt.Rows)
            {
                excelSheets[i] = row["TABLE_NAME"].ToString();
                i++;
            }

            // Loop through all of the sheets if you want too...
            for (int j = 0; j < excelSheets.Length; j++)
            {
                // Query each excel sheet.
            }
            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    #endregion

    #region btnImport_Click
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                CommonClasses.Execute("DELETE FROM STORE_TEMP_TRANSACTION where TRAN_STORE_CODE='" + ddlToStore.SelectedValue + "'");
                PanelMsg.Visible = false;
                DirectoryInfo DI = new DirectoryInfo(Server.MapPath(@"~/UpLoadPath/STOCK/"));
                FileInfo[] DelfilesXLS = DI.GetFiles("*.xls");
                FileInfo[] DelfilesXLSX = DI.GetFiles("*.xlsx");
                int i = 0;
                foreach (FileInfo fi in DelfilesXLS)
                {
                    System.IO.File.Delete(DI + "/" + DelfilesXLS[i]);
                    i++;
                }
                i = 0;
                foreach (FileInfo fi in DelfilesXLSX)
                {
                    System.IO.File.Delete(DI + "/" + DelfilesXLSX[i]);
                    i++;
                }
                string filename1 = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.FileName);

                if (new[] { ".xls", ".xlsx" }.Contains(fileExtension.ToLower()))
                {
                    FileUpload1.SaveAs(Server.MapPath(@"~/UpLoadPath/STOCK/" + filename1));
                    string filename = Server.MapPath(@"~/UpLoadPath/STOCK/" + filename1);
                    string[] SheetName1 = GetExcelSheetNames(filename);
                    string SheetName = SheetName1[0].ToString();
                    string Con = "";
                    if (fileExtension.Trim() == ".xls")
                    {
                        Con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (fileExtension.Trim() == ".xlsx")
                    {
                        Con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    OleDbConnection oleConn = new OleDbConnection(Con);
                    oleConn.Open();
                    OleDbCommand oleCmdSelect = new OleDbCommand();
                    oleCmdSelect = new OleDbCommand(
                            @"SELECT * FROM ["
                            + SheetName +
                            "]", oleConn);
                    OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
                    oleAdapter.SelectCommand = oleCmdSelect;
                    DataTable dt = new DataTable("Table1");
                    oleAdapter.FillSchema(dt, SchemaType.Source);
                    oleAdapter.Fill(dt);
                    oleCmdSelect.Dispose();
                    oleCmdSelect = null;
                    oleAdapter.Dispose();
                    oleAdapter = null;
                    oleConn.Dispose();
                    oleConn = null;

                    if (dt.Columns[1].ColumnName.Trim().ToUpper() != "ITEM CODE")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The 2nd(B) Column Name  Sould be 'Item Code' instead of " + dt.Columns[1].ColumnName + "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    if (dt.Columns[5].ColumnName.Trim().ToUpper() != "PHYSICAL QTY")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The 5rd(C) Column Name Sould be 'Physical QTY' instead of " + dt.Columns[2].ColumnName + "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    #region BulkInsert
                    /* Clear Existing Temp Data*/
                    CommonClasses.Execute("truncate table TEMP_STORE");
                    string strConnString = string.Empty;
                    strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
                    SqlConnection conn = null;
                    conn = new SqlConnection(strConnString);
                    conn.Open();
                    SqlBulkCopy bulkInsert = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                    bulkInsert.DestinationTableName = "TEMP_STORE";
                    bulkInsert.ColumnMappings.Add("ITEM CODE", "TRAN_I_CODE");
                    bulkInsert.ColumnMappings.Add("ITEM_NAME", "TRAN_I_NAME");
                    bulkInsert.ColumnMappings.Add("Physical Qty", "TRAN_QTY");
                    bulkInsert.WriteToServer(dt);
                    conn.Close();
                    /*Check Duplicate Items in Temp Table*/
                    DataTable dtDuplicate = CommonClasses.Execute("select TRAN_I_CODE +' '+TRAN_I_NAME as TRAN_I_CODE from TEMP_STORE GROUP BY TRAN_I_CODE +' '+TRAN_I_NAME HAVING COUNT(TRAN_I_CODE +' '+TRAN_I_NAME)>1");
                    if (dtDuplicate.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Duplicate Item Found " + dtDuplicate.Rows[0]["TRAN_I_CODE"].ToString();
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        /* Clear Temp Data*/
                        CommonClasses.Execute("truncate table TEMP_STORE");
                        return;
                    }
                    /* Clear inserted Temp Data*/
                    CommonClasses.Execute("truncate table TEMP_STORE");
                    #endregion BulkInsert

                    #region I_Code_Not_Found_Code_Comment
                    //if (dt.Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dt.Rows.Count; k++)
                    //    {
                    //        DataTable dtIcodeCheck = CommonClasses.Execute("select * from ITEM_MASTER where ES_DELETE=0 and UPPER(LTRIM(RTRIM((I_CODENO))))='" + dt.Rows[k]["ITEM CODE"].ToString().TrimEnd().TrimStart().ToUpper() + "'");
                    //        if (dtIcodeCheck.Rows.Count > 0)
                    //        {
                    //        }
                    //        else
                    //        {
                    //            PanelMsg.Visible = true;
                    //            lblmsg.Text = "Item Code Not found at Sr No " + (k + 1);
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //            return;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "No rows Found";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //    return;
                    //}
                    //string I_Cat = "";
                    //string I_code = "";
                    //string UOM = "";
                    //string W_UOM = "";
                    //string ISDEV = ""; 
                    #endregion I_Code_Not_Found_Code_Comment

                    //CommonClasses.BulkInsertDataTable("TEMP_STORE", dt);

                    string I_code = "";
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        /*Issue resolve :-  Bind Item Name As well for finding Item Code*/
                        DataTable dtIcodeCheck = CommonClasses.Execute("select I_CODE,I_CODENO+' '+ I_NAME as Item_Name from ITEM_MASTER where ES_DELETE=0 and UPPER(LTRIM(RTRIM((I_CODENO))))='" + dt.Rows[j]["ITEM CODE"].ToString().TrimEnd().TrimStart().ToUpper().Replace("'", "''") + "' and UPPER(LTRIM(RTRIM((I_NAME))))='" + dt.Rows[j]["ITEM_NAME"].ToString().TrimEnd().TrimStart().ToUpper().Replace("'", "''") + "' ");
                        if (dtIcodeCheck.Rows.Count > 0)
                        {
                            I_code = dtIcodeCheck.Rows[0]["I_CODE"].ToString();
                            CommonClasses.Execute("INSERT INTO STORE_TEMP_TRANSACTION(TRAN_DATE, TRAN_STORE_CODE, TRAN_ITEM_CODE, TRAN_QTY, TRAN_USER_CODE)VALUES     ('" + Convert.ToDateTime(System.DateTime.Now).ToString("dd/MMM/yyyy") + "','" + ddlToStore.SelectedValue + "','" + I_code + "','" + dt.Rows[j]["Physical Qty"].ToString() + "','" + Session["UserCode"].ToString() + "')");
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = dt.Rows[j]["ITEM CODE"].ToString() + "Item Not Found ... Please Check In Item Master";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    DataTable dtTostore = CommonClasses.Execute(" SELECT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,I_INV_RATE,I_UWEIGHT ,   STOCK_LEDGER.STL_STORE_TYPE, STORE_MASTER.STORE_NAME, SUM(STOCK_LEDGER.STL_DOC_QTY) AS STL_DOC_QTY ,   ISNULL((SELECT ISNULL(STORE_TEMP_TRANSACTION.TRAN_QTY,0) FROM  STORE_TEMP_TRANSACTION where TRAN_ITEM_CODE= I_CODE AND TRAN_STORE_CODE='" + ddlToStore.SelectedValue + "'  ),0 ) AS UPLOAD_QTY INTO #temp  FROM STOCK_LEDGER INNER JOIN ITEM_MASTER ON STOCK_LEDGER.STL_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN STORE_MASTER ON STOCK_LEDGER.STL_STORE_TYPE = STORE_MASTER.STORE_CODE  WHERE     (ITEM_MASTER.ES_DELETE = 0) AND I_CAT_CODE=-2147483648 AND (STOCK_LEDGER.STL_STORE_TYPE = '" + ddlToStore.SelectedValue + "')  GROUP BY ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, I_INV_RATE,I_UWEIGHT , ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,STOCK_LEDGER.STL_STORE_TYPE, STORE_MASTER.STORE_NAME   SELECT I_CODE,I_CODENO,I_NAME,I_UOM_CODE,I_UOM_NAME,STL_STORE_TYPE,STORE_NAME,STL_DOC_QTY,   UPLOAD_QTY,  ROUND(ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0),2)	 AS  DIFF_QTY,I_INV_RATE,I_UWEIGHT ,   ABS(ROUND((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0))	* I_INV_RATE,2)) AS AMT,   ABS(ROUND((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0))	 * I_UWEIGHT,2)) AS TONUGE FROM #temp  ORDER BY DIFF_QTY    DROP TABLE #temp   ");
                    if (dtTostore.Rows.Count > 0)
                    {
                        dgMaterialAcceptance.DataSource = dtTostore;
                        dgMaterialAcceptance.DataBind();
                        dgMaterialAcceptance.Enabled = true;
                        double amt = 0, tonnuge = 0, nos = 0;
                        for (int p = 0; p < dtTostore.Rows.Count; p++)
                        {
                            amt = amt + Convert.ToDouble(dtTostore.Rows[p]["AMT"].ToString());
                            tonnuge = tonnuge + Convert.ToDouble(dtTostore.Rows[p]["TONUGE"].ToString());
                            nos = nos + Math.Abs(Convert.ToDouble(dtTostore.Rows[p]["DIFF_QTY"].ToString()));
                        }
                        lblAMT.Text = amt.ToString();
                        lbltonnuge.Text = tonnuge.ToString();
                        lblNOS.Text = nos.ToString();
                    }
                    btnSubmit.Visible = true;
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Physical Stock Is importred for " + ddlToStore.SelectedItem;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Visible = true;
                    lblmsg.Text = "Upload Correct Format";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError(" -VIEW", "btnImport_Click", ex.Message); ;
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Save"))
        {
            if (ddlToStore.SelectedValue != "0")
            {
                if (dgMaterialAcceptance.Rows.Count != 0)
                {

                    if (SaveRec())
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Receord Upload Sucessfully For " + ddlToStore.SelectedItem;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Receord Not Upload For " + ddlToStore.SelectedItem;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Fill Table";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select To Store Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You have No Rights to Save ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
        ddlToStore_SelectedIndexChanged(null, null);
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("Select isnull(max(SAM_DOC_NO),0) as SAM_DOC_NO FROM STOCK_ADJUSTMENT_MASTER WHERE SAM_CM_COMP_CODE = " + (string)Session["CompanyCode"] + " AND SAM_TYPE=1 and ES_DELETE=0");
        if (dt.Rows[0]["SAM_DOC_NO"] == null || dt.Rows[0]["SAM_DOC_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["SAM_DOC_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            int Doc_no = Convert.ToInt32(Numbering());
            DataTable dt = new DataTable();

            if (CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_MASTER(SAM_DATE,SAM_DOC_NO,SAM_CM_COMP_CODE,SAM_FROM_STORE,SAM_TYPE) VALUES ('" + Convert.ToDateTime(System.DateTime.Now).ToString("dd MMM yyyy") + "','" + Doc_no + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlToStore.SelectedValue + "',1)"))
            {
                string Code = CommonClasses.GetMaxId("Select Max(SAM_CODE) from STOCK_ADJUSTMENT_MASTER");
                for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                {
                    //Inserting Into Issue To Production Detail
                    result = CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_DETAIL(SAD_SAM_CODE,SAD_I_CODE,SAD_ADJUSTMENT_QTY,SAD_REMARK,SAD_TO_STORE)VALUES('" + Code + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblDIFF_QTY")).Text + "','Upload','" + ddlToStore.SelectedValue + "')");

                    if (result == true)
                    {
                        // Inserting Into Stock Ledger
                        if (Convert.ToDouble(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblDIFF_QTY")).Text) != 0)
                        {
                            // Inserting Into Stock Ledger
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "','" + Code + "','" + Doc_no + "','STOCK ERROR CORRECTION','" + Convert.ToDateTime(System.DateTime.Now).ToString("dd MMM yyyy") + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblDIFF_QTY")).Text + "','" + ddlToStore.SelectedValue + "')");//Insert From Store here
                            }
                            // relasing Stock Form Item Master

                            if (result == true)
                            {
                                if (ddlToStore.SelectedValue == "-2147483642")
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblDIFF_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(System.DateTime.Now).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "'");
                                }
                                else
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblDIFF_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(System.DateTime.Now).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "'");
                                }
                            }
                        }
                        CommonClasses.Execute("DELETE FROM STORE_TEMP_TRANSACTION where TRAN_STORE_CODE='" + ddlToStore.SelectedValue + "'");
                    }
                }
                CommonClasses.WriteLog("Stock Adjustment Utility", "Save", "Stock Adjustment", Convert.ToString(Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Could not saved";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("STOCK_ADJUSTMENT_MASTER", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (dgMaterialAcceptance.Rows.Count != 0)
        {
            Session["dtTostore"] = "";
            DataTable dtTostore = CommonClasses.Execute("SELECT ITEM_MASTER.I_CODE,I_INV_RATE,I_UWEIGHT ,  ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,  STOCK_LEDGER.STL_STORE_TYPE, STORE_MASTER.STORE_NAME, SUM(STOCK_LEDGER.STL_DOC_QTY) AS STL_DOC_QTY ,   ISNULL((SELECT ISNULL(STORE_TEMP_TRANSACTION.TRAN_QTY,0) FROM  STORE_TEMP_TRANSACTION where TRAN_ITEM_CODE= I_CODE AND TRAN_STORE_CODE='" + ddlToStore.SelectedValue + "'  ),0 ) AS UPLOAD_QTY INTO #temp  FROM STOCK_LEDGER INNER JOIN ITEM_MASTER ON STOCK_LEDGER.STL_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN STORE_MASTER ON STOCK_LEDGER.STL_STORE_TYPE = STORE_MASTER.STORE_CODE  WHERE     (ITEM_MASTER.ES_DELETE = 0) AND I_CAT_CODE=-2147483648 AND (STOCK_LEDGER.STL_STORE_TYPE = '" + ddlToStore.SelectedValue + "')  GROUP BY ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,STOCK_LEDGER.STL_STORE_TYPE, STORE_MASTER.STORE_NAME  ,  I_INV_RATE,I_UWEIGHT    SELECT I_CODE,I_CODENO,I_NAME,I_UOM_CODE,I_UOM_NAME,STL_STORE_TYPE,STORE_NAME,ROUND(STL_DOC_QTY,2) AS STL_DOC_QTY,ROUND(UPLOAD_QTY,2) AS UPLOAD_QTY,ROUND((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0)),2) AS  DIFF_QTY,  I_INV_RATE,I_UWEIGHT ,ABS(round((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0))*I_INV_RATE ,2)) AS AMT,ABS(round((ISNULL(UPLOAD_QTY,0)-ISNULL(STL_DOC_QTY,0))*I_UWEIGHT ,2)) AS TOUNNGE  FROM #temp  ORDER BY DIFF_QTY    DROP TABLE #temp  ");
            if (dtTostore.Rows.Count > 0)
            {
                try
                {
                    Session["dtTostore"] = dtTostore;
                    Response.Redirect("~/Transactions/ADD/Export.aspx", false);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
    #endregion
}

