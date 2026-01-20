using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_StandardInventoryMaster : System.Web.UI.Page
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
                        LoadGroup(); //Method call

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
                        CommonClasses.SendError("Standard Inventory Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Standard Inventory Master", "Page_Load", ex.Message);
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
            
            dt = CommonClasses.Execute("SELECT DISTINCT G.GP_NAME, I_CODENO +' - '+ I_NAME AS PROD_NAME ,I.I_CODE,G.GP_CODE,ISNULL(SIM_FOUNDRY,0) AS SIM_FOUNDRY,ISNULL(SIM_RFM_STORE,0) AS SIM_RFM_STORE,ISNULL(SIM_VENDOR,0) AS SIM_VENDOR,ISNULL(SIM_MAIN_STORE,0) AS SIM_MAIN_STORE,ISNULL(SIM_MACHINE_SHOP,0) AS SIM_MACHINE_SHOP,ISNULL(SIM_RFI_STORE,0) AS SIM_RFI_STORE,ISNULL(SIM_FINISH_GOODS,0) AS SIM_FINISH_GOODS ,ISNULL(SIM_CUST_STOCK,0) AS SIM_CUST_STOCK FROM STANDARD_INVENTARY_MASTER S INNER JOIN ITEM_MASTER I on S.SIM_I_CODE=I.I_CODE INNER JOIN GROUP_MASTER G ON S.SIM_GP_CODE=G.GP_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_CODE= '" + ViewState["mlCode"] + "'AND S.SIM_COMP_ID='" + (string)Session["CompanyId"] + "'");
            if (dt.Rows.Count > 0)
            {
                ddlGroup.SelectedValue = dt.Rows[0]["GP_CODE"].ToString();
                LoadItem();
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                txtFoundry.Text = dt.Rows[0]["SIM_FOUNDRY"].ToString();
                txtRfmStore.Text = dt.Rows[0]["SIM_RFM_STORE"].ToString();
                txtVendor.Text = dt.Rows[0]["SIM_VENDOR"].ToString();
                txtMainStore.Text = dt.Rows[0]["SIM_MAIN_STORE"].ToString();
                txtMachineShop.Text = dt.Rows[0]["SIM_MACHINE_SHOP"].ToString();
                txtRFIStore.Text = dt.Rows[0]["SIM_RFI_STORE"].ToString();
                txtFinishGoods.Text = dt.Rows[0]["SIM_FINISH_GOODS"].ToString();
                txtCustomer_Std_Stock.Text = dt.Rows[0]["SIM_CUST_STOCK"].ToString();
                if (str == "VIEW")
                {
                    ddlItemName.Enabled = false; ddlGroup.Enabled = false; txtFoundry.Enabled = false; txtRfmStore.Enabled = false; txtVendor.Enabled = false;
                    txtMainStore.Enabled = false; txtMachineShop.Enabled = false; txtRFIStore.Enabled = false; txtFinishGoods.Enabled = false;
                    btnSubmit.Visible = false; txtCustomer_Std_Stock.Enabled = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("STANDARD_INVENTARY_MASTER", "MODIFY", "SIM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Standard Inventory Master", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        #region Validation
        if (ddlItemName.SelectedIndex == -1 || ddlItemName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Enter Part Name", CommonClasses.MSG_Warning);
            ddlItemName.Focus();
            return;
        }
        if (ddlGroup.SelectedIndex == -1 || ddlGroup.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Owner Name", CommonClasses.MSG_Warning);
            ddlGroup.Focus();
            return;
        }
        if ((txtFoundry.Text.Trim() == "" || txtFoundry.Text.Trim() == "0") && (txtRfmStore.Text.Trim() == "" || txtRfmStore.Text.Trim() == "0") && (txtVendor.Text.Trim() == "" || txtVendor.Text.Trim() == "0") && (txtMainStore.Text.Trim() == "" || txtMainStore.Text.Trim() == "0") && (txtMachineShop.Text.Trim() == "" || txtMachineShop.Text.Trim() == "0") && (txtRFIStore.Text.Trim() == "" || txtRFIStore.Text.Trim() == "0") && (txtFinishGoods.Text.Trim() == "" || txtFinishGoods.Text.Trim() == "0"))
        {
            ShowMessage("#Avisos", "Please Enter Entry Fields", CommonClasses.MSG_Warning);
            txtFoundry.Focus();
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
                dt = CommonClasses.Execute("Select SIM_CODE FROM STANDARD_INVENTARY_MASTER WHERE SIM_I_CODE= '" + ddlItemName.SelectedValue + "' and SIM_GP_CODE ='" + ddlGroup.SelectedValue + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into STANDARD_INVENTARY_MASTER (SIM_COMP_ID,SIM_I_CODE,SIM_GP_CODE,SIM_FOUNDRY,SIM_RFM_STORE,SIM_VENDOR,SIM_MAIN_STORE,SIM_MACHINE_SHOP,SIM_RFI_STORE,SIM_FINISH_GOODS,SIM_CUST_STOCK)values('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlItemName.SelectedValue + "','" + ddlGroup.SelectedValue + "','" + txtFoundry.Text + "','" + txtRfmStore.Text + "','" + txtVendor.Text + "','" + txtMainStore.Text + "','" + txtMachineShop.Text + "','" + txtRFIStore.Text + "','" + txtFinishGoods.Text + "','" + txtCustomer_Std_Stock.Text + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SIM_CODE) from STANDARD_INVENTARY_MASTER");
                        CommonClasses.WriteLog("Standard Inventory Master", "Save", "Standard Inventory Master", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewStandardInventoryMaster.aspx", false);
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
                dt = CommonClasses.Execute("SELECT * FROM STANDARD_INVENTARY_MASTER WHERE ES_DELETE=0 AND SIM_CODE!= '" + ViewState["mlCode"] + "' and SIM_I_CODE='" + ddlItemName.SelectedValue + "' and SIM_GP_CODE='" + ddlGroup.SelectedValue + "'"); //AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("update STANDARD_INVENTARY_MASTER set SIM_I_CODE='" + ddlItemName.SelectedValue + "',SIM_GP_CODE='" + ddlGroup.SelectedValue + "',SIM_FOUNDRY='" + txtFoundry.Text + "',SIM_RFM_STORE='" + txtRfmStore.Text + "',SIM_VENDOR='" + txtVendor.Text + "',SIM_MAIN_STORE='" + txtMainStore.Text + "',SIM_MACHINE_SHOP='" + txtMachineShop.Text + "',SIM_RFI_STORE='" + txtRFIStore.Text + "',SIM_FINISH_GOODS='" + txtFinishGoods.Text + "',SIM_CUST_STOCK='" + txtCustomer_Std_Stock.Text + "' WHERE SIM_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("STANDARD_INVENTARY_MASTER", "MODIFY", "SIM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Standard Inventory Master", "Update", "Standard Inventory Master", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewStandardInventoryMaster.aspx", false);
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
            CommonClasses.SendError("Standard Inventory Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("STANDARD_INVENTARY_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Standard Inventory Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("STANDARD_INVENTARY_MASTER", "MODIFY", "SIM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewStandardInventoryMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Standard Inventory Master", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Standard Inventory Master", "CheckValid", Ex.Message);
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
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlGroup_SelectedIndexChanged
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadItem();
        //DataTable dtLoadItem = new DataTable();
        //dtLoadItem = CommonClasses.Execute("select DISTINCT I.I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from GROUP_MASTER G inner join PRODUCT_MASTER P on G.GP_CODE=P.PROD_GP_CODE INNER JOIN ITEM_MASTER I ON P.PROD_I_CODE=I.I_CODE WHERE GP_CODE='" + ddlGroup.SelectedValue + "' ORDER BY I_CODENO +' - '+ I_NAME");
        //if (dtLoadItem.Rows.Count > 0)
        //{
        //    ddlItemName.DataSource = dtLoadItem;
        //    ddlItemName.DataTextField = "ICODE_INAME";
        //    ddlItemName.DataValueField = "I_CODE";
        //    ddlItemName.DataBind();
        //    ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        //}
        //else
        //{
        //    ddlItemName.Items.Clear(); // Clear the Items form Dropdownlist
        //}
    }
    #endregion ddlGroup_SelectedIndexChanged

    #region LoadItem
    protected void LoadItem()
    {
        string str = "";

        if (ddlGroup.SelectedIndex != 0)
        {
            str = str + "GP_CODE='" + ddlGroup.SelectedValue + "' AND ";
        }
        DataTable dtFinishItem = new DataTable();
        dtFinishItem = CommonClasses.Execute("select DISTINCT I.I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from GROUP_MASTER G inner join PRODUCT_MASTER P on G.GP_CODE=P.PROD_GP_CODE INNER JOIN ITEM_MASTER I ON P.PROD_I_CODE=I.I_CODE WHERE " + str + " I.ES_DELETE=0 and G.ES_DELETE=0 AND I_CAT_CODE='-2147483648' ORDER BY I_CODENO +' - '+ I_NAME");
        // dtFinishItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_CODENO +' - '+ I_NAME");
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
