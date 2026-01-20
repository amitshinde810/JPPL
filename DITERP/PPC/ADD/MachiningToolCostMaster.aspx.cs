using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_MachiningToolCostMaster : System.Web.UI.Page
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
                        LoadItems();  // Method call
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
                        CommonClasses.SendError("Machining Tool Cost Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machining Tool Cost Master", "Page_Load", ex.Message);
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
            dt = CommonClasses.Execute("SELECT DISTINCT MTM_CODE,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME,I_NAME ,isnull(MTM_TOOL_COST_COMP,0) as MTM_TOOL_COST_COMP FROM MACHINE_TOOL_MASTER P INNER JOIN ITEM_MASTER I ON P.MTM_I_CODE=I.I_CODE where P.ES_DELETE=0 and I.ES_DELETE=0 and MTM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and MTM_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtToolCost.Text = dt.Rows[0]["MTM_TOOL_COST_COMP"].ToString();
                ddlPartName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                if (str == "VIEW")
                {
                    txtToolCost.Enabled = false; ddlPartName.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("MACHINE_TOOL_MASTER", "MODIFY", "MTM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machining Tool Cost Master", "ViewRec", ex.Message);
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
        if (txtToolCost.Text == "" || Convert.ToInt32(txtToolCost.Text.Trim()) == 0)
        {
            ShowMessage("#Avisos", "Please Insert Number Of Machines", CommonClasses.MSG_Warning);
            txtToolCost.Focus();
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
                dt = CommonClasses.Execute("Select MTM_CODE FROM MACHINE_TOOL_MASTER WHERE MTM_I_CODE= '" + ddlPartName.SelectedValue + "' AND MTM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO MACHINE_TOOL_MASTER(MTM_COMP_ID,MTM_I_CODE,MTM_TOOL_COST_COMP)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlPartName.SelectedValue + "','" + txtToolCost.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(MTM_CODE) from MACHINE_TOOL_MASTER");
                        CommonClasses.WriteLog("Machining Tool Cost Master", "Save", "Machining Tool Cost Master", ddlPartName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewMachiningToolCostMaster.aspx", false);
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
                dt = CommonClasses.Execute("SELECT * FROM MACHINE_TOOL_MASTER WHERE ES_DELETE=0 AND MTM_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND MTM_I_CODE= '" + ddlPartName.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE MACHINE_TOOL_MASTER SET MTM_I_CODE='" + ddlPartName.SelectedValue + "',MTM_TOOL_COST_COMP='" + txtToolCost.Text.Trim().Replace("'", "\''") + "' WHERE MTM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("MACHINE_TOOL_MASTER", "MODIFY", "MTM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Machining Tool Cost Master", "Update", "Machining Tool Cost Master", ddlPartName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewMachiningToolCostMaster.aspx", false);
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
            CommonClasses.SendError("Machining Tool Cost Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("MACHINE_TOOL_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Machining Tool Cost Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("MACHINE_TOOL_MASTER", "MODIFY", "MTM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewMachiningToolCostMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Machining Tool Cost Master", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Machining Tool Cost Master", "CheckValid", Ex.Message);
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

    #region ddlPallet_SelectedIndexChanged
    protected void ddlPallet_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlPallet_SelectedIndexChanged

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
