using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;

public partial class Admin_Add_Test : System.Web.UI.Page
{
    # region Variables
    //UserMaster_BL BL_UserMaster = null;
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
                        //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='2'");
                        //right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                        txtusername.Text = "";
                        txtpass.Text = "";
                        txtUserEmail.Text = "";
                        // LoadBranches();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            // BL_UserMaster = new UserMaster_BL();
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            // BL_UserMaster = new UserMaster_BL();
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }

                        LoadchkStore();


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
        }


        //Console.WriteLine(dtStore);
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
            //else if (txtUserEmail.Text.Trim() == "")
            //{
            //    flag = false;
            //}
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
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("USER_MASTER", "MODIFY", "UM_CODE", mlCode);
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
        //if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
        //{
        SaveRec();
        //}
        //else
        //{

        //}
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            // BL_UserMaster = new UserMaster_BL(mlCode);

            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT UM_CODE,UM_USERNAME,UM_PASSWORD,UM_LEVEL,UM_LASTLOGIN_DATETIME,UM_IP_ADDRESS,IS_ACTIVE,UM_IS_ADMIN,UM_NAME,UM_EMAIL FROM USER_MASTER WHERE UM_CODE=" + mlCode + " AND UM_CM_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

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
                CommonClasses.SetModifyLock("USER_MASTER", "MODIFY", "UM_CODE", mlCode);
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
                    //ShowMessage("#Avisos", "Please Enter Correct Password", CommonClasses.MSG_Warning);
                    return false;
                }
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select UM_CODE,UM_USERNAME FROM USER_MASTER WHERE lower(UM_USERNAME)= lower('" + txtUserID.Text.Trim() + "') and ES_DELETE='False'");
                //result = CommonClasses.CheckExistSave("PARTY_MASTER", "P_NAME",txtCustomerName.Text.Trim());
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
                            foreach (ListItem li in chkItems.Items)
                            {
                                if (li.Selected)
                                {

                                }
                            }
                        }
                        CommonClasses.WriteLog("User Master", "Save", "User Master", txtusername.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewUsers.aspx", false);
                    }
                    else
                    {
                        //PanelMsg.Visible = true;
                        //lblmsg.Text = "Could not saved";
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtUserID.Focus();
                    }
                }
                else
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "User ID Already Exists";
                    ShowMessage("#Avisos", "User ID Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM USER_MASTER WHERE ES_DELETE='FALSE' AND UM_CODE!= '" + mlCode + "' AND lower(UM_USERNAME) = lower('" + txtUserID.Text + "')");
                //result = CommonClasses.CheckExistUpdate("PARTY_MASTER", "P_NAME", txtCustomerName.Text, "P_CODE", mlCode);
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE dbo.USER_MASTER SET UM_USERNAME='" + txtUserID.Text + "',UM_PASSWORD='" + CommonClasses.Encrypt(txtpass.Text) + "',UM_LEVEL='" + ddlUserlevel.SelectedValue + "',UM_LASTLOGIN_DATETIME='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd hh:MM:ss tt") + "',UM_IP_ADDRESS='" + GetIP4Address() + "',IS_ACTIVE='" + ChkActive.Checked + "',UM_IS_ADMIN='" + ChkIsAdmin.Checked + "',UM_NAME='" + txtusername.Text + "',UM_EMAIL='" + txtUserEmail.Text + "' WHERE UM_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.Execute("DELETE FROM USER_STORE_DETAIL WHERE USER_STORE_DETAIL.UM_CODE = '" + mlCode + "'");
                        for (int i = 0; i < chkItems.Items.Count; i++)
                        {
                            if (chkItems.Items[i].Selected == true)
                            {

                                CommonClasses.Execute1("INSERT INTO USER_STORE_DETAIL (UM_CODE,STORE_CODE) VALUES ('" + mlCode + "' , '" + chkItems.Items[i].Value.ToString() + "')");
                            }
                        }
                        CommonClasses.RemoveModifyLock("dbo.USER_MASTER", "MODIFY", "UM_CODE", mlCode);
                        CommonClasses.WriteLog("User Master", "Update", "user Master", txtUserID.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "User ID Already Exists";
                    ShowMessage("#Avisos", "User ID Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtUserID.Focus();
                }
                //if (BL_UserMaster.Update())
                //{
                //    CommonClasses.RemoveModifyLock("CM_USER_MASTER", "UM_MODIFY_FLAG", "UM_CODE", mlCode);
                //    CommonClasses.WriteLog("Admin User Master", "Update", "Admin User Master", BL_UserMaster.UM_USERNAME, mlCode, Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                //    result = true;
                //    Response.Redirect("~/Admin/VIEW/ViewUsers.aspx", false);
                //}
                //else
                //{
                //    if (BL_UserMaster.Msg != "")
                //    {
                //        //lblmsg.Visible = true;
                //        //lblmsg.Text = BL_UserMaster.Msg;
                //        BL_UserMaster.Msg = "";
                //    }

                //}
                // }
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
        //try
        //{
        //    BL_UserMaster.UM_CM_ID = Convert.ToInt32(Session["CompanyId"]);
        //    BL_UserMaster.UM_USERNAME = txtusername.Text.ToUpper();
        //    BL_UserMaster.UM_EMAIL = txtUserEmail.Text.ToUpper(); 
        //    BL_UserMaster.UM_PASSWORD = CommonClasses.Encrypt(txtpass.Text);
        //    BL_UserMaster.UM_LEVEL = ddlUserlevel.Text;

        //    BL_UserMaster.UM_LASTLOGIN_DATETIME = DateTime.Now;
        //    BL_UserMaster.UM_IP_ADDRESS = GetIP4Address();
        //    BL_UserMaster.UM_ACTIVE_IND = ChkActive.Checked;
        //    BL_UserMaster.UM_IS_ADMIN = ChkIsAdmin.Checked;
        //    res = true;
        //}
        //catch (Exception ex)
        //{
        //    CommonClasses.SendError("User Master", "Setvalues", ex.Message);
        //}
        return res;
    }
    #endregion Setvalues

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        //try
        //{

        //    ddlUserlevel.Text = BL_UserMaster.UM_LEVEL;
        //    txtUsername.Text = BL_UserMaster.UM_USERNAME;
        //    txtpass.Attributes.Add("value", CommonClasses.DecriptText(BL_UserMaster.UM_PASSWORD));
        //    txtRepass.Attributes.Add("value", CommonClasses.DecriptText(BL_UserMaster.UM_PASSWORD));
        //    ChkActive.Checked = BL_UserMaster.UM_ACTIVE_IND;
        //    ChkIsAdmin.Checked = BL_UserMaster.UM_IS_ADMIN;
        //    int BM_CODE = Convert.ToInt32(BL_UserMaster.UM_BM_CODE);
        //    int Emp = BL_UserMaster.UM_EM_CODE;
        //    LoadBranches();
        //    ddlBranch.SelectedValue = BM_CODE.ToString();
        //    LoadEmployee();
        //    ddlEmployeeName.SelectedValue = Emp.ToString();
        //    DataTable dt = CommonClasses.Execute("SELECT UMD_BM_CODE,BM_NAME FROM CM_USER_MASTER_DETAIL,CM_BRANCH_MASTER WHERE UMD_BM_CODE = BM_CODE AND UMD_UM_CODE = " + mlCode + "");
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        foreach (ListItem li in chkItems.Items)
        //        {
        //            if (li.Value.ToString() == dt.Rows[i]["UMD_BM_CODE"].ToString())
        //                li.Selected = true;
        //        }
        //    }
        //    DataTable dtnoti = CommonClasses.Execute("SELECT UND_NM_CODE FROM CM_USER_MASTER_NOTIFICATION,CM_USER_MASTER WHERE UND_UM_CODE = UM_CODE AND UM_CODE = " + mlCode + "");
        //    for (int j = 0; j < dtnoti.Rows.Count; j++)
        //    {
        //        foreach (ListItem li in CheckBoxListNoti.Items)
        //        {
        //            if (li.Value.ToString() == dtnoti.Rows[j]["UND_NM_CODE"].ToString())
        //                li.Selected = true;
        //        }
        //    }
        //    if (str == "VIEW")
        //    {
        //        ddlBranch.Enabled = false;
        //        ddlEmployeeName.Enabled = false;
        //        txtUsername.Enabled = false;
        //        txtpass.Enabled = false;
        //        ddlUserlevel.Enabled = false;
        //        txtRepass.Enabled = false;
        //        ChkActive.Enabled = false;
        //        ChkIsAdmin.Enabled = false;
        //        btnSubmit.Visible = false;
        //        chkItems.Enabled = false;
        //        CheckBoxListNoti.Enabled = false;
        //    }
        //    res = true;
        //}
        //catch (Exception ex)
        //{
        //    CommonClasses.SendError("User Master", "GetValues", ex.Message);
        //}
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

    //

    protected void chkItems_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }

    #endregion
}
