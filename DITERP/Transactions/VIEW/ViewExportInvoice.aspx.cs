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

public partial class Transactions_VIEW_ViewExportInvoice : System.Web.UI.Page
{

    #region Variable
    static string right = "";   
    DataTable dtFilter = new DataTable();
    static string PrintType = "";
    static string Code = "";
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

                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='20'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    Session["InvoiceType"] = "Export";
                    LoadStatus(txtString);
                    //LoadInvoice();
                    //if (dgInvoiceDettail.Rows.Count == 0)
                    //{
                    //    dtFilter.Clear();

                    //    if (dtFilter.Columns.Count == 0)
                    //    {
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    //        dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));

                    //        dtFilter.Rows.Add(dtFilter.NewRow());
                    //        dgInvoiceDettail.DataSource = dtFilter;
                    //        dgInvoiceDettail.DataBind();
                    //        dgInvoiceDettail.Enabled = false;
                    //    }
                    //}
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice View", "Page_Load", Ex.Message);
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
            if ((string)Session["InvoiceType"] == "Export")
            {
                if (txtString.Text != "")
                    dtfilter = CommonClasses.Execute("SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and (INM_NO like upper('%" + str + "%') OR convert(varchar,INM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) and INM_INVOICE_TYPE=2 order by INM_NO DESC");
                else
                    dtfilter = CommonClasses.Execute("SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and INM_INVOICE_TYPE=2 order by INM_NO DESC");
            }
            else
            {
                if (txtString.Text != "")
                    dtfilter = CommonClasses.Execute("SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and (INM_NO like upper('%" + str + "%') OR INM_DATE like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) and INM_INVOICE_TYPE=3 order by INM_NO DESC");
                else
                    dtfilter = CommonClasses.Execute("SELECT INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and INM_INVOICE_TYPE=3 order by INM_NO DESC");
            }          

            if (dtfilter.Rows.Count > 0)
            {
                dgInvoiceDettail.DataSource = dtfilter;
                dgInvoiceDettail.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("INM_DATE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgInvoiceDettail.DataSource = dtFilter;
                    dgInvoiceDettail.DataBind();
                    dgInvoiceDettail.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Export Invoice View", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region Filter
    protected void Export_Click(object sender, EventArgs e)
    {
        Session["InvoiceType"] = "Export";
        LoadStatus(txtString);
    }
     #endregion

    #region Filter
    protected void Proforma_Click(object sender, EventArgs e)
    {
        Session["InvoiceType"] = "Proforma";
        LoadStatus(txtString);
    }
    #endregion


    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (rbtType.SelectedIndex == 0)
            {
                PrintType = "ExpInv";
            }
            if (rbtType.SelectedIndex == 1)
            {
                PrintType = "ExpInvDom";
            }
            if (rbtType.SelectedIndex == 2)
            {
                PrintType = "AreInv";
            }
            if (rbtType.SelectedIndex == 3)
            {
                PrintType = "PackList";
            }
            if(rbtType.SelectedIndex==4)
            {
                PrintType = "ExpInvIndA";
            }
            if(rbtType.SelectedIndex==5)
            {
                PrintType = "ARE1Declaration";
            }
            if(rbtType.SelectedIndex==6)
            {
                PrintType = "Eximination";
            }
            if (rbtType.SelectedIndex == 7)
            {
                PrintType = "EximinationAdditionalReport";
            }
            if (rbtType.SelectedIndex == 8)
            {
                PrintType = "DangerousGoodsDeclaration";
            }
            if (rbtType.SelectedIndex == 9)
            {
                PrintType = "FormSDF";
            }
            if (rbtType.SelectedIndex == 10)
            {
                PrintType = "ANNEXURE C-1";
            }
            if (rbtType.SelectedIndex == 11)
            {
                PrintType = "PROFORMA INVOICE";
            }
            if (rbtType.SelectedIndex == 12)
            {
                PrintType = "AUTHORITYLETTER";
            }
            Response.Redirect("~/RoportForms/ADD/ExportInvoicePrint.aspx?qe_code=" + Code + "&Print_Type=" + PrintType, false);
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Export Invoice View", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadInvoice
    private void LoadInvoice()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select P_NAME,INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE from INVOICE_MASTER,PARTY_MASTER where INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE  and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_INVOICE_TYPE!=1 order by INM_NO DESC");
            dgInvoiceDettail.DataSource = dt;
            dgInvoiceDettail.DataBind();
            if (dgInvoiceDettail.Rows.Count > 0)
            {
                dgInvoiceDettail.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice View", "LoadInvoice", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
           
            DataTable dt = CommonClasses.Execute("select MODIFY from INVOICE_MASTER where INM_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("Export Invoice View", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region  txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice View", "txtString_TextChanged", Ex.Message);
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
            Response.Redirect("~/Transactions/ADD/ExportInvoice.aspx?c_name=" + type, false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice View", "btnAddNew_Click", Ex.Message);
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
            CommonClasses.SendError("Export Invoice", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceDettail_PageIndexChanging
    protected void dgInvoiceDettail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInvoiceDettail.PageIndex = e.NewPageIndex;
            LoadInvoice();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgInvoiceDettail_RowCommand
    protected void dgInvoiceDettail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                        string type = "VIEW";
                        string inv_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Transactions/ADD/ExportInvoice.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
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
                        string inv_code = e.CommandArgument.ToString();

                        Response.Redirect("~/Transactions/ADD/ExportInvoice.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Is Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                    return;
                }
            }
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    string inv_code = e.CommandArgument.ToString();
                    Code = inv_code;
                    popUpPanel5.Visible = true;
                    ModalPopupPrintSelection.Show();
                   
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
            CommonClasses.SendError("Export Invoice View", "dgInvoiceDettail_RowCommand", Ex.Message);
        }
    }
    #endregion 

    #region dgInvoiceDettail_RowDeleting
    protected void dgInvoiceDettail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

         if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
         {
            if (!ModifyLog(((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text))
            {

                try
                {
                    string exp_Inv_Code = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text;

                    bool flag = CommonClasses.Execute1("UPDATE INVOICE_MASTER SET ES_DELETE = 1 WHERE INM_CODE='" + Convert.ToInt32(exp_Inv_Code) + "'");
                    if (flag == true)
                    {
                        DataTable dt = CommonClasses.Execute("select isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT,INM_INVOICE_TYPE from INVOICE_MASTER where INM_CODE='" + Convert.ToInt32(exp_Inv_Code) + "'");
                         if (dt.Rows.Count > 0)
                         {
                             if (Convert.ToBoolean(dt.Rows[0]["INM_IS_SUPPLIMENT"]) || Convert.ToInt32(dt.Rows[0]["INM_INVOICE_TYPE"])==3)
                             {

                             }
                             else
                             {
                                 DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + exp_Inv_Code + " ");
                                 for (int i = 0; i < dtq.Rows.Count; i++)
                                 {
                                     CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where CPOD_CPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and CPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                     CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");

                                 }
                                 flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + exp_Inv_Code + "' and STL_DOC_TYPE='EXPINV'");
                             }
                         }
                        CommonClasses.WriteLog("Export Invoice", "Delete", "Export Invoice", exp_Inv_Code, Convert.ToInt32(exp_Inv_Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
       
                        LoadInvoice();
                    }               

                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Export Invoice", "dgInvoiceDettail_RowDeleting", Ex.Message);
                }
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
    #endregion
   
}
