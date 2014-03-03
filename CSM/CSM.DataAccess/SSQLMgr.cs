using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using CSM.DataAccess;

namespace CSM.DataLayer
{
	public class SSQLMgr : iSQLMgr
    {
        #region Private Member Variables
        private static string connectionString = ConfigurationManager.ConnectionStrings["ApplicationDB"].ToString();
        private SqlTransaction _transaction;       
        #endregion

        #region Public Properties
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }
        #endregion

        #region Public Methods
        public SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }        
        
        /// <summary>
        /// This method is used to return DataTable.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="tableName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static public DataTable ExecuteQuery(string storedProcedureName, string tableName, Array parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(storedProcedureName, connection);
            foreach (SqlParameter param in parameters)
            {
				adapter.SelectCommand.Parameters.Add(param);
            }

            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            adapter.Fill(ds, tableName);
            return ds.Tables[tableName];
        }

        /// <summary>
        /// This method is used to return DataTable.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="tableName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static public DataSet ExecuteQuery(string storedProcedureName, Array parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(storedProcedureName, connection);
            foreach (SqlParameter param in parameters)
            {
				adapter.SelectCommand.Parameters.Add(param);
            }

            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// This method is used to insert and update records in database.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        static public int ExecuteNonQuery(string storedProcedureName, Array parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters)
            {
                command.Parameters.Add(param);
            }
            connection.Open();
            int affectedRows = command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
            command.Dispose();
            return affectedRows;
        }

        /// <summary>
        /// This method is used to insert record in database and returns identity of the new inserted row.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <returns>rowId</returns>
        static public int ExecuteScaler(string storedProcedureName, Array parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters)
            {
                command.Parameters.Add(param);
            }
            connection.Open();
            int rowId = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            connection.Dispose();
            command.Dispose();
            return rowId;
        }
        #endregion

        #region Transaction
        /// <summary>
        /// All the methods under this region will be used in case with SQL Transaction only.        
        /// </summary>

        /// <summary>
        /// This method is used to insert record in database and returns identity of the new inserted row.
        /// This method uses the Transaction object for CRUD operations
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns>rowId</returns>
        public int ExecuteScaler(string storedProcedureName, Array parameters, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(storedProcedureName, connection, Transaction);
            command.CommandType = CommandType.StoredProcedure;
            //command.Connection = connection;
            //command.Transaction = Transaction;
            foreach (SqlParameter param in parameters)
            {
                command.Parameters.Add(param);
            }
            int rowId = Convert.ToInt32(command.ExecuteScalar());
            return rowId;
        }

        /// <summary>
        /// This method is used to return DataTable.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="tableName"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteQuery(string storedProcedureName, string tableName, Array parameters, SqlConnection connection)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(storedProcedureName, connection,Transaction);
            foreach (SqlParameter param in parameters)
            {
				
                adapter.SelectCommand.Parameters.Add(param);
            }

            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            adapter.Fill(ds, tableName);
            return ds.Tables[tableName];
        }

        /// <summary>
        /// This method is used to insert & update records in database.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string storedProcedureName, Array parameters, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(storedProcedureName, connection, Transaction);
            command.CommandType = CommandType.StoredProcedure;
            //command.Connection = connection;
            //command.Transaction = Transaction;
            foreach (SqlParameter param in parameters)
            {
				
                command.Parameters.Add(param);
            }
            int affectedRows = command.ExecuteNonQuery();
            return affectedRows;
        }
        #endregion

        #region SQL Parameters
        static public SqlParameter CreateIntParameter(string name, int value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Int);
            param.Value = value;
            return param;
        }
        static public SqlParameter CreateStringParameter(string name, string value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.NVarChar);
            param.Value = value;
            return param;
        }
        static public SqlParameter CreateDateTimeParameter(string name, DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                SqlParameter param = new SqlParameter(name, SqlDbType.DateTime);
                param.Value = DBNull.Value;
                return param;
            }
            else
            {
                SqlParameter param = new SqlParameter(name, SqlDbType.DateTime);
                param.Value = value;
                return param;
            }
        }

        static public SqlParameter CreateLongParameter(string name, long value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.BigInt);
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateBoolParameter(string name, bool value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Bit);
            param.Value = value;
            return param;
        }

        static public SqlParameter CreateByteParameter(string name, byte[] value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.Image);
            param.Value = value;
            return param;
        }

        static public SqlParameter GetLongOutputParameter(string name, long value)
        {
            SqlParameter param = new SqlParameter(name, SqlDbType.BigInt);
            param.Direction = ParameterDirection.Output;
            param.Value = value;
            return param;
        }
        #endregion
    }
}
