using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewItemCategoryMaster : System.Web.UI.Page
{
    #region " Var "
    ItemCategoryMaster_BL BL_ItemCategoryMaster = null;
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");

            home.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='9'");

                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(string)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgItemCategory.DataSource = dtFilter;
                        dgItemCategory.DataBind();
                        dgItemCategory.Enabled = false;
                    }
                    LoadCategory();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Master - View", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Item Category Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadCategory
    private void LoadCategory()
    {
        try
        {
            BL_ItemCategoryMaster = new ItemCategoryMaster_BL();
            BL_ItemCategoryMaster.I_CAT_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_ItemCategoryMaster.FillGrid(dgItemCategory);
            if (dgItemCategory.Rows.Count > 0)
            {
                dgItemCategory.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Master - View", "LoadItemCategory", Ex.Message);
        }
    }
    #endregion

    #region dgItemCategory_RowDeleting
    protected void dgItemCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgItemCategory.Rows[e.RowIndex].FindControl("lblI_CAT_CODE"))).Text))
                {
                    BL_ItemCategoryMaster = new ItemCategoryMaster_BL();
                    string i_cat_code = ((Label)(dgItemCategory.Rows[e.RowIndex].FindControl("lblI_CAT_CODE"))).Text;
                    string i_cat_name = ((Label)(dgItemCategory.Rows[e.RowIndex].FindControl("lblI_CAT_NAME"))).Text;

                    BL_ItemCategoryMaster.I_CAT_CODE = Convert.ToInt32(i_cat_code);
                    if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_CAT_CODE", "AND ES_DELETE=0", i_cat_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_ItemCategoryMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Item Category Master", "Delete", "Item Category Master", i_cat_code, Convert.ToInt32(i_cat_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
      
                        }
                    }
                    LoadCategory();
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
            CommonClasses.SendError("Item Category Master-View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgItemCategory_RowEditing
    protected void dgItemCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string i_cat_code = ((Label)(dgItemCategory.Rows[e.NewEditIndex].FindControl("lblI_CAT_CODE"))).Text;
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
            CommonClasses.SendError("Item Category Master-View", "dgItemCategory_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgItemCategory_PageIndexChanging
    protected void dgItemCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgItemCategory.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgItemCategory_RowCommand
    protected void dgItemCategory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string i_cat_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/ItemCategoryMaster.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);
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
                        string i_cat_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/ItemCategoryMaster.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "record Is Used By Another Person";
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
            CommonClasses.SendError("Item Category Master - View", "dgItemCategory_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgItemCategory_RowUpdatingMyRegion
    protected void dgItemCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from ITEM_CATEGORY_MASTER where I_CAT_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Item Category Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Item Category Master-View", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Item Category Master - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT Top 500 I_CAT_CODE,I_CAT_NAME FROM ITEM_CATEGORY_MASTER WHERE I_CAT_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (I_CAT_NAME like upper('%" + str + "%')) order by I_CAT_NAME");
            else
                dtfilter = CommonClasses.Execute("SELECT Top 500 I_CAT_CODE,I_CAT_NAME FROM ITEM_CATEGORY_MASTER WHERE I_CAT_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by I_CAT_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgItemCategory.DataSource = dtfilter;
                dgItemCategory.DataBind();
                dgItemCategory.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgItemCategory.DataSource = dtFilter;
                    dgItemCategory.DataBind();
                    dgItemCategory.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Category Master - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Masters/ADD/ItemCategoryMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Item Category Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}