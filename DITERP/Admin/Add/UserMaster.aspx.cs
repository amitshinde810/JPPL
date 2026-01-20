using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class Admin_Add_UserMaster : System.Web.UI.Page
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
                        txtusername.Text = "";
                        txtpass.Text = "";
                        ViewState["mlCode"] = mlCode;
                        txtUserEmail.Text = "";
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else
                        {
                            LoadchkStore();
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
        }
    }
    #endregion

    #region LoadchkStore                        
    public void LoadchkStore()
    {
        DataTable dtStore = CommonClasses.Execute(" SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ES_DELETE=0 ");
        for (int i = 0; i < dtStore.Rows.Count; i++)
        {
            ListItem item = new ListItem();
            item.Text = dtStore.Rows[i]["STORE_NAME"].ToString();
            item.Value = dtStore.Rows[i]["STORE_CODE"].ToString();
            item.Selected = false;
            chkItems.Items.Add(item);
            chkOwner.Items.Add(item);
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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtusername.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtUserID.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtpass.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtRepass.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlUserlevel.SelectedIndex <= 0)
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
            CommonClasses.SendError("User Master", "CheckValid", Ex.Message);
        }
        return flag;
    }

    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("USER_MASTER", "MODIFY", "UM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Admin/VIEW/ViewUsers.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtusername.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter User Name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtusername.Focus();
            return;
        }
        if (txtUserID.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter User ID";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtUserID.Focus();
            return;
        }
        if (txtpass.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Password";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtpass.Focus();
            return;
        }
        if (txtRepass.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Re-Enter Password";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtRepass.Focus();
            return;
        }
        SaveRec();
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT UM_CODE,UM_USERNAME,UM_PASSWORD,UM_LEVEL,UM_LASTLOGIN_DATETIME,UM_IP_ADDRESS,IS_ACTIVE,UM_IS_ADMIN,UM_NAME,UM_EMAIL FROM USER_MASTER WHERE UM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " AND UM_CM_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtUserID.Text = dt.Rows[0]["UM_USERNAME"].ToString();
                ddlUserlevel.SelectedValue = dt.Rows[0]["UM_LEVEL"].ToString();
                txtpass.Attributes.Add("value", CommonClasses.DecriptText(dt.Rows[0]["UM_PASSWORD"].ToString()));
                txtRepass.Attributes.Add("value", CommonClasses.DecriptText(dt.Rows[0]["UM_PASSWORD"].ToString()));
                txtusername.Text = dt.Rows[0]["UM_NAME"].ToString();
                txtUserEmail.Text = dt.Rows[0]["UM_EMAIL"].ToString();
                if (dt.Rows[0]["IS_ACTIVE"].ToString() == "True")
                {
                    ChkActive.Checked = true;
                }
                else
                {
                    ChkActive.Checked = false;
                }
                if (dt.Rows[0]["UM_IS_ADMIN"].ToString() == "True")
                {
                    ChkIsAdmin.Checked = true;
                }
                else
                {
                    ChkIsAdmin.Checked = false;
                }
               DataTable dtStoreSelect = CommonClasses.Execute("SELECT * FROM USER_STORE_DETAIL WHERE UM_CODE= '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                DataTable dtStoreOwner = CommonClasses.Execute("SELECT * FROM USER_STORE_OWNER WHERE UM_CODE= '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                DataTable dtStore = CommonClasses.Execute(" SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ES_DELETE=0 ");
                chkItems.Items.Clear();
                chkOwner.Items.Clear();
                for (int i = 0; i < dtStore.Rows.Count; i++)
                {
                    ListItem item = new ListItem();
                    ListItem item1 = new ListItem();
                    item.Text = dtStore.Rows[i]["STORE_NAME"].ToString();
                    item.Value = dtStore.Rows[i]["STORE_CODE"].ToString();
                    item1.Text = dtStore.Rows[i]["STORE_NAME"].ToString();
                    item1.Value = dtStore.Rows[i]["STORE_CODE"].ToString();
                    item.Selected = false;
                    item1.Selected = false; 
                    chkItems.Items.Add(item);
                    chkOwner.Items.Add(item1);
                }
                for (int j = 0; j < dtStoreSelect.Rows.Count; j++)
                {
                    foreach (ListItem li in chkItems.Items)
                    {
                        if (li.Value.ToString() == dtStoreSelect.Rows[j]["STORE_CODE"].ToString())
                        {
                            li.Selected = true;
                        }
                    }
                }
                for (int p = 0; p < dtStoreOwner.Rows.Count; p++)
                {
                    foreach (ListItem liO in chkOwner.Items)
                    {
                        if (liO.Value.ToString() == dtStoreOwner.Rows[p]["STORE_CODE"].ToString())
                        {
                            liO.Selected = true;
                        }
                    }
                }
                if (str == "VIEW")
                {
                    txtUserID.Enabled = false;
                    txtpass.Enabled = false;
                    ddlUserlevel.Enabled = false;
                    txtRepass.Enabled = false;
                    ChkActive.Enabled = false;
                    ChkIsAdmin.Enabled = false;
                    txtusername.Enabled = false;
                    txtUserEmail.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("USER_MASTER", "MODIFY", "UM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        PanelMsg.Visible = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (txtpass.Text == txtRepass.Text)
                {
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Correct Password";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select UM_CODE,UM_USERNAME FROM USER_MASTER WHERE lower(UM_USERNAME)= lower('" + txtUserID.Text.Trim() + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO USER_MASTER (UM_CM_ID,UM_USERNAME,UM_PASSWORD,UM_LEVEL,UM_LASTLOGIN_DATETIME,UM_IP_ADDRESS,IS_ACTIVE,UM_IS_ADMIN,UM_NAME,UM_EMAIL)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtUserID.Text.Trim() + "','" + CommonClasses.Encrypt(txtpass.Text) + "','" + ddlUserlevel.SelectedValue + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd hh:MM:ss tt") + "','" + GetIP4Address() + "','" + ChkActive.Checked + "','" + ChkIsAdmin.Checked + "','" + txtusername.Text.Trim() + "','" + txtUserEmail.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(UM_CODE) from USER_MASTER");
                        for (int i = 0; i < chkItems.Items.Count; i++)
                        {
                            if (chkItems.Items[i].Selected)
                            {
                                CommonClasses.Execute1("INSERT INTO USER_STORE_DETAIL (UM_CODE,STORE_CODE) VALUES ('" + Code + "' , '" + chkItems.Items[i].Value.ToString() + "')");
                            }
                        }
                        for (int i = 0; i < chkOwner.Items.Count; i++)
                        {
                            if (chkOwner.Items[i].Selected)
                            {
                                CommonClasses.Execute1("INSERT INTO USER_STORE_OWNER (UM_CODE,STORE_CODE) VALUES ('" + Code + "' , '" + chkOwner.Items[i].Value.ToString() + "')");
                            }
                        }
                        CommonClasses.WriteLog("User Master", "Save", "User Master", txtusername.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewUsers.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtUserID.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "User ID Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM USER_MASTER WHERE ES_DELETE='FALSE' AND UM_CODE!= '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND lower(UM_USERNAME) = lower('" + txtUserID.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE dbo.USER_MASTER SET UM_USERNAME='" + txtUserID.Text + "',UM_PASSWORD='" + CommonClasses.Encrypt(txtpass.Text) + "',UM_LEVEL='" + ddlUserlevel.SelectedValue + "',UM_LASTLOGIN_DATETIME='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd hh:MM:ss tt") + "',UM_IP_ADDRESS='" + GetIP4Address() + "',IS_ACTIVE='" + ChkActive.Checked + "',UM_IS_ADMIN='" + ChkIsAdmin.Checked + "',UM_NAME='" + txtusername.Text + "',UM_EMAIL='" + txtUserEmail.Text + "' WHERE UM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                    {
                        CommonClasses.Execute("DELETE FROM USER_STORE_DETAIL WHERE USER_STORE_DETAIL.UM_CODE = '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        for (int i = 0; i < chkItems.Items.Count; i++)
                        {
                            if (chkItems.Items[i].Selected == true)
                            {
                                CommonClasses.Execute1("INSERT INTO USER_STORE_DETAIL (UM_CODE,STORE_CODE) VALUES ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' , '" + chkItems.Items[i].Value.ToString() + "')");
                            }
                        }
                        CommonClasses.Execute("DELETE FROM USER_STORE_OWNER WHERE USER_STORE_OWNER.UM_CODE = '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        for (int i = 0; i < chkOwner.Items.Count; i++)
                        {
                            if (chkOwner.Items[i].Selected)
                            {
                                CommonClasses.Execute1("INSERT INTO USER_STORE_OWNER (UM_CODE,STORE_CODE) VALUES ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' , '" + chkOwner.Items[i].Value.ToString() + "')");
                            }
                        }
                        CommonClasses.RemoveModifyLock("dbo.USER_MASTER", "MODIFY", "UM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("User Master", "Update", "user Master", txtUserID.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewUsers.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtUserID.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "User ID Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtUserID.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        return res;
    }
    #endregion Setvalues

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        return res;
    }
    #endregion GetValues

    #region GetIP4Address
    string GetIP4Address()
    {
        string IP4Address = String.Empty;
        try
        {
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "GetIP4Address", ex.Message);
            return IP4Address;
        }
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void chkItems_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

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
