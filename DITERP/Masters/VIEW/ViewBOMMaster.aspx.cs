using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewBOMMaster : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='44'");
                    right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                    LoadBOM();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material-View", "Page_Load", Ex.Message);
        }
    }

    #endregion

    #region LoadBOM
    private void LoadBOM()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT TOP 500 BM_CODE,I_CODENO,I_NAME,I_CODE FROM BOM_MASTER,ITEM_MASTER WHERE BM_I_CODE = I_CODE AND ITEM_MASTER.ES_DELETE = 0 AND BOM_MASTER.ES_DELETE = 0 AND BM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_CODENO");
            if (dt.Rows.Count <= 0)
            {
                dgBillofMaterial.Enabled = false;
                dt.Rows.Add(dt.NewRow());
                dgBillofMaterial.DataSource = dt;
                dgBillofMaterial.DataBind();
            }
            else
            {
                dgBillofMaterial.Enabled = true;
                dgBillofMaterial.DataSource = dt;
                dgBillofMaterial.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Master - View", "LoadUnit", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Of Material", "txtString_TextChanged", ex.Message);
        }
    }

    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT TOP 500 BM_CODE,I_CODENO,I_NAME,I_CODE FROM BOM_MASTER,ITEM_MASTER WHERE BM_I_CODE = I_CODE AND ITEM_MASTER.ES_DELETE = 0 AND BOM_MASTER.ES_DELETE = 0 AND BM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "'  and (I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) ORDER BY I_CODENO");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 BM_CODE,I_CODENO,I_NAME,I_CODE FROM BOM_MASTER,ITEM_MASTER WHERE BM_I_CODE = I_CODE AND ITEM_MASTER.ES_DELETE = 0 AND BOM_MASTER.ES_DELETE = 0 AND BM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_CODENO");

            if (dtfilter.Rows.Count > 0)
            {
                dgBillofMaterial.Enabled = true;
                dgBillofMaterial.DataSource = dtfilter;
                dgBillofMaterial.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgBillofMaterial.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("BM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgBillofMaterial.DataSource = dtFilter;
                    dgBillofMaterial.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill OF Material", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Masters/ADD/BOMaterial.aspx?c_name=" + type, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgBillofMaterial_RowDeleting
    protected void dgBillofMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgBillofMaterial.Rows[e.RowIndex].FindControl("lblBM_CODE"))).Text))
            {

                string bm_code = ((Label)(dgBillofMaterial.Rows[e.RowIndex].FindControl("lblBM_CODE"))).Text;
                string i_name = ((Label)(dgBillofMaterial.Rows[e.RowIndex].FindControl("lblI_NAME"))).Text;
                string i_code = ((Label)(dgBillofMaterial.Rows[e.RowIndex].FindControl("lblI_CODENO"))).Text;
                string ItemCode = ((Label)(dgBillofMaterial.Rows[e.RowIndex].FindControl("lblI_CODE"))).Text;
                if (CommonClasses.CheckUsedInTran("MATERIAL_REQUISITION_MASTER", "MR_I_CODE", "AND ES_DELETE=0", ItemCode))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record it has used in Material Requisition";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
                else
                {
                    bool flag = CommonClasses.Execute1("UPDATE BOM_MASTER SET ES_DELETE = 1 WHERE BM_CODE='" + Convert.ToInt32(bm_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Bill Of Material", "Delete", "Bill Of Material", bm_code, Convert.ToInt32(bm_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
            LoadBOM();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgBillofMaterial_RowEditing
    protected void dgBillofMaterial_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string bm_code = ((Label)(dgBillofMaterial.Rows[e.NewEditIndex].FindControl("lblBM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/BOMaterial.aspx?c_name=" + type + "&bm_code=" + bm_code, false);
            }
            else
            {
                ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material - View", "dgBillofMaterial_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgBillofMaterial_PageIndexChanging
    protected void dgBillofMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgBillofMaterial.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgBillofMaterial_RowCommand
    protected void dgBillofMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string bm_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/BOMaterial.aspx?c_name=" + type + "&bm_code=" + bm_code, false);
                }
                else
                {
                    lblmsg.Text = "You Have No Rights To View";
                    PanelMsg.Visible = true;
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
                        string bm_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/BOMaterial.aspx?c_name=" + type + "&bm_code=" + bm_code, false);
                    }
                    else
                    {
                        lblmsg.Text = "Record Used By Another Person";
                        PanelMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    lblmsg.Text = "You Have No Rights To Modify";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material - View", "dgBillofMaterial_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgBillofMaterial_RowUpdating
    protected void dgBillofMaterial_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from BOM_MASTER where BM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    lblmsg.Text = "Record Used By Another Person";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Bill Of Material - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
