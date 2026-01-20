using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.IO;
using System.Drawing;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Data.SqlClient; 
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_Test : System.Web.UI.Page
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
            CommonClasses.SendError("FOUNDRY IRN ENTRY", "btnCancel_Click", ex.Message.ToString());
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
            dt = CommonClasses.Execute("SELECT   IRN_CODE, IRN_NO, IRN_CM_ID, IRN_DATE FROM IRN_ENTRY WHERE     (ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dt.Rows.Count > 0)
            {
                txtGRNno.Text = dt.Rows[0]["IRN_NO"].ToString();
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IRN_DATE"].ToString()).ToString("dd/MMM/yyyy");

            }
            DataTable dtdetails = new DataTable();
            dtdetails = CommonClasses.Execute("SELECT IRN_DETAIL.IRND_RSM_CODE, CASE WHEN IRND_TYPE=1 then 'CASTING' ELSE 'MECHINING' END AS IRND_TYPE, IRN_DETAIL.IRND_I_CODE, IRN_DETAIL.IRND_UOM, IRN_DETAIL.IRND_RM_CODE,   IRN_DETAIL.IRND_PROD_QTY, IRN_DETAIL.IRND_REJ_QTY, IRN_DETAIL.IRND_RATE, ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,   ITEM_UNIT_MASTER.I_UOM_NAME, '' AS RSM_NO,'' AS RM_DEFECT, '' AS RSM_NAME FROM IRN_ENTRY INNER JOIN IRN_DETAIL ON IRN_ENTRY.IRN_CODE = IRN_DETAIL.IRND_IRN_CODE INNER JOIN ITEM_UNIT_MASTER INNER JOIN ITEM_MASTER ON ITEM_UNIT_MASTER.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE ON IRN_DETAIL.IRND_I_CODE = ITEM_MASTER.I_CODE  WHERE     (IRN_ENTRY.ES_DELETE = 0) AND IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
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
            //txtUnitName.Text = BL_UnitMaster.I_UOM_NAME;
            //txtUnitDesc.Text = BL_UnitMaster.I_UOM_DESC;
            if (str == "VIEW")
            {

                //txtName.Enabled = false;
                //txtStageNo.Enabled = false;
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

            if (Request.QueryString[0].Equals("INSERT"))
            {
                txtGRNno.Text = CommonClasses.GetMaxNO("SELECT MAX(IRN_NO)+1 FROM IRN_ENTRY WHERE IRN_TYPE=0 AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 ");
                //initilize sql connection
                string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
                //object of sql transaction
                SqlTransaction trans;
                //initilize connection
                SqlConnection connection = new SqlConnection(strConnString);
                //open connection
                connection.Open();
                //start of sql trandaction
                trans = connection.BeginTransaction();

                try
                {
                    //trans = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand("INSERT INTO IRN_ENTRY (IRN_NO,IRN_DATE,IRN_CM_ID,IRN_TRANS_TYPE)VALUES ('" + txtGRNno.Text.Trim() + "','" + txtGRNDate.Text + "','" + Convert.ToInt32(Session["CompanyId"]) + "',1) ", connection, trans);
                    command.ExecuteNonQuery();
                    string Code = "";
                    SqlCommand cmd1 = new SqlCommand("Select Max(IRN_CODE) from IRN_ENTRY", connection, trans);
                    cmd1.Transaction = trans;
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    while (dr1.Read())
                    {
                        Code = (dr1[0].ToString().Trim());
                    }
                    cmd1.Dispose();
                    dr1.Dispose();
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        bool type = false;
                        if (((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text.ToUpper() == "CASTING")
                        {
                            type = true;
                        }
                        // CommonClasses.Execute1();
                        SqlCommand command1 = new SqlCommand("INSERT INTO IRN_DETAIL (IRND_IRN_CODE, IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE,IRND_AMT)  VALUES     ('" + Code + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + type + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "','" + Math.Round(Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text) * Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text), 2).ToString() + "') ", connection, trans);
                        command1.ExecuteNonQuery();
                        // Stock Ledger  
                        if (type == true)
                        {
                            SqlCommand command2 = new SqlCommand(" INSERT INTO STOCK_LEDGER (STL_I_CODE	,STL_DOC_NO	,STL_DOC_NUMBER	,STL_DOC_TYPE	,STL_DOC_DATE	,STL_DOC_QTY ,STL_STORE_TYPE) VALUES('" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + Code + "','" + txtGRNno.Text.Trim() + "','Production To Foundary','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "',' -2147483646' )", connection, trans);
                            command2.ExecuteNonQuery();
                            SqlCommand command3 = new SqlCommand(" UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "  WHERE I_CODE='" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "' ", connection, trans);
                            command3.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                    CommonClasses.WriteLog("PROD ENTRY", "Save", "PROD ENTRY", txtGRNno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewProdEntry.aspx", false);
                    //}

                    //  trans.Commit();
                }
                catch (Exception ex) //error occurred
                {
                    trans.Rollback();
                    //Handel error
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                if (CommonClasses.Execute1("UPDATE IRN_ENTRY SET IRN_NO='" + txtGRNno.Text.Trim() + "',IRN_DATE='" + txtGRNDate.Text.Trim() + "'     where IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    DataTable dtDetail = CommonClasses.Execute("SELECT * FROM IRN_DETAIL where IRND_IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "' AND    IRND_TYPE=1");
                    if (dtDetail.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtDetail.Rows.Count; i++)
                        {
                            CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' AND STL_I_CODE='" + dtDetail.Rows[i]["IRND_I_CODE"].ToString() + "' and STL_DOC_TYPE='Production To Foundary'");
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + dtDetail.Rows[i]["IRND_PROD_QTY"].ToString() + "  WHERE I_CODE='" + dtDetail.Rows[i]["IRND_I_CODE"].ToString() + "'");
                        }
                    }
                    CommonClasses.Execute1("DELETE FROM IRN_DETAIL where IRND_IRN_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        bool type = false;
                        if (((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text.ToUpper() == "CASTING")
                        {
                            type = true;
                        }
                        // CommonClasses.Execute1("INSERT INTO IRN_DETAIL (IRND_IRN_CODE, IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE)  VALUES     ('" + mlCode + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_TYPE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "') ");
                        CommonClasses.Execute1("INSERT INTO IRN_DETAIL (IRND_IRN_CODE, IRND_RSM_CODE, IRND_TYPE, IRND_I_CODE, IRND_UOM, IRND_RM_CODE, IRND_PROD_QTY, IRND_REJ_QTY, IRND_RATE,IRND_AMT)  VALUES     ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RSM_CODE")).Text + "','" + type + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RM_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text + "','" + Math.Round(Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_RATE")).Text) * Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text), 2).ToString() + "') ");
                        if (type == true)
                        {
                            CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE	,STL_DOC_NO	,STL_DOC_NUMBER	,STL_DOC_TYPE	,STL_DOC_DATE	,STL_DOC_QTY ,STL_STORE_TYPE) VALUES('" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "','" + Convert.ToInt32(ViewState["mlCode"]) + "','" + txtGRNno.Text.Trim() + "','Production To Foundary','" + Convert.ToDateTime(txtGRNDate.Text).ToString("dd/MMM/yyyy") + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "',' -2147483646')");
                            CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_PROD_QTY")).Text + "  WHERE I_CODE='" + ((Label)dgIRN.Rows[i].FindControl("lblIRND_I_CODE")).Text + "'");
                        }
                    }
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

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {

        try
        {
            PanelMsg.Visible = false;
            if (Convert.ToInt32(ddlItemCode.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            } string strQR = "";

            strQR = ddlItemCode.SelectedItem.ToString() + "," + ddlItemName.SelectedItem.ToString() + "," + ddlDefect.SelectedItem.ToString();

           

            if (txtProdQty.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Prod Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtProdQty.Focus();
                return;

            }
            if (Convert.ToDouble(txtProdQty.Text.Trim()) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Prod Qty Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtProdQty.Focus();
                return;

            }

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
        ddlStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select Stage----", "0"));
    }

    public void loadItem()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT I_CODE,I_CODENO, I_NAME   FROM ITEM_MASTER WHERE ES_DELETE=0 AND I_CAT_CODE=-2147483648 AND  I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_CODENO");
        ddlItemCode.DataSource = dtStage;
        ddlItemCode.DataTextField = "I_CODENO";
        ddlItemCode.DataValueField = "I_CODE";
        ddlItemCode.DataBind();
        ddlItemCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select Item Code----", "0"));

        dtStage.DefaultView.Sort = "I_NAME";
        dtStage.DefaultView.ToTable();
        ddlItemName.DataSource = dtStage;
        ddlItemName.DataTextField = "I_NAME";
        ddlItemName.DataValueField = "I_CODE";
        ddlItemName.DataBind();
        ddlItemName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select Item Name----", "0"));
    }



    public void loadunit()
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT I_UOM_CODE,I_UOM_NAME  FROM ITEM_UNIT_MASTER WHERE ES_DELETE=0 AND  I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY  I_UOM_NAME");
        ddlUOM.DataSource = dtStage;
        ddlUOM.DataTextField = "I_UOM_NAME";
        ddlUOM.DataValueField = "I_UOM_CODE";
        ddlUOM.DataBind();
        ddlUOM.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select Unit----", "0"));
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
        ddlDefect.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select Defect----", "0"));
    }
    public void loadDefect(string stage)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute(" SELECT RM_CODE ,RM_DEFECT  FROM REASON_MASTER WHERE RM_TYPE=1 AND ES_DELETE=0   AND RM_CM_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND RM_RSM_CODE='" + stage + "' ORDER BY  RM_DEFECT");
        ddlDefect.DataSource = dtStage;
        ddlDefect.DataTextField = "RM_DEFECT";
        ddlDefect.DataValueField = "RM_CODE";
        ddlDefect.DataBind();
        ddlDefect.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select Defect----", "0"));
    }
    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadDefect(ddlStage.SelectedValue.ToString());
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
}
