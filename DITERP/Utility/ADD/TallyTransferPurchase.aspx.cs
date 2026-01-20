using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Utility_ADD_TallyTransferPurchase : System.Web.UI.Page
{
    DataTable dtFilter = new DataTable();
    static DataTable DtInvDet = new DataTable();
    DataRow dr;
    static int mlCode = 0;

    #region Page_Load
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility");
        home1.Attributes["class"] = "active";

        if (!IsPostBack)
        {
            LoadBillNo();
            if (Request.QueryString[0].Equals("VIEW"))
            {
                mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                ViewRec("VIEW");
            }
            else
            {
                ddlSupplier.Enabled = false;
                ddlFromInvNo.Enabled = false;
                ddlToInvNo.Enabled = false;
                txtInvFromDate.Enabled = false;
                txtInvToDate.Enabled = false;
                ddlStatus.Enabled = false;
                chkAllSupplier.Checked = true;
                chkAllInv.Checked = true;
                chkAllStatus.Checked = true;
                chkDateAll.Checked = true;
                txtDocDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                txtDocDate.Attributes.Add("readonly", "readonly");
                txtInvFromDate.Attributes.Add("readonly", "readonly");
                txtInvToDate.Attributes.Add("readonly", "readonly");
                LoadFilter();
            }
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER WHERE SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and SUPP_PO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
            ddlSupplier.DataSource = custdet;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos(string type)
    {
        try
        {
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,SUPP_PO_MASTER WHERE SPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POTYPE='" + type + "'     ORDER BY P_NAME ");
            ddlSupplier.DataSource = custdet;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        dtFilter.Clear();

        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("BPM_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("BPM_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("BPM_DATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("BPM_BASIC_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("BPM_IS_TALL_TRANS", typeof(String)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgBillDetails.DataSource = dtFilter;
            dgBillDetails.DataBind();

            dgBillDetails.Enabled = false;
        }
    }
    #endregion

    #region LoadBillNo
    private void LoadBillNo()
    {
        try
        {
            DataTable FromBill = new DataTable();
            FromBill = CommonClasses.Execute("select distinct BPM_CODE,BPM_NO FROM BILL_PASSING_MASTER WHERE  BPM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 ORDER BY BPM_NO ");
            ddlFromInvNo.DataSource = FromBill;
            ddlFromInvNo.DataTextField = "BPM_NO";
            ddlFromInvNo.DataValueField = "BPM_CODE";
            ddlFromInvNo.DataBind();
            ddlFromInvNo.Items.Insert(0, new ListItem("Select Bill No", "0"));

            ddlToInvNo.DataSource = FromBill;
            ddlToInvNo.DataTextField = "BPM_NO";
            ddlToInvNo.DataValueField = "BPM_CODE";
            ddlToInvNo.DataBind();
            ddlToInvNo.Items.Insert(0, new ListItem("Select Bill No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "LoadBillNo", Ex.Message);
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            txtInvFromDate.Enabled = false;
            txtInvToDate.Enabled = false;
        }
        else
        {
            txtInvFromDate.Enabled = true;
            txtInvToDate.Enabled = true;
        }
    }
    #endregion

    #region chkAllInv_CheckedChanged
    protected void chkAllInv_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllInv.Checked == true)
        {
            ddlFromInvNo.Enabled = false;
            ddlToInvNo.Enabled = false;
        }
        else
        {
            ddlFromInvNo.Enabled = true;
            ddlToInvNo.Enabled = true;
            ddlInvType_OnSelectedIndexChanged(null, null);
        }
    }
    #endregion

    #region chkAllSupplier_CheckedChanged
    protected void chkAllSupplier_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllSupplier.Checked == true)
        {
            if (ddlInvType.SelectedValue == "1")
            {
                LoadCombos("0");
            }
            else if (ddlInvType.SelectedValue == "2")
            {
                LoadCombos("1");
            }
            ddlSupplier.Enabled = false;
        }
        else
        {
            if (ddlInvType.SelectedValue == "1")
            {
                LoadCombos("0");
            }
            else if (ddlInvType.SelectedValue == "2")
            {
                LoadCombos("1");
            }
            ddlSupplier.Enabled = true;
        }
    }
    #endregion

    #region chkAllStatus_CheckedChanged
    protected void chkAllStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllStatus.Checked == true)
        {
            ddlStatus.Enabled = false;
        }
        else
        {
            ddlStatus.Enabled = true;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnLoad_Click
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkAllInv.Checked == false && (ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0))
            {
                lblmsg.Text = "Please Select Invoice From & To";
                PanelMsg.Visible = true;
            }
            if (chkDateAll.Checked == false && (txtInvFromDate.Text == "" || txtInvToDate.Text == ""))
            {
                lblmsg.Text = "Please Select Bill From Date & To date";
                PanelMsg.Visible = true;
            }
            if (chkAllSupplier.Checked == false && ddlSupplier.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Supplier";
                PanelMsg.Visible = true;
            }
            if (chkAllStatus.Checked == false && ddlStatus.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Status";
                PanelMsg.Visible = true;
            }
            string Query = "";
            if (ddlInvType.SelectedIndex == 1)
            {
                Query = "select BPM_CODE,BPM_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,CAST(BPM_G_AMT AS NUMERIC(10,2)) AS BPM_BASIC_AMT,BPM_P_CODE as P_CODE,P_NAME,(CASE BPM_IS_TALL_TRANS WHEN 'FALSE' THEN 'PENDING' ELSE 'TRANSFERED' END) AS BPM_IS_TALL_TRANS FROM BILL_PASSING_MASTER,PARTY_MASTER WHERE BPM_P_CODE=P_CODE AND BPM_TYPE='IWIM' AND BILL_PASSING_MASTER.ES_DELETE=0   ";
            }
            else
            {
                Query = "select BPM_CODE,BPM_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,CAST(BPM_G_AMT AS NUMERIC(10,2)) AS BPM_BASIC_AMT,BPM_P_CODE as P_CODE,P_NAME,(CASE BPM_IS_TALL_TRANS WHEN 'FALSE' THEN 'PENDING' ELSE 'TRANSFERED' END) AS BPM_IS_TALL_TRANS FROM BILL_PASSING_MASTER,PARTY_MASTER WHERE BPM_P_CODE=P_CODE AND BPM_TYPE='OUTCUSTINV' AND BILL_PASSING_MASTER.ES_DELETE=0  ";
            }
            if (chkAllInv.Checked != true)
            {
                Query = Query + " AND BPM_CODE BETWEEN '" + ddlFromInvNo.SelectedValue + "' AND '" + ddlToInvNo.SelectedValue + "'";
            }
            if (chkDateAll.Checked != true)
            {
                Query = Query + " AND BPM_DATE BETWEEN '" + Convert.ToDateTime(txtInvFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(txtInvToDate.Text).ToString("dd/MMM/yyyy") + "'";
            }
            if (chkAllSupplier.Checked != true)
            {
                Query = Query + " AND BPM_P_CODE='" + ddlSupplier.SelectedValue + "'";
            }
            if (chkAllStatus.Checked != true)
            {
                Query = Query + " AND BPM_IS_TALL_TRANS='" + ddlStatus.SelectedIndex + "'";
            }
            Query = Query + "   ORDER BY BPM_NO";

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count == 0)
            {
                LoadFilter();
            }
            else
            {
                dgBillDetails.DataSource = dt;
                dgBillDetails.DataBind();
                dgBillDetails.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

    double TDSPER = 0;

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (DtInvDet.Columns.Count == 0)
            {
                DtInvDet.Columns.Add("BPM_CODE");
                DtInvDet.Columns.Add("BPM_NO");
                DtInvDet.Columns.Add("BPM_DATE");
                DtInvDet.Columns.Add("P_CODE");
                DtInvDet.Columns.Add("P_NAME");
                DtInvDet.Columns.Add("BPM_BASIC_AMT");
                DtInvDet.Columns.Add("BPM_IS_TALL_TRANS");
            }
            DtInvDet.Rows.Clear();
            for (int j = 0; j < dgBillDetails.Rows.Count; j++)
            {
                CheckBox chkRow = (((CheckBox)(dgBillDetails.Rows[j].FindControl("chkSelect"))) as CheckBox);
                if (chkRow.Checked)
                {
                    dr = DtInvDet.NewRow();
                    dr["BPM_CODE"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblBPM_CODE"))).Text;
                    dr["BPM_NO"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblBPM_NO"))).Text;
                    dr["BPM_DATE"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblBPM_DATE"))).Text;
                    dr["P_CODE"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblP_CODE"))).Text;
                    dr["P_NAME"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblP_NAME"))).Text;
                    dr["BPM_BASIC_AMT"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblBPM_BASIC_AMT"))).Text;
                    dr["BPM_IS_TALL_TRANS"] = ((Label)(dgBillDetails.Rows[j].FindControl("lblBPM_IS_TALL_TRANS"))).Text;
                    DtInvDet.Rows.Add(dr);
                }
            }
            bool result = false;

            try
            {
                #region Saving Data To Master and Detail table
                result = SaveRec();
                #endregion

                if (result == true)
                {
                    #region Tally Details
                    //Getting Basic Required Data //Company Name
                    DataTable dtCompanyName = CommonClasses.Execute("SELECT CM_NAME FROM COMPANY_MASTER WHERE CM_ID='" + Session["CompanyId"] + "'");
                    string CompanyName = dtCompanyName.Rows[0]["CM_NAME"].ToString();
                    //finantial year
                    DataTable dtFinaYear = CommonClasses.Execute("select (Cast((YEAR(CM_OPENING_DATE)%100) as char(2))+'- '+ Cast((YEAR(CM_CLOSING_DATE)%100) as char(4))) As FinaYear from COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"] + "'");
                    string FinaYear = dtFinaYear.Rows[0]["FinaYear"].ToString();
                    XmlTextWriter writer = new XmlTextWriter(Server.MapPath("~/Purchase.xml"), System.Text.Encoding.UTF8);
                    writer.WriteStartElement("ENVELOPE");
                    writer.WriteStartElement("HEADER");
                    writer.WriteRaw("<TALLYREQUEST>Import Data</TALLYREQUEST>");
                    writer.WriteEndElement();
                    writer.WriteStartElement("BODY");
                    writer.WriteStartElement("IMPORTDATA");
                    writer.WriteStartElement("REQUESTDESC");
                    writer.WriteRaw("<REPORTNAME>Vouchers</REPORTNAME>");
                    writer.WriteStartElement("STATICVARIABLES");
                    writer.WriteRaw("<SVCURRENTCOMPANY>" + CompanyName + ".</SVCURRENTCOMPANY>");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteStartElement("REQUESTDATA");
                    writer.WriteStartElement("TALLYMESSAGE");
                    writer.WriteAttributeString("xmlns:UDF", "TallyUDF");
                    for (int i = 0; i < DtInvDet.Rows.Count; i++)
                    {
                        //InvoiceNo
                        int InvNo = Convert.ToInt32(DtInvDet.Rows[i]["BPM_NO"]);
                        string invdate = Convert.ToDateTime(DtInvDet.Rows[i]["BPM_DATE"]).ToString("yyyyMMdd");
                        //Voucher Name
                        string voucherid = "";
                        if (ddlInvType.SelectedIndex == 1)
                        {
                            voucherid = "PV/" + FinaYear + "-TI-" + InvNo.ToString().PadLeft(8, '0') + "";
                        }
                        else
                        {
                            voucherid = "EXPV/" + FinaYear + "-TI-" + InvNo.ToString().PadLeft(8, '0') + "";
                        }
                        //Bill Grand Total
                        string ERROR = "";
                        DataTable DtGrandTotal = CommonClasses.Execute("SELECT CAST(BPM_G_AMT AS NUMERIC(10,2)) AS BPM_G_AMT,BPM_INV_NO, ISNULL(BPM_ACCESS_AMT,0) AS BPM_ACCESS_AMT,	BPM_INV_DATE FROM BILL_PASSING_MASTER WHERE ES_DELETE=0 AND BPM_CODE=" + DtInvDet.Rows[i]["BPM_CODE"] + "");
                        if (DtGrandTotal.Rows.Count < 0)
                        {
                            ERROR = "1";
                        }
                        string invNO = DtGrandTotal.Rows[0]["BPM_INV_NO"].ToString();
                        string invDATE = Convert.ToDateTime(DtGrandTotal.Rows[0]["BPM_INV_DATE"]).ToString("yyyyMMdd");
                     
                        DataTable Dtnaration = CommonClasses.Execute("  SELECT  ('GRN No '+convert(varchar,IWM_NO)+'  PROJECT CODE  '+PROCM_NAME+'  PO NO '+SPOM_PONO +' BILL NO '+convert(varchar,BPM_INV_NO)+'  / '+convert(varchar,BPM_INV_DATE,105)+' '+I_CODENO+' '+I_NAME+'  QTY ' +CONVERT(varchar, ROUND(IWD_CON_OK_QTY,2 ))+'  RATE '+CONVERT(varchar, ROUND(BPD_RATE,2 )) +'  HSN No '+ EXCISE_TARIFF_MASTER.E_TARIFF_NO +'  SAC No '+ SACMASTER.E_TARIFF_NO ) As Narration   FROM BILL_PASSING_MASTER,BILL_PASSING_DETAIL,ITEM_MASTER,INWARD_MASTER,INWARD_DETAIL,SUPP_PO_MASTER  , PROJECT_CODE_MASTER,EXCISE_TARIFF_MASTER,EXCISE_TARIFF_MASTER AS SACMASTER   WHERE BPM_CODE=BPD_BPM_CODE AND BPD_I_CODE=I_CODE and BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'  AND IWM_CODE=IWD_IWM_CODE AND BPD_IWM_CODE=IWM_CODE AND BPD_I_CODE=IWD_I_CODE   AND SUPP_PO_MASTER.SPOM_CODE=IWD_CPOM_CODE   AND    EXCISE_TARIFF_MASTER.E_CODE=I_E_CODE AND SACMASTER.e_code=I_SCAT_CODE AND  SPOM_PROJECT=PROJECT_CODE_MASTER.PROCM_CODE");

                        DataTable dtgIN = CommonClasses.Execute(" SELECT DISTINCT   IWM_NO As IWM_NO   FROM BILL_PASSING_MASTER,BILL_PASSING_DETAIL,ITEM_MASTER,INWARD_MASTER,INWARD_DETAIL,SUPP_PO_MASTER  , PROJECT_CODE_MASTER   WHERE BPM_CODE=BPD_BPM_CODE AND BPD_I_CODE=I_CODE and BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'  AND IWM_CODE=IWD_IWM_CODE AND BPD_IWM_CODE=IWM_CODE AND BPD_I_CODE=IWD_I_CODE   AND SUPP_PO_MASTER.SPOM_CODE=IWD_CPOM_CODE   AND SPOM_PROJECT=PROJECT_CODE_MASTER.PROCM_CODE");
                        //TALLY NAME
                        DataTable dtTalllyName = CommonClasses.Execute("SELECT P_CODE,P_TALLY, ISNULL(P_TDS,0) AS P_TDS ,  SUBSTRING(RTRIM(LTRIM(P_PAN)),4,1) AS P_PAN FROM PARTY_MASTER,BILL_PASSING_MASTER WHERE BPM_P_CODE=P_CODE AND BILL_PASSING_MASTER.ES_DELETE=0 and BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'");
                        if (dtTalllyName.Rows.Count > 0)
                        {
                            if (dtTalllyName.Rows[0]["P_PAN"].ToString().ToUpper() == "P")
                            {
                                TDSPER = 1;
                            }
                            else
                            {
                                TDSPER = 2;
                            }
                        }
                        else
                        {
                            TDSPER = 2;
                        }
                        DataTable dtAMTDeclaration = CommonClasses.Execute(" SELECT T1.TALLY_NAME AS ITEM_TALLY_NAME,BPM_TAX_CODE,0 AS SALETAX_TALLY_NAME,Round(isnull(BPM_TAX_AMT,0),2) as BPM_TAX_AMT,SUM(isnull(BPD_AMT,0)) as BPD_AMT,cast(isnull(BPD_DISC_AMT,0) as numeric(20,2)) as BPM_DISCOUNT_AMT,cast(isnull(BPM_PACKING_AMT,0) as numeric(20,2)) as BPM_PACKING_AMT,cast(isnull(BPM_OTHER_AMT,0) as numeric(20,2)) as BPM_OTHER_AMT,cast(isnull(BPM_FREIGHT,0) as numeric(20,2)) as BPM_FREIGHT,cast(isnull(BPM_INSURRANCE,0) as numeric(20,2)) as BPM_INSURRANCE,cast(isnull(BPM_TRANSPORT,0) as numeric(20,2)) as BPM_TRANSPORT,cast(isnull(BPM_OCTRO_AMT,0) as numeric(20,2)) as BPM_OCTRO_AMT,cast(isnull(BPM_ADD_DUTY,0) as numeric(20,2)) as BPM_ADD_DUTY ,ISNULL(BPM_EXCPER,0) AS  BPM_EXCPER ,ISNULL(BPM_EXCEDCESS_PER,0) AS BPM_EXCEDCESS_PER,ISNULL(BPM_EXCHIEDU_PER,0) AS BPM_EXCHIEDU_PER FROM BILL_PASSING_DETAIL,BILL_PASSING_MASTER,ITEM_MASTER,TALLY_MASTER T1  WHERE BPD_BPM_CODE = BPM_CODE AND BPD_I_CODE = I_CODE AND ITEM_MASTER.I_ACCOUNT_PURCHASE = T1.TALLY_CODE   AND BILL_PASSING_MASTER.ES_DELETE=0  AND BPD_BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "' AND BPM_CM_CODE='" + Session["CompanyCode"] + "'  group by T1.TALLY_NAME ,BPM_TAX_CODE,BPM_TAX_AMT,BPD_DISC_AMT, BPM_PACKING_AMT,BPM_OTHER_AMT,BPM_FREIGHT,BPM_INSURRANCE,BPM_TRANSPORT,BPM_OCTRO_AMT,BPM_ADD_DUTY,  BPM_EXCPER , BPM_EXCEDCESS_PER, BPM_EXCHIEDU_PER");

                        //Other Tally Account
                        DataTable dtOtherAccounts = CommonClasses.Execute("SELECT T1.TALLY_NAME AS DISC_TALLY_NAME,T2.TALLY_NAME AS PACK_TALLY_NAME,T3.TALLY_NAME AS OTHER_TALLY_NAME,T4.TALLY_NAME AS FRIGHT_TALLY_NAME,T5.TALLY_NAME AS INSUR_TALLY_NAME,T6.TALLY_NAME AS TRANS_TALLY_NAME,T7.TALLY_NAME AS OCTRI_TALLY_NAME,T8.TALLY_NAME AS TCS_TALLY_NAME,T9.TALLY_NAME AS ADV_TALLY_NAME FROM TALLY_MASTER T1,TALLY_MASTER T2,TALLY_MASTER T3,TALLY_MASTER T4,TALLY_MASTER T5,TALLY_MASTER T6,TALLY_MASTER T7,TALLY_MASTER T8,TALLY_MASTER T9,TALLY_MASTER T10,TALLY_MASTER T11,SYSTEM_CONFIG_SETTING WHERE  SCS_DISC_TALLY_CODE=T1.TALLY_CODE AND SCS_PACKAMT_TALLY_CODE=T2.TALLY_CODE AND SCS_OTHER_TALLY_CODE=T3.TALLY_CODE AND SCS_FREIGHT_TALLY_CODE=T4.TALLY_CODE AND SCS_INSUR_TALLY_CODE=T5.TALLY_CODE AND SCS_TRANBS_TAYYLY_CODE=T6.TALLY_CODE AND SCS_OCTRI_TALLY_CODE=T7.TALLY_CODE AND SCS_TCS_TALLY_CODE=T8.TALLY_CODE AND SCS_ADV_TALLY_CODE=T9.TALLY_CODE GROUP BY T1.TALLY_NAME,T2.TALLY_NAME,T3.TALLY_NAME,T4.TALLY_NAME,T5.TALLY_NAME,T6.TALLY_NAME,T7.TALLY_NAME,T8.TALLY_NAME,T9.TALLY_NAME");

                        writer.WriteStartElement("VOUCHER");
                        writer.WriteAttributeString("REMOTEID", "" + voucherid + "");

                        if (ddlInvType.SelectedIndex == 1)
                        {
                            writer.WriteAttributeString("VCHTYPE", "Purchase");
                        }
                        else
                        {
                            writer.WriteAttributeString("VCHTYPE", "Expenses Offloading");
                        }
                        if (rbtOperation.SelectedIndex == 0)
                            writer.WriteAttributeString("Action", "Create");
                        else
                            writer.WriteAttributeString("Action", "Alter");

                        writer.WriteRaw("<DATE>" + invdate + "</DATE>");
                        writer.WriteRaw("<GUID>" + voucherid + "</GUID>");

                        string naration = "";
                        for (int k = 0; k < Dtnaration.Rows.Count; k++)
                        {
                            naration = naration + Dtnaration.Rows[k]["Narration"].ToString() + " | ";
                        }
                        string GIN = "";
                        for (int p = 0; p < dtgIN.Rows.Count; p++)
                        {
                            GIN = GIN + dtgIN.Rows[p]["IWM_NO"].ToString() + " , ";
                        }

                        GIN = GIN.Remove(GIN.Length - 2, 2);
                        naration = naration.Replace("&", "&amp;");
                        writer.WriteRaw("<NARRATION>" + naration + "</NARRATION>");

                        if (ddlInvType.SelectedIndex == 1)
                        {
                            writer.WriteRaw("<VOUCHERTYPENAME>Purchase</VOUCHERTYPENAME>");
                        }
                        else
                        {
                            writer.WriteRaw("<VOUCHERTYPENAME>Expenses Offloading</VOUCHERTYPENAME>");
                        }
                        string voucherNo = "";
                        if (ddlInvType.SelectedIndex == 1)
                        {
                            voucherNo = "PV/" + FinaYear.Trim() + "-" + InvNo + "";
                        }
                        else
                        {
                            voucherNo = "EXPV/" + FinaYear.Trim() + "-" + InvNo + "";
                        }
                        writer.WriteRaw("<VOUCHERNUMBER>" + voucherNo + "</VOUCHERNUMBER>");
                        //writer.WriteRaw("<REFERENCE> GIN No." + GIN + "</REFERENCE>");

                        writer.WriteRaw("<REFERENCE> Inv No." + invNO + "</REFERENCE>");

                        string P_NAMES = dtTalllyName.Rows[0]["P_TALLY"].ToString();

                        writer.WriteRaw("<PARTYLEDGERNAME>" + P_NAMES.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");

                        writer.WriteRaw("<CSTFORMISSUETYPE/>");
                        writer.WriteRaw("<CSTFORMRECVTYPE/>");
                        writer.WriteRaw("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                        writer.WriteRaw("<VCHGSTCLASS/>");
                        writer.WriteRaw("<ENTEREDBY>1962</ENTEREDBY>");
                        writer.WriteRaw("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
                        writer.WriteRaw("<AUDITED>No</AUDITED>");
                        writer.WriteRaw("<FORJOBCOSTING>No</FORJOBCOSTING>");
                        writer.WriteRaw("<ISOPTIONAL>No</ISOPTIONAL>");
                        writer.WriteRaw("<EFFECTIVEDATE>" + invdate + "</EFFECTIVEDATE>");
                        writer.WriteRaw("<USEFORINTEREST>No</USEFORINTEREST>");
                        writer.WriteRaw("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
                        writer.WriteRaw("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                        writer.WriteRaw("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
                        writer.WriteRaw("<ALTERID>" + InvNo + "</ALTERID>");
                        writer.WriteRaw("<EXCISEOPENING>No</EXCISEOPENING>");
                        writer.WriteRaw("<ISCANCELLED>No</ISCANCELLED>");
                        writer.WriteRaw("<HASCASHFLOW>No</HASCASHFLOW>");
                        writer.WriteRaw("<ISPOSTDATED>No</ISPOSTDATED>");
                        writer.WriteRaw("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                        writer.WriteRaw("<ISINVOICE>No</ISINVOICE>");
                        writer.WriteRaw("<MFGJOURNAL>No</MFGJOURNAL>");
                        writer.WriteRaw("<HASDISCOUNTS>No</HASDISCOUNTS>");
                        writer.WriteRaw("<ASPAYSLIP>No</ASPAYSLIP>");
                        writer.WriteRaw("<ISDELETED>No</ISDELETED>");
                        writer.WriteRaw("<ASORIGINAL>Yes</ASORIGINAL>");
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + P_NAMES.Replace("&", "&amp;") + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        double TDSAmt = 0;
                        double NetAmt = 0;

                        double newTDS = 0;
                        if (ddlInvType.SelectedIndex == 1)
                        {
                            if (Convert.ToDouble(dtTalllyName.Rows[0]["P_TDS"]).ToString().ToUpper() != "0")
                            {
                                newTDS = Math.Round(Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_ACCESS_AMT"].ToString()) * Convert.ToDouble(dtTalllyName.Rows[0]["P_TDS"].ToString()) / 100, 2);
                            }
                        }
                        if (ddlInvType.SelectedIndex == 1)
                        {
                            writer.WriteRaw("<AMOUNT>" + (Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_G_AMT"].ToString()) - newTDS).ToString() + "</AMOUNT>");
                        }
                        else
                        {
                            TDSAmt = Math.Round(Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_ACCESS_AMT"].ToString()) * TDSPER / 100, 2);
                            NetAmt = Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_G_AMT"].ToString()) - TDSAmt;
                            writer.WriteRaw("<AMOUNT>" + Math.Round(NetAmt, 2) + "</AMOUNT>");
                        }
                        writer.WriteRaw("<BILLALLOCATIONS.LIST>");
                        writer.WriteRaw("<NAME>" + invNO + "/" + Convert.ToDateTime(DtGrandTotal.Rows[0]["BPM_INV_DATE"]).ToString("dd-MM-yyyy") + "</NAME>");
                        writer.WriteRaw("<BILLTYPE>New Ref</BILLTYPE>");
                        if (ddlInvType.SelectedIndex == 1)
                        {
                           // writer.WriteRaw("<AMOUNT>" + DtGrandTotal.Rows[0]["BPM_G_AMT"].ToString() + "</AMOUNT>");
                            writer.WriteRaw("<AMOUNT>" + (Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_G_AMT"].ToString()) - newTDS).ToString() + "</AMOUNT>");
                        }
                        else
                        {
                            TDSAmt = Math.Round(Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_ACCESS_AMT"].ToString()) * TDSPER / 100, 2);
                            NetAmt = Convert.ToDouble(DtGrandTotal.Rows[0]["BPM_G_AMT"].ToString()) - TDSAmt;
                            writer.WriteRaw("<AMOUNT>" + Math.Round(NetAmt, 2) + "</AMOUNT>");
                        }
                        writer.WriteRaw("</BILLALLOCATIONS.LIST>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");

                        for (int g = 0; g < dtAMTDeclaration.Rows.Count; g++)
                        {
                            //Item Tally Name And Basic Amount
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            if (ddlInvType.SelectedIndex == 1)
                            {
                                string ITEM_TALLY_NAME = dtAMTDeclaration.Rows[g]["ITEM_TALLY_NAME"].ToString();
                                writer.WriteRaw("<LEDGERNAME>" + ITEM_TALLY_NAME.Replace("&", "&amp;") + "</LEDGERNAME>");
                            }
                            else
                            {
                                writer.WriteRaw("<LEDGERNAME>Offloading Charges (Foundry)</LEDGERNAME>");
                            }
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>-" + Math.Round(Convert.ToDouble(dtAMTDeclaration.Rows[g]["BPD_AMT"].ToString()), 2).ToString() + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }

                        #region Excise Detail
                        //Basic Exc Details
                        DataTable DtBasicExcDetail = CommonClasses.Execute("SELECT top 1 BPD_I_CODE,I_NAME,'NA' AS BASICEXC_TALLY_NAME,E_TALLY_BASIC,cast(ISnull(BPM_EXCIES_AMT,0) as numeric(20,2)) as BPM_EXCIES_AMT FROM BILL_PASSING_DETAIL,BILL_PASSING_MASTER,ITEM_MASTER, EXCISE_TARIFF_MASTER WHERE BPD_BPM_CODE = BPM_CODE AND BPD_I_CODE = I_CODE AND I_E_CODE=E_CODE   AND BILL_PASSING_MASTER.ES_DELETE=0 AND BPM_CM_CODE='" + Session["CompanyCode"] + "' AND BPD_BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'");

                        //Basic Exc Details
                        DataTable DtEduExcDetail = CommonClasses.Execute("SELECT top 1 BPD_I_CODE,I_NAME,'NA' AS EDUTALLY_NAME,E_TALLY_EDU,cast(ISnull(BPM_ECESS_AMT,0) as numeric(20,2)) as BPM_ECESS_AMT FROM BILL_PASSING_DETAIL,BILL_PASSING_MASTER,ITEM_MASTER, EXCISE_TARIFF_MASTER WHERE BPD_BPM_CODE = BPM_CODE AND BPD_I_CODE = I_CODE AND I_E_CODE=E_CODE   AND BILL_PASSING_MASTER.ES_DELETE=0 AND BPM_CM_CODE='" + Session["CompanyCode"] + "' AND BPD_BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'");

                        //Basic Exc Details
                        DataTable DtHighExcDetail = CommonClasses.Execute("SELECT top 1 BPD_I_CODE,I_NAME,'NA' AS SHEDU_TALLY_NAME,E_TALLY_H_EDU,cast(ISnull(BPM_HECESS_AMT,0) as numeric(20,2)) as BPM_HECESS_AMT FROM BILL_PASSING_DETAIL,BILL_PASSING_MASTER,ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE BPD_BPM_CODE = BPM_CODE AND BPD_I_CODE = I_CODE AND I_E_CODE=E_CODE    AND BILL_PASSING_MASTER.ES_DELETE=0 AND BPM_CM_CODE='" + Session["CompanyCode"] + "' AND BPD_BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'");

                        for (int a = 0; a < DtBasicExcDetail.Rows.Count; a++)
                        {
                            if (Convert.ToDouble(DtBasicExcDetail.Rows[a]["BPM_EXCIES_AMT"]) > 0)
                            {
                                //Basic Excise Tally Name And Amount
                                writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                                if (ddlInvType.SelectedIndex == 1 || ddlInvType.SelectedIndex == 2)
                                {
                                    writer.WriteRaw("<LEDGERNAME>CGST ON PURCHASES " + dtAMTDeclaration.Rows[0]["BPM_EXCPER"].ToString() + "  % </LEDGERNAME>");
                                }
                                else
                                {
                                    writer.WriteRaw("<LEDGERNAME>CGST ON SERVICES " + dtAMTDeclaration.Rows[0]["BPM_EXCPER"].ToString() + "  % </LEDGERNAME>");
                                }
                                writer.WriteRaw("<GSTCLASS/>");
                                writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                                writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                                writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                                writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                                writer.WriteRaw("<AMOUNT>-" + Math.Round(Convert.ToDouble(DtBasicExcDetail.Rows[a]["BPM_EXCIES_AMT"].ToString()), 2).ToString() + "</AMOUNT>");
                                writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                            }
                        }
                        for (int b = 0; b < DtEduExcDetail.Rows.Count; b++)
                        {
                            if (Convert.ToDouble(DtEduExcDetail.Rows[b]["BPM_ECESS_AMT"]) > 0)
                            {
                                //Education Excise Tally Name And Amount
                                writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                                if (ddlInvType.SelectedIndex == 1 || ddlInvType.SelectedIndex == 2)
                                {
                                    writer.WriteRaw("<LEDGERNAME>SGST ON PURCHASES " + dtAMTDeclaration.Rows[0]["BPM_EXCEDCESS_PER"].ToString() + "  % </LEDGERNAME>");
                                }
                                else
                                {
                                    writer.WriteRaw("<LEDGERNAME>SGST ON SERVICES " + dtAMTDeclaration.Rows[0]["BPM_EXCEDCESS_PER"].ToString() + "  % </LEDGERNAME>");
                                }
                                writer.WriteRaw("<GSTCLASS/>");
                                writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                                writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                                writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                                writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                                writer.WriteRaw("<AMOUNT>-" + Math.Round(Convert.ToDouble(DtEduExcDetail.Rows[b]["BPM_ECESS_AMT"].ToString()), 2).ToString() + "</AMOUNT>");
                                writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                            }
                        }
                        for (int c = 0; c < DtHighExcDetail.Rows.Count; c++)
                        {
                            if (Convert.ToDouble(DtHighExcDetail.Rows[c]["BPM_HECESS_AMT"]) > 0)
                            {
                                //Higher Educational Excise Tally Name And Amount
                                writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                                if (ddlInvType.SelectedIndex == 1 || ddlInvType.SelectedIndex == 2)
                                {
                                    writer.WriteRaw("<LEDGERNAME>IGST ON PURCHASES " + dtAMTDeclaration.Rows[0]["BPM_EXCHIEDU_PER"].ToString() + "  % </LEDGERNAME>");
                                }
                                else
                                {
                                    writer.WriteRaw("<LEDGERNAME>IGST ON SERVICES " + dtAMTDeclaration.Rows[0]["BPM_EXCHIEDU_PER"].ToString() + "  % </LEDGERNAME>");
                                }
                                writer.WriteRaw("<GSTCLASS/>");
                                writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                                writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                                writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                                writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                                writer.WriteRaw("<AMOUNT>-" + Math.Round(Convert.ToDouble(DtHighExcDetail.Rows[c]["BPM_HECESS_AMT"].ToString()), 2).ToString() + "</AMOUNT>");
                                writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                            }
                        }
                        #endregion

                        #region Packaging
                        //Packaging Tally Name And Amount
                        if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["BPM_PACKING_AMT"]) > 0)
                        {
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            writer.WriteRaw("<LEDGERNAME>Packing Material</LEDGERNAME>");
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>-" + dtAMTDeclaration.Rows[0]["BPM_PACKING_AMT"].ToString() + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }
                        #endregion

                        #region TDS
                        //Packaging Tally Name And Amount
                        if (newTDS > 0)
                        {
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            writer.WriteRaw("<LEDGERNAME>TDS PAYABLE 194Q</LEDGERNAME>");
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>NO</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>" + newTDS + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }
                        #endregion

                        #region Sales Tax
                        //Sales Tax Tally Name And Amount
                        if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["BPM_TAX_AMT"]) > 0)
                        {
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            writer.WriteRaw("<LEDGERNAME>" + dtAMTDeclaration.Rows[0]["SALETAX_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>-" + dtAMTDeclaration.Rows[0]["BPM_TAX_AMT"].ToString() + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }
                        #endregion

                        #region TDS  
                        //Sales Tax Tally Name And Amount  FOR Offloading only
                        if (ddlInvType.SelectedIndex == 2)
                        {
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            //writer.WriteRaw("<LEDGERNAME>TDS Payable (Contractors-194C)2011-12</LEDGERNAME>");
                            writer.WriteRaw("<LEDGERNAME>TDS Pbl(194C - Works Contract)</LEDGERNAME>");
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>" + Math.Round(TDSAmt, 2) + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }
                        #endregion

                        #region Insurance
                        //Insurance Tally Name And Amount
                        if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["BPM_INSURRANCE"]) > 0)
                        {
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["INSUR_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>-" + dtAMTDeclaration.Rows[0]["BPM_INSURRANCE"].ToString() + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }
                        #endregion

                        #region BPM_FREIGHT
                        //Insurance Tally Name And Amount
                        if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["BPM_FREIGHT"]) > 0)
                        {
                            writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            writer.WriteRaw("<LEDGERNAME>Freight Inward</LEDGERNAME>");
                            writer.WriteRaw("<GSTCLASS/>");
                            writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                            writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            writer.WriteRaw("<AMOUNT>-" + dtAMTDeclaration.Rows[0]["BPM_FREIGHT"].ToString() + "</AMOUNT>");
                            writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }
                        #endregion

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    #endregion

                    writer.Close();
                    DtInvDet.Rows.Clear();
                    txtDocDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                    txtDocNo.Text = "";
                    ddlFromInvNo.SelectedIndex = 0;
                    ddlToInvNo.SelectedIndex = 0;
                    ddlSupplier.SelectedIndex = 0;
                    txtInvFromDate.Text = "";
                    txtInvToDate.Text = "";
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

    protected void lb_DownloadXML_Click(object sender, EventArgs e)
    {
        string strFullPath = Server.MapPath("~/Purchase.xml");
        string strContents = null;
        System.IO.StreamReader objReader = default(System.IO.StreamReader);
        objReader = new System.IO.StreamReader(strFullPath);
        strContents = objReader.ReadToEnd();
        objReader.Close();
        string attachment = "attachment; filename=Purchase.xml";
        Response.ClearContent();
        Response.ContentType = "application/xml";
        Response.AddHeader("content-disposition", attachment);
        Response.Write(strContents);
        Response.End();
    }

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            int Tal_Tran_No = 0;
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("Select isnull(max(TTM_NO),0) as TTM_NO FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and TTM_TYPE=1");
            if (dt.Rows.Count > 0)
            {
                Tal_Tran_No = Convert.ToInt32(dt.Rows[0]["TTM_NO"]);
                Tal_Tran_No = Tal_Tran_No + 1;
            }
            if (CommonClasses.Execute1("INSERT INTO TALLY_TRANSFER_MASTER (TTM_COMP_CODE,TTM_NO,TTM_DATE,TTM_TYPE,TTM_ENTRY_TYPE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Tal_Tran_No + "','" + Convert.ToDateTime(txtDocDate.Text).ToString("dd/MMM/yyyy") + "','1'," + rbtOperation.SelectedIndex + ")"))
            {
                string Code = CommonClasses.GetMaxId("Select Max(TTM_CODE) from TALLY_TRANSFER_MASTER");
                for (int i = 0; i < DtInvDet.Rows.Count; i++)
                {
                    result = CommonClasses.Execute1("INSERT INTO TALLY_TRANSFER_DETAIL (TTD_TTM_CODE,TTD_INM_CODE,TTD_INM_DATE,TTD_INM_P_CODE,TTD_INM_NET_AMT,TTD_INM_STATUS) values ('" + Code + "','" + DtInvDet.Rows[i]["BPM_CODE"] + "','" + Convert.ToDateTime(DtInvDet.Rows[i]["BPM_DATE"]).ToString("dd/MMM/yyyy") + "','" + DtInvDet.Rows[i]["P_CODE"] + "','" + DtInvDet.Rows[i]["BPM_BASIC_AMT"] + "','" + DtInvDet.Rows[i]["BPM_IS_TALL_TRANS"] + "')");

                    if (result == true)
                    {
                        result = CommonClasses.Execute1("UPDATE BILL_PASSING_MASTER SET BPM_IS_TALL_TRANS = '1' WHERE BPM_CODE='" + DtInvDet.Rows[i]["BPM_CODE"] + "'");
                    }
                }
                CommonClasses.WriteLog("Tally Trasnfer Purchase", "Save", "Tally Trasnfer Purchase", Convert.ToString(Tal_Tran_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ViewRec

    private void ViewRec(string str)
    {
        DtInvDet.Rows.Clear();
        try
        {
            dt = CommonClasses.Execute("select TTM_CODE,TTM_NO,convert(varchar,TTM_DATE,106) as TTM_DATE,TTM_ENTRY_TYPE FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE='" + Session["CompanyCode"] + "' and TTM_CODE=" + mlCode + " and TTM_TYPE=1");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["TTM_CODE"]);
                txtDocNo.Text = dt.Rows[0]["TTM_NO"].ToString();
                txtDocDate.Text = dt.Rows[0]["TTM_DATE"].ToString();
                rbtOperation.SelectedIndex = Convert.ToInt32(dt.Rows[0]["TTM_ENTRY_TYPE"]);
                DataTable dtDetails = CommonClasses.Execute("select TTD_INM_CODE as BPM_CODE,BPM_NO,CONVERT(varchar,TTD_INM_DATE,106) as BPM_DATE,TTD_INM_P_CODE As P_CODE,P_NAME,cast(TTD_INM_NET_AMT as numeric(10,2)) as BPM_BASIC_AMT,TTD_INM_STATUS as BPM_IS_TALL_TRANS from TALLY_TRANSFER_DETAIL,TALLY_TRANSFER_MASTER,BILL_PASSING_MASTER,PARTY_MASTER where TTD_TTM_CODE=TTM_CODE and TTD_INM_CODE=BPM_CODE and TTD_INM_P_CODE=P_CODE and TALLY_TRANSFER_MASTER.ES_DELETE=0");
                if (dtDetails.Rows.Count != 0)
                {
                    dgBillDetails.DataSource = dtDetails;
                    dgBillDetails.DataBind();
                }
                else
                {
                    dtDetails.Rows.Add(dtDetails.NewRow());
                    dgBillDetails.DataSource = dtDetails;
                    dgBillDetails.DataBind();
                }
                if (str == "VIEW")
                {
                    txtDocNo.Enabled = false;
                    txtDocDate.Enabled = false;
                    ddlFromInvNo.Enabled = false;
                    ddlToInvNo.Enabled = false;
                    ddlSupplier.Enabled = false;
                    txtInvFromDate.Enabled = false;
                    txtInvToDate.Enabled = false;
                    ddlStatus.Enabled = false;
                    rbtOperation.Enabled = false;
                    dgBillDetails.Enabled = false;
                    btnExport.Enabled = false;
                    btnLoad.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Purchase", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Tally Transfer Purchase", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ddlInvType_TextChanged
    protected void ddlInvType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable FromInv = new DataTable();

            if (ddlInvType.SelectedValue == "1")
            {
                FromInv = CommonClasses.Execute("select distinct BPM_CODE,BPM_NO FROM BILL_PASSING_MASTER WHERE  BPM_CM_CODE=" + Session["CompanyCode"].ToString() + " AND  BPM_TYPE='IWIM'  and ES_DELETE=0 ORDER BY BPM_NO ");
                LoadCombos("0");
            }
            else if (ddlInvType.SelectedValue == "2")
            {
                FromInv = CommonClasses.Execute("select distinct BPM_CODE,BPM_NO FROM BILL_PASSING_MASTER WHERE  BPM_CM_CODE=" + Session["CompanyCode"].ToString() + " AND  BPM_TYPE='OUTCUSTINV'  and ES_DELETE=0 ORDER BY BPM_NO ");
                LoadCombos("1");
            }
            ddlFromInvNo.DataSource = FromInv;
            ddlFromInvNo.DataTextField = "BPM_NO";
            ddlFromInvNo.DataValueField = "BPM_CODE";
            ddlFromInvNo.DataBind();
            ddlFromInvNo.Items.Insert(0, new ListItem("Select Bill Passing No", "0"));

            ddlToInvNo.DataSource = FromInv;
            ddlToInvNo.DataTextField = "BPM_NO";
            ddlToInvNo.DataValueField = "BPM_CODE";
            ddlToInvNo.DataBind();
            ddlToInvNo.Items.Insert(0, new ListItem("Select Bill Passing No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Tranfer Purchase", "ddlInvType_OnSelectedIndexChanged", Ex.Message.ToString());
        }
    }
    #endregion
}
