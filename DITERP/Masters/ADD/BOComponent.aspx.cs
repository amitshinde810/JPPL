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

public partial class Masters_ADD_BOComponent : System.Web.UI.Page
{
    #region Declartion
    static int mlCode = 0;
    static DataTable BindTable = new DataTable();
    DataRow dr;
    static DataTable dtBOCDetail = new DataTable();
    static string DetailMod = "-1";
    static string ItemUpdateIndex = "-1";
    static DataTable TemTaable = new DataTable();
    static DataTable dtInfo = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    static string right = "";
    public static string str = "";
    #endregion

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {

            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='12'");
                right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    #region Blank Grid
                    //DataTable BindTable = new DataTable();

                    BindTable.Clear();

                    if (BindTable.Columns.Count == 0)
                    {
                        BindTable.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
                        BindTable.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                        BindTable.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                        BindTable.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
                        BindTable.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
                    }
                    BindTable.Rows.Add(BindTable.NewRow());

                    dgvBOComponentDetails.DataSource = BindTable;
                    dgvBOComponentDetails.DataBind();
                    #endregion

                    LoadCombos();

                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        //BL_DepartmentMaster = new DepartmentMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        //BL_DepartmentMaster = new DepartmentMaster_BL();
                        mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    ddlFItemCodeno.Focus();


                    DetailMod = "-1";
                    ViewState["DetailMod"] = DetailMod;

                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Bill of Component", "PageLoad", ex.Message);
                }
            }
            else
            {
                DetailMod = (string)ViewState["DetailMod"];
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
            if (dgvBOComponentDetails.Rows.Count != 0)
            {
                SaveRec();
            }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Bill Of Component", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
            InserRecord();
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Bill Of Component", "btnInsert_Click", Ex.Message);
        }
    }

    private void InserRecord()
    {
        #region Validations
        if (ddlFItemCodeno.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Finished Component Code", CommonClasses.MSG_Warning);

            ddlFItemCodeno.Focus();
            return;
        }
        if (ddlFItemName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Finished Component Name", CommonClasses.MSG_Warning);

            ddlFItemName.Focus();
            return;
        }

        if (ddlSubComponentCode.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Sub Component Code", CommonClasses.MSG_Warning);

            ddlSubComponentCode.Focus();
            return;
        }
        if (ddlSubComponentName.SelectedIndex == 0)
        {
            ShowMessage("#Avisos", "Select Sub Component Name", CommonClasses.MSG_Warning);

            ddlSubComponentName.Focus();
            return;
        }
        if (txtVQty.Text=="")
        {
            ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);

            ddlSubComponentName.Focus();
            return;
        }
        #endregion

        #region CheckExist
        if (dgvBOComponentDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgvBOComponentDetails.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgvBOComponentDetails.Rows[i].FindControl("lblBD_I_CODE"))).Text;
                if (ItemUpdateIndex == "-1")
                {
                    if (bd_i_code == ddlSubComponentCode.SelectedValue.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        return;
                    }

                }
                else
                {
                    if (bd_i_code == ddlSubComponentCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        return;
                    }

                }

            }
        }
        #endregion

        #region Datatable Initialization
        //BindTable.Clear();

        if (BindTable.Rows.Count > 0)
        {
            for (int i = BindTable.Rows.Count - 1; i >= 0; i--)
            {
                if (BindTable.Rows[i][1] == DBNull.Value)
                    BindTable.Rows[i].Delete();
            }
            BindTable.AcceptChanges();
        }

        if (BindTable.Columns.Count == 0)
        {
            BindTable.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));
        }
        #endregion

        #region Insert Record to Table
        dr = BindTable.NewRow();
        dr["BD_I_CODE"] = ddlSubComponentCode.SelectedValue.ToString();
        dr["I_CODENO"] = ddlSubComponentCode.SelectedItem.ToString();
        dr["I_NAME"] = ddlSubComponentName.SelectedItem.ToString();
        dr["UOM"] = txtUOM.Text;
        dr["BD_VQTY"] = txtVQty.Text;
        #endregion

        #region InsertData
        if (str == "Modify")
        {
            if (BindTable.Rows.Count > 0)
            {
                BindTable.Rows.RemoveAt(Index);
                BindTable.Rows.InsertAt(dr, Index);
            }
        }
        else
        {
            BindTable.Rows.Add(dr);
        }
        #endregion

        #region Bind aTable
        //if (ViewState["DetailMod"].ToString() != "-1")
        //{
        //    if (BindTable.Rows.Count > 0)
        //    {
        //        DataRow[] drr = BindTable.Select("BD_I_CODE='" + ddlSubComponentCode.SelectedValue.ToString());
        //        for (int i = 0; i < drr.Length; i++)
        //            drr[i].Delete();
        //        BindTable.AcceptChanges();
        //    }
        //}
        //for (int i = BindTable.Rows.Count - 1; i >= 0; i--)
        //{
        //    if (BindTable.Rows[i][1] == DBNull.Value)
        //        BindTable.Rows[i].Delete();
        //}
        //BindTable.AcceptChanges(); 
        #endregion

        dgvBOComponentDetails.DataSource = BindTable;
        dgvBOComponentDetails.DataBind();

        clearDetail();
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("BOC_MASTER", "MODIFY", "BM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/VeiwBOComponent.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill of Component", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region ddlFItemCodeno_SelectedIndexChanged
    protected void ddlFItemCodeno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFItemCodeno.SelectedIndex != -1)
            {
                ddlFItemName.SelectedValue = ddlFItemCodeno.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlFItemName.SelectedValue + "' and ITEM_MASTER.ES_DELETE=0");
                   
                
                if (dt.Rows.Count > 0)
                {
                    txtFinUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlFItemName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Components", "ddlFItemCodeno_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlFItemName_SelectedIndexChanged
    protected void ddlFItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFItemName.SelectedIndex != -1)
            {
                ddlFItemCodeno.SelectedValue = ddlFItemName.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT I_UOM_CODE,I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlFItemName.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    txtFinUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlFItemCodeno.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Components", "ddlFItemName_SelectedIndexChanged", ex.Message);
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
                if (dt.Rows.Count > 0)
                {
                    txtUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlSubComponentName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Components", "ddlSubComponentCode_SelectedIndexChanged", ex.Message);
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
                    txtUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlSubComponentCode.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Components", "ddlSubComponentName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion


    #region GridEvents

    #region dgvBOComponentDetails_RowDeleting
    protected void dgvBOComponentDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    protected void dgvBOComponentDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void dgvBOComponentDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgvBOComponentDetails.Rows[Index];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgvBOComponentDetails.DeleteRow(rowindex);
                BindTable.Rows.RemoveAt(rowindex);
                dgvBOComponentDetails.DataSource = BindTable;
                dgvBOComponentDetails.DataBind();
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlSubComponentCode.SelectedValue = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                ddlSubComponentCode_SelectedIndexChanged(null, null);
                txtVQty.Text = ((Label)(row.FindControl("lblBD_VQTY"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component", "dgMainPO_RowCommand", Ex.Message);
        }

    }

    #endregion

    #endregion

    #region Methods

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlSubComponentCode.SelectedValue = "0";
            ddlSubComponentName.SelectedValue = "0";
            txtUOM.Text = "";
            txtVQty.Text = "0.00";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Component", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        try
        {
            //BL_EmployeeMaster = new EmployeeMaster_BL();
            dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE='-2147483647' ORDER BY I_NAME");

            ddlFItemCodeno.DataSource = dt;
            ddlFItemCodeno.DataTextField = "I_CODENO";
            ddlFItemCodeno.DataValueField = "I_CODE";
            ddlFItemCodeno.DataBind();
            ddlFItemCodeno.Items.Insert(0, new ListItem("Select Finished Component Code", "0"));

            ddlFItemName.DataSource = dt;
            ddlFItemName.DataTextField = "I_NAME";
            ddlFItemName.DataValueField = "I_CODE";
            ddlFItemName.DataBind();
            ddlFItemName.Items.Insert(0, new ListItem("Select Finished Component Name", "0"));


            dt = CommonClasses.Execute("select BM_CODE,I_CODENO FROM BOC_MASTER,ITEM_MASTER WHERE BM_I_CODE=I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and BM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and BOC_MASTER.ES_DELETE=0 ORDER BY BM_CODE");
            ddlMaterialFrom.DataSource = dt;
            ddlMaterialFrom.DataTextField = "I_CODENO";
            ddlMaterialFrom.DataValueField = "BM_CODE";
            ddlMaterialFrom.DataBind();
            ddlMaterialFrom.Items.Insert(0, new ListItem("Select Component Code", "0"));


            dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 AND I_CAT_CODE<>'-2147483647' ORDER BY I_NAME");

            ddlSubComponentCode.DataSource = dt;
            ddlSubComponentCode.DataTextField = "I_CODENO";
            ddlSubComponentCode.DataValueField = "I_CODE";
            ddlSubComponentCode.DataBind();
            ddlSubComponentCode.Items.Insert(0, new ListItem("Select Component Code", "0"));


            ddlSubComponentName.DataSource = dt;
            ddlSubComponentName.DataTextField = "I_NAME";
            ddlSubComponentName.DataValueField = "I_CODE";
            ddlSubComponentName.DataBind();
            ddlSubComponentName.Items.Insert(0, new ListItem("Select Component Name", "0"));

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Component", "LoadCombos", ex.Message);
        }
        finally
        {
            //dt.Dispose();
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT BM_CODE,BM_I_CODE FROM BOC_MASTER WHERE BM_CODE=" + mlCode + " AND BM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["BM_CODE"]); ;
                ddlFItemCodeno.SelectedValue = dt.Rows[0]["BM_I_CODE"].ToString();
                ddlFItemCodeno_SelectedIndexChanged(null,null);
                ddlFItemName.SelectedValue = dt.Rows[0]["BM_I_CODE"].ToString();

                GetDetails(mlCode);

                if (str == "VIEW")
                {
                    ddlFItemCodeno.Enabled = false;
                    ddlFItemName.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("BOC_MASTER", "MODIFY", "BM_CODE", mlCode);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill of Component", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetDetails
    private void GetDetails(int mlCode)
    {
        DataTable dtDetails = new DataTable();

        dtDetails = CommonClasses.Execute("SELECT BD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME AS UOM,BD_VQTY FROM BOC_MASTER,BOC_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE BD_BM_CODE=" + mlCode + " AND BD_BM_CODE=BM_CODE AND BD_I_CODE=I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE");

        if (dtDetails != null && dtDetails.Rows.Count > 0)
        {
            for (int j = 0; j < dtDetails.Rows.Count; j++)
            {
                BindTable.Rows.Add();
                BindTable.Rows[j]["BD_I_CODE"] = dtDetails.Rows[j]["BD_I_CODE"].ToString();
                BindTable.Rows[j]["I_CODENO"] = dtDetails.Rows[j]["I_CODENO"].ToString();
                BindTable.Rows[j]["I_NAME"] = dtDetails.Rows[j]["I_NAME"].ToString();
                BindTable.Rows[j]["UOM"] = dtDetails.Rows[j]["UOM"].ToString();
                BindTable.Rows[j]["BD_VQTY"] = dtDetails.Rows[j]["BD_VQTY"].ToString();
            }
        }

        dgvBOComponentDetails.DataSource = dtDetails;
        dgvBOComponentDetails.DataBind();

        if (Request.QueryString[0].Equals("VIEW"))
        {
            dgvBOComponentDetails.Enabled = false;
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

                if (CommonClasses.Execute1("INSERT INTO BOC_MASTER(BM_CM_COMP_ID,BM_I_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlFItemCodeno.SelectedValue.ToString() + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(BM_CODE) from BOC_MASTER");
                    for (int i = 0; i < dgvBOComponentDetails.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO BOC_DETAIL(BD_BM_CODE,BD_I_CODE,BD_VQTY)VALUES('" + Code + "','" + ((Label)dgvBOComponentDetails.Rows[i].FindControl("lblBD_I_CODE")).Text + "','" + ((Label)dgvBOComponentDetails.Rows[i].FindControl("lblBD_VQTY")).Text + "')");
                    }
                    CommonClasses.WriteLog("Bill Of Component Master", "Save", "Bill Of Component Master", ddlFItemCodeno.SelectedItem.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Masters/VIEW/VeiwBOComponent.aspx", false);
                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);

                    ddlFItemCodeno.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE BOC_MASTER SET BM_I_CODE='" + ddlFItemCodeno.SelectedValue.ToString() + "' WHERE BM_CODE='"+ mlCode +"'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM BOC_DETAIL WHERE BD_BM_CODE='" + mlCode + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgvBOComponentDetails.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO BOC_DETAIL(BD_BM_CODE,BD_I_CODE,BD_VQTY)VALUES('" + mlCode + "','" + ((Label)dgvBOComponentDetails.Rows[i].FindControl("lblBD_I_CODE")).Text + "','" + ((Label)dgvBOComponentDetails.Rows[i].FindControl("lblBD_VQTY")).Text + "')");
                        }

                        CommonClasses.RemoveModifyLock("BOC_MASTER", "MODIFY", "BM_CODE", mlCode);
                        CommonClasses.WriteLog("Bill Of Component Master", "Update", "Bill Of Component Master", ddlFItemCodeno.SelectedValue.ToString(), mlCode, Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/VeiwBOComponent.aspx", false);
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);

                    //txtCustomerID.Focus();
                }

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Of Component", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Bill Of Component", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #endregion

    protected void ddlMaterialFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlMaterialFrom.SelectedIndex != 0)
            {
                GetDetails(Convert.ToInt32(ddlMaterialFrom.SelectedValue));
            }
        }
        catch (Exception Ex)
        {
        }
    }
}