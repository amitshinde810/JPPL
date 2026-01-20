using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewSectorMaster : System.Web.UI.Page
{
    #region Variables
    SalesTaxMaster_BL BL_SalesTaxMAster = null;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Events

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='16'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadSalesTaxMasterDetails();

                    if (dgSalesTaxMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_ALIAS", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_TCS_TAX", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_SET_OFF", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_FORM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_ACC_HEAD", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_ACC_HEAD", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgSalesTaxMaster.DataSource = dtFilter;
                            dgSalesTaxMaster.DataBind();
                            dgSalesTaxMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Tax Master-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region dgSectorMaster_RowDeleting
    protected void dgSalesTaxMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_CODE"))).Text))
                {
                    BL_SalesTaxMAster = new SalesTaxMaster_BL();
                    string ST_CODE = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_CODE"))).Text;
                    string ST_TAX_NAME = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_TAX_NAME"))).Text;
                    string ST_ALIAS = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_ALIAS"))).Text;
                    string ST_SALES_TAX = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_SALES_TAX"))).Text;
                    string ST_TCS_TAX = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_TCS_TAX"))).Text;
                    string ST_FORM_NO = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_FORM_NO"))).Text;
                    string ST_SALES_ACC_HEAD = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_SALES_ACC_HEAD"))).Text;
                    string ST_TAX_ACC_HEAD = ((Label)(dgSalesTaxMaster.Rows[e.RowIndex].FindControl("lblST_TAX_ACC_HEAD"))).Text;

                    BL_SalesTaxMAster.ST_CODE = Convert.ToInt32(ST_CODE);
                    if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_ACCOUNT_SALES", "AND ES_DELETE=0", ST_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_ACCOUNT_PURCHASE", "AND ES_DELETE=0", ST_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Item Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_SalesTaxMAster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Sales Tax Master", "Delete", "Sales Tax Master", ST_CODE, Convert.ToInt32(ST_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadSalesTaxMasterDetails();
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Saltes Tax Master", "dgSalesTaxMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgSalesTaxMaster_RowEditing
    protected void dgSalesTaxMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string E_CODE = ((Label)(dgSalesTaxMaster.Rows[e.NewEditIndex].FindControl("lblE_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/ExciseTariffDetails.aspx?c_name=" + type + "&SCT_CODE=" + E_CODE, false);
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
            CommonClasses.SendError("Sales Tax Master", "dgSalesTaxMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgSalesTaxMaster_PageIndexChanging
    protected void dgSalesTaxMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgSalesTaxMaster.PageIndex = e.NewPageIndex;
            LoadSalesTaxMasterDetails();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgSalesTaxMaster_RowUpdating
    protected void dgSalesTaxMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgSalesTaxMaster_RowCommand
    protected void dgSalesTaxMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string ST_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/SalesTaxMaster.aspx?c_name=" + type + "&ST_CODE=" + ST_CODE, false);
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
                        string ST_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/SalesTaxMaster.aspx?c_name=" + type + "&SCT_CODE=" + ST_CODE, false);
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
            CommonClasses.SendError("Sales Tax Master", "dgSalesTaxMaster_RowCommand", Ex.Message);
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
            CommonClasses.SendError("Sales Tax Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("~/Masters/ADD/SalesTaxMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Sales Tax Master", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Sales Tax Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #endregion

    #region Methods

    #region Load Sales Tax Details
    private void LoadSalesTaxMasterDetails()
    {
        try
        {
            BL_SalesTaxMAster = new SalesTaxMaster_BL();
            BL_SalesTaxMAster.ST_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_SalesTaxMAster.FillGrid(dgSalesTaxMaster);
            if (dgSalesTaxMaster.Rows.Count > 0)
            {
                dgSalesTaxMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Tax Master", "LoadSalesTaxMasterDetails", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from  SALES_TAX_MASTER where ST_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Sales Tax Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Sales Tax Master", "ShowMessage", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT ST_CODE, ST_TAX_NAME, ST_ALIAS, ST_SALES_TAX, ST_TCS_TAX, ST_SET_OFF, ST_FORM_NO,ST_SALES_ACC_HEAD,ST_TAX_ACC_HEAD FROM  SALES_TAX_MASTER WHERE ST_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (ST_TAX_NAME like upper('%" + str + "%') OR ST_ALIAS like upper('%" + str + "%') OR ST_SALES_TAX like upper('%" + str + "%') OR ST_TCS_TAX like upper('%" + str + "%') OR ST_SET_OFF like upper('%" + str + "%') OR ST_FORM_NO like upper('%" + str + "%') OR ST_SALES_ACC_HEAD like upper('%" + str + "%') OR ST_TAX_ACC_HEAD like upper('%" + str + "%') ) order by ST_TAX_NAME ASC");
            else
                dtfilter = CommonClasses.Execute("SELECT ST_CODE, ST_TAX_NAME, ST_ALIAS, ST_SALES_TAX, ST_TCS_TAX, ST_SET_OFF, ST_FORM_NO,ST_SALES_ACC_HEAD,ST_TAX_ACC_HEAD FROM  SALES_TAX_MASTER WHERE ST_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by ST_TAX_NAME ASC");

            if (dtfilter.Rows.Count > 0)
            {
                dgSalesTaxMaster.DataSource = dtfilter;
                dgSalesTaxMaster.DataBind();
                dgSalesTaxMaster.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_ALIAS", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_TCS_TAX", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_SET_OFF", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_FORM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_ACC_HEAD", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_ACC_HEAD", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSalesTaxMaster.DataSource = dtFilter;
                    dgSalesTaxMaster.DataBind();
                    dgSalesTaxMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Salesd Tax Master", "LoadStatus", ex.Message);
        }
    }
    #endregion
    #endregion
}