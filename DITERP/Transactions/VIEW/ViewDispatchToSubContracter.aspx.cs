using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_VIEW_ViewDispatchToSubContracter : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    DataTable dtFilter = new DataTable();
    public static int Index = 0;

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='128'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadInvoice();
                    if (dgInvoiceDettail.Rows.Count == 0)
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
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "Page_Load", Ex.Message);
        }
    }

    #region LoadInvoice
    private void LoadInvoice()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("select TOP 500 P_NAME,INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE from INVOICE_MASTER,PARTY_MASTER where INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE=P_CODE  and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_TYPE='OUTSUBINM' order by INM_CODE DESC");
            dgInvoiceDettail.DataSource = dt;
            dgInvoiceDettail.DataBind();
            if (dgInvoiceDettail.Rows.Count > 0)
            {
                dgInvoiceDettail.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "LoadInvoice", Ex.Message);
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Masters/ADD/ProductionDefault.aspx", false);
    }

    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        LoadStatus(txtString);
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Transactions/ADD/DispatchToSubCon.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Tax Invoice View", "btnAddNew_Click", Ex.Message);
        }
    }

    protected void dgInvoiceDettail_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    string inv_code = e.CommandArgument.ToString();
                    Response.Redirect("~/Transactions/ADD/DispatchToSubCon.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
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
                        Index = Convert.ToInt32(e.CommandArgument.ToString());
                        DataTable dtcheck = new DataTable();


                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        int RowIndex = row.RowIndex; // this find the index of row

                        string varName1 = ((Label)row.FindControl("lblINM_DATE")).Text; //this store the  value in varName1


                        dtcheck = CommonClasses.Execute(" SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_TYPE='IWIAP' AND CL_CH_NO IN (SELECT  CL_CH_NO  FROM CHALLAN_STOCK_LEDGER  WHERE CL_DOC_TYPE='OutSUBINM' AND  CL_DOC_ID='" + Index + "') AND CL_DATE='" + Convert.ToDateTime(varName1).ToString("dd/MMM/yyyy") + "'");
                        if (dtcheck.Rows.Count > 0)
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Used In Inward you can't Modify It.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                        else
                        {
                            string type = "MODIFY";
                            string inv_code = e.CommandArgument.ToString();
                            Response.Redirect("~/Transactions/ADD/DispatchToSubCon.aspx?c_name=" + type + "&inv_code=" + inv_code, false);
                        }
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
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        string inv_code = e.CommandArgument.ToString();
                        string type = "Single";
                        Response.Redirect("~/RoportForms/VIEW/ViewDispToSubReport.aspx?inv_code=" + inv_code + "&type=" + type, false);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Print";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (e.CommandName.Equals("PrintMult"))
            {
                if (!ModifyLog(e.CommandArgument.ToString()))
                {
                    string inv_code = e.CommandArgument.ToString();
                    string type = "Mult";
                    Response.Redirect("~/RoportForms/VIEW/ViewDispToSubReport.aspx?inv_code=" + inv_code + "&type=" + type, false);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DISPATCH TO SUB CONTRACTOR View", "dgInvoiceDettail_RowCommand", Ex.Message);
        }
    }

    protected void dgInvoiceDettail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text))
            {
                try
                {
                    DataTable dtcheck = new DataTable();
                    string inv_code = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_CODE"))).Text;
                    string inv_no = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_NO"))).Text;
                    string inv_Date = ((Label)(dgInvoiceDettail.Rows[e.RowIndex].FindControl("lblINM_DATE"))).Text;
                    dtcheck = CommonClasses.Execute(" SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_TYPE='IWIAP'  AND CL_DATE='" + Convert.ToDateTime(inv_Date).ToString("dd/MMM/yyyy") + "'  AND CL_CH_NO='" + inv_no + "'");
                    if (dtcheck.Rows.Count > 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Used In Inward you can't Delete It.";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE INVOICE_MASTER SET ES_DELETE = 1 WHERE INM_CODE='" + Convert.ToInt32(inv_code) + "'");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Dispatch To Sub Contractor", "Delete", "Dispatch To Sub Contractor", inv_no, Convert.ToInt32(inv_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE,IND_STORE_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + inv_code + " ");
                            for (int i = 0; i < dtq.Rows.Count; i++)
                            {
                                CommonClasses.Execute("update SUPP_PO_DETAILS set SPOD_DISPACH = SPOD_DISPACH - " + dtq.Rows[i]["IND_INQTY"] + " where SPOD_SPOM_CODE='" + dtq.Rows[i]["IND_CPOM_CODE"] + "' and SPOD_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                if (dtq.Rows[i]["IND_STORE_CODE"].ToString() == "-2147483642")
                                {
                                    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                }
                                else
                                {
                                    CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                                }
                            }
                            flag = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + inv_code + "' and STL_DOC_TYPE='OutSUBINM'");
                            flag = CommonClasses.Execute1("DELETE FROM CHALLAN_STOCK_LEDGER WHERE CL_DOC_ID = '" + inv_code + "' AND CL_DOC_TYPE='OutSUBINM'");

                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        LoadInvoice();
                    }
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Dispatch to Sub Contractor View", "dgInvoiceDettail_RowDeleting", Ex.Message);
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

    protected void dgInvoiceDettail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgInvoiceDettail.PageIndex = e.NewPageIndex;
        LoadStatus(txtString);
    }

    #region User Defined Method

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
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice View", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Tax Invoice View", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim().Replace("'", "\'");
            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT TOP 500 INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and (INM_NO like upper('%" + str + "%') OR convert(varchar,INM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) and INM_TYPE='OUTSUBINM' order by INM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 INM_CODE,INM_NO,convert(varchar,INM_DATE,106) as INM_DATE,P_NAME FROM INVOICE_MASTER,PARTY_MASTER WHERE INM_P_CODE=P_CODE and INVOICE_MASTER.INM_CM_CODE= '" + Convert.ToInt32(Session["CompanyCode"]) + "' AND INVOICE_MASTER.ES_DELETE='0' and INM_TYPE='OUTSUBINM' order by INM_CODE DESC");

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
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice View", "LoadStatus", ex.Message);
        }
    }
    #endregion

    #endregion
}
