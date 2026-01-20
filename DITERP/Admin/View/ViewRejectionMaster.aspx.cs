using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_View_ViewRejectionMaster : System.Web.UI.Page
{
    #region Variables

    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Evenets
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='5'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadRejection();
                    dgRejectionMaster.Enabled = true;
                    if (dgRejectionMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_MELTING_LOSS", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_CASTING_REJECTION", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_MACHINING_REJECTION", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_CORE_BREAKAGE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_SAND_WASTAGE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgRejectionMaster.DataSource = dtFilter;
                            dgRejectionMaster.DataBind();
                            dgRejectionMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Master ", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Rejection Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region dgRejectionMaster_RowDeleting
    protected void dgRejectionMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgRejectionMaster.Rows[e.RowIndex].FindControl("lblRM_CODE"))).Text))
                {

                    string RM_CODE = ((Label)(dgRejectionMaster.Rows[e.RowIndex].FindControl("lblRM_CODE"))).Text;
                    string RM_MELTING_LOSS = ((Label)(dgRejectionMaster.Rows[e.RowIndex].FindControl("lblRM_MELTING_LOSS"))).Text;

                    //if (CommonClasses.CheckUsedInTran("STATE_MASTER", "SM_RM_CODE", "AND ES_DELETE=0", RM_CODE))
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "You cant delete this record it has used in State Master";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //}
                    //else
                    //{
                    bool flag = CommonClasses.Execute1("UPDATE [REJECTION_MASTER] SET ES_DELETE = 1 WHERE RM_CODE='" + Convert.ToInt32(RM_CODE) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Rejection Master", "Delete", "Rejection Master", RM_MELTING_LOSS, Convert.ToInt32(RM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    //}
                }
                LoadRejection();
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
            CommonClasses.SendError("Rejection Master", "dgRejectionMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgRejectionMaster_RowEditing
    protected void dgRejectionMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string RM_CODE = ((Label)(dgRejectionMaster.Rows[e.NewEditIndex].FindControl("lblRM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/RejectionMaster.aspx?c_name=" + type + "&RM_CODE=" + RM_CODE, false);
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
            CommonClasses.SendError("Rejection Master", "dgRejectionMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgRejectionMaster_PageIndexChanging
    protected void dgRejectionMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgRejectionMaster.PageIndex = e.NewPageIndex;
            LoadRejection();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgRejectionMaster_RowUpdating
    protected void dgRejectionMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgRejectionMaster_RowCommand
    protected void dgRejectionMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string RM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/RejectionMaster.aspx?c_name=" + type + "&RM_CODE=" + RM_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
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
                        string RM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/RejectionMaster.aspx?c_name=" + type + "&RM_CODE=" + RM_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
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
            CommonClasses.SendError("Rejection Master", "dgRejectionMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("~/Admin/Add/RejectionMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Rejection Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadRejection
    private void LoadRejection()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM [REJECTION_MASTER] where RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0");
            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgRejectionMaster.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_MELTING_LOSS", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CASTING_REJECTION", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_MACHINING_REJECTION", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CORE_BREAKAGE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_SAND_WASTAGE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgRejectionMaster.DataSource = dtFilter;
                    dgRejectionMaster.DataBind();
                }
            }
            else
            {
                dgRejectionMaster.Enabled = true;
                dgRejectionMaster.DataSource = dt;
                dgRejectionMaster.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Master", "LoadRejection", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from REJECTION_MASTER where RM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Rejection Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Rejection Master", "ShowMessage", Ex.Message);
            return false;
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
                dtfilter = CommonClasses.Execute("SELECT RM_CODE,RM_MELTING_LOSS FROM REJECTION_MASTER WHERE RM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (RM_MELTING_LOSS like upper('%" + str + "%')) order by RM_MELTING_LOSS ");
            else
                dtfilter = CommonClasses.Execute("SELECT RM_CODE,RM_MELTING_LOSS FROM REJECTION_MASTER WHERE RM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by RM_MELTING_LOSS");

            if (dtfilter.Rows.Count > 0)
            {
                dgRejectionMaster.Enabled = true;
                dgRejectionMaster.DataSource = dtfilter;
                dgRejectionMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_MELTING_LOSS", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CASTING_REJECTION", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_MACHINING_REJECTION", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CORE_BREAKAGE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_SAND_WASTAGE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgRejectionMaster.DataSource = dtFilter;
                    dgRejectionMaster.DataBind();
                    dgRejectionMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Master", "LoadStatus", ex.Message);
        }
    }
    #endregion
    #endregion
}
