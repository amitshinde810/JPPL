using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_Delivery_Challan : System.Web.UI.Page
{
    #region Variable
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    static int mlCode = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    static string msg = "";
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Sale1MV");
            home1.Attributes["class"] = "active";

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
                        ViewState["mlCode"] = mlCode;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        ViewState["dt2"] = dt2;
                        LoadCustomer();
                        LoadICode();
                        LoadIName();
                        LoadStore();
                        LoadUnit();
                        str = "";
                        if (Request.QueryString[0].Equals("VIEW"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            ((DataTable)ViewState["dt2"]).Rows.Clear();
                            LoadFilter();
                            txtChallanDate.Attributes.Add("readonly", "readonly");
                            txtChallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            txtOrderDate.Attributes.Add("readonly", "readonly");
                            txtOrderDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            dgMainDC.Enabled = false;
                        }
                        txtOrderNo.Focus();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Delivery Challan", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Delivery Challan", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Customer Name", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }
            if (dgMainDC.Enabled && dgMainDC.Rows.Count > 0)
            {
                SaveRec();
            }
            else
            {
                ShowMessage("#Avisos", "Record Not Exist In Table", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion btnSubmit_Click

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlCustomer.SelectedIndex == 0)
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
            CommonClasses.SendError("Delivery Challan", "CheckValid", Ex.Message);
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
            CommonClasses.SendError("Delivery Challan", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("DELIVERY_CHALLAN_MASTER", "MODIFY", "DCM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewDeliveryChallan.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "CancelRecord", Ex.Message);
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
            CommonClasses.SendError("Delivery Challan", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtChallanDate.Attributes.Add("readonly", "readonly");
            txtOrderDate.Attributes.Add("readonly", "readonly");

            LoadCustomer();
            LoadICode();
            LoadIName();
            dtInwardDetail.Clear();

            dt = CommonClasses.Execute("Select DCM_CODE,DCM_P_CODE,DCM_NO,DCM_CM_CODE,DCM_DATE,DCM_INV_NO,DCM_THROUGH,DCM_VEH_NO,DCM_LR_NO,DCM_ORDER_NO,DCM_ORDER_DATE,MODIFY,DCM_IS_RETURNABLE from DELIVERY_CHALLAN_MASTER where ES_DELETE=0 and DCM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and DCM_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["DCM_CODE"]); ;
                ddlCustomer.SelectedValue = dt.Rows[0]["DCM_P_CODE"].ToString();
                txtChallanNumber.Text = dt.Rows[0]["DCM_NO"].ToString();
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["DCM_DATE"]).ToString("dd MMM yyyy");
                txtInvoiceNo.Text = dt.Rows[0]["DCM_INV_NO"].ToString();
                txtThrough.Text = dt.Rows[0]["DCM_THROUGH"].ToString();
                txtLRno.Text = dt.Rows[0]["DCM_LR_NO"].ToString();
                txtVehicleNo.Text = dt.Rows[0]["DCM_VEH_NO"].ToString();
                txtOrderNo.Text = dt.Rows[0]["DCM_ORDER_NO"].ToString();
                txtOrderDate.Text = Convert.ToDateTime(dt.Rows[0]["DCM_ORDER_DATE"]).ToString("dd MMM yyyy");
                chkIsReturn.Checked = Convert.ToBoolean(dt.Rows[0]["DCM_IS_RETURNABLE"].ToString());

                dtInwardDetail = CommonClasses.Execute("select DCD_I_CODE as ItemCode,I_CODENO as ItemCodeNo, I_NAME as ItemName,STORE_CODE AS StoreCode,STORE_NAME AS StoreName,DCD_UM_CODE as Unit1,I_UOM_NAME as Unit,cast(DCD_ORD_QTY as numeric(10,3))  as OrderQty,DCD_REMARK AS Remark,0 as Stock From ITEM_UNIT_MASTER,dbo.DELIVERY_CHALLAN_DETAIL,ITEM_MASTER,STORE_MASTER where DCD_I_CODE=I_CODE AND DELIVERY_CHALLAN_DETAIL.ES_DELETE=0 and  DCD_STORE_CODE=STORE_CODE  AND  ITEM_UNIT_MASTER.I_UOM_CODE=DCD_UM_CODE and DCD_DCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' order by DCD_I_CODE");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgMainDC.DataSource = dtInwardDetail;
                    dgMainDC.DataBind();
                    dgMainDC.Enabled = true;
                    ViewState["dt2"] = dtInwardDetail;
                }
                else
                {
                    dtInwardDetail.Rows.Add(dtInwardDetail.NewRow());
                    dgMainDC.DataSource = dtInwardDetail;
                    dgMainDC.DataBind();
                    dgMainDC.Enabled = false;
                }
            }
            if (str == "VIEW")
            {
                txtVehicleNo.Enabled = false;
                txtLRno.Enabled = false;
                txtRemark.Enabled = false;
                txtChallanDate.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtStock.Enabled = false;
                txtOrderDate.Enabled = false;
                txtOrderNo.Enabled = false;
                txtOrderQty.Enabled = false;
                txtThrough.Enabled = false;
                ddlUnit.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                btnSubmit.Enabled = false;
                btnInsert.Enabled = false;
                dgMainDC.Enabled = false;
            }
            if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                CommonClasses.SetModifyLock("DELIVERY_CHALLAN_MASTER", "MODIFY", "DCM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region LoadUnit
    private void LoadUnit()
    {
        try
        {
            dt = CommonClasses.Execute("select I_UOM_CODE ,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ");
            ddlUnit.DataSource = dt;
            ddlUnit.DataTextField = "I_UOM_NAME";
            ddlUnit.DataValueField = "I_UOM_CODE";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("Unit", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier PO Transaction", "LoadUnit", Ex.Message);
        }
    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_ACTIVE_IND=1 order by P_NAME--and P_TYPE='1'");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND I_CAT_CODE!=-2147483633 ORDER BY I_CODENO--and I_CAT_CODE IN ('-2147483648' ,'-2147483633')");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + "   AND I_CAT_CODE!=-2147483633  ORDER BY I_NAME--and I_CAT_CODE IN ('-2147483648' ,'-2147483633')");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "LoadIName", Ex.Message);
        }
    }
    #endregion

    #region LoadUOM
    private void LoadUOM()
    {
        DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE, I_UOM_NAME from ITEM_UNIT_MASTER,DELIVERY_CHALLAN_DETAIL where DELIVERY_CHALLAN_DETAIL.DCM_UM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and DELIVERY_CHALLAN_DETAIL.ES_DELETE=0 and DCD_I_CODE='" + ddlItemCode.SelectedValue + "'");
        ddlUnit.DataSource = dt1;
        ddlUnit.DataTextField = "I_UOM_NAME";
        ddlUnit.DataValueField = "I_UOM_CODE";
        ddlUnit.DataBind();
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Po_Doc_no = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(DCM_NO),0) as DCM_NO FROM DELIVERY_CHALLAN_MASTER WHERE DCM_CM_CODE = " + Convert.ToInt32(Session["CompanyCode"]) + " AND  DCM_TYPE='DLC' and ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["DCM_NO"]);
                    Po_Doc_no = Po_Doc_no + 1;
                }
                if (CommonClasses.Execute1("INSERT INTO DELIVERY_CHALLAN_MASTER (DCM_CM_CODE,DCM_P_CODE,DCM_TYPE,DCM_NO,DCM_DATE,DCM_ORDER_DATE,DCM_INV_NO,DCM_THROUGH,DCM_VEH_NO,DCM_LR_NO,DCM_ORDER_NO,DCM_IS_RETURNABLE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlCustomer.SelectedValue + "','DLC','" + Po_Doc_no + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtOrderDate.Text).ToString("dd/MMM/yyyy") + "','" + txtInvoiceNo.Text + "','" + txtThrough.Text + "','" + txtVehicleNo.Text + "','" + txtLRno.Text + "','" + txtOrderNo.Text + "','" + chkIsReturn.Checked.ToString() + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(DCM_CODE) from DELIVERY_CHALLAN_MASTER");
                    for (int i = 0; i < dgMainDC.Rows.Count; i++)
                    {
                        result = CommonClasses.Execute1("INSERT INTO DELIVERY_CHALLAN_DETAIL (DCD_DCM_CODE,DCD_I_CODE,DCD_ORD_QTY,DCD_REMARK,DCD_UM_CODE,DCD_STORE_CODE) values ('" + Code + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblRemark")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblUnit1")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                        if (result)
                        {
                            result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Po_Doc_no + "','DCOUT','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                        }
                        if (result)
                        {
                            if (((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text == "-2147483642")
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=I_DISPATCH_BAL-" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }
                            else
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }
                        }
                    }
                    CommonClasses.WriteLog("Delivery Challan", "Save", "Delivery Challan", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewDeliveryChallan.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    txtChallanNumber.Focus();
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE DELIVERY_CHALLAN_MASTER SET DCM_P_CODE='" + ddlCustomer.SelectedValue + "',DCM_NO='" + txtChallanNumber.Text + "',DCM_TYPE='DLC',DCM_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "',DCM_INV_NO='" + txtInvoiceNo.Text + "',DCM_THROUGH='" + txtThrough.Text + "',DCM_VEH_NO='" + txtVehicleNo.Text + "',DCM_LR_NO='" + txtLRno.Text + "',DCM_ORDER_NO='" + txtOrderNo.Text + "',DCM_ORDER_DATE='" + Convert.ToDateTime(txtOrderDate.Text).ToString("dd/MMM/yyyy") + "',DCM_IS_RETURNABLE='" + chkIsReturn.Checked.ToString() + "' where DCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    DataTable dtq = CommonClasses.Execute("SELECT DCD_ORD_QTY,DCD_I_CODE   ,DCD_STORE_CODE  FROM DELIVERY_CHALLAN_DETAIL where DCD_DCM_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + " ");
                    for (int i = 0; i < dtq.Rows.Count; i++)
                    {
                        if (dtq.Rows[i]["DCD_STORE_CODE"].ToString() == "-2147483642")
                        {
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=I_DISPATCH_BAL+" + dtq.Rows[i]["DCD_ORD_QTY"] + " where I_CODE='" + dtq.Rows[i]["DCD_I_CODE"] + "'");
                        }
                        else
                        {
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["DCD_ORD_QTY"] + " where I_CODE='" + dtq.Rows[i]["DCD_I_CODE"] + "'");
                        }
                    }
                    result = CommonClasses.Execute1("DELETE FROM DELIVERY_CHALLAN_DETAIL WHERE DCD_DCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"]) + "' and STL_DOC_TYPE='DCOUT'");
                    if (result)
                    {
                        for (int i = 0; i < dgMainDC.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO DELIVERY_CHALLAN_DETAIL (DCD_DCM_CODE,DCD_I_CODE,DCD_ORD_QTY,DCD_REMARK,DCD_UM_CODE,DCD_STORE_CODE) values ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblRemark")).Text.Trim().Replace("'", "\''") + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblUnit1")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                            if (result)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + Convert.ToInt32(ViewState["mlCode"]) + "','" + txtChallanNumber.Text.Trim() + "','DCOUT','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                            }
                            if (result)
                            {
                                if (((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text == "-2147483642")
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=I_DISPATCH_BAL-" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }
                                else
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }
                            }
                        }
                        CommonClasses.RemoveModifyLock("DELIVERY_CHALLAN_MASTER", "MODIFY", "DCM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("Delivery Challan", "Update", "Delivery Challan", txtChallanNumber.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewDeliveryChallan.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtChallanNumber.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Delivery Challan", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "" || Convert.ToDouble(txtOrderQty.Text) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Qty.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Focus();
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");

            #region Check Duplicate Inserted_Grid
            if (dgMainDC.Enabled)
            {
                for (int i = 0; i < dgMainDC.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgMainDC.Rows[i].FindControl("lblItemCode"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            ddlCustomer.Focus();
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
                            ddlCustomer.Focus();
                            return;
                        }
                    }
                }
            }
            #endregion Check Duplicate Inserted_Grid

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCodeNo");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit1");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit");
                ((DataTable)ViewState["dt2"]).Columns.Add("Stock");
                ((DataTable)ViewState["dt2"]).Columns.Add("StoreCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("StoreName");
                ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("Remark");
                ((DataTable)ViewState["dt2"]).Columns.Add("NoOfPacks");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemName.SelectedValue;
            dr["ItemCodeNo"] = ddlItemCode.SelectedItem;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["Unit1"] = ddlUnit.SelectedValue;
            dr["Unit"] = ddlUnit.SelectedItem;
            dr["StoreCode"] = ddlStore.SelectedValue;
            dr["StoreName"] = ddlStore.SelectedItem;
            dr["OrderQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtOrderQty.Text)), 3));
            dr["Stock"] = txtStock.Text;
            dr["Remark"] = txtRemark.Text;
            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
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

            #region Binding data to Grid
            dgMainDC.Visible = true;
            dgMainDC.DataSource = ((DataTable)ViewState["dt2"]);
            dgMainDC.DataBind();
            dgMainDC.Enabled = true;
            #endregion

            //To avoid same item insert in Grid
            ViewState["ItemUpdateIndex"] = "-1";
            clearDetail();
            ddlItemCode.Focus();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "btnInsert_Click", Ex.Message);
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
            CommonClasses.SendError("Delivery Challan", "ShowMessage", Ex.Message);
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
            txtOrderQty.Text = "0.000";
            txtRemark.Text = "";
            txtStock.Text = "";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Delivery Challan", "clearDetail", Ex.Message);
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
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,ISNULL(I_CURRENT_BAL,0) AS I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");

                if (dt1.Rows.Count > 0)
                {
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    ddlStore_SelectedIndexChanged(null, null);
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                txtOrderQty.Text = "0";
                txtOrderQty.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "ddlItemCode_SelectedIndexChanged", Ex.Message);
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
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,ISNULL(I_CURRENT_BAL,0) AS I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");

                if (dt1.Rows.Count > 0)
                {
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    ddlStore_SelectedIndexChanged(null, null);
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                txtOrderQty.Text = "0";
                txtOrderQty.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Delivery Challan", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlStore_SelectedIndexChanged
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlStore.SelectedIndex != 0 && ddlItemCode.SelectedIndex != 0)
            {
                DataTable dt1 = CommonClasses.Execute(" SELECT ISNULL(SUM(STL_DOC_QTY),0) AS STL_DOC_QTY  FROM STOCK_LEDGER where STL_STORE_TYPE= '" + ddlStore.SelectedValue + "' AND STL_I_CODE='" + ddlItemCode.SelectedValue + "'");
                if (dt1.Rows.Count > 0)
                {
                    txtStock.Text = dt1.Rows[0]["STL_DOC_QTY"].ToString();
                    txtOrderQty.Text = "0";
                }
                else
                {
                    txtStock.Text = "0";
                    txtOrderQty.Text = "0";
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Delivery Challan", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region CheckNo
    public void CheckNo(TextBox t)
    {
        if (t.Text != "")
        {
            double num;
            bool b = Double.TryParse(t.Text, out num);
            if (b != true)
            {
                ShowMessage("#Avisos", "Enter Valid Number!!!", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                t.Text = "0.00";
                t.Focus();
            }
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgMainDC.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemCodeNo", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Unit1", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Stock", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("StoreCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("StoreName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("OrderQty", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Remark", typeof(String)));
                dtFilter.Rows.Add(dtFilter.NewRow());
                dgMainDC.DataSource = dtFilter;
                dgMainDC.DataBind();
            }
        }
    }
    #endregion

    #region dgMainDC_Deleting
    protected void dgMainDC_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMainDC_RowCommand
    protected void dgMainDC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgMainDC.Rows[Convert.ToInt32(ViewState["Index"])];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
                {
                    string itemCode = ((Label)(row.FindControl("lblItemCode"))).Text;
                }
                dgMainDC.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);

                dgMainDC.DataSource = ((DataTable)ViewState["dt2"]);
                dgMainDC.DataBind();
                if (dgMainDC.Rows.Count == 0)
                {
                    dgMainDC.Enabled = false;
                    LoadFilter();
                }
                else
                {
                }
            }
            if (e.CommandName == "Select")
            {
                str = "Modify";
                LoadUnit();
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlStore.SelectedValue = ((Label)(row.FindControl("lblStoreCode"))).Text;
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtOrderQty.Text = ((Label)(row.FindControl("lblOrderQty"))).Text;
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblUnit1"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblRemark"))).Text;
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    txtStock.Text = (Convert.ToDouble(txtStock.Text) + Convert.ToDouble(txtOrderQty.Text)).ToString();
                }
                foreach (GridViewRow gvr in dgMainDC.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "dgMainDC_RowCommand", Ex.Message);
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

    #region txtOrderQty_TextChanged
    protected void txtOrderQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0.00";
            }
            if (txtStock.Text == "")
            {
                txtStock.Text = "0.00";
            }
            if (Convert.ToDouble(txtStock.Text) < Convert.ToDouble(txtOrderQty.Text))
            {
                ShowMessage("#Avisos", "Quantity Should Not greater than Stock", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtOrderQty.Text = "0.00";
                txtOrderQty.Focus();
                return;
            }
            string totalStr = DecimalMasking(txtOrderQty.Text);
            txtOrderQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Inventory Customer Order Transaction", "txtOrderQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region LoadStore
    private void LoadStore()
    {
        try
        {
            dt = CommonClasses.Execute("  SELECT STORE_CODE,STORE_NAME  FROM STORE_MASTER where ES_DELETE=0   and STORE_COMP_ID=" + (string)Session["CompanyId"] + "      ORDER BY STORE_NAME ");
            ddlStore.DataSource = dt;
            ddlStore.DataTextField = "STORE_NAME";
            ddlStore.DataValueField = "STORE_CODE";
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new ListItem("Select Store", "0"));
            ddlStore.SelectedValue = "-2147483647";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan", "LoadIName", Ex.Message);
        }
    }
    #endregion
}
