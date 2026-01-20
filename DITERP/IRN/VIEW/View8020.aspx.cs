using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_View8020 : System.Web.UI.Page
{
    static string right = "";
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='138'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                chkDateAll.Checked = false;
                chkDateAll_CheckedChanged(null, null);
                LoadCombos();
                LoadVendor();
                ddlFinishedComponent.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlVendor.Enabled = false;
                txtFromDate.Attributes.Add("readonly", "readonly");

                chkType.Checked = true;
                chkType_CheckedChanged(null, null);
                chkVendor.Checked = true;
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
        try
        {
            DataTable dtItemDet = new DataTable();
            DateTime date = Convert.ToDateTime(txtFromDate.Text);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            dtItemDet = CommonClasses.Execute("SELECT DISTINCT IRND_I_CODE ,I_CODENO,I_NAME FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER WHERE IRN_CODE=IRND_IRN_CODE   AND IRN_ENTRY.ES_DELETE=0 AND IRND_I_CODE=I_CODE  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND   IRN_DATE between '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' ORDER BY I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "IRND_I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

            ddlItemCode.DataSource = dtItemDet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "IRND_I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadVendor
    private void LoadVendor()
    {
        try
        {
            DataTable dtItemDet = new DataTable();
            if (ddlItemCode.SelectedValue == "0")
            {
                dtItemDet = CommonClasses.Execute("SELECT DISTINCT P_CODE,PARTY_MASTER.P_NAME FROM PARTY_MASTER,IRN_ENTRY,IRN_DETAIL WHERE IRND_IRN_CODE = IRN_CODE AND IRN_ENTRY.ES_DELETE = 0 AND IRND_P_CODE = P_CODE   AND P_CODE NOT IN (-2147483210 ) AND IRN_CM_ID = '" + Session["CompanyId"].ToString() + "' ORDER BY P_NAME");
            }
            else
            {
                dtItemDet = CommonClasses.Execute("SELECT DISTINCT P_CODE,PARTY_MASTER.P_NAME FROM PARTY_MASTER,IRN_ENTRY,IRN_DETAIL WHERE IRND_IRN_CODE = IRN_CODE AND IRN_ENTRY.ES_DELETE = 0 AND IRND_P_CODE = P_CODE   AND P_CODE NOT IN (-2147483210 )  AND  IRND_I_CODE='" + ddlItemCode.SelectedValue + "' AND IRN_CM_ID = '" + Session["CompanyId"].ToString() + "' ORDER BY P_NAME");
            }

            ddlVendor.DataSource = dtItemDet;
            ddlVendor.DataTextField = "P_NAME";
            ddlVendor.DataValueField = "P_CODE";
            ddlVendor.DataBind();
            ddlVendor.Items.Insert(0, new ListItem("Select Vendor", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadVendor", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 5)), this, "For Menu"))
            {
                DateTime date = Convert.ToDateTime(txtFromDate.Text);
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Item Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                string StrCond = "";
                if (chkDateAll.Checked != true)
                {
                    firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                    lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                }
                if (chkVendor.Checked != true)
                {
                    if (ddlVendor.SelectedIndex != 0)
                    {
                        StrCond = StrCond + " ID.IRND_P_CODE='" + ddlVendor.SelectedValue + "' AND ";
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Vendor";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkType.Checked != true)
                {

                    if (ddlType.SelectedValue != "-1")
                    {
                        StrCond = StrCond + " ID.IRND_TYPE='" + ddlType.SelectedValue + "' AND ";
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Type";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                string StrTypeC = "-1";
                if (chkType.Checked != true)
                {
                    StrTypeC = ddlType.SelectedValue.ToString();
                }
                Response.Redirect("../../IRN/ADD/Add8020.aspx?Title=" + Title + "&DATE=" + firstDayOfMonth + "&TDATE=" + lastDayOfMonth + "&i_name=" + ddlFinishedComponent.SelectedValue.ToString() + "&Cond=" + StrCond + "&TypeC=" + StrTypeC + "&Vendor=" + ddlVendor.SelectedValue + "&INH=" + tbType.SelectedValue + "", false);
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To Print.');window.location='../Default.aspx'; </script>");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Issue To Production Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dateCheck
    protected void dateCheck()
    {
        DataTable dt = new DataTable();
        string From = "";
        string To = "";
        From = txtFromDate.Text;

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
            }
        }
        else
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
            DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
            From = From1.ToShortDateString();
            To = To2.ToShortDateString();
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemCode.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("MMM yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
        }
        dateCheck();
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
        LoadVendor();
    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region chkType_CheckedChanged
    protected void chkType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkType.Checked == true)
        {
            LoadCombos();
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = false;
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlType.SelectedIndex = 0;
            ddlType.Enabled = true;
            ddlType.Focus();
        }
    }
    #endregion

    #region ddlVendor_SelectedIndexChanged
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region chkVendor_CheckedChanged
    protected void chkVendor_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVendor.Checked == true)
        {
            LoadVendor();
            tbType.Enabled = true;
            ddlVendor.SelectedIndex = 0;
            ddlVendor.Enabled = false;
            ddlVendor.SelectedIndex = 0;
            ddlVendor.Enabled = false;
        }
        else
        {
            LoadVendor();
            tbType.Enabled = false;
            tbType.SelectedValue = "2";
            ddlVendor.SelectedIndex = 0;
            ddlVendor.Enabled = true;
            ddlVendor.Focus();
        }
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
        LoadVendor();
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }
}
