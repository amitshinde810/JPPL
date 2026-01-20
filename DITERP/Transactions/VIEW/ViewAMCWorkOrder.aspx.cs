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

public partial class Transactions_VIEW_ViewAMCWorkOrder : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='70'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    dtFilter.Clear();
                    dgWODetails.Enabled = false;
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("WO_AMC_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("WO_AMC_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("WO_GRAND_TOT", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("WO_AM_COUNT", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgWODetails.DataSource = dtFilter;
                        dgWODetails.DataBind();
                    }

                    LoadWODetail();



                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("AMC WO Transaction", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadWODetail
    private void LoadWODetail()
    {
        try
        {
            DataTable dt = new DataTable();


            //dt = CommonClasses.Execute("select SPOM_AMOUNT, P_NAME,SPOM_CODE,SPOM_PO_NO,convert(varchar,SPOM_DATE,106) as SPOM_DATE,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER ,PARTY_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 and SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by SPOM_PO_NO DESC");
            dt = CommonClasses.Execute("select WO_AMC_CODE,WO_AMC_NO,convert(varchar,WO_PO_DATE,106) as SPOM_DATE,P_NAME,cast(WO_GRAND_TOT as numeric(20,2)) as WO_GRAND_TOT,case when WO_AM_COUNT =0 then 'NO' else 'YES' end as WO_AM_COUNT from WORK_AMC_ORDER_MASTER,PARTY_MASTER where WORK_AMC_ORDER_MASTER.ES_DELETE=0  and WO_TYPE='WO' and WO_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 and WO_CM_CODE=" + (string)Session["CompanyCode"] + " order by WO_AMC_NO DESC");
            if (dt.Rows.Count > 0)
            {
                dgWODetails.Enabled = true;
                dgWODetails.DataSource = dt;
                dgWODetails.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgWODetails.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_AMC_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_AMC_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_GRAND_TOT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_AM_COUNT", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgWODetails.DataSource = dtFilter;
                    dgWODetails.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "LoadWODetail", Ex.Message);
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
                Response.Redirect("~/Transactions/ADD/AMCWorkOrder.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("AMC WO Transaction", "txtString_TextChanged", Ex.Message);
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

                dtfilter = CommonClasses.Execute("select WO_AMC_CODE,WO_AMC_NO,convert(varchar,WO_PO_DATE,106) as SPOM_DATE,P_NAME,cast(WO_GRAND_TOT as numeric(20,2)) as WO_GRAND_TOT,case when WO_AM_COUNT =0 then 'NO' else 'YES' end as WO_AM_COUNT from WORK_AMC_ORDER_MASTER,PARTY_MASTER where WORK_AMC_ORDER_MASTER.ES_DELETE=0 and WO_TYPE='WO' and WO_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 and WO_CM_CODE=" + (string)Session["CompanyCode"] + " and (WO_AMC_NO like upper('%" + str + "%') OR convert(varchar,WO_PO_DATE,106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR WO_GRAND_TOT like '%" + str + "%' ) order by WO_AMC_NO DESC");

            else
                dtfilter = CommonClasses.Execute("select WO_AMC_CODE,WO_AMC_NO,convert(varchar,WO_PO_DATE,106) as SPOM_DATE,P_NAME,cast(WO_GRAND_TOT as numeric(20,2)) as WO_GRAND_TOT,case when WO_AM_COUNT =0 then 'NO' else 'YES' end as WO_AM_COUNT from WORK_AMC_ORDER_MASTER,PARTY_MASTER where WORK_AMC_ORDER_MASTER.ES_DELETE=0 and WO_TYPE='WO' and WO_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 and WO_CM_CODE=" + (string)Session["CompanyCode"] + " order by WO_AMC_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgWODetails.Enabled = true;
                dgWODetails.DataSource = dtfilter;
                dgWODetails.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dgWODetails.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_AMC_CODE", typeof(String)));

                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_AMC_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_GRAND_TOT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_AM_COUNT", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgWODetails.DataSource = dtFilter;
                    dgWODetails.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region dgWODetails_RowEditing

    protected void dgWODetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string lblWO_AMC_CODE = ((Label)(dgWODetails.Rows[e.NewEditIndex].FindControl("lblWO_AMC_CODE"))).Text;
            string type = "MODIFY";


            Response.Redirect("~/Transactions/ADD/AMCWorkOrder.aspx?c_name=" + type + "&lblWO_AMC_CODE=" + lblWO_AMC_CODE, false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "dgWODetails_RowEditing", Ex.Message);
        }
    }


    #endregion

    #region dgWODetails_RowDeleting

    protected void dgWODetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgWODetails.Rows[e.RowIndex].FindControl("lblWO_AMC_CODE"))).Text))
                {
                    string WO_AMC_CODE = ((Label)(dgWODetails.Rows[e.RowIndex].FindControl("lblWO_AMC_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE WORK_AMC_ORDER_MASTER SET ES_DELETE = 1 WHERE WO_AMC_CODE='" + Convert.ToInt32(WO_AMC_CODE) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("Annual Mainteance Contract", "Delete", "Annual Mainteance Contract", WO_AMC_CODE, Convert.ToInt32(WO_AMC_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


                    }
                    LoadWODetail();
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
            }
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "dgDetailPO_RowDeleting", Ex.Message);
        }



    }

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from WORK_AMC_ORDER_MASTER where WO_AMC_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region dgWODetails_RowCommand
    protected void dgWODetails_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string WO_AMC_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/AMCWorkOrder.aspx?c_name=" + type + "&WO_AMC_CODE=" + WO_AMC_CODE, false);
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
                    lblmsg.Text = "You have no rights to View";
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
                        string WO_AMC_CODE = e.CommandArgument.ToString();

                        Response.Redirect("~/Transactions/ADD/AMCWorkOrder.aspx?c_name=" + type + "&WO_AMC_CODE=" + WO_AMC_CODE, false);

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Record Is used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
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
            if (e.CommandName.Equals("AMEND"))
            {

                if (!ModifyLog(e.CommandArgument.ToString()))
                {

                    string type = "AMEND";
                    string cpom_code = e.CommandArgument.ToString();
                    int cnt = 0;

                    Response.Redirect("~/Transactions/ADD/AMCWorkOrder.aspx?c_name=" + type + "&WO_AMC_CODE=" + cpom_code, false);

                    //}
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                    return;
                }

            }
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {


                        string WO_AMC_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/RoportForms/ADD/AMCWOPrint.aspx?WO_AMC_CODE=" + WO_AMC_CODE, false);
                    }

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC WO Transaction", "dgWODetails_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgWODetails_PageIndexChanging
    protected void dgWODetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgWODetails.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
