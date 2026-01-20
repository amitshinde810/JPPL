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


public partial class Transactions_VIEW_ViewPOTransfer : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string message = "";

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='42'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    dtFilter.Clear();
                    dgDetailSupplierPO.Enabled = false;
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_ORDER_QTY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_INW_QTY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_POST", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgDetailSupplierPO.DataSource = dtFilter;
                        dgDetailSupplierPO.DataBind();
                    }

                    LoadSupplierPO();
                    CommonClasses.FillCombo("PARTY_MASTER,SUPP_PO_MASTER", "P_NAME", "P_CODE", "SUPP_PO_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and SPOM_P_CODE=P_CODE  ", ddlSupplierName);
                    ddlSupplierName.Items.Insert(0, new ListItem("Select Supplier Name", "0"));
                    ddlSupplierName.Enabled = false;
                    txtFormDate.Enabled = false;
                    txtToDate.Enabled = false;
                    txtFormDate.Text = Convert.ToDateTime(Session["OpeningDate"]).ToString("dd/MM/yyyy");
                    txtToDate.Text = Convert.ToDateTime(Session["ClosingDate"]).ToString("dd/MM/yyyy");
                    txtFormDate.Attributes.Add("readonly", "readonly");
                    txtToDate.Attributes.Add("readonly", "readonly");
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Master", "Page_Load", Ex.Message);
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
            CommonClasses.SendError("Supplier PO Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region LoadSupplierPO
    private void LoadSupplierPO()
    {
        try
        {
            DataTable dt = new DataTable();
            string Query = "";
            string From = "";
            string To = "";
            From = txtFormDate.Text;
            To = txtToDate.Text;
            Query = "With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE  and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1";
            if (chkFormToAll.Checked == false)
            {
                if (From != "" && To != "")
                {
                    DateTime Date1 = Convert.ToDateTime(txtFormDate.Text);
                    DateTime Date2 = Convert.ToDateTime(txtToDate.Text);
                    if (Date1 < Convert.ToDateTime(Session["OpeningDate"]) || Date1 > Convert.ToDateTime(Session["ClosingDate"]) || Date2 < Convert.ToDateTime(Session["OpeningDate"]) || Date2 > Convert.ToDateTime(Session["ClosingDate"]))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date And ToDate Must Be In Between Financial Year! ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else if (Date1 > Date2)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "From Date Must Be Equal or Smaller Than ToDate";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                }
            }
            else
            {
                DateTime From1 = Convert.ToDateTime(Session["OpeningDate"]);
                DateTime To2 = Convert.ToDateTime(Session["ClosingDate"]);
                From = From1.ToShortDateString();
                To = To2.ToShortDateString();
            }
            if (!chkSupplierAll.Checked)
            {
                if (ddlSupplierName.SelectedIndex != 0)
                {
                    Query = Query + "and P_CODE='" + ddlSupplierName.SelectedValue + "'";
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please, Select Supplier Name";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            if (chkFormToAll.Checked)
            {
                Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
            }
            else
            {
                Query = Query + " and SPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
            }
            if (rbtGroup.SelectedValue == "0")
            {
                Query = Query + " and SPOM_IS_TRANSFER='0' ";
            }
            else
            {
                Query = Query + " and SPOM_IS_TRANSFER='1' ";
            }
            Query = Query + " AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " ) select * from RESULT where rn=1 order by SPOM_PO_NO DESC ";
             //dt = CommonClasses.Execute("select SPOM_AMOUNT, P_NAME,SPOM_CODE,SPOM_PO_NO,convert(varchar,SPOM_DATE,106) as SPOM_DATE,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER ,PARTY_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 and SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by SPOM_PO_NO DESC");
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                dgDetailSupplierPO.Enabled = true;
                dgDetailSupplierPO.DataSource = dt;
                dgDetailSupplierPO.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgDetailSupplierPO.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_ORDER_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_INW_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_POST", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailSupplierPO.DataSource = dtFilter;
                    dgDetailSupplierPO.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Transaction", "LoadSupplierPO", Ex.Message);
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
                Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Supplier PO Transaction", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Supplier PO Transaction", "txtString_TextChanged", Ex.Message);
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
                //(upper(SPOM_PO_NO) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(I_NAME) like upper('%" + str + "%')  OR convert(varchar,SPOM_DATE,106) like upper('%" + str + "%') or upper(SPOD_ORDER_QTY) like upper('%" + str + "%') or upper(SPOD_INW_QTY) like upper('%" + str + "%'))
               // dtfilter = CommonClasses.Execute("select cast(SPOM_AMOUNT as numeric(10,2)) as SPOM_AMOUNT, P_NAME,SPOM_CODE,SPOM_PO_NO,convert(varchar,SPOM_DATE,106) as SPOM_DATE,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER ,PARTY_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and (upper(SPOM_PO_NO) like upper('%" + str + "%') OR convert(varchar,SPOM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR SPOM_AMOUNT like '%" + str + "%' ) order by SPOM_PO_NO DESC");
                dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE  and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and (upper(SPOM_PO_NO) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(I_NAME) like upper('%" + str + "%')  OR convert(varchar,SPOM_DATE,106) like upper('%" + str + "%') or upper(SPOD_ORDER_QTY) like upper('%" + str + "%') or upper(SPOD_INW_QTY) like upper('%" + str + "%'))) select * from RESULT where rn=1 order by SPOM_PO_NO DESC");

            else
                //dtfilter = CommonClasses.Execute("select cast(SPOM_AMOUNT as numeric(20,2)) as SPOM_AMOUNT, P_NAME,SPOM_CODE,SPOM_PO_NO,convert(varchar,SPOM_DATE,106) as SPOM_DATE,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER ,PARTY_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " order by SPOM_PO_NO DESC");
                dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE  and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + ") select * from RESULT where rn=1 order by SPOM_PO_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgDetailSupplierPO.Enabled = true;
                dgDetailSupplierPO.DataSource = dtfilter;
                dgDetailSupplierPO.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dgDetailSupplierPO.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_PO_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_ORDER_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOD_INW_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_POST", typeof(String)));


                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailSupplierPO.DataSource = dtFilter;
                    dgDetailSupplierPO.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Suppiler PO Transaction", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region dgDetailSupplierPO_RowEditing
    protected void dgDetailSupplierPO_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            string cpom_code = ((Label)(dgDetailSupplierPO.Rows[e.NewEditIndex].FindControl("lblCPOM_CODE"))).Text;
            string type = "MODIFY";


            Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Po Transactoin", "dgDetailSupplierPO_RowEditing", Ex.Message);
        }
    } 
    #endregion

    #region dgDetailSupplierPO_RowDeleting
    protected void dgDetailSupplierPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {


        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgDetailSupplierPO.Rows[e.RowIndex].FindControl("lblCPOM_CODE"))).Text))
                {
                    string cpom_code = ((Label)(dgDetailSupplierPO.Rows[e.RowIndex].FindControl("lblCPOM_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", "and IWD_IWM_CODE=IWM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Inward";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else
                    {
                        DataTable dt = CommonClasses.Execute("select isnull(SPOM_POST,0) as SPOM_POST,isnull(SPOM_P_AMEND_CODE,0) as SPOM_P_AMEND_CODE from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            //if (Convert.ToInt32(dt.Rows[0]["SPOM_P_AMEND_CODE"]) != 0)
                            //{
                            //    PanelMsg.Visible = true;
                            //    lblmsg.Text = "You Can't Delete Because These PO Is Amended";
                            //    return;
                            //}
                            if (Convert.ToBoolean(dt.Rows[0]["SPOM_POST"].ToString()) == true)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You Can't Delete, Because This PO Is Posted";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                return;
                            }

                            else
                            {
                                bool flag = CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET ES_DELETE = 1 WHERE SPOM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                                if (flag == true)
                                {
                                    CommonClasses.WriteLog("Supplier Purchase Order ", "Delete", "Supplier Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "Record deleted successfully";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


                                }
                            }
                        }
                    }
                    LoadSupplierPO();
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
            CommonClasses.SendError("Supplier PO Transaction", "dgDetailPO_RowDeleting", Ex.Message);
        }



    } 
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from SUPP_PO_MASTER where SPOM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Supplier PO Master-View", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region dgDetailSupplierPO_RowCommand
    protected void dgDetailSupplierPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "VIEW";
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            #endregion

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string cpom_code = e.CommandArgument.ToString();
                        DataTable dt = CommonClasses.Execute("select isnull(SPOM_POST,0) as SPOM_POST,isnull(SPOM_P_AMEND_CODE,0) as SPOM_P_AMEND_CODE  from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["SPOM_POST"].ToString()) == true)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "This PO is posted and can not be modified";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                return;
                            }
                            //else if (Convert.ToInt32(dt.Rows[0]["SPOM_P_AMEND_CODE"]) != 0)
                            //{
                            //    PanelMsg.Visible = true;
                            //    lblmsg.Text = "You Can't Modify Because These PO Is Amend";
                            //    return;
                            //}
                            else
                            {
                                if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", "AND IWD_CPOM_CODE=SPOM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                                {
                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "You can't delete this record, it is used in Inward Master";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                    return;
                                }
                                else
                                {
                                    Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

                                }
                            }
                        }
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
            #endregion

            #region Print
		 else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        

                        string cpom_code = e.CommandArgument.ToString();
                        string PoType = "";
                        DataTable dtAuth = CommonClasses.Execute("Select SPOM_AUTHR_FLAG,SPOM_TYPE,PO_T_SHORT_NAME from SUPP_PO_MASTER,PO_TYPE_MASTER  where  PO_T_CODE=SPOM_TYPE and SPOM_CODE='" + cpom_code + "'");
                        string AuthoFlag = "";
                        if (dtAuth.Rows[0]["SPOM_AUTHR_FLAG"].ToString() == "False")
                        {
                            AuthoFlag = "true";
                        }
                        //if (dtAuth.Rows[0]["SPOM_TYPE"].ToString() == "-2147483647")
                        if (dtAuth.Rows[0]["PO_T_SHORT_NAME"].ToString().Contains("IMPORT"))
                        {
                            PoType = "Export PO";
                        }
                        else
                        {
                            PoType = "Domestic PO";
                        }
                        Response.Redirect("~/RoportForms/ADD/SupplierOrderPrint.aspx?cpom_code=" + cpom_code + "&AuthoType=" + AuthoFlag + "&PoType=" + PoType, false);
                    }
                    //else
                    //{
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "Record used by another person";

                    //    return;
                    //}
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
 
                    return;
                }
            }
 
	#endregion  

            #region Authorize
		else if (e.CommandName.Equals("Authorize"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string qe_code = ((Label)dgDetailSupplierPO.Rows[index].FindControl("lblCPOM_CODE")).Text;

                CommonClasses.Execute("update SUPP_PO_MASTER set SPOM_AUTHR_FLAG=1 where SPOM_CODE=" + qe_code + "");

            }
 
	#endregion           

            #region Post
            else if (e.CommandName.Equals("Post"))
            {

                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    string cpom_code = e.CommandArgument.ToString();
                    if (CommonClasses.Execute1("update SUPP_PO_MASTER set SPOM_POST=1 where SPOM_CODE='" + cpom_code + "'"))
                    {
                        LoadSupplierPO();
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Posted";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Posted";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                }
                //else
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "Record used by another person";
                //    return;
                //}

            } 
            #endregion

            #region Amend
            else if (e.CommandName.Equals("Amend"))
            {
                //string cpom_code = e.CommandArgument.ToString();
                //DataTable dt = CommonClasses.Execute("select isnull(SPOM_POST,0) as SPOM_POST  from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code + "' ");
                //if (dt.Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dt.Rows[0]["SPOM_POST"].ToString()) == false)
                //    {
                //        PanelMsg.Visible = true;
                //        lblmsg.Text = "First Post This PO";
                //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //        return;
                //    }
                //    else
                //    {
                //        if (!ModifyLog(e.CommandArgument.ToString()))
                //        {
                //            string type = "AMEND";
                //            Session["AMEND"] = "AMEND";
                //            Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                //        }
                //        //else
                //        //{
                //        //    PanelMsg.Visible = true;
                //        //    lblmsg.Text = "Record used by another person";
                //        //    return;
                //        //}
                //    }
                //}

                string cpom_code = e.CommandArgument.ToString();
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    DataTable dt = CommonClasses.Execute("select SPOD_SPOM_CODE,SPOM_IS_SHORT_CLOSE, sum(SPOD_ORDER_QTY) AS SPOD_ORDER_QTY,sum(SPOD_INW_QTY) AS SPOD_INW_QTY FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE='" + cpom_code + "' AND SPOM_POST=1 Group BY SPOD_SPOM_CODE,SPOM_IS_SHORT_CLOSE");
                    if (dt.Rows.Count > 0)
                    {
                        double ORDER_QTY = Convert.ToDouble(dt.Rows[0]["SPOD_ORDER_QTY"].ToString());
                        double INW_QTY = Convert.ToDouble(dt.Rows[0]["SPOD_INW_QTY"].ToString());
                        string IS_SHORT_CLOSE = dt.Rows[0]["SPOM_IS_SHORT_CLOSE"].ToString();
                        //if (ORDER_QTY - INW_QTY == 0 || INW_QTY == 0 || IS_SHORT_CLOSE == "True")
                        if (IS_SHORT_CLOSE == "True" || INW_QTY == 0)
                        {
                            string type1 = "AMEND";
                            Session["AMEND"] = "AMEND";
                            Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code, false);
                            return;
                        }
                        else if (INW_QTY > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Couldn't  Amend, This is Open Purchase order.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }


                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "First Post This PO";
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
            #endregion

            #region ShortClose
            else if (e.CommandName.Equals("ShortClose"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    string cpom_code = e.CommandArgument.ToString();
                    DataTable dt = CommonClasses.Execute("select isnull(SPOM_IS_SHORT_CLOSE,0) as SPOM_IS_SHORT_CLOSE,SPOM_POST  from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code + "' AND SPOM_IS_SHORT_CLOSE=0");
                    if (dt.Rows.Count > 0)
                    {
                        if ((dt.Rows[0]["SPOM_POST"]).ToString() == "True")
                        {
                            DataTable DtInw = CommonClasses.Execute("SELECT MAX (SPOD_INW_QTY) as SPOD_INW_QTY FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE='" + cpom_code + "'");
                            double INW_QTY = Convert.ToDouble(DtInw.Rows[0]["SPOD_INW_QTY"].ToString());
                            if (INW_QTY > 0)
                            {
                                bool Res = CommonClasses.Execute1("UPDATE SUPP_PO_MASTER SET SPOM_IS_SHORT_CLOSE=1 WHERE SPOM_CODE='" + cpom_code + "'");
                                if (Res == true)
                                {
                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "Short Close Successfully";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                    return;
                                }

                            }
                            else
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Can Not Short Close Pending for Inward";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;
                            }

                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "First Post This PO";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;

                        }
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Allready Short Closed";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            } 
            #endregion

            #region CancelPO
            else if (e.CommandName.Equals("CancelPO"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    string cpom_code = e.CommandArgument.ToString();
                    if (CommonClasses.CheckUsedInTran("INWARD_MASTER,INWARD_DETAIL", "IWD_CPOM_CODE", "AND IWD_CPOM_CODE=SPOM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can't Cancel This PO, It Is Used In Inward Master";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                    else
                    {
                        if (CommonClasses.Execute1("update SUPP_PO_MASTER set SPOM_CANCEL_PO=1 where SPOM_CODE='" + cpom_code + "'"))
                        {
                            LoadSupplierPO();
                            PanelMsg.Visible = true;
                            lblmsg.Text = "PO Canceled";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "PO Canceled";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                    }
                }
            }
             #endregion


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Po Transaction", "dgDetailSupplierPO_RowCommand", Ex.Message);
        }
    }
    #endregion 

    #region dgDetailSupplierPO_PageIndexChanging
    protected void dgDetailSupplierPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailSupplierPO.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion 

    #region chkSupplierAll_CheckedChanged
    protected void chkSupplierAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSupplierAll.Checked == true)
            {
                ddlSupplierName.Enabled = false;
            }
            else
            {
                ddlSupplierName.Enabled = true;
                return;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("PO Transfer", "chkSupplierAll_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region chkFormToAll_CheckedChanged
    protected void chkFormToAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkFormToAll.Checked == true)
            {
                txtFormDate.Enabled = false;
                txtToDate.Enabled = false;
            }
            else
            {
                txtFormDate.Enabled = true;
                txtToDate.Enabled = true;
                txtFormDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                return;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("PO Transfer", "chkFormToAll_CheckedChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            LoadSupplierPO();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("PO Transfer", "btnShow_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec(string code)
    {
        bool result = false;         
        DL_DBAccess = new DatabaseAccessLayer();
        string msg = "";
        try
        {
            int SPOM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            string CM_CODE = code.ToString();
            for (int i = 0; i < dgDetailSupplierPO.Rows.Count; i++)
            {
                int SPOM_CODE;                
                CheckBox chkRow = (((CheckBox)(dgDetailSupplierPO.Rows[i].FindControl("chkSelect"))) as CheckBox);
                if (chkRow.Checked)
                {
                    SPOM_CODE = Convert.ToInt32(((Label)dgDetailSupplierPO.Rows[i].FindControl("lblCPOM_CODE")).Text);                   
                 
                    SqlParameter[] Para =               
                    	  {
				                new SqlParameter("@SPOM_CODE",SPOM_CODE),
				                new SqlParameter("@SPOM_CM_CODE",SPOM_CM_CODE),
                                 new SqlParameter("@CM_CODE",CM_CODE),
                                new SqlParameter("@PK_CODE", DBNull.Value)
			                };
                    result = DL_DBAccess.Insertion_Updation_Delete("prPoTransfer", Para, out message);
                    msg = message;
                }
            }
            if (msg == "PO Transfer Successfully Saved")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = msg.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
              
                LoadSupplierPO();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("PO Transfer", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {        

        if (dgDetailSupplierPO.Rows.Count != 0)
        {
            string NextYearCode = "";
            DataTable dt = CommonClasses.Execute("select * from COMPANY_MASTER where  CM_OPENING_DATE=DATEADD(DAY,1,'" + Convert.ToDateTime(Session["ClosingDate"]) + "') and CM_ID='"+Session["CompanyId"]+"' and CM_DELETE_FLAG='0' ");
            if (dt.Rows.Count > 0)
            {
                NextYearCode = Convert.ToString(dt.Rows[0]["CM_CODE"]);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "New Financial Year Not Created";              
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (dgDetailSupplierPO.Enabled && dgDetailSupplierPO.Rows.Count > 0)
            {
                SaveRec(NextYearCode);
            }
            else
            {
                ShowMessage("#Avisos", "Records Not Founds", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Record Not Exist In Grid View";
                return;
            }


        }
        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);

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
            CommonClasses.SendError("PO Transfer", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
