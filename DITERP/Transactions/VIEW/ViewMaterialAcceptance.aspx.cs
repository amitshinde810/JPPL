using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_VIEW_ViewMaterialAcceptance : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='148'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    if (string.IsNullOrEmpty((string)Session["InspType"]))
                    {
                        Session["InspType"] = "Pending";
                    }
                    dgMaterialAcceptance.Enabled = false;
                    LoadGridData();
                    if (dgMaterialAcceptance.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(String)));

                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("IM_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("FROM_STORE_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("FROM_STORE_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("TRANS_TYPE", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgMaterialAcceptance.DataSource = dtFilter;
                            dgMaterialAcceptance.DataBind();
                        }
                        else
                        {
                            dgMaterialAcceptance.Enabled = true;
                        }
                    }
                    FillCombo();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Acceptance", "btnShow_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/ProductionDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Acceptance", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region GridEvent

    #region dgMaterialAcceptance_RowCommand
    protected void dgMaterialAcceptance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "VIEW";
                        int index = Convert.ToInt32(e.CommandArgument.ToString());
                        string code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIM_CODE"))).Text;
                        string cpom_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIM_NO"))).Text;
                        Response.Redirect("~/Transactions/ADD/MaterialAcceptance.aspx?c_name=" + type + "&cpom_code=" + code, false);
                    }
                }

                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    return;
                }
            }
            #endregion View

            #region Modify
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (ddlToStore.SelectedValue.ToString() == "0")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please select To Store";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    int index = Convert.ToInt32(e.CommandArgument.ToString());
                    string code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIM_CODE"))).Text;
                    string TRANS_TYPE = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblTRANS_TYPE"))).Text;
                    string ToStoreCode = ddlToStore.SelectedValue;
                    string IM_CODE = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIM_CODE"))).Text;
                    if (!ModifyLog(IM_CODE))
                    {
                        string type = "MODIFY";


                        CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=1 where IM_CODE='" + IM_CODE + "'");
                        Response.Redirect("~/Transactions/ADD/MaterialAcceptance.aspx?c_name=" + type + "&IM_CODE=" + code + "&ToStoreCode=" + ToStoreCode + "&Type=" + TRANS_TYPE + "", false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to Add/Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion Modify

            #region Check
            else if (e.CommandName.Equals("Check"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "Check";
                        int index = Convert.ToInt32(e.CommandArgument.ToString());
                        string code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIM_CODE"))).Text;
                        string From_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblFROM_STORE_NAME"))).Text;
                        DataTable dtStock = new DataTable();
                        if (ddlToStore.SelectedValue == "0")
                        {
                            dtStock = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE,ITEM_MASTER.I_CODE,I_CODENO ,I_NAME ,IM_NO ,IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME], IMD_ISSUE_QTY ,0 as OK_Qty, 0 as Rej_Qty ,STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,STORE_MASTER.STORE_NAME +' - '+TOStore.STORE_NAME AS NARR FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND ISSUE_MASTER.IM_CODE= '" + code + "' ");
                        }
                        else
                        {
                            dtStock = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE,ITEM_MASTER.I_CODE,I_CODENO ,I_NAME ,IM_NO ,IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME], IMD_ISSUE_QTY ,0 as OK_Qty, 0 as Rej_Qty ,STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,STORE_MASTER.STORE_NAME +' - '+TOStore.STORE_NAME AS NARR FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND ISSUE_MASTER.IM_CODE= '" + code + "'  AND IMD_To_STORE='" + ddlToStore.SelectedValue + "' ");
                        }
                        if (dtStock.Rows.Count > 0)
                        {
                            string ReceivedQty = "";
                            for (int i = 0; i < dtStock.Rows.Count; i++)
                            {
                                ReceivedQty = dtStock.Rows[i]["IMD_ISSUE_QTY"].ToString();

                                // Stock Ledger :- ToStore record insert 
                                CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE	,STL_DOC_NO	,STL_DOC_NUMBER	,STL_DOC_TYPE	,STL_DOC_DATE	,STL_DOC_QTY ,STL_STORE_TYPE) VALUES('" + dtStock.Rows[i]["I_CODE"].ToString() + "','" + dtStock.Rows[i]["IM_CODE"].ToString() + "','" + dtStock.Rows[i]["IM_NO"].ToString() + "','" + dtStock.Rows[i]["NARR"].ToString() + "','" + dtStock.Rows[i]["IM_DATE"].ToString() + "','" + ReceivedQty.ToString() + "','" + dtStock.Rows[i]["TO_STORE_CODE"].ToString() + "')");

                                // ISSUE_MASTER_DETAIL :- Update IMD_STORE_TYPE = 1 flag 
                                CommonClasses.Execute("UPDATE ISSUE_MASTER_DETAIL SET IMD_STORE_TYPE = 1  WHERE IMD_To_STORE='" + dtStock.Rows[i]["TO_STORE_CODE"].ToString() + "' AND ISSUE_MASTER_DETAIL.IM_CODE = '" + dtStock.Rows[i]["IM_CODE"].ToString() + "' AND IMD_I_CODE='" + dtStock.Rows[i]["I_CODE"].ToString() + "' ");

                                if (dtStock.Rows[i]["TO_STORE_CODE"].ToString() == "-2147483642")
                                {
                                    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DISPATCH_BAL = ISNULL(I_DISPATCH_BAL,0)+'" + ReceivedQty.ToString() + "'  WHERE     I_CODE='" + dtStock.Rows[i]["I_CODE"].ToString() + "' ");
                                }
                                else
                                {
                                    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL = I_CURRENT_BAL+'" + ReceivedQty.ToString() + "'  WHERE     I_CODE='" + dtStock.Rows[i]["I_CODE"].ToString() + "' ");
                                }
                                ddlToStore_SelectedIndexChanged(null, null);
                                LoadGridData();
                            }
                        }
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to View";
                    return;
                }
            }
            #endregion Check
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Acceptance-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgMaterialAcceptance_PageIndexChanging
    protected void dgMaterialAcceptance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgMaterialAcceptance.PageIndex = e.NewPageIndex;
        }
        catch (Exception)
        {
        }
    }
    #endregion
    #endregion

    #region UserDefiendMethod

    #region FillCombo
    private void FillCombo()
    {
        try
        {
            DataTable dtUser = CommonClasses.Execute("SELECT * FROM USER_STORE_OWNER WHERE UM_CODE ='" + Session["UserCode"] + "' ");
            if (dtUser.Rows.Count == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "User have no rights of store...";
                return;
            }
            string Codes = "";
            for (int i = 0; i < dtUser.Rows.Count; i++)
            {
                Codes = Codes + "'" + dtUser.Rows[i]["STORE_CODE"].ToString() + "'" + ",";
            }
            Codes = Codes.TrimEnd(',');
            CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (" + Codes + ") ORDER BY STORE_NAME", ddlToStore);
            ddlToStore.Items.Insert(0, new ListItem("Select To Store Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "FillCombo", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from ISSUE_MASTER where IM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Material Acceptance -View", "ModifyLog", Ex.Message);
        }
        return false;
    }
    #endregion
    #endregion

    #region ddlToStore_SelectedIndexChanged
    public void ddlToStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTostore = new DataTable();
            if (ddlToStore.SelectedValue == "-2147483647")
            {
                dtTostore = CommonClasses.Execute(" SELECT * FROM ( SELECT  DISTINCT ISSUE_MASTER.IM_CODE,'' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO  ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE]  ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE  ISSUE_MASTER.ES_DELETE=0 AND  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1 AND IMD_To_STORE='" + ddlToStore.SelectedValue + "'   UNION  SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store' AS [FROM_STORE_NAME] , 'Main Store' AS [TO_STORE_NAME],'-2147483641'  as [FROM_STORE_CODE], '-2147483647' AS [TO_STORE_CODE] ,1 AS TRANS_TYPE  FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0   AND RTF_STORE_CODE=-2147483647     ) AS temp     where TO_STORE_CODE IN (SELECT STORE_CODE  FROM USER_STORE_OWNER where UM_CODE='" + Session["UserCode"] + "' )        ORDER BY IM_CODE DESC");
            }
            else if (ddlToStore.SelectedValue == "-2147483634")
            {
                dtTostore = CommonClasses.Execute(" SELECT * FROM ( SELECT  DISTINCT ISSUE_MASTER.IM_CODE,'' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO  ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE]  ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE  ISSUE_MASTER.ES_DELETE=0 AND  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1 AND IMD_To_STORE='" + ddlToStore.SelectedValue + "'   UNION  SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store (Plant II)' AS [FROM_STORE_NAME] , 'Main Store (Plant II)' AS [TO_STORE_NAME],'-2147483628'  as [FROM_STORE_CODE], '-2147483634' AS [TO_STORE_CODE] ,1 AS TRANS_TYPE  FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0   AND RTF_STORE_CODE=-2147483647     UNION SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store II' AS [FROM_STORE_NAME] , 'Main Store II' AS [TO_STORE_NAME],'-2147483628'  as [FROM_STORE_CODE], '-2147483634' AS [TO_STORE_CODE]  ,1 AS TRANS_TYPE FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0  AND RTF_STORE_CODE!=-2147483647 ) AS temp     where TO_STORE_CODE IN (SELECT STORE_CODE  FROM USER_STORE_OWNER where UM_CODE='" + Session["UserCode"] + "' )        ORDER BY IM_CODE DESC");
            }
            else
            {
                dtTostore = CommonClasses.Execute("SELECT * FROM ( SELECT  DISTINCT ISSUE_MASTER.IM_CODE,'' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO  ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE  ISSUE_MASTER.ES_DELETE=0 AND  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1 AND IMD_To_STORE='" + ddlToStore.SelectedValue + "'    ) AS temp     where TO_STORE_CODE IN (SELECT STORE_CODE  FROM USER_STORE_OWNER where UM_CODE='" + Session["UserCode"] + "' )        ORDER BY IM_CODE DESC");
            }
            if (dtTostore.Rows.Count > 0)
            {
                dgMaterialAcceptance.DataSource = dtTostore;
                dgMaterialAcceptance.DataBind();
                dgMaterialAcceptance.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FROM_STORE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FROM_STORE_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRANS_TYPE", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMaterialAcceptance.DataSource = dtFilter;
                    dgMaterialAcceptance.DataBind();
                    dgMaterialAcceptance.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    #endregion ddlToStore_SelectedIndexChanged

    #region LoadGridData
    public void LoadGridData()
    {
        try
        {
            //DataTable dtTostore = CommonClasses.Execute("SELECT  DISTINCT ISSUE_MASTER.IM_CODE,'' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER,USER_STORE_DETAIL , STORE_MASTER AS TOStore WHERE ISSUE_MASTER.ES_DELETE=0 AND ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1   AND IMD_To_STORE=USER_STORE_DETAIL.STORE_CODE AND   UM_CODE='" + Session["UserCode"] + "'  UNION  SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store' AS [FROM_STORE_NAME] , 'Main Store' AS [TO_STORE_NAME],'-2147483641'  as [FROM_STORE_CODE], '-2147483647' AS [TO_STORE_CODE]  ,1 AS TRANS_TYPE FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0        ORDER BY IM_CODE DESC ");

            //DataTable dtTostore = CommonClasses.Execute("SELECT * FROM (SELECT  DISTINCT ISSUE_MASTER.IM_CODE,'' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER, STORE_MASTER AS TOStore WHERE ISSUE_MASTER.ES_DELETE=0 AND ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1    UNION  SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store' AS [FROM_STORE_NAME] , 'Main Store' AS [TO_STORE_NAME],'-2147483641'  as [FROM_STORE_CODE], '-2147483647' AS [TO_STORE_CODE]  ,1 AS TRANS_TYPE FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0 ) AS temp     where TO_STORE_CODE IN (SELECT STORE_CODE  FROM USER_STORE_OWNER where UM_CODE='" + Session["UserCode"] + "' )        ORDER BY IM_CODE DESC ");

            DataTable dtTostore = CommonClasses.Execute("SELECT * FROM (SELECT  DISTINCT ISSUE_MASTER.IM_CODE,'' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,STORE_MASTER.STORE_PREFIX+' '+CONVERT(VARCHAR,IM_NO) AS IM_NO ,CONVERT(VARCHAR,IM_DATE,106) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME],STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] ,0 AS TRANS_TYPE FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER, STORE_MASTER AS TOStore WHERE ISSUE_MASTER.ES_DELETE=0 AND ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND IMD_STORE_TYPE = 0  AND IM_TRANS_TYPE=1     UNION  SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store' AS [FROM_STORE_NAME] , 'Main Store' AS [TO_STORE_NAME],'-2147483641'  as [FROM_STORE_CODE], '-2147483647' AS [TO_STORE_CODE]  ,1 AS TRANS_TYPE FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0 AND RTF_STORE_CODE=-2147483647   UNION SELECT RTF_CODE  AS IM_CODE, '' AS I_CODE,'' AS I_CODENO ,'' AS I_NAME ,'CASTING CONVERSION  '+ RTF_DOC_NO  AS IM_NO, CONVERT(VARCHAR,RTF_DOC_DATE,106) AS IM_DATE ,  'Rejection Store II' AS [FROM_STORE_NAME] , 'Main Store II' AS [TO_STORE_NAME],'-2147483628'  as [FROM_STORE_CODE], '-2147483634' AS [TO_STORE_CODE]  ,1 AS TRANS_TYPE FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_ISUSED=0  AND RTF_STORE_CODE!=-2147483647 ) AS temp     where TO_STORE_CODE IN (SELECT STORE_CODE  FROM USER_STORE_OWNER where UM_CODE='" + Session["UserCode"] + "' )        ORDER BY IM_CODE DESC ");

            if (dtTostore.Rows.Count > 0)
            {
                dgMaterialAcceptance.DataSource = dtTostore;
                dgMaterialAcceptance.DataBind();
                dgMaterialAcceptance.Enabled = true;
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FROM_STORE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FROM_STORE_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TO_STORE_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TRANS_TYPE", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMaterialAcceptance.DataSource = dtFilter;
                    dgMaterialAcceptance.DataBind();
                    dgMaterialAcceptance.Enabled = false;
                }
            }
            ddlToStore.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    #endregion LoadGridData
}
