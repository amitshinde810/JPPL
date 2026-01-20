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
/// Summary description for StateMaster_BL
/// </summary>
public class StateMaster_BL
{
    #region Data Members
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _SM_CODE;
    private int _SM_CM_COMP_ID;
    private string _SM_NAME;
    private string _SM_STATE_CODE;

    public string SM_STATE_CODE
    {
        get { return _SM_STATE_CODE; }
        set { _SM_STATE_CODE = value; }
    }
    private int _SM_COUNTRY_CODE;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    #region Constructor
    public StateMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public StateMaster_BL(int Id)
    {
        _SM_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int SM_CODE
    {
        get { return _SM_CODE; }
        set { _SM_CODE = value; }
    }
    public int SM_CM_COMP_ID
    {
        get { return _SM_CM_COMP_ID; }
        set { _SM_CM_COMP_ID = value; }
    }
    public string SM_NAME
    {
        get { return _SM_NAME; }
        set { _SM_NAME = value; }
    }
    public int SM_COUNTRY_CODE
    {
        get { return _SM_COUNTRY_CODE; }
        set { _SM_COUNTRY_CODE = value; }
    }


    public bool ES_DELETE
    {
        get { return _ES_DELETE; }
        set { _ES_DELETE = value; }
    }
    public bool MODIFY
    {
        get { return _MODIFY; }
        set { _MODIFY = value; }
    }
    #endregion

    #region GetRecords
    public DataTable GetRecords(string Type)
    {
        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@SM_CODE",SqlDbType.Int),
				new SqlParameter("@SM_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@SM_NAME",SqlDbType.VarChar),
				new SqlParameter("@SM_COUNTRY_CODE",SqlDbType.Int),
		        new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit), 
                new SqlParameter("@TYPE",SqlDbType.VarChar)
		
			};


            if (SM_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = SM_CODE;
            }

            if (SM_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = SM_CM_COMP_ID;
            }

            if (SM_NAME != null)
            {
                Params[2].Value = SM_NAME;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (SM_COUNTRY_CODE != null)
            {
                Params[3].Value = SM_COUNTRY_CODE;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }


            if (ES_DELETE != null)
            {
                Params[4].Value = ES_DELETE;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[5].Value = MODIFY;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }
            if (Type != null)
            {
                Params[6].Value = Type;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }
            dt = DL_DBAccess.SelectData("SP_STATE_MASTER_SELECT", Params);

            return dt;

        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("State Master Class", "GetInfo", ex.Message);
            return dt;
        }

    }
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DataTable dt = new DataTable();
        dt = GetRecords("GETINFO");

        if (dt.Rows.Count > 0)
        {
            SM_CODE = Convert.ToInt32(dt.Rows[0]["SM_CODE"]);
            SM_NAME = dt.Rows[0]["SM_NAME"].ToString();
            SM_COUNTRY_CODE = Convert.ToInt32(dt.Rows[0]["SM_COUNTRY_CODE"]);
            SM_STATE_CODE = dt.Rows[0]["SM_STATE_CODE"].ToString();
        }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DataTable dt = new DataTable();
        dt = GetRecords("FILLGRID");

        XGrid.DataSource = dt;
        XGrid.DataBind();
    }
    #endregion

    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {

        bool res = false;
        DataTable dt = new DataTable();
        dt = GetRecords("CHECKSAVE");

        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion

    #region ChkExstUpdateName
    public bool ChkExstUpdateName()
    {
        bool res = false;
        DataTable dt = new DataTable();
        dt = GetRecords("CHECKUPDATE");
        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

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

                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@SM_CM_COMP_ID",SM_CM_COMP_ID),
				    new SqlParameter("@SM_NAME",SM_NAME),
                    new SqlParameter("@SM_COUNTRY_CODE",SM_COUNTRY_CODE),
                     new SqlParameter("@SM_STATE_CODE",SM_STATE_CODE)
                };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_STATE_MASTER_INSERT", Params);
            }
            else
            {
                Msg = "State Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master Class", "Save", Ex.Message);
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
                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@SM_CODE",SM_CODE),
				    new SqlParameter("@SM_CM_COMP_ID",SM_CM_COMP_ID),
				    new SqlParameter("@SM_NAME",SM_NAME),
                    new SqlParameter("@SM_COUNTRY_CODE",SM_COUNTRY_CODE),
                      new SqlParameter("@SM_STATE_CODE",SM_STATE_CODE)
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_STATE_MASTER_UPDATE", Params);
            }
            else
            {
                Msg = "State Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("State Master Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@SM_CODE", SM_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_STATE_MASTER_DELETE", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("State Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion


}
