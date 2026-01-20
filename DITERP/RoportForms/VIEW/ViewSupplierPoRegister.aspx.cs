using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_VIEW_ViewSupplierPoRegister : System.Web.UI.Page
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
            txtFromDate.Text = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
            txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");

            LoadCombos();
            LoadType();
            LoadProCode();

            ddlPoFor.Enabled = false;
            chkPoFOr.Checked = true;
            ddlCustomerName.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            ddlItemCode.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
            chkProCodeAll.Checked = true;
            chkAllPOType.Checked = true;
            ddlPOType.Enabled = false;
            ddlProCode.Enabled = false;            
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
            From = txtFromDate.Text;
            To = txtToDate.Text;
            string str1 = "";
            string str = "";
            string strCond = "";

            if (chkDateAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(Session["OpeningDate"]);
                    DateTime Date2 = Convert.ToDateTime(Session["ClosingDate"]);
                    if (Convert.ToDateTime(From) < Convert.ToDateTime(Date1) || Convert.ToDateTime(From) > Convert.ToDateTime(Date2) || Convert.ToDateTime(To) < Convert.ToDateTime(Date1) || Convert.ToDateTime(To) > Convert.ToDateTime(Date2))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date And To Date Must Be In Between Financial Year! ";
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
            #region Detail
            if (rbtType.SelectedIndex == 0)
            {
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
            }
            #endregion

            #region Summary
            if (rbtType.SelectedIndex == 1)
            {
                str1 = "Pending";

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

            if (chkDateAll.Checked != true)
            {
                strCond = strCond + " SPOM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            else
            {
                strCond = strCond + " SPOM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MMM/yyyy") + "'  AND  ";
            }
            if (chkAllItem.Checked != true)
            {
                strCond = strCond + " I_CODE = '" + ddlFinishedComponent.SelectedValue + "'  AND  ";
            }
            if (chkAllCustomer.Checked != true)
            {
                strCond = strCond + " P_CODE = '" + ddlCustomerName.SelectedValue + "'  AND  ";
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
            if (chkPoFOr.Checked != true)
            {
                if (ddlPoFor.SelectedValue != "0")
                {
                    if (ddlPoFor.SelectedValue == "-2147483648")
                    {
                        strCond = strCond + "  SPOM_POTYPE=0    AND  ";  //Master name PROJECT_CODE_MASTER
                    }
                    else
                    {
                        strCond = strCond + "   SPOM_POTYPE=1   AND  ";  //Master name PROJECT_CODE_MASTER
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please select Supplier Type";
                    return;
                }
            }
            Response.Redirect("../ADD/SupplierPORegister.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCond + "", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "btnShow_Click", Ex.Message);
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
            CommonClasses.SendError("Supplier Order", "ShowMessage", Ex.Message);
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
            LoadProCode(ddlItemCode.SelectedValue.ToString());
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

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            LoadItem();
            LoadProCode();
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

    #region chkPoFOr_CheckedChanged
    protected void chkPoFOr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPoFOr.Checked == true)
        {
            LoadParty();
            ddlPoFor.SelectedIndex = 0;
            ddlPoFor.Enabled = false;
        }
        else
        {
            ddlPoFor.SelectedIndex = 0;
            ddlPoFor.Enabled = true;
            ddlPoFor.Focus();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        dt = CommonClasses.Execute("SELECT STM_CODE,STM_TYPE_CODE FROM SUPPLIER_TYPE_MASTER where SUPPLIER_TYPE_MASTER.ES_DELETE=0 AND  STM_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY STM_TYPE_CODE");
        ddlPoFor.DataSource = dt;
        ddlPoFor.DataTextField = "STM_TYPE_CODE";
        ddlPoFor.DataValueField = "STM_CODE";
        ddlPoFor.DataBind();
        ddlPoFor.Items.Insert(0, new ListItem("Select Supplier Type ", "0"));

        dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
        ddlFinishedComponent.DataSource = dt;
        ddlFinishedComponent.DataTextField = "I_NAME";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

        dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO");
        ddlItemCode.DataSource = dt;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new ListItem("Select Item Code ", "0"));

        dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER ,SUPP_PO_MASTER where PARTY_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0  AND  SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
        ddlCustomerName.DataSource = dt;
        ddlCustomerName.DataTextField = "P_NAME";
        ddlCustomerName.DataValueField = "P_CODE";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
    }
    #endregion

    #region LoadParty
    private void LoadParty()
    {
        dt = CommonClasses.Execute(" select distinct(P_CODE) ,P_NAME from PARTY_MASTER ,SUPP_PO_MASTER where PARTY_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0  AND  SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
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
        dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
        ddlFinishedComponent.DataSource = dt;
        ddlFinishedComponent.DataTextField = "I_NAME";
        ddlFinishedComponent.DataValueField = "I_CODE";
        ddlFinishedComponent.DataBind();
        ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

        dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO");
        ddlItemCode.DataSource = dt;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new ListItem("Select Item Code ", "0"));
    }
    #endregion

    #region ddlPoFor_SelectedIndexChanged
    protected void ddlPoFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dt = CommonClasses.Execute(" select distinct(P_CODE) ,P_NAME from PARTY_MASTER ,SUPP_PO_MASTER where PARTY_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0  AND  SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 AND P_STM_CODE='" + ddlPoFor.SelectedValue + "' order by P_NAME");
            ddlCustomerName.DataSource = dt;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Party Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlPoFor_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlCustomerName_SelectedIndexChanged
    protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomerName.SelectedValue != "0")
            {
                dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'  ORDER BY I_NAME");
                ddlFinishedComponent.DataSource = dt;
                ddlFinishedComponent.DataTextField = "I_NAME";
                ddlFinishedComponent.DataValueField = "I_CODE";
                ddlFinishedComponent.DataBind();
                ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

                dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY I_CODENO");
                ddlItemCode.DataSource = dt;
                ddlItemCode.DataTextField = "I_CODENO";
                ddlItemCode.DataValueField = "I_CODE";
                ddlItemCode.DataBind();
                ddlItemCode.Items.Insert(0, new ListItem("Select Item Code ", "0"));

                dt = CommonClasses.Execute("SELECT DISTINCT(PO_T_CODE) ,PO_T_DESC FROM PO_TYPE_MASTER,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0  AND PO_TYPE_MASTER.ES_DELETE=0 AND SPOM_TYPE=PO_T_CODE  and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY PO_T_DESC");
                ddlPOType.DataSource = dt;
                ddlPOType.DataTextField = "PO_T_DESC";
                ddlPOType.DataValueField = "PO_T_CODE";
                ddlPOType.DataBind();
                ddlPOType.Items.Insert(0, new ListItem("Select Purchase Order", "0"));

                dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME FROM SUPP_PO_MASTER ,SUPP_PO_DETAILS,PROJECT_CODE_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOM_PROJECT=PROCM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 and  PROJECT_CODE_MASTER.ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + "  AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'    order by PROCM_NAME");
                ddlProCode.DataSource = dt;
                ddlProCode.DataTextField = "PROCM_NAME";
                ddlProCode.DataValueField = "PROCM_CODE";
                ddlProCode.DataBind();
                ddlProCode.Items.Insert(0, new ListItem("Project Code", "0"));
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME");
                ddlFinishedComponent.DataSource = dt;
                ddlFinishedComponent.DataTextField = "I_NAME";
                ddlFinishedComponent.DataValueField = "I_CODE";
                ddlFinishedComponent.DataBind();
                ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));

                dt = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE ITEM_MASTER.ES_DELETE=0 AND   SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND  SPOD_I_CODE=I_CODE  and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "    ORDER BY I_CODENO");
                ddlItemCode.DataSource = dt;
                ddlItemCode.DataTextField = "I_CODENO";
                ddlItemCode.DataValueField = "I_CODE";
                ddlItemCode.DataBind();
                ddlItemCode.Items.Insert(0, new ListItem("Select Item Code ", "0"));

                LoadType();
                LoadProCode();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlCustomerName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlFinishedComponent.SelectedValue = ddlItemCode.SelectedValue;
            LoadProCode(ddlFinishedComponent.SelectedValue.ToString());
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadProCode
    private void LoadProCode(string ICODE)
    {
        DataTable dt = new DataTable();
        try
        {
            string con = "";
            if (chkAllCustomer.Checked == false)
            {
                con = con + "  AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "' ";
            }
            if (chkAllItem.Checked == false)
            {
                con = con + "  AND SPOD_I_CODE='" + ddlItemCode.SelectedValue + "'  ";
            }

            dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME FROM SUPP_PO_MASTER ,SUPP_PO_DETAILS,PROJECT_CODE_MASTER WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOM_PROJECT=PROCM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 and  PROJECT_CODE_MASTER.ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " " + con + "      order by PROCM_NAME");

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

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlItemCode.SelectedValue = ddlFinishedComponent.SelectedValue;
            LoadProCode(ddlItemCode.SelectedValue.ToString());
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlFinishedComponent_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region POtype
    private void POtype(string ICode)
    {
        dt = CommonClasses.Execute("SELECT DISTINCT(PO_T_CODE) ,PO_T_DESC FROM PO_TYPE_MASTER,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0  AND PO_TYPE_MASTER.ES_DELETE=0 AND SPOM_TYPE=PO_T_CODE  and PO_T_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_P_CODE='" + ddlCustomerName.SelectedValue + "'   ORDER BY PO_T_DESC");
        ddlPOType.DataSource = dt;
        ddlPOType.DataTextField = "PO_T_DESC";
        ddlPOType.DataValueField = "PO_T_CODE";
        ddlPOType.DataBind();
        ddlPOType.Items.Insert(0, new ListItem("Select Purchase Order", "0"));
    }
    #endregion

    #region ddlPOType_SelectedIndexChanged
    protected void ddlPOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Order Register", "ddlCustomerName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion
}
