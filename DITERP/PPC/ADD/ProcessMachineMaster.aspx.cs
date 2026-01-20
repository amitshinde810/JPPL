using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_ProcessMachineMaster : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    # endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
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
                        ViewState["mlCode"] = "";
                        ViewState["mlCode"] = mlCode;
                        LoadProcess(); LoadMachines(); // Method call
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
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Process Machine Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Machine Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadProcess(); LoadMachines();
            dt = CommonClasses.Execute("select PMM_CODE,PMN_NAME,PMN_CODE,M_NAME,M_CODE,PMM_NO_MACHINES from PROCESS_MACHINE_MASTER PM inner join MACHINE_MASTER M on PM.PMM_M_ID=M.M_CODE INNER JOIN PROCESS_MASTER_NEW P ON PM.PMM_P_ID=P.PMN_CODE WHERE PM.ES_DELETE=0 and M.ES_DELETE=0 and P.ES_DELETE=0 and PMM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PMM_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtNumbersOfMachines.Text = dt.Rows[0]["PMM_NO_MACHINES"].ToString();
                ddlProcess.SelectedValue = dt.Rows[0]["PMN_CODE"].ToString();
                ddlMachine.SelectedValue = dt.Rows[0]["M_CODE"].ToString();
                if (str == "VIEW")
                {
                    txtNumbersOfMachines.Enabled = false; ddlProcess.Enabled = false; ddlMachine.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PROCESS_MACHINE_MASTER", "MODIFY", "PMM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Machine Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlProcess.SelectedIndex == -1 || ddlProcess.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Process Name", CommonClasses.MSG_Warning);
            ddlProcess.Focus();
            return;
        }
        if (ddlMachine.SelectedIndex == -1 || ddlMachine.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Machine Name", CommonClasses.MSG_Warning);
            ddlMachine.Focus();
            return;
        }
        if (txtNumbersOfMachines.Text.Trim() == "" || Convert.ToDouble(txtNumbersOfMachines.Text.Trim()) == 0)
        {
            ShowMessage("#Avisos", "Please Insert Number", CommonClasses.MSG_Warning);
            txtNumbersOfMachines.Focus();
            return;
        }
        #endregion

        SaveRec();
    }
    #endregion

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
                dt = CommonClasses.Execute("Select PMM_CODE,PMM_P_ID FROM PROCESS_MACHINE_MASTER WHERE PMM_P_ID= '" + ddlProcess.SelectedValue + "' AND PMM_M_ID= '" + ddlMachine.SelectedValue + "' AND PMM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO PROCESS_MACHINE_MASTER(PMM_COMP_ID,PMM_P_ID,PMM_M_ID,PMM_NO_MACHINES)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlProcess.SelectedValue + "','" + ddlMachine.SelectedValue + "','" + txtNumbersOfMachines.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PMM_CODE) from PROCESS_MACHINE_MASTER");
                        CommonClasses.WriteLog("Process Machine Master", "Save", "Process Machine Master", ddlProcess.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProcessMachineMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlProcess.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Process Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PROCESS_MACHINE_MASTER WHERE ES_DELETE=0 AND PMM_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND PMM_P_ID= '" + ddlProcess.SelectedValue + "' AND PMM_M_ID= '" + ddlMachine.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PROCESS_MACHINE_MASTER SET PMM_P_ID='" + ddlProcess.SelectedValue + "',PMM_M_ID='" + ddlMachine.SelectedValue + "',PMM_NO_MACHINES='" + txtNumbersOfMachines.Text.Trim().Replace("'", "\''") + "' WHERE PMM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PROCESS_MACHINE_MASTER", "MODIFY", "PMM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Process Machine Master", "Update", "Process Machine Master", ddlProcess.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProcessMachineMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlProcess.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Process Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlProcess.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Machine Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("PROCESS_MACHINE_MASTER", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
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
            CommonClasses.SendError("Process Machine Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("PROCESS_MACHINE_MASTER", "MODIFY", "PMM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewProcessMachineMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Machine Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlProcess.SelectedIndex == -1)
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
            CommonClasses.SendError("Process Machine Master", "CheckValid", Ex.Message);
        }

        return flag;
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

    #region ddlProcess_SelectedIndexChanged
    protected void ddlProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlProcess_SelectedIndexChanged

    #region ddlMachine_SelectedIndexChanged
    protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlMachine_SelectedIndexChanged

    #region LoadProcess
    protected void LoadProcess()
    {
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("select PMN_CODE,PMN_NAME from PROCESS_MASTER_NEW where ES_DELETE=0 and PMN_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by PMN_NAME");
        ddlProcess.DataSource = dtProcess;
        ddlProcess.DataTextField = "PMN_NAME";
        ddlProcess.DataValueField = "PMN_CODE";
        ddlProcess.DataBind();
        ddlProcess.Items.Insert(0, new ListItem("Select Process Name", "0"));
    }
    #endregion LoadProcess

    #region LoadMachines
    protected void LoadMachines()
    {
        DataTable dtMachines = new DataTable();
        dtMachines = CommonClasses.Execute("SELECT M_CODE,M_NAME,M_DESCR FROM MACHINE_MASTER where M_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 ORDER by M_NAME");
        ddlMachine.DataSource = dtMachines;
        ddlMachine.DataTextField = "M_NAME";
        ddlMachine.DataValueField = "M_CODE";
        ddlMachine.DataBind();
        ddlMachine.Items.Insert(0, new ListItem("Select Machine Name", "0"));
    }
    #endregion LoadMachines
}
