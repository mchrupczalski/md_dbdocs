using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Helpers
{
    public class ServerObjectsHelper
    {
        private readonly SqlConnection _sqlConnection;

        public ServerObjectsHelper(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public bool CreateServerObjects()
        {
            bool allCreated = false;

            // create schema
            // create tables
            // create procedure to upload project files details

            return allCreated;
        }
    }
}
