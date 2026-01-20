﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class IRN_ADD_foundaryRejYearly : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    public static string strUser = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("IRN1MV");
        home1.Attributes["class"] = "active";
    }

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            ViewState["strUser"] = strUser;
            if (!IsPostBack)
            {
                ViewState["strUser"] = "0";
            }
            string Title = Request.QueryString[0];

            string From = Request.QueryString[1].ToString();
            string Todt = Request.QueryString[2].ToString();
            string i_name = Request.QueryString[3].ToString();
            string Type = Request.QueryString[4].ToString();

            GenerateReport(Title, From, Todt, i_name, Type);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string i_name, string Type)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            if (i_name == "0")
            {
               // Query = "(select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'NOS' AS types  from (SELECT I_CODE,I_CODENO,I_NAME, SUM(IRND_REJ_QTY) AS IRND_REJ_QTY,DATEPART(MM,IRN_DATE) AS IRN_MONTH FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'RS' AS types  from (SELECT I_CODE,I_CODENO,I_NAME,  CASE when SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000  END   AS IRND_REJ_QTY  ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_ENTRY.ES_DELETE=0  AND IRN_ENTRY.IRN_TRANS_TYPE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRND_TYPE=0 and IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'t  Per(%)' AS types  from ( SELECT I_CODE,I_CODENO,I_NAME,      CASE WHEN (SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  AND IRND_I_CODE=I_CODE)=0 THEN 0 ELSE (  (SELECT SUM(IRND_REJ_QTY)   FROM IRN_ENTRY,IRN_DETAIL where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  GROUP BY DATEPART(MM, IRN_DATE) )/(SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  AND IRND_I_CODE=I_CODE)*100) END  AS IRND_REJ_QTY ,DATEPART(MM, IR.IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where  IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE  AND ID.IRND_I_CODE=I_CODE    AND IR.IRN_TRANS_TYPE=0 and ID.IRND_TYPE=0  and IR.IRN_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IR.IRN_DATE),I_CODE,I_CODENO,I_NAME,DATEPART(YYYY, IR.IRN_DATE) ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME )ORDER BY I_CODENO  ,types";
                Query = "(select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'NOS' AS types  from (SELECT I_CODE,I_CODENO,I_NAME, SUM(IRND_REJ_QTY) AS IRND_REJ_QTY,DATEPART(MM,IRN_DATE) AS IRN_MONTH FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'RS' AS types  from (SELECT I_CODE,I_CODENO,I_NAME,  CASE when SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000  END   AS IRND_REJ_QTY  ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_ENTRY.ES_DELETE=0  AND IRN_ENTRY.IRN_TRANS_TYPE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRND_TYPE=0 and IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'t  Per(%)' AS types  from ( SELECT I_CODE,I_CODENO,I_NAME,      CASE WHEN (SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  AND IRND_I_CODE=I_CODE)=0 THEN 0 ELSE (  (SELECT SUM(IRND_REJ_QTY)   FROM IRN_ENTRY,IRN_DETAIL where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  GROUP BY DATEPART(MM, IRN_DATE) )/(SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  AND IRND_I_CODE=I_CODE)*100) END  AS IRND_REJ_QTY ,DATEPART(MM, IR.IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where  IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE  AND ID.IRND_I_CODE=I_CODE    AND IR.IRN_TRANS_TYPE=0 and ID.IRND_TYPE=0  and IR.IRN_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IR.IRN_DATE),I_CODE,I_CODENO,I_NAME,DATEPART(YYYY, IR.IRN_DATE) ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME      UNION       select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'total' AS types  from ( SELECT DISTINCT I_CODE,I_CODENO,I_NAME,      ISNULL(CASE WHEN (SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND    IRN_ENTRY.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_I_CODE=I_CODE)=0  THEN 0 ELSE (  (SELECT SUM(IRND_REJ_QTY)   FROM IRN_ENTRY,IRN_DETAIL where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and  IRN_ENTRY.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRND_I_CODE=I_CODE )  /(SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  IRN_ENTRY.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRND_I_CODE=I_CODE AND IRND_I_CODE=I_CODE)*100) END ,0) AS IRND_REJ_QTY , 1 AS IRN_MONTH  FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where  IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE  AND ID.IRND_I_CODE=I_CODE    AND IR.IRN_TRANS_TYPE=0 and ID.IRND_TYPE=0  and IR.IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IR.IRN_DATE),I_CODE,I_CODENO,I_NAME,DATEPART(YYYY, IR.IRN_DATE)     ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME)  ORDER BY I_CODENO  ,types";
            }
            else
            {
                Query = "(select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'NOS' AS types  from (SELECT I_CODE,I_CODENO,I_NAME, SUM(IRND_REJ_QTY) AS IRND_REJ_QTY,DATEPART(MM,IRN_DATE) AS IRN_MONTH FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and  I_CODE=" + i_name + " and  IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'   GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'RS' AS types  from (SELECT I_CODE,I_CODENO,I_NAME,  CASE when SUM(IRND_AMT)=0 then 0 else SUM(IRND_AMT)/100000  END   AS IRND_REJ_QTY  ,DATEPART(MM, IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY,IRN_DETAIL,ITEM_MASTER where IRN_ENTRY.ES_DELETE=0  AND IRN_ENTRY.IRN_TRANS_TYPE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRND_TYPE=0  and I_CODE=" + i_name + " and  IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IRN_DATE),I_CODE,I_CODENO,I_NAME ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN (  [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME UNION select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'t  Per(%)' AS types  from ( SELECT I_CODE,I_CODENO,I_NAME, CASE WHEN (SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  AND IRND_I_CODE=I_CODE)=0 THEN 0 ELSE (  (SELECT SUM(IRND_REJ_QTY)   FROM IRN_ENTRY,IRN_DETAIL where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  GROUP BY DATEPART(MM, IRN_DATE) )/(SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  DATEPART(MM, IRN_ENTRY.IRN_DATE)= DATEPART(MM, IR.IRN_DATE) AND  DATEPART(YYYY, IRN_ENTRY.IRN_DATE)= DATEPART(YYYY, IR.IRN_DATE)  AND IRND_I_CODE=I_CODE)*100) END  AS IRND_REJ_QTY ,DATEPART(MM, IR.IRN_DATE) AS IRN_MONTH  FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where  IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE  AND ID.IRND_I_CODE=I_CODE  AND IR.IRN_TRANS_TYPE=0 and ID.IRND_TYPE=0   and I_CODE=" + i_name + " and  IR.IRN_DATE between  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IR.IRN_DATE),I_CODE,I_CODENO,I_NAME,DATEPART(YYYY, IR.IRN_DATE) ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME   UNION       select ISNULL(SUM([1]),0) AS [1],ISNULL(SUM([2]),0) AS [2],ISNULL(SUM([3]),0) AS [3],ISNULL(SUM([4]),0) AS [4],ISNULL(SUM([5]),0) AS [5],ISNULL(SUM([6]),0) AS [6],ISNULL(SUM([7]),0) AS [7],ISNULL(SUM([8]),0) AS [8],ISNULL(SUM([9]),0) AS [9],ISNULL(SUM([10]),0) AS [10],ISNULL(SUM([11]),0) AS [11],ISNULL(SUM([12]),0) AS [12], I_CODE,I_CODENO,I_NAME,'total' AS types  from ( SELECT DISTINCT I_CODE,I_CODENO,I_NAME,      ISNULL(CASE WHEN (SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND    IRN_ENTRY.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRND_I_CODE=I_CODE)=0  THEN 0 ELSE (  (SELECT SUM(IRND_REJ_QTY)   FROM IRN_ENTRY,IRN_DETAIL where  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRND_I_CODE=I_CODE and IRN_TRANS_TYPE=0 AND  IRND_TYPE=0 and  IRN_ENTRY.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRND_I_CODE=I_CODE )  /(SELECT SUM(IRND_PROD_QTY) FROM IRN_ENTRY,IRN_DETAIL WHERE  IRN_ENTRY.ES_DELETE=0 AND IRN_CODE=IRND_IRN_CODE AND IRN_TRANS_TYPE=1 AND IRND_TYPE=1 AND  IRN_ENTRY.IRN_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  AND IRND_I_CODE=I_CODE AND IRND_I_CODE=I_CODE)*100) END ,0) AS IRND_REJ_QTY , 1 AS IRN_MONTH  FROM IRN_ENTRY IR,IRN_DETAIL ID,ITEM_MASTER where  IR.ES_DELETE=0 AND IR.IRN_CODE=ID.IRND_IRN_CODE  AND ID.IRND_I_CODE=I_CODE   and I_CODE=" + i_name + "  AND IR.IRN_TRANS_TYPE=0 and ID.IRND_TYPE=0  and IR.IRN_DATE between   '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "'  GROUP BY DATEPART(MM, IR.IRN_DATE),I_CODE,I_CODENO,I_NAME,DATEPART(YYYY, IR.IRN_DATE)     ) AS SOURCETABLE PIVOT (sum(IRND_REJ_QTY) for IRN_MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])) AS PivotTable GROUP BY I_CODE,I_CODENO,I_NAME)  ORDER BY I_CODENO  ,types";
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                rptname.Load(Server.MapPath("~/Reports/foundaryRejYearly.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/foundaryRejYearly.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtPeriod", "FOUNDRY REJECTION MONTH WISE From " + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(Todt).ToString("MMM yyyy"));
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
            Response.Redirect("~/IRN/VIEW/ViewfoundaryRejYearly.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
