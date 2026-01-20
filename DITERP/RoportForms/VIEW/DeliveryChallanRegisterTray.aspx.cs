using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class RoportForms_VIEW_DeliveryChallanRegisterTray : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
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
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                chkDateAll.Checked = true;
                chkAllCustomer.Checked = true;
                chkAllItem.Checked = true;
                dateCheck();

              
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion Page_Load

    private void LoadItems()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "DCM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            if (chkAllCustomer.Checked != true)
            {
                if (ddlCustomerName.SelectedIndex != 0)
                {
                    str = str + "P_CODE=" + ddlCustomerName.SelectedValue + " and ";
                }
            }
            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_NAME, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_CODE FROM DELIVERY_CHALLAN_MASTER INNER JOIN PARTY_MASTER ON DELIVERY_CHALLAN_MASTER.DCM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN DELIVERY_CHALLAN_DETAIL INNER JOIN ITEM_MASTER ON DELIVERY_CHALLAN_DETAIL.DCD_I_CODE = ITEM_MASTER.I_CODE ON DELIVERY_CHALLAN_MASTER.DCM_CODE = DELIVERY_CHALLAN_DETAIL.DCD_DCM_CODE WHERE " + str + " DCM_TYPE='DLCT' and (DELIVERY_CHALLAN_MASTER.ES_DELETE = 0) AND I_CM_COMP_ID=" + Session["CompanyId"].ToString() + " AND (PARTY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) order by I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "LoadCombos", Ex.Message);
        }
    }

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
                    str = str + "DCM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            DataTable custdet = new DataTable();

            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,DELIVERY_CHALLAN_MASTER WHERE " + str + " DCM_P_CODE=P_CODE and DCM_TYPE='DLCT' and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and DELIVERY_CHALLAN_MASTER.ES_DELETE=0 ORDER BY P_NAME ");

            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Party Name", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Delivery Challan Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadItems();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            LoadItems();
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
            LoadItems();
        }
        else
        {
            LoadCombos();
            LoadItems();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
        dateCheck();
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
            LoadItems();
        }
        else
        {
            LoadItems();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
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
                    lblmsg.Text = "From Date And To Date Must Be In Between Financial Year! ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else if (Date1 > Date2)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "From Date Must Be Equal or Smaller Than To Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
    #endregion dateCheck

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
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlFinishedComponent.Focus();
                        return;
                    }
                }

                string From = "";
                string To = "";
                From = txtFromDate.Text;
                To = txtToDate.Text;
                if (chkDateAll.Checked == false)
                {
                    if (From != "" && To != "")
                    {
                        DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                        DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                        if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "From Date And To Date Must Be In Between Financial Year! ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else if (Date1 > Date2)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "From Date should be less than equal to To Date";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
                else
                {
                    DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                    From = From1.ToShortDateString();
                    To = To2.ToShortDateString();
                }

                string str1 = "";
                string str = "";

                if (rbtGroup.SelectedIndex == 0)
                {
                    str = "Datewise";
                }
                if (rbtGroup.SelectedIndex == 1)
                {
                    str = "SuppWise";
                }
                string strcondition = "";
                if (chkDateAll.Checked != true)
                {
                    strcondition = strcondition + "  DCM_DATE  BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' AND ";
                }
                else
                {
                    strcondition = strcondition + "  DCM_DATE  BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "' AND ";
                }
                if (chkAllCustomer.Checked != true)
                {
                    strcondition = strcondition + "  DCM_P_CODE ='" + ddlCustomerName.SelectedValue + "'  AND ";
                }
                if (chkAllItem.Checked != true)
                {
                    strcondition = strcondition + "  DCD_I_CODE ='" + ddlFinishedComponent.SelectedValue + "'  AND ";
                }
                Response.Redirect("../../RoportForms/ADD/DeliveryChallanRegisterTray.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strcondition + "", false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Print";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion btnShow_Click

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

    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadItems();
    }
}
