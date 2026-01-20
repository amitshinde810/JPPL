using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Add_ISOMaster : System.Web.UI.Page
{
    #region General Declaration
    static int mlCode = 0;
    static string right = "";
    public static string str = "";
    DataTable dt = new DataTable();
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='80'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadScreen();
                str = "";
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
                else if (Request.QueryString[0].Equals("INSERT"))
                {
                    txtDate.Attributes.Add("readonly", "readonly");
                    txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("ISO Master", "Page_Load", Ex.Message);
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    }
    #endregion

    #region btnClose_Click
    #endregion btnClose_Click

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
            CommonClasses.SendError("ISO Master", "btnCancel_Click", ex.Message.ToString());
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtISOno.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlScreenName.SelectedIndex <= 0)
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
            CommonClasses.SendError("ISO Master", "CheckValid", Ex.Message);
        }
        return flag;
    }

    private void CancelRecord()
    {
        txtDate.Text = "";
        ddlScreenName.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("ISONO_MASTER", "MODIFY", "ISO_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/ViewStateMaster.aspx", false);
    }
    #endregion

    #region ClearFied
    private void ClearFied()
    {
        try
        {
            ddlScreenName.SelectedIndex = 0;
            txtISOno.Text = "";
            txtDate.Text = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ISO Master", "ClearFied", Ex.Message);
        }
    }
    #endregion

    #region LoadScreen
    private void LoadScreen()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select SM_CODE,SM_NAME from SCREEN_MASTER order by SM_NAME");
                ddlScreenName.DataSource = dt;
                ddlScreenName.DataTextField = "SM_NAME";
                ddlScreenName.DataValueField = "SM_CODE";
                ddlScreenName.DataBind();
                ddlScreenName.Items.Insert(0, new ListItem("Select Screen Name", "0"));

            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ISO Master", "LoadScreen", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadScreen();
            dt = CommonClasses.Execute("Select * from ISONO_MASTER where ISO_COMP_ID='" + (string)Session["CompanyId"] + "'  and ISO_CODE='" + mlCode + "'");
            if (dt.Rows.Count > 0)
            {
                ddlScreenName.SelectedValue = dt.Rows[0]["ISO_SCREEN_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["ISO_WEF_DATE"]).ToString("dd MMM yyyy");
                txtISOno.Text = dt.Rows[0]["ISO_NO"].ToString();
            }
            if (str == "MOD")
            {
                ddlScreenName.Enabled = false;
                CommonClasses.SetModifyLock("ISONO_MASTER", "MODIFY", "ISO_CODE", mlCode);
            }
            if (str == "VIEW")
            {
                ddlScreenName.Enabled = false;
                txtDate.Enabled = false;
                txtISOno.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ISO Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dtISO = CommonClasses.Execute("select * from ISONO_MASTER WHERE ISO_SCREEN_NO='" + ddlScreenName.SelectedValue + "' and ISO_WEF_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' and ISO_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                if (dtISO.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO ISONO_MASTER (ISO_COMP_ID,ISO_SCREEN_NO,ISO_WEF_DATE,ISO_NO)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlScreenName.SelectedValue + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + txtISOno.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(ISO_CODE) from ISONO_MASTER");

                        CommonClasses.WriteLog("ISO Matser", "Save", "ISO Master", Convert.ToString(ddlScreenName.SelectedValue), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/IsoNoMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlScreenName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record is already Exist", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlScreenName.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE ISONO_MASTER SET ISO_SCREEN_NO='" + ddlScreenName.SelectedValue + "',ISO_WEF_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',ISO_NO='" + txtISOno.Text + "' where ISO_CODE='" + mlCode + "'"))
                {
                    CommonClasses.RemoveModifyLock("CUSTPO_DETAIL", "MODIFY", "ISO_CODE", mlCode);
                    CommonClasses.WriteLog("ISO Matser", "Save", "ISO Master", Convert.ToString(ddlScreenName.SelectedValue), Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Admin/View/IsoNoMaster.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlScreenName.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("ISO Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("ISO Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        txtISOno.Text = "";
        txtDate.Text = "";
        ddlScreenName.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("ISONO_MASTER", "MODIFY", "ISO_CODE", mlCode);
        }
        Response.Redirect("~/Admin/View/IsoNoMaster.aspx", false);
    }
    #endregion
}
