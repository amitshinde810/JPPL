using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class ToolRoom_VIEW_TDModelStorageNReqForRevision : System.Web.UI.Page
{
    #region Variable Declaration
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='198'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadTools();
                    if (dgMPPM.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("TMS_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TMS_REV_DATE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgMPPM.DataSource = dtFilter;
                            dgMPPM.DataBind();
                            dgMPPM.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadTools
    private void LoadTools()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select TMS_CODE,T_NAME,I_CODENO,I_NAME,convert(varchar,TMS_REV_DATE,106) as TMS_REV_DATE from TDMODELSTORAGE_MASTER INNER JOIN TOOL_MASTER ON TDMODELSTORAGE_MASTER.TMS_T_CODE = TOOL_MASTER.T_CODE AND TDMODELSTORAGE_MASTER.TMS_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE where TDMODELSTORAGE_MASTER.ES_DELETE=0 and TMS_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 ORDER BY T_NAME");

            dgMPPM.DataSource = dt;
            dgMPPM.DataBind();
            if (dgMPPM.Rows.Count > 0)
            {
                dgMPPM.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "LoadTools", Ex.Message);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("select TMS_CODE,T_NAME,I_CODENO,I_NAME,convert(varchar,TMS_REV_DATE,106) as TMS_REV_DATE from TDMODELSTORAGE_MASTER INNER JOIN TOOL_MASTER ON TDMODELSTORAGE_MASTER.TMS_T_CODE = TOOL_MASTER.T_CODE AND TDMODELSTORAGE_MASTER.TMS_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE where TDMODELSTORAGE_MASTER.ES_DELETE=0 and TMS_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and (T_NAME like upper('%" + str + "%') OR I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) order by T_NAME");
            else
                dtfilter = CommonClasses.Execute("select TMS_CODE,T_NAME,I_CODENO,I_NAME,convert(varchar,TMS_REV_DATE,106) as TMS_REV_DATE from TDMODELSTORAGE_MASTER INNER JOIN TOOL_MASTER ON TDMODELSTORAGE_MASTER.TMS_T_CODE = TOOL_MASTER.T_CODE AND TDMODELSTORAGE_MASTER.TMS_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE where TDMODELSTORAGE_MASTER.ES_DELETE=0 and TMS_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 ORDER BY T_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgMPPM.DataSource = dtfilter;
                dgMPPM.DataBind();
                dgMPPM.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("TMS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TMS_REV_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMPPM.DataSource = dtFilter;
                    dgMPPM.DataBind();
                    dgMPPM.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "LoadStatus", ex.Message);
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
                Response.Redirect("~/ToolRoom/ADD/TDModelStorageNReqForRevision1.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events
    protected void dgMPPM_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgMPPM.Rows[e.RowIndex].FindControl("lblTMS_CODE"))).Text))
                {

                    string TMS_CODE = ((Label)(dgMPPM.Rows[e.RowIndex].FindControl("lblTMS_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE TDMODELSTORAGE_MASTER SET ES_DELETE = 1 WHERE TMS_CODE='" + Convert.ToInt32(TMS_CODE) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("3D Model Storage and Request For Revision", "Delete", "3D Model Storage and Request For Revision", TMS_CODE, Convert.ToInt32(TMS_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadTools();
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
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "dgMPPM_RowDeleting", Ex.Message);
        }
    }
    protected void dgMPPM_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string TMS_CODE = ((Label)(dgMPPM.Rows[e.NewEditIndex].FindControl("lblTMS_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/ToolRoom/ADD/MonthlyPlannedPreventiveMaintenanceDIE.aspx?c_name=" + type + "&p_code=" + TMS_CODE, false);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "dgMPPM_RowEditing", Ex.Message);
        }
    }
    protected void dgMPPM_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgMPPM.PageIndex = e.NewPageIndex;
            LoadTools();
        }
        catch (Exception)
        {
        }
    }
    protected void dgMPPM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string TMS_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/ToolRoom/ADD/TDModelStorageNReqForRevision1.aspx?c_name=" + type + "&i_uom_code=" + TMS_CODE, false);
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
                        string TMS_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/ToolRoom/ADD/TDModelStorageNReqForRevision1.aspx?c_name=" + type + "&p_code=" + TMS_CODE, false);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "GridView1_RowCommand", Ex.Message);
        }
    }

    protected void dgMPPM_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select ES_MODIFY from TDMODELSTORAGE_MASTER where TMS_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
