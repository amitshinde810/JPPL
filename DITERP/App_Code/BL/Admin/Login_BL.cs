using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Login_BL
/// </summary>
public class Login_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = new DatabaseAccessLayer();
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region "Variables"
    #endregion

    #region "Properties"
    #endregion

    #region "Constructor"
    public Login_BL()
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region "Methods"
    
    #region VerifyLogin
    public DataTable VerifyLogin(string username, string password, string CompanyId, string CompCode)
    {
        try
        {
            //DL_DBAccess = new DatabaseAccessLayer();
            DataTable dt = new DataTable();

            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@UserName", username);
            par[1] = new SqlParameter("@Password", password);
            par[2] = new SqlParameter("@CompanyId", CompanyId);
            par[3] = new SqlParameter("@CompCode", CompCode);
          
            //dt = DL_DBAccess.SelectData("STRP_SMP_VerifyLogin", par);
            dt = DL_DBAccess.SelectData("SP_VerifyLogin", par);
            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {}
    }
    #endregion


    #region DataTable for Email
    public DataTable DataEmail()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClasses.Execute("SELECT SMP_RG_ID as [Registration ID],SMP_RG_DATE as [Registration Date],SMP_RG_COMP_NAME as [Company Name],SMP_RG_CNT_FNAME as [First Name],SMP_RG_CNT_LNAME as [Last Name],SMP_RG_MOB as [Mobile No] FROM SMP_RG_MSR order by SMP_RG_DATE desc");
        }
        catch (Exception ex)
        {

        }
        return dt;
    }
    #endregion
    
    //#region GetCompanyName
    //public DataSet GetCompanyName()
    //{
    //    try
    //    {
    //        DL_DBAccess = new DatabaseAccessLayer();
    //        ds = DL_DBAccess.SelectDataDataset("SP_HR_GetCompanyName", null);
    //        return ds;
    //    }
    //    catch (Exception)
    //    {
    //        return null;

    //    }
    //}
    //#endregion

    //#region GetFinancialYear
    //public DataSet GetFinancialYear(string companyid)
    //{
    //    try
    //    {
    //        DL_DBAccess = new DatabaseAccessLayer();
    //        SqlParameter [] parm = new SqlParameter[1];
    //        parm[0] = new SqlParameter("@CompanyId", companyid);
    //        ds = DL_DBAccess.SelectDataDataset("SP_CM_GetFinancialYear", parm);
    //        return ds;
    //    }
    //    catch (Exception)
    //    {
    //        return null;

    //    }
    //}
    //#endregion

    #endregion
}
