using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transaction_ViewVendorWeeklyPlan : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='239'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadData();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Weekly Plan", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT DISTINCT VWP_CODE, P_CODE,P_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME,C.VWP_DATE AS DOC_DATE, right(convert(varchar, C.VWP_DATE, 106), 8) as VWP_DATE ,isnull(VWP_VENDOR_STOCK,0) as VWP_VENDOR_STOCK,isnull(VWP_QTY,0) as VWP_QTY,case when VWP_WEEK='1' then 'Week1' when VWP_WEEK='2' then 'Week2' when VWP_WEEK='3' then 'Week3' when VWP_WEEK='4' then 'Week4'  else '0' end As  VWP_WEEK FROM VENDOR_WEEKLY_PLAN AS C INNER JOIN ITEM_MASTER AS I ON C.VWP_I_CODE = I.I_CODE  inner join PARTY_MASTER P ON C.VWP_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.VWP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND C.VWP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ORDER BY VWP_DATE DESC,VWP_WEEK DESC, P_NAME,I_CODENO +' - '+ I_NAME");
            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgCustomer.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_VENDOR_STOCK", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_WEEK", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomer.DataSource = dtFilter;
                    dgCustomer.DataBind();
                }
            }
            else
            {
                dgCustomer.Enabled = true;
                dgCustomer.DataSource = dt;
                dgCustomer.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Weekly Plan", "LoadUser", Ex.Message);
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
            CommonClasses.SendError("Vendor Weekly Plan", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");
            DataTable dtfilter = new DataTable();
            if (txtString.Text.Trim().Replace("'", "\''") != "")
                //dtfilter = CommonClasses.Execute("SELECT DISTINCT VWP_CODE, P_CODE,P_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME, right(convert(varchar, C.VWP_DATE, 106), 8) as VWP_DATE ,isnull(VWP_VENDOR_STOCK,0) as VWP_VENDOR_STOCK,isnull(VWP_QTY,0) as VWP_QTY,case when VWP_WEEK='1' then 'Week1' when VWP_WEEK='2' then 'Week2' when VWP_WEEK='3' then 'Week3' when VWP_WEEK='4' then 'Week4'  else '0' end As  VWP_WEEK FROM VENDOR_WEEKLY_PLAN AS C INNER JOIN ITEM_MASTER AS I ON C.VWP_I_CODE = I.I_CODE  inner join PARTY_MASTER P ON C.VWP_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.VWP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND (lower(I_NAME) like lower('%" + str + "%') or lower(I_CODENO) like lower('%" + str + "%') or lower(P_NAME) like lower('%" + str + "%') or lower(VWP_DATE) like lower('%" + str + "%') or lower(VWP_VENDOR_STOCK) like lower('%" + str + "%') or lower(VWP_VENDOR_STOCK) like lower('%" + str + "%'))ORDER BY P_NAME,I_CODENO +' - '+ I_NAME,VWP_DATE,VWP_WEEK");
                dtfilter = CommonClasses.Execute(" SELECT DISTINCT VWP_CODE, P_CODE,P_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME, C.VWP_DATE AS DOC_DATE,right(convert(varchar, C.VWP_DATE, 106), 8) as VWP_DATE ,isnull(VWP_VENDOR_STOCK,0) as VWP_VENDOR_STOCK,isnull(VWP_QTY,0) as VWP_QTY,case when VWP_WEEK='1' then 'Week1' when VWP_WEEK='2' then 'Week2' when VWP_WEEK='3' then 'Week3' when VWP_WEEK='4' then 'Week4'  else '0' end As  VWP_WEEK INTO #temp FROM VENDOR_WEEKLY_PLAN AS C INNER JOIN ITEM_MASTER AS I ON C.VWP_I_CODE = I.I_CODE  inner join PARTY_MASTER P ON C.VWP_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.VWP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND C.VWP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  SELECT * FROM #temp where  (lower(VWP_WEEK) like lower('%" + str + "%') or lower(ICODE_INAME) like lower('%" + str + "%') or lower(P_NAME) like lower('%" + str + "%') or datename(YYYY,VWP_DATE) like lower('%" + str + "%') or datename(MM,VWP_DATE) like lower('%" + str + "%') or lower(VWP_VENDOR_STOCK) like lower('%" + str + "%') or lower(VWP_VENDOR_STOCK) like lower('%" + str + "%')) ORDER BY DOC_DATE DESC,VWP_WEEK DESC,P_NAME,ICODE_INAME DROP TABLE #temp");
            else
                dtfilter = CommonClasses.Execute("SELECT DISTINCT VWP_CODE, P_CODE,P_NAME,I_CODENO +' - '+ I_NAME AS ICODE_INAME, right(convert(varchar, C.VWP_DATE, 106), 8) as VWP_DATE ,isnull(VWP_VENDOR_STOCK,0) as VWP_VENDOR_STOCK,isnull(VWP_QTY,0) as VWP_QTY,case when VWP_WEEK='1' then 'Week1' when VWP_WEEK='2' then 'Week2' when VWP_WEEK='3' then 'Week3' when VWP_WEEK='4' then 'Week4'  else '0' end As  VWP_WEEK FROM VENDOR_WEEKLY_PLAN AS C INNER JOIN ITEM_MASTER AS I ON C.VWP_I_CODE = I.I_CODE  inner join PARTY_MASTER P ON C.VWP_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.VWP_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' order by P_NAME,I_CODENO +' - '+ I_NAME,VWP_DATE,VWP_WEEK");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomer.Enabled = true;
                dgCustomer.DataSource = dtfilter;
                dgCustomer.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgCustomer.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ICODE_INAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_VENDOR_STOCK", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("VWP_WEEK", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomer.DataSource = dtFilter;
                    dgCustomer.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Weekly Plan - View", "LoadStatus", Ex.Message);
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
                Response.Redirect("~/PPC/ADD/VendorWeeklyPlan.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Vendor Weekly Plan", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region GridView1_PageIndexChanging
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomer.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Weekly Plan", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    #endregion GridView1_PageIndexChanging

    #region GridView1_RowCommand
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Response.Redirect("~/PPC/ADD/VendorWeeklyPlan.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
                        Response.Redirect("~/PPC/ADD/VendorWeeklyPlan.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            CommonClasses.SendError("Vendor Weekly Plan", "GridView1_RowCommand", exc.Message);
        }
    }
    #endregion GridView1_RowCommand

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from VENDOR_WEEKLY_PLAN where VWP_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Vendor Weekly Plan", "GridView1_RowEditing", exc.Message);
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
                if (!ModifyLog(((Label)(dgCustomer.Rows[e.RowIndex].FindControl("lblVWP_CODE"))).Text))
                {
                    string um_code = ((Label)(dgCustomer.Rows[e.RowIndex].FindControl("lblVWP_CODE"))).Text;
                    bool flag = CommonClasses.Execute1("UPDATE VENDOR_WEEKLY_PLAN SET ES_DELETE = 1 WHERE VWP_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Vendor Weekly Plan", "Delete", "Vendor Weekly Plan", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
            CommonClasses.SendError("Vendor Weekly Plan", "GridView1_RowEditing", exc.Message);
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

    #region GridView1_RowEditing
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgCustomer.Rows[e.NewEditIndex].FindControl("lblVWP_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/PPC/ADD/VendorWeeklyPlan.aspx?c_name=" + type + "&u_code=" + user_code, false);
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
            CommonClasses.SendError("Vendor Weekly Plan", "GridView1_RowEditing", exc.Message);
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
            CommonClasses.SendError("Vendor Weekly Plan", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
