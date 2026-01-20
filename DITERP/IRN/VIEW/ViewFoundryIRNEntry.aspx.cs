using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewFoundryIRNEntry : System.Web.UI.Page
{
    #region " Var "
    UnitMaster_BL BL_UnitMaster = null;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='123'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadData();
                    if (dgIRNF.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("IRN_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IRN_NO", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IRN_DATE", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgIRNF.DataSource = dtFilter;
                            dgIRNF.DataBind();
                            dgIRNF.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT DISTINCT IRN_CODE, IRN_NO,convert(varchar, IRN_DATE,106) AS IRN_DATE FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_CODE = IRND_IRN_CODE WHERE   IRN_TRANS_TYPE=0 AND IRN_TYPE=0 AND  (IRN_ENTRY.ES_DELETE = 0) AND IRN_CM_ID ='" + Session["CompanyId"].ToString() + "' ORDER BY IRN_NO DESC");
            if (dt.Rows.Count > 0)
            {
                dgIRNF.DataSource = dt;
                dgIRNF.DataBind();
                dgIRNF.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRN_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRN_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRN_DATE", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgIRNF.DataSource = dtFilter;
                    dgIRNF.DataBind();
                    dgIRNF.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Other IRN Entry-View", "LoadData", Ex.Message);
        }
    }
    #endregion

    #region dgIRNF_RowDeleting
    protected void dgIRNF_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgIRNF.Rows[e.RowIndex].FindControl("lblIRN_CODE"))).Text))
                {

                    string IRN_CODE = ((Label)(dgIRNF.Rows[e.RowIndex].FindControl("lblIRN_CODE"))).Text;
                    string IRN_NO = ((Label)(dgIRNF.Rows[e.RowIndex].FindControl("lblIRN_NO"))).Text; 
                    bool flag = CommonClasses.Execute1("UPDATE IRN_ENTRY SET ES_DELETE=1 where  IRN_CODE='" + IRN_CODE + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Other IRN Entry", "Delete", "Other IRN Entry", IRN_NO, Convert.ToInt32(IRN_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadData();
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgIRNF_RowEditing
    protected void dgIRNF_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string RSM_CODE = ((Label)(dgIRNF.Rows[e.NewEditIndex].FindControl("lblRSM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/IRN/ADD/FoundryIRNEntry.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "dgIRNF_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgIRNF_PageIndexChanging
    protected void dgIRNF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgIRNF.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgIRNF_RowUpdating
    protected void dgIRNF_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgIRNF_RowCommand
    protected void dgIRNF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string RSM_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/IRN/ADD/FoundryIRNEntry.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string RSM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/IRN/ADD/FoundryIRNEntry.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from IRN_ENTRY where IRN_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Other IRN Entry - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT  DISTINCT IRN_CODE, IRN_NO,convert(varchar, IRN_DATE,106) AS IRN_DATE FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_CODE = IRND_IRN_CODE WHERE  IRN_TRANS_TYPE=0 AND   IRN_TYPE=0 AND (IRN_ENTRY.ES_DELETE = 0) AND IRN_CM_ID ='" + Session["CompanyId"].ToString() + "' and (IRN_NO like upper('%" + str + "%') or IRN_DATE like upper('%" + str + "%')) ORDER BY IRN_NO DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT  DISTINCT IRN_CODE, IRN_NO,convert(varchar, IRN_DATE,106) AS IRN_DATE FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_CODE = IRND_IRN_CODE WHERE  IRN_TRANS_TYPE=0 AND  IRN_TYPE=0 AND  (IRN_ENTRY.ES_DELETE = 0) AND IRN_CM_ID ='" + Session["CompanyId"].ToString() + "'   ORDER BY IRN_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgIRNF.DataSource = dtfilter;
                dgIRNF.DataBind();
                dgIRNF.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRN_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRN_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRN_DATE", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgIRNF.DataSource = dtFilter;
                    dgIRNF.DataBind();
                    dgIRNF.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Other IRN Entry - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/IRN/ADD/FoundryIRNEntry.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Other IRN Entry - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
