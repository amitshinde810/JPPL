using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_ExportInvoiceRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            string From = Request.QueryString[4].ToString();
            string To = Request.QueryString[5].ToString();
            string p_name = Request.QueryString[6].ToString();
            string i_name = Request.QueryString[7].ToString();
            string group = Request.QueryString[8].ToString();
            string way = Request.QueryString[9].ToString();

            i_name = i_name.Replace("'", "''");


            #region Detail
            if (way == "Detail")
            {
                if (group == "Datewise")
                {

                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {

                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                    }
                }
                if (group == "CustWise")
                {

                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                    }
                }
            }
            #endregion

            #region Summary
            if (way == "Summary")
            {
                if (group == "Datewise")
                {
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                    }
                }
                if (group == "ItemWise")
                {
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                    }
                }
                if (group == "CustWise")
                {
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                    }
                    if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                    {
                        GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                    }
                    if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                    {
                        GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                    }
                }

            }
            #endregion


        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string date1, string cust, string item, string From, string To, string p_name, string i_name, string group, string way)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            //Query = "SELECT DISTINCT(I_CODE),P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,round((IND_INQTY*IND_RATE),0) AS IND_AMT FROM CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER WHERE CPOM_TYPE='-2147483647' and INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and IND_I_CODE=CPOD_I_CODE ";
            Query = "SELECT DISTINCT(I_CODE),I_CODENO,P_CODE,P_NAME,P_ADD1,P_CST,P_VAT,P_ECC_NO,INM_NO,INM_DATE,INM_TRANSPORT,I_NAME,CPOD_CUST_I_CODE,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,round((IND_INQTY*IND_RATE),0) AS IND_AMT,cast(isnull(IND_AMT,0) as numeric(20,2)) as IND_AMT,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0) as numeric(20,2)) as INM_OCTRI_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_G_AMT,0) as numeric(20,2)) as INM_G_AMT,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT FROM CUSTPO_DETAIL,CUSTPO_MASTER,INVOICE_MASTER,INVOICE_DETAIL,ITEM_MASTER,PARTY_MASTER WHERE CPOM_TYPE='-2147483647' and INM_CODE = IND_INM_CODE and CPOD_CPOM_CODE = CPOM_CODE and INM_P_CODE = P_CODE AND INM_P_CODE = CPOM_P_CODE and CPOM_CODE = IND_CPOM_CODE AND IND_I_CODE=CPOD_I_CODE and IND_I_CODE = I_CODE and IND_I_CODE=CPOD_I_CODE and INVOICE_MASTER.ES_DELETE=0 ";

            #region Detail
            if (way == "Detail")
            {
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                       // Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' ";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    //if (date1 == "All" && cust == "All" && item == "All")
                    //{
                    //    Query = Query + " and LG_U_NAME='" + u_name + "' ";
                    //}
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        //Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' ";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    //if (date1 == "All" && cust == "All" && item == "All")
                    //{
                    //    Query = Query + " and LG_U_NAME='" + u_name + "' ";
                    //}
                }
                #endregion
                #region CustWise
                if (group == "CustWise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                       // Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' ";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    //if (date1 == "All" && cust == "All" && item == "All")
                    //{
                    //    Query = Query + " and LG_U_NAME='" + u_name + "' ";
                    //}
                }
                #endregion
            }
            #endregion

            #region Summary
            if (way == "Summary")
            {
                #region Datewise
                if (group == "Datewise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        //Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' ";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    //if (date1 == "All" && cust == "All" && item == "All")
                    //{
                    //    Query = Query + " and LG_U_NAME='" + u_name + "' ";
                    //}
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                       // Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' ";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    //if (date1 == "All" && cust == "All" && item == "All")
                    //{
                    //    Query = Query + " and LG_U_NAME='" + u_name + "' ";
                    //}
                }
                #endregion
                #region CustWise
                if (group == "CustWise")
                {

                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        //Query = Query;


                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";

                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";

                    }
                    if (date1 == "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' ";

                    }
                    if (date1 == "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and ITEM_MASTER.I_NAME='" + i_name + "' ";

                    }
                    if (date1 != "All" && cust == "All" && item != "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and CPOM_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' ";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    //if (date1 == "All" && cust == "All" && item == "All")
                    //{
                    //    Query = Query + " and LG_U_NAME='" + u_name + "' ";
                    //}
                }
                #endregion

            }
            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
           // Query = Query + " GROUP BY INM_LR_DATE,I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO,INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE";
            Query = Query + "    GROUP BY  INM_LR_DATE,I_CODENO, I_CODE,INM_NO,P_CODE,P_NAME,P_ADD1,P_VEND_CODE,P_CST,P_VAT,P_ECC_NO, INM_DATE,INM_TRANSPORT,INM_LR_NO,I_NAME,CPOD_CUST_I_CODE,CPOD_CUST_I_NAME,CPOM_PONO,CPOM_DATE,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_INQTY,IND_RATE,IND_AMT,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_PACK_AMT,INM_ACCESSIBLE_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT, INM_TAXABLE_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_ROUNDING_AMT,INM_G_AMT,INM_TAX_TCS_AMT";

            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {


                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptExportInvoiceDatewiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptExportInvoiceDatewiseReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "ItemWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptExportInvoiceItemwiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptExportInvoiceItemwiseReg.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "CustWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptExportInvoiceCustwiseReg.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptExportInvoiceCustwiseReg.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    if (way == "Summary")
                    {
                        rptname.SetParameterValue("txtType", "1");

                    }
                    else
                    {
                        rptname.SetParameterValue("txtType", "0");

                    }
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());

                    CrystalReportViewer1.ReportSource = rptname;

                }
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
            CommonClasses.SendError("Export Invoice Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/ViewExportInvoiceRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Export Invoice Register Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion
}
