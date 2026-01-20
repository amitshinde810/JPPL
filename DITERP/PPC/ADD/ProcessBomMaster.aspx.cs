using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_ProcessBomMaster : System.Web.UI.Page
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
                        LoadItems(); // Method call
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
                        CommonClasses.SendError("Process BOM Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process BOM Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadItems();
            dt = CommonClasses.Execute("select PBM.PBM_CODE,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME,PBM_MELTING,PBM_CORE,PBM_CASTING,PBM_CUTTING,PBM_DECORING,PBM_HEAT_TREATMENT,PBM_FETTLING,PBM_SHOT_BLASTING,PBM_INSPECTION,PBM_MACHINING,PBM_IMP,PBM_ASSEMBLY,PBM_LEAKAGE_TESTING,PBM_WASHING,PBM_PACKING,isnull(PBM_FINAL_INSPECTION,0) as PBM_FINAL_INSPECTION from PROCESS_BOM_MASTER PBM INNER JOIN ITEM_MASTER I ON PBM.PBM_I_CODE=I.I_CODE where PBM.ES_DELETE=0 AND I.ES_DELETE=0 and PBM.PBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PBM_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                ddlPartName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                chkMelting.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_MELTING"].ToString());
                ChkCore.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_CORE"].ToString());
                chkCasting.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_CASTING"].ToString());
                chkCutting.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_CUTTING"].ToString());
                chkDecoring.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_DECORING"].ToString());
                chkHeatTreatment.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_HEAT_TREATMENT"].ToString());
                chkFettling.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_FETTLING"].ToString());
                chkShotBlasting.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_SHOT_BLASTING"].ToString());
                chkInspection.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_INSPECTION"].ToString());
                chkMachining.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_MACHINING"].ToString());
                chkImp.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_IMP"].ToString());
                chkAssembly.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_ASSEMBLY"].ToString());
                chkLeakageTesting.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_LEAKAGE_TESTING"].ToString());
                chkWashing.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_WASHING"].ToString());
                chkPacking.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_PACKING"].ToString());
                chkFinalInspection.Checked = Convert.ToBoolean(dt.Rows[0]["PBM_FINAL_INSPECTION"].ToString());
                if (str == "VIEW")
                {


                    ddlPartName.Enabled = false; btnSubmit.Visible = false; chkMelting.Enabled = false; ChkCore.Enabled = false;
                    chkCasting.Enabled = false; chkCutting.Enabled = false; chkDecoring.Enabled = false; chkHeatTreatment.Enabled = false;
                    chkFettling.Enabled = false; chkShotBlasting.Enabled = false; chkInspection.Enabled = false; chkMachining.Enabled = false;
                    chkImp.Enabled = false; chkAssembly.Enabled = false; chkLeakageTesting.Enabled = false; chkWashing.Enabled = false;
                    chkPacking.Enabled = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PROCESS_BOM_MASTER", "MODIFY", "PBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process BOM Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlPartName.SelectedIndex == -1 || ddlPartName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Part Name", CommonClasses.MSG_Warning);
            ddlPartName.Focus();
            return;
        }
        if (chkMelting.Checked == false && ChkCore.Checked == false && chkCasting.Checked == false && chkCutting.Checked == false && chkDecoring.Checked == false && chkHeatTreatment.Checked == false && chkFettling.Checked == false && chkShotBlasting.Checked == false && chkInspection.Checked == false && chkMachining.Checked == false && chkImp.Checked == false && chkAssembly.Checked == false && chkLeakageTesting.Checked == false && chkWashing.Checked == false && chkPacking.Checked == false)
        {
            ShowMessage("#Avisos", "Please Select Reason", CommonClasses.MSG_Warning);
            ddlPartName.Focus();
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
                dt = CommonClasses.Execute("Select PBM_CODE FROM PROCESS_BOM_MASTER WHERE PBM_I_CODE= '" + ddlPartName.SelectedValue + "' AND PBM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO PROCESS_BOM_MASTER(PBM_COMP_ID,PBM_I_CODE,PBM_MELTING,PBM_CORE,PBM_CASTING,PBM_CUTTING,PBM_DECORING,PBM_HEAT_TREATMENT,PBM_FETTLING,PBM_SHOT_BLASTING,PBM_INSPECTION,PBM_MACHINING,PBM_IMP,PBM_ASSEMBLY,PBM_LEAKAGE_TESTING,PBM_WASHING,PBM_PACKING,PBM_FINAL_INSPECTION)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlPartName.SelectedValue + "','" + chkMelting.Checked + "','" + ChkCore.Checked + "','" + chkCasting.Checked + "','" + chkCutting.Checked + "','" + chkDecoring.Checked + "','" + chkHeatTreatment.Checked + "','" + chkFettling.Checked + "','" + chkShotBlasting.Checked + "','" + chkInspection.Checked + "','" + chkMachining.Checked + "','" + chkImp.Checked + "','" + chkAssembly.Checked + "','" + chkLeakageTesting.Checked + "','" + chkWashing.Checked + "','" + chkPacking.Checked + "','" + chkFinalInspection.Checked + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PBM_CODE) from PROCESS_BOM_MASTER");
                        CommonClasses.WriteLog("Process BOM Master", "Save", "Process BOM Master", ddlPartName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProcessBomMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlPartName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Part Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PROCESS_BOM_MASTER WHERE ES_DELETE=0 AND PBM_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND PBM_I_CODE= '" + ddlPartName.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PROCESS_BOM_MASTER SET PBM_I_CODE='" + ddlPartName.SelectedValue + "',PBM_MELTING='" + chkMelting.Checked + "',PBM_CORE='" + ChkCore.Checked + "',PBM_CASTING='" + chkCasting.Checked + "',PBM_CUTTING='" + chkCutting.Checked + "',PBM_DECORING='" + chkDecoring.Checked + "',PBM_HEAT_TREATMENT='" + chkHeatTreatment.Checked + "',PBM_FETTLING='" + chkFettling.Checked + "',PBM_SHOT_BLASTING='" + chkShotBlasting.Checked + "',PBM_INSPECTION='" + chkInspection.Checked + "',PBM_MACHINING='" + chkMachining.Checked + "',PBM_IMP='" + chkImp.Checked + "',PBM_ASSEMBLY='" + chkAssembly.Checked + "',PBM_LEAKAGE_TESTING='" + chkLeakageTesting.Checked + "',PBM_WASHING='" + chkWashing.Checked + "',PBM_PACKING ='" + chkPacking.Checked + "',PBM_FINAL_INSPECTION='" + chkFinalInspection.Checked + "' WHERE PBM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PROCESS_BOM_MASTER", "MODIFY", "PBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Process BOM Master", "Update", "Process BOM Master", ddlPartName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProcessBomMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlPartName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Part Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlPartName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process BOM Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("PROCESS_BOM_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Process BOM Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("PROCESS_BOM_MASTER", "MODIFY", "PBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewProcessBomMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process BOM Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlPartName.SelectedIndex == -1)
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
            CommonClasses.SendError("Process BOM Master", "CheckValid", Ex.Message);
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

    #region ddlPartName_SelectedIndexChanged
    protected void ddlPartName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlPartName_SelectedIndexChanged

    #region LoadItems
    protected void LoadItems()
    {
        DataTable dtFinishItem = new DataTable();
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        ddlPartName.DataSource = dtFinishItem;
        ddlPartName.DataTextField = "ICODE_INAME";
        ddlPartName.DataValueField = "I_CODE";
        ddlPartName.DataBind();
        ddlPartName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItems

}
