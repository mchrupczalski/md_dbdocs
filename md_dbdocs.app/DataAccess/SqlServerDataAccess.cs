using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.DataAccess
{
    internal class SqlServerDataAccess : IDisposable
    {
        private readonly ConfigModel config;

        public SqlConnection DbConnection { get; set; }
        public bool IsConnected { get; set; }

        public SqlServerDataAccess(ConfigModel config) => this.config = config;

        public void CreateDbObjects()
        {
            //throw new NotImplementedException();
        }

        private void ClearDbObjects()
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            // if is connected, clear db objects and close connection
            if (this.IsConnected)
            {
                //ClearDbObjects();
                this.DbConnection.Close();
            }
        }

        /// <summary>
        /// Generate connection string based on config file
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string cnxString;

            if (config.UseWindowsAuth)
            {
                cnxString = $"Server={ config.Server };" +
                            $"Database={ config.DataBase };" +
                            $"Trusted_Connection=True;";
            }
            else
            {
                cnxString = $"Server={ config.Server };" +
                            $"Database={ config.DataBase };" +
                            $"User Id={ config.SqlUserName };" +
                            $"Password={ config.SqlPassword };";
            }

            return cnxString;
        }

        /// <summary>
        /// Establish connection to database
        /// </summary>
        /// <returns></returns>
        public void ConnectToDb()
        {
            SqlConnection sqlConnection = new SqlConnection(GetConnectionString());
            try
            {
                sqlConnection.Open();
                DbConnection = sqlConnection;
                IsConnected = true;
            }
            catch (Exception ex)
            {
                DbConnection = null;
                IsConnected = false;
                throw ex;
            }

        }
    }
}
