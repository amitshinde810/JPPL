using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class IRN_ADD_RejectionStageMaster : System.Web.UI.Page
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
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";

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
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtStageNo.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Rejection Stage Master", "PageLoad", ex.Message);
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
            if (txtStageNo.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Please Enter Stage No", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStageNo.Focus();
                return;
            }
            if (txtName.Text.Trim() == "")
            {
                ShowMessage("#Avisos", "Please Enter Stage Name", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtName.Focus();
                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Rejection Stage Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtStageNo.Text.Trim() == "")
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
            CommonClasses.SendError("Rejection Stage Master", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("REJECTIONSTAGE_MASTER", "MODIFY", "RSM_CODE", mlCode);
            }
            Response.Redirect("~/IRN/VIEW/ViewRejectionStageMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT RSM_CODE, RSM_NO, RSM_NAME, RSM_CM_ID,RSM_CASTING,RSM_MECHINING FROM REJECTIONSTAGE_MASTER WHERE     (ES_DELETE = 0) AND RSM_CODE='" + mlCode + "'");
            if (dt.Rows.Count > 0)
            {
                txtStageNo.Text = dt.Rows[0]["RSM_NO"].ToString();
                txtName.Text = dt.Rows[0]["RSM_NAME"].ToString();
                chkCasting.Checked = Convert.ToBoolean(dt.Rows[0]["RSM_CASTING"].ToString());
                chkMachining.Checked = Convert.ToBoolean(dt.Rows[0]["RSM_MECHINING"].ToString());
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("REJECTIONSTAGE_MASTER", "MODIFY", "RSM_CODE", mlCode);
            }
            else
            {
                txtStageNo.Enabled = false;
                txtName.Enabled = false;
                btnSubmit.Visible = false;
                chkCasting.Enabled = false;
                chkMachining.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master", "ViewRec", Ex.Message);
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
                txtName.Enabled = false;
                txtStageNo.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {

            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Stage Master", "GetValues", ex.Message);
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
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Stage Master", "Setvalues", ex.Message);
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
            string strType = "";

            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dtExist = new DataTable();
                dtExist = CommonClasses.Execute("SELECT * FROM  REJECTIONSTAGE_MASTER where RSM_NO='" + txtStageNo.Text.Trim().Replace("'", "\''") + "' AND RSM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0");
                if (dtExist.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "This Stage No. already Exist.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStageNo.Focus();
                }
                else
                {
                    if (CommonClasses.Execute1("INSERT INTO REJECTIONSTAGE_MASTER (RSM_NO, RSM_NAME, RSM_CM_ID,RSM_CASTING,RSM_MECHINING)VALUES ('" + txtStageNo.Text.Trim().Replace("'", "\''") + "','" + txtName.Text.Trim().Replace("'", "\''") + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + chkCasting.Checked + "','" + chkMachining.Checked + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(RSM_CODE) from REJECTIONSTAGE_MASTER");
                        CommonClasses.WriteLog("Rejection Stage Master", "Save", "Rejection Stage Master", txtStageNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/IRN/VIEW/ViewRejectionStageMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtStageNo.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dtExist = new DataTable();
                dtExist = CommonClasses.Execute("SELECT * FROM  REJECTIONSTAGE_MASTER where RSM_NO='" + txtStageNo.Text.Trim().Replace("'", "\''") + "' AND RSM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND RSM_CODE!='" + mlCode + "'");
                if (dtExist.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "This Stage No. already Exist.", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStageNo.Focus();
                }
                else
                {
                    if (CommonClasses.Execute1("UPDATE REJECTIONSTAGE_MASTER  SET RSM_NO='" + txtStageNo.Text.Trim().Replace("'", "\''") + "',RSM_NAME='" + txtName.Text.Trim().Replace("'", "\''") + "' ,RSM_CASTING='" + chkCasting.Checked + "',RSM_MECHINING='" + chkMachining.Checked + "'  where RSM_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("REJECTIONSTAGE_MASTER", "MODIFY", "RSM_CODE", mlCode);
                        CommonClasses.WriteLog("Rejection Stage Master", "Update", "Rejection Stage Master", txtStageNo.Text.Trim().Replace("'", "\''"), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/IRN/VIEW/ViewRejectionStageMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtStageNo.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Stage Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Rejection Stage Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
