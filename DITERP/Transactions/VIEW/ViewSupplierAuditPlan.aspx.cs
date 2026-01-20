using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ToolRoom_VIEW_ViewSupplierAuditPlan : System.Web.UI.Page
{
    #region Declaration
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='255'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadIssue();
                    if (dgSupplierAudit.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_DOC_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_AUDIT_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_AUDITOR_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_DOC_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_FILE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAP_STATUS", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgSupplierAudit.DataSource = dtFilter;
                            dgSupplierAudit.DataBind();
                            dgSupplierAudit.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadIssue
    private void LoadIssue()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select SAP_CODE,SAP_DOC_NO,CONVERT(varchar,SAP_DOC_DATE,106) AS SAP_DOC_DATE,CONVERT(varchar,SAP_AUDIT_DATE,106) AS SAP_AUDIT_DATE,SAP_AUDITOR_NAME,P_NAME,SAP_FILE,CASE WHEN SAP_STATUS=1 then '1' Else '0' END  AS SAP_STATUS from SUPPLIER_AUDIT_PLAN,PARTY_MASTER where SUPPLIER_AUDIT_PLAN.SAP_P_CODE=P_CODE and SUPPLIER_AUDIT_PLAN.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and SUPPLIER_AUDIT_PLAN.SAP_CM_CODE=" + (string)Session["CompanyCode"] + " order by SAP_CODE DESC");
            dgSupplierAudit.DataSource = dt;
            dgSupplierAudit.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "Supplier Audit Plan", Ex.Message);
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
            CommonClasses.SendError("Supplier Audit Plan", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")

                dtfilter = CommonClasses.Execute("select SAP_CODE,SAP_DOC_NO,CONVERT(varchar,SAP_DOC_DATE,106) AS SAP_DOC_DATE,CONVERT(varchar,SAP_AUDIT_DATE,106) AS SAP_AUDIT_DATE,SAP_AUDITOR_NAME,P_NAME,SAP_FILE,CASE WHEN SAP_STATUS=1 then '1' Else '0' END  AS SAP_STATUS from SUPPLIER_AUDIT_PLAN,PARTY_MASTER where SUPPLIER_AUDIT_PLAN.SAP_P_CODE=P_CODE and SUPPLIER_AUDIT_PLAN.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and SUPPLIER_AUDIT_PLAN.SAP_CM_CODE=" + (string)Session["CompanyCode"] + " and (upper(SAP_DOC_NO) like upper('%" + str + "%') OR convert(varchar,SAP_AUDIT_DATE,106) like upper('%" + str + "%') OR SAP_AUDITOR_NAME like '%" + str + "%' OR P_NAME like '%" + str + "%') order by SAP_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("select SAP_CODE,SAP_DOC_NO,CONVERT(varchar,SAP_DOC_DATE,106) AS SAP_DOC_DATE,CONVERT(varchar,SAP_AUDIT_DATE,106) AS SAP_AUDIT_DATE,SAP_AUDITOR_NAME,P_NAME,SAP_FILE,CASE WHEN SAP_STATUS=1 then '1' Else '0' END  AS SAP_STATUS from SUPPLIER_AUDIT_PLAN,PARTY_MASTER where SUPPLIER_AUDIT_PLAN.SAP_P_CODE=P_CODE and SUPPLIER_AUDIT_PLAN.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and SUPPLIER_AUDIT_PLAN.SAP_CM_CODE=" + (string)Session["CompanyCode"] + " order by SAP_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgSupplierAudit.DataSource = dtfilter;
                dgSupplierAudit.DataBind();
                dgSupplierAudit.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_DOC_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_AUDIT_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_AUDITOR_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_DOC_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_FILE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAP_STATUS", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSupplierAudit.DataSource = dtFilter;
                    dgSupplierAudit.DataBind();
                    dgSupplierAudit.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/SupplierAuditPlan.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Supplier Audit Plan", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgSupplierAudit_PageIndexChanging
    protected void dgSupplierAudit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgSupplierAudit.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgSupplierAudit_RowCommand
    protected void dgSupplierAudit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/SupplierAuditPlan.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion View

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();

                        DataTable DtDetails = CommonClasses.Execute("select * from SUPPLIER_AUDIT_PLAN WHERE (SUPPLIER_AUDIT_PLAN.ES_DELETE = 0) AND SUPPLIER_AUDIT_PLAN.SAP_CODE='" + um_code + "' AND SAP_STATUS=1");
                        if (DtDetails.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Modify ,Used In Other Transaction ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                        Response.Redirect("~/Transactions/ADD/SupplierAuditPlan.aspx?c_name=" + type + "&u_code=" + um_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
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
            #endregion Modify

            //#region Print
            //else if (e.CommandName.Equals("Print"))
            //{
            //    if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Print"))
            //    {
            //        if (!ModifyLog(e.CommandArgument.ToString()))
            //        {
            //            string type = "0";
            //            string MatReq_Code = e.CommandArgument.ToString();
            //            Response.Redirect("~/ToolRoom/ADD/BreakdownSlipReport.aspx?MatReq_Code=" + MatReq_Code + "&Type=" + type + "", false);
            //        }
            //    }
            //    else
            //    {
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "You Have No Rights To Print";
            //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //        return;
            //    }
            //}
            //#endregion Print

            #region UpdateStatus
            else if (e.CommandName.Equals("Status"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For UpdateStatus"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "UpdateStatus";
                        string um_code = e.CommandArgument.ToString();
                        int Index = Convert.ToInt32(e.CommandArgument.ToString());
                        GridViewRow row = dgSupplierAudit.Rows[Index];
                        string mt_month = ((Label)(row.FindControl("lblSAP_CODE"))).Text;
                        string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;

                        if (Status == "Close")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Have No Rights To Change Status";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        Response.Redirect("~/Transactions/ADD/SupplierAuditPlan.aspx?c_name=" + type + "&u_code=" + mt_month, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
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
            #endregion Modify
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "dgDetailSupplierPO_RowCommand", Ex.Message);
        }
    }
    #endregion dgSupplierAudit_RowCommand

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "btnClose_Click", ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select ISNULL(MODIFY,0) AS MODIFY from SUPPLIER_AUDIT_PLAN where SAP_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("SUPPLIER_AUDIT_PLAN-View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion

    #region dgSupplierAudit_RowDeleting
    protected void dgSupplierAudit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgSupplierAudit.Rows[e.RowIndex].FindControl("lblSAP_CODE"))).Text))
                {
                    string um_code = ((Label)(dgSupplierAudit.Rows[e.RowIndex].FindControl("lblSAP_CODE"))).Text;

                    DataTable DtDetails = CommonClasses.Execute("select * from SUPPLIER_AUDIT_PLAN WHERE  (SUPPLIER_AUDIT_PLAN.ES_DELETE = 0) AND   SUPPLIER_AUDIT_PLAN.SAP_CODE='" + um_code + "' AND SAP_STATUS=1");

                    if (DtDetails.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can Not Delete ,This is Used In other transaction.  ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    bool flag = CommonClasses.Execute1("UPDATE SUPPLIER_AUDIT_PLAN SET ES_DELETE = 1 WHERE SAP_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Supplier Audit Plan", "Delete", "Supplier Audit Plan", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadIssue();
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
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region dgSupplierAudit_RowEditing
    protected void dgSupplierAudit_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    #endregion
}

