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

public partial class Masters_VIEW_ViewStateMaster : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string fieldName;
    DataTable dtFilter = new DataTable();
    static string right = "";
   
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
                    //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='13'");
                    //right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadState();
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "Page_Load", Ex.Message);
        }
    }


    #region LoadState
    private void LoadState()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and STATE_MASTER.ES_DELETE=0");

            dgState.DataSource = dt;
            dgState.DataBind();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "LoadState", Ex.Message);
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

        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='0' and (upper(SM_NAME) like upper('%" + str + "%') OR upper(COUNTRY_NAME) like upper('%" + str + "%'))");
            else
                dtfilter = CommonClasses.Execute("SELECT SM_CODE,SM_NAME,COUNTRY_NAME FROM STATE_MASTER,COUNTRY_MASTER WHERE COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.COUNTRY_CODE and SM_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='0'");

            if (dtfilter.Rows.Count > 0)
            {

                dgState.DataSource = dtfilter;
                dgState.DataBind();
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Add"))
            //{
            string type = "INSERT"; 
            Response.Redirect("~/Masters/ADD/StateMaster.aspx?c_name=" + type, false);
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events

    protected void dgState_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (!ModifyLog(((Label)(dgState.Rows[e.RowIndex].FindControl("lblSM_CODE"))).Text))
            {

                string s_code = ((Label)(dgState.Rows[e.RowIndex].FindControl("lblSM_CODE"))).Text;
                string s_name = ((Label)(dgState.Rows[e.RowIndex].FindControl("lblSTATE_NAME"))).Text;
                
                //if (CommonClasses.CheckUsedInTran("ENQUERY_MASTER", "EQ_s_code", "AND ES_DELETE=0", s_code))
                //{
                //    ShowMessage("#Avisos", "You cant delete this record it has used in Components", CommonClasses.MSG_Warning);
                //}
                //else
                //{
                bool flag = CommonClasses.Execute1("UPDATE STATE_MASTER SET ES_DELETE = 1 WHERE SM_CODE='" + Convert.ToInt32(s_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("State Master", "Delete", "State Master", s_code, Convert.ToInt32(s_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);

                    }
                //}
                //}
                LoadState();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "dgState_RowDeleting", Ex.Message);
        }
    }
    protected void dgState_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string s_code = ((Label)(dgState.Rows[e.NewEditIndex].FindControl("lblSM_CODE"))).Text;
            string type = "MODIFY";
            Response.Redirect("~/Masters/ADD/StateMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "dgState_RowEditing", Ex.Message);
        }
    }
    protected void dgState_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void dgState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    //if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For View"))
                    //{
                    string type = "VIEW";
                    string s_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/StateMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    //}
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    //if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Modify"))
                    //{
                    string type = "MODIFY";
                    string s_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/StateMaster.aspx?c_name=" + type + "&s_code=" + s_code, false);
                    //}
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master", "dgState_RowCommand", Ex.Message);
        }
    }
    protected void dgState_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from STATE_MASTER where s_code=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("State Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("State Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

}
