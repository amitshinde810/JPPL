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
using System.Net;
/// <summary>
/// Summary description for UserMaster_BL
/// </summary>
public class UserMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region "Variables"
    int _UM_CODE;
    int _UM_CM_ID;
    int _UM_BM_CODE;
    long _UM_EM_CODE;
    string _UM_USERNAME;
    string _UM_EMAIL;
    string _UM_PASSWORD;
    string _UM_LEVEL;
    DateTime _UM_LASTLOGIN_DATETIME;
    string _UM_IP_ADDRESS;
    bool _UM_ACTIVE_IND;
    bool _UM_IS_ADMIN;
    #endregion

    #region "Properties"

    public int UM_CODE
    {
        get { return _UM_CODE; }
        set { _UM_CODE = value; }
    }

    public int UM_BM_CODE
    {
        get { return _UM_BM_CODE; }
        set { _UM_BM_CODE = value; }
    }

    public long UM_EM_CODE
    {
        get { return _UM_EM_CODE; }
        set { _UM_EM_CODE = value; }
    }

    public int UM_CM_ID
    {
        get { return _UM_CM_ID; }
        set { _UM_CM_ID = value; }
    }

  
  
    public string UM_USERNAME
    {
        get
        {
            return _UM_USERNAME;
        }
        set
        {
            _UM_USERNAME = value;
        }
    }

    public string UM_EMAIL
    {
        get
        {
            return _UM_EMAIL;
        }
        set
        {
            _UM_EMAIL = value;
        }
    }


    public string UM_PASSWORD
    {
        get { return _UM_PASSWORD; }
        set { _UM_PASSWORD = value; }
    }

    public string UM_LEVEL
    {
        get { return _UM_LEVEL; }
        set { _UM_LEVEL = value; }
    }

    public DateTime UM_LASTLOGIN_DATETIME
    {
        get { return _UM_LASTLOGIN_DATETIME; }
        set { _UM_LASTLOGIN_DATETIME = value; }
    }

    public string UM_IP_ADDRESS
    {
        get { return _UM_IP_ADDRESS; }
        set { _UM_IP_ADDRESS = value; }
    }

    public bool UM_ACTIVE_IND
    {
        get { return _UM_ACTIVE_IND; }
        set { _UM_ACTIVE_IND = value; }
    }

    public bool UM_IS_ADMIN
    {
        get { return _UM_IS_ADMIN; }
        set { _UM_IS_ADMIN = value; }
    }

    public string Msg = "";


    #endregion

    #region "Constructor"
    public UserMaster_BL()
    {
    }
    #endregion "Constructor"

    #region Parameterise Constructor
    public UserMaster_BL(int Id)
    {
        _UM_CODE = Id;
    }
    #endregion

    #region UserDefineMethods

    #region FillCombo
    public DataTable GetDepartment(string Query)
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
            CommonClasses.SendError("User Master Class", "GetDepartment", ex.Message);
        }
        return dt;
    }
    #endregion FillCombo

    #region Save
    public bool Save(CheckBoxList chkLst, CheckBoxList CheckBoxListNoti)
    {
        bool result = false;
        try
        {
            if (CheckExistSaveName())
            {
                DL_DBAccess = new DatabaseAccessLayer();
                SqlParameter[] par = new SqlParameter[10];
                par[0] = new SqlParameter("@UM_CM_ID", UM_CM_ID);
                par[1] = new SqlParameter("@UM_BM_CODE", UM_BM_CODE);
                par[2] = new SqlParameter("@UM_EM_CODE", UM_EM_CODE);
                par[3] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
                par[4] = new SqlParameter("@UM_PASSWORD", UM_PASSWORD);
                par[5] = new SqlParameter("@UM_LEVEL", UM_LEVEL);
                par[6] = new SqlParameter("@UM_LASTLOGIN_DATETIME", UM_LASTLOGIN_DATETIME);
                par[7] = new SqlParameter("@UM_IP_ADDRESS", UM_IP_ADDRESS);
                par[8] = new SqlParameter("@UM_ACTIVE_IND", UM_ACTIVE_IND);
                par[9] = new SqlParameter("@UM_IS_ADMIN", UM_IS_ADMIN);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_InsertUser", par);
                if (result == true)
                {
                    DataTable dt = CommonClasses.Execute("SELECT Max(UM_CODE) FROM CM_USER_MASTER");
                    foreach (ListItem li in chkLst.Items)
                    {
                        if (li.Selected)
                        {
                            DL_DBAccess = new DatabaseAccessLayer();
                            SqlParameter[] par1 = new SqlParameter[2];
                            par1[0] = new SqlParameter("@UMD_UM_CODE", dt.Rows[0][0]);
                            par1[1] = new SqlParameter("@UMD_BM_CODE", li.Value.ToString());
                            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_UserDetail", par1);
                        }
                    }
                    foreach (ListItem li in CheckBoxListNoti.Items)
                    {
                        if (li.Selected)
                        {
                            DL_DBAccess = new DatabaseAccessLayer();
                            SqlParameter[] par1 = new SqlParameter[2];
                            par1[0] = new SqlParameter("@UND_UM_CODE", dt.Rows[0][0]);
                            par1[1] = new SqlParameter("@UND_NM_CODE", li.Value.ToString());
                            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_USERNOTIFICATION", par1);
                        }
                    }
                }
            }
            else
            {
                Msg = "User Name Already Exist";
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master Class", "Save", ex.Message);
        }
        return result;
    }
    #endregion Save

    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {
        bool res = false;
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[3];
            par[0] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
            par[1] = new SqlParameter("@UM_CM_ID", UM_CM_ID);
            par[2] = new SqlParameter("@UM_BM_CODE", UM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_CM_CheckSaveUser", par);
            if (dt.Rows.Count > 0)
                res = false;
            else
                res = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Class", "CheckExistSaveName", Ex.Message);
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
            par[0] = new SqlParameter("@UM_CODE", UM_CODE);
            par[1] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
            par[2] = new SqlParameter("@UM_CM_ID", UM_CM_ID);
            par[3] = new SqlParameter("@UM_BM_CODE", UM_BM_CODE);
            dt = DL_DBAccess.SelectData("SP_CM_CheckUpdateUser", par);
            if (dt.Rows.Count > 0)
                res = false;
            else
                res = true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Class", "ChkExstUpdateName", Ex.Message);
        }
        return res;
    }
    #endregion

    #region Update
    public bool Update(CheckBoxList chkLst, CheckBoxList CheckBoxListNoti)
    {
        bool result = false;
        try
        {
            if (ChkExstUpdateName())
            {
                DL_DBAccess = new DatabaseAccessLayer();
                SqlParameter[] par = new SqlParameter[10];
                par[0] = new SqlParameter("@UM_CODE", UM_CODE);
                par[1] = new SqlParameter("@UM_BM_CODE", UM_BM_CODE);
                par[2] = new SqlParameter("@UM_EM_CODE", UM_EM_CODE);
                par[3] = new SqlParameter("@UM_USERNAME", UM_USERNAME);
                par[4] = new SqlParameter("@UM_PASSWORD", UM_PASSWORD);
                par[5] = new SqlParameter("@UM_LEVEL", UM_LEVEL);
                par[6] = new SqlParameter("@UM_LASTLOGIN_DATETIME", UM_LASTLOGIN_DATETIME);
                par[7] = new SqlParameter("@UM_IP_ADDRESS", UM_IP_ADDRESS);
                par[8] = new SqlParameter("@UM_ACTIVE_IND", UM_ACTIVE_IND);
                par[9] = new SqlParameter("@UM_IS_ADMIN", UM_IS_ADMIN);
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_UpdateUser", par);
                if (result == true)
                {
                    CommonClasses.Execute("DELETE FROM CM_USER_MASTER_DETAIL WHERE UMD_UM_CODE=" + UM_CODE + "");
                    CommonClasses.Execute("DELETE FROM CM_USER_MASTER_NOTIFICATION WHERE UND_UM_CODE=" + UM_CODE + "");
                    foreach (ListItem li in chkLst.Items)
                    {
                        if (li.Selected)
                        {
                            DL_DBAccess = new DatabaseAccessLayer();
                            SqlParameter[] par1 = new SqlParameter[2];
                            par1[0] = new SqlParameter("@UMD_UM_CODE", UM_CODE);
                            par1[1] = new SqlParameter("@UMD_BM_CODE", li.Value.ToString());
                            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_UserDetail", par1);
                        }
                    }
                    foreach (ListItem li in CheckBoxListNoti.Items)
                    {
                        if (li.Selected)
                        {
                            DL_DBAccess = new DatabaseAccessLayer();
                            SqlParameter[] par1 = new SqlParameter[2];
                            par1[0] = new SqlParameter("@UND_UM_CODE", UM_CODE);
                            par1[1] = new SqlParameter("@UND_NM_CODE", li.Value.ToString());
                            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_INSERT_USERNOTIFICATION", par1);
                        }
                    }
                }
            }
            else
            {
                Msg = "User Name Already Exist";
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("User Master Class", "Update", Ex.Message);
        }
        return result;
    }
    #endregion
        
    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@UM_CM_ID", UM_CM_ID);
            dt = DL_DBAccess.SelectData("SP_CM_FILLUser", par);
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master Class", "FillGrid", ex.Message);
        }
    }
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[1];
            par[0] = new SqlParameter("@UM_CODE", UM_CODE);
            dt = DL_DBAccess.SelectData("SP_CM_GetInfoUser", par);
            if (dt.Rows.Count > 0)
            {
                UM_CODE = Convert.ToInt32(dt.Rows[0]["UM_CODE"]);
                UM_BM_CODE = Convert.ToInt32(dt.Rows[0]["UM_BM_CODE"]);
                UM_EM_CODE = Convert.ToInt64(dt.Rows[0]["UM_EM_CODE"]);
                UM_USERNAME = dt.Rows[0]["UM_USERNAME"].ToString();
                UM_PASSWORD = dt.Rows[0]["UM_PASSWORD"].ToString();
                UM_LEVEL = dt.Rows[0]["UM_LEVEL"].ToString();
                UM_ACTIVE_IND = Convert.ToBoolean(dt.Rows[0]["UM_ACTIVE_IND"].ToString());
                UM_IS_ADMIN = Convert.ToBoolean(dt.Rows[0]["UM_IS_ADMIN"].ToString());
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master Class", "GetInfo", ex.Message);
        }

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
            par[0] = new SqlParameter("@UM_CODE", UM_CODE);
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DeleteUser", par);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("User Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion

    #region FillCombo
    public DataTable FillCombo(string Query)
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
            CommonClasses.SendError("User Master Class", "FillCombo", ex.Message);
        }
        return dt;
    }
    #endregion FillCombo

    #endregion Methods


}
