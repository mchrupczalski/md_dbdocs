using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;

namespace md_dbdocs.app.ViewModels
{
    public class DetailsViewModel
    {
        private readonly ConfigModel _configModel;
        public ObservableCollection<SolutionObjectModel> SolutionObjects { get; set; }

        public DetailsViewModel(ConfigModel configModel)
        {
            this._configModel = configModel;

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

            var fs = new Services.FileScannerService(_configModel);
            Dictionary<string, ProjectObjectModel> projectObjects = fs.GetProjectObjects();

            this.SolutionObjects = MatchObjects(serverObjects, projectObjects);


        }

        private ObservableCollection<SolutionObjectModel> MatchObjects(Dictionary<string, ServerObjectParentModel> serverObjects, Dictionary<string, ProjectObjectModel> projectObjects)
        {
            ObservableCollection<SolutionObjectModel> output = new ObservableCollection<SolutionObjectModel>();

            // loop on all server objects
            foreach (var serverObj in serverObjects)
            {
                SolutionObjectModel solutionObject = new SolutionObjectModel
                {
                    // add server object
                    ServerObjectModel = serverObj.Value
                };

                // find matching projObject
                string keyServer = serverObj.Key;
                if (projectObjects.ContainsKey(keyServer))
                {
                    solutionObject.ProjectObjectModel = projectObjects[keyServer];
                    projectObjects.Remove(keyServer);
                }

                // remove from dict
                serverObjects.Remove(keyServer);

                output.Add(solutionObject);
            }

            // loop on all project objects if any remain
            foreach (var projObj in projectObjects)
            {
                SolutionObjectModel solutionObject = new SolutionObjectModel
                {
                    ProjectObjectModel = projObj.Value
                };

                projectObjects.Remove(projObj.Key);
                output.Add(solutionObject);
            }

            return output;
        }

        private Dictionary<string, ServerObjectParentModel> GetServerObjects()
        {
            const string sqlMajorObj = "GetParentObjects.sql";
            const string sqlMinorObjCols = "GetColumnsInfo.sql";
            const string sqlMinorObjCols_Replace = "<<TableId>>"; // placeholder to be replaced with value
            const string sqlMinorObjParam = "GetParametersInfo.sql";
            const string sqlMinorObjParam_Replace = "<<ProcedureId>>"; // placeholder to be replaced with value

            // load query from file
            string queryMajor = GetSqlFile(sqlMajorObj);

            Dictionary<string, ServerObjectParentModel> serverObjects = new Dictionary<string, ServerObjectParentModel>();

            using (var dal = new DataAccess.DataAccess(Helpers.ConnectionStringHelper.GetConnectionString(_configModel)))
            {
                List<ServerObjectParentModel> parentModels = dal.LoadDataModel<ServerObjectParentModel>(queryMajor);

                foreach (var item in parentModels)
                {
                    // pick a query to run based on item type
                    string queryChildren = "";
                    string replaceTag = "";
                    string query = "";
                    switch (item.ObjectId.ToUpper())
                    {
                        case "FN":      // SQL_SCALAR_FUNCTION
                        case "IF":      // SQL_INLINE_TABLE_VALUED_FUNCTION
                        case "TF":      // SQL_TABLE_VALUED_FUNCTION
                        case "P":       // SQL_STORED_PROCEDURE
                            queryChildren = sqlMinorObjParam;
                            replaceTag = sqlMinorObjParam_Replace;
                            query = GetSqlFile(queryChildren).Replace(replaceTag, item.ObjectId);
                            item.ChildParameters = dal.LoadDataModel<ServerObjectChildParameterModel>(query);
                            break;
                        case "U":       // USER_TABLE
                        case "V":       // VIEW
                        case "TTYPE":   // TABLE_TYPE
                            queryChildren = sqlMinorObjCols;
                            replaceTag = sqlMinorObjCols_Replace;
                            query = GetSqlFile(queryChildren).Replace(replaceTag, item.ObjectId);
                            item.ChildColumns = dal.LoadDataModel<ServerObjectChildColumnModel>(query);
                            break;
                        default:
                            break;
                    }

                    string key = $"{ item.SchemaName.ToLower() }.{ item.ObjectName.ToLower() }";
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
