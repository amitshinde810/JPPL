using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_VIEW_ViewTurningInward : System.Web.UI.Page
{
    #region Variable
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='114'");
                    right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                    LoadInward();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Turning Inward", "Page_Load", Ex.Message);
        }
    }

    #region LoadInward
    private void LoadInward()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT TOP 500 SM_CODE,SM_IWM_NO,convert(varchar,SM_IWM_DATE,106) as SM_IWM_DATE,P_NAME FROM SCRAP_MASTER,PARTY_MASTER WHERE SM_P_CODE=P_CODE AND SM_CM_CODE='" + Session["CompanyCode"].ToString() + "' AND SCRAP_MASTER.ES_DELETE=0 ORDER BY SM_CODE DESC");

            if (dt.Rows.Count == 0)
            {
                if (dgDetailPO.Rows.Count == 0)
                {
                    dgDetailPO.Enabled = false;
                    dtFilter.Clear();
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("SM_CODE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SM_IWM_NO", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("SM_IWM_DATE", typeof(string)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
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
            str = txtString.Text.Trim();

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
            {
                dtfilter = CommonClasses.Execute("SELECT TOP 500 SM_CODE,SM_IWM_NO,convert(varchar,SM_IWM_DATE,106) as SM_IWM_DATE,P_NAME FROM SCRAP_MASTER,PARTY_MASTER WHERE SCRAP_MASTER.ES_DELETE=0 and P_CODE=SM_P_CODE and SCRAP_MASTER.SM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  and (SM_IWM_NO like upper('%" + str + "%') OR CONVERT(VARCHAR, SM_IWM_DATE, 106) like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%')) order by SM_IWM_NO desc");
            }
            else
            {
                dtfilter = CommonClasses.Execute("select TOP 500 SM_CODE,SM_IWM_NO,convert(varchar,SM_IWM_DATE,106) as SM_IWM_DATE,P_NAME FROM SCRAP_MASTER,PARTY_MASTER where SCRAP_MASTER.ES_DELETE=0 and P_CODE=SM_P_CODE and SCRAP_MASTER.SM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND SCRAP_MASTER.ES_DELETE='0'  order by SM_IWM_NO desc ");
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
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_CODE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_IWM_NO", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("SM_IWM_DATE", typeof(string)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Turning Inward", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/TurningInward.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Turning Inward", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblSM_CODE"))).Text))
            {
                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblSM_CODE"))).Text;
                    bool flag = CommonClasses.Execute1("UPDATE SCRAP_MASTER SET ES_DELETE = 1 WHERE SM_CODE='" + Convert.ToInt32(cpom_code) + "'");
                    if (flag == true)
                    {
                        CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + cpom_code + "' and  STL_DOC_TYPE='TURIWD'");
                        DataTable dtinwDetail = CommonClasses.Execute("select SD_I_CODE,I_CODENO,I_NAME,SD_REV_QTY,I_UOM_NAME,SD_UOM_CODE from SCRAP_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE I_CODE=SD_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=SD_UOM_CODE AND SD_SM_CODE='" + cpom_code + "'");
                        DataTable dtPCode = CommonClasses.Execute("SELECT SM_P_CODE FROM SCRAP_MASTER WHERE SM_CODE='" + Convert.ToInt32(cpom_code) + "'");

                        for (int i = 0; i < dtinwDetail.Rows.Count; i++)
                        {
                            DataTable dtRevQty = CommonClasses.Execute("SELECT ROUND((IWD_TUR_QTY-IWD_REC_TUR_QTY),3) AS TUR_BAL,IWD_TUR_QTY,IWD_REC_TUR_QTY,IWD_CODE,IWM_CODE,IWD_IWM_CODE,IWD_I_CODE,IWM_NO,IWM_DATE,P_NAME,I_CODENO,I_NAME,IWD_REV_QTY,IWD_TUR_WEIGHT FROM INWARD_DETAIL,ITEM_MASTER,INWARD_MASTER,PARTY_MASTER WHERE IWD_I_CODE=I_CODE AND IWM_CODE=IWD_IWM_CODE AND P_CODE=IWM_P_CODE AND IWD_TUR_WEIGHT > 0 AND P_CODE='" + dtPCode.Rows[0]["SM_P_CODE"].ToString() + "' and ROUND(IWD_REC_TUR_QTY,3) >0 ORDER BY  IWM_CODE desc");

                            double TurQty = 0;
                            double TEmpQty = 0;
                            double inwUpdtedQty = 0;

                            TurQty = Convert.ToDouble(dtinwDetail.Rows[i]["SD_REV_QTY"].ToString());
                            CommonClasses.Execute("UPDATE ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + TurQty + ",I_RECEIPT_DATE='" + DateTime.Now.ToString("dd/MMM/yyyy") + "' where  I_CODE='" + dtinwDetail.Rows[i]["SD_I_CODE"].ToString() + "'");
                            TEmpQty = TurQty;

                            for (int j = 0; j < dtRevQty.Rows.Count; j++)
                            {
                                inwUpdtedQty = Convert.ToDouble(dtRevQty.Rows[j]["IWD_REC_TUR_QTY"].ToString());
                                if (TEmpQty > inwUpdtedQty)
                                {
                                    CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY- " + inwUpdtedQty + " WHERE IWD_CODE='" + dtRevQty.Rows[j]["IWD_CODE"].ToString() + "'");
                                    TEmpQty = TEmpQty - inwUpdtedQty;
                                }
                                else
                                {
                                    CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_REC_TUR_QTY = IWD_REC_TUR_QTY- " + TEmpQty + " WHERE IWD_CODE='" + dtRevQty.Rows[j]["IWD_CODE"].ToString() + "'");
                                    break;
                                }
                            }
                        }

                        LoadInward();

                        CommonClasses.WriteLog("Material Inward ", "Delete", "Material Inward", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record deleted successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Turning Inward", "dgDetailPO_RowDeleting", Ex.Message);
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
                        Response.Redirect("~/Transactions/ADD/TurningInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                        Response.Redirect("~/Transactions/ADD/TurningInward.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Turning Inward-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("SELECT MODIFY from SCRAP_MASTER where SM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Turning Inward Master-View", "ModifyLog", Ex.Message);
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
        Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
    }

}
