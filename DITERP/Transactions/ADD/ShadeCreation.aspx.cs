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

public partial class Transactions_ADD_ShadeCreation : System.Web.UI.Page
{
    #region Variable

    DataTable dt = new DataTable();
    DataTable dtShadeCreation = new DataTable();
    static int mlCode = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();
    static string formulacode = "";
    DatabaseAccessLayer objdb = new DatabaseAccessLayer();
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
                        LoadFormulaType();
                        Loadlocation();
                        LoadColor();
                        LoadProcess();
                        LoadICode();
                        LoadIName();
                        LoadIUnit();
                        LoadItemBase();
                        LoadCustomer();
                        LoadProjectNo();
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
                            //txtFormulaCode.Attributes.Add("readonly", "readonly");
                            dgMainShade.Enabled = false;
                                
                        }
                        ddlFormulaType.Focus();
                        //dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Shade Creation", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            
            if (ddlFormulaType.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Formula Type", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select PO Type";
                ddlFormulaType.Focus();
                return;
            }
            if (ddllocation.SelectedIndex == 0 && ddlFormulaType.SelectedIndex != 1)
            {
                ShowMessage("#Avisos", "Select Location", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select PO Type";
                ddllocation.Focus();
                return;
            }


            if (ddlColorini.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Color", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //PanelMsg.Visible = true;
                //lblmsg.Text = "Select PO Type";
                ddlColorini.Focus();
                return;
            }
            if (ddlFormulaType.SelectedIndex != 0 && ddlFormulaType.SelectedIndex == 1 && ddlBaseItem.SelectedIndex ==0)
            {
                ShowMessage("#Avisos", "Select Base Item Name", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                               
                ddlProjectNo.Focus();
                return;
            }
           
                DataTable dtFormulaCode =new DataTable();
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    dtFormulaCode = CommonClasses.Execute("select * from SHADE_MASTER WHERE SHM_BASE_I_CODE='" + ddlBaseItem.SelectedValue + "' and  ES_DELETE=0");
                }
                else
                {
                    dtFormulaCode = CommonClasses.Execute("select * from SHADE_MASTER WHERE SHM_BASE_I_CODE='" + ddlBaseItem.SelectedValue + "' and SHM_CODE<>'" + mlCode + "' and ES_DELETE=0");
                }
                if (dtFormulaCode.Rows.Count > 0 && dtFormulaCode.Rows[0]["SHM_BASE_I_CODE"].ToString() != "0" && ddlFormulaType.SelectedIndex == 1)
                {
                    ShowMessage("#Avisos", "This Base Item Allready Used In Shade " + dtFormulaCode.Rows[0]["SHM_FORMULA_CODE"].ToString(), CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    ddlBaseItem.Focus();
                    return;
                }
           
            if (dgMainShade.Enabled && dgMainShade.Rows.Count > 0)
            {               
                SaveRec();             
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
            CommonClasses.SendError("Shade Creation", "btnSubmit_Click", Ex.Message);
        }

    }

    #endregion btnSubmit_Click

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {

            if (ddlFormulaType.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddllocation.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddlColorini.SelectedIndex == 0)
            {
                flag = false;
            }
            //else if (dgMainShade.Enabled)
            //{
            //    flag = true;
            //}
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Shade Creation", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("SHADE_MASTER", "MODIFY", "SHM_CODE", mlCode);
            }

            dt2.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewShadeCreation.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "CancelRecord", Ex.Message);
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
            CommonClasses.SendError("Shade Creation", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion
    
    #region LoadCombos
    
            #region LoadFormulaType
    private void LoadFormulaType()
                {
                    try
                    {
                        dt = CommonClasses.FillCombo("FORMAULA_TYPE_MASTER", "FORMULA_TYPE", "FORMULA_ID", "ES_DELETE=0 AND FORMULA_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlFormulaType);
                        ddlFormulaType.Items.Insert(0, new ListItem("Select Formula Type ", "0"));
                    }
                    catch (Exception Ex)
                    {
                        CommonClasses.SendError("Shade Creation", "LoadFormulaType", Ex.Message);
                    }
                }
            #endregion

            #region Loadlocation
            private void Loadlocation()
            {
                try
                {
                    dt = CommonClasses.FillCombo("LOCATION_MASTER", "LOC_NAME", "LOC_ID", "ES_DELETE=0 AND LOC_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'",ddllocation);
                    ddllocation.Items.Insert(0, new ListItem("Select Location", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "Loadlocation", Ex.Message);
                }
            }
            #endregion

            #region LoadColor
            private void LoadColor()
            {
                try
                {
                    dt = CommonClasses.FillCombo("COLOR_MASTER", "COLOR_NAME", "CLOLOR_ID", "ES_DELETE=0 AND COLOR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlColorini);
                    ddlColorini.Items.Insert(0, new ListItem("Select Color", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "LoadColor", Ex.Message);
                }
            }
            #endregion

            #region LoadProcess
            private void LoadProcess()
            {
                try
                {
                    dt = CommonClasses.FillCombo("PROCESS_MASTER", "PROCESS_NAME", "PROCESS_CODE", "ES_DELETE=0 AND PROCESS_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlProcess);
                    ddlProcess.Items.Insert(0, new ListItem("Select Process Type", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "Load Proccess", Ex.Message);
                }
            }
            #endregion

            #region LoadICode
            private void LoadICode()
            {
                try
                {
                    dt = CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE in ('-2147483648','-2147483647') ", ddlItemCode);
                    ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "Load Item Code", Ex.Message);
                }
            }
            #endregion

            #region LoadIName
            private void LoadIName()
            {
                try
                {
                    dt = CommonClasses.FillCombo("ITEM_MASTER", "I_NAME", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE in ('-2147483648','-2147483647') ", ddlItemName);
                    ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "Load Item Name", Ex.Message);
                }
            }
            #endregion

            #region LoadIUnit
            private void LoadIUnit()
            {
                try
                {
                    dt = CommonClasses.FillCombo("ITEM_UNIT_MASTER", "I_UOM_NAME", "I_UOM_CODE", "ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'", ddlUnit);
                    ddlUnit.Items.Insert(0, new ListItem("Select Unit", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "Unit", Ex.Message);
                }
            }
            #endregion

            #region LoadItemBase
            private void LoadItemBase()
            {
                try
                {
                    dt = CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE=-2147483647", ddlBaseItem);
                    ddlBaseItem.Items.Insert(0, new ListItem("Select Base Item", "0"));
                }
                catch (Exception Ex)
                {
                    CommonClasses.SendError("Shade Creation", "LoadItemBase", Ex.Message);
                }
            }
            #endregion
    
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            Loadlocation();
            LoadFormulaType();
            LoadICode();
            LoadIName();
            LoadIUnit();
            LoadColor();
            LoadCustomer();
            LoadProjectNo();
            dtShadeCreation.Clear();

            dt = CommonClasses.Execute("Select SHM_CODE,SHM_LOC_CODE,SHM_FORMULA_CODE,SHM_FORMULA_TYPE_CODE,SHM_GLOSS,SHM_COLOR_CODE,SHM_FORMULA_NAME,SHM_REMARK,MODIFY,isnull(SHM_TOTAL_IN_KG,0) as SHM_TOTAL_IN_KG,isnull(SHM_DENSITY,0) as SHM_DENSITY,SHM_BASE_I_CODE,isnull(SHM_VOL_SOLID,0) as SHM_VOL_SOLID,SHM_INQ_CODE,SHM_P_CODE from SHADE_MASTER where ES_DELETE=0 and  SHM_CODE=" + mlCode + "");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["SHM_CODE"]); ;
                ddllocation.SelectedValue = Convert.ToInt32(dt.Rows[0]["SHM_LOC_CODE"]).ToString();
                ddlFormulaType.SelectedValue = dt.Rows[0]["SHM_FORMULA_TYPE_CODE"].ToString();
                if (ddlFormulaType.SelectedIndex != 0 && ddlFormulaType.SelectedIndex == 1)
                {
                    ddllocation.Enabled = false;
                    txtGloss.Enabled = false;
                    ddllocation.SelectedIndex = -1;                   
                }
                else
                {
                    ddllocation.Enabled = true;
                    txtGloss.Enabled = true;
                }
                ddlColorini.SelectedValue = dt.Rows[0]["SHM_COLOR_CODE"].ToString();
                ddlBaseItem.SelectedValue = dt.Rows[0]["SHM_BASE_I_CODE"].ToString();
                txtGloss.Text = dt.Rows[0]["SHM_GLOSS"].ToString();
                txtFormulaName.Text = dt.Rows[0]["SHM_FORMULA_NAME"].ToString();
                txtFormulaCode.Text = dt.Rows[0]["SHM_FORMULA_CODE"].ToString();
                txtRemark.Text = dt.Rows[0]["SHM_REMARK"].ToString();
                txtTotalInKg.Text = dt.Rows[0]["SHM_TOTAL_IN_KG"].ToString();
                totInKg = Convert.ToDouble( dt.Rows[0]["SHM_TOTAL_IN_KG"]);
                txtAvgDensity.Text = dt.Rows[0]["SHM_DENSITY"].ToString();
                txtVolumeSolids.Text = dt.Rows[0]["SHM_VOL_SOLID"].ToString();
                ddlProjectNo.SelectedValue = dt.Rows[0]["SHM_INQ_CODE"].ToString();
                ddlProjectNo_SelectedIndexChanged(null,null);
                ddlCustomer.SelectedValue = dt.Rows[0]["SHM_P_CODE"].ToString();
                dtShadeCreation = CommonClasses.Execute("select SHM_PROCESS_CODE as Process_code,PROCESS_NAME as Process_name, SHM_ITEM_CODE as ItemCode,I_CODENO as ItemCodeNo,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME as Unit,cast(SHM_QTY_KG as numeric(10,3)) AS QtyinKG,cast(SHM_QTY_LTR as numeric(10,3)) AS QtyInLTR,cast(SHM_RATE as numeric(10,2)) as Rate,cast(isnull(SHADE_DETAIL.SHM_DENSITY,0) as numeric(10,2))  as Density,SHM_PROCESS_STEPS as ProcessSteps,cast((SHM_QTY_KG * SHM_RATE) as numeric(10,2)) as Amount From SHADE_DETAIL,SHADE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,PROCESS_MASTER where SHM_ITEM_CODE=I_CODE AND SHM_CODE=SHD_SHM_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND SHM_PROCESS_CODE=PROCESS_CODE AND SHADE_MASTER.ES_DELETE=0 and SHM_CODE='" + mlCode + "' order by SHM_PROCESS_STEPS");

                //dtShadeCreation = CommonClasses.Execute("select SHM_PROCESS_CODE as Process_code,PROCESS_NAME as Process_name, SHM_ITEM_CODE as ItemCode,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as I_UOM_CODE,I_UOM_NAME as Unit,cast(SHM_QTY_KG as numeric(10,3)) AS QtyinKG,cast(SHM_QTY_LTR as numeric(10,3)) AS QtyInLTR,cast(SHM_RATE as numeric(10,2)) as Rate,cast(SHM_DENSITY as numeric(10,2))  as Density,SHM_PROCESS_STEPS as ProcessSteps,cast((SHM_QTY_KG * SHM_RATE) as numeric(10,2)) as Amount From SHADE_DETAIL,SHADE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,PROCESS_MASTER where SHM_ITEM_CODE=I_CODE AND SHM_CODE=SHD_SHM_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND SHM_PROCESS_CODE=PROCESS_CODE AND SHADE_MASTER.ES_DELETE=0 and SHM_CODE='" + mlCode + "' order by SHM_ITEM_CODE");


                if (dtShadeCreation.Rows.Count != 0)
                {
                    dgMainShade.DataSource = dtShadeCreation;
                    dgMainShade.DataBind();
                    dgMainShade.Enabled = true;
                    dt2 = dtShadeCreation;
                    GetTotal();
                }
                else
                {
                    dtShadeCreation.Rows.Add(dtShadeCreation.NewRow());
                    dgMainShade.DataSource = dtShadeCreation;
                    dgMainShade.DataBind();
                    dgMainShade.Enabled = false;
                }

            }
            
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                dgMainShade.Enabled = false;
                
            }
            if (str == "MOD" || str == "AMEND")
            {
                
                CommonClasses.SetModifyLock("SHADE_MASTER", "MODIFY", "SHM_CODE", Convert.ToInt32(mlCode));

            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "ViewRec", Ex.Message);
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
                // if (!CommonClasses.CheckExistSave("CUSTPO_MASTER", "CPOM_WORK_ODR_NO", txtOrderNo.Text))
                DataTable dtFormulaCode = CommonClasses.Execute("select * from SHADE_MASTER WHERE SHM_FORMULA_CODE='" + txtFormulaCode.Text + "' and SHM_CM_CODE = " + Convert.ToInt32(Session["CompanyCode"]) + " and ES_DELETE=0");
                if (dtFormulaCode.Rows.Count == 0)
                {

                    if (CommonClasses.Execute1("INSERT INTO SHADE_MASTER (SHM_CM_CODE,SHM_LOC_CODE,SHM_FORMULA_TYPE_CODE,SHM_FORMULA_CODE,SHM_GLOSS,SHM_COLOR_CODE,SHM_FORMULA_NAME,SHM_REMARK,SHM_TOTAL_IN_KG,SHM_DENSITY,SHM_BASE_I_CODE,SHM_VOL_SOLID,SHM_INQ_CODE,SHM_P_CODE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddllocation.SelectedValue + "','" + ddlFormulaType.SelectedValue + "','" + txtFormulaCode.Text + "','" + txtGloss.Text + "','" + ddlColorini.SelectedValue + "','" + txtFormulaName.Text + "','" + txtRemark.Text + "','" + txtTotalInKg.Text + "','" + txtAvgDensity.Text + "','" + ddlBaseItem.SelectedValue + "','" + txtVolumeSolids.Text + "','"+ddlProjectNo.SelectedValue+"','"+ddlCustomer.SelectedValue+"')"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(SHM_CODE) from SHADE_MASTER");
                        for (int i = 0; i < dgMainShade.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO SHADE_DETAIL (SHD_SHM_CODE,SHM_PROCESS_CODE,SHM_ITEM_CODE,SHM_UNIT_CODE,SHM_QTY_KG,SHM_QTY_LTR,SHM_RATE,SHM_DENSITY,SHM_PROCESS_STEPS) values ('" + Code + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblProcessCode")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblUnitCode")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQtyInKg")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQtyInLTR")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblDensity")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblProSteps")).Text + "')");
                        }
                        if (ddlFormulaType.SelectedIndex == 1 && ddlBaseItem.SelectedIndex != 0)
                        {
                            CommonClasses.Execute1("update ITEM_MASTER set I_DENSITY='"+txtDensity.Text+"' where I_CODE='"+ddlBaseItem.SelectedValue+"'");
                        }
                        CommonClasses.WriteLog("Shade Creation", "Save", "Shade Creation", Convert.ToString(txtFormulaCode), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        dt2.Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewShadeCreation.aspx", false);

                    }
                    else
                    {

                        ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                        ddllocation.Focus();
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Formula Code Already Exists", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddllocation.Focus();

                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                 DataTable dtFormulaCode = CommonClasses.Execute("select * from SHADE_MASTER WHERE SHM_FORMULA_CODE='" + txtFormulaCode.Text + "' and SHM_CM_CODE = " + Convert.ToInt32(Session["CompanyCode"]) + " and ES_DELETE=0 and SHM_CODE !='"+mlCode+"'");
                 if (dtFormulaCode.Rows.Count == 0)
                 {
                     if (CommonClasses.Execute1("UPDATE SHADE_MASTER SET SHM_LOC_CODE='" + ddllocation.SelectedValue + "',SHM_FORMULA_TYPE_CODE='" + ddlFormulaType.SelectedValue + "',SHM_FORMULA_CODE='" + txtFormulaCode.Text + "',SHM_GLOSS='" + txtGloss.Text + "',SHM_COLOR_CODE='" + ddlColorini.SelectedValue + "',SHM_FORMULA_NAME='" + txtFormulaName.Text + "',SHM_REMARK='" + txtRemark.Text + "',SHM_TOTAL_IN_KG='" + txtTotalInKg.Text + "',SHM_DENSITY='" + txtAvgDensity.Text + "',SHM_BASE_I_CODE='" + ddlBaseItem.SelectedValue + "',SHM_VOL_SOLID='" + txtVolumeSolids.Text + "',SHM_INQ_CODE='" + ddlProjectNo.SelectedValue + "',SHM_P_CODE='"+ddlCustomer.SelectedValue+"' where SHM_CODE='" + mlCode + "'"))
                     {

                         result = CommonClasses.Execute1("DELETE FROM SHADE_DETAIL WHERE SHD_SHM_CODE='" + mlCode + "'");
                         if (result)
                         {

                             for (int i = 0; i < dgMainShade.Rows.Count; i++)
                             {
                                 CommonClasses.Execute1("INSERT INTO SHADE_DETAIL (SHD_SHM_CODE,SHM_PROCESS_CODE,SHM_ITEM_CODE,SHM_UNIT_CODE,SHM_QTY_KG,SHM_QTY_LTR,SHM_RATE,SHM_DENSITY,SHM_PROCESS_STEPS) values ('" + mlCode + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblProcessCode")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblUnitCode")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQtyInKg")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblQtyInLTR")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblRate")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblDensity")).Text + "','" + ((Label)dgMainShade.Rows[i].FindControl("lblProSteps")).Text + "')");
                             }
                             if (ddlFormulaType.SelectedIndex == 1 && ddlBaseItem.SelectedIndex != 0)
                             {
                                 CommonClasses.Execute1("update ITEM_MASTER set I_DENSITY='" + txtAvgDensity.Text + "' where I_CODE='" + ddlBaseItem.SelectedValue + "'");
                             }


                             CommonClasses.RemoveModifyLock("SHADE_MASTER", "MODIFY", "SHM_CODE", mlCode);
                             CommonClasses.WriteLog("Shade Creation", "Update", "Shade Creation", txtFormulaCode.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                             dt2.Rows.Clear();
                             result = true;
                         }
                         Response.Redirect("~/Transactions/VIEW/ViewShadeCreation.aspx", false);
                     }
                     else
                     {
                         ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                         ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                         ddllocation.Focus();
                     }
                 }
                 else
                 {
                     ShowMessage("#Avisos", "Formula Code Already Exists", CommonClasses.MSG_Warning);
                     ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                     ddllocation.Focus();

                 }

            }
            
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
           // Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");
            if (txtProcessStep.Text=="0")
            {
                ShowMessage("#Avisos", "Process Steps Is Required", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtProcessStep.Focus();
                return;
            }


            if (dgMainShade.Enabled)
            {
                for (int i = 0; i < dgMainShade.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgMainShade.Rows[i].FindControl("lblItemCode"))).Text;
                    string PROCESSS_CODE = ((Label)(dgMainShade.Rows[i].FindControl("lblProcessCode"))).Text;
                    string steps = ((Label)(dgMainShade.Rows[i].FindControl("lblProSteps"))).Text;
                    if (ItemUpdateIndex == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && PROCESSS_CODE==ddlProcess.SelectedValue.ToString())
                        {
                            ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            ddlItemCode.Focus();
                            return;
                        }
                        if (steps == txtProcessStep.Text)
                        {
                            ShowMessage("#Avisos", "Steps No Already Exist", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtProcessStep.Focus();
                            return;
                        }

                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString() && PROCESSS_CODE == ddlProcess.SelectedValue.ToString())
                        {
                            ShowMessage("#Avisos", "Record Already Exist", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            ddlItemCode.Focus();
                            return;
                        }
                        if (steps == txtProcessStep.Text && ItemUpdateIndex != i.ToString())
                        {
                            ShowMessage("#Avisos", "Steps No Already Exist", CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtProcessStep.Focus();
                            return;
                        }
                    }

                }
            }

            #region datatable structure
            if (dt2.Columns.Count == 0)
            {
                dt2.Columns.Add("Process_code");
                dt2.Columns.Add("Process_name");
                dt2.Columns.Add("ItemCode");
                dt2.Columns.Add("ItemCodeNo");
                dt2.Columns.Add("ItemName");
                dt2.Columns.Add("I_UOM_CODE");
                dt2.Columns.Add("Unit");
                dt2.Columns.Add("QtyinKG");
                dt2.Columns.Add("QtyInLTR");
                dt2.Columns.Add("Rate");
                dt2.Columns.Add("Amount");
                dt2.Columns.Add("Density");
                dt2.Columns.Add("ProcessSteps");


            }
            #endregion

            #region add control value to Dt

            dr = dt2.NewRow();
            dr["Process_code"] = ddlProcess.SelectedValue;
            dr["Process_name"] = ddlProcess.SelectedItem;
            dr["ItemCode"] = ddlItemName.SelectedValue;
            dr["ItemCodeNo"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["I_UOM_CODE"] = ddlUnit.SelectedValue;
            dr["Unit"] = ddlUnit.SelectedItem;
            dr["QtyinKG"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtQtyInKG.Text)), 3));
            dr["QtyInLTR"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtQtyinltr.Text)), 3));
            dr["Rate"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));
            dr["Amount"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtQtyInKG.Text)*Convert.ToDouble(txtRate.Text)), 2));
            dr["Density"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtDensity.Text)), 2)); 
            dr["ProcessSteps"] = txtProcessStep.Text;

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

            DataView dv = dt2.DefaultView;
            dv.Sort = "ProcessSteps";
            dt2 = dv.ToTable();

            #region Binding data to Grid
            dgMainShade.Visible = true;
            dgMainShade.DataSource = dt2;
            dgMainShade.DataBind();
            dgMainShade.Enabled = true;

            DataRow[] dr1 = dt2.Select("ProcessSteps= MAX(ProcessSteps)");

            if (dr1 != null)
            {
                // Console.WriteLine(dr[0]["RowNum"]);
                int maxVal = Convert.ToInt32(dr1[0]["ProcessSteps"])+10;
                txtProcessStep.Text = maxVal.ToString();
            }
            #endregion

            GetTotal();
            clearDetail();
            ddlItemCode.Focus();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "btnInsert_Click", Ex.Message);
        }

    }
    #endregion

    #region GetTotal
    private void GetTotal()     
    {
        double TotalInKg = 0,TotalInLtr=0,AvgDensity=0,VolumeSolids=0,TotalSolid=0;
        try
        {
            if (dgMainShade.Rows.Count > 0)
            {
                for (int i = 0; i < dgMainShade.Rows.Count; i++)
                {
                    //TotalInKg = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text);
                    //TotalInLtr = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInLTR"))).Text);
                    TotalInKg = TotalInKg + Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text);
                    TotalInLtr = TotalInLtr + Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInLTR"))).Text);
                    string ItemSold = objdb.GetColumn("select isnull(I_SOLIDS,0) from ITEM_MASTER where I_CODE='" + ((Label)(dgMainShade.Rows[i].FindControl("lblItemCode"))).Text + "'");
                    TotalSolid = TotalSolid +( Convert.ToDouble(ItemSold) * Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text));
                }
            }
            txtTotalInKg.Text = string.Format("{0:0.00}", Math.Round(TotalInKg,3));
            totInKg = Math.Round(TotalInKg, 3);
            AvgDensity = TotalInKg / TotalInLtr;
            txtAvgDensity.Text = string.Format("{0:0.00}", Math.Round(AvgDensity,2));
            txtToatlLtr.Text = string.Format("{0:0.00}", Math.Round(TotalInLtr, 3));
            for (int j = 0; j < dgMainShade.Rows.Count; j++)
            {
                string ItemSold = objdb.GetColumn("select isnull(I_SOLIDS,0) from ITEM_MASTER where I_CODE='" + ((Label)(dgMainShade.Rows[j].FindControl("lblItemCode"))).Text + "'");              
            }
            double Itemdensity = (AvgDensity / 0.89);
            VolumeSolids = (100-((Itemdensity) * (100 - Convert.ToDouble(TotalSolid))));
            txtVolumeSolids.Text = VolumeSolids.ToString();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "GetTotal", Ex.Message);
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
                    CommonClasses.SendError("Shade Creation", "ShowMessage", Ex.Message);
                    return false;
                }
            }
            #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            ddlUnit.SelectedIndex = 0;
            txtQtyinltr.Text = "0.000";
            txtRate.Text = "0.00";
            txtQtyInKG.Text = "0.00";
            txtProcessStep.Text = "";
            txtDensity.Text = "";
            txtProcessStep.Text = "";

            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Shade Creation", "clearDetail", Ex.Message);
        }
    }
    #endregion  
    
    #region ddlFormulaType_SelectedIndexChanged
    protected void ddlFormulaType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFormulaType.SelectedIndex != 0 && ddlFormulaType.SelectedIndex == 1)
            {
                ddllocation.Enabled = false;
                txtGloss.Enabled = false;
                ddllocation.SelectedIndex = -1;
                txtGloss.Text = "0";
            }
            else
            {
                ddllocation.Enabled = true;
                txtGloss.Enabled = true;
            }
            FormulaCodeCreation();
        }
        catch (Exception Ex)
        {
             CommonClasses.SendError(" Shade Creation", "ddlFormulaType_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddllocation_SelectedIndexChanged
    protected void ddllocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FormulaCodeCreation();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Shade Creation", "ddllocation_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlColorini_SelectedIndexChanged
    protected void ddlColorini_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FormulaCodeCreation();
            if (ddlFormulaType.SelectedIndex == 1)
            {
                string ItemCode = objdb.GetColumn("select I_CODE from ITEM_MASTER where I_CODENO='" + txtFormulaCode.Text + "' and I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' ");
                if (ItemCode != "")
                {
                    if (txtFormulaCode.Text == ItemCode)
                    {
                        ddlBaseItem.SelectedValue = ItemCode;
                    }
                    else
                    {
                        ddlBaseItem.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
             CommonClasses.SendError(" Shade Creation", "ddlColorini_SelectedIndexChanged", Ex.Message);
        }
    }

    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,cast(isnull(I_DENSITY,0) as numeric(10,2)) as I_DENSITY,cast(isnull(I_INV_RATE,0) as numeric(10,2)) as  I_INV_RATE,I_CAT_CODE  from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");


                if (dt1.Rows.Count > 0)
                {
                    //txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtDensity.Text = dt1.Rows[0]["I_DENSITY"].ToString();
                    //if (dt1.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647")
                    //{
                    //    string rate = objdb.GetColumn("select sum(cast(isnull(I_INV_RATE,0) as numeric(10,2))) as I_INV_RATE from BOM_MASTER,BOM_DETAIL,ITEM_MASTER where BOM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID='" + Session["CompanyId"] + "' and BM_I_CODE='" + ddlItemName.SelectedValue + "' and BM_CODE=BD_BM_CODE and BD_I_CODE=I_CODE");
                    //    if (rate == "")
                    //    {
                    //        txtRate.Text = "0.00";
                    //    }
                    //    else
                    //    {
                    //        txtRate.Text = string.Format("{0:0.00}", rate).ToString();
                    //    }
                    //}
                    //else
                    //{
                        txtRate.Text = string.Format("{0:0.00}",dt1.Rows[0]["I_INV_RATE"]).ToString();
                    //}

                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                txtQtyInKG.Focus();


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "ddlItemCode_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion
    
    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,cast(isnull(I_DENSITY,0) as numeric(10,2)) as I_DENSITY,cast(isnull(I_INV_RATE,0) as numeric(10,2)) as  I_INV_RATE,I_CAT_CODE from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtitemcode = CommonClasses.Execute("select distinct(I_CODE),I_CODENO,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_NAME=" + ddlItemName.Text + "");

                if (dt1.Rows.Count > 0)
                {
                    //txtUnit.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtDensity.Text = dt1.Rows[0]["I_DENSITY"].ToString();
                    //if (dt1.Rows[0]["I_CAT_CODE"].ToString() == "-2147483647")
                    //{
                    //    string rate = objdb.GetColumn("select sum(cast(isnull(I_INV_RATE,0) as numeric(10,2))) as I_INV_RATE from BOM_MASTER,BOM_DETAIL,ITEM_MASTER where BOM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID='" + Session["CompanyId"] + "' and BM_I_CODE='" + ddlItemCode.SelectedValue + "' and BM_CODE=BD_BM_CODE and BD_I_CODE=I_CODE");
                    //    if (rate == "")
                    //    {
                    //        txtRate.Text = "0.00";
                    //    }
                    //    else
                    //    {
                    //        txtRate.Text = string.Format("{0:0.00}", rate).ToString();
                    //    }
                    //}
                    //else
                    //{
                        txtRate.Text = string.Format("{0:0.00}",dt1.Rows[0]["I_INV_RATE"]).ToString();
                    //}
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                txtQtyInKG.Focus();

            }
            
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Shade Creation", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlProjectNo_SelectedIndexChanged
    protected void ddlProjectNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProjectNo.SelectedIndex != 0)
            {
                LoadCustomer();
            }
            else
            {
                ddlCustomer.SelectedIndex = 0;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "ddlProjectNo_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region dgMainShade_Deleting
    protected void dgMainShade_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    //#region txtRate_TextChanged
    //protected void txtRate_TextChanged(object sender, EventArgs e)
    //{

    //}
    //#endregion

    #region txtGloss_TextChanged
    protected void txtGloss_TextChanged(object sender, EventArgs e)
    {
        try
        {           
                
            
            if (txtGloss.Text == "")
            {
                txtGloss.Text = "0";
            }
            if (Convert.ToDouble(txtGloss.Text) >= 0 && Convert.ToDouble(txtGloss.Text) <= 100)
            {
                FormulaCodeCreation();
            }
            else
            {
                if (ddlFormulaType.SelectedIndex != 0 && ddlFormulaType.SelectedIndex == 1)
                {
                    FormulaCodeCreation();
                    ShowMessage("#Avisos", "Gloss Should Be Greater Than 0 Or Less Than 100", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtGloss.Text = "0";
                    txtGloss.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "LoadFilter", ex.Message.ToString());
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
                double TotalInKg = 0, TotalOneKg = 0, Density = 0, Rate=0;
                if (dgMainShade.Rows.Count > 0)
                {
                    for (int i = 0; i < dgMainShade.Rows.Count; i++)
                    {
                        Rate = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblRate"))).Text);
                        Density = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblDensity"))).Text);
                        TotalInKg = Convert.ToDouble(((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text);
                        
                        TotalOneKg = TotalInKg / totInKg;
                        ((Label)(dgMainShade.Rows[i].FindControl("lblQtyInKg"))).Text = string.Format("{0:0.000}", Math.Round((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)),3));
                        ((Label)(dgMainShade.Rows[i].FindControl("lblQtyInLTR"))).Text = string.Format("{0:0.000}", Math.Round(((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)) / Density), 3));
                        ((Label)(dgMainShade.Rows[i].FindControl("lblAmount"))).Text = string.Format("{0:0.000}", Math.Round(((TotalOneKg * Convert.ToDouble(txtTotalInKg.Text)) * Rate), 2));
                    }
                }
                totInKg = Convert.ToDouble(txtTotalInKg.Text);
               
            }
            
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "txtTotalInKG_TextChanged", ex.Message.ToString());
        }
    }
    #endregion

    #region dgMainShade_RowCommand
    protected void dgMainShade_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgMainShade.Rows[Index];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                if (mlCode != 0 && mlCode != null)
                {
                    string itemCode = ((Label)(row.FindControl("lblItemCode"))).Text;
                    //if (CommonClasses.CheckUsedInTran("INVOICE_DETAIL", "IND_I_CODE", " and IND_CPOM_CODE='" + mlCode + "' ", itemCode))
                    //{
                    //    //PanelMsg.Visible = true;
                    //    //lblmsg.Text = "Record not deleted, it is used in Invoice";

                    //    ShowMessage("#Avisos", "You can't delete this record, it is used in Invoice", CommonClasses.MSG_Warning);
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //    return;
                    //}
                }
                dgMainShade.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);

                DataView dv = dt2.DefaultView;
                dv.Sort = "ProcessSteps";
                dt2 = dv.ToTable();

                dgMainShade.DataSource = dt2;
                dgMainShade.DataBind();
                if (dgMainShade.Rows.Count == 0)
                {
                    dgMainShade.Enabled = false;
                    LoadFilter();
                }
                else
                {
                    
                }
                GetTotal();
            }
            if (e.CommandName == "Select")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlProcess.SelectedValue = ((Label)(row.FindControl("lblProcessCode"))).Text;
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode.SelectedItem.Text = ((Label)(row.FindControl("lblItemCodeNo"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblUnitCode"))).Text;
                txtQtyInKG.Text = ((Label)(row.FindControl("lblQtyInKg"))).Text;
                txtQtyinltr.Text = ((Label)(row.FindControl("lblQtyInLTR"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblRate"))).Text;
                txtDensity.Text = ((Label)(row.FindControl("lblDensity"))).Text;
                txtProcessStep.Text = ((Label)(row.FindControl("lblProSteps"))).Text;
                
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "dgMainShade_RowCommand", Ex.Message);
        }
    }
    #endregion
    
    #region LoadFilter
    public void LoadFilter()
    {
        try
        {
            if (dgMainShade.Rows.Count == 0)
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("Process_code", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Process_name", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ItemCodeNo", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("QtyinKG", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("QtyInLTR", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Amount", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("Density", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("ProcessSteps", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgMainShade.DataSource = dtFilter;
                    dgMainShade.DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "LoadFilter", ex.Message.ToString());
        }
    }
    #endregion
    
    #region txtQtyInKG_TextChanged
    protected void txtQtyInKG_TextChanged(object sender, EventArgs e)
    {
        double QtyLtr = (Convert.ToDouble(txtQtyInKG.Text) / Convert.ToDouble(txtDensity.Text));
        txtQtyinltr.Text = string.Format("{0:0.00}", Math.Round(QtyLtr, 3));
    }
    #endregion

    #region FormulaCodeCreation
    public void FormulaCodeCreation()
    {
        string A = "";
        string B = "";
        string C = "";
        string D = "";
        string E = "";
        string formulacode = "";
        try
        {
            if (ddllocation.SelectedIndex != 0)
            {
                char[] ch = ddllocation.SelectedItem.ToString().ToCharArray();
                if (ddlFormulaType.SelectedIndex == 1)
                {
                    A = "";
                }
                else
                {
                    A = ch[0].ToString();
                }
            }
            if (ddlFormulaType.SelectedIndex != 0)
            {
                if (ddlFormulaType.SelectedIndex == 1)
                {
                    B = "BP";
                }
                else
                {
                    if (Convert.ToInt32((ddlFormulaType.SelectedIndex) - 1) == 7)
                    {
                        B = "9";
                    }
                    else
                    {
                        B = (Convert.ToInt32(ddlFormulaType.SelectedIndex) - 1).ToString();
                    }
                   
                }
                
            }
            if (txtGloss.Text == "" || txtGloss.Text == "0")
            {
                C = "";
            }
            else if (Convert.ToDouble(txtGloss.Text) < 20)
            {
                C = "L";
            }
            else if (Convert.ToDouble(txtGloss.Text) >= 20 && Convert.ToDouble(txtGloss.Text) < 60)
            {
                C = "M";
            }
            else if (Convert.ToDouble(txtGloss.Text) >= 60 && Convert.ToDouble(txtGloss.Text) <= 100)
            {
                C = "H";
            }
         
            if (ddlColorini.SelectedIndex != 0)
            {
                char[] ch = ddlColorini.SelectedItem.ToString().ToCharArray();
                //D = ch[0].ToString();
                string colorAbb = objdb.GetColumn("select COLOR_ABBRIVATION from COLOR_MASTER where CLOLOR_ID='"+ddlColorini.SelectedValue+"'");
                D = colorAbb.ToString().Trim();

                int col_count = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(count(SHM_COLOR_CODE),0) as SHM_COLOR_CODE FROM SHADE_MASTER WHERE SHM_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and SHM_COLOR_CODE='" + ddlColorini.SelectedValue + "' and SHM_FORMULA_TYPE_CODE='"+ddlFormulaType.SelectedValue+"'");
                if (dt.Rows.Count > 0)
                {
                    col_count = Convert.ToInt32(dt.Rows[0]["SHM_COLOR_CODE"]);
                    col_count = col_count + 1;
                }
                E = col_count.ToString();
             
            }

            if (ddlFormulaType.SelectedIndex == 1)
            {
                formulacode = A + B + "-"+D+E;
            }
            else
            {
                formulacode = A + B + "-" + C + D + "-" + E;
            }
          
            txtFormulaCode.Text = formulacode.ToString();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Shade Creation", "FormulaCodeCreation", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            if (ddlProjectNo.SelectedIndex != 0)
            {
                DataTable dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME,INQ_SHADE_NAME from PARTY_MASTER,ENQUIRY_MASTER where P_CODE=INQ_CUST_NAME and INQ_CODE='" + ddlProjectNo.SelectedValue + "' and PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1 order by P_NAME");
                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "P_NAME";
                ddlCustomer.DataValueField = "P_CODE";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
                ddlCustomer.SelectedIndex = 1;
                if (dt.Rows.Count > 0)
                {
                    txtShadeName.Text = Convert.ToString(dt.Rows[0]["INQ_SHADE_NAME"]);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadProjectNo
    private void LoadProjectNo()
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select INQ_CODE,CONVERT(varchar(10),isnull(P_ABBREVATION,''))+''+CONVERT(varchar(10), INQ_NO) as INQ_NO from ENQUIRY_MASTER,PARTY_MASTER where P_CODE=INQ_CUST_NAME and INQ_CM_COMP_ID='" + Session["CompanyId"] + "' order by INQ_NO asc");
            ddlProjectNo.DataSource = dt;
            ddlProjectNo.DataTextField = "INQ_NO";
            ddlProjectNo.DataValueField = "INQ_CODE";
            ddlProjectNo.DataBind();
            ddlProjectNo.Items.Insert(0, new ListItem("Select Project No", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Shade Creation", "LoadProjectNo", Ex.Message);
        }

    }
    #endregion

}
