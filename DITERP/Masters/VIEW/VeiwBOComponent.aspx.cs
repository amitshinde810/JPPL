using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_VeiwBOComponent : System.Web.UI.Page
{

    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();


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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='12'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadBOC();
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component-View", "Page_Load", Ex.Message);
        }

    }

    #region LoadBOC
    private void LoadBOC()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT BM_CODE,I_CODENO,I_NAME FROM BOC_MASTER,ITEM_MASTER WHERE BM_I_CODE = I_CODE AND ITEM_MASTER.ES_DELETE = 0 AND BOC_MASTER.ES_DELETE = 0 AND BM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "'");

            dgBillofComponent.DataSource = dt;
            dgBillofComponent.DataBind();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Master - View", "LoadUnit", Ex.Message);
        }
    }
    #endregion
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
           LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Of Component", "btnSearch_Click", ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT BM_CODE,I_CODENO,I_NAME FROM BOC_MASTER,ITEM_MASTER WHERE BM_I_CODE = I_CODE AND ITEM_MASTER.ES_DELETE = 0 AND BOC_MASTER.ES_DELETE = 0 AND BM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND ITEM_MASTER.I_CAT_CODE='-2147483647' and (I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%'))");
            else
                dtfilter = CommonClasses.Execute("SELECT BM_CODE,I_CODENO,I_NAME FROM BOC_MASTER,ITEM_MASTER WHERE BM_I_CODE = I_CODE AND ITEM_MASTER.ES_DELETE = 0 AND BOC_MASTER.ES_DELETE = 0 AND BM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND ITEM_MASTER.I_CAT_CODE='-2147483647'");

            if (dtfilter.Rows.Count > 0)
            {

                dgBillofComponent.DataSource = dtfilter;
                dgBillofComponent.DataBind();
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sub Component Master", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Masters/ADD/BOComponent.aspx?c_name=" + type, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component - View", "btnAddNew_Click", Ex.Message);
        }
    }
    protected void dgBillofComponent_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgBillofComponent.Rows[e.RowIndex].FindControl("lblBM_CODE"))).Text))
            {

                string bm_code = ((Label)(dgBillofComponent.Rows[e.RowIndex].FindControl("lblBM_CODE"))).Text;
                string i_name = ((Label)(dgBillofComponent.Rows[e.RowIndex].FindControl("lblI_NAME"))).Text;
                string i_code = ((Label)(dgBillofComponent.Rows[e.RowIndex].FindControl("lblI_CODENO"))).Text;

                if (CommonClasses.CheckUsedInTran("ENQUERY_MASTER", "EQ_P_CODE", "AND ES_DELETE=0", bm_code))
                {
                    ShowMessage("#Avisos", "You cant delete this record it has used in Components", CommonClasses.MSG_Warning);
                }
                else
                {
                    bool flag = CommonClasses.Execute1("UPDATE BOC_MASTER SET ES_DELETE = 1 WHERE BM_CODE='" + Convert.ToInt32(bm_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Bill Of Component", "Delete", "Bill Of Component", bm_code, Convert.ToInt32(bm_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);

                    }
                }
            }
            
            LoadBOC();
        }
        else
        {
            ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            return;
        }
    }
    protected void dgBillofComponent_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string bm_code = ((Label)(dgBillofComponent.Rows[e.NewEditIndex].FindControl("lblBM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/BOComponent.aspx?c_name=" + type + "&bm_code=" + bm_code, false);
            }
            else
            {
                ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component - View", "dgBillofComponent_RowEditing", Ex.Message);
        }
    }
    protected void dgBillofComponent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgBillofComponent.PageIndex = e.NewPageIndex;
        LoadBOC();

    }
    protected void dgBillofComponent_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string bm_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/BOComponent.aspx?c_name=" + type + "&bm_code=" + bm_code, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
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
                        Response.Redirect("~/Masters/ADD/BOComponent.aspx?c_name=" + type + "&bm_code=" + bm_code, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }

                else
                {
                    ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component - View", "GridView1_RowCommand", Ex.Message);
        }
    }
    protected void dgBillofComponent_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from BOC_MASTER where BM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Bill Of Component - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}