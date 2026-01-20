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

public partial class Transactions_ADD_MaterialRequisition : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
    MaterialRequisition_BL materialRequsition_BL = null;
    static int mlCode = 0;

    public static string str = "";
    static DataTable dt2 = new DataTable();
    DataTable dtRequsitionDetail = new DataTable();
    static string ItemUpdateIndex = "-1";
    DataRow dr;
    public static int Index = 0;

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
                    txtDate.Attributes.Add("readonly", "readonly");
                    LoadFilter();
                  
                    if (Request.QueryString[0].Equals("VIEW"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        materialRequsition_BL = new MaterialRequisition_BL(mlCode);
                        FillCombo("VIEW");
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        materialRequsition_BL = new MaterialRequisition_BL(mlCode);
                        FillCombo("MODIFY");
                        EnabaleTextBoxes(MainPanel);
                        ViewRec("MOD");

                    }
                    else if (Request.QueryString[0].Equals("AMEND"))
                    {                       
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        materialRequsition_BL = new MaterialRequisition_BL(mlCode);
                        FillCombo("MODIFY");
                        EnabaleTextBoxes(MainPanel);
                        ViewRec("AMEND");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtDate.Attributes.Add("readonly","readonly");
                        FillCombo("INSERT");

                        dt2.Rows.Clear();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                      
                        dgvBOMaterialDetails.Enabled = false;
                        // LoadFilter();
                    }
                    txtDepartment.Focus();
                    ddlFormualCode.Enabled = false;
                    ddlType.Enabled = false;
                    ddlOrderNo.Enabled = false;
                    ddlItemName.Enabled = false;
                    //LoadFilter();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region User Defined Method


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

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgvBOMaterialDetails.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("STEP_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("WEIGHT_IN_KG", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("MR_BALANCE_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ADD_IN_QTY", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgvBOMaterialDetails.DataSource = dtFilter;
                dgvBOMaterialDetails.DataBind();
            }
        }
    }
    #endregion

    #region FillCombo
    private void FillCombo(string type)
    {
        try
        {
            DataTable dtItembatchno=new DataTable ();
            //CommonClasses.FillCombo("CUSTPO_MASTER", "CPOM_PONO", "CPOM_CODE", "ES_DELETE=0 and CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlOrderNo);
            CommonClasses.FillCombo("WORK_ORDER_MASTER", "WO_NO", "WO_CODE", "ES_DELETE=0 and WO_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlOrderNo);
            ddlOrderNo.Items.Insert(0, new ListItem("Select Order", "0"));
            DataTable dt = CommonClasses.FillCombo("ITEM_MASTER", "(I_CODENO+' ('+(I_NAME)+')') as I_NAME", "I_CODE", "ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE IN ('-2147483646','-2147483647') ");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));

            DataTable dtItem = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0  ORDER BY I_NAME");

            ddlRawComponentCode.DataSource = dtItem;
            ddlRawComponentCode.DataTextField = "I_CODENO";
            ddlRawComponentCode.DataValueField = "I_CODE";
            ddlRawComponentCode.DataBind();
            ddlRawComponentCode.Items.Insert(0, new ListItem("Select Material Code", "0"));


            ddlRawComponentName.DataSource = dtItem;
            ddlRawComponentName.DataTextField = "I_NAME";
            ddlRawComponentName.DataValueField = "I_CODE";
            ddlRawComponentName.DataBind();
            ddlRawComponentName.Items.Insert(0, new ListItem("Select Material Name", "0"));


            if (type == "INSERT")
            {
                //dt = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and BT_IS_MAT_REQUSITION=0", ddlbatchNo);
                dt = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO DESC ", ddlbatchNo);
                ddlbatchNo.Items.Insert(0, new ListItem("Please Select Batch NO", "0"));
               dtItembatchno = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO DESC ", ddlItembatchno );
                ddlItembatchno.Items.Insert(0, new ListItem("Please Select Batch NO", "0"));


            }
            else
            {
                dt = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  order by BT_NO DESC ", ddlbatchNo);
                ddlbatchNo.Items.Insert(0, new ListItem("Please Select Batch NO", "0"));

                dtItembatchno = CommonClasses.FillCombo("BATCH_MASTER", "BT_NO", "BT_CODE", "ES_DELETE=0 AND BT_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by BT_NO DESC ", ddlItembatchno);
                ddlItembatchno.Items.Insert(0, new ListItem("Please Select Batch NO", "0"));

            }

            dt = CommonClasses.FillCombo("SHADE_MASTER", "SHM_FORMULA_CODE", "SHM_CODE", "ES_DELETE=0 ", ddlFormualCode);
            ddlFormualCode.Items.Insert(0, new ListItem("Please Select Formula Code", "0"));

            dt = CommonClasses.FillCombo("PROCESS_MASTER", "PROCESS_NAME", "PROCESS_CODE", "ES_DELETE=0 AND PROCESS_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlProcess);
            ddlProcess.Items.Insert(0, new ListItem("Please Select Process Type", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Requisition", "FillCombo", Ex.Message);
        }

    }

    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            FillCombo("VIEW");           
            txtDate.Attributes.Add("readonly", "readonly");
            dt = CommonClasses.Execute("select MR_CODE,MR_DEPT_NAME,convert(varchar,MR_DATE,106) as MR_DATE,MR_BATCH_NO,MR_FORMULA,MR_TYPE,MR_CPOM_CODE,MR_I_CODE,MR_FORMULA_CODE,MR_BATCH_CODE,MR_FORMULA_CODE  from MATERIAL_REQUISITION_MASTER WHERE MR_COMP_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and MR_CODE='" + mlCode + "' and ES_DELETE=0 ");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["MR_CODE"]);
                txtDepartment.Text = dt.Rows[0]["MR_DEPT_NAME"].ToString();
                txtBatchNo.Text = dt.Rows[0]["MR_BATCH_NO"].ToString();
                ddlbatchNo.SelectedValue =Convert.ToInt32(dt.Rows[0]["MR_BATCH_CODE"]).ToString();
                ddlOrderNo.SelectedValue = Convert.ToInt32(dt.Rows[0]["MR_BATCH_CODE"]).ToString();
                ddlFormualCode.SelectedValue = Convert.ToInt32(dt.Rows[0]["MR_FORMULA_CODE"]).ToString();
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["MR_DATE"]).ToString("dd MMM yyyy");

                //txtFormula.Text = (dt.Rows[0]["MR_FORMULA"]).ToString();
                if (dt.Rows[0]["MR_TYPE"].ToString() == "As Per Batch")
                {
                    ddlType.SelectedIndex = 1;

                }
                else
                {
                    ddlType.SelectedIndex = 2;

                }

                ddlType_SelectedIndexChanged(null, null);
                ddlOrderNo.SelectedValue = dt.Rows[0]["MR_CPOM_CODE"].ToString();
                //ddlOrderNo_SelectedIndexChanged(null, null);
                ddlItemName.SelectedValue = dt.Rows[0]["MR_I_CODE"].ToString();
                ddlItemName.Enabled = false;
                ddlType.Enabled = false;
                ddlOrderNo.Enabled = false;
                ddlbatchNo.Enabled = false;
                ddlFormualCode.Enabled = false;
                if (ddlType.SelectedIndex == 1)
                {
                    //dtRequsitionDetail = CommonClasses.Execute("select distinct MRD_PROCESS_CODE as PROCESS_CODE ,PROCESS_NAME,MRD_I_CODE as BD_I_CODE,I_CODENO,I_NAME,cast(isnull(MRD_REQ_QTY,0) as numeric(10,3)) as BD_VQTY,Round(isnull(MRD_REQ_QTY,0)*isnull(I_DENSITY,0),3) as WEIGHT_IN_KG,cast(isnull(MRD_ADD_IN,0) as numeric(10,3)) as ADD_IN_QTY,cast(isnull(I_CURRENT_BAL,0) as numeric(10,3)) as I_CURRENT_BAL,cast(isnull(I_MIN_LEVEL,0) as numeric(10,3)) as I_MIN_LEVEL,MRD_STEPS_NO as STEP_NO,cast(isnull(MRD_REQ_QTY-MRD_ISSUE_QTY,0) as numeric(10,3)) as MR_BALANCE_QTY,MRD_BT_CODE as BT_CODE from MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_MASTER,PROCESS_MASTER where MR_CODE=MRD_MR_CODE and MRD_I_CODE=I_CODE and MR_CODE='" + mlCode + "' and MRD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 order by MRD_STEPS_NO");
                    dtRequsitionDetail = CommonClasses.Execute("select distinct MATERIAL_REQUISITION_DETAIL.MRD_PROCESS_CODE as PROCESS_CODE ,PROCESS_MASTER.PROCESS_NAME,MATERIAL_REQUISITION_DETAIL.MRD_I_CODE as BD_I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,cast(isnull(MATERIAL_REQUISITION_DETAIL.MRD_REQ_QTY,0) as numeric(10,3)) as BD_VQTY,Round(isnull(MATERIAL_REQUISITION_DETAIL.MRD_REQ_QTY,0)*isnull(ITEM_MASTER.I_DENSITY,0),3) as WEIGHT_IN_KG,cast(isnull(MATERIAL_REQUISITION_DETAIL.MRD_ADD_IN,0) as numeric(10,3)) as ADD_IN_QTY,cast(isnull(ITEM_MASTER.I_CURRENT_BAL,0) as numeric(10,3)) as I_CURRENT_BAL,cast(isnull(ITEM_MASTER.I_MIN_LEVEL,0) as numeric(10,3)) as I_MIN_LEVEL,MATERIAL_REQUISITION_DETAIL.MRD_STEPS_NO as STEP_NO,cast(isnull(MATERIAL_REQUISITION_DETAIL.MRD_REQ_QTY-MATERIAL_REQUISITION_DETAIL.MRD_ISSUE_QTY,0) as numeric(10,3)) as MR_BALANCE_QTY,BT_CODE,BT_NO from MATERIAL_REQUISITION_MASTER INNER JOIN MATERIAL_REQUISITION_DETAIL ON MATERIAL_REQUISITION_MASTER.MR_CODE=MATERIAL_REQUISITION_DETAIL.MRD_MR_CODE LEFT OUTER JOIN BATCH_MASTER ON MATERIAL_REQUISITION_DETAIL.MRD_BT_CODE=BATCH_MASTER.BT_CODE INNER JOIN ITEM_MASTER ON MATERIAL_REQUISITION_DETAIL.MRD_I_CODE=ITEM_MASTER.I_CODE INNER JOIN PROCESS_MASTER ON MATERIAL_REQUISITION_DETAIL.MRD_PROCESS_CODE=PROCESS_MASTER.PROCESS_CODE  where MATERIAL_REQUISITION_MASTER.MR_CODE= "+mlCode+" and  PROCESS_MASTER.ES_DELETE=0 order by MATERIAL_REQUISITION_DETAIL.MRD_STEPS_NO");
                }
                else
                {
                    //dtRequsitionDetail = CommonClasses.Execute("select distinct MRD_PROCESS_CODE as PROCESS_CODE,PROCESS_NAME,MRD_I_CODE as BD_I_CODE,I_CODENO,I_NAME,cast(isnull(MRD_REQ_QTY,0) as numeric(10,3)) as BD_VQTY,Round(isnull(MRD_REQ_QTY,0)*isnull(I_DENSITY,0),3) as WEIGHT_IN_KG,cast(isnull(MRD_ADD_IN,0) as numeric(10,3)) as ADD_IN_QTY,cast(isnull(I_CURRENT_BAL,0) as numeric(10,3)) as I_CURRENT_BAL,cast(isnull(I_MIN_LEVEL,0) as numeric(10,3)) as I_MIN_LEVEL,MRD_STEPS_NO as STEP_NO,cast(isnull(MRD_REQ_QTY-MRD_ISSUE_QTY,0) as numeric(10,3)) as MR_BALANCE_QTY,MRD_BT_CODE as BT_CODE from MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL,ITEM_MASTER,PROCESS_MASTER where MR_CODE=MRD_MR_CODE and MRD_I_CODE=I_CODE and MR_CODE='" + mlCode + "' and MRD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 order by MRD_STEPS_NO");
                    dtRequsitionDetail = CommonClasses.Execute("select distinct MATERIAL_REQUISITION_DETAIL.MRD_PROCESS_CODE as PROCESS_CODE ,PROCESS_MASTER.PROCESS_NAME,MATERIAL_REQUISITION_DETAIL.MRD_I_CODE as BD_I_CODE,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME,cast(isnull(MATERIAL_REQUISITION_DETAIL.MRD_REQ_QTY,0) as numeric(10,3)) as BD_VQTY,Round(isnull(MATERIAL_REQUISITION_DETAIL.MRD_REQ_QTY,0)*isnull(ITEM_MASTER.I_DENSITY,0),3) as WEIGHT_IN_KG,cast(isnull(MATERIAL_REQUISITION_DETAIL.MRD_ADD_IN,0) as numeric(10,3)) as ADD_IN_QTY,cast(isnull(ITEM_MASTER.I_CURRENT_BAL,0) as numeric(10,3)) as I_CURRENT_BAL,cast(isnull(ITEM_MASTER.I_MIN_LEVEL,0) as numeric(10,3)) as I_MIN_LEVEL,MATERIAL_REQUISITION_DETAIL.MRD_STEPS_NO as STEP_NO,cast(isnull(MATERIAL_REQUISITION_DETAIL.MRD_REQ_QTY-MATERIAL_REQUISITION_DETAIL.MRD_ISSUE_QTY,0) as numeric(10,3)) as MR_BALANCE_QTY,BT_CODE,BT_NO from MATERIAL_REQUISITION_MASTER INNER JOIN MATERIAL_REQUISITION_DETAIL ON MATERIAL_REQUISITION_MASTER.MR_CODE=MATERIAL_REQUISITION_DETAIL.MRD_MR_CODE LEFT OUTER JOIN BATCH_MASTER ON MATERIAL_REQUISITION_DETAIL.MRD_BT_CODE=BATCH_MASTER.BT_CODE INNER JOIN ITEM_MASTER ON MATERIAL_REQUISITION_DETAIL.MRD_I_CODE=ITEM_MASTER.I_CODE INNER JOIN PROCESS_MASTER ON MATERIAL_REQUISITION_DETAIL.MRD_PROCESS_CODE=PROCESS_MASTER.PROCESS_CODE  where MATERIAL_REQUISITION_MASTER.MR_CODE= " + mlCode + " and  PROCESS_MASTER.ES_DELETE=0 order by MATERIAL_REQUISITION_DETAIL.MRD_STEPS_NO");
                }
                if (dtRequsitionDetail.Rows.Count != 0)
                {
                    dgvBOMaterialDetails.DataSource = dtRequsitionDetail;
                    dgvBOMaterialDetails.DataBind();
                    dt2 = dtRequsitionDetail;
                }
                CalculateTotal();

            }
            if (str == "MOD" || str == "AMEND")
            {
                CommonClasses.SetModifyLock("MATERIAL_REQUISITION_MASTER", "MODIFY", "MR_CODE", mlCode);
            }
            if (str == "VIEW")
            {
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
                txtDepartment.Text = materialRequsition_BL.MR_DEPT_NAME;
                
                txtBatchNo.Text = materialRequsition_BL.MR_BATCH_NO;
                txtDate.Text = Convert.ToDateTime(materialRequsition_BL.MR_DATE).ToString("dd MMM yyyy");
                ddlbatchNo.SelectedValue = materialRequsition_BL.MR_BATCH_CODE.ToString();
                ddlFormualCode.SelectedValue = materialRequsition_BL.MR_FORMULA_CODE.ToString();
                //txtFormula.Text = materialRequsition_BL.MR_FORMULA;
                ddlType.SelectedItem.Text = materialRequsition_BL.MR_TYPE;
                ddlOrderNo.SelectedValue = materialRequsition_BL.MR_CPOM_CODE.ToString();
                ddlItemName.SelectedValue = materialRequsition_BL.MR_I_CODE.ToString();

            }

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Reqisition", "GetValues", ex.Message);
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

            materialRequsition_BL.MR_DEPT_NAME = txtDepartment.Text;
            materialRequsition_BL.MR_BATCH_NO =txtBatchNo.Text;
            materialRequsition_BL.MR_BATCH_CODE = Convert.ToInt32(ddlbatchNo.SelectedValue);

            materialRequsition_BL.MR_DATE = Convert.ToDateTime(txtDate.Text);
            materialRequsition_BL.MR_FORMULA = "";
            materialRequsition_BL.MR_FORMULA_CODE =Convert.ToInt32(ddlFormualCode.SelectedValue);
            materialRequsition_BL.MR_TYPE = ddlType.SelectedItem.ToString();
            if (ddlType.SelectedIndex != 1)
            {
                materialRequsition_BL.MR_CPOM_CODE = 0;
            }
            else
            {
                materialRequsition_BL.MR_CPOM_CODE = Convert.ToInt32(ddlOrderNo.SelectedValue);
            }
            materialRequsition_BL.MR_I_CODE = Convert.ToInt32(ddlItemName.SelectedValue);
            materialRequsition_BL.MR_UM_CODE = Convert.ToInt32(Session["UserCode"]);
            materialRequsition_BL.MR_COMP_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Reqisition", "Setvalues", ex.Message);
        }
        return res;
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

                materialRequsition_BL = new MaterialRequisition_BL();
                txtBatchNo.Text = Numbering();
                if (Setvalues())
                {
                    if (materialRequsition_BL.Save(dgvBOMaterialDetails, "INSERT"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(MR_CODE) from MATERIAL_REQUISITION_MASTER");
                        CommonClasses.WriteLog("Material Requsition", "Save", "Material Requsition", materialRequsition_BL.MR_BATCH_NO.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
                    }
                    else
                    {
                        if (materialRequsition_BL.Msg != "")
                        {
                            //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                            materialRequsition_BL.Msg = "";
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Saved";
                        }
                        txtDepartment.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                materialRequsition_BL = new MaterialRequisition_BL(mlCode);

                if (Setvalues())
                {
                    if (materialRequsition_BL.Save(dgvBOMaterialDetails, "UPDATE"))
                    {
                        CommonClasses.RemoveModifyLock("MATERIAL_REQUISITION_MASTER", "MODIFY", "MR_CODE", mlCode);
                        CommonClasses.WriteLog("Material Requsition", "Update", "Material Requsition", materialRequsition_BL.MR_BATCH_NO.ToString(), Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
                    }
                    else
                    {
                        if (materialRequsition_BL.Msg != "")
                        {
                            //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                            materialRequsition_BL.Msg = "";
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Saved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        txtDepartment.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("AMEND"))
            {
                int AMEND_COUNT = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = CommonClasses.Execute("select isnull(MR_AM_COUNT,0) as MR_AM_COUNT from MATERIAL_REQUISITION_MASTER WHERE MR_COMP_CM_CODE = '" + Convert.ToInt32(Session["CompanyCode"]) + "' and ES_DELETE=0 and MR_CODE='" + mlCode + "'");
                if (dt.Rows.Count > 0)
                {
                    AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["MR_AM_COUNT"]);
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (AMEND_COUNT == 0)
                {
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                CommonClasses.Execute1("update  MATERIAL_REQUISITION_MASTER set MR_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',MR_AM_COUNT='" + AMEND_COUNT + "' WHERE MR_CODE='" + mlCode + "'");
                if (CommonClasses.Execute1("INSERT INTO MATERIAL_REQUISITION_AMD_MASTER select * from MATERIAL_REQUISITION_MASTER where MR_CODE='" + mlCode + "' "))
                {
                    string MatserCode = CommonClasses.GetMaxId("Select Max(AM_CODE) from MATERIAL_REQUISITION_AMD_MASTER");
                    if (CommonClasses.Execute1("INSERT INTO MATERIAL_REQUISITION_AMD_DETAIL select MRD_CODE,MRD_MR_CODE,MRD_I_CODE,MRD_REQ_QTY,MRD_ISSUE_QTY,MRD_PROD_QTY,MRD_PURC_REQ_QTY,MRD_PROCESS_CODE,MRD_STEPS_NO,MRD_ADD_IN,'" + MatserCode + "' as AMD_AM_CODE from MATERIAL_REQUISITION_DETAIL where MRD_MR_CODE='" + mlCode + "' "))
                    {
                       // CommonClasses.Execute1("update  MATERIAL_REQUISITION_AMD_DETAIL set AMD_AM_CODE='" + MatserCode + "' WHERE AMD_AM_CODE is NULL");
                       
                        #region ModifyOriginalPO
                       
                        materialRequsition_BL = new MaterialRequisition_BL(mlCode);

                        if (Setvalues())
                        {
                            if (materialRequsition_BL.Save(dgvBOMaterialDetails, "UPDATE"))
                            {
                                CommonClasses.RemoveModifyLock("MATERIAL_REQUISITION_MASTER", "MODIFY", "MR_CODE", mlCode);
                                result = CommonClasses.Execute1("update  MATERIAL_REQUISITION_AMD_MASTER set MR_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE MR_CODE='" + mlCode + "' and MR_AM_COUNT='" + AMEND_COUNT + "'");
                                CommonClasses.WriteLog("Material Requsition", "Update", "Material Requsition", materialRequsition_BL.MR_BATCH_NO.ToString(), Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                                result = true;
                                Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
                            }
                            else
                            {
                                if (materialRequsition_BL.Msg != "")
                                {
                                    //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                                    materialRequsition_BL.Msg = "";
                                    PanelMsg.Visible = true;
                                    lblmsg.Text = "Record Not Saved";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                }
                                txtDepartment.Focus();
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                    PanelMsg.Visible = true;
                    lblmsg.Text = "PO Not Amend";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                   
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(cast((isnull(MR_BATCH_NO,0)) as numeric(10,0))) as MR_BATCH_NO from MATERIAL_REQUISITION_MASTER where MR_COMP_CM_CODE='" + Session["CompanyCode"] + "' and ES_DELETE=0 ");
        if (dt.Rows[0]["MR_BATCH_NO"] == null || dt.Rows[0]["MR_BATCH_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["MR_BATCH_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from MATERIAL_REQUISITION_MASTER where MR_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    //PanelMsg.Visible = true;
                    //lblmsg.Text = "Record used by another user";
                    ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requsition -ADD", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Material Requsition - ADD", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    private void InserRecord()
    {
        #region Validations
        if (ddlProcess.SelectedIndex == 0)
        {
            //ShowMessage("#Avisos", "Select Required For", CommonClasses.MSG_Warning);
            //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Required For Process ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlProcess.Focus();
            return;
        }
       
        if (ddlRawComponentCode.SelectedIndex == 0)
        {
            //ShowMessage("#Avisos", "Select Material Code", CommonClasses.MSG_Warning);
            //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Required For Material Code ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlRawComponentCode.Focus();
            return;
        }
        if (ddlRawComponentName.SelectedIndex == 0)
        {
           // ShowMessage("#Avisos", "Select Material Name", CommonClasses.MSG_Warning);
           // ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Required For Material Name ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlRawComponentName.Focus();
            return;
        }        
        double MRD_REQ_QTY = 0;
        if (Convert.ToString(txtVQty.Text) != null || Convert.ToString(txtVQty.Text) != "")
        {
            MRD_REQ_QTY = Math.Round(Convert.ToDouble(txtVQty.Text), 3);
        }
        if (MRD_REQ_QTY == 0.0)
        {
            lblmsg.Text = "Please Enter Required Qty";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtVQty.Focus();
            return;
        }
        double MRD_ADD_QTY = 0;
        if (Convert.ToString(txtAddIn.Text) != null || Convert.ToString(txtAddIn.Text) != "")
        {
            MRD_ADD_QTY = Math.Round(Convert.ToDouble(txtAddIn.Text), 3);
        }
        if (MRD_ADD_QTY == 0.0)
        {
            lblmsg.Text = "Please Enter Add In Qty";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtAddIn.Focus();
            return;
        }
        int MRD_STEP = 0;
        if (Convert.ToString(txtSteps.Text) != null || Convert.ToString(txtSteps.Text) != "")
        {
            MRD_STEP = Convert.ToInt32(txtSteps.Text);
        }
        if (MRD_STEP == 0)
        {
            lblmsg.Text = "Please Enter Steps No";
            PanelMsg.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtSteps.Focus();
            return;
        }         
      
        #endregion

        #region CheckExist
        if (dgvBOMaterialDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE"))).Text;
                string PROCESSS_CODE = ((Label)(dgvBOMaterialDetails.Rows[i].FindControl("lblPROCESS_CODE"))).Text;
                string BT_CODE = ((Label)(dgvBOMaterialDetails.Rows[i].FindControl("lblBT_CODE"))).Text;
                if (ItemUpdateIndex == "-1")
                {
                    if (bd_i_code == ddlRawComponentCode.SelectedValue.ToString() && PROCESSS_CODE == ddlProcess.SelectedValue.ToString() && BT_CODE ==ddlItembatchno .SelectedValue.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }

                }
                else
                {
                    if (bd_i_code == ddlRawComponentCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString() && PROCESSS_CODE == ddlProcess.SelectedValue.ToString() && BT_CODE == ddlItembatchno.SelectedValue.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }

                }

            }
        }
        #endregion

        #region Datatable Initialization

        if (dt2.Columns.Count == 0)
        {
            dt2.Columns.Add(new System.Data.DataColumn("STEP_NO", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("PROCESS_CODE", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("WEIGHT_IN_KG", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("MR_BALANCE_QTY", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("ADD_IN_QTY", typeof(String)));
        }
        #endregion

        if (dt2.Rows.Count > 0)
        {
            for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
            {
                double PTS_QTY = 0;
                if (Convert.ToString(((TextBox)dgvBOMaterialDetails.Rows[i].FindControl("txtADD_IN_QTY")).Text) != null || Convert.ToString(((TextBox)dgvBOMaterialDetails.Rows[i].FindControl("txtADD_IN_QTY")).Text) != "")
                {
                    PTS_QTY =Math.Round(Convert.ToDouble(((TextBox)dgvBOMaterialDetails.Rows[i].FindControl("txtADD_IN_QTY")).Text),3);
                }
                dt2.Rows[i]["ADD_IN_QTY"] = PTS_QTY.ToString();
                dt2.AcceptChanges();
            }
        }

        #region Insert Record to Table
        dr = dt2.NewRow();
        dr["PROCESS_CODE"] = ddlProcess.SelectedValue.ToString();
        dr["PROCESS_NAME"] = ddlProcess.SelectedItem.ToString();
        dr["BD_I_CODE"] = ddlRawComponentCode.SelectedValue.ToString();
        dr["I_CODENO"] = ddlRawComponentCode.SelectedItem.ToString();
        dr["I_NAME"] = ddlRawComponentName.SelectedItem.ToString();
        if (ddlItembatchno.SelectedIndex == 0)
        {
            dr["BT_NO"] = "0";
            dr["BT_CODE"] = "0";
        }
        else
        {
            dr["BT_NO"] = ddlItembatchno.SelectedItem.ToString();
            dr["BT_CODE"] = ddlItembatchno.SelectedValue.ToString();
        }
        dr["STEP_NO"] = txtSteps.Text;
        dr["BD_VQTY"] = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtVQty.Text), 3));
        dr["ADD_IN_QTY"] = Math.Round(Convert.ToDouble(txtAddIn.Text),3);
       // dr["ADD_IN_QTY"] = txtAddIn.Text.Trim() == "" ? 0 : Convert.ToInt32(txtAddIn.Text);


        //dr["ADD_IN_QTY"] = Math.Round(Convert.ToDouble(txtAddIn.Text), 3).ToString();
        dr["MR_BALANCE_QTY"] = "0.00" ;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select isnull(I_CURRENT_BAL,0) as I_CURRENT_BAL,isnull(I_MIN_LEVEL,0) as I_MIN_LEVEL,isnull(I_DENSITY,0) as I_DENSITY from ITEM_MASTER where ES_DELETE=0 AND I_CODE='" + ddlRawComponentCode.SelectedValue + "'");
        dr["I_CURRENT_BAL"] = string.Format("{0:0.000}", dt.Rows[0]["I_CURRENT_BAL"].ToString());
        dr["I_MIN_LEVEL"] = string.Format("{0:0.000}", dt.Rows[0]["I_MIN_LEVEL"].ToString());
        double BD_VQTY = Math.Round(Convert.ToDouble(txtVQty.Text), 3);
        double WEIGHT_IN_KG = Math.Round((BD_VQTY * Convert.ToDouble(dt.Rows[0]["I_DENSITY"])), 3);
        dr["WEIGHT_IN_KG"] = string.Format("{0:0.000}", Math.Round(WEIGHT_IN_KG, 3));
        #endregion

        #region InsertData
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

        
            dt2.DefaultView.Sort = "PROCESS_CODE ASC";
            dt2.AcceptChanges();

            DataView dv = dt2.DefaultView;
            dv.Sort = "PROCESS_CODE ASC";
            dt2 = dv.ToTable();

            for (int i = 0; i < dt2.Rows.Count; i++)
        {
            dt2.Rows[i]["STEP_NO"] = ((i + 1) * 10).ToString();
        }

        dgvBOMaterialDetails.DataSource = dt2;       
        dgvBOMaterialDetails.DataBind();
        dgvBOMaterialDetails.Enabled = true;
        CalculateTotal();
        clearDetail();
    }

    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlRawComponentCode.SelectedValue = "0";
            ddlRawComponentName.SelectedValue = "0";
            ddlItembatchno.SelectedValue = "0";
            txtVQty.Text = "0.000";
            txtAddIn.Text = "0.000";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvBOMaterialDetails.Enabled)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Material Not Present.", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }

        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Material Requisition", "btnSubmit_Click", Ex.Message);
        }
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

            CommonClasses.SendError("Material Requisition", "btnInsert_Click", Ex.Message);
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
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("Material Requisition", "btnCancel_Click", ex.Message.ToString());
        }

    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlType.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDepartment.Text == "")
            {
                flag = false;
            }
            else if (ddlItemName.SelectedIndex == 0)
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
            CommonClasses.SendError("Material Requisition", "CheckValid", Ex.Message);
        }

        return flag;

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
                CommonClasses.RemoveModifyLock("MATERIAL_REQUISITION_MASTER", "MODIFY", "MR_CODE", mlCode);
            }
            Response.Redirect("~/Transactions/VIEW/ViewMaterialRequisition.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Reqisition", "btnCancel_Click", Ex.Message);
        }
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

            CommonClasses.SendError("Material Requisition", "btnOk_Click", Ex.Message);
        }
    }
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
            // int n = no - 1;
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

    //#region txtVQty_TextChanged
    //protected void txtVQty_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string totalStr = DecimalMasking(txtVQty.Text);
    //        txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    //        //txtVQty.Text = string.Format("{0:0.000}", Convert.ToDouble(totalStr));
    //    }
    //    catch (Exception Ex)
    //    {

    //        CommonClasses.SendError("Material Requisition", "txtVQty_TextChanged", Ex.Message);
    //    }
    //}
    //#endregion

    //#region txtAddIn_TextChanged
    //protected void txtAddIn_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string totalStr = DecimalMasking(txtAddIn.Text);
    //        txtAddIn.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    //        //txtVQty.Text = string.Format("{0:0.000}", Convert.ToDouble(totalStr));
    //    }
    //    catch (Exception Ex)
    //    {

    //        CommonClasses.SendError("Material Requisition", "txtAddIn_TextChanged", Ex.Message);
    //    }
    //}
    //#endregion


    #region GridEvents

    #region dgvBOMaterialDetails_RowDeleting
    protected void dgvBOMaterialDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgvBOMaterialDetails_SelectedIndexChanged
    protected void dgvBOMaterialDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region dgvBOMaterialDetails_RowCommand
    protected void dgvBOMaterialDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgvBOMaterialDetails.Rows[Index];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgvBOMaterialDetails.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);
                dgvBOMaterialDetails.DataSource = dt2;
                dgvBOMaterialDetails.DataBind();

                for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
                {
                    string Code = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                    if (Code == "")
                    {
                        dgvBOMaterialDetails.Enabled = false;
                    }
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlRawComponentCode.SelectedValue = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                ddlRawComponentCode_SelectedIndexChanged(null, null);
                txtVQty.Text = ((Label)(row.FindControl("lblBD_VQTY"))).Text;
                txtAddIn.Text = ((TextBox)(row.FindControl("txtADD_IN_QTY"))).Text;
                txtSteps.Text = ((Label)(row.FindControl("lblSTEP_NO"))).Text;
                ddlProcess.SelectedValue = ((Label)(row.FindControl("lblPROCESS_CODE"))).Text;
                ddlItembatchno.SelectedValue = ((Label)(row.FindControl("lblBT_CODE"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition", "dgvBOMaterialDetails_RowCommand", Ex.Message);
        }

    }
    #endregion

    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable();
            if (ddlType.SelectedIndex == 1 && ddlOrderNo.SelectedIndex != 0)
            {
                dt = CommonClasses.Execute("select I_CODENO,BD_I_CODE,cast(BD_VQTY*CPOD_ORD_QTY as numeric(10,3)) as BD_VQTY,I_NAME ,I_MIN_LEVEL,I_CURRENT_BAL from BOM_DETAIL,BOM_MASTER,ITEM_MASTER,CUSTPO_MASTER,CUSTPO_DETAIL where BM_CODE=BD_BM_CODE and BOM_MASTER.ES_DELETE=0 AND I_CODE=BD_I_CODE and BM_I_CODE='" + ddlItemName.SelectedValue + "' and CPOD_CPOM_CODE=CPOM_CODE and CPOM_CODE='" + ddlOrderNo.SelectedValue + "' and CPOD_I_CODE=BM_I_CODE");

                ddlRawComponentCode.DataSource = dt;
                ddlRawComponentCode.DataTextField = "I_CODENO";
                ddlRawComponentCode.DataValueField = "BD_I_CODE";
                ddlRawComponentCode.DataBind();
                ddlRawComponentCode.Items.Insert(0, new ListItem("Select  Material Code", "0"));


                ddlRawComponentName.DataSource = dt;
                ddlRawComponentName.DataTextField = "I_NAME";
                ddlRawComponentName.DataValueField = "BD_I_CODE";
                ddlRawComponentName.DataBind();
                ddlRawComponentName.Items.Insert(0, new ListItem("Select  Material Name", "0"));
            
            }
            else
            {
                dt = CommonClasses.Execute("select I_CODENO,BD_I_CODE,cast(BD_VQTY as numeric(10,3)) as BD_VQTY,I_NAME ,I_MIN_LEVEL,I_CURRENT_BAL from BOM_DETAIL,BOM_MASTER,ITEM_MASTER where BM_CODE=BD_BM_CODE and BOM_MASTER.ES_DELETE=0 AND I_CODE=BD_I_CODE and BM_I_CODE='" + ddlItemName.SelectedValue + "'");

                ddlRawComponentCode.DataSource = dt;
                ddlRawComponentCode.DataTextField = "I_CODENO";
                ddlRawComponentCode.DataValueField = "BD_I_CODE";
                ddlRawComponentCode.DataBind();
                ddlRawComponentCode.Items.Insert(0, new ListItem("Select  Material Code", "0"));


                ddlRawComponentName.DataSource = dt;
                ddlRawComponentName.DataTextField = "I_NAME";
                ddlRawComponentName.DataValueField = "BD_I_CODE";
                ddlRawComponentName.DataBind();
                ddlRawComponentName.Items.Insert(0, new ListItem("Select  Material Name", "0"));
            
            
            }
            if (dt.Rows.Count > 0)
            {
                dgvBOMaterialDetails.DataSource = dt;
                dgvBOMaterialDetails.DataBind();
                dt2 = dt;
                dgvBOMaterialDetails.Enabled = true;
            }
            else
            {
                dgvBOMaterialDetails.DataSource = null;
                dgvBOMaterialDetails.DataBind();
                dt2.Clear();
                dgvBOMaterialDetails.Enabled = false;
                PanelMsg.Visible = true;
                lblmsg.Text = "Bill Of Material Is Not Available";
                return;
            }

            
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlItemName_SelectedIndexChanged", ex.Message);
        }
    }

    private void loadItemBatchNo()
    {
        DataTable dtitembatchno = CommonClasses.Execute("select BT_CODE,BT_NO from BATCH_MASTER where BT_WOD_I_CODE="+ddlRawComponentCode.SelectedValue+" and ES_DELETE =0");
        //if (dtitembatchno.Rows.Count > 0)
        //{
            ddlItembatchno.DataSource = dtitembatchno;
            ddlItembatchno.DataTextField = "BT_NO";
            ddlItembatchno.DataValueField = "BT_CODE";
            ddlItembatchno.DataBind();
            ddlItembatchno.Items.Insert(0, new ListItem("Select Batch No", "0"));
        //}

    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dgvBOMaterialDetails.DataSource = null;
            dgvBOMaterialDetails.DataBind();
            LoadFilter();

            if (ddlType.SelectedIndex == 1)
            {
                //CommonClasses.FillCombo("CUSTPO_MASTER", "CPOM_PONO", "CPOM_CODE", "ES_DELETE=0 and CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlOrderNo);
                CommonClasses.FillCombo("WORK_ORDER_MASTER", "WO_NO", "WO_CODE", "ES_DELETE=0 and WO_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlOrderNo);
                ddlOrderNo.Items.Insert(0, new ListItem("Select Order", "0"));
                ddlOrderNo.Enabled = true;
                ddlItemName.SelectedIndex = 0;
            }
            else
            {
                ddlOrderNo.SelectedIndex = 0;
                DataTable dt = CommonClasses.FillCombo("ITEM_MASTER,BOM_MASTER", "(I_CODENO+' ('+(I_NAME)+')') as I_NAME", "I_CODE", "BM_I_CODE=I_CODE and BOM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE='-2147483646' ");
                ddlItemName.DataSource = dt;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));
                ddlOrderNo.Enabled = false;
                ddlItemName.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlType_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlOrderNo_SelectedIndexChanged
    protected void ddlOrderNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlOrderNo.SelectedIndex != -1 && ddlOrderNo.SelectedIndex != 0)
            {
                DataTable dt = CommonClasses.FillCombo("CUSTPO_DETAIL,ITEM_MASTER", "(I_CODENO+' ('+(I_NAME)+')') as I_NAME", "I_CODE", "I_CODE=CPOD_I_CODE and CPOD_CPOM_CODE='" + ddlOrderNo.SelectedValue + "' ");
                ddlItemName.DataSource = dt;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));
                ddlItemName.Enabled = true;
                ddlItemName.SelectedIndex = 1;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlOrderNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlRawComponentCode_SelectedIndexChanged
    protected void ddlRawComponentCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRawComponentCode.SelectedIndex != -1)
            {
                ddlRawComponentName.SelectedValue = ddlRawComponentCode.SelectedValue.ToString();
                loadItemBatchNo();
                ddlRawComponentName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlRawComponentCode_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlRawComponentName_SelectedIndexChanged
    protected void ddlRawComponentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRawComponentName.SelectedIndex != -1)
            {
                ddlRawComponentCode.SelectedValue = ddlRawComponentName.SelectedValue.ToString();
                loadItemBatchNo();
                ddlRawComponentCode.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlRawComponentName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlbatchNo_SelectedIndexChanged
    protected void ddlbatchNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlbatchNo.SelectedIndex != 0)
            {
                DataTable dtBatch = CommonClasses.Execute("select BT_SHM_CODE,isnull(BT_WO_CODE,0) as BT_WO_CODE,BT_WOD_I_CODE from BATCH_MASTER where BT_CODE='" + ddlbatchNo.SelectedValue + "'");
                if (dtBatch.Rows.Count > 0)
                {
                    ddlFormualCode.SelectedValue = dtBatch.Rows[0]["BT_SHM_CODE"].ToString();
                    if (Convert.ToInt32(dtBatch.Rows[0]["BT_WO_CODE"]) == 0)
                    {
                        ddlOrderNo.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlOrderNo.SelectedValue = dtBatch.Rows[0]["BT_WO_CODE"].ToString();
                    }                  
                    ddlItemName.SelectedValue = dtBatch.Rows[0]["BT_WOD_I_CODE"].ToString();
                    ddlItemName.Enabled = false;
                    ddlOrderNo.Enabled = false;
                    ddlFormualCode.Enabled = false;
                    ddlType.Enabled = false;
                    ddlType.SelectedIndex = 1;
                    getBatchDetail();
                    CalculateTotal();
                }
                else
                {
                    ddlItemName.SelectedIndex = 0;
                    ddlOrderNo.SelectedIndex = 0;
                    ddlFormualCode.SelectedIndex = 0;
                    ddlType.SelectedIndex = 0;
                    ddlItemName.Enabled = false;
                    ddlOrderNo.Enabled = false;
                    ddlFormualCode.Enabled = false;
                    ddlType.Enabled = false;

                    dtFilter.Clear();

                    if (dtFilter.Columns.Count == 0)
                    {
                        dtFilter.Columns.Add(new System.Data.DataColumn("STEP_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("MR_BALANCE_QTY", typeof(String)));
                        dtFilter.Columns.Add(new System.Data.DataColumn("ADD_IN_QTY", typeof(float)));
                        dtFilter.Rows.Add(dtFilter.NewRow());
                        dgvBOMaterialDetails.DataSource = dtFilter;
                        dgvBOMaterialDetails.DataBind();
                    }
                }
                //CommonClasses.FillCombo("SHADE_MASTER,BATCH_MASTER", "SHM_FORMULA_CODE", "SHM_CODE", "SHM_CODE=BT_SHM_CODE AND BATCH_MASTER.ES_DELETE=0 AND SHADE_MASTER.ES_DELETE=0 and SHM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and BT_CODE='" + ddlbatchNo.SelectedValue + "' ", ddlFormualCode);
                //ddlFormualCode.Items.Insert(0, new ListItem("Select Formula", "0"));
                //ddlFormualCode.Enabled = false;
                //ddlFormualCode.SelectedIndex = 1;
            }
            else
            {
                ddlItemName.SelectedIndex = 0;
                ddlOrderNo.SelectedIndex = 0;
                ddlFormualCode.SelectedIndex = 0;
                ddlType.SelectedIndex = 0;
                ddlItemName.Enabled = false;
                ddlOrderNo.Enabled = false;
                ddlFormualCode.Enabled = false;
                ddlType.Enabled = false;

                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("STEP_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BT_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("MR_BALANCE_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ADD_IN_QTY", typeof(float)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgvBOMaterialDetails.DataSource = dtFilter;
                    dgvBOMaterialDetails.DataBind();
                }
            }
            
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlbatchNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlFormualCode_SelectedIndexChanged
    protected void ddlFormualCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlType.SelectedValue =Convert.ToInt32(1).ToString();
            ddlType.Enabled = false;
           // CommonClasses.FillCombo("CUSTPO_MASTER", "CPOM_PONO", "CPOM_CODE", "ES_DELETE=0 and CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlOrderNo);
            CommonClasses.FillCombo("WORK_ORDER_MASTER", "WO_NO", "WO_CODE", "ES_DELETE=0 and WO_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyId"]) + "' ", ddlOrderNo);
            ddlOrderNo.Items.Insert(0, new ListItem("Select Order", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Requisition", "ddlFormualCode_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region getBatchDetail
    public void getBatchDetail()
    {
        DataTable dtBatch = new DataTable();
        DatabaseAccessLayer db = new DatabaseAccessLayer();
        if (ddlFormualCode.SelectedIndex != 0)
        {
            double OrderQty = 0;
           // string Count="0";
            DataTable dtOrderQty = CommonClasses.Execute("select WOD_I_CODE,isnull(WOD_WORK_ORDER_QTY,0) as WOD_WORK_ORDER_QTY from WORK_ORDER_MASTER,WORK_ORDER_DETAIL where WO_CODE=WOD_WO_CODE and WOD_I_CODE='" + ddlItemName.SelectedValue + "' and WO_CODE='" + ddlOrderNo.SelectedValue + "'");
            if (dtOrderQty.Rows.Count > 0)
            {
                OrderQty = Convert.ToDouble(dtOrderQty.Rows[0]["WOD_WORK_ORDER_QTY"]);
            }

            //dtBatch = CommonClasses.Execute("select SHM_PROCESS_CODE as PROCESS_CODE ,PROCESS_NAME,SHM_ITEM_CODE as BD_I_CODE,I_CODENO,I_NAME,sum(SHM_QTY_KG * '" + OrderQty + "') as BD_VQTY,cast(isnull(I_CURRENT_BAL,0) as numeric(10,3)) as I_CURRENT_BAL,cast(isnull(I_MIN_LEVEL,0) as numeric(10,3)) as I_MIN_LEVEL from SHADE_MASTER,SHADE_DETAIL,ITEM_MASTER,PROCESS_MASTER where SHD_SHM_CODE=SHM_CODE and SHM_ITEM_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and SHM_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and SHM_CODE='" + ddlFormualCode.SelectedValue + "' group by SHM_PROCESS_CODE,PROCESS_NAME,SHM_ITEM_CODE,I_CODENO,I_NAME,I_CURRENT_BAL,I_MIN_LEVEL order by PROCESS_NAME,I_CODENO");
            //Count = db.GetColumn("select Count(MR_BATCH_CODE) from MATERIAL_REQUISITION_MASTER where ES_DELETE=0 and MR_BATCH_CODE='" + ddlbatchNo.SelectedValue + "'");
            //if (Count == "0")
            //{
            dtBatch = CommonClasses.Execute("select BTD_PROCESS_CODE as PROCESS_CODE,BTD_STEP_NO as STEP_NO ,PROCESS_NAME,BTD_I_CODE as BD_I_CODE,I_CODENO,I_NAME,BT_NO,BT_CODE,sum(BTD_QTY) as BD_VQTY,sum(BTD_WGT) as WEIGHT_IN_KG,0.0  as ADD_IN_QTY,cast(isnull(I_CURRENT_BAL,0) as numeric(10,3)) as I_CURRENT_BAL,cast(isnull(I_MIN_LEVEL,0) as numeric(10,3)) as I_MIN_LEVEL,sum(BTD_QTY) as MR_BALANCE_QTY from BATCH_MASTER,BATCH_DETAIL,ITEM_MASTER,PROCESS_MASTER where BT_CODE=BTD_BT_CODE and BTD_I_CODE=I_CODE and ITEM_MASTER.ES_DELETE=0 and BTD_PROCESS_CODE=PROCESS_CODE and PROCESS_MASTER.ES_DELETE=0 and BT_CODE='" + ddlbatchNo.SelectedValue + "' group by BTD_PROCESS_CODE,BTD_STEP_NO,PROCESS_NAME,BTD_I_CODE,I_CODENO,I_NAME,I_CURRENT_BAL,I_MIN_LEVEL,BT_NO,BT_CODE order by BTD_STEP_NO");
            //}
            //else
            //{
            //    dtBatch = CommonClasses.Execute("SELECT BATCH_DETAIL.BTD_PROCESS_CODE AS PROCESS_CODE, dbo.BATCH_DETAIL.BTD_STEP_NO AS STEP_NO,dbo.PROCESS_MASTER.PROCESS_NAME, dbo.BATCH_DETAIL.BTD_I_CODE AS BD_I_CODE, dbo.ITEM_MASTER.I_CODENO, dbo.ITEM_MASTER.I_NAME,SUM(dbo.BATCH_DETAIL.BTD_QTY) AS BD_VQTY,0  as ADD_IN_QTY,ADD_IN_QTY as 0,SUM(dbo.BATCH_DETAIL.BTD_QTY)-MATERIAL_REQUISITION_DETAIL.MRD_ISSUE_QTY as MR_BALANCE_QTY, CAST(ISNULL(dbo.ITEM_MASTER.I_CURRENT_BAL, 0) AS numeric(10, 3)) AS I_CURRENT_BAL,CAST(ISNULL(dbo.ITEM_MASTER.I_MIN_LEVEL, 0) AS numeric(10, 3)) AS I_MIN_LEVEL FROM dbo.MATERIAL_REQUISITION_DETAIL INNER JOIN dbo.MATERIAL_REQUISITION_MASTER ON dbo.MATERIAL_REQUISITION_DETAIL.MRD_MR_CODE = dbo.MATERIAL_REQUISITION_MASTER.MR_CODE RIGHT OUTER JOIN dbo.BATCH_MASTER INNER JOIN dbo.BATCH_DETAIL ON dbo.BATCH_MASTER.BT_CODE = dbo.BATCH_DETAIL.BTD_BT_CODE INNER JOIN dbo.ITEM_MASTER ON dbo.BATCH_DETAIL.BTD_I_CODE = dbo.ITEM_MASTER.I_CODE INNER JOIN dbo.PROCESS_MASTER ON dbo.BATCH_DETAIL.BTD_PROCESS_CODE = dbo.PROCESS_MASTER.PROCESS_CODE ON dbo.MATERIAL_REQUISITION_DETAIL.MRD_I_CODE = dbo.BATCH_DETAIL.BTD_I_CODE AND dbo.MATERIAL_REQUISITION_MASTER.MR_BATCH_CODE = dbo.BATCH_MASTER.BT_CODE WHERE (dbo.ITEM_MASTER.ES_DELETE = 0) AND (dbo.PROCESS_MASTER.ES_DELETE = 0) AND (dbo.BATCH_MASTER.BT_CODE = '" + ddlbatchNo.SelectedValue + "') and dbo.MATERIAL_REQUISITION_MASTER.ES_DELETE=0 GROUP BY dbo.BATCH_DETAIL.BTD_PROCESS_CODE, dbo.BATCH_DETAIL.BTD_STEP_NO, dbo.PROCESS_MASTER.PROCESS_NAME, dbo.BATCH_DETAIL.BTD_I_CODE, dbo.ITEM_MASTER.I_CODENO, dbo.ITEM_MASTER.I_NAME,MATERIAL_REQUISITION_DETAIL.MRD_ISSUE_QTY, dbo.ITEM_MASTER.I_CURRENT_BAL, dbo.ITEM_MASTER.I_MIN_LEVEL ORDER BY STEP_NO");
            //}
            if (dtBatch.Rows.Count > 0)
            {
                dgvBOMaterialDetails.DataSource = dtBatch;
                dgvBOMaterialDetails.DataBind();
                dt2 = dtBatch;
                dgvBOMaterialDetails.Enabled = true;
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("STEP_NO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("PROCESS_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("WEIGHT_IN_KG", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("MR_BALANCE_QTY", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ADD_IN_QTY", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgvBOMaterialDetails.DataSource = dtFilter;
                    dgvBOMaterialDetails.DataBind();
                }
            }
        }
    }
    #endregion

    #region txtADD_IN_QTY_TextChanged
    protected void txtADD_IN_QTY_TextChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    TextBox thisCheckBox = (TextBox)sender;
        //    GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
        //    int index = thisGridViewRow.RowIndex;
        //    double PTS_QTY = 0;
        //    if (Convert.ToString(((TextBox)dgvBOMaterialDetails.Rows[index].FindControl("txtADD_IN_QTY")).Text) != null || Convert.ToString(((TextBox)dgvBOMaterialDetails.Rows[index].FindControl("txtADD_IN_QTY")).Text) != "")
        //    {
        //        PTS_QTY = Math.Round((Convert.ToDouble(((TextBox)dgvBOMaterialDetails.Rows[index].FindControl("txtADD_IN_QTY")).Text)), 3);
        //    }
        //    dt2.Rows[index]["ADD_IN_QTY"] = PTS_QTY.ToString();
        //    dt2.AcceptChanges();       
        //}
        //catch (Exception ex)
        //{
        //    CommonClasses.SendError("Issue To Production", "txtIssueQty_TextChanged", ex.Message.ToString());
        //}
    }
    #endregion

    #region CalculateTotal
    public void CalculateTotal()
    {
        try
        {
            if (dt2.Rows.Count > 0)
            {
                double addInTotal = 0, PTS_QTY=0;
                for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
                {
                    if (Convert.ToString(((TextBox)dgvBOMaterialDetails.Rows[i].FindControl("txtADD_IN_QTY")).Text) != null || Convert.ToString(((TextBox)dgvBOMaterialDetails.Rows[i].FindControl("txtADD_IN_QTY")).Text) != "")
                    {
                        PTS_QTY = Math.Round((Convert.ToDouble(((TextBox)dgvBOMaterialDetails.Rows[i].FindControl("txtADD_IN_QTY")).Text)), 3);
                        addInTotal = addInTotal + PTS_QTY;   
                    }
                }
                txtAddInTotal.Text = string.Format("{0:0.000}", addInTotal);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Requisition", "CalculateTotal", Ex.Message);
        }
    }
    #endregion

    #region btn_Calculate_Click
    protected void btn_Calculate_Click(object sender, EventArgs e)
    {
        try
        {
            CalculateTotal();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Material Requisition", "btn_Calculate_Click", Ex.Message);
        }
    }
    #endregion

}

