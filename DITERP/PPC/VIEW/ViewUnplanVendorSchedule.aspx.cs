using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PPC_VIEW_ViewUnplanVendorSchedule : System.Web.UI.Page
{
    #region " Var "
    UnplanVendorScheduleBL BL_UNPLANVENDOR = null;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='9'");

                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("UVS_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("UVS_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("TOTALQTY", typeof(String)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgCustomerSchedule.DataSource = dtFilter;
                        dgCustomerSchedule.DataBind();
                        dgCustomerSchedule.Enabled = false;
                    }
                      LoadStatus(txtString);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unplan Vendor Schedule - View", "Page_Load", Ex.Message);
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

    

    #region dgCustomerSchedule_RowDeleting
    protected void dgCustomerSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblVS_CODE"))).Text))
                {
                    BL_UNPLANVENDOR = new UnplanVendorScheduleBL();
                    string code = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblVS_CODE"))).Text;
                    string Date = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblVS_DATE"))).Text;

                    BL_UNPLANVENDOR.UVS_CODE = Convert.ToInt32(code);
                    if (CommonClasses.CheckUsedInTran("VENDORSCHEDULE_APPROVAL", "VSA_I_CODE", "AND ES_DELETE=0 AND VSA_MONTH='" + Convert.ToDateTime(Date).ToString("dd/MMM/yyyy") + "'", code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Approval";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_UNPLANVENDOR.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Unplan Vendor Schedule", "Delete", "Unplan Vendor Schedule", BL_UNPLANVENDOR.UVS_CODE.ToString(), Convert.ToInt32(BL_UNPLANVENDOR.UVS_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        }
                    }
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
            CommonClasses.SendError("Unplan Vendor Schedule-View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerSchedule_RowEditing
    protected void dgCustomerSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string i_cat_code = ((Label)(dgCustomerSchedule.Rows[e.NewEditIndex].FindControl("lblI_CAT_CODE"))).Text;
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
            CommonClasses.SendError("Item Category Master-View", "dgCustomerSchedule_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerSchedule_PageIndexChanging
    protected void dgCustomerSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgCustomerSchedule.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgCustomerSchedule_RowCommand
    protected void dgCustomerSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                
                    string type = "VIEW";
                    string i_cat_code = e.CommandArgument.ToString();
                    Response.Redirect("~/PPC/ADD/UnplanVendorSchedule.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);
                 
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string i_cat_code = e.CommandArgument.ToString();
                        Response.Redirect("~/PPC/ADD/UnplanVendorSchedule.aspx?c_name=" + type + "&i_cat_code=" + i_cat_code, false);
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
            CommonClasses.SendError("Unplan Vendor Schedule - View", "dgCustomerSchedule_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerSchedule_RowUpdatingMyRegion
    protected void dgCustomerSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from UNPLAN_VENDOR_SCHEDULE where UVS_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Unplan Vendor Schedule-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Unplan Vendor Schedule - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();
            BL_UNPLANVENDOR = new UnplanVendorScheduleBL();
            BL_UNPLANVENDOR.UVS_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            dtfilter=BL_UNPLANVENDOR.FillGrid(str);
            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerSchedule.DataSource = dtfilter;
                dgCustomerSchedule.DataBind();
                dgCustomerSchedule.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("UVS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UVS_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TOTALQTY", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerSchedule.DataSource = dtFilter;
                    dgCustomerSchedule.DataBind();
                    dgCustomerSchedule.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Unplan Vendor Schedule - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/PPC/ADD/UnplanVendorSchedule.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Unplan Vendor Schedule - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
