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
using System.Web.Caching;

public partial class Transactions_ADD_IndentDetail : System.Web.UI.Page
{
    #region General Declaration
    DirectoryInfo ObjSearchDir;
    double PedQty = 0;
    static int mlCode = 0;
    static string right = "";
    static string ItemUpdateIndex = "-1";
    static string ItemUpdateIndex1 = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    DataTable dtFilter1 = new DataTable();
    static DataTable dt3 = new DataTable();
    public static string str = "";
    public static string str1 = "";
    public static int Index = 0;
    public static int Index1 = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();
    DataTable dtFileUpload = new DataTable();
    string fileName = "";
    string fileName1 = "";
    string fileName2 = "";
    string fileName3 = "";

    #endregion

    public string Msg = "";
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
                ViewState["Index"] = Index;
                ViewState["Index1"] = Index1;
                ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                ViewState["ItemUpdateIndex1"] = ItemUpdateIndex1;
                ViewState["dt2"] = dt2;
                ViewState["dt3"] = dt3;
                ViewState["fileName"] = fileName;
                ViewState["fileName1"] = fileName1;
                ViewState["fileName2"] = fileName2;
                ViewState["fileName3"] = fileName3;
                ViewState["str"] = str;
                ViewState["str1"] = str1;
                LoadIndentType();
                LoadDeprtment();

                LoadCombos();
                LoadProCode();
                txtIndentNo.Enabled = false;
                ddlSupplierName.Visible = false;
                lblsuppname.Visible = false;
                ((DataTable)ViewState["dt2"]).Rows.Clear();
                ((DataTable)ViewState["dt3"]).Rows.Clear();
                str = "";
                ViewState["str"] = str;

                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='278'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");

                    }
                    else if (Request.QueryString[0].Equals("Export"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        export(ViewState["mlCode"].ToString());
                    }
                    else if (Request.QueryString[0].Equals("PExport"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        pexport(ViewState["mlCode"].ToString());
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("Approve"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("Approve");
                    }
                    else if (Request.QueryString[0].Equals("Authorize"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("Authorize");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtIndentDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtValidity.Text = System.DateTime.Now.AddDays(30).ToString("dd MMM yyyy");
                        BlankGrid();
                        BlankgridFile();
                        LoadCombos();
                        LoadIndentType();
                        LoadDeprtment();
                        dtFilter.Rows.Clear();
                        txtIndentDate.Attributes.Add("readonly", "readonly");
                        txtValidity.Attributes.Add("readonly", "readonly");
                        txtValidity.Enabled = false;
                        //txtChallanDate.Attributes.Add("readonly", "readonly");
                        //txtInvoiceDate.Attributes.Add("readonly", "readonly");
                    }
                    //ddlSupplier.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Indent Detail", "PageLoad", ex.Message);
                }
            }
            #region MasterFiles

            if (IsPostBack && fuCheque.PostedFile != null)
            {
                if (fuCheque.PostedFile.FileName.Length > 0)
                {
                    fileName = fuCheque.PostedFile.FileName;
                    ViewState["fileName"] = fileName;
                    UploadCheque(null, null);
                }
            }
            if (IsPostBack && fugst.PostedFile != null)
            {
                if (fugst.PostedFile.FileName.Length > 0)
                {
                    fileName1 = fugst.PostedFile.FileName;
                    ViewState["fileName1"] = fileName1;
                    UploadGST(null, null);
                }
            }
            if (IsPostBack && fucertificate.PostedFile != null)
            {
                if (fucertificate.PostedFile.FileName.Length > 0)
                {
                    fileName2 = fucertificate.PostedFile.FileName;
                    ViewState["fileName2"] = fileName2;
                    UploadCertificate(null, null);
                }
            }
            #endregion MasterFiles

            #region DetailFile
            if (IsPostBack && fuFile.PostedFile != null)
            {
                if (fuFile.PostedFile.FileName.Length > 0)
                {
                    fileName3 = fuFile.PostedFile.FileName;
                    ViewState["fileName3"] = fileName3;
                    UploadFile(null, null);
                }
            }
            #endregion

        }
    }

    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_DESC", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SPECIFICATION", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("IND_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_RATE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_AMT", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInwardMaster.DataSource = dtFilter;
            dgInwardMaster.DataBind();
            ((DataTable)ViewState["dt2"]).Clear();

        }
    }


    public void export(string cpom_code)
    {

        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute("SELECT IN_PRO_CODE,IND_DESC,IND_QTY,IND_RATE,IND_AMT FROM INDENT_DETAIL,INDENT_MASTER WHERE INM_CODE=IND_INM_CODE AND ES_DELETE=0 AND INM_CODE='" + cpom_code + "' AND IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ORDER BY IND_DESC");

        dtExport.Columns.Add("Sr No.");
        dtExport.Columns.Add("Project Code");
        dtExport.Columns.Add("Item Code");
        dtExport.Columns.Add("Item Name");
        dtExport.Columns.Add("Qty");
        dtExport.Columns.Add("Rate");
        dtExport.Columns.Add("Amount");

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["IN_PRO_CODE"].ToString(),
                              dtResult.Rows[i]["IND_DESC"].ToString(),
                              dtResult.Rows[i]["IND_DESC"].ToString(),
                              dtResult.Rows[i]["IND_QTY"].ToString(),
                              dtResult.Rows[i]["IND_RATE"].ToString(),
                              dtResult.Rows[i]["IND_AMT"].ToString()
                             );
        }

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=IndentExport.xls");
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
                    //if (i == 4)
                    //{
                    //    if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                    //    {
                    //        HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                    //    }
                    //    else
                    //    {
                    //        HttpContext.Current.Response.Write("<Td>");
                    //    }
                    //}
                    //else
                    //{
                    HttpContext.Current.Response.Write("<Td>");
                    //}
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

    }
    public void pexport(string cpom_code)
    {

        DataTable dt = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable dtExport = new DataTable();
        dtResult = CommonClasses.Execute(" select  IN_SUPP_NAME,	IN_CONTACT	,IN_ADDRESS	,IN_EMAIL	,IN_MOB	,IN_GST,IN_PAN,	IN_PIN  from INDENT_MASTER where INM_CODE='" + cpom_code + "'");

        dtExport.Columns.Add("Sr No.");
        dtExport.Columns.Add("Supplier Name");
        dtExport.Columns.Add("Contact Name");
        dtExport.Columns.Add("Address");
        dtExport.Columns.Add("Email ID");
        dtExport.Columns.Add("Mobile No.");
        dtExport.Columns.Add("GST No.");
        dtExport.Columns.Add("PAN No.");
        dtExport.Columns.Add("Pin Code");

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            dtExport.Rows.Add(i + 1,
                              dtResult.Rows[i]["IN_SUPP_NAME"].ToString(),
                              dtResult.Rows[i]["IN_CONTACT"].ToString(),
                              dtResult.Rows[i]["IN_ADDRESS"].ToString(),
                              dtResult.Rows[i]["IN_EMAIL"].ToString(),
                              dtResult.Rows[i]["IN_MOB"].ToString(),
                              dtResult.Rows[i]["IN_GST"].ToString(),
                              dtResult.Rows[i]["IN_PAN"].ToString(),
                              dtResult.Rows[i]["IN_PIN"].ToString()
                             );
        }

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=PartyExport.xls");
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
                    HttpContext.Current.Response.Write("<Td>");
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

    }
    public void BlankgridFile()
    {
        dtFilter1.Clear();
        if (dtFilter1.Columns.Count == 0)
        {
            dgfileupload.Enabled = false;
            dtFilter1.Columns.Add(new System.Data.DataColumn("IFU_CODE", typeof(string)));
            dtFilter1.Columns.Add(new System.Data.DataColumn("IFU_DESC", typeof(string)));
            dtFilter1.Columns.Add(new System.Data.DataColumn("IFU_FILE", typeof(string)));
            dtFilter1.Columns.Add(new System.Data.DataColumn("IFU_APPROVE", typeof(string)));
            dtFilter1.Rows.Add(dtFilter1.NewRow());
            dgfileupload.DataSource = dtFilter1;
            dgfileupload.DataBind();
            ((DataTable)ViewState["dt3"]).Clear();
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {

        try
        {
            DataTable dtParty = new DataTable();

            dt = CommonClasses.Execute(" SELECT I_CODE,I_CODENO +' - '+I_NAME  as I_CODENO FROM ITEM_MASTER WHERE  I_CM_COMP_ID = '1' and ITEM_MASTER.ES_DELETE='0'  ORDER BY I_CODENO +' - '+I_NAME ");

            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "I_CODENO";
            ddlItem.DataValueField = "I_CODE";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("Please Select part ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
        }
    }
    #endregion

    #region UploadCheque
    protected void UploadCheque(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            fuCheque.SaveAs(Server.MapPath("~/UpLoadPath/InFILEUPLOAD/" + ViewState["fileName"].ToString()));
        }
        else
        {
            fuCheque.SaveAs(Server.MapPath("~/UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
        lnkcheque.Visible = true;
        lnkcheque.Text = ViewState["fileName"].ToString();
    }
    #endregion

    #region UploadGST
    protected void UploadGST(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            fugst.SaveAs(Server.MapPath("~/UpLoadPath/InFILEUPLOAD/" + ViewState["fileName1"].ToString()));
        }
        else
        {
            fugst.SaveAs(Server.MapPath("~/UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName1"].ToString()));
        }
        lnkGST.Visible = true;
        lnkGST.Text = ViewState["fileName1"].ToString();
    }
    #endregion

    #region UploadCertificate
    protected void UploadCertificate(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            fucertificate.SaveAs(Server.MapPath("~/UpLoadPath/InFILEUPLOAD/" + ViewState["fileName2"].ToString()));
        }
        else
        {
            fucertificate.SaveAs(Server.MapPath("~/UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
        }
        lnkCertificate.Visible = true;
        lnkCertificate.Text = ViewState["fileName2"].ToString();
    }
    #endregion

    #region UploadFile
    protected void UploadFile(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFileMulti/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFileMulti/" + ViewState["mlCode"].ToString() + "/");
        }
        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            fuFile.SaveAs(Server.MapPath("~/UpLoadPath/IndentFileMulti/" + ViewState["fileName3"].ToString()));
        }
        else
        {
            fuFile.SaveAs(Server.MapPath("~/UpLoadPath/IndentFileMulti/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName3"].ToString()));
        }
        lnkFile.Visible = true;
        lnkFile.Text = ViewState["fileName3"].ToString();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        IframeViewPDF.Attributes["src"] = null;
        ModalPopDocument.Hide();
    }
    #endregion

    #region LoadProCode
    private void LoadProCode()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select distinct PROCM_CODE,PROCM_NAME from PROJECT_CODE_MASTER where ES_DELETE=0 and PROCM_COMP_ID=" + (string)Session["CompanyId"] + " AND PROCM_APPROVE=1 order by PROCM_NAME");
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "PROCM_NAME";
            ddlProject.DataValueField = "PROCM_CODE";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("Project Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Order", "LoadProCode", Ex.Message);
        }
    }
    #endregion

    protected void lnkPost_Click(object sender, EventArgs e)
    {
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {

        DataTable dtcat = CommonClasses.Execute("select  ISNULL(IM_APPROVAL1,0) AS IM_APPROVAL1,ISNULL(IM_APPROVAL2,0) AS IM_APPROVAL2,ISNULL(IM_APPROVAL3,0) AS IM_APPROVAL3, ISNULL(IM_APPROVDBY1,0) AS IM_APPROVDBY1, ISNULL(IM_APPROVDBY2,0) AS IM_APPROVDBY2, ISNULL(IM_APPROVDBY3,0) AS IM_APPROVDBY3  FROM INDENT_TYPE_MASTER where IM_CODE='" + ddlIndentType.SelectedValue + "' AND (IM_APPROVDBY1='" + Session["UserCode"] + "' OR IM_APPROVDBY2='" + Session["UserCode"] + "' OR IM_APPROVDBY3='" + Session["UserCode"] + "')");

        if (dtcat.Rows.Count > 0)
        {

            if (dtcat.Rows[0]["IM_APPROVDBY1"].ToString().ToUpper() == Session["UserCode"].ToString())
            {
                if (Request.QueryString[0].Equals("Approve"))
                {
                    CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_APPROVE=1 ,IN_APPROVEBY='" + Session["UserCode"] + "' ,IN_STATUS=1 WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"]).ToString() + "'");
                    Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights For Approve";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }


    protected void btnAuthorize_Click(object sender, EventArgs e)
    {

        DataTable dtcat = CommonClasses.Execute("select  ISNULL(IM_APPROVAL1,0) AS IM_APPROVAL1,ISNULL(IM_APPROVAL2,0) AS IM_APPROVAL2,ISNULL(IM_APPROVAL3,0) AS IM_APPROVAL3, ISNULL(IM_APPROVDBY1,0) AS IM_APPROVDBY1, ISNULL(IM_APPROVDBY2,0) AS IM_APPROVDBY2, ISNULL(IM_APPROVDBY3,0) AS IM_APPROVDBY3  FROM INDENT_TYPE_MASTER where IM_CODE='" + ddlIndentType.SelectedValue + "' AND (IM_APPROVDBY1='" + Session["UserCode"] + "' OR IM_APPROVDBY2='" + Session["UserCode"] + "' OR IM_APPROVDBY3='" + Session["UserCode"] + "')");

        if (dtcat.Rows.Count > 0)
        {

            if (dtcat.Rows[0]["IM_APPROVAL3"].ToString().ToUpper() == "FALSE" || dtcat.Rows[0]["IM_APPROVAL3"].ToString().ToUpper() == "0")
            {
                if (dtcat.Rows[0]["IM_APPROVDBY2"].ToString().ToUpper() == Session["UserCode"].ToString())
                {
                    if (Request.QueryString[0].Equals("Authorize"))
                    {
                        CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_AUTHORIZE=1,IN_AUTHORIZEBY='" + Session["UserCode"] + "'  ,IN_AUTHORIZE1=1,IN_AUTHORIZEBY1='" + Session["UserCode"] + "',IN_STATUS=3,IN_VALIDITY='" + Convert.ToDateTime(txtValidity.Text).ToString("dd/MMM/yyyy") + "'  WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"]).ToString() + "'");
                        Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
                    }
                }
                else
                {

                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights For Authorize";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else
            {
                if (dtcat.Rows[0]["IM_APPROVDBY2"].ToString().ToUpper() == Session["UserCode"].ToString())
                {

                    CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_AUTHORIZE=1,IN_AUTHORIZEBY='" + Session["UserCode"] + "' ,IN_VALIDITY='" + Convert.ToDateTime(txtValidity.Text).ToString("dd/MMM/yyyy") + "'   WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"]).ToString() + "'");
                    Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);

                }
                else if (dtcat.Rows[0]["IM_APPROVDBY3"].ToString().ToUpper() == Session["UserCode"].ToString())
                {

                    CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_AUTHORIZE1=1,IN_AUTHORIZEBY1='" + Session["UserCode"] + "',IN_STATUS=3,IN_VALIDITY='" + Convert.ToDateTime(txtValidity.Text).ToString("dd/MMM/yyyy") + "' WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"]).ToString() + "'");
                    Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);

                }
                else
                {

                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights For Authorize";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }


        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_APPROVE=2  ,IN_APPROVEBY='" + Session["UserCode"] + "' ,  IN_STATUS=1   WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"]).ToString() + "'");
        Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
    }
    #region LoadIndentType
    public void LoadIndentType()
    {
        try
        {
            DataTable dtIndenttype = new DataTable();
            dtIndenttype = CommonClasses.Execute("SELECT IM_CODE,(IM_DESC+''+'('+IM_SHORT+')') AS NAME FROM INDENT_TYPE_MASTER WHERE ES_DELETE=0 AND IM_CM_ID='" + Session["CompanyID"] + "'");
            ddlIndentType.DataSource = dtIndenttype;
            ddlIndentType.DataTextField = "NAME";
            ddlIndentType.DataValueField = "IM_CODE";
            ddlIndentType.DataBind();
            ddlIndentType.Items.Insert(0, new ListItem("Select Indent type", "0"));
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Indent Detail", "LoadIndentType", Ex.Message);
        }
    }
    #endregion

    #region LoadDeprtment
    public void LoadDeprtment()
    {
        try
        {
            DataTable dtDepart = new DataTable();
            dtDepart = CommonClasses.Execute("SELECT DM_CODE,DM_NAME FROM DEPARTMENT_MASTER WHERE ES_DELETE=0 AND DM_CM_COMP_ID='" + Session["CompanyID"] + "' ORDER BY DM_NAME");
            ddlDepartment.DataSource = dtDepart;
            ddlDepartment.DataTextField = "DM_NAME";
            ddlDepartment.DataValueField = "DM_CODE";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("Select Department", "0"));
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Indent Detail", "LoadDeprtment", Ex.Message);
        }
    }
    #endregion

    #region FillItem
    //    try
    //    {
    //        dt = CommonClasses.FillCombo("ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER", "I_NAME", "I_CODE", "SUPP_PO_MASTER.ES_DELETE=0  AND SPOM_POTYPE=0 AND SPOM_CODE=SPOD_SPOM_CODE and i_code=SPOD_I_CODE and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "    ORDER BY I_NAME", ddlItemName);
    //        ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));

    //        DataTable dt1 = CommonClasses.FillCombo("ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER", "I_CODENO", "I_CODE", "SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_POTYPE=0  AND SPOM_CODE=SPOD_SPOM_CODE and i_code=SPOD_I_CODE and (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0))>0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "   ORDER BY I_CODENO", ddlItemCode);
    //        ddlItemCode.Items.Insert(0, new ListItem("Please Select Item ", "0"));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
    //    }
    //    #endregion

    //    #region FillUOM
    //    try
    //    {
    //        dt = CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_UOM_NAME", ddlUom);
    //        ddlUom.Items.Insert(0, new ListItem(" ", "0"));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
    //    }
    //    #endregion

    //    #region PO
    //    try
    //    {
    //        dt = CommonClasses.Execute("select SPOM_CODE,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO from SUPP_PO_MASTER,COMPANY_MASTER where ES_DELETE=0 and SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 AND SPOM_CM_COMP_ID='" + Session["CompanyId"] + "'   AND SPOM_POTYPE=0  AND  SPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and SPOM_CM_CODE=CM_CODE order by SPOM_CODE desc");
    //        ddlPoNumber.DataSource = dt;
    //        ddlPoNumber.DataTextField = "SPOM_PO_NO";
    //        ddlPoNumber.DataValueField = "SPOM_CODE";
    //        ddlPoNumber.DataBind();
    //        ddlPoNumber.Items.Insert(0, new ListItem("Select PO", "0"));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "LoadCombos", Ex.Message);
    //    }
    //    #endregion
    //}
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlIndentType.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select indent type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlIndentType.Focus();
                return;
            }
            if (ddlDepartment.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select department";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlIndentType.Focus();
                return;
            }
            if (ddlProject.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Project Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlProject.Focus();
                return;
            }
            if (ddlSuppType.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select supplier type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlIndentType.Focus();
                return;
            }
            ////if (txtBudgetNo.Text == "" || txtBudgetNo.Text == "0")
            ////{
            ////    PanelMsg.Visible = true;
            //////    lblmsg.Text = "Enter budget number";
            //////    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ////    ddlIndentType.Focus();
            ////    return;
            ////}

            if (ddlSuppType.SelectedValue == "1")
            {
                if (txtSuppName.Text == "" || txtSuppName.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter supplier name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtSuppName.Focus();
                    return;
                }
                if (txtContactNo.Text == "" || txtContactNo.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter contact person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtContactNo.Focus();
                    return;
                }
                if (txtAddress.Text == "" || txtAddress.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter address";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtAddress.Focus();
                    return;
                }
                if (txtEmailid.Text == "" || txtEmailid.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter email id";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtEmailid.Focus();
                    return;
                }
                if (txtMobileNo.Text == "" || txtMobileNo.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter mobile no.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtMobileNo.Focus();
                    return;
                }
                if (txtGSTNo.Text == "" || txtGSTNo.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter GST no.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtGSTNo.Focus();
                    return;
                }

                if (txtPinCode.Text == "" || txtPinCode.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter pincode";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtPinCode.Focus();
                    return;
                }
                if (txtPanNo.Text == "" || txtPanNo.Text == null)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter pan no.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtPanNo.Focus();
                    return;
                }

                PanelMsg.Visible = false;
            }
            //if (dgfileupload.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
            //    {
            //        string QED_AMT = ((LinkButton)(dgfileupload.Rows[i].FindControl("lnkFiledesc"))).Text;
            //        if (QED_AMT.Trim() == "")
            //        {
            //            PanelMsg.Visible = true;
            //            lblmsg.Text = "Enter attach Quotation File ";
            //            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //            dgfileupload.Focus();
            //            return;
            //        }
            //    }
            //}
            if (txtPaymentTerm.Text == "" || txtPaymentTerm.Text == null)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Payment Terms";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPaymentTerm.Focus();
                return;
            }
            if (lbl.Text.ToUpper() != "TRUE"  )
            {
                if (Convert.ToDouble(lblamt.Text) > Convert.ToDouble(txtBalamt.Text))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Indent amount should be less than Bal. Amount For Indent";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    lblamt.Focus();
                    return;
                }
            }

            if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
            {
                SaveRec();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Fill the table";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDesc.Focus();
                return;
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Indent Detail", "btnSubmit_Click", Ex.Message);
        }

    }
    #endregion


    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {

            txtIndentDate.Attributes.Add("readonly", "readonly");
            txtValidity.Attributes.Add("readonly", "readonly");
            LoadIndentType();
            LoadDeprtment();
            LoadSupplier();
            LoadProCode();
            //dt = CommonClasses.Execute("SELECT IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,IWM_CODE,IWM_INWARD_TYPE,IWM_NO,IWM_TYPE,IWM_DATE,IWM_P_CODE,IWM_CHALLAN_NO,IWM_CHAL_DATE,IWM_EGP_NO,IWM_LR_NO,IWM_OCT_NO,IWM_VEH_NO,IWM_CM_CODE FROM INWARD_MASTER where ES_DELETE=0 and IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and IWM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            dt = CommonClasses.Execute("SELECT INM_CODE,IN_SUPP_CODE,CONVERT(VARCHAR,IN_VALIDITY,106) AS IN_VALIDITY,IN_TYPE,IN_TNO,CONVERT(VARCHAR,IN_DATE,106) AS IN_DATE,IN_PRO_CODE,IN_BUDGET,IN_PROJECT,IN_DEPT,IN_SUPP_TYPE,IN_SUPP_CODE,IN_SUPP_NAME,IN_CONTACT,	IN_ADDRESS,	IN_EMAIL,IN_MOB,IN_GST,IN_PAN,IN_PIN,IN_GSTFILE,IN_CHEQUE,IN_CERTIFICATE,IN_PAYMENT,ES_DELETE,IN_NO ,ISNULL(IN_REASON,'') AS IN_REASON  FROM INDENT_MASTER 	WHERE IN_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["INM_CODE"]).ToString();
                // DataTable dtno = new DataTable();
                //dtno = CommonClasses.Execute("select IM_SHORT from INDENT_TYPE_MASTER,INDENT_MASTER WHERE IM_CODE=IN_TYPE AND INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " AND INDENT_TYPE_MASTER.ES_DELETE=0 AND IN_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + "");
                txtIndentNo.Text = dt.Rows[0]["IN_TNO"].ToString();
                txtIndentDate.Text = Convert.ToDateTime(dt.Rows[0]["IN_DATE"]).ToString("dd MMM yyyy");
                txtValidity.Text = Convert.ToDateTime(dt.Rows[0]["IN_VALIDITY"]).ToString("dd MMM yyyy");
                ddlIndentType.SelectedValue = dt.Rows[0]["IN_TYPE"].ToString();
                ddlIndentType_SelectedIndexChanged(null, null);
                ddlProjectCode.SelectedItem.Text = dt.Rows[0]["IN_PRO_CODE"].ToString();
                //ddlProjectCode_SelectedIndexChanged(null, null);
                txtBudgetNo.Text = dt.Rows[0]["IN_BUDGET"].ToString();
                // txtProjectCode.Text = (dt.Rows[0]["IN_PROJECT"]).ToString();
                ddlProject.SelectedValue = (dt.Rows[0]["IN_PROJECT"]).ToString();
                ddlProject_SelectedIndexChanged(null, null);
                ddlDepartment.SelectedValue = dt.Rows[0]["IN_DEPT"].ToString();
                ddlDepartment_SelectedIndexChanged(null, null);
                ddlSuppType.SelectedValue = dt.Rows[0]["IN_SUPP_TYPE"].ToString();
                if (ddlSuppType.SelectedValue == "2")
                {
                    lblsuppname.Visible = true;
                    ddlSupplierName.Visible = true;
                    ddlSupplierName.SelectedValue = dt.Rows[0]["IN_SUPP_CODE"].ToString();
                    ddlSupplierName_SelectedIndexChanged(null, null);
                }
                //ddlSuppType_SelectedIndexChanged(null, null);
                txtSuppName.Text = (dt.Rows[0]["IN_SUPP_NAME"]).ToString();
                txtContactNo.Text = (dt.Rows[0]["IN_CONTACT"]).ToString();
                txtAddress.Text = (dt.Rows[0]["IN_ADDRESS"]).ToString();
                txtEmailid.Text = (dt.Rows[0]["IN_EMAIL"]).ToString();
                txtMobileNo.Text = (dt.Rows[0]["IN_MOB"]).ToString();
                txtGSTNo.Text = (dt.Rows[0]["IN_GST"]).ToString();
                txtPanNo.Text = (dt.Rows[0]["IN_PAN"]).ToString();
                txtPinCode.Text = (dt.Rows[0]["IN_PIN"]).ToString();
                txtPaymentTerm.Text = (dt.Rows[0]["IN_PAYMENT"]).ToString();
                lnkcheque.Text = (dt.Rows[0]["IN_CHEQUE"]).ToString();
                lnkGST.Text = (dt.Rows[0]["IN_GSTFILE"]).ToString();
                lnkCertificate.Text = (dt.Rows[0]["IN_CERTIFICATE"]).ToString();
                txtReason.Text = (dt.Rows[0]["IN_REASON"]).ToString();
                //in modify state bind file names
                ViewState["fileName1"] = lnkGST.Text;
                ViewState["fileName"] = lnkcheque.Text;
                ViewState["fileName2"] = lnkCertificate.Text;

                //dtInwardDetail = CommonClasses.Execute("SELECT IWD_BATCH_NO,IWD_IWM_CODE,IWD_I_CODE,I_CODENO,I_NAME,Convert(varchar,SPOM_PO_NO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) as SPOM_PO_NO,cast(IWD_RATE  as numeric(20,2)) as IWD_RATE,IWD_UOM_CODE,IWD_CPOM_CODE,cast(IWD_CH_QTY as numeric(10,3)) as IWD_CH_QTY,cast(IWD_REV_QTY as numeric(10,3)) as IWD_REV_QTY,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME,IWD_REMARK FROM INWARD_DETAIL,INWARD_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,SUPP_PO_MASTER,COMPANY_MASTER WHERE IWM_CODE=IWD_IWM_CODE and IWD_I_CODE=ITEM_MASTER.I_CODE AND SPOM_CODE=IWD_CPOM_CODE AND IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and  SPOM_CM_CODE=CM_CODE and IWD_IWM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                dtInwardDetail = CommonClasses.Execute("SELECT IND_CODE, IND_DESC,IND_QTY,IND_RATE,IND_AMT,ISNULL(IND_SPECIFICATION,0) AS IND_SPECIFICATION FROM INDENT_DETAIL,INDENT_MASTER WHERE IND_INM_CODE=INM_CODE AND IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                    GetTotal();
                }
                dtFileUpload = CommonClasses.Execute("SELECT IFU_CODE,IFU_FILE,IFU_DESC,ISNULL(IFU_APPROVE,0) AS IFU_APPROVE FROM INDENT_FILE_UPLOAD,INDENT_MASTER WHERE IFU_INM_CODE=INM_CODE AND IFU_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND IN_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
                if (dtFileUpload.Rows.Count != 0)
                {
                    dgfileupload.DataSource = dtFileUpload;
                    dgfileupload.DataBind();
                    ViewState["dt3"] = dtFileUpload;
                }
            }
            if (str == "VIEW")
            {
                //ddlSupplier.Enabled = false;
                ddlIndentType.Enabled = false;
                txtIndentNo.Enabled = false;
                ddlProjectCode.Enabled = false;
                txtIndentDate.Enabled = false;
                txtBudgetNo.Enabled = false;
                ddlDepartment.Enabled = false;
                dgInwardMaster.Enabled = false;
                // txtProjectCode.Enabled = false;
                ddlSuppType.Enabled = false;
                txtSuppName.Enabled = false;
                txtAddress.Enabled = false;
                txtContactNo.Enabled = false;
                txtEmailid.Enabled = false;
                txtMobileNo.Enabled = false;
                txtGSTNo.Enabled = false;
                txtPinCode.Enabled = false;
                txtPanNo.Enabled = false;
                BtnInsert.Enabled = false;
                txtDesc.Enabled = false;
                txtQty.Enabled = false;
                txtRate.Enabled = false;
                txtAmount.Enabled = false;
                btnSubmit.Enabled = false;
                txtPaymentTerm.Enabled = false;
                ddlSupplierName.Enabled = false;
                fucertificate.Enabled = false;
                fugst.Enabled = false;
                fucertificate.Enabled = false;
                fuFile.Enabled = false;
                txtFileDesc.Enabled = false;
                // dgfileupload.Enabled = false;
                btnUpload.Enabled = false;
                dgfileupload.Columns[5].Visible = true;
                btnApprove.Visible = true;
                btnReject.Visible = true;
                btnApprove.Enabled = false;
                btnAuthorize.Enabled = false;
                btnReject.Enabled = false;
                txtReason.Enabled = false;
            }

            if (str == "MOD")
            {
                txtReason.Enabled = false;
                ddlIndentType.Enabled = false;
                txtIndentNo.Enabled = false;
                ddlProjectCode.Enabled = false;
                txtIndentDate.Enabled = false;
                txtBudgetNo.Enabled = false;
                ddlDepartment.Enabled = false;
                // txtProjectCode.Enabled = false;


                if (ddlSuppType.SelectedValue == "2")
                {
                    txtSuppName.Enabled = false;
                    txtAddress.Enabled = false;
                    txtContactNo.Enabled = false;
                    txtEmailid.Enabled = false;
                    txtMobileNo.Enabled = false;
                    txtGSTNo.Enabled = false;
                    txtPinCode.Enabled = false;
                    txtPanNo.Enabled = false;
                }



                //ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("INDENT_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            if (str == "Approve" || str == "Authorize")
            {
                txtReason.Enabled = false;
                ddlIndentType.Enabled = false;
                txtIndentNo.Enabled = false;
                ddlProjectCode.Enabled = false;
                txtIndentDate.Enabled = false;
                txtBudgetNo.Enabled = false;
                ddlDepartment.Enabled = false;
                dgInwardMaster.Enabled = false;
                //  txtProjectCode.Enabled = false;
                ddlSuppType.Enabled = false;
                txtSuppName.Enabled = false;
                txtAddress.Enabled = false;
                txtContactNo.Enabled = false;
                txtEmailid.Enabled = false;
                txtMobileNo.Enabled = false;
                txtGSTNo.Enabled = false;
                txtPinCode.Enabled = false;
                txtPanNo.Enabled = false;
                BtnInsert.Enabled = false;
                txtDesc.Enabled = false;
                txtQty.Enabled = false;
                txtRate.Enabled = false;
                txtAmount.Enabled = false;
                btnSubmit.Enabled = false;
                txtPaymentTerm.Enabled = false;
                ddlSupplierName.Enabled = false;
                fuCheque.Enabled = false;
                fugst.Enabled = false;
                fucertificate.Enabled = false;
                fuFile.Enabled = false;
                txtFileDesc.Enabled = false;
                btnUpload.Enabled = false;
                dgfileupload.Columns[5].Visible = true;
                if (str == "Approve")
                {
                    btnApprove.Visible = true;
                    btnAuthorize.Visible = false;
                }
                if (str == "Authorize")
                {
                    btnApprove.Visible = false;
                    btnAuthorize.Visible = true;
                }
                if (str != "Authorize")
                {
                    btnReject.Visible = true;
                }

                dgfileupload.Columns[0].Visible = false;
                dgfileupload.Columns[1].Visible = false;
                //btnApprove.Enabled = false;
                dgfileupload.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "ViewRec", Ex.Message);
        }
    }
    #endregion


    protected void ddlIndentType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    protected void lnkPop_Click(object sender, EventArgs e)
    {
        DataTable dtIndent = new DataTable();
        dtIndent = CommonClasses.Execute(" select IN_SUPP_NAME,IN_CONTACT,IN_MOB,IN_TNO,CONVERT(varchar,IN_DATE,106) AS  IN_DATE, ISNULL(IN_USEDIN,'') AS IN_USEDIN,SUM(IND_AMT) AS IND_AMT from INDENT_MASTER,INDENT_DETAIL   where INM_CODE=IND_INM_CODE AND   IN_STATUS!=2 AND ES_DELETE=0 AND IN_PROJECT='" + ddlProject.SelectedValue + "' group by IN_SUPP_NAME,IN_CONTACT,IN_MOB,IN_TNO, ISNULL(IN_USEDIN,''),IN_DATE");
        if (dtIndent.Rows.Count > 0)
        {
            dgDetail.Enabled = true;
            dgDetail.DataSource = dtIndent;
            dgDetail.DataBind();

            PnlDetail.Visible = true;
            ModalPopupDetail.Show();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Could not saved";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }


    protected void dgDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetail.PageIndex = e.NewPageIndex;
            DataTable dtIndent = new DataTable();
            dtIndent = CommonClasses.Execute(" select IN_SUPP_NAME,IN_CONTACT,IN_MOB,IN_TNO,CONVERT(varchar,IN_DATE,106) AS  IN_DATE, ISNULL(IN_USEDIN,'') AS IN_USEDIN,SUM(IND_AMT) AS IND_AMT from INDENT_MASTER,INDENT_DETAIL   where INM_CODE=IND_INM_CODE AND   IN_STATUS!=2 AND ES_DELETE=0 AND IN_PROJECT='" + ddlProject.SelectedValue + "' group by IN_SUPP_NAME,IN_CONTACT,IN_MOB,IN_TNO, ISNULL(IN_USEDIN,''),IN_DATE");
            if (dtIndent.Rows.Count > 0)
            {
                dgDetail.Enabled = true;
                dgDetail.DataSource = dtIndent;
                dgDetail.DataBind();

                PnlDetail.Visible = true;
                ModalPopupDetail.Show();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Could not saved";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception)
        {
        }
    }


    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProject.SelectedValue != "0")
        {
            DataTable DtProj = new DataTable();

            DtProj = CommonClasses.Execute(" select ISNULL(PROCM_AMT,0) AS PROCM_AMT ,PROCM_ALLOW  from PROJECT_CODE_MASTER where PROCM_CODE='" + ddlProject.SelectedValue + "'");


            if (DtProj.Rows.Count > 0)
            {
                txtBudgetAmt.Text = DtProj.Rows[0]["PROCM_AMT"].ToString();
                lbl.Text = DtProj.Rows[0]["PROCM_ALLOW"].ToString();
            }
            else
            {
                txtBudgetAmt.Text = "0";
                lbl.Text = "0";
            }
            DataTable DtInd = new DataTable();
            if (Request.QueryString[0].Equals("MODIFY"))
                DtInd = CommonClasses.Execute("  select ISNULL(SUM(IND_AMT),0) As IND_AMT from INDENT_MASTER,INDENT_DETAIL   where INM_CODE=IND_INM_CODE AND   IN_STATUS!=2 AND ES_DELETE=0 AND IN_PROJECT='" + ddlProject.SelectedValue + "'");
            else
                DtInd = CommonClasses.Execute("  select ISNULL(SUM(IND_AMT),0) As IND_AMT from INDENT_MASTER,INDENT_DETAIL   where INM_CODE=IND_INM_CODE AND   IN_STATUS!=2 AND ES_DELETE=0 AND IN_PROJECT='" + ddlProject.SelectedValue + "' AND INM_CODE!='" + ViewState["mlCode"].ToString() + "'");

            if (DtInd.Rows.Count > 0)
            {
                txtIndRaisedAmt.Text = DtInd.Rows[0]["IND_AMT"].ToString();
            }
            else
            {
                txtIndRaisedAmt.Text = "0";
            }
            txtBalamt.Text = (Convert.ToDouble(txtBudgetAmt.Text) - Convert.ToDouble(txtIndRaisedAmt.Text)).ToString();
        }
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlProjectCode_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlSuppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSuppType.SelectedValue == "2")
            {
                lblsuppname.Visible = true;
                 
                ddlSupplierName.Visible = true;
                LoadSupplier();
                txtSuppName.Enabled = false;
                txtContactNo.Enabled = false;
                txtAddress.Enabled = false;
                txtEmailid.Enabled = false;
                txtMobileNo.Enabled = false;
                txtGSTNo.Enabled = false;
                txtPanNo.Enabled = false;
                txtPinCode.Enabled = false;
                txtSuppName.Text = "";
                txtContactNo.Text = "";
                txtAddress.Text = "";
                txtEmailid.Text = "";
                txtMobileNo.Text = "";
                txtGSTNo.Text = "";
                txtPanNo.Text = "";
                txtPinCode.Text = "";
            }
            else
            {
                lblsuppname.Visible = false;
                ddlSupplierName.Visible = false;
                txtSuppName.Text = "";
                txtContactNo.Text = "";
                txtAddress.Text = "";
                txtEmailid.Text = "";
                txtMobileNo.Text = "";
                txtGSTNo.Text = "";
                txtPanNo.Text = "";
                txtPinCode.Text = "";
                txtSuppName.Enabled = true;
                txtContactNo.Enabled = true;
                txtAddress.Enabled = true;
                txtEmailid.Enabled = true;
                txtMobileNo.Enabled = true;
                txtGSTNo.Enabled = true;
                txtPanNo.Enabled = true;
                txtPinCode.Enabled = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "ddlSuppType_SelectedIndexChanged", Ex.Message);
        }
    }
    protected void ddlSupplierName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtsuppdetail = new DataTable();
            dtsuppdetail = CommonClasses.Execute("select distinct P_CONTACT,P_NAME,P_ADD1,P_PIN_CODE,P_MOB,P_EMAIL,P_GST_NO,P_PAN from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=1 and P_TYPE='2' AND  P_ACTIVE_IND=1 and P_CODE='" + ddlSupplierName.SelectedValue + "'");
            txtSuppName.Text = (dtsuppdetail.Rows[0]["P_NAME"]).ToString().Replace("'", "\''");
            txtContactNo.Text = (dtsuppdetail.Rows[0]["P_CONTACT"]).ToString();
            txtAddress.Text = (dtsuppdetail.Rows[0]["P_ADD1"]).ToString().Replace("'", "\''");
            txtEmailid.Text = (dtsuppdetail.Rows[0]["P_EMAIL"]).ToString();
            txtMobileNo.Text = (dtsuppdetail.Rows[0]["P_MOB"]).ToString();
            txtGSTNo.Text = (dtsuppdetail.Rows[0]["P_GST_NO"]).ToString();
            txtPanNo.Text = (dtsuppdetail.Rows[0]["P_PAN"]).ToString();
            txtPinCode.Text = (dtsuppdetail.Rows[0]["P_PIN_CODE"]).ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "ddlSupplierName_SelectedIndexChanged", Ex.Message);
        }

    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtDesc.Text = ddlItem.SelectedItem.ToString();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "ddlSupplierName_SelectedIndexChanged", Ex.Message);
        }

    }

    #region LoadSupplier
    public void LoadSupplier()
    {
        try
        {
            DataTable dtsupp = new DataTable();
            dtsupp = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2'    AND    P_ACTIVE_IND=1  order by P_NAME");
            ddlSupplierName.DataSource = dtsupp;
            ddlSupplierName.DataTextField = "P_NAME";
            ddlSupplierName.DataValueField = "P_CODE";
            ddlSupplierName.DataBind();
            ddlSupplierName.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "ddlSupplierName_SelectedIndexChanged", Ex.Message);
        }
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
                string strSql = "";
                int ginNo = 0;
                int inti = 0;
                string Invoice_No = "";
                DataTable dt = new DataTable();
                DataTable dtint = new DataTable();
                dtint = CommonClasses.Execute("Select  isnull(max(IN_SERIAL),0) AS IN_SERIAL FROM INDENT_MASTER WHERE    IN_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");

                dt = CommonClasses.Execute("Select isnull(max(IN_NO),0) as IN_NO FROM INDENT_MASTER WHERE IN_TYPE='" + ddlIndentType.SelectedValue + "' AND   IN_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    ginNo = Convert.ToInt32(dt.Rows[0]["IN_NO"]);
                    ginNo = ginNo + 1;
                    Invoice_No = CommonClasses.GenBillNo(ginNo);

                }
                if (dtint.Rows.Count > 0)
                {
                    inti = Convert.ToInt32(dtint.Rows[0]["IN_SERIAL"]);
                    inti = inti + 1;
                }

                if (ddlSuppType.SelectedValue == "1")
                {
                    strSql = "INSERT INTO INDENT_MASTER(IN_CM_CODE,IN_TYPE,IN_NO,IN_DATE,IN_PRO_CODE,IN_BUDGET,IN_PROJECT,IN_DEPT,IN_SUPP_TYPE,IN_SUPP_CODE,IN_SUPP_NAME,IN_CONTACT,IN_ADDRESS,IN_EMAIL,IN_MOB,IN_GST,IN_PAN,IN_PIN,IN_GSTFILE,IN_CHEQUE,IN_CERTIFICATE,IN_PAYMENT,IN_USER,IN_APPROVE,IN_SERIAL,IN_REASON,IN_STATUS,IN_VALIDITY) VALUES ('" + Session["CompanyCode"] + "','" + ddlIndentType.SelectedValue + "'," + ginNo + ",'" + Convert.ToDateTime(txtIndentDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlProjectCode.SelectedItem + "','" + txtBudgetNo.Text + "','" + ddlProject.SelectedValue + "','" + ddlDepartment.SelectedValue + "','" + ddlSuppType.SelectedValue + "',0,                                      '" + txtSuppName.Text.Replace("'", "\''") + "','" + txtContactNo.Text.Replace("'", "\''") + "','" + txtAddress.Text.Replace("'", "\''") + "','" + txtEmailid.Text.Replace("'", "\''") + "','" + txtMobileNo.Text.Replace("'", "\''") + "','" + txtGSTNo.Text + "','" + txtPanNo.Text + "','" + txtPinCode.Text + "','" + ViewState["fileName1"].ToString().Replace("'", "\''") + "','" + ViewState["fileName"].ToString().Replace("'", "\''") + "','" + ViewState["fileName2"].ToString().Replace("'", "\''") + "','" + txtPaymentTerm.Text.Replace("'", "\''") + "','" + Convert.ToInt32(Session["UserCode"]) + "','0','" + inti + "','" + txtReason.Text.Replace("'", "\''") + "',0,'" + Convert.ToDateTime(txtValidity.Text).ToString("dd/MMM/yyyy") + "')";
                }
                else
                {
                    strSql = "INSERT INTO INDENT_MASTER(IN_CM_CODE,IN_TYPE,IN_NO,IN_DATE,IN_PRO_CODE,IN_BUDGET,IN_PROJECT,IN_DEPT,IN_SUPP_TYPE,IN_SUPP_CODE,IN_SUPP_NAME,IN_CONTACT,IN_ADDRESS,IN_EMAIL,IN_MOB,IN_GST,IN_PAN,IN_PIN,IN_GSTFILE,IN_CHEQUE,IN_CERTIFICATE,IN_PAYMENT,IN_USER,IN_APPROVE,IN_SERIAL,IN_REASON,IN_STATUS,IN_VALIDITY) VALUES ('" + Session["CompanyCode"] + "','" + ddlIndentType.SelectedValue + "'," + ginNo + ",'" + Convert.ToDateTime(txtIndentDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlProjectCode.SelectedItem + "','" + txtBudgetNo.Text + "','" + ddlProject.SelectedValue + "','" + ddlDepartment.SelectedValue + "','" + ddlSuppType.SelectedValue + "','" + ddlSupplierName.SelectedValue + "','" + txtSuppName.Text.Replace("'", "\''") + "','" + txtContactNo.Text.Replace("'", "\''") + "','" + txtAddress.Text.Replace("'", "\'',") + "','" + txtEmailid.Text.Replace("'", "\''") + "','" + txtMobileNo.Text.Replace("'", "\''") + "','" + txtGSTNo.Text + "','" + txtPanNo.Text + "','" + txtPinCode.Text + "','" + ViewState["fileName1"].ToString().Replace("'", "\''") + "','" + ViewState["fileName"].ToString().Replace("'", "\''") + "','" + ViewState["fileName2"].ToString().Replace("'", "\''") + "','" + txtPaymentTerm.Text.Replace("'", "\''") + "','" + Convert.ToInt32(Session["UserCode"]) + "','0','" + inti + "','" + txtReason.Text.Replace("'", "\''") + "',0,'" + Convert.ToDateTime(txtValidity.Text).ToString("dd/MMM/yyyy") + "')";
                }
                if (CommonClasses.Execute1(strSql))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(INM_CODE) from INDENT_MASTER");

                    #region Master_File_Uplaod
                    #region Cheque
                    if (ViewState["fileName"].ToString() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + Code + ""); //IndentFile InFILEUPLOAD

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    

                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\InFILEUPLOAD";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD/" + ViewState["fileName"].ToString());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + Code + "/" + ViewState["fileName"].ToString());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion Cheque

                    #region GST
                    if (ViewState["fileName1"].ToString() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    

                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\InFILEUPLOAD ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD/" + ViewState["fileName1"].ToString());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + Code + "/" + ViewState["fileName1"].ToString());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion GST

                    #region Certificate
                    if (ViewState["fileName2"].ToString() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    

                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\InFILEUPLOAD ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/InFILEUPLOAD/" + ViewState["fileName2"].ToString());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/IndentFile/" + Code + "/" + ViewState["fileName2"].ToString());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion Certificate

                    #endregion Master_File_Uplaod

                    for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO INDENT_DETAIL(IND_INM_CODE,IND_DESC,IND_QTY,IND_RATE,IND_AMT,IND_SPECIFICATION)VALUES(" + Code + ",'" + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_DESC")).Text + "'," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_QTY")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_RATE")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_AMT")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_SPECIFICATION")).Text.Replace("'", "\''") + ")");

                    }
                    for (int i = 0; i < dgfileupload.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO INDENT_FILE_UPLOAD(IFU_INM_CODE,IFU_FILE,IFU_DESC)VALUES('" + Code + "','" + ((LinkButton)dgfileupload.Rows[i].FindControl("lnkFiledesc")).Text + "','" + ((Label)dgfileupload.Rows[i].FindControl("lblIFU_DESC")).Text + "')");

                        #region FilDesc
                        if (((LinkButton)dgfileupload.Rows[i].FindControl("lnkFiledesc")).Text != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/IndentFileMulti/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/IndentFileMulti");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    

                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\IndentFileMulti";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/IndentFileMulti/" + ((LinkButton)dgfileupload.Rows[i].FindControl("lnkFiledesc")).Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/IndentFileMulti/" + Code + "/" + ((LinkButton)dgfileupload.Rows[i].FindControl("lnkFiledesc")).Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion FilDesc
                    }
                    result = true;
                    DataTable dtno = new DataTable();
                    dtno = CommonClasses.Execute("SELECT IM_SHORT FROM INDENT_TYPE_MASTER,INDENT_MASTER WHERE INM_CODE='" + Code + "' AND IN_TYPE='" + ddlIndentType.SelectedValue + "' AND IN_TYPE=IM_CODE");
                    if (dtno.Rows.Count > 0)
                    {
                        string num = "";
                        num = (dtno.Rows[0]["IM_SHORT"].ToString()) + Invoice_No;
                        CommonClasses.Execute("UPDATE INDENT_MASTER SET IN_TNO='" + num + "' WHERE INM_CODE='" + Code + "'");
                    }
                    dt2.Rows.Clear();
                    dt3.Rows.Clear();
                    CommonClasses.WriteLog("Indent Detail", "Save", "Indent Detail", ginNo.ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Could not saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (ddlSuppType.SelectedValue == "2")
                {
                    if (CommonClasses.Execute1("UPDATE INDENT_MASTER SET IN_SUPP_CODE='" + ddlSupplierName.SelectedValue + "',IN_SUPP_NAME='" + txtSuppName.Text.Replace("'", "\''") + "',IN_CONTACT='" + txtContactNo.Text + "',IN_ADDRESS='" + txtAddress.Text.Replace("'", "\''") + "',IN_EMAIL='" + txtEmailid.Text.Replace("'", "\''") + "',IN_MOB='" + txtMobileNo.Text + "',IN_GST='" + txtGSTNo.Text + "',IN_PAN='" + txtPanNo.Text + "',IN_PIN='" + txtPinCode.Text + "',IN_GSTFILE='" + ViewState["fileName1"].ToString() + "',IN_CHEQUE='" + ViewState["fileName"].ToString() + "',IN_CERTIFICATE='" + ViewState["fileName2"].ToString() + "',IN_PAYMENT='" + txtPaymentTerm.Text.Replace("'", "\''") + "',IN_SUPP_TYPE='" + ddlSuppType.SelectedValue + "'  ,IN_REASON='" + txtReason.Text.Replace("'", "\''") + "'   WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and IN_CM_CODE='" + Session["CompanyCode"] + "'")) ;
                    {
                        result = CommonClasses.Execute1("DELETE FROM INDENT_DETAIL WHERE IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                            {
                                CommonClasses.Execute("INSERT INTO INDENT_DETAIL(IND_INM_CODE,IND_DESC,IND_QTY,IND_RATE,IND_AMT,IND_SPECIFICATION)VALUES(" + ViewState["mlCode"].ToString() + ",'" + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_DESC")).Text + "'," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_QTY")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_RATE")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_AMT")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_SPECIFICATION")).Text + ")");
                            }

                        }
                        result = CommonClasses.Execute1("DELETE FROM INDENT_FILE_UPLOAD WHERE IFU_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgfileupload.Rows.Count; i++)
                            {
                                CommonClasses.Execute("INSERT INTO INDENT_FILE_UPLOAD(IFU_INM_CODE,IFU_DESC,IFU_FILE)VALUES(" + ViewState["mlCode"].ToString() + ",'" + ((Label)dgfileupload.Rows[i].FindControl("lblIFU_DESC")).Text + "','" + ((LinkButton)dgfileupload.Rows[i].FindControl("lnkFiledesc")).Text + "')");
                            }
                        }
                    }
                }
                else
                {
                    if (CommonClasses.Execute1("UPDATE INDENT_MASTER SET IN_SUPP_CODE='0',IN_SUPP_NAME='" + txtSuppName.Text.Replace("'", "\''") + "',IN_CONTACT='" + txtContactNo.Text + "',IN_ADDRESS='" + txtAddress.Text.Replace("'", "\''") + "',IN_EMAIL='" + txtEmailid.Text.Replace("'", "\''") + "',IN_MOB='" + txtMobileNo.Text + "',IN_GST='" + txtGSTNo.Text + "',IN_PAN='" + txtPanNo.Text + "',IN_PIN='" + txtPinCode.Text + "',IN_GSTFILE='" + ViewState["fileName1"].ToString().Replace("'", "\''") + "',IN_CHEQUE='" + ViewState["fileName"].ToString().Replace("'", "\''") + "',IN_CERTIFICATE='" + ViewState["fileName2"].ToString().Replace("'", "\''") + "',IN_PAYMENT='" + txtPaymentTerm.Text.Replace("'", "\''") + "',IN_SUPP_TYPE='" + ddlSuppType.SelectedValue + "'  ,IN_REASON='" + txtReason.Text.Replace("'", "\''") + "' WHERE INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and IN_CM_CODE='" + Session["CompanyCode"] + "'")) ;
                    {
                        result = CommonClasses.Execute1("DELETE FROM INDENT_DETAIL WHERE IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                            {
                                CommonClasses.Execute("INSERT INTO INDENT_DETAIL(IND_INM_CODE,IND_DESC,IND_QTY,IND_RATE,IND_AMT)VALUES(" + ViewState["mlCode"].ToString() + ",'" + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_DESC")).Text + "'," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_QTY")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_RATE")).Text + "," + ((Label)dgInwardMaster.Rows[i].FindControl("lblIND_AMT")).Text + ")");
                            }
                        }
                        result = CommonClasses.Execute1("DELETE FROM INDENT_FILE_UPLOAD WHERE IFU_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgfileupload.Rows.Count; i++)
                            {
                                CommonClasses.Execute("INSERT INTO INDENT_FILE_UPLOAD(IFU_INM_CODE,IFU_DESC,IFU_FILE)VALUES(" + ViewState["mlCode"].ToString() + ",'" + ((Label)dgfileupload.Rows[i].FindControl("lblIFU_DESC")).Text + "','" + ((LinkButton)dgfileupload.Rows[i].FindControl("lnkFiledesc")).Text + "')");
                            }
                        }
                    }
                }

                CommonClasses.RemoveModifyLock("INDENT_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                CommonClasses.WriteLog("Indent Detail", "Update", "Indent Detail", txtIndentNo.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                dt2.Rows.Clear();
                dt3.Rows.Clear();
                result = true;

                Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Detail", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ShowMessage
    //public bool ShowMessage(string DiveName, string Message, string MessageType)
    //{
    //    try
    //    {
    //        if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
    //        {
    //            Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
    //        }
    //        return true;
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "ShowMessage", Ex.Message);
    //        return false;
    //    }
    //}
    #endregion

    #region Numbering
    //string Numbering()
    //{
    //    int GenGINNO;
    //    DataTable dt = new DataTable();
    //    dt = CommonClasses.Execute("select max(IWM_NO) as IWM_NO from INWARD_MASTER where IWM_CM_CODE='" + Session["CompanyCode"] + "' AND IWM_TYPE='IWIM' and ES_DELETE=0");
    //    if (dt.Rows[0]["IWM_NO"] == null || dt.Rows[0]["IWM_NO"].ToString() == "")
    //    {
    //        GenGINNO = 1;
    //    }
    //    else
    //    {
    //        GenGINNO = Convert.ToInt32(dt.Rows[0]["IWM_NO"]) + 1;
    //    }
    //    return GenGINNO.ToString();
    //}
    #endregion


    protected void dgInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void dgfileupload_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #region dgInwardMaster_RowCommand
    protected void dgInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgInwardMaster.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                dgInwardMaster.DataBind();
                if (dgInwardMaster.Rows.Count == 0)
                    BlankGrid();
                GetTotal();
            }
            if (e.CommandName == "Select")
            {
                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                string s = ((Label)(row.FindControl("lblIND_CODE"))).Text;
                ddlItem.SelectedValue = ((Label)(row.FindControl("lblIND_SPECIFICATION"))).Text;

                txtDesc.Text = ((Label)(row.FindControl("lblIND_DESC"))).Text;
                txtQty.Text = ((Label)(row.FindControl("lblIND_QTY"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblIND_RATE"))).Text;
                txtAmount.Text = ((Label)(row.FindControl("lblIND_AMT"))).Text;

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    protected void dgfileupload_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";

            ViewState["Index1"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgfileupload.Rows[Convert.ToInt32(ViewState["Index1"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgfileupload.DeleteRow(rowindex);
                ((DataTable)ViewState["dt3"]).Rows.RemoveAt(rowindex);
                dgfileupload.DataSource = ((DataTable)ViewState["dt3"]);
                dgfileupload.DataBind();
                if (dgfileupload.Rows.Count == 0)
                    BlankgridFile();
            }
            if (e.CommandName == "Select")
            {
                ViewState["str1"] = "Modify";
                ViewState["ItemUpdateIndex1"] = e.CommandArgument.ToString();
                string s = ((Label)(row.FindControl("lblIFU_CODE"))).Text;
                ViewState["fileName3"] = ((LinkButton)(row.FindControl("lnkFiledesc"))).Text;
                txtFileDesc.Text = ((Label)(row.FindControl("lblIFU_DESC"))).Text.ToString();
                lnkFile.Visible = true;
                lnkFile.Text = ((LinkButton)(row.FindControl("lnkFiledesc"))).Text;
            }
            foreach (GridViewRow gvr in dgfileupload.Rows)
            {
                LinkButton lnkButton = ((LinkButton)gvr.FindControl("lnkDelete"));
                lnkButton.Enabled = false;
            }

            if (e.CommandName == "ViewFiledesc")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    if (Request.QueryString[0].Equals("INSERT"))
                    {
                        filePath = ((LinkButton)(row.FindControl("lnkFiledesc"))).Text;
                        directory = "../../UpLoadPath/IndentFileMulti/" + filePath;
                    }
                    else
                    {
                        code = ViewState["mlCode"].ToString();
                        filePath = ((LinkButton)(row.FindControl("lnkFiledesc"))).Text;
                        directory = "../../UpLoadPath/IndentFileMulti/" + code + "/" + filePath;
                    }

                    IframeViewPDF.Attributes["src"] = directory;
                    ModalPopDocument.Show();
                    PopDocument.Visible = true;
                }
            }

            if (e.CommandName == "Status")
            {

                string bm_code = ((Label)(row.FindControl("lblIFU_CODE"))).Text;
                CommonClasses.Execute("UPDATE INDENT_FILE_UPLOAD SET IFU_APPROVE=1 WHERE IFU_CODE='" + bm_code + "' ");
                dgfileupload.Enabled = false;
                // btnApprove.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Detail", "dgfileupload_RowCommand", Ex.Message);
        }
    }

    protected void dgfileupload_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //string bm_code = ((Label)(dgfileupload.FindControl("lblIFU_CODE"))).Text;
        //DataTable dtexists = new DataTable();
        //for (int i = 0; i < dgfileupload.Rows.Count; i++)
        //{
        //    dtexists = CommonClasses.Execute("SELECT * FROM INDENT_FILE_UPLOAD WHERE IFU_APPROVE=1 AND IFU_CODE='" + bm_code + "'");
        //}
        //if (dtexists.Rows.Count > 0)
        //{
        //    dgfileupload.Enabled = false;
        //}

        //int index = Convert.ToInt32(e.);

        ////string lnkPost = (LinkButton)e.Row.FindControl("lnkPost").ToString();
        //LinkButton linkbtn = (LinkButton)dgfileupload.Columns[5].ToString();
        //string sapNo1 = linkbtn.Text;
        //if (e.Row.Cells[5].Text.ToString() == "Approve")
        //{
        //    dgfileupload.Enabled = false;
        //}
        //else
        //{
        //    dgfileupload.Enabled = true;
        //}
    }

    #region clearDetail
    //private void clearDetail()
    //{
    //    try
    //    {
    //        ddlItemName.SelectedValue = "0";
    //        ddlItemCode.SelectedValue = "0";
    //        ddlPoNumber.SelectedValue = "0";
    //        txtRate.Text = "0.00";
    //        txtChallanQty.Text = "0.00";
    //        txtRecdQty.Text = "0.00";
    //        ViewState["str"] = "";
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError(" Material Inward ", "clearDetail", Ex.Message);
    //    }
    //}
    #endregion


    #region GetTotal
    private void GetTotal()
    {

        try
        {
            double Amount = 0.00;

            for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgInwardMaster.Rows[i].FindControl("lblIND_AMT"))).Text;
                Amount = Amount + Convert.ToDouble(QED_AMT);
            }

            lblamt.Text = Amount.ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Total", "GetTotal", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;


            if (txtDesc.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please enter description";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDesc.Focus();
                return;
            }

            if (txtQty.Text.Trim() == "" || Convert.ToDecimal(txtQty.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please enter qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtQty.Focus();
                return;
            }
            //double chq = Convert.ToDouble(txtChallanQty.Text);
            //double pdq = Convert.ToDouble(txtPendingQty.Text);



            if (txtRate.Text.Trim() == "" || Convert.ToDecimal(txtRate.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please enter rate";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRate.Focus();
                return;
            }

            PanelMsg.Visible = false;

            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string Desc = ((Label)(dgInwardMaster.Rows[i].FindControl("lblIND_DESC"))).Text;
                    string itemcode = ((Label)(dgInwardMaster.Rows[i].FindControl("lblIND_SPECIFICATION"))).Text;

                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (itemcode == ddlItem.SelectedValue)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record already exist for this description in table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (itemcode == ddlItem.SelectedValue && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record already exist for this description in table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
                ViewState["ItemUpdateIndex"] = "-1";
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_DESC");


                ((DataTable)ViewState["dt2"]).Columns.Add("IND_SPECIFICATION");

                ((DataTable)ViewState["dt2"]).Columns.Add("IND_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_AMT");
                //((DataTable)ViewState["dt2"]).Columns.Add("IWD_RATE");
                //((DataTable)ViewState["dt2"]).Columns.Add("IWD_CH_QTY");
                //((DataTable)ViewState["dt2"]).Columns.Add("IWD_REV_QTY");
                //((DataTable)ViewState["dt2"]).Columns.Add("IWD_REMARK");
                //((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_NAME");
                //((DataTable)ViewState["dt2"]).Columns.Add("IWD_UOM_CODE");
                //((DataTable)ViewState["dt2"]).Columns.Add("IWD_BATCH_NO");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IND_DESC"] = txtDesc.Text.Trim().Replace("'", "\'");
            dr["IND_SPECIFICATION"] = ddlItem.SelectedValue;
            dr["IND_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtQty.Text)));
            dr["IND_RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text)));
            dr["IND_AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtAmount.Text)));

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtDesc.Text = "";
                    txtRate.Text = "";
                    txtAmount.Text = "";
                    txtQty.Text = "";
                    ddlItem.SelectedValue = "0";

                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtDesc.Text = "";
                txtRate.Text = "";
                txtAmount.Text = "";
                txtQty.Text = "";
                ddlItem.SelectedValue = "0";
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            GetTotal();
            #endregion
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
        }

    }
    #endregion

    #region
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        #region datatable structure
        if (((DataTable)ViewState["dt3"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt3"]).Columns.Add("IFU_CODE");
            ((DataTable)ViewState["dt3"]).Columns.Add("IFU_FILE");


            ((DataTable)ViewState["dt3"]).Columns.Add("IFU_DESC");
            ((DataTable)ViewState["dt3"]).Columns.Add("IFU_APPROVE");
        }
        #endregion
        //if (dgfileupload.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dgfileupload.Rows.Count; i++)
        //    {
        //        string ITEM_CODE = ((Label)(dgfileupload.Rows[i].FindControl("lblIFU_FILE"))).Text;
        //        if (ViewState["ItemUpdateIndex1"].ToString() == "-1")
        //        {
        //            if (ITEM_CODE == lnkFile.Text.ToString())
        //            {
        //                PanelMsg.Visible = true;
        //                lblmsg.Text = "Record Already Exist For This File In Table";
        //                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            if (ITEM_CODE == lnkFile.Text.ToString()&& ViewState["ItemUpdateIndex1"].ToString() != i.ToString())
        //            {
        //                //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
        //                PanelMsg.Visible = true;
        //                lblmsg.Text = "Record Already Exist For This Item In Table";
        //                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        //                return;
        //            }
        //        }
        //    }
        //    ViewState["ItemUpdateIndex1"] = "-1";
        //}

        #region  add control value to Dt
        dr = ((DataTable)ViewState["dt3"]).NewRow();
        if (ViewState["fileName3"] == "" || ViewState["fileName3"] == null)
            ViewState["fileName3"] = DBNull.Value;
        dr["IFU_FILE"] = ViewState["fileName3"].ToString();
        dr["IFU_DESC"] = txtFileDesc.Text;
        #endregion

        #region check Data table,insert or Modify Data
        if (ViewState["str1"].ToString() == "Modify")
        {
            if (((DataTable)ViewState["dt3"]).Rows.Count > 0)
            {
                ((DataTable)ViewState["dt3"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index1"].ToString()));
                ((DataTable)ViewState["dt3"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index1"].ToString()));
            }
        }
        else
        {
            ((DataTable)ViewState["dt3"]).Rows.Add(dr);
        }
        #endregion

        #region Binding data to Grid
        dgfileupload.Visible = true;
        dgfileupload.DataSource = ((DataTable)ViewState["dt3"]);
        dgfileupload.DataBind();
        dgfileupload.Enabled = true;
        ViewState["str1"] = "";
        ViewState["ItemUpdateIndex1"] = "-1";
        #endregion

        #region Clear Control
        txtFileDesc.Text = "";
        lnkFile.Text = "";
        #endregion
    }
    #endregion

    #region loadschedle
    //public void loadschedle(string str)
    //{
    //    if (txtInvoiceNo.Text == "1" || txtInvoiceNo.Text.Trim().ToUpper() == "TRUE")
    //    {
    //        DataTable dtSche = new DataTable();
    //        DataTable dtInward = new DataTable();
    //        if (str != "-2147483616")
    //        {
    //            dtSche = CommonClasses.Execute("SELECT  ISNULL(SUM(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY),0) AS ScheduleQty  FROM PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE WHERE     (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0)   AND   datepart(MM,PSM_SCHEDULE_MONTH) = '" + Convert.ToDateTime(txtGRNDate.Text).Month + "' AND   datepart(YYYY,PSM_SCHEDULE_MONTH) = '" + Convert.ToDateTime(txtGRNDate.Text).Year + "' AND (PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = '" + ddlIndentType.SelectedValue + "') AND PSD_I_CODE='" + ddlItemCode.SelectedValue + "'  ");

    //            dtInward = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0) AS  IWD_REV_QTY ,ISNULL(SUM(IWD_CON_OK_QTY),0) AS IWD_CON_OK_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND    (INWARD_MASTER.ES_DELETE = 0)   AND   datepart(MM,IWM_DATE) = '" + Convert.ToDateTime(txtGRNDate.Text).Month + "' AND   datepart(YYYY,IWM_DATE) = '" + Convert.ToDateTime(txtGRNDate.Text).Year + "' AND (IWM_P_CODE = '" + ddlIndentType.SelectedValue + "') AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "'  ");

    //        }
    //        else
    //        {
    //            DataTable dtCompany = CommonClasses.Execute(" select CM_ADDRESS3 from COMPANY_MASTER where  CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ");

    //            if (dtCompany.Rows[0]["CM_ADDRESS3"].ToString().ToUpper() == "YES")
    //            {
    //                dtSche = CommonClasses.Execute("SELECT  ISNULL( SUM(DCSD_QTY),0) AS ScheduleQty  FROM DAILY_CORE_SCHEDULE_MASTER INNER JOIN DAILY_CORE_SCHEDULE_DETAIL ON DCSM_CODE = DCSD_DCSM_CODE WHERE (DAILY_CORE_SCHEDULE_MASTER.ES_DELETE = 0)   AND    DCSM_SCHEDULE_DATE  = '" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "'  AND (DCSM_P_CODE = '" + ddlIndentType.SelectedValue + "') AND DCSD_I_CODE='" + ddlItemCode.SelectedValue + "'  ");

    //                dtInward = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0) AS  IWD_REV_QTY ,ISNULL(SUM(IWD_CON_OK_QTY),0) AS IWD_CON_OK_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND    (INWARD_MASTER.ES_DELETE = 0)   AND     IWM_DATE  = '" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "' AND    (IWM_P_CODE = '" + ddlIndentType.SelectedValue + "') AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "'  ");

    //            }
    //            else
    //            {
    //                dtSche = CommonClasses.Execute("SELECT  ISNULL(SUM(CSD_MONTH_QTY) ,0) AS ScheduleQty  FROM CORE_SCHEDULE_MASTER INNER JOIN CORE_SCHEDULE_DETAIL ON CSM_CODE = CSD_CSM_CODE WHERE (CORE_SCHEDULE_MASTER.ES_DELETE = 0)   AND   datepart(MM,CSM_SCHEDULE_MONTH) = '" + Convert.ToDateTime(txtGRNDate.Text).Month + "' AND   datepart(YYYY,CSM_SCHEDULE_MONTH) = '" + Convert.ToDateTime(txtGRNDate.Text).Year + "' AND (CSM_P_CODE = '" + ddlIndentType.SelectedValue + "') AND CSD_I_CODE='" + ddlItemCode.SelectedValue + "'  ");

    //                dtInward = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0) AS  IWD_REV_QTY ,ISNULL(SUM(IWD_CON_OK_QTY),0) AS IWD_CON_OK_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND    (INWARD_MASTER.ES_DELETE = 0)   AND   datepart(MM,IWM_DATE) = '" + Convert.ToDateTime(txtGRNDate.Text).Month + "' AND   datepart(YYYY,IWM_DATE) = '" + Convert.ToDateTime(txtGRNDate.Text).Year + "' AND (IWM_P_CODE = '" + ddlIndentType.SelectedValue + "') AND IWD_I_CODE='" + ddlItemCode.SelectedValue + "'  ");

    //            }

    //        }

    //        txtSchdeuleQTy.Text = (Convert.ToDouble(dtSche.Rows[0]["ScheduleQty"].ToString()) - Convert.ToDouble(dtInward.Rows[0]["IWD_REV_QTY"].ToString())).ToString();
    //    }
    //}
    #endregion loadschedle

    #region ddlItemName_SelectedIndexChanged
    //protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlItemCode.SelectedIndex != 0)
    //    {
    //        ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
    //        DataTable dtitem = new DataTable();
    //        dtitem = CommonClasses.Execute("SELECT * FROM ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");

    //       // if (dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483613" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483619" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483635" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483616")
    //        if (dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483613" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483619" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483635"  )
    //        {

    //            loadschedle(dtitem.Rows[0]["I_CAT_CODE"].ToString());
    //        }
    //        else
    //        {
    //            txtSchdeuleQTy.Text = "NA";
    //        }
    //        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
    //        if (dt1.Rows.Count > 0)
    //        {
    //            ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
    //        }
    //        else
    //        {
    //            ddlUom.Text = "";
    //        }
    //    }
    //    txtChallanQty.Text = "";
    //    txtRecdQty.Text = "";
    //    pendingQty();
    //}
    #endregion

    #region ddlItemName_SelectedIndexChanged
    //protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
    //    if (ddlItemName.SelectedIndex != 0)
    //    {
    //        ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
    //        DataTable dtitem = new DataTable();
    //        dtitem = CommonClasses.Execute("SELECT * FROM ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");

    //        //if (dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483613" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483619" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483635" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483616")
    //        if (dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483613" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483619" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647" || dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483635" )
    //        {
    //            loadschedle(dtitem.Rows[0]["I_CAT_CODE"].ToString());
    //        }
    //        else
    //        {
    //            txtSchdeuleQTy.Text = "NA";
    //        }
    //        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
    //        if (dt1.Rows.Count > 0)
    //        {
    //            ddlUom.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
    //        }
    //        else
    //        {
    //            ddlUom.Text = "";
    //        }
    //        txtChallanQty.Text = "";
    //        txtRecdQty.Text = "";
    //        pendingQty();
    //    }
    //}
    #endregion

    #region ddlPoNumber_SelectedIndexChanged
    //protected void ddlPoNumber_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtChallanQty.Text = "";
    //    txtRecdQty.Text = "";
    //    try
    //    {
    //        dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY,  SPOD_RATE - ROUND((SPOD_DISC_AMT/SPOD_ORDER_QTY),2)  AS SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlIndentType.SelectedValue + "' AND  SPOM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
    //        if (dt.Rows.Count > 0)
    //        {
    //            if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
    //            {
    //                txtChallanQty.Enabled = false;
    //                txtRecdQty.Enabled = false;
    //                PanelMsg.Visible = true;
    //                lblmsg.Text = "Only Post PO Can InWard";
    //            }
    //            else
    //            {
    //                PanelMsg.Visible = false;
    //                txtChallanQty.Enabled = true;
    //                txtRecdQty.Enabled = true;
    //            }
    //            txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
    //            txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["SPOD_RATE"].ToString()));
    //            ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();
    //        }
    //        else
    //        {
    //            txtPendingQty.Text = Convert.ToInt32(0).ToString();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "btnInsert_Click", Ex.Message);
    //    }
    //}
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    //protected void ddlIndentType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    #region FillItems
    //    try
    //    {
    //        ((DataTable)ViewState["dt2"]).Rows.Clear();
    //        BlankGrid();
    //        int id = Convert.ToInt32(ddlIndentType.SelectedValue);
    //        ddlItemName.Items.Clear();
    //        ddlItemCode.Items.Clear();
    //        txtSchdeuleQTy.Text = "";
    //        DataTable dtpart = CommonClasses.Execute("SELECT * FROM PARTY_MASTER WHERE P_CODE='" + id + "'");
    //        if (dtpart.Rows.Count > 0)
    //        {
    //            txtInvoiceNo.Text = dtpart.Rows[0]["P_INHOUSE_IND"].ToString();
    //        }
    //        DataTable dtItem = new DataTable();
    //        dtItem = CommonClasses.Execute("SELECT DISTINCT I_NAME,I_CODE,I_CODENO FROM ITEM_MASTER,SUPP_PO_DETAILS,SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 and I_CODE=SPOD_I_CODE and SPOM_CANCEL_PO=0 and SPOM_POST=1  AND SPOM_IS_SHORT_CLOSE=0 AND SPOM_POTYPE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY)  and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "'    AND SPOM_POTYPE=0   AND  SPOM_POST=1 AND SPOM_P_CODE=" + id + " ORDER BY I_NAME");
    //        ddlItemName.DataSource = dtItem;
    //        ddlItemName.DataTextField = "I_NAME";
    //        ddlItemName.DataValueField = "I_CODE";
    //        ddlItemName.DataBind();
    //        ddlItemCode.DataSource = dtItem;
    //        ddlItemCode.DataTextField = "I_CODENO";
    //        ddlItemCode.DataValueField = "I_CODE";
    //        ddlItemCode.DataBind();

    //        ddlItemName.Items.Insert(0, new ListItem("Please Select Item ", "0"));
    //        ddlItemCode.Items.Insert(0, new ListItem("Please Select Item", "0"));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "ddlSupplier_SelectedIndexChanged", Ex.Message);
    //    }
    //    #endregion
    //}
    #endregion

    #region pendingQty
    //private void pendingQty()
    //{
    //    #region PendingQty
    //    try
    //    {
    //        ddlPoNumber.Items.Clear();
    //        if (Request.QueryString[0].Equals("MODIFY"))
    //        {
    //            dtPO = CommonClasses.Execute("select DISTINCT SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'') as SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,ITEM_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SPOM_PROJECT AND  SPOM_CODE=SPOD_SPOM_CODE AND SPOM_POTYPE=0 AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlIndentType.SelectedValue + "' AND SPOD_I_CODE=I_CODE AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND  SPOM_CM_CODE=CM_CODE AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   order by SPOM_CODE  ");
    //        }
    //        else
    //        {
    //            dtPO = CommonClasses.Execute("select DISTINCT SPOM_CODE,Convert(varchar,SPOM_PONO) + '/' + Right(Convert(varchar,Year(CM_OPENING_DATE)),2)+ '-' + Right(Convert(varchar,Year(CM_CLOSING_DATE)),2) +' -'+ISNULL(PROCM_NAME,'') as SPOM_PO_NO FROM SUPP_PO_MASTER,SUPP_PO_DETAILS,PROJECT_CODE_MASTER,ITEM_MASTER,COMPANY_MASTER WHERE PROCM_CODE=SPOM_PROJECT AND  SPOM_CODE=SPOD_SPOM_CODE  AND SPOM_POTYPE=0 AND SPOM_POST=1 AND SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0 and  SPOM_P_CODE='" + ddlIndentType.SelectedValue + "' AND SPOD_I_CODE=I_CODE AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' AND SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND   SPOM_CM_CODE=CM_CODE AND  SUPP_PO_MASTER.ES_DELETE=0 and SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND (SPOD_INW_QTY) < (SPOD_ORDER_QTY) order by SPOM_CODE  ");
    //        }
    //        ddlPoNumber.DataSource = dtPO;
    //        ddlPoNumber.DataTextField = "SPOM_PO_NO";
    //        ddlPoNumber.DataValueField = "SPOM_CODE";
    //        ddlPoNumber.DataBind();
    //        dt = CommonClasses.Execute("select (ISNULL(SPOD_ORDER_QTY,0)-ISNULL(SPOD_INW_QTY,0)) as PENDQTY, SPOD_RATE - ROUND((SPOD_DISC_AMT/SPOD_ORDER_QTY),2) AS SPOD_RATE,SPOD_UOM_CODE,SPOM_POST from SUPP_PO_MASTER,SUPP_PO_DETAILS where SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=SPOD_SPOM_CODE AND SPOM_P_CODE='" + ddlIndentType.SelectedValue + "' AND SPOM_POTYPE=0   AND  SPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND SPOD_I_CODE='" + ddlItemName.SelectedValue + "' and SPOM_CODE='" + ddlPoNumber.SelectedValue + "'");
    //        if (dt.Rows.Count > 0)
    //        {
    //            if (dt.Rows[0]["SPOM_POST"].ToString() == "False")
    //            {
    //                txtChallanQty.Enabled = false;
    //                txtRecdQty.Enabled = false;
    //                PanelMsg.Visible = true;
    //                lblmsg.Text = "Only Post PO Can InWard";
    //            }
    //            txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dt.Rows[0]["PENDQTY"].ToString()));
    //            txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(dt.Rows[0]["SPOD_RATE"].ToString()));
    //            ddlUom.SelectedValue = dt.Rows[0]["SPOD_UOM_CODE"].ToString();
    //        }
    //        else
    //        {
    //            txtPendingQty.Text = Convert.ToInt32(0).ToString();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Material Inward", "PendingQty", Ex.Message);
    //    }
    //    #endregion
    //}
    #endregion

    #region CtlDisable
    //public void CtlDisable()
    //{
    //    // ddlSupplier.Enabled = false;
    //    txtGRNno.Enabled = false;
    //    txtChallanNo.Enabled = false;
    //    txtEgpNo.Enabled = false;
    //    txtLrNo.Enabled = false;
    //    txtOctNo.Enabled = false;
    //    txtVehno.Enabled = false;
    //    txtChallanDate.Enabled = false;
    //    txtGRNDate.Enabled = false;
    //    BtnInsert.Visible = false;
    //    btnSubmit.Visible = false;
    //}
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
            }
            else
            {
                if (CheckValid())
                {
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
            CommonClasses.SendError("Indent Detail", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    protected void lnkcheque_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (lnkcheque.Text != "")
                {
                    filePath = lnkcheque.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/InFILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkcheque.Text != "")
                {
                    filePath = lnkcheque.Text;
                }

                directory = "../../UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Detail", "lnkcheque_Click", ex.Message);
        }
    }

    protected void lnkGST_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {

                if (lnkGST.Text != "")
                {
                    filePath = lnkGST.Text;
                }

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/InFILEUPLOAD/" + filePath;
            }
            else
            {
                if (lnkGST.Text != "")
                {
                    filePath = lnkGST.Text;
                }

                directory = "../../UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Detail", "lnkGST_Click", ex.Message);
        }
    }

    protected void lnkCertificate_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {

                filePath = lnkCertificate.Text;

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/InFILEUPLOAD/" + filePath;
            }
            else
            {

                filePath = lnkCertificate.Text;

                directory = "../../UpLoadPath/IndentFile/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Detail", "lnkCertificate_Click", ex.Message);
        }
    }

    protected void lnkFile_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {

                filePath = lnkFile.Text;

                //filePath = lnkQuotation.Text;
                directory = "../../UpLoadPath/InFILEUPLOAD/" + filePath;
            }
            else
            {

                filePath = lnkFile.Text;

                directory = "../../UpLoadPath/IndentFileMulti/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Detail", "lnkFile_Click", ex.Message);
        }
    }


    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("INDENT_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Transactions/VIEW/IndentDetail.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Detail", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            //if (ddlSupplier.Text == "")
            //{
            //    flag = false;
            //}
            if (txtSuppName.Text == "")
            {
                flag = false;
            }
            else if (txtContactNo.Text == "")
            {
                flag = false;
            }
            else if (txtAddress.Text == "")
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

    #region  txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double rate = Convert.ToDouble(txtRate.Text);
            double qty = Convert.ToDouble(txtQty.Text);

            double amount = qty * rate;
            txtAmount.Text = Convert.ToDouble(amount).ToString();

        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Indent Detail", "txtRate_TextChanged", Ex.Message);
        }
    }
    #endregion txtChallanQty_TextChanged

    #region txtQty_TextChanged
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        txtRate.Enabled = true;
        //try
        //{

        //    double qty = Convert.ToDouble(txtQty.Text);
        //    double rate = Convert.ToDouble(txtRate.Text);


        //    double amount = qty * rate;
        //    txtAmount.Text = Convert.ToDouble(amount).ToString();

        //}
        //catch (Exception Ex)
        //{

        //    CommonClasses.SendError("Indent Detail", "txtQty_TextChanged", Ex.Message);
        //}
    }
    #endregion

    #region txtRecdQty_TextChanged
    //protected void txtRecdQty_TextChanged(object sender, EventArgs e)
    //{
    //    string totalStr = DecimalMasking(txtRecdQty.Text);
    //    txtRecdQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    //    DataTable dtApproved = CommonClasses.Execute("select P_CODE,P_NAME,P_INHOUSE_IND from PARTY_MASTER where ES_DELETE=0 and P_TYPE=2 and P_CM_COMP_ID=1 AND P_CODE='" + ddlIndentType.SelectedValue + "'");
    //    if (dtApproved.Rows.Count > 0)
    //    {
    //        if (dtApproved.Rows[0]["P_INHOUSE_IND"].ToString().ToUpper() == "TRUE")
    //        {
    //            if (txtSchdeuleQTy.Text != "NA")
    //            {
    //                if (Convert.ToDouble(txtRecdQty.Text) <= Convert.ToDouble(txtSchdeuleQTy.Text))
    //                {
    //                }
    //                else
    //                {
    //                    PanelMsg.Visible = true;
    //                    lblmsg.Text = "Recieved Qty Should be Less than or equal to Schedule Qty..";
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    //                    txtChallanQty.Focus();
    //                    txtRecdQty.Text = "0.000";
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //}
    #endregion txtRecdQty_TextChanged

    #region DecimalMasking
    //protected string DecimalMasking(string Text)
    //{
    //    string result1 = "";
    //    string totalStr = "";
    //    string result2 = "";
    //    string s = Text;
    //    string result = s.Substring(0, (s.IndexOf(".") + 1));
    //    int no = s.Length - result.Length;
    //    if (no != 0)
    //    {
    //        if (no > 10)
    //        {
    //            no = 10;
    //        }
    //        result1 = s.Substring((result.IndexOf(".") + 1), no);

    //        try
    //        {
    //            result2 = result1.Substring(0, result1.IndexOf("."));
    //        }
    //        catch { }

    //        string result3 = s.Substring((s.IndexOf(".") + 1), 1);
    //        if (result3 == ".")
    //        {
    //            result1 = "00";
    //        }
    //        if (result2 != "")
    //        {
    //            totalStr = result + result2;
    //        }
    //        else
    //        {
    //            totalStr = result + result1;
    //        }
    //    }
    //    else
    //    {
    //        result1 = "00";
    //        totalStr = result + result1;
    //    }
    //    return totalStr;
    //}
    #endregion

    //protected void txtChallanNo_TextChanged(object sender, EventArgs e)
    //{
    //    challandetail();
    //}

    #region challandetail
    //public void challandetail()
    //{
    //    if (txtChallanNo.Text != "" && txtChallanNo.Text != "0" && txtChallanDate.Text != "")
    //    {//ddlSupplier.SelectedValue != "0" 
    //        DataTable dtChallan = new DataTable();
    //        if (Request.QueryString[0].Equals("INSERT"))
    //        {
    //            dtChallan = CommonClasses.Execute("select * from INWARD_MASTER where IWM_CHALLAN_NO='" + txtChallanNo.Text.Trim() + "' and IWM_P_CODE='" + ddlIndentType.SelectedValue + "' and ES_DELETE=0 and IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + "  ");
    //        }
    //        else
    //        {
    //            dtChallan = CommonClasses.Execute("select * from INWARD_MASTER where IWM_CHALLAN_NO='" + txtChallanNo.Text.Trim() + "' and IWM_P_CODE='" + ddlIndentType.SelectedValue + "' and ES_DELETE=0 and IWM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + "   AND IWM_CODE!='" + ViewState["mlCode"].ToString() + "'");
    //        }
    //        if (dtChallan.Rows.Count > 0)
    //        {
    //            PanelMsg.Visible = true;
    //            lblmsg.Text = "Challan No. Is Exist for this party";
    //            txtChallanNo.Text = "";
    //            txtChallanNo.Focus();
    //        }
    //        else
    //        {
    //            PanelMsg.Visible = false;
    //            lblmsg.Text = "";
    //        }
    //    }
    //}
    #endregion challandetail
}
