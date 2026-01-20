using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;

public partial class ToolRoom_VIEW_ViewToolRRefubrish : System.Web.UI.Page
{
    #region Variable Declaration
    DirectoryInfo ObjSearchDir;
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    string fileName = "";
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='176'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadTools();
                    ViewState["fileName"] = fileName;

                    if (dgWPM.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("TRR_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TRR_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TRR_STD_PROD", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TRR_REF_REV_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgWPM.DataSource = dtFilter;
                            dgWPM.DataBind();
                            dgWPM.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("TOOl ROOM REFURBISH", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadTools
    private void LoadTools()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select TRR_CODE,T_NAME,I_NAME,I_CODENO,case when TRR_TYPE=0 then 'REFURBISH' else 'SCRAP' end as TRR_TYPE,TRR_STD_PROD,TRR_REF_REV_NO from TOOLROOM_REFURBISH_MASTER,TOOL_MASTER,ITEM_MASTER where T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 AND TRR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 and TOOLROOM_REFURBISH_MASTER.ES_DELETE=0 and TOOL_MASTER.T_CODE=TOOLROOM_REFURBISH_MASTER.TRR_T_CODE ORDER BY TRR_CODE desc");
            if (dt.Rows.Count > 0)
            {
                dgWPM.DataSource = dt;
                dgWPM.DataBind();
            }
            if (dgWPM.Rows.Count > 0)
            {
                dgWPM.Enabled = true;
            }
            else
            {
                LoadStatus(txtString);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("TOOl ROOM REFURBISH", "LoadTools", Ex.Message);
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
            CommonClasses.SendError("TOOl ROOM REFURBISH", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select TRR_CODE,T_NAME,I_NAME,I_CODENO,case when TRR_TYPE=0 then 'REFURBISH' else 'SCRAP' end as TRR_TYPE,TRR_STD_PROD,TRR_REF_REV_NO from TOOLROOM_REFURBISH_MASTER,TOOL_MASTER,ITEM_MASTER where T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 AND TRR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 and TOOLROOM_REFURBISH_MASTER.ES_DELETE=0 and TOOL_MASTER.T_CODE=TOOLROOM_REFURBISH_MASTER.TRR_T_CODE and ((T_NAME like upper('%" + str + "%')) OR (I_NAME like upper('%" + str + "%')) OR (I_CODENO like upper('%" + str + "%'))) ORDER BY TRR_CODE desc");
            else
                dtfilter = CommonClasses.Execute("select TRR_CODE,T_NAME,I_NAME,I_CODENO,case when TRR_TYPE=0 then 'REFURBISH' else 'SCRAP' end as TRR_TYPE,TRR_STD_PROD,TRR_REF_REV_NO from TOOLROOM_REFURBISH_MASTER,TOOL_MASTER,ITEM_MASTER where T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 AND TRR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 and TOOLROOM_REFURBISH_MASTER.ES_DELETE=0 and TOOL_MASTER.T_CODE=TOOLROOM_REFURBISH_MASTER.TRR_T_CODE ORDER BY TRR_CODE desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgWPM.DataSource = dtfilter;
                dgWPM.DataBind();
                dgWPM.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRR_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRR_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRR_STD_PROD", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRR_REF_REV_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgWPM.DataSource = dtFilter;
                    dgWPM.DataBind();
                    dgWPM.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("TOOl ROOM REFURBISH", "LoadStatus", ex.Message);
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
                Response.Redirect("~/ToolRoom/ADD/ToolRRefurbish.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("TOOl ROOM REFURBISH", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("TOOl ROOM REFURBISH", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events
    protected void dgWPM_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgWPM.Rows[e.RowIndex].FindControl("lblTRR_CODE"))).Text))
                {
                    string TRR_CODE = ((Label)(dgWPM.Rows[e.RowIndex].FindControl("lblTRR_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE TOOLROOM_REFURBISH_MASTER SET ES_DELETE = 1 WHERE TRR_CODE='" + Convert.ToInt32(TRR_CODE) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("TOOl ROOM REFURBISH", "Delete", "TOOl ROOM REFURBISH", TRR_CODE, Convert.ToInt32(TRR_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
            CommonClasses.SendError("TOOl ROOM REFURBISH", "dgWPM_RowDeleting", Ex.Message);
        }
    }
    protected void dgWPM_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string TRR_CODE = ((Label)(dgWPM.Rows[e.NewEditIndex].FindControl("lblTRR_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/ToolRoom/ADD/ToolRRefurbish.aspx?c_name=" + type + "&p_code=" + TRR_CODE, false);
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
            CommonClasses.SendError("TOOl ROOM REFURBISH", "dgWPM_RowEditing", Ex.Message);
        }
    }

    protected void dgWPM_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgWPM.PageIndex = e.NewPageIndex;
            LoadTools();
        }
        catch (Exception)
        {
        }
    }

    protected void dgWPM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string TRR_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/ToolRoom/ADD/ToolRRefurbish.aspx?c_name=" + type + "&i_uom_code=" + TRR_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Print"))
                {

                    string TRR_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/ToolRoom/ADD/ToolRRefurbish.aspx?i_uom_code=" + TRR_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
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
                        string TRR_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/ToolRoom/ADD/ToolRRefurbish.aspx?c_name=" + type + "&p_code=" + TRR_CODE, false);
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
            #region UpdateStatus
            else if (e.CommandName.Equals("Status"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For UpdateStatus"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "UpdateStatus";
                        string um_code = e.CommandArgument.ToString();
                        int Index = Convert.ToInt32(e.CommandArgument.ToString());
                        GridViewRow row = dgWPM.Rows[Index];
                        string mt_month = ((Label)(row.FindControl("lblTRR_CODE"))).Text;
                        string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;

                        if (Status == "Close")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Have No Rights To Change Status";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        Response.Redirect("~/ToolRoom/ADD/ToolRRefurbish.aspx?c_name=" + type + "&u_code=" + mt_month, false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("TOOl ROOM REFURBISH", "GridView1_RowCommand", Ex.Message);
        }
    }

    protected void dgWPM_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select ES_MODIFY from TOOLROOM_REFURBISH_MASTER where TRR_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("TOOl ROOM REFURBISH", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("TOOl ROOM REFURBISH", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion
}

