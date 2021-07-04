using md_dbdocs.app.Models;
using md_dbdocs.app.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace md_dbdocs.app.ViewModels
{
    public class DetailsViewModel
    {
        private readonly ConfigModel _configModel;
        private readonly SqlConnection _sqlConnection;

        public DetailsViewModel(ConfigModel configModel, SqlConnection sqlConnection)
        {
            this._configModel = configModel;
            _sqlConnection = sqlConnection;

            LoadDetails();
        }

        public void LoadDetails()
        {
            /* create table in db and populate with object details
             * create list of all *.sql files in project folder and subfolders
             * check each file for tags and assign bool val if found
             * if dbdocs found, extract yaml and serialise to DetailsModel
             * find CREATE statement and determine object schema and name
             * send to sql server 
             */

            /*
             * scan all files in project directory
             * if CREATE statement found in file
             * extract schema and name of the created object
             * add to list of FileDetailsModel
             * check if tag dbdocs in file
             * if found deserialize to HeaderModel
             * get object details from the server and add to ObjectModel
             */

            /*
             * Create dictionary with db objects, concat schema and object name for key
             * Create dictionary with project files details
             * Create Observable Collection of objects from both dictionaries
             */
            Dictionary<string, ServerObjectParentModel> serverObjects = GetServerObjects();


            //var scanner = new FileScannerService(_configModel);
            //scanner.PopulateProjectFilesInfoTable();
        }

        private Dictionary<string, ServerObjectParentModel> GetServerObjects()
        {
            const string sqlMajorObj = "GetParentObjects.sql";
            const string sqlMinorObjCols = "GetColumnsInfo.sql";
            const string sqlMinorObjCols_Replace = "<<TableId>>"; // placeholder to be replaced with value

            // load query from file
            string queryMajor = GetSqlFile(sqlMajorObj);

            Dictionary<string, ServerObjectParentModel> serverObjects = new Dictionary<string, ServerObjectParentModel>();

            using (var dal = new DataAccess.DataAccess(Helpers.ConnectionStringHelper.GetConnectionString(_configModel)))
            {
                List<ServerObjectParentModel> parentModels = dal.LoadDataModel<ServerObjectParentModel>(queryMajor);

                foreach (var item in parentModels)
                {
                    // load object children details
                    string queryColumns = GetSqlFile(sqlMinorObjCols).Replace(sqlMinorObjCols_Replace, item.ObjectId);
                    item.ChildObjects = dal.LoadDataModel<ServerObjectChildModel>(queryColumns);

                    string key = $"{ item.SchemaName }.{ item.ObjectName }";
                    serverObjects.Add(key, item);
                }
            }

            return serverObjects;
        }

        private string GetSqlFile(string fileName)
        {
            string sqlFilePath = Environment.CurrentDirectory + "\\SQL\\" + fileName;
            FileInfo procFile = new FileInfo(sqlFilePath);
            return procFile.OpenText().ReadToEnd();
        }
    }
}
