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


public partial class Transactions_ADD_PurchaseRequisition : System.Web.UI.Page
{

    #region Variable
    DataTable dtFilter = new DataTable();
   PurchaseRequisition_BL PurReq_BL = null;
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

                    //LoadBlankGrid();
                    if (Request.QueryString[0].Equals("VIEW"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        PurReq_BL = new PurchaseRequisition_BL(mlCode);
                        FillCombo();
                        ViewRec("VIEW");
                        //DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        PurReq_BL = new PurchaseRequisition_BL(mlCode);
                        FillCombo();
                        ViewRec("MOD");
                        //EnabaleTextBoxes(MainPanel);

                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        txtDate.Attributes.Add("readonly","readonly");
                        txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        FillCombo();

                        dt2.Rows.Clear();
                        str = "";
                        //EnabaleTextBoxes(MainPanel);
                        ddlMatReqNo.Enabled = false;
                        ddlItemName.Enabled = false;
                        dgvPurReqDet.Enabled = false;
                        LoadBlankGrid();
                    }
                    txtPerReqNo.Focus();
                    //LoadBlankGrid();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region LoadBlankGrid
    public void LoadBlankGrid()
    {
        DataTable dtFill = new DataTable();
        if (dgvPurReqDet.Rows.Count == 0)
        {

            dtFill.Clear();

            if (dtFill.Columns.Count == 0)
            {
                dtFill.Columns.Add(new System.Data.DataColumn("PRD_I_CODE", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("PRD_REQ_QTY", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("PRD_OLD_QTY", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("PRD_ORD_QTY", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("PRD_REMARK", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                dtFill.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
                dtFill.Rows.Add(dtFill.NewRow());
                dgvPurReqDet.DataSource = dtFill;
                dgvPurReqDet.DataBind();
                dgvPurReqDet.Enabled = false;
            }
        }
    }
    #endregion

    #region FillCombo
    private void FillCombo()
    {
        try
        {
            //CommonClasses.FillCombo("MATERIAL_REQUISITION_MASTER", "MR_BATCH_NO", "MR_CODE", "ES_DELETE=0 and MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' ", ddlMatReqNo);
            //ddlMatReqNo.Items.Insert(0, new ListItem("Select Batch No", "0"));
            DataTable dt = CommonClasses.Execute("select MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO from MATERIAL_REQUISITION_MASTER where ES_DELETE=0 ORDER BY MR_BATCH_NO DESC");
            ddlMatReqNo.DataSource = dt;
            ddlMatReqNo.DataTextField = "MR_BATCH_NO";
            ddlMatReqNo.DataValueField = "MR_CODE";
            ddlMatReqNo.DataBind();
            ddlMatReqNo.Items.Insert(0, new ListItem("Select Material Req.No", "0"));
            CommonClasses.FillCombo("ITEM_MASTER", "I_NAME", "I_CODE", "ES_DELETE=0 and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE='-2147483646' ", ddlItemName);
            ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));

            DataTable dtItem = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_NAME");

            ddlRawComponentCode.DataSource = dtItem;
            ddlRawComponentCode.DataTextField = "I_CODENO";
            ddlRawComponentCode.DataValueField = "I_CODE";
            ddlRawComponentCode.DataBind();
            ddlRawComponentCode.Items.Insert(0, new ListItem("Select Raw Material Code", "0"));


            ddlRawComponentName.DataSource = dtItem;
            ddlRawComponentName.DataTextField = "I_NAME";
            ddlRawComponentName.DataValueField = "I_CODE";
            ddlRawComponentName.DataBind();
            ddlRawComponentName.Items.Insert(0, new ListItem("Select Raw Material Name", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "FillCombo", Ex.Message);
        }

    }

    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        txtDate.Attributes.Add("readonly", "readonly");
        DataTable dt = new DataTable();
        try
        {
            //FillCombo();
            dt = CommonClasses.Execute("select PRM_CODE,PRM_TYPE,PRM_NO,convert(varchar,PRM_DATE,106) as PRM_DATE,PRM_MR_CODE,PRM_I_CODE,PRM_DEPARTMENT  from PRUCHASE_REQUISITION_MASTER WHERE PRM_CM_COMP_CODE = '" + Session["CompanyCode"].ToString() + "' and PRM_CODE='" + mlCode + "' and ES_DELETE=0 ");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["PRM_CODE"]);
                if (dt.Rows[0]["PRM_TYPE"].ToString() == "1")
                {
                    ddlType.SelectedIndex = 1;
                }
                else
                {
                    ddlType.SelectedIndex = 2;
                }

                ddlType_SelectedIndexChanged(null, null);
                txtPerReqNo.Text = dt.Rows[0]["PRM_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["PRM_DATE"]).ToString("dd MMM yyyy");
                ddlMatReqNo.SelectedValue = dt.Rows[0]["PRM_MR_CODE"].ToString();
                ddlMatReqNo_SelectedIndexChanged(null, null);
                txtDepartment.Text = dt.Rows[0]["PRM_DEPARTMENT"].ToString();
                ddlItemName.SelectedValue = dt.Rows[0]["PRM_I_CODE"].ToString();

                dtRequsitionDetail = CommonClasses.Execute("select distinct PRD_I_CODE,I_CODENO,I_NAME,cast(PRD_REQ_QTY as Numeric(10,3)) as PRD_REQ_QTY,PRD_ORD_QTY as PRD_OLD_QTY,cast(PRD_ORD_QTY AS Numeric(10,3)) as PRD_ORD_QTY,PRD_REMARK,I_CURRENT_BAL,I_MIN_LEVEL from PRUCHASE_REQUISITION_MASTER,PURCHASE_REQUISION_DETAIL,ITEM_MASTER where PRM_CODE=PRD_PRM_CODE and PRD_I_CODE=I_CODE and PRM_CODE='" + mlCode + "'");

                if (dtRequsitionDetail.Rows.Count != 0)
                {
                    dgvPurReqDet.DataSource = dtRequsitionDetail;
                    dgvPurReqDet.DataBind();
                    dt2 = dtRequsitionDetail;
                    dgvPurReqDet.Enabled = true;
                }


            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("PRUCHASE_REQUISITION_MASTER", "MODIFY", "PRM_CODE", mlCode);

            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                ddlType.Enabled = false;
                ddlItemName.Enabled = false;
                txtDate.Enabled = false;
                ddlMatReqNo.Enabled = false;
                txtDepartment.Enabled = false;
                ddlRawComponentCode.Enabled = false;
                ddlRawComponentName.Enabled = false;
                txtOrdQty.Enabled = false;
                txtReqQty.Enabled = false;
                txtRemark.Enabled = false;
                txtoldQty.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requsition", "ViewRec", Ex.Message);
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

                ddlType.SelectedIndex = PurReq_BL.PRM_TYPE;
                txtPerReqNo.Text = PurReq_BL.PRM_NO;
                txtDate.Text = Convert.ToDateTime(PurReq_BL.PRM_DATE).ToString("dd MMM yyyy");
                ddlMatReqNo.SelectedValue = PurReq_BL.PRM_MR_CODE.ToString();
                ddlItemName.SelectedValue = PurReq_BL.PRM_I_CODE.ToString();
                txtDepartment.Text = PurReq_BL.PRM_DEPARTMENT;

            }

            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Reqisition", "GetValues", ex.Message);
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
            PurReq_BL.PRM_TYPE = ddlType.SelectedIndex;
            PurReq_BL.PRM_NO = txtPerReqNo.Text;
            PurReq_BL.PRM_DATE = Convert.ToDateTime(txtDate.Text);
            if (ddlType.SelectedIndex != 1)
            {
                PurReq_BL.PRM_MR_CODE = 0;
            }
            else
            {
                PurReq_BL.PRM_MR_CODE = Convert.ToInt32(ddlMatReqNo.SelectedValue);
            }
            PurReq_BL.PRM_I_CODE = Convert.ToInt32(ddlItemName.SelectedValue);
            PurReq_BL.PRM_DEPARTMENT = txtDepartment.Text;
            PurReq_BL.PRM_UM_CODE = Convert.ToInt32(Session["UserCode"]);
            PurReq_BL.PRM_CM_COMP_CODE = Convert.ToInt32(Session["CompanyCode"]);
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Reqisition", "Setvalues", ex.Message);
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

                PurReq_BL = new PurchaseRequisition_BL();
                txtPerReqNo.Text = Numbering();
                if (Setvalues())
                {
                    if (PurReq_BL.Save(dgvPurReqDet, "INSERT"))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(PRM_CODE) from PRUCHASE_REQUISITION_MASTER");
                        CommonClasses.WriteLog("Purchase Requsition", "Save", "Purchase Requisition", PurReq_BL.PRM_NO.ToString(), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewPurchaseRequisition.aspx", false);
                    }
                    else
                    {
                        if (PurReq_BL.Msg != "")
                        {
                            //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                            PurReq_BL.Msg = "";
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Saved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        txtPerReqNo.Focus();
                    }
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                PurReq_BL = new PurchaseRequisition_BL(mlCode);

                if (Setvalues())
                {
                    if (PurReq_BL.Save(dgvPurReqDet, "UPDATE"))
                    {
                        CommonClasses.RemoveModifyLock("PRUCHASE_REQUISITION_MASTER", "MODIFY", "PRM_CODE", mlCode);
                        CommonClasses.WriteLog("Purchase Requsition", "Update", "Purchase Requisition", PurReq_BL.PRM_NO.ToString(), Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Transactions/VIEW/ViewPurchaseRequisition.aspx", false);
                    }
                    else
                    {
                        if (PurReq_BL.Msg != "")
                        {
                            //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                            PurReq_BL.Msg = "";
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Not Saved";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                        txtPerReqNo.Focus();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(cast((isnull(PRM_NO,0)) as numeric(10,0))) as PRM_NO from PRUCHASE_REQUISITION_MASTER  ");
        if (dt.Rows[0]["PRM_NO"] == null || dt.Rows[0]["PRM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["PRM_NO"]) + 1;
        }
        return GenGINNO.ToString();
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
            CommonClasses.SendError("Purchase Requsition ", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Purchase Requsition ", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlRawComponentCode.SelectedValue = "0";
            ddlRawComponentName.SelectedValue = "0";
            txtReqQty.Text = "";
            txtOrdQty.Text = "";
            txtoldQty.Text = "";
            txtRemark.Text = "";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region InserRecord

    private void InserRecord()
    {
        #region Validations

        if (ddlType.SelectedIndex == 1)
        {
            if (ddlMatReqNo.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Material Requision No", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            else
            {
                if (ddlItemName.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Item", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
            }
        }


        if (ddlRawComponentCode.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Raw Material Code", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

            ddlRawComponentCode.Focus();
            return;
        }
        if (ddlRawComponentName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Raw Material Name", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            ddlRawComponentName.Focus();
            return;
        }

        if (ddlRawComponentCode.SelectedIndex != 0)
        {
            if (txtReqQty.Text == "")
            {
                ShowMessage("#Avisos", "Please Enter Req. Qty", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (txtOrdQty.Text == "")
            {
                ShowMessage("#Avisos", "Select Enter Order Qty", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            if (txtRemark.Text == "")
            {
                ShowMessage("#Avisos", "Select Enter Remark", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
        }



        #endregion

        #region CheckExist
        if (dgvPurReqDet.Rows.Count > 0)
        {
            for (int i = 0; i < dgvPurReqDet.Rows.Count; i++)
            {
                string PRD_I_CODE = ((Label)(dgvPurReqDet.Rows[i].FindControl("lblPRD_I_CODE"))).Text;
                if (ItemUpdateIndex == "-1")
                {
                    if (PRD_I_CODE == ddlRawComponentCode.SelectedValue.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Warning);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }

                }
                else
                {
                    if (PRD_I_CODE == ddlRawComponentCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString())
                    {

                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Warning);
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
            dt2.Columns.Add(new System.Data.DataColumn("PRD_I_CODE", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("PRD_REQ_QTY", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("PRD_OLD_QTY", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("PRD_ORD_QTY", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("PRD_REMARK", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
            dt2.Columns.Add(new System.Data.DataColumn("I_MIN_LEVEL", typeof(String)));
        }
        #endregion

        #region Insert Record to Table
        dr = dt2.NewRow();
        dr["PRD_I_CODE"] = ddlRawComponentCode.SelectedValue.ToString();
        dr["I_CODENO"] = ddlRawComponentCode.SelectedItem.ToString();
        dr["I_NAME"] = ddlRawComponentName.SelectedItem.ToString();
        dr["PRD_REQ_QTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtReqQty.Text));
        dr["PRD_ORD_QTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtOrdQty.Text));
        dr["PRD_REMARK"] = txtRemark.Text;
        DataTable dtItem = CommonClasses.Execute("select * FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CODE='" + ddlRawComponentName.SelectedValue + "' ORDER BY I_NAME");

        dr["I_CURRENT_BAL"] = dtItem.Rows[0]["I_CURRENT_BAL"].ToString();
        dr["I_MIN_LEVEL"] = dtItem.Rows[0]["I_MIN_LEVEL"].ToString();
        #endregion

        #region InsertData
        if (str == "Modify")
        {
            if (dt2.Rows.Count > 0)
            {
                dr["PRD_OLD_QTY"] = txtoldQty.Text;
                dt2.Rows.RemoveAt(Index);
                dt2.Rows.InsertAt(dr, Index);
            }
        }
        else
        {
            dr["PRD_OLD_QTY"] = "0.000";
            dt2.Rows.Add(dr);
        }
        #endregion

        if (dt2.Rows.Count == 0)
        {
            dgvPurReqDet.Enabled = false;
        }
        else
        {
            
            dgvPurReqDet.DataSource = dt2;
            dgvPurReqDet.DataBind();
            dgvPurReqDet.Enabled = true;
        }
            clearDetail();
    }

    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (dgvPurReqDet.Enabled == true)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }

        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Purchase Requisition", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Purchase Requisition", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("PRUCHASE_REQUISITION_MASTER", "MODIFY", "PRM_CODE", mlCode);
            }
            Response.Redirect("~/Transactions/VIEW/ViewPurchaseRequisition.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Reqisition", "btnCancel_Click", Ex.Message);
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
            else if (ddlType.SelectedIndex == 1 && ddlMatReqNo.SelectedIndex == 0)
            {
                    flag = false;
            }
            else if (ddlItemName.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDepartment.Text == "")
            {
                flag = false;
            }
            else if (dgvPurReqDet.Enabled != true)
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
            CommonClasses.SendError("Purchase Requisition", "CheckValid", Ex.Message);
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

            CommonClasses.SendError("Purchase Requisition", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
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

            CommonClasses.SendError("Purchase Requisition", "btnInsert_Click", Ex.Message);
        }
    }


    #endregion
    
    #region GridEvents

    #region dgvPurReqDet_RowDeleting
    protected void dgvPurReqDet_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgvPurReqDet_SelectedIndexChanged
    protected void dgvPurReqDet_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region dgvPurReqDet_RowCommand
    protected void dgvPurReqDet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgvPurReqDet.Rows[Index];

            if (e.CommandName == "Delete")
            {


                int rowindex = row.RowIndex;
                dgvPurReqDet.DeleteRow(rowindex);
                dt2.Rows.RemoveAt(rowindex);
                dgvPurReqDet.DataSource = dt2;
                dgvPurReqDet.DataBind();

                for (int i = 0; i < dgvPurReqDet.Rows.Count; i++)
                {
                    string Code = ((Label)(row.FindControl("lblPRD_I_CODE"))).Text;
                    if (Code == "")
                    {
                        dgvPurReqDet.Enabled = false;
                    }
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                //FillCombo();
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlRawComponentCode.SelectedValue = ((Label)(row.FindControl("lblPRD_I_CODE"))).Text;
                ddlRawComponentCode_SelectedIndexChanged(null, null);
                txtReqQty.Text = ((Label)(row.FindControl("lblPRD_REQ_QTY"))).Text;
                txtReqQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtReqQty.Text));
                txtoldQty.Text = ((Label)(row.FindControl("lblPRD_OLD_QTY"))).Text;
                txtoldQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtoldQty.Text));
                txtOrdQty.Text = ((TextBox)(row.FindControl("txtPRD_ORD_QTY"))).Text;
                txtOrdQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtOrdQty.Text));
                txtRemark.Text = ((Label)(row.FindControl("lblPRD_REMARK"))).Text;
                if (ddlType.SelectedIndex == 1)
                {
                    ddlRawComponentCode.Enabled = false;
                    ddlRawComponentName.Enabled = false;
                    txtReqQty.Enabled = false;
                }
            
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Requisition", "dgvPurReqDet_RowCommand", Ex.Message);
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

    #region txtOrdQty_TextChanged
    protected void txtOrdQty_TextChanged(object sender, EventArgs e)
    {
        
        try
        {
            if (txtReqQty.Text=="")
            {
                txtReqQty.Text = "0.000";
            }
            string totalStr = DecimalMasking(txtOrdQty.Text);
            txtOrdQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

            double a = Convert.ToDouble(txtReqQty.Text);
            double b = Convert.ToDouble(txtOrdQty.Text);
            if (b > a)
            {
                
                PanelMsg.Visible = true;
                lblmsg.Text = "Order Qty Should Not Be Greater Than Requested Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrdQty.Text = "0.000";
                return;
            }
            else
            {
                txtOrdQty.Text = string.Format("{0:0.000}",Convert.ToDouble(txtOrdQty.Text) );
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlMatReqNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region txtReqQty_TextChanged
    protected void txtReqQty_TextChanged(object sender, EventArgs e)
    {
        
        try
        {
            string totalStr = DecimalMasking(txtReqQty.Text);
            txtReqQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            //txtReqQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtReqQty.Text));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlMatReqNo_SelectedIndexChanged", ex.Message);
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
                CommonClasses.FillCombo("MATERIAL_REQUISITION_MASTER,ITEM_MASTER", "I_NAME", "MR_I_CODE", "I_CODE=MR_I_CODE and MR_CODE='" + ddlMatReqNo.SelectedValue + "' ", ddlItemName);
                ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));
                ddlItemName.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlMatReqNo_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                DataTable dt = new DataTable();
                if (ddlType.SelectedIndex == 1)
                {
                    dt = CommonClasses.Execute("select MRD_I_CODE as PRD_I_CODE,I_CODENO,I_NAME,cast(case when sum(MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY) > 0 then sum(MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY) else 0 end as numeric(10,3)) as PRD_REQ_QTY,0.000 as PRD_OLD_QTY,cast(case when sum(MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY)-I_CURRENT_BAL > 0 then sum(MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY)-I_CURRENT_BAL else 0 end as numeric(10,3)) as PRD_ORD_QTY,'' as PRD_REMARK,I_MIN_LEVEL,I_CURRENT_BAL from MATERIAL_REQUISITION_DETAIL,MATERIAL_REQUISITION_MASTER,ITEM_MASTER where MR_CODE=MRD_MR_CODE and MATERIAL_REQUISITION_MASTER.ES_DELETE=0 AND I_CODE=MRD_I_CODE and MR_I_CODE='" + ddlItemName.SelectedValue + "' and (MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY) > 0 and MR_CODE='" + ddlMatReqNo.SelectedValue + "' group by MRD_I_CODE,I_CODENO,I_NAME,I_MIN_LEVEL,I_CURRENT_BAL");

                    ddlRawComponentCode.DataSource = dt;
                    ddlRawComponentCode.DataTextField = "I_CODENO";
                    ddlRawComponentCode.DataValueField = "PRD_I_CODE";
                    ddlRawComponentCode.DataBind();
                    ddlRawComponentCode.Items.Insert(0, new ListItem("Select Raw Material Code", "0"));

                    ddlRawComponentName.DataSource = dt;
                    ddlRawComponentName.DataTextField = "I_NAME";
                    ddlRawComponentName.DataValueField = "PRD_I_CODE";
                    ddlRawComponentName.DataBind();
                    ddlRawComponentName.Items.Insert(0, new ListItem("Select Raw Material Name", "0"));

                    if (dt.Rows.Count > 0)
                    {
                        dgvPurReqDet.DataSource = dt;
                        dgvPurReqDet.DataBind();
                        dt2 = dt;
                        dgvPurReqDet.Enabled = true;
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Found";

                        return;
                    }
                }
               
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlItemName_SelectedIndexChanged", ex.Message);
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
                if (!Request.QueryString[0].Equals("MODIFY") && !Request.QueryString[0].Equals("VIEW"))
                {


                    DataTable DT = CommonClasses.Execute("select distinct MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL WHERE MRD_MR_CODE=MR_CODE AND (MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY)>0 and MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND MR_TYPE='As Per Batch' AND ES_DELETE=0 order BY MR_BATCH_NO DESC");

                    //CommonClasses.FillCombo("MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL", "MR_BATCH_NO", "MR_CODE", "MRD_MR_CODE=MR_CODE AND (MRD_REQ_QTY-MRD_ISSUE_QTY-MRD_PURC_REQ_QTY)>0 and MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND MR_TYPE='As Per Order' AND ES_DELETE=0 GROUP BY MR_BATCH_NO", ddlMatReqNo);
                    //ddlMatReqNo.Items.Insert(0, new ListItem("Select Batch No", "0"));
                    //ddlMatReqNo.Enabled = true;
                    //ddlItemName.SelectedIndex = 0;

                    ddlMatReqNo.DataSource = DT;
                    ddlMatReqNo.DataTextField = "MR_BATCH_NO";
                    ddlMatReqNo.DataValueField = "MR_CODE";
                    ddlMatReqNo.DataBind();
                    ddlMatReqNo.Items.Insert(0, new ListItem("Select Material Req. No", "0"));
                    ddlMatReqNo.SelectedIndex = -1;
                    ddlMatReqNo.Enabled = true;
                }
                else
                {
                    DataTable DT = CommonClasses.Execute("select distinct MR_CODE,cast(MR_BATCH_NO as numeric(10,0)) as MR_BATCH_NO FROM MATERIAL_REQUISITION_MASTER,MATERIAL_REQUISITION_DETAIL WHERE MRD_MR_CODE=MR_CODE AND  MR_COMP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' AND MR_TYPE='As Per Order' AND ES_DELETE=0 order BY MR_BATCH_NO DESC ");

                    
                    ddlMatReqNo.DataSource = DT;
                    ddlMatReqNo.DataTextField = "MR_BATCH_NO";
                    ddlMatReqNo.DataValueField = "MR_CODE";
                    ddlMatReqNo.DataBind();
                    ddlMatReqNo.Items.Insert(0, new ListItem("Select Material Req. No", "0"));
                    ddlMatReqNo.SelectedIndex = -1;
                    ddlMatReqNo.Enabled = true;
                }
            }
            else
            {
                //ddlMatReqNo.SelectedIndex = 0;
                //CommonClasses.FillCombo("ITEM_MASTER", "I_NAME", "I_CODE", "ES_DELETE=0 and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CAT_CODE='-2147483646' ", ddlItemName);
                //ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));
                //ddlMatReqNo.Enabled = false;
                //ddlItemName.Enabled = true;

                DataTable DT = CommonClasses.Execute("select distinct(I_CODE),I_NAME FROM ITEM_MASTER WHERE ES_DELETE=0 and I_CAT_CODE='-2147483646' and I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_NAME");

                ddlItemName.DataSource = DT;
                ddlItemName.DataTextField = "I_NAME";
                ddlItemName.DataValueField = "I_CODE";
                ddlItemName.DataBind();
                ddlItemName.Items.Insert(0, new ListItem("Select Item", "0"));
                ddlItemName.SelectedIndex = -1;
                ddlMatReqNo.Enabled = false;
                ddlItemName.Enabled = true;

            }
            dgvPurReqDet.DataSource = null;
            dgvPurReqDet.DataBind();
            LoadBlankGrid();
            

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlType_SelectedIndexChanged", ex.Message);
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

                ddlRawComponentName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlRawComponentCode_SelectedIndexChanged", ex.Message);
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

                ddlRawComponentCode.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Purchase Requisition", "ddlRawComponentName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion



}
