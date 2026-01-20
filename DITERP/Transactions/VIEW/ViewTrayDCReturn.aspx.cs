using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewTrayDCReturn : System.Web.UI.Page
{
    #region Variable
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='263'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadDeliveryChallan();

                    if (dgDetailDC.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("DCM_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("DCM_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("DCM_DATE", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgDetailDC.DataSource = dtFilter;
                            dgDetailDC.DataBind();
                            dgDetailDC.Enabled = false;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Dellivery Challan Return", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadDeliveryChallan
    private void LoadDeliveryChallan()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT DISTINCT TOP 500 P_NAME,DNM_CODE AS DCM_CODE,DNM_NO AS DCM_NO,  convert(varchar,DNM_DATE,106) as DCM_DATE FROM DC_RETURN_MASTER,DC_RETURN_DETAIL,PARTY_MASTER where DNM_CODE=DND_DNM_CODE AND DNM_P_CODE=P_CODE	AND DC_RETURN_MASTER.ES_DELETE=0  and DNM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND DNM_TYPE='DCTIN' order by DNM_CODE DESC ");
            dgDetailDC.DataSource = dt;
            dgDetailDC.DataBind();
            if (dgDetailDC.Rows.Count > 0)
            {
                dgDetailDC.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Return", "LoadDeliveryChallanReturn", Ex.Message);
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
            CommonClasses.SendError("Tray Delivery Challan", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT DISTINCT TOP 500 P_NAME,DNM_CODE AS DCM_CODE,DNM_NO AS DCM_NO,convert(varchar,DNM_DATE,106) as DCM_DATE FROM DC_RETURN_MASTER,DC_RETURN_DETAIL,PARTY_MASTER where DNM_CODE=DND_DNM_CODE AND DNM_P_CODE=P_CODE	AND DC_RETURN_MASTER.ES_DELETE=0  and DNM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND DNM_TYPE='DCTIN' and (DNM_NO like upper('%" + str + "%') OR upper(DNM_DATE) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) order by DNM_CODE desc");
            else
                dtfilter = CommonClasses.Execute("SELECT DISTINCT TOP 500 P_NAME,DNM_CODE AS DCM_CODE,DNM_NO AS DCM_NO,convert(varchar,DNM_DATE,106) as DCM_DATE FROM DC_RETURN_MASTER,DC_RETURN_DETAIL,PARTY_MASTER where DNM_CODE=DND_DNM_CODE AND DNM_P_CODE=P_CODE	AND DC_RETURN_MASTER.ES_DELETE=0  and DNM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " AND DNM_TYPE='DCTIN' order by DNM_CODE desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgDetailDC.DataSource = dtfilter;
                dgDetailDC.DataBind();
                dgDetailDC.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("DCM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("DCM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("DCM_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailDC.DataSource = dtFilter;
                    dgDetailDC.DataBind();
                    dgDetailDC.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tray Delivery Challan", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/TrayDCReturn.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Tray Delivery Challan", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tray Delivery Challan", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailDC_RowDeleting
    protected void dgDetailDC_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailDC.Rows[e.RowIndex].FindControl("lblDCM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgDetailDC.Rows[e.RowIndex].FindControl("lblDCM_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE DC_RETURN_MASTER SET ES_DELETE = 1 WHERE DNM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                    if (flag == true)
                    {
                        DataTable dtq = CommonClasses.Execute("SELECT DND_REC_QTY  ,DND_SCRAP_QTY,DND_REC_QTY+DND_SCRAP_QTY AS TOTAL_QTY ,DND_I_CODE FROM DC_RETURN_DETAIL where DND_DNM_CODE=" + Convert.ToInt32(cpom_code) + " ");

                        for (int i = 0; i < dtq.Rows.Count; i++)
                        {
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtq.Rows[i]["DND_REC_QTY"] + " where I_CODE='" + dtq.Rows[i]["DND_I_CODE"] + "'");
                            DataTable stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + Convert.ToInt32(cpom_code) + "' and CL_DOC_TYPE='DCTIN' AND  CL_I_CODE ='" + dtq.Rows[i]["DND_I_CODE"] + "'  ");
                            for (int z = 0; z < stockLedger.Rows.Count; z++)
                            {
                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + stockLedger.Rows[z]["CL_P_CODE"].ToString() + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='DCTOUT'");
                            }
                            CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + Convert.ToInt32(cpom_code) + "' and CL_DOC_TYPE='DCTIN' AND CL_I_CODE='" + dtq.Rows[i]["DND_I_CODE"] + "'  ");
                        }
                        CommonClasses.Execute(" DELETE FROM STOCK_LEDGER where STL_DOC_NO='" + cpom_code + "' AND  STL_DOC_TYPE='DCTIN' ");

                        CommonClasses.WriteLog("Tray  Delivery Challan", "Delete", "Delivery Challan", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Tray Delivery_Challan", "dgDetailDC_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            LoadDeliveryChallan();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You have no rights to delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgDetailDC_RowCommand
    protected void dgDetailDC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if ((string)e.CommandArgument == "0" || (string)e.CommandArgument == "")
            {
                return;
            }
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    string type = "VIEW";
                    string cpom_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/TrayDCReturn.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to view";
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
                        string cpom_code = e.CommandArgument.ToString();
                        int cnt = 0;
                        Response.Redirect("~/Transactions/ADD/TrayDCReturn.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/DCReturn.aspx?cpom_code=" + cpom_code + "&print_type=" + type1 + "&type=0", false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Return", "dgDetailDC_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from DC_RETURN_MASTER where DCM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Delivery Challan", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgDetailDC_PageIndexChanging
    protected void dgDetailDC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailDC.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
