#region Class library
///<summary>
///Class library
///</summary>
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
#endregion

public partial class Masters_ADD_BOMaterial : System.Web.UI.Page
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

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Masters1MV");
        home1.Attributes["class"] = "active";

        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {

            if (!IsPostBack)
            {
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='44'");
                right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    LoadCombos();
                    BlankGrid();
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
                    ddlSItemCodeno.Focus();
                    DetailMod = "-1";
                    ViewState["DetailMod"] = DetailMod;
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Bill of Material", "PageLoad", ex.Message);
                }
            }
            else
            {
                DetailMod = (string)ViewState["DetailMod"];
            }
        }
    }

    #endregion

    #region Blank Grid
    public void BlankGrid()
    {
        BindTable.Clear();
        if (BindTable.Columns.Count == 0)
        {
            BindTable.Columns.Add(new System.Data.DataColumn("BD_I_CODE", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
            BindTable.Columns.Add(new System.Data.DataColumn("BD_VQTY", typeof(String)));//BD_SCRAPQTY
            BindTable.Columns.Add(new System.Data.DataColumn("BD_SCRAPQTY", typeof(String)));//
        }

        dgvBOMaterialDetails.Enabled = false;
        BindTable.Rows.Add(BindTable.NewRow());

        dgvBOMaterialDetails.DataSource = BindTable;
        dgvBOMaterialDetails.DataBind();
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(0, 1)), this, "For Save"))
            {
                DataTable dd = new DataTable();
                if (!Request.QueryString[0].Equals("MODIFY"))
                {
                    dd = CommonClasses.Execute("select * from BOM_MASTER where BM_I_CODE='" + ddlSItemCodeno.SelectedValue.ToString() + "' AND ES_DELETE=0 ");
                    if (dd.Rows.Count > 0)
                    {
                        ShowMessage("#Avisos", "Bill of Material Already Generated", CommonClasses.MSG_Info);
                        return;
                    }
                }
                if (dgvBOMaterialDetails.Enabled)
                {

                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (dgvBOMaterialDetails.Rows.Count != 0)
                {
                    SaveRec();
                }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Bill Of Material", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Bill Of Material", "btnInsert_Click", Ex.Message);
        }
    }

    private void InserRecord()
    {
        if (!Request.QueryString[0].Equals("MODIFY"))
        {
            DataTable dd = new DataTable();
            dd = CommonClasses.Execute("select * from BOM_MASTER where BM_I_CODE='" + ddlSItemCodeno.SelectedValue.ToString() + "' AND ES_DELETE=0 ");
            if (dd.Rows.Count > 0)
            {
                ShowMessage("#Avisos", "Bill of Material Already Generated", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }

        #region Validations
        //if (ddlSItemCodeno.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Finished Product Code", CommonClasses.MSG_Warning);

        //    ddlSItemCodeno.Focus();
        //    return;
        //}
        //if (ddlSItemName.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Finished Product Name", CommonClasses.MSG_Warning);

        //    ddlSItemName.Focus();
        //    return;
        //}

        //if (ddlRawComponentCode.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Raw Material Code", CommonClasses.MSG_Warning);

        //    ddlRawComponentCode.Focus();
        //    return;
        //}
        //if (ddlRawComponentName.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Raw Material Name", CommonClasses.MSG_Warning);

        //    ddlRawComponentName.Focus();
        //    return;
        //}
        //if (txtVQty.Text=="")
        //{
        //    ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);

        //    ddlRawComponentName.Focus();
        //    return;
        //}
        #endregion

        #region CheckExist
        if (dgvBOMaterialDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE"))).Text;
                if (ItemUpdateIndex == "-1")
                {
                    if (bd_i_code == ddlRawComponentCode.SelectedValue.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        return;
                    }
                }
                else
                {
                    if (bd_i_code == ddlRawComponentCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

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
            dgvBOMaterialDetails.Enabled = true;
            for (int i = BindTable.Rows.Count - 1; i >= 0; i--)
            {
                if (BindTable.Rows[i][1] == DBNull.Value)
                    BindTable.Rows[i].Delete();
            }
            BindTable.AcceptChanges();
        }
        else
        {
            dgvBOMaterialDetails.Enabled = false;
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
        dr["BD_I_CODE"] = ddlRawComponentCode.SelectedValue.ToString();
        dr["I_CODENO"] = ddlRawComponentCode.SelectedItem.ToString();
        dr["I_NAME"] = ddlRawComponentName.SelectedItem.ToString();
        dr["UOM"] = txtUOM.Text;
        dr["BD_VQTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtVQty.Text));
        dr["BD_SCRAPQTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtScraQty.Text));

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


        dgvBOMaterialDetails.DataSource = BindTable;
        dgvBOMaterialDetails.DataBind();

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
            CommonClasses.SendError("Bill Of Material", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlSItemCodeno.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (ddlSItemName.SelectedIndex == 0)
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
            CommonClasses.SendError("Bill Of Material", "CheckValid", Ex.Message);
        }

        return flag;
    }
    #endregion

    private void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("BOM_MASTER", "MODIFY", "BM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill of Material", "btnCancel_Click", Ex.Message);
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }

    #region ddlSItemCodeno_SelectedIndexChanged
    protected void ddlSItemCodeno_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSItemCodeno.SelectedIndex != -1)
            {
                ddlSItemName.SelectedValue = ddlSItemCodeno.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlSItemName.SelectedValue + "' and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    txtSinUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlSItemName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Material", "ddlSItemCodeno_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlSItemName_SelectedIndexChanged
    protected void ddlSItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSItemName.SelectedIndex != -1)
            {
                ddlSItemCodeno.SelectedValue = ddlSItemName.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlSItemName.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    txtSinUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlSItemCodeno.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Material", "ddlSItemName_SelectedIndexChanged", ex.Message);
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
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlRawComponentCode.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    txtUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlRawComponentName.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Material", "ddlRawComponentCode_SelectedIndexChanged", ex.Message);
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
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlRawComponentCode.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    txtUOM.Text = dt.Rows[0]["I_UOM_NAME"].ToString();
                }
                ddlRawComponentCode.Focus();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Material", "ddlSubComponentName_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region GridEvents

    #region dgvBOMaterialDetails_RowDeleting
    protected void dgvBOMaterialDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    protected void dgvBOMaterialDetails_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
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
                BindTable.Rows.RemoveAt(rowindex);
                dgvBOMaterialDetails.DataSource = BindTable;
                dgvBOMaterialDetails.DataBind();
                if (dgvBOMaterialDetails.Rows.Count == 0)
                {
                    BlankGrid();
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlRawComponentCode.SelectedValue = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                ddlRawComponentCode_SelectedIndexChanged(null, null);
                txtVQty.Text = ((Label)(row.FindControl("lblBD_VQTY"))).Text;
                txtScraQty.Text = ((Label)(row.FindControl("lblBD_SCRAPQTY"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material", "dgvBOMaterialDetails_RowCommand", Ex.Message);
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
            txtUOM.Text = "";
            txtVQty.Text = "0.00";
            txtScraQty.Text = "0.00";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Of Material", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region LoadCombos
    /// <summary>  
    ///  This class performs an important function.  
    /// </summary>

    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        try
        {
            dt = CommonClasses.Execute("select distinct( I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CODE NOT IN ( SELECT BM_I_CODE from BOM_MASTER where BOM_MASTER.ES_DELETE=0) ORDER BY I_CODENO");

            ddlSItemCodeno.DataSource = dt;
            //ddlSItemCodeno.DataTextField = "I_CODENO";
            //ddlSItemCodeno.DataValueField = "I_CODE";
            ddlSItemCodeno.DataBind();
            ddlSItemCodeno.Items.Insert(0, new ListItem("Select Finished Product Code", "0"));

            dt1 = CommonClasses.Execute(" select distinct( I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CODE NOT IN ( SELECT BM_I_CODE from BOM_MASTER where BOM_MASTER.ES_DELETE=0) ORDER BY I_NAME");

            ddlSItemName.DataSource = dt1;
            ddlSItemName.DataTextField = "I_NAME";
            ddlSItemName.DataValueField = "I_CODE";
            ddlSItemName.DataBind();
            ddlSItemName.Items.Insert(0, new ListItem("Select Finished Product Name", "0"));

            dt = CommonClasses.Execute("select BM_CODE,I_CODENO FROM BOM_MASTER,ITEM_MASTER WHERE BM_I_CODE=I_CODE and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and BM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and BOM_MASTER.ES_DELETE=0 ORDER BY BM_CODE");
            ddlMaterialFrom.DataSource = dt;
            ddlMaterialFrom.DataTextField = "I_CODENO";
            ddlMaterialFrom.DataValueField = "BM_CODE";
            ddlMaterialFrom.DataBind();
            ddlMaterialFrom.Items.Insert(0, new ListItem("Select Finished Product Code", "0"));

            dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_CODENO");
            ddlRawComponentCode.DataSource = dt;
            ddlRawComponentCode.DataTextField = "I_CODENO";
            ddlRawComponentCode.DataValueField = "I_CODE";
            ddlRawComponentCode.DataBind();
            ddlRawComponentCode.Items.Insert(0, new ListItem("Select Raw Material Code", "0"));

            dt = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ES_DELETE=0 ORDER BY I_NAME");
            ddlRawComponentName.DataSource = dt;
            ddlRawComponentName.DataTextField = "I_NAME";
            ddlRawComponentName.DataValueField = "I_CODE";
            ddlRawComponentName.DataBind();
            ddlRawComponentName.Items.Insert(0, new ListItem("Select Raw Material Name", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill of Material", "LoadCombos", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt2 = new DataTable();
            DataTable dt1 = new DataTable();
            dt2 = CommonClasses.Execute(" select distinct( I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0  ORDER BY I_CODENO"); //AND I_CAT_CODE= '-2147483648'
            ddlSItemCodeno.DataSource = dt2;
            ddlSItemCodeno.DataTextField = "I_CODENO";
            ddlSItemCodeno.DataValueField = "I_CODE";
            ddlSItemCodeno.DataBind();
            ddlSItemCodeno.Items.Insert(0, new ListItem("Select Finished Product Code", "0"));

            dt1 = CommonClasses.Execute(" select distinct( I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 ORDER BY I_NAME"); //AND I_CAT_CODE= '-2147483648'

            ddlSItemName.DataSource = dt1;
            ddlSItemName.DataTextField = "I_NAME";
            ddlSItemName.DataValueField = "I_CODE";
            ddlSItemName.DataBind();
            ddlSItemName.Items.Insert(0, new ListItem("Select Finished Product Name", "0"));

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT BM_CODE,BM_I_CODE FROM BOM_MASTER WHERE BM_CODE=" + mlCode + " AND BM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                //ddlSItemCodeno_SelectedIndexChanged(null, null);
                mlCode = Convert.ToInt32(dt.Rows[0]["BM_CODE"]); ;
                ddlSItemCodeno.SelectedValue = dt.Rows[0]["BM_I_CODE"].ToString();
              
                ddlSItemName.SelectedValue = dt.Rows[0]["BM_I_CODE"].ToString();
                ddlSItemCodeno_SelectedIndexChanged(null, null);
                GetDetails(mlCode);
                ddlSItemCodeno.Enabled = false;
                ddlSItemName.Enabled = false;
                if (str == "VIEW")
                {
                    ddlSItemCodeno.Enabled = false;
                    ddlSItemName.Enabled = false;
                    btnSubmit.Visible = false;
                    ddlMaterialFrom.Enabled = false;
                    dgvBOMaterialDetails.Enabled = false;
                    btnInsert.Enabled = false;
                }
                else if (str == "MOD")
                {
                    CommonClasses.SetModifyLock("BOM_MASTER", "MODIFY", "BM_CODE", mlCode);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill of Material", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetDetails
    private void GetDetails(int mlCode)
    {
        DataTable dtDetails = new DataTable();

        dtDetails = CommonClasses.Execute("SELECT BD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME AS UOM,cast(BD_VQTY as Numeric(10,3)) as BD_VQTY  ,cast(BD_SCRAPQTY as Numeric(10,3)) as    BD_SCRAPQTY  FROM BOM_MASTER,BOM_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE BD_BM_CODE=" + mlCode + " AND BD_BM_CODE=BM_CODE AND BD_I_CODE=I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE");

        if (dtDetails != null && dtDetails.Rows.Count > 0)
        {
            BindTable.Rows.Clear();
            for (int j = 0; j < dtDetails.Rows.Count; j++)
            {
                dgvBOMaterialDetails.Enabled = true;

                BindTable.Rows.Add();
                BindTable.Rows[j]["BD_I_CODE"] = dtDetails.Rows[j]["BD_I_CODE"].ToString();
                BindTable.Rows[j]["I_CODENO"] = dtDetails.Rows[j]["I_CODENO"].ToString();
                BindTable.Rows[j]["I_NAME"] = dtDetails.Rows[j]["I_NAME"].ToString();
                BindTable.Rows[j]["UOM"] = dtDetails.Rows[j]["UOM"].ToString();
                BindTable.Rows[j]["BD_VQTY"] = dtDetails.Rows[j]["BD_VQTY"].ToString();
                BindTable.Rows[j]["BD_SCRAPQTY"] = dtDetails.Rows[j]["BD_SCRAPQTY"].ToString();
            }
        }
        dgvBOMaterialDetails.DataSource = dtDetails;
        dgvBOMaterialDetails.DataBind();

        if (Request.QueryString[0].Equals("VIEW"))
        {
            dgvBOMaterialDetails.Enabled = false;
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
            CommonClasses.SendError("Bill Of Material", "ShowMessage", Ex.Message);
            return false;
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
                if (CommonClasses.Execute1("INSERT INTO BOM_MASTER(BM_CM_COMP_ID,BM_I_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlSItemCodeno.SelectedValue.ToString() + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(BM_CODE) from BOM_MASTER");
                    for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO BOM_DETAIL(BD_BM_CODE,BD_I_CODE,BD_VQTY,BD_SCRAPQTY)VALUES('" + Code + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_VQTY")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_SCRAPQTY")).Text + "')");
                    }
                    CommonClasses.WriteLog("Bill Of Material", "Save", "Bill Of Material", ddlSItemCodeno.SelectedItem.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlSItemCodeno.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE BOM_MASTER SET BM_I_CODE='" + ddlSItemCodeno.SelectedValue.ToString() + "' WHERE BM_CODE='" + mlCode + "'"))
                {
                    result = CommonClasses.Execute1("DELETE FROM BOM_DETAIL WHERE BD_BM_CODE='" + mlCode + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("INSERT INTO BOM_DETAIL(BD_BM_CODE,BD_I_CODE,BD_VQTY,BD_SCRAPQTY)VALUES('" + mlCode + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_VQTY")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_SCRAPQTY")).Text + "')");
                        }
                        CommonClasses.RemoveModifyLock("BOM_MASTER", "MODIFY", "BM_CODE", mlCode);
                        CommonClasses.WriteLog("Bill Of Material", "Update", "Bill Of Material", ddlSItemCodeno.SelectedValue.ToString(), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewBOMMaster.aspx", false);
                    }
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Of Material", "SaveRec", ex.Message);
        }
        return result;
    }
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

    protected void txtVQty_TextChanged(object sender, EventArgs e)
    {
        if (txtVQty.Text != "")
        {
            string totalStr = DecimalMasking(txtVQty.Text);
            txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        }
    }

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
}