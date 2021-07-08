using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace md_dbdocs.app.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private readonly ConfigModel _configModel;
        private SolutionObjectModel _selectedItem;
        private ObservableCollection<SolutionObjectModel> _solutionObjects;
        private ObservableCollection<ObjectChildModel> _objectChildren;

        public ObservableCollection<SolutionObjectModel> SolutionObjects { get => _solutionObjects; set { _solutionObjects = value; base.OnPropertyChanged(); } }      // objects with match
        public SolutionObjectModel SelectedItem { get => _selectedItem; set { _selectedItem = value; base.OnPropertyChanged(); } }

        public ObservableCollection<ObjectChildModel> ObjectChildren { get => _objectChildren; set { _objectChildren = value; base.OnPropertyChanged(); } }

        public RelayCommand SelectItemCommend { get; private set; }

        public int MatchedToSource { get => SolutionObjects.Count(p => p.IsServer && p.IsProject); }
        public int NotMatchedToSource { get => SolutionObjects.Count(p => p.IsServer && !p.IsProject); }
        public int NotMatchedToObject { get => SolutionObjects.Count(p => !p.IsServer && p.IsProject); }

        public DetailsViewModel(ConfigModel configModel)
        {
            this._configModel = configModel;
            this.SelectItemCommend = new RelayCommand(SelectItemExecute, SelectItemCanExecute);

            LoadDetails();
            this.SelectedItem = this.SolutionObjects[0];
        }

        private void SelectItemExecute(object obj)
        {
            var item = (SolutionObjectModel)obj;
            this.SelectedItem = item;
            this.ObjectChildren = GetChildrenInfo(item);
        }

        private ObservableCollection<ObjectChildModel> GetChildrenInfo(SolutionObjectModel solutionObjectModel)
        {
            var outList = new ObservableCollection<ObjectChildModel>();

            // loop on columns and parameters in server object and add to list
            foreach (var item in solutionObjectModel.ServerObjectModel.ChildColumns)
            {
                var child = new ObjectChildModel
                {
                    Type = "COLUMN",
                    IsServer = true,
                    Name = item.ColumnName,
                    ServerDesc = item.ExtendedDesc,
                    DataType = item.DataType
                };

                outList.Add(child);
            }

            foreach (var item in solutionObjectModel.ServerObjectModel.ChildParameters)
            {
                var child = new ObjectChildModel
                {
                    Type = "PARAMETER",
                    IsServer = true,
                    Name = item.ParameterName,
                    ServerDesc = item.ExtendedDesc,
                    DataType = item.DataType
                };

                outList.Add(child);
            }

            // loop on columns and parameters in project object, find item with the same name and extend info, or add to list if not found
            foreach (KeyValuePair<string, string> item in solutionObjectModel.ProjectObjectModel.HeaderModel.Fields)
            {
                var listItem = outList.Where(i => i.Name == item.Key).FirstOrDefault();
                if (listItem == null)
                {
                    var child = new ObjectChildModel
                    {
                        Type = "COLUMN",
                        IsProject = true,
                        Name = item.Key,
                        ProjectDesc = item.Value
                    };
                }
                else
                {
                    listItem.IsProject = true;
                    listItem.ProjectDesc = item.Value;
                }
            }

            foreach (KeyValuePair<string, string> item in solutionObjectModel.ProjectObjectModel.HeaderModel.Parameters)
            {
                var listItem = outList.Where(i => i.Name == item.Key).FirstOrDefault();
                if (listItem == null)
                {
                    var child = new ObjectChildModel
                    {
                        Type = "PARAMETER",
                        IsProject = true,
                        Name = item.Key,
                        ProjectDesc = item.Value
                    };
                }
                else
                {
                    listItem.IsProject = true;
                    listItem.ProjectDesc = item.Value;
                }
            }

            return outList;
        }

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
                }
                else
                {
                    // if not matched, add project obj as null
                    solutionObject.ServerObjectModel = obj.Value;
                    solutionObject.ProjectObjectModel = null;
                }

                solutionObj.Add(solutionObject);
            }

            // loop on all project objects if any remain
            foreach (var obj in projectObjects)
            {
                SolutionObjectModel solutionObject = new SolutionObjectModel();
                solutionObject.ProjectObjectModel = obj.Value;
                solutionObject.ServerObjectModel = null;
                solutionObj.Add(solutionObject);
            }

            // assign to properties
            this.SolutionObjects = solutionObj;
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
