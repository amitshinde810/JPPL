using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_mainStoreStockTransfer : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='149'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadIssue();
                    if (dgIssueToProduction.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_TYPE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_MATERIAL_REQ", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_ISSUEBY", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_REQBY", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgIssueToProduction.DataSource = dtFilter;
                            dgIssueToProduction.DataBind();
                            dgIssueToProduction.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "Page_Load", Ex.Message);
        }
    }

    #region LoadIssue
    private void LoadIssue()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT DISTINCT TOP 500 ISSUE_MASTER.IM_CODE, ISSUE_MASTER.IM_MATERIAL_REQ, ISNULL(STORE_MASTER.STORE_PREFIX, '') + ' ' + CONVERT(VARCHAR, ISSUE_MASTER.IM_NO) AS IM_NO, CONVERT(varchar, ISSUE_MASTER.IM_DATE, 106) AS IM_DATE, (CASE WHEN IM_TYPE = 1 THEN 'As Per Material Req' ELSE 'Direct' END) AS IM_TYPE, '' AS MR_BATCH_NO,ISSUE_MASTER.IM_ISSUEBY, STORE_MASTER.STORE_NAME AS IM_REQBY, STORE_MASTER.STORE_NAME, STORE_MASTER_1.STORE_NAME AS TO_STORE, STORE_MASTER_1.STORE_PREFIX AS TO_STORE_PREFIX  FROM ISSUE_MASTER_DETAIL INNER JOIN STORE_MASTER AS STORE_MASTER_1 ON ISSUE_MASTER_DETAIL.IMD_To_STORE = STORE_MASTER_1.STORE_CODE INNER JOIN ISSUE_MASTER INNER JOIN STORE_MASTER ON ISSUE_MASTER.IM_FROM_STORE = STORE_MASTER.STORE_CODE ON ISSUE_MASTER_DETAIL.IM_CODE = ISSUE_MASTER.IM_CODE  WHERE     (ISSUE_MASTER.ES_DELETE = 0) AND (ISSUE_MASTER.IM_TRANS_TYPE = 1)  AND (ISSUE_MASTER.IM_COMP_ID = " + (string)Session["CompanyCode"] + ")  AND STORE_MASTER.STORE_CODE IN (-2147483634,-2147483647)  AND STORE_MASTER_1.STORE_CODE IN (-2147483634,-2147483647) ORDER BY ISSUE_MASTER.IM_CODE DESC");
            dgIssueToProduction.DataSource = dt;
            dgIssueToProduction.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadSupplierPO", Ex.Message);
        }
    }
    #endregion

    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Productionn", "txtString_TextChanged", Ex.Message);
        }
    }
    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")

                dtfilter = CommonClasses.Execute("SELECT DISTINCT TOP 500 ISSUE_MASTER.IM_CODE,ISSUE_MASTER.IM_NO AS NO1,ISSUE_MASTER.IM_MATERIAL_REQ, ISNULL(STORE_MASTER.STORE_PREFIX, '') + ' ' + CONVERT(VARCHAR, ISSUE_MASTER.IM_NO) AS IM_NO, CONVERT(varchar, ISSUE_MASTER.IM_DATE, 106) AS IM_DATE, (CASE WHEN IM_TYPE = 1 THEN 'As Per Material Req' ELSE 'Direct' END) AS IM_TYPE, '' AS MR_BATCH_NO,ISSUE_MASTER.IM_ISSUEBY, STORE_MASTER.STORE_NAME AS IM_REQBY, STORE_MASTER.STORE_NAME, STORE_MASTER_1.STORE_NAME AS TO_STORE, STORE_MASTER_1.STORE_PREFIX AS TO_STORE_PREFIX  FROM ISSUE_MASTER_DETAIL INNER JOIN STORE_MASTER AS STORE_MASTER_1 ON ISSUE_MASTER_DETAIL.IMD_To_STORE = STORE_MASTER_1.STORE_CODE INNER JOIN ISSUE_MASTER INNER JOIN STORE_MASTER ON ISSUE_MASTER.IM_FROM_STORE = STORE_MASTER.STORE_CODE ON ISSUE_MASTER_DETAIL.IM_CODE = ISSUE_MASTER.IM_CODE WHERE (ISSUE_MASTER.ES_DELETE = 0) AND (ISSUE_MASTER.IM_TRANS_TYPE = 1)  and IM_COMP_ID=" + Session["CompanyCode"] + " and (upper(ISNULL(STORE_MASTER.STORE_PREFIX,'') + CONVERT(VARCHAR,ISSUE_MASTER.IM_NO)) like upper('%" + str + "%') OR convert(varchar,IM_DATE,106) like upper('%" + str + "%') OR upper(IM_TYPE) like upper('%" + str + "%')   OR STORE_MASTER.STORE_NAME like '%" + str + "%'  OR STORE_MASTER_1.STORE_NAME like '%" + str + "%') order by ISSUE_MASTER.IM_NO DESC");
            else
                //dtfilter = CommonClasses.Execute("SELECT DISTINCT TOP 500 ISSUE_MASTER.IM_CODE,ISSUE_MASTER.IM_NO AS NO1,ISSUE_MASTER.IM_MATERIAL_REQ, ISNULL(STORE_MASTER.STORE_PREFIX, '') + ' ' + CONVERT(VARCHAR, ISSUE_MASTER.IM_NO) AS IM_NO, CONVERT(varchar, ISSUE_MASTER.IM_DATE, 106) AS IM_DATE, (CASE WHEN IM_TYPE = 1 THEN 'As Per Material Req' ELSE 'Direct' END) AS IM_TYPE, '' AS MR_BATCH_NO,ISSUE_MASTER.IM_ISSUEBY,  STORE_MASTER.STORE_NAME AS IM_REQBY, STORE_MASTER.STORE_NAME, STORE_MASTER_1.STORE_NAME AS TO_STORE, STORE_MASTER_1.STORE_PREFIX AS TO_STORE_PREFIX  FROM ISSUE_MASTER_DETAIL INNER JOIN STORE_MASTER AS STORE_MASTER_1 ON ISSUE_MASTER_DETAIL.IMD_To_STORE = STORE_MASTER_1.STORE_CODE INNER JOIN ISSUE_MASTER INNER JOIN STORE_MASTER ON ISSUE_MASTER.IM_FROM_STORE = STORE_MASTER.STORE_CODE ON ISSUE_MASTER_DETAIL.IM_CODE = ISSUE_MASTER.IM_CODE WHERE (ISSUE_MASTER.ES_DELETE = 0) AND (ISSUE_MASTER.IM_TRANS_TYPE = 1)  and IM_COMP_ID=" + Session["CompanyCode"] + " order by ISSUE_MASTER.IM_NO DESC");
                dtfilter = CommonClasses.Execute("SELECT DISTINCT TOP 500 ISSUE_MASTER.IM_CODE, ISSUE_MASTER.IM_MATERIAL_REQ, ISNULL(STORE_MASTER.STORE_PREFIX, '') + ' ' + CONVERT(VARCHAR, ISSUE_MASTER.IM_NO) AS IM_NO, CONVERT(varchar, ISSUE_MASTER.IM_DATE, 106) AS IM_DATE, (CASE WHEN IM_TYPE = 1 THEN 'As Per Material Req' ELSE 'Direct' END) AS IM_TYPE, '' AS MR_BATCH_NO,ISSUE_MASTER.IM_ISSUEBY, STORE_MASTER.STORE_NAME AS IM_REQBY, STORE_MASTER.STORE_NAME, STORE_MASTER_1.STORE_NAME AS TO_STORE, STORE_MASTER_1.STORE_PREFIX AS TO_STORE_PREFIX  FROM ISSUE_MASTER_DETAIL INNER JOIN STORE_MASTER AS STORE_MASTER_1 ON ISSUE_MASTER_DETAIL.IMD_To_STORE = STORE_MASTER_1.STORE_CODE INNER JOIN ISSUE_MASTER INNER JOIN STORE_MASTER ON ISSUE_MASTER.IM_FROM_STORE = STORE_MASTER.STORE_CODE ON ISSUE_MASTER_DETAIL.IM_CODE = ISSUE_MASTER.IM_CODE  WHERE     (ISSUE_MASTER.ES_DELETE = 0) AND (ISSUE_MASTER.IM_TRANS_TYPE = 1)  AND (ISSUE_MASTER.IM_COMP_ID = " + (string)Session["CompanyCode"] + ") ORDER BY ISSUE_MASTER.IM_CODE DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgIssueToProduction.DataSource = dtfilter;
                dgIssueToProduction.DataBind();
                dgIssueToProduction.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_MATERIAL_REQ", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_ISSUEBY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_REQBY", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgIssueToProduction.DataSource = dtFilter;
                    dgIssueToProduction.DataBind();
                    dgIssueToProduction.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadStatus", ex.Message);
        }
    }

    #endregion

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/mainStoreStockTransfer.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Issue To Production", "btnAddNew_Click", Ex.Message);
        }
    }

    protected void dgIssueToProduction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgIssueToProduction.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }

    #region dgIssueToProduction_RowCommand
    protected void dgIssueToProduction_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Response.Redirect("~/Transactions/ADD/mainStoreStockTransfer.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            #endregion View

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();

                        DataTable DtDetails = CommonClasses.Execute("select ISNULL(IM_DIRECT_POST,0) AS IM_DIRECT_POST ,IMD_I_CODE,IMD_ISSUE_QTY,I_CURRENT_BAL,I_CODENO,ISNULL(IMD_STORE_TYPE,0) AS IMD_STORE_TYPE from ISSUE_MASTER_DETAIL,ITEM_MASTER,ISSUE_MASTER WHERE IMD_I_CODE=I_CODE   AND ISSUE_MASTER_DETAIL.IM_CODE=ISSUE_MASTER.IM_CODE AND  ISSUE_MASTER_DETAIL.IM_CODE='" + um_code + "'");

                        if (DtDetails.Rows[0]["IM_DIRECT_POST"].ToString().ToUpper() == "1" || DtDetails.Rows[0]["IM_DIRECT_POST"].ToString().ToUpper() == "TRUE")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Modify ,This is Short Qty Return Entry Posted Directly.  ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        for (int i = 0; i < DtDetails.Rows.Count; i++)
                        {
                            if (DtDetails.Rows[i]["IMD_STORE_TYPE"].ToString().ToUpper() == "1" || DtDetails.Rows[i]["IMD_STORE_TYPE"].ToString().ToUpper() == "TRUE")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You Can Not Modify ,Used In Other Transaction ";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;
                            }
                        }
                        Response.Redirect("~/Transactions/ADD/mainStoreStockTransfer.aspx?c_name=" + type + "&u_code=" + um_code, false);
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
            #endregion Modify

            #region Print
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "STORE";
                        string MatReq_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/RoportForms/ADD/StoreToStorePrint.aspx?MatReq_Code=" + MatReq_Code + "&print_type=" + type, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion Print

            #region PrintMultiStore
            else if (e.CommandName.Equals("PrintMultiStore"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "PrintMultiStore";
                        string MatReq_Code = e.CommandArgument.ToString();
                        Response.Redirect("~/RoportForms/ADD/StoreToStorePrint.aspx?MatReq_Code=" + MatReq_Code + "&print_type=" + type, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion PrintMultiStore
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "dgDetailSupplierPO_RowCommand", Ex.Message);
        }
    }
    #endregion dgIssueToProduction_RowCommand

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "btnClose_Click", ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from ISSUE_MASTER where IM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
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
    protected void dgIssueToProduction_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgIssueToProduction.Rows[e.RowIndex].FindControl("lblIM_CODE"))).Text))
                {
                    string um_code = ((Label)(dgIssueToProduction.Rows[e.RowIndex].FindControl("lblIM_CODE"))).Text;
                    string um_name = ((Label)(dgIssueToProduction.Rows[e.RowIndex].FindControl("lblIM_NO"))).Text;

                    DataTable DtDetails = CommonClasses.Execute("select ISNULL(IM_DIRECT_POST,0) AS IM_DIRECT_POST, IMD_I_CODE,IMD_ISSUE_QTY,I_CURRENT_BAL,I_CODENO,ISNULL(IMD_STORE_TYPE,0) AS IMD_STORE_TYPE from ISSUE_MASTER_DETAIL,ITEM_MASTER,ISSUE_MASTER WHERE IMD_I_CODE=I_CODE AND ISSUE_MASTER_DETAIL.IM_CODE=ISSUE_MASTER.IM_CODE AND   ISSUE_MASTER_DETAIL.IM_CODE='" + um_code + "'");

                    if (DtDetails.Rows[0]["IM_DIRECT_POST"].ToString().ToUpper() == "1" || DtDetails.Rows[0]["IM_DIRECT_POST"].ToString().ToUpper() == "TRUE")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can Not Delete ,This is Short Qty Return Entry Posted Directly.  ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    for (int i = 0; i < DtDetails.Rows.Count; i++)
                    {
                        if ((Convert.ToDouble(DtDetails.Rows[i]["I_CURRENT_BAL"].ToString()) - Convert.ToDouble(DtDetails.Rows[i]["IMD_ISSUE_QTY"].ToString()) < 0))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Delete " + DtDetails.Rows[i]["I_CODENO"].ToString() + " Stock Used In Other Transaction ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        if (DtDetails.Rows[i]["IMD_STORE_TYPE"].ToString().ToUpper() == "1" || DtDetails.Rows[i]["IMD_STORE_TYPE"].ToString().ToUpper() == "TRUE")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Delete ,Used In Other Transaction ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    bool flag = CommonClasses.Execute1("UPDATE ISSUE_MASTER SET ES_DELETE = 1 WHERE IM_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        DataTable DtOldDetails = CommonClasses.Execute("select IMD_I_CODE,IMD_ISSUE_QTY from ISSUE_MASTER_DETAIL WHERE IM_CODE='" + um_code + "'");
                        DataTable DtREQRef = CommonClasses.Execute("select IM_MATERIAL_REQ ,IM_FROM_STORE  from ISSUE_MASTER WHERE IM_CODE='" + um_code + "'");

                        //---- Reseting Item Master Stock
                        for (int n = 0; n < DtOldDetails.Rows.Count; n++)
                        {
                            if (DtREQRef.Rows[0]["IM_FROM_STORE"].ToString() == "-2147483642")
                            {
                                CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + DtOldDetails.Rows[n]["IMD_ISSUE_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["IMD_I_CODE"] + "'");
                            }
                            else
                            {
                                CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + DtOldDetails.Rows[n]["IMD_ISSUE_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["IMD_I_CODE"] + "'");
                            }
                        }

                        flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + um_code + "' and STL_DOC_TYPE LIKE 'ISS%'");

                        CommonClasses.WriteLog("Issue To Production", "Delete", "Issue To Production", um_name, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadIssue();
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("User Master", "GridView1_RowEditing", exc.Message);
        }
    }

    protected void dgIssueToProduction_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
}
