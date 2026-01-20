using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Utility_VIEW_ViewTallyTransferSales : System.Web.UI.Page
{
    #region " Var "
    CustomerTypeMaster_BL BL_CustomerTypeMaster = null;
    static string right = "";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility1MV");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Utility");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='67'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadTallyDet();
                    if (dgTallySales.Rows.Count == 0)
                    {
                        if (dtFilter.Columns.Count == 0)
                        {
                            dgTallySales.Enabled = false;
                            dtFilter.Columns.Add(new System.Data.DataColumn("TTM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TTM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TTM_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TTM_ENTRY_TYPE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgTallySales.DataSource = dtFilter;
                            dgTallySales.DataBind();

                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "Page_Load", Ex.Message);
        }

    }
    #endregion

    #region Button Add
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Utility/ADD/TallyTransSales .aspx?c_name=" + type, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnAddNew_Click", Ex.Message);
        }

    }
    #endregion

    #region LoadTallyDet
    private void LoadTallyDet()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select TTM_CODE,TTM_NO,convert(varchar,TTM_DATE,106) as TTM_DATE,(Case TTM_ENTRY_TYPE when 0 then 'Create' else 'Alter' end) as TTM_ENTRY_TYPE FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE='" + Session["CompanyCode"] + "' and TTM_TYPE=0 order by TTM_CODE desc");
            
            dgTallySales.DataSource = dt;
            dgTallySales.DataBind();
            if (dgTallySales.Rows.Count > 0)
            {
                dgTallySales.Enabled = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Sales View", "LoadTallyDet", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from CUSTOMER_TYPE_MASTER where TTM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Tally Transfer Sales-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Tally Transfer Sales-View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/UtilityDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Row Command
    protected void dgTallySales_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.Equals("View"))
            {
                    if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                    {
                        string type = "VIEW";
                        string TTM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Utility/ADD/TallyTransSales .aspx?c_name=" + type + "&TTM_CODE=" + TTM_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Have No Rights To View";
                    }
                
            }
            //if (e.CommandName.Equals("Modify"))
            //{
            //    if (!ModifyLog(e.CommandArgument.ToString()))
            //    {
            //        if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            //        {
            //            string type = "MODIFY";
            //            string TTM_CODE = e.CommandArgument.ToString();
            //            Response.Redirect("~/Masters/ADD/CustomerTypeMaster.aspx?c_name=" + type + "&TTM_CODE=" + TTM_CODE, false);
            //        }
            //        else
            //        {
            //            PanelMsg.Visible = true;
            //            lblmsg.Text = "You Have No Rights To Modify";
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales", "dgTallySales_RowCommand", Ex.Message);
        }



    }
    #endregion

    #region dgTallySales_RowEditing
    protected void dgTallySales_RowEditing(object sender, GridViewEditEventArgs e)
    {


        try
        {
            string TTM_CODE = ((Label)(dgTallySales.Rows[e.NewEditIndex].FindControl("lblTTM_CODE"))).Text;
            string type = "MODIFY";
            Response.Redirect("~/Utility/ADD/TallyTransSales.aspx?c_name=" + type + "&TTM_CODE=" + TTM_CODE, false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tally Transfer Sales-View", "dgTallySales_RowEditing", Ex.Message);
        }

    }
    #endregion

    #region dgTallySales_PageIndexChanging
    protected void dgCustomerTypeMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgTallySales.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text;

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")

                dtfilter = CommonClasses.Execute("select TTM_CODE,TTM_NO,convert(varchar,TTM_DATE,106) as TTM_DATE,(Case TTM_ENTRY_TYPE when 0 then 'Create' else 'Alter' end) as TTM_ENTRY_TYPE FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE='" + Session["CompanyCode"] + "' and TTM_TYPE=0 and TTM_CODE like ('%" + str + "%') or TTM_NO like ('%" + str + "%') or CONVERT(VARCHAR, TTM_DATE, 106) like ('%" + str + "%') order by TTM_CODE");
            else
                dtfilter = CommonClasses.Execute("select TTM_CODE,TTM_NO,convert(varchar,TTM_DATE,106) as TTM_DATE,(Case TTM_ENTRY_TYPE when 0 then 'Create' else 'Alter' end) as TTM_ENTRY_TYPE FROM TALLY_TRANSFER_MASTER WHERE TTM_COMP_CODE='" + Session["CompanyCode"] + "' and TTM_TYPE=0 order by TTM_CODE");

            if (dtfilter.Rows.Count > 0)
            {
                dgTallySales.Enabled = true;
                dgTallySales.DataSource = dtfilter;
                dgTallySales.DataBind();
            }

            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgTallySales.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("TTM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TTM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TTM_DATE", typeof(String)));

                    dtFilter.Columns.Add(new System.Data.DataColumn("TTM_ENTRY_TYPE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgTallySales.DataSource = dtFilter;
                    dgTallySales.DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Transfer Sales-View", "LoadStatus", ex.Message);
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

        catch (Exception ex)
        {
            CommonClasses.SendError("Tally Transfer Sales-View", "btnSearch_Click", ex.Message);
        }
    }
    #endregion
}
