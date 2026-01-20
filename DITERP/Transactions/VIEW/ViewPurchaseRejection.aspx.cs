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


public partial class Transactions_VIEW_ViewPurchaseRejection : System.Web.UI.Page
{
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyCode"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='68'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();


                    LoadPurchaseRejection();
                    //if (dgPurchaseRejection.Rows.Count == 0)
                    //{
                    //    dtFilter.Clear();

                    //    if (dtFilter.Columns.Count == 0)
                    //    {
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_CODE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_NO", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_DATE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_TYPE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_CHALLAN_NO", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_CHALLAN_DATE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("PR_P_CODE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                    //        dtFilter.Rows.Add(dtFilter.NewRow());
                    //        dgPurchaseRejection.DataSource = dtFilter;
                    //        dgPurchaseRejection.DataBind();
                    //    }
                    //}
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "Page_Load", Ex.Message);
        }
    }

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadPurchaseRejection
    private void LoadPurchaseRejection()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select PR_CODE, PR_NO, convert(varchar,PR_DATE,106) as PR_DATE, PR_TYPE, PR_CHALLAN_NO, convert(varchar,PR_CHALLAN_DATE,106) as PR_CHALLAN_DATE, PR_P_CODE, P_NAME from PURCHASE_REJECTION_MASTER, PARTY_MASTER where PURCHASE_REJECTION_MASTER.ES_DELETE=0 AND PR_P_CODE=P_CODE and PR_COMP_CODE=" + (string)Session["CompanyCode"] + " order by PR_NO desc");
            if (dt.Rows.Count == 0)
            {
                if (dt.Rows.Count == 0)
                {
                    dtFilter.Clear();

                    if (dtFilter.Columns.Count == 0)
                    {
                        dgPurchaseRejection.Enabled = false;
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_TYPE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_CHALLAN_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_CHALLAN_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PR_P_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgPurchaseRejection.DataSource = dtFilter;
                        dgPurchaseRejection.DataBind();
                    }
                }
            }
            else
            {
                dgPurchaseRejection.Enabled = true;
                dgPurchaseRejection.DataSource = dt;
                dgPurchaseRejection.DataBind();

            }


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "LoadPurchaseRejection", Ex.Message);
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
            CommonClasses.SendError("Purchase Rejection", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select PR_CODE, PR_NO, convert(varchar,PR_DATE,106) as PR_DATE, PR_TYPE, PR_CHALLAN_NO, convert(varchar,PR_CHALLAN_DATE,106) as PR_CHALLAN_DATE, PR_P_CODE, P_NAME from PURCHASE_REJECTION_MASTER, PARTY_MASTER where PURCHASE_REJECTION_MASTER.ES_DELETE=0 AND PR_P_CODE=P_CODE and PR_COMP_CODE=" + (string)Session["CompanyCode"] + " and (P_NAME like upper('%" + str + "%') OR convert(varchar,PR_DATE,106) like upper('%" + str + "%') OR PR_NO like upper('%" + str + "%') OR PR_CHALLAN_NO like upper('%" + str + "%') OR convert(varchar,PR_CHALLAN_DATE,106) like upper('%" + str + "%') OR PR_P_CODE like upper('%" + str + "%')) order by PR_NO desc");
            else
                dtfilter = CommonClasses.Execute("select PR_CODE, PR_NO, convert(varchar,PR_DATE,106) as PR_DATE, PR_TYPE, PR_CHALLAN_NO, convert(varchar,PR_CHALLAN_DATE,106) as PR_CHALLAN_DATE, PR_P_CODE, P_NAME from PURCHASE_REJECTION_MASTER, PARTY_MASTER where PURCHASE_REJECTION_MASTER.ES_DELETE=0 AND PR_P_CODE=P_CODE and PR_COMP_CODE=" + (string)Session["CompanyCode"] + " order by PR_NO desc");

            if (dtfilter.Rows.Count > 0)
            {
                dgPurchaseRejection.Enabled = true;
                dgPurchaseRejection.DataSource = dtfilter;
                dgPurchaseRejection.DataBind();
            }
            else
            {

                dtFilter.Clear();
                dgPurchaseRejection.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_CHALLAN_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_CHALLAN_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PR_P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgPurchaseRejection.DataSource = dtFilter;
                    dgPurchaseRejection.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Rejection", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/PurchaseRejection.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Purchase Rejection", "btnAddNew_Click", Ex.Message);
        }
    }
    protected void dgPurchaseRejection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgPurchaseRejection.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "dgPurchaseRejection_PageIndexChanging", Ex.Message);
        }
    }
    protected void dgPurchaseRejection_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string PR_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/PurchaseRejection.aspx?c_name=" + type + "&PR_CODE=" + PR_CODE, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
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
                        string PR_CODE = e.CommandArgument.ToString();
                        int cnt = 0;
                        //DataTable dtcheck = CommonClasses.Execute("select CPOD_PR_CODE,CPOD_WO_FLAG from CUSTPO_DETAIL where CPOD_PR_CODE=" + PR_CODE + "");
                        //for (int n = 0; n < dtcheck.Rows.Count; n++)
                        //{
                        //    if (dtcheck.Rows[n]["CPOD_WO_FLAG"].ToString() == "True")
                        //    {
                        //        cnt = 1;
                        //    }
                        //    else
                        //    {
                        //        cnt = 0;
                        //    }
                        //}
                        //if (cnt == 1)
                        //{
                        //    ShowMessage("#Avisos", "Work Order for perticular Customer Order is generated, can't modify the record", CommonClasses.MSG_Erro);
                        //}
                        //else
                        //{
                        //    Response.Redirect("~/Transactions/ADD/CustomerPO.aspx?c_name=" + type + "&PR_CODE=" + PR_CODE, false);
                        //}

                        //if (CommonClasses.CheckUsedInTran("WORK_ORDER_MASTER", "WO_PR_CODE", "AND ES_DELETE=0", PR_CODE))
                        //{  PanelMsg.Visible = true;
                        // lblmsg.Text = "You can't modity this record, it is used in Work Order";

                        //    ShowMessage("#Avisos", "You can't modity this record, it is used in Work Order", CommonClasses.MSG_Warning);
                        //    return;
                        //}
                        //else
                        //{
                        Response.Redirect("~/Transactions/ADD/PurchaseRejection.aspx?c_name=" + type + "&PR_CODE=" + PR_CODE, false);

                        //}
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                //if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                //{
                if (!ModifyLog(e.CommandArgument.ToString()))
                {

                    //string type = "MODIFY";
                    string CR_CODE = e.CommandArgument.ToString();
                    //string type1 = "saleorder";
                    Response.Redirect("~/RoportForms/ADD/PurchaseRejectionPrint.aspx?CR_CODE=" + CR_CODE, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    // ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                    return;
                }

                //}
                //else
                //{
                //    ShowMessage("#Avisos", "You Have No Rights To Print", CommonClasses.MSG_Erro);
                //    return;
                //}
            }
          }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "GridView1_RowCommand", Ex.Message);
        }
    }

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from PURCHASE_REJECTION_MASTER where PR_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion
    protected void dgPurchaseRejection_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgPurchaseRejection.Rows[e.RowIndex].FindControl("lblPR_CODE"))).Text))
            {

                try
                {
                    string PR_CODE = ((Label)(dgPurchaseRejection.Rows[e.RowIndex].FindControl("lblPR_CODE"))).Text;

                    DataTable dtRejDet = CommonClasses.Execute("SELECT PRD_ORIGINAL_QTY,PRD_RECEIVED_QTY,PRD_I_CODE,PR_IWM_NO from PURCHASE_REJECTION_DETAIL,PURCHASE_REJECTION_MASTER where PRD_PR_CODE=PR_CODE and PRD_PR_CODE='" + PR_CODE + "'");

                    if (dtRejDet.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtRejDet.Rows.Count; k++)
                        {
                            if (dtRejDet.Rows[k]["PR_IWM_NO"].ToString()!="0" && dtRejDet.Rows[k]["PRD_ORIGINAL_QTY"].ToString() != "0.000")
                            {
                                double N1 = Convert.ToDouble(dtRejDet.Rows[k]["PRD_ORIGINAL_QTY"].ToString());
                                double N2 = Convert.ToDouble(dtRejDet.Rows[k]["PRD_RECEIVED_QTY"].ToString());
                                double N3 = N1 - N2;
                                CommonClasses.Execute1("update INWARD_DETAIL SET IWD_RETURN_QTY=IWD_RETURN_QTY+"+ N3 +" WHERE IWD_I_CODE IN (SELECT IWD_I_CODE FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE=IWD_IWM_CODE and IWM_NO=" + Convert.ToInt32(dtRejDet.Rows[k]["PR_IWM_NO"]) + " and IWD_I_CODE=" + dtRejDet.Rows[k]["PRD_I_CODE"] + ")");
                            }
                        }
                    }


                    bool flag = CommonClasses.Execute1("UPDATE PURCHASE_REJECTION_MASTER SET ES_DELETE = 1 WHERE PR_CODE='" + Convert.ToInt32(PR_CODE) + "'");
                    if (flag == true)
                    {
                        CommonClasses.WriteLog("PURCHASE_REJECTION_MASTER", "Delete", "PURCHASE_REJECTION_MASTER", PR_CODE, Convert.ToInt32(PR_CODE), Convert.ToInt32(Session["CompanyCode"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";

                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }
                
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("PURCHASE_REJECTION_MASTER", "dgPurchaseRejection_RowDeleting", Ex.Message);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another Person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                return;
            }

            LoadPurchaseRejection();
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            return;
        }
    }
    protected void dgPurchaseRejection_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {

                string PR_CODE = ((Label)(dgPurchaseRejection.Rows[e.NewEditIndex].FindControl("lblPR_CODE"))).Text;
                string type = "MODIFY";

                // if (CommonClasses.CheckUsedInTran("WORK_ORDER_MASTER", "WO_PR_CODE", "AND ES_DELETE=0", PR_CODE))
                // {
                //     PanelMsg.Visible = true;
                //     lblmsg.Text = "You Have No Rights To Delete";
                // ShowMessage("#Avisos", "You can't modify this record, it is used in Work Order", CommonClasses.MSG_Warning);
                // }
                // else
                // {
                Response.Redirect("~/Transactions/ADD/PurchaseRejection.aspx?c_name=" + type + "&PR_CODE=" + PR_CODE, false);
                //  }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Rejection", "dgCustomerMaster_RowEditing", Ex.Message);
        }
    }




}
