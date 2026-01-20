﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_Add8020 : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string i_name = Request.QueryString[3].ToString();
            string Cond = Request.QueryString[4].ToString();
            string Typec = Request.QueryString[5].ToString();
            string Vendor = Request.QueryString[6].ToString();
            string InHouse = Request.QueryString[7].ToString();

            GenerateReport(Title, From, Todt, i_name, Cond, Typec, Vendor, InHouse);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string i_name, string Cond, string Typec, string Vendor, string InHouse)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //if all vendor and None
            if (Vendor == "0" && InHouse == "2")
            {
                Query = " (SELECT DISTINCT I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1    AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0 AND IRND_P_CODE NOT IN (-2147483210 ) AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_P_CODE NOT IN (-2147483210 ) AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRND_P_CODE NOT IN (-2147483210 ) AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND  IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_P_CODE NOT IN (-2147483210 )  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_P_CODE NOT IN (-2147483210 ) AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE)AS a  ";
            }
            //if all vendor and with inhouse
            else if (Vendor == "0" && InHouse == "1")
            {
               // Query = " SELECT * into #temp from (SELECT DISTINCT I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1    AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0   AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND  IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE)AS a  SELECT * FROM #temp order by IRND_AMT desc DROP TABLE #temp";

                Query = " (SELECT DISTINCT I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1    AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0   AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND  IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE)AS a   ";
            }
            else if (Vendor == "0" && InHouse == "0")
            {
                Query = "   (SELECT DISTINCT I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0 AND IRND_P_CODE   IN (-2147483210 ) AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_P_CODE  IN (-2147483210 ) AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRND_P_CODE IN (-2147483210 ) AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND  IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_P_CODE IN (-2147483210 )  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_P_CODE  IN (-2147483210 ) AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE)AS a ";
            }
            //if Selected vendor and none
            else if (Vendor != "0" && InHouse == "2")
            {
                Query = " (SELECT DISTINCT I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1    AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0 AND IRND_P_CODE  IN ('" + Vendor + "' ) AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_P_CODE  IN ('" + Vendor + "') AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE  AND IRND_P_CODE  IN ('" + Vendor + "') AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND  IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_P_CODE   IN ('" + Vendor + "' )  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_P_CODE   IN ('" + Vendor + "' ) AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE)AS a  ";
            }
            //if Selected vendor and Inhouse
            else if (Vendor != "0" && InHouse == "0")
            {
                Query = "  (SELECT DISTINCT I_CODE,I_NAME,I_CODENO,ID.IRND_P_CODE,ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE  AND IRND_P_CODE   IN (-2147483210 )  AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IR.IRN_TRANS_TYPE=0  AND  ID.IRND_P_CODE='-2147483210' GROUP BY I_CODE,I_NAME,I_CODENO,ID.IRND_P_CODE,ID.IRND_TYPE)AS a   ";
            }
            //if Selected vendor and Inhouse with
            else if (Vendor != "0" && InHouse == "1")
            {
                Query = "  (SELECT DISTINCT I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND IRN_TRANS_TYPE=0  AND IRND_P_CODE   IN (-2147483210,'" + Vendor + "' ) AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY) AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_TYPE=ID.IRND_TYPE  AND  IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END AS IRND_AMTONDATE, CASE WHEN SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END AS IRND_AMT,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0  AND IRND_TYPE=ID.IRND_TYPE AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE='" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND  ID.IRND_P_CODE='" + Vendor + "'  AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO, ID.IRND_TYPE)AS a   ";
            }
            //it type =all then 
            if (Typec == "-1")
            {
                Query="SELECT * into #temp from  "+ Query+"   SELECT I_CODE,	I_NAME	,I_CODENO,		MAX(IRND_PROD_QTYONDTE) AS IRND_PROD_QTYONDTE   ,MAX(IRND_PROD_QTYCUMM) AS 	IRND_PROD_QTYCUMM,	  SUM(IRND_REJ_QTYONDTE) AS  IRND_REJ_QTYONDTE,SUM(IRND_REJ)	IRND_REJ, 	SUM(IRND_AMTONDATE) AS IRND_AMTONDATE,	SUM(IRND_AMT) AS IRND_AMT,    CASE when MAX(IRND_PROD_QTYONDTE)=0 then 0 else  ROUND((SUM(IRND_REJ_QTYONDTE)/ MAX(IRND_PROD_QTYONDTE)*100),2) END AS 	ONDATEREJ,      CASE when MAX(IRND_PROD_QTYCUMM)=0 then 0 else  ROUND((SUM(IRND_REJ)/ MAX(IRND_PROD_QTYCUMM)*100),2) END AS 	CUMMDATEREJ  FROM #temp    group by I_CODE,	I_NAME	,I_CODENO    order by 	SUM(IRND_AMT)  desc       drop table #temp " ;
            }
            else
            {
                Query = "SELECT * into #temp from  " + Query + "  SELECT * FROM #temp order by IRND_AMT desc DROP TABLE #temp ";
            }
            //if Selected vendor and Inhouse with
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);

            if (i_name != "0")
            {
                DataView dv1 = dt.DefaultView;
                dv1.RowFilter = " I_CODE in(" + i_name + ")";
                dt = dv1.ToTable();
            }
            if (Typec != "-1")
            {
                DataView dv1 = dt.DefaultView;
                if (Typec == "0")
                {
                    Typec = "False";
                }
                else
                {
                    Typec = "True";
                }
                dv1.RowFilter = " IRND_TYPE  ='" + Convert.ToBoolean(Typec) + "'";
                dt = dv1.ToTable();
            }
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rpt8020CompSpecRejPer.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rpt8020CompSpecRejPer.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                string Type = "";
                if (InHouse == "0")
                {
                    Type = "In House";
                }
                if (InHouse == "2")
                {
                    Type = "All Vendor";
                    if (Vendor != "0")
                    {
                        DataTable dtvendor = CommonClasses.Execute("SELECT * FROM PARTY_MASTER WHERE P_CODE='" + Vendor + "'");
                        Type = "Vendor " + dtvendor.Rows[0]["P_NAME"].ToString();
                    }
                }
                if (InHouse == "1")
                {
                    Type = "All Vendor And In House";
                }

                if (Typec == "-1")
                {
                    rptname.SetParameterValue("txtPeriod", "Rejection In ascending Order for " + Type + " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "  To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
                else if (Typec == "False")
                {
                    rptname.SetParameterValue("txtPeriod", "Casting Rejection In ascending Order for " + Type + " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "  To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
                else
                {
                    rptname.SetParameterValue("txtPeriod", "Mechining Rejection In ascending Order for " + Type + " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "  To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
                }
                CrystalReportViewer1.ReportSource = rptname;
                CrystalReportViewer1.ID = Title.Replace(" ", "_");
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/IRN/VIEW/View8020.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

