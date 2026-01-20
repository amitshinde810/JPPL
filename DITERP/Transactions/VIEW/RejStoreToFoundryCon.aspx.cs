using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Transactions_VIEW_RejStoreToFoundryCon : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='150'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadProduction();
                    LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadProduction
    private void LoadProduction()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT TOP 500 RTF_CODE,RTF_DOC_NO,convert(varchar,RTF_DOC_DATE,106) AS RTF_DOC_DATE  from REJECTION_TO_FOUNDRY_MASTER where ES_DELETE=0 and RTF_CM_CODE='" + Session["CompanyCode"] + "' order by CONVERT(INT, RTF_DOC_NO) DESC");
            if (dt.Rows.Count > 0)
            {
                dgProductionStore.Enabled = true;
                dgProductionStore.DataSource = dt;
                dgProductionStore.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "LoadProduction", Ex.Message);
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select TOP 500 RTF_CODE,RTF_DOC_NO,convert(varchar,RTF_DOC_DATE,106) AS RTF_DOC_DATE from REJECTION_TO_FOUNDRY_MASTER where ES_DELETE=0 and RTF_CM_CODE='" + Session["CompanyCode"] + "' and (upper(dbo.REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_NO) like upper('%" + str + "%') OR upper(dbo.REJECTION_TO_FOUNDRY_MASTER.RTF_DOC_DATE) like upper('%" + str + "%')) ORDER BY RTF_DOC_NO DESC");
            else
                dtfilter = CommonClasses.Execute("select TOP 500 RTF_CODE,RTF_DOC_NO,convert(varchar,RTF_DOC_DATE,106) AS RTF_DOC_DATE from REJECTION_TO_FOUNDRY_MASTER where ES_DELETE=0 and RTF_CM_CODE='" + Session["CompanyCode"] + "' order by RTF_DOC_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgProductionStore.Enabled = true;
                dgProductionStore.DataSource = dtfilter;
                dgProductionStore.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgProductionStore.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RTF_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RTF_DOC_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RTF_DOC_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProductionStore.DataSource = dtFilter;
                    dgProductionStore.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "LoadStatus", ex.Message);
        }
    }

    public void LoadFilter()
    {
        try
        {
            if (dgProductionStore.Rows.Count == 0)
            {
                dtFilter.Clear();
                dgProductionStore.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("RTF_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RTF_DOC_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("RTF_DOC_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgProductionStore.DataSource = dtFilter;
                    dgProductionStore.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "LoadFilter", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/RejStoreToFoundryCon.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region dgProductionStore_RowDeleting
    protected void dgProductionStore_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgProductionStore.Rows[e.RowIndex].FindControl("lblPS_CODE"))).Text))
            {
                string ps_code = ((Label)(dgProductionStore.Rows[e.RowIndex].FindControl("lblPS_CODE"))).Text;
                DataTable dtDetail = new DataTable();

                dtDetail = CommonClasses.Execute(" SELECT  RTF_ISUSED,RTFD_CODE, RTFD_RTF_CODE, RTFD_REJ_ITEMCODE,RTFD_CON_ITEMCODE, RTFD_STK_REJ_QTY,RTFD_STAND_WEIGHT, RTFD_CON_QTY,RTFD_IS_ACCEPTANCE, RTF_CODE, RTF_DOC_NO, RTF_DOC_DATE, RTF_CM_CODE FROM REJECTION_TO_FOUNDRY_DETAIL INNER JOIN REJECTION_TO_FOUNDRY_MASTER ON REJECTION_TO_FOUNDRY_DETAIL.RTFD_RTF_CODE = REJECTION_TO_FOUNDRY_MASTER.RTF_CODE WHERE (REJECTION_TO_FOUNDRY_MASTER.ES_DELETE = 0)  AND RTFD_RTF_CODE='" + ps_code + "' ORDER BY  RTFD_IS_ACCEPTANCE DESC");
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    if (dtDetail.Rows[i]["RTF_ISUSED"].ToString().ToUpper() == "TRUE" || dtDetail.Rows[i]["RTF_ISUSED"].ToString().ToUpper() == "1")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used In other Transaction You can not Delete It.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                bool flag = CommonClasses.Execute1("UPDATE REJECTION_TO_FOUNDRY_MASTER SET ES_DELETE = 1 WHERE RTF_CODE='" + Convert.ToInt32(ps_code) + "'");

                for (int j = 0; j < dtDetail.Rows.Count; j++)
                {
                    CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtDetail.Rows[j]["RTFD_STK_REJ_QTY"].ToString() + "  where I_CODE='" + dtDetail.Rows[j]["RTFD_REJ_ITEMCODE"].ToString() + "'");
                }
                CommonClasses.Execute1(" DELETE FROM STOCK_LEDGER where STL_DOC_NO='" + ps_code + "' AND    STL_DOC_TYPE IN ('Issue for Casting Conversion','Conversion Of Casting','Issue To Main Store')");
                LoadProduction();
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights to Delete";           
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return ;
        }
    }

    #endregion

    #region dgProductionStore_PageIndexChanging
    protected void dgProductionStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgProductionStore.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store- View", "dgProductionStore_PageIndexChanging", Ex.Message);
        }
    }
    #endregion

    #region dgProductionStore_RowCommand
    protected void dgProductionStore_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string ps_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/RejStoreToFoundryCon.aspx?c_name=" + type + "&ps_code=" + ps_code, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights to view";
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string ps_code = e.CommandArgument.ToString();
                        DataTable DtDetails = CommonClasses.Execute("select ISNULL(RTF_ISUSED,0) AS RTF_ISUSED FROM REJECTION_TO_FOUNDRY_MASTER  WHERE   RTF_CODE='" + ps_code + "'");

                        if (DtDetails.Rows[0]["RTF_ISUSED"].ToString().ToUpper() == "1" || DtDetails.Rows[0]["RTF_ISUSED"].ToString().ToUpper() == "TRUE")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You Can Not Modify ,This is Used In Acceptance Entry.  ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        string type = "MODIFY";
                       
                        Response.Redirect("~/Transactions/ADD/RejStoreToFoundryCon.aspx?c_name=" + type + "&ps_code=" + ps_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another user";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights to view";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "dgProductionStore_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select ES_MODIFY from REJECTION_TO_FOUNDRY_MASTER where RTF_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-VIEW", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}

