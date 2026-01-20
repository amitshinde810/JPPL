using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Admin_Add_ProcessMaster : System.Web.UI.Page
{
    #region General Declaration
    ProcessMaster_BL BL_ProcessMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='88'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_ProcessMaster = new ProcessMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_ProcessMaster = new ProcessMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtProcessName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Process Master", "PageLoad", ex.Message);
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
            if (txtProcessName.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "The Field 'Process Name' is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Process Master", "btnCancel_Click", ex.Message.ToString());
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
            if (txtProcessName.Text.Trim() == "")
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
            CommonClasses.SendError("Process Master", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("Process_MASTER", "MODIFY", "Process_CODE", mlCode);
            }
            Response.Redirect("~/Admin/View/ViewProcessMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Methods

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_ProcessMaster = new ProcessMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_ProcessMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("Process_MASTER", "MODIFY", "Process_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "ViewRec", Ex.Message);
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
                txtProcessName.Text = BL_ProcessMaster.PROCESS_NAME;
                txtProcessName.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtProcessName.Text = BL_ProcessMaster.PROCESS_NAME;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Master", "GetValues", ex.Message);
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
            BL_ProcessMaster.PROCESS_NAME = txtProcessName.Text.ToUpper();
            BL_ProcessMaster.PROCESS_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Master", "Setvalues", ex.Message);
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
            string StrReplaceSctorName = txtProcessName.Text;

            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_ProcessMaster = new ProcessMaster_BL();
                if (Setvalues())
                {
                    if (BL_ProcessMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(Process_CODE) from Process_MASTER");
                        CommonClasses.WriteLog("Process Master", "Save", "Process Master", BL_ProcessMaster.PROCESS_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewProcessMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_ProcessMaster.Msg != "")
                        {
                            lblmsg.Text = "Record Already Exists";
                            PanelMsg.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_ProcessMaster.Msg = "";
                        }
                        txtProcessName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_ProcessMaster = new ProcessMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_ProcessMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("Process_MASTER", "MODIFY", "Process_CODE", mlCode);
                        CommonClasses.WriteLog("Process Master", "Update", "Process Master", BL_ProcessMaster.PROCESS_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewProcessMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_ProcessMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ProcessMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_ProcessMaster.Msg = "";
                        }
                        txtProcessName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Process Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion
}
