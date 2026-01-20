using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_VIEW_ViewLabourChargeInvoiceNew : System.Web.UI.Page
{
    #region Variable
    static string right = "";
    BillPassing_BL billPassing_BL = null;
    DataTable dtFilter = new DataTable();
    static string Code = "";
    static string type = "";
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
                    //for plain print
                    ddlPrintOpt.SelectedValue = "2";
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='19'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadInv();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward -View", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region dgInvoice_PageIndexChanging
    protected void dgInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInvoice.PageIndex = e.NewPageIndex;
            LoadInv();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Labour Inv New - View", "dgInvoice_PageIndexChanging", Ex.Message);
        }
    }
    #endregion

    #region LoadInv
    private void LoadInv()
    {
        try
        {
            DataTable dt = new DataTable();

            string str = "";
            str = CommonClasses.ToLiteral(txtString.Text.Trim());

            if (txtString.Text != "")
                //dt = CommonClasses.Execute("select LIM_CODE, CONVERT(VARCHAR,LIM_NO)+'/'+ SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 ) AS  LIM_NO ,LIM_P_CODE,convert(varchar,LIM_DATE,106) as LIM_DATE,P_NAME from LABOR_INVOICE_MASTER,PARTY_MASTER where LABOR_INVOICE_MASTER.ES_DELETE=0 and P_CODE=LIM_P_CODE and LIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by LIM_CODE DESC");
                dt = CommonClasses.Execute("select LIM_CODE,  CONVERT(VARCHAR,LIM_NO)+'/'+ SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 ) AS  LIM_NO ,LIM_P_CODE,convert(varchar,LIM_DATE,106) as LIM_DATE,P_NAME from LABOR_INVOICE_MASTER,PARTY_MASTER where LABOR_INVOICE_MASTER.ES_DELETE=0 and P_CODE=LIM_P_CODE   and  LIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND ('P'+''+(upper(CONVERT(VARCHAR,LIM_NO)+'/'+ SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 ))) like upper('%" + str + "%') OR upper(convert(varchar,LIM_DATE,106)) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%')) order by LIM_CODE DESC");
            else
                dt = CommonClasses.Execute("select LIM_CODE, CONVERT(VARCHAR,LIM_NO)+'/'+ SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 ) AS  LIM_NO ,LIM_P_CODE,convert(varchar,LIM_DATE,106) as LIM_DATE,P_NAME from LABOR_INVOICE_MASTER,PARTY_MASTER where LABOR_INVOICE_MASTER.ES_DELETE=0 and P_CODE=LIM_P_CODE  and  LIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by LIM_CODE DESC");

            if (dt.Rows.Count == 0)
            {
                //if (dgInvoice.Rows.Count == 0)
                //{
                dtFilter.Clear();
                dgInvoice.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgInvoice.DataSource = dtFilter;
                    dgInvoice.DataBind();
                }
                //}
            }
            else
            {
                dgInvoice.Enabled = true;
                dgInvoice.DataSource = dt;
                dgInvoice.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Labour Inv New", "LoadInv", Ex.Message);
        }
    }
    #endregion

    #region dgInvoice_RowCommand
    protected void dgInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        Response.Redirect("~/Transactions/ADD/LabourChargeInvoiceNew.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
                        Response.Redirect("~/Transactions/ADD/LabourChargeInvoiceNew.aspx?c_name=" + type + "&cpom_code=" + cpom_code, false);
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
            if (e.CommandName.Equals("Print"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {
                        //string type = "MODIFY";
                        string inv_code = e.CommandArgument.ToString();
                        Code = inv_code;
                        type = "Single";
                        popUpPanel5.Visible = true;
                        ModalPopupPrintSelection.Show();
                        return;
                        //  Response.Redirect("~/RoportForms/VIEW/ViewLabourChargeInvoice.aspx?inv_code=" + inv_code + "&type=" + type, false);
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
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Labour Inv New-View", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInvoice_RowDeleting
    protected void dgInvoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgInvoice.Rows[e.RowIndex].FindControl("lblLIM_CODE"))).Text))
                {
                    string cpom_code = ((Label)(dgInvoice.Rows[e.RowIndex].FindControl("lblLIM_CODE"))).Text;

                    if (CommonClasses.Execute1("update LABOR_INVOICE_MASTER set ES_DELETE=1 where LIM_CODE=" + cpom_code))
                    {
                        DataTable dtDetail = CommonClasses.Execute("SELECT PARTY_MASTER.P_NAME, LABOR_INVOICE_MASTER.LIM_P_CODE AS INM_P_CODE, LABOR_INVOICE_DETAIL.LID_I_CODE AS I_CODE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, LABOR_INVOICE_DETAIL.LID_INM_CODE AS INM_CODE, LABOR_INVOICE_DETAIL.LID_INM_NO AS INM_NO, LABOR_INVOICE_DETAIL.LID_INM_DATE AS INM_DATE, LABOR_INVOICE_DETAIL.LID_QTY AS IND_INQTY, LABOR_INVOICE_DETAIL.LID_RATE AS IND_RATE, LABOR_INVOICE_DETAIL.LID_AMT AS IND_AMT, LABOR_INVOICE_DETAIL.LID_CGST_AMT AS IND_EX_AMT, LABOR_INVOICE_DETAIL.LID_SGST_AMT AS IND_E_CESS_AMT, LABOR_INVOICE_DETAIL.LID_IGST_AMT AS IND_SH_CESS_AMT, LABOR_INVOICE_DETAIL.LID_TOTAL AS Total,E_TARIFF_NO,E_CODE FROM LABOR_INVOICE_DETAIL INNER JOIN LABOR_INVOICE_MASTER ON LABOR_INVOICE_DETAIL.LID_LIM_CODE = LABOR_INVOICE_MASTER.LIM_CODE INNER JOIN PARTY_MASTER ON LABOR_INVOICE_MASTER.LIM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN EXCISE_TARIFF_MASTER ON LABOR_INVOICE_DETAIL.LID_SAC_CODE = EXCISE_TARIFF_MASTER.E_CODE INNER JOIN ITEM_MASTER ON LABOR_INVOICE_DETAIL.LID_I_CODE = ITEM_MASTER.I_CODE where   PARTY_MASTER.ES_DELETE=0 AND EXCISE_TARIFF_MASTER.ES_DELETE=0 AND LIM_CODE=" + cpom_code);
                        if (dtDetail.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                CommonClasses.Execute("Update INVOICE_DETAIL set IND_LID_FLAG=0 WHERE IND_I_CODE='" + dr["I_CODE"].ToString() + "' AND IND_INM_CODE='" + dr["INM_CODE"].ToString() + "'");
                            }

                        }
                        CommonClasses.WriteLog("Labour charge inv New", "Delete", "Labour charge inv New", "", Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        LoadInv();
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
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Labour Inv New-View", "dgInvoice_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgInvoice_RowEditing
    protected void dgInvoice_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from LABOR_INVOICE_MASTER where LIM_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()))
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
            CommonClasses.SendError("Labour Inv New-View", "ModifyLog", Ex.Message);
        }

        return false;
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadInv();
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Labour Inv New", "txtString_TextChanged", Ex.Message);
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
                dtfilter = CommonClasses.Execute("With Result As(SELECT TOP 500 LIM_CODE,CASE WHEN LIM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,LIM_NO) AS  LIM_NO ,LIM_NO AS BillNO,LIM_P_CODE,LIM_INV_NO,convert(varchar,LIM_DATE,106) as LIM_DATE,Cast(CONVERT(DECIMAL(10,2),LIM_G_AMT) as nvarchar) AS LIM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=LIM_P_CODE  and  LIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ) Select * From Result where (upper(LIM_NO) like upper('%" + str + "%') OR convert(varchar,LIM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(LIM_INV_NO) like upper('%" + str + "%')OR upper(LIM_G_AMT) like upper('%" + str + "%')) Order By LIM_NO Desc");
            //dtfilter = CommonClasses.Execute("SELECT TOP 500 LIM_CODE,CASE WHEN LIM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,LIM_NO) AS  LIM_NO ,LIM_NO AS BillNO,LIM_P_CODE,LIM_INV_NO,convert(varchar,LIM_DATE,106) as LIM_DATE,Cast(CONVERT(DECIMAL(10,2),LIM_G_AMT) as nvarchar) AS LIM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=LIM_P_CODE  and  LIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and (upper(LIM_NO) like upper('%" + str + "%') OR convert(varchar,LIM_DATE,106) like upper('%" + str + "%') OR upper(P_NAME) like upper('%" + str + "%') OR upper(LIM_INV_NO) like upper('%" + str + "%')OR upper(LIM_G_AMT) like upper('%" + str + "%')) order by LIM_CODE DESC");
            else
                dtfilter = CommonClasses.Execute("SELECT TOP 500 LIM_CODE,CASE WHEN LIM_TYPE='OUTCUSTINV' THEN 'EXPV' ELSE'PV' END +'/'+SUBSTRING (CONVERT(varchar, DATEPART(YY,LIM_DATE)) ,3 , 2 )  +' '+SUBSTRING (CONVERT(varchar, DATEPART(YY,(DATEADD(YY,1,LIM_DATE)))) ,3 , 2 )+'/'+ CONVERT(VARCHAR,LIM_NO) AS  LIM_NO ,LIM_NO AS BillNO,LIM_P_CODE,LIM_INV_NO,convert(varchar,LIM_DATE,106) as LIM_DATE,Cast(CONVERT(DECIMAL(10,2),LIM_G_AMT) as nvarchar) AS LIM_G_AMT,P_NAME from BILL_PASSING_MASTER,PARTY_MASTER where BILL_PASSING_MASTER.ES_DELETE='0' and P_CODE=LIM_P_CODE  and  LIM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by LIM_CODE DESC ");
            if (dtfilter.Rows.Count > 0)
            {
                dgInvoice.Enabled = true;
                dgInvoice.DataSource = dtfilter;
                dgInvoice.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dgInvoice.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_DATE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("LIM_P_CODE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgInvoice.DataSource = dtFilter;
                    dgInvoice.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Labour Inv New", "LoadStatus", ex.Message);
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
                Response.Redirect("~/Transactions/ADD/LabourChargeInvoiceNew.aspx?c_name=" + type, false);
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
            CommonClasses.SendError("Labour Inv New-View", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/Add/SalesDefault.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Labour Inv New", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region chk1_CheckedChanged
    protected void chk1_CheckedChanged(object sender, EventArgs e)
    {
        ModalPopupPrintSelection.Show();
    }
    #endregion chk1_CheckedChanged

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPrintOpt.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select print Option";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ModalPopupPrintSelection.Show();
                return;
            }
            if (type == "Single")
            {
                Response.Redirect("~/RoportForms/ADD/LabourChargeInvoicePrint.aspx?Title=" + Title + "&Cond=" + ddlPrintOpt.SelectedValue + "&chkPrint4=" + rbtPrintCopyList.SelectedValue + "&code=" + Code + "&type=" + type + "&Supp=" + chlSupp.Checked + "&FrmDelieveryChallanFlag=1", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Invoice Print ", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlPrintOpt_SelectedIndexChanged
    protected void ddlPrintOpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupPrintSelection.Show();
        return;
    }
    #endregion ddlPrintOpt_SelectedIndexChanged
}
