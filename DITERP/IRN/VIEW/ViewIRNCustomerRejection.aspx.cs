using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewIRNCustomerRejection : System.Web.UI.Page
{
    //Created by Suja
    #region " Var "
    UnitMaster_BL BL_UnitMaster = null;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='256'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    if (string.IsNullOrEmpty((string)Session["Status"]))
                    {
                        Session["Status"] = "0";
                    }
                    if (Session["Status"] == "0")
                    {
                        rbType.SelectedValue = "0";
                    }
                    else
                    {
                        rbType.SelectedValue = "1";
                    }
                    LoadData();
                    if (dgCustRej.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("CRM_NOCHAR", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CRM_DOC_DATE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CRM_STATUS", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CD_I_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CD_CHALLAN_QTY", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(string)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CD_CHK_REJ_STATUS", typeof(string)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgCustRej.DataSource = dtFilter;
                            dgCustRej.DataBind();
                            dgCustRej.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();

            //ASP per New  
            if (rbType.SelectedValue == "1")
            {
                dt = CommonClasses.Execute("SELECT DISTINCT CRM_CODE,CASE WHEN CRM_STATUS=1 then '1' Else '0' END  AS CRM_STATUS,CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END AS CD_CHK_REJ_STATUS,convert(varchar, CRM_DOC_DATE,106) AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE  INNER JOIN IRN_CUSTOMER_REJECTION_MASTER ON CUSTREJECTION_MASTER.CR_CODE = IRN_CUSTOMER_REJECTION_MASTER.CRM_CR_CODE INNER JOIN IRN_CUSTOMER_REJECTION_DETAIL ON CRD_I_CODE=CD_I_CODE AND CRM_CODE=CRD_CRM_CODE WHERE (PARTY_MASTER.ES_DELETE=0) AND I_DEVELOMENT=0 AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "')  AND (CR_CM_CODE='" + Session["CompanyCode"] + "')  AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND   I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CASE WHEN CRM_STATUS=1 then '1' Else '0' END=1  ORDER BY CR_GIN_NO DESC ");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT CRM_CODE,CASE WHEN CRM_STATUS=1 then '1' Else '0' END  AS CRM_STATUS,CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END AS CD_CHK_REJ_STATUS,convert(varchar, CRM_DOC_DATE,106) AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE  INNER JOIN IRN_CUSTOMER_REJECTION_MASTER ON CUSTREJECTION_MASTER.CR_CODE = IRN_CUSTOMER_REJECTION_MASTER.CRM_CR_CODE INNER JOIN IRN_CUSTOMER_REJECTION_DETAIL ON CRD_I_CODE=CD_I_CODE AND CRM_CODE=CRD_CRM_CODE WHERE (PARTY_MASTER.ES_DELETE=0) AND I_DEVELOMENT=0 AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "')  AND (CR_CM_CODE='" + Session["CompanyCode"] + "')  AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0)  AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CASE WHEN CRM_STATUS=1 then '1' Else '0' END=0 UNION SELECT DISTINCT '' as CRM_CODE,'0' AS CRM_STATUS,'' AS CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS, ' ' AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,'' AS CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE      WHERE (PARTY_MASTER.ES_DELETE=0) AND I_DEVELOMENT=0 AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "') AND (CR_CM_CODE='" + Session["CompanyCode"] + "') AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CD_CHK_REJ_STATUS=0 ORDER BY CR_GIN_NO DESC ");

            }
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For UpdateStatus"))
                {

                    DataView dv1 = dt.DefaultView;
                    dv1.RowFilter = " CD_CHK_REJ_STATUS=1 ";
                    //dt.Rows.Clear();
                    dt = dv1.ToTable();
                }
            }
            if (dt.Rows.Count > 0)
            {
                dgCustRej.DataSource = dt;
                dgCustRej.DataBind();
                dgCustRej.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_NOCHAR", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_DOC_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_STATUS", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CD_I_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CD_CHALLAN_QTY", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CD_CHK_REJ_STATUS", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustRej.DataSource = dtFilter;
                    dgCustRej.DataBind();
                    dgCustRej.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "LoadData", Ex.Message);
        }
    }
    #endregion

    #region dgCustRej_RowDeleting
    protected void dgCustRej_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustRej.Rows[e.RowIndex].FindControl("lblCRM_CODE"))).Text))
                {
                    string CRM_CODE = ((Label)(dgCustRej.Rows[e.RowIndex].FindControl("lblCRM_CODE"))).Text;
                    string CRM_NO = ((Label)(dgCustRej.Rows[e.RowIndex].FindControl("lblCRM_NO"))).Text;
                    bool flag = CommonClasses.Execute1("UPDATE IRN_CUSTOMER_REJECTION_MASTER SET ES_DELETE=1 where CRM_CODE='" + CRM_CODE + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("IRN Customer Rejection", "Delete", "IRN Customer Rejection", CRM_NO, Convert.ToInt32(CRM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                    LoadData();
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "GridView1_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgCustRej_RowEditing
    protected void dgCustRej_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string RSM_CODE = ((Label)(dgCustRej.Rows[e.NewEditIndex].FindControl("lblRSM_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/IRN/ADD/ReasonMaster.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "dgCustRej_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgCustRej_PageIndexChanging
    protected void dgCustRej_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgCustRej.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }
    #endregion

    #region dgCustRej_RowUpdating
    protected void dgCustRej_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    #endregion

    #region dgCustRej_RowCommand
    protected void dgCustRej_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string RSM_CODE = e.CommandArgument.ToString();
                    Response.Redirect("~/IRN/ADD/IRNCustomerRejection.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string RSM_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/IRN/ADD/IRNCustomerRejection.aspx?c_name=" + type + "&i_uom_code=" + RSM_CODE, false);
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
            #region Add
            if (e.CommandName.Equals("Add"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    int index = Convert.ToInt32(e.CommandArgument.ToString());
                    string code = ((Label)(dgCustRej.Rows[index].FindControl("lblCRM_CODE"))).Text;
                    string No = ((Label)(dgCustRej.Rows[index].FindControl("lblCR_GIN_NO"))).Text;
                    string Date = ((Label)(dgCustRej.Rows[index].FindControl("lblCR_GIN_DATE"))).Text;
                    string ICode = ((Label)(dgCustRej.Rows[index].FindControl("lblCD_I_CODE"))).Text;
                    string PCode = ((Label)(dgCustRej.Rows[index].FindControl("lblP_CODE"))).Text;
                    if (!ModifyLog(code))
                    {
                        string type = "Add";
                        Response.Redirect("~/IRN/ADD/IRNCustomerRejection.aspx?c_name=" + type + "&IM_CODE=" + code + "&RejNo=" + No + "&RejDate=" + Date + "&ICode=" + ICode + "&PCode=" + PCode + "", false);
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
            #region Print
            else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Print"))
                {
                    string type = "0";
                    string MatReq_Code = e.CommandArgument.ToString();
                    Response.Redirect("~/IRN/ADD/IRNCustomerRejectionPrint.aspx?MatReq_Code=" + MatReq_Code + "&Type=" + type + "", false);
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

            #region UpdateStatus
            else if (e.CommandName.Equals("Status"))
            {
                string type = "";
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For UpdateStatus"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        if (e.CommandName.Equals("Status"))
                        {
                            type = "UpdateStatus";
                        }
                        else
                        {
                            type = "VIEW";
                        }

                        int index = Convert.ToInt32(e.CommandArgument.ToString());
                        string code = ((Label)(dgCustRej.Rows[index].FindControl("lblCRM_CODE"))).Text;
                        string um_code = e.CommandArgument.ToString();
                        int Index = Convert.ToInt32(e.CommandArgument.ToString());
                        GridViewRow row = dgCustRej.Rows[Index];
                        string mt_month = ((Label)(row.FindControl("lblCRM_CODE"))).Text;
                        string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;
                        string No = ((Label)(dgCustRej.Rows[index].FindControl("lblCR_GIN_NO"))).Text;
                        string Date = ((Label)(dgCustRej.Rows[index].FindControl("lblCR_GIN_DATE"))).Text;
                        string ICode = ((Label)(dgCustRej.Rows[index].FindControl("lblCD_I_CODE"))).Text;
                        string PCode = ((Label)(dgCustRej.Rows[index].FindControl("lblP_CODE"))).Text;



                        if (e.CommandName.Equals("Status"))
                        {
                            if (Status == "Close")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You Have No Rights To Change Status";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                return;
                            }

                            Response.Redirect("~/IRN/ADD/IRNCustomerRejection.aspx?c_name=" + type + "&IM_CODE=" + code + "&RejNo=" + No + "&RejDate=" + Date + "&ICode=" + ICode + "&PCode=" + PCode + "", false);
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
            #endregion UpdateStatus
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgCustRej_RowDataBound
    protected void dgCustRej_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Customer Rejection master IRN form flag checked
            LinkButton lnkPost = (LinkButton)e.Row.FindControl("lnkPost");

            //Customer rejection inward detail table flag checked
            Label LblStatus = (Label)e.Row.FindControl("lblCD_CHK_REJ_STATUS");
            LinkButton lnkAdd = ((LinkButton)e.Row.FindControl("lnkAdd"));
            LinkButton lnkPrint = ((LinkButton)e.Row.FindControl("lnkPrint"));
            LinkButton lnkDelete = ((LinkButton)e.Row.FindControl("lnkDelete"));
            LinkButton lnkView = ((LinkButton)e.Row.FindControl("lnkView"));
            //If false then open Button InActive otherwise Active
            if (LblStatus.Text == "0")
            {
                //Non Editatble
                lnkPost.Enabled = false;
                lnkPrint.Enabled = false;
                //Editatble
                lnkAdd.Enabled = true;
                lnkView.Enabled = false;
                lnkDelete.Enabled = false;

            }
            else
            {
                //Editatble
                lnkPost.Enabled = true;
                lnkPrint.Enabled = true;
                //Non Editable
                lnkAdd.Enabled = false;


                lnkView.Enabled = true;
                lnkDelete.Enabled = true;
            }
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from IRN_CUSTOMER_REJECTION_MASTER where CRM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("IRN Customer Rejection-View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/IRNDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "btnSearch_Click", ex.Message);
        }
    }
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\''");

            DataTable dtfilter = new DataTable();

            if (str != "")
            {
                if (rbType.SelectedValue == "1")
                {
                    dtfilter = CommonClasses.Execute("SELECT DISTINCT CRM_CODE,CASE WHEN CRM_STATUS=1 then '1' Else '0' END  AS CRM_STATUS,CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS, convert(varchar, CRM_DOC_DATE,106)  AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE  INNER JOIN IRN_CUSTOMER_REJECTION_MASTER ON CUSTREJECTION_MASTER.CR_CODE = IRN_CUSTOMER_REJECTION_MASTER.CRM_CR_CODE INNER JOIN IRN_CUSTOMER_REJECTION_DETAIL ON CRD_I_CODE=CD_I_CODE AND CRM_CODE=CRD_CRM_CODE   WHERE (PARTY_MASTER.ES_DELETE=0) AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "')  AND (CR_CM_CODE='" + Session["CompanyCode"] + "')  AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CASE WHEN CRM_STATUS=1 then '1' Else '0' END=1 and ( CRM_NOCHAR like upper('%" + str + "%') OR CR_GIN_NO like upper('%" + str + "%') or I_CODENO like upper('%" + str + "%') or I_NAME like upper('%" + str + "%') or P_NAME like upper('%" + str + "%') or convert(varchar,CR_GIN_DATE,106) like upper('%" + str + "%'))    ORDER BY CR_GIN_NO DESC ");
                }
                else
                {
                    dtfilter = CommonClasses.Execute("SELECT DISTINCT CRM_CODE,CASE WHEN CRM_STATUS=1 then '1' Else '0' END  AS CRM_STATUS,CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS, convert(varchar, CRM_DOC_DATE,106)  AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE  INNER JOIN IRN_CUSTOMER_REJECTION_MASTER ON CUSTREJECTION_MASTER.CR_CODE = IRN_CUSTOMER_REJECTION_MASTER.CRM_CR_CODE INNER JOIN IRN_CUSTOMER_REJECTION_DETAIL ON CRD_I_CODE=CD_I_CODE AND CRM_CODE=CRD_CRM_CODE   WHERE (PARTY_MASTER.ES_DELETE=0) AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "')  AND (CR_CM_CODE='" + Session["CompanyCode"] + "')  AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CASE WHEN CRM_STATUS=1 then '1' Else '0' END=0 and ( CRM_NOCHAR like upper('%" + str + "%') OR CR_GIN_NO like upper('%" + str + "%') or I_CODENO like upper('%" + str + "%') or I_NAME like upper('%" + str + "%') or P_NAME like upper('%" + str + "%') or convert(varchar,CR_GIN_DATE,106) like upper('%" + str + "%')) UNION SELECT DISTINCT '' as CRM_CODE,'0' AS CRM_STATUS,'' AS CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS, ' ' AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,'' AS CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE WHERE (PARTY_MASTER.ES_DELETE=0) AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "') AND (CR_CM_CODE='" + Session["CompanyCode"] + "') AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "'  AND CD_CHK_REJ_STATUS=0 and (CR_GIN_NO like upper('%" + str + "%') or I_CODENO like upper('%" + str + "%') or I_NAME like upper('%" + str + "%') or P_NAME like upper('%" + str + "%') or convert(varchar,CR_GIN_DATE,106) like upper('%" + str + "%')) ORDER BY CR_GIN_NO DESC ");
                }
            }
            else
            {
                if (rbType.SelectedValue == "1")
                {
                    dtfilter = CommonClasses.Execute("SELECT DISTINCT CRM_CODE,CASE WHEN CRM_STATUS=1 then '1' Else '0' END  AS CRM_STATUS,CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS,  convert(varchar, CRM_DOC_DATE,106)  AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE  INNER JOIN IRN_CUSTOMER_REJECTION_MASTER ON CUSTREJECTION_MASTER.CR_CODE = IRN_CUSTOMER_REJECTION_MASTER.CRM_CR_CODE INNER JOIN IRN_CUSTOMER_REJECTION_DETAIL ON CRD_I_CODE=CD_I_CODE AND CRM_CODE=CRD_CRM_CODE WHERE (PARTY_MASTER.ES_DELETE=0) AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "')  AND (CR_CM_CODE='" + Session["CompanyCode"] + "')  AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CASE WHEN CRM_STATUS=1 then '1' Else '0' END=1  ORDER BY CR_GIN_NO DESC ");

                }
                else
                {
                    dtfilter = CommonClasses.Execute("SELECT DISTINCT CRM_CODE,CASE WHEN CRM_STATUS=1 then '1' Else '0' END  AS CRM_STATUS,CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS,  convert(varchar, CRM_DOC_DATE,106)  AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE  INNER JOIN IRN_CUSTOMER_REJECTION_MASTER ON CUSTREJECTION_MASTER.CR_CODE = IRN_CUSTOMER_REJECTION_MASTER.CRM_CR_CODE INNER JOIN IRN_CUSTOMER_REJECTION_DETAIL ON CRD_I_CODE=CD_I_CODE AND CRM_CODE=CRD_CRM_CODE WHERE (PARTY_MASTER.ES_DELETE=0) AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "')  AND (CR_CM_CODE='" + Session["CompanyCode"] + "')  AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' AND CASE WHEN CRM_STATUS=1 then '1' Else '0' END=0 UNION SELECT DISTINCT '' as CRM_CODE,'0' AS CRM_STATUS,'' AS CRM_NOCHAR,CASE WHEN CD_CHK_REJ_STATUS=1 then '1' Else '0' END  AS CD_CHK_REJ_STATUS, ' ' AS CRM_DOC_DATE ,CD_I_CODE,CUSTREJECTION_MASTER.CR_CODE,CUSTREJECTION_MASTER.CR_GIN_NO,P_CODE,'' AS CRM_NO,convert(varchar, CR_GIN_DATE,106) as CR_GIN_DATE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,CUSTREJECTION_DETAIL.CD_CHALLAN_QTY,PARTY_MASTER.P_NAME FROM CUSTREJECTION_DETAIL INNER JOIN ITEM_MASTER ON CUSTREJECTION_DETAIL.CD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN CUSTREJECTION_MASTER ON CUSTREJECTION_DETAIL.CD_CR_CODE = CUSTREJECTION_MASTER.CR_CODE INNER JOIN PARTY_MASTER ON CUSTREJECTION_MASTER.CR_P_CODE = PARTY_MASTER.P_CODE      WHERE (PARTY_MASTER.ES_DELETE=0) AND (PARTY_MASTER.P_CM_COMP_ID='" + Session["CompanyId"] + "') AND (CR_CM_CODE='" + Session["CompanyCode"] + "') AND (CUSTREJECTION_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.ES_DELETE = 0) AND  I_DEVELOMENT=0 AND CR_GIN_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "'  AND CD_CHK_REJ_STATUS=0  ORDER BY CR_GIN_NO DESC ");
                }
            }

            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For UpdateStatus"))
                {

                    DataView dv1 = dtfilter.DefaultView;
                    dv1.RowFilter = " CD_CHK_REJ_STATUS=1 ";
                    //dt.Rows.Clear();
                    dtfilter = dv1.ToTable();
                }
            }

            if (dtfilter.Rows.Count > 0)
            {
                dgCustRej.DataSource = dtfilter;
                dgCustRej.DataBind();
                dgCustRej.Enabled = true;
            }
            else
            {
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_NOCHAR", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_DOC_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CRM_STATUS", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CR_GIN_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CD_I_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CD_CHALLAN_QTY", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CD_CHK_REJ_STATUS", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustRej.DataSource = dtFilter;
                    dgCustRej.DataBind();
                    dgCustRej.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "LoadStatus", ex.Message);
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
                Response.Redirect("~/IRN/ADD/IRNCustomerRejection.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection-View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region rbType_SelectedIndexChanged
    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Status"] = rbType.SelectedValue.ToString();
        LoadData();
    }
    #endregion
}
