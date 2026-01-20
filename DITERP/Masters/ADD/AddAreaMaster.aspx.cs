using System;
using System.Web.UI;
using System.Data;

public partial class Masters_ADD_AddAreaMaster : System.Web.UI.Page
{
    #region General Declaration
    AreaMaster_BL BL_AreaMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

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
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_AreaMaster = new AreaMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_AreaMaster = new AreaMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtAreaCode.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Area Master", "PageLoad", ex.Message);
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
            CommonClasses.SendError("Area Master", "btnCancel_Click", Ex.Message);
        }
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_AreaMaster = new AreaMaster_BL();
                if (Setvalues())
                {
                    if (BL_AreaMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(A_CODE) from AREA_MASTER");
                        CommonClasses.WriteLog("Area Master", "Save", "Area Master", BL_AreaMaster.A_NO, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewAreaMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_AreaMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_AreaMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            BL_AreaMaster.Msg = "";
                        }
                        txtAreaCode.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_AreaMaster = new AreaMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_AreaMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("AREA_MASTER", "MODIFY", "A_CODE", mlCode);
                        CommonClasses.WriteLog("Area Master", "Update", "Area Master", BL_AreaMaster.A_NO, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewAreaMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_AreaMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_AreaMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_AreaMaster.Msg = "";
                        }
                        txtAreaCode.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Area Master", "SaveRec", ex.Message);
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
            BL_AreaMaster.A_NO = txtAreaCode.Text;

            BL_AreaMaster.A_DESC = txtAreaDescription.Text;
            BL_AreaMaster.A_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Area Master", "Setvalues", ex.Message);
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
            CommonClasses.SendError("Admin Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_AreaMaster = new AreaMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_AreaMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("AREA_MASTER", "MODIFY", "A_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Master", "ViewRec", Ex.Message);
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
                txtAreaCode.Text = BL_AreaMaster.A_NO;
                txtAreaDescription.Text = BL_AreaMaster.A_DESC;

                txtAreaCode.Enabled = false;
                txtAreaDescription.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtAreaCode.Text = BL_AreaMaster.A_NO;
                txtAreaDescription.Text = BL_AreaMaster.A_DESC;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Area Master", "GetValues", ex.Message);
        }
        return res;
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
            CommonClasses.SendError("Area Master", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("AREA_MASTER", "MODIFY", "A_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewAreaMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Area Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtAreaCode.Text == "")
            {
                flag = false;
            }
            else if (txtAreaDescription.Text == "")
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
            CommonClasses.SendError("Area Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

}