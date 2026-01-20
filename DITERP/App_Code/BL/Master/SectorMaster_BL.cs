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
/// Summary description for SectorMaster_BL
/// </summary>
public class SectorMaster_BL
{
	#region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _SCT_CODE;
    private int _SCT_CM_COMP_ID;
    private string _SCT_DESC;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    #region "Constructor"
    public SectorMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public SectorMaster_BL(int Id)
    {
        _SCT_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int SCT_CODE
    {
        get { return _SCT_CODE; }
        set { _SCT_CODE = value; }
    }
    public int SCT_CM_COMP_ID
    {
        get { return _SCT_CM_COMP_ID; }
        set { _SCT_CM_COMP_ID = value; }
    }
    public string SCT_DESC
    {
        get { return _SCT_DESC; }
        set { _SCT_DESC = value; }
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

    #region Public Methods
    
    #region GetRecords
    public DataTable GetRecords(string Type)
    {
        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            SqlParameter[] Params = 
			{ 
				new SqlParameter("@SCT_CODE",SqlDbType.Int),
				new SqlParameter("@SCT_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@SCT_DESC",SqlDbType.VarChar),
				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit),
                new SqlParameter("@TYPE",SqlDbType.VarChar)
			};


            if (SCT_CODE == 0 )
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = SCT_CODE;
            }

            if (SCT_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = SCT_CM_COMP_ID;
            }

            if (SCT_DESC != null)
            {
                Params[2].Value = SCT_DESC;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }

           
            if (ES_DELETE != null)
            {
                Params[3].Value = ES_DELETE;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[4].Value = MODIFY;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }

            if (Type != null)
            {
                Params[5].Value = Type;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }

            dt = DL_DBAccess.SelectData("SP_SECTOR_MASTER_Select", Params);

            return dt;

        }
        catch (Exception ex)
        {
            
            //throw new Exception(ex.Message);
            CommonClasses.SendError("Secor Master Class", "GetInfo", ex.Message);
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
            SCT_CODE = Convert.ToInt32(dt.Rows[0]["SCT_CODE"]);
            SCT_DESC = dt.Rows[0]["SCT_DESC"].ToString();
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
				    new SqlParameter("@SCT_CM_COMP_ID",SCT_CM_COMP_ID),
				    new SqlParameter("@SCT_DESC",SCT_DESC),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_SECTOR_MASTER_Insert", Params);
            }
            else
            {
                Msg = "Sector Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sector Master Class", "Save", Ex.Message);
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
				    new SqlParameter("@SCT_CODE",SCT_CODE),
				    new SqlParameter("@SCT_CM_COMP_ID",SCT_CM_COMP_ID),
				    new SqlParameter("@SCT_DESC",SCT_DESC),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 

			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_SECTOR_MASTER_Update", Params);
            }
            else
            {
                Msg = "Sector Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sector Master Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@SCT_CODE", SCT_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_SECTOR_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sector Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion


    #endregion
}
