using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_VIEW_ViewSupplierPerfRemark : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='247'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Menu"))
                    {
                        LoadStore();
                        dgSuppCategory.Enabled = true;
                        if (dgSuppCategory.Rows.Count == 0)
                        {
                            dtFilter.Clear();
                            if (dtFilter.Columns.Count == 0)
                            {
                                dtFilter.Columns.Add(new System.Data.DataColumn("SPR_CODE", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("SPR_NAME", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("SPR_FROM", typeof(string)));
                                dtFilter.Columns.Add(new System.Data.DataColumn("SPR_TO", typeof(string)));

                                dtFilter.Rows.Add(dtFilter.NewRow());
                                dgSuppCategory.DataSource = dtFilter;
                                dgSuppCategory.DataBind();
                                dgSuppCategory.Enabled = false;
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
            CommonClasses.SendError("Supplier Performance Remark", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadStore
    private void LoadStore()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM SUPPLIER_PERFORMANCE_REMARK WHERE SPR_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0 order by SPR_NAME ");

            dgSuppCategory.DataSource = dt;
            dgSuppCategory.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Category Master", "LoadUser", Ex.Message);
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
                Response.Redirect("~/Masters/Add/SupplierPerfRemark.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";

            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Supplier Performance Remark", "btnInsert_Click", exc.Message);
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
            CommonClasses.SendError("Supplier Performance Remark - View", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT  * FROM SUPPLIER_PERFORMANCE_REMARK WHERE ES_DELETE='FALSE' AND SPR_CM_COMP_ID = '" + Session["CompanyId"] + "' and lower(SPR_NAME) like lower('%" + str + "%') order by SPR_NAME");
            else
                dtfilter = CommonClasses.Execute("SELECT  * FROM SUPPLIER_PERFORMANCE_REMARK WHERE ES_DELETE='FALSE' AND SPR_CM_COMP_ID = '" + Session["CompanyId"] + "' order by SPR_NAME ");


            if (dtfilter.Rows.Count > 0)
            {
                dgSuppCategory.DataSource = dtfilter;
                dgSuppCategory.DataBind();
                dgSuppCategory.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPR_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPR_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPR_FROM", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPR_TO", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgSuppCategory.DataSource = dtFilter;
                    dgSuppCategory.DataBind();
                    dgSuppCategory.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Performance Remark - View", "LoadStatus", Ex.Message);
        }
    }
    #endregion

    #region dgSuppCategory_PageIndexChanging
    protected void dgSuppCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgSuppCategory.PageIndex = e.NewPageIndex;
            LoadStore();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Performance Remark", "GridView1_PageIndexChanging", Ex.Message);
        }
    }
    #endregion dgSuppCategory_PageIndexChanging

    #region dgSuppCategory_RowCommand
    protected void dgSuppCategory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string um_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Masters/Add/SupplierPerfRemark.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
                    string um_code = e.CommandArgument.ToString();
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        Response.Redirect("~/Masters/Add/SupplierPerfRemark.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            CommonClasses.SendError("Supplier Category Master", "GridView1_RowCommand", exc.Message);
        }
    }
    #endregion dgSuppCategory_RowCommand

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from SUPPLIER_PERFORMANCE_REMARK where SPR_CODE=" + PrimaryKey + "  ");
            if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another User";
                return true;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Supplier Performance Remark", "GridView1_RowEditing", exc.Message);
        }

        return false;
    }
    #endregion

    #region dgSuppCategory_RowDeleting
    protected void dgSuppCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgSuppCategory.Rows[e.RowIndex].FindControl("lblSPR_CODE"))).Text))
                {
                    bool flag = false;
                    string um_code = ((Label)(dgSuppCategory.Rows[e.RowIndex].FindControl("lblSPR_CODE"))).Text;
                    string um_name = ((Label)(dgSuppCategory.Rows[e.RowIndex].FindControl("lblSPR_NAME"))).Text;

                    DataTable dtFixStores = CommonClasses.Execute("select SPR_CODE,SPR_NAME from SUPPLIER_PERFORMANCE_REMARK where SPR_CODE in (-2147483648,-2147483647,-2147483646,-2147483645,-2147483644,-2147483643,-2147483642,-2147483641) and SPR_CODE='" + um_code + "' AND ES_DELETE=0");
                    if (dtFixStores.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        flag = CommonClasses.Execute1("UPDATE SUPPLIER_PERFORMANCE_REMARK SET ES_DELETE = 1 WHERE SPR_CODE='" + Convert.ToInt32(um_code) + "'");
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }

                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Supplier Category Master", "Delete", "Supplier Category Master", um_name, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadStore();
                }
            }
            else
            {
                ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Supplier Performance Remark", "GridView1_RowEditing", exc.Message);
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
            CommonClasses.SendError("Supplier Performance Remark", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgSuppCategory_RowEditing
    protected void dgSuppCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgSuppCategory.Rows[e.NewEditIndex].FindControl("lblSPR_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/Add/StoreMaster.aspx?c_name=" + type + "&u_code=" + user_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("Supplier Performance Remark", "GridView1_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Performance Remark - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
