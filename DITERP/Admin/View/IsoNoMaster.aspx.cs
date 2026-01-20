using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_View_IsoNoMaster : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='80'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadStatus(txtString);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ISO Master", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("ISO Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadISO
    private void LoadISO()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT ISO_CODE,convert(varchar,ISO_WEF_DATE,106) as ISO_WEF_DATE,ISO_NO,SM_NAME FROM ISONO_MASTER,SCREEN_MASTER WHERE ISO_SCREEN_NO=SM_CODE and ISO_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='0' order by ISO_WEF_DATE ");
            dgISODet.DataSource = dt;
            dgISODet.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ISO Master", "LoadISO", Ex.Message);
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
            CommonClasses.SendError("ISO Master", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT ISO_CODE,convert(varchar,ISO_WEF_DATE,106) as ISO_WEF_DATE,ISO_NO,SM_NAME FROM ISONO_MASTER,SCREEN_MASTER WHERE ISO_SCREEN_NO=SM_CODE and ISO_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='0' and (convert(varchar,ISO_WEF_DATE,106) as ISO_WEF_DATE like upper('%" + str + "%') OR ISO_NO like upper('%" + str + "%') OR SM_NAME like upper('%" + str + "%')) order by ISO_WEF_DATE");
            else
                dtfilter = CommonClasses.Execute("SELECT ISO_CODE,convert(varchar,ISO_WEF_DATE,106) as ISO_WEF_DATE,ISO_NO,SM_NAME FROM ISONO_MASTER,SCREEN_MASTER WHERE ISO_SCREEN_NO=SM_CODE and ISO_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='0' order by ISO_WEF_DATE ");

            if (dtfilter.Rows.Count > 0)
            {
                dgISODet.Enabled = true;
                dgISODet.DataSource = dtfilter;
                dgISODet.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ISO_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ISO_WEF_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ISO_NO", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgISODet.DataSource = dtFilter;
                    dgISODet.DataBind();
                    dgISODet.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("ISO Master", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Admin/Add/ISOMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ISO Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events

    protected void dgISODet_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgISODet.Rows[e.RowIndex].FindControl("lblISO_CODE"))).Text))
                {
                    string s_code = ((Label)(dgISODet.Rows[e.RowIndex].FindControl("lblISO_CODE"))).Text;
                    string s_name = ((Label)(dgISODet.Rows[e.RowIndex].FindControl("lblISO_NO"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE ISONO_MASTER SET ES_DELETE = 1 WHERE ISO_CODE='" + Convert.ToInt32(s_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("ISO Master", "Delete", "ISO Master", s_code, Convert.ToInt32(s_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadISO();
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
            CommonClasses.SendError("ISO Master", "dgISODet_RowDeleting", Ex.Message);
        }
    }
    protected void dgISODet_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string s_code = ((Label)(dgISODet.Rows[e.NewEditIndex].FindControl("lblISO_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/ISOMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
            CommonClasses.SendError("ISO Master", "dgISODet_RowEditing", Ex.Message);
        }
    }
    protected void dgISODet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
    }

    protected void dgISODet_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Response.Redirect("~/Admin/Add/ISOMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
                        Response.Redirect("~/Admin/Add/ISOMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
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
            CommonClasses.SendError("ISO Master", "dgISODet_RowCommand", Ex.Message);
        }
    }
    protected void dgISODet_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from ISONO_MASTER where ISO_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("ISO Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("ISO Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}