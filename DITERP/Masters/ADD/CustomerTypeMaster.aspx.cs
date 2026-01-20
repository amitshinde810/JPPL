using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_ADD_CustomerTypeMaster : System.Web.UI.Page
{
    #region General Declaration
    CustomerTypeMaster_BL BL_CustomerTypeMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";

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
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_CustomerTypeMaster = new CustomerTypeMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_CustomerTypeMaster = new CustomerTypeMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtCustomerTypeCode.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Customer Type Master", "PageLoad", ex.Message);
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    }

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
                   // ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "btnCancel_Click", Ex.Message);
        }
    }

    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_CustomerTypeMaster.CTM_TYPE_CODE = txtCustomerTypeCode.Text.Trim().Replace("'", "\''");
            BL_CustomerTypeMaster.CTM_TYPE_DESC = txtCustomerTypeDesc.Text.Trim().Replace("'", "\''");
            BL_CustomerTypeMaster.CTM_FIRST_LETTER = txtSuuplierNoFirstName.Text.Trim().Replace("'", "\''");
            BL_CustomerTypeMaster.CTM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master", "Setvalues", ex.Message);
        }
        return res;
    }

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_CustomerTypeMaster = new CustomerTypeMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_CustomerTypeMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CUSTOMER_TYPE_MASTER", "MODIFY", "CTM_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "ViewRec", Ex.Message);
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
                txtCustomerTypeCode.Text = BL_CustomerTypeMaster.CTM_TYPE_CODE;
                txtCustomerTypeDesc.Text = BL_CustomerTypeMaster.CTM_TYPE_DESC;
                txtSuuplierNoFirstName.Text = BL_CustomerTypeMaster.CTM_FIRST_LETTER;
                txtCustomerTypeCode.Enabled = false;
                txtCustomerTypeDesc.Enabled = false;
                txtSuuplierNoFirstName.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtCustomerTypeCode.Text = BL_CustomerTypeMaster.CTM_TYPE_CODE;
                txtCustomerTypeDesc.Text = BL_CustomerTypeMaster.CTM_TYPE_DESC;
                txtSuuplierNoFirstName.Text = BL_CustomerTypeMaster.CTM_FIRST_LETTER;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master", "GetValues", ex.Message);
        }
        return res;
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
            CommonClasses.SendError("Customer Type Master", "ShowMessage", Ex.Message);
            return false;
        }
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
                BL_CustomerTypeMaster = new CustomerTypeMaster_BL();
                if (Setvalues())
                {
                    if (BL_CustomerTypeMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(CTM_CODE) from CUSTOMER_TYPE_MASTER");
                        CommonClasses.WriteLog("Customer Type Master", "Save", "Customer Type Master", BL_CustomerTypeMaster.CTM_TYPE_CODE, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewCustomerTypeMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CustomerTypeMaster.Msg != "")
                        {
                            lblmsg.Text = BL_CustomerTypeMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_CustomerTypeMaster.Msg = "";
                        }
                        txtCustomerTypeCode.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_CustomerTypeMaster = new CustomerTypeMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_CustomerTypeMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("CUSTOMER_TYPE_MASTER", "MODIFY", "CTM_CODE", mlCode);
                        CommonClasses.WriteLog("Customer Type Master", "Update", "Customer Type Master", BL_CustomerTypeMaster.CTM_TYPE_CODE, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewCustomerTypeMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_CustomerTypeMaster.Msg != "")
                        {
                            lblmsg.Text = BL_CustomerTypeMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_CustomerTypeMaster.Msg = "";
                        }
                        txtCustomerTypeCode.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Customer Type Master", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("CUSTOMER_TYPE_MASTER", "MODIFY", "CTM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewCustomerTypeMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCustomerTypeCode.Text == "")
            {
                flag = false;
            }
            else if (txtCustomerTypeDesc.Text == "")
            {
                flag = false;
            }
            else if (txtSuuplierNoFirstName.Text == "")
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
            CommonClasses.SendError("Customer Type Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    protected void txtCustomerTypeDesc_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerTypeDesc.Text.Trim() != "")
        {
            char[] ch = txtCustomerTypeDesc.Text.ToCharArray();
            txtSuuplierNoFirstName.Text = ch[0].ToString();
        }
    }
}
