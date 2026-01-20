using System;
using System.Web.UI;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class IRN_ADD_EffectivenessOfCAPA : System.Web.UI.Page
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
            // string i_name = Request.QueryString[3].ToString();
            string Cond = Request.QueryString[3].ToString();
            // string Party = Request.QueryString[4].ToString();
            // string Vendor = Request.QueryString[6].ToString();
            // string InHouse = Request.QueryString[7].ToString();

            GenerateReport(Title, From, Todt, Cond);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Issue To Production Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string Todt, string Cond)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            //Query = "SELECT * into #Temp FROM (SELECT CRM_CODE,CRM_NO,CONVERT(char(3), CRM_DATE, 0) AS CRM_DATE1,month(CRM_DATE) as CRMDATE1,CONVERT(char(3),CRM_DATE,0) AS CRM_DATE,CRM_COMPLAINT_NO,I_CODENO,I_NAME,P_NAME,CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_FILE,ISNULL(CRD_QTY,0) AS CRD_QTY,CRD_REASON,CRD_TYPE FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER, PARTY_MASTER WHERE " + Cond + " ITEM_UNIT_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND CRM_CM_COMP_CODE='" + Session["CompanyCode"] + "' AND CRM_CODE=CRD_CRM_CODE AND I_CODE=CRD_I_CODE AND CRM_P_CODE=P_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE)SRC PIVOT(SUM(CRD_QTY) FOR CRM_DATE IN ([JAN],[FEB],[MAR],[APR],[MAY],[JUN],[JUL],[AUG],[SEP],[OCT],[NOV],[DEC])) AS PIVOTTABLE select case when #Temp.CRD_TYPE='0' and #Temp.CRMDATE1= MONTH(getdate()) then 'No' else 'Yes' End as YesOrNo , * from #Temp drop table #Temp";
            // 11/12/2018 :- Change in query for add 12 months result(yes/no) with checking CRD_TYPE=0 and selected month

            //not understand 
            //Query = "SELECT * into #Temp FROM (SELECT CRM_CODE,CRM_NO,CONVERT(char(3), CRM_DATE, 0) AS CRM_DATE1,month(CRM_DATE) as CRMDATE1,CONVERT(char(3),CRM_DATE,0) AS CRM_DATE,CRM_COMPLAINT_NO,I_CODENO,I_NAME,P_NAME,CRD_REASON AS CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE,CRD_FILE,ISNULL(CRD_QTY,0) AS CRD_QTY,CRD_REASON,CRD_TYPE FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER, PARTY_MASTER WHERE " + Cond + " ITEM_UNIT_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CRM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND CRM_CM_COMP_CODE='" + Session["CompanyCode"] + "' AND CRM_CODE=CRD_CRM_CODE AND I_CODE=CRD_I_CODE AND CRM_P_CODE=P_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE)SRC PIVOT(SUM(CRD_QTY) FOR CRM_DATE IN ([JAN],[FEB],[MAR],[APR],[MAY],[JUN],[JUL],[AUG],[SEP],[OCT],[NOV],[DEC])) AS PIVOTTABLE select case when #Temp.CRD_TYPE='0' and #Temp.CRMDATE1= MONTH(getdate()) then 'No' else 'Yes' End as YesOrNo , CRM_CODE,CRM_NO	,CRM_DATE1	,CRMDATE1	,CRM_COMPLAINT_NO	,I_CODENO	,I_NAME	,P_NAME	,CRD_DEF_OBSERVED	,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE	,CRD_FILE	,CRD_REASON	,CRD_TYPE,case when #Temp.CRD_TYPE='0' and isnull(#Temp.JAN,0) >0 then 'Yes' else 'No' End as JAN ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.FEB,0) >0 then 'Yes' else 'No' End as FEB ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.MAR,0) >0 then 'Yes' else 'No' End as MAR ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.APR,0) >0 then 'Yes' else 'No' End as APR ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.MAY,0) >0 then 'Yes' else 'No' End as MAY ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.JUN,0) >0 then 'Yes' else 'No' End as JUN ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.JUL,0) >0 then 'Yes' else 'No' End as JUL ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.AUG,0) >0 then 'Yes' else 'No' End as AUG ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.SEP,0) >0 then 'Yes' else 'No' End as SEP ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.OCT,0) >0 then 'Yes' else 'No' End as OCT ,case when #Temp.CRD_TYPE='0' and isnull(#Temp.NOV,0) >0 then 'Yes' else 'No' End as NOV,case when #Temp.CRD_TYPE='0' and isnull(#Temp.DEC,0) >0 then 'Yes' else 'No' End as [DEC]  from #Temp drop table #Temp";

            //if Selected vendor and Inhouse with

            string strmonth = "";

            strmonth = Convert.ToDateTime(Todt).ToString("MMM") + ',' + Convert.ToDateTime(Todt).AddMonths(-1).ToString("MMM") + ',' + Convert.ToDateTime(Todt).AddMonths(-2).ToString("MMM") + ',' + Convert.ToDateTime(Todt).AddMonths(-3).ToString("MMM") + ',' + Convert.ToDateTime(Todt).AddMonths(-4).ToString("MMM") + ',' + Convert.ToDateTime(Todt).AddMonths(-5).ToString("MMM");
            //ok 
            ///Query = "SELECT   DISTINCT I_CODENO, I_NAME,   P_NAME,   CRD_DEF_OBSERVED,   CRD_ACTION_TAKEN,  CRD_ROUTE_CAUSE,  CASE WHEN len(ISNULL(jan,0) )>1 then '1' ELSE ISNULL(jan,0)  END AS     jan,CASE WHEN len(ISNULL(feb,0) )>1 then '1' ELSE ISNULL(feb,0)  END AS     feb,CASE WHEN len(ISNULL(mar,0) )>1 then '1' ELSE ISNULL(mar,0)  END AS     mar,CASE WHEN len(ISNULL(apr,0) )>1 then '1' ELSE ISNULL(apr,0)  END AS     apr,CASE WHEN len(ISNULL(may,0) )>1 then '1' ELSE ISNULL(may,0)  END AS     may,CASE WHEN len(ISNULL(jun,0) )>1 then '1' ELSE ISNULL(jun,0)  END AS     jun,CASE WHEN len(ISNULL(jul,0) )>1 then '1' ELSE ISNULL(jul,0)  END AS     jul,CASE WHEN len(ISNULL(aug,0) )>1 then '1' ELSE ISNULL(aug,0)  END AS     aug,CASE WHEN len(ISNULL(sep,0) )>1 then '1' ELSE ISNULL(sep,0)  END AS     sep,CASE WHEN len(ISNULL(oct,0) )>1 then '1' ELSE ISNULL(oct,0)  END AS     oct,CASE WHEN len(ISNULL(nov,0) )>1 then '1' ELSE ISNULL(nov,0)  END AS     nov,CASE WHEN len(ISNULL(dec,0) )>1 then '1' ELSE ISNULL(dec,0)  END AS     dec   into #temp   FROM (SELECT  CONVERT(varchar, CRM_DATE, 106) as CRM_DATE,I_CODENO,I_NAME,P_NAME,CRD_REASON AS CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE,CONVERT(char(3), CRM_DATE, 0) as CRMDATE1 FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER, PARTY_MASTER WHERE " + Cond + " ITEM_UNIT_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND CRM_DATE between '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' and '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND CRM_CM_COMP_CODE='" + Session["CompanyCode"] + "'  AND CRM_CODE=CRD_CRM_CODE AND I_CODE=CRD_I_CODE AND CRM_P_CODE=P_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE ) as s   PIVOT(    MAX(CRM_DATE)    FOR [CRMDATE1] IN (jan, feb, mar, apr,     may, jun, jul, aug, sep, oct, nov, dec))AS pvt  SELECT I_CODENO	,I_NAME,	P_NAME,	CRD_DEF_OBSERVED,	CRD_ACTION_TAKEN	,CRD_ROUTE_CAUSE,	SUM(CONVERT (float,jan))   AS jan,SUM(CONVERT (float,feb))   AS 	feb,SUM(CONVERT (float,mar))  AS 	mar,SUM(CONVERT (float,apr))   AS 	apr,SUM(CONVERT (float,may))   AS 	may,SUM(CONVERT (float,jun))   AS 	jun,SUM(CONVERT (float,jul))   AS 	jul,SUM(CONVERT (float,aug))   AS 	aug,SUM(CONVERT (float,sep))   AS 	sep,SUM(CONVERT (float,oct))   AS 	oct,SUM(CONVERT (float,nov))  AS 	nov,SUM(CONVERT (float,dec))   AS 	dec    FROM #temp GROUP BY  I_CODENO	,I_NAME,	P_NAME,	CRD_DEF_OBSERVED,	CRD_ACTION_TAKEN	,CRD_ROUTE_CAUSE DROP TABLE #temp";

            //  Query = "SELECT  CRM_CODE, CONVERT(varchar, CRM_DATE, 106) as CRM_DATE,month(CRM_DATE) as CRMDATE1,I_CODENO,  I_CODE,I_NAME,P_CODE,P_NAME,CRD_REASON AS CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE,CONVERT(char(3), CRM_DATE, 0) as CRMDATE2,ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)='" + Convert.ToDateTime(Todt).AddMonths(-1).Month + "' and DATEPART(YYYY,CRM_DATE)='" + Convert.ToDateTime(Todt).AddMonths(-1).Year + "' ),0) AS DEC,ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND   DATEPART(MM,CRM_DATE)='" + Convert.ToDateTime(Todt).AddMonths(-2).Month + "' and DATEPART(YYYY,CRM_DATE)='" + Convert.ToDateTime(Todt).AddMonths(-2).Year + "'),0) AS NOV, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)='" + Convert.ToDateTime(Todt).AddMonths(-3).Month + "' and DATEPART(YYYY,CRM_DATE)='" + Convert.ToDateTime(Todt).AddMonths(-3).Year + "'),0) AS OCT, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=09 and DATEPART(YYYY,CRM_DATE)=2018),0) AS SEP, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND  IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=08 and DATEPART(YYYY,CRM_DATE)=2018),0) AS AUG,ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0  AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=07 and DATEPART(YYYY,CRM_DATE)=2018),0) AS JUL ,  ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=06 and DATEPART(YYYY,CRM_DATE)=2018),0) AS JUN ,ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=05 and DATEPART(YYYY,CRM_DATE)=2018),0) AS MAY, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=04 and DATEPART(YYYY,CRM_DATE)=2018),0) AS APR, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=03 and DATEPART(YYYY,CRM_DATE)=2018),0) AS MAR, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=02 and DATEPART(YYYY,CRM_DATE)=2018),0) AS FEB, ISNULL((SELECT top 1'1' FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL where CRM_CODE=CRD_CRM_CODE AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND IRN_CUSTOMER_REJECTION_DETAIL.CRD_I_CODE=I_CODE AND CRM_P_CODE=P_CODE AND DATEPART(MM,CRM_DATE)=01 and DATEPART(YYYY,CRM_DATE)=2018),0) AS JAN   FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER, PARTY_MASTER WHERE  ITEM_UNIT_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0 AND  DATEPART(MM,CRM_DATE)='" + Convert.ToDateTime(Todt).Month + "' and DATEPART(YYYY,CRM_DATE)='" + Convert.ToDateTime(Todt).Year + "'  AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 AND CRM_CM_COMP_CODE='-2147483647' AND CRM_CODE=CRD_CRM_CODE AND I_CODE=CRD_I_CODE AND CRM_P_CODE=P_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE";

            //NEW Qury 
            Query = "SELECT   DISTINCT I_CODENO,   I_NAME,   P_NAME,   CRD_DEF_OBSERVED,   CRD_ACTION_TAKEN,  CRD_ROUTE_CAUSE,  CASE WHEN len(ISNULL(jan,0) )>1 then 1 ELSE ISNULL(jan,0)  END AS     jan,CASE WHEN len(ISNULL(feb,0) )>1 then 1 ELSE ISNULL(feb,0)  END AS     feb,CASE WHEN len(ISNULL(mar,0) )>1 then 1 ELSE ISNULL(mar,0)  END AS     mar,CASE WHEN len(ISNULL(apr,0) )>1 then 1 ELSE ISNULL(apr,0)  END AS     apr,CASE WHEN len(ISNULL(may,0) )>1 then 1 ELSE ISNULL(may,0)  END AS     may,CASE WHEN len(ISNULL(jun,0) )>1 then 1 ELSE ISNULL(jun,0)  END AS     jun,CASE WHEN len(ISNULL(jul,0) )>1 then 1 ELSE ISNULL(jul,0)  END AS     jul,CASE WHEN len(ISNULL(aug,0) )>1 then 1 ELSE ISNULL(aug,0)  END AS     aug,CASE WHEN len(ISNULL(sep,0) )>1 then 1 ELSE ISNULL(sep,0)  END AS     sep,CASE WHEN len(ISNULL(oct,0) )>1 then 1 ELSE ISNULL(oct,0)  END AS     oct,CASE WHEN len(ISNULL(nov,0) )>1 then 1 ELSE ISNULL(nov,0)  END AS     nov,CASE WHEN len(ISNULL(dec,0) )>1 then 1 ELSE ISNULL(dec,0)  END AS     dec  into #temp  FROM (SELECT  CONVERT(varchar, CRM_DATE, 106) as CRM_DATE,month(CRM_DATE) as CRMDATE1,I_CODENO,I_NAME,P_NAME,CRD_REASON AS CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE,CONVERT(char(3), CRM_DATE, 0) as CRMDATE2  FROM IRN_CUSTOMER_REJECTION_MASTER,IRN_CUSTOMER_REJECTION_DETAIL,ITEM_MASTER,ITEM_UNIT_MASTER, PARTY_MASTER WHERE  " + Cond + "  ITEM_UNIT_MASTER.ES_DELETE=0 AND ITEM_MASTER.ES_DELETE=0     AND IRN_CUSTOMER_REJECTION_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0   AND CRM_CODE=CRD_CRM_CODE AND I_CODE=CRD_I_CODE AND CRM_P_CODE=P_CODE AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  ) as ss   PIVOT(  MAX(CRM_DATE)  FOR [CRMDATE2] IN (jan, feb, mar, apr,  may, jun, jul, aug, sep, oct, nov, dec))AS pvt   SELECT I_CODENO,I_NAME,P_NAME,CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE,SUM(jan) AS  jan,SUM(feb) AS  feb,SUM(mar) AS  mar,SUM(apr) AS  apr,SUM(may) AS  may,SUM(jun) AS  jun,SUM(jul) AS  jul,SUM(aug) AS  aug,SUM(sep) AS  sep, SUM(oct) AS  oct,SUM(nov) AS  nov,SUM(dec) AS  dec    into #temp2  FROM #temp GROUP BY  I_CODENO,I_NAME,P_NAME,CRD_DEF_OBSERVED,CRD_ACTION_TAKEN,CRD_ROUTE_CAUSE  declare @sqlqury varchar(max) = ''DECLARE  @number INT = 6  declare @month as varchar(3) declare @startmonth as varchar(3) = CONVERT(char(3), DATEADD(month, -5,  '" + Convert.ToDateTime(Todt).ToString("dd/MMM/yyyy") + "' ), 0) While(@number > 0) Begin set @month =  CONVERT(char(3), DATEADD(month, -@number,  '" + Convert.ToDateTime(Todt).AddDays(1).ToString("dd/MMM/yyyy") + "' ), 0) if @number=6  set @sqlqury =   @month else  set @sqlqury =   @month +'+'+ @sqlqury  set @number = @number - 1 End set @sqlqury = 'SELECT  #temp2.*,('+ @sqlqury + ') as Total , CASE WHEN ('+ @startmonth+'=1 and ('+ @sqlqury +')=1 )then '+'''Effective''' +' when ('+@sqlqury +') > 1 then '+'''Non Effective'''+'      when ('+ @startmonth+'=0 and ('+@sqlqury +')  = 1) then '+'''Under Monitoring'''+'   ELSE '+'''Effective''' +' END   AS  YesOrNo  FROM #temp2  ' print @sqlqury  exec(@sqlqury) DROP TABLE #temp DROP TABLE #temp2";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);

            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/EffectiveOfCapaRpt.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/EffectiveOfCapaRpt.rpt");

                rptname.Refresh();
                rptname.SetDataSource(dt);

                DateTime dtDate = System.DateTime.Now;

                //11/12/2018 :- for showing status CAPA EFFECTIVENES using date range 
                /* Logic :- check last 6 months (Yes/No) from todate and hide unhide the Status (EFFECTIVE/UNDER MONITERING)*/


                DateTime dtTodate = new DateTime();
                dtTodate = Convert.ToDateTime(Todt.ToString());

                rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                rptname.SetParameterValue("txtCurrentMonth", dtDate.ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("txtPeriod", "" + Title + " from " + "" + Convert.ToDateTime(From).ToString("MMM yyyy") + " To " + Convert.ToDateTime(dtTodate).ToString("MMM yyyy") + "");
                rptname.SetParameterValue("txtTodate", Convert.ToDateTime(dtTodate).ToString("MM"));
                rptname.SetParameterValue("txtFromdate", Convert.ToDateTime(dtTodate).AddMonths(-6).ToString("MM")); // 6 MONTHS BACK FROM TODATE
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
            Response.Redirect("~/IRN/VIEW/ViewIRNCustRejectionReport8D.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}

