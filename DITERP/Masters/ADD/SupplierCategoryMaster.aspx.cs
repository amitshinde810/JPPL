using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Admin_Add_SupplierCategoryMaster : System.Web.UI.Page
{
    #region General Declaration
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='245'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
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
                    txtCategoryName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Supplier Category Master", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCategoryName.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field 'Category Name' is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Category Master", "btnSubmit_Click", Ex.Message);
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
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Category Master", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtCategoryName.Text.Trim() == "")
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
            CommonClasses.SendError("Supplier Category Master", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion CheckValid

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion btnOk_Click

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion btnCancel1_Click

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("SUPPLIER_CATEGORY_MASTER", "MODIFY", "SCM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/View/ViewSupplierCategoryMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Category Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion CancelRecord

    #endregion

    #region Methods

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM SUPPLIER_CATEGORY_MASTER WHERE SCM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND SCM_CODE='" + mlCode.ToString() + "'");
            txtCategoryName.Text = dt.Rows[0]["SCM_NAME"].ToString();
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SUPPLIER_CATEGORY_MASTER", "MODIFY", "SCM_CODE", mlCode);
            }
            if (str == "VIEW")
            {
                txtCategoryName.Enabled = false;
                btnSubmit.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Category Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            DataTable dtExist = new DataTable();
            string StrReplaceSctorName = txtCategoryName.Text;
            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");

            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dtExist = CommonClasses.Execute("SELECT * FROM SUPPLIER_CATEGORY_MASTER WHERE SCM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND lower(SCM_NAME)=lower('" + StrReplaceSctorName.Trim().ToString() + "')");
                if (dtExist.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into SUPPLIER_CATEGORY_MASTER (SCM_CM_COMP_ID,SCM_NAME) values ('" + Convert.ToInt32(Session["CompanyId"]) + "','" + StrReplaceSctorName.Trim().ToString() + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SCM_CODE) from SUPPLIER_CATEGORY_MASTER");
                        CommonClasses.WriteLog("Supplier Category Master", "Save", "Supplier Category Master", StrReplaceSctorName, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/ViewSupplierCategoryMaster.aspx", false);
                    }
                    else
                    {
                        lblmsg.Text = "Record Not Save";
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtCategoryName.Focus();
                    }
                }
                else
                {
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtCategoryName.Focus();
                }
            }
            #endregion Insert

            #region Modify
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtExist = CommonClasses.Execute("SELECT * FROM SUPPLIER_CATEGORY_MASTER WHERE SCM_CODE!='" + mlCode + "' AND SCM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND lower(SCM_NAME)=lower('" + StrReplaceSctorName.Trim().ToString() + "')");
                if (dtExist.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("update SUPPLIER_CATEGORY_MASTER set SCM_NAME='" + StrReplaceSctorName.Trim().ToString() + "' where SCM_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("SUPPLIER_CATEGORY_MASTER", "MODIFY", "SCM_CODE", mlCode);
                        CommonClasses.WriteLog("Supplier Category Master", "Update", "Supplier Category Master", StrReplaceSctorName, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/ViewSupplierCategoryMaster.aspx", false);
                    }
                    else
                    {
                        lblmsg.Text = "Record Not Save";
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtCategoryName.Focus();
                    }
                }
                else
                {
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtCategoryName.Focus();
                }
            }
        }
            #endregion Modify

        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Category Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Supplier Category Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion
}
