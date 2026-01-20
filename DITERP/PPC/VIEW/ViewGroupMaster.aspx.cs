using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewGroupMaster : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    #region PageLoad
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='180'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadGroup();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Group Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadGroup
    private void LoadGroup()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT GP_CODE,GP_NAME,GP_DESC,UM_NAME as GP_OWNER,UM_USERNAME FROM GROUP_MASTER inner join USER_MASTER on GP_OWNER=UM_CODE where USER_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0 and GP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER by GP_NAME");
            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgGroupMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_OWNER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgGroupMaster.DataSource = dtFilter;
                    dgGroupMaster.DataBind();
                }
            }
            else
            {
                dgGroupMaster.Enabled = true;
                dgGroupMaster.DataSource = dt;
                dgGroupMaster.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Group Master", "LoadUser", Ex.Message);
        }
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
            CommonClasses.SendError("Group Master- View", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT GP_CODE,GP_NAME,GP_DESC,UM_NAME as GP_OWNER,UM_USERNAME FROM GROUP_MASTER inner join USER_MASTER on GP_OWNER=UM_CODE where USER_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0 and GP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  and (lower(GP_NAME) like lower('%" + str + "%') or lower(GP_DESC) like lower('%" + str + "%')) order by GP_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT GP_CODE,GP_NAME,GP_DESC,UM_NAME as GP_OWNER,UM_USERNAME FROM GROUP_MASTER inner join USER_MASTER on GP_OWNER=UM_CODE where USER_MASTER.ES_DELETE=0 and GROUP_MASTER.ES_DELETE=0 and GP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by GP_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgGroupMaster.Enabled = true;
                dgGroupMaster.DataSource = dtfilter;
                dgGroupMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgGroupMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("GP_OWNER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgGroupMaster.DataSource = dtFilter;
                    dgGroupMaster.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Group Master - View", "LoadStatus", Ex.Message);
        }
    }
    #endregion

    #region btnInsert
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/PPC/ADD/GroupMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Group Master", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region GridView1_PageIndexChanging
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgGroupMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Group Master", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    #endregion GridView1_PageIndexChanging

    #region GridView1_RowCommand
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                return;
            }
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string um_code = e.CommandArgument.ToString();
                    Response.Redirect("~/PPC/ADD/GroupMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion View

            #region Modify
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/PPC/ADD/GroupMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            #endregion Modify
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Group Master", "GridView1_RowCommand", exc.Message);
        }
    }
    #endregion GridView1_RowCommand

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from GROUP_MASTER where GP_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Group Master", "GridView1_RowEditing", exc.Message);
        }
        return false;
    }
    #endregion

    #region GridView1_RowDeleting
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgGroupMaster.Rows[e.RowIndex].FindControl("lblGP_CODE"))).Text))
                {
                    string um_code = ((Label)(dgGroupMaster.Rows[e.RowIndex].FindControl("lblGP_CODE"))).Text;

                    DataTable DtDetails = CommonClasses.Execute(" SELECT * FROM PRODUCT_MASTER where   ES_DELETE=0 AND PROD_GP_CODE='" + um_code + "'");

                    if (DtDetails.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can Not Delete ,This is Used In Product Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    bool flag = CommonClasses.Execute1("UPDATE GROUP_MASTER SET ES_DELETE = 1 WHERE GP_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Group Master", "Delete", "Group Master", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadGroup();
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record deleted Successfully...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Group Master", "GridView1_RowEditing", exc.Message);
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

    #region GridView1_RowEditing
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgGroupMaster.Rows[e.NewEditIndex].FindControl("lblGP_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/PPC/ADD/GroupMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
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
            CommonClasses.SendError("Group Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PPCDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Group Master", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
