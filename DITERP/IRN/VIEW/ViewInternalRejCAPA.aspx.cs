using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_VIEW_ViewInternalRejCAPA : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='257'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadData();
                    if (dgIRNF.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_DATE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_STATUS", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgIRNF.DataSource = dtFilter;
                            dgIRNF.DataBind();
                            dgIRNF.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT IRCM_CODE,LEFT(DATENAME(MONTH, IRCM_DATE), 3) + ' ' + DATENAME(YEAR,IRCM_DATE) as IRCM_DATE,CASE WHEN IRCM_STATUS=1 then '1' Else '0' END  AS IRCM_STATUS FROM INTERNAL_REJECTION_CAPA_MASTER WHERE ES_DELETE=0 AND IRCM_CM_COMP_CODE='" + Session["CompanyCode"].ToString() + "' ORDER BY IRCM_CODE DESC");
            if (dt.Rows.Count > 0)
            {
                dgIRNF.DataSource = dt;
                dgIRNF.DataBind();
                dgIRNF.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_STATUS", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgIRNF.DataSource = dtFilter;
                    dgIRNF.DataBind();
                    dgIRNF.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production Entry-View", "LoadData", Ex.Message);
        }
    }
    #endregion

    #region dgIRNF_RowDeleting
    protected void dgIRNF_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgIRNF.Rows[e.RowIndex].FindControl("lblIRCM_CODE"))).Text))
                {
                    string IRCM_CODE = ((Label)(dgIRNF.Rows[e.RowIndex].FindControl("lblIRCM_CODE"))).Text;
                    
                    DataTable dt = new DataTable();
                    dt = CommonClasses.Execute("select IRCM_DATE,IRCM_STATUS from INTERNAL_REJECTION_CAPA_MASTER where IRCM_CODE='" + IRCM_CODE + "' and ES_DELETE=0 and IRCM_STATUS=1");
                    if (dt.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can Not Delete Approved Record";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    DataTable dtDetail = CommonClasses.Execute("SELECT * FROM INTERNAL_REJECTION_CAPA_DETAIL where IRCD_IRCM_CODE='" + IRCM_CODE + "'");
                    if (dtDetail.Rows.Count > 0)
                    {
                    }
                    bool flag = CommonClasses.Execute1("UPDATE INTERNAL_REJECTION_CAPA_MASTER SET ES_DELETE=1 where IRCM_CODE='" + IRCM_CODE + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Iternal Rejection CAPA", "Delete", "Iternal Rejection CAPA", IRCM_CODE, Convert.ToInt32(IRCM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadData();
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
            CommonClasses.SendError("Iternal Rejection CAPA", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgIRNF_RowEditing
    protected void dgIRNF_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string RSM_CODE = ((Label)(dgIRNF.Rows[e.NewEditIndex].FindControl("lblRSM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/IRN/ADD/InternalRejCAPA.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "dgIRNF_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgIRNF_PageIndexChanging
    protected void dgIRNF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgIRNF.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgIRNF_RowUpdating
    protected void dgIRNF_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgIRNF_RowCommand
    protected void dgIRNF_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string RSM_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/IRN/ADD/InternalRejCAPA.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string um_code = e.CommandArgument.ToString();

                        DataTable dt = new DataTable();
                        dt = CommonClasses.Execute("select IRCM_DATE,IRCM_STATUS from INTERNAL_REJECTION_CAPA_MASTER where IRCM_CODE='" + um_code + "' and ES_DELETE=0 and IRCM_STATUS=1");
                        if (dt.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can Not Modify Approved Record";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }

                        string type = "MODIFY";
                        string RSM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/IRN/ADD/InternalRejCAPA.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #region UpdateStatus
            if (e.CommandName.Equals("Status"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For UpdateStatus"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string um_code = e.CommandArgument.ToString();
                        int Index = Convert.ToInt32(e.CommandArgument.ToString());
                        GridViewRow row = dgIRNF.Rows[Index];
                        string mt_Code = ((Label)(row.FindControl("lblIRCM_CODE"))).Text;
                        string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;

                        if (Status == "Approved")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Have No Rights To Change Status";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        CommonClasses.Execute1("UPDATE INTERNAL_REJECTION_CAPA_MASTER SET IRCM_STATUS=1 WHERE IRCM_CODE='" + mt_Code + "' AND ES_DELETE=0");
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadData();
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            #endregion Modify
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from INTERNAL_REJECTION_CAPA_MASTER where IRCM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "btnClose_Click", ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "btnSearch_Click", ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT IRCM_CODE,LEFT(DATENAME(MONTH, IRCM_DATE), 3) + ' ' + DATENAME(YEAR,IRCM_DATE) as IRCM_DATE,CASE WHEN IRCM_STATUS=1 then '1' Else '0' END  AS IRCM_STATUS FROM INTERNAL_REJECTION_CAPA_MASTER WHERE ES_DELETE=0 AND IRCM_CM_COMP_CODE='" + Session["CompanyCode"].ToString() + "' and (convert(varchar,IRCM_DATE,106) like upper('%" + str + "%')) ORDER BY IRCM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT IRCM_CODE,LEFT(DATENAME(MONTH, IRCM_DATE), 3) + ' ' + DATENAME(YEAR,IRCM_DATE) as IRCM_DATE,CASE WHEN IRCM_STATUS=1 then '1' Else '0' END  AS IRCM_STATUS FROM INTERNAL_REJECTION_CAPA_MASTER WHERE ES_DELETE=0 AND IRCM_CM_COMP_CODE='" + Session["CompanyCode"].ToString() + "' ORDER BY IRCM_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgIRNF.DataSource = dtfilter;
                dgIRNF.DataBind();
                dgIRNF.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IRCM_STATUS", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgIRNF.DataSource = dtFilter;
                    dgIRNF.DataBind();
                    dgIRNF.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "LoadStatus", ex.Message);
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
                Response.Redirect("~/IRN/ADD/InternalRejCAPA.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
