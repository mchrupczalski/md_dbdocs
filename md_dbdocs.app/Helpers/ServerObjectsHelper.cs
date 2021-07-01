using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Helpers
{
    public class ServerObjectsHelper
    {
        private readonly SqlConnection _sqlConnection;
        private readonly string _dbSchema;
        private readonly string _connectionString;

        public ServerObjectsHelper(string connectionString)
        {
            _dbSchema = "dbdocs";
            this._connectionString = connectionString;
        }

        public bool CreateServerObjects()
        {
            bool allCreated = false;
            // clear old objects
            ClearSchema();

            // create schema
            // create tables
            // create procedure to upload project files details

            return allCreated;
        }

        private void ClearSchema()
        {
            const string spClearName = "dbo.dbdocs_CleanUpSchema";
            const string sqlClearProcFileName = "dbo.dbdocs_CleanUpSchema.sql";
            
            string execDelProc = $"DROP PROCEDURE IF EXISTS { spClearName }";

            string sqlClearProcFilePath = Environment.CurrentDirectory + "\\SQL\\" + sqlClearProcFileName;

            FileInfo procFile = new FileInfo(sqlClearProcFilePath);
            string createProcedure = procFile.OpenText().ReadToEnd();

            
            using (var dal = new DataAccess.DataAccess(_connectionString))
            {
                // remove procedure if already exists
                dal.ExecuteText(execDelProc);

                // create procedure for clearin schema
                dal.ExecuteText(createProcedure);

                // clear schema
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SchemaName",_dbSchema),
                    new SqlParameter("@WorkTest","w")
                };
                dal.Execute(spClearName, parameters);

                // remove procedure
                dal.ExecuteText(execDelProc);
            }
        }
    }
}
