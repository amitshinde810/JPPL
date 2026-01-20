using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Masters_PalletMaster : System.Web.UI.Page
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
                        LoadItems(); LoadPallets(); // Method call
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
                        CommonClasses.SendError("Pallet Master", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Pallet Master", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            LoadItems(); LoadPallets();
            dt = CommonClasses.Execute("SELECT DISTINCT PAL_CODE,I_CODE,I_CODENO +' - '+ I_NAME AS ICODE_INAME,I_NAME ,(SELECT I_CODE from PALLET_MASTER inner join ITEM_MASTER ON PAL_PALLET_I_TRAY=I_CODE where PALLET_MASTER.PAL_CODE=P.PAL_CODE and PALLET_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0  and I_CM_COMP_ID='1') as I_CODE1,(SELECT I_NAME from PALLET_MASTER inner join ITEM_MASTER ON PAL_PALLET_I_TRAY=I_CODE where PALLET_MASTER.PAL_CODE=P.PAL_CODE and PALLET_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0  and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "') as PAL_PALLET_I_TRAY,isnull(PAL_BOX_QTY,0) as PAL_BOX_QTY FROM PALLET_MASTER P INNER JOIN ITEM_MASTER I ON P.PAL_I_CODE=I.I_CODE where P.ES_DELETE=0 and I.ES_DELETE=0 and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and PAL_CODE='" + ViewState["mlCode"] + "'");
            if (dt.Rows.Count > 0)
            {
                txtBoxQty.Text = dt.Rows[0]["PAL_BOX_QTY"].ToString();
                ddlPartName.SelectedValue = dt.Rows[0]["I_CODE"].ToString();
                ddlPallet.SelectedValue = dt.Rows[0]["I_CODE1"].ToString();
                if (str == "VIEW")
                {
                    txtBoxQty.Enabled = false; ddlPartName.Enabled = false; ddlPallet.Enabled = false; btnSubmit.Visible = false;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PALLET_MASTER", "MODIFY", "PAL_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Pallet Master", "ViewRec", ex.Message);
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
        if (ddlPallet.SelectedIndex == -1 || ddlPallet.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Please Select Pallet Name", CommonClasses.MSG_Warning);
            ddlPallet.Focus();
            return;
        }
        if (txtBoxQty.Text.Trim() == "" || Convert.ToInt32(txtBoxQty.Text.Trim()) <= 0)
        {
            ShowMessage("#Avisos", "Please Insert Number", CommonClasses.MSG_Warning);
            ddlPallet.Focus();
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
                dt = CommonClasses.Execute("Select PAL_CODE FROM PALLET_MASTER WHERE PAL_I_CODE= '" + ddlPartName.SelectedValue + "' AND PAL_PALLET_I_TRAY= '" + ddlPallet.SelectedValue + "' AND PAL_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='False'");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("INSERT INTO PALLET_MASTER(PAL_COMP_ID,PAL_I_CODE,PAL_PALLET_I_TRAY,PAL_BOX_QTY)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlPartName.SelectedValue + "','" + ddlPallet.SelectedValue + "','" + txtBoxQty.Text.Trim().Replace("'", "\''") + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PAL_CODE) from PALLET_MASTER");
                        CommonClasses.WriteLog("Pallet Master", "Save", "Pallet Master", ddlPartName.SelectedValue, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewPalletMaster.aspx", false);
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
                dt = CommonClasses.Execute("SELECT * FROM PALLET_MASTER WHERE ES_DELETE=0 AND PAL_CODE!= '" + Convert.ToInt32(ViewState["mlCode"]) + "' AND PAL_I_CODE= '" + ddlPartName.SelectedValue + "' AND PAL_PALLET_I_TRAY= '" + ddlPallet.SelectedValue + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE PALLET_MASTER SET PAL_I_CODE='" + ddlPartName.SelectedValue + "',PAL_PALLET_I_TRAY='" + ddlPallet.SelectedValue + "',PAL_BOX_QTY='" + txtBoxQty.Text.Trim().Replace("'", "\''") + "' WHERE PAL_CODE='" + ViewState["mlCode"] + "'"))
                    {
                        CommonClasses.RemoveModifyLock("PALLET_MASTER", "MODIFY", "PAL_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Pallet Master", "Update", "Pallet Master", ddlPartName.SelectedValue, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/PPC/VIEW/ViewPalletMaster.aspx", false);
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
            CommonClasses.SendError("Pallet Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("PALLET_MASTER", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Pallet Master", "btnCancel_Click", ex.Message.ToString());
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
                CommonClasses.RemoveModifyLock("PALLET_MASTER", "MODIFY", "PAL_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/PPC/VIEW/ViewPalletMaster.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Pallet Master", "btnCancel_Click", ex.Message);
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
            CommonClasses.SendError("Pallet Master", "CheckValid", Ex.Message);
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

    #region LoadPallets
    protected void LoadPallets()
    {
        DataTable dtTrayItems = new DataTable();
        /*Tray :--2147483633 and  PACKAGING MATERIAL=-2147483619*/
        dtTrayItems = CommonClasses.Execute("SELECT DISTINCT I_CODE as I_CODE1,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CAT_CODE=-2147483619 and I_CM_COMP_ID='" + Session["CompanyId"] + "' order by I_NAME");
        ddlPallet.DataSource = dtTrayItems;
        ddlPallet.DataTextField = "I_NAME";
        ddlPallet.DataValueField = "I_CODE1";
        ddlPallet.DataBind();
        ddlPallet.Items.Insert(0, new ListItem("Select Pallet Name", "0"));
    }
    #endregion LoadPallets
}
