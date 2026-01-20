using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;

public partial class Transactions_ADD_SupplierPurchaseOrder : System.Web.UI.Page
{
    #region Variable
    DirectoryInfo ObjSearchDir;
    string fileName = "";
    string fileName1 = "";
    string fileName2 = "";
    string fileName3 = "";
    string fileName4 = "";
    string fileName5 = "";
    string fileName6 = "";
    string fileName7 = "";
    string fileName8 = "";
    string path1 = "";
    static int File_Code1 = 0;
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtPODetail = new DataTable();
    int Active = 0;
    static int File_Code = 0;
    string path = "";
    static int mlCode = 0;
    DataRow dr;
    public static DataTable dt2 = new DataTable();
    public static DataTable dt3 = new DataTable();
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    Double DescAmount;
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                IframeViewPDF.Attributes["src"] = "";
                if (!IsPostBack)
                {
                    ViewState["fileName"] = fileName;
                    ViewState["fileName1"] = fileName1;
                    ViewState["fileName2"] = fileName2;
                    ViewState["fileName3"] = fileName3;
                    ViewState["fileName4"] = fileName4;
                    ViewState["fileName5"] = fileName5;
                    ViewState["fileName6"] = fileName6;
                    ViewState["fileName7"] = fileName7;
                    ViewState["fileName8"] = fileName8;
                    ViewState["mlCode"] = mlCode;
                    ViewState["Index"] = Index;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["str"] = str;
                    LoadType();
                    LoadProCode();
                    LoadITariff();
                    AggrementFile.Visible = false;
                    ISCostBreakupFile.Visible = false;
                    IsQuotationFile.Visible = false;

                    chkIsDrawingFile.Visible = false;
                    PDIRFile.Visible = false;
                    samplingPlanFile.Visible = false;
                    TrayLayoutFile.Visible = false;
                    MISCFile.Visible = false;
                    try
                    {
                        ViewState["mlCode"] = mlCode;
                        if (dt2 != null)
                        {
                        }
                        else
                        {
                            dt2 = new DataTable();
                        }
                        if (dt3 == null)
                        {
                            dt3 = new DataTable();
                        }
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        ViewState["dt3"] = dt3;
                        if (((DataTable)ViewState["dt3"]).Rows.Count > 0)
                        {
                            ((DataTable)ViewState["dt3"]).Rows.Clear();
                        }
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("AMEND"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("AMEND");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadPoType();
                            LoadSupplier();
                            LoadICode();
                            LoadIName();
                            LoadTax();
                            LoadUnit();
                            LoadRateUOM();
                            LoadIndentNo();
                            LoadCurr();
                            chkActiveInd.Checked = true;
                            BlankGridView();
                            txtConversionRetio.Enabled = false;
                            txtPoDate.Attributes.Add("readonly", "readonly");
                            txtPoDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtSupplierRefDate.Attributes.Add("readonly", "readonly");
                            txtSupplierRefDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtValid.Attributes.Add("readonly", "readonly");
                            txtValid.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
                        }
                        ddlPOType.Focus();
                        //imgUpload.Attributes["onchange"] = "UploadFile(this)";
                        //IsQuotationFile.Attributes["onchange"] = "UploadFile(this)";
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Supplier Order", "Page_Load", ex.Message.ToString());
                    }
                }
                #region DetailFiles
                if (IsPostBack && imgUpload.PostedFile != null)
                {
                    if (imgUpload.PostedFile.FileName.Length > 0)
                    {
                        fileName = imgUpload.PostedFile.FileName;
                        ViewState["fileName"] = fileName;
                        Upload(null, null);
                    }
                }
                if (IsPostBack && chkIsDrawingFile.PostedFile != null)
                {
                    if (chkIsDrawingFile.PostedFile.FileName.Length > 0)
                    {
                        fileName4 = chkIsDrawingFile.PostedFile.FileName;
                        ViewState["fileName4"] = fileName4;
                        UploadDrawing(null, null);
                    }
                }
                if (IsPostBack && PDIRFile.PostedFile != null)
                {
                    if (PDIRFile.PostedFile.FileName.Length > 0)
                    {
                        fileName5 = PDIRFile.PostedFile.FileName;
                        ViewState["fileName5"] = fileName5;
                        UploadPDIR(null, null);
                    }
                }
                if (IsPostBack && samplingPlanFile.PostedFile != null)
                {
                    if (samplingPlanFile.PostedFile.FileName.Length > 0)
                    {
                        fileName6 = samplingPlanFile.PostedFile.FileName;
                        ViewState["fileName6"] = fileName6;
                        UploadsamplingPlan(null, null);
                    }
                }
                if (IsPostBack && TrayLayoutFile.PostedFile != null)
                {
                    if (TrayLayoutFile.PostedFile.FileName.Length > 0)
                    {
                        fileName7 = TrayLayoutFile.PostedFile.FileName;
                        ViewState["fileName7"] = fileName7;
                        UploadTrayLayout(null, null);
                    }
                }
                if (IsPostBack && MISCFile.PostedFile != null)
                {
                    if (MISCFile.PostedFile.FileName.Length > 0)
                    {
                        fileName8 = MISCFile.PostedFile.FileName;
                        ViewState["fileName8"] = fileName8;
                        UploadMISCFile(null, null);
                    }
                }

                #endregion DetailFiles

                #region MasterFiles

                if (IsPostBack && IsQuotationFile.PostedFile != null)
                {
                    if (IsQuotationFile.PostedFile.FileName.Length > 0)
                    {
                        fileName1 = IsQuotationFile.PostedFile.FileName;
                        ViewState["fileName1"] = fileName1;
                        UploadQuotation(null, null);
                    }
                }
                if (IsPostBack && ISCostBreakupFile.PostedFile != null)
                {
                    if (ISCostBreakupFile.PostedFile.FileName.Length > 0)
                    {
                        fileName2 = ISCostBreakupFile.PostedFile.FileName;
                        ViewState["fileName2"] = fileName2;
                        UploadCostBreakup(null, null);
                    }
                }
                if (IsPostBack && AggrementFile.PostedFile != null)
                {
                    if (AggrementFile.PostedFile.FileName.Length > 0)
                    {
                        fileName3 = AggrementFile.PostedFile.FileName;
                        ViewState["fileName3"] = fileName3;
                        UploadAggrement(null, null);
                    }
                }
                #endregion MasterFiles
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region LoadITariff
    private void LoadITariff()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT E_CODE,E_TARIFF_NO FROM EXCISE_TARIFF_MASTER WHERE ES_DELETE=0 AND E_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY E_TARIFF_NO");
            ddlTariff.DataSource = dt;
            ddlTariff.DataTextField = "E_TARIFF_NO";
            ddlTariff.DataValueField = "E_CODE";
            ddlTariff.DataBind();
            ddlTariff.Items.Insert(0, new ListItem("HSN No.", "0"));
            ddlTariff.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("LoadIUnit", "LoadIUnit", Ex.Message);
        }
    }
    #endregion

    #region CarryForward
    public void CarryForward()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TOP 1 * from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' order by SPOM_CODE DESC");
            if (dt.Rows.Count > 0)
            {
                txtPaymentTerm.Text = dt.Rows[0]["SPOM_PAY_TERM1"].ToString();
                txtDeliverySchedule.Text = dt.Rows[0]["SPOM_DEL_SHCEDULE"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["SPOM_DELIVERED_TO"].ToString();
                txtFreightTermsg.Text = dt.Rows[0]["SOM_FREIGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["SPOM_GUARNTY"].ToString();
                txtPacking.Text = dt.Rows[0]["SPOM_PACKING"].ToString();
                txtNote.Text = dt.Rows[0]["SPOM_NOTES"].ToString();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Order", "CarryForward", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadType
    private void LoadType()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(PO_T_CODE) ,PO_T_DESC from PO_TYPE_MASTER where ES_DELETE=0 and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by SO_T_DESC");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "PO_T_DESC";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Purchase Order", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadIndentNo
    public void LoadIndentNo()
    {
        try
        {
            DataTable dtIndenttype = new DataTable();
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dtIndenttype = CommonClasses.Execute("SELECT INM_CODE,IN_TNO FROM INDENT_MASTER WHERE ES_DELETE=0 AND IN_CM_CODE='" + Session["CompanyCode"] + "' AND ISNULL(IN_AUTHORIZE1,0)=1  AND INM_CODE NOT IN (select ISNULL(SPOM_IN_NO,0) from SUPP_PO_MASTER  where ES_DELETE=0 AND SPOM_CM_CODE='" + Session["CompanyCode"] + "') ORDER BY INM_CODE DESC");

            }
            else
            {
                dtIndenttype = CommonClasses.Execute("SELECT INM_CODE,IN_TNO FROM INDENT_MASTER WHERE ES_DELETE=0 AND IN_CM_CODE='" + Session["CompanyCode"] + "' AND ISNULL(IN_AUTHORIZE1,0)=1 ORDER BY INM_CODE DESC");

            }
            ddlindentNo.DataSource = dtIndenttype;
            ddlindentNo.DataTextField = "IN_TNO";
            ddlindentNo.DataValueField = "INM_CODE";
            ddlindentNo.DataBind();
            ddlindentNo.Items.Insert(0, new ListItem("Select Indent ", "0"));
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Indent Detail", "LoadIndentType", Ex.Message);
        }
    }
    #endregion

    #region LoadIndentSupplier
    public void LoadIndentSupplier()
    {

    }
    #endregion

    #region LoadItemCode
    private void LoadItemCode()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!='-2147483648'  AND I_CODE IN ( select IND_SPECIFICATION  from INDENT_DETAIL where IND_INM_CODE='" + ddlindentNo.SelectedValue + "') ORDER BY I_CODENO");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;

            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region ddlindentNo_SelectedIndexChanged
    protected void ddlindentNo_SelectedIndexChanged(object sender, EventArgs e)
    {


        LoadSupplierIndent();
        LoadItemCode();
        //string Code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
        DataTable dtfile = new DataTable();
        dtfile = CommonClasses.Execute("select IFU_FILE,IFU_INM_CODE from  INDENT_FILE_UPLOAD,INDENT_MASTER where IFU_INM_CODE=INM_CODE and ES_DELETE=0 and IN_APPROVE=1 and IFU_APPROVE=1 and INM_CODE='" + ddlindentNo.SelectedValue + "'");
        if (dtfile.Rows.Count > 0)
        {
            string rootFolderPath = Server.MapPath(@"~\UpLoadPath\IndentFileMulti\" + dtfile.Rows[0]["IFU_INM_CODE"].ToString());
            string destinationPath = Server.MapPath(@"~\UpLoadPath\FILEUPLOAD\");
            // string filesToDelete = @"*_DONE.wav";   // Only delete WAV files ending by "_DONE" in their filenames

            /* Create directory if not exist*/
            ObjSearchDir = new DirectoryInfo(destinationPath);
            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }

            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath);
            foreach (string file in fileList)
            {
                string fileToMove = file;
                /* Get file name from folder*/
                string lastvalue = Path.GetFileName(file);
                if (lastvalue == dtfile.Rows[0]["IFU_FILE"].ToString())
                {
                    string moveTo = destinationPath + '\\' + lastvalue;
                    FileInfo file1 = new FileInfo(moveTo);
                    if (File.Exists(moveTo))
                    {
                        file1.Delete();
                    }
                    //moving file
                    /* File from source to destination folder*/
                    System.IO.File.Copy(fileToMove, moveTo);
                    lnkQuotation.Text = lastvalue.ToString();
                    ViewState["fileName1"] = lastvalue.ToString();
                }

            }

        }
    }
    #endregion

    #region LoadSupplierIndent
    public void LoadSupplierIndent()
    {
        DataTable dtindentsupp = new DataTable();
        dtindentsupp = CommonClasses.Execute("SELECT IN_SUPP_CODE,IN_SUPP_NAME,IN_PROJECT FROM INDENT_MASTER WHERE IN_APPROVE=1 AND ES_DELETE=0 AND IN_CM_CODE='" + Session["CompanyCode"] + "'  AND INM_CODE='" + ddlindentNo.SelectedValue + "'");
        //ddlSupplier.DataSource = dtindentsupp;
        //ddlSupplier.DataTextField = "IN_SUPP_NAME";
        //ddlSupplier.DataValueField = "IN_SUPP_CODE";
        //ddlSupplier.DataBind();
        if (!Request.QueryString[0].Equals("AMEND"))
        {
            ddlSupplier.SelectedValue = dtindentsupp.Rows[0]["IN_SUPP_CODE"].ToString();
            ddlProjectCode.SelectedValue = dtindentsupp.Rows[0]["IN_PROJECT"].ToString();
        }
        //ddlSupplier.Items.Insert(0, new ListItem("Select Supplier ", "0"));
    }
    #endregion


    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgSupplierPurchaseOrder.Rows.Count != 0)
        {
            if (ddlindentNo.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Indent ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlindentNo.Focus();
                return;
            }
            if (ddlPOType.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select PO Type", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlPOType.Focus();
                return;
            }
            if (ddlProjectCode.SelectedValue == "0")
            {
                ShowMessage("#Avisos", "Select Project Code", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlProjectCode.Focus();
                return;
            }
            if (ddlSupplier.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlSupplier.Focus();
                return;
            }
            if (txtPoDate.Text == "")
            {
                ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPoDate.Focus();
            }
            if (txtPaymentTerm.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Please Enter PO Payment Terms", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPaymentTerm.Focus();
                return;
            }
            if (Convert.ToDateTime(txtPoDate.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]))
            {
                ShowMessage("#Avisos", "Please Select Date in current financial year", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPoDate.Focus();
                return;
            }

            if (Convert.ToDateTime(txtPoDate.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
            {
                ShowMessage("#Avisos", "Please Select Date in current financial year ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPoDate.Focus();
                return;
            }

            #region Check_Null_Value_For_File
            if (ViewState["fileName1"] == "" || ViewState["fileName1"] == null)
                ViewState["fileName1"] = DBNull.Value;
            if (ViewState["fileName2"] == "" || ViewState["fileName2"] == null)
                ViewState["fileName2"] = DBNull.Value;
            if (ViewState["fileName3"] == "" || ViewState["fileName3"] == null)
                ViewState["fileName3"] = DBNull.Value;
            #endregion Check_Null_Value_For_File

            if (dgSupplierPurchaseOrder.Enabled && dgSupplierPurchaseOrder.Rows.Count > 0)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Insert Item Detail", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            //if (mlCode != 0 && mlCode != null)
            //{
            //    CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", mlCode);
            //}

            //dt2.Rows.Clear();
            //Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);

            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
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
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Purchase Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    private void CreateDataTable()
    {
        #region datatable structure
        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode1");

            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
            ((DataTable)ViewState["dt2"]).Columns.Add("Rate");

            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("Round");
            ((DataTable)ViewState["dt2"]).Columns.Add("TotalAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("NetTotal");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscPerc");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("ConversionRatio");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcInclusive");
            ((DataTable)ViewState["dt2"]).Columns.Add("Excdatepass");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcGapRate");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcDuty");
            ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");

            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax1");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax");
            ((DataTable)ViewState["dt2"]).Columns.Add("round1");
            ((DataTable)ViewState["dt2"]).Columns.Add("PurVatCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("SerTCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("Specification");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_ITEM_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("ActiveInd");

            ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_INW_QTY");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_DRAWING");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_PDIR");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_SAMPPLAN");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_TRAYLAYOUT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_MISC");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_DRAWING_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_PDIR_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_SAMPPLAN_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_TRAYLAYOUT_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_MISC_FILE");
        }
        #endregion
        ViewState["NewTable"] = dt2;
    }

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        Double Per = 100;
        try
        {
            if (ddlPOType.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select PO Type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlPOType.Focus();
                return;
            }

            if (ddlSupplier.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Supplier";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlSupplier.Focus();
                return;
            }
            if (txtPoDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Po Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPoDate.Focus();
                return;
            }

            if (ddlItemCode.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }

            if (ddlItemName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            if (txtOrderQty.Text == String.Empty)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Order Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtMissItemName.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should not less than Inward Qty " + txtMissItemName.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            if (txtRate.Text == "" || txtRate.Text == "0.00")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter  Rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRate.Focus();
                return;
            }
            if (ddlRateUOM.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Rate Unit";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlRateUOM.Focus();
                return;
            }
            if (chkActiveInd.Checked == true)
            {
                Active = 1;
            }
            else
            {
                Active = 0;
            }
            //if (ddlSalesTax.SelectedIndex == 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Please Select Sales Tax";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlSalesTax.Focus();
            //    return;
            //}
            if (txtConversionRetio.Text == "" || txtConversionRetio.Text == "0.0")
            {
                if (ddlStockUOM.SelectedItem.Text != ddlRateUOM.SelectedItem.Text)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Conversion Ratio";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0.00";
            }
            if (txtTotalAmount.Text == "")
            {
                txtTotalAmount.Text = "0.00";
            }
            if (txtDescAmount.Text == "")
            {
                DescAmount = 0.00;
            }
            else
            {
                DescAmount = Convert.ToDouble(txtDescAmount.Text);
            }
            if (txtDescPerc.Text == "")
            {
                txtDescPerc.Text = "0";
            }
            else if (Convert.ToDouble(txtDescPerc.Text) > Per)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Percentage not greter than 100";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDescPerc.Focus();
                return;
            }
            if (txtExcisePer.Text.Trim() == "")
            {
                txtExcisePer.Text = "0.00";
            }
            if (txtDescAmount.Text == "")
            {
                txtDescAmount.Text = "0.00";
            }
            // dt = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE  from SALES_TAX_MASTER where  ST_CODE=" + ddlSalesTax.SelectedValue + " and ES_DELETE=0 and  ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "");
            //Double Tax = Convert.ToDouble(dt.Rows[0]["ST_SALES_TAX"].ToString());

            Double Subamount = (Convert.ToDouble(txtTotalAmount.Text.ToString()) - DescAmount);

            double E_BASIC = 0;
            double E_EDU_CESS = 0;
            double E_H_EDU = 0;

            double SPOD_EXC_AMT = 0;
            double SPOD_EXC_E_AMT = 0;
            double SPOD_EXC_HE_AMT = 0;

            double AccesableValue = Convert.ToDouble(txtTotalAmount.Text) - Convert.ToDouble(txtDescAmount.Text);

            dt = CommonClasses.Execute("select E_BASIC, E_EDU_CESS,E_H_EDU from EXCISE_TARIFF_MASTER,ITEM_MASTER where I_E_CODE=E_CODE AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND ITEM_MASTER.I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"].ToString());
                E_EDU_CESS = Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"].ToString());
                E_H_EDU = Convert.ToDouble(dt.Rows[0]["E_H_EDU"].ToString());

                DataTable dtCompState = CommonClasses.Execute("SELECT CM_STATE FROM COMPANY_MASTER WHERE CM_ID=" + (string)Session["CompanyId"] + "");
                DataTable dtPartyStae = CommonClasses.Execute("SELECT P_SM_CODE FROM PARTY_MASTER WHERE P_CODE='" + ddlSupplier.SelectedValue + "'");

                if (dtCompState.Rows[0][0].ToString() == dtPartyStae.Rows[0][0].ToString())
                {
                    SPOD_EXC_AMT = Math.Round((AccesableValue * E_BASIC / 100), 2);
                    SPOD_EXC_E_AMT = Math.Round((AccesableValue * E_EDU_CESS / 100), 2);
                }
                else
                {
                    SPOD_EXC_HE_AMT = Math.Round((AccesableValue * E_H_EDU / 100), 2);
                }
            }

            if (dgSupplierPurchaseOrder.Rows.Count > 0)
            {
                for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
                ViewState["ItemUpdateIndex"] = "-1";
            }

            dt2 = (DataTable)ViewState["NewTable"];

            DataTable dtsample = (DataTable)ViewState["dt2"];

            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                for (int i = ((DataTable)ViewState["dt2"]).Rows.Count - 1; i >= 0; i--)
                {
                    if (((DataTable)ViewState["dt2"]).Rows[i][0] == DBNull.Value)
                        ((DataTable)ViewState["dt2"]).Rows[i].Delete();
                }
                ((DataTable)ViewState["dt2"]).AcceptChanges();
            }

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemCode.SelectedValue;
            dr["ItemCode1"] = ddlItemCode.SelectedItem;
            dr["ItemName1"] = ddlItemName.SelectedValue;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["StockUOM1"] = ddlStockUOM.SelectedValue;
            dr["StockUOM"] = ddlStockUOM.SelectedItem;
            dr["OrderQty"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text)), 2));
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));
            dr["RateUOM1"] = ddlRateUOM.SelectedValue;
            dr["RateUOM"] = ddlRateUOM.SelectedItem;
            dr["TotalAmount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtOrderQty.Text)), 2));
            dr["DiscPerc"] = txtDescPerc.Text;
            dr["DiscAmount"] = string.Format("{0:0.00}", Math.Round(DescAmount), 2);
            if (txtConversionRetio.Text == "")
            {
                dr["ConversionRatio"] = 0.0;
            }
            else
            {
                dr["ConversionRatio"] = Convert.ToDouble(txtConversionRetio.Text);
            }
            dr["ExcInclusive"] = chkExcInclusive.Checked;
            dr["SalesTax1"] = "-2147483648";
            dr["SalesTax"] = "Not Applicable";
            dr["Specification"] = txtSpecification.Text;
            dr["SPOD_ITEM_NAME"] = txtMissItemName.Text.Trim();
            dr["ActiveInd"] = Active;
            dr["E_BASIC"] = E_BASIC;
            dr["E_EDU_CESS"] = E_EDU_CESS;
            dr["E_H_EDU"] = E_H_EDU;
            if (txtMissItemName.Text.Trim() == "")
            {
                txtMissItemName.Text = "0";
            }
            dr["SPOD_INW_QTY"] = txtMissItemName.Text;

            dr["DocName"] = ViewState["fileName"].ToString();
            dr["SPOD_E_CODE"] = ddlTariff.SelectedValue;
            dr["SPOD_E_TARIFF_NO"] = ddlTariff.SelectedItem;

            dr["SPOD_EXC_AMT"] = string.Format("{0:0.00}", Math.Round(SPOD_EXC_AMT, 2));
            dr["SPOD_EXC_E_AMT"] = string.Format("{0:0.00}", Math.Round(SPOD_EXC_E_AMT, 2));
            dr["SPOD_EXC_HE_AMT"] = string.Format("{0:0.00}", Math.Round(SPOD_EXC_HE_AMT, 2));

            dr["SPOD_IS_DRAWING"] = chkIsDrawing.Checked;
            dr["SPOD_IS_PDIR"] = chkPDIR.Checked;
            dr["SPOD_IS_SAMPPLAN"] = chksamplingPlan.Checked;
            dr["SPOD_IS_TRAYLAYOUT"] = chkIsTrayLayout.Checked;
            dr["SPOD_IS_MISC"] = chkIsMISC.Checked;

            if (ViewState["fileName4"] == "" || ViewState["fileName4"] == null)
                ViewState["fileName4"] = DBNull.Value;
            if (ViewState["fileName5"] == "" || ViewState["fileName5"] == null)
                ViewState["fileName5"] = DBNull.Value;
            if (ViewState["fileName6"] == "" || ViewState["fileName6"] == null)
                ViewState["fileName6"] = DBNull.Value;
            if (ViewState["fileName7"] == "" || ViewState["fileName7"] == null)
                ViewState["fileName7"] = DBNull.Value;
            if (ViewState["fileName8"] == "" || ViewState["fileName8"] == null)
                ViewState["fileName8"] = DBNull.Value;

            if (chkIsDrawing.Checked == true)
            {
                dr["SPOD_DRAWING_FILE"] = ViewState["fileName4"].ToString();
            }
            if (chkPDIR.Checked == true)
            {
                dr["SPOD_PDIR_FILE"] = ViewState["fileName5"].ToString();
            }
            if (chksamplingPlan.Checked == true)
            {
                dr["SPOD_SAMPPLAN_FILE"] = ViewState["fileName6"].ToString();
            }
            if (chkIsTrayLayout.Checked == true)
            {
                dr["SPOD_TRAYLAYOUT_FILE"] = ViewState["fileName7"].ToString();
            }
            if (chkIsMISC.Checked == true)
            {
                dr["SPOD_MISC_FILE"] = ViewState["fileName8"].ToString();
            }

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            dgSupplierPurchaseOrder.Visible = true;
            dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
            dgSupplierPurchaseOrder.DataBind();
            dgSupplierPurchaseOrder.Enabled = true;
            #endregion

            ddlItemCode.Enabled = true;
            ddlItemName.Enabled = true;

            GetTotal();
            clearDetail();
            ddlRateUOM.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Order", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlPOType.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtPoDate.Text == "")
            {
                flag = false;
            }
            else if (ddlSupplier.SelectedIndex == 0)
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
            CommonClasses.SendError("Supplier PO", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!='-2147483648' ORDER BY I_CODENO");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region LoadPoType
    private void LoadPoType()
    {
        try
        {
            dt = CommonClasses.Execute("select PO_T_CODE,PO_T_SHORT_NAME AS PO_T_SHORT_NAME from PO_TYPE_MASTER where ES_DELETE=0 and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY PO_T_SHORT_NAME");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "PO_T_SHORT_NAME";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Select Po Type", "0"));
            ddlPOType.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE!='-2147483648' ORDER BY I_NAME");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadTax
    private void LoadTax()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ST_TAX_NAME");
            ddlSalesTax.DataSource = dt;
            ddlSalesTax.DataTextField = "ST_TAX_NAME";
            ddlSalesTax.DataValueField = "ST_CODE";
            ddlSalesTax.DataBind();
            ddlSalesTax.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            ddlSalesTax.SelectedIndex = 2;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order ", "LoadTax", Ex.Message);
        }
    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2'   AND (P_STM_CODE=-2147483648 OR P_STM_CODE=-2147483646)  AND    P_ACTIVE_IND=1 order by P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadRateUOM
    private void LoadRateUOM()
    {
        try
        {
            DataTable dt1 = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID=1");

            if (dt1.Rows.Count > 0)
            {
                ddlRateUOM.DataSource = dt1;
                ddlRateUOM.DataTextField = "I_UOM_NAME";
                ddlRateUOM.DataValueField = "I_UOM_CODE";
                ddlRateUOM.DataBind();
                ddlRateUOM.Items.Insert(0, new ListItem("Select Po Unit", "0"));
                ddlRateUOM.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order -Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    void SetHSN()
    {
        DataTable dt1 = CommonClasses.Execute("Select I_E_CODE from ITEM_MASTER where ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlTariff.SelectedValue = dt1.Rows[0]["I_E_CODE"].ToString();
    }

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {

                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;

                SetHSN();

                LoadUOM();
                ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                DataTable dt = CommonClasses.Execute("select isnull(SPOD_RATE,0) as SPOD_RATE from ITEM_MASTER ,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CODE=SPOD_I_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE= (SELECT MAX(SPOM_CODE) FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "')");
                if (dt.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["SPOD_RATE"]);
                }
                else
                {
                    DataTable dtt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");
                    txtRate.Text = string.Format("{0:0.00}", dtt.Rows[0]["I_INV_RATE"]);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order -Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlPOType_SelectedIndexChanged
    protected void ddlPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPOType.SelectedIndex != 0)
        {
        }
        if (ddlPOType.SelectedValue == "-2147483647")
        {
            pnlCurrancy.Visible = true;
        }
        else
        {
            pnlCurrancy.Visible = false;
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSupplier.SelectedIndex != 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Clear();
            BlankGridView();
            CarryForward();
        }
    }
    #endregion

    #region LoadUnit
    private void LoadUnit()
    {
        try
        {
            dt = CommonClasses.Execute("select I_UOM_CODE ,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ");
            ddlStockUOM.DataSource = dt;
            ddlStockUOM.DataTextField = "I_UOM_NAME";
            ddlStockUOM.DataValueField = "I_UOM_CODE";
            ddlStockUOM.DataBind();
            ddlStockUOM.Items.Insert(0, new ListItem("Unit", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Transaction", "LoadUnit", Ex.Message);
        }
    }
    #endregion

    #region LoadUOM
    private void LoadUOM()
    {
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE, I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlStockUOM.DataSource = dt1;
        ddlStockUOM.DataTextField = "I_UOM_NAME";
        ddlStockUOM.DataValueField = "I_UOM_CODE";
        ddlStockUOM.DataBind();
    }
    #endregion

    #region LoadCurr
    private void LoadCurr()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER where ES_DELETE=0 and CURR_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataTextField = "CURR_NAME";
            ddlCurrency.DataValueField = "CURR_CODE";
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, new ListItem("Select currency", "0"));
            ddlCurrency.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                SetHSN();
                LoadUOM();
                ddlRateUOM.SelectedValue = ddlStockUOM.SelectedValue;
                DataTable dt = CommonClasses.Execute("select isnull(SPOD_RATE,0) as SPOD_RATE from ITEM_MASTER ,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CODE=SPOD_I_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE= (SELECT MAX(SPOM_CODE) FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "')");
                if (dt.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.00}", dt.Rows[0]["SPOD_RATE"]);
                }
                else
                {
                    DataTable dtt = CommonClasses.Execute("select isnull(I_INV_RATE,0) as I_INV_RATE from ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");
                    txtRate.Text = string.Format("{0:0.00}", dtt.Rows[0]["I_INV_RATE"]);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order -Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadRateUOM();
            LoadPoType();
            LoadSupplier();
            LoadICode();
            LoadIName();
            LoadTax();
            LoadCurr();
            LoadProCode();
            dtPODetail.Clear();
            LoadIndentNo();
            txtPoDate.Attributes.Add("readonly", "readonly");
            txtSupplierRefDate.Attributes.Add("readonly", "readonly");
            txtValid.Attributes.Add("readonly", "readonly");

            dt = CommonClasses.Execute("Select SPOM_CODE,SPOM_TYPE,SPOM_DATE,SPOM_IN_NO,SPOM_PLANT,SPOM_PO_NO,SPOM_P_CODE,SPOM_AMOUNT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_VALID_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_PROJECT,SPOM_PONO,SPOM_PROJ_NAME,SPOM_IS_QUOTATION,SPOM_IS_COSTBREAKUP,SPOM_IS_AGGREMENT,SPOM_QUOTATION_FILE,SPOM_COSTBREAKUP_FILE,SPOM_AGGREMENT_FILE from SUPP_PO_MASTER where ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SPOM_CODE"]);
                ddlPOType.SelectedValue = dt.Rows[0]["SPOM_TYPE"].ToString();
                if (ddlPOType.SelectedValue == "-2147483647")
                {
                    pnlCurrancy.Visible = true;
                    ddlCurrency.SelectedValue = dt.Rows[0]["SPOM_CURR_CODE"].ToString();
                    ddlCurrency_SelectedIndexChanged(null, null);
                }
                else
                {
                    pnlCurrancy.Visible = false;
                }

                ddlindentNo.SelectedValue = dt.Rows[0]["SPOM_IN_NO"].ToString();

                ddlSupplier.SelectedValue = dt.Rows[0]["SPOM_P_CODE"].ToString();
                txtPoNo.Text = dt.Rows[0]["SPOM_PO_NO"].ToString();
                txtPoNo.Text = dt.Rows[0]["SPOM_PONO"].ToString();

                txtPoDate.Text = Convert.ToDateTime(dt.Rows[0]["SPOM_DATE"]).ToString("dd MMM yyyy");
                txtSupplierRefDate.Text = Convert.ToDateTime(dt.Rows[0]["SPOM_SUP_REF_DATE"]).ToString("dd MMM yyyy");

                txtValid.Text = Convert.ToDateTime(dt.Rows[0]["SPOM_VALID_DATE"]).ToString("dd MMM yyyy");
                txtSupplierRef.Text = dt.Rows[0]["SPOM_SUP_REF"].ToString();
                ddlProjectCode.SelectedValue = dt.Rows[0]["SPOM_PROJECT"].ToString();
                txtProjName.Text = dt.Rows[0]["SPOM_PROJ_NAME"].ToString();
                txtFinalTotalAmount.Text = string.Format("{0:0.00}", dt.Rows[0]["SPOM_AMOUNT"]);
                txtTranspoter.Text = dt.Rows[0]["SPOM_TRANSPOTER"].ToString();
                txtFreightTermsg.Text = dt.Rows[0]["SOM_FREIGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["SPOM_GUARNTY"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["SPOM_DELIVERED_TO"].ToString();
                txtNote.Text = dt.Rows[0]["SPOM_NOTES"].ToString();
                txtPaymentTerm.Text = dt.Rows[0]["SPOM_PAY_TERM1"].ToString();
                txtDeliverySchedule.Text = dt.Rows[0]["SPOM_DEL_SHCEDULE"].ToString();
                txtCIF.Text = dt.Rows[0]["SPOM_CIF_NO"].ToString();
                txtPacking.Text = dt.Rows[0]["SPOM_PACKING"].ToString();
                rbtWithAmt.SelectedValue = dt.Rows[0]["SPOM_PLANT"].ToString();

                if (dt.Rows[0]["SPOM_IS_QUOTATION"].ToString().ToUpper() == "TRUE")
                {
                    chkIsQuotation.Checked = true;
                    IsQuotationFile.Visible = true;
                }
                else
                {
                    chkIsQuotation.Checked = false;
                }
                if (dt.Rows[0]["SPOM_IS_COSTBREAKUP"].ToString().ToUpper() == "TRUE")
                {
                    chkISCostBreakup.Checked = true;
                    ISCostBreakupFile.Visible = true;
                }
                else
                {
                    chkISCostBreakup.Checked = false;
                }
                if (dt.Rows[0]["SPOM_IS_AGGREMENT"].ToString().ToUpper() == "TRUE")
                {
                    chkAggrement.Checked = true;
                    AggrementFile.Visible = true;
                }
                else
                {
                    chkAggrement.Checked = false;
                }
                if (chkIsQuotation.Checked == true)
                {
                    lnkQuotation.Text = dt.Rows[0]["SPOM_QUOTATION_FILE"].ToString();
                }
                if (chkISCostBreakup.Checked == true)
                {
                    lnkCostBreakup.Text = dt.Rows[0]["SPOM_COSTBREAKUP_FILE"].ToString();
                }
                if (chkAggrement.Checked == true)
                {
                    lnkAggrement.Text = dt.Rows[0]["SPOM_AGGREMENT_FILE"].ToString();
                }

                ViewState["fileName1"] = dt.Rows[0]["SPOM_QUOTATION_FILE"].ToString();
                ViewState["fileName2"] = dt.Rows[0]["SPOM_COSTBREAKUP_FILE"].ToString();
                ViewState["fileName3"] = dt.Rows[0]["SPOM_AGGREMENT_FILE"].ToString();

                dtPODetail = CommonClasses.Execute("select SPOD_I_CODE as ItemCode,I_CODENO as ShortName,SPOD_FILE as Docname,I_DOC_PATH,SPOD_I_CODE as ItemName1, I_CODENO as ItemCode1,I_NAME as ItemName,SPOD_UOM_CODE as StockUOM1,I_UOM_NAME as StockUOM, cast(SPOD_ORDER_QTY as numeric(10,3)) as OrderQty,cast(SPOD_RATE as numeric(20,2)) as Rate,SPOD_RATE_UOM as RateUOM1,I_UOM_NAME as RateUOM,SPOD_CONV_RATIO as ConversionRatio,cast(SPOD_TOTAL_AMT as numeric(20,2)) as TotalAmount,cast(SPOD_DISC_PER as float) as DiscPerc,cast(SPOD_DISC_AMT as numeric(20,2)) as DiscAmount,isnull(SPOD_EXC_Y_N,0) as ExcInclusive,SPOD_T_CODE as SalesTax1,ST_TAX_NAME as SalesTax ,SPOD_ACTIVE_IND as ActiveInd ,SPOD_SPECIFICATION as Specification,SPOD_ITEM_NAME,SPOD_EXC_PER as E_BASIC, SPOD_EDU_CESS_PER as E_EDU_CESS,SPOD_H_EDU_CESS as E_H_EDU,ISNULL(SPOD_INW_QTY,0) AS SPOD_INW_QTY ,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT, ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,ISNULL(SPOD_E_CODE,0) AS SPOD_E_CODE,ISNULL(SPOD_E_TARIFF_NO,0) AS SPOD_E_TARIFF_NO,SPOD_IS_DRAWING,SPOD_IS_PDIR,SPOD_IS_SAMPPLAN,SPOD_IS_TRAYLAYOUT,SPOD_IS_MISC,SPOD_DRAWING_FILE,SPOD_PDIR_FILE,SPOD_SAMPPLAN_FILE,SPOD_TRAYLAYOUT_FILE,SPOD_MISC_FILE FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,ITEM_UNIT_MASTER,SALES_TAX_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=SUPP_PO_DETAILS.SPOD_UOM_CODE and SPOD_I_CODE=ITEM_MASTER.I_CODE and SPOD_T_CODE=SALES_TAX_MASTER.ST_CODE and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                if (dtPODetail.Rows.Count != 0)
                {
                    dgSupplierPurchaseOrder.DataSource = dtPODetail;
                    dgSupplierPurchaseOrder.DataBind();
                    ViewState["dt2"] = dtPODetail;

                    GetTotal();
                }
                DataTable dtDoc = CommonClasses.Execute("select SPOA_SPOM_CODE,SPOA_DOC_NAME,SPOA_DOC_PATH from SUPP_PO_ATTACHMENT where SPOA_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (dtDoc.Rows.Count > 0)
                {
                    dgDocView.DataSource = dtDoc;
                    dgDocView.DataBind();
                    ViewState["dt3"] = dtDoc;
                }
            }

            if (str == "VIEW")
            {
                rbtWithAmt.Enabled = false;
                btnSubmit.Visible = false;
                btnInsert.Enabled = false;
                txtConversionRetio.Enabled = false;
                txtPoDate.Enabled = false;
                txtDescAmount.Enabled = false;
                txtDescPerc.Enabled = false;
                txtOrderQty.Enabled = false;
                txtPoDate.Enabled = false;
                txtPoNo.Enabled = false;
                txtValid.Enabled = false;
                ddlProjectCode.Enabled = false;
                txtRate.Enabled = false;
                txtGuranteeWaranty.Enabled = false;
                txtSpecification.Enabled = false;
                ddlStockUOM.Enabled = false;
                txtSupplierRef.Enabled = false;
                txtSupplierRefDate.Enabled = false;
                txtTotalAmount.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlPOType.Enabled = false;
                ddlRateUOM.Enabled = false;
                ddlSalesTax.Enabled = false;
                ddlSupplier.Enabled = false;
                chkActiveInd.Enabled = false;
                chkExcInclusive.Enabled = false;
                txtDeliverySchedule.Enabled = false;
                txtTranspoter.Enabled = false;
                txtPaymentTerm.Enabled = false;
                txtDeliveryTo.Enabled = false;
                txtFreightTermsg.Enabled = false;
                txtNote.Enabled = false;
                // dgSupplierPurchaseOrder.Enabled = false;
                ddlCurrency.Enabled = false;
                txtCurrencyRate.Enabled = false;
                txtMissItemName.Enabled = false;
                chkAggrement.Enabled = false;
                chkIsQuotation.Enabled = false;
                chkISCostBreakup.Enabled = false;
                IsQuotationFile.Enabled = false;
                AggrementFile.Enabled = false;
                ISCostBreakupFile.Enabled = false;
                chkIsDrawing.Enabled = false;
                chkPDIR.Enabled = false;
                chksamplingPlan.Enabled = false;
                chkIsTrayLayout.Enabled = false;
                chkIsMISC.Enabled = false;
                chkIsDrawingFile.Enabled = false;
                PDIRFile.Enabled = false;
                chksamplingPlan.Enabled = false;
                TrayLayoutFile.Enabled = false;
                MISCFile.Enabled = false;
                dgSupplierPurchaseOrder.Columns[0].Visible = false;
                dgSupplierPurchaseOrder.Columns[1].Visible = false;

            }
            else if (str == "MOD" || str == "AMEND")
            {
                rbtWithAmt.Enabled = false;
                ddlindentNo.Enabled = false;
                if (ddlindentNo.SelectedValue == "0")
                {
                    ddlindentNo.Enabled = true;
                }

                ddlSupplier.Enabled = false;
                txtConversionRetio.Enabled = false;
                CommonClasses.SetModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Supplier Purchase Order Transaction", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Supplier Purchase Order ", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            ddlStockUOM.SelectedIndex = 0;
            ddlTariff.SelectedIndex = 0;
            txtOrderQty.Text = "";
            txtRate.Text = "";
            ddlRateUOM.SelectedIndex = 0;
            ViewState["fileName"] = "";
            txtTotalAmount.Text = "";
            txtDescPerc.Text = "";
            txtDescAmount.Text = "";
            txtConversionRetio.Text = "";
            chkExcInclusive.Checked = false;
            chkIsMISC.Checked = false;
            chkIsDrawing.Checked = false;
            chkPDIR.Checked = false;
            chksamplingPlan.Checked = false;
            chkIsTrayLayout.Checked = false;

            // ddlSalesTax.SelectedIndex = 0;

            txtSpecification.Text = "";
            txtMissItemName.Text = "0.00";
            txtExcisePer.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double CGST_AMT = 0, SGST_AMT = 0, IGST_AMT = 0;
        if (dgSupplierPurchaseOrder.Rows.Count > 0)
        {
            for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount"))).Text;
                string QED_Disc = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount"))).Text;
                double Amount = Convert.ToDouble(QED_AMT);
                double Discount = Convert.ToDouble(QED_Disc);
                decTotal = decTotal + Amount - Discount;

                string SGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT"))).Text;
                string CGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT"))).Text;
                string IGST = ((Label)(dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT"))).Text;

                CGST_AMT = CGST_AMT + Convert.ToDouble(CGST);
                SGST_AMT = SGST_AMT + Convert.ToDouble(SGST);
                IGST_AMT = IGST_AMT + Convert.ToDouble(IGST);
            }
        }
        txtFinalTotalAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal + CGST_AMT + SGST_AMT + IGST_AMT), 2);
    }
    #endregion

    #region txtOrderQty_TextChanged
    protected void txtOrderQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "" || txtOrderQty.Text == Convert.ToDouble(0).ToString())
            {
                txtOrderQty.Text = "0";
            }
            if (txtMissItemName.Text.Trim() == "")
            {
                txtMissItemName.Text = "0";
            }
            if (Convert.ToDouble(txtOrderQty.Text) < Convert.ToDouble(txtMissItemName.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should not less than Inward Qty " + txtMissItemName.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            Calculate();
            txtRate.Focus();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Transaction", "txtOrderQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {
                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {

            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion

    #region Calculate
    public void Calculate()
    {
        string totalStr = "";

        if (txtOrderQty.Text == "")
        {
            txtOrderQty.Text = "0.00";
        }
        if (txtRate.Text == "")
        {
            txtRate.Text = "0.00";
        }
        if (txtTotalAmount.Text == "")
        {
            txtTotalAmount.Text = "0.00";
        }

        if (txtDescPerc.Text == "")
        {
            txtDescPerc.Text = "0.00";
        }
        if (txtDescAmount.Text == "")
        {
            txtDescAmount.Text = "0.00";
        }
        if (txtConversionRetio.Text == "")
        {
            txtConversionRetio.Text = "0.00";
        }
        totalStr = DecimalMasking(txtOrderQty.Text);
        txtOrderQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        totalStr = DecimalMasking(txtRate.Text);
        txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtOrderQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2));

        if (ddlStockUOM.SelectedItem.ToString() != ddlRateUOM.SelectedItem.ToString())
        {
            totalStr = DecimalMasking(txtConversionRetio.Text);
            txtConversionRetio.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            double TempTotal = Convert.ToDouble(txtOrderQty.Text) * (Convert.ToDouble(txtConversionRetio.Text));
            txtTotalAmount.Text = string.Format("{0:0.00}", Math.Round(TempTotal) * Convert.ToDouble(txtRate.Text.ToString()), 2);
        }
        totalStr = DecimalMasking(txtDescPerc.Text);
        txtDescPerc.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtDescAmount.Text = string.Format("{0:0.00}", Math.Round((((Convert.ToDouble(txtTotalAmount.Text.ToString())) / 100) * Convert.ToDouble(txtDescPerc.Text)), 2));

    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calculate();
            txtDescPerc.Focus();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order Transaction", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDescPerc_TextChanged
    protected void txtDescPerc_TextChanged(object sender, EventArgs e)
    {
        Calculate();
    }
    #endregion

    #region dgSupplierPurchaseOrder_RowCommand
    protected void dgSupplierPurchaseOrder_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgSupplierPurchaseOrder.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                string ICode = ((Label)(row.FindControl("lblItemCode"))).Text;

                DataTable dtCheckExistItem = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IWD_I_CODE='" + ICode + "'");

                if (dtCheckExistItem.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cannot delete the item bcause the item used in Inward";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtOrderQty.Focus();
                    return;
                }
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

                dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
                dgSupplierPurchaseOrder.DataBind();
                if (((DataTable)ViewState["dt2"]).Rows.Count == 0)
                {
                    BlankGridView();
                }
            }
            if (e.CommandName == "Modify")
            {
                LoadICode();
                LoadIName();
                LoadTax();
                LoadRateUOM();

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_NAME1"))).Text;
                ddlItemName_SelectedIndexChanged(null, null);
                ddlStockUOM.SelectedValue = ((Label)(row.FindControl("lblUOM1"))).Text;
                txtOrderQty.Text = ((Label)(row.FindControl("lblOrderQty"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblRate"))).Text;
                ddlRateUOM.SelectedValue = ((Label)(row.FindControl("lblRateUOM1"))).Text;
                txtTotalAmount.Text = ((Label)(row.FindControl("lblTotalAmount"))).Text;
                txtDescPerc.Text = ((Label)(row.FindControl("lblDiscPerc"))).Text;
                txtDescAmount.Text = ((Label)(row.FindControl("lblDiscAmount"))).Text;
                txtConversionRetio.Text = ((Label)(row.FindControl("lblConversionRatio"))).Text;
                ddlSalesTax.SelectedValue = ((Label)(row.FindControl("lblSalesTax1"))).Text;
                txtSpecification.Text = ((Label)(row.FindControl("lblSpecification"))).Text;
                txtMissItemName.Text = ((Label)(row.FindControl("lblSPOD_INW_QTY"))).Text;

                ViewState["fileName"] = ((LinkButton)(row.FindControl("lnkView"))).Text;
                ViewState["fileName4"] = ((LinkButton)(row.FindControl("lnkDrawongView"))).Text;
                ViewState["fileName5"] = ((LinkButton)(row.FindControl("lnkPDIRView"))).Text;
                ViewState["fileName6"] = ((LinkButton)(row.FindControl("lnkSPlanView"))).Text;
                ViewState["fileName7"] = ((LinkButton)(row.FindControl("lnkTrayLayoutView"))).Text;
                ViewState["fileName8"] = ((LinkButton)(row.FindControl("lnkMISCView"))).Text;

                lnkView.Visible = true;
                lnkView.Text = ((LinkButton)(row.FindControl("lnkView"))).Text;

                lnkDrawing.Visible = true;
                lnkDrawing.Text = ((LinkButton)(row.FindControl("lnkDrawongView"))).Text;

                lnkPDIR.Visible = true;
                lnkPDIR.Text = ((LinkButton)(row.FindControl("lnkPDIRView"))).Text;

                lnksamplingPlan.Visible = true;
                lnksamplingPlan.Text = ((LinkButton)(row.FindControl("lnkSPlanView"))).Text;

                lnkTrayLayout.Visible = true;
                lnkTrayLayout.Text = ((LinkButton)(row.FindControl("lnkTrayLayoutView"))).Text;

                lnkMISC.Visible = true;
                lnkMISC.Text = ((LinkButton)(row.FindControl("lnkMISCView"))).Text;

                if ((((Label)(row.FindControl("lblExcInclusive"))).Text).ToString() == "True")
                {
                    chkExcInclusive.Checked = true;
                }
                else
                {
                    chkExcInclusive.Checked = false;
                }
                if ((((Label)(row.FindControl("lblSPOD_IS_DRAWING"))).Text).ToString() == "True")
                {
                    chkIsDrawing.Checked = true;
                    chkIsDrawingFile.Visible = true;
                }
                else
                {
                    chkIsDrawing.Checked = false;
                }
                if ((((Label)(row.FindControl("lblSPOD_IS_PDIR"))).Text).ToString() == "True")
                {
                    chkPDIR.Checked = true;
                    PDIRFile.Visible = true;
                }
                else
                {
                    chkPDIR.Checked = false;
                }
                if ((((Label)(row.FindControl("lblSPOD_IS_SAMPPLAN"))).Text).ToString() == "True")
                {
                    chksamplingPlan.Checked = true;
                    samplingPlanFile.Visible = true;
                }
                else
                {
                    chksamplingPlan.Checked = false;
                }
                if ((((Label)(row.FindControl("lblSPOD_IS_TRAYLAYOUT"))).Text).ToString() == "True")
                {
                    chkIsTrayLayout.Checked = true;
                    TrayLayoutFile.Visible = true;
                }
                else
                {
                    chkIsTrayLayout.Checked = false;
                }
                if ((((Label)(row.FindControl("lblSPOD_IS_MISC"))).Text).ToString() == "True")
                {
                    chkIsMISC.Checked = true;
                    MISCFile.Visible = true;
                }
                else
                {
                    chkIsMISC.Checked = false;
                }
                txtExcisePer.Text = ((Label)(row.FindControl("lblE_BASIC"))).Text;

                if ((((Label)(row.FindControl("lblActiveInd"))).Text) == "1")
                {
                    chkActiveInd.Checked = true;
                }
                else
                {
                    chkActiveInd.Checked = false;
                }
                DataTable dtCheckExistItem = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND IWM_TYPE='IWIM' AND IWD_CPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dtCheckExistItem.Rows.Count > 0)
                {
                    ddlItemCode.Enabled = false;
                    ddlItemName.Enabled = false;
                }
                else
                {
                    ddlItemCode.Enabled = true;
                    ddlItemName.Enabled = true;
                }

                foreach (GridViewRow gvr in dgSupplierPurchaseOrder.Rows)
                {
                    LinkButton lnkButton = ((LinkButton)gvr.FindControl("lnkDelete"));
                    lnkButton.Enabled = false;
                }
            }
            if (e.CommandName == "ViewPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkView"))).Text;
                        directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                    }
                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            if (e.CommandName == "ViewDrawongPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkDrawongView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkDrawongView"))).Text;
                        directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            if (e.CommandName == "ViewPDIRPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkPDIRView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkPDIRView"))).Text;
                        directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            if (e.CommandName == "ViewSPlanPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkSPlanView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkSPlanView"))).Text;
                        directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            if (e.CommandName == "ViewTrayLayoutPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkTrayLayoutView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkTrayLayoutView"))).Text;
                        directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            if (e.CommandName == "ViewMISCPDF")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkMISCView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkMISCView"))).Text;
                        directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Purchase Order Transaction", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgSupplierPurchaseOrder_RowDeleting
    protected void dgSupplierPurchaseOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region LoadProCode
    private void LoadProCode()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
            ddlProjectCode.DataSource = dt;
            ddlProjectCode.DataTextField = "PROCM_NAME";
            ddlProjectCode.DataValueField = "PROCM_CODE";
            ddlProjectCode.DataBind();
            ddlProjectCode.Items.Insert(0, new ListItem("Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadProCode", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        bool result1 = false;
        try
        {
            if (txtSupplierRefDate.Text == "")
            {
                txtSupplierRefDate.Text = System.DateTime.Now.ToString();
            }
            if (txtPoDate.Text == "")
            {
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Po_Doc_no = 0;
                DataTable dt = new DataTable();
                string strPONo = "";
                dt = CommonClasses.Execute("Select isnull(max(SPOM_PO_NO),0) as SPOM_PO_NO FROM SUPP_PO_MASTER WHERE SPOM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and SPOM_CM_CODE='" + Session["CompanyCode"] + "'  and ES_DELETE=0 AND SPOM_POTYPE=0");
                if (dt.Rows.Count > 0)
                {
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["SPOM_PO_NO"]);

                    Po_Doc_no = Po_Doc_no + 1;
                    strPONo = "JPPL" + CommonClasses.GenBillNo(Po_Doc_no);
                }
                SqlTransaction trans = null;

                if (CommonClasses.Execute1("INSERT INTO SUPP_PO_MASTER(SPOM_CM_CODE,SPOM_AMOUNT,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SPOM_DEL_SHCEDULE,SPOM_TRANSPOTER,SPOM_PAY_TERM1,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_PROJECT,SPOM_VALID_DATE,SPOM_USER_CODE,SPOM_POTYPE,SPOM_PROJ_NAME,SPOM_PONO,SPOM_IS_QUOTATION,SPOM_IS_COSTBREAKUP,SPOM_IS_AGGREMENT,SPOM_QUOTATION_FILE,SPOM_COSTBREAKUP_FILE,SPOM_AGGREMENT_FILE,SPOM_IN_NO,SPOM_PLANT)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDouble(txtFinalTotalAmount.Text) + "','" + Convert.ToInt32(Session["CompanyId"]) + "', '" + ddlPOType.SelectedValue + "' ,'" + Po_Doc_no + "','" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtSupplierRef.Text.Replace("'", "\''") + "','" + Convert.ToDateTime(txtSupplierRefDate.Text).ToString("dd/MMM/yyyy") + "','" + txtDeliverySchedule.Text.Replace("'", "\''") + "','" + txtTranspoter.Text.Replace("'", "\''") + "','" + txtPaymentTerm.Text.Replace("'", "\''") + "','" + txtFreightTermsg.Text.Replace("'", "\''") + "','" + txtGuranteeWaranty.Text.Replace("'", "\''") + "','" + txtNote.Text.Replace("'", "\''") + "','" + txtDeliveryTo.Text.Replace("'", "\''") + "','" + ddlCurrency.SelectedValue + "','" + txtCIF.Text.Replace("'", "\''") + "','" + txtPacking.Text.Replace("'", "\''") + "','" + ddlProjectCode.SelectedValue + "','" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToInt32(Session["UserCode"].ToString()) + "',0,'" + txtProjName.Text.ToUpper().Trim().Replace("'", "\''") + "','" + strPONo + "','" + chkIsQuotation.Checked + "','" + chkISCostBreakup.Checked + "','" + chkAggrement.Checked + "','" + ViewState["fileName1"].ToString() + "','" + ViewState["fileName2"].ToString() + "','" + ViewState["fileName3"].ToString() + "','" + ddlindentNo.SelectedValue + "','" + rbtWithAmt.SelectedValue + "')"))
                {
                    CommonClasses.Execute("UPDATE  INDENT_MASTER  SET  IN_USEDIN='" + strPONo + "' where INM_CODE='" + ddlindentNo.SelectedValue + "'");
                    string Code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
                    #region Master_File_Uplaod
                    #region Qutation
                    if (ViewState["fileName1"].ToString() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    

                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName1"].ToString());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + Code + "/" + ViewState["fileName1"].ToString());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion Qutation

                    #region costbreakup
                    if (ViewState["fileName2"].ToString() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    

                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName2"].ToString());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + Code + "/" + ViewState["fileName2"].ToString());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion costbreakup

                    #region Aggrement
                    if (ViewState["fileName3"].ToString() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    

                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName3"].ToString());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + Code + "/" + ViewState["fileName3"].ToString());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion Aggrement
                    #endregion Master_File_Uplaod

                    for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                    {
                        //Done changes new suja
                        CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_EXC_AMT,SPOD_EXC_E_AMT,SPOD_EXC_HE_AMT,SPOD_E_CODE,SPOD_E_TARIFF_NO,SPOD_FILE,SPOD_IS_DRAWING,SPOD_IS_PDIR,SPOD_IS_SAMPPLAN,SPOD_IS_TRAYLAYOUT,SPOD_IS_MISC,SPOD_DRAWING_FILE,SPOD_PDIR_FILE,SPOD_SAMPPLAN_FILE,SPOD_TRAYLAYOUT_FILE,SPOD_MISC_FILE) values ('" + Code + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Replace("'", "\''") + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','0'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_TARIFF_NO")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_DRAWING")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_PDIR")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_SAMPPLAN")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_TRAYLAYOUT")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_MISC")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkDrawongView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkPDIRView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkSPlanView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkTrayLayoutView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkMISCView")).Text + "')");

                        #region lnkView
                        if (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text));
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + (((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text));
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion lnkView

                        #region Drawing
                        if (ViewState["fileName4"].ToString() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName4"].ToString());
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + ViewState["fileName4"].ToString());
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion Drawing

                        #region PDIR
                        if (ViewState["fileName5"].ToString() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName5"].ToString());
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + ViewState["fileName5"].ToString());
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion PDIR

                        #region Sampling Plan
                        if (ViewState["fileName6"].ToString() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName6"].ToString());
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + ViewState["fileName6"].ToString());
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion Sampling Plan

                        #region Tray Layout
                        if (ViewState["fileName7"].ToString() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName7"].ToString());
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + ViewState["fileName7"].ToString());
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion Tray Layout

                        #region MISC
                        if (ViewState["fileName8"].ToString() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOAD ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName8"].ToString());
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + Code + "/" + ViewState["fileName8"].ToString());
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion MISC
                    }
                    CommonClasses.WriteLog("Supplier Purchase Order", "Save", "Supplier Purchase Order", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ddlPOType.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("update SUPP_PO_MASTER set SPOM_TYPE='" + ddlPOType.SelectedValue + "',SPOM_SUP_REF ='" + txtSupplierRef.Text.Trim().Replace("'", "\''") + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text.Replace("'", "\''") + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text.Replace("'", "\''") + "',SPOM_TRANSPOTER='" + txtTranspoter.Text.Replace("'", "\''") + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text.Replace("'", "\''") + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text.Replace("'", "\''") + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text.Replace("'", "\''") + "',SPOM_NOTES='" + txtNote.Text.Replace("'", "\''") + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text.Replace("'", "\''") + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "' , SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "'  ,SPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "',SPOM_IS_QUOTATION='" + chkIsQuotation.Checked + "',SPOM_IS_COSTBREAKUP='" + chkISCostBreakup.Checked + "',SPOM_IS_AGGREMENT='" + chkAggrement.Checked + "',SPOM_QUOTATION_FILE='" + ViewState["fileName1"].ToString() + "',SPOM_COSTBREAKUP_FILE='" + ViewState["fileName2"].ToString() + "',SPOM_AGGREMENT_FILE='" + ViewState["fileName3"].ToString() + "' WHERE SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result1 = CommonClasses.Execute1("DELETE FROM SUPP_PO_ATTACHMENT WHERE SPOA_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                        {
                            //Done changes new suja
                            CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_EXC_AMT,SPOD_EXC_E_AMT,SPOD_EXC_HE_AMT,SPOD_E_CODE,SPOD_E_TARIFF_NO,SPOD_FILE,SPOD_IS_DRAWING,SPOD_IS_PDIR,SPOD_IS_SAMPPLAN,SPOD_IS_TRAYLAYOUT,SPOD_IS_MISC,SPOD_DRAWING_FILE,SPOD_PDIR_FILE,SPOD_SAMPPLAN_FILE,SPOD_TRAYLAYOUT_FILE,SPOD_MISC_FILE) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Replace("'", "\''") + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_INW_QTY")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_TARIFF_NO")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_DRAWING")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_PDIR")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_SAMPPLAN")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_TRAYLAYOUT")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_MISC")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkDrawongView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkPDIRView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkSPlanView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkTrayLayoutView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkMISCView")).Text + "')");
                        }

                        CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Supplier Purchase Order", "Update", "Supplier Purchase Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPOType.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("AMEND"))
            {
                int AMEND_COUNT = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = CommonClasses.Execute("select isnull(SPOM_AM_COUNT,0) as SPOM_AM_COUNT from SUPP_PO_MASTER WHERE SPOM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (dt.Rows.Count > 0)
                {
                    AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["SPOM_AM_COUNT"]);
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (AMEND_COUNT == 0)
                {
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                //When User Amend the po Update the Authorize flag to 0 (False) here
                CommonClasses.Execute1("update SUPP_PO_MASTER set SPOM_TYPE='" + ddlPOType.SelectedValue + "', SPOM_IS_SHORT_CLOSE='0' , SPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',SPOM_AM_COUNT='" + AMEND_COUNT + "',SPOM_SUP_REF ='" + txtSupplierRef.Text.Trim() + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "' , SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "' ,SPOM_VALID_DATE='" + Convert.ToDateTime(txtValid.Text).ToString("dd/MMM/yyyy") + "',SPOM_USER_CODE='" + Convert.ToInt32(Session["UserCode"].ToString()) + "',SPOM_IS_QUOTATION='" + chkIsQuotation.Checked + "',SPOM_IS_COSTBREAKUP='" + chkISCostBreakup.Checked + "',SPOM_AUTHR_FLAG=0,SPOM_IS_AGGREMENT='" + chkAggrement.Checked + "',SPOM_QUOTATION_FILE='" + ViewState["fileName1"].ToString() + "',SPOM_COSTBREAKUP_FILE='" + ViewState["fileName2"].ToString() + "',SPOM_AGGREMENT_FILE='" + ViewState["fileName3"].ToString() + "' WHERE SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (CommonClasses.Execute1("INSERT INTO SUPP_PO_AM_MASTER select SPOM_CODE,SPOM_CM_CODE,SPOM_CM_COMP_ID,SPOM_TYPE,SPOM_LM_CODE,SPOM_QUT_NO,SPOM_CONTACT_PERSON,SPOM_CONTACT_DETAIL,SPOM_PT_CODE,SPOM_PO_NO,SPOM_DATE,SPOM_P_CODE,SPOM_DISC_PER,SPOM_DISC_AMT,SPOM_AMOUNT,SPOM_TRANSPORT_AMT,SPOM_TRANSPORT_DESC,SPOM_OCTROI_PER,SPOM_OCTROI_DESC,SPOM_DELIVERY_AT,SPOM_LOADING_PER,SPOM_LOADING_DESC,SPOM_FREIGHT_AMT,SPOM_FREIGHT_DESC,SPOM_TERM_CODE,SPOM_INW_QTY,SPOM_DET_TERMS,SPOM_SCHED_S_DATE,SPOM_SCHED_E_DATE,SPOM_PAY_TERM,SPOM_REMARK,SPOM_DELIVERY_MODE,SPOM_VALID_DATE,MODIFY,ES_DELETE,SPOM_TEN_ID,SPOM_POST,SPOM_P_AMEND_CODE,SPOM_DEL_SHCEDULE,SPOM_PAY_TERM1,SPOM_TERMS_COND,SPOM_IS_AMEND,SPOM_WITH_CASH,SPOM_PO_UNIT,SPOM_SUP_REF,SPOM_SUP_REF_DATE,SOM_FREIGHT_TERM,SPOM_GUARNTY,SPOM_NOTES,SPOM_DELIVERED_TO,SPOM_TRANSPOTER,SPOM_AUTHR_FLAG,SPOM_CURR_CODE,SPOM_CIF_NO,SPOM_PACKING,SPOM_IS_SHORT_CLOSE,SPOM_AM_COUNT,SPOM_AM_DATE,SPOM_CANCEL_PO,SPOM_PROJECT, SPOM_USER_CODE,SPOM_POTYPE,SPOM_PONO,SPOM_PROJ_NAME,SPOM_IS_QUOTATION,SPOM_IS_COSTBREAKUP,SPOM_IS_AGGREMENT,SPOM_QUOTATION_FILE,SPOM_COSTBREAKUP_FILE,SPOM_AGGREMENT_FILE,SPOM_IN_NO from SUPP_PO_MASTER where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' "))
                {
                    CommonClasses.Execute("UPDATE  INDENT_MASTER  SET  IN_USEDIN='" + txtPoNo.Text + "' where INM_CODE='" + ddlindentNo.SelectedValue + "'");

                    string MatserCode = CommonClasses.GetMaxId("Select Max(AM_CODE) from SUPP_PO_AM_MASTER");
                    DataTable dtDetail = CommonClasses.Execute("select * from SUPP_PO_DETAILS where SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        CommonClasses.Execute1("INSERT INTO SUPP_PO_AMD_DETAILS (SPOD_AMD_CODE,SPOD_AMD_SPOM_AM_CODE,SPOD_AMD_I_CODE,SPOD_AMD_UOM_CODE,SPOD_AMD_ORDER_QTY,SPOD_AMD_RATE,SPOD_AMD_RATE_UOM,SPOD_AMD_CONV_RATIO,SPOD_AMD_DISC_PER,SPOD_AMD_DISC_AMT,SPOD_AMD_EXC_Y_N,SPOD_AMD_T_CODE,SPOD_AMD_TOTAL_AMT,SPOD_AMD_INW_QTY,SPOD_AMD_ACTIVE_IND,SPOD_AMD_SPECIFICATION,SPOD_AMD_EXC_PER,SPOD_AMD_EDU_CESS_PER,SPOD_AMD_H_EDU_CESS,AMD_AM_CODE,SPOD_AMD_ITEM_NAME,SPOD_EXC_AMT,SPOD_EXC_E_AMT,SPOD_EXC_HE_AMT,SPOD_E_CODE,SPOD_E_TARIFF_NO,SPOD_AMD_IS_DRAWING,SPOD_AMD_IS_PDIR,SPOD_AMD_IS_SAMPPLAN,SPOD_AMD_IS_TRAYLAYOUT,SPOD_AMD_IS_MISC,SPOD_AMD_DRAWING_FILE,SPOD_AMD_PDIR_FILE,SPOD_AMD_SAMPPLAN_FILE,SPOD_AMD_TRAYLAYOUT_FILE,SPOD_AMD_MISC_FILE) values('" + MatserCode + "','" + dtDetail.Rows[j]["SPOD_SPOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_I_CODE"] + "','" + dtDetail.Rows[j]["SPOD_UOM_CODE"] + "','" + dtDetail.Rows[j]["SPOD_ORDER_QTY"] + "','" + dtDetail.Rows[j]["SPOD_RATE"] + "','" + dtDetail.Rows[j]["SPOD_RATE_UOM"] + "','" + dtDetail.Rows[j]["SPOD_CONV_RATIO"] + "','" + dtDetail.Rows[j]["SPOD_DISC_PER"] + "','" + dtDetail.Rows[j]["SPOD_DISC_AMT"] + "','" + dtDetail.Rows[j]["SPOD_EXC_Y_N"] + "','" + dtDetail.Rows[j]["SPOD_T_CODE"] + "','" + dtDetail.Rows[j]["SPOD_TOTAL_AMT"] + "','" + dtDetail.Rows[j]["SPOD_INW_QTY"] + "','" + dtDetail.Rows[j]["SPOD_ACTIVE_IND"] + "','" + dtDetail.Rows[j]["SPOD_SPECIFICATION"].ToString().Replace("'", "\''") + "','" + dtDetail.Rows[j]["SPOD_EXC_PER"] + "','" + dtDetail.Rows[j]["SPOD_EDU_CESS_PER"] + "','" + dtDetail.Rows[j]["SPOD_H_EDU_CESS"] + "','" + MatserCode + "','" + dtDetail.Rows[j]["SPOD_ITEM_NAME"] + "','" + dtDetail.Rows[j]["SPOD_EXC_AMT"] + "','" + dtDetail.Rows[j]["SPOD_EXC_E_AMT"] + "','" + dtDetail.Rows[j]["SPOD_EXC_HE_AMT"] + "','" + dtDetail.Rows[j]["SPOD_E_CODE"] + "','" + dtDetail.Rows[j]["SPOD_E_TARIFF_NO"] + "','" + dtDetail.Rows[j]["SPOD_IS_DRAWING"] + "','" + dtDetail.Rows[j]["SPOD_IS_PDIR"] + "','" + dtDetail.Rows[j]["SPOD_IS_SAMPPLAN"] + "','" + dtDetail.Rows[j]["SPOD_IS_TRAYLAYOUT"] + "','" + dtDetail.Rows[j]["SPOD_IS_MISC"] + "','" + dtDetail.Rows[j]["SPOD_DRAWING_FILE"] + "','" + dtDetail.Rows[j]["SPOD_PDIR_FILE"] + "','" + dtDetail.Rows[j]["SPOD_SAMPPLAN_FILE"] + "','" + dtDetail.Rows[j]["SPOD_TRAYLAYOUT_FILE"] + "','" + dtDetail.Rows[j]["SPOD_MISC_FILE"] + "')");
                    }

                    #region ModifyOriginalPO
                    if (CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_TYPE='" + ddlPOType.SelectedValue + "',SPOM_P_CODE='" + ddlSupplier.SelectedValue + "',SPOM_PO_NO='" + txtPoNo.Text + "',SPOM_DATE='" + Convert.ToDateTime(txtPoDate.Text).ToString("dd/MMM/yyyy") + "',SPOM_AMOUNT='" + txtFinalTotalAmount.Text + "', SPOM_SUP_REF_DATE='" + txtSupplierRefDate.Text + "',SPOM_DEL_SHCEDULE='" + txtDeliverySchedule.Text + "',SPOM_TRANSPOTER='" + txtTranspoter.Text + "',SPOM_PAY_TERM1='" + txtPaymentTerm.Text + "',SOM_FREIGHT_TERM='" + txtFreightTermsg.Text + "',SPOM_GUARNTY='" + txtGuranteeWaranty.Text + "',SPOM_NOTES='" + txtNote.Text + "',SPOM_DELIVERED_TO='" + txtDeliveryTo.Text + "',SPOM_CURR_CODE='" + ddlCurrency.SelectedValue + "',SPOM_CIF_NO='" + txtCIF.Text + "',SPOM_PACKING='" + txtPacking.Text + "',SPOM_AM_COUNT='" + AMEND_COUNT + "',SPOM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',SPOM_PROJECT='" + ddlProjectCode.SelectedValue + "',SPOM_IS_QUOTATION='" + chkIsQuotation.Checked + "',SPOM_IS_COSTBREAKUP='" + chkISCostBreakup.Checked + "',SPOM_IS_AGGREMENT='" + chkAggrement.Checked + "',SPOM_QUOTATION_FILE='" + ViewState["fileName1"].ToString() + "',SPOM_COSTBREAKUP_FILE='" + ViewState["fileName2"].ToString() + "',SPOM_AGGREMENT_FILE='" + ViewState["fileName3"].ToString() + "' where SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                    {
                        result = CommonClasses.Execute1("update  SUPP_PO_AM_MASTER set SPOM_AM_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE SPOM_AM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and SPOM_AM_AM_COUNT='" + AMEND_COUNT + "'");
                        result = CommonClasses.Execute1("DELETE FROM SUPP_PO_DETAILS WHERE SPOD_SPOM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgSupplierPurchaseOrder.Rows.Count; i++)
                            {
                                CommonClasses.Execute1("INSERT INTO SUPP_PO_DETAILS (SPOD_SPOM_CODE,SPOD_I_CODE,SPOD_UOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOD_RATE_UOM,SPOD_CONV_RATIO,SPOD_TOTAL_AMT,SPOD_DISC_PER,SPOD_DISC_AMT,SPOD_EXC_Y_N,SPOD_T_CODE,SPOD_ACTIVE_IND,SPOD_SPECIFICATION,SPOD_ITEM_NAME,SPOD_INW_QTY,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_EXC_AMT,SPOD_EXC_E_AMT,SPOD_EXC_HE_AMT,SPOD_E_CODE,SPOD_E_TARIFF_NO,SPOD_FILE,SPOD_IS_DRAWING,SPOD_IS_PDIR,SPOD_IS_SAMPPLAN,SPOD_IS_TRAYLAYOUT,SPOD_IS_MISC,SPOD_DRAWING_FILE,SPOD_PDIR_FILE,SPOD_SAMPPLAN_FILE,SPOD_TRAYLAYOUT_FILE,SPOD_MISC_FILE) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblRateUOM1")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblConversionRatio")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblTotalAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscPerc")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblDiscAmount")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblExcInclusive")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSalesTax1")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblActiveInd")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSpecification")).Text.Replace("'", "\''") + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_ITEM_NAME")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_INW_QTY")).Text + "'," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_BASIC")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_EDU_CESS")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblE_H_EDU")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_E_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_EXC_HE_AMT")).Text + "," + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_CODE")).Text + ",'" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_E_TARIFF_NO")).Text + "', '" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkView")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_DRAWING")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_PDIR")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_SAMPPLAN")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_TRAYLAYOUT")).Text + "','" + ((Label)dgSupplierPurchaseOrder.Rows[i].FindControl("lblSPOD_IS_MISC")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkDrawongView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkPDIRView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkSPlanView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkTrayLayoutView")).Text + "','" + ((LinkButton)dgSupplierPurchaseOrder.Rows[i].FindControl("lnkMISCView")).Text + "')");
                            }

                            CommonClasses.RemoveModifyLock("SUPP_PO_MASTER", "MODIFY", "SPOM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                            CommonClasses.WriteLog("Supplier Purchase Order", "Amend", "Supplier Purcahse Order", txtPoNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            result = true;
                        }
                        Response.Redirect("~/Transactions/VIEW/ViewSupplierPurchaseOrder.aspx", false);
                    }
                    #endregion
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "PO Not Amend";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPOType.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Purchase Order", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ddlRateUOM_SelectedIndexChanged
    protected void ddlRateUOM_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStockUOM.SelectedItem.Text == ddlRateUOM.SelectedItem.Text)
        {
            txtConversionRetio.Enabled = false;
            txtConversionRetio.Text = "0";
            Calculate();
        }
        else
        {
            txtConversionRetio.Enabled = true;
        }
    }
    #endregion

    private void BlankGridView()
    {
        if (((DataTable)ViewState["dt2"]) != null)
        {
            ((DataTable)ViewState["dt2"]).Clear();
        }
        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName1");
            ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("StockUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
            ((DataTable)ViewState["dt2"]).Columns.Add("Rate");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM1");
            ((DataTable)ViewState["dt2"]).Columns.Add("RateUOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("Round");
            ((DataTable)ViewState["dt2"]).Columns.Add("TotalAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("NetTotal");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscPerc");
            ((DataTable)ViewState["dt2"]).Columns.Add("DiscAmount");
            ((DataTable)ViewState["dt2"]).Columns.Add("ConversionRatio");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcInclusive");
            ((DataTable)ViewState["dt2"]).Columns.Add("Excdatepass");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcGapRate");
            ((DataTable)ViewState["dt2"]).Columns.Add("ExcDuty");
            ((DataTable)ViewState["dt2"]).Columns.Add("EduCess");
            ((DataTable)ViewState["dt2"]).Columns.Add("SHECess");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax1");
            ((DataTable)ViewState["dt2"]).Columns.Add("SalesTax");
            ((DataTable)ViewState["dt2"]).Columns.Add("round1");
            ((DataTable)ViewState["dt2"]).Columns.Add("PurVatCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("SerTCatOn");
            ((DataTable)ViewState["dt2"]).Columns.Add("Specification");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_ITEM_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("ActiveInd");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_BASIC");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_EDU_CESS");
            ((DataTable)ViewState["dt2"]).Columns.Add("E_H_EDU");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_INW_QTY");
            ((DataTable)ViewState["dt2"]).Columns.Add("DocName");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_E_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_EXC_HE_AMT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_E_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_E_TARIFF_NO");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_DRAWING");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_PDIR");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_SAMPPLAN");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_TRAYLAYOUT");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_IS_MISC");

            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_DRAWING_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_PDIR_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_SAMPPLAN_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_TRAYLAYOUT_FILE");
            ((DataTable)ViewState["dt2"]).Columns.Add("SPOD_MISC_FILE");
        }
        ((DataTable)ViewState["dt2"]).Rows.Add(((DataTable)ViewState["dt2"]).NewRow());

        dgSupplierPurchaseOrder.Visible = true;
        dgSupplierPurchaseOrder.DataSource = ((DataTable)ViewState["dt2"]);
        dgSupplierPurchaseOrder.DataBind();
        dgSupplierPurchaseOrder.Enabled = false;

        ((DataTable)ViewState["dt3"]).Clear();
        if (((DataTable)ViewState["dt3"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_SPOM_CODE");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_NAME");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_PATH");
        }
        ((DataTable)ViewState["dt3"]).Rows.Add(((DataTable)ViewState["dt3"]).NewRow());

        dgDocView.Visible = true;
        dgDocView.DataSource = ((DataTable)ViewState["dt3"]);
        dgDocView.DataBind();
    }

    protected void btnAddDoc_Click(object sender, EventArgs e)
    {
        BindGridRow();
    }

    private void BindGridRow()
    {
        if (((DataTable)ViewState["dt3"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_SPOM_CODE");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_NAME");
            ((DataTable)ViewState["dt3"]).Columns.Add("SPOA_DOC_PATH");
        }
        ((DataTable)ViewState["dt3"]).Rows.Add(((DataTable)ViewState["dt3"]).NewRow());

        dgDocView.Visible = true;
        dgDocView.DataSource = ((DataTable)ViewState["dt3"]);
        dgDocView.DataBind();
    }

    protected void txtConversionRetio_TextChanged(object sender, EventArgs e)
    {
        Double TempTotal;
        if (ddlItemCode.SelectedIndex != 0)
        {
            if (txtTotalAmount.Text != "" || txtTotalAmount.Text != "0")
            {
                if (ddlStockUOM.SelectedItem.Text != ddlRateUOM.SelectedItem.Text)
                {
                    if (txtConversionRetio.Text != "" || txtConversionRetio.Text != "0")
                    {
                        Calculate();
                    }
                    else
                    {
                    }
                }
                else
                {
                    txtConversionRetio.Text = "";
                    txtConversionRetio.Enabled = false;
                    ddlRateUOM.SelectedIndex = 0;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Qty and Rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtConversionRetio.Text = "";
                txtConversionRetio.Enabled = false;
                ddlRateUOM.SelectedIndex = 0;
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please First Select on Item Code";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtConversionRetio.Text = "";
            txtConversionRetio.Enabled = false;
            ddlRateUOM.SelectedIndex = 0;
        }
    }
    protected void ddlStockUOM_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCurrrate = CommonClasses.Execute("SELECT CURR_CODE,CAST(CURR_RATE as numeric(20,2)) AS CURR_RATE FROM CURRANCY_MASTER WHERE ES_DELETE=0 and CURR_CODE='" + ddlCurrency.SelectedValue + "'");
            if (dtCurrrate.Rows.Count > 0)
            {
                txtCurrencyRate.Text = dtCurrrate.Rows[0]["CURR_RATE"].ToString();
            }
            else
            {
                txtCurrencyRate.Text = "0.00";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Po", "ddlCurrency_SelectedIndexChanged", Ex.Message);
        }
    }

    #region dgDocView_RowCommand
    protected void dgDocView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
        GridViewRow row = dgDocView.Rows[Convert.ToInt32(ViewState["Index"].ToString())];
        string filePath = "";
        FileInfo File;
        string code = "";
        string directory = "";

        if (e.CommandName == "Delete")
        {
            ((DataTable)ViewState["dt3"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));

            dgDocView.DataSource = ((DataTable)ViewState["dt3"]);
            dgDocView.DataBind();
            if (((DataTable)ViewState["dt3"]).Rows.Count == 0)
            {
                BlankGridView();
            }
        }
        if (e.CommandName == "View")
        {

            if (filePath != "")
            {
                File = new FileInfo(filePath);
            }
            else
            {
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    code = CommonClasses.GetMaxId("select isnull(max(SPOM_CODE+1),0)as Code from SUPP_PO_MASTER");
                }
                else
                {
                    code = ((Label)(row.FindControl("lblSPOA_SPOM_CODE"))).Text;
                    if (code == "")
                    {
                        code = CommonClasses.GetMaxId("Select SPOM_CODE from SUPP_PO_MASTER where SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");
                    }
                }
                filePath = ((Label)(row.FindControl("lblfilename"))).Text;
                directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
                Context.Response.Write("<script> language='javascript'>window.open('../../Transactions/ADD/ViewPdf.aspx?" + directory + "','_newtab');</script>");
            }
        }
        if (e.CommandName == "Download")
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                code = CommonClasses.GetMaxId("Select Max(SPOM_CODE) from SUPP_PO_MASTER");
            }
            else
            {
                code = ((Label)(row.FindControl("lblSPOA_SPOM_CODE"))).Text;
                if (code == "")
                {
                    code = CommonClasses.GetMaxId("Select SPOM_CODE from SUPP_PO_MASTER where SPOM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");
                }
            }
            filePath = ((Label)(row.FindControl("lblfilename"))).Text;
            directory = "../../UpLoadPath/SupplierPO/" + code + "/" + filePath;
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + directory + "\"");
            Response.TransmitFile(Server.MapPath(directory));
            Response.End();
        }
    }
    #endregion

    #region FileLoactionCreate

    void FileLoactionCreate()
    {
        DataTable dt = new DataTable();
        if (Request.QueryString[0].Equals("MODIFY"))
        {
            ViewState["File_Code"] = Convert.ToInt32(ViewState["mlCode"].ToString());
        }
        else
        {
            dt = CommonClasses.Execute("select isnull(max(SPOM_CODE+1),0)as Code from SUPP_PO_MASTER");
            if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
            {
                DataTable dt1 = CommonClasses.Execute(" SELECT IDENT_CURRENT('SUPP_PO_MASTER')+1");
                if (dt.Rows[0][0].ToString() == "-2147483647")
                {
                    ViewState["File_Code"] = -2147483648;
                }
                else
                {
                    ViewState["File_Code"] = int.Parse(dt1.Rows[0][0].ToString());
                }
            }
            else
            {
                ViewState["File_Code"] = int.Parse(dt.Rows[0][0].ToString());
            }
        }

        string sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["File_Code"].ToString() + "");

        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
    }

    #endregion

    protected void dgDocView_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            ViewState["fileName"] = null;
            FileLoactionCreate();

            GridViewRow row = dgDocView.Rows[e.RowIndex];

            FileUpload flUp = (FileUpload)row.FindControl("imgUpload");
            string directory = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["File_Code"].ToString() + "");

            string fileName = Path.GetFileName(flUp.PostedFile.FileName);

            flUp.SaveAs(Path.Combine(directory, fileName));

            ((Label)(dgDocView.Rows[e.RowIndex].FindControl("lblfilename"))).Text = fileName.ToString();
            ViewState["fileName"] = ViewState["File_Code"].ToString() + "/" + fileName.ToString();
        }
        catch { }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
    }

    #region dgDocView_RowDeleting
    protected void dgDocView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region chkExcInclusive_CheckedChanged
    protected void chkExcInclusive_CheckedChanged(object sender, EventArgs e)
    {
        if (chkExcInclusive.Checked == true)
        {
            txtExcisePer.Text = "0.00";
            txtExcisePer.Enabled = false;
        }
        else
        {
            txtExcisePer.Enabled = true;
        }
    }
    #endregion

    #region master
    #region UploadQuotation
    protected void UploadQuotation(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            IsQuotationFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName1"].ToString()));
        }
        else
        {
            IsQuotationFile.SaveAs(Server.MapPath("~/UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName1"].ToString()));
        }
        lnkQuotation.Visible = true;
        lnkQuotation.Text = ViewState["fileName1"].ToString();
    }
    #endregion

    #region UploadCostBreakup
    protected void UploadCostBreakup(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            ISCostBreakupFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName2"].ToString()));
        }
        else
        {
            ISCostBreakupFile.SaveAs(Server.MapPath("~/UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
        }
        lnkCostBreakup.Visible = true;
        lnkCostBreakup.Text = ViewState["fileName2"].ToString();
    }
    #endregion

    #region UploadAggrement
    protected void UploadAggrement(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            AggrementFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName3"].ToString()));
        }
        else
        {
            AggrementFile.SaveAs(Server.MapPath("~/UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName3"].ToString()));
        }
        lnkAggrement.Visible = true;
        lnkAggrement.Text = ViewState["fileName3"].ToString();
    }
    #endregion

    protected void lnkQuotation_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkQuotation.Text != "")
                {
                    filePath = lnkQuotation.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkQuotation.Text != "")
                {
                    filePath = lnkQuotation.Text;
                }

                directory = "../../UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnkQuotation1_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {

                if (lnkCostBreakup.Text != "")
                {
                    filePath = lnkCostBreakup.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkCostBreakup.Text != "")
                {
                    filePath = lnkCostBreakup.Text;
                }

                directory = "../../UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnkQuotation2_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {

                filePath = lnkAggrement.Text;

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {

                filePath = lnkAggrement.Text;

                directory = "../../UpLoadPath/SuppPoFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void chkIsQuotation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsQuotation.Checked == true)
        {
            IsQuotationFile.Visible = true;
        }
        else
        {
            IsQuotationFile.Visible = false;
            ViewState["fileName1"] = DBNull.Value;
        }
    }

    protected void chkISCostBreakup_CheckedChanged(object sender, EventArgs e)
    {
        if (chkISCostBreakup.Checked == true)
        {
            ISCostBreakupFile.Visible = true;
        }
        else
        {
            ISCostBreakupFile.Visible = false;
            ViewState["fileName2"] = DBNull.Value;
        }
    }

    protected void chkAggrement_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAggrement.Checked == true)
        {
            AggrementFile.Visible = true;
        }
        else
        {
            AggrementFile.Visible = false;
            ViewState["fileName3"] = DBNull.Value;
        }
    }
    #endregion Master

    #region Detail
    #region UploadDrawing
    protected void UploadDrawing(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            chkIsDrawingFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName4"].ToString()));
        }
        else
        {
            chkIsDrawingFile.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName4"].ToString()));
        }
        lnkDrawing.Visible = true;
        lnkDrawing.Text = ViewState["fileName4"].ToString();
    }
    #endregion

    #region UploadPDIR
    protected void UploadPDIR(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            PDIRFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName5"].ToString()));
        }
        else
        {
            PDIRFile.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName5"].ToString()));
        }
        lnkPDIR.Visible = true;
        lnkPDIR.Text = ViewState["fileName5"].ToString();
    }
    #endregion

    #region UploadsamplingPlan
    protected void UploadsamplingPlan(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            samplingPlanFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName6"].ToString()));
        }
        else
        {
            samplingPlanFile.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName6"].ToString()));
        }
        lnksamplingPlan.Visible = true;
        lnksamplingPlan.Text = ViewState["fileName6"].ToString();
    }
    #endregion

    #region UploadTrayLayout
    protected void UploadTrayLayout(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            TrayLayoutFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName7"].ToString()));
        }
        else
        {
            TrayLayoutFile.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName7"].ToString()));
        }
        lnkTrayLayout.Visible = true;
        lnkTrayLayout.Text = ViewState["fileName7"].ToString();
    }
    #endregion

    #region UploadMISCFile
    protected void UploadMISCFile(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            MISCFile.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName8"].ToString()));
        }
        else
        {
            MISCFile.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName8"].ToString()));
        }
        lnkMISC.Visible = true;
        lnkMISC.Text = ViewState["fileName8"].ToString();
    }
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        //if (imgUpload.PostedFile.ContentLength > 0)
        //{
        if (Request.QueryString[0].Equals("INSERT"))
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOAD/" + ViewState["fileName"].ToString()));
        }
        else
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
        lnkView.Visible = true;
        lnkView.Text = ViewState["fileName"].ToString();
        //}
    }
    #endregion

    protected void chkIsDrawing_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsDrawing.Checked == true)
        {
            chkIsDrawingFile.Visible = true;
        }
        else
        {
            chkIsDrawingFile.Visible = false;
            ViewState["fileName4"] = DBNull.Value;
        }
    }

    protected void chkPDIR_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPDIR.Checked == true)
        {
            PDIRFile.Visible = true;
        }
        else
        {
            PDIRFile.Visible = false;
            ViewState["fileName5"] = DBNull.Value;
        }
    }

    protected void chkamplingPlan_CheckedChanged(object sender, EventArgs e)
    {
        if (chksamplingPlan.Checked == true)
        {
            samplingPlanFile.Visible = true;
        }
        else
        {
            samplingPlanFile.Visible = false;
            ViewState["fileName6"] = DBNull.Value;
        }
    }

    protected void chkIsTrayLayout_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsTrayLayout.Checked == true)
        {
            TrayLayoutFile.Visible = true;
        }
        else
        {
            TrayLayoutFile.Visible = false;
            ViewState["fileName7"] = DBNull.Value;
        }
    }

    protected void chkIsMISC_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsMISC.Checked == true)
        {
            MISCFile.Visible = true;
        }
        else
        {
            MISCFile.Visible = false;
            ViewState["fileName8"] = DBNull.Value;
        }
    }

    protected void lnkDrawing_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkDrawing.Text != "")
                {
                    filePath = lnkDrawing.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkDrawing.Text != "")
                {
                    filePath = lnkDrawing.Text;
                }

                directory = "../../UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnkPDIR_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkPDIR.Text != "")
                {
                    filePath = lnkPDIR.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkPDIR.Text != "")
                {
                    filePath = lnkPDIR.Text;
                }

                directory = "../../UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnksamplingPlan_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnksamplingPlan.Text != "")
                {
                    filePath = lnksamplingPlan.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnksamplingPlan.Text != "")
                {
                    filePath = lnksamplingPlan.Text;
                }

                directory = "../../UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnkTrayLayout_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkTrayLayout.Text != "")
                {
                    filePath = lnkTrayLayout.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkTrayLayout.Text != "")
                {
                    filePath = lnkTrayLayout.Text;
                }

                directory = "../../UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnkMISC_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkMISC.Text != "")
                {
                    filePath = lnkMISC.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkMISC.Text != "")
                {
                    filePath = lnkMISC.Text;
                }

                directory = "../../UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkView.Text != "")
                {
                    filePath = lnkView.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkView.Text != "")
                {
                    filePath = lnkView.Text;
                }

                directory = "../../UpLoadPath/SupplierPO/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }

    #endregion Detail
}
