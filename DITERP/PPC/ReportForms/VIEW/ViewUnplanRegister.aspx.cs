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

public partial class PPC_ReportForms_VIEW_ViewUnplanRegister : System.Web.UI.Page
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

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='251'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();


            txtMonth.Text = Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy");
            txtTodate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd MMM yyyy");
            LoadCombos(); LoadGroup(); //Call Method
            ddlPartName.Enabled = false;
            txtMonth.Attributes.Add("readonly", "readonly"); //Set Month field readonly
            txtTodate.Attributes.Add("readonly", "readonly");
            chkAllItem.Checked = true;

        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dtPartName = new DataTable();
        dtPartName = CommonClasses.Execute(" SELECT UNPLAN_SALE_SCHEDULE.US_I_CODE AS I_CODE,  I_CODENO + ' - ' + I_NAME AS ICODE_INAME FROM UNPLAN_SALE_SCHEDULE INNER JOIN ITEM_MASTER ON UNPLAN_SALE_SCHEDULE.US_I_CODE = ITEM_MASTER.I_CODE WHERE     (UNPLAN_SALE_SCHEDULE.ES_DELETE = 0) AND (UNPLAN_SALE_SCHEDULE.US_DATE BETWEEN '" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtTodate.Text).ToString("dd/MMM/yyyy") + "' AND US_CM_CODE='" + Session["CompanyCode"].ToString() + "')");
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

                if (chkAllItem.Checked != true)
                    StrCond = StrCond + " SIM_I_CODE = '" + ddlPartName.SelectedValue + "' AND  ";

                //  Response.Redirect("~/PPC/ReportForms/ADD/CastingInventory.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&GroupCode=" + ddlGroup.SelectedValue + "&StrCond=" + StrCond + "", false);
                Response.Redirect("~/PPC/ReportForms/ADD/UnplanRegister.aspx?Title=" + Title + "&FDate=" + txtMonth.Text + "&TDate=" + txtTodate.Text + "&StrCond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Casting Inventory Report", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Casting Inventory Report", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Casting Inventory Report", "ShowMessage", Ex.Message);
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
            ddlPartName.AutoPostBack = true;
        }
        else
        {
            LoadCombos();
            ddlPartName.SelectedIndex = 0;
            ddlPartName.Enabled = true;
            ddlPartName.Focus();
        }
    }
    #endregion

    #region ddlGroup_SelectedIndexChanged
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion ddlGroup_SelectedIndexChanged

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

    #region LoadGroup
    protected void LoadGroup()
    {
        //DataTable dtGroup = new DataTable();
        //dtGroup = CommonClasses.Execute("select distinct GP_NAME,GP_CODE from GROUP_MASTER where ES_DELETE=0 and GP_COMP_ID='" + Session["CompanyId"] + "' ORDER BY GP_NAME");
        //ddlGroup.DataSource = dtGroup;
        //ddlGroup.DataTextField = "GP_NAME";
        //ddlGroup.DataValueField = "GP_CODE";
        //ddlGroup.DataBind();
        //ddlGroup.Items.Insert(0, new ListItem("Select Group Name", "0"));
    }
    #endregion LoadGroup
}
