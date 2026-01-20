using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;


public partial class ToolRoom_VIEW_ViewTools : System.Web.UI.Page
{
    #region Variable Declaration
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Tools");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("ToolsMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='167'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    ViewState["cnt"] = "0";
                    LoadTools();

                    if (dgToolMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));

                            dtFilter.Columns.Add(new System.Data.DataColumn("T_PHOTO_PATH", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_3D_PATH", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgToolMaster.DataSource = dtFilter;
                            dgToolMaster.DataBind();
                            dgToolMaster.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master(View)", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadTools
    private void LoadTools()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = "";

            if (ddlTType.SelectedIndex != 0)
            {
                str = str + "T_TYPE='" + ddlTType.SelectedValue + "' AND";
            }

            // dt = CommonClasses.Execute("SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME, PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,COUNT(T_3D_PATH) as DCount,COUNT(T_PHOTO_PATH) as ToolCount FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and TOOL_MASTER.ES_DELETE=0 group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC");
            //dt = CommonClasses.Execute("select case when T_3D_PATH='' then 0 else 1 end as T_3D_PATH into #temp from TOOL_MASTER where (T_3D_PATH<>'' or T_PHOTO_PATH<>'') or (T_3D_PATH is not null or T_PHOTO_PATH is not null) group by T_3D_PATH select case when T_PHOTO_PATH='' then 0 else 1 end as T_PHOTO_PATH into #temp1 from TOOL_MASTER where (T_3D_PATH<>'' or T_PHOTO_PATH<>'') or (T_3D_PATH is not null or T_PHOTO_PATH is not null) group by T_PHOTO_PATH  SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME,PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,(select SUM(T_3D_PATH) as T_3D_PATH from #temp) as DCount,  (select SUM(T_PHOTO_PATH) as T_PHOTO_PATH from #temp1) as ToolCount  FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE  P_CM_COMP_ID = '1' and TOOL_MASTER.ES_DELETE=0 group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC drop table #temp drop table #temp1");

            dt = CommonClasses.Execute("SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME,PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,  SUM(case when T_3D_PATH='' then 0 else 1 END) as  DCount, SUM(case when T_PHOTO_PATH='' then 0 else 1 end)   as ToolCount  FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE  P_CM_COMP_ID = '1' and TOOL_MASTER.ES_DELETE=0 group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC  ");

            dgToolMaster.DataSource = dt;
            dgToolMaster.DataBind();
            if (dgToolMaster.Rows.Count > 0)
            {

                dgToolMaster.Enabled = true;
                ViewState["cnt"] = dt.Rows.Count.ToString();
                long sum = (long)dt.Compute("Sum(ToolCount)", "True");
                long sum1 = (long)dt.Compute("Sum(DCount)", "True");
                dgToolMaster.HeaderRow.Cells[9].Text = "Tool Image (" + sum + "/" + ViewState["cnt"].ToString() + ")";
                dgToolMaster.HeaderRow.Cells[10].Text = "3D Image (" + sum + "/" + ViewState["cnt"].ToString() + ")";
            }
            else
            {
                if (dgToolMaster.Rows.Count == 0)
                {
                    ViewState["cnt"] = "0";
                    dtFilter.Clear();
                    // dgToolMaster.HeaderRow.Cells[9].Text = "Tool Image  ";
                    //dgToolMaster.HeaderRow.Cells[10].Text = "3D Image ";
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));

                        dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("T_PHOTO_PATH", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("T_3D_PATH", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("DCount", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("ToolCount", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgToolMaster.DataSource = dtFilter;
                        dgToolMaster.DataBind();
                        dgToolMaster.Enabled = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "LoadTools", Ex.Message);
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
            CommonClasses.SendError("Tool Master", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtString_TextChanged
    protected void ddlTType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadTools();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "txtString_TextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "''");

            DataTable dtfilter = new DataTable();

            if (txtString.Text.Trim() != "")
                // dtfilter = CommonClasses.Execute("SELECT TOOL_MASTER.T_CODE, CASE WHEN T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME, PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,COUNT(T_3D_PATH) as DCount,COUNT(T_PHOTO_PATH) as ToolCount FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND TOOL_MASTER.ES_DELETE='0' and (T_NAME like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR T_TYPE like upper('%" + str + "%')OR I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH order by T_NAME");
                //dtfilter = CommonClasses.Execute("select case when T_3D_PATH='' then 0 else 1 end as T_3D_PATH into #temp from TOOL_MASTER where (T_3D_PATH<>'' or T_PHOTO_PATH<>'') or (T_3D_PATH is not null or T_PHOTO_PATH is not null) group by T_3D_PATH select case when T_PHOTO_PATH='' then 0 else 1 end as T_PHOTO_PATH into #temp1 from TOOL_MASTER where (T_3D_PATH<>'' or T_PHOTO_PATH<>'') or (T_3D_PATH is not null or T_PHOTO_PATH is not null) group by T_PHOTO_PATH  SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME,PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,(select SUM(T_3D_PATH) as T_3D_PATH from #temp) as DCount,(select SUM(T_PHOTO_PATH) as T_PHOTO_PATH from #temp1) as ToolCount FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE P_CM_COMP_ID = '1' and TOOL_MASTER.ES_DELETE=0 and (T_NAME like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR T_TYPE like upper('%" + str + "%')OR I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC drop table #temp drop table #temp1");
                dtfilter = CommonClasses.Execute(" SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME,PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,  SUM(case when T_3D_PATH='' then 0 else 1 END) as  DCount, SUM(case when T_PHOTO_PATH='' then 0 else 1 end)   as ToolCount FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE P_CM_COMP_ID = '1' and TOOL_MASTER.ES_DELETE=0 and (T_NAME like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR T_TYPE like upper('%" + str + "%')OR I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC  ");
            else
                // dtfilter = CommonClasses.Execute("SELECT TOOL_MASTER.T_CODE, CASE WHEN T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME, PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,COUNT(T_3D_PATH) as DCount,COUNT(T_PHOTO_PATH) as ToolCount FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND TOOL_MASTER.ES_DELETE='0' group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH order by T_NAME");
                //dtfilter = CommonClasses.Execute("select case when T_3D_PATH='' then 0 else 1 end as T_3D_PATH into #temp from TOOL_MASTER where (T_3D_PATH<>'' or T_PHOTO_PATH<>'') or (T_3D_PATH is not null or T_PHOTO_PATH is not null) group by T_3D_PATH select case when T_PHOTO_PATH='' then 0 else 1 end as T_PHOTO_PATH into #temp1 from TOOL_MASTER where (T_3D_PATH<>'' or T_PHOTO_PATH<>'') or (T_3D_PATH is not null or T_PHOTO_PATH is not null) group by T_PHOTO_PATH  SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME,PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH,(select SUM(T_3D_PATH) as T_3D_PATH from #temp) as DCount,(select SUM(T_PHOTO_PATH) as T_PHOTO_PATH from #temp1) as ToolCount FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE P_CM_COMP_ID = '1' and TOOL_MASTER.ES_DELETE=0 group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC drop table #temp drop table #temp1");
                dtfilter = CommonClasses.Execute(" SELECT TOOL_MASTER.T_CODE, CASE WHEN  T_TYPE=0 then 'DIE' ELSE 'CORE BOX' END AS  T_TYPE,TOOL_MASTER.T_NAME,PARTY_MASTER.P_CODE, PARTY_MASTER.P_NAME, ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,T_PHOTO_PATH,T_3D_PATH, SUM(case when T_3D_PATH='' then 0 else 1 END) as  DCount, SUM(case when T_PHOTO_PATH='' then 0 else 1 end)   as ToolCount  FROM TOOL_MASTER INNER JOIN PARTY_MASTER ON TOOL_MASTER.T_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON TOOL_MASTER.T_I_CODE = ITEM_MASTER.I_CODE WHERE P_CM_COMP_ID = '1' and TOOL_MASTER.ES_DELETE=0 group by T_CODE,T_TYPE,T_NAME,P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,T_PHOTO_PATH,T_3D_PATH ORDER BY T_CODE,T_TYPE DESC  ");

            if (dtfilter.Rows.Count > 0)
            {
                dgToolMaster.DataSource = dtfilter;
                dgToolMaster.DataBind();
                dgToolMaster.Enabled = true;
                ViewState["cnt"] = dtfilter.Rows.Count.ToString();
                long sum = (long)dtfilter.Compute("Sum(ToolCount)", "True");
                long sum1 = (long)dtfilter.Compute("Sum(DCount)", "True");
                dgToolMaster.HeaderRow.Cells[9].Text = "Tool Image (" + sum + "/" + ViewState["cnt"].ToString() + ")";
                dgToolMaster.HeaderRow.Cells[10].Text = "3D Image (" + sum + "/" + ViewState["cnt"].ToString() + ")";
            }
            else
            {
                dtFilter.Clear();
                dgToolMaster.HeaderRow.Cells[9].Text = "Tool Image  ";
                dgToolMaster.HeaderRow.Cells[10].Text = "3D Image ";
                if (dtFilter.Columns.Count == 0)
                {
                    ViewState["cnt"] = "0";
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_PHOTO_PATH", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_3D_PATH", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("DCount", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ToolCount", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgToolMaster.DataSource = dtFilter;
                    dgToolMaster.DataBind();
                    dgToolMaster.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tool Master", "LoadStatus", ex.Message);
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
                Response.Redirect("~/ToolRoom/ADD/Tools.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ToolsDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Grid events
    protected void dgToolMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgToolMaster.Rows[e.RowIndex].FindControl("lblT_CODE"))).Text))
                {

                    string t_code = ((Label)(dgToolMaster.Rows[e.RowIndex].FindControl("lblT_CODE"))).Text;
                    if (CommonClasses.CheckUsedInTran("MP_PREVENTIVE_MAINTENANCE_MASTER", "PM_TOOL_NO", "AND ES_DELETE=0", t_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Monthly Preventive Maintenance";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    if (CommonClasses.CheckUsedInTran("TDMODELSTORAGE_MASTER", "TMS_I_CODE", "AND ES_DELETE=0", t_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in 3D Model Storage and Request For Revision";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    bool flag = CommonClasses.Execute1("UPDATE TOOL_MASTER SET ES_DELETE = 1 WHERE T_CODE='" + Convert.ToInt32(t_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Tool Master", "Delete", "Tool Master", t_code, Convert.ToInt32(t_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadTools();
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
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
            CommonClasses.SendError("Tool Master", "dgToolMaster_RowDeleting", Ex.Message);
        }
    }
    protected void dgToolMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string t_code = ((Label)(dgToolMaster.Rows[e.NewEditIndex].FindControl("lblT_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/ToolRoom/ADD/tools.aspx?c_name=" + type + "&p_code=" + t_code, false);
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
            CommonClasses.SendError("Tool Master", "dgToolMaster_RowEditing", Ex.Message);
        }
    }
    protected void dgToolMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgToolMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    protected void dgToolMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string filePath = "";
            string code = "";
            string directory = "";

            if (e.CommandName.Equals("View"))
            {
                int index = Convert.ToInt32(e.CommandArgument);

                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string T_code = e.CommandArgument.ToString();
                    Response.Redirect("~/ToolRoom/ADD/Tools.aspx?c_name=" + type + "&i_uom_code=" + T_code, false);
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
                int index = Convert.ToInt32(e.CommandArgument);

                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string T_code = e.CommandArgument.ToString();
                        Response.Redirect("~/ToolRoom/ADD/Tools.aspx?c_name=" + type + "&p_code=" + T_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
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
            if (e.CommandName == "ViewPDF")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                int page = dgToolMaster.PageIndex;
                int Size = dgToolMaster.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgToolMaster.Rows[index];

                code = ((Label)(gvRow.FindControl("lblT_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkTToolmage"))).Text;
                directory = "../../UpLoadPath/ToolRoom/ToolPhoto/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;
            }
            if (e.CommandName == "ViewPDF1")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                int page = dgToolMaster.PageIndex;
                int Size = dgToolMaster.PageSize;
                index = index - (page * Size);
                GridViewRow gvRow = dgToolMaster.Rows[index];

                code = ((Label)(gvRow.FindControl("lblT_CODE"))).Text;
                filePath = ((LinkButton)(gvRow.FindControl("lnkTPhotoImage"))).Text;
                directory = "../../UpLoadPath/ToolRoom/3DModel/" + code + "/" + filePath;
                ModalPopDocument.Show();
                IframeViewPDF.Attributes["src"] = directory;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tool Master", "GridView1_RowCommand", Ex.Message);
        }
    }
    protected void dgToolMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    protected void dgToolMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string lblToolCount = "", lblTotal = "";
        //Find Control from Grid and assign store image count in the string

        if (dgToolMaster.Rows.Count > 0)
        {
            int RowCount = dgToolMaster.Rows.Count;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int page = dgToolMaster.PageCount;
                int Size = dgToolMaster.PageSize;


                Label lblToolImg = (e.Row.FindControl("lblToolCount") as Label);//T_PHOTO_PATH
                lblToolCount = "" + lblToolImg.Text + "/" + ViewState["cnt"].ToString() + " ";

                Label lblTImg = (e.Row.FindControl("lblTCount") as Label);//T_3D_PATH
                lblTotal = "" + lblTImg.Text + "/" + ViewState["cnt"].ToString() + " ";

                if (dgToolMaster.HeaderRow != null)
                {
                    //Tool Image count
                    Label txtToolImg = (Label)dgToolMaster.HeaderRow.FindControl("txtToolImg");
                    txtToolImg.Text = "Tool Image (" + lblToolCount + ")";

                    //3D Image count
                    Label txtTImage = (Label)dgToolMaster.HeaderRow.FindControl("txtTImage");
                    txtTImage.Text = "3D Image (" + lblTotal + ")";
                }
            }
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select ES_MODIFY from TOOL_MASTER where  T_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Tool Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Tool Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
