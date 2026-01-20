using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_View_ViewCountryMaster : System.Web.UI.Page
{
    #region Variables
    CountryMaster_BL BL_CountryMaster = null;
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Evenets
    #region Page_Load
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='5'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    LoadCountry();
                    dgCountryMaster.Enabled = true;
                    if (dgCountryMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCountryMaster.DataSource = dtFilter;
                            dgCountryMaster.DataBind();
                            dgCountryMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Country Master ", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region dgCountryMaster_RowDeleting
    protected void dgCountryMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCountryMaster.Rows[e.RowIndex].FindControl("lblCOUNTRY_CODE"))).Text))
                {
                    BL_CountryMaster = new CountryMaster_BL();
                    string COUNTRY_CODE = ((Label)(dgCountryMaster.Rows[e.RowIndex].FindControl("lblCOUNTRY_CODE"))).Text;
                    string COUNTRY_NAME = ((Label)(dgCountryMaster.Rows[e.RowIndex].FindControl("lblCOUNTRY_NAME"))).Text;
                    BL_CountryMaster.COUNTRY_CODE = Convert.ToInt32(COUNTRY_CODE);
                    if (CommonClasses.CheckUsedInTran("STATE_MASTER", "SM_COUNTRY_CODE", "AND ES_DELETE=0", COUNTRY_CODE))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in State Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    else
                    {
                        bool flag = BL_CountryMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Country Master", "Delete", "Country Master", COUNTRY_NAME, Convert.ToInt32(COUNTRY_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                }
                LoadCountry();
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
            CommonClasses.SendError("Country Master", "dgCountryMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgCountryMaster_RowEditing
    protected void dgCountryMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string COUNTRY_CODE = ((Label)(dgCountryMaster.Rows[e.NewEditIndex].FindControl("lblCOUNTRY_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/CountryMaster.aspx?c_name=" + type + "&COUNTRY_CODE=" + COUNTRY_CODE, false);
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
            CommonClasses.SendError("Country Master", "dgCountryMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgCountryMaster_PageIndexChanging
    protected void dgCountryMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCountryMaster.PageIndex = e.NewPageIndex;
            LoadCountry();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgCountryMaster_RowUpdating
    protected void dgCountryMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgCountryMaster_RowCommand
    protected void dgCountryMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string COUNTRY_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/CountryMaster.aspx?c_name=" + type + "&COUNTRY_CODE=" + COUNTRY_CODE, false);
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
                        string COUNTRY_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/CountryMaster.aspx?c_name=" + type + "&COUNTRY_CODE=" + COUNTRY_CODE, false);
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
            CommonClasses.SendError("Country Master", "dgCountryMaster_RowCommand", Ex.Message);
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
            CommonClasses.SendError("Country Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("~/Admin/Add/CountryMaster.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Country Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion
    #endregion

    #region Methods
    #region LoadCountry
    private void LoadCountry()
    {
        try
        {
            BL_CountryMaster = new CountryMaster_BL();
            BL_CountryMaster.COUNTRY_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_CountryMaster.FillGrid(dgCountryMaster);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Country Master", "LoadCountry", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from COUNTRY_MASTER where COUNTRY_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Country Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Country Master", "ShowMessage", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT COUNTRY_CODE,COUNTRY_NAME FROM COUNTRY_MASTER WHERE COUNTRY_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (COUNTRY_NAME like upper('%" + str + "%')) order by COUNTRY_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT COUNTRY_CODE,COUNTRY_NAME FROM COUNTRY_MASTER WHERE COUNTRY_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by COUNTRY_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgCountryMaster.Enabled = true;
                dgCountryMaster.DataSource = dtfilter;
                dgCountryMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCountryMaster.DataSource = dtFilter;
                    dgCountryMaster.DataBind();
                    dgCountryMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Country Master", "LoadStatus", ex.Message);
        }
    }
    #endregion
    #endregion
}
