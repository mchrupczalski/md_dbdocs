using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.Helpers
{
    internal class DbConnectionHelper
    {
        private readonly string server;
        private readonly string database;
        private readonly bool useWinAuth;
        private readonly string sqlUser;
        private readonly string sqlPass;

        public DbConnectionHelper(string server, string database, bool useWinAuth)
        {
            this.server = server;
            this.database = database;
            this.useWinAuth = useWinAuth;
        }

        public DbConnectionHelper(string server, string database, string sqlUser, string sqlPass)
        {
            this.server = server;
            this.database = database;
            this.sqlUser = sqlUser;
            this.sqlPass = sqlPass;
        }

        internal string GetAndConnectionString()
        {
            string cnxString;

            if (this.useWinAuth)
            {
                cnxString = $"Server={ this.server };" +
                            $"Database={ this.database };" +
                            $"Trusted_Connection=True;";
            }
            else
            {
                cnxString = $"Server={ this.server };" +
                            $"Database={ this.database };" +
                            $"User Id={ this.sqlUser };" +
                            $"Password={ this.sqlPass };";
            }

            return cnxString;
        }

        internal bool IsConnectionValid(string cnxString)
        {
            bool result;
            SqlConnection sqlConnection = new SqlConnection(cnxString);
            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }

            return result;
        }
    }
}
