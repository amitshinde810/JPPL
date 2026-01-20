using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

//In this transaction we can directly 
public partial class ToolRoom_VIEW_ViewPMFailuerReport : System.Web.UI.Page
{
    static string right = "";
    static bool CheckModifyLog = false;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='175'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

           
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = Convert.ToDateTime(DateTime.Now).ToString("MMM yyyy");
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
             Loadreport();
        }
    }
    #endregion

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Loadreport();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    protected void dgPMPending_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region UpdateStatus
            if (e.CommandName.Equals("Status"))
            {
                int Index = Convert.ToInt32(e.CommandArgument.ToString());
                GridViewRow row = dgPMPending.Rows[Index];
                string PM_code = ((Label)(row.FindControl("lblT_CODE"))).Text;
                string mt_month = ((Label)(row.FindControl("lblPM_MONTH"))).Text;
                string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;
                string Reason = ((DropDownList)(row.FindControl("ddlReason"))).SelectedValue;

                if (Status == "Close")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Status already close";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else
                {
                    if (Reason != "0")
                    {
                        //Update MP_PREVENTIVE_MAINTENANCE_MASTER flag to 1 means automatically close the mothly PM here...
                        DataTable dt = CommonClasses.Execute("UPDATE MP_PREVENTIVE_MAINTENANCE_MASTER SET PM_STATUS=1,PM_REASON_CODE=" + Reason + " where PM_CODE=" + PM_code + "");
                        CommonClasses.WriteLog("Preventive Maintenance Pending Report", "Save", "Preventive Maintenance Pending Report", PM_code, Convert.ToInt32(PM_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please select Reason";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    Loadreport();
                }
            }
            #endregion Modify
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void dgPMPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT PMR_CODE,PMR_REASON FROM PM_REASON_MASTER WHERE ES_DELETE=0 AND PMR_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");

            if (dt.Rows.Count > 0)
            {
                DropDownList ddlReason = (DropDownList)e.Row.FindControl("ddlReason");

                ddlReason.DataSource = dt;

                ddlReason.DataTextField = "PMR_REASON";

                ddlReason.DataValueField = "PMR_CODE";
                ddlReason.Items.Insert(0, new ListItem("Select Reason ", "0"));
                ddlReason.DataBind();
            }
        }
    }

    #region Loadreport
    private void Loadreport()
    {
        DataTable dt = new DataTable();
        string str = "";

        if (txtFromDate.Text != "")
        {
            str = str + "PM_MONTH='" + txtFromDate.Text + "' AND ";
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Month";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        //here bind tool no. which (PM_TOOL_NO) is not in weekly PM
        dt = CommonClasses.Execute("SELECT DISTINCT PM_CODE,I_NAME,I_CODENO,T_NAME,LEFT(DATENAME(MONTH,PM_MONTH),3) AS PM_MONTH,YEAR(PM_MONTH) AS PM_YEAR,PM_REASON_CODE,PM_STATUS FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,TOOL_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER,ITEM_MASTER WHERE " + str + " TOOL_MASTER.ES_DELETE=0 AND MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=ITEM_MASTER.I_CODE AND MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_STATUS=0 AND T_CODE=PM_TOOL_NO AND PM_TOOL_NO NOT IN (SELECT DISTINCT WPM_T_CODE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER WHERE ES_DELETE=0) and T_CM_COMP_ID='" + Session["CompanyID"].ToString() + "'");

        if (dt.Rows.Count > 0)
        {
            dgPMPending.DataSource = dt;
            dgPMPending.DataBind();
        }
        else
        {
            dgPMPending.DataSource = dt;
            dgPMPending.DataBind();

            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtFromDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Month";
                return;
            }
            Loadreport();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tool History card", "btnShow_Click", ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool History card", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlReason_SelectedIndexChanged
    protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlReason_SelectedIndexChanged
}
