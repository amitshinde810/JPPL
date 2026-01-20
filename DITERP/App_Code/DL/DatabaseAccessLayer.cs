using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

/// <summary>
///
/// new design for the cooljobvilla using storeprocedure.
/// </summary>
public class DatabaseAccessLayer
{
    #region "Data Members"
    private static string strConnString = string.Empty;
    private int lintReturn;
    SqlConnection conn = null;
    SqlCommand cmd = null;
    SqlDataAdapter adapter = null;
    SqlDataReader read;
    #endregion

    #region "Constructors"
    public DatabaseAccessLayer()
    {
        strConnString = ConfigurationManager.ConnectionStrings["CONNECT_TO_ERP"].ToString();
    }
    #endregion

    /// <summary>
    /// Insert & Update the data in data base
    
    #region "Insertion_Updation_Delete"
    public bool Insertion_Updation_Delete(string storeProcedure, SqlParameter[] parameterList)
    {
        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            int result = cmd.ExecuteNonQuery();

            if (result >= 0 ||result==-1)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
    }
    #endregion

    #region "Insertion_Updation_Delete Single Out Put"
    public bool Insertion_Updation_Delete(string storeProcedure, SqlParameter[] parameterList, out string message)
    {
        try
        {
            message = "";
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            SqlParameter param = new SqlParameter("@ERROR", SqlDbType.VarChar, 200);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            int result = cmd.ExecuteNonQuery();
            message = cmd.Parameters["@ERROR"].Value.ToString();
            if (result > 0)
                return true;
            else
                return false;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(storeProcedure, "Insertion_Updation_Delete", Ex.Message);
            message = "";
            return false;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
    }
    #endregion

    #region "Insertion_Updation_Delete Two Out Put"
    public bool Insertion_Updation_Delete(string storeProcedure, SqlParameter[] parameterList, out string message, out int PK_CODE)
    {
        try
        {
            message = "";
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            SqlParameter param = new SqlParameter("@ERROR", SqlDbType.VarChar, 200);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);
            SqlParameter param1 = new SqlParameter("@PK_CODE", SqlDbType.Int);
            param1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param1);

            int result = cmd.ExecuteNonQuery();
            message = cmd.Parameters["@ERROR"].Value.ToString();
            PK_CODE = Convert.ToInt32(cmd.Parameters["@PK_CODE"].Value);
            if (result > 0)
                return true;
            else
                return false;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(storeProcedure, "Insertion_Updation_Delete", Ex.Message);
            message = "";
            PK_CODE = 0;
            return false;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
        }
    }
    #endregion

    #region Return Dataset
    public DataSet SelectDataDataset(string storeprocedue, SqlParameter[] parameterList)
    {
        DataSet ds = new DataSet();
        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeprocedue, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }
        finally
        {
            ds.Dispose();
        }

    }
    #endregion

    public DataSet SelectDataDataset(string storeprocedue1, string storeprocedure2, SqlParameter[] parameterList, SqlParameter[] parameterList1, string Table1, string Table2)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        SqlCommand cmd1 = new SqlCommand();
        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();

            cmd = new SqlCommand(storeprocedue1, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds, Table1);

            cmd1 = new SqlCommand(storeprocedure2, conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            if (parameterList1 != null)
            {
                for (int index = 0; index < parameterList1.Length; index++)
                {
                    cmd1.Parameters.Add(parameterList1[index]);
                }
            }
            adapter = new SqlDataAdapter(cmd1);
            adapter.Fill(ds, Table2);


            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }
        finally
        {
            ds.Dispose();
        }

    }

    public DataSet SelectDataDatasetQuery(string Query1, string Query2, string Table1, string Table2)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        SqlCommand cmd1 = new SqlCommand();
        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();

            cmd = new SqlCommand(Query1, conn);
            cmd.CommandType = CommandType.Text;
            
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds, Table1);

            cmd1 = new SqlCommand(Query2, conn);
            cmd1.CommandType = CommandType.Text;
            
            adapter = new SqlDataAdapter(cmd1);
            adapter.Fill(ds, Table2);
            
            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }
        finally
        {
            ds.Dispose();
        }

    }

    public DataSet SelectDataDataset(string storeprocedure2, SqlParameter[] parameterList, string Table1)
    {
        DataSet ds = new DataSet();
        SqlCommand cmd1 = new SqlCommand();
        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeprocedure2, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds, Table1);


            return ds;
        }
        catch (Exception ex)
        {
            return ds;
        }
        finally
        {
            ds.Dispose();
        }

    }


    /// <summary>
    /// Return result in Data Table
    /// </summary>
    /// 


    #region Return result in Data Table
    public DataTable SelectData(string storeProcedure, SqlParameter[] parameterList, out string message, out int PK_CODE)
    {
        DataTable _dt = new DataTable();

        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            SqlParameter param = new SqlParameter("@ERROR", SqlDbType.VarChar, 200);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);
            SqlParameter param1 = new SqlParameter("@PK_CODE", SqlDbType.Int);
            param1.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param1);
            //int result = cmd.ExecuteNonQuery();
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(_dt);
            message = cmd.Parameters["@ERROR"].Value.ToString();
            PK_CODE = Convert.ToInt32(cmd.Parameters["@PK_CODE"].Value);
            return _dt;
        }
        catch (Exception ex)
        {
            PK_CODE = 0;
            message = ex.Message;
            return _dt;
        }
        finally
        {
            cmd.Dispose();
            _dt.Dispose();
            adapter.Dispose();
            conn.Close();
        }
    }
    #endregion
    
    #region Return result in Data Table
    public DataTable SelectData(string storeProcedure, SqlParameter[] parameterList)
    {
        DataTable _dt = new DataTable();

        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(_dt);
            return _dt;
        }
        catch (Exception ex)
        {
            return _dt;
        }
        finally
        {
            cmd.Dispose();
            _dt.Dispose();
            adapter.Dispose();
            conn.Close();
        }
    }
    #endregion  
  
    #region Return GetData in Data Table
    public DataTable GetData(string storeProcedure)
    {
        DataTable _dt = new DataTable();

        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(_dt);
            return _dt;
        }
        catch (Exception ex)
        {
            return _dt;
        }
        finally
        {
            cmd.Dispose();
            _dt.Dispose();
            adapter.Dispose();
            conn.Close();
        }
    }
    #endregion

    #region VerifyCredentials
    public bool VerifyCredentials(string storeProcedure, SqlParameter[] parameterList)
    {
        try
        {
            conn = new SqlConnection(strConnString);
            conn.Open();
            cmd = new SqlCommand(storeProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterList != null)
            {
                for (int index = 0; index < parameterList.Length; index++)
                {
                    cmd.Parameters.Add(parameterList[index]);
                }
            }

            int responce = cmd.ExecuteNonQuery();
            
            if (responce == 1)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
        }
    }
    #endregion

    #region SelectDataWithString
    public DataTable SelectDataWithString(string strSqlQuery)
    {
        try
        {
            //create a dataset object
            DataTable dsSelectResult = new DataTable();
            /*
            try
            {*/
            //Establish connection with the main databse using  strConnString.
            SqlConnection objNewConn = new SqlConnection(strConnString);

            //open the connection
            objNewConn.Open();

            //create a data adapter object to execute the select query
            SqlDataAdapter daSelectAdapter = new SqlDataAdapter(strSqlQuery, objNewConn);

            //fill the dataset
            daSelectAdapter.Fill(dsSelectResult);


            //close the connection
            objNewConn.Close();


            return (dsSelectResult);
        }
        catch (Exception)
        {
            return null;
        }
    }
    #endregion

    #region GetColumn
    public string GetColumn(string strQuery)
    {
        string tmp = "";
        conn = new SqlConnection(strConnString);
        cmd = new SqlCommand(strQuery, conn);
        conn.Open();
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            cmd.CommandText = strQuery;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            read = cmd.ExecuteReader();
            if (read.HasRows)
                if (read.Read())
                {
                    tmp = read[0].ToString();
                    read.Close();
                }
            // read.Close();
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            read.Close();
            conn.Close();
        }
        return tmp;
    }
    #endregion


    public bool BulkInsertDataTable(string tableName, DataTable dataTable)
    {
        bool isSuccuss;
        try
        {

            conn = new SqlConnection(strConnString);
            conn.Open();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
            bulkCopy.DestinationTableName = tableName;
           // bulkCopy.ColumnMappings.Add("ContactID", "Id");

            bulkCopy.WriteToServer(dataTable);
            isSuccuss = true;
        }
        catch (Exception ex)
        {
            isSuccuss = false;
        }
        return isSuccuss;
    }
}
