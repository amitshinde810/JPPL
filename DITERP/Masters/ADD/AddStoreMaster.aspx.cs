using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Masters_ADD_AddStoreMaster : System.Web.UI.Page
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
                        CommonClasses.SendError("Store Master", "Imgclose_Click", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master", "Imgclose_Click", ex.Message);
        }
    }

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            // BL_UserMaster = new UserMaster_BL(mlCode);
            DataTable dt = new DataTable();
            //29012019 :- Assign values to control from db against primary key
            dt = CommonClasses.Execute("SELECT * FROM STORE_MASTER WHERE STORE_CODE='" + ViewState["mlCode"].ToString() + "' AND STORE_COMP_ID= " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                txtStoreName.Text = dt.Rows[0]["STORE_NAME"].ToString();
                txtStorePre.Text = dt.Rows[0]["STORE_PREFIX"].ToString();
                txtStoreAddress.Text = dt.Rows[0]["STORE_ADDRESS"].ToString();
                if (str == "VIEW")
                {
                    txtStoreName.Enabled = false; txtStorePre.Enabled = false; txtStoreAddress.Enabled = false;
                    btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                txtStoreName.Enabled = false; txtStorePre.Enabled = false;
                CommonClasses.SetModifyLock("STORE_MASTER", "MODIFY", "STORE_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Store Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtStoreName.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "The Field 'Store Name' is Required", CommonClasses.MSG_Warning);

            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        if (txtStorePre.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "The Field 'Store Prefix' is Required", CommonClasses.MSG_Warning);

            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        if (txtStoreAddress.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "The Field 'Store Prefix' is Required", CommonClasses.MSG_Warning);

            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        else
        {
            SaveRec();
        }
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string strName = txtStoreName.Text.Trim();
            strName = strName.Replace("'", "\''");
            string strNamePre = txtStorePre.Text.Trim();
            strNamePre = strNamePre.Replace("'", "\''");
            string StoreAddress = txtStoreAddress.Text.Trim();
            StoreAddress = StoreAddress.Replace("'", "\''");

            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select STORE_CODE,STORE_NAME,STORE_PREFIX FROM STORE_MASTER WHERE lower(STORE_NAME)= lower('" + strName + "') and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO STORE_MASTER (STORE_COMP_ID,STORE_NAME,STORE_PREFIX,STORE_ADDRESS)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + strName + "','" + strNamePre + "','" + StoreAddress + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(STORE_CODE) from STORE_MASTER");
                        CommonClasses.WriteLog("Store Master", "Save", "Store Master", strName, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/StoreMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtStoreName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Store Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region Modify
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM STORE_MASTER WHERE ES_DELETE=0 AND STORE_CODE!= '" + ViewState["mlCode"].ToString() + "' AND lower(STORE_NAME) = lower('" + strName + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE STORE_MASTER SET STORE_NAME='" + strName + "',STORE_PREFIX='" + strNamePre + "',STORE_ADDRESS ='" + StoreAddress + "' WHERE STORE_CODE='" + ViewState["mlCode"].ToString() + "'"))
                    {
                        CommonClasses.RemoveModifyLock("STORE_MASTER", "MODIFY", "STORE_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Store Master", "Update", "Store Master", strName, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/StoreMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtStoreName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Store Name Already Exists", CommonClasses.MSG_Warning);
                    txtStoreName.Focus();
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion Modify
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Store Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Store Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("STORE_MASTER", "MODIFY", "STORE_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/Masters/VIEW/StoreMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Store Master", "btnCancel_Click", ex.Message);
        }
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtStoreName.Text.Trim() == "")
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
            CommonClasses.SendError("Store Master", "CheckValid", Ex.Message);
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
}