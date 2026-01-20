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


public partial class Transactions_VIEW_ViewCustomerEnquiry : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    DatabaseAccessLayer databaseaccess = new DatabaseAccessLayer();

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='95'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();

                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("INQ_CODE", typeof(string)));

                        dtFilter.Columns.Add(new System.Data.DataColumn("INQ_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("INQ_REQ_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("INQ_CUST_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("INQ_SHADE_NAME", typeof(string)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgCustomerEnquiry.DataSource = dtFilter;
                        dgCustomerEnquiry.DataBind();
                        dgCustomerEnquiry.Enabled = false;
                    }

                    LoadEnquiry();
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadEnquiry
    private void LoadEnquiry()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT INQ_CODE,CONVERT(varchar(10),isnull(P_ABBREVATION,''))+''+CONVERT(varchar(10), INQ_NO) as INQ_NO,convert(varchar,INQ_REQ_DATE,106) as INQ_REQ_DATE,P_NAME as INQ_CUST_NAME,INQ_SHADE_NAME FROM ENQUIRY_MASTER,PARTY_MASTER WHERE INQ_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' AND ENQUIRY_MASTER.ES_DELETE=0 and P_CODE=INQ_CUST_NAME  ORDER BY INQ_CODE desc");
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
            CommonClasses.SendError("Customer Enquiry - View", "LoadEnquiry", Ex.Message);
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
            CommonClasses.SendError("Customer Enquiry - View", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT INQ_CODE,CONVERT(varchar(10),isnull(P_ABBREVATION,''))+''+CONVERT(varchar(10), INQ_NO) as INQ_NO,convert(varchar,INQ_REQ_DATE,106) as INQ_REQ_DATE,P_NAME as INQ_CUST_NAME,INQ_SHADE_NAME  FROM ENQUIRY_MASTER,PARTY_MASTER WHERE INQ_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' AND ENQUIRY_MASTER.ES_DELETE='0' and P_CODE=INQ_CUST_NAME and (upper(INQ_NO) like upper('%" + str + "%') OR CONVERT(VARCHAR, INQ_REQ_DATE, 106) like ('%" + str + "%') OR upper(P_ABBREVATION) like upper('%" + str + "%') OR upper(INQ_CUST_NAME) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR  upper(INQ_SHADE_NAME) like upper('%" + str + "%')) ORDER BY INQ_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT INQ_CODE,CONVERT(varchar(10),isnull(P_ABBREVATION,''))+''+CONVERT(varchar(10), INQ_NO) as INQ_NO,convert(varchar,INQ_REQ_DATE,106) as INQ_REQ_DATE,P_NAME as INQ_CUST_NAME,INQ_SHADE_NAME FROM ENQUIRY_MASTER,PARTY_MASTER WHERE INQ_CM_COMP_ID= '" + Convert.ToInt32(Session["CompanyId"]) + "' AND ENQUIRY_MASTER.ES_DELETE=0 and P_CODE=INQ_CUST_NAME  ORDER BY INQ_CODE DESC");

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
                    dtFilter.Columns.Add(new System.Data.DataColumn("INQ_CODE", typeof(string)));

                    dtFilter.Columns.Add(new System.Data.DataColumn("INQ_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INQ_REQ_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INQ_CUST_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INQ_SHADE_NAME", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerEnquiry.DataSource = dtFilter;
                    dgCustomerEnquiry.DataBind();
                    dgCustomerEnquiry.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Enquiry - View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/CustomerEnquiry.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to add";

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry - View", "btnAddNew_Click", Ex.Message);
        }
    }

    #endregion

    #region Grid Events
    protected void dgCustomerEnquiry_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string ce_code = ((Label)(dgCustomerEnquiry.Rows[e.NewEditIndex].FindControl("lblCE_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Transactions/ADD/CustomerEnquiry.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to modify";

                //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry - View", "dgCustomerEnquiry_RowEditing", Ex.Message);
        }
    }

    protected void dgCustomerEnquiry_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgCustomerEnquiry.Rows[e.RowIndex].FindControl("lblINQ_CODE"))).Text))
            {

                string ce_code = ((Label)(dgCustomerEnquiry.Rows[e.RowIndex].FindControl("lblINQ_CODE"))).Text;

                if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER", "CPOM_INQ_CODE", "AND ES_DELETE=0", ce_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Sale Order";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //ShowMessage("#Avisos", "You cant delete this record it has used in Quotation", CommonClasses.MSG_Warning);
                }
                else
                {
                    bool flag = CommonClasses.Execute1("UPDATE ENQUIRY_MASTER  SET ES_DELETE = 1 WHERE INQ_CODE='" + Convert.ToInt32(ce_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Customer Enquiry", "Delete", "Customer Enquiry", ce_code, Convert.ToInt32(ce_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                       
                        //   ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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

            LoadEnquiry();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record used by another person";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            return;
        }
    }
    protected void dgCustomerEnquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgCustomerEnquiry.PageIndex = e.NewPageIndex;
        LoadEnquiry();
    }
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
                    Response.Redirect("~/Transactions/ADD/CustomerEnquiry.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to view";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    // ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
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
                        DataTable dtcheck = CommonClasses.Execute("select INQ_CODE,INQ_QT_FLAG from ENQUIRY_MASTER where INQ_CODE=" + ce_code + "");
                        if (dtcheck.Rows[0]["INQ_QT_FLAG"].ToString() == "True")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Perticular Enquiry is used In Sale Order, can't modify the record";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            //ShowMessage("#Avisos", "Perticular Enquiry is used In quotation, can't modify the record", CommonClasses.MSG_Erro);
                        }
                        else
                        {
                            Response.Redirect("~/Transactions/ADD/CustomerEnquiry.aspx?c_name=" + type + "&ce_code=" + ce_code, false);
                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        // ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }

            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();                  
                    Response.Redirect("~/RoportForms/ADD/CustomerEnquiryPrint.aspx?cpom_code=" + cpom_code, false);
                }
                else
                {                   
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            if (e.CommandName.Equals("Convert"))
            {
                string cpom_code = e.CommandArgument.ToString();   
                string Sale = databaseaccess.GetColumn("select isnull(INQ_QT_FLAG,0) as INQ_QT_FLAG from ENQUIRY_MASTER where INQ_CODE='"+cpom_code+"'");
                if (!Convert.ToBoolean(Sale))
                {
                    string type = "ConvertToOrder";
                    Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    lblmsg.Text = "Allready Create Sale Order";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Enquiry - View", "dgCustomerEnquiry_RowCommand", Ex.Message);
        }
    }
    protected void dgCustomerEnquiry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from ENQUIRY_MASTER where INQ_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Customer Enquiry - View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Customer Enquiry - View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
       
}
