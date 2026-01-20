using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_TrayChallan : System.Web.UI.Page
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='71'");
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
            CommonClasses.SendError("Tray Dellivery Challan", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadDeliveryChallan
    private void LoadDeliveryChallan()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select P_NAME,DCM_CODE,DCM_NO,convert(varchar,DCM_DATE,106) as DCM_DATE from DELIVERY_CHALLAN_MASTER ,PARTY_MASTER where DELIVERY_CHALLAN_MASTER.ES_DELETE=0 and DCM_P_CODE=P_CODE  and DCM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + "  AND  DCM_TYPE='DLCT'  order by DCM_CODE DESC ");
            dgDetailDC.DataSource = dt;
            dgDetailDC.DataBind();
            if (dgDetailDC.Rows.Count > 0)
            {
                dgDetailDC.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tray Delivery Challan", "LoadDeliveryChallan", Ex.Message);
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
                dtfilter = CommonClasses.Execute("SELECT DCM_CODE,DCM_NO,convert(varchar,DCM_DATE,106) as DCM_DATE,P_NAME FROM DELIVERY_CHALLAN_MASTER,PARTY_MASTER WHERE DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.DCM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND DELIVERY_CHALLAN_MASTER.ES_DELETE='0' AND  DCM_TYPE='DLCT'  and (DCM_NO like upper('%" + str + "%') OR upper(DCM_DATE) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) order by DCM_CODE desc");
            else
                dtfilter = CommonClasses.Execute("SELECT DCM_CODE,DCM_NO,convert(varchar,DCM_DATE,106) as DCM_DATE,P_NAME FROM DELIVERY_CHALLAN_MASTER,PARTY_MASTER WHERE DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.DCM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND DELIVERY_CHALLAN_MASTER.ES_DELETE='0'  AND  DCM_TYPE='DLCT'  order by DCM_CODE desc");

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
                Response.Redirect("~/Transactions/ADD/TrayDelivery_Challan.aspx?c_name=" + type, false);
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

                    if (CommonClasses.CheckUsedInTran("CHALLAN_STOCK_LEDGER", "CL_DOC_ID", " AND CL_DOC_TYPE='DCTOUT' AND CL_CON_QTY>0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record not deleted, it is used in Tray Deliver challan Return";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE DELIVERY_CHALLAN_MASTER SET ES_DELETE = 1 WHERE DCM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                        if (flag == true)
                        {
                            DataTable dtq = CommonClasses.Execute("SELECT DCD_ORD_QTY,DCD_I_CODE FROM DELIVERY_CHALLAN_DETAIL where DCD_DCM_CODE=" + Convert.ToInt32(cpom_code) + " ");

                            for (int i = 0; i < dtq.Rows.Count; i++)
                            {
                                CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["DCD_ORD_QTY"] + " where I_CODE='" + dtq.Rows[i]["DCD_I_CODE"] + "'");
                                CommonClasses.Execute1("DELETE FROM CHALLAN_STOCK_LEDGER WHERE CL_DOC_ID='" + Convert.ToInt32(cpom_code) + "'   AND CL_DOC_TYPE='DCTOUT'  AND CL_I_CODE='" + dtq.Rows[i]["DCD_I_CODE"] + "'");
                                CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(cpom_code) + "' and STL_DOC_TYPE='DCTOUT'   AND STL_I_CODE='" + dtq.Rows[i]["DCD_I_CODE"] + "'");
                            }
                            CommonClasses.WriteLog("Tray  Delivery Challan", "Delete", "Delivery Challan", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
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
                    Response.Redirect("~/Transactions/ADD/TrayDelivery_Challan.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                    string cpom_code = e.CommandArgument.ToString();
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        if (CommonClasses.CheckUsedInTran("CHALLAN_STOCK_LEDGER", "CL_DOC_ID", " AND CL_DOC_TYPE='DCTOUT' AND CL_CON_QTY>0", cpom_code))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record not Modify, it is used in Tray Delivery challan Return";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {
                            string type = "MODIFY";
                            int cnt = 0;
                            Response.Redirect("~/Transactions/ADD/TrayDelivery_Challan.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/DeliveryChallan.aspx?cpom_code=" + cpom_code + "&print_type=" + type1 + "&type=0", false);
                }
                else
                {
                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("Print1"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/DeliveryChallan.aspx?cpom_code=" + cpom_code + "&print_type=" + type1 + "&type=1", false);
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
            CommonClasses.SendError("Delivery Challan", "dgDetailDC_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from DELIVERY_CHALLAN_MASTER where DCM_CODE=" + PrimaryKey + "  ");
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
            LoadDeliveryChallan();
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
