using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Transactions_VIEW_ViewPurchaseSchedule : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region PageLoad
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='240'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadPurSchedule();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadPurSchedule
    private void LoadPurSchedule()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select TOP 500 PSM_CODE,PSM_P_CODE,PSM_SCHEDULE_NO,convert(varchar,PSM_SCHEDULE_DATE,106) as PSM_SCHEDULE_DATE,P_NAME from PURCHASE_SCHEDULE_MASTER,PARTY_MASTER where PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 and P_CODE=PSM_P_CODE and PSM_CM_COMP_ID='" + Session["CompanyId"] + "' and P_TYPE='2' order by PSM_CODE desc");

            if (dt.Rows.Count == 0)
            {
                if (dgpurSchedule.Rows.Count == 0)
                {
                    dgpurSchedule.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("PSM_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PSM_SCHEDULE_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PSM_SCHEDULE_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PSM_P_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgpurSchedule.DataSource = dtFilter;
                        dgpurSchedule.DataBind();
                    }
                }
            }
            else
            {
                dgpurSchedule.Enabled = true;
                dgpurSchedule.DataSource = dt;
                dgpurSchedule.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "LoadInward", Ex.Message);
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
            CommonClasses.SendError("Purchase Schedule", "txtString_TextChanged", Ex.Message);
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
            {
                dtfilter = CommonClasses.Execute("SELECT TOP 500 PSM_CODE,PSM_P_CODE,PSM_SCHEDULE_NO,convert(varchar,PSM_SCHEDULE_DATE,106) as PSM_SCHEDULE_DATE,P_NAME FROM PURCHASE_SCHEDULE_MASTER,PARTY_MASTER WHERE PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 and P_CODE=PSM_P_CODE and PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND PURCHASE_SCHEDULE_MASTER.ES_DELETE='0' and (PSM_SCHEDULE_NO like upper('%" + str + "%') OR CONVERT(VARCHAR, PSM_SCHEDULE_DATE, 106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%')) and P_TYPE='2' order by PSM_CODE desc");
            }
            else
            {
                dtfilter = CommonClasses.Execute("select TOP 500 PSM_CODE,PSM_P_CODE,PSM_SCHEDULE_NO,convert(varchar,PSM_SCHEDULE_DATE,106) as PSM_SCHEDULE_DATE,P_NAME FROM PURCHASE_SCHEDULE_MASTER,PARTY_MASTER where PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 and P_CODE=PSM_P_CODE and PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND PURCHASE_SCHEDULE_MASTER.ES_DELETE='0' and P_TYPE='2' order by PSM_CODE desc ");
            }
            if (dtfilter.Rows.Count > 0)
            {
                dgpurSchedule.Enabled = true;
                dgpurSchedule.DataSource = dtfilter;
                dgpurSchedule.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgpurSchedule.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("PSM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PSM_SCHEDULE_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PSM_SCHEDULE_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PSM_P_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgpurSchedule.DataSource = dtFilter;
                    dgpurSchedule.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Schedule", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/PurchaseSchedule.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "you have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgpurSchedule_RowDeleting
    protected void dgpurSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgpurSchedule.Rows[e.RowIndex].FindControl("lblPSM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgpurSchedule.Rows[e.RowIndex].FindControl("lblPSM_CODE"))).Text;
                    string P_code = ((Label)(dgpurSchedule.Rows[e.RowIndex].FindControl("lblPSM_P_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL,PARTY_MASTER", "IWM_P_CODE", "AND IWM_P_CODE=P_CODE and P_INHOUSE_IND=1 and IWD_IWM_CODE=IWM_CODE and PARTY_MASTER.ES_DELETE=0 and INWARD_MASTER.ES_DELETE=0 and IWM_TYPE='IWIM'", P_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Material Inward";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE PURCHASE_SCHEDULE_MASTER SET ES_DELETE = 1 WHERE PSM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Purchase Schedule", "Delete", "Purchase Schedule", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                    LoadPurSchedule();
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Purchase Schedule", "dgpurSchedule_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadPurSchedule();
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

    #region dgpurSchedule_RowEditing
    protected void dgpurSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string cpom_code = ((Label)(dgpurSchedule.Rows[e.NewEditIndex].FindControl("lblPSM_CODE"))).Text;
                string type = "MODIFY";

                if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_PSM_CODE", "AND ES_DELETE=0", cpom_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record, it is used in Material Inspection ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else
                {
                    Response.Redirect("~/Transactions/ADD/PurchaseSchedule.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "dgPODetail_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgpurSchedule_RowCommand
    protected void dgpurSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/PurchaseSchedule.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                    lblmsg.Text = "You have no rights to View";
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

                        if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_PSM_CODE", "AND ES_DELETE=0", cpom_code))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Inspection already done";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {
                            Response.Redirect("~/Transactions/ADD/PurchaseSchedule.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                    lblmsg.Text = "You have no rights to Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else if (e.CommandName.Equals("Delete"))
            {

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from PURCHASE_SCHEDULE_MASTER where PSM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Purchase Schedule", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Purchase Schedule", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgpurSchedule_PageIndexChanging
    protected void dgpurSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgpurSchedule.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
    }
}
