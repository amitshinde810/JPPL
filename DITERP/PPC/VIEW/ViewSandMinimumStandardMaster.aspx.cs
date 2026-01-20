using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewSandMinimumStandardMaster : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='185'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadGroup();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadGroup
    private void LoadGroup()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM SAND_RAW_STANDARD_MASTER where SRSM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 ORDER by SRSM_AC4B");
            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgSandRawStandardMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_AC4B", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_LM25", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_SAND", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSandRawStandardMaster.DataSource = dtFilter;
                    dgSandRawStandardMaster.DataBind();
                }
            }
            else
            {
                dgSandRawStandardMaster.Enabled = true;
                dgSandRawStandardMaster.DataSource = dt;
                dgSandRawStandardMaster.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "LoadUser", Ex.Message);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER- View", "txtString_TextChanged", Ex.Message);
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
                //dtfilter = CommonClasses.Execute("SELECT M_CODE,M_NAME,M_DESCR FROM MACHINE_MASTER where ES_DELETE=0 and M_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  and (lower(M_NAME) like lower('%" + str + "%') or lower(M_DESCR) like lower('%" + str + "%')) order by M_NAME ");
                dtfilter = CommonClasses.Execute("Select * FROM [SAND_RAW_STANDARD_MASTER] WHERE (lower(SRSM_AC4B) LIKE lower('%" + str + "%') or  lower(SRSM_LM25) LIKE lower('%" + str + "%') or  lower(SRSM_SAND)LIKE lower('%" + str + "%')) AND ES_DELETE=0");
            else
                dtfilter = CommonClasses.Execute("Select * FROM [SAND_RAW_STANDARD_MASTER] WHERE SRSM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0");

            if (dtfilter.Rows.Count > 0)
            {
                dgSandRawStandardMaster.Enabled = true;
                dgSandRawStandardMaster.DataSource = dtfilter;
                dgSandRawStandardMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgSandRawStandardMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_AC4B", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_LM25", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SRSM_SAND", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSandRawStandardMaster.DataSource = dtFilter;
                    dgSandRawStandardMaster.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER - View", "LoadStatus", Ex.Message);
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
                Response.Redirect("~/PPC/ADD/SandMinimumStandardMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region dgSandRawStandardMaster_PageIndexChanging
    protected void dgSandRawStandardMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgSandRawStandardMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "dgSandRawStandardMaster_PageIndexChanging", Ex.Message);
        }
    }
    #endregion dgSandRawStandardMaster_PageIndexChanging

    #region dgSandRawStandardMaster_RowCommand
    protected void dgSandRawStandardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Response.Redirect("~/PPC/ADD/SandMinimumStandardMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
                        Response.Redirect("~/PPC/ADD/SandMinimumStandardMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "dgSandRawStandardMaster_RowCommand", exc.Message);
        }
    }
    #endregion dgSandRawStandardMaster_RowCommand

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from [SAND_RAW_STANDARD_MASTER] where SRSM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "dgSandRawStandardMaster_RowEditing", exc.Message);
        }
        return false;
    }
    #endregion

    #region dgSandRawStandardMaster_RowDeleting
    protected void dgSandRawStandardMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgSandRawStandardMaster.Rows[e.RowIndex].FindControl("lblSRSM_CODE"))).Text))
                {
                    string um_code = ((Label)(dgSandRawStandardMaster.Rows[e.RowIndex].FindControl("lblSRSM_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE SAND_RAW_STANDARD_MASTER SET ES_DELETE = 1 WHERE SRSM_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("SAND RAW STANDARD MASTER", "Delete", "SAND RAW STANDARD MASTER", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "dgSandRawStandardMaster_RowEditing", exc.Message);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgSandRawStandardMaster_RowEditing
    protected void dgSandRawStandardMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgSandRawStandardMaster.Rows[e.NewEditIndex].FindControl("lblSRSM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/PPC/ADD/SandMinimumStandardMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "dgSandRawStandardMaster_RowEditing", exc.Message);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
