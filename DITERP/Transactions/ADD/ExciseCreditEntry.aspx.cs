using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_ADD_ExciseCreditEntry : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    ExciseCreditEntry_BL ExEntry_BL = null;
    static int mlCode = 0;
    static string right = "";
    public static string str = "";
    static DataTable dt2 = new DataTable();
    DataTable dtBillPassing = new DataTable();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Excise1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='105'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ExEntry_BL = new ExciseCreditEntry_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        LoadCombos();
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ExEntry_BL = new ExciseCreditEntry_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        LoadCombos();
                        ViewRec("MOD");
                        ddlSupplierName.Enabled = false;
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtBillDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtInvoiceDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        LoadCombos();
                        dt2.Rows.Clear();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        LoadFilter();
                    }
                    txtGatePassDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                    txtInvoiceDate.Attributes.Add("readonly", "readonly");
                    txtGatePassDate.Attributes.Add("readonly", "readonly");
                    ddlSupplierName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("EXICSE ENTRY", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region Event

    #region GirdEvent
    protected void dgBillPassing_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBillPassing.PageIndex = e.NewPageIndex;
            LoadBill();
        }
        catch (Exception)
        {
        }
    }

    protected void dgBillPassing_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion

    #region ButtonEvent

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect("~/Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE_ENTRY", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
                int flag = 0;
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        flag++;
                    }
                }
                if (flag == 0)
                {
                    ShowMessage("#Avisos", "Please Select Item", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (Convert.ToDouble(txtGrandTotal.Text) <= 0)
                {
                    ShowMessage("#Avisos", "Please Select Item", CommonClasses.MSG_Warning);
                    return;
                }
                if (ddlCenvatType.SelectedValue == "0")
                {
                    ShowMessage("#Avisos", "Please Select Cenvat Credit Type", CommonClasses.MSG_Warning);
                    return;
                }
                if (txtInvoceNo.Text == "")
                {
                    ShowMessage("#Avisos", "Please Select Invoice Number", CommonClasses.MSG_Warning);
                    return;
                }
                if (SaveRec())
                {
                }
                ddlSupplierName.Focus();
            }
            else
            {
                ShowMessage("#Avisos", "You have no rights to Save", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "btnSubmit_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region Cancel Record
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("EXICSE_ENTRY", "MODIFY", "EX_CODE", mlCode);
            }
            Response.Redirect("~/Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE_ENTRY", "CancelRecord", ex.Message);
        }
    }
    #endregion

    #region btnOK_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE_ENTRY", "btnOK_Click", ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region Check Validation
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtBillDate.Text == "")
            {
                flag = false;
            }
            else if (ddlSupplierName.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtInvoceNo.Text == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE_ENTRY", "CheckValid", ex.Message);
        }
        return flag;
    }
    #endregion
    #endregion

    protected void chkUseCustRej_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            LoadCombos();
        }
        catch (Exception)
        {

        }
    }

    #region chkSelect_CheckedChanged
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;
            Calculate(index);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region ddlSupplierName_SelectedIndexChanged
    protected void ddlSupplierName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlSupplierName.SelectedIndex != 0)
            //{
            //    LoadBill();
            //}
            //else if (ddlSupplierName.SelectedIndex == 0)
            //{
            //    dtFilter.Clear();
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWD_RATE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("NET_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("EXC_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("EDU_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("HEDU_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("T_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CODE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_GP_RATE", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("GP_AMT", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("ECPG", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SECG", typeof(String)));
            //    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String)));

            //    dtFilter.Rows.Add(dtFilter.NewRow());
            //    dgBillPassing.DataSource = dtFilter;
            //    dgBillPassing.DataBind();
            //    dgBillPassing.Enabled = false;
            //}
            LoadGINNo();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Supplier PO Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion
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

    #region User Defined Method

    #region Enabale
    public static void EnabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = true;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = true;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = true;
                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = true;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        EnabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region Diabale
    public static void DiabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = false;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = false;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = false;
                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = false;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        DiabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("SELECT * FROM EXICSE_ENTRY,PARTY_MASTER WHERE EX_P_CODE=P_CODE and EX_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and EX_CODE='" + mlCode + "'");
            if (dt.Rows.Count > 0)
            {
                chkUseCustRej.Checked = Convert.ToBoolean(dt.Rows[0]["EX_IS_CUSTREJ"]);
                LoadCombos();
                mlCode = Convert.ToInt32(dt.Rows[0]["EX_CODE"]);

                if (dt.Rows[0]["EX_TYPE"].ToString() == "Capital")
                {
                    ddlCenvatType.SelectedIndex = 1;
                }
                if (dt.Rows[0]["EX_TYPE"].ToString() == "Input")
                {
                    ddlCenvatType.SelectedIndex = 2;
                }
                if (dt.Rows[0]["EX_TYPE"].ToString() == "Service")
                {
                    ddlCenvatType.SelectedIndex = 3;
                }
                txtBillNo.Text = Convert.ToInt32(dt.Rows[0]["EX_NO"]).ToString();
                txtBillDate.Text = Convert.ToDateTime(dt.Rows[0]["EX_DATE"]).ToString("dd MMM yyyy");
                txtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["EX_INV_DATE"]).ToString("dd MMM yyyy");
                ddlSupplierName.SelectedValue = Convert.ToInt32(dt.Rows[0]["EX_P_CODE"]).ToString();
                txtInvoceNo.Text = (dt.Rows[0]["EX_INV_NO"]).ToString();
                txtBasicAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_BASIC_AMT"]));
                txtsheper.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_S_HCESS"]));
                txteducessper.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_S_CESS"]));
                txtexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_S_DUTY"]));
                txtExciseAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_EXCIES_AMT"]));
                txtEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_ECESS_AMT"]));
                txtSHEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_HECESS_AMT"]));
                txtsalestaxamt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_TAX_AMT"]));
                txtDiscountAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_DISCOUNT_AMT"]));
                txtAccesableValue.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_NET_AMT"]));
                txtFreightAmt1.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_FREIGHT"]));
                txtPackingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_PACKING_AMT"]));
                txtOctroiAmt2.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_OCTRO_AMT"]));
                txtGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_G_AMT"]));
                txtGatePassNo.Text = dt.Rows[0]["EX_DOC_NO"].ToString();
                txtGatePassDate.Text = Convert.ToDateTime(dt.Rows[0]["EX_DOC_DATE"]).ToString("dd MMM yyyy");
                txtExamt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_EX_DUTY"]));
                txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_TRANSPORT_AMT"]));
                txtOtherCharges.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_OTHER_AMT"]));
                txtInsuranceAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_INSURANCE_AMT"]));
                txtEDU.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_EX_CESS"]));
                txtEDUC.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_EX_HCESS"]));
                txtloadingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_ADDDUTY"]));
                txtGatePassNo.Text = dt.Rows[0]["EX_GATE_NO"].ToString();
                txtGatePassDate.Text = Convert.ToDateTime(dt.Rows[0]["EX_GATE_DATE"]).ToString("dd MMM yyyy");
                txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
                txtTaxPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["EX_T_PER"]));
                ddlSupplierName.Enabled = false;
                if (chkUseCustRej.Checked)
                {
                    dtBillPassing = CommonClasses.Execute("select distinct 'CUSTOMER-REJECTION' as IWM_TYPE,CR_CODE as IWM_CODE,CD_PO_CODE as IWD_CPOM_CODE,CPOM_PONO as SPOM_PO_NO,CR_GIN_NO as IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,CR_CHALLAN_NO as IWM_CHALLAN_NO,convert(varchar,CR_CHALLAN_DATE,106) as IWM_CHAL_DATE,CD_I_CODE as IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,cast(CD_CHALLAN_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(CD_RECEIVED_QTY as numeric(10,3)) as IWD_REV_QTY,cast(CD_CHALLAN_QTY as numeric(10,3)) as IWD_CON_OK_QTY,CONVERT(decimal(10,2),CD_RATE) AS IWD_RATE,CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))AS NET_AMT,cast(CD_RATE as numeric(20,2)) as SPOD_RATE,cast(CD_CHALLAN_QTY *CPOD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,'0.00' as SPOD_DISC_AMT,CASE WHEN CM_STATE=P_SM_CODE then E_BASIC else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_BASIC*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 )else 0 END as EXC_AMT,CASE WHEN CM_STATE=P_SM_CODE then E_EDU_CESS else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_EDU_CESS*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE)) /100) ELSE 0 END as EDU_AMT ,CASE WHEN CM_STATE!=P_SM_CODE then E_H_EDU else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),E_H_EDU*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 ) ELSE 0 END   AS HEDU_AMT,'' as SPOD_T_CODE,'' as ST_TAX_NAME,'' AS ST_SALES_TAX,'0.00' AS T_AMT,CPOM_CODE as SPOM_CODE,CR_CODE as IWM_CODE,'0.00' AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,'1' as SPOD_EXC_Y_N FROM CUSTPO_MASTER,CUSTPO_DETAIL,CUSTREJECTION_MASTER, CUSTREJECTION_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,EXICSE_ENTRY,EXCISE_DETAIL,EXCISE_TARIFF_MASTER ,PARTY_MASTER,COMPANY_MASTER where CD_I_CODE=CPOD_I_CODE and I_E_CODE=E_CODE and CPOM_CODE =CPOD_CPOM_CODE AND CR_CODE = CD_CR_CODE AND CD_I_CODE = I_CODE AND  CD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND EXD_SPOM_CODE=CPOM_CODE  and EXD_SPOM_CODE=CD_PO_CODE AND CD_MODVAT_FLG = 1 AND  CUSTREJECTION_MASTER.ES_DELETE = 0 AND CPOM_P_CODE=P_CODE AND CR_CM_CODE=CM_CODE AND  EXD_IWM_CODE=CR_CODE and EXD_EX_CODE=EX_CODE and CD_I_CODE=EXD_I_CODE  and EXICSE_ENTRY.ES_DELETE=0 and   EX_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and EXD_EX_CODE='" + mlCode + "' and EX_NO='" + txtBillNo.Text.Trim() + "' ORDER BY CUSTREJECTION_MASTER.CR_GIN_NO");
                }
                else
                {
                    dtBillPassing = CommonClasses.Execute(" select distinct CASE   IWM_TYPE  WHEN 'IWIM' THEN 'Supplier' WHEN 'OUTCUSTINV' THEN 'Sub-Contractor' WHEN 'IWIC' THEN 'CUSTOMER-REJECTION' END AS IWM_TYPE,IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,CONVERT(decimal(10,2),IWD_RATE) AS IWD_RATE,CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE))AS NET_AMT,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(SPOD_RATE as numeric(20,2)) as SPOD_RATE,cast(IWD_CH_QTY*SPOD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT,CASE WHEN CM_STATE=P_SM_CODE then SPOD_EXC_PER else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),SPOD_EXC_PER*CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE))/100 )else 0 END as EXC_AMT,CASE WHEN CM_STATE=P_SM_CODE then SPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),SPOD_EDU_CESS_PER*CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE)) /100) ELSE 0 END as EDU_AMT ,CASE WHEN CM_STATE!=P_SM_CODE then SPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),SPOD_H_EDU_CESS*CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE))/100 ) ELSE 0 END   AS HEDU_AMT,SPOD_T_CODE,'' ST_TAX_NAME,0 AS ST_SALES_TAX,'0.00' AS T_AMT,SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),ISNULL(SPOD_GP_RATE,0)) AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,SPOD_EXC_Y_N FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,EXICSE_ENTRY,EXCISE_DETAIL,PARTY_MASTER,COMPANY_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND EXD_SPOM_CODE=SPOM_CODE  and EXD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE  AND IWM_P_CODE=P_CODE AND IWM_CM_CODE=CM_CODE  AND IWD_MODVAT_FLG = 1 AND IWD_INSP_FLG = 1 AND (IWM_TYPE='IWIM' OR IWM_TYPE='OUTCUSTINV') AND INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' and EXD_IWM_CODE=IWM_CODE and EXD_EX_CODE=EX_CODE and EXICSE_ENTRY.ES_DELETE=0 and EX_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and EXD_EX_CODE='" + mlCode + "' and EX_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");
                }
                if (dtBillPassing.Rows.Count != 0)
                {
                    dgBillPassing.DataSource = dtBillPassing;
                    dgBillPassing.DataBind();
                    dt2 = dtBillPassing;
                    for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        chkRow.Checked = true;
                    }
                }
                if (Convert.ToDouble(txtsheper.Text) == 0)
                {
                    txtsheper.Enabled = false;
                    txtSHEduCessAmt.Enabled = false;
                    txtEDUC.Enabled = false;
                    txtexcper.Enabled = true;
                    txtExciseAmount.Enabled = true;
                    txtExamt.Enabled = true;
                    txteducessper.Enabled = true;
                    txtEduCessAmt.Enabled = true;
                    txtEDU.Enabled = true;
                }
                if (Convert.ToDouble(txtexcper.Text) == 0)
                {
                    txtsheper.Enabled = true;
                    txtSHEduCessAmt.Enabled = true;
                    txtEDUC.Enabled = true;
                    txtexcper.Enabled = false;
                    txtExciseAmount.Enabled = false;
                    txtExamt.Enabled = false;
                    txteducessper.Enabled = false;
                    txtEduCessAmt.Enabled = false;
                    txtEDU.Enabled = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("EXICSE_ENTRY", "MODIFY", "EX_CODE", mlCode);
            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "ViewRec", Ex.Message);
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
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "GetValues", ex.Message);
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
            ExEntry_BL = new ExciseCreditEntry_BL();
            ExEntry_BL.EX_CM_CODE = Convert.ToInt32(Session["CompanyCode"].ToString());
            ExEntry_BL.EX_TYPE = ddlCenvatType.SelectedItem.ToString();
            ExEntry_BL.EX_IND = "Credit";
            ExEntry_BL.EX_INV_NO = txtInvoceNo.Text.Trim();
            ExEntry_BL.EX_INV_DATE = Convert.ToDateTime(txtInvoiceDate.Text).ToString("dd/MMM/yyyy");
            ExEntry_BL.EX_NO = txtBillNo.Text.Trim();
            ExEntry_BL.EX_DATE = Convert.ToDateTime(txtBillDate.Text).ToString("dd/MMM/yyyy");
            ExEntry_BL.EX_DOC_TYPE = Type;
            ExEntry_BL.EX_P_CODE = Convert.ToInt32(ddlSupplierName.SelectedValue);
            DataTable dtCat = CommonClasses.Execute("SELECT case when P_CATEGORY = 0 then 'A' when P_CATEGORY = 1 then 'B' when P_CATEGORY = 2 then 'C' when P_CATEGORY = 3 then 'D' when P_CATEGORY = 4 then 'E' when P_CATEGORY = 5 then 'G'  end as P_CATEGORY FROM PARTY_MASTER  WHERE P_CODE='" + ddlSupplierName.SelectedValue + "'");
            ExEntry_BL.EX_SUPP_TYPE = dtCat.Rows[0]["P_CATEGORY"].ToString();
            ExEntry_BL.EX_BASIC_AMT = txtBasicAmount.Text;
            ExEntry_BL.EX_EXCIES_AMT = txtExciseAmount.Text;
            ExEntry_BL.EX_ECESS_AMT = txtEduCessAmt.Text;
            ExEntry_BL.EX_HECESS_AMT = txtSHEduCessAmt.Text;
            ExEntry_BL.EX_TAX_AMT = txtsalestaxamt.Text;
            ExEntry_BL.EX_DISCOUNT_AMT = txtDiscountAmt.Text;
            ExEntry_BL.EX_NET_AMT = txtAccesableValue.Text;
            ExEntry_BL.EX_FREIGHT = txtFreightAmt1.Text;
            ExEntry_BL.EX_PACKING_AMT = txtPackingAmt.Text;
            ExEntry_BL.EX_INSURANCE_AMT = txtInsuranceAmt.Text;
            ExEntry_BL.EX_OTHER_AMT = txtOtherCharges.Text;
            ExEntry_BL.EX_TRANSPORT_AMT = txtTransportAmt.Text;
            ExEntry_BL.EX_OCTRO_AMT = txtOctroiAmt2.Text;
            ExEntry_BL.EX_G_AMT = txtGrandTotal.Text;
            ExEntry_BL.EX_DOC_NO = txtGatePassNo.Text;
            ExEntry_BL.EX_DOC_DATE = Convert.ToDateTime(txtGatePassDate.Text).ToString("dd/MMM/yyyy");
            ExEntry_BL.EX_BANK_AMT = txtGrandTotal.Text;
            ExEntry_BL.EX_EX_DUTY = txtExamt.Text;
            ExEntry_BL.EX_EX_CESS = txtEDU.Text;
            ExEntry_BL.EX_EX_HCESS = txtEDUC.Text;
            ExEntry_BL.EX_ADDDUTY = txtloadingAmt.Text;
            ExEntry_BL.EX_GATE_NO = txtGatePassNo.Text;
            ExEntry_BL.EX_GATE_DATE = Convert.ToDateTime(txtGatePassDate.Text).ToString("dd/MMM/yyyy");
            ExEntry_BL.EX_S_DUTY = txtexcper.Text;
            ExEntry_BL.EX_S_CESS = txteducessper.Text;
            ExEntry_BL.EX_S_HCESS = txtsheper.Text;
            if (chkUseCustRej.Checked)
            {
                ExEntry_BL.EX_IS_CUSTREJ = "1";
                ExEntry_BL.EX_T_PER = txtTaxPer.Text;
            }
            else
            {
                ExEntry_BL.EX_IS_CUSTREJ = "0";
                ExEntry_BL.EX_T_PER = "0";
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    string Type = "";

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt3 = CommonClasses.Execute("SELECT * FROM EXICSE_ENTRY WHERE ES_DELETE=0 AND EX_INV_NO='" + txtInvoceNo.Text.Trim() + "' AND EX_P_CODE='" + ddlSupplierName.SelectedValue + "' AND EX_CM_CODE=" + Session["CompanyCode"] + "");
                if (dt3.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Invoice No allready Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        Type = ((Label)dgBillPassing.Rows[i].FindControl("lblGinType")).Text;
                        if (Type == "Supplier")
                        {
                            Type = "Invoice";
                        }
                        else
                        {
                            Type = "offloading";
                        }
                        break;
                    }
                }
                ExEntry_BL = new ExciseCreditEntry_BL();
                txtBillNo.Text = Numbering();
                if (Setvalues())
                {
                    if (ExEntry_BL.Save(dgBillPassing, "INSERT"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(EX_CODE) FROM EXICSE_ENTRY");
                        CommonClasses.WriteLog("EXICSE ENTRY", "Save", "EXICSE ENTRY", ExEntry_BL.EX_NO.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
                    }
                    else
                    {
                        ddlSupplierName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt3 = CommonClasses.Execute("SELECT * FROM EXICSE_ENTRY WHERE ES_DELETE=0 AND EX_INV_NO='" + txtInvoceNo.Text.Trim() + "'  AND EX_P_CODE='" + ddlSupplierName.SelectedValue + "'  AND EX_CM_CODE=" + Session["CompanyCode"] + " AND EX_CODE !='" + mlCode + "'");
                if (dt3.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Invoice No already Exist", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return false;
                }
                if (Setvalues())
                {
                    ExEntry_BL.EX_CODE = mlCode;
                    if (ExEntry_BL.Save(dgBillPassing, "UPDATE"))
                    {
                        CommonClasses.RemoveModifyLock("EXICSE_ENTRY", "MODIFY", "EX_CODE", mlCode);
                        CommonClasses.WriteLog("EXICSE ENTRY", "Update", "EXICSE ENTRY", ExEntry_BL.EX_NO.ToString(), Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewExciseCreditEntry.aspx", false);
                    }
                    else
                    {
                        ddlSupplierName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(EX_NO) as EX_NO from EXICSE_ENTRY WHERE   EX_TYPE='" + ddlCenvatType.SelectedItem.ToString() + "'  AND EX_DOC_TYPE='" + Type + "' AND EX_CM_CODE=" + Session["CompanyCode"] + " AND  ES_DELETE=0");
        if (dt.Rows[0]["EX_NO"] == null || dt.Rows[0]["EX_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["EX_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region ClearFunction
    public static void ClearTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Text = String.Empty;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.SelectedIndex = 0;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t = ctrl as CheckBox;

                    if (t != null)
                    {
                        t.Checked = false;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region Calucalte
    void Calculate(int index)
    {
        try
        {
            string totalStr = "";
            if (index != -1)
            {
                double BasicAmt = 0;
                double DiscountAmt = 0;
                double acceablevalue = 0;
                double ExcPer = 0;
                double ExcEduCessPer = 0;
                double ExcHighPer = 0;
                double ExcAmt = 0;
                double ExcEduCessAmt = 0;
                double ExcHighAmt = 0;
                double Taxper = 0;
                double TaxAmt = 0;
                double TaxableAmt = 0, OtherAmt = 0, AddAmt = 0, FrightAmt = 0, InsurranceAmt = 0, TransportAmt = 0, OctriPer = 0;
                double OctriAmt = 0, RoundOff = 0, GrandAmt = 0;
                bool flag = false; int count = 0;
                for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);
                        ExcAmt = ExcAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEXC_AMT"))).Text);
                        ExcEduCessAmt = ExcEduCessAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEDU_AMT"))).Text);
                        ExcHighAmt = ExcHighAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblHEDU_AMT"))).Text);
                        ExcPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_PER"))).Text);
                        ExcEduCessPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EDU_CESS_PER"))).Text);
                        ExcHighPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_H_EDU_CESS"))).Text);
                        if (count == 0 && flag == false)
                        {
                            count = 1;
                            flag = true;
                        }
                    }
                }
                if (BasicAmt > 0)
                {
                    txtBasicAmount.Text = string.Format("{0:0.00}", Math.Round(BasicAmt, 2));
                    txtDiscountAmt.Text = string.Format("{0:0.00}", Math.Round(DiscountAmt, 2));
                    txtPackingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackingAmt.Text), 2));
                    acceablevalue = BasicAmt - DiscountAmt + Convert.ToDouble(txtPackingAmt.Text);
                    txtAccesableValue.Text = string.Format("{0:0.00}", acceablevalue);
                    totalStr = DecimalMasking(txtOtherCharges.Text);
                    txtOtherCharges.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    OtherAmt = Convert.ToDouble(txtOtherCharges.Text);
                    totalStr = DecimalMasking(txtloadingAmt.Text);
                    txtloadingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    AddAmt = Convert.ToDouble(txtloadingAmt.Text);
                    totalStr = DecimalMasking(txtFreightAmt1.Text);
                    txtFreightAmt1.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    FrightAmt = Convert.ToDouble(txtFreightAmt1.Text);
                    totalStr = DecimalMasking(txtTransportAmt.Text);
                    txtTransportAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    TransportAmt = Convert.ToDouble(txtTransportAmt.Text);
                    totalStr = DecimalMasking(txtInsuranceAmt.Text);
                    txtInsuranceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    InsurranceAmt = Convert.ToDouble(txtInsuranceAmt.Text);
                    totalStr = DecimalMasking(txtOctroiAmt2.Text);
                    txtOctroiAmt2.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    OctriAmt = Convert.ToDouble(txtOctroiAmt2.Text);
                    double gston = 0;
                    gston = acceablevalue + TaxableAmt + TaxAmt + OtherAmt + AddAmt + FrightAmt + InsurranceAmt + TransportAmt + OctriAmt;
                    TaxableAmt = gston;
                    txtTaxableAmt.Text = string.Format("{0:0.00}", TaxableAmt);
                    txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
                    TaxAmt = Math.Round((TaxableAmt * Taxper / 100), 2);
                    txtexcper.Text = string.Format("{0:0.00}", ExcPer);
                    txteducessper.Text = string.Format("{0:0.00}", ExcEduCessPer);
                    txtsheper.Text = string.Format("{0:0.00}", ExcHighPer);
                    txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                    txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                    txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                    txtExamt.Text = string.Format("{0:0.00}", ExcAmt);
                    txtEDU.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                    txtEDUC.Text = string.Format("{0:0.00}", ExcHighAmt);

                    if (ExcHighPer == 0)
                    {
                        txtsheper.Enabled = false;
                        txtSHEduCessAmt.Enabled = false;
                        txtEDUC.Enabled = false;
                        txtexcper.Enabled = true;
                        txtExciseAmount.Enabled = true;
                        txtExamt.Enabled = true;
                        txteducessper.Enabled = true;
                        txtEduCessAmt.Enabled = true;
                        txtEDU.Enabled = true;
                    }

                    if (ExcPer == 0)
                    {
                        txtsheper.Enabled = true;
                        txtSHEduCessAmt.Enabled = true;
                        txtEDUC.Enabled = true;
                        txtexcper.Enabled = false;
                        txtExciseAmount.Enabled = false;
                        txtExamt.Enabled = false;
                        txteducessper.Enabled = false;
                        txtEduCessAmt.Enabled = false;
                        txtEDU.Enabled = false;
                    }
                    totalStr = DecimalMasking(txtRoundOff.Text);
                    txtRoundOff.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    RoundOff = Convert.ToDouble(txtRoundOff.Text);
                    txtTaxPer.Text = string.Format("{0:0.00}", Taxper);
                    txtsalestaxamt.Text = string.Format("{0:0.00}", TaxAmt);
                    GrandAmt = TaxableAmt + RoundOff + ExcAmt + ExcEduCessAmt + ExcHighAmt;
                    txtGrandTotal.Text = string.Format("{0:0.00}", GrandAmt);
                }
                else
                {
                    txtBasicAmount.Text = "0.00";
                    txtDiscountAmt.Text = "0.00";
                    txtPackingAmt.Text = "0.00";
                    txtAccesableValue.Text = "0.00";
                    txtExciseAmount.Text = "0.00";
                    txtEduCessAmt.Text = "0.00";
                    txtSHEduCessAmt.Text = "0.00";
                    txtTaxableAmt.Text = "0.00";
                    txtsalestaxamt.Text = "0.00";
                    txtTaxPer.Text = "0.00";
                    txtOtherCharges.Text = "0.00";
                    txtloadingAmt.Text = "0.00";
                    txtFreightAmt1.Text = "0.00";
                    txtInsuranceAmt.Text = "0.00";
                    txtOctroiAmt2.Text = "0.00";
                    txtRoundOff.Text = "0.00";
                    txtGrandTotal.Text = "0.00";
                    txtexcper.Text = "0.00";
                    txteducessper.Text = "0.00";
                    txtsheper.Text = "0.00";
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "Calculate", ex.Message.ToString());
        }
    }
    #endregion

    #region Calucalte
    void Calculate()
    {
        try
        {
            string totalStr = "";
            double BasicAmt = 0;
            double DiscountAmt = 0;
            double acceablevalue = 0;
            double ExcPer = 0;
            double ExcEduCessPer = 0;
            double ExcHighPer = 0;
            double ExcAmt = 0;
            double ExcEduCessAmt = 0;
            double ExcHighAmt = 0;
            double Taxper = 0;
            double TaxAmt = 0;
            double TaxableAmt = 0, OtherAmt = 0, AddAmt = 0, FrightAmt = 0, InsurranceAmt = 0, TransportAmt = 0, OctriPer = 0;
            double OctriAmt = 0, RoundOff = 0, GrandAmt = 0;
            bool flag = false; int count = 0;
            for (int i = 0; i < dgBillPassing.Rows.Count; i++)
            {
                CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                if (chkRow.Checked)
                {
                    BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);
                    ExcAmt = ExcAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEXC_AMT"))).Text);
                    ExcEduCessAmt = ExcEduCessAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblEDU_AMT"))).Text);
                    ExcHighAmt = ExcHighAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblHEDU_AMT"))).Text);
                    ExcPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_PER"))).Text);
                    ExcEduCessPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EDU_CESS_PER"))).Text);
                    ExcHighPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_H_EDU_CESS"))).Text);
                    if (count == 0 && flag == false)
                    {
                        count = 1;
                        flag = true;
                    }
                }
            }
            if (BasicAmt > 0)
            {
                txtBasicAmount.Text = string.Format("{0:0.00}", Math.Round(BasicAmt, 2));
                txtDiscountAmt.Text = string.Format("{0:0.00}", Math.Round(DiscountAmt, 2));
                txtPackingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackingAmt.Text), 2));
                acceablevalue = BasicAmt - DiscountAmt + Convert.ToDouble(txtPackingAmt.Text);
                txtAccesableValue.Text = string.Format("{0:0.00}", acceablevalue);
                totalStr = DecimalMasking(txtOtherCharges.Text);
                txtOtherCharges.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                OtherAmt = Convert.ToDouble(txtOtherCharges.Text);
                totalStr = DecimalMasking(txtloadingAmt.Text);
                txtloadingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                AddAmt = Convert.ToDouble(txtloadingAmt.Text);
                totalStr = DecimalMasking(txtFreightAmt1.Text);
                txtFreightAmt1.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                FrightAmt = Convert.ToDouble(txtFreightAmt1.Text);
                totalStr = DecimalMasking(txtTransportAmt.Text);
                txtTransportAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                TransportAmt = Convert.ToDouble(txtTransportAmt.Text);
                totalStr = DecimalMasking(txtInsuranceAmt.Text);
                txtInsuranceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                InsurranceAmt = Convert.ToDouble(txtInsuranceAmt.Text);
                totalStr = DecimalMasking(txtOctroiAmt2.Text);
                txtOctroiAmt2.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                OctriAmt = Convert.ToDouble(txtOctroiAmt2.Text);
                double gston = 0;
                gston = acceablevalue + TaxableAmt + TaxAmt + OtherAmt + AddAmt + FrightAmt + InsurranceAmt + TransportAmt + OctriAmt;
                TaxableAmt = gston;
                txtTaxableAmt.Text = string.Format("{0:0.00}", TaxableAmt);
                txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
                TaxAmt = Math.Round((TaxableAmt * Taxper / 100), 2);
                txtexcper.Text = string.Format("{0:0.00}", ExcPer);
                txteducessper.Text = string.Format("{0:0.00}", ExcEduCessPer);
                txtsheper.Text = string.Format("{0:0.00}", ExcHighPer);
                txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                txtExamt.Text = string.Format("{0:0.00}", ExcAmt);
                txtEDU.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                txtEDUC.Text = string.Format("{0:0.00}", ExcHighAmt);

                if (ExcHighPer == 0)
                {
                    txtsheper.Enabled = false;
                    txtSHEduCessAmt.Enabled = false;
                    txtEDUC.Enabled = false;
                    txtexcper.Enabled = true;
                    txtExciseAmount.Enabled = true;
                    txtExamt.Enabled = true;
                    txteducessper.Enabled = true;
                    txtEduCessAmt.Enabled = true;
                    txtEDU.Enabled = true;
                }

                if (ExcPer == 0)
                {
                    txtsheper.Enabled = true;
                    txtSHEduCessAmt.Enabled = true;
                    txtEDUC.Enabled = true;
                    txtexcper.Enabled = false;
                    txtExciseAmount.Enabled = false;
                    txtExamt.Enabled = false;
                    txteducessper.Enabled = false;
                    txtEduCessAmt.Enabled = false;
                    txtEDU.Enabled = false;
                }
                totalStr = DecimalMasking(txtRoundOff.Text);
                txtRoundOff.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                RoundOff = Convert.ToDouble(txtRoundOff.Text);
                txtTaxPer.Text = string.Format("{0:0.00}", Taxper);
                txtsalestaxamt.Text = string.Format("{0:0.00}", TaxAmt);
                GrandAmt = TaxableAmt + RoundOff + ExcAmt + ExcEduCessAmt + ExcHighAmt;
                txtGrandTotal.Text = string.Format("{0:0.00}", GrandAmt);
            }
            else
            {
                txtBasicAmount.Text = "0.00";
                txtDiscountAmt.Text = "0.00";
                txtPackingAmt.Text = "0.00";
                txtAccesableValue.Text = "0.00";
                txtExciseAmount.Text = "0.00";
                txtEduCessAmt.Text = "0.00";
                txtSHEduCessAmt.Text = "0.00";
                txtTaxableAmt.Text = "0.00";
                txtsalestaxamt.Text = "0.00";
                txtTaxPer.Text = "0.00";
                txtOtherCharges.Text = "0.00";
                txtloadingAmt.Text = "0.00";
                txtFreightAmt1.Text = "0.00";
                txtInsuranceAmt.Text = "0.00";
                txtOctroiAmt2.Text = "0.00";
                txtRoundOff.Text = "0.00";
                txtGrandTotal.Text = "0.00";
                txtexcper.Text = "0.00";
                txteducessper.Text = "0.00";
                txtsheper.Text = "0.00";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "Calculate", ex.Message.ToString());
        }
    }
    #endregion

    #region txtPackingAmt_TextChanged
    protected void txtPackingAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                string totalStr = DecimalMasking(txtPackingAmt.Text);
                txtPackingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtPackingAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscountAmt_TextChanged
    protected void txtDiscountAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtDiscountAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtOtherCharges_TextChanged
    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtOtherCharges_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtloadingAmt_TextChanged
    protected void txtloadingAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtloadingAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtFreightAmt1_TextChanged
    protected void txtFreightAmt1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtInsuranceAmt_TextChanged
    protected void txtInsuranceAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtInsuranceAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtTransportAmt_TextChanged
    protected void txtTransportAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtTransportAmt_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtOctroiAmt2_TextChanged
    protected void txtOctroiAmt2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtOctroiAmt2_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtRoundOff_TextChanged
    protected void txtRoundOff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtRoundOff_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscountAmt1_TextChanged
    protected void txtDiscountAmt1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtDiscountAmt1_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtDiscountAmt2_TextChanged
    protected void txtDiscountAmt2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (dgBillPassing.Enabled == true)
            {
                Calculate(1);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtDiscountAmt2_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region LoadBill
    private void LoadBill()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                if (ddlSupplierName.SelectedIndex != 0)
                {
                    if (chkUseCustRej.Checked)
                    {
                        dt = CommonClasses.Execute("SELECT CD_PO_CODE as IWD_CPOM_CODE,'CUSTOMER-REJECTION' AS IWM_TYPE , CPOM_PONO as SPOM_PO_NO,CR_GIN_NO as IWM_NO,CONVERT(VARCHAR,CR_GIN_DATE,106) AS IWM_DATE,CR_CHALLAN_NO as IWM_CHALLAN_NO,CONVERT(VARCHAR,CR_CHALLAN_DATE,106) AS  IWM_CHAL_DATE,CD_I_CODE as IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),CD_CHALLAN_QTY) AS IWD_CH_QTY,CONVERT(decimal(10,3),CD_RECEIVED_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),CD_CHALLAN_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),CD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))AS NET_AMT,'0.00' AS SPOD_DISC_AMT , CASE WHEN CM_STATE=P_SM_CODE then E_BASIC else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_BASIC*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 )else 0 END as EXC_AMT,CASE WHEN CM_STATE=P_SM_CODE then E_EDU_CESS else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),E_EDU_CESS*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE)) /100) ELSE 0 END as EDU_AMT ,CASE WHEN CM_STATE!=P_SM_CODE then E_H_EDU else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),E_H_EDU*CONVERT(decimal(10,2),(CD_CHALLAN_QTY *CPOD_RATE))/100 ) ELSE 0 END   AS HEDU_AMT,CPOD_ST_CODE as SPOD_T_CODE,' ' as ST_TAX_NAME, '' AS ST_SALES_TAX,'0.00' AS T_AMT,CPOM_CODE as SPOM_CODE,CR_CODE as IWM_CODE,'0.00' AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,'1' as SPOD_EXC_Y_N From CUSTREJECTION_MASTER, CUSTREJECTION_DETAIL, CUSTPO_MASTER, CUSTPO_DETAIL, ITEM_MASTER, ITEM_UNIT_MASTER, EXCISE_TARIFF_MASTER,PARTY_MASTER,COMPANY_MASTER WHERE CD_I_CODE=CPOD_I_CODE and I_E_CODE=E_CODE and CR_CODE = CD_CR_CODE AND CD_I_CODE = I_CODE AND CD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND CPOM_CODE =CPOD_CPOM_CODE AND CPOM_CODE = CD_PO_CODE AND CD_I_CODE = I_CODE AND CPOM_P_CODE=P_CODE AND CR_CM_CODE=CM_CODE    AND CD_MODVAT_FLG=0 AND CUSTREJECTION_MASTER.ES_DELETE = 0 AND CR_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyID"]) + "' AND CR_P_CODE='" + ddlSupplierName.SelectedValue + "' AND CR_CODE=" + ddlGINNo.SelectedValue + " ORDER BY CPOM_PONO,IWM_NO");
                    }
                    else
                    {
                        dt = CommonClasses.Execute("SELECT IWD_CPOM_CODE,CASE IWM_TYPE WHEN 'IWIM' THEN 'Supplier' WHEN 'OUTCUSTINV' THEN 'Sub-Contractor' WHEN 'IWIC' THEN 'CUSTOMER-REJECTION' END AS IWM_TYPE , SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106) AS IWM_DATE,IWM_CHALLAN_NO,CONVERT(VARCHAR,IWM_CHAL_DATE,106) AS  IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),IWD_CH_QTY) AS IWD_CH_QTY,CONVERT(decimal(10,3),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),IWD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE))AS NET_AMT,CONVERT(decimal(10,2),SPOD_DISC_AMT) AS SPOD_DISC_AMT ,CASE WHEN CM_STATE=P_SM_CODE then SPOD_EXC_PER else 0 END as SPOD_EXC_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),SPOD_EXC_PER*CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE))/100 )else 0 END as EXC_AMT,CASE WHEN CM_STATE=P_SM_CODE then SPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER,CASE WHEN CM_STATE=P_SM_CODE then CONVERT(decimal(10,2),SPOD_EDU_CESS_PER*CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE)) /100) ELSE 0 END as EDU_AMT ,CASE WHEN CM_STATE!=P_SM_CODE then SPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,CASE WHEN CM_STATE!=P_SM_CODE then CONVERT(decimal(10,2),SPOD_H_EDU_CESS*CONVERT(decimal(10,2),(IWD_CH_QTY *IWD_RATE))/100 ) ELSE 0 END   AS HEDU_AMT, SPOD_T_CODE,'' as ST_TAX_NAME, 0 AS ST_SALES_TAX,'0.00' AS T_AMT,  SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),ISNULL(SPOD_GP_RATE,0)) AS SPOD_GP_RATE,'0.00' as GP_AMT,'0.00' as ECPG,'0.00' as SECG,SPOD_EXC_Y_N  From INWARD_MASTER, INWARD_DETAIL, SUPP_PO_MASTER, SUPP_PO_DETAILS, ITEM_MASTER, ITEM_UNIT_MASTER,PARTY_MASTER,COMPANY_MASTER WHERE IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND SPOM_CODE =SPOD_SPOM_CODE AND SPOM_P_CODE=P_CODE  AND SPOM_CODE = IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE  AND IWD_MODVAT_FLG=0 AND IWM_CM_CODE=CM_CODE AND IWD_INSP_FLG = 1 AND (IWM_TYPE='IWIM' OR IWM_TYPE='OUTCUSTINV') AND IWM_INWARD_TYPE IN(0,1) AND IWD_MODVAT_FLG = 0 AND IWD_BILL_PASS_FLG=0  AND INWARD_MASTER.ES_DELETE = 0 AND IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND IWM_CODE=" + ddlGINNo.SelectedValue + " ORDER BY SPOM_PO_NO,IWM_NO");
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dgBillPassing.Enabled = true;
                        dgBillPassing.DataSource = dt;
                        dgBillPassing.DataBind();
                    }
                    else
                    {
                        dgBillPassing.DataSource = null;
                        dgBillPassing.DataBind();
                        LoadFilter();
                        PanelMsg.Visible = true;
                        lblmsg.Text = "This Supplier Bill Not Present";
                        LoadFilter();
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Taluka Master", "LoadDistrict", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillSupplier
        DataTable dt = new DataTable();
        try
        {
            if (chkUseCustRej.Checked)
            {
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dt = CommonClasses.FillCombo("CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER", "P_NAME", "P_CODE", "CR_CODE=CD_CR_CODE AND P_CODE=CR_P_CODE AND CR_CM_COMP_ID= " + Convert.ToInt32(Session["CompanyId"]) + " ORDER by P_NAME", ddlSupplierName);
                }
                else
                {
                    dt = CommonClasses.FillCombo("CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER", "P_NAME", "P_CODE", "CR_CODE=CD_CR_CODE AND P_CODE=CR_P_CODE AND CD_MODVAT_FLG=0 AND CR_CM_COMP_ID= " + Convert.ToInt32(Session["CompanyId"]) + " ORDER by P_NAME", ddlSupplierName);
                }
            }
            else
            {
                dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + "   AND IWD_IWM_CODE=IWM_CODE AND IWD_INSP_FLG='1' AND PARTY_MASTER.ES_DELETE='0' AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  ORDER by P_NAME", ddlSupplierName);
            }
            ddlSupplierName.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "FillSupplier", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgBillPassing.Rows.Count == 0)
        {
            dtFilter.Clear();
            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("NET_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EXC_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EDU_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("HEDU_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("T_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_GP_RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("GP_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ECPG", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SECG", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
            }
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from SUPP_PO_MASTERS where SPOM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY-ADD", "ModifyLog", Ex.Message);
        }
        return false;
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
            CommonClasses.SendError("EXICSE ENTRY - ADD", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion

    protected void txtexcper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
    }

    protected void txteducessper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
    }

    protected void txtsheper_TextChanged(object sender, EventArgs e)
    {
        try
        {
            grandtotal();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
    }

    public void grandtotal()
    {
        txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
        txtExciseAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txtexcper.Text) / 100);
        txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
        txtEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txteducessper.Text) / 100);
        txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
        txtSHEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txtsheper.Text) / 100);
        txtGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtExciseAmount.Text) + Convert.ToDouble(txtEduCessAmt.Text) + Convert.ToDouble(txtSHEduCessAmt.Text));
        txtExamt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtExciseAmount.Text));
        txtEDU.Text = string.Format("{0:0.00}", Convert.ToDouble(txtEduCessAmt.Text));
        txtEDUC.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSHEduCessAmt.Text));
    }

    protected void txtExciseAmount_TextChanged(object sender, EventArgs e)
    {
        grandtotal();
    }

    protected void txtEduCessAmt_TextChanged(object sender, EventArgs e)
    {
        grandtotal();
    }

    protected void txtSHEduCessAmt_TextChanged(object sender, EventArgs e)
    {
        grandtotal();
    }

    #region ddlGINNo_SelectedIndexChanged
    protected void ddlGINNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSupplierName.SelectedIndex != 0)
        {
            LoadBill();
        }
        else if (ddlSupplierName.SelectedIndex == 0)
        {
            dtFilter.Clear();
            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_RATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("NET_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("EXC_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("EDU_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("HEDU_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("T_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_GP_RATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("GP_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ECPG", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SECG", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgBillPassing.DataSource = dtFilter;
            dgBillPassing.DataBind();
            dgBillPassing.Enabled = false;
        }
    }
    #endregion

    //Bind GIN NO. to Dropdown
    #region LoadGINNo
    private void LoadGINNo()
    {
        DataTable dtGinNo = new DataTable();
        if (chkUseCustRej.Checked)
        {
            dtGinNo = CommonClasses.Execute("SELECT DISTINCT CR_GIN_NO as IWM_NO,CR_CODE as IWM_CODE From CUSTREJECTION_MASTER, CUSTREJECTION_DETAIL, CUSTPO_MASTER, CUSTPO_DETAIL, ITEM_MASTER, ITEM_UNIT_MASTER, EXCISE_TARIFF_MASTER,PARTY_MASTER,COMPANY_MASTER WHERE CD_I_CODE=CPOD_I_CODE and I_E_CODE=E_CODE and CR_CODE = CD_CR_CODE AND CD_I_CODE = I_CODE AND CD_UOM = ITEM_UNIT_MASTER.I_UOM_CODE AND CPOM_CODE =CPOD_CPOM_CODE AND CPOM_CODE = CD_PO_CODE AND CD_I_CODE = I_CODE AND CPOM_P_CODE=P_CODE AND CR_CM_CODE=CM_CODE AND CD_MODVAT_FLG=0 AND CUSTREJECTION_MASTER.ES_DELETE = 0 AND CR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND CR_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY IWM_NO");
        }
        else
        {
            dtGinNo = CommonClasses.Execute("SELECT DISTINCT IWM_NO,IWM_CODE From INWARD_MASTER, INWARD_DETAIL, SUPP_PO_MASTER, SUPP_PO_DETAILS, ITEM_MASTER, ITEM_UNIT_MASTER,PARTY_MASTER,COMPANY_MASTER WHERE IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND SPOM_CODE =SPOD_SPOM_CODE AND SPOM_P_CODE=P_CODE  AND SPOM_CODE = IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE  AND IWD_MODVAT_FLG=0 AND IWM_CM_CODE=CM_CODE AND IWD_INSP_FLG = 1 AND (IWM_TYPE='IWIM' OR IWM_TYPE='OUTCUSTINV') AND IWM_INWARD_TYPE IN(0,1) AND IWD_MODVAT_FLG = 0 AND IWD_BILL_PASS_FLG=0 AND INWARD_MASTER.ES_DELETE=0 AND IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY IWM_NO");
        }
        ddlGINNo.DataSource = dtGinNo;
        ddlGINNo.DataTextField = "IWM_NO";
        ddlGINNo.DataValueField = "IWM_CODE";
        ddlGINNo.DataBind();
        ddlGINNo.Items.Insert(0, new ListItem("Select GIN No.", "0"));
    }
    #endregion

    //All Checkbox: Select all record from gridview using this checkbox
    protected void chkAllRec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkAllRec.Checked == true)
            {
                if (dgBillPassing.Enabled == true)
                {
                    for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                    {
                        ((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))).Checked = true;
                    }
                }
            }
            else
            {
                if (dgBillPassing.Enabled == true)
                {
                    for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                    {
                        ((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))).Checked = false;
                    }
                }
            }
            Calculate();
        }
        catch (Exception)
        {

        }
    }
}
