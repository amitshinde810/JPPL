using System;
using System.Data;
using System.Web.UI;

public partial class Admin_Add_CountryMaster : System.Web.UI.Page
{
    #region General Declaration
    CountryMaster_BL BL_CountryMaster = null;
    static int mlCode = 0;
    static string right = "";
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='5'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_CountryMaster = new CountryMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_CountryMaster = new CountryMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtCountryName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Country Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCountryName.Text == "")
            {
                ShowMessage("#Avisos", "The Field 'Country Name' is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Country Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCountryName.Text.Trim() == "")
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
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("COUNTRY_MASTER", "MODIFY", "COUNTRY_CODE", mlCode);
            }
            Response.Redirect("~/Admin/View/ViewCountryMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Country Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Methods
    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_CountryMaster = new CountryMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_CountryMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("COUNTRY_MASTER", "MODIFY", "COUNTRY_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Country Master", "ViewRec", Ex.Message);
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
                txtCountryName.Text = BL_CountryMaster.COUNTRY_NAME;
                txtCountryName.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtCountryName.Text = BL_CountryMaster.COUNTRY_NAME;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master", "GetValues", ex.Message);
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
            BL_CountryMaster.COUNTRY_NAME = txtCountryName.Text.ToUpper();
            BL_CountryMaster.COUNTRY_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master", "Setvalues", ex.Message);
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
            string StrReplaceSctorName = txtCountryName.Text;
            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_CountryMaster = new CountryMaster_BL();
                if (Setvalues())
                {
                    if (BL_CountryMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(COUNTRY_CODE) from COUNTRY_MASTER");
                        CommonClasses.WriteLog("Country Master", "Save", "Country Master", BL_CountryMaster.COUNTRY_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewCountryMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CountryMaster.Msg != "")
                        {
                            lblmsg.Text = "Record Already Exists";
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            BL_CountryMaster.Msg = "";
                        }
                        txtCountryName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_CountryMaster = new CountryMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_CountryMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("COUNTRY_MASTER", "MODIFY", "COUNTRY_CODE", mlCode);
                        CommonClasses.WriteLog("Country Master", "Update", "Country Master", BL_CountryMaster.COUNTRY_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewCountryMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CountryMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_CountryMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_CountryMaster.Msg = "";
                        }
                        txtCountryName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Country Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion
}
