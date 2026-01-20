using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_VIEW_ViewSupplierAudit : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='244'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadSupllierAudit();

                    if (dgSuppAudit.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAM_FDATE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgSuppAudit.DataSource = dtFilter;
                            dgSuppAudit.DataBind();
                            dgSuppAudit.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadSupllierAudit
    private void LoadSupllierAudit()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select Top 500 SAM_CODE,P_NAME,datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as SAM_FDATE,convert(varchar,SAM_TDATE,106) as SAM_TDATE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where P_CODE=SAM_P_CODE and PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 order by SAM_CODE desc");
            dgSuppAudit.DataSource = dt;
            dgSuppAudit.DataBind();
            if (dgSuppAudit.Rows.Count > 0)
            {
                dgSuppAudit.Enabled = true;
            }
            else
            {
                if (dt.Rows.Count == 0)
                {
                    if (dgSuppAudit.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SAM_FDATE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgSuppAudit.DataSource = dtFilter;
                            dgSuppAudit.DataBind();
                            dgSuppAudit.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit", "LoadSupllierAudit", Ex.Message);
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
            CommonClasses.SendError("Supplier Audit", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT TOP 500 SAM_CODE,P_NAME,datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as SAM_FDATE,convert(varchar,SAM_TDATE,106) as SAM_TDATE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where P_CODE=SAM_P_CODE and PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 and (SAM_FDATE like upper('%" + str + "%') OR upper(SAM_TDATE) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) order by SAM_CODE desc");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 SAM_CODE,P_NAME,datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as SAM_FDATE,convert(varchar,SAM_TDATE,106) as SAM_TDATE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where P_CODE=SAM_P_CODE and PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 order by SAM_CODE desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgSuppAudit.DataSource = dtfilter;
                dgSuppAudit.DataBind();
                dgSuppAudit.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SAM_FDATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSuppAudit.DataSource = dtFilter;
                    dgSuppAudit.DataBind();
                    dgSuppAudit.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/SupplierAudit.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Supplier Audit", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Supplier Audit", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgSuppAudit_RowDeleting
    protected void dgSuppAudit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgSuppAudit.Rows[e.RowIndex].FindControl("lblSAM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgSuppAudit.Rows[e.RowIndex].FindControl("lblSAM_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE SUPPLIER_AUDIT_MASTER SET ES_DELETE = 1 WHERE SAM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                    if (flag == true)
                    {
                        DataTable dtq = CommonClasses.Execute("SELECT DCD_ORD_QTY,DCD_I_CODE FROM DELIVERY_CHALLAN_DETAIL where DCD_SAM_CODE=" + Convert.ToInt32(cpom_code) + " ");

                        for (int i = 0; i < dtq.Rows.Count; i++)
                        {
                        }
                        CommonClasses.WriteLog("Supplier Audit", "Delete", "Supplier Audit", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        LoadSupllierAudit();
                    }
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Supplier Audit", "dgSuppAudit_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadSupllierAudit();
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

    #region dgSuppAudit_RowCommand
    protected void dgSuppAudit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                return;
            }
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/SupplierAudit.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                    string cpom_code = e.CommandArgument.ToString();
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        int cnt = 0;
                        Response.Redirect("~/Transactions/ADD/SupplierAudit.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/SupplierAuditPrint.aspx?cpom_code=" + cpom_code + "", false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit", "dgSuppAudit_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            System.Data.DataTable dt = CommonClasses.Execute("select ES_MODIFY from SUPPLIER_AUDIT_MASTER where SAM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Supplier Audit", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Supplier Audit", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgSuppAudit_PageIndexChanging
    protected void dgSuppAudit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgSuppAudit.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion
}

