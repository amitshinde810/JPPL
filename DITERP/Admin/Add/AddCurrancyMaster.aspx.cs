using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_ADD_AddCurrancyMaster : System.Web.UI.Page
{
    #region General Declaration
    CurrancyMaster_BL BL_CurrancyMaster = null;
    static int mlCode = 0;
    static string right = "";
    DataTable dt5 = new DataTable();
    #endregion

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='13'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                LoadCountry();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_CurrancyMaster = new CurrancyMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_CurrancyMaster = new CurrancyMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    ddlCountry.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Currancy Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCurrancyName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtCurrancyShortName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtCurrancyDesc.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtCurrencyRate.Text.Trim() == "")
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
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("CURRANCY_MASTER", "MODIFY", "CURR_CODE", mlCode);
            }
            Response.Redirect("~/Admin/VIEW/ViewCurrancyMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Currancy Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click

    #region SaveRec
    bool SaveRec()
    {
        int Zero = 0;
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (txtCurrencyRate.Text == Zero.ToString())
                {
                    ShowMessage("#Avisos", "Rate not be a 0 ", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                else
                {
                }
                BL_CurrancyMaster = new CurrancyMaster_BL();
                if (Setvalues())
                {
                    if (BL_CurrancyMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CURR_CODE) from CURRANCY_MASTER");
                        CommonClasses.WriteLog("Currency Master", "Save", "Item Category Master", BL_CurrancyMaster.CURR_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/VIEW/ViewCurrancyMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CurrancyMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_CurrancyMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            BL_CurrancyMaster.Msg = "";
                        }
                        ddlCountry.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_CurrancyMaster = new CurrancyMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_CurrancyMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("CURRANCY_MASTER", "MODIFY", "CURR_CODE", mlCode);
                        CommonClasses.WriteLog("Currancy Master", "Update", "Currancy Master", BL_CurrancyMaster.CURR_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/VIEW/ViewCurrancyMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CurrancyMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_CurrancyMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_CurrancyMaster.Msg = "";
                        }
                        txtCurrancyName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_CurrancyMaster.CURR_COUNTRY_CODE = Convert.ToInt32(ddlCountry.SelectedValue);
            BL_CurrancyMaster.CURR_NAME = txtCurrancyName.Text.ToUpper();
            BL_CurrancyMaster.CURR_SHORT_NAME = txtCurrancyShortName.Text;
            BL_CurrancyMaster.CURR_DESC = txtCurrancyDesc.Text;
            BL_CurrancyMaster.CURR_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            string d = string.Format("{0:0.00}", Convert.ToDouble(txtCurrencyRate.Text));
            BL_CurrancyMaster.CURR_RATE = Convert.ToDouble(d);

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Master", "Setvalues", ex.Message);
        }
        return res;
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
            CommonClasses.SendError("Currancy Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_CurrancyMaster = new CurrancyMaster_BL(mlCode);

            dt5 = CommonClasses.Execute("Select * from CURRANCY_MASTER where CURR_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and CURR_CODE=" + mlCode + "");
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_CATEGORY_MASTER", "MODIFY", "I_CAT_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                ddlCountry.SelectedValue = dt5.Rows[0]["CURR_COUNTRY_CODE"].ToString();
                txtCurrancyName.Text = dt5.Rows[0]["CURR_NAME"].ToString();
                txtCurrancyShortName.Text = dt5.Rows[0]["CURR_SHORT_NAME"].ToString();
                txtCurrancyDesc.Text = dt5.Rows[0]["CURR_DESC"].ToString();
                txtCurrencyRate.Text = dt5.Rows[0]["CURR_RATE"].ToString();

                ddlCountry.Enabled = false;
                txtCurrancyName.Enabled = false;
                txtCurrancyShortName.Enabled = false;
                txtCurrancyDesc.Enabled = false;
                btnSubmit.Visible = false;
                txtCurrencyRate.Enabled = false;
            }
            else if (str == "MOD")
            {
                ddlCountry.SelectedValue = dt5.Rows[0]["CURR_COUNTRY_CODE"].ToString();
                txtCurrancyName.Text = dt5.Rows[0]["CURR_NAME"].ToString();
                txtCurrancyShortName.Text = dt5.Rows[0]["CURR_DESC"].ToString();
                txtCurrancyDesc.Text = dt5.Rows[0]["CURR_SHORT_NAME"].ToString();
                txtCurrencyRate.Text = dt5.Rows[0]["CURR_RATE"].ToString();
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currancy Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region LoadCountry
    private void LoadCountry()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID=" + Session["CompanyId"] + "");

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
            CommonClasses.SendError("Country Master", "LoadCountry", ex.Message);
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

    protected void txtCurrencyRate_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtCurrencyRate.Text);
        txtCurrencyRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
}