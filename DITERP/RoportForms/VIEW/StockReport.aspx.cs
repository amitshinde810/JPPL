using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_StockReport : System.Web.UI.Page
{
    static string right = "";
    string From = "";
    string To = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='152'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            ddlComponent.Enabled = false;
            chkAllComp.Checked = true;
            chkAllItemName.Checked = true;
            ddlItemName.Enabled = false;
            LoadComponent();
            LoadStore();
            rbtType_SelectedIndexChanged(null, null);
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
        }
    }
    #endregion Page_Load

    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE=-2147483648 ORDER BY I_NAME");

            ddlComponent.DataSource = dt;
            ddlComponent.DataTextField = "I_CODENO";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "LoadComponent", Ex.Message);
        }
    }
    #endregion

    #region LoadStore
    private void LoadStore()
    {
        try
        {
            DataTable dtStore = CommonClasses.Execute(" SELECT STORE_CODE,STORE_NAME FROM STORE_MASTER WHERE ES_DELETE=0 ");
            if (dtStore.Rows.Count > 0)
            {
                ddlStore.DataSource = dtStore;
                ddlStore.DataTextField = "STORE_NAME";
                ddlStore.DataValueField = "STORE_CODE";
                ddlStore.DataBind();
                ddlStore.Items.Insert(0, new ListItem("Select Store Name", "0"));
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion LoadStore

    #region LoadComponent by Category
    private void LoadComponent(string CAT_CODE)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE='" + CAT_CODE + "' and ES_DELETE=0  ORDER BY I_NAME");

            ddlComponent.DataSource = dt;
            ddlComponent.DataTextField = "I_CODENO";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "LoadComponent", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception)
        {
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
            CommonClasses.SendError("Stock Ledger Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllItemName_CheckedChanged
    protected void chkAllItemName_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItemName.Checked == true)
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
        }
        else
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlItemName.Focus();
        }
    }
    #endregion

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = false;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
        }
        else
        {
            ddlComponent.SelectedIndex = 0;
            ddlComponent.Enabled = true;
            ddlComponent.Focus();
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlItemName.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region ddlComponent_SelectedIndexChanged
    protected void ddlComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlComponent.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlComponent.SelectedValue;
            }
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlComponent.SelectedValue = ddlItemName.SelectedValue;
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string i_code = ""; string stl_code = "";
            string str1 = "";
            string ChkWithAmt = "";

            if (ddlStore.SelectedIndex != 0)
            {
                stl_code = ddlStore.SelectedValue.ToString();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Store Name";
                return;
            }
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "Detail";
                Response.Redirect("~/RoportForms/ADD/StockReport.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&Comp_All=" + chkAllComp.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&Item_Code=" + ddlComponent.SelectedValue.ToString() + "&detail=" + str1 + "&stl_code=" + stl_code + "", false);
            }
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "Summary";

                Response.Redirect("~/RoportForms/ADD/StockReport.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&Comp_All=" + chkAllComp.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&Item_Code=" + ddlComponent.SelectedValue.ToString() + "&detail=" + str1 + "&stl_code=" + stl_code + "", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtType.SelectedValue == "0")
        {
            rbtWithAmt.Visible = false;
        }
        if (rbtType.SelectedValue == "1")
        {
            rbtWithAmt.Visible = false;//True
        }
        if (rbtType.SelectedValue == "2")
        {
            rbtWithAmt.Visible = false;
        }
    }
    #endregion rbtType_SelectedIndexChanged

    #region rbtWithAmt_SelectedIndexChanged
    protected void rbtWithAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtWithAmt.SelectedValue == "0")
        {
            rbtType.Items[0].Enabled = true;
            rbtType.Items[1].Selected = true;
        }
        if (rbtWithAmt.SelectedValue == "1")
        {
            rbtType.Items[0].Enabled = true;
            rbtType.Items[0].Selected = true;
        }
    }
    #endregion rbtWithAmt_SelectedIndexChanged

    #region ddlStore_SelectedIndexChanged
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStore.SelectedValue != "0")
        {
            DataTable dt = CommonClasses.Execute(" SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,STOCK_LEDGER where ITEM_MASTER.ES_DELETE=0 AND STL_I_CODE=I_CODE   AND STL_STORE_TYPE='" + ddlStore.SelectedValue + "'  AND I_CAT_CODE=-2147483648 AND STL_DOC_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND   '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND I_CM_COMP_ID=" + (string)Session["CompanyId"] + "     ORDER BY I_NAME");

            ddlComponent.DataSource = dt;
            ddlComponent.DataTextField = "I_CODENO";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
    }
    #endregion

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        ddlStore_SelectedIndexChanged(null, null);
    }
}
