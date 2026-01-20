using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewCustRej : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='21'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadCustomerRejection();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "Page_Load", Ex.Message);
        }
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomerRejection
    private void LoadCustomerRejection()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select   CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_CODE=" + (string)Session["CompanyCode"] + " AND    ISNULL(CR_TRANS_TYPE,0)=1 order by CR_GIN_NO desc");
            if (dt.Rows.Count == 0)
            {
                if (dt.Rows.Count == 0)
                {
                    dtFilter.Clear();

                    if (dtFilter.Columns.Count == 0)
                    {
                        dgCustomerRejection.Enabled = false;
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_TYPE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("CR_P_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgCustomerRejection.DataSource = dtFilter;
                        dgCustomerRejection.DataBind();
                    }
                }
            }
            else
            {
                dgCustomerRejection.Enabled = true;
                dgCustomerRejection.DataSource = dt;
                dgCustomerRejection.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadCustomerRejection", Ex.Message);
        }
    }
    #endregion

    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "txtString_TextChanged", Ex.Message);
        }
    }

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();
            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("select   CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_CODE=" + (string)Session["CompanyCode"] + " AND    ISNULL(CR_TRANS_TYPE,0)=1 and (P_NAME like upper('%" + str + "%') OR convert(varchar,CR_GIN_DATE,106) like upper('%" + str + "%') OR CR_GIN_NO like upper('%" + str + "%') OR CR_CHALLAN_NO like upper('%" + str + "%') OR convert(varchar,CR_CHALLAN_DATE,106) like upper('%" + str + "%') OR CR_P_CODE like upper('%" + str + "%')) order by CR_GIN_NO desc");
            else
                dtfilter = CommonClasses.Execute("select   CR_CODE, CR_GIN_NO, convert(varchar,CR_GIN_DATE,106) as CR_GIN_DATE, CR_TYPE, CR_CHALLAN_NO, convert(varchar,CR_CHALLAN_DATE,106) as CR_CHALLAN_DATE, CR_P_CODE, P_NAME from CUSTREJECTION_MASTER, PARTY_MASTER where CUSTREJECTION_MASTER.ES_DELETE=0 AND CR_P_CODE=P_CODE and CR_CM_CODE=" + (string)Session["CompanyCode"] + " AND    ISNULL(CR_TRANS_TYPE,0)=1 order by CR_GIN_NO desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerRejection.Enabled = true;
                dgCustomerRejection.DataSource = dtfilter;
                dgCustomerRejection.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgCustomerRejection.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CHALLAN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerRejection.DataSource = dtFilter;
                    dgCustomerRejection.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadStatus", ex.Message);
        }
    }

    #endregion

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/IRN/ADD/CustRej.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Customer Rejection Transaction", "btnAddNew_Click", Ex.Message);
        }
    }
    protected void dgCustomerRejection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerRejection.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Transaction", "dgCustomerRejection_PageIndexChanging", Ex.Message);
        }
    }
    protected void dgCustomerRejection_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string CR_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/IRN/ADD/CustRej.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);
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
                        string CR_CODE = e.CommandArgument.ToString();
                        int cnt = 0;
                        Response.Redirect("~/IRN/ADD/CustRej.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);
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
            if (e.CommandName.Equals("Print"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    string CR_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/RoportForms/ADD/CustomerRejectionRPTForm.aspx?CR_CODE=" + CR_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "GridView1_RowCommand", Ex.Message);
        }
    }

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from CUSTREJECTION_MASTER where CR_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Customer Rejection", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion
    protected void dgCustomerRejection_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgCustomerRejection.Rows[e.RowIndex].FindControl("lblCR_CODE"))).Text))
            {
                try
                {
                    string CR_CODE = ((Label)(dgCustomerRejection.Rows[e.RowIndex].FindControl("lblCR_CODE"))).Text;
                    bool flag = false;
                    DataTable dtRejDet = CommonClasses.Execute("SELECT CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_I_CODE,CR_INV_NO,ISNULL(CD_INSP_FLG,0) AS CD_INSP_FLG from CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER where CD_CR_CODE=CR_CODE and CD_CR_CODE='" + CR_CODE + "'");

                    if (dtRejDet.Rows.Count > 0)
                    {
                        for (int z = 0; z < dtRejDet.Rows.Count; z++)
                        {
                            if (dtRejDet.Rows[z]["CD_INSP_FLG"].ToString() == "1" || dtRejDet.Rows[z]["CD_INSP_FLG"].ToString().ToUpper() == "TRUE")
                            {

                                PanelMsg.Visible = true;
                                lblmsg.Text = "You can't delete this record, it is used in  Inspection";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;

                            }
                        }

                        flag = CommonClasses.Execute1("UPDATE CUSTREJECTION_MASTER SET ES_DELETE = 1 WHERE CR_CODE='" + Convert.ToInt32(CR_CODE) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("CUSTREJECTION_MASTER", "Delete", "CUSTREJECTION_MASTER", CR_CODE, Convert.ToInt32(CR_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }

                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("CUSTREJECTION_MASTER", "dgCustomerRejection_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another Person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadCustomerRejection();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }

    protected void dgCustomerRejection_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string CR_CODE = ((Label)(dgCustomerRejection.Rows[e.NewEditIndex].FindControl("lblCR_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/IRN/ADD/CustRej.aspx?c_name=" + type + "&CR_CODE=" + CR_CODE, false);
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
            CommonClasses.SendError("Customer Po Transactoin", "dgCustomerMaster_RowEditing", Ex.Message);
        }
    }
}
