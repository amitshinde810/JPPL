using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_CashInward : System.Web.UI.Page
{
    #region General Declaration
    Inward_BL BL_InwardMaster = null;
    double PedQty = 0;
    static int mlCode = 0;
    static string right = "";
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    public static string str = "";
    public static int Index = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();
    #endregion
    public string Msg = "";
    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='39'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    ViewState["dt2"] = dt2;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    ViewState["ItemUpdateIndex"] = "-1"; // ItemUpdateIndex;
                    ViewState["str"] = str;
                    ViewState["Index"] = Index;
                    LoadCombos();
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_InwardMaster = new Inward_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["mlCode"] = mlCode;
                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_InwardMaster = new Inward_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["mlCode"] = mlCode;
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtGRNDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");

                        BlankGrid();
                        LoadCombos();

                        dtFilter.Rows.Clear();
                        ViewState["str"] = "";
                        txtGRNDate.Attributes.Add("readonly", "readonly");
                        txtChallanDate.Attributes.Add("readonly", "readonly");
                        txtInvoiceDate.Attributes.Add("readonly", "readonly");
                        chkWithOutPO_CheckedChanged(null, null);
                    }
                    ddlSupplier.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Material Inward", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region BlankGrid
    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_RATE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_UOM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REMARK", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_BATCH_NO", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInwardMaster.DataSource = dtFilter;
            dgInwardMaster.DataBind();
            ((DataTable)ViewState["dt2"]).Rows.Clear();
        }
    }
    #endregion BlankGrid

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillSupplier
        try
        {
            DataTable dtParty = new DataTable();
            if (Convert.ToInt32(ViewState["mlCode"]) == 0)
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0  and SPOM_CANCEL_PO=0  AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY) AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SPOM_POTYPE=0 and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE PARTY_MASTER.ES_DELETE=0 AND SPOM_P_CODE=P_CODE AND SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POST=1   AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND  SPOM_POTYPE=0 and P_TYPE='2' and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillItem
        try
        {
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillUOM
        try
        {
            dt = CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_UOM_NAME", ddlUom);
            ddlUom.Items.Insert(0, new ListItem(" ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region PO
        try
        {
            dt = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER,COMPANY_MASTER where ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Session["CompanyId"] + "' and SPOM_CM_CODE=CM_CODE order by SPOM_CODE desc");
            ddlPoNumber.DataSource = dt;
            ddlPoNumber.DataTextField = "SPOM_PO_NO";
            ddlPoNumber.DataValueField = "SPOM_CODE";
            ddlPoNumber.DataBind();
            ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            {
                if (((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtGRNDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtGRNDate.Text))) && ((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtChallanDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtChallanDate.Text))))
                {
                    if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Supplier Name";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemCode.Focus();
                        return;
                    }
                    if (txtChallanNo.Text == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field Challan/Invoice No Is Required";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemCode.Focus();
                        return;
                    }
                    if (Convert.ToDateTime(txtGRNDate.Text) < Convert.ToDateTime(Session["CompanyOpeningDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Date in current financial year ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtGRNDate.Focus();
                        return;
                    }

                    if (Convert.ToDateTime(txtGRNDate.Text) > Convert.ToDateTime(Session["CompanyClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Select Date in current financial year ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtGRNDate.Focus();
                        return;
                    }
                    if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
                    {
                        SaveRec();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemName.Focus();
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Current Year Date ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtGRNDate.Attributes.Add("readonly", "readonly");
            txtInvoiceDate.Attributes.Add("readonly", "readonly");
            dt = CommonClasses.Execute("SELECT IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,IWM_CODE,IWM_INWARD_TYPE,IWM_NO,IWM_TYPE,IWM_DATE,IWM_P_CODE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWM_EGP_NO,IWM_LR_NO,IWM_OCT_NO,IWM_VEH_NO,IWM_CM_CODE FROM INWARD_MASTER where ES_DELETE=0 and IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and IWM_CODE=" + ViewState["mlCode"] + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["IWM_CODE"]);
                string id = dt.Rows[0]["IWM_P_CODE"].ToString();
                if (id == "-2147483578")
                {
                    rbLstIsPO.SelectedValue = "1";
                    DataTable dtS = CommonClasses.FillCombo("PARTY_MASTER,SUPP_PO_MASTER", "P_NAME", "P_CODE", "SUPP_PO_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + "   and P_ACTIVE_IND=1 and SPOM_P_CODE=P_CODE  ORDER BY P_NAME", ddlSupplier);
                }
                if (rbLstIsPO.SelectedValue == "1")
                {
                    LoadcashSuppiler();
                }
                else
                {
                    LoadCombos();
                }
                rbLstIsPO.Enabled = false;
                txtGRNno.Text = Convert.ToInt32(dt.Rows[0]["IWM_NO"]).ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = id;
                txtChallanNo.Text = dt.Rows[0]["IWM_CHALLAN_NO"].ToString();
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_CHAL_DATE"]).ToString("dd MMM yyyy");
                txtEgpNo.Text = (dt.Rows[0]["IWM_EGP_NO"]).ToString();
                txtLrNo.Text = (dt.Rows[0]["IWM_LR_NO"]).ToString();
                txtOctNo.Text = (dt.Rows[0]["IWM_OCT_NO"]).ToString();
                txtVehno.Text = (dt.Rows[0]["IWM_VEH_NO"]).ToString();
                txtInvoiceNo.Text = (dt.Rows[0]["IWM_INV_NO"]).ToString();
                txtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_INV_DATE"]).ToString("dd MMM yyyy");
                if (txtInvoiceDate.Text == "01 Jan 1900")
                {
                    txtInvoiceDate.Text = "";
                }
                else
                {
                }
                txtTransporterName.Text = (dt.Rows[0]["IWM_TRANSPORATOR_NAME"]).ToString();
                ddlItemName.Items.Clear();
                if (id == "-2147483578")
                {
                    CommonClasses.FillCombo("ITEM_MASTER  ", "I_NAME", "I_CODE", "ES_DELETE=0 AND I_CODE=-2147483158 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemName);
                    CommonClasses.FillCombo("ITEM_MASTER  ", "I_NAME", "I_CODE", "ES_DELETE=0 AND I_CODE=-2147483158 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemCode);
                }
                else
                {
                    CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD ", "I_NAME", "I_CODE", "SM.SPOM_P_CODE=" + id + " and SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SD.SPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemName);
                    CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD ", "I_NAME", "I_CODE", "SM.SPOM_P_CODE=" + id + " and SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SD.SPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemCode);
                }
                ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                if (id != "-2147483578") //2147483271
                {
                    ddlPoNumber.Items.Clear();

                    dt = CommonClasses.Execute("select  DISTINCT SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER SM,SUPP_PO_DETAILS SD,ITEM_MASTER IM ,COMPANY_MASTER where SPOM_CM_CODE=CM_CODE AND SM.SPOM_CODE=SD.SPOD_SPOM_CODE and SM.SPOM_P_CODE=" + ddlSupplier.SelectedValue + " and IM.ES_DELETE=0 and SPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]) + " AND SPOM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"].ToString()) + "'");
                    ddlPoNumber.DataSource = dt;
                    ddlPoNumber.DataTextField = "SPOM_PO_NO";
                    ddlPoNumber.DataValueField = "SPOM_CODE";
                    ddlPoNumber.DataBind();
                    dtInwardDetail = CommonClasses.Execute("SELECT IWD_BATCH_NO,IWD_IWM_CODE,IWD_I_CODE,I_CODENO,I_NAME,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO,cast(IWD_RATE  as numeric(20,2)) as IWD_RATE,IWD_UOM_CODE,IWD_CPOM_CODE,cast(IWD_CH_QTY as numeric(10,3)) as IWD_CH_QTY,cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME,IWD_REMARK FROM INWARD_DETAIL,INWARD_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,SUPP_PO_MASTER,COMPANY_MASTER WHERE IWM_CODE=IWD_IWM_CODE and IWD_I_CODE=ITEM_MASTER.I_CODE AND SPOM_CODE=IWD_CPOM_CODE AND IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and  SPOM_CM_CODE=CM_CODE and IWD_IWM_CODE='" + ViewState["mlCode"] + "'");
                }
                else
                {
                    dtInwardDetail = CommonClasses.Execute("SELECT IWD_BATCH_NO,IWD_IWM_CODE,IWD_I_CODE,I_CODENO,I_NAME,0 as SPOM_PO_NO,cast(IWD_RATE  as numeric(20,2)) as IWD_RATE,IWD_UOM_CODE,IWD_CPOM_CODE,cast(IWD_CH_QTY as numeric(10,3)) as IWD_CH_QTY,cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME,IWD_REMARK FROM INWARD_DETAIL,INWARD_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,COMPANY_MASTER WHERE IWM_CODE=IWD_IWM_CODE and IWD_I_CODE=ITEM_MASTER.I_CODE AND IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and    IWD_IWM_CODE='" + ViewState["mlCode"] + "'");
                }

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                }
            }
            if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;

                ddlItemName.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlPoNumber.Enabled = false;
                txtChallanQty.Enabled = false;
                txtRecdQty.Enabled = false;
                txtRemark.Enabled = false;
                dgInwardMaster.Enabled = false;
                txtBatchNo.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtInvoiceDate.Enabled = false;
            }
            if (str == "MOD")
            {
                ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                ddlSupplier.SelectedValue = BL_InwardMaster.IWM_P_CODE.ToString();
                txtGRNno.Text = BL_InwardMaster.IWM_NO.ToString();
                txtChallanNo.Text = BL_InwardMaster.IWM_CHALLAN_NO.ToString();
                txtEgpNo.Text = BL_InwardMaster.IWM_EGP_NO.ToString();
                txtLrNo.Text = BL_InwardMaster.IWM_LR_NO.ToString();
                txtOctNo.Text = BL_InwardMaster.IWM_OCT_NO.ToString();
                txtVehno.Text = BL_InwardMaster.IWM_VEH_NO.ToString();
                txtGRNDate.Text = BL_InwardMaster.IWM_DATE.ToString("dd MMM YYYY");
                txtChallanDate.Text = BL_InwardMaster.IWM_CHAL_DATE.ToString("dd MMM YYYY");
            }
            else if (str == "VIEW")
            {
                ddlSupplier.Enabled = false;
                txtGRNno.Enabled = false;
                txtChallanNo.Enabled = false;
                txtEgpNo.Enabled = false;
                txtLrNo.Enabled = false;
                txtOctNo.Enabled = false;
                txtVehno.Enabled = false;
                txtChallanDate.Enabled = false;
                txtGRNDate.Enabled = false;
                BtnInsert.Visible = false;
                btnSubmit.Visible = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_InwardMaster.IWM_P_CODE = Convert.ToInt32(ddlSupplier.SelectedValue);
            BL_InwardMaster.IWM_NO = Convert.ToInt32(txtGRNno.Text);
            BL_InwardMaster.IWM_CHALLAN_NO = txtChallanNo.Text.ToString();
            BL_InwardMaster.IWM_TYPE = "IWCP";
            BL_InwardMaster.IWM_INWARD_TYPE = 1;
            BL_InwardMaster.IWM_CHAL_DATE = Convert.ToDateTime(txtChallanDate.Text);
            BL_InwardMaster.IWM_DATE = Convert.ToDateTime(txtGRNDate.Text);
            BL_InwardMaster.IWM_EGP_NO = txtEgpNo.Text.ToString();
            BL_InwardMaster.IWM_LR_NO = txtLrNo.Text.ToString();
            BL_InwardMaster.IWM_OCT_NO = txtOctNo.Text.ToString();
            BL_InwardMaster.IWM_VEH_NO = txtVehno.Text.ToString();
            BL_InwardMaster.IWM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            BL_InwardMaster.IWM_INV_NO = txtInvoiceNo.Text;

            if (txtInvoiceDate.Text == "")
            {
                BL_InwardMaster.IWM_INV_DATE = Convert.ToDateTime("1/1/1900");
            }
            else
            {
                BL_InwardMaster.IWM_INV_DATE = Convert.ToDateTime(txtInvoiceDate.Text);
            }
            BL_InwardMaster.IWM_TRANSPORATOR_NAME = txtTransporterName.Text;
            BL_InwardMaster.IWD_BATCH_NO = txtBatchNo.Text;
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_InwardMaster = new Inward_BL();
                txtGRNno.Text = Numbering();
                if (Setvalues())
                {
                    if (BL_InwardMaster.Save(dgInwardMaster))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(IWM_CODE) from INWARD_MASTER");
                        CommonClasses.WriteLog("Material Inward", "Insert", "Material Inward", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);
                    }
                    else
                    {
                        if (BL_InwardMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_InwardMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_InwardMaster.Msg = "";
                        }
                        ddlSupplier.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_InwardMaster = new Inward_BL(Convert.ToInt32(ViewState["mlCode"]));

                if (Setvalues())
                {
                    if (BL_InwardMaster.Update(dgInwardMaster))
                    {
                        CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Material Inward ", "Update", "Material Inward", ViewState["mlCode"].ToString(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);
                    }
                    else
                    {
                        if (BL_InwardMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_InwardMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_InwardMaster.Msg = "";
                        }
                        ddlSupplier.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Material Inward", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(IWM_NO) as IWM_NO from INWARD_MASTER where IWM_CM_CODE='" + Session["CompanyCode"] + "' and ES_DELETE=0  AND   IWM_TYPE='IWCP'");
        if (dt.Rows[0]["IWM_NO"] == null || dt.Rows[0]["IWM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["IWM_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    protected void dgInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    #region dgInwardMaster_RowCommand
    protected void dgInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["Index"] = Index;
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"])];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgInwardMaster.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                dgInwardMaster.DataBind();
                if (dgInwardMaster.Rows.Count == 0)
                    BlankGrid();
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                string s = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIWD_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                // LoadCombos();
                ddlPoNumber.SelectedValue = ((Label)(row.FindControl("lblIWD_CPOM_CODE"))).Text;
                txtRecdQty.Text = ((Label)(row.FindControl("lblIWD_REV_QTY"))).Text;
                if (txtRecdQty.Text == "")
                {
                    txtRecdQty.Text = "0";
                }
                txtRate.Text = ((Label)(row.FindControl("lblIWD_RATE"))).Text;
                txtChallanQty.Text = ((Label)(row.FindControl("lblIWD_CH_QTY"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblIWD_REMARK"))).Text;
                dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,SPOD_RATE,SPOD_UOM_CODE FROM SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
                if (rbLstIsPO.SelectedValue == "1")
                {
                    PedQty = 0;
                }
                else
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]) + Convert.ToDouble(txtRecdQty.Text);
                }
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(PedQty.ToString()));
                txtBatchNo.Text = ((Label)(row.FindControl("lblIWD_BATCH_NO"))).Text;
                // For Enabled Delete Button when record is modify.
                foreach (GridViewRow gvr in dgInwardMaster.Rows)
                {
                    LinkButton lnkButton = ((LinkButton)gvr.FindControl("lnkDelete"));
                    lnkButton.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedValue = "0";
            ddlItemCode.SelectedValue = "0";
            ddlPoNumber.SelectedValue = "0";
            txtRate.Text = "0.00";
            txtChallanQty.Text = "0.00";
            txtRecdQty.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Material Inward ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            if (Convert.ToInt32(ddlSupplier.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Supplier Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (txtChallanNo.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Challan/Invoice No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (Convert.ToInt32(ddlItemName.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (txtChallanQty.Text == "" || Convert.ToDecimal(txtChallanQty.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtChallanQty.Focus();
                return;
            }
            double chq = Convert.ToDouble(txtChallanQty.Text);
            double pdq = Convert.ToDouble(txtPendingQty.Text);

            if (rbLstIsPO.SelectedValue != "1")
            {
                if (pdq < chq)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Challan quantity should always be less or equal to pending quantity";
                    txtChallanQty.Text = "";
                    txtChallanQty.Focus();
                    return;
                }
            }
            if (txtRecdQty.Text == "" || Convert.ToDecimal(txtRecdQty.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Recd. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRecdQty.Focus();
                return;
            }
            double req = Convert.ToDouble(txtRecdQty.Text);

            if (chq < req)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Recd. Qty less or equal to Challan Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRecdQty.Text = "";
                txtRecdQty.Focus();
                return;
            }
            if (rbLstIsPO.SelectedValue == "1")
            {
                if (txtRate.Text.Trim() == "")
                {
                    txtRate.Text = "0";
                }
                if ((req * Convert.ToDouble(txtRate.Text)) > 1000)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Cash GIN amount should be less than 1000 Rs";
                    txtRecdQty.Text = "";
                    txtRecdQty.Focus();
                    return;
                }
            }
            if (Convert.ToDouble(txtRate.Text) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Rate";
                txtRate.Focus(); return;
            }
            PanelMsg.Visible = false;

            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblIWD_I_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_CPOM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("SPOM_PO_NO", typeof(string));
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_CH_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_REV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_REMARK");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_UOM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IWD_BATCH_NO");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IWD_I_CODE"] = ddlItemName.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem;
            dr["I_NAME"] = ddlItemName.SelectedItem;
            if (rbLstIsPO.SelectedValue == "0")
            {
                dr["IWD_CPOM_CODE"] = ddlPoNumber.SelectedValue;
                dr["SPOM_PO_NO"] = ddlPoNumber.SelectedItem.Text;
            }
            else
            {
                dr["IWD_CPOM_CODE"] = 0;
                dr["SPOM_PO_NO"] = 0;
            }
            dr["IWD_RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text)));
            dr["IWD_CH_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtChallanQty.Text)));
            dr["IWD_REV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtRecdQty.Text)));
            dr["IWD_REMARK"] = txtRemark.Text;
            dr["I_UOM_NAME"] = ddlUom.SelectedItem;
            dr["IWD_UOM_CODE"] = ddlUom.SelectedValue;
            dr["IWD_BATCH_NO"] = txtBatchNo.Text;

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"]));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"]));
                    txtChallanQty.Text = "";
                    txtRecdQty.Text = "";
                    txtRemark.Text = "";
                    ddlItemName.SelectedIndex = 0;
                    ddlItemCode.SelectedIndex = 0;
                    txtPendingQty.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtChallanQty.Text = "";
                txtRecdQty.Text = "";
                txtRemark.Text = "";
                ddlItemName.SelectedIndex = 0;
                ddlItemCode.SelectedIndex = 0;
                txtPendingQty.Text = "";
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            #endregion
            //To avoid same item insert in Grid
            ViewState["ItemUpdateIndex"] = "-1";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCode.SelectedIndex != 0)
        {
            ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
            DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");

            if (dt1.Rows.Count > 0)
            {
                ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
            }
            else
            {
                ddlUom.Text = "";
            }
        }
        txtChallanQty.Text = "";
        txtRecdQty.Text = "";
        pendingQty();
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
        if (ddlItemName.SelectedIndex != 0)
        {
            ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
            DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");

            if (dt1.Rows.Count > 0)
            {
                ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
            }
            else
            {
                ddlUom.Text = "";
            }
            txtChallanQty.Text = "";
            txtRecdQty.Text = "";
            pendingQty();
        }
    }
    #endregion

    #region ddlPoNumber_SelectedIndexChanged
    protected void ddlPoNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtChallanQty.Text = "";
        txtRecdQty.Text = "";
        try
        {
            dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
                {
                    txtChallanQty.Enabled = false;
                    txtRecdQty.Enabled = false;
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Only Post PO Can InWard";
                }
                else
                {
                    PanelMsg.Visible = false;
                    txtChallanQty.Enabled = true;
                    txtRecdQty.Enabled = true;
                }
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
                txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["SPOD_RATE"].ToString()));
                ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();
            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            if (rbLstIsPO.SelectedValue != "1")
            {
                int id = Convert.ToInt32(ddlSupplier.SelectedValue);
                ddlItemName.Items.Clear();
                ddlItemCode.Items.Clear();
                DataTable dtItem = new DataTable();
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY)  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND  SPOM_POST=1 AND SPOM_P_CODE=" + id + " ORDER BY I_NAME");
                ddlItemName.DataSource = dtItem;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemCode.DataSource = dtItem;
                ddlItemCode.DataTextField = "I_CODENO";
                ddlItemCode.DataValueField = "I_CODE";
                ddlItemCode.DataBind();
                ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlItemCode.Items.Insert(0, new ListItem("Please Select Item", "0"));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "ddlSupplier_SelectedIndexChanged", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region pendingQty
    private void pendingQty()
    {
        #region PendingQty
        try
        {
            ddlPoNumber.Items.Clear();
            dtPO = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PONO) as SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_MASTER,COMPANY_MASTER WHERE  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND SPOD_I_CODE=I_CODE AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and SPOM_CM_CODE=CM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by SPOM_CODE");//AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY)
            ddlPoNumber.DataSource = dtPO;
            ddlPoNumber.DataTextField = "SPOM_PO_NO";
            ddlPoNumber.DataValueField = "SPOM_CODE";
            ddlPoNumber.DataBind();
            dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
                {
                    txtChallanQty.Enabled = false;
                    txtRecdQty.Enabled = false;
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Only Post PO Can Inward";
                }

                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
                txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["SPOD_RATE"].ToString()));
                ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();
            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "PendingQty", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region CtlDisable
    public void CtlDisable()
    {
        ddlSupplier.Enabled = false;
        txtGRNno.Enabled = false;
        txtChallanNo.Enabled = false;
        txtEgpNo.Enabled = false;
        txtLrNo.Enabled = false;
        txtOctNo.Enabled = false;
        txtVehno.Enabled = false;
        txtChallanDate.Enabled = false;
        txtGRNDate.Enabled = false;
        BtnInsert.Visible = false;
        btnSubmit.Visible = false;
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
                    // ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("Material Inward", "btnOk_Click", Ex.Message);
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
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && ViewState["mlCode"] != null)
            {
                CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlSupplier.Text == "")
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
            else if (txtGRNDate.Text == "")
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
            CommonClasses.SendError("Material Inward", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region txtChallanQty_TextChanged
    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);
        txtChallanQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }
    #endregion txtChallanQty_TextChanged

    #region txtRecdQty_TextChanged
    protected void txtRecdQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRecdQty.Text);
        txtRecdQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }
    #endregion txtRecdQty_TextChanged

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRate.Text);
        txtRate.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }
    #endregion txtRate_TextChanged

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

    #region chlWithPO_CheckedChanged
    protected void chlWithPO_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chlWithPO.Checked)
            {
                chkWithOutPO.Checked = false;
                ddlSupplier.Enabled = true;
                ddlPoNumber.Visible = true;
                LoadCombos();
                Div3.Visible = false;
                txtRate.Enabled = true;
                BlankGrid();
            }
            else
            {
                chkWithOutPO.Checked = true;
                ddlSupplier.Enabled = false;
                ddlPoNumber.Visible = false;
                LoadcashSuppiler();
                Div3.Visible = true;
                BlankGrid();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region chkWithOutPO_CheckedChanged
    protected void chkWithOutPO_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkWithOutPO.Checked)
            {
                chlWithPO.Checked = false;
                ddlSupplier.Enabled = false;
                ddlPoNumber.Visible = false;
                LoadcashSuppiler();
                Div3.Visible = true;
            }
            else
            {
                chlWithPO.Checked = true;
                ddlSupplier.Enabled = true;
                ddlPoNumber.Visible = true;
                LoadCombos();
                Div3.Visible = false;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadcashSuppiler
    public void LoadcashSuppiler()
    {
        dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  AND     P_ACTIVE_IND=1  AND P_CODE IN (SELECT P_CODE FROM CASH_INWARD ) ORDER BY P_NAME ");
        ddlSupplier.DataSource = dt;
        ddlSupplier.DataTextField = "P_NAME";
        ddlSupplier.DataValueField = "P_CODE";
        ddlSupplier.DataBind();
        ddlItemCode.DataSource = null;
        ddlItemCode.DataBind();
        ddlItemName.DataSource = null;
        ddlItemName.DataBind();

        DataTable dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER WHERE ITEM_MASTER.ES_DELETE=0 AND I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     AND I_CODE IN (SELECT I_CODE FROM CASH_INWARD ) ORDER BY I_NAME ");
        ddlItemCode.DataSource = dtItem;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new ListItem("Please Select Item", "0"));
        ddlItemName.DataSource = dtItem;
        ddlItemName.DataTextField = "I_NAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
    }
    #endregion LoadcashSuppiler

    #region rbLstIsPO_SelectedIndexChanged
    protected void rbLstIsPO_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            #region Empty_Grid_Fields
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            BlankGrid();
            txtRate.Text = "0.00";
            txtChallanQty.Text = "0.00";
            txtRecdQty.Text = "0.00";
            #endregion Empty_Grid_Fields

            if (rbLstIsPO.SelectedValue == "1")
            {
                chlWithPO.Checked = false;
                ddlSupplier.Enabled = false;
                ddlPoNumber.Visible = false;
                LoadcashSuppiler();
                Div3.Visible = true;
            }
            else
            {
                chlWithPO.Checked = true;
                ddlSupplier.Enabled = true;
                ddlPoNumber.Visible = true;
                LoadCombos();
                Div3.Visible = false;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion rbLstIsPO_SelectedIndexChanged
}
