using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transaction_VIEW_ViewVendorSchedule : System.Web.UI.Page
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='205'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadData();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT DISTINCT   V.VS_DATE AS ddd,VS_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME,right(convert(varchar, VS_DATE, 106), 8) as VS_DATE,P_NAME, I.I_CODE, ISNULL(V.VS_CASTING_OFFLOADED, 0) AS VS_CASTING_OFFLOADED, ISNULL(V.VS_WEEK1, 0) AS VS_WEEK1,  ISNULL(V.VS_WEEK2, 0) AS VS_WEEK2, ISNULL(V.VS_WEEK3, 0) AS VS_WEEK3, ISNULL(V.VS_WEEK4, 0) AS VS_WEEK4,P_CODE FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE INNER JOIN PARTY_MASTER P ON V.VS_P_CODE=P.P_CODE WHERE (V.ES_DELETE = 0) and (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + (string)Session["CompanyId"] + "' AND V.VS_CM_CODE='" + (string)Session["CompanyCode"] + "' ORDER BY   V.VS_DATE DESC ,P_NAME,I_CODENO +' - '+ I_NAME ");
            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgCustomerSchedule.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("VS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VS_DATE", typeof(String))); 
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerSchedule.DataSource = dtFilter;
                    dgCustomerSchedule.DataBind();
                }
            }
            else
            {
                dgCustomerSchedule.Enabled = true;
                dgCustomerSchedule.DataSource = dt;
                dgCustomerSchedule.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule", "LoadUser", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule- View", "txtString_TextChanged", Ex.Message);
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
            if (txtString.Text.Trim() != "")
                dtfilter = CommonClasses.Execute("SELECT DISTINCT  V.VS_DATE AS ddd,VS_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME,right(convert(varchar, VS_DATE, 106), 8) as VS_DATE,P_NAME, I.I_CODE, ISNULL(V.VS_CASTING_OFFLOADED, 0) AS VS_CASTING_OFFLOADED, ISNULL(V.VS_WEEK1, 0) AS VS_WEEK1,  ISNULL(V.VS_WEEK2, 0) AS VS_WEEK2, ISNULL(V.VS_WEEK3, 0) AS VS_WEEK3, ISNULL(V.VS_WEEK4, 0) AS VS_WEEK4,P_CODE FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE INNER JOIN PARTY_MASTER P ON V.VS_P_CODE=P.P_CODE WHERE (V.ES_DELETE = 0) and (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + (string)Session["CompanyId"] + "'  AND V.VS_CM_CODE='" + (string)Session["CompanyCode"] + "' and (lower(I_CODENO) like lower('%" + str + "%') or lower(I_NAME) like lower('%" + str + "%') or  lower(P_NAME) like lower('%" + str + "%') or   DATENAME(MM,VS_DATE) like lower('%" + str + "%') or   DATENAME(YYYY,VS_DATE) like lower('%" + str + "%')) order by  V.VS_DATE DESC ,P_NAME,I_CODENO +' - '+ I_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT DISTINCT  V.VS_DATE AS ddd,VS_CODE,I.I_CODENO + ' - ' + I.I_NAME AS ICODE_INAME,right(convert(varchar, VS_DATE, 106), 8) as VS_DATE,P_NAME, I.I_CODE, ISNULL(V.VS_CASTING_OFFLOADED, 0) AS VS_CASTING_OFFLOADED, ISNULL(V.VS_WEEK1, 0) AS VS_WEEK1,  ISNULL(V.VS_WEEK2, 0) AS VS_WEEK2, ISNULL(V.VS_WEEK3, 0) AS VS_WEEK3, ISNULL(V.VS_WEEK4, 0) AS VS_WEEK4,P_CODE FROM VENDOR_SCHEDULE AS V INNER JOIN ITEM_MASTER AS I ON V.VS_I_CODE = I.I_CODE INNER JOIN PARTY_MASTER P ON V.VS_P_CODE=P.P_CODE WHERE (V.ES_DELETE = 0) and (P.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND V.VS_COMP_ID='" + (string)Session["CompanyId"] + "'  AND V.VS_CM_CODE='" + (string)Session["CompanyCode"] + "' order by  V.VS_DATE DESC ,P_NAME,I_CODENO +' - '+ I_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerSchedule.Enabled = true;
                dgCustomerSchedule.DataSource = dtfilter;
                dgCustomerSchedule.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgCustomerSchedule.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("VS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VS_DATE", typeof(String))); 
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerSchedule.DataSource = dtFilter;
                    dgCustomerSchedule.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule - View", "LoadStatus", Ex.Message);
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
                Response.Redirect("~/PPC/ADD/VendorSchedule.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Vendor Schedule", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region dgCustomerSchedule_PageIndexChanging
    protected void dgCustomerSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerSchedule.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule", "dgCustomerSchedule_PageIndexChanging", Ex.Message);
        }
    }
    #endregion dgCustomerSchedule_PageIndexChanging

    #region dgCustomerSchedule_RowCommand
    protected void dgCustomerSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                return;
            }
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string um_code = e.CommandArgument.ToString();
                    Response.Redirect("~/PPC/ADD/VendorSchedule.aspx?c_name=" + type + "&u_code=" + um_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion View

            #region Modify
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/PPC/ADD/VendorSchedule.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            #endregion Modify
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Vendor Schedule", "dgCustomerSchedule_RowCommand", exc.Message);
        }
    }
    #endregion dgCustomerSchedule_RowCommand

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from VENDOR_SCHEDULE where VS_CODE=" + PrimaryKey + "  ");
            if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another User";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return true;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Vendor Schedule", "dgCustomerSchedule_RowEditing", exc.Message);
        }
        return false;
    }
    #endregion

    #region dgCustomerSchedule_RowDeleting
    protected void dgCustomerSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblVS_CODE"))).Text))
                {
                    string um_code = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblVS_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE VENDOR_SCHEDULE SET ES_DELETE = 1 WHERE VS_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Vendor Schedule", "Delete", "Vendor Schedule", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadData();
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record deleted Successfully...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
            CommonClasses.SendError("Vendor Schedule", "dgCustomerSchedule_RowEditing", exc.Message);
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgCustomerSchedule_RowEditing
    protected void dgCustomerSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgCustomerSchedule.Rows[e.NewEditIndex].FindControl("lblVS_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/PPC/ADD/VendorSchedule.aspx?c_name=" + type + "&u_code=" + user_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Vendor Schedule", "dgCustomerSchedule_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PPCDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
