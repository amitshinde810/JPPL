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
/// Summary description for CityMaster_BL
/// </summary>
public class CityMaster_BL
{
    #region Data Members
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _CITY_CODE;
    private int _CITY_CM_COMP_ID;
    private string _CITY_NAME;
    private int _CITY_COUNTRY_CODE;
    private int _CITY_SM_CODE;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

     #region Constructor
    public CityMaster_BL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public CityMaster_BL(int Id)
    {
        _CITY_CODE = Id;
    } 
    #endregion

    #region Public Properties
    public int CITY_CODE
    {
        get { return _CITY_CODE; }
        set { _CITY_CODE = value; }
    }
    public int CITY_CM_COMP_ID
    {
        get { return _CITY_CM_COMP_ID; }
        set { _CITY_CM_COMP_ID = value; }
    }
    public string CITY_NAME
    {
        get { return _CITY_NAME; }
        set { _CITY_NAME = value; }
    }
    public int CITY_COUNTRY_CODE
    {
        get { return _CITY_COUNTRY_CODE; }
        set { _CITY_COUNTRY_CODE = value; }
    }
    public int CITY_SM_CODE
    {
        get { return _CITY_SM_CODE; }
        set { _CITY_SM_CODE = value; }
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
				new SqlParameter("@CITY_CODE",SqlDbType.Int),
				new SqlParameter("@CITY_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@CITY_NAME",SqlDbType.VarChar),
				new SqlParameter("@CITY_COUNTRY_CODE",SqlDbType.Int),
               	new SqlParameter("@CITY_SM_CODE",SqlDbType.Int),
		        new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit), 
                new SqlParameter("@TYPE",SqlDbType.VarChar)
		
			};


            if (CITY_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = CITY_CODE;
            }

            if (CITY_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = CITY_CM_COMP_ID;
            }

            if (CITY_NAME != null)
            {
                Params[2].Value = CITY_NAME;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (CITY_COUNTRY_CODE != null)
            {
                Params[3].Value = CITY_COUNTRY_CODE;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }
            if (CITY_SM_CODE != null)
            {
                Params[4].Value = CITY_SM_CODE;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }


            if (ES_DELETE != null)
            {
                Params[5].Value = ES_DELETE;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[6].Value = MODIFY;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }
            if (Type != null)
            {
                Params[7].Value = Type;
            }
            else
            {
                Params[7].Value = DBNull.Value;
            }
            dt = DL_DBAccess.SelectData("SP_CITY_MASTER_SELECT2", Params);

            return dt;

        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("City Master Class", "GetInfo", ex.Message);
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
            CITY_CODE = Convert.ToInt32(dt.Rows[0]["CITY_CODE"]);
            CITY_COUNTRY_CODE = Convert.ToInt32(dt.Rows[0]["CITY_COUNTRY_CODE"]);
            CITY_SM_CODE = Convert.ToInt32(dt.Rows[0]["CITY_SM_CODE"]);
            CITY_NAME = dt.Rows[0]["CITY_NAME"].ToString();
           
        
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
       // dt = GetRecords("CHECKSAVE");
        dt = GetRecords("CHECKUPDATE");

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
				    new SqlParameter("@CITY_CM_COMP_ID",CITY_CM_COMP_ID),
				    new SqlParameter("@CITY_COUNTRY_CODE",CITY_COUNTRY_CODE),
                    new SqlParameter("@CITY_SM_CODE",CITY_SM_CODE),
			        new SqlParameter("@CITY_NAME",CITY_NAME)
                  
                
                };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CITY_MASTER_INSERT", Params);
            }
            else
            {
                Msg = "City Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master Class", "Save", Ex.Message);
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
				    new SqlParameter("@CITY_CODE",CITY_CODE),
				    new SqlParameter("@CITY_CM_COMP_ID",CITY_CM_COMP_ID),
				    new SqlParameter("@CITY_COUNTRY_CODE",CITY_COUNTRY_CODE),
                    new SqlParameter("@CITY_SM_CODE",CITY_SM_CODE),
                    new SqlParameter("@CITY_NAME",CITY_NAME)
                    
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CITY_MASTER_UPDATE", Params);
            }
            else
            {
                Msg = "City Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("City Master Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@CITY_CODE", CITY_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CITY_MASTER_DELETE", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("City Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion



	
}
