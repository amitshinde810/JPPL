using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_ReasonMaster : System.Web.UI.Page
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
                        LoadOwner();
                        ViewState["mlCode"] = "";
                        ViewState["mlCode"] = mlCode;
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
                        CommonClasses.SendError("Reason Master New", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reason Master New", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadOwner();
            dt = CommonClasses.Execute("select RMN_RTM_CODE,RMN_DESCR from REASON_MASTER_NEW INNER JOIN REASON_TYPE_MASTER on RMN_RTM_CODE=RTM_CODE where REASON_MASTER_NEW.ES_DELETE=0 and REASON_TYPE_MASTER.ES_DELETE=0 and RMN_COMP_ID='" + (string)Session["CompanyId"] + "' and RMN_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtDescription.Text = dt.Rows[0]["RMN_DESCR"].ToString();
                ddlType.SelectedValue = dt.Rows[0]["RMN_RTM_CODE"].ToString();
                if (str == "VIEW")
                {
                    txtDescription.Enabled = false; ddlType.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("REASON_MASTER_NEW", "MODIFY", "RMN_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reason Master New", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlType.SelectedIndex == -1 || ddlType.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Reason Type", CommonClasses.MSG_Warning);
            ddlType.Focus();
            return;
        }
        if (txtDescription.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter Reason Description", CommonClasses.MSG_Warning);
            ddlType.Focus();
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
                dt = CommonClasses.Execute("Select * FROM REASON_MASTER_NEW WHERE RMN_RTM_CODE='" + ddlType.SelectedValue + "' and RMN_DESCR='" + txtDescription.Text.Trim() + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO REASON_MASTER_NEW(RMN_COMP_ID,RMN_RTM_CODE,RMN_DESCR)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlType.SelectedValue + "','" + txtDescription.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(RMN_CODE) from REASON_MASTER_NEW");
                        CommonClasses.WriteLog("Reason Master New", "Save", "Reason Master New", txtDescription.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewReasonMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlType.Focus();
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
                dt = CommonClasses.Execute("SELECT * FROM REASON_MASTER_NEW WHERE ES_DELETE=0 AND RMN_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND RMN_RTM_CODE='" + ddlType.SelectedValue + "' and RMN_DESCR='" + txtDescription.Text.Trim() + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE REASON_MASTER_NEW SET RMN_RTM_CODE='" + ddlType.SelectedValue + "',RMN_DESCR='" + txtDescription.Text.Trim().Replace("'", "\''") + "' WHERE RMN_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("REASON_MASTER_NEW", "MODIFY", "RMN_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Reason Master New", "Update", "Reason Master New", ddlType.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewReasonMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlType.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Type Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlType.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reason Master New", "SaveRec", ex.Message);
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
            CommonClasses.SendError("REASON_MASTER_NEW", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Reason Master New", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("REASON_MASTER_NEW", "MODIFY", "RMN_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewReasonMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reason Master New", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlType.SelectedIndex == -1)
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
            CommonClasses.SendError("Reason Master New", "CheckValid", Ex.Message);
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

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlType_SelectedIndexChanged

    #region LoadOwner
    protected void LoadOwner()
    {
        DataTable dtType = new DataTable();
        dtType = CommonClasses.Execute("select DISTINCT RTM_CODE,RTM_TYPE from REASON_TYPE_MASTER where RTM_COMP_ID=1 and ES_DELETE=0 ORDER BY RTM_TYPE");
        ddlType.DataSource = dtType;
        ddlType.DataTextField = "RTM_TYPE";
        ddlType.DataValueField = "RTM_CODE";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("Select Reason Type", "0"));
    }
    #endregion LoadOwner
}
