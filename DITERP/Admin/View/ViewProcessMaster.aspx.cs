using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admin_View_ViewProcessMaster : System.Web.UI.Page
{
    #region Variables
    ProcessMaster_BL BL_ProcessMaster = null;  
    static string right = "";  
    DataTable dtFilter = new DataTable();
    #endregion

    #region Evenets

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='88'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadStatus(txtString);                   
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master ", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/RNDDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region dgProcessMaster_RowDeleting
    protected void dgProcessMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgProcessMaster.Rows[e.RowIndex].FindControl("lblProcess_CODE"))).Text))
                {
                    BL_ProcessMaster = new ProcessMaster_BL();
                    string Process_CODE = ((Label)(dgProcessMaster.Rows[e.RowIndex].FindControl("lblProcess_CODE"))).Text;
                    string Process_NAME = ((Label)(dgProcessMaster.Rows[e.RowIndex].FindControl("lblProcess_NAME"))).Text;
                    BL_ProcessMaster.PROCESS_CODE = Convert.ToInt32(Process_CODE);
                    if (CommonClasses.CheckUsedInTran("INVOICE_MASTER,INVOICE_DETAIL", "IND_PROCESS_CODE", "AND INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutSUBINM' ", Process_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record.It has been used in Dispath To Sub Contactor";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_ProcessMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Process Master", "Delete", "Process Master", Process_NAME, Convert.ToInt32(Process_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                }
                LoadProcess();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "dgProcessMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgProcessMaster_RowEditing
    protected void dgProcessMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string Process_CODE = ((Label)(dgProcessMaster.Rows[e.NewEditIndex].FindControl("lblProcess_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/ProcessMaster.aspx?c_name=" + type + "&Process_CODE=" + Process_CODE, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "dgProcessMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgProcessMaster_PageIndexChanging
    protected void dgProcessMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgProcessMaster.PageIndex = e.NewPageIndex;
            LoadProcess();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgProcessMaster_RowUpdating
    protected void dgProcessMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgProcessMaster_RowCommand
    protected void dgProcessMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string Process_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/ProcessMaster.aspx?c_name=" + type + "&Process_CODE=" + Process_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
                        string Process_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/ProcessMaster.aspx?c_name=" + type + "&Process_CODE=" + Process_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }
               else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "dgProcessMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("~/Admin/Add/ProcessMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Process Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadProcess
    private void LoadProcess()
    {
        try
        {
            BL_ProcessMaster = new ProcessMaster_BL();
            BL_ProcessMaster.PROCESS_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_ProcessMaster.FillGrid(dgProcessMaster);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "LoadProcess", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {           
            DataTable dt = CommonClasses.Execute("select MODIFY from Process_MASTER where Process_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Process Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Process Master", "ShowMessage", Ex.Message);
            return false;
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
                dtfilter = CommonClasses.Execute("SELECT Process_CODE,Process_NAME FROM Process_MASTER WHERE Process_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (Process_NAME like upper('%" + str + "%')) order by Process_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT Process_CODE,Process_NAME FROM Process_MASTER WHERE Process_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by Process_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgProcessMaster.Enabled = true;
                dgProcessMaster.DataSource = dtfilter;
                dgProcessMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("Process_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Process_NAME", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProcessMaster.DataSource = dtFilter;
                    dgProcessMaster.DataBind();
                    dgProcessMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Process Master", "LoadStatus", ex.Message);
        }
    }
    #endregion
    #endregion
}
