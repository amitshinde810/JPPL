using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Transactions_VIEW_RejectionToFoundryConReg : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");

            LoadCombos();
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            //txtFromDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            //txtToDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            chkAllItem.Checked = true;
            dateCheck();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CODE in (-2147479680,-2147479679) order by I_CODENO ", ddlItemCode);
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

            dt = CommonClasses.FillCombo("ITEM_MASTER", "I_NAME", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CODE in (-2147479680,-2147479679) order by I_NAME ", ddlFinishedComponent);
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "LoadCombos", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        string From = "";
        string To = "";
        try
        {
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == "" || txtToDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Date";
                    return;
                }
            }
            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item Name";
                    return;
                }
            }
            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
                From = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            }

            string str1 = "";
            string str = "";
            string StrCond = "";

            if (chkDateAll.Checked != true)
            {
                StrCond = StrCond + "  REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text) + "'  AND '" + Convert.ToDateTime(txtToDate.Text) + "'  AND ";
            }
            if (chkAllItem.Checked != true)
            {
                StrCond = StrCond + "  ITEM_MASTER_1.I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND  ";
            }
            Response.Redirect("../../RoportForms/ADD/RejectionToFoundryConReg.aspx?Title=" + Title + "&Date_All=" + chkDateAll.Checked.ToString() + "&FromDate=" + From + "&ToDate=" + To + "&Item_Code=" + ddlFinishedComponent.SelectedValue.ToString() + "&Cond=" + StrCond + "", false);
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
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Store To Foundry Conversion Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Rejection Store To Foundry Conversion Register", "ShowMessage", Ex.Message);
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
        To = txtToDate.Text;
        string str1 = "";
        string str = "";

        if (chkDateAll.Checked == false)
        {
            if (From != "" && To != "")
            {
                DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                    return;
                }
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
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
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
            txtToDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd/MMM/yyyy");
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
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
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    #endregion
}
