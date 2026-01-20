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

public partial class PPC_VIEW_ViewVendorScheduleApproval : System.Web.UI.Page
{

    #region Variable
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='260'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadVndorSchedule();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadVndorSchedule
    private void LoadVndorSchedule()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(" SELECT DISTINCT  CONVERT(CHAR(4), VSA_MONTH, 100) + CONVERT(CHAR(4), VSA_MONTH, 120) as VSA_MONTH ,VENDORSCHEDULE_APPROVAL.VSA_MONTH AS ssss FROM VENDORSCHEDULE_APPROVAL  where VSA_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND ES_DELETE=0 order by VENDORSCHEDULE_APPROVAL.VSA_MONTH desc");

            if (dt.Rows.Count == 0)
            {
                if (dgVendorSchedule.Rows.Count == 0)
                {
                    dgVendorSchedule.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("VSA_MONTH", typeof(string)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgVendorSchedule.DataSource = dtFilter;
                        dgVendorSchedule.DataBind();
                    }
                }
            }
            else
            {
                dgVendorSchedule.Enabled = true;
                dgVendorSchedule.DataSource = dt;
                dgVendorSchedule.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "LoadInward", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Approval", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT      DISTINCT  CONVERT(CHAR(4), VSA_MONTH, 100) + CONVERT(CHAR(4), VSA_MONTH, 120) as VSA_MONTH,VENDORSCHEDULE_APPROVAL.VSA_MONTH AS ssss FROM VENDORSCHEDULE_APPROVAL WHERE ES_DELETE=0 and   VSA_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'   and (VSA_MONTH like upper('%" + str + "%') OR CONVERT(VARCHAR, VSA_MONTH, 106) like upper('%" + str + "%') OR VSA_MONTH like upper('%" + str + "%'))   order by VENDORSCHEDULE_APPROVAL.VSA_MONTH desc");
            }
            else
            {
                dtfilter = CommonClasses.Execute("SELECT      DISTINCT  CONVERT(CHAR(4), VSA_MONTH, 100) + CONVERT(CHAR(4), VSA_MONTH, 120) as VSA_MONTH ,VENDORSCHEDULE_APPROVAL.VSA_MONTH AS ssss FROM VENDORSCHEDULE_APPROVAL  where ES_DELETE=0 and  VSA_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by VENDORSCHEDULE_APPROVAL.VSA_MONTH desc");
            }
            if (dtfilter.Rows.Count > 0)
            {
                dgVendorSchedule.Enabled = true;
                dgVendorSchedule.DataSource = dtfilter;
                dgVendorSchedule.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgVendorSchedule.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("VSA_MONTH", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgVendorSchedule.DataSource = dtFilter;
                    dgVendorSchedule.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "LoadStatus", ex.Message);
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
                    Response.Redirect("~/PPC/ADD/VendorScheduleApproval.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Vendor Schedule Approval", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgVendorSchedule_RowDeleting
    protected void dgVendorSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgVendorSchedule.Rows[e.RowIndex].FindControl("lblVSA_MONTH"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgVendorSchedule.Rows[e.RowIndex].FindControl("lblVSA_MONTH"))).Text;

                    if (CommonClasses.CheckUsedInTran("VENDOR_SCHEDULE_MASTER,VENDOR_SCHEDULE_DETAIL", "VSM_SCHEDULE_DATE", "  AND VSM_CODE=VSD_PSM_CODE AND VSD_I_CODE IN (  SELECT VSA_I_CODE FROM VENDORSCHEDULE_APPROVAL where VSA_MONTH='" + Convert.ToDateTime(cpom_code).ToString("dd/MMM/yyyy") + "')", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Vendor Schedule";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL SET ES_DELETE = 1 WHERE VSA_MONTH='" + Convert.ToDateTime(cpom_code).ToString("dd/MMM/yyyy") + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Vendor Schedule Approval", "Delete", "Vendor Schedule Approval", cpom_code, Convert.ToInt32(0), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                    LoadVndorSchedule();
                    txtString.Text = "";
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Vendor Schedule Approval", "dgVendorSchedule_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadVndorSchedule();
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

    #region dgVendorSchedule_RowEditing
    protected void dgVendorSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string cpom_code = ((Label)(dgVendorSchedule.Rows[e.NewEditIndex].FindControl("lblVSM_CODE"))).Text;
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
                    Response.Redirect("~/PPC/ADD/VendorScheduleApproval.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            CommonClasses.SendError("Vendor Schedule Approval", "dgPODetail_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgVendorSchedule_RowCommand
    protected void dgVendorSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {

                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/PPC/ADD/VendorScheduleApproval.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

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
                            Response.Redirect("~/PPC/ADD/VendorScheduleApproval.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            CommonClasses.SendError("Vendor Schedule Approval", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from VENDORSCHEDULE_APPROVAL where VSA_MONTH='" + Convert.ToDateTime(PrimaryKey).ToString("dd/MMM/yyyy") + " '   AND ES_DELETE=0");
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
            CommonClasses.SendError("Vendor Schedule Approval", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Approval", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgVendorSchedule_PageIndexChanging
    protected void dgVendorSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgVendorSchedule.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/PPCDefault.aspx", false);
    }
    
}
