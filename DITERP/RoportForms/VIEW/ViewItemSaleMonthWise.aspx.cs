using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewItemSaleMonthWise : System.Web.UI.Page
{
    static string right = "";
    string From = "";
    string To = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='116'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
            FillCombos();
            chkAllComp.Checked = true;
            chkDateAll.Checked = true;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            ddlComponent.Enabled = false;
            ddlItemName.Enabled = false;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("MMM yyyy");
        }
    }

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("MMM yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    void FillCombos()
    {
        try
        {
            DataTable dtItem = CommonClasses.Execute("SELECT I_NAME,I_CODENO,I_CODE FROM ITEM_MASTER WHERE I_CAT_CODE=-2147483648 AND ES_DELETE=0");

            ddlComponent.DataSource = dtItem;
            ddlComponent.DataTextField = "I_CODENO";
            ddlComponent.DataValueField = "I_CODE";
            ddlComponent.DataBind();
            ddlComponent.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = dtItem;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception)
        {
        }
    }

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

    protected void chkAllItemName_CheckedChanged(object sender, EventArgs e)
    {
    }

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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception)
        {

        }
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
        {
            try
            {
                From = txtFromDate.Text;
                To = txtToDate.Text;
                string i_code = "";
                string str1 = "";
                if (ddlComponent.SelectedIndex != 0)
                {
                    i_code = ddlComponent.SelectedValue.ToString();
                }
                if (ddlItemName.SelectedIndex != 0)
                {
                    i_code = ddlComponent.SelectedValue.ToString();

                }
                //str1 = "Detail";
                Response.Redirect("~/RoportForms/ADD/ItemSaleMonthWise.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&Comp_All=" + chkAllComp.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&Item_Code=" + i_code + "", false);
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("ItemSaleMonthWise", "btnShow_Click", Ex.Message);
            }
        }
    }
    #endregion
}
