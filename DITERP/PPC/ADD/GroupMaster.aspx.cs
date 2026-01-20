using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_GroupMaster : System.Web.UI.Page
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
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
                        ViewState["mlCode"] = "";
                        ViewState["mlCode"] = mlCode;
                        LoadOwner();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Group Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Group Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadOwner();
            dt = CommonClasses.Execute("SELECT GP_CODE,GP_NAME,GP_DESC, GP_OWNER,UM_USERNAME FROM GROUP_MASTER inner join USER_MASTER on GP_OWNER=UM_CODE where USER_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0 and GP_CODE='" + ViewState["mlCode"] + "' AND GP_COMP_ID='" + (string)Session["CompanyId"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtGroupName.Text = dt.Rows[0]["GP_NAME"].ToString();
                txtDescription.Text = dt.Rows[0]["GP_DESC"].ToString();
                ddlOwner.SelectedValue = dt.Rows[0]["GP_OWNER"].ToString();
                if (str == "VIEW")
                {
                    txtDescription.Enabled = false; txtGroupName.Enabled = false; ddlOwner.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("GROUP_MASTER", "MODIFY", "GP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Group Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (txtGroupName.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter Group Name", CommonClasses.MSG_Warning);
            txtGroupName.Focus();
            return;
        }
        if (ddlOwner.SelectedIndex == -1 || ddlOwner.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Owner Name", CommonClasses.MSG_Warning);
            ddlOwner.Focus();
            return;
        }
        #endregion

        SaveRec();
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select GP_CODE,GP_NAME FROM GROUP_MASTER WHERE lower(GP_NAME)= lower('" + txtGroupName.Text.Trim().Replace("'", "\''") + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into GROUP_MASTER (GP_COMP_ID,GP_NAME,GP_DESC,GP_OWNER) values('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtGroupName.Text.Trim().Replace("'", "\''") + "','" + txtDescription.Text.Trim().Replace("'", "\''") + "','" + ddlOwner.SelectedValue + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(GP_CODE) from GROUP_MASTER");
                        CommonClasses.WriteLog("GROUP MASTER", "Save", "GROUP MASTER", txtDescription.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewGroupMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtGroupName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Group Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM GROUP_MASTER WHERE ES_DELETE=0 AND GP_CODE!= '" + ViewState["mlCode"] + "' AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE GROUP_MASTER SET GP_NAME='" + txtGroupName.Text.Trim().Replace("'", "\''") + "',GP_DESC='" + txtDescription.Text.Trim().Replace("'", "\''") + "',GP_OWNER='" + ddlOwner.SelectedValue + "' WHERE GP_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("GROUP_MASTER", "MODIFY", "GP_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("GROUP Master", "Update", "GROUP Master", txtGroupName.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewGroupMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtGroupName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Group Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtGroupName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Group Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("GROUP_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Group Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("GROUP_MASTER", "MODIFY", "GP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewGroupMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Group Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtGroupName.Text.Trim() == "")
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
            CommonClasses.SendError("Group Master", "CheckValid", Ex.Message);
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

    #region LoadOwner
    protected void LoadOwner()
    {
        DataTable dtOwner = new DataTable();
        dtOwner = CommonClasses.Execute("SELECT distinct UM_CODE,UM_NAME from USER_MASTER where ES_DELETE=0 and UM_CM_ID='" + Session["CompanyId"] + "' ORDER BY UM_NAME");
        ddlOwner.DataSource = dtOwner;
        ddlOwner.DataTextField = "UM_NAME";
        ddlOwner.DataValueField = "UM_CODE";
        ddlOwner.DataBind();
        ddlOwner.Items.Insert(0, new ListItem("Select Owner Name", "0"));
    }
    #endregion LoadOwner
}
