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
/// Summary description for UnitMaster_BL
/// </summary>
public class UnitMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _I_UOM_CODE;
    private int _I_UOM_CM_COMP_ID;
    private string _I_UOM_NAME;
    private string _I_UOM_DESC;
    private bool _ES_DELETE;
    private bool _MODIFY;
    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    #region "Constructor"
    public UnitMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public UnitMaster_BL(int Id)
    {
        _I_UOM_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int I_UOM_CODE
    {
        get { return _I_UOM_CODE; }
        set { _I_UOM_CODE = value; }
    }
    public int I_UOM_CM_COMP_ID
    {
        get { return _I_UOM_CM_COMP_ID; }
        set { _I_UOM_CM_COMP_ID = value; }
    }
    public string I_UOM_NAME
    {
        get { return _I_UOM_NAME; }
        set { _I_UOM_NAME = value; }
    }
    public string I_UOM_DESC
    {
        get { return _I_UOM_DESC; }
        set { _I_UOM_DESC = value; }
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
				new SqlParameter("@I_UOM_CODE",SqlDbType.Int),
				new SqlParameter("@I_UOM_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@I_UOM_NAME",SqlDbType.VarChar),
				new SqlParameter("@I_UOM_DESC",SqlDbType.VarChar),
				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit),
                new SqlParameter("@TYPE",SqlDbType.VarChar)
		
			};


            if (I_UOM_CODE == 0 )
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = I_UOM_CODE;
            }

            if (I_UOM_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = I_UOM_CM_COMP_ID;
            }

            if (I_UOM_NAME != null)
            {
                Params[2].Value = I_UOM_NAME;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }

            if (I_UOM_DESC != null)
            {
                Params[3].Value = I_UOM_DESC;
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
            dt = DL_DBAccess.SelectData("SP_ITEM_UNIT_MASTER_Select", Params);

            return dt;

        }
        catch (Exception ex)
        {
            
            //throw new Exception(ex.Message);
            CommonClasses.SendError("Unit Master Class", "GetInfo", ex.Message);
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
            I_UOM_CODE = Convert.ToInt32(dt.Rows[0]["I_UOM_CODE"]);
            I_UOM_NAME = dt.Rows[0]["I_UOM_NAME"].ToString();
            I_UOM_DESC = dt.Rows[0]["I_UOM_DESC"].ToString();
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
				    new SqlParameter("@I_UOM_CM_COMP_ID",I_UOM_CM_COMP_ID),
				    new SqlParameter("@I_UOM_NAME",I_UOM_NAME),
				    new SqlParameter("@I_UOM_DESC",I_UOM_DESC),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_UNIT_MASTER_Insert", Params);
            }
            else
            {
                Msg = "Unit Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master Class", "Save", Ex.Message);
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
            //if (ChkExstUpdateName())
            //{
            DL_DBAccess = new DatabaseAccessLayer();
            if (CommonClasses.Execute("SELECT I_UOM_CODE,I_UOM_NAME,I_UOM_DESC FROM ITEM_UNIT_MASTER where I_UOM_NAME='" + I_UOM_NAME + "'  and I_UOM_CODE!=" + I_UOM_CODE + "").Rows.Count == 0)
            {
                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@I_UOM_CODE",I_UOM_CODE),
				    new SqlParameter("@I_UOM_CM_COMP_ID",I_UOM_CM_COMP_ID),
				    new SqlParameter("@I_UOM_NAME",I_UOM_NAME),
				    new SqlParameter("@I_UOM_DESC",I_UOM_DESC),
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_UNIT_MASTER_Update", Params);
            }
            else
            {
                Msg = "Unit Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Unit Master Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@I_UOM_CODE", I_UOM_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_UNIT_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Unit Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion


    #endregion
}
