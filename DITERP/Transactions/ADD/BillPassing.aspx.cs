using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_ADD_BillPassing : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    BillPassing_BL billPassing_BL = null;
    static int mlCode = 0;
    static string right = "";
    public static string str = "";
    public static string Type = "";
    static DataTable dt2 = new DataTable();
    DataTable dtBillPassing = new DataTable();
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
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
                ViewState["mlCode"] = mlCode;
                ViewState["right"] = right;
                ViewState["Type"] = Type;
                ViewState["dt2"] = dt2;
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='29'");
                ViewState["right"] = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        billPassing_BL = new BillPassing_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        LoadCombos();
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        billPassing_BL = new BillPassing_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        LoadCombos();
                        ViewRec("MOD");
                        // EnabaleTextBoxes(MainPanel);
                        ddlSupplierName.Enabled = false;
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtBillDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtInvoiceDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        LoadCombos();
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        LoadFilter();
                    }
                    txtBillDate.Attributes.Add("readonly", "readonly");
                    txtInvoiceDate.Attributes.Add("readonly", "readonly");
                    lnkPop.Enabled = true;
                    ddlSupplierName.Focus();
                    //rbLstIsExise.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Bill Passing", "PageLoad", ex.Message);
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

    #region rbLstIsExise_SelectedIndexChanged
    protected void rbLstIsExise_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSupplierName.SelectedIndex != 0)
            {
                LoadBill();
                lnkPop.Enabled = true;
            }
            else if (ddlSupplierName.SelectedIndex == 0)
            {
                dtFilter.Clear();
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_RATE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_TOTAL_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_DUTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_HCESS", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String)));
                #region Comment_Existing
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_RATE", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_TOTAL_AMT", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_DUTY", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));

                //dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
                //dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_Y_N", typeof(String))); 
                #endregion

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgBillPassing.DataSource = dtFilter;
                dgBillPassing.DataBind();
                dgBillPassing.Enabled = false;
                lnkPop.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Supplier PO Transaction", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                //CancelRecord();
                Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("BILL_PASSING_MASTER", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(ViewState["right"].ToString().Substring(0, 1)), this, "For Save"))
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
                if (ddlSupplierName.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Please Select Supplier", CommonClasses.MSG_Warning);
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
            CommonClasses.SendError("Bill Passing", "btnSubmit_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region Cancel Record
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("BILL_PASSING_MASTER", "MODIFY", "BPM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("BILL_PASSING_MASTER", "CancelRecord", ex.Message);
        }
    }
    #endregion

    #region btnOK_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec();
            CancelRecord();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("BILL_PASSING_MASTER", "btnOK_Click", ex.Message);
        }
    }

    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
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
            CommonClasses.SendError("BILL_PASSING_MASTER", "CheckValid", ex.Message);
        }
        return flag;
    }
    #endregion

    #endregion

    #region chkSelect_CheckedChanged
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;
            Calculate(index);
            txtAccesableValue.Focus();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region ddlSupplierName_SelectedIndexChanged
    protected void ddlSupplierName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //lblmsg.Visible = false;
            //PanelMsg.Visible = false;
            if (ddlSupplierName.SelectedIndex != 0)
            {
                LoadBill();
            }
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
            // int n = no - 1;
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

    #region grandtotal
    public void grandtotal()
    {
        txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtAccesableValue.Text) + Convert.ToDouble(txtloadingAmt.Text) + Convert.ToDouble(txtInsuranceAmt.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtFreightAmt1.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtOctroiAmt2.Text));
        txtExciseAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txtexcper.Text) / 100);
        txtEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txteducessper.Text) / 100);
        txtSHEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) * Convert.ToDouble(txtsheper.Text) / 100);

        txtGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtExciseAmount.Text) + Convert.ToDouble(txtEduCessAmt.Text) + Convert.ToDouble(txtSHEduCessAmt.Text) + Convert.ToDouble(txtRoundOff.Text));
        //txtExamt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtExciseAmount.Text));
        //txtEDU.Text = string.Format("{0:0.00}", Convert.ToDouble(txtEduCessAmt.Text));
        //txtEDUC.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSHEduCessAmt.Text));
    }
    #endregion grandtotal

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
            LoadCombos();
            dt = CommonClasses.Execute("select   CASE WHEN BPM_TYPE='OUTCUSTINV' then 'OUTCUSTINV' else 'IWIM' END IWM_TYPE ,BPM_CODE,BPM_NO,CONVERT(varchar,BPM_DATE,106) as BPM_DATE, ISNULL(BPM_EX_TYPE,0)  AS BPM_EX_TYPE,BPM_P_CODE,P_NAME,BPM_IWM_CODE,BPM_INV_NO,CONVERT(varchar,BPM_INV_DATE,106) as BPM_INV_DATE,BPM_BILL_PASS_BY,BPM_BASIC_AMT, BPM_DISCOUNT_AMT,	BPM_PACKING_AMT,BPM_ACCESS_AMT ,BPM_EXCIES_AMT ,BPM_ECESS_AMT,BPM_HECESS_AMT ,BPM_EXCPER ,BPM_EXCEDCESS_PER ,BPM_EXCHIEDU_PER ,BPM_TAXABLE_AMT ,BPM_TAX_AMT ,BPM_TAX_PER ,BPM_TAX_CODE ,BPM_OTHER_AMT ,BPM_ADD_DUTY ,BPM_FREIGHT ,BPM_INSURRANCE ,BPM_TRANSPORT ,isnull(BPM_OCTRO_AMT,0) as BPM_OCTRO_AMT,isnull(BPM_ROUND_OFF,0) as BPM_ROUND_OFF ,BPM_G_AMT   from BILL_PASSING_MASTER,PARTY_MASTER WHERE BPM_P_CODE=P_CODE and BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["BPM_CODE"]);
                txtBillNo.Text = Convert.ToInt32(dt.Rows[0]["BPM_NO"]).ToString();
                txtBillDate.Text = Convert.ToDateTime(dt.Rows[0]["BPM_DATE"]).ToString("dd MMM yyyy");
                txtInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["BPM_INV_DATE"]).ToString("dd MMM yyyy");
                ddlSupplierName.SelectedValue = Convert.ToInt32(dt.Rows[0]["BPM_P_CODE"]).ToString();
                txtInvoceNo.Text = (dt.Rows[0]["BPM_INV_NO"]).ToString();
                //ddlBillPass.SelectedItem.Text = (dt.Rows[0]["BPM_BILL_PASS_BY"]).ToString();


                rbLstIsExise.SelectedValue = dt.Rows[0]["BPM_EX_TYPE"].ToString();

                txtBasicAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_BASIC_AMT"]));
                txtDiscountAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_DISCOUNT_AMT"]));
                //txtNetAmt.Text = string.Format("{0:0.00}",Convert.ToDouble(dt.Rows[0]["BPM_NET_AMT"]));
                txtPackingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_PACKING_AMT"]));
                txtAccesableValue.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_ACCESS_AMT"]));

                txtExciseAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_EXCIES_AMT"]));
                txtEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_ECESS_AMT"]));
                txtSHEduCessAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_HECESS_AMT"]));

                txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_TAXABLE_AMT"]));

                txtTaxPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_TAX_PER"]));
                txtsalestaxamt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_TAX_AMT"]));
                txtOtherCharges.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_OTHER_AMT"]));
                txtloadingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_ADD_DUTY"]));

                txtFreightAmt1.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_FREIGHT"]));
                txtInsuranceAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_INSURRANCE"]));
                txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_TRANSPORT"]));

                txtOctroiAmt2.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_OCTRO_AMT"]));
                txtRoundOff.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_ROUND_OFF"]));
                txtGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_G_AMT"]));

                txtexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_EXCPER"]));
                txteducessper.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_EXCEDCESS_PER"]));
                txtsheper.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["BPM_EXCHIEDU_PER"]));
                rbLstIsExise.Enabled = false;
                ddlSupplierName.Enabled = false;
                //dtBillPassing = CommonClasses.Execute("select distinct IWM_TYPE AS IWM_TYPE,  IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,2)) as SPOD_RATE,cast(IWD_CH_QTY*IWD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_T_CODE,ST_TAX_NAME,ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY  FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,SALES_TAX_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND SPOD_T_CODE = ST_CODE AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "'  and BPD_IWM_CODE=IWM_CODE and BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");

                // dtBillPassing = CommonClasses.Execute("select distinct IWM_TYPE AS IWM_TYPE,  IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,2)) as SPOD_RATE,cast(IWD_CH_QTY*IWD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT,SPOD_EXC_PER,SPOD_EDU_CESS_PER,SPOD_H_EDU_CESS,SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE   AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "'  and BPD_IWM_CODE=IWM_CODE and BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");

                dtBillPassing = CommonClasses.Execute(" select distinct IWM_TYPE AS IWM_TYPE,  IWM_CODE,IWD_CPOM_CODE,SPOM_PO_NO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,IWD_I_CODE,I_NAME,cast(IWD_CH_QTY as numeric(10,3)) AS IWD_CH_QTY, cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,cast(IWD_CON_OK_QTY as numeric(10,3)) as IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_CON_SCRAP_QTY,cast(IWD_RATE as numeric(20,2)) as SPOD_RATE,cast(IWD_CH_QTY*IWD_RATE as numeric(20,2)) as SPOD_TOTAL_AMT,cast(SPOD_DISC_AMT as numeric(20,2)) as SPOD_DISC_AMT, CASE WHEN P_SM_CODE=CM_STATE then SPOD_EXC_PER else 0 END  AS SPOD_EXC_PER,CASE WHEN P_SM_CODE=CM_STATE then SPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER, CASE WHEN P_SM_CODE<>CM_STATE then SPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,SPOD_T_CODE,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N,ISNULL(BPD_EXC_AMT,0) AS EX_EX_DUTY ,ISNULL(BPD_EDU_AMT,0) AS EX_EX_CESS,ISNULL(BPD_HSEDU_AMT,0) AS EX_EX_HCESS FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,BILL_PASSING_MASTER,BILL_PASSING_DETAIL,PARTY_MASTER,COMPANY_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND BPD_SPOM_CODE=SPOM_CODE  and BPD_SPOM_CODE=IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE   AND IWD_BILL_PASS_FLG = 1 AND IWD_INSP_FLG = 1 AND  INWARD_MASTER.ES_DELETE = 0 AND      BPD_IWM_CODE=IWM_CODE and BPM_CM_CODE=CM_CODE AND P_CODE=BPM_P_CODE AND  BPD_BPM_CODE=BPM_CODE and BILL_PASSING_MASTER.ES_DELETE=0 and  BPM_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and BPD_BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' and BPM_NO='" + txtBillNo.Text + "' ORDER BY INWARD_MASTER.IWM_NO");

                if (dtBillPassing.Rows.Count != 0)
                {
                    dgBillPassing.DataSource = dtBillPassing;
                    dgBillPassing.DataBind();
                    ViewState["dt2"] = dtBillPassing;
                    for (int i = 0; i < dgBillPassing.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgBillPassing.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        chkRow.Checked = true;
                    }
                }


            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("BILL_PASSING_MASTER", "MODIFY", "BPM_CODE", Convert.ToInt32(ViewState["mlCode"]));

            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
            if (Convert.ToDouble(txtsheper.Text) == 0)
            {
                txtsheper.Enabled = false;
                txtSHEduCessAmt.Enabled = false;
                txtexcper.Enabled = true;
                txtExciseAmount.Enabled = true;
                txteducessper.Enabled = true;
                txtEduCessAmt.Enabled = true;

            }

            if (Convert.ToDouble(txtexcper.Text) == 0)
            {
                txtsheper.Enabled = true;
                txtSHEduCessAmt.Enabled = true;
                txtexcper.Enabled = false;
                txtExciseAmount.Enabled = false;
                txteducessper.Enabled = false;
                txtEduCessAmt.Enabled = false;

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "ViewRec", Ex.Message);
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

                txtBillNo.Text = billPassing_BL.BPM_NO.ToString();
                txtBillDate.Text = billPassing_BL.BPM_DATE.ToString("dd MMM yyyy");
                txtInvoiceDate.Text = billPassing_BL.BPM_INV_DATE.ToString("dd MMM yyyy");
                ddlSupplierName.SelectedValue = billPassing_BL.BPM_P_CODE.ToString();
                txtInvoceNo.Text = billPassing_BL.BPM_INV_NO.ToString();
                //ddlBillPass.SelectedItem.Text = billPassing_BL.BPM_BILL_PASS_BY.ToString();

                txtBasicAmount.Text = string.Format("{0:0.00}", billPassing_BL.BPM_BASIC_AMT);
                txtDiscountAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_DISCOUNT_AMT);
                txtPackingAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_PACKING_AMT);
                txtAccesableValue.Text = string.Format("{0:0.00}", billPassing_BL.BPM_ACCESS_AMT);
                //txtNetAmt.Text = string.Format("{0:0.00}",billPassing_BL.BPM_NET_AMT);
                txtExciseAmount.Text = string.Format("{0:0.00}", billPassing_BL.BPM_EXCIES_AMT);
                txtEduCessAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_ECESS_AMT);
                txtSHEduCessAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_HECESS_AMT);
                txtTaxableAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_TAXABLE_AMT);
                txtsalestaxamt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_TAX_AMT);
                txtOtherCharges.Text = string.Format("{0:0.00}", billPassing_BL.BPM_OTHER_AMT);
                txtloadingAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_ADD_DUTY);
                txtFreightAmt1.Text = string.Format("{0:0.00}", billPassing_BL.BPM_FREIGHT);

                txtInsuranceAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_INSURRANCE);
                txtTransportAmt.Text = string.Format("{0:0.00}", billPassing_BL.BPM_TRANSPORT);
                //txtFreightAmt1.Text = Convert.ToDouble(dt.Rows[0]["BPM_FREIGHT"]).ToString();
                //txtOctroiAmt1.Text = string.Format("{0:0.00}",billPassing_BL.BPM_OCTRO_PER);
                txtOctroiAmt2.Text = string.Format("{0:0.00}", billPassing_BL.BPM_OCTRO_AMT);
                txtRoundOff.Text = string.Format("{0:0.00}", billPassing_BL.BPM_ROUND_OFF);

                txtGrandTotal.Text = string.Format("{0:0.00}", billPassing_BL.BPM_G_AMT);
                rbLstIsExise.SelectedValue = billPassing_BL.BPM_EX_TYPE.ToString();
            }

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "GetValues", ex.Message);
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
            billPassing_BL.BPM_NO = Convert.ToInt32(txtBillNo.Text);
            billPassing_BL.BPM_DATE = Convert.ToDateTime(txtBillDate.Text);
            billPassing_BL.BPM_INV_DATE = Convert.ToDateTime(txtInvoiceDate.Text);
            billPassing_BL.BPM_P_CODE = Convert.ToInt32(ddlSupplierName.SelectedValue);
            billPassing_BL.BPM_INV_NO = txtInvoceNo.Text.ToString();
            billPassing_BL.BPM_BILL_PASS_BY = (string)Session["Username"];

            billPassing_BL.BPM_BASIC_AMT = Math.Round(Convert.ToDouble(txtBasicAmount.Text), 2);
            billPassing_BL.BPM_DISCOUNT_AMT = Math.Round(Convert.ToDouble(txtDiscountAmt.Text), 2);
            billPassing_BL.BPM_PACKING_AMT = Math.Round(Convert.ToDouble(txtPackingAmt.Text), 2);
            billPassing_BL.BPM_ACCESS_AMT = Math.Round(Convert.ToDouble(txtAccesableValue.Text), 2);
            // billPassing_BL.BPM_NET_AMT = float.Parse(txtNetAmt.Text);
            billPassing_BL.BPM_EXCIES_AMT = Math.Round(Convert.ToDouble(txtExciseAmount.Text), 2);
            billPassing_BL.BPM_ECESS_AMT = Math.Round(Convert.ToDouble(txtEduCessAmt.Text), 2);
            billPassing_BL.BPM_HECESS_AMT = Math.Round(Convert.ToDouble(txtSHEduCessAmt.Text), 2);
            //billPassing_BL.BPM_EXCPER = Math.Round(Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_EXC_PER"))).Text), 2);
            //billPassing_BL.BPM_EXCEDCESS_PER = Math.Round(Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_EDU_CESS_PER"))).Text), 2);
            //billPassing_BL.BPM_EXCHIEDU_PER = Math.Round(Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_H_EDU_CESS"))).Text), 2);


            billPassing_BL.BPM_EXCPER = Math.Round(Convert.ToDouble(txtexcper.Text), 2);
            billPassing_BL.BPM_EXCEDCESS_PER = Math.Round(Convert.ToDouble(txteducessper.Text), 2);
            billPassing_BL.BPM_EXCHIEDU_PER = Math.Round(Convert.ToDouble(txtsheper.Text), 2);

            billPassing_BL.BPM_TAXABLE_AMT = Math.Round(Convert.ToDouble((txtTaxableAmt.Text)), 2);
            //billPassing_BL.BPM_TAX_AMT = float.Parse(txtsalestaxamt.Text);
            billPassing_BL.BPM_TAX_AMT = Math.Round(Convert.ToDouble(txtsalestaxamt.Text), 2);
            billPassing_BL.BPM_TAX_PER = Math.Round(Convert.ToDouble(txtTaxPer.Text), 2);
            billPassing_BL.BPM_TAX_CODE = Convert.ToInt32(((Label)(dgBillPassing.Rows[0].FindControl("lblSPOD_T_CODE"))).Text);
            billPassing_BL.BPM_OTHER_AMT = Math.Round(Convert.ToDouble(txtOtherCharges.Text), 2);
            billPassing_BL.BPM_ADD_DUTY = Math.Round(Convert.ToDouble(txtloadingAmt.Text), 2);
            billPassing_BL.BPM_FREIGHT = Math.Round(Convert.ToDouble(txtFreightAmt1.Text), 2);
            billPassing_BL.BPM_INSURRANCE = Math.Round(Convert.ToDouble(txtInsuranceAmt.Text), 2);
            billPassing_BL.BPM_TRANSPORT = Math.Round(Convert.ToDouble(txtTransportAmt.Text), 2);
            billPassing_BL.BPM_OCTRO_AMT = Math.Round(Convert.ToDouble(txtOctroiAmt2.Text), 2);
            billPassing_BL.BPM_ROUND_OFF = Math.Round(Convert.ToDouble(txtRoundOff.Text), 2);
            billPassing_BL.BPM_G_AMT = Math.Round(Convert.ToDouble(txtGrandTotal.Text), 2);
            billPassing_BL.BPM_CM_CODE = Convert.ToInt32(Session["CompanyCode"].ToString());
            billPassing_BL.BPM_EX_TYPE = rbLstIsExise.SelectedValue.ToString();
            billPassing_BL.BPM_TYPE = ViewState["Type"].ToString();

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "Setvalues", ex.Message);
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
                DataTable dt3 = CommonClasses.Execute("SELECT * FROM BILL_PASSING_MASTER WHERE ES_DELETE=0 AND BPM_INV_NO='" + txtInvoceNo.Text + "'  AND BPM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND BPM_CM_CODE=" + Session["CompanyCode"] + "");
                if (dt3.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Invoice No allready Exist", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Invoice No allready Exist";
                    return false;
                }
                billPassing_BL = new BillPassing_BL();
                txtBillNo.Text = Numbering();
                if (Setvalues())
                {
                    if (billPassing_BL.Save(dgBillPassing, "INSERT"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(BPM_CODE) from BILL_PASSING_MASTER");
                        CommonClasses.WriteLog("Bill Passing", "Save", "Bill Passing", billPassing_BL.BPM_NO.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
                    }
                    else
                    {
                        if (billPassing_BL.Msg != "")
                        {
                            ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            billPassing_BL.Msg = "";
                            //PanelMsg.Visible = true;
                            //lblmsg.Text = "Record Not Saved";
                        }
                        ddlSupplierName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt3 = CommonClasses.Execute("SELECT * FROM BILL_PASSING_MASTER WHERE ES_DELETE=0 AND BPM_INV_NO='" + txtInvoceNo.Text + "'  AND BPM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND BPM_CM_CODE=" + Session["CompanyCode"] + " AND BPM_CODE !='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dt3.Rows.Count > 0)
                {
                    ShowMessage("#Avisos", "Invoice No already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Invoice No allready Exist";
                    return false;
                }
                billPassing_BL = new BillPassing_BL(Convert.ToInt32(ViewState["mlCode"]));

                if (Setvalues())
                {
                    if (billPassing_BL.Save(dgBillPassing, "UPDATE"))
                    {
                        CommonClasses.RemoveModifyLock("BILL_PASSING_MASTER", "MODIFY", "BPM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Bill Passing", "Update", "Bill Passing", billPassing_BL.BPM_NO.ToString(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewBillPassing.aspx", false);
                    }
                    else
                    {
                        if (billPassing_BL.Msg != "")
                        {
                            ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            billPassing_BL.Msg = "";
                            //PanelMsg.Visible = true;
                            //lblmsg.Text = "Record Not Saved";
                        }
                        ddlSupplierName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(BPM_NO) as BPM_NO from BILL_PASSING_MASTER  WHERE ES_DELETE=0 AND BPM_TYPE='" + ViewState["Type"].ToString() + "'  AND BPM_CM_CODE=" + Session["CompanyCode"] + "");
        if (dt.Rows[0]["BPM_NO"] == null || dt.Rows[0]["BPM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["BPM_NO"]) + 1;
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
                        DiscountAmt = DiscountAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_DISC_AMT"))).Text);


                        ExcAmt = Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblEX_EX_DUTY"))).Text);
                        ExcEduCessAmt = Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblEX_EX_CESS"))).Text);
                        ExcHighAmt = Convert.ToDouble(((Label)(dgBillPassing.Rows[0].FindControl("lblEX_EX_HCESS"))).Text);

                        string type = ((Label)(dgBillPassing.Rows[i].FindControl("lblIWM_TYPE"))).Text;


                        if (type == "OUTCUSTINV")
                        {
                            ViewState["Type"] = "OUTCUSTINV";
                        }
                        else
                        {
                            ViewState["Type"] = "IWIM";
                        }
                        //ViewState["Type"] 
                        //BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);
                        //BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);
                        //BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);
                        //BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_TOTAL_AMT"))).Text);

                        if (count == 0 && flag == false)
                        {
                            count = 1;
                            flag = true;
                        }
                    }
                    if (flag == true && count == 1)
                    {
                        count = 0;
                        string str1 = (((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_Y_N"))).Text);

                        if (str1 == "True")
                        {
                            ExcPer = Convert.ToDouble(txtexcper.Text);
                            ExcEduCessPer = Convert.ToDouble(txteducessper.Text);
                            ExcHighPer = Convert.ToDouble(txtsheper.Text);
                            txtexcper.ReadOnly = false;
                            txteducessper.ReadOnly = false;
                            txtsheper.ReadOnly = false;
                            ExcPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_PER"))).Text);
                            ExcEduCessPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EDU_CESS_PER"))).Text);
                            ExcHighPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_H_EDU_CESS"))).Text);
                        }
                        else
                        {
                            txtexcper.ReadOnly = true;
                            txteducessper.ReadOnly = true;
                            txtsheper.ReadOnly = true;
                            ExcPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EXC_PER"))).Text);
                            ExcEduCessPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_EDU_CESS_PER"))).Text);
                            ExcHighPer = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblSPOD_H_EDU_CESS"))).Text);
                        }

                        //Taxper = Convert.ToDouble(((Label)(dgBillPassing.Rows[i].FindControl("lblT_SALESTAX"))).Text);

                    }
                }
                if (BasicAmt > 0)
                {

                    txtBasicAmount.Text = string.Format("{0:0.00}", Math.Round(BasicAmt, 2));
                    txtDiscountAmt.Text = string.Format("{0:0.00}", Math.Round(DiscountAmt, 2));
                    txtPackingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackingAmt.Text), 2));
                    //acceablevalue = BasicAmt - DiscountAmt + Convert.ToDouble(txtPackingAmt.Text);
                    acceablevalue = BasicAmt + Convert.ToDouble(txtPackingAmt.Text);
                    txtAccesableValue.Text = string.Format("{0:0.00}", acceablevalue);

                    if (rbLstIsExise.SelectedValue == "1")
                    {
                        //ExcAmt = acceablevalue * ExcPer / 100;
                        //ExcEduCessAmt = ExcAmt * ExcEduCessPer / 100;
                        //ExcHighAmt = ExcAmt * ExcHighPer / 100;
                        txtexcper.Text = string.Format("{0:0.00}", ExcPer);
                        txteducessper.Text = string.Format("{0:0.00}", ExcEduCessPer);
                        txtsheper.Text = string.Format("{0:0.00}", ExcHighPer);
                        txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                        txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                        txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                    }
                    else
                    {
                        //ExcAmt = ExcAmt;
                        //ExcEduCessAmt = ExcAmt * ExcEduCessPer / 100;
                        //ExcHighAmt = ExcAmt * ExcHighPer / 100;
                        txtexcper.Text = string.Format("{0:0.00}", ExcPer);
                        txteducessper.Text = string.Format("{0:0.00}", ExcEduCessPer);
                        txtsheper.Text = string.Format("{0:0.00}", ExcHighPer);
                        txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                        txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                        txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);
                    }


                    if (ExcHighPer == 0)
                    {
                        txtsheper.Enabled = false;
                        txtSHEduCessAmt.Enabled = false;


                        txtexcper.Enabled = true;
                        txtExciseAmount.Enabled = true;

                        txteducessper.Enabled = true;
                        txtEduCessAmt.Enabled = true;

                    }

                    if (ExcPer == 0)
                    {
                        txtsheper.Enabled = true;
                        txtSHEduCessAmt.Enabled = true;


                        txtexcper.Enabled = false;
                        txtExciseAmount.Enabled = false;


                        txteducessper.Enabled = false;
                        txtEduCessAmt.Enabled = false;

                    }

                    //TaxAmt = Math.Round((TaxableAmt * Taxper / 100), 2);

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

                    TaxableAmt = acceablevalue + AddAmt + FrightAmt + InsurranceAmt + TransportAmt + OctriAmt + OtherAmt;
                    txtTaxableAmt.Text = string.Format("{0:0.00}", TaxableAmt);

                    totalStr = DecimalMasking(txtRoundOff.Text);
                    txtRoundOff.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                    RoundOff = Convert.ToDouble(txtRoundOff.Text);



                    txtTaxPer.Text = string.Format("{0:0.00}", Taxper);
                    txtsalestaxamt.Text = string.Format("{0:0.00}", TaxAmt);
                    GrandAmt = Math.Round(TaxableAmt + ExcAmt + ExcEduCessAmt + ExcHighAmt + RoundOff, 2);
                    txtGrandTotal.Text = string.Format("{0:0.00}", GrandAmt);
                    //grandtotal();
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
            CommonClasses.SendError("Bill Passing", "Calculate", ex.Message.ToString());
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtPackingAmt_TextChanged", Ex.Message.ToString());
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
            CommonClasses.SendError("Bill Passing", "txtDiscountAmt_TextChanged", Ex.Message.ToString());
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtOtherCharges_TextChanged", Ex.Message.ToString());
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
            CommonClasses.SendError("Bill Passing", "txtloadingAmt_TextChanged", Ex.Message.ToString());
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtInsuranceAmt_TextChanged", Ex.Message.ToString());
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtTransportAmt_TextChanged", Ex.Message.ToString());
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

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "txtOctroiAmt2_TextChanged", Ex.Message.ToString());
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
            CommonClasses.SendError("Bill Passing", "txtRoundOff_TextChanged", Ex.Message.ToString());
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
            CommonClasses.SendError("Bill Passing", "txtDiscountAmt1_TextChanged", Ex.Message.ToString());
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
            CommonClasses.SendError("Bill Passing", "txtDiscountAmt2_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    //#region txtOctroiAmt1_TextChanged
    //protected void txtOctroiAmt1_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Calculate(-1);

    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Bill Passing", "txtOctroiAmt1_TextChanged", Ex.Message.ToString());
    //    }
    //}
    //#endregion


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

                    if (rbLstIsExise.SelectedIndex == 1)
                    {
                        dt.Rows.Clear();
                        // dt = CommonClasses.Execute("SELECT distinct  IWM_TYPE AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO, CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),ROUND(IWD_RATE,2)) AS IWD_RATE , CONVERT(decimal(10,2),(IWD_REV_QTY * ROUND(IWD_RATE,2)))AS SPOD_TOTAL_AMT,CONVERT(decimal(10,2),SPOD_DISC_AMT) AS SPOD_DISC_AMT ,SPOD_EXC_PER,0 AS EX_EX_DUTY,SPOD_EDU_CESS_PER,0 AS EX_EX_CESS, SPOD_H_EDU_CESS ,0 AS EX_EX_HCESS,SPOD_T_CODE,ST_ALIAS AS T_SHORT,  ST_SALES_TAX AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,ROUND(IWD_RATE,2) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,2)*IWD_CON_OK_QTY,2) AS SPOD_TOTAL_AMT,ST_TAX_NAME,ST_SALES_TAX,SPOD_EXC_Y_N   FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,SALES_TAX_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND  IWD_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND SPOD_T_CODE = ST_CODE AND IWD_BILL_PASS_FLG = 0 AND IWD_INSP_FLG = 1   AND INWARD_MASTER.ES_DELETE = 0 AND  IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY INWARD_MASTER.IWM_NO");
                        dt = CommonClasses.Execute("SELECT distinct  IWM_TYPE AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO, CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),ROUND(IWD_RATE,2)) AS IWD_RATE , CONVERT(decimal(10,2),(IWD_CH_QTY * ROUND(IWD_RATE,2)))AS SPOD_TOTAL_AMT,CONVERT(decimal(10,2),SPOD_DISC_AMT) AS SPOD_DISC_AMT , CASE WHEN P_SM_CODE=CM_STATE then SPOD_EXC_PER else 0 END  AS SPOD_EXC_PER,0 AS EX_EX_DUTY,CASE WHEN P_SM_CODE=CM_STATE then SPOD_EDU_CESS_PER else 0 END AS SPOD_EDU_CESS_PER,0 AS EX_EX_CESS, CASE WHEN P_SM_CODE<>CM_STATE then SPOD_H_EDU_CESS else 0 END AS SPOD_H_EDU_CESS ,0 AS EX_EX_HCESS,SPOD_T_CODE,'' AS T_SHORT,  '' AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE, ROUND(IWD_CH_QTY,2) AS IWD_CH_QTY,ROUND(IWD_RATE,2) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,2)*IWD_CH_QTY,2) AS SPOD_TOTAL_AMT,'' AS ST_TAX_NAME,'' AS ST_SALES_TAX,SPOD_EXC_Y_N    ,P_SM_CODE,CM_STATE FROM   SUPP_PO_MASTER ,SUPP_PO_DETAILS,INWARD_MASTER,INWARD_DETAIL,ITEM_UNIT_MASTER,ITEM_MASTER,PARTY_MASTER,COMPANY_MASTER where SPOD_SPOM_CODE = SPOM_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND IWM_CODE = IWD_IWM_CODE AND IWD_I_CODE = I_CODE AND IWM_CM_CODE=  " + Convert.ToInt32(Session["CompanyCode"]) + " AND  IWD_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOD_I_CODE = I_CODE AND IWD_BILL_PASS_FLG = 0 AND IWD_INSP_FLG = 1   AND INWARD_MASTER.ES_DELETE = 0 AND P_CODE=SPOM_P_CODE AND IWM_CM_CODE=CM_CODE  AND    IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' ORDER BY INWARD_MASTER.IWM_NO");
                        txtBasicAmount.Enabled = true;
                        txtDiscountAmt.Enabled = true;
                        txtPackingAmt.Enabled = true;
                        txtAccesableValue.Enabled = true;
                        txtExciseAmount.Enabled = true;
                        txtEduCessAmt.Enabled = true;
                        txtSHEduCessAmt.Enabled = true;
                        txtTaxableAmt.Enabled = true;
                        txtsalestaxamt.Enabled = true;
                        txtTaxPer.Enabled = true;
                        txtOtherCharges.Enabled = true;
                        txtloadingAmt.Enabled = true;
                        txtFreightAmt1.Enabled = true;
                        txtInsuranceAmt.Enabled = true;
                        txtOctroiAmt2.Enabled = true;
                        txtTransportAmt.Enabled = true;
                        txtGrandTotal.Enabled = true;
                        txtexcper.Enabled = true;
                        txteducessper.Enabled = true;
                        txtsheper.Enabled = true;
                    }
                    else
                    {
                        dt.Rows.Clear();
                        //  dt = CommonClasses.Execute("SELECT  distinct IWM_TYPE  AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO,CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),IWD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(IWD_REV_QTY * ROUND(IWD_RATE,2)))AS SPOD_TOTAL_AMT,CONVERT(decimal(10,2),SPOD_DISC_AMT) AS SPOD_DISC_AMT ,SPOD_EXC_PER,EX_EX_DUTY,SPOD_EDU_CESS_PER,EX_EX_CESS, SPOD_H_EDU_CESS ,EX_EX_HCESS,SPOD_T_CODE,ST_ALIAS AS T_SHORT,  ST_SALES_TAX AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,ROUND(IWD_RATE,2) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,2)*IWD_CON_OK_QTY,2) AS SPOD_TOTAL_AMT,ST_TAX_NAME,ST_SALES_TAX,SPOD_EXC_Y_N  From EXICSE_ENTRY, EXCISE_DETAIL, Inward_master, INWARD_DETAIL, SUPP_PO_MASTER, SUPP_PO_DETAILS,ITEM_MASTER, ITEM_UNIT_MASTER, SALES_TAX_MASTER WHERE EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = IWM_CODE AND IWM_CODE = IWD_IWM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOM_CODE = SPOD_SPOM_CODE  AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE =  ITEM_UNIT_MASTER.I_UOM_CODE AND SPOD_T_CODE = ST_CODE AND IWD_BILL_PASS_FLG=0 AND IWD_INSP_FLG = 1 AND (IWM_TYPE='IWIM' OR IWM_TYPE='IWIAP') AND IWM_INWARD_TYPE IN(0,1) AND INWARD_MASTER.ES_DELETE = 0 AND IWD_MODVAT_FLG = 1 AND EXICSE_ENTRY.ES_DELETE = 0  AND SPOD_EXC_PER >=0 AND IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND SPOD_I_CODE = IWD_I_CODE AND EX_INV_NO = '" + txtInvoceNo.Text.Trim() + "'  GROUP BY IWM_TYPE,SPOM_PO_NO,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,IWD_REV_QTY,IWD_CON_OK_QTY,SPOD_RATE,IWD_RATE,SPOD_DISC_AMT,SPOD_EXC_PER,EX_EX_DUTY,SPOD_EDU_CESS_PER,EX_EX_CESS,SPOD_H_EDU_CESS , EX_EX_HCESS, SPOD_T_CODE, ST_ALIAS, ST_SALES_TAX, SPOM_CODE, IWM_CODE, SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,SPOD_RATE,SPOD_TOTAL_AMT,ST_TAX_NAME,ST_SALES_TAX,SPOD_EXC_Y_N,SPOM_PONO order by IWM_NO");
                        // dt = CommonClasses.Execute(" SELECT  distinct IWM_TYPE  AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO,CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),IWD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(IWD_REV_QTY * ROUND(IWD_RATE,2)))AS SPOD_TOTAL_AMT,CONVERT(decimal(10,2),SPOD_DISC_AMT) AS SPOD_DISC_AMT ,SPOD_EXC_PER,EX_EX_DUTY,SPOD_EDU_CESS_PER,EX_EX_CESS, SPOD_H_EDU_CESS ,EX_EX_HCESS,SPOD_T_CODE,'' AS T_SHORT, '' AS ST_SALES_TAX ,''AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,ROUND(IWD_RATE,2) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,2)*IWD_CON_OK_QTY,2) AS SPOD_TOTAL_AMT, '' AS ST_TAX_NAME,'' as ST_SALES_TAX,SPOD_EXC_Y_N  From EXICSE_ENTRY, EXCISE_DETAIL, Inward_master, INWARD_DETAIL, SUPP_PO_MASTER, SUPP_PO_DETAILS,ITEM_MASTER, ITEM_UNIT_MASTER WHERE EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = IWM_CODE AND IWM_CODE = IWD_IWM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOM_CODE = SPOD_SPOM_CODE  AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE =  ITEM_UNIT_MASTER.I_UOM_CODE   AND IWD_BILL_PASS_FLG=0 AND IWD_INSP_FLG = 1 AND (IWM_TYPE='IWIM' OR IWM_TYPE='OUTCUSTINV') AND IWM_INWARD_TYPE IN(0,1) AND INWARD_MASTER.ES_DELETE = 0 AND IWD_MODVAT_FLG = 1 AND EXICSE_ENTRY.ES_DELETE = 0  AND SPOD_EXC_PER >=0 AND IWM_P_CODE='" + ddlSupplierName.SelectedValue + "'  AND SPOD_I_CODE = IWD_I_CODE AND EX_INV_NO =  '" + txtInvoceNo.Text.Trim() + "'  GROUP BY IWM_TYPE,SPOM_PO_NO,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,IWD_REV_QTY,IWD_CON_OK_QTY,SPOD_RATE,IWD_RATE,SPOD_DISC_AMT,SPOD_EXC_PER,EX_EX_DUTY,SPOD_EDU_CESS_PER,EX_EX_CESS,SPOD_H_EDU_CESS , EX_EX_HCESS, SPOD_T_CODE, SPOM_CODE, IWM_CODE, SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,SPOD_RATE,SPOD_TOTAL_AMT,SPOD_EXC_Y_N,SPOM_PONO order by IWM_NO");

                        dt = CommonClasses.Execute("SELECT distinct IWM_TYPE  AS IWM_TYPE  ,SPOM_PONO AS SPOM_PO_NO,IWM_NO,CONVERT(VARCHAR,IWM_DATE,106)  AS IWM_DATE,IWM_CHALLAN_NO,CONVERT(VARCHAR,IWM_CHAL_DATE,106)  AS IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,CONVERT(decimal(10,3),IWD_REV_QTY) AS IWD_REV_QTY, CONVERT(decimal(10,3),IWD_CON_OK_QTY) AS IWD_CON_OK_QTY ,CONVERT(decimal(10,2),IWD_RATE) AS IWD_RATE , CONVERT(decimal(10,2),(IWD_CH_QTY * ROUND(IWD_RATE,2)))AS SPOD_TOTAL_AMT,CONVERT(decimal(10,2),SPOD_DISC_AMT) AS SPOD_DISC_AMT ,EXD_EXC_PER AS  SPOD_EXC_PER,EX_EX_DUTY,EXD_EDU_PER AS SPOD_EDU_CESS_PER,EX_EX_CESS, EXD_HSEDU_PER AS SPOD_H_EDU_CESS ,EX_EX_HCESS,SPOD_T_CODE,'' AS T_SHORT, '' AS ST_SALES_TAX ,''AS T_SALESTAX,'0.00' AS SALESTAX_AMT ,SPOM_CODE,IWM_CODE,CONVERT(decimal(10,3),SPOD_GP_RATE) AS SPOD_GP_RATE,IWD_CPOM_CODE,ROUND(IWD_CH_QTY,2) AS IWD_CH_QTY,ROUND(IWD_RATE,2) AS SPOD_RATE,ROUND(ROUND(IWD_RATE,2)*IWD_CH_QTY,2) AS SPOD_TOTAL_AMT, '' AS ST_TAX_NAME,'' as ST_SALES_TAX,SPOD_EXC_Y_N  From EXICSE_ENTRY, EXCISE_DETAIL, Inward_master, INWARD_DETAIL, SUPP_PO_MASTER, SUPP_PO_DETAILS,ITEM_MASTER, ITEM_UNIT_MASTER WHERE EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = IWM_CODE AND IWM_CODE = IWD_IWM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOM_CODE = SPOD_SPOM_CODE  AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE =  ITEM_UNIT_MASTER.I_UOM_CODE   AND IWD_BILL_PASS_FLG=0 AND IWD_INSP_FLG = 1 AND IWM_CM_CODE=  " + Convert.ToInt32(Session["CompanyCode"]) + " AND (IWM_TYPE='IWIM' OR IWM_TYPE='OUTCUSTINV') AND IWM_INWARD_TYPE IN(0,1) AND INWARD_MASTER.ES_DELETE = 0 AND IWD_MODVAT_FLG = 1 AND EXICSE_ENTRY.ES_DELETE = 0  AND EXD_I_CODE= IWD_I_CODE   AND IWM_P_CODE='" + ddlSupplierName.SelectedValue + "'  AND SPOD_I_CODE = IWD_I_CODE AND EX_INV_NO =  '" + txtInvoceNo.Text.Trim() + "' GROUP BY IWM_TYPE,SPOM_PO_NO,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME,IWD_REV_QTY,IWD_CON_OK_QTY,SPOD_RATE,IWD_RATE,SPOD_DISC_AMT,SPOD_EXC_PER,EX_EX_DUTY,SPOD_EDU_CESS_PER,EX_EX_CESS,SPOD_H_EDU_CESS , EX_EX_HCESS, SPOD_T_CODE, SPOM_CODE, IWM_CODE, SPOD_GP_RATE,IWD_CPOM_CODE,IWD_CH_QTY,SPOD_RATE,SPOD_TOTAL_AMT,SPOD_EXC_Y_N,SPOM_PONO, EXD_EXC_PER,EXD_EDU_PER,EXD_HSEDU_PER order by IWM_NO");

                        txtBasicAmount.Enabled = false;
                        txtDiscountAmt.Enabled = false;
                        txtPackingAmt.Enabled = false;
                        txtAccesableValue.Enabled = false;
                        txtExciseAmount.Enabled = false;
                        txtEduCessAmt.Enabled = false;
                        txtSHEduCessAmt.Enabled = false;
                        txtTaxableAmt.Enabled = false;
                        txtsalestaxamt.Enabled = false;
                        txtTaxPer.Enabled = false;
                        txtOtherCharges.Enabled = false;
                        txtloadingAmt.Enabled = false;
                        txtFreightAmt1.Enabled = false;
                        txtInsuranceAmt.Enabled = false;
                        txtOctroiAmt2.Enabled = false;
                        txtTransportAmt.Enabled = false;
                        txtGrandTotal.Enabled = false;
                        txtexcper.Enabled = false;
                        txteducessper.Enabled = false;
                        txtsheper.Enabled = false;
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
                        //PanelMsg.Visible = true;
                        //lblmsg.Text = "This Supplier Bill Not Present";
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
            dt = CommonClasses.FillCombo("PARTY_MASTER,INWARD_MASTER,INWARD_DETAIL", "P_NAME", "P_CODE", "IWM_P_CODE=P_CODE AND IWM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + "   AND IWD_IWM_CODE=IWM_CODE AND IWD_INSP_FLG='1' AND PARTY_MASTER.ES_DELETE='0' AND P_TYPE=2 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY P_NAME", ddlSupplierName);
            ddlSupplierName.Items.Insert(0, new ListItem("Please Select Supplier ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "FillSupplier", Ex.Message);
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
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CPOM_CODE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_TYPE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHAL_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CH_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_REV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IWD_CON_OK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_RATE", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_TOTAL_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_DISC_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EXC_PER", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_EDU_CESS_PER", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_H_EDU_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_DUTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_CESS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("EX_EX_HCESS", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_T_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ST_TAX_NAME", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("ST_SALES_TAX", typeof(String)));
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
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Record used by another user";
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);

                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing-ADD", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Bill Passing - ADD", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #endregion

    protected void txtexcper_TextChanged(object sender, EventArgs e)
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
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
        //txtExciseAmount.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtAccesableValue.Text)*Convert.ToDouble(txtexcper.Text))/100);
    }
    protected void txteducessper_TextChanged(object sender, EventArgs e)
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
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
        //txtEduCessAmt.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtExciseAmount.Text) * Convert.ToDouble(txteducessper.Text)) / 100);
    }
    protected void txtsheper_TextChanged(object sender, EventArgs e)
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
            CommonClasses.SendError("Bill Passing", "txtFreightAmt_TextChanged", Ex.Message.ToString());
        }
        //txtSHEduCessAmt.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtExciseAmount.Text) * Convert.ToDouble(txtsheper.Text)) / 100);
    }
    protected void txtExciseAmount_TextChanged(object sender, EventArgs e)
    {
        if (dgBillPassing.Enabled == true)
        {
            Calculate(1);
        }
    }

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
            Calculate(1);
        }
        catch (Exception)
        {

        }
    }

    //LoadDetail function used for show select all records from gridview in Popup 
    private void LoadDetail()
    {
        DataTable dtDetail = new DataTable();
        string ICode = "";
        if (dgBillPassing.Enabled == true)
        {
            if (ddlSupplierName.SelectedIndex != 0)
            {
                if (rbLstIsExise.SelectedIndex == 0)
                {
                    ViewState["ICode"] = null;

                    foreach (GridViewRow row in dgBillPassing.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            ICode = "";
                            CheckBox chkRow = (row.Cells[0].FindControl("chkSelect") as CheckBox);
                            if (chkRow.Checked)
                            {
                                ICode = (row.Cells[17].FindControl("lblIWD_I_CODE") as Label).Text;
                                ICode += ",";
                            }
                        }
                        ViewState["ICode"] = ViewState["ICode"] + ICode;
                    }
                }
            }
        }
        else
        {
            ShowMessage("#Avisos", "Record Not found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        ViewState["ICode"] = ViewState["ICode"].ToString().TrimEnd(',');
        //Bind Gridview Bill Passing query here
        //dtDetail = CommonClasses.Execute("SELECT DISTINCT COUNT(IWD_I_CODE) AS ICount,EXD_EXC_PER AS SPOD_EXC_PER,sum(EX_EX_DUTY) as EX_EX_DUTY,EXD_EDU_PER AS SPOD_EDU_CESS_PER,sum(EX_EX_CESS) as EX_EX_CESS,EXD_HSEDU_PER AS SPOD_H_EDU_CESS,Sum(EX_EX_HCESS) as EX_EX_HCESS From EXICSE_ENTRY, EXCISE_DETAIL, Inward_master, INWARD_DETAIL, SUPP_PO_MASTER, SUPP_PO_DETAILS,ITEM_MASTER, ITEM_UNIT_MASTER WHERE EXD_EX_CODE = EX_CODE And EXD_IWM_CODE = IWM_CODE AND IWM_CODE = IWD_IWM_CODE AND SPOM_CODE = IWD_CPOM_CODE AND SPOM_CODE = SPOD_SPOM_CODE AND IWD_I_CODE = I_CODE AND IWD_UOM_CODE =ITEM_UNIT_MASTER.I_UOM_CODE AND IWD_INSP_FLG = 1 AND IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND (IWM_TYPE='IWIM' OR IWM_TYPE='OUTCUSTINV') AND IWM_INWARD_TYPE IN(0,1) AND INWARD_MASTER.ES_DELETE = 0 AND IWD_MODVAT_FLG = 1 AND EXICSE_ENTRY.ES_DELETE = 0 AND EXD_I_CODE=IWD_I_CODE AND IWM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND SPOD_I_CODE = IWD_I_CODE AND EX_INV_NO ='" + txtInvoceNo.Text + "' AND IWD_I_CODE IN (" + ICode + ") GROUP BY IWM_TYPE,EXD_EXC_PER,EXD_EDU_PER,EXD_HSEDU_PER");
        if (Request.QueryString[0].Equals("INSERT"))
        {
            dtDetail = CommonClasses.Execute("SELECT COUNT(EXD_I_CODE) AS ICOUNT,EXD_EXC_PER,SUM(EXD_EXC_AMT) AS EXD_EXC_AMT,EXD_EDU_PER,SUM(EXD_EDU_AMT) AS EXD_EDU_AMT,EXD_HSEDU_PER,SUM(EXD_HSEDU_AMT) AS EXD_HSEDU_AMT FROM EXICSE_ENTRY,EXCISE_DETAIL WHERE EXD_EX_CODE = EX_CODE AND EXICSE_ENTRY.ES_DELETE=0 AND EX_P_CODE='" + ddlSupplierName.SelectedValue + "' AND EX_INV_NO ='" + txtInvoceNo.Text + "' AND EX_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND EXD_I_CODE IN (" + ViewState["ICode"] + ") GROUP BY EXD_EXC_PER,EXD_EDU_PER,EXD_HSEDU_PER");
        }
        else
        {
            dtDetail = CommonClasses.Execute("SELECT COUNT(BPD_I_CODE) AS ICOUNT,BPM_EXCPER as EXD_EXC_PER,SUM(BPD_EXC_AMT) AS EXD_EXC_AMT,BPM_EXCEDCESS_PER as EXD_EDU_PER,SUM(BPD_EDU_AMT) AS EXD_EDU_AMT,BPM_EXCHIEDU_PER as EXD_HSEDU_PER,SUM(BPD_HSEDU_AMT) AS EXD_HSEDU_AMT FROM BILL_PASSING_DETAIL,BILL_PASSING_MASTER WHERE BPM_CODE=BPD_BPM_CODE AND BILL_PASSING_MASTER.ES_DELETE=0 AND BPM_P_CODE='" + ddlSupplierName.SelectedValue + "' AND BPM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' AND BPM_INV_NO='" + txtInvoceNo.Text + "' AND BPM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND BPD_I_CODE IN (" + ViewState["ICode"] + ") GROUP BY BPM_EXCPER,BPM_EXCEDCESS_PER,BPM_EXCHIEDU_PER");
        }
        dgDetail.Enabled = true;
        dgDetail.DataSource = dtDetail;
        dgDetail.DataBind();
    }

    //link button detail: show popup of GST Details
    protected void lnkPop_Click(object sender, EventArgs e)
    {
        LoadDetail();
        if (dgDetail.Rows.Count > 0)
        {
            PnlDetail.Visible = true;
            ModalPopupDetail.Show();
        }
        else
        {
            ShowMessage("#Avisos", "Record Not Selected", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }

    protected void dgDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    protected void dgDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetail.PageIndex = e.NewPageIndex;
            LoadDetail();
        }
        catch (Exception)
        {
        }
    }
}
