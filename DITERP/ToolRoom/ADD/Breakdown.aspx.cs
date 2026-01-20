using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class ToolRoom_ADD_Breakdown : System.Web.UI.Page
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
                        txtBreakDownDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                        txtClosureDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                        ViewState["mlCode"] = mlCode;
                        LoadTool();
                        loadPart();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            act.Visible = false;
                            hrs.Visible = false;
                            rep.Visible = false;
                            close.Visible = false;
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("UpdateStatus"))
                        {
                            //act.Visible = true;
                            //hrs.Visible = false;
                            //rep.Visible = false;
                            //close.Visible = false;
                            txtClosureDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("UPDATE");
                        }
                        else
                        {
                            act.Visible = false;
                            hrs.Visible = false;
                            rep.Visible = false;
                            close.Visible = false;

                            txtBreakDownDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                            txtClosureDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                        }
                        //txtBreakDownDate.Attributes.Add("readonly", "readonly");
                        //txtClosureDate.Attributes.Add("readonly", "readonly");
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Breakdown Entry", "Page_Load", ex.Message);
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
            if (IsPostBack && FUDrawingTR.PostedFile != null)
            {
                if (FUDrawingTR.PostedFile.FileName.Length > 0)
                {
                    fileName1 = FUDrawingTR.PostedFile.FileName;
                    ViewState["fileName2"] = fileName1.Trim();
                    lnkFUDrawingTR.Text = ViewState["fileName2"].ToString();
                    Upload1(null, null);
                }
            }
            else
            {
                ViewState["fileName2"] = "";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region LoadTool
    //for load tool 
    public void LoadTool()
    {
        DataTable Dt = new DataTable();
        string str = "";
        if (ddlPart.SelectedIndex != -1)
        {
            str = str + "T_I_CODE=" + ddlPart.SelectedValue + " AND ";
        }
        Dt = CommonClasses.Execute("SELECT DISTINCT T_CODE,T_NAME FROM TOOL_MASTER where " + str + " ES_DELETE=0 AND T_STATUS=1   AND T_TYPE='" + ddlType.SelectedValue + "' AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0  AND TRR_TYPE=1) AND T_CM_COMP_ID='" + Session["CompanyId"].ToString() + "'");
       // Dt = CommonClasses.Execute("select distinct T_CODE,I_CODE,T_NAME , (select ISNULL(SUM(isnull(IRND_PROD_QTY,0)),0) from irn_entry inner join IRN_DETAIL on IRN_CODE=IRND_IRN_CODE where ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_T_CODE=T_CODE  AND IRND_I_CODE=I_CODE) AS IRND_PROD_QTY, T_PENDTOOLLIFE,(ISNULL((SELECT ISNULL(SUM(isnull(TRR_STD_PROD,0)),0) FROM TOOLROOM_REFURBISH_MASTER where ES_DELETE=0 AND TRR_T_CODE=T_CODE),0)) AS TRR_STD_PROD into #temp from TOOL_MASTER ,ITEM_MASTER where " + str + " TOOL_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND T_CODE NOT IN (SELECT TRR_T_CODE FROM TOOLROOM_REFURBISH_MASTER WHERE ES_DELETE=0 AND TRR_TYPE=1)  SELECT T_CODE,T_NAME FROM #temp where IRND_PROD_QTY<(T_PENDTOOLLIFE+TRR_STD_PROD) union select T_CODE,T_NAME from TOOL_MASTER where " + str + " ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE='" + ddlType.SelectedValue + "' AND T_STATUS=1 AND T_CODE NOT IN(select TRR_T_CODE from TOOLROOM_REFURBISH_MASTER where ES_DELETE=0) AND T_CODE NOT IN (select PM_TOOL_NO from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 AND DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtBreakDownDate.Text).Month + "' AND  DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtBreakDownDate.Text).Year + "' AND ES_DELETE=0)  order by T_NAME drop table #temp");

        if (Dt.Rows.Count > 0)
        {
            ddlTool.DataSource = Dt;
            ddlTool.DataTextField = "T_NAME";
            ddlTool.DataValueField = "T_CODE";
            ddlTool.DataBind();
            ddlTool.Items.Insert(0, new ListItem("Select Tool ", "0"));
            if (ddlPart.SelectedIndex != -1)
            {
                ddlTool.SelectedIndex = 1;
            }
        }
        else
        {
            ddlTool.DataSource = Dt;
            ddlTool.DataBind();
            ddlTool.Items.Insert(0, new ListItem("Select Tool ", "0"));
        }
    }
    #endregion

    public void loadPart()
    {
        DataTable Dt = new DataTable();
        string str = "";
        if (ddlTool.SelectedValue != "0")
        {
            str = str + "T_CODE=" + ddlTool.SelectedValue + " AND ";
        }
        Dt = CommonClasses.Execute("SELECT DISTINCT I_CODE, I_CODENO +' - '+ I_NAME AS I_CODENO FROM TOOL_MASTER,ITEM_MASTER where " + str + " TOOL_MASTER.ES_DELETE=0 AND T_STATUS=1   AND T_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND I_CODE=T_I_CODE");
        if (Dt.Rows.Count > 0)
        {
            ddlPart.DataSource = Dt;
            ddlPart.DataTextField = "I_CODENO";
            ddlPart.DataValueField = "I_CODE";
            ddlPart.DataBind();
            ddlPart.Items.Insert(0, new ListItem("Select Part ", "0"));
            if (ddlTool.SelectedIndex != 0)
            {
                ddlPart.SelectedIndex = 1;
            }
        }
        else
        {
            ddlPart.DataSource = Dt;
            ddlPart.DataBind();
            ddlPart.Items.Insert(0, new ListItem("Select Part ", "0"));
        }
    }

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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Breakdown Entry", "CheckValid", Ex.Message);
        }
        return flag;
    }

    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("BREAKDOWN_ENTRY", "ES_MODIFY", "B_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/ToolRoom/VIEW/ViewBreakdown.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "btnCancel_Click", ex.Message);
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
        if (txtReason.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Reason";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtReason.Focus();
            return;
        }
        if (ddlTool.SelectedValue == "0")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Tool";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlTool.Focus();
            return;
        }
        if (ddlPart.SelectedValue == "0")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Select Part";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlTool.Focus();
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
                lblmsg.Text = "Please Upload Report File";
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
            dt = CommonClasses.Execute("SELECT B_CODE, B_T_CODE,B_I_CODE, B_NO,B_SLIPNO, B_DATE, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE,B_DRAWING_UPLOAD,T_NAME,CASE WHEN T_TYPE=1 then '1' Else '0' END  AS T_TYPE, I_CODENO +' - '+ I_NAME AS I_CODENO FROM BREAKDOWN_ENTRY INNER JOIN TOOL_MASTER ON BREAKDOWN_ENTRY.B_T_CODE = TOOL_MASTER.T_CODE INNER JOIN ITEM_MASTER ON BREAKDOWN_ENTRY.B_I_CODE = ITEM_MASTER.I_CODE WHERE (BREAKDOWN_ENTRY.ES_DELETE = 0) AND (TOOL_MASTER.ES_DELETE = 0)  AND B_CODE=" + ViewState["mlCode"].ToString() + " AND B_CM_ID= " + (string)Session["CompanyId"] + "  ");

            if (dt.Rows.Count > 0)
            {
                txtBNo.Text = dt.Rows[0]["B_NO"].ToString();
                txtSlipNo.Text = dt.Rows[0]["B_SLIPNO"].ToString();
                ddlType.SelectedValue = dt.Rows[0]["T_TYPE"].ToString();
                ddlPart.SelectedValue = dt.Rows[0]["B_I_CODE"].ToString();
                LoadTool();
                ddlTool.SelectedValue = dt.Rows[0]["B_T_CODE"].ToString();
                // ddlTool_SelectedIndexChanged(null, null);
                txtReason.Text = dt.Rows[0]["B_REASON"].ToString();
                txtAction.Text = dt.Rows[0]["B_ACTION"].ToString();
                txtBreakDownDate.Text = Convert.ToDateTime(dt.Rows[0]["B_DATE"]).ToString("dd/MMM/yyyy HH:mm");
                txtClosureDate.Text = Convert.ToDateTime(dt.Rows[0]["B_CLOSURE"]).ToString("dd/MMM/yyyy HH:mm");
                txtHours.Text = dt.Rows[0]["B_HOURS"].ToString();
                lnkupload.Text = dt.Rows[0]["B_FILE"].ToString();
                lnkFUDrawingTR.Text = dt.Rows[0]["B_DRAWING_UPLOAD"].ToString();
                txtBNo.Enabled = false;
                if (str == "VIEW")
                {
                    txtSlipNo.Enabled = false;
                    ddlType.Enabled = false;
                    ddlTool.Enabled = false;
                    ddlPart.Enabled = false;
                    txtReason.Enabled = false;
                    txtAction.Enabled = false;
                    txtBreakDownDate.Enabled = false;
                    txtHours.Enabled = false;
                    txtClosureDate.Enabled = false;
                    btnSubmit.Visible = false;
                    FUDrawingTR.Enabled = false;
                    FileUpload1.Enabled = false;
                }
                if (str == "UPDATE")
                {
                    txtClosureDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                    Hours();
                    txtSlipNo.Enabled = false;
                    ddlType.Enabled = false;
                    ddlTool.Enabled = false;
                    ddlPart.Enabled = false;
                    txtReason.Enabled = false;
                    // txtAction.Enabled = false;
                    txtBreakDownDate.Enabled = false;
                    txtHours.Enabled = false;
                    FUDrawingTR.Enabled = false;
                    CommonClasses.SetModifyLock("BREAKDOWN_ENTRY", "ES_MODIFY", "B_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    // btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("BREAKDOWN_ENTRY", "ES_MODIFY", "B_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region SaveRec
    bool SaveRec()
    {
        string file = "";
        bool result = false;
        PanelMsg.Visible = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt1 = new DataTable();
                dt1 = CommonClasses.Execute("SELECT ISNULL(MAX(B_NO),0)+1 AS B_NO FROM BREAKDOWN_ENTRY WHERE B_TYPE=0 AND B_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"].ToString()) + "' AND ES_DELETE='False'");
                if (dt1.Rows.Count > 0)
                {
                    txtBNo.Text = dt1.Rows[0][0].ToString();
                }
                //DataTable dt = new DataTable();
                // dt = CommonClasses.Execute("SELECT * FROM BREAKDOWN_ENTRY WHERE lower(B_SLIPNO)= lower('" + txtSlipNo.Text.Trim().Replace("'", "\''") + "')  AND  B_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"].ToString()) + "' and ES_DELETE='False'");
                // if (dt.Rows.Count == 0)
                // {
                if (CommonClasses.Execute1("INSERT INTO BREAKDOWN_ENTRY (B_NO, B_SLIPNO, B_CM_CODE, B_CM_ID,B_T_TYPE, B_T_CODE,B_I_CODE, B_DATE, B_REASON, B_ACTION, B_CLOSURE, B_HOURS, B_FILE,B_DRAWING_UPLOAD)VALUES('" + txtBNo.Text + "','" + txtSlipNo.Text.Trim().Replace("'", "\''") + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Convert.ToInt32(Session["CompanyId"]) + "' ,'" + ddlType.SelectedValue + "','" + ddlTool.SelectedValue + "','" + ddlPart.SelectedValue + "','" + Convert.ToDateTime(txtBreakDownDate.Text).ToString("dd/MMM/yyyy HH:mm:ss tt") + "', '" + txtReason.Text.Trim().Replace("'", "\''") + "', '" + txtAction.Text.Trim().Replace("'", "\''") + "','" + Convert.ToDateTime(txtClosureDate.Text).ToString("dd/MMM/yyyy HH:mm:ss tt") + "','" + txtHours.Text + "','" + lnkupload.Text + "','" + lnkFUDrawingTR.Text + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(B_CODE) from BREAKDOWN_ENTRY");
                    if (ViewState["fileName"].ToString().Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/BreakDown/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/Maintaince");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\Maintaince ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/Maintaince/" + ViewState["fileName"].ToString().Trim());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/BreakDown/" + Code + "/" + ViewState["fileName"].ToString().Trim());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    if (ViewState["fileName2"].ToString().Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/Drawingupload/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/Drawingupload");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\Drawingupload ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/Drawingupload/" + ViewState["fileName2"].ToString().Trim());
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath/Drawingupload/" + Code + "/" + ViewState["fileName2"].ToString().Trim());
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    CommonClasses.WriteLog("Breakdown Entry", "Save", "Breakdown Entry", txtBNo.Text, Convert.ToInt32(txtBNo.Text), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/ToolRoom/View/ViewBreakdown.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtBNo.Focus();
                }
                //}
                //else
                //{
                //    ShowMessage("#Avisos", "BreakDown Slip No. Already Exists", CommonClasses.MSG_Warning);
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //}
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                //dt = CommonClasses.Execute(" SELECT * FROM BREAKDOWN_ENTRY WHERE lower(B_SLIPNO)= lower('" + txtSlipNo.Text.Trim().Replace("'", "\''") + "')  AND B_CODE!='" + ViewState["mlCode"].ToString() + "' and ES_DELETE='False'");
                //if (dt.Rows.Count == 0)
                //{
                if (CommonClasses.Execute1("UPDATE BREAKDOWN_ENTRY SET B_SLIPNO='" + txtSlipNo.Text.Trim().Replace("'", "\''") + "',B_T_TYPE='" + ddlType.SelectedValue + "',B_T_CODE='" + ddlTool.SelectedValue + "',B_I_CODE='" + ddlPart.SelectedValue + "',B_DATE='" + Convert.ToDateTime(txtBreakDownDate.Text).ToString("dd/MMM/yyyy HH:MM:ss tt") + "',B_REASON='" + txtReason.Text.Trim().Replace("'", "\''") + "'   ,B_ACTION='" + txtAction.Text.Trim().Replace("'", "\''") + "',B_CLOSURE='" + Convert.ToDateTime(txtClosureDate.Text).ToString("dd/MMM/yyyy HH:MM:ss tt") + "',B_HOURS='" + txtHours.Text + "',B_FILE='" + lnkupload.Text + "',B_DRAWING_UPLOAD='" + lnkFUDrawingTR.Text + "' WHERE B_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("BREAKDOWN_ENTRY", "ES_MODIFY", "B_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("Breakdown Entry", "Update", "Breakdown Entry", txtBNo.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/ToolRoom/View/ViewBreakdown.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtBNo.Focus();
                }
                //}
                //else
                //{
                //    ShowMessage("#Avisos", "BreakDown Slip No Already Exists", CommonClasses.MSG_Warning);
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    txtBNo.Focus();
                //}
            }
            #endregion MODIFY

            #region UpdateStatus
            else if (Request.QueryString[0].Equals("UpdateStatus"))
            {

                if (CommonClasses.Execute1("UPDATE BREAKDOWN_ENTRY SET B_ACTION='" + txtAction.Text.Trim().Replace("'", "\''") + "',B_CLOSURE='" + Convert.ToDateTime(txtClosureDate.Text).ToString("dd/MMM/yyyy HH:mm:ss tt") + "',B_HOURS='" + txtHours.Text + "',B_FILE='" + lnkupload.Text + "',B_STATUS=1 WHERE B_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("BREAKDOWN_ENTRY", "ES_MODIFY", "B_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("Breakdown Entry", "UpdateStatus", "Breakdown Entry", txtBNo.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/ToolRoom/View/ViewBreakdown.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtBNo.Focus();
                }

            }
            #endregion UpdateStatus
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Breakdown Entry", "GetIP4Address", ex.Message);
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
            CommonClasses.SendError("Breakdown Entry", "ShowMessage", Ex.Message);
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

    #region ddlTool_SelectedIndexChanged
    protected void ddlTool_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadPart();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "ddlTool_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    protected void ddlPart_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTool();
    }

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadTool();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "ddlType_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/Maintaince/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/BreakDown/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/Maintaince/" + ViewState["fileName"].ToString()));
        }
        else
        {
            FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/BreakDown/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
        }
    }
    #endregion

    #region Upload1
    protected void Upload1(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/Drawingupload/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/Drawingupload/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            FUDrawingTR.SaveAs(Server.MapPath("~/UpLoadPath/Drawingupload/" + ViewState["fileName2"].ToString()));
        }
        else
        {
            FUDrawingTR.SaveAs(Server.MapPath("~/UpLoadPath/Drawingupload/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
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
            // int height = Convert.ToInt32(FileUpload1.Height);

            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/Maintaince/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/BreakDown/" + code + "/" + filePath;

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
                directory = "../../UpLoadPath/Drawingupload/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkFUDrawingTR.Text;
                directory = "../../UpLoadPath/Drawingupload/" + code + "/" + filePath;
            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkFUDrawingTR_Click", ex.Message);
        }
    }
    #endregion

    #region Hours
    private void Hours()
    {
        //txtHours.Text = (Convert.ToDateTime(txtClosureDate.Text) - Convert.ToDateTime(txtBreakDownDate.Text)).TotalHours.ToString("N2");
        // TimeSpan t = Convert.ToDateTime(txtClosureDate.Text) - Convert.ToDateTime(txtBreakDownDate.Text);
        //var t1 = Convert.ToDateTime(txtClosureDate.Text)(Convert.ToDateTime(txtBreakDownDate.Text)).TotalHours;

        System.TimeSpan diff = Convert.ToDateTime(txtClosureDate.Text).Subtract((Convert.ToDateTime(txtBreakDownDate.Text)));
        //double DiffHours = t1.TotalHours;
        txtHours.Text = diff.ToString();//DiffHours.ToString();
    }
    #endregion Hours

    #region txtClosureDate_TextChanged
    protected void txtClosureDate_TextChanged(object sender, EventArgs e)
    {
        DateTime myDate;
        if (DateTime.TryParse(txtClosureDate.Text, out myDate))
        {
            //date ok!
        }
        else
        {
            //not valid datetime - not ok!
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Time Must Be less than or equal to 23:59 Hours..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtClosureDate.Focus();
            return;
        }
        DateTime dtDate1 = Convert.ToDateTime(txtBreakDownDate.Text);
        DateTime dtDate = Convert.ToDateTime(txtClosureDate.Text);

        if (dtDate1 < Convert.ToDateTime(Session["OpeningDate"]) || dtDate1 > Convert.ToDateTime(Session["ClosingDate"]))
        {
            txtClosureDate.Text = System.DateTime.Now.ToString("MMM yyyy");// Set Bydefault current date
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Date Must Be In Between Financial Year..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtClosureDate.Focus(); Hours();
            return;
        }
        if (dtDate1 <= dtDate)
        { }
        else
        {
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Closure date should be Greater than BreakDown Date..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtClosureDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
        }
        Hours();
    }
    #endregion txtClosureDate_TextChanged

    #region txtBreakDownDate_TextChanged
    protected void txtBreakDownDate_TextChanged(object sender, EventArgs e)
    {
        DateTime myDate;
        if (DateTime.TryParse(txtBreakDownDate.Text, out myDate))
        {
            //date ok!
        }
        else
        {
            //not valid datetime - not ok!
            PanelMsg.Visible = true; lblmsg.Visible = true;
            lblmsg.Text = "Time Must Be less than or equal to 23:59 Hours..";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtBreakDownDate.Focus();
            return;
        }
    }
    #endregion txtBreakDownDate_TextChanged
}
