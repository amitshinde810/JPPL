using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_SupplierAuditPrint : System.Web.UI.Page
{
 DatabaseAccessLayer DL_DBAccess = null;
 string cpom_code = "";

 protected void Page_Load(object sender,EventArgs e)
 {
  HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
  home.Attributes["class"] = "active";
  HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
  home1.Attributes["class"] = "active";
 }

 protected void Page_Init(object sender,EventArgs e)
 {
  cpom_code = Request.QueryString[0];
  // type = Request.QueryString[2];
  GenerateReport(cpom_code);
 }

 #region btnCancel_Click
 protected void btnCancel_Click(object sender,EventArgs e)
 {
  try
  {
   Response.Redirect("~/Transactions/VIEW/ViewSupplierAudit.aspx",false);
  }
  catch (Exception Ex)
  {
   CommonClasses.SendError("Supplier Audit print","btnCancel_Click",Ex.Message);
  }
 }
 #endregion

 #region GenerateReport
 private void GenerateReport(string code)
 {
  try
  {
   double DeliveryPer = 0.0,QualityPer1 = 0.0,AuditPer = 0.0,PremiumPer = 0.0,CustomerPer = 0.0;
   DataTable dtfinal = new DataTable();
   DataTable dtType = new DataTable();
   DataTable dtmaster = new DataTable();

   dtmaster = CommonClasses.Execute("SELECT * FROM SUPPLIER_AUDIT_SCORE ORDER BY SRNO");
   if (dtmaster.Rows.Count > 0)
   {
    DeliveryPer = Convert.ToDouble(dtmaster.Rows[0]["Per"].ToString());
    QualityPer1 = Convert.ToDouble(dtmaster.Rows[1]["Per"].ToString());
    AuditPer = Convert.ToDouble(dtmaster.Rows[2]["Per"].ToString());
    PremiumPer = Convert.ToDouble(dtmaster.Rows[3]["Per"].ToString());
    CustomerPer = Convert.ToDouble(dtmaster.Rows[4]["Per"].ToString());

   }
   // dtType = CommonClasses.Execute("select P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,ISNULL((ISNULL(SAM_QUALITY,0)/100*10),0) as SAM_QUALITY,ISNULL((ISNULL(SAM_DELIVERY,0)/100*10),0) as SAM_DELIVERY,ISNULL(SAM_COST,0) as SAM_COST,ISNULL(SAM_RESPONSE,0) as SAM_RESPONSE,ISNULL(SAM_TOTAL,0) as SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK,datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as MonthYr,ISNULL(SAM_DELIVERY,0) as DeliveryPer,ISNULL(SAM_TOTAL,0) as TotPer,ISNULL((SELECT ISNULL(SUM(PSD_W1_QTY+	PSD_W2_QTY+	PSD_W3_QTY+	PSD_W4_QTY),0) AS SCHEDULE_QTY FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_SCHEDULE_MONTH=SAM_FDATE AND PSM_P_CODE=SAM_P_CODE),0) AS SCHEDULE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where SAM_CODE='" + cpom_code + "' AND SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " AND P_CODE=SAM_P_CODE");

   //after adding New Filed
   dtType = CommonClasses.Execute("select P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,ISNULL((ISNULL(SAM_QUALITY,0)/100*" + AuditPer + "),0) as SAM_QUALITY,ISNULL((ISNULL(SAM_DELIVERY,0)/100*" + PremiumPer + "),0) as SAM_DELIVERY,ISNULL((ISNULL(SAM_COST,0)/100*" + CustomerPer + "),0) as SAM_COST,ISNULL(SAM_RESPONSE,0) as SAM_RESPONSE,ISNULL(SAM_TOTAL,0) as SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK,datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as MonthYr,ISNULL(SAM_DELIVERY,0) as DeliveryPer,ISNULL(SAM_TOTAL,0) as TotPer,ISNULL((SELECT ISNULL(SUM(PSD_W1_QTY+	PSD_W2_QTY+	PSD_W3_QTY+	PSD_W4_QTY),0) AS SCHEDULE_QTY FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_SCHEDULE_MONTH=SAM_FDATE AND PSM_P_CODE=SAM_P_CODE),0) AS SCHEDULE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where SAM_CODE='" + cpom_code + "' AND SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " AND P_CODE=SAM_P_CODE");
   // dtfinal = CommonClasses.Execute("select P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,ISNULL(SAM_QUALITY,0) as SAM_QUALITY,ISNULL(SAM_DELIVERY,0) as SAM_DELIVERY,ISNULL(SAM_COST,0) as SAM_COST,ISNULL(SAM_RESPONSE,0) as SAM_RESPONSE,ISNULL(SAM_TOTAL,0) as SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK, datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as MonthYr,ISNULL(SAM_DELIVERY,0) as DeliveryPer,ISNULL(SAM_TOTAL,0) as TotPer,ISNULL((SELECT ISNULL(SUM(PSD_W1_QTY+PSD_W2_QTY+PSD_W3_QTY+	PSD_W4_QTY),0) AS SCHEDULE_QTY FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_SCHEDULE_MONTH=SAM_FDATE AND PSM_P_CODE=SAM_P_CODE),0) AS SCHEDULE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where SAM_P_CODE= (SELECT SAM_P_CODE FROM SUPPLIER_AUDIT_MASTER where SAM_CODE='" + cpom_code + "')AND SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " AND P_CODE=SAM_P_CODE AND SAM_FDATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' ");



   //dtfinal = CommonClasses.Execute("select P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,ISNULL(SAM_QUALITY,0) as SAM_QUALITY,ISNULL(SAM_DELIVERY,0) as SAM_DELIVERY,ISNULL(SAM_COST,0) as SAM_COST,ISNULL(SAM_RESPONSE,0) as SAM_RESPONSE,ISNULL(SAM_TOTAL,0) as SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK, datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as MonthYr,ISNULL(SAM_DELIVERY,0) as DeliveryPer,ISNULL(SAM_TOTAL,0) as TotPer,ISNULL((SELECT ISNULL(SUM(PSD_W1_QTY+PSD_W2_QTY+PSD_W3_QTY+	PSD_W4_QTY),0) AS SCHEDULE_QTY FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_SCHEDULE_MONTH=SAM_FDATE AND PSM_P_CODE=SAM_P_CODE),0) AS SCHEDULE,(SELECT SUM(IWD_CON_OK_QTY ) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(MM,IWM_DATE)=DATEPART(MM,SAM_FDATE) AND DATEPART(YYYY,IWM_DATE)=DATEPART(YYYY,SAM_FDATE) AND IWM_P_CODE=SAM_P_CODE AND  IWM_INWARD_TYPE IN (1,0) ) AS Delivary_QTy, (SELECT SUM(IWD_CON_REJ_QTY ) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(MM,IWM_DATE)=DATEPART(MM,SAM_FDATE) AND DATEPART(YYYY,IWM_DATE)=DATEPART(YYYY,SAM_FDATE) AND IWM_P_CODE=SAM_P_CODE AND IWM_INWARD_TYPE IN (1,0) ) AS REj_QTy  into #temp from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where SAM_P_CODE= (SELECT SAM_P_CODE FROM SUPPLIER_AUDIT_MASTER where SAM_CODE='" + cpom_code + "')AND SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE =" + (string)Session["CompanyCode"] + " AND P_CODE=SAM_P_CODE AND SAM_FDATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' SELECT P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,SAM_QUALITY,SAM_DELIVERY,SAM_COST,SAM_RESPONSE,SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK,MonthYr,DeliveryPer,TotPer,SCHEDULE,Delivary_QTy,REj_QTy, CASE WHEN SCHEDULE<>0 then ROUND( Delivary_QTy/SCHEDULE* 100,2) ELSE 0 END AS DEL_PERF, CASE WHEN Delivary_QTy<>0 then ROUND( REj_QTy/Delivary_QTy* 100,2) ELSE 0 END AS Quality_PERF into #temp1 FROM #temp   SELECT P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,SAM_QUALITY,SAM_DELIVERY,SAM_COST,SAM_RESPONSE,SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK,MonthYr,DeliveryPer,TotPer,SCHEDULE,Delivary_QTy,REj_QTy,DEL_PERF, CASE WHEN Quality_PERF>1.5 then 50 WHEN Quality_PERF>1 then 75 WHEN Quality_PERF>0.5 then 90 ELSE 100 END AS Qu_PERF,ROUND(ROUND(DEL_PERF*40/100,2)+ROUND(CASE WHEN Quality_PERF>1.5 then 50 WHEN Quality_PERF>1 then 75 WHEN Quality_PERF>0.5 then 90 ELSE 100 END *40/100,2) + ROUND(SAM_QUALITY*10/100,2)+ ROUND(SAM_DELIVERY*10/100,2),0) AS TOTALDEL_PERF FROM #temp1 DROP TABLE #temp DROP TABLE #temp1 ");
   dtfinal = CommonClasses.Execute("select P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,ISNULL(SAM_QUALITY,0) as SAM_QUALITY,ISNULL(SAM_DELIVERY,0) as SAM_DELIVERY,ISNULL(SAM_COST,0) as SAM_COST,ISNULL(SAM_RESPONSE,0) as SAM_RESPONSE,ISNULL(SAM_TOTAL,0) as SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK, datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as MonthYr,ISNULL(SAM_DELIVERY,0) as DeliveryPer,ISNULL(SAM_TOTAL,0) as TotPer,ISNULL((SELECT ISNULL(SUM(PSD_W1_QTY+PSD_W2_QTY+PSD_W3_QTY+	PSD_W4_QTY),0) AS SCHEDULE_QTY FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_SCHEDULE_MONTH=SAM_FDATE AND PSM_P_CODE=SAM_P_CODE),0) AS SCHEDULE,(SELECT SUM(IWD_CON_OK_QTY ) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(MM,IWM_DATE)=DATEPART(MM,SAM_FDATE) AND DATEPART(YYYY,IWM_DATE)=DATEPART(YYYY,SAM_FDATE) AND IWM_P_CODE=SAM_P_CODE AND  IWM_INWARD_TYPE IN (1,0) ) AS Delivary_QTy, (SELECT SUM(IWD_CON_REJ_QTY ) FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(MM,IWM_DATE)=DATEPART(MM,SAM_FDATE) AND DATEPART(YYYY,IWM_DATE)=DATEPART(YYYY,SAM_FDATE) AND IWM_P_CODE=SAM_P_CODE AND IWM_INWARD_TYPE IN (1,0) ) AS REj_QTy  into #temp from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where SAM_P_CODE= (SELECT SAM_P_CODE FROM SUPPLIER_AUDIT_MASTER where SAM_CODE='" + cpom_code + "')AND SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE =" + (string)Session["CompanyCode"] + " AND P_CODE=SAM_P_CODE AND SAM_FDATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"]).ToString("dd MMM yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"]).ToString("dd MMM yyyy") + "' SELECT P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,SAM_QUALITY,SAM_DELIVERY,SAM_COST,SAM_RESPONSE,SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK,MonthYr,DeliveryPer,TotPer,SCHEDULE,Delivary_QTy,REj_QTy, CASE WHEN SCHEDULE<>0 then ROUND( Delivary_QTy/SCHEDULE* 100,2) ELSE 0 END AS DEL_PERF, CASE WHEN Delivary_QTy<>0 then ROUND( REj_QTy/Delivary_QTy* 100,2) ELSE 0 END AS Quality_PERF into #temp1 FROM #temp   SELECT P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,SAM_QUALITY,SAM_DELIVERY,SAM_COST,SAM_RESPONSE,SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK,MonthYr,DeliveryPer,TotPer,SCHEDULE,Delivary_QTy,REj_QTy,DEL_PERF, CASE WHEN Quality_PERF>1.5 then 50 WHEN Quality_PERF>1 then 75 WHEN Quality_PERF>0.5 then 90 ELSE 100 END AS Qu_PERF,ROUND(ROUND(DEL_PERF*" + QualityPer1 + "/100,2)+ROUND(CASE WHEN Quality_PERF>1.5 then 50 WHEN Quality_PERF>1 then 75 WHEN Quality_PERF>0.5 then 90 ELSE 100 END *" + QualityPer1 + "/100,2) + ROUND(SAM_QUALITY*" + PremiumPer + "/100,2)+ ROUND(SAM_DELIVERY*" + PremiumPer + "/100,2),0) AS TOTALDEL_PERF FROM #temp1 DROP TABLE #temp DROP TABLE #temp1 ");


   //dtfinal = CommonClasses.Execute("select P_NAME,P_CONTACT,SAM_CODE,SAM_FDATE,SAM_TDATE,SAM_P_CODE,ISNULL(SAM_QUALITY,0) as SAM_QUALITY,ISNULL(SAM_DELIVERY,0) as SAM_DELIVERY,ISNULL(SAM_COST,0) as SAM_COST,ISNULL(SAM_RESPONSE,0) as SAM_RESPONSE,ISNULL(SAM_TOTAL,0) as SAM_TOTAL,SAM_BUYER_REMARK,SAM_QA_REMARK, datename(m,SAM_FDATE)+' '+cast(datepart(yyyy,SAM_FDATE) as varchar) as MonthYr,ISNULL(SAM_DELIVERY,0) as DeliveryPer,ISNULL(SAM_TOTAL,0) as TotPer,ISNULL((SELECT ISNULL(SUM(PSD_W1_QTY+PSD_W2_QTY+PSD_W3_QTY+	PSD_W4_QTY),0) AS SCHEDULE_QTY FROM PURCHASE_SCHEDULE_MASTER,PURCHASE_SCHEDULE_DETAIL where PSM_CODE=PSD_PSM_CODE AND PURCHASE_SCHEDULE_MASTER.ES_DELETE=0 AND PSM_SCHEDULE_MONTH=SAM_FDATE AND PSM_P_CODE=SAM_P_CODE),0) AS SCHEDULE from SUPPLIER_AUDIT_MASTER,PARTY_MASTER where SAM_P_CODE= (SELECT SAM_P_CODE FROM SUPPLIER_AUDIT_MASTER where SAM_CODE='" + cpom_code + "')AND SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and SAM_CM_COMP_CODE=" + (string)Session["CompanyCode"] + " AND P_CODE=SAM_P_CODE AND datepart(mm,SAM_FDATE) =(SELECT datepart(mm,SAM_FDATE) FROM SUPPLIER_AUDIT_MASTER where SAM_CODE='" + cpom_code + "' and ES_DELETE=0) and datepart(YYYY,SAM_FDATE) =(SELECT datepart(YYYY,SAM_FDATE) FROM SUPPLIER_AUDIT_MASTER where SAM_CODE='" + cpom_code + "' and ES_DELETE=0)");
   DateTime dtstartdt = Convert.ToDateTime(Session["OpeningDate"]);

   DateTime dtENDdt = Convert.ToDateTime(Session["ClosingDate"]);
   DataTable dtnewFinal = new DataTable();
   DataTable dtGraphNew = new DataTable();

   if (dtnewFinal.Columns.Count == 0)
   {
    dtnewFinal.Columns.Add(new System.Data.DataColumn("P_NAME",typeof(string)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("P_CONTACT",typeof(string)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("SAM_FDATE",typeof(DateTime)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("SAM_QUALITY",typeof(float)));

    dtnewFinal.Columns.Add(new System.Data.DataColumn("SAM_DELIVERY",typeof(float)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("SAM_COST",typeof(float)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("SAM_RESPONSE",typeof(float)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("SCHEDULE",typeof(float)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("DeliveryPer",typeof(float)));
    dtnewFinal.Columns.Add(new System.Data.DataColumn("MonthYr",typeof(string)));
    dtnewFinal.Rows.Add(dtnewFinal.NewRow());

   }
   Boolean falg;
   /* Copy the Structure for Save Monthwise result and Bind Monthwise Perf...*/
   dtGraphNew = dtnewFinal.Clone();
   //dtGraphNew.Rows.Add();
   for (int i = 0; dtstartdt < dtENDdt; i++)
   {
    falg = false;
    for (int j = 0; j < dtfinal.Rows.Count; j++)
    {
     if (Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"].ToString()).ToString("dd/MMM/yyyy") == dtstartdt.ToString("dd/MMM/yyyy"))
     {
      falg = true;
      //DataTable dtGraph = CommonClasses.Execute("select P.P_CODE,P.P_NAME,P_CONTACT,ISNULL(P_VEND_CODE,'') AS P_VEND_CODE,P_E_CODE,PSD_PO_CODE,ISNULL((SELECT DISTINCT top(1)ISNULL(SPOM_PONO,'') FROM SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=PSD_PO_CODE ),0) AS SPOM_PONO,I_CODE,I_CODENO,I_NAME,PSD_MONTH_QTY AS Schedule_Qty,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_OK_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime( dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime( dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_OK_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_REV_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_REV_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_REJ_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_REJ_QTY INTO #Temp from PURCHASE_SCHEDULE_MASTER PSM INNER JOIN PURCHASE_SCHEDULE_DETAIL PSD ON PSM.PSM_CODE = PSD.PSD_PSM_CODE INNER JOIN PARTY_MASTER P ON PSM.PSM_P_CODE=P.P_CODE INNER JOIN ITEM_MASTER ON PSD.PSD_I_CODE = I_CODE and PSM.PSM_P_CODE='" + dtfinal.Rows[j]["SAM_P_CODE"].ToString() + "' AND PSM.ES_DELETE=0 AND P.ES_DELETE=0 AND datepart(mm,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' select P_CODE,P_NAME,P_CONTACT,ISNULL(SUM(IWD_CON_OK_QTY),0) as IWD_CON_OK_QTY,ISNULL(SUM(IWD_CON_REJ_QTY),0) as IWD_CON_REJ_QTY,ISNULL(SUM(Schedule_Qty),0) as Schedule_Qty,(ISNULL(SAM_QUALITY,0)/100)*10 AS SAM_QUALITY,(ISNULL(SAM_DELIVERY,0)/100)*10 AS SAM_DELIVERY,ISNULL((select isnull(SCM_NAME,'') as SCM_NAME from SUPPLIER_CATEGORY_MASTER where P_E_CODE=SCM_CODE ),'') AS Category,case when ISNULL(SUM(Schedule_Qty),0)=ISNULL(SUM(IWD_CON_OK_QTY),0) then 100 when ISNULL(SUM(Schedule_Qty),0)=0 then 0 else (ISNULL(SUM(IWD_CON_OK_QTY),0)/ISNULL(SUM(Schedule_Qty),0))*100 end as DeliveryPerf,case when ISNULL(SUM(IWD_CON_REJ_QTY),0)=0 then 0 else (ISNULL(SUM(IWD_CON_REJ_QTY),0)/ISNULL(SUM(IWD_CON_OK_QTY),0))*100 end as QualityPerf into #Temp1 from #Temp inner join SUPPLIER_AUDIT_MASTER ON P_CODE=SAM_P_CODE and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND datepart(mm,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' group by P_CODE,P_NAME,SAM_QUALITY,SAM_DELIVERY,P_E_CODE,P_CONTACT SELECT P_CODE,P_NAME,P_CONTACT,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,Category,DeliveryPerf,CASE WHEN (QualityPerf >=0 and QualityPerf<=0.5) then 100 when (QualityPerf >0.5 and QualityPerf<=1) then 90 when (QualityPerf >1 and QualityPerf<=1.5) then 75 when(QualityPerf >1.5) then 50 END AS QualityPerf into #temp2 FROM #temp1 select P_CODE,P_NAME,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,Category,DeliveryPerf,QualityPerf,isnull((ISNULL(DeliveryPerf*40/100,0)+isnull((QualityPerf*40/100),0)+isnull(SAM_DELIVERY,0)+isnull(SAM_QUALITY,0)),0) as TOTALDEL_PERF,Category,P_CONTACT from #Temp2 drop table #Temp drop table #Temp1 drop table #temp2");
      /*Change Query :- For Vendor Add R4A + R5 Rejection From IRN Module (Mahining Rejection) Vendor wise...*/
      //DataTable dtGraph = CommonClasses.Execute("select P.P_CODE,P.P_NAME,P_CONTACT,ISNULL(P_VEND_CODE,'') AS P_VEND_CODE,P_E_CODE,PSD_PO_CODE,ISNULL((SELECT DISTINCT top(1)ISNULL(SPOM_PONO,'') FROM SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=PSD_PO_CODE ),0) AS SPOM_PONO,I_CODE,I_CODENO,I_NAME,PSD_MONTH_QTY AS Schedule_Qty,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_OK_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_OK_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_REV_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_REV_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_REJ_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_REJ_QTY,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE),0) AS R4A,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE ),0) AS R5,ISNULL((SELECT distinct ISNULL(IWM_TYPE,'') FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) as SuppType INTO #Temp from PURCHASE_SCHEDULE_MASTER PSM INNER JOIN PURCHASE_SCHEDULE_DETAIL PSD ON PSM.PSM_CODE = PSD.PSD_PSM_CODE INNER JOIN PARTY_MASTER P ON PSM.PSM_P_CODE=P.P_CODE INNER JOIN ITEM_MASTER ON PSD.PSD_I_CODE = I_CODE and PSM.PSM_P_CODE='" + dtfinal.Rows[j]["SAM_P_CODE"].ToString() + "' AND PSM.ES_DELETE=0 AND P.ES_DELETE=0 AND datepart(mm,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' select P_CODE,	P_NAME,	P_CONTACT,	P_VEND_CODE,	P_E_CODE,	PSD_PO_CODE,	SPOM_PONO,	I_CODE,	I_CODENO,	I_NAME,	Schedule_Qty,	IWD_CON_OK_QTY,	IWD_REV_QTY,	case when SuppType='OUTCUSTINV' then ISNULL((isnull(R4A,0)+isnull(R5,0)),0) else IWD_CON_REJ_QTY end as IWD_CON_REJ_QTY,	R4A,	R5,	SuppType INTO #Temp3 from #Temp select P_CODE,P_NAME,P_CONTACT,ISNULL(SUM(IWD_CON_OK_QTY),0) as IWD_CON_OK_QTY,ISNULL(SUM(IWD_CON_REJ_QTY),0) as IWD_CON_REJ_QTY,ISNULL(SUM(Schedule_Qty),0) as Schedule_Qty,(ISNULL(SAM_QUALITY,0)/100)*10 AS SAM_QUALITY,(ISNULL(SAM_DELIVERY,0)/100)*10 AS SAM_DELIVERY,ISNULL((select isnull(SCM_NAME,'') as SCM_NAME from SUPPLIER_CATEGORY_MASTER where P_E_CODE=SCM_CODE ),'') AS Category,case when ISNULL(SUM(Schedule_Qty),0)=ISNULL(SUM(IWD_CON_OK_QTY),0) then 100 when ISNULL(SUM(Schedule_Qty),0)=0 then 0 else (ISNULL(SUM(IWD_CON_OK_QTY),0)/ISNULL(SUM(Schedule_Qty),0))*100 end as DeliveryPerf,case when ISNULL(SUM(IWD_CON_REJ_QTY),0)=0 then 0 else (ISNULL(SUM(IWD_CON_REJ_QTY),0)/ISNULL(SUM(IWD_CON_OK_QTY),0))*100 end as QualityPerf into #Temp1 from #Temp3 inner join SUPPLIER_AUDIT_MASTER ON P_CODE=SAM_P_CODE and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND datepart(mm,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' group by P_CODE,P_NAME,SAM_QUALITY,SAM_DELIVERY,P_E_CODE,P_CONTACT SELECT P_CODE,P_NAME,P_CONTACT,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,Category,DeliveryPerf,CASE WHEN (QualityPerf >=0 and QualityPerf<=0.5) then 100 when (QualityPerf >0.5 and QualityPerf<=1) then 90 when (QualityPerf >1 and QualityPerf<=1.5) then 75 when(QualityPerf >1.5) then 50 END AS QualityPerf into #temp2 FROM #temp1 select P_CODE,P_NAME,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,Category,DeliveryPerf,QualityPerf,isnull((ISNULL(DeliveryPerf*40/100,0)+isnull((QualityPerf*40/100),0)+isnull(SAM_DELIVERY,0)+isnull(SAM_QUALITY,0)),0) as TOTALDEL_PERF,Category,P_CONTACT from #Temp2 drop table #Temp drop table #Temp1 drop table #temp2 drop table #Temp3");/

      //asper 5th colum add new logic
      //DataTable dtGraph = CommonClasses.Execute("select P.P_CODE,P.P_NAME,P_CONTACT,ISNULL(P_VEND_CODE,'') AS P_VEND_CODE,P_E_CODE,PSD_PO_CODE,ISNULL((SELECT DISTINCT top(1)ISNULL(SPOM_PONO,'') FROM SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=PSD_PO_CODE ),0) AS SPOM_PONO,I_CODE,I_CODENO,I_NAME,PSD_MONTH_QTY AS Schedule_Qty,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_OK_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_OK_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_REV_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_REV_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_REJ_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_REJ_QTY,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE),0) AS R4A,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE ),0) AS R5,ISNULL((SELECT distinct ISNULL(IWM_TYPE,'') FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) as SuppType INTO #Temp from PURCHASE_SCHEDULE_MASTER PSM INNER JOIN PURCHASE_SCHEDULE_DETAIL PSD ON PSM.PSM_CODE = PSD.PSD_PSM_CODE INNER JOIN PARTY_MASTER P ON PSM.PSM_P_CODE=P.P_CODE INNER JOIN ITEM_MASTER ON PSD.PSD_I_CODE = I_CODE and PSM.PSM_P_CODE='" + dtfinal.Rows[j]["SAM_P_CODE"].ToString() + "' AND PSM.ES_DELETE=0 AND P.ES_DELETE=0 AND datepart(mm,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' select P_CODE,	P_NAME,	P_CONTACT,	P_VEND_CODE,	P_E_CODE,	PSD_PO_CODE,	SPOM_PONO,	I_CODE,	I_CODENO,	I_NAME,	Schedule_Qty,	IWD_CON_OK_QTY,	IWD_REV_QTY,	case when SuppType='OUTCUSTINV' then ISNULL((isnull(R4A,0)+isnull(R5,0)),0) else IWD_CON_REJ_QTY end as IWD_CON_REJ_QTY,	R4A,	R5,	SuppType INTO #Temp3 from #Temp select P_CODE,P_NAME,P_CONTACT,ISNULL(SUM(IWD_CON_OK_QTY),0) as IWD_CON_OK_QTY,ISNULL(SUM(IWD_CON_REJ_QTY),0) as IWD_CON_REJ_QTY,ISNULL(SUM(Schedule_Qty),0) as Schedule_Qty,(ISNULL(SAM_QUALITY,0)/100)*5 AS SAM_QUALITY,(ISNULL(SAM_DELIVERY,0)/100)*10 AS SAM_DELIVERY,(ISNULL(SAM_COST,0)/100)*15 AS SAM_COST,SAM_COST AS SAM_COST1,ISNULL((select isnull(SCM_NAME,'') as SCM_NAME from SUPPLIER_CATEGORY_MASTER where P_E_CODE=SCM_CODE ),'') AS Category,case when ISNULL(SUM(Schedule_Qty),0)=ISNULL(SUM(IWD_CON_OK_QTY),0) then 100 when ISNULL(SUM(Schedule_Qty),0)=0 then 0 else (ISNULL(SUM(IWD_CON_OK_QTY),0)/ISNULL(SUM(Schedule_Qty),0))*100 end as DeliveryPerf,case when ISNULL(SUM(IWD_CON_REJ_QTY),0)=0 then 0 else (ISNULL(SUM(IWD_CON_REJ_QTY),0)/ISNULL(SUM(IWD_CON_OK_QTY),0))*100 end as QualityPerf into #Temp1 from #Temp3 inner join SUPPLIER_AUDIT_MASTER ON P_CODE=SAM_P_CODE and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND datepart(mm,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' group by P_CODE,P_NAME,SAM_QUALITY,SAM_DELIVERY,SAM_COST,P_E_CODE,P_CONTACT SELECT P_CODE,P_NAME,P_CONTACT,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,SAM_COST,Category,DeliveryPerf,CASE WHEN (QualityPerf >=0 and QualityPerf<=0.5) then 100 when (QualityPerf >0.5 and QualityPerf<=1) then 90 when (QualityPerf >1 and QualityPerf<=1.5) then 75 when(QualityPerf >1.5) then 50 END AS QualityPerf,SAM_COST1 into #temp2 FROM #temp1 select P_CODE,P_NAME,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,SAM_COST,Category,DeliveryPerf,QualityPerf,isnull((ISNULL(DeliveryPerf*30/100,0)+isnull((QualityPerf*40/100),0)+isnull(SAM_DELIVERY,0)+isnull(SAM_QUALITY,0)+isnull(SAM_COST,0)),0) as TOTALDEL_PERF,Category,P_CONTACT,isnull(SAM_COST1,0) as SAM_COST1 from #Temp2 drop table #Temp drop table #Temp1 drop table #temp2 drop table #Temp3");
      // 26/12/2018 :- Add new Logic for Delivery Perf and Quality Perf
      DataTable dtGraph = CommonClasses.Execute("select P.P_CODE,P.P_NAME,P_CONTACT,ISNULL(P_VEND_CODE,'') AS P_VEND_CODE,P_E_CODE,PSD_PO_CODE,ISNULL((SELECT DISTINCT top(1)ISNULL(SPOM_PONO,'') FROM SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=PSD_PO_CODE ),0) AS SPOM_PONO,I_CODE,I_CODENO,I_NAME,PSD_MONTH_QTY AS Schedule_Qty,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_OK_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_OK_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_REV_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_REV_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_REJ_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_REJ_QTY,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE),0) AS R4A,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE ),0) AS R5,ISNULL((SELECT distinct ISNULL(IWM_TYPE,'') FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) as SuppType INTO #Temp from PURCHASE_SCHEDULE_MASTER PSM INNER JOIN PURCHASE_SCHEDULE_DETAIL PSD ON PSM.PSM_CODE = PSD.PSD_PSM_CODE INNER JOIN PARTY_MASTER P ON PSM.PSM_P_CODE=P.P_CODE INNER JOIN ITEM_MASTER ON PSD.PSD_I_CODE = I_CODE and PSM.PSM_P_CODE='" + dtfinal.Rows[j]["SAM_P_CODE"].ToString() + "' AND PSM.ES_DELETE=0 AND P.ES_DELETE=0 AND datepart(mm,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' select P_CODE,	P_NAME,	P_CONTACT,	P_VEND_CODE,	P_E_CODE,	PSD_PO_CODE,	SPOM_PONO,	I_CODE,	I_CODENO,	I_NAME,	Schedule_Qty,	IWD_CON_OK_QTY,	IWD_REV_QTY,	case when SuppType='OUTCUSTINV' then ISNULL((isnull(R4A,0)+isnull(R5,0)),0) else IWD_CON_REJ_QTY end as IWD_CON_REJ_QTY,	R4A,	R5,	SuppType INTO #Temp3 from #Temp select P_CODE,P_NAME,P_CONTACT,ISNULL(SUM(IWD_CON_OK_QTY),0) as IWD_CON_OK_QTY,ISNULL(SUM(IWD_CON_REJ_QTY),0) as IWD_CON_REJ_QTY,ISNULL(SUM(Schedule_Qty),0) as Schedule_Qty,(ISNULL(SAM_QUALITY,0)/100)*5 AS SAM_QUALITY,(ISNULL(SAM_DELIVERY,0)/100)*10 AS SAM_DELIVERY,(ISNULL(SAM_COST,0)/100)*15 AS SAM_COST,SAM_COST AS SAM_COST1,ISNULL((select isnull(SCM_NAME,'') as SCM_NAME from SUPPLIER_CATEGORY_MASTER where P_E_CODE=SCM_CODE ),'') AS Category,case when ISNULL(SUM(Schedule_Qty),0)=ISNULL(SUM(IWD_CON_OK_QTY),0) then 100 when ISNULL(SUM(Schedule_Qty),0)=0 then 0 else (ISNULL(SUM(IWD_CON_OK_QTY),0)/ISNULL(SUM(Schedule_Qty),0))*100 end as DeliveryPerf,case when ISNULL(SUM(IWD_CON_REJ_QTY),0)=0 then 0 else (ISNULL(SUM(IWD_CON_REJ_QTY),0)/ISNULL(SUM(IWD_CON_OK_QTY),0))*100 end as QualityPerf into #Temp1 from #Temp3 inner join SUPPLIER_AUDIT_MASTER ON P_CODE=SAM_P_CODE and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND datepart(mm,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,SAM_FDATE)='" + Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"]).ToString("yyyy") + "' group by P_CODE,P_NAME,SAM_QUALITY,SAM_DELIVERY,SAM_COST,P_E_CODE,P_CONTACT SELECT P_CODE,P_NAME,P_CONTACT,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,SAM_COST,Category,CASE WHEN (DeliveryPerf >=95) then 100 when (DeliveryPerf >=90 and DeliveryPerf <95) then 85 when (DeliveryPerf >=80 and DeliveryPerf<90) then 70 when(DeliveryPerf <80) then 0 END AS DeliveryPerf ,CASE WHEN (QualityPerf >=0 and QualityPerf<=1) then 100 when (QualityPerf >1 and QualityPerf<=2) then 80 when (QualityPerf >2 and QualityPerf<=3) then 70 when(QualityPerf >3) then 0 END AS QualityPerf,SAM_COST1 into #temp2 FROM #temp1 select P_CODE,P_NAME,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,SAM_COST,Category,DeliveryPerf,QualityPerf,isnull((ISNULL(DeliveryPerf*30/100,0)+isnull((QualityPerf*40/100),0)+isnull(SAM_DELIVERY,0)+isnull(SAM_QUALITY,0)+isnull(SAM_COST,0)),0) as TOTALDEL_PERF,Category,P_CONTACT,isnull(SAM_COST1,0) as SAM_COST1 from #Temp2 drop table #Temp drop table #Temp1 drop table #temp2 drop table #Temp3");
      if (dtGraph.Rows.Count > 0)
      {
       dtGraphNew.Rows.Add(dtGraph.Rows[0]["P_NAME"].ToString(),dtGraph.Rows[0]["P_CONTACT"].ToString(),Convert.ToDateTime(dtfinal.Rows[j]["SAM_FDATE"].ToString()).ToString("dd/MMM/yyyy"),dtGraph.Rows[0]["QualityPerf"].ToString(),dtGraph.Rows[0]["DeliveryPerf"].ToString(),0.0,0.0,dtGraph.Rows[0]["Schedule_Qty"].ToString(),dtGraph.Rows[0]["TOTALDEL_PERF"].ToString(),dtfinal.Rows[j]["MonthYr"].ToString());
       dtnewFinal.Rows.Add(dtGraph.Rows[0]["P_NAME"].ToString(),dtGraph.Rows[0]["P_CONTACT"].ToString(),dtfinal.Rows[j]["SAM_FDATE"].ToString(),dtGraph.Rows[0]["QualityPerf"].ToString(),dtGraph.Rows[0]["DeliveryPerf"].ToString(),dtGraph.Rows[0]["SAM_COST1"].ToString(),0,dtGraph.Rows[0]["Schedule_Qty"].ToString(),dtGraph.Rows[0]["TOTALDEL_PERF"].ToString(),dtfinal.Rows[j]["MonthYr"].ToString());

      }
      else
      {
       dtnewFinal.Rows.Add(dtfinal.Rows[0]["P_NAME"].ToString(),dtfinal.Rows[0]["P_CONTACT"].ToString(),dtstartdt,0,0,0,0,0,0,dtstartdt.ToString("MMM"));

      }
      // dtnewFinal.Rows.Add(dtfinal.Rows[j]["P_NAME"].ToString(),dtfinal.Rows[j]["P_CONTACT"].ToString(),dtfinal.Rows[j]["SAM_FDATE"].ToString(),dtfinal.Rows[j]["Qu_PERF"].ToString(),dtfinal.Rows[j]["DEL_PERF"].ToString(),dtfinal.Rows[j]["SAM_COST"].ToString(),dtfinal.Rows[j]["SAM_RESPONSE"].ToString(),dtfinal.Rows[j]["SCHEDULE"].ToString(),dtfinal.Rows[j]["TOTALDEL_PERF"].ToString(),dtfinal.Rows[j]["MonthYr"].ToString());
     }
    }
    if (falg == false)
    {
     dtnewFinal.Rows.Add(dtfinal.Rows[0]["P_NAME"].ToString(),dtfinal.Rows[0]["P_CONTACT"].ToString(),dtstartdt,0,0,0,0,0,0,dtstartdt.ToString("MMM"));
    }
    dtstartdt = dtstartdt.AddMonths(1);
   }
   dtfinal = dtGraphNew;
   DataTable dt = new DataTable();
   //Got only P_CODE from queryfile:///D:\For Asus\PCPL\ERP_LIVE\Live\SIMYAERP\RoportForms\ADD\ExportInvoicePrint.aspx
   dt = CommonClasses.Execute("SELECT ISNULL(SUM(PURCHASE_SCHEDULE_DETAIL.PSD_W1_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W2_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W3_QTY + PURCHASE_SCHEDULE_DETAIL.PSD_W4_QTY),0) AS ScheduleQty FROM PURCHASE_SCHEDULE_MASTER INNER JOIN PURCHASE_SCHEDULE_DETAIL ON PURCHASE_SCHEDULE_MASTER.PSM_CODE = PURCHASE_SCHEDULE_DETAIL.PSD_PSM_CODE INNER JOIN PARTY_MASTER ON PURCHASE_SCHEDULE_MASTER.PSM_P_CODE = PARTY_MASTER.P_CODE WHERE (PARTY_MASTER.P_INHOUSE_IND = 1) AND (PARTY_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.ES_DELETE = 0) AND (PURCHASE_SCHEDULE_MASTER.PSM_CM_COMP_ID = " + Session["CompanyId"] + ") AND (P_CODE = '" + dtType.Rows[0]["SAM_P_CODE"] + "') AND (CONVERT(DATE,PSM_SCHEDULE_MONTH) = '" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MMM yyyy") + "')");
   //dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME,ISNULL(P_VEND_CODE,'') AS P_VEND_CODE,SPOM_PONO,IWD_I_CODE,I_CODENO,I_NAME,SUM(IWD_REV_QTY) AS IWD_REV_QTY,SUM(IWD_CON_OK_QTY) AS IWD_CON_OK_QTY,SUM(IWD_CON_REJ_QTY) AS IWD_CON_REJ_QTY into #Temp FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE INNER JOIN PARTY_MASTER ON IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER ON IWD_I_CODE = I_CODE inner join SUPP_PO_MASTER SPOM on IWD_CPOM_CODE=SPOM_CODE WHERE P_CODE='" + dtfinal.Rows[0]["SAM_P_CODE"] + "' AND datepart(mm,IWM_DATE)='07' AND datepart(yyyy,IWM_DATE)='2018' AND (INWARD_MASTER.ES_DELETE = 0) GROUP BY P_CODE,P_NAME,IWD_I_CODE,I_CODENO,I_NAME,P_VEND_CODE,SPOM_PONO select P_CODE,P_NAME,P_VEND_CODE,SPOM_PONO,IWD_I_CODE,I_CODENO,I_NAME,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,PSD_MONTH_QTY as Schedule_Qty from #Temp inner join PURCHASE_SCHEDULE_MASTER as PSM on P_CODE=PSM.PSM_P_CODE INNER JOIN PURCHASE_SCHEDULE_DETAIL PSD ON PSM.PSM_CODE = PSD.PSD_PSM_CODE and PSD.PSD_I_CODE= IWD_I_CODE where P_CODE='" + dtfinal.Rows[0]["SAM_P_CODE"] + "' AND datepart(mm,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtfinal.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' drop table #Temp");
   DataTable dtInward = new DataTable();
   //OLD Qry dtInward = CommonClasses.Execute("SELECT ISNULL(SUM(IWD_REV_QTY),0) AS IWD_REV_QTY,ISNULL(SUM(IWD_CON_OK_QTY),0) AS IWD_CON_OK_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(mm,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[0]["SAM_FDATE"]).ToString("MM") + "' and DATEPART(YYYY,IWM_DATE)='" + Convert.ToDateTime(dtfinal.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IWM_P_CODE='" + dtfinal.Rows[0]["SAM_P_CODE"] + "'");
   //dtInward = CommonClasses.Execute("SELECT ISNULL(SUM(IWD_CON_OK_QTY),0) AS IWD_CON_OK_QTY,isnull(sum(IWD_CON_REJ_QTY),0) as IWD_CON_REJ_QTY FROM INWARD_MASTER,INWARD_DETAIL where IWM_CODE=IWD_IWM_CODE AND INWARD_MASTER.ES_DELETE=0 AND DATEPART(mm,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' and DATEPART(YYYY,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IWM_P_CODE='" + dtType.Rows[0]["SAM_P_CODE"] + "'");

   //Used For Remark
   DataTable DtRemark1 = CommonClasses.Execute("select SPR_CODE,SPR_NAME,SPR_FROM,SPR_TO,SPR_REMARK from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=" + (string)Session["CompanyId"] + " and Lower(SPR_NAME)='A'");
   DataTable DtRemark2 = CommonClasses.Execute("select SPR_CODE,SPR_NAME,SPR_FROM,SPR_TO,SPR_REMARK from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=" + (string)Session["CompanyId"] + " and Lower(SPR_NAME)='B'");
   DataTable DtRemark3 = CommonClasses.Execute("select SPR_CODE,SPR_NAME,SPR_FROM,SPR_TO,SPR_REMARK from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=" + (string)Session["CompanyId"] + " and Lower(SPR_NAME)='C'");
   DataTable DtRemark4 = CommonClasses.Execute("select SPR_CODE,SPR_NAME,SPR_FROM,SPR_TO,SPR_REMARK from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=" + (string)Session["CompanyId"] + " and Lower(SPR_NAME)='D'");

   //dt used for Grade chart
   DataTable dtGrade = CommonClasses.Execute("SELECT * into #temp from (select convert(varchar,SPR_FROM,SPR_TO) +' & Above' AS A,'' AS B,'' AS C,'' AS D from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=1 AND SPR_CODE=-2147483648 UNION select '' AS A,convert(varchar,SPR_FROM) +' To ' +convert(varchar,SPR_TO) AS B,'' AS C,'' AS D from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=1 AND SPR_CODE=-2147483647 UNION select '' AS A,'' AS B,convert(varchar,SPR_FROM) +' To ' +convert(varchar,SPR_TO) AS C,'' AS D from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND SPR_CODE=-2147483646 UNION select '' AS A,'' AS B,'' AS C,' Below ' +convert(varchar,SPR_TO) AS D from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID=1 AND SPR_CODE=-2147483645) AS ss SELECT MAX(A) AS A,MAX(B) AS B,MAX(C) AS C,MAX(D) AS D FRom #temp drop TABLE #temp");
   double ShortQty = 0,SchQty = 0,RejQty = 0,OkQty = 0,DeleveryPer = 0,QualityPer = 0;
   /*Calculation For Selected Month*/
   dtInward = CommonClasses.Execute("select P.P_CODE,P.P_NAME,P_CONTACT,ISNULL(P_VEND_CODE,'') AS P_VEND_CODE,P_E_CODE,PSD_PO_CODE,ISNULL((SELECT DISTINCT top(1)ISNULL(SPOM_PONO,'') FROM SUPP_PO_MASTER WHERE SUPP_PO_MASTER.ES_DELETE=0 AND SPOM_CODE=PSD_PO_CODE ),0) AS SPOM_PONO,I_CODE,I_CODENO,I_NAME,PSD_MONTH_QTY AS Schedule_Qty,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_OK_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_OK_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_REV_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_REV_QTY,ISNULL((SELECT ISNULL(SUM(ISNULL(IWD_CON_REJ_QTY,0)),0) FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) AS IWD_CON_REJ_QTY,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483645 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE),0) AS R4A,ISNULL((SELECT SUM(ISNULL(IRND_REJ_QTY,0)) FROM IRN_ENTRY,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND IRN_ENTRY.ES_DELETE=0 AND IRND_RSM_CODE=-2147483643 AND datepart(mm,IRN_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IRN_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IRND_I_CODE=I_CODE and IRND_TYPE=1 and IRND_P_CODE=PSM.PSM_P_CODE ),0) AS R5,ISNULL((SELECT distinct ISNULL(IWM_TYPE,'') FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON IWM_CODE = IWD_IWM_CODE WHERE INWARD_MASTER.ES_DELETE=0 AND IWM_P_CODE=PSM.PSM_P_CODE AND datepart(mm,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,IWM_DATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' AND IWD_I_CODE=PSD.PSD_I_CODE AND IWM_INWARD_TYPE IN (1,0) ),0) as SuppType INTO #Temp from PURCHASE_SCHEDULE_MASTER PSM INNER JOIN PURCHASE_SCHEDULE_DETAIL PSD ON PSM.PSM_CODE = PSD.PSD_PSM_CODE INNER JOIN PARTY_MASTER P ON PSM.PSM_P_CODE=P.P_CODE INNER JOIN ITEM_MASTER ON PSD.PSD_I_CODE = I_CODE and PSM.PSM_P_CODE='" + dtType.Rows[0]["SAM_P_CODE"].ToString() + "' AND PSM.ES_DELETE=0 AND P.ES_DELETE=0 AND datepart(mm,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,PSM_SCHEDULE_MONTH)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' select P_CODE,	P_NAME,	P_CONTACT,	P_VEND_CODE,	P_E_CODE,	PSD_PO_CODE,	SPOM_PONO,	I_CODE,	I_CODENO,	I_NAME,	Schedule_Qty,	IWD_CON_OK_QTY,	IWD_REV_QTY,	case when SuppType='OUTCUSTINV' then ISNULL((isnull(R4A,0)+isnull(R5,0)),0) else IWD_CON_REJ_QTY end as IWD_CON_REJ_QTY,	R4A,	R5,	SuppType INTO #Temp3 from #Temp select P_CODE,P_NAME,P_CONTACT,ISNULL(SUM(IWD_CON_OK_QTY),0) as IWD_CON_OK_QTY,ISNULL(SUM(IWD_CON_REJ_QTY),0) as IWD_CON_REJ_QTY,ISNULL(SUM(Schedule_Qty),0) as Schedule_Qty,(ISNULL(SAM_QUALITY,0)/100)*10 AS SAM_QUALITY,(ISNULL(SAM_DELIVERY,0)/100)*10 AS SAM_DELIVERY,ISNULL((select isnull(SCM_NAME,'') as SCM_NAME from SUPPLIER_CATEGORY_MASTER where P_E_CODE=SCM_CODE ),'') AS Category,case when ISNULL(SUM(Schedule_Qty),0)=ISNULL(SUM(IWD_CON_OK_QTY),0) then 100 when ISNULL(SUM(Schedule_Qty),0)=0 then 0 else (ISNULL(SUM(IWD_CON_OK_QTY),0)/ISNULL(SUM(Schedule_Qty),0))*100 end as DeliveryPerf,case when ISNULL(SUM(IWD_CON_REJ_QTY),0)=0 then 0 else (ISNULL(SUM(IWD_CON_REJ_QTY),0)/ISNULL(SUM(IWD_CON_OK_QTY),0))*100 end as QualityPerf into #Temp1 from #Temp3 inner join SUPPLIER_AUDIT_MASTER ON P_CODE=SAM_P_CODE and SUPPLIER_AUDIT_MASTER.ES_DELETE=0 AND datepart(mm,SAM_FDATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MM") + "' AND datepart(yyyy,SAM_FDATE)='" + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("yyyy") + "' group by P_CODE,P_NAME,SAM_QUALITY,SAM_DELIVERY,P_E_CODE,P_CONTACT SELECT P_CODE,P_NAME,P_CONTACT,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,Category,CASE WHEN (DeliveryPerf >=95) then 100 when (DeliveryPerf >=90 and DeliveryPerf <95) then 85 when (DeliveryPerf >=80 and DeliveryPerf<90) then 70 when(DeliveryPerf <80) then 0 END AS DeliveryPerf ,CASE WHEN (QualityPerf >=0 and QualityPerf<=1) then 100 when (QualityPerf >1 and QualityPerf<=2) then 80 when (QualityPerf >2 and QualityPerf<=3) then 70 when(QualityPerf >3) then 0 END AS QualityPerf into #temp2 FROM #temp1 select P_CODE,P_NAME,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,Schedule_Qty,SAM_QUALITY,SAM_DELIVERY,Category,DeliveryPerf,QualityPerf,isnull((ISNULL(DeliveryPerf*40/100,0)+isnull((QualityPerf*40/100),0)+isnull(SAM_DELIVERY,0)+isnull(SAM_QUALITY,0)),0) as TOTALDEL_PERF,Category,P_CONTACT from #Temp2 drop table #Temp drop table #Temp1 drop table #temp2 drop table #Temp3");
   if (dtInward.Rows.Count > 0)
   {
    DeleveryPer = Math.Round(((Convert.ToDouble(dtInward.Rows[0]["DeliveryPerf"].ToString())) * DeliveryPer) / 100,2);
    QualityPer = Math.Round(((Convert.ToDouble(dtInward.Rows[0]["QualityPerf"].ToString())) * QualityPer1) / 100,2);
   }
   else
   {
    DeleveryPer = 0; QualityPer = 0;
   }
   #region Comment_Logic_24072018
   //if (dt.Rows.Count > 0)
   //{
   // //Purchase schedule Qty
   // SchQty = Convert.ToDouble(dt.Rows[0]["ScheduleQty"]);
   //}
   //if (dtInward.Rows.Count > 0)
   //{
   // //Inward Rejected Qty
   // RejQty = Convert.ToDouble(dtInward.Rows[0]["IWD_CON_REJ_QTY"]);
   // //Inspection OK Qty
   // OkQty = Convert.ToDouble(dtInward.Rows[0]["IWD_CON_OK_QTY"]);

   // //Ok minus purchase schedule qty
   // ShortQty = SchQty - OkQty;
   // if (OkQty > SchQty)
   // {
   //  ShortQty = 0;
   // }
   // else
   // {
   // }
   //}
   //if (OkQty == 0 || ShortQty == 0)
   //{
   // RejQty = 0;
   // OkQty = 0;
   // ShortQty = 0;
   //}
   //else
   //{
   // //DeleveryPer = Math.Round(((RejQty / OkQty) * 40),2);
   // //QualityPer = Math.Round(((OkQty / ShortQty) * 40),2);

   // DeleveryPer = Math.Round(((OkQty / SchQty) * 40),2);
   // if (RejQty == 0)
   //  QualityPer = 0;
   // else
   // {
   //  QualityPer = Math.Round(((RejQty / OkQty) * 40),2);
   //  //QualityPer = QualityPer / 100 * 40;
   // }
   //}
   #endregion Comment_Logic

   #region Comment
   //double Quality = Convert.ToDouble(dtType.Rows[0]["SAM_QUALITY"].ToString());
   //double Delivery = Convert.ToDouble(dtType.Rows[0]["SAM_DELIVERY"].ToString());
   //double Sum = Quality + Delivery + DeleveryPer + QualityPer;
   //string Remark = "";
   //if (Sum >0)
   //{
   // for (int i = 0; i <= DtRemark.Rows.Count; i++)
   // {
   //  Remark = DtRemark.Rows[i]["SPR_REMARK"].ToString();
   // }
   //} 
   #endregion Comment

   if (dtfinal.Rows.Count > 0)
   {
    ReportDocument rptname = null;
    rptname = new ReportDocument();

    rptname.Load(Server.MapPath("~/Reports/rptSupplierAudit.rpt"));
    rptname.FileName = Server.MapPath("~/Reports/rptSupplierAudit.rpt");
    rptname.Refresh();
    rptname.Refresh();
    rptname.SetDataSource(dtnewFinal);
    rptname.SetParameterValue("txtCompName",Session["CompanyName"].ToString());
    rptname.SetParameterValue("txtTitle","Supplier Performance Rating");
    rptname.SetParameterValue("txtQuality",Math.Round(Convert.ToDouble(dtType.Rows[0]["SAM_QUALITY"].ToString()),2).ToString());
    rptname.SetParameterValue("txtDelivary",Math.Round(Convert.ToDouble(dtType.Rows[0]["SAM_DELIVERY"].ToString()),2).ToString());
    rptname.SetParameterValue("txtCost",Math.Round(Convert.ToDouble(dtType.Rows[0]["SAM_COST"].ToString()),2).ToString());
    rptname.SetParameterValue("txtDeleveryPer",DeleveryPer);
    rptname.SetParameterValue("txtQualityPer",QualityPer);
    rptname.SetParameterValue("txtRemark1",DtRemark1.Rows[0]["SPR_REMARK"]);
    rptname.SetParameterValue("txtRemark2",DtRemark2.Rows[0]["SPR_REMARK"]);
    rptname.SetParameterValue("txtRemark3",DtRemark3.Rows[0]["SPR_REMARK"]);
    rptname.SetParameterValue("txtRemark4",DtRemark4.Rows[0]["SPR_REMARK"]);
    //rptname.SetParameterValue("txtRemark",Remark);
    rptname.SetParameterValue("txtGradeA",dtGrade.Rows[0]["A"]);
    rptname.SetParameterValue("txtGradeB",dtGrade.Rows[0]["B"]);
    rptname.SetParameterValue("txtGradeC",dtGrade.Rows[0]["C"]);
    rptname.SetParameterValue("txtGradeD",dtGrade.Rows[0]["D"]);
    rptname.SetParameterValue("txtDeliveryPer",DeliveryPer.ToString());
    rptname.SetParameterValue("txtQualityPer1",QualityPer1.ToString());
    rptname.SetParameterValue("txtAuditPer",AuditPer.ToString());
    rptname.SetParameterValue("txtPremiumPer",PremiumPer.ToString());
    rptname.SetParameterValue("txtCustomerPer",CustomerPer.ToString());
    //rptname.SetParameterValue("txtBuyer",dtType.Rows[0]["SAM_BUYER_REMARK"].ToString());

    /* Check Grade For Particular Month*/
    DataTable dtFrom = CommonClasses.Execute("SELECT * into #TempFrom from (select SPR_FROM AS [FromA],'' AS [FromB],'' AS [FromC],'' AS [FromD] from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND SPR_CODE=-2147483648 UNION select '' AS [FromA],SPR_FROM AS [FromB],'' AS [FromC],'' AS [FromD] from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND SPR_CODE=-2147483647 UNION select '' AS [FromA],'' AS [FromB],SPR_FROM AS [FromC],'' AS [FromD] from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND SPR_CODE=-2147483646 UNION select '' AS [FromA],'' AS [FromB],'' AS [FromC],SPR_FROM AS [FromD]from SUPPLIER_PERFORMANCE_REMARK where ES_DELETE=0 and SPR_CM_COMP_ID='" + (string)Session["CompanyId"] + "' AND SPR_CODE=-2147483645) AS ss SELECT MAX([FromA]) AS [FromA],MAX([FromB]) AS [FromB],MAX([FromC]) AS [FromC],MAX([FromD]) AS [FromD] FRom #TempFrom drop TABLE #TempFrom");
    if (dtFrom.Rows.Count > 0)
    {
     rptname.SetParameterValue("txtFromA",dtFrom.Rows[0]["FromA"]);
     rptname.SetParameterValue("txtFromB",dtFrom.Rows[0]["FromB"]);
     rptname.SetParameterValue("txtFromC",dtFrom.Rows[0]["FromC"]);
     rptname.SetParameterValue("txtFromD",dtFrom.Rows[0]["FromD"]);
    }
    else
    {
     PanelMsg.Visible = true;
     lblmsg.Text = "Please Insert Data in Supplier Performance Master...";
     return;
    }
    rptname.SetParameterValue("txtMonth","Supplier Performance Rating For " + Convert.ToDateTime(dtType.Rows[0]["SAM_FDATE"]).ToString("MMM yyyy") + "");

    CrystalReportViewer1.ReportSource = rptname;
   }
  }
  catch (Exception Ex)
  {
   CommonClasses.SendError("Delivery Challan Report","GenerateReport",Ex.Message);
  }
 }
 #endregion
}

