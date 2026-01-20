#region class liabraries
using System;
using System.Data;  
using System.Web.UI;
using System.Web.UI.WebControls; 
#endregion

public partial class Masters_ADD_CityMaster : System.Web.UI.Page
{
    #region General Declaration
    CityMaster_BL BL_CityMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='7'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadCountry();

                if (Request.QueryString[0].Equals("VIEW"))
                {
                    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                    ViewRec("VIEW");
                }
                else if (Request.QueryString[0].Equals("MODIFY"))
                {
                    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                    ViewRec("MOD");
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("City Master", "Page_Load", Ex.Message);
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

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        LoadCancel();
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                LoadCancel();
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
                    LoadCancel();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "btnCancel_Click", ex.Message.ToString());
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        LoadCancel();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCityName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlCountry.SelectedIndex <= 0)
            {
                flag = false;
            }
            else if (ddlState.SelectedIndex <= 0)
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
            CommonClasses.SendError("City Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    void LoadCancel()
    {
        txtCityName.Text = "";
        ddlCountry.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/ViewCityMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtCityName.Text = "";
            ddlCountry.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "ClearFied", Ex.Message);
        }
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
                dt = CommonClasses.Execute("select COUNTRY_CODE,COUNTRY_NAME from COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID='" + (string)Session["CompanyId"] + "'");
                ddlCountry.DataSource = dt;
                ddlCountry.DataTextField = "COUNTRY_NAME";
                ddlCountry.DataValueField = "COUNTRY_CODE";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "LoadCountry", Ex.Message);
        }
    }
    #endregion

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadState();
    }

    #region Load State
    private void LoadState()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from STATE_MASTER where ES_DELETE=0 and SM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and STATE_MASTER.SM_COUNTRY_CODE=" + ddlCountry.SelectedValue + "");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "SM_NAME";
                ddlState.DataValueField = "SM_CODE";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("Select State", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("City Master", "LoadState", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "LoadState", ex.Message);
        }
    }
    #endregion Load State

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_CityMaster = new CityMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_CityMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceCityName = txtCityName.Text;

            StrReplaceCityName = StrReplaceCityName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_CityMaster = new CityMaster_BL();
                if (Setvalues())
                {
                    if (BL_CityMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CITY_CODE) from CITY_MASTER");
                        CommonClasses.WriteLog("City Master", "Save", "City Master", BL_CityMaster.CITY_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewCityMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CityMaster.Msg != "")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = BL_CityMaster.Msg.ToString();
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_CityMaster.Msg = "";
                        }
                        txtCityName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_CityMaster = new CityMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_CityMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("CITY_MASTER", "MODIFY", "CITY_CODE", mlCode);
                        CommonClasses.WriteLog("City Master", "Update", "City Master", BL_CityMaster.CITY_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewCityMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CityMaster.Msg != "")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = BL_CityMaster.Msg.ToString();
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_CityMaster.Msg = "";
                        }
                        txtCityName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("City Master", "ShowMessage", Ex.Message);
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
            ddlCountry.SelectedValue = BL_CityMaster.CITY_COUNTRY_CODE.ToString();
            ddlCountry_SelectedIndexChanged(null, null);
            ddlState.SelectedValue = BL_CityMaster.CITY_SM_CODE.ToString();
            txtCityName.Text = BL_CityMaster.CITY_NAME;

            if (str == "VIEW")
            {
                txtCityName.Enabled = false;
                ddlCountry.Enabled = false;
                ddlState.Enabled = false;
                btnSubmit.Visible = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "GetValues", ex.Message);
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
            BL_CityMaster.CITY_COUNTRY_CODE = Convert.ToInt32(ddlCountry.SelectedValue);
            BL_CityMaster.CITY_SM_CODE = Convert.ToInt32(ddlState.SelectedValue);
            BL_CityMaster.CITY_NAME = txtCityName.Text;
            BL_CityMaster.CITY_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion
}
