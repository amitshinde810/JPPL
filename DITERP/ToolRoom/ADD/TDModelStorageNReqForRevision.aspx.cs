using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web;

public partial class ToolRoom_ADD_TDModelStorageNReqForRevision : System.Web.UI.Page
{
    #region Declartion
    DirectoryInfo ObjSearchDir;
    static int mlCode = 0;
    static string right = "";
    int CValue;
    string fileName = "";
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
                    LoadToolNo();
                    LoadPartNo();
                    ViewState["fileName"] = fileName;
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
                    txtMonth.Attributes.Add("readonly", "readonly");
                    txtMonth.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("3D Model Storage and Request For Revision", "PageLoad", ex.Message);
                }
            }
            if (IsPostBack && toolingupload.PostedFile != null)
            {
                if (toolingupload.PostedFile.FileName.Length > 0)
                {
                    fileName = toolingupload.PostedFile.FileName;
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
    }
    #endregion

    #region lnkupload_Click
    protected void lnkupload_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            FileInfo File;
            string code = "";
            string directory = "";

            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/ToolRoom/3DModelStoragenRev/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkupload.Text;
                lnkupload.Visible = true;
                directory = "../../UpLoadPath/ToolRoom/3DModelStoragenRev/" + code + "/" + filePath;

            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);

        }
    }
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModelStoragenRev/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            toolingupload.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + ViewState["fileName"].ToString()));
        }
        else
        {
            toolingupload.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select TMS_CODE,TMS_I_CODE,TMS_DOC_UPLOAD,TMS_T_CODE,TMS_REV_DATE,TMS_T_REV_CODE FROM TDMODELSTORAGE_MASTER where TMS_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " AND TMS_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["TMS_CODE"]);
                LoadToolNo();
                ddlToolNo.SelectedValue = dt.Rows[0]["TMS_T_CODE"].ToString();
                LoadPartNo();
                ddlpart.SelectedValue = dt.Rows[0]["TMS_I_CODE"].ToString();
                //LoadRevNo();
                ddlRevNo.Text = dt.Rows[0]["TMS_T_REV_CODE"].ToString();
                txtMonth.Text = dt.Rows[0]["TMS_REV_DATE"].ToString();
                lnkupload.Text = dt.Rows[0]["TMS_DOC_UPLOAD"].ToString();

                txtMonth.Text = Convert.ToDateTime(dt.Rows[0]["TMS_REV_DATE"]).ToString("dd MMM yyyy");

                if (str == "VIEW")
                {
                    ddlRevNo.Enabled = false;
                    ddlpart.Enabled = false;
                    ddlToolNo.Enabled = false;
                    txtMonth.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("TDMODELSTORAGE_MASTER", "ES_MODIFY", "TMS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "ViewRec", Ex.Message);
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
        string Code = "";
        //start of sql trandaction
        trans = connection.BeginTransaction();

        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                SqlCommand command = new SqlCommand("INSERT INTO TDMODELSTORAGE_MASTER (TMS_I_CODE,TMS_DOC_UPLOAD,TMS_T_CODE,TMS_REV_DATE,TMS_T_REV_CODE,TMS_CM_COMP_ID)VALUES ('" + ddlpart.SelectedValue + "','" + lnkupload.Text + "','" + ddlToolNo.SelectedValue.ToString() + "','" + txtMonth.Text + "','" + ddlRevNo.Text + "','" + Convert.ToInt32(Session["CompanyId"]) + "')", connection, trans);
                command.ExecuteNonQuery();

                SqlCommand cmd1 = new SqlCommand("Select Max(TMS_CODE) from TDMODELSTORAGE_MASTER", connection, trans);
                cmd1.Transaction = trans;
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    Code = (dr1[0].ToString().Trim());
                }
                cmd1.Dispose();
                dr1.Dispose();

                // string Code = CommonClasses.GetMaxId("Select Max(TMS_CODE) from TDMODELSTORAGE_MASTER");
                if (ViewState["fileName"].ToString().Trim() != "")
                {
                    string sDirPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + Code + "");

                    ObjSearchDir = new DirectoryInfo(sDirPath);

                    string sDirPath1 = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModelStoragenRev");
                    DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                    dir.Refresh();

                    if (!ObjSearchDir.Exists)
                    {
                        ObjSearchDir.Create();
                    }
                    string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                    //Get the full path of the file    
                    string fullFilePath1 = currentApplicationPath + "UpLoadPath\\ToolRoom\\3DModelStoragenRev ";
                    string fullFilePath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + ViewState["fileName"].ToString().Trim());
                    // Get the destination path
                    string copyToPath = Server.MapPath(@"~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + Code + "/" + ViewState["fileName"].ToString().Trim());
                    DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                    System.IO.File.Move(fullFilePath, copyToPath);
                }


                trans.Commit();
                CommonClasses.WriteLog("3D Model Storage and Request For Revision", "Save", "3D Model Storage and Request For Revision", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/TDModelStorageNReqForRevision.aspx", false);
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("UPDATE TDMODELSTORAGE_MASTER SET TMS_T_CODE='" + ddlToolNo.SelectedValue + "', TMS_REV_DATE='" + txtMonth.Text + "', TMS_I_CODE='" + ddlpart.SelectedValue + "',TMS_T_REV_CODE='" + ddlRevNo.Text + "',TMS_DOC_UPLOAD='" + lnkupload.Text + "' WHERE TMS_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'", connection, trans);
                command.ExecuteNonQuery();

                CommonClasses.RemoveModifyLock("TDMODELSTORAGE_MASTER", "ES_MODIFY", "TMS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                trans.Commit();
                CommonClasses.WriteLog("3D Model Storage and Request For Revision", "Update", "3D Model Storage and Request For Revision", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/TDModelStorageNReqForRevision.aspx", false);
            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            CommonClasses.SendError("3D Model Storage and Request For Revision", "SaveRec", ex.Message);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    protected void ddlToolNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LoadPartNo();
        DataTable dt = new DataTable(); string str = "";
        ddlpart.Items.Clear();
        ddlpart.DataBind();
        if (ddlToolNo.SelectedIndex != 0)
        {
            str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
        }
        dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from ITEM_MASTER,TOOL_MASTER where " + str + " I_CM_COMP_ID=" + Session["CompanyID"] + " and T_I_CODE=I_CODE and I_ACTIVE_IND=1 and I_CAT_CODE=-2147483648 AND TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 order by I_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlpart.DataSource = dt;
            ddlpart.DataTextField = "I_NAME";
            ddlpart.DataValueField = "I_CODE";
            ddlpart.DataBind();
        }
    }

    protected void ddlpart_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadToolNo();
    }

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("TDMODELSTORAGE_MASTER", "ES_MODIFY", "TMS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/ToolRoom/VIEW/TDModelStorageNReqForRevision.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("3D Model Storage and Request For Revision", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlToolNo.Text.Trim() == "")
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
            CommonClasses.SendError("3D Model Storage and Request For Revision", "CheckValid", Ex.Message);
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

    private void LoadToolNo()
    {
        DataTable dt = new DataTable();
        string str = "";
        if (ddlpart.SelectedIndex != -1)
        {
            str = str + "T_I_CODE='" + ddlpart.SelectedValue + "' AND ";
        }

        dt = CommonClasses.Execute("select distinct T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END+'- '+T_NAME AS T_NAME from TOOL_MASTER where " + str + " ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " order by T_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
            if (ddlpart.SelectedIndex != -1)
            {
                ddlToolNo.SelectedIndex = 1;
            }
        }
    }

    private void LoadPartNo()
    {
        DataTable dt = new DataTable();
        string str = "";

        if (ddlToolNo.SelectedIndex != 0)
        {
            str = str + "T_CODE='" + ddlToolNo.SelectedValue + "' AND ";
        }
        dt = CommonClasses.Execute("select distinct I_CODE,I_CODENO+'-'+I_NAME as I_NAME from ITEM_MASTER,TOOL_MASTER where " + str + " I_CM_COMP_ID=" + Session["CompanyID"] + " and T_I_CODE=I_CODE and I_ACTIVE_IND=1 and I_CAT_CODE=-2147483648 AND TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 order by I_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlpart.DataSource = dt;
            ddlpart.DataTextField = "I_NAME";
            ddlpart.DataValueField = "I_CODE";
            ddlpart.DataBind();
            ddlpart.Items.Insert(0, new ListItem("Select Part No.", "0"));

            if (ddlToolNo.SelectedIndex != 0)
            {
                ddlpart.SelectedIndex = 1;
            }
        }
    }
}
