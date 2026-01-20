using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_VIEW_ViewCustomerTypeMaster : System.Web.UI.Page
{
    #region " Var "
    CustomerTypeMaster_BL BL_CustomerTypeMaster = null;
    static string right = "";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='64'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadSupplier();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region Button Add
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Masters/ADD/CustomerTypeMaster.aspx?c_name=" + type, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCategory
    private void LoadSupplier()
    {
        try
        {
            BL_CustomerTypeMaster = new CustomerTypeMaster_BL();
            BL_CustomerTypeMaster.CTM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_CustomerTypeMaster.FillGrid(dgCustomerTypeMaster);
            if (dgCustomerTypeMaster.Rows.Count == 0)
            {
                dgCustomerTypeMaster.Enabled = false;
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_TYPE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_TYPE_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_FIRST_LETTER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerTypeMaster.DataSource = dtFilter;
                    dgCustomerTypeMaster.DataBind();
                }
            }
            else
            {
                dgCustomerTypeMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master-View", "LoadSupplier", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from CUSTOMER_TYPE_MASTER where CTM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Customer Type Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Customer Type Master-View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Row Command
    protected void dgCustomerTypeMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                    {
                        string type = "VIEW";
                        string CTM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/CustomerTypeMaster.aspx?c_name=" + type + "&CTM_CODE=" + CTM_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Have No Rights To View";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                    {
                        string type = "MODIFY";
                        string CTM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/CustomerTypeMaster.aspx?c_name=" + type + "&CTM_CODE=" + CTM_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Have No Rights To Modify";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "dgCustomerTypeMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerTypeMaster_RowEditing
    protected void dgCustomerTypeMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string CTM_CODE = ((Label)(dgCustomerTypeMaster.Rows[e.NewEditIndex].FindControl("lblCTM_CODE"))).Text;
            string type = "MODIFY";
            Response.Redirect("~/Masters/ADD/CustomerTypeMaster.aspx?c_name=" + type + "&CTM_CODE=" + CTM_CODE, false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master-View", "dgCustomerTypeMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region Grid Row Deleting
    protected void dgCustomerTypeMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustomerTypeMaster.Rows[e.RowIndex].FindControl("lblCTM_CODE"))).Text))
                {
                    BL_CustomerTypeMaster = new CustomerTypeMaster_BL();
                    string Supplier_Code = ((Label)(dgCustomerTypeMaster.Rows[e.RowIndex].FindControl("lblCTM_CODE"))).Text;
                    string Supplier_Type_Code = ((Label)(dgCustomerTypeMaster.Rows[e.RowIndex].FindControl("lblCTM_TYPE_CODE"))).Text;
                    string Supplier_Desc = ((Label)(dgCustomerTypeMaster.Rows[e.RowIndex].FindControl("lblCTM_TYPE_DESC"))).Text;
                    string Supplier_First_Letter = ((Label)(dgCustomerTypeMaster.Rows[e.RowIndex].FindControl("lblCTM_FIRST_LETTER"))).Text;

                    BL_CustomerTypeMaster.CTM_CODE = Convert.ToInt32(Supplier_Code);
                    if (CommonClasses.CheckUsedInTran("PARTY_MASTER", "P_CUST_TYPE", "AND ES_DELETE=0 and P_TYPE=1", Supplier_Code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Customer Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_CustomerTypeMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Customer Type Master", "Delete", "Customer Type Master", Supplier_Code, Convert.ToInt32(Supplier_Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadSupplier();
                    }
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
            CommonClasses.SendError("Customer Type Master-View", "dgCustomerTypeMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerTypeMaster_PageIndexChanging
    protected void dgCustomerTypeMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgCustomerTypeMaster.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text;
            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")

                dtfilter = CommonClasses.Execute("SELECT CTM_CODE,CTM_TYPE_CODE,CTM_TYPE_DESC,CTM_FIRST_LETTER  FROM CUSTOMER_TYPE_MASTER WHERE CTM_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE=0 and ((CTM_TYPE_CODE like ('%" + str + "%')) or (CTM_TYPE_DESC like ('%" + str + "%')) or (CTM_FIRST_LETTER like ('%" + str + "%'))) order by CTM_TYPE_DESC ASC ");
            else
                dtfilter = CommonClasses.Execute("SELECT CTM_CODE,CTM_TYPE_CODE,CTM_TYPE_DESC,CTM_FIRST_LETTER  FROM CUSTOMER_TYPE_MASTER WHERE CTM_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE=0 order by CTM_TYPE_DESC ASC");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerTypeMaster.Enabled = true;
                dgCustomerTypeMaster.DataSource = dtfilter;
                dgCustomerTypeMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgCustomerTypeMaster.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_TYPE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_TYPE_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CTM_FIRST_LETTER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerTypeMaster.DataSource = dtFilter;
                    dgCustomerTypeMaster.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master-View", "LoadStatus", ex.Message);
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
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master-View", "btnSearch_Click", ex.Message);
        }
    }
    #endregion
}
