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

public partial class Transactions_VIEW_ViewIssueFillOffSheet : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='105'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();

                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("FOS_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("FOS_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("FOS_BATCH_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("FOS_DATE", typeof(string)));
                        //dtFilter.Columns.Add(new System.Data.DataColumn("CUST_NAME", typeof(string)));
                       
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgFillOffSheet.DataSource = dtFilter;
                        dgFillOffSheet.DataBind();
                        //dgFillOffSheet.Enabled = false;
                    }
                    LoadFillOffSheet();                   
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill Off Sheet-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadFillOffSheet
    private void LoadFillOffSheet()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT FOS_CODE,FOS_NO,BT_NO as FOS_BATCH_NO,convert(varchar,FOS_DATE,106) as FOS_DATE FROM ISSUE_FILL_OFF_SHEET,BATCH_MASTER WHERE BT_CODE=FOS_BT_CODE and FOS_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND ISSUE_FILL_OFF_SHEET.ES_DELETE=0  ORDER BY FOS_NO DESC");
            if (dt.Rows.Count == 0)
            {
                LoadStatus(txtString);
            }
            else
            {
                dgFillOffSheet.DataSource = dt;
                dgFillOffSheet.DataBind();             
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill Off Sheet-View", "LoadFillOffSheet", Ex.Message);
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
            CommonClasses.SendError("Issue Fill Off Sheet-View", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT FOS_CODE,FOS_NO,BT_NO as FOS_BATCH_NO,convert(varchar,FOS_DATE,106) as FOS_DATE  FROM ISSUE_FILL_OFF_SHEET ,BATCH_MASTER WHERE BT_CODE=FOS_BT_CODE and FOS_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND ISSUE_FILL_OFF_SHEET.ES_DELETE='0'  and (upper(BT_NO) like upper('%" + str + "%') OR upper(FOS_NO) like upper('%" + str + "%') OR CONVERT(VARCHAR, FOS_DATE, 106) like ('%" + str + "%')  ) ORDER BY FOS_NO DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT FOS_CODE,FOS_NO,BT_NO as FOS_BATCH_NO,convert(varchar,FOS_DATE,106) as FOS_DATE FROM ISSUE_FILL_OFF_SHEET ,BATCH_MASTER WHERE BT_CODE=FOS_BT_CODE and FOS_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND ISSUE_FILL_OFF_SHEET.ES_DELETE='0'  ORDER BY FOS_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgFillOffSheet.DataSource = dtfilter;
                dgFillOffSheet.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOS_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOS_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOS_BATCH_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOS_DATE", typeof(string)));
                    //dtFilter.Columns.Add(new System.Data.DataColumn("CUST_NAME", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgFillOffSheet.DataSource = dtFilter;
                    dgFillOffSheet.DataBind();
                    dgFillOffSheet.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill Off Sheet-View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/IssueFillOffSheet.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill Off Sheet-View", "btnAddNew_Click", Ex.Message);
        }
    }

    #endregion

    #region Grid Events    

    protected void dgFillOffSheet_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgFillOffSheet.Rows[e.RowIndex].FindControl("lblFOS_CODE"))).Text))
            {

                string ce_code = ((Label)(dgFillOffSheet.Rows[e.RowIndex].FindControl("lblFOS_CODE"))).Text;

               
                bool flag = CommonClasses.Execute1("UPDATE ISSUE_FILL_OFF_SHEET  SET ES_DELETE = 1 WHERE FOS_CODE='" + Convert.ToInt32(ce_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Fill Off Sheet", "Delete", "Fill Off Sheet", ce_code, Convert.ToInt32(ce_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                       
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadFillOffSheet();
               
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
            lblmsg.Text = "Record used by another person";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }

    protected void dgFillOffSheet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgFillOffSheet.PageIndex = e.NewPageIndex;
        LoadFillOffSheet();
    }

    protected void dgFillOffSheet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string ce_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/IssueFillOffSheet.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
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
                        Response.Redirect("~/Transactions/ADD/IssueFillOffSheet.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
                      
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

            //if (e.CommandName.Equals("Print"))
            //{
            //    if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            //    {
            //        //string type = "MODIFY";
            //        string cpom_code = e.CommandArgument.ToString();                  
            //        Response.Redirect("~/RoportForms/ADD/FillOffSheetPrint.aspx?cpom_code=" + cpom_code, false);
            //    }
            //    else
            //    {                   
            //        lblmsg.Text = "You have no rights to print";
            //        PanelMsg.Visible = true;
            //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            //        return;
            //    }
            //}           

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill Off Sheet-View", "dgFillOffSheet_RowCommand", Ex.Message);
        }
    }
  
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from ISSUE_FILL_OFF_SHEET where FOS_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Issue Fill Off Sheet-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Issue Fill Off Sheet-View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
