using System;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI;
public partial class Transactions_ADD_LabourChargeInvoiceNew : System.Web.UI.Page
{
    DataTable dtFilter = new DataTable();
    static DataTable DtInvDet = new DataTable();
    DataRow dr;
    static int mlCode = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {

            LoadCombos();
            LoadInvoices();
            LoadFilter();
            txtToDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Attributes.Add("readonly", "readonly");

            if (Request.QueryString[0].Equals("VIEW"))
            {
                ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                pnl.Visible = false;
                ViewRec("VIEW");
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                pnl.Visible = false;
                ViewRec("MOD");
            }
            else if (Request.QueryString[0].Equals("INSERT"))
            {
                LoadFilter();
                dgInvDetails.Enabled = false;
            }
        }
    }

    private void ViewRec(string p)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("SELECT * FROM LABOR_INVOICE_MASTER WHERE ES_DELETE=0 and LIM_CODE=" + ViewState["mlCode"].ToString());
            if (dt.Rows.Count > 0)
            {
                ddlCustomer.SelectedValue = dt.Rows[0]["LIM_P_CODE"].ToString();
                //txtBasicAmount.Text = dt.Rows[0][""].ToString();
                //txtEduCessAmt.Text = dt.Rows[0][""].ToString();
                //txtSHEduCessAmt.Text = dt.Rows[0][""].ToString();
                //txtExciseAmount.Text = dt.Rows[0][""].ToString();
                //txtTaxableAmt.Text = dt.Rows[0][""].ToString();
                //txtGrandTotal.Text = dt.Rows[0][""].ToString();
            }
            DataTable dtDetail = new DataTable();
            dtDetail = CommonClasses.Execute("SELECT PARTY_MASTER.P_NAME, LABOR_INVOICE_MASTER.LIM_P_CODE AS INM_P_CODE, LABOR_INVOICE_DETAIL.LID_I_CODE AS I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, LABOR_INVOICE_DETAIL.LID_INM_CODE AS INM_CODE, LABOR_INVOICE_DETAIL.LID_INM_NO AS INM_NO,convert(varchar,LABOR_INVOICE_DETAIL.LID_INM_DATE,106) AS INM_DATE, LABOR_INVOICE_DETAIL.LID_QTY AS IND_INQTY, LABOR_INVOICE_DETAIL.LID_RATE AS IND_RATE, LABOR_INVOICE_DETAIL.LID_AMT AS IND_AMT, LABOR_INVOICE_DETAIL.LID_CGST_AMT AS IND_EX_AMT, LABOR_INVOICE_DETAIL.LID_SGST_AMT AS IND_E_CESS_AMT, LABOR_INVOICE_DETAIL.LID_IGST_AMT AS IND_SH_CESS_AMT, LABOR_INVOICE_DETAIL.LID_TOTAL AS Total,E_TARIFF_NO,E_CODE FROM LABOR_INVOICE_DETAIL INNER JOIN LABOR_INVOICE_MASTER ON LABOR_INVOICE_DETAIL.LID_LIM_CODE = LABOR_INVOICE_MASTER.LIM_CODE INNER JOIN PARTY_MASTER ON LABOR_INVOICE_MASTER.LIM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN EXCISE_TARIFF_MASTER ON LABOR_INVOICE_DETAIL.LID_SAC_CODE = EXCISE_TARIFF_MASTER.E_CODE INNER JOIN ITEM_MASTER ON LABOR_INVOICE_DETAIL.LID_I_CODE = ITEM_MASTER.I_CODE where LABOR_INVOICE_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND LIM_CODE=" + ViewState["mlCode"].ToString());
            if (dtDetail.Rows.Count > 0)
            {
                dgInvDetails.DataSource = dtDetail;
                dgInvDetails.DataBind();
                dgInvDetails.Enabled = true;
                for (int i = 0; i < dgInvDetails.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    chkRow.Checked = true;
                }
                Calculate(1);
            }
            else
            {
                LoadFilter();
            }
            if (p == "MOD")
            {
                ddlCustomer.Enabled = false;
                btnLoad.Enabled = false;
                // CommonClasses.SetModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            else
            {
                ddlCustomer.Enabled = false;
                btnLoad.Enabled = false;
                btnExport.Visible = false;
                dgInvDetails.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void LoadInvoices()
    {
        try
        {
            DataTable FromInv = new DataTable();
            // FromInv = CommonClasses.Execute("SELECT DISTINCT INM_CODE,INM_NO FROM INVOICE_MASTER WHERE   ES_DELETE=0 AND INM_INVOICE_TYPE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND INM_TYPE='TAXINV' AND ISNULL(INM_IS_TALLY_TRANS,0)=0 ORDER BY INM_NO DESC");

            //FromInv = CommonClasses.Execute("SELECT DISTINCT top 1000 INM_CODE,INM_NO FROM INVOICE_MASTER WHERE   ES_DELETE=0 AND INM_INVOICE_TYPE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND INM_TYPE='TAXINV'  ORDER BY INM_NO DESC");
            FromInv = CommonClasses.Execute("SELECT DISTINCT INM_CODE,INM_NO FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INVOICE_MASTER.ES_DELETE=0 AND INM_INVOICE_TYPE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND INM_TYPE='OutJWINM' and INM_INV_TYPE=1  AND INM_CODE=IND_INM_CODE  AND isnull(IND_LID_FLAG,0)=0 ORDER BY INM_NO DESC");
            ddlFromInvNo.DataSource = FromInv;
            ddlFromInvNo.DataTextField = "INM_NO";
            ddlFromInvNo.DataValueField = "INM_CODE";
            ddlFromInvNo.DataBind();
            ddlFromInvNo.Items.Insert(0, new ListItem("Select Invoice No", "0"));

            ddlToInvNo.DataSource = FromInv;
            ddlToInvNo.DataTextField = "INM_NO";
            ddlToInvNo.DataValueField = "INM_CODE";
            ddlToInvNo.DataBind();
            ddlToInvNo.Items.Insert(0, new ListItem("Select Invoice No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Create VCM", "LoadInvoices", Ex.Message.ToString());
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable custdet = new DataTable();

            custdet = CommonClasses.Execute("SELECT DISTINCT PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME FROM PARTY_MASTER INNER JOIN INVOICE_MASTER ON PARTY_MASTER.P_CODE = INVOICE_MASTER.INM_P_CODE WHERE (PARTY_MASTER.P_CM_COMP_ID = " + Session["CompanyId"].ToString() + ") and PARTY_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and INM_INV_TYPE=1 ORDER BY PARTY_MASTER.P_NAME");
            ddlCustomer.DataSource = custdet;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally transfer Sales", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region btnLoad_Click
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            if (txtFromDate.Text.Equals(String.Empty) || txtToDate.Text.Equals(String.Empty))
            {
                lblmsg.Text = "Please Select From & To Date";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            if (ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Invoice From & To";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (ddlCustomer.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Customer";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            //string Query = "SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE FROM INVOICE_MASTER WHERE INM_NO BETWEEN '" + ddlFromInvNo.SelectedItem.ToString() + "' AND '" + ddlToInvNo.SelectedItem.ToString() + "' AND INM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and INM_CM_CODE='" + Session["CompanyCode"].ToString() + "'";


            string Query = "SELECT PARTY_MASTER.P_NAME, INVOICE_MASTER.INM_P_CODE, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, INVOICE_MASTER.INM_CODE, INVOICE_MASTER.INM_NO, convert(varchar,INVOICE_MASTER.INM_DATE,106) as INM_DATE, INVOICE_DETAIL.IND_INQTY, INVOICE_DETAIL.IND_RATE, INVOICE_DETAIL.IND_EX_AMT, INVOICE_DETAIL.IND_E_CESS_AMT, INVOICE_DETAIL.IND_SH_CESS_AMT, INVOICE_DETAIL.IND_AMT, ROUND(INVOICE_DETAIL.IND_AMT + INVOICE_DETAIL.IND_EX_AMT + INVOICE_DETAIL.IND_E_CESS_AMT+ INVOICE_DETAIL.IND_SH_CESS_AMT, 2) AS Total,EXCISE_TARIFF_MASTER.E_TARIFF_NO, EXCISE_TARIFF_MASTER.E_CODE FROM INVOICE_DETAIL INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_SCAT_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE (EXCISE_TARIFF_MASTER.ES_DELETE = 0) AND (PARTY_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND (INVOICE_MASTER.ES_DELETE = 0) AND (INVOICE_MASTER.INM_TYPE = 'OutJWINM') AND (INVOICE_MASTER.INM_INV_TYPE = 1) and (INM_NO between " + ddlFromInvNo.SelectedItem + " and " + ddlToInvNo.SelectedItem + ") and (P_CODE=" + ddlCustomer.SelectedValue + ") and (INM_DATE between '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd MMM yyyy") + "' and '" + Convert.ToDateTime(txtToDate.Text).ToString("dd MMM yyyy") + "')  and (INM_CM_CODE='" + Session["CompanyCode"].ToString() + "') and isnull(IND_LID_FLAG,0)=0   ";

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count == 0)
            {
                LoadFilter();
                lblmsg.Text = "No record found";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            else
            {
                dgInvDetails.DataSource = dt;
                dgInvDetails.DataBind();
                dgInvDetails.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion
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

    void Calculate(int index)
    {
        try
        {

            if (index != -1)
            {
                double BasicAmt = 0;

                double ExcPer = 0;

                double ExcHighPer = 0;
                double ExcAmt = 0;
                double ExcEduCessAmt = 0;
                double ExcHighAmt = 0;
                double Taxper = 0;
                double TaxAmt = 0;
                double TaxableAmt = 0, RoundOff = 0, GrandAmt = 0;
                bool flag = false; int count = 0;
                for (int i = 0; i < dgInvDetails.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgInvDetails.Rows[i].FindControl("lblIND_AMT"))).Text);
                        ExcAmt = ExcAmt + Convert.ToDouble(((Label)(dgInvDetails.Rows[i].FindControl("lblIND_EX_AMT"))).Text);
                        ExcEduCessAmt = ExcEduCessAmt + Convert.ToDouble(((Label)(dgInvDetails.Rows[i].FindControl("lblIND_E_CESS_AMT"))).Text);
                        ExcHighAmt = ExcHighAmt + Convert.ToDouble(((Label)(dgInvDetails.Rows[i].FindControl("lblIND_SH_CESS_AMT"))).Text);

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



                    TaxableAmt = Convert.ToDouble(txtBasicAmount.Text);
                    txtTaxableAmt.Text = string.Format("{0:0.00}", TaxableAmt);


                    txtExciseAmount.Text = string.Format("{0:0.00}", ExcAmt);
                    txtEduCessAmt.Text = string.Format("{0:0.00}", ExcEduCessAmt);
                    txtSHEduCessAmt.Text = string.Format("{0:0.00}", ExcHighAmt);



                    txtSHEduCessAmt.Enabled = false;
                    txtExciseAmount.Enabled = false;
                    txtEduCessAmt.Enabled = false;



                    GrandAmt = TaxableAmt + ExcAmt + ExcEduCessAmt + ExcHighAmt;
                    txtGrandTotal.Text = string.Format("{0:0.00}", GrandAmt);
                }
                else
                {
                    txtBasicAmount.Text = "0.00";

                    txtExciseAmount.Text = "0.00";
                    txtEduCessAmt.Text = "0.00";
                    txtSHEduCessAmt.Text = "0.00";
                    txtTaxableAmt.Text = "0.00";

                    txtGrandTotal.Text = "0.00";

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("EXICSE ENTRY", "Calculate", ex.Message.ToString());
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        if (!txtFromDate.Text.Equals(String.Empty) && !txtToDate.Text.Equals(String.Empty))
        {
            DateTime dateFrom = Convert.ToDateTime(txtFromDate.Text);
            DateTime dateTo = Convert.ToDateTime(txtToDate.Text);
            if (dateFrom > dateTo)
            {
                lblmsg.Text = "From date must be less than To date";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
    }
    protected void ddlInvNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlToInvNo.SelectedIndex != 0 && ddlFromInvNo.SelectedIndex != 0)
        {
            if (Convert.ToInt32(ddlToInvNo.SelectedItem.ToString()) < Convert.ToInt32(ddlFromInvNo.SelectedItem.ToString()))
            {
                lblmsg.Text = "From Inv no must be less than To Inv no";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
    }
    int Numbering()
    {
        int GenGINNO = 0;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT (ISNULL(max(LIM_NO),0)+1) AS LIM_NO FROM LABOR_INVOICE_MASTER where ES_DELETE=0 AND LIM_CM_CODE='" + Session["CompanyCode"].ToString() + "'");
        if (dt.Rows.Count > 0)
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0][0].ToString());
        }
        //else
        //{
        //    GenGINNO = Convert.ToInt32(dt.Rows[0]["EX_NO"]) + 1;
        //}
        return GenGINNO;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (true)
            {
                int flag = 0;
                for (int i = 0; i < dgInvDetails.Rows.Count; i++)
                {
                    CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {
                        flag++;
                    }
                }
                if (flag == 0)
                {
                    lblmsg.Text = "Please Select Item";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }

                bool result = false;
                int LIM_CODE = 0;
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    int LIM_NO = Numbering();
                    //string Inv_NO = "P" + LIM_NO;
                    result = CommonClasses.Execute1("INSERT INTO [LABOR_INVOICE_MASTER] ([LIM_P_CODE] ,[LIM_DATE],[LIM_NO],[LIM_CM_CODE] ) VALUES (" + ddlCustomer.SelectedValue + " ,GETDATE()," + LIM_NO + "," + Session["CompanyCode"].ToString() + ")");
                    DataTable dtMax = CommonClasses.Execute("SELECT MAX(LIM_CODE) AS LIM_CODE FROM LABOR_INVOICE_MASTER");
                    LIM_CODE = Convert.ToInt32(dtMax.Rows[0][0].ToString());
                }
                //result = DL_DBAccess.Insertion_Updation_Delete("EXICSE_ENTRY", Params, out message, out PK_CODE);
                //int BPD_BPM_CODE = PK_CODE;
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    LIM_CODE = Convert.ToInt32(ViewState["mlCode"].ToString());
                    result = CommonClasses.Execute1("DELETE FROM LABOR_INVOICE_DETAIL WHERE LID_LIM_CODE='" + LIM_CODE + "'");
                    if (result)
                    {
                        //result = CommonClasses.Execute1("UPDATE EXICSE_ENTRY SET EX_CM_CODE='" + EX_CM_CODE + "', EX_TYPE='" + EX_TYPE + "',EX_IND='" + EX_IND + "',EX_INV_NO='" + EX_INV_NO + "',EX_INV_DATE='" + EX_INV_DATE + "',EX_NO='" + EX_NO + "',EX_DATE='" + EX_DATE + "',EX_DOC_TYPE='" + EX_DOC_TYPE + "',EX_P_CODE='" + EX_P_CODE + "',EX_SUPP_TYPE='" + EX_SUPP_TYPE + "',EX_BASIC_AMT='" + EX_BASIC_AMT + "',EX_EXCIES_AMT='" + EX_EXCIES_AMT + "',EX_ECESS_AMT='" + EX_ECESS_AMT + "',EX_HECESS_AMT='" + EX_HECESS_AMT + "',EX_TAX_AMT='" + EX_TAX_AMT + "',EX_DISCOUNT_AMT='" + EX_DISCOUNT_AMT + "',EX_NET_AMT='" + EX_NET_AMT + "',EX_FREIGHT='" + EX_FREIGHT + "',EX_PACKING_AMT='" + EX_PACKING_AMT + "',EX_OCTRO_AMT='" + EX_OCTRO_AMT + "',EX_G_AMT='" + EX_G_AMT + "',EX_DOC_NO='" + EX_DOC_NO + "',EX_DOC_DATE='" + EX_DOC_DATE + "',EX_BANK_AMT='" + EX_BANK_AMT + "',EX_EX_DUTY='" + EX_EX_DUTY + "',EX_EX_CESS='" + EX_EX_CESS + "',EX_EX_HCESS='" + EX_EX_HCESS + "',EX_ADDDUTY='" + EX_ADDDUTY + "',EX_IS_CUSTREJ='" + EX_IS_CUSTREJ + "',EX_T_PER='" + EX_T_PER + "',EX_GATE_NO='" + EX_GATE_NO + "',EX_GATE_DATE='" + EX_GATE_DATE + "',EX_S_DUTY='" + EX_S_DUTY + "', EX_S_CESS='" + EX_S_CESS + "',EX_S_HCESS='" + EX_S_HCESS + "',EX_INSURANCE_AMT='" + EX_INSURANCE_AMT + "',EX_OTHER_AMT='" + EX_OTHER_AMT + "',EX_TRANSPORT_AMT='" + EX_TRANSPORT_AMT + "'  WHERE EX_CODE='" + EX_CODE + "'");
                    }
                }

                for (int i = 0; i < dgInvDetails.Rows.Count; i++)
                {


                    string LID_LIM_CODE, LID_INM_CODE, LID_INM_NO, LID_INM_DATE, LID_I_CODE, LID_QTY, LID_RATE, LID_AMT, LID_SAC_CODE, LID_CGST_AMT, LID_SGST_AMT, LID_IGST_AMT, LID_TOTAL;

                    LID_LIM_CODE = LIM_CODE.ToString();
                    LID_INM_CODE = ((Label)dgInvDetails.Rows[i].FindControl("lblINM_CODE")).Text;
                    LID_INM_NO = ((Label)dgInvDetails.Rows[i].FindControl("lblINM_NO")).Text;
                    LID_INM_DATE = Convert.ToDateTime( ((Label)dgInvDetails.Rows[i].FindControl("lblINM_DATE")).Text).ToString("dd/MMM/yyyy");
                    LID_I_CODE = ((Label)dgInvDetails.Rows[i].FindControl("lblI_CODE")).Text;
                    LID_QTY = ((Label)dgInvDetails.Rows[i].FindControl("lblIND_INQTY")).Text;
                    LID_RATE = ((Label)dgInvDetails.Rows[i].FindControl("lblIND_RATE")).Text;
                    LID_AMT = ((Label)dgInvDetails.Rows[i].FindControl("lblIND_AMT")).Text;
                    LID_SAC_CODE = ((Label)dgInvDetails.Rows[i].FindControl("lblE_CODE")).Text;
                    LID_CGST_AMT = ((Label)dgInvDetails.Rows[i].FindControl("lblIND_EX_AMT")).Text;
                    LID_SGST_AMT = ((Label)dgInvDetails.Rows[i].FindControl("lblIND_E_CESS_AMT")).Text;
                    LID_IGST_AMT = ((Label)dgInvDetails.Rows[i].FindControl("lblIND_SH_CESS_AMT")).Text;
                    LID_TOTAL = ((Label)dgInvDetails.Rows[i].FindControl("lblTotal")).Text;


                    //UpdatePanel 
                    //CommonClasses.Execute("Update INWARD_DETAIL set IWD_MODVAT_FLG=0 where IWD_I_CODE='" + EXD_I_CODE + "' and IWD_IWM_CODE='" + EXD_IWM_CODE + "'");
                    CommonClasses.Execute("Update INVOICE_DETAIL set IND_LID_FLAG=0 WHERE IND_I_CODE='" + LID_I_CODE + "' AND IND_INM_CODE='" + LID_INM_CODE + "'");
                    CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[i].FindControl("chkSelect"))) as CheckBox);
                    if (chkRow.Checked)
                    {

                        //result = CommonClasses.Execute1("INSERT INTO EXCISE_DETAIL(EXD_EX_CODE,EXD_IWM_CODE,EXD_SPOM_CODE,EXD_I_CODE,EXD_RECD_QTY,EXD_OK_QTY,EXD_RATE,EXD_AMT,EXD_DISC_AMT,EXD_EXC_PER,EXD_EXC_AMT,EXD_EDU_PER,EXD_EDU_AMT,EXD_HSEDU_PER,EXD_HSEDU_AMT,EXD_T_CODE,EXD_T_PER,EXD_T_AMT,EXD_G_RATE,EXD_G_AMT,EXD_G_CESS,EXD_G_SHCESS) VALUES ('" + EXD_EX_CODE + "','" + EXD_IWM_CODE + "','" + EXD_SPOM_CODE + "','" + EXD_I_CODE + "','" + EXD_RECD_QTY + "','" + EXD_OK_QTY + "','" + EXD_RATE + "','" + EXD_AMT + "','" + EXD_DISC_AMT + "','" + EXD_EXC_PER + "','" + EXD_EXC_AMT + "','" + EXD_EDU_PER + "','" + EXD_EDU_AMT + "','" + EXD_HSEDU_PER + "','" + EXD_HSEDU_AMT + "','" + EXD_T_CODE + "','" + EXD_T_PER + "','" + EXD_T_AMT + "','" + EXD_G_RATE + "','" + EXD_G_AMT + "','" + EXD_G_CESS + "','" + EXD_G_SHCESS + "')");
                        result = CommonClasses.Execute1("INSERT INTO [dbo].[LABOR_INVOICE_DETAIL] ([LID_LIM_CODE] ,[LID_INM_CODE] ,[LID_INM_NO] ,[LID_INM_DATE] ,[LID_I_CODE] ,[LID_QTY] ,[LID_RATE] ,[LID_AMT] ,[LID_SAC_CODE] ,[LID_CGST_AMT] ,[LID_SGST_AMT] ,[LID_IGST_AMT] ,[LID_TOTAL]) VALUES ('" + LID_LIM_CODE + "','" + LID_INM_CODE + "','" + LID_INM_NO + "','" +  LID_INM_DATE + "','" + LID_I_CODE + "','" + LID_QTY + "','" + LID_RATE + "','" + LID_AMT + "','" + LID_SAC_CODE + "','" + LID_CGST_AMT + "','" + LID_SGST_AMT + "','" + LID_IGST_AMT + "','" + LID_TOTAL + "')");
                        // Update Query To Update the flag in INward Master
                        if (result == true)
                        {
                            //Update Inward Detail Flag
                            CommonClasses.Execute("Update INVOICE_DETAIL set IND_LID_FLAG=1 WHERE IND_I_CODE='" + LID_I_CODE + "' AND IND_INM_CODE='" + LID_INM_CODE + "'");
                        }
                    }
                    else
                    {

                    }
                }
                CommonClasses.WriteLog("Labout Tax Invoice", "Save", "Tax Invoice", Convert.ToString(LIM_CODE), Convert.ToInt32(LIM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                //result = true;
                //((DataTable)ViewState["dt2"]).Rows.Clear();
                Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoiceNew.aspx", false);
            }
        }
        catch (Exception)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoiceNew.aspx", false);
    }

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvDetails.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_P_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_INQTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Total", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("E_TARIFF_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("E_CODE", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvDetails.DataSource = dtFilter;
                dgInvDetails.DataBind();
                dgInvDetails.Enabled = false;
            }
        }
        else
        {
            dgInvDetails.Enabled = true;
        }
    }
    #endregion
}
