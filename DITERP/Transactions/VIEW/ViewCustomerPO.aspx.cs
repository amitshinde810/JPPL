using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transaction_VIEW_ViewCustomerPO : System.Web.UI.Page
{
    #region Variable
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='18'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadCustomerPO();
                    if (dgDetailPO.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_WORK_ODR_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_PONO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_CUST_I_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_STATUS", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_APPROVE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_AUTH_FLG", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgDetailPO.DataSource = dtFilter;
                            dgDetailPO.DataBind();
                            dgDetailPO.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomerPO
    private void LoadCustomerPO()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT TOp 500 P_NAME,CPOM_CODE,CPOM_PONO,convert(varchar,CPOM_DATE,106) as CPOM_DATE,case when CPOD_STATUS=0 then 'Active' else 'Short Close' end as CPOD_STATUS, case when CPOM_AUTH_FLG=0 then 'Pending' else 'Authorize' end as CPOM_AUTH_FLG ,case when ISNULL(CPOM_APPROVE,0)=0 then 'Pending' else 'Approve' end as CPOM_APPROVE , CPOM_WORK_ODR_NO,case when CPOM_AM_COUNT =0 then 'NO' else 'YES' end as CPOM_AM_COUNT,CPOD_CUST_I_CODE from CUSTPO_MASTER ,PARTY_MASTER,CUSTPO_DETAIL where CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CODE = CPOD_CPOM_CODE and CPOM_P_CODE=P_CODE  and CPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by CPOM_CODE DESC ");
            dgDetailPO.DataSource = dt;
            dgDetailPO.DataBind();
            if (dgDetailPO.Rows.Count > 0)
            {
                dgDetailPO.Enabled = true;
            }
            else
            {
                dgDetailPO.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomerPO", Ex.Message);
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
            CommonClasses.SendError("Customer Order", "txtString_TextChanged", Ex.Message);
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
              //  dtfilter = CommonClasses.Execute("SELECT TOp 500 CPOM_CODE,CPOM_PONO,case when CPOD_STATUS=0 then 'Active' else 'Short Close' end as CPOD_STATUS,convert(varchar,CPOM_DATE,106) as CPOM_DATE,P_NAME,CPOM_WORK_ODR_NO,case when CPOM_AM_COUNT =0 then 'NO' else 'YES' end as CPOM_AM_COUNT,CPOD_CUST_I_CODE FROM CUSTPO_MASTER,PARTY_MASTER,CUSTPO_DETAIL WHERE CPOM_P_CODE=P_CODE AND CPOM_CODE = CPOD_CPOM_CODE  and CUSTPO_MASTER.CPOM_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' AND CUSTPO_MASTER.ES_DELETE='0' and (CPOM_PONO like upper('%" + str + "%') OR convert(varchar,CPOM_DATE,106) like upper('%" + str + "%') OR convert(varchar,CPOD_CUST_I_CODE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(CPOM_DOC_NO) like upper('%" + str + "%')) order by CPOM_CODE desc");
                dtfilter = CommonClasses.Execute("SELECT TOp 500 CPOM_CODE,CPOM_PONO,case when CPOD_STATUS=0 then 'Active' else 'Short Close' end as CPOD_STATUS,convert(varchar,CPOM_DATE,106) as CPOM_DATE,P_NAME,CPOM_WORK_ODR_NO,case when CPOM_AM_COUNT =0 then 'NO' else 'YES' end as CPOM_AM_COUNT,CPOD_CUST_I_CODE,CPOM_DOC_NO, case when CPOM_AUTH_FLG=0 then 'Pending' else 'Authorize' end as CPOM_AUTH_FLG ,case when ISNULL(CPOM_APPROVE,0)=0 then 'Pending' else 'Approve' end as CPOM_APPROVE    FROM CUSTPO_MASTER,PARTY_MASTER,CUSTPO_DETAIL WHERE CPOM_P_CODE=P_CODE AND CPOM_CODE = CPOD_CPOM_CODE  and CUSTPO_MASTER.CPOM_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' AND CUSTPO_MASTER.ES_DELETE='0' AND   (CPOM_PONO like upper('%" + str + "%') OR convert(varchar,CPOM_DATE,106) like upper('%" + str + "%') OR convert(varchar,CPOD_CUST_I_CODE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(CPOM_DOC_NO) like upper('%" + str + "%') OR upper(case when CPOM_AM_COUNT =0 then 'NO' else 'YES' end) like upper('%" + str + "%')) order by CPOM_CODE desc DROP TABLE #temp");
            else
                dtfilter = CommonClasses.Execute("SELECT TOp 500 CPOM_CODE,CPOM_PONO,case when CPOD_STATUS=0 then 'Active' else 'Short Close' end as CPOD_STATUS,convert(varchar,CPOM_DATE,106) as CPOM_DATE,P_NAME,CPOM_WORK_ODR_NO,case when CPOM_AM_COUNT =0 then 'NO' else 'YES' end as CPOM_AM_COUNT,CPOD_CUST_I_CODE,CPOM_DOC_NO, case when CPOM_AUTH_FLG=0 then 'Pending' else 'Authorize' end as CPOM_AUTH_FLG ,case when ISNULL(CPOM_APPROVE,0)=0 then 'Pending' else 'Approve' end as CPOM_APPROVE    FROM CUSTPO_MASTER,PARTY_MASTER,CUSTPO_DETAIL WHERE CPOM_P_CODE=P_CODE AND CPOM_CODE = CPOD_CPOM_CODE  and CUSTPO_MASTER.CPOM_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' AND CUSTPO_MASTER.ES_DELETE='0' order by CPOM_CODE desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgDetailPO.DataSource = dtfilter;
                dgDetailPO.DataBind();
                dgDetailPO.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_WORK_ODR_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_PONO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_CUST_I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOD_STATUS", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_APPROVE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_AUTH_FLG", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                    dgDetailPO.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Order", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Order", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblCPOM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblCPOM_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("INVOICE_MASTER,INVOICE_DETAIL", "IND_CPOM_CODE", " AND INVOICE_MASTER.ES_DELETE=0 AND  INM_CODE=IND_INM_CODE AND INM_TYPE IN ('TAXINV','OutJWINM')", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record not deleted, it is used in Invoice";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", " AND INWARD_MASTER.ES_DELETE=0  AND IWM_CODE=IWD_IWM_CODE AND IWM_TYPE IN ('IWIFP')", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record not deleted, it is used in Inward";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE CUSTPO_MASTER SET ES_DELETE = 1 WHERE CPOM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Customer Order", "Delete", "Customer Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadCustomerPO();
                    }
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Customer Order", "dgDetailPO_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You have no rights to delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgDetailPO_RowCommand
    protected void dgDetailPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                if (e.CommandName.Equals("Authorize"))
                {
                    DataTable dtRightss = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='281'");
                    string rights = dtRightss.Rows.Count == 0 ? "0000000" : dtRightss.Rows[0][0].ToString();
                    if (CommonClasses.ValidRights(int.Parse(rights.Substring(5, 1)), this, "For Authorize"))
                    {
                        if (!ModifyLog(e.CommandArgument.ToString()))
                        {
                            int Index = Convert.ToInt32(e.CommandArgument.ToString());
                            GridViewRow row = dgDetailPO.Rows[Index];
                            string mt_month = ((Label)(row.FindControl("lblCPOM_CODE"))).Text;
                            string Status = ((LinkButton)(row.FindControl("lnkCPOM_APPROVE"))).Text;
                            if (Status == "Approve")
                            {
                                string type = "Authorize";
                                Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&cpom_code=" + mt_month, false);
                            }
                            else
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Please Approve OA before Authorize";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;
                            }
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record used by another person";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You have no rights to Authorize";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else if (e.CommandName.Equals("Approve"))
                {
                    DataTable dtRightss = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='280'");
                    string rights = dtRightss.Rows.Count == 0 ? "0000000" : dtRightss.Rows[0][0].ToString();
                    if (CommonClasses.ValidRights(int.Parse(rights.Substring(4, 1)), this, "For Approve"))
                    {
                        if (!ModifyLog(e.CommandArgument.ToString()))
                        {
                            int Index = Convert.ToInt32(e.CommandArgument.ToString());
                            GridViewRow row = dgDetailPO.Rows[Index];
                            string mt_month = ((Label)(row.FindControl("lblCPOM_CODE"))).Text;
                            string Status = ((LinkButton)(row.FindControl("lnkCPOM_AUTH_FLG"))).Text;
                            if (Status == "Pending")
                            {
                                string type = "Approve";
                                Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&cpom_code=" + mt_month, false);
                            }
                            else
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "This OA Is Authorized";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;
                            }


                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record used by another person";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You have no rights to Authorize";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to view";
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
                        string cpom_code = e.CommandArgument.ToString();
                        DataTable dtcheck = CommonClasses.Execute("select WO_NO from WORK_ORDER_MASTER,WORK_ORDER_DETAIL,CUSTPO_MASTER where WORK_ORDER_MASTER.WO_CODE=WORK_ORDER_DETAIL.WOD_WO_CODE and CUSTPO_MASTER.CPOM_CODE=WORK_ORDER_MASTER.WO_CPOM_CODE and WORK_ORDER_MASTER.ES_DELETE=0 and CPOM_CODE=" + cpom_code + "");
                        if (dtcheck.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can't Modify It Is Used In Work Order No: " + dtcheck.Rows[0]["WO_NO"].ToString();
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {
                            Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("AMEND"))
            {

                if (!ModifyLog(e.CommandArgument.ToString()))
                {

                    string type = "AMEND";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/CustomerPORPTForm.aspx?cpom_code=" + cpom_code + "&print_type=" + type1, false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("PrintWRK"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "wrkorder";
                    Response.Redirect("~/RoportForms/ADD/CustomerPORPTForm.aspx?cpom_code=" + cpom_code + "&print_type=" + type1, false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "dgDetailPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from CUSTPO_MASTER where CPOM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Customer Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgDetailPO_PageIndexChanging
    protected void dgDetailPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailPO.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
