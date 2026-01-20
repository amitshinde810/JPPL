using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class Transactions_ADD_SupplierAudit : System.Web.UI.Page
{
    #region Declartion
    DirectoryInfo ObjSearchDir;
    static int mlCode = 0;
    static string right = "";
    int CValue;
    int SValue;
    string fileName = "";
    #endregion

    #region Events

    #region Page_Load
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
                try
                {
                    txtFromDate.Text = System.DateTime.Now.ToString("MMM/yyyy");
                    LoadCust();
                    ViewState["mlCode"] = mlCode;
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
                    txtFromDate.Attributes.Add("readonly", "readonly");
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Supplier Audit", "PageLoad", ex.Message);
                }
            }
            if (IsPostBack && FUDrawingTR.PostedFile != null)
            {
                if (FUDrawingTR.PostedFile.FileName.Length > 0)
                {
                    fileName = FUDrawingTR.PostedFile.FileName;
                    ViewState["fileName"] = fileName.Trim();
                    lnkFUDrawingTR.Text = ViewState["fileName"].ToString();
                    Upload1(null, null);
                }
            }
            else
            {
                ViewState["fileName2"] = "";
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validation
            if (txtFromDate.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Month";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Focus();
                return;
            }
            if (ddlSupplier.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Supplier";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlSupplier.Focus();
                return;
            }
            if (txtAuditScore.Text.Trim() == "" || txtAuditScore.Text.Trim() == "0.00" || txtAuditScore.Text.Trim() == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Audit Score";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtAuditScore.Focus();
                return;
            }
            if (txtCustLineDiscruption.Text.Trim() == "" || txtCustLineDiscruption.Text.Trim() == "0.00" || txtCustLineDiscruption.Text.Trim() == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Customer Line Disruption";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtCustLineDiscruption.Focus();
                return;
            }
            if (txtCustComplaint.Text.Trim() == "" || txtCustComplaint.Text.Trim() == "0.00" || txtCustComplaint.Text.Trim() == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Customer Complaint";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtCustLineDiscruption.Focus();
                return;
            }
            #endregion Validation

            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE FROM SUPPLIER_AUDIT_MASTER WHERE SUPPLIER_AUDIT_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + "");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["SAM_P_CODE"].ToString() == ddlSupplier.SelectedValue.ToString() && Convert.ToDateTime(dt.Rows[0]["SAM_FDATE"]).ToString("MMM yyyy") == Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy"))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Once entered supplier in perticuar Month can not be enter again";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlSupplier.Focus();
                        return;
                    }
                }
            }

            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit", "btnSubmit_Click", Ex.Message);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_FILE,SAM_P_CODE,SAM_QUALITY,SAM_DELIVERY,ISNULL(SAM_COST,0) AS SAM_COST,SAM_TOTAL,SAM_RESPONSE,SAM_BUYER_REMARK,SAM_QA_REMARK From SUPPLIER_AUDIT_MASTER where SUPPLIER_AUDIT_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " AND SAM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["SAM_CODE"]); ;
                LoadCust();
                txtFromDate.Text = Convert.ToDateTime(dt.Rows[0]["SAM_FDATE"]).ToString("MMM yyyy");
                ddlSupplier.SelectedValue = dt.Rows[0]["SAM_P_CODE"].ToString();
                txtAuditScore.Text = dt.Rows[0]["SAM_QUALITY"].ToString();
                txtCustLineDiscruption.Text = dt.Rows[0]["SAM_DELIVERY"].ToString();
                txtCustComplaint.Text = dt.Rows[0]["SAM_COST"].ToString();
                lnkFUDrawingTR.Text = dt.Rows[0]["SAM_FILE"].ToString();
                if (str == "VIEW")
                {
                    ddlSupplier.Enabled = false;
                    FUDrawingTR.Enabled = false;
                    txtCustComplaint.Enabled = false;
                    txtFromDate.Enabled = false;
                    txtAuditScore.Enabled = false;
                    txtCustLineDiscruption.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    ddlSupplier.Enabled = false;
                    txtFromDate.Enabled = false;
                    CommonClasses.SetModifyLock("SUPPLIER_AUDIT_MASTER", "ES_MODIFY", "SAM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Audit", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        //initilize sql connection
        string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
        //object of sql transaction
        SqlTransaction trans;
        //initilize connection
        SqlConnection connection = new SqlConnection(strConnString);
        //open connection
        connection.Open();
        //start of sql trandaction
        trans = connection.BeginTransaction();

        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                SqlCommand command = new SqlCommand("INSERT INTO SUPPLIER_AUDIT_MASTER(SAM_FDATE,SAM_P_CODE,SAM_QUALITY,SAM_DELIVERY,SAM_CM_COMP_CODE,SAM_FILE,SAM_COST) VALUES('" + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "','" + ddlSupplier.SelectedValue + "','" + txtAuditScore.Text.Trim() + "','" + txtCustLineDiscruption.Text.Trim() + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + lnkFUDrawingTR.Text + "','" + txtCustComplaint.Text + "')", connection, trans);
                command.ExecuteNonQuery();

                string Code = "";
                SqlCommand cmd1 = new SqlCommand("Select Max(SAM_CODE) from SUPPLIER_AUDIT_MASTER", connection, trans);
                cmd1.Transaction = trans;
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    Code = (dr1[0].ToString().Trim());
                }
                cmd1.Dispose();
                dr1.Dispose();
                trans.Commit();
                if (lnkFUDrawingTR.Text != "")
                {
                    if (ViewState["fileName"].ToString().Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/SuppAudit/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/SuppAudit");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\SuppAudit ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/SuppAudit/" + ViewState["fileName"].ToString().Trim());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/SuppAudit/" + Code + "/" + ViewState["fileName"].ToString().Trim());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                }
                CommonClasses.WriteLog("Supplier Audit", "Save", "Supplier Audit", txtFromDate.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/Transactions/VIEW/ViewSupplierAudit.aspx", false);

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                SqlCommand command = new SqlCommand("UPDATE SUPPLIER_AUDIT_MASTER SET SAM_FDATE='" + Convert.ToDateTime(txtFromDate.Text).ToString("MMM yyyy") + "',SAM_P_CODE='" + ddlSupplier.SelectedValue + "',SAM_QUALITY='" + txtAuditScore.Text + "',SAM_DELIVERY='" + txtCustLineDiscruption.Text + "',SAM_FILE='" + lnkFUDrawingTR.Text + "' ,SAM_COST='" + txtCustComplaint.Text + "'   WHERE SAM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'", connection, trans);
                command.ExecuteNonQuery();

                trans.Commit();
                CommonClasses.RemoveModifyLock("SUPPLIER_AUDIT_MASTER", "ES_MODIFY", "SAM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

                CommonClasses.WriteLog("Supplier Audit", "Update", "Supplier Audit", txtFromDate.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/Transactions/VIEW/ViewSupplierAudit.aspx", false);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            CommonClasses.SendError("Supplier Audit", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Customer Master", "ShowMessage", Ex.Message);
            return false;
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
            CommonClasses.SendError("Customer Master", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlSupplier_SelectedIndexChanged
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataTable dt = new DataTable();

        //dt = CommonClasses.Execute(" SELECT ISNULL(SUM(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY),0) AS ScheduleQty FROM  PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE INNER JOIN PARTY_MASTER ON PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE WHERE (PARTY_MASTER.P_INHOUSE_IND = 1) AND (PARTY_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID =  " + Session["CompanyId"] + ")     AND (P_CODE = '" + ddlSupplier.SelectedValue + "') AND (CONVERT(DATE, PSM_SCHEDULE_MONTH) BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "')  ");
        //DataTable dtInward = new DataTable();
        //dtInward = CommonClasses.Execute(" SELECT ISNULL(SUM(IWD_REV_QTY),0) AS  IWD_REV_QTY ,ISNULL(SUM(IWD_CON_OK_QTY),0) AS IWD_CON_OK_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND (CONVERT(DATE,IWM_DATE) BETWEEN   '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND  '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "') AND IWM_P_CODE='" + ddlSupplier.SelectedValue + "'");
        //double SchQty = 0, ActualQty = 0, OkQty = 0, DeleveryPer = 0, QualityPer = 0;
        //if (dt.Rows.Count > 0)
        //{
        //    //Purchase schedule Qty
        //    SchQty = Convert.ToDouble(dt.Rows[0]["SCHEDULEQTY"]);
        //}

        //if (dtInward.Rows.Count > 0)
        //{
        //    //Inward Received Qty
        //    ActualQty = Convert.ToDouble(dtInward.Rows[0]["IWD_REV_QTY"]);
        //    //Inspection OK Qty
        //    OkQty = Convert.ToDouble(dtInward.Rows[0]["IWD_CON_OK_QTY"]);
        //}
        //if (SchQty == 0)
        //{
        //    txtQuality.Text = "0";
        //    txtDelivery.Text = "0";
        //}
        //else
        //{
        //    DeleveryPer = Math.Round(((ActualQty / SchQty) * 40), 2);
        //    QualityPer = Math.Round(((OkQty / SchQty) * 40), 2);
        //}

        //if (DeleveryPer.ToString() == "NaN" || DeleveryPer.ToString() == "Infinity")
        //{
        //    DeleveryPer = 0;
        //    QualityPer = 0;
        //}
        //txtDelivery.Text = DeleveryPer.ToString();
        //txtQuality.Text = QualityPer.ToString();
    }
    #endregion ddlSupplier_SelectedIndexChanged

    #region txtAuditScore_TextChanged
    protected void txtAuditScore_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtAuditScore.Text) > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Audit Score can not be greater than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtAuditScore.Text = "0";
            txtAuditScore.Focus();
            return;
        }
    }
    #endregion txtQuality_TextChanged

    #region txtCustLineDiscruption_TextChanged
    protected void txtCustLineDiscruption_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtCustLineDiscruption.Text) > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Customer Line Disruption can not be greater than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtCustLineDiscruption.Text = "0";
            txtCustLineDiscruption.Focus();
            return;
        }
    }
    #endregion txtDelivery_TextChanged

    #region txtCustComplaint_TextChanged
    protected void txtCustComplaint_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtCustComplaint.Text) > 100)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Customer Complaint can not be greater than 100";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtCustComplaint.Text = "0";
            txtCustComplaint.Focus();
            return;
        }
    }
    #endregion txtDelivery_TextChanged

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
                CommonClasses.RemoveModifyLock("SUPPLIER_AUDIT_MASTER", "ES_MODIFY", "SAM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Transactions/VIEW/ViewSupplierAudit.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtFromDate.Text.Trim() == "")
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
            CommonClasses.SendError("Customer Master", "CheckValid", Ex.Message);
        }
        return flag;
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

    #region txtFromDate_TextChanged
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCust();
        /*Change By Mahesh :- Validate Date SHould be within Financial year*/
        DateTime dtDate = Convert.ToDateTime(txtFromDate.Text);
        if (dtDate < Convert.ToDateTime(Session["OpeningDate"]) || dtDate > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtFromDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date

            PanelMsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtFromDate.Focus();
            return;
        }
    }
    #endregion txtFromDate_TextChanged

    #region LoadCust
    private void LoadCust()
    {
        DataTable dt = new DataTable();
        string str = "";

        if (txtFromDate.Text != "")
        {
            //str = str + "SAM_FDATE BETWEEN '" + txtFromDate.Text + "' AND '" + txtToDate.Text + "' AND ";
        }
        if (Convert.ToInt32(ViewState["mlCode"]) == 0)
        {
            DateTime dtDate = Convert.ToDateTime(txtFromDate.Text);
            //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE, P_NAME FROM PARTY_MASTER WHERE " + str + " (ES_DELETE = 0) AND (P_TYPE = 2) AND (P_ACTIVE_IND = 1) AND (P_CM_COMP_ID = " + Session["CompanyID"] + ") and (P_INHOUSE_IND=1) and P_CODE NOT IN (SELECT P_CODE FROM SUPPLIER_AUDIT_MASTER WHERE SAM_P_CODE=P_CODE and ES_DELETE=0 and SAM_FDATE='" + txtFromDate.Text + "' and SAM_TDATE='" + txtToDate.Text + "') ORDER BY P_NAME");
            dt = CommonClasses.Execute("SELECT DISTINCT P_CODE, P_NAME FROM PARTY_MASTER WHERE (ES_DELETE = 0) AND (P_TYPE = 2) AND (P_ACTIVE_IND = 1) AND (P_CM_COMP_ID = 1) and (P_INHOUSE_IND=1) and P_CODE not  IN (SELECT P_CODE FROM SUPPLIER_AUDIT_MASTER WHERE SAM_P_CODE=P_CODE and ES_DELETE=0 and  (DATEPART(mm,SAM_FDATE)='" + dtDate.Month + "' and  DATEPART(YYYY,SAM_FDATE)='" + dtDate.Year + "') ) ORDER BY P_NAME");
        }
        else
        {
            dt = CommonClasses.Execute("SELECT DISTINCT P_CODE, P_NAME FROM PARTY_MASTER WHERE " + str + " (ES_DELETE = 0) AND (P_TYPE = 2) AND (P_ACTIVE_IND = 1) AND (P_CM_COMP_ID = " + Session["CompanyID"] + ") and (P_INHOUSE_IND=1)ORDER BY P_NAME");
        }
        if (dt.Rows.Count > 0)
        {
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
    }
    #endregion LoadCust

    #region Upload1
    protected void Upload1(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppAudit/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/SuppAudit/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            FUDrawingTR.SaveAs(Server.MapPath("~/UpLoadPath/SuppAudit/" + ViewState["fileName"].ToString()));
        }
        else
        {
            FUDrawingTR.SaveAs(Server.MapPath("~/UpLoadPath/SuppAudit/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
    }
    #endregion

    #region lnkFUDrawingTR_Click
    protected void lnkFUDrawingTR_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";

            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkFUDrawingTR.Text;
                directory = "../../UpLoadPath/SuppAudit/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkFUDrawingTR.Text;
                directory = "../../UpLoadPath/SuppAudit/" + code + "/" + filePath;
            }
            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Audit", "lnkFUDrawingTR_Click", ex.Message);
        }
    }
    #endregion
}
