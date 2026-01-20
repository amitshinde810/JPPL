using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewCustomerMaster : System.Web.UI.Page
{
    #region Variable Declaration
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='14'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadCustomers();
                    if (dgCustomerMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();


                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_PARTY_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_CONTACT", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_PHONE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("A_NO", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCustomerMaster.DataSource = dtFilter;
                            dgCustomerMaster.DataBind();
                            dgCustomerMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master(View)", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomers
    private void LoadCustomers()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT TOP 500 P_CODE,P_PARTY_CODE,P_NAME,P_CONTACT,P_PHONE,A_NO FROM PARTY_MASTER, AREA_MASTER WHERE A_CODE=P_A_CODE AND P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND P_TYPE=1 and PARTY_MASTER.ES_DELETE=0 ORDER BY P_NAME");

            dgCustomerMaster.DataSource = dt;
            dgCustomerMaster.DataBind();
            if (dgCustomerMaster.Rows.Count > 0)
            {
                dgCustomerMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "LoadUnit", Ex.Message);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT TOP 500 P_CODE,P_PARTY_CODE,P_NAME,P_CONTACT,P_PHONE,A_NO FROM PARTY_MASTER, AREA_MASTER WHERE A_CODE=P_A_CODE AND P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and PARTY_MASTER.ES_DELETE='0' AND P_TYPE=1 and (P_PARTY_CODE like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR P_CONTACT like upper('%" + str + "%')OR P_PHONE like upper('%" + str + "%') OR A_NO like upper('%" + str + "%')) order by P_NAME");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 P_CODE,P_PARTY_CODE,P_NAME,P_CONTACT,P_PHONE,A_NO FROM PARTY_MASTER, AREA_MASTER WHERE A_CODE=P_A_CODE AND P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND P_TYPE=1 and PARTY_MASTER.ES_DELETE='0' order by P_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerMaster.DataSource = dtfilter;
                dgCustomerMaster.DataBind();
                dgCustomerMaster.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_PARTY_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CONTACT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_PHONE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("A_NO", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerMaster.DataSource = dtFilter;
                    dgCustomerMaster.DataBind();
                    dgCustomerMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Master", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Masters/ADD/CustomerMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events
    protected void dgCustomerMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_CODE"))).Text))
                {

                    string p_code = ((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_CODE"))).Text;
                    string p_name = ((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_NAME"))).Text;
                    string p_party_code = ((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_PARTY_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER", "CPOM_P_CODE", "AND ES_DELETE=0", p_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Sales Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("SUPP_PO_MASTER", "SPOM_P_CODE", "AND ES_DELETE=0", p_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Sub contractor Purchase Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("DELIVERY_CHALLAN_MASTER", "DCM_P_CODE", "AND ES_DELETE=0", p_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Delivary Challan";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE PARTY_MASTER SET ES_DELETE = 1 WHERE  P_TYPE=1 AND P_CODE='" + Convert.ToInt32(p_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Customer Master", "Delete", "Customer Master", p_code, Convert.ToInt32(p_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadCustomers();
                    }
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
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Master", "dgCustomerMaster_RowDeleting", Ex.Message);
        }
    }
    protected void dgCustomerMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string p_code = ((Label)(dgCustomerMaster.Rows[e.NewEditIndex].FindControl("lblP_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/CustomerMaster.aspx?c_name=" + type + "&p_code=" + p_code, false);
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
            CommonClasses.SendError("Customer Master", "dgCustomerMaster_RowEditing", Ex.Message);
        }
    }
    protected void dgCustomerMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    protected void dgCustomerMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string p_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/CustomerMaster.aspx?c_name=" + type + "&i_uom_code=" + p_code, false);
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
                        string p_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/CustomerMaster.aspx?c_name=" + type + "&p_code=" + p_code, false);
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
            CommonClasses.SendError("Customer Master", "GridView1_RowCommand", Ex.Message);
        }
    }

    protected void dgCustomerMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from PARTY_MASTER where  P_TYPE=1 AND P_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Customer Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Customer Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}