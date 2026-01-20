using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class Admin_Add_CompanyInfo : System.Web.UI.Page
{
    CompanyMaster_BL CompanyMaster_BL = null;
    static int mlCode = 0;
    static string right = "";
    DateTime d = new DateTime();

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='1'");
                        right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                        if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                        {
                            LoadCountry();
                            CompanyMaster_BL = new CompanyMaster_BL();
                            mlCode = Convert.ToInt32(Session["CompanyCode"]);
                            ViewRec("MOD");
                        }
                        else
                        {
                            Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Company Master", "PageLoad", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "PageLoad", ex.Message);
        }
    }
    #endregion Page_Load

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            CompanyMaster_BL = new CompanyMaster_BL(mlCode);
            DataTable dt = new DataTable();
            CompanyMaster_BL.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Company Master", "ViewRec", Ex.Message);
        }
    }
    #endregion ViewRec

    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();
        try
        {
            CompanyMaster_BL = new CompanyMaster_BL();
            dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID=" + Session["CompanyId"] + "");
            ddlCountry.DataSource = dt;
            ddlCountry.DataTextField = "COUNTRY_NAME";
            ddlCountry.DataValueField = "COUNTRY_CODE";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("------Country-------", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "LoadCountry", ex.Message);
        }
        finally
        {
            dt.Dispose();
        }
    }
    #endregion LoadCountry

    #region Load State
    private void LoadState()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                CompanyMaster_BL = new CompanyMaster_BL();
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + Session["CompanyId"] + " and SM_COUNTRY_CODE=" + ddlCountry.SelectedValue + "");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "SM_NAME";
                ddlState.DataValueField = "SM_CODE";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("------State-------", "0"));
                ddlState.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("Company Master", "LoadState", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "LoadState", ex.Message);
        }
    }
    #endregion Load State

    #region LoadCity
    private void LoadCity()
    {
        DataTable dt = new DataTable();
        try
        {
            CompanyMaster_BL = new CompanyMaster_BL();
            dt = CommonClasses.Execute("select CITY_CODE,CITY_NAME from CITY_MASTER where ES_DELETE=0 and CITY_CM_COMP_ID=" + Session["CompanyId"] + " and CITY_COUNTRY_CODE=" + ddlCountry.SelectedValue + " and CITY_SM_CODE=" + ddlState.SelectedValue + "");
            ddlCity.DataSource = dt;
            ddlCity.DataTextField = "CITY_NAME";
            ddlCity.DataValueField = "CITY_CODE";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("------City-------", "0"));
            ddlCity.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "LoadCity", ex.Message);
        }
        finally
        {
            dt.Dispose();
        }
    }
    #endregion LoadCity

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    }
    #endregion btnSubmit_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            CompanyMaster_BL = new CompanyMaster_BL(mlCode);
            if (Setvalues())
            {
                if (cbActiveIndex.Checked == false)
                {
                    ShowMessage("#Avisos", "Please Check Active Checkbox", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                if (CompanyMaster_BL.Update())
                {
                    CommonClasses.RemoveModifyLock("COMPANY_MASTER", "CM_MODIFY_FLAG",Session["CompanyId"].ToString(), mlCode);
                    CommonClasses.WriteLog("Company Information", "Update", "Company Information", CompanyMaster_BL.CM_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Admin/Default.aspx", false);
                }
                else
                {
                    if (CompanyMaster_BL.Msg != "")
                    {
                        ShowMessage("#Avisos", CompanyMaster_BL.Msg, CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        CompanyMaster_BL.Msg = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        d = (Convert.ToDateTime("01/Jan/1930"));
        bool res = false;
        try
        {
            CompanyMaster_BL.CM_CODE = Convert.ToInt32(Session["CompanyCode"]); ;
            CompanyMaster_BL.CM_ID = Convert.ToInt32(Session["CompanyId"]);
            CompanyMaster_BL.CM_NAME = Regex.Replace(txtCompanyName.Text.ToUpper(), "'", "`");
            CompanyMaster_BL.CM_ADDRESS1 = Regex.Replace(txtAddressLine1.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_ADDRESS2 = Regex.Replace(txtAddressLine2.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_ADDRESS3 = "";
            CompanyMaster_BL.CM_CITY = Convert.ToInt32(ddlCity.SelectedValue.ToString());
            CompanyMaster_BL.CM_STATE = Convert.ToInt32(ddlState.SelectedValue.ToString());
            CompanyMaster_BL.CM_COUNTRY = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
            CompanyMaster_BL.CM_OWNER = Regex.Replace(txtAuthSign.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_PHONENO1 = Regex.Replace(txtPhoneNo1.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_PHONENO2 = Regex.Replace(txtPhoneNumber2.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_PHONENO3 = Regex.Replace(txtPhoneNumber3.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_FAXNO = Regex.Replace(txtFaxNo.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_EMAILID = Regex.Replace(txtEmailId.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_WEBSITE = Regex.Replace(txtWebsite.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_SURCHARGE_NO = "";
            CompanyMaster_BL.CM_VAT_TIN_NO = Regex.Replace(txtVatTinNo.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_CST_NO = Regex.Replace(txtCstNo.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_SERVICE_TAX_NO = Regex.Replace(txtServiceTax.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_PAN_NO = Regex.Replace(txtPanNo.Text.Trim(), "'", "`");
            CompanyMaster_BL.CM_COMMODITY_NO = "";
            CompanyMaster_BL.CM_ACTIVE_IND = cbActiveIndex.Checked;
            CompanyMaster_BL.CM_OPENING_DATE = Convert.ToDateTime(txtOpeningDate.Text.Trim());
            CompanyMaster_BL.CM_CLOSING_DATE = Convert.ToDateTime(txtClosingDate.Text.Trim());
            CompanyMaster_BL.CM_REGD_NO = txtReghNo.Text.Trim();
            CompanyMaster_BL.CM_ECC_NO = txtEccNo.Text.Trim();
            
            CompanyMaster_BL.CM_CIN_NO = txtCinNo.Text.Trim();

            if (txtVatWef.Text == "")
            {
                CompanyMaster_BL.CM_VAT_WEF = d;
            }
            else
            {
                CompanyMaster_BL.CM_VAT_WEF = Convert.ToDateTime(txtVatWef.Text.Trim());
            }
            if (txtCstWef.Text == "")
            {
                CompanyMaster_BL.CM_CST_WEF = d;
            }
            else
            {
                CompanyMaster_BL.CM_CST_WEF = Convert.ToDateTime(txtCstWef.Text.Trim());
            }
            CompanyMaster_BL.CM_ISO_NUMBER = txtISONumber.Text.Trim();
            CompanyMaster_BL.CM_EXP_LICEN_NO = txtExpLicenNo.Text.Trim();
            CompanyMaster_BL.CM_EXP_PERMISSOIN_NO = txtExpPermisiionNo.Text.Trim();
            CompanyMaster_BL.CM_EXCISE_RANGE = txtExciseRange.Text.Trim();
            CompanyMaster_BL.CM_EXCISE_DIVISION = txtExciseDevision.Text.Trim();
            CompanyMaster_BL.CM_COMMISONERATE = txtCommisionerate.Text.Trim();
            CompanyMaster_BL.CM_EXC_SUPRE_DETAILS = txtExcSupreDetail.Text.Trim();
            CompanyMaster_BL.CM_BANK_NAME = txtBankersName.Text.Trim();
            CompanyMaster_BL.CM_BANK_ADDRESS = txtBranchAddress.Text.Trim();
            CompanyMaster_BL.CM_BANK_ACC_NO = txtAccountNo.Text.Trim();
            CompanyMaster_BL.CM_ACC_TYPE = txtTypeofAccount.Text.Trim();
            CompanyMaster_BL.CM_B_SWIFT_CODE = txtSwiftCode.Text.Trim();
            CompanyMaster_BL.CM_IFSC_CODE = txtIFSCCode.Text.Trim();
            CompanyMaster_BL.CM_COMM_CUSTOM = txtCommCustom.Text.Trim();
            CompanyMaster_BL.CM_AUT_SPEC_SIGN = txtSpecimenSign.Text.Trim();
            CompanyMaster_BL.CM_GST_NO = txtGSTNo.Text.Trim();

            res = true;

            if (CompanyMaster_BL.CM_CLOSING_DATE <= CompanyMaster_BL.CM_OPENING_DATE)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Correct Cloasing Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                res = false;
                return res;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "SetValues", ex.Message);
        }
        return res;
    }
    #endregion Setvalues

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            txtCompanyName.Text = CompanyMaster_BL.CM_NAME;
            txtAddressLine1.Text = CompanyMaster_BL.CM_ADDRESS1;
            txtAddressLine2.Text = CompanyMaster_BL.CM_ADDRESS2;
            txtAuthSign.Text = CompanyMaster_BL.CM_OWNER;
            txtPhoneNo1.Text = CompanyMaster_BL.CM_PHONENO1;
            txtPhoneNumber2.Text = CompanyMaster_BL.CM_PHONENO2;
            txtPhoneNumber3.Text = CompanyMaster_BL.CM_PHONENO3;
            txtFaxNo.Text = CompanyMaster_BL.CM_FAXNO;
            txtEmailId.Text = CompanyMaster_BL.CM_EMAILID;
            txtWebsite.Text = CompanyMaster_BL.CM_WEBSITE;
            txtVatTinNo.Text = CompanyMaster_BL.CM_VAT_TIN_NO;
            txtCstNo.Text = CompanyMaster_BL.CM_CST_NO;
            txtServiceTax.Text = CompanyMaster_BL.CM_SERVICE_TAX_NO;
            txtPanNo.Text = CompanyMaster_BL.CM_PAN_NO;
            cbActiveIndex.Checked = CompanyMaster_BL.CM_ACTIVE_IND;
            txtOpeningDate.Text = CompanyMaster_BL.CM_OPENING_DATE.ToString("dd/MMM/yyyy");
            txtClosingDate.Text = CompanyMaster_BL.CM_CLOSING_DATE.ToString("dd/MMM/yyyy");
            txtReghNo.Text = CompanyMaster_BL.CM_REGD_NO;
            txtEccNo.Text = CompanyMaster_BL.CM_ECC_NO;
            if (CompanyMaster_BL.CM_VAT_WEF.ToString() == "01-01-1930 00:00:00")
            {
                txtVatWef.Text = "";
            }
            else
            {
                txtVatWef.Text = CompanyMaster_BL.CM_VAT_WEF.ToString("dd/MMM/yyyy");
            }
            if (CompanyMaster_BL.CM_CST_WEF.ToString() == Convert.ToDateTime("01-01-1930 00:00:00").ToString())
            {
                txtCstWef.Text = "";
            }
            else
            {
                txtCstWef.Text = CompanyMaster_BL.CM_CST_WEF.ToString("dd/MMM/yyyy");
            }
            txtISONumber.Text = CompanyMaster_BL.CM_ISO_NUMBER;

            txtExpLicenNo.Text = CompanyMaster_BL.CM_EXP_LICEN_NO;
            txtExpPermisiionNo.Text = CompanyMaster_BL.CM_EXP_PERMISSOIN_NO;

            txtExciseRange.Text = CompanyMaster_BL.CM_EXCISE_RANGE;
            txtExciseDevision.Text = CompanyMaster_BL.CM_EXCISE_DIVISION;
            txtCommisionerate.Text = CompanyMaster_BL.CM_COMMISONERATE;
            txtExcSupreDetail.Text = CompanyMaster_BL.CM_EXC_SUPRE_DETAILS;

            txtBankersName.Text = CompanyMaster_BL.CM_BANK_NAME;
            txtBranchAddress.Text = CompanyMaster_BL.CM_BANK_ADDRESS;
            txtAccountNo.Text = CompanyMaster_BL.CM_BANK_ACC_NO;
            txtTypeofAccount.Text = CompanyMaster_BL.CM_ACC_TYPE;
            txtSwiftCode.Text = CompanyMaster_BL.CM_B_SWIFT_CODE;
            txtIFSCCode.Text = CompanyMaster_BL.CM_IFSC_CODE;
            txtCinNo.Text = CompanyMaster_BL.CM_CIN_NO;
            txtCommCustom.Text = CompanyMaster_BL.CM_COMM_CUSTOM;
            txtSpecimenSign.Text = CompanyMaster_BL.CM_AUT_SPEC_SIGN;
            txtGSTNo.Text = CompanyMaster_BL.CM_GST_NO;

            int Country = CompanyMaster_BL.CM_COUNTRY;
            int state = CompanyMaster_BL.CM_STATE;
            int City = CompanyMaster_BL.CM_CITY;
            LoadCountry();
            ddlCountry.SelectedValue = Country.ToString();
            LoadState();
            ddlState.SelectedValue = state.ToString();
            LoadCity();
            ddlCity.SelectedValue = City.ToString();

            if (str == "VIEW")
            {
                txtCompanyName.Enabled = false;
                txtAddressLine1.Enabled = false;
                txtAddressLine2.Enabled = false;
                ddlCountry.Enabled = false;
                txtAuthSign.Enabled = false;
                txtPhoneNo1.Enabled = false;
                txtPhoneNumber2.Enabled = false;
                txtPhoneNumber3.Enabled = false;
                txtFaxNo.Enabled = false;
                txtEmailId.Enabled = false;
                txtWebsite.Enabled = false;
                txtVatTinNo.Enabled = false;
                txtCstNo.Enabled = false;
                txtServiceTax.Enabled = false;
                txtPanNo.Enabled = false;
                txtBankersName.Enabled = false;
                txtBranchAddress.Enabled = false;
                txtAccountNo.Enabled = false;
                txtTypeofAccount.Enabled = false;
                txtSwiftCode.Enabled = false;
                txtIFSCCode.Enabled = false;
                cbActiveIndex.Enabled = false;
                txtOpeningDate.Enabled = false;
                txtClosingDate.Enabled = false;
                txtCommCustom.Enabled = false;
                txtSpecimenSign.Enabled = false;
                btnSubmit.Visible = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion GetValues

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckValid())
            {
                ModalPopupPrintSelection.TargetControlID = "btnCancel";
                ModalPopupPrintSelection.Show();
                popUpPanel5.Visible = true;
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
    #endregion btnCancel_Click

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCompanyName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtAddressLine1.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtOpeningDate.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtClosingDate.Text.Trim() == "")
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

    private void CancelRecord()
    {
        try
        {
            CommonClasses.RemoveModifyLock("CM_COMPANY_MASTER", "CM_MODIFY_FLAG", Session["CompanyId"].ToString(), mlCode);
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "btnCancel_Click", ex.Message);
        }
    }
    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click

    #region ddlCountry_SelectedIndexChanged
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadState();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "ddlCountry_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion ddlCountry_SelectedIndexChanged

    #region ddlState_SelectedIndexChanged
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCity();
    }
    #endregion ddlState_SelectedIndexChanged

    #region Imgclose_Click
    protected void Imgclose_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            CommonClasses.RemoveModifyLock("CM_COMPANY_MASTER", "CM_MODIFY_FLAG", Session["CompanyId"].ToString(), mlCode);
            Response.Redirect("~/Masters/ADD/BranchSelection.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "Imgclose_Click", ex.Message);
        }
    }
    #endregion Imgclose_Click

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
            CommonClasses.SendError("Department Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void txtOpeningDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOpeningDate.Text != "")
            {
                if (Convert.ToDateTime(txtOpeningDate.Text).Month <= 3)
                {
                    txtClosingDate.Text = "31/Mar/" + Convert.ToDateTime(txtOpeningDate.Text).Year;
                }
                else
                {
                    txtClosingDate.Text = "31/Mar/" + (Convert.ToDateTime(txtOpeningDate.Text).Year + 1);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "txtOpeningDate_TextChanged", ex.Message);
        }
    }
    protected void txtVatTinNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVatTinNo.Text != "")
            {
                char[] ch = txtVatTinNo.Text.ToCharArray();
                ch[ch.Length - 1] = 'C';
                txtCstNo.Text = new string(ch);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Company Master", "txtVatTinNo_TextChanged", ex.Message);
        }
    }
}
