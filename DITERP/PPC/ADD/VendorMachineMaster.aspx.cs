using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_VendorMachineMaster : System.Web.UI.Page
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
                        LoadVendor(); LoadMachines(); // Method call
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
            LoadVendor(); LoadMachines();
            //Check Hardcoded Sub Contractor (Sub,Both from Suplier Master)
            dt = CommonClasses.Execute("select VMM_CODE,P_CODE,P_NAME,M_NAME,M_CODE,VMM_NO_MACHINES from VENDOR_MACHINE_MASTER VM inner join PARTY_MASTER P ON VM.VMM_P_ID=P.P_CODE inner join MACHINE_MASTER M on VM.VMM_M_ID=M.M_CODE  where VMM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'and P_TYPE=2 and P_STM_CODE in(-2147483647,-2147483646) and VM.ES_DELETE=0 and P.ES_DELETE=0 and VMM_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtNumbersOfMachines.Text = dt.Rows[0]["VMM_NO_MACHINES"].ToString();
                ddlVendor.SelectedValue = dt.Rows[0]["P_CODE"].ToString();
                ddlMachine.SelectedValue = dt.Rows[0]["M_CODE"].ToString();
                if (str == "VIEW")
                {
                    txtNumbersOfMachines.Enabled = false; ddlVendor.Enabled = false; ddlMachine.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("VENDOR_MACHINE_MASTER", "MODIFY", "VMM_CODE", Convert.ToInt32(ViewState["mlCode"]));
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
        if (ddlVendor.SelectedIndex == -1 || ddlVendor.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Vendor Name", CommonClasses.MSG_Warning);
            ddlVendor.Focus();
            return;
        }
        if (ddlMachine.SelectedIndex == -1 || ddlMachine.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Machine Name", CommonClasses.MSG_Warning);
            ddlMachine.Focus();
            return;
        }
        if (txtNumbersOfMachines.Text.Trim() == "" || Convert.ToInt32(txtNumbersOfMachines.Text.Trim()) <= 0)
        {
            ShowMessage("#Avisos", "Please Insert Number", CommonClasses.MSG_Warning);
            ddlMachine.Focus();
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
                dt = CommonClasses.Execute("Select VMM_CODE,VMM_P_ID FROM VENDOR_MACHINE_MASTER WHERE VMM_P_ID= '" + ddlVendor.SelectedValue + "' AND VMM_M_ID= '" + ddlMachine.SelectedValue + "' AND VMM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO VENDOR_MACHINE_MASTER(VMM_COMP_ID,VMM_P_ID,VMM_M_ID,VMM_NO_MACHINES)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlVendor.SelectedValue + "','" + ddlMachine.SelectedValue + "','" + txtNumbersOfMachines.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(VMM_CODE) from VENDOR_MACHINE_MASTER");
                        CommonClasses.WriteLog("Process Machine Master", "Save", "Process Machine Master", ddlVendor.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewVendorMachineMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlVendor.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Vendor Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM VENDOR_MACHINE_MASTER WHERE ES_DELETE=0 AND VMM_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND VMM_P_ID= '" + ddlVendor.SelectedValue + "' AND VMM_M_ID= '" + ddlMachine.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE VENDOR_MACHINE_MASTER SET VMM_P_ID='" + ddlVendor.SelectedValue + "',VMM_M_ID='" + ddlMachine.SelectedValue + "',VMM_NO_MACHINES='" + txtNumbersOfMachines.Text.Trim().Replace("'", "\''") + "' WHERE VMM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("VENDOR_MACHINE_MASTER", "MODIFY", "VMM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Process Machine Master", "Update", "Process Machine Master", ddlVendor.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewVendorMachineMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlVendor.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Vendor Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlVendor.Focus();
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
            CommonClasses.SendError("VENDOR_MACHINE_MASTER", "ShowMessage", Ex.Message);
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
                CommonClasses.RemoveModifyLock("VENDOR_MACHINE_MASTER", "MODIFY", "VMM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewVendorMachineMaster.aspx", false);
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
            if (ddlVendor.SelectedIndex == -1)
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

    #region ddlVendor_SelectedIndexChanged
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlVendor_SelectedIndexChanged

    #region ddlMachine_SelectedIndexChanged
    protected void ddlMachine_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlMachine_SelectedIndexChanged

    #region LoadVendor
    protected void LoadVendor()
    {
        DataTable dtProcess = new DataTable();
        dtProcess = CommonClasses.Execute("select P_CODE,P_NAME from PARTY_MASTER where P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and P_TYPE=2 and P_STM_CODE in(-2147483647,-2147483646) and ES_DELETE=0 order by P_NAME");
        ddlVendor.DataSource = dtProcess;
        ddlVendor.DataTextField = "P_NAME";
        ddlVendor.DataValueField = "P_CODE";
        ddlVendor.DataBind();
        ddlVendor.Items.Insert(0, new ListItem("Select Vendor Name", "0"));
    }
    #endregion LoadVendor

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
