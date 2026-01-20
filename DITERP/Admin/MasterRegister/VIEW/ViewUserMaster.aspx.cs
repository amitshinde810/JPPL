using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_MasterRegister_VIEW_ViewUserMaster : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='27'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                LoadUser();
                ddlUserName.Enabled = false;
                ChkUserNameAll.Checked = true;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadUser
    private void LoadUser()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select UM_CODE,UM_NAME FROM USER_MASTER WHERE UM_CM_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ");

            ddlUserName.DataSource = dt;
            ddlUserName.DataTextField = "UM_NAME";
            ddlUserName.DataValueField = "UM_CODE";
            ddlUserName.DataBind();
            ddlUserName.Items.Insert(0, new ListItem("User Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
            {
                if (ChkUserNameAll.Checked == false)
                {
                    if (ddlUserName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select User Name";
                        return;
                    }
                }
                if (ChkUserNameAll.Checked == true)
                {
                    Response.Redirect("~/Admin/MasterRegister/ADD/AddUserMaster.aspx?Title=" + Title + "&ChkUserNameAll=" + ChkUserNameAll.Checked.ToString() + "&User_Name=" + ddlUserName.SelectedValue.ToString() + "", false);
                }
                if (ChkUserNameAll.Checked == false)
                {
                    Response.Redirect("~/Admin/MasterRegister/ADD/AddUserMaster.aspx?Title=" + Title + "&ChkUserNameAll=" + ChkUserNameAll.Checked.ToString() + "&User_Name=" + ddlUserName.SelectedValue.ToString() + "", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Please Select User Name", CommonClasses.MSG_Warning);
                    return;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To View";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnShow_Click", Ex.Message);
        }
    }

    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("User Master Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ChkUserNameAll_CheckedChanged
    protected void ChkUserNameAll_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkUserNameAll.Checked == true)
        {
            ddlUserName.SelectedIndex = 0;
            ddlUserName.Enabled = false;
        }
        else
        {
            ddlUserName.SelectedIndex = 0;
            ddlUserName.Enabled = true;
            ddlUserName.Focus();
        }
    }
    #endregion
}
