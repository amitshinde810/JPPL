using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Transactions_ADD_ExportInvoice : System.Web.UI.Page
{
    #region Variable  
    static DataTable dtInvoiceDetail = new DataTable();
    static int mlCode = 0;
    DataRow dr;
  
    public static int Index = 0;
    static string msg = "";  
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
                        dtInvoiceDetail.Rows.Clear();
                       

                        txtDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtIssueDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtRemovalDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtMFGDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtExpDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtARE1FormDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtBondDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtValidUpTo.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtIECDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtExamDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtLCDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtLRDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");

                        txtDate.Attributes.Add("readonly","readonly");
                        txtIssueDate.Attributes.Add("readonly", "readonly");
                        txtRemovalDate.Attributes.Add("readonly", "readonly");
                        txtMFGDate.Attributes.Add("readonly", "readonly");
                        txtExpDate.Attributes.Add("readonly", "readonly");
                        txtARE1FormDate.Attributes.Add("readonly", "readonly");
                        txtBondDate.Attributes.Add("readonly", "readonly");
                        txtValidUpTo.Attributes.Add("readonly", "readonly");
                        txtIECDate.Attributes.Add("readonly", "readonly");
                        txtExamDate.Attributes.Add("readonly", "readonly");
                        txtLCDate.Attributes.Add("readonly", "readonly");
                        txtLRDate.Attributes.Add("readonly", "readonly");


                        LoadCustomer();
                        LoadCurr();
                        LoadCountry();
                        LoadICode();
                        LoadIName();
                        Loadtax();
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
                            LoadFilter();
                            dgInvoiceAddDetail.Enabled = false;
                            CarryForward();
                        }
                        txtInvoiceNo.Focus();
                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Export Invoice", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Export Invoice", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {   
            LoadCustomer();
            txtDate.Attributes.Add("readonly", "readonly");
            txtIssueDate.Attributes.Add("readonly", "readonly");
            txtRemovalDate.Attributes.Add("readonly", "readonly");
            txtMFGDate.Attributes.Add("readonly", "readonly");
            txtExpDate.Attributes.Add("readonly", "readonly");
            txtARE1FormDate.Attributes.Add("readonly", "readonly");
            txtBondDate.Attributes.Add("readonly", "readonly");
            txtValidUpTo.Attributes.Add("readonly", "readonly");
            txtIECDate.Attributes.Add("readonly", "readonly");
            txtExamDate.Attributes.Add("readonly", "readonly");
            txtLRDate.Attributes.Add("readonly", "readonly");
            txtLCDate.Attributes.Add("readonly", "readonly");

            dtInvoiceDetail.Clear();

            DataTable dtMast = CommonClasses.Execute("Select INM_CODE,INM_CM_CODE,INM_INVOICE_TYPE,INM_NO,INM_DATE,INM_P_CODE,INM_NET_AMT,cast(INM_DISC as numeric(20,2)) as INM_DISC,cast(INM_DISC_AMT as numeric(20,2)) as INM_DISC_AMT,cast(INM_G_AMT as numeric(20,2)) as INM_G_AMT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_BUYER_NAME,INM_BUYTER_ADD,cast(INM_INSURANCE as numeric(20,2)) as INM_INSURANCE,cast(INM_FREIGHT as numeric(20,2)) as INM_FREIGHT,INM_FINAL_DEST,INM_PRE_CARRIAGE,INM_PORT_OF_LOAD,INM_PORT_OF_DISCH,INM_PLACE_OF_DEL,INM_CURR_CODE,INM_CLEARANCE,INM_FLIGHT_NO,INM_MFG_DATE,INM_EXP_DATE,INM_CURR_RATE,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,INM_SHIPMENT,INM_CENVAT_AC_NO,INM_BOND_NO,INM_BOND_DATE,INM_UT1_FILE_NO,INM_FILE_NO,INM_EXPORT_FLAG,INM_VALID_DATE,INM_EXA_BOXES,INM_TOD,INM_TOP,INM_VOY_NO,INM_PLACE_REC,INM_M_NO,INM_NOS_PACK,INM_UN_NO,INM_HAZ,INM_HS_CODENO,INM_CONTA_NO,INM_SEAL_NO,INM_OTS_NO,INM_CONTRY_ORIGIN,INM_COUNTRY_DEST,INM_TRANSPORT_BY,INM_ARE_REMARK,INM_CARRIER_NAME,INM_CARRIER_BOOK_NO,INM_TECH_NAME,INM_OUT_PAKGS,INM_INR_PAKG,INM_SUB_CLASS,INM_UN_PAK_GRP,INM_UN_PAK_CODE,INM_EMS_NO,INM_FLASH_POINT,isnull(INM_MARINE_POLLT,0) as INM_MARINE_POLLT,INM_SHIP_DECLAR,INM_IEC_NO,INM_IEC_DATE,INM_CEN_EXC_REG,INM_DATE_OF_EXAM,INM_SUP_C_EXC_NAME,INM_INSP_C_EXC_NAME,INM_CUST_SEAL_NO,INM_PER_E_NO,INM_NONCARGO_NOPAKG,INM_SHIPP_BILL_NO,cast(INM_PACK_AMT as numeric(20,2)) as INM_PACK_AMT,cast(INM_ACCESSIBLE_AMT as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(INM_BEXCISE as numeric(20,2)) as INM_BEXCISE,cast(INM_BE_AMT as numeric(20,2)) as INM_BE_AMT,cast(INM_EDUC_CESS as numeric(20,2)) as INM_EDUC_CESS,cast(INM_EDUC_AMT as numeric(20,2)) as INM_EDUC_AMT,cast(INM_H_EDUC_CESS as numeric(20,2)) as INM_H_EDUC_CESS,cast(INM_H_EDUC_AMT as numeric(20,2)) as INM_H_EDUC_AMT,cast(INM_ADV_DUTY as numeric(20,2)) as INM_ADV_DUTY,cast(INM_TAXABLE_AMT as numeric(20,2)) as INM_TAXABLE_AMT,INM_T_CODE,cast(INM_S_TAX as numeric(20,2)) as INM_S_TAX,cast(INM_S_TAX_AMT as numeric(20,2)) as INM_S_TAX_AMT,cast(INM_OTHER_AMT as numeric(20,2)) as INM_OTHER_AMT,cast(INM_INSURANCE as numeric(20,2)) as INM_INSURANCE,cast(INM_TRANS_AMT as numeric(20,2)) as INM_TRANS_AMT,cast(INM_OCTRI_AMT as numeric(20,2)) as INM_OCTRI_AMT,cast(INM_TAX_TCS_AMT as numeric(20,2)) as INM_TAX_TCS_AMT,cast(INM_ROUNDING_AMT as numeric(20,2)) as INM_ROUNDING_AMT,isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME,INM_LR_NO,INM_LR_DATE,INM_LC_NO,INM_LC_DATE,INM_TRANSPORT_OWNER,INM_TRANSPORT_ADDRESS from INVOICE_MASTER where ES_DELETE=0 and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_CODE=" + mlCode + "");
            if (dtMast.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dtMast.Rows[0]["INM_CODE"]);
                ddlInvoiceType.SelectedValue = dtMast.Rows[0]["INM_INVOICE_TYPE"].ToString();
                txtInvoiceNo.Text = dtMast.Rows[0]["INM_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = dtMast.Rows[0]["INM_P_CODE"].ToString();
                ddlCustomer_SelectedIndexChanged(null, null);

                txtNetAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_NET_AMT"].ToString()));
                txtDiscPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC"].ToString()));
                txtDiscount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC_AMT"].ToString()));
                txtGrandTotal.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_G_AMT"].ToString()));
                txtIssueDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_ISSUE_DATE"]).ToString("dd MMM yyyy");
                txtRemovalDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_REMOVAL_DATE"]).ToString("dd MMM yyyy");
                txtBuyerName.Text = dtMast.Rows[0]["INM_BUYER_NAME"].ToString();
                chkIsSuppliement.Checked = Convert.ToBoolean(dtMast.Rows[0]["INM_IS_SUPPLIMENT"]);
                txtBuyerAddress.Text = dtMast.Rows[0]["INM_BUYTER_ADD"].ToString();
                txtInsurance.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_INSURANCE"].ToString()));
                txtFreight.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_FREIGHT"].ToString()));
                txtFinalDestination.Text = dtMast.Rows[0]["INM_FINAL_DEST"].ToString();
                txtPreCarriageBy.Text = dtMast.Rows[0]["INM_PRE_CARRIAGE"].ToString();
                txtPortOfLoading.Text = dtMast.Rows[0]["INM_PORT_OF_LOAD"].ToString();
                txtPortOfDischarge.Text = dtMast.Rows[0]["INM_PORT_OF_DISCH"].ToString();
                txtPlaceofDelivery.Text = dtMast.Rows[0]["INM_PLACE_OF_DEL"].ToString();
                ddlCurrency.SelectedValue = dtMast.Rows[0]["INM_CURR_CODE"].ToString();
                txtPlaceofDelivery.Text = dtMast.Rows[0]["INM_PLACE_OF_DEL"].ToString();
                txtFlightNo.Text = dtMast.Rows[0]["INM_FLIGHT_NO"].ToString();
                txtMFGDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_MFG_DATE"]).ToString("dd MMM yyyy");
                txtExpDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_EXP_DATE"]).ToString("dd MMM yyyy");
                txtCurrencyRate.Text = dtMast.Rows[0]["INM_CURR_RATE"].ToString();
                txtAuthSign.Text = dtMast.Rows[0]["INM_AUTHO_SIGN"].ToString();
                txtARE1FormNo.Text = dtMast.Rows[0]["INM_AREA_FORM_NO"].ToString();
                txtARE1FormDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_FORM_DATE"]).ToString("dd MMM yyyy");
                txtShipment.Text = dtMast.Rows[0]["INM_SHIPMENT"].ToString();
                txtCenvatAccountEntryNo.Text = dtMast.Rows[0]["INM_CENVAT_AC_NO"].ToString();
                txtBondNo.Text = dtMast.Rows[0]["INM_BOND_NO"].ToString();
                txtBondDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_BOND_DATE"]).ToString("dd MMM yyyy");
                txtUT1FileNo.Text = dtMast.Rows[0]["INM_UT1_FILE_NO"].ToString();
                txtFileNo.Text = dtMast.Rows[0]["INM_FILE_NO"].ToString();
                ChkExportUnderDutyClaim.Checked = Convert.ToBoolean(dtMast.Rows[0]["INM_EXPORT_FLAG"].ToString());
                txtValidUpTo.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_VALID_DATE"]).ToString("dd MMM yyyy");
                txtExaminedBoxes.Text = dtMast.Rows[0]["INM_EXA_BOXES"].ToString();


                txtTrmOfDel.Text = dtMast.Rows[0]["INM_TOD"].ToString();
                txtTrmOfPay.Text = dtMast.Rows[0]["INM_TOP"].ToString();
                txtVoyageNo.Text = dtMast.Rows[0]["INM_VOY_NO"].ToString();
                txtPlaceOfPreCarriage.Text = dtMast.Rows[0]["INM_PLACE_REC"].ToString();
                txtMarksNos.Text = dtMast.Rows[0]["INM_M_NO"].ToString();
                txtNoKindPkg.Text = dtMast.Rows[0]["INM_NOS_PACK"].ToString();
                txtUnNo.Text = dtMast.Rows[0]["INM_UN_NO"].ToString();
                txtHASClass.Text = dtMast.Rows[0]["INM_HAZ"].ToString();
                txtHSCodeNo.Text = dtMast.Rows[0]["INM_HS_CODENO"].ToString();
                txtContainer.Text = dtMast.Rows[0]["INM_CONTA_NO"].ToString();
                txtBottleSealNo.Text = dtMast.Rows[0]["INM_SEAL_NO"].ToString();
                txtOTSNo.Text = dtMast.Rows[0]["INM_OTS_NO"].ToString();
                ddlCOuntryOrigin.SelectedValue = dtMast.Rows[0]["INM_CONTRY_ORIGIN"].ToString();
                ddlCountryOfDest.SelectedValue = dtMast.Rows[0]["INM_COUNTRY_DEST"].ToString();
                txtTransportBy.Text = dtMast.Rows[0]["INM_TRANSPORT_BY"].ToString();
                txtARERemark.Text = dtMast.Rows[0]["INM_ARE_REMARK"].ToString();

                //IMO Dangeroues & Annexute C-I detail
                txtCarrierName.Text = dtMast.Rows[0]["INM_CARRIER_NAME"].ToString();
                txtCarrierBookNo.Text = dtMast.Rows[0]["INM_CARRIER_BOOK_NO"].ToString();
                txtTechnicalName.Text = dtMast.Rows[0]["INM_TECH_NAME"].ToString();
                txtOuterPack.Text = dtMast.Rows[0]["INM_OUT_PAKGS"].ToString();
                txtInnerPack.Text = dtMast.Rows[0]["INM_INR_PAKG"].ToString();
                txtSubClass.Text = dtMast.Rows[0]["INM_SUB_CLASS"].ToString();
                txtUnPackingGroup.Text = dtMast.Rows[0]["INM_UN_PAK_GRP"].ToString();
                txtUnPackingCode.Text = dtMast.Rows[0]["INM_UN_PAK_CODE"].ToString();
                txtEMSNo.Text = dtMast.Rows[0]["INM_EMS_NO"].ToString();
                txtFlashPoint.Text = dtMast.Rows[0]["INM_FLASH_POINT"].ToString();
                if (dtMast.Rows[0]["INM_MARINE_POLLT"].ToString() == "1")
                {
                    chkMarrinePollutent.Checked = true;
                }
                else
                {
                    chkMarrinePollutent.Checked = false;
                }
                txtIECNo.Text = dtMast.Rows[0]["INM_IEC_NO"].ToString();
                txtIECDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_IEC_DATE"]).ToString("dd MMM yyyy");
                txtCentExcRegNo.Text = dtMast.Rows[0]["INM_CEN_EXC_REG"].ToString();
                txtExamDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_DATE_OF_EXAM"]).ToString("dd MMM yyyy");
                txtSuperintendentName.Text = dtMast.Rows[0]["INM_SUP_C_EXC_NAME"].ToString();
                txtInspectorName.Text = dtMast.Rows[0]["INM_INSP_C_EXC_NAME"].ToString();
                txtCustSealNo.Text = dtMast.Rows[0]["INM_CUST_SEAL_NO"].ToString();
                txtStuffingPerNo.Text = dtMast.Rows[0]["INM_PER_E_NO"].ToString();
                txtCargoNoOfPack.Text = dtMast.Rows[0]["INM_NONCARGO_NOPAKG"].ToString();
                txtShippingBillno.Text = dtMast.Rows[0]["INM_SHIPP_BILL_NO"].ToString();

                txtPackagingAmt.Text = dtMast.Rows[0]["INM_PACK_AMT"].ToString();
                txtAccessable.Text = dtMast.Rows[0]["INM_ACCESSIBLE_AMT"].ToString();
                txtBasicExcPer.Text = dtMast.Rows[0]["INM_BEXCISE"].ToString();
                txtBasicExcAmt.Text = dtMast.Rows[0]["INM_BE_AMT"].ToString();
                txtducexcper.Text = dtMast.Rows[0]["INM_EDUC_CESS"].ToString();
                txtEdueceAmt.Text = dtMast.Rows[0]["INM_EDUC_AMT"].ToString();
                txtSHEExcPer.Text = dtMast.Rows[0]["INM_H_EDUC_CESS"].ToString();
                txtSHEExcAmt.Text = dtMast.Rows[0]["INM_H_EDUC_AMT"].ToString();
                txtAdvDuty.Text = dtMast.Rows[0]["INM_ADV_DUTY"].ToString();
                txtTaxableAmt.Text = dtMast.Rows[0]["INM_TAXABLE_AMT"].ToString();
                txtSalesTaxAmount.Text=dtMast.Rows[0]["INM_S_TAX_AMT"].ToString();
                txtSalesTaxPer.Text = dtMast.Rows[0]["INM_S_TAX"].ToString();
                txtOtherCharges.Text = dtMast.Rows[0]["INM_OTHER_AMT"].ToString();
                txtInsurance.Text = dtMast.Rows[0]["INM_INSURANCE"].ToString();
                txtTransportAmt.Text = dtMast.Rows[0]["INM_TRANS_AMT"].ToString();
                txtOctri.Text = dtMast.Rows[0]["INM_OCTRI_AMT"].ToString();
                txtTCSAmt.Text = dtMast.Rows[0]["INM_TAX_TCS_AMT"].ToString();
                txtRoundingAmt.Text = dtMast.Rows[0]["INM_ROUNDING_AMT"].ToString();
                
                ddlTaxName.SelectedValue = dtMast.Rows[0]["INM_T_CODE"].ToString();

                txtIssuetime.Text = dtMast.Rows[0]["INM_ISSU_TIME"].ToString();
                txtRemoveltime.Text = dtMast.Rows[0]["INM_REMOVEL_TIME"].ToString();

                txtLRNo.Text = dtMast.Rows[0]["INM_LR_NO"].ToString();
                txtLCNo.Text = dtMast.Rows[0]["INM_LC_NO"].ToString();

                txtLRDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_LR_DATE"]).ToString("dd MMM yyyy");
                txtLCDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_LC_DATE"]).ToString("dd MMM yyyy");

                txtTransporterName.Text = dtMast.Rows[0]["INM_TRANSPORT_OWNER"].ToString();
                txtTransportAdd.Text = dtMast.Rows[0]["INM_TRANSPORT_ADDRESS"].ToString();

                btnSubmit.Visible = true;
                btnInsert.Visible = true;
                
                //dtInvoiceDetail = CommonClasses.Execute("select CPOD_I_CODE as ItemCode,I_CODENO as ShortName,cast((CPOD_ORD_QTY*CPOD_RATE) as numeric(20,2)) as Amount,CPOD_AMT,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as UnitCode,I_UOM_NAME as Unit,cast(CPOD_ORD_QTY as numeric(20,2))  as OrderQty,cast(CPOD_RATE as  numeric(10,2)) as Rate,CPOD_DESC as Description FROM CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER WHERE CPOD_I_CODE=I_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND ITEM_MASTER.ES_DELETE=0  and CPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and CPOM_CODE='" + mlCode + "' order by CPOD_I_CODE");
                //dtInvoiceDetail = CommonClasses.Execute("select IND_I_CODE,I_CODENO as IND_I_CODENO,0 as UOM_CODE,IND_GROSS_WEIGHT as GROSS_WGHT,IND_NET_WEIGHT as NET_WGHT,IND_SIZE_OF_BOX as BOX_SIZE,IND_NO_OF_BARRELS as NOOF_BARRELS,IND_NO_OF_PACK_DESC as NOOF_PACK_DESC,IND_CONTAINER_NO as CONT_NO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,'0.00' as STOCK_QTY,'0.00' as PEND_QTY,IND_INQTY as INV_QTY,I_UWEIGHT as ACT_WGHT,IND_RATE as RATE,IND_INQTY*IND_RATE as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT as EBASIC,IND_E_CESS_AMT as EEDUCESS,IND_SH_CESS_AMT as EHEDUCESS from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_MASTER where IND_INM_CODE=INM_CODE and IND_CPOM_CODE=CPOM_CODE and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE and INM_CODE='" + mlCode + "' ");
                if (ddlInvoiceType.SelectedValue == "2")
                {
                    dtInvoiceDetail = CommonClasses.Execute("select IND_I_CODE,I_CODENO as IND_I_CODENO,IND_UOM_CODE as UOM_CODE,IND_GROSS_WEIGHT as GROSS_WGHT,IND_NET_WEIGHT as NET_WGHT,IND_SIZE_OF_BOX as BOX_SIZE,IND_NO_OF_BARRELS as NOOF_BARRELS,IND_NO_PACK as NO_OF_PACK,IND_PACK_DESC as PACK_DESC,IND_CONTAINER_NO as CONT_NO,I_NAME as IND_I_NAME, I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,I_CURRENT_BAL as STOCK_QTY, cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(10,3)) as PEND_QTY,cast(IND_INQTY as numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as numeric(20,2)) as ACT_WGHT, cast(IND_RATE as numeric(20,2)) as RATE, cast(IND_INQTY*IND_RATE as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,isnull(IND_EX_AMT,0) as IND_EX_AMT, isnull(IND_E_CESS_AMT,0) as IND_E_CESS_AMT,isnull(IND_SH_CESS_AMT,0) as IND_SH_CESS_AMT,IND_BACHNO from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_MASTER, CUSTPO_DETAIL where IND_INM_CODE=INM_CODE and IND_CPOM_CODE=CPOM_CODE AND  CPOM_CODE=CPOD_CPOM_CODE AND CPOD_DISPACH > 0   and CPOD_I_CODE=I_CODE AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE  and INM_CODE='" + mlCode + "' ");
                }
                else
                {
                    dtInvoiceDetail = CommonClasses.Execute("select IND_I_CODE,I_CODENO as IND_I_CODENO,IND_UOM_CODE as UOM_CODE,IND_GROSS_WEIGHT as GROSS_WGHT,IND_NET_WEIGHT as NET_WGHT,IND_SIZE_OF_BOX as BOX_SIZE,IND_NO_OF_BARRELS as NOOF_BARRELS,IND_NO_PACK as NO_OF_PACK,IND_PACK_DESC as PACK_DESC,IND_CONTAINER_NO as CONT_NO,I_NAME as IND_I_NAME, I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,I_CURRENT_BAL as STOCK_QTY, cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(10,3)) as PEND_QTY,cast(IND_INQTY as numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as numeric(20,2)) as ACT_WGHT, cast(IND_RATE as numeric(20,2)) as RATE, cast(IND_INQTY*IND_RATE as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,isnull(IND_EX_AMT,0) as IND_EX_AMT, isnull(IND_E_CESS_AMT,0) as IND_E_CESS_AMT,isnull(IND_SH_CESS_AMT,0) as IND_SH_CESS_AMT,IND_BACHNO from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_MASTER, CUSTPO_DETAIL where IND_INM_CODE=INM_CODE and IND_CPOM_CODE=CPOM_CODE AND  CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=I_CODE AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE  and INM_CODE='" + mlCode + "' ");
                }
                    LoadICode();
                LoadIName();
             
                if (dtInvoiceDetail.Rows.Count != 0)
                {
                    dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
                    dgInvoiceAddDetail.DataBind();
                    dgInvoiceAddDetail.Enabled = true;
                }
            }
            if (str == "VIEW")
            {
                //ddlPOType.Enabled = false;
                ddlInvoiceType.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtDate.Enabled = false;
                ddlCustomer.Enabled = false;
                txtNetAmount.Enabled = false;
                txtDiscPer.Enabled = false;
                txtDiscount.Enabled = false;
                txtGrandTotal.Enabled = false;
                txtIssueDate.Enabled = false;
                txtRemovalDate.Enabled = false;
                txtBuyerName.Enabled = false;
                txtBuyerAddress.Enabled = false;
                chkIsSuppliement.Enabled = false;
                txtInsurance.Enabled = false;
                txtFinalDestination.Enabled = false;
                txtPreCarriageBy.Enabled = false;
                txtPortOfLoading.Enabled = false;
                txtPortOfDischarge.Enabled = false;
                txtPlaceofDelivery.Enabled = false;
                ddlCurrency.Enabled = false;
                txtPlaceofDelivery.Enabled = false;
                txtFlightNo.Enabled = false;
                txtMFGDate.Enabled = false;
                txtExpDate.Enabled = false;
                txtCurrencyRate.Enabled = false;
                txtAuthSign.Enabled = false;
                txtARE1FormNo.Enabled = false;
                txtARE1FormDate.Enabled = false;
                txtShipment.Enabled = false;
                txtCenvatAccountEntryNo.Enabled = false;
                txtBondNo.Enabled = false;
                txtBondDate.Enabled = false;
                txtUT1FileNo.Enabled = false;
                txtFileNo.Enabled = false;
                ChkExportUnderDutyClaim.Enabled = false;
                txtValidUpTo.Enabled = false;
                txtExaminedBoxes.Enabled = false;
                dgInvoiceAddDetail.Enabled = false;

                txtCarrierName.Enabled = false;
                txtCarrierBookNo.Enabled = false;
                txtTechnicalName.Enabled = false;
                txtOuterPack.Enabled = false;
                txtInnerPack.Enabled = false;
                txtSubClass.Enabled = false;
                txtUnPackingGroup.Enabled = false;
                txtUnPackingCode.Enabled = false;
                txtEMSNo.Enabled = false;
                txtFlashPoint.Enabled = false;
                chkMarrinePollutent.Enabled = false;
                txtIECNo.Enabled = false;
                txtIECDate.Enabled = false;
                txtCentExcRegNo.Enabled = false;
                txtExamDate.Enabled = false;
                txtSuperintendentName.Enabled = false;
                txtInspectorName.Enabled = false;
                txtCustSealNo.Enabled = false;
                txtStuffingPerNo.Enabled = false;
                txtCargoNoOfPack.Enabled = false;
                txtShippingBillno.Enabled = false;

                txtIssuetime.Enabled = false;
                txtRemoveltime.Enabled = false;

                txtLRNo.Enabled = false;
                txtLCNo.Enabled = false;

                txtLRDate.Enabled = false;
                txtLCDate.Enabled = false;

                 txtTransporterName.Enabled=false;
                 txtTransportAdd.Enabled = false;

                btnSubmit.Visible = false;
                btnInsert.Visible = false;
            }
            else if (str == "MOD")
            {
                CommonClasses.SetModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(mlCode));
                ddlCustomer.Enabled = false;
                ddlInvoiceType.Enabled = false;
                chkIsSuppliement.Enabled = false;
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region CarryForward
    public void CarryForward()
    {
        if (ddlCountryOfDest.SelectedIndex != 0)
        {
            DataTable dt = CommonClasses.Execute("select  top 1 * from INVOICE_MASTER where INM_INVOICE_TYPE<>1 and ES_DELETE=0 and INM_COUNTRY_DEST='" + ddlCountryOfDest.SelectedValue + "' order by INM_CODE desc");
            if (dt.Rows.Count > 0)
            {

                ddlCountryOfDest.SelectedValue = dt.Rows[0]["INM_COUNTRY_DEST"].ToString();
                if (ddlCountryOfDest.SelectedItem.Text != "RUSSIA")
                {
                    ddlCOuntryOrigin.SelectedValue = dt.Rows[0]["INM_CONTRY_ORIGIN"].ToString();
                    txtPreCarriageBy.Text = dt.Rows[0]["INM_PRE_CARRIAGE"].ToString();
                    txtPlaceOfPreCarriage.Text = dt.Rows[0]["INM_PLACE_REC"].ToString();
                    txtTrmOfDel.Text = dt.Rows[0]["INM_TOD"].ToString();
                    txtPortOfLoading.Text = dt.Rows[0]["INM_PORT_OF_LOAD"].ToString();

                    txtTrmOfPay.Text = dt.Rows[0]["INM_TOP"].ToString();
                    txtPortOfDischarge.Text = dt.Rows[0]["INM_PORT_OF_DISCH"].ToString();
                    txtFinalDestination.Text = dt.Rows[0]["INM_FINAL_DEST"].ToString();
                    txtMarksNos.Text = dt.Rows[0]["INM_M_NO"].ToString();
                    txtUnPackingGroup.Text = dt.Rows[0]["INM_UN_PAK_GRP"].ToString();
                    txtUnPackingCode.Text = dt.Rows[0]["INM_UN_PAK_CODE"].ToString();

                    txtUnNo.Text = dt.Rows[0]["INM_UN_NO"].ToString();
                    txtHASClass.Text = dt.Rows[0]["INM_HAZ"].ToString();

                    txtUT1FileNo.Text = dt.Rows[0]["INM_UT1_FILE_NO"].ToString();
                    txtFileNo.Text = dt.Rows[0]["INM_FILE_NO"].ToString();

                    txtTransporterName.Text = dt.Rows[0]["INM_TRANSPORT_OWNER"].ToString();
                    txtTransportAdd.Text = dt.Rows[0]["INM_TRANSPORT_ADDRESS"].ToString();
                }
            }
        }
    }
    #endregion

    #region LoadCurr
    private void LoadCurr()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER where ES_DELETE=0 and CURR_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataTextField = "CURR_NAME";
            ddlCurrency.DataValueField = "CURR_CODE";
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, new ListItem("Select currency", "0"));
            ddlCurrency.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT Distinct(P_CODE),P_NAME FROM CUSTPO_MASTER,PARTY_MASTER WHERE CPOM_P_CODE=P_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND P_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND P_TYPE='1' AND P_ACTIVE_IND=1 AND CPOM_TYPE='-2147483647' order by P_NAME ASC");
            //DataTable dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadCountry
    private void LoadCountry()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("SELECT COUNTRY_CODE,COUNTRY_NAME FROM COUNTRY_MASTER where ES_DELETE=0 and COUNTRY_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY COUNTRY_NAME");
            ddlCOuntryOrigin.DataSource = dt;
            ddlCOuntryOrigin.DataTextField = "COUNTRY_NAME";
            ddlCOuntryOrigin.DataValueField = "COUNTRY_CODE";
            ddlCOuntryOrigin.DataBind();
            ddlCOuntryOrigin.Items.Insert(0, new ListItem("Select", "0"));

            ddlCountryOfDest.DataSource = dt;
            ddlCountryOfDest.DataTextField = "COUNTRY_NAME";
            ddlCountryOfDest.DataValueField = "COUNTRY_CODE";
            ddlCountryOfDest.DataBind();
            ddlCountryOfDest.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "LoadCountry", Ex.Message);
        }
    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        try
        {
            DataTable dtPO = new DataTable();
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and  CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE)  and CPOM_TYPE=-2147483647");
            }
            else
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0 and CPOM_TYPE=-2147483647");
            }

            ddlPONO.DataSource = dtPO;
            ddlPONO.DataTextField = "CPOM_PONO";
            ddlPONO.DataValueField = "CPOM_CODE";
            ddlPONO.DataBind();
            ddlPONO.Items.Insert(0, new ListItem("Select PO", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "LoadPO", Ex.Message);
        }

    }
    #endregion

    #region Loadtax
    private void Loadtax()
    {
        try
        {
            DataTable dt = new DataTable();
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
            CommonClasses.SendError("Export Invoice", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAdd = CommonClasses.Execute("select P_ADD1 from PARTY_MASTER where P_CODE=" + ddlCustomer.SelectedValue + "");
            txtBuyerName.Text = ddlCustomer.SelectedItem.ToString();
            if (dtAdd.Rows.Count > 0)
            {
                txtBuyerAddress.Text = dtAdd.Rows[0]["P_ADD1"].ToString();
            }
            LoadICode();
            LoadIName();
            if (ddlItemCode.SelectedIndex == 0)
            {
                if (ddlPONO.SelectedIndex != -1)
                {
                    ddlPONO.SelectedIndex = 0;
                    txtPendingQty.Text = "0.000";
                    txtInvoiceQty.Text = "0.000";
                    txtRate.Text = "0.00";
                    txtAmount.Text = "0.00";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlCountryOfDest_SelectedIndexChanged
    protected void ddlCountryOfDest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CarryForward();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ddlCountryOfDest_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select DISTINCT I_CODE,I_CODENO from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE  and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_TYPE=-2147483647 and CPOM_CODE=CPOD_CPOM_CODE  order by I_CODENO ASC");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select DISTINCT I_CODE,I_NAME from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_TYPE=-2147483647");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "LoadIName", Ex.Message);
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
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                DataTable dtPO = new DataTable();
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE) and IND_INM_CODE=" + mlCode + " and CPOM_TYPE=-2147483647");
                }
                else
                {
                    dtPO = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and CPOM_TYPE=-2147483647 and CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and (CPOD_ORD_QTY-CPOD_DISPACH)>0");
                    //dtPO = CommonClasses.Execute("SELECT CPOM_CODE,CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0");
                }

                ddlPONO.DataSource = dtPO;
                ddlPONO.DataTextField = "CPOM_PONO";
                ddlPONO.DataValueField = "CPOM_CODE";
                ddlPONO.DataBind();
                ddlPONO.Items.Insert(0, new ListItem("Select PO", "0"));
                //ddlPONo_SelectedIndexChanged(null, null);
                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    lblUOM.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                }
                else
                {
                    txtUOM.Text = "";
                }

            }
            else
            {
                ddlItemName.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ddlItemCode_SelectedIndexChanged", Ex.Message);

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
                //DataTable dtPO = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and CPOM_TYPE=-2147483647 and CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and (CPOD_ORD_QTY-CPOD_DISPACH)>0");
                //DataTable dtPO = CommonClasses.Execute("select distinct(CPOM_CODE),CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and ES_DELETE=0");
                DataTable dtPO = new DataTable();
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE) and IND_INM_CODE=" + mlCode + " and CPOM_TYPE=-2147483647");
                }
                else
                {
                    dtPO = CommonClasses.Execute("select CPOM_CODE,CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and CPOM_TYPE=-2147483647 and CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and (CPOD_ORD_QTY-CPOD_DISPACH)>0");
                    //dtPO = CommonClasses.Execute("SELECT CPOM_CODE,CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0");
                }
                
                ddlPONO.DataSource = dtPO;
                ddlPONO.DataTextField = "CPOM_PONO";
                ddlPONO.DataValueField = "CPOM_CODE";
                ddlPONO.DataBind();
                ddlPONO.Items.Insert(0, new ListItem("Select PO", "0"));

                //ddlPONo.SelectedIndexChanged(null, null);

                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    lblUOM.Text = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                }
                else
                {
                    txtUOM.Text = "";
                    lblUOM.Text = "";
                }
                DataTable DtRate = new DataTable();
            }
            else
            {
                ddlItemCode.SelectedValue = "0";

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceAddDetail_Deleting
    protected void dgInvoiceAddDetail_Deleting(object sender, GridViewDeleteEventArgs e)
    {
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
                dtInvoiceDetail.Rows.RemoveAt(rowindex);
                dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
                dgInvoiceAddDetail.DataBind();
                if (dgInvoiceAddDetail.Rows.Count == 0)
                {
                    dgInvoiceAddDetail.Enabled = false;
                    GetGrantTot();
                    LoadFilter();
                    txtFinalDestination.Text = "";
                    txtPreCarriageBy.Text = "";
                    txtPortOfLoading.Text ="";
                    txtPortOfDischarge.Text ="";
                    txtPlaceofDelivery.Text = "";
                    ddlCurrency.SelectedIndex = 0;
                    txtTrmOfPay.Text = "";
                    ddlCurrency_SelectedIndexChanged(null, null);
                }
                else
                {
                    GetGrantTot();
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                //ddlItemCode_SelectedIndexChanged(null, null);
                txtUOM.Text = ((Label)(row.FindControl("lblUOM"))).Text;
                ddlPONO.SelectedValue = ((Label)(row.FindControl("lblPO_CODE"))).Text;
                txtStockQty.Text = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(((Label)(row.FindControl("lblSTOCK_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text)), 3));
                txtPendingQty.Text = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(((Label)(row.FindControl("lblPEND_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text)), 3));
                double pendQty = Convert.ToDouble(((Label)(row.FindControl("lblPEND_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text);
                txtPendingQty.Text = string.Format("{0:0.000}", Math.Round(pendQty, 3));
                txtInvoiceQty.Text = string.Format("{0:0.000}",Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text),3));
                ///txtActWght.Text = ((Label)(row.FindControl("lblACT_WGHT"))).Text;
                txtRate.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblRATE"))).Text),2));
                txtAmount.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAMT"))).Text),2));
                txtGrossWght.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblGROSS_WGHT"))).Text),2));
                txtNetWght.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblNET_WGHT"))).Text),2));
                txtSizeofBox.Text = Convert.ToDouble(((Label)(row.FindControl("lblBOX_SIZE"))).Text).ToString();
                txtNoofBarrets.Text = Convert.ToInt32(((Label)(row.FindControl("lblNOOF_BARRELS"))).Text).ToString();
                txtNoofPackDesc.Text = ((Label)(row.FindControl("lblNOOF_PACK_DESC"))).Text;
                txtPackDesc.Text = ((Label)(row.FindControl("lblPACK_DESC"))).Text;
                txtContainerNo.Text = ((Label)(row.FindControl("lblCONT_NO"))).Text;
                txtbatchno.Text = ((Label)(row.FindControl("lblIND_BACHNO"))).Text;
            }           

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion


    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCurrrate = CommonClasses.Execute("SELECT CURR_CODE,CAST(CURR_RATE as numeric(20,2)) AS CURR_RATE FROM CURRANCY_MASTER WHERE ES_DELETE=0 and CURR_CODE='" + ddlCurrency.SelectedValue + "'");
            if (dtCurrrate.Rows.Count > 0)
            {
                txtCurrencyRate.Text = dtCurrrate.Rows[0]["CURR_RATE"].ToString();
            }
            else
            {
                txtCurrencyRate.Text = "0.00";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ddlCurrency_SelectedIndexChanged", Ex.Message);
        }
    }



    #region ddlPONO_SelectedIndexChanged
    protected void ddlPONO_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                DataTable dt1 = CommonClasses.Execute("SELECT CPOD_RATE,(CPOD_ORD_QTY-CPOD_DISPACH) as Qty FROM CUSTPO_DETAIL WHERE CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOD_CPOM_CODE=" + ddlPONO.SelectedValue + "");
                //DataTable dtQty = CommonClasses.Execute("SELECT (CPOD_ORD_QTY-CPOD_DISPACH) as Qty FROM CUSTPO_DETAIL where CPOD_I_CODE=" + ddlItemCode.SelectedValue + " and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + "");
                if (dt1.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.00}",dt1.Rows[0]["CPOD_RATE"]);
                }
                else
                {
                    txtRate.Text = "0.00";
                }
                if (dt1.Rows.Count > 0)
                {
                    txtPendingQty.Text = string.Format("{0:0.000}",dt1.Rows[0]["Qty"]);
                    txtInvoiceQty.Text = string.Format("{0:0.000}",dt1.Rows[0]["Qty"]);
                    txtAmount.Text = string.Format("{0:0.00}", (Convert.ToDouble(txtInvoiceQty.Text) * Convert.ToDouble(txtRate.Text)));
                    //txtVQty_OnTextChanged(null, null);
                }
                else
                {
                    txtPendingQty.Text = "0.000";
                    txtInvoiceQty.Text = "0.000";
                    txtAmount.Text = "0.00";
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "ddlPONo_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion    

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {

            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");
 
             
            if (txtInvoiceQty.Text=="0.000")
            {
                //ShowMessage("#Avisos", "Please Enter Invoice Qty", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Invoice Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtInvoiceQty.Focus();
                return;
            }
            if (!chkIsSuppliement.Checked)
            {
                if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtInvoiceQty.Text))
                {
                    //ShowMessage("#Avisos", "Please Enter Invoice Qty", CommonClasses.MSG_Warning);

                    PanelMsg.Visible = true;
                    lblmsg.Text = "Invoice Qty Is Not Greater Than Pending Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtInvoiceQty.Focus();
                    return;
                }

                if (ddlInvoiceType.SelectedValue != "3")
                {
                    if (Convert.ToDouble(txtStockQty.Text) < Convert.ToDouble(txtInvoiceQty.Text))
                    {
                        //ShowMessage("#Avisos", "Stock Qty Is Not Less Than Invloice Qty", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Stock Qty Is Not Less Than Invoice Qty";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtStockQty.Focus();
                        return;
                    }
                }
            }
            if (txtGrossWght.Text == "0.00" || txtGrossWght.Text == "")
            {

                txtGrossWght.Text = "0.00";
              
            }
            if (txtSizeofBox.Text=="0.00" || txtSizeofBox.Text=="")
            {
                //ShowMessage("#Avisos", "Enter Size Of Box", CommonClasses.MSG_Warning);
                txtSizeofBox.Text = "0.00";
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Enter Size Of Box";
                //txtSizeofBox.Focus();
                //return;
            }
           
            if (txtNoofBarrets.Text=="0" || txtNoofBarrets.Text=="")
            {
               // ShowMessage("#Avisos", "Enter No Of Barrels", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter No Of Barrels";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtNoofBarrets.Focus();
                return;
            }

            if (txtNoofPackDesc.Text == "")
            {
               // ShowMessage("#Avisos", "Enter No Of Pack", CommonClasses.MSG_Warning);
                txtNoofPackDesc.Text = "0.00";
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Enter No Of Pack";
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                //txtNoofPackDesc.Focus();
                //return;
            }
            if (txtContainerNo.Text == "")
            {
                txtContainerNo.Text = "0";
                //ShowMessage("#Avisos", "Enter No Container No", CommonClasses.MSG_Warning);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Enter No Container No";
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                //txtContainerNo.Focus();
                //return;
            }
            //txtGrossWght.Text = "0.00";

            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_I_CODE"))).Text;
                    string IND_BACHNO = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_BACHNO"))).Text;
                   
                    if (ItemUpdateIndex == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && IND_BACHNO==txtbatchno.Text)
                        {
                            ShowMessage("#Avisos", "Record Already Exist.", CommonClasses.MSG_Info);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            //PanelMsg.Visible = true;
                            //lblmsg.Text = "Record Already Exist.";
                            return;
                        }

                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString() && IND_BACHNO == txtbatchno.Text)
                        {
                            ShowMessage("#Avisos", "Record Already Exist.", CommonClasses.MSG_Info);
                            //PanelMsg.Visible = true;
                            //lblmsg.Text = "Record Already Exist.";
                            return;
                        }

                    }

                }
            }

            #region datatable structure
            if (dtInvoiceDetail.Columns.Count <= 0)
            {
                dtInvoiceDetail.Columns.Add("IND_I_CODE");
                dtInvoiceDetail.Columns.Add("IND_I_CODENO");
                dtInvoiceDetail.Columns.Add("IND_I_NAME");
                dtInvoiceDetail.Columns.Add("UOM");
                dtInvoiceDetail.Columns.Add("UOM_CODE");
                dtInvoiceDetail.Columns.Add("PO_CODE");
                dtInvoiceDetail.Columns.Add("PO_NO");
                dtInvoiceDetail.Columns.Add("STOCK_QTY");
                dtInvoiceDetail.Columns.Add("PEND_QTY");
                dtInvoiceDetail.Columns.Add("INV_QTY");
                dtInvoiceDetail.Columns.Add("RATE");
                dtInvoiceDetail.Columns.Add("AMT");
                dtInvoiceDetail.Columns.Add("GROSS_WGHT");
                dtInvoiceDetail.Columns.Add("NET_WGHT");
                dtInvoiceDetail.Columns.Add("BOX_SIZE");
                dtInvoiceDetail.Columns.Add("NOOF_BARRELS");
                dtInvoiceDetail.Columns.Add("NO_OF_PACK");
                dtInvoiceDetail.Columns.Add("PACK_DESC");
                dtInvoiceDetail.Columns.Add("CONT_NO");
                dtInvoiceDetail.Columns.Add("IND_EX_AMT");
                dtInvoiceDetail.Columns.Add("IND_E_CESS_AMT");
                dtInvoiceDetail.Columns.Add("IND_SH_CESS_AMT");

                dtInvoiceDetail.Columns.Add("IND_BACHNO");

            }
            #endregion

            #region add control value to Dt
            dr = dtInvoiceDetail.NewRow();

            dr["IND_I_CODE"] = ddlItemName.SelectedValue;
            dr["IND_I_CODENO"] = ddlItemCode.SelectedItem;
            dr["IND_I_NAME"] = ddlItemName.SelectedItem;
            dr["UOM"] = txtUOM.Text;
            dr["UOM_CODE"] = lblUOM.Text;
            dr["PO_CODE"] = ddlPONO.SelectedValue;
            dr["PO_NO"] = ddlPONO.SelectedItem;
            dr["STOCK_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtStockQty.Text == "" ? "0.00" : txtStockQty.Text)));
            dr["PEND_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtPendingQty.Text) - Convert.ToDouble(txtInvoiceQty.Text)));
            dr["INV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtInvoiceQty.Text == "" ? "0.00" : txtInvoiceQty.Text)));
            //dr["STOCK_QTY"] = txtStockQty.Text;
            //dr["PEND_QTY"] = txtPendingQty.Text;
            //dr["INV_QTY"] = txtInvoiceQty.Text;
            dr["RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text)));
            dr["AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtInvoiceQty.Text)));
            //dr["AMT"] = txtAmount.Text;
            dr["GROSS_WGHT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtGrossWght.Text)));
            dr["NET_WGHT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtNetWght.Text)));
            dr["BOX_SIZE"] = txtSizeofBox.Text;
            dr["NOOF_BARRELS"] = txtNoofBarrets.Text;
            dr["NO_OF_PACK"] = txtNoofPackDesc.Text;
            dr["PACK_DESC"] = txtPackDesc.Text;
            dr["CONT_NO"] = txtContainerNo.Text;



            #region ExciseCalculation
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

            txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EBasicPer));
            txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(EEduCessPer));
            txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EHEduCessPer));

            EBasic = Math.Round((((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtInvoiceQty.Text)) * EBasicPer) / 100),2);
            EEduCess = Math.Round(EBasic * EEduCessPer / 100, 2);
            EHEduCess = Math.Round(EBasic * EHEduCessPer / 100, 2);
            dr["IND_EX_AMT"] = EBasic;
            dr["IND_E_CESS_AMT"] = EEduCess;
            dr["IND_SH_CESS_AMT"] = EHEduCess;


            dr["IND_BACHNO"] = txtbatchno.Text;


            #endregion
           
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (dtInvoiceDetail.Rows.Count > 0)
                {
                    dtInvoiceDetail.Rows.RemoveAt(Index);
                    dtInvoiceDetail.Rows.InsertAt(dr, Index);
                }
            }
            else
            {
                dtInvoiceDetail.Rows.Add(dr);
            }
            #endregion
           
            #region Binding data to Grid
            dgInvoiceAddDetail.Visible = true;
            dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
            dgInvoiceAddDetail.DataBind();

            #endregion



            //#region Po detail carry forward
            //if (dtInvoiceDetail.Rows.Count == 1)
            //{
            //    DataTable dtPoDetail = CommonClasses.Execute("select *  from CUSTPO_MASTER where CPOM_CODE='" + ddlPONO.SelectedValue + "'");
            //    if (dtPoDetail.Rows.Count > 0)
            //    {
            //        txtFinalDestination.Text = dtPoDetail.Rows[0]["CPOM_FINAL_DEST"].ToString();
            //        txtPreCarriageBy.Text = dtPoDetail.Rows[0]["CPOM_PRE_CARR_BY"].ToString();
            //        txtPortOfLoading.Text = dtPoDetail.Rows[0]["CPOM_PORT_LOAD"].ToString();
            //        txtPortOfDischarge.Text = dtPoDetail.Rows[0]["CPOM_PORT_DIS"].ToString();
            //        txtPlaceofDelivery.Text = dtPoDetail.Rows[0]["CPOM_PLACE_DEL"].ToString();
            //        ddlCurrency.SelectedValue = dtPoDetail.Rows[0]["CPOM_CURR_CODE"].ToString();
            //        txtTrmOfPay.Text = dtPoDetail.Rows[0]["CPOM_PAY_TERM"].ToString();
            //        ddlCurrency_SelectedIndexChanged(null,null);
            //    }
            //}
            //#endregion


            #region Tax
            if (dtInvoiceDetail.Rows.Count == 1)
            {
                DataTable dtTax = CommonClasses.Execute("select CPOD_ST_CODE from CUSTPO_DETAIL,CUSTPO_MASTER where CPOM_CODE='" + ddlPONO.SelectedValue + "' and CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE=CPOD_CPOM_CODE");
                if (dtTax.Rows.Count > 0)
                {
                    ddlTaxName.SelectedValue = dtTax.Rows[0]["CPOD_ST_CODE"].ToString();
                    ddlTaxName_SelectedIndexChanged(null, null);
                }
            }
            #endregion

            dgInvoiceAddDetail.Enabled = true;
            GetGrantTot();

            #region Clear Controles
            ClearControles();
            #endregion

            
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Exsport Invoice", "btnInsert_Click", Ex.Message);
        }

    }
    #endregion

    #region ClearControles
    void ClearControles()
    {
        ddlItemCode.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        ddlPONO.SelectedIndex = 0;
        txtUOM.Text = "";
        txtStockQty.Text = "0.000";
        txtPendingQty.Text = "0.000";
        txtInvoiceQty.Text = "0.000";
        txtGrossWght.Text = "0.00";
        txtNetWght.Text = "0.00";
        txtRate.Text = "0.00";
        txtAmount.Text = "";
        txtSizeofBox.Text = "";
        txtNoofBarrets.Text = "0";
        txtNoofPackDesc.Text = "";
        txtContainerNo.Text = "";
        txtPackDesc.Text = "";
        txtbatchno.Text = "";
    }
    #endregion

    #region txtDiscPer_TextChanged
    protected void txtDiscPer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtDiscPer.Text);

            txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            //txtDiscount.Text = string.Format("{0:0.00}", ((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text)) / 100));
            //txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtDiscPer.Text), 2));
            GetGrantTot();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "txtDiscPer_TextChanged", Ex.Message);
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
                // txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtSalesTaxPer.Text) / 100), 0));
                //txtGrandAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtGrandAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text));
                //GetTotal();
            }
        }
        else
        {
            txtSalesTaxPer.Text = "0.00";
            txtSalesTaxAmount.Text = "0.00";
        }
    }
    #endregion

    #region txtFreight_TextChanged
    protected void txtFreight_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtFreight.Text);

            txtFreight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            //txtFreight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtFreight.Text), 2));
            GetGrantTot();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "txtFreight_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtInsurance_TextChanged
    protected void txtInsurance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtInsurance.Text);

            txtInsurance.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

           // txtInsurance.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtInsurance.Text), 0));
            GetGrantTot();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "txtInsurance_TextChanged", Ex.Message);
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
            // CalcExise();
            string totalStr = DecimalMasking(txtTransportAmt.Text);

            txtTransportAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

           // txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTransportAmt.Text));
            GetGrantTot();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtAdvDuty_TextChanged
    protected void txtAdvDuty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = "";
        try
        {
            if (txtTransportAmt.Text == "")
            {
                txtTransportAmt.Text = "0.00";
            }
            // CalcExise();
             totalStr = DecimalMasking(txtAdvDuty.Text);

            txtAdvDuty.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

             totalStr = DecimalMasking(txtTransportAmt.Text);

            txtTransportAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            //txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTransportAmt.Text));
            GetGrantTot();
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
            // CalcExise();

            string totalStr = DecimalMasking(txtOctri.Text);

            txtOctri.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

           // txtOctri.Text = string.Format("{0:0.00}", Convert.ToDouble(txtOctri.Text));
            GetGrantTot();
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
            // CalcExise();
            string totalStr = DecimalMasking(txtTCSAmt.Text);

            txtTCSAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

           // txtTCSAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTCSAmt.Text));
            GetGrantTot();
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

            // CalcExise();
            GetGrantTot();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtDiscAmt_TextChanged
    protected void txtDiscAmt_TextChanged(object sender, EventArgs e)
    {
        // CalcExise();
    }
    #endregion

    #region txtBasicExcPer_TextChanged
    protected void txtBasicExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcPer.Text);

        txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        //txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(txtBasicExcPer.Text));
        GetGrantTot();
    }
    #endregion

    #region txtducexcper_TextChanged
    protected void txtducexcper_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtducexcper.Text);

        txtducexcper.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
       // txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(txtducexcper.Text));
        GetGrantTot();
    }
    #endregion

    #region txtBasicExcAmt_TextChanged
    protected void txtBasicExcAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcAmt.Text);

        txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        // txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(txtducexcper.Text));
        GetGrantTot();
    }
    #endregion

    #region txtEdueceAmt_TextChanged
    protected void txtEdueceAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtEdueceAmt.Text);

        txtEdueceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        // txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(txtducexcper.Text));
        GetGrantTot();
    }
    #endregion



    #region txtSHEExcPer_TextChanged
    protected void txtSHEExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtSHEExcPer.Text);

        txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
       // txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSHEExcPer.Text));
        GetGrantTot();
    }
    #endregion

    
    #region txtPackAmt_TextChanged
    protected void txtPackagingAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtPackagingAmt.Text);

        txtPackagingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        //txtPackagingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtPackagingAmt.Text));
        GetGrantTot();
    }
    #endregion

    #region txtSalesTaxPer_TextChanged
    protected void txtSalesTaxPer_TextChanged(object sender, EventArgs e)
    {
        txtSalesTaxPer.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSalesTaxPer.Text));
        GetGrantTot();
    }
    #endregion

    #region txtOtherCharges_TextChanged
    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtOtherCharges.Text);

        txtOtherCharges.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        //txtOtherCharges.Text = string.Format("{0:0.00}", Convert.ToDouble(txtOtherCharges.Text));
        GetGrantTot();
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

    #region txtInvoiceQty_OnTextChanged
    protected void txtInvoiceQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtInvoiceQty.Text == "")
            {
                txtInvoiceQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtInvoiceQty.Text);

                txtInvoiceQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

               // txtInvoiceQty.Text = string.Format("{0:0.00}", Convert.ToDouble(txtInvoiceQty.Text));
            }
            if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtInvoiceQty.Text))
            {
//                ShowMessage("#Avisos", "Please Enter Invoice Qty", CommonClasses.MSG_Warning);

                PanelMsg.Visible = true;
                lblmsg.Text = "Invoice Qty Is Not Greater Than Pending Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtInvoiceQty.Focus();
                return;
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            else
            {
                txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtInvoiceQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Incvoice", "txtInvoiceQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtGrossWght_OnTextChanged
    protected void txtGrossWght_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            
          txtGrossWght.Text = string.Format("{0:0.000}", Convert.ToDouble(txtGrossWght.Text));
       
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Incvoice", "txtGrossWght_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtNetWght_OnTextChanged
    protected void txtNetWght_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtNetWght.Text);

            txtNetWght.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            //txtNetWght.Text = string.Format("{0:0.000}", Convert.ToDouble(txtNetWght.Text));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Incvoice", "txtNetWght_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (txtInvoiceQty.Text == "")
            {
                txtInvoiceQty.Text = "0.000";
            }
            else
            {
                txtInvoiceQty.Text = string.Format("{0:0.00}",Convert.ToDouble(txtInvoiceQty.Text));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.000";
            }
            else
            {
                txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtInvoiceQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
          
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        string Invoice_Type = ddlInvoiceType.SelectedValue;
        int Marrine_Pollutnet = 0;
        if (chkMarrinePollutent.Checked)
        {
            Marrine_Pollutnet = 1;
        }
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
                dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and INM_INVOICE_TYPE='" + Invoice_Type + "'");
                if (dt.Rows.Count > 0)
                {
                    Inv_No = Convert.ToInt32(dt.Rows[0]["INM_NO"]);
                    Inv_No = Inv_No + 1;
                }
                if (CommonClasses.Execute1("INSERT INTO INVOICE_MASTER (INM_CM_CODE,INM_NO,INM_DATE,INM_P_CODE,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_G_AMT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_BUYER_NAME,INM_BUYTER_ADD,INM_INSURANCE,INM_FREIGHT,INM_FINAL_DEST,INM_PRE_CARRIAGE,INM_PORT_OF_LOAD,INM_PORT_OF_DISCH,INM_PLACE_OF_DEL,INM_CURR_CODE,INM_CLEARANCE,INM_FLIGHT_NO,INM_MFG_DATE,INM_EXP_DATE,INM_CURR_RATE,INM_AUTHO_SIGN,INM_AREA_FORM_NO,INM_FORM_DATE,INM_SHIPMENT,INM_CENVAT_AC_NO,INM_BOND_NO,INM_BOND_DATE,INM_UT1_FILE_NO,INM_FILE_NO,INM_EXPORT_FLAG,INM_VALID_DATE,INM_EXA_BOXES,INM_INVOICE_TYPE,INM_TOD,INM_TOP,INM_VOY_NO,INM_PLACE_REC,INM_M_NO,INM_NOS_PACK,INM_UN_NO,INM_HAZ,INM_HS_CODENO,INM_CONTA_NO,INM_SEAL_NO,INM_OTS_NO,INM_CONTRY_ORIGIN,INM_COUNTRY_DEST,INM_TRANSPORT_BY,INM_ARE_REMARK,INM_CARRIER_NAME,INM_CARRIER_BOOK_NO,INM_TECH_NAME,INM_OUT_PAKGS,INM_INR_PAKG,INM_SUB_CLASS,INM_UN_PAK_GRP,INM_UN_PAK_CODE,INM_EMS_NO,INM_FLASH_POINT,INM_MARINE_POLLT,INM_IEC_NO,INM_IEC_DATE,INM_CEN_EXC_REG,INM_DATE_OF_EXAM,INM_SUP_C_EXC_NAME,INM_INSP_C_EXC_NAME,INM_CUST_SEAL_NO,INM_PER_E_NO,INM_NONCARGO_NOPAKG,INM_SHIPP_BILL_NO,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_ADV_DUTY,INM_TAXABLE_AMT,INM_T_CODE,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_TRANS_AMT,INM_OCTRI_AMT,INM_TAX_TCS_AMT,INM_ROUNDING_AMT,INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME,INM_LR_NO,INM_LR_DATE,INM_LC_NO,INM_LC_DATE,INM_TRANSPORT_OWNER,INM_TRANSPORT_ADDRESS)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlCustomer.SelectedValue + "','" + txtNetAmount.Text + "','" + txtDiscPer.Text + "','" + txtDiscount.Text + "','" + txtGrandTotal.Text + "','" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtRemovalDate.Text).ToString("dd/MMM/yyyy") + "','" + txtBuyerName.Text + "','" + txtBuyerAddress.Text + "','" + txtInsurance.Text + "','" + txtFreight.Text + "','" + txtFinalDestination.Text + "','" + txtPreCarriageBy.Text + "','" + txtPortOfLoading.Text + "','" + txtPortOfDischarge.Text + "','" + txtPlaceofDelivery.Text + "','" + ddlCurrency.SelectedValue + "','" + txtClearance.Text + "','" + txtFlightNo.Text + "','" + Convert.ToDateTime(txtMFGDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtExpDate.Text).ToString("dd/MMM/yyyy") + "','" + txtCurrencyRate.Text + "','" + txtAuthSign.Text + "','" + txtARE1FormNo.Text + "','" + Convert.ToDateTime(txtARE1FormDate.Text).ToString("dd/MMM/yyyy") + "','" + txtShipment.Text + "','" + txtCenvatAccountEntryNo.Text + "','" + txtBondNo.Text + "','" + Convert.ToDateTime(txtBondDate.Text).ToString("dd/MMM/yyyy") + "','" + txtUT1FileNo.Text + "','" + txtFileNo.Text + "','" + ChkExportUnderDutyClaim.Checked + "','" + Convert.ToDateTime(txtValidUpTo.Text).ToString("dd/MMM/yyyy") + "','" + txtExaminedBoxes.Text + "','" + Invoice_Type + "','" + txtTrmOfDel.Text + "','" + txtTrmOfPay.Text + "','" + txtVoyageNo.Text + "','" + txtPlaceOfPreCarriage.Text + "','" + txtMarksNos.Text + "','" + txtNoKindPkg.Text + "','" + txtUnNo.Text + "','" + txtHASClass.Text + "','" + txtHSCodeNo.Text + "','" + txtContainer.Text + "','" + txtBottleSealNo.Text + "','" + txtOTSNo.Text + "','" + ddlCOuntryOrigin.SelectedValue + "','" + ddlCountryOfDest.SelectedValue + "','" + txtTransportBy.Text + "','" + txtARERemark.Text + "','" + txtCarrierName.Text + "','" + txtCarrierBookNo.Text + "','" + txtTechnicalName.Text + "','" + txtOuterPack.Text + "','" + txtInnerPack.Text + "','" + txtSubClass.Text + "','" + txtUnPackingGroup.Text + "','" + txtUnPackingCode.Text + "','" + txtEMSNo.Text + "','" + txtFlashPoint.Text + "','" + Marrine_Pollutnet + "','" + txtIECNo.Text + "','" + Convert.ToDateTime(txtIECDate.Text).ToString("dd/MMM/yyyy") + "','" + txtCentExcRegNo.Text + "','" + Convert.ToDateTime(txtExamDate.Text).ToString("dd/MMM/yyyy") + "','" + txtSuperintendentName.Text + "','" + txtInspectorName.Text + "','" + txtCustSealNo.Text + "','" + txtStuffingPerNo.Text + "','" + txtCargoNoOfPack.Text + "','" + txtShippingBillno.Text + "','" + txtPackagingAmt.Text + "','" + txtAccessable.Text + "','" + txtBasicExcPer.Text + "','" + txtBasicExcAmt.Text + "','" + txtducexcper.Text + "','" + txtEdueceAmt.Text + "','" + txtSHEExcPer.Text + "','" + txtSHEExcAmt.Text + "','" + txtAdvDuty.Text + "','" + txtTaxableAmt.Text + "','" + ddlTaxName.SelectedValue + "','" + txtSalesTaxPer.Text + "','" + txtSalesTaxAmount.Text + "','" + txtOtherCharges.Text + "','" + txtTransportAmt.Text + "','" + txtOctri.Text + "','" + txtTCSAmt.Text + "','" + txtRoundingAmt.Text + "','" + IsSuppliement + "','" + txtIssuetime.Text + "','" + txtRemoveltime.Text + "','" + txtLRNo.Text + "','" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "','" + txtLCNo.Text + "','" + Convert.ToDateTime(txtLCDate.Text).ToString("dd/MMM/yyyy") + "','"+txtTransporterName.Text+"','"+txtTransportAdd.Text+"')"))
                {                   
                    string Code = CommonClasses.GetMaxId("SELECT Max(INM_CODE) FROM INVOICE_MASTER");
                    for (int i = 0; i < dtInvoiceDetail.Rows.Count; i++)
                    {
                        //***
                        result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_GROSS_WEIGHT,IND_NET_WEIGHT,IND_SIZE_OF_BOX,IND_NO_OF_BARRELS,IND_NO_PACK,IND_PACK_DESC,IND_CONTAINER_NO,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_BACHNO,IND_UOM_CODE) values ('" + Code + "','" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "','" + dtInvoiceDetail.Rows[i]["PO_CODE"] + "','" + dtInvoiceDetail.Rows[i]["INV_QTY"] + "','" + dtInvoiceDetail.Rows[i]["RATE"] + "','" + dtInvoiceDetail.Rows[i]["AMT"] + "','" + dtInvoiceDetail.Rows[i]["GROSS_WGHT"] + "','" + dtInvoiceDetail.Rows[i]["NET_WGHT"] + "','" + dtInvoiceDetail.Rows[i]["BOX_SIZE"] + "','" + dtInvoiceDetail.Rows[i]["NOOF_BARRELS"] + "','" + dtInvoiceDetail.Rows[i]["NO_OF_PACK"] + "','" + dtInvoiceDetail.Rows[i]["PACK_DESC"] + "','" + dtInvoiceDetail.Rows[i]["CONT_NO"] + "','" + dtInvoiceDetail.Rows[i]["IND_EX_AMT"] + "','" + dtInvoiceDetail.Rows[i]["IND_E_CESS_AMT"] + "','" + dtInvoiceDetail.Rows[i]["IND_SH_CESS_AMT"] + "','" + dtInvoiceDetail.Rows[i]["INV_QTY"] + "','" + dtInvoiceDetail.Rows[i]["IND_BACHNO"] + "','" + dtInvoiceDetail.Rows[i]["UOM_CODE"] + "')");
                        if (!chkIsSuppliement.Checked)
                        {
                            if (result == true && Invoice_Type == "2")
                            {
                                result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + dtInvoiceDetail.Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + dtInvoiceDetail.Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true && Invoice_Type == "2")
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "','" + Code + "','" + Inv_No + "','EXPINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + dtInvoiceDetail.Rows[i]["INV_QTY"] + "')");

                            }
                            //Removing Stock
                            if (result == true && Invoice_Type == "2")
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtInvoiceDetail.Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "'");
                            }
                        }
                   
                    }

                    //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");
                    CommonClasses.WriteLog("Export Invoice", "Save", "Export Invoice", Convert.ToString(Inv_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    dtInvoiceDetail.Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewExportInvoice.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                    txtInvoiceNo.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE INVOICE_MASTER SET INM_CM_CODE=" + Session["CompanyCode"] + ",INM_NO='" + txtInvoiceNo.Text + "', INM_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',INM_P_CODE='" + ddlCustomer.SelectedValue + "',INM_NET_AMT='" + txtNetAmount.Text + "',INM_DISC='" + txtDiscPer.Text + "',INM_DISC_AMT='" + txtDiscount.Text + "',INM_G_AMT='" + txtGrandTotal.Text + "',INM_ISSUE_DATE='" + Convert.ToDateTime(txtIssueDate.Text).ToString("dd/MMM/yyyy") + "',INM_REMOVAL_DATE='" + Convert.ToDateTime(txtRemovalDate.Text).ToString("dd/MMM/yyyy") + "',INM_BUYER_NAME='" + txtBuyerName.Text + "',INM_BUYTER_ADD='" + txtBuyerAddress.Text + "',INM_INSURANCE='" + txtInsurance.Text + "',INM_FREIGHT='" + txtFreight.Text + "',INM_FINAL_DEST='" + txtFinalDestination.Text + "',INM_PRE_CARRIAGE='" + txtPreCarriageBy.Text + "',INM_PORT_OF_LOAD='" + txtPortOfLoading.Text + "',INM_PORT_OF_DISCH='" + txtPortOfDischarge.Text + "',INM_PLACE_OF_DEL='" + txtPlaceofDelivery.Text + "',INM_CURR_CODE='" + ddlCurrency.SelectedValue + "',INM_CLEARANCE='" + txtClearance.Text + "',INM_FLIGHT_NO='" + txtFlightNo.Text + "',INM_MFG_DATE='" + Convert.ToDateTime(txtMFGDate.Text).ToString("dd/MMM/yyyy") + "',INM_EXP_DATE='" + Convert.ToDateTime(txtExpDate.Text).ToString("dd/MMM/yyyy") + "',INM_CURR_RATE='" + txtCurrencyRate.Text + "',INM_AUTHO_SIGN='" + txtAuthSign.Text + "',INM_AREA_FORM_NO='" + txtARE1FormNo.Text + "',INM_FORM_DATE='" + Convert.ToDateTime(txtARE1FormDate.Text).ToString("dd/MMM/yyyy") + "',INM_SHIPMENT='" + txtShipment.Text + "',INM_CENVAT_AC_NO='" + txtCenvatAccountEntryNo.Text + "',INM_BOND_NO='" + txtBondNo.Text + "',INM_BOND_DATE='" + Convert.ToDateTime(txtBondDate.Text).ToString("dd/MMM/yyyy") + "',INM_UT1_FILE_NO='" + txtUT1FileNo.Text + "',INM_FILE_NO='"+txtFileNo.Text+"',INM_EXPORT_FLAG='" + ChkExportUnderDutyClaim.Checked.ToString() + "',INM_VALID_DATE='" + Convert.ToDateTime(txtValidUpTo.Text).ToString("dd/MMM/yyyy") + "',INM_EXA_BOXES='" + txtExaminedBoxes.Text + "',INM_INVOICE_TYPE='" + Invoice_Type + "',INM_TOD='" + txtTrmOfDel.Text + "',INM_TOP='" + txtTrmOfPay.Text + "',INM_VOY_NO='" + txtVoyageNo.Text + "',INM_PLACE_REC='" + txtPlaceOfPreCarriage.Text + "',INM_M_NO='" + txtMarksNos.Text + "',INM_NOS_PACK='" + txtNoKindPkg.Text + "',INM_UN_NO='" + txtUnNo.Text + "',INM_HAZ='" + txtHASClass.Text + "',INM_HS_CODENO='" + txtHSCodeNo.Text + "',INM_CONTA_NO='" + txtContainer.Text + "',INM_SEAL_NO='" + txtBottleSealNo.Text + "',INM_OTS_NO='" + txtOTSNo.Text + "',INM_CONTRY_ORIGIN='" + ddlCOuntryOrigin.SelectedValue + "',INM_COUNTRY_DEST='" + ddlCountryOfDest.SelectedValue + "',INM_TRANSPORT_BY='" + txtTransportBy.Text + "',INM_ARE_REMARK='" + txtARERemark.Text + "',INM_CARRIER_NAME='" + txtCarrierName.Text + "',INM_CARRIER_BOOK_NO='" + txtCarrierBookNo.Text + "',INM_TECH_NAME='" + txtTechnicalName.Text + "',INM_OUT_PAKGS='" + txtOuterPack.Text + "',INM_INR_PAKG='" + txtInnerPack.Text + "',INM_SUB_CLASS='" + txtSubClass.Text + "',INM_UN_PAK_GRP='" + txtUnPackingGroup.Text + "',INM_UN_PAK_CODE='" + txtUnPackingCode.Text + "',INM_EMS_NO='" + txtEMSNo.Text + "',INM_FLASH_POINT='" + txtFlashPoint.Text + "',INM_MARINE_POLLT='" + Marrine_Pollutnet + "',INM_IEC_NO='" + txtIECNo.Text + "',INM_IEC_DATE='" + Convert.ToDateTime(txtIECDate.Text).ToString("dd/MMM/yyyy") + "',INM_CEN_EXC_REG='" + txtCentExcRegNo.Text + "',INM_DATE_OF_EXAM='" + Convert.ToDateTime(txtExamDate.Text).ToString("dd/MMM/yyyy") + "',INM_SUP_C_EXC_NAME='" + txtSuperintendentName.Text + "',INM_INSP_C_EXC_NAME='" + txtInspectorName.Text + "',INM_CUST_SEAL_NO='" + txtCustSealNo.Text + "',INM_PER_E_NO='" + txtStuffingPerNo.Text + "',INM_NONCARGO_NOPAKG='" + txtCargoNoOfPack.Text + "',INM_SHIPP_BILL_NO='" + txtShippingBillno.Text + "',INM_PACK_AMT=" + txtPackagingAmt.Text + ",INM_ACCESSIBLE_AMT=" + txtAccessable.Text + ",INM_BEXCISE=" + txtBasicExcPer.Text + ",INM_BE_AMT=" + txtBasicExcAmt.Text + ",INM_EDUC_CESS=" + txtducexcper.Text + ",INM_EDUC_AMT=" + txtEdueceAmt.Text + ",INM_H_EDUC_CESS=" + txtSHEExcPer.Text + ",INM_H_EDUC_AMT=" + txtSHEExcAmt.Text + ",INM_ADV_DUTY=" + txtAdvDuty.Text + ",INM_TAXABLE_AMT=" + txtTaxableAmt.Text + ",INM_T_CODE=" + ddlTaxName.SelectedValue + ",INM_S_TAX=" + txtSalesTaxPer.Text + ",INM_S_TAX_AMT=" + txtSalesTaxAmount.Text + ",INM_OTHER_AMT=" + txtOtherCharges.Text + ",INM_TRANS_AMT=" + txtTransportAmt.Text + ",INM_OCTRI_AMT=" + txtOctri.Text + ",INM_TAX_TCS_AMT=" + txtTCSAmt.Text + ",INM_ROUNDING_AMT=" + txtRoundingAmt.Text + ",INM_IS_SUPPLIMENT='" + IsSuppliement + "',INM_ISSU_TIME='" + txtIssuetime.Text + "',INM_REMOVEL_TIME='" + txtRemoveltime.Text + "',INM_LR_NO='" + txtLRNo.Text + "',INM_LC_NO='" + txtLCNo.Text + "',INM_LR_DATE='" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "',INM_LC_DATE='" + Convert.ToDateTime(txtLCDate.Text).ToString("dd/MMM/yyyy") + "',INM_TRANSPORT_OWNER='" + txtTransporterName.Text + "',INM_TRANSPORT_ADDRESS='" + txtTransportAdd.Text + "' where INM_CODE='" + mlCode + "'"))
                {
                    if (Invoice_Type == "2")
                    {
                        if (!chkIsSuppliement.Checked)
                        {
                            DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + mlCode + " ");
                            for (int i = 0; i < dtq.Rows.Count; i++)
                            {
                                CommonClasses.Execute("UPDATE CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");

                                CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");

                            }
                        }
                    }

                    //for (int i = 0; i < dtInvoiceDetail.Rows.Count; i++)
                    //{
                    //    DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY FROM INVOICE_DETAIL where IND_INM_CODE=" + mlCode + " and IND_I_CODE='" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "' and IND_CPOM_CODE='" + dtInvoiceDetail.Rows[i]["PO_CODE"] + "'");
                    //    CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[0]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtInvoiceDetail.Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "'");

                    //}
                    result = CommonClasses.Execute1("DELETE FROM INVOICE_DETAIL WHERE IND_INM_CODE='" + mlCode + "'");
                    if (result == true && Invoice_Type == "2")
                    {
                        if (!chkIsSuppliement.Checked)
                        {
                            result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + mlCode + "' and STL_DOC_TYPE='EXPINV'");
                        }
                    }
                    if (result)
                    {

                        for (int i = 0; i < dtInvoiceDetail.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_GROSS_WEIGHT,IND_NET_WEIGHT,IND_SIZE_OF_BOX,IND_NO_OF_BARRELS,IND_NO_PACK,IND_PACK_DESC,IND_CONTAINER_NO,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_BACHNO,IND_UOM_CODE) values ('" + mlCode + "','" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "','" + dtInvoiceDetail.Rows[i]["PO_CODE"] + "','" + dtInvoiceDetail.Rows[i]["INV_QTY"] + "','" + dtInvoiceDetail.Rows[i]["RATE"] + "','" + dtInvoiceDetail.Rows[i]["AMT"] + "','" + dtInvoiceDetail.Rows[i]["GROSS_WGHT"] + "','" + dtInvoiceDetail.Rows[i]["NET_WGHT"] + "','" + dtInvoiceDetail.Rows[i]["BOX_SIZE"] + "','" + dtInvoiceDetail.Rows[i]["NOOF_BARRELS"] + "','" + dtInvoiceDetail.Rows[i]["NO_OF_PACK"] + "','" + dtInvoiceDetail.Rows[i]["PACK_DESC"] + "','" + dtInvoiceDetail.Rows[i]["CONT_NO"] + "','" + dtInvoiceDetail.Rows[i]["IND_EX_AMT"] + "','" + dtInvoiceDetail.Rows[i]["IND_E_CESS_AMT"] + "','" + dtInvoiceDetail.Rows[i]["IND_SH_CESS_AMT"] + "','" + dtInvoiceDetail.Rows[i]["INV_QTY"] + "','" + dtInvoiceDetail.Rows[i]["IND_BACHNO"] + "','" + dtInvoiceDetail.Rows[i]["UOM_CODE"] + "')");
                            if (!chkIsSuppliement.Checked)
                            {
                                if (result == true && Invoice_Type == "2")
                                {
                                    result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + dtInvoiceDetail.Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + dtInvoiceDetail.Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "'");
                                }
                                //Entry In Stock Ledger
                                if (result == true && Invoice_Type == "2")
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "','" + mlCode + "','" + txtInvoiceNo.Text + "','EXPINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + dtInvoiceDetail.Rows[i]["INV_QTY"] + "')");

                                }
                                //Removing Stock
                                if (result == true && Invoice_Type == "2")
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtInvoiceDetail.Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + dtInvoiceDetail.Rows[i]["IND_I_CODE"] + "'");
                                }
                            }
                            
                        }
                        CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", mlCode);
                        dtInvoiceDetail.Rows.Clear();
                        CommonClasses.WriteLog("Export Invoice", "Modify", "Export Invoice", Convert.ToString(mlCode), Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewExportInvoice.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //   txtPONo.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Export Invoice", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select Customer Name";
                ddlCustomer.Focus();
                return;

            }
            if (txtDate.Text == "")
            {
                ShowMessage("#Avisos", "Select Invoice Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select Invoice Date";
                txtDate.Focus();
                return;
            }
            if (ddlCurrency.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Currancy", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCurrency.Focus();
                return;

            }
            if (ddlCOuntryOrigin.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Country Of Origin", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCurrency.Focus();
                return;

            }
            if (ddlCountryOfDest.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Country Of Destination", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCurrency.Focus();
                return;

            }
            if (dgInvoiceAddDetail.Enabled)
            {

            }
            else
            {
                ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Record Not Exist";
                return;
            }
            if (dgInvoiceAddDetail.Rows.Count > 0)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Exist", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
               // PanelMsg.Visible = true;
               // lblmsg.Text = "Record Not Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "btnSubmit_Click", Ex.Message);
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
                CancelRecord();
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
            CommonClasses.SendError("Export Invoice", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {

            CancelRecord();
            //SaveRec();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Export Invocie", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        CancelRecord();
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

            dtInvoiceDetail.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewExportInvoice.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Export Invoice", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion 

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDate.Text == "")
            {
                flag = false;
            }            
            else if (dgInvoiceAddDetail.Enabled && dgInvoiceAddDetail.Rows.Count > 0)
            {
                flag = true;
            }
            else
            {

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region GetGrantTot
    void GetGrantTot()
    {
        try
        {
            double decTotal = 0;

            double ExcBasic = 0;
            double Excedu = 0;
            double ExcSH = 0;


            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string QED_AMT = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblAMT"))).Text;
                    string Basic = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_EX_AMT"))).Text;
                    string EduCess = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_E_CESS_AMT"))).Text;
                    string SH = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_SH_CESS_AMT"))).Text;

                    double Amount = Convert.ToDouble(QED_AMT);
                    decTotal = decTotal + Amount;
                    ExcBasic = ExcBasic + Convert.ToDouble(Basic);
                    Excedu = Excedu + Convert.ToDouble(EduCess);
                    ExcSH = ExcSH + Convert.ToDouble(SH);
                }
               
            }


            if (txtNetAmount.Text == "")
            {
                txtNetAmount.Text = "0.00";
            }
            if (txtDiscount.Text == "")
            {
                txtDiscount.Text = "0.00";
            }
            if (txtFreight.Text == "")
            {
                txtFreight.Text = "0.00";
            }
            if (txtInsurance.Text == "")
            {
                txtInsurance.Text = "0.00";
            }
            if (txtNetAmount.Text == "")
            {
                txtNetAmount.Text = "0.00";
            }

            if (txtDiscount.Text == "")
            {
                txtDiscount.Text = "0.00";
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


            //if (txtFreight.Text == "")
            //{
            //    txtFreight.Text = "0";
            //}
            if (txtSalesTaxPer.Text == "")
            {
                txtSalesTaxPer.Text = "0.00";
            }

            if (txtSalesTaxAmount.Text == "")
            {
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtPackagingAmt.Text == "")
            {
                txtPackagingAmt.Text = "0.00";
            }

            if (txtGrandTotal.Text == "")
            {
                txtGrandTotal.Text = "0.00";
            }
            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));


            //Discu Amount
            txtDiscount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2));
            //Packing Amount
            txtPackagingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackagingAmt.Text), MidpointRounding.AwayFromZero));
            //Accessable Amount
            double AccessableValue = Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtDiscount.Text) + Convert.ToDouble(txtPackagingAmt.Text);
            txtAccessable.Text = AccessableValue.ToString();
            txtAccessable.Text = string.Format("{0:0.00}",Math.Round(Convert.ToDouble(txtAccessable.Text),MidpointRounding.AwayFromZero));

            //Basic Excise Amt
            double ExcAmt = (Convert.ToDouble(txtAccessable.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100));
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
            double TaxableAmt = Convert.ToDouble(txtAccessable.Text) + Convert.ToDouble(txtBasicExcAmt.Text) + Convert.ToDouble(txtEdueceAmt.Text) + Convert.ToDouble(txtSHEExcAmt.Text) + Convert.ToDouble(txtAdvDuty.Text);
            txtTaxableAmt.Text = TaxableAmt.ToString();
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtTaxableAmt.Text), MidpointRounding.AwayFromZero));

            //Tax Amt
            double TaxAmt = (Convert.ToDouble(txtTaxableAmt.Text) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
            txtSalesTaxAmount.Text = TaxAmt.ToString();
            txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSalesTaxAmount.Text), MidpointRounding.AwayFromZero));

            //GrandAmount
            txtGrandTotal.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtInsurance.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTCSAmt.Text) + Convert.ToDouble(txtRoundingAmt.Text))), MidpointRounding.AwayFromZero);


            
            
            //if (txtDiscPer.Text == "")
            //{
            //    txtDiscPer.Text = "0.00";
            //}
            //else
            //{
            //    txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtDiscPer.Text), 2));
            //}
            //txtDiscount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2);

            //double Total = (Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtInsurance.Text));
            //txtGrandTotal.Text = string.Format("{0:0.00}", Math.Round((Total - Convert.ToDouble(txtDiscount.Text)), 2));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice", "GetGrantTot", Ex.Message);
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
                dtFilter.Columns.Add(new System.Data.DataColumn("UOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("STOCK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PEND_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INV_QTY", typeof(String)));
              
                dtFilter.Columns.Add(new System.Data.DataColumn("RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("GROSS_WGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("NET_WGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BOX_SIZE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("NOOF_BARRELS", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("NO_OF_PACK", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PACK_DESC", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("CONT_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));


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
            CommonClasses.SendError("Export Invoice", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void txtSizeofBox_TextChanged(object sender, EventArgs e)
    {

        string totalStr = DecimalMasking(txtSizeofBox.Text);

        txtSizeofBox.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtNoofPackDesc_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtNoofPackDesc.Text);

        txtNoofPackDesc.Text = string.Format("{0:0}", Math.Round(Convert.ToDouble(totalStr), 2));
    }

    protected void txtCurrencyRate_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtCurrencyRate.Text);

        txtCurrencyRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
    }
}
