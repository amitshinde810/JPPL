using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for AreaMaster_BL
/// </summary>
public class AreaMaster_BL
{


    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion


    #region Private Variables
    private int _A_CODE;
    private int _A_U_CODE;
    private DateTime _A_U_DATE;
    private int _A_CM_COMP_ID;
    private string _A_NO;
    private string _A_DESC;
    private bool _ES_DELETE;
    private bool _MODIFY;
    #endregion


    #region Public Properties

    public int A_CODE
    {
        get { return _A_CODE; }
        set { _A_CODE = value; }
    }

    public int A_U_CODE
    {
        get { return _A_U_CODE; }
        set { _A_U_CODE = value; }
    }
    public DateTime A_U_DATE
    {
        get { return _A_U_DATE; }
        set { _A_U_DATE = value; }
    }
    public int A_CM_COMP_ID
    {
        get { return _A_CM_COMP_ID; }
        set { _A_CM_COMP_ID = value; }
    }
    public string A_NO
    {
        get { return _A_NO; }
        set { _A_NO = value; }
    }
    public string A_DESC
    {
        get { return _A_DESC; }
        set { _A_DESC = value; }
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

	public AreaMaster_BL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public AreaMaster_BL(int Id)
    {
        _A_CODE = Id;
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

                //SqlParameter[] Params = 
                //{ 
				    
                //    new SqlParameter("@A_CM_COMP_ID",A_CM_COMP_ID),
                //    new SqlParameter("@A_NO",A_NO),
                //    new SqlParameter("@A_DESC",A_DESC)
                    
                   
                //};
                //result = DL_DBAccess.Insertion_Updation_Delete("SP_ADMIN_MASTER_INSERT", Params);

                result = CommonClasses.Execute1("INSERT INTO AREA_MASTER (A_CM_COMP_ID,A_NO,A_DESC) values (" + A_CM_COMP_ID + ",'" + A_NO + "','" + A_DESC + "')");
            }
            else
            {
                Msg = "Area Code  Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Admin Class", "Save", Ex.Message);
        }
        return result;
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



    #region GetRecords
    public DataTable GetRecords(string Type)
    {
        DataTable dt = new DataTable();
        DL_DBAccess = new DatabaseAccessLayer();

        try
        {
            SqlParameter[] Params = 
			{ 
                new SqlParameter("@A_CODE",SqlDbType.Int),
                new SqlParameter("@A_U_CODE",SqlDbType.Int),
				new SqlParameter("@A_U_DATE",SqlDbType.Date),
                new SqlParameter("@A_CM_COMP_ID",SqlDbType.Int),
                new SqlParameter("@A_NO",SqlDbType.VarChar),
                new SqlParameter("@A_DESC",SqlDbType.VarChar),
                new SqlParameter("@ES_DELETE",SqlDbType.Bit),   
                new SqlParameter("@MODIFY",SqlDbType.Bit), 
                new SqlParameter("@TYPE",SqlDbType.VarChar)
			};

            if (A_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = A_CODE; //DBNull.Value;

            }
            if (A_U_CODE == 0)
            {
                Params[1].Value = DBNull.Value;

            }
            else
            {

                Params[1].Value = A_U_CODE;

            }

            if (A_U_DATE != null)
            {
                Params[2].Value = DBNull.Value;
               
            }
            else
            {
                Params[2].Value = A_U_DATE;
                

            }

            if (A_CM_COMP_ID == 0)
            {
                Params[3].Value = DBNull.Value;
            }
            else
            {
                Params[3].Value = A_CM_COMP_ID;


            }

            if (A_NO != null)
            {
                Params[4].Value = A_NO;
            }
            else
            {
                Params[4].Value = DBNull.Value;

            }

            if (A_DESC != null)
            {
                Params[5].Value = A_DESC;
            }
            else
            {
                
                Params[5].Value = DBNull.Value;


            }


            if (ES_DELETE != null)
            {
                Params[6].Value = ES_DELETE;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[7].Value = MODIFY;
            }
            else
            {
                Params[7].Value = DBNull.Value;
            }
            if (Type != null)
            {
                Params[8].Value = Type;
            }
            else
            {
                Params[8].Value = DBNull.Value;
            }



            dt = DL_DBAccess.SelectData("SP_AREA_MASTER_Select", Params);

            return dt;
        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("Currency Master", "GetRecords", ex.Message);
            return dt;
        }

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
            if (CommonClasses.Execute("SELECT A_CODE,A_NO,A_DESC FROM AREA_MASTER where A_DESC='" + A_DESC + "' and A_CODE!=" + A_CODE + "").Rows.Count == 0)
            {
                //SqlParameter[] Params = 
                //{ 
				    
                //  new SqlParameter("@A_CODE",A_CODE),
                //    new SqlParameter("@A_CM_COMP_ID",A_CM_COMP_ID),
                //    new SqlParameter("@A_NO",A_NO),
                //    new SqlParameter("@A_DESC",A_DESC)

                //};
                //result = DL_DBAccess.Insertion_Updation_Delete("SP_AREA_MASTER_Update", Params);

                result = CommonClasses.Execute1("UPDATE AREA_MASTER set A_NO='" + A_NO + "',A_DESC='" + A_DESC + "' where A_CODE=" + A_CODE + "");
            }
            else
            {
                Msg = "Area Code Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Area Class", "Update", Ex.Message);
        }
        return result;
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



    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DataTable dt = new DataTable();
        dt = GetRecords("FILLGRID");

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
            SqlParameter[] Params = { new SqlParameter("@A_CODE", A_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_AREA_MASTER_Delete", Params);
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
        dt = GetRecords("GETINFO");

        if (dt.Rows.Count > 0)
        {
            A_NO = dt.Rows[0]["A_NO"].ToString();
            A_DESC = dt.Rows[0]["A_DESC"].ToString();
            //CURR_SHORT_NAME = dt.Rows[0]["CURR_SHORT_NAME"].ToString();
        }
    }
    #endregion



}
