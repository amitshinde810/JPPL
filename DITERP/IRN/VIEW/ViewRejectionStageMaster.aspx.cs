using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewRejectionStageMaster : System.Web.UI.Page
{
    #region " Var "
    UnitMaster_BL BL_UnitMaster = null;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='121'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadData();
                    if (dgStageMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NAME", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgStageMaster.DataSource = dtFilter;
                            dgStageMaster.DataBind();
                            dgStageMaster.Enabled = false;
                        }
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT * FROM REJECTIONSTAGE_MASTER where ES_DELETE=0 AND RSM_CM_ID ='" + Session["CompanyId"].ToString() + "'");
            if (dt.Rows.Count > 0)
            {
                dgStageMaster.DataSource = dt;
                dgStageMaster.DataBind();
              
                dgStageMaster.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NAME", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgStageMaster.DataSource = dtFilter;
                    dgStageMaster.DataBind();
                    dgStageMaster.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master-View", "LoadData", Ex.Message);
        }
    }
    #endregion

    #region dgStageMaster_RowDeleting
    protected void dgStageMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgStageMaster.Rows[e.RowIndex].FindControl("lblRSM_CODE"))).Text))
                {

                    string RSM_CODE = ((Label)(dgStageMaster.Rows[e.RowIndex].FindControl("lblRSM_CODE"))).Text;
                    string RSM_NO = ((Label)(dgStageMaster.Rows[e.RowIndex].FindControl("lblRSM_NO"))).Text;
                    string RSM_NAME = ((Label)(dgStageMaster.Rows[e.RowIndex].FindControl("lblRSM_NAME"))).Text;

                    if (CommonClasses.CheckUsedInTran("REASON_MASTER", "RM_RSM_CODE", "AND ES_DELETE=0", RSM_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record. It is used in Reason Master.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        // ShowMessage("#Avisos", "You cant delete this record it has used in Components", CommonClasses.MSG_Warning);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE REJECTIONSTAGE_MASTER SET ES_DELETE=1 where  RSM_CODE='" + RSM_CODE + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Rejection Stage Master", "Delete", "Rejection Stage Master", RSM_NAME, Convert.ToInt32(RSM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        }

                        LoadData();
                    }

                }

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgStageMaster_RowEditing
    protected void dgStageMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string RSM_CODE = ((Label)(dgStageMaster.Rows[e.NewEditIndex].FindControl("lblRSM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/IRN/ADD/RejectionStageMaster.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "dgStageMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgStageMaster_PageIndexChanging
    protected void dgStageMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgStageMaster.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgStageMaster_RowUpdating
    protected void dgStageMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region dgStageMaster_RowCommand
    protected void dgStageMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {

                    string type = "VIEW";
                    string RSM_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/IRN/ADD/RejectionStageMaster.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "MODIFY";
                        string RSM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/IRN/ADD/RejectionStageMaster.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record is used by another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from REJECTIONSTAGE_MASTER where RSM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Rejection Stage Master - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Rejection Stage Master - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT     RSM_CODE, RSM_NO, RSM_NAME, RSM_CM_ID FROM REJECTIONSTAGE_MASTER WHERE     (ES_DELETE = 0) AND  RSM_CM_ID='" + Session["CompanyId"] + "'   and (RSM_NO like upper('%" + str + "%') or RSM_NAME like upper('%" + str + "%')) order by RSM_NO");
            else
                dtfilter = CommonClasses.Execute("SELECT     RSM_CODE, RSM_NO, RSM_NAME, RSM_CM_ID FROM REJECTIONSTAGE_MASTER WHERE     (ES_DELETE = 0) AND  RSM_CM_ID='" + Session["CompanyId"] + "'   order by RSM_NO");

            if (dtfilter.Rows.Count > 0)
            {
                dgStageMaster.DataSource = dtfilter;
                dgStageMaster.DataBind();
                dgStageMaster.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NAME", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgStageMaster.DataSource = dtFilter;
                    dgStageMaster.DataBind();
                    dgStageMaster.Enabled = false;
                }

            }


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/IRN/ADD/RejectionStageMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Rejection Stage Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
