using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;


public partial class Masters_ADD_AddSupplierTypeMaster : System.Web.UI.Page
{
    #region General Declaration
    SupplierTypeMaster_BL BL_SupplierTypeMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='17'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_SupplierTypeMaster = new SupplierTypeMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_SupplierTypeMaster = new SupplierTypeMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtSupplierTypeCode.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Supplier Type Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtSupplierTypeCode.Text.Trim() == "")
        {
            lblmsg.Text = "Please Enter Type Code";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtSupplierTypeCode.Focus();
            return;
        }
        if (txtSupplierTypeDesc.Text.Trim() == "")
        {
            lblmsg.Text = "Please Enter Type Desc";
            PanelMsg.Visible = true;
            lblmsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtSupplierTypeDesc.Focus();
            return;
        }
        if (txtSuuplierNoFirstName.Text.Trim() == "")
        {
            lblmsg.Text = "Please Enter Starting Character";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtSuuplierNoFirstName.Focus();
            return;
        }
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
                    ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Supplier Type Master", "btnCancel_Click", Ex.Message);
        }
    }

    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_SupplierTypeMaster.STM_TYPE_CODE = txtSupplierTypeCode.Text.Trim().Replace("'", "\''");
            BL_SupplierTypeMaster.STM_TYPE_DESC = txtSupplierTypeDesc.Text.Trim().Replace("'", "\''");
            BL_SupplierTypeMaster.STM_FIRST_LETTER = txtSuuplierNoFirstName.Text.Trim().Replace("'", "\''");
            BL_SupplierTypeMaster.STM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Type Master", "Setvalues", ex.Message);
        }
        return res;
    }

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_SupplierTypeMaster = new SupplierTypeMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_SupplierTypeMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SUPPLIER_TYPE_MASTER", "MODIFY", "STM_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Type Master", "ViewRec", Ex.Message);
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
                txtSupplierTypeCode.Text = BL_SupplierTypeMaster.STM_TYPE_CODE;
                txtSupplierTypeDesc.Text = BL_SupplierTypeMaster.STM_TYPE_DESC;
                txtSuuplierNoFirstName.Text = BL_SupplierTypeMaster.STM_FIRST_LETTER;
                txtSupplierTypeCode.Enabled = false;
                txtSupplierTypeDesc.Enabled = false;
                txtSuuplierNoFirstName.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtSupplierTypeCode.Text = BL_SupplierTypeMaster.STM_TYPE_CODE;
                txtSupplierTypeDesc.Text = BL_SupplierTypeMaster.STM_TYPE_DESC;
                txtSuuplierNoFirstName.Text = BL_SupplierTypeMaster.STM_FIRST_LETTER;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Type Master", "GetValues", ex.Message);
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
            CommonClasses.SendError("Supplier Type Master", "ShowMessage", Ex.Message);
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
                BL_SupplierTypeMaster = new SupplierTypeMaster_BL();
                if (Setvalues())
                {
                    if (BL_SupplierTypeMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(STM_CODE) from SUPPLIER_TYPE_MASTER");
                        CommonClasses.WriteLog("Supplier Type Master", "Save", "Supplier Type Master", BL_SupplierTypeMaster.STM_TYPE_CODE, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSupplierTypeMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_SupplierTypeMaster.Msg != "")
                        {
                            lblmsg.Text = BL_SupplierTypeMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_SupplierTypeMaster.Msg = "";
                        }
                        txtSupplierTypeCode.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_SupplierTypeMaster = new SupplierTypeMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_SupplierTypeMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("SUPPLIER_TYPE_MASTER", "MODIFY", "STM_CODE", mlCode);
                        CommonClasses.WriteLog("Supplier Type Master", "Update", "Supplier Type Master", BL_SupplierTypeMaster.STM_TYPE_CODE, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSupplierTypeMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_SupplierTypeMaster.Msg != "")
                        {
                            lblmsg.Text = BL_SupplierTypeMaster.Msg;
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_SupplierTypeMaster.Msg = "";
                        }
                        txtSupplierTypeCode.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Type Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Supplier Type Master", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("SUPPLIER_TYPE_MASTER", "MODIFY", "STM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewSupplierTypeMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Type Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtSupplierTypeCode.Text == "")
            {
                flag = false;
            }
            else if (txtSupplierTypeDesc.Text == "")
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
            CommonClasses.SendError("Supplier Type Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    protected void txtSupplierTypeDesc_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierTypeDesc.Text.Trim() != "")
        {
            char[] ch = txtSupplierTypeDesc.Text.ToCharArray();
            txtSuuplierNoFirstName.Text = ch[0].ToString();
        }
    }
}
