using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

public partial class Masters_ADD_AddProjectCodeMaster : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    string fileName = "";
    DirectoryInfo ObjSearchDir;
    # endregion

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
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("Amend"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("Authorize"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewState["mlCode"] = mlCode;
                            ViewRec("VIEW");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Project Code Master", "Imgclose_Click", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
        }
        if (IsPostBack && FileUpload2.PostedFile != null)
        {
            if (FileUpload2.PostedFile.FileName.Length > 0)
            {
                fileName = FileUpload2.PostedFile.FileName;
                ViewState["fileName"] = fileName;
                Upload(null, null);
            }
        }
    }

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/BUDGET/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/BUDGET/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/BUDGET/" + ViewState["fileName"].ToString()));
            }
            else
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/BUDGET/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
            }
            lnkupload.Visible = true;
            lnkupload.Text = ViewState["fileName"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion



    protected void lnkupload_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/BUDGET/" + filePath;

            }
            else
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/BUDGET/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }



            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;



        }
        catch (Exception ex)
        {
            CommonClasses.SendError("upplier Master Entry", "lnkupload_Click", ex.Message);
        }
    }

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM PROJECT_CODE_MASTER WHERE PROCM_CODE=" + ViewState["mlCode"].ToString() + " AND PROCM_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtProCode.Text = dt.Rows[0]["PROCM_NAME"].ToString();
                txtBudget.Text = dt.Rows[0]["PROCM_AMT"].ToString();
                lnkupload.Text = dt.Rows[0]["PROCM_FILE"].ToString();
                lblamdno.Text = dt.Rows[0]["PROCM_AMNO"].ToString();

                if (dt.Rows[0]["PROCM_ALLOW"].ToString().ToUpper() == "TRUE" || dt.Rows[0]["PROCM_ALLOW"].ToString().ToUpper() == "1")
                {
                    chkAuto.Checked = true;
                }
                else
                {
                    chkAuto.Checked = false;
                }

                if (str == "VIEW")
                {
                    txtProCode.Enabled = false;
                    txtBudget.Enabled = false;

                    btnSubmit.Visible = false;
                }
                if (Request.QueryString[0].Equals("Authorize"))
                {
                    btnSubmit.Visible = true;
                    btnSubmit.Text = "Authorize";
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtProCode.Text.Trim() == "")
        {
            // ShowMessage("#Avisos", "The Field 'Project Code' is Required", CommonClasses.MSG_Warning);

            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Project Code";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;


        }
        if (!Request.QueryString[0].Equals("INSERT"))
        {
            DataTable dtIndent = new DataTable();
            dtIndent = CommonClasses.Execute(" select ISNULL(SUM(IND_AMT),0) AS IND_AMT from INDENT_MASTER,INDENT_DETAIL   where INM_CODE=IND_INM_CODE AND   IN_STATUS!=2 AND ES_DELETE=0 AND IN_PROJECT='" + ViewState["mlCode"].ToString() + "'");
            if (dtIndent.Rows.Count > 0)
            {
                if (Convert.ToDouble(txtBudget.Text) < Convert.ToDouble(dtIndent.Rows[0]["IND_AMT"].ToString()))
                {

                    PanelMsg.Visible = true;
                    lblmsg.Text = "Budget amount should not less than '" + dtIndent.Rows[0]["IND_AMT"].ToString() + "'";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;

                    //ShowMessage("#Avisos", "Budget amount should not less than '" + dtIndent.Rows[0]["IND_AMT"].ToString() + "'", CommonClasses.MSG_Warning);
                    //return;
                }
            }
        }

        SaveRec();
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {

                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select PROCM_CODE,PROCM_NAME FROM PROJECT_CODE_MASTER WHERE lower(PROCM_NAME)= lower('" + txtProCode.Text.Trim().Replace("'", "\''") + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO PROJECT_CODE_MASTER (PROCM_COMP_ID,PROCM_NAME,PROCM_APPROVE,PROCM_DATE,PROCM_USER,PROCM_AMNO,PROCM_AMT,PROCM_FILE,PROCM_ALLOW)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtProCode.Text.Trim().Replace("'", "\''") + "',0,'" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "','" + Convert.ToInt32(Session["UserCode"]) + "',0,'" + txtBudget.Text + "','" + lnkupload.Text + "','" + chkAuto.Checked + "')"))
                    {

                        string Code = CommonClasses.GetMaxId("Select Max(PROCM_CODE) from PROJECT_CODE_MASTER");

                        #region file upload for tooling Photo
                        if (lnkupload.Text.Trim() != "")
                        {
                            string sDirPath = Server.MapPath(@"~/UpLoadPath/BUDGET/" + Code + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath);

                            string sDirPath1 = Server.MapPath(@"~/UpLoadPath/BUDGET/");
                            DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                            dir.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\BUDGET ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/BUDGET/" + lnkupload.Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath//BUDGET/" + Code + "/" + lnkupload.Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion
                        CommonClasses.WriteLog("PROJECT_CODE_MASTER", "Save", "PROJECT_CODE_MASTER", txtProCode.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtProCode.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Project Code Already Exists", CommonClasses.MSG_Warning);
                }
            }
            #endregion
            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PROJECT_CODE_MASTER WHERE ES_DELETE=0 AND PROCM_CODE!= '" + ViewState["mlCode"].ToString() + "' AND lower(PROCM_NAME) = lower('" + txtProCode.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PROJECT_CODE_MASTER SET PROCM_NAME='" + txtProCode.Text + "', PROCM_AMT='" + txtBudget.Text + "' ,PROCM_FILE='" + lnkupload.Text + "', PROCM_ALLOW='" + chkAuto.Checked + "' WHERE PROCM_CODE='" + ViewState["mlCode"].ToString() + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Project Code Master", "Update", "Project Code Master", txtProCode.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        txtProCode.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Project Code Already Exists", CommonClasses.MSG_Warning);
                    txtProCode.Focus();
                }
            }
            #endregion
            #region Amend
            else if (Request.QueryString[0].Equals("Amend"))
            {
                int AMEND_COUNT = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = CommonClasses.Execute("select isnull(PROCM_AMNO,0) as PROCM_AMNO from PROJECT_CODE_MASTER WHERE PROCM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and PROCM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                if (dt.Rows.Count > 0)
                {
                    AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["PROCM_AMNO"]);
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (AMEND_COUNT == 0)
                {
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (CommonClasses.Execute1("insert into PROJECT_CODE_MASTER_AMMEND select PROCM_CODE,PROCM_NAME,PROCM_COMP_ID,ES_DELETE,MODIFY,PROCM_APPROVE,PROCM_DATE,PROCM_USER,PROCM_AMNO,	PROCM_AMT,PROCM_FILE,PROCM_ALLOW from PROJECT_CODE_MASTER where PROCM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' "))
                {


                    if (CommonClasses.Execute1("UPDATE PROJECT_CODE_MASTER SET PROCM_NAME='" + txtProCode.Text + "', PROCM_AMT='" + txtBudget.Text + "' ,PROCM_FILE='" + lnkupload.Text + "' ,PROCM_AMNO= '" + AMEND_COUNT + "',PROCM_DATE='" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "', PROCM_USER='" + Convert.ToInt32(Session["UserCode"]) + "' ,PROCM_APPROVE=0   ,PROCM_ALLOW='" + chkAuto.Checked + "' WHERE PROCM_CODE='" + ViewState["mlCode"].ToString() + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PROJECT_CODE_MASTER", "Amend", "PROCM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Project Code Master", "Update", "Project Code Master", txtProCode.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
                    }
                }
                else
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "PO Not Amend";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //ddlPOType.Focus();
                }
            }
            #endregion
            #region Authorize
            else if (Request.QueryString[0].Equals("Authorize"))
            {

                if (CommonClasses.Execute1("UPDATE PROJECT_CODE_MASTER  SET PROCM_APPROVE=1 WHERE PROCM_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                    CommonClasses.WriteLog("Project Code Master", "Authorize", "Project Code Master", txtProCode.Text, Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    txtProCode.Focus();
                }

            }
            #endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


    #region btnCancelok_Click
    protected void btnCancelok_Click(object sender, EventArgs e)
    {
        myframe.Attributes["src"] = null;
        ModalPopupExtenderDovView.Hide();

    }
    #endregion btnCancelok_Click

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
            CommonClasses.SendError("Project Code Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && ViewState["mlCode"].ToString() != null)
            {
                CommonClasses.RemoveModifyLock("PROJECT_CODE_MASTER", "MODIFY", "PROCM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Masters/VIEW/ViewProjectCodeMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master", "btnCancel_Click", ex.Message);
        }
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtProCode.Text.Trim() == "")
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
            CommonClasses.SendError("Project Code Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnClose_Click
}