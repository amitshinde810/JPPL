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
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for ExciseTariffDetails_BL
/// </summary>
public class ExciseTariffDetails_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    public string Msg = "";
    #endregion

    #region Private Variables
    private int _E_CODE;
    private int _E_CM_COMP_ID;
    private string _E_TARIFF_NO;
    private string _E_COMMODITY;
    private double _E_BASIC;
    private double _E_SPECIAL;
    private double _E_EDU_CESS;
    private double _E_H_EDU;
    private bool _ES_DELETE;
    private bool _MODIFY;
    private string _E_EX_TYPE;
    private int _E_TALLY_BASIC;
    private int _E_TALLY_SPECIAL;
    private int _E_TALLY_EDU;
    private int _E_TALLY_H_EDU;

    private bool _E_TALLY_GST_EXCISE;

    //clsITEM_UNIT_MASTER objclsITEM_UNIT_MASTER;
    #endregion


    public ExciseTariffDetails_BL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Parameterise Constructor
    public ExciseTariffDetails_BL(int Id)
    {
        _E_CODE = Id;
    }
    #endregion

    #region Public Properties
    public int E_CODE
    {
        get { return _E_CODE; }
        set { _E_CODE = value; }
    }
    public int E_CM_COMP_ID
    {
        get { return _E_CM_COMP_ID; }
        set { _E_CM_COMP_ID = value; }
    }
    public string E_TARIFF_NO
    {
        get { return _E_TARIFF_NO; }
        set { _E_TARIFF_NO = value; }
    }
    public string E_COMMODITY
    {
        get { return _E_COMMODITY; }
        set { _E_COMMODITY = value; }
    }

    public double E_BASIC
    {
        get { return _E_BASIC; }
        set { _E_BASIC = value; }
    }

    public double E_SPECIAL
    {
        get { return _E_SPECIAL; }
        set { _E_SPECIAL = value; }
    }

    public double E_EDU_CESS
    {
        get { return _E_EDU_CESS; }
        set { _E_EDU_CESS = value; }
    }

    public double E_H_EDU
    {
        get { return _E_H_EDU; }
        set { _E_H_EDU = value; }
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

    public string E_EX_TYPE
    {
        get { return _E_EX_TYPE; }
        set { _E_EX_TYPE = value; }
    }


    public int E_TALLY_BASIC
    {
        get { return _E_TALLY_BASIC; }
        set { _E_TALLY_BASIC = value; }
    }
    public int E_TALLY_SPECIAL
    {
        get { return _E_TALLY_SPECIAL; }
        set { _E_TALLY_SPECIAL = value; }
    }

    public int E_TALLY_EDU
    {
        get { return _E_TALLY_EDU; }
        set { _E_TALLY_EDU = value; }
    }
    public int E_TALLY_H_EDU
    {
        get { return _E_TALLY_H_EDU; }
        set { _E_TALLY_H_EDU = value; }
    }


    public bool E_TALLY_GST_EXCISE
    {
        get { return _E_TALLY_GST_EXCISE; }
        set { _E_TALLY_GST_EXCISE = value; }
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
				new SqlParameter("@E_CODE",SqlDbType.Int),
				new SqlParameter("@E_CM_COMP_ID",SqlDbType.Int),
				new SqlParameter("@E_TARIFF_NO",SqlDbType.VarChar),
                new SqlParameter("@E_COMMODITY",SqlDbType.VarChar),
                new SqlParameter("@E_BASIC",SqlDbType.Float),
                new SqlParameter("@E_SPECIAL",SqlDbType.Float),
                new SqlParameter("@E_EDU_CESS",SqlDbType.Float),
                new SqlParameter("@E_H_EDU",SqlDbType.Float),

                new SqlParameter("@E_TALLY_BASIC",SqlDbType.Int),
                new SqlParameter("@E_TALLY_SPECIAL",SqlDbType.Int),
                new SqlParameter("@E_TALLY_EDU",SqlDbType.Int),
                new SqlParameter("@E_TALLY_H_EDU",SqlDbType.Int),


				new SqlParameter("@ES_DELETE",SqlDbType.Bit),
				new SqlParameter("@MODIFY",SqlDbType.Bit),
                new SqlParameter("@TYPE",SqlDbType.VarChar),
                new SqlParameter("@E_TALLY_GST_EXCISE",SqlDbType.Bit)
			};


            if (E_CODE == 0)
            {
                Params[0].Value = DBNull.Value;
            }
            else
            {
                Params[0].Value = E_CODE;
            }

            if (E_CM_COMP_ID == 0)
            {
                Params[1].Value = DBNull.Value;
            }
            else
            {
                Params[1].Value = E_CM_COMP_ID;
            }

            if (E_TARIFF_NO != null)
            {
                Params[2].Value = E_TARIFF_NO;
            }
            else
            {
                Params[2].Value = DBNull.Value;
            }

            if (E_COMMODITY != null)
            {
                Params[3].Value = E_COMMODITY;
            }
            else
            {
                Params[3].Value = DBNull.Value;
            }

            if (E_BASIC != 0.0)
            {
                Params[4].Value = E_BASIC;
            }
            else
            {
                Params[4].Value = DBNull.Value;
            }

            if (E_SPECIAL != 0.0)
            {
                Params[5].Value = E_SPECIAL;
            }
            else
            {
                Params[5].Value = DBNull.Value;
            }


            if (E_EDU_CESS != 0.0)
            {
                Params[6].Value = E_EDU_CESS;
            }
            else
            {
                Params[6].Value = DBNull.Value;
            }

            if (E_H_EDU != 0.0)
            {
                Params[7].Value = E_H_EDU;
            }
            else
            {
                Params[7].Value = DBNull.Value;
            }


            if (E_TALLY_BASIC != 0)
            {
                Params[8].Value = E_TALLY_BASIC;
            }
            else
            {
                Params[8].Value = DBNull.Value;
            }
            if (E_TALLY_SPECIAL != 0)
            {
                Params[9].Value = E_TALLY_SPECIAL;
            }
            else
            {
                Params[9].Value = DBNull.Value;
            }
            if (E_TALLY_EDU != 0)
            {
                Params[10].Value = E_TALLY_EDU;
            }
            else
            {
                Params[10].Value = DBNull.Value;
            }
            if (E_TALLY_H_EDU != 0)
            {
                Params[11].Value = E_TALLY_H_EDU;
            }
            else
            {
                Params[11].Value = DBNull.Value;
            }
            if (ES_DELETE != null)
            {
                Params[12].Value = ES_DELETE;
            }
            else
            {
                Params[12].Value = DBNull.Value;
            }

            if (MODIFY != null)
            {
                Params[13].Value = MODIFY;
            }
            else
            {
                Params[13].Value = DBNull.Value;
            }

            if (Type != null)
            {
                Params[14].Value = Type;
            }
            else
            {
                Params[14].Value = DBNull.Value;
            }
            if (E_TALLY_GST_EXCISE != null)
            {
                Params[15].Value = E_TALLY_GST_EXCISE;
            }
            else
            {
                Params[15].Value = DBNull.Value;
            }

            dt = DL_DBAccess.SelectData("SP_EXCISE_TARIFF_MASTER_Select", Params);

            return dt;

        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
            CommonClasses.SendError("Excise Tariff Details Class", "GetInfo", ex.Message);
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
            E_CODE = Convert.ToInt32(dt.Rows[0]["E_CODE"]);
            E_TARIFF_NO = dt.Rows[0]["E_TARIFF_NO"].ToString();
            E_COMMODITY = dt.Rows[0]["E_COMMODITY"].ToString();
            E_BASIC = Convert.ToDouble(dt.Rows[0]["E_BASIC"]);
            E_SPECIAL = Convert.ToDouble(dt.Rows[0]["E_SPECIAL"]);
            E_EDU_CESS = Convert.ToDouble(dt.Rows[0]["E_EDU_CESS"]);
            E_H_EDU = Convert.ToDouble(dt.Rows[0]["E_H_EDU"]);
            E_TALLY_BASIC = Convert.ToInt32(dt.Rows[0]["E_TALLY_BASIC"]);
            E_TALLY_SPECIAL = Convert.ToInt32(dt.Rows[0]["E_TALLY_SPECIAL"]);
            E_TALLY_EDU = Convert.ToInt32(dt.Rows[0]["E_TALLY_EDU"]);
            E_TALLY_H_EDU = Convert.ToInt32(dt.Rows[0]["E_TALLY_H_EDU"]);

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
				    new SqlParameter("@E_CM_COMP_ID",E_CM_COMP_ID),
				    new SqlParameter("@E_TARIFF_NO",E_TARIFF_NO),
                    new SqlParameter("@E_COMMODITY",E_COMMODITY),
                    new SqlParameter("@E_BASIC",E_BASIC),
                    new SqlParameter("@E_SPECIAL",E_SPECIAL),
                    new SqlParameter("@E_EDU_CESS",E_EDU_CESS),
                    new SqlParameter("@E_H_EDU",E_H_EDU),

                    new SqlParameter("@E_TALLY_BASIC",E_TALLY_BASIC),
                    new SqlParameter("@E_TALLY_SPECIAL",E_TALLY_SPECIAL),
                    new SqlParameter("@E_TALLY_EDU",E_TALLY_EDU),
                    new SqlParameter("@E_TALLY_H_EDU",E_TALLY_H_EDU),
                    new SqlParameter("@E_TALLY_GST_EXCISE",E_TALLY_GST_EXCISE),
                    
				    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false)
                    
			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_EXCISE_TARIFF_MASTER_Insert", Params);
            }
            else
            {
                Msg = "Tariff Number Or Commdity Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Details Class", "Save", Ex.Message);
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
				    new SqlParameter("@E_CODE",E_CODE),
				   new SqlParameter("@E_CM_COMP_ID",E_CM_COMP_ID),
				    new SqlParameter("@E_TARIFF_NO",E_TARIFF_NO),
                     new SqlParameter("@E_COMMODITY",E_COMMODITY),
                      new SqlParameter("@E_BASIC",E_BASIC),
                       new SqlParameter("@E_SPECIAL",E_SPECIAL),
                        new SqlParameter("@E_EDU_CESS",E_EDU_CESS),
                        new SqlParameter("@E_H_EDU",E_H_EDU),
                    
                    new SqlParameter("@E_TALLY_BASIC",E_TALLY_BASIC),
                    new SqlParameter("@E_TALLY_SPECIAL",E_TALLY_SPECIAL),
                    new SqlParameter("@E_TALLY_EDU",E_TALLY_EDU),
                    new SqlParameter("@E_TALLY_H_EDU",E_TALLY_H_EDU),
				   
                    new SqlParameter("@ES_DELETE",false),
				    new SqlParameter("@MODIFY",false)
                    

			    };
                result = DL_DBAccess.Insertion_Updation_Delete("SP_EXCISE_TARIFF_MASTER_Update", Params);
            }
            else
            {
                Msg = "Tariff Number Already Exist";
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Excise Tariff Details Class", "Update", Ex.Message);
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
            SqlParameter[] Params = { new SqlParameter("@E_CODE", E_CODE) };
            result = DL_DBAccess.Insertion_Updation_Delete("SP_EXCISE_TARIFF_MASTER_Delete", Params);
            return result;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Excise Tariff Details Class", "Delete", ex.Message);
            return false;
        }
        finally
        { }
    }
    #endregion


    #endregion
}
