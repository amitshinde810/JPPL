using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
public partial class IRN_VIEW_PurchaseScheduleRegister : System.Web.UI.Page
{
    static string right = "";
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            //txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            //txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");

            // LoadCombos();


            txtFromDate.Text = Convert.ToDateTime(System.DateTime.Now).ToString("MMM/yyyy");

            LoadItem();
            ddlType_SelectedIndexChanged(null, null);
            //LoadParty();
            ddlCustomerName.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.Enabled = false;
            // txtFromDate.Enabled = false;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
        }
    }

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
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

            if (chkAllItem.Checked == false)
            {
                if (ddlFinishedComponent.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Item ";
                    return;
                }
            }
            if (chkAllCustomer.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Customer ";
                    return;
                }
            }

            string From = "";
            string To = "";
            From = Convert.ToDateTime(txtFromDate.Text).ToString("01/MMM/yyyy");
            To = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy");
            string str1 = "";
            string str = "";
            string strCond = "";


            #region Detail
            str1 = "All";
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
            #endregion

            #region Summary

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
            #endregion


            strCond = strCond + " PSM_SCHEDULE_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  AND  ";

            if (chkAllItem.Checked != true)
            {
                strCond = strCond + " I_CODE = '" + ddlFinishedComponent.SelectedValue + "' AND ";
            }
            if (chkAllCustomer.Checked != true)
            {
                strCond = strCond + " P_CODE = '" + ddlCustomerName.SelectedValue + "' AND ";
            }
            Response.Redirect("../ADD/PurchaseSCheduleRegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "&Type=" + ddlType.SelectedValue + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
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
            CommonClasses.SendError("Purchase Schedule Register", "ShowMessage", Ex.Message);
            return false;
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
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadItem();
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    //#region LoadCombos
    //private void LoadCombos()
    //{
    //    dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL WHERE ITEM_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_CODE = PSD_PSM_CODE AND PSD_I_CODE = I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY I_NAME");
    //    ddlFinishedComponent.DataSource = dt;
    //    ddlFinishedComponent.DataTextField = "I_NAME";
    //    ddlFinishedComponent.DataValueField = "I_CODE";
    //    ddlFinishedComponent.DataBind();
    //    ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

    //    dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL WHERE ITEM_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_CODE = PSD_PSM_CODE AND PSD_I_CODE = I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY I_CODENO");
    //    ddlItemCode.DataSource = dt;
    //    ddlItemCode.DataTextField = "I_CODENO";
    //    ddlItemCode.DataValueField = "I_CODE";
    //    ddlItemCode.DataBind();
    //    ddlItemCode.Items.Insert(0, new ListItem("Select Item Code ", "0"));

    //    dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME from PARTY_MASTER,PURCHASE_SCHEDULE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
    //    ddlCustomerName.DataSource = dt;
    //    ddlCustomerName.DataTextField = "P_NAME";
    //    ddlCustomerName.DataValueField = "P_CODE";
    //    ddlCustomerName.DataBind();
    //    ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    //}
    //#endregion

    #region LoadParty
    private void LoadParty()
    {
        string str = "";

        str = str + " and PSM_SCHEDULE_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("01/MMM/yyyy") + "' and '" + Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "' ";


        dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME from PARTY_MASTER,PURCHASE_SCHEDULE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1    " + str + "   order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion

    #region LoadItem
    private void LoadItem()
    {
        string str = "";
        string str1 = "";
        //string From = "";
        //string To = "";
        //From = Convert.ToDateTime(txtFromDate.Text).ToString("01/MMM/yyyy");
        //To = Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy");

        str = str + " and PSM_SCHEDULE_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("01/MMM/yyyy") + "' and '" + Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "' ";

        if (chkAllCustomer.Checked != true)
        {

            if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedValue != "")
            {

                str1 = str1 + " and PSM_P_CODE = '" + ddlCustomerName.SelectedValue + "'";
            }
        }

        dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL WHERE ITEM_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_CODE = PSD_PSM_CODE AND PSD_I_CODE = I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " " + str + " " + str1 + "  ORDER BY I_NAME");
        ddlFinishedComponent.DataSource = dt;
        ddlFinishedComponent.DataTextField = "I_NAME";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

        dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL WHERE ITEM_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_CODE = PSD_PSM_CODE AND PSD_I_CODE = I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " " + str + " " + str1 + "  ORDER BY I_CODENO");
        ddlItemCode.DataSource = dt;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new ListItem("Select Item Code ", "0"));
    }
    #endregion

    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadItem();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Register", "ddlCustomerName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Register", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Register", "ddlFinishedComponent_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion
    protected void txtFromDate_OnTextChanged(object sender, EventArgs e)
    {
        LoadParty();
        LoadItem();
    }

    protected void txtToDate_OnTextChanged(object sender, EventArgs e)
    {
        LoadParty();
        LoadItem();
    }

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = "", str = "";
        if (ddlType.SelectedValue == "0")
            type = type + " and P_STM_CODE in('-2147483646','-2147483648')  "; //-2147483646 for Both and -2147483648 - Supplier
        else
            type = type + " and P_STM_CODE in('-2147483646','-2147483647')  "; //-2147483646 for Both and -2147483647  - Subcontractor

        str = str + " and PSM_SCHEDULE_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("01/MMM/yyyy") + "' and '" + Convert.ToDateTime(txtFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy") + "' ";
        dt = CommonClasses.Execute("SELECT DISTINCT(P_CODE),P_NAME from PARTY_MASTER,PURCHASE_SCHEDULE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' " + type + " and P_ACTIVE_IND=1  " + str + "   order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));

    }
    #endregion ddlType_SelectedIndexChanged
}
