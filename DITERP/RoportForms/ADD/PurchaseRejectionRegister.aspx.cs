using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_PurchaseRejectionRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl home = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase");
        home.Attributes["class"] = "active";
        HtmlGenericControl home1 = (HtmlGenericControl)this.Page.Master.FindControl("Menus").FindControl("Purchase1MV");
        home1.Attributes["class"] = "active";
    }
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/PurchaseRejectionRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

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
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
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

            Query = "Select PR_CODE,PRD_PR_CODE,PR_NO ,I_CODENO,I_NAME,I_UOM_NAME,P_NAME,CONVERT(VARCHAR,PR_DATE,106) AS PR_DATE,cast(PRD_RATE as numeric(10,3)) as  PRD_RATE,cast(PRD_ORIGINAL_QTY as numeric(10,3)) as  PRD_ORIGINAL_QTY, cast(PRD_CHALLAN_QTY as numeric(10,3)) as  PRD_CHALLAN_QTY, cast(PRD_RECEIVED_QTY as numeric(10,3)) as  PRD_RECEIVED_QTY, cast(PRD_AMOUNT as numeric(10,3)) as  PRD_AMOUNT from PURCHASE_REJECTION_MASTER,PURCHASE_REJECTION_DETAIL,SUPP_PO_MASTER,ITEM_UNIT_MASTER,ITEM_MASTER,PARTY_MASTER where PURCHASE_REJECTION_MASTER.PR_P_CODE=P_CODE and PR_CODE=PRD_PR_CODE and ITEM_UNIT_MASTER.I_UOM_CODE=PRD_UOM and SPOM_CODE= PRD_PO_CODE and I_CODE=PRD_I_CODE";

            #region Detail
            if (way == "Detail")
            {
                #region Datewise
                if (group == "Datewise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
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
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "'";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";
                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
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
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "'";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                }
                #endregion
                #region CustWise
                if (group == "CustWise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";
                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
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
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "'";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
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
                        Query = Query;
                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";
                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
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
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "'";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                }
                #endregion
                #region ItemWise
                if (group == "ItemWise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";
                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
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
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "'";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                }
                #endregion
                #region CustWise
                if (group == "CustWise")
                {
                    if (date1 == "All" && cust == "All" && item == "All")
                    {
                        Query = Query;
                    }
                    if (date1 != "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "'";
                    }
                    if (date1 != "All" && cust == "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "'";
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
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                    if (date1 != "All" && cust != "All" && item == "All")
                    {
                        Query = Query + " and PR_DATE between '" + Convert.ToDateTime(From).ToString("yyyy/MM/dd") + "' and '" + Convert.ToDateTime(To).ToString("yyyy/MM/dd") + "' and PARTY_MASTER.P_NAME='" + p_name + "'";
                    }
                    if (date1 == "All" && cust != "All" && item != "All")
                    {
                        Query = Query + " and PARTY_MASTER.P_NAME='" + p_name + "' and ITEM_MASTER.I_NAME='" + i_name + "' ";
                    }
                }
                #endregion
            }
            #endregion

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptPurchaseRejectionRegisterDateWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptPurchaseRejectionRegisterDateWise.rpt");
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
                    rptname.Load(Server.MapPath("~/Reports/rptPurchaseRejectionRegisterItemWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptPurchaseRejectionRegisterItemWise.rpt");
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
                    rptname.Load(Server.MapPath("~/Reports/rptPurchaseRejectionSuppWise.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptPurchaseRejectionSuppWise.rpt");
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
