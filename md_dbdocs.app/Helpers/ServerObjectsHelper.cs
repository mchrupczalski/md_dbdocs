using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

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
            const string sqlObjTypesTableName = "dbdocs.ObjectTypes.sql";
            const string sqlMajorTableName = "dbdocs.MajorObjectsInfo.sql";
            const string sqlMinorTableName = "dbdocs.MinorObjectsInfo.sql";
            const string sqlDbDocsMajor = "dbdocs.DbDocsMajorObjects.sql";
            const string sqlDbDocsMinor = "dbdocs.DbDocsMinorObjects.sql";
            const string sqlAddDbdMajor = "dbdocs.spAddDbDocsMajorObjects.sql";
            const string sqlAddDbdMinor = "dbdocs.spAddDbDocsMinorObjects.sql";

            bool allCreated = false;
            // clear old objects
            ClearSchema();

            // make list of scripts to execute
            List<string> scripts = new List<string>();

            // create schema
            scripts.Add("CREATE SCHEMA [dbdocs]");

            // create tables
            scripts.Add(GetSqlFile(sqlObjTypesTableName));
            scripts.Add(GetSqlFile(sqlMajorTableName));
            scripts.Add(GetSqlFile(sqlMinorTableName));
            scripts.Add(GetSqlFile(sqlDbDocsMajor));
            scripts.Add(GetSqlFile(sqlDbDocsMinor));

            // create procedure to upload project files details
            scripts.Add(GetSqlFile(sqlAddDbdMajor));
            scripts.Add(GetSqlFile(sqlAddDbdMinor));

            // execute scripts
            ExecuteSqlScripts(scripts);


            // ToDo: Add wrapper try and return true if no errors
            return allCreated;
        }

        /// <summary>
        /// Gets the SQL file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>File content as string</returns>
        private string GetSqlFile(string fileName)
        {
            string sqlFilePath = Environment.CurrentDirectory + "\\SQL\\" + fileName;
            FileInfo procFile = new FileInfo(sqlFilePath);
            return procFile.OpenText().ReadToEnd();
        }

        /// <summary>
        /// Executes the SQL scripts.
        /// </summary>
        /// <param name="sqlScripts">The SQL scripts.</param>
        /// <returns></returns>
        private void ExecuteSqlScripts(List<string> sqlScripts)
        {
            using (var dal = new DataAccess.DataAccess(_connectionString))
            {
                foreach (var script in sqlScripts)
                {
                    dal.ExecuteText(script);
                }
            }
        }

        /// <summary>
        /// Clears the schema.
        /// </summary>
        private void ClearSchema()
        {
            const string spClearName = "dbo.dbdocs_CleanUpSchema";
            const string sqlClearProcFileName = "dbo.dbdocs_CleanUpSchema.sql";

            string execDelProc = $"DROP PROCEDURE IF EXISTS { spClearName }";
            string createProcedure = GetSqlFile(sqlClearProcFileName);

            using (var dal = new DataAccess.DataAccess(_connectionString))
            {
                // remove procedure if already exists
                dal.ExecuteText(execDelProc);

                // create procedure for clearing schema
                dal.ExecuteText(createProcedure);

                // clear schema
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SchemaName",_dbSchema),
                    new SqlParameter("@WorkTest","w")
                };

                var outputParam = new SqlParameter() { ParameterName = "@OutMsg", Direction = System.Data.ParameterDirection.Output, SqlDbType = System.Data.SqlDbType.NVarChar, Size = 4000 };

                string clearingResult = (string)dal.ExecuteProcedureWithOutput(spClearName, outputParam, parameters);
                MessageBox.Show(clearingResult);

                // remove procedure
                dal.ExecuteText(execDelProc);
            }
        }
    }
}
