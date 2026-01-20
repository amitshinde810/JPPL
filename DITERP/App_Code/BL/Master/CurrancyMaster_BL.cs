using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
/// Summary description for CurrancyMaster
/// </summary>
public class CurrancyMaster_BL
{


    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _CURR_COUNTRY_CODE;
    private int _CURR_CM_COMP_ID;
    private int _CURR_CODE;
    private string _CURR_NAME;
    private string _CURR_SHORT_NAME;
    private string _CURR_DESC;
    private bool _ES_DELETE;
    private bool _MODIFY;
    private double _CURR_RATE;
    #endregion


    #region Public Properties
    public int CURR_COUNTRY_CODE
    {
        get { return _CURR_COUNTRY_CODE; }
        set { _CURR_COUNTRY_CODE=value; }
    }

    public int CURR_CM_COMP_ID
    {
        get { return _CURR_CM_COMP_ID; }
        set { _CURR_CM_COMP_ID = value; }
    }

    public int CURR_CODE
    {
        get { return _CURR_CODE; }
        set { _CURR_CODE = value; }
    }
    public string CURR_NAME
    {
        get { return _CURR_NAME; }
        set { _CURR_NAME = value; }
    }
    public string CURR_SHORT_NAME
    {
        get { return _CURR_SHORT_NAME; }
        set { _CURR_SHORT_NAME = value; }
    }
    public string CURR_DESC
    {
        get{return _CURR_DESC;}
        set{_CURR_DESC=value;}
    }

 
    public bool ES_DELETE
    {
        get { return _ES_DELETE; }
        set { _ES_DELETE = value; }
    }
    public double CURR_RATE
    {
        get { return _CURR_RATE; }
        set { _CURR_RATE = value; }
    }
    public bool MODIFY
    {
        get { return _MODIFY; }
        set { _MODIFY = value; }
    }

    #endregion



	public CurrancyMaster_BL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public CurrancyMaster_BL( int Id)
    {
        _CURR_CODE = Id;
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

                SqlParameter[] Params = 
			    { 
				    new SqlParameter("@CURR_COUNTRY_CODE",CURR_COUNTRY_CODE),
				    new SqlParameter("@CURR_CM_COMP_ID",CURR_CM_COMP_ID),
				    new SqlParameter("@CURR_NAME",CURR_NAME),
                    new SqlParameter("@CURR_SHORT_NAME",CURR_SHORT_NAME),
                    new SqlParameter("@CURR_DESC",CURR_DESC),
                    new SqlParameter("@CURR_RATE",CURR_RATE)

                   
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CURRANCY_MASTER_INSERT", Params);
            }
            else
            {
                Msg = "Currency Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Currancy  Class", "Save", Ex.Message);
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
                new SqlParameter("@CURR_COUNTRY_CODE",SqlDbType.Int),
                new SqlParameter("@CURR_CM_COMP_ID",SqlDbType.Int),
                new SqlParameter("@CURR_CODE",SqlDbType.Int),
				new SqlParameter("@CURR_NAME",SqlDbType.VarChar),
                new SqlParameter("@CURR_SHORT_NAME",SqlDbType.VarChar),
                new SqlParameter("@CURR_DESC",SqlDbType.VarChar),
                new SqlParameter("@ES_DELETE",SqlDbType.Bit),   
                new SqlParameter("@MODIFY",SqlDbType.Bit), 
                new SqlParameter("@TYPE",SqlDbType.VarChar),
                new SqlParameter("@CURR_RATE",SqlDbType.Float)
			};


            if (CURR_COUNTRY_CODE ==0)
            {

                Params[0].Value = DBNull.Value;
                
            }
            else
            {
                Params[0].Value = CURR_COUNTRY_CODE;
               
            }

            if (CURR_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = CURR_CM_COMP_ID;

            }
            if (CURR_CODE == 0)
            {
                Params[2].Value = DBNull.Value;

            }
            else
            {

                Params[2].Value = CURR_CODE;

            }

            if (CURR_NAME != null)
            {
                Params[3].Value = CURR_NAME;
            }
            else
            {
                
                Params[3].Value = DBNull.Value;

            }

            if (CURR_SHORT_NAME != null)
            {
                Params[4].Value = CURR_SHORT_NAME;
            }
            else
            {
                Params[4].Value = DBNull.Value;


            }

            if (CURR_DESC != null)
            {
                Params[5].Value = CURR_DESC;
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
            if (CURR_RATE != null)
            {
                Params[9].Value = CURR_RATE;
            }
            else
            {
                Params[9].Value = DBNull.Value;
            }


            dt = DL_DBAccess.SelectData("SP_CURRANCY_MASTER_Select", Params);
          

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


    #region FillGrid
    public void FillGrid(GridView XGrid)
    {
        DataTable dt = new DataTable();
        //dt = GetRecords("GETINFO");
        //dt = CommonClasses.Execute("Select * fro CURRANCY_MASTER where CURR_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE=0");
        //XGrid.DataSource = dt;
        //XGrid.DataBind();
    }
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DataTable dt = new DataTable();
        dt = GetRecords( "GETINFO");

        if (dt.Rows.Count > 0)
        {
            //CURR_COUNTRY_CODE =Convert.ToInt32( dt.Rows[0]["CURR_COUNTRY_CODE"].ToString());
            CURR_NAME = dt.Rows[0]["CURR_NAME"].ToString();
            CURR_DESC = dt.Rows[0]["CURR_DESC"].ToString();
            CURR_SHORT_NAME = dt.Rows[0]["CURR_SHORT_NAME"].ToString();
        }
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
                    new SqlParameter("@CURR_COUNTRY_CODE",CURR_COUNTRY_CODE),
				    new SqlParameter("@CURR_CODE",CURR_CODE),
				    new SqlParameter("@CURR_CM_COMP_ID",CURR_CM_COMP_ID),
				    new SqlParameter("@CURR_NAME",CURR_NAME),
				    new SqlParameter("@CURR_SHORT_NAME",CURR_SHORT_NAME),
				    new SqlParameter("@CURR_DESC",CURR_DESC),
                    new SqlParameter("@CURR_RATE",CURR_RATE) 

			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_CURRANCY_MASTER_Update", Params);
            }
            else
            {
                Msg = "Currancy  Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Cyrrancy Class", "Update", Ex.Message);
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


    #region Delete
    public bool Delete()
    {
        bool result = false;
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            SqlParameter[] Params = { new SqlParameter("@CURR_CODE", CURR_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_CURRANCY_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Currency Category Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion



}
