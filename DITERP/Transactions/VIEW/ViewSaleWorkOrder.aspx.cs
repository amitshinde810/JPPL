using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Transactions_VIEW_ViewSaleWorkOrder : System.Web.UI.Page
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
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='94'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadWorkOrder();
                    if (dgDetailPO.Rows.Count == 0)
                    {
                        dtFilter.Clear();

                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("WO_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("WO_NO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("WO_DATE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_PONO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                  
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgDetailPO.DataSource = dtFilter;
                            dgDetailPO.DataBind();
                            dgDetailPO.Enabled = false;
                        }
                    }
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Work Order", "Page_Load", Ex.Message);
        }

    }
    #endregion

    #region LoadWorkOrder
    private void LoadWorkOrder()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select WO_CODE,WO_NO,convert(varchar,WO_DATE,106) as WO_DATE,CPOM_PONO,P_NAME from WORK_ORDER_MASTER ,PARTY_MASTER,CUSTPO_MASTER where WORK_ORDER_MASTER.ES_DELETE=0 and WO_P_CODE=P_CODE and  WO_CPOM_CODE=CPOM_CODE  and WO_CM_COMP_CODE=" + (string)Session["CompanyId"] + " order by WO_NO DESC ");
            dgDetailPO.DataSource = dt;
            dgDetailPO.DataBind();
            if (dgDetailPO.Rows.Count > 0)
            {
                dgDetailPO.Enabled = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Work Order", "LoadWorkOrder", Ex.Message);
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
            CommonClasses.SendError("Work Order", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select WO_CODE,WO_NO,convert(varchar,WO_DATE,106) as WO_DATE,CPOM_PONO,P_NAME from WORK_ORDER_MASTER ,PARTY_MASTER,CUSTPO_MASTER where WORK_ORDER_MASTER.ES_DELETE=0 and WO_P_CODE=P_CODE and  WO_CPOM_CODE=CPOM_CODE  and WO_CM_COMP_CODE=" + (string)Session["CompanyId"] + " and (WO_NO like upper('%" + str + "%') OR convert(varchar,WO_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(CPOM_PONO) like upper('%" + str + "%')) order by WO_NO desc");
            else
                dtfilter = CommonClasses.Execute("select WO_CODE,WO_NO,convert(varchar,WO_DATE,106) as WO_DATE,CPOM_PONO,P_NAME from WORK_ORDER_MASTER ,PARTY_MASTER,CUSTPO_MASTER where WORK_ORDER_MASTER.ES_DELETE=0 and WO_P_CODE=P_CODE and  WO_CPOM_CODE=CPOM_CODE  and WO_CM_COMP_CODE=" + (string)Session["CompanyId"] + " order by WO_NO DESC ");

            if (dtfilter.Rows.Count > 0)
            {
                dgDetailPO.DataSource = dtfilter;
                dgDetailPO.DataBind();
                dgDetailPO.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WO_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("CPOM_PONO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                  
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgDetailPO.DataSource = dtFilter;
                    dgDetailPO.DataBind();
                    dgDetailPO.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Work Order", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/SaleWorkOrder.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Work Order", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Work Order", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgDetailPO_RowDeleting
    protected void dgDetailPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblWO_CODE"))).Text))
            {

                try
                {
                    string cpom_code = ((Label)(dgDetailPO.Rows[e.RowIndex].FindControl("lblWO_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("BATCH_MASTER", "BT_WO_CODE", " and BATCH_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record not deleted, it is used in Batch Ticket";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "You can't delete this record, it is used in Work Order", CommonClasses.MSG_Warning);
                        return;
                    }
                    else
                    {
                    bool flag = CommonClasses.Execute1("UPDATE WORK_ORDER_MASTER SET ES_DELETE = 1 WHERE WO_CODE='" + Convert.ToInt32(cpom_code) + "'");
                        if (flag == true)
                        {
                            //DataTable dtOld = CommonClasses.Execute("select CPOD_CPOM_CODE,CPOD_I_CODE,I_CODENO,I_NAME,cast(CPOD_ORD_QTY as numeric(10,3)) as CPOD_ORD_QTY,cast(WOD_WORK_ORDER_QTY as numeric(10,3)) as WORK_ORD_QTY,cast(CPOD_WO_QTY as numeric(10,3)) as WORK_BAL_QTY from CUSTPO_DETAIL,ITEM_MASTER,CUSTPO_MASTER,WORK_ORDER_MASTER,WORK_ORDER_DETAIL where CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE=WOD_I_CODE and WOD_I_CODE=I_CODE and WO_CODE=WOD_WO_CODE and WOD_CPOM_CODE=CPOM_CODE and  WO_CODE='" + cpom_code + "' ");

                            //for (int i = 0; i < dtOld.Rows.Count; i++)
                            //{
                            //    CommonClasses.Execute("UPDATE CUSTPO_DETAIL set CPOD_WO_QTY = CPOD_WO_QTY - " + dtOld.Rows[i]["WORK_BAL_QTY"] + " where CPOD_CPOM_CODE='" + dtOld.Rows[i]["CPOD_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtOld.Rows[i]["CPOD_I_CODE"] + "'");
                            //}
                            //CommonClasses.Execute1("update CUSTPO_DETAIL set CPOD_IS_ORDER=0 ,CPOD_WO_CODE='' where CPOD_WO_CODE='" + Convert.ToInt32(cpom_code) + "'");
                                                   
                      
                            CommonClasses.WriteLog("Sales-Work Order", "Delete", "Sales-Work Order", cpom_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            LoadWorkOrder();
                          }

                        }
                       
                    }

                //}
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Work Order", "dgDetailPO_RowDeleting", Ex.Message);
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
            lblmsg.Text = "You have no rights to delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            return;
        }
    }
    #endregion

    #region dgDetailPO_RowCommand
    protected void dgDetailPO_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    Response.Redirect("~/Transactions/ADD/SaleWorkOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);


                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to view";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
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
                        Response.Redirect("~/Transactions/ADD/SaleWorkOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record used by another person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You have no rights to modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }
            if (e.CommandName.Equals("AMEND"))
            {

                if (!ModifyLog(e.CommandArgument.ToString()))
                {

                    string type = "AMEND";
                    string cpom_code = e.CommandArgument.ToString();
                    int cnt = 0;

                    Response.Redirect("~/Transactions/ADD/SaleWorkOrder.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

                    //}
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
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
                    Response.Redirect("~/RoportForms/ADD/SaleWorkOrder.aspx?cpom_code=" + cpom_code + "&print_type=" + type1, false);


                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To Print", CommonClasses.MSG_Erro);

                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            if (e.CommandName.Equals("PrintWork"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {

                    //string type = "MODIFY";
                    string cpom_code = e.CommandArgument.ToString();
                    string type1 = "PrintWork";
                    Response.Redirect("~/RoportForms/ADD/CustomerPORPTForm.aspx?cpom_code=" + cpom_code + "&print_type=" + type1, false);

                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To Print", CommonClasses.MSG_Erro);

                    lblmsg.Text = "You have no rights to print";
                    PanelMsg.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Work Order", "dgDetailPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from WORK_ORDER_MASTER where WO_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Work Order", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Work Order", "ShowMessage", Ex.Message);
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
            LoadWorkOrder();
        }
        catch (Exception)
        {
        }
    }
    #endregion
}
