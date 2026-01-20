using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewCityMaster : System.Web.UI.Page
{
    #region Variables
    CityMaster_BL BL_CityMaster = null;
    static bool CheckModifyLog = false;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='7'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadCity();
                    dgCity.Enabled = true;

                    if (dgCity.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("CITY_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CITY_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("SM_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCity.DataSource = dtFilter;
                            dgCity.DataBind();
                            dgCity.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Excise Tariff Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadCity
    private void LoadCity()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT CITY_CODE,CITY_NAME,SM_NAME,COUNTRY_NAME FROM CITY_MASTER,STATE_MASTER,COUNTRY_MASTER WHERE CITY_MASTER.CITY_COUNTRY_CODE=COUNTRY_MASTER.COUNTRY_CODE and CITY_MASTER.CITY_SM_CODE=STATE_MASTER.SM_CODE and COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and CITY_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and CITY_MASTER.ES_DELETE=0 order by CITY_NAME,SM_NAME,COUNTRY_NAME");

            dgCity.DataSource = dt;
            dgCity.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "LoadCity", Ex.Message);
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
            CommonClasses.SendError("City Master", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT CITY_CODE,CITY_NAME,SM_NAME,COUNTRY_NAME FROM CITY_MASTER,STATE_MASTER,COUNTRY_MASTER WHERE CITY_MASTER.CITY_COUNTRY_CODE=COUNTRY_MASTER.COUNTRY_CODE and CITY_MASTER.CITY_SM_CODE=STATE_MASTER.SM_CODE and COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and CITY_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and CITY_MASTER.ES_DELETE='0' and (CITY_NAME like upper('%" + str + "%') OR SM_NAME like upper('%" + str + "%') OR COUNTRY_NAME like upper('%" + str + "%'))order by CITY_NAME,SM_NAME,COUNTRY_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT CITY_CODE,CITY_NAME,SM_NAME,COUNTRY_NAME FROM CITY_MASTER,STATE_MASTER,COUNTRY_MASTER WHERE CITY_MASTER.CITY_COUNTRY_CODE=COUNTRY_MASTER.COUNTRY_CODE and CITY_MASTER.CITY_SM_CODE=STATE_MASTER.SM_CODE and COUNTRY_MASTER.COUNTRY_CODE=STATE_MASTER.SM_COUNTRY_CODE and CITY_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and CITY_MASTER.ES_DELETE='0' order by CITY_NAME,SM_NAME,COUNTRY_NAME  ");

            if (dtfilter.Rows.Count > 0)
            {
                dgCity.Enabled = true;
                dgCity.DataSource = dtfilter;
                dgCity.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CITY_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CITY_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCity.DataSource = dtFilter;
                    dgCity.DataBind();
                    dgCity.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Admin/Add/CityMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("City Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events

    protected void dgCity_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCity.Rows[e.RowIndex].FindControl("lblCityCode"))).Text))
                {
                    string c_code = ((Label)(dgCity.Rows[e.RowIndex].FindControl("lblCityCode"))).Text;
                    string c_name = ((Label)(dgCity.Rows[e.RowIndex].FindControl("lblCityName"))).Text;
                    bool flag = CommonClasses.Execute1("UPDATE CITY_MASTER SET ES_DELETE = 1 WHERE CITY_CODE='" + Convert.ToInt32(c_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("City Master", "Delete", "City Master", c_code, Convert.ToInt32(c_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadCity();
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master", "dgCity_RowDeleting", Ex.Message);
        }
    }
    protected void dgCity_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string c_code = ((Label)(dgCity.Rows[e.NewEditIndex].FindControl("lblCityCode"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/CityMaster.aspx?c_name=" + type + "&c_code=" + c_code, false);
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
            CommonClasses.SendError("City Master", "dgCity_RowEditing", Ex.Message);
        }
    }
    protected void dgCity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCity.PageIndex = e.NewPageIndex;
            LoadCity();
        }
        catch (Exception)
        {
        }
    }
    protected void dgCity_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string c_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/CityMaster.aspx?c_name=" + type + "&c_code=" + c_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another user";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
                        string c_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/CityMaster.aspx?c_name=" + type + "&c_code=" + c_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another user";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
            CommonClasses.SendError("City Master", "dgCity_RowCommand", Ex.Message);
        }
    }
    protected void dgCity_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from CITY_MASTER where CITY_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("City Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("City Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

}
