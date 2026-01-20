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
/// Summary description for CategoryMaster
/// </summary>
public class ItemCategoryMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _I_CAT_CODE;
    private int _I_CAT_CM_COMP_ID;
    private string _I_CAT_NAME;
    private bool _ES_DELETE;
    private bool _MODIFY;
    private bool _I_CAT_SHORTCLOSE;

    
    #endregion

    #region "Constructor"
    public ItemCategoryMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    #region Parameterise Constructor
    public ItemCategoryMaster_BL(int Id)
    {
        _I_CAT_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int I_CAT_CODE
    {
        get { return _I_CAT_CODE; }
        set { _I_CAT_CODE = value; }
    }
    public int I_CAT_CM_COMP_ID
    {
        get { return _I_CAT_CM_COMP_ID; }
        set { _I_CAT_CM_COMP_ID = value; }
    }
    public string I_CAT_NAME
    {
        get { return _I_CAT_NAME; }
        set { _I_CAT_NAME = value; }
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
    public bool I_CAT_SHORTCLOSE
    {
        get { return _I_CAT_SHORTCLOSE; }
        set { _I_CAT_SHORTCLOSE = value; }
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
				new SqlParameter("@I_CAT_CODE",SqlDbType.Int),
				new SqlParameter("@I_CAT_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@I_CAT_NAME",SqlDbType.VarChar),
				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit),
                new SqlParameter("@I_CAT_SHORTCLOSE",SqlDbType.Bit),
                new SqlParameter("@TYPE",SqlDbType.VarChar)
			};


            if (I_CAT_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = I_CAT_CODE;
                
            }

            if (I_CAT_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = I_CAT_CM_COMP_ID;
                
            }

            if (I_CAT_NAME != null)
            {
                Params[2].Value = I_CAT_NAME;
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
            if (I_CAT_SHORTCLOSE != null)
            {
                Params[5].Value = I_CAT_SHORTCLOSE;
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

            dt = DL_DBAccess.SelectData("SP_ITEM_CATEGORY_MASTER_Select", Params);

            return dt;
        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("Item Category Master Class", "GetRecords", ex.Message);
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
            I_CAT_CODE = Convert.ToInt32(dt.Rows[0]["I_CAT_CODE"]);
            I_CAT_NAME = dt.Rows[0]["I_CAT_NAME"].ToString();
            I_CAT_SHORTCLOSE =Convert.ToBoolean(dt.Rows[0]["I_CAT_SHORTCLOSE"].ToString());
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
				    
				    new SqlParameter("@I_CAT_CM_COMP_ID",I_CAT_CM_COMP_ID),
				    new SqlParameter("@I_CAT_NAME",I_CAT_NAME),
				    new SqlParameter("@ES_DELETE",ES_DELETE),
				    new SqlParameter("@MODIFY",MODIFY) ,
                     new SqlParameter("@I_CAT_SHORTCLOSE",I_CAT_SHORTCLOSE) 
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_CATEGORY_MASTER_Insert", Params);
            }
            else
            {
                Msg = "Item Category Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Class", "Save", Ex.Message);
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
            if (CommonClasses.Execute("SELECT I_CAT_CODE,I_CAT_CM_COMP_ID,I_CAT_NAME FROM ITEM_CATEGORY_MASTER where I_CAT_NAME='" + I_CAT_NAME + "' and I_CAT_CODE!=" + I_CAT_CODE + "").Rows.Count == 0)
            {
                //SqlParameter[] Params = 
                //{ 
                //    new SqlParameter("@I_CAT_CODE",I_CAT_CODE),
                //    new SqlParameter("@I_CAT_CM_COMP_ID",I_CAT_CM_COMP_ID),
                //    new SqlParameter("@I_CAT_NAME",I_CAT_NAME),
                //    new SqlParameter("@ES_DELETE",ES_DELETE),
                //    new SqlParameter("@MODIFY",MODIFY) 
                //};
                //result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_CATEGORY_MASTER_Update", Params);

                result = CommonClasses.Execute1("update ITEM_CATEGORY_MASTER set I_CAT_NAME='" + I_CAT_NAME + "' ,I_CAT_SHORTCLOSE='" + I_CAT_SHORTCLOSE + "'  where I_CAT_CODE='" + I_CAT_CODE + "'");
            }
            else
            {
                Msg = "Item Category Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Category Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@I_CAT_CODE", I_CAT_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_ITEM_CATEGORY_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Category Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion
    
    
    #endregion



}