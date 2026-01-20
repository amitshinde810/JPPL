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

public partial class Masters_VIEW_ViewSubCategoryMaster : System.Web.UI.Page
{
    #region Variables
    StateMaster_BL BL_StateMaster = null;   
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='34'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadCategory();
                    if (dgItemSubCategory.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_DESC", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(String)));


                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgItemSubCategory.DataSource = dtFilter;
                            dgItemSubCategory.DataBind();
                            dgItemSubCategory.Enabled = false;
                        }
                    }         

                    
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "Page_Load", Ex.Message);
        }
    }

    #region LoadCategory
    private void LoadCategory()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT SCAT_CODE,SCAT_DESC,I_CAT_NAME FROM ITEM_SUBCATEGORY_MASTER,ITEM_CATEGORY_MASTER WHERE I_CAT_CODE=SCAT_CAT_CODE and SCAT_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_SUBCATEGORY_MASTER.ES_DELETE=0 order by I_CAT_NAME,SCAT_DESC");

            dgItemSubCategory.DataSource = dt;
            dgItemSubCategory.DataBind();
            dgItemSubCategory.Enabled = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "LoadCategory", Ex.Message);
        }
    }
    #endregion

    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "txtString_TextChanged", Ex.Message);
        }
    }
    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT SCAT_CODE,SCAT_DESC,I_CAT_NAME FROM ITEM_SUBCATEGORY_MASTER,ITEM_CATEGORY_MASTER WHERE I_CAT_CODE=SCAT_CAT_CODE and SCAT_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_SUBCATEGORY_MASTER.ES_DELETE=0 and (SCAT_DESC like upper('%" + str + "%') OR I_CAT_NAME like upper('%" + str + "%')) order by I_CAT_NAME,SCAT_DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT SCAT_CODE,SCAT_DESC,I_CAT_NAME FROM ITEM_SUBCATEGORY_MASTER,ITEM_CATEGORY_MASTER WHERE I_CAT_CODE=SCAT_CAT_CODE and SCAT_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_SUBCATEGORY_MASTER.ES_DELETE=0 order by I_CAT_NAME,SCAT_DESC");

            if (dtfilter.Rows.Count > 0)
            {

                dgItemSubCategory.DataSource = dtfilter;
                dgItemSubCategory.DataBind();
                dgItemSubCategory.Enabled = true;
            }
            else
            {
                dtFilter.Clear();


                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SCAT_DESC", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(String)));


                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgItemSubCategory.DataSource = dtFilter;
                    dgItemSubCategory.DataBind();
                    dgItemSubCategory.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "LoadStatus", ex.Message);
        }
    }

    #endregion
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Masters/Add/SubCategoryMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";

                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "btnAddNew_Click", Ex.Message);
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER - View", "btnClose_Click", ex.Message);
        }
    }
    protected void dgItemSubCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    protected void dgItemSubCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string s_code = ((Label)(dgItemSubCategory.Rows[e.NewEditIndex].FindControl("lblSCAT_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/Add/SubCategoryMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "dgItemSubCategory_RowEditing", Ex.Message);
        }
    }
    protected void dgItemSubCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgItemSubCategory.Rows[e.RowIndex].FindControl("lblSCAT_CODE"))).Text))
                {

                    string s_code = ((Label)(dgItemSubCategory.Rows[e.RowIndex].FindControl("lblSCAT_CODE"))).Text;
                    string s_name = ((Label)(dgItemSubCategory.Rows[e.RowIndex].FindControl("lblSCAT_DESC"))).Text;

                    if (CommonClasses.CheckUsedInTran("ITEM_MASTER", "I_SCAT_CODE", "AND ES_DELETE=0", s_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record because it has used in Item Master";

                        //ShowMessage("#Avisos", "You cant delete this record it has used in city master", CommonClasses.MSG_Warning);
                    }
                    else
                    {
                    bool flag = CommonClasses.Execute1("UPDATE ITEM_SUBCATEGORY_MASTER SET ES_DELETE = 1 WHERE SCAT_CODE='" + Convert.ToInt32(s_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("ITEM_SUBCATEGORY_MASTER", "Delete", "ITEM_SUBCATEGORY_MASTER", s_code, Convert.ToInt32(s_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            // ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";



                        }
                         LoadCategory();
                    }                  
                   
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                    //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                    return;
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                //    //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                //   return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "dgItemSubCategory_RowDeleting", Ex.Message);
        }
    }
    protected void dgItemSubCategory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {

                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "VIEW";
                        string s_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/Add/SubCategoryMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
                    return;
                }

            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {

                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string s_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/Add/SubCategoryMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                        // ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "dgItemSubCategory_RowCommand", Ex.Message);
        }
    }
    protected void dgItemSubCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgItemSubCategory.PageIndex = e.NewPageIndex;
            LoadCategory();
        }
        catch (Exception)
        {
        }
    }
    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
         
            DataTable dt = CommonClasses.Execute("select MODIFY from ITEM_SUBCATEGORY_MASTER where SCAT_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
    
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("ITEM_SUBCATEGORY_MASTER", "ModifyLog", Ex.Message);
        }

        return false;
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
}
