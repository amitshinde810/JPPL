using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Masters_VIEW_ViewProjectCodeMaster : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    #region PageLoad
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='112'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                    {
                        LoadProCode();
                        dgProCode.Enabled = true;
                        if (dgProCode.Rows.Count == 0)
                        {
                            dtFilter.Clear();
                            if (dtFilter.Columns.Count == 0)
                            {
                                dtFilter.Columns.Add(new System.Data.DataColumn("PROCM_CODE", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("PROCM_NAME", typeof(string)));

                                dtFilter.Rows.Add(dtFilter.NewRow());
                                dgProCode.DataSource = dtFilter;
                                dgProCode.DataBind();
                                dgProCode.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Project Code Master", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadTally
    private void LoadProCode()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT  * FROM PROJECT_CODE_MASTER WHERE PROCM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 order by PROCM_CODE DESC ");

            dgProCode.DataSource = dt;
            dgProCode.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Project Code Master", "LoadUser", Ex.Message);
        }
    }
    #endregion

    #region btnInsert
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Masters/ADD/AddProjectCodeMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Project Code Master", "btnInsert_Click", exc.Message);
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
            CommonClasses.SendError("Project Code Master -View", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT   * FROM PROJECT_CODE_MASTER WHERE ES_DELETE='FALSE' AND PROCM_COMP_ID = '" + Session["CompanyId"] + "' and (lower(PROCM_NAME) like lower('%" + str + "%')  OR lower(PROCM_AMT) like lower('%" + str + "%'))order by PROCM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT   * FROM PROJECT_CODE_MASTER WHERE ES_DELETE='FALSE' AND PROCM_COMP_ID = '" + Session["CompanyId"] + "' order by PROCM_CODE DESC ");

            if (dtfilter.Rows.Count > 0)
            {
                dgProCode.DataSource = dtfilter;
                dgProCode.DataBind();
                dgProCode.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCM_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCM_AMT", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCM_APPROVE", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProCode.DataSource = dtFilter;
                    dgProCode.DataBind();
                    dgProCode.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Project Code Master -View", "LoadStatus", Ex.Message);
        }
    }
    #endregion

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgProCode.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Project Code Master", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string um_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/AddProjectCodeMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
                    { string um_code = e.CommandArgument.ToString();

                    DataTable dtisapprove = CommonClasses.Execute("select * from PROJECT_CODE_MASTER where PROCM_CODE='" + um_code + "' AND PROCM_APPROVE=1");
                    if (dtisapprove.Rows.Count>0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Approved You can not Modify, use ammend ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                        string type = "MODIFY";
                       
                        Response.Redirect("~/Masters/ADD/AddProjectCodeMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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

            if (e.CommandName.Equals("Amend"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string qe_code = ((Label)dgProCode.Rows[index].FindControl("lblPROCM_CODE")).Text;
                string Party_Status = ((Label)dgProCode.Rows[index].FindControl("lblPROCM_APPROVE")).Text;
                if (Party_Status == "0" || Party_Status.ToUpper() == "FALSE")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Project Code not Authorize,Please Authorize Party First";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                string um_code = qe_code;

                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(qe_code))
                    {

                        string type = "Amend"; 
                        Response.Redirect("~/Masters/ADD/AddProjectCodeMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            if (e.CommandName.Equals("Authorize"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string qe_code = ((Label)dgProCode.Rows[index].FindControl("lblPROCM_CODE")).Text;
                string Party_Status = ((Label)dgProCode.Rows[index].FindControl("lblPROCM_APPROVE")).Text;
                if (Party_Status == "1" || Party_Status.ToUpper() == "TRUE")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Project Code is Authorize";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
               string um_code = qe_code;


               if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Authorize"))
                {
                    if (!ModifyLog(qe_code))
                    {
                        string type = "Authorize"; 
                        Response.Redirect("~/Masters/ADD/AddProjectCodeMaster.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Project Code Master", "GridView1_RowCommand", exc.Message);
        }
    }
    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from PROJECT_CODE_MASTER where PROCM_CODE=" + PrimaryKey + "  ");
            if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another User";
                return true;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Project Code Master", "GridView1_RowEditing", exc.Message);
        }

        return false;
    }
    #endregion

    #region GridView1_RowDeleting
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgProCode.Rows[e.RowIndex].FindControl("lblPROCM_CODE"))).Text))
                {
                    string um_code = ((Label)(dgProCode.Rows[e.RowIndex].FindControl("lblPROCM_CODE"))).Text;
                    string um_name = ((Label)(dgProCode.Rows[e.RowIndex].FindControl("lblPROCM_NAME"))).Text;
                    if (CommonClasses.CheckUsedInTran("SUPP_PO_MASTER", "SPOM_PROJECT", "AND ES_DELETE=0", um_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Purchase Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER", "CPOM_PROJECT_CODE", "AND ES_DELETE=0", um_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Sales Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE PROJECT_CODE_MASTER SET ES_DELETE = 1 WHERE PROCM_CODE='" + Convert.ToInt32(um_code) + "'");

                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Project Code Master", "Delete", "Project Code Master", um_name, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        }
                        LoadProCode();
                    }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Project Code Master", "GridView1_RowEditing", exc.Message);
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
            CommonClasses.SendError("Project Code Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region GridView1_RowEditing
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgProCode.Rows[e.NewEditIndex].FindControl("lblPROCM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/AddProjectCodeMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Project Code Master", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Project Code Master -View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
