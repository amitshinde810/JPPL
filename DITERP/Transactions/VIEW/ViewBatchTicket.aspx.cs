using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Transactions_VIEW_ViewBatchTicket : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    DatabaseAccessLayer databaseaccess = new DatabaseAccessLayer();
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='98'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();

                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BT_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SHM_FORMULA_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BT_IS_AUTHORISE", typeof(string)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgBatch.DataSource = dtFilter;
                        dgBatch.DataBind();
                        dgBatch.Enabled = false;
                    }

                    LoadBatch();
                    CountBatch();
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "Page_Load", Ex.Message);
        }
    }

    private void CountBatch()
    {
        DataTable dtCount = CommonClasses.Execute("select COUNT(BT_NO) as BT_NO from BATCH_MASTER where BT_CM_CODE= '" + Session["CompanyCode"] + "' AND BATCH_MASTER.ES_DELETE=0 ");
        if (dtCount.Rows.Count > 0)
        {
            lblBT_NO_Count.Text = dtCount.Rows[0]["BT_NO"].ToString();
        }
    }
    #endregion

    #region LoadBatch
    private void LoadBatch()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT BT_CODE,BT_NO,convert(varchar,BT_DATE,106) as BT_DATE,SHM_FORMULA_CODE,(case when BT_IS_AUTHORISE=1 then 'true' else 'false' end) as BT_IS_AUTHORISE FROM BATCH_MASTER,SHADE_MASTER WHERE  BT_SHM_CODE=SHM_CODE and SHADE_MASTER.ES_DELETE=0 and BT_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND BATCH_MASTER.ES_DELETE=0 ORDER BY BT_NO desc");
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
                dgBatch.DataSource = dt;
                dgBatch.DataBind();
                dgBatch.Enabled = false;
            }
            else
            {
                dgBatch.DataSource = dt;
                dgBatch.DataBind();
                dgBatch.Enabled = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "LoadBatch", Ex.Message);
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
            CommonClasses.SendError("Batch Ticket", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT BT_CODE,BT_NO,convert(varchar,BT_DATE,106) as BT_DATE,SHM_FORMULA_CODE,(case when BT_IS_AUTHORISE=1 then 'true' else 'false' end) as BT_IS_AUTHORISE FROM BATCH_MASTER,SHADE_MASTER WHERE  BT_SHM_CODE=SHM_CODE and SHADE_MASTER.ES_DELETE=0 and BT_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND BATCH_MASTER.ES_DELETE=0  and (upper(BT_NO) like upper('%" + str + "%') OR upper(BT_DATE) like upper('%" + str + "%') OR upper(SHM_FORMULA_CODE) like upper('%" + str + "%')) ORDER BY BT_NO desc");
            else
                dtfilter = CommonClasses.Execute("SELECT BT_CODE,BT_NO,convert(varchar,BT_DATE,106) as BT_DATE,SHM_FORMULA_CODE,(case when BT_IS_AUTHORISE=1 then 'true' else 'false' end) as BT_IS_AUTHORISE FROM BATCH_MASTER,SHADE_MASTER WHERE  BT_SHM_CODE=SHM_CODE and SHADE_MASTER.ES_DELETE=0 and BT_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND BATCH_MASTER.ES_DELETE=0  ORDER BY BT_NO desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgBatch.DataSource = dtfilter;
                dgBatch.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SHM_FORMULA_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_IS_AUTHORISE", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgBatch.DataSource = dtFilter;
                    dgBatch.DataBind();
                    dgBatch.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Batch Ticket", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/BatchTicket.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "btnAddNew_Click", Ex.Message);
        }
    }

    #endregion

    #region Grid Events

    #region dgBatch_RowEditing
    protected void dgBatch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string ce_code = ((Label)(dgBatch.Rows[e.NewEditIndex].FindControl("lblBT_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Transactions/ADD/BatchTicket.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "dgBatch_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgBatch_RowDeleting
    protected void dgBatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgBatch.Rows[e.RowIndex].FindControl("lblBT_CODE"))).Text))
            {

                string ce_code = ((Label)(dgBatch.Rows[e.RowIndex].FindControl("lblBT_CODE"))).Text;

                if (CommonClasses.CheckUsedInTran("TINTER_SHEET_MASTER", "TSM_BT_CODE", "AND ES_DELETE=0", ce_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Tinter Sheet";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //ShowMessage("#Avisos", "You cant delete this record it has used in Quotation", CommonClasses.MSG_Warning);
                }
                else
                {
                bool flag = CommonClasses.Execute1("UPDATE BATCH_MASTER  SET ES_DELETE = 1 WHERE BT_CODE='" + Convert.ToInt32(ce_code) + "'");
                if (flag == true)
                {
                    CommonClasses.WriteLog("Batch Ticket", "Delete", "Batch Ticket", ce_code, Convert.ToInt32(ce_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record deleted successfully";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            LoadBatch();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record used by another person";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgBatch_PageIndexChanging
    protected void dgBatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgBatch.PageIndex = e.NewPageIndex;
        LoadBatch();
    }
    #endregion

    #region dgBatch_RowCommand
    protected void dgBatch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string ce_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/BatchTicket.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
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
                        string ce_code = e.CommandArgument.ToString();

                        Response.Redirect("~/Transactions/ADD/BatchTicket.aspx?c_name=" + type + "&ce_code=" + ce_code, false);

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
            if (e.CommandName.Equals("Post"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {                      
                        string ce_code = e.CommandArgument.ToString();
                        CommonClasses.Execute("UPDATE BATCH_MASTER SET BT_IS_AUTHORISE=1 WHERE BT_CODE='" + ce_code + "' ");
                        LoadStatus(txtString);
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
                    lblmsg.Text = "You have no rights to Authorization";
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
                    Response.Redirect("~/RoportForms/ADD/BatchTicketPrint.aspx?cpom_code=" + cpom_code, false);
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
            CommonClasses.SendError("Batch Ticket", "dgBatch_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgBatch_RowUpdating
    protected void dgBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from BATCH_MASTER where BT_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Batch Ticket", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Batch Ticket", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void lnkbtnformoreinfo_Click(object sender, EventArgs e)
    {
        string  CompanyCode = Session["CompanyCode"].ToString();
        Response.Redirect("~/RoportForms/ADD/BatchTicketCount.aspx?cpom_code="+CompanyCode ,false );
    }
}
