using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_VIEW_ViewWeeklyPM : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='201'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = Convert.ToDateTime(DateTime.Now).ToString("MMM yyyy");

                chkToolNo.Checked = true;
                ddlToolNo.Enabled = false;
                LoadWeeks();
                ddlWeek1_SelectedIndexChanged(null, null);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    private void LoadWeeks()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select distinct PM_WEEK,(CASE PM_WEEK when '0' then '1 WEEK' when '1' then '2 WEEK' when '2' then '3 WEEK' when '3' then '4 WEEK' else 'PM_WEEK' END) as PM_WEEKNM from MP_PREVENTIVE_MAINTENANCE_MASTER,WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 and PM_CM_COMP_ID=" + Session["CompanyID"] + " and WPM_WEEK=PM_WEEK and MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0");
        if (dt.Rows.Count > 0)
        {
            ddlWeekly.DataSource = dt;
            ddlWeekly.DataTextField = "PM_WEEKNM";
            ddlWeekly.DataValueField = "PM_WEEK";
            ddlWeekly.DataBind();
            //ddlWeekly.Items.Insert(0, new ListItem("Select Week", "0"));
        }
    }

    private void LoadToolNo()
    {
        DataTable dt = new DataTable();
        string str = "";


        str = str + "WPM_WEEK='" + ddlWeekly.SelectedValue + "' AND ";

        //dt = CommonClasses.Execute("select T_CODE,T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME from TOOL_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER,ITEM_MASTER where " + str + " MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_TOOL_NO=T_CODE AND T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " and PM_MONTH='" + txtFromDate.Text + "' and TOOL_MASTER.ES_DELETE=0 order by T_NAME");
        dt = CommonClasses.Execute("select T_CODE,T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME from TOOL_MASTER,WEEKLY_PREVENTIVE_MAINTENANCE_MASTER,ITEM_MASTER where " + str + " WEEKLY_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND WPM_T_CODE=T_CODE AND T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 and T_CM_COMP_ID='" + Session["CompanyID"] + "' and datepart(mm, WPM_TOOL_COMPLETED_DATE)='" + Convert.ToDateTime(txtFromDate.Text).ToString("MM") + "' and datepart(YYYY, WPM_TOOL_COMPLETED_DATE)='" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy") + "' and TOOL_MASTER.ES_DELETE=0 order by T_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            string StrCond = "";
            string StrParty = "ALL";

            if (txtFromDate.Text != "")
            {
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            }

            StrCond = StrCond + " WPM_WEEK= '" + ddlWeekly.SelectedValue + "' AND ";

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {

                if (chkToolNo.Checked == false)
                {
                    if (ddlToolNo.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Tool No.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        StrCond = StrCond + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
                    }
                }

                Response.Redirect("../../ToolRoom/ADD/WeeklyPM.aspx?Title=" + Title + "&Cond=" + StrCond + "&FDATE=" + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "&tool=" + ddlWeekly.SelectedValue + "&WeekNM=" + ddlWeekly.SelectedItem + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tooling Master Report", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Tooling Master Report", "btnCancel_Click", Ex.Message);
        }
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
            CommonClasses.SendError("Tooling Master Report", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkToolNo_CheckedChanged
    protected void chkToolNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkToolNo.Checked == true)
        {
            ddlToolNo.SelectedIndex = 0;
            ddlToolNo.Enabled = false;
        }
        else
        {
            LoadToolNo();
            ddlToolNo.SelectedIndex = 0;
            ddlToolNo.Enabled = true;
            ddlToolNo.Focus();
        }
    }
    #endregion chkToolNo_CheckedChanged

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadToolNo();
    }
    #endregion

    protected void ddlWeek1_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadToolNo();
    }

    #region dateCheck
    protected void dateCheck()
    {
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;
        string str1 = "";
        string str = "";


        if (From != "" && To != "")
        {
            DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
        }
    }
    #endregion
}
