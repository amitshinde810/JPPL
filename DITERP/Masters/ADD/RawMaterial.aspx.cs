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
using System.IO;

public partial class Masters_ADD_RawMaterial : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int File_Code = 0;
    static string right = "";
    RawMaterial_BL BL_RawMaterial = null;

    public static string imgEmpImage;
    public static string ImageUrl;
    DataTable dt = new DataTable();
    DataRow dr;
    DirectoryInfo ObjSearchDir;
    string c_type = "";
    string fileName = "";
    string fileName2 = "";
    private static readonly string[] _validExtensions = { ".jpeg", ".jpg", ".bmp", ".gif", ".png", ".tif", ".pdf", ".txt" };
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");

        home.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
            }
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='11'");
                right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadStockUOM();
                    LoadItemCategory();
                    LoadItemSubCategory();
                    LoadTariffHeading();
                    LoadSAC();
                    LoadTaxHeadSales();
                    LoadTaxHeadPurchase();
                    LoadItemSubCategory();
                    ddlItemCategory.Enabled = true;
                    ViewState["mlCode"] = mlCode;
                    ViewState["fileName"] = "";
                    ViewState["fileName2"] = "";
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_RawMaterial = new RawMaterial_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_RawMaterial = new RawMaterial_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtItemCode.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Item Master", "PageLoad", ex.Message);
                }
            }
            if (IsPostBack && FileUpload2.PostedFile != null)
            {
                if (FileUpload2.PostedFile.FileName.Length > 0)
                {
                    fileName = FileUpload2.PostedFile.FileName;
                    ViewState["fileName"] = fileName;
                    Upload(null, null);
                }
            }
            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                if (FileUpload1.PostedFile.FileName.Length > 0)
                {
                    fileName2 = FileUpload1.PostedFile.FileName;
                    ViewState["fileName2"] = fileName2;
                    Upload2(null, null);
                }
            }
        }
    }
    #endregion


    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhotoF");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhoto/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/ItemMaster/PartPhotoF/" + ViewState["fileName"].ToString()));
            }
            else
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/ItemMaster/PartPhoto/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
            }
            lnkupload.Visible = true;
            lnkupload.Text = ViewState["fileName"].ToString();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    #endregion
    protected void lnkupload_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/ItemMaster/PartPhotoF/" + filePath;

            }
            else
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/ItemMaster/PartPhoto/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            string fileExtension = System.IO.Path.GetExtension(filePath);
            if (_validExtensions.Contains(fileExtension.ToLower().ToString()))
            {
                ModalPopupExtenderDovView.Show();
                myframe.Attributes["src"] = directory;
            }/*call this method when File is not image*/
            else
            {
                ModalPopupExtenderDovView.Show();
                myframe.Attributes["src"] = directory;
                ModalPopupExtenderDovView.Hide();
            }
            //ModalPopupExtenderDovView.Show();
            //myframe.Attributes["src"] = directory;

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);
        }
    }


    #region Upload2
    protected void Upload2(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhotoF/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhoto/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/ItemMaster/PartPhotoF/" + ViewState["fileName2"].ToString()));
            }
            else
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/ItemMaster/PartPhoto/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
            }
            lnkupload2.Visible = true;
            lnkupload2.Text = ViewState["fileName2"].ToString();
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    #endregion

    protected void lnkupload2_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload2.Text;
                directory = "../../UpLoadPath/ItemMaster/PartPhotoF/" + filePath;

            }
            else
            {
                filePath = lnkupload2.Text;
                directory = "../../UpLoadPath/ItemMaster/PartPhoto/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            string fileExtension = System.IO.Path.GetExtension(filePath);
            if (_validExtensions.Contains(fileExtension.ToLower().ToString()))
            {
                ModalPopupExtenderDovView.Show();
                myframe.Attributes["src"] = directory;
            }/*call this method when File is not image*/
            else
            {
                ModalPopupExtenderDovView.Show();
                myframe.Attributes["src"] = directory;
                ModalPopupExtenderDovView.Hide();
            }
            //ModalPopupExtenderDovView.Show();
            //myframe.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);
        }
    }

    #region LoadTaxHeadSales
    private void LoadTaxHeadSales()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY TALLY_NAME");
            ddlTallyAccS.DataSource = dt;
            ddlTallyAccS.DataTextField = "TALLY_NAME";
            ddlTallyAccS.DataValueField = "TALLY_CODE";
            ddlTallyAccS.DataBind();
            ddlTallyAccS.Items.Insert(0, new ListItem("----Select Tax----", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "Load Tax Head", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadTaxHeadPurchase
    private void LoadTaxHeadPurchase()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where TALLY_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY TALLY_NAME");
            ddltallyAccP.DataSource = dt;
            ddltallyAccP.DataTextField = "TALLY_NAME";
            ddltallyAccP.DataValueField = "TALLY_CODE";
            ddltallyAccP.DataBind();
            ddltallyAccP.Items.Insert(0, new ListItem("----Select Tax----", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "Load Tax Head", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadStockUOM
    private void LoadStockUOM()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where I_UOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_UOM_NAME");

            ddlStockUOM.DataSource = dt;
            ddlStockUOM.DataTextField = "I_UOM_NAME";
            ddlStockUOM.DataValueField = "I_UOM_CODE";
            ddlStockUOM.DataBind();
            ddlStockUOM.Items.Insert(0, new ListItem("Select..", "0"));
            ddlWeightUOM.DataSource = dt;
            ddlWeightUOM.DataTextField = "I_UOM_NAME";
            ddlWeightUOM.DataValueField = "I_UOM_CODE";
            ddlWeightUOM.DataBind();
            ddlWeightUOM.Items.Insert(0, new ListItem("Select..", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadStockUOM", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadTariffHeading
    private void LoadTariffHeading()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where E_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND  E_TALLY_GST_EXCISE=1 AND E_EX_TYPE=0 and ES_DELETE=0 ORDER BY E_TARIFF_NO");

            ddlTariffHeading.DataSource = dt;
            ddlTariffHeading.DataTextField = "E_TARIFF_NO";
            ddlTariffHeading.DataValueField = "E_CODE";
            ddlTariffHeading.DataBind();
            ddlTariffHeading.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadTariffHeading", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadSAC
    private void LoadSAC()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select E_CODE,E_TARIFF_NO from EXCISE_TARIFF_MASTER where E_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND E_EX_TYPE=1 AND E_TALLY_GST_EXCISE=1  ORDER BY E_TARIFF_NO");

            ddlSAC.DataSource = dt;
            ddlSAC.DataTextField = "E_TARIFF_NO";
            ddlSAC.DataValueField = "E_CODE";
            ddlSAC.DataBind();
            ddlSAC.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadTariffHeading", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadItemCategory
    private void LoadItemCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select I_CAT_CODE,I_CAT_NAME from ITEM_CATEGORY_MASTER where I_CAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_CAT_NAME");
            ddlItemCategory.DataSource = dt;
            ddlItemCategory.DataTextField = "I_CAT_NAME";
            ddlItemCategory.DataValueField = "I_CAT_CODE";
            ddlItemCategory.DataBind();
            ddlItemCategory.Items.Insert(0, new ListItem("Select Category", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadStockUOM", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region ddlItemCategory_SelectedIndexChanged
    protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadItemSubCategory();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "ddlItemCategory_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region LoadItemSubCategory
    private void LoadItemSubCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            if (ddlItemCategory.SelectedIndex == 0)
            {
                dt = CommonClasses.Execute("select SCAT_CODE,SCAT_DESC from ITEM_SUBCATEGORY_MASTER where SCAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY SCAT_DESC");
                ddlSubCategory.Enabled = false;
            }
            else
            {
                dt = CommonClasses.Execute("select SCAT_CODE,SCAT_DESC from ITEM_SUBCATEGORY_MASTER where SCAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and SCAT_CAT_CODE='" + ddlItemCategory.SelectedValue + "' and ES_DELETE=0 ORDER BY SCAT_DESC");
                ddlSubCategory.Enabled = true;
            }
            ddlSubCategory.DataSource = dt;
            ddlSubCategory.DataTextField = "SCAT_DESC";
            ddlSubCategory.DataValueField = "SCAT_CODE";
            ddlSubCategory.DataBind();
            ddlSubCategory.Items.Insert(0, new ListItem("Select Sub Category", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadItemSubCategory", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            try
            {
                BL_RawMaterial = new RawMaterial_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));
                DataTable dt = new DataTable();
                BL_RawMaterial.I_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);

                BL_RawMaterial.GetInfo();
                GetValues(str);
                if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("Item Master", "ViewRec", Ex.Message);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Item Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            ViewState["mlCode"] = BL_RawMaterial.I_CODE;
            ddlItemCategory.SelectedValue = BL_RawMaterial.I_CAT_CODE.ToString();
            ddlSubCategory.Enabled = true;
            ddlSAC.SelectedValue = BL_RawMaterial.I_SCAT_CODE.ToString();
            txtItemCode.Text = BL_RawMaterial.I_CODENO;
            txtDrawingNumnber.Text = BL_RawMaterial.I_DRAW_NO;
            txtItemName.Text = BL_RawMaterial.I_NAME;
            txtSpecifications.Text = BL_RawMaterial.I_SPECIFICATION;
            ddlTariffHeading.SelectedValue = BL_RawMaterial.I_E_CODE.ToString();
            ddltallyAccP.SelectedValue = BL_RawMaterial.I_ACCOUNT_PURCHASE.ToString();
            ddlTallyAccS.SelectedValue = BL_RawMaterial.I_ACCOUNT_SALES.ToString();
            txtCoastingHead.Text = BL_RawMaterial.I_COSTING_HEAD;
            ddlStockUOM.SelectedValue = BL_RawMaterial.I_UOM_CODE.ToString();
            ddlInventoryCategory.SelectedIndex = Convert.ToInt32(BL_RawMaterial.I_INV_CAT);
            txtUniWeight.Text = BL_RawMaterial.I_UWEIGHT.ToString();
            txtMaximumLevel.Text = BL_RawMaterial.I_MAX_LEVEL.ToString();
            txtMinimumLevel.Text = BL_RawMaterial.I_MIN_LEVEL.ToString();
            txtReOrderLevel.Text = BL_RawMaterial.I_REORDER_LEVEL.ToString();
            txtOpeningBalance.Text = BL_RawMaterial.I_OP_BAL.ToString();
            txtOpeningBalanceRate.Text = BL_RawMaterial.I_OP_BAL_RATE.ToString();
            txtStoreLocation.Text = BL_RawMaterial.I_STORE_LOC.ToString();
            txtInventoryRate.Text = BL_RawMaterial.I_INV_RATE.ToString();
            txtCurrentBal.Text = BL_RawMaterial.I_CURRENT_BAL.ToString();
            txtLastRecdDate.Text = Convert.ToDateTime(BL_RawMaterial.I_RECEIPT_DATE).ToString("dd MMM yyyy");
            txtLastIssueDate.Text = Convert.ToDateTime(BL_RawMaterial.I_ISSUE_DATE).ToString("dd MMM yyyy"); ;
            txtDensity.Text = BL_RawMaterial.I_DENSITY.ToString();
            txtPigment.Text = BL_RawMaterial.I_PIGMENT.ToString();
            txtSolids.Text = BL_RawMaterial.I_SOLIDS.ToString();
            txtVolatile.Text = BL_RawMaterial.I_VOLATILE.ToString();
            ddlWeightUOM.SelectedValue = BL_RawMaterial.I_WEIGHT_UOM.ToString();
            ChkActiveInd.Checked = BL_RawMaterial.I_ACTIVE_IND;
            chkDevelopment.Checked = BL_RawMaterial.I_DEVELOMENT;
            txtTargetWeight.Text = BL_RawMaterial.I_TARGET_WEIGHT.ToString();
            txtStdWeight.Text = BL_RawMaterial.I_STD_WEIGHT.ToString();
            txtDispatchBal.Text = BL_RawMaterial.I_DISPATCH_BAL.ToString();
            txtStandardProduction.Text = BL_RawMaterial.I_STANDARD_PRODUCTION.ToString();
            txtAs_Cast_Weight.Text = BL_RawMaterial.I_AS_CAST_WEIGHT.ToString();
            lnkupload.Text = BL_RawMaterial.I_DRAWING_PATH.ToString();
            lnkupload2.Text = BL_RawMaterial.I_PHOTO_PATH.ToString();
            if (Convert.ToBoolean(BL_RawMaterial.I_ACTIVE_IND) == true)
            {
                ChkActiveInd.Checked = true;
            }
            else
            {
                ChkActiveInd.Checked = false;
            }

            if (str == "VIEW")
            {
                ddlSubCategory.Enabled = false;
                ddlItemCategory.Enabled = false;
                txtItemCode.Enabled = false;
                txtDrawingNumnber.Enabled = false;
                txtItemName.Enabled = false;
                ddltallyAccP.Enabled = false;
                ddlTallyAccS.Enabled = false;
                txtCoastingHead.Enabled = false;
                ddlTariffHeading.Enabled = false;
                ddlStockUOM.Enabled = false;
                ddlInventoryCategory.Enabled = false;
                ChkActiveInd.Enabled = false;
                txtMaximumLevel.Enabled = false;
                txtMinimumLevel.Enabled = false;
                txtReOrderLevel.Enabled = false;
                txtOpeningBalance.Enabled = false;
                txtOpeningBalanceRate.Enabled = false;
                txtInventoryRate.Enabled = false;
                txtCurrentBal.Enabled = false;
                txtLastIssueDate.Enabled = false;
                txtLastRecdDate.Enabled = false;
                txtStoreLocation.Enabled = false;
                txtUniWeight.Enabled = false;
                btnSubmit.Visible = false;
                txtDensity.Enabled = false;
                txtPigment.Enabled = false;
                txtSolids.Enabled = false;
                txtVolatile.Enabled = false;
                txtSpecifications.Enabled = false;
                ddlSAC.Enabled = false;
                ddlWeightUOM.Enabled = false;
                chkDevelopment.Enabled = false;
                txtTargetWeight.Enabled = false;
                FileUpload1.Enabled = false;
                FileUpload2.Enabled = false;
            }
            else if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            string StrReplaceCodeno = txtItemCode.Text.Trim();
            string StrReplaceName = txtItemName.Text.Trim();
            string StrReplaceSpecify = txtSpecifications.Text.Trim();
            string StrReplaceCostHeading = txtCoastingHead.Text.Trim();
            StrReplaceCodeno = StrReplaceCodeno.Replace("'", "''");
            StrReplaceName = StrReplaceName.Replace("'", "''");
            StrReplaceSpecify = StrReplaceSpecify.Replace("'", "''");
            StrReplaceCostHeading = StrReplaceCostHeading.Replace("'", "''");
            BL_RawMaterial.I_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_RawMaterial.I_CAT_CODE = Convert.ToInt32(ddlItemCategory.SelectedValue);
            BL_RawMaterial.I_SCAT_CODE = Convert.ToInt32(ddlSAC.SelectedValue);
            BL_RawMaterial.I_CODENO = StrReplaceCodeno;
            BL_RawMaterial.I_DRAW_NO = txtDrawingNumnber.Text.Trim().Replace("'", "''");
            BL_RawMaterial.I_DRAWING_PATH = lnkupload.Text;
            BL_RawMaterial.I_PHOTO_PATH = lnkupload2.Text;
            BL_RawMaterial.I_NAME = StrReplaceName;
            BL_RawMaterial.I_SPECIFICATION = StrReplaceSpecify;
            BL_RawMaterial.I_E_CODE = Convert.ToInt32(ddlTariffHeading.SelectedValue);
            BL_RawMaterial.I_ACCOUNT_SALES = Convert.ToInt32(ddlTallyAccS.SelectedValue);
            BL_RawMaterial.I_ACCOUNT_PURCHASE = Convert.ToInt32(ddltallyAccP.SelectedValue);
            BL_RawMaterial.I_UOM_CODE = Convert.ToInt32(ddlStockUOM.SelectedValue);
            BL_RawMaterial.I_INV_CAT = Convert.ToInt32(ddlInventoryCategory.SelectedIndex).ToString();
            BL_RawMaterial.I_MAX_LEVEL = Convert.ToDouble(txtMaximumLevel.Text == "" ? "0" : txtMaximumLevel.Text);
            BL_RawMaterial.I_MIN_LEVEL = Convert.ToDouble(txtMinimumLevel.Text == "" ? "0" : txtMinimumLevel.Text);
            BL_RawMaterial.I_REORDER_LEVEL = Convert.ToDouble(txtReOrderLevel.Text == "" ? "0" : txtReOrderLevel.Text);
            BL_RawMaterial.I_OP_BAL = Convert.ToDouble(txtOpeningBalance.Text == "" ? "0" : txtOpeningBalance.Text);
            BL_RawMaterial.I_OP_BAL_RATE = Convert.ToDouble(txtOpeningBalanceRate.Text == "" ? "0" : txtOpeningBalanceRate.Text);
            BL_RawMaterial.I_STORE_LOC = txtStoreLocation.Text.Trim().Replace("'", "''");
            BL_RawMaterial.I_INV_RATE = Convert.ToDouble(txtInventoryRate.Text == "" ? "0" : txtInventoryRate.Text);
            string receiptDate = DateTime.Now.ToString();
            BL_RawMaterial.I_RECEIPT_DATE = Convert.ToDateTime(txtLastRecdDate.Text == "" ? Convert.ToDateTime(receiptDate).ToString("dd/MMM/yyyy") : txtLastRecdDate.Text);
            BL_RawMaterial.I_ISSUE_DATE = Convert.ToDateTime(txtLastIssueDate.Text == "" ? Convert.ToDateTime(receiptDate).ToString("dd/MMM/yyyy") : txtLastIssueDate.Text);
            BL_RawMaterial.I_CURRENT_BAL = Convert.ToDouble(txtCurrentBal.Text == "" ? "0" : txtCurrentBal.Text);
            BL_RawMaterial.I_ACTIVE_IND = Convert.ToBoolean(ChkActiveInd.Checked);
            BL_RawMaterial.I_UWEIGHT = Convert.ToDouble(txtUniWeight.Text == "" ? "0" : txtUniWeight.Text);
            BL_RawMaterial.I_COSTING_HEAD = StrReplaceCostHeading;
            BL_RawMaterial.I_INV_CAT = Convert.ToInt32(ddlInventoryCategory.SelectedValue).ToString();
            BL_RawMaterial.I_OP_BAL_RATE = Convert.ToDouble(txtOpeningBalanceRate.Text == "" ? "0" : txtOpeningBalanceRate.Text);
            BL_RawMaterial.I_DENSITY = Math.Round(Convert.ToDouble(txtDensity.Text == "" ? "0" : txtDensity.Text.ToString()), 2);
            BL_RawMaterial.I_PIGMENT = Math.Round(Convert.ToDouble(txtPigment.Text == "" ? "0" : txtPigment.Text.ToString()), 2);
            BL_RawMaterial.I_SOLIDS = Math.Round(Convert.ToDouble(txtSolids.Text == "" ? "0" : txtSolids.Text.ToString()), 2);
            BL_RawMaterial.I_VOLATILE = Math.Round(Convert.ToDouble(txtVolatile.Text == "" ? "0" : txtVolatile.Text.ToString()), 2);
            BL_RawMaterial.I_WEIGHT_UOM = Convert.ToInt32(ddlWeightUOM.SelectedValue);
            BL_RawMaterial.I_DEVELOMENT = Convert.ToBoolean(chkDevelopment.Checked);
            BL_RawMaterial.I_TARGET_WEIGHT = Convert.ToDouble(txtTargetWeight.Text == "" ? "0" : txtTargetWeight.Text);
            BL_RawMaterial.I_STD_WEIGHT = Convert.ToDouble(txtStdWeight.Text == "" ? "0" : txtStdWeight.Text);
            BL_RawMaterial.I_DISPATCH_BAL = Convert.ToDouble(txtDispatchBal.Text == "" ? "0" : txtDispatchBal.Text);
            BL_RawMaterial.I_STANDARD_PRODUCTION = Convert.ToDouble(txtStandardProduction.Text == "" ? "0" : txtStandardProduction.Text);
            BL_RawMaterial.I_AS_CAST_WEIGHT = Convert.ToDouble(txtAs_Cast_Weight.Text == "" ? "0" : txtAs_Cast_Weight.Text);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_RawMaterial = new RawMaterial_BL();
                if (Setvalues())
                {
                    if (BL_RawMaterial.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(I_CODE) from ITEM_MASTER");
                        #region file upload for Drawing
                        if (ViewState["fileName"].ToString().Trim() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhoto/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhotoF/");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\ItemMaster\\PartPhotoF ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhotoF/" + lnkupload.Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath//ItemMaster/PartPhoto/" + Code + "/" + lnkupload.Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion


                        #region file upload for Part Photo
                        if (ViewState["fileName2"].ToString().Trim() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhoto/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhotoF/");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\ItemMaster\\PartPhotoF ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/ItemMaster/PartPhotoF/" + lnkupload2.Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath//ItemMaster/PartPhoto/" + Code + "/" + lnkupload2.Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion

                        CommonClasses.WriteLog("item Master", "Save", "item Master", BL_RawMaterial.I_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + c_type + "", false);
                    }
                    else
                    {
                        if (BL_RawMaterial.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_RawMaterial.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        ddlItemCategory.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_RawMaterial = new RawMaterial_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));
                if (Setvalues())
                {
                    if (BL_RawMaterial.Update())
                    {
                        CommonClasses.RemoveModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("item Master", "Update", "item Master", BL_RawMaterial.I_NAME, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + c_type + "", false);
                    }
                    else
                    {
                        if (BL_RawMaterial.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_RawMaterial.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            BL_RawMaterial.Msg = "";
                        }
                        ddlItemCategory.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
                SaveRec();
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Item Master", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
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
            CommonClasses.SendError("Item Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtItemCode.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtItemName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlItemCategory.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlSubCategory.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlStockUOM.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlInventoryCategory.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddltallyAccP.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlTallyAccS.SelectedIndex <= 0)
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
            CommonClasses.SendError("Item Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("ITEM_MASTER", "MODIFY", "I_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Masters/VIEW/ViewRawMaterialMaster.aspx?c_name=" + c_type + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "btnCancel_Click", Ex.Message);
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

    #region txtMinimumLevel_TextChanged
    protected void txtMinimumLevel_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtMinimumLevel.Text);

        txtMinimumLevel.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    #endregion

    #region txtReOrderLevel_TextChanged
    protected void txtReOrderLevel_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtReOrderLevel.Text);
        txtReOrderLevel.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    #endregion

    #region txtInventoryRate_TextChanged
    protected void txtInventoryRate_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtInventoryRate.Text);
        txtInventoryRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    #endregion

    #region txtMaximumLevel_TextChanged
    protected void txtMaximumLevel_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtMaximumLevel.Text);
        txtMaximumLevel.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    #endregion

    #region txtUniWeight_TextChanged
    protected void txtUniWeight_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtUniWeight.Text);
        txtUniWeight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    #endregion

    #region txtTargetWeight_TextChanged
    protected void txtTargetWeight_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtTargetWeight.Text);
        txtTargetWeight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
    #endregion

    #region txtItemCode_TextChanged
    protected void txtItemCode_TextChanged(object sender, EventArgs e)
    {
        txtDrawingNumnber.Text = txtItemCode.Text;
    }
    #endregion
}