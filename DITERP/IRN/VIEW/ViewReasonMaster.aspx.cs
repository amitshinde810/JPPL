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

public partial class IRN_VIEW_ViewReasonMaster : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='122'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadData();
                    if (dgReasonMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_RSM_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NAME", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_TYPE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("RM_DEFECT", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgReasonMaster.DataSource = dtFilter;
                            dgReasonMaster.DataBind();
                            dgReasonMaster.Enabled = false;
                        }
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reason Master - View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT TOP 500 REASON_MASTER.RM_CODE, REASON_MASTER.RM_RSM_CODE, REJECTIONSTAGE_MASTER.RSM_NO, REJECTIONSTAGE_MASTER.RSM_NAME,  CASE WHEN RM_TYPE=1 then 'CASTING' ELSE'MACHINING' END AS RM_TYPE, REASON_MASTER.RM_DEFECT FROM REASON_MASTER INNER JOIN REJECTIONSTAGE_MASTER ON REASON_MASTER.RM_RSM_CODE = REJECTIONSTAGE_MASTER.RSM_CODE  WHERE     (REASON_MASTER.ES_DELETE = 0)  AND RSM_CM_ID ='" + Session["CompanyId"].ToString() + "'");
            
            if (dt.Rows.Count > 0)
            {
                dgReasonMaster.DataSource = dt;
                dgReasonMaster.DataBind();

                dgReasonMaster.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_RSM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_TYPE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_DEFECT", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgReasonMaster.DataSource = dtFilter;
                    dgReasonMaster.DataBind();
                    dgReasonMaster.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reason Master-View", "LoadData", Ex.Message);
        }
    }
    #endregion

    #region dgReasonMaster_RowDeleting
    protected void dgReasonMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgReasonMaster.Rows[e.RowIndex].FindControl("lblRM_CODE"))).Text))
                {

                    string RM_CODE = ((Label)(dgReasonMaster.Rows[e.RowIndex].FindControl("lblRM_CODE"))).Text;
                    string RSM_NO = ((Label)(dgReasonMaster.Rows[e.RowIndex].FindControl("lblRSM_NO"))).Text;
                    string RSM_NAME = ((Label)(dgReasonMaster.Rows[e.RowIndex].FindControl("lblRSM_NAME"))).Text;

                    if (CommonClasses.CheckUsedInTran("IRN_ENTRY,IRN_DETAIL", "IRND_RM_CODE", "AND IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0", RM_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record. It is used in IRN Entry.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        // ShowMessage("#Avisos", "You cant delete this record it has used in Components", CommonClasses.MSG_Warning);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE REASON_MASTER SET ES_DELETE=1 where  RM_CODE='" + RM_CODE + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Reason Master", "Delete", "Reason Master", RSM_NO, Convert.ToInt32(RM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
                lblmsg.Text = "You have no rights to Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reason Master - View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgReasonMaster_RowEditing
    protected void dgReasonMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string RM_CODE = ((Label)(dgReasonMaster.Rows[e.NewEditIndex].FindControl("lblRM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/IRN/ADD/ReasonMaster.aspx?c_name=" + type + "&i_uom_code=" + RM_CODE, false);
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
            CommonClasses.SendError("Reason Master - View", "dgReasonMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgReasonMaster_PageIndexChanging
    protected void dgReasonMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgReasonMaster.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgReasonMaster_RowUpdating
    protected void dgReasonMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region dgReasonMaster_RowCommand
    protected void dgReasonMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {

                    string type = "VIEW";
                    string RM_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/IRN/ADD/ReasonMaster.aspx?c_name=" + type + "&i_uom_code=" + RM_CODE, false);

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
                        string RM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/IRN/ADD/ReasonMaster.aspx?c_name=" + type + "&i_uom_code=" + RM_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record is used by another User";
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
            CommonClasses.SendError("Reason Master - View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from REASON_MASTER where RM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Reason Master - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Reason Master - View", "ShowMessage", Ex.Message);
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
            CommonClasses.SendError("Reason Master - View", "btnClose_Click", ex.Message);
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
            CommonClasses.SendError("Reason Master - View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text.Trim() != "")
                dtfilter = CommonClasses.Execute("SELECT TOP 500 REASON_MASTER.RM_CODE, REASON_MASTER.RM_RSM_CODE, REJECTIONSTAGE_MASTER.RSM_NO, REJECTIONSTAGE_MASTER.RSM_NAME,   CASE WHEN RM_TYPE=1 then 'CASTING' ELSE'MACHINING' END AS RM_TYPE, REASON_MASTER.RM_DEFECT  FROM REASON_MASTER INNER JOIN REJECTIONSTAGE_MASTER ON REASON_MASTER.RM_RSM_CODE = REJECTIONSTAGE_MASTER.RSM_CODE WHERE     (REASON_MASTER.ES_DELETE = 0 )  AND  RSM_CM_ID='" + Session["CompanyId"] + "'   and (RSM_NO like upper('%" + str + "%') or RM_DEFECT like upper('%" + str + "%')  or RSM_NAME like upper('%" + str + "%')  OR  CASE WHEN RM_TYPE=1 then 'CASTING' ELSE'MACHINING' END like upper('%" + str + "%') ) order by RM_DEFECT");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 REASON_MASTER.RM_CODE, REASON_MASTER.RM_RSM_CODE, REJECTIONSTAGE_MASTER.RSM_NO, REJECTIONSTAGE_MASTER.RSM_NAME,   CASE WHEN RM_TYPE=1 then 'CASTING' ELSE'MACHINING' END AS RM_TYPE, REASON_MASTER.RM_DEFECT  FROM REASON_MASTER INNER JOIN REJECTIONSTAGE_MASTER ON REASON_MASTER.RM_RSM_CODE = REJECTIONSTAGE_MASTER.RSM_CODE WHERE     (REASON_MASTER.ES_DELETE = 0 )   AND  RSM_CM_ID='" + Session["CompanyId"] + "'   order by RM_DEFECT");

            if (dtfilter.Rows.Count > 0)
            {
                dgReasonMaster.DataSource = dtfilter;
                dgReasonMaster.DataBind();
                dgReasonMaster.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_RSM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_TYPE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RM_DEFECT", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgReasonMaster.DataSource = dtFilter;
                    dgReasonMaster.DataBind();
                    dgReasonMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reason Master - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/IRN/ADD/ReasonMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Reason Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
}
