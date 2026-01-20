using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SalesTaxMaster_BL
/// </summary>
public class SalesTaxMaster_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables

    private int _ST_CODE;
    private int _ST_CM_COMP_ID;
    private string _ST_TAX_NAME;
    private string _ST_ALIAS;
    private double _ST_SALES_TAX;
    private double _ST_TCS_TAX;
    private double _ST_SET_OFF;
    private int _ST_FORM_NO;
    private string _ST_SALES_ACC_HEAD;
    private string _ST_TAX_ACC_HEAD;
    private string _ST_TAX_SALE_ACC;
    private string _ST_TAX_PUR_ACC;
    private bool _ES_DELETE;
    private bool _MODIFY;

    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion

    public SalesTaxMaster_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Parameterise Constructor
    public SalesTaxMaster_BL(int Id)
    {
        _ST_CODE = Id;
    }
    #endregion

    #region Public Properties

    public int ST_CODE
    {
        get { return _ST_CODE; }
        set { _ST_CODE = value; }
    }
    public int ST_CM_COMP_ID
    {
        get { return _ST_CM_COMP_ID; }
        set { _ST_CM_COMP_ID = value; }
    }
    public string ST_TAX_NAME
    {
        get { return _ST_TAX_NAME; }
        set { _ST_TAX_NAME = value; }
    }
    public string ST_ALIAS
    {
        get { return _ST_ALIAS; }
        set { _ST_ALIAS = value; }
    }

    public double ST_SALES_TAX
    {
        get { return _ST_SALES_TAX; }
        set { _ST_SALES_TAX = value; }
    }

    public double ST_TCS_TAX
    {
        get { return _ST_TCS_TAX; }
        set { _ST_TCS_TAX = value; }
    }

    public double ST_SET_OFF
    {
        get { return _ST_SET_OFF; }
        set { _ST_SET_OFF = value; }
    }
    public int ST_FORM_NO
    {
        get { return _ST_FORM_NO; }
        set { _ST_FORM_NO = value; }
    }
    public string ST_SALES_ACC_HEAD
    {
        get { return _ST_SALES_ACC_HEAD; }
        set { _ST_SALES_ACC_HEAD = value; }
    }
    public string ST_TAX_ACC_HEAD
    {
        get { return _ST_TAX_ACC_HEAD; }
        set { _ST_TAX_ACC_HEAD = value; }
    }

    public string ST_TAX_SALE_ACC
    {
        get { return _ST_TAX_SALE_ACC; }
        set { _ST_TAX_SALE_ACC = value; }
    }
    public string ST_TAX_PUR_ACC
    {
        get { return _ST_TAX_PUR_ACC; }
        set { _ST_TAX_PUR_ACC = value; }
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
				new SqlParameter("@ST_CODE",SqlDbType.Int),
				new SqlParameter("@ST_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@ST_TAX_NAME",SqlDbType.VarChar),
                new SqlParameter("@ST_ALIAS",SqlDbType.VarChar),
                new SqlParameter("@ST_SALES_TAX",SqlDbType.Float),
                new SqlParameter("@ST_TCS_TAX",SqlDbType.Float),
                new SqlParameter("@ST_SET_OFF",SqlDbType.Float),
                 new SqlParameter("@ST_FORM_NO",SqlDbType.Int),
                 new SqlParameter("@ST_SALES_ACC_HEAD",SqlDbType.VarChar),
                new SqlParameter("@ST_TAX_ACC_HEAD",SqlDbType.VarChar),
				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit),
                new SqlParameter("@TYPE",SqlDbType.VarChar)
               
			};


            if (ST_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = ST_CODE;
            }

            if (ST_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = ST_CM_COMP_ID;
            }

            if (ST_TAX_NAME != null)
            {
                Params[2].Value = ST_TAX_NAME;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }

            if (ST_ALIAS != null)
            {
                Params[3].Value = ST_ALIAS;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }

            if (ST_SALES_TAX != 0.0)
            {
                Params[4].Value = ST_SALES_TAX;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }

            if (ST_TCS_TAX != 0.0)
            {
                Params[5].Value = ST_TCS_TAX;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }


            if (ST_SET_OFF != 0.0)
            {
                Params[6].Value = ST_SET_OFF;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }

            if (ST_FORM_NO == 0)
            {
                Params[7].Value = DBNull.Value;
            }
            else
            {
                Params[7].Value = ST_FORM_NO;
            }

            if (ST_SALES_ACC_HEAD != null)
            {
                Params[8].Value = DBNull.Value;
            }
            else
            {
                Params[8].Value = ST_SALES_ACC_HEAD;
            }

            if (ST_TAX_ACC_HEAD != null)
            {
                Params[9].Value = DBNull.Value;
            }
            else
            {
                Params[9].Value = ST_TAX_ACC_HEAD;
            }

            if (ES_DELETE != null)
            {
                Params[10].Value = ES_DELETE;
            }
            else
            {
                Params[10].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[11].Value = MODIFY;
            }
            else
            {
                Params[11].Value = DBNull.Value;
            }

            if (Type != null)
            {
                Params[12].Value = Type;
            }
            else
            {
                Params[12].Value = DBNull.Value;
            }


            //dt = DL_DBAccess.SelectData("", Params);

            dt = CommonClasses.Execute("SELECT ST_CODE, ST_CM_COMP_ID, ST_TAX_NAME, ST_ALIAS, cast(ST_SALES_TAX as numeric(20,2)) as ST_SALES_TAX,cast(ST_TCS_TAX as numeric(20,2)) as ST_TCS_TAX, ST_SET_OFF, ST_FORM_NO, ST_SALES_ACC_HEAD, ST_TAX_ACC_HEAD FROM SALES_TAX_MASTER WHERE ES_DELETE =0 order by ST_TAX_NAME ASC");

            return dt;

        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("Sales Tax Master Class", "GetInfo", ex.Message);
            return dt;
        }

    }
    #endregion

    #region GetInfo
    public void GetInfo()
    {
        DataTable dt = new DataTable();
        //dt = GetRecords("GETINFO");

        dt = dt = CommonClasses.Execute("SELECT ST_TAX_NAME, ST_ALIAS, ST_SALES_TAX, ST_TCS_TAX, ST_SET_OFF, ST_FORM_NO, ST_SALES_ACC_HEAD, ST_TAX_ACC_HEAD,ST_TAX_SALE_ACC,ST_TAX_PUR_ACC   FROM SALES_TAX_MASTER WHERE ST_CODE='" + ST_CODE + "'");


        if (dt.Rows.Count > 0)
        {
            //ST_CODE = Convert.ToInt32(dt.Rows[0]["ST_CODE"]);
            ST_TAX_NAME = dt.Rows[0]["ST_TAX_NAME"].ToString();
            ST_ALIAS = dt.Rows[0]["ST_ALIAS"].ToString();
            ST_SALES_TAX = Convert.ToDouble(dt.Rows[0]["ST_SALES_TAX"]);
            ST_TCS_TAX = Convert.ToDouble(dt.Rows[0]["ST_TCS_TAX"]);
            ST_SET_OFF = Convert.ToDouble(dt.Rows[0]["ST_SET_OFF"]);
            ST_FORM_NO = Convert.ToInt32(dt.Rows[0]["ST_FORM_NO"]);
            ST_SALES_ACC_HEAD = dt.Rows[0]["ST_SALES_ACC_HEAD"].ToString();
            ST_TAX_ACC_HEAD = dt.Rows[0]["ST_TAX_ACC_HEAD"].ToString();
            ST_TAX_SALE_ACC = dt.Rows[0]["ST_TAX_SALE_ACC"].ToString();
            ST_TAX_PUR_ACC = dt.Rows[0]["ST_TAX_PUR_ACC"].ToString();

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
        //dt = GetRecords("CHECKSAVE");
        dt = CommonClasses.Execute("SELECT ST_TAX_NAME, ST_ALIAS, ST_SALES_TAX, "
        + " ST_TCS_TAX, ST_SET_OFF, ST_FORM_NO, ST_SALES_ACC_HEAD, ST_TAX_ACC_HEAD "
        + " FROM SALES_TAX_MASTER WHERE ES_DELETE =0 AND ST_TAX_NAME='" + ST_TAX_NAME + "'");


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
        //dt = GetRecords("CHECKUPDATE");
        dt = dt = CommonClasses.Execute("SELECT * FROM SALES_TAX_MASTER WHERE ES_DELETE =0 AND ST_CODE != '" + ST_CODE + "' AND lower(ST_TAX_NAME) = lower('" + ST_TAX_NAME + "')");
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
                CommonClasses.Execute("INSERT INTO SALES_TAX_MASTER(ST_CM_COMP_ID, ST_TAX_NAME, ST_ALIAS,ST_SALES_TAX, ST_TCS_TAX, ST_SET_OFF, ST_FORM_NO, ST_SALES_ACC_HEAD, ST_TAX_ACC_HEAD,ST_TAX_SALE_ACC,ST_TAX_PUR_ACC)VALUES(" + ST_CM_COMP_ID + ",'" + ST_TAX_NAME + "','" + ST_ALIAS + "','" + ST_SALES_TAX + "','" + ST_TCS_TAX + "','" + ST_SET_OFF + "','" + ST_FORM_NO + "','" + ST_SALES_ACC_HEAD + "','" + ST_TAX_ACC_HEAD + "','" + ST_TAX_SALE_ACC + "','" + ST_TAX_PUR_ACC + "')");
                result = true;
            }
            else
            {
                Msg = "Tax Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Tax Master Class", "Save", Ex.Message);
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
				   new SqlParameter("@ST_CODE",ST_CODE),
				   new SqlParameter("@ST_CM_COMP_ID",ST_CM_COMP_ID),
				   new SqlParameter("@ST_TAX_NAME",ST_TAX_NAME),
                   new SqlParameter("@ST_ALIAS",ST_ALIAS),
                   new SqlParameter("@ST_SALES_TAX",ST_SALES_TAX),
                   new SqlParameter("@ST_TCS_TAX",ST_TCS_TAX),
                   new SqlParameter("@ST_SET_OFF",ST_SET_OFF),
                   new SqlParameter("@ST_FORM_NO",ST_FORM_NO),
                   new SqlParameter("@ST_SALES_ACC_HEAD",ST_SALES_ACC_HEAD),
                   new SqlParameter("@ST_TAX_ACC_HEAD",ST_TAX_ACC_HEAD),
                   new SqlParameter("@ST_TAX_SALE_ACC",ST_TAX_SALE_ACC),
                   new SqlParameter("@ST_TAX_PUR_ACC",ST_TAX_PUR_ACC),
				   new SqlParameter("@ES_DELETE",false),
				   new SqlParameter("@MODIFY",false)
                    

			    };
                //result = DL_DBAccess.Insertion_Updation_Delete("", Params);
                CommonClasses.Execute("UPDATE SALES_TAX_MASTER SET ST_CM_COMP_ID='" + ST_CM_COMP_ID + "', ST_TAX_NAME='" + ST_TAX_NAME + "', ST_ALIAS='" + ST_ALIAS + "',ST_SALES_TAX='" + ST_SALES_TAX + "', ST_TCS_TAX='" + ST_TCS_TAX + "', ST_SET_OFF='" + ST_SET_OFF + "', ST_FORM_NO='" + ST_FORM_NO + "', ST_SALES_ACC_HEAD='" + ST_SALES_ACC_HEAD + "', ST_TAX_ACC_HEAD='" + ST_TAX_ACC_HEAD + "',ST_TAX_SALE_ACC='" + ST_TAX_SALE_ACC + "',ST_TAX_PUR_ACC='" + ST_TAX_PUR_ACC + "' WHERE ST_CODE= '" + ST_CODE + "'");
                result = true;
            }
            else
            {
                Msg = "Tax Name Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Sales Tax Master Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@ST_CODE", ST_CODE) };

            CommonClasses.Execute("UPDATE SALES_TAX_MASTER SET ES_DELETE=1 WHERE ST_CODE= '" + ST_CODE + "'");
            //result = DL_DBAccess.Insertion_Updation_Delete("", Params);

            return result=true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Sales Tax Master Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion


    #endregion
}
