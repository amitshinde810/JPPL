using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BOM_Master_BL
/// </summary>
public class BOM_Master_BL
{
    public BOM_Master_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

     #region Parameterise Constructor
    public BOM_Master_BL(int Id)
    {
        CPBM_CODE = Id;
    }
    #endregion
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    private int _CPBM_CODE;
    private int? _CPBM_CM_COMP_ID;
    private int? _CPBM_I_CODE;
    private int? _CPBM_FINISH_I_CODE;
    private bool? _ES_DELETE;
    private bool? _MODIFY;
    static DataTable dt2 = new DataTable();
    #endregion

    #region Private Variables
    public int CPBM_CODE
    {
        get { return _CPBM_CODE; }
        set { _CPBM_CODE = value; }
    }
    public int? CPBM_CM_COMP_ID
    {
        get { return _CPBM_CM_COMP_ID; }
        set { _CPBM_CM_COMP_ID = value; }
    }
    public int? CPBM_I_CODE
    {
        get { return _CPBM_I_CODE; }
        set { _CPBM_I_CODE = value; }
    }
    public int? CPBM_FINISH_I_CODE
    {
        get { return _CPBM_FINISH_I_CODE; }
        set { _CPBM_FINISH_I_CODE = value; }
    }
    public bool? ES_DELETE
    {
        get { return _ES_DELETE; }
        set { _ES_DELETE = value; }
    }
    public bool? MODIFY
    {
        get { return _MODIFY; }
        set { _MODIFY = value; }
    }
    public string message = "";
    public string Msg = "";
    public int PK_CODE;
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = {
            new SqlParameter("@PROCESS", "GetInfo"), 
            new SqlParameter("@CPBM_CODE", CPBM_CODE), 
        };
            dt = DL_DBAccess.SelectData("CP_BOM_MASTER_CURD", par, out Msg, out PK_CODE);
            if (dt.Rows.Count > 0)
            {
                CPBM_CODE = Convert.ToInt32(dt.Rows[0]["CPBM_CODE"]);
                CPBM_I_CODE = Convert.ToInt32(dt.Rows[0]["CPBM_I_CODE"].ToString());
                CPBM_FINISH_I_CODE = Convert.ToInt32(dt.Rows[0]["CPBM_FINISH_I_CODE"].ToString());
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region FillGrid
    public void FillGrid(GridView XGrid)
    {

        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            dt = CommonClasses.Execute("SELECT CP_BOM_MASTER.CPBM_CODE,ITEM_MASTER.I_NAME,ITEM_MASTER.I_CODENO,ITEM_MASTER_1.I_CODENO AS FINISH_CODE,ITEM_MASTER_1.I_NAME AS FINISH_NAME FROM ITEM_MASTER INNER JOIN CP_BOM_MASTER ON ITEM_MASTER.I_CODE = CP_BOM_MASTER.CPBM_I_CODE INNER JOIN ITEM_MASTER AS ITEM_MASTER_1 ON CP_BOM_MASTER.CPBM_FINISH_I_CODE = ITEM_MASTER_1.I_CODE WHERE (ITEM_MASTER.ES_DELETE = 0) AND (CP_BOM_MASTER.ES_DELETE = 0) AND (ITEM_MASTER.I_ACTIVE_IND=1) AND (ITEM_MASTER_1.I_ACTIVE_IND=1)");
            XGrid.DataSource = dt;
            XGrid.DataBind();
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region FillGrid
    public DataTable FillGrid(string str)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = {
            new SqlParameter("@PROCESS", "FillGrid"), 
            new SqlParameter("@str", str)
        };
            dt = DL_DBAccess.SelectData("CP_BOM_MASTER_CURD", par, out Msg, out PK_CODE);
            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    #endregion

    #region Save
    public bool Save(string Process)
    {

        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
        }
        catch (Exception ex)
        { }
        try
        {
            SqlParameter[] Params = 
			{ 
                
                new SqlParameter("@PROCESS", Process),
				new SqlParameter("@CPBM_CODE", CPBM_CODE),
				new SqlParameter("@CPBM_CM_COMP_ID",CPBM_CM_COMP_ID),
				new SqlParameter("@CPBM_I_CODE",CPBM_I_CODE),
				new SqlParameter("@CPBM_FINISH_I_CODE",CPBM_FINISH_I_CODE)
				
			};

            result = DL_DBAccess.Insertion_Updation_Delete("CP_BOM_MASTER_CURD", Params, out message, out PK_CODE);

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "Save/Update", Ex.Message);

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
            //Update Master Table Flag
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] par = new SqlParameter[5];
            par[0] = new SqlParameter("@PK_CODE", CPBM_CODE);
            par[1] = new SqlParameter("@PK_Field", "CPBM_CODE");
            par[2] = new SqlParameter("@ES_DELETE", "1");
            par[3] = new SqlParameter("@DELETE", "ES_DELETE");
            par[4] = new SqlParameter("@TABLE_NAME", "CP_BOM_MASTER");
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_DELETE", par);

            return result;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Bill Passing", "Delete", Ex.Message);

            return false;
        }
        finally
        { }
    }
    #endregion
}
