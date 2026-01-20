using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class IRN_ADD_OtherIRNEntry : System.Web.UI.Page
{
    #region General Declaration
    UnitMaster_BL BL_UnitMaster = null;
    static int mlCode = 0;
    static string right = "";
    DataRow dr;
    public static string str = "";
    public static int Index = 0;
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
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
                    CommonClasses.SendError("OTHER IRN ENTRY", "PageLoad", ex.Message);
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
            CommonClasses.SendError("OTHER IRN ENTRY", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("OTHER IRN ENTRY", "btnCancel_Click", ex.Message.ToString());
        }
    }

    private bool CheckValid()
    {
        bool flag = false;
        try
        {
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("OTHER IRN ENTRY", "CheckValid", Ex.Message);
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
            Response.Redirect("~/IRN/VIEW/ViewOtherIRNEntry.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("OTHER IRN ENTRY", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/IRN/VIEW/ViewOtherIRNEntry.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("OTHER IRN ENTRY", "btnClose_Click", Ex.Message);
        }
    }
    #endregion btnClose_Click
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT   IRN_CODE, IRN_NO, IRN_CM_ID, IRN_DATE FROM IRN_ENTRY WHERE     (ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dt.Rows.Count > 0)
            {
                txtGRNno.Text = dt.Rows[0]["IRN_NO"].ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IRN_DATE"].ToString()).ToString("dd/MMM/yyyy");
            }
            DataTable dtdetails = new DataTable();
            dtdetails = CommonClasses.Execute(" SELECT     IRN_DETAIL.IRND_RSM_CODE, IRN_DETAIL.IRND_TYPE, CASE WHEN IRND_TYPE = 0 THEN 'CASTING' ELSE 'MECHINING' END AS IRND_TYPE1,  IRN_DETAIL.IRND_I_CODE, IRN_DETAIL.IRND_UOM, IRN_DETAIL.IRND_RM_CODE, IRN_DETAIL.IRND_PROD_QTY, IRN_DETAIL.IRND_REJ_QTY,  IRN_DETAIL.IRND_RATE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME, ITEM_UNIT_MASTER.I_UOM_NAME, RSM_NO+' - '+RSM_NAME AS RSM_NO,  REASON_MASTER.RM_DEFECT, REJECTIONSTAGE_MASTER.RSM_NAME, IRN_DETAIL.IRND_P_CODE, PARTY_MASTER.P_NAME FROM REJECTIONSTAGE_MASTER INNER JOIN IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE ON REJECTIONSTAGE_MASTER.RSM_CODE = IRN_DETAIL.IRND_RSM_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON IRN_DETAIL.IRND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN REASON_MASTER ON IRN_DETAIL.IRND_RM_CODE = REASON_MASTER.RM_CODE INNER JOIN PARTY_MASTER ON IRN_DETAIL.IRND_P_CODE = PARTY_MASTER.P_CODE  WHERE     (IRN_ENTRY.ES_DELETE = 0)  AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");

            if (dtdetails.Rows.Count > 0)
            {
                dgIRN.Enabled = true;
                ViewState["dt2"] = dtdetails;
                dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
                dgIRN.DataBind();
            }

            if (str == "MOD")
            {
                CommonClasses.SetModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            else
            {
                txtGRNno.Enabled = false;
                txtGRNDate.Enabled = false;
                ddlDefect.Enabled = false;
                ddlStage.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlType.Enabled = false;
                ddlParty.Enabled = false;
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
            CommonClasses.SendError("OTHER IRN ENTRY", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("OTHER IRN ENTRY", "GetValues", ex.Message);
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
            CommonClasses.SendError("OTHER IRN ENTRY", "Setvalues", ex.Message);
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

            if (Request.QueryString[0].Equals("INSERT"))
            {
                txtGRNno.Text = CommonClasses.GetMaxNO("SELECT MAX(ISNULL(IRN_NO,0))+1 FROM IRN_ENTRY WHERE  IRN_TYPE=1 AND ES_DELETE=0 AND IRN_TRANS_TYPE=0");
                if (CommonClasses.Execute1("INSERT INTO IRN_ENTRY (IRN_NO,  IRN_DATE,IRN_CM_ID,IRN_TYPE,IRN_TRANS_TYPE)VALUES ('" + txtGRNno.Text.Trim() + "','" + txtGRNDate.Text + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + true + "',0) "))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(IRN_CODE) from IRN_ENTRY");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO IRN_DETAIL (IRND_IRN_CODE, IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE,IRND_P_CODE,IRND_AMT)  VALUES     ('" + Code + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_P_CODE")).Text + "','" + Math.Round(Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text) * Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text), 2).ToString() + "') ");
                    }
                    CommonClasses.WriteLog("OTHER IRN ENTRY", "Save", "OTHER IRN ENTRY", txtGRNno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewOtherIRNEntry.aspx", false);
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

                if (CommonClasses.Execute1("UPDATE IRN_ENTRY  SET IRN_NO='" + txtGRNno.Text.Trim() + "',IRN_DATE='" + txtGRNDate.Text.Trim() + "'     where IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    CommonClasses.Execute1("DELETE FROM IRN_DETAIL     where IRND_IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO IRN_DETAIL (IRND_IRN_CODE, IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE,IRND_P_CODE,IRND_AMT)  VALUES     ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_P_CODE")).Text + "','" + Math.Round(Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text) * Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text), 2).ToString() + "') ");
                    }
                    CommonClasses.RemoveModifyLock("IRN_ENTRY", "MODIFY", "IRN_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("OTHER IRN ENTRY", "Update", "OTHER IRN ENTRY", txtGRNno.Text.Trim(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewOtherIRNEntry.aspx", false);
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
            CommonClasses.SendError("OTHER IRN ENTRY", "SaveRec", ex.Message);
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
            CommonClasses.SendError("OTHER IRN ENTRY", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;

            if (Convert.ToInt32(ddlStage.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Stage";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlStage.Focus();
                return;
            }
            if (Convert.ToInt32(ddlItemCode.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (Convert.ToInt32(ddlDefect.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Defect";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlDefect.Focus();
                return;
            }
            if (Convert.ToInt32(ddlParty.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Party";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlParty.Focus();
                return;
            }
            if (ddlType.SelectedValue == "-1")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select type";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlType.Focus();
                return;
            }
            if (txtRejQTy.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Rej. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRejQTy.Focus();
                return;
            }
            if (Convert.ToDouble(txtRejQTy.Text.Trim()) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Rej. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRejQTy.Focus();
                return;
            }
            PanelMsg.Visible = false;

            if (dgIRN.Rows.Count > 0)
            {
                for (int i = 0; i < dgIRN.Rows.Count; i++)
                {
                    string IRND_I_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_I_CODE"))).Text;
                    string IRND_RSM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE"))).Text;
                    string IRND_RM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_RM_CODE"))).Text;
                    string IRND_P_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_P_CODE"))).Text;
                    string IRND_TYPE = ((Label)(dgIRN.Rows[i].FindControl("lblIRND_TYPE"))).Text;

                    if (IRND_TYPE.ToUpper() == "TRUE'")
                    {
                        IRND_TYPE = "0";
                    }
                    else
                    {
                        IRND_TYPE = "1";
                    }
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (IRND_TYPE == ddlType.SelectedValue.ToString() && IRND_I_CODE == ddlItemName.SelectedValue.ToString() && IRND_RSM_CODE == ddlStage.SelectedValue.ToString() && IRND_RM_CODE == ddlDefect.SelectedValue.ToString() && IRND_P_CODE == ddlParty.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (IRND_TYPE == ddlType.SelectedValue.ToString() && IRND_I_CODE == ddlItemName.SelectedValue.ToString() && IRND_RSM_CODE == ddlStage.SelectedValue.ToString() && IRND_RM_CODE == ddlDefect.SelectedValue.ToString() && IRND_P_CODE == ddlParty.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IRND_RSM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("RSM_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IRND_TYPE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IRND_TYPE1");
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
                ((DataTable)ViewState["dt2"]).Columns.Add("IRND_P_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("P_NAME");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IRND_RSM_CODE"] = ddlStage.SelectedValue;
            dr["RSM_NO"] = ddlStage.SelectedItem;
            if (ddlType.SelectedValue == "0")
            {
                dr["IRND_TYPE"] = true;
            }
            else
            {
                dr["IRND_TYPE"] = false;
            }

            dr["IRND_TYPE1"] = ddlType.SelectedItem;
            dr["IRND_I_CODE"] = ddlItemCode.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem.Text;
            dr["I_NAME"] = ddlItemName.SelectedItem.Text;
            dr["IRND_UOM"] = ddlUOM.SelectedValue;
            dr["I_UOM_NAME"] = ddlUOM.SelectedValue;
            dr["I_UOM_NAME"] = ddlUOM.SelectedItem;
            dr["IRND_RM_CODE"] = ddlDefect.SelectedValue;
            dr["RM_DEFECT"] = ddlDefect.SelectedItem;
            dr["IRND_PROD_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtProdQty.Text)));
            dr["IRND_REJ_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRejQTy.Text)));
            dr["IRND_RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text)));
            dr["IRND_P_CODE"] = ddlParty.SelectedValue;
            dr["P_NAME"] = ddlParty.SelectedItem;

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtProdQty.Text = "0";
                    txtRejQTy.Text = "";
                    txtRejQTy.Text = "";
                    ddlDefect.SelectedIndex = 0;
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtProdQty.Text = "0";
                txtRejQTy.Text = "";
                txtRejQTy.Text = "";
                ddlDefect.SelectedIndex = 0;
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    protected void dgIRN_Deleting(object sender, GridViewDeleteEventArgs e)
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
                string stype = ((Label)(row.FindControl("lblIRND_TYPE"))).Text;
                if (stype.ToUpper() == "TRUE")
                {
                    ddlType.SelectedValue = "0";
                }
                else
                {
                    ddlType.SelectedValue = "1";
                }
                ddlType_SelectedIndexChanged(null, null);
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIRND_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIRND_I_CODE"))).Text;
                ddlUOM.SelectedValue = ((Label)(row.FindControl("lblIRND_UOM"))).Text;
                ddlDefect.SelectedValue = ((Label)(row.FindControl("lblIRND_RM_CODE"))).Text;
                ddlParty.SelectedValue = ((Label)(row.FindControl("lblIRND_P_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtProdQty.Text = ((Label)(row.FindControl("lblIRND_PROD_QTY"))).Text;
                txtRejQTy.Text = ((Label)(row.FindControl("lblIRND_REJ_QTY"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblIRND_RATE"))).Text;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "dgIRN_RowCommand", Ex.Message);
        }
    }
    #endregion

    DataTable dtFilter = new DataTable();
    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgIRN.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_RSM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_TYPE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_TYPE1", typeof(string)));
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
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_P_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(string)));
            dtFilter.Rows.Add(dtFilter.NewRow());
            dgIRN.DataSource = dtFilter;
            dgIRN.DataBind();
        }
    }

    public void loadStage()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute("SELECT RSM_CODE, RSM_NO+' - '+RSM_NAME  AS RSM_NO  FROM  REJECTIONSTAGE_MASTER where    RSM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'  AND RSM_CODE!=-2147483648 AND ES_DELETE=0");
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
        dtStage = CommonClasses.Execute(" SELECT RM_CODE ,RM_DEFECT  FROM REASON_MASTER WHERE RM_TYPE=1 AND ES_DELETE=0   AND RM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY  RM_DEFECT");
        ddlDefect.DataSource = dtStage;
        ddlDefect.DataTextField = "RM_DEFECT";
        ddlDefect.DataValueField = "RM_CODE";
        ddlDefect.DataBind();
        ddlDefect.Items.Insert(0, new ListItem("----Select Defect----", "0"));
    }

    public void loadDefect(string stage)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT RM_CODE ,RM_DEFECT  FROM REASON_MASTER WHERE RM_TYPE=1 AND ES_DELETE=0   AND RM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_RSM_CODE='" + stage + "' ORDER BY  RM_DEFECT");
        ddlDefect.DataSource = dtStage;
        ddlDefect.DataTextField = "RM_DEFECT";
        ddlDefect.DataValueField = "RM_CODE";
        ddlDefect.DataBind();
        ddlDefect.Items.Insert(0, new ListItem("----Select Defect----", "0"));
    }
    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadType(ddlStage.SelectedValue.ToString());
        LoadParty(ddlStage.SelectedValue.ToString());
    }
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemCode.SelectedValue = ddlItemName.SelectedValue.ToString();

        loadunit(ddlItemName.SelectedValue.ToString());
    }
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.SelectedValue = ddlItemCode.SelectedValue.ToString();
        loadunit(ddlItemCode.SelectedValue.ToString());
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT RM_CODE ,RM_DEFECT FROM REASON_MASTER WHERE ES_DELETE=0 AND RM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_RSM_CODE='" + ddlStage.SelectedValue + "' AND RM_TYPE='" + ddlType.SelectedValue + "' ORDER BY  RM_DEFECT");
        ddlDefect.DataSource = dtStage;
        ddlDefect.DataTextField = "RM_DEFECT";
        ddlDefect.DataValueField = "RM_CODE";
        ddlDefect.DataBind();
        ddlDefect.Items.Insert(0, new ListItem("----Select Defect----", "0"));
    }
    public void loadType(string stage)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT ISNULL(RSM_CASTING,0) AS RSM_CASTING ,ISNULL(RSM_MECHINING,0) AS RSM_MECHINING  FROM REJECTIONSTAGE_MASTER where RSM_CODE='" + stage + "' AND RSM_CM_ID=1 AND ES_DELETE=0");
        if (dtStage.Rows.Count > 0)
        {
            ddlType.Items.Clear();
            int count = 0;
            ddlType.Items.Insert(count, new ListItem("Select", "-1"));
            count = count + 1;
            if (Convert.ToBoolean(dtStage.Rows[0]["RSM_CASTING"].ToString()) == true)
            {
                ddlType.Items.Insert(count, new ListItem("CASTING", "1"));
                count = count + 1;
            }
            else
            {

            }
            if (Convert.ToBoolean(dtStage.Rows[0]["RSM_MECHINING"].ToString()) == true)
            {
                ddlType.Items.Insert(count, new ListItem("MECHINING", "0"));
            }
            else
            {

            }
        }
        ddlType_SelectedIndexChanged(null, null);
    }

    public void LoadParty(string stage)
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("SELECT * FROM STAGEWISEPARTY where RSM_CODE='" + stage + "'");
        DataTable dtStage = new DataTable();
        if (dt.Rows[0]["PARTY"].ToString().Trim() == "SUB")
        {
            dtStage = CommonClasses.Execute(" SELECT  * FROM PARTY_MASTER  WHERE ES_DELETE = 0 AND  P_TYPE=2 AND P_STM_CODE IN(-2147483647 , -2147483646) AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   ORDER BY P_NAME");
            ddlParty.DataSource = dtStage;
            ddlParty.DataTextField = "P_NAME";
            ddlParty.DataValueField = "P_CODE";
            ddlParty.DataBind();
            ddlParty.Items.Insert(0, new ListItem("----Select Party----", "0"));
        }
        else if (dt.Rows[0]["PARTY"].ToString().Trim() == "CUST")
        {
            dtStage = CommonClasses.Execute(" SELECT  * FROM PARTY_MASTER  WHERE  ES_DELETE = 0 AND  P_TYPE=1   AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   ORDER BY P_NAME");
            ddlParty.DataSource = dtStage;
            ddlParty.DataTextField = "P_NAME";
            ddlParty.DataValueField = "P_CODE";
            ddlParty.DataBind();
            ddlParty.Items.Insert(0, new ListItem("----Select Party----", "0"));
        }
        else if (dt.Rows[0]["PARTY"].ToString().Trim() == "ALL")
        {
            dtStage = CommonClasses.Execute("  SELECT  * FROM PARTY_MASTER  where   ES_DELETE = 0 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   ORDER BY P_NAME");
            ddlParty.DataSource = dtStage;
            ddlParty.DataTextField = "P_NAME";
            ddlParty.DataValueField = "P_CODE";
            ddlParty.DataBind();
            ddlParty.Items.Insert(0, new ListItem("----Select Party----", "0"));
        }
        else
        {
            dtStage = CommonClasses.Execute(" SELECT  * FROM PARTY_MASTER  WHERE P_CODE= '" + dt.Rows[0]["PARTY"].ToString().Trim() + "'  AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'   ORDER BY P_NAME");
            ddlParty.DataSource = dtStage;
            ddlParty.DataTextField = "P_NAME";
            ddlParty.DataValueField = "P_CODE";
            ddlParty.DataBind();
        }
    }
}


