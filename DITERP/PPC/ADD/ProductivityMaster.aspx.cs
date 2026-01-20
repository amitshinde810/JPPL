using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_ProductivityMaster : System.Web.UI.Page
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
                        LoadGroup(); //Method call
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
                        CommonClasses.SendError("Productivity Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Productivity Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadGroup();
            dt = CommonClasses.Execute("SELECT DISTINCT G.GP_NAME, I_CODENO +' - '+ I_NAME AS PROD_NAME ,I.I_CODE,G.GP_CODE,ISNULL(PROM_I_CODE,0) AS PROM_I_CODE,ISNULL(PROM_GP_CODE,0) AS PROM_GP_CODE,ISNULL(PROM_CORE,0) AS PROM_CORE,ISNULL(PROM_CASTING,0) AS PROM_CASTING,ISNULL(PROM_VMC,0) AS PROM_VMC,ISNULL(PROM_1STSETUP,0) AS PROM_1STSETUP,ISNULL(PROM_2NDSETUP,0) AS PROM_2NDSETUP,ISNULL(PROM_4THAXIS,0) AS PROM_4THAXIS,ISNULL(PROM_5THAXIS,0) AS PROM_5THAXIS,ISNULL(PROM_HMC,0) AS PROM_HMC,ISNULL(PROM_IMP,0) AS PROM_IMP,ISNULL(PROM_LEAKAGE_TESTING,0) AS PROM_LEAKAGE_TESTING,ISNULL(PROM_WASHING,0) AS PROM_WASHING FROM PRODUCTIVITY_MASTER S INNER JOIN ITEM_MASTER I on S.PROM_I_CODE=I.I_CODE INNER JOIN GROUP_MASTER G ON S.PROM_GP_CODE=G.GP_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.PROM_CODE= '" + ViewState["mlCode"] + "'AND S.PROM_COMP_ID='" + (string)Session["CompanyId"] + "'");
            if (dt.Rows.Count > 0)
            {
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                ddlGroup.SelectedValue = dt.Rows[0]["GP_CODE"].ToString();
                ddlGroup_SelectedIndexChanged(null, null);
                txtCore.Text = dt.Rows[0]["PROM_CORE"].ToString();
                txtCasting.Text = dt.Rows[0]["PROM_CASTING"].ToString();
                txt1stSetup.Text = dt.Rows[0]["PROM_1STSETUP"].ToString();
                txt2ndSetup.Text = dt.Rows[0]["PROM_2NDSETUP"].ToString();

                txt4thAxis.Text = dt.Rows[0]["PROM_4THAXIS"].ToString();
                txt5thAxis.Text = dt.Rows[0]["PROM_5THAXIS"].ToString();
                txtHMC.Text = dt.Rows[0]["PROM_HMC"].ToString();
                txtIMP.Text = dt.Rows[0]["PROM_IMP"].ToString();
                txtLeakageTesting.Text = dt.Rows[0]["PROM_LEAKAGE_TESTING"].ToString();
                txtWashing.Text = dt.Rows[0]["PROM_WASHING"].ToString();
                txtVMC.Text = dt.Rows[0]["PROM_VMC"].ToString();
                if (str == "VIEW")
                {
                    ddlItemName.Enabled = false; ddlGroup.Enabled = false; txtCore.Enabled = false; txtCasting.Enabled = false;
                    txt1stSetup.Enabled = false; txt2ndSetup.Enabled = false; txtVMC.Enabled = false;
                    txt4thAxis.Enabled = false; txt5thAxis.Enabled = false; txtHMC.Enabled = false; txtIMP.Enabled = false;
                    txtLeakageTesting.Enabled = false; txtWashing.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PRODUCTIVITY_MASTER", "MODIFY", "PROM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Productivity Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlGroup.SelectedIndex == -1)
        {
            ShowMessage("#Avisos", "Please Enter Group Name", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
        if (ddlItemName.SelectedIndex == -1)
        {
            ShowMessage("#Avisos", "Please Select Part Name", CommonClasses.MSG_Warning);
            ddlGroup.Focus();
            return;
        }
        if (txtCasting.Text.Trim() == "0" && txtCore.Text.Trim() == "0" && txtCasting.Text.Trim() == "0" && txt1stSetup.Text.Trim() == "0" && txt2ndSetup.Text.Trim() == "0" && txt4thAxis.Text.Trim() == "0" && txt5thAxis.Text.Trim() == "0" && txtHMC.Text.Trim() == "0" && txtIMP.Text.Trim() == "0" && txtLeakageTesting.Text.Trim() == "0" && txtWashing.Text.Trim() == "0")
        {
            ShowMessage("#Avisos", "Please Insert Quantity", CommonClasses.MSG_Warning);
            txtCore.Focus();
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
                dt = CommonClasses.Execute("Select PROM_CODE FROM PRODUCTIVITY_MASTER WHERE PROM_I_CODE= '" + ddlItemName.SelectedValue + "' and PROM_GP_CODE= '" + ddlGroup.SelectedValue + "' AND ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into PRODUCTIVITY_MASTER(PROM_COMP_ID,PROM_I_CODE,PROM_GP_CODE,PROM_CORE,PROM_CASTING,PROM_1STSETUP,PROM_2NDSETUP,PROM_4THAXIS,PROM_5THAXIS,PROM_HMC,PROM_IMP,PROM_LEAKAGE_TESTING,PROM_WASHING,PROM_VMC) VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlItemName.SelectedValue + "','" + ddlGroup.SelectedValue + "','" + txtCore.Text + "','" + txtCasting.Text + "','" + txt1stSetup.Text + "','" + txt2ndSetup.Text + "','" + txt4thAxis.Text + "','" + txt5thAxis.Text + "','" + txtHMC.Text + "','" + txtIMP.Text + "','" + txtLeakageTesting.Text + "','" + txtWashing.Text + "','" + txtVMC.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PROM_CODE) from PRODUCTIVITY_MASTER");
                        CommonClasses.WriteLog("Productivity Master", "Save", "Productivity Master", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProductivityMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlItemName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PRODUCTIVITY_MASTER WHERE ES_DELETE=0 AND PROM_CODE!= '" + ViewState["mlCode"] + "' and PROM_I_CODE='" + ddlItemName.SelectedValue + "' and PROM_GP_CODE='" + ddlGroup.SelectedValue + "'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PRODUCTIVITY_MASTER SET PROM_I_CODE='" + ddlItemName.SelectedValue + "',PROM_GP_CODE='" + ddlGroup.SelectedValue + "',PROM_CORE='" + txtCore.Text + "',PROM_CASTING='" + txtCasting.Text + "',PROM_1STSETUP='" + txt1stSetup.Text + "',PROM_2NDSETUP='" + txt2ndSetup.Text + "',PROM_4THAXIS='" + txt4thAxis.Text + "',PROM_5THAXIS='" + txt5thAxis.Text + "',PROM_HMC='" + txtHMC.Text + "',PROM_IMP='" + txtIMP.Text + "',PROM_LEAKAGE_TESTING='" + txtLeakageTesting.Text + "',PROM_WASHING='" + txtWashing.Text + "',PROM_VMC='" + txtVMC.Text + "' WHERE PROM_CODE ='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PRODUCTIVITY_MASTER", "MODIFY", "PROM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Productivity Master", "Update", "Productivity Master", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProductivityMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlItemName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Productivity Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("PRODUCTIVITY_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Productivity Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("PRODUCTIVITY_MASTER", "MODIFY", "PROM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewProductivityMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Productivity Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlItemName.SelectedIndex == -1)
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
            CommonClasses.SendError("Productivity Master", "CheckValid", Ex.Message);
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

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Clear_Controls
        txtCore.Text = "0"; txtCasting.Text = "0"; txt1stSetup.Text = "0"; txt2ndSetup.Text = "0"; txt4thAxis.Text = "0";
        txt5thAxis.Text = "0"; txtHMC.Text = "0"; txtIMP.Text = "0"; txtLeakageTesting.Text = "0"; txtWashing.Text = "0";
        txtVMC.Text = "0";
        #endregion Clear_Controls

        /*Only for Casting  (Standard Prod)* 2*/
        DataTable dtStdProd = CommonClasses.Execute("SELECT (ISNULL(I_STANDARD_PRODUCTION,0)*2) AS I_STANDARD_PRODUCTION  from ITEM_MASTER where ES_DELETE=0 AND I_CM_COMP_ID='" + Session["CompanyId"].ToString() + "' AND I_CODE='" + ddlItemName.SelectedValue + "'");
        if (dtStdProd.Rows.Count == 0)
        { }
        else
        {
            txtCasting.Text = dtStdProd.Rows[0]["I_STANDARD_PRODUCTION"].ToString();
        }
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlGroup_SelectedIndexChanged
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtLoadItem = new DataTable();
        dtLoadItem = CommonClasses.Execute("select DISTINCT I.I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from GROUP_MASTER G inner join PRODUCT_MASTER P on G.GP_CODE=P.PROD_GP_CODE INNER JOIN ITEM_MASTER I ON P.PROD_I_CODE=I.I_CODE WHERE GP_CODE='" + ddlGroup.SelectedValue + "' ORDER BY I_CODENO +' - '+ I_NAME");
        if (dtLoadItem.Rows.Count > 0)
        {
            ddlItemName.DataSource = dtLoadItem;
            ddlItemName.DataTextField = "ICODE_INAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        else
        {
            ddlItemName.Items.Clear(); // Clear the Items form Dropdownlist
        }
    }
    #endregion ddlGroup_SelectedIndexChanged

    #region LoadItem
    protected void LoadItem(string Group_Code)
    {
        DataTable dtFinishItem = new DataTable();
        dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
        ddlItemName.DataSource = dtFinishItem;
        ddlItemName.DataTextField = "ICODE_INAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }
    #endregion LoadItem

    #region LoadGroup
    protected void LoadGroup()
    {
        DataTable dtGroup = new DataTable();
        dtGroup = CommonClasses.Execute("select distinct GP_NAME,GP_CODE from GROUP_MASTER where ES_DELETE=0 and GP_COMP_ID='" + Session["CompanyId"] + "' ORDER BY GP_NAME");
        ddlGroup.DataSource = dtGroup;
        ddlGroup.DataTextField = "GP_NAME";
        ddlGroup.DataValueField = "GP_CODE";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, new ListItem("Select Group Name", "0"));
    }
    #endregion LoadGroup

}
