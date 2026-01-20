using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewStateMaster : System.Web.UI.Page
{
    #region Variables
    StateMaster_BL BL_StateMaster = null;
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='6'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadState();
                    dgState.Enabled = true;
                    if (dgState.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("SM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SM_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgState.DataSource = dtFilter;
                            dgState.DataBind();
                            dgState.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("State Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadState
    private void LoadState()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE=0 order by SM_NAME,COUNTRY_NAME");

            dgState.DataSource = dt;
            dgState.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "LoadState", Ex.Message);
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
            CommonClasses.SendError("State Master", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE='0' and (SM_NAME like upper('%" + str + "%') OR COUNTRY_NAME like upper('%" + str + "%')) order by SM_NAME,COUNTRY_NAME");
            else
                dtfilter = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE='0' order by SM_NAME,COUNTRY_NAME ");

            if (dtfilter.Rows.Count > 0)
            {
                dgState.Enabled = true;
                dgState.DataSource = dtfilter;
                dgState.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgState.DataSource = dtFilter;
                    dgState.DataBind();
                    dgState.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master", "LoadStatus", ex.Message);
        }
    }
    #endregion

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Admin/Add/StateMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events

    protected void dgState_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgState.Rows[e.RowIndex].FindControl("lblSM_CODE"))).Text))
                {

                    string s_code = ((Label)(dgState.Rows[e.RowIndex].FindControl("lblSM_CODE"))).Text;
                    string s_name = ((Label)(dgState.Rows[e.RowIndex].FindControl("lblSTATE_NAME"))).Text;

                    if (CommonClasses.CheckUsedInTran("CITY_MASTER", "CITY_SM_CODE", "AND ES_DELETE=0", s_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record becouse it has used in city master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE STATE_MASTER SET ES_DELETE = 1 WHERE SM_CODE='" + Convert.ToInt32(s_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("State Master", "Delete", "State Master", s_code, Convert.ToInt32(s_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                    LoadState();
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "dgState_RowDeleting", Ex.Message);
        }
    }
    protected void dgState_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string s_code = ((Label)(dgState.Rows[e.NewEditIndex].FindControl("lblSM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/StateMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "dgState_RowEditing", Ex.Message);
        }
    }
    protected void dgState_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void dgState_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string s_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/StateMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
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
                        string s_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/StateMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "dgState_RowCommand", Ex.Message);
        }
    }
    protected void dgState_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from STATE_MASTER where SM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "ModifyLog", Ex.Message);
        }
        return false;
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
            CommonClasses.SendError("State Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
