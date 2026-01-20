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

public partial class PPC_VIEW_ViewCustomerSchedule : System.Web.UI.Page
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
                LoadCustomer(); LoadCombos();  //Call Method

                chkAllItem.Checked = true;
                chkCustomerAll.Checked = true;
                ddlPartName.Enabled = false;
                ddlCustomer.Enabled = false;
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
        dtPartName = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME FROM CUSTOMER_SCHEDULE AS C INNER JOIN ITEM_MASTER AS I ON C.CS_I_CODE = I.I_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND C.CS_COMP_ID='" + (string)Session["CompanyId"] + "'  ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
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
                if (chkCustomerAll.Checked == false)
                {
                    if (ddlCustomer.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Customer Name";
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
                if (chkCustomerAll.Checked != true && chkAllItem.Checked == true)
                    StrCond = StrCond + " C.CS_P_CODE='" + ddlCustomer.SelectedValue + "' AND  ";
                else if (chkAllItem.Checked != true && chkCustomerAll.Checked == true)
                    StrCond = StrCond + " C.CS_I_CODE = '" + ddlPartName.SelectedValue + "' AND  ";
                else if (chkAllItem.Checked != true && chkCustomerAll.Checked != true)
                    StrCond = StrCond + " C.CS_P_CODE='" + ddlCustomer.SelectedValue + "' AND C.CS_I_CODE = '" + ddlPartName.SelectedValue + "' AND ";
                #endregion StrCond

                Response.Redirect("~/PPC/ReportForms/ADD/CustomerSchedule.aspx?Title=" + Title + "&Date=" + txtMonth.Text + "&StrCond=" + StrCond + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Schedule Report", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Customer Schedule Report", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Customer Schedule Report", "ShowMessage", Ex.Message);
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

    #region LoadCustomer
    protected void LoadCustomer()
    {
        DataTable dtCustomer = new DataTable();
        dtCustomer = CommonClasses.Execute("select DISTINCT P_CODE,P_NAME from CUSTOMER_SCHEDULE as CS INNER JOIN PARTY_MASTER P ON CS.CS_P_CODE=P.P_CODE WHERE P.ES_DELETE=0 AND CS.ES_DELETE=0 and P_CM_COMP_ID='" + Session["CompanyId"] + "' order by P_NAME");
        ddlCustomer.DataSource = dtCustomer;
        ddlCustomer.DataTextField = "P_NAME";
        ddlCustomer.DataValueField = "P_CODE";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
    }
    #endregion LoadCustomer

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtPartName = new DataTable();
        dtPartName = CommonClasses.Execute("SELECT DISTINCT I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME FROM CUSTOMER_SCHEDULE AS CS INNER JOIN ITEM_MASTER AS I ON CS.CS_I_CODE = I.I_CODE WHERE (CS.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND CS.CS_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND CS.CS_P_CODE='" + ddlCustomer.SelectedValue + "' ORDER BY I.I_CODE,I.I_CODENO + ' - ' + I.I_NAME");
        ddlPartName.DataSource = dtPartName;
        ddlPartName.DataTextField = "ICODE_INAME";
        ddlPartName.DataValueField = "I_CODE";
        ddlPartName.DataBind();
        ddlPartName.Items.Insert(0, new ListItem("Select Part Name And Number", "0"));
    }
    #endregion ddlCustomer_SelectedIndexChanged

    #region chkCustomerAll_CheckedChanged
    protected void chkCustomerAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomerAll.Checked == true)
        {
            ddlCustomer.SelectedIndex = 0;
            ddlCustomer.Enabled = false;
        }
        else
        {
            ddlCustomer.SelectedIndex = 0;
            ddlCustomer.Enabled = true;
            ddlCustomer.Focus();
        }
    }
    #endregion chkCustomerAll_CheckedChanged
}
