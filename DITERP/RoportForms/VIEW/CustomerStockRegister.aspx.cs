using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_CustomerStockRegister : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    string From = "";
    string To = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //Menu highlight code
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='65  '");
            right = dtRights.Rows.Count.Equals(0) ? "0000000" : dtRights.Rows[0][0].ToString();
           
            //This code for disable calendar dates which is not in financial year
            //txtFromDate_CalenderExtender.StartDate = Convert.ToDateTime(Session["OpeningDate"]);
            //txtFromDate_CalenderExtender.EndDate = Convert.ToDateTime(Session["ClosingDate"]);
            //CalendarExtender1.StartDate = Convert.ToDateTime(Session["OpeningDate"]);
            //CalendarExtender1.EndDate = Convert.ToDateTime(Session["ClosingDate"]);

            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
            {
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                chkDateAll.Checked = true;
                ddlCustomerName.Enabled = false;
                chkAllComp.Checked = true;
                LoadCustomer();
                LoadItemCodes();
                LoadCategory();
                chkAllCategory.Checked = true;
                chkAllItems.Checked = true;
                ddlItemName.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemCategory.Enabled = false;
                rbtWithAmt.Visible = false;
                txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            }
            else
            {
                Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
            }
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "IWM_DATE between '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
                }
            }
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME from PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL where " + str + " P_TYPE=1 AND PARTY_MASTER.ES_DELETE=0 AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE=P_CODE AND P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " ORDER BY P_NAME ");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Stock Ledger", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadCategory
    private void LoadCategory()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT DISTINCT ITEM_MASTER.I_CAT_CODE,I_CAT_NAME FROM ITEM_CATEGORY_MASTER,ITEM_MASTER,INVOICE_DETAIL,INVOICE_MASTER WHERE INM_TYPE='OutJWINM' AND INM_CODE=IND_INM_CODE AND IND_I_CODE = I_CODE AND ITEM_MASTER.I_CAT_CODE = ITEM_CATEGORY_MASTER.I_CAT_CODE AND  I_CAT_CM_COMP_ID=" + (string)Session["CompanyId"] + "  and ITEM_CATEGORY_MASTER.ES_DELETE=0  ORDER BY I_CAT_NAME");
            ddlItemCategory.DataSource = dt;
            ddlItemCategory.DataTextField = "I_CAT_NAME";
            ddlItemCategory.DataValueField = "I_CAT_CODE";
            ddlItemCategory.DataBind();
            ddlItemCategory.Items.Insert(0, new ListItem("Select Item Category", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Stock Ledger Register", "LoadCategory", Ex.Message);
        }
    }
    #endregion

    #region LoadItemCodes
    private void LoadItemCodes()
    {
        try
        {
            DataTable dtItemCode = new DataTable();
            dtItemCode = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,INVOICE_MASTER,INVOICE_DETAIL WHERE INM_TYPE='OutJWINM' AND INM_CODE = IND_INM_CODE AND IND_I_CODE = I_CODE AND I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0  and INVOICE_MASTER.ES_DELETE=0   ORDER BY I_NAME");
            ddlItemCode.DataSource = dtItemCode;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = dtItemCode;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Stock Ledger", "LoadItemCode", Ex.Message);
        }
    }
    #endregion

    #region LoadItemCodes
    private void LoadItemCodes(string Category)
    {
        try
        {
            DataTable dtItemCode = new DataTable();
            dtItemCode = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,INVOICE_MASTER,INVOICE_DETAIL WHERE INM_TYPE='OutJWINM' AND INM_CODE = IND_INM_CODE AND IND_I_CODE = I_CODE AND I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and INVOICE_MASTER.ES_DELETE=0  ORDER BY I_NAME");

            ddlItemCode.DataSource = dtItemCode;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = dtItemCode;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Stock Ledger", "LoadItemCode", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("Customer Stock Report", "ShowMessage", Ex.Message);
            return false;
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

    #region chkAllComp_CheckedChanged
    protected void chkAllComp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllComp.Checked == true)
        {
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

    #region chkAllCategory_CheckedChanged
    protected void chkAllCategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCategory.Checked == true)
        {
            ddlItemCategory.SelectedIndex = 0;
            ddlItemCategory.Enabled = false;
            LoadItemCodes();
        }
        else
        {
            ddlItemCategory.SelectedIndex = 0;
            ddlItemCategory.Enabled = true;
            ddlItemCategory.Focus();
        }
    }
    #endregion

    #region chkAllItems_CheckedChanged
    protected void chkAllItems_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItems.Checked == true)
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
        }
        else
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = true;
            ddlItemCode.Focus();
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
                LoadItemCodes(ddlItemCategory.SelectedValue);
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
            string str1 = "";
            string strCondition = "";
            string ChkWithAmt = "";
            if (chkDateAll.Checked == false)
            {
                if (txtFromDate.Text == String.Empty || txtToDate.Text == String.Empty)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (chkAllComp.Checked == false)
            {
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Customer Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (chkAllCategory.Checked == false)
            {
                if (ddlItemCategory.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Category";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (chkAllItems.Checked == false)
            {
                if (ddlItemCode.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Item";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (chkAllComp.Checked != true)
            {
                if (ddlCustomerName.SelectedIndex != 0)
                {
                    strCondition = strCondition + " P_CODE= '" + ddlCustomerName.SelectedValue + "'  AND";
                }
            }
            if (chkAllCategory.Checked != true)
            {
                if (ddlItemCategory.SelectedIndex != 0)
                {
                    strCondition = strCondition + " ITEM_MASTER.I_CAT_CODE= '" + ddlItemCategory.SelectedValue + "'  AND";
                }
            }
            if (chkAllItems.Checked != true)
            {
                if (ddlItemCode.SelectedIndex != 0)
                {
                    strCondition = strCondition + " ITEM_MASTER.I_CODE= '" + ddlItemName.SelectedValue + "'  AND";
                }
            }
            if (rbtType.SelectedValue == "0")
            {
                if (chkDateAll.Checked != true)
                {
                    strCondition = strCondition + " CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'  AND  ";
                }
                else
                {
                    strCondition = strCondition + " CL_DOC_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND   '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'   AND ";
                }
            }
            if (rbtType.SelectedValue == "0")
            {
                if (rbtTransType.SelectedValue == "0")
                    str1 = "O2ODetail";
                else
                    str1 = "BOMDetail";
            }
            if (rbtType.SelectedValue == "1")
            {
                str1 = "Summary";
            }
            if (rbtWithAmt.SelectedIndex == 0)
            {
                ChkWithAmt = ""; // rbtWithAmt remove
            }
            else if (rbtWithAmt.SelectedIndex == 1)
            {
                ChkWithAmt = ""; //op
            }
            Response.Redirect("~/RoportForms/ADD/CustomerStockRegister.aspx?Title=" + Title + "&Condition=" + strCondition + "&FromDate=" + From + "&ToDate=" + To + "&type=" + str1 + "&ttype=" + rbtTransType.SelectedValue.ToString() + "&WithAmt=" + ChkWithAmt + "&Party=" + ddlCustomerName.SelectedValue + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Stock Ledger", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string strCondition = "";
            if (chkDateAll.Checked == true)
            {
                strCondition = strCondition + " INM_DATE BETWEEN '" + Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).ToString("dd/MMM/yyyy") + "'  ";
            }
            else
            {
                strCondition = strCondition + " INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  ";
            }
            if (chkAllComp.Checked != true)
            {
                strCondition = strCondition + " AND  INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
            }
            string type = "EXPORT";
            Response.Redirect("~/RoportForms/ADD/DispatchToSubcontractor.aspx?Title=" + Title + "&Condition=" + strCondition + "&type=" + type, false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Stock Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
    }
    #endregion ddlItemCode_SelectedIndexChanged

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region rbtType_SelectedIndexChanged
    protected void rbtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        rbtWithAmt.Visible = false;
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
}
