using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Masters_VIEW_AreaMaster : System.Web.UI.Page
{
    #region " Var "
    AreaMaster_BL BL_AreaMaster = null;
    static string right = "";
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='13'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadArea();
                    dtFilter.Clear();
                    if (dgAreaMaster.Rows.Count == 0)
                    {
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("A_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("A_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("A_DESC", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgAreaMaster.DataSource = dtFilter;
                            dgAreaMaster.DataBind();
                            dgAreaMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Master-View", "Page_Load", Ex.Message);
        }
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Master-View", "btnCancel_Click", Ex.Message);
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
                Response.Redirect("~/Masters/ADD/AddAreaMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Area Master-View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadArea
    private void LoadArea()
    {
        try
        {
            BL_AreaMaster = new AreaMaster_BL();
            BL_AreaMaster.A_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_AreaMaster.FillGrid(dgAreaMaster);
            if (dgAreaMaster.Rows.Count > 0)
            {
                dgAreaMaster.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Master-View", "LoadArea", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from AREA_MASTER where A_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Area Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Area Master-View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgItemCategory_RowCommand

    protected void dgAreaMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {



        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {

                    string type = "VIEW";
                    string A_Code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/ADD/AddAreaMaster.aspx?c_name=" + type + "&A_Code=" + A_Code, false);

                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "MODIFY";
                        string A_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/AddAreaMaster.aspx?c_name=" + type + "&A_Code=" + A_Code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You have no rights to Modify";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Master", "dgAreaMaster_RowCommand", Ex.Message);
        }

    }
    #endregion

    protected void dgAreaMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string A_Code = ((Label)(dgAreaMaster.Rows[e.NewEditIndex].FindControl("lblA_CODE"))).Text;
            string type = "MODIFY";
            Response.Redirect("~/Masters/ADD/AddAreaMaster.aspx?c_name=" + type + "&A_Code=" + A_Code, false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Master-View", "dgAreaMaster_RowEditing", Ex.Message);
        }
    }
    protected void dgAreaMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgAreaMaster.Rows[e.RowIndex].FindControl("lblA_Code"))).Text))
                {
                    BL_AreaMaster = new AreaMaster_BL();
                    string Area_Code = ((Label)(dgAreaMaster.Rows[e.RowIndex].FindControl("lblA_Code"))).Text;
                    string Area_No = ((Label)(dgAreaMaster.Rows[e.RowIndex].FindControl("lblA_NO"))).Text;
                    string Area_Desc = ((Label)(dgAreaMaster.Rows[e.RowIndex].FindControl("lblA_DESC"))).Text;

                    BL_AreaMaster.A_CODE = Convert.ToInt32(Area_Code);
                    if (CommonClasses.CheckUsedInTran("PARTY_MASTER", "P_A_CODE", "AND ES_DELETE=0", Area_Code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Party Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_AreaMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Area Master", "Delete", "Area Master", Area_Code, Convert.ToInt32(Area_Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadArea();
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
            CommonClasses.SendError("Area Master-View", "dgAreaMaster_RowDeleting", Ex.Message);
        }
    }

    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Area Master-View", "btnSearch_Click", ex.Message);
        }
    }
    protected void dgAreaMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgAreaMaster.PageIndex = e.NewPageIndex;
        LoadArea();
    }

    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text;

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")

                dtfilter = CommonClasses.Execute("SELECT A_CODE,A_NO,A_DESC  FROM AREA_MASTER WHERE A_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (A_NO like ('%" + str + "%')) OR (A_DESC like ('%" + str + "%')) order by A_DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT A_CODE,A_NO,A_DESC  FROM AREA_MASTER WHERE A_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by A_DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgAreaMaster.DataSource = dtfilter;
                dgAreaMaster.DataBind();
                dgAreaMaster.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("A_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("A_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("A_DESC", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgAreaMaster.DataSource = dtFilter;
                    dgAreaMaster.DataBind();
                    dgAreaMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Area Master-View", "LoadStatus", ex.Message);
        }
    }
}






