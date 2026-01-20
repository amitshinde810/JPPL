using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_VIEW_MonthlyPlannedPreventiveMaintenanceDIE : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='169'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadTools();
                    if (dgMPPM.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("PM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("PM_MONTH", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("PM_WEEK", typeof(String)));
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadTools
    private void LoadTools()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select PM_CODE,T_NAME,I_CODENO,I_NAME,LEFT(DATENAME(MONTH, PM_MONTH), 3) + ' ' + DATENAME(YEAR, PM_MONTH) as PM_MONTH,(CASE PM_WEEK when '0' then '1 WEEK' when '1' then '2 WEEK' when '2' then '3 WEEK' when '3' then '4 WEEK' else 'PM_WEEK' END) as PM_WEEK from MP_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and PM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.T_CODE=MP_PREVENTIVE_MAINTENANCE_MASTER.PM_TOOL_NO and TOOL_MASTER.ES_DELETE=0 and I_CODE=T_I_CODE and T_I_CODE=PM_I_CODE and ITEM_MASTER.ES_DELETE=0 ORDER BY PM_CODE desc");

            dgMPPM.DataSource = dt;
            dgMPPM.DataBind();
            if (dgMPPM.Rows.Count > 0)
            {
                dgMPPM.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "LoadTools", Ex.Message);
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select PM_CODE,T_NAME,I_CODENO,I_NAME,LEFT(DATENAME(MONTH, PM_MONTH), 3) + ' ' + DATENAME(YEAR, PM_MONTH) as PM_MONTH,(CASE PM_WEEK when '0' then '1 WEEK' when '1' then '2 WEEK' when '2' then '3 WEEK' when '3' then '4 WEEK' else 'PM_WEEK' END) as PM_WEEK from MP_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and PM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.T_CODE=MP_PREVENTIVE_MAINTENANCE_MASTER.PM_TOOL_NO and TOOL_MASTER.ES_DELETE=0 and I_CODE=T_I_CODE and T_I_CODE=PM_I_CODE and ITEM_MASTER.ES_DELETE=0 and (T_NAME like upper('%" + str + "%') OR I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) ORDER BY PM_CODE desc");
            else
                dtfilter = CommonClasses.Execute("select PM_CODE,T_NAME,I_CODENO,I_NAME,LEFT(DATENAME(MONTH, PM_MONTH), 3) + ' ' + DATENAME(YEAR, PM_MONTH) as PM_MONTH,(CASE PM_WEEK when '0' then '1 WEEK' when '1' then '2 WEEK' when '2' then '3 WEEK' when '3' then '4 WEEK' else 'PM_WEEK' END) as PM_WEEK from MP_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,ITEM_MASTER where MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and PM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.T_CODE=MP_PREVENTIVE_MAINTENANCE_MASTER.PM_TOOL_NO and TOOL_MASTER.ES_DELETE=0 and I_CODE=T_I_CODE and T_I_CODE=PM_I_CODE and ITEM_MASTER.ES_DELETE=0 ORDER BY PM_CODE desc");

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
                    dtFilter.Columns.Add(new System.Data.DataColumn("PM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PM_MONTH", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PM_WEEK", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMPPM.DataSource = dtFilter;
                    dgMPPM.DataBind();
                    dgMPPM.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "LoadStatus", ex.Message);
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
                Response.Redirect("~/ToolRoom/ADD/MonthlyPlannedPreventiveMaintenanceDIE.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "btnCancel_Click", Ex.Message);
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
                if (!ModifyLog(((Label)(dgMPPM.Rows[e.RowIndex].FindControl("lblPM_CODE"))).Text))
                {

                    string PM_CODE = ((Label)(dgMPPM.Rows[e.RowIndex].FindControl("lblPM_CODE"))).Text;

                    string PM_CODE1 = ((Label)(dgMPPM.Rows[e.RowIndex].FindControl("lblT_NAME"))).Text;
                    string PM_MONTH = ((Label)(dgMPPM.Rows[e.RowIndex].FindControl("lblPM_MONTH"))).Text;
                    DateTime dtmonth = Convert.ToDateTime(PM_MONTH);

                    DataTable dttool = CommonClasses.Execute("SELECT * FROM TOOL_MASTER where ES_DELETE=0 AND T_NAME='" + PM_CODE1 + "'");
                    if (CommonClasses.CheckUsedInTran("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", "WPM_T_CODE", "AND ES_DELETE=0 AND WPM_MONTH='" + dtmonth.ToString("dd/MMM/yyyy") + "'", dttool.Rows[0]["T_CODE"].ToString()))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Weekly Preventive Maintenance";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    bool flag = CommonClasses.Execute1("UPDATE MP_PREVENTIVE_MAINTENANCE_MASTER SET ES_DELETE = 1 WHERE  PM_CODE='" + Convert.ToInt32(PM_CODE) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Monthly Planned Preventive maintenance", "Delete", "Monthly Planned Preventive maintenance", PM_CODE, Convert.ToInt32(PM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "dgMPPM_RowDeleting", Ex.Message);
        }
    }
    protected void dgMPPM_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string PM_CODE = ((Label)(dgMPPM.Rows[e.NewEditIndex].FindControl("lblPM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/ToolRoom/ADD/MonthlyPlannedPreventiveMaintenanceDIE.aspx?c_name=" + type + "&p_code=" + PM_CODE, false);
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "dgMPPM_RowEditing", Ex.Message);
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
                    string PM_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/ToolRoom/ADD/MonthlyPlannedPreventiveMaintenanceDIE.aspx?c_name=" + type + "&i_uom_code=" + PM_CODE, false);
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
                        string PM_CODE = e.CommandArgument.ToString();

                        if (CommonClasses.CheckUsedInTran("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER", "PM_CODE", "AND WPM_T_CODE=PM_TOOL_NO and WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0", PM_CODE))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't modify this record it has used in Weekly Preventive Maintenance";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        Response.Redirect("~/ToolRoom/ADD/MonthlyPlannedPreventiveMaintenanceDIE.aspx?c_name=" + type + "&p_code=" + PM_CODE, false);
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "GridView1_RowCommand", Ex.Message);
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
            DataTable dt = CommonClasses.Execute("select ES_MODIFY from MP_PREVENTIVE_MAINTENANCE_MASTER where PM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Monthly Planned Preventive maintenance", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
