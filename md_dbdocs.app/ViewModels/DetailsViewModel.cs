using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace md_dbdocs.app.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private readonly ConfigModel _configModel;
        private ObservableCollection<SolutionObjectModel> _selectedList;
        private SolutionObjectModel _selectedItem;

        public ObservableCollection<SolutionObjectModel> SolutionObjects { get; set; }      // objects with match
        public ObservableCollection<SolutionObjectModel> ServerObjects { get; set; }        // server objects without match
        public ObservableCollection<SolutionObjectModel> ProjectObjects { get; set; }       // project objects without match
        public ObservableCollection<SolutionObjectModel> SelectedList { get => _selectedList; set { _selectedList = value; base.OnPropertyChanged(); } }
        public SolutionObjectModel SelectedItem { get => _selectedItem; set { _selectedItem = value; base.OnPropertyChanged(); } }

        public RelayCommand ChangeListCommand { get; private set; }
        public RelayCommand SelectItemCommend { get; private set; }

        public DetailsViewModel(ConfigModel configModel)
        {
            this._configModel = configModel;
            this.ChangeListCommand = new RelayCommand(ChangeListExecute, ChangeListCanExecute);
            this.SelectItemCommend = new RelayCommand(SelectItemExecute, SelectItemCanExecute);

            LoadDetails();
            this.SelectedList = this.SolutionObjects;
            this.SelectedItem = this.SelectedList[0];
        }



        private void ChangeListExecute(object obj) => this.SelectedList = (ObservableCollection<SolutionObjectModel>)obj;
        private void SelectItemExecute(object obj) => this.SelectedItem = (SolutionObjectModel)obj;
        private bool ChangeListCanExecute(object obj) => true;
        private bool SelectItemCanExecute(object obj) => true;

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

            MatchObjects(serverObjects, projectObjects);

        }

        /// <summary>
        /// Matches server and project objects.
        /// </summary>
        /// <param name="serverObjects">The server objects.</param>
        /// <param name="projectObjects">The project objects.</param>
        private void MatchObjects(Dictionary<string, ServerObjectParentModel> serverObjects, Dictionary<string, ProjectObjectModel> projectObjects)
        {
            ObservableCollection<SolutionObjectModel> solutionObj = new ObservableCollection<SolutionObjectModel>();
            ObservableCollection<SolutionObjectModel> serverObj = new ObservableCollection<SolutionObjectModel>();
            ObservableCollection<SolutionObjectModel> projectObj = new ObservableCollection<SolutionObjectModel>();

            // loop on all server objects
            foreach (var obj in serverObjects)
            {
                SolutionObjectModel solutionObject = new SolutionObjectModel();

                // find matching projObject
                string keyServer = obj.Key;
                if (projectObjects.ContainsKey(keyServer))
                {
                    // if matched add both object to solution model, and add solution model to collection
                    solutionObject.ServerObjectModel = obj.Value;
                    solutionObject.ProjectObjectModel = projectObjects[keyServer];

                    projectObjects.Remove(keyServer);
                    solutionObj.Add(solutionObject);
                }
                else
                {
                    // if not matched, add server obj to not matched collection
                    solutionObject.ServerObjectModel = obj.Value;
                    serverObj.Add(solutionObject);
                }
            }

            // loop on all project objects if any remain, and add to not matched project objects collection
            foreach (var obj in projectObjects)
            {
                SolutionObjectModel solutionObject = new SolutionObjectModel();
                solutionObject.ProjectObjectModel = obj.Value;
                projectObj.Add(solutionObject);
            }

            // assign to properties
            this.SolutionObjects = solutionObj;
            this.ServerObjects = serverObj;
            this.ProjectObjects = projectObj;
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
                    switch (item.ObjectTypeId.ToUpper().Trim())
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
                            queryChildren = sqlMinorObjCols;
                            replaceTag = sqlMinorObjCols_Replace;
                            query = GetSqlFile(queryChildren).Replace(replaceTag, item.ObjectId);
                            item.ChildColumns = dal.LoadDataModel<ServerObjectChildColumnModel>(query);
                            break;
                        case "TTYPE":   // TABLE_TYPE
                            queryChildren = sqlMinorObjCols;
                            replaceTag = sqlMinorObjCols_Replace;
                            query = GetSqlFile(queryChildren).Replace(replaceTag, item.UserTypeId);
                            item.ChildColumns = dal.LoadDataModel<ServerObjectChildColumnModel>(query);
                            break;
                        default:
                            break;
                    }

                    string key = $"{ Helpers.SqlObjectTypeTranslator.GetObjectTypeId(item.ObjectTypeId) }.{ item.SchemaName.ToLower() }.{ item.ObjectName.ToLower() }";
                    Debug.Print(key);
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
