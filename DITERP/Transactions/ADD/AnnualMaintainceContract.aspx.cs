using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;

public partial class Transactions_ADD_AnnualMaintainceContract : System.Web.UI.Page
{
    #region Variable
    clsSqlDatabaseAccess cls = new clsSqlDatabaseAccess();
    DataTable dt = new DataTable();
    DataTable dtInwardDetail = new DataTable();
    DataTable dtInw = new DataTable();
    DataTable dtPODetail = new DataTable();
    DataTable dtDetails = new DataTable();
    public static string imgEmpImage;
    static int File_Code = 0;
    static int File_Code1 = 0;
    static DataTable dtCurrentTable = new DataTable();    

    static int mlCode = 0;
    DataRow dr;
    static DataTable BindTable = new DataTable();
    static DataTable TemTaable = new DataTable();
    static DataTable dtInfo = new DataTable();
    static DataTable dt2 = new DataTable();
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
 
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        string Remark = "";
        try
        {
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
                        BindTable.Clear();
                        txtAMCDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtfromDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                        txtToDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
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
                        else if (Request.QueryString[0].Equals("AMEND"))
                        {
                            mlCode = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("AMEND");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadSupplier();
                            BlankGridView();
                            LoadIUnit();
                            txtAMCDate.Attributes.Add("readonly", "readonly");
                            txtfromDate.Attributes.Add("readonly", "readonly");
                            txtToDate.Attributes.Add("readonly", "readonly");
                            Remark = "1) Agreement period start on "+txtfromDate.Text+" to "+txtToDate.Text+"." + Environment.NewLine + "2) All service request/calls for repair work must be attended within 24 hours. " + Environment.NewLine + "3) The release of payment will be made on quarterly basis (at the begining of next quarter)." + Environment.NewLine + " on the basis of satisfactory performance." + Environment.NewLine + " 4) The case of delay in attending to the complaints penalty will be levied." + Environment.NewLine + "5) The period will be reckoned from the time of lodging the complaint by telephone/fax/letter, etc." + Environment.NewLine + "";
                            txtNote.Text = Remark.ToString();

                            //dt2.Rows.Clear();
                            //dt2.Columns.Clear();
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("AMC Transaction", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("AMC Transaction", "Page_Load", ex.Message.ToString());
        }

    }
    #endregion Page_Load
    
    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (dgAMCDetails.Rows.Count != 0)
        {
            if (ddlSupplier.SelectedIndex == 0)
            {
                ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "Please Select Supplier";
                ddlSupplier.Focus();
                return;
            }
            if (txtAMCDate.Text == "")
            {
                ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "PleaseEnter PO Date";
                txtAMCDate.Focus();
                return;
            }
            if (txtfromDate.Text == "")
            {
                ShowMessage("#Avisos", "Select From Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "PleaseEnter PO Date";
                txtfromDate.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                ShowMessage("#Avisos", "Select To Date", CommonClasses.MSG_Warning);
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "PleaseEnter PO Date";
                txtToDate.Focus();
                return;
            }
            if (txtReferance.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Referance Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtReferance.Focus();
                return;

            }
            if (txtContactPerson.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Contact Person Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtContactPerson.Focus();
                return;

            }
            if (txtPhoneNo.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Phone No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPhoneNo.Focus();
                return;

            }
            if (txtServiceTax.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "The Field Phone No Is Required";
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //txtPhoneNo.Focus();
                //return;
                txtServiceTax.Text = "0.00";
            }
            SaveRec();


        }
        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);

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
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
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
            CommonClasses.SendError("AMC Transaction", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE) ,P_NAME from PARTY_MASTER where ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 order by P_NAME");
            ddlSupplier.DataSource = dt;
            ddlSupplier.DataTextField = "P_NAME";
            ddlSupplier.DataValueField = "P_CODE";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("Select Supplier", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "LoadSupplier", Ex.Message);
        }

    }
    #endregion

    #region LoadIUnit
    private void LoadIUnit()
    {
        try
        {
            dt = CommonClasses.Execute("select I_UOM_CODE,I_UOM_NAME from ITEM_UNIT_MASTER where ES_DELETE=0 and I_UOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " ORDER BY I_UOM_NAME");
            ddlUnit.DataSource = dt;
            ddlUnit.DataTextField = "I_UOM_NAME";
            ddlUnit.DataValueField = "I_UOM_CODE";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("Select Item Unit", "0"));
            ddlUnit.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction ", "LoadIUnit", Ex.Message);
        }

    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {

        try
        {

            if (ddlSupplier.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Supplier";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                ddlSupplier.Focus();
                return;
            }
            if (txtAMCDate.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field AMC Date Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtAMCDate.Focus();
                return;

            }
            if (txtReferance.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Referance Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtReferance.Focus();
                return;

            }
            if (txtContactPerson.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Contact Person Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtContactPerson.Focus();
                return;

            }
            if (txtPhoneNo.Text == "")
            {
                //ShowMessage("#Avisos", "Select PO Date", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Phone No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPhoneNo.Focus();
                return;

            }
            if (txtProductName.Text == "")
            {
                //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Product Name Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtProductName.Focus();
                return;
            }

           
            if (txtQty.Text == "")
            {
                //ShowMessage("#Avisos", "Please Enter Qty", CommonClasses.MSG_Warning);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "The Field Qty Is Required";
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //txtQty.Focus();
                //return;
                txtQty.Text = "0.00";
            }
            if (txtRate.Text == "")
            {
                //ShowMessage("#Avisos", "Please Enter Rate", CommonClasses.MSG_Warning);
                //PanelMsg.Visible = true;
                //lblmsg.Text = "The Field Rate Is Required";
                //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //txtRate.Focus();
                //return;
                txtRate.Text = "0.00";
            }
            if (txtAmount.Text == "" || txtAmount.Text == "0.00")
            {
                //ShowMessage("#Avisos", "Please Enter Rate", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "The Field Amount Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtAmount.Focus();
                return;
            }
            if (ddlUnit.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Supplier", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Unit";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                ddlUnit.Focus();
                return;
            }


            if (dgAMCDetails.Rows.Count > 0)
            {
                for (int i = 0; i < dgAMCDetails.Rows.Count; i++)
                {
                    string PROD_NAME = ((Label)(dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME"))).Text;
                    if (ItemUpdateIndex == "-1")
                    {
                        if (PROD_NAME == txtProductName.Text)
                        {
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Same Product In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                    }
                    else
                    {
                        if (PROD_NAME == txtProductName.Text && ItemUpdateIndex != i.ToString())
                        {
                            ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Same Product In Table In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                            return;
                        }
                    }
                }
            }

            if (dt2.Rows.Count > 0)
            {
                for (int i = dt2.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt2.Rows[i][1] == DBNull.Value)
                        dt2.Rows[i].Delete();
                }
                dt2.AcceptChanges();
            }
            NoGen(out File_Code, out File_Code1);


            #region datatable structure
            if (dt2.Columns.Count == 0)
            {
                dt2.Columns.Add("WOD_PROD_NAME");
                dt2.Columns.Add("WOD_PROD_DESC");

                dt2.Columns.Add("WOD_PREV_MAINTAIN_DEC");
                dt2.Columns.Add("WOD_UOM_CODE");
                dt2.Columns.Add("WOD_UOM_NAME");
                dt2.Columns.Add("WOD_QTY");
                dt2.Columns.Add("WOD_RATE");
                dt2.Columns.Add("WOD_TOT_AMT");
                dt2.Columns.Add("WOD_CODE");
            }
            #endregion

            #region add control value to Dt
            dr = dt2.NewRow();
            dr["WOD_PROD_NAME"] = txtProductName.Text;
            dr["WOD_PROD_DESC"] = txtProducDesc.Text;
            dr["WOD_PREV_MAINTAIN_DEC"] = txtPreventiveDesc.Text;
            dr["WOD_UOM_CODE"] = ddlUnit.SelectedValue;
            dr["WOD_UOM_NAME"] = ddlUnit.SelectedItem.Text;
            dr["WOD_QTY"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtQty.Text)), 2));
            dr["WOD_RATE"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtRate.Text)), 2));
            dr["WOD_TOT_AMT"] = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtAmount.Text)), 2));
            dr["WOD_CODE"] = File_Code1.ToString();
            

            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (dt2.Rows.Count > 0)
                {
                    dt2.Rows.RemoveAt(Index);
                    dt2.Rows.InsertAt(dr, Index);
                }
            }
            else
            {
                dt2.Rows.Add(dr);
            }
            #endregion

            #region Binding data to Grid
            dgAMCDetails.Visible = true;
            dgAMCDetails.DataSource = dt2;
            dgAMCDetails.DataBind();
            dgAMCDetails.Enabled = true;


            #endregion
            GetTotal();
            clearDetail();
            

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "btnInsert_Click", Ex.Message);
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
            CommonClasses.SendError("AMC Transaction", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            txtProductName.Text = "";
            txtProducDesc.Text = ""; ;
            txtPreventiveDesc.Text = "";
            txtQty.Text = "";
            txtRate.Text = "";
            ddlUnit.SelectedIndex = 0;
            txtAmount.Text = "";
            InitialRow_AppDoc();
            dgAttachment.Visible = false;
            str = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region BlankGridView

    private void BlankGridView()
    {
        dt2.Clear();
        if (dt2.Columns.Count == 0)
        {

            dt2.Columns.Add("WOD_PROD_NAME");
            dt2.Columns.Add("WOD_PROD_DESC");

            dt2.Columns.Add("WOD_PREV_MAINTAIN_DEC");
            dt2.Columns.Add("WOD_UOM_CODE");
            dt2.Columns.Add("WOD_UOM_NAME");
            dt2.Columns.Add("WOD_QTY");
            dt2.Columns.Add("WOD_RATE");
            dt2.Columns.Add("WOD_TOT_AMT");
            dt2.Columns.Add("WOD_CODE");
        }
        dt2.Rows.Add(dt2.NewRow());


        dgAMCDetails.Visible = true;
        dgAMCDetails.DataSource = dt2;
        dgAMCDetails.DataBind();
        dgAMCDetails.Enabled = false;


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

                int amc_no = 0;
                DataTable dt = new DataTable();
                dt = CommonClasses.Execute("Select isnull(max(WO_AMC_NO),0) as WO_AMC_NO FROM WORK_AMC_ORDER_MASTER WHERE WO_CM_CODE= " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and WO_TYPE='AMC'");
                if (dt.Rows.Count > 0)
                {
                    amc_no = Convert.ToInt32(dt.Rows[0]["WO_AMC_NO"]);
                    amc_no = amc_no + 1;
                }
                if (CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_MASTER (WO_AMC_NO,WO_PO_DATE,WO_P_CODE,WO_TYPE,WO_CM_CODE,WO_REF,WO_CONTACT_PER,WO_PHONE_NO,WO_DELIVERY_SCHD,WO_PAY_TERM,WO_DELIVER_TO,WO_FRIEGHT_TERM,WO_GARUNTEE_TERM,WO_TRANSPORTOR,WO_NOTE,WO_SER_TAX_PER,WO_TAX_APPLICABLE,WO_GRAND_TOT,WO_UM_CODE,WO_FROM_DATE,WO_TO_DATE)VALUES('" + amc_no + "','" + Convert.ToDateTime(txtAMCDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlSupplier.SelectedValue + "', 'AMC' ,'" + Convert.ToInt32(Session["CompanyCode"]) + "','" + txtReferance.Text + "','" + txtContactPerson.Text + "','" + txtPhoneNo.Text + "','" + txtDeliverySchedule.Text + "','" + txtPaymentTerm.Text + "','" + txtDeliveryTo.Text + "','" + txtFreightTermsg.Text + "','" + txtGuranteeWaranty.Text + "','" + txtTranspoter.Text + "','" + txtNote.Text + "','" + txtServiceTax.Text + "','" + txtTaxApplicable.Text + "','" + txtFinalTotalAmount.Text + "','" + Convert.ToInt32(Session["UserCode"]) + "','" + Convert.ToDateTime(txtfromDate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "')"))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(WO_AMC_CODE) from WORK_AMC_ORDER_MASTER");
                    for (int i = 0; i < dgAMCDetails.Rows.Count; i++)
                    {
                       // CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_DETAIL (WOD_WO_AMC_CODE,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,WOD_QTY,WOD_RATE,WOD_TOT_AMT) values ('" + Code + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_DESC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PREV_MAINTAIN_DEC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_QTY")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_RATE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT")).Text + "')");
                        CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_DETAIL (WOD_WO_AMC_CODE,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,WOD_QTY,WOD_RATE,WOD_TOT_AMT,WOD_CODE,WOD_UOM_CODE) values ('" + Code + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_DESC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PREV_MAINTAIN_DEC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_QTY")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_RATE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_CODE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_UOM_CODE")).Text + "')");
                    }
                    CommonClasses.WriteLog("AMC Transaction", "Save", "AMC Transaction", Convert.ToString(amc_no), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    dt2.Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    txtAMCNo.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {

                if (CommonClasses.Execute1("UPDATE WORK_AMC_ORDER_MASTER SET WO_AMC_NO='" + txtAMCNo.Text + "',WO_PO_DATE='" + Convert.ToDateTime(txtAMCDate.Text).ToString("dd/MMM/yyyy") + "',WO_P_CODE='" + ddlSupplier.Text + "',WO_TYPE='AMC',WO_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "',WO_REF='" + txtReferance.Text + "',WO_CONTACT_PER='" + txtContactPerson.Text + "',WO_PHONE_NO='" + txtPhoneNo.Text + "',WO_DELIVERY_SCHD='" + txtDeliverySchedule.Text + "',WO_PAY_TERM='" + txtPaymentTerm.Text + "',WO_DELIVER_TO='" + txtDeliveryTo.Text + "',WO_FRIEGHT_TERM='" + txtFreightTermsg.Text + "',WO_GARUNTEE_TERM='" + txtGuranteeWaranty.Text + "',WO_TRANSPORTOR='" + txtTranspoter.Text + "',WO_NOTE='" + txtNote.Text + "',WO_SER_TAX_PER='" + txtServiceTax.Text + "',WO_TAX_APPLICABLE='" + txtTaxApplicable.Text + "',WO_GRAND_TOT='" + txtFinalTotalAmount.Text + "',WO_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "',WO_FROM_DATE='" + Convert.ToDateTime(txtfromDate.Text).ToString("dd/MMM/yyyy") + "',WO_TO_DATE='" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' where WO_AMC_CODE='" + mlCode + "'"))
                { 

                    result = CommonClasses.Execute1("DELETE FROM WORK_AMC_ORDER_DETAIL WHERE WOD_WO_AMC_CODE='" + mlCode + "'");
                    if (result)
                    {

                        for (int i = 0; i < dgAMCDetails.Rows.Count; i++)
                        {
                            //CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_DETAIL (WOD_WO_AMC_CODE,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,WOD_QTY,WOD_RATE,WOD_TOT_AMT) values ('" + mlCode + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_DESC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PREV_MAINTAIN_DEC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_QTY")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_RATE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT")).Text + "')");
                            CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_DETAIL (WOD_WO_AMC_CODE,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,WOD_QTY,WOD_RATE,WOD_TOT_AMT,WOD_CODE,WOD_UOM_CODE) values ('" + mlCode + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_DESC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PREV_MAINTAIN_DEC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_QTY")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_RATE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_CODE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_UOM_CODE")).Text + "')");
                        }

                        CommonClasses.RemoveModifyLock("WORK_AMC_ORDER_MASTER", "MODIFY", "WO_AMC_CODE", mlCode);
                        CommonClasses.WriteLog("AMC Transaction", "Update", "AMC Transaction", txtAMCNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        dt2.Rows.Clear();
                        result = true;
                    }


                    Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);
                }
                else
                {
                    //ShowMessage("#Avisos", "Record Not Saved", CommonClasses.MSG_Warning);

                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Not Saved";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    txtAMCNo.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("AMEND"))
            {
                int AMEND_COUNT = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                dt = CommonClasses.Execute("select isnull(WO_AM_COUNT,0) as WO_AM_COUNT from WORK_AMC_ORDER_MASTER WHERE WO_CM_CODE = '" + Convert.ToInt32(Session["CompanyCode"]) + "' and ES_DELETE=0 and WO_AMC_CODE='" + mlCode + "'");
                if (dt.Rows.Count > 0)
                {
                    AMEND_COUNT = Convert.ToInt32(dt.Rows[0]["WO_AM_COUNT"]);
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                if (AMEND_COUNT == 0)
                {
                    AMEND_COUNT = AMEND_COUNT + 1;
                }
                CommonClasses.Execute1("update  WORK_AMC_ORDER_MASTER set WO_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "',WO_AM_COUNT='" + AMEND_COUNT + "' WHERE WO_AMC_CODE='" + mlCode + "'");
                if (CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_AM_MASTER select * from WORK_AMC_ORDER_MASTER where WO_AMC_CODE='" + mlCode + "' "))
                {
                    string MatserCode = CommonClasses.GetMaxId("Select Max(WO_AM_CODE) from WORK_AMC_ORDER_AM_MASTER");
                    DataTable dtDetail = CommonClasses.Execute("select * from WORK_AMC_ORDER_DETAIL where WOD_WO_AMC_CODE='" + mlCode + "'");
                    for (int j = 0; j < dtDetail.Rows.Count; j++)
                    {
                        CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_AMD_DETAIL values('" + dtDetail.Rows[j]["WOD_CODE"] + "','" + dtDetail.Rows[j]["WOD_WO_AMC_CODE"] + "','" + dtDetail.Rows[j]["WOD_SR_NO"] + "','" + dtDetail.Rows[j]["WOD_PROD_NAME"] + "','" + dtDetail.Rows[j]["WOD_PROD_DESC"] + "','" + dtDetail.Rows[j]["WOD_PREV_MAINTAIN_DEC"] + "','" + dtDetail.Rows[j]["WOD_QTY"] + "','" + dtDetail.Rows[j]["WOD_RATE"] + "','" + dtDetail.Rows[j]["WOD_TOT_AMT"] + "',NULL,NULL,NULL,'" + dtDetail.Rows[j]["WOD_UOM_CODE"] + "','" + MatserCode + "')");
                    }

                    #region ModifyOriginalPO

                    if (CommonClasses.Execute1("UPDATE WORK_AMC_ORDER_MASTER SET WO_AMC_NO='" + txtAMCNo.Text + "',WO_PO_DATE='" + Convert.ToDateTime(txtAMCDate.Text).ToString("dd/MMM/yyyy") + "',WO_P_CODE='" + ddlSupplier.Text + "',WO_TYPE='AMC',WO_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "',WO_REF='" + txtReferance.Text + "',WO_CONTACT_PER='" + txtContactPerson.Text + "',WO_PHONE_NO='" + txtPhoneNo.Text + "',WO_DELIVERY_SCHD='" + txtDeliverySchedule.Text + "',WO_PAY_TERM='" + txtPaymentTerm.Text + "',WO_DELIVER_TO='" + txtDeliveryTo.Text + "',WO_FRIEGHT_TERM='" + txtFreightTermsg.Text + "',WO_GARUNTEE_TERM='" + txtGuranteeWaranty.Text + "',WO_TRANSPORTOR='" + txtTranspoter.Text + "',WO_NOTE='" + txtNote.Text + "',WO_SER_TAX_PER='" + txtServiceTax.Text + "',WO_TAX_APPLICABLE='" + txtTaxApplicable.Text + "',WO_GRAND_TOT='" + txtFinalTotalAmount.Text + "',WO_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "',WO_FROM_DATE='" + Convert.ToDateTime(txtfromDate.Text).ToString("dd/MMM/yyyy") + "',WO_TO_DATE='" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "' where WO_AMC_CODE='" + mlCode + "'"))
                    {
                        result = CommonClasses.Execute1("update  WORK_AMC_ORDER_AM_MASTER set WO_AM_DATE='" + System.DateTime.Now.Date.ToString("dd MMM yyyy") + "' WHERE WO_AM_CODE='" + mlCode + "' and WO_AM_COUNT='" + AMEND_COUNT + "'");
                        result = CommonClasses.Execute1("DELETE FROM WORK_AMC_ORDER_DETAIL WHERE WOD_WO_AMC_CODE='" + mlCode + "'");
                        if (result)
                        {

                            for (int i = 0; i < dgAMCDetails.Rows.Count; i++)
                            {
                                //CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_DETAIL (WOD_WO_AMC_CODE,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,WOD_QTY,WOD_RATE,WOD_TOT_AMT) values ('" + mlCode + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_DESC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PREV_MAINTAIN_DEC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_QTY")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_RATE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT")).Text + "')");
                                CommonClasses.Execute1("INSERT INTO WORK_AMC_ORDER_DETAIL (WOD_WO_AMC_CODE,WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,WOD_QTY,WOD_RATE,WOD_TOT_AMT,WOD_CODE,WOD_UOM_CODE) values ('" + mlCode + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_NAME")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PROD_DESC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_PREV_MAINTAIN_DEC")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_QTY")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_RATE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_CODE")).Text + "','" + ((Label)dgAMCDetails.Rows[i].FindControl("lblWOD_UOM_CODE")).Text + "')");
                            }

                            CommonClasses.RemoveModifyLock("WORK_AMC_ORDER_MASTER", "MODIFY", "WO_AMC_CODE", mlCode);
                            CommonClasses.WriteLog("AMC Transaction", "Update", "AMC Transaction", txtAMCNo.Text, mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            dt2.Rows.Clear();
                            result = true;
                        }

                        Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);
                    }

                    #endregion

                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("AMC Transaction", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtRate.Text);

            if (txtQty.Text == "")
            {
                txtQty.Text = "0.000";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            } 
            txtQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtQty.Text), 3));
            txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
           
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtQty_TextChanged
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtQty.Text);
            if (txtQty.Text == "")
            {
                txtQty.Text = "0.000";
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            txtQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 2));
            txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtRate.Text), 2));
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            txtRate.Focus();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "txtQty_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtfromDate_TextChanged
    protected void txtfromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
          string  Remark = "1) Agreement period start on " + txtfromDate.Text + " to " + txtToDate.Text + "." + Environment.NewLine + "2) All service request/calls for repair work must be attended within 24 hours. " + Environment.NewLine + "3) The release of payment will be made on quarterly basis (at the begining of next quarter)." + Environment.NewLine + " on the basis of satisfactory performance." + Environment.NewLine + " 4) The case of delay in attending to the complaints penalty will be levied." + Environment.NewLine + "5) The period will be reckoned from the time of lodging the complaint by telephone/fax/letter, etc." + Environment.NewLine + "";
            txtNote.Text = Remark.ToString();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "txtfromDate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtToDate_TextChanged
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string Remark = "1) Agreement period start on " + txtfromDate.Text + " to " + txtToDate.Text + "." + Environment.NewLine + "2) All service request/calls for repair work must be attended within 24 hours. " + Environment.NewLine + "3) The release of payment will be made on quarterly basis (at the begining of next quarter)." + Environment.NewLine + " on the basis of satisfactory performance." + Environment.NewLine + " 4) The case of delay in attending to the complaints penalty will be levied." + Environment.NewLine + "5) The period will be reckoned from the time of lodging the complaint by telephone/fax/letter, etc." + Environment.NewLine + "";
            txtNote.Text = Remark.ToString();

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "txtToDate_TextChanged", Ex.Message.ToString());
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
            // int n = no - 1;
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

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;

        if (dgAMCDetails.Rows.Count > 0)
        {
            for (int i = 0; i < dgAMCDetails.Rows.Count; i++)
            {
                string QED_AMT = ((Label)(dgAMCDetails.Rows[i].FindControl("lblWOD_TOT_AMT"))).Text;
                double Amount = Convert.ToDouble(QED_AMT);
                decTotal = decTotal + Amount;


            }
        }
        txtFinalTotalAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal), 2);





    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            LoadSupplier();
            InitialRow_AppDoc();
            dtPODetail.Clear();
            txtAMCDate.Attributes.Add("readonly", "readonly");
            txtfromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

            dt = CommonClasses.Execute("Select WO_AMC_CODE,WO_AMC_NO,WO_PO_DATE,WO_P_CODE,WO_TYPE,WO_REF,WO_CONTACT_PER,WO_PHONE_NO,WO_DELIVERY_SCHD,WO_PAY_TERM,WO_DELIVER_TO,WO_FRIEGHT_TERM,WO_GARUNTEE_TERM,WO_TRANSPORTOR,WO_NOTE,WO_TAX_NAME,WO_TAX_APPLICABLE,WO_GRAND_TOT,isnull(WO_SER_TAX_PER,0) as WO_SER_TAX_PER,WO_FROM_DATE,WO_TO_DATE FROM WORK_AMC_ORDER_MASTER where ES_DELETE=0 and WO_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and WO_AMC_CODE=" + mlCode + "");
            if (dt.Rows.Count > 0)
            {
                mlCode = Convert.ToInt32(dt.Rows[0]["WO_AMC_CODE"]);
                txtAMCNo.Text = dt.Rows[0]["WO_AMC_NO"].ToString();
                txtAMCDate.Text = Convert.ToDateTime(dt.Rows[0]["WO_PO_DATE"]).ToString("dd MMM yyyy");
                txtfromDate.Text = Convert.ToDateTime(dt.Rows[0]["WO_FROM_DATE"]).ToString("dd MMM yyyy");
                txtToDate.Text = Convert.ToDateTime(dt.Rows[0]["WO_TO_DATE"]).ToString("dd MMM yyyy");
                ddlSupplier.SelectedValue = dt.Rows[0]["WO_P_CODE"].ToString();

                txtReferance.Text = dt.Rows[0]["WO_REF"].ToString();
                txtContactPerson.Text = dt.Rows[0]["WO_CONTACT_PER"].ToString();
                txtPhoneNo.Text = dt.Rows[0]["WO_PHONE_NO"].ToString();

                txtDeliverySchedule.Text = dt.Rows[0]["WO_DELIVERY_SCHD"].ToString();
                txtPaymentTerm.Text = dt.Rows[0]["WO_PAY_TERM"].ToString();
                txtDeliveryTo.Text = dt.Rows[0]["WO_DELIVER_TO"].ToString();
                txtTranspoter.Text = dt.Rows[0]["WO_TRANSPORTOR"].ToString();
                txtFreightTermsg.Text = dt.Rows[0]["WO_FRIEGHT_TERM"].ToString();
                txtGuranteeWaranty.Text = dt.Rows[0]["WO_GARUNTEE_TERM"].ToString();
                txtNote.Text = dt.Rows[0]["WO_NOTE"].ToString();
                txtServiceTax.Text =string.Format("{0:0.00}", dt.Rows[0]["WO_SER_TAX_PER"]);
                txtFinalTotalAmount.Text = string.Format("{0:0.00}", dt.Rows[0]["WO_GRAND_TOT"]);

                dtPODetail = CommonClasses.Execute("select WOD_PROD_NAME,WOD_PROD_DESC,WOD_PREV_MAINTAIN_DEC,cast(WOD_QTY as numeric(10,3)) as WOD_QTY,cast(WOD_RATE as numeric(20,2)) as WOD_RATE,cast(WOD_TOT_AMT as numeric(20,2)) as WOD_TOT_AMT,WOD_CODE,WOD_UOM_CODE,I_UOM_NAME as WOD_UOM_NAME FROM WORK_AMC_ORDER_MASTER,WORK_AMC_ORDER_DETAIL,ITEM_UNIT_MASTER WHERE WOD_WO_AMC_CODE=WO_AMC_CODE and I_UOM_CODE=WOD_UOM_CODE and WO_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and WOD_WO_AMC_CODE='" + mlCode + "'");

                if (dtPODetail.Rows.Count != 0)
                {
                    dgAMCDetails.DataSource = dtPODetail;
                    dgAMCDetails.DataBind();
                    dt2 = dtPODetail;
                    GetTotal();                   
                }
                DataTable dtAtt = CommonClasses.Execute("SELECT  WOA_FILE_NAME as FileName,WOA_FILE_PATH AS Download FROM WORK_AMC_ATTACHMENT WHERE  WOA_WO_AMC_CODE='" + mlCode + "'");
                if (dtAtt.Rows.Count != 0)
                {
                    dgAttachment.DataSource = dtAtt;
                    dgAttachment.DataBind();
                    for (int i = 0; i < dtAtt.Rows.Count; i++)
                    {
                        ((Label)(dgAttachment.Rows[i].FindControl("lblfilename"))).Text = dtAtt.Rows[i]["FileName"].ToString();
                        //((LinkButton)(dgAttachment.Rows[i].FindControl("lnkDownload"))).Text = dtAtt.Rows[i]["FileName"].ToString();
                        //dgAttachment.DataSource = dtAtt;
                        //dgAttachment.DataBind();                  
                        //dt2 = dtAtt; 
                    }
                }

            }

            if (str == "VIEW")
            {
                btnSubmit.Visible = false;
                btnInsert.Enabled = false;

                txtAMCDate.Enabled = false;
                txtfromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtQty.Enabled = false;
                txtAMCNo.Enabled = false;
                txtRate.Enabled = false;
                ddlUnit.Enabled = false;
                txtGuranteeWaranty.Enabled = false;
                txtAmount.Enabled = false;
                txtProductName.Enabled = false;
                txtProducDesc.Enabled = false;
                txtPreventiveDesc.Enabled = false;
                ddlSupplier.Enabled = false;
                txtDeliverySchedule.Enabled = false;
                txtTranspoter.Enabled = false;
                txtPaymentTerm.Enabled = false;
                txtDeliveryTo.Enabled = false;
                txtFreightTermsg.Enabled = false;
                txtNote.Enabled = false;
                dgAMCDetails.Enabled = false;
                txtServiceTax.Enabled = false;
            }
            else if (str == "MOD" || str == "AMEND")
            {
                ddlSupplier.Enabled = false;
                CommonClasses.SetModifyLock("WORK_AMC_ORDER_MASTER", "MODIFY", "WO_AMC_CODE", Convert.ToInt32(mlCode));

            }
      
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (txtAMCDate.Text == "")
            {
                flag = false;
            }
            else if (ddlSupplier.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtReferance.Text == "")
            {
                flag = false;
            }
            else if (txtContactPerson.Text == "")
            {
                flag = false;
            }
            else if (txtPhoneNo.Text == "")
            {
                flag = false;
            }
            //else if (dgMainPO.Enabled)
            //{
            //    flag = true;
            //}
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Tarnsaction", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec(); 
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("AMC transaction", "btnOk_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("WORK_AMC_ORDER_MASTER", "MODIFY", "WO_AMC_CODE", mlCode);
            }

            dt2.Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewAnnualMaintainseContract.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "CancelRecord", Ex.Message);
        }
    }
    #endregion

    #region dgAMCDetails_RowCommand
    protected void dgAMCDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgAMCDetails.Rows[Index];

            if (e.CommandName == "Delete")
            {
                // int rowindex = row.RowIndex;
                //dgSupplierPurchaseOrder.DeleteRow(Index);
                dt2.Rows.RemoveAt(Index);

                dgAMCDetails.DataSource = dt2;
                dgAMCDetails.DataBind();
            }
            if (e.CommandName == "Modify")
            {
               

                str = "Modify";
                ItemUpdateIndex = e.CommandArgument.ToString();
                txtProductName.Text = ((Label)(row.FindControl("lblWOD_PROD_NAME"))).Text;
                txtProducDesc.Text = ((Label)(row.FindControl("lblWOD_PROD_DESC"))).Text;
                txtPreventiveDesc.Text = ((Label)(row.FindControl("lblWOD_PREV_MAINTAIN_DEC"))).Text;
                txtQty.Text = ((Label)(row.FindControl("lblWOD_QTY"))).Text;
                txtRate.Text = ((Label)(row.FindControl("lblWOD_RATE"))).Text;
                txtAmount.Text = ((Label)(row.FindControl("lblWOD_TOT_AMT"))).Text;
                
               

            }



        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("AMC Transaction", "dgAMCDetails_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgAMCDetails_RowDeleting
    protected void dgAMCDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region AddRowDoc
    protected void btnAddDoc_Click(object sender, EventArgs e)
    {
        dgAttachment.Visible = true;
        if (dgAttachment.Rows.Count == 0)
        {
            InitialRow_AppDoc();

        }
        else
        {
            AddNewRowToAppDoc();
            //DataTable dt = new DataTable();
            //DataRow dr = null;
            //dt.Columns.Add(new DataColumn("Browse", typeof(string)));
            //dt.Columns.Add(new DataColumn("Upload", typeof(string)));
            //dt.Columns.Add(new DataColumn("FileName", typeof(string)));
            //dt.Columns.Add(new DataColumn("Download", typeof(string)));

            //dr = dt.NewRow();
            //dr["Browse"] = string.Empty;
            //dr["Upload"] = string.Empty;
            //dr["FileName"] = string.Empty;
            //dr["Download"] = string.Empty;
            //dt.Rows.Add(dr);
            //ViewState["AppDocumentTable"] = dt;
            //dgAttachment.DataSource = dt;
            //dgAttachment.DataBind();
        }
    }
    #endregion AddRowDoc

    #region InitialRow_AppDocument
    private void InitialRow_AppDoc()
    {

        DataTable dt = new DataTable();
        DataRow dr = null;
        dt.Columns.Add(new DataColumn("Browse", typeof(string)));
        dt.Columns.Add(new DataColumn("Upload", typeof(string)));
        dt.Columns.Add(new DataColumn("FileName", typeof(string)));
        dt.Columns.Add(new DataColumn("Download", typeof(string)));

        dr = dt.NewRow();
        dr["Browse"] = string.Empty;
        dr["Upload"] = string.Empty;
        dr["FileName"] = string.Empty;
        dr["Download"] = string.Empty;
        dt.Rows.Add(dr);
        ViewState["AppDocumentTable"] = dt;
        dgAttachment.DataSource = dt;
        dgAttachment.DataBind();
    }
    #endregion InitialRow_AppDocument

    #region AddNewRowToAppDoc
    private void AddNewRowToAppDoc()
    {

        int rowIndex = 0;
        if (ViewState["AppDocumentTable"] != null)
        {
            dtCurrentTable = (DataTable)ViewState["AppDocumentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {

                    Label box1 = (Label)dgAttachment.Rows[rowIndex].Cells[0].FindControl("lblfilename");
                    FileUpload box2 = (FileUpload)dgAttachment.Rows[rowIndex].Cells[1].FindControl("imgUpload");
                    LinkButton box3 = (LinkButton)dgAttachment.Rows[rowIndex].Cells[2].FindControl("btnupload");


                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["FileName"] = string.Empty;
                    drCurrentRow["Browse"] = string.Empty;
                    drCurrentRow["UpLoad"] = string.Empty;

                    rowIndex++;
                }

                //add new row to DataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Store the current data to ViewState
                ViewState["AppDocumentTable"] = dtCurrentTable;
                //Rebind the Grid with the current data
                dgAttachment.DataSource = dtCurrentTable;
                dgAttachment.DataBind();
            }
        }
        else
        {
            //  Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        //SetPreviousData_AppDoc();
    }
    #endregion AddNewRowToAppDoc

    #region dgAttachment_RowUpdating Upload File
    protected void dgAttachment_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            FileLoactionCreate();

            GridViewRow row = dgAttachment.Rows[e.RowIndex];
            //Label lblfilename = (Label)row.FindControl("lblfilename");

            FileUpload flUp = (FileUpload)row.FindControl("imgUpload");
            string directory = Server.MapPath(@"~/UpLoadPath/AnnualMaintainceContract/" + File_Code + "/" + File_Code1 + "");

            string fileName = Path.GetFileName(flUp.PostedFile.FileName);
            string fName = "";
            fName = txtProductName.Text.Trim();
            fName = fName + txtProducDesc.Text.Trim();
            //fileName = fName + fileName;
            string extension;

            extension = Path.GetExtension(fileName);
            fName = fName + extension;
            //string fExtension = Path.GetExtension(flUp.PostedFile.FileName);
            //string fullfile = fileName + fExtension;

            flUp.SaveAs(Path.Combine(directory, fName));

            ((Label)(dgAttachment.Rows[e.RowIndex].FindControl("lblfilename"))).Text = fName.ToString();

            CommonClasses.Execute1("INSERT INTO WORK_AMC_ATTACHMENT (WOA_WO_AMC_CODE, WOA_WOD_CODE, WOA_FILE_NAME, WOA_FILE_PATH) VALUES ('" + File_Code + "','" + File_Code1 + "','" + fName + "','" + "~/UpLoadPath/AnnualMaintainceContract/" + File_Code + "" + '/' + "" + File_Code1 + "" + '/' + "" + fName + "')");
        }
        catch
        {


        }
    }

    #endregion  dgAttachment_RowUpdating Upload File

    #region FileLoactionCreate
    void FileLoactionCreate()
    {
        //DataTable dt = new DataTable();
        //if (Request.QueryString[0].Equals("MODIFY"))
        //{
        //    File_Code = mlCode;
        //}
        //else
        //{

        //    dt = CommonClasses.Execute("select isnull(max(WO_AMC_CODE+1),0)as Code from WORK_AMC_ORDER_MASTER");
        //    if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
        //    {
        //        int i = dgAMCDetails.Rows.Count;
        //        i = i + 1;
        //        dtDetails = CommonClasses.Execute("select isnull(max(WOD_CODE+ i),0)as Code from WORK_AMC_ORDER_DETAIL");
        //        File_Code1 = int.Parse(dtDetails.Rows[0]["Code"].ToString());
        //        DataTable dt1 = CommonClasses.Execute(" SELECT IDENT_CURRENT('WORK_AMC_ORDER_MASTER')+1");
        //        if (dt.Rows[0][0].ToString() == "-2147483647")
        //        {
        //            File_Code = -2147483648;
        //        }
        //        else
        //        {

        //            File_Code = int.Parse(dt.Rows[0][0].ToString());
        //        }
        //    }
        //    else
        //    {
        //        string Prod = ((Label)(dgAMCDetails.Rows[0].FindControl("lblWOD_PROD_NAME"))).Text;
        //        int a = 0;
        //        if (Prod == "")
        //        {
        //            a = 0;
        //        }
        //        else
        //        {
        //            a = dgAMCDetails.Rows.Count;
        //        }



        //        dtDetails = CommonClasses.Execute("select isnull(max(WOD_CODE),0)as Code from WORK_AMC_ORDER_DETAIL");
        //        File_Code1 = int.Parse(dtDetails.Rows[0]["Code"].ToString());
        //        File_Code1 = File_Code1 + a;


        //        File_Code = int.Parse(dt.Rows[0][0].ToString());
        //    }
        //}

        NoGen(out File_Code, out File_Code1);
        string sDirPath = Server.MapPath(@"~/UpLoadPath/AnnualMaintainceContract/" + File_Code + "/" + File_Code1 + "");

        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
        }
    }

    #endregion


    #region NoGen
    public void NoGen(out int filecode, out int filecode1)
    {
        DataTable dt = new DataTable();
        int a = 0;
        dt = CommonClasses.Execute("select isnull(max(WO_AMC_CODE),0)+1 as Code from WORK_AMC_ORDER_MASTER");
        if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
        {
            int i = dgAMCDetails.Rows.Count;
            i = i + 1;
            dtDetails = CommonClasses.Execute("select isnull(max(WOD_CODE),0)+ 1 as Code from WORK_AMC_ORDER_DETAIL");
            File_Code1 = int.Parse(dtDetails.Rows[0]["Code"].ToString());
            DataTable dt1 = CommonClasses.Execute(" SELECT IDENT_CURRENT('WORK_AMC_ORDER_MASTER')+1");
            if (dt.Rows[0][0].ToString() == "-2147483647")
            {
                File_Code = -2147483648;
            }
            else
            {
                File_Code = int.Parse(dt.Rows[0][0].ToString());
            }
        }
        else
        {
            string Prod = ((Label)(dgAMCDetails.Rows[0].FindControl("lblWOD_PROD_NAME"))).Text;

            if (Prod == "")
            {
                a = 0;
            }
            else
            {
                a = dgAMCDetails.Rows.Count;
            }
        }
        if (Request.QueryString[0].Equals("MODIFY"))
        {
            File_Code = Convert.ToInt32(Request.QueryString[1]);
            dtDetails = CommonClasses.Execute("select isnull(max(WOD_CODE),0)+1 as Code from WORK_AMC_ORDER_DETAIL");
            File_Code1 = int.Parse(dtDetails.Rows[0]["Code"].ToString());
            File_Code1 = File_Code1 + a;

        }
        else
        {
            dtDetails = CommonClasses.Execute("select isnull(max(WOD_CODE),0)+1 as Code from WORK_AMC_ORDER_DETAIL");
            File_Code1 = int.Parse(dtDetails.Rows[0]["Code"].ToString());
            File_Code1 = File_Code1 + a;


            File_Code = int.Parse(dt.Rows[0][0].ToString());
        }

        filecode = File_Code;
        filecode1 = File_Code1;
    }
    #endregion


    #region lnkDelete_Click
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            // //Index = Convert.ToInt32(e.CommandArgument.ToString());
            // GridViewRow row = dgAttachment.Rows[Index];
            //// //if (e.CommandName == "Delete")
            //// //{
           
            int rowindex = gvrow.RowIndex;

            string Prod = ((Label)(dgAttachment.Rows[rowindex].FindControl("lblfilename"))).Text;            
            CommonClasses.Execute1("DELETE FROM WORK_AMC_ATTACHMENT WHERE WOA_FILE_NAME='" + Prod + "'");
             dgAttachment.DeleteRow(rowindex);
            //dtCurrentTable.Rows.RemoveAt(rowindex);
            //dgAttachment.DataSource = dtCurrentTable;
            //dgAttachment.DataBind();
            DataTable dtAtt = CommonClasses.Execute("SELECT  WOA_FILE_NAME as FileName,WOA_FILE_PATH AS Download FROM WORK_AMC_ATTACHMENT WHERE  WOA_WO_AMC_CODE='" + mlCode + "'");
            if (dtAtt.Rows.Count != 0)
            {
                dgAttachment.DataSource = dtAtt;
                dgAttachment.DataBind();
                for (int i = 0; i < dtAtt.Rows.Count; i++)
                {
                    ((Label)(dgAttachment.Rows[i].FindControl("lblfilename"))).Text = dtAtt.Rows[i]["FileName"].ToString();
                    //((LinkButton)(dgAttachment.Rows[i].FindControl("lnkDownload"))).Text = dtAtt.Rows[i]["FileName"].ToString();
                    //dgAttachment.DataSource = dtAtt;
                    //dgAttachment.DataBind();                  
                    //dt2 = dtAtt; 
                }
            }

            ViewState["AppDocumentTable"] = dtCurrentTable;
            ////// }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region lnkDownload_Click
    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkbtn = sender as LinkButton;
            GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
            string filePath = dgAttachment.DataKeys[gvrow.RowIndex].Value.ToString();
            FileInfo File = new FileInfo(filePath);
            string filename = File.Name;
            if (filePath != "")
            {
                //Response.ContentType = "image/jpg";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
        }
        catch
        {


        }
    }
    #endregion

    #region dgAttachment_Deleting
    protected void dgAttachment_Deleting(object sender, GridViewDeleteEventArgs e)
    { }
    #endregion



}
