using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_RptInspectionGIN_IWIM : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string Type = "";
    string PrintType = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            string Code = Request.QueryString[1];

            Type = Request.QueryString[2].ToString();
            PrintType = Request.QueryString[3].ToString();

            GenerateReport(Title, Code, PrintType, Type);
        }
        catch (Exception Ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = Ex.Message;
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string Code, string PrintType1, string Type1)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWD_IWM_CODE=" + Code + " join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";


            if (Type1 == "IWCP")
            {
                DataTable dtparty = new DataTable();
                dtparty = CommonClasses.Execute("SELECT IWM_P_CODE FROM INWARD_MASTER where IWM_CODE='" + Code + "'");
                if (dtparty.Rows[0]["IWM_P_CODE"].ToString() == "-2147483578")
                {
                    Query = "SELECT DISTINCT 0 as SPOM_CODE,PARTY_MASTER.P_NAME AS LM_NAME, IND.IWD_BATCH_NO, IWM.IWM_INV_NO, IWM.IWM_INV_DATE, IWM.IWM_TRANSPORATOR_NAME, ITEM_MASTER.I_CODENO, IWM.IWM_LR_NO,IWM_INV_DATE  as SPOM_DATE,PARTY_MASTER.P_ADD1 AS LM_ADD1, IWM.IWM_NO, IWM.IWM_DATE, IWM.IWM_CHALLAN_NO,  IWM.IWM_CHAL_DATE, ITEM_MASTER.I_NAME, IND.IWD_CH_QTY, IND.IWD_REV_QTY, IWD_REV_QTY as   IWD_CON_OK_QTY, IND.IWD_CON_REJ_QTY, IND.IWD_RATE,  IND.IWD_REMARK, ITEM_UNIT_MASTER.I_UOM_NAME AS UOM_NAME, IWM.IWM_INSP_NO AS IWD_INSP_NO, IND.IWD_INSP_FLG, LOG_MASTER.LG_U_NAME,  LOG_MASTER.LG_U_CODE, USER_MASTER.UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN LOG_MASTER ON LOG_MASTER.LG_DOC_CODE = IWM.IWM_CODE INNER JOIN USER_MASTER ON LOG_MASTER.LG_U_CODE = USER_MASTER.UM_CODE WHERE (IND.IWD_IWM_CODE = '" + Code + "') AND (LOG_MASTER.LG_SOURCE = 'Material Inward') AND IWM.IWM_CM_CODE=" + Session["CompanyCode"] + " AND (LOG_MASTER.LG_EVENT = 'Insert') ";
                }
                else
                {
                    Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME,IWM_INSP_NO as IWD_INSP_NO,IWD_INSP_FLG ,LG_U_NAME ,LG_U_CODE,UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND  IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_MASTER.SPOM_CODE = IND.IWD_CPOM_CODE   inner join LOG_MASTER on LG_DOC_CODE=IWM_CODE  inner join USER_MASTER on LG_U_CODE=UM_CODE  WHERE IWD_IWM_CODE=" + Code + "   and  IWM.IWM_CM_CODE=" + Session["CompanyCode"] + " AND LOG_MASTER.LG_SOURCE= 'Material Inward' and LG_EVENT='Insert' --order by IWM_NO";//and IWD_INSP_FLG=1
                }
            }
            else
            {
                if (PrintType1 == "Mult")
                {
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "' and IWM.ES_DELETE=0 join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
                    Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME,IWM_INSP_NO as IWD_INSP_NO,IWD_INSP_FLG ,LG_U_NAME ,LG_U_CODE,UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND  IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_MASTER.SPOM_CODE = IND.IWD_CPOM_CODE   inner join LOG_MASTER on LG_DOC_CODE=IWM_CODE  inner join USER_MASTER on LG_U_CODE=UM_CODE  WHERE IWM_NO between '" + Title + "' and '" + Code + "' and IWM_TYPE='" + Type1.Trim() + "' and IWM.IWM_CM_CODE=" + Session["CompanyCode"] + " AND  LOG_MASTER.LG_SOURCE= 'Material Inward' and LG_EVENT='Insert' and IWD_INSP_FLG=1 order by IWM_NO";
                }
                else
                {
                    //Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME from INWARD_MASTER IWM join INWARD_DETAIL IND on IWM_CODE=IWD_IWM_CODE and IWD_IWM_CODE=" + Code + " join ITEM_MASTER on IWD_I_CODE=I_CODE join  ITEM_UNIT_MASTER on IWD_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE join PARTY_MASTER on IWM_P_CODE=P_CODE join SUPP_PO_MASTER on SPOM_CODE= IWD_CPOM_CODE left outer join INSPECTION_S_MASTER on IWM_CODE=INSM_IWM_CODE";
                    Query = "select distinct SPOM_CODE,P_NAME as LM_NAME ,IWD_BATCH_NO,IWM_INV_NO,IWM_INV_DATE,IWM_TRANSPORATOR_NAME,I_CODENO,IWM_LR_NO,SPOM_DATE,P_ADD1 as LM_ADD1,IWM_NO,IWM_DATE,IWM_CHALLAN_NO,IWM_CHAL_DATE,SPOM_PO_NO,I_NAME,IWD_CH_QTY,IWD_REV_QTY,IWD_CON_OK_QTY,IWD_CON_REJ_QTY,IWD_RATE,IWD_REMARK,I_UOM_NAME as UOM_NAME,IWM_INSP_NO as IWD_INSP_NO,IWD_INSP_FLG ,LG_U_NAME ,LG_U_CODE,UM_NAME  FROM INWARD_MASTER AS IWM INNER JOIN INWARD_DETAIL AS IND ON IWM.IWM_CODE = IND.IWD_IWM_CODE AND  IWM.ES_DELETE = 0 INNER JOIN ITEM_MASTER ON IND.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN ITEM_UNIT_MASTER ON IND.IWD_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON IWM.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN SUPP_PO_MASTER ON SUPP_PO_MASTER.SPOM_CODE = IND.IWD_CPOM_CODE   inner join LOG_MASTER on LG_DOC_CODE=IWM_CODE  inner join USER_MASTER on LG_U_CODE=UM_CODE  WHERE IWD_IWM_CODE=" + Code + "   and  IWM.IWM_CM_CODE=" + Session["CompanyCode"] + " AND  LOG_MASTER.LG_SOURCE= 'Material Inward' and LG_EVENT='Insert' and IWD_INSP_FLG=1 --order by IWM_NO";
                }
            }
            ReportDocument rptname = null;
            rptname = new ReportDocument();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            #region Count
            if (dt.Rows.Count > 0)
            {
                rptname.Load(Server.MapPath("~/Reports/RptInspectionGIN_IWIM.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/RptInspectionGIN_IWIM.rpt");
                rptname.Refresh();
                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txttitle", Session["CompanyName"].ToString());
                rptname.SetParameterValue("CompAdd", Session["CompanyAdd"].ToString());

                if (Type1.Trim() == "IWCP")
                {
                    rptname.SetParameterValue("Type", "(Cash)");
                }
                if (Type1.Trim() == "IWIFP")
                {
                    rptname.SetParameterValue("Type", "(For Process Inward)");
                }
                if (Type1.Trim() == "IWIM")
                {
                    rptname.SetParameterValue("Type", "(Raw Material)");
                }
                if (Type1.Trim() == "OUTCUSTINV")
                {
                    rptname.SetParameterValue("Type", "(Sub Contractor)");
                }
                CrystalReportViewer1.ReportSource = rptname;



            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = " No Record Found! ";

            }
            #endregion
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #region Cancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString[2].ToString()=="IWCP")  
        {
            Response.Redirect("~/Transactions/VIEW/ViewCashInward.aspx", false);  
        }
        else
        {
            Response.Redirect("~/Transactions/VIEW/ViewForProcessInward.aspx", false);
        }
    }
    #endregion
}
