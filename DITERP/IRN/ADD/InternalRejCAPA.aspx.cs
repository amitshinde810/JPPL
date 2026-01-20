using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;

public partial class IRN_ADD_InternalRejCAPA : System.Web.UI.Page
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
    DirectoryInfo ObjSearchDir;
    string fileName = "";
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
                    ViewState["fileName"] = fileName;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    str = "";
                    ViewState["str"] = str;
                    txtGRNDate.Text = System.DateTime.Now.ToString("MMM yyyy");
                    txtGRNDate.Attributes.Add("readonly", "readonly");
                    LoadItem();
                    LoadGrid();
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

                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Iternal Rejection CAPA", "PageLoad", ex.Message);
                }
            }
            if (IsPostBack && FileUpload2.PostedFile != null)
            {
                if (FileUpload2.PostedFile.FileName.Length > 0)
                {
                    fileName = FileUpload2.PostedFile.FileName;
                    ViewState["fileName"] = fileName;
                    Upload(null, null);
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
            if (dgIRN.Rows.Count == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Please Fill Table";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                return;
            }
            for (int i = 0; i < dgIRN.Rows.Count; i++)
            {
                if (((TextBox)dgIRN.Rows[i].FindControl("txtIRCD_ACTION")).Text.Trim() == "" || ((TextBox)dgIRN.Rows[i].FindControl("txtIRCD_ROOT_CAUSE")).Text.Trim() == "")
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Data in Action and route Cause";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                    return;
                }
            }
           
            SaveRec();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "btnSubmit_Click", Ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "btnCancel_Click", ex.Message.ToString());
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
            CommonClasses.SendError("Iternal Rejection CAPA", "CheckValid", Ex.Message);
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
                CommonClasses.RemoveModifyLock("INTERNAL_REJECTION_CAPA_MASTER", "MODIFY", "IRCM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            Response.Redirect("~/IRN/VIEW/ViewInternalRejCAPA.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region LoadItem
    public void LoadItem()
    {
        DataTable DtItem = new DataTable();

        //Bind item from irn detail table and Only finished Items are binded here

        DateTime date = Convert.ToDateTime(txtGRNDate.Text);
        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        firstDayOfMonth = Convert.ToDateTime(date.AddMonths(-1).ToString("01/MMM/yyyy"));
        lastDayOfMonth = Convert.ToDateTime(date.AddDays(-1).ToString("dd/MMM/yyyy"));



        //DtItem = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE   AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT DISTINCT   I_CODE,I_NAME,I_CODENO,ROUND(IRND_AMT,2) AS IRND_AMT, '' AS IRCD_ACTION, '' AS IRCD_ROOT_CAUSE FROM #temp order by IRND_AMT desc DROP TABLE #temp");
        DtItem = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE   AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT DISTINCT  I_CODE,	I_NAME	,I_CODENO,		MAX(IRND_PROD_QTYONDTE) AS IRND_PROD_QTYONDTE   ,'' AS IRCD_ACTION, '' AS IRCD_ROOT_CAUSE ,MAX(IRND_PROD_QTYCUMM) AS 	IRND_PROD_QTYCUMM,	  SUM(IRND_REJ_QTYONDTE) AS  IRND_REJ_QTYONDTE,SUM(IRND_REJ)	IRND_REJ, 	SUM(IRND_AMTONDATE) AS IRND_AMTONDATE,	round(SUM(IRND_AMT),2) AS IRND_AMT,    CASE when MAX(IRND_PROD_QTYONDTE)=0 then 0 else  ROUND((SUM(IRND_REJ_QTYONDTE)/ MAX(IRND_PROD_QTYONDTE)*100),2) END AS 	ONDATEREJ,      CASE when MAX(IRND_PROD_QTYCUMM)=0 then 0 else  ROUND((SUM(IRND_REJ)/ MAX(IRND_PROD_QTYCUMM)*100),2) END AS 	CUMMDATEREJ  FROM #temp    group by I_CODE,	I_NAME	,I_CODENO    order by round(SUM(IRND_AMT),2)  desc       drop table #temp");


        // DtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM ITEM_MASTER,IRN_ENTRY,IRN_DETAIL WHERE I_CAT_CODE=-2147483648 AND IRN_ENTRY.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND IRND_I_CODE=I_CODE AND I_ACTIVE_IND=1 AND I_CM_COMP_ID=1 AND IRN_CODE=IRND_IRN_CODE ORDER BY I_CODENO");
        if (DtItem.Rows.Count > 0)
        {
            ddlFinishedComponent.DataSource = DtItem;
            ddlFinishedComponent.DataTextField = "I_CODENO";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("----Select Item Code----", "0"));

            ddlFinishedComponentName.DataSource = DtItem;
            ddlFinishedComponentName.DataTextField = "I_NAME";
            ddlFinishedComponentName.DataValueField = "I_CODE";
            ddlFinishedComponentName.DataBind();
            ddlFinishedComponentName.Items.Insert(0, new ListItem("----Select Item Name----", "0"));
        }
    }
    #endregion

    #region LoadItemAmt
    public void LoadItemAmt(string code)
    {
        DataTable DtItem = new DataTable();

        //Bind item from irn detail table and Only finished Items are binded here

        DateTime date = Convert.ToDateTime(txtGRNDate.Text);
        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


        firstDayOfMonth = Convert.ToDateTime(date.AddMonths(-1).ToString("01/MMM/yyyy"));
        lastDayOfMonth = Convert.ToDateTime(date.AddDays(-1).ToString("dd/MMM/yyyy"));

       // DtItem = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE   AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT DISTINCT   I_CODE,I_NAME,I_CODENO,ROUND(IRND_AMT,2) AS IRND_AMT, '' AS IRCD_ACTION, '' AS IRCD_ROOT_CAUSE FROM #temp  where I_CODE='" + code + "' order by IRND_AMT desc DROP TABLE #temp");
        DtItem = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE   AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT DISTINCT  I_CODE,	I_NAME	,I_CODENO,		MAX(IRND_PROD_QTYONDTE) AS IRND_PROD_QTYONDTE   ,'' AS IRCD_ACTION, '' AS IRCD_ROOT_CAUSE ,MAX(IRND_PROD_QTYCUMM) AS 	IRND_PROD_QTYCUMM,	  SUM(IRND_REJ_QTYONDTE) AS  IRND_REJ_QTYONDTE,SUM(IRND_REJ)	IRND_REJ, 	SUM(IRND_AMTONDATE) AS IRND_AMTONDATE,	round(SUM(IRND_AMT),2) AS IRND_AMT,    CASE when MAX(IRND_PROD_QTYONDTE)=0 then 0 else  ROUND((SUM(IRND_REJ_QTYONDTE)/ MAX(IRND_PROD_QTYONDTE)*100),2) END AS 	ONDATEREJ,      CASE when MAX(IRND_PROD_QTYCUMM)=0 then 0 else  ROUND((SUM(IRND_REJ)/ MAX(IRND_PROD_QTYCUMM)*100),2) END AS 	CUMMDATEREJ  FROM #temp  where  I_CODE='" + code + "'    group by I_CODE,	I_NAME	,I_CODENO    order by round(SUM(IRND_AMT),2)  desc       drop table #temp");

        if (DtItem.Rows.Count > 0)
        {
            txtRejAmt.Text = DtItem.Rows[0]["IRND_AMT"].ToString();
        }
        else
        {
            txtRejAmt.Text = "0";
        }
    }
    #endregion

    #region ddlFinishedComponent_SelectedIndexChanged
    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponentName.SelectedValue = ddlFinishedComponent.SelectedValue;
        LoadItemAmt(ddlFinishedComponentName.SelectedValue.ToString());
    }
    #endregion

    #region ddlFinishedComponentName_SelectedIndexChanged
    protected void ddlFinishedComponentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFinishedComponent.SelectedValue = ddlFinishedComponentName.SelectedValue;
        LoadItemAmt(ddlFinishedComponent.SelectedValue.ToString());
    }
    #endregion

    #region txtGRNDate_TextChanged
    protected void txtGRNDate_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("Select IRCM_CODE,IRCM_DATE,IRCM_STATUS from INTERNAL_REJECTION_CAPA_MASTER where ES_DELETE=0 AND IRCM_DATE='" + Convert.ToDateTime(txtGRNDate.Text).ToString("MMM yyyy") + "'");
        if (dt.Rows.Count > 0)
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Already Exist For This Month";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            BlankGrid();
            return;
        }
        LoadGrid();
    }
    #endregion
    #endregion

    #region Upload
    protected void Upload(object sender, EventArgs e)
    {
        try
        {
            string sDirPath = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/IternalRej/");
            }
            else
            {
                sDirPath = Server.MapPath(@"~/UpLoadPath/IternalRej/" + ViewState["mlCode"].ToString() + "/");
            }

            ObjSearchDir = new DirectoryInfo(sDirPath);

            if (!ObjSearchDir.Exists)
            {
                ObjSearchDir.Create();
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/IternalRej/" + ViewState["fileName"].ToString()));
            }
            else
            {
                FileUpload2.SaveAs(Server.MapPath("~/UpLoadPath/IternalRej/" + ViewState["mlCode"].ToString() + "/" + ViewState["fileName"].ToString()));
            }
            lnkupload.Visible = true;
            lnkupload.Text = ViewState["fileName"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region lnkupload_Click
    protected void lnkupload_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = "";
            string directory = "";
            if (Request.QueryString[0].Equals("INSERT"))
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/IternalRej/" + filePath;
            }
            else
            {
                filePath = lnkupload.Text;
                directory = "../../UpLoadPath/IternalRej/" + ViewState["mlCode"].ToString() + "/" + filePath;
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

    #region BlankGrid
    private void BlankGrid()
    {
        DataTable dtFilter = new DataTable();
        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRND_AMT", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRCD_ACTION", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IRCD_ROOT_CAUSE", typeof(string)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgIRN.DataSource = dtFilter;
            dgIRN.DataBind();
            dgIRN.Enabled = false;
        }
    }
    #endregion

    #region LoadGrid
    private void LoadGrid()
    {
        DateTime date = Convert.ToDateTime(txtGRNDate.Text);
        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddDays(-1);

        firstDayOfMonth =Convert.ToDateTime( date.AddMonths(-1).ToString("01/MMM/yyyy"));
        lastDayOfMonth = Convert.ToDateTime(date.AddDays(-1).ToString("dd/MMM/yyyy"));

        DataTable dt = new DataTable();
        // dt = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210 ) AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210) AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_P_CODE IN (-2147483210) AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210) AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_P_CODE  IN (-2147483210 ) AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT DISTINCT TOP 10 * FROM #temp order by IRND_AMT desc DROP TABLE #temp");

       // dt = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE   AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT DISTINCT TOP 10 I_CODE,I_NAME,I_CODENO,ROUND(IRND_AMT,2) AS IRND_AMT, '' AS IRCD_ACTION, '' AS IRCD_ROOT_CAUSE FROM #temp order by IRND_AMT desc DROP TABLE #temp");

        dt = CommonClasses.Execute("SELECT DISTINCT * INTO #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE   AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a  SELECT DISTINCT top 10 I_CODE,	I_NAME	,I_CODENO,		MAX(IRND_PROD_QTYONDTE) AS IRND_PROD_QTYONDTE   ,'' AS IRCD_ACTION, '' AS IRCD_ROOT_CAUSE ,MAX(IRND_PROD_QTYCUMM) AS 	IRND_PROD_QTYCUMM,	  SUM(IRND_REJ_QTYONDTE) AS  IRND_REJ_QTYONDTE,SUM(IRND_REJ)	IRND_REJ, 	SUM(IRND_AMTONDATE) AS IRND_AMTONDATE,	round(SUM(IRND_AMT),2) AS IRND_AMT,    CASE when MAX(IRND_PROD_QTYONDTE)=0 then 0 else  ROUND((SUM(IRND_REJ_QTYONDTE)/ MAX(IRND_PROD_QTYONDTE)*100),2) END AS 	ONDATEREJ,      CASE when MAX(IRND_PROD_QTYCUMM)=0 then 0 else  ROUND((SUM(IRND_REJ)/ MAX(IRND_PROD_QTYCUMM)*100),2) END AS 	CUMMDATEREJ  FROM #temp    group by I_CODE,	I_NAME	,I_CODENO    order by round(SUM(IRND_AMT),2)  desc       drop table #temp");
        if (dt.Rows.Count > 0)
        {
            dgIRN.DataSource = dt;
            dgIRN.DataBind();
            dgIRN.Enabled = true;
            ViewState["dt2"] = dt;
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute("SELECT IRCM_CODE,IRCM_DATE,IRCM_FILE FROM INTERNAL_REJECTION_CAPA_MASTER WHERE (ES_DELETE = 0) AND IRCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dt.Rows.Count > 0)
            {
                txtGRNDate.Text = Convert.ToDateTime(dt.Rows[0]["IRCM_DATE"].ToString()).ToString("MMM yyyy");
                lnkupload.Text = dt.Rows[0]["IRCM_FILE"].ToString();
            }

            DateTime date = Convert.ToDateTime(txtGRNDate.Text);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DataTable dtdetails = new DataTable();
            //dtdetails = CommonClasses.Execute("SELECT * into #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,'' as IRCD_ACTION,'' as IRCD_ROOT_CAUSE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210 ) AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210) AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_P_CODE IN (-2147483210) AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0))=0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210) AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "'),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE AND ID.IRND_I_CODE IN (SELECT DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "') AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(firstDayOfMonth).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(lastDayOfMonth).ToString("dd/MMM/yyyy") + "' AND IRND_P_CODE  IN (-2147483210 ) AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_TYPE)AS a SELECT TOP 10 * FROM #temp order by IRND_AMT desc DROP TABLE #temp");//AND IRCM_CODE=
            dtdetails = CommonClasses.Execute("select I_CODE,I_CODENO,I_NAME,IRCD_ACTION,IRCD_ROOT_CAUSE,IRCD_REJ_AMT as IRND_AMT from INTERNAL_REJECTION_CAPA_DETAIL,ITEM_MASTER where ITEM_MASTER.ES_DELETE=0 and I_CODE=IRCD_I_CODE and IRCD_IRCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
            if (dtdetails.Rows.Count > 0)
            {
                dgIRN.Enabled = true;
                dgIRN.DataSource = dtdetails;
                dgIRN.DataBind();
                ViewState["dt2"] = dtdetails;
            }
            if (str == "MOD")
            {
                txtGRNDate.Enabled = false;
                CommonClasses.SetModifyLock("INTERNAL_REJECTION_CAPA_MASTER", "MODIFY", "IRCM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }
            else
            {
                FileUpload2.Enabled = false;
                txtGRNDate.Enabled = false;
                btnSubmit.Visible = false;
                txtRejAmt.Enabled = false;
                ddlFinishedComponentName.Enabled = false;
                ddlFinishedComponent.Enabled = false;
                btnInsert.Enabled = false;
                dgIRN.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "ViewRec", Ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "GetValues", ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "Setvalues", ex.Message);
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
            if (Request.QueryString[0].Equals("INSERT"))
            {
                if (CommonClasses.Execute1("INSERT INTO INTERNAL_REJECTION_CAPA_MASTER (IRCM_DATE,IRCM_CM_COMP_CODE,IRCM_FILE)VALUES ('" + txtGRNDate.Text + "','" + Convert.ToInt32(Session["CompanyCode"]) + "','" + lnkupload.Text + "') "))
                {
                    string Code = CommonClasses.GetMaxId("Select Max(IRCM_CODE) from INTERNAL_REJECTION_CAPA_MASTER");
                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        //change by suja
                        CommonClasses.Execute1("INSERT INTO INTERNAL_REJECTION_CAPA_DETAIL (IRCD_IRCM_CODE,IRCD_I_CODE,IRCD_ACTION,IRCD_ROOT_CAUSE,IRCD_REJ_AMT) VALUES ('" + Code + "','" + ((Label)dgIRN.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgIRN.Rows[i].FindControl("txtIRCD_ACTION")).Text + "','" + ((TextBox)dgIRN.Rows[i].FindControl("txtIRCD_ROOT_CAUSE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIIRND_AMT")).Text + "')");
                    }

                    #region file upload
                    if (ViewState["fileName"].ToString().Trim() != "")
                    {
                        string sDirPath = Server.MapPath(@"~/UpLoadPath/IternalRej/" + Code + "");

                        ObjSearchDir = new DirectoryInfo(sDirPath);

                        string sDirPath1 = Server.MapPath(@"~/UpLoadPath/IternalRej/");
                        DirectoryInfo dir = new DirectoryInfo(sDirPath1);

                        dir.Refresh();

                        if (!ObjSearchDir.Exists)
                        {
                            ObjSearchDir.Create();
                        }
                        string currentApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

                        //Get the full path of the file    
                        string fullFilePath1 = currentApplicationPath + "UpLoadPath\\IternalRej ";
                        string fullFilePath = Server.MapPath(@"~/UpLoadPath/IternalRej/" + lnkupload.Text);
                        // Get the destination path
                        string copyToPath = Server.MapPath(@"~/UpLoadPath//IternalRej/" + Code + "/" + lnkupload.Text);
                        DirectoryInfo di = new DirectoryInfo(fullFilePath1);
                        System.IO.File.Move(fullFilePath, copyToPath);
                    }
                    #endregion

                    CommonClasses.WriteLog("Iternal Rejection CAPA", "Save", "Iternal Rejection CAPA", txtGRNDate.Text, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewInternalRejCAPA.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record not save", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE INTERNAL_REJECTION_CAPA_MASTER SET IRCM_DATE='" + txtGRNDate.Text.Trim() + "',IRCM_FILE='" + lnkupload.Text + "' where IRCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'"))
                {
                    DataTable dtDetail = CommonClasses.Execute("SELECT * FROM INTERNAL_REJECTION_CAPA_DETAIL where IRCD_IRCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                    if (dtDetail.Rows.Count > 0)
                    {

                    }
                    CommonClasses.Execute1("DELETE FROM INTERNAL_REJECTION_CAPA_DETAIL where IRCD_IRCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");

                    for (int i = 0; i < dgIRN.Rows.Count; i++)
                    {
                        //change by suja
                        CommonClasses.Execute1("INSERT INTO INTERNAL_REJECTION_CAPA_DETAIL (IRCD_IRCM_CODE,IRCD_I_CODE,IRCD_ACTION,IRCD_ROOT_CAUSE,IRCD_REJ_AMT) VALUES ('" + Convert.ToInt32(ViewState["mlCode"]) + "','" + ((Label)dgIRN.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((TextBox)dgIRN.Rows[i].FindControl("txtIRCD_ACTION")).Text + "','" + ((TextBox)dgIRN.Rows[i].FindControl("txtIRCD_ROOT_CAUSE")).Text + "','" + ((Label)dgIRN.Rows[i].FindControl("lblIIRND_AMT")).Text + "')");
                    }

                    CommonClasses.RemoveModifyLock("INTERNAL_REJECTION_CAPA_MASTER", "MODIFY", "IRCM_CODE", Convert.ToInt32(ViewState["mlCode"]));
                    CommonClasses.WriteLog("Iternal Rejection CAPA", "Update", "Iternal Rejection CAPA", txtGRNDate.Text, Convert.ToInt32(ViewState["mlCode"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/IRN/VIEW/ViewInternalRejCAPA.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Record not save", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "SaveRec", ex.Message);
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
            CommonClasses.SendError("Iternal Rejection CAPA", "ShowMessage", Ex.Message);
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
                string IRCD_I_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblI_CODE"))).Text;
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
            ((DataTable)ViewState["dt2"]).Columns.Add("IRCD_I_CODE");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
            ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRCD_ACTION");
            ((DataTable)ViewState["dt2"]).Columns.Add("IRCD_ROOT_CAUSE");
        }
        #endregion

        #region check Data table,insert or Modify Data
        if (ViewState["str"].ToString() == "Modify")
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
        dgIRN.Enabled = true;
        dgIRN.Visible = true;
        dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
        dgIRN.DataBind();
        ViewState["str"] = "";
        ViewState["ItemUpdateIndex"] = "-1";
        #endregion
    }
    #endregion LoadInsertData

    #region dgIRN_Deleting
    protected void dgIRN_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region dgShortProReason_Deleting
    protected void dgShortProReason_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlFinishedComponent.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlFinishedComponent.Focus();
                return;
            }
            if (ddlFinishedComponentName.SelectedIndex == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                ddlFinishedComponentName.Focus();
                return;
            }
            if (txtRejAmt.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Amount";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                txtRejAmt.Focus();
                return;
            }
            if (dgIRN.Rows.Count > 0)
            {
                for (int i = 0; i < dgIRN.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgIRN.Rows[i].FindControl("lblI_CODE"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlFinishedComponent.SelectedValue.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlFinishedComponent.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                }
            }
            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("IRND_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IRCD_ACTION");
                ((DataTable)ViewState["dt2"]).Columns.Add("IRCD_ROOT_CAUSE");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["I_CODE"] = ddlFinishedComponent.SelectedValue;
            dr["I_CODENO"] = ddlFinishedComponent.SelectedItem;
            dr["I_NAME"] = ddlFinishedComponentName.SelectedItem;
            dr["IRND_AMT"] = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(txtRejAmt.Text)), 3));
            if (ViewState["str"].ToString() == "Modify" && ViewState["ItemUpdateIndex"].ToString() != "-1")
            {

            }
            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
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
            dgIRN.Visible = true;
            dgIRN.DataSource = ((DataTable)ViewState["dt2"]);
            dgIRN.DataBind();
            dgIRN.Enabled = true;
            #endregion
            ViewState["ItemUpdateIndex"] = "-1";
            ddlFinishedComponent.SelectedValue = "0";
            ddlFinishedComponentName.SelectedValue = "0";
            txtRejAmt.Text = "0";
            lblmsg.Text = "";
            PanelMsg.Visible = false;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

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
                // if (dgIRN.Rows.Count == 0)
                // BlankGrid();

                string I_CODE = ((Label)(row.FindControl("lblI_CODE"))).Text;

                CommonClasses.Execute1("DELETE FROM INTERNAL_REJECTION_CAPA_DETAIL WHERE IRCD_I_CODE='" + I_CODE + "' AND IRCD_IRCM_CODE='" + Convert.ToInt32(ViewState["mlCode"]) + "'");
                // BlankGridReason();
            }
            if (e.CommandName == "Select")
            {
                foreach (GridViewRow gvr in dgIRN.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }

                ddlFinishedComponent.SelectedValue = ((Label)(row.FindControl("lblI_CODE"))).Text;
                ddlFinishedComponentName.SelectedValue = ((Label)(row.FindControl("lblI_CODE"))).Text;
                txtRejAmt.Text = ((Label)(row.FindControl("lblIIRND_AMT"))).Text;

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Iternal Rejection CAPA", "dgIRN_RowCommand", Ex.Message);
        }
    }
    #endregion
}

