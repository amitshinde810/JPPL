using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class PPC_VIEW_CPBOM_Master : System.Web.UI.Page
{
    #region " Var "
    BOM_Master_BL BL_BOM_Master = null;
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
                    DataTable dtRights = CommonClasses.Execute("SELECT UR_RIGHTS FROM USER_RIGHT WHERE UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' AND UR_SM_CODE='266'");

                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("CPBM_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("FINISH_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("FINISH_NAME", typeof(String)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgBox.DataSource = dtFilter;
                        dgBox.DataBind();
                        dgBox.Enabled = false;
                    }
                    LoadStatus(txtString);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("CP BOM Master - View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CP BOM Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region dgBox_RowDeleting
    protected void dgBox_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgBox.Rows[e.RowIndex].FindControl("lblCPBM_CODE"))).Text))
                {
                    BL_BOM_Master = new BOM_Master_BL();
                    string code = ((Label)(dgBox.Rows[e.RowIndex].FindControl("lblCPBM_CODE"))).Text;
                   // string Date = ((Label)(dgBox.Rows[e.RowIndex].FindControl("lblVS_DATE"))).Text;

                    BL_BOM_Master.CPBM_CODE = Convert.ToInt32(code);
                    //if (CommonClasses.CheckUsedInTran("VENDORSCHEDULE_APPROVAL", "VSA_I_CODE", "AND ES_DELETE=0 AND VSA_MONTH='" + Convert.ToDateTime(Date).ToString("dd/MMM/yyyy") + "'", code))
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "You cant delete this record it has used in Approval";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //}
                    //else
                    //{
                    bool flag = BL_BOM_Master.Delete();
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("CP BOM Master ", "Delete", "CP BOM Master ", BL_BOM_Master.CPBM_CODE.ToString(), Convert.ToInt32(BL_BOM_Master.CPBM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    // }
                    LoadStatus(txtString);
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
            CommonClasses.SendError("CP BOM Master-View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgBox_RowEditing
    protected void dgBox_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string i_cat_code = ((Label)(dgBox.Rows[e.NewEditIndex].FindControl("lblCPBM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/ItemCategoryMaster.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);
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
            CommonClasses.SendError("CP BOM Master-View", "dgBox_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgBox_PageIndexChanging
    protected void dgBox_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgBox.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgBox_RowCommand
    protected void dgBox_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {

                string type = "VIEW";
                string i_cat_code = e.CommandArgument.ToString();
                Response.Redirect("~/PPC/ADD/CPBOM_MASTER.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);

            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string i_cat_code = e.CommandArgument.ToString();
                        Response.Redirect("~/PPC/ADD/CPBOM_MASTER.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);
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
            CommonClasses.SendError("CP BOM Master - View", "dgBox_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgBox_RowUpdatingMyRegion
    protected void dgBox_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from CP_BOM_MASTER where CPBM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("CP BOM Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("CP BOM Master-View", "ShowMessage", Ex.Message);
            return false;
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
            CommonClasses.SendError("CP BOM Master - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();
            BL_BOM_Master = new BOM_Master_BL();
            BL_BOM_Master.CPBM_CM_COMP_ID = Convert.ToInt32(Session["CompanyCode"]);
            dtfilter = BL_BOM_Master.FillGrid(str);
            if (dtfilter.Rows.Count > 0)
            {
                dgBox.DataSource = dtfilter;
                dgBox.DataBind();
                dgBox.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPBM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FINISH_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FINISH_NAME", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgBox.DataSource = dtFilter;
                    dgBox.DataBind();
                    dgBox.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("CP BOM Master - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/PPC/ADD/CPBOM_MASTER.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("CP BOM Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
