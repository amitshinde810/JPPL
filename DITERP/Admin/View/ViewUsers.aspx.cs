using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_View_ViewUsers : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='2'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                    {
                        LoadUser();
                        dgUser.Enabled = true;

                        if (dgUser.Rows.Count == 0)
                        {

                            dtFilter.Clear();
                            if (dtFilter.Columns.Count == 0)
                            {
                                dtFilter.Columns.Add(new System.Data.DataColumn("UM_CODE", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("UM_USERNAME", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("QE_REV_NO", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("UM_NAME", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("UM_EMAIL", typeof(string)));

                                dtFilter.Rows.Add(dtFilter.NewRow());
                                dgUser.DataSource = dtFilter;
                                dgUser.DataBind();
                                dgUser.Enabled = false;
                            }
                        }
                    }
                    else
                    {

                        Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadUser
    private void LoadUser()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT UM_CODE,UM_USERNAME,UM_NAME,UM_EMAIL FROM USER_MASTER WHERE UM_CM_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 order by UM_USERNAME,UM_NAME");

            dgUser.DataSource = dt;
            dgUser.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master", "LoadUser", Ex.Message);
        }
    }
    #endregion

    #region GridView1_RowEditing
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgUser.Rows[e.NewEditIndex].FindControl("lblUM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/UserMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region GridView1_RowDeleting
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgUser.Rows[e.RowIndex].FindControl("lblUM_CODE"))).Text))
                {
                    //BL_UserMaster = new UserMaster_BL();
                    string um_code = ((Label)(dgUser.Rows[e.RowIndex].FindControl("lblUM_CODE"))).Text;
                    string um_name = ((Label)(dgUser.Rows[e.RowIndex].FindControl("lblUserID"))).Text;

                    DataTable dtAdmin = CommonClasses.Execute("SELECT UM_IS_ADMIN FROM USER_MASTER WHERE ES_DELETE=0 AND UM_CM_ID=" + Session["CompanyId"] + " AND UM_CODE = " + um_code + " ");
                    if (Convert.ToBoolean(dtAdmin.Rows[0]["UM_IS_ADMIN"]) != true)
                    {
                        bool flag = CommonClasses.Execute1("UPDATE USER_MASTER SET ES_DELETE = 1 WHERE UM_CODE='" + Convert.ToInt32(um_code) + "'");

                        if (flag == true)
                        {
                            CommonClasses.WriteLog("User Master", "Delete", "User Master", um_name, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        }
                        LoadUser();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Admin Can Not Be Delete";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            }
            else
            {
                ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from USER_MASTER where UM_CODE=" + PrimaryKey + "  ");
            if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another User";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return true;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
        return false;
    }
    #endregion

    #region GridView1_RowUpdating
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region GridView1_RowCommand
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "VIEW";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/UserMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/UserMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowCommand", exc.Message);
        }
    }
    #endregion

    #region GridView1_PageIndexChanging
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgUser.PageIndex = e.NewPageIndex;
            LoadUser();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Admin/Add/UserMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region dgUser_SelectedIndexChanged
    protected void dgUser_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master - View", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();
            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT UM_CODE,UM_USERNAME,UM_NAME,UM_EMAIL FROM USER_MASTER WHERE ES_DELETE='FALSE' AND UM_CM_ID = '" + Session["CompanyId"] + "' and (lower(UM_USERNAME) like lower('%" + str + "%') or lower(UM_NAME) like lower('%" + str + "%') or  lower(UM_EMAIL) like lower('%" + str + "%'))order by UM_USERNAME,UM_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT UM_CODE,UM_USERNAME,UM_NAME,UM_EMAIL FROM USER_MASTER WHERE ES_DELETE='FALSE' AND UM_CM_ID = '" + Session["CompanyId"] + "' order by UM_USERNAME,UM_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgUser.Enabled = true;
                dgUser.DataSource = dtfilter;
                dgUser.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("UM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UM_USERNAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("QE_REV_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UM_EMAIL", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgUser.DataSource = dtFilter;
                    dgUser.DataBind();
                    dgUser.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master - View", "LoadStatus", Ex.Message);
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

}
