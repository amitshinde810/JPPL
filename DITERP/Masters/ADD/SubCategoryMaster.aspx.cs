using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Masters_ADD_SubCategoryMaster : System.Web.UI.Page
{
    #region General Declaration
    StateMaster_BL BL_StateMaster = null;
    static int mlCode = 0;
    static string right = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='34'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                LoadItemCategory();

                if (Request.QueryString[0].Equals("VIEW"))
                {
                    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                    ViewRec("VIEW");
                }
                else if (Request.QueryString[0].Equals("MODIFY"))
                {
                    mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                    ViewRec("MOD");
                }
            }
            catch (Exception Ex)
            {
                CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "Page_Load", Ex.Message);
            }
        }
    }
    #region ViewRec
    private void ViewRec(string str)
    {


        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("select * from ITEM_SUBCATEGORY_MASTER where SCAT_CODE="+mlCode);
            ddlItemCategory.SelectedValue =dt.Rows[0]["SCAT_CAT_CODE"].ToString();
            txtSubCategoryDesc.Text = dt.Rows[0]["SCAT_DESC"].ToString();
            if (str == "VIEW")
            {
              
                txtSubCategoryDesc.Enabled = false;
                ddlItemCategory.Enabled = false;
                btnSubmit.Visible = false;
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("ITEM_SUBCATEGORY_MASTER", "MODIFY", "SCAT_CODE", mlCode);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    private void LoadItemCategory()
    {
        DataTable dt = new DataTable();
        try
        {
            try
            {
                dt = CommonClasses.Execute("select I_CAT_CODE,I_CAT_NAME from ITEM_CATEGORY_MASTER where ES_DELETE=0 and I_CAT_CM_COMP_ID='" + (string)Session["CompanyId"] + "'");
                ddlItemCategory.DataSource = dt;
                ddlItemCategory.DataTextField = "I_CAT_NAME";
                ddlItemCategory.DataValueField = "I_CAT_CODE";
                ddlItemCategory.DataBind();
                ddlItemCategory.Items.Insert(0, new ListItem("Select Item Category", "0"));

            }
            catch (Exception ex)
            { }
            finally
            {
                dt.Dispose();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "LoadItemCategory", Ex.Message);

        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveRec();
    }

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            string StrReplaceStateName = txtSubCategoryDesc.Text;


            StrReplaceStateName = StrReplaceStateName.Replace("'", "''");

            if (Request.QueryString[0].Equals("INSERT"))
            {

                if (CommonClasses.Execute("SELECT SCAT_CODE,SCAT_CM_COMP_ID,SCAT_DESC,SCAT_CAT_CODE,ES_DELETE,MODIFY FROM ITEM_SUBCATEGORY_MASTER where SCAT_DESC='" + txtSubCategoryDesc.Text + "' and SCAT_CAT_CODE=" + ddlItemCategory.SelectedValue + "").Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("insert into ITEM_SUBCATEGORY_MASTER(SCAT_CM_COMP_ID,SCAT_DESC,SCAT_CAT_CODE,ES_DELETE,MODIFY)values(" + Convert.ToInt32(Session["CompanyId"]) + ",'" + txtSubCategoryDesc.Text + "'," + ddlItemCategory.SelectedValue + ",0,0)"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SCAT_CODE) from ITEM_SUBCATEGORY_MASTER");
                        CommonClasses.WriteLog("item Sub category Master", "Save", "item Sub category Master", txtSubCategoryDesc.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]),  Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/ViewSubCategoryMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_StateMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_StateMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                            BL_StateMaster.Msg = "";
                        }
                        
                        ddlItemCategory.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos","Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                    //lblmsg.Text = "Record Already Exists";
                    //PanelMsg.Visible = true;
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                DataTable dt = CommonClasses.Execute("SELECT * FROM ITEM_SUBCATEGORY_MASTER where SCAT_CODE!=" + mlCode + " and SCAT_DESC='" + txtSubCategoryDesc.Text + "' ");
                if(dt.Rows.Count==0)
                {

                    if (CommonClasses.Execute1("UPDATE ITEM_SUBCATEGORY_MASTER SET SCAT_DESC = '" + txtSubCategoryDesc.Text + "',SCAT_CAT_CODE = " + ddlItemCategory.SelectedValue + " where SCAT_CODE=" + mlCode + ""))
                    {
                        CommonClasses.RemoveModifyLock("ITEM_SUBCATEGORY_MASTER", "MODIFY", "SCAT_CODE", mlCode);
                        CommonClasses.WriteLog("ITEM SUB CATEGORY MASTER", "Update", "ITEM SUBCATEGORY MASTER", txtSubCategoryDesc.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/View/ViewSubCategoryMaster.aspx", false);
                    }
                    else
                    {
                        if (BL_StateMaster.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_StateMaster.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                            BL_StateMaster.Msg = "";
                        }
                        ddlItemCategory.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Record Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                }
                
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("ITEM SUBCATEGORY MASTER", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

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
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "btnCancel_Click", ex.Message.ToString());
        }
    }
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtSubCategoryDesc.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlItemCategory.SelectedIndex <= 0)
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
            CommonClasses.SendError("Item Sub Category Master", "CheckValid", Ex.Message);
        }

        return flag;
    }
    private void CancelRecord()
    {
        txtSubCategoryDesc.Text = "";
        ddlItemCategory.SelectedIndex = 0;
        if (mlCode != 0 && mlCode != null)
        {
            CommonClasses.RemoveModifyLock("ITEM_SUBCATEGORY_MASTER", "MODIFY", "SM_CODE", mlCode);
        }
        Response.Redirect("~/Masters/View/ViewSubCategoryMaster.aspx", false);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        // SaveRec();
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
}
