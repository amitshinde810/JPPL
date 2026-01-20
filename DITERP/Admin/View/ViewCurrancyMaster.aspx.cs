using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Masters_VIEW_ViewCurrancyMaster : System.Web.UI.Page
{
    #region " Var "
    CurrancyMaster_BL Bl_CrrancyMaster = null;
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    DataTable dt = new DataTable();
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='8'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadCurrancy();
                    dgCurrancyMaster.Enabled = true;

                    if (dgCurrancyMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("CURR_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CURR_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CURR_SHORT_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CURR_DESC", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CURR_RATE", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCurrancyMaster.DataSource = dtFilter;
                            dgCurrancyMaster.DataBind();
                            dgCurrancyMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Currency Master-View", "Page_Load", Ex.Message);
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
                Response.Redirect("~/Admin/ADD/AddCurrancyMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Add Currancy Master", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Currency Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadCategory
    private void LoadCurrancy()
    {
        try
        {
            Bl_CrrancyMaster = new CurrancyMaster_BL();
            Bl_CrrancyMaster.CURR_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            dt = CommonClasses.Execute("Select  COUNTRY_NAME,CURR_NAME,CURR_CODE,CURR_SHORT_NAME,CURR_DESC,CURR_RATE from CURRANCY_MASTER,COUNTRY_MASTER where CURR_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and CURRANCY_MASTER.ES_DELETE=0 and COUNTRY_MASTER.COUNTRY_CODE=CURRANCY_MASTER.CURR_COUNTRY_CODE  Order by CURR_NAME ");
            dgCurrancyMaster.DataSource = dt;
            dgCurrancyMaster.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Currancy Master-View", "LoadCurrancy", Ex.Message);
        }
    }
    #endregion

    #region dgItemCategory_RowCommand
    protected void dgCurrancyMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string Curr_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/ADD/AddCurrancyMaster.aspx?c_name=" + type + "&Curr_Code=" + Curr_Code, false);
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
                        string Curr_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/ADD/AddCurrancyMaster.aspx?c_name=" + type + "&Curr_Code=" + Curr_Code, false);
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
            CommonClasses.SendError("Currency Master", "dgCurrancyMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from CURRANCY_MASTER where CURR_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
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

    #region dgCurrancyMaster_RowDeleting
    protected void dgCurrancyMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCurrancyMaster.Rows[e.RowIndex].FindControl("lblC_Code"))).Text))
                {
                    Bl_CrrancyMaster = new CurrancyMaster_BL();
                    string Curr_Code = ((Label)(dgCurrancyMaster.Rows[e.RowIndex].FindControl("lblC_Code"))).Text;
                   
                    Bl_CrrancyMaster.CURR_CODE = Convert.ToInt32(Curr_Code);
                    if (CommonClasses.CheckUsedInTran("INVOICE_MASTER", "INM_CURR_CODE", "AND ES_DELETE=0", Curr_Code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Invoice";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("SUPP_PO_MASTER", "SPOM_CURR_CODE", "AND ES_DELETE=0", Curr_Code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Purchase Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = Bl_CrrancyMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Currancy  Master", "Delete", "Currancy Master", Curr_Code, Convert.ToInt32(Curr_Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                }
                LoadCurrancy();
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
            CommonClasses.SendError("Currency Master-View", "dgCurrancyMaster_RowDeleting", Ex.Message);
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
            CommonClasses.SendError("Currency Master-View", "btnSearch_Click", ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT COUNTRY_NAME,CURR_NAME, CURR_CODE,CURR_SHORT_NAME,CURR_DESC,CURR_RATE FROM CURRANCY_MASTER,COUNTRY_MASTER WHERE CURR_CM_COMP_ID='" + Session["CompanyId"] + "'and CURRANCY_MASTER.ES_DELETE=0 and COUNTRY_MASTER.COUNTRY_CODE=CURRANCY_MASTER.CURR_COUNTRY_CODE and (COUNTRY_NAME like upper('%" + str + "%') or  CURR_SHORT_NAME like upper('%" + str + "%')or  CURR_NAME like upper('%" + str + "%') or CURR_DESC like upper('%" + str + "%')or CURR_RATE like upper('%" + str + "%') ) Order by CURR_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT COUNTRY_NAME,CURR_NAME, CURR_CODE,CURR_SHORT_NAME,CURR_DESC,CURR_RATE FROM CURRANCY_MASTER,COUNTRY_MASTER WHERE CURR_CM_COMP_ID='" + Session["CompanyId"] + "' and CURRANCY_MASTER.ES_DELETE=0 and COUNTRY_MASTER.COUNTRY_CODE=CURRANCY_MASTER.CURR_COUNTRY_CODE Order by CURR_NAME ");

            if (dtfilter.Rows.Count > 0)
            {
                dgCurrancyMaster.Enabled = true;
                dgCurrancyMaster.DataSource = dtfilter;
                dgCurrancyMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CURR_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CURR_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CURR_SHORT_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CURR_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CURR_RATE", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCurrancyMaster.DataSource = dtFilter;
                    dgCurrancyMaster.DataBind();
                    dgCurrancyMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Master-View", "LoadStatus", ex.Message);
        }
    } 
    #endregion

    #region dgCurrancyMaster_RowEditing
    protected void dgCurrancyMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Delete"))
            {
                string Curr_Code = ((Label)(dgCurrancyMaster.Rows[e.NewEditIndex].FindControl("lblC_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/ADD/AddCurrancyMaster.aspx?c_name=" + type + "&Curr_Code=" + Curr_Code, false);
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
            CommonClasses.SendError("Currency Master-View", "dgCurrancyMaster_RowEditing", Ex.Message);
        }
    } 
    #endregion

    #region dgCurrancyMaster_PageIndexChanging
    protected void dgCurrancyMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgCurrancyMaster.PageIndex = e.NewPageIndex;
        LoadCurrancy();
    } 
    #endregion

    #region BlankGridView
    private void BlankGridView()
    {
        dt.Clear();
        if (dt.Columns.Count == 0)
        {
            dt.Columns.Add("Currency Code");
            dt.Columns.Add("Currency Name");
            dt.Columns.Add("Currancy Short Name");
            dt.Columns.Add("Currency Description");
        }
        dt.Rows.Add(dt.NewRow());
        dgCurrancyMaster.Visible = true;
        dgCurrancyMaster.DataSource = dt;
        dgCurrancyMaster.DataBind();
    } 
    #endregion
}