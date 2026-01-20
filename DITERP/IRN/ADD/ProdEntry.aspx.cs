using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_ADD_ProdEntry : System.Web.UI.Page
{
    #region General Declaration
    UnitMaster_BL BL_UnitMaster = null;
    static int mlCode = 0;
    static string right = "";
    DataRow dr;
    public static string str = "", str1 = "";
    public static int Index = 0;
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    static DataTable dt3 = new DataTable();
    static DataTable dtReason = new DataTable();
    #endregion

    #region Events
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
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
                    ViewState["dtReason"] = dtReason;
                    ((DataTable)ViewState["dtReason"]).Rows.Clear();
                    ViewState["GTotal"] = null;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    str = "";
                    ViewState["str"] = str;
                    BlankGrid();
                    txtGRNDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                    txtGRNDate.Attributes.Add("readonly", "readonly");

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
                    loadStage();
                    loadItem();
                    loadDefect();
                    loadunit();
                    ddlStage.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("FOUNDRY IRN ENTRY", "PageLoad", ex.Message);
                }
            }
        }
    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (((Label)dgIRN.Rows[0].FindControl("lblIRND_I_CODE")).Text != "")
            {
                SaveRec();
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Fill Table";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "btnSubmit_Click", Ex.Message);
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
                    pnlShortReason.Visible = true;
                    ModalCancleConfirmation.Show();
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
            flag = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "CheckValid", Ex.Message);
        }

        return flag;
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CancelRecord();
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
    }
    private void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/IRN/VIEW/ViewProdEntry.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT IRN_CODE, IRN_NO, IRN_CM_ID, IRN_DATE,IRN_PLANT FROM IRN_ENTRY WHERE (ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dt.Rows.Count > 0)
            {
                txtGRNno.Text = dt.Rows[0]["IRN_NO"].ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IRN_DATE"].ToString()).ToString("dd/MMM/yyyy");
                rbtWithAmt.SelectedValue = dt.Rows[0]["IRN_PLANT"].ToString();
            }
            DataTable dtdetails = new DataTable();
            dtdetails = CommonClasses.Execute("SELECT IRN_DETAIL.IRND_RSM_CODE, CASE WHEN IRND_TYPE=1 then 'CASTING' ELSE 'MECHINING' END AS IRND_TYPE, IRN_DETAIL.IRND_I_CODE, IRN_DETAIL.IRND_UOM, IRN_DETAIL.IRND_RM_CODE,IRN_DETAIL.IRND_PROD_QTY,IRN_DETAIL.IRND_REJ_QTY,IRN_DETAIL.IRND_RATE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,ITEM_UNIT_MASTER.I_UOM_NAME, '' AS RSM_NO,'' AS RM_DEFECT, '' AS RSM_NAME,isnull(IRND_STANDARD_PROD,0) as IRND_STANDARD_PROD,isnull(IRND_SHORT_PROD,0) as IRND_SHORT_PROD,IRN_DETAIL.IRND_T_CODE FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON IRN_DETAIL.IRND_I_CODE = ITEM_MASTER.I_CODE  WHERE     (IRN_ENTRY.ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dtdetails.Rows.Count > 0)
            {
                dgIRN.Enabled = true;
                ViewState["dt2"] = dtdetails;
                dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
                dgIRN.DataBind();
            }
            if (str == "MOD")
            {
                rbtWithAmt.Enabled = false;
                txtGRNDate.Enabled = false;
                CommonClasses.SetModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            else
            {
                rbtWithAmt.Enabled = false;
                txtGRNno.Enabled = false;
                txtGRNDate.Enabled = false;
                ddlDefect.Enabled = false;
                ddlStage.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlUOM.Enabled = false;
                txtRate.Enabled = false;
                BtnInsert.Enabled = false;
                txtProdQty.Enabled = false;
                txtRejQTy.Enabled = false;
                btnSubmit.Visible = false;
                dgIRN.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "ViewRec", Ex.Message);
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
                btnSubmit.Visible = false;
            }
            else if (str == "MOD")
            {
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "GetValues", ex.Message);
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
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "Setvalues", ex.Message);
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
            string strType = "";

            int storeCode = -2147483637;
            if (rbtWithAmt.SelectedValue == "-2147483647")
            {
                storeCode = -2147483646;
            }

            if (Request.QueryString[0].Equals("INSERT"))
            {
                txtGRNno.Text = CommonClasses.GetMaxNO("SELECT MAX(IRN_NO)+1 FROM IRN_ENTRY WHERE  IRN_TYPE=0 AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 ");
                if (CommonClasses.Execute1("INSERT INTO IRN_ENTRY (IRN_NO,IRN_DATE,IRN_CM_ID,IRN_TRANS_TYPE)VALUES ('" + txtGRNno.Text.Trim() + "','" + txtGRNDate.Text + "','" + Convert.ToInt32(Session["CompanyId"]) + "',1) "))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(IRN_CODE) from IRN_ENTRY");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        bool type = false;
                        if (((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text.ToUpper() == "CASTING")
                        {
                            type = true;
                        }


                        //change suja add two fields for stand. prod. and Short Prod.
                        CommonClasses.Execute1("INSERT INTO IRN_DETAIL (IRND_IRN_CODE,IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE,IRND_AMT,IRND_STANDARD_PROD,IRND_SHORT_PROD,IRND_T_CODE) VALUES ('" + Code + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + type + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "','" + Math.Round(Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text) * Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text), 2).ToString() + "','" + ((Label)dgIRN.Rows[i].FindControl("lblII_STANDARD_PRODUCTION")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblISHORT_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_T_CODE")).Text + "') ");

                        // Stock Ledger  
                        if (type == true)
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE	,STL_DOC_NO	,STL_DOC_NUMBER	,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY ,STL_STORE_TYPE) VALUES('" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + Code + "','" + txtGRNno.Text.Trim() + "','Production To Foundary','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + storeCode + " ')");
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "  WHERE I_CODE='" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "'");
                        }
                    }

                    //change suja Short Production Entry Reason insert into Datatable
                    for (int i = 0; i < ((DataTable)(ViewState["dtReason"])).Rows.Count; i++)
                    {
                        ((DataTable)(ViewState["dtReason"])).Rows[i]["IRN_CODE"] = Code;
                    }

                    CommonClasses.BulkInsertDataTable("IRN_SPDETAIL", ((DataTable)(ViewState["dtReason"])));
                    CommonClasses.WriteLog("PROD ENTRY", "Save", "PROD ENTRY", txtGRNno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewProdEntry.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtGRNno.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE IRN_ENTRY SET IRN_NO='" + txtGRNno.Text.Trim() + "',IRN_DATE='" + txtGRNDate.Text.Trim() + "'     where IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    DataTable dtDetail = CommonClasses.Execute("SELECT * FROM IRN_DETAIL where IRND_IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' AND IRND_TYPE=1");
                    if (dtDetail.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDetail.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND STL_I_CODE='" + dtDetail.Rows[i]["IRND_I_CODE"].ToString() + "' and STL_DOC_TYPE='Production To Foundary'");
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtDetail.Rows[i]["IRND_PROD_QTY"].ToString() + "  WHERE I_CODE='" + dtDetail.Rows[i]["IRND_I_CODE"].ToString() + "'");
                        }
                    }
                    CommonClasses.Execute1("DELETE FROM IRN_DETAIL where IRND_IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    //Delete old record if exist
                    CommonClasses.Execute1("DELETE FROM IRN_SPDETAIL where IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");

                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        bool type = false;
                        if (((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text.ToUpper() == "CASTING")
                        {
                            type = true;
                        }
                        //change suja add two fields for stand. prod. and Short Prod.
                        CommonClasses.Execute1("INSERT INTO IRN_DETAIL (IRND_IRN_CODE, IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE,IRND_AMT,IRND_STANDARD_PROD,IRND_SHORT_PROD,IRND_T_CODE) VALUES ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + type + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "','" + Math.Round(Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text) * Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text), 2).ToString() + "','" + ((Label)dgIRN.Rows[i].FindControl("lblII_STANDARD_PRODUCTION")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblISHORT_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_T_CODE")).Text + "') ");

                        if (type == true)
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE	,STL_DOC_NO	,STL_DOC_NUMBER	,STL_DOC_TYPE	,STL_DOC_DATE	,STL_DOC_QTY ,STL_STORE_TYPE) VALUES('" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + Convert.ToInt32(ViewState["mlCode"]) + "','" + txtGRNno.Text.Trim() + "','Production To Foundary','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + storeCode + "')");
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "  WHERE I_CODE='" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "'");
                        }
                    }

                    for (int i = 0; i < ((DataTable)(ViewState["dtReason"])).Rows.Count; i++)
                    {
                        ((DataTable)(ViewState["dtReason"])).Rows[i]["IRN_CODE"] = Convert.ToInt32(ViewState["mlCode"]);
                    }

                    CommonClasses.BulkInsertDataTable("IRN_SPDETAIL", ((DataTable)(ViewState["dtReason"])));

                    CommonClasses.RemoveModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("PROD ENTRY", "Update", "PROD ENTRY", txtGRNno.Text.Trim(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewProdEntry.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtGRNno.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "SaveRec", ex.Message);
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
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region LoadInsertData
    public void LoadInsertData()
    {
        PanelMsg.Visible = false;
        if (dgIRN.Rows.Count > 0)
        {
            for (int i = 0; i < dgIRN.Rows.Count; i++)
            {
                string IRND_I_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_I_CODE"))).Text;
                string IRND_TYPE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_TYPE"))).Text;
                string IRND_RM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_RM_CODE"))).Text;
                if (IRND_TYPE.ToUpper() == "MECHINING")
                {
                    IRND_TYPE = "0";
                }
                else
                {
                    IRND_TYPE = "1";
                }
                if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                {
                }
                else
                {
                }
            }
        }

        #region datatable structure
        if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
        {
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_RSM_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("RSM_NO");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_TYPE");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_I_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_UOM");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_RM_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("RM_DEFECT");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_PROD_QTY");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_REJ_QTY");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_RATE");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_STANDARD_PROD");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_SHORT_PROD");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRND_T_CODE");
        }
        #endregion

        #region add control value to Dt
        dr = ((DataTable)ViewState["dt2"]).NewRow();
        dr["IRND_RSM_CODE"] = ddlStage.SelectedValue;
        dr["RSM_NO"] = ddlStage.SelectedItem;
        if (ddlType.SelectedValue == "1")
        {
            dr["IRND_TYPE"] = "CASTING";
        }
        else
        {
            dr["IRND_TYPE"] = "MECHINING";
        }
        dr["IRND_I_CODE"] = ddlItemCode.SelectedValue;
        dr["I_CODENO"] = ddlItemCode.SelectedItem.Text;
        dr["I_NAME"] = ddlItemName.SelectedItem.Text;
        dr["IRND_UOM"] = ddlUOM.SelectedValue;
        dr["I_UOM_NAME"] = ddlUOM.SelectedValue;
        dr["I_UOM_NAME"] = ddlUOM.SelectedItem;
        dr["IRND_RM_CODE"] = ddlDefect.SelectedValue;
        dr["RM_DEFECT"] = ddlDefect.SelectedItem;
        dr["IRND_PROD_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtProdQty.Text)));
        dr["IRND_REJ_QTY"] = string.Format("{0:0.00}", 0);
        dr["IRND_RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text)));
        dr["IRND_STANDARD_PROD"] = string.Format("{0:0.00}", (Convert.ToDouble(txtStanProd.Text)));
        dr["IRND_SHORT_PROD"] = string.Format("{0:0.00}", (Convert.ToDouble(txtShortProd.Text)));
        dr["IRND_T_CODE"] = "0";
        #endregion

        #region check Data table,insert or Modify Data
        if (ViewState["str"].ToString() == "Modify")
        {
            if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
            {
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                txtProdQty.Text = "";
                txtRejQTy.Text = "0";
                txtRejQTy.Text = "0";
                ddlItemName.SelectedIndex = 0;
                ddlItemCode.SelectedIndex = 0;
                ddlStage.SelectedIndex = 0;
                ddlDefect.SelectedIndex = 0;
                ddlUOM.SelectedIndex = 0;
            }
        }
        else
        {
            ((DataTable)ViewState["dt2"]).Rows.Add(dr);
            txtProdQty.Text = "";
            txtRejQTy.Text = "0";
            txtRejQTy.Text = "0";
            ddlItemName.SelectedIndex = 0;
            ddlItemCode.SelectedIndex = 0;
            ddlStage.SelectedIndex = 0;
            ddlDefect.SelectedIndex = 0;
            ddlUOM.SelectedIndex = 0;
            //ddlToolNo.SelectedIndex = 0;
        }
        #endregion

        #region Binding data to Grid
        dgIRN.Enabled = true;
        dgIRN.Visible = true;
        dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
        dgIRN.DataBind();
        ViewState["str"] = "";
        ViewState["ItemUpdateIndex"] = "-1";
        #endregion
    }
    #endregion LoadInsertData

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedValue == "0" || ddlItemCode.SelectedValue == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            //if (ddlToolNo.SelectedValue == "0" || ddlToolNo.SelectedValue == "")
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Select Tool No.";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlToolNo.Focus();
            //    return;
            //}
            if (txtProdQty.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Prod Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtProdQty.Focus();
                return;
            }
            for (int i = 0; i < dgIRN.Rows.Count; i++)
            {
                string ITEM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_I_CODE"))).Text;
                string Type = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_TYPE"))).Text;

                if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                {
                    if (ITEM_CODE == ddlItemCode.SelectedValue.ToString())// && Type == ddlType.SelectedItem.ToString()
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Already Exist For This Item In Table";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
                else
                {
                    if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())// && Type == ddlType.SelectedItem.ToString()
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Already Exist For This Item In Table";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }
            }
            if (Convert.ToDouble(txtProdQty.Text) == Convert.ToDouble(txtStanProd.Text) || Convert.ToDouble(txtShortProd.Text) == 0)
            {
                LoadInsertData();
            }
            else
            {
                //LoadInsertData();
                LoadShortProReason();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    protected void dgIRN_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void dgShortProReason_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    #region dgIRN_RowCommand
    protected void dgIRN_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgIRN.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgIRN.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
                dgIRN.DataBind();
                if (dgIRN.Rows.Count == 0)
                    BlankGrid();

                // ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIRND_I_CODE"))).Text;
                string I_CODE = ((Label)(row.FindControl("lblIRND_I_CODE"))).Text;

                CommonClasses.Execute1("DELETE FROM IRN_SPDETAIL WHERE I_CODE='" + I_CODE + "' AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                // BlankGridReason();
            }
            if (e.CommandName == "Select")
            {
                foreach (GridViewRow gvr in dgIRN.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                ddlStage.SelectedValue = ((Label)(row.FindControl("lblIRND_RSM_CODE"))).Text;
                ddlStage_SelectedIndexChanged(null, null);
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIRND_I_CODE"))).Text;

                string type = ((Label)(row.FindControl("lblIRND_TYPE"))).Text;
                if (type == "MECHINING")
                {
                    ddlType.SelectedValue = "0";
                }
                else
                {
                    ddlType.SelectedValue = "1";
                }
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIRND_I_CODE"))).Text;
                ddlUOM.SelectedValue = ((Label)(row.FindControl("lblIRND_UOM"))).Text;
                ddlDefect.SelectedValue = ((Label)(row.FindControl("lblIRND_RM_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtProdQty.Text = ((Label)(row.FindControl("lblIRND_PROD_QTY"))).Text;
                txtRejQTy.Text = ((Label)(row.FindControl("lblIRND_REJ_QTY"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblIRND_RATE"))).Text;
                txtShortProd.Text = ((Label)(row.FindControl("lblISHORT_QTY"))).Text;
                ddlToolNo.SelectedValue = ((Label)(row.FindControl("lblIRND_T_CODE"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "dgIRN_RowCommand", Ex.Message);
        }
    }
    #endregion

    private void BlankGrid()
    {
        DataTable dtFilter = new DataTable();
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgIRN.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_RSM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_TYPE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_UOM", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_RM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RM_DEFECT", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_PROD_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_REJ_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_RATE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_STANDARD_PROD", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_SHORT_PROD", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_T_CODE", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgIRN.DataSource = dtFilter;
            dgIRN.DataBind();
        }
    }

    public void loadStage()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute("SELECT RSM_CODE, RSM_NO+' - '+RSM_NAME  AS RSM_NO  FROM  REJECTIONSTAGE_MASTER where    RSM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND ES_DELETE=0");
        ddlStage.DataSource = dtStage;
        ddlStage.DataTextField = "RSM_NO";
        ddlStage.DataValueField = "RSM_CODE";
        ddlStage.DataBind();
        ddlStage.Items.Insert(0, new ListItem("----Select Stage----", "0"));
    }

    public void loadItem()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT I_CODE,I_CODENO, I_NAME   FROM ITEM_MASTER WHERE ES_DELETE=0 AND I_CAT_CODE=-2147483648 AND  I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_CODENO");
        ddlItemCode.DataSource = dtStage;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new ListItem("----Select Item Code----", "0"));

        dtStage.DefaultView.Sort = "I_NAME";
        dtStage.DefaultView.ToTable();
        ddlItemName.DataSource = dtStage;
        ddlItemName.DataTextField = "I_NAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new ListItem("----Select Item Name----", "0"));
    }

    public void loadunit()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT I_UOM_CODE,I_UOM_NAME  FROM ITEM_UNIT_MASTER WHERE ES_DELETE=0 AND  I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY  I_UOM_NAME");
        ddlUOM.DataSource = dtStage;
        ddlUOM.DataTextField = "I_UOM_NAME";
        ddlUOM.DataValueField = "I_UOM_CODE";
        ddlUOM.DataBind();
        ddlUOM.Items.Insert(0, new ListItem("----Select Unit----", "0"));
    }

    public void loadunit(string Item)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME ,I_INV_RATE FROM ITEM_MASTER,ITEM_UNIT_MASTER WHERE  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND  ITEM_UNIT_MASTER.ES_DELETE=0 AND  ITEM_MASTER.ES_DELETE=0 AND  I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND I_CODE='" + Item + "' ORDER BY  I_UOM_NAME");

        if (dtStage.Rows.Count > 0)
        {
            ddlUOM.SelectedValue = dtStage.Rows[0]["I_UOM_CODE"].ToString();
            ddlUOM.Enabled = false;
            txtRate.Text = dtStage.Rows[0]["I_INV_RATE"].ToString();
        }
        else
        {
            txtRate.Text = "0";
        }
    }

    public void loadDefect()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT RM_CODE ,RM_DEFECT  FROM REASON_MASTER WHERE RM_TYPE=1 AND ES_DELETE=0 AND RM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY  RM_DEFECT");
        ddlDefect.DataSource = dtStage;
        ddlDefect.DataTextField = "RM_DEFECT";
        ddlDefect.DataValueField = "RM_CODE";
        ddlDefect.DataBind();
        ddlDefect.Items.Insert(0, new ListItem("----Select Defect----", "0"));
    }

    public void loadDefect(string stage)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT RM_CODE ,RM_DEFECT  FROM REASON_MASTER WHERE RM_TYPE=1 AND ES_DELETE=0 AND RM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_RSM_CODE='" + stage + "' ORDER BY  RM_DEFECT");
        ddlDefect.DataSource = dtStage;
        ddlDefect.DataTextField = "RM_DEFECT";
        ddlDefect.DataValueField = "RM_CODE";
        ddlDefect.DataBind();
        ddlDefect.Items.Insert(0, new ListItem("----Select Defect----", "0"));
    }

    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadDefect(ddlStage.SelectedValue.ToString());
    }
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue.ToString();

        loadunit(ddlItemName.SelectedValue.ToString());
        DataTable dtStandPro = CommonClasses.Execute("select isnull(I_STANDARD_PRODUCTION,0) as I_STANDARD_PRODUCTION from ITEM_MASTER where ES_DELETE=0 and I_ACTIVE_IND=1 and I_CODE='" + ddlItemName.SelectedValue + "' and I_CM_COMP_ID=1");
        if (dtStandPro.Rows.Count > 0)
        {
            txtStanProd.Text = Convert.ToDouble(dtStandPro.Rows[0]["I_STANDARD_PRODUCTION"]).ToString();
        }
        LoadToolNo();
    }
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlItemCode.SelectedValue.ToString();
        loadunit(ddlItemCode.SelectedValue.ToString());

        DataTable dtStandPro = CommonClasses.Execute("select isnull(I_STANDARD_PRODUCTION,0) as I_STANDARD_PRODUCTION from ITEM_MASTER where ES_DELETE=0 and I_ACTIVE_IND=1 and I_CODE='" + ddlItemCode.SelectedValue + "' and I_CM_COMP_ID=1");
        if (dtStandPro.Rows.Count > 0)
        {
            txtStanProd.Text = Convert.ToDouble(dtStandPro.Rows[0]["I_STANDARD_PRODUCTION"]).ToString();
        }
        LoadToolNo();
    }

    private void BlankGridReason()
    {
        DataTable dt = new DataTable();
        int I_CODE = Convert.ToInt32(ddlItemCode.SelectedValue);

        dt = CommonClasses.Execute("select SPR_CODE,SPR_DESC ,0 AS SHORT_QTY,'" + I_CODE + "' as I_CODE from SHORTPROD_REASON where ES_DELETE=0 AND SPR_COMPID='" + Convert.ToInt32(Session["CompanyId"]) + "'");
        if (dt.Rows.Count > 0)
        {
            dgShortProReason.Enabled = false;
            // dt.Rows.Add(dt.NewRow());
            dgShortProReason.DataSource = dt;
            dgShortProReason.DataBind();
        }
    }

    #region LoadShortProReason
    private void LoadShortProReason()
    {
        try
        {
            string str = "";
            DataTable dt = new DataTable();
            int I_CODE = Convert.ToInt32(ddlItemCode.SelectedValue);
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (ViewState["str"].ToString() == "Modify")
                {
                    string Code = CommonClasses.GetMaxId("Select Max(IRN_CODE) from IRN_ENTRY");
                    if (((DataTable)(ViewState["dtReason"])).Rows.Count == 0)
                    {
                        ((DataTable)ViewState["dtReason"]).Columns.Add("SPRD_CODE");
                        ((DataTable)ViewState["dtReason"]).Columns.Add("IRN_CODE");
                        ((DataTable)ViewState["dtReason"]).Columns.Add("I_CODE");
                        ((DataTable)ViewState["dtReason"]).Columns.Add("SPR_CODE");
                        ((DataTable)ViewState["dtReason"]).Columns.Add("SHORT_QTY");
                        ((DataTable)ViewState["dtReason"]).Columns.Add("SPR_DESC");
                    }
                    for (int i = 0; i < ((DataTable)(ViewState["dtReason"])).Rows.Count; i++)
                    {
                        ((DataTable)(ViewState["dtReason"])).Rows[i]["IRN_CODE"] = Code;
                    }
                    DataTable dtmod = new DataTable();

                    DataRow[] foundRows;

                    if (((DataTable)(ViewState["dtReason"])).Rows.Count > 0)
                    {
                        // Use the Select method to find all rows matching the filter.
                        foundRows = (((DataTable)(ViewState["dtReason"])).Select("I_CODE = '" + I_CODE + "'"));
                        if (foundRows.Length > 0)
                        {
                            dtmod = foundRows.CopyToDataTable();
                        }
                        else
                        {
                            dtmod = CommonClasses.Execute("select SPR_CODE,SPR_DESC ,0 AS SHORT_QTY,'" + I_CODE + "' as I_CODE from SHORTPROD_REASON where ES_DELETE=0 AND SPR_COMPID='" + Convert.ToInt32(Session["CompanyId"]) + "'");
                        }
                    }
                    else
                    {
                        dtmod = CommonClasses.Execute("select SPR_CODE,SPR_DESC ,0 AS SHORT_QTY,'" + I_CODE + "' as I_CODE from SHORTPROD_REASON where ES_DELETE=0 AND SPR_COMPID='" + Convert.ToInt32(Session["CompanyId"]) + "'");
                    }
                    pnlPop.Visible = false;
                    lblShortPro.Text = txtShortProd.Text;
                    dgShortProReason.Enabled = true;
                    dgShortProReason.DataSource = dtmod;
                    dgShortProReason.DataBind();
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    //Whent insert new record in pro. enrty
                    dt = CommonClasses.Execute("select SPR_CODE,SPR_DESC ,0 AS SHORT_QTY,'" + I_CODE + "' as I_CODE from SHORTPROD_REASON where ES_DELETE=0 AND SPR_COMPID='" + Convert.ToInt32(Session["CompanyId"]) + "'");
                    if (dt.Rows.Count == 0)
                    {
                        dgShortProReason.Enabled = false;
                        dt.Rows.Add(dt.NewRow());
                        dgShortProReason.DataSource = dt;
                        dgShortProReason.DataBind();
                        ModalPopupPrintSelection.Hide();
                        popUpPanel5.Visible = false;
                    }
                    else
                    {
                        pnlPop.Visible = false;
                        lblShortPro.Text = txtShortProd.Text;
                        dgShortProReason.Enabled = true;
                        dgShortProReason.DataSource = dt;
                        dgShortProReason.DataBind();
                        ModalPopupPrintSelection.Show();
                        popUpPanel5.Visible = true;
                    }
                }
            }
            else
            {
                //When Modify old record in pro. enrty
                dt = CommonClasses.Execute("select SPRD_CODE,IRN_CODE,SHORTPROD_REASON.SPR_CODE,IRN_SPDETAIL.SPR_DESC,SHORT_QTY,I_CODE from SHORTPROD_REASON,IRN_SPDETAIL where ES_DELETE=0 and IRN_SPDETAIL.SPR_CODE=SHORTPROD_REASON.SPR_CODE and SPR_COMPID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CODE='" + I_CODE + "' AND (ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");

                if (dt.Rows.Count == 0)
                {
                    dt = CommonClasses.Execute("select SPR_CODE,SPR_DESC ,0 AS SHORT_QTY,'" + I_CODE + "' as I_CODE from SHORTPROD_REASON where ES_DELETE=0 AND SPR_COMPID='" + Convert.ToInt32(Session["CompanyId"]) + "'");
                    if (dt.Rows.Count > 0)
                    {
                        pnlPop.Visible = false;
                        lblShortPro.Text = txtShortProd.Text;
                        dgShortProReason.Enabled = false;
                        dgShortProReason.DataSource = dt;
                        dgShortProReason.DataBind();
                        ModalPopupPrintSelection.Show();
                        popUpPanel5.Visible = true;
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    dgShortProReason.Enabled = false;
                    dt.Rows.Add(dt.NewRow());
                    dgShortProReason.DataSource = dt;
                    dgShortProReason.DataBind();
                    ModalPopupPrintSelection.Hide();
                    popUpPanel5.Visible = false;
                }
                else
                {
                    pnlPop.Visible = false;
                    lblShortPro.Text = txtShortProd.Text;
                    dgShortProReason.Enabled = true;
                    dgShortProReason.DataSource = dt;
                    dgShortProReason.DataBind();
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Activity Transaction - View", "LoadActivity", Ex.Message);
        }
    }
    #endregion

    #region btnConfirm_Click
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlItemCode.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            //if (txtProdQty.Text.Trim() == "")
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "The Field Prod Qty Is Required";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtProdQty.Focus();
            //    return;
            //}
            //if (Convert.ToDouble(txtProdQty.Text.Trim()) == 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "The Field Prod Qty Is Required";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtProdQty.Focus();
            //    return;
            //}
            //to display Sum of gridview rows
            float GTotal = 0;
            for (int i = 0; i < dgShortProReason.Rows.Count; i++)
            {
                double total = Convert.ToDouble((dgShortProReason.Rows[i].FindControl("lblSHORT_QTY") as TextBox).Text);
                GTotal += Convert.ToSingle(total);
            }

            ViewState["GTotal"] = GTotal;

            if (GTotal < Convert.ToDouble(txtShortProd.Text) || GTotal > Convert.ToDouble(txtShortProd.Text))
            {
                pnlPop.Visible = true;
                lblPopMsg.Text = "Short Qty. does not match with Short Prod.";
                ModalPopupPrintSelection.Show();
                popUpPanel5.Visible = true;
                return;
            }

            if (Convert.ToDouble(txtShortProd.Text) == GTotal)
            {
                #region datatable structure
                if (((DataTable)ViewState["dtReason"]).Columns.Count == 0)
                {
                    ((DataTable)ViewState["dtReason"]).Columns.Add("SPRD_CODE");
                    ((DataTable)ViewState["dtReason"]).Columns.Add("IRN_CODE");
                    ((DataTable)ViewState["dtReason"]).Columns.Add("I_CODE");
                    ((DataTable)ViewState["dtReason"]).Columns.Add("SPR_CODE");
                    ((DataTable)ViewState["dtReason"]).Columns.Add("SHORT_QTY");
                    ((DataTable)ViewState["dtReason"]).Columns.Add("SPR_DESC");
                }
                #endregion

                if (ViewState["str"].ToString() == "Modify")
                {
                    DataRow[] foundRows;
                    foundRows = ((DataTable)ViewState["dtReason"]).Select("I_CODE <>'" + ddlItemCode.SelectedValue + "'");
                    if (foundRows.Length > 0)
                    {
                        ViewState["dtReason"] = foundRows.CopyToDataTable();
                    }
                    else
                    {
                        ((DataTable)ViewState["dtReason"]).Clear();
                        ((DataTable)ViewState["dtReason"]).Rows.Add(((DataTable)ViewState["dtReason"]).NewRow());
                    }
                }
                #region add control value to Dt
                for (int i = 0; i < dgShortProReason.Rows.Count; i++)
                {
                    ((DataTable)ViewState["dtReason"]).Rows.Add(null, 0, (dgShortProReason.Rows[i].FindControl("lblI_CODE") as Label).Text, (dgShortProReason.Rows[i].FindControl("lblSPRD_CODE") as Label).Text, Convert.ToDouble((dgShortProReason.Rows[i].FindControl("lblSHORT_QTY") as TextBox).Text), (dgShortProReason.Rows[i].FindControl("lblSPR_DESC") as Label).Text);
                }
                #endregion

                LoadInsertData();
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    protected void txtProdQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double ProQty = Convert.ToDouble(txtProdQty.Text), StandQty = Convert.ToDouble(txtStanProd.Text);
            if (txtStanProd.Text.Trim() == "")
            {
                txtStanProd.Text = "0";
            }
            if (txtProdQty.Text.Trim() == "")
            {
                txtProdQty.Text = "0";
            }
            if (ProQty > StandQty)
            {
                txtShortProd.Text = "0";
            }
            else
            {
                txtShortProd.Text = (Convert.ToDouble(txtStanProd.Text) - Convert.ToDouble(txtProdQty.Text)).ToString();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region LoadToolNo
    private void LoadToolNo()
    {
        DataTable dt = new DataTable();
        string strcon = "";
        if (ddlItemCode.SelectedIndex != 0)
        {
            strcon = strcon + "T_I_CODE=" + ddlItemCode.SelectedValue + " AND ";
        }
        if (ddlItemName.SelectedIndex != 0)
        {
            strcon = strcon + "T_I_CODE=" + ddlItemCode.SelectedValue + " AND ";
        }
        dt = CommonClasses.Execute("select T_CODE,T_NAME from TOOL_MASTER where " + strcon + " ES_DELETE=0 and T_CM_COMP_ID=" + Session["CompanyID"] + " AND T_TYPE=0 and T_STATUS='1' order by T_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlToolNo.DataSource = dt;
            ddlToolNo.DataTextField = "T_NAME";
            ddlToolNo.DataValueField = "T_CODE";
            ddlToolNo.DataBind();
            //ddlToolNo.Items.Insert(0, new ListItem("Tool No.", "0"));
        }
        else
        {
            ddlToolNo.Items.Clear();
            ddlToolNo.DataSource = null;
            ddlToolNo.DataBind();
        }
    }
    #endregion LoadToolNo
    #region rbtWithAmt_SelectedIndexChanged
    protected void rbtWithAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        ((DataTable)ViewState["dt2"]).Clear();
        BlankGrid();
        loadItem();
        //ddlItemCode_SelectedIndexChanged(null, null);
    }
    #endregion rbtWithAmt_SelectedIndexChanged
}


