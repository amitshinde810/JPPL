using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;

public partial class Utility_ADD_EInvoice : System.Web.UI.Page
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
        }
    }

    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            DataTable FromInv = new DataTable();
            if (rbtGroup.SelectedValue == "0")
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='TAXINV' AND  INM_SUPPLEMENTORY=0 AND INM_IS_SUPPLIMENT=0 ORDER BY INM_NO ");
            }
            else if (rbtGroup.SelectedValue == "1")
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='OutJWINM' AND  INM_INV_TYPE=0 ORDER BY INM_NO ");
            }
            else if (rbtGroup.SelectedValue == "2")
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='OutJWINM' AND  INM_INV_TYPE=1 ORDER BY INM_NO ");
            }
            else if (rbtGroup.SelectedValue == "3")
            {
                FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='TAXINV' AND  INM_SUPPLEMENTORY=1 AND INM_IS_SUPPLIMENT=1 ORDER BY INM_NO ");
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
            CommonClasses.SendError("Tally transfer Sales", "LoadCombos", Ex.Message);
        }

    }
    #endregion



    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {


    }
    #endregion

    #region chkAllInv_CheckedChanged
    protected void chkAllInv_CheckedChanged(object sender, EventArgs e)
    {


    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {


    }
    #endregion

    #region chkAllStatus_CheckedChanged
    protected void chkAllStatus_CheckedChanged(object sender, EventArgs e)
    {


    }
    #endregion


    #region ddlInvType_TextChanged
    protected void ddlInvType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable FromInv = new DataTable();
            FromInv = CommonClasses.Execute("select distinct INM_CODE,INM_NO FROM INVOICE_MASTER WHERE  INM_CM_CODE=" + Session["CompanyCode"].ToString() + " and ES_DELETE=0 and INM_TYPE='TAXINV' ORDER BY INM_NO ");
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

        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

    #region MessageAlert
    public void setalert(string message)
    {
        string script = "window.onload = function() { Alertmsz('" + message + "'); };";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Alertmsz", script, true);
        return;
    }
    #endregion


    #region rbtGroup_SelectedIndexChanged
    protected void rbtGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();

    }
    #endregion rbtGroup_SelectedIndexChanged

    #region btnExport_Click
    protected void btnExport_Click(object sender, EventArgs e)
    {

        if ((ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0))
        {
            setalert("Please Select Invoice From & To");

            return;
        }

        try
        {
            string FromNo = ddlFromInvNo.SelectedItem.ToString();
            string toNo = ddlToInvNo.SelectedItem.ToString();
            #region Export_Excel
            DataTable dtResult = new DataTable();
            DataTable dt = new DataTable();
            if (rbtGroup.SelectedValue == "0")
            {
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,'1' AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE  INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'TAXINV' ))   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND  INM_SUPPLEMENTORY=0 AND INM_IS_SUPPLIMENT=0 AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            }
            else if (rbtGroup.SelectedValue == "1")
            {
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,'1' AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE   WHERE   INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'OutJWINM')) AND INM_INV_TYPE=0   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            }
            else if (rbtGroup.SelectedValue == "1")
            {
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,'1' AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE   INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'OutJWINM')) AND INM_INV_TYPE=1   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            }
            else
            {
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,'1' AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE  INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'TAXINV' ))   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND  INM_SUPPLEMENTORY=1 AND INM_IS_SUPPLIMENT=1 AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");

            }

            //DataTable dt = CommonClasses.Execute("SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,'1' AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE WHERE     (INVOICE_MASTER.INM_TYPE = 'TAXINV') AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            dtResult = dt;
            DataTable dtExport = new DataTable();
            if (dt.Rows.Count > 0)
            {
                dtExport.Columns.Add("Supply Type code *");
                dtExport.Columns.Add("Reverse Charge");
                dtExport.Columns.Add("e-Comm GSTIN");
                dtExport.Columns.Add("Igst On Intra");
                dtExport.Columns.Add("Document Type *");
                dtExport.Columns.Add("Document Number *");
                dtExport.Columns.Add("Document Date (DD/MM/YYYY) *");
                dtExport.Columns.Add("Buyer GSTIN *");
                dtExport.Columns.Add("Buyer Legal Name *");
                dtExport.Columns.Add("Buyer Trade Name ");
                dtExport.Columns.Add("Buyer POS *");
                dtExport.Columns.Add("Buyer Addr1 *");
                dtExport.Columns.Add("Buyer Addr2");
                dtExport.Columns.Add("Buyer Location *");
                dtExport.Columns.Add("Buyer Pin Code");
                dtExport.Columns.Add("Buyer State *");
                dtExport.Columns.Add("Buyer Phone Number");
                dtExport.Columns.Add("Buyer Email Id");
                dtExport.Columns.Add("Dispatch Name");
                dtExport.Columns.Add("Dispatch Addr1");
                dtExport.Columns.Add("Dispatch Addr2");
                dtExport.Columns.Add("Dispatch Location");
                dtExport.Columns.Add("Dispatch Pin Code");
                dtExport.Columns.Add("Dispatch State");
                dtExport.Columns.Add("Shipping GSTIN ");
                dtExport.Columns.Add("Shipping Legal Name");
                dtExport.Columns.Add("Shipping Trade Name ");
                dtExport.Columns.Add("Shipping Addr1");
                dtExport.Columns.Add("Shipping Addr2");
                dtExport.Columns.Add("Shipping Location");
                dtExport.Columns.Add("Shipping Pin Code");
                dtExport.Columns.Add("Shipping State");
                dtExport.Columns.Add("Sl.No. *");
                dtExport.Columns.Add("Product Description");
                dtExport.Columns.Add("Is_Service *");
                dtExport.Columns.Add("HSN code *");
                dtExport.Columns.Add("Bar code ");
                dtExport.Columns.Add("Quantity *");
                dtExport.Columns.Add("Free Quantity");
                dtExport.Columns.Add("Unit *");
                dtExport.Columns.Add("Unit Price *");
                dtExport.Columns.Add("Gross Amount *");
                dtExport.Columns.Add("Discount");
                dtExport.Columns.Add("Pre Tax Value");
                dtExport.Columns.Add("Taxable value *");
                dtExport.Columns.Add("GST Rate (%) *");
                dtExport.Columns.Add("Sgst Amt(Rs)");
                dtExport.Columns.Add("Cgst Amt (Rs)");
                dtExport.Columns.Add("Igst Amt (Rs)");
                dtExport.Columns.Add("Cess Rate (%)");
                dtExport.Columns.Add("Cess Amt Adval (Rs)");
                dtExport.Columns.Add("Cess Non Adval Amt (Rs)");
                dtExport.Columns.Add("State Cess Rate (%)");
                dtExport.Columns.Add("State Cess Adval Amt (Rs)");
                dtExport.Columns.Add("State Cess Non-Adval Amt (Rs)");
                dtExport.Columns.Add("Other Charges  ");
                dtExport.Columns.Add("Item Total *");
                //dtExport.Columns.Add("Order line reference");
                //dtExport.Columns.Add("Orgin country");
                //dtExport.Columns.Add(" Unique item Sl. No.");
                dtExport.Columns.Add("Batch Name");
                dtExport.Columns.Add("Batch Expiry Dt");
                dtExport.Columns.Add("Warranty Dt");
                //dtExport.Columns.Add("Attribute Details of the items");
                //dtExport.Columns.Add("Attribute Value of the Items");
                dtExport.Columns.Add("Total Taxable value *");
                dtExport.Columns.Add("Sgst Amt");
                dtExport.Columns.Add("Cgst Amt");
                dtExport.Columns.Add("Igst Amt");
                dtExport.Columns.Add("Cess Amt");
                dtExport.Columns.Add("State Cess Amt");
                dtExport.Columns.Add("Discount ");
                dtExport.Columns.Add("Other charges");
                dtExport.Columns.Add("Round off");
                dtExport.Columns.Add("Total Invoice value *");
                // dtExport.Columns.Add("Total Invoice value  in Additional Currency");
                dtExport.Columns.Add("Shipping Bill No");
                dtExport.Columns.Add("Shipping Bill Dt");
                dtExport.Columns.Add("Port");
                dtExport.Columns.Add("Supplier Refund");
                dtExport.Columns.Add("Foreign Currency ");
                dtExport.Columns.Add("Country Code ");
                dtExport.Columns.Add("Export Duty Amount");
                dtExport.Columns.Add("Trans ID");
                dtExport.Columns.Add("Trans Name");
                dtExport.Columns.Add("Trans Mode ");
                dtExport.Columns.Add("Distance ");
                dtExport.Columns.Add("Trans Doc No.");
                dtExport.Columns.Add("Trans Doc Date");
                dtExport.Columns.Add("Vehicle No.");
                dtExport.Columns.Add("Vehicle Type");
                //dtExport.Columns.Add("Payee Name");
                //dtExport.Columns.Add("Account Number");
                //dtExport.Columns.Add("Mode");
                //dtExport.Columns.Add("Branch/IFSC Code");
                //dtExport.Columns.Add("Term  of payment");
                //dtExport.Columns.Add("Payment instruction");
                //dtExport.Columns.Add("Credit Transfer");
                //dtExport.Columns.Add("direct debit");
                //dtExport.Columns.Add("credit days");
                //dtExport.Columns.Add("Paided amount");
                //dtExport.Columns.Add("Due amount");
                //dtExport.Columns.Add("Remarks");
                //dtExport.Columns.Add("Invoice period start date");
                //dtExport.Columns.Add("Invoice period end date");
                //dtExport.Columns.Add("Original Invoice");
                //dtExport.Columns.Add("Preceding Invoice Date");
                //dtExport.Columns.Add("Other Reference");
                //dtExport.Columns.Add("Receipt Advice Number");
                //dtExport.Columns.Add("Date of Receipt Advice");
                //dtExport.Columns.Add("Lot/Batch Reference Number");
                //dtExport.Columns.Add("Contract Reference Number");
                //dtExport.Columns.Add("Any other reference");
                //dtExport.Columns.Add("Project Reference Number");
                //dtExport.Columns.Add("Vendor PO Reference Number");
                //dtExport.Columns.Add("Vendor PO Reference date");
                //dtExport.Columns.Add("Supporting Doc URL");
                //dtExport.Columns.Add("Supporting Doc in Base 64 format");
                //dtExport.Columns.Add("Any additional information");
                dtExport.Columns.Add("Error List");


                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtExport.Rows.Add(dtResult.Rows[i]["SUPPLY_TYPE"].ToString(),
    dtResult.Rows[i]["REV"].ToString(),
    dtResult.Rows[i]["E_GST"].ToString(),
    dtResult.Rows[i]["IGST_ONINTRA"].ToString(),
    dtResult.Rows[i]["DOC_TYPE"].ToString(),
    dtResult.Rows[i]["INM_TNO"].ToString(),
     Convert.ToDateTime(dtResult.Rows[i]["INM_DATE"].ToString()).ToString("dd/MM/yyyy"),
    dtResult.Rows[i]["P_GST_NO"].ToString(),
    dtResult.Rows[i]["P_NAME"].ToString(),
    dtResult.Rows[i]["P_NAME"].ToString(),
    dtResult.Rows[i]["SM_NAME"].ToString(),
    dtResult.Rows[i]["P_ADD1"].ToString(),
    dtResult.Rows[i]["P_ADD2"].ToString(),
    dtResult.Rows[i]["BUY_LOCATION"].ToString(),
    dtResult.Rows[i]["P_PIN_CODE"].ToString(),
    dtResult.Rows[i]["BUY_STATE"].ToString(),
    dtResult.Rows[i]["P_MOB"].ToString(),
    dtResult.Rows[i]["P_EMAIL"].ToString(),
    dtResult.Rows[i]["DISPATCH_NAME"].ToString(),
    dtResult.Rows[i]["DIS_ADD1"].ToString(),
    dtResult.Rows[i]["DIS_ADD2"].ToString(),
    dtResult.Rows[i]["DIS_LOCATION"].ToString(),
    dtResult.Rows[i]["DIS_PIN"].ToString(),
    dtResult.Rows[i]["DIS_STATE"].ToString(),
    dtResult.Rows[i]["SHIPPING_GST"].ToString(),
    dtResult.Rows[i]["SHIPPING_LNAME"].ToString(),
    dtResult.Rows[i]["SHIPPING_TNAME"].ToString(),
    dtResult.Rows[i]["SHIPPING_ADD1"].ToString(),
    dtResult.Rows[i]["SHIPPING_ADD2"].ToString(),
    dtResult.Rows[i]["SHIPPING_LOCATION"].ToString(),
    dtResult.Rows[i]["SHIPPING_PIN"].ToString(),
    dtResult.Rows[i]["SHIPPING_STATE"].ToString(),
    dtResult.Rows[i]["SR_NO"].ToString(),
    dtResult.Rows[i]["I_CODENO"].ToString(),
    dtResult.Rows[i]["ISSERVICE"].ToString(),
    dtResult.Rows[i]["IND_E_TARIFF_NO"].ToString(),
    dtResult.Rows[i]["BARCODE"].ToString(),
    dtResult.Rows[i]["IND_INQTY"].ToString(),
    dtResult.Rows[i]["FREE_QTY"].ToString(),
    dtResult.Rows[i]["I_UOM_DESC"].ToString(),
    dtResult.Rows[i]["IND_RATE"].ToString(),
    dtResult.Rows[i]["IND_AMT"].ToString(),
    dtResult.Rows[i]["INM_DISC_AMT"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_ACCESSIBLE_AMT"].ToString(),
    dtResult.Rows[i]["GST_PER"].ToString(),
    dtResult.Rows[i]["IND_EX_AMT"].ToString(),
    dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
    dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
    dtResult.Rows[i]["CESS_RATE"].ToString(),
    dtResult.Rows[i]["CESS_AMT"].ToString(),
    dtResult.Rows[i]["CESS_NON_ADV"].ToString(),
    dtResult.Rows[i]["STATE_CESS_RATE"].ToString(),
    dtResult.Rows[i]["STAE_CESS_AMT"].ToString(),
    dtResult.Rows[i]["STATE_CESS_NON_ADV"].ToString(),
    dtResult.Rows[i]["OTHER_CHARGES"].ToString(),
    dtResult.Rows[i]["ITEM_TOTAL"].ToString(),
                        //dtResult.Rows[i]["ORDER_LINE"].ToString(),
                        //dtResult.Rows[i]["ORIGIN_COUNTRY"].ToString(),
                        //dtResult.Rows[i]["UNI_SR_NO"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_ACCESSIBLE_AMT"].ToString(),
    dtResult.Rows[i]["INM_BE_AMT"].ToString(),
    dtResult.Rows[i]["INM_EDUC_AMT"].ToString(),
    dtResult.Rows[i]["INM_H_EDUC_AMT"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_TAX_TCS_AMT"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_G_AMT"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString()
                                     );
                }
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=E_Invoice.xls");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
            "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
            "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = dtExport.Columns.Count;
            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            for (int k = 0; k < dtExport.Rows.Count; k++)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    if (i == dtExport.Columns.Count - 1)
                    {
                        HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    else
                    {
                        if (i == 33 || i == 6)
                        {
                            if (dtExport.Rows[k]["Document Date (DD/MM/YYYY) *"].ToString().Length > 0)
                            {
                                HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            }
                            else if (dtExport.Rows[k]["Product Description"].ToString().Length > 0)
                            {
                                HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<Td>");
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<Td>");
                        }
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            #endregion Export_Excel

        }

        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion



    private String[] GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            // Connection String. Change the excel file to the file you
            // will search.
            String connString = "";
            try
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
            }
            catch (Exception ex)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\";";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
            }


            // Get the data table containg the schema guid.
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }

            String[] excelSheets = new String[dt.Rows.Count];
            int i = 0;

            // Add the sheet name to the string array.
            foreach (DataRow row in dt.Rows)
            {
                excelSheets[i] = row["TABLE_NAME"].ToString();
                i++;
            }

            // Loop through all of the sheets if you want too...
            for (int j = 0; j < excelSheets.Length; j++)
            {
                // Query each excel sheet.
            }

            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            // Clean up.
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }


    protected void btnImport1_Click(object sender, EventArgs e)
    {
        if (FileUpload2.HasFile)
        {

            DirectoryInfo DI = new DirectoryInfo(Server.MapPath(@"~/UpLoadPath/"));
            FileInfo[] Delfiles = DI.GetFiles("*.xlsx");
            int ifiles = 0;
            foreach (FileInfo fi in Delfiles)
            {
                System.IO.File.Delete(DI + "/" + Delfiles[ifiles]);
                ifiles++;
            }
            string filename1 = Path.GetFileName(FileUpload2.PostedFile.FileName);
            FileUpload2.SaveAs(Server.MapPath(@"~/UpLoadPath/" + filename1));
            string filename = Server.MapPath(@"~/UpLoadPath/" + filename1);
            //string SheetName = "Sheet1";

            string[] SheetName1 = GetExcelSheetNames(filename);
            string SheetName = SheetName1[0].ToString();// "ContractualMonthAttend-3";
            string Con = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                        @"Data Source=" + filename + ";" +
                        @"Extended Properties=Excel 12.0"; //+Convert.ToChar(34).ToString() +
            //@"Excel 8.0;" + "Imex=2;" + "HDR=Yes;" + Convert.ToChar(34).ToString();
            OleDbConnection oleConn = new OleDbConnection(Con);
            oleConn.Open();
            OleDbCommand oleCmdSelect = new OleDbCommand();
            oleCmdSelect = new OleDbCommand(
                    @"SELECT * FROM ["
                    + SheetName +
                    "]", oleConn);
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            oleAdapter.SelectCommand = oleCmdSelect;
            DataTable dt = new DataTable("Table1");
            oleAdapter.FillSchema(dt, SchemaType.Source);
            oleAdapter.Fill(dt);
            oleCmdSelect.Dispose();
            oleCmdSelect = null;
            oleAdapter.Dispose();
            oleAdapter = null;
            oleConn.Dispose();
            oleConn = null;
            bool res = false;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //CommonClasses.Execute("UPDATE  INVOICE_MASTER SET AckNo='" + dt.Rows[i][2].ToString() + "', AckDate='" + dt.Rows[i][3].ToString() + "' , InvValue='" + dt.Rows[i][7].ToString() + "', ReciptGSTIn='" + dt.Rows[i][8].ToString() + "' , EInvStatus='" + dt.Rows[i][9].ToString() + "', IRN='" + dt.Rows[i][1].ToString() + "' , QRCode='" + dt.Rows[i][10].ToString() + "', EwayBill='" + dt.Rows[i][11].ToString() + "'   where  INM_TNO='" + dt.Rows[i][4].ToString() + "'    AND INM_CM_CODE='" + Session["CompanyCode"].ToString() + "'");

                if (CommonClasses.Execute1("UPDATE  INVOICE_MASTER SET AckNo='" + dt.Rows[i][2].ToString() + "', AckDate='" + dt.Rows[i][3].ToString() + "' , InvValue='" + dt.Rows[i][7].ToString() + "', ReciptGSTIn='" + dt.Rows[i][8].ToString() + "' , EInvStatus='" + dt.Rows[i][9].ToString() + "', IRN='" + dt.Rows[i][1].ToString() + "' , QRCode='" + dt.Rows[i][10].ToString() + "', EwayBill='" + dt.Rows[i][11].ToString() + "'   where  INM_TNO='" + dt.Rows[i][4].ToString() + "'  AND      INM_CM_CODE='" + Session["CompanyCode"].ToString() + "'"))
                {
                    res = true;
                    CommonClasses.WriteLog("IRN Upload", "Upload", "IRN Upload", dt.Rows[i][3].ToString(), Convert.ToInt32(dt.Rows[i][0].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                }
                else
                {
                    res = false;
                    break;
                }
            }
            if (res)
            {
                setalert("E Invoice Added Successfully...");
            }
            else
            {
                setalert("E Invoice Not Uploaded Successfully... Please again Upload file ..................");
            }

            //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }

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



    #region btnExportB_Click
    protected void btnExportB_Click(object sender, EventArgs e)
    {

        if ((ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0))
        {
            setalert("Please Select Invoice From & To");

            return;
        }

        try
        {
            string FromNo = ddlFromInvNo.SelectedItem.ToString();
            string toNo = ddlToInvNo.SelectedItem.ToString();
            #region Export_Excel
            DataTable dtResult = new DataTable();
            DataTable dt = new DataTable();
            if (rbtGroup.SelectedValue == "0")
            {
                //dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_HSN_CODE AS IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE  INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'TAXINV' ))   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, REPLACE (CONVERT(varchar, INM_DATE, 103),'-','/') AS   INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE, IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE  INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'TAXINV' ))   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");

            }
            else
            {
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, REPLACE (CONVERT(varchar, INM_DATE, 103),'-','/') AS  INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE, IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE   WHERE   INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'OutJWINM')) AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            }


            //DataTable dt = CommonClasses.Execute("SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE WHERE     (INVOICE_MASTER.INM_TYPE = 'TAXINV') AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            dtResult = dt;
            DataTable dtExport = new DataTable();
            if (dt.Rows.Count > 0)
            {
                dtExport.Columns.Add("Supply Type code *");
                dtExport.Columns.Add("Reverse Charge");
                dtExport.Columns.Add("e-Comm GSTIN");
                dtExport.Columns.Add("Igst On Intra");
                dtExport.Columns.Add("Document Type *");
                dtExport.Columns.Add("Document Number *");
                dtExport.Columns.Add("Document Date (DD/MM/YYYY) *");
                dtExport.Columns.Add("Buyer GSTIN *");
                dtExport.Columns.Add("Buyer Legal Name *");
                dtExport.Columns.Add("Buyer Trade Name ");
                dtExport.Columns.Add("Buyer POS *");
                dtExport.Columns.Add("Buyer Addr1 *");
                dtExport.Columns.Add("Buyer Addr2");
                dtExport.Columns.Add("Buyer Location *");
                dtExport.Columns.Add("Buyer Pin Code");
                dtExport.Columns.Add("Buyer State *");
                dtExport.Columns.Add("Buyer Phone Number");
                dtExport.Columns.Add("Buyer Email Id");
                dtExport.Columns.Add("Dispatch Name");
                dtExport.Columns.Add("Dispatch Addr1");
                dtExport.Columns.Add("Dispatch Addr2");
                dtExport.Columns.Add("Dispatch Location");
                dtExport.Columns.Add("Dispatch Pin Code");
                dtExport.Columns.Add("Dispatch State");
                dtExport.Columns.Add("Shipping GSTIN ");
                dtExport.Columns.Add("Shipping Legal Name");
                dtExport.Columns.Add("Shipping Trade Name ");
                dtExport.Columns.Add("Shipping Addr1");
                dtExport.Columns.Add("Shipping Addr2");
                dtExport.Columns.Add("Shipping Location");
                dtExport.Columns.Add("Shipping Pin Code");
                dtExport.Columns.Add("Shipping State");
                //dtExport.Columns.Add("Sl.No. *");
                //dtExport.Columns.Add("Product Description");
                //dtExport.Columns.Add("Is_Service *");
                //dtExport.Columns.Add("HSN code *");
                //dtExport.Columns.Add("Bar code ");
                //dtExport.Columns.Add("Quantity *");
                //dtExport.Columns.Add("Free Quantity");
                //dtExport.Columns.Add("Unit *");
                //dtExport.Columns.Add("Unit Price *");
                //dtExport.Columns.Add("Gross Amount *");
                //dtExport.Columns.Add("Discount");
                //dtExport.Columns.Add("Pre Tax Value");
                //dtExport.Columns.Add("Taxable value *");
                //dtExport.Columns.Add("GST Rate (%) *");
                //dtExport.Columns.Add("Sgst Amt(Rs)");
                //dtExport.Columns.Add("Cgst Amt (Rs)");
                //dtExport.Columns.Add("Igst Amt (Rs)");
                //dtExport.Columns.Add("Cess Rate (%)");
                //dtExport.Columns.Add("Cess Amt Adval (Rs)");
                //dtExport.Columns.Add("Cess Non Adval Amt (Rs)");
                //dtExport.Columns.Add("State Cess Rate (%)");
                //dtExport.Columns.Add("State Cess Adval Amt (Rs)");
                //dtExport.Columns.Add("State Cess Non-Adval Amt (Rs)");
                //dtExport.Columns.Add("Other Charges  ");
                //dtExport.Columns.Add("Item Total *");
                ////dtExport.Columns.Add("Order line reference");
                ////dtExport.Columns.Add("Orgin country");
                ////dtExport.Columns.Add(" Unique item Sl. No.");
                //dtExport.Columns.Add("Batch Name");
                //dtExport.Columns.Add("Batch Expiry Dt");
                //dtExport.Columns.Add("Warranty Dt");
                ////dtExport.Columns.Add("Attribute Details of the items");
                ////dtExport.Columns.Add("Attribute Value of the Items");
                dtExport.Columns.Add("Total Taxable value *");
                dtExport.Columns.Add("Sgst Amt");
                dtExport.Columns.Add("Cgst Amt");
                dtExport.Columns.Add("Igst Amt");
                dtExport.Columns.Add("Cess Amt");
                dtExport.Columns.Add("State Cess Amt");
                dtExport.Columns.Add("Discount ");
                dtExport.Columns.Add("Other charges");
                dtExport.Columns.Add("Round off");
                dtExport.Columns.Add("Total Invoice value *");
                // dtExport.Columns.Add("Total Invoice value  in Additional Currency");
                dtExport.Columns.Add("Shipping Bill No");
                dtExport.Columns.Add("Shipping Bill Dt");
                dtExport.Columns.Add("Port");
                dtExport.Columns.Add("Supplier Refund");
                dtExport.Columns.Add("Foreign Currency ");
                dtExport.Columns.Add("Country Code ");
                dtExport.Columns.Add("Export Duty Amount");
                dtExport.Columns.Add("Trans ID");
                dtExport.Columns.Add("Trans Name");
                dtExport.Columns.Add("Trans Mode ");
                dtExport.Columns.Add("Distance ");
                dtExport.Columns.Add("Trans Doc No.");
                dtExport.Columns.Add("Trans Doc Date");
                dtExport.Columns.Add("Vehicle No.");
                dtExport.Columns.Add("Vehicle Type");
                //dtExport.Columns.Add("Payee Name");
                //dtExport.Columns.Add("Account Number");
                //dtExport.Columns.Add("Mode");
                //dtExport.Columns.Add("Branch/IFSC Code");
                //dtExport.Columns.Add("Term  of payment");
                //dtExport.Columns.Add("Payment instruction");
                //dtExport.Columns.Add("Credit Transfer");
                //dtExport.Columns.Add("direct debit");
                //dtExport.Columns.Add("credit days");
                //dtExport.Columns.Add("Paided amount");
                //dtExport.Columns.Add("Due amount");
                //dtExport.Columns.Add("Remarks");
                //dtExport.Columns.Add("Invoice period start date");
                //dtExport.Columns.Add("Invoice period end date");
                //dtExport.Columns.Add("Original Invoice");
                //dtExport.Columns.Add("Preceding Invoice Date");
                //dtExport.Columns.Add("Other Reference");
                //dtExport.Columns.Add("Receipt Advice Number");
                //dtExport.Columns.Add("Date of Receipt Advice");
                //dtExport.Columns.Add("Lot/Batch Reference Number");
                //dtExport.Columns.Add("Contract Reference Number");
                //dtExport.Columns.Add("Any other reference");
                //dtExport.Columns.Add("Project Reference Number");
                //dtExport.Columns.Add("Vendor PO Reference Number");
                //dtExport.Columns.Add("Vendor PO Reference date");
                //dtExport.Columns.Add("Supporting Doc URL");
                //dtExport.Columns.Add("Supporting Doc in Base 64 format");
                //dtExport.Columns.Add("Any additional information");
                dtExport.Columns.Add("Error List");


                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtExport.Rows.Add(dtResult.Rows[i]["SUPPLY_TYPE"].ToString(),
    dtResult.Rows[i]["REV"].ToString(),
    dtResult.Rows[i]["E_GST"].ToString(),
    dtResult.Rows[i]["IGST_ONINTRA"].ToString(),
    dtResult.Rows[i]["DOC_TYPE"].ToString(),
    dtResult.Rows[i]["INM_TNO"].ToString(),
    dtResult.Rows[i]["INM_DATE"].ToString().Replace('-', '/'),
    dtResult.Rows[i]["P_GST_NO"].ToString(),
    dtResult.Rows[i]["P_NAME"].ToString(),
    dtResult.Rows[i]["P_NAME"].ToString(),
    dtResult.Rows[i]["SM_NAME"].ToString(),
    dtResult.Rows[i]["P_ADD1"].ToString(),
    dtResult.Rows[i]["P_ADD2"].ToString(),
    dtResult.Rows[i]["BUY_LOCATION"].ToString(),
    dtResult.Rows[i]["P_PIN_CODE"].ToString(),
    dtResult.Rows[i]["BUY_STATE"].ToString(),
    dtResult.Rows[i]["P_MOB"].ToString(),
    dtResult.Rows[i]["P_EMAIL"].ToString(),
    dtResult.Rows[i]["DISPATCH_NAME"].ToString(),
    dtResult.Rows[i]["DIS_ADD1"].ToString(),
    dtResult.Rows[i]["DIS_ADD2"].ToString(),
    dtResult.Rows[i]["DIS_LOCATION"].ToString(),
    dtResult.Rows[i]["DIS_PIN"].ToString(),
    dtResult.Rows[i]["DIS_STATE"].ToString(),
    dtResult.Rows[i]["SHIPPING_GST"].ToString(),
    dtResult.Rows[i]["SHIPPING_LNAME"].ToString(),
    dtResult.Rows[i]["SHIPPING_TNAME"].ToString(),
    dtResult.Rows[i]["SHIPPING_ADD1"].ToString(),
    dtResult.Rows[i]["SHIPPING_ADD2"].ToString(),
    dtResult.Rows[i]["SHIPPING_LOCATION"].ToString(),
    dtResult.Rows[i]["SHIPPING_PIN"].ToString(),
    dtResult.Rows[i]["SHIPPING_STATE"].ToString(),
                        //dtResult.Rows[i]["SR_NO"].ToString(),
                        //dtResult.Rows[i]["I_CODENO"].ToString(),
                        //dtResult.Rows[i]["ISSERVICE"].ToString(),
                        //dtResult.Rows[i]["IND_E_TARIFF_NO"].ToString(),
                        //dtResult.Rows[i]["BARCODE"].ToString(),
                        //dtResult.Rows[i]["IND_INQTY"].ToString(),
                        //dtResult.Rows[i]["FREE_QTY"].ToString(),
                        //dtResult.Rows[i]["I_UOM_DESC"].ToString(),
                        //dtResult.Rows[i]["IND_RATE"].ToString(),
                        //dtResult.Rows[i]["IND_AMT"].ToString(),
                        //dtResult.Rows[i]["INM_DISC_AMT"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["INM_ACCESSIBLE_AMT"].ToString(),
                        //dtResult.Rows[i]["GST_PER"].ToString(),
                        //dtResult.Rows[i]["IND_EX_AMT"].ToString(),
                        //dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
                        //dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
                        //dtResult.Rows[i]["CESS_RATE"].ToString(),
                        //dtResult.Rows[i]["CESS_AMT"].ToString(),
                        //dtResult.Rows[i]["CESS_NON_ADV"].ToString(),
                        //dtResult.Rows[i]["STATE_CESS_RATE"].ToString(),
                        //dtResult.Rows[i]["STAE_CESS_AMT"].ToString(),
                        //dtResult.Rows[i]["STATE_CESS_NON_ADV"].ToString(),
                        //dtResult.Rows[i]["OTHER_CHARGES"].ToString(),
                        //dtResult.Rows[i]["INM_G_AMT"].ToString(),
                        //dtResult.Rows[i]["ITEM_TOTAL"].ToString(),
                        //dtResult.Rows[i]["ORDER_LINE"].ToString(),
                        //dtResult.Rows[i]["ORIGIN_COUNTRY"].ToString(),
                        //dtResult.Rows[i]["UNI_SR_NO"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_ACCESSIBLE_AMT"].ToString(),
    dtResult.Rows[i]["INM_BE_AMT"].ToString(),
    dtResult.Rows[i]["INM_EDUC_AMT"].ToString(),
    dtResult.Rows[i]["INM_H_EDUC_AMT"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_TAX_TCS_AMT"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["INM_G_AMT"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["BLANK"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString()
                                     );
                }
            }


            //DataTable distinctTable = dtExport.DefaultView.ToTable( /*distinct*/ true);
            dtExport = dtExport.DefaultView.ToTable( /*distinct*/ true);


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=E_Invoice.xls");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
            "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
            "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = dtExport.Columns.Count;
            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            for (int k = 0; k < dtExport.Rows.Count; k++)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    if (i == dtExport.Columns.Count - 1)
                    {
                        HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    else
                    {
                        if (i == 33 || i == 6)
                        {
                            if (dtExport.Rows[k]["Document Date (DD/MM/YYYY) *"].ToString().Length > 0)
                            {
                                HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            }
                            else if (dtExport.Rows[k]["Product Description"].ToString().Length > 0)
                            {
                                HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<Td>");
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<Td>");
                        }
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            #endregion Export_Excel

        }

        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

    #region btnExportBDetails
    protected void btnExportBDetails_Click(object sender, EventArgs e)
    {

        if ((ddlFromInvNo.SelectedIndex == 0 || ddlToInvNo.SelectedIndex == 0))
        {
            setalert("Please Select Invoice From & To");

            return;
        }

        try
        {
            string FromNo = ddlFromInvNo.SelectedItem.ToString();
            string toNo = ddlToInvNo.SelectedItem.ToString();
            #region Export_Excel
            DataTable dtResult = new DataTable();
            DataTable dt = new DataTable();
            if (rbtGroup.SelectedValue == "0")
            {
                //dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_HSN_CODE AS IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE  INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'TAXINV' ))   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO,  REPLACE (CONVERT(varchar, INM_DATE, 103),'-','/') AS INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE, IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE   AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES,  IND_AMT+ IND_EX_AMT +	IND_E_CESS_AMT +	IND_SH_CESS_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  WHERE  INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'TAXINV' ))   AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");

            }
            else
            {
                //dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, REPLACE (CONVERT(varchar, INM_DATE, 103),'-','/') AS  INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_HSN_CODE AS IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,E_BASIC_CentralT+E_EDU_CESS_State+E_H_EDU_Integrated   AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES,  IND_AMT+ IND_EX_AMT +	IND_E_CESS_AMT +	IND_SH_CESS_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE   WHERE   INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'OutJWINM')) AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
                dt = CommonClasses.Execute(" SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, REPLACE (CONVERT(varchar, INM_DATE, 103),'-','/') AS  INM_DATE,P_LBT_NO AS P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE, IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT, EXCISE_TARIFF_MASTER.E_H_EDU    AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES,  IND_AMT+ IND_EX_AMT +	IND_E_CESS_AMT +	IND_SH_CESS_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE  INNER JOIN EXCISE_TARIFF_MASTER ON ITEM_MASTER.I_SCAT_CODE=E_CODE  WHERE   INVOICE_MASTER.ES_DELETE=0 AND   (INVOICE_MASTER.INM_TYPE in ( 'OutJWINM')) AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            }


            //DataTable dt = CommonClasses.Execute("SELECT   '' AS BLANK, INM_TAX_TCS_AMT,  'B2B' AS SUPPLY_TYPE,'NO' AS REV,'' AS E_GST,'NO' IGST_ONINTRA,'Tax Invoice' AS DOC_TYPE,INM_TNO, convert(varchar,INM_DATE,105) AS INM_DATE,P_GST_NO,P_NAME,P_NAME,SM_NAME,P_ADD1,'' As P_ADD2,CITY_NAME AS BUY_LOCATION,P_PIN_CODE,SM_NAME AS BUY_STATE,P_MOB,P_EMAIL,'' AS DISPATCH_NAME,'' AS DIS_ADD1,'' AS DIS_ADD2,'' AS DIS_LOCATION,'' AS DIS_PIN,'' AS DIS_STATE,'' AS SHIPPING_GST,'' AS SHIPPING_LNAME,'' AS SHIPPING_TNAME,'' AS SHIPPING_ADD1,'' AS SHIPPING_ADD2,'' AS SHIPPING_LOCATION,'' AS SHIPPING_PIN,'' AS SHIPPING_STATE,ROW_NUMBER() Over (Order by IND_INM_CODE) AS SR_NO,I_CODENO,case when INM_TYPE='OutJWINM' then 'YES' ELSE 'NO' END as ISSERVICE,IND_E_TARIFF_NO,'' AS BARCODE,IND_INQTY ,'' AS FREE_QTY,I_UOM_DESC,IND_RATE,IND_AMT,INM_ACCESSIBLE_AMT,INM_EDUC_CESS+INM_H_EDUC_CESS+INM_BEXCISE  AS GST_PER,IND_EX_AMT,	IND_E_CESS_AMT,	IND_SH_CESS_AMT , INM_DISC_AMT,'' As CESS_RATE,'' As CESS_AMT,'' As CESS_NON_ADV,'' As STATE_CESS_RATE,'' As STAE_CESS_AMT,'' As STATE_CESS_NON_ADV,'' As OTHER_CHARGES, INM_TAXABLE_AMT As ITEM_TOTAL,'' AS ORDER_LINE,'' AS ORIGIN_COUNTRY,'' AS UNI_SR_NO, INM_BE_AMT,INM_EDUC_AMT,INM_H_EDUC_AMT ,INM_ROUNDING_AMT,INM_G_AMT,INM_TRANSPORT,INM_VEH_NO  FROM         INVOICE_MASTER INNER JOIN                      INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN                      PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN                      ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN                      ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN                      STATE_MASTER ON PARTY_MASTER.P_SM_CODE = STATE_MASTER.SM_CODE INNER JOIN                      CITY_MASTER ON PARTY_MASTER.P_CITY_CODE = CITY_MASTER.CITY_CODE INNER JOIN                      COUNTRY_MASTER ON PARTY_MASTER.P_COUNTRY_CODE = COUNTRY_MASTER.COUNTRY_CODE WHERE     (INVOICE_MASTER.INM_TYPE = 'TAXINV') AND (INVOICE_MASTER.INM_CM_CODE = '" + Session["CompanyCode"].ToString() + "') AND (INVOICE_MASTER.INM_NO between '" + FromNo + "' AND '" + toNo + "')");
            dtResult = dt;
            DataTable dtExport = new DataTable();
            if (dt.Rows.Count > 0)
            {

                dtExport.Columns.Add("Document Type *");
                dtExport.Columns.Add("Document Number *");
                dtExport.Columns.Add("Document Date (DD/MM/YYYY) *");

                dtExport.Columns.Add("Sl.No. *");
                dtExport.Columns.Add("Product Description");
                dtExport.Columns.Add("Is_Service *");
                dtExport.Columns.Add("HSN code *");
                dtExport.Columns.Add("Bar code ");
                dtExport.Columns.Add("Quantity *");
                dtExport.Columns.Add("Free Quantity");
                dtExport.Columns.Add("Unit *");
                dtExport.Columns.Add("Unit Price *");
                dtExport.Columns.Add("Gross Amount *");
                dtExport.Columns.Add("Discount");
                dtExport.Columns.Add("Pre Tax Value");
                dtExport.Columns.Add("Taxable value *");
                dtExport.Columns.Add("GST Rate (%) *");
                dtExport.Columns.Add("Sgst Amt(Rs)");
                dtExport.Columns.Add("Cgst Amt (Rs)");
                dtExport.Columns.Add("Igst Amt (Rs)");
                dtExport.Columns.Add("Cess Rate (%)");
                dtExport.Columns.Add("Cess Amt Adval (Rs)");
                dtExport.Columns.Add("Cess Non Adval Amt (Rs)");
                dtExport.Columns.Add("State Cess Rate (%)");
                dtExport.Columns.Add("State Cess Adval Amt (Rs)");
                dtExport.Columns.Add("State Cess Non-Adval Amt (Rs)");
                dtExport.Columns.Add("Other Charges  ");
                dtExport.Columns.Add("Item Total *");

                dtExport.Columns.Add("Batch Name");
                dtExport.Columns.Add("Batch Expiry Dt");
                dtExport.Columns.Add("Warranty Dt");


                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    dtExport.Rows.Add(
    dtResult.Rows[i]["DOC_TYPE"].ToString(),
    dtResult.Rows[i]["INM_TNO"].ToString(),
   dtResult.Rows[i]["INM_DATE"].ToString().Replace('-', '/'),
    dtResult.Rows[i]["SR_NO"].ToString(),
    dtResult.Rows[i]["I_CODENO"].ToString(),
    dtResult.Rows[i]["ISSERVICE"].ToString(),
    dtResult.Rows[i]["IND_E_TARIFF_NO"].ToString(),
    dtResult.Rows[i]["BARCODE"].ToString(),
    dtResult.Rows[i]["IND_INQTY"].ToString(),
    dtResult.Rows[i]["FREE_QTY"].ToString(),
    dtResult.Rows[i]["I_UOM_DESC"].ToString(),
    dtResult.Rows[i]["IND_RATE"].ToString(),
    dtResult.Rows[i]["IND_AMT"].ToString(),
    dtResult.Rows[i]["INM_DISC_AMT"].ToString(),
    dtResult.Rows[i]["BLANK"].ToString(),
                        //dtResult.Rows[i]["INM_ACCESSIBLE_AMT"].ToString(),
    dtResult.Rows[i]["IND_AMT"].ToString(),
    dtResult.Rows[i]["GST_PER"].ToString(),
    dtResult.Rows[i]["IND_EX_AMT"].ToString(),
    dtResult.Rows[i]["IND_E_CESS_AMT"].ToString(),
    dtResult.Rows[i]["IND_SH_CESS_AMT"].ToString(),
    dtResult.Rows[i]["CESS_RATE"].ToString(),
    dtResult.Rows[i]["CESS_AMT"].ToString(),
    dtResult.Rows[i]["CESS_NON_ADV"].ToString(),
    dtResult.Rows[i]["STATE_CESS_RATE"].ToString(),
    dtResult.Rows[i]["STAE_CESS_AMT"].ToString(),
    dtResult.Rows[i]["STATE_CESS_NON_ADV"].ToString(),
    dtResult.Rows[i]["OTHER_CHARGES"].ToString(),
    dtResult.Rows[i]["ITEM_TOTAL"].ToString(),

                        dtResult.Rows[i]["BLANK"].ToString(),
                        dtResult.Rows[i]["BLANK"].ToString(),
         dtResult.Rows[i]["BLANK"].ToString()
                                     );
                }
            }


            //DataTable distinctTable = dtExport.DefaultView.ToTable( /*distinct*/ true);
            dtExport = dtExport.DefaultView.ToTable( /*distinct*/ true);


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=E_InvoiceItem.xls");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
            "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
            "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //am getting my grid's column headers
            int columnscount = dtExport.Columns.Count;
            for (int j = 0; j < columnscount; j++)
            {      //write in new column
                HttpContext.Current.Response.Write("<Td>");
                //Get column headers  and make it as bold in excel columns
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            for (int k = 0; k < dtExport.Rows.Count; k++)
            {//write in new row
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    if (i == dtExport.Columns.Count - 1)
                    {
                        HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    else
                    {
                        if (i == 33 || i == 2)
                        {
                            if (dtExport.Rows[k]["Document Date (DD/MM/YYYY) *"].ToString().Length > 0)
                            {
                                HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            }
                            else if (dtExport.Rows[k]["Product Description"].ToString().Length > 0)
                            {
                                HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("<Td>");
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("<Td>");
                        }
                        HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                        HttpContext.Current.Response.Write("</Td>");
                    }
                }
                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
            #endregion Export_Excel

        }

        catch (Exception Ex)
        {
            //  CommonClasses.SendError("Tally Transfer Sales", "btnLoad_Click", Ex.Message);
        }
    }
    #endregion

}