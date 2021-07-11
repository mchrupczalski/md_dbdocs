using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using md_dbdocs.app.Models.YamlModel;
using md_dbdocs.app.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

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
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand GenerateDocsCommand { get; set; }

        public int MatchedToSource { get => SolutionObjects.Count(p => p.IsServer && p.IsProject); }
        public int NotMatchedToSource { get => SolutionObjects.Count(p => p.IsServer && !p.IsProject); }
        public int NotMatchedToObject { get => SolutionObjects.Count(p => !p.IsServer && p.IsProject); }

        public bool IgnoreUnreviewed { get; set; }

        public DetailsViewModel(ConfigModel configModel)
        {
            this._configModel = configModel;
            this.SelectItemCommend = new RelayCommand(SelectItemExecute, SelectItemCanExecute);
            this.EditCommand = new RelayCommand(EditExecute, EditCanExecute);
            this.SaveCommand = new RelayCommand(SaveExecute, SaveCanExecute);
            this.GenerateDocsCommand = new RelayCommand(GenerateDocsExecute, GenerateDocsCanExecute);

            LoadDetails();
            this.SelectedItem = this.SolutionObjects[0];
        }

        private void GenerateDocsExecute(object obj)
        {
            // create view model for GenerateDocsView and navigate to view
        }

        private bool GenerateDocsCanExecute(object obj)
        {
            bool allReviewed = SolutionObjects.Count(p => p.IsReviewed) == this.MatchedToSource;
            return IgnoreUnreviewed ? true : allReviewed;
        }

        private void SaveExecute(object obj)
        {
            string schemaName = SelectedItem.ServerObjectModel.SchemaName;
            string objectName = SelectedItem.ServerObjectModel.ObjectName;
            string objectType = SelectedItem.ProjectObjectModel.CreateObjectType;
            string oldDesc = SelectedItem.ServerObjectModel.ExtendedDesc;
            string newDesc = SelectedItem.ProjectObjectModel.HeaderModel.Description;

            string addExtPropParent = $"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{ newDesc }' , @level0type=N'SCHEMA',@level0name=N'{ schemaName }', @level1type=N'{ objectType }',@level1name=N'{ objectName }'";
            string updExtPropParent = $"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'{ newDesc }' , @level0type=N'SCHEMA',@level0name=N'{ schemaName }', @level1type=N'{ objectType }',@level1name=N'{ objectName }'";

            
            using (var dal = new DataAccess.DataAccess(Helpers.ConnectionStringHelper.GetConnectionString(_configModel)))
            {
                // update object description
                string query = string.IsNullOrEmpty(oldDesc) ? addExtPropParent : updExtPropParent;
                dal.ExecuteText(query);

                // update fields/parameters description
                foreach (var item in ObjectChildren)
                {
                    string childType = item.Type;
                    string childName = childType == "PARAMETER" ? $"@{ item.Name }" : item.Name;
                    string oldChildDesc = item.ServerDesc;
                    string newChildDesc = item.ProjectDesc;

                    string addExtPropChild = $"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{ newChildDesc }', @level0type=N'SCHEMA', @level0name=N'{ schemaName }', @level1type=N'{ objectType }', @level1name=N'{ objectName }', @level2type=N'{ childType }', @level2name=N'{ childName }'";
                    string updExtPropChild = $"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'{ newChildDesc }', @level0type=N'SCHEMA', @level0name=N'{ schemaName }', @level1type=N'{ objectType }', @level1name=N'{ objectName }', @level2type=N'{ childType }', @level2name=N'{ childName }'";

                    string queryChild = string.IsNullOrEmpty(oldChildDesc) ? addExtPropChild : updExtPropChild;
                    dal.ExecuteText(queryChild);
                }
            }

            // mark as reviewed
            SelectedItem.IsReviewed = true;
            for (int i = 0; i < SolutionObjects.Count; i++)
            {
                var item = SolutionObjects[i];
                bool isProj = item.ProjectObjectModel != null;
                if (isProj && item.ProjectObjectModel.FileInfo.FullName == SelectedItem.ProjectObjectModel.FileInfo.FullName)
                {
                    SolutionObjects[i].IsReviewed = true;
                    break;
                }
            }
        }

        private bool SaveCanExecute(object obj)
        {
            // can save when source and target are known
            return SelectedItem.IsServer && SelectedItem.IsProject;
        }

        private void EditExecute(object obj)
        {
            // open dialog window with partial info for the header (object schema, name, server description of objects (if known), list of fields/parameters
            var editModel = new EditSourceViewModel(SelectedItem);
            var editView = new EditSourceView(editModel);
            Window editWindow = new Window();
            editWindow.Content = editView;
            editWindow.Width = 900;
            editWindow.Height = 600;
            editWindow.ShowDialog();

            // after dialog closes rescan the file
            var fs = new Services.FileScannerService();
            fs.GetDbDocsTags(SelectedItem.ProjectObjectModel);

            // update fields / parameters
            this.ObjectChildren = GetChildrenInfo(SelectedItem);

            // update item in collection
            foreach (var item in SolutionObjects)
            {
                bool isModel = item.ProjectObjectModel != null;
                if (isModel && item.ProjectObjectModel.FileInfo.FullName == SelectedItem.ProjectObjectModel.FileInfo.FullName)
                {
                    item.ProjectObjectModel = SelectedItem.ProjectObjectModel;
                    break;
                }
            }
        }

        private bool EditCanExecute(object obj)
        {
            // can execute when source file is known
            return SelectedItem.IsProject;
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

            // define lists and dictionaries, either use from models or create new, to avoid null reference exception
            ServerObjectParentModel serverObject = solutionObjectModel.ServerObjectModel ?? new ServerObjectParentModel();
            ProjectObjectModel projectObject = solutionObjectModel.ProjectObjectModel ?? new ProjectObjectModel();
            HeaderCommentModel commentModel = projectObject.HeaderModel ?? new HeaderCommentModel();

            List<ServerObjectChildColumnModel> serverCols = serverObject.ChildColumns ?? new List<ServerObjectChildColumnModel>();
            List<ServerObjectChildParameterModel> serverParams = serverObject.ChildParameters ?? new List<ServerObjectChildParameterModel>();
            Dictionary<string, string> projCols = commentModel.Fields ?? new Dictionary<string, string>();
            Dictionary<string, string> projParams = commentModel.Parameters ?? new Dictionary<string, string>();


            // loop on columns and parameters in server object and add to list
            foreach (var item in serverCols)
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

            foreach (var item in serverParams)
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
            foreach (KeyValuePair<string, string> item in projCols)
            {
                var listItem = outList.FirstOrDefault(i => i.Name == item.Key);
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

            foreach (KeyValuePair<string, string> item in projParams)
            {
                var listItem = outList.FirstOrDefault(i => i.Name == item.Key);
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
