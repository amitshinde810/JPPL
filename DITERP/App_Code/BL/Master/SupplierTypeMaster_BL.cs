using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for SupplierTypeMaster_BL
/// </summary>
public class SupplierTypeMaster_BL
{



    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion


    #region Private Variables
    private int _STM_CODE;
    private int _STM_CM_COMP_ID;
    private string _STM_TYPE_CODE;
    private string _STM_TYPE_DESC;
    private string _STM_FIRST_LETTER;
    private bool _ES_DELETE;
    private bool _MODIFY;
    #endregion


    #region Public Properties

    public int STM_CODE
    {
        get { return _STM_CODE; }
        set { _STM_CODE = value; }
    }

    public int STM_CM_COMP_ID
    {
        get { return _STM_CM_COMP_ID; }
        set { _STM_CM_COMP_ID = value; }
    }
    public string STM_TYPE_CODE
    {
        get { return _STM_TYPE_CODE; }
        set { _STM_TYPE_CODE = value; }
    }
    public string STM_TYPE_DESC
    {
        get { return _STM_TYPE_DESC; }
        set { _STM_TYPE_DESC = value; }
    }
    public string STM_FIRST_LETTER
    {
        get { return _STM_FIRST_LETTER; }
        set { _STM_FIRST_LETTER = value; }
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

    public SupplierTypeMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public SupplierTypeMaster_BL(int Id)
    {
        _STM_CODE = Id;
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

                result = CommonClasses.Execute1("Insert into SUPPLIER_TYPE_MASTER(STM_CM_COMP_ID,STM_TYPE_CODE,STM_TYPE_DESC,STM_FIRST_LETTER)values("+STM_CM_COMP_ID+",'"+STM_TYPE_CODE+"','"+STM_TYPE_DESC+"','"+STM_FIRST_LETTER+"')");

            }
                

            else
            {
                Msg = "Supplier Type Code Already Exist";
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
        dt = CommonClasses.Execute("Select STM_CODE,STM_CM_COMP_ID,STM_TYPE_CODE FROM SUPPLIER_TYPE_MASTER where STM_TYPE_CODE='" + STM_TYPE_CODE + "' AND ES_DELETE=0");

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

                result = CommonClasses.Execute1("Update  SUPPLIER_TYPE_MASTER set STM_CM_COMP_ID="+STM_CM_COMP_ID+",STM_TYPE_CODE='"+STM_TYPE_CODE+"',STM_TYPE_DESC='"+STM_TYPE_DESC+"',STM_FIRST_LETTER='"+STM_FIRST_LETTER+"' where STM_CODE="+STM_CODE+" ");

            }

            else
            {
                Msg = "Supplier Type Code Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Class", "Update", Ex.Message);
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
        dt = CommonClasses.Execute("Select * FROM SUPPLIER_TYPE_MASTER where STM_CODE!="+STM_CODE+" and STM_TYPE_CODE='" + STM_TYPE_CODE + "' AND ES_DELETE=0");


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
        dt = CommonClasses.Execute("Select STM_CODE,STM_CM_COMP_ID,STM_TYPE_CODE,STM_TYPE_DESC,STM_FIRST_LETTER,ES_DELETE,MODIFY FROM SUPPLIER_TYPE_MASTER where STM_CM_COMP_ID='" + STM_CM_COMP_ID + "' AND ES_DELETE=0 ORDER BY STM_TYPE_CODE  ");
        
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
            result = CommonClasses.Execute1("Update SUPPLIER_TYPE_MASTER set ES_DELETE=1 where STM_CODE="+STM_CODE+"");
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Area Class", "Delete", ex.Message);
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
        dt = CommonClasses.Execute("Select STM_CODE,STM_CM_COMP_ID,STM_TYPE_CODE,STM_TYPE_DESC,STM_FIRST_LETTER,ES_DELETE,MODIFY FROM SUPPLIER_TYPE_MASTER where STM_CODE='" + STM_CODE + "' AND ES_DELETE=0");

        if (dt.Rows.Count > 0)
        {
            STM_TYPE_CODE = dt.Rows[0]["STM_TYPE_CODE"].ToString();
            STM_TYPE_DESC = dt.Rows[0]["STM_TYPE_DESC"].ToString();
            STM_FIRST_LETTER = dt.Rows[0]["STM_FIRST_LETTER"].ToString();
        }
    }
    #endregion






}
