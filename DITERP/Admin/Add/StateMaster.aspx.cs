using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_ADD_StateMaster : System.Web.UI.Page
{
    #region General Declaration
    StateMaster_BL BL_StateMaster = null;
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='6'");
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
                CommonClasses.SendError("State Master", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("State Master", "btnCancel_Click", ex.Message.ToString());
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtStateName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlCountry.SelectedIndex <= 0)
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
            CommonClasses.SendError("State Master", "CheckValid", Ex.Message);
        }
        return flag;
    }

    private void CancelRecord()
    {
        txtStateName.Text = "";
        ddlCountry.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            txtStateName.Text = "";
            ddlCountry.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "ClearFied", Ex.Message);
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
            CommonClasses.SendError("State Master", "LoadCountry", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_StateMaster = new StateMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_StateMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceStateName = txtStateName.Text;

            StrReplaceStateName = StrReplaceStateName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_StateMaster = new StateMaster_BL();
                if (Setvalues())
                {
                    if (BL_StateMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SM_CODE) from STATE_MASTER");
                        CommonClasses.WriteLog("State Master", "Save", "State Master", BL_StateMaster.SM_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_StateMaster.Msg != "")
                        {
                            lblmsg.Text = BL_StateMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_StateMaster.Msg = "";
                        }
                        ddlCountry.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_StateMaster = new StateMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_StateMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
                        CommonClasses.WriteLog("State Master", "Update", "State Master", BL_StateMaster.SM_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_StateMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_StateMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_StateMaster.Msg = "";
                        }
                        ddlCountry.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("State Master", "ShowMessage", Ex.Message);
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
            if (str == "VIEW")
            {
                ddlCountry.SelectedValue = BL_StateMaster.SM_COUNTRY_CODE.ToString();
                txtStateName.Text = BL_StateMaster.SM_NAME;
                txtStateCode.Text = BL_StateMaster.SM_STATE_CODE;
                txtStateName.Enabled = false;
                txtStateCode.Enabled = false;
                ddlCountry.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                ddlCountry.SelectedValue = BL_StateMaster.SM_COUNTRY_CODE.ToString();
                txtStateName.Text = BL_StateMaster.SM_NAME;
                txtStateCode.Text = BL_StateMaster.SM_STATE_CODE;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master", "GetValues", ex.Message);
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
            BL_StateMaster.SM_COUNTRY_CODE = Convert.ToInt32(ddlCountry.SelectedValue);
            BL_StateMaster.SM_NAME = txtStateName.Text.Trim();
            BL_StateMaster.SM_STATE_CODE = txtStateCode.Text.Trim();
            BL_StateMaster.SM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        txtStateName.Text = "";
        ddlCountry.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("STATE_MASTER", "MODIFY", "SM_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
    }
    #endregion
}
