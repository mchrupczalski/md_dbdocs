using md_dbdocs.app.DataAccess;
using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using md_dbdocs.app.Services;
using md_dbdocs.app.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace md_dbdocs.app.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        private const string configFileName = "config.json";
        private string jsonConfigPath = Environment.CurrentDirectory + "\\" + configFileName;

        private ConfigModel config;
        public ConfigModel Config { get => config; set { config = value; base.OnPropertyChanged(); } }

        public RelayCommand ValidateCommand { get; private set; }
        public RelayCommand NavNextCommand { get; private set; }


        public ConfigViewModel()
        {
            this.Config = LoadConfig();
            Config.Validation = new ObservableCollection<ConfigValidationModel>();
            this.ValidateCommand = new RelayCommand(ValidateExecute, ValidateCanExecute);
            this.NavNextCommand = new RelayCommand(NavNextExecute, NavNextCanExecute);
        }

        private void NavNextExecute(object obj)
        {
            var dbCnx = new SqlServerDataAccess(Config);
            dbCnx.ConnectToDb();
            var detailsVm = new DetailsViewModel(Config, dbCnx.DbConnection);
            ViewNavigationService.ViewNav.Navigate(new DetailsView(detailsVm));
        }

        private bool NavNextCanExecute(object obj)
        {
            bool isConnected = config.Validation.Where(m => m.ItemId == "db").Select(m => m.IsValid).FirstOrDefault();
            bool isDbo = config.Validation.Where(m => m.ItemId == "dbo").Select(m => m.IsValid).FirstOrDefault();

            return isConnected && isDbo;
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
            // check markdown-include config file

            // check prerequisite apps
            Config.Validation.Add(ValidateApps("node", "Node.js", cmdNode));
            Config.Validation.Add(ValidateApps("npm", "npm", cmdNpm));
            Config.Validation.Add(ValidateApps("mdi", "markdown-include", cmdMdi));
        }

        private ConfigModel LoadConfig()
        {
            StreamReader reader = new StreamReader(jsonConfigPath);
            string jsonText = reader.ReadToEnd();

            ConfigModel config;
            using (var s = new JsonSerializer())
            {
                config = s.FromJson<ConfigModel>(jsonText);
            }

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
