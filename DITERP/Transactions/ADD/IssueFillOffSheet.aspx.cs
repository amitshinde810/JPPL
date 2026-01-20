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
using System.Data.SqlClient;

public partial class Transactions_ADD_IssueFillOffSheet : System.Web.UI.Page
{
    # region Variables
    static int mlCode = 0;
    static string right = "";
    DataTable dtFilter = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFOSDetail = new DataTable();
    DataRow dr;
    # endregion

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
                    try
                    {
                        FillCombo();
                       
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            dt2.Rows.Clear();
                            txtDate.Attributes.Add("readonly", "readonly");
                            txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtFillDate.Attributes.Add("readonly", "readonly");
                            txtFillDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            LoadFilter();                           
                            // dgMainShade.Enabled = false;
                        }
                        DataTable dt = new DataTable();
                        dt = CommonClasses.Execute("select I_UOM_CODE from ITEM_UNIT_MASTER where  I_UOM_NAME like '%ltrs%' and I_UOM_CM_COMP_ID='"+Session["CompanyId"]+"' ");
                        if (dt.Rows.Count > 0)
                        {
                            ddlUnit.SelectedValue = dt.Rows[0]["I_UOM_CODE"].ToString();                          
                        }
                        else
                        {
                            ddlUnit.SelectedIndex = -1;                           
                        }
                        ddlCustomer.Enabled = false;
                        ddlFilterUnit.Enabled = false;
                        ddlbatchNo.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Issue Fill off Sheet", "Page_Load", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "Page_Load", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtDate.Attributes.Add("readonly", "readonly");
            txtFillDate.Attributes.Add("readonly", "readonly");
            DataTable dt = new DataTable();
            FillCombo();
            dt = CommonClasses.Execute("SELECT * FROM ISSUE_FILL_OFF_SHEET WHERE FOS_CODE=" + mlCode + " AND FOS_CM_CODE= " + (string)Session["CompanyCode"] + " and ES_DELETE=0");

            if (dt.Rows.Count > 0)
            {
               mlCode=Convert.ToInt32(dt.Rows[0]["FOS_CODE"]);
               ddlSheetNo.SelectedValue = dt.Rows[0]["FOS_SHEET_NO"].ToString();
               txtDocNo.Text = dt.Rows[0]["FOS_NO"].ToString();
                ddlbatchNo.SelectedValue = dt.Rows[0]["FOS_BT_CODE"].ToString();
                ddlBatchNo_SelectedIndexChanged(null, null);
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["FOS_DATE"]).ToString("dd MMM yyyy");
                txtApprovedBy.Text = dt.Rows[0]["FOS_APPROVED_BY"].ToString();
                txtSpecInstruction.Text = dt.Rows[0]["FOS_SPECIAL_INSTR"].ToString();
                txtFillDate.Text = Convert.ToDateTime(dt.Rows[0]["FOS_FILL_DATE"]).ToString("dd MMM yyyy");
                txtFillby.Text = dt.Rows[0]["FOS_FILL_BY"].ToString();
                txtFilterUsed.Text = dt.Rows[0]["FOS_FILTER_USED"].ToString();
                txtFinalyield.Text = dt.Rows[0]["FOS_FINAL_YIELD"].ToString();
                txtNotes.Text = dt.Rows[0]["FOS_NOTES"].ToString();
                ddlFilterUnit.SelectedValue = dt.Rows[0]["FOS_FILTER_UNIT"].ToString();
                dtFOSDetail = CommonClasses.Execute("select FOSD_FOS_CODE,FOSD_TYPE,cast(isnull(FOSD_QTY,0) as numeric(10,3)) as FOSD_QTY,cast(isnull(FOSD_ISSUE_QTY,0) as numeric(10,3)) as FOSD_ISSUE_QTY,cast(isnull(FOSD_WGT,0) as numeric(10,3)) as FOSD_WGT,FOSD_UOM_CODE as UOM_CODE,I_UOM_NAME as UOM_NAME,(CASE FOSD_TYPE WHEN 1 THEN 'Package' WHEN 2 THEN 'Extra' WHEN 3 THEN 'Sample' END) AS Type_Name From ISSUE_FILL_OFF_SHEET,ISSUE_FILL_OFF_SHEET_DETAIL,ITEM_UNIT_MASTER where FOS_CODE=FOSD_FOS_CODE AND FOSD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND ITEM_UNIT_MASTER.ES_DELETE=0 and FOS_CODE='" + mlCode + "' order by FOSD_TYPE");

                if (dtFOSDetail.Rows.Count != 0)
                {
                    dgvFillDetail.DataSource = dtFOSDetail;
                    dgvFillDetail.DataBind();
                    dgvFillDetail.Enabled = true;
                    dt2 = dtFOSDetail;                 
                }
                else
                {
                    dtFOSDetail.Rows.Add(dtFOSDetail.NewRow());
                    dgvFillDetail.DataSource = dtFOSDetail;
                    dgvFillDetail.DataBind();
                    dgvFillDetail.Enabled = false;
                }
                if (str == "VIEW")
                {
                    ddlSheetNo.Enabled = false;
                    ddlbatchNo.Enabled = false;
                    txtDate.Enabled = false;
                    txtApprovedBy.Enabled = false;
                    txtSpecInstruction.Enabled = false;
                    txtFillby.Enabled = false;
                    txtFillDate.Enabled = false;
                    txtFinalyield.Text = "";
                    ddlType.Enabled = false;
                    txtQty.Enabled = false;
                    txtBarrels.Enabled = false;
                    ddlUnit.Enabled = false;
                    txtNotes.Enabled = false;
                    dgvFillDetail.Enabled = false;
                    txtFilterUsed.Enabled = false;
                    ddlFilterUnit.Enabled = false;
                    btnSubmit.Visible = false;
                    txtFinalyield.Enabled = false;
                }
            }

            if (str == "MOD")
            {
                ddlSheetNo.Enabled = false;
                ddlbatchNo.Enabled = false;
                CommonClasses.SetModifyLock("ISSUE_FILL_OFF_SHEET", "MODIFY", "FOS_CODE", mlCode);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "ViewRec", ex.Message);
        }
    }
    #endregion ViewRec

    #region LoadFilter
    public void LoadFilter()
    {
        try
        {
            if (dgvFillDetail.Rows.Count == 0)
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Type_Name", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_ISSUE_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_WGT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UOM_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgvFillDetail.DataSource = dtFilter;
                    dgvFillDetail.DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "LoadFilter", ex.Message.ToString());
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
           // Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");

            if (txtBarrels.Text == "0.000")
            {
                ShowMessage("#Avisos", "The Field 'Barrels' Is Required", CommonClasses.MSG_Warning);                
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Enter Order Quantity";
                txtBarrels.Focus();
                return;
            }
            if (txtQty.Text == "0.00")
            {
                ShowMessage("#Avisos", "The Filed 'Quantity' Is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Enter Rate";
                txtQty.Focus();
                return;
            }
            if (dgvFillDetail.Enabled)
            {
                for (int i = 0; i < dgvFillDetail.Rows.Count; i++)
                {
                    string FOSD_TYPE = ((Label)(dgvFillDetail.Rows[i].FindControl("lblFOSD_TYPE"))).Text;
                    if (ItemUpdateIndex == "-1")
                    {
                        if (FOSD_TYPE == ddlType.SelectedValue.ToString())
                        {
                            ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            ddlType.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (FOSD_TYPE == ddlType.SelectedValue.ToString() && ItemUpdateIndex != i.ToString())
                        {
                            ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                                        
                            ddlType.Focus();
                            return;
                        }
                    }

                }
            }

            #region datatable structure
            if (dt2.Columns.Count == 0)
            {
                dt2.Columns.Add("FOSD_TYPE");
                dt2.Columns.Add("Type_Name");
                dt2.Columns.Add("FOSD_QTY");
                dt2.Columns.Add("FOSD_WGT");
                dt2.Columns.Add("UOM_CODE");
                dt2.Columns.Add("UOM_NAME");               

            }
            #endregion

            #region add control value to Dt
            dr = dt2.NewRow();
            dr["FOSD_TYPE"] = ddlType.SelectedValue;
            dr["Type_Name"] = ddlType.SelectedItem;
            dr["FOSD_QTY"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtBarrels.Text)), 3));
            dr["FOSD_WGT"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtQty.Text)), 3));
            dr["UOM_CODE"] = ddlUnit.SelectedValue;
            dr["UOM_NAME"] = ddlUnit.SelectedItem;

            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (dt2.Rows.Count > 0)
                {
                    dt2.Rows.RemoveAt(Index);
                    dt2.Rows.InsertAt(dr, Index);
                }
            }
            else
            {
                dt2.Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            dgvFillDetail.Visible = true;
            dgvFillDetail.DataSource = dt2;
            dgvFillDetail.DataBind();
            dgvFillDetail.Enabled = true;
            #endregion          
            clearDetail();
            ddlType.Focus();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "btnInsert_Click", Ex.Message);
        }

    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlbatchNo.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "The Field 'Batch No' Is Required", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlbatchNo.Focus();
            return;
        }
        if (txtDate.Text == "")
        {
            ShowMessage("#Avisos", "The Field 'Date' Is Required", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtDate.Focus();
            return;
        }
        if (txtFillDate.Text == "")
        {
            ShowMessage("#Avisos", "The Field 'Fill Date' Is Required", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtFillDate.Focus();
            return;
        }
        if (txtFinalyield.Text == "")
        {
            txtFinalyield.Text = "0.00";
        }
        SaveRec();
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {

            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();              
                    int Doc_no = 0;
                    DataTable dtNo = new DataTable();
                    dtNo = CommonClasses.Execute("Select isnull(max(FOS_NO),0) as FOS_NO FROM ISSUE_FILL_OFF_SHEET WHERE FOS_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
                    if (dtNo.Rows.Count > 0)
                    {
                        Doc_no = Convert.ToInt32(dtNo.Rows[0]["FOS_NO"]);
                        Doc_no = Doc_no + 1;
                    }
                    if (CommonClasses.Execute1("INSERT INTO ISSUE_FILL_OFF_SHEET VALUES('"+ddlSheetNo.SelectedValue+"','" + Doc_no + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlbatchNo.SelectedValue + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + txtApprovedBy.Text + "','" + txtSpecInstruction.Text + "','" + txtFillby.Text + "','" + Convert.ToDateTime(txtFillDate.Text).ToString("dd/MMM/yyyy") + "','" + txtNotes.Text + "','" + txtFilterUsed.Text + "','" + txtFinalyield.Text + "',0,0,'" + ddlFilterUnit.SelectedValue + "')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(FOS_CODE) from ISSUE_FILL_OFF_SHEET");
                        for (int i = 0; i < dgvFillDetail.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO ISSUE_FILL_OFF_SHEET_DETAIL (FOSD_FOS_CODE,FOSD_TYPE,FOSD_QTY,FOSD_WGT,FOSD_UOM_CODE,FOSD_ISSUE_QTY) values ('" + Code + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblFOSD_TYPE")).Text + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblFOSD_QTY")).Text + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblFOSD_WGT")).Text + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblUOM_CODE")).Text + "','" + ((TextBox)dgvFillDetail.Rows[i].FindControl("txtIssueQty")).Text + "')");
                        }
                        CommonClasses.WriteLog("Issue Fill off Sheet", "Save", "Issue Fill off Sheet", Convert.ToString(Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewIssueFillOffSheet.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                        ddlbatchNo.Focus();
                    }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT * FROM ISSUE_FILL_OFF_SHEET WHERE ES_DELETE=0 AND FOS_CODE!= '" + mlCode + "' AND lower(FOS_NO) = lower('" + txtDocNo.Text + "')");
                if (dt.Rows.Count == 0)
                {
                    if (CommonClasses.Execute1("UPDATE ISSUE_FILL_OFF_SHEET SET FOS_SHEET_NO='"+ddlSheetNo.SelectedValue+"',FOS_NO='" + txtDocNo.Text + "',FOS_BT_CODE='" + ddlbatchNo.SelectedValue.ToString() + "',FOS_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',FOS_APPROVED_BY='" + txtApprovedBy.Text + "',FOS_SPECIAL_INSTR='" + txtSpecInstruction.Text + "',FOS_FILL_BY='" + txtFillby.Text + "',FOS_FILL_DATE='" + Convert.ToDateTime(txtFillDate.Text).ToString("dd/MMM/yyyy") + "',FOS_NOTES='" + txtNotes.Text + "',FOS_FILTER_USED='" + txtFilterUsed.Text + "',FOS_FINAL_YIELD='" + txtFinalyield.Text + "',FOS_FILTER_UNIT='" + ddlFilterUnit.SelectedValue + "' where FOS_CODE='" + mlCode + "' "))
                    {
                        result = CommonClasses.Execute1("DELETE FROM ISSUE_FILL_OFF_SHEET_DETAIL WHERE FOSD_FOS_CODE='" + mlCode + "'");
                        if (result)
                        {
                            for (int i = 0; i < dgvFillDetail.Rows.Count; i++)
                            {
                                CommonClasses.Execute1("INSERT INTO ISSUE_FILL_OFF_SHEET_DETAIL (FOSD_FOS_CODE,FOSD_TYPE,FOSD_QTY,FOSD_WGT,FOSD_UOM_CODE,FOSD_ISSUE_QTY) values ('" + mlCode + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblFOSD_TYPE")).Text + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblFOSD_QTY")).Text + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblFOSD_WGT")).Text + "','" + ((Label)dgvFillDetail.Rows[i].FindControl("lblUOM_CODE")).Text + "','" + ((TextBox)dgvFillDetail.Rows[i].FindControl("txtIssueQty")).Text + "')");
                            }
                            CommonClasses.RemoveModifyLock("ISSUE_FILL_OFF_SHEET", "MODIFY", "FOS_CODE", mlCode);
                            CommonClasses.WriteLog("Issue Fill off Sheet", "Update", "Issue Fill off Sheet", txtDocNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                            result = true;
                        }
                        Response.Redirect("~/Transactions/VIEW/ViewIssueFillOffSheet.aspx", false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlbatchNo.Focus();
                    }
                }
                else
                {                   
                    ShowMessage("#Avisos", "Issue Fill off Sheet Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlbatchNo.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region FillCombo
    private void FillCombo()
    {
        try
        {
            CommonClasses.FillCombo("FILL_OFF_SHEET", "FOS_NO", "FOS_CODE", "ES_DELETE=0 AND FOS_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by FOS_NO desc", ddlSheetNo);
            ddlSheetNo.Items.Insert(0, new ListItem("Select Sheet NO", "0"));

           CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO desc", ddlbatchNo);
            ddlbatchNo.Items.Insert(0, new ListItem("Select Batch NO", "0"));

            CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlUnit);
            ddlUnit.Items.Insert(0, new ListItem("Select Unit", "0"));

            CommonClasses.FillCombo("PARTY_MASTER", "P_NAME", "P_CODE", "ES_DELETE=0 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlCustomer);
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));

            CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlFilterUnit);
            ddlFilterUnit.Items.Insert(0, new ListItem("Select Filter Unit", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "FillCombo", Ex.Message);
        }

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
            CommonClasses.SendError("Issue Fill off Sheet", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Cancel Button
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    // ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();

                }
                else
                {
                    CancelRecord();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ISSUE_FILL_OFF_SHEET", "MODIFY", "FOS_CODE", mlCode);
            }

            Response.Redirect("~/Transactions/VIEW/ViewIssueFillOffSheet.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlbatchNo.SelectedIndex ==0)
            {
                flag = false;
            }
            else if (txtDate.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtFillDate.Text.Trim() == "")
            {
                flag = false;
            }
            else
            {
                flag = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //SaveRec();
        CancelRecord();
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion
       
    #region dgvFillDetail_RowCommand
    protected void dgvFillDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgvFillDetail.Rows[Index];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;               
                dgvFillDetail.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);

                dgvFillDetail.DataSource = dt2;
                dgvFillDetail.DataBind();
                if (dgvFillDetail.Rows.Count == 0)
                {
                    dgvFillDetail.Enabled = false;
                    LoadFilter();
                }
                else
                {                   
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();

                ddlType.SelectedValue = ((Label)(row.FindControl("lblFOSD_TYPE"))).Text;
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblUOM_CODE"))).Text;
                txtBarrels.Text = ((Label)(row.FindControl("lblFOSD_QTY"))).Text;
                txtQty.Text = ((Label)(row.FindControl("lblFOSD_WGT"))).Text;              

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue Fill off Sheet", "dgvFillDetail_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgvFillDetail_Deleting
    protected void dgvFillDetail_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region ddlBatchNo_SelectedIndexChanged
    protected void ddlBatchNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlbatchNo.SelectedIndex != -1 && ddlbatchNo.SelectedIndex != 0)
            {                
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT  dbo.SHADE_MASTER.SHM_FORMULA_CODE, isnull(dbo.WORK_ORDER_MASTER.WO_P_CODE,0) as WO_P_CODE FROM  dbo.BATCH_MASTER INNER JOIN  dbo.SHADE_MASTER ON dbo.BATCH_MASTER.BT_SHM_CODE = dbo.SHADE_MASTER.SHM_CODE LEFT OUTER JOIN  dbo.WORK_ORDER_MASTER ON dbo.BATCH_MASTER.BT_WO_CODE = dbo.WORK_ORDER_MASTER.WO_CODE where dbo.BATCH_MASTER.BT_CODE='" + ddlbatchNo.SelectedValue + "'");

                if (dt.Rows.Count > 0)
                {
                    ddlCustomer.SelectedValue = dt.Rows[0]["WO_P_CODE"].ToString();
                    txtFormula.Text = dt.Rows[0]["SHM_FORMULA_CODE"].ToString();
                }
                else
                {
                    ddlCustomer.SelectedIndex = -1;
                    txtFormula.Text = "";
                }

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Fill of Sheet", "ddlBatchNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlSheetNo_SelectedIndexChanged
    protected void ddlSheetNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSheetNo.SelectedIndex != -1 && ddlSheetNo.SelectedIndex != 0)
            {
                DataTable dt = new DataTable();

                dt = CommonClasses.Execute("SELECT * FROM FILL_OFF_SHEET WHERE FOS_CODE=" + ddlSheetNo.SelectedValue + " AND FOS_CM_CODE= " + (string)Session["CompanyCode"] + " and ES_DELETE=0");

                if (dt.Rows.Count > 0)
                {
                    mlCode = Convert.ToInt32(dt.Rows[0]["FOS_CODE"]);
                    txtDocNo.Text = dt.Rows[0]["FOS_NO"].ToString();
                    ddlbatchNo.SelectedValue = dt.Rows[0]["FOS_BT_CODE"].ToString();
                    ddlBatchNo_SelectedIndexChanged(null, null);
                    txtDate.Text = Convert.ToDateTime(dt.Rows[0]["FOS_DATE"]).ToString("dd MMM yyyy");
                    txtApprovedBy.Text = dt.Rows[0]["FOS_APPROVED_BY"].ToString();
                    txtSpecInstruction.Text = dt.Rows[0]["FOS_SPECIAL_INSTR"].ToString();
                    txtFillDate.Text = Convert.ToDateTime(dt.Rows[0]["FOS_FILL_DATE"]).ToString("dd MMM yyyy");
                    txtFillby.Text = dt.Rows[0]["FOS_FILL_BY"].ToString();
                    txtFilterUsed.Text = dt.Rows[0]["FOS_FILTER_USED"].ToString();
                    txtFinalyield.Text = dt.Rows[0]["FOS_FINAL_YIELD"].ToString();
                    txtNotes.Text = dt.Rows[0]["FOS_NOTES"].ToString();
                    ddlFilterUnit.SelectedValue = dt.Rows[0]["FOS_FILTER_UNIT"].ToString();
                    dtFOSDetail = CommonClasses.Execute("select FOSD_FOS_CODE,FOSD_TYPE,cast(isnull(FOSD_QTY,0) as numeric(10,3)) as FOSD_QTY,0 AS FOSD_ISSUE_QTY,cast(isnull(FOSD_WGT,0) as numeric(10,3)) as FOSD_WGT,FOSD_UOM_CODE as UOM_CODE,I_UOM_NAME as UOM_NAME,(CASE FOSD_TYPE WHEN 1 THEN 'Package' WHEN 2 THEN 'Extra' WHEN 3 THEN 'Sample' END) AS Type_Name From FILL_OFF_SHEET,FILL_OFF_SHEET_DETAIL,ITEM_UNIT_MASTER where FOS_CODE=FOSD_FOS_CODE AND FOSD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND ITEM_UNIT_MASTER.ES_DELETE=0 and FOS_CODE='" + mlCode + "' order by FOSD_TYPE");

                    if (dtFOSDetail.Rows.Count != 0)
                    {
                        dgvFillDetail.DataSource = dtFOSDetail;
                        dgvFillDetail.DataBind();
                        dgvFillDetail.Enabled = true;
                        dt2 = dtFOSDetail;
                    }
                    else
                    {
                        dtFOSDetail.Rows.Add(dtFOSDetail.NewRow());
                        dgvFillDetail.DataSource = dtFOSDetail;
                        dgvFillDetail.DataBind();
                        dgvFillDetail.Enabled = false;
                        dt2.Rows.Clear();
                    }
                }
            }
            else
            {
                txtDocNo.Text = "";
                ddlbatchNo.SelectedIndex = -1;
                txtFormula.Text = "";
                ddlCustomer.SelectedIndex = -1;
                txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                txtApprovedBy.Text = "";
                txtSpecInstruction.Text = "";
                txtFillDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                txtFillby.Text = "";
                txtFilterUsed.Text = "";
                txtFinalyield.Text = "";
                txtNotes.Text = "";
                ddlFilterUnit.SelectedIndex = -1;

                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_TYPE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Type_Name", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_ISSUE_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("FOSD_WGT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("UOM_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgvFillDetail.DataSource = dtFilter;
                    dgvFillDetail.DataBind();

                }
                dgvFillDetail.Enabled = false;
                dt2.Rows.Clear();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Fill of Sheet", "ddlSheetNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlType.SelectedIndex = 0;
            txtBarrels.Text = "0.00";
            txtQty.Text = "0.00";           
            //ddlUnit.SelectedIndex = 0;           
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Fill Off Detail", "clearDetail", Ex.Message);
        }
    }
    #endregion

    protected void txtIssueQty1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox thisCheckBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int index = thisGridViewRow.RowIndex;

            double PTS_QTY = 0, Req_Qty = 0;
            if (Convert.ToString(((TextBox)dgvFillDetail.Rows[index].FindControl("txtIssueQty")).Text) != null || Convert.ToString(((TextBox)dgvFillDetail.Rows[index].FindControl("txtIssueQty")).Text) != "")
            {
                PTS_QTY = Math.Round((Convert.ToDouble(((TextBox)dgvFillDetail.Rows[index].FindControl("txtIssueQty")).Text)), 3);
            }
            if (Convert.ToString(((Label)dgvFillDetail.Rows[index].FindControl("lblFOSD_WGT")).Text) != null || Convert.ToString(((Label)dgvFillDetail.Rows[index].FindControl("lblFOSD_WGT")).Text) != "")
            {
                Req_Qty = Math.Round((Convert.ToDouble(((Label)dgvFillDetail.Rows[index].FindControl("lblFOSD_WGT")).Text)), 3);
            }
            if (PTS_QTY > Req_Qty)
            {
                ((TextBox)dgvFillDetail.Rows[index].FindControl("txtIssueQty")).Text = Req_Qty.ToString();
                //((TextBox)dgIssueTo.Rows[index].FindControl("txtIssueQty")).Focus();
                return;
            }           


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Issue To Production", "txtIssueQty_TextChanged", ex.Message.ToString());
        }
    }

}
