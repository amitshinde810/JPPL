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
using System.Drawing;

public partial class Transactions_ADD_BatchTicket : System.Web.UI.Page
{
    #region Variable

    DataTable dt = new DataTable();
    DataTable dtBatch = new DataTable();
    static int mlCode = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    public static string str = "";  
    DataTable dtFilter = new DataTable();
    public static double totInKg = 0; 
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

                    try
                    {
                        LoadFormulaCode();                                                          
                        LoadICode();
                        LoadWorkOrder();

                        str = "";
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
                            LoadFilter();
                            txtBatchDate.Attributes.Add("readonly", "readonly");
                            txtBatchDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            dgMainShade.Enabled = false;
                        }
                        ddlBatchType.Focus();
                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Batch Ticket", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Batch Ticket", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            
                if (ddlBatchType.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Batch Type", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlBatchType.Focus();
                    return;
                }
               
                if (ddlBatchType.SelectedValue == "1" && ddlWorkOrderNo.SelectedIndex==0)
                {
                    ShowMessage("#Avisos", "Select Work Order No", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    ddlWorkOrderNo.Focus();
                    return;
                }
                if (ddlItemCode.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlItemCode.Focus();
                    return;
                }
                if (ddlFormulaType.SelectedIndex ==0)
                {
                    ShowMessage("#Avisos", "Select Formula Type", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlFormulaType.Focus();
                    return;
                }
                if (ddlFormulaCode.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Formula Code", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlFormulaCode.Focus();
                    return;
                }
                if (ddlBatchType.SelectedIndex == 1)
                {
                    if (Convert.ToDouble(txtTotalInLtr.Text) > Convert.ToDouble(txtWorkOrdQty.Text))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Batch Qty Can't Be Greater Than Work Order Qty";
                        //ShowMessage("#Avisos", "Batch Qty Can't Be Greater Than Work Order Qty", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        txtTotalInLtr.Focus();
                        return;
                    }
                }
            if (dgMainShade.Rows.Count > 0)
            {
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    int Doc_no = 0;
                    DataTable dt = new DataTable();
                    dt = CommonClasses.Execute("Select isnull(max(BT_NO),0) as BT_NO FROM BATCH_MASTER WHERE BT_CM_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
                    if (dt.Rows.Count > 0)
                    {
                        Doc_no = Convert.ToInt32(dt.Rows[0]["BT_NO"]);
                        Doc_no = Doc_no + 1;
                    }                   
                    txtAutoBatchNo.Text = Doc_no.ToString();
                    Panel1.Visible = true;
                    ModalPopupPrintSelection1.Show();
                }
                else
                {
                    SaveRec();     
                }
               

            }
            else
            {
                ShowMessage("#Avisos", "No Record !! Please Insert Record In Table", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                return;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "btnSubmit_Click", Ex.Message);

        }

    }

    #endregion btnSubmit_Click

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {

            if (ddlBatchType.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddlItemCode.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddlFormulaCode.SelectedIndex == 0)
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
            CommonClasses.SendError("Batch Ticket", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec(); 
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Batch Ticket", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region btnOkBatchNo_Click
    protected void btnOkBatchNo_Click(object sender, EventArgs e)
    {
        try
        {
            txtBatchNo.Text = txtAutoBatchNo.Text;
            DataTable dt = new DataTable();
                    dt = CommonClasses.Execute("Select BT_NO FROM BATCH_MASTER WHERE BT_CM_ID = " + (string)Session["CompanyId"] + " and BT_NO='"+txtBatchNo.Text+"' and ES_DELETE=0");
                    if (dt.Rows.Count > 0)
                    {
                        ShowMessage("#Avisos", "Batch Number Allready Exists", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtBatchNo.Focus();
                        return;
                        
                    }
                    else
                    {
                        SaveRec();   
                    }
                     
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Batch Ticket", "btnOkBatchNo_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCamcelBatch_Click
    protected void btnCamcelBatch_Click(object sender, EventArgs e)
    {
        txtAutoBatchNo.Text ="";
        Panel1.Visible = false;
        //ModalPopupPrintSelection1.Show();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("BATCH_MASTER", "MODIFY", "BT_CODE", mlCode);
            }

            dt2.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewBatchTicket.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
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
            CommonClasses.SendError("Batch Ticket", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadCombos

    #region LoadFormulaCode
    private void LoadFormulaCode()
    {
        try
        {
            dt = CommonClasses.FillCombo("SHADE_MASTER", "SHM_FORMULA_CODE", "SHM_CODE", "ES_DELETE=0 ", ddlFormulaCode);
            ddlFormulaCode.Items.Insert(0, new ListItem("Select Formula Code ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "LoadFormulaCode", Ex.Message);
        }
    }
    #endregion
   
    #region LoadWorkOrder
    private void LoadWorkOrder()
    {
        try
        {
            dt = CommonClasses.FillCombo("WORK_ORDER_MASTER", "WO_NO", "WO_CODE", "ES_DELETE=0 AND WO_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyId"]) + "' order by WO_NO desc", ddlWorkOrderNo);
            ddlWorkOrderNo.Items.Insert(0, new ListItem("Select Order No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "LoadWorkOrder", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlItemCode);
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "Load Item Code", Ex.Message);
        }
    }
    #endregion
   
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadFormulaCode();
            LoadICode();
            LoadWorkOrder();
            dtBatch.Clear();

            dt = CommonClasses.Execute("Select BT_CODE,BT_CM_CODE,BT_TYPE,BT_NO,BT_DATE,BT_WO_CODE,BT_WOD_I_CODE,BT_SHM_CODE,BT_FORMULA_TYPE from BATCH_MASTER where ES_DELETE=0 and BT_CM_CODE=" + (string)Session["CompanyCode"] + " and BT_CODE=" + mlCode + "");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["BT_CODE"]); ;
                ddlBatchType.SelectedValue = Convert.ToInt32(dt.Rows[0]["BT_TYPE"]).ToString();
                txtBatchNo.Text = dt.Rows[0]["BT_NO"].ToString();
                txtBatchDate.Text = Convert.ToDateTime(dt.Rows[0]["BT_DATE"]).ToString("dd MMM yyyy");
                ddlWorkOrderNo.SelectedValue = dt.Rows[0]["BT_WO_CODE"].ToString();
                ddlFormulaType.SelectedValue = dt.Rows[0]["BT_FORMULA_TYPE"].ToString();

                ddlItemCode.Text = dt.Rows[0]["BT_WOD_I_CODE"].ToString();
                ddlFormulaCode.Text = dt.Rows[0]["BT_SHM_CODE"].ToString();
                ddlWorkOrderNo_SelectedIndexChanged(null,null);
                //txtTotalInKg.Text = dt.Rows[0]["BT_TOTAL_IN_KG"].ToString();
                //totInKg = Convert.ToDouble(dt.Rows[0]["BT_TOTAL_IN_KG"]);
                //txtAvgDensity.Text = dt.Rows[0]["BT_DENSITY"].ToString();

                dtBatch = CommonClasses.Execute("select BTD_PROCESS_CODE as PROCESS_CODE,PROCESS_NAME, BTD_STEP_NO as STEP_NO,cast(BTD_QTY as numeric(20,3)) as QTY_IN_LTR,I_CODENO,I_CODE,cast(BTD_WGT as numeric(20,3)) as WEIGHT_IN_KG,cast(BTD_QTY_IN as numeric(20,3)) as QtyinKG,cast(I_DENSITY as numeric(20,3)) as Density,cast(isnull(I_CURRENT_BAL,0) as numeric(20,3)) as I_CURRENT_BAL from BATCH_MASTER,BATCH_DETAIL,PROCESS_MASTER,ITEM_MASTER where BT_CODE=BTD_BT_CODE and BTD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and BTD_I_CODE=I_CODE and BT_CODE='" + mlCode + "'");

                if (dtBatch.Rows.Count != 0)
                {
                    dgMainShade.DataSource = dtBatch;
                    dgMainShade.DataBind();
                    dgMainShade.Enabled = true;
                    dt2 = dtBatch;
                    GetTotal();
                }
                else
                {
                    dtBatch.Rows.Add(dtBatch.NewRow());
                    dgMainShade.DataSource = dtBatch;
                    dgMainShade.DataBind();
                    dgMainShade.Enabled = false;
                }
            }

            if (str == "VIEW")
            {
                btnSubmit.Visible = false;              
                dgMainShade.Enabled = false;
                ddlBatchType.Enabled = false;
                txtBatchNo.Enabled = false;
                txtBatchDate.Enabled = false;
                ddlWorkOrderNo.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlFormulaCode.Enabled = false;
                ddlFormulaType.Enabled = false;

            }
            if (str == "MOD" || str == "AMEND")
            {
                ddlBatchType.Enabled = false;
                txtBatchNo.Enabled = false;
                CommonClasses.SetModifyLock("BATCH_MASTER", "MODIFY", "BT_CODE", Convert.ToInt32(mlCode));

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "ViewRec", Ex.Message);
        }
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

                if (CommonClasses.Execute1("INSERT INTO BATCH_MASTER (BT_CM_CODE,BT_TYPE,BT_NO,BT_DATE,BT_WO_CODE,BT_WOD_I_CODE,BT_SHM_CODE,BT_FORMULA_TYPE,BT_CM_ID)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlBatchType.SelectedValue + "','" + txtBatchNo.Text + "','" + Convert.ToDateTime(txtBatchDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlWorkOrderNo.SelectedValue + "','" + ddlItemCode.SelectedValue + "','" + ddlFormulaCode.SelectedValue + "','" + ddlFormulaType.SelectedValue + "','" + Convert.ToInt32(Session["CompanyId"]) + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(BT_CODE) from BATCH_MASTER");
                    for (int i = 0; i < dgMainShade.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO BATCH_DETAIL (BTD_BT_CODE,BTD_I_CODE,BTD_STEP_NO,BTD_PROCESS_CODE,BTD_QTY,BTD_WGT,BTD_QTY_IN) values ('" + Code + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblSTEP_NO")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblPROCESS_CODE")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQTY_IN_LTR")).Text + "','" + (dgMainShade.Rows[i].Cells[7]).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQtyInKg")).Text + "')");
                    }
                    CommonClasses.WriteLog("Batch Ticket", "Save", "Batch Ticket", txtBatchNo.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    dt2.Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewBatchTicket.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ddlBatchType.Focus();
                }              
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE BATCH_MASTER SET BT_TYPE='" + ddlBatchType.SelectedValue + "',BT_NO='" + txtBatchNo.Text + "',BT_DATE='" + Convert.ToDateTime(txtBatchDate.Text).ToString("dd/MMM/yyyy") + "',BT_WO_CODE='" + ddlWorkOrderNo.SelectedValue + "',BT_WOD_I_CODE='" + ddlItemCode.SelectedValue + "',BT_SHM_CODE='" + ddlFormulaCode.SelectedValue + "',BT_FORMULA_TYPE='"+ddlFormulaType.SelectedValue+"' where BT_CODE='" + mlCode + "'"))
                {

                    result = CommonClasses.Execute1("DELETE FROM BATCH_DETAIL WHERE BTD_BT_CODE='" + mlCode + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgMainShade.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO BATCH_DETAIL (BTD_BT_CODE,BTD_I_CODE,BTD_STEP_NO,BTD_PROCESS_CODE,BTD_QTY,BTD_WGT,BTD_QTY_IN) values ('" + mlCode + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblSTEP_NO")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblPROCESS_CODE")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQTY_IN_LTR")).Text + "','" + (dgMainShade.Rows[i].Cells[7]).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQtyInKg")).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("BATCH_MASTER", "MODIFY", "BT_CODE", mlCode);
                        CommonClasses.WriteLog("Batch Ticket", "Update", "Batch Ticket", txtBatchNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        dt2.Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewBatchTicket.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlBatchType.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Batch Ticket", "SaveRec", ex.Message);
        }
        return result;
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
            CommonClasses.SendError("Batch Ticket", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion   

    #region ddlBatchType_SelectedIndexChanged
    protected void ddlBatchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlBatchType.SelectedIndex != 0)
            {
                if (ddlBatchType.SelectedValue == "1")
                {
                    ddlWorkOrderNo.Enabled = true;
                    ddlWorkOrderNo.SelectedIndex = 0;
                    ddlItemCode.SelectedIndex = 0;
                    ddlFormulaCode.SelectedIndex = 0;
                }
                else
                {
                    LoadICode();
                    ddlWorkOrderNo.SelectedIndex = 0;
                    ddlWorkOrderNo.Enabled = false;
                    ddlFormulaCode.SelectedIndex = 0;
                }

            }
            else
            {
                ddlWorkOrderNo.SelectedIndex = 0;
                ddlItemCode.SelectedIndex = 0;
                ddlFormulaCode.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "ddlBatchType_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlWorkOrderNo_SelectedIndexChanged
    protected void ddlWorkOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlWorkOrderNo.SelectedIndex != 0)
            {
                if (ddlBatchType.SelectedValue == "1")
                {
                    DataTable dtItem = CommonClasses.FillCombo("ITEM_MASTER,WORK_ORDER_MASTER,WORK_ORDER_DETAIL", "I_CODENO", "I_CODE", "ITEM_MASTER.ES_DELETE=0 and WORK_ORDER_MASTER.ES_DELETE=0 and WO_CODE=WOD_WO_CODE and WOD_I_CODE=I_CODE and WO_CODE='" + ddlWorkOrderNo.SelectedValue + "' AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlItemCode);
                    ddlItemCode.Items.Insert(0, new ListItem("Please Select Item Code", "0"));
                    if (dtItem.Rows.Count > 0)
                    {
                        ddlItemCode.SelectedIndex = 1;
                       DatabaseAccessLayer dbaccess=new DatabaseAccessLayer();
                       txtWorkOrdQty.Text = dbaccess.GetColumn("select WOD_WORK_ORDER_QTY from WORK_ORDER_MASTER,WORK_ORDER_DETAIL where WO_CODE='" + ddlWorkOrderNo.SelectedValue + "' and WOD_WO_CODE=WO_CODE and WOD_I_CODE='"+ddlItemCode.SelectedValue+"' ");
                    }
                }
                else
                {
                    LoadICode();
                }
            }
            else
            {
                LoadICode();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Batch Ticket", "ddlWorkOrderNo_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlWorkOrderNo.SelectedIndex != 0 && ddlItemCode.SelectedIndex !=0)
            {
                if (ddlBatchType.SelectedValue == "1")
                {                   
                    DatabaseAccessLayer dbaccess = new DatabaseAccessLayer();
                    txtWorkOrdQty.Text = dbaccess.GetColumn("select WOD_WORK_ORDER_QTY from WORK_ORDER_MASTER,WORK_ORDER_DETAIL where WO_CODE='" + ddlWorkOrderNo.SelectedValue + "' and WOD_WO_CODE=WO_CODE and WOD_I_CODE='" + ddlItemCode.SelectedValue + "' ");                    
                }
            }
            else
            {
                txtWorkOrdQty.Text = "0.000";
            }
           
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Batch Ticket", "ddlWorkOrderNo_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlFormulaCode_SelectedIndexChanged
    protected void ddlFormulaCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (ddlFormulaCode.SelectedIndex != 0)
            //{
                FillFormulaDetail();
            //}
            //else
            //{
            //    LoadFilter();
            //}

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "ddlFormulaCode_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion 

    #region ddlFormulaType_SelectedIndexChanged
    protected void ddlFormulaType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
            FillFormulaDetail();
            
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Batch Ticket", "ddlFormulaType_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion 

    #region FillFormulaDetail
    public void FillFormulaDetail()
    {
        if (ddlFormulaCode.SelectedIndex != 0 && ddlFormulaType.SelectedIndex != 0)
        {       
        double OrderQty = 0;
        DataTable dtMain = new DataTable();
        DataTable dtOrderQty = CommonClasses.Execute("select WOD_I_CODE,isnull(WOD_WORK_ORDER_QTY,0) as WOD_WORK_ORDER_QTY,isnull(I_DENSITY,0) as I_DENSITY from WORK_ORDER_MASTER,WORK_ORDER_DETAIL,ITEM_MASTER where WO_CODE=WOD_WO_CODE and WOD_I_CODE='" + ddlItemCode.SelectedValue + "' and WOD_I_CODE=I_CODE and WO_CODE='" + ddlWorkOrderNo.SelectedValue + "'");
        if (dtOrderQty.Rows.Count > 0)
        {
            OrderQty = Convert.ToDouble(dtOrderQty.Rows[0]["WOD_WORK_ORDER_QTY"]) * Convert.ToDouble(dtOrderQty.Rows[0]["I_DENSITY"]);
        }
        if (ddlBatchType.SelectedIndex == 2)
        {
            OrderQty = 100;
        }
        DataTable dtFormulaDetail = CommonClasses.Execute("select SHM_PROCESS_CODE as PROCESS_CODE,PROCESS_NAME,SHM_PROCESS_STEPS as STEP_NO,cast(sum(SHM_QTY_LTR) * '" + OrderQty + "'/100 as numeric(20,3)) as QTY_IN_LTR,I_CODENO,I_CODE,I_CAT_CODE,cast(isnull(I_DENSITY,0) as numeric(20,2)) as I_DENSITY,cast(sum(SHM_QTY_KG) *  '" + OrderQty + "'/100 as numeric(20,3)) as WEIGHT_IN_KG,cast(Round(sum(SHM_QTY_KG) *  '" + OrderQty + "'/100,0) as numeric(20,3)) as QtyinKG,cast(isnull(I_CURRENT_BAL,0) as numeric(20,3)) as I_CURRENT_BAL from SHADE_MASTER,SHADE_DETAIL,PROCESS_MASTER,ITEM_MASTER where SHD_SHM_CODE=SHM_CODE and SHM_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and SHM_ITEM_CODE=I_CODE and SHM_CODE='" + ddlFormulaCode.SelectedValue + "' group by SHM_PROCESS_CODE,PROCESS_NAME,SHM_PROCESS_STEPS,I_CODENO,I_CODE,I_CAT_CODE,I_DENSITY,I_CURRENT_BAL");
        if (dtMain.Columns.Count == 0)
        {
            DataColumn d1 = new DataColumn("PROCESS_CODE");
            dtMain.Columns.Add(d1);
            DataColumn d2 = new DataColumn("PROCESS_NAME");
            dtMain.Columns.Add(d2);
            DataColumn d3 = new DataColumn("STEP_NO");
            dtMain.Columns.Add(d3);
            DataColumn d4 = new DataColumn("QTY_IN_LTR");
            dtMain.Columns.Add(d4);
            DataColumn d5 = new DataColumn("I_CODENO");
            dtMain.Columns.Add(d5);
            DataColumn d6 = new DataColumn("I_CODE");
            dtMain.Columns.Add(d6);
            DataColumn d7 = new DataColumn("WEIGHT_IN_KG");
            dtMain.Columns.Add(d7);
            DataColumn d8 = new DataColumn("QtyinKG");
            dtMain.Columns.Add(d8);
            DataColumn d9 = new DataColumn("Density");
            dtMain.Columns.Add(d9);
            DataColumn d10 = new DataColumn("I_CURRENT_BAL");
            dtMain.Columns.Add(d10);
        }
        for (int i = 0; i < dtFormulaDetail.Rows.Count; i++)
        {
            double weght = 0, Qty = 0; ;            
            weght = Convert.ToDouble(dtFormulaDetail.Rows[i]["WEIGHT_IN_KG"]);
            if (dtFormulaDetail.Rows[i]["I_CAT_CODE"].ToString() == "-2147483647" && ddlFormulaType.SelectedValue == "2" && !dtFormulaDetail.Rows[i]["PROCESS_NAME"].ToString().Contains("STOP"))
            {
                //DataTable dtBOMDetail = CommonClasses.Execute("select BM_I_CODE,BD_I_CODE,cast(BD_VQTY * '" + weght + "' * '" + OrderQty + "' as numeric(20,3)) as BD_VQTY,BD_SQTY,I_DENSITY,I_CODENO from BOM_MASTER,BOM_DETAIL,ITEM_MASTER where BM_CODE=BD_BM_CODE and BOM_MASTER.ES_DELETE=0 and BD_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and BM_I_CODE='" + dtFormulaDetail.Rows[i]["I_CODE"].ToString() + "' and I_CM_COMP_ID='"+Session["CompanyId"]+"'");
                //DataTable dtBOMDetail = CommonClasses.Execute("select SHM_BASE_I_CODE,SHM_PROCESS_STEPS,PROCESS_CODE,PROCESS_NAME,SHM_ITEM_CODE,cast((SHM_QTY_KG/100) as numeric(20,3)) as SHM_QTY_KG,SHM_QTY_LTR,cast(isnull(I_DENSITY,0) as numeric(20,2)) as I_DENSITY,I_CODENO from SHADE_MASTER,SHADE_DETAIL,ITEM_MASTER,PROCESS_MASTER where SHM_CODE=SHD_SHM_CODE and SHADE_MASTER.ES_DELETE=0 and SHM_ITEM_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and SHM_PROCESS_CODE=PROCESS_CODE  and SHM_BASE_I_CODE='" + dtFormulaDetail.Rows[i]["I_CODE"].ToString() + "'");
                DataTable dtBOMDetail = CommonClasses.Execute("select SHM_BASE_I_CODE,SHM_PROCESS_STEPS,PROCESS_CODE,PROCESS_NAME,SHM_ITEM_CODE,cast((SHM_QTY_KG/100) as numeric(20,3)) as SHM_QTY_KG,SHM_QTY_LTR,cast(isnull(I_DENSITY,0) as numeric(20,2)) as I_DENSITY,I_CODENO,cast(isnull(I_CURRENT_BAL,0) as numeric(20,3)) as I_CURRENT_BAL from SHADE_MASTER,SHADE_DETAIL,ITEM_MASTER,PROCESS_MASTER where SHM_CODE=SHD_SHM_CODE and SHADE_MASTER.ES_DELETE=0 and SHM_ITEM_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and SHM_PROCESS_CODE=PROCESS_CODE  and SHM_BASE_I_CODE='" + dtFormulaDetail.Rows[i]["I_CODE"].ToString() + "'");
                for (int j = 0; j < dtBOMDetail.Rows.Count; j++)
                {
                    double wghtinkg = 0;
                    int Count = 0;
                    for (int k = 0; k < dtMain.Rows.Count; k++)
                    {
                        if (dtBOMDetail.Rows[j]["PROCESS_CODE"].ToString() == dtMain.Rows[k]["PROCESS_CODE"].ToString() && dtBOMDetail.Rows[j]["SHM_ITEM_CODE"].ToString() == dtMain.Rows[k]["I_CODE"].ToString())
                        {
                            wghtinkg = Math.Round(((Convert.ToDouble(dtBOMDetail.Rows[j]["SHM_QTY_KG"]) * weght) + (Convert.ToDouble(dtMain.Rows[k]["WEIGHT_IN_KG"]))), 3);
                            Qty = Math.Round((wghtinkg / Convert.ToDouble(dtBOMDetail.Rows[j]["I_DENSITY"])), 3);
                            //dtMain.Rows.Add(dtBOMDetail.Rows[i]["PROCESS_CODE"].ToString(), dtBOMDetail.Rows[j]["PROCESS_NAME"].ToString(), dtBOMDetail.Rows[j]["SHM_PROCESS_STEPS"].ToString(), Qty, dtBOMDetail.Rows[j]["I_CODENO"].ToString(), dtBOMDetail.Rows[j]["SHM_ITEM_CODE"].ToString(), string.Format("{0:0.000}", wghtinkg), string.Format("{0:0.000}", Math.Round(wghtinkg), dtBOMDetail.Rows[j]["I_DENSITY"].ToString()));
                            dtMain.Rows[k]["QTY_IN_LTR"] = Qty;
                            dtMain.Rows[k]["WEIGHT_IN_KG"] = wghtinkg;
                            dtMain.Rows[k]["QtyinKG"] = wghtinkg;                   

                            Count = 1;
                        }                        
                    }
                    if (Count == 0)
                    {
                        wghtinkg = Math.Round((Convert.ToDouble(dtBOMDetail.Rows[j]["SHM_QTY_KG"]) * weght), 3);
                        Qty = Math.Round(( wghtinkg / Convert.ToDouble(dtBOMDetail.Rows[j]["I_DENSITY"])), 3);
                        dtMain.Rows.Add(dtBOMDetail.Rows[j]["PROCESS_CODE"].ToString(), dtBOMDetail.Rows[j]["PROCESS_NAME"].ToString(), dtBOMDetail.Rows[j]["SHM_PROCESS_STEPS"].ToString(), Qty, dtBOMDetail.Rows[j]["I_CODENO"].ToString(), dtBOMDetail.Rows[j]["SHM_ITEM_CODE"].ToString(), string.Format("{0:0.000}", wghtinkg), string.Format("{0:0.000}", wghtinkg), dtBOMDetail.Rows[j]["I_DENSITY"].ToString(), dtBOMDetail.Rows[j]["I_CURRENT_BAL"].ToString());
                    }
                }
            }
            else
            {
                dtMain.Rows.Add(dtFormulaDetail.Rows[i]["PROCESS_CODE"].ToString(), dtFormulaDetail.Rows[i]["PROCESS_NAME"].ToString(), dtFormulaDetail.Rows[i]["STEP_NO"].ToString(), dtFormulaDetail.Rows[i]["QTY_IN_LTR"].ToString(), dtFormulaDetail.Rows[i]["I_CODENO"].ToString(), dtFormulaDetail.Rows[i]["I_CODE"].ToString(), dtFormulaDetail.Rows[i]["WEIGHT_IN_KG"].ToString(), dtFormulaDetail.Rows[i]["QtyinKG"].ToString(), dtFormulaDetail.Rows[i]["I_DENSITY"].ToString(), dtFormulaDetail.Rows[i]["I_CURRENT_BAL"].ToString());
            }
        }
        if (ddlFormulaType.SelectedIndex == 2)
        {
            dtMain.DefaultView.Sort = "PROCESS_CODE DESC";
            dtMain.AcceptChanges();

            DataView dv = dtMain.DefaultView;
            dv.Sort = "PROCESS_CODE desc";
            dtMain = dv.ToTable();
        }
        for (int i = 0; i < dtMain.Rows.Count; i++)
        {
            dtMain.Rows[i]["STEP_NO"] = ((i+1)*10).ToString();
        }
        if (dtMain.Rows.Count > 0)
        {
            dgMainShade.DataSource = dtMain;
            dgMainShade.DataBind();
            GetTotal();
        }
        //if (dtFormulaDetail.Rows.Count > 0)
        //{
        //    dgMainShade.DataSource = dtFormulaDetail;
        //    dgMainShade.DataBind();
        //}
        else
        {
            LoadFilter();
        }
        }
        else
        {
            LoadFilter();
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        try
        {
            
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("STEP_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("QTY_IN_LTR", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WEIGHT_IN_KG", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("QtyinKG", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Density", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMainShade.DataSource = dtFilter;
                    dgMainShade.DataBind();

                }
           
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Batch Ticket", "LoadFilter", ex.Message.ToString());
        }
    }
    #endregion

    #region txtTotalInKG_TextChanged
    protected void txtTotalInKG_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTotalInKg.Text != "")
            {
                double TotalInKg = 0, TotalOneKg = 0, Density = 0, Rate = 0;
                if (dgMainShade.Rows.Count > 0)
                {
                    for (int i = 0; i < dgMainShade.Rows.Count; i++)
                    {
                       // Rate = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblRate"))).Text);
                        Density = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblDensity"))).Text);
                        TotalInKg = Convert.ToDouble((dgMainShade.Rows[i].Cells[7]).Text);

                        TotalOneKg = TotalInKg / totInKg;
                        (dgMainShade.Rows[i].Cells[7]).Text = string.Format("{0:0.000}", Math.Round((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)), 3));
                        ((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text = string.Format("{0:0.000}", Math.Round((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)), 3));
                        ((Label)(dgMainShade.Rows[i].FindControl("lblQTY_IN_LTR"))).Text = string.Format("{0:0.000}", Math.Round(((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)) / Density), 3));
                        //.((Label)(dgMainShade.Rows[i].FindControl("lblAmount"))).Text = string.Format("{0:0.000}", Math.Round(((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)) * Rate), 2));
                    }
                }
                //totInKg = Convert.ToDouble(txtTotalInKg.Text);
                GetTotal();
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "txtTotalInKG_TextChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double TotalInKg = 0, TotalInLtr = 0, AvgDensity = 0;
        try
        {
            if (dgMainShade.Rows.Count > 0)
            {
                for (int i = 0; i < dgMainShade.Rows.Count; i++)
                {
                    //TotalInKg = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text);
                    //TotalInLtr = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInLTR"))).Text);
                    //TotalInKg = TotalInKg + Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblWEIGHT_IN_KG"))).Text);
                    //TotalInLtr = TotalInLtr + Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQTY_IN_LTR"))).Text);
                    TotalInKg = TotalInKg + Convert.ToDouble(((dgMainShade.Rows[i].Cells[7]).Text));
                    TotalInLtr = TotalInLtr + Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQTY_IN_LTR"))).Text);
                }
            }
            txtTotalInKg.Text = string.Format("{0:0.00}", Math.Round(TotalInKg, 3));
            txtTotalInLtr.Text = string.Format("{0:0.00}", Math.Round(TotalInLtr, 3));
            totInKg = Math.Round(TotalInKg, 3);
            AvgDensity = TotalInKg /TotalInLtr ;
            txtAvgDensity.Text = string.Format("{0:0.00}", Math.Round(AvgDensity, 2));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "GetTotal", Ex.Message);
        }

    }
    #endregion


    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            double quantity = 0.00, wghtInKg = 0.00;
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if(e.Row.Cells[3].Text !="&nbsp;")
                {
                    quantity = string.IsNullOrEmpty(e.Row.Cells[3].Text) ? 0.00 : Convert.ToDouble(e.Row.Cells[3].Text);
                }
                if (e.Row.Cells[7].Text != "&nbsp;")
                {
                    wghtInKg = string.IsNullOrEmpty(e.Row.Cells[7].Text) ? 0.00 : Convert.ToDouble(e.Row.Cells[7].Text);
                }
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (quantity < wghtInKg)
                    {
                        cell.BackColor = Color.Yellow;
                    }
                }
            }
        }
    }

     

}
