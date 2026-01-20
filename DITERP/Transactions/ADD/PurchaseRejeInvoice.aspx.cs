using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_PurchaseRejeInvoice : System.Web.UI.Page
{
    #region Variable
    DataTable dt = new DataTable();
    DataTable dtInvoiceDetail = new DataTable();   
    static int mlCode = 0;
    DataRow dr; 
    static DataTable dt2 = new DataTable();
    public static int Index = 0;    
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
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
                        dt2.Clear();
                        chkIsSuppliement.Enabled = false;
                        LoadSupplier();
                        LoadPurchaseNo();
                        LoadICode();
                        LoadIName();
                        Loadtax();
                      
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                            ddlddlSupplier_SelectedIndexChanged(null, null);
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadFilter();
                            dgInvoiceAddDetail.Enabled = false;
                            txtDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtIssuedate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtremovaldate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtLRDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            txtDate.Attributes.Add("readonly","readonly");
                            txtIssuedate.Attributes.Add("readonly", "readonly");
                            txtremovaldate.Attributes.Add("readonly", "readonly");
                            txtLRDate.Attributes.Add("readonly", "readonly");
                        }
                        txtInvoiceNo.Focus();
                        dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
        }
    }

    private void LoadPurchaseNo()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select PR_CODE,PR_NO from PURCHASE_REJECTION_MASTER where PR_P_CODE=" + ddlSupplier.SelectedValue + " and PURCHASE_REJECTION_MASTER.ES_DELETE =0");
            if (dt.Rows.Count > 0)
            {
                ddlPurchaseNo.DataSource = dt;
                ddlPurchaseNo.DataTextField = "PR_NO";
                ddlPurchaseNo.DataValueField = "PR_CODE";
                ddlPurchaseNo.DataBind();
                ddlPurchaseNo.Items.Insert(0, new ListItem("Select Purchase No", "0"));
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion Page_Load

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgInvoiceAddDetail.Enabled==false)
        {

            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            return;
        }
        if (ddlTaxName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Tax Name", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            ddlTaxName.Focus();
            return;
        }
        if (dgInvoiceAddDetail.Rows.Count > 0)
        {
            SaveRec();
        }
        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        }
    }
    #endregion btnSubmit_Click

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
            CommonClasses.SendError("Tax Invoice", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("Tax Invocie", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", mlCode);
            }

            dt2.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejeInvoice.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion 

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlSupplier.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDate.Text == "")
            {
                flag = false;
            }
            else if (ddlTaxName.SelectedIndex == 0)
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
            CommonClasses.SendError("Tax Invoice", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region Loadtax
    private void Loadtax()
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
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT Distinct(PURCHASE_REJECTION_MASTER.PR_P_CODE),PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM PARTY_MASTER INNER JOIN PURCHASE_REJECTION_MASTER ON PARTY_MASTER.P_CODE = PURCHASE_REJECTION_MASTER.PR_P_CODE where PARTY_MASTER.P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and PURCHASE_REJECTION_MASTER.ES_DELETE=0");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT DISTINCT(PURCHASE_REJECTION_DETAIL.PRD_I_CODE),ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO FROM PURCHASE_REJECTION_MASTER INNER JOIN PURCHASE_REJECTION_DETAIL ON PURCHASE_REJECTION_MASTER.PR_CODE = PURCHASE_REJECTION_DETAIL.PRD_PR_CODE INNER JOIN PARTY_MASTER ON PURCHASE_REJECTION_MASTER.PR_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON PURCHASE_REJECTION_DETAIL.PRD_I_CODE = ITEM_MASTER.I_CODE where PURCHASE_REJECTION_MASTER.PR_P_CODE="+ddlSupplier.SelectedValue +"");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection Invoice", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT PURCHASE_REJECTION_MASTER.PR_CODE, PURCHASE_REJECTION_DETAIL.PRD_I_CODE,ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME FROM PURCHASE_REJECTION_MASTER INNER JOIN PURCHASE_REJECTION_DETAIL ON PURCHASE_REJECTION_MASTER.PR_CODE = PURCHASE_REJECTION_DETAIL.PRD_PR_CODE INNER JOIN PARTY_MASTER ON PURCHASE_REJECTION_MASTER.PR_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON PURCHASE_REJECTION_DETAIL.PRD_I_CODE = ITEM_MASTER.I_CODE where PURCHASE_REJECTION_MASTER.PR_P_CODE='" + ddlSupplier.SelectedValue + "'");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection Invoice ", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvoiceAddDetail.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("STOCK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PEND_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ACT_WGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SUBHEADING", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_NO_PACK", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvoiceAddDetail.DataSource = dtFilter;
                dgInvoiceAddDetail.DataBind();
                dgInvoiceAddDetail.Enabled = false;
            }
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
            CommonClasses.SendError("Tax Invoice", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;

                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='"+ddlItemCode.SelectedValue +"'");

                DataTable dtPO = new DataTable();

                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT Distinct(CUSTPO_MASTER.CPOM_CODE),CUSTPO_MASTER.CPOM_PONO,PURCHASE_REJECTION_DETAIL.PRD_PO_CODE FROM CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN PURCHASE_REJECTION_DETAIL ON CUSTPO_MASTER.CPOM_CODE = PURCHASE_REJECTION_DETAIL.PRD_PO_CODE where CUSTPO_MASTER.ES_DELETE=0 and PURCHASE_REJECTION_DETAIL.PRD_PR_CODE="+mlCode +"");
                }
                else
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT CUSTPO_MASTER.CPOM_CODE, CUSTPO_MASTER.CPOM_PONO, PURCHASE_REJECTION_DETAIL.PRD_PO_CODE, ITEM_MASTER.I_CODE FROM CUSTPO_MASTER INNER JOIN CUSTPO_DETAIL ON CUSTPO_MASTER.CPOM_CODE = CUSTPO_DETAIL.CPOD_CPOM_CODE INNER JOIN PURCHASE_REJECTION_DETAIL ON CUSTPO_MASTER.CPOM_CODE = PURCHASE_REJECTION_DETAIL.PRD_PO_CODE INNER JOIN ITEM_MASTER ON PURCHASE_REJECTION_DETAIL.PRD_I_CODE = ITEM_MASTER.I_CODE WHERE(CUSTPO_MASTER.ES_DELETE = 0) and PURCHASE_REJECTION_DETAIL.PRD_I_CODE="+ddlItemCode.SelectedValue+"");
                }

                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                }
                else
                {
                    txtUOM.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection Invoice", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion  

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadPurchaseNo();
            LoadICode();
            LoadIName();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                DataTable dtPO = new DataTable();
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE) and IND_INM_CODE=" + mlCode + " and CPOM_TYPE=-2147483648");
                }
                else
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0 and CPOM_TYPE=-2147483648");
                }
                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                }
                else
                {
                    txtUOM.Text = "";
                }
                DataTable DtRate = new DataTable();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlItemName_SelectedIndexChanged", Ex.Message);
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
                GetTotal();
            }
        }
        else
        {
            txtSalesTaxPer.Text = "0.00";
            txtSalesTaxAmount.Text = "0.00";
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtDate.Attributes.Add("readonly", "readonly");
            txtIssuedate.Attributes.Add("readonly", "readonly");
            txtremovaldate.Attributes.Add("readonly", "readonly");
            txtLRDate.Attributes.Add("readonly", "readonly");
            dtInvoiceDetail.Clear();
            DataTable dtMast = CommonClasses.Execute("Select INM_G_AMT,INM_CODE,INM_NO,INM_DATE,INM_P_CODE,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,INM_CM_CODE,MODIFY,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_TAX_TCS,0) as numeric(20,2)) as INM_TAX_TCS,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0)  as numeric(20,2)) as INM_OCTRI_AMT,isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME from INVOICE_MASTER where ES_DELETE=0 and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_CODE=" + mlCode + "");
                               
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dtMast.Rows[0]["INM_CODE"]); ;

                txtInvoiceNo.Text = dtMast.Rows[0]["INM_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = dtMast.Rows[0]["INM_P_CODE"].ToString();
                ddlddlSupplier_SelectedIndexChanged(null, null);
                txtNetAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_NET_AMT"].ToString()));
                txtDiscPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC"].ToString()));
                txtDiscAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC_AMT"].ToString()));
                txtPackAmt.Text=string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_PACK_AMT"].ToString()));
                txtAccessableAmt.Text=string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_ACCESSIBLE_AMT"].ToString()));
                txtBasicExcPer.Text = dtMast.Rows[0]["INM_BEXCISE"].ToString();
                txtBasicExcAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_BE_AMT"].ToString()));
                txtducexcper.Text = dtMast.Rows[0]["INM_EDUC_CESS"].ToString();
                txtEdueceAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_EDUC_AMT"].ToString()));
                txtSHEExcPer.Text = dtMast.Rows[0]["INM_H_EDUC_CESS"].ToString();
                txtSHEExcAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_H_EDUC_AMT"].ToString()));
                txtTaxableAmt.Text=string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TAXABLE_AMT"].ToString()));
                txtSalesTaxPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_S_TAX"].ToString()));
                txtSalesTaxAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_S_TAX_AMT"].ToString()));
                txtOtherCharges.Text = dtMast.Rows[0]["INM_OTHER_AMT"].ToString();
                txtFreight.Text = dtMast.Rows[0]["INM_FREIGHT"].ToString();
                txtIncurance.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_INSURANCE"].ToString()));
                txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TRANS_AMT"].ToString()));
                txtOctri.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_OCTRI_AMT"].ToString()));
                txtTCSAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TAX_TCS_AMT"].ToString()));
                txtRoundingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_ROUNDING_AMT"].ToString()));
                txtGrandAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_G_AMT"].ToString()));
                ddlTaxName.SelectedValue = dtMast.Rows[0]["INM_T_CODE"].ToString();
                txtstroreloc.Text = dtMast.Rows[0]["INM_STO_LOC"].ToString();
                txtVechicleNo.Text = dtMast.Rows[0]["INM_VEH_NO"].ToString();
                txtTransport.Text = dtMast.Rows[0]["INM_TRANSPORT"].ToString();
                txtIssuedate.Text =Convert.ToDateTime(dtMast.Rows[0]["INM_ISSUE_DATE"]).ToString("dd MMM yyyy");
                txtremovaldate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_REMOVAL_DATE"]).ToString("dd MMM yyyy");
                chkIsSuppliement.Checked =Convert.ToBoolean(dtMast.Rows[0]["INM_IS_SUPPLIMENT"]);
                txtRemark.Text = dtMast.Rows[0]["INM_REMARK"].ToString();
                txtLRNo.Text = dtMast.Rows[0]["INM_LR_NO"].ToString();
                if (dtMast.Rows[0]["INM_LR_DATE"] != DBNull.Value)
                {
                    txtLRDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_LR_DATE"]).ToString("dd MMM yyyy");
                }
                txtIssuetime.Text = dtMast.Rows[0]["INM_ISSU_TIME"].ToString();
                txtRemoveltime.Text = dtMast.Rows[0]["INM_REMOVEL_TIME"].ToString();

                btnSubmit.Visible = true;
                btnInsert.Visible = true;

                dtInvoiceDetail = CommonClasses.Execute("SELECT INVOICE_MASTER.INM_CODE,INVOICE_DETAIL.IND_I_CODE, INVOICE_DETAIL.IND_CPOM_CODE as PO_CODE, CUSTPO_MASTER.CPOM_PONO As PO_NO, ITEM_MASTER.I_NAME as IND_I_NAME, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME As UOM,CAST(ITEM_MASTER.I_UWEIGHT AS numeric(10, 2)) AS ACT_WGHT,CAST(INVOICE_DETAIL.IND_INQTY AS numeric(10, 3)) AS INV_QTY, CAST(INVOICE_DETAIL.IND_RATE AS numeric(20, 2)) AS RATE,CAST(INVOICE_DETAIL.IND_INQTY * INVOICE_DETAIL.IND_RATE AS numeric(20, 2)) AS AMT, INVOICE_DETAIL.IND_SUBHEADING, INVOICE_DETAIL.IND_BACHNO, INVOICE_DETAIL.IND_NO_PACK, INVOICE_DETAIL.IND_PAK_QTY, INVOICE_DETAIL.IND_EX_AMT, INVOICE_DETAIL.IND_E_CESS_AMT,INVOICE_DETAIL.IND_SH_CESS_AMT, INVOICE_DETAIL.IND_PACK_DESC FROM INVOICE_DETAIL INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN CUSTPO_MASTER ON INVOICE_DETAIL.IND_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE where INVOICE_MASTER.INM_CODE=-2147483639");
               
                LoadICode();
                LoadIName();

                if (dtInvoiceDetail.Rows.Count != 0)
                {
                    dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
                    dgInvoiceAddDetail.DataBind();
                    dt2 = dtInvoiceDetail;
                    dgInvoiceAddDetail.Enabled = true;
                    GetTotal();
                }
            }
            if (str == "VIEW")
            {
                txtInvoiceNo.Enabled = false;
                txtDate.Enabled = false;
                ddlSupplier.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtAmount.Enabled = false;
                txtVQty.Enabled = false;
                txtRate.Enabled = false;
                txtVechicleNo.Enabled = false;
                txtTransport.Enabled = false;
                txtIssuedate.Enabled = false;
                txtremovaldate.Enabled = false;
                txtRemark.Enabled = false;
                txtLRNo.Enabled = false;
                txtLRDate.Enabled = false;
                txtDiscPer.Enabled = false;
                txtDiscAmt.Enabled = false;
                txtPackAmt.Enabled = false;
                ddlTaxName.Enabled = false;
                txtAccessableAmt.Enabled = false;
                txtTaxableAmt.Enabled = false;
                txtBasicExcPer.Enabled = false;
                txtBasicExcAmt.Enabled = false;
                txtducexcper.Enabled = false;
                txtEdueceAmt.Enabled = false;
                txtSHEExcAmt.Enabled = false;
                txtSHEExcPer.Enabled = false;
                dgInvoiceAddDetail.Enabled = false;
                txtFreight.Enabled = false;
                txtOtherCharges.Enabled = false;
                txtIncurance.Enabled = false;
                txtTransportAmt.Enabled = false;
                txtOctri.Enabled = false;
                txtTCSAmt.Enabled = false;
                txtRoundingAmt.Enabled = false;                
                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                dgInvoiceAddDetail.Enabled = false;
                chkIsSuppliement.Enabled = false;
                txtPackDesc.Enabled = false;
                txtIssuetime.Enabled = false;
                txtRemoveltime.Enabled = false;
            }
            if (str == "MOD")
            {
                ddlSupplier.Enabled = false;
                chkIsSuppliement.Enabled = false;
                CommonClasses.SetModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(mlCode));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ViewRec", Ex.Message);
        }
    }
    #endregion

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

    #region txtVQty_OnTextChanged
    protected void txtVQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVQty.Text == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);

                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

                //txtVQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtVQty.Text));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);

                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

                //txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }
            if (txtPendingQty.Text!="")
            {
            if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
            {
                //ShowMessage("#Avisos", "Invoice Qty Is Not Greater Than Pending Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Correct Invoice Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtVQty.Focus();
                return;
            }
            }


          
            txtAmount.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())),2);
            if (txtNoPackaeg.Text == "")
            {
                txtNoPackaeg.Text = "0";
            }
            if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
            {
                double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
                txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Incvoice", "txtVQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVQty.Text == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            }
            txtAmount.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())),2);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion  

    #region txtNoPackaeg_TextChanged
    protected void txtNoPackaeg_TextChanged(object sender, EventArgs e)
    {
        if (txtNoPackaeg.Text == "")
        {
            txtNoPackaeg.Text = "0";
        }
        if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
        {
            if (txtVQty.Text!="")
            {
            double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
            txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
            }
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");
            if (ddlSupplier.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
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
            if (txtVQty.Text == "" || txtVQty.Text == "0.000")
            {
                //ShowMessage("#Avisos", "The Field 'Invoice Qty' Is Required", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Invoice Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtBatchNo.Focus();
                return;
            }
            if (txtVQty.Text == "" || txtVQty.Text == "0.000")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Invoice Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPendingQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Pending Qty Is Not Less Than Invoice Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtVQty.Focus();
            }
            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_I_CODE"))).Text;
                    string IND_BACHNO = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblBatch"))).Text;
                    
                    if (ItemUpdateIndex == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && IND_BACHNO==txtBatchNo.Text.Trim())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString() && IND_BACHNO == txtBatchNo.Text.Trim())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                }
            }

            #region Add Data table coloums
            if (dt2.Columns.Count == 0)
            {
                dt2.Columns.Add("IND_I_CODE");
                dt2.Columns.Add("IND_I_CODENO");
                dt2.Columns.Add("IND_I_NAME");
                dt2.Columns.Add("UOM");
                dt2.Columns.Add("PO_CODE");
                dt2.Columns.Add("PO_NO");
                dt2.Columns.Add("STOCK_QTY");
                dt2.Columns.Add("PEND_QTY");
                dt2.Columns.Add("INV_QTY");
                dt2.Columns.Add("ACT_WGHT");
                dt2.Columns.Add("RATE");
                dt2.Columns.Add("AMT");
                dt2.Columns.Add("IND_SUBHEADING");
                dt2.Columns.Add("IND_BACHNO");
                dt2.Columns.Add("IND_NO_PACK");
                dt2.Columns.Add("IND_PAK_QTY");
                dt2.Columns.Add("IND_EX_AMT");
                dt2.Columns.Add("IND_E_CESS_AMT");
                dt2.Columns.Add("IND_SH_CESS_AMT");
                dt2.Columns.Add("IND_PACK_DESC");
            }
            #endregion

            #region add control value to Dt
            dr = dt2.NewRow();
            dr["IND_I_CODE"] = ddlItemName.SelectedValue;
            dr["IND_I_CODENO"] = ddlItemCode.SelectedItem;
            dr["IND_I_NAME"] = ddlItemName.SelectedItem;
            dr["UOM"] = txtUOM.Text;
            dr["PO_CODE"] = ddlPurchaseNo.SelectedValue;
            dr["PO_NO"] = ddlPurchaseNo.SelectedItem ;
            dr["STOCK_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtStockQty.Text == "" ? "0.00" : txtStockQty.Text)));
            dr["PEND_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtPendingQty.Text)-Convert.ToDouble(txtVQty.Text)));
            dr["INV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtVQty.Text == "" ? "0.00" : txtVQty.Text)));
            dr["ACT_WGHT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtActWght.Text == "" ? "0.00" : txtActWght.Text)));
            dr["RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text == "" ? "0.00" : txtRate.Text)));
            dr["AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)));
            dr["IND_SUBHEADING"] = txtSubHeading.Text;
            dr["IND_BACHNO"] = txtBatchNo.Text;
            dr["IND_NO_PACK"] = txtNoPackaeg.Text;
            dr["IND_PAK_QTY"] = txtQtyPerPack.Text;
            dr["IND_PACK_DESC"] = txtPackDesc.Text;

            #region ExciseCalculation
            double EBasicPer = 0;
            double EEduCessPer = 0;
            double EHEduCessPer = 0;
            double EBasic = 0;
            double EEduCess = 0;
            double EHEduCess = 0;
            DataTable dtExcisePer = CommonClasses.Execute("select PR_EXC_PER,PR_EDU_EXC_PER ,PR_HEDU_EXC_PER  from PURCHASE_REJECTION_MASTER where PR_NO='"+ddlPurchaseNo.SelectedItem.Text +"'");
            if (dtExcisePer.Rows.Count > 0)
            {
                EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["PR_EXC_PER"].ToString());
                EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["PR_EDU_EXC_PER"].ToString());
                EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["PR_HEDU_EXC_PER"].ToString());
            }
            txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EBasicPer));
            txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(EEduCessPer));
            txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EHEduCessPer));

            EBasic = Math.Round((((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)) * EBasicPer) / 100), 2);
            EEduCess = Math.Round(EBasic * EEduCessPer / 100, 2);
            EHEduCess = Math.Round(EBasic * EHEduCessPer / 100, 2);
            dr["IND_EX_AMT"] = EBasic;
            dr["IND_E_CESS_AMT"] = EEduCess;
            dr["IND_SH_CESS_AMT"] = EHEduCess;
            #endregion
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (dt2.Rows.Count > 0)
                {
                    dt2.Rows.RemoveAt(Index);
                    dt2.Rows.InsertAt(dr, Index);
                    dgInvoiceAddDetail.Enabled = true;
                }
            }
            else
            {
                dt2.Rows.Add(dr);
                dgInvoiceAddDetail.Enabled = true;
            }
            #endregion

            #region Binding data to Grid
            dgInvoiceAddDetail.Visible = true;
            dgInvoiceAddDetail.Enabled = true;
            dgInvoiceAddDetail.DataSource = dt2;
            dgInvoiceAddDetail.DataBind();
            #endregion

            #region Tax
            if (dt2.Rows.Count == 1)
            {
                DataTable dtTax = CommonClasses.Execute("SELECT DISTINCT(PURCHASE_REJECTION_MASTER.PR_ST_CODE),PURCHASE_REJECTION_MASTER.PR_TAX_PER, SALES_TAX_MASTER.ST_TAX_NAME FROM PURCHASE_REJECTION_MASTER INNER JOIN SALES_TAX_MASTER ON PURCHASE_REJECTION_MASTER.PR_ST_CODE = SALES_TAX_MASTER.ST_CODE where PURCHASE_REJECTION_MASTER.PR_NO="+ddlPurchaseNo.SelectedItem.Text  +"");
              
                if (dtTax.Rows.Count > 0)
                {
                    ddlTaxName.SelectedValue = dtTax.Rows[0]["PR_ST_CODE"].ToString();
                    txtSalesTaxPer.Text = dtTax.Rows[0]["PR_TAX_PER"].ToString();
                }
            }
            #endregion

            #region Clear Controles
            ClearControles();
            #endregion

            GetTotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ClearControles
    void ClearControles()
    {
        ddlItemCode.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        ddlPurchaseNo.SelectedIndex = 0;
        txtUOM.Text = "";
        txtStockQty.Text = "0.000";
        txtPendingQty.Text = "0.000";
        txtVQty.Text = "0.000";
        txtActWght.Text = "0.00";
        txtRate.Text = "0.00";
        txtAmount.Text = "0.00";
        txtSubHeading.Text = "";
        txtBatchNo.Text = "";
        txtNoPackaeg.Text = "0";
        txtQtyPerPack.Text = "0.00";
        txtPackDesc.Text = "";
        str = "";
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double ExcBasic = 0;
        double Excedu = 0;
        double ExcSH = 0;
        try
        {
            
            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string QED_AMT = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblAMT"))).Text;
                    string Basic = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEBasic"))).Text;
                    string EduCess = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEEcess"))).Text;
                    string SH = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEHEcess"))).Text;

                    double Amount = Convert.ToDouble(QED_AMT);
                    decTotal = decTotal + Amount;
                    ExcBasic = ExcBasic + Convert.ToDouble(Basic);
                    Excedu = Excedu + Convert.ToDouble(EduCess);
                    ExcSH = ExcSH + Convert.ToDouble(SH);
                }
            }
            else
            {
                ddlTaxName.SelectedIndex = 0;
            }
            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));
            
                if (dgInvoiceAddDetail.Enabled)
                {
                }
                else
                {
                    txtNetAmount.Text = "0.00";
                    txtDiscAmt.Text = "0.00";
                    txtBasicExcAmt.Text = "0.00";
                    txtEdueceAmt.Text = "0.00";
                    txtSHEExcAmt.Text = "0.00";
                }
           
            if (txtNetAmount.Text == "")
            {
                txtNetAmount.Text = "0.00";
            }
            if (txtDiscAmt.Text == "")
            {
                txtDiscAmt.Text = "0.00";
            }
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0.00";
            }
            if (txtBasicExcAmt.Text == "")
            {
                txtBasicExcAmt.Text = "0.00";
            }
            if (txtBasicExcPer.Text == "")
            {
                txtBasicExcPer.Text = "0.00";
            }
            if (txtEdueceAmt.Text == "")
            {
                txtEdueceAmt.Text = "0.00";
            }
            if (txtSHEExcPer.Text == "")
            {
                txtSHEExcPer.Text = "0.00";
            }
            if (ddlTaxName.SelectedIndex == 0)
            {
                txtSalesTaxPer.Text = "0.00";
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtSalesTaxPer.Text == "")
            {
                txtSalesTaxPer.Text = "0.00";
            }
            if (txtSalesTaxAmount.Text == "")
            {
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtPackAmt.Text == "")
            {
                txtPackAmt.Text = "0.00";
            }

            if (txtGrandAmt.Text == "")
            {
                txtGrandAmt.Text = "0.00";
            }
            //Discu Amount
            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), MidpointRounding.AwayFromZero));
            //Packing Amount
            txtPackAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackAmt.Text), MidpointRounding.AwayFromZero));
            //Accessable Amount
            double AccessableValue = Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtDiscAmt.Text) + Convert.ToDouble(txtPackAmt.Text);
            txtAccessableAmt.Text = AccessableValue.ToString();
            txtAccessableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtAccessableAmt.Text), MidpointRounding.AwayFromZero));
            //Basic Excise Amt
            double ExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100));
            txtBasicExcAmt.Text = ExcAmt.ToString();
            txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicExcAmt.Text), MidpointRounding.AwayFromZero));
            //Educational Excise Amt
            double EduExcAmt = (Convert.ToDouble(txtBasicExcAmt.Text) * (Convert.ToDouble(txtducexcper.Text) / 100));
            txtEdueceAmt.Text = EduExcAmt.ToString();
            txtEdueceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtEdueceAmt.Text), MidpointRounding.AwayFromZero));
            //HigherEducational Excise Amt
            double HEduExcAmt = (Convert.ToDouble(txtBasicExcAmt.Text) * (Convert.ToDouble(txtSHEExcPer.Text) / 100));
            txtSHEExcAmt.Text = HEduExcAmt.ToString();
            txtSHEExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSHEExcAmt.Text), MidpointRounding.AwayFromZero));
            //Taxable Amt
            double TaxableAmt = Convert.ToDouble(txtAccessableAmt.Text) + Convert.ToDouble(txtBasicExcAmt.Text) + Convert.ToDouble(txtEdueceAmt.Text) + Convert.ToDouble(txtSHEExcAmt.Text);
            txtTaxableAmt.Text = TaxableAmt.ToString();
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtTaxableAmt.Text), MidpointRounding.AwayFromZero));
            //Tax Amt
            double TaxAmt = (Convert.ToDouble(txtTaxableAmt.Text) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
            txtSalesTaxAmount.Text = TaxAmt.ToString();
            txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSalesTaxAmount.Text), MidpointRounding.AwayFromZero));
            //GrandAmount
            txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text)) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtIncurance.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTCSAmt.Text) + Convert.ToDouble(txtRoundingAmt.Text))), MidpointRounding.AwayFromZero);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Incvoice", "GetTotal", Ex.Message);
        }
     }
    #endregion
    
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

            txtDiscAmt.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100),2);
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
            if (txtIncurance.Text == "")
            {
                txtIncurance.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtIncurance.Text);
            txtIncurance.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
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
            if (txtTransportAmt.Text == "")
            {
                txtTransportAmt.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtTransportAmt.Text);
            txtTransportAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
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

    #region txtTaxableAmt_TextChanged
    protected void txtTaxableAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTaxableAmt.Text == "")
            {
                txtTaxableAmt.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtTaxableAmt.Text);
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
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
            if (txtTCSAmt.Text == "")
            {
                txtTCSAmt.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtTCSAmt.Text);
            txtTCSAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtRoundingAmt_TextChanged
    protected void txtRoundingAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRoundingAmt.Text == "")
            {
                txtRoundingAmt.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtRoundingAmt.Text);
            txtRoundingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtDiscAmt_TextChanged
    protected void txtDiscAmt_TextChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region txtBasicExcPer_TextChanged
    protected void txtBasicExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcPer.Text);
        txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion

    #region txtducexcper_TextChanged
    protected void txtducexcper_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtducexcper.Text);
        txtducexcper.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion

    #region txtSHEExcPer_TextChanged
    protected void txtSHEExcPer_TextChanged(object sender, EventArgs e)
    {

        string totalStr = DecimalMasking(txtSHEExcPer.Text);
        txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion

    #region txtSalesTaxPer_TextChanged
    protected void txtSalesTaxPer_TextChanged(object sender, EventArgs e)
    {
        GetTotal();
    }
    #endregion
    
    #region txtPackAmt_TextChanged
    protected void txtPackAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtPackAmt.Text);
        txtPackAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion

    #region txtOtherCharges_TextChanged
    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtOtherCharges.Text);
        txtOtherCharges.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion
    
    #region dgInvoiceAddDetail_RowCommand
    protected void dgInvoiceAddDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {           
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInvoiceAddDetail.Rows[Index];
          
            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgInvoiceAddDetail.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);
                dgInvoiceAddDetail.DataSource = dt2;
                dgInvoiceAddDetail.DataBind();

                if (dgInvoiceAddDetail.Rows.Count == 0)
                {
                    dgInvoiceAddDetail.Enabled = false;
                    GetTotal();
                    LoadFilter();
                }
                else
                {
                    GetTotal();
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtUOM.Text = ((Label)(row.FindControl("lblUOM"))).Text;
                ddlPurchaseNo.SelectedValue = ((Label)(row.FindControl("lblPO_CODE"))).Text;
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text),3));
                txtActWght.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblACT_WGHT"))).Text),2));
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblRATE"))).Text),2));
                txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAMT"))).Text),2));
                txtSubHeading.Text = ((Label)(row.FindControl("lblSubHeading"))).Text;
                txtBatchNo.Text = ((Label)(row.FindControl("lblBatch"))).Text;
                txtNoPackaeg.Text = (Convert.ToInt32(((Label)(row.FindControl("lblNoPack"))).Text)).ToString();
                txtQtyPerPack.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblPakQty"))).Text),2));
                txtPackDesc.Text = ((Label)(row.FindControl("lblIND_PACK_DESC"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "dgInvoiceAddDetail_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceAddDetail_Deleting
    protected void dgInvoiceAddDetail_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            int IsSuppliement = 0;
            if (chkIsSuppliement.Checked)
            {
                IsSuppliement = 1;
            }
            else
            {
                IsSuppliement = 0;
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Inv_No = 0;
                DataTable dt = new DataTable();               
                dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    Inv_No = Convert.ToInt32(dt.Rows[0]["INM_NO"]);
                    Inv_No = Inv_No + 1;
                }
                if (CommonClasses.Execute1("INSERT INTO INVOICE_MASTER (INM_CM_CODE,INM_NO,INM_DATE,INM_INVOICE_TYPE,INM_P_CODE,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_TAX_TCS,INM_TAX_TCS_AMT,INM_PACK_AMT,INM_G_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,INM_ACCESSIBLE_AMT,INM_TAXABLE_AMT,INM_ROUNDING_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','3','" + ddlSupplier.SelectedValue + "','" + txtNetAmount.Text + "','" + txtDiscPer.Text + "','" + txtDiscAmt.Text + "','" + txtBasicExcPer.Text + "','" + txtBasicExcAmt.Text + "','" + txtducexcper.Text + "','" + txtEdueceAmt.Text + "','" + txtSHEExcPer.Text + "','" + txtSHEExcAmt.Text + "','" + txtSalesTaxPer.Text + "','" + txtSalesTaxAmount.Text + "','0','" + txtTCSAmt.Text + "','" + txtPackAmt.Text + "','" + txtGrandAmt.Text + "','" + ddlTaxName.SelectedValue + "','" + txtstroreloc.Text + "','" + txtVechicleNo.Text + "','" + txtTransport.Text + "','" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "','" + txtRemark.Text + "','" + txtLRNo.Text + "','" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "','" + txtAccessableAmt.Text + "','" + txtTaxableAmt.Text + "','" + txtRoundingAmt.Text + "','" + txtOtherCharges.Text + "','" + txtFreight.Text + "','" + txtIncurance.Text + "','" + txtTransportAmt.Text + "','" + txtOctri.Text + "','" + IsSuppliement + "','"+txtIssuetime.Text+"','"+txtRemoveltime.Text+"')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(INM_CODE) from INVOICE_MASTER");
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC) values ('" + Code + "','" + dt2.Rows[i]["IND_I_CODE"] + "','" + dt2.Rows[i]["PO_CODE"] + "','" + dt2.Rows[i]["INV_QTY"] + "','" + dt2.Rows[i]["RATE"] + "','" + dt2.Rows[i]["AMT"] + "','" + dt2.Rows[i]["IND_SUBHEADING"] + "','" + dt2.Rows[i]["IND_BACHNO"] + "','" + dt2.Rows[i]["IND_NO_PACK"] + "','" + dt2.Rows[i]["IND_PAK_QTY"] + "','" + dt2.Rows[i]["IND_EX_AMT"] + "','" + dt2.Rows[i]["IND_E_CESS_AMT"] + "','" + dt2.Rows[i]["IND_SH_CESS_AMT"] + "','" + dt2.Rows[i]["INV_QTY"] + "','" + dt2.Rows[i]["IND_PACK_DESC"] + "')");
                       
                        if (!chkIsSuppliement.Checked)
                        {
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + dt2.Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + dt2.Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + dt2.Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + dt2.Rows[i]["IND_I_CODE"] + "','" + Code + "','" + Inv_No + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + dt2.Rows[i]["INV_QTY"] + "')");

                            }
                            //Removing Stock
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dt2.Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + dt2.Rows[i]["IND_I_CODE"] + "'");
                            }
                       }                 
                    }
                    CommonClasses.WriteLog("Tax Invoice", "Save", "Tax Invoice", Convert.ToString(Inv_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    dt2.Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewPurchaseRejeInvoice.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtInvoiceNo.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE INVOICE_MASTER SET INM_CM_CODE=" + Session["CompanyCode"] + ",INM_NO='" + txtInvoiceNo.Text + "', INM_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',INM_P_CODE='" + ddlSupplier.SelectedValue + "',INM_NET_AMT='" + txtNetAmount.Text + "',INM_DISC='" + txtDiscPer.Text + "',INM_DISC_AMT='" + txtDiscAmt.Text + "',INM_BEXCISE='" + txtBasicExcPer.Text + "',INM_BE_AMT='" + txtBasicExcAmt.Text + "',INM_EDUC_CESS='" + txtducexcper.Text + "',INM_EDUC_AMT='" + txtEdueceAmt.Text + "',INM_H_EDUC_CESS='" + txtSHEExcPer.Text + "',INM_H_EDUC_AMT = '" + txtSHEExcAmt.Text + "',INM_S_TAX='" + txtSalesTaxPer.Text + "',INM_S_TAX_AMT='" + txtSalesTaxAmount.Text + "',INM_PACK_AMT='" + txtPackAmt.Text + "',INM_G_AMT='" + txtGrandAmt.Text + "',INM_T_CODE='" + ddlTaxName.SelectedValue + "',INM_STO_LOC='" + txtstroreloc.Text + "',INM_VEH_NO='" + txtVechicleNo.Text + "',INM_TRANSPORT='" + txtTransport.Text + "',INM_ISSUE_DATE='" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "',INM_REMOVAL_DATE='" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "',INM_REMARK='" + txtRemark.Text + "',INM_LR_NO='" + txtLRNo.Text + "',INM_LR_DATE='" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "',INM_ACCESSIBLE_AMT='" + txtAccessableAmt.Text + "',INM_TAXABLE_AMT='" + txtTaxableAmt.Text + "',INM_ROUNDING_AMT='" + txtRoundingAmt.Text + "',INM_OTHER_AMT='" + txtOtherCharges.Text + "',INM_FREIGHT='" + txtFreight.Text + "',INM_INSURANCE='" + txtIncurance.Text + "',INM_TRANS_AMT='" + txtTransportAmt.Text + "',INM_OCTRI_AMT='" + txtOctri.Text + "',INM_IS_SUPPLIMENT='" + IsSuppliement + "',INM_ISSU_TIME='" + txtIssuetime.Text + "',INM_REMOVEL_TIME='"+txtRemoveltime.Text+"' where INM_CODE='" + mlCode + "'"))
                {
                    if (!chkIsSuppliement.Checked)
                    {
                        DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + mlCode + " ");
                        for (int i = 0; i < dtq.Rows.Count; i++)
                        {
                            CommonClasses.Execute("UPDATE CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                        }
                        result = CommonClasses.Execute1("DELETE FROM INVOICE_DETAIL WHERE IND_INM_CODE='" + mlCode + "'");
                        result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + mlCode + "' and STL_DOC_TYPE='TAXINV'");
                    }
                    if (result)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC) values ('" + mlCode + "','" + dt2.Rows[i]["IND_I_CODE"] + "','" + dt2.Rows[i]["PO_CODE"] + "','" + dt2.Rows[i]["INV_QTY"] + "','" + dt2.Rows[i]["RATE"] + "','" + dt2.Rows[i]["AMT"] + "','" + dt2.Rows[i]["IND_SUBHEADING"] + "','" + dt2.Rows[i]["IND_BACHNO"] + "','" + dt2.Rows[i]["IND_NO_PACK"] + "','" + dt2.Rows[i]["IND_PAK_QTY"] + "','" + dt2.Rows[i]["IND_EX_AMT"] + "','" + dt2.Rows[i]["IND_E_CESS_AMT"] + "','" + dt2.Rows[i]["IND_SH_CESS_AMT"] + "','" + dt2.Rows[i]["INV_QTY"] + "','" + dt2.Rows[i]["IND_PACK_DESC"] + "')");
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + dt2.Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + dt2.Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + dt2.Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + dt2.Rows[i]["IND_I_CODE"] + "','" + mlCode + "','" + txtInvoiceNo.Text + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + dt2.Rows[i]["INV_QTY"] + "')");

                            }
                            //Removing Stock
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dt2.Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + dt2.Rows[i]["IND_I_CODE"] + "'");
                            }
                        }
                        CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", mlCode);
                        CommonClasses.WriteLog("Tax Invoice", "Update", "Tax Invoice", Convert.ToString(mlCode), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        dt2.Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewTaxinvoice.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    protected void ddlPurchaseNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select PR_OTHER_AMT,PR_FREIGHT_AMT,PR_INSURANCE_AMT,PR_TRANS_AMT,PR_OCTRI_AMT,PR_TCS_AMT,PR_GRAND_TOT   from PURCHASE_REJECTION_MASTER where PR_NO='" + ddlPurchaseNo.SelectedItem.Text + "'");
            if (dt.Rows.Count > 0)
            {
                txtOtherCharges.Text = dt.Rows[0]["PR_OTHER_AMT"].ToString();
                txtFreight.Text = dt.Rows[0]["PR_FREIGHT_AMT"].ToString();
                txtIncurance.Text = dt.Rows[0]["PR_INSURANCE_AMT"].ToString();
                txtTransportAmt.Text = dt.Rows[0]["PR_TRANS_AMT"].ToString();
                txtOctri.Text = dt.Rows[0]["PR_OCTRI_AMT"].ToString(); ;
                txtTCSAmt.Text = dt.Rows[0]["PR_TCS_AMT"].ToString();
            }
            DataTable dt1 = CommonClasses.Execute("select PRD_RATE from PURCHASE_REJECTION_DETAIL where PRD_PR_CODE =" + ddlPurchaseNo.SelectedValue  + "");
            DataTable dtQty = CommonClasses.Execute("SELECT (PRD_RECEIVED_QTY) as Qty FROM PURCHASE_REJECTION_DETAIL where PRD_PR_CODE=" + ddlPurchaseNo.SelectedValue  + "");
            if (dt1.Rows.Count > 0)
            {
                txtRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["PRD_RATE"]);
            }
            else
            {
                txtRate.Text = "0.00";
            }
            if (dtQty.Rows.Count > 0)
            {
                txtPendingQty.Text = string.Format("{0:0.000}", dtQty.Rows[0]["Qty"]);
                txtVQty.Text = string.Format("{0:0.000}", dtQty.Rows[0]["Qty"]);
                txtVQty_OnTextChanged(null, null);
            }
            else
            {
                txtPendingQty.Text = "0.000";
                txtVQty.Text = "0.000";
            }
        }
        catch (Exception ex)
        {
        }
    }
}
        