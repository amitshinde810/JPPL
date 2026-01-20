using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CrystalDecisions.Enterprise;
using System.IO;

public partial class IRN_ADD_IRNCustomerRejection : System.Web.UI.Page
{
    //Created by Suja
    #region General Declaration
    static int mlCode = 0;
    DataRow dr;
    public static string str = "";
    public static int Index = 0;
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    static int File_Code = 0;

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
                    ViewState["Qty"] = 0;   // use this variable for compare actual rej inward qty and Manual rej qty
                    ViewState["GrdSum"] = 0; // Sum of Manual Rej Qty
                    ViewState["RejQty"] = 0;
                    ViewState["ImageUpload"] = "0";
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    str = "";
                    ViewState["str"] = str;
                    BlankGrid();
                    txtGRNDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                    txtDocdate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                    txtGRNDate.Attributes.Add("readonly", "readonly");
                    txtDocdate.Attributes.Add("readonly", "readonly");
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
                    //when click on gridview Open button that time this code will work
                    else if (Request.QueryString[0].Equals("UpdateStatus"))
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["No"] = Convert.ToInt32(Request.QueryString[2]).ToString();
                        ViewState["date"] = Convert.ToDateTime(Request.QueryString[3]);
                        ViewState["Icode"] = Convert.ToInt32(Request.QueryString[4]).ToString();
                        ViewState["Pcode"] = Convert.ToInt32(Request.QueryString[5]).ToString();
                        ViewRec("UPDATE");

                        DataTable dtId = CommonClasses.Execute("SELECT CRM_CODE  FROM IRN_CUSTOMER_REJECTION_MASTER , IRN_CUSTOMER_REJECTION_DETAIL  where CRM_CODE =CRD_CRM_CODE AND ES_DELETE=0  AND CRD_I_CODE='" + ViewState["Icode"].ToString() + "' AND  CRM_P_CODE='" + ViewState["Pcode"].ToString() + "' AND  CRM_CR_CODE='" + ViewState["mlCode"].ToString() + "'");

                        if (dtId.Rows.Count > 0)
                        {
                            ViewState["ImageUpload"] = dtId.Rows[0]["CRM_CODE"].ToString();
                        }

                        //Disable all controls on Open status click
                        txtGRNDate.Enabled = false;
                        ddlParty.Enabled = false;
                        txtComplaintNo.Enabled = false;
                        ddlItemCode.Enabled = false;
                        ddlItemName.Enabled = false;
                        ddlUOM.Enabled = false;
                        txtQTy.Enabled = false;
                        txtReason.Enabled = false;
                        BtnInsert.Enabled = false;
                        ddlType.Enabled = false;
                        txtRejQty.Enabled = false;

                        //When griview is empty that time disabled the modify and delete buttons
                        if (((DataTable)ViewState["dt2"]).Rows.Count == 0)
                        {
                            foreach (GridViewRow gvr in dgIRN.Rows)
                            {
                                LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                                lnkDelete.Enabled = false;
                                LinkButton lnkSelect = (LinkButton)(gvr.FindControl("lnkSelect"));
                                lnkSelect.Enabled = false;
                            }
                        }
                    }
                    else if (Request.QueryString[0].Equals("Add"))//when click on gridview add button that time this code will work
                    {
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewState["No"] = Convert.ToInt32(Request.QueryString[2]).ToString();
                        ViewState["date"] = Convert.ToDateTime(Request.QueryString[3]);
                        ViewState["Icode"] = Convert.ToInt32(Request.QueryString[4]).ToString();
                        ViewState["Pcode"] = Convert.ToInt32(Request.QueryString[5]).ToString();
                        ViewRec("Add");
                    }

                    loadItem();
                    loadunit();
                    LoadParty();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("IRN Customer Rejection", "PageLoad", ex.Message);
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
            if (ddlParty.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Customer";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (txtComplaintNo.Text == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter Complaint No.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if (Request.QueryString[0].Equals("Add"))
            {
                DataTable dtCheckEist = CommonClasses.Execute("select CRM_CODE,CRM_P_CODE,CRM_COMPLAINT_NO from IRN_CUSTOMER_REJECTION_MASTER where ES_DELETE=0 and CRM_CM_COMP_CODE=" + Session["CompanyCode"] + " and CRM_P_CODE='" + ddlParty.SelectedValue + "' AND CRM_COMPLAINT_NO='" + txtComplaintNo.Text + "'");
                if (dtCheckEist.Rows.Count > 0)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Complaint No. Already Exist For This Customer";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }
            if (((Label)dgIRN.Rows[0].FindControl("lblCRD_I_CODE")).Text != "")
            {
                #region Comment Data from Rejection Inward
                //DataTable dtInwdQty = new DataTable();
                //dtInwdQty = CommonClasses.Execute("SELECT CD_CHALLAN_QTY,CD_RECEIVED_QTY,CR_GIN_DATE FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_I_CODE='" + Request.QueryString[4].ToString() + "' AND CR_GIN_NO='" + txtGRNno.Text + "' AND CR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'");
                //if (dtInwdQty.Rows.Count > 0)
                //{
                //    txtQTy.Text = dtInwdQty.Rows[0]["CD_CHALLAN_QTY"].ToString();
                //}
                //double qty = Convert.ToDouble(txtQTy.Text);
                //double RQty = Convert.ToDouble((Label)dgIRN.Rows[0].FindControl("lblCRD_REJ_QTYGrandTotal"));
                // double AQty = Convert.ToDouble((Label)dgIRN.Rows[0].FindControl("lblTotal"));

                #endregion

                if (Request.QueryString[0].Equals("UpdateStatus"))
                {

                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        TextBox txtCRD_ROUTE_CAUSE = (TextBox)dgIRN.Rows[i].FindControl("txtCRD_ROUTE_CAUSE");
                        TextBox txtCRD_ACTION_TAKEN = ((TextBox)(dgIRN.Rows[i].FindControl("txtCRD_ACTION_TAKEN")));
                        if (txtCRD_ROUTE_CAUSE.Text.Trim() == "" || txtCRD_ACTION_TAKEN.Text.Trim() == "")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Please Enter Action And Route Cause";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            ((TextBox)dgIRN.Rows[i].FindControl("txtCRD_ROUTE_CAUSE")).Focus();
                            return;
                        }

                        if (((Label)(dgIRN.Rows[i].FindControl("lblCRD_IS_REQUIRED"))).Text.Trim() == "YES")
                        {
                            if (((Label)(dgIRN.Rows[i].FindControl("lblfilename"))).Text.Trim() == "")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Please upload 8D Report";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                ((Label)(dgIRN.Rows[i].FindControl("lblfilename"))).Focus();
                                return;
                            }
                        }
                        if (((Label)(dgIRN.Rows[i].FindControl("lblCRD_EVIDENCE_REQUIRED"))).Text.Trim() == "YES")
                        {
                            if (((Label)(dgIRN.Rows[i].FindControl("lblCRD_EVIDENCE"))).Text.Trim() == "")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Please upload Evidence";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                ((Label)(dgIRN.Rows[i].FindControl("lblCRD_EVIDENCE"))).Focus();
                                return;
                            }
                        }
                        if (((Label)(dgIRN.Rows[i].FindControl("lblCRD_DOCUMENT_REQUIRED"))).Text.Trim() == "YES")
                        {
                            if (((Label)(dgIRN.Rows[i].FindControl("lblCRD_DOCUMENT"))).Text.Trim() == "")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Please upload 8D Report";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                                ((Label)(dgIRN.Rows[i].FindControl("lblCRD_DOCUMENT_REQUIRED"))).Focus();
                                return;
                            }
                        }
                    }
                }

                double GrdSum = 0, Qty = 0;
                for (int i = 0; i < dgIRN.Rows.Count; i++)
                {
                    GrdSum = GrdSum + Convert.ToDouble(((Label)dgIRN.Rows[i].FindControl("lblCRD_REJ_QTY")).Text);

                }
                if (Convert.ToDouble(ViewState["Qty"]) == 0.0 || Convert.ToDouble(ViewState["Qty"]) == 0)
                {
                    Qty = Qty + Convert.ToDouble(((Label)dgIRN.Rows[0].FindControl("lblCRD_QTY")).Text);
                    ViewState["Qty"] = Qty;
                }

                ViewState["GrdSum"] = GrdSum;
                if (ViewState["Qty"].ToString() == ViewState["GrdSum"].ToString())
                {
                    SaveRec();
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Rejection Qty and Rejection Inward Qty should be same...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
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
            CommonClasses.SendError("IRN Customer Rejection", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("IRN Customer Rejection", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("IRN Customer Rejection", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("IRN_CUSTOMER_REJECTION_MASTER", "MODIFY", "CRM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection", "btnCancel_Click", Ex.Message);
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
                CommonClasses.RemoveModifyLock("IRN_CUSTOMER_REJECTION_MASTER", "MODIFY", "CRM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection", "btnClose_Click", Ex.Message);
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
            if (str == "VIEW")
            {
                dt = CommonClasses.Execute("select CRM_CODE,CRM_NO,CRM_DATE,CRM_P_CODE,CRM_COMPLAINT_NO,CRM_NOCHAR,CRM_STATUS,CRM_CR_CODE,CRM_DOC_DATE from IRN_CUSTOMER_REJECTION_MASTER where CRM_CODE='" + Convert.ToInt32(Request.QueryString[1]).ToString() + "' and ES_DELETE=0 and CRM_CM_COMP_CODE='" + Session["CompanyCode"] + "'");
                if (dt.Rows.Count > 0)
                {
                    txtGRNno.Text = dt.Rows[0]["CRM_NOCHAR"].ToString();
                    txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["CRM_DATE"].ToString()).ToString("dd/MMM/yyyy");
                    txtComplaintNo.Text = dt.Rows[0]["CRM_COMPLAINT_NO"].ToString();
                    LoadParty();
                    ddlParty.SelectedValue = dt.Rows[0]["CRM_P_CODE"].ToString();
                    txtDocdate.Text = Convert.ToDateTime(dt.Rows[0]["CRM_DOC_DATE"].ToString()).ToString("dd/MMM/yyyy");

                }
                DataTable dtdetails = new DataTable();
                dtdetails = CommonClasses.Execute("select CRD_CRM_CODE,CRD_I_CODE,I_CODENO,I_NAME,CRD_UOM,I_UOM_NAME,CRD_QTY,CRD_REJ_QTY,CRD_TYPE,CRD_TYPE as CRD_TYPE1,CRD_REASON,CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE,CRD_FILE, CASE when ISNULL(CRD_IS_REQUIRED,0) =1 then 'YES' Else 'NO' END AS CRD_IS_REQUIRED,CRD_DOCUMENT,CRD_EVIDENCE, CASE when ISNULL(CRD_DOCUMENT_REQUIRED,0) =1 then 'YES' Else 'NO' END AS CRD_DOCUMENT_REQUIRED, CASE when ISNULL(CRD_EVIDENCE_REQUIRED,0) =1 then 'YES' Else 'NO' END AS CRD_EVIDENCE_REQUIRED from IRN_CUSTOMER_REJECTION_DETAIL,IRN_CUSTOMER_REJECTION_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER where I_CODE=CRD_I_CODE and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND CRM_CODE=CRD_CRM_CODE and CRD_CRM_CODE='" + Convert.ToInt32(Request.QueryString[1]).ToString() + "' and CRM_CM_COMP_CODE='" + Session["CompanyCode"] + "'");

                if (dtdetails.Rows.Count > 0)
                {
                    dgIRN.Enabled = true;
                    ViewState["dt2"] = dtdetails;
                    dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
                    dgIRN.DataBind();
                }
            }
            if (str == "UPDATE")
            {
                dt = CommonClasses.Execute("SELECT CRM_CODE,CRM_NO,CRM_DATE,CRM_P_CODE,CRM_COMPLAINT_NO,CRM_NOCHAR,CRM_DOC_DATE FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL WHERE (ES_DELETE = 0) AND CRM_P_CODE='" + Convert.ToInt32(Request.QueryString[5]).ToString() + "' AND CRM_CR_CODE='" + (Request.QueryString[1]).ToString() + "' and CRD_CRM_CODE=CRM_CODE and CRD_I_CODE='" + Convert.ToInt32(Request.QueryString[4]).ToString() + "'");

                if (dt.Rows.Count > 0)
                {
                    txtGRNno.Text = dt.Rows[0]["CRM_NOCHAR"].ToString();
                    txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["CRM_DATE"].ToString()).ToString("dd/MMM/yyyy");
                    txtComplaintNo.Text = dt.Rows[0]["CRM_COMPLAINT_NO"].ToString();
                    LoadParty();
                    ddlParty.SelectedValue = dt.Rows[0]["CRM_P_CODE"].ToString();
                }
                DataTable dtdetails = new DataTable();
                dtdetails = CommonClasses.Execute("select CRD_CRM_CODE,CRD_I_CODE,I_UOM_NAME,I_NAME,I_CODENO,CRD_QTY,CRD_REASON,IRN_CUSTOMER_REJECTION_DETAIL.CRD_UOM,CRD_DEF_OBSERVED,CRD_ROUTE_CAUSE,CRD_ACTION_TAKEN,CRD_FILE,CRD_REJ_QTY,CRD_TYPE,CRD_TYPE as CRD_TYPE1, CASE when ISNULL(CRD_IS_REQUIRED,0) =1 then 'YES' Else 'NO' END AS CRD_IS_REQUIRED , CASE when ISNULL(CRD_EVIDENCE_REQUIRED,0) =1 then 'YES' Else 'NO' END AS CRD_EVIDENCE_REQUIRED, CASE when ISNULL(CRD_DOCUMENT_REQUIRED,0) =1 then 'YES' Else 'NO' END AS CRD_DOCUMENT_REQUIRED,CRD_EVIDENCE,CRD_DOCUMENT from IRN_CUSTOMER_REJECTION_DETAIL,IRN_CUSTOMER_REJECTION_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER where IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 and ITEM_MASTER.ES_DELETE=0 and IRN_CUSTOMER_REJECTION_MASTER.CRM_CODE=IRN_CUSTOMER_REJECTION_DETAIL.CRD_CRM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and ITEM_UNIT_MASTER.ES_DELETE=0 and IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE and CRD_I_CODE='" + Convert.ToInt32(Request.QueryString[4]).ToString() + "' AND CRM_CR_CODE='" + (Request.QueryString[1]).ToString() + "'");

                if (dtdetails.Rows.Count > 0)
                {
                    dgIRN.Enabled = true;
                    ViewState["dt2"] = dtdetails;
                    dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
                    dgIRN.DataBind();

                    //  dgIRN.FooterRow.Cells[7].Text = "Total";
                }
                if (dgIRN.Rows.Count > 0)
                {
                    foreach (GridViewRow gr in dgIRN.Rows)
                    {
                        dgIRN.Enabled = true;
                        dgIRN.Columns[11].Visible = false;//Type
                        dgIRN.Columns[12].Visible = true;//Reason
                        // dgIRN.Columns[13].Visible = true;//Defect Observed
                        dgIRN.Columns[14].Visible = true;//Action
                        dgIRN.Columns[15].Visible = true;//Route Cause
                        dgIRN.Columns[16].Visible = true;//file upload control
                        dgIRN.Columns[17].Visible = true;//Upload Button
                        dgIRN.Columns[18].Visible = true;//View button
                        dgIRN.Columns[21].Visible = true;//file upload Evidence
                        dgIRN.Columns[22].Visible = true;//Upload Button Evidence
                        dgIRN.Columns[23].Visible = true;//View button Evidence
                        dgIRN.Columns[26].Visible = true;//Upload Button Document
                        dgIRN.Columns[27].Visible = true;//View button Document
                        dgIRN.Columns[28].Visible = true;//file upload Document

                        LinkButton lnkModify = (LinkButton)(gr.FindControl("lnkSelect"));
                        lnkModify.Enabled = false;
                        LinkButton inkDelete = (LinkButton)(gr.FindControl("lnkDelete"));
                        inkDelete.Enabled = false;
                        TextBox TxtDefOserv = (TextBox)(gr.FindControl("txtCRD_DEF_OBSERVED"));
                        TxtDefOserv.Enabled = false;

                        Label lblCRD_IS_REQUIRED = (Label)(gr.FindControl("lblCRD_IS_REQUIRED"));
                        Label lblCRD_EVIDENCE_REQUIRED = (Label)(gr.FindControl("lblCRD_EVIDENCE_REQUIRED"));
                        Label lblCRD_DOCUMENT_REQUIRED = (Label)(gr.FindControl("lblCRD_DOCUMENT_REQUIRED"));

                        LinkButton lnkView = (LinkButton)(gr.FindControl("lnkView"));
                        LinkButton btnupload = (LinkButton)(gr.FindControl("btnupload"));
                        FileUpload imgUpload = (FileUpload)(gr.FindControl("imgUpload"));

                        LinkButton lnkEvidenceView = (LinkButton)(gr.FindControl("lnkEvidenceView"));
                        LinkButton btnEvidenceupload = (LinkButton)(gr.FindControl("btnEvidenceupload"));
                        FileUpload EvidenceUpload = (FileUpload)(gr.FindControl("EvidenceUpload"));

                        LinkButton lnkDocumentView = (LinkButton)(gr.FindControl("lnkDocumentView"));
                        LinkButton btnDocumentupload = (LinkButton)(gr.FindControl("btnDocumentupload"));
                        FileUpload DocumentUpload = (FileUpload)(gr.FindControl("DocumentUpload"));

                        if (lblCRD_IS_REQUIRED.Text.ToUpper() == "No".ToUpper())
                        {
                            lnkView.Enabled = false; btnupload.Enabled = false; imgUpload.Enabled = false;
                        }
                        else
                        {
                            lnkView.Enabled = true; btnupload.Enabled = true; imgUpload.Enabled = true;
                        }
                        if (lblCRD_DOCUMENT_REQUIRED.Text.ToUpper() == "No".ToUpper())
                        {
                            lnkDocumentView.Enabled = false; btnDocumentupload.Enabled = false; DocumentUpload.Enabled = false;
                        }
                        else
                        {
                            lnkDocumentView.Enabled = true; btnDocumentupload.Enabled = true; DocumentUpload.Enabled = true;
                        }
                        if (lblCRD_EVIDENCE_REQUIRED.Text.ToUpper() == "No".ToUpper())
                        {
                            lnkEvidenceView.Enabled = false; btnEvidenceupload.Enabled = false; EvidenceUpload.Enabled = false;
                        }
                        else
                        {
                            lnkEvidenceView.Enabled = true; btnEvidenceupload.Enabled = true; EvidenceUpload.Enabled = true;
                        }
                    }

                    //txtGRNno.Text = Request.QueryString[2].ToString();
                    //txtGRNDate.Text = Request.QueryString[3].ToString();
                    ddlItemCode.SelectedValue = Request.QueryString[4].ToString();//ViewState["Icode"].ToString();
                    ddlItemName.SelectedValue = Request.QueryString[4].ToString();

                    DataTable dtU = new DataTable();
                    dtU = CommonClasses.Execute("SELECT * FROM IRN_CUSTOMER_REJECTION_DETAIL,IRN_CUSTOMER_REJECTION_MASTER WHERE CRD_CRM_CODE=CRM_CODE AND CRD_I_CODE='" + Request.QueryString[4] + "' AND CRM_DATE='" + Convert.ToDateTime(Request.QueryString[3]).ToString("dd MMM yyyy") + "' AND CRM_P_CODE='" + Request.QueryString[5] + "' AND CRM_CM_COMP_CODE='" + Session["CompanyCode"] + "'");
                    ddlType.SelectedValue = dtU.Rows[0]["CRD_TYPE"].ToString();
                    if (ddlType.SelectedValue == "0")//New
                    {
                        txtReason.Enabled = true;
                        ddlReason.Visible = false;
                        lblreason.Visible = false;
                    }
                    else//Repetative
                    {
                        txtReason.Enabled = false;
                        ddlReason.Visible = true;
                        lblreason.Visible = true;
                        LoadReason();
                        ddlReason.SelectedValue = dtU.Rows[0]["CRD_REASON"].ToString();
                        ddlReason.Enabled = false;
                    }
                    DataTable Dtunit = new DataTable();
                    Dtunit = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_INV_RATE FROM ITEM_MASTER,ITEM_UNIT_MASTER WHERE ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND ITEM_UNIT_MASTER.ES_DELETE=0 AND  ITEM_MASTER.ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CODE='" + Request.QueryString[4].ToString() + "' ORDER BY I_UOM_NAME");
                    if (Dtunit.Rows.Count > 0)
                    {
                        ddlUOM.SelectedValue = Dtunit.Rows[0]["I_UOM_CODE"].ToString();
                    }
                    ddlParty.SelectedValue = Request.QueryString[5].ToString();

                    DataTable dtInwdQty = new DataTable();
                    dtInwdQty = CommonClasses.Execute("SELECT CD_CHALLAN_QTY,CD_RECEIVED_QTY,CR_GIN_DATE FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_I_CODE='" + Request.QueryString[4].ToString() + "' AND CRM_NOCHAR='" + txtGRNno.Text + "' AND CR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'");

                    if (dtInwdQty.Rows.Count > 0)
                    {
                        txtQTy.Text = dtInwdQty.Rows[0]["CD_CHALLAN_QTY"].ToString();
                        ViewState["Qty"] = txtQTy.Text.ToString();
                    }
                }
                CommonClasses.SetModifyLock("IRN_CUSTOMER_REJECTION_MASTER", "MODIFY", "CRM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            else if (str == "MOD")
            {
                txtGRNDate.Enabled = false;
                ddlParty.Enabled = false;
                txtComplaintNo.Enabled = false;

                CommonClasses.SetModifyLock("IRN_CUSTOMER_REJECTION_MASTER", "MODIFY", "CRM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            else if (str == "Add")
            {
                txtGINO.Text = Request.QueryString[2].ToString();
                txtGRNDate.Text = Request.QueryString[3].ToString();
                ddlItemCode.SelectedValue = Request.QueryString[4].ToString();//ViewState["Icode"].ToString();
                ddlItemName.SelectedValue = Request.QueryString[4].ToString();

                DataTable Dtunit = new DataTable();
                Dtunit = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_INV_RATE FROM ITEM_MASTER,ITEM_UNIT_MASTER WHERE ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND  ITEM_UNIT_MASTER.ES_DELETE=0 AND  ITEM_MASTER.ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CODE='" + Request.QueryString[4].ToString() + "' ORDER BY I_UOM_NAME");

                if (Dtunit.Rows.Count > 0)
                {
                    ddlUOM.SelectedValue = Dtunit.Rows[0]["I_UOM_CODE"].ToString();
                }
                ddlParty.SelectedValue = Request.QueryString[5].ToString();

                DataTable dtInwdQty = new DataTable();
                dtInwdQty = CommonClasses.Execute("SELECT CD_CHALLAN_QTY,CD_RECEIVED_QTY,CR_GIN_DATE FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_I_CODE='" + Request.QueryString[4].ToString() + "' AND CR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND CR_CODE='" + Request.QueryString[1].ToString() + "' ");

                if (dtInwdQty.Rows.Count > 0)
                {
                    txtQTy.Text = dtInwdQty.Rows[0]["CD_CHALLAN_QTY"].ToString();
                    ViewState["Qty"] = txtQTy.Text.ToString();
                }
            }
            else
            {
                DataTable dtStatus = new DataTable();
                dtStatus = CommonClasses.Execute("SELECT CRM_CODE,CRM_STATUS FROM IRN_CUSTOMER_REJECTION_MASTER WHERE ES_DELETE=0 and CRM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                if (dtStatus.Rows.Count > 0)
                {
                    if (dtStatus.Rows[0]["CRM_STATUS"].ToString() == "True")
                    {

                        ViewState["ImageUpload"] = dtStatus.Rows[0]["CRM_CODE"].ToString();

                        dgIRN.Columns[11].Visible = true;
                        dgIRN.Columns[12].Visible = true;
                        //dgIRN.Columns[13].Visible = true;
                        dgIRN.Columns[14].Visible = true;
                        dgIRN.Columns[15].Visible = true;
                        dgIRN.Columns[18].Visible = true;//View button
                        dgIRN.Columns[23].Visible = true;//View button
                        dgIRN.Columns[28].Visible = true;//View button
                    }
                }
                else
                {
                    dgIRN.Columns[15].Visible = false;
                }
                foreach (GridViewRow gvr in dgIRN.Rows)
                {
                    LinkButton lnkButton = ((LinkButton)gvr.FindControl("lnkDelete"));
                    lnkButton.Enabled = false;
                    LinkButton lnkModify = ((LinkButton)gvr.FindControl("lnkSelect"));
                    lnkModify.Enabled = false;
                    TextBox DefectObs = ((TextBox)gvr.FindControl("txtCRD_DEF_OBSERVED"));
                    DefectObs.Enabled = false;
                    TextBox Rautcaus = ((TextBox)gvr.FindControl("txtCRD_ROUTE_CAUSE"));
                    Rautcaus.Enabled = false;
                    TextBox Action = ((TextBox)gvr.FindControl("txtCRD_ACTION_TAKEN"));
                    Action.Enabled = false;
                    FileUpload File = ((FileUpload)gvr.FindControl("imgUpload"));
                    File.Enabled = false;
                    Label RejQty = ((Label)gvr.FindControl("lblCRD_REJ_QTY"));
                    RejQty.Enabled = false;
                    Label Type = ((Label)gvr.FindControl("lblCRD_TYPE"));
                    Type.Enabled = false;
                }

                txtGRNno.Enabled = false;
                txtGRNDate.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                ddlParty.Enabled = false;
                ddlUOM.Enabled = false;
                BtnInsert.Enabled = false;
                txtQTy.Enabled = false;
                btnSubmit.Visible = false;
                dgIRN.Enabled = true;
                txtComplaintNo.Enabled = false;
                txtReason.Enabled = false;
                txtRejQty.Enabled = false;
                ddlType.Enabled = false;
                txtDocdate.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN Customer Rejection", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("IRN Customer Rejection", "GetValues", ex.Message);
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
            CommonClasses.SendError("IRN Customer Rejection", "Setvalues", ex.Message);
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
            //Click on Add button that time execute this code
            if (Request.QueryString[0].Equals("Add"))
            {
                DataTable dtcust = CommonClasses.Execute("SELECT P_CODE,P_NAME FROM PARTY_MASTER WHERE P_CODE='" + Convert.ToInt32(Request.QueryString[5]).ToString() + "'");
                string CustName = "", NMSpit = "";
                CustName = dtcust.Rows[0]["P_NAME"].ToString();
                var firstSpaceIndex = CustName.IndexOf(" ");//got first space
                NMSpit = CustName.Substring(0, firstSpaceIndex);//get first name befre space
                int IRNCustRejNo = 0;
                string Odate = Convert.ToDateTime(Session["OpeningDate"]).ToString("yy");
                string Closedate = Convert.ToDateTime(Session["ClosingDate"]).ToString("yy");
                string strCustrejNo = "";

                DataTable dt = CommonClasses.Execute("SELECT isnull(MAX(ISNULL(CRM_NO,0)),0) as CRM_NO FROM IRN_CUSTOMER_REJECTION_MASTER WHERE ES_DELETE=0");
                IRNCustRejNo = Convert.ToInt32(dt.Rows[0]["CRM_NO"]);
                IRNCustRejNo = IRNCustRejNo + 1;
                strCustrejNo = NMSpit + Odate + "-" + Closedate + CommonClasses.GenBillNo(IRNCustRejNo);
                // txtGRNno.Text = strCustrejNo;
                //txtGRNno.Text = "0000" + txtGRNno.Text + Odate + "-" + Closedate;
                if (CommonClasses.Execute1("INSERT INTO IRN_CUSTOMER_REJECTION_MASTER (CRM_NO,CRM_NOCHAR,CRM_DATE,CRM_CM_COMP_ID,CRM_CM_COMP_CODE,CRM_P_CODE,CRM_COMPLAINT_NO,CRM_CR_CODE,CRM_DOC_DATE) VALUES('" + IRNCustRejNo + "','" + strCustrejNo + "','" + txtGRNDate.Text + "','" + Convert.ToInt32(Session["CompanyId"]) + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + ddlParty.SelectedValue + "','" + txtComplaintNo.Text + "','" + Request.QueryString[1] + "','" + txtDocdate.Text + "') "))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(CRM_CODE) from IRN_CUSTOMER_REJECTION_MASTER");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        Boolean type = false, evidencetype = false, documenttype = false;
                        //if ((((Label)dgIRN.Rows[i].FindControl("lblCRD_IS_REQUIRED")).Text) == "YES")
                        //{
                        //    type = true;
                        //}
                        type = (((Label)dgIRN.Rows[i].FindControl("lblCRD_IS_REQUIRED")).Text) == "YES" ? true : false;
                        evidencetype = (((Label)dgIRN.Rows[i].FindControl("lblCRD_EVIDENCE_REQUIRED")).Text) == "YES" ? true : false;
                        documenttype = (((Label)dgIRN.Rows[i].FindControl("lblCRD_DOCUMENT_REQUIRED")).Text) == "YES" ? true : false;
                        CommonClasses.Execute1("INSERT INTO IRN_CUSTOMER_REJECTION_DETAIL (CRD_CRM_CODE,CRD_I_CODE,CRD_UOM,CRD_QTY,CRD_REASON,CRD_DEF_OBSERVED,CRD_REJ_QTY,CRD_TYPE,CRD_IS_REQUIRED,CRD_EVIDENCE_REQUIRED,CRD_DOCUMENT_REQUIRED) VALUES ('" + Code + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_REASON")).Text + "','" + ((TextBox)dgIRN.Rows[i].FindControl("txtCRD_DEF_OBSERVED")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_TYPE1")).Text + "','" + type + "','" + evidencetype + "','" + documenttype + "')");
                        CommonClasses.Execute("UPDATE CUSTREJECTION_DETAIL SET CD_CHK_REJ_STATUS=1 WHERE CD_I_CODE='" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_I_CODE")).Text + "' AND CD_CR_CODE='" + Convert.ToInt32(Request.QueryString[1].ToString()) + "'");
                    }
                    CommonClasses.WriteLog("IRN Customer Rejection", "Save", "IRN Customer Rejection", txtGRNno.Text.Trim().Replace("'", "\''"), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
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
                if (CommonClasses.Execute1("UPDATE IRN_CUSTOMER_REJECTION_MASTER SET CRM_NO='" + txtGRNno.Text.Trim() + "',CRM_DATE='" + txtGRNDate.Text.Trim() + "',CRM_P_CODE='" + ddlParty.SelectedValue + "',CRM_COMPLAINT_NO='" + txtComplaintNo.Text + "',CRM_DOC_DATE='" + txtDocdate.Text + "' where CRM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    CommonClasses.Execute1("DELETE FROM IRN_CUSTOMER_REJECTION_DETAIL WHERE CRD_CRM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        CommonClasses.Execute1("INSERT INTO IRN_CUSTOMER_REJECTION_DETAIL (CRD_CRM_CODE,CRD_I_CODE,CRD_UOM,CRD_QTY,CRD_REASON,CRD_DEF_OBSERVED,CRD_REJ_QTY,CRD_TYPE,CRD_IS_REQUIRED) VALUES('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_I_CODE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_UOM")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_REASON")).Text + "','" + ((TextBox)dgIRN.Rows[i].FindControl("txtCRD_DEF_OBSERVED")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_REJ_QTY")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_TYPE1")).Text + "',,'" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_IS_REQUIRED")).Text + "')");
                    }
                    CommonClasses.RemoveModifyLock("IRN_CUSTOMER_REJECTION_MASTER", "MODIFY", "CRM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("IRN Customer Rejection", "Update", "IRN Customer Rejection", txtGRNno.Text.Trim(), Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record not save ", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtGRNno.Focus();
                }
            }
            #region UpdateStatus
            //Click on Open button that time execute this code
            else if (Request.QueryString[0].Equals("UpdateStatus"))
            {
                string lblCRD_CRM_CODE = "";
                for (int i = 0; i < dgIRN.Rows.Count; i++)
                {
                    Label I_CODE = (Label)dgIRN.Rows[i].FindControl("lblCRD_I_CODE");
                    string QEA_DOC_NAME = ((Label)(dgIRN.Rows[i].FindControl("lblfilename"))).Text;
                    lblCRD_CRM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblCRD_CRM_CODE"))).Text;
                    CommonClasses.Execute1("UPDATE IRN_CUSTOMER_REJECTION_DETAIL SET CRD_ROUTE_CAUSE='" + ((TextBox)dgIRN.Rows[i].FindControl("txtCRD_ROUTE_CAUSE")).Text + "',CRD_ACTION_TAKEN='" + ((TextBox)dgIRN.Rows[i].FindControl("txtCRD_ACTION_TAKEN")).Text + "',CRD_FILE='" + ((Label)(dgIRN.Rows[i].FindControl("lblfilename"))).Text + "' ,CRD_EVIDENCE='" + ((Label)(dgIRN.Rows[i].FindControl("lblCRD_EVIDENCE"))).Text + "'  ,CRD_DOCUMENT='" + ((Label)(dgIRN.Rows[i].FindControl("lblCRD_DOCUMENT"))).Text + "'   WHERE CRD_CRM_CODE='" + lblCRD_CRM_CODE + "' AND CRD_I_CODE='" + I_CODE.Text + "' and CRD_REASON='" + ((Label)dgIRN.Rows[i].FindControl("lblCRD_REASON")).Text + "'");
                }
                CommonClasses.Execute1("UPDATE IRN_CUSTOMER_REJECTION_MASTER SET CRM_STATUS=1,CRM_CURR_DATE=GETDATE() WHERE CRM_CODE='" + lblCRD_CRM_CODE + "'");

                CommonClasses.RemoveModifyLock("IRN_CUSTOMER_REJECTION_DETAIL", "MODIFY", "CRD_CRM_CODE", Convert.ToInt32(lblCRD_CRM_CODE));
                CommonClasses.WriteLog("IRN Customer Rejection", "UpdateStatus", "IRN Customer Rejection", txtQTy.Text, Convert.ToInt32(lblCRD_CRM_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/IRN/VIEW/ViewIRNCustomerRejection.aspx", false);
            }
            #endregion UpdateStatus
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("IRN Customer Rejection", "SaveRec", ex.Message);
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
            CommonClasses.SendError("IRN Customer Rejection", "ShowMessage", Ex.Message);
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
            }
            if (Convert.ToInt32(ddlParty.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Party";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlParty.Focus();
                return;
            }
            if (txtRejQty.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Rej. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRejQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtRejQty.Text.Trim()) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Rej. Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtRejQty.Focus();
                return;
            }
            PanelMsg.Visible = false;
            if (Convert.ToDouble(txtQTy.Text.Trim()) < Convert.ToDouble(txtRejQty.Text.Trim()))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Rej. Qty should not be Greater than Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }

            for (int i = 0; i < dgIRN.Rows.Count; i++)
            {
                string CRD_I_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblCRD_I_CODE"))).Text;
                string CRD_CRM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblCRD_CRM_CODE"))).Text;
                string DefObserved = ((Label)(dgIRN.Rows[i].FindControl("lblCRD_REASON"))).Text;

                if (txtReason.Text == DefObserved && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Different Defect Observed";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
            }

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_CRM_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("RSM_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_UOM");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_UOM_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_REJ_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_TYPE");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_TYPE1");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_REASON");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_DEF_OBSERVED");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_ROUTE_CAUSE");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_ACTION_TAKEN");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_FILE");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_IS_REQUIRED");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_EVIDENCE_REQUIRED");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_EVIDENCE");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_DOCUMENT_REQUIRED");
                ((DataTable)ViewState["dt2"]).Columns.Add("CRD_DOCUMENT");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();

            txtQTy.Text = (Convert.ToDouble(txtQTy.Text) - Convert.ToDouble(txtRejQty.Text)).ToString();
            //ViewState["Qty"] = (Convert.ToDouble(txtQTy.Text) - Convert.ToDouble(txtRejQty.Text)).ToString();

            dr["CRD_I_CODE"] = ddlItemCode.SelectedValue;
            dr["I_CODENO"] = ddlItemCode.SelectedItem.Text;
            dr["I_NAME"] = ddlItemName.SelectedItem.Text;
            dr["CRD_UOM"] = ddlUOM.SelectedValue;
            dr["I_UOM_NAME"] = ddlUOM.SelectedValue;
            dr["I_UOM_NAME"] = ddlUOM.SelectedItem;
            dr["CRD_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(ViewState["Qty"])));
            dr["CRD_REJ_QTY"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRejQty.Text)));
            if (ddlType.SelectedValue == "1")
            {
                txtReason.Text = ddlReason.SelectedItem.ToString();
            }
            dr["CRD_REASON"] = txtReason.Text;
            dr["CRD_TYPE"] = ddlType.SelectedItem.Text;
            dr["CRD_TYPE1"] = ddlType.SelectedValue;
            dr["CRD_IS_REQUIRED"] = ChkIsRequired.Checked ? "YES" : "NO";
            dr["CRD_EVIDENCE_REQUIRED"] = chkEvidence.Checked ? "YES" : "NO";
            dr["CRD_DOCUMENT_REQUIRED"] = chkDocument.Checked ? "YES" : "NO";

            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    //txtQTy.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                // txtQTy.Text = "";
            }
            #endregion
            ViewState["RejQty"] = Convert.ToDouble(ViewState["RejQty"]) + Convert.ToDouble(txtRejQty.Text);
            #region Binding data to Grid
            dgIRN.Enabled = true;
            dgIRN.Visible = true;
            dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
            dgIRN.DataBind();

            txtRejQty.Text = "0";
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
            string code = "";
            string filePath = "";
            string directory = "";
            FileInfo File;

            if (e.CommandName == "Delete")
            {
                int rowindex = row.RowIndex;
                dgIRN.DeleteRow(rowindex);

                string rejqty = ((Label)(row.FindControl("lblCRD_REJ_QTY"))).Text;

                txtQTy.Text = (Convert.ToDouble(ViewState["Qty"]) - Convert.ToDouble(ViewState["RejQty"]) + Convert.ToDouble(rejqty)).ToString();
                ViewState["RejQty"] = Convert.ToDouble(ViewState["RejQty"]) - Convert.ToDouble(rejqty);
                txtQTy.Text = (Convert.ToDouble(ViewState["Qty"]) - Convert.ToDouble(ViewState["RejQty"])).ToString();
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
                dgIRN.DataBind();

                //dgIRN.FooterRow.Cells[7].Text = "Total";

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

                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblCRD_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblCRD_I_CODE"))).Text;
                ddlUOM.SelectedValue = ((Label)(row.FindControl("lblCRD_UOM"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                txtQTy.Text = ((Label)(row.FindControl("lblCRD_QTY"))).Text;
                txtReason.Text = ((Label)(row.FindControl("lblCRD_REASON"))).Text;
                txtRejQty.Text = ((Label)(row.FindControl("lblCRD_REJ_QTY"))).Text;
                //if (((Label)(row.FindControl("lblCRD_IS_REQUIRED"))).Text == "YES")
                //{
                //    ChkIsRequired.Checked = true;
                //}
                //else
                //{
                //    ChkIsRequired.Checked = false;
                //}
                ChkIsRequired.Checked = (((Label)row.FindControl("lblCRD_IS_REQUIRED")).Text) == "YES" ? true : false;
                chkEvidence.Checked = (((Label)row.FindControl("lblCRD_EVIDENCE_REQUIRED")).Text) == "YES" ? true : false;
                chkDocument.Checked = (((Label)row.FindControl("lblCRD_DOCUMENT_REQUIRED")).Text) == "YES" ? true : false;

                //ddlType.SelectedValue = ((Label)(row.FindControl("lblCRD_TYPE"))).Text;
                ddlType_SelectedIndexChanged(null, null);
                LoadReason();
                ddlReason.SelectedValue = ((Label)(row.FindControl("lblCRD_REASON"))).Text;

                //  txtQTy.Text = (Convert.ToDouble(((Label)(row.FindControl("lblCRD_QTY"))).Text) - Convert.ToDouble(((Label)(row.FindControl("lblCRD_REJ_QTY"))).Text)).ToString();
                txtQTy.Text = (Convert.ToDouble(ViewState["Qty"]) - Convert.ToDouble(ViewState["RejQty"]) + Convert.ToDouble(txtRejQty.Text)).ToString();
                ViewState["RejQty"] = Convert.ToDouble(ViewState["RejQty"]) - Convert.ToDouble(txtRejQty.Text);
                //for (int i = 0; i < dgIRN.Rows.Count; i++)  
                //{

                //}
            }
            if (e.CommandName == "View")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    filePath = ((Label)(row.FindControl("lblfilename"))).Text;
                    if (filePath == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "File Not Exist";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    directory = "../../UpLoadPath/CustRejDoc/" + ViewState["ImageUpload"].ToString() + "/" + filePath;
                    ModalPopDocument.Show();
                    IframeViewPDF.Attributes["src"] = directory;
                }
            }

            #region ViewEvidence
            if (e.CommandName == "ViewEvidence")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    filePath = ((Label)(row.FindControl("lblCRD_EVIDENCE"))).Text;
                    if (filePath == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "File Not Exist";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    directory = "../../UpLoadPath/CustRejDoc/" + ViewState["ImageUpload"].ToString() + "/" + filePath;
                    ModalPopDocument.Show();
                    IframeViewPDF.Attributes["src"] = directory;
                }
            }
            #endregion ViewEvidence
       
            #region ViewDocument
            if (e.CommandName == "ViewDocument")
            {
                if (filePath != "")
                {
                    File = new FileInfo(filePath);
                }
                else
                {
                    filePath = ((Label)(row.FindControl("lblCRD_DOCUMENT"))).Text;
                    if (filePath == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "File Not Exist";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                    directory = "../../UpLoadPath/CustRejDoc/" + ViewState["ImageUpload"].ToString() + "/" + filePath;
                    ModalPopDocument.Show();
                    IframeViewPDF.Attributes["src"] = directory;
                }
            }
            #endregion ViewDocument
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("IRN ENTRY", "dgIRN_RowCommand", Ex.Message);
        }
    }
    #endregion

    protected void dgIRN_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            ViewState["fileName"] = null;
            //FileLoactionCreate();

            string sDirPath = Server.MapPath(@"~/UpLoadPath/CustRejDoc/" + ViewState["ImageUpload"].ToString() + "");
            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);
            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            GridViewRow row = dgIRN.Rows[e.RowIndex];
            #region Comment
            //FileUpload flUp = new FileUpload();
            //if ((FileUpload)row.FindControl("imgUpload") != null)
            //    flUp = (FileUpload)row.FindControl("imgUpload");
            //else if ((FileUpload)row.FindControl("EvidenceUpload") != null)
            //    flUp = (FileUpload)row.FindControl("EvidenceUpload");
            //else if ((FileUpload)row.FindControl("DocumentUpload") != null)
            //    flUp = (FileUpload)row.FindControl("DocumentUpload");

            #endregion
            FileUpload flUp = (FileUpload)row.FindControl("imgUpload");
            FileUpload flUpEvidenceUpload = (FileUpload)row.FindControl("EvidenceUpload");
            FileUpload flUpDocumentUpload = (FileUpload)row.FindControl("DocumentUpload");
            FileUpload BrochureUpload = (FileUpload)dgIRN.Rows[e.RowIndex].FindControl("BrochureUpload");
            string directory = Server.MapPath(@"~/UpLoadPath/CustRejDoc/" + ViewState["ImageUpload"].ToString() + "");

            string fileName = ""; //Path.GetFileName(flUp.PostedFile.FileName);

            if (flUp.PostedFile != null && flUp.PostedFile.FileName.Trim() != "")
            {
                try
                {
                    fileName = Path.GetFileName(flUp.PostedFile.FileName);
                    flUp.SaveAs(Path.Combine(directory, fileName));
                    ((Label)(dgIRN.Rows[e.RowIndex].FindControl("lblfilename"))).Text = fileName.ToString();
                }
                catch //Found Exception then Set Blank Name to Gridview control
                {
                    ((Label)(dgIRN.Rows[e.RowIndex].FindControl("lblfilename"))).Text = "";
                }
            }
            if (flUpEvidenceUpload.PostedFile != null && flUpEvidenceUpload.PostedFile.FileName.Trim() != "")
            {
                try
                {
                    fileName = Path.GetFileName(flUpEvidenceUpload.PostedFile.FileName);
                    flUpEvidenceUpload.SaveAs(Path.Combine(directory, fileName));
                    ((Label)(dgIRN.Rows[e.RowIndex].FindControl("lblCRD_EVIDENCE"))).Text = fileName.ToString();
                }
                catch
                {
                    ((Label)(dgIRN.Rows[e.RowIndex].FindControl("lblCRD_EVIDENCE"))).Text = "";
                }
            }
            if (flUpDocumentUpload.PostedFile != null && flUpDocumentUpload.PostedFile.FileName.Trim() != "")
            {
                try
                {
                    fileName = Path.GetFileName(flUpDocumentUpload.PostedFile.FileName);
                    flUpDocumentUpload.SaveAs(Path.Combine(directory, fileName));
                    ((Label)(dgIRN.Rows[e.RowIndex].FindControl("lblCRD_DOCUMENT"))).Text = fileName.ToString();
                }
                catch
                {
                    ((Label)(dgIRN.Rows[e.RowIndex].FindControl("lblCRD_DOCUMENT"))).Text = "";
                }
            }
            ViewState["fileName"] = fileName.ToString();

        }
        catch
        {
        }
    }

    private void FileLoactionCreate(string Code1, string FileLoactionCreate, string flUp1)
    {
        try
        {
            DataTable dt = new DataTable();
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                File_Code = Convert.ToInt32(Code1);
            }
            else
            {
                dt = CommonClasses.Execute("select isnull(max(CRM_CODE+1),0)as Code from IRN_CUSTOMER_REJECTION_MASTER");
                if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
                {
                    DataTable dt1 = CommonClasses.Execute("SELECT IDENT_CURRENT('IRN_CUSTOMER_REJECTION_MASTER')+1");
                    if (dt.Rows[0][0].ToString() == "-2147483647")
                    {
                        File_Code = -2147483648;
                    }
                    else
                    {
                        File_Code = int.Parse(dt1.Rows[0][0].ToString());
                    }
                }
                else
                {
                    File_Code = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            string sDirPath = Server.MapPath(@"~/UpLoadPath/CustRejDoc/" + File_Code + "");
            string fileName = Path.GetFileName(FileLoactionCreate);

            FileUpload flUp = new FileUpload();

            DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);
            //create folder or derectory code
            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            // Get the current app path:
            var currentApplicationPath = Server.MapPath(flUp1);

            //Get the full path of the file    
            var fullFilePath = currentApplicationPath;

            // Get the destination path
            var copyToPath = sDirPath + "/" + fileName;

            // Copy the file
            System.IO.File.Copy(fullFilePath, copyToPath);
        }
        catch (Exception ex1)
        {
            //throw;
        }
    }

    #region FileLoactionCreate

    void FileLoactionCreate1()
    {
        DataTable dt = new DataTable();
        if (Request.QueryString[0].Equals("MODIFY"))
        {
            File_Code = Convert.ToInt32(ViewState["mlCode"]);
        }
        else if (Request.QueryString[0].Equals("UpdateStatus"))
        {
            DataTable dtCode = new DataTable();
            dtCode = CommonClasses.Execute("SELECT CRM_CODE FROM IRN_CUSTOMER_REJECTION_MASTER WHERE CRM_P_CODE='" + Convert.ToInt32(Request.QueryString[5]).ToString() + "' AND CRM_STATUS=0 AND ES_DELETE=0");

            File_Code = Convert.ToInt32(dtCode.Rows[0]["CRM_CODE"]); //Convert.ToInt32(ViewState["mlCode"]);
        }
        else
        {
            dt = CommonClasses.Execute("select isnull(max(CRM_CODE+1),0)as Code from IRN_CUSTOMER_REJECTION_MASTER");
            if (dt.Rows[0][0].ToString() == "" || dt.Rows[0][0].ToString() == "0")
            {
                DataTable dt1 = CommonClasses.Execute("SELECT IDENT_CURRENT('IRN_CUSTOMER_REJECTION_MASTER')+1");
                if (dt.Rows[0][0].ToString() == "-2147483647")
                {
                    File_Code = -2147483648;
                }
                else
                {
                    File_Code = int.Parse(dt1.Rows[0][0].ToString());
                }
            }
            else
            {
                File_Code = int.Parse(dt.Rows[0][0].ToString());
            }
        }

        string sDirPath = Server.MapPath(@"~/UpLoadPath/CustRejDoc/" + File_Code + "");

        DirectoryInfo ObjSearchDir = new DirectoryInfo(sDirPath);

        if (!ObjSearchDir.Exists)
        {
            ObjSearchDir.Create();
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
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_CRM_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RSM_NO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_UOM", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_REJ_QTY", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_TYPE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_TYPE1", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_REASON", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_DEF_OBSERVED", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_ROUTE_CAUSE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_ACTION_TAKEN", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_FILE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_IS_REQUIRED", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_EVIDENCE_REQUIRED", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_EVIDENCE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_DOCUMENT_REQUIRED", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("CRD_DOCUMENT", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgIRN.DataSource = dtFilter;
            dgIRN.DataBind();
        }
    }

    public void LoadReason()
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select DISTINCT CRD_REASON from IRN_CUSTOMER_REJECTION_DETAIL,IRN_CUSTOMER_REJECTION_MASTER where ES_DELETE=0 and CRM_CODE=CRD_CRM_CODE and CRM_P_CODE='" + Convert.ToInt32(Request.QueryString[5]).ToString() + "' and CRD_I_CODE='" + Convert.ToInt32(Request.QueryString[4]).ToString() + "' and CRM_CM_COMP_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' order by CRD_REASON");

        ddlReason.DataSource = dt;
        ddlReason.DataTextField = "CRD_REASON";
        ddlReason.DataValueField = "CRD_REASON";
        ddlReason.DataBind();
        //ddlReason.Items.Insert(0, new ListItem("----Select Item Code----", "0"));
    }

    public void loadItem()
    {
        DataTable dtStage = new DataTable();
        string ICode = "";

        if (!Request.QueryString[0].Equals("VIEW"))
        {
            ICode = Convert.ToInt32(Request.QueryString[4]).ToString();
        }
        if (!Request.QueryString[0].Equals("VIEW"))
        {
            dtStage = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CODE='" + ICode + "' ORDER BY I_CODENO");
        }
        else
        {
            dtStage = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER WHERE ES_DELETE=0 AND I_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY I_CODENO");
        }

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
        dtStage = CommonClasses.Execute("SELECT I_UOM_CODE,I_UOM_NAME FROM ITEM_UNIT_MASTER WHERE ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' ORDER BY  I_UOM_NAME");
        ddlUOM.DataSource = dtStage;
        ddlUOM.DataTextField = "I_UOM_NAME";
        ddlUOM.DataValueField = "I_UOM_CODE";
        ddlUOM.DataBind();
        ddlUOM.Items.Insert(0, new ListItem("----Select Unit----", "0"));
    }

    public void loadunit(string Item)
    {
        DataTable dtStage = new DataTable();
        dtStage = CommonClasses.Execute("SELECT ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_INV_RATE FROM ITEM_MASTER,ITEM_UNIT_MASTER WHERE ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND  ITEM_UNIT_MASTER.ES_DELETE=0 AND  ITEM_MASTER.ES_DELETE=0 AND I_UOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CODE='" + Item + "' ORDER BY I_UOM_NAME");

        if (dtStage.Rows.Count > 0)
        {
            ddlUOM.SelectedValue = dtStage.Rows[0]["I_UOM_CODE"].ToString();
            ddlUOM.Enabled = false;
        }
        else
        {
        }
    }

    protected void ddlParty_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlItemName.Items.Clear();
        ddlItemCode.Items.Clear();
        txtQTy.Text = "0.00";
        txtReason.Text = "";
        ddlUOM.SelectedIndex = 0;
        BlankGrid();
        ((DataTable)ViewState["dt2"]).Rows.Clear();
        loadItem();
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
        ddlParty.SelectedValue = Request.QueryString[5].ToString();

        DataTable dtInwdQty = new DataTable();
        dtInwdQty = CommonClasses.Execute("SELECT CD_CHALLAN_QTY,CD_RECEIVED_QTY,CR_GIN_DATE FROM CUSTREJECTION_DETAIL,CUSTREJECTION_MASTER WHERE CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND CD_I_CODE='" + Request.QueryString[4].ToString() + "' AND CR_GIN_NO='" + txtGRNno.Text + "' AND CR_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "'");

        if (dtInwdQty.Rows.Count > 0)
        {
            txtQTy.Text = dtInwdQty.Rows[0]["CD_CHALLAN_QTY"].ToString();
        }
    }


    #region ddlReason_SelectedIndexChanged
    protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReason.SelectedIndex == -1)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "There is no any Repetative defects please select new Type";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            ddlType.Focus();
            ddlType.SelectedIndex = -1; //Set New Type
            lblreason.Visible = false;
            ddlReason.Visible = false;
            return;
        }
        else
            txtReason.Text = ddlReason.SelectedItem.ToString();
    }
    #endregion ddlReason_SelectedIndexChanged

    public void LoadParty()
    {
        DataTable dtP = new DataTable();
        if (!Request.QueryString[0].Equals("VIEW"))
        {
            dtP = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND P_TYPE=1 AND P_CODE='" + Convert.ToInt32(Request.QueryString[5]).ToString() + "' AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND PARTY_MASTER.ES_DELETE=0 ORDER BY P_NAME");
        }
        else
        {
            dtP = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CUSTPO_MASTER WHERE CPOM_P_CODE=P_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND P_TYPE=1 AND P_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND PARTY_MASTER.ES_DELETE=0 ORDER BY P_NAME");
        }

        ddlParty.DataSource = dtP;
        ddlParty.DataTextField = "P_NAME";
        ddlParty.DataValueField = "P_CODE";
        ddlParty.DataBind();
        ddlParty.Items.Insert(0, new ListItem("----Select Customer----", "0"));
    }

    // For New Defect or Repetative (if not found show message)
    #region ddlType_SelectedIndexChanged
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "0")////New
        {
            txtReason.Enabled = true;
            lblreason.Visible = false;
            ddlReason.Visible = false;
            txtReason.Text = "";
        }
        else//Repetative
        {
            LoadReason();
            ddlReason_SelectedIndexChanged(null, null);
            txtReason.Enabled = false;
            lblreason.Visible = true;
            ddlReason.Visible = true;
        }
    }
    #endregion ddlType_SelectedIndexChanged
}

