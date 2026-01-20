using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_VIEW_ViewSupplierMaster : System.Web.UI.Page
{
    #region " Var "
    SupplierTypeMaster_BL BL_SupplierTypeMaster = null;
    static string right = "";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='17'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadSupplier();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Type Master", "Page_Load", Ex.Message);
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
                Response.Redirect("~/Masters/ADD/AddSupplierTypeMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Supplier Type Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCategory
    private void LoadSupplier()
    {
        try
        {
            BL_SupplierTypeMaster = new SupplierTypeMaster_BL();
            BL_SupplierTypeMaster.STM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_SupplierTypeMaster.FillGrid(dgSupplierTypeMaster);
            if (dgSupplierTypeMaster.Rows.Count == 0)
            {
                dgSupplierTypeMaster.Enabled = false;
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_TYPE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_TYPE_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_FIRST_LETTER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSupplierTypeMaster.DataSource = dtFilter;
                    dgSupplierTypeMaster.DataBind();
                }
            }
            else
            {
                dgSupplierTypeMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Type Master-View", "LoadSupplier", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from SUPPLIER_TYPE_MASTER where STM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Supplier Type Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Supplier Type Master-View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Type Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Row Command
    protected void dgSupplierTypeMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string STM_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/AddSupplierTypeMaster.aspx?c_name=" + type + "&STM_Code=" + STM_Code, false);
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
                        string STM_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/AddSupplierTypeMaster.aspx?c_name=" + type + "&STM_Code=" + STM_Code, false);
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
            CommonClasses.SendError("Supplier Type Master", "dgSupplierTypeMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgSupplierTypeMaster_RowEditing
    protected void dgSupplierTypeMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string STM_Code = ((Label)(dgSupplierTypeMaster.Rows[e.NewEditIndex].FindControl("lblSTM_CODE"))).Text;
            string type = "MODIFY";
            Response.Redirect("~/Masters/ADD/AddAreaMaster.aspx?c_name=" + type + "&STM_Code=" + STM_Code, false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Type Master-View", "dgSupplierTypeMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region Grid Row Deleting
    protected void dgSupplierTypeMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgSupplierTypeMaster.Rows[e.RowIndex].FindControl("lblSTM_Code"))).Text))
                {
                    BL_SupplierTypeMaster = new SupplierTypeMaster_BL();
                    string Supplier_Code = ((Label)(dgSupplierTypeMaster.Rows[e.RowIndex].FindControl("lblSTM_Code"))).Text;
                    string Supplier_Type_Code = ((Label)(dgSupplierTypeMaster.Rows[e.RowIndex].FindControl("lblSTM_TYPE_CODE"))).Text;
                    string Supplier_Desc = ((Label)(dgSupplierTypeMaster.Rows[e.RowIndex].FindControl("lblSTM_TYPE_DESC"))).Text;
                    string Supplier_First_Letter = ((Label)(dgSupplierTypeMaster.Rows[e.RowIndex].FindControl("lblSTM_FIRST_LETTER"))).Text;

                    BL_SupplierTypeMaster.STM_CODE = Convert.ToInt32(Supplier_Code);
                    if (CommonClasses.CheckUsedInTran("PARTY_MASTER", "P_STM_CODE", "AND ES_DELETE=0 and P_TYPE=2 ", Supplier_Code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Supplier Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_SupplierTypeMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Supplier Type Master", "Delete", "Supplier Type Master", Supplier_Code, Convert.ToInt32(Supplier_Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
            CommonClasses.SendError("Supplier Type Master-View", "dgSupplierTypeMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgSupplierTypeMaster_PageIndexChanging
    protected void dgSupplierTypeMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgSupplierTypeMaster.PageIndex = e.NewPageIndex;
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

                dtfilter = CommonClasses.Execute("SELECT STM_CODE,STM_TYPE_CODE,STM_TYPE_DESC,STM_FIRST_LETTER  FROM SUPPLIER_TYPE_MASTER WHERE STM_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (STM_TYPE_CODE like ('%" + str + "%')) or (STM_TYPE_DESC like ('%" + str + "%')) or (STM_FIRST_LETTER like ('%" + str + "%')) order by STM_TYPE_CODE  ");
            else
                dtfilter = CommonClasses.Execute("SELECT STM_CODE,STM_TYPE_CODE,STM_TYPE_DESC,STM_FIRST_LETTER  FROM SUPPLIER_TYPE_MASTER WHERE STM_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by STM_TYPE_CODE ");

            if (dtfilter.Rows.Count > 0)
            {
                dgSupplierTypeMaster.Enabled = true;
                dgSupplierTypeMaster.DataSource = dtfilter;
                dgSupplierTypeMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgSupplierTypeMaster.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_TYPE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_TYPE_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STM_FIRST_LETTER", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSupplierTypeMaster.DataSource = dtFilter;
                    dgSupplierTypeMaster.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Type Master-View", "LoadStatus", ex.Message);
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
            CommonClasses.SendError("Supplier Type Master-View", "btnSearch_Click", ex.Message);
        }
    }
    #endregion
}
