using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_ADD_ForProcessInward : System.Web.UI.Page
{
    #region General Declaration
    ForProcessInward BL_InwardMaster = null;
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
                    ViewState["mlCode"] = mlCode;
                    ViewState["Index"] = Index;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["dt2"] = dt2;
                    str = "";
                    ViewState["str"] = str;
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_InwardMaster = new ForProcessInward();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_InwardMaster = new ForProcessInward();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtGRNDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");

                        BlankGrid();
                        LoadCombos();
                        dtFilter.Rows.Clear();
                        txtGRNDate.Attributes.Add("readonly", "readonly");
                        txtChallanDate.Attributes.Add("readonly", "readonly");
                        txtInvoiceDate.Attributes.Add("readonly", "readonly");
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
            ((DataTable)ViewState["dt2"]).Clear();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillCustomer
        try
        {
            DataTable dtParty = new DataTable();

            //for job work po
            dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL WHERE PARTY_MASTER.ES_DELETE=0 AND CPOM_P_CODE=P_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0  AND CPOM_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND  CPOM_TYPE=-2147483648 and P_TYPE='1' and P_ACTIVE_IND=1 ORDER BY P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("For Process Inward", "LoadCombos", Ex.Message);
        }
        #endregion

        #region FillItem
        try
        {
            dt = CommonClasses.FillCombo("ITEM_MASTER,SUPP_PO_DETAILS", "I_NAME", "I_CODE", "ES_DELETE=0 and i_code=SPOD_I_CODE and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_NAME", ddlItemName);
            ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));

            DataTable dt1 = CommonClasses.FillCombo("ITEM_MASTER,SUPP_PO_DETAILS", "I_CODENO", "I_CODE", "ES_DELETE=0 and i_code=SPOD_I_CODE and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY I_CODENO", ddlItemCode);
            ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
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
            dt = CommonClasses.Execute("SELECT Convert(varchar,CPOM_PONO) + '/' + CONVERT(varchar,CPOM_DATE,106) as CPOM_PONO ,CPOM_CODE FROM CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]) + "  AND CPOM_TYPE=-2147483648");
            //dt = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER,COMPANY_MASTER where ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Session["CompanyId"] + "' and SPOM_CM_CODE=CM_CODE order by SPOM_CODE desc");
            ddlPoNumber.DataSource = dt;
            ddlPoNumber.DataTextField = "CPOM_PONO";
            ddlPoNumber.DataValueField = "CPOM_CODE";
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
            ddlInwardType.Enabled = false;
            LoadCombos();
            dt = CommonClasses.Execute("SELECT IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,IWM_CODE,IWM_INWARD_TYPE,IWM_NO,IWM_TYPE,IWM_DATE,IWM_P_CODE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWM_EGP_NO,IWM_LR_NO,IWM_OCT_NO,IWM_VEH_NO,IWM_CM_CODE FROM INWARD_MASTER where ES_DELETE=0 and IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and IWM_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["IWM_CODE"]);
                ddlInwardType.SelectedValue = Convert.ToInt32(dt.Rows[0]["IWM_INWARD_TYPE"]).ToString(); ;
                txtGRNno.Text = Convert.ToInt32(dt.Rows[0]["IWM_NO"]).ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IWM_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = Convert.ToInt32(dt.Rows[0]["IWM_P_CODE"]).ToString();
                txtChallanNo.Text = (dt.Rows[0]["IWM_CHALLAN_NO"]).ToString();
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
                int id = Convert.ToInt32(ddlSupplier.SelectedValue);
                CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,CUSTPO_MASTER SM,CUSTPO_DETAIL SD ", "I_NAME", "I_CODE", "SM.CPOM_P_CODE=" + id + " and SM.CPOM_TYPE=-2147483648  AND  SM.CPOM_CODE=SD.CPOD_CPOM_CODE and SD.CPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemName);
                CommonClasses.FillCombo("ITEM_MASTER IM,PARTY_MASTER PM,CUSTPO_MASTER SM,CUSTPO_DETAIL SD ", "I_CODENO", "I_CODE", "SM.CPOM_P_CODE=" + id + " and SM.CPOM_TYPE=-2147483648  AND  SM.CPOM_CODE=SD.CPOD_CPOM_CODE and SD.CPOD_I_CODE=IM.I_CODE and IM.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"], ddlItemCode);
                ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
                ddlPoNumber.Items.Clear();

                dt = CommonClasses.Execute("SELECT Convert(varchar,CPOM_PONO) + '/' + CONVERT(varchar,CPOM_DATE,106) as CPOM_PONO ,CPOM_CODE FROM CUSTPO_MASTER,CUSTPO_DETAIL  where CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOM_P_CODE=" + ddlSupplier.SelectedValue + " and CPOM_CM_COMP_ID=" + Convert.ToInt32(Session["CompanyId"]) + "  AND CPOM_TYPE=-2147483648");
                ddlPoNumber.DataSource = dt;
                ddlPoNumber.DataTextField = "CPOM_PONO";
                ddlPoNumber.DataValueField = "CPOM_CODE";
                ddlPoNumber.DataBind();

                dtInwardDetail = CommonClasses.Execute(" SELECT IWD_BATCH_NO,IWD_IWM_CODE,IWD_I_CODE,I_CODENO,I_NAME,Convert(varchar,CPOM_PONO) + '/' + CONVERT(varchar,CPOM_DATE,106) as SPOM_PO_NO,cast(IWD_RATE  as numeric(20,2)) as IWD_RATE,IWD_UOM_CODE,IWD_CPOM_CODE,cast(IWD_CH_QTY as numeric(10,3)) as IWD_CH_QTY,cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME,IWD_REMARK FROM INWARD_DETAIL,INWARD_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_MASTER  WHERE IWM_CODE=IWD_IWM_CODE and IWD_I_CODE=ITEM_MASTER.I_CODE AND CPOM_CODE=IWD_CPOM_CODE AND IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  and    IWD_IWM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");

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
            BL_InwardMaster.IWM_TYPE = "IWIFP";
            BL_InwardMaster.IWM_INWARD_TYPE = Convert.ToInt32(ddlInwardType.SelectedValue);
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
                BL_InwardMaster = new ForProcessInward();
                txtGRNno.Text = Numbering();
                if (Setvalues())
                {
                    if (BL_InwardMaster.Save(dgInwardMaster))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(IWM_CODE) from INWARD_MASTER");
                        CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_INSP_FLG=0 WHERE IWD_IWM_CODE='" + Code + "'");
                        CommonClasses.WriteLog("For Process Inward ", "Insert", "Material Inward", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
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
                BL_InwardMaster = new ForProcessInward(Convert.ToInt32(ViewState["mlCode"]));

                if (Setvalues())
                {
                    if (BL_InwardMaster.Update(dgInwardMaster))
                    {
                        CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Material Inward ", "Update", "Material Inward", Convert.ToInt32(ViewState["mlCode"]).ToString(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
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
        dt = CommonClasses.Execute("select max(IWM_NO) as IWM_NO from INWARD_MASTER where IWM_CM_CODE='" + Session["CompanyCode"] + "' AND ES_DELETE=0 AND IWM_TYPE='IWIFP'");
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
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
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
                ddlPoNumber.SelectedValue = (((Label)(row.FindControl("lblIWD_CPOM_CODE"))).Text).ToString();
                txtRecdQty.Text = ((Label)(row.FindControl("lblIWD_REV_QTY"))).Text;
                if (txtRecdQty.Text == "")
                {
                    txtRecdQty.Text = "0";
                }
                txtRate.Text = ((Label)(row.FindControl("lblIWD_RATE"))).Text;
                txtChallanQty.Text = ((Label)(row.FindControl("lblIWD_CH_QTY"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblIWD_REMARK"))).Text;
                dt = CommonClasses.Execute("select (ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_DISPACH,0)) as PENDQTY,CPOD_RATE AS SPOD_RATE,CPOD_UOM_CODE AS SPOD_UOM_CODE FROM CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]) + Convert.ToDouble(txtRecdQty.Text);
                }
                else
                {
                    PedQty = Convert.ToDouble(dt.Rows[0]["PENDQTY"]);
                }
                txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(PedQty.ToString()));
                ddlUom.SelectedValue = ((Label)(row.FindControl("lblUOM_CODE"))).Text;
                txtBatchNo.Text = ((Label)(row.FindControl("lblIWD_BATCH_NO"))).Text;
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

            if (pdq != 0)
            {
                if (pdq > 0)
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
            PanelMsg.Visible = false;

            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblIWD_I_CODE"))).Text;
                    if ((ViewState["ItemUpdateIndex"]).ToString() == "-1")
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
                        if (ITEM_CODE == ddlItemName.SelectedValue.ToString() && (ViewState["ItemUpdateIndex"]).ToString() != i.ToString())
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
            dr["IWD_CPOM_CODE"] = ddlPoNumber.SelectedValue;
            dr["SPOM_PO_NO"] = ddlPoNumber.SelectedItem.Text;
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
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            #endregion
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
            DataTable dtIStock = new DataTable();
            double inwardstock = 0;
            if (ddlInwardType.SelectedValue == "0")
            {
                //dt = CommonClasses.Execute(" SELECT  SUM(( BD_VQTY +BD_SCRAPQTY) *(CPOD_ORD_QTY-CPOD_DISPACH)) AS PENDQTY,I_INV_RATE AS CPOD_RATE,I_UOM_CODE AS CPOD_UOM_CODE,0 AS SPOM_POST  FROM BOM_MASTER,BOM_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER   where BM_CODE=BD_BM_CODE   AND CPOM_CODE=CPOD_CPOM_CODE AND BD_I_CODE=I_CODE AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'    AND  CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND BM_I_CODE=CPOD_I_CODE AND BD_I_CODE='" + ddlItemName.SelectedValue + "' ");
                dt = CommonClasses.Execute(" SELECT  SUM(( BD_VQTY +BD_SCRAPQTY) *(CPOD_ORD_QTY-CPOD_DISPACH)) AS PENDQTY,I_INV_RATE AS CPOD_RATE,I_UOM_CODE AS CPOD_UOM_CODE,0 AS SPOM_POST  FROM BOM_MASTER,BOM_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER   where BM_CODE=BD_BM_CODE   AND CPOM_CODE=CPOD_CPOM_CODE AND BD_I_CODE=I_CODE AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'    AND  CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND BM_I_CODE=CPOD_I_CODE AND BD_I_CODE='" + ddlItemName.SelectedValue + "'   GROUP BY I_INV_RATE,I_UOM_CODE");
                dtIStock = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0)  AS IWD_REV_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_TYPE='IWIFP' AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND IWD_CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND IWD_I_CODE='" + ddlItemName.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0");

                // dtIStock = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0)  AS IWD_REV_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_TYPE='IWIFP' AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND IWD_CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND IWD_I_CODE='" + ddlItemName.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0  GROUP BY I_INV_RATE,I_UOM_CODE");
                if (dtIStock.Rows.Count > 0)
                {
                    inwardstock = Convert.ToDouble(dtIStock.Rows[0]["IWD_REV_QTY"].ToString());
                }
            }
            else
            {
                dt = CommonClasses.Execute(" select (ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_DISPACH,0)) as PENDQTY,CPOD_RATE,CPOD_UOM_CODE,0 AS SPOM_POST from CUSTPO_MASTER,CUSTPO_DETAIL where CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            }
            if (dt.Rows.Count > 0)
            {
                txtPendingQty.Text = string.Format("{0:0.000}", (Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()) - inwardstock));
                txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["CPOD_RATE"].ToString()));
                ddlUom.SelectedValue = dt.Rows[0]["CPOD_UOM_CODE"].ToString();
            }
            else
            {
                txtPendingQty.Text = Convert.ToInt32(0).ToString();
            }
            //pendingQty();
            //dt = CommonClasses.Execute("select (ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_DISPACH,0)) as PENDQTY, CPOD_RATE AS SPOD_RATE,CPOD_UOM_CODE AS SPOD_UOM_CODE,0 AS SPOM_POST   from CUSTPO_MASTER,CUSTPO_DETAIL where CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CODE=CPOD_CPOM_CODE   AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            //if (dt.Rows.Count > 0)
            //{
            //    if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
            //    {
            //        txtChallanQty.Enabled = false;
            //        txtRecdQty.Enabled = false;
            //        PanelMsg.Visible = true;
            //        lblmsg.Text = "Only Post PO Can InWard";
            //    }
            //    else
            //    {
            //        PanelMsg.Visible = false;
            //        txtChallanQty.Enabled = true;
            //        txtRecdQty.Enabled = true;
            //    }
            //    txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
            //    txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["SPOD_RATE"].ToString()));
            //    ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();
            //}
            //else
            //{
            //    txtPendingQty.Text = Convert.ToInt32(0).ToString();
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    protected void ddlInwardType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BlankGrid();
            ddlSupplier_SelectedIndexChanged(null, null);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            int id = Convert.ToInt32(ddlSupplier.SelectedValue);
            ddlItemName.Items.Clear();
            ddlItemCode.Items.Clear();
            DataTable dtItem = new DataTable();
            if (ddlInwardType.SelectedValue == "0")
            {
                dtItem = CommonClasses.Execute(" SELECT DISTINCT  I_CODE,I_CODENO,I_NAME FROM BOM_MASTER,BOM_DETAIL,ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER  where BM_CODE=BD_BM_CODE AND BOM_MASTER.ES_DELETE=0 AND BD_I_CODE=I_CODE AND CUSTPO_MASTER.ES_DELETE=0 and BM_I_CODE=CPOD_I_CODE   AND   CPOM_CODE=CPOD_CPOM_CODE AND  ((CPOD_DISPACH) < (CPOD_ORD_QTY) OR CPOD_ORD_QTY=0 )  AND CPOM_TYPE=-2147483648 and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   AND CPOM_P_CODE=" + id + " ORDER BY I_NAME");
            }
            else
            {
                dtItem = CommonClasses.Execute(" SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER WHERE CUSTPO_MASTER.ES_DELETE=0 and I_CODE=CPOD_I_CODE   AND   CPOM_CODE=CPOD_CPOM_CODE AND  ((CPOD_DISPACH) < (CPOD_ORD_QTY) OR CPOD_ORD_QTY=0 )   and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'AND CPOM_TYPE=-2147483648    AND CPOD_STATUS=0  AND CPOM_P_CODE=" + id + " ORDER BY I_NAME");
            }
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
            DataTable dtIStock = new DataTable();
            if (ddlInwardType.SelectedValue == "0")
            {
                dtPO = CommonClasses.Execute("SELECT CPOM_CODE,Convert(varchar,CPOM_PONO)+' | '+ convert(varchar,CPOM_DATE,106) as  CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND CPOD_I_CODE=I_CODE AND CPOD_I_CODE IN (  SELECT BM_I_CODE FROM BOM_MASTER,BOM_DETAIL where BM_CODE=BD_BM_CODE  AND BD_I_CODE='" + ddlItemCode.SelectedValue + "') AND CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND  CUSTPO_MASTER.ES_DELETE=0      order by CPOM_CODE    ");
            }
            else
            {
                dtPO = CommonClasses.Execute("SELECT CPOM_CODE,Convert(varchar,CPOM_PONO)+' | '+ convert(varchar,CPOM_DATE,106) as  CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND CPOD_I_CODE=I_CODE AND CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND  CUSTPO_MASTER.ES_DELETE=0      order by CPOM_CODE    ");
            }
            ddlPoNumber.DataSource = dtPO;
            ddlPoNumber.DataTextField = "CPOM_PONO";
            ddlPoNumber.DataValueField = "CPOM_CODE";
            ddlPoNumber.DataBind();
            double inwardstock = 0;
            if (ddlInwardType.SelectedValue == "0")
            {
                //dt = CommonClasses.Execute(" SELECT  SUM(( BD_VQTY +BD_SCRAPQTY) *(CPOD_ORD_QTY-CPOD_DISPACH)) AS PENDQTY,I_INV_RATE AS CPOD_RATE,I_UOM_CODE AS CPOD_UOM_CODE,0 AS SPOM_POST  FROM BOM_MASTER,BOM_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER   where BM_CODE=BD_BM_CODE   AND CPOM_CODE=CPOD_CPOM_CODE AND BD_I_CODE=I_CODE AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'    AND  CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND BM_I_CODE=CPOD_I_CODE AND BD_I_CODE='" + ddlItemName.SelectedValue + "' ");
                dt = CommonClasses.Execute(" SELECT  SUM(( BD_VQTY +BD_SCRAPQTY) *(CPOD_ORD_QTY-CPOD_DISPACH)) AS PENDQTY,I_INV_RATE AS CPOD_RATE,I_UOM_CODE AS CPOD_UOM_CODE,0 AS SPOM_POST  FROM BOM_MASTER,BOM_DETAIL,CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER   where BM_CODE=BD_BM_CODE   AND CPOM_CODE=CPOD_CPOM_CODE AND BD_I_CODE=I_CODE AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'    AND  CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND BM_I_CODE=CPOD_I_CODE AND BD_I_CODE='" + ddlItemName.SelectedValue + "'   GROUP BY I_INV_RATE,I_UOM_CODE");
                dtIStock = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0)  AS IWD_REV_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_TYPE='IWIFP' AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND IWD_CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND IWD_I_CODE='" + ddlItemName.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0");

                // dtIStock = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0)  AS IWD_REV_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_TYPE='IWIFP' AND IWM_CODE=IWD_IWM_CODE AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "' AND IWD_CPOM_CODE='" + ddlPoNumber.SelectedValue + "' AND IWD_I_CODE='" + ddlItemName.SelectedValue + "' AND INWARD_MASTER.ES_DELETE=0  GROUP BY I_INV_RATE,I_UOM_CODE");
                if (dtIStock.Rows.Count > 0)
                {
                    inwardstock = Convert.ToDouble(dtIStock.Rows[0]["IWD_REV_QTY"].ToString());
                }
            }
            else
            {
                dt = CommonClasses.Execute(" select (ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_DISPACH,0)) as PENDQTY,CPOD_RATE,CPOD_UOM_CODE,0 AS SPOM_POST from CUSTPO_MASTER,CUSTPO_DETAIL where CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE='" + ddlSupplier.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
            }
            if (dt.Rows.Count > 0)
            {
                txtPendingQty.Text = string.Format("{0:0.000}", (Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()) - inwardstock));
                txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["CPOD_RATE"].ToString()));
                ddlUom.SelectedValue = dt.Rows[0]["CPOD_UOM_CODE"].ToString();
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
                Response.Redirect("~/Transactions/VIEW/ViewMaterialInward.aspx", false);
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
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
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

    protected void txtChallanQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtChallanQty.Text);
        txtChallanQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtRecdQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRecdQty.Text);
        txtRecdQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
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
}
