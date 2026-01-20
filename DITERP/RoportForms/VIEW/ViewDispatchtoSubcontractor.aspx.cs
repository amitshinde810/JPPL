using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewDispatchtoSubcontractor : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    string From = "";
    string To = "";
    #endregion

    #region Page_Load
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
            ddlCustomerName.Enabled = false;
            chkAllComp.Checked = true;
            ddlItemCode.Enabled = false;
            ddlItemName.Enabled = false;

            chkAllItem.Checked = true;
          
           
            
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            LoadCustomer();
            LoadItem();
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,INVOICE_MASTER WHERE INM_P_CODE=P_CODE AND INM_TYPE='OutSUBINM'  AND INM_DATE   BETWEEN  '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and INVOICE_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Sub Contractor", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadItem
    private void LoadItem()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("  SELECT DISTINCT  I_CODE,(I_CODENO +' '+'-'+ I_NAME) AS I_NAME,I_CODENO FROM INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER  where INM_CODE=IND_INM_CODE AND INM_TYPE='OutSUBINM' AND INVOICE_MASTER.ES_DELETE=0 AND I_CODE=IND_I_CODE AND INM_DATE   BETWEEN  '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO");
            ddlItemCode.DataSource = custdet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = custdet;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "LoadCustomer", Ex.Message);
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
            CommonClasses.SendError("Sales Order Report", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Sales Order Report", "ShowMessage", Ex.Message);
            return false;
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
            LoadItem();
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemName.Enabled = false;
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = false;
        }
        else
        {
            ddlItemCode.SelectedIndex = 0;
            ddlItemCode.Enabled = true;
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
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            LoadCustomer();
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            LoadCustomer();
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkDateAll.Checked != true)
            {
                From = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
            }
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
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Sub Contractor";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlCustomerName.Focus();
                    return;
                }
            }
            if (chkAllItem.Checked != true)
            {
                strCondition = strCondition + " AND  IND_I_CODE= '" + ddlItemCode.SelectedValue + "'";
            }

            string Group = "";
            if (rbtGroup.SelectedValue == "0")
            {
                Group = "DateWise";
            }
            if (rbtGroup.SelectedValue == "1")
            {
                Group = "ItemWise";
            }
            if (rbtGroup.SelectedValue == "2")
            {
                Group = "PartyWise";
            }
            string type = "SHOW";
            Response.Redirect("~/RoportForms/ADD/DispatchToSubcontractor.aspx?Title=" + Title + "&Condition=" + strCondition + "&type=" + type + "&FromDate=" + From + "&ToDate=" + To + "&Group=" + Group + "& repType = " + rbtType.SelectedValue, false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnShow_Click", Ex.Message);
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
                if (ddlCustomerName.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Sub Contractor";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlCustomerName.Focus();
                    return;
                }
            }
            if (chkAllComp.Checked != true)
            {
                strCondition = strCondition + " AND  IND_I_CODE= '" + ddlItemCode.SelectedValue + "'";
            }
            string type = "EXPORT";

            string Group = "";
            Response.Redirect("~/RoportForms/ADD/DispatchToSubcontractor.aspx?Title=" + Title + "&Condition=" + strCondition + "&type=" + type + "&FromDate=" + From + "&ToDate=" + To + "&Group=" + Group + "& repType = " + rbtType.SelectedValue, false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("  SELECT DISTINCT  I_CODE,I_NAME,I_CODENO FROM INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER  where INM_CODE=IND_INM_CODE AND INM_TYPE='OutSUBINM' AND INVOICE_MASTER.ES_DELETE=0 AND I_CODE=IND_I_CODE   AND INM_P_CODE='" + ddlCustomerName.SelectedValue + "'  AND INM_DATE   BETWEEN  '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ORDER BY I_CODENO");
            ddlItemCode.DataSource = custdet;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlItemName.DataSource = custdet;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Order Report", "LoadItem", Ex.Message);
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
    }
    #endregion
    
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }
}
