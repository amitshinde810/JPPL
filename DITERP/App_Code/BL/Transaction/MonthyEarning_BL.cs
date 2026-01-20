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

/// <summary>
/// Summary description for MonthlyDeductions
/// </summary>
public class MonthyEarning_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region Private Variables
    private int _ME_CODE;
    private int _ME_CM_CODE;
    private int _ME_BM_CODE;
    private int _ME_EM_CODE;
    private int _ME_EEM_CODE;
    private int _ME_MONTH;
    private int _ME_YEAR;
    private float _ME_AMOUNT;
    public string Msg = "";

    public int userCode;
    public string userName = "";
    #endregion

    #region Public Properties
    public int ME_CODE
    {
        get { return _ME_CODE; }
        set { _ME_CODE = value; }
    }
    public int ME_CM_CODE
    {
        get { return _ME_CM_CODE; }
        set { _ME_CM_CODE = value; }
    }
    public int ME_BM_CODE
    {
        get { return _ME_BM_CODE; }
        set { _ME_BM_CODE = value; }
    }

    public int ME_EM_CODE
    {
        get { return _ME_EM_CODE; }
        set { _ME_EM_CODE = value; }
    }
    public int ME_EEM_CODE
    {
        get { return _ME_EEM_CODE; }
        set { _ME_EEM_CODE = value; }
    }
    public int ME_MONTH
    {
        get { return _ME_MONTH; }
        set { _ME_MONTH = value; }
    }
    public int ME_YEAR
    {
        get { return _ME_YEAR; }
        set { _ME_YEAR = value; }
    }
    public float ME_AMOUNT
    {
        get { return _ME_AMOUNT; }
        set { _ME_AMOUNT = value; }
    }

    #endregion

    #region "Constructor"
    public MonthyEarning_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public MonthyEarning_BL(int Id)
    {
        _ME_EEM_CODE = Id;
    }
    #endregion



    #region GetInfo
    public DataTable GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@ME_CM_CODE", ME_CM_CODE);
            par[1] = new SqlParameter("@ME_EEM_CODE", ME_EEM_CODE);
            par[2] = new SqlParameter("@ME_MONTH", ME_MONTH);
            par[3] = new SqlParameter("@ME_YEAR", ME_YEAR);
            par[4] = new SqlParameter("@ME_BM_CODE", ME_BM_CODE);

            dt = DL_DBAccess.SelectData("SP_HR_GetInfoMonthlyEarning", par);
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
            SqlParameter[] par = new SqlParameter[7];
            par[0] = new SqlParameter("@ME_CM_CODE", ME_CM_CODE);
            par[1] = new SqlParameter("@ME_BM_CODE", ME_BM_CODE);
            par[2] = new SqlParameter("@ME_EM_CODE", ME_EM_CODE);
            par[3] = new SqlParameter("@ME_EEM_CODE", ME_EEM_CODE);
            par[4] = new SqlParameter("@ME_MONTH", ME_MONTH);
            par[5] = new SqlParameter("@ME_YEAR", ME_YEAR);
            par[6] = new SqlParameter("@ME_AMOUNT", ME_AMOUNT);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthlyEarning", par);
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
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();

            SqlParameter[] par = new SqlParameter[7];
            par[0] = new SqlParameter("@ME_CM_CODE", ME_CM_CODE);
            par[1] = new SqlParameter("@ME_BM_CODE", ME_BM_CODE);
            par[2] = new SqlParameter("@ME_EM_CODE", ME_EM_CODE);
            par[3] = new SqlParameter("@ME_EEM_CODE", ME_EEM_CODE);
            par[4] = new SqlParameter("@ME_MONTH", ME_MONTH);
            par[5] = new SqlParameter("@ME_YEAR", ME_YEAR);
            par[6] = new SqlParameter("@ME_AMOUNT", ME_AMOUNT);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertMonthlyEarning", par);

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
            par[0] = new SqlParameter("@ME_EEM_CODE", ME_EEM_CODE);
            par[1] = new SqlParameter("@ME_MONTH", ME_MONTH);
            par[2] = new SqlParameter("@ME_YEAR", ME_YEAR);

            DataTable dt = new DataTable();
            dt = GetInfo();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CommonClasses.WriteLog("HR Monthly Earning", "Delete", "HR Monthly Earning", ME_MONTH + "/" + ME_YEAR, Convert.ToInt32(dt.Rows[i]["ME_CODE"].ToString()), ME_CM_CODE, userName, userCode);
            }
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_DeleteMonthlyEarning", par);
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
            par[0] = new SqlParameter("@ME_CM_CODE", ME_CM_CODE);
            par[1] = new SqlParameter("@ME_BM_CODE", ME_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_FILLMonthlyEarning", par);
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

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

}
