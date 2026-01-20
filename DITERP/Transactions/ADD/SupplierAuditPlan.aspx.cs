using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class Transactions_ADD_SupplierAuditPlan : System.Web.UI.Page
{
    # region Variables
    DirectoryInfo ObjSearchDir;
    string fileName = "";
    string fileName1 = "";
    static int mlCode = 0;
    static string right = "";
    # endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                    try
                    {
                        txtDocDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                        txtAuditdate.Text = System.DateTime.Now.ToString("MMM yyyy");
                        ViewState["mlCode"] = mlCode;
                        LoadSupplier();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            act.Visible = false;
                            AName.Visible = false;
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("UpdateStatus"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("UPDATE");
                        }
                        else
                        {
                            act.Visible = false;
                            AName.Visible = false;

                            txtDocDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtAuditdate.Text = System.DateTime.Now.ToString("MMM yyyy");
                        }
                        txtDocDate.Attributes.Add("readonly", "readonly");
                        txtAuditdate.Attributes.Add("readonly", "readonly");
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Supplier Audit Plan", "Page_Load", ex.Message);
                    }
                }
            }
            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                if (FileUpload1.PostedFile.FileName.Length > 0)
                {
                    fileName = FileUpload1.PostedFile.FileName;
                    ViewState["fileName"] = fileName.Trim();
                    lnkupload.Text = ViewState["fileName"].ToString();
                    Upload(null, null);
                }
            }
            else
            {
                ViewState["fileName"] = "";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region LoadSupplier
    //for load tool 
    public void LoadSupplier()
    {
        DataTable Dt = new DataTable();
        string str = "";

        Dt = CommonClasses.Execute("select P_CODE,P_NAME From PARTY_MASTER where P_INHOUSE_IND=1 and ES_DELETE=0 and P_CM_COMP_ID='" + Session["CompanyId"] + "' and P_ACTIVE_IND=1 and P_TYPE=2 order by P_NAME");

        if (Dt.Rows.Count > 0)
        {
            ddlSupplier.DataSource = Dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier ", "0"));
        }
        else
        {
            ddlSupplier.DataSource = Dt;
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier ", "0"));
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
            CommonClasses.SendError("Supplier Audit Plan", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtDocDate.Text.Trim() == "")
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
            CommonClasses.SendError("Supplier Audit Plan", "CheckValid", Ex.Message);
        }
        return flag;
    }

    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("SUPPLIER_AUDIT_PLAN", "MODIFY", "SAP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/Transactions/VIEW/ViewSupplierAuditPlan.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlSupplier.SelectedValue == "0")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Supplier";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlSupplier.Focus();
            return;
        }
        if (txtAuditorName.Text == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Auditor Name";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtAuditorName.Focus();
            return;
        }
        if (Request.QueryString[0].Equals("UpdateStatus"))
        {
            if (txtAction.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Action";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtAction.Focus();
                return;
            }
            if (lnkupload.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Upload File";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                lnkupload.Focus();
                return;
            }
        }
        SaveRec();
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            // dt = CommonClasses.Execute("SELECT B_CODE, SAP_P_CODE,B_I_CODE, SAP_DOC_NO,B_SLIPNO, SAP_DOC_DATE, B_REASON, SAP_ACTION, SAP_AUDIT_DATE, B_HOURS, SAP_FILE,B_DRAWING_UPLOAD,T_NAME,CASE WHEN T_TYPE=1 then '1' Else '0' END  AS T_TYPE, I_CODENO +' - '+ I_NAME AS I_CODENO FROM SUPPLIER_AUDIT_PLAN INNER JOIN TOOL_MASTER ON SUPPLIER_AUDIT_PLAN.SAP_P_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON SUPPLIER_AUDIT_PLAN.B_I_CODE = ITEM_MASTER.I_CODE WHERE (SUPPLIER_AUDIT_PLAN.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0)  AND B_CODE=" + ViewState["mlCode"].ToString() + " AND B_CM_ID= " + (string)Session["CompanyId"] + "  ");
            dt = CommonClasses.Execute("SELECT SAP_CODE,SAP_DOC_NO,SAP_CM_CODE,SAP_DOC_DATE,SAP_P_CODE,SAP_AUDIT_DATE,SAP_AUDITOR_NAME,SAP_ACTION,SAP_FILE from SUPPLIER_AUDIT_PLAN WHERE (SUPPLIER_AUDIT_PLAN.ES_DELETE = 0) AND SAP_CODE=" + ViewState["mlCode"].ToString() + "");
            if (dt.Rows.Count > 0)
            {
                txtDocNo.Text = dt.Rows[0]["SAP_DOC_NO"].ToString();
                LoadSupplier();
                ddlSupplier.SelectedValue = dt.Rows[0]["SAP_P_CODE"].ToString();
                txtAction.Text = dt.Rows[0]["SAP_ACTION"].ToString();
                txtDocDate.Text = Convert.ToDateTime(dt.Rows[0]["SAP_DOC_DATE"]).ToString("dd/MMM/yyyy");
                txtAuditorName.Text = dt.Rows[0]["SAP_AUDITOR_NAME"].ToString();
                txtAuditdate.Text = Convert.ToDateTime(dt.Rows[0]["SAP_AUDIT_DATE"]).ToString("MMM yyyy");
                lnkupload.Text = dt.Rows[0]["SAP_FILE"].ToString();
                if (str == "VIEW")
                {
                    DataTable dtStatus = CommonClasses.Execute("select SAP_CODE,SAP_STATUS from SUPPLIER_AUDIT_PLAN where SAP_STATUS=1 and ES_DELETE=0 AND SAP_CODE=" + ViewState["mlCode"].ToString() + "");
                    if (dtStatus.Rows.Count > 0)
                    {
                        if (dtStatus.Rows[0]["SAP_STATUS"].ToString() == "True")
                        {
                            // txtAction.Visible = true;
                            // FileUpload1.Visible = true;
                        }
                    }
                    else
                    {
                        lblAction.Visible = false;
                        lblAttach.Visible = false;
                        txtAction.Visible = false;
                        FileUpload1.Visible = false;
                    }
                    ddlSupplier.Enabled = false;
                    txtAction.Enabled = false;
                    txtAuditdate.Enabled = false;
                    txtDocDate.Enabled = false;
                    btnSubmit.Visible = false;
                    FileUpload1.Enabled = false;
                    txtAuditorName.Enabled = false;
                }
                if (str == "UPDATE")
                {
                    txtAuditdate.Text = System.DateTime.Now.ToString("MMM yyyy");
                    txtAuditdate.Enabled = false;
                    txtAuditorName.Enabled = false;
                    ddlSupplier.Enabled = false;
                    // txtAction.Enabled = false;
                    txtDocDate.Enabled = false;
                    CommonClasses.SetModifyLock("SUPPLIER_AUDIT_PLAN", "MODIFY", "SAP_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    // btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SUPPLIER_AUDIT_PLAN", "MODIFY", "SAP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        PanelMsg.Visible = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt1 = new DataTable();
                dt1 = CommonClasses.Execute("SELECT ISNULL(MAX(SAP_DOC_NO),0)+1 AS SAP_DOC_NO FROM SUPPLIER_AUDIT_PLAN WHERE SAP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"].ToString()) + "' AND ES_DELETE='False'");
                if (dt1.Rows.Count > 0)
                {
                    txtDocNo.Text = dt1.Rows[0][0].ToString();
                }
                if (CommonClasses.Execute1("INSERT INTO SUPPLIER_AUDIT_PLAN (SAP_DOC_NO,SAP_CM_CODE,SAP_DOC_DATE,SAP_P_CODE,SAP_AUDIT_DATE,SAP_AUDITOR_NAME) VALUES('" + txtDocNo.Text + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToDateTime(txtDocDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "','" + Convert.ToDateTime(txtAuditdate.Text).ToString("MMM yyyy") + "','" + txtAuditorName.Text.Trim().Replace("'", "\''") + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(SAP_CODE) from SUPPLIER_AUDIT_PLAN");
                    if (ViewState["fileName"].ToString().Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/SuppAudiPlan/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/SuppAudiPlan");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\SuppAudiPlan";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/SuppAudiPlan/" + ViewState["fileName"].ToString().Trim());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/SuppAudiPlan/" + Code + "/" + ViewState["fileName"].ToString().Trim());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    CommonClasses.WriteLog("Supplier Audit Plan", "Save", "Supplier Audit Plan", txtDocNo.Text, Convert.ToInt32(txtDocNo.Text), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Transactions/View/ViewSupplierAuditPlan.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtDocNo.Focus();
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();

                if (CommonClasses.Execute1("UPDATE SUPPLIER_AUDIT_PLAN SET SAP_P_CODE='" + ddlSupplier.SelectedValue + "',SAP_DOC_DATE='" + Convert.ToDateTime(txtDocDate.Text).ToString("dd/MMM/yyyy") + "',SAP_AUDITOR_NAME='" + txtAuditorName.Text + "',SAP_ACTION='" + txtAction.Text.Trim().Replace("'", "\''") + "',SAP_AUDIT_DATE='" + Convert.ToDateTime(txtAuditdate.Text).ToString("MMM yyyy") + "',SAP_FILE='" + lnkupload.Text + "' WHERE SAP_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("SUPPLIER_AUDIT_PLAN", "MODIFY", "SAP_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("Supplier Audit Plan", "Update", "Supplier Audit Plan", txtDocNo.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Transactions/View/ViewSupplierAuditPlan.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtDocNo.Focus();
                }
            }
            #endregion MODIFY

            #region UpdateStatus
            else if (Request.QueryString[0].Equals("UpdateStatus"))
            {
                // 17122018 :- Add SAP_COMPLETED_DATE add update with current date time ...
                if (CommonClasses.Execute1("UPDATE SUPPLIER_AUDIT_PLAN SET SAP_ACTION='" + txtAction.Text.Trim().Replace("'", "\''") + "',SAP_FILE='" + lnkupload.Text + "',SAP_STATUS=1,SAP_COMPLETED_DATE='" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "' WHERE SAP_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("SUPPLIER_AUDIT_PLAN", "MODIFY", "SAP_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("Supplier Audit Plan", "UpdateStatus", "Supplier Audit Plan", txtDocNo.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Transactions/View/ViewSupplierAuditPlan.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtDocNo.Focus();
                }

            }
            #endregion UpdateStatus
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        return res;
    }
    #endregion Setvalues

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        return res;
    }
    #endregion GetValues

    #region GetIP4Address
    string GetIP4Address()
    {
        string IP4Address = String.Empty;
        try
        {
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "GetIP4Address", ex.Message);
            return IP4Address;
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
            CommonClasses.SendError("Supplier Audit Plan", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppAudiPlan/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppAudiPlan/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/SuppAudiPlan/" + ViewState["fileName"].ToString()));
        }
        else
        {
            FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/SuppAudiPlan/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
    }
    #endregion

    #region lnkupload_Click
    protected void lnkupload_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string code = "";
            string directory = "";
            // int height = Convert.ToInt32(FileUpload1.Height);

            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/SuppAudiPlan/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/SuppAudiPlan/" + code + "/" + filePath;

            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit Plan", "lnkupload_Click", ex.Message);
        }
    }
    #endregion

    #region txtAuditdate_TextChanged
    protected void txtAuditdate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtDate1 = Convert.ToDateTime(txtAuditdate.Text);
        DateTime dtDate = Convert.ToDateTime(txtDocDate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtAuditdate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year...";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtDocDate.Focus();
            return;
        }
    }
    #endregion txtAuditdate_TextChanged

    #region txtDocDate_TextChanged
    protected void txtDocDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtDate1 = Convert.ToDateTime(txtDocDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtAuditdate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtDocDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year...";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtDocDate.Focus();
            return;
        }
    }
    #endregion txtDocDate_TextChanged

}

