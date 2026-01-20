using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Transactions_VIEW_ViewSupplierPurchaseOrder : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    static string Code = "";
    static string type = "";

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
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_AUTHR_FLAG", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CANCEL_PO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_IS_SHORT_CLOSE", typeof(String)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgDetailSupplierPO.DataSource = dtFilter;
                        dgDetailSupplierPO.DataBind();
                    }
                    LoadSupplierPO();
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




    #region GetData
    public DataTable GetData()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PONO AS SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST , case when SPOM_AUTHR_FLAG =1 then 'Yes' else 'No' end as  SPOM_AUTHR_FLAG ,SPOM_CANCEL_PO,SPOM_IS_SHORT_CLOSE  from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE  and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_POTYPE=0 ) select   * from RESULT where rn=1 order by SPOM_PO_NO DESC");

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Transaction", "LoadSupplierPO", Ex.Message);
        }
        return dt;
    }
    #endregion

    #region LoadSupplierPO
    private void LoadSupplierPO()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PONO AS SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST , case when SPOM_AUTHR_FLAG =1 then 'Yes' else 'No' end as  SPOM_AUTHR_FLAG ,SPOM_CANCEL_PO,SPOM_IS_SHORT_CLOSE  from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE  and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_POTYPE=0 ) select   * from RESULT where rn=1 order by SPOM_PO_NO DESC");
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
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_AUTHR_FLAG", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CANCEL_PO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_IS_SHORT_CLOSE", typeof(String)));
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

    #region Blank Grid
    private void BlankGrid()
    {
       
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
            str = txtString.Text.Trim().Replace("'", "\''");
            DataTable dtfilter = new DataTable();
            if (txtString.Text.Trim() != "")
                dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PONO AS SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST  , case when SPOM_AUTHR_FLAG =1 then 'Yes' else 'No' end as  SPOM_AUTHR_FLAG ,SPOM_CANCEL_PO,SPOM_IS_SHORT_CLOSE from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + "  AND SPOM_POTYPE=0) select   * from RESULT where rn=1 and (upper(SPOM_PO_NO) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(I_NAME) like upper('%" + str + "%') OR convert(varchar,SPOM_DATE,106) like upper('%" + str + "%')) order by SPOM_PO_NO DESC");
            //dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE,SPOM_PONO AS SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST  , case when SPOM_AUTHR_FLAG =1 then 'Yes' else 'No' end as  SPOM_AUTHR_FLAG  from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + "  AND SPOM_POTYPE=0 and (upper(SPOM_PO_NO) like upper('%" + str + "%') or upper(P_NAME) like upper('%" + str + "%') or upper(I_NAME) like upper('%" + str + "%')  OR convert(varchar,SPOM_DATE,106) like upper('%" + str + "%') or upper(SPOD_ORDER_QTY) like upper('%" + str + "%') or upper(SPOD_INW_QTY) like upper('%" + str + "%'))) select TOP 500 * from RESULT where rn=1 order by SPOM_PO_NO DESC");
            else
                dtfilter = CommonClasses.Execute("With RESULT as( select ROW_NUMBER() OVER (PARTITION BY SPOM_CODE ORDER BY SPOM_CODE) AS rn,SPOM_CODE, SPOM_PONO AS SPOM_PO_NO,P_NAME,SPOD_I_CODE,I_NAME, convert(varchar,SPOM_DATE,106) as SPOM_DATE,cast(SPOD_ORDER_QTY as numeric(20,3)) as SPOD_ORDER_QTY,cast(SPOD_ORDER_QTY-SPOD_INW_QTY as numeric(20,3)) as SPOD_INW_QTY,case when SPOM_POST =1 then 'Yes' else 'No' end as SPOM_POST , case when SPOM_AUTHR_FLAG =1 then 'Yes' else 'No' end as  SPOM_AUTHR_FLAG,SPOM_CANCEL_PO,SPOM_IS_SHORT_CLOSE from SUPP_PO_MASTER,PARTY_MASTER,SUPP_PO_DETAILS,ITEM_MASTER where SUPP_PO_MASTER.ES_DELETE=0 and SPOM_P_CODE=P_CODE and P_TYPE=2 and SPOM_CODE=SPOD_SPOM_CODE and SPOD_I_CODE=I_CODE and P_ACTIVE_IND=1 AND SPOM_CM_CODE= " + Convert.ToInt32(Session["CompanyCode"]) + " and  SPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPOM_POTYPE=0 ) select   * from RESULT where rn=1 order by SPOM_PO_NO DESC");

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
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_AUTHR_FLAG", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CANCEL_PO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_IS_SHORT_CLOSE", typeof(String)));
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
                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                        DataTable dt = CommonClasses.Execute("select ISNULL(SPOM_CANCEL_PO,0) AS SPOM_CANCEL_PO,isnull(SPOM_POST,0) as SPOM_POST,isnull(SPOM_P_AMEND_CODE,0) as SPOM_P_AMEND_CODE  from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["SPOM_POST"].ToString()) == true)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "This PO is posted and can not be modified";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                return;
                            }
                            else if (Convert.ToBoolean(dt.Rows[0]["SPOM_CANCEL_PO"].ToString()) == true)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You can't Modify this record, it is canceled ";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                                return;
                            }
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
                //DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='70'");
                //string right1 = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                //if (CommonClasses.ValidRights(int.Parse(right1.Substring(2, 1)), this, "For Authorize"))
                //{
                string cpom_code = e.CommandArgument.ToString();

                Code = cpom_code;
                type = "Single";
                DataTable dtAutoFlag = CommonClasses.Execute("select SPOM_AUTHR_FLAG from SUPP_PO_MASTER where SPOM_CODE='" + cpom_code + "'");
                if (dtAutoFlag.Rows[0]["SPOM_AUTHR_FLAG"].ToString() == "True")
                {
                    if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                    {
                        if (!ModifyLog(e.CommandArgument.ToString()))
                        {
                            //string cpom_code = e.CommandArgument.ToString();
                            string PoType = "";
                            DataTable dtAuth = CommonClasses.Execute("Select SPOM_AUTHR_FLAG,SPOM_TYPE,PO_T_SHORT_NAME from SUPP_PO_MASTER,PO_TYPE_MASTER  where  PO_T_CODE=SPOM_TYPE and SPOM_CODE='" + cpom_code + "'");
                            string AuthoFlag = "";
                            if (dtAuth.Rows[0]["SPOM_AUTHR_FLAG"].ToString() == "False")
                            {
                                AuthoFlag = "true";
                            }
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
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You have no rights to Print";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Firstly Authorize PO";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                //}
                //else
                //{
                //    PanelMsg.Visible = true;
                //    lblmsg.Text = "You have no rights to Authorize";
                //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    return;   
                //}
            }
            #endregion

            #region Authorize
            else if (e.CommandName.Equals("Authorize"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string qe_code = ((Label)dgDetailSupplierPO.Rows[index].FindControl("lblCPOM_CODE")).Text;
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='70'");
                string right1 = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                if (CommonClasses.ValidRights(int.Parse(right1.Substring(2, 1)), this, "For Authorize"))
                {
                    CommonClasses.Execute("update SUPP_PO_MASTER set SPOM_AUTHR_FLAG=1 where SPOM_CODE=" + qe_code + "");
                    CommonClasses.WriteLog("Supplier Purchase Order ", "Authorize", "Supplier Purchase Order", qe_code, Convert.ToInt32(qe_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    LoadStatus(txtString);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Authorize";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion

            #region Post
            else if (e.CommandName.Equals("Post"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Post"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='69'");
                        string right1 = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                        if (CommonClasses.ValidRights(int.Parse(right1.Substring(2, 1)), this, "For Post"))
                        {
                            string cpom_code = e.CommandArgument.ToString();
                            if (CommonClasses.Execute1("update SUPP_PO_MASTER set SPOM_POST=1 where SPOM_CODE='" + cpom_code + "'"))
                            {
                                CommonClasses.WriteLog("Supplier Purchase Order ", "Post", "Supplier Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

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
                        else
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You have no rights to Post";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region Amend
            else if (e.CommandName.Equals("Amend"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Amend"))
                {
                    string cpom_code = e.CommandArgument.ToString();
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        DataTable dt = CommonClasses.Execute("select SPOD_SPOM_CODE,SPOM_IS_SHORT_CLOSE,SPOM_CANCEL_PO, sum(SPOD_ORDER_QTY) AS SPOD_ORDER_QTY,sum(SPOD_INW_QTY) AS SPOD_INW_QTY FROM SUPP_PO_MASTER,SUPP_PO_DETAILS WHERE SPOM_CODE=SPOD_SPOM_CODE AND SPOM_CODE='" + cpom_code + "' AND SPOM_POST=1 Group BY SPOD_SPOM_CODE,SPOM_IS_SHORT_CLOSE,SPOM_CANCEL_PO");
                        if (dt.Rows.Count > 0)
                        {
                            double ORDER_QTY = Convert.ToDouble(dt.Rows[0]["SPOD_ORDER_QTY"].ToString());
                            double INW_QTY = Convert.ToDouble(dt.Rows[0]["SPOD_INW_QTY"].ToString());
                            string IS_SHORT_CLOSE = dt.Rows[0]["SPOM_IS_SHORT_CLOSE"].ToString();
                            string SPOM_CANCEL_PO = dt.Rows[0]["SPOM_CANCEL_PO"].ToString();

                            string type1 = "AMEND";
                            Session["AMEND"] = "AMEND";
                            if (SPOM_CANCEL_PO.ToUpper() == "TRUE" || SPOM_CANCEL_PO.ToUpper() == "1")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "PO is Cancel You can not Change It.";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;
                            }
                            else
                            {
                                Response.Redirect("~/Transactions/ADD/SupplierPurchaseOrder.aspx?c_name=" + type1 + "&cpom_code=" + cpom_code, false);
                            }
                            return;
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
            }
            #endregion

            #region ShortClose
            else if (e.CommandName.Equals("ShortClose"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For ShortClose"))
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
                                        CommonClasses.WriteLog("Supplier Purchase Order ", "ShortClose", "Supplier Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
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
            }
            #endregion

            #region CancelPO
            else if (e.CommandName.Equals("CancelPO"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For CancelPO"))
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
                                CommonClasses.WriteLog("Supplier Purchase Order ", "CancelPO", "Supplier Purchase Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

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
    #region ddlFilter_SelectedIndexChanged
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = this.GetData();
        DataView dataView = dt.DefaultView;

        if (ddlFilter.SelectedValue == "4")
        {
            dataView.RowFilter = "SPOM_CANCEL_PO=1 ";
        }
        else if (ddlFilter.SelectedValue == "3")
        {
            dataView.RowFilter = "SPOM_IS_SHORT_CLOSE=1";
        }
        else if (ddlFilter.SelectedValue == "2")
        {
            dataView.RowFilter = "SPOM_IS_SHORT_CLOSE=0 and SPOM_CANCEL_PO=0";
           
        }
       else if (ddlFilter.SelectedValue == "0")
        {
            LoadSupplierPO();
        }
        dgDetailSupplierPO.DataSource = dataView;
        if (dt.DefaultView.Count == 0)
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
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_AUTHR_FLAG", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_CANCEL_PO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("SPOM_IS_SHORT_CLOSE", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgDetailSupplierPO.DataSource = dtFilter;
                dgDetailSupplierPO.DataBind();
            }
        }
        else
        {
            dgDetailSupplierPO.DataBind();
        }      
    }
    #endregion
}
