using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewMaterialInward : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region PageLoad
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='39'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadInward();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadInward
    private void LoadInward()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select TOP 500 IWM_CODE,IWM_P_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,P_NAME from INWARD_MASTER,PARTY_MASTER where INWARD_MASTER.ES_DELETE=0 and P_CODE=IWM_P_CODE  and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  and P_TYPE='2'  AND IWM_TYPE='IWIM'    order by IWM_CODE desc ");

            if (dt.Rows.Count == 0)
            {
                if (dgDetailPO.Rows.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IWM_P_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(string)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgDetailPO.DataSource = dtFilter;
                        dgDetailPO.DataBind();
                    }
                }
            }
            else
            {
                dgDetailPO.Enabled = true;
                dgDetailPO.DataSource = dt;
                dgDetailPO.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "LoadInward", Ex.Message);
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
            CommonClasses.SendError("Material Inward", "txtString_TextChanged", Ex.Message);
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

            if (txtString.Text != "")
            {
                dtfilter = CommonClasses.Execute("SELECT TOP 500 IWM_CODE,IWM_P_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME FROM INWARD_MASTER,PARTY_MASTER WHERE INWARD_MASTER.ES_DELETE=0 and P_CODE=IWM_P_CODE and INWARD_MASTER.IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INWARD_MASTER.ES_DELETE='0' and (IWM_NO like upper('%" + str + "%') OR CONVERT(VARCHAR, IWM_DATE, 106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR IWM_CHALLAN_NO like upper('%" + str + "%') OR IWM_CHAL_DATE like upper('%" + str + "%') OR IWM_P_CODE like upper('%" + str + "%'))   and P_TYPE='2' AND IWM_TYPE='IWIM'   order by IWM_CODE desc");
            }
            else
            {
                dtfilter = CommonClasses.Execute("select TOP 500 IWM_CODE,IWM_P_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_CHALLAN_NO,convert(varchar,IWM_CHAL_DATE,106) as IWM_CHAL_DATE,P_NAME FROM INWARD_MASTER,PARTY_MASTER where INWARD_MASTER.ES_DELETE=0 and P_CODE=IWM_P_CODE and INWARD_MASTER.IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INWARD_MASTER.ES_DELETE='0' and P_TYPE='2'  AND IWM_TYPE='IWIM'     order by IWM_CODE desc ");
            }
            if (dtfilter.Rows.Count > 0)
            {
                dgDetailPO.Enabled = true;
                dgDetailPO.DataSource = dtfilter;
                dgDetailPO.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_P_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("IWM_CHALLAN_NO", typeof(string)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inward", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/MaterialInward.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "you have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblIWM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblIWM_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_IWM_CODE", "AND ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record, it is used in Material Inspection";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE INWARD_MASTER SET ES_DELETE = 1 WHERE IWM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                        if (flag == true)
                        {
                            DataTable dtq = CommonClasses.Execute("SELECT IWD_REV_QTY,IWD_I_CODE,IWD_CPOM_CODE FROM INWARD_DETAIL where IWD_IWM_CODE=" + cpom_code + " ");

                            for (int i = 0; i < dtq.Rows.Count; i++)
                            {
                                CommonClasses.Execute("update SUPP_PO_DETAILS set SPOD_INW_QTY=SPOD_INW_QTY-" + dtq.Rows[i]["IWD_REV_QTY"] + " where SPOD_I_CODE='" + dtq.Rows[i]["IWD_I_CODE"] + "' and SPOD_SPOM_CODE='" + dtq.Rows[i]["IWD_CPOM_CODE"] + "'");
                                CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtq.Rows[i]["IWD_REV_QTY"] + " where I_CODE='" + dtq.Rows[i]["IWD_I_CODE"] + "'");
                            }

                            flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + cpom_code + "' and STL_DOC_TYPE='RAWINWARD'");
                            CommonClasses.WriteLog("Material Inward ", "Delete", "Material Inward", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record deleted successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Material Inward", "dgDetailPO_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record used by another person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadInward();
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

    #region dgDetailPO_RowEditing
    protected void dgDetailPO_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string cpom_code = ((Label)(dgDetailPO.Rows[e.NewEditIndex].FindControl("lblIWM_CODE"))).Text;
                string type = "MODIFY";

                if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_IWM_CODE", "AND ES_DELETE=0", cpom_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You can't delete this record, it is used in Material Inspection ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                else
                {
                    Response.Redirect("~/Transactions/ADD/MaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "dgPODetail_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowCommand
    protected void dgDetailPO_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string cpom_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/MaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                        string cpom_code = e.CommandArgument.ToString();

                        if (CommonClasses.CheckUsedInTran("INSPECTION_S_MASTER", "INSM_IWM_CODE", "AND ES_DELETE=0", cpom_code))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You can't Modify this record, it's Inspection already done";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {
                            Response.Redirect("~/Transactions/ADD/MaterialInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                    lblmsg.Text = "You have no rights to Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            else if (e.CommandName.Equals("Delete"))
            {

            }
            else if (e.CommandName.Equals("Print"))
            {
                string Type = "IWIM";
                string PrintType = "Single";
                string cpom_code = e.CommandArgument.ToString();
                Response.Redirect("~/RoportForms/ADD/RptInspectionGIN_IWIM.aspx?Title=" + Title + "&cpom_code=" + cpom_code + "&Type=" + Type + "&PType=" + PrintType, false);
            }
            else if (e.CommandName.Equals("PrintMult"))
            {
                string Type = "IWIM";
                string PrintType = "Mult";
                string cpom_code = e.CommandArgument.ToString();
                Response.Redirect("~/RoportForms/VIEW/ViewInwardReports.aspx?Title=" + Title + "&cpom_code=" + cpom_code + "&Type=" + Type + "&PType=" + PrintType, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from INWARD_MASTER where IWM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Inward Master-View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Material Inward -View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region dgDetailPO_PageIndexChanging
    protected void dgDetailPO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgDetailPO.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception)
        {
        }
    }
    #endregion

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
    }
}
