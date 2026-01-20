using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Transactions_ADD_DCReturn : System.Web.UI.Page
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
                    try
                    {
                        ViewState["mlCode"] = mlCode;
                        ViewState["dt2"] = dt2;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        LoadCustomer();
                        LoadUnit();
                        LoadStore();
                        LoadChallan("DCIN");
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
                            txtPchallanDate.Attributes.Add("readonly", "readonly");
                            txtPchallanDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            dgMainDC.Enabled = false;
                        }
                        txtOrderNo.Focus();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("DC Return", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("DC Return", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load
    #region LoadStore
    private void LoadStore()
    {
        CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "    ORDER BY STORE_NAME", ddlStore);
        ddlStore.Items.Insert(0, new ListItem("Select Store Name", "0"));
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomer.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;
            }

            if (txtPartyChNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Party Challan No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPartyChNo.Focus();
                return;
            }
            if (txtPchallanDate.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Party Challan Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPchallanDate.Focus();
                return;
            }

            if (dgMainDC.Enabled && dgMainDC.Rows.Count > 0)
            {
                SaveRec();

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Exist In Table";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("DC Return", "CheckValid", Ex.Message);
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
            CommonClasses.SendError("DC Return", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("DC_RETURN_MASTER", "MODIFY", "DNM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewDCReturn.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "CancelRecord", Ex.Message);
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
            CommonClasses.SendError("DC Return", "btnCancel_Click", ex.Message.ToString());
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

            dt = CommonClasses.Execute("Select DNM_CODE,DNM_P_CODE,DNM_NO,DNM_CM_CODE,DNM_DATE,DNM_PARTY_DC_NO, DNM_PARTY_DC_DATE, DNM_OUT_DC_NO, DNM_OUT_DC_DATE, ES_DELETE, MODIFY, DNM_MAT_TYPE, DNM_TYPE from DC_RETURN_MASTER where ES_DELETE=0 and DNM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and DNM_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + "");
            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["DNM_CODE"]); ;
                ddlCustomer.SelectedValue = dt.Rows[0]["DNM_P_CODE"].ToString();
                txtChallanNumber.Text = dt.Rows[0]["DNM_NO"].ToString();
                txtChallanDate.Text = Convert.ToDateTime(dt.Rows[0]["DNM_DATE"]).ToString("dd MMM yyyy");
                ddlType.SelectedValue = dt.Rows[0]["DNM_TYPE"].ToString();
                //challan no date
                LoadChallan(dt.Rows[0]["DNM_TYPE"].ToString());
                txtOrderDate.Text = Convert.ToDateTime(dt.Rows[0]["DNM_PARTY_DC_DATE"]).ToString("dd MMM yyyy");
                ddlchallanNo.SelectedValue = dt.Rows[0]["DNM_PARTY_DC_NO"].ToString();

                txtPartyChNo.Text = dt.Rows[0]["DNM_OUT_DC_NO"].ToString();
                txtPchallanDate.Text = Convert.ToDateTime(dt.Rows[0]["DNM_OUT_DC_DATE"]).ToString("dd MMM yyyy");
                ddlchallanNo_SelectedIndexChanged(null, null);
                dtInwardDetail = CommonClasses.Execute("select  DND_I_CODE as ItemCode,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as Unit1,I_UOM_NAME as Unit,cast(DND_REC_QTY as numeric(10,3))  as OrderQty,DND_IREMARK AS Remark,0 as Stock, cast(DND_SCRAP_QTY  as numeric(10,3))AS Scrap ,DND_STORE_CODE AS StoreCode , STORE_NAME AS StoreName  From ITEM_UNIT_MASTER,DC_RETURN_DETAIL,ITEM_MASTER,STORE_MASTER where DND_STORE_CODE=STORE_CODE AND DND_I_CODE=I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and DND_DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' order by DND_I_CODE");
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
                CommonClasses.SetModifyLock("DC_RETURN_MASTER", "MODIFY", "DNM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                ddlType.Enabled = false;
                ddlchallanNo.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "ViewRec", Ex.Message);
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
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0   and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_ACTIVE_IND=1 order by P_NAME--and P_TYPE='1'");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "LoadCustomer", Ex.Message);
        }
    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE IN ('-2147483633') ORDER BY I_CODENO");
            ddlItemCode.DataSource = dt;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "LoadICode", Ex.Message);
        }
    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_NAME from ITEM_MASTER where ES_DELETE=0 and I_CM_COMP_ID=" + (string)Session["CompanyId"] + " and I_CAT_CODE IN ('-2147483633') ORDER BY I_NAME");
            ddlItemName.DataSource = dt;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "LoadIName", Ex.Message);
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

    #region LoadChallan
    private void LoadChallan(string type)
    {
        DataTable dt1 = new DataTable();
        if (type == "DCTIN")
        {
            dt1 = CommonClasses.Execute(" SELECT DCM_CODE,DCM_NO  FROM DELIVERY_CHALLAN_MASTER where  DELIVERY_CHALLAN_MASTER.ES_DELETE=0 AND DCM_TYPE='DLCT'    AND DCM_IS_RETURNABLE=1 ORDER BY DCM_CODE DESC");
        }
        else
        {
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dt1 = CommonClasses.Execute("  SELECT DISTINCT  DCM_CODE,DCM_NO  FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL  where DCM_CODE=DCD_DCM_CODE    AND  DELIVERY_CHALLAN_MASTER.ES_DELETE=0  AND DCM_TYPE='DLC' AND DCM_IS_RETURNABLE=1   AND (DCD_ORD_QTY-ISNULL(DCD_RET_QTY,0)) >0 ORDER BY DCM_CODE DESC");
            }
            else
            {
                dt1 = CommonClasses.Execute(" SELECT DCM_CODE,DCM_NO  FROM DELIVERY_CHALLAN_MASTER where  DELIVERY_CHALLAN_MASTER.ES_DELETE=0  AND DCM_TYPE='DLC'   AND DCM_IS_RETURNABLE=1  ORDER BY DCM_CODE DESC");
            }
        }
        ddlchallanNo.DataSource = dt1;
        ddlchallanNo.DataTextField = "DCM_NO";
        ddlchallanNo.DataValueField = "DCM_CODE";
        ddlchallanNo.DataBind();
        ddlchallanNo.Items.Insert(0, new ListItem("Select Challan No", "0"));
        ddlchallanNo.SelectedIndex = -1;
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
                dt = CommonClasses.Execute("Select isnull(max(DNM_NO),0) as DNM_NO FROM DC_RETURN_MASTER WHERE DNM_CM_CODE = " + Convert.ToInt32(Session["CompanyCode"]) + " AND  DNM_TYPE='DCIN' AND  ES_DELETE=0");
                if (dt.Rows.Count > 0)
                {
                    Po_Doc_no = Convert.ToInt32(dt.Rows[0]["DNM_NO"]);
                    Po_Doc_no = Po_Doc_no + 1;
                }
                if (CommonClasses.Execute1("INSERT INTO DC_RETURN_MASTER (DNM_CM_CODE,DNM_P_CODE,DNM_NO,DNM_DATE,DNM_PARTY_DC_NO,DNM_PARTY_DC_DATE,DNM_OUT_DC_NO,DNM_OUT_DC_DATE,DNM_TYPE)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlCustomer.SelectedValue + "','" + Po_Doc_no + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlchallanNo.SelectedValue + "','" + Convert.ToDateTime(txtOrderDate.Text).ToString("dd/MMM/yyyy") + "','" + txtPartyChNo.Text + "','" + Convert.ToDateTime(txtPchallanDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlType.SelectedValue + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(DNM_CODE) from DC_RETURN_MASTER");
                    for (int i = 0; i < dgMainDC.Rows.Count; i++)
                    {
                        result = CommonClasses.Execute1("INSERT INTO DC_RETURN_DETAIL (DND_DNM_CODE,DND_I_CODE,DND_REC_QTY,DND_IREMARK,DND_SCRAP_QTY,DND_STORE_CODE) values ('" + Code + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblRemark")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblScrap")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");

                        if (result)
                        {
                            result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + Code + "','" + Po_Doc_no + "','" + ddlType.SelectedValue + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "',  '" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                            result = CommonClasses.Execute1(" UPDATE  DELIVERY_CHALLAN_DETAIL SET DCD_RET_QTY=ISNULL(DCD_RET_QTY,0)+'" + (Convert.ToDouble(((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text) + Convert.ToDouble(((Label)dgMainDC.Rows[i].FindControl("lblScrap")).Text)) + "' where DCD_DCM_CODE='" + ddlchallanNo.SelectedValue + "' AND DCD_I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                        }
                        if (result)
                        {
                            if (((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text.ToString() == "-2147483642")
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=I_DISPATCH_BAL+" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }
                            else
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                            }
                        }
                    }
                    //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");
                    CommonClasses.WriteLog("DC Return", "Save", "DC Return", Convert.ToString(Po_Doc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewDCReturn.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    // ShowMessage("#Avisos", "", CommonClasses.MSG_Warning);
                    txtChallanNumber.Focus();
                }
            }
            #endregion INSERT

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE DC_RETURN_MASTER SET DNM_P_CODE='" + ddlCustomer.SelectedValue + "',DNM_NO='" + txtChallanNumber.Text + "', DNM_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "',DNM_PARTY_DC_NO='" + ddlchallanNo.SelectedValue + "',DNM_PARTY_DC_DATE='" + Convert.ToDateTime(txtOrderDate.Text).ToString("dd/MMM/yyyy") + "',DNM_OUT_DC_NO='" + txtPartyChNo.Text + "',DNM_OUT_DC_DATE='" + Convert.ToDateTime(txtOrderDate.Text).ToString("dd/MMM/yyyy") + "'  where DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    DataTable dtq = CommonClasses.Execute("SELECT DND_REC_QTY ,DND_STORE_CODE ,DND_SCRAP_QTY,DND_REC_QTY+DND_SCRAP_QTY AS TOTAL_QTY ,DND_I_CODE FROM DC_RETURN_DETAIL where DND_DNM_CODE=" + Convert.ToInt32(ViewState["mlCode"]) + " ");
                    for (int i = 0; i < dtq.Rows.Count; i++)
                    {
                        if (dtq.Rows[i]["DND_STORE_CODE"].ToString() == "-2147483642")
                        {
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=I_DISPATCH_BAL-" + dtq.Rows[i]["DND_REC_QTY"] + " where I_CODE='" + dtq.Rows[i]["DND_I_CODE"] + "'");
                        }
                        else
                        {
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtq.Rows[i]["DND_REC_QTY"] + " where I_CODE='" + dtq.Rows[i]["DND_I_CODE"] + "'");
                        }
                        result = CommonClasses.Execute1(" UPDATE  DELIVERY_CHALLAN_DETAIL SET DCD_RET_QTY=ISNULL(DCD_RET_QTY,0)-" + dtq.Rows[i]["TOTAL_QTY"] + " where DCD_DCM_CODE='" + ddlchallanNo.SelectedValue + "' AND DCD_I_CODE='" + dtq.Rows[i]["DND_I_CODE"].ToString() + "'");
                    }
                    result = CommonClasses.Execute1("DELETE FROM DC_RETURN_DETAIL WHERE DND_DNM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"]) + "' and STL_DOC_TYPE='" + ddlType.SelectedValue + "'");
                    if (result)
                    {
                        for (int i = 0; i < dgMainDC.Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO DC_RETURN_DETAIL (DND_DNM_CODE,DND_I_CODE,DND_REC_QTY,DND_IREMARK,DND_SCRAP_QTY,DND_STORE_CODE) values ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblRemark")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblScrap")).Text + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                            if (result)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "','" + Convert.ToInt32(ViewState["mlCode"]) + "','" + txtChallanNumber.Text + "','" + ddlType.SelectedValue + "','" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "','" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + "' ,'" + ((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text + "')");
                                result = CommonClasses.Execute1(" UPDATE  DELIVERY_CHALLAN_DETAIL SET DCD_RET_QTY=ISNULL(DCD_RET_QTY,0)+" + (Convert.ToDouble(((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text) + Convert.ToDouble(((Label)dgMainDC.Rows[i].FindControl("lblScrap")).Text)) + " where DCD_DCM_CODE='" + ddlchallanNo.SelectedValue + "' AND DCD_I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
 
                            }
                            if (result)
                            {
                                if (((Label)dgMainDC.Rows[i].FindControl("lblStoreCode")).Text.ToString() == "-2147483642")
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_DISPATCH_BAL=I_DISPATCH_BAL+" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }
                                else
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgMainDC.Rows[i].FindControl("lblOrderQty")).Text + ",I_RECEIPT_DATE='" + Convert.ToDateTime(txtChallanDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((Label)dgMainDC.Rows[i].FindControl("lblItemCode")).Text + "'");
                                }
                            }
                        }
                        //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");
                        CommonClasses.RemoveModifyLock("DC_RETURN_MASTER", "MODIFY", "DNM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                        CommonClasses.WriteLog("DC Return", "Update", "DC Return", txtChallanNumber.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }
                    Response.Redirect("~/Transactions/VIEW/ViewDCReturn.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    // ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtChallanNumber.Focus();
                }
            }
            #endregion MODIFY
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("DC Return", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlStore.SelectedValue.ToString() == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Store";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlStore.Focus();
                return;
            }

            DataTable dtItem = new DataTable();
            dtItem = CommonClasses.Execute("SELECT * FROM ITEM_MASTER where I_CODE='" + ddlItemCode.SelectedValue + "'");
            if (dtItem.Rows.Count > 0)
            {
                if (dtItem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483648" && ddlStore.SelectedValue.ToString() == "-2147483647")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "FG Item Can not Move to Main Store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStore.Focus();
                    return;
                }

            }

            challandetail();
            if (txtOrderQty.Text == "" || txtOrderQty.Text == "0.00")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Quantity Not be zero";
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
            #endregion Check Duplicate Inserted_GridCheck Duplicate Inserted_Grid

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("ItemName");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit1");
                ((DataTable)ViewState["dt2"]).Columns.Add("Unit");
                ((DataTable)ViewState["dt2"]).Columns.Add("Stock");
                ((DataTable)ViewState["dt2"]).Columns.Add("OrderQty");
                ((DataTable)ViewState["dt2"]).Columns.Add("Remark");
                ((DataTable)ViewState["dt2"]).Columns.Add("Scrap");
                ((DataTable)ViewState["dt2"]).Columns.Add("NoOfPacks");
                ((DataTable)ViewState["dt2"]).Columns.Add("StoreCode");
                ((DataTable)ViewState["dt2"]).Columns.Add("StoreName");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["ItemCode"] = ddlItemName.SelectedValue;
            dr["ItemName"] = ddlItemName.SelectedItem;
            dr["Unit1"] = ddlUnit.SelectedValue;
            dr["Unit"] = ddlUnit.SelectedItem;
            dr["OrderQty"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtOrderQty.Text)), 3));
            dr["Stock"] = txtStock.Text;
            dr["Remark"] = txtRemark.Text;
            dr["Scrap"] = txtScrapQty.Text;
            dr["StoreCode"] = ddlStore.SelectedValue;
            dr["StoreName"] = ddlStore.SelectedItem;
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
            CommonClasses.SendError("DC Return", "btnInsert_Click", Ex.Message);
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
            CommonClasses.SendError("DC Return", "ShowMessage", Ex.Message);
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
            ddlStore.SelectedIndex = 0;
            txtScrapQty.Text = "0.000";
            txtOrderQty.Text = "0.000";
            txtRemark.Text = "";
            txtStock.Text = "";
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" DC Return", "clearDetail", Ex.Message);
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
                DataTable dt1 = CommonClasses.Execute(" SELECT DCM_CODE,DCM_NO,DCM_P_CODE,DCM_DATE,SUM(DCD_ORD_QTY-ISNULL(DCD_RET_QTY,0)) AS DEC_QTY,DCD_I_CODE,I_UOM_CODE   FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER    where  DCM_CODE=DCD_DCM_CODE AND  DCD_I_CODE=I_CODE AND   DELIVERY_CHALLAN_MASTER.ES_DELETE=0 AND DCM_IS_RETURNABLE=1 AND  DCM_CODE='" + ddlchallanNo.SelectedValue + "'  and DCD_I_CODE='" + ddlItemCode.SelectedValue + "' GROUP BY  DCM_CODE,DCM_NO,DCM_P_CODE,DCM_DATE,DCD_I_CODE,I_UOM_CODE");

                if (dt1.Rows.Count > 0)
                {
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtStock.Text = dt1.Rows[0]["DEC_QTY"].ToString();
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                txtOrderQty.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "ddlItemCode_SelectedIndexChanged", Ex.Message);

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
                DataTable dt1 = CommonClasses.Execute(" SELECT DCM_CODE,DCM_NO,DCM_P_CODE,DCM_DATE,SUM(DCD_ORD_QTY-ISNULL(DCD_RET_QTY,0)) AS DEC_QTY,DCD_I_CODE,I_UOM_CODE   FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER    where  DCM_CODE=DCD_DCM_CODE AND  DCD_I_CODE=I_CODE AND   DELIVERY_CHALLAN_MASTER.ES_DELETE=0 AND DCM_IS_RETURNABLE=1 AND  DCM_CODE='" + ddlchallanNo.SelectedValue + "'  and DCD_I_CODE='" + ddlItemCode.SelectedValue + "' GROUP BY  DCM_CODE,DCM_NO,DCM_P_CODE,DCM_DATE,DCD_I_CODE,I_UOM_CODE");

                if (dt1.Rows.Count > 0)
                {
                    ddlUnit.SelectedValue = dt1.Rows[0]["I_UOM_CODE"].ToString();
                    txtStock.Text = dt1.Rows[0]["DEC_QTY"].ToString();
                }
                else
                {
                    ddlUnit.SelectedIndex = 0;
                }
                txtOrderQty.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" DC Return", "ddlItemName_SelectedIndexChanged", Ex.Message);
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
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Valid Number!!!";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                t.Text = "0.00";
                t.Focus();
            }
        }
    }
    #endregion

    #region ddlchallanNo_SelectedIndexChanged
    protected void ddlchallanNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dt1 = CommonClasses.Execute("  SELECT DCM_CODE,DCM_NO,DCM_P_CODE,P_NAME ,DCM_DATE FROM DELIVERY_CHALLAN_MASTER,PARTY_MASTER   where DCM_P_CODE=P_CODE AND   DELIVERY_CHALLAN_MASTER.ES_DELETE=0 AND DCM_IS_RETURNABLE=1  AND  DCM_CODE='" + ddlchallanNo.SelectedValue + "'");

            ddlCustomer.DataSource = dt1;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "DCM_P_CODE";
            ddlCustomer.DataBind();

            txtOrderDate.Text = Convert.ToDateTime(dt1.Rows[0]["DCM_DATE"].ToString()).ToString("dd/MMM/yyyy");

            DataTable dtItems = new DataTable();
            if (ddlType.SelectedValue == "DCIN")
            {
                dtItems = CommonClasses.Execute(" SELECT DCM_CODE,DCM_NO,DCM_P_CODE,P_NAME ,DCM_DATE,I_CODE,I_CODENO,I_NAME FROM DELIVERY_CHALLAN_MASTER,PARTY_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER where DCM_P_CODE=P_CODE AND DCM_CODE=DCD_DCM_CODE AND DCD_I_CODE=I_CODE AND DELIVERY_CHALLAN_MASTER.ES_DELETE=0 AND DCM_IS_RETURNABLE=1 AND DCM_TYPE='DLC' AND   DCM_CODE='" + ddlchallanNo.SelectedValue + "'");

            }
            else
            {
                dtItems = CommonClasses.Execute(" SELECT DCM_CODE,DCM_NO,DCM_P_CODE,P_NAME ,DCM_DATE,I_CODE,I_CODENO,I_NAME FROM DELIVERY_CHALLAN_MASTER,PARTY_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER where DCM_P_CODE=P_CODE AND DCM_CODE=DCD_DCM_CODE AND DCD_I_CODE=I_CODE AND DELIVERY_CHALLAN_MASTER.ES_DELETE=0 AND DCM_IS_RETURNABLE=1 AND DCM_TYPE='DLCT' AND   DCM_CODE='" + ddlchallanNo.SelectedValue + "'");

            }
            ddlItemCode.DataSource = dtItems;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;

            ddlItemName.DataSource = dtItems;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        challandetail();
    }
    #endregion

    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadChallan(ddlType.SelectedValue.ToString());
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
                dtFilter.Columns.Add(new System.Data.DataColumn("ItemName", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Unit", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Unit1", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Stock", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("OrderQty", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Remark", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("Scrap", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("StoreCode", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("StoreName", typeof(String)));
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
                LoadStore();
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblItemCode"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtOrderQty.Text = ((Label)(row.FindControl("lblOrderQty"))).Text;
                ddlUnit.SelectedValue = ((Label)(row.FindControl("lblUnit1"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblRemark"))).Text;
                txtScrapQty.Text = ((Label)(row.FindControl("lblScrap"))).Text;
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    txtStock.Text = (Convert.ToDouble(txtStock.Text) + Convert.ToDouble(((Label)(row.FindControl("lblOrderQty"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblScrap"))).Text)).ToString();
                }
                else
                {
                    txtStock.Text = (Convert.ToDouble(txtStock.Text) + Convert.ToDouble(((Label)(row.FindControl("lblOrderQty"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblScrap"))).Text)).ToString();
                }
                ddlStore.SelectedValue = ((Label)(row.FindControl("lblStoreCode"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("DC Return", "dgMainDC_RowCommand", Ex.Message);
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
            if (txtScrapQty.Text == "")
            {
                txtScrapQty.Text = "0.00";
            }
            if (Convert.ToDouble(txtStock.Text) < (Convert.ToDouble(txtScrapQty.Text) + Convert.ToDouble(txtOrderQty.Text)))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Quantity Should Not greater than Stock";
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

    #region txtScrapQty_TextChanged
    protected void txtScrapQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOrderQty.Text == "")
            {
                txtOrderQty.Text = "0.00";
            }
            if (txtScrapQty.Text == "")
            {
                txtScrapQty.Text = "0.00";
            }

            if (Convert.ToDouble(txtStock.Text) < (Convert.ToDouble(txtScrapQty.Text) + Convert.ToDouble(txtOrderQty.Text)))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Quantity Should Not greater than Stock";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtScrapQty.Text = "0.00";
                txtScrapQty.Focus();
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

    #region challandetail
    public void challandetail()
    {
        if (ddlCustomer.SelectedValue != "0" && txtPartyChNo.Text != "" && txtPartyChNo.Text != "0" && txtChallanDate.Text != "")
        {
            DataTable dtChallan = new DataTable();
            if (Request.QueryString[0].Equals("INSERT"))
            {
                dtChallan = CommonClasses.Execute("select * from DC_RETURN_MASTER where DNM_OUT_DC_NO='" + txtPartyChNo.Text.Trim() + "' and DNM_P_CODE='" + ddlCustomer.SelectedValue + "' and DNM_PARTY_DC_NO='" + ddlchallanNo.SelectedValue + "' and ES_DELETE=0 and DNM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and DNM_TYPE='DCIN'");
            }
            else
            {
                dtChallan = CommonClasses.Execute("SELECT * FROM DC_RETURN_MASTER where DNM_OUT_DC_NO='" + txtPartyChNo.Text.Trim() + "' AND DNM_P_CODE='" + ddlCustomer.SelectedValue + "' and DNM_PARTY_DC_NO='" + ddlchallanNo.SelectedValue + "' and ES_DELETE=0 and DNM_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and DNM_TYPE='DCIN' AND DNM_CODE!='" + ViewState["mlCode"].ToString() + "'");
            }
            if (dtChallan.Rows.Count > 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Challan No. Is Exist for this party";
                txtPartyChNo.Text = "";
                txtPartyChNo.Focus();
            }
            else
            {
                PanelMsg.Visible = false;
                lblmsg.Text = "";
            }
        }
    }
    #endregion

    #region txtPartyChNo_TextChanged
    protected void txtPartyChNo_TextChanged(object sender, EventArgs e)
    {
        challandetail();
    }
    #endregion
}
