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
/// Summary description for AdminIsoMaster
/// </summary>
public class AdminIsoMaster
{
	public AdminIsoMaster()
	{
		
	}

    #region Data Members
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _ISO_CODE;
    private int _ISO_COMP_ID;
    private string _ISO_SCREEN_NO;
    private int _ISO_WEF_DATE;
    private string _ISO_NO;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    #region Constructor

    public AdminIsoMaster(int Id)
    {
        _ISO_CODE = Id;
    } 
    #endregion

    #region Public Properties
    public int ISO_CODE
    {
        get { return _ISO_CODE; }
        set { _ISO_CODE = value; }
    }
    public int ISO_COMP_ID
    {
        get { return _ISO_COMP_ID; }
        set { _ISO_COMP_ID = value; }
    }
    public string ISO_SCREEN_NO
    {
        get { return _ISO_SCREEN_NO; }
        set { _ISO_SCREEN_NO = value; }
    }
    public int ISO_WEF_DATE
    {
        get { return _ISO_WEF_DATE; }
        set { _ISO_WEF_DATE = value; }
    }

    public string ISO_NO
    {
        get { return _ISO_NO; }
        set { _ISO_NO = value; }
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
				new SqlParameter("@ISO_CODE",SqlDbType.Int),
				new SqlParameter("@ISO_COMP_ID",SqlDbType.Int),
				new SqlParameter("@ISO_SCREEN_NO",SqlDbType.VarChar),
				new SqlParameter("@ISO_WEF_DATE",SqlDbType.Int),
                new SqlParameter("@ISO_NO",SqlDbType.VarChar),
		        new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit), 
                new SqlParameter("@TYPE",SqlDbType.VarChar)
		
			};


            if (ISO_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = ISO_CODE;
            }

            if (ISO_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = ISO_COMP_ID;
            }

            if (ISO_SCREEN_NO != null)
            {
                Params[2].Value = ISO_SCREEN_NO;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }
            if (ISO_WEF_DATE != null)
            {
                Params[3].Value = ISO_WEF_DATE;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }

            if (ISO_NO != null)
            {
                Params[3].Value = ISO_NO;
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
            ISO_CODE = Convert.ToInt32(dt.Rows[0]["ISO_CODE"]);
            ISO_SCREEN_NO = dt.Rows[0]["ISO_SCREEN_NO"].ToString();
            ISO_WEF_DATE = Convert.ToInt32(dt.Rows[0]["ISO_WEF_DATE"]);
        
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
				    new SqlParameter("@ISO_COMP_ID",ISO_COMP_ID),
				    new SqlParameter("@ISO_SCREEN_NO",ISO_SCREEN_NO),
                    new SqlParameter("@ISO_WEF_DATE",ISO_WEF_DATE)
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
				    new SqlParameter("@ISO_CODE",ISO_CODE),
				    new SqlParameter("@ISO_COMP_ID",ISO_COMP_ID),
				    new SqlParameter("@ISO_SCREEN_NO",ISO_SCREEN_NO),
                    new SqlParameter("@ISO_WEF_DATE",ISO_WEF_DATE)
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
            SqlParameter[] Params = { new SqlParameter("@ISO_CODE", ISO_CODE) };
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
