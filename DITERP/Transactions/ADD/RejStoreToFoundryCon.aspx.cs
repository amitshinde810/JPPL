using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class Transactions_RejStoreToFoundryCon : System.Web.UI.Page
{
    #region Variable
    DataTable dtFilter = new DataTable();
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='150'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadFilter1();
                    ViewState["mlCode"] = mlCode;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["Index"] = Index;

                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        DiabaleTextBoxes(MainPanel);
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        EnabaleTextBoxes(MainPanel);
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        LoadCombos();
                        LoadConvertIntoItems();
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        str = "";
                        EnabaleTextBoxes(MainPanel);
                        dgvProductionStoreDetails.Enabled = false;
                        LoadFilter();
                        txtdocDate.Attributes.Add("readonly", "readonly");
                        txtdocDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                        txtDate.Attributes.Add("readonly", "readonly");
                        txtDate.Text = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy")).AddDays(-1).ToString("dd/MMM/yyyy");
                        txtDate_TextChanged(null, null);
                    }
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
            if (ddlConIntoItem.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlConIntoItem.Focus();
                return;
            }
            if (txtConQty.Text.Trim() == "" || Convert.ToDouble(txtConQty.Text) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockRejQty.Focus();
                return;
            }
            Boolean res = false;
            for (int i = 0; i < dgConvert.Rows.Count; i++)
            {
                CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);

                if (chkRow.Checked)
                {
                    res = true;
                }
            }
            if (res == false)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item From Grid ";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                dgConvert.Focus();
                return;
            }
            if (dgConvert.Enabled)
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion

    #region chkSelect_CheckedChanged
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            double BasicAmt = 0;
            for (int i = 0; i < dgConvert.Rows.Count; i++)
            {
                CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);

                if (chkRow.Checked)
                {
                    BasicAmt = BasicAmt + Convert.ToDouble(((Label)(dgConvert.Rows[i].FindControl("lblTOTALWEIGHT"))).Text);
                }
            }
            lblWeight.Text = BasicAmt.ToString();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Bill Passing", "chkSelect_CheckedChanged", ex.Message.ToString());
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
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("REJECTION_TO_FOUNDRY_MASTER", "MODIFY", "RTF_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect("~/Transactions/VIEW/RejStoreToFoundryCon.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "btnCancel_Click", Ex.Message);
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region InserRecord
    private void InserRecord()
    {
        #region Validations

        if (ddlRejItem.SelectedIndex == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Item Rej Item";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlRejItem.Focus();
            return;
        }
        if (ddlConIntoItem.SelectedIndex == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Select Item Converted Item ";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlConIntoItem.Focus();
            return;
        }
        if (txtStockRejQty.Text.Trim() == "" || Convert.ToDouble(txtStockRejQty.Text) == 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Please Enter Rej. Qty";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtStockRejQty.Focus();
            return;
        }
        #endregion

        #region CheckExist
        if (dgvProductionStoreDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgvProductionStoreDetails.Rows.Count; i++)
            {
                string bd_i_code = ((Label)(dgvProductionStoreDetails.Rows[i].FindControl("lblRTFD_REJ_ITEMCODE"))).Text;
                if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                {
                    if (bd_i_code == ddlRejItem.SelectedValue.ToString())
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Already Exist For This Item In Table";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }
                }
                else
                {
                    if (bd_i_code == ddlRejItem.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Already Exist For This Item In Table";
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
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_REJ_ITEMCODE", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_CON_ITEMCODE", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_REJ_ITEMNAME", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_CON_ITEMNAME", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_STK_REJ_QTY", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_STAND_WEIGHT", typeof(String)));
            ((DataTable)ViewState["dt2"]).Columns.Add(new System.Data.DataColumn("RTFD_CON_QTY", typeof(String)));
        }
        #endregion

        #region Insert Record to Table
        dr = ((DataTable)ViewState["dt2"]).NewRow();
        dr["RTFD_REJ_ITEMNAME"] = ddlRejItem.SelectedItem.ToString();
        dr["RTFD_CON_ITEMNAME"] = ddlConIntoItem.SelectedItem.ToString();
        dr["RTFD_REJ_ITEMCODE"] = ddlRejItem.SelectedValue.ToString();
        dr["RTFD_CON_ITEMCODE"] = ddlConIntoItem.SelectedValue.ToString();
        dr["RTFD_STK_REJ_QTY"] = txtStockRejQty.Text.ToString();
        dr["RTFD_STAND_WEIGHT"] = txtStandWeight.Text.ToString();
        dr["RTFD_CON_QTY"] = txtConQty.Text.ToString();
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
            if (ddlRejItem.SelectedIndex != -1)
            {
                ddlRejItem.SelectedValue = ddlRejItem.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlRejItem.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");

                DataTable dtitembatchno = CommonClasses.Execute("select BT_CODE,BT_NO from BATCH_MASTER where BT_WOD_I_CODE=" + ddlRejItem.SelectedValue + " and ES_DELETE =0");
                if (dt.Rows.Count > 0)
                {
                }
                else
                {

                }
                if (dtitembatchno.Rows.Count > 0)
                {
                }
                else
                {
                }
                ddlRejItem.Focus();
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
            if (ddlRejItem.SelectedIndex != -1)
            {
                ddlRejItem.SelectedValue = ddlRejItem.SelectedValue.ToString();
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,ITEM_UNIT_MASTER.I_UOM_NAME FROM ITEM_UNIT_MASTER,ITEM_MASTER WHERE ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE AND I_CODE='" + ddlRejItem.SelectedValue + "' AND I_CM_COMP_ID = " + (string)Session["CompanyId"] + " and ITEM_MASTER.ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                }
                else
                {
                }
                ddlRejItem.Focus();
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
            string I_code = ((Label)(row.FindControl("lblRTFD_REJ_ITEMCODE"))).Text;
            string QTy = ((Label)(row.FindControl("lblRTFD_STK_REJ_QTY"))).Text;
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                DataTable dt = new DataTable();
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
                    string Code = ((Label)(row.FindControl("lblRTFD_REJ_ITEMCODE"))).Text;
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
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlRejItem.SelectedValue = ((Label)(row.FindControl("lblRTFD_REJ_ITEMCODE"))).Text;
                ddlRejItem_SelectedIndexChanged(null, null);
                ddlConIntoItem.SelectedValue = ((Label)(row.FindControl("lblRTFD_CON_ITEMCODE"))).Text;
                txtStockRejQty.Text = ((Label)(row.FindControl("lblRTFD_STK_REJ_QTY"))).Text;
                txtConQty.Text = ((Label)(row.FindControl("lblRTFD_CON_QTY"))).Text;
                txtStandWeight.Text = ((Label)(row.FindControl("lblRTFD_STAND_WEIGHT"))).Text;
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    txtstock.Text = (Convert.ToDouble(txtStockRejQty.Text) + Convert.ToDouble(txtstock.Text)).ToString();
                }
                foreach (GridViewRow gvr in dgvProductionStoreDetails.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "dgMainPO_RowCommand", Ex.Message);
        }
    }

    #endregion
    #endregion

    #region txtGinDate_TextChanged
    protected void txtGinDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {
        }
        else
        {
            if (Convert.ToDateTime(txtdocDate.Text) >= DateTime.Now)
            {

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtdocDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }
    }
    #endregion

    #region txtDate_TextChanged
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {
        }
        else
        {
            if (Convert.ToDateTime(txtDate.Text) >= DateTime.Now)
            {

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtDate.Text = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MMM/yyyy")).AddDays(-1).ToString("dd/MMM/yyyy");
                return;
            }
        }


        DataTable dtlist = new DataTable();
        string storecode = "-2147483641";
        if (rbtWithAmt.SelectedValue == "-2147483647")
        {
            storecode = "-2147483628";
        }
        dtlist = CommonClasses.Execute(" SELECT I_CODE,I_CODENO,I_NAME,'' AS IMD_REC_DATE,ISNULL(SUM(STL_DOC_QTY),0) AS STL_DOC_QTY,I_UWEIGHT, ROUND(I_UWEIGHT*ISNULL(SUM(STL_DOC_QTY),0),2) AS TOTALWEIGHT   FROM STOCK_LEDGER,ITEM_MASTER where    STL_STORE_TYPE ='" + storecode + "'     AND I_CODE=STL_I_CODE   GROUP BY  I_CODE,I_CODENO,I_NAME,I_UWEIGHT    HAVING ISNULL(SUM(STL_DOC_QTY),0)>0  ORDER BY I_CODENO");
        if (dtlist.Rows.Count > 0)
        {
            dgConvert.DataSource = dtlist;
            dgConvert.DataBind();
        }
        else
        {
            dgConvert.DataSource = null;
            dgConvert.DataBind();
            LoadFilter1();
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter1()
    {
        if (dgConvert.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("STL_DOC_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("I_UWEIGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("TOTALWEIGHT", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgConvert.DataSource = dtFilter;
                dgConvert.DataBind();
                dgConvert.Enabled = false;
            }
        }
    }
    #endregion

    #region ddlConIntoItem_SelectedIndexChanged
    protected void ddlConIntoItem_SelectedIndexChanged(object sender, EventArgs e)
    { }
    #endregion

    #region ddlRejItem_SelectedIndexChanged
    protected void ddlRejItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRejItem.SelectedValue != "0")
            {
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("SELECT  ISNULL(SUM(STL_DOC_QTY),0) AS STL_DOC_QTY ,ISNULL(I_STD_WEIGHT,0) AS I_STD_WEIGHT FROM  STOCK_LEDGER,ITEM_MASTER where  STL_I_CODE=I_CODE AND STL_STORE_TYPE=-2147483641 AND STL_I_CODE='" + ddlRejItem.SelectedValue + "' GROUP BY I_STD_WEIGHT");
                if (dt.Rows.Count > 0)
                {
                    txtstock.Text = dt.Rows[0]["STL_DOC_QTY"].ToString();
                    txtStandWeight.Text = dt.Rows[0]["I_STD_WEIGHT"].ToString();
                    txtStockRejQty.Text = "0";
                    txtConQty.Text = "0";
                }
                else
                {
                    txtstock.Text = "0";
                    txtStandWeight.Text = "0";
                    txtStockRejQty.Text = "0";
                    txtConQty.Text = "0";
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #endregion

    #region Methods

    #region LoadCombos
    private void LoadCombos()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute(" SELECT DISTINCT I_CODENO,I_CODE,SUM(STL_DOC_QTY) AS STL_DOC_QTY  FROM STOCK_LEDGER,ITEM_MASTER where ES_DELETE=0 AND I_CM_COMP_ID='1' AND I_CAT_CODE=-2147483648 and STL_I_CODE=I_CODE and STL_STORE_TYPE=-2147483641  GROUP BY I_CODENO,I_CODE   HAVING (SUM(STL_DOC_QTY)) >0 ORDER BY I_CODENO");

            ddlRejItem.DataSource = dt;
            ddlRejItem.DataTextField = "I_CODENO";
            ddlRejItem.DataValueField = "I_CODE";
            ddlRejItem.DataBind();
            ddlRejItem.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "LoadCombos", ex.Message);
        }
        finally
        {

        }
    }
    #endregion

    #region LoadConvertIntoItems
    private void LoadConvertIntoItems()
    {
        DataTable dt = new DataTable();

        dt = CommonClasses.FillCombo("ITEM_MASTER", "I_CODENO", "I_CODE", "ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and I_CODE in ( SELECT  I_CODE   FROM  RAISER where CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ) order by I_CODENO ", ddlConIntoItem);
        ddlConIntoItem.Items.Insert(0, new ListItem("Select Rej. Item Name", "0"));
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtdocDate.Attributes.Add("readonly", "readonly");
            LoadCombos();
            LoadConvertIntoItems();
            dt = CommonClasses.Execute("select RTF_CODE,RTF_DOC_NO,convert(varchar,RTF_DOC_DATE,106) as RTF_DOC_DATE ,   RTF_I_CODE  , RTF_QTY ,convert(varchar,RTF_DATE,106) as RTF_DATE  ,ISNULL(RTF_CQTY,0) AS RTF_CQTY  from REJECTION_TO_FOUNDRY_MASTER WHERE RTF_CM_CODE = '" + Session["CompanyCode"].ToString() + "' and RTF_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and ES_DELETE=0 ");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["RTF_CODE"]);
                txtdocNo.Text = dt.Rows[0]["RTF_DOC_NO"].ToString();
                txtdocDate.Text = Convert.ToDateTime(dt.Rows[0]["RTF_DOC_DATE"]).ToString("dd MMM yyyy");
                txtConQty.Text = dt.Rows[0]["RTF_QTY"].ToString();
                txtDate.Text = Convert.ToDateTime(dt.Rows[0]["RTF_DATE"]).ToString("dd MMM yyyy");
                ddlConIntoItem.SelectedValue = dt.Rows[0]["RTF_I_CODE"].ToString();
                lblWeight.Text = dt.Rows[0]["RTF_CQTY"].ToString();
                ddlConIntoItem.Enabled = false;

                dtProductionDetail = CommonClasses.Execute("  SELECT I_CODE,I_CODENO,I_NAME, I_UWEIGHT, ISNULL(SUM(RTFD_STK_REJ_QTY),0) AS STL_DOC_QTY ,ROUND(ISNULL(SUM(RTFD_STK_REJ_QTY),0)* I_UWEIGHT,2) AS TOTALWEIGHT FROM REJECTION_TO_FOUNDRY_DETAIL,ITEM_MASTER where RTFD_RTF_CODE='" + ViewState["mlCode"].ToString() + "' AND RTFD_REJ_ITEMCODE=I_CODE  GROUP BY  I_CODE,I_CODENO,I_NAME,I_UWEIGHT ");

                if (dtProductionDetail.Rows.Count != 0)
                {
                    dgConvert.DataSource = dtProductionDetail;
                    dgConvert.DataBind();
                    ViewState["dt2"] = dtProductionDetail;
                    for (int i = 0; i < dgConvert.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        chkRow.Checked = true;
                    }
                }
            }
            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("REJECTION_TO_FOUNDRY_MASTER", "ES_MODIFY", "RTF_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                DiabaleTextBoxes(MainPanel);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "ViewRec", Ex.Message);
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

            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "GetValues", ex.Message);
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
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Numbering
    string Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(cast((isnull(RTF_DOC_NO,0)) as numeric(10,0))) as RTF_DOC_NO from REJECTION_TO_FOUNDRY_MASTER where ES_DELETE=0  AND RTF_CM_CODE='" + Session["CompanyCode"] + "' ");
        if (dt.Rows[0]["RTF_DOC_NO"] == null || dt.Rows[0]["RTF_DOC_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["RTF_DOC_NO"]) + 1;
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
                txtdocNo.Text = Numbering();
                string Code = "";
                string Mainstorecode = "-2147483647";
                if (rbtWithAmt.SelectedValue == "-2147483647")
                {
                    Mainstorecode = "-2147483635";
                }
                if (CommonClasses.Execute1(" INSERT INTO REJECTION_TO_FOUNDRY_MASTER(RTF_DOC_NO, RTF_DOC_DATE, RTF_CM_CODE,RTF_I_CODE,RTF_QTY,RTF_DATE,RTF_CQTY,RTF_STORE_CODE)VALUES ('" + txtdocNo.Text + "','" + Convert.ToDateTime(txtdocDate.Text).ToString("dd/MMM/yyyy") + "','" + (string)Session["CompanyCode"] + "','" + ddlConIntoItem.SelectedValue + "','" + txtConQty.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + lblWeight.Text + "','" + Mainstorecode + "')"))
                {
                    Code = CommonClasses.GetMaxId("Select Max(RTF_CODE) from REJECTION_TO_FOUNDRY_MASTER");
                    string storecode = "-2147483641";
                    if (rbtWithAmt.SelectedValue == "-2147483647")
                    {
                        storecode = "-2147483628";
                    }
                    for (int i = 0; i < dgConvert.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        if (chkRow.Checked)
                        {
                            result = CommonClasses.Execute1("INSERT INTO REJECTION_TO_FOUNDRY_DETAIL( RTFD_RTF_CODE, RTFD_REJ_ITEMCODE,  RTFD_STK_REJ_QTY ) values ( '" + Code + "','" + ((Label)dgConvert.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgConvert.Rows[i].FindControl("lblSTL_DOC_QTY")).Text + "'  )");
                            if (result == true)
                            {

                                // Inserting Into Stock Ledger
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgConvert.Rows[i].FindControl("lblI_CODE")).Text + "','" + Code + "','" + txtdocNo.Text.Trim() + "','Issue for Casting Conversion','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((Label)dgConvert.Rows[i].FindControl("lblSTL_DOC_QTY")).Text + "','" + storecode + "')");//Insert From Store here

                                //relasing Stock Form Item Master 
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((Label)dgConvert.Rows[i].FindControl("lblSTL_DOC_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' where I_CODE='" + ((Label)dgConvert.Rows[i].FindControl("lblI_CODE")).Text + "'");
                            }
                        }
                    }
                    CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ddlConIntoItem.SelectedValue + "','" + Code + "','" + txtdocNo.Text.Trim() + "','Conversion Of Casting','" + Convert.ToDateTime(txtdocDate.Text).ToString("dd/MMM/yyyy") + "','" + txtConQty.Text.Trim() + "','" + storecode + "')");//Insert From Store here

                    CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ddlConIntoItem.SelectedValue + "','" + Code + "','" + txtdocNo.Text.Trim() + "','Issue To Main Store','" + Convert.ToDateTime(txtdocDate.Text).ToString("dd/MMM/yyyy") + "','-" + txtConQty.Text.Trim() + "','" + storecode + "')");//Insert From Store here
                }
                CommonClasses.WriteLog("Casting Convetsion ", "Save", "Casting Convetsion ", Convert.ToString(txtdocNo.Text), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                ((DataTable)ViewState["dt2"]).Rows.Clear();
                Response.Redirect("~/Transactions/VIEW/RejStoreToFoundryCon.aspx", false);
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1(" UPDATE REJECTION_TO_FOUNDRY_MASTER SET RTF_DOC_DATE ='" + Convert.ToDateTime(txtdocDate.Text).ToString("dd/MMM/yyyy") + "'  , RTF_QTY='" + txtConQty.Text.Trim() + "' ,RTF_CQTY='" + lblWeight.Text + "'   WHERE (RTF_CODE = '" + ViewState["mlCode"].ToString() + "') "))
                {
                    DataTable dtDetail = new DataTable();

                    string storecode = "-2147483641";
                    if (rbtWithAmt.SelectedValue == "-2147483647")
                    {
                        storecode = "-2147483628";
                    }
                    dtDetail = CommonClasses.Execute("SELECT * FROM REJECTION_TO_FOUNDRY_DETAIL ,REJECTION_TO_FOUNDRY_MASTER where RTF_CODE=RTFD_RTF_CODE  AND ES_DELETE=0 AND RTFD_RTF_CODE='" + ViewState["mlCode"].ToString() + "'");
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtDetail.Rows[j]["RTFD_STK_REJ_QTY"].ToString() + "  where I_CODE='" + dtDetail.Rows[j]["RTFD_REJ_ITEMCODE"].ToString() + "'");
                    }
                    result = CommonClasses.Execute1(" DELETE FROM STOCK_LEDGER where STL_DOC_NO='" + ViewState["mlCode"].ToString() + "' AND  STL_DOC_TYPE IN ('Issue for Casting Conversion','Conversion Of Casting','Issue To Main Store')");
                    result = CommonClasses.Execute1(" DELETE FROM REJECTION_TO_FOUNDRY_DETAIL where RTFD_RTF_CODE ='" + ViewState["mlCode"].ToString() + "'  ");

                    for (int i = 0; i < dgConvert.Rows.Count; i++)
                    {
                        CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);
                        if (chkRow.Checked)
                        {
                            result = CommonClasses.Execute1("INSERT INTO REJECTION_TO_FOUNDRY_DETAIL( RTFD_RTF_CODE, RTFD_REJ_ITEMCODE,  RTFD_STK_REJ_QTY ) values ( '" + ViewState["mlCode"].ToString() + "','" + ((Label)dgConvert.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgConvert.Rows[i].FindControl("lblSTL_DOC_QTY")).Text + "'  )");
                            if (result == true)
                            {
                                // Inserting Into Stock Ledger
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgConvert.Rows[i].FindControl("lblI_CODE")).Text + "','" + ViewState["mlCode"].ToString() + "','" + txtdocNo.Text.Trim() + "','Issue for Casting Conversion','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((Label)dgConvert.Rows[i].FindControl("lblSTL_DOC_QTY")).Text + "','" + storecode + "')");//Insert From Store here

                                //relasing Stock Form Item Master 
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((Label)dgConvert.Rows[i].FindControl("lblSTL_DOC_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' where I_CODE='" + ((Label)dgConvert.Rows[i].FindControl("lblI_CODE")).Text + "'");
                            }
                        }
                    }
                    CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ddlConIntoItem.SelectedValue + "','" + ViewState["mlCode"].ToString() + "','" + txtdocNo.Text.Trim() + "','Conversion Of Casting','" + Convert.ToDateTime(txtdocDate.Text).ToString("dd/MMM/yyyy") + "','" + txtConQty.Text.Trim() + "','" + storecode + "')");//Insert From Store here

                    CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ddlConIntoItem.SelectedValue + "','" + ViewState["mlCode"].ToString() + "','" + txtdocNo.Text.Trim() + "','Issue To Main Store','" + Convert.ToDateTime(txtdocDate.Text).ToString("dd/MMM/yyyy") + "','-" + txtConQty.Text.Trim() + "','" + storecode + "')");//Insert From Store here
                }
                CommonClasses.RemoveModifyLock("REJECTION_TO_FOUNDRY_MASTER", "MODIFY", "RTF_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                CommonClasses.WriteLog("Casting Convetsion ", "Update", "Casting Convetsion ", Convert.ToString(txtdocNo.Text), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                ((DataTable)ViewState["dt2"]).Rows.Clear();
                Response.Redirect("~/Transactions/VIEW/RejStoreToFoundryCon.aspx", false);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            DataTable dt = CommonClasses.Execute("select ES_MODIFY from REJECTION_TO_FOUNDRY_MASTER where RTF_CODE=" + PrimaryKey + "  ");
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
            CommonClasses.SendError("REJECTION_TO_FOUNDRY-ADD", "ModifyLog", Ex.Message);
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
            txtstock.Text = "0";
            ddlConIntoItem.SelectedIndex = -1;
            ddlRejItem.SelectedIndex = -1;
            txtStockRejQty.Text = "0.000";
            txtStandWeight.Text = "0.000";
            txtConQty.Text = "0.000";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("REJECTION_TO_FOUNDRY", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgvProductionStoreDetails.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_REJ_ITEMCODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_CON_ITEMCODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_REJ_ITEMNAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_CON_ITEMNAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_STK_REJ_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_STAND_WEIGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RTFD_CON_QTY", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgvProductionStoreDetails.DataSource = dtFilter;
                dgvProductionStoreDetails.DataBind();
                dgvProductionStoreDetails.Enabled = false;
            }
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

    #region txtStockRejQty_TextChanged
    protected void txtStockRejQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDouble(txtstock.Text) < Convert.ToDouble(txtStockRejQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Rej Qty Should be less than Stock Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockRejQty.Text = "";
                txtStockRejQty.Focus();
                return;
            }
            if (txtStockRejQty.Text.Trim() != "")
            {
                txtConQty.Text = (Convert.ToDouble(txtStandWeight.Text) * Convert.ToDouble(txtStockRejQty.Text)).ToString();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region chkSelectAll_CheckedChanged
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked == true)
        {
            for (int i = 0; i < dgConvert.Rows.Count; i++)
            {
                CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);
                chkRow.Checked = true;
            }
            chkSelect_CheckedChanged(null, null);
        }
        else
        {
            for (int i = 0; i < dgConvert.Rows.Count; i++)
            {
                CheckBox chkRow = (((CheckBox)(dgConvert.Rows[i].FindControl("chkSelect"))) as CheckBox);
                chkRow.Checked = false;
            }
            chkSelect_CheckedChanged(null, null);
        }
    }
    #endregion

    #region rbtWithAmt_SelectedIndexChanged
    protected void rbtWithAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtDate_TextChanged(null, null);
        //LoadCombos();
        //ddlItemCode_SelectedIndexChanged(null, null);
    }
    #endregion rbtWithAmt_SelectedIndexChanged
}
