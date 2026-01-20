using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Transactions_VIEW_ViewShadeCreation : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    DatabaseAccessLayer databaseaccess = new DatabaseAccessLayer();
    #endregion

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='96'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();

                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("SHM_CODE", typeof(string)));

                        dtFilter.Columns.Add(new System.Data.DataColumn("SHM_FORMULA_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SHM_FORMULA_NAME", typeof(string)));
                       
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgCustomerEnquiry.DataSource = dtFilter;
                        dgCustomerEnquiry.DataBind();
                        dgCustomerEnquiry.Enabled = false;
                    }

                    LoadShade();
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadShade
    private void LoadShade()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT SHM_CODE,SHM_FORMULA_CODE,SHM_FORMULA_NAME FROM SHADE_MASTER WHERE  ES_DELETE=0  ORDER BY SHM_FORMULA_CODE");
            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(dt.NewRow());
                dgCustomerEnquiry.DataSource = dt;
                dgCustomerEnquiry.DataBind();
                dgCustomerEnquiry.Enabled = false;
            }
            else
            {
                dgCustomerEnquiry.DataSource = dt;
                dgCustomerEnquiry.DataBind();
                dgCustomerEnquiry.Enabled = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "LoadShade", Ex.Message);
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
            CommonClasses.SendError("Shade Creation", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT SHM_CODE,SHM_FORMULA_CODE,SHM_FORMULA_NAME FROM SHADE_MASTER WHERE  ES_DELETE=0  and (upper(SHM_FORMULA_CODE) like upper('%" + str + "%') OR upper(SHM_FORMULA_NAME) like upper('%" + str + "%')) ORDER BY SHM_FORMULA_CODE");
            else
                dtfilter = CommonClasses.Execute("SELECT SHM_CODE,SHM_FORMULA_CODE,SHM_FORMULA_NAME FROM SHADE_MASTER WHERE  ES_DELETE=0  ORDER BY SHM_FORMULA_CODE");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerEnquiry.DataSource = dtfilter;
                dgCustomerEnquiry.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SHM_CODE", typeof(string)));

                    dtFilter.Columns.Add(new System.Data.DataColumn("SHM_FORMULA_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SHM_FORMULA_NAME", typeof(string)));
                       
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerEnquiry.DataSource = dtFilter;
                    dgCustomerEnquiry.DataBind();
                    dgCustomerEnquiry.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/ShadeCreation.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to add";

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "btnAddNew_Click", Ex.Message);
        }
    }

    #endregion

    #region Grid Events

    #region dgCustomerEnquiry_RowEditing
    protected void dgCustomerEnquiry_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string ce_code = ((Label)(dgCustomerEnquiry.Rows[e.NewEditIndex].FindControl("lblCE_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Transactions/ADD/ShadeCreation.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "dgCustomerEnquiry_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerEnquiry_RowDeleting
    protected void dgCustomerEnquiry_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgCustomerEnquiry.Rows[e.RowIndex].FindControl("lblSHM_CODE"))).Text))
            {

                string ce_code = ((Label)(dgCustomerEnquiry.Rows[e.RowIndex].FindControl("lblSHM_CODE"))).Text;

                if (CommonClasses.CheckUsedInTran("BATCH_MASTER", "BT_SHM_CODE", "AND ES_DELETE=0", ce_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Batch Ticket";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    
                }
                else
                {
                bool flag = CommonClasses.Execute1("UPDATE SHADE_MASTER  SET ES_DELETE = 1 WHERE SHM_CODE='" + Convert.ToInt32(ce_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Shade Creation", "Delete", "Shade Creation", ce_code, Convert.ToInt32(ce_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted successfully";

                    }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            LoadShade();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record used by another person";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgCustomerEnquiry_PageIndexChanging
    protected void dgCustomerEnquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgCustomerEnquiry.PageIndex = e.NewPageIndex;
        LoadShade();
    }
    #endregion

    #region dgCustomerEnquiry_RowCommand
    protected void dgCustomerEnquiry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string ce_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/ShadeCreation.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to view";
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
                        string ce_code = e.CommandArgument.ToString();
                      
                        Response.Redirect("~/Transactions/ADD/ShadeCreation.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
                       
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/RoportForms/ADD/ShadePrint.aspx?cpom_code=" + cpom_code, false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
           
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "dgCustomerEnquiry_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgCustomerEnquiry_RowUpdating
    protected void dgCustomerEnquiry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from SHADE_MASTER where SHM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";

                    // ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Shade Creation", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

}