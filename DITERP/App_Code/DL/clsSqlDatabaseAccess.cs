using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlClient;

public class clsSqlDatabaseAccess
{
    #region "Data Members"

    private static string strConnString = null;
    private int lintReturn;

    #endregion

    #region "Constructors"
    public clsSqlDatabaseAccess()
    {
        try
        {
            strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
        }
        catch (Exception exc)
        {
          
        }
    }
    #endregion

    #region "Public Methods"

    #region "SelectData Method"
    /**
            *	The "SelectData" method selects data from the specified table in the main database & returns these data as a dataset.
            *	@method 	SelectData
            *	@param	    strSqlQuery - Select query to be executed
            *	@return 	dsSelectResult - dataset filled with selected data
            */
    public DataSet SelectData(string strSqlQuery)
    {
        //create a dataset object
        DataSet dsSelectResult = new DataSet();
        /*
        try
        {*/
        //Establish connection with the main databse using  strConnString.
        SqlConnection objNewConn = new SqlConnection(strConnString);

        //open the connection
        ConnectionState cs = objNewConn.State;
        
        
        //if(o==true)

        objNewConn.Open();

        SqlCommand objCommand = new SqlCommand(strSqlQuery,objNewConn);
		objCommand.CommandText = strSqlQuery;
		objCommand.CommandType = CommandType.Text;
        //create a data adapter object to execute the select query
        SqlDataAdapter daSelectAdapter = new SqlDataAdapter(objCommand);

        //fill the dataset
        daSelectAdapter.Fill(dsSelectResult);


        //close the connection
        objNewConn.Close();

        /*}
        catch (Exception exc)
        {
            //clsErrorLog.WriteErrorLog("~/ErrorLogs/ErrorLog", exc);
        }*/

        //return the result
        return (dsSelectResult);
    }
    
    public DataTable SelectDataTble(string strSqlQuery)
    {
        //create a dataset object
        DataTable dsSelectResult = new DataTable();
        /*
        try
        {*/
        //Establish connection with the main databse using  strConnString.
        SqlConnection objNewConn = new SqlConnection(strConnString);

        //open the connection
        ConnectionState cs = objNewConn.State;

        //if(o==true)

        objNewConn.Open();

        //create a data adapter object to execute the select query
        SqlDataAdapter daSelectAdapter = new SqlDataAdapter(strSqlQuery, objNewConn);

        //fill the dataset
        daSelectAdapter.Fill(dsSelectResult);


        //close the connection
        objNewConn.Close();

        /*}
        catch (Exception exc)
        {
            //clsErrorLog.WriteErrorLog("~/ErrorLogs/ErrorLog", exc);
        }*/

        //return the result
        return (dsSelectResult);
    }
    
    public int SelectDataReader(string strSqlQuery)
    {
        //create a dataset object

        SqlCommand cmd = new SqlCommand(strSqlQuery);
        SqlDataReader dr = null;
        int flag = 0;
        /*
        try
        {*/
        //Establish connection with the main databse using  strConnString.
        SqlConnection objNewConn = new SqlConnection(strConnString);
        //open the connection
        objNewConn.Open();

        cmd.Connection = objNewConn;

        dr = cmd.ExecuteReader();
        //dr.Read();
        //if (dr.GetString(0)==null)
        //    return 1;
        //else
        //    return 0;
        string username=string.Empty;

        while (dr.Read())
        {
            if ((string)dr["Username"] != "")
            {
                flag = 1;
                break;
            }            
        }
        return flag;
        dr.Close();
        
        //close the connection
        objNewConn.Close();

        /*}
        catch (Exception exc)
        {
            //clsErrorLog.WriteErrorLog("~/ErrorLogs/ErrorLog", exc);
        }*/

        //return the result
        
    }
    
    #endregion

    #region "SelectData_Reader Method"
    /**
            *	The "SelectData_Reader" method selects data from the specified table in the main database & returns these data as a dataset.
            *	@method 	SelectData_Reader
            *	@param	    strSqlQuery - Select query to be executed
            *	@return 	drSelectResult - data reader
            */
    public SqlDataReader SelectData_Reader(string strSqlQuery)
    {
        //create a datareader object
        SqlDataReader drSelectResult;
        /*
        try
        {*/
        //Establish connection with the main databse using  strConnString.
        SqlConnection objNewConn = new SqlConnection(strConnString);

        //create a command object to execute the select query
        SqlCommand objCommd = new SqlCommand(strSqlQuery, objNewConn);

        //open the connection
        objNewConn.Open();

        //execute the command
        drSelectResult = objCommd.ExecuteReader();

        /*}
        catch (Exception exc)
        {
            //clsErrorLog.WriteErrorLog("~/ErrorLogs/ErrorLog", exc);
        }*/

        //return the result
        return (drSelectResult);
    }
    
    #endregion

    #region "InsertData Method"
    /**
            *	The "InsertData" method inserts data into the specified table in the main database.
            *	@method 	InsertData
            *	@param	    strSqlQuery - insert query to be executed
            *	@return 	int-0 or 1
            */
    public int InsertData(string strSqlQuery)
    {
        try
        {
            //Establish connection with the main databse using  strConnString.
            SqlConnection objNewConn = new SqlConnection(strConnString);

            //open the connection
            objNewConn.Open();

            //create a command object to execute insert query
            SqlCommand objSqlCmd = new SqlCommand(strSqlQuery, objNewConn);

            //execute the insert query
            lintReturn = objSqlCmd.ExecuteNonQuery();

            //close the connection
            objNewConn.Close();

        }
        catch (Exception exc)
        {
            //return exc;

            //  clsErrorLog.WriteErrorLog("D:/OnlineLLR/masterpage/ErrorLogs/ErrorLog", exc);
        }
        return (lintReturn);
    }

    public string InsertDataWithLastValue(string strSqlQuery)
    {
        String str = null;
        try
        {
            //Establish connection with the main databse using strConnString.
            SqlConnection objNewConn = new SqlConnection(strConnString);

            //open the connection
            objNewConn.Open();

            //create a command object to execute insert query
            SqlCommand objSqlCmd = new SqlCommand(strSqlQuery, objNewConn);

            //execute the insert query
            str = objSqlCmd.ExecuteScalar().ToString();

            //close the connection
            objNewConn.Close();

        }
        catch (Exception exc)
        {
            //return exc;

            // clsErrorLog.WriteErrorLog("D:/OnlineLLR/masterpage/ErrorLogs/ErrorLog", exc);
        }
        return (str);
    }


    public string SelectPOUID(string strSqlQuery)
    {
        String str = null;
        try
        {
            //Establish connection with the main databse using strConnString.
            SqlConnection objNewConn = new SqlConnection(strConnString);

            //open the connection
            objNewConn.Open();

            //create a command object to execute insert query
            SqlCommand objSqlCmd = new SqlCommand(strSqlQuery, objNewConn);

            //execute the insert query
            str = objSqlCmd.ExecuteScalar().ToString();

            //close the connection
            objNewConn.Close();

        }
        catch (Exception exc)
        {
            //return exc;

            // clsErrorLog.WriteErrorLog("D:/OnlineLLR/masterpage/ErrorLogs/ErrorLog", exc);
        }
        return (str);
    }

    #endregion

    #region "DeleteData Method"
    /**
             *	The "DeleteData" method deletes data from the specified table in the main database.
             *	@method 	DeleteData
             *	@param	    strSqlQuery - insert query to be executed
             *	@return 	int-0 or 1
             */
    public int DeleteData(string strSqlQuery)
    {
        try
        {
            //Establish connection with the main databse using  strConnString.
            SqlConnection objNewConn = new SqlConnection(strConnString);

            //open the connection
            objNewConn.Open();

            //create a command object to execute delete query
            SqlCommand objSqlCmd = new SqlCommand(strSqlQuery, objNewConn);

            //execute the delete query
            lintReturn = objSqlCmd.ExecuteNonQuery();


            //close the connection
            objNewConn.Close();
        }
        catch (Exception exc)
        {
            //clsErrorLog.WriteErrorLog("~/ErrorLogs/ErrorLog", exc);
        }
        return (lintReturn);
    }
    
    #endregion

    #region "UpdateData Method"
    /**
             *	The "UpdateData" method deletes data from the specified table in the main database.
             *	@method 	UpdateData
             *	@param	    strSqlQuery - insert query to be executed
             *	@return 	int- 0 or 1
             */
    public int UpdateData(string strSqlQuery)
    {
        try
        {
            //Establish connection with the main databse using  strConnString.
            SqlConnection objNewConn = new SqlConnection(strConnString);

            //open the connection
            objNewConn.Open();

            //create a command object to execute delete query
            SqlCommand objSqlCmd = new SqlCommand(strSqlQuery, objNewConn);

            //execute the delete query
            lintReturn = objSqlCmd.ExecuteNonQuery();

            //close the connection
            objNewConn.Close();
        }
        catch (Exception exc)
        {
            //clsErrorLog.WriteErrorLog("~/ErrorLogs/ErrorLog", exc);
        }
        return (lintReturn);
    }
    #endregion

    #endregion
}

