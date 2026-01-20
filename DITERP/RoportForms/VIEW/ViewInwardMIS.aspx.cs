using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewInwardMIS : System.Web.UI.Page
{
    static string right = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
        if (!IsPostBack)
        {
            LoadCombos();
            LoadType();
            LoadProCode();
            ddlCustomerName.Enabled = false;
            ddlItemCode.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFromDate.Text = Convert.ToDateTime(Session["CompanyOpeningDate"]).ToString("dd MMM yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["CompanyClosingDate"]).ToString("dd MMM yyyy");
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
            chkProCodeAll.Checked = true;
            chkAllPOType.Checked = true;
            ddlPOType.Enabled = false;
            ddlProCode.Enabled = false;
        }
    }

    private void LoadCombos()
    {
        CommonClasses.FillCombo("ITEM_MASTER", "I_NAME", "I_CODE", "ES_DELETE=0 and I_CM_COMP_ID=" + Session["CompanyId"] + " order by I_NAME ", ddlFinishedComponent);
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
        CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 and I_CM_COMP_ID=" + Session["CompanyId"] + " order by I_CODENO ", ddlItemCode);
        ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

        CommonClasses.FillCombo("PARTY_MASTER", "P_NAME", "P_CODE", "ES_DELETE=0 and P_TYPE=2 and P_CM_COMP_ID=" + Session["CompanyId"] + " order by P_NAME ", ddlCustomerName);
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }

    #region LoadType
    private void LoadType()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = CommonClasses.Execute("select distinct(PO_T_CODE) ,PO_T_DESC from PO_TYPE_MASTER where ES_DELETE=0 and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " order by PO_T_DESC");
            ddlPOType.DataSource = dt;
            ddlPOType.DataTextField = "PO_T_DESC";
            ddlPOType.DataValueField = "PO_T_CODE";
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, new ListItem("Purchase Order", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadProCode
    private void LoadProCode()
    {
        DataTable dt = new DataTable();

        try
        {
            dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " order by PROCM_NAME");
            ddlProCode.DataSource = dt;
            ddlProCode.DataTextField = "PROCM_NAME";
            ddlProCode.DataValueField = "PROCM_CODE";
            ddlProCode.DataBind();
            ddlProCode.Items.Insert(0, new ListItem("Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region \btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Inward Supplierwise", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Supplier Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
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

    #region chkAllPOType_CheckedChanged
    protected void chkAllPOType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllPOType.Checked == true)
        {
            ddlPOType.SelectedIndex = 0;
            ddlPOType.Enabled = false;
        }
        else
        {
            ddlPOType.SelectedIndex = 0;
            ddlPOType.Enabled = true;
            ddlPOType.Focus();
        }
    }
    #endregion

    #region chkProCodeAll_CheckedChanged
    protected void chkProCodeAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkProCodeAll.Checked == true)
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = false;
        }
        else
        {
            ddlProCode.SelectedIndex = 0;
            ddlProCode.Enabled = true;
            ddlProCode.Focus();
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
                    lblmsg.Text = "Select Supplier ";
                    return;
                }
            }

            if (chkAllPOType.Checked == false)
            {
                if (ddlPOType.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Po Type ";
                    return;
                }
            }

            if (chkProCodeAll.Checked == false)
            {
                if (ddlProCode.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select Project Code ";
                    return;
                }
            }

            string From = "";
            string To = "";
            From = txtFromDate.Text;
            To = txtToDate.Text;

            string str1 = "";
            string str = "";
            string str2 = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFromDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate And ToDate Must Be In Between Financial Year! ";
                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "FromDate Must Be Equal or Smaller Than ToDate";
                        return;
                    }
                }
            }
            else
            {
                From = Convert.ToDateTime(Session["OpeningDate"]).ToShortDateString();
                To = Convert.ToDateTime(Session["ClosingDate"]).ToShortDateString();
            }

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
                str2 = ddlInwardType.SelectedItem.Text;
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
                str2 = ddlInwardType.SelectedItem.Text;
            }
            #endregion

            string POType = "";
            string strCond = "  IWM_TYPE='" + ddlInwardType.SelectedValue + "'  AND ";

            if (chkDateAll.Checked != true)
            {

                strCond = strCond + " IWM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            else
            {
                strCond = strCond + " IWM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            if (chkAllItem.Checked != true)
            {
                strCond = strCond + " IWD_I_CODE = '" + ddlItemCode.SelectedValue + "'  AND  ";
            }
            if (chkAllCustomer.Checked != true)
            {
                strCond = strCond + " IWM_P_CODE = '" + ddlCustomerName.SelectedValue + "'  AND  ";
            }
            if (ddlInwardType.SelectedValue == "0")
            {
                POType = "SUPP";
                strCond = strCond + " IWM_TYPE = 'IWIM'  AND  ";
            }
            if (ddlInwardType.SelectedValue == "1")
            {
                POType = "SUPP";
                strCond = strCond + " IWM_TYPE = 'OUTCUSTINV'  AND  ";
            }
            if (ddlInwardType.SelectedValue == "2")
            {
                POType = "SUPP";
                strCond = strCond + " IWM_TYPE = 'IWCP'  AND  ";
            }
            if (ddlInwardType.SelectedValue == "3")
            {
                POType = "CUST";
                strCond = strCond + " IWM_TYPE = 'IWIFP'  AND  ";
            }
            if (chkAllPOType.Checked != true)
            {
                if (ddlPOType.SelectedValue != "0")
                {
                    strCond = strCond + " PO_T_CODE = '" + ddlPOType.SelectedValue + "'  AND  ";
                }
            }
            if (chkProCodeAll.Checked != true)
            {
                if (ddlProCode.SelectedValue != "0")
                {
                    strCond = strCond + " PROCM_CODE = '" + ddlProCode.SelectedValue + "'  AND  ";  //Master name PROJECT_CODE_MASTER
                }
            }

            Response.Redirect("../ADD/InwardMIS.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "&InwdType=" + str2 + "&PTYPE=" + POType + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inward Supp Wise", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
    }
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
    }
    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        loadParty();
    }
    #endregion


    private void loadParty()
    {
        DataTable dtparty = new DataTable();

        dtparty = CommonClasses.Execute("SELECT DISTINCT PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME   FROM INWARD_MASTER INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE WHERE    (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND  IWM_TYPE='" + ddlInwardType.SelectedValue + "' AND IWM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ");
        if (dtparty.Rows.Count > 0)
        {
            ddlCustomerName.DataSource = dtparty;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
        }
    }

    private void loadItem()
    {
        DataTable dtparty = new DataTable();
        if (ddlCustomerName.SelectedValue == "0")
        {
            dtparty = CommonClasses.Execute("SELECT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE WHERE (INWARD_MASTER.ES_DELETE = 0)  AND  IWM_TYPE='" + ddlInwardType.SelectedValue + "' AND (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND   IWM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ");
        }
        else
        {
            dtparty = CommonClasses.Execute("SELECT ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE WHERE (INWARD_MASTER.ES_DELETE = 0)  AND  IWM_TYPE='" + ddlInwardType.SelectedValue + "'   AND  IWM_P_CODE='" + ddlCustomerName.SelectedValue + "' AND (INWARD_MASTER.IWM_CM_CODE =  " + Convert.ToInt32(Session["CompanyCode"]) + ") AND (INWARD_MASTER.ES_DELETE = 0) AND   IWM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' ");
        }
        if (dtparty.Rows.Count > 0)
        {
            ddlItemCode.DataSource = dtparty;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));


            ddlFinishedComponent.DataSource = dtparty;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
    }
    protected void ddlInwardType_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadParty();
    }
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomerName.SelectedValue != "0")
        {
            loadItem();
        }
    }
}
