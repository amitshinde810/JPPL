using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Masters_VIEW_ItemStandWgtMaster : System.Web.UI.Page
{

    #region Vraible
    static string right = "";
    DataTable dtFilter = new DataTable();
    string c_type = "";
    #endregion

    #region Event
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPP");
            home.Attributes["class"] = "active";

            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("COPPMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='165'");
                    right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                    LoadItem();
                    if (dgRawMaterial.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        dgRawMaterial.Enabled = false;
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("LM_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("STG_NAME", typeof(String)));

                            dtFilter.Columns.Add(new System.Data.DataColumn("I_STANDARD_PRODUCTION", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgRawMaterial.DataSource = dtFilter;
                            dgRawMaterial.DataBind();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Standard Weight Master", "Page_Load", Ex.Message);
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
                Response.Redirect("~/Masters/ADD/AddItemStandWgtMaster.aspx?c_name=" + type + "&c_type=" + c_type, false);
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
            CommonClasses.SendError("Item Standard Weight Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/COPPDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Standard Weight Master", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Item Standard Weight Master", "btnSearch_Click", ex.Message);
        }
    }
    #endregion

    #region dgRawMaterial_RowDeleting
    protected void dgRawMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_CODE"))).Text))
            {
                string i_code = ((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_CODE"))).Text;
                string i_name = ((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_NAME"))).Text;
                string i_codeno = ((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_CODENO"))).Text;

                if (CommonClasses.CheckUsedInTran("SUPP_PO_MASTER,SUPP_PO_DETAILS", "SPOD_I_CODE", "AND SPOD_SPOM_CODE=SPOM_CODE and SUPP_PO_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Purchase Order";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER,CUSTPO_DETAIL", "CPOD_I_CODE", "AND CPOD_CPOM_CODE=CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Sales Order";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran("ISSUE_MASTER,ISSUE_MASTER_DETAIL ", "IMD_I_CODE", " AND ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE AND ISSUE_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Issue To Production";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran("PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL ", "PSD_I_CODE", " AND PS_CODE=PSD_PS_CODE AND PRODUCTION_TO_STORE_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Production TO store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran(" BOM_MASTER,BOM_DETAIL", "BD_I_CODE", " AND BM_CODE=BD_BM_CODE AND BOM_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Bill of Master details";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran(" BOM_MASTER,BOM_DETAIL", "BM_I_CODE", " AND BM_CODE=BD_BM_CODE AND BOM_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Bill of Master";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran(" INWARD_MASTER,INWARD_DETAIL", "IWD_I_CODE", " AND IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Inward Master";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran(" STOCK_LEDGER", "STL_I_CODE", " ", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record.It has been used in Stock ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                bool flag = CommonClasses.Execute1("UPDATE ITEM_MASTER SET ES_DELETE = 1 WHERE I_CODE='" + Convert.ToInt32(i_code) + "'");
                if (flag == true)
                {
                    CommonClasses.WriteLog("Item Standard Weight Master", "Delete", "Item Standard Weight Master", i_name, Convert.ToInt32(i_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Deleted Successfully";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another Person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadItem();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgRawMaterial_PageIndexChanging
    protected void dgRawMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgRawMaterial.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Standard Weight Master", "dgRawMaterial_PageIndexChanging", ex.Message);
        }
    }
    #endregion

    #region dgRawMaterial_RowCommand
    protected void dgRawMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string i_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/AddItemStandWgtMaster.aspx?c_name=" + type + "&i_code=" + i_code + "&c_type=" + c_type, false);
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
                        string i_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/AddItemStandWgtMaster.aspx?c_name=" + type + "&i_code=" + i_code + "&c_type=" + c_type, false);
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
            CommonClasses.SendError("Item Standard Weight Master", "dgRawMaterial_RowCommand", Ex.Message);
        }
    }
    #endregion
    #endregion
    #region User Defined Method

    #region LoadItem
    private void LoadItem()
    {
        try
        {
            string strSql = "";
            DataTable dtfilter = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME,LM_NAME,STG_NAME,I_STANDARD_PRODUCTION FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER,LINE_MASTER,LINE_CHANGE,STAGE_MASTER,STGAEWISE_ITEM WHERE ITEM_CATEGORY_MASTER.I_CAT_CODE=ITEM_MASTER.I_CAT_CODE and I_CM_COMP_ID = '1' and ITEM_MASTER.ES_DELETE='0' AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_MASTER.ES_DELETE='0' and ITEM_MASTER.I_CAT_CODE=-2147483648 and LM_CODE=LC_LM_CODE and LC_I_CODE=I_CODE and SI_STG_CODE=STG_CODE and SI_I_CODE=I_CODE and STAGE_MASTER.ES_DELETE=0 and LINE_MASTER.ES_DELETE=0 and SI_ACTIVE=1 and LC_ACTIVE=1 ORDER BY I_CODENO");
            if (dtfilter.Rows.Count > 0)
            {
                dgRawMaterial.Enabled = true;
                dgRawMaterial.DataSource = dtfilter;
                dgRawMaterial.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Standard Weight Master", "LoadItem", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "''");
            string strSql = "";
            strSql = "SELECT I_CODE,I_CODENO,I_NAME,LM_NAME,STG_NAME,I_STANDARD_PRODUCTION FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER,LINE_MASTER,LINE_CHANGE,STAGE_MASTER,STGAEWISE_ITEM WHERE ITEM_CATEGORY_MASTER.I_CAT_CODE=ITEM_MASTER.I_CAT_CODE and I_CM_COMP_ID = '1' and ITEM_MASTER.ES_DELETE='0' AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_MASTER.ES_DELETE='0' and ITEM_MASTER.I_CAT_CODE=-2147483648 and LM_CODE=LC_LM_CODE and LC_I_CODE=I_CODE and SI_STG_CODE=STG_CODE and SI_I_CODE=I_CODE and STAGE_MASTER.ES_DELETE=0 and LINE_MASTER.ES_DELETE=0 and SI_ACTIVE=1 and LC_ACTIVE=1 ";
            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute(strSql + " and (I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%'))");
            else
                dtfilter = CommonClasses.Execute(strSql);
            dtfilter.DefaultView.Sort = "I_CODENO";
            dtfilter.DefaultView.ToTable();
            if (dtfilter.Rows.Count > 0)
            {
                dgRawMaterial.Enabled = true;
                dgRawMaterial.DataSource = dtfilter;
                dgRawMaterial.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgRawMaterial.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STG_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_STANDARD_PRODUCTION", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgRawMaterial.DataSource = dtFilter;
                    dgRawMaterial.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Standard Weight Master", "LoadStatus", ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from ITEM_MASTER where I_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Standard Weight Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Item Standard Weight Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion
}