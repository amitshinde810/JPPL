# region Liabraries
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
#endregion

public partial class Admin_Add_TallyMaster : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    # endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
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
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Tally Master", "Imgclose_Click", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
        }
    }
    #region Events
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtTallyName.Text.Trim() == "")
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "The Field 'Tally Name' is Required";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        else
        {
            SaveRec();
        }
    }
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
            CommonClasses.SendError("Tally Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("TALLY_MASTER", "MODIFY", "TALLY_CODE", mlCode);
            }
            Response.Redirect("~/Admin/VIEW/ViewTallyMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Master", "btnCancel_Click", ex.Message);
        }
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtTallyName.Text.Trim() == "")
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
            CommonClasses.SendError("Tally Master", "CheckValid", Ex.Message);
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
    #endregion
    #region Function
    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM TALLY_MASTER WHERE TALLY_CODE=" + mlCode + " AND TALLY_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
                txtTallyName.Text = dt.Rows[0]["TALLY_NAME"].ToString();

                if (str == "VIEW")
                {
                    txtTallyName.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("TALLY_MASTER", "MODIFY", "TALLY_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec
    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string strName = txtTallyName.Text.Trim();
            strName = strName.Replace("'", "\''");
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select TALLY_CODE,TALLY_NAME FROM TALLY_MASTER WHERE lower(TALLY_NAME)= lower('" + strName + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO TALLY_MASTER (TALLY_COMP_ID,TALLY_NAME)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + strName + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(TALLY_CODE) from TALLY_MASTER");
                        CommonClasses.WriteLog("TALLY MASTER", "Save", "TALLY MASTER", strName, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewTallyMaster.aspx", false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Could not saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtTallyName.Focus();
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Tally Name Already Exists";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM TALLY_MASTER WHERE ES_DELETE=0 AND TALLY_CODE!= '" + mlCode + "' AND lower(TALLY_NAME) = lower('" + strName + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE TALLY_MASTER SET TALLY_NAME='" + strName + "' WHERE TALLY_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("TALLY_MASTER", "MODIFY", "TALLY_CODE", mlCode);
                        CommonClasses.WriteLog("Tally Master", "Update", "Tally Master", strName, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewTallyMaster.aspx", false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Invalid Update";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtTallyName.Focus();
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Tally Name Already Exists";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtTallyName.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Master", "SaveRec", ex.Message);
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
    #endregion
}
