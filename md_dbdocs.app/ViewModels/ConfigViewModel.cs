using md_dbdocs.app.DataAccess;
using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using md_dbdocs.app.Services;
using md_dbdocs.app.Views;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace md_dbdocs.app.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        private const string _configFileName = "config.json";
        private string _jsonConfigPath = Environment.CurrentDirectory + "\\" + _configFileName;

        private ConfigModel _config;
        public ConfigModel Config { get => _config; set { _config = value; base.OnPropertyChanged(); } }

        public RelayCommand ValidateCommand { get; private set; }
        public RelayCommand NavNextCommand { get; private set; }
        public RelayCommand BrowseCommand { get; set; }


        public ConfigViewModel()
        {
            this.Config = LoadConfig();
            Config.Validation = new ObservableCollection<ConfigValidationModel>();
            this.ValidateCommand = new RelayCommand(ValidateExecute, ValidateCanExecute);
            this.NavNextCommand = new RelayCommand(NavNextExecute, NavNextCanExecute);
            this.BrowseCommand = new RelayCommand(BrowseExecute, BrowseCanExecute);
        }

        private void BrowseExecute(object obj = null)
        {
            string path = "";
            var dialog = new OpenFileDialog();
            dialog.Filter = "VS SQL Server Project | *.sqlproj";
            dialog.Title = "Select sql project path.";
            dialog.InitialDirectory = "C:\\";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = System.IO.Directory.GetParent(dialog.FileName).ToString();
            }

            Config.SqlProjectRootPath = path;
        }

        private bool BrowseCanExecute(object obj) => true;

        private void NavNextExecute(object obj)
        {
            SaveConfig(Config);

            var detailsVm = new DetailsViewModel(Config);
            ViewNavigationService.ViewNav.Navigate(new DetailsView(detailsVm));
        }

        private void SaveConfig(ConfigModel config)
        {
            string configJson;

            using (var s = new Helpers.JsonSerializer())
            {
                configJson = s.ToJson<ConfigModel>(config);
            }

            // delete existing file
            File.Delete(_jsonConfigPath);

            // create new file
            File.WriteAllText(_jsonConfigPath, configJson);
        }

        private bool NavNextCanExecute(object obj)
        {
            bool isConnected = _config.Validation.Where(m => m.ItemId == "db").Select(m => m.IsValid).FirstOrDefault();
            bool isDbo = _config.Validation.Where(m => m.ItemId == "dbo").Select(m => m.IsValid).FirstOrDefault();
            bool isProjectPath = _config.Validation.Where(m => m.ItemId == "root").Select(m => m.IsValid).FirstOrDefault();

            return isConnected && isDbo && isProjectPath;
        }

        private bool ValidateCanExecute(object obj) => true;

        private void ValidateExecute(object obj)
        {
            const string cmdNode = "node -v";
            const string cmdNpm = "npm -v";
            const string cmdMdi = "npm list -g markdown-include";

            Config.Validation.Clear();

            // check database connection
            var validateDb = ValidateDbConnection();
            Config.Validation.Add(validateDb);

            // check if user is dbo
            var validateUser = new ConfigValidationModel();
            validateUser.ItemId = "dbo";
            validateUser.ValidationItem = "User is db_owner";
            if (validateDb.IsValid)
            {
                var dbCnx = new SqlServerDataAccess(Config);
                dbCnx.ConnectToDb();
                Services.DataService dataService = new DataService(dbCnx.DbConnection);
                validateUser.IsValid = dataService.IsUserDbo();
            }
            else
            {
                validateUser.IsValid = false;
                validateUser.ExtMessage = "User is not a member of db_owner role or connection could not be established.";
            }
            Config.Validation.Add(validateUser);

            // check sql root path
            var validateProjectPath = new ConfigValidationModel();
            validateProjectPath.ItemId = "root";
            validateProjectPath.ValidationItem = "SQL Project root path";
            validateProjectPath.IsValid = !string.IsNullOrEmpty(Config.SqlProjectRootPath);
            Config.Validation.Add(validateProjectPath);

            // check markdown-include config file

            // check prerequisite apps
            Config.Validation.Add(ValidateApps("node", "Node.js", cmdNode));
            Config.Validation.Add(ValidateApps("npm", "npm", cmdNpm));
            Config.Validation.Add(ValidateApps("mdi", "markdown-include", cmdMdi));
        }

        private ConfigModel LoadConfig()
        {
            ConfigModel config = new ConfigModel();
            bool loaded = false;
            int retry = 0;

            while (!loaded && retry < 3)
            {
                try
                {
                    StreamReader reader = new StreamReader(_jsonConfigPath);
                    string jsonText = reader.ReadToEnd();

                    using (var s = new JsonSerializer())
                    {
                        config = s.FromJson<ConfigModel>(jsonText);
                    }

                    break;
                }
                catch (FileNotFoundException)
                {
                    // create blank config file
                    SaveConfig(new ConfigModel());
                    retry++;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            

            // create blank config file if config don't exists or serializer returned null


            return config;
        }

        private ConfigValidationModel ValidateDbConnection()
        {
            var vldt = new ConfigValidationModel();

            using (var dbCnx = new DataAccess.DataAccess(ConnectionStringHelper.GetConnectionString(Config)))
            {
                vldt.ItemId = "db";
                vldt.ValidationItem = "Database connection.";

                try
                {
                    vldt.IsValid = dbCnx.IsConnectionValid();
                    vldt.ExtMessage = "Connected.";
                }
                catch (Exception ex)
                {
                    vldt.ExtMessage = ex.Message;
                }
            };

            return vldt;
        }

        private ConfigValidationModel ValidateApps(string itemId, string appName, string cmdParameter)
        {
            string appV = CmdHelper.GetCmdOutput(cmdParameter);
            bool gotApp = !string.IsNullOrEmpty(appV);

            var appVldt = new ConfigValidationModel();
            appVldt.ItemId = itemId;
            appVldt.ValidationItem = appName;
            appVldt.IsValid = gotApp;
            appVldt.ExtMessage = appV;

            return appVldt;
        }

    }
}
