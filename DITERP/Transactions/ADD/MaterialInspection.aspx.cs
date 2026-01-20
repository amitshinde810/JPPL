using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;

public partial class Transactions_ADD_MaterialInspection : System.Web.UI.Page
{
    #region Variable
    static int mlCode = 0;
    static int InwardNo = 0;
    MaterialInspection_BL inspect_BL = new MaterialInspection_BL();
    static DataTable inspectiondt = new DataTable();
    static DataTable dt = new DataTable();
    static string type = "";
    static string p_type = "";
    static string Iwm_type = "";
    public int icode;
    string fileName = "";
    string fileName2 = "";
    string fileNameD = "";
    DirectoryInfo ObjSearchDir;
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
                    p_type = Request.QueryString[2].ToString();
                    Iwm_type = Request.QueryString[3].ToString();
                    ViewState["InwardNo"] = "0";
                    ViewState["dt"] = dt;
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["InwardNo"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("VIEW");
                        type = "VIEW";
                        lblType.Text = "Inspected List";
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["InwardNo"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                        type = "MOD";
                        lblType.Text = "Inspected List";
                        CommonClasses.SetModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["InwardNo"].ToString()));
                    }
                    else if (Request.QueryString[0].Equals("ADD"))
                    {
                        txtInspDate.Attributes.Add("readonly", "readonly");

                        ViewState["InwardNo"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("ADD");
                        type = "ADD";
                        lblType.Text = "Pending List";
                    }
                    else if (Request.QueryString[0].Equals("ADDHOLD"))
                    {
                        txtInspDate.Attributes.Add("readonly", "readonly");

                        ViewState["InwardNo"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("ADDHOLD");
                        type = "ADD";
                        lblType.Text = "Pending List";
                    }
                    FillCombo();
                }
                if (IsPostBack && FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.PostedFile.FileName.Length > 0)
                    {
                        fileName2 = FileUpload1.PostedFile.FileName;
                        ViewState["fileName2"] = fileName2;
                        Upload2(null, null);
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

    #region Upload2
    protected void Upload2(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("ADD"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/Inspection/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("ADD"))
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/Inspection/" + ViewState["fileName2"].ToString()));
            }
            else
            {
                FileUpload1.SaveAs(Server.MapPath("~/UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName2"].ToString()));
            }
            lnkTModel.Visible = true;
            lnkTModel.Text = ViewState["fileName2"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region lnkuploadTModel_Click
    protected void lnkuploadTModel_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("ADD"))
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/Inspection/" + filePath;
            }
            else
            {
                filePath = lnkTModel.Text;
                directory = "../../UpLoadPath/Inspection/" + ViewState["mlCode"].ToString() + "/" + filePath;
            }

            ModalPopupExtenderDovView.Show();
            myframe.Attributes["src"] = directory;


        }
        catch (Exception ex)
        {
            CommonClasses.SendError("upplier Master Entry", "lnkupload_Click", ex.Message);
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtInspDate.Attributes.Add("readonly", "readonly");
            #region VIEW
            if (str == "VIEW")
            {
                if (p_type == "0")
                {
                    inspectiondt = CommonClasses.Execute("SELECT CR_CODE AS IWM_CODE,CD_I_CODE AS IWD_I_CODE,CD_INSP_NO AS IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,ROUND(CD_RATE,2) AS IWD_RATE,P_CODE AS  IWM_P_CODE,I_CODENO,CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER,CUSTPO_MASTER,ITEM_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0  AND CR_P_CODE=P_CODE AND I_CODE=CD_I_CODE AND CPOM_CODE=CD_PO_CODE  AND CD_INSP_FLG=1 and CR_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "'  AND  CR_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  ");
                }
                else
                {
                    if (Iwm_type == "IWIFP")
                    {
                        inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,CPOM_PONO as SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,CUSTPO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=CUSTPO_MASTER.CPOM_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                    else
                    {
                        inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                }
                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = true;
                    dgInspection.Columns[1].Visible = false;
                    dgInspection.Columns[2].Visible = false;
                    dgInspection.Columns[3].Visible = false;
                    dgInspection.Columns[4].Visible = false;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }
            }
            #endregion
            #region ADD
            else if (str == "ADD")
            {
                txtInspDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                if (p_type == "0")
                {
                    inspectiondt = CommonClasses.Execute(" SELECT CR_CODE  AS IWM_CODE,CD_I_CODE AS IWD_I_CODE,CD_INSP_NO AS IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,ROUND(CD_RATE,2) AS IWD_RATE,P_CODE AS  IWM_P_CODE,I_CODENO,CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER,CUSTPO_MASTER,ITEM_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0  AND CR_P_CODE=P_CODE AND I_CODE=CD_I_CODE AND CPOM_CODE=CD_PO_CODE  AND CD_INSP_FLG=0 and CR_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                }
                else
                {
                    if (Iwm_type == "IWIFP")
                    {
                        inspectiondt = CommonClasses.Execute("SELECT IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,CPOM_PONO as SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,CUSTPO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0  AND ISNULL(IWD_HOLD,0)=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=CUSTPO_MASTER.CPOM_CODE AND IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                    else
                    {
                        inspectiondt = CommonClasses.Execute("SELECT IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0  AND ISNULL(IWD_HOLD,0)=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                }

                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = false;
                    dgInspection.Columns[1].Visible = false;
                    dgInspection.Columns[2].Visible = true;
                    dgInspection.Columns[3].Visible = false;
                    if (p_type == "1")
                    {
                        dgInspection.Columns[4].Visible = true;
                    }
                    else
                    {
                        dgInspection.Columns[4].Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
            }
            #endregion
            #region ADD
            else if (str == "ADDHOLD")
            {
                txtInspDate.Text = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                if (p_type == "0")
                {
                    inspectiondt = CommonClasses.Execute("SELECT CR_CODE  AS IWM_CODE,CD_I_CODE AS IWD_I_CODE,CD_INSP_NO AS IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,ROUND(CD_RATE,2) AS IWD_RATE,P_CODE AS  IWM_P_CODE,I_CODENO,CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER,CUSTPO_MASTER,ITEM_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0  AND CR_P_CODE=P_CODE AND I_CODE=CD_I_CODE AND CPOM_CODE=CD_PO_CODE  AND CD_INSP_FLG=0 and CR_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                }
                else
                {
                    if (Iwm_type == "IWIFP")
                    {
                        inspectiondt = CommonClasses.Execute("SELECT IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,CUSTPO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 AND ISNULL(IWD_HOLD,0)=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=CUSTPO_MASTER.CPOM_CODE AND IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                    else
                    {
                        inspectiondt = CommonClasses.Execute("SELECT IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 AND ISNULL(IWD_HOLD,0)=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                }

                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = false;
                    dgInspection.Columns[1].Visible = false;
                    dgInspection.Columns[2].Visible = true;
                    dgInspection.Columns[3].Visible = false;
                    dgInspection.Columns[4].Visible = false;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
            }
            #endregion
            #region MOD
            else if (str == "MOD")
            {
                if (p_type == "0")
                {
                    inspectiondt = CommonClasses.Execute(" SELECT CR_CODE AS  IWM_CODE,CD_I_CODE AS IWD_I_CODE,CD_INSP_NO AS IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,ROUND(CD_RATE,2) AS IWD_RATE,P_CODE AS IWM_P_CODE,I_CODENO,CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE    FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER,ITEM_MASTER,CUSTPO_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0   AND CD_INSP_FLG=1 AND P_CODE=CR_P_CODE  AND CD_I_CODE=I_CODE AND CPOM_CODE=CD_PO_CODE    and  CR_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and CR_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                }
                else
                {
                    if (Iwm_type == "IWIFP")
                    {
                        inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,CUSTPO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=CUSTPO_MASTER.CPOM_CODE and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                    else
                    {
                        inspectiondt = CommonClasses.Execute("select IWM_CODE,IWD_I_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE,I_CODENO,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER where INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                }
                if (inspectiondt.Rows.Count > 0)
                {
                    dgInspection.DataSource = inspectiondt;
                    dgInspection.DataBind();
                    dgInspection.Visible = true;
                    dgInspection.Columns[0].Visible = true;
                    dgInspection.Columns[1].Visible = true;
                    dgInspection.Columns[2].Visible = false;
                    dgInspection.Columns[3].Visible = true;
                    dgInspection.Columns[4].Visible = false;
                }
                else
                {
                    Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
                }
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
            }
            #endregion
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
            #region "VIEW"
            if (str == "VIEW" || str == "MOD")
            {
                if (p_type == "0")
                {
                    ViewState["dt"] = CommonClasses.Execute("SELECT I_CODENO,'CustRej' AS IWM_TYPE,I_CURRENT_BAL,ISNULL(INSM_FILE,'') AS INSM_FILE ,INSM_CODE,INSM_REMARK,convert(varchar,INSM_DATE,106) as INSM_DATE,CD_CR_CODE AS IWD_IWM_CODE,CD_I_CODE AS IWD_I_CODE,CD_PO_CODE AS IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,CD_INSP_NO  AS IWD_INSP_NO,I_NAME,CPOM_PONO AS SPOM_PO_NO,cast(isnull(CD_RECEIVED_QTY,0) as numeric(10,3)) as IWD_REV_QTY, cast(isnull(INSM_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY, cast(isnull(INSM_REJ_QTY,0) as numeric(10,3))  as IWD_CON_REJ_QTY,  cast(isnull(INSM_SCRAP_QTY,0) as numeric(10,3))    as  IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME,ISNULL(INSM_PDR_CHECK,0) AS INSM_PDR_CHECK,INSM_PDR_NO,ROUND(CD_RATE,2) AS IWD_RATE,INSM_STORE_TYPE,CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,P_CODE AS  IWM_P_CODE  FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER,ITEM_MASTER,CUSTPO_MASTER,ITEM_UNIT_MASTER,INSPECTION_S_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0 AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE   AND CD_INSP_FLG=1 AND P_CODE=CR_P_CODE AND INSM_IWM_CODE=CR_CODE  AND CD_I_CODE=I_CODE AND CPOM_CODE=CD_PO_CODE AND INSPECTION_S_MASTER.ES_DELETE=0 AND CD_I_CODE='" + icode + "' and  CR_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and  INSM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and  CR_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "'   AND INSM_STORE_TYPE IS NOT NULL");
                }
                else
                {
                    if (Iwm_type == "IWIFP")
                    {
                        ViewState["dt"] = CommonClasses.Execute("select I_CODENO,IWM_TYPE,I_CURRENT_BAL,ISNULL(INSM_FILE,'') AS INSM_FILE ,INSM_CODE,INSM_REMARK,convert(varchar,INSM_DATE,106) as INSM_DATE,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,CPOM_PONO as SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME,ISNULL(INSM_PDR_CHECK,0) AS INSM_PDR_CHECK,INSM_PDR_NO,ROUND(IWD_RATE,2) AS IWD_RATE,INSM_STORE_TYPE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_P_CODE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,CUSTPO_MASTER,ITEM_UNIT_MASTER,INSPECTION_S_MASTER where INSPECTION_S_MASTER.ES_DELETE=0 AND INSM_IWM_CODE=IWM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 AND IWD_I_CODE=INSM_I_CODE and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=CUSTPO_MASTER.CPOM_CODE AND IWD_I_CODE='" + icode + "' and IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                    else
                    {
                        ViewState["dt"] = CommonClasses.Execute("select I_CODENO,IWM_TYPE,I_CURRENT_BAL,ISNULL(INSM_FILE,'') AS INSM_FILE ,INSM_CODE,INSM_REMARK,convert(varchar,INSM_DATE,106) as INSM_DATE,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME,ISNULL(INSM_PDR_CHECK,0) AS INSM_PDR_CHECK,INSM_PDR_NO,ROUND(IWD_RATE,2) AS IWD_RATE,INSM_STORE_TYPE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWM_P_CODE from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER,INSPECTION_S_MASTER where INSPECTION_S_MASTER.ES_DELETE=0 AND INSM_IWM_CODE=IWM_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE and INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=1 AND IWD_I_CODE=INSM_I_CODE  and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE AND IWD_I_CODE='" + icode + "' and  IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                }
                if (((DataTable)ViewState["dt"]).Rows.Count > 0)
                {
                    ViewState["mlCode"] = ((DataTable)ViewState["dt"]).Rows[0]["INSM_CODE"].ToString();
                    lblGRNCODE.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_IWM_CODE"].ToString();
                    txtInspNo.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_INSP_NO"].ToString();
                    txtPONumber.Text = ((DataTable)ViewState["dt"]).Rows[0]["SPOM_PO_NO"].ToString();
                    txtItemName.Text = ((DataTable)ViewState["dt"]).Rows[0]["I_NAME"].ToString();
                    txtInspDate.Text = Convert.ToDateTime(((DataTable)ViewState["dt"]).Rows[0]["INSM_DATE"].ToString()).ToString("dd/MMM/yyyy HH:mm");
                    txtrecQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_REV_QTY"].ToString();
                    txtUnitName.Text = ((DataTable)ViewState["dt"]).Rows[0]["UOM_NAME"].ToString();
                    txtOkQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_OK_QTY"].ToString();
                    txtRejQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_REJ_QTY"].ToString();
                    txtScrapQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_SCRAP_QTY"].ToString();
                    txtRemark.Text = ((DataTable)ViewState["dt"]).Rows[0]["INSM_REMARK"].ToString();
                    txtPDRNo.Text = ((DataTable)ViewState["dt"]).Rows[0]["INSM_PDR_NO"].ToString();
                    chkPDR.Checked = Convert.ToBoolean(((DataTable)ViewState["dt"]).Rows[0]["INSM_PDR_CHECK"].ToString());
                    txtRate.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_RATE"].ToString();
                    lblpartycode.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWM_P_CODE"].ToString();
                    txtItemCode.Text = ((DataTable)ViewState["dt"]).Rows[0]["I_CODENO"].ToString();
                    ddlStoreType.SelectedValue = ((DataTable)ViewState["dt"]).Rows[0]["INSM_STORE_TYPE"].ToString();
                    lblIwddate.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWM_DATE"].ToString();
                    lblIwdNo.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWM_NO"].ToString();


                    lnkTModel.Text = ((DataTable)ViewState["dt"]).Rows[0]["INSM_FILE"].ToString();
                    if (str == "MOD")
                    {
                        DataTable DtItemMaster = new DataTable();

                        if (((DataTable)ViewState["dt"]).Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                        {
                            DtItemMaster = CommonClasses.Execute("SELECT SUM(STL_DOC_QTY) AS I_CURRENT_BAL FROM STOCK_LEDGER where STL_I_CODE='" + icode + "'  AND  STL_STORE_TYPE='" + ddlStoreType.SelectedValue + "'");

                            if ((Convert.ToDouble(DtItemMaster.Rows[0]["I_CURRENT_BAL"].ToString()) - (Convert.ToDouble(((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_OK_QTY"].ToString()) + Convert.ToDouble(((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_REJ_QTY"].ToString()))) < 0)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You cant Modify this record, it's qty Is used In other transaction";
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
                            DtItemMaster = CommonClasses.Execute("SELECT SUM(STL_DOC_QTY) AS I_CURRENT_BAL FROM STOCK_LEDGER where STL_I_CODE='" + icode + "'  AND  STL_STORE_TYPE='" + ddlStoreType.SelectedValue + "'");

                            if ((Convert.ToDouble(DtItemMaster.Rows[0]["I_CURRENT_BAL"].ToString()) - (Convert.ToDouble(((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_OK_QTY"].ToString()))) < 0)
                            {
                                PanelMsg.Visible = true;
                                lblmsg.Text = "You can't Modify this record, it's qty Is used In other transaction";
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
                        DataTable dtReason = CommonClasses.Execute("select INSD_RM_CODE as Reason from INSPECTION_S_DETAIL where INSD_INSM_CODE='" + ((DataTable)ViewState["dt"]).Rows[0]["INSM_CODE"].ToString() + "' and INSD_I_CODE='" + icode + "'");
                        if (dtReason.Rows.Count > 0)
                        {
                            txtReason.Text = dtReason.Rows[0]["Reason"].ToString();
                        }
                    }
                }
            }
            #endregion

            #region ASDD
            else if (str == "ADD")
            {
                if (p_type == "0")
                {
                    ViewState["dt"] = CommonClasses.Execute("SELECT I_CODENO,CD_CR_CODE AS IWD_IWM_CODE,CR_GIN_NO AS IWM_NO,convert(varchar,CR_GIN_DATE,106) as IWM_DATE,CD_I_CODE AS  IWD_I_CODE,CD_PO_CODE AS IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,CD_INSP_NO AS IWD_INSP_NO,I_NAME,CPOM_PONO AS  SPOM_PO_NO,cast(isnull(CD_RECEIVED_QTY,0)as numeric(10,3)) as IWD_REV_QTY,0 as IWD_CON_OK_QTY, 0 as IWD_CON_REJ_QTY, 0 as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(CD_RATE,2) AS  IWD_RATE,P_CODE AS IWM_P_CODE,-2147483648 AS SPOM_PLANT FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL,PARTY_MASTER,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER where CR_CODE=CD_CR_CODE AND CUSTREJECTION_MASTER.ES_DELETE=0  AND CR_P_CODE=P_CODE AND I_CODE=CD_I_CODE AND CPOM_CODE=CD_PO_CODE  AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  and CR_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' AND   CD_I_CODE='" + icode + "' ");
                }
                else
                {
                    if (Iwm_type == "IWIFP")
                    {
                        ViewState["dt"] = CommonClasses.Execute("select I_CODENO,IWD_IWM_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,CPOM_PONO as SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(IWD_RATE,2) AS IWD_RATE,IWM_P_CODE ,-2147483648 AS SPOM_PLANT from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,CUSTPO_MASTER,ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=CUSTPO_MASTER.CPOM_CODE AND IWD_I_CODE='" + icode + "' AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                    else
                    {
                        ViewState["dt"] = CommonClasses.Execute("select I_CODENO,IWD_IWM_CODE,IWM_NO,convert(varchar,IWM_DATE,106) as IWM_DATE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE ,SPOM_PLANT  from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE  AND IWD_I_CODE='" + icode + "' AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + Convert.ToInt32(ViewState["InwardNo"].ToString()) + "' ");
                    }
                }
                if (((DataTable)ViewState["dt"]).Rows.Count > 0)
                {
                    lblGRNCODE.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_IWM_CODE"].ToString();
                    txtInspNo.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_INSP_NO"].ToString();
                    txtPONumber.Text = ((DataTable)ViewState["dt"]).Rows[0]["SPOM_PO_NO"].ToString();
                    txtItemName.Text = ((DataTable)ViewState["dt"]).Rows[0]["I_NAME"].ToString();
                    txtItemCode.Text = ((DataTable)ViewState["dt"]).Rows[0]["I_CODENO"].ToString();
                    txtrecQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_REV_QTY"].ToString();
                    txtUnitName.Text = ((DataTable)ViewState["dt"]).Rows[0]["UOM_NAME"].ToString();
                    txtOkQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_REV_QTY"].ToString();
                    txtRejQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_REJ_QTY"].ToString();
                    txtScrapQty.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_CON_SCRAP_QTY"].ToString();
                    txtRate.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWD_RATE"].ToString();
                    lblpartycode.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWM_P_CODE"].ToString();
                    lblIwddate.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWM_DATE"].ToString();
                    lblIwdNo.Text = ((DataTable)ViewState["dt"]).Rows[0]["IWM_NO"].ToString();
                    rbtWithAmt.SelectedValue = ((DataTable)ViewState["dt"]).Rows[0]["SPOM_PLANT"].ToString();
                    rbtWithAmt_SelectedIndexChanged(null, null);
                }
            }
            #endregion
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
            inspect_BL.INSM_PTYPE = p_type.ToString();
            inspect_BL.INSM_STORE_TYPE = Convert.ToInt32(ddlStoreType.SelectedValue);
            inspect_BL.INSM_FILE = lnkTModel.Text;
            DataTable dtParty = new DataTable();

            dtParty = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + ViewState["InwardNo"].ToString() + "'");

            string INWARD_NO = lblIwdNo.Text.Trim();
            string Type = "";

            if (dtParty.Rows.Count > 0)
            {

                if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                {
                    inspect_BL.INSM_TYPE = "IWIAP";
                }
                else if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "IWIFP")
                {
                    inspect_BL.INSM_TYPE = "IWIFP";
                }
                else
                {
                    inspect_BL.INSM_TYPE = "IWIM";
                }
            }
            if (p_type == "0")
            {
                inspect_BL.INSM_TYPE = "CustRej";
            }

            if (inspect_BL.INSM_REJ_QTY > 0)
            {
                inspect_BL.INSD_RM_CODE = txtReason.Text;
            }
            if (((DataTable)ViewState["dt"]).Rows.Count > 0)
            {
                if (type == "MOD")
                {
                    inspect_BL.INSM_CODE = Convert.ToInt32(((DataTable)ViewState["dt"]).Rows[0]["INSM_CODE"].ToString());
                }
                inspect_BL.INSM_IWM_CODE = Convert.ToInt32(ViewState["InwardNo"].ToString());
                inspect_BL.INSM_SPOM_CODE = Convert.ToInt32(((DataTable)ViewState["dt"]).Rows[0]["IWD_CPOM_CODE"].ToString());
                inspect_BL.INSM_I_CODE = Convert.ToInt32(((DataTable)ViewState["dt"]).Rows[0]["IWD_I_CODE"].ToString());
                inspect_BL.INSM_UOM_CODE = Convert.ToInt32(((DataTable)ViewState["dt"]).Rows[0]["IWD_UOM_CODE"].ToString());

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
                    if (inspect_BL.Save(out mlCode))
                    {
                        #region FileUpload for 3D Model
                        if (inspect_BL.INSM_FILE != "")
                        {
                            string sDirPath13 = Server.MapPath(@"~/UpLoadPath/Inspection/" + mlCode + "");

                            ObjSearchDir = new DirectoryInfo(sDirPath13);

                            string sDirPath12 = Server.MapPath(@"~/UpLoadPath/Inspection/");
                            DirectoryInfo dir1 = new DirectoryInfo(sDirPath13);

                            dir1.Refresh();

                            if (!ObjSearchDir.Exists)
                            {
                                ObjSearchDir.Create();
                            }
                            string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                            //Get the full path of the file    
                            string fullFilePath1 = currentApplicationPath + "UpLoadPath\\Inspection ";
                            string fullFilePath = Server.MapPath(@"~/UpLoadPath/Inspection/" + lnkTModel.Text);
                            // Get the destination path
                            string copyToPath = Server.MapPath(@"~/UpLoadPath/Inspection/" + mlCode + "/" + lnkTModel.Text);
                            DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                            System.IO.File.Move(fullFilePath, copyToPath);
                        }
                        #endregion
                        CommonClasses.WriteLog("Material Inspection", "Save", "Material Inspection", "", mlCode, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        // CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["InwardNo"].ToString()));

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
                    if (p_type == "0")
                    {
                        dtItemMast = CommonClasses.Execute(" SELECT 'CustRej' AS IWM_TYPE,INSM_I_CODE,I_CURRENT_BAL, INSM_NO,INSM_TYPE,INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY  FROM INSPECTION_S_MASTER,CUSTREJECTION_MASTER,ITEM_MASTER where INSM_IWM_CODE=CR_CODE AND  INSM_IWM_CODE='" + lblGRNCODE.Text + "' AND INSPECTION_S_MASTER.ES_DELETE=0 AND I_CODE=INSM_I_CODE AND INSM_TYPE=0");
                    }
                    else
                    {
                        dtItemMast = CommonClasses.Execute(" SELECT IWM_TYPE,INSM_I_CODE,I_CURRENT_BAL, INSM_RECEIVED_QTY,INSM_OK_QTY,INSM_REJ_QTY,INSM_SCRAP_QTY  FROM INSPECTION_S_MASTER,INWARD_MASTER,ITEM_MASTER where INSM_IWM_CODE=IWM_CODE AND  INSM_IWM_CODE='" + lblGRNCODE.Text + "' AND INSPECTION_S_MASTER.ES_DELETE=0 AND I_CODE=INSM_I_CODE");
                    }
                    double Ok_Qty = 0;
                    if (dtItemMast.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                    {
                        Ok_Qty = Convert.ToDouble(txtOkQty.Text) + Convert.ToDouble(txtRejQty.Text);
                    }
                    else
                    {
                        Ok_Qty = Convert.ToDouble(txtOkQty.Text) + Convert.ToDouble(txtRejQty.Text);
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

                        // CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["InwardNo"].ToString()));
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
                ddlStoreType.Enabled = false;
                FileUpload1.Enabled = false;
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


    protected void dgPMPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT * FROM INSPECTION_REASONMASTER WHERE ES_DELETE=0 AND INSP_CM_ID=" + Session["CompanyId"].ToString() + "");

            if (dt.Rows.Count > 0)
            {
                DropDownList ddlReason = (DropDownList)e.Row.FindControl("ddlReason");

                ddlReason.DataSource = dt;

                ddlReason.DataTextField = "INSP_RDESC";

                ddlReason.DataValueField = "INSP_RCODE";
                ddlReason.Items.Insert(0, new ListItem("Select Reason ", "0"));
                ddlReason.DataBind();
            }
        }
    }
    #region dgInspection_RowCommand
    protected void dgInspection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;
            lblmsg.Text = "";
            if (e.CommandName.Equals("View"))
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                ViewInspection("VIEW", cpom_code);
            }
            else if (e.CommandName.Equals("Modify"))
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                ViewInspection(type, cpom_code);
            }
            else if (e.CommandName.Equals("HOLD"))
            {
                //int index = Convert.ToInt32(e.CommandArgument.ToString());
                //string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                //ViewInspection(type, cpom_code);
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                LinkButton cpom_code = ((LinkButton)(dgInspection.Rows[index].FindControl("lnkHold")));
                if (cpom_code.Text.ToLower() == "hold")
                {
                    dgInspection.Columns[2].Visible = false;
                    dgInspection.Columns[5].Visible = true;
                    cpom_code.Text = "UPDATE";
                }
                else
                {
                    DropDownList ddl = ((DropDownList)(dgInspection.Rows[index].FindControl("ddlReason")));
                    string Iwd_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_CODE"))).Text;
                    string I_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                    if (ddl.SelectedValue != "0")
                    {
                        CommonClasses.Execute("UPDATE INWARD_DETAIL SET IWD_HOLD=1,IWD_HOLDR='" + ddl.SelectedValue + "',IWD_HDATE=GETDATE()  where IWD_IWM_CODE='" + Iwd_code + "'   AND IWD_I_CODE='" + I_code + "'");
                        ViewRec("ADD");
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please select Reason for Hold";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    }
                }
            }
            else if (e.CommandName.Equals("Delete"))
            {
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string cpom_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_CODE"))).Text;
                string item_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_I_CODE"))).Text;
                string insp_no = ((Label)(dgInspection.Rows[index].FindControl("lblIWD_INSP_NO"))).Text;
                string p_code = ((Label)(dgInspection.Rows[index].FindControl("lblIWM_P_CODE"))).Text;

                DataTable dtParty = new DataTable();
                if (p_type == "0")
                {
                    dtParty = CommonClasses.Execute("SELECT CR_CODE AS IWM_CODE, CR_GIN_NO AS IWM_NO, CR_GIN_DATE AS IWM_DATE ,'CustRej' AS IWM_TYPE FROM CUSTREJECTION_MASTER WHERE CR_CODE='" + cpom_code + "'");
                }
                else
                {
                    dtParty = CommonClasses.Execute("SELECT * FROM INWARD_MASTER WHERE IWM_CODE='" + cpom_code + "'");
                }

                string INWARD_NO = dtParty.Rows[0]["IWM_NO"].ToString();
                string Type = "";
                if (dtParty.Rows.Count > 0)
                {
                    if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "OUTCUSTINV")
                    {
                        Type = "IWIAP";
                    }
                    else if (dtParty.Rows[0]["IWM_TYPE"].ToString() == "IWIFP")
                    {
                        Type = "IWIFP";
                    }
                    else
                    {
                        Type = "IWIM";
                    }
                }
                if (p_type == "0")
                {
                    Type = "CustRej";
                }
                if (type == "MOD")
                {
                    DataTable dtinsecption = new DataTable();

                    DataTable DtItemMaster = new DataTable();
                    if (Type == "IWIAP")
                    {
                        dtinsecption = CommonClasses.Execute("  SELECT INSM_OK_QTY+INSM_REJ_QTY AS INSM_OK_QTY,INSM_STORE_TYPE FROM INSPECTION_S_MASTER where    INSM_I_CODE='" + item_code + "'  AND  INSM_NO='" + insp_no + "' AND ES_DELETE=0  ");
                        DtItemMaster = CommonClasses.Execute("SELECT SUM(STL_DOC_QTY) AS I_CURRENT_BAL FROM STOCK_LEDGER where STL_I_CODE='" + item_code + "'  AND  STL_STORE_TYPE='" + dtinsecption.Rows[0]["INSM_STORE_TYPE"].ToString() + "'");
                    }
                    else
                    {
                        dtinsecption = CommonClasses.Execute("  SELECT INSM_OK_QTY+INSM_REJ_QTY AS INSM_OK_QTY,INSM_STORE_TYPE FROM INSPECTION_S_MASTER where    INSM_I_CODE='" + item_code + "'  AND  INSM_NO='" + insp_no + "'   AND ES_DELETE=0 ");
                        DtItemMaster = CommonClasses.Execute("SELECT SUM(STL_DOC_QTY) AS I_CURRENT_BAL FROM STOCK_LEDGER where STL_I_CODE='" + item_code + "' AND  STL_STORE_TYPE='" + dtinsecption.Rows[0]["INSM_STORE_TYPE"].ToString() + "'");
                    }
                    if (p_type == "0")
                    {
                        if (CommonClasses.CheckUsedInTran("CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL", "CD_CR_CODE", "AND CD_I_CODE='" + item_code + "' and CD_MODVAT_FLG='1' and    CR_CODE=CD_CR_CODE and CUSTREJECTION_MASTER.ES_DELETE=0", cpom_code))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "You cant delete this record it has used in Credit Entry";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        }
                    }
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
                    else
                    {
                        if (p_type == "0")
                        {
                            CommonClasses.Execute1("update CUSTREJECTION_DETAIL set  CD_INSP_NO='',CD_INSP_FLG=0 where CD_CR_CODE='" + cpom_code + "' and CD_I_CODE='" + item_code + "' ");
                        }
                        else
                        {
                            CommonClasses.Execute1("update INWARD_DETAIL set IWD_CON_OK_QTY=0,IWD_CON_REJ_QTY=0,IWD_CON_SCRAP_QTY=0,IWD_INSP_NO='',IWD_INSP_FLG=0 where IWD_IWM_CODE='" + cpom_code + "' and IWD_I_CODE='" + item_code + "' ");
                        }
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
                        DataTable dtInward = new DataTable();
                        if (p_type == "0")
                        {
                            dtInward = CommonClasses.Execute("SELECT CR_CODE AS IWM_CODE, CR_GIN_NO AS IWM_NO, CR_GIN_DATE AS IWM_DATE,CD_RECEIVED_QTY AS IWD_REV_QTY  FROM CUSTREJECTION_MASTER,CUSTREJECTION_DETAIL WHERE  CR_CODE=CD_CR_CODE AND CR_CODE='" + cpom_code + "' and CD_I_CODE='" + item_code + "' ");
                        }
                        else
                        {
                            dtInward = CommonClasses.Execute("SELECT * FROM INWARD_MASTER,INWARD_DETAIL WHERE  IWM_CODE=IWD_IWM_CODE AND   IWM_CODE='" + cpom_code + "'  and IWD_I_CODE='" + item_code + "'");

                        }
                        CommonClasses.Execute("INSERT INTO STOCK_LEDGER (STL_I_CODE ,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY,STL_STORE_TYPE) VALUES ('" + item_code + "' ,'" + dtInward.Rows[0]["IWM_CODE"].ToString() + "' ,'" + dtInward.Rows[0]["IWM_NO"].ToString() + "' ,'" + Type + "' ,'" + Convert.ToDateTime(dtInward.Rows[0]["IWM_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' ,'" + dtInward.Rows[0]["IWD_REV_QTY"].ToString() + "','-2147483648')");

                        CommonClasses.Execute("Update ITEM_MASTER set I_CURRENT_BAL=I_CURRENT_BAL+" + dtInward.Rows[0]["IWD_REV_QTY"].ToString() + " where  I_CODE='" + item_code + "'");

                        CommonClasses.WriteLog("Material Inspection", "Delete", "Material Inspection", item_code, Convert.ToInt32(cpom_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));

                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Deleted Successfully";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                        ViewRec(type);
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
            CommonClasses.SendError("Material Inspection-ADD", "dgInspection_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInspection_RowDeleting
    protected void dgInspection_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region dgInspection_PageIndexChanging
    protected void dgInspection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgInspection.PageIndex = e.NewPageIndex;
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
            #region Validation
            if (ddlStoreType.SelectedValue == "0")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Select Store Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            double RecQty = double.Parse(txtOkQty.Text) + double.Parse(txtRejQty.Text) + double.Parse(txtScrapQty.Text);
            if (double.Parse(txtrecQty.Text) != RecQty)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Check Rejected & Ok Quantity";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            if ((double.Parse(txtRejQty.Text) > 0.000 && txtReason.Text == "") || (double.Parse(txtScrapQty.Text) > 0.000 && txtReason.Text == ""))
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter The Reason";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtReason.Focus();
                return;
            }

            DataTable dtitem = new DataTable();
            dtitem = CommonClasses.Execute("SELECT * FROM ITEM_MASTER where I_CODENO='" + txtItemCode.Text.Trim() + "'   AND ES_DELETE=0");
            if (dtitem.Rows.Count > 0)
            {
                if (ddlStoreType.SelectedValue == "-2147483647" && dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483648")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Casting Can not Move to Main Store Please change store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStoreType.Focus();
                    return;
                }
                else if (ddlStoreType.SelectedValue == "-2147483634" && dtitem.Rows[0]["I_CAT_CODE"].ToString() == "-2147483648")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Casting Can not Move to Main Store Please change store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStoreType.Focus();
                    return;
                }
                else if (ddlStoreType.SelectedValue == "-2147483648" && dtitem.Rows[0]["I_CAT_CODE"].ToString() != "-2147483648")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Consumable Can not Move to Inward Store Please change store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStoreType.Focus();
                    return;
                }
                else if (ddlStoreType.SelectedValue == "-2147483635" && dtitem.Rows[0]["I_CAT_CODE"].ToString() != "-2147483648")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Consumable Can not Move to Inward Store Please change store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlStoreType.Focus();
                    return;
                }
            }
            if (chkPDR.Checked == true && txtPDRNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Enter PDR N0.";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtPDRNo.Focus();
                return;
            }
            #endregion Validation

            else
            {
                if (type == "ADD")
                {
                    txtInspNo.Text = Numbering().ToString();
                    if (txtInspNo.Text != "")
                    {
                        SaveRec();
                    }
                }
                else if (type == "MOD")
                {
                    SaveRec();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Material Inspection", "btnSave_Click", Ex.Message);
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
                Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
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
        Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
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
            catch { }

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
                    lblmsg.Text = "Sum of (Ok Qty,Reject Qty,Scrap Qty) Should not be Greater than Receive Qty";
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
            //SaveRec();
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
        //CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (mlCode != 0 && mlCode != null)
            {
                CommonClasses.RemoveModifyLock("INSPECTION_S_MASTER", "MODIFY", "INSM_CODE", mlCode);

            }
            else
            {
                CommonClasses.RemoveModifyLock("INWARD_MASTER", "MODIFY", "IWM_CODE", Convert.ToInt32(ViewState["InwardNo"].ToString()));

            }

            Response.Redirect("~/Transactions/View/ViewMaterialInspection.aspx", false);
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

    protected void ddlStoreType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #region FillCombo
    private void FillCombo()
    {
        try
        {
            //DataTable dtUser = CommonClasses.Execute("SELECT * FROM USER_STORE_DETAIL WHERE UM_CODE ='" + Session["UserCode"] + "'  AND STORE_CODE IN (-2147483648,-2147483647)");
            //if (dtUser.Rows.Count == 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "User have no rights of store...";
            //    ddlStoreType.Items.Insert(0, new ListItem("Select To Store Name", "0"));
            //    return;
            //}
            //string Codes = "";
            //for (int i = 0; i < dtUser.Rows.Count; i++)
            //{
            //    Codes = Codes + "'" + dtUser.Rows[i]["STORE_CODE"].ToString() + "'" + ",";
            //}
            //Codes = Codes.TrimEnd(',');
            if (rbtWithAmt.SelectedValue=="-2147483648")
            {
                CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (-2147483648,-2147483647) ORDER BY STORE_NAME", ddlStoreType);               
            }
            else
            {
                CommonClasses.FillCombo("STORE_MASTER", "STORE_NAME", "STORE_CODE", "ES_DELETE=0 AND STORE_COMP_ID=" + (string)Session["CompanyId"] + "  AND STORE_CODE IN (-2147483635,-2147483634) ORDER BY STORE_NAME", ddlStoreType);
            }
            ddlStoreType.Items.Insert(0, new ListItem("Select To Store Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Materail Acceptance-View", "FillCombo", Ex.Message);
        }
    }
    #endregion


    #region rbtWithAmt_SelectedIndexChanged
    protected void rbtWithAmt_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCombo();
    }
    #endregion rbtWithAmt_SelectedIndexChanged
}
