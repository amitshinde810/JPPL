using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_ViewCustomerRejectionRegister : System.Web.UI.Page
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
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='53'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
                LoadCombos();
                ddlCustomerName.Enabled = false;
                ddlFinishedComponent.Enabled = false;
                ddlItemName.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                chkDateAll.Checked = true;
                chkAllCustomer.Checked = true;
                chkAllItem.Checked = true;
                dateCheck();
               // txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
               // txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
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
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' and ";
                }
            }

            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM CUSTREJECTION_MASTER,PARTY_MASTER where " + str + " CR_P_CODE =P_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + "  AND ISNULL(CR_TRANS_TYPE,0)=0 ORDER BY P_NAME ");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));

            DataTable dtUserDet = new DataTable();
            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadItem
    private void LoadItem()
    {
        string str = "";
        if (chkDateAll.Checked != true)
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                str = str + "CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' and ";
            }
        }
        if (chkAllCustomer.Checked != true)
        {
            if (ddlCustomerName.SelectedValue != "0")
            {
                str = str + "P_CODE='" + ddlCustomerName.SelectedValue + "'  AND ";
            }
        }

        DataTable dtItemDet = new DataTable();
        dtItemDet = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME FROM CUSTREJECTION_MASTER INNER JOIN CUSTREJECTION_DETAIL ON CUSTREJECTION_MASTER.CR_CODE = CUSTREJECTION_DETAIL.CD_CR_CODE INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE WHERE " + str + " (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_CM_COMP_ID = " + (string)Session["CompanyId"] + ") AND (ITEM_MASTER.ES_DELETE = 0) and (PARTY_MASTER.ES_DELETE=0) AND ISNULL(CR_TRANS_TYPE,0)=0 ORDER BY ITEM_MASTER.I_NAME");

        ddlFinishedComponent.DataSource = dtItemDet;
        ddlFinishedComponent.DataTextField = "I_CODENO";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item code", "0"));

        ddlItemName.DataSource = dtItemDet;
        ddlItemName.DataTextField = "I_NAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
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
            CommonClasses.SendError("Customer Rejection Register", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadCombos();
            LoadItem();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            LoadCombos();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            LoadCombos();
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            LoadCombos();
        }
        dateCheck();
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            LoadItem();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
        }
        else
        {
            LoadItem();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlFinishedComponent.Focus();
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

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Menu"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        return;
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ddlCustomerName.Focus();
                        return;
                    }
                }
                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item  ";
                        ddlFinishedComponent.Focus();
                        return;
                    }
                }
                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;

                string str1 = "";
                string str = "";

                #region Detail
                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                }
                #endregion

                #region Summary
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";

                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";
                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";
                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                }
                #endregion

                string Cond = "";
                if (chkDateAll.Checked != true)
                {
                    Cond = Cond + "  CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'   AND  ";
                }
                else
                {
                    From = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy");

                    Cond = Cond + "  CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'   AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "'   AND  ";
                }
                if (chkAllCustomer.Checked != true)
                {
                    Cond = Cond + "  P_CODE='" + ddlCustomerName.SelectedValue + "'  AND  ";
                }
                if (chkAllItem.Checked != true)
                {
                    Cond = Cond + "   I_CODE='" + ddlFinishedComponent.SelectedValue + "' AND  ";
                }

                Response.Redirect("~/RoportForms/ADD/CustomerRejectionRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + Cond + "", false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Print";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlItemName.SelectedValue = ddlFinishedComponent.SelectedValue;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "ddlFinishedComponent_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlFinishedComponent.SelectedValue = ddlItemName.SelectedValue;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "ddlItemName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadItem();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "ddlCustomerName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtFromDate.Focus();
                return;
            }
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {
            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtToDate.Focus();
                return;
            }
        }
    }
}
