using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewBillPassing : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    BillPassing_BL billPassing_BL = null;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='41'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadBill();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward -View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region dgBillPassing_PageIndexChanging
    protected void dgBillPassing_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgBillPassing.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing - View", "dgBillPassing_PageIndexChanging", Ex.Message);
        }
    }
    #endregion

    #region LoadBill
    private void LoadBill()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select TOP 500 BPM_CODE,CASE WHEN BPM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,BPM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,BPM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,BPM_NO) AS  BPM_NO ,BPM_NO AS BillNO,BPM_P_CODE,BPM_INV_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,Cast(CONVERT(DECIMAL(10,2),BPM_G_AMT) as nvarchar) AS BPM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE=0 and P_CODE=BPM_P_CODE  and  BPM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BPM_CODE DESC ");
            if (dt.Rows.Count == 0)
            {
                if (dgBillPassing.Rows.Count == 0)
                {
                    dtFilter.Clear();
                    dgBillPassing.Enabled = false;
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("BPM_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BPM_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BPM_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BPM_P_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BPM_INV_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BPM_G_AMT", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgBillPassing.DataSource = dtFilter;
                        dgBillPassing.DataBind();
                    }
                }
            }
            else
            {
                dgBillPassing.Enabled = true;
                dgBillPassing.DataSource = dt;
                dgBillPassing.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "LoadBill", Ex.Message);
        }
    }
    #endregion

    #region dgBillPassing_RowCommand
    protected void dgBillPassing_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/BillPassing.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/BillPassing.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgBillPassing_RowDeleting
    protected void dgBillPassing_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgBillPassing.Rows[e.RowIndex].FindControl("lblBPM_CODE"))).Text))
                {
                    string cpom_code = ((Label)(dgBillPassing.Rows[e.RowIndex].FindControl("lblBPM_CODE"))).Text;
                    billPassing_BL = new BillPassing_BL();
                    billPassing_BL.BPM_CODE = Convert.ToInt32(cpom_code);
                    if (billPassing_BL.Delete())
                    {
                        CommonClasses.WriteLog("Bill Passing", "Delete", "Bill Passing", "", Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        LoadBill();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Deleted..";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing-View", "dgBillPassing_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgBillPassing_RowEditing
    protected void dgBillPassing_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from BILL_PASSING_MASTERS where BPM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Bill Passing-View", "ModifyLog", Ex.Message);
        }

        return false;
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
            CommonClasses.SendError("Bill Passing", "txtString_TextChanged", Ex.Message);
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

                dtfilter = CommonClasses.Execute("SELECT TOP 500 BPM_CODE,CASE WHEN BPM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,BPM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,BPM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,BPM_NO) AS  BPM_NO ,BPM_NO AS BillNO,BPM_P_CODE,BPM_INV_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,Cast(CONVERT(DECIMAL(10,2),BPM_G_AMT) as nvarchar) AS BPM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=BPM_P_CODE  and  BPM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND   (upper(BPM_NO) like upper('%" + str + "%') OR convert(varchar,BPM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(BPM_INV_NO) like upper('%" + str + "%')OR upper(BPM_G_AMT) like upper('%" + str + "%')) ");
            //dtfilter = CommonClasses.Execute("With Result As( SELECT TOP 500 BPM_CODE,CASE WHEN BPM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,BPM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,BPM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,BPM_NO) AS  BPM_NO ,BPM_NO AS BillNO,BPM_P_CODE,BPM_INV_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,Cast(CONVERT(DECIMAL(10,2),BPM_G_AMT) as nvarchar) AS BPM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=BPM_P_CODE  and  BPM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ) Select * From Result where (upper(BPM_NO) like upper('%" + str + "%') OR convert(varchar,BPM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(BPM_INV_NO) like upper('%" + str + "%')OR upper(BPM_G_AMT) like upper('%" + str + "%')) Order By BPM_NO Desc");
            //dtfilter = CommonClasses.Execute("SELECT TOP 500 BPM_CODE,CASE WHEN BPM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,BPM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,BPM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,BPM_NO) AS  BPM_NO ,BPM_NO AS BillNO,BPM_P_CODE,BPM_INV_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,Cast(CONVERT(DECIMAL(10,2),BPM_G_AMT) as nvarchar) AS BPM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=BPM_P_CODE  and  BPM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and (upper(BPM_NO) like upper('%" + str + "%') OR convert(varchar,BPM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(BPM_INV_NO) like upper('%" + str + "%')OR upper(BPM_G_AMT) like upper('%" + str + "%')) order by BPM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 BPM_CODE,CASE WHEN BPM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,BPM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,BPM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,BPM_NO) AS  BPM_NO ,BPM_NO AS BillNO,BPM_P_CODE,BPM_INV_NO,convert(varchar,BPM_DATE,106) as BPM_DATE,Cast(CONVERT(DECIMAL(10,2),BPM_G_AMT) as nvarchar) AS BPM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=BPM_P_CODE  and  BPM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BPM_CODE DESC ");
            if (dtfilter.Rows.Count > 0)
            {
                dgBillPassing.Enabled = true;
                dgBillPassing.DataSource = dtfilter;
                dgBillPassing.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgBillPassing.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("BPM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BPM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BPM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BPM_P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BPM_INV_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BPM_G_AMT", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgBillPassing.DataSource = dtFilter;
                    dgBillPassing.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/BillPassing.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Bill Passing-View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/PurchaseDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion
}
