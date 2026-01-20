using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Transactions_ADD_MaterialAcceptance : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int IM_No = 0;
    MaterialInspection_BL inspect_BL = new MaterialInspection_BL();
    static DataTable inspectiondt = new DataTable();
    static DataTable dt = new DataTable();
    static string type = "";
    public int icode;
    static int To_storeCode = 0;
    static int TRANS_TYPE = 0;
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
                    type = Request.QueryString[0].ToString();
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        IM_No = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        type = "VIEW";
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        IM_No = Convert.ToInt32(Request.QueryString[1].ToString());
                        To_storeCode = Convert.ToInt32(Request.QueryString[2].ToString());
                        TRANS_TYPE = Convert.ToInt32(Request.QueryString[3].ToString());
                        ViewRec("MOD");
                        type = "MOD";
                    }
                    else if (Request.QueryString[0].Equals("ADD"))
                    {
                        txtInspDate.Attributes.Add("readonly", "readonly");

                        IM_No = Convert.ToInt32(Request.QueryString[1].ToString());
                        To_storeCode = Convert.ToInt32(Request.QueryString[2].ToString());
                        ViewRec("ADD");
                        type = "ADD";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Inspection-ADD", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtInspDate.Attributes.Add("readonly", "readonly");
            if (str == "VIEW")
            {
                inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + IM_No + "' ");
                if (inspectiondt.Rows.Count > 0)
                {
                    dgMaterialAcceptance.DataSource = inspectiondt;
                    dgMaterialAcceptance.DataBind();
                    dgMaterialAcceptance.Visible = true;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
                }
            }
            else if (str == "ADD")
            {
                txtInspDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy");
                if (TRANS_TYPE == 0)
                {
                    inspectiondt = CommonClasses.Execute("SELECT I_CODENO	AS [Item Code],I_NAME AS [Item Name],IM_NO AS [Document No.]	,IM_DATE AS [Document Date]  , STORE_MASTER.STORE_NAME AS [From Store] ,TOStore.STORE_NAME AS [To Store], IMD_ISSUE_QTY AS [Received qty.] FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE  ISSUE_MASTER.ES_DELETE=0 AND  ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE AND ISSUE_MASTER.IM_CODE=  '" + IM_No + "' ");
                }
                else
                {
                    inspectiondt = CommonClasses.Execute(" SELECT I_CODENO	AS [Item Code],I_NAME AS [Item Name],RTF_DOC_NO AS [Document No.]	,RTF_DOC_DATE AS [Document Date]  ,  'Rejection Store' AS  [From Store] , 'Main Store' AS [To Store],  RTF_QTY AS [Received qty.]    FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_CODE=  '" + IM_No + "' ");
                }
                if (inspectiondt.Rows.Count > 0)
                {
                    dgMaterialAcceptance.DataSource = inspectiondt;
                    dgMaterialAcceptance.DataBind();
                    dgMaterialAcceptance.Visible = true;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
                }
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
            }
            else if (str == "MOD")
            {
                if (TRANS_TYPE == 0)
                {
                    //Sujata: Make Round 2 IMD_ISSUE_QTY
                    inspectiondt = CommonClasses.Execute("SELECT ISSUE_MASTER.IM_CODE,ITEM_MASTER.I_CODE,I_CODENO ,I_NAME ,IM_NO ,CONVERT(VARCHAR,IM_DATE,113) AS IM_DATE , STORE_MASTER.STORE_NAME AS [FROM_STORE_NAME] ,TOStore.STORE_NAME AS [TO_STORE_NAME], ISNULL(ROUND(IMD_ISSUE_QTY,2),0) AS IMD_ISSUE_QTY ,'' as OK_Qty, 0 as Rej_Qty ,STORE_MASTER.STORE_CODE  as [FROM_STORE_CODE], TOStore.STORE_CODE AS [TO_STORE_CODE] FROM   ISSUE_MASTER , ISSUE_MASTER_DETAIL , STORE_MASTER , ITEM_MASTER , STORE_MASTER AS TOStore WHERE ISSUE_MASTER.IM_CODE = ISSUE_MASTER_DETAIL.IM_CODE AND STORE_MASTER.STORE_CODE = ISSUE_MASTER.IM_FROM_STORE  AND TOStore.STORE_CODE = ISSUE_MASTER_DETAIL.IMD_To_STORE  AND ITEM_MASTER.I_CODE = ISSUE_MASTER_DETAIL.IMD_I_CODE  AND  IMD_STORE_TYPE=0 AND ISSUE_MASTER.IM_CODE= '" + IM_No + "' AND ISSUE_MASTER_DETAIL.IMD_To_STORE = '" + To_storeCode + "'");
                }
                else
                {
                    //inspectiondt = CommonClasses.Execute("SELECT RTF_CODE AS IM_CODE,I_CODE, I_CODENO,I_NAME,RTF_DOC_NO AS IM_NO,CONVERT(VARCHAR,RTF_DOC_DATE,113) AS IM_DATE ,  'Rejection Store' AS [FROM_STORE_NAME] , 'Main Store'AS [TO_STORE_NAME],  RTF_QTY AS IMD_ISSUE_QTY ,'' as OK_Qty, 0 as Rej_Qty ,'-2147483641'  as [FROM_STORE_CODE], '-2147483647' AS [TO_STORE_CODE] FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_CODE='" + IM_No + "' ");
                    inspectiondt = CommonClasses.Execute("SELECT RTF_CODE AS IM_CODE,I_CODE, I_CODENO,I_NAME,RTF_DOC_NO AS IM_NO,CONVERT(VARCHAR,RTF_DOC_DATE,113) AS IM_DATE ,  CASE WHEN RTF_STORE_CODE=-2147483647 then 'Rejection Store' ELSE 'Rejection Store (Plant II)' END  AS [FROM_STORE_NAME] , CASE WHEN RTF_STORE_CODE=-2147483647 then 'Main Store' ELSE 'Main Store (Plant II)' END AS [TO_STORE_NAME],  RTF_QTY AS IMD_ISSUE_QTY ,'' as OK_Qty, 0 as Rej_Qty ,CASE WHEN RTF_STORE_CODE=-2147483647 then '-2147483641' else '-2147483628' END  as [FROM_STORE_CODE], CASE WHEN RTF_STORE_CODE=-2147483647 then'-2147483647' else '-2147483634' END AS [TO_STORE_CODE] FROM REJECTION_TO_FOUNDRY_MASTER,ITEM_MASTER where REJECTION_TO_FOUNDRY_MASTER.ES_DELETE=0 AND RTF_I_CODE=I_CODE AND RTF_CODE='" + IM_No + "' ");
                }
                if (inspectiondt.Rows.Count > 0)
                {
                    dgMaterialAcceptance.DataSource = inspectiondt;
                    dgMaterialAcceptance.DataBind();
                    dgMaterialAcceptance.Visible = true;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
                }
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region getInfo
    private bool getInfo(string str)
    {
        bool flag = false;
        try
        {
            if (str == "VIEW" || str == "MOD")
            {
                dt = CommonClasses.Execute("select I_CODENO,IWM_TYPE,I_CURRENT_BAL,INSM_CODE,INSM_REMARK,convert(varchar,INSM_DATE,113) as INSM_DATE,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME,ISNULL(INSM_PDR_CHECK,0) AS INSM_PDR_CHECK,INSM_PDR_NO,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER,INSPECTION_S_MASTER where INSPECTION_S_MASTER.ES_DELETE=0 AND INSM_IWM_CODE=IWM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 AND IWD_I_CODE=INSM_I_CODE  and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND IWD_I_CODE='" + icode + "' and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + IM_No + "' ");
                if (dt.Rows.Count > 0)
                {
                    lblGRNCODE.Text = dt.Rows[0]["IWD_IWM_CODE"].ToString();
                    txtInspNo.Text = dt.Rows[0]["IWD_INSP_NO"].ToString();
                    txtPONumber.Text = dt.Rows[0]["SPOM_PO_NO"].ToString();
                    txtItemName.Text = dt.Rows[0]["I_NAME"].ToString();
                    txtInspDate.Text = Convert.ToDateTime(dt.Rows[0]["INSM_DATE"].ToString()).ToString("dd MMM yyyy");
                    txtrecQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();
                    txtUnitName.Text = dt.Rows[0]["UOM_NAME"].ToString();
                    txtOkQty.Text = dt.Rows[0]["IWD_CON_OK_QTY"].ToString();
                    txtRejQty.Text = dt.Rows[0]["IWD_CON_REJ_QTY"].ToString();
                    txtScrapQty.Text = dt.Rows[0]["IWD_CON_SCRAP_QTY"].ToString();
                    txtRemark.Text = dt.Rows[0]["INSM_REMARK"].ToString();
                    txtPDRNo.Text = dt.Rows[0]["INSM_PDR_NO"].ToString();
                    chkPDR.Checked = Convert.ToBoolean(dt.Rows[0]["INSM_PDR_CHECK"].ToString());
                    txtRate.Text = dt.Rows[0]["IWD_RATE"].ToString();
                    lblpartycode.Text = dt.Rows[0]["IWM_P_CODE"].ToString();
                    txtItemCode.Text = dt.Rows[0]["I_CODENO"].ToString();
                    if (str == "MOD")
                    {
                        if (dt.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                        {
                            if ((Convert.ToDouble(dt.Rows[0]["I_CURRENT_BAL"].ToString()) - (Convert.ToDouble(dt.Rows[0]["IWD_CON_OK_QTY"].ToString()) + Convert.ToDouble(dt.Rows[0]["IWD_CON_REJ_QTY"].ToString()))) < 0)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You cant Modify this record it's qty Is used In other transacrion";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                btnSubmit.Visible = false;
                                return false;
                            }
                            else
                            {
                                btnSubmit.Visible = true;
                            }
                        }
                        else
                        {
                            if ((Convert.ToDouble(dt.Rows[0]["I_CURRENT_BAL"].ToString()) - (Convert.ToDouble(dt.Rows[0]["IWD_CON_OK_QTY"].ToString()))) < 0)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You cant Modify this record it's qty Is used In other transacrion";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                btnSubmit.Visible = false;
                                return false;
                            }
                            else
                            {
                                btnSubmit.Visible = true;
                            }
                        }
                    }
                    if (chkPDR.Checked == true)
                    {
                        txtPDRNo.Enabled = true;
                    }
                    if (Convert.ToDouble(txtRejQty.Text) > 0)
                    {
                        DataTable dtReason = CommonClasses.Execute("select INSD_RM_CODE as Reason from INSPECTION_S_DETAIL where INSD_INSM_CODE='" + dt.Rows[0]["INSM_CODE"].ToString() + "' and INSD_I_CODE='" + icode + "'");
                        if (dtReason.Rows.Count > 0)
                        {
                            txtReason.Text = dtReason.Rows[0]["Reason"].ToString();
                        }
                    }
                }
            }
            else if (str == "ADD")
            {
                dt = CommonClasses.Execute("select I_CODENO,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE  from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE  AND IWD_I_CODE='" + icode + "' AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + IM_No + "' ");
                if (dt.Rows.Count > 0)
                {
                    lblGRNCODE.Text = dt.Rows[0]["IWD_IWM_CODE"].ToString();
                    txtInspNo.Text = dt.Rows[0]["IWD_INSP_NO"].ToString();
                    txtPONumber.Text = dt.Rows[0]["SPOM_PO_NO"].ToString();
                    txtItemName.Text = dt.Rows[0]["I_NAME"].ToString();
                    txtItemCode.Text = dt.Rows[0]["I_CODENO"].ToString();
                    txtrecQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();
                    txtUnitName.Text = dt.Rows[0]["UOM_NAME"].ToString();
                    txtOkQty.Text = dt.Rows[0]["IWD_REV_QTY"].ToString();
                    txtRejQty.Text = dt.Rows[0]["IWD_CON_REJ_QTY"].ToString();
                    txtScrapQty.Text = dt.Rows[0]["IWD_CON_SCRAP_QTY"].ToString();
                    txtRate.Text = dt.Rows[0]["IWD_RATE"].ToString();
                    lblpartycode.Text = dt.Rows[0]["IWM_P_CODE"].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "getInfo", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            inspect_BL.INSM_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);
            inspect_BL.INSM_NO = Convert.ToInt32(txtInspNo.Text);
            inspect_BL.INSM_DATE = Convert.ToDateTime(txtInspDate.Text);
            inspect_BL.INSM_RECEIVED_QTY = float.Parse(txtrecQty.Text == "" ? "0" : txtrecQty.Text);
            inspect_BL.INSM_OK_QTY = float.Parse(txtOkQty.Text == "" ? "0" : txtOkQty.Text);
            inspect_BL.INSM_REJ_QTY = float.Parse(txtRejQty.Text == "" ? "0" : txtRejQty.Text);
            inspect_BL.INSM_SCRAP_QTY = float.Parse(txtScrapQty.Text == "" ? "0" : txtScrapQty.Text);
            inspect_BL.INSM_REMARK = txtRemark.Text;
            inspect_BL.INSM_RATE = Convert.ToDouble(txtRate.Text == "" ? "0" : txtRate.Text);
            inspect_BL.INSM_PDR_CHECK = chkPDR.Checked;
            inspect_BL.INSM_PDR_NO = txtPDRNo.Text.Trim();
            DataTable dtParty = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + lblGRNCODE.Text + "'");
            string INWARD_NO = dtParty.Rows[0]["IWM_NO"].ToString();
            string Type = "";
            if (dtParty.Rows.Count > 0)
            {
                if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                {
                    inspect_BL.INSM_TYPE = "IWIAP";
                }
                else
                {
                    inspect_BL.INSM_TYPE = "IWIM";
                }
            }

            if (inspect_BL.INSM_REJ_QTY > 0)
            {
                inspect_BL.INSD_RM_CODE = txtReason.Text;
            }
            if (dt.Rows.Count > 0)
            {
                if (type == "MOD")
                {
                    inspect_BL.INSM_CODE = Convert.ToInt32(dt.Rows[0]["INSM_CODE"].ToString());
                }
                inspect_BL.INSM_IWM_CODE = Convert.ToInt32(dt.Rows[0]["IWD_IWM_CODE"].ToString());
                inspect_BL.INSM_SPOM_CODE = Convert.ToInt32(dt.Rows[0]["IWD_CPOM_CODE"].ToString());
                inspect_BL.INSM_I_CODE = Convert.ToInt32(dt.Rows[0]["IWD_I_CODE"].ToString());
                inspect_BL.INSM_UOM_CODE = Convert.ToInt32(dt.Rows[0]["IWD_UOM_CODE"].ToString());
                res = true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "Setvalues", Ex.Message);
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
            if (type == "ADD")
            {
                if (Setvalues())
                {
                    string msg = "";
                    if (inspect_BL.Save(out mlCode))
                    {
                        CommonClasses.WriteLog("Material Inspection", "Save", "Material Inspection", "", mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        mlCode = 0;
                        result = true;
                        ClearTextBoxes(panelDetail);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Saved Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        panelDetail.Visible = false;
                        panelInspection.Visible = true;
                        ViewRec("ADD");
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
            else if (type == "MOD")
            {
                if (Setvalues())
                {
                    string msg = "";
                    DataTable dtItemMast = new DataTable();
                    dtItemMast = CommonClasses.Execute(" SELECT IWM_TYPE,INSM_I_CODE,I_CURRENT_BAL, INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY  FROM INSPECTION_S_MASTER,INWARD_MASTER,ITEM_MASTER where INSM_IWM_CODE=IWM_CODE AND  INSM_IWM_CODE='" + lblGRNCODE.Text + "' AND INSPECTION_S_MASTER.ES_DELETE=0 AND I_CODE=INSM_I_CODE");
                    double Ok_Qty = 0;
                    if (dtItemMast.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                    {
                        Ok_Qty = Convert.ToDouble(txtOkQty.Text) + Convert.ToDouble(txtRejQty.Text);
                    }
                    else
                    {
                        Ok_Qty = Convert.ToDouble(txtOkQty.Text);
                    }
                    if ((Convert.ToDouble(dtItemMast.Rows[0]["I_CURRENT_BAL"].ToString()) + Ok_Qty) < 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant Modify this record it's qty Is used In other transacrion";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return false;
                    }
                    if (inspect_BL.Update(out mlCode))
                    {
                        CommonClasses.WriteLog("Material Inspection", "Update", "Material Inspection", "", mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        mlCode = 0;
                        result = true;
                        ClearTextBoxes(panelDetail);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Saved Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        panelDetail.Visible = false;
                        panelInspection.Visible = true;
                        ViewRec("MOD");
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Saved";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "SaveRec", Ex.Message);
        }
        return result;
    }
    #endregion

    #region ClearFunction
    private void ClearTextBoxes(Control p1)
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
                        t.Text = String.Empty;
                    }
                }
                if (ctrl is DropDownList)
                {
                    DropDownList t = ctrl as DropDownList;

                    if (t != null)
                    {
                        t.SelectedIndex = 0;
                    }
                }
                if (ctrl is CheckBox)
                {
                    CheckBox t = ctrl as CheckBox;

                    if (t != null)
                    {
                        t.Checked = false;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection -ADD", "ClearTextBoxes", Ex.Message);
        }
    }
    #endregion

    #region ViewInspection
    private void ViewInspection(string str, string item_code)
    {
        try
        {
            if (str == "VIEW")
            {
                panelInspection.Visible = false;
                panelDetail.Visible = true;
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
                icode = Convert.ToInt32(item_code);
                getInfo("VIEW");
                txtInspDate.Enabled = false;
                txtRejQty.Enabled = false;
                txtScrapQty.Enabled = false;
                txtRemark.Enabled = false;
                txtReason.Enabled = false;
                txtPDRNo.Enabled = false;
                chkPDR.Enabled = false;
            }
            else if (str == "MOD")
            {
                panelInspection.Visible = false;
                panelDetail.Visible = true;
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
                icode = Convert.ToInt32(item_code);
                getInfo("MOD");
            }
            else if (str == "ADD")
            {
                panelInspection.Visible = false;
                panelDetail.Visible = true;
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
                icode = Convert.ToInt32(item_code);
                getInfo("ADD");
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "ViewRec", Ex.Message);
        }
    }
    #endregion

    #region Numbering
    int Numbering()
    {
        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(INSM_NO) as INSM_NO from INSPECTION_S_MASTER where ES_DELETE=0 and INSM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'");
        if (dt.Rows[0]["INSM_NO"] == null || dt.Rows[0]["INSM_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["INSM_NO"]) + 1;
        }
        return GenGINNO;
    }
    #endregion

    #region Event

    #region GridEvent

    #region dgMainPO_Deleting
    protected void dgMainPO_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMainPO_SelectedIndexChanged
    protected void dgMainPO_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region dgMainPO_RowCommand
    protected void dgMainPO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion

    #region dgMaterialAcceptance_RowCommand
    protected void dgMaterialAcceptance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            if (e.CommandName.Equals("View"))
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                ViewInspection("VIEW", cpom_code);
            }
            else if (e.CommandName.Equals("Modify"))
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                ViewInspection(type, cpom_code);
            }
            else if (e.CommandName.Equals("Delete"))
            {

                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIWM_CODE"))).Text;
                string item_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                string insp_no = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIWD_INSP_NO"))).Text;
                string p_code = ((Label)(dgMaterialAcceptance.Rows[index].FindControl("lblIWM_P_CODE"))).Text;
                DataTable dtParty = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + cpom_code + "'");
                string INWARD_NO = dtParty.Rows[0]["IWM_NO"].ToString();
                string Type = "";
                if (dtParty.Rows.Count > 0)
                {
                    if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                    {
                        Type = "IWIAP";
                    }
                    else
                    {
                        Type = "IWIM";
                    }
                }
                if (type == "MOD")
                {
                    DataTable dtinsecption = new DataTable();
                    if (Type == "IWIAP")
                    {
                        dtinsecption = CommonClasses.Execute("  SELECT INSM_OK_QTY+INSM_REJ_QTY AS INSM_OK_QTY FROM INSPECTION_S_MASTER where    INSM_I_CODE='" + item_code + "'  AND  INSM_NO='" + insp_no + "' AND ES_DELETE=0  ");
                    }
                    else
                    {
                        dtinsecption = CommonClasses.Execute("  SELECT INSM_OK_QTY AS INSM_OK_QTY FROM INSPECTION_S_MASTER where    INSM_I_CODE='" + item_code + "'  AND  INSM_NO='" + insp_no + "'   AND ES_DELETE=0 ");
                    }
                    DataTable DtItemMaster = new DataTable();
                    DtItemMaster = CommonClasses.Execute("SELECT * FROM ITEM_MASTER WHERE I_CODE='" + item_code + "' AND ES_DELETE=0");
                    if (CommonClasses.CheckUsedInTran("INWARD_DETAIL,INWARD_MASTER", "IWD_IWM_CODE", "AND IWD_I_CODE='" + item_code + "' and IWD_BILL_PASS_FLG='1' and  IWD_IWM_CODE=IWM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Bill Passing";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    }
                    else if (CommonClasses.CheckUsedInTran("INWARD_DETAIL,INWARD_MASTER", "IWD_IWM_CODE", "AND IWD_I_CODE='" + item_code + "' and IWD_MODVAT_FLG='1' and  IWD_IWM_CODE=IWM_CODE and INWARD_MASTER.ES_DELETE=0", cpom_code))
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it has used in Credit Entry";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    }
                    //for check qty not going less than current stock
                    else if ((Convert.ToDouble(DtItemMaster.Rows[0]["I_CURRENT_BAL"].ToString()) - Convert.ToDouble(dtinsecption.Rows[0]["INSM_OK_QTY"].ToString())) < 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You cant delete this record it's qty Is used In other transacrion";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    }
                    else if (CommonClasses.Execute1("update INWARD_DETAIL set IWD_CON_OK_QTY=0,IWD_CON_REJ_QTY=0,IWD_CON_SCRAP_QTY=0,IWD_INSP_NO='',IWD_INSP_FLG=0 where IWD_IWM_CODE='" + cpom_code + "' and IWD_I_CODE='" + item_code + "' "))
                    {
                        for (int k = 0; k < dtinsecption.Rows.Count; k++)
                        {
                            CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL-" + dtinsecption.Rows[k]["INSM_OK_QTY"] + " where  I_CODE='" + item_code + "'");
                        }
                        CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_I_CODE='" + item_code + "'   AND STL_DOC_NO='" + cpom_code + "'     AND STL_DOC_NUMBER='" + INWARD_NO + "' AND STL_DOC_TYPE='" + Type + "'");
                        if (Type == "IWIAP")
                        {
                            CommonClasses.Execute("DELETE FROM STOCK_LEDGER WHERE STL_I_CODE='" + item_code + "' AND STL_DOC_NO='" + cpom_code + "'  AND STL_DOC_NUMBER='" + INWARD_NO + "' AND STL_DOC_TYPE='SubContractorRejection'");
                        }
                        CommonClasses.Execute1("update INSPECTION_S_MASTER set ES_DELETE=1 where INSM_NO='" + insp_no + "'");
                      
                        CommonClasses.WriteLog("Material Inspection", "Delete", "Material Inspection", item_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        ViewRec(type);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Not Deleted..";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    PanelMsg.Visible = true;
                    lblmsg.Text = "These Is Not Inspected Record";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection-ADD", "dgMaterialAcceptance_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgMaterialAcceptance_RowDeleting
    protected void dgMaterialAcceptance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgMaterialAcceptance_PageIndexChanging
    protected void dgMaterialAcceptance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgMaterialAcceptance.PageIndex = e.NewPageIndex;
        }
        catch (Exception)
        {
        }
    }
    #endregion
    #endregion

    #region ButtonEvent
    #region btnSave_Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            btnSubmit.Attributes.Add("onclick", "javascript:" + btnSubmit.ClientID + ".disabled=true;" + ClientScript.GetPostBackEventReference(btnSubmit, ""));

            string ReceivedQty = "";
            chkMaterialAcceptance_CheckedChanged(null, null);
            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {
                ReceivedQty = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIMD_ISSUE_QTY")).Text;
                string OkQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text.Trim();
                string RejQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text;
                string ItemCode = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODENO")).Text;
                CheckBox lblchk = ((CheckBox)dgMaterialAcceptance.Rows[i].FindControl("lblchk"));
                if (lblchk.Checked != true)
                {
                    if (OkQty == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Quantity For ..." + ItemCode;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }
                    if (RejQty == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Quantity For ..." + ItemCode;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        return;
                    }
                }
            }
            chkMaterialAcceptance.Checked = true;
            chkMaterialAcceptance_CheckedChanged(null, null);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Acceptance", "btnSave_Click", Ex.Message);
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
                CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
                Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
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
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnInspCancel_Click
    protected void btnInspCancel_Click(object sender, EventArgs e)
    {
        CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
        Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
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

    #region txtRejQty_TextChanged
    protected void txtRejQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtRejQty.Text);
            txtRejQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            if (Convert.ToDouble(txtRejQty.Text) > Convert.ToDouble(txtrecQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Rejected Quantity Not Greater Than Received Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtRejQty.Text = "0";
                return;
            }
            else
            {
                if (txtScrapQty.Text == "")
                {
                    txtScrapQty.Text = "0";
                }
                if (txtRejQty.Text == "")
                {
                    txtRejQty.Text = "0";
                }
                txtOkQty.Text = (Convert.ToDouble(txtrecQty.Text.ToString()) - (Convert.ToDouble(txtScrapQty.Text.ToString()) + Convert.ToDouble(txtRejQty.Text.ToString()))).ToString();

                if (Convert.ToDouble(txtOkQty.Text) < 0)
                {
                    txtOkQty.Text = "0.000";
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Sum of (Ok Qty,  Reject Qty,  Scrap Qty) Should not be Greater than  Receive Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
                txtOkQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtOkQty.Text));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspetion", "txtRejQty_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtScrapQty_TextChanged
    protected void txtScrapQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string totalStr = DecimalMasking(txtScrapQty.Text);

            txtScrapQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            if (Convert.ToDouble(txtScrapQty.Text) > Convert.ToDouble(txtrecQty.Text))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Scrap Quantity Not Greater Than Received Qty";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            else
            {
                if (txtScrapQty.Text == "")
                {
                    txtScrapQty.Text = "0";
                }
                if (txtRejQty.Text == "")
                {
                    txtRejQty.Text = "0";
                }
                txtOkQty.Text = (Convert.ToDouble(txtrecQty.Text.ToString()) - (Convert.ToDouble(txtScrapQty.Text.ToString()) + Convert.ToDouble(txtRejQty.Text.ToString()))).ToString();

                if (Convert.ToDouble(txtOkQty.Text) < 0)
                {
                    txtOkQty.Text = "0.000";
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Sum of (Ok Qty,  Reject Qty,  Scrap Qty) Should not be Greater than  Receive Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
                txtOkQty.Text = string.Format("{0:0.000}", Convert.ToDouble(txtOkQty.Text));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspetion", "txtScrapQty_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion
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
            CommonClasses.SendError("Material Inspection", "btnOk_Click", Ex.Message);
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
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("ISSUE_MASTER", "MODIFY", "IM_CODE", mlCode);
            }
            CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
            Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Material Inspection", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (panelDetail.Visible == true)
            {
                if (Convert.ToDouble(txtOkQty.Text) == 0.000)
                {
                    flag = false;
                }
                Double RecQty = Convert.ToDouble(txtOkQty.Text) + Convert.ToDouble(txtRejQty.Text) + Convert.ToDouble(txtScrapQty.Text);
                if (Convert.ToDouble(txtrecQty.Text) != RecQty)
                {
                    flag = false;
                }
                else if (Convert.ToDouble(txtRejQty.Text) > 0 && txtReason.Text == "")
                {
                    flag = false;
                }
                else if (txtRemark.Text == "")
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                flag = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inward", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region chkPDR_CheckedChanged
    protected void chkPDR_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPDR.Checked == true)
        {
            txtPDRNo.Enabled = true;
        }
        else
        {
            txtPDRNo.Enabled = false;
        }
    }
    #endregion chkPDR_CheckedChanged

    #region chkMaterialAcceptance_CheckedChanged
    protected void chkMaterialAcceptance_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkMaterialAcceptance.Checked)
            {
                string ReceivedQty = "";

                for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                {
                    string narra2 = "ACCEPT FROM " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblFROM_STORE_NAME")).Text + " - " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_NAME")).Text;

                    ReceivedQty = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIMD_ISSUE_QTY")).Text;
                    string OkQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text.Trim();
                    string RejQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text;
                    string ItemCode = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODENO")).Text;
                    CheckBox lblchk = ((CheckBox)dgMaterialAcceptance.Rows[i].FindControl("lblchk"));
                    if (lblchk.Checked != true)
                    {
                        if (OkQty == "")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Please Insert Quantity For ..." + ItemCode;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                        if (RejQty == "")
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Please Insert Quantity For ..." + ItemCode;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                        
                        DataTable dtexist = CommonClasses.Execute("SELECT * FROM STOCK_LEDGER where  STL_DOC_TYPE='" + narra2 + "' AND STL_I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "' AND STL_DOC_NUMBER='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_NO")).Text + "' AND STL_DOC_NO='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "'");
                        if (dtexist.Rows.Count>0)
                        {
                             PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Accepted  For ..." + ItemCode;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                }
                for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                {
                    ReceivedQty = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIMD_ISSUE_QTY")).Text;
                    string OkQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text.Trim();
                    string RejQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text;
                    CheckBox lblchk = ((CheckBox)dgMaterialAcceptance.Rows[i].FindControl("lblchk"));
                    if (lblchk.Checked != true)
                    {
                        string strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
                        SqlTransaction trans;

                        SqlConnection connection = new SqlConnection(strConnString);
                        connection.Open();
                        trans = connection.BeginTransaction();
                        try
                        {
                            #region validation
                            if (OkQty == "")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Please Insert Quantity ...";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            }
                            if (RejQty == "")
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "Please Insert Quantity ...";
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                                return;
                            } 
                            #endregion
                            #region accept QTY
                            if (Convert.ToDecimal(OkQty) > 0)
                            {
                                string narra = "ACCEPT FROM " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblFROM_STORE_NAME")).Text + " - " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_NAME")).Text;

                                SqlCommand command = new SqlCommand(" INSERT INTO STOCK_LEDGER (STL_I_CODE	,STL_DOC_NO	,STL_DOC_NUMBER	,STL_DOC_TYPE	,STL_DOC_DATE	,STL_DOC_QTY ,STL_STORE_TYPE) VALUES('" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_NO")).Text + "','" + narra + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_DATE")).Text + "','" + ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_CODE")).Text + "')", connection, trans);
                                command.Transaction = trans;
                                command.ExecuteNonQuery();

                                if (((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_CODE")).Text == "-2147483642")
                                {
                                    SqlCommand command1 = new SqlCommand(" UPDATE ITEM_MASTER SET I_DISPATCH_BAL = ISNULL(I_DISPATCH_BAL,0)+'" + OkQty.ToString() + "'  WHERE     I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "' ", connection, trans);
                                    command1.Transaction = trans;
                                    command1.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCommand command2 = new SqlCommand(" UPDATE ITEM_MASTER SET I_CURRENT_BAL = I_CURRENT_BAL+'" + OkQty.ToString() + "'  WHERE     I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "'  ", connection, trans);
                                    command2.Transaction = trans;
                                    command2.ExecuteNonQuery();
                                }
                            } 
                            #endregion
                            #region rejection QTY
                            if (Convert.ToDecimal(RejQty) > 0)
                            {
                                string narra = "RETURN FROM " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_NAME")).Text + " - " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblFROM_STORE_NAME")).Text;

                                int Po_Doc_no = 0;

                                SqlCommand cmd1 = new SqlCommand("Select isnull(max(IM_NO),0) as IM_NO FROM ISSUE_MASTER WHERE IM_COMP_ID = '" + (string)Session["CompanyCode"] + "' and ES_DELETE=0 and IM_FROM_STORE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_CODE")).Text + "' AND IM_TRANS_TYPE=1", connection, trans);
                                cmd1.Transaction = trans;
                                SqlDataReader dr1 = cmd1.ExecuteReader();
                                while (dr1.Read())
                                {
                                    Po_Doc_no = Convert.ToInt32(dr1[0].ToString().Trim());
                                    Po_Doc_no = Po_Doc_no + 1;
                                }
                                cmd1.Dispose();
                                dr1.Dispose();
                                if (CommonClasses.Execute1("INSERT INTO ISSUE_MASTER (IM_COMP_ID,IM_NO,IM_DATE,IM_TYPE,IM_MATERIAL_REQ,IM_ISSUEBY,IM_REQBY,IM_UM_CODE,IM_FROM_STORE,IM_TRANS_TYPE,IM_DIRECT_POST)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Po_Doc_no + "','" + Convert.ToDateTime(System.DateTime.Now).ToString("dd/MMM/yyyy") + "','2','0','" + Session["Username"].ToString() + "',' ','" + Convert.ToInt32(Session["UserCode"].ToString()) + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_CODE")).Text + "',1,1)"))
                                {
                                    string Code = CommonClasses.GetMaxId("Select Max(IM_CODE) from ISSUE_MASTER");

                                    string ToStore = "ISS " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_NAME")).Text + " - " + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblFROM_STORE_NAME")).Text;
                                    //Inserting Into Issue To Production Detail
                                    double PTS_QTY = 0;

                                    PTS_QTY = Convert.ToDouble(RejQty);

                                    SqlCommand command5 = new SqlCommand("  INSERT INTO ISSUE_MASTER_DETAIL(IMD_COMP_ID,IM_CODE,IMD_I_CODE,IMD_UOM,IMD_CURR_STOCK,IMD_REQ_QTY,IMD_ISSUE_QTY,IMD_REMARK,IMD_RATE,IMD_AMOUNT,IMD_To_STORE) values ('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Code + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','-2147483648','0',' " + PTS_QTY + "','" + PTS_QTY + "','Retrun Short Qty','0','0','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblFROM_STORE_CODE")).Text + "')", connection, trans);
                                    command5.Transaction = trans;
                                    command5.ExecuteNonQuery();


                                }
                            } 
                            #endregion
                            // ISSUE_MASTER_DETAIL :- Update IMD_STORE_TYPE = 1 flag 
                            #region update Master Entry
                            if (TRANS_TYPE == 0)
                            {
                                SqlCommand command3 = new SqlCommand(" UPDATE ISSUE_MASTER_DETAIL SET IMD_STORE_TYPE = 1  ,IMD_REC_QTY='" + OkQty + "',IMD_SHORT_QTY='" + RejQty + "'  ,IMD_REC_DATE='" + Convert.ToDateTime(System.DateTime.Now).ToString("dd/MMM/yyyy HH:mm") + "' ,IMD_REC_BY='" + Convert.ToInt32(Session["UserCode"].ToString()) + "'  WHERE IMD_To_STORE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblTO_STORE_CODE")).Text + "' AND ISSUE_MASTER_DETAIL.IM_CODE = '" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "' AND IMD_I_CODE= '" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "'  ", connection, trans);
                                command3.Transaction = trans;
                                command3.ExecuteNonQuery();
                            }
                            else
                            {
                                SqlCommand command4 = new SqlCommand(" UPDATE REJECTION_TO_FOUNDRY_MASTER SET RTF_ISUSED = 1  ,RTF_OK_QTY='" + OkQty + "',RTF_REJ_QTY='" + RejQty + "'  ,RTF_REC_DATE='" + Convert.ToDateTime(System.DateTime.Now).ToString("dd/MMM/yyyy HH:mm") + "',RTF_REC_BY='" + Convert.ToInt32(Session["UserCode"].ToString()) + "'  WHERE  RTF_CODE = '" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIM_CODE")).Text + "' AND RTF_I_CODE= '" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "' ", connection, trans);
                                command4.Transaction = trans;
                                command4.ExecuteNonQuery();
                            } 
                            #endregion
                            trans.Commit();
                        }
                        catch (Exception ex) //error occurred
                        {
                            trans.Rollback();
                            //Handel error
                            CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
                            Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
                        }
                    }
                }
                CommonClasses.WriteLog("Material Acceptance", "Save", "Material Acceptance", Convert.ToString(IM_No), Convert.ToInt32(IM_No), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
                Response.Redirect("~/Transactions/View/ViewMaterialAcceptance.aspx", false);
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion chkMaterialAcceptance_CheckedChanged

    #region lblOK_Qty_TextChanged
    protected void lblOK_Qty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {
                string ReceivedQty = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIMD_ISSUE_QTY")).Text;
                string OkQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text;
                string RejQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text;

                ReceivedQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(ReceivedQty), 3));
                OkQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(OkQty), 3));
                RejQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(RejQty), 3));
                if (Convert.ToDouble(OkQty) > Convert.ToDouble(ReceivedQty))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Ok Quantity Not Greater Than Received Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text = "0";
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Focus();
                    return;
                }
                if (Convert.ToDouble(OkQty) >= 0)
                {
                    RejQty = (Convert.ToDouble(ReceivedQty) - Convert.ToDouble(OkQty)).ToString();
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text = RejQty;
                }
            }
        }
        catch
        {

        }
    }
    #endregion lblOK_Qty_TextChanged

    #region lblRej_Qty_TextChanged
    protected void lblRej_Qty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {
                string ReceivedQty = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblIMD_ISSUE_QTY")).Text;
                string OkQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text;
                string RejQty = ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text;

                ReceivedQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(ReceivedQty), 3));
                OkQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(OkQty), 3));
                RejQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(RejQty), 3));

                if (Convert.ToDouble(RejQty) > Convert.ToDouble(ReceivedQty))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Rejected Quantity Not Greater Than Received Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Text = "0";
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblRej_Qty")).Focus();
                    return;
                }
                if (Convert.ToDouble(RejQty) > 0)
                {
                    OkQty = (Convert.ToDouble(ReceivedQty) - Convert.ToDouble(RejQty)).ToString();
                    ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("lblOK_Qty")).Text = OkQty;
                }
            }
        }
        catch
        {
        }
    }
    #endregion lblRej_Qty_TextChanged
}
