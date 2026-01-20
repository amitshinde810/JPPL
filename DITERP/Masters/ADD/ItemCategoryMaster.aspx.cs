using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Masters_ADD_ItemCategoryMaster : System.Web.UI.Page
{

    #region General Declaration
    ItemCategoryMaster_BL BL_ItemCategoryMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");

        home.Attributes["class"] = "active";

        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");

        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }

        else
        {
            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='9'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_ItemCategoryMaster = new ItemCategoryMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        BL_ItemCategoryMaster = new ItemCategoryMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    txtItemCategoryName.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Item Category Master", "PageLoad", ex.Message);
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
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            {
                SaveRec();
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Item Category Master", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    //#region btnClose_Click
    //protected void btnClose_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (mlCode != 0 && mlCode != null)
    //        {
    //            CommonClasses.RemoveModifyLock("ITEM_CATEGORY_MASTER", "MODIFY", "I_CAT_CODE", mlCode);
    //        }
    //        Response.Redirect("~/Masters/VIEW/ViewItemCategoryMaster.aspx", false);
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Item Category Master", "btnClose_Click", Ex.Message);
    //    }
    //}
    //#endregion btnClose_Click

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
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Item Category Master", "btnCancel_Click", ex.Message.ToString());
        }

    }

   
    #endregion
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtItemCategoryName.Text.Trim() == "")
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
            CommonClasses.SendError("Item Category Master", "CheckValid", Ex.Message);
        }

        return flag;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ITEM_CATEGORY_MASTER", "MODIFY", "I_CAT_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewItemCategoryMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Master", "btnCancel_Click", Ex.Message);
        }
    } 
   
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            BL_ItemCategoryMaster = new ItemCategoryMaster_BL(mlCode);
            DataTable dt = new DataTable();
            BL_ItemCategoryMaster.GetInfo();
            GetValues(str);
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_CATEGORY_MASTER", "MODIFY", "I_CAT_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                txtItemCategoryName.Text = BL_ItemCategoryMaster.I_CAT_NAME;
                chkAuto.Checked = BL_ItemCategoryMaster.I_CAT_SHORTCLOSE;
                txtItemCategoryName.Enabled = false;
                
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
                txtItemCategoryName.Text = BL_ItemCategoryMaster.I_CAT_NAME;
                chkAuto.Checked = BL_ItemCategoryMaster.I_CAT_SHORTCLOSE;
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

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            if (txtItemCategoryName.Text.ToUpper().Trim()!="")
            {

                BL_ItemCategoryMaster.I_CAT_NAME = txtItemCategoryName.Text.ToUpper().Trim().Replace("'", "\'");

                BL_ItemCategoryMaster.I_CAT_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
                BL_ItemCategoryMaster.I_CAT_SHORTCLOSE = chkAuto.Checked;
                res = true;  
            }
            else
            {
                txtItemCategoryName.Text = txtItemCategoryName.Text.ToUpper().Trim().Replace("'", "\'");
                ShowMessage("#Avisos", "Please Enter Category", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtItemCategoryName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Category Master", "Setvalues", ex.Message);
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
                BL_ItemCategoryMaster = new ItemCategoryMaster_BL();
                if (Setvalues())
                {
                    if (BL_ItemCategoryMaster.Save())
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(I_CAT_CODE) from ITEM_CATEGORY_MASTER");
                        CommonClasses.WriteLog("Item Category Master", "Save", "Item Category Master", BL_ItemCategoryMaster.I_CAT_NAME, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewItemCategoryMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_ItemCategoryMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ItemCategoryMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                            //lblmsg.Text = BL_ItemCategoryMaster.Msg;
                            //PanelMsg.Visible = true;
                            BL_ItemCategoryMaster.Msg = "";
                        }
                        txtItemCategoryName.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_ItemCategoryMaster = new ItemCategoryMaster_BL(mlCode);
                if (Setvalues())
                {
                    if (BL_ItemCategoryMaster.Update())
                    {
                        CommonClasses.RemoveModifyLock("ITEM_CATEGORY_MASTER", "MODIFY", "I_CAT_CODE", mlCode);
                        CommonClasses.WriteLog("ITEM CATEGORY MASTER", "Update", "ITEM CATEGORY MASTER", BL_ItemCategoryMaster.I_CAT_NAME, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewItemCategoryMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_ItemCategoryMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_ItemCategoryMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                            BL_ItemCategoryMaster.Msg = "";
                        }
                        txtItemCategoryName.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Category Master", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Item Category Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}