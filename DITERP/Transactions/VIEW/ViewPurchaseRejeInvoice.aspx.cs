using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewPurchaseRejeInvoice : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Event

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadInvoice();
                    if (dgInvoiceDettail.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgInvoiceDettail.DataSource = dtFilter;
                            dgInvoiceDettail.DataBind();
                            dgInvoiceDettail.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Tax Invoice View", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region  btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/PurchaseRejeInvoice.aspx?c_name=" + type, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Tax Invoice", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceDettail_RowDeleting
    protected void dgInvoiceDettail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text))
            {
                try
                {
                    string inv_code = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text;
                    string inv_no = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_NO"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE INVOICE_MASTER SET ES_DELETE = 1 WHERE INM_CODE='" + Convert.ToInt32(inv_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Tax Invoice", "Delete", "Tax Invoice", inv_no, Convert.ToInt32(inv_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        DataTable dt = CommonClasses.Execute("select isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT from INVOICE_MASTER where INM_CODE='" + Convert.ToInt32(inv_code) + "'");
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["INM_IS_SUPPLIMENT"]))
                            {
                            }
                            else
                            {
                                DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + inv_code + " ");
                                for (int i = 0; i < dtq.Rows.Count; i++)
                                {
                                    CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                }
                                flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + inv_code + "' and STL_DOC_TYPE='TAXINV'");
                            }
                        }
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadInvoice();
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Tax Invoice View", "dgInvoiceDettail_RowDeleting", Ex.Message);
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
    #endregion

    #region dgInvoiceDettail_RowCommand
    protected void dgInvoiceDettail_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    string inv_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/PurchaseRejeInvoice.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
                        string inv_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/PurchaseRejeInvoice.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        //string type = "MODIFY";
                        string inv_code = e.CommandArgument.ToString();
                        string type1 = "saleorder";
                        Response.Redirect("~/RoportForms/ADD/PurchaseRejeTaxInvoicePrint.aspx?inv_code=" + inv_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection Tax Invoice View", "dgInvoiceDettail_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceDettail_PageIndexChanging
    protected void dgInvoiceDettail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInvoiceDettail.PageIndex = e.NewPageIndex;
            LoadInvoice();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice View", "dgInvoiceDettail_PageIndexChanging", ex.Message);
        }
    }
    #endregion

    #endregion

    #region User Defined Method

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from INVOICE_MASTER where INM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Tax Invoice View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Tax Invoice View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadInvoice
    private void LoadInvoice()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select P_NAME,INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE from INVOICE_MASTER,PARTY_MASTER where INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE  and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_INVOICE_TYPE= '3' order by INM_CODE DESC");
            dgInvoiceDettail.DataSource = dt;
            dgInvoiceDettail.DataBind();
            if (dgInvoiceDettail.Rows.Count > 0)
            {
                dgInvoiceDettail.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "LoadInvoice", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and (INM_NO like upper('%" + str + "%') OR convert(varchar,INM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) and INM_INVOICE_TYPE ='3' order by INM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and INM_INVOICE_TYPE = '3' order by INM_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgInvoiceDettail.DataSource = dtfilter;
                dgInvoiceDettail.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgInvoiceDettail.DataSource = dtFilter;
                    dgInvoiceDettail.DataBind();
                    dgInvoiceDettail.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice View", "LoadStatus", ex.Message);
        }
    }
    #endregion
    #endregion
}
