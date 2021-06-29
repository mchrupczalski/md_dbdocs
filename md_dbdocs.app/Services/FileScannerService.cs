using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Services
{
    public class FileScannerService
    {
        private readonly string _sqlProjectRootPath;
        private readonly SqlConnection _sqlConnection;

        public FileScannerService(string sqlProjectRootPath, SqlConnection sqlConnection)
        {
            this._sqlProjectRootPath = sqlProjectRootPath;
            this._sqlConnection = sqlConnection;
        }

        public List<SqlObjectDetailsModel> GetSqlObjectDetailsModels()
        {
            var objList = new List<SqlObjectDetailsModel>();

            // get list of files
            DirectoryInfo sqlProjInfo = new DirectoryInfo(this._sqlProjectRootPath);
            FileInfo[] fileInfos = sqlProjInfo.GetFiles("*.sql", SearchOption.AllDirectories);

            foreach (FileInfo file in fileInfos)
            {
                // second filter on sql files required as GetFiles also returning sqlproject, etc
                if (file.Extension == ".sql")
                {
                    var projFile = new SqlObjectDetailsModel();
                    projFile.FileInfo = file;

                    // check if CREATE statement
                    GetSqlCreateDetails(projFile);
                }
            }

            return objList;
        }

        private void GetSqlCreateDetails(SqlObjectDetailsModel projFile)
        {
            StreamReader reader = projFile.GetFileText();
            string line;
            int createPos;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                createPos = line.ToUpper().IndexOf("CREATE");
                if (createPos > -1)
                {
                    // remove any double spacing
                    line = ReplaceAllString("  ", " ", line);
                    string[] lineSplit = line.Split(' ');

                    string schemaAndName = lineSplit[2];
                }
            }

        }

        private string ReplaceAllString(string findOld, string replaceWith, string searchIn)
        {
            while (searchIn.IndexOf(findOld) > -1)
            {
                searchIn.Replace(findOld, replaceWith);
            }

            return searchIn;
        }
    }
}
