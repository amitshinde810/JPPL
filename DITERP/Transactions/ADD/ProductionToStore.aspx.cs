using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_ProductionToStore : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    ProductionToStore_BL productionStore_BL = null;
    static int mlCode = 0;
    DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
    public static string str = "";
    static DataTable dt2 = new DataTable();
    DataTable dtProductionDetail = new DataTable();
    static string ItemUpdateIndex = "-1";
    DataRow dr;
    public static int Index = 0;
    static string right = "";
    #endregion

    #region Events

    #region Page_Load
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='52'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadFilter();
                    ddlType.Enabled = false;
                    ddlMatReqNo.Enabled = false;
                    ViewState["mlCode"] = mlCode;
                    LoadDept();
                    fillCategory();
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["Index"] = Index;
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        productionStore_BL = new ProductionToStore_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        productionStore_BL = new ProductionToStore_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));
                        EnabaleTextBoxes(MainPanel);
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtGinDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtGinDate.Attributes.Add("readonly", "readonly");
                        LoadCombos();
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        dgvProductionStoreDetails.Enabled = false;
                        LoadFilter();
                    }
                    txtGinDate.Focus();
                    ddlMatReqNo.Enabled = false;
                    ddlCustomer.Enabled = false;
                    txtunit.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store-ADD", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvProductionStoreDetails.Enabled)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Exists", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Production To Store", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtGinDate.Text.Trim() == "")
            {
                flag = false;
            }
            else if (txtPersonName.Text.Trim() == "")
            {
                flag = false;
            }
            else if (ddlType.SelectedIndex == 0)
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
            CommonClasses.SendError("Production To Store", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region CancelRecord
    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("PRODUCTION_TO_STORE_MASTER", "MODIFY", "PS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Transactions/VIEW/ViewProductionToStore.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            InserRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region InserRecord
    private void InserRecord()
    {
        #region Validations
        if (ddlSubComponentCode.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Info);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            ddlSubComponentCode.Focus();
            return;
        }
        if (ddlSubComponentName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Info);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            ddlSubComponentName.Focus();
            return;
        }
        if (ddlType.SelectedValue != "2")
        {
            if (txtQty.Text == "" || Convert.ToDouble(txtQty.Text) == 0)
            {
                ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtQty.Focus();
                return;
            }
        }
        #endregion

        #region CheckExist
        if (dgvProductionStoreDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgvProductionStoreDetails.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgvProductionStoreDetails.Rows[i].FindControl("lblPSD_I_CODE"))).Text;
                string BT_CODE = ((Label)(dgvProductionStoreDetails.Rows[i].FindControl("lblBT_CODE"))).Text;
                if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                {
                    if (bd_i_code == ddlSubComponentName.SelectedValue.ToString())
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record already exists in table";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }
                }
                else
                {
                    if (bd_i_code == ddlSubComponentName.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record already exists in table";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }
                }
            }
        }
        #endregion

        #region Datatable Initialization

        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("PSD_I_CODE", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("UOM_CODE", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("PSD_QTY", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("PSD_REMARK", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(String)));
        }
        #endregion

        #region Insert Record to Table
        dr = ((DataTable)ViewState["dt2"]).NewRow();
        dr["PSD_I_CODE"] = ddlSubComponentCode.SelectedValue.ToString();
        dr["I_CODENO"] = ddlSubComponentCode.SelectedItem.ToString();
        dr["I_NAME"] = ddlSubComponentName.SelectedItem.ToString();
        dr["UOM"] = txtunit.Text.ToString();
        dr["UOM_CODE"] = ddlUnit.SelectedValue.ToString();
        dr["PSD_QTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtQty.Text));
        dr["PSD_REMARK"] = txtRemark.Text;
        dr["BT_CODE"] = "0";
        dr["BT_NO"] = "0";
        #endregion

        #region InsertData
        if (str == "Modify")
        {
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
            }
        }
        else
        {
            ((DataTable)ViewState["dt2"]).Rows.Add(dr);
        }
        #endregion

        dgvProductionStoreDetails.DataSource = ((DataTable)ViewState["dt2"]);
        dgvProductionStoreDetails.DataBind();
        dgvProductionStoreDetails.Enabled = true;

        //To avoid same item insert in Grid
        ViewState["ItemUpdateIndex"] = "-1";
        clearDetail();
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
            CommonClasses.SendError("Currency Order", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region ddlSubComponentCode_SelectedIndexChanged
    protected void ddlSubComponentCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSubComponentCode.SelectedIndex != -1)
            {
                ddlSubComponentName.SelectedValue = ddlSubComponentCode.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlSubComponentCode.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");

                DataTable dtitembatchno = CommonClasses.Execute("select BT_CODE,BT_NO from BATCH_MASTER where BT_WOD_I_CODE=" + ddlSubComponentCode.SelectedValue + " and ES_DELETE =0");
                if (dt.Rows.Count > 0)
                {
                    ddlUnit.SelectedValue = dt.Rows[0]["I_UOM_CODE"].ToString();
                    txtunit.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                else
                {
                    ddlUnit.SelectedIndex = -1;
                }
                if (dtitembatchno.Rows.Count > 0)
                {
                    ddlItembatchno.DataSource = dtitembatchno;
                    ddlItembatchno.DataTextField = "BT_NO";
                    ddlItembatchno.DataValueField = "BT_CODE";
                    ddlItembatchno.DataBind();
                    ddlItembatchno.Items.Insert(0, new ListItem("Select Batch No", "0"));
                    ddlItembatchno.SelectedValue = ddlBatchNo.SelectedValue.ToString();
                }
                else
                {
                    ddlItembatchno.SelectedValue = ddlBatchNo.SelectedValue.ToString();
                }
                ddlSubComponentCode.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Stores", "ddlSubComponentCode_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlSubComponentName_SelectedIndexChanged
    protected void ddlSubComponentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSubComponentName.SelectedIndex != -1)
            {
                ddlSubComponentCode.SelectedValue = ddlSubComponentName.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlSubComponentCode.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    ddlUnit.SelectedValue = dt.Rows[0]["I_UOM_CODE"].ToString();
                    txtunit.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                else
                {
                    ddlUnit.SelectedIndex = -1;
                }
                ddlSubComponentName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Stores", "ddlSubComponentName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region GridEvents

    #region dgvProductionStoreDetails_RowDeleting
    protected void dgvProductionStoreDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgvProductionStoreDetails_SelectedIndexChanged
    protected void dgvProductionStoreDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region dgvProductionStoreDetails_RowCommand
    protected void dgvProductionStoreDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgvProductionStoreDetails.Rows[Convert.ToInt32(ViewState["Index"].ToString())];
            string I_code = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
            string QTy = ((Label)(row.FindControl("lblPSD_QTY"))).Text;
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute(" SELECT * FROM ITEM_MASTER WHERE I_CODE='" + I_code + "'");
                if (Convert.ToDouble(dt.Rows[0]["I_CURRENT_BAL"].ToString()) - (Convert.ToDouble(QTy)) < 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Can Not Modify/Delete " + dt.Rows[0]["I_CODENO"].ToString() + " Stock Used In Other Transaction ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgvProductionStoreDetails.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgvProductionStoreDetails.DataSource = ((DataTable)ViewState["dt2"]);
                dgvProductionStoreDetails.DataBind();

                for (int i = 0; i < dgvProductionStoreDetails.Rows.Count; i++)
                {
                    string Code = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
                    if (Code == "")
                    {
                        dgvProductionStoreDetails.Enabled = false;

                    }
                }
                if (dgvProductionStoreDetails.Rows.Count == 0)
                {
                    LoadFilter();
                }
            }
            if (e.CommandName == "Modify")
            {
                LoadIUnit();
                fillCategory();

                DataTable dtitem = CommonClasses.Execute("Select * from ITEM_MASTER where I_CODE='" + ((Label)(row.FindControl("lblPSD_I_CODE"))).Text + "'");

                ddlCategory.SelectedValue = dtitem.Rows[0]["I_CAT_CODE"].ToString();
                ddlCategory_SelectedIndexChanged(null, null);
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlSubComponentCode.SelectedValue = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
                ddlSubComponentName.SelectedValue = ((Label)(row.FindControl("lblPSD_I_CODE"))).Text;
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblUOMCODE"))).Text;
                txtunit.Text = ((Label)(row.FindControl("lblUOM"))).Text;
                txtQty.Text = ((Label)(row.FindControl("lblPSD_QTY"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblPSD_REMARK"))).Text;
                ddlItembatchno.SelectedValue = ((Label)(row.FindControl("lblBT_CODE"))).Text;
                foreach (GridViewRow gvr in dgvProductionStoreDetails.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion
    #endregion
    #endregion

    #region Methods

    #region LoadIUnit
    private void LoadIUnit()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY I_UOM_NAME");
            ddlUnit.DataSource = dt;
            ddlUnit.DataTextField = "I_UOM_NAME";
            ddlUnit.DataValueField = "I_UOM_CODE";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("Select Item Unit", "0"));
            ddlUnit.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store", "LoadIUnit", Ex.Message);
        }
    }
    #endregion

    #region LoadDept
    private void LoadDept()
    {
        try
        {
            DataTable dt = new DataTable();
            if (rbtWithAmt.SelectedValue == "-2147483634")
            {
                dt = CommonClasses.Execute(" select *  from  Department where DEPT_PLANT='" + -2147483647 + "'");
            }
            else
            {
                dt = CommonClasses.Execute(" select *  from  Department where DEPT_PLANT='" + -2147483648 + "'");
            }

            txtPersonName.DataSource = dt;
            txtPersonName.DataTextField = "Dept_Name";
            txtPersonName.DataValueField = "Dept_Name";
            txtPersonName.DataBind();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production ", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO DESC ", ddlBatchNo);
            DataTable dtitembatchno = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO DESC ", ddlItembatchno);
            ddlBatchNo.Items.Insert(0, new ListItem("Please Select Batch NO", "0"));
            DataTable dtItem = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND  I_CAT_CODE<>-2147483648 OR I_CODE=-2147479804  AND  ES_DELETE=0 ORDER BY I_NAME ASC");

            ddlSubComponentCode.DataSource = dtItem;
            ddlSubComponentCode.DataTextField = "I_CODENO";
            ddlSubComponentCode.DataValueField = "I_CODE";
            ddlSubComponentCode.DataBind();
            ddlSubComponentCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlSubComponentName.DataSource = dtItem;
            ddlSubComponentName.DataTextField = "I_NAME";
            ddlSubComponentName.DataValueField = "I_CODE";
            ddlSubComponentName.DataBind();
            ddlSubComponentName.Items.Insert(0, new ListItem("Select Item Name", "0"));

            LoadIUnit();

            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1 order by P_NAME");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "LoadCombos", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region LoadMaterialReq
    private void LoadMaterialReq()
    {
        try
        {
            DataTable dt = new DataTable();
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dt = CommonClasses.Execute("select distinct MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO from MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL where ES_DELETE=0 and MR_CODE=MRD_MR_CODE and MRD_PROD_QTY<MRD_ISSUE_QTY ORDER BY MR_BATCH_NO DESC");
            }
            else
            {
                dt = CommonClasses.Execute("select distinct MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO from MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL where ES_DELETE=0 and MR_CODE=MRD_MR_CODE  ORDER BY MR_BATCH_NO DESC");
            }
            ddlMatReqNo.DataSource = dt;
            ddlMatReqNo.DataTextField = "MR_BATCH_NO";
            ddlMatReqNo.DataValueField = "MR_CODE";
            ddlMatReqNo.DataBind();
            ddlMatReqNo.Items.Insert(0, new ListItem("Select Material Req.No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store  ", "LoadMaterialReq", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtGinDate.Attributes.Add("readonly", "readonly");
            LoadCombos();
            dt = CommonClasses.Execute("select PS_CODE,PS_GIN_NO,convert(varchar,PS_GIN_DATE,106) as PS_GIN_DATE,PS_TYPE,PS_PERSON_NAME,PS_MR_CODE ,isnull(PS_P_CODE,0) as PS_P_CODE,isnull(PS_BATCH_NO,0) as PS_BATCH_NO from PRODUCTION_TO_STORE_MASTER WHERE PS_CM_COMP_CODE = '" + Session["CompanyCode"].ToString() + "' and PS_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and ES_DELETE=0 ");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["PS_CODE"]);
                txtGinNo.Text = dt.Rows[0]["PS_GIN_NO"].ToString();
                txtGinDate.Text = Convert.ToDateTime(dt.Rows[0]["PS_GIN_DATE"]).ToString("dd MMM yyyy");
                //ddlCustomer.SelectedValue = dt.Rows[0]["PS_P_CODE"].ToString();
                rbtWithAmt.SelectedValue = dt.Rows[0]["PS_P_CODE"].ToString();
               
                ddlBatchNo.SelectedValue = dt.Rows[0]["PS_BATCH_NO"].ToString();
                if (Convert.ToInt32(dt.Rows[0]["PS_TYPE"]) == 1)
                {
                    //ddlType.SelectedIndex = 1;
                    ddlType.SelectedValue = "1";
                    DataTable dtBatch = new DataTable();
                    dtBatch = CommonClasses.Execute("SELECT  dbo.SHADE_MASTER.SHM_FORMULA_CODE, isnull(dbo.WORK_ORDER_MASTER.WO_P_CODE,0) as WO_P_CODE FROM  dbo.BATCH_MASTER INNER JOIN  dbo.SHADE_MASTER ON dbo.BATCH_MASTER.BT_SHM_CODE = dbo.SHADE_MASTER.SHM_CODE LEFT OUTER JOIN  dbo.WORK_ORDER_MASTER ON dbo.BATCH_MASTER.BT_WO_CODE = dbo.WORK_ORDER_MASTER.WO_CODE where dbo.BATCH_MASTER.BT_CODE='" + ddlBatchNo.SelectedValue + "'");
                    if (dtBatch.Rows.Count > 0)
                    {
                        ddlCustomer.SelectedValue = dtBatch.Rows[0]["WO_P_CODE"].ToString();
                        txtFormula.Text = dtBatch.Rows[0]["SHM_FORMULA_CODE"].ToString();
                    }
                    else
                    {
                        ddlCustomer.SelectedIndex = -1;
                        txtFormula.Text = "";
                    }
                }
                else if (Convert.ToInt32(dt.Rows[0]["PS_TYPE"]) == 2)
                {
                    //ddlType.SelectedIndex = 2;
                    ddlType.SelectedValue = "2";
                    ddlMatReqNo.SelectedIndex = 0;
                    ddlMatReqNo.Enabled = false;
                    ddlType.Enabled = false;

                    ddlType_SelectedIndexChanged(null, null);
                }
                else
                {
                    ddlType.Enabled = false;
                    ddlType.SelectedIndex = 0;
                }
                txtPersonName.Text = dt.Rows[0]["PS_PERSON_NAME"].ToString();

                if (ddlType.SelectedIndex == 1)
                {
                    dtProductionDetail = CommonClasses.Execute("select distinct PSD_I_CODE as PSD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME as UOM,0 as UOM_CODE,cast(PSD_QTY as numeric(10,3)) as PSD_QTY,PSD_REMARK,0 AS BT_CODE,0 AS BT_NO  from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER where PS_CODE=PSD_PS_CODE and PSD_I_CODE=I_CODE and PS_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE ");
                }
                else
                {
                    dtProductionDetail = CommonClasses.Execute("select distinct PSD_I_CODE as PSD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME as UOM,0 as UOM_CODE,cast(PSD_QTY as numeric(10,3)) as PSD_QTY,PSD_REMARK,0 AS BT_CODE,0 AS BT_NO  from PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER where PS_CODE=PSD_PS_CODE and PSD_I_CODE=I_CODE and PS_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE ");
                }
                if (dtProductionDetail.Rows.Count != 0)
                {
                    ViewState["dt2"] = dtFilter;
                    rbtWithAmt_SelectedIndexChanged(null, null);
                    dgvProductionStoreDetails.DataSource = dtProductionDetail;
                    dgvProductionStoreDetails.DataBind();
                    ViewState["dt2"] = dtProductionDetail;
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PRODUCTION_TO_STORE_MASTER", "MODIFY", "PS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                ddlBatchNo.Enabled = false;
                txtPersonName.Enabled = false;
                ddlType.Enabled = false;
                rbtWithAmt.Enabled = false;
            }
            if (str == "VIEW")
            {
                rbtWithAmt.Enabled = false;
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                txtGinNo.Text = productionStore_BL.PS_GIN_NO.ToString();
                txtGinDate.Text = Convert.ToDateTime(productionStore_BL.PS_GIN_DATE).ToString("dd MMM yyyy");
                ddlType.SelectedIndex = productionStore_BL.PS_TYPE;
                ddlMatReqNo.SelectedValue = productionStore_BL.PS_MR_CODE.ToString();
                txtPersonName.Text = productionStore_BL.PS_PERSON_NAME.ToString();
                ddlBatchNo.SelectedValue = productionStore_BL.PS_BATCH_NO.ToString();
                ddlCustomer.SelectedValue = productionStore_BL.ToString();
                res = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            productionStore_BL.PS_GIN_NO = Convert.ToInt32(txtGinNo.Text);
            productionStore_BL.PS_CM_COMP_CODE = Convert.ToInt32(Session["CompanyCode"]);
            productionStore_BL.PS_GIN_DATE = Convert.ToDateTime(txtGinDate.Text);
            productionStore_BL.PS_TYPE = Convert.ToInt32(ddlType.SelectedValue);
            productionStore_BL.PS_PERSON_NAME = txtPersonName.Text;
            productionStore_BL.PS_BATCH_NO = Convert.ToInt32(ddlBatchNo.SelectedValue);
            productionStore_BL.PS_P_CODE = Convert.ToInt32(rbtWithAmt.SelectedValue);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(cast((isnull(PS_GIN_NO,0)) as numeric(10,0))) as PS_GIN_NO from PRODUCTION_TO_STORE_MASTER where PS_CM_COMP_CODE='" + Session["CompanyCode"] + "' ");
        if (dt.Rows[0]["PS_GIN_NO"] == null || dt.Rows[0]["PS_GIN_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["PS_GIN_NO"]) + 1;
        }
        return GenGINNO.ToString();
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
                productionStore_BL = new ProductionToStore_BL();
                txtGinNo.Text = Numbering();
                if (Setvalues())
                {
                    if (productionStore_BL.Save(dgvProductionStoreDetails, "INSERT"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PS_CODE) from PRODUCTION_TO_STORE_MASTER");
                        CommonClasses.WriteLog("Production To Store", "Save", "Production To Store", productionStore_BL.PS_GIN_NO.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewProductionToStore.aspx", false);
                    }
                    else
                    {
                        if (productionStore_BL.Msg != "")
                        {
                            productionStore_BL.Msg = "";
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Saved";
                        }
                        txtGinDate.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                productionStore_BL = new ProductionToStore_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (productionStore_BL.Save(dgvProductionStoreDetails, "UPDATE"))
                    {
                        CommonClasses.RemoveModifyLock("PRODUCTION_TO_STORE_MASTER", "MODIFY", "PS_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Production To Store", "Save", "Production To Store", productionStore_BL.PS_GIN_NO.ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewProductionToStore.aspx", false);
                    }
                    else
                    {
                        if (productionStore_BL.Msg != "")
                        {
                            productionStore_BL.Msg = "";
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Saved";
                        }
                        txtGinDate.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select MODIFY from PRODUCTION_TO_STORE_MASTER where PS_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store -ADD", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Production To Store- ADD", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlSubComponentCode.SelectedIndex = -1;
            ddlSubComponentName.SelectedIndex = -1;
            ddlUnit.SelectedIndex = -1;
            txtQty.Text = "0.000";
            txtRemark.Text = "";
            txtunit.Text = "";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Production To Store", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
         
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("PSD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("UOM_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PSD_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PSD_REMARK", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgvProductionStoreDetails.DataSource = dtFilter;
                dgvProductionStoreDetails.DataBind();
                dgvProductionStoreDetails.Enabled = false;
                ViewState["dt2"] = dtFilter;
            }
        
    }
    #endregion

    #region Enabale
    public static void EnabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = true;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = true;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = true;
                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = true;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        EnabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region Diabale
    public static void DiabaleTextBoxes(Control p1)
    {
        try
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Enabled = false;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.Enabled = false;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t1 = ctrl as CheckBox;

                    if (t1 != null)
                    {
                        t1.Enabled = false;
                    }
                }
                if (ctrl is GridView)
                {
                    GridView t1 = ctrl as GridView;

                    if (t1 != null)
                    {
                        t1.Enabled = false;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        DiabaleTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlType.SelectedIndex == 1)
            {
                txtQty.Text = "0.00";
                txtQty.Enabled = false;
            }
            else
            {
                txtQty.Text = "0.00";
                txtQty.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Production To Store", "ddlType_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlMatReqNo_SelectedIndexChanged
    protected void ddlMatReqNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMatReqNo.SelectedIndex != -1 && ddlMatReqNo.SelectedIndex != 0)
            {
                CommonClasses.FillCombo("MATERIAL_REQUISITION_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER", "I_NAME", "I_CODE", "MR_I_CODE=ITEM_MASTER.I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and MR_CODE='" + ddlMatReqNo.SelectedValue + "'", ddlSubComponentName);
                ddlSubComponentName.Items.Insert(0, new ListItem("Select Item", "0"));
                CommonClasses.FillCombo("MATERIAL_REQUISITION_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER", "I_CODENO", "I_CODE", "MR_I_CODE=ITEM_MASTER.I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and MR_CODE='" + ddlMatReqNo.SelectedValue + "'", ddlSubComponentCode);
                ddlSubComponentCode.Items.Insert(0, new ListItem("Select Item", "0"));

                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("select I_CODENO,MR_I_CODE as PSD_I_CODE,I_NAME,I_UOM_NAME as UOM ,cast(0 as numeric(10,3)) as PSD_QTY,'' as PSD_REMARK from MATERIAL_REQUISITION_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER where MR_I_CODE=ITEM_MASTER.I_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and MR_CODE='" + ddlMatReqNo.SelectedValue + "'");

                if (dt.Rows.Count > 0)
                {
                    dgvProductionStoreDetails.DataSource = dt;
                    dgvProductionStoreDetails.DataBind();
                    dgvProductionStoreDetails.Enabled = true;
                    ViewState["dt2"] = dt;
                }
                string CustCode = DL_DBAccess.GetColumn("select CPOM_P_CODE from MATERIAL_REQUISITION_MASTER,CUSTPO_MASTER where MR_CPOM_CODE=CPOM_CODE and MR_CODE='" + ddlMatReqNo.SelectedValue + "'");
                if (CustCode != "")
                {
                    ddlCustomer.SelectedValue = CustCode;
                    ddlCustomer.Enabled = false;
                }
                ddlBatchNo.Enabled = false;
                ddlBatchNo.SelectedValue = DL_DBAccess.GetColumn("select MR_BATCH_CODE from MATERIAL_REQUISITION_MASTER where MR_CODE='" + ddlMatReqNo.SelectedValue + "'");
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlMatReqNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlBatchNo_SelectedIndexChanged
    protected void ddlBatchNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlBatchNo.SelectedIndex != -1 && ddlBatchNo.SelectedIndex != 0)
            {
                DataTable dtItem = CommonClasses.Execute("select distinct I_CODE,I_CODENO,I_NAME,I_UOM_CODE FROM ITEM_MASTER,BATCH_MASTER WHERE BATCH_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and BT_WOD_I_CODE=I_CODE and BATCH_MASTER.ES_DELETE='0' and  I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and BT_CODE='" + ddlBatchNo.SelectedValue + "'  ORDER BY I_NAME ASC");

                ddlSubComponentCode.DataSource = dtItem;
                ddlSubComponentCode.DataTextField = "I_CODENO";
                ddlSubComponentCode.DataValueField = "I_CODE";
                ddlSubComponentCode.DataBind();
                ddlSubComponentCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

                ddlSubComponentName.DataSource = dtItem;
                ddlSubComponentName.DataTextField = "I_NAME";
                ddlSubComponentName.DataValueField = "I_CODE";
                ddlSubComponentName.DataBind();
                ddlSubComponentName.Items.Insert(0, new ListItem("Select Item Name", "0"));
                if (dtItem.Rows.Count > 0)
                {
                    ddlSubComponentCode.SelectedIndex = 1;
                    ddlSubComponentName.SelectedIndex = 1;
                    ddlUnit.SelectedValue = dtItem.Rows[0]["I_UOM_CODE"].ToString();
                }
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT  dbo.SHADE_MASTER.SHM_FORMULA_CODE, isnull(dbo.WORK_ORDER_MASTER.WO_P_CODE,0) as WO_P_CODE FROM  dbo.BATCH_MASTER INNER JOIN  dbo.SHADE_MASTER ON dbo.BATCH_MASTER.BT_SHM_CODE = dbo.SHADE_MASTER.SHM_CODE LEFT OUTER JOIN  dbo.WORK_ORDER_MASTER ON dbo.BATCH_MASTER.BT_WO_CODE = dbo.WORK_ORDER_MASTER.WO_CODE where dbo.BATCH_MASTER.BT_CODE='" + ddlBatchNo.SelectedValue + "'");

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
                ddlSubComponentCode_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlBatchNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion
    #endregion

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            if (no > 15)
            {
                no = 15;
            }
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {
                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {

            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion


    #region ItemCodeAll
    private void ItemCodeAll(string category)
    {

        DataTable dt = new DataTable();
        dt = CommonClasses.Execute(" select distinct(I_CODE),I_CODENO ,I_NAME from ITEM_MASTER where ES_DELETE=0 AND I_ACTIVE_IND=1 AND I_CAT_CODE='" + category + "' ORDER BY I_CODENO");
        ddlSubComponentCode.DataSource = dt;
        ddlSubComponentCode.DataTextField = "I_CODENO";
        ddlSubComponentCode.DataValueField = "I_CODE";
        ddlSubComponentCode.DataBind();
        ddlSubComponentCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
        ddlSubComponentCode.SelectedIndex = -1;

        ddlSubComponentName.DataSource = dt;
        ddlSubComponentName.DataTextField = "I_NAME";
        ddlSubComponentName.DataValueField = "I_CODE";
        ddlSubComponentName.DataBind();
        ddlSubComponentName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        ddlSubComponentName.SelectedIndex = -1;


    }
    #endregion

    #region ddlCategory_SelectedIndexChanged
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCategory.SelectedIndex != 0)
            {
                ItemCodeAll(ddlCategory.SelectedValue.ToString());
            }
            else
            {
                ddlCategory.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    private void fillCategory()
    {
        DataTable dtCategory = CommonClasses.FillCombo("ITEM_CATEGORY_MASTER", "I_CAT_NAME", "I_CAT_CODE", "ES_DELETE=0 AND   I_CAT_CM_COMP_ID=" + (string)Session["CompanyId"] + "  AND  I_CAT_CODE<>-2147483648  ORDER BY I_CAT_NAME", ddlCategory);
        ddlCategory.Items.Insert(0, new ListItem("Select Category ", "0"));
    }

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtQty.Text);
        txtQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtGinDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {
        }
        else
        {
            if (Convert.ToDateTime(txtGinDate.Text) >= DateTime.Now)
            {
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtGinDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }
    }


    #region rbtWithAmt_SelectedIndexChanged
    protected void rbtWithAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFilter();
        ((DataTable)ViewState["dt2"]).Clear();
        
        LoadDept();
        
        
        //ddlItemCode_SelectedIndexChanged(null, null);
    }
    #endregion rbtWithAmt_SelectedIndexChanged
}

