using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewGST_HSNSAC_Master : System.Web.UI.Page
{
    #region Variables
    ExciseTariffDetails_BL BL_ExciseTariffDetails = null;
    static string right = "";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Evenets
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='15'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    dtFilter.Clear();

                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_CM_COMP_ID", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_TARIFF_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_COMMODITY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_BASIC", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_SPECIAL", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_EDU_CESS", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("E_H_EDU", typeof(String)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgExciseTariffDetails.DataSource = dtFilter;
                        dgExciseTariffDetails.DataBind();
                        dgExciseTariffDetails.Enabled = false;
                    }
                    LoadExciseTariffDetails();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region dgSectorMaster_RowDeleting
    protected void dgExciseTariffDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_CODE"))).Text))
                {
                    BL_ExciseTariffDetails = new ExciseTariffDetails_BL();
                    string E_CODE = ((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_CODE"))).Text;
                    string E_TARIFF_NO = ((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_TARIFF_NO"))).Text;
                    string E_COMMODITY = ((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_COMMODITY"))).Text;
                    string E_BASIC = ((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_BASIC"))).Text;
                    string E_SPECIAL = ((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_SPECIAL"))).Text;
                    string E_EDU_CESS = ((Label)(dgExciseTariffDetails.Rows[e.RowIndex].FindControl("lblE_EDU_CESS"))).Text;

                    BL_ExciseTariffDetails.E_CODE = Convert.ToInt32(E_CODE);
                    if (CommonClasses.CheckUsedInTran("SERVICE_TYPE_MASTER", "S_E_CODE", "AND ES_DELETE=0", E_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record.It has been used in Service Type Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_E_CODE", "AND ES_DELETE=0", E_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record.It has been used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_SCAT_CODE", "AND ES_DELETE=0", E_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record.It has been used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    bool flag = BL_ExciseTariffDetails.Delete();
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Excise Tariff Details", "Delete", "Excise Tariff Details", E_COMMODITY, Convert.ToInt32(E_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));                       
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadExciseTariffDetails();
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
            CommonClasses.SendError("ExciseTariffDetails", "dgExciseTariffDetails_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgExciseTariffDetails_RowEditing
    protected void dgExciseTariffDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string E_CODE = ((Label)(dgExciseTariffDetails.Rows[e.NewEditIndex].FindControl("lblE_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/GST_HSNSAC_Master.aspx?c_name=" + type + "&SCT_CODE=" + E_CODE, false);
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
            CommonClasses.SendError("Excise Tariff Master", "dgExciseTariffDetails_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgExciseTariffDetails_PageIndexChanging
    protected void dgExciseTariffDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgExciseTariffDetails.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "dgExciseTariffDetails_PageIndexChanging", Ex.Message);
        }
    }
    #endregion

    #region dgExciseTariffDetails_RowUpdating
    protected void dgExciseTariffDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgExciseTariffDetails_RowCommand
    protected void dgExciseTariffDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string E_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/GST_HSNSAC_Master.aspx?c_name=" + type + "&E_CODE=" + E_CODE, false);
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
                        string E_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/GST_HSNSAC_Master.aspx?c_name=" + type + "&SCT_CODE=" + E_CODE, false);
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
            CommonClasses.SendError("Excise Tariff Master", "dgExciseTariffDetails_RowCommand", Ex.Message);
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
            CommonClasses.SendError("Excise Tariff Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("~/Masters/ADD/GST_HSNSAC_Master.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Excise Tariff Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #endregion

    #region Methods

    #region Load Excise Tariff Details
    private void LoadExciseTariffDetails()
    {
        try
        {
            BL_ExciseTariffDetails = new ExciseTariffDetails_BL();
            BL_ExciseTariffDetails.E_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_ExciseTariffDetails.E_TALLY_GST_EXCISE = true;
            BL_ExciseTariffDetails.FillGrid(dgExciseTariffDetails);
            if (dgExciseTariffDetails.Rows.Count > 0)
            {
                dgExciseTariffDetails.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "LoadSector", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from  EXCISE_TARIFF_MASTER where E_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Excise Tariff Master", "ShowMessage", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT E_CODE, E_CM_COMP_ID, E_TARIFF_NO, E_COMMODITY, E_BASIC, E_SPECIAL, E_EDU_CESS, E_H_EDU FROM  EXCISE_TARIFF_MASTER WHERE E_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and E_TALLY_GST_EXCISE='1' and (E_TARIFF_NO like upper('%" + str + "%') OR E_COMMODITY like upper('%" + str + "%') OR E_BASIC like upper('%" + str + "%') OR E_SPECIAL like upper('%" + str + "%') OR E_EDU_CESS like upper('%" + str + "%'))");
            else
                dtfilter = CommonClasses.Execute("SELECT E_CODE, E_CM_COMP_ID, E_TARIFF_NO, E_COMMODITY, E_BASIC, E_SPECIAL, E_EDU_CESS, E_H_EDU FROM  EXCISE_TARIFF_MASTER WHERE E_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0'  and E_TALLY_GST_EXCISE='1'");

            if (dtfilter.Rows.Count > 0)
            {
                dgExciseTariffDetails.DataSource = dtfilter;
                dgExciseTariffDetails.DataBind();
                dgExciseTariffDetails.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_CM_COMP_ID", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_TARIFF_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_COMMODITY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_BASIC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_SPECIAL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_EDU_CESS", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("E_H_EDU", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgExciseTariffDetails.DataSource = dtFilter;
                    dgExciseTariffDetails.DataBind();
                    dgExciseTariffDetails.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Excise Tariff Master", "LoadStatus", ex.Message);
        }
    }
    #endregion
    #endregion
}
