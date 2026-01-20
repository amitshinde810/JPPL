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

public partial class Transactions_VIEW_ViewPurchaseRequisition : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    PurchaseRequisition_BL PerReq_BL= null;
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='54'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadDetail();
                    
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Reqisition ", "Page_Load", Ex.Message);
        }


    }
    #endregion

    #region LoadDetail
    private void LoadDetail()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select PRM_CODE,cast(PRM_NO as numeric(10,0)) as PRM_NO,PRM_I_CODE,convert(varchar,PRM_DATE,106) as PRM_DATE,I_NAME from PRUCHASE_REQUISITION_MASTER,ITEM_MASTER where PRUCHASE_REQUISITION_MASTER.ES_DELETE=0 and I_CODE=PRM_I_CODE  and  PRM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by PRM_NO DESC");
            if (dt.Rows.Count == 0)
            {
                if (dgPurchaseRequsition.Rows.Count == 0)
                {
                    dtFilter.Clear();
                    dgPurchaseRequsition.Enabled = false;
                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("PRM_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PRM_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PRM_DATE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PRM_I_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));

                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgPurchaseRequsition.DataSource = dtFilter;
                        dgPurchaseRequsition.DataBind();
                    }
                }
            }
            else
            {
                dgPurchaseRequsition.Enabled = true;
                dgPurchaseRequsition.DataSource = dt;
                dgPurchaseRequsition.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requsition", "LoadDetail", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from PRUCHASE_REQUISITION_MASTER where PRM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
  
                    // ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ModifyLog", Ex.Message);
        }

        return false;
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
                Response.Redirect("~/Transactions/ADD/PurchaseRequisition.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
  
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region dgPurchaseRequsition_PageIndexChanging
    protected void dgPurchaseRequsition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgPurchaseRequsition.PageIndex = e.NewPageIndex;
            LoadStatus(txtString);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Reqisition", "dgPurchaseRequsition_PageIndexChanging", Ex.Message);
        }

    }
    #endregion

    #region dgPurchaseRequsition_RowCommand
    protected void dgPurchaseRequsition_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string p_order = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/PurchaseRequisition.aspx?c_name=" + type + "&p_order=" + p_order, false);
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
            else if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string type = "MODIFY";
                        string cpom_code = e.CommandArgument.ToString();

                        Response.Redirect("~/Transactions/ADD/PurchaseRequisition.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);

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
           else if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string inv_code = e.CommandArgument.ToString();
                    int inv_code1 = Convert.ToInt32(inv_code);
                    //Code = inv_code;
                    //popUpPanel5.Visible = true;
                    //ModalPopupPrintSelection.Show();
                    Response.Redirect("~/RoportForms/ADD/PurchaseRequisitionPrint.aspx?cpom_code=" + inv_code1, false);

                    return;
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
                // Response.Redirect("~/RoportForms/ADD/ExportInvoicePrint.aspx?inv_code=" + inv_code, false);

            }



        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Reqisition", "dgPurchaseRequsition_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgPurchaseRequsition_RowDeleting
    protected void dgPurchaseRequsition_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgPurchaseRequsition.Rows[e.RowIndex].FindControl("lblPRM_CODE"))).Text))
                {
                    string p_order = ((Label)(dgPurchaseRequsition.Rows[e.RowIndex].FindControl("lblPRM_CODE"))).Text;
                    PerReq_BL = new PurchaseRequisition_BL();
                    PerReq_BL.PRM_CODE = Convert.ToInt32(p_order);
                    if (PerReq_BL.Delete())
                    {
                        CommonClasses.WriteLog("Purchase Requisition", "Delete", "Purchase Requisition", "", Convert.ToInt32(p_order), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
  
                        LoadDetail();
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Deleted..";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
  
                    }
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You have no rights to delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
  
                //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "dgPurchaseRequsition_RowDeleting", Ex.Message);
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
            CommonClasses.SendError("Purchase Requisition", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("select PRM_CODE,cast(PRM_NO as numeric(10,0)) as PRM_NO,PRM_I_CODE,convert(varchar,PRM_DATE,106) as PRM_DATE,I_NAME from PRUCHASE_REQUISITION_MASTER,ITEM_MASTER where PRUCHASE_REQUISITION_MASTER.ES_DELETE=0 and I_CODE=PRM_I_CODE  and  PRM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and (PRM_NO like upper('%" + str + "%') OR convert(varchar,PRM_DATE,106) like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%')) order by PRM_NO DESC");
            else
                dtfilter = CommonClasses.Execute("select PRM_CODE,cast(PRM_NO as numeric(10,0)) as PRM_NO,PRM_I_CODE,convert(varchar,PRM_DATE,106) as PRM_DATE,I_NAME from PRUCHASE_REQUISITION_MASTER,ITEM_MASTER where PRUCHASE_REQUISITION_MASTER.ES_DELETE=0 and I_CODE=PRM_I_CODE  and  PRM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by PRM_NO DESC");

            if (dtfilter.Rows.Count > 0)
            {
                dgPurchaseRequsition.Enabled = true;
                dgPurchaseRequsition.DataSource = dtfilter;
                dgPurchaseRequsition.DataBind();
            }
            else
            {

                dtFilter.Clear();
                dgPurchaseRequsition.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("PRM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PRM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PRM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PRM_I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgPurchaseRequsition.DataSource = dtFilter;
                    dgPurchaseRequsition.DataBind();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase  Requisition", "LoadStatus", ex.Message);
        }
    }

    #endregion
    
}
