﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_CompWiseReport : System.Web.UI.Page
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
            string L_name = Request.QueryString[4].ToString();
            string S_name = Request.QueryString[5].ToString();
            string Type = Request.QueryString[6].ToString();
            string RType = Request.QueryString[7].ToString();

            GenerateReport(Title, From, Todt, i_name, Type, L_name, S_name, RType);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string i_name, string Type, string L_name, string S_name, string RType)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            if (Type == "0")
            {
                if (L_name == "0" && S_name == "0")
                {
                    Query = " SELECT * into #temp from ( SELECT DISTINCT I_CODE,I_NAME,I_CODENO,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE  , ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY)  AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=    '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END  AS IRND_AMTONDATE, CASE WHEN  SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END   AS IRND_AMT, ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE=  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO    )AS a  SELECT * FROM #temp order by IRND_AMT desc    DROP TABLE #temp";
                }
                else if (L_name != "0" && S_name == "0")
                {
                    Query = " SELECT * into #temp from ( SELECT DISTINCT I_CODE,I_NAME,I_CODENO,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE  , ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY)  AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=    '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END  AS IRND_AMTONDATE, CASE WHEN  SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END   AS IRND_AMT, ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE=  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0     AND ID.IRND_I_CODE  IN ( SELECT DISTINCT LC_I_CODE  FROM LINE_CHANGE  where LC_ACTIVE=1  AND LC_LM_CODE='" + L_name + "' )   GROUP BY I_CODE,I_NAME,I_CODENO    )AS a  SELECT * FROM #temp order by IRND_AMT desc    DROP TABLE #temp";
                }
                else if (L_name == "0" && S_name != "0")
                {
                    Query = " SELECT * into #temp from ( SELECT DISTINCT I_CODE,I_NAME,I_CODENO,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE  , ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY)  AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=    '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END  AS IRND_AMTONDATE, CASE WHEN  SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END   AS IRND_AMT, ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE=  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0     AND ID.IRND_I_CODE  IN (SELECT DISTINCT SI_I_CODE  FROM STGAEWISE_ITEM  where SI_ACTIVE=1  AND SI_STG_CODE='" + S_name + "' )   GROUP BY I_CODE,I_NAME,I_CODENO    )AS a  SELECT * FROM #temp order by IRND_AMT desc    DROP TABLE #temp";
                }
                else
                {
                    Query = " SELECT * into #temp from ( SELECT DISTINCT I_CODE,I_NAME,I_CODENO,ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYONDTE  , ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN    '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "'  AND  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE= '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0) AS IRND_REJ_QTYONDTE, SUM(ID.IRND_REJ_QTY)  AS IRND_REJ , CASE WHEN ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)=0 then 0 ELSE ISNULL((SELECT SUM(IRND_AMT)  FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE=    '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE),0)/100000 END  AS IRND_AMTONDATE, CASE WHEN  SUM(ID.IRND_AMT)=0 THEN 0 ELSE SUM(ID.IRND_AMT)/100000 END   AS IRND_AMT, ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRND_TYPE=1 AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) =0 then 0 else ( SELECT ISNULL(SUM(IRND_REJ_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=0 AND IRND_I_CODE=I_CODE AND IRN_DATE=   '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' )/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0  AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE=  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0)) END ) *100) AS ONDATEREJ,((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ),0)) END )*100) AS CUMMDATEREJ FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   AND IR.IRN_TRANS_TYPE=0     AND ID.IRND_I_CODE  IN ( SELECT DISTINCT LC_I_CODE  FROM LINE_CHANGE  where LC_ACTIVE=1  AND LC_LM_CODE='" + L_name + "' )    AND ID.IRND_I_CODE  IN (SELECT DISTINCT SI_I_CODE  FROM STGAEWISE_ITEM  where SI_ACTIVE=1  AND SI_STG_CODE='" + S_name + "' )   GROUP BY I_CODE,I_NAME,I_CODENO    )AS a  SELECT * FROM #temp order by IRND_AMT desc    DROP TABLE #temp";
                }
            }
            else
            {
                Query = "  SELECT * into #temp from ( SELECT DISTINCT I_CODE,I_NAME,I_CODENO,DATEPART(MM,IR.IRN_DATE) AS IRN_MONTH, DATEPART(YYYY,IR.IRN_DATE) AS IRN_YEAR ,   ISNULL((SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=1AND IRND_TYPE=1 AND IRND_I_CODE=I_CODE),0) AS IRND_PROD_QTYCUMM  ,    ISNULL((SELECT SUM(IRND_REJ_QTY) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0)  AS IRND_REJ ,     CASE WHEN   ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0) =0 THEN 0 ELSE  ROUND( ISNULL((SELECT SUM(IRND_AMT) FROM IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) AND IRN_TRANS_TYPE=0  AND IRND_I_CODE=I_CODE),0)/100000 ,2) END AS IRND_AMT,     ((case when (ISNULL(( SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND IRN_TRANS_TYPE=1 AND   IRND_TYPE=1 AND IRND_I_CODE=I_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE)),0) ) =0 then 0 else (SUM(ID.IRND_REJ_QTY))/(ISNULL((SELECT ISNULL(SUM(IRND_PROD_QTY),0) FROM IRN_ENTRY ,IRN_DETAIL  where IRN_CODE=IRND_IRN_CODE AND ES_DELETE=0 AND   IRND_TYPE=1 AND IRN_TRANS_TYPE=1 AND IRND_I_CODE=I_CODE AND DATEPART(MM,IRN_DATE)=DATEPART(MM,IR.IRN_DATE) AND DATEPART(YYYY,IRN_DATE)=DATEPART(YYYY,IR.IRN_DATE) ),0)) END )*100)   AS CUMMDATEREJ     FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE AND ID.IRND_I_CODE=I_CODE  AND ID.IRND_I_CODE  IN (SELECT  DISTINCT IRND_I_CODE from   IRN_ENTRY,IRN_DETAIL where IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_DATE BETWEEN     '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ) AND IR.IRN_DATE BETWEEN   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IR.IRN_TRANS_TYPE=0 GROUP BY I_CODE,I_NAME,I_CODENO,DATEPART(MM,IRN_DATE) ,DATEPART(YYYY,IRN_DATE)   )AS a  SELECT I_CODE,I_NAME,I_CODENO,IRND_PROD_QTYCUMM,IRND_REJ,IRND_AMT,CUMMDATEREJ,   CAST(CONVERT(VARCHAR, IRN_YEAR) + '-' + CONVERT(VARCHAR, IRN_MONTH) + '-' + CONVERT(VARCHAR, 1) AS DATETIME) AS DATE FROM #temp    order by IRND_AMT desc      DROP TABLE #temp";
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (i_name != "0")
                {
                    DataView dv1 = dt.DefaultView;
                    dv1.RowFilter = "I_CODE in(" + i_name + ")";
                    dt = dv1.ToTable();
                }
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (Type == "0")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptCompStgaeWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptCompStgaeWise.rpt");
                }
                else
                {
                    if (RType == "0")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptCompGraph.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptCompGraph.rpt");
                    }
                    else if (RType == "1")
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptCompGraph1.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptCompGraph1.rpt");
                    }
                    else
                    {
                        rptname.Load(Server.MapPath("~/Reports/rptCompGraph2.rpt"));
                        rptname.FileName = Server.MapPath("~/Reports/rptCompGraph2.rpt");
                    }
                }
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "STATUS OF COMPONENTS " + Title + "  From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "  To " + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy"));
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
            Response.Redirect("~/IRN/VIEW/ViewCompWiseReport.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
