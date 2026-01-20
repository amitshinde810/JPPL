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

public partial class Transactions_ADD_TinterSheet : System.Web.UI.Page
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
                        loadBatch(Request.QueryString[0].ToString());
                        LoadParty();
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
                            txtDate.Attributes.Add("readonly", "readonly");
                            txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            // dgMainShade.Enabled = false;
                        }
                        ddlBatchNo.Focus();
                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Tinter Sheet", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tinter Sheet", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlBatchNo.SelectedIndex != 0)
            {
                if (ddlBatchNo.SelectedValue == "1")
                {
                    ShowMessage("#Avisos", "Select Batch No", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    // ddlWorkOrderNo.Focus();
                    return;
                }
            }

            //if (dgMainShade.Rows.Count > 0)
            {
                SaveRec();

            }
            //else
            //{
            //    ShowMessage("#Avisos", "No Record !! Please Insert Record In Table", CommonClasses.MSG_Warning);
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            //    return;
            //}

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet", "btnSubmit_Click", Ex.Message);

        }

    }

    #endregion btnSubmit_Click

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {

            if (ddlBatchNo.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddlCustomer.SelectedIndex == 0)
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
            CommonClasses.SendError("Tinter Sheet", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Tinter Sheet", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("TINTER_SHEET_MASTER", "MODIFY", "TSM_CODE", mlCode);
            }

            dt2.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewTinterSheet.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet", "CancelRecord", Ex.Message);
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
            CommonClasses.SendError("Tinter Sheet", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadCombos

    private void loadBatch(string type)
    {
        try
        {
            DataTable dtBatch = new DataTable();
            if (type == "INSERT")
            {
                dtBatch = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "BATCH_MASTER.ES_DELETE=0 and BT_CODE not in (select distinct TSM_BT_CODE from TINTER_SHEET_MASTER where TSM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and TINTER_SHEET_MASTER.ES_DELETE=0) AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO desc", ddlBatchNo);
                ddlBatchNo.Items.Insert(0, new ListItem("Select Batch No ", "0"));
            }
            else
            {
                dtBatch = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "BATCH_MASTER.ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO desc", ddlBatchNo);
                ddlBatchNo.Items.Insert(0, new ListItem("Select Batch No ", "0"));
            }
           
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet", "LoadFormulaCode", Ex.Message);
        }
    }

    #region LoadFormulaCode
    private void LoadFormulaCode()
    {

    }
    #endregion

    #region LoadWorkOrder
    private void LoadWorkOrder()
    {
        try
        {
            //dt = CommonClasses.FillCombo("WORK_ORDER_MASTER", "WO_NO", "WO_CODE", "ES_DELETE=0 AND WO_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'", ddlWorkOrderNo);
            // ddlWorkOrderNo.Items.Insert(0, new ListItem("Select Order No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet", "LoadWorkOrder", Ex.Message);
        }
    }
    #endregion

    #region Party
    private void LoadParty()
    {
        DataTable dtParty = new DataTable();
        dtParty = CommonClasses.FillCombo("PARTY_MASTER", "P_NAME", "P_CODE", "PARTY_MASTER.ES_DELETE=0 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlCustomer);
        ddlCustomer.Items.Insert(0, new ListItem("Select Customer ", "0"));
    }
    #endregion

    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadFormulaCode();
            LoadParty();
            LoadWorkOrder();
            dtBatch.Clear();

            dt = CommonClasses.Execute("SELECT TSM_CODE, TSM_CM_CODE, TSM_BT_CODE, TSM_FORMULA, TSM_DATE,cast(isnull(TSM_LITERS,0) as numeric(10,3)) as TSM_LITERS,Cast(isnull(TSM_KG,0) as numeric(10,3)) as TSM_KG, TSM_P_CODE, TSM_TINTER_NAME,cast(isnull(TSM_INITIAL_DL,0) as numeric(10,2)) as TSM_INITIAL_DL,cast(isnull(TSM_INITIAL_DA,0) as numeric(10,2)) as TSM_INITIAL_DA,cast(isnull(TSM_INITIAL_DB,0) as numeric(10,2)) as TSM_INITIAL_DB,cast(isnull(TSM_INITIAL_GLOSS,0) as numeric(10,2)) as TSM_INITIAL_GLOSS,cast(isnull(TSM_INITIAL_VISCOSITY,0) as numeric(10,2)) as TSM_INITIAL_VISCOSITY, TSM_CUP,cast(isnull(TSM_INITIAL_SG,0) as numeric(10,2)) as TSM_INITIAL_SG,cast(isnull(TSM_FINAL_DL,0) as numeric(10,2)) as TSM_FINAL_DL,cast(isnull(TSM_FINAL_DA,0) as numeric(10,2)) as TSM_FINAL_DA,cast(isnull(TSM_FINAL_DB,0) as numeric(10,2)) as TSM_FINAL_DB,cast(isnull(TSM_FINAL_GLOSS,0) as numeric(10,2)) as TSM_FINAL_GLOSS,cast(isnull(TSM_FINAL_VISCOSITY,0) as numeric(10,2)) as TSM_FINAL_VISCOSITY,cast(isnull(TSM_FINAL_SG,0) as numeric(10,2)) as TSM_FINAL_SG, TSM_CHECKED_BY, TSM_APPROVED_BY,cast(isnull(TSM_FINAL_DE,0) as numeric(10,2)) as TSM_FINAL_DE FROM TINTER_SHEET_MASTER where ES_DELETE=0 and TSM_CM_CODE=" + (string)Session["CompanyCode"] + " and TSM_CODE=" + mlCode + "");

            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["TSM_CODE"]);
                ddlBatchNo.SelectedValue = Convert.ToInt32(dt.Rows[0]["TSM_BT_CODE"]).ToString();
                txtFormula.Text = dt.Rows[0]["TSM_FORMULA"].ToString();
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["TSM_DATE"]).ToString("dd MMM yyyy");
                txtLiters.Text = dt.Rows[0]["TSM_LITERS"].ToString();
                txtKg.Text = dt.Rows[0]["TSM_KG"].ToString();             
                ddlCustomer.SelectedValue = Convert.ToInt32(dt.Rows[0]["TSM_P_CODE"]).ToString();
                txtTinerName.Text = dt.Rows[0]["TSM_TINTER_NAME"].ToString();
                txtDL.Text = dt.Rows[0]["TSM_INITIAL_DL"].ToString();
                txtDa.Text = dt.Rows[0]["TSM_INITIAL_DA"].ToString();
                txtDb.Text = dt.Rows[0]["TSM_INITIAL_DB"].ToString();
                txtInitialGloss.Text = dt.Rows[0]["TSM_INITIAL_GLOSS"].ToString();
                txtInitialViscosity.Text = dt.Rows[0]["TSM_INITIAL_VISCOSITY"].ToString();
                txtCup.Text = dt.Rows[0]["TSM_CUP"].ToString();
                txtInitialSG.Text = dt.Rows[0]["TSM_INITIAL_SG"].ToString();
                txtFDL.Text = dt.Rows[0]["TSM_FINAL_DL"].ToString();
                txtFDa.Text = dt.Rows[0]["TSM_FINAL_DA"].ToString();
                txtFDB.Text = dt.Rows[0]["TSM_FINAL_DB"].ToString();
                txtFinalGloss.Text = dt.Rows[0]["TSM_FINAL_GLOSS"].ToString();
                txtFinalVisxosity.Text = dt.Rows[0]["TSM_FINAL_VISCOSITY"].ToString();
                txtFinalSG.Text = dt.Rows[0]["TSM_FINAL_SG"].ToString();
                txtCheckedBy.Text = dt.Rows[0]["TSM_CHECKED_BY"].ToString();
                txtApporvedBy.Text = dt.Rows[0]["TSM_APPROVED_BY"].ToString();
                txtFDE.Text = dt.Rows[0]["TSM_FINAL_DE"].ToString();


                dtBatch = CommonClasses.Execute("SELECT PROCESS_NAME,TSD_PROCESS_CODE,I_CODE,I_CODENO,TSD_LOT ,cast(isnull(TSD_QTY,0) as numeric(10,3)) as TSD_QTY ,TSD_ADD_BY,cast(TSD_DL as numeric(10,2)) as TSD_DL,cast(isnull(TSD_DA,0) as numeric(10,2)) as TSD_DA,cast(isnull(TSD_DB,0) as numeric(10,2)) as TSD_DB,cast(isnull(TSD_GLOSS,0) as numeric(10,2)) as TSD_GLOSS,cast(isnull(TSD_VISCOSITY,0) as numeric(10,2)) as TSD_VISCOSITY FROM TINTER_SHEET_DETAIL,ITEM_MASTER,PROCESS_MASTER WHERE I_CODE=TSD_I_CODE AND PROCESS_MASTER.ES_DELETE=0 and TSD_PROCESS_CODE=PROCESS_CODE and TSD_TSM_CODE='" + mlCode + "' order by PROCESS_NAME,I_CODENO");

                if (dtBatch.Rows.Count != 0)
                {
                    dgMainShade.DataSource = dtBatch;
                    dgMainShade.DataBind();
                    dgMainShade.Enabled = true;
                    dt2 = dtBatch;

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
                ddlBatchNo.Enabled = false;
                txtFormula.Enabled = false;
                txtDate.Enabled = false;
                txtLiters.Enabled = false;
                txtKg.Enabled = false;
                ddlCustomer.Enabled = false;
                txtTinerName.Enabled = false;
                txtDL.Enabled = false;
                txtDa.Enabled = false;
                txtDb.Enabled = false;
                txtInitialGloss.Enabled = false;
                txtInitialViscosity.Enabled = false;
                txtCup.Enabled = false;
                txtInitialSG.Enabled = false;
                txtFDL.Enabled = false;
                txtFDa.Enabled = false;
                txtFDB.Enabled = false;
                txtFinalGloss.Enabled = false;
                txtFinalVisxosity.Enabled = false;
                txtFinalSG.Enabled = false;
                txtCheckedBy.Enabled = false;
                txtApporvedBy.Enabled = false;
                txtFDE.Enabled = false;
                dgMainShade.Enabled = false;
                btnSubmit.Visible = false; 
                 
            }
            if (str == "MOD" || str == "AMEND")
            {
                ddlBatchNo.Enabled = false;
                txtFormula.Enabled = false;
                // txtBatchNo.Enabled = false;
                CommonClasses.SetModifyLock("TINTER_SHEET_MASTER", "MODIFY", "TSM_CODE", Convert.ToInt32(mlCode));

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tinter Sheet", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        string qty = "0", DL = "0", DA = "0", DB = "0", GLOSS = "0", VISC = "0";
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
               
                //if (CommonClasses.Execute1("INSERT INTO BATCH_MASTER (BT_CM_CODE,BT_TYPE,BT_NO,BT_DATE,BT_WO_CODE,BT_WOD_I_CODE,BT_SHM_CODE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlBatchNo.SelectedValue + "','" + Doc_no + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + 0 + "','" + ddlCustomer.SelectedValue + "','" + 0 + "')"))
                if (CommonClasses.Execute1("INSERT INTO TINTER_SHEET_MASTER (TSM_CM_CODE,TSM_BT_CODE, TSM_FORMULA, TSM_DATE, TSM_LITERS, TSM_KG, TSM_P_CODE, TSM_TINTER_NAME, TSM_INITIAL_DL, TSM_INITIAL_DA, TSM_INITIAL_DB, TSM_INITIAL_GLOSS, TSM_INITIAL_VISCOSITY, TSM_CUP, TSM_INITIAL_SG, TSM_FINAL_DL, TSM_FINAL_DA, TSM_FINAL_DB, TSM_FINAL_GLOSS, TSM_FINAL_VISCOSITY, TSM_FINAL_SG, TSM_CHECKED_BY, TSM_APPROVED_BY,TSM_FINAL_DE)VALUES ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlBatchNo.SelectedValue + "', '" + txtFormula.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "', '" + txtLiters.Text + "', '" + txtKg.Text + "','" + ddlCustomer.SelectedValue + "', '" + txtTinerName.Text + "','" + txtDL.Text + "','" + txtDa.Text + "','" + txtDb.Text + "','" + txtInitialGloss.Text + "','" + txtInitialViscosity.Text + "','" + txtCup.Text + "','" + txtInitialSG.Text + "','" + txtFDL.Text + "','" + txtFDa.Text + "','" + txtFDB.Text + "','" + txtFinalGloss.Text + "','" + txtFinalVisxosity.Text + "','" + txtFinalSG.Text + "','" + txtCheckedBy.Text + "','" + txtApporvedBy.Text + "','"+txtFDE.Text+"')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(TSM_CODE) from TINTER_SHEET_MASTER");
                    for (int i = 0; i < dgMainShade.Rows.Count; i++)
                    {
                        qty = ((TextBox)dgMainShade.Rows[i].FindControl("txtQTY")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtQTY")).Text;
                        DL = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDL")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDL")).Text;
                        DA = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDA")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDA")).Text;
                        DB = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDB")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDB")).Text;
                        GLOSS = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddGLOSS")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddGLOSS")).Text;
                        VISC = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddVISCOSITY")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddVISCOSITY")).Text;

                        CommonClasses.Execute1("INSERT INTO TINTER_SHEET_DETAIL(TSD_TSM_CODE, TSD_I_CODE, TSD_LOT, TSD_QTY, TSD_ADD_BY, TSD_DL, TSD_DA, TSD_DB, TSD_GLOSS, TSD_VISCOSITY,TSD_PROCESS_CODE)VALUES('" + Code + "', '" + ((Label)dgMainShade.Rows[i].FindControl("lblI_CODE")).Text + "', '" + ((TextBox)dgMainShade.Rows[i].FindControl("txtLot")).Text + "','" + qty + "', '" + ((TextBox)dgMainShade.Rows[i].FindControl("txtAddBy")).Text + "', '" + DL + "', '" + DA + "', '" + DB + "','" + GLOSS + "', '" + VISC + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblTSD_PROCESS_CODE")).Text + "')");                       
                    }
                    CommonClasses.WriteLog("Tinter Sheet", "Save", "Tinter Sheet", "0", Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    dt2.Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewTinterSheet.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ddlBatchNo.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                //if (CommonClasses.Execute1("UPDATE BATCH_MASTER SET BT_TYPE='" + ddlBatchNo.SelectedValue + "',BT_NO='" + '0' + "',BT_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',BT_WO_CODE='" + 0 + "',BT_WOD_I_CODE='" + ddlCustomer.SelectedValue + "',BT_SHM_CODE='" + 0 + "' where BT_CODE='" + mlCode + "'"))
                if (CommonClasses.Execute1("UPDATE TINTER_SHEET_MASTER SET TSM_BT_CODE ='" + ddlBatchNo.SelectedValue + "', TSM_FORMULA ='" + txtFormula.Text + "', TSM_DATE ='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' ,TSM_LITERS ='" + txtLiters.Text + "', TSM_KG ='" + txtKg.Text + "', TSM_P_CODE ='" + ddlCustomer.SelectedValue + "', TSM_TINTER_NAME ='" + txtTinerName.Text + "', TSM_INITIAL_DL ='" + txtDL.Text + "', TSM_INITIAL_DA ='" + txtDa.Text + "',TSM_INITIAL_DB ='" + txtDb.Text + "', TSM_INITIAL_GLOSS ='" + txtInitialGloss + "', TSM_INITIAL_VISCOSITY ='" + txtInitialViscosity.Text + "', TSM_CUP ='" + txtCup.Text + "', TSM_INITIAL_SG ='" + txtInitialSG.Text + "', TSM_FINAL_DL ='" + txtFDL.Text + "', TSM_FINAL_DA ='" + txtFDa.Text + "', TSM_FINAL_DB ='" + txtFDB.Text + "',TSM_FINAL_GLOSS ='" + txtFinalGloss.Text + "', TSM_FINAL_VISCOSITY ='" + txtFinalVisxosity.Text + "', TSM_FINAL_SG ='" + txtInitialSG.Text + "', TSM_CHECKED_BY ='" + txtCheckedBy.Text + "', TSM_APPROVED_BY ='" + txtApporvedBy.Text + "' ,TSM_FINAL_DE='"+txtFDE.Text+"' where TSM_CODE='" + mlCode + "'"))
                {

                    result = CommonClasses.Execute1("DELETE FROM TINTER_SHEET_DETAIL WHERE TSD_TSM_CODE='" + mlCode + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgMainShade.Rows.Count; i++)
                        {
                            qty = ((TextBox)dgMainShade.Rows[i].FindControl("txtQTY")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtQTY")).Text;
                            DL = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDL")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDL")).Text;
                            DA = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDA")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDA")).Text;
                            DB = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDB")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddDB")).Text;
                            GLOSS = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddGLOSS")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddGLOSS")).Text;
                            VISC = ((TextBox)dgMainShade.Rows[i].FindControl("txtaddVISCOSITY")).Text == "" ? "0" : ((TextBox)dgMainShade.Rows[i].FindControl("txtaddVISCOSITY")).Text;

                            CommonClasses.Execute1("INSERT INTO TINTER_SHEET_DETAIL(TSD_TSM_CODE, TSD_I_CODE, TSD_LOT, TSD_QTY, TSD_ADD_BY, TSD_DL, TSD_DA, TSD_DB, TSD_GLOSS, TSD_VISCOSITY,TSD_PROCESS_CODE)VALUES('" + mlCode + "', '" + ((Label)dgMainShade.Rows[i].FindControl("lblI_CODE")).Text + "', '" + ((TextBox)dgMainShade.Rows[i].FindControl("txtLot")).Text + "','" + qty + "', '" + ((TextBox)dgMainShade.Rows[i].FindControl("txtAddBy")).Text + "', '" + DL + "', '" + DA + "', '" + DB + "','" + GLOSS + "', '" + VISC + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblTSD_PROCESS_CODE")).Text + "')");                       
                        }
                        CommonClasses.RemoveModifyLock("TINTER_SHEET_MASTER", "MODIFY", "TSM_CODE", mlCode);
                        CommonClasses.WriteLog("Tinter Sheet", "Update", "Tinter Sheet", "0", mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        dt2.Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewTinterSheet.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlBatchNo.Focus();
                }
            }

        }
        catch (Exception ex)
        {
              CommonClasses.SendError("Tinter Sheet", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Tinter Sheet", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        try
        {
            //  if (dgMainShade.Rows.Count == 0)
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_PROCESS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_LOT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_ADD_BY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_DL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_DA", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_DB", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_GLOSS", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("TSD_VISCOSITY", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                     dgMainShade.DataSource = dtFilter;
                    dgMainShade.DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tinter Sheet", "LoadFilter", ex.Message.ToString());
        }
    }
    #endregion

    #region ddlBatchNo_SelectedIndexChanged
    protected void ddlBatchNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        dt = CommonClasses.Execute("SELECT SHM_FORMULA_CODE,SHM_FORMULA_NAME FROm BATCH_MASTER,SHADE_MASTER	 WHERE BT_SHM_CODE=SHM_CODE AND BATCH_MASTER.ES_DELETE=0 AND BT_CM_CODE =" + (string)Session["CompanyCode"] + " and BT_CODE=" + ddlBatchNo.SelectedValue + "");
        if (dt.Rows.Count > 0)
        {
            txtFormula.Text = dt.Rows[0]["SHM_FORMULA_CODE"].ToString();
        }
        else
        {
            txtFormula.Text = "";
        }
        DataTable dtfillgrid = new DataTable();
        DataTable dtBatchDetail = new DataTable();
        dtBatchDetail = CommonClasses.Execute("SELECT P_CODE,cast(sum(BTD_QTY) as numeric(10,3)) as BTD_QTY,cast(sum(BTD_WGT) as numeric(10,3)) as BTD_WGT  FROm WORK_ORDER_MASTER,BATCH_MASTER,BATCH_DETAIL,PARTY_MASTER WHERE WO_CODE=BT_WO_CODE AND  WO_P_CODE=P_CODE and WORK_ORDER_MASTER.ES_DELETE=0 and PARTY_MASTER.ES_DELETE=0 and BT_CODE=BTD_BT_CODE AND  BT_CODE=" + ddlBatchNo.SelectedValue + " group by P_CODE");
        dtfillgrid = CommonClasses.Execute("SELECT PROCESS_NAME,BTD_PROCESS_CODE as TSD_PROCESS_CODE ,I_CODE,I_CODENO,'' AS TSD_LOT, '' AS TSD_QTY, '' AS TSD_ADD_BY , '' AS TSD_DL , '' AS TSD_DA , '' AS TSD_DB, '' AS TSD_GLOSS, '' AS TSD_VISCOSITY   FROm WORK_ORDER_MASTER,BATCH_MASTER,BATCH_DETAIL,ITEM_MASTER,PROCESS_MASTER WHERE WO_CODE=BT_WO_CODE AND BT_CODE=BTD_BT_CODE AND BTD_I_CODE=I_CODE AND PROCESS_MASTER.ES_DELETE=0 and BTD_PROCESS_CODE=PROCESS_CODE and  BT_CODE=" + ddlBatchNo.SelectedValue + " order by PROCESS_NAME,I_CODENO ");
        if (dtfillgrid.Rows.Count > 0)
        {
            dgMainShade.DataSource = dtfillgrid;
            dgMainShade.DataBind();
            if (dtBatchDetail.Rows.Count > 0)
            {
                ddlCustomer.SelectedValue = dtBatchDetail.Rows[0]["P_CODE"].ToString();
                txtLiters.Text = dtBatchDetail.Rows[0]["BTD_QTY"].ToString();
                txtKg.Text = dtBatchDetail.Rows[0]["BTD_WGT"].ToString();
            }
        }
        else
        {
            LoadFilter();
        }

    } 
    #endregion
}
