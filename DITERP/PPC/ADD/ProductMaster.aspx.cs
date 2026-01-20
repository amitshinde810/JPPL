using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_ProductMaster : System.Web.UI.Page
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
                        LoadItem(); LoadGroup(); LoadLine();

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
                        CommonClasses.SendError("Product Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Product Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadItem();
            LoadGroup();
            //case when LOWER(PROD_RAW_TYPE) ='LM25' then 1 when LOWER(PROD_RAW_TYPE) ='AC4B' then 2 else 0 end as PROD_RAW_TYPE
            dt = CommonClasses.Execute("select DISTINCT P.PROD_CODE, I_CODENO +' - '+ I_NAME AS PROD_NAME,PROD_RAW_TYPE, G.GP_NAME,case when LOWER(P.PROD_INV_TYPE)='RUNNER' then 1 when LOWER(P.PROD_INV_TYPE)='REPEATER' then 2 when LOWER(P.PROD_INV_TYPE)='STRAINGER' then 3 else 0 end as PROD_INV_TYPE,isnull(PROD_CORE_WT,0) AS PROD_CORE_WT,isnull(PROD_CAST_WT,0) AS PROD_CAST_WT,isnull(PROD_FINISH_WT,0) AS PROD_FINISH_WT ,case when LOWER(PROD_RAW_TYPE) ='LM24' then 1 when LOWER(PROD_RAW_TYPE) ='LM25' then 2 when LOWER(PROD_RAW_TYPE) ='LM26' then 3 when LOWER(PROD_RAW_TYPE) ='LM28' then 4 when LOWER(PROD_RAW_TYPE) ='AC4B' then 5 else 0 end as PROD_RAW_TYPE,case when LOWER(PROD_MACHINE_LOC)='INHOUSE' then 1 when LOWER(PROD_MACHINE_LOC)='OFFLOADED' then 2 when LOWER(PROD_MACHINE_LOC)='BOTH' then 3 else 0  end as PROD_MACHINE_LOC,I.I_CODE,G.GP_CODE,[PROD_RFM_STORE]  ,[PROD_VENDOR]  ,[PROD_MAIN_STORE]  ,[PROD_MACHINE_SHOP]  ,[PROD_RFI_STORE],PROD_MTYPE  ,[PROD_RFD_STORE],case when PROD_SAND='Sand' THEN 1 else 0 end as PROD_SAND ,isnull((select LM_CODE from LINE_MASTER L where P.PROD_LINE=L.LM_CODE AND L.ES_DELETE=0  ),0) as LM_CODE from PRODUCT_MASTER P inner join ITEM_MASTER I on P.PROD_I_CODE=I.I_CODE INNER JOIN GROUP_MASTER G ON P.PROD_GP_CODE=G.GP_CODE where P.PROD_COMP_ID=1 AND P.ES_DELETE=0 AND G.ES_DELETE=0 AND I.ES_DELETE=0 AND P.PROD_COMP_ID ='" + (string)Session["CompanyId"] + "' AND P.PROD_CODE= '" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                ddlItemName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                ddlGroup.SelectedValue = dt.Rows[0]["GP_CODE"].ToString();
                ddlInventory.SelectedValue = dt.Rows[0]["PROD_INV_TYPE"].ToString();
                ddlRaw.SelectedValue = dt.Rows[0]["PROD_RAW_TYPE"].ToString();
                ddlSand.SelectedValue = dt.Rows[0]["PROD_SAND"].ToString();
                ddlMachining.SelectedValue = dt.Rows[0]["PROD_MACHINE_LOC"].ToString();
                txtCoreWt.Text = dt.Rows[0]["PROD_CORE_WT"].ToString();
                txtCastWt.Text = dt.Rows[0]["PROD_CAST_WT"].ToString();
                txtFinishWt.Text = dt.Rows[0]["PROD_FINISH_WT"].ToString();
                ddlLine.SelectedValue = dt.Rows[0]["LM_CODE"].ToString();
                txtRfmStore.Text = dt.Rows[0]["PROD_RFM_STORE"].ToString();
                txtVendor.Text = dt.Rows[0]["PROD_VENDOR"].ToString();
                txtMainStore.Text = dt.Rows[0]["PROD_MAIN_STORE"].ToString();
                txtMachineShop.Text = dt.Rows[0]["PROD_MACHINE_SHOP"].ToString();
                txtRFIStore.Text = dt.Rows[0]["PROD_RFI_STORE"].ToString();
                txtRFIStore.Text = dt.Rows[0]["PROD_RFD_STORE"].ToString();
                rbtGroup.SelectedValue = dt.Rows[0]["PROD_MTYPE"].ToString();
                if (str == "VIEW")
                {
                    if (ddlMachining.SelectedValue == "3")
                    {
                        rbtGroup.Visible = true;
                    }
                    else
                    {
                        rbtGroup.Visible = false;
                    }
                    ddlItemName.Enabled = false; ddlGroup.Enabled = false; ddlInventory.Enabled = false; txtCoreWt.Enabled = false;
                    txtCastWt.Enabled = false; txtFinishWt.Enabled = false; ddlRaw.Enabled = false; ddlMachining.Enabled = false; ddlLine.Enabled = false;
                    btnSubmit.Visible = false; rbtGroup.Enabled = false; ddlSand.Enabled = false;
                }
            }
            if (str == "MOD")
            {
                if (ddlMachining.SelectedValue == "3")
                {
                    rbtGroup.Visible = true;
                }
                else
                {
                    rbtGroup.Visible = false;
                }
                CommonClasses.SetModifyLock("PRODUCT_MASTER", "MODIFY", "GP_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Product Master", "ViewRec", ex.Message);
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
            ShowMessage("#Avisos", "Please Enter Group Name", CommonClasses.MSG_Warning);
            ddlGroup.Focus();
            return;
        }
        if (ddlRaw.SelectedIndex == -1 || ddlRaw.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Raw Type", CommonClasses.MSG_Warning);
            ddlRaw.Focus();
            return;
        }
        if (ddlMachining.SelectedIndex == -1 || ddlMachining.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Machining", CommonClasses.MSG_Warning);
            ddlMachining.Focus();
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
                dt = CommonClasses.Execute("Select PROD_CODE,PROD_I_CODE FROM PRODUCT_MASTER WHERE PROD_I_CODE= '" + ddlItemName.SelectedValue + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into PRODUCT_MASTER (PROD_COMP_ID,PROD_I_CODE,PROD_GP_CODE,PROD_INV_TYPE,PROD_CORE_WT,PROD_CAST_WT,PROD_FINISH_WT,PROD_RAW_TYPE,PROD_MACHINE_LOC,PROD_LINE ,[PROD_RFM_STORE]  ,[PROD_VENDOR]  ,[PROD_MAIN_STORE]  ,[PROD_MACHINE_SHOP]  ,[PROD_RFI_STORE]  ,[PROD_RFD_STORE],PROD_SAND,PROD_MTYPE) values('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlItemName.SelectedValue + "','" + ddlGroup.SelectedValue + "','" + ddlInventory.SelectedItem + "','" + txtCoreWt.Text.ToString() + "','" + txtCastWt.Text.ToString() + "','" + txtFinishWt.Text.ToString() + "','" + ddlRaw.SelectedItem + "','" + ddlMachining.SelectedItem + "','" + ddlLine.SelectedValue + "','" + txtRfmStore.Text + "','" + txtVendor.Text + "','" + txtMainStore.Text + "','" + txtMachineShop.Text + "','" + txtRFIStore.Text + "','" + txtRFDStore.Text + "','" + ddlSand.SelectedItem + "'," + Convert.ToInt32(rbtGroup.SelectedValue) + ")"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PROD_CODE) from PRODUCT_MASTER");
                        CommonClasses.WriteLog("Product Master", "Save", "Product Master", ddlItemName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProductMaster.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlItemName.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Item Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM PRODUCT_MASTER WHERE ES_DELETE=0 AND PROD_CODE!= '" + ViewState["mlCode"] + "' and PROD_I_CODE='" + ddlItemName.SelectedValue + "' and PROD_GP_CODE='" + ddlGroup.SelectedValue + "' and PROD_INV_TYPE='" + ddlInventory.SelectedItem + "'"); //AND lower(GP_NAME) = lower('" + txtGroupName.Text + "')
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PRODUCT_MASTER SET PROD_I_CODE='" + ddlItemName.SelectedValue + "',PROD_GP_CODE='" + ddlGroup.SelectedValue + "',PROD_INV_TYPE='" + ddlInventory.SelectedItem + "',PROD_CORE_WT='" + txtCoreWt.Text + "',PROD_CAST_WT='" + txtCastWt.Text + "',PROD_FINISH_WT='" + txtFinishWt.Text + "',PROD_RAW_TYPE='" + ddlRaw.SelectedItem + "',PROD_MACHINE_LOC='" + ddlMachining.SelectedItem + "',PROD_LINE='" + ddlLine.SelectedValue + "',[PROD_RFM_STORE]='" + txtRfmStore.Text + "',[PROD_VENDOR]='" + txtVendor.Text + "',[PROD_MAIN_STORE]='" + txtMainStore.Text + "',[PROD_MACHINE_SHOP]='" + txtMachineShop.Text + "',[PROD_RFI_STORE]='" + txtRFIStore.Text + "',[PROD_RFD_STORE]='" + txtRFIStore.Text + "',PROD_SAND='" + ddlSand.SelectedItem + "',PROD_MTYPE=" + Convert.ToInt32(rbtGroup.SelectedValue) + " WHERE PROD_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PRODUCT_MASTER", "MODIFY", "PROD_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Product Master", "Update", "Product Master", ddlItemName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewProductMaster.aspx", false);
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
                    ShowMessage("#Avisos", "Item Name Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemName.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Product Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("PRODUCT_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Product Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("PRODUCT_MASTER", "MODIFY", "PROD_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewProductMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Product Master", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Product Master", "CheckValid", Ex.Message);
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
        DataTable dtItem = new DataTable();
        DataTable dtLine = new DataTable();
        LoadLine();
        dtItem = CommonClasses.Execute("select isnull(I_UWEIGHT,0) as CastWt,isnull(I_DENSITY,0) As CoreWt, isnull(I_TARGET_WEIGHT,0) As FinishWt from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE='-2147483648' and I_CM_COMP_ID='" + Session["CompanyId"] + "' and I_CODE='" + ddlItemName.SelectedValue + "'");
        dtLine = CommonClasses.Execute("SELECT DISTINCT LM_NAME,LM_CODE FROM LINE_MASTER INNER JOIN  LINE_CHANGE ON LINE_MASTER.LM_CODE = LINE_CHANGE.LC_LM_CODE  INNER JOIN  ITEM_MASTER ON LINE_CHANGE.LC_I_CODE = ITEM_MASTER.I_CODE WHERE (LINE_MASTER.ES_DELETE = 0) AND LM_CM_ID = '" + Session["CompanyId"] + "' AND LC_ACTIVE=1 and I_CODE='" + ddlItemName.SelectedValue + "'");
        if (dtItem.Rows.Count > 0)
        {
            txtCastWt.Text = dtItem.Rows[0]["CastWt"].ToString();
            //txtCoreWt.Text = dtItem.Rows[0]["CoreWt"].ToString();
            txtFinishWt.Text = dtItem.Rows[0]["FinishWt"].ToString();
        }
        if (dtLine.Rows.Count > 0)
        {
            ddlLine.SelectedValue = dtLine.Rows[0]["LM_CODE"].ToString();
        }
    }
    #endregion ddlItemName_SelectedIndexChanged

    #region ddlLine_SelectedIndexChanged
    protected void ddlLine_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion ddlLine_SelectedIndexChanged

    #region ddlGroup_SelectedIndexChanged
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion ddlGroup_SelectedIndexChanged

    protected void ddlMachining_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMachining.SelectedValue == "3")
        {
            rbtGroup.Visible = true;
        }
        else
        {
            rbtGroup.Visible = false;
        }
    }

    #region LoadItem
    protected void LoadItem()
    {
        DataTable dtFinishItem = new DataTable();
        // Load Only (FINISH GOODS) I_COSTING_HEAD='FINISH GOODS'
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

    #region LoadLine
    protected void LoadLine()
    {
        DataTable dtLine = new DataTable();
        dtLine = CommonClasses.Execute("SELECT distinct LM_CODE,LM_NAME FROM LINE_MASTER where ES_DELETE=0 and LM_CM_ID='" + Session["CompanyId"] + "' ORDER BY LM_NAME");
        ddlLine.DataSource = dtLine;
        ddlLine.DataTextField = "LM_NAME";
        ddlLine.DataValueField = "LM_CODE";
        ddlLine.DataBind();
        ddlLine.Items.Insert(0, new ListItem("Line Name", "0"));
    }
    #endregion LoadLine
}
