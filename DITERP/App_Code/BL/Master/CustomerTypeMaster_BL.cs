using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for CustomerTypeMaster_BL
/// </summary>
public class CustomerTypeMaster_BL
{
	 #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion


    #region Private Variables
    private int _CTM_CODE;
    private int _CTM_CM_COMP_ID;
    private string _CTM_TYPE_CODE;
    private string _CTM_TYPE_DESC;
    private string _CTM_FIRST_LETTER;
    private bool _ES_DELETE;
    private bool _MODIFY;
    #endregion


    #region Public Properties

    public int CTM_CODE
    {
        get { return _CTM_CODE; }
        set { _CTM_CODE = value; }
    }

    public int CTM_CM_COMP_ID
    {
        get { return _CTM_CM_COMP_ID; }
        set { _CTM_CM_COMP_ID = value; }
    }
    public string CTM_TYPE_CODE
    {
        get { return _CTM_TYPE_CODE; }
        set { _CTM_TYPE_CODE = value; }
    }
    public string CTM_TYPE_DESC
    {
        get { return _CTM_TYPE_DESC; }
        set { _CTM_TYPE_DESC = value; }
    }
    public string CTM_FIRST_LETTER
    {
        get { return _CTM_FIRST_LETTER; }
        set { _CTM_FIRST_LETTER = value; }
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

    public CustomerTypeMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public CustomerTypeMaster_BL(int Id)
    {
        _CTM_CODE = Id;
    }

    #region Save
    public bool Save()
    {
        bool result = false;
        DL_DBAccess = new DatabaseAccessLayer();
        try
        {
            if (CheckExistSaveName())
            {

            //    SqlParameter[] Params = 
            //    { 
				    
            //        new SqlParameter("@A_CM_COMP_ID",A_CM_COMP_ID),
            //        new SqlParameter("@A_NO",A_NO),
            //        new SqlParameter("@A_DESC",A_DESC)
                    
                   
            //    };
            //    result = DL_DBAccess.Insertion_Updation_Delete("SP_ADMIN_MASTER_INSERT", Params);

                result = CommonClasses.Execute1("Insert into CUSTOMER_TYPE_MASTER(CTM_CM_COMP_ID,CTM_TYPE_CODE,CTM_TYPE_DESC,CTM_FIRST_LETTER)values(" + CTM_CM_COMP_ID + ",'" + CTM_TYPE_CODE + "','" + CTM_TYPE_DESC + "','" + CTM_FIRST_LETTER + "')");

            }
                

            else
            {
                Msg = "Customer Description Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Class", "Save", Ex.Message);
        }
        return result;
    }
    #endregion



    #region CheckExistSaveName
    public bool CheckExistSaveName()
    {

        bool res = false;
        DataTable dt = new DataTable();
       // dt = GetRecords("CHECKUPDATE");
        dt = CommonClasses.Execute("Select CTM_CODE,CTM_CM_COMP_ID,CTM_TYPE_CODE FROM CUSTOMER_TYPE_MASTER where CTM_TYPE_DESC='" + CTM_TYPE_DESC + "' AND ES_DELETE=0");

        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion



    //#region GetRecords
    //public DataTable GetRecords(string Type)
    //{
    //    DataTable dt = new DataTable();
    //    DL_DBAccess = new DatabaseAccessLayer();

    //    try
    //    {
    //        SqlParameter[] Params = 
    //        { 
    //            new SqlParameter("@A_CODE",SqlDbType.Int),
    //            new SqlParameter("@A_U_CODE",SqlDbType.Int),
    //            new SqlParameter("@A_U_DATE",SqlDbType.Date),
    //            new SqlParameter("@A_CM_COMP_ID",SqlDbType.Int),
    //            new SqlParameter("@A_NO",SqlDbType.VarChar),
    //            new SqlParameter("@A_DESC",SqlDbType.VarChar),
    //            new SqlParameter("@ES_DELETE",SqlDbType.Bit),   
    //            new SqlParameter("@MODIFY",SqlDbType.Bit), 
    //            new SqlParameter("@TYPE",SqlDbType.VarChar)
    //        };

    //        if (A_CODE == 0)
    //        {
    //            Params[0].Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            Params[0].Value = DBNull.Value;//A_CODE;

    //        }
    //        if (A_U_CODE == 0)
    //        {
    //            Params[1].Value = DBNull.Value;

    //        }
    //        else
    //        {

    //            Params[1].Value = A_U_CODE;

    //        }

    //        if (A_U_DATE != null)
    //        {
    //            Params[2].Value = DBNull.Value;

    //        }
    //        else
    //        {
    //            Params[2].Value = A_U_DATE;


    //        }

    //        if (A_CM_COMP_ID != 0)
    //        {
    //            Params[3].Value = A_CM_COMP_ID;
    //        }
    //        else
    //        {
    //            Params[3].Value = DBNull.Value;


    //        }

    //        if (A_NO != null)
    //        {
    //            Params[4].Value = A_NO;
    //        }
    //        else
    //        {
    //            Params[4].Value = DBNull.Value;

    //        }

    //        if (A_DESC != null)
    //        {
    //            Params[5].Value = A_DESC;
    //        }
    //        else
    //        {

    //            Params[5].Value = DBNull.Value;


    //        }


    //        if (ES_DELETE != null)
    //        {
    //            Params[6].Value = ES_DELETE;
    //        }
    //        else
    //        {
    //            Params[6].Value = DBNull.Value;
    //        }

    //        if (MODIFY != null)
    //        {
    //            Params[7].Value = MODIFY;
    //        }
    //        else
    //        {
    //            Params[7].Value = DBNull.Value;
    //        }
    //        if (Type != null)
    //        {
    //            Params[8].Value = Type;
    //        }
    //        else
    //        {
    //            Params[8].Value = DBNull.Value;
    //        }



    //        dt = DL_DBAccess.SelectData("SP_AREA_MASTER_Select", Params);

    //        return dt;
    //    }
    //    catch (Exception ex)
    //    {

    //        throw new Exception(ex.Message);
    //        CommonClasses.SendError("Currency Master", "GetRecords", ex.Message);
    //        return dt;
    //    }

    //}
    //#endregion


    #region Update
    public bool Update()
    {
        bool result = false;
        try
        {
            if (ChkExstUpdateName())
            {
                //SqlParameter[] Params = 
                //{ 
				    
                //  new SqlParameter("@A_CODE",A_CODE),
                //    new SqlParameter("@A_CM_COMP_ID",A_CM_COMP_ID),
                //    new SqlParameter("@A_NO",A_NO),
                //    new SqlParameter("@A_DESC",A_DESC)

                //};
               // result = DL_DBAccess.Insertion_Updation_Delete("SP_AREA_MASTER_Update", Params);

                result = CommonClasses.Execute1("Update  CUSTOMER_TYPE_MASTER set CTM_CM_COMP_ID=" + CTM_CM_COMP_ID + ",CTM_TYPE_CODE='" + CTM_TYPE_CODE + "',CTM_TYPE_DESC='" + CTM_TYPE_DESC + "',CTM_FIRST_LETTER='" + CTM_FIRST_LETTER + "' where CTM_CODE=" + CTM_CODE + " ");

            }

            else
            {
                Msg = "Customer Description Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Type Master", "Update", Ex.Message);
        }
        return result;
    }
    #endregion

    #region ChkExstUpdateName
    public bool ChkExstUpdateName()
    {
        bool res = false;
        DataTable dt = new DataTable();
        //dt = GetRecords("CHECKUPDATE");
        dt = CommonClasses.Execute("Select * FROM CUSTOMER_TYPE_MASTER where CTM_CODE!=" + CTM_CODE + " and CTM_TYPE_CODE='" + CTM_TYPE_CODE + "' AND ES_DELETE=0");


        if (dt.Rows.Count > 0)
            res = false;
        else
            res = true;

        return res;
    }
    #endregion



    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DataTable dt = new DataTable();
       // dt = GetRecords("GETINFO");
        dt = CommonClasses.Execute("Select CTM_CODE,CTM_CM_COMP_ID,CTM_TYPE_CODE,CTM_TYPE_DESC,CTM_FIRST_LETTER,ES_DELETE,MODIFY FROM CUSTOMER_TYPE_MASTER where CTM_CM_COMP_ID='" + CTM_CM_COMP_ID + "' AND ES_DELETE=0 ORDER BY CTM_TYPE_DESC ASC");
        
        XGrid.DataSource = dt;
        XGrid.DataBind();
    }
    #endregion

    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            //SqlParameter[] Params = { new SqlParameter("@A_CODE", A_CODE) };
            //result = DL_DBAccess.Insertion_Updation_Delete("SP_AREA_MASTER_Delete", Params);
            result = CommonClasses.Execute1("Update CUSTOMER_TYPE_MASTER set ES_DELETE=1 where CTM_CODE=" + CTM_CODE + "");
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Customer Type Master", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion



    #region GetInfo
    public void GetInfo()
    {
        DataTable dt = new DataTable();
       // dt = GetRecords("GETINFO");
        dt = CommonClasses.Execute("Select CTM_CODE,CTM_CM_COMP_ID,CTM_TYPE_CODE,CTM_TYPE_DESC,CTM_FIRST_LETTER,ES_DELETE,MODIFY FROM CUSTOMER_TYPE_MASTER where CTM_CODE='" + CTM_CODE + "' AND ES_DELETE=0");

        if (dt.Rows.Count > 0)
        {
            CTM_TYPE_CODE = dt.Rows[0]["CTM_TYPE_CODE"].ToString();
            CTM_TYPE_DESC = dt.Rows[0]["CTM_TYPE_DESC"].ToString();
            CTM_FIRST_LETTER = dt.Rows[0]["CTM_FIRST_LETTER"].ToString();
        }
    }
    #endregion



}
