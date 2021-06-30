using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.DataAccess
{
    public class DataAccess : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqlConnection _sqlConnection;

        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
            _sqlConnection = new SqlConnection(_connectionString);
        }

        public void Dispose(){}

        /// <summary>
        /// Run text query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SqlDataReader Query(string query)
        {
            SqlDataReader reader;
                       
            SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandTimeout = 300;

            try
            {
                _sqlConnection.Open();
                reader = sqlCommand.ExecuteReader();
                _sqlConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reader;
        }
    
        /// <summary>
        /// Validate connection
        /// </summary>
        /// <returns></returns>
        public bool IsConnectionValid()
        {
            bool isValid = false;

            try
            {
                _sqlConnection.Open();
                _sqlConnection.Close();
                isValid = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return isValid;
        }
    }
}