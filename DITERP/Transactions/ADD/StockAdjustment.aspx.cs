using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_StockAdjustment : System.Web.UI.Page
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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='60'");
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
                            BlankGridView();
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            ((DataTable)ViewState["dt2"]).Columns.Clear();
                            txtStockAdjustmentDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        }
                        txtStockAdjustmentDate.Focus();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Stock Adjustment", "Page_Load", ex.Message.ToString());
                    }
                    FillCombo();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Stock Adjustment", "Page_Load", ex.Message.ToString());
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
            CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 and STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (" + Codes + ") ORDER BY STORE_NAME", ddlTostore);
            ddlStoreType.Items.Insert(0, new ListItem("Select To Store Name", "0"));
            ddlTostore.Items.Insert(0, new ListItem("select To Store", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "FillCombo", Ex.Message);
        }
    }
    #endregion

    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_NAME  from ITEM_MASTER where ES_DELETE=0 ORDER BY I_NAME ASC");
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

    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(I_CODE) as I_CODE, I_CODENO  from ITEM_MASTER where ES_DELETE=0 ORDER BY I_CODENO");
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

    private void BlankGridView()
    {
        DataTable dtFilter = new DataTable();
        dtFilter.Clear();

        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("SAD_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SAD_ADJUSTMENT_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SAD_REMARK", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SAD_TO_STORE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("SAD_TO_STORE_CODE", typeof(String)));
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
            dtDetail.Clear();
            dt = CommonClasses.Execute("SELECT SAM_DATE,SAM_DOC_NO,SAM_FROM_STORE FROM STOCK_ADJUSTMENT_MASTER where ES_DELETE=0 and SAM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and SAM_CODE=" + mlCode + "");

            if (dt.Rows.Count > 0)
            {
                ddlStoreType.SelectedValue = dt.Rows[0]["SAM_FROM_STORE"].ToString();
                txtIssueNo.Text = dt.Rows[0]["SAM_DOC_NO"].ToString();
                txtStockAdjustmentDate.Text = Convert.ToDateTime(dt.Rows[0]["SAM_DATE"]).ToString("dd MMM yyyy");

                dtDetail = CommonClasses.Execute("select SAD_I_CODE,I_NAME as ItemName,I_CODENO,cast(SAD_ADJUSTMENT_QTY as numeric(10,3)) as SAD_ADJUSTMENT_QTY,SAD_REMARK,SAD_TO_STORE as SAD_TO_STORE_CODE, '' as SAD_TO_STORE FROM STOCK_ADJUSTMENT_DETAIL,ITEM_MASTER where SAD_I_CODE=I_CODE and SAD_SAM_CODE='" + mlCode + "'  ");

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
            }
            else if (str == "MOD")
            {
                ddlStoreType.Enabled = false;
                txtStockAdjustmentDate.Enabled = false;
                CommonClasses.SetModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", Convert.ToInt32(mlCode));
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

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {

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
                txtStockAdjustmentDate.Focus();
                return;
            }
            if (txtCurrStock.Text.Trim() == "")
            {
                txtCurrStock.Text = "0";
            }
            if ((Convert.ToDouble(txtCurrStock.Text) + Convert.ToDouble(txtStockAdjustmentQty.Text)) < 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " Adjustment Qty should not greater than Current Stock";
                txtStockAdjustmentQty.Focus();
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
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName"); //change Binding Value Name From StockUOM1 to UOM_CODE
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_ADJUSTMENT_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_REMARK");
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_TO_STORE");
                ((DataTable)ViewState["dt2"]).Columns.Add("SAD_TO_STORE_CODE");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["SAD_I_CODE"] = ddlItemCode.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["SAD_ADJUSTMENT_QTY"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtStockAdjustmentQty.Text)), 3));
            dr["SAD_REMARK"] = txtRemark.Text;
            dr["SAD_TO_STORE"] = ddlTostore.SelectedItem.Text;
            dr["SAD_TO_STORE_CODE"] = ddlTostore.SelectedValue;//lblSAD_TO_STORE_CODE
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
            txtCurrStock.Text = "";
            txtStockAdjustmentQty.Text = "";
            txtRemark.Text = "";
            ddlTostore.SelectedIndex = 0;
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
                CommonClasses.RemoveModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", mlCode);
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions//VIEW/ViewStockAdjustment.aspx", false);

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
        dt = CommonClasses.Execute("Select isnull(max(SAM_DOC_NO),0) as SAM_DOC_NO FROM STOCK_ADJUSTMENT_MASTER WHERE SAM_CM_COMP_CODE = " + (string)Session["CompanyCode"] + "  AND SAM_TYPE=0 and ES_DELETE=0");
        if (dt.Rows[0]["SAM_DOC_NO"] == null || dt.Rows[0]["SAM_DOC_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["SAM_DOC_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    bool SaveRec()
    {
        bool result = false;
        try
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {

                int Doc_no = Convert.ToInt32(Numbering());
                DataTable dt = new DataTable();

                if (CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_MASTER(SAM_DATE,SAM_DOC_NO,SAM_CM_COMP_CODE,SAM_FROM_STORE) VALUES ('" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + Doc_no + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlStoreType.SelectedValue + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(SAM_CODE) from STOCK_ADJUSTMENT_MASTER");
                    for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                    {
                        //Inserting Into Issue To Production Detail
                        result = CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_DETAIL(SAD_SAM_CODE,SAD_I_CODE,SAD_ADJUSTMENT_QTY,SAD_REMARK,SAD_TO_STORE)VALUES('" + Code + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblRemark")).Text.Replace("'", "\''") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_TO_STORE_CODE")).Text + "')");

                        if (result == true)
                        {
                            // Inserting Into Stock Ledger
                            if (((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text != "0.000")
                            {
                                // Inserting Into Stock Ledger
                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Doc_no + "','STCADJ','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ddlStoreType.SelectedValue + "')");//Insert From Store here
                                    //result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Doc_no + "','STCADJ','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_TO_STORE_CODE")).Text + "')");//Insert to Store here
                                }
                                // relasing Stock Form Item Master
                                if (result == true)
                                {
                                    if (ddlStoreType.SelectedValue == "-2147483642")
                                    {
                                        result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                                    }
                                    else
                                    {
                                        result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                                    }
                                }
                            }
                        }
                    }

                    CommonClasses.WriteLog("Stock Adjustment", "Save", "Stock Adjustment", Convert.ToString(Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewStockAdjustment.aspx", false);

                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Could not saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStockAdjustmentDate.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE STOCK_ADJUSTMENT_MASTER SET SAM_DATE ='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "' where SAM_CODE='" + mlCode + "' and SAM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'"))
                {
                    //---- Getting Old Details
                    DataTable DtOldDetails = CommonClasses.Execute("select SAD_I_CODE,SAD_ADJUSTMENT_QTY from STOCK_ADJUSTMENT_DETAIL WHERE SAD_SAM_CODE='" + mlCode + "'");

                    //---- Reseting Item Master Stock
                    for (int n = 0; n < DtOldDetails.Rows.Count; n++)
                    {
                        if (ddlStoreType.SelectedValue == "-2147483642")
                        {
                            //   result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                            if (Convert.ToInt32(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"]) > 0)
                                CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)-" + DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                            else
                                CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                        }
                        else
                        {
                            if (Convert.ToInt32(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"]) > 0)
                                CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"] + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                            else
                                CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + Math.Abs(Convert.ToDouble(DtOldDetails.Rows[n]["SAD_ADJUSTMENT_QTY"].ToString())) + " where I_CODE='" + DtOldDetails.Rows[n]["SAD_I_CODE"] + "'");
                        }
                    }

                    result = CommonClasses.Execute1("DELETE FROM STOCK_ADJUSTMENT_DETAIL WHERE SAD_SAM_CODE='" + mlCode + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + mlCode + "' and STL_DOC_TYPE='STCADJ'");

                    if (result)
                    {

                        for (int i = 0; i < dgStockAdjustment.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO STOCK_ADJUSTMENT_DETAIL (SAD_SAM_CODE,SAD_I_CODE,SAD_ADJUSTMENT_QTY,SAD_REMARK,SAD_TO_STORE) values ('" + mlCode + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblRemark")).Text.Replace("'", "\''") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_TO_STORE_CODE")).Text + "')");

                            // Inserting Into Stock Ledger
                            if (result == true)
                            {
                                if (((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text != "0.000")
                                {
                                    // Inserting Into Stock Ledger
                                    if (result == true)
                                    {
                                        result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + mlCode + "','" + txtIssueNo.Text + "','STCADJ','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ddlStoreType.SelectedValue + "')");//Insert From Store here
                                        //  result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "','" + mlCode + "','" + txtIssueNo.Text + "','STCADJ','" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + "','" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_TO_STORE_CODE")).Text + "')");//Insert to Store here
                                    }
                                    // relasing Stock Form Item Master
                                    if (result == true)
                                    {
                                        if (ddlStoreType.SelectedValue == "-2147483642")
                                        {
                                            result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=ISNULL(I_DISPATCH_BAL,0)+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                                        }
                                        else
                                        {
                                            result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblSAD_ADJUSTMENT_QTY")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtStockAdjustmentDate.Text).ToString("dd MMM yyyy") + "'  where I_CODE='" + ((Label)dgStockAdjustment.Rows[i].FindControl("lblItemCode")).Text + "'");
                                        }
                                    }
                                }
                            }
                        }
                        CommonClasses.RemoveModifyLock("STOCK_ADJUSTMENT_MASTER", "MODIFY", "SAM_CODE", mlCode);
                        CommonClasses.WriteLog("STOCK_ADJUSTMENT_MASTER", "Update", "STOCK_ADJUSTMENT_MASTER", txtIssueNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewStockAdjustment.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStockAdjustmentDate.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("STOCK_ADJUSTMENT_MASTER", "SaveRec", ex.Message);
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
            string QTy = ((Label)(row.FindControl("lblSAD_ADJUSTMENT_QTY"))).Text;
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
                txtStockAdjustmentQty.Text = ((Label)(row.FindControl("lblSAD_ADJUSTMENT_QTY"))).Text;
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
                txtRemark.Text = ((Label)(row.FindControl("lblRemark"))).Text;
                foreach (GridViewRow gvr in dgStockAdjustment.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Adjustment", "dgStockAdjustment_RowCommand", Ex.Message);
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
    }
}
