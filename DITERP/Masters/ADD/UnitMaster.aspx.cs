using System;
using System.Data;
using System.Web.UI;

public partial class Masters_ADD_UnitMaster : System.Web.UI.Page
{
    #region General Declaration
    UnitMaster_BL BL_UnitMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Events

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
                //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='13'");
                //right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_UnitMaster = new UnitMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_UnitMaster = new UnitMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtUnitName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Unit Master", "PageLoad", ex.Message);
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
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            //{
            SaveRec();
            //}
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Unit Master", "btnSubmit_Click", Ex.Message);
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
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Unit Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtUnitDesc.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtUnitName.Text.Trim() == "")
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
            CommonClasses.SendError("Unit Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ITEM_UNIT_MASTER", "MODIFY", "I_UOM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewUnitMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_UnitMaster = new UnitMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_UnitMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_UNIT_MASTER", "MODIFY", "I_UOM_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            txtUnitName.Text = BL_UnitMaster.I_UOM_NAME;
            txtUnitDesc.Text = BL_UnitMaster.I_UOM_DESC;
            if (str == "VIEW")
            {
                txtUnitName.Enabled = false;
                txtUnitDesc.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Unit Master", "GetValues", ex.Message);
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
            BL_UnitMaster.I_UOM_NAME = txtUnitName.Text.ToUpper();
            BL_UnitMaster.I_UOM_DESC = txtUnitDesc.Text.ToUpper();
            BL_UnitMaster.I_UOM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Unit Master", "Setvalues", ex.Message);
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
                BL_UnitMaster = new UnitMaster_BL();
                if (Setvalues())
                {
                    if (BL_UnitMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(I_UOM_CODE) from ITEM_UNIT_MASTER");
                        CommonClasses.WriteLog("Unit Master", "Save", "Unit Master", BL_UnitMaster.I_UOM_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewUnitMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_UnitMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_UnitMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_UnitMaster.Msg = "";
                        }
                        txtUnitName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_UnitMaster = new UnitMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_UnitMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("ITEM_UNIT_MASTER", "MODIFY", "I_UOM_CODE", mlCode);
                        CommonClasses.WriteLog("Unit Master", "Update", "Unit Master", BL_UnitMaster.I_UOM_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewUnitMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_UnitMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_UnitMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            BL_UnitMaster.Msg = "";
                        }
                        txtUnitName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Unit Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Unit Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}