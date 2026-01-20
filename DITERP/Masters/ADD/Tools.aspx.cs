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

public partial class ToolRoom_ADD_Tools : System.Web.UI.Page
{
    #region Declartion
    DirectoryInfo ObjSearchDir;
    static int mlCode = 0;
    static string right = "";
    int CValue;
    string fileName = "";
    string fileName2 = "";
    string fileNameD = "";
    int SValue;
    #endregion

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
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
                    ViewState["fileName"] = fileName;
                    ViewState["fileName2"] = fileName2;
                    ViewState["fileNameD"] = fileNameD;
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
                    LoadCust();
                    LoadPartNo();
                    txtRevDate.Attributes.Add("readonly", "readonly");
                    txtRevDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Tool Master", "PageLoad", ex.Message);
                }
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
            if (IsPostBack && FileUpload1.PostedFile != null)
            {
                if (FileUpload1.PostedFile.FileName.Length > 0)
                {
                    fileName2 = FileUpload1.PostedFile.FileName;
                    ViewState["fileName2"] = fileName2;
                    Upload2(null, null);
                }
            }

        }
    }
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/ToolPhotoF/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/ToolPhoto/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/ToolPhotoF/" + ViewState["fileName"].ToString()));
            }
            else
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/ToolPhoto/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
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

    #region Upload2
    protected void Upload2(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/TModelF/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModel/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/TModelF/" + ViewState["fileName2"].ToString()));
            }
            else
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/3DModel/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
            }
            lnkTModel.Visible = true;
            lnkTModel.Text = ViewState["fileName2"].ToString();
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
                directory = "../../UpLoadPath/ToolRoom/ToolPhotoF/" + filePath;

            }
            else
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/ToolRoom/ToolPhoto/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }



            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;



        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);
        }
    }

    protected void lnkuploadTModel_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/ToolRoom/TModelF/" + filePath;
            }
            else
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/ToolRoom/3DModel/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }

            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);
        }
    }

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txttoolno.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Tool No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txttoolno.Focus();
                return;
            }
            if (ddlCustomer.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Customer";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }
            if (ddlpart.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select part";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlpart.Focus();
                return;
            }
            if (txtRevNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Revision No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRevNo.Focus();
                return;
            }
            if (txtStdToolLife.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Standard tool life";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStdToolLife.Focus();
                return;
            }
            if (txtPMfreq.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter  Preventive Maint Frequance (SHOTS) ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPMfreq.Focus();
                return;
            }
            if (txtPendingMonth.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter  Pending Tools Life(month) ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPMfreq.Focus();
                return;
            }
            if (txtPendingShots.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Pending Tools Life(Shots) ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPMfreq.Focus();
                return;
            }

            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Tool Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select T_CODE,T_NAME,T_TYPE,T_P_CODE,T_I_CODE,T_PHOTO,T_PHOTO_PATH,T_STDLIFE,T_PENDTOOLLIFE,T_PENDTOOLLIFEMONTH,T_OWNER,T_PMFREQ,T_3D,T_3D_PATH,T_REVNO,T_STATUS,T_REF_NO,T_REV_DATE,T_TOOLNO FROM TOOL_MASTER where T_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " AND T_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["T_CODE"]); ;
                txttoolno.Text = dt.Rows[0]["T_NAME"].ToString();//txtNoTools
                ddlType.SelectedValue = dt.Rows[0]["T_TYPE"].ToString();
                LoadCust();
                ddlCustomer.SelectedValue = dt.Rows[0]["T_P_CODE"].ToString();
                LoadPartNo();
                ddlpart.SelectedValue = dt.Rows[0]["T_I_CODE"].ToString();
                txtStdToolLife.Text = dt.Rows[0]["T_STDLIFE"].ToString();
                txtcavity.Text = dt.Rows[0]["T_TOOLNO"].ToString();
                txtPendingMonth.Text = dt.Rows[0]["T_PENDTOOLLIFEMONTH"].ToString();
                txtPendingShots.Text = dt.Rows[0]["T_PENDTOOLLIFE"].ToString();
                txtRevDate.Text = Convert.ToDateTime(dt.Rows[0]["T_REV_DATE"]).ToString("dd MMM yyyy");
                txtRevNo.Text = dt.Rows[0]["T_REVNO"].ToString();
                ddlOwner.SelectedValue = dt.Rows[0]["T_OWNER"].ToString();
                txtPMfreq.Text = dt.Rows[0]["T_PMFREQ"].ToString();
                string file1st = dt.Rows[0]["T_PHOTO"].ToString();
                string file2nd = dt.Rows[0]["T_3D"].ToString();
                //FileUpload1="";
                //FileUpload2.FileName=;
                lnkupload.Text = dt.Rows[0]["T_PHOTO_PATH"].ToString();

                lnkTModel.Text = dt.Rows[0]["T_3D_PATH"].ToString();
                txtRefNo.Text = dt.Rows[0]["T_REF_NO"].ToString();

                if (dt.Rows[0]["T_STATUS"].ToString().ToUpper() == "TRUE")
                {
                    chkStatus.Checked = true;
                }
                else
                {
                    chkStatus.Checked = false;
                }

                if (str == "VIEW")
                {
                    ddlType.Enabled = false;
                    txttoolno.Enabled = false;
                    ddlCustomer.Enabled = false;
                    ddlOwner.Enabled = false;
                    ddlpart.Enabled = false;
                    txtcavity.Enabled = false;
                    txtStdToolLife.Enabled = false;
                    txtPendingMonth.Enabled = false;
                    txtPendingShots.Enabled = false;
                    txtRevDate.Enabled = false;
                    txtRevNo.Enabled = false;
                    FileUpload1.Enabled = false;
                    FileUpload2.Enabled = false;
                    txtPMfreq.Enabled = false;
                    btnSubmit.Visible = false;
                    chkStatus.Enabled = false;
                    txtRefNo.Enabled = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("TOOL_MASTER", "ES_MODIFY", "T_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "ViewRec", Ex.Message);
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
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select * FROM TOOL_MASTER WHERE T_NAME= lower('" + txttoolno.Text.Trim().Replace("'", "\''") + "')  AND ES_DELETE='False'");

                if (dt.Rows.Count == 0)
                { //trans = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand("INSERT INTO TOOL_MASTER (T_NAME, T_TYPE, T_P_CODE, T_I_CODE, T_STDLIFE, T_PENDTOOLLIFE, T_PENDTOOLLIFEMONTH, T_OWNER,T_PMFREQ, T_REVNO, T_REV_DATE,T_CM_COMP_ID,T_STATUS,T_REF_NO,T_PHOTO_PATH,T_3D_PATH,T_TOOLNO)VALUES ('" + txttoolno.Text.Trim().Replace("'", "\''") + "','" + ddlType.SelectedValue.ToString() + "','" + ddlCustomer.SelectedValue.ToString() + "','" + ddlpart.SelectedValue.ToString() + "','" + txtStdToolLife.Text.Trim() + "','" + txtPendingShots.Text.Trim() + "','" + txtPendingMonth.Text.Trim() + "','" + ddlOwner.SelectedValue + "','" + txtPMfreq.Text.Trim() + "','" + txtRevNo.Text.Trim() + "','" + Convert.ToDateTime(txtRevDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + chkStatus.Checked + "','" + txtRefNo.Text.Trim().Replace("'", "\''") + "','" + lnkupload.Text + "','" + lnkTModel.Text + "','" + txtcavity.Text.Trim() + "')", connection, trans);
                    command.ExecuteNonQuery();

                    string Code = "";
                    SqlCommand cmd1 = new SqlCommand("Select Max(T_CODE) from TOOL_MASTER", connection, trans);
                    cmd1.Transaction = trans;
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    while (dr1.Read())
                    {
                        Code = (dr1[0].ToString().Trim());
                    }
                    cmd1.Dispose();
                    dr1.Dispose();
                    trans.Commit();

                    #region file upload for tooling Photo
                    if (ViewState["fileName"].ToString().Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/ToolPhoto/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/ToolRoom/ToolPhotoF/");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\ToolRoom\\ToolPhotoF ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/ToolRoom/ToolPhotoF/" + lnkupload.Text);
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath//ToolRoom/ToolPhoto/" + Code + "/" + lnkupload.Text);
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion

                    #region FileUpload for 3D Model
                    if (ViewState["fileName2"].ToString().Trim() != "")
                    {
                        string sDirPath13 = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModel/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath13);

                        string sDirPath12 = Server.MapPath(@"~/UpLoadPath/ToolRoom/TModelF/");
                        DirectoryInfo dir1 = new DirectoryInfo(sDirPath13);

                        dir1.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\ToolRoom\\3DModel ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/ToolRoom/TModelF/" + lnkTModel.Text);
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModel/" + Code + "/" + lnkTModel.Text);
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion

                    CommonClasses.WriteLog("Tool Master", "Save", "Tool Master", txttoolno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/ToolRoom/VIEW/ViewTools.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select * FROM TOOL_MASTER WHERE T_NAME= lower('" + txttoolno.Text.Trim().Replace("'", "\''") + "')  AND T_CODE!='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND ES_DELETE='False'");

                if (dt.Rows.Count == 0)
                {
                    SqlCommand command = new SqlCommand("UPDATE TOOL_MASTER SET T_NAME='" + txttoolno.Text.Trim().Replace("'", "\''") + "', T_TYPE='" + ddlType.SelectedValue + "', T_P_CODE='" + ddlCustomer.SelectedValue + "', T_I_CODE='" + ddlpart.SelectedValue + "' , T_PHOTO_PATH='" + lnkupload.Text + "', T_STDLIFE='" + txtStdToolLife.Text.Trim() + "', T_PENDTOOLLIFE='" + txtPendingShots.Text.Trim() + "', T_PENDTOOLLIFEMONTH='" + txtPendingMonth.Text.Trim() + "', T_OWNER='" + ddlOwner.SelectedValue + "',T_PMFREQ='" + txtPMfreq.Text.Trim() + "',T_3D_PATH='" + lnkTModel.Text + "', T_REVNO='" + txtRevNo.Text + "', T_REV_DATE='" + Convert.ToDateTime(txtRevDate.Text).ToString("dd/MMM/yyyy") + "',T_STATUS='" + chkStatus.Checked + "',T_REF_NO='" + txtRefNo.Text.Trim().Replace("'", "\''") + "',T_TOOLNO='" + txtcavity.Text.Trim() + "' WHERE T_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'", connection, trans);
                    command.ExecuteNonQuery();


                    trans.Commit();
                    CommonClasses.RemoveModifyLock("TOOL_MASTER", "ES_MODIFY", "T_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

                    CommonClasses.WriteLog("Tool Master", "Update", "Tool Master", txttoolno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/ToolRoom/VIEW/ViewTools.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                }
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            CommonClasses.SendError("Tool Master", "SaveRec", ex.Message);
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

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPartNo();
    }

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("TOOL_MASTER", "ES_MODIFY", "T_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/ToolRoom/VIEW/ViewTools.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tool Master", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txttoolno.Text.Trim() == "")
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

    private void LoadCust()
    {
        DataTable dt = new DataTable();

        dt = CommonClasses.Execute("select distinct P_CODE,P_NAME from PARTY_MASTER,CUSTPO_MASTER where PARTY_MASTER.ES_DELETE=0 and P_TYPE=1 and P_ACTIVE_IND=1 and CPOM_P_CODE=P_CODE and CUSTPO_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + Session["CompanyID"] + " order by P_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
    }

    private void LoadPartNo()
    {
        DataTable dt = new DataTable();
        string str = "";

        if (ddlCustomer.SelectedIndex != 0)
        {
            str = str + "CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ";
        }
        dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL where " + str + " CUSTPO_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + Session["CompanyID"] + " and CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE=I_CODE and I_ACTIVE_IND=1 and I_CAT_CODE=-2147483648 order by I_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlpart.DataSource = dt;
            ddlpart.DataTextField = "I_NAME";
            ddlpart.DataValueField = "I_CODE";
            ddlpart.DataBind();
            ddlpart.Items.Insert(0, new ListItem("Select Part No.", "0"));
        }
    }
}
