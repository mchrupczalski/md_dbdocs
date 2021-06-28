using DbDocsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.DataAccess
{
    internal class DataAccessService : IDisposable
    {
        private readonly ConfigModel config;
        private readonly string cnxString;

        public SqlConnection DbConnection { get; set; }
        public bool IsConnected { get; set; }

        internal DataAccessService(ConfigModel config)
        {
            this.config = config;

            Console.Clear();

            // get connection string
            Console.WriteLine("Generating connection string from the configuration.");
            this.cnxString = GetConnectionString();

            // open and validate connection
            Console.WriteLine("Establishing connection.");
            this.DbConnection = ConnectToDb();

            // create database objects
            if (IsConnected)
            {
                Console.WriteLine("Creating database objects required for the tool.");
                //CreateDbObjects();
            }
            else
            {
                Console.WriteLine("Database connection could not be established.");
            }
        }

        private void CreateDbObjects()
        {
            throw new NotImplementedException();
        }

        private void ClearDbObjects()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // if is connected, clear db objects and close connection
            if (this.IsConnected)
            {
                ClearDbObjects();
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
                Console.WriteLine("Opening connection with Windows Authorization.");
                cnxString = $"Server={ config.Server };" +
                            $"Database={ config.DataBase };" +
                            $"Trusted_Connection=True;";
            }
            else
            {
                Console.WriteLine("Opening connection with SQL Authorization.");
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
        private SqlConnection ConnectToDb()
        {
            SqlConnection sqlConnection = new SqlConnection(this.cnxString);
            try
            {
                sqlConnection.Open();
                IsConnected = true;
                Console.WriteLine("Connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsConnected = false;
                return null;
            }

            return sqlConnection;
        }
    }
}
