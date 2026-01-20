using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class PPC_ADD_CPBOM_MASTER : System.Web.UI.Page
{
    # region Variables
    BOM_Master_BL BL_BOM_Master = null;
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

                        LoadVendor();
                        FinishItem();

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
                        CommonClasses.SendError("Vendor Schedule Transaction", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "Page_Load", ex.Message);
        }
    }
    #endregion


    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            LoadVendor();
            ddlPartNo.SelectedValue = BL_BOM_Master.CPBM_I_CODE.ToString();
            FinishItem();
            ddlfinishedNo.SelectedValue = BL_BOM_Master.CPBM_FINISH_I_CODE.ToString();

            if (str == "VIEW")
            {
                ddlPartNo.Enabled = false;
                ddlfinishedNo.Enabled = false;
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                ddlPartNo.Enabled = false;
                ddlfinishedNo.Enabled = false;
                btnSubmit.Visible = true;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Category Master", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_BOM_Master = new BOM_Master_BL(Convert.ToInt32(ViewState["mlCode"]));
            DataTable dt = new DataTable();
            BL_BOM_Master.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("CP_BOM_MASTER", "MODIFY", "CPBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlPartNo.SelectedIndex == -1 || ddlPartNo.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Finihsed(Sale) Code/Name", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlPartNo.Focus();
            return;
        }
        if (ddlfinishedNo.SelectedIndex == -1 || ddlfinishedNo.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Finished Code/Name", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlfinishedNo.Focus();
            return;
        }
        #endregion

        SaveRec();
    }
    #endregion


    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_BOM_Master.CPBM_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_BOM_Master.CPBM_I_CODE = Convert.ToInt32(ddlPartNo.SelectedValue);
            BL_BOM_Master.CPBM_FINISH_I_CODE = Convert.ToInt32(ddlfinishedNo.SelectedValue);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "Setvalues", ex.Message);
        }
        return res;
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
                BL_BOM_Master = new BOM_Master_BL();
                if (Setvalues())
                {
                    if (BL_BOM_Master.Save("INSERT"))
                    {
                        CommonClasses.WriteLog("CP BOM Master ", "Insert", "CP BOM Master ", BL_BOM_Master.PK_CODE.ToString(), Convert.ToInt32(BL_BOM_Master.PK_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/CPBOM_MASTER.aspx", false);
                    }
                    else
                    {
                        if (BL_BOM_Master.message != "")
                        {
                            ShowMessage("#Avisos", BL_BOM_Master.message.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            BL_BOM_Master.message = "";
                        }
                        ddlPartNo.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_BOM_Master = new BOM_Master_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (BL_BOM_Master.Save("MODIFY"))
                    {
                        CommonClasses.RemoveModifyLock("CP_BOM_MASTER", "MODIFY", "CPBM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("CP BOM Master ", "Update", "CP BOM Master ", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/CPBOM_MASTER.aspx", false);
                    }
                    else
                    {
                        if (BL_BOM_Master.message != "")
                        {
                            ShowMessage("#Avisos", BL_BOM_Master.message.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_BOM_Master.message = "";
                        }
                        ddlPartNo.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "SaveRec", ex.Message);
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
            CommonClasses.SendError("VENDOR_SCHEDULE", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Transaction", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("CP_BOM_MASTER", "MODIFY", "CPBM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/CPBOM_MASTER.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Transaction", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlPartNo.SelectedIndex == -1)
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
            CommonClasses.SendError("Vendor Schedule Transaction", "CheckValid", Ex.Message);
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

    #region ddlfinishedNo_SelectedIndexChanged
    protected void ddlfinishedNo_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    #endregion ddlfinishedNo_SelectedIndexChanged

    #region LoadVendor
    protected void LoadVendor()
    {
        DataTable dtProcess = new DataTable();
        //Here Part name binded which is in cust po(Sales order) form only
        dtProcess = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO+' - '+I_NAME as I_CODENO FROM ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOD_I_CODE=I_CODE AND CUSTPO_MASTER.CPOM_CODE=CPOD_CPOM_CODE AND ITEM_MASTER.ES_DELETE=0 AND CUSTPO_MASTER.ES_DELETE=0 AND ITEM_MASTER.I_ACTIVE_IND=1 and ITEM_MASTER.I_CM_COMP_ID=" + Session["CompanyId"] + " order by I_CODENO+' - '+I_NAME"); //and P_STM_CODE in(-2147483647,-2147483646) REMOVE 12072018
        ddlPartNo.DataSource = dtProcess;
        ddlPartNo.DataTextField = "I_CODENO";
        ddlPartNo.DataValueField = "I_CODE";
        ddlPartNo.DataBind();
        ddlPartNo.Items.Insert(0, new ListItem("Select Finihsed(Sale) Code/Name", "0"));
    }
    #endregion LoadVendor

    #region FinishItem
    protected void FinishItem()
    {
        DataTable dtFinishItem = new DataTable();
        //Here Item Category binded FINISH GOODS Only
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO+' - '+I_NAME AS I_CODENO FROM ITEM_MASTER WHERE ES_DELETE=0 AND I_ACTIVE_IND=1 AND I_CAT_CODE=-2147483648 AND ITEM_MASTER.I_CM_COMP_ID=" + Session["CompanyId"] + " ORDER BY I_CODENO+' - '+I_NAME");
        ddlfinishedNo.DataSource = dtFinishItem;
        ddlfinishedNo.DataTextField = "I_CODENO";
        ddlfinishedNo.DataValueField = "I_CODE";
        ddlfinishedNo.DataBind();
        ddlfinishedNo.Items.Insert(0, new ListItem("Select Finished Code/Name", "0"));
    }
    #endregion FinishItem

    #region ddlPartNo_SelectedIndexChanged
    protected void ddlPartNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FinishItem();
    }
    #endregion ddlPartNo_SelectedIndexChanged
}

