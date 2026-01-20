using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class PPC_VIEW_WeeklySalePlanningReport : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
            home1.Attributes["class"] = "active";

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='230'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtMonth.Text = Convert.ToDateTime(DateTime.Now).ToString("MMM yyyy");
                LoadCombos(); //LoadGroup(); //Call Method
                ddlPartName.Enabled = false;
                ddlWeek.Enabled = false;
                txtMonth.Attributes.Add("readonly", "readonly");
                chkAllWeek.Checked = true;
                chkAllItem.Checked = true;
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        //Join with Process BOM Master With Checking for PBM_MACHINING
        DataTable dtPartName = new DataTable();
        //dtPartName = CommonClasses.Execute("select DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME from PROCESS_BOM_MASTER PBM INNER JOIN ITEM_MASTER AS I ON PBM.PBM_I_CODE = I.I_CODE WHERE PBM.PBM_MACHINING='1' AND PBM.ES_DELETE = 0 AND I.ES_DELETE = 0  AND PBM.PBM_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
        dtPartName = CommonClasses.Execute("select DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME from CUSTOMER_WEEKLY_PLAN CWP INNER JOIN ITEM_MASTER AS I ON CWP.CWP_I_CODE = I.I_CODE WHERE CWP.ES_DELETE=0 AND I.ES_DELETE=0 AND CWP.CWP_DATE='" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' and CWP.CWP_COMP_ID='" + (string)Session["CompanyId"] + "'");
        ddlPartName.DataSource = dtPartName;
        ddlPartName.DataTextField = "ICODE_INAME";
        ddlPartName.DataValueField = "I_CODE";
        ddlPartName.DataBind();
        ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                #region Validation
                if (chkAllWeek.Checked == false)
                {
                    if (ddlWeek.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Week Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkAllItem.Checked == false)
                {
                    if (ddlPartName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Part Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                #endregion Validation

                string StrCond = "";
                if (chkAllItem.Checked == true && chkAllWeek.Checked != true)
                    StrCond = StrCond + " CWP_WEEK='" + ddlWeek.SelectedValue + "' AND  ";
                if (chkAllItem.Checked != true && chkAllWeek.Checked == true)
                    StrCond = StrCond + " SIM_I_CODE = '" + ddlPartName.SelectedValue + "' AND  ";
                if (chkAllWeek.Checked != true && chkAllItem.Checked != true)
                    StrCond = StrCond + " SIM_I_CODE = '" + ddlPartName.SelectedValue + "' AND CWP_WEEK='" + ddlWeek.SelectedValue + "' AND  ";

                Response.Redirect("~/PPC/ReportForms/ADD/WeeklySalePlanningReport.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&StrCond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Foundry Capacity Booking Summary", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PPCDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Foundry Capacity Booking Summary", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Foundry Capacity Booking Summary", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlPartName.SelectedIndex = 0;
            ddlPartName.Enabled = false;
        }
        else
        {
            ddlPartName.SelectedIndex = 0;
            ddlPartName.Enabled = true;
            ddlPartName.Focus();
        }
    }
    #endregion

    #region chkAllWeek_CheckedChanged
    protected void chkAllWeek_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllWeek.Checked == true)
        {
            ddlWeek.SelectedIndex = 0;
            ddlWeek.Enabled = false;
        }
        else
        {
            ddlWeek.SelectedIndex = 0;
            ddlWeek.Enabled = true;
            ddlWeek.Focus();
        }
    }
    #endregion chkAllWeek_CheckedChanged

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region txtMonth_TextChanged
    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        if (txtMonth.Text != "")
        {
            LoadCombos();
        }
    }
    #endregion
}

