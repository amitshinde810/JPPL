using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;

public partial class Transactions_ADD_TaxInvoice : System.Web.UI.Page
{
    #region Variable

    DataTable dt = new DataTable();
    DataTable dtInvoiceDetail = new DataTable();
    static int mlCode = 0;
    static int RowCount = 0;
    DirectoryInfo ObjSearchDir;
    string fileName = "";
    string fileName1 = "";
    static double TrayQty = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();
    public static string SaveResult = "0";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
        home1.Attributes["class"] = "active";

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
                        ViewState["fileName"] = fileName;
                        ViewState["fileName1"] = fileName1;
                        ViewState["mlCode"] = "";
                        ViewState["RowCount"] = "";
                        ViewState["Index"] = "";
                        ViewState["SaveResult"] = "0";
                        ViewState["mlCode"] = mlCode;
                        ViewState["RowCount"] = RowCount;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        dt2.Clear();
                        ViewState["str"] = str;
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Clear();
                        LoadCustomer();
                        // LoadPOMulti();
                        LoadICode();
                        LoadIName();
                        Loadtax();
                        LoadTray();
                        LoadITariff();
                        TrayQty = 0;
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
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
                            txtIssuetime.Text = DateTime.Now.ToString("HH:mm ");
                            DateTime dtremovalDate = new DateTime();
                            dtremovalDate = Convert.ToDateTime(txtIssuetime.Text).AddMinutes(10);
                            txtRemoveltime.Text = dtremovalDate.ToString("HH:mm");
                            txtDate.Attributes.Add("readonly", "readonly");
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
                #region DetailFiles
                if (IsPostBack && imgUpload.PostedFile != null)
                {
                    if (imgUpload.PostedFile.FileName.Length > 0)
                    {
                        fileName = imgUpload.PostedFile.FileName;
                        ViewState["fileName"] = fileName;
                        Upload(null, null);
                    }
                }
                if (IsPostBack && FilePDI.PostedFile != null)
                {
                    if (FilePDI.PostedFile.FileName.Length > 0)
                    {
                        fileName1 = FilePDI.PostedFile.FileName;
                        ViewState["fileName1"] = fileName1;
                        UploadPDI(null, null);
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOADTaxM/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/TaxMTCFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        //if (imgUpload.PostedFile.ContentLength > 0)
        //{
        if (Request.QueryString[0].Equals("INSERT"))
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOADTaxM/" + ViewState["fileName"].ToString()));
        }
        else
        {
            imgUpload.SaveAs(Server.MapPath("~/UpLoadPath/TaxMTCFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
        lnkView.Visible = true;
        lnkView.Text = ViewState["fileName"].ToString();
        //}
    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkView.Text != "")
                {
                    filePath = lnkView.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/FILEUPLOADTaxM/" + filePath;
            }
            else
            {
                if (lnkView.Text != "")
                {
                    filePath = lnkView.Text;
                }

                directory = "../../UpLoadPath/TaxMTCFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "lnkView_Click", ex.Message);
        }
    }
    #endregion

    #region UploadPDI
    protected void UploadPDI(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/FILEUPLOADTaxMPDI/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/TaxMPDIFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        //if (imgUpload.PostedFile.ContentLength > 0)
        //{
        if (Request.QueryString[0].Equals("INSERT"))
        {
            FilePDI.SaveAs(Server.MapPath("~/UpLoadPath/FILEUPLOADTaxMPDI/" + ViewState["fileName1"].ToString()));
        }
        else
        {
            FilePDI.SaveAs(Server.MapPath("~/UpLoadPath/TaxMPDIFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName1"].ToString()));
        }
        lnkViewPDI.Visible = true;
        lnkViewPDI.Text = ViewState["fileName1"].ToString();
        //}
    }

    protected void lnkPDIView_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkViewPDI.Text != "")
                {
                    filePath = lnkViewPDI.Text;
                }

                directory = "../../UpLoadPath/FILEUPLOADTaxMPDI/" + filePath;
            }
            else
            {
                if (lnkViewPDI.Text != "")
                {
                    filePath = lnkViewPDI.Text;
                }

                directory = "../../UpLoadPath/TaxMPDIFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier PO", "lnkQuotation_Click", ex.Message);
        }
    }
    #endregion


    #region LoadITariff
    private void LoadITariff()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT E_CODE,E_TARIFF_NO FROM EXCISE_TARIFF_MASTER WHERE ES_DELETE=0 AND E_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND   E_TALLY_GST_EXCISE=1 ORDER BY E_TARIFF_NO");
            ddlTariff.DataSource = dt;
            ddlTariff.DataTextField = "E_TARIFF_NO";
            ddlTariff.DataValueField = "E_CODE";
            ddlTariff.DataBind();
            ddlTariff.Items.Insert(0, new ListItem("Select Item HSN", "0"));
            ddlTariff.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("LoadIUnit", "LoadIUnit", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgInvoiceAddDetail.Enabled == false)
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
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
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
            if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDate.Text.Trim() == "")
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

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER,CUSTPO_MASTER where CPOM_P_CODE=P_CODE and CUSTPO_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1   ORDER BY P_NAME ASC-- and CPOM_TYPE=-2147483648  ORDER BY P_NAME ASC");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
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
            string str = "";
            if (ddlPoFilter.SelectedIndex != 0)
            {
                str = str + "CPOM_CODE='" + ddlPoFilter.SelectedValue + "' AND ";
            }
            dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where " + str + " CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0  order by I_CODENO ASC");
            if (dt.Rows.Count > 0)
            {
                ddlItemCode.DataSource = dt;
                ddlItemCode.DataTextField = "I_CODENO";
                ddlItemCode.DataValueField = "I_CODE";
                ddlItemCode.DataBind();
                ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
                ddlItemCode.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
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
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE AND CPOM_CODE='" + ddlPoFilter.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE)   ");
            }
            else
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE AND CPOM_CODE='" + ddlPoFilter.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0  ");
            }
            ddlPONo.DataSource = dtPO;
            ddlPONo.DataTextField = "CPOM_PONO";
            ddlPONo.DataValueField = "CPOM_CODE";
            ddlPONo.DataBind();
            ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadPO", Ex.Message);
        }
    }
    #endregion

    #region LoadPOMulti
    private void LoadPOMulti()
    {
        try
        {
            DataTable dtPO = new DataTable();
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and  CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0  ");
            }
            else
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0  AND CPOM_AUTH_FLG=1");
            }
            if (dtPO.Rows.Count > 0)
            {
                ddlPoFilter.DataSource = dtPO;
                ddlPoFilter.DataTextField = "CPOM_PONO";
                ddlPoFilter.DataValueField = "CPOM_CODE";
                ddlPoFilter.DataBind();
                ddlPoFilter.Items.Insert(0, new ListItem("Select PO", "0"));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadPOMulti", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            string str = "";
            if (ddlPoFilter.SelectedIndex != 0)
            {
                str = str + "CPOM_CODE='" + ddlPoFilter.SelectedValue + "' AND ";
            }
            dt = CommonClasses.Execute("select distinct I_CODE,I_NAME from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where " + str + " CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 order by I_NAME ASC");
            if (dt.Rows.Count > 0)
            {
                ddlItemName.DataSource = dt;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
                ddlItemName.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadIName", Ex.Message);
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
                dtFilter.Columns.Add(new System.Data.DataColumn("AmortRate", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AmortAmount", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SUBHEADING", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_NO_PACK", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SR_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_DIE_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_DIE_RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_DIE_AMOUNT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_TARIFF_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_TRASPORT_RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_TRASPORT_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_TEX_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_TE_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_TSH_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_TC_FILE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PDI_FILE", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvoiceAddDetail.DataSource = dtFilter;
                dgInvoiceAddDetail.DataBind();
                dgInvoiceAddDetail.Enabled = false;
            }
        }
    }
    #endregion

    #region LoadFilter1
    public void LoadFilter1()
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
            dtFilter.Columns.Add(new System.Data.DataColumn("AmortRate", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AmortAmount", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SUBHEADING", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_NO_PACK", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SR_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_DIE_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_DIE_RATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_DIE_AMOUNT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_TARIFF_NO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_TRASPORT_RATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_TRASPORT_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_TEX_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_TE_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_TSH_CESS_AMT", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("IND_TC_FILE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PDI_FILE", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInvoiceAddDetail.DataSource = dtFilter;
            dgInvoiceAddDetail.DataBind();
            dgInvoiceAddDetail.Enabled = false;
        }
    }
    #endregion

    #region LoadTray
    private void LoadTray()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where es_delete ='0' and I_CAT_CODE ='-2147483633' order by I_CODENO ASC");
            ddlTray.DataSource = dt;
            ddlTray.DataTextField = "I_CODENO";
            ddlTray.DataValueField = "I_CODE";
            ddlTray.DataBind();
            ddlTray.Items.Insert(0, new ListItem("Select Tray", "0"));
            ddlTray.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadTray", Ex.Message);
        }
    }
    #endregion

    #region LoadSalesSchedule
    private void LoadSalesSchedule()
    {
        try
        {
            dt = CommonClasses.Execute(" SELECT  DSED_ACTUAL_QTY ,ISNULL((SELECT SUM(IND_INQTY) FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND IND_I_CODE=DSED_I_CODE   AND INM_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' AND  INM_TYPE='TAXINV' AND INM_SUPPLEMENTORY=0 ),0) AS SALES_QTY  FROM DAILY_SALE_ENTRY_DETAIL,DAILY_SALE_ENTRY_MASTER where DSEM_CODE=DSED_DSEM_CODE AND DAILY_SALE_ENTRY_MASTER.ES_DELETE=0 AND DSEM_MONTH='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' AND DSED_I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dt.Rows.Count > 0)
            {
                txtDailySchedule.Text = (Convert.ToDouble(dt.Rows[0]["DSED_ACTUAL_QTY"].ToString()) - Convert.ToDouble(dt.Rows[0]["SALES_QTY"].ToString())).ToString();
            }
            else
            {
                txtDailySchedule.Text = "0.00";
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadTray", Ex.Message);
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
                //Suja-RFD Store Sum
                DataTable dtStkQty = new DataTable();
                if (rbtWithAmt.SelectedValue == "-2147483648")
                {
                    dtStkQty = CommonClasses.Execute("select isnull(sum(STL_DOC_QTY),0) as STL_DOC_QTY from STOCK_LEDGER where STL_STORE_TYPE=-2147483642 and STL_I_CODE='" + ddlItemCode.SelectedValue + "'");
                }
                else
                {
                    dtStkQty = CommonClasses.Execute("select isnull(sum(STL_DOC_QTY),0) as STL_DOC_QTY from STOCK_LEDGER where STL_STORE_TYPE=-2147483627 and STL_I_CODE='" + ddlItemCode.SelectedValue + "'");
                }

                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,ISNULL(I_DEVELOMENT,0) AS I_DEVELOMENT,I_CURRENT_BAL,I_E_CODE from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                DataTable dtPO = new DataTable();
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0   and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "  AND CPOD_STATUS=0   and CPOM_IS_VERBAL=0");
                }
                else
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0 ) AND CPOD_STATUS=0   and CPOM_IS_VERBAL=0 AND CPOM_AUTH_FLG=1");
                }

                ddlPONo.DataSource = dtPO;
                ddlPONo.DataTextField = "CPOM_PONO";
                ddlPONo.DataValueField = "CPOM_CODE";
                ddlPONo.DataBind();

                ddlPONo_SelectedIndexChanged(null, null);
                //if (dt1.Rows[0]["I_DEVELOMENT"].ToString() == "1" || dt1.Rows[0]["I_DEVELOMENT"].ToString().ToUpper() == "TRUE")
                //{
                //    txtDailySchedule.Text = "NA";
                //}
                //else
                //{
                LoadSalesSchedule();
                //}
                if (dtStkQty.Rows.Count > 0)
                {
                    txtStockQty.Text = dtStkQty.Rows[0]["STL_DOC_QTY"].ToString();
                }
                else
                {
                    txtStockQty.Text = "0";
                }
                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    ddlTariff.SelectedValue = dt1.Rows[0]["I_E_CODE"].ToString();
                    txtVQty.Text = "";
                }
                else
                {
                    txtUOM.Text = "";
                    txtVQty.Text = "";
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlPONo_SelectedIndexChanged
    protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                DataTable dt1 = CommonClasses.Execute("select CPOD_RATE ,ISNULL(CPOD_TRANSPORT_RATE,0) AS CPOD_TRANSPORT_RATE,ISNULL(CPOD_MODNO,0) AS ModNo,ISNULL(CPOD_MODDATE,GetDate()) AS ModDate,ISNULL(CPOD_AMORTRATE,0)  AS CPOD_AMORTRATE,  ISNULL(CPOD_DIEAMORTRATE,0)  AS CPOD_DIEAMORTRATE,( ISNULL(CPOD_DIEAMORTQTY,0) -  ISNULL(CPOD_DIEAMORTQTYRETURN,0))  AS CPOD_DIEAMORTQTYRETURN  from CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
                DataTable dtQty = CommonClasses.Execute("SELECT CPOD_ORD_QTY, (CPOD_ORD_QTY-CPOD_DISPACH) as Qty FROM CUSTPO_DETAIL where CPOD_I_CODE=" + ddlItemCode.SelectedValue + " and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + "  ");
                if (dt1.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_RATE"]);
                    txtAmortrate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_AMORTRATE"]);
                    txtDieRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_DIEAMORTRATE"]);
                    txtTransRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(dt1.Rows[0]["CPOD_TRANSPORT_RATE"]), 2));
                }
                else
                {
                    txtRate.Text = "0.00"; txtTransRate.Text = "0.00";
                }
                if (dtQty.Rows.Count > 0)
                {
                    txtPendingQty.Text = string.Format("{0:0.000}", dtQty.Rows[0]["Qty"]);
                    txtpQTY.Text = string.Format("{0:0.000}", dtQty.Rows[0]["CPOD_ORD_QTY"]);
                    txtVQty.Text = "";
                }
                else
                {
                    txtPendingQty.Text = "0.000";
                    txtpQTY.Text = "0";
                    txtVQty.Text = "";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlPONo_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlPoFilter_SelectedIndexChanged
    protected void ddlPoFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadFilter1();
            LoadICode();
            LoadIName();
            LoadPO();
            ddlItemName_SelectedIndexChanged(null, null);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ((DataTable)ViewState["dt2"]).Clear();
            LoadFilter1();
            LoadPOMulti();
            // After clear grid, item not inserted for new customer so assign value to rowcount
            ViewState["RowCount"] = 0;
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
                //Suja-RFD Store Sum
                DataTable dtStkQty = new DataTable();
                if (rbtWithAmt.SelectedValue == "-2147483648")
                {
                    dtStkQty = CommonClasses.Execute("select isnull(sum(STL_DOC_QTY),0) as STL_DOC_QTY from STOCK_LEDGER where STL_STORE_TYPE=-2147483642 and STL_I_CODE='" + ddlItemCode.SelectedValue + "'");
                }
                else
                {
                    dtStkQty = CommonClasses.Execute("select isnull(sum(STL_DOC_QTY),0) as STL_DOC_QTY from STOCK_LEDGER where STL_STORE_TYPE=-2147483627 and STL_I_CODE='" + ddlItemCode.SelectedValue + "'");
                }
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,ISNULL(I_DEVELOMENT,0) AS I_DEVELOMENT,I_CURRENT_BAL,I_E_CODE from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtPO = CommonClasses.Execute("select distinct(CPOM_CODE),CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and ES_DELETE=0 and CPOM_TYPE=-2147483648");
                DataTable dtPO = new DataTable();
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CPOD_STATUS=0    AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE) and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " --and CPOM_TYPE=-2147483648");
                }
                else
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 AND CPOD_STATUS=0   and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0 AND CPOM_AUTH_FLG=1 ) --and CPOM_TYPE=-2147483648");
                }
                ddlPONo.DataSource = dtPO;
                ddlPONo.DataTextField = "CPOM_PONO";
                ddlPONo.DataValueField = "CPOM_CODE";
                ddlPONo.DataBind();
                ddlPONo_SelectedIndexChanged(null, null);
                //if (dt1.Rows[0]["I_DEVELOMENT"].ToString() == "1" || dt1.Rows[0]["I_DEVELOMENT"].ToString().ToUpper() == "TRUE")
                //{
                //    txtDailySchedule.Text = "NA";
                //}
                //else
                //{
                LoadSalesSchedule();
                //}
                if (dtStkQty.Rows.Count > 0)
                {
                    txtStockQty.Text = dtStkQty.Rows[0]["STL_DOC_QTY"].ToString();
                }
                else
                {
                    txtStockQty.Text = "0";
                }
                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    ddlTariff.SelectedValue = dt1.Rows[0]["I_E_CODE"].ToString();
                    txtVQty.Text = "0.00";
                }
                else
                {
                    txtUOM.Text = "";
                    txtVQty.Text = "0.00";
                }

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

    #region ddlTray_SelectedIndexChanged
    protected void ddlTray_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTrayStock = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlTray.SelectedValue + "'");
            if (dtTrayStock.Rows.Count > 0)
            {
                txtTrayStock.Text = dtTrayStock.Rows[0]["I_CURRENT_BAL"].ToString();
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
        try
        {
            txtDate.Attributes.Add("readonly", "readonly");
            txtIssuedate.Attributes.Add("readonly", "readonly");
            txtremovaldate.Attributes.Add("readonly", "readonly");
            txtLRDate.Attributes.Add("readonly", "readonly");
            dtInvoiceDetail.Clear();
            DataTable dtMast = CommonClasses.Execute("SELECT ISNULL(INM_PLANT,-2147483648) AS INM_PLANT, INM_G_AMT,INM_CODE,INM_NO,INM_DATE,INM_P_CODE,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,INM_CM_CODE,MODIFY,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_TAX_TCS,0) as numeric(20,2)) as INM_TAX_TCS,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0)  as numeric(20,2)) as INM_OCTRI_AMT,isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME,INM_TRAY_CODE,INM_TRAY_QTY , INM_ELECTRREFNUM,INM_TERMSNCONDITIONS,INM_AUTHORIZEDNAME FROM INVOICE_MASTER WHERE ES_DELETE=0 AND INM_CM_CODE= '" + (string)Session["CompanyCode"] + "' AND INM_CODE= '" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dtMast.Rows[0]["INM_CODE"]); ;

                txtInvoiceNo.Text = dtMast.Rows[0]["INM_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = dtMast.Rows[0]["INM_P_CODE"].ToString();
                ddlCustomer_SelectedIndexChanged(null, null);
                txtNetAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_NET_AMT"].ToString()));
                txtstroreloc.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_STO_LOC"].ToString()));
                txtDiscPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC"].ToString()));
                txtDiscAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC_AMT"].ToString()));
                txtPackAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_PACK_AMT"].ToString()));
                txtAccessableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_ACCESSIBLE_AMT"].ToString()));
                txtBasicExcPer.Text = dtMast.Rows[0]["INM_BEXCISE"].ToString();
                txtBasicExcAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_BE_AMT"].ToString()));
                txtducexcper.Text = dtMast.Rows[0]["INM_EDUC_CESS"].ToString();
                txtEdueceAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_EDUC_AMT"].ToString()));
                txtSHEExcPer.Text = dtMast.Rows[0]["INM_H_EDUC_CESS"].ToString();
                txtSHEExcAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_H_EDUC_AMT"].ToString()));
                txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TAXABLE_AMT"].ToString()));
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
                txtVechicleNo.Text = dtMast.Rows[0]["INM_VEH_NO"].ToString();
                txtTransport.Text = dtMast.Rows[0]["INM_TRANSPORT"].ToString();
                txtIssuedate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_ISSUE_DATE"]).ToString("dd MMM yyyy");
                txtremovaldate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_REMOVAL_DATE"]).ToString("dd MMM yyyy");
                chkIsSuppliement.Checked = Convert.ToBoolean(dtMast.Rows[0]["INM_IS_SUPPLIMENT"]);
                if (chkIsSuppliement.Checked)
                {
                    txtBasicExcAmt.Enabled = true;
                }
                txtRemark.Text = dtMast.Rows[0]["INM_REMARK"].ToString();
                txtLRNo.Text = dtMast.Rows[0]["INM_LR_NO"].ToString();
                if (dtMast.Rows[0]["INM_LR_DATE"] != DBNull.Value)
                {
                    txtLRDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_LR_DATE"]).ToString("dd MMM yyyy");
                }
                txtIssuetime.Text = dtMast.Rows[0]["INM_ISSU_TIME"].ToString();
                txtRemoveltime.Text = dtMast.Rows[0]["INM_REMOVEL_TIME"].ToString();
                ddlTray.SelectedValue = dtMast.Rows[0]["INM_TRAY_CODE"].ToString();
                txtTrayQty.Text = dtMast.Rows[0]["INM_TRAY_QTY"].ToString();
                TrayQty = Convert.ToDouble(dtMast.Rows[0]["INM_TRAY_QTY"].ToString());
                txtElectrRefNum.Text = dtMast.Rows[0]["INM_ELECTRREFNUM"].ToString();
                txtTermsNConditions.Text = dtMast.Rows[0]["INM_TERMSNCONDITIONS"].ToString();
                txtAuthorizedName.Text = dtMast.Rows[0]["INM_AUTHORIZEDNAME"].ToString();
                txtTrayStock.Text = "0";
                btnSubmit.Visible = true;
                btnInsert.Visible = true;
                rbtWithAmt.SelectedValue = dtMast.Rows[0]["INM_PLANT"].ToString();
                rbtWithAmt.Enabled = false;
                dtInvoiceDetail = CommonClasses.Execute("select DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,I_CURRENT_BAL as STOCK_QTY,cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(20,3)) as PEND_QTY,cast(IND_INQTY as  numeric(20,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(20,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(IND_AMT as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT as IND_EX_AMT,IND_E_CESS_AMT as IND_E_CESS_AMT,IND_SH_CESS_AMT as IND_SH_CESS_AMT,IND_PACK_DESC ,ISNULL(IND_AMORTRATE,0) AS AmortRate,ISNULL(IND_AMORTAMT,0) AS AmortAmount,ISNULL(IND_TRASPORT_RATE,0) as IND_TRASPORT_RATE,ISNULL(IND_TRASPORT_AMT,0) as IND_TRASPORT_AMT,IND_SR_NO, ISNULL(IND_DIE_QTY,0) AS IND_DIE_QTY,ISNULL(IND_DIE_RATE,0) AS IND_DIE_RATE,ISNULL(IND_DIE_AMOUNT,0) AS IND_DIE_AMOUNT,IND_E_CODE,IND_E_TARIFF_NO  ,ISNULL(IND_TEX_AMT,0) AS IND_TEX_AMT,ISNULL(IND_TE_CESS_AMT,0) AS IND_TE_CESS_AMT,ISNULL(IND_TSH_CESS_AMT,0) AS IND_TSH_CESS_AMT,IND_TC_FILE,IND_PDI_FILE FROM INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER  where INM_CODE=IND_INM_CODE and IND_CPOM_CODE=CPOM_CODE AND  CPOM_CODE=CPOD_CPOM_CODE AND   CPOD_I_CODE=I_CODE AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");

                LoadICode();
                LoadIName();
                LoadPO();
                if (dtInvoiceDetail.Rows.Count != 0)
                {
                    dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
                    dgInvoiceAddDetail.DataBind();
                    ViewState["dt2"] = dtInvoiceDetail;
                    dgInvoiceAddDetail.Enabled = true;
                    GetTotal();
                    ViewState["RowCount"] = dtInvoiceDetail.Rows.Count;
                }
            }

            if (str == "VIEW")
            {
                txtInvoiceNo.Enabled = false;
                txtDate.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtAmount.Enabled = false;
                ddlPONo.Enabled = false;
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
                txtsrNo.Enabled = false;
                txtDiePending.Enabled = false;
                txtDieAmount.Enabled = false;
                txtDieRate.Enabled = false;
                txtBatchNo.Enabled = false;
                txtNoPackaeg.Enabled = false;
                btnUpload.Enabled = false;
                FilePDI.Enabled = false;
                imgUpload.Enabled = false;
                dgInvoiceAddDetail.Enabled = false;
                ddlPONo.Enabled = false;
            }
            if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                chkIsSuppliement.Enabled = false;
                CommonClasses.SetModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
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

    #region txtTransRate_TextChanged
    protected void txtTransRate_TextChanged(object sender, EventArgs e)
    {
        if (txtTransRate.Text.Trim() == "")
        {
            txtTransRate.Text = "0.000";
        }
        else
        {
            string totalStr = DecimalMasking(txtTransRate.Text);
            txtTransRate.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            txtTransAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtTransRate.Text.ToString())), 2);
        }
    }
    #endregion

    #region txtVQty_OnTextChanged
    protected void txtVQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPONo.SelectedValue == "" || ddlPONo.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select PO";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlPONo.Focus();
                txtVQty.Text = "0";
                return;
            }
            if (txtVQty.Text.Trim() == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtRate.Text.Trim() == "")
            {
                txtRate.Text = "0.00";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            }
            if (txtAmortrate.Text.Trim() == "")
            {
                txtAmortrate.Text = "0.00";
            }
            else
            {
                string totalStr = DecimalMasking(txtAmortrate.Text);
                txtAmortrate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            }
            if (txtDiePending.Text.Trim() == "")
            {
                txtDiePending.Text = "0";
            }
            if (chkIsSuppliement.Checked == false)
            {
                if (txtPendingQty.Text.Trim() != "")
                {
                    if (Convert.ToDouble(txtPendingQty.Text) == 0 && Convert.ToDouble(txtpQTY.Text) != 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "pending qty not available for this item";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Text = "0.00";
                        txtVQty.Focus();
                        return;
                    }
                    if (Convert.ToDouble(txtPendingQty.Text) > 0)
                    {
                        if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Please Enter Correct Invoice Qty";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtVQty.Text = "0.00";
                            txtVQty.Focus();
                            return;
                        }
                    }
                    if (Convert.ToDouble(txtStockQty.Text) < Convert.ToDouble(txtVQty.Text))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Enter Invoice Qty Less than stock";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Text = "0.00";
                        txtVQty.Focus();
                        return;
                    }
                    //if (txtDailySchedule.Text != "NA")
                    //{
                    //    if (Convert.ToDouble(txtDailySchedule.Text) < Convert.ToDouble(txtVQty.Text))
                    //    {
                    //        PanelMsg.Visible = true;
                    //        lblmsg.Text = "Please Enter Invoice Qty Less than Schedule Qty";
                    //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //        txtVQty.Text = "0.00";
                    //        txtVQty.Focus();
                    //        return;
                    //    }
                    //}
                }
            }
            DataTable dt1 = CommonClasses.Execute("select CPOD_RATE,ISNULL(CPOD_MODNO,0) AS ModNo,ISNULL(CPOD_MODDATE,GetDate()) AS ModDate,ISNULL(CPOD_AMORTRATE,0)  AS CPOD_AMORTRATE,  ISNULL(CPOD_DIEAMORTRATE,0)  AS CPOD_DIEAMORTRATE,( ISNULL(CPOD_DIEAMORTQTY,0) -  ISNULL(CPOD_DIEAMORTQTYRETURN,0))  AS CPOD_DIEAMORTQTYRETURN  from CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                txtDiePending.Text = (Convert.ToDouble(txtDiePending.Text) + Convert.ToDouble(dt1.Rows[0]["CPOD_DIEAMORTQTYRETURN"].ToString())).ToString();
            }
            else
            {
                txtDiePending.Text = dt1.Rows[0]["CPOD_DIEAMORTQTYRETURN"].ToString();
            }
            if (Convert.ToDouble(txtDiePending.Text) >= Convert.ToDouble(txtVQty.Text))
            {
                txtDieAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtDieRate.Text.ToString())), 2);
                txtDiePending.Text = txtVQty.Text;
            }
            else
            {
                txtDieAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtDiePending.Text.ToString()) * Convert.ToDouble(txtDieRate.Text.ToString())), 2);
                txtDiePending.Text = txtDiePending.Text;
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString()), 2));
            txtAmortAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtAmortrate.Text.ToString())), 2);
            if (txtNoPackaeg.Text.Trim() == "")
            {
                txtNoPackaeg.Text = "0";
            }
            if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
            {
                double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
                txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
            }

            if (txtTransRate.Text.Trim() == "")
            {
                txtTransRate.Text = "0";
            }
            if (Convert.ToDouble(txtTransRate.Text) > 0)
            {
                double TransAmt = Math.Round(Convert.ToDouble(txtVQty.Text) * Convert.ToDouble(txtTransRate.Text), 3);
                txtTransAmt.Text = string.Format("{0:0.000}", (TransAmt));
            }
            else
            {
                txtTransAmt.Text = "0.00";
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
            if (txtVQty.Text.Trim() == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtRate.Text.Trim() == "")
            {
                txtRate.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
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
        if (txtNoPackaeg.Text.Trim() == "")
        {
            txtNoPackaeg.Text = "0";
        }
        if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
        {
            if (txtVQty.Text.Trim() != "")
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
            //if (Convert.ToInt32(ViewState["RowCount"].ToString()) != 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Only One Item added in invoice";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlCustomer.Focus();
            //    return;
            //}
            if (ddlCustomer.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
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
            if (chkIsSuppliement.Checked == false)
            {
                if (txtVQty.Text.Trim() == "" || Convert.ToDouble(txtVQty.Text) == 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter Invoice Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtVQty.Focus();
                    return;
                }
                if (txtStockQty.Text.Trim() == "" || txtStockQty.Text == "0.000")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please check Stock  Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStockQty.Focus();
                    return;
                }
            }
            if (!chkIsSuppliement.Checked)
            {
            }
            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_I_CODE"))).Text;
                    string PO_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblPO_CODE"))).Text;

                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
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
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("UOM");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("STOCK_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PEND_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("INV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("ACT_WGHT");
                ((DataTable)ViewState["dt2"]).Columns.Add("RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("AmortRate");
                ((DataTable)ViewState["dt2"]).Columns.Add("AmortAmount");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_SUBHEADING");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_BACHNO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_NO_PACK");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PAK_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_EX_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_CESS_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_SH_CESS_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PACK_DESC");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_SR_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_DIE_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_DIE_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_DIE_AMOUNT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_TARIFF_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_TRASPORT_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_TRASPORT_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_TEX_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_TE_CESS_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_TSH_CESS_AMT");

                ((DataTable)ViewState["dt2"]).Columns.Add("IND_TC_FILE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PDI_FILE");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IND_I_CODE"] = ddlItemName.SelectedValue;
            dr["IND_I_CODENO"] = ddlItemCode.SelectedItem;
            dr["IND_I_NAME"] = ddlItemName.SelectedItem;
            dr["UOM"] = txtUOM.Text;
            dr["PO_CODE"] = ddlPONo.SelectedValue;
            dr["PO_NO"] = ddlPONo.SelectedItem;
            dr["STOCK_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtStockQty.Text.Trim() == "" ? "0.00" : txtStockQty.Text) - Convert.ToDouble(txtVQty.Text)));
            dr["PEND_QTY"] = string.Format("{0:0.000}", ((Convert.ToDouble(txtPendingQty.Text.Trim() == "" ? "0.00" : txtPendingQty.Text)) - Convert.ToDouble(txtVQty.Text)));
            dr["INV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtVQty.Text.Trim() == "" ? "0.00" : txtVQty.Text)));
            dr["ACT_WGHT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtActWght.Text.Trim() == "" ? "0.00" : txtActWght.Text)));
            dr["RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text.Trim() == "" ? "0.00" : txtRate.Text)));
            dr["AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtAmount.Text)));
            dr["AmortRate"] = string.Format("{0:0.00}", (Convert.ToDouble(txtAmortrate.Text.Trim() == "" ? "0.00" : txtAmortrate.Text)));
            if (chkIsSuppliement.Checked == true)
            {
                dr["AmortAmount"] = string.Format("{0:0.00}", 0.00);
            }
            else
            {
                dr["AmortAmount"] = string.Format("{0:0.00}", (Convert.ToDouble(txtAmortAmount.Text.Trim() == "" ? "0.00" : txtAmortAmount.Text)));
            }
            dr["IND_TRASPORT_RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtTransRate.Text.Trim() == "" ? "0.00" : txtTransRate.Text)));
            dr["IND_TRASPORT_AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtTransAmt.Text.Trim() == "" ? "0.00" : txtTransAmt.Text)));

            dr["IND_SUBHEADING"] = txtSubHeading.Text.Trim().Replace("'", "\'");
            dr["IND_BACHNO"] = txtBatchNo.Text.Trim().Replace("'", "\'");
            dr["IND_NO_PACK"] = txtNoPackaeg.Text.Trim().Replace("'", "\'");
            dr["IND_PAK_QTY"] = txtQtyPerPack.Text.Trim().Replace("'", "\'");
            dr["IND_PACK_DESC"] = txtPackDesc.Text.Trim().Replace("'", "\'");
            dr["IND_SR_NO"] = txtsrNo.Text;
            dr["IND_DIE_QTY"] = txtDiePending.Text;
            dr["IND_DIE_RATE"] = txtDieRate.Text;
            dr["IND_DIE_AMOUNT"] = txtDieAmount.Text;

            dr["IND_TC_FILE"] = ViewState["fileName"].ToString();
            dr["IND_PDI_FILE"] = ViewState["fileName1"].ToString();

            #region ExciseCalculation
            double EBasicPer = 0;
            double EEduCessPer = 0;
            double EHEduCessPer = 0;
            double EBasic = 0;
            double EEduCess = 0;
            double EHEduCess = 0;
            double TEBasic = 0;
            double TEEduCess = 0;
            double TEHEduCess = 0;
            DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + ddlItemCode.SelectedValue + "");
            if (dtExcisePer.Rows.Count > 0)
            {
                EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
                EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
                EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
            }

            DataTable dtCompState = CommonClasses.Execute("SELECT CM_STATE FROM COMPANY_MASTER WHERE CM_ID=" + (string)Session["CompanyId"] + "");
            DataTable dtPartyStae = CommonClasses.Execute("SELECT P_SM_CODE FROM PARTY_MASTER WHERE P_CODE='" + ddlCustomer.SelectedValue + "'");

            if (dtCompState.Rows[0][0].ToString() == dtPartyStae.Rows[0][0].ToString())
            {
                EBasic = Math.Round((((Convert.ToDouble(txtAmount.Text)) * EBasicPer) / 100), 2);
                EEduCess = Math.Round((Convert.ToDouble(txtAmount.Text)) * EEduCessPer / 100, 2);

                TEBasic = Math.Round((((Convert.ToDouble(txtTransAmt.Text)) * EBasicPer) / 100), 2);
                TEEduCess = Math.Round((Convert.ToDouble(txtTransAmt.Text)) * EEduCessPer / 100, 2);
                EHEduCess = 0.00;
                txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EBasicPer));
                txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(EEduCessPer));
                txtSHEExcPer.Text = "0.00";
            }
            else
            {
                EBasic = 0.00;
                EEduCess = 0.00;
                EHEduCess = Math.Round((Convert.ToDouble(txtAmount.Text)) * EHEduCessPer / 100, 2);
                TEHEduCess = Math.Round((Convert.ToDouble(txtTransAmt.Text)) * EHEduCessPer / 100, 2);
                txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EHEduCessPer));
                txtBasicExcPer.Text = "0.00";
                txtducexcper.Text = "0.00";
            }
            dr["IND_EX_AMT"] = EBasic;
            dr["IND_E_CESS_AMT"] = EEduCess;
            dr["IND_SH_CESS_AMT"] = EHEduCess;
            dr["IND_TEX_AMT"] = TEBasic;
            dr["IND_TE_CESS_AMT"] = TEEduCess;
            dr["IND_TSH_CESS_AMT"] = TEHEduCess;
            txtBasicExcAmt.Text = "0";
            dr["IND_E_CODE"] = ddlTariff.SelectedValue;
            dr["IND_E_TARIFF_NO"] = ddlTariff.SelectedItem;

            #endregion
            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    dgInvoiceAddDetail.Enabled = true;
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                dgInvoiceAddDetail.Enabled = true;
            }
            #endregion

            #region Binding data to Grid
            dgInvoiceAddDetail.Visible = true;
            dgInvoiceAddDetail.Enabled = true;
            dgInvoiceAddDetail.DataSource = ((DataTable)ViewState["dt2"]);
            dgInvoiceAddDetail.DataBind();
            #endregion

            #region Tax
            if (((DataTable)ViewState["dt2"]).Rows.Count == 1)
            {
                DataTable dtTax = CommonClasses.Execute("select CPOD_ST_CODE from CUSTPO_DETAIL,CUSTPO_MASTER where CPOM_CODE='" + ddlPONo.SelectedValue + "' and CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE=CPOD_CPOM_CODE");
                if (dtTax.Rows.Count > 0)
                {
                    ddlTaxName.SelectedValue = "-2147483648";
                }
            }
            #endregion

            #region Clear Controles
            ClearControles();
            #endregion
            GetTotal();
            ViewState["RowCount"] = 1;
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
        ddlTariff.SelectedIndex = 0;
        ddlItemCode.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        ddlPONo.SelectedIndex = 0;
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
        txtAmortAmount.Text = "0.000";
        txtAmortrate.Text = "0.000";
        txtsrNo.Text = "";
        txtDieAmount.Text = "0.00";
        txtDieRate.Text = "0.00";
        txtDiePending.Text = "0.00";
        txtTransAmt.Text = "";
        txtTransRate.Text = "";
        ViewState["str"] = "";
        ViewState["ItemUpdateIndex"] = "-1";
        ViewState["RowCount"] = 0;
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double ExcBasic = 0;
        double Excedu = 0;
        double ExcSH = 0;
        double AmortAmount = 0;
        try
        {
            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string QED_AMT = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblAMT"))).Text;
                    string Amort = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblAmortAmount"))).Text;
                    string Basic = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEBasic"))).Text;
                    string EduCess = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEEcess"))).Text;
                    string SH = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEHEcess"))).Text;
                    string TransAmt = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_TRASPORT_AMT"))).Text;
                    double TAmt = Convert.ToDouble(TransAmt);
                    double Amount = Convert.ToDouble(QED_AMT);
                    decTotal = decTotal + Amount + TAmt;
                    ExcBasic = ExcBasic + Convert.ToDouble(Basic);
                    Excedu = Excedu + Convert.ToDouble(EduCess);
                    ExcSH = ExcSH + Convert.ToDouble(SH);
                    if (chkIsSuppliement.Checked != true)
                    {
                        AmortAmount = AmortAmount + Convert.ToDouble(Amort);
                    }
                }
            }
            else
            {
                ddlTaxName.SelectedIndex = 0;
            }
            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));

            //userd for amort
            txtstroreloc.Text = string.Format("{0:0.00}", Math.Round(AmortAmount, 2));
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

            if (txtNetAmount.Text.Trim() == "")
            {
                txtNetAmount.Text = "0.00";
            }
            if (txtDiscAmt.Text.Trim() == "")
            {
                txtDiscAmt.Text = "0.00";
            }
            if (txtDiscPer.Text.Trim() == "")
            {
                txtDiscPer.Text = "0.00";
            }
            if (txtBasicExcAmt.Text.Trim() == "")
            {
                txtBasicExcAmt.Text = "0.00";
            }
            if (txtBasicExcPer.Text.Trim() == "")
            {
                txtBasicExcPer.Text = "0.00";
            }
            if (txtEdueceAmt.Text.Trim() == "")
            {
                txtEdueceAmt.Text = "0.00";
            }
            if (txtSHEExcPer.Text.Trim() == "")
            {
                txtSHEExcPer.Text = "0.00";
            }
            if (ddlTaxName.SelectedIndex == 0)
            {
                txtSalesTaxPer.Text = "0.00";
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtSalesTaxPer.Text.Trim() == "")
            {
                txtSalesTaxPer.Text = "0.00";
            }
            if (txtSalesTaxAmount.Text.Trim() == "")
            {
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtPackAmt.Text.Trim() == "")
            {
                txtPackAmt.Text = "0.00";
            }
            if (txtGrandAmt.Text.Trim() == "")
            {
                txtGrandAmt.Text = "0.00";
            }
            if (txtOtherCharges.Text.Trim() == "")
            {
                txtOtherCharges.Text = "0.00";
            }
            if (txtFreight.Text.Trim() == "")
            {
                txtFreight.Text = "0.00";
            }
            if (txtIncurance.Text.Trim() == "")
            {
                txtIncurance.Text = "0.00";
            }
            if (txtTransportAmt.Text.Trim() == "")
            {
                txtTransportAmt.Text = "0.00";
            }
            if (txtOctri.Text.Trim() == "")
            {
                txtOctri.Text = "0.00";
            }
            if (txtTCSAmt.Text.Trim() == "")
            {
                txtTCSAmt.Text = "0.00";
            }
            if (txtTransRate.Text.Trim() == "")
            {
                txtTransRate.Text = "0.00";
            }
            if (txtTransAmt.Text.Trim() == "")
            {
                txtTransAmt.Text = "0.00";
            }
            //Discu Amount
            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2));
            //Packing Amount
            txtPackAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackAmt.Text), 2));
            //Accessable Amount
            double AccessableValue = Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtstroreloc.Text) - Convert.ToDouble(txtDiscAmt.Text) + Convert.ToDouble(txtPackAmt.Text) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtIncurance.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTCSAmt.Text);
            txtAccessableAmt.Text = AccessableValue.ToString();
            txtAccessableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtAccessableAmt.Text), 2));
            //Basic Excise Amt
            double ExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100));

            DataTable dtCompState = CommonClasses.Execute("SELECT CM_STATE FROM COMPANY_MASTER WHERE CM_ID=" + (string)Session["CompanyId"] + "");
            DataTable dtPartyStae = CommonClasses.Execute("SELECT P_SM_CODE FROM PARTY_MASTER WHERE P_CODE='" + ddlCustomer.SelectedValue + "'");

            if (dtCompState.Rows[0][0].ToString() == dtPartyStae.Rows[0][0].ToString())
            {
                txtBasicExcAmt.Text = Math.Round(ExcAmt, 2).ToString();
                txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicExcAmt.Text), 2));

                //Educational Excise Amt
                double EduExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtducexcper.Text) / 100));
                txtEdueceAmt.Text = Math.Round((EduExcAmt), 2).ToString();
                txtEdueceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtEdueceAmt.Text), 2));

                //HigherEducational Excise Amt
                double HEduExcAmt = 0.00;
                txtSHEExcAmt.Text = Math.Round((HEduExcAmt), 2).ToString();
                txtSHEExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSHEExcAmt.Text), 2));
            }
            else
            {
                txtBasicExcAmt.Text = "0.00";
                txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicExcAmt.Text), 2));

                //Educational Excise Amt
                double EduExcAmt = 0.00;
                txtEdueceAmt.Text = EduExcAmt.ToString();
                txtEdueceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtEdueceAmt.Text), 2));

                //HigherEducational Excise Amt
                double HEduExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtSHEExcPer.Text) / 100));
                txtSHEExcAmt.Text = Math.Round((HEduExcAmt), 2).ToString();

            }
            //Taxable Amt
            double TaxableAmt = Convert.ToDouble(txtAccessableAmt.Text) - Convert.ToDouble(txtstroreloc.Text) + Convert.ToDouble(txtBasicExcAmt.Text) + Convert.ToDouble(txtEdueceAmt.Text) + Convert.ToDouble(txtSHEExcAmt.Text);
            txtTaxableAmt.Text = TaxableAmt.ToString();
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtTaxableAmt.Text), 2));

            //Tax Amt
            double TaxAmt = (Convert.ToDouble(txtTaxableAmt.Text) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
            txtSalesTaxAmount.Text = "0.00";
            txtSalesTaxPer.Text = "0.00";
            ddlTaxName.SelectedValue = "-2147483648";
            //GrandAmount
            //txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text) + Convert.ToDouble(txtstroreloc.Text)) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtIncurance.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTCSAmt.Text) + Convert.ToDouble(txtRoundingAmt.Text) - AmortAmount), 2));
            /*Change By Mahesh on 27082018:-Grand Amount shows Wrong when insert Transport/Freight/Insu etc amt */
            txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtRoundingAmt.Text) - AmortAmount), 2));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Incvoice", "GetTotal", Ex.Message);
        }
    }
    #endregion

    #region GetTotaldummy
    private void GetTotal1()
    {
        double decTotal = 0;
        double ExcBasic = 0;
        double Excedu = 0;
        double ExcSH = 0;
        double AmortAmount = 0;
        try
        {
            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));
            //userd for amort
            txtstroreloc.Text = string.Format("{0:0.00}", Math.Round(AmortAmount, 2));

            if (txtNetAmount.Text.Trim() == "")
            {
                txtNetAmount.Text = "0.00";
            }
            if (txtDiscAmt.Text.Trim() == "")
            {
                txtDiscAmt.Text = "0.00";
            }
            if (txtDiscPer.Text.Trim() == "")
            {
                txtDiscPer.Text = "0.00";
            }
            if (txtBasicExcAmt.Text.Trim() == "")
            {
                txtBasicExcAmt.Text = "0.00";
            }
            if (txtBasicExcPer.Text.Trim() == "")
            {
                txtBasicExcPer.Text = "0.00";
            }
            if (txtEdueceAmt.Text.Trim() == "")
            {
                txtEdueceAmt.Text = "0.00";
            }
            if (txtSHEExcPer.Text.Trim() == "")
            {
                txtSHEExcPer.Text = "0.00";
            }
            if (ddlTaxName.SelectedIndex == 0)
            {
                txtSalesTaxPer.Text = "0.00";
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtSalesTaxPer.Text.Trim() == "")
            {
                txtSalesTaxPer.Text = "0.00";
            }
            if (txtSalesTaxAmount.Text.Trim() == "")
            {
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtPackAmt.Text.Trim() == "")
            {
                txtPackAmt.Text = "0.00";
            }
            if (txtGrandAmt.Text.Trim() == "")
            {
                txtGrandAmt.Text = "0.00";
            }
            //Discu Amount
            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2));
            //Packing Amount
            txtPackAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackAmt.Text), 2));
            //Accessable Amount
            double AccessableValue = Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtstroreloc.Text) - Convert.ToDouble(txtDiscAmt.Text) + Convert.ToDouble(txtPackAmt.Text);
            txtAccessableAmt.Text = AccessableValue.ToString();
            txtAccessableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtAccessableAmt.Text), 2));
            //Basic Excise Amt
            double ExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100));

            if (txtBasicExcAmt.Text == "0")
            {
                txtBasicExcAmt.Text = Math.Round(ExcAmt, 0).ToString();
                txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicExcAmt.Text), 0));
            }

            //Educational Excise Amt
            double EduExcAmt = (Convert.ToDouble(txtBasicExcAmt.Text) * (Convert.ToDouble(txtducexcper.Text) / 100));
            txtEdueceAmt.Text = EduExcAmt.ToString();
            txtEdueceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtEdueceAmt.Text), 0));

            //HigherEducational Excise Amt
            double HEduExcAmt = (Convert.ToDouble(txtBasicExcAmt.Text) * (Convert.ToDouble(txtSHEExcPer.Text) / 100));
            txtSHEExcAmt.Text = HEduExcAmt.ToString();
            txtSHEExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSHEExcAmt.Text), 0));

            //Taxable Amt
            double TaxableAmt = Convert.ToDouble(txtAccessableAmt.Text) - Convert.ToDouble(txtstroreloc.Text) + Convert.ToDouble(txtBasicExcAmt.Text) + Convert.ToDouble(txtEdueceAmt.Text) + Convert.ToDouble(txtSHEExcAmt.Text);
            txtTaxableAmt.Text = TaxableAmt.ToString();
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtTaxableAmt.Text), 2));

            //Tax Amt
            double TaxAmt = (Convert.ToDouble(txtTaxableAmt.Text) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
            txtSalesTaxAmount.Text = TaxAmt.ToString();
            txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSalesTaxAmount.Text), 2));

            //GrandAmount
            txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text) + Convert.ToDouble(txtstroreloc.Text)) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtIncurance.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTCSAmt.Text) + Convert.ToDouble(txtRoundingAmt.Text) - AmortAmount), 2));
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
            if (txtDiscPer.Text.Trim() == "")
            {
                txtDiscPer.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtDiscPer.Text);
            txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2);
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
            if (txtFreight.Text.Trim() == "")
            {
                txtFreight.Text = "0.00";
            }
            // CalcExise();
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
            if (txtIncurance.Text.Trim() == "")
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
            if (txtTransportAmt.Text.Trim() == "")
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
            if (txtOctri.Text.Trim() == "")
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
            if (txtTaxableAmt.Text.Trim() == "")
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

    #region txtTrayQty_OnTextChanged
    protected void txtTrayQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTrayQty.Text.Trim() == "")
            {
                txtTrayQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtTrayQty.Text);

                txtTrayQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtTrayStock.Text.Trim() != "")
            {
                if (Convert.ToDouble(txtTrayStock.Text) < Convert.ToDouble(txtTrayQty.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Correct Tray Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtVQty.Text = "0.00";
                    txtVQty.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "txtTrayQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtTCSAmt_TextChanged
    protected void txtTCSAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTCSAmt.Text.Trim() == "")
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
            if (txtRoundingAmt.Text.Trim() == "")
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
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInvoiceAddDetail.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                ViewState["RowCount"] = 0;
                int rowindex = row.RowIndex;
                dgInvoiceAddDetail.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInvoiceAddDetail.DataSource = ((DataTable)ViewState["dt2"]);
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
                ViewState["str"] = "Modify";
                ViewState["RowCount"] = 0;
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtUOM.Text = ((Label)(row.FindControl("lblUOM"))).Text;
                ddlPONo.SelectedValue = ((Label)(row.FindControl("lblPO_CODE"))).Text;
                if (chkIsSuppliement.Checked)
                {
                    chkIsSuppliement_CheckedChanged(null, null);
                }
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    txtStockQty.Text = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtStockQty.Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text)), 3));
                }
                else
                {
                    txtStockQty.Text = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtStockQty.Text)), 3));
                }
                double pendQty = Convert.ToDouble(((Label)(row.FindControl("lblPEND_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text);
                txtPendingQty.Text = string.Format("{0:0.000}", Math.Round((pendQty), 3));
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text), 3));


                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    txtDailySchedule.Text = (Convert.ToDouble(txtDailySchedule.Text) + Convert.ToDouble(txtVQty.Text)).ToString();
                }


                txtActWght.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblACT_WGHT"))).Text), 2));
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblRATE"))).Text), 2));
                txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAMT"))).Text), 2));
                txtSubHeading.Text = ((Label)(row.FindControl("lblSubHeading"))).Text;
                txtBatchNo.Text = ((Label)(row.FindControl("lblBatch"))).Text;
                txtNoPackaeg.Text = (Convert.ToInt32(((Label)(row.FindControl("lblNoPack"))).Text)).ToString();
                txtQtyPerPack.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblPakQty"))).Text), 2));
                txtPackDesc.Text = ((Label)(row.FindControl("lblIND_PACK_DESC"))).Text;
                txtsrNo.Text = ((Label)(row.FindControl("lblIND_SR_NO"))).Text;
                txtDieRate.Text = ((Label)(row.FindControl("lblIND_DIE_RATE"))).Text;
                txtDiePending.Text = ((Label)(row.FindControl("lblIND_DIE_QTY"))).Text;
                txtDieAmount.Text = ((Label)(row.FindControl("lblIND_DIE_AMOUNT"))).Text;

                txtTransRate.Text = ((Label)(row.FindControl("lblIND_TRASPORT_RATE"))).Text;
                txtTransAmt.Text = ((Label)(row.FindControl("lblIND_TRASPORT_AMT"))).Text;

                ViewState["fileName"] = ((LinkButton)(row.FindControl("lnkTCView"))).Text;
                ViewState["fileName1"] = ((LinkButton)(row.FindControl("lnkPDIView"))).Text;
                lnkView.Text = ViewState["fileName"].ToString();
                lnkViewPDI.Text = ViewState["fileName1"].ToString();

                foreach (GridViewRow gvr in dgInvoiceAddDetail.Rows)
                {
                    LinkButton lnkButton = ((LinkButton)gvr.FindControl("lnkDelete"));
                    lnkButton.Enabled = false;
                }
            }
            if (e.CommandName == "ViewTC")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkTCView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOADTaxM/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkTCView"))).Text;
                        directory = "../../UpLoadPath/TaxMTCFile/" + code + "/" + filePath;
                    }
                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
            if (e.CommandName == "ViewPDI")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkPDIView"))).Text;
                        directory = "../../UpLoadPath/FILEUPLOADTaxMPDI/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkPDIView"))).Text;
                        directory = "../../UpLoadPath/TaxMPDIFile/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "dgMainPO_RowCommand", Ex.Message);
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
                string Invoice_No = "";
                string INM_SUPPLEMENTORY = "0";
                if (chkIsSuppliement.Checked)
                {
                    dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + " AND INM_INVOICE_TYPE=0 AND INM_TYPE='TAXINV' AND  INM_IS_SUPPLIMENT=1 AND INM_SUPPLEMENTORY=1 and ES_DELETE=0");
                    INM_SUPPLEMENTORY = "1";
                }
                else
                {
                    dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + " AND INM_INVOICE_TYPE=0 AND INM_TYPE='TAXINV'  AND  INM_IS_SUPPLIMENT=0 AND INM_SUPPLEMENTORY=0 and ES_DELETE=0");
                }
                if (dt.Rows.Count > 0)
                {
                    Inv_No = Convert.ToInt32(dt.Rows[0]["INM_NO"]);
                    Inv_No = Inv_No + 1;
                    Invoice_No = CommonClasses.GenBillNo(Inv_No);
                    Invoice_No = "J" + Invoice_No + "/" + (Convert.ToDateTime(Session["CompanyOpeningDate"].ToString()).Year % 100) + "-" + (Convert.ToDateTime(Session["CompanyClosingDate"].ToString()).Year % 100);
                }

                if (CommonClasses.Execute1("INSERT INTO INVOICE_MASTER (INM_CM_CODE,INM_NO,INM_DATE,INM_INVOICE_TYPE,INM_P_CODE,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_TAX_TCS,INM_TAX_TCS_AMT,INM_PACK_AMT,INM_G_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,INM_ACCESSIBLE_AMT,INM_TAXABLE_AMT,INM_ROUNDING_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME,INM_TRAY_CODE,INM_TRAY_QTY,INM_TNO,INM_TYPE  , INM_ELECTRREFNUM , INM_TERMSNCONDITIONS, INM_AUTHORIZEDNAME,INM_SUPPLEMENTORY,INM_MULTI_FLAG,INM_PLANT) VALUES ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','0','" + ddlCustomer.SelectedValue + "','" + txtNetAmount.Text + "','" + txtDiscPer.Text + "','" + txtDiscAmt.Text + "','" + txtBasicExcPer.Text + "','" + txtBasicExcAmt.Text + "','" + txtducexcper.Text + "','" + txtEdueceAmt.Text + "','" + txtSHEExcPer.Text + "','" + txtSHEExcAmt.Text + "','" + txtSalesTaxPer.Text + "','" + txtSalesTaxAmount.Text + "','0','" + txtTCSAmt.Text + "','" + txtPackAmt.Text + "','" + txtGrandAmt.Text + "','" + ddlTaxName.SelectedValue + "','" + txtstroreloc.Text.Trim().Replace("'", "\''") + "','" + txtVechicleNo.Text.Trim().Replace("'", "\''") + "','" + txtTransport.Text.Trim().Replace("'", "\''") + "','" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "','" + txtRemark.Text.Trim().Replace("'", "\''") + "','" + txtLRNo.Text.Trim().Replace("'", "\''") + "','" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "','" + txtAccessableAmt.Text + "','" + txtTaxableAmt.Text + "','" + txtRoundingAmt.Text + "','" + txtOtherCharges.Text + "','" + txtFreight.Text + "','" + txtIncurance.Text + "','" + txtTransportAmt.Text + "','" + txtOctri.Text + "','" + IsSuppliement + "','" + txtIssuetime.Text.Trim().Replace("'", "\''") + "','" + txtRemoveltime.Text.Trim().Replace("'", "\''") + "','" + ddlTray.SelectedValue + "','" + txtTrayQty.Text.Trim().Replace("'", "\''") + "','" + Invoice_No + "','TAXINV','" + txtElectrRefNum.Text.Trim().Replace("'", "\''") + "','" + txtTermsNConditions.Text.Trim().Replace("'", "\''") + "','" + txtAuthorizedName.Text.ToUpper().Trim().Replace("'", "\''") + "','" + INM_SUPPLEMENTORY + "','1','" + rbtWithAmt.SelectedValue + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(INM_CODE) from INVOICE_MASTER");
                    //Tray Stock Entry
                    #region Tray Stock Entry

                    //CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ddlTray.SelectedValue + "','" + Code + "','" + Inv_No + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + txtTrayQty.Text + "','-2147483642')");
                    //CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + txtTrayQty.Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ddlTray.SelectedValue + "'");
                    #endregion

                    for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                    {
                        result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL(IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC,IND_AMORTRATE,IND_AMORTAMT,IND_SR_NO,IND_DIE_QTY,IND_DIE_RATE,IND_DIE_AMOUNT,IND_E_CODE,IND_E_TARIFF_NO,IND_TRASPORT_RATE,IND_TRASPORT_AMT,IND_TEX_AMT,IND_TE_CESS_AMT,IND_TSH_CESS_AMT,IND_TC_FILE,IND_PDI_FILE) values ('" + Code + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SUBHEADING"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_BACHNO"].ToString().Replace("'", "\''") + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_NO_PACK"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PAK_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_EX_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SH_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PACK_DESC"].ToString().Replace("'", "\''") + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortRate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortAmount"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SR_NO"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_AMOUNT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_TARIFF_NO"] + "' ,'" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TRASPORT_RATE"] + "' ,'" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TRASPORT_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TEX_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TE_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TSH_CESS_AMT"] + "','" + ((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkTCView")).Text + "','" + ((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkPDIView")).Text + "')");

                        #region lnkView TC
                        if (((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkTCView")).Text != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/TaxMTCFile/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOADTaxM");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOADTaxM";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOADTaxM/" + (((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkTCView")).Text));
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/TaxMTCFile/" + Code + "/" + (((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkTCView")).Text));
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion lnkView

                        #region lnkViewPDI
                        if (((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkPDIView")).Text != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/TaxMPDIFile/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/FILEUPLOADTaxMPDI");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\FILEUPLOADTaxMPDI";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/FILEUPLOADTaxMPDI/" + (((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkPDIView")).Text));
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/TaxMPDIFile/" + Code + "/" + (((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkPDIView")).Text));
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion lnkView

                        if (!chkIsSuppliement.Checked)
                        {
                            CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DIEAMORTQTYRETURN = CPOD_DIEAMORTQTYRETURN + " + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_QTY"] + " where CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true)
                            {
                                if (rbtWithAmt.SelectedValue=="-2147483648")
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Code + "','" + Inv_No + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','-2147483642')");
                                }
                                else
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Code + "','" + Inv_No + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','-2147483627')");
                                }
                            }
                            //Removing Stock
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                        }
                    }
                    CommonClasses.WriteLog("Tax Invoice", "Save", "Tax Invoice", Convert.ToString(Inv_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewTaxInvoice.aspx", false);
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
                if (CommonClasses.Execute1("UPDATE INVOICE_MASTER SET INM_CM_CODE=" + Session["CompanyCode"] + " , INM_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',INM_P_CODE='" + ddlCustomer.SelectedValue + "',INM_NET_AMT='" + txtNetAmount.Text + "',INM_DISC='" + txtDiscPer.Text + "',INM_DISC_AMT='" + txtDiscAmt.Text + "',INM_BEXCISE='" + txtBasicExcPer.Text + "',INM_BE_AMT='" + txtBasicExcAmt.Text + "',INM_EDUC_CESS='" + txtducexcper.Text + "',INM_EDUC_AMT='" + txtEdueceAmt.Text + "',INM_H_EDUC_CESS='" + txtSHEExcPer.Text + "',INM_H_EDUC_AMT = '" + txtSHEExcAmt.Text + "',INM_S_TAX='" + txtSalesTaxPer.Text + "',INM_S_TAX_AMT='" + txtSalesTaxAmount.Text + "',INM_PACK_AMT='" + txtPackAmt.Text + "',INM_G_AMT='" + txtGrandAmt.Text + "',INM_T_CODE='" + ddlTaxName.SelectedValue + "',INM_STO_LOC='" + txtstroreloc.Text + "',INM_VEH_NO='" + txtVechicleNo.Text.Trim().Replace("'", "\''") + "',INM_TRANSPORT='" + txtTransport.Text.Trim().Replace("'", "\''") + "',INM_ISSUE_DATE='" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "',INM_REMOVAL_DATE='" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "',INM_REMARK='" + txtRemark.Text.Trim().Replace("'", "\''") + "',INM_LR_NO='" + txtLRNo.Text.Trim().Replace("'", "\''") + "',INM_LR_DATE='" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "',INM_ACCESSIBLE_AMT='" + txtAccessableAmt.Text + "',INM_TAXABLE_AMT='" + txtTaxableAmt.Text + "',INM_ROUNDING_AMT='" + txtRoundingAmt.Text + "',INM_OTHER_AMT='" + txtOtherCharges.Text + "',INM_FREIGHT='" + txtFreight.Text + "',INM_INSURANCE='" + txtIncurance.Text + "',INM_TRANS_AMT='" + txtTransportAmt.Text + "',INM_OCTRI_AMT='" + txtOctri.Text + "',INM_IS_SUPPLIMENT='" + IsSuppliement + "',INM_ISSU_TIME='" + txtIssuetime.Text + "',INM_REMOVEL_TIME='" + txtRemoveltime.Text + "' ,INM_ELECTRREFNUM=    '" + txtElectrRefNum.Text.Trim().Replace("'", "\''") + "' ,INM_TERMSNCONDITIONS='" + txtTermsNConditions.Text.Trim().Replace("'", "\''") + "' ,INM_AUTHORIZEDNAME=    '" + txtAuthorizedName.Text.ToUpper().Trim().Replace("'", "\''") + "',INM_TAX_TCS_AMT='" + txtTCSAmt.Text + "' WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    if (!chkIsSuppliement.Checked)
                    {
                        DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE ,ISNULL(IND_DIE_QTY,0) AS IND_DIE_QTY FROM INVOICE_DETAIL where IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");

                        for (int i = 0; i < dtq.Rows.Count; i++)
                        {
                            CommonClasses.Execute("UPDATE CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                            CommonClasses.Execute("UPDATE CUSTPO_DETAIL set CPOD_DIEAMORTQTYRETURN = CPOD_DIEAMORTQTYRETURN - " + dtq.Rows[i]["IND_DIE_QTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                        }
                        result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and STL_DOC_TYPE='TAXINV'");
                        //Tray Current Balance Update
                        //  CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + TrayQty + " where I_CODE='" + ddlTray.SelectedValue + "'");
                    }
                    result = CommonClasses.Execute1("DELETE FROM INVOICE_DETAIL WHERE IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                    if (result)
                    {
                        for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC,IND_AMORTRATE,IND_AMORTAMT,IND_SR_NO,IND_DIE_QTY,IND_DIE_RATE,IND_DIE_AMOUNT,IND_E_CODE,IND_E_TARIFF_NO,IND_TRASPORT_RATE,IND_TRASPORT_AMT,IND_TEX_AMT,IND_TE_CESS_AMT,IND_TSH_CESS_AMT,IND_TC_FILE,IND_PDI_FILE) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SUBHEADING"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_BACHNO"].ToString().Replace("'", "\''") + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_NO_PACK"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PAK_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_EX_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SH_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PACK_DESC"].ToString().Replace("'", "\''") + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortRate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortAmount"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SR_NO"] + "' ,'" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_AMOUNT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_TARIFF_NO"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TRASPORT_RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TRASPORT_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TEX_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TE_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_TSH_CESS_AMT"] + "','" + ((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkTCView")).Text + "','" + ((LinkButton)dgInvoiceAddDetail.Rows[i].FindControl("lnkPDIView")).Text + "')");
                            if (!chkIsSuppliement.Checked)
                            {
                                CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DIEAMORTQTYRETURN = CPOD_DIEAMORTQTYRETURN + " + ((DataTable)ViewState["dt2"]).Rows[i]["IND_DIE_QTY"] + " where CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                                }
                                //Entry In Stock Ledger
                                if (result == true)
                                {
                                    if (rbtWithAmt.SelectedValue == "-2147483648")
                                    {
                                        result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','-2147483642')");
                                    }
                                    else
                                    {
                                        result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','TAXINV','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','-2147483627')");
                                    }
                                }
                                //Removing Stock
                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                                }
                            }
                        }
                        CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Tax Invoice", "Update", "Tax Invoice", Convert.ToString(Convert.ToInt32(ViewState["mlCode"].ToString())), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
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
            ViewState["RowCount"] = 0;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region txtDate_TextChanged
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        txtIssuedate.Text = txtDate.Text;
        txtremovaldate.Text = txtDate.Text;
    }
    #endregion

    #region chkIsSuppliement_CheckedChanged
    protected void chkIsSuppliement_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsSuppliement.Checked == true)
        {
            txtActWght.Enabled = true;
            txtActWght.ReadOnly = false;
            txtRate.Enabled = true;
            txtRate.ReadOnly = false;
            txtBasicExcPer.Enabled = true;
            txtBasicExcAmt.Enabled = true;
            txtducexcper.Enabled = true;
            txtSHEExcPer.Enabled = true;
            ddlTaxName.Enabled = true;
        }
        else
        {
            txtActWght.Enabled = false;
            txtActWght.ReadOnly = true;
            txtRate.Enabled = false;
            txtBasicExcAmt.Enabled = false;
            txtBasicExcPer.Enabled = false;
            txtducexcper.Enabled = false;
            txtSHEExcPer.Enabled = false;
            ddlTaxName.Enabled = false;
        }
    }
    #endregion

    #region txtBasicExcAmt_TextChanged
    protected void txtBasicExcAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcAmt.Text);
        txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion

    #region rbtWithAmt_SelectedIndexChanged
    protected void rbtWithAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
        LoadICode();
        LoadPO();
        //ddlItemCode_SelectedIndexChanged(null, null);
    }
    #endregion rbtWithAmt_SelectedIndexChanged
}
