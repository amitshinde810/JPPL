using System;
using System.Data;
using System.Web.UI;

public partial class Admin_Add_RejectionMaster : System.Web.UI.Page
{
    #region General Declaration

    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='5'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                ViewState["mlCode"] = "";
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
                    txtMeltingLoss.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Rejection Master", "PageLoad", ex.Message);
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
            if (txtMeltingLoss.Text == "")
            {
                ShowMessage("#Avisos", "The Field 'Country Name' is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Master", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtMeltingLoss.Text.Trim() == "")
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
            CommonClasses.SendError("Currency Master", "CheckValid", Ex.Message);
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
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("REJECTION_MASTER", "MODIFY", "RM_CODE", mlCode);
            }
            Response.Redirect("~/Admin/View/ViewRejectionMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Methods
    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * from [REJECTION_MASTER] where RM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and RM_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtMeltingLoss.Text = dt.Rows[0]["RM_MELTING_LOSS"].ToString();
                txtCastingRejection.Text = dt.Rows[0]["RM_CASTING_REJECTION"].ToString();
                txtMachiningRejection.Text = dt.Rows[0]["RM_MACHINING_REJECTION"].ToString();
                txtCoreBreakage.Text = dt.Rows[0]["RM_CORE_BREAKAGE"].ToString();
                txtSandWastage.Text = dt.Rows[0]["RM_SAND_WASTAGE"].ToString();
                if (str == "VIEW")
                {
                    txtMeltingLoss.Enabled = false; txtCastingRejection.Enabled = false; txtMachiningRejection.Enabled = false; txtCoreBreakage.Enabled = false; txtSandWastage.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("REJECTION_MASTER", "MODIFY", "RM_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceSctorName = txtMeltingLoss.Text;
            StrReplaceSctorName = StrReplaceSctorName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select * FROM REJECTION_MASTER WHERE RM_MELTING_LOSS='" + txtMeltingLoss.Text.Trim().Replace("'", "\''") + "') AND  RM_CASTING_REJECTION='" + txtCastingRejection.Text.Trim().Replace("'", "\''") + "' AND  RM_MACHINING_REJECTION='" + txtMachiningRejection.Text.Trim().Replace("'", "\''") + "') AND  RM_CORE_BREAKAGE='" + txtCoreBreakage.Text.Trim().Replace("'", "\''") + "') AND  RM_SAND_WASTAGE='" + txtSandWastage.Text.Trim().Replace("'", "\''") + "') AND ES_DELETE=0");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO REJECTION_MASTER (RM_COMP_ID, RM_MELTING_LOSS, RM_CASTING_REJECTION, RM_MACHINING_REJECTION, RM_CORE_BREAKAGE, RM_SAND_WASTAGE,RM_DATE) VALUES ('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtMeltingLoss.Text + "','" + txtCastingRejection.Text + "','" + txtMachiningRejection.Text + "','" + txtCoreBreakage.Text + "','" + txtSandWastage.Text + "','" + System.DateTime.Now.ToString("dd/MMM/yyyy") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(RM_CODE) from [REJECTION_MASTER]");
                        CommonClasses.WriteLog("REJECTION_MASTER", "Save", "REJECTION_MASTER", txtMeltingLoss.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/View/ViewRejectionMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtMeltingLoss.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                //dt = CommonClasses.Execute("Select * FROM [SAND_RAW_STANDARD_MASTER] WHERE lower(SRSM_AC4B)= lower('" + txtAC4B.Text.Trim().Replace("'", "\''") + "') AND  lower(SRSM_LM25)= lower('" + txtLM25.Text.Trim().Replace("'", "\''") + "' AND  lower(SRSM_SAND)= lower('" + txtSAND.Text.Trim().Replace("'", "\''") + "') AND ES_DELETE=0 AND SRSM_CODE!= '" + mlCode + "'");
                dt = CommonClasses.Execute("SELECT * FROM REJECTION_MASTER WHERE RM_MELTING_LOSS='" + txtMeltingLoss.Text.Trim().Replace("'", "\''") + "') AND  RM_CASTING_REJECTION='" + txtCastingRejection.Text.Trim().Replace("'", "\''") + "' AND  RM_MACHINING_REJECTION='" + txtMachiningRejection.Text.Trim().Replace("'", "\''") + "') AND  RM_CORE_BREAKAGE='" + txtCoreBreakage.Text.Trim().Replace("'", "\''") + "') AND  RM_SAND_WASTAGE='" + txtSandWastage.Text.Trim().Replace("'", "\''") + "') AND ES_DELETE=0 AND RM_CODE!= '" + mlCode + "'");

                if (dt.Rows.Count == 0)
                {
                    //if (CommonClasses.Execute1("UPDATE [SAND_RAW_STANDARD_MASTER] SET SRSM_AC4B='" + txtAC4B.Text.Trim().Replace("'", "\''") + "',SRSM_LM25='" + txtLM25.Text.Trim().Replace("'", "\''") + "',SRSM_SAND='" + txtSAND.Text.Trim().Replace("'", "\''") + "' WHERE SRSM_CODE='" + ViewState["mlCode"] + "'"))
                    if (CommonClasses.Execute1("UPDATE [REJECTION_MASTER] SET [RM_MELTING_LOSS] = '" + txtMeltingLoss.Text.Trim() + "',[RM_CASTING_REJECTION] = " + txtCastingRejection.Text.Trim() + " ,[RM_MACHINING_REJECTION] = " + txtMachiningRejection.Text.Trim() + " ,[RM_CORE_BREAKAGE] = " + txtCoreBreakage.Text.Trim() + " ,[RM_SAND_WASTAGE] = " + txtSandWastage.Text.Trim() + " WHERE RM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("[REJECTION_MASTER]", "MODIFY", "RM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("REJECTION_MASTER", "Update", "REJECTION_MASTER", txtMeltingLoss.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Admin/VIEW/ViewRejectionMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtMeltingLoss.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtMeltingLoss.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Rejection Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
    #endregion
}
