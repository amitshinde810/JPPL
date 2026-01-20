using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_PurchaseRejection : System.Web.UI.Page
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
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
                        BindTable.Clear();
                        LoadTaxName();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadSupplier();
                            dt2.Rows.Clear();
                            LoadFilter();
                            str = "INSERT";
                            txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtGINDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");

                            txtChallanDate.Attributes.Add("readonly", "readonly");
                            txtGINDate.Attributes.Add("readonly", "readonly");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Purchase Rejection", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadFilter
    private void LoadFilter()
    {
        if (dgPurchaseRejection.Rows.Count == 0)
        {
            dtInwardDetail.Clear();

            if (dtInwardDetail.Columns.Count == 0)
            {
                dgPurchaseRejection.Enabled = false;
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("UnitCode", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("OriginalQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ChallanQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("ReceivedQty", typeof(String)));
                dtInwardDetail.Columns.Add(new System.Data.DataColumn("Amount", typeof(String)));
                dtInwardDetail.Rows.Add(dtInwardDetail.NewRow());
                dgPurchaseRejection.DataSource = dtInwardDetail;
                dgPurchaseRejection.DataBind();
            }
        }
    }
    #endregion

    #region LoadTaxName
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
            ddlTaxName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "LoadTaxName", Ex.Message);
        }
    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "LoadSupplier", Ex.Message);
        }
    }
    #endregion

    #region LoadIWMNo
    public void LoadIWMNo(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();
            if (ddlSupplier.SelectedIndex != 0)
            {
                if (txtString.Text != "")
                    dtfilter = CommonClasses.Execute("select IWM_NO from INWARD_MASTER, PARTY_MASTER where IWM_P_CODE=P_CODE and INWARD_MASTER.ES_DELETE=0 and P_CODE=" + ddlSupplier.SelectedValue + " and IWM_NO like '%" + str + "%'");
                else
                    dtfilter = CommonClasses.Execute("select IWM_NO from INWARD_MASTER, PARTY_MASTER where IWM_P_CODE=P_CODE and INWARD_MASTER.ES_DELETE=0 and P_CODE=" + ddlSupplier.SelectedValue + "");

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
            CommonClasses.SendError("Purchase Rejection", "LoadStatus", ex.Message);
        }
    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        try
        {
            if (txtInvDate.Text != "" && txtInwardNo.Text != "" && txtInwardNo.Text != "0")
            {
                dt = CommonClasses.Execute("select SPOM_CODE,SPOM_PO_NO,SPOD_RATE,SPOD_ORDER_QTY,IWD_REV_QTY from SUPP_PO_MASTER,INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,SUPP_PO_DETAILS where SPOM_CODE=SPOD_SPOM_CODE and SPOM_CODE=IWD_CPOM_CODE and I_CODE=SPOD_I_CODE and I_CODE=IWD_I_CODE and IWD_IWM_CODE=IWM_CODE and IWM_DATE='" + txtInvDate.Text + "' and IWM_NO=" + txtInwardNo.Text + " and I_CODE=" + ddlItemCode.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    txtOrigionalQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();
                }
                else
                {
                }
            }
            else
            {
                dt = CommonClasses.Execute("select SPOM_CODE,SPOD_ORDER_QTY,SPOD_RATE,SPOM_PO_NO,SPOM_P_CODE from SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SPOM_P_CODE=" + ddlSupplier.SelectedValue + " AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=I_CODE and I_CODE=" + ddlItemCode.SelectedValue + "");
                txtOrigionalQty.Text = "";
            }

            ddlPO.Items.Clear();
            ddlPO.SelectedIndex = -1;
            ddlPO.SelectedValue = null;
            ddlPO.ClearSelection();

            txtChallanQty.Text = "0.000";
            txtReceivedQty.Text = "0.000";

            if (dt.Rows.Count > 0)
            {
                txtRate.Text = dt.Rows[0]["SPOD_RATE"].ToString();
                ddlPO.DataSource = dt;
                ddlPO.DataTextField = "SPOM_PO_NO";
                ddlPO.DataValueField = "SPOM_CODE";
                ddlPO.DataBind();
                ddlPO.Items.Insert(0, new ListItem("Select PO", "0"));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "LoadPO", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadSupplier();

            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtGINDate.Attributes.Add("readonly", "readonly");

            dtInwardDetail.Clear();

            dt = CommonClasses.Execute("SELECT PR_CODE,PR_TYPE,PR_NO,PR_DATE,PR_CHALLAN_NO,PR_CHALLAN_DATE,PR_IWM_NO,PR_IWM_DATE,PR_P_CODE,PR_NET_AMT,cast(isnull(PR_DISC_PER,0) as numeric(20,2)) AS PR_DISC_PER,cast(isnull(PR_DISC_AMT,0) as numeric(20,2)) AS PR_DISC_AMT,cast(isnull(PR_PACKING_AMT,0) as numeric(20,2)) AS PR_PACKING_AMT,cast(isnull(PR_EXC_PER,0) as numeric(20,2)) AS PR_EXC_PER,cast(isnull(PR_EXC_AMT,0) as numeric(20,2)) AS PR_EXC_AMT,cast(isnull(PR_EDU_EXC_PER,0) as numeric(20,2)) AS PR_EDU_EXC_PER,cast(isnull(PR_EDU_EXC_AMT,0) as numeric(20,2)) AS PR_EDU_EXC_AMT,cast(isnull(PR_HEDU_EXC_PER,0) as numeric(20,2)) AS PR_HEDU_EXC_PER,cast(isnull(PR_HEDU_ESC_AMT,0) as numeric(20,2)) AS PR_HEDU_ESC_AMT,cast(isnull(PR_TAX_PER,0) as numeric(20,2)) AS PR_TAX_PER,cast(isnull(PR_TAX_AMT,0) as numeric(20,2)) AS PR_TAX_AMT,cast(isnull(PR_OTHER_AMT,0) as numeric(20,2)) AS PR_OTHER_AMT,cast(isnull(PR_FREIGHT_AMT,0) as numeric(20,2)) AS PR_FREIGHT_AMT,cast(isnull(PR_INSURANCE_AMT,0) as numeric(20,2)) AS PR_INSURANCE_AMT,cast(isnull(PR_TRANS_AMT,0) as numeric(20,2)) AS PR_TRANS_AMT,cast(isnull(PR_OCTRI_AMT,0) as numeric(20,2)) AS PR_OCTRI_AMT,cast(isnull(PR_TCS_AMT,0) as numeric(20,2)) AS PR_TCS_AMT,cast(isnull(PR_ACCESS_VALUE,0) as numeric(20,2)) AS PR_ACCESS_VALUE,cast(isnull(PR_TAXABLE_AMT,0) as numeric(20,2)) AS PR_TAXABLE_AMT,cast(isnull(PR_GRAND_TOT,0) as numeric(20,2)) AS PR_GRAND_TOT,PR_ST_CODE FROM PURCHASE_REJECTION_MASTER where PR_CODE=" + mlCode + "");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["PR_CODE"].ToString()); ;
                optLstType.SelectedIndex = (Convert.ToBoolean(dt.Rows[0]["PR_TYPE"].ToString()) == false) ? 0 : 1;
                if (optLstType.SelectedIndex == 1)
                {
                    txtInwardNo.Enabled = false;
                    txtInvDate.Enabled = false;
                }
                else
                {
                    txtInwardNo.Enabled = true;
                    txtInvDate.Enabled = true;
                }
                optLstType_SelectedIndexChanged(null, null);
                txtGINNo.Text = dt.Rows[0]["PR_NO"].ToString();
                txtChallanNo.Text = dt.Rows[0]["PR_CHALLAN_NO"].ToString();
                txtGINDate.Text = Convert.ToDateTime(dt.Rows[0]["PR_DATE"]).ToString("dd MMM yyyy");
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["PR_CHALLAN_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = dt.Rows[0]["PR_P_CODE"].ToString();
                txtInvDate.Text = Convert.ToDateTime(dt.Rows[0]["PR_IWM_DATE"]).ToString("dd MMM yyyy");
                txtInwardNo.Text = dt.Rows[0]["PR_IWM_NO"].ToString();
                txtDiscPer.Text = dt.Rows[0]["PR_DISC_PER"].ToString();
                txtDiscAmt.Text = dt.Rows[0]["PR_DISC_AMT"].ToString();
                txtPackingAmount.Text = dt.Rows[0]["PR_PACKING_AMT"].ToString();
                txtBasicExcPer.Text = dt.Rows[0]["PR_EXC_PER"].ToString();
                txtBasicExcise.Text = dt.Rows[0]["PR_EXC_AMT"].ToString();
                txtducexcper.Text = dt.Rows[0]["PR_EDU_EXC_PER"].ToString();
                txtEduCess.Text = dt.Rows[0]["PR_EDU_EXC_AMT"].ToString();
                txtSHEExcPer.Text = dt.Rows[0]["PR_HEDU_EXC_PER"].ToString();
                txtHigherEduCess.Text = dt.Rows[0]["PR_HEDU_ESC_AMT"].ToString();
                txtSalesTaxPer.Text = dt.Rows[0]["PR_TAX_PER"].ToString();
                txtSalesTax.Text = dt.Rows[0]["PR_TAX_AMT"].ToString();
                txtOtherCharges.Text = dt.Rows[0]["PR_OTHER_AMT"].ToString();
                txtFreight.Text = dt.Rows[0]["PR_FREIGHT_AMT"].ToString();
                txtInsurance.Text = dt.Rows[0]["PR_INSURANCE_AMT"].ToString();
                txttransport.Text = dt.Rows[0]["PR_TRANS_AMT"].ToString();
                txtOctri.Text = dt.Rows[0]["PR_OCTRI_AMT"].ToString();
                txtTcsAmt.Text = dt.Rows[0]["PR_TCS_AMT"].ToString();
                txtAccessableAmt.Text = dt.Rows[0]["PR_ACCESS_VALUE"].ToString();
                txtTaxableAmt.Text = dt.Rows[0]["PR_TAXABLE_AMT"].ToString();
                txtGrandAmt.Text = dt.Rows[0]["PR_GRAND_TOT"].ToString();
                ddlTaxName.SelectedValue = dt.Rows[0]["PR_ST_CODE"].ToString();
                LoadICode();
                LoadIName();
                dtInwardDetail = CommonClasses.Execute("SELECT PRD_I_CODE as ItemCode,I_NAME as ItemName,I_UOM_NAME as Unit,PRD_UOM as UnitCode,PRD_PO_CODE as PO_CODE,CPOM_PONO as PO_NO, cast(PRD_RATE as numeric(20,2)) as Rate,cast(PRD_ORIGINAL_QTY as numeric(10,3)) as  OriginalQty,cast(PRD_CHALLAN_QTY as numeric(10,3)) as ChallanQty,cast(PRD_RECEIVED_QTY as numeric(10,3)) as ReceivedQty,cast(PRD_AMOUNT as numeric(20,2)) as  Amount FROM PURCHASE_REJECTION_DETAIL,CUSTPO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=PRD_UOM and CPOM_CODE=PRD_PO_CODE and I_CODE=PRD_I_CODE and PRD_PR_CODE=" + mlCode + "");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgPurchaseRejection.Enabled = true;
                    dgPurchaseRejection.DataSource = dtInwardDetail;
                    dgPurchaseRejection.DataBind();
                    dt2 = dtInwardDetail;
                    GetTotal();
                }
            }
            if (optLstType.SelectedValue == "1")
            {
                txtInwardNo.Enabled = true;
            }
            else
            {
            }
            if (str == "VIEW")
            {
                ddlPO.Enabled = false;
                txtGINDate.Enabled = false;
                txtInvDate.Enabled = false;
                txtInwardNo.Enabled = false;
                ddlSupplier.Enabled = false;
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
                dgPurchaseRejection.Enabled = false;
                txtNetAmount.Enabled = false;
                txtDiscPer.Enabled = false;
                txtDiscAmt.Enabled = false;
                txtPackingAmount.Enabled = false;
                txtAccessableAmt.Enabled = false;
                txtBasicExcPer.Enabled = false;
                txtBasicExcise.Enabled = false;
                txtducexcper.Enabled = false;
                txtEduCess.Enabled = false;
                txtSHEExcPer.Enabled = false;
                txtHigherEduCess.Enabled = false;
                txtTaxableAmt.Enabled = false;
                ddlTaxName.Enabled = false;
                txtSalesTaxPer.Enabled = false;
                txtSalesTax.Enabled = false;
                txtOtherCharges.Enabled = false;
                txtFreight.Enabled = false;
                txtInsurance.Enabled = false;
                txttransport.Enabled = false;
                txtOctri.Enabled = false;
                txtTcsAmt.Enabled = false;
                txtGrandAmt.Enabled = false;
            }
            else if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PURCHASE_REJECTION_MASTER", "MODIFY", "PRD_PR_CODE", Convert.ToInt32(mlCode));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            if (txtInvDate.Text != "" && txtInwardNo.Text != "" && txtInwardNo.Text != "0")
                dt = CommonClasses.Execute("select I_CODE,I_CODENO from INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER where IWM_NO=" + txtInwardNo.Text + " and IWD_IWM_CODE=IWM_CODE and IWM_DATE='" + txtInvDate.Text + "' and I_CODE=IWD_I_CODE");
            else
                dt = CommonClasses.Execute("select I_CODE,I_CODENO from SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SPOM_P_CODE=" + ddlSupplier.SelectedValue + " AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=I_CODE");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection ", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            if (txtInvDate.Text != "" && txtInwardNo.Text != "" && txtInwardNo.Text != "0")
                dt = CommonClasses.Execute("select I_CODE,I_NAME from INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER where IWM_NO=" + txtInwardNo.Text + " and IWD_IWM_CODE=IWM_CODE and IWM_DATE='" + txtInvDate.Text + "' and I_CODE=IWD_I_CODE");
            else
                dt = CommonClasses.Execute("select I_CODE,I_NAME from SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SPOM_P_CODE=" + ddlSupplier.SelectedValue + " AND SPOM_CODE=SPOD_SPOM_CODE AND SPOD_I_CODE=I_CODE");
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

    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
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
                if (ddlPO.Items.Count > 0)
                {
                    ddlPO.SelectedIndex = 1;
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
            CommonClasses.SendError("Purchase Rejection", "ddlItemCode_SelectedIndexChanged", Ex.Message);
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
                if (ddlPO.Items.Count > 0)
                {
                    ddlPO.SelectedIndex = 1;
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
            CommonClasses.SendError("Purchase Rejection", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
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
            if (ddlSupplier.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Supplier";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlSupplier.Focus();
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
                if (txtInwardNo.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Inward No";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInwardNo.Focus();
                    return;
                }
                if (txtInvDate.Text == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Inward Date";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInvDate.Focus();
                    return;
                }
            }
            if (optLstType.SelectedValue == "2")
            {
                if (ddlPO.SelectedIndex == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Select PO";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPO.Focus();
                    return;
                }
            }
            if (dgPurchaseRejection.Rows.Count > 0)
            {
                for (int i = 0; i < dgPurchaseRejection.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgPurchaseRejection.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ItemUpdateIndex == "-1")
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
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString())
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
            if (dt2.Columns.Count == 0)
            {
                dt2.Columns.Add("ItemCode");
                dt2.Columns.Add("ItemName");
                dt2.Columns.Add("Unit");
                dt2.Columns.Add("UnitCode");
                dt2.Columns.Add("PO_CODE");
                dt2.Columns.Add("PO_NO");
                dt2.Columns.Add("Rate");
                dt2.Columns.Add("OriginalQty");
                dt2.Columns.Add("ChallanQty");
                dt2.Columns.Add("ReceivedQty");
                dt2.Columns.Add("Amount");
            }
            #endregion

            #region add control value to Dt
            dr = dt2.NewRow();
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
            if (txtInwardNo.Text != "0" && txtInwardNo.Text != "")
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

            if (txtInwardNo.Text != "0" && txtInwardNo.Text != "")
            {
                if (Convert.ToDouble(txtOrigionalQty.Text) < Convert.ToDouble(txtChallanQty.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Challan Qty should be less than or equal to Original Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtChallanQty.Focus();
                }
            }
            dr["ReceivedQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtReceivedQty.Text)), 3));
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));
            if (ddlPO.SelectedIndex > 0)
            {
                dr["PO_Code"] = ddlPO.SelectedValue;
                dr["PO_No"] = ddlPO.SelectedItem;
            }
            dr["Amount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtReceivedQty.Text)), 2));

            double EBasicPer = 0;
            double EEduCessPer = 0;
            double EHEduCessPer = 0;
            double EBasic = 0;
            double EEduCess = 0;
            double EHEduCess = 0;

            DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + ddlItemCode.SelectedValue + "");
            if (dtExcisePer.Rows.Count > 0)
            {
                EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
                EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
                EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
            }

            txtBasicExcPer.Text = EBasicPer.ToString();
            txtducexcper.Text = EEduCessPer.ToString();
            txtSHEExcPer.Text = EHEduCessPer.ToString();
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (dt2.Rows.Count > 0)
                {
                    dt2.Rows.RemoveAt(Index);
                    dt2.Rows.InsertAt(dr, Index);
                }
            }
            else
            {
                dt2.Rows.Add(dr);
            }
            #endregion
            #region Tax
            if (dt2.Rows.Count == 1)
            {
                DataTable dtTax = CommonClasses.Execute("select SPOD_T_CODE from SUPP_PO_DETAILS,SUPP_PO_MASTER where SPOM_CODE='" + ddlPO.SelectedValue + "' and SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE=SPOD_SPOM_CODE");
                if (dtTax.Rows.Count > 0)
                {
                    ddlTaxName.SelectedValue = dtTax.Rows[0]["SPOD_T_CODE"].ToString();
                    ddlTaxName_SelectedIndexChanged(null, null);
                }
            }
            #endregion
            #region Binding data to Grid
            if (dt2.Rows.Count > 0)
            {
                dgPurchaseRejection.Enabled = true;
                dgPurchaseRejection.Visible = true;
                dgPurchaseRejection.DataSource = dt2;
                dgPurchaseRejection.DataBind();
            }
            #endregion
            GetTotal();
            ddlPO.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "btnInsert_Click", ex.Message);
        }
    }
    #endregion

    #region ddlTaxName_SelectedIndexChanged
    protected void ddlTaxName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTaxName.SelectedIndex != 0)
        {
            DataTable dtSTaxPer = CommonClasses.Execute("SELECT cast(ISNULL(ST_SALES_TAX,0) as numeric(20,2)) as ST_SALES_TAX FROM SALES_TAX_MASTER WHERE ST_CODE=" + ddlTaxName.SelectedValue + "");
            if (dtSTaxPer.Rows.Count > 0)
            {
                txtSalesTaxPer.Text = dtSTaxPer.Rows[0]["ST_SALES_TAX"].ToString();
            }
        }
        else
        {
            txtSalesTaxPer.Text = "0.00";
            txtSalesTax.Text = "0.00";
        }
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;

        if (dgPurchaseRejection.Rows.Count > 0)
        {
            for (int i = 0; i < dgPurchaseRejection.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgPurchaseRejection.Rows[i].FindControl("lblAmount"))).Text;
                string Icode = ((Label)(dgPurchaseRejection.Rows[i].FindControl("lblItemCode"))).Text;
                double Amount = Convert.ToDouble(QED_AMT);
                decTotal = decTotal + Amount;
            }
        }

        txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));

        if (dgPurchaseRejection.Enabled)
        {
        }
        else
        {
            txtNetAmount.Text = "0.00";
            txtDiscPer.Text = "0.00";
            txtDiscAmt.Text = "0.00";
            txtPackingAmount.Text = "0.00";
            txtOtherCharges.Text = "0.00";
            txtFreight.Text = "0.00";
            txtInsurance.Text = "0.00";
            txttransport.Text = "0.00";
            txtOctri.Text = "0.00";
            txtTcsAmt.Text = "0.00";
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
        if (txtDiscAmt.Text == "")
        {
            txtDiscAmt.Text = "0.00";
        }
        if (txtPackingAmount.Text == "")
        {
            txtPackingAmount.Text = "0.00";
        }
        if (txtOtherCharges.Text == "")
        {
            txtOtherCharges.Text = "0.00";
        }
        if (txtFreight.Text == "")
        {
            txtFreight.Text = "0.00";
        }
        if (txtInsurance.Text == "")
        {
            txtInsurance.Text = "0.00";
        }
        if (txttransport.Text == "")
        {
            txttransport.Text = "0.00";
        }
        if (txtOctri.Text == "")
        {
            txtOctri.Text = "0.00";
        }
        if (txtTcsAmt.Text == "")
        {
            txtTcsAmt.Text = "0.00";
        }
        if (txtGrandAmt.Text == "")
        {
            txtGrandAmt.Text = "0.00";
        }
        //Discu Amount
        txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2));
        //Packing Amount
        txtPackingAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackingAmount.Text), 2));
        //Accessable Amount
        double AccessableValue = Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtDiscAmt.Text) + Convert.ToDouble(txtPackingAmount.Text);
        txtAccessableAmt.Text = AccessableValue.ToString();
        txtAccessableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccessableAmt.Text));
        //Basic Excise Amt
        double ExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100));
        txtBasicExcise.Text = ExcAmt.ToString();
        txtBasicExcise.Text = string.Format("{0:0.00}", Convert.ToDouble(txtBasicExcise.Text));
        //Educational Excise Amt
        double EduExcAmt = (Convert.ToDouble(txtBasicExcise.Text) * (Convert.ToDouble(txtducexcper.Text) / 100));
        txtEduCess.Text = EduExcAmt.ToString();
        txtEduCess.Text = string.Format("{0:0.00}", Convert.ToDouble(txtEduCess.Text));
        //HigherEducational Excise Amt
        double HEduExcAmt = (Convert.ToDouble(txtBasicExcise.Text) * (Convert.ToDouble(txtSHEExcPer.Text) / 100));
        txtHigherEduCess.Text = HEduExcAmt.ToString();
        txtHigherEduCess.Text = string.Format("{0:0.00}", Convert.ToDouble(txtHigherEduCess.Text));
        //Taxable Amt
        double TaxableAmt = Convert.ToDouble(txtAccessableAmt.Text) + Convert.ToDouble(txtBasicExcise.Text) + Convert.ToDouble(txtEduCess.Text) + Convert.ToDouble(txtHigherEduCess.Text);
        txtTaxableAmt.Text = TaxableAmt.ToString();
        txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text));
        //Tax Amt
        double TaxAmt = (Convert.ToDouble(txtTaxableAmt.Text) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
        txtSalesTax.Text = TaxAmt.ToString();
        txtSalesTax.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSalesTax.Text));
        //GrandAmount
        txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtSalesTax.Text)) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtInsurance.Text) + Convert.ToDouble(txttransport.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTcsAmt.Text))), 2);
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
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Customer Rejection ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    protected void optLstType_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkSelection();
    }
    public void checkSelection()
    {
        if (optLstType.SelectedIndex == 0)
        {
            txtInvDate.Enabled = true;
            txtInwardNo.Enabled = true;
            ddlPO.Enabled = false;
            ddlTaxName.Enabled = false;
            txtBasicExcPer.Enabled = false;
            txtSHEExcPer.Enabled = false;
            txtducexcper.Enabled = false;
        }
        else
        {
            txtInvDate.Enabled = false;
            txtInwardNo.Enabled = false;
            ddlTaxName.Enabled = true;
            txtBasicExcPer.Enabled = true;
            txtSHEExcPer.Enabled = true;
            txtducexcper.Enabled = true;
            ddlPO.Enabled = true;
            txtInvDate.Text = "";
            txtInwardNo.Text = "";
        }
    }

    protected void txtInwardNo_TextChanged(object sender, EventArgs e)
    {
        DataTable dtCheckInv = CommonClasses.Execute("select * from INWARD_MASTER where IWM_P_CODE=" + ddlSupplier.SelectedValue + " and IWM_NO=" + txtInwardNo.Text.Trim() + "");
        if (dtCheckInv.Rows.Count <= 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter valid Inward no";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtInwardNo.Focus();
        }
        else
        {
            LoadIName();
            LoadICode();
            txtInvDate.Text = Convert.ToDateTime(dtCheckInv.Rows[0]["IWM_DATE"]).ToString("dd MMM yyyy");
        }
    }

    protected void txtInvDate_TextChanged(object sender, EventArgs e)
    {
        DataTable dtCheckInvDae = CommonClasses.Execute("select * from INWARD_MASTER where IWM_P_CODE=" + ddlSupplier.SelectedValue + " and IWM_NO=" + txtInwardNo.Text.Trim() + " and IWM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'");
        if (dtCheckInvDae.Rows.Count <= 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please enter valid inward Date";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtInvDate.Focus();
        }
        else
        {
            LoadICode();
            LoadIName();
        }
    }

    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);

        txtChallanQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

        if (Convert.ToDouble(txtReceivedQty.Text) > Convert.ToDouble(txtChallanQty.Text))
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Received qty should be less than or equal to challan qty";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtChallanQty.Focus();
        }
        if (txtInwardNo.Text != "0" && txtInwardNo.Text != "" && txtOrigionalQty.Text != "")
        {
            if (Convert.ToDouble(txtOrigionalQty.Text) < Convert.ToDouble(txtChallanQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Challan qty should be less than or equal to original qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
            }
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
            catch { }
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

    protected void txtBasicExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcPer.Text);
        txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    protected void txtducexcper_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtducexcper.Text);
        txtducexcper.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    protected void txtSalesTaxPer_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtSHEExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSHEExcPer.Text);
        txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }

    protected void dgPurchaseRejection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgPurchaseRejection.Rows[Index];
            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgPurchaseRejection.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);
                dgPurchaseRejection.DataSource = dt2;
                dgPurchaseRejection.DataBind();
                GetTotal();
                clearDetail();
                if (dgPurchaseRejection.Rows.Count == 0)
                    LoadFilter();
            }
            if (e.CommandName == "Select")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged1(null, null);
                ddlPO.SelectedValue = ((Label)(row.FindControl("lblPO_Code"))).Text;
                txtReceivedQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblReceivedQty"))).Text);
                txtRate.Text = string.Format("{0:0.00}", ((Label)(row.FindControl("lblRate"))).Text);
                txtChallanQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblChallanQty"))).Text);
                txtOrigionalQty.Text = string.Format("{0:0.000}", ((Label)(row.FindControl("lblOriginalQty"))).Text);
                txtUnit.Text = ((Label)(row.FindControl("lblUnit"))).Text;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "dgPurchaseRejection_RowCommand", ex.Message);
        }
    }
    protected void dgPurchaseRejection_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "btnOk_Click", Ex.Message);
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
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("PURCHASE_REJECTION_MASTER", "MODIFY", "PR_CODE", mlCode);
            }

            dt2.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejection.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "CancelRecord", ex.Message.ToString());
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
            else if (ddlSupplier.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (dgPurchaseRejection.Enabled == false)
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    protected void txtChallanQty_DataBinding(object sender, EventArgs e)
    {

    }

    protected void txtChallanQty_Load(object sender, EventArgs e)
    {

    }


    #region txtDiscPer_TextChanged
    protected void txtDiscPer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtDiscPer.Text);
            txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtPackingAmount_TextChanged
    protected void txtPackingAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtDiscPer.Text);
            txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtFreight_TextChanged
    protected void txtFreight_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtFreight.Text == "")
            {
                txtFreight.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtFreight.Text);
            txtFreight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtIncurance_TextChanged
    protected void txtIncurance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtInsurance.Text == "")
            {
                txtInsurance.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtInsurance.Text);
            txtInsurance.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtTransportAmt_TextChanged
    protected void txtTransportAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txttransport.Text == "")
            {
                txttransport.Text = "0.00";
            }
            string totalStr = DecimalMasking(txttransport.Text);
            txttransport.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtOctri_TextChanged
    protected void txtOctri_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOctri.Text == "")
            {
                txtOctri.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtOctri.Text);
            txtOctri.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtTCSAmt_TextChanged
    protected void txtTCSAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTcsAmt.Text == "")
            {
                txtTcsAmt.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtTcsAmt.Text);
            txtTcsAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtOtherCharges_TextChanged
    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOtherCharges.Text == "")
            {
                txtOtherCharges.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtOtherCharges.Text);
            txtOtherCharges.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgPurchaseRejection.Rows.Count != 0)
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

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                string strSql = "";
                int ginNo = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(PR_NO),0) as PR_NO FROM PURCHASE_REJECTION_MASTER WHERE PR_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    ginNo = Convert.ToInt32(dt.Rows[0]["PR_NO"]);
                    ginNo = ginNo + 1;
                }
                if (txtInvDate.Text != "" && txtInwardNo.Text != "" && txtInwardNo.Text != "0")
                {
                    strSql = "INSERT INTO PURCHASE_REJECTION_MASTER(PR_COMP_CODE,PR_TYPE,PR_NO,PR_DATE,PR_CHALLAN_NO,PR_CHALLAN_DATE,PR_IWM_NO,PR_IWM_DATE,PR_P_CODE,PR_NET_AMT,PR_DISC_PER,PR_DISC_AMT,PR_PACKING_AMT,PR_EXC_PER,PR_EXC_AMT,PR_EDU_EXC_PER,PR_EDU_EXC_AMT,PR_HEDU_EXC_PER,PR_HEDU_ESC_AMT,PR_ST_CODE,PR_TAX_PER,PR_TAX_AMT,PR_OTHER_AMT,PR_FREIGHT_AMT,PR_INSURANCE_AMT,PR_TRANS_AMT,PR_OCTRI_AMT,PR_TCS_AMT,PR_ACCESS_VALUE,PR_TAXABLE_AMT,PR_GRAND_TOT) VALUES ('" + Session["CompanyCode"] + "','" + optLstType.SelectedIndex + "'," + ginNo + ",'" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "','" + txtChallanNo.Text + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','" + txtInwardNo.Text + "','" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "'," + ddlSupplier.SelectedValue + "," + txtNetAmount.Text + "," + txtDiscPer.Text + "," + txtDiscAmt.Text + "," + txtPackingAmount.Text + "," + txtBasicExcPer.Text + "," + txtBasicExcise.Text + "," + txtducexcper.Text + ",'" + txtEduCess.Text + "'," + txtSHEExcPer.Text + "," + txtHigherEduCess.Text + "," + ddlTaxName.SelectedValue + "," + txtSalesTaxPer.Text + "," + txtSalesTax.Text + "," + txtOtherCharges.Text + "," + txtFreight.Text + "," + txtInsurance.Text + "," + txttransport.Text + "," + txtOctri.Text + "," + txtTcsAmt.Text + "," + txtAccessableAmt.Text + "," + txtTaxableAmt.Text + "," + txtGrandAmt.Text + ")";
                }
                else
                {
                    strSql = "INSERT INTO PURCHASE_REJECTION_MASTER(PR_COMP_CODE,PR_TYPE,PR_NO,PR_DATE,PR_CHALLAN_NO,PR_CHALLAN_DATE,PR_IWM_NO,PR_IWM_DATE,PR_P_CODE,PR_NET_AMT,PR_DISC_PER,PR_DISC_AMT,PR_PACKING_AMT,PR_EXC_PER,PR_EXC_AMT,PR_EDU_EXC_PER,PR_EDU_EXC_AMT,PR_HEDU_EXC_PER,PR_HEDU_ESC_AMT,PR_ST_CODE,PR_TAX_PER,PR_TAX_AMT,PR_OTHER_AMT,PR_FREIGHT_AMT,PR_INSURANCE_AMT,PR_TRANS_AMT,PR_OCTRI_AMT,PR_TCS_AMT,PR_ACCESS_VALUE,PR_TAXABLE_AMT,PR_GRAND_TOT) VALUES ('" + Session["CompanyCode"] + "','" + optLstType.SelectedIndex + "'," + ginNo + ",'" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "','" + txtChallanNo.Text + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','','" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "'," + ddlSupplier.SelectedValue + "," + txtNetAmount.Text + "," + txtDiscPer.Text + "," + txtDiscAmt.Text + "," + txtPackingAmount.Text + "," + txtBasicExcPer.Text + "," + txtBasicExcise.Text + "," + txtducexcper.Text + ",'" + txtEduCess.Text + "'," + txtSHEExcPer.Text + "," + txtHigherEduCess.Text + "," + ddlTaxName.SelectedValue + "," + txtSalesTaxPer.Text + "," + txtSalesTax.Text + "," + txtOtherCharges.Text + "," + txtFreight.Text + "," + txtInsurance.Text + "," + txttransport.Text + "," + txtOctri.Text + "," + txtTcsAmt.Text + "," + txtAccessableAmt.Text + "," + txtTaxableAmt.Text + "," + txtGrandAmt.Text + ")";
                }
                if (CommonClasses.Execute1(strSql))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(PR_CODE) from PURCHASE_REJECTION_MASTER");
                    for (int i = 0; i < dgPurchaseRejection.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO PURCHASE_REJECTION_DETAIL(PRD_PR_CODE,PRD_I_CODE,PRD_UOM,PRD_PO_CODE,PRD_RATE,PRD_ORIGINAL_QTY,PRD_CHALLAN_QTY,PRD_RECEIVED_QTY,PRD_AMOUNT) VALUES(" + Code + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblPO_CODE")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblAmount")).Text + ")");
                        if (txtInvDate.Text != "" && txtInwardNo.Text != "" && txtInwardNo.Text != "0" && ((Label)dgPurchaseRejection.Rows[i].FindControl("lblOriginalQty")).Text != "0.000")
                        {
                            double N1 = Convert.ToDouble(((Label)dgPurchaseRejection.Rows[i].FindControl("lblOriginalQty")).Text);
                            double N2 = Convert.ToDouble(((Label)dgPurchaseRejection.Rows[i].FindControl("lblReceivedQty")).Text);
                            double N3 = N1 - N2;
                            CommonClasses.Execute1("update INWARD_DETAIL SET IWD_RETURN_QTY=IWD_RETURN_QTY+" + N3 + " WHERE IWD_I_CODE IN (SELECT IWD_I_CODE FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE and IWM_NO=" + txtInwardNo.Text + " and IWD_I_CODE=" + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblItemCode")).Text + ")");
                        }
                    }
                    result = true;
                    dt2.Rows.Clear();
                    CommonClasses.WriteLog("Purchase Rejection", "Save", "Purchase Rejection", ginNo.ToString(), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejection.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Could not saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE PURCHASE_REJECTION_MASTER SET PR_COMP_CODE='" + Session["CompanyCode"] + "',PR_TYPE='" + optLstType.SelectedIndex + "',PR_NO=" + txtGINNo.Text + ",PR_DATE='" + Convert.ToDateTime(txtGINDate.Text).ToString("dd/MMM/yyyy") + "',PR_CHALLAN_NO='" + txtChallanNo.Text + "',PR_CHALLAN_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "',PR_IWM_NO=" + txtInwardNo.Text + ",PR_IWM_DATE='" + Convert.ToDateTime(txtInvDate.Text).ToString("dd/MMM/yyyy") + "',PR_P_CODE=" + ddlSupplier.SelectedValue + ",PR_NET_AMT=" + txtNetAmount.Text + ",PR_DISC_PER=" + txtDiscPer.Text + ",PR_DISC_AMT=" + txtDiscAmt.Text + ",PR_PACKING_AMT=" + txtPackingAmount.Text + ",PR_EXC_PER=" + txtBasicExcPer.Text + ",PR_EXC_AMT=" + txtBasicExcise.Text + ",PR_EDU_EXC_PER=" + txtducexcper.Text + ",PR_EDU_EXC_AMT=" + txtEduCess.Text + ",PR_HEDU_EXC_PER=" + txtSHEExcPer.Text + ",PR_HEDU_ESC_AMT=" + txtHigherEduCess.Text + ",PR_ST_CODE=" + ddlTaxName.SelectedValue + ",PR_TAX_PER=" + txtSalesTaxPer.Text + ",PR_TAX_AMT=" + txtSalesTax.Text + ",PR_OTHER_AMT=" + txtOtherCharges.Text + ",PR_FREIGHT_AMT=" + txtFreight.Text + ",PR_INSURANCE_AMT=" + txtInsurance.Text + ",PR_TRANS_AMT=" + txttransport.Text + ",PR_OCTRI_AMT=" + txtOctri.Text + ",PR_TCS_AMT=" + txtTcsAmt.Text + ",PR_ACCESS_VALUE=" + txtAccessableAmt.Text + ",PR_TAXABLE_AMT=" + txtTaxableAmt.Text + ",PR_GRAND_TOT=" + txtGrandAmt.Text + "  where PR_CODE='" + mlCode + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM PURCHASE_REJECTION_DETAIL WHERE PRD_PR_CODE='" + mlCode + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgPurchaseRejection.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO PURCHASE_REJECTION_DETAIL(PRD_PR_CODE,PRD_I_CODE,PRD_UOM,PRD_PO_CODE,PRD_RATE,PRD_ORIGINAL_QTY,PRD_CHALLAN_QTY,PRD_RECEIVED_QTY,PRD_AMOUNT) VALUES(" + mlCode + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblItemCode")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblUnitCode")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblPO_CODE")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblRate")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblOriginalQty")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblChallanQty")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblReceivedQty")).Text + "," + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblAmount")).Text + ")");
                            if (txtInvDate.Text != "" && txtInwardNo.Text != "" && txtInwardNo.Text != "0" && ((Label)dgPurchaseRejection.Rows[i].FindControl("lblOriginalQty")).Text != "")
                            {
                                double N1 = Convert.ToDouble(((Label)dgPurchaseRejection.Rows[i].FindControl("lblOriginalQty")).Text);
                                double N2 = Convert.ToDouble(((Label)dgPurchaseRejection.Rows[i].FindControl("lblReceivedQty")).Text);
                                double N3 = N1 - N2;
                                CommonClasses.Execute1("update INWARD_DETAIL SET IWD_RETURN_QTY=IWD_RETURN_QTY+" + N3 + " WHERE IWD_I_CODE IN (SELECT IWD_I_CODE FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE and IWM_NO=" + txtInwardNo.Text + " and IWD_I_CODE=" + ((Label)dgPurchaseRejection.Rows[i].FindControl("lblItemCode")).Text + ")");
                            }
                        }
                        CommonClasses.RemoveModifyLock("PURCHASE_REJECTION_MASTER", "MODIFY", "PR_CODE", mlCode);
                        CommonClasses.WriteLog("Purchase Rejection", "Update", "Purchase Rejection", txtGINNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        dt2.Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejection.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Invalid Update";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "SaveRec", ex.Message);
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
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion
}
