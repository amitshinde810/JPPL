using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class RoportForms_ADD_DispToSubContPrint : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string invoice_code = "";
    string reportType = "";
    string rrreportType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Production1MV");
        home1.Attributes["class"] = "active";
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        invoice_code = Request.QueryString[0];
        reportType = Request.QueryString[1];
        rrreportType = Request.QueryString[2];
        GenerateReport(invoice_code, rrreportType);
    }


    #region GenerateReport
    private void GenerateReport(string code, string Cond)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            DataTable dtfinal = new DataTable();
            if (reportType == "Single")
            {
                dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY, IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT,cast(isnull(SPOD_RATE,0) as numeric(20,2)) as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE, CASE when I_CAT_CODE=-2147483648 then '7616' ELSE  E_TARIFF_NO END AS  E_TARIFF_NO,E_CODE,E_COMMODITY,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE,IND_PROCESS_CODE ,PROCESS_NAME  ,CASE when SM_CODE=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)*(CASE when I_CAT_CODE=-2147483648 then 9 else E_BASIC END))/100 ),2)  ELSE 0 END AS CGST,CASE when SM_CODE=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)* (CASE when I_CAT_CODE=-2147483648 then 9 else E_EDU_CESS END) )/100 ),2)  ELSE 0 END AS SGST,CASE when SM_CODE!=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)*(CASE when I_CAT_CODE=-2147483648 then 18 else E_H_EDU END))/100 ),2)  ELSE 0 END AS IGST    FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER ,PROCESS_MASTER WHERE INM_CODE = IND_INM_CODE  AND PROCESS_CODE=IND_PROCESS_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_CODE='" + code + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE and   SPOD_I_CODE=IND_I_CODE  AND INVOICE_MASTER.ES_DELETE=0  ");
            }
            else
            {
               // dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT,cast(isnull(SPOD_RATE,0) as numeric(20,2)) as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,E_TARIFF_NO,E_CODE,E_COMMODITY,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 and   SPOD_I_CODE=IND_I_CODE  AND INM_INVOICE_TYPE=1 AND    INM_CM_CODE='" + Session["CompanyCode"] + "'  UNION SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,cast(isnull(IND_RATE,0) as numeric(20,2)) as IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT, 0 as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,E_TARIFF_NO,E_CODE,E_COMMODITY,0 AS SPOD_EXC_AMT,0 AS SPOD_EXC_E_AMT,0 AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER,BOM_MASTER,BOM_DETAIL WHERE INM_CODE = IND_INM_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 AND INM_INVOICE_TYPE=0 and BD_BM_CODE=BM_CODE and BD_I_CODE=IND_I_CODE AND SPOD_I_CODE=BM_I_CODE AND    INM_CM_CODE='" + Session["CompanyCode"] + "'");
                dtfinal = CommonClasses.Execute("SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,   IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT,cast(isnull(SPOD_RATE,0) as numeric(20,2)) as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,CASE when I_CAT_CODE=-2147483648 then '7616' ELSE  E_TARIFF_NO END AS  E_TARIFF_NO,E_CODE,E_COMMODITY,ISNULL(SPOD_EXC_AMT,0) AS SPOD_EXC_AMT,ISNULL(SPOD_EXC_E_AMT,0) AS SPOD_EXC_E_AMT,ISNULL(SPOD_EXC_HE_AMT,0) AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE,IND_PROCESS_CODE ,PROCESS_NAME ,CASE when SM_CODE=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)*(CASE when I_CAT_CODE=-2147483648 then 9 else E_BASIC END))/100 ),2)  ELSE 0 END AS CGST,CASE when SM_CODE=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)* (CASE when I_CAT_CODE=-2147483648 then 9 else E_EDU_CESS END) )/100 ),2)  ELSE 0 END AS SGST,CASE when SM_CODE!=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)*(CASE when I_CAT_CODE=-2147483648 then 18 else E_H_EDU END))/100 ),2)  ELSE 0 END AS IGST  FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER ,PROCESS_MASTER WHERE INM_CODE = IND_INM_CODE  AND PROCESS_CODE=IND_PROCESS_CODE and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 and   SPOD_I_CODE=IND_I_CODE  AND INM_INVOICE_TYPE=1 AND    INM_CM_CODE='" + Session["CompanyCode"] + "'  UNION SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_VEND_CODE,INM_CODE,INM_NO,INM_DATE,ISNULL(INM_TRANSPORT,'') as INM_TRANSPORT,INM_VEH_NO,I_NAME,I_CODENO,SPOM_PO_NO,SPOM_PONO,CONVERT(VARCHAR(11),SPOM_DATE,113) as SPOM_DATE,cast(isnull(IND_INQTY,0) as numeric(20,3)) as IND_INQTY,  IND_RATE,cast(isnull(SPOD_DISC_PER,0) as numeric(20,2)) as SPOD_DISC_PER,cast(isnull(SPOD_DISC_AMT,0) as numeric(20,2)) as SPOD_DISC_AMT, 0 as SPOD_RATE,ITEM_UNIT_MASTER.I_UOM_NAME,IND_ACT_WEIGHT ,CASE WHEN P_LBT_IND=1 then  P_LBT_NO ELSE 'NA' END AS P_GST_NO ,SM_NAME,SM_STATE_CODE,CASE when I_CAT_CODE=-2147483648 then '7616' ELSE  E_TARIFF_NO END AS E_TARIFF_NO,E_CODE,E_COMMODITY,0 AS SPOD_EXC_AMT,0 AS SPOD_EXC_E_AMT,0 AS SPOD_EXC_HE_AMT,INM_ISSUE_DATE,IND_PROCESS_CODE ,PROCESS_NAME ,CASE when SM_CODE=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)*(CASE when I_CAT_CODE=-2147483648 then 9 else E_BASIC END))/100 ),2)  ELSE 0 END AS CGST,CASE when SM_CODE=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)* (CASE when I_CAT_CODE=-2147483648 then 9 else E_EDU_CESS END) )/100 ),2)  ELSE 0 END AS SGST,CASE when SM_CODE!=P_SM_CODE then  ROUND((((IND_INQTY*IND_RATE)*(CASE when I_CAT_CODE=-2147483648 then 18 else E_H_EDU END))/100 ),2)  ELSE 0 END AS IGST  FROM SUPP_PO_DETAILS,SUPP_PO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER,STATE_MASTER,EXCISE_TARIFF_MASTER,BOM_MASTER,BOM_DETAIL,PROCESS_MASTER  WHERE INM_CODE = IND_INM_CODE  AND PROCESS_CODE=IND_PROCESS_CODE  and SPOD_SPOM_CODE = SPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = SPOM_P_CODE and P_SM_CODE=SM_CODE and SPOM_CODE = IND_CPOM_CODE and IND_I_CODE = I_CODE AND INM_NO BETWEEN '" + invoice_code + "' AND '" + reportType + "' and ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND I_E_CODE=E_CODE  AND INVOICE_MASTER.ES_DELETE=0 AND INM_INVOICE_TYPE=0 and BD_BM_CODE=BM_CODE and BD_I_CODE=IND_I_CODE AND SPOD_I_CODE=BM_I_CODE AND    INM_CM_CODE='" + Session["CompanyCode"] + "'");
            }

            if (dtfinal.Rows.Count > 0)
            {

                DataTable dtTemp = new DataTable();
                DataSet ds57A4 = new DataSet();
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER,STATE_MASTER where CM_STATE=SM_CODE and  CM_ID='" + Session["CompanyId"] + "' AND     CM_CODE='" + Session["CompanyCode"] + "' AND ISNULL(CM_DELETE_FLAG,0) ='0'");

                if (rrreportType == "1")
                {

                    rptname.Load(Server.MapPath("~/Reports/rptDipToSubCont_1.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDipToSubCont_1.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtState", dtComp.Rows[0]["SM_NAME"].ToString());
                    CrystalReportViewer1.ReportSource = rptname;

                }
                else
                {
                    #region Cond_2_Plain Print 

                    DataTable dt57A4 = new DataTable();
                    dt57A4 = dtfinal;

                    dtTemp.Columns.Add("INM_CODE", typeof(int));
                    dtTemp.Columns.Add("INM_NO", typeof(int));
                    dtTemp.Columns.Add("INM_NAME", typeof(string));
                    dtTemp.Columns.Add("INM_SEQNO", typeof(int));

                    int x = Convert.ToInt32(invoice_code);
                    int y = Convert.ToInt32(reportType);


                    for (; x <= y; x++)
                    {
                        DataTable dtInm = CommonClasses.Execute(" SELECT * FROM INVOICE_MASTER where INM_TYPE='OutSUBINM' AND ES_DELETE=0   AND INM_CM_CODE='" + Session["CompanyCode"] + "' AND INM_NO ='" + x.ToString() + "'");
                        code = dtInm.Rows[0]["INM_CODE"].ToString();

                        DataRow dr = dtTemp.NewRow();
                        dr["INM_CODE"] = code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Original For Job work";
                        dr["INM_SEQNO"] = 1;
                        dtTemp.Rows.Add(dr);

                        dr = dtTemp.NewRow();
                        dr["INM_CODE"] = code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Duplicate For Transporter";
                        dr["INM_SEQNO"] = 2;
                        dtTemp.Rows.Add(dr);

                        dr = dtTemp.NewRow();
                        dr["INM_CODE"] = code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Triplicate for Supplier";
                        dr["INM_SEQNO"] = 3;
                        dtTemp.Rows.Add(dr);

                        dr = dtTemp.NewRow();
                        dr["INM_CODE"] = code;
                        dr["INM_NO"] = 1;
                        dr["INM_NAME"] = "Quaradruplicate";
                        dr["INM_SEQNO"] = 4;
                        dtTemp.Rows.Add(dr);
                    }
                    DataTable dta = new DataTable();
                    dta = (DataTable)dtTemp;
                    ds57A4.Tables.Add(dt57A4);
                    ds57A4.Tables[0].TableName = "dtDispToSubCont";
                    
                    ds57A4.Tables.Add(dta);
                    ds57A4.Tables[1].TableName = "NoOfCopy57A4";

                    // Create a DataRow, add Name and Age data, and add to the DataTable
                    rptname.Load(Server.MapPath("~/Reports/rptDipToSubCont1.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDipToSubCont1.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(ds57A4);
                    rptname.VerifyDatabase();
                    rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                    rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                    rptname.SetParameterValue("txtGSTNo", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    rptname.SetParameterValue("txtUser", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    CrystalReportViewer1.ReportSource = rptname;
                    CrystalReportViewer1.RefreshReport();


                    #endregion Cond_2_Plain Print
                }


            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "GenerateReport", Ex.Message);

        }
    }
    #endregion


    protected void btnCancel_Click(object sender, EventArgs e)
    {



        Response.Redirect("~/Transactions/VIEW/ViewDispatchToSubContracter.aspx", false);
    }
}
