using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewSOTypeMaster : System.Web.UI.Page
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='63'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadPo();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadPo
    private void LoadPo()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM SO_TYPE_MASTER WHERE SO_T_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 order by SO_T_SHORT_NAME");

            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgPoTypeMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_SHORT_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_FIRST_LETTER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgPoTypeMaster.DataSource = dtFilter;
                    dgPoTypeMaster.DataBind();
                }
            }
            else
            {
                dgPoTypeMaster.Enabled = true;
                dgPoTypeMaster.DataSource = dt;
                dgPoTypeMaster.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "LoadUser", Ex.Message);
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
            CommonClasses.SendError("Sale Order Type Master- View", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT  * FROM SO_TYPE_MASTER WHERE ES_DELETE='FALSE' AND SO_T_COMP_ID = '" + Session["CompanyId"] + "' and (lower(SO_T_SHORT_NAME) like lower('%" + str + "%') or lower(SO_T_DESC) like lower('%" + str + "%') or lower(SO_T_FIRST_LETTER) like lower('%" + str + "%') ) order by SO_T_SHORT_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT  * FROM SO_TYPE_MASTER WHERE ES_DELETE='FALSE' AND SO_T_COMP_ID = '" + Session["CompanyId"] + "' order by SO_T_SHORT_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgPoTypeMaster.Enabled = true;
                dgPoTypeMaster.DataSource = dtfilter;
                dgPoTypeMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgPoTypeMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_SHORT_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SO_T_FIRST_LETTER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgPoTypeMaster.DataSource = dtFilter;
                    dgPoTypeMaster.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Order Type Master - View", "LoadStatus", Ex.Message);
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
                Response.Redirect("~/Masters/ADD/SoTypeMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Sale Order Type Master", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgPoTypeMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
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
                        Response.Redirect("~/Masters/ADD/SoTypeMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
                        Response.Redirect("~/Masters/ADD/SoTypeMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            CommonClasses.SendError("Sale Order Type Master", "GridView1_RowCommand", exc.Message);
        }
    }

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from SO_TYPE_MASTER where SO_T_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Sale Order Type Master", "GridView1_RowEditing", exc.Message);
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
                if (!ModifyLog(((Label)(dgPoTypeMaster.Rows[e.RowIndex].FindControl("lblSO_T_CODE"))).Text))
                {
                    string um_code = ((Label)(dgPoTypeMaster.Rows[e.RowIndex].FindControl("lblSO_T_CODE"))).Text;
                    string um_name = ((Label)(dgPoTypeMaster.Rows[e.RowIndex].FindControl("lblPO_T_SHORT_NAME"))).Text;
                    if (um_code == "-2147483648" || um_code == "-2147483647")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "These record is fixed";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER", "CPOM_TYPE", "AND ES_DELETE=0", um_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record because it has used in Sales Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE SO_TYPE_MASTER SET ES_DELETE = 1 WHERE SO_T_CODE='" + Convert.ToInt32(um_code) + "'");

                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Sale Order Type Master", "Delete", "Sale Order Type Master", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        }
                        LoadPo();
                    }
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
            CommonClasses.SendError("Sale Order Type Master", "GridView1_RowEditing", exc.Message);
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
                string user_code = ((Label)(dgPoTypeMaster.Rows[e.NewEditIndex].FindControl("lblSO_T_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/SoTypeMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
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
            CommonClasses.SendError("Sale Order Type Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
