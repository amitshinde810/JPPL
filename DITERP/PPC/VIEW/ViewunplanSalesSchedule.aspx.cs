using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class PPC_VIEW_ViewunplanSalesSchedule : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPC");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("PPCMV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='251'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadStatus(txtString);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadData
    private void LoadData()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = CommonClasses.Execute("SELECT DISTINCT G.GP_NAME,S.SIM_CODE, I_CODENO +' - '+ I_NAME AS PROD_NAME ,I.I_CODE,G.GP_CODE,ISNULL(SIM_FOUNDRY,0) AS SIM_FOUNDRY,ISNULL(SIM_RFM_STORE,0) AS SIM_RFM_STORE,ISNULL(SIM_VENDOR,0) AS SIM_VENDOR,ISNULL(SIM_MAIN_STORE,0) AS SIM_MAIN_STORE,ISNULL(SIM_MACHINE_SHOP,0) AS SIM_MACHINE_SHOP,ISNULL(SIM_RFI_STORE,0) AS SIM_RFI_STORE,ISNULL(SIM_FINISH_GOODS,0) AS SIM_FINISH_GOODS  FROM STANDARD_INVENTARY_MASTER S INNER JOIN ITEM_MASTER I on S.SIM_I_CODE=I.I_CODE INNER JOIN GROUP_MASTER G ON S.SIM_GP_CODE=G.GP_CODE WHERE S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0 AND S.SIM_COMP_ID='" + (string)Session["CompanyId"] + "'");
            dt = CommonClasses.Execute("SELECT DISTINCT C.CS_DATE AS ddd,CS_CODE, P_CODE,P_NAME,I_CODENO +' - '+ I_NAME AS PROD_NAME,I_CODE, right(convert(varchar, C.CS_DATE, 106), 8) as CS_DATE,I.I_CODE, ISNULL(C.CS_SCHEDULE_QTY, 0) AS CS_SCHEDULE_QTY, ISNULL(C.CS_WEEK1, 0) AS CS_WEEK1, ISNULL(C.CS_WEEK2, 0) AS CS_WEEK2, ISNULL(C.CS_WEEK3, 0) AS CS_WEEK3, ISNULL(C.CS_WEEK4, 0) AS CS_WEEK4 FROM CUSTOMER_SCHEDULE AS C INNER JOIN ITEM_MASTER AS I ON C.CS_I_CODE = I.I_CODE  inner join PARTY_MASTER P ON C.CS_P_CODE=P.P_CODE WHERE (C.ES_DELETE = 0) AND (I.ES_DELETE = 0) AND P.ES_DELETE =0 AND C.CS_COMP_ID='" + (string)Session["CompanyId"] + "' AND C.CS_CM_CODE='" + (string)Session["CompanyCode"] + "'  ORDER BY C.CS_DATE DESC,P_NAME,I_CODENO +' - '+ I_NAME");
            if (dt.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgCustomerSchedule.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("CS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CS_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROD_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerSchedule.DataSource = dtFilter;
                    dgCustomerSchedule.DataBind();
                }
            }
            else
            {
                dgCustomerSchedule.Enabled = true;
                dgCustomerSchedule.DataSource = dt;
                dgCustomerSchedule.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "LoadUser", Ex.Message);
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
            CommonClasses.SendError("unplan Sales Schedule- View", "txtString_TextChanged", Ex.Message);
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
            if (str != "")
                dtfilter = CommonClasses.Execute(" SELECT DISTINCT U.US_DATE AS ddd,US_CODE,  I_CODENO +' - '+ I_NAME AS PROD_NAME,I_CODE,  convert(varchar, U.US_DATE, 106)  as US_DATE,  ISNULL(U.US_QTY, 0) AS US_QTY  FROM UNPLAN_SALE_SCHEDULE U,ITEM_MASTER  where I_CODE=U.US_I_CODE AND U.ES_DELETE=0 AND (ITEM_MASTER.ES_DELETE = 0)     AND U.US_CM_CODE='" + (string)Session["CompanyCode"] + "'  and (lower(I_CODENO +' - '+ I_NAME) like lower('%" + str + "%') or (DATENAME(MM,US_DATE) like lower('%" + str + "%')) or (DATENAME(YYYY,US_DATE) like lower('%" + str + "%')) or (US_QTY  like lower('%" + str + "%'))  )  ORDER BY U.US_DATE DESC ,I_CODENO +' - '+ I_NAME");
            else
                dtfilter = CommonClasses.Execute(" SELECT DISTINCT U.US_DATE AS ddd,US_CODE,  I_CODENO +' - '+ I_NAME AS PROD_NAME,I_CODE,  convert(varchar, U.US_DATE, 106)  as US_DATE,  ISNULL(U.US_QTY, 0) AS US_QTY  FROM UNPLAN_SALE_SCHEDULE U,ITEM_MASTER  where I_CODE=U.US_I_CODE AND U.ES_DELETE=0 AND (ITEM_MASTER.ES_DELETE = 0)     AND U.US_CM_CODE='" + (string)Session["CompanyCode"] + "'   ORDER BY U.US_DATE DESC ,I_CODENO +' - '+ I_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerSchedule.Enabled = true;
                dgCustomerSchedule.DataSource = dtfilter;
                dgCustomerSchedule.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgCustomerSchedule.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("US_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("US_DATE", typeof(String))); 
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROD_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("US_QTY", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerSchedule.DataSource = dtFilter;
                    dgCustomerSchedule.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("unplan Sales Schedule - View", "LoadStatus", Ex.Message);
        }
    }
    #endregion

    #region btnInsert
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/PPC/ADD/unplanSalesSchedule.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("unplan Sales Schedule", "btnInsert_Click", exc.Message);
        }
    }
    #endregion

    #region dgCustomerSchedule_PageIndexChanging
    protected void dgCustomerSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerSchedule.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "dgCustomerSchedule_PageIndexChanging", Ex.Message);
        }
    }
    #endregion dgCustomerSchedule_PageIndexChanging

    #region dgCustomerSchedule_RowCommand
    protected void dgCustomerSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                return;
            }
            #region View
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string um_code = e.CommandArgument.ToString();
                    Response.Redirect("~/PPC/ADD/unplanSalesSchedule.aspx?c_name=" + type + "&u_code=" + um_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            #endregion View

            #region Modify
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string um_code = e.CommandArgument.ToString();
                        Response.Redirect("~/PPC/ADD/unplanSalesSchedule.aspx?c_name=" + type + "&u_code=" + um_code, false);
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

    
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("unplan Sales Schedule", "dgCustomerSchedule_RowCommand", exc.Message);
        }
    }
    #endregion dgCustomerSchedule_RowCommand

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from UNPLAN_SALE_SCHEDULE where US_CODE=" + PrimaryKey + "  ");
            if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another User";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return true;
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("unplan Sales Schedule", "dgCustomerSchedule_RowEditing", exc.Message);
        }
        return false;
    }
    #endregion

    #region dgCustomerSchedule_RowDeleting
    protected void dgCustomerSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblUS_CODE"))).Text))
                {
                    string um_code = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblUS_CODE"))).Text;

                    string CS_DATE = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblUS_DATE"))).Text;

                    string I_CODE = ((Label)(dgCustomerSchedule.Rows[e.RowIndex].FindControl("lblI_CODE"))).Text;

                     
                    DataTable DtDetail = CommonClasses.Execute(" SELECT * FROM INVOICE_MASTER,INVOICE_DETAIL where INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0 AND INM_DATE= '" + Convert.ToDateTime(CS_DATE).ToString("dd/MMM/yyyy") + "'   AND IND_I_CODE ='" + I_CODE + "'  ");

                    if (DtDetail.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You Can Not Delete ,This is Used In Invoice. ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                    bool flag = CommonClasses.Execute1("UPDATE UNPLAN_SALE_SCHEDULE SET ES_DELETE = 1 WHERE US_CODE='" + Convert.ToInt32(um_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("unplan Sales Schedule", "Delete", "unplan Sales Schedule", um_code, Convert.ToInt32(um_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    }
                    LoadStatus(txtString);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record deleted Successfully...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
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
            CommonClasses.SendError("unplan Sales Schedule", "dgCustomerSchedule_RowEditing", exc.Message);
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
            CommonClasses.SendError("User Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgCustomerSchedule_RowEditing
    protected void dgCustomerSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Update"))
            {
                string user_code = ((Label)(dgCustomerSchedule.Rows[e.NewEditIndex].FindControl("lblCS_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/PPC/ADD/CustomerSchedule.aspx?c_name=" + type + "&u_code=" + user_code, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception exc)
        {
            CommonClasses.SendError("unplan Sales Schedule", "dgCustomerSchedule_RowEditing", exc.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PPCDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("unplan Sales Schedule", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click
}
