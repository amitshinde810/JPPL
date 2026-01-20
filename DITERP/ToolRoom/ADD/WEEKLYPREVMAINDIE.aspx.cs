using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;

public partial class ToolRoom_ADD_WEEKLYPREVMAINDIE : System.Web.UI.Page
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
                    ViewState["mlCode"] = mlCode;
                    txtMonth.Attributes.Add("readonly", "readonly");
                    txtReceivedDate.Attributes.Add("readonly", "readonly");
                    txtECompDate.Attributes.Add("readonly", "readonly");
                    txtcompetedDate.Attributes.Add("readonly", "readonly");
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        rep.Visible = true;
                        CDate.Visible = true;
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                        //rep.Visible = true;
                        //CDate.Visible = true;
                        if (txtcompetedDate.Text != "")
                        {
                            txtcompetedDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                        }
                    }
                    else if (Request.QueryString[0].Equals("UpdateStatus"))
                    {
                        txtMonth.Enabled = false;
                        ddlWeekly.Enabled = false;
                        ddlToolNo.Enabled = false;

                        txtECompDate.Enabled = false;

                        txtcompetedDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("UPDATE");
                    }
                    else
                    {
                        txtMonth.Text = System.DateTime.Now.ToString("MMM/yyyy");
                        txtMonth_TextChanged(null, null);
                        txtReceivedDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy  HH:mm");
                        txtECompDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                        txtcompetedDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                        lblReason.Visible = false;
                        ddlReason.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Weekly Preventive maintenance", "PageLoad", ex.Message);
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
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlWeekly.SelectedValue == "0" && (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) >= Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("07/MMM/yyyy"))))
            if (ddlWeekly.SelectedValue == "0" && (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) > Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("07/MMM/yyyy")) || (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) < Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("01/MMM/yyyy")))))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Current Week";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlWeekly.Focus();
                return;
            }
            if (ddlWeekly.SelectedValue == "1" && (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) > Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("15/MMM/yyyy")) || (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) < Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("08/MMM/yyyy")))))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Current Week";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlWeekly.Focus();
                ddlWeekly.SelectedIndex = -1;
                return;
            }
            if (ddlWeekly.SelectedValue == "2" && (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) > Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("23/MMM/yyyy")) || (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) < Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("16/MMM/yyyy")))))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Current Week";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlWeekly.Focus();
                return;
            }
            //txtmonth Replace by txtReceivedDate Change by Sujata
            //if (ddlWeekly.SelectedValue == "3" && (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) > Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("31/MMM/yyyy")) || (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) < Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("24/MMM/yyyy")))))
            if (ddlWeekly.SelectedValue == "3" && (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) > Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy")) || (Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy")) < Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("24/MMM/yyyy")))))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select Current Week";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlWeekly.Focus();
                return;
            }
            if (Convert.ToDateTime(txtECompDate.Text) < Convert.ToDateTime(txtMonth.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Expected Date Must be greater than Received Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtECompDate.Focus();
                return;
            }
            if (ddlToolNo.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Tool No.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlToolNo.Focus();
                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Weekly Preventive maintenance", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    //when Completed date is greater than Expected Completion date then show reason dropdown 
    protected void txtcompetedDate_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        // string WPM_CODE = ((Label)(dgWPM.Rows[0].FindControl("lblWPM_CODE"))).Text;

        //dt = CommonClasses.Execute("select WPM_CODE,WPM_TOOL_RECEIVED_DATE from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");
        //string RecDate = Convert.ToDateTime(dt.Rows[0]["WPM_TOOL_RECEIVED_DATE"]).ToString("dd MMM yyyy");
        PanelMsg.Visible = false;
        if (Convert.ToDateTime(txtcompetedDate.Text) > Convert.ToDateTime(txtECompDate.Text))
        {
            lblReason.Visible = true;
            ddlReason.Visible = true;
            // divReason.Visible = true;
            DataTable dtReason = CommonClasses.Execute("select PMR_CODE,PMR_REASON from PM_REASON_MASTER where ES_DELETE=0 and PMR_CM_COMP_ID=1");
            if (dtReason.Rows.Count > 0)
            {
                ddlReason.DataSource = dtReason;
                ddlReason.DataTextField = "PMR_REASON";
                ddlReason.DataValueField = "PMR_CODE";
                ddlReason.DataBind();
                ddlReason.Items.Insert(0, new ListItem("Select Reason", "0"));
            }
        }
        else
        {
            lblReason.Visible = false;
            ddlReason.Visible = false;
        }

        DateTime dtDate1 = Convert.ToDateTime(Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy"));
        DateTime dtDate = Convert.ToDateTime(txtcompetedDate.Text);

        if (dtDate1 <= dtDate)
        { }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Completed date should not be Less than HandOver Date.";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtECompDate.Focus();
            return;
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Weekly Preventive maintenance", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select WPM_CODE,WPM_SLIP_NO,WPM_T_CODE,WPM_MONTH,WPM_UPLOAD_FILE,WPM_REASON_CODE,WPM_TOOL_EXPCOMP_DATE,isnull(WPM_TOOL_COMPLETED_DATE,GETDATE()) AS WPM_TOOL_COMPLETED_DATE,WPM_WEEK,WPM_TOOL_RECEIVED_DATE,WPM_STATUS FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where WPM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " AND WPM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["WPM_CODE"]);
                txtslipNo.Text = dt.Rows[0]["WPM_SLIP_NO"].ToString();
                txtMonth.Text = Convert.ToDateTime(dt.Rows[0]["WPM_MONTH"]).ToString("MMM yyyy");
                LoadWeeks();
                ddlWeekly.SelectedValue = dt.Rows[0]["WPM_WEEK"].ToString();
                LoadToolNo();
                ddlToolNo.SelectedValue = dt.Rows[0]["WPM_T_CODE"].ToString();
                txtReceivedDate.Text = Convert.ToDateTime(dt.Rows[0]["WPM_TOOL_RECEIVED_DATE"]).ToString("dd MMM yyyy");
                txtECompDate.Text = Convert.ToDateTime(dt.Rows[0]["WPM_TOOL_EXPCOMP_DATE"]).ToString("dd MMM yyyy");
                lnkupload.Text = dt.Rows[0]["WPM_UPLOAD_FILE"].ToString();

                if (dt.Rows[0]["WPM_STATUS"].ToString().ToUpper() == "FALSE")
                {
                    CDate.Visible = false;
                    rep.Visible = false;
                }
                if (str == "VIEW")
                {
                    txtReceivedDate.Enabled = false;
                    txtcompetedDate.Text = Convert.ToDateTime(dt.Rows[0]["WPM_TOOL_COMPLETED_DATE"]).ToString("dd MMM yyyy");
                    txtcompetedDate_TextChanged(null, null);
                    ddlReason.SelectedValue = dt.Rows[0]["WPM_REASON_CODE"].ToString();
                    txtReceivedDate.Enabled = false;
                    txtslipNo.Enabled = false;
                    ddlToolNo.Enabled = false;
                    ddlWeekly.Enabled = false;
                    txtMonth.Enabled = false;
                    btnSubmit.Visible = false;
                    ddlReason.Enabled = false;
                    ddlReason.Visible = true;
                    lblReason.Visible = true;
                    txtECompDate.Enabled = false;
                    txtcompetedDate.Enabled = false;
                    FileUpload1.Enabled = false;
                }
                if (str == "UPDATE")
                {
                    txtReceivedDate.Enabled = false;
                    txtcompetedDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                    txtcompetedDate_TextChanged(null, null);
                    CDate.Visible = true;
                    rep.Visible = true;
                    txtslipNo.Enabled = false;
                    //if (txtcompetedDate.Text != "")
                    //{
                    //    txtcompetedDate.Text = Convert.ToDateTime(dt.Rows[0]["WPM_TOOL_COMPLETED_DATE"]).ToString("dd MMM yyyy");
                    //}
                    //ddlReason.SelectedValue = dt.Rows[0]["WPM_REASON_CODE"].ToString();

                    CommonClasses.SetModifyLock("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "WPM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    // btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    CDate.Visible = false; txtReceivedDate.Enabled = false;
                    txtslipNo.Enabled = false;
                    rep.Visible = false;
                    CommonClasses.SetModifyLock("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "WPM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Weekly Preventive maintenance", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        string sDirPath = "";
        if (Request.QueryString[0].Equals("INSERT"))
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/WEEKLYPMAIN/");
        }
        else
        {
            sDirPath = Server.MapPath(@"~/UpLoadPath/WEEKLYPMAIN/" + ViewState["mlCode"].ToString() + "/");
        }

        ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
        if (Request.QueryString[0].Equals("INSERT"))
        {
            FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/WEEKLYPMAIN/" + ViewState["fileName"].ToString()));
        }
        else
        {//WEEKLYPMAIN
            //FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/ToolRoom/3DModelStoragenRev/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
            FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/WEEKLYPMAIN/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
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
                DataTable dt1 = new DataTable();
                dt1 = CommonClasses.Execute("SELECT ISNULL(MAX(WPM_SLIP_NO),0)+1 AS WPM_SLIP_NO FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER WHERE WPM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"].ToString()) + "' AND ES_DELETE='False'");
                if (dt1.Rows.Count > 0)
                {
                    txtslipNo.Text = dt1.Rows[0][0].ToString();
                }
                SqlCommand command = new SqlCommand("INSERT INTO WEEKLY_PREVENTIVE_MAINTENANCE_MASTER (WPM_SLIP_NO,WPM_MONTH,WPM_WEEK,WPM_T_CODE,WPM_TOOL_RECEIVED_DATE,WPM_CM_COMP_ID,WPM_UPLOAD_FILE,WPM_TOOL_EXPCOMP_DATE)VALUES ('" + txtslipNo.Text + "','" + Convert.ToDateTime(txtMonth.Text).ToString("01/MMM/yyyy") + "','" + ddlWeekly.SelectedValue + "','" + ddlToolNo.SelectedValue.ToString() + "','" + Convert.ToDateTime(txtReceivedDate.Text).ToString("dd/MMM/yyyy hh:mm") + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + lnkupload.Text + "','" + Convert.ToDateTime(txtECompDate.Text).ToString("dd/MMM/yyyy") + "')", connection, trans);
                command.ExecuteNonQuery();

                //string Code = CommonClasses.GetMaxId("Select Max(WPM_CODE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER");

                SqlCommand cmd1 = new SqlCommand("Select Max(WPM_CODE) from WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", connection, trans);
                cmd1.Transaction = trans;
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    Code = (dr1[0].ToString().Trim());
                }
                cmd1.Dispose();
                dr1.Dispose();

                if (ViewState["fileName"].ToString().Trim() != "")
                {
                    string sDirPath = Server.MapPath(@"~/UpLoadPath/WEEKLYPMAIN/" + Code + "");

                    ObjSearchDir = new DirectoryInfo(sDirPath);

                    string sDirPath1 = Server.MapPath(@"~/UpLoadPath/WeeklyPM");
                    DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                    dir.Refresh();

                    if (!ObjSearchDir.Exists)
                    {
                        ObjSearchDir.Create();
                    }
                    string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                    //Get the full path of the file    
                    string fullFilePath1 = currentApplicationPath + "UpLoadPath\\WeeklyPM ";
                    string fullFilePath = Server.MapPath(@"~/UpLoadPath/WeeklyPM/" + ViewState["fileName"].ToString().Trim());
                    // Get the destination path
                    string copyToPath = Server.MapPath(@"~/UpLoadPath/WEEKLYPMAIN/" + Code + "/" + ViewState["fileName"].ToString().Trim());
                    DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                    System.IO.File.Move(fullFilePath, copyToPath);
                }

                trans.Commit();
                CommonClasses.WriteLog("Weekly Preventive maintenance", "Save", "Weekly Preventive maintenance", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/WEEKLYPREVMAINDIE.aspx", false);
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand("UPDATE WEEKLY_PREVENTIVE_MAINTENANCE_MASTER SET WPM_MONTH='" + Convert.ToDateTime(txtMonth.Text).ToString("01/MMM/yyyy") + "', WPM_WEEK='" + ddlWeekly.SelectedValue + "', WPM_T_CODE='" + ddlToolNo.SelectedValue + "',WPM_TOOL_EXPCOMP_DATE='" + Convert.ToDateTime(txtECompDate.Text).ToString("dd/MMM/yyyy") + "' WHERE WPM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'", connection, trans);
                command.ExecuteNonQuery();

                //dt = CommonClasses.Execute("Update WEEKLY_PREVENTIVE_MAINTENANCE_MASTER set where WPM_CODE=''" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                trans.Commit();
                CommonClasses.RemoveModifyLock("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "WPM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));

                CommonClasses.WriteLog("Weekly Preventive maintenance", "Update", "Weekly Preventive maintenance", ddlToolNo.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/ToolRoom/VIEW/WEEKLYPREVMAINDIE.aspx", false);
            }
            #region UpdateStatus
            else if (Request.QueryString[0].Equals("UpdateStatus"))
            {

                if (CommonClasses.Execute1("UPDATE WEEKLY_PREVENTIVE_MAINTENANCE_MASTER SET WPM_REASON_CODE='" + ddlReason.SelectedValue + "',WPM_TOOL_COMPLETED_DATE='" + Convert.ToDateTime(txtcompetedDate.Text).ToString("dd/MMM/yyyy") + "',WPM_UPLOAD_FILE='" + lnkupload.Text + "' ,WPM_STATUS=1 WHERE WPM_CODE='" + ViewState["mlCode"].ToString() + "'"))
                {
                    CommonClasses.RemoveModifyLock("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "WPM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("Weekly Preventive maintenance", "UpdateStatus", "Weekly Preventive maintenance", txtcompetedDate.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/ToolRoom/View/WEEKLYPREVMAINDIE.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Invalid Update";
                    // ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtcompetedDate.Focus();
                }

            }
            #endregion UpdateStatus
        }
        catch (Exception ex)
        {
            trans.Rollback();
            CommonClasses.SendError("Weekly Preventive maintenance", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Weekly Preventive maintenance", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //Directory = string.Empty;
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Weekly Preventive maintenance", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    private void LoadWeeks()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select distinct PM_WEEK,(CASE PM_WEEK when '0' then '1 WEEK' when '1' then '2 WEEK' when '2' then '3 WEEK' when '3' then '4 WEEK' else 'PM_WEEK' END) as PM_WEEKNM from MP_PREVENTIVE_MAINTENANCE_MASTER where ES_DELETE=0 and PM_CM_COMP_ID=" + Session["CompanyID"] + " and DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "'and DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' ");
        if (dt.Rows.Count > 0)
        {
            ddlWeekly.DataSource = dt;
            ddlWeekly.DataTextField = "PM_WEEKNM";
            ddlWeekly.DataValueField = "PM_WEEK";
            ddlWeekly.DataBind();
            //ddlWeekly.Items.Insert(0, new ListItem("Select Week", "0"));
        }
        else
        {
            ddlWeekly.DataSource = dt;
            ddlWeekly.DataBind();
        }
    }

    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        LoadWeeks();
        ddlWeek1_SelectedIndexChanged(null, null);
    }

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("WEEKLY_PREVENTIVE_MAINTENANCE_MASTER", "ES_MODIFY", "WPM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/ToolRoom/VIEW/WEEKLYPREVMAINDIE.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Weekly Preventive maintenance", "CancelRecord", ex.Message.ToString());
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
            CommonClasses.SendError("Weekly Preventive maintenance", "CheckValid", Ex.Message);
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
        str = str + "PM_WEEK<='" + ddlWeekly.SelectedValue + "' AND ";

        if (Request.QueryString[0].Equals("INSERT"))
        {
            dt = CommonClasses.Execute("select T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME from TOOL_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER,ITEM_MASTER where " + str + " MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_STATUS=0 AND PM_TOOL_NO=T_CODE and T_CM_COMP_ID=" + Session["CompanyID"] + " and DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "'and DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' and TOOL_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 AND T_CODE NOT IN (SELECT WPM_T_CODE FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where DATEPART(MM,WPM_MONTH )='" + Convert.ToDateTime(txtMonth.Text).Month + "'and DATEPART(YYYY,WPM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "'  AND ES_DELETE=0) order by T_NAME");
        }
        else
        {
            dt = CommonClasses.Execute("select T_CODE,CASE WHEN T_TYPE=0 THEN 'DIE' ELSE 'CORE BOX' END  +'- '+T_NAME+' PART NO :- '+I_CODENO+' PART NAME :- '+I_NAME AS T_NAME from TOOL_MASTER,MP_PREVENTIVE_MAINTENANCE_MASTER,ITEM_MASTER where " + str + " MP_PREVENTIVE_MAINTENANCE_MASTER.ES_DELETE=0 AND PM_STATUS=0 AND PM_TOOL_NO=T_CODE and T_CM_COMP_ID=" + Session["CompanyID"] + " and DATEPART(MM,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Month + "' and DATEPART(YYYY,PM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' and TOOL_MASTER.ES_DELETE=0 AND T_I_CODE=I_CODE AND ITEM_MASTER.ES_DELETE=0 order by T_NAME");//AND T_CODE NOT IN (SELECT WPM_T_CODE  FROM WEEKLY_PREVENTIVE_MAINTENANCE_MASTER where DATEPART(MM,WPM_MONTH )='" + Convert.ToDateTime(txtMonth.Text).Month + "'and DATEPART(YYYY,WPM_MONTH)='" + Convert.ToDateTime(txtMonth.Text).Year + "' AND ES_DELETE=0)
        }
        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
        else
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataBind();
            ddlToolNo.Items.Insert(0, new ListItem("Select Tool No.", "0"));
        }
    }

    protected void ddlWeek1_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadToolNo();
    }

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
                directory = "../../UpLoadPath/WEEKLYPMAIN/" + filePath;
            }
            else
            {
                code = ViewState["mlCode"].ToString();
                filePath = lnkupload.Text;
                lnkupload.Visible = true;
                directory = "../../UpLoadPath/WEEKLYPMAIN/" + code + "/" + filePath;

            }
            ModalPopDocument.Show();
            IframeViewPDF.Attributes["src"] = directory;
            directory = String.Empty;
            //Response.Flush();
            // Response.End();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Breakdown Entry", "lnkupload_Click", ex.Message);
        }
    }
    #endregion

    protected void txtECompDate_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(txtECompDate.Text) < Convert.ToDateTime(txtMonth.Text))
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Expected Date Must be greater than Received Date.";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtECompDate.Focus();
            return;
        }
    }

    #region txtReceivedDate_TextChanged
    protected void txtReceivedDate_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion txtReceivedDate_TextChanged
}

