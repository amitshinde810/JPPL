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
using System.Net;

public partial class Masters_ADD_SupplierMaster : System.Web.UI.Page
{
    #region Declartion
    static int mlCode = 0;
    DirectoryInfo ObjSearchDir;
    static string right = "";
    int CValue;
    int SValue;
    string fileName = "";
    string fileName2 = "";
    string fileNameD = "";
    #endregion

    #region Events
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
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

            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='26'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadCountry();
                    LoadState();
                    LoadArea();
                    LoadCity();
                    LoadSupplierType();
                    LoadSuppCategory();
                    ChkActiveInd.Checked = true;
                    ViewState["fileName"] = fileName;
                    ViewState["fileName2"] = fileName2;
                    ViewState["fileNameD"] = fileNameD;
                    ViewState["mlCode"] = mlCode;
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
                    txtPartyName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Supplier Master", "PageLoad", ex.Message);
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
                sDirPath = Server.MapPath(@"~/UpLoadPath/SUPPILER/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/SUPPILER/" + ViewState["fileName"].ToString()));
            }
            else
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/SUPPILER/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
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

    #region Upload2
    protected void Upload2(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/SUPPILER/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/SUPPILER/" + ViewState["fileName2"].ToString()));
            }
            else
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/SUPPILER/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
            }
            lnkTModel.Visible = true;
            lnkTModel.Text = ViewState["fileName2"].ToString();
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
                directory = "../../UpLoadPath/SUPPILER/" + filePath;

            }
            else
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/SUPPILER/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }



            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;



        }
        catch (Exception ex)
        {
            CommonClasses.SendError("upplier Master Entry", "lnkupload_Click", ex.Message);
        }
    }

    protected void lnkuploadTModel_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/SUPPILER/" + filePath;
            }
            else
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/SUPPILER/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }

            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("upplier Master Entry", "lnkupload_Click", ex.Message);
        }
    }

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (chlLBTApplicable.Checked != false)
            {
                if (txtLBTNo.Text == "" || txtLBTNo.Text == "0")
                {
                    ShowMessage("#Avisos", "Enter GST NO.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (txtPartyName.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter Party Name", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtContactPerson.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter Contact Person Name ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtEmailId.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter Email Id ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtAddress.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter Address ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtPinCode.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter Pin Code ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtMobileNo.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter Mobile No. ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            else
            {
                if (txtMobileNo.Text.Trim() != "")
                {
                    // Code Comment :- //int MobNo = Convert.ToInt32(txtMobileNo.Text.Trim());
                    if (txtMobileNo.Text.Trim() == "0")
                    {
                        ShowMessage("#Avisos", "Enter valid Mobile no. ", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            }
            if (txtPANNO.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Enter PAN No. ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtPPassword.Text.Trim() != txtConPassword.Text.Trim())
            {
                PanelMsg.Visible = true;
                ShowMessage("#Avisos", "Please Enter Correct Password ", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "btnSubmit_Click", Ex.Message);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtPartyName.Text == "")
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
            CommonClasses.SendError("Supplier Master", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("PARTY_MASTER", "MODIFY", "P_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/Masters/VIEW/ViewSupplierMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "CancelRecord", ex.Message.ToString());
        }
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
            CommonClasses.SendError("Supplier Master", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        SValue = Convert.ToInt32(ddlState.SelectedValue);
        LoadCity();
    }
    #endregion

    #region Methods
    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID=" + Session["CompanyId"] + " order by COUNTRY_NAME");

                ddlCountry.DataSource = dt;
                ddlCountry.DataTextField = "COUNTRY_NAME";
                ddlCountry.DataValueField = "COUNTRY_CODE";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("------Country-------", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Customer Master", "LoadCountry", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadCountry", ex.Message);
        }
    }
    #endregion Load Country

    #region Load State
    private void LoadState()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + " and SM_COUNTRY_CODE= " + CValue + " order by SM_NAME");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "SM_NAME";
                ddlState.DataValueField = "SM_CODE";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("------State-------", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Customer Master", "LoadSate", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadState", ex.Message);
        }
    }
    #endregion Load State

    #region LoadCity
    private void LoadCity()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select CITY_CODE,CITY_NAME from CITY_MASTER where ES_DELETE=0 and CITY_CM_COMP_ID=" + Session["CompanyId"] + " and CITY_SM_CODE= " + SValue + " order by CITY_NAME");
                ddlCity.DataSource = dt;
                ddlCity.DataTextField = "CITY_NAME";
                ddlCity.DataValueField = "CITY_CODE";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("------City-------", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Customer Master", "LoadSate", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadCity", ex.Message);
        }
    }
    #endregion Load City

    #region LoadArea
    private void LoadArea()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select A_CODE,A_DESC from AREA_MASTER where A_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 order by A_DESC");

            ddlArea.DataSource = dt;
            ddlArea.DataTextField = "A_DESC";
            ddlArea.DataValueField = "A_CODE";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("Select..", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadArea", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadSupplierType
    private void LoadSupplierType()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select STM_CODE,STM_TYPE_CODE from SUPPLIER_TYPE_MASTER where STM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 order by STM_TYPE_CODE");

            ddlSupplierType.DataSource = dt;
            ddlSupplierType.DataTextField = "STM_TYPE_CODE";
            ddlSupplierType.DataValueField = "STM_CODE";
            ddlSupplierType.DataBind();
            ddlSupplierType.Items.Insert(0, new ListItem("Select..", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadSupplierType", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadSuppCategory
    private void LoadSuppCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select SCM_CODE,SCM_NAME from SUPPLIER_CATEGORY_MASTER where SCM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 order by SCM_NAME");

            ddlExiseType.DataSource = dt;
            ddlExiseType.DataTextField = "SCM_NAME";
            ddlExiseType.DataValueField = "SCM_CODE";
            ddlExiseType.DataBind();
            ddlExiseType.Items.Insert(0, new ListItem("Select Category..", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadSuppCategory", ex.Message);
        }
        finally
        {
        }
    }
    #endregion LoadSuppCategory


    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute(" SELECT * FROM PARTY_MASTER WHERE P_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + " AND P_CM_COMP_ID = " + (string)Session["CompanyId"] + " and P_TYPE=2 and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["P_CODE"]);
                string P_NAME = dt.Rows[0]["P_NAME"].ToString();
                txtPartyCode.Text = "PC-" + P_NAME.Substring(0, 1) + dt.Rows[0]["P_PARTY_CODE"].ToString();
                txtPartyName.Text = dt.Rows[0]["P_NAME"].ToString();
                ddlSupplierType.SelectedValue = dt.Rows[0]["P_STM_CODE"].ToString();
                txtContactPerson.Text = dt.Rows[0]["P_CONTACT"].ToString();
                txtAddress.Text = dt.Rows[0]["P_ADD1"].ToString();
                txtPhoneNo.Text = dt.Rows[0]["P_PHONE"].ToString();
                txtEmailId.Text = dt.Rows[0]["P_EMAIL"].ToString();
                txtTallyName.Text = dt.Rows[0]["P_TALLY"].ToString();
                txtPhoneNo.Text = dt.Rows[0]["P_PHONE"].ToString();
                txtVATTINNo.Text = dt.Rows[0]["P_VAT"].ToString();
                txtCSTTINNo.Text = dt.Rows[0]["P_CST"].ToString();
                txtFaxNo.Text = dt.Rows[0]["P_FAX"].ToString();
                txtMobileNo.Text = dt.Rows[0]["P_MOB"].ToString();
                txtPinCode.Text = dt.Rows[0]["P_PIN_CODE"].ToString();
                txtPANNO.Text = dt.Rows[0]["P_PAN"].ToString();
                txtCreditdays.Text = dt.Rows[0]["P_CREDITDAYS"].ToString();
                txtVendorCode.Text = dt.Rows[0]["P_VEND_CODE"].ToString();
                txtECCNo.Text = dt.Rows[0]["P_ECC_NO"].ToString();
                txtExiseDevision.Text = dt.Rows[0]["P_EXC_DIV"].ToString();
                txtExiseRange.Text = dt.Rows[0]["P_EXC_RANGE"].ToString();
                txtExiseCollectorate.Text = dt.Rows[0]["P_EXC_COLLECTORATE"].ToString();
                txtTDSTAX.Text = dt.Rows[0]["P_TDS"].ToString();
                txtLBTNo.Text = dt.Rows[0]["P_LBT_NO"].ToString();
                txtSerTaxRegNo.Text = dt.Rows[0]["P_SER_TAX_NO"].ToString();
                ddlCategory.Text = dt.Rows[0]["P_CATEGORY"].ToString();
                ddlArea.Text = dt.Rows[0]["P_A_CODE"].ToString();
                ddlExiseType.Text = dt.Rows[0]["P_E_CODE"].ToString();
                ddlSupplierType.Text = dt.Rows[0]["P_STM_CODE"].ToString();
                txtGSTNo.Text = dt.Rows[0]["P_GST_NO"].ToString();
                ddlCountry.SelectedValue = dt.Rows[0]["P_COUNTRY_CODE"].ToString();
                ddlCountry_SelectedIndexChanged(null, null);
                ddlState.SelectedValue = dt.Rows[0]["P_SM_CODE"].ToString();
                ddlState_SelectedIndexChanged(null, null);
                ddlCity.SelectedValue = dt.Rows[0]["P_CITY_CODE"].ToString();
                txtPUserName.Text = dt.Rows[0]["P_USER_NAME"].ToString();
                lnkupload.Text = dt.Rows[0]["P_COORDINATOR"].ToString();
                lnkTModel.Text = dt.Rows[0]["P_COORDINATOR_EMAIL"].ToString();
                if (dt.Rows[0]["P_PASSWORD"].ToString() == "")
                {

                }
                else
                {
                    txtPPassword.Attributes.Add("value", CommonClasses.DecriptText(dt.Rows[0]["P_PASSWORD"].ToString()));
                    txtConPassword.Attributes.Add("value", CommonClasses.DecriptText(dt.Rows[0]["P_PASSWORD"].ToString()));
                }
                if ((object)dt.Rows[0]["P_ACTIVE_IND"].ToString() != "" && Convert.ToBoolean(dt.Rows[0]["P_ACTIVE_IND"].ToString()) == true)
                {
                    ChkActiveInd.Checked = true;
                }
                else
                {
                    ChkActiveInd.Checked = false;
                }
                if ((object)dt.Rows[0]["P_INHOUSE_IND"].ToString() != "" && Convert.ToBoolean(dt.Rows[0]["P_INHOUSE_IND"].ToString()) == true)
                {
                    chkApproved.Checked = true;
                }
                else
                {
                    chkApproved.Checked = false;
                }
                if ((object)dt.Rows[0]["P_LBT_IND"].ToString() != "" && Convert.ToBoolean(dt.Rows[0]["P_LBT_IND"].ToString()) == true)
                {
                    chlLBTApplicable.Checked = true;
                }
                else
                {
                    chlLBTApplicable.Checked = false;
                }

                if (str == "VIEW")
                {
                    txtAddress.Enabled = false;
                    txtContactPerson.Enabled = false;
                    txtCreditdays.Enabled = false;
                    txtCSTTINNo.Enabled = false;
                    txtECCNo.Enabled = false;
                    txtEmailId.Enabled = false;
                    txtExiseCollectorate.Enabled = false;
                    txtExiseDevision.Enabled = false;
                    txtExiseRange.Enabled = false;
                    txtFaxNo.Enabled = false;
                    txtLBTNo.Enabled = false;
                    txtMobileNo.Enabled = false;
                    txtPANNO.Enabled = false;
                    txtPartyCode.Enabled = false;
                    txtPartyName.Enabled = false;
                    txtPhoneNo.Enabled = false;
                    txtPinCode.Enabled = false;
                    txtSerTaxRegNo.Enabled = false;
                    txtTallyName.Enabled = false;
                    txtTDSTAX.Enabled = false;
                    txtVATTINNo.Enabled = false;
                    txtVendorCode.Enabled = false;
                    ddlArea.Enabled = false;
                    ddlCategory.Enabled = false;
                    ddlCity.Enabled = false;
                    ddlCountry.Enabled = false;
                    ddlExiseType.Enabled = false;
                    ddlState.Enabled = false;
                    ddlSupplierType.Enabled = false;
                    ChkActiveInd.Enabled = false;
                    chlLBTApplicable.Enabled = false;
                    txtGSTNo.Enabled = false;
                    btnSubmit.Visible = false;
                    txtPPassword.Enabled = false;
                    txtConPassword.Enabled = false;
                    txtPUserName.Enabled = false;
                    chkApproved.Enabled = false;
                    FileUpload1.Enabled = false;
                    FileUpload2.Enabled = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("PARTY_MASTER", "MODIFY", "P_CODE", Convert.ToInt32(ViewState["mlCode"]));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetIP4Address
    string GetIP4Address()
    {
        string IP4Address = String.Empty;
        try
        {
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "GetIP4Address", ex.Message);
            return IP4Address;
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        string StrPartyName = txtPartyName.Text.Trim();
        string StrReplaceCustAddt = txtAddress.Text.Trim();
        string StrReplacePerson = txtContactPerson.Text.Trim();
        string StrReplacetallyname = txtTallyName.Text.Trim();
        string UserName = txtPUserName.Text.Trim();
        string Password = txtPPassword.Text.Trim();
        StrPartyName = StrPartyName.Replace("'", "''");
        StrReplaceCustAddt = StrReplaceCustAddt.Replace("'", "''");
        StrReplacePerson = StrReplacePerson.Replace("'", "''");
        StrReplacetallyname = StrReplacetallyname.Replace("'", "''");
        UserName.Replace("'", "''");
        Password.Replace("'", "''");

        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select P_CODE,P_NAME,P_PARTY_CODE FROM PARTY_MASTER WHERE P_NAME= lower('" + txtPartyName.Text.Trim().Replace("'", "\''") + "')and P_TYPE=2 and ES_DELETE='False'");

                if (dt.Rows.Count == 0)
                {
                    int CUST_NO = 0;
                    dt = CommonClasses.Execute("Select isnull(max(P_PARTY_CODE),0) as P_PARTY_CODE FROM PARTY_MASTER WHERE P_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                    if (dt.Rows.Count > 0)
                    {
                        CUST_NO = Convert.ToInt32(dt.Rows[0]["P_PARTY_CODE"]);
                        CUST_NO = CUST_NO + 1;
                    }

                    if (CommonClasses.Execute1("INSERT INTO PARTY_MASTER (P_STM_CODE,P_EXC_RANGE,P_PARTY_CODE,P_TYPE,P_CM_COMP_ID,P_NAME,P_CONTACT,P_VEND_CODE,P_ADD1,P_COUNTRY_CODE,P_CITY_CODE,P_SM_CODE,P_PIN_CODE,P_PHONE,P_MOB,P_CATEGORY,P_A_CODE,P_FAX,P_EMAIL,P_PAN,P_CST,P_VAT,P_SER_TAX_NO,P_ECC_NO,P_E_CODE,P_EXC_DIV,P_EXC_COLLECTORATE,P_TALLY,P_ACTIVE_IND,P_TDS,P_LBT_IND,P_LBT_NO,P_CREDITDAYS,P_GST_NO,P_USER_NAME,P_PASSWORD,P_INHOUSE_IND,P_COORDINATOR,P_COORDINATOR_EMAIL)VALUES('" + ddlSupplierType.SelectedValue + "','" + txtExiseRange.Text + "'," + CUST_NO + ",2," + Convert.ToInt32(Session["CompanyId"]) + ",'" + StrPartyName + "','" + StrReplacePerson + "','" + txtVendorCode.Text + "','" + StrReplaceCustAddt + "','" + ddlCountry.SelectedValue.ToString() + "','" + ddlCity.SelectedValue.ToString() + "','" + ddlState.SelectedValue.ToString() + "','" + txtPinCode.Text + "','" + txtPhoneNo.Text + "','" + txtMobileNo.Text + "','" + ddlCategory.SelectedValue.ToString() + "','" + ddlArea.SelectedValue.ToString() + "','" + txtFaxNo.Text + "','" + txtEmailId.Text + "','" + txtPANNO.Text + "','" + txtCSTTINNo.Text + "','" + txtVATTINNo.Text + "','" + txtSerTaxRegNo.Text + "','" + txtECCNo.Text + "','" + ddlExiseType.SelectedValue.ToString() + "','" + txtExiseDevision.Text + "','" + txtExiseCollectorate.Text + "','" + StrReplacetallyname + "','" + ChkActiveInd.Checked + "','" + Convert.ToDouble(txtTDSTAX.Text) + "','" + chlLBTApplicable.Checked + "','" + txtLBTNo.Text + "','" + txtCreditdays.Text + "','" + txtLBTNo.Text + "','" + UserName + "','" + CommonClasses.Encrypt(Password) + "','" + chkApproved.Checked + "','" + lnkupload.Text + "','" + lnkTModel.Text + "' )"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(P_CODE) from PARTY_MASTER");

                        #region file upload for tooling Photo
                        if (ViewState["fileName"].ToString().Trim() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/SUPPILER/");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\SUPPILER ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + lnkupload.Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath//SUPPILER/" + Code + "/" + lnkupload.Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion

                        #region FileUpload for 3D Model
                        if (ViewState["fileName2"].ToString().Trim() != "")
                        {
                            string sDirPath13 = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath13);

                            string sDirPath12 = Server.MapPath(@"~/UpLoadPath/SUPPILER/");
                            DirectoryInfo dir1 = new DirectoryInfo(sDirPath13);

                            dir1.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\SUPPILER ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + lnkTModel.Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/SUPPILER/" + Code + "/" + lnkTModel.Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion

                        // CommonClasses.Execute1("INSERT INTO USER_MASTER (UM_CM_ID,UM_USERNAME,UM_PASSWORD,UM_LEVEL,UM_LASTLOGIN_DATETIME,UM_IP_ADDRESS,IS_ACTIVE,UM_IS_ADMIN,UM_NAME,UM_EMAIL,P_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + UserName + "','" + CommonClasses.Encrypt(Password) + "','','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd hh:MM:ss tt") + "','" + GetIP4Address() + "','" + ChkActiveInd.Checked + "','0','" + UserName + "','','" + Code + "')");

                        CommonClasses.WriteLog("Supplier Master", "Save", "Supplier Master", txtPartyName.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSupplierMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtPartyName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PARTY_MASTER WHERE ES_DELETE='FALSE' AND P_CODE != '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND lower(P_NAME) = lower('" + txtPartyName.Text.Trim().Replace("'", "\''") + "')AND P_TYPE= 2 ");

                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PARTY_MASTER SET P_TYPE=2,P_NAME= '" + StrPartyName + "', P_CONTACT='" + StrReplacePerson + "',P_VEND_CODE='" + txtVendorCode.Text + "',P_ADD1='" + StrReplaceCustAddt + "',P_COUNTRY_CODE= '" + ddlCountry.SelectedValue.ToString() + "',P_CITY_CODE= '" + ddlCity.SelectedValue.ToString() + "',P_SM_CODE='" + ddlState.SelectedValue.ToString() + "',P_PIN_CODE='" + txtPinCode.Text + "',P_PHONE='" + txtPhoneNo.Text + "',P_MOB='" + txtMobileNo.Text + "',P_CATEGORY='" + ddlCategory.SelectedValue.ToString() + "',P_A_CODE='" + ddlArea.SelectedValue.ToString() + "',P_FAX='" + txtFaxNo.Text + "',P_EMAIL='" + txtEmailId.Text + "',P_PAN='" + txtPANNO.Text + "',P_CST='" + txtCSTTINNo.Text + "',P_VAT= '" + txtVATTINNo.Text + "',P_SER_TAX_NO='" + txtSerTaxRegNo.Text + "',P_ECC_NO= '" + txtECCNo.Text + "',P_E_CODE='" + ddlExiseType.SelectedValue.ToString() + "',P_EXC_DIV='" + txtExiseDevision.Text + "',P_EXC_COLLECTORATE='" + txtExiseCollectorate.Text + "',P_TALLY='" + StrReplacetallyname + "',P_ACTIVE_IND='" + ChkActiveInd.Checked + "',P_TDS='" + txtTDSTAX.Text + "',P_LBT_IND='" + chlLBTApplicable.Checked + "',P_LBT_NO='" + txtLBTNo.Text + "',P_STM_CODE=" + ddlSupplierType.SelectedValue + ",P_GST_NO='" + txtLBTNo.Text + "',P_USER_NAME='" + UserName + "',P_PASSWORD='" + CommonClasses.Encrypt(Password) + "'  , P_INHOUSE_IND='" + chkApproved.Checked + "' ,P_COORDINATOR='" + lnkupload.Text + "',P_COORDINATOR_EMAIL='" + lnkTModel.Text + "',P_CREDITDAYS='" + txtCreditdays.Text + "' WHERE P_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' "))
                    {
                        CommonClasses.Execute1("UPDATE dbo.USER_MASTER SET UM_USERNAME='" + UserName + "',UM_PASSWORD='" + CommonClasses.Encrypt(Password) + "',UM_LEVEL='',UM_LASTLOGIN_DATETIME='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd hh:MM:ss tt") + "',UM_IP_ADDRESS='" + GetIP4Address() + "',IS_ACTIVE='" + ChkActiveInd.Checked + "',UM_IS_ADMIN='0',UM_NAME='" + UserName + "',UM_EMAIL='' WHERE P_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                        CommonClasses.RemoveModifyLock("PARTY_MASTER", "MODIFY", "P_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Supplier Master", "Update", "Supplier Master", txtPartyName.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSupplierMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtPartyName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Customer Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion

    protected void chlLBTApplicable_CheckedChanged(object sender, EventArgs e)
    {
        if (chlLBTApplicable.Checked == true)
        {
        }
        else
        {
            txtLBTNo.Text = "";
        }
    }

    protected void txtVatTinNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVATTINNo.Text != "")
            {
                char[] ch = txtVATTINNo.Text.ToCharArray();
                ch[ch.Length - 1] = 'C';
                txtCSTTINNo.Text = new string(ch);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "txtVatTinNo_TextChanged", ex.Message);
        }
    }

    protected void txtPartyName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtPartyName.Text != "")
            {
                txtTallyName.Text = txtPartyName.Text.Trim();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "txtVatTinNo_TextChanged", ex.Message);
        }
    }

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

    #region txtTDSTAX_TextChanged
    protected void txtTDSTAX_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTDSTAX.Text != "")
            {
                string totalStr = DecimalMasking(txtTDSTAX.Text);
                double tdsper = Math.Round(Convert.ToDouble(totalStr), 2);
                txtTDSTAX.Text = string.Format("{0:0.00}", tdsper);
                if (tdsper <= 100)
                {
                }
                else
                {
                    txtTDSTAX.Text = "0.00";
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "txtTDSTAX_TextChanged", ex.Message);
        }
    }
    #endregion

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        CValue = Convert.ToInt32(ddlCountry.SelectedValue);
        LoadState();
    }
}