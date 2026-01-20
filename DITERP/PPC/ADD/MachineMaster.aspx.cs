using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_MachineMaster : System.Web.UI.Page
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
                        CommonClasses.SendError("Machine Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT M_CODE,M_NAME,M_DESCR FROM MACHINE_MASTER where M_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and M_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtDescription.Text = dt.Rows[0]["M_DESCR"].ToString();
                txtName.Text = dt.Rows[0]["M_NAME"].ToString();
                if (str == "VIEW")
                {
                    txtDescription.Enabled = false; txtName.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("MACHINE_MASTER", "MODIFY", "M_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (txtName.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter Name", CommonClasses.MSG_Warning);
            txtName.Focus();
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
                dt = CommonClasses.Execute("Select M_CODE,M_NAME FROM MACHINE_MASTER WHERE lower(M_NAME)= lower('" + txtName.Text.Trim().Replace("'", "\''") + "') AND M_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO MACHINE_MASTER(M_COMP_ID,M_NAME,M_DESCR)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtName.Text.Trim().Replace("'", "\''") + "','" + txtDescription.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(M_CODE) from MACHINE_MASTER");
                        CommonClasses.WriteLog("Machine Master", "Save", "Machine Master", txtName.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewMachineMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM MACHINE_MASTER WHERE ES_DELETE=0 AND M_CODE!= '" + mlCode + "' AND lower(M_NAME) = lower('" + txtName.Text.Trim() + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE MACHINE_MASTER SET M_NAME='" + txtName.Text.Trim().Replace("'", "\''") + "',M_DESCR='" + txtDescription.Text.Trim().Replace("'", "\''") + "' WHERE M_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("MACHINE_MASTER", "MODIFY", "M_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Machine Master", "Update", "Machine Master", txtName.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewMachineMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("MACHINE_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Machine Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("MACHINE_MASTER", "MODIFY", "M_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewMachineMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machine Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtName.Text.Trim() == "")
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
            CommonClasses.SendError("Machine Master", "CheckValid", Ex.Message);
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

}
