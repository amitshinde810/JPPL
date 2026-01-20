using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Utility_TallyTransSales_ : System.Web.UI.Page
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
            LoadCombos();
            string str = "0";

            if (str == "1")
            {
                mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                ViewRec("VIEW");
            }
            else
            {
                ddlCustomer.Enabled = false;
                ddlFromInvNo.Enabled = false;
                ddlToInvNo.Enabled = false;
                txtInvFromDate.Enabled = false;
                txtInvToDate.Enabled = false;
                ddlStatus.Enabled = false;
                chkAllCustomer.Checked = true;
                chkAllInv.Checked = true;
                chkAllStatus.Checked = true;
                chkDateAll.Checked = true;
                txtDocDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                LoadFilter();
                ddlInvType_OnSelectedIndexChanged(null, null);
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
            custdet = CommonClasses.Execute("select distinct P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " and CUSTPO_MASTER.ES_DELETE=0  ORDER BY P_NAME ");
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

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvDetails.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_NET_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INM_IS_TALLY_TRANS", typeof(String)));
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
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            ddlCustomer.Enabled = false;
        }
        else
        {
            ddlCustomer.Enabled = true;
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

    #region ddlInvType_TextChanged
    protected void ddlInvType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable FromInv = new DataTable();
            if (ddlInvType.SelectedValue == "1")
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='TAXINV' AND INM_SUPPLEMENTORY=0 ORDER BY INM_NO ");
            }
            else if (ddlInvType.SelectedValue == "2")
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='OutJWINM' AND    INM_INV_TYPE=0 ORDER BY INM_NO ");
            }
            else if (ddlInvType.SelectedValue == "3")
            {
                FromInv = CommonClasses.Execute("select distinct LIM_CODE AS INM_CODE, LIM_NO AS INM_NO FROM LABOR_INVOICE_MASTER WHERE  LIM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0   ORDER BY LIM_NO ");
            }
            else
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='TAXINV' AND INM_SUPPLEMENTORY=1 ORDER BY INM_NO ");
            }
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
            CommonClasses.SendError("Tally Tranfer Sales", "ddlInvType_OnSelectedIndexChanged", Ex.Message.ToString());
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
            CommonClasses.SendError("tally Transfer Sales", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnLoad_Click
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlInvType.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Invoice Type";
                PanelMsg.Visible = true;
            }
            if (chkAllInv.Checked == false && (ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0))
            {
                lblmsg.Text = "Please Select Invoice From & To";
                PanelMsg.Visible = true;
            }
            if (chkDateAll.Checked == false && (txtInvFromDate.Text == "" || txtInvToDate.Text == ""))
            {
                lblmsg.Text = "Please Select Invoice From Date & To date";
                PanelMsg.Visible = true;
            }
            if (chkAllCustomer.Checked == false && ddlCustomer.SelectedIndex == 0)
            {
                lblmsg.Text = "Please Select Customer";
                PanelMsg.Visible = true;
            }
            string condition = "";
            string conditionL = "";
            if (chkAllInv.Checked != true)
            {
                condition = condition + " INM_CODE BETWEEN '" + ddlFromInvNo.SelectedValue + "' AND '" + ddlToInvNo.SelectedValue + "'  AND  ";
                conditionL = conditionL + " LIM_CODE BETWEEN '" + ddlFromInvNo.SelectedValue + "' AND '" + ddlToInvNo.SelectedValue + "'  AND  ";
            }
            if (chkDateAll.Checked != true)
            {
                condition = condition + "  INM_DATE BETWEEN '" + Convert.ToDateTime(txtInvFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(txtInvToDate.Text).ToString("dd/MMM/yyyy") + "'   AND  ";
                conditionL = conditionL + "  LIM_DATE BETWEEN '" + Convert.ToDateTime(txtInvFromDate.Text).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(txtInvToDate.Text).ToString("dd/MMM/yyyy") + "'   AND  ";

            }
            if (chkAllCustomer.Checked != true)
            {
                condition = condition + " INM_P_CODE = '" + ddlCustomer.SelectedValue + "'   AND  ";
                conditionL = conditionL + " LIM_P_CODE = '" + ddlCustomer.SelectedValue + "'   AND  ";
            }
            if (chkAllStatus.Checked != true)
            {
                condition = condition + "  INM_IS_TALLY_TRANS='" + ddlStatus.SelectedIndex + "'   AND  ";

            }
            string Query = "";
            if (ddlInvType.SelectedValue == "1")
            {
                Query = "select INM_CODE,INM_TNO AS INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,CAST(INM_G_AMT AS NUMERIC(20,2)) AS INM_NET_AMT,INM_P_CODE as P_CODE,P_NAME,(CASE isnull(INM_IS_TALLY_TRANS,'False') WHEN 'FALSE' THEN 'PENDING' ELSE 'TRANSFERED' END) AS INM_IS_TALLY_TRANS FROM INVOICE_MASTER,PARTY_MASTER WHERE " + condition + "  INM_P_CODE=P_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='TAXINV'  AND INM_SUPPLEMENTORY=0 and INM_CM_CODE='" + Session["CompanyCode"] + "'";
            }
            else if (ddlInvType.SelectedValue == "2")
            {
                Query = "select INM_CODE,INM_TNO AS INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,CAST(INM_G_AMT AS NUMERIC(20,2)) AS INM_NET_AMT,INM_P_CODE as P_CODE,P_NAME,(CASE isnull(INM_IS_TALLY_TRANS,'False') WHEN 'FALSE' THEN 'PENDING' ELSE 'TRANSFERED' END) AS INM_IS_TALLY_TRANS FROM INVOICE_MASTER,PARTY_MASTER WHERE " + condition + "  INM_P_CODE=P_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutJWINM' and INM_CM_CODE='" + Session["CompanyCode"] + "'  and INM_INV_TYPE<>'1'";
            }
            else if (ddlInvType.SelectedValue == "3")
            {
                Query = "select LIM_CODE AS INM_CODE,LIM_NO AS INM_NO,convert(varchar,LIM_DATE,106) as INM_DATE,CAST(SUM(LID_TOTAL ) AS NUMERIC(20,2)) AS INM_NET_AMT,LIM_P_CODE as P_CODE,P_NAME,  'PENDING'   AS INM_IS_TALLY_TRANS FROM  LABOR_INVOICE_MASTER,LABOR_INVOICE_DETAIL,PARTY_MASTER WHERE " + conditionL + "  LIM_P_CODE=P_CODE AND LABOR_INVOICE_MASTER.ES_DELETE=0 AND   LIM_CODE=LID_LIM_CODE and LIM_CM_CODE='" + Session["CompanyCode"] + "'";
            }
            else
            {
                Query = "select INM_CODE,INM_TNO AS INM_NO ,convert(varchar,INM_DATE,106) as INM_DATE,CAST(INM_G_AMT AS NUMERIC(20,2)) AS INM_NET_AMT,INM_P_CODE as P_CODE,P_NAME,(CASE isnull(INM_IS_TALLY_TRANS,'False') WHEN 'FALSE' THEN 'PENDING' ELSE 'TRANSFERED' END) AS INM_IS_TALLY_TRANS FROM INVOICE_MASTER,PARTY_MASTER WHERE " + condition + "  INM_P_CODE=P_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='TAXINV' AND INM_SUPPLEMENTORY=1  and INM_CM_CODE='" + Session["CompanyCode"] + "'";
            }

            if (ddlInvType.SelectedValue == "3")
            {
                Query = Query + "  GROUP BY  LIM_CODE,LIM_NO ,LIM_DATE,LIM_P_CODE,P_NAME ORDER BY LIM_CODE";
            }
            else
            {
                Query = Query + " ORDER BY INM_CODE";
            }
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count == 0)
            {
                LoadFilter();
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

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (DtInvDet.Columns.Count == 0)
            {
                DtInvDet.Columns.Add("INM_CODE");
                DtInvDet.Columns.Add("INM_NO");
                DtInvDet.Columns.Add("INM_DATE");
                DtInvDet.Columns.Add("P_CODE");
                DtInvDet.Columns.Add("P_NAME");
                DtInvDet.Columns.Add("INM_NET_AMT");
                DtInvDet.Columns.Add("INM_IS_TALLY_TRANS");
            }
            DtInvDet.Rows.Clear();
            for (int j = 0; j < dgInvDetails.Rows.Count; j++)
            {
                CheckBox chkRow = (((CheckBox)(dgInvDetails.Rows[j].FindControl("chkSelect"))) as CheckBox);
                if (chkRow.Checked && dgInvDetails.Enabled)
                {
                    dr = DtInvDet.NewRow();
                    dr["INM_CODE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_CODE"))).Text;
                    dr["INM_NO"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_NO"))).Text;
                    dr["INM_DATE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblIWM_DATE"))).Text;
                    dr["P_CODE"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblP_CODE"))).Text;
                    dr["P_NAME"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblP_NAME"))).Text;
                    dr["INM_NET_AMT"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblINM_NET_AMT"))).Text;
                    dr["INM_IS_TALLY_TRANS"] = ((Label)(dgInvDetails.Rows[j].FindControl("lblINM_IS_TALLY_TRANS"))).Text;
                    DtInvDet.Rows.Add(dr);
                }
            }
            bool result = false;
            #region Saving Data To Master and Detail table
            if (DtInvDet.Rows.Count > 0)
            {
                result = SaveRec();
            }
            #endregion
            if (result == true)
            {
                #region Tally Details
                //Getting Basic Required Data

                //Company Name                
                string CompanyName = Session["CompanyName"].ToString();
                //finantial year
                // DataTable dtFinaYear = CommonClasses.Execute("select (Cast((YEAR(CM_OPENING_DATE)%100) as char(4))+'- '+ Cast((YEAR(CM_CLOSING_DATE)%100) as char(4))) As FinaYear from COMPANY_MASTER where CM_CODE='" + Session["CompanyCode"] + "'");
                string FinaYear = Session["CompanyFinancialYr"].ToString();

                string startyear = Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).Year.ToString().Substring(2, 2);

                string Endyear = Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).Year.ToString().Substring(2, 2);

                string findate = startyear + "_" + Endyear;
                //Session["CompanyClosingDate"].ToString();
                //Invoice No
                string path = "";
                XmlTextWriter writer = new XmlTextWriter(Server.MapPath("~/Sales.xml"), System.Text.Encoding.UTF8);
                writer.WriteStartElement("ENVELOPE");
                writer.WriteStartElement("HEADER");
                writer.WriteRaw("<TALLYREQUEST>Import Data</TALLYREQUEST>");
                writer.WriteEndElement();
                writer.WriteStartElement("BODY");
                writer.WriteStartElement("IMPORTDATA");
                writer.WriteStartElement("REQUESTDESC");
                writer.WriteRaw("<REPORTNAME>Voucher</REPORTNAME>");
                writer.WriteStartElement("STATICVARIABLES");
                writer.WriteRaw("<SVCURRENTCOMPANY>" + CompanyName + "</SVCURRENTCOMPANY>");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("REQUESTDATA");
                writer.WriteStartElement("TALLYMESSAGE");
                writer.WriteAttributeString("xmlns:UDF", "TallyUDF");
                for (int i = 0; i < DtInvDet.Rows.Count; i++)
                {
                    //invoiceno
                    string InvNo = DtInvDet.Rows[i]["INM_NO"].ToString();
                    string invdate = Convert.ToDateTime(DtInvDet.Rows[i]["INM_DATE"]).ToString("yyyyMMdd");
                    //voucher name
                    string voucherid = "SMP" + FinaYear + "-TI-" + InvNo.ToString().PadLeft(8, '0') + "";
                    DataTable DtGrandTotal = new DataTable();
                    DataTable Dtnaration = new DataTable();
                    DataTable dtTalllyName = new DataTable();
                    DataTable dtAMTDeclaration = new DataTable();

                    DataTable dtOtherAccounts = new DataTable();
                    DataTable DtInvoiceAll = new DataTable();
                    if (ddlInvType.SelectedValue != "3")
                    {

                        //Invoice Grand Total
                        DtGrandTotal = CommonClasses.Execute("SELECT CAST(INM_G_AMT AS NUMERIC(20,2)) AS INM_G_AMT FROM INVOICE_MASTER WHERE ES_DELETE=0 AND INM_CODE=" + DtInvDet.Rows[i]["INM_CODE"] + "");
                        //Narration
                         Dtnaration = CommonClasses.Execute("SELECT (I_CODENO+' || '+I_NAME+' ||Qty:- '+CAST(CAST(IND_INQTY AS DECIMAL(10, 0)) AS VARCHAR(20))+'||Rate:- '+CAST(CAST(IND_RATE AS DECIMAL(10, 0)) AS VARCHAR(20))) As Narration FROM INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER WHERE INM_CODE=IND_INM_CODE AND IND_I_CODE=I_CODE and INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");
                        //Party TALLY NAME
                         dtTalllyName = CommonClasses.Execute("SELECT P_CODE,P_TALLY,ISNULL(P_TDS,0) AS P_TDS FROM PARTY_MASTER,INVOICE_MASTER WHERE INM_P_CODE=P_CODE AND INVOICE_MASTER.ES_DELETE=0 and INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");
                        //Item Wise Tax And Excise Tally Name and Amt                      
                        dtAMTDeclaration = CommonClasses.Execute("SELECT T1.TALLY_NAME AS ITEM_TALLY_NAME, INVOICE_MASTER.INM_T_CODE, SUM(INVOICE_MASTER.INM_ACCESSIBLE_AMT) AS IND_AMT,  INVOICE_MASTER.INM_S_TAX_AMT, CAST(ISNULL(INVOICE_MASTER.INM_DISC_AMT, 0) AS numeric(20, 2)) AS INM_DISC_AMT,  CAST(ISNULL(INVOICE_MASTER.INM_PACK_AMT, 0) AS numeric(20, 2)) AS INM_PACK_AMT, CAST(ISNULL(INVOICE_MASTER.INM_OTHER_AMT, 0) AS numeric(20, 2))   AS INM_OTHER_AMT, CAST(ISNULL(INVOICE_MASTER.INM_FREIGHT, 0) AS numeric(20, 2)) AS INM_FREIGHT, CAST(ISNULL(INVOICE_MASTER.INM_INSURANCE, 0) AS numeric(20, 2)) AS INM_INSURANCE, CAST(ISNULL(INVOICE_MASTER.INM_TRANS_AMT, 0) AS numeric(20, 2)) AS INM_TRANS_AMT,  CAST(ISNULL(INVOICE_MASTER.INM_OCTRI_AMT, 0) AS numeric(20, 2)) AS INM_OCTRI_AMT, CAST(ISNULL(INVOICE_MASTER.INM_TAX_TCS_AMT, 0) AS numeric(20, 2)) AS INM_TAX_TCS_AMT, CAST(ISNULL(INVOICE_MASTER.INM_ADV_DUTY, 0) AS numeric(20, 2)) AS INM_ADV_DUTY,  CAST(ISNULL(INVOICE_DETAIL.IND_AMORTAMT, 0) AS numeric(20, 2)) AS IND_AMORTAMT FROM INVOICE_DETAIL INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN TALLY_MASTER AS T1 ON ITEM_MASTER.I_ACCOUNT_SALES = T1.TALLY_CODE INNER JOIN EXCISE_TARIFF_MASTER ON INVOICE_DETAIL.IND_E_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE     (INVOICE_MASTER.ES_DELETE = 0) AND (INVOICE_MASTER.INM_CM_CODE = '" + Convert.ToInt32(Session["CompanyCode"]) + "') AND (INVOICE_DETAIL.IND_INM_CODE = '" + DtInvDet.Rows[i]["INM_CODE"] + "') GROUP BY T1.TALLY_NAME, INVOICE_MASTER.INM_T_CODE, INVOICE_MASTER.INM_S_TAX_AMT, INVOICE_MASTER.INM_DISC_AMT, INVOICE_MASTER.INM_PACK_AMT,INVOICE_MASTER.INM_OTHER_AMT, INVOICE_MASTER.INM_FREIGHT, INVOICE_MASTER.INM_INSURANCE, INVOICE_MASTER.INM_INSURANCE, INVOICE_MASTER.INM_TRANS_AMT, INVOICE_MASTER.INM_OCTRI_AMT, INVOICE_MASTER.INM_TAX_TCS_AMT, INVOICE_MASTER.INM_ADV_DUTY, INVOICE_DETAIL.IND_AMORTAMT");
                        //Other Tally Account
                        dtOtherAccounts = CommonClasses.Execute("SELECT T1.TALLY_NAME AS DISC_TALLY_NAME,T2.TALLY_NAME AS PACK_TALLY_NAME,T3.TALLY_NAME AS OTHER_TALLY_NAME,T4.TALLY_NAME AS FRIGHT_TALLY_NAME,T5.TALLY_NAME AS INSUR_TALLY_NAME,T6.TALLY_NAME AS TRANS_TALLY_NAME,T7.TALLY_NAME AS OCTRI_TALLY_NAME,T8.TALLY_NAME AS TCS_TALLY_NAME,T9.TALLY_NAME AS ADV_TALLY_NAME FROM TALLY_MASTER T1,TALLY_MASTER T2,TALLY_MASTER T3,TALLY_MASTER T4,TALLY_MASTER T5,TALLY_MASTER T6,TALLY_MASTER T7,TALLY_MASTER T8,TALLY_MASTER T9,TALLY_MASTER T10,TALLY_MASTER T11,SYSTEM_CONFIG_SETTING WHERE  SCS_DISC_TALLY_CODE=T1.TALLY_CODE AND SCS_PACKAMT_TALLY_CODE=T2.TALLY_CODE AND SCS_OTHER_TALLY_CODE=T3.TALLY_CODE AND SCS_FREIGHT_TALLY_CODE=T4.TALLY_CODE AND SCS_INSUR_TALLY_CODE=T5.TALLY_CODE AND SCS_TRANBS_TAYYLY_CODE=T6.TALLY_CODE AND SCS_OCTRI_TALLY_CODE=T7.TALLY_CODE AND SCS_TCS_TALLY_CODE=T8.TALLY_CODE AND SCS_ADV_TALLY_CODE=T9.TALLY_CODE GROUP BY T1.TALLY_NAME,T2.TALLY_NAME,T3.TALLY_NAME,T4.TALLY_NAME,T5.TALLY_NAME,T6.TALLY_NAME,T7.TALLY_NAME,T8.TALLY_NAME,T9.TALLY_NAME");
                        //All Data
                        if (ddlInvType.SelectedValue == "1")
                        {
                            DtInvoiceAll = CommonClasses.Execute("SELECT INM_CODE,INM_DATE,P_NAME,P_TALLY,TALLY_NAME,INM_NET_AMT,ROUND(INM_BE_AMT,2) AS INM_BE_AMT, ROUND(INM_EDUC_AMT,2) AS INM_EDUC_AMT,  ROUND(INM_H_EDUC_AMT,2) AS INM_H_EDUC_AMT,0 AS SALE_ACC_CODE,'0' AS SALE_ACC_TAXCLASS,INM_T_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_G_AMORT_AMT,INM_PACK_AMT,INM_FREIGHT,INM_DISC_AMT,INM_TEMP_TALLYTNF,INM_ISSUE_DATE,INM_REMOVAL_DATE,I_CODENO,I_NAME,IND_INQTY,IND_RATE,E_BASIC,E_EDU_CESS,E_H_EDU From  ITEM_MASTER, INVOICE_DETAIL, INVOICE_MASTER, PARTY_MASTER,  TALLY_MASTER,EXCISE_TARIFF_MASTER Where IND_I_CODE = I_CODE And INM_P_CODE = P_CODE And IND_INM_CODE = INM_CODE   AND IND_E_CODE=E_CODE AND INM_CODE= '" + DtInvDet.Rows[i]["INM_CODE"] + "' AND INM_TEMP_TALLYTNF = 0 AND INM_TYPE='TAXINV' AND INVOICE_MASTER.ES_DELETE = 0 AND I_ACCOUNT_SALES = TALLY_CODE ORDER BY INM_NO ");
                        }
                        else if (ddlInvType.SelectedValue == "2")
                        {
                            DtInvoiceAll = CommonClasses.Execute("SELECT INM_CODE,INM_DATE,P_NAME,P_TALLY,TALLY_NAME,INM_NET_AMT, ROUND(INM_BE_AMT,2) AS INM_BE_AMT, ROUND(INM_EDUC_AMT,2) AS INM_EDUC_AMT,  ROUND(INM_H_EDUC_AMT,2) AS INM_H_EDUC_AMT,0 AS SALE_ACC_CODE,'0' AS SALE_ACC_TAXCLASS,INM_T_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_G_AMORT_AMT,INM_PACK_AMT,INM_FREIGHT,INM_DISC_AMT,INM_TEMP_TALLYTNF,INM_ISSUE_DATE,INM_REMOVAL_DATE,I_CODENO,I_NAME,IND_INQTY,IND_RATE,E_BASIC,E_EDU_CESS,E_H_EDU From  ITEM_MASTER, INVOICE_DETAIL, INVOICE_MASTER, PARTY_MASTER,  TALLY_MASTER,EXCISE_TARIFF_MASTER Where IND_I_CODE = I_CODE And INM_P_CODE = P_CODE And IND_INM_CODE = INM_CODE   AND IND_E_CODE=E_CODE  AND INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "' AND INM_TEMP_TALLYTNF = 0 AND INM_TYPE='OutJWINM' AND INVOICE_MASTER.ES_DELETE = 0 AND I_ACCOUNT_SALES = TALLY_CODE ORDER BY INM_NO");
                        }
                        else
                        {
                            DtInvoiceAll = CommonClasses.Execute("SELECT INM_CODE,INM_DATE,P_NAME,P_TALLY,TALLY_NAME,INM_NET_AMT,ROUND(INM_BE_AMT,2) AS INM_BE_AMT, ROUND(INM_EDUC_AMT,2) AS INM_EDUC_AMT,  ROUND(INM_H_EDUC_AMT,2) AS INM_H_EDUC_AMT,0 AS SALE_ACC_CODE, '0' AS SALE_ACC_TAXCLASS,INM_T_AMT,INM_G_AMT,INM_TAX_TCS_AMT,INM_G_AMORT_AMT,INM_PACK_AMT,INM_FREIGHT,INM_DISC_AMT,INM_TEMP_TALLYTNF,INM_ISSUE_DATE,INM_REMOVAL_DATE,I_CODENO,I_NAME,IND_INQTY,IND_RATE,E_BASIC,E_EDU_CESS,E_H_EDU From  ITEM_MASTER, INVOICE_DETAIL, INVOICE_MASTER, PARTY_MASTER,  TALLY_MASTER,EXCISE_TARIFF_MASTER Where IND_I_CODE = I_CODE And INM_P_CODE = P_CODE And IND_INM_CODE = INM_CODE   AND IND_E_CODE=E_CODE AND INM_CODE= '" + DtInvDet.Rows[i]["INM_CODE"] + "' AND INM_TEMP_TALLYTNF = 0 AND INM_TYPE='TAXINV' AND INVOICE_MASTER.ES_DELETE = 0 AND I_ACCOUNT_SALES = TALLY_CODE ORDER BY INM_NO ");

                        }
                    }
                    else
                    {
                        //Invoice Grand Total
                        DtGrandTotal = CommonClasses.Execute("SELECT  CAST(SUM(LID_TOTAL ) AS NUMERIC(20,2)) AS INM_G_AMT FROM LABOR_INVOICE_MASTER,LABOR_INVOICE_DETAIL WHERE ES_DELETE=0  AND   LIM_CODE=LID_LIM_CODE AND LIM_CODE=" + DtInvDet.Rows[i]["INM_CODE"] + "");
                        //Narration
                        Dtnaration = CommonClasses.Execute("SELECT (I_CODENO+' || '+I_NAME+' ||Qty:- '+CAST(CAST(LID_QTY AS DECIMAL(10, 0)) AS VARCHAR(20))+'||Rate:- '+CAST(CAST(LID_RATE AS DECIMAL(10, 0)) AS VARCHAR(20))) As Narration FROM  LABOR_INVOICE_MASTER,LABOR_INVOICE_DETAIL,ITEM_MASTER WHERE  LIM_CODE=LID_LIM_CODE AND LID_I_CODE=I_CODE and LIM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");
                        //Party TALLY NAME
                        dtTalllyName = CommonClasses.Execute("SELECT P_CODE,P_TALLY,ISNULL(P_TDS,0) AS P_TDS  FROM PARTY_MASTER,LABOR_INVOICE_MASTER WHERE LIM_P_CODE=P_CODE AND LABOR_INVOICE_MASTER.ES_DELETE=0 and LIM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");
                        //Item Wise Tax And Excise Tally Name and Amt
                        dtAMTDeclaration = CommonClasses.Execute("SELECT T1.TALLY_NAME AS ITEM_TALLY_NAME,  LIM_NO AS INM_T_CODE, SUM(LID_AMT) AS IND_AMT,  0 AS INM_S_TAX_AMT, 0 AS INM_INSURANCE, 0 AS INM_DISC_AMT, 0 AS INM_PACK_AMT, 0     AS INM_OTHER_AMT,  0 AS INM_FREIGHT,  0   AS INM_TRANS_AMT,  0   AS INM_OCTRI_AMT, 0   AS INM_TAX_TCS_AMT,  0   AS INM_ADV_DUTY,   0 AS IND_AMORTAMT FROM LABOR_INVOICE_DETAIL INNER JOIN LABOR_INVOICE_MASTER ON LID_LIM_CODE = LIM_CODE INNER JOIN ITEM_MASTER ON LID_I_CODE = ITEM_MASTER.I_CODE INNER JOIN TALLY_MASTER AS T1 ON ITEM_MASTER.I_ACCOUNT_SALES = T1.TALLY_CODE INNER JOIN EXCISE_TARIFF_MASTER ON   I_E_CODE = EXCISE_TARIFF_MASTER.E_CODE WHERE     (LABOR_INVOICE_MASTER.ES_DELETE = 0) AND (LABOR_INVOICE_MASTER.LIM_CM_CODE = '" + Convert.ToInt32(Session["CompanyCode"]) + "') AND (LID_LIM_CODE = '" + DtInvDet.Rows[i]["INM_CODE"] + "') GROUP BY T1.TALLY_NAME,LIM_NO  ");
                        //Other Tally Account
                        dtOtherAccounts = CommonClasses.Execute("SELECT T1.TALLY_NAME AS DISC_TALLY_NAME,T2.TALLY_NAME AS PACK_TALLY_NAME,T3.TALLY_NAME AS OTHER_TALLY_NAME,T4.TALLY_NAME AS FRIGHT_TALLY_NAME,T5.TALLY_NAME AS INSUR_TALLY_NAME,T6.TALLY_NAME AS TRANS_TALLY_NAME,T7.TALLY_NAME AS OCTRI_TALLY_NAME,T8.TALLY_NAME AS TCS_TALLY_NAME,T9.TALLY_NAME AS ADV_TALLY_NAME FROM TALLY_MASTER T1,TALLY_MASTER T2,TALLY_MASTER T3,TALLY_MASTER T4,TALLY_MASTER T5,TALLY_MASTER T6,TALLY_MASTER T7,TALLY_MASTER T8,TALLY_MASTER T9,TALLY_MASTER T10,TALLY_MASTER T11,SYSTEM_CONFIG_SETTING WHERE  SCS_DISC_TALLY_CODE=T1.TALLY_CODE AND SCS_PACKAMT_TALLY_CODE=T2.TALLY_CODE AND SCS_OTHER_TALLY_CODE=T3.TALLY_CODE AND SCS_FREIGHT_TALLY_CODE=T4.TALLY_CODE AND SCS_INSUR_TALLY_CODE=T5.TALLY_CODE AND SCS_TRANBS_TAYYLY_CODE=T6.TALLY_CODE AND SCS_OCTRI_TALLY_CODE=T7.TALLY_CODE AND SCS_TCS_TALLY_CODE=T8.TALLY_CODE AND SCS_ADV_TALLY_CODE=T9.TALLY_CODE GROUP BY T1.TALLY_NAME,T2.TALLY_NAME,T3.TALLY_NAME,T4.TALLY_NAME,T5.TALLY_NAME,T6.TALLY_NAME,T7.TALLY_NAME,T8.TALLY_NAME,T9.TALLY_NAME");
                        //All Data
                        DtInvoiceAll = CommonClasses.Execute("SELECT LIM_CODE AS INM_CODE, LIM_DATE AS INM_DATE,P_NAME,P_TALLY,MAX(TALLY_NAME) AS  TALLY_NAME,   CAST(SUM(LID_AMT ) AS NUMERIC(20,2)) AS   INM_NET_AMT, ROUND( SUM(LID_CGST_AMT),2) AS INM_BE_AMT, ROUND(SUM(LID_SGST_AMT),2) AS INM_EDUC_AMT,  ROUND(SUM(LID_IGST_AMT),2) AS INM_H_EDUC_AMT,0 AS SALE_ACC_CODE,'0' AS SALE_ACC_TAXCLASS,    0 AS  INM_T_AMT,   0 AS  INM_G_AMT,0 AS INM_TAX_TCS_AMT,0 AS INM_G_AMORT_AMT,0 AS INM_PACK_AMT,0 AS INM_FREIGHT,0 AS INM_INSURANCE,0 AS INM_DISC_AMT,0 AS INM_TEMP_TALLYTNF,LIM_DATE AS INM_ISSUE_DATE,LIM_DATE AS INM_REMOVAL_DATE,'NA' AS I_CODENO,'NA' AS I_NAME,0 AS IND_INQTY,0 AS IND_RATE, MAX(E_BASIC) AS E_BASIC,MAX(E_EDU_CESS) AS E_EDU_CESS,MAX(E_BASIC) AS E_H_EDU From  ITEM_MASTER, LABOR_INVOICE_MASTER,LABOR_INVOICE_DETAIL, PARTY_MASTER,  TALLY_MASTER,EXCISE_TARIFF_MASTER Where LID_I_CODE = I_CODE And LIM_P_CODE = P_CODE And LIM_CODE=LID_LIM_CODE   AND I_SCAT_CODE=E_CODE  AND LIM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'   AND LABOR_INVOICE_MASTER.ES_DELETE = 0 AND I_ACCOUNT_SALES = TALLY_CODE   GROUP BY LIM_CODE,LIM_DATE, P_NAME,P_TALLY    ORDER BY LIM_CODE ");

                    }

                    double TDSAmt = 0;
                    if (ddlInvType.SelectedIndex == 2 || ddlInvType.SelectedIndex == 3)
                    {
                        TDSAmt = Math.Round((Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_NET_AMT"].ToString())) * 2 / 100, 2);
                    }

                    writer.WriteStartElement("VOUCHER");
                    writer.WriteAttributeString("REMOTEID", "" + voucherid + "");
                    if (ddlInvType.SelectedValue == "1")
                        writer.WriteAttributeString("VCHTYPE", "Sales");
                    else if (ddlInvType.SelectedValue == "2")
                        writer.WriteAttributeString("VCHTYPE", "Sales (Labour) Chakan");
                    else if (ddlInvType.SelectedValue == "3")
                        writer.WriteAttributeString("VCHTYPE", "Sales (Labour) Chakan");
                    else
                        writer.WriteAttributeString("VCHTYPE", "Sales Supplementary");
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
                    naration = naration.Replace("&", "&amp;");

                    naration = naration.Remove(naration.Length - 2, 2);
                    writer.WriteRaw("<NARRATION>" + naration + "</NARRATION>");
                    if (ddlInvType.SelectedValue == "1")
                        writer.WriteRaw("<VOUCHERTYPENAME>SALES</VOUCHERTYPENAME>");
                    else if (ddlInvType.SelectedValue == "2")
                        writer.WriteRaw("<VOUCHERTYPENAME>SALES LC</VOUCHERTYPENAME>");
                    else if (ddlInvType.SelectedValue == "3")
                        writer.WriteRaw("<VOUCHERTYPENAME>SALES PDC</VOUCHERTYPENAME>");
                    else
                        writer.WriteRaw("<VOUCHERTYPENAME>SUPPLEMENTARY SALES</VOUCHERTYPENAME>");

                    writer.WriteRaw("<VOUCHERNUMBER>" + InvNo + "/" + findate + "</VOUCHERNUMBER>");
                    writer.WriteRaw("<REFERENCE>" + InvNo + "</REFERENCE>");


                   

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


                    double NewTDS = 0;
                    if (  (dtTalllyName.Rows[0]["P_TDS"]).ToString().ToUpper() != "0")
                    {
                        NewTDS = Math.Round((Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_NET_AMT"].ToString())) * Convert.ToDouble(dtTalllyName.Rows[0]["P_TDS"]) / 100, 2);
                    }

                 
                    writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    writer.WriteRaw("<LEDGERNAME>" + P_NAMES.Replace("&", "&amp;") + "</LEDGERNAME>");
                    writer.WriteRaw("<GSTCLASS/>");
                    writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                    writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    writer.WriteRaw("<ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                    writer.WriteRaw("<AMOUNT>-" + (Convert.ToDouble(DtGrandTotal.Rows[0]["INM_G_AMT"].ToString()) - NewTDS).ToString() + "</AMOUNT>");
                    writer.WriteRaw("<BILLALLOCATIONS.LIST>");
                    writer.WriteRaw("<NAME>" + InvNo + "-" + Convert.ToDateTime(DtInvDet.Rows[i]["INM_DATE"]).ToString("dd-MMM-yyyy") + "</NAME>");
                    writer.WriteRaw("<BILLTYPE>New Ref</BILLTYPE>");
                    writer.WriteRaw("<AMOUNT>-" + (Convert.ToDouble(DtGrandTotal.Rows[0]["INM_G_AMT"].ToString()) - NewTDS).ToString() + "</AMOUNT>");
                    writer.WriteRaw("</BILLALLOCATIONS.LIST>");
                    writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");

                    #region ItemSales
                    //Itemss Sales
                    writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    writer.WriteRaw("<LEDGERNAME>" + DtInvoiceAll.Rows[0]["TALLY_NAME"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                    writer.WriteRaw("<GSTCLASS/>");
                    writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    writer.WriteRaw("<ISPARTYLEDGER>No</ISPARTYLEDGER>");
                    //writer.WriteRaw("<AMOUNT>" + (Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_NET_AMT"].ToString()) + Convert.ToDouble(dtAMTDeclaration.Rows[0]["IND_AMORTAMT"].ToString())).ToString() + "</AMOUNT>");

                    writer.WriteRaw("<AMOUNT>" + (Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_NET_AMT"].ToString())).ToString() + "</AMOUNT>");
                    writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //End Items Sales 
                    #endregion

                    #region Discount
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_DISC_AMT"]) > 0)
                    {

                        //Discount Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["DISC_TALLY_NAME"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_DISC_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region Packing Amount
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_PACK_AMT"]) > 0)
                    {
                        //Packaging Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["PACK_TALLY_NAME"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_PACK_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region OldItemSale
                    //for (int g = 0; g < dtAMTDeclaration.Rows.Count; g++)
                    //{

                    //    //Item Tally Name And Basic Amount
                    //    writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    //    writer.WriteRaw("<LEDGERNAME>" + dtAMTDeclaration.Rows[g]["SALETAX_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                    //    writer.WriteRaw("<GSTCLASS/>");
                    //    writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    //    writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    //    writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    //    writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                    //    writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[g]["IND_AMT"].ToString() + "</AMOUNT>");
                    //    writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //} 
                    #endregion

                    #region ExciseEntry
                    if (Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_BE_AMT"].ToString()) > 0)
                    {


                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        // writer.WriteRaw("<LEDGERNAME>Excise Duty (On Sales)</LEDGERNAME>");
                        writer.WriteRaw("<LEDGERNAME>CGST ON SALES " + DtInvoiceAll.Rows[0]["E_BASIC"].ToString() + "% </LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + DtInvoiceAll.Rows[0]["INM_BE_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }

                    if (Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_EDUC_AMT"].ToString()) > 0)
                    {
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>SGST ON SALES " + DtInvoiceAll.Rows[0]["E_EDU_CESS"].ToString() + "%</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + DtInvoiceAll.Rows[0]["INM_EDUC_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    if (Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_H_EDUC_AMT"].ToString()) > 0)
                    {
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>IGST ON SALES " + DtInvoiceAll.Rows[0]["E_H_EDU"].ToString() + "%</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + DtInvoiceAll.Rows[0]["INM_H_EDUC_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    if (Convert.ToDouble(DtInvoiceAll.Rows[0]["INM_NET_AMT"].ToString()) > 0)
                    {
                        if (ddlInvType.SelectedIndex == 2 || ddlInvType.SelectedIndex == 3)
                        {
                            //writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                            //writer.WriteRaw("<LEDGERNAME>TDS Receivable " + FinaYear + "</LEDGERNAME>");
                            //writer.WriteRaw("<GSTCLASS/>");
                            //writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                            //writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                            //writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                            //writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                            //writer.WriteRaw("<AMOUNT>-" + Math.Round(TDSAmt, 2) + "</AMOUNT>");
                            //writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                        }

                    }
                    #endregion

                    #region OldExcise
                    //Basic Exc Details
                    //DataTable DtBasicExcDetail = CommonClasses.Execute("SELECT top 1 E_TALLY_BASIC,T2.TALLY_NAME AS BASICEXC_TALLY_NAME,cast(Round(ISnull(INM_BE_AMT,0),0) as numeric(20,2)) as IND_EX_AMT FROM INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,TALLY_MASTER T2,EXCISE_TARIFF_MASTER WHERE IND_INM_CODE = INM_CODE AND IND_I_CODE = I_CODE AND  I_E_CODE=E_CODE AND E_TALLY_BASIC=T2.TALLY_CODE AND   INVOICE_MASTER.ES_DELETE=0 AND INM_CM_CODE='" + Session["CompanyCode"] + "' AND IND_INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");

                    //Basic Exc Details
                    //DataTable DtEduExcDetail = CommonClasses.Execute("SELECT top 1  E_TALLY_EDU,T3.TALLY_NAME AS EDUTALLY_NAME,cast(Round(ISnull(INM_EDUC_AMT,0),0) as numeric(20,2)) as IND_E_CESS_AMT FROM INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,TALLY_MASTER T3,EXCISE_TARIFF_MASTER WHERE IND_INM_CODE = INM_CODE AND IND_I_CODE = I_CODE AND  I_E_CODE=E_CODE AND  E_TALLY_EDU=T3.TALLY_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_CM_CODE='" + Session["CompanyCode"] + "' AND IND_INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");

                    //Basic Exc Details
                    //DataTable DtHighExcDetail = CommonClasses.Execute("SELECT top 1 E_TALLY_H_EDU,T4.TALLY_NAME AS SHEDU_TALLY_NAME,cast(Round(ISnull(INM_H_EDUC_AMT,0),0) as numeric(20,2)) as IND_SH_CESS_AMT FROM INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,TALLY_MASTER T4,EXCISE_TARIFF_MASTER WHERE IND_INM_CODE = INM_CODE AND IND_I_CODE = I_CODE AND I_E_CODE=E_CODE AND E_TALLY_H_EDU=T4.TALLY_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_CM_CODE='" + Session["CompanyCode"] + "' AND IND_INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");

                    //for (int a = 0; a < DtBasicExcDetail.Rows.Count; a++)
                    //{
                    //    if (Convert.ToDouble(DtBasicExcDetail.Rows[a]["IND_EX_AMT"]) > 0)
                    //    {
                    //        //Basic Excise Tally Name And Amount
                    //        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    //        writer.WriteRaw("<LEDGERNAME>" + DtBasicExcDetail.Rows[a]["BASICEXC_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                    //        writer.WriteRaw("<GSTCLASS/>");
                    //        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    //        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    //        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    //        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                    //        writer.WriteRaw("<AMOUNT>" + DtBasicExcDetail.Rows[a]["IND_EX_AMT"].ToString() + "</AMOUNT>");
                    //        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //    }

                    //} 
                    #endregion

                    #region OldEduCess
                    //for (int b = 0; b < DtEduExcDetail.Rows.Count; b++)
                    //{
                    //    if (Convert.ToDouble(DtEduExcDetail.Rows[b]["IND_E_CESS_AMT"]) > 0)
                    //    {
                    //        //Education Excise Tally Name And Amount
                    //        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    //        writer.WriteRaw("<LEDGERNAME>" + DtEduExcDetail.Rows[b]["EDUTALLY_NAME"].ToString() + "</LEDGERNAME>");
                    //        writer.WriteRaw("<GSTCLASS/>");
                    //        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    //        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    //        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    //        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                    //        writer.WriteRaw("<AMOUNT>" + DtEduExcDetail.Rows[b]["IND_E_CESS_AMT"].ToString() + "</AMOUNT>");
                    //        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //    }
                    //} 
                    #endregion

                    #region OldHighSales
                    //for (int c = 0; c < DtHighExcDetail.Rows.Count; c++)
                    //{
                    //    if (Convert.ToDouble(DtHighExcDetail.Rows[c]["IND_SH_CESS_AMT"]) > 0)
                    //    {
                    //        //Higher Educational Excise Tally Name And Amount
                    //        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    //        writer.WriteRaw("<LEDGERNAME>" + DtHighExcDetail.Rows[c]["SHEDU_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                    //        writer.WriteRaw("<GSTCLASS/>");
                    //        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    //        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    //        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    //        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                    //        writer.WriteRaw("<AMOUNT>" + DtHighExcDetail.Rows[c]["IND_SH_CESS_AMT"].ToString() + "</AMOUNT>");
                    //        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //    }
                    //} 
                    #endregion

                    #region INM_S_TAX_AMT
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_S_TAX_AMT"]) > 0)
                    {
                        //Sales Tax Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        //writer.WriteRaw("<TAXCLASSIFICATIONNAME>" + dtAMTDeclaration.Rows[0]["SALETAX_TALLY_NAME"].ToString() + "</TAXCLASSIFICATIONNAME>");
                        writer.WriteRaw("<LEDGERNAME>" + dtAMTDeclaration.Rows[0]["SALETAX_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_S_TAX_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region OtherAmount
                    //if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_OTHER_AMT"]) > 0)
                    //{
                    //    //Other Chnrges Tally Name And Amount
                    //    writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    //    writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["OTHER_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                    //    writer.WriteRaw("<GSTCLASS/>");
                    //    writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    //    writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    //    writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    //    writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                    //    writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_OTHER_AMT"].ToString() + "</AMOUNT>");
                    //    writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //} 
                    #endregion

                    #region INM_FREIGHT
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_FREIGHT"]) > 0)
                    {
                        //Freight Chnrges Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["FRIGHT_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_FREIGHT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region INM_INSURANCE
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_INSURANCE"]) > 0)
                    {
                        //Insurance Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["INSUR_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_INSURANCE"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region INM_INSURANCE
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_INSURANCE"]) > 0)
                    {
                        //Transport Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["TRANS_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_TRANS_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region INM_OCTRI_AMT
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_OCTRI_AMT"]) > 0)
                    {
                        //Octri Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["OCTRI_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_OCTRI_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region INM_TAX_TCS_AMT
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_TAX_TCS_AMT"]) > 0)
                    {
                        //TCS Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>TCS ON SCRAP 6CE(2017-18)</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_TAX_TCS_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region NewTDS
                    if (NewTDS > 0)
                    {
                        //TCS Tally Name And Amount 
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>TAX DEDUCTED AT SOURCE 194Q</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>-" +NewTDS + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region IND_AMORTAMT
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["IND_AMORTAMT"]) > 0)
                    {      //Amort Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME> Amortization</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>-" + dtAMTDeclaration.Rows[0]["IND_AMORTAMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion

                    #region INM_TRANS_AMT
                    if (Convert.ToDouble(dtAMTDeclaration.Rows[0]["INM_TRANS_AMT"]) > 0)
                    {
                        //Transport Tally Name And Amount
                        writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                        writer.WriteRaw("<LEDGERNAME>Freight Outward</LEDGERNAME>");
                        writer.WriteRaw("<GSTCLASS/>");
                        writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                        writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_TRANS_AMT"].ToString() + "</AMOUNT>");
                        writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    }
                    #endregion
                    //if (ddlInvType.SelectedIndex == 2)
                    //{

                    //    //Advance Duty Tally Name And Amount
                    //    writer.WriteRaw("<ALLLEDGERENTRIES.LIST>");
                    //    writer.WriteRaw("<LEDGERNAME>" + dtOtherAccounts.Rows[0]["ADV_TALLY_NAME"].ToString() + "</LEDGERNAME>");
                    //    writer.WriteRaw("<GSTCLASS/>");
                    //    writer.WriteRaw("<ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                    //    writer.WriteRaw("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
                    //    writer.WriteRaw("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                    //    writer.WriteRaw("<ISPARTYLEDGER>no</ISPARTYLEDGER>");
                    //    writer.WriteRaw("<AMOUNT>" + dtAMTDeclaration.Rows[0]["INM_ADV_DUTY"].ToString() + "</AMOUNT>");
                    //    writer.WriteRaw("</ALLLEDGERENTRIES.LIST>");
                    //}
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
                //txtDocDate.Text = "";
                txtDocNo.Text = "";
                //ddlInvType.SelectedIndex = 0;
                ddlFromInvNo.SelectedIndex = 0;
                ddlToInvNo.SelectedIndex = 0;
                ddlCustomer.SelectedIndex = 0;
                txtInvFromDate.Text = "";
                txtInvToDate.Text = "";
            }
            else
            {
                ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

    protected void lb_DownloadXML_Click(object sender, EventArgs e)
    {
        string strFullPath = Server.MapPath("~/Sales.xml");
        string strContents = null;
        System.IO.StreamReader objReader = default(System.IO.StreamReader);
        objReader = new System.IO.StreamReader(strFullPath);
        strContents = objReader.ReadToEnd();
        objReader.Close();

        string attachment = "attachment; filename=Sales.xml";
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
            CommonClasses.SendError("Tally Transfer Sales", "CancelRecord", ex.Message.ToString());
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
            dt = CommonClasses.Execute("Select isnull(max(TTM_NO),0) as TTM_NO FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and TTM_TYPE=0");
            if (dt.Rows.Count > 0)
            {
                Tal_Tran_No = Convert.ToInt32(dt.Rows[0]["TTM_NO"]);
                Tal_Tran_No = Tal_Tran_No + 1;
            }
            if (CommonClasses.Execute1("INSERT INTO TALLY_TRANSFER_MASTER (TTM_COMP_CODE,TTM_NO,TTM_DATE,TTM_TYPE,TTM_ENTRY_TYPE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Tal_Tran_No + "','" + Convert.ToDateTime(txtDocDate.Text).ToString("dd/MMM/yyyy") + "','0'," + rbtOperation.SelectedIndex + ")"))
            {

                string Code = CommonClasses.GetMaxId("Select Max(TTM_CODE) from TALLY_TRANSFER_MASTER");
                for (int i = 0; i < DtInvDet.Rows.Count; i++)
                {
                    result = CommonClasses.Execute1("INSERT INTO TALLY_TRANSFER_DETAIL (TTD_TTM_CODE,TTD_INM_CODE,TTD_INM_DATE,TTD_INM_P_CODE,TTD_INM_NET_AMT,TTD_INM_STATUS) values ('" + Code + "','" + DtInvDet.Rows[i]["INM_CODE"] + "','" + Convert.ToDateTime(DtInvDet.Rows[i]["INM_DATE"]).ToString("dd/MMM/yyyy") + "','" + DtInvDet.Rows[i]["P_CODE"] + "','" + DtInvDet.Rows[i]["INM_NET_AMT"] + "','" + DtInvDet.Rows[i]["INM_IS_TALLY_TRANS"] + "')");

                    if (result == true)
                    {
                        result = CommonClasses.Execute1("UPDATE INVOICE_MASTER SET INM_IS_TALLY_TRANS = '1' WHERE INM_CODE='" + DtInvDet.Rows[i]["INM_CODE"] + "'");
                    }

                }
                CommonClasses.WriteLog("Tally Trasnfer Sales", "Save", "Tally Trasnfer Sales", Convert.ToString(Tal_Tran_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "SaveRec", ex.Message);
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
            dt = CommonClasses.Execute("select TTM_CODE,TTM_NO,TTM_TYPE,convert(varchar,TTM_DATE,106) as TTM_DATE,TTM_ENTRY_TYPE FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE='" + Session["CompanyCode"] + "' and TTM_CODE=" + mlCode + " and TTM_TYPE=0");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["TTM_CODE"]);

                txtDocNo.Text = dt.Rows[0]["TTM_NO"].ToString();
                txtDocDate.Text = dt.Rows[0]["TTM_DATE"].ToString();
                ddlInvType.SelectedIndex = Convert.ToInt32(dt.Rows[0]["TTM_TYPE"]);
                ddlInvType_OnSelectedIndexChanged(null, null);
                rbtOperation.SelectedIndex = Convert.ToInt32(dt.Rows[0]["TTM_ENTRY_TYPE"]);

                DataTable dtDetails = CommonClasses.Execute("select TTD_INM_CODE as INM_CODE,INM_NO,CONVERT(varchar,TTD_INM_DATE,106) as INM_DATE,TTD_INM_P_CODE As P_CODE,P_NAME,cast(TTD_INM_NET_AMT as numeric(10,2)) as INM_NET_AMT,TTD_INM_STATUS as INM_IS_TALLY_TRANS from TALLY_TRANSFER_DETAIL,TALLY_TRANSFER_MASTER,INVOICE_MASTER,PARTY_MASTER where TTD_TTM_CODE=TTM_CODE and TTD_INM_CODE=INM_CODE and TTD_INM_P_CODE=P_CODE and TALLY_TRANSFER_MASTER.ES_DELETE=0 and TTM_CODE='" + mlCode + "' GROUP BY TTD_INM_CODE,INM_NO,TTD_INM_DATE,TTD_INM_P_CODE,P_NAME,TTD_INM_NET_AMT,TTD_INM_STATUS ");
                if (dtDetails.Rows.Count != 0)
                {
                    dgInvDetails.DataSource = dtDetails;
                    dgInvDetails.DataBind();
                }
                else
                {
                    dtDetails.Rows.Add(dtDetails.NewRow());
                    dgInvDetails.DataSource = dtDetails;
                    dgInvDetails.DataBind();
                }

                if (str == "VIEW")
                {
                    btnExport.Enabled = false;
                    txtDocNo.Enabled = false;
                    txtDocDate.Enabled = false;
                    ddlInvType.Enabled = false;
                    ddlFromInvNo.Enabled = false;
                    ddlToInvNo.Enabled = false;
                    ddlCustomer.Enabled = false;
                    txtInvFromDate.Enabled = false;
                    txtInvToDate.Enabled = false;
                    ddlStatus.Enabled = false;
                    rbtOperation.Enabled = false;
                    dgInvDetails.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Tally Transfer Sales", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}