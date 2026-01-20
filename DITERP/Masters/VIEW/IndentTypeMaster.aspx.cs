using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;


public partial class Masters_VIEW_IndentTypeMaster : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    static string fieldName;
    DatabaseAccessLayer DB_Access;
    DataTable dt = new DataTable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='278'");
                    right = dtRights.Rows.Count.Equals(0) ? "0000000" : dtRights.Rows[0][0].ToString();

                    displaydata();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Type Master - View", "Page_Load", Ex.Message);
        }
    }

    #region bindgrid
    public void bindgrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(string)));
        dt.Columns.Add(new System.Data.DataColumn("IM_SHORT", typeof(string)));
        dt.Columns.Add(new System.Data.DataColumn("IM_DESC", typeof(string)));
     
        dt.Rows.Add(dt.NewRow());
        dgvMachinMaster.DataSource = dt;
        dgvMachinMaster.DataBind();

    }
    #endregion

    //This method is use for display the data of the database on gridview
    #region displaydata
    public void displaydata()
    {

        dt = new DataTable();
        dt = CommonClasses.Execute("SELECT IM_CODE,UPPER(IM_SHORT)AS IM_SHORT,UPPER(IM_DESC)AS IM_DESC FROM INDENT_TYPE_MASTER where IM_CM_ID=" + Session["CompanyId"] + "   AND  INDENT_TYPE_MASTER.ES_DELETE=0 ORDER BY IM_SHORT");

        if (dt.Rows.Count > 0)
        {
            dgvMachinMaster.DataSource = dt;
            dgvMachinMaster.DataBind();
        }
        else
        {
            bindgrid();
        }
    }
    #endregion

    //For seaching the perticular entry
    #region Search
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        LoadStatus(txtString);
    }

    public void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = CommonClasses.ToLiteral(txtString.Text.Trim());

            DataTable dt1 = new DataTable();

            if (txtString.Text != "")
                dt1 = CommonClasses.Execute("SELECT IM_CODE,UPPER(IM_SHORT) AS IM_SHORT,UPPER(IM_DESC) AS IM_DESC FROM INDENT_TYPE_MASTER  WHERE  INDENT_TYPE_MASTER.ES_DELETE='0' and (IM_SHORT like ('%" + str + "%') OR IM_DESC like ('%" + str + "%')) order by IM_SHORT");
            else
                dt1 = CommonClasses.Execute("SELECT IM_CODE,UPPER(IM_SHORT)AS IM_SHORT,UPPER(IM_DESC)AS IM_DESC FROM INDENT_TYPE_MASTER where IM_CM_ID=" + Session["CompanyId"] + "   AND  INDENT_TYPE_MASTER.ES_DELETE=0 ORDER BY IM_SHORT");

            if (dt1.Rows.Count > 0)
            {
                dgvMachinMaster.DataSource = dt1;
                dgvMachinMaster.DataBind();
                dgvMachinMaster.Enabled = true;
                dgvMachinMaster.Columns[0].Visible = true;
            }
            else
            {
                //displaydata();
                dt.Clear();
                if (dt.Columns.Count.Equals(0))
                {
                    dt.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(string)));
                    dt.Columns.Add(new System.Data.DataColumn("IM_SHORT", typeof(string)));
                    dt.Columns.Add(new System.Data.DataColumn("IM_DESC", typeof(string)));


                    dt.Rows.Add(dt.NewRow());
                    dgvMachinMaster.DataSource = dt;
                    dgvMachinMaster.DataBind();
                    dgvMachinMaster.Enabled = false;
                    dgvMachinMaster.Columns[0].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Type Master - View", "LoadStatus", ex.Message);
        }
    }
    #endregion

    //Add new Machine Master data
    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string inm = "insert";
                Response.Redirect("~/Masters/ADD/IndentTypeMaster.aspx?insert=" + inm, false);
                return;
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
            CommonClasses.SendError("Indent Type Master - View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    //On row command View and Modify(Update) the data
    #region dgvMachinMaster_RowCommand
    protected void dgvMachinMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Modify")
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                int rindex = Convert.ToInt32(e.CommandArgument.ToString());
                string mcode = e.CommandArgument.ToString();
                string comname = "";
                ViewState["index"] = rindex;
                comname = "modify";
                Response.Redirect("~/Masters/ADD/IndentTypeMaster.aspx?modify=" + comname + "&IM_CODE=" + mcode, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You cant update this record it has used in Operator Master";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }

        }
        else if (e.CommandName == "View")
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Modify"))
            {
                int rindex = Convert.ToInt32(e.CommandArgument.ToString());
                string mcode = e.CommandArgument.ToString();
                string comname = "";
                ViewState["index"] = rindex;
                comname = "View";
                Response.Redirect("~/Masters/ADD/IndentTypeMaster.aspx?modify=" + comname + "&IM_CODE=" + mcode, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You cant view this record it has used in Operator Master";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }

        }
    }
    #endregion

    //Give the page index to the gridview, like 1-15 records on one page then another 15 on another page of gridview
    #region dgvMachinMaster_PageIndexChanging
    protected void dgvMachinMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgvMachinMaster.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
            displaydata();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Type Master", "dgvMachinMaster_PageIndexChanging", ex.Message);
        }
    }
    #endregion

    //Deleting the row of gridview not data from the database
    #region dgvMachinMaster_RowDeleting
    protected void dgvMachinMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgvMachinMaster.Rows[e.RowIndex].FindControl("lblIM_CODE"))).Text))
            {
                string m_code = ((Label)(dgvMachinMaster.Rows[e.RowIndex].FindControl("lblIM_CODE"))).Text;
                string m_name = ((Label)(dgvMachinMaster.Rows[e.RowIndex].FindControl("lblIM_SHORT"))).Text;
                string m_desc = ((Label)(dgvMachinMaster.Rows[e.RowIndex].FindControl("lblIM_DESC"))).Text;
                if (CommonClasses.CheckUsedInTran("INDENT_MASTER,INDENT_DETAIL", "IN_TYPE", "AND  INM_CODE=IND_INM_CODE AND ES_DELETE=0", m_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Indent ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                bool flag = CommonClasses.Execute1("update INDENT_TYPE_MASTER set ES_DELETE=1 where IM_CODE='" + Convert.ToInt32(m_code) + "'");
                if (flag == true)
                {
                    ViewState["index"] = e.RowIndex;
                    //Label lblM_CODE = dgvMachinMaster.Rows[e.RowIndex].FindControl("lblM_CODE") as Label;
                    //int mc = Convert.ToInt32(lblM_CODE.Text);
                    CommonClasses.WriteLog("Indent Type Master", "Delete", "Indent Type Master", m_name, Convert.ToInt32(m_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    //  dt = new DataTable();
                    // dt = CommonClasses.Execute("update MACHINE_MASTER set ES_DELETE=1 where M_CODE='" + mc + "'");
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Deleted Successfully";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
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
            lblmsg.Text = "You cant delete this record it has used in indent transaction";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }

        displaydata();
    }
    #endregion

    //This method is use for lock the record 
    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from INDENT_TYPE_MASTER where IM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Indent Type Master", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    //Button Close, the view form of Machine master click on cross sign then jump on the master default page
    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Indent Type Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion
}
