using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public class MonthlyDeductions_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region Variables"
    int _MD_CODE;
    int _MD_CM_CODE;
    int _MD_BM_CODE;
    int _MD_EM_CODE;
    int _MD_EDM_CODE;
    int _MD_MONTH;
    int _MD_YEAR;
    string _MD_REMARK;
    double _MD_AMOUNT;
    public string Msg = "";

    public int userCode;
    public string userName = "";



    #endregion

    #region  "Propertey"

    #region MD_CODE
    public int MD_CODE
    {
        get { return _MD_CODE; }
        set { _MD_CODE = value; }
    }
    #endregion

    #region MD_CM_CODE
    public int MD_CM_CODE
    {
        get { return _MD_CM_CODE; }
        set { _MD_CM_CODE = value; }
    }
    #endregion

    #region MD_BM_CODE
    public int MD_BM_CODE
    {
        get { return _MD_BM_CODE; }
        set { _MD_BM_CODE = value; }
    }
    #endregion

    #region MD_EM_CODE
    public int MD_EM_CODE
    {
        get { return _MD_EM_CODE; }
        set { _MD_EM_CODE = value; }
    }
    #endregion

    #region MD_EDM_CODE
    public int MD_EDM_CODE
    {
        get { return _MD_EDM_CODE; }
        set { _MD_EDM_CODE = value; }
    }
    #endregion

    #region MD_MONTH
    public int MD_MONTH
    {
        get { return _MD_MONTH; }
        set { _MD_MONTH = value; }
    }
    #endregion

    #region MD_YEAR
    public int MD_YEAR
    {
        get { return _MD_YEAR; }
        set { _MD_YEAR = value; }
    }
    #endregion

    #region MD_REMARK
    public string MD_REMARK
    {
        get { return _MD_REMARK; }
        set { _MD_REMARK = value; }
    }
    #endregion

    #region MD_AMOUNT
    public double MD_AMOUNT
    {
        get { return _MD_AMOUNT; }
        set { _MD_AMOUNT = value; }
    }
    #endregion


    #endregion

    #region "Constructor"
    public MonthlyDeductions_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public MonthlyDeductions_BL(int Id)
    {
        _MD_EDM_CODE = Id;
    }
    #endregion


    #region UserDefine Methods

    #region FillCombo
    public DataTable GetCountry(string Query)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@Query", Query);

            dt = DL_DBAccess.SelectData("SP_HR_EmpCountryCombo", par);

        }
        catch (Exception ex)
        {
        }
        return dt;
    }
    #endregion FillCombo

    #region GetInfo
    public DataTable GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@MD_EDM_CODE", MD_EDM_CODE);
            par[1] = new SqlParameter("@MD_CM_CODE", MD_CM_CODE);
            par[2] = new SqlParameter("@MD_MONTH", MD_MONTH);
            par[3] = new SqlParameter("@MD_YEAR", MD_YEAR);
            par[4] = new SqlParameter("@MD_BM_CODE", MD_BM_CODE);

            dt = DL_DBAccess.SelectData("SP_HR_GetInfoMonthlyDeduction", par);

        }
        catch (Exception ex)
        {

        }
        return dt;
    }
    #endregion

    #region Save
    public bool Save()
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[8];
            par[0] = new SqlParameter("@MD_CM_CODE", MD_CM_CODE);
            par[1] = new SqlParameter("@MD_EM_CODE", MD_EM_CODE);
            par[2] = new SqlParameter("@MD_EDM_CODE", MD_EDM_CODE);
            par[3] = new SqlParameter("@MD_MONTH", MD_MONTH);
            par[4] = new SqlParameter("@MD_YEAR", MD_YEAR);
            par[5] = new SqlParameter("@MD_AMOUNT", MD_AMOUNT);
            par[6] = new SqlParameter("@MD_REMARK", MD_REMARK);
            par[7] = new SqlParameter("@MD_BM_CODE", MD_BM_CODE);



            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthlyDeduction", par);
            if (result)
            {
                CommonClasses.Execute("DELETE FROM HR_LOAN_TRANSACTION WHERE MONTH(LTR_DATE) = " + MD_MONTH + " AND YEAR(LTR_DATE)=" + MD_YEAR + " AND LTR_EM_CODE=" + MD_EM_CODE + " and LTR_TYPE='Installment'");
                DataTable dt = CommonClasses.Execute("select max(MD_CODE) as  MD_CODE from HR_MONTHLY_DEDUCTIONS");
                DateTime date = new DateTime(MD_YEAR, MD_MONTH, 1);
                DataTable dtLoanCode = CommonClasses.Execute("SELECT CST_DEDLOAN FROM CM_TAXITION WHERE CST_CM_COMP_ID=" + MD_CM_CODE + "");
                if (MD_EDM_CODE.ToString() == dtLoanCode.Rows[0][0].ToString())
                {
                    SqlParameter[] par1 = new SqlParameter[6];
                    par1[0] = new SqlParameter("@LTR_CM_CODE", MD_CM_CODE);
                    par1[1] = new SqlParameter("@LTR_DOC_CODE", dt.Rows[0]["MD_CODE"].ToString());
                    par1[2] = new SqlParameter("@LTR_EM_CODE", MD_EM_CODE);
                    par1[3] = new SqlParameter("@LTR_DATE", date);
                    par1[4] = new SqlParameter("@LTR_AMT", -(MD_AMOUNT));
                    par1[5] = new SqlParameter("@LTR_TYPE", "Installment");
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertLoanTrans", par1);
                }
                if (result == true)
                {
                    DataTable dtAdvCode = CommonClasses.Execute("SELECT CST_DEDADVANCE FROM CM_TAXITION WHERE CST_CM_COMP_ID=" + MD_CM_CODE + "");
                    if (MD_EDM_CODE.ToString() == dtAdvCode.Rows[0][0].ToString())
                    {
                        CommonClasses.Execute("INSERT INTO HR_ADVANCE_TRANSACTION(HR_AT_EM_CODE,HR_AT_MONTH,HR_AT_YEAR,HR_AT_TRAN_CODE,HR_AT_TYPE,HR_AT_TRAN_AMT,HR_AT_BAL_AMT,HR_AT_TRAN_FROM)VALUES('" + MD_EM_CODE + "','" + MD_MONTH + "','" + MD_YEAR + "','" + dt.Rows[0]["MD_CODE"].ToString() + "','Advance Allocation','" + MD_AMOUNT + "','" + MD_AMOUNT + "','MONTH DED')");
                    }
                }
            }


        }
        catch (Exception Ex)
        {

        }
        return result;
    }
    #endregion

    #region Update
    public bool Update()
    {
        bool result = false;

        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] par = new SqlParameter[7];
            par[0] = new SqlParameter("@MD_CM_CODE", MD_CM_CODE);
            par[1] = new SqlParameter("@MD_EM_CODE", MD_EM_CODE);
            par[2] = new SqlParameter("@MD_EDM_CODE", MD_EDM_CODE);
            par[3] = new SqlParameter("@MD_MONTH", MD_MONTH);
            par[4] = new SqlParameter("@MD_YEAR", MD_YEAR);
            par[5] = new SqlParameter("@MD_AMOUNT", MD_AMOUNT);
            par[6] = new SqlParameter("@MD_REMARK", MD_REMARK);

            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthlyDeduction", par);
            if (result)
            {
                CommonClasses.Execute("DELETE FROM HR_LOAN_TRANSACTION WHERE MONTH(LTR_DATE) = " + MD_MONTH + " AND YEAR(LTR_DATE)=" + MD_YEAR + " AND LTR_EM_CODE=" + MD_EM_CODE + " and LTR_TYPE='Installment'");
                DataTable dt = CommonClasses.Execute("select max(MD_CODE) as  MD_CODE from HR_MONTHLY_DEDUCTIONS");
                DateTime date = new DateTime(MD_YEAR, MD_MONTH, 1);
                DataTable dtLoanCode = CommonClasses.Execute("SELECT CST_DEDLOAN FROM CM_TAXITION WHERE CST_CM_COMP_ID=" + MD_CM_CODE + "");
                if (MD_EDM_CODE.ToString() == dtLoanCode.Rows[0][0].ToString())
                {
                    SqlParameter[] par1 = new SqlParameter[6];
                    par1[0] = new SqlParameter("@LTR_CM_CODE", MD_CM_CODE);
                    par1[1] = new SqlParameter("@LTR_DOC_CODE", dt.Rows[0]["MD_CODE"].ToString());
                    par1[2] = new SqlParameter("@LTR_EM_CODE", MD_EM_CODE);
                    par1[3] = new SqlParameter("@LTR_DATE", date);
                    par1[4] = new SqlParameter("@LTR_AMT", -(MD_AMOUNT));
                    par1[5] = new SqlParameter("@LTR_TYPE", "Installment");
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertLoanTrans", par1);
                }
                if (result == true)
                {
                    DataTable dtAdvCode = CommonClasses.Execute("SELECT CST_DEDADVANCE FROM CM_TAXITION WHERE CST_CM_COMP_ID=" + MD_CM_CODE + "");
                    if (MD_EDM_CODE.ToString() == dtAdvCode.Rows[0][0].ToString())
                    {
                        CommonClasses.Execute("INSERT INTO HR_ADVANCE_TRANSACTION(HR_AT_EM_CODE,HR_AT_MONTH,HR_AT_YEAR,HR_AT_TRAN_CODE,HR_AT_TYPE,HR_AT_TRAN_AMT,HR_AT_BAL_AMT,HR_AT_TRAN_FROM)VALUES('" + MD_EM_CODE + "','" + MD_MONTH + "','" + MD_YEAR + "','" + dt.Rows[0]["MD_CODE"].ToString() + "','Advance Allocation','" + MD_AMOUNT + "','" + MD_AMOUNT + "','MONTH DED')");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
        return result;
    }
    #endregion

    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@MD_EDM_CODE", MD_EDM_CODE);
            par[1] = new SqlParameter("@MD_MONTH", MD_MONTH);
            par[2] = new SqlParameter("@MD_YEAR", MD_YEAR);
            DataTable dt = new DataTable();
            dt = GetInfo();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string query = "Delete from HR_LOAN_TRANSACTION where LTR_DOC_CODE=" + dt.Rows[i]["MD_CODE"].ToString() + " and LTR_TYPE='Installment'";
                SqlParameter[] par1 = new SqlParameter[1];
                par1[0] = new SqlParameter("@Query", query);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_Delete", par1);
                CommonClasses.Execute("DELETE FROM HR_ADVANCE_TRANSACTION WHERE HR_AT_TRAN_CODE=" + dt.Rows[i]["MD_CODE"].ToString() + " AND HR_AT_TYPE='Advance Allocation'");
                CommonClasses.WriteLog("HR Monthly Deductions", "Delete", "HR Monthly Deductions", MD_MONTH + "/" + MD_YEAR, Convert.ToInt32(dt.Rows[i]["MD_CODE"].ToString()), MD_CM_CODE, userName, userCode);
            }
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_DeleteMonthlyDeduction", par);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        { }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[2];
            par[0] = new SqlParameter("@MD_CM_CODE", MD_CM_CODE);
            par[1] = new SqlParameter("@MD_BM_CODE", MD_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_FILLMonthlyDeduction", par);
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #endregion

}