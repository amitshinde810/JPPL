#region Class library
///<summary>
///Class library
///</summary>
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
#endregion

public partial class Masters_ADD_ItemTrasferMaster : System.Web.UI.Page
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
    static int RowCount = 0;
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
                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='160'");
                right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    ViewState["RowCount"] = "";
                    ViewState["RowCount"] = RowCount;
                    ViewState["dt2"] = dt2;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    str = "";
                    ViewState["str"] = str;
                    LoadCombos();
                    LoadDetaiItem();
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
                        ViewState["RowCount"] = "1";
                    }
                    ddlSItemCodeno.Focus();
                    DetailMod = "-1";
                    ViewState["DetailMod"] = DetailMod;
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Item Trasfer Master", "PageLoad", ex.Message);
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
        ((DataTable)ViewState["dt2"]).Clear();
        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("TD_I_CODE", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("TD_VQTY", typeof(String)));//TD_SCRAPQTY
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("TD_SCRAPQTY", typeof(String)));//
        }

        dgvBOMaterialDetails.Enabled = false;
        ((DataTable)ViewState["dt2"]).Rows.Add(((DataTable)ViewState["dt2"]).NewRow());

        dgvBOMaterialDetails.DataSource = ((DataTable)ViewState["dt2"]);
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
                    dd = CommonClasses.Execute("select * from TRANSFER_MASTER where TM_I_CODE='" + ddlSItemCodeno.SelectedValue.ToString() + "' AND ES_DELETE=0 ");
                    if (dd.Rows.Count > 0)
                    {
                        ShowMessage("#Avisos", "Item Transfer Master of this Item Already Generated", CommonClasses.MSG_Info);
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
            CommonClasses.SendError("Item Trasfer Master", "btnSubmit_Click", Ex.Message);
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
                if (ddlRawComponentCode.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Info);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlRawComponentCode.Focus();
                    return;
                }
                if (ddlRawComponentName.SelectedIndex == 0)
                {
                    ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Info);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlRawComponentName.Focus();
                    return;
                }
                InserRecord();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "btnInsert_Click", Ex.Message);
        }
    }

    private void InserRecord()
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");
        if (Convert.ToInt32(ViewState["RowCount"].ToString()) != 0)
        {
            ShowMessage("#Avisos", "Only One Item added in Item Transfer Master", CommonClasses.MSG_Info);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
        if (!Request.QueryString[0].Equals("MODIFY"))
        {
            DataTable dd = new DataTable();
            dd = CommonClasses.Execute("select * from TRANSFER_MASTER where TM_I_CODE='" + ddlSItemCodeno.SelectedValue.ToString() + "' AND ES_DELETE=0 ");
            if (dd.Rows.Count > 0)
            {
                ShowMessage("#Avisos", "Item Transfer Master of this Item Already Generated", CommonClasses.MSG_Info);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        #region CheckExist
        if (dgvBOMaterialDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
            {
                string TD_I_CODE = ((Label)(dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE"))).Text;
                if (ItemUpdateIndex == "-1")
                {
                    if (TD_I_CODE == ddlRawComponentCode.SelectedValue.ToString())
                    {
                        ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    if (TD_I_CODE == ddlRawComponentCode.SelectedValue.ToString() && ItemUpdateIndex != i.ToString())
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
        if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
        {
            dgvBOMaterialDetails.Enabled = true;
            for (int i = ((DataTable)ViewState["dt2"]).Rows.Count - 1; i >= 0; i--)
            {
                if (((DataTable)ViewState["dt2"]).Rows[i][1] == DBNull.Value)
                    ((DataTable)ViewState["dt2"]).Rows[i].Delete();
            }
            ((DataTable)ViewState["dt2"]).AcceptChanges();
        }
        else
        {
            dgvBOMaterialDetails.Enabled = false;
        }

        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("TD_I_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("UOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("TD_VQTY");
            ((DataTable)ViewState["dt2"]).Columns.Add("TD_SCRAPQTY");
        }
        #endregion

        #region Insert Record to Table
        dr = ((DataTable)ViewState["dt2"]).NewRow();
        dr["TD_I_CODE"] = ddlRawComponentCode.SelectedValue.ToString();
        dr["I_CODENO"] = ddlRawComponentCode.SelectedItem.ToString();
        dr["I_NAME"] = ddlRawComponentName.SelectedItem.ToString();
        dr["UOM"] = txtUOM.Text;
        dr["TD_VQTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtVQty.Text));
        dr["TD_SCRAPQTY"] = string.Format("{0:0.000}", Convert.ToDouble(txtScraQty.Text));
        #endregion

        #region check Data table,insert or Modify Data
        if (ViewState["str"].ToString() == "Modify")
        {
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"]));
                ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"]));
            }
        }
        else
        {
            ((DataTable)ViewState["dt2"]).Rows.Add(dr);
        }
        #endregion

        dgvBOMaterialDetails.DataSource = ((DataTable)ViewState["dt2"]);
        dgvBOMaterialDetails.DataBind();
        dgvBOMaterialDetails.Enabled = true;
        clearDetail();
        ViewState["RowCount"] = 1;
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
            CommonClasses.SendError("Item Trasfer Master", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("Item Trasfer Master", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("TRANSFER_MASTER", "MODIFY", "TM_CODE", mlCode);
            }
            Response.Redirect("~/Masters/VIEW/ViewItemTransferMaster.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "btnCancel_Click", Ex.Message);
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
            LoadDetaiItem();
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
            CommonClasses.SendError("Item Trasfer Master", "ddlSItemCodeno_SelectedIndexChanged", ex.Message);
        }
    }
    #endregion

    #region ddlSItemName_SelectedIndexChanged
    protected void ddlSItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LoadDetaiItem();
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
            CommonClasses.SendError("Item Trasfer Master", "ddlSItemName_SelectedIndexChanged", ex.Message);
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
            CommonClasses.SendError("Item Trasfer Master", "ddlRawComponentCode_SelectedIndexChanged", ex.Message);
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
            CommonClasses.SendError("Item Trasfer Master", "ddlSubComponentName_SelectedIndexChanged", ex.Message);
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
                ViewState["RowCount"] = 0;
                dgvBOMaterialDetails.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgvBOMaterialDetails.DataSource = ((DataTable)ViewState["dt2"]);
                dgvBOMaterialDetails.DataBind();
                if (dgvBOMaterialDetails.Rows.Count == 0)
                {
                    BlankGrid();
                }
            }
            if (e.CommandName == "Modify")
            {
                ViewState["str"] = "Modify";
                //str = "Modify";
                ViewState["RowCount"] = 0;
                ItemUpdateIndex = e.CommandArgument.ToString();
                ddlRawComponentCode.SelectedValue = ((Label)(row.FindControl("lblBD_I_CODE"))).Text;
                ddlRawComponentCode_SelectedIndexChanged(null, null);
                txtVQty.Text = ((Label)(row.FindControl("lblBD_VQTY"))).Text;

                txtScraQty.Text = ((Label)(row.FindControl("lblBD_SCRAPQTY"))).Text;
                if (txtScraQty.Text == "")
                {
                    txtScraQty.Text = "0.00";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "dgvBOMaterialDetails_RowCommand", Ex.Message);
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
            // str = "";
            ViewState["str"] = "0.00";
            ViewState["RowCount"] = 0;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "clearDetail", Ex.Message);
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
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                dt = CommonClasses.Execute("select distinct( I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 AND I_CAT_CODE= '-2147483648' ORDER BY I_CODENO");
            }
            else
            {
                //Not in used bcoz already transferd item can not be repeated again
                dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 AND I_CODE NOT IN(select TM_I_CODE from TRANSFER_MASTER WHERE TRANSFER_MASTER.ES_DELETE=0) ORDER BY I_CODENO");
            }
            ddlSItemCodeno.DataSource = dt;
            ddlSItemCodeno.DataTextField = "I_CODENO";
            ddlSItemCodeno.DataValueField = "I_CODE";
            ddlSItemCodeno.DataBind();
            ddlSItemCodeno.Items.Insert(0, new ListItem("Select Item Code", "0"));

            ddlSItemName.DataSource = dt;
            ddlSItemName.DataTextField = "I_NAME";
            ddlSItemName.DataValueField = "I_CODE";
            ddlSItemName.DataBind();
            ddlSItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "LoadCombos", ex.Message);
        }
        finally
        {
        }
    }
    #endregion

    private void LoadDetaiItem()
    {
        DataTable dt = new DataTable();

        if (Request.QueryString[0].Equals("MODIFY"))
        {
            dt = CommonClasses.Execute("select distinct( I_CODE),I_CODENO,I_NAME FROM ITEM_MASTER WHERE I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 AND I_CAT_CODE= '-2147483648' ORDER BY I_CODENO");
        }
        else
        {
            if (ddlSItemCodeno.SelectedIndex != 0)
            {
                //Not in used bcoz already transferd item can not be repeated again
                dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 AND I_CODE NOT IN(select TD_I_CODE from TRANSFER_DETAIL WHERE TRANSFER_DETAIL.ES_DELETE=0) AND I_CODE not in ('" + ddlSItemCodeno.SelectedValue + "') ORDER BY I_CODENO");
                // dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 AND I_CODE not in ('" + ddlSItemCodeno.SelectedValue + "') ORDER BY I_CODENO");
            }
            else
            {
                dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 AND I_CODE NOT IN(select TD_I_CODE from TRANSFER_DETAIL WHERE TRANSFER_DETAIL.ES_DELETE=0) ORDER BY I_CODENO");
                //dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 ORDER BY I_CODENO");
            }
        }

        ddlRawComponentCode.DataSource = dt;
        ddlRawComponentCode.DataTextField = "I_CODENO";
        ddlRawComponentCode.DataValueField = "I_CODE";
        ddlRawComponentCode.DataBind();
        ddlRawComponentCode.Items.Insert(0, new ListItem("Select Item Code", "0"));

        if (ddlSItemCodeno.SelectedIndex != 0)
        {
            dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 AND I_CODE not in ('" + ddlSItemCodeno.SelectedValue + "') ORDER BY I_CODENO");
        }
        else
        {
            dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO from ITEM_MASTER where I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0 and I_CAT_CODE=-2147483648 ORDER BY I_CODENO");
        }
        ddlRawComponentName.DataSource = dt;
        ddlRawComponentName.DataTextField = "I_NAME";
        ddlRawComponentName.DataValueField = "I_CODE";
        ddlRawComponentName.DataBind();
        ddlRawComponentName.Items.Insert(0, new ListItem("Select Item Name", "0"));
    }

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadCombos();

            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT TM_CODE,TM_I_CODE FROM TRANSFER_MASTER WHERE TM_CODE=" + mlCode + " AND TM_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ES_DELETE=0");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["TM_CODE"]); ;
                ddlSItemCodeno.SelectedValue = dt.Rows[0]["TM_I_CODE"].ToString();
                ddlSItemCodeno_SelectedIndexChanged(null, null);
                ddlSItemName.SelectedValue = dt.Rows[0]["TM_I_CODE"].ToString();

                GetDetails(mlCode);
                ddlSItemCodeno.Enabled = false;
                ddlSItemName.Enabled = false;
                if (ViewState["str"].ToString() == "VIEW")
                {
                    ddlSItemCodeno.Enabled = false;
                    ddlSItemName.Enabled = false;
                    btnSubmit.Visible = false;
                    dgvBOMaterialDetails.Enabled = false;
                }
                else if (ViewState["str"].ToString() == "MOD")
                {
                    CommonClasses.SetModifyLock("TRANSFER_MASTER", "MODIFY", "TM_CODE", mlCode);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region GetDetails
    private void GetDetails(int mlCode)
    {
        DataTable dtDetails = new DataTable();
        dtDetails = CommonClasses.Execute("SELECT TD_I_CODE,I_CODENO,I_NAME,I_UOM_NAME AS UOM,cast(TD_VQTY as Numeric(10,3)) as TD_VQTY,cast(TD_SCRAPQTY as Numeric(10,3)) as TD_SCRAPQTY FROM TRANSFER_MASTER,TRANSFER_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER WHERE TD_TM_CODE=" + mlCode + " AND TD_TM_CODE=TM_CODE AND TD_I_CODE=I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE");
        if (dtDetails != null && dtDetails.Rows.Count > 0)
        {
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            for (int j = 0; j < dtDetails.Rows.Count; j++)
            {
                dgvBOMaterialDetails.Enabled = true;
                ((DataTable)ViewState["dt2"]).Rows.Add();
                ((DataTable)ViewState["dt2"]).Rows[j]["TD_I_CODE"] = dtDetails.Rows[j]["TD_I_CODE"].ToString();
                ((DataTable)ViewState["dt2"]).Rows[j]["I_CODENO"] = dtDetails.Rows[j]["I_CODENO"].ToString();
                ((DataTable)ViewState["dt2"]).Rows[j]["I_NAME"] = dtDetails.Rows[j]["I_NAME"].ToString();
                ((DataTable)ViewState["dt2"]).Rows[j]["UOM"] = dtDetails.Rows[j]["UOM"].ToString();
                ((DataTable)ViewState["dt2"]).Rows[j]["TD_VQTY"] = dtDetails.Rows[j]["TD_VQTY"].ToString();
                ((DataTable)ViewState["dt2"]).Rows[j]["TD_SCRAPQTY"] = dtDetails.Rows[j]["TD_SCRAPQTY"].ToString();
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
            CommonClasses.SendError("Item Trasfer Master", "ShowMessage", Ex.Message);
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
            string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
            SqlTransaction trans;

            SqlConnection connection = new SqlConnection(strConnString);
            connection.Open();
            trans = connection.BeginTransaction();
            try
            {
                if (Request.QueryString[0].Equals("INSERT"))
                {
                    SqlCommand command = new SqlCommand("INSERT INTO TRANSFER_MASTER(TM_CM_COMP_ID,TM_I_CODE)VALUES('" + Convert.ToInt32(Session["CompanyId"]) + "','" + ddlSItemCodeno.SelectedValue.ToString() + "')", connection, trans);
                    command.ExecuteNonQuery();
                    string Code = "";
                    SqlCommand cmd1 = new SqlCommand("Select Max(TM_CODE) from TRANSFER_MASTER", connection, trans);
                    cmd1.Transaction = trans;
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    while (dr1.Read())
                    {
                        Code = (dr1[0].ToString().Trim());
                    }
                    cmd1.Dispose();
                    dr1.Dispose();

                    for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
                    {
                        SqlCommand command1 = new SqlCommand("INSERT INTO TRANSFER_DETAIL(TD_TM_CODE,TD_I_CODE,TD_VQTY,TD_SCRAPQTY)VALUES('" + Code + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_VQTY")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_SCRAPQTY")).Text + "')", connection, trans);
                        command1.ExecuteNonQuery();
                    }
                    trans.Commit();
                    CommonClasses.WriteLog("Item Trasfer Master", "Save", "Item Trasfer Master", ddlSItemCodeno.SelectedItem.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/Masters/VIEW/ViewItemTransferMaster.aspx", false);
                }
                else if (Request.QueryString[0].Equals("MODIFY"))
                {
                    SqlCommand command = new SqlCommand("UPDATE TRANSFER_MASTER SET TM_I_CODE='" + ddlSItemCodeno.SelectedValue.ToString() + "' WHERE TM_CODE='" + mlCode + "'", connection, trans);
                    command.ExecuteNonQuery();
                    result = CommonClasses.Execute1("DELETE FROM TRANSFER_DETAIL WHERE TD_TM_CODE='" + mlCode + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgvBOMaterialDetails.Rows.Count; i++)
                        {
                            SqlCommand command1 = new SqlCommand("INSERT INTO TRANSFER_DETAIL(TD_TM_CODE,TD_I_CODE,TD_VQTY)VALUES('" + mlCode + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_I_CODE")).Text + "','" + ((Label)dgvBOMaterialDetails.Rows[i].FindControl("lblBD_VQTY")).Text + "')", connection, trans);
                            command1.ExecuteNonQuery();
                        }
                        trans.Commit();
                        CommonClasses.RemoveModifyLock("TRANSFER_MASTER", "MODIFY", "TM_CODE", mlCode);
                        CommonClasses.WriteLog("Item Trasfer Master", "Update", "Item Trasfer Master", ddlSItemCodeno.SelectedValue.ToString(), mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect("~/Masters/VIEW/ViewItemTransferMaster.aspx", false);
                    }
                }
                ViewState["RowCount"] = 0;
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Trasfer Master", "SaveRec", ex.Message);
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