using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Masters_SoTypeMaster : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    # endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Sale Order Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM SO_TYPE_MASTER WHERE SO_T_CODE=" + mlCode + " AND SO_T_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtShortName.Text = dt.Rows[0]["SO_T_SHORT_NAME"].ToString();
                txtDescription.Text = dt.Rows[0]["SO_T_DESC"].ToString();
                txtFirstLetter.Text = dt.Rows[0]["SO_T_FIRST_LETTER"].ToString();

                if (str == "VIEW")
                {
                    txtDescription.Enabled = false;
                    txtFirstLetter.Enabled = false;
                    txtShortName.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }

            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SO_TYPE_MASTER", "MODIFY", "SO_T_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtShortName.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter Short Name", CommonClasses.MSG_Warning);
            txtShortName.Focus();
            return;
        }
        if (txtFirstLetter.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter First Letter", CommonClasses.MSG_Warning);
            txtFirstLetter.Focus();
            return;
        }
        if (txtDescription.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter Description", CommonClasses.MSG_Warning);
            txtDescription.Focus();
            return;
        }
        SaveRec();
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
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select SO_T_CODE,SO_T_SHORT_NAME FROM SO_TYPE_MASTER WHERE lower(SO_T_SHORT_NAME)= lower('" + txtShortName.Text.Trim().Replace("'", "\''") + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO SO_TYPE_MASTER (SO_T_COMP_ID,SO_T_SHORT_NAME,SO_T_DESC,SO_T_FIRST_LETTER)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtShortName.Text.Trim().Replace("'", "\''") + "','" + txtDescription.Text.Trim().Replace("'", "\''") + "','" + txtFirstLetter.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SO_T_CODE) from SO_TYPE_MASTER");
                        CommonClasses.WriteLog("Sale Order Type Master", "Save", "Sale Order Type Master", txtDescription.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSOTypeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtShortName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Short Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM SO_TYPE_MASTER WHERE ES_DELETE=0 AND SO_T_CODE!= '" + mlCode + "' AND lower(SO_T_SHORT_NAME) = lower('" + txtShortName.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE SO_TYPE_MASTER SET SO_T_SHORT_NAME='" + txtShortName.Text.Trim().Replace("'", "\''") + "',SO_T_DESC='" + txtDescription.Text.Trim().Replace("'", "\''") + "',SO_T_FIRST_LETTER='" + txtFirstLetter.Text.Trim().Replace("'", "\''") + "' WHERE SO_T_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("SO_TYPE_MASTER", "MODIFY", "SO_T_CODE", mlCode);
                        CommonClasses.WriteLog("Sale Order Type Master", "Update", "Sale Order type Master", txtShortName.Text.Trim().Replace("'", "\''"), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewSOTypeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtShortName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Short Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtShortName.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("SO_TYPE_MASTER", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
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
            CommonClasses.SendError("Sale Order Type Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("SO_TYPE_MASTER", "MODIFY", "SO_T_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewSOTypeMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtShortName.Text.Trim() == "")
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
            CommonClasses.SendError("Sale Order Type Master", "CheckValid", Ex.Message);
        }

        return flag;
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
}
