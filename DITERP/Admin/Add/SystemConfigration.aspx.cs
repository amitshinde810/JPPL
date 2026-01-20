using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Add_SystemConfigration : System.Web.UI.Page
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
                        LoadTallyAcc();
                        ViewRec("MOD");
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("System Configration", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("System Configration", "Page_Load", ex.Message);
        }
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
            CommonClasses.SendError("Syetem Configration", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
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
        catch (Exception ex)
        {
            CommonClasses.SendError("System Configration", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("SYSTEM_CONFIG_SETTING", "MODIFY", "SCS_CODE", mlCode);
            }
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("System Configration", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlTallyDisc.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyPackAmt.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyOther.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyFreight.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyFreight.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyInsurance.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyTrans.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyOctri.SelectedIndex == 0)
            {
                flag = false;
            }
            if (ddlTallyTcs.SelectedIndex == 0)
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
            CommonClasses.SendError("System Configration", "CheckValid", Ex.Message);
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

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM SYSTEM_CONFIG_SETTING WHERE SCS_COMP_ID= " + (string)Session["CompanyId"] + "");

            if (dt.Rows.Count > 0)
            {
                ddlTallyDisc.SelectedValue = dt.Rows[0]["SCS_DISC_TALLY_CODE"].ToString();
                ddlTallyPackAmt.SelectedValue = dt.Rows[0]["SCS_PACKAMT_TALLY_CODE"].ToString();
                ddlTallyOther.SelectedValue = dt.Rows[0]["SCS_OTHER_TALLY_CODE"].ToString();
                ddlTallyFreight.SelectedValue = dt.Rows[0]["SCS_FREIGHT_TALLY_CODE"].ToString();
                ddlTallyInsurance.SelectedValue = dt.Rows[0]["SCS_INSUR_TALLY_CODE"].ToString();
                ddlTallyTrans.SelectedValue = dt.Rows[0]["SCS_TRANBS_TAYYLY_CODE"].ToString();
                ddlTallyOctri.SelectedValue = dt.Rows[0]["SCS_OCTRI_TALLY_CODE"].ToString();
                ddlTallyTcs.SelectedValue = dt.Rows[0]["SCS_TCS_TALLY_CODE"].ToString();
                ddlTallyAdvDuty.SelectedValue = dt.Rows[0]["SCS_ADV_TALLY_CODE"].ToString();

                if (str == "VIEW")
                {
                    btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SYSTEM_CONFIG_SETTING", "MODIFY", "SCS_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sale Order Type Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    }
    #endregion

    #region LoadTallyAcc
    private void LoadTallyAcc()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select TALLY_CODE,TALLY_NAME from TALLY_MASTER where ES_DELETE=0 and TALLY_COMP_ID=" + Session["CompanyId"] + "");

                ddlTallyDisc.DataSource = dt;
                ddlTallyDisc.DataTextField = "TALLY_NAME";
                ddlTallyDisc.DataValueField = "TALLY_CODE";
                ddlTallyDisc.DataBind();
                ddlTallyDisc.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyPackAmt.DataSource = dt;
                ddlTallyPackAmt.DataTextField = "TALLY_NAME";
                ddlTallyPackAmt.DataValueField = "TALLY_CODE";
                ddlTallyPackAmt.DataBind();
                ddlTallyPackAmt.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyOther.DataSource = dt;
                ddlTallyOther.DataTextField = "TALLY_NAME";
                ddlTallyOther.DataValueField = "TALLY_CODE";
                ddlTallyOther.DataBind();
                ddlTallyOther.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyFreight.DataSource = dt;
                ddlTallyFreight.DataTextField = "TALLY_NAME";
                ddlTallyFreight.DataValueField = "TALLY_CODE";
                ddlTallyFreight.DataBind();
                ddlTallyFreight.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyInsurance.DataSource = dt;
                ddlTallyInsurance.DataTextField = "TALLY_NAME";
                ddlTallyInsurance.DataValueField = "TALLY_CODE";
                ddlTallyInsurance.DataBind();
                ddlTallyInsurance.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyTrans.DataSource = dt;
                ddlTallyTrans.DataTextField = "TALLY_NAME";
                ddlTallyTrans.DataValueField = "TALLY_CODE";
                ddlTallyTrans.DataBind();
                ddlTallyTrans.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyOctri.DataSource = dt;
                ddlTallyOctri.DataTextField = "TALLY_NAME";
                ddlTallyOctri.DataValueField = "TALLY_CODE";
                ddlTallyOctri.DataBind();
                ddlTallyOctri.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyTcs.DataSource = dt;
                ddlTallyTcs.DataTextField = "TALLY_NAME";
                ddlTallyTcs.DataValueField = "TALLY_CODE";
                ddlTallyTcs.DataBind();
                ddlTallyTcs.Items.Insert(0, new ListItem("Select Tally Name", "0"));

                ddlTallyAdvDuty.DataSource = dt;
                ddlTallyAdvDuty.DataTextField = "TALLY_NAME";
                ddlTallyAdvDuty.DataValueField = "TALLY_CODE";
                ddlTallyAdvDuty.DataBind();
                ddlTallyAdvDuty.Items.Insert(0, new ListItem("Select Tally Name", "0"));
            }
            catch (Exception ex)
            {
                CommonClasses.SendError("System Configration", "LoadTallyAcc", ex.Message);
            }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("System Configration", "LoadTallyAcc", ex.Message);
        }
    }
    #endregion Load Country

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            DataTable dt = new DataTable();
            if (dt.Rows.Count == 0)
            {
                if (CommonClasses.Execute1("UPDATE SYSTEM_CONFIG_SETTING  SET SCS_COMP_ID=" + Session["CompanyId"] + ",SCS_DISC_TALLY_CODE=" + ddlTallyDisc.SelectedValue + ",SCS_PACKAMT_TALLY_CODE=" + ddlTallyPackAmt.SelectedValue + ",SCS_OTHER_TALLY_CODE=" + ddlTallyOther.SelectedValue + ",SCS_FREIGHT_TALLY_CODE=" + ddlTallyFreight.SelectedValue + ",SCS_INSUR_TALLY_CODE=" + ddlTallyInsurance.SelectedValue + ",SCS_TRANBS_TAYYLY_CODE=" + ddlTallyTrans.SelectedValue + ",SCS_OCTRI_TALLY_CODE=" + ddlTallyOctri.SelectedValue + ",SCS_TCS_TALLY_CODE=" + ddlTallyTcs.SelectedValue + ",SCS_ADV_TALLY_CODE=" + ddlTallyAdvDuty.SelectedValue + ""))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(SCS_CODE) from SYSTEM_CONFIG_SETTING");
                    CommonClasses.WriteLog("System Configration", "Save", "System Configration", Convert.ToString(Code), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Admin/Default.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                }
            }
            else
            {
                ShowMessage("#Avisos", "Short Name Already Exists", CommonClasses.MSG_Warning);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("System Configration", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion
}
