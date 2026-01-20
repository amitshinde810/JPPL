using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewStockLevel : System.Web.UI.Page
{
    static string right = "";
    string From = "";
    string To = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            ddlComponent.Enabled = false;
            chkAllComp.Checked = true;
            chkAllCategory.Checked = true;
            chkAllItemName.Checked = true;
            ddlItemCategory.Enabled = false;
            ddlItemName.Enabled = false;
            LoadComponent();
            LoadCategory();
            rbtType_SelectedIndexChanged(null, null);
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
        }
    }
    #region LoadComponent
    private void LoadComponent()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE!='" + -2147483648 + "'  ORDER BY I_NAME");

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

    #region LoadCategory
    private void LoadCategory()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_CAT_CODE,I_CAT_NAME FROM ITEM_CATEGORY_MASTER WHERE I_CAT_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  AND I_CAT_CODE!='" + -2147483648 + "' ORDER BY I_CAT_NAME");

            ddlItemCategory.DataSource = dt;
            ddlItemCategory.DataTextField = "I_CAT_NAME";
            ddlItemCategory.DataValueField = "I_CAT_CODE";
            ddlItemCategory.DataBind();
            ddlItemCategory.Items.Insert(0, new ListItem("Select Item Category", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "LoadCategory", Ex.Message);
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

    #region chkAllCategory_CheckedChanged
    protected void chkAllCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCategory.Checked == true)
        {
            ddlItemCategory.SelectedIndex = 0;
            ddlItemCategory.Enabled = false;
            LoadComponent();
        }
        else
        {
            ddlItemCategory.SelectedIndex = 0;
            ddlItemCategory.Enabled = true;
            ddlItemCategory.Focus();
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

    #region ddlItemCategory_SelectedIndexChanged
    protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCategory.SelectedIndex != 0)
            {

                LoadComponent(ddlItemCategory.SelectedValue);
            }
        }
        catch (Exception ex)
        {

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
            string i_code = "";
            string str1 = "";
            string ChkWithAmt = "";
            if (ddlComponent.SelectedIndex != 0)
            {
                i_code = ddlComponent.SelectedValue.ToString();
            }

            if (ddlItemName.SelectedIndex != 0)
            {
                i_code = ddlComponent.SelectedValue.ToString();
            }
            if (rbtType.SelectedIndex == 0)
            {
                str1 = "MIN";

            }
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "MAX";

            }
            string cond = "";
            if (chkAllCategory.Checked == false)
            {
                if (ddlItemCategory.SelectedValue != "0")
                {
                    cond = cond + "  ITEM_MASTER.I_CAT_CODE ='" + ddlItemCategory.SelectedValue + "' AND ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (chkAllComp.Checked == false)
            {
                if (ddlComponent.SelectedValue != "0")
                {
                    cond = cond + "  ITEM_MASTER.I_CODE ='" + ddlComponent.SelectedValue + "' AND ";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "The Field 'Date' is required ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            Response.Redirect("~/RoportForms/ADD/StockLevel.aspx?Title=" + Title + "&detail=" + str1 + "&cond=" + cond + "", false);


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (rbtType.SelectedValue == "0")
        //{
        //    rbtWithAmt.Visible = false;
        //}
        //if (rbtType.SelectedValue == "1")
        //{
        //    rbtWithAmt.Visible = true;
        //}
        //if (rbtType.SelectedValue == "2")
        //{
        //    rbtWithAmt.Visible = false;
        //}
    }


}
