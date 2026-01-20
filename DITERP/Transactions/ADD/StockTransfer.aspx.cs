using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Transactions_ADD_StockTransfer : System.Web.UI.Page
{
    static int mlCode = 0;
    DataTable dt = new DataTable();
    DataTable dtDetail = new DataTable();
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static string right = "";
    public static string str = "";
    static DataTable dt2 = new DataTable();
    static string ItemUpdateIndex = "-1";
    public static int Index = 0;

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='161'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

                    ViewState["BindTable"] = BindTable;
                    ViewState["dt2"] = dt2;
                    ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                    ViewState["Index"] = Index;
                    try
                    {
                        ((DataTable)ViewState["BindTable"]).Clear();

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

                        if (Request.QueryString[0].Equals("INSERT"))
                        {
                            txtStockAdjustmentDate.Attributes.Add("readonly", "readonly");
                            str = "";
                            LoadICode();
                            LoadIName();
                            LoadICodeTran();
                            LoadINameTran();
                            BlankGridView();
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            ((DataTable)ViewState["dt2"]).Columns.Clear();
                            txtStockAdjustmentDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        }
                        txtStockAdjustmentDate.Focus();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Stock Transfer", "Page_Load", ex.Message.ToString());
                    }
                    FillCombo();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Stock Transfer", "Page_Load", ex.Message.ToString());
        }
    }

    #region FillCombo
    private void FillCombo()
    {
        try
        {
            DataTable dtUser = CommonClasses.Execute("SELECT * FROM USER_STORE_DETAIL WHERE UM_CODE ='" + Session["UserCode"] + "' ");
            if (dtUser.Rows.Count == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "User have no rights of store...";
                return;
            }
            string Codes = "";
            for (int i = 0; i < dtUser.Rows.Count; i++)
            {
                Codes = Codes + "'" + dtUser.Rows[i]["STORE_CODE"].ToString() + "'" + ",";
            }
            Codes = Codes.TrimEnd(',');
            CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (" + Codes + ") ORDER BY STORE_NAME", ddlStoreType);
            // CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 and STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (" + Codes + ") ORDER BY STORE_NAME", ddlTostore);
            ddlStoreType.Items.Insert(0, new ListItem("Select Store Name", "0"));
            // ddlTostore.Items.Insert(0, new ListItem("select Store", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "FillCombo", Ex.Message);
        }
    }
    #endregion

    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_CODENO from ITEM_MASTER,TRANSFER_DETAIL,TRANSFER_MASTER where ITEM_MASTER.ES_DELETE=0 AND TRANSFER_DETAIL.ES_DELETE=0 AND TM_I_CODE=I_CODE AND TM_CODE=TD_TM_CODE AND TRANSFER_MASTER.ES_DELETE=0  ORDER BY I_CODENO");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadICode", Ex.Message);
        }
    }

    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE,I_NAME from ITEM_MASTER,TRANSFER_DETAIL,TRANSFER_MASTER where ITEM_MASTER.ES_DELETE=0 AND TRANSFER_DETAIL.ES_DELETE=0 AND TM_I_CODE=I_CODE AND TM_CODE=TD_TM_CODE AND TRANSFER_MASTER.ES_DELETE=0  ORDER BY I_NAME");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production ", "LoadIName", Ex.Message);
        }
    }

    private void LoadICodeTran()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_CODENO  from ITEM_MASTER,TRANSFER_DETAIL,TRANSFER_MASTER where ITEM_MASTER.ES_DELETE=0 AND TRANSFER_DETAIL.ES_DELETE=0 AND TD_I_CODE=I_CODE AND TM_CODE=TD_TM_CODE AND TRANSFER_MASTER.ES_DELETE=0  ORDER BY I_CODENO");
            ddlItemTranCode.DataSource = dt;
            ddlItemTranCode.DataTextField = "I_CODENO";
            ddlItemTranCode.DataValueField = "I_CODE";
            ddlItemTranCode.DataBind();
            ddlItemTranCode.Items.Insert(0, new ListItem("Select Transfer Item Code", "0"));
            ddlItemTranCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadICode", Ex.Message);
        }
    }

    private void LoadINameTran()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_NAME from ITEM_MASTER,TRANSFER_DETAIL,TRANSFER_MASTER where ITEM_MASTER.ES_DELETE=0 AND TRANSFER_DETAIL.ES_DELETE=0 AND TD_I_CODE=I_CODE AND TM_CODE=TD_TM_CODE AND TRANSFER_MASTER.ES_DELETE=0  ORDER BY I_NAME");
            ddlItemTranName.DataSource = dt;
            ddlItemTranName.DataTextField = "I_NAME";
            ddlItemTranName.DataValueField = "I_CODE";
            ddlItemTranName.DataBind();
            ddlItemTranName.Items.Insert(0, new ListItem("Select Transfer Item Name", "0"));
            ddlItemTranName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production ", "LoadIName", Ex.Message);
        }
    }

    #region LoadCurrStock
    private void LoadCurrStock()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CURRENT_BAL,isnull(sum(STL_DOC_QTY),0) as STL_DOC_QTY from ITEM_MASTER,STOCK_LEDGER where ITEM_MASTER.ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CODE='" + ddlItemCode.SelectedValue + "' and I_CODE=STL_I_CODE  AND STL_STORE_TYPE='" + ddlStoreType.SelectedValue + "' group by I_CODE,I_CURRENT_BAL,I_INV_RATE ");

            if (dt.Rows.Count == 0)
            {
                txtCurrStock.Text = "0.000";
            }
            else
            {
                txtCurrStock.Text = dt.Rows[0]["STL_DOC_QTY"].ToString();
                txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "LoadCurrStock", Ex.Message);
        }
    }
    #endregion

    private void BlankGridView()
    {
        DataTable dtFilter = new DataTable();
        dtFilter.Clear();

        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("STD_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("STD_CURR_STOCK", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("STD_TRAN_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO_1", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ItemName_1", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("STD_TRAN_QTY", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgStockAdjustment.DataSource = dtFilter;
            dgStockAdjustment.DataBind();
            dgStockAdjustment.Enabled = false;
        }
    }

    private void ViewRec(string str)
    {
        try
        {
            txtStockAdjustmentDate.Attributes.Add("readonly", "readonly");
            LoadICode();
            ddlItemCode_SelectedIndexChanged(null, null);
            LoadIName();
            LoadICodeTran();
            LoadINameTran();
            dtDetail.Clear();
            dt = CommonClasses.Execute("SELECT STM_DOC_DATE,STM_DOC_NO,STM_STORE FROM STOCK_TRANSFER_MASTER where ES_DELETE=0 and STM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and STM_CODE=" + mlCode + "");

            if (dt.Rows.Count > 0)
            {
                ddlStoreType.SelectedValue = dt.Rows[0]["STM_STORE"].ToString();
                txtStockAdjustmentDate.Text = Convert.ToDateTime(dt.Rows[0]["STM_DOC_DATE"]).ToString("dd MMM yyyy");
                txtIssueNo.Text = dt.Rows[0]["STM_DOC_NO"].ToString();
                dtDetail = CommonClasses.Execute("SELECT STOCK_TRANSFER_MASTER.STM_CODE,ITEM_MASTER_1.I_CODENO as I_CODENO_1,ITEM_MASTER_1.I_NAME as ItemName_1,ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME AS ItemName,STOCK_TRANSFER_DETAIL.STD_CURR_STOCK, STOCK_TRANSFER_DETAIL.STD_TRAN_I_CODE,STOCK_TRANSFER_DETAIL.STD_TRAN_QTY, STOCK_TRANSFER_DETAIL.STD_I_CODE FROM STOCK_TRANSFER_MASTER INNER JOIN STOCK_TRANSFER_DETAIL ON STOCK_TRANSFER_MASTER.STM_CODE = STOCK_TRANSFER_DETAIL.STD_STM_CODE INNER JOIN ITEM_MASTER ON STOCK_TRANSFER_DETAIL.STD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_MASTER AS ITEM_MASTER_1 ON STOCK_TRANSFER_DETAIL.STD_TRAN_I_CODE = ITEM_MASTER_1.I_CODE where STOCK_TRANSFER_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND ITEM_MASTER_1.ES_DELETE=0 AND STD_STM_CODE=" + mlCode + " AND STM_CM_COMP_CODE= " + (string)Session["CompanyCode"] + "");
                
                if (dtDetail.Rows.Count > 0)
                {
                    dgStockAdjustment.DataSource = dtDetail;
                    dgStockAdjustment.DataBind();
                    ViewState["dt2"] = dtDetail;
                }
            }

            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                btnInsert.Enabled = false;
                txtStockAdjustmentDate.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlStoreType.Enabled = false;
                txtCurrStock.Enabled = false;
                txtStockAdjustmentQty.Enabled = false;
                dgStockAdjustment.Enabled = false;
                txtRemark.Enabled = false;
                ddlItemTranCode.Enabled = false;
                ddlItemTranName.Enabled = false;
                txtTranQty.Enabled = false;
            }
            else if (str == "MOD")
            {
                ddlStoreType.Enabled = false;
                txtStockAdjustmentDate.Enabled = false;
                CommonClasses.SetModifyLock("STOCK_TRANSFER_MASTER", "MODIFY", "STM_CODE", Convert.ToInt32(mlCode));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ViewRec", Ex.Message);
        }
    }

    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                DataTable dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO,TD_I_CODE,TM_I_CODE from ITEM_MASTER,TRANSFER_DETAIL,TRANSFER_MASTER where ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND TD_TM_CODE=TM_CODE AND I_CODE=TM_I_CODE AND TM_I_CODE='" + ddlItemCode.SelectedValue + "'");
                ddlItemTranCode.SelectedValue = dt.Rows[0]["TD_I_CODE"].ToString();
                ddlItemTranName.SelectedValue = ddlItemTranCode.SelectedValue;
                LoadCurrStock();
            }
            else
            {
                ddlItemName.SelectedIndex = 0;
            }
            txtStockAdjustmentQty.Text = "0";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }

    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                DataTable dt = CommonClasses.Execute("select I_CODE,I_NAME,I_CODENO,TD_I_CODE,TM_I_CODE from ITEM_MASTER,TRANSFER_DETAIL,TRANSFER_MASTER where ITEM_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 AND TD_TM_CODE=TM_CODE AND I_CODE=TM_I_CODE AND TM_I_CODE='" + ddlItemCode.SelectedValue + "'");
                ddlItemTranCode.SelectedValue = dt.Rows[0]["TD_I_CODE"].ToString();
                ddlItemTranName.SelectedValue = ddlItemTranCode.SelectedValue;
                LoadCurrStock();
            }
            else
            {
                ddlItemCode.SelectedIndex = 0;
            }
            txtStockAdjustmentQty.Text = "0";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }

    protected void ddlItemTranCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemTranCode.SelectedIndex != 0)
            {
                ddlItemTranName.SelectedValue = ddlItemTranCode.SelectedValue;
                //LoadCurrStock();
            }
            else
            {
                ddlItemTranName.SelectedIndex = 0;
            }
            txtTranQty.Text = "0";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemCode_SelectedIndexChanged", Ex.Message);
        }
    }

    protected void ddlItemTranName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemTranName.SelectedIndex != 0)
            {
                ddlItemTranCode.SelectedValue = ddlItemTranName.SelectedValue;
                // LoadCurrStock();
            }
            else
            {
                ddlItemTranCode.SelectedIndex = 0;
            }
            txtStockAdjustmentQty.Text = "0";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlStoreType.SelectedValue == "0")
            {
                PanelMsg.Visible = true;    
                lblmsg.Text = "Please select Store name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlStoreType.Focus();
                return;
            }
            if (txtStockAdjustmentDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Stock Adjustment Date Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentDate.Focus();
                return;
            }
            if (ddlItemCode.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            if (txtStockAdjustmentQty.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Stock Adjustment Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentQty.Focus();
                return;
            }
            if (ddlItemTranCode.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Transfer Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemTranCode.Focus();
                return;
            }
            if (ddlItemTranName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Transfer Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemTranName.Focus();
                return;
            }
            if (txtTranQty.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Transfer Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtTranQty.Focus();
                return;
            }
            if (txtCurrStock.Text.Trim() == "")
            {
                txtCurrStock.Text = "0";
            }
            if ((Convert.ToDouble(txtCurrStock.Text) + Convert.ToDouble(txtStockAdjustmentQty.Text)) < 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Adjustment Qty should not greater than Current Stock";
                txtCurrStock.Focus();
                return;
            }

            #region Check Duplicate Inserted_Grid
            if (dgStockAdjustment.Rows.Count > 0)
            {
                for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgStockAdjustment.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion Check Duplicate Inserted_Grid

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("STD_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName"); //change Binding Value Name From StockUOM1 to UOM_CODE
                ((DataTable)ViewState["dt2"]).Columns.Add("STD_CURR_STOCK");
                ((DataTable)ViewState["dt2"]).Columns.Add("STD_TRAN_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO_1");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName_1"); //change Binding Value Name From StockUOM1 to UOM_CODE
                ((DataTable)ViewState["dt2"]).Columns.Add("STD_TRAN_QTY");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["STD_I_CODE"] = ddlItemCode.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["STD_CURR_STOCK"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtCurrStock.Text)), 3));
            dr["STD_TRAN_I_CODE"] = ddlItemTranCode.SelectedValue;
            dr["I_CODENO_1"] = ddlItemTranCode.SelectedItem;
            dr["ItemName_1"] = ddlItemTranName.SelectedItem;
            dr["STD_TRAN_QTY"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtTranQty.Text)), 3));
            #endregion

            #region check Data table,insert or Modify Data
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

            #region Binding data to Grid
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                dgStockAdjustment.Enabled = true;
                dgStockAdjustment.Visible = true;
                dgStockAdjustment.DataSource = ((DataTable)ViewState["dt2"]);
                dgStockAdjustment.DataBind();
            }
            #endregion

            //To avoid same item insert in Grid
            ViewState["ItemUpdateIndex"] = "-1";
            clearDetail();
        }
        catch (Exception ex)
        {

        }
    }

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            ddlItemTranName.SelectedIndex = 0;
            ddlItemTranCode.SelectedIndex = 0;
            txtCurrStock.Text = "";
            txtTranQty.Text = "";
            txtStockAdjustmentQty.Text = "";
            //txtRemark.Text = "";
            // ddlTostore.SelectedIndex = 0;
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
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
            CommonClasses.SendError("Issue To Production", "btnCancel_Click", ex.Message.ToString());
        }
    }

    #endregion
    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtStockAdjustmentDate.Text == "")
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
            CommonClasses.SendError("Issue To Production", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion
    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("STOCK_TRANSFER_MASTER", "MODIFY", "STM_CODE", mlCode);
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions//VIEW/ViewStockTransfer.aspx", false);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production", "CancelRecord", Ex.Message);
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
            CommonClasses.SendError("Supplier Purchase Order ", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dgStockAdjustment.Enabled == true)
        {
            if (txtStockAdjustmentDate.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Stock Adjustment Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentDate.Focus();
                return;
            }
            if (ddlStoreType.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please select From Store";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlStoreType.Focus();
                return;
            }
            if (dgStockAdjustment.Rows.Count > 0)
            {
                SaveRec();
            }
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found In Table";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        }
    }

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("Select isnull(max(STM_DOC_NO),0) as STM_DOC_NO FROM STOCK_TRANSFER_MASTER WHERE STM_CM_COMP_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");
        if (dt.Rows[0]["STM_DOC_NO"] == null || dt.Rows[0]["STM_DOC_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["STM_DOC_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

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
                    int Doc_no = Convert.ToInt32(Numbering());
                    DataTable dt = new DataTable();

                    SqlCommand command = new SqlCommand("INSERT INTO STOCK_TRANSFER_MASTER(STM_DOC_DATE,STM_DOC_NO,STM_CM_COMP_CODE,STM_STORE) VALUES ('" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + Doc_no + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlStoreType.SelectedValue + "')", connection, trans);
                    command.ExecuteNonQuery();
                    {
                        string Code = "";
                        SqlCommand cmd1 = new SqlCommand("Select Max(STM_CODE) from STOCK_TRANSFER_MASTER", connection, trans);
                        cmd1.Transaction = trans;
                        SqlDataReader dr1 = cmd1.ExecuteReader();
                        while (dr1.Read())
                        {
                            Code = (dr1[0].ToString().Trim());
                        }
                        cmd1.Dispose();
                        dr1.Dispose();

                        for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                        {
                            //Inserting Into Issue To Production Detail
                            SqlCommand command1 = new SqlCommand("INSERT INTO STOCK_TRANSFER_DETAIL (STD_STM_CODE,STD_I_CODE,STD_CURR_STOCK,STD_TRAN_I_CODE,STD_TRAN_QTY) values ('" + Code + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_CURR_STOCK")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + "')", connection, trans);
                            command1.ExecuteNonQuery();
                            //if (result == true)
                            //{
                            // Inserting Into Stock Ledger
                            if (((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_CURR_STOCK")).Text != "0.000")
                            {
                                // Inserting Into Stock Ledger
                                //if (result == true)
                                //{
                                SqlCommand command2 = new SqlCommand("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Doc_no + "','Stock Transfer','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','-" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + "','" + ddlStoreType.SelectedValue + "')", connection, trans);//Insert From Store here
                                command2.ExecuteNonQuery();
                                SqlCommand command3 = new SqlCommand("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "','" + Code + "','" + Doc_no + "','Stock Transfer','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + "','" + ddlStoreType.SelectedValue + "')", connection, trans);//Insert From Store here
                                command3.ExecuteNonQuery();
                                // }
                                // relasing Stock Form Item Master
                                //if (result == true)
                                // {
                                if (ddlStoreType.SelectedValue == "-2147483642")
                                {
                                    SqlCommand command6 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)-" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'", connection, trans);
                                    command6.ExecuteNonQuery();
                                    SqlCommand command7 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "'", connection, trans);
                                    command7.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCommand command8 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'", connection, trans);
                                    command8.ExecuteNonQuery();
                                    SqlCommand command9 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "'", connection, trans);
                                    command9.ExecuteNonQuery();
                                }
                                //}
                                // }
                            }
                        }
                        trans.Commit();
                        CommonClasses.WriteLog("Stock Transfer", "Save", "Stock Transfer", Convert.ToString(Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        Response.Redirect("~/Transactions/VIEW/ViewStockTransfer.aspx", false);
                    }
                }
                else if (Request.QueryString[0].Equals("MODIFY"))
                {
                    SqlCommand command = new SqlCommand("UPDATE STOCK_TRANSFER_MASTER SET STM_DOC_DATE ='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where (STM_CODE='" + mlCode + "') and (STM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "')", connection, trans);
                    command.ExecuteNonQuery();
                    {
                        //---- Getting Old Details
                        DataTable DtOldDetails = CommonClasses.Execute("select STD_I_CODE,STD_TRAN_QTY,STD_TRAN_I_CODE from STOCK_TRANSFER_DETAIL WHERE STD_STM_CODE='" + mlCode + "'");

                        //---- Reseting Item Master Stock
                        for (int n = 0; n < DtOldDetails.Rows.Count; n++)
                        {
                            if (ddlStoreType.SelectedValue == "-2147483642")
                            {
                                if (Convert.ToInt32(DtOldDetails.Rows[n]["STD_TRAN_QTY"]) > 0)
                                {
                                    SqlCommand command1 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)-" + DtOldDetails.Rows[n]["STD_TRAN_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["STD_I_CODE"] + "'", connection, trans);
                                    command1.ExecuteNonQuery();
                                    SqlCommand command2 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + DtOldDetails.Rows[n]["STD_TRAN_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["STD_TRAN_I_CODE"] + "'", connection, trans);
                                    command2.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCommand command3 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)=" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["STD_TRAN_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["STD_I_CODE"] + "'", connection, trans);
                                    command3.ExecuteNonQuery();
                                    SqlCommand command4 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["STD_TRAN_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["STD_TRAN_I_CODE"] + "'", connection, trans);
                                    command4.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(DtOldDetails.Rows[n]["STD_TRAN_QTY"]) > 0)
                                {
                                    SqlCommand command5 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + DtOldDetails.Rows[n]["STD_TRAN_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["STD_I_CODE"] + "'", connection, trans);
                                    command5.ExecuteNonQuery();
                                    SqlCommand command6 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + DtOldDetails.Rows[n]["STD_TRAN_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["STD_TRAN_I_CODE"] + "'", connection, trans);
                                    command6.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCommand command7 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["STD_TRAN_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["STD_I_CODE"] + "'", connection, trans);
                                    command7.ExecuteNonQuery();
                                    SqlCommand command8 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["STD_TRAN_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["STD_TRAN_I_CODE"] + "'", connection, trans);
                                    command8.ExecuteNonQuery();
                                }
                            }
                        }
                        SqlCommand command9 = new SqlCommand("DELETE FROM STOCK_TRANSFER_DETAIL WHERE (STD_STM_CODE='" + mlCode + "')", connection, trans);
                        command9.ExecuteNonQuery();
                        SqlCommand command10 = new SqlCommand("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + mlCode + "' and STL_DOC_TYPE='Stock Transfer'", connection, trans);
                        command10.ExecuteNonQuery();

                        for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                        {
                            SqlCommand command11 = new SqlCommand("INSERT INTO STOCK_TRANSFER_DETAIL (STD_STM_CODE,STD_I_CODE,STD_CURR_STOCK,STD_TRAN_I_CODE,STD_TRAN_QTY) values ('" + mlCode + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_CURR_STOCK")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + "')", connection, trans);
                            command11.ExecuteNonQuery();
                            // Inserting Into Stock Ledger
                            //if (result == true)
                            //{
                            if (((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_CURR_STOCK")).Text != "0.000")
                            {
                                // Inserting Into Stock Ledger

                                SqlCommand command12 = new SqlCommand("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + mlCode + "','" + txtIssueNo.Text + "','Stock Transfer','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_CURR_STOCK")).Text + "','" + ddlStoreType.SelectedValue + "')", connection, trans);//Insert From Store here
                                command12.ExecuteNonQuery();
                                SqlCommand command13 = new SqlCommand("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + mlCode + "','" + txtIssueNo.Text + "','Stock Transfer','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + "','" + ddlStoreType.SelectedValue + "')", connection, trans);//Insert From Store here
                                command13.ExecuteNonQuery();

                                // relasing Stock Form Item Master
                                if (ddlStoreType.SelectedValue == "-2147483642")
                                {
                                    SqlCommand command14 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)-" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'", connection, trans);
                                    command14.ExecuteNonQuery();
                                    SqlCommand command15 = new SqlCommand("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "'", connection, trans);
                                    command15.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCommand command16 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'", connection, trans);
                                    command16.ExecuteNonQuery();
                                    SqlCommand command17 = new SqlCommand("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSTD_TRAN_I_CODE")).Text + "'", connection, trans);
                                    command17.ExecuteNonQuery();
                                }
                            }
                            //}
                        }
                        trans.Commit();
                        CommonClasses.RemoveModifyLock("STOCK_TRANSFER_MASTER", "MODIFY", "STM_CODE", mlCode);
                        CommonClasses.WriteLog("STOCK_TRANSFER_MASTER", "Update", "STOCK_TRANSFER_MASTER", txtIssueNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;

                        Response.Redirect("~/Transactions/VIEW/ViewStockTransfer.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("STOCK_TRANSFER_MASTER", "SaveRec", ex.Message);
        }
        return result;
    }

    protected void dgStockAdjustment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    #region dgStockAdjustment_RowCommand
    protected void dgStockAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgStockAdjustment.Rows[Convert.ToInt32(ViewState["Index"].ToString())];
            string I_code = ((Label)(row.FindControl("lblItemCode"))).Text;
            string QTy = ((Label)(row.FindControl("lblSTD_CURR_STOCK"))).Text;
            string I_code_Tran = ((Label)(row.FindControl("lblSTD_TRAN_I_CODE"))).Text;
            string Tran_QTy = ((Label)(row.FindControl("lblSTD_TRAN_QTY"))).Text;

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
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                dgStockAdjustment.DataSource = ((DataTable)ViewState["dt2"]);
                dgStockAdjustment.DataBind();
                if (dgStockAdjustment.Rows.Count == 0)
                    BlankGridView();
            }
            if (e.CommandName == "Modify")
            {
                LoadICode();
                LoadIName();
                str = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtStockAdjustmentQty.Text = ((Label)(row.FindControl("lblSTD_CURR_STOCK"))).Text;
                txtTranQty.Text = ((Label)(row.FindControl("lblSTD_TRAN_QTY"))).Text;
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    if (Convert.ToDouble(txtStockAdjustmentQty.Text) > 0)
                    {
                        txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text) - Convert.ToDouble(txtStockAdjustmentQty.Text));
                    }
                    else
                    {
                        txtCurrStock.Text = string.Format("{0:0.000}", Convert.ToDouble(txtCurrStock.Text) + Math.Abs(Convert.ToDouble(txtStockAdjustmentQty.Text)));
                    }
                }
                txtStockAdjustmentQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtStockAdjustmentQty.Text));
               // txtRemark.Text = ((Label)(row.FindControl("lblRemark"))).Text;
                foreach (GridViewRow gvr in dgStockAdjustment.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Transfer", "dgStockAdjustment_RowCommand", Ex.Message);
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

    protected void txtStockAdjustmentQty_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtStockAdjustmentQty.Text);
        txtStockAdjustmentQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    protected void txtStockAdjustmentDate_TextChanged(object sender, EventArgs e)
    {
        if (int.Parse(right.Substring(6, 1)) == 1)
        {
        }
        else
        {
            if (Convert.ToDateTime(txtStockAdjustmentDate.Text) >= DateTime.Now)
            {

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Do Back Date Entry";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtStockAdjustmentDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                return;
            }
        }
    }

    protected void ddlStoreType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ((DataTable)ViewState["dt2"]).Clear();
        BlankGridView();
        ddlItemCode_SelectedIndexChanged(null, null);
       // LoadCurrStock();
    }

    protected void txtTranQty_TextChanged(object sender, EventArgs e)
    {
        double TQty = Convert.ToDouble(txtTranQty.Text);
        double CStock = Convert.ToDouble(txtCurrStock.Text);

        if (TQty > CStock)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Transfer Qty. should not be greater than Curr. Stock";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            txtTranQty.Focus();
        }
    }
}

