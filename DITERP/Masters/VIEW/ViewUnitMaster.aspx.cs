using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_UnitMaster : System.Web.UI.Page
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
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='10'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadUnit();
                    if (dgUnitMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_DESC", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgUnitMaster.DataSource = dtFilter;
                            dgUnitMaster.DataBind();
                            dgUnitMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master - View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadUnit
    private void LoadUnit()
    {
        try
        {
            BL_UnitMaster = new UnitMaster_BL();
            BL_UnitMaster.I_UOM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_UnitMaster.FillGrid(dgUnitMaster);
            if (dgUnitMaster.Rows.Count > 0)
            {
                dgUnitMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master-View", "LoadUnit", Ex.Message);
        }
    }
    #endregion

    #region dgUnitMaster_RowDeleting
    protected void dgUnitMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgUnitMaster.Rows[e.RowIndex].FindControl("lblI_UOM_CODE"))).Text))
                {
                    BL_UnitMaster = new UnitMaster_BL();
                    string i_uom_code = ((Label)(dgUnitMaster.Rows[e.RowIndex].FindControl("lblI_UOM_CODE"))).Text;
                    string i_uom_name = ((Label)(dgUnitMaster.Rows[e.RowIndex].FindControl("lblI_UOM_NAME"))).Text;
                    string i_uom_desc = ((Label)(dgUnitMaster.Rows[e.RowIndex].FindControl("lblI_UOM_DESC"))).Text;
                    BL_UnitMaster.I_UOM_CODE = Convert.ToInt32(i_uom_code);
                    if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_UOM_CODE", "AND ES_DELETE=0", i_uom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_UnitMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Unit Master", "Delete", "Unit Master", i_uom_name, Convert.ToInt32(i_uom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadUnit();
                    }
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
            CommonClasses.SendError("Unit Master - View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgUnitMaster_RowEditing
    protected void dgUnitMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string i_uom_code = ((Label)(dgUnitMaster.Rows[e.NewEditIndex].FindControl("lblI_UOM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/UnitMaster.aspx?c_name=" + type + "&i_uom_code=" + i_uom_code, false);
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
            CommonClasses.SendError("Unit Master - View", "dgUnitMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgUnitMaster_PageIndexChanging
    protected void dgUnitMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgUnitMaster.PageIndex = e.NewPageIndex;
        LoadUnit();
    }
    #endregion

    #region dgUnitMaster_RowUpdating
    protected void dgUnitMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgUnitMaster_RowCommand
    protected void dgUnitMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string i_uom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/UnitMaster.aspx?c_name=" + type + "&i_uom_code=" + i_uom_code, false);
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
                        string i_uom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/UnitMaster.aspx?c_name=" + type + "&i_uom_code=" + i_uom_code, false);
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
            CommonClasses.SendError("Unit Master - View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from ITEM_UNIT_MASTER where I_UOM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Unit Master - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Unit Master - View", "ShowMessage", Ex.Message);
            return false;
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
            CommonClasses.SendError("Unit Master - View", "btnClose_Click", ex.Message);
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
            CommonClasses.SendError("Unit Master - View", "btnSearch_Click", ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT I_UOM_CODE,I_UOM_NAME,I_UOM_DESC FROM ITEM_UNIT_MASTER WHERE I_UOM_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (I_UOM_NAME like upper('%" + str + "%') or I_UOM_DESC like upper('%" + str + "%')) order by I_UOM_NAME,I_UOM_DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT I_UOM_CODE,I_UOM_NAME,I_UOM_DESC FROM ITEM_UNIT_MASTER WHERE I_UOM_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by I_UOM_NAME,I_UOM_DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgUnitMaster.DataSource = dtfilter;
                dgUnitMaster.DataBind();
                dgUnitMaster.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_DESC", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgUnitMaster.DataSource = dtFilter;
                    dgUnitMaster.DataBind();
                    dgUnitMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Unit Master - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Masters/ADD/UnitMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Unit Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}