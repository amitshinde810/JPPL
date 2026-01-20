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

public partial class ToolRoom_VIEW_ViewImprovement : System.Web.UI.Page
{
    #region Declaration
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='197'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadIssue();
                    if (dgBreakdown.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("B_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("B_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("B_SLIPNO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("B_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("B_FILE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("B_STATUS", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgBreakdown.DataSource = dtFilter;
                            dgBreakdown.DataBind();
                            dgBreakdown.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Improvement Entry", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadIssue
    private void LoadIssue()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT B_CODE, B_T_CODE,B_SLIPNO, B_NO,CONVERT(varchar, B_DATE,106) AS B_DATE, B_REASON, B_ACTION, B_CLOSURE,CASE WHEN B_STATUS=1 then '1' Else '0' END  AS B_STATUS, B_HOURS, B_FILE, T_NAME, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE, I_CODENO +' - '+ I_NAME AS I_CODENO FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE AND BREAKDOWN_ENTRY.B_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=1 AND (B_CM_CODE = " + (string)Session["CompanyCode"] + ") ORDER BY B_CODE DESC");
            dgBreakdown.DataSource = dt;
            dgBreakdown.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Improvement Entry", "Improvement Entry", Ex.Message);
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
            CommonClasses.SendError("Improvement Entry", "txtString_TextChanged", Ex.Message);
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

                dtfilter = CommonClasses.Execute("SELECT B_CODE, B_T_CODE,B_SLIPNO, B_NO, CONVERT(varchar, B_DATE,106) AS B_DATE, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE, T_NAME,CASE WHEN B_STATUS=1 then '1' Else '0' END  AS B_STATUS, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,I_CODENO +' - '+ I_NAME AS I_CODENO FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE AND BREAKDOWN_ENTRY.B_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=1 and B_CM_CODE=" + Session["CompanyCode"] + " and (upper(B_NO) like upper('%" + str + "%') OR convert(varchar,B_DATE,106) like upper('%" + str + "%') OR upper(B_TYPE) like upper('%" + str + "%')   OR  I_CODE like '%" + str + "%'  OR T_NAME like '%" + str + "%' OR I_CODENO like '%" + str + "%' OR I_NAME like '%" + str + "%') order by B_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT B_CODE, B_T_CODE,B_SLIPNO, B_NO, CONVERT(varchar, B_DATE,106) AS B_DATE, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE, T_NAME,CASE WHEN B_STATUS=1 then '1' Else '0' END  AS B_STATUS, CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END AS T_TYPE,I_CODENO +' - '+ I_NAME AS I_CODENO FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE AND BREAKDOWN_ENTRY.B_I_CODE = TOOL_MASTER.T_I_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0) AND B_TYPE=1 and B_CM_CODE=" + Session["CompanyCode"] + " order by B_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgBreakdown.DataSource = dtfilter;
                dgBreakdown.DataBind();
                dgBreakdown.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("B_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("B_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("B_SLIPNO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("B_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("B_FILE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("B_STATUS", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgBreakdown.DataSource = dtFilter;
                    dgBreakdown.DataBind();
                    dgBreakdown.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Improvement Entry", "LoadStatus", ex.Message);
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
                Response.Redirect("~/ToolRoom/ADD/Improvement.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Improvement Entry", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgBreakdown_PageIndexChanging
    protected void dgBreakdown_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBreakdown.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgBreakdown_RowCommand
    protected void dgBreakdown_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/ToolRoom/ADD/Improvement.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion View

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();

                        DataTable DtDetails = CommonClasses.Execute("select * from BREAKDOWN_ENTRY WHERE  (BREAKDOWN_ENTRY.ES_DELETE = 0) AND   BREAKDOWN_ENTRY.B_CODE='" + um_code + "' AND B_STATUS=1");
                        if (DtDetails.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Modify ,Used In Other Transaction ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                        Response.Redirect("~/ToolRoom/ADD/Improvement.aspx?c_name=" + type + "&u_code=" + um_code, false);
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

            #region Print
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "1";
                        string MatReq_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/ToolRoom/ADD/BreakdownSlipReport.aspx?MatReq_Code=" + MatReq_Code + "&print_type=" + type, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion Print

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
                        GridViewRow row = dgBreakdown.Rows[Index];
                        string mt_month = ((Label)(row.FindControl("lblB_CODE"))).Text;
                        string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;

                        if (Status == "Close")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Have No Rights To Change Status";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        Response.Redirect("~/ToolRoom/ADD/Improvement.aspx?c_name=" + type + "&u_code=" + mt_month, false);
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
            CommonClasses.SendError("Improvement Entry", "dgDetailSupplierPO_RowCommand", Ex.Message);
        }
    }
    #endregion dgBreakdown_RowCommand

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Improvement Entry", "btnClose_Click", ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select ISNULL(ES_MODIFY,0) AS ES_MODIFY from BREAKDOWN_ENTRY where B_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("BREAKDOWN_ENTRY-View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion

    #region dgBreakdown_RowDeleting
    protected void dgBreakdown_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgBreakdown.Rows[e.RowIndex].FindControl("lblB_CODE"))).Text))
                {
                    string um_code = ((Label)(dgBreakdown.Rows[e.RowIndex].FindControl("lblB_CODE"))).Text;

                    DataTable DtDetails = CommonClasses.Execute("select * from BREAKDOWN_ENTRY WHERE  (BREAKDOWN_ENTRY.ES_DELETE = 0) AND   BREAKDOWN_ENTRY.B_CODE='" + um_code + "' AND B_STATUS=1");

                    if (DtDetails.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can Not Delete ,This is Used In other transaction.  ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    bool flag = CommonClasses.Execute1("UPDATE BREAKDOWN_ENTRY SET ES_DELETE = 1 WHERE B_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {

                        CommonClasses.WriteLog("Breakdown Entry", "Delete", "Breakdown Entry", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadIssue();
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
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region dgBreakdown_RowEditing
    protected void dgBreakdown_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    #endregion
}
