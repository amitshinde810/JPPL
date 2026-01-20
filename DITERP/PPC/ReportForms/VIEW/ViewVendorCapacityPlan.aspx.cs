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

public partial class PPC_VIEW_ViewVendorCapacityPlan : System.Web.UI.Page
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

            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='206'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtMonth.Text = Convert.ToDateTime(DateTime.Now).ToString("MMM yyyy");
                LoadVendor(); LoadCombos(); //Method Call
                //chkVendorAll.Checked = true;
                chkAllItem.Checked = true;
                ddlPartName.Enabled = false;

                txtMonth.Attributes.Add("readonly", "readonly");
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
        DataTable dtPartName = new DataTable();
        dtPartName = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE WHERE (V.ES_DELETE = 0)   AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME"); //AND   IRN_DATE BETWEEN '" + Convert.ToDateTime(txtMonth.Text).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(txtTodate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "'
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
                if (chkVendorAll.Checked == false)
                {
                    if (ddlVendor.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Vendor Name";
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

                #region StrCond
                string StrCond = "";
                if (chkVendorAll.Checked != true && chkAllItem.Checked == true)
                    StrCond = StrCond + " V.VS_P_CODE='" + ddlVendor.SelectedValue + "' AND  ";
                else if (chkAllItem.Checked != true && chkVendorAll.Checked == true)
                    StrCond = StrCond + " V.VS_I_CODE = '" + ddlPartName.SelectedValue + "' AND  ";
                else if (chkAllItem.Checked != true && chkVendorAll.Checked != true)
                    StrCond = StrCond + " V.VS_P_CODE='" + ddlVendor.SelectedValue + "' AND V.VS_I_CODE = '" + ddlPartName.SelectedValue + "' AND ";
                #endregion StrCond

                Response.Redirect("~/PPC/ReportForms/ADD/VendorCapacityPlan.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&StrCond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Report", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Report", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Report", "ShowMessage", Ex.Message);
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

    #region ddlVendor_SelectedIndexChanged
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtPartName = new DataTable();
        dtPartName = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE WHERE (V.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + (string)Session["CompanyId"] + "' AND V.VS_P_CODE='" + ddlVendor.SelectedValue + "' ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
        ddlPartName.DataSource = dtPartName;
        ddlPartName.DataTextField = "ICODE_INAME";
        ddlPartName.DataValueField = "I_CODE";
        ddlPartName.DataBind();
        ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
    }
    #endregion ddlVendor_SelectedIndexChanged

    #region LoadVendor
    protected void LoadVendor()
    {
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("select DISTINCT P_CODE,P_NAME from VENDOR_SCHEDULE as VS inner join PARTY_MASTER P on VS.VS_P_CODE=P.P_CODE where VS.ES_DELETE=0 AND P.ES_DELETE=0 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by P_NAME");
        ddlVendor.DataSource = dtProcess;
        ddlVendor.DataTextField = "P_NAME";
        ddlVendor.DataValueField = "P_CODE";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, new ListItem("Select Vendor Name", "0"));
    }
    #endregion LoadVendor

    #region chkVendorAll_CheckedChanged
    protected void chkVendorAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVendorAll.Checked == true)
        {
            ddlVendor.SelectedIndex = 0;
            ddlVendor.Enabled = false;
        }
        else
        {
            ddlVendor.SelectedIndex = 0;
            ddlVendor.Enabled = true;
            ddlVendor.Focus();
        }
    }
    #endregion chkVendorAll_CheckedChanged
}
