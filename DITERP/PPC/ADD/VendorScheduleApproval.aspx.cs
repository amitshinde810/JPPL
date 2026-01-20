using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PPC_VIEW_VendorScheduleApproval : System.Web.UI.Page
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
            HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
            home.Attributes["class"] = "active";
            HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
            home1.Attributes["class"] = "active";

            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {

                    ViewState["type"] = type;
                    ViewState["type"] = Request.QueryString[0].ToString();
                    txtInspDate.Attributes.Add("readonly", "readonly");
                    txtInspDate.Text = System.DateTime.Now.ToString("MMM/yyyy");
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        ViewState["type"] = Request.QueryString[1].ToString();
                        txtInspDate.Text = Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy");
                        txtInspDate.Enabled = false;
                        dgMaterialAcceptance.Enabled = false;
                        getScheduleDetail();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        ViewState["type"] = Request.QueryString[1].ToString();
                        txtInspDate.Text = Convert.ToDateTime(ViewState["type"]).ToString("MMM/yyyy");
                        txtInspDate.Enabled = true;
                        getScheduleDetail();
                        CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL  SET MODIFY=1 where VSA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        getSchedule();
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

    #region getScheduleDetail
    public void getScheduleDetail()
    {
        DateTime dtMonth = new DateTime();
        dtMonth = Convert.ToDateTime(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy"));

        DataTable dtAll = new DataTable();
        //dtAll.Columns.Add("P_CODE");
        //dtAll.Columns.Add("P_NAME");
        dtAll.Columns.Add("I_CODE", typeof(string));
        dtAll.Columns.Add("I_CODENO", typeof(string));//change Binding Value Name From ItemCode1 to ItemCodeNo
        dtAll.Columns.Add("I_NAME", typeof(string));
        dtAll.Columns.Add("QTY", typeof(string));
        dtAll.Columns.Add("VSA_QTY_PPC", typeof(string));//change Binding Value Name From StockUOM1 to UOM_CODE
        dtAll.Columns.Add("I_INV_RATE", typeof(string));
        dtAll.Columns.Add("AMT", typeof(string));
        dtAll.Columns.Add("VS_STATUS", typeof(string));
        dtAll.Columns.Add("VS_TYPE", typeof(string));
        dtAll.Columns.Add("VSA_TYPE", typeof(string));

        DataTable dtraw = new DataTable();


        //dtraw = CommonClasses.Execute(" SELECT VSA_QTY_PPC AS  VSA_QTY_PPC,ISNULL((SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where ES_DELETE=0 AND VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VS_I_CODE=I_CODE ),0)   AS QTY ,VSA_RATE AS I_INV_RATE,VSA_AMT AS AMT,I_CODE,I_CODENO,I_NAME ,isnull(( SELECT  top 1  VS_STATUS FROM VENDOR_SCHEDULE   where VS_I_CODE=I_CODE AND  VS_STATUS=1),0) As VS_STATUS ,0 AS VS_TYPE,1 AS VSA_TYPE  FROM VENDORSCHEDULE_APPROVAL ,ITEM_MASTER where VSA_I_CODE=I_CODE AND VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "'");


        dtraw = CommonClasses.Execute(" SELECT SUM(VSA_QTY_PPC) AS VSA_QTY_PPC,ISNULL((SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where ES_DELETE=0 AND VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VS_I_CODE=I_CODE ),0)   AS QTY ,VSA_RATE AS I_INV_RATE,SUM(VSA_AMT) AS AMT,I_CODE,I_CODENO,I_NAME ,isnull(( SELECT  top 1  VS_STATUS FROM VENDOR_SCHEDULE   where VS_I_CODE=I_CODE AND  VS_STATUS=1),0) As VS_STATUS ,0 AS VS_TYPE,1 AS VSA_TYPE  FROM VENDORSCHEDULE_APPROVAL ,ITEM_MASTER where VSA_I_CODE=I_CODE AND VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "' GROUP BY  I_CODE,I_CODENO,I_NAME,VSA_RATE");
        DataTable dtraw1 = new DataTable();




        //dtraw1 = CommonClasses.Execute("   SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO,I_INV_RATE , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + dtMonth.Month + "'   and DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' ),0) AS CS_SCHEDULE_QTY  INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'      and VS_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' SELECT I_CODE,I_INV_RATE,I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	  ),0)  AS QTY   INTO #temp1 FROM #temp    SELECT QTY AS VSA_QTY_PPC,QTY ,I_INV_RATE,round(QTY*I_INV_RATE,2) AS AMT,I_CODE,I_CODENO,I_NAME,1 AS VS_STATUS  FROM   #temp1 where I_CODE NOT IN (   SELECT VSA_I_CODE    FROM VENDORSCHEDULE_APPROVAL   where   VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("01/MMM/yyyy") + "')      DROP TABLE #temp1 DROP TABLE #temp");


        //   dtraw1 = CommonClasses.Execute("   SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO,I_INV_RATE , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + dtMonth.Month + "'   and DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' ),0) AS CS_SCHEDULE_QTY  INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'      and VS_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' SELECT I_CODE,I_INV_RATE,I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	  ),0)  AS QTY   INTO #temp1 FROM #temp    SELECT QTY AS VSA_QTY_PPC,QTY ,I_INV_RATE,round(QTY*I_INV_RATE,2) AS AMT,I_CODE,I_CODENO,I_NAME,1 AS VS_STATUS ,0 AS VS_TYPE,1 AS VSA_TYPE FROM   #temp1 where I_CODE NOT IN (   SELECT VSA_I_CODE    FROM VENDORSCHEDULE_APPROVAL   where   VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("01/MMM/yyyy") + "') UNION  SELECT  SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4))AS VSA_QTY_PPC,SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4))AS QTY  ,I_INV_RATE,(SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4)) * I_INV_RATE ) AS AMT , I_CODE,I_CODENO,I_NAME,  ISNULL((SELECT UVS_STATUS FROM UNPLAN_VENDOR_SCHEDULE where  UVS_I_CODE=I_CODE    AND UVS_STATUS=1 AND DATEPART(MM,UVS_DATE)='" + dtMonth.Month + "' AND  DATEPART(yyyy,UVS_DATE)='" + dtMonth.Year + "'  ),0)  AS  VS_STATUS ,1 AS VS_TYPE,1 AS VSA_TYPE  FROM UNPLAN_VENDOR_SCHEDULE,ITEM_MASTER where UNPLAN_VENDOR_SCHEDULE.ES_DELETE=0  AND UVS_I_CODE=I_CODE   AND DATEPART(MM,UVS_DATE)='" + dtMonth.Month + "'  AND  DATEPART(yyyy,UVS_DATE)='" + dtMonth.Year + "'  AND  AND    UVS_I_CODE NOT IN   (   SELECT VSA_I_CODE    FROM VENDORSCHEDULE_APPROVAL   where   VENDORSCHEDULE_APPROVAL.ES_DELETE=0  AND DATEPART(MM,VSA_MONTH)='" + dtMonth.Month + "'  AND  DATEPART(yyyy,VSA_MONTH)='" + dtMonth.Month + "'     )  GROUP BY  I_CODE,I_CODENO,I_NAME,I_INV_RATE    DROP TABLE #temp1 DROP TABLE #temp");

        //  dtraw1 = CommonClasses.Execute("   SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO,I_INV_RATE , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + dtMonth.Month + "'   and DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' ),0) AS CS_SCHEDULE_QTY  INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'      and VS_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' SELECT I_CODE,I_INV_RATE,I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	  ),0)  AS QTY   INTO #temp1 FROM #temp    SELECT QTY AS VSA_QTY_PPC,QTY ,I_INV_RATE,round(QTY*I_INV_RATE,2) AS AMT,I_CODE,I_CODENO,I_NAME,1 AS VS_STATUS ,0 AS VS_TYPE,1 AS VSA_TYPE FROM   #temp1 where I_CODE NOT IN (   SELECT VSA_I_CODE    FROM VENDORSCHEDULE_APPROVAL   where   VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("01/MMM/yyyy") + "') UNION  SELECT  SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4))AS VSA_QTY_PPC,SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4))AS QTY  ,I_INV_RATE,(SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4)) * I_INV_RATE ) AS AMT , I_CODE,I_CODENO,I_NAME,  ISNULL((SELECT UVS_STATUS FROM UNPLAN_VENDOR_SCHEDULE where  UVS_I_CODE=I_CODE    AND UVS_STATUS=1 AND DATEPART(MM,UVS_DATE)='" + dtMonth.Month + "' AND  DATEPART(yyyy,UVS_DATE)='" + dtMonth.Year + "'  ),0)  AS  VS_STATUS ,1 AS VS_TYPE,1 AS VSA_TYPE  FROM UNPLAN_VENDOR_SCHEDULE,ITEM_MASTER where UNPLAN_VENDOR_SCHEDULE.ES_DELETE=0  AND UVS_I_CODE=I_CODE   AND DATEPART(MM,UVS_DATE)='" + dtMonth.Month + "'  AND  DATEPART(yyyy,UVS_DATE)='" + dtMonth.Year + "'  AND UVS_STATUS=1  GROUP BY  I_CODE,I_CODENO,I_NAME,I_INV_RATE    DROP TABLE #temp1 DROP TABLE #temp");


        dtraw1 = CommonClasses.Execute("   SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO,I_INV_RATE , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + dtMonth.Month + "'   and DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' ),0) AS CS_SCHEDULE_QTY  INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'      and VS_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' SELECT I_CODE,I_INV_RATE,I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	  ),0)  AS QTY   INTO #temp1 FROM #temp    SELECT QTY AS VSA_QTY_PPC,QTY ,I_INV_RATE,round(QTY*I_INV_RATE,2) AS AMT,I_CODE,I_CODENO,I_NAME,1 AS VS_STATUS ,0 AS VS_TYPE,1 AS VSA_TYPE FROM   #temp1 where I_CODE NOT IN (   SELECT VSA_I_CODE    FROM VENDORSCHEDULE_APPROVAL   where   VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("01/MMM/yyyy") + "')   DROP TABLE #temp1 DROP TABLE #temp");







        //new LOGIC


        //dtraw = CommonClasses.Execute(" SELECT  P_CODE,P_NAME,SUM(VSA_QTY_PPC) AS VSA_QTY_PPC,ISNULL((SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where ES_DELETE=0 AND VS_DATE='" + dtMonth.ToString("dd/MMM/yyyy") + "' AND VS_I_CODE=I_CODE ),0)   AS QTY ,VSA_RATE AS I_INV_RATE,SUM(VSA_AMT) AS AMT,I_CODE,I_CODENO,I_NAME ,isnull(( SELECT  top 1  VS_STATUS FROM VENDOR_SCHEDULE   where VS_I_CODE=I_CODE AND VS_P_CODE=P_CODE AND  VS_STATUS=1),0) As VS_STATUS ,0 AS VS_TYPE,1 AS VSA_TYPE  FROM VENDORSCHEDULE_APPROVAL ,ITEM_MASTER ,PARTY_MASTER where VSA_I_CODE=I_CODE AND VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("dd/MMM/yyyy") + "'  AND VSA_P_CODE=P_CODE GROUP BY  I_CODE,I_CODENO,I_NAME,VSA_RATE, P_CODE,P_NAME");
        //DataTable dtraw1 = new DataTable();

        //dtraw1 = CommonClasses.Execute(" SELECT P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,VS_WEEK1+VS_WEEK2+VS_WEEK3 +VS_WEEK4  AS VSA_QTY_PPC, VS_WEEK1+VS_WEEK2+VS_WEEK3 +VS_WEEK4  AS QTY,ISNULL( ( SELECT TOP 1 (SPOD_RATE)  FROM SUPP_PO_MASTER,SUPP_PO_DETAILS where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOD_I_CODE=I_CODE AND SPOM_P_CODE=P_CODE   ),0) AS I_INV_RATE,   ISNULL(( SELECT SUM( IWD_REV_QTY)  FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE  AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(MM,IWM_DATE)='" + dtMonth.Month + "' AND DATEPART(YYYY,IWM_DATE)='" + dtMonth.Year + "'  AND IWM_P_CODE=P_CODE AND IWD_I_CODE=I_CODE),0) AS INWARD_QTY , 0 AS VS_TYPE ,  VS_STATUS ,0 AS VSA_TYPE  into #temp FROM VENDOR_SCHEDULE,PARTY_MASTER,ITEM_MASTER where VENDOR_SCHEDULE.ES_DELETE=0 AND DATEPART(MM,VS_DATE)='" + dtMonth.Month + "' AND DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' AND VS_I_CODE=I_CODE AND VS_P_CODE=P_CODE AND VS_CM_CODE='" + Session["CompanyCode"] + "'   SELECT *,ROUND((QTY*I_INV_RATE),2) AS AMT FROM #temp   where( I_CODE NOT IN  (   SELECT VSA_I_CODE    FROM VENDORSCHEDULE_APPROVAL   where   VENDORSCHEDULE_APPROVAL.ES_DELETE=0 AND VSA_MONTH='" + dtMonth.ToString("01/MMM/yyyy") + "' AND VSA_P_CODE=P_CODE)    ) DROP TABLE #temp ");

        dtAll = dtraw.Copy();


        if (dtraw1.Rows.Count > 0)
        {
            dtAll.Merge(dtraw1, true, MissingSchemaAction.Ignore);
        }
        if (dtAll.Rows.Count > 0)
        {
            DataView dv = dtAll.DefaultView;
            dv.Sort = "VS_STATUS desc";
            DataTable sortedDT = dv.ToTable();

            dgMaterialAcceptance.DataSource = sortedDT;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;
        }
        else
        {
            DataTable dtfill = new DataTable();

            //dtfill.Columns.Add("P_CODE");
            //dtfill.Columns.Add("P_NAME");
            dtfill.Columns.Add("I_CODE");
            dtfill.Columns.Add("I_CODENO");//change Binding Value Name From ItemCode1 to ItemCodeNo
            dtfill.Columns.Add("I_NAME");
            dtfill.Columns.Add("QTY");
            dtfill.Columns.Add("VSA_QTY_PPC");//change Binding Value Name From StockUOM1 to UOM_CODE
            dtfill.Columns.Add("I_INV_RATE");
            dtfill.Columns.Add("AMT");
            dtfill.Columns.Add("VS_STATUS");
            dtfill.Columns.Add("VS_TYPE", typeof(string));
            dtfill.Columns.Add("VSA_TYPE", typeof(string));

            dtfill.Rows.Add(dtfill.NewRow());

            dgMaterialAcceptance.DataSource = dtfill;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;
        }
    }
    #endregion

    #region txtInspDate_TextChanged
    protected void txtInspDate_TextChanged(object sender, EventArgs e)
    {

        getSchedule();
    }
    #endregion

    #region getSchedule
    public void getSchedule()
    {
        DateTime dtMonth = new DateTime();
        dtMonth = Convert.ToDateTime(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy"));


        //string Query = " SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO,I_INV_RATE , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + dtMonth.Month + "'   and DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' ),0) AS CS_SCHEDULE_QTY  INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'      and VS_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' SELECT I_CODE,I_INV_RATE,I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	  ),0)  AS QTY   INTO #temp1 FROM #temp DROP TABLE #temp   SELECT I_CODE,I_CODENO,I_NAME,QTY,QTY AS VSA_QTY_PPC ,I_INV_RATE,round(QTY*I_INV_RATE,2) AS AMT,0 AS VS_STATUS   FROM   #temp1 DROP TABLE #temp1 ";


        //New Query After adding Unplan Schedule

        string Query = " SELECT DISTINCT I.I_CODE,G.GP_CODE,G.GP_NAME, I_CODENO,I_INV_RATE , I_NAME ,ISNULL(( SELECT SUM(VS_WEEK1+VS_WEEK2+VS_WEEK3+VS_WEEK4) FROM VENDOR_SCHEDULE where VENDOR_SCHEDULE.VS_I_CODE=I.I_CODE and VENDOR_SCHEDULE.ES_DELETE=0 and DATEPART(MM,VS_DATE)='" + dtMonth.Month + "'   and DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' ),0) AS CS_SCHEDULE_QTY  INTO #temp  FROM GROUP_MASTER G INNER JOIN STANDARD_INVENTARY_MASTER S ON G.GP_CODE =S.SIM_GP_CODE INNER JOIN ITEM_MASTER I ON S.SIM_I_CODE=I.I_CODE INNER JOIN VENDOR_SCHEDULE V ON SIM_I_CODE=V.VS_I_CODE inner join PRODUCT_MASTER PROD on S.SIM_I_CODE=PROD.PROD_I_CODE  WHERE   PROD.PROD_MACHINE_LOC in('OFFLOADED','BOTH') AND PROD.ES_DELETE=0 AND S.ES_DELETE=0 AND I.ES_DELETE=0 AND G.ES_DELETE=0  AND V.ES_DELETE=0 AND S.SIM_COMP_ID='" + Session["CompanyId"] + "'      and VS_DATE='" + dtMonth.ToString("01/MMM/yyyy") + "' SELECT I_CODE,I_INV_RATE,I_CODENO,	I_NAME,	ROUND((CS_SCHEDULE_QTY	  ),0)  AS QTY   INTO #temp1 FROM #temp DROP TABLE #temp   SELECT I_CODE,I_CODENO,I_NAME,QTY,QTY AS VSA_QTY_PPC ,I_INV_RATE,round(QTY*I_INV_RATE,2) AS AMT,0 AS VS_STATUS  ,0 AS VS_TYPE,1 AS VSA_TYPE   FROM   #temp1   UNION   SELECT I_CODE,I_CODENO,I_NAME,SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4))AS QTY , SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4))AS VSA_QTY_PPC ,I_INV_RATE,(SUM((UVS_WEEK1+UVS_WEEK2+ UVS_WEEK3+ UVS_WEEK4)) * I_INV_RATE ) AS AMT ,   ISNULL((SELECT UVS_STATUS FROM UNPLAN_VENDOR_SCHEDULE where  UVS_I_CODE=I_CODE   AND DATEPART(MM,UVS_DATE)='" + dtMonth.Month + "' AND  DATEPART(yyyy,UVS_DATE)='" + dtMonth.Year + "'  ),0)  AS  VS_STATUS  ,0 AS VS_TYPE,1 AS VSA_TYPE    FROM UNPLAN_VENDOR_SCHEDULE,ITEM_MASTER where UNPLAN_VENDOR_SCHEDULE.ES_DELETE=0  AND UVS_I_CODE=I_CODE   AND DATEPART(MM,UVS_DATE)='" + dtMonth.Month + "'  AND  DATEPART(yyyy,UVS_DATE)='" + dtMonth.Year + "'     GROUP BY  I_CODE,I_CODENO,I_NAME,I_INV_RATE  DROP TABLE #temp1  ";



        //AS per NEW logic of Direct Approval


        //string Query = "SELECT P_CODE,P_NAME,I_CODE,I_CODENO,I_NAME,VS_WEEK1+VS_WEEK2+VS_WEEK3 +VS_WEEK4  AS VSA_QTY_PPC, VS_WEEK1+VS_WEEK2+VS_WEEK3 +VS_WEEK4  AS QTY,ISNULL( ( SELECT TOP 1 (SPOD_RATE)  FROM SUPP_PO_MASTER,SUPP_PO_DETAILS where SPOM_CODE=SPOD_SPOM_CODE AND SUPP_PO_MASTER.ES_DELETE=0 AND SPOD_I_CODE=I_CODE AND SPOM_P_CODE=P_CODE   ),0) AS I_INV_RATE,   ISNULL(( SELECT SUM( IWD_REV_QTY)  FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE  AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(MM,IWM_DATE)='" + dtMonth.Month + "' AND DATEPART(YYYY,IWM_DATE)='" + dtMonth.Year + "'  AND IWM_P_CODE=P_CODE AND IWD_I_CODE=I_CODE),0) AS INWARD_QTY , 0 AS VS_TYPE ,  VS_STATUS ,0 AS VSA_TYPE  into #temp FROM VENDOR_SCHEDULE,PARTY_MASTER,ITEM_MASTER where VENDOR_SCHEDULE.ES_DELETE=0 AND DATEPART(MM,VS_DATE)='" + dtMonth.Month + "' AND DATEPART(YYYY,VS_DATE)='" + dtMonth.Year + "' AND VS_I_CODE=I_CODE AND VS_P_CODE=P_CODE AND VS_CM_CODE='" + Session["CompanyCode"] + "'   SELECT *,ROUND((QTY*I_INV_RATE),2) AS AMT FROM #temp DROP TABLE #temp";

        DataTable dtraw = new DataTable();
        dtraw = CommonClasses.Execute(Query);

        if (dtraw.Rows.Count > 0)
        {

            dgMaterialAcceptance.DataSource = dtraw;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;

            double sum = 0;
            for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
            {

                sum += Convert.ToDouble(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text);
            }
            txtTotal.Text = Convert.ToDouble(sum).ToString();
        }
        else
        {
            DataTable dtfill = new DataTable();


            //dtfill.Columns.Add("P_CODE");
            //dtfill.Columns.Add("P_NAME");


            dtfill.Columns.Add("I_CODE");
            dtfill.Columns.Add("I_CODENO");//change Binding Value Name From ItemCode1 to ItemCodeNo
            dtfill.Columns.Add("I_NAME");
            dtfill.Columns.Add("QTY");//change Binding Value Name From StockUOM1 to UOM_CODE
            dtfill.Columns.Add("VSA_QTY_PPC");
            dtfill.Columns.Add("I_INV_RATE");
            dtfill.Columns.Add("AMT");
            dtfill.Columns.Add("VS_STATUS");
            dtfill.Columns.Add("VS_TYPE", typeof(string));
            dtfill.Columns.Add("VSA_TYPE", typeof(string));
            dtfill.Rows.Add(dtfill.NewRow());

            dgMaterialAcceptance.DataSource = dtfill;
            dgMaterialAcceptance.DataBind();
            dgMaterialAcceptance.Visible = true;
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
                }
            }
            else if (str == "ADD")
            {
                dt = CommonClasses.Execute("select I_CODENO,IWD_IWM_CODE,IWD_I_CODE,IWD_CPOM_CODE,(ITEM_UNIT_MASTER.I_UOM_CODE) as IWD_UOM_CODE,IWD_INSP_NO,I_NAME,SPOM_PO_NO,cast(isnull(IWD_REV_QTY,0) as numeric(10,3)) as IWD_REV_QTY,cast(isnull(IWD_CON_OK_QTY,0) as numeric(10,3)) as IWD_CON_OK_QTY,cast(isnull(IWD_CON_REJ_QTY,0) as numeric(10,3)) as IWD_CON_REJ_QTY,cast(isnull(IWD_CON_SCRAP_QTY,0) as numeric(10,3)) as IWD_CON_SCRAP_QTY,ITEM_UNIT_MASTER.I_UOM_NAME as UOM_NAME ,ROUND(IWD_RATE,2) AS  IWD_RATE,IWM_P_CODE  from INWARD_MASTER,INWARD_DETAIL,ITEM_MASTER,SUPP_PO_MASTER,ITEM_UNIT_MASTER where ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE  AND INWARD_MASTER.ES_DELETE=0 and INWARD_MASTER.IWM_CODE=INWARD_DETAIL.IWD_IWM_CODE and IWD_INSP_FLG=0 and INWARD_DETAIL.IWD_I_CODE=ITEM_MASTER.I_CODE and INWARD_DETAIL.IWD_CPOM_CODE=SUPP_PO_MASTER.SPOM_CODE  AND IWD_I_CODE='" + icode + "' AND   IWM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and IWM_CODE='" + IM_No + "' ");
                if (dt.Rows.Count > 0)
                {

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Purchase Schedule Approval", "getInfo", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region txtVSA_QTY_PPC_TextChanged
    protected void txtVSA_QTY_PPC_TextChanged(object sender, EventArgs e)
    {
        TextBox thisCheckBox = (TextBox)sender;
        GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
        int index = thisGridViewRow.RowIndex;

        if (Convert.ToString(((Label)dgMaterialAcceptance.Rows[index].FindControl("lblVS_STATUS")).Text) == "1" || Convert.ToString(((Label)dgMaterialAcceptance.Rows[index].FindControl("lblVS_STATUS")).Text).ToUpper() == "TRUE")
        {
            CommonClasses.Execute("UPDATE VENDOR_SCHEDULE SET VS_STATUS=0 where ES_DELETE=0 AND VS_DATE='" + Convert.ToDateTime(txtInspDate.Text).ToString("dd/MMM/yyyy") + "' AND VS_I_CODE='" + ((Label)dgMaterialAcceptance.Rows[index].FindControl("lblI_CODE")).Text + "' ");
            // CommonClasses.Execute("UPDATE VENDOR_SCHEDULE SET VS_STATUS=0 where ES_DELETE=0 AND VS_DATE='" + Convert.ToDateTime(txtInspDate.Text).ToString("dd/MMM/yyyy") + "' AND VS_I_CODE='" + ((Label)dgMaterialAcceptance.Rows[index].FindControl("lblI_CODE")).Text + "' AND VS_P_CODE='" + ((Label)dgMaterialAcceptance.Rows[index].FindControl("lblP_CODE")).Text + "'");
        }

        Calculate();

    }
    #endregion txtVSA_QTY_PPC_TextChanged

    #region Calculate
    public void Calculate()
    {
        double sum = 0;
        string totalStr = "";
        string PurchaseQty = "";
        string rate = "";
        string Amount = "";
        for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
        {
            GridViewRow gRow = dgMaterialAcceptance.Rows[i];
            TextBox txtQty = (TextBox)gRow.FindControl("txtVSA_QTY_PPC");
            Label lbrate = (Label)gRow.FindControl("lblI_INV_RATE");
            Label lblamt = (Label)gRow.FindControl("lblAMT");
            PurchaseQty = txtQty.Text.Trim();
            rate = lbrate.Text.Trim();
            Amount = lblamt.Text.Trim();

            if (PurchaseQty == "")
            {
                PurchaseQty = "0.00";
            }
            if (rate == "")
            {
                rate = "0.00";
            }
            if (Amount == "")
            {
                Amount = "0.00";
            }
            totalStr = DecimalMasking(PurchaseQty);
            PurchaseQty = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));

            totalStr = DecimalMasking(rate);
            rate = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            Amount = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(PurchaseQty) * Convert.ToDouble(rate)), 2));

            ((Label)gRow.FindControl("lblAMT")).Text = Amount;

            sum += Convert.ToDouble(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text);
            txtTotal.Text = Convert.ToDouble(sum).ToString();
        }
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
                dt = CommonClasses.Execute("Select * FROM VENDORSCHEDULE_APPROVAL WHERE VSA_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0 and  VSA_MONTH='" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "'");
                if (dt.Rows.Count == 0)
                {
                    for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                    {
                        //CommonClasses.Execute1("INSERT INTO VENDORSCHEDULE_APPROVAL( VSA_MONTH, VSA_I_CODE, VSA_QTY,VSA_QTY_PPC, VSA_RATE, VSA_AMT,VSA_CM_CODE,VSA_P_CODE)VALUES ( '" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblQTY")).Text + "','" + ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtVSA_QTY_PPC")).Text + "' ,'" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_INV_RATE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text + "','" + (string)Session["CompanyCode"].ToString() + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblP_CODE")).Text + "')");
                        CommonClasses.Execute1("INSERT INTO VENDORSCHEDULE_APPROVAL( VSA_MONTH, VSA_I_CODE, VSA_QTY,VSA_QTY_PPC, VSA_RATE, VSA_AMT,VSA_CM_CODE)VALUES ( '" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblQTY")).Text + "','" + ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtVSA_QTY_PPC")).Text + "' ,'" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_INV_RATE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text + "','" + (string)Session["CompanyCode"].ToString() + "' )");
                    }
                    CommonClasses.WriteLog("VENDORSCHEDULE_APPROVAL", "Save", "VENDORSCHEDULE_APPROVAL", Convert.ToString(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy")), Convert.ToInt32(0), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    Response.Redirect("~/PPC/VIEW/ViewVendorScheduleApproval.aspx", false);
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Already Exist for Selected Month";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtInspDate.Focus();
                }
            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                result = CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL SET ES_DELETE=1 where VSA_MONTH='" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "' AND VSA_CM_CODE = " + (string)Session["CompanyCode"] + " and ES_DELETE=0");//Insert From Store here

                for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
                {
                    CommonClasses.Execute1("INSERT INTO VENDORSCHEDULE_APPROVAL( VSA_MONTH, VSA_I_CODE, VSA_QTY,VSA_QTY_PPC, VSA_RATE, VSA_AMT,VSA_CM_CODE)VALUES ( '" + Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy") + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblQTY")).Text + "','" + ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtVSA_QTY_PPC")).Text + "' ,'" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_INV_RATE")).Text + "','" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblAMT")).Text + "','" + (string)Session["CompanyCode"].ToString() + "' )");
                    if (Convert.ToString((((Label)dgMaterialAcceptance.Rows[i].FindControl("lblVS_STATUS")).Text)) == "1" || Convert.ToString(((Label)dgMaterialAcceptance.Rows[i].FindControl("lblVS_STATUS")).Text).ToUpper() == "TRUE")
                    {
                        CommonClasses.Execute("UPDATE VENDOR_SCHEDULE SET VS_STATUS=0 where ES_DELETE=0 AND VS_DATE='" + Convert.ToDateTime(txtInspDate.Text).ToString("dd/MMM/yyyy") + "' AND VS_I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "'  ");

                        //CommonClasses.Execute("UPDATE VENDOR_SCHEDULE SET VS_STATUS=0 where ES_DELETE=0 AND VS_DATE='" + Convert.ToDateTime(txtInspDate.Text).ToString("dd/MMM/yyyy") + "' AND VS_I_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblI_CODE")).Text + "' AND VS_P_CODE='" + ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblP_CODE")).Text + "'");
                    }
                }
                CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL  SET MODIFY=0 where VSA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

                CommonClasses.WriteLog("VENDORSCHEDULE_APPROVAL", "Update", "VENDORSCHEDULE_APPROVAL", Convert.ToString(Convert.ToDateTime(txtInspDate.Text).ToString("01/MMM/yyyy")), Convert.ToInt32(0), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                result = true;
                Response.Redirect("~/PPC/VIEW/ViewVendorScheduleApproval.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "SaveRec", Ex.Message);
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
            CommonClasses.SendError("Vendor Schedule Approval -ADD", "ClearTextBoxes", Ex.Message);
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

            }
            else if (str == "MOD")
            {

            }
            else if (str == "ADD")
            {

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "ViewRec", Ex.Message);
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
        #region UpdateStatus
        if (e.CommandName.Equals("Status"))
        {
            int index = Convert.ToInt32(e.CommandArgument.ToString());
            string um_code = e.CommandArgument.ToString();
            int Index = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgMaterialAcceptance.Rows[Index];

            string Status = ((LinkButton)(row.FindControl("lnkPost"))).Text;

            string qty = ((Label)(row.FindControl("lblQTY"))).Text;
            ((TextBox)(row.FindControl("txtVSA_QTY_PPC"))).Text = qty.ToString();


            if (Convert.ToString(((Label)dgMaterialAcceptance.Rows[index].FindControl("lblVS_STATUS")).Text) == "1" || Convert.ToString(((Label)dgMaterialAcceptance.Rows[index].FindControl("lblVS_STATUS")).Text).ToUpper() == "TRUE")
            {
                CommonClasses.Execute("UPDATE VENDOR_SCHEDULE SET VS_STATUS=0 where ES_DELETE=0 AND VS_DATE='" + Convert.ToDateTime(txtInspDate.Text).ToString("dd/MMM/yyyy") + "' AND VS_I_CODE='" + ((Label)dgMaterialAcceptance.Rows[index].FindControl("lblI_CODE")).Text + "'");
            }
            // ((Label)(row.FindControl("lblVS_STATUS"))).Text = "0";
        }

        #endregion UpdateStatus
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

            bool result = false;
            DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
            string msg = "";

            SaveRec();


        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "btnSave_Click", Ex.Message);
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
                CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL  SET MODIFY=0 where VSA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

                Response.Redirect("~/PPC/View/ViewVendorScheduleApproval.aspx", false);
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
            CommonClasses.SendError("Vendor Schedule Approval", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnInspCancel_Click
    protected void btnInspCancel_Click(object sender, EventArgs e)
    {
        //CommonClasses.Execute("UPDATE ISSUE_MASTER SET  MODIFY=0 where IM_CODE='" + IM_No + "'");
        CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL  SET MODIFY=0 where VSA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

        Response.Redirect("~/PPC/View/ViewVendorScheduleApproval.aspx", false);
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
            CommonClasses.SendError("Vendor Schedule Approval", "btnOk_Click", Ex.Message);
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
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                CommonClasses.Execute1("UPDATE VENDORSCHEDULE_APPROVAL  SET MODIFY=0 where VSA_MONTH ='" + Convert.ToDateTime(ViewState["type"]).ToString("dd/MMM/yyyy") + "' AND  ES_DELETE=0");

            }

            Response.Redirect("~/PPC/View/ViewVendorScheduleApproval.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {

            flag = false;

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Vendor Schedule Approval", "CheckValid", Ex.Message);
        }
        return flag;
    }
    #endregion

    #region dgMaterialAcceptance_RowDataBound1
    protected void dgMaterialAcceptance_RowDataBound1(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            if (drv["VS_STATUS"].ToString().ToUpper().Equals("TRUE"))
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
    #endregion

    #region btnAccept All
    protected void Button1_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < dgMaterialAcceptance.Rows.Count; i++)
        {
            ((TextBox)dgMaterialAcceptance.Rows[i].FindControl("txtVSA_QTY_PPC")).Text = ((Label)dgMaterialAcceptance.Rows[i].FindControl("lblQTY")).Text;
        }
    }
    #endregion
}
