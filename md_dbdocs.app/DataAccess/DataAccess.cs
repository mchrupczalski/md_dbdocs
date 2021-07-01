using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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


        /// <summary>Executes the specified procedure name.</summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="sqlParameters">The SQL parameters.</param>
        public void Execute(string procedureName, List<SqlParameter> sqlParameters = null)
        {
            // connection
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 300;

            // add parameters
            if (sqlParameters.Count > 0)
            {
                foreach (SqlParameter parameter in sqlParameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                }
            }

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;

                /* ToDo: Handle Errors
                 * "There is already an object named 'dbdocs_CleanUpSchema' in the database."

                    "Cannot drop the schema 'dbdocs', because it does not exist or you do not have permission.\r\nDROP SCHEMA dbdocs\r\n------- ALL - DONE -------"

                 * 
                 */
            }
        }


        /// <summary>Executes the text query.</summary>
        /// <param name="query">The query.</param>
        public void ExecuteText(string query)
        {
            // connection
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlCommand = new SqlCommand(@query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandTimeout = 300;

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
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