using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Admin_Add_SupplierPerfRemark : System.Web.UI.Page
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='247'");
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
                    txtGrade.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Supplier Performance Remark", "PageLoad", ex.Message);
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
            if (txtGrade.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field 'Grade Name' is Required";
                //  ShowMessage("#Avisos", "The Field 'Category Name' is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (txtFrom.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field 'From' is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (txtTo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field 'To' is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (Convert.ToInt32(txtTo.Text) > 100)
            {
                lblmsg.Text = "Qty should be less than 100";
                PanelMsg.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtGrade.Focus();
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Performance Remark", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region txtTo_TextChanged
    protected void txtTo_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(txtTo.Text) > 100)
        {
            lblmsg.Text = "Qty should be less than 100";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            txtGrade.Focus();
        }
    }
    #endregion txtTo_TextChanged

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
            CommonClasses.SendError("Supplier Performance Remark", "btnCancel_Click", ex.Message.ToString());
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
            if (txtGrade.Text.Trim() == "")
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
            CommonClasses.SendError("Supplier Performance Remark", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("SUPPLIER_PERFORMANCE_REMARK", "MODIFY", "SPR_CODE", mlCode);
            }
            Response.Redirect("~/Masters/View/ViewSupplierPerfRemark.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Performance Remark", "btnCancel_Click", Ex.Message);
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
            dt = CommonClasses.Execute("SELECT * FROM SUPPLIER_PERFORMANCE_REMARK WHERE SPR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND SPR_CODE='" + mlCode.ToString() + "'");
            txtGrade.Text = dt.Rows[0]["SPR_NAME"].ToString();
            txtFrom.Text = dt.Rows[0]["SPR_FROM"].ToString();
            txtTo.Text = dt.Rows[0]["SPR_TO"].ToString();
            txtRemark.Text = dt.Rows[0]["SPR_REMARK"].ToString();
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SUPPLIER_PERFORMANCE_REMARK", "MODIFY", "SPR_CODE", mlCode);
            }
            if (str == "VIEW")
            {
                txtGrade.Enabled = false; txtFrom.Enabled = false; txtTo.Enabled = false;
                btnSubmit.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Performance Remark", "ViewRec", Ex.Message);
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
            string StrReplaceSctorName = txtGrade.Text;
            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");
            string StrRemark = txtRemark.Text;
            StrRemark = StrRemark.Replace("'", "''");
            #region Insert
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dtExist = CommonClasses.Execute("SELECT * FROM SUPPLIER_PERFORMANCE_REMARK WHERE SPR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND lower(SPR_NAME)=lower('" + StrReplaceSctorName.Trim().ToString() + "')");
                if (dtExist.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into SUPPLIER_PERFORMANCE_REMARK (SPR_CM_COMP_ID,SPR_NAME,SPR_FROM,SPR_TO,SPR_REMARK) values ('" + Convert.ToInt32(Session["CompanyId"]) + "','" + StrReplaceSctorName.Trim().ToString().ToUpper() + "','" + txtFrom.Text + "','" + txtTo.Text + "','" + StrRemark.ToString() + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SPR_CODE) from SUPPLIER_PERFORMANCE_REMARK");
                        CommonClasses.WriteLog("Supplier Performance Remark", "Save", "Supplier Performance Remark", StrReplaceSctorName, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/ViewSupplierPerfRemark.aspx", false);
                    }
                    else
                    {
                        lblmsg.Text = "Record Not Save";
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtGrade.Focus();
                    }
                }
                else
                {
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtGrade.Focus();
                }
            }
            #endregion Insert

            #region Modify
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtExist = CommonClasses.Execute("SELECT * FROM SUPPLIER_PERFORMANCE_REMARK WHERE SPR_CODE!='" + mlCode + "' AND SPR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0 AND lower(SPR_NAME)=lower('" + StrReplaceSctorName.Trim().ToString() + "')");
                if (dtExist.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("update SUPPLIER_PERFORMANCE_REMARK set SPR_NAME='" + StrReplaceSctorName.Trim().ToString().ToUpper() + "',SPR_FROM='" + txtFrom.Text + "',SPR_TO='" + txtTo.Text + "',SPR_REMARK='" + StrRemark.ToString() + "' where SPR_CODE='" + mlCode + "'"))
                    {
                        CommonClasses.RemoveModifyLock("SUPPLIER_PERFORMANCE_REMARK", "MODIFY", "SPR_CODE", mlCode);
                        CommonClasses.WriteLog("Supplier Performance Remark", "Update", "Supplier Performance Remark", StrReplaceSctorName, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/ViewSupplierPerfRemark.aspx", false);
                    }
                    else
                    {
                        lblmsg.Text = "Record Not Save";
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtGrade.Focus();
                    }
                }
                else
                {
                    lblmsg.Text = "Record Already Exists";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    txtGrade.Focus();
                }
            }
        }
            #endregion Modify

        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Performance Remark", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Supplier Performance Remark", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion
}
