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
/// Summary description for CompanyMaster_BL
/// </summary>
public class DepartmentMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region "Variables"
    private string _DM_NAME;
    private int _DM_CODE;
    private int _DM_CM_CODE;
    private int _DM_BM_CODE;
    public string Msg = "";
    #endregion

    #region "Constructor"
    public DepartmentMaster_BL()
    { }
    #endregion

    #region Parameterise Constructor
    public DepartmentMaster_BL(int Id)
    {
        _DM_CODE = Id;
    }
    #endregion

    #region "Properties"

    #region DM_NAME
    public string DM_NAME
    {
        get { return _DM_NAME; }
        set { _DM_NAME = value; }
    }
    #endregion

    #region DM_CODE
    public int DM_CODE
    {
        get { return _DM_CODE; }
        set { _DM_CODE = value; }
    }
    #endregion

    #region DM_CM_CODE
    public int DM_CM_CODE
    {
        get { return _DM_CM_CODE; }
        set { _DM_CM_CODE = value; }
    }
    #endregion

    #region DM_BM_CODE
    public int DM_BM_CODE
    {
        get { return _DM_BM_CODE; }
        set { _DM_BM_CODE = value; }
    }
    #endregion

    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@DM_CODE", DM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_GetInfoDept", par);
            if (dt.Rows.Count > 0)
            {
                DM_CODE = Convert.ToInt32(dt.Rows[0]["DM_CODE"]);
                DM_NAME = dt.Rows[0]["DM_NAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Department Master Class", "GetInfo", ex.Message);
        }

    }
    #endregion

    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@DM_NAME", DM_NAME);
            par[1] = new SqlParameter("@DM_BM_CODE", DM_BM_CODE);
            par[2] = new SqlParameter("@DM_CM_CODE", DM_CM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_CheckSaveDept", par);
            if (dt.Rows.Count > 0)
                res = false;
            else
                res = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Department Master Class", "CheckExistSaveName", Ex.Message);
        }
        return res;
    }
    #endregion

    #region ChkExstUpdateName
    public bool ChkExstUpdateName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[4];
            par[0] = new SqlParameter("@DM_CODE", DM_CODE);
            par[1] = new SqlParameter("@DM_NAME", DM_NAME);
            par[2] = new SqlParameter("@DM_BM_CODE", DM_BM_CODE);
            par[3] = new SqlParameter("@DM_CM_CODE", DM_CM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_CheckUpdateDept", par);
            if (dt.Rows.Count > 0)
                res = false;
            else
                res = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Department Master Class", "ChkExstUpdateName", Ex.Message);
        }
        return res;
    }
    #endregion

    #region Save
    public bool Save()
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (CheckExistSaveName())
            {
                SqlParameter[] par = new SqlParameter[3];
                par[0] = new SqlParameter("@DM_CM_CODE", DM_CM_CODE);
                par[1] = new SqlParameter("@DM_NAME", DM_NAME);
                par[2] = new SqlParameter("@DM_BM_CODE", DM_BM_CODE);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_InsertDept", par);
            }
            else
            {
                Msg = "Department Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Department Master Class", "Save", Ex.Message);
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
            if (ChkExstUpdateName())
            {
                SqlParameter[] par = new SqlParameter[4];
                par[0] = new SqlParameter("@DM_CODE", DM_CODE);
                par[1] = new SqlParameter("@DM_CM_CODE", DM_CM_CODE);
                par[2] = new SqlParameter("@DM_NAME", DM_NAME);
                par[3] = new SqlParameter("@DM_BM_CODE", DM_BM_CODE);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_UpdateDept", par);
            }
            else
            {
                Msg = "Department Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Department Master Class", "Update", Ex.Message);
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
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@DM_CODE", DM_CODE);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_HR_DeleteDept", par);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Department Master Class", "Delete", ex.Message);
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
            par[0] = new SqlParameter("@DM_CM_CODE", DM_CM_CODE);
             par[1] = new SqlParameter("@DM_BM_CODE", DM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_HR_FILLDEPT", par);
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Department Master Class", "FillGrid", ex.Message);
        }
    }
    #endregion
}
