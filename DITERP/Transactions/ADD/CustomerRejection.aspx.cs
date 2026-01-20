using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_CustomerRejection : System.Web.UI.Page
{
    #region Variable
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    DataTable dtInw = new DataTable();
    static int mlCode = 0;
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static DataTable TemTaable = new DataTable();
    static DataTable dtInfo = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    static string right = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        txtInvDate.Attributes.Add("readonly", "readonly");
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        BindTable.Clear();
                        optLstType_SelectedIndexChanged(null, null);
                        LoadTaxName();
                        ViewState["mlCode"] = mlCode;
                        ViewState["str"] = str;
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadCustomer();
                            LoadFilter();
                            txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtGINDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtChallanDate.Attributes.Add("readonly", "readonly");
                            txtGINDate.Attributes.Add("readonly", "readonly");
                            txtInvDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");

                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Customer Rejection", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "Page_Load", ex.Message.ToString());
        }
    }

    private void LoadFilter()
    {
        if (dgCustomerRejection.Rows.Count == 0)
        {
            dtInwardDetail.Clear();

            if (dtInwardDetail.Columns.Count == 0)
            {
                dgCustomerRejection.Enabled = false;
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("UnitCode", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("PO_Code", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("PO_No", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("OriginalQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ChallanQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ReceivedQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Amount", typeof(String)));
                dtInwardDetail.Rows.Add(dtInwardDetail.NewRow());
                dgCustomerRejection.DataSource = dtInwardDetail;
                dgCustomerRejection.DataBind();
            }
        }
    }
    #endregion

    private void LoadTaxName()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ST_TAX_NAME");
            ddlTaxName.DataSource = dt;
            ddlTaxName.DataTextField = "ST_TAX_NAME";
            ddlTaxName.DataValueField = "ST_CODE";
            ddlTaxName.DataBind();
            ddlTaxName.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            ddlTaxName.SelectedValue = "-2147483648";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }
    }

    public void LoadInVNo(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();
            if (ddlCustomer.SelectedIndex != 0)
            {
                if (txtString.Text != "")
                    dtfilter = CommonClasses.Execute("select INM_NO from INVOICE_MASTER, PARTY_MASTER where INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and P_CODE=" + ddlCustomer.SelectedValue + "  AND INM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'  and INM_NO like '%" + str + "%'");
                else
                    dtfilter = CommonClasses.Execute("select INM_NO from INVOICE_MASTER, PARTY_MASTER where INM_P_CODE=P_CODE and INVOICE_MASTER.ES_DELETE=0 and P_CODE=" + ddlCustomer.SelectedValue + "");
                lstview.Items.Clear();
                if (dtfilter.Rows.Count != 0)
                {
                    lstview.Visible = true;
                    for (int i = 0; i < dtfilter.Rows.Count; i++)
                    {
                        lstview.Items.Add(dtfilter.Rows[i][0].ToString());
                    }
                }
                else
                    lstview.Visible = false;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadStatus", ex.Message);
        }
    }
    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        try
        {
            if (txtInvoiceNo.Text.Trim() != "" && txtInvoiceNo.Text.Trim() != "0" )
            {
                dt = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO,CPOD_RATE,CPOD_ORD_QTY,IND_REFUNDABLE_QTY from CUSTPO_MASTER,INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER,CUSTPO_DETAIL where CUSTPO_MASTER.ES_DELETE=0 AND  INVOICE_MASTER.ES_DELETE=0 AND CPOM_CODE=CPOD_CPOM_CODE and CPOM_CODE=IND_CPOM_CODE and I_CODE=CPOD_I_CODE and I_CODE=IND_I_CODE and IND_INM_CODE=INM_CODE  and INM_NO=" + txtInvoiceNo.Text + " AND INM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'  and I_CODE=" + ddlItemCode.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txtOrigionalQty.Text = dt.Rows[0]["IND_REFUNDABLE_QTY"].ToString();
                }
                else
                {
                }
            }
            else
            {
                dt = CommonClasses.Execute("select CPOM_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOM_PONO,CPOM_P_CODE from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CUSTPO_MASTER.ES_DELETE=0 AND  CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE and I_CODE=" + ddlItemCode.SelectedValue + "");
                txtOrigionalQty.Text = "";
            }
            ddlPONo.Items.Clear();
            ddlPONo.SelectedIndex = -1;
            ddlPONo.SelectedValue = null;
            ddlPONo.ClearSelection();
            txtChallanQty.Text = "0.000";
            txtReceivedQty.Text = "0.000";

            if (dt.Rows.Count > 0)
            {
                txtRate.Text = dt.Rows[0]["CPOD_RATE"].ToString();
                ddlPONo.DataSource = dt;
                ddlPONo.DataTextField = "CPOM_PONO";
                ddlPONo.DataValueField = "CPOM_CODE";
                ddlPONo.DataBind();
                ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "LoadPO", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadCustomer();
            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtGINDate.Attributes.Add("readonly", "readonly");
            dtInwardDetail.Clear();

            dt = CommonClasses.Execute("SELECT CR_CODE,CR_TYPE,CR_GIN_NO,CR_GIN_DATE,CR_CHALLAN_NO,CR_CHALLAN_DATE,CR_INV_NO,CR_INV_DATE,CR_P_CODE,CR_NET_AMT,cast((CR_BASIC_EXCISE) as numeric(20,2)) AS CR_BASIC_EXCISE,cast((CR_EDU_CESS) as numeric(20,2)) AS CR_EDU_CESS,cast((CR_H_EDU_CESS) as numeric(20,2)) AS CR_H_EDU_CESS,cast((CR_SALES_TAX) as numeric(20,2)) AS CR_SALES_TAX,cast((CR_GRAND_TOTAL) as numeric(20,2)) AS CR_GRAND_TOTAL,CR_ST_CODE FROM CUSTREJECTION_MASTER where CR_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["CR_CODE"].ToString()); ;
                optLstType.SelectedIndex = (Convert.ToBoolean(dt.Rows[0]["CR_TYPE"].ToString()) == false) ? 0 : 1;
                if (optLstType.SelectedIndex == 1)
                {
                    txtInvoiceNo.Enabled = false;
                    txtInvDate.Enabled = false;
                }
                else
                {
                    txtInvoiceNo.Enabled = true;
                    txtInvDate.Enabled = true;
                }
                optLstType_SelectedIndexChanged(null, null);
                txtGINNo.Text = dt.Rows[0]["CR_GIN_NO"].ToString();
                txtChallanNo.Text = dt.Rows[0]["CR_CHALLAN_NO"].ToString();
                txtGINDate.Text = Convert.ToDateTime(dt.Rows[0]["CR_GIN_DATE"]).ToString("dd MMM yyyy");
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["CR_CHALLAN_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = dt.Rows[0]["CR_P_CODE"].ToString();
                txtInvDate.Text = Convert.ToDateTime(dt.Rows[0]["CR_INV_DATE"]).ToString("dd MMM yyyy");
                txtInvoiceNo.Text = dt.Rows[0]["CR_INV_NO"].ToString();
                txtBasicExcise.Text = dt.Rows[0]["CR_BASIC_EXCISE"].ToString();
                txtHigherEduCess.Text = dt.Rows[0]["CR_H_EDU_CESS"].ToString();
                txtEduCess.Text = (dt.Rows[0]["CR_EDU_CESS"].ToString() == "" ? "0.00" : dt.Rows[0]["CR_EDU_CESS"].ToString());
                ddlTaxName.SelectedValue = (dt.Rows[0]["CR_ST_CODE"].ToString() == "" ? "0" : dt.Rows[0]["CR_ST_CODE"].ToString());
                txtSalesTax.Text = (dt.Rows[0]["CR_SALES_TAX"].ToString() == "" ? "0.00" : dt.Rows[0]["CR_SALES_TAX"].ToString());
                txtGrandAmt.Text = (dt.Rows[0]["CR_GRAND_TOTAL"].ToString() == "" ? "0.00" : dt.Rows[0]["CR_GRAND_TOTAL"].ToString());
                LoadICode();
                LoadIName();
                dtInwardDetail = CommonClasses.Execute("SELECT CD_I_CODE as ItemCode,I_NAME as ItemName,I_UOM_NAME as Unit, CD_UOM as UnitCode,CD_PO_CODE as PO_Code,CPOM_PONO as PO_No, cast(CD_RATE as numeric(20,2)) as Rate,cast(CD_ORIGIONAL_QTY as numeric(10,3)) as  OriginalQty,cast(CD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(CD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(CD_AMOUNT as numeric(20,2)) as  Amount FROM CUSTREJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=CD_UOM and CPOM_CODE=CD_PO_CODE and I_CODE=CD_I_CODE and CD_CR_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgCustomerRejection.Enabled = true;
                    dgCustomerRejection.DataSource = dtInwardDetail;
                    dgCustomerRejection.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                    GetTotal();
                    txtNetAmount_TextChanged(null, null);
                }
            }
            if (optLstType.SelectedValue == "1")
            {
                txtInvoiceNo.Enabled = true;
            }
            else
            {
            }
            if (ViewState["str"].ToString() == "VIEW")
            {
                ddlPONo.Enabled = false;
                txtGINDate.Enabled = false;
                txtInvDate.Enabled = false;
                txtInvoiceNo.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtRate.Enabled = false;
                txtUnit.Enabled = false;
                txtNetAmount.Enabled = false;
                txtChallanQty.Enabled = false;
                txtReceivedQty.Enabled = false;
                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                txtChallanNo.Enabled = false;
                txtChallanDate.Enabled = false;
                dgCustomerRejection.Enabled = false;
            }
            else if (ViewState["str"].ToString() == "MOD")
            {
                ddlCustomer.Enabled = false;
                CommonClasses.SetModifyLock("CUSTREJECTION_MASTER", "MODIFY", "CR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection Transaction", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
                dt = CommonClasses.Execute("select I_CODE,I_CODENO from INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER where INM_NO=" + txtInvoiceNo.Text.Trim() + " AND INM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'  AND  INM_P_CODE=" + ddlCustomer.SelectedValue + "  and IND_INM_CODE=INM_CODE  AND INM_TYPE='TAXINV' AND INVOICE_MASTER.ES_DELETE=0 and I_CODE=IND_I_CODE");
            else
                dt = CommonClasses.Execute("select I_CODE,I_CODENO from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection ", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
                dt = CommonClasses.Execute("select I_CODE,I_NAME from INVOICE_DETAIL,ITEM_MASTER,INVOICE_MASTER where INM_NO=" + txtInvoiceNo.Text.Trim() + " AND  INM_P_CODE=" + ddlCustomer.SelectedValue + "  and IND_INM_CODE=INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='TAXINV'  and I_CODE=IND_I_CODE");
            else
                dt = CommonClasses.Execute("select I_CODE,I_NAME from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE     AND CPOD_I_CODE=I_CODE");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection ", "LoadIName", Ex.Message);
        }
    }
    #endregion

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadIName();
        LoadICode();
    }

    protected void ddlItemCode_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                LoadPO();
                if (ddlPONo.Items.Count > 0)
                {
                    ddlPONo.SelectedIndex = 1;
                }
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");

                if (dt1.Rows.Count > 0)
                {
                    txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                }
                else
                {
                    txtUnit.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Transaction", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                LoadPO();
                if (ddlPONo.Items.Count > 0)
                {
                    ddlPONo.SelectedIndex = 1;
                }
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");

                if (dt1.Rows.Count > 0)
                {
                    txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    lblUnit.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                }
                else
                {
                    txtUnit.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }

    protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        dt = CommonClasses.Execute("select CPOM_CODE,CPOD_ORD_QTY,CPOD_RATE,CPOM_PONO,CPOM_P_CODE from CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER where CPOM_P_CODE=" + ddlCustomer.SelectedValue + " AND CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE and CPOM_CODE=" + ddlPONo.SelectedValue + "");
        if (dt.Rows.Count > 0)
        {
            txtRate.Text = dt.Rows[0]["CPOD_RATE"].ToString();
        }
        else
        {
            txtRate.Text = "0";
        }
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
    }

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            #region validation
            if (txtGINDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter GIN Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtGINDate.Focus();
                return;
            }
            if (txtChallanNo.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Challan No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanNo.Focus();
                return;
            }
            if (txtChallanDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Challan Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanDate.Focus();
                return;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }

            if (ddlItemCode.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            if (txtReceivedQty.Text == "0.00")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtReceivedQty.Focus();
                return;
            }
            if (txtChallanQty.Text == "0.00")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
                return;
            }
            if (optLstType.SelectedValue == "1")
            {
                if (txtInvoiceNo.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Invoice No";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInvoiceNo.Focus();
                    return;
                }
                if (txtInvDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Invoice Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInvDate.Focus();
                    return;
                }
            }
            if (optLstType.SelectedValue == "2")
            {
                if (ddlPONo.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select PO";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPONo.Focus();
                    return;
                }
            }
            if (optLstType.SelectedValue == "1")
            {
                if (ddlPONo.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select PO";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPONo.Focus();
                    return;
                }
            }

            if (dgCustomerRejection.Rows.Count > 0)
            {
                for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region datatable structure
            PanelMsg.Visible = false;
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit");
                ((DataTable)ViewState["dt2"]).Columns.Add("UnitCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_Code");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_No");
                ((DataTable)ViewState["dt2"]).Columns.Add("Rate");
                ((DataTable)ViewState["dt2"]).Columns.Add("OriginalQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("ChallanQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("ReceivedQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("Amount");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemName.SelectedValue;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["Unit"] = txtUnit.Text;
            dr["UnitCode"] = lblUnit.Text;
            if (txtOrigionalQty.Text == "")
            {
                txtOrigionalQty.Text = "0.000";
            }
            else
            {
            }
            if (txtInvoiceNo.Text != "0" && txtInvoiceNo.Text != "")
            {
                dr["OriginalQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtOrigionalQty.Text)), 3));
            }
            else
            {
                dr["OriginalQty"] = "0.000";
            }
            if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Received Qty should be less than or equal to Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
            }
            else
                dr["ChallanQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtChallanQty.Text)), 3));

            if (txtInvoiceNo.Text != "0" && txtInvoiceNo.Text != "")
            {
                if (Convert.ToDouble(txtOrigionalQty.Text) < Convert.ToDouble(txtChallanQty.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Challan Qty should be less than or equal to Original Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtChallanQty.Focus();
                    return;
                }
            }
            dr["ReceivedQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtReceivedQty.Text)), 3));
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));
            if (ddlPONo.SelectedIndex > 0)
            {
                dr["PO_Code"] = ddlPONo.SelectedValue;
                dr["PO_No"] = ddlPONo.SelectedItem;
            }
            dr["Amount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtChallanQty.Text)), 2));
            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                dgCustomerRejection.Enabled = true;
                dgCustomerRejection.Visible = true;
                dgCustomerRejection.DataSource = ((DataTable)ViewState["dt2"]);
                dgCustomerRejection.DataBind();
            }
            #endregion

            GetTotal();
            txtNetAmount_TextChanged(null, null);
            CalGrandTotal();
            clearDetail();
            ddlPONo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double EBasicPer = 0;
        double EEduCessPer = 0;
        double EHEduCessPer = 0;
        double EBasic = 0;
        double EEduCess = 0;
        double EHEduCess = 0;
        if (dgCustomerRejection.Rows.Count > 0)
        {
            for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblAmount"))).Text;
                string Icode = ((Label)(dgCustomerRejection.Rows[i].FindControl("lblItemCode"))).Text;
                double Amount = Convert.ToDouble(QED_AMT);

                if (i == 0)
                {
                    DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + Icode + "");
                    if (dtExcisePer.Rows.Count > 0)
                    {
                        EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
                        EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
                        EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
                    }
                }
                DataTable dtCompState = CommonClasses.Execute("SELECT CM_STATE FROM COMPANY_MASTER WHERE CM_ID=" + (string)Session["CompanyId"] + "");
                DataTable dtPartyStae = CommonClasses.Execute("SELECT P_SM_CODE FROM PARTY_MASTER WHERE P_CODE='" + ddlCustomer.SelectedValue + "'");

                if (dtCompState.Rows[0][0].ToString() == dtPartyStae.Rows[0][0].ToString())
                {
                    txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(EBasicPer, 2));
                    txtducexcper.Text = string.Format("{0:0.00}", Math.Round(EEduCessPer, 2));
                    txtSHEExcPer.Text = string.Format("{0:0.00}", 0);
                    EBasic = EBasic + Math.Round(((Amount * EBasicPer) / 100), 0);
                    EEduCess = EEduCess + Math.Round(((Amount * EEduCessPer) / 100), 0);
                }
                else
                {
                    txtBasicExcPer.Text = string.Format("{0:0.00}", 0);
                    txtducexcper.Text = string.Format("{0:0.00}", 0);
                    txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(EHEduCessPer, 2));
                    EHEduCess = EHEduCess + Math.Round(((Amount * EHEduCessPer) / 100), 0);
                }
                decTotal = decTotal + Amount;
            }
        }

        txtNetAmount.Text = string.Format("{0:0.00}", decTotal);

        if (dgCustomerRejection.Enabled)
        {
        }
        else
        {
            txtNetAmount.Text = "0.00";
            txtBasicExcise.Text = "0.00";
            txtEduCess.Text = "0.00";
            txtHigherEduCess.Text = "0.00";
        }
        if (txtNetAmount.Text == "")
        {
            txtNetAmount.Text = "0.00";
        }
        if (txtBasicExcise.Text == "")
        {
            txtBasicExcise.Text = "0.00";
        }
        if (txtBasicExcPer.Text == "")
        {
            txtBasicExcPer.Text = "0.00";
        }
        if (txtducexcper.Text == "")
        {
            txtducexcper.Text = "0.00";
        }
        if (txtSHEExcPer.Text == "")
        {
            txtSHEExcPer.Text = "0.00";
        }
        if (txtSalesTaxPer.Text == "")
        {
            txtSalesTaxPer.Text = "0.00";
        }
        if (txtSalesTax.Text == "")
        {
            txtSalesTax.Text = "0.00";
        }
        if (txtGrandAmt.Text == "")
        {
            txtGrandAmt.Text = "0.00";
        }
        txtBasicExcise.Text = string.Format("{0:0.00}", Math.Round(EBasic, 2));
        txtEduCess.Text = string.Format("{0:0.00}", Math.Round(EEduCess, 2));
        txtHigherEduCess.Text = string.Format("{0:0.00}", Math.Round(EHEduCess, 2));

        if (txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
        {
            //DataTable dtSTAX = new DataTable();
            //dtSTAX = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from INVOICE_MASTER,SALES_TAX_MASTER where ST_CODE=INM_T_CODE AND INM_NO=" + txtInvoiceNo.Text);
            //if (dtSTAX.Rows.Count > 0)
            //{
            //    txtSalesTax.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text)) * (Convert.ToDouble(dtSTAX.Rows[0]["ST_SALES_TAX"].ToString()) / 100)));
            //    txtSalesTaxPer.Text = dtSTAX.Rows[0]["ST_SALES_TAX"].ToString();
            //    ddlTaxName.SelectedValue = dtSTAX.Rows[0]["ST_CODE"].ToString();
            //}
        }
        else
        {
            //DataTable dtSTAX = new DataTable();
            //if (ddlTaxName.SelectedIndex > 0)
            //{
            //    dtSTAX = CommonClasses.Execute("select ST_SALES_TAX,ST_CODE from SALES_TAX_MASTER where ST_CODE=" + ddlTaxName.SelectedValue);
            //    txtSalesTaxPer.Text = dtSTAX.Rows[0]["ST_SALES_TAX"].ToString();
            //    txtSalesTax.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text)) * Convert.ToDouble(txtSalesTaxPer.Text) / 100), 2));
            //}
        }
        CalGrandTotal();
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            txtUnit.Text = "";
            txtOrigionalQty.Text = "0.00";
            txtRate.Text = "0.00";
            txtReceivedQty.Text = "0.00";
            txtChallanQty.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click1
    protected void btnSubmit_Click1(object sender, EventArgs e)
    {
        if (ddlTaxName.SelectedIndex == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Tax";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlCustomer.Focus();
            return;
        }
        if (txtChallanNo.Text == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Challan No";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtChallanNo.Focus();
            return;
        }
        if (ddlCustomer.SelectedIndex == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Customer";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlCustomer.Focus();
            return;
        }
        if (dgCustomerRejection.Rows.Count != 0)
        {
            SaveRec();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found In Table";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }
    #endregion btnSubmit_Click1

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                string strSql = "";
                int ginNo = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(CR_GIN_NO),0) as CR_GIN_NO FROM CUSTREJECTION_MASTER WHERE CR_CM_CODE = " + (string)Session["CompanyCode"] + " AND ISNULL(CR_TRANS_TYPE,0)=0 and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    ginNo = Convert.ToInt32(dt.Rows[0]["CR_GIN_NO"]);
                    ginNo = ginNo + 1;
                }
                if (txtInvDate.Text != "" && txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0")
                {
                    strSql = "INSERT INTO CUSTREJECTION_MASTER(CR_TYPE,CR_GIN_NO,CR_GIN_DATE,CR_CHALLAN_NO,CR_CHALLAN_DATE,CR_INV_NO,CR_INV_DATE,CR_P_CODE,CR_NET_AMT,CR_BASIC_EXCISE,CR_EDU_CESS,CR_H_EDU_CESS,CR_CM_COMP_ID,MODIFY,ES_DELETE,CR_GRAND_TOTAL,CR_SALES_TAX,CR_ST_CODE,CR_CM_CODE)VALUES('" + optLstType.SelectedIndex + "'," + ginNo + ",'" + txtGINDate.Text + "','" + txtChallanNo.Text + "','" + txtChallanDate.Text + "'," + txtInvoiceNo.Text + ",'" + txtInvDate.Text + "'," + ddlCustomer.SelectedValue + "," + txtNetAmount.Text + "," + txtBasicExcise.Text + "," + txtEduCess.Text + "," + txtHigherEduCess.Text + ",'" + Convert.ToInt32(Session["CompanyId"]) + "',0,0," + txtGrandAmt.Text + "," + txtSalesTax.Text + "," + ddlTaxName.SelectedValue + ",'" + Convert.ToInt32(Session["CompanyCode"]) + "')";
                }
                else
                {
                    strSql = "INSERT INTO CUSTREJECTION_MASTER(CR_TYPE,CR_GIN_NO,CR_GIN_DATE,CR_CHALLAN_NO,CR_CHALLAN_DATE,CR_INV_NO,CR_INV_DATE,CR_P_CODE,CR_NET_AMT,CR_BASIC_EXCISE,CR_EDU_CESS,CR_H_EDU_CESS,CR_CM_COMP_ID,MODIFY,ES_DELETE,CR_GRAND_TOTAL,CR_SALES_TAX,CR_ST_CODE,CR_CM_CODE)VALUES('" + optLstType.SelectedIndex + "'," + ginNo + ",'" + txtGINDate.Text + "','" + txtChallanNo.Text + "','" + txtChallanDate.Text + "','','" + System.DateTime.Now.ToString("dd MMM yyyy") + "'," + ddlCustomer.SelectedValue + "," + txtNetAmount.Text + "," + txtBasicExcise.Text + "," + txtEduCess.Text + "," + txtHigherEduCess.Text + ",'" + Convert.ToInt32(Session["CompanyId"]) + "',0,0," + txtGrandAmt.Text + "," + txtSalesTax.Text + "," + ddlTaxName.SelectedValue + ",'" + Convert.ToInt32(Session["CompanyCode"]) + "')";
                }
                if (CommonClasses.Execute1(strSql))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(CR_CODE) from CUSTREJECTION_MASTER");
                    for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO CUSTREJECTION_DETAIL(CD_CR_CODE,CD_I_CODE,CD_UOM,CD_PO_CODE,CD_RATE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_AMOUNT) VALUES(" + Code + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblPO_Code")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblAmount")).Text + ")");
                        if (optLstType.SelectedValue == "1")
                        {
                            if (txtInvDate.Text != "" && txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0" && ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text != "0.000")
                            {
                                double N1 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text);
                                double N2 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text);
                                double N3 = N1 - N2;
                                CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY- " + N2 + " WHERE IND_I_CODE IN (SELECT IND_I_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " and IND_I_CODE=" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + ")");
                            }
                        }
                        CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+ " + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + " WHERE I_CODE ='" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "'  ");

                        CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY ,STL_STORE_TYPE ) VALUES ('" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + ginNo + "','IWIC','" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + "',-2147483648)");
                    }
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    CommonClasses.WriteLog("Customer Rejection", "Save", "Customer Rejection", ginNo.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Could not saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion

            #region Update
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE CUSTREJECTION_MASTER SET CR_TYPE='" + optLstType.SelectedIndex + "',CR_GIN_NO=" + txtGINNo.Text + ",CR_GIN_DATE='" + txtGINDate.Text + "',CR_CHALLAN_NO=" + txtChallanNo.Text + ",CR_CHALLAN_DATE='" + txtChallanDate.Text + "',CR_INV_NO=" + txtInvoiceNo.Text + ",CR_INV_DATE='" + txtInvDate.Text + "',CR_P_CODE=" + ddlCustomer.SelectedValue + ",CR_NET_AMT=" + txtNetAmount.Text + ",CR_BASIC_EXCISE=" + txtBasicExcise.Text + ",CR_EDU_CESS=" + txtEduCess.Text + ",CR_H_EDU_CESS=" + txtHigherEduCess.Text + ",CR_GRAND_TOTAL=" + txtGrandAmt.Text + ", CR_SALES_TAX = " + txtSalesTax.Text + ", CR_ST_CODE = " + ddlTaxName.SelectedValue + "  where CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    DataTable DtRej = new DataTable();
                    if (optLstType.SelectedValue == "1")
                    {
                        DtRej = CommonClasses.Execute("SELECT CD_I_CODE,CD_PO_CODE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND  CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND CR_INV_NO='" + txtInvoiceNo.Text + "' AND CR_INV_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'");
                    }
                    else
                    {
                        DtRej = CommonClasses.Execute("SELECT CD_I_CODE,CD_PO_CODE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND  CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'  ");

                    }
                    for (int p = 0; p < DtRej.Rows.Count; p++)
                    {
                        if (optLstType.SelectedValue == "1")
                        {
                            CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY+ " + DtRej.Rows[p]["CD_RECEIVED_QTY"].ToString() + " WHERE IND_I_CODE ='" + DtRej.Rows[p]["CD_I_CODE"].ToString() + "' AND  IND_INM_CODE IN (SELECT IND_INM_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " and IND_I_CODE=" + DtRej.Rows[p]["CD_I_CODE"].ToString() + ")");
                        }
                        CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL- " + DtRej.Rows[p]["CD_RECEIVED_QTY"].ToString() + " WHERE I_CODE ='" + DtRej.Rows[p]["CD_I_CODE"].ToString() + "'  ");

                    }
                    result = CommonClasses.Execute1("DELETE FROM CUSTREJECTION_DETAIL WHERE CD_CR_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND STL_DOC_TYPE='IWIC'");

                    if (result)
                    {
                        for (int i = 0; i < dgCustomerRejection.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO CUSTREJECTION_DETAIL(CD_CR_CODE,CD_I_CODE,CD_UOM,CD_PO_CODE,CD_RATE,CD_ORIGIONAL_QTY,CD_CHALLAN_QTY,CD_RECEIVED_QTY,CD_AMOUNT)VALUES(" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblPO_Code")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgCustomerRejection.Rows[i].FindControl("lblAmount")).Text + ")");
                            if (optLstType.SelectedValue == "1")
                            {
                                if (txtInvDate.Text != "" && txtInvoiceNo.Text != "" && txtInvoiceNo.Text != "0" && ((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text != "")
                                {
                                    double N1 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblOriginalQty")).Text);
                                    double N2 = Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text);
                                    double N3 = N1 - N2;
                                    CommonClasses.Execute1("update INVOICE_DETAIL SET IND_REFUNDABLE_QTY=IND_REFUNDABLE_QTY- " + N2 + " WHERE IND_I_CODE='" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "' AND IND_INM_CODE IN (SELECT IND_INM_CODE FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE and INM_NO=" + txtInvoiceNo.Text + " )");
                                }
                            }
                            CommonClasses.Execute1("update ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+ " + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + " WHERE I_CODE ='" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "'  ");
                            CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY , STL_STORE_TYPE ) VALUES ('" + ((Label)dgCustomerRejection.Rows[i].FindControl("lblItemCode")).Text + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtGINNo.Text + "','IWIC','" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDouble(((Label)dgCustomerRejection.Rows[i].FindControl("lblReceivedQty")).Text) + "' , -2147483648)");
                        }
                        CommonClasses.RemoveModifyLock("CUSTREJECTION_MASTER", "MODIFY", "CR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Customer Rejection", "Update", "Customer Rejection", txtGINNo.ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Invalid Update";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("CUSTREJECTION_MASTER", "MODIFY", "CR_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewCustomerRejection.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Rejection", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtGINDate.Text == "")
            {
                flag = false;
            }
            else if (txtChallanNo.Text == "")
            {
                flag = false;
            }
            else if (txtChallanDate.Text == "")
            {
                flag = false;
            }
            else if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (dgCustomerRejection.Rows.Count > 0)
            {
                flag = true;
            }
            else
            {
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        invoice();
    }

    private void invoice()
    {
        DataTable dtCheckInv = CommonClasses.Execute("select * from INVOICE_MASTER where INM_P_CODE=" + ddlCustomer.SelectedValue + "    AND INM_TYPE='TAXINV' and INM_NO=" + txtInvoiceNo.Text.Trim() + " and INM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'");
        if (dtCheckInv.Rows.Count <= 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter valid invoice no & Date";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
        else
        {
            PanelMsg.Visible = false;
            LoadIName();
            LoadICode();
        }
    }

    protected void txtInvDate_TextChanged(object sender, EventArgs e)
    {
        invoice();
    }

    protected void optLstType_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkSelection();
    }

    public void checkSelection()
    {
        if (optLstType.SelectedIndex == 0)
        {
            txtInvDate.Enabled = true;
            txtInvoiceNo.Enabled = true;
            ddlPONo.Enabled = false;
            ddlTaxName.Enabled = false;
            txtBasicExcPer.Enabled = false;
            txtSHEExcPer.Enabled = false;
            txtducexcper.Enabled = false;
        }
        else
        {
            txtInvDate.Enabled = false;
            txtInvoiceNo.Enabled = false;
            ddlTaxName.Enabled = true;
            txtBasicExcPer.Enabled = true;
            txtSHEExcPer.Enabled = true;
            txtducexcper.Enabled = true;
            ddlPONo.Enabled = true;
            txtInvDate.Text = "";
            txtInvoiceNo.Text = "";
        }
    }

    protected void dgCustomerRejection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgCustomerRejection.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgCustomerRejection.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgCustomerRejection.DataSource = ((DataTable)ViewState["dt2"]);
                dgCustomerRejection.DataBind();

                GetTotal();
                clearDetail();
                if (dgCustomerRejection.Rows.Count == 0)
                    LoadFilter();
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged1(null, null);
                ddlPONo.SelectedValue = ((Label)(row.FindControl("lblPO_Code"))).Text;
                txtReceivedQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblReceivedQty"))).Text);
                txtRate.Text = string.Format("{0:0.00}", ((Label)(row.FindControl("lblRate"))).Text);
                txtChallanQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblChallanQty"))).Text);
                txtOrigionalQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblOriginalQty"))).Text);
                txtUnit.Text = ((Label)(row.FindControl("lblUnit"))).Text;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void dgCustomerRejection_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void txtNetAmount_TextChanged(object sender, EventArgs e)
    {
    }

    public void CalGrandTotal()
    {

        txtGrandAmt.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text == "" ? "0.00" : txtNetAmount.Text) + Convert.ToDouble(txtBasicExcise.Text == "" ? "0.00" : txtBasicExcise.Text) + Convert.ToDouble(txtHigherEduCess.Text == "" ? "0.00" : txtHigherEduCess.Text) + Convert.ToDouble(txtEduCess.Text == "" ? "0.00" : txtEduCess.Text) + Convert.ToDouble(txtSalesTax.Text == "" ? "0.00" : txtSalesTax.Text)));
    }

    protected void txtGrandAmt_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);

        txtChallanQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Received Qty should be less than or equal to Challan Qty";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtChallanQty.Focus();
        }
        if (txtInvoiceNo.Text != "0" && txtInvoiceNo.Text != "" && txtOrigionalQty.Text != "")
        {
            if (Convert.ToDouble(txtOrigionalQty.Text) < Convert.ToDouble(txtChallanQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Challan Qty should be less than or equal to Original Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
            }
        }
    }

    protected void txtReceivedQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtReceivedQty.Text);
        txtReceivedQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Received Qty should be less than or equal to Challan Qty";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtReceivedQty.Focus();
        }
    }

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {
                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {
            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion

    protected void txtBasicExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcPer.Text);
        txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtBasicExcise.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtNetAmount.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100)));
        CalGrandTotal();
    }

    protected void txtducexcper_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtducexcper.Text);
        txtducexcper.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtEduCess.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtBasicExcise.Text)) * (Convert.ToDouble(txtducexcper.Text) / 100)));
        CalGrandTotal();
    }

    protected void txtSalesTaxPer_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtSHEExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSHEExcPer.Text);
        txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        txtHigherEduCess.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtBasicExcise.Text)) * (Convert.ToDouble(txtSHEExcPer.Text) / 100)));
        CalGrandTotal();
    }

    protected void ddlTaxName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text == "0" || txtInvoiceNo.Text == "")
        {
            txtSalesTax.Text = "0";
            txtSalesTaxPer.Text = "0";
            ddlTaxName.SelectedValue = "-2147483648";
        }
        CalGrandTotal();
    }
}
