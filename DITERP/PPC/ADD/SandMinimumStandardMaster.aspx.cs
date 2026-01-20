using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_SandMinimumStandardMaster : System.Web.UI.Page
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
                        CommonClasses.SendError("SAND RAW STANDARD MASTER", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT SRSM_CODE,SRSM_AC4CH, SRSM_ADC12,SRSM_KS1275,SRSM_LM26,SRSM_LM28, SRSM_COMP_ID,SRSM_AC4B,SRSM_LM25,SRSM_SAND,SRSM_LM24,ES_DELETE,MODIFY from [SAND_RAW_STANDARD_MASTER] where SRSM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 and SRSM_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtAC4B.Text = dt.Rows[0]["SRSM_AC4B"].ToString();
                txtLM25.Text = dt.Rows[0]["SRSM_LM25"].ToString();
                txtSAND.Text = dt.Rows[0]["SRSM_SAND"].ToString();

                txtLM24.Text = dt.Rows[0]["SRSM_LM24"].ToString();
                txtAC4CH.Text = dt.Rows[0]["SRSM_AC4CH"].ToString();
                txtADC12.Text = dt.Rows[0]["SRSM_ADC12"].ToString();
                txtKS1275.Text = dt.Rows[0]["SRSM_KS1275"].ToString();
                txtLM26.Text = dt.Rows[0]["SRSM_LM26"].ToString();
                txtLM28.Text = dt.Rows[0]["SRSM_LM28"].ToString();

                if (str == "VIEW")
                {
                    txtAC4B.Enabled = false; txtLM25.Enabled = false; txtSAND.Enabled = false; btnSubmit.Visible = false;
                    txtLM24.Enabled = false; txtAC4CH.Enabled = false; txtADC12.Enabled = false;
                    txtKS1275.Enabled = false; txtLM26.Enabled = false; txtLM28.Enabled = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("SAND_RAW_STANDARD_MASTER", "MODIFY", "SRSM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (txtAC4B.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter AC4B", CommonClasses.MSG_Warning);
            txtAC4B.Focus();
            return;
        }
        if (txtLM25.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter LM25", CommonClasses.MSG_Warning);
            txtAC4B.Focus();
            return;
        }
        if (txtSAND.Text.Trim() == "")
        {
            ShowMessage("#Avisos", "Please Enter SAND", CommonClasses.MSG_Warning);
            txtAC4B.Focus();
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
                //dt = CommonClasses.Execute("Select M_CODE,M_NAME FROM MACHINE_MASTER WHERE lower(M_NAME)= lower('" + txtAC4B.Text.Trim().Replace("'", "\''") + "') AND M_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                dt = CommonClasses.Execute("Select * FROM [SAND_RAW_STANDARD_MASTER] WHERE lower(SRSM_AC4B)= lower('" + txtAC4B.Text.Trim().Replace("'", "\''") + "') AND  lower(SRSM_LM25)= lower('" + txtLM25.Text.Trim().Replace("'", "\''") + "' AND  lower(SRSM_SAND)= lower('" + txtSAND.Text.Trim().Replace("'", "\''") + "') AND ES_DELETE=0");
                if (dt.Rows.Count == 0)
                {
                    //if (CommonClasses.Execute1("INSERT INTO MACHINE_MASTER(M_COMP_ID,M_NAME,M_DESCR)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + txtAC4B.Text.Trim().Replace("'", "\''") + "','" + txtLM25.Text.Trim().Replace("'", "\''") + "')"))
                    if (CommonClasses.Execute1("INSERT INTO [SAND_RAW_STANDARD_MASTER] ([SRSM_COMP_ID], [SRSM_AC4B], [SRSM_LM25] ,[SRSM_SAND],SRSM_LM24,SRSM_AC4CH,SRSM_ADC12,SRSM_KS1275,SRSM_LM26,SRSM_LM28) VALUES ('" + Convert.ToInt32(Session["CompanyId"]) + "', '" + txtAC4B.Text.Trim().Replace("'", "\''") + "','" + txtLM25.Text.Trim().Replace("'", "\''") + "', '" + txtSAND.Text.Trim().Replace("'", "\''") + "', '" + txtLM24.Text.Trim().Replace("'", "\''") + "', '" + txtAC4CH.Text.Trim().Replace("'", "\''") + "', '" + txtADC12.Text.Trim().Replace("'", "\''") + "', '" + txtKS1275.Text.Trim().Replace("'", "\''") + "', '" + txtLM26.Text.Trim().Replace("'", "\''") + "', '" + txtLM28.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SRSM_CODE) from [SAND_RAW_STANDARD_MASTER]");
                        CommonClasses.WriteLog("SAND_RAW_STANDARD_MASTER", "Save", "SAND_RAW_STANDARD_MASTER", txtAC4B.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewSandMinimumStandardMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        txtAC4B.Focus();
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
                //dt = CommonClasses.Execute("SELECT * FROM MACHINE_MASTER WHERE ES_DELETE=0 AND M_CODE!= '" + mlCode + "' AND lower(M_NAME) = lower('" + txtAC4B.Text.Trim() + "')");
                dt = CommonClasses.Execute("Select * FROM [SAND_RAW_STANDARD_MASTER] WHERE lower(SRSM_AC4B)= lower('" + txtAC4B.Text.Trim().Replace("'", "\''") + "') AND  lower(SRSM_LM25)= lower('" + txtLM25.Text.Trim().Replace("'", "\''") + "' AND  lower(SRSM_SAND)= lower('" + txtSAND.Text.Trim().Replace("'", "\''") + "') AND ES_DELETE=0 AND SRSM_CODE!= '" + mlCode + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE [SAND_RAW_STANDARD_MASTER] SET SRSM_AC4B='" + txtAC4B.Text.Trim().Replace("'", "\''") + "',SRSM_LM25='" + txtLM25.Text.Trim().Replace("'", "\''") + "',SRSM_SAND='" + txtSAND.Text.Trim().Replace("'", "\''") + "' ,SRSM_LM24='" + txtLM24.Text.Trim().Replace("'", "\''") + "' ,SRSM_AC4CH='" + txtAC4CH.Text.Trim().Replace("'", "\''") + "' ,SRSM_ADC12='" + txtADC12.Text.Trim().Replace("'", "\''") + "',SRSM_KS1275='" + txtKS1275.Text.Trim().Replace("'", "\''") + "',SRSM_LM26='" + txtLM26.Text.Trim().Replace("'", "\''") + "',SRSM_LM28='" + txtLM28.Text.Trim().Replace("'", "\''") + "' WHERE SRSM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("[SAND_RAW_STANDARD_MASTER]", "MODIFY", "SRSM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("SAND_RAW_STANDARD_MASTER", "Update", "SAND_RAW_STANDARD_MASTER", txtAC4B.Text.Trim().Replace("'", "\''"), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewSandMinimumStandardMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtAC4B.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtAC4B.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "SaveRec", ex.Message);
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
            CommonClasses.SendError("SAND_RAW_STANDARD_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("SAND_RAW_STANDARD_MASTER", "MODIFY", "SRSM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewSandMinimumStandardMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtAC4B.Text.Trim() == "")
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
            CommonClasses.SendError("SAND RAW STANDARD MASTER", "CheckValid", Ex.Message);
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
