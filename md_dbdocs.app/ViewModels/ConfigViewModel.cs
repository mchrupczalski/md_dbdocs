using md_dbdocs.app.DataAccess;
using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using md_dbdocs.app.Services;
using md_dbdocs.app.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace md_dbdocs.app.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        private const string configFileName = "config.json";
        private string jsonConfigPath = Environment.CurrentDirectory + "\\" + configFileName;

        private ConfigModel config;
        public ConfigModel Config { get => config; set { config = value; base.OnPropertyChanged(); } }

        public ObservableCollection<ConfigValidationModel> Validation { get; set; }

        public RelayCommand ValidateCommand { get; private set; }
        public RelayCommand NavNextCommand { get; private set; }


        public ConfigViewModel()
        {
            this.Config = LoadConfig();
            this.Validation = new ObservableCollection<ConfigValidationModel>();
            this.ValidateCommand = new RelayCommand(ValidateExecute, ValidateCanExecute);
            this.NavNextCommand = new RelayCommand(NavNextExecute, NavNextCanExecute);
        }

        private void NavNextExecute(object obj)
        {
            ViewNavigationService.ViewNav.Navigate(new DetailsView());
        }

        private bool NavNextCanExecute(object obj)
        {
            return true;
        }

        private bool ValidateCanExecute(object obj) => true;

        private void ValidateExecute(object obj)
        {
            const string cmdNode = "node -v";
            const string cmdNpm = "npm -v";
            const string cmdMdi = "npm list -g markdown-include";

            this.Validation.Clear();

            // check database connection
            this.Validation.Add(ValidateDbConnection());

            // check sql root path
            // check markdown-include config file

            // check prerequisite apps
            this.Validation.Add(ValidateApps("Node.js", cmdNode));
            this.Validation.Add(ValidateApps("npm", cmdNpm));
            this.Validation.Add(ValidateApps("markdown-include", cmdMdi));
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

            using (var cnx = new SqlServerDataAccess(Config))
            {
                vldt.ValidationItem = "Database connection.";
                
                try
                {
                    cnx.ConnectToDb();
                    vldt.ExtMessage = "Connected.";
                }
                catch (Exception ex)
                {
                    vldt.ExtMessage = ex.Message;
                }

                vldt.IsValid = cnx.IsConnected;
            }
            return vldt;
        }

        private ConfigValidationModel ValidateApps(string appName, string cmdParameter)
        {            
            string appV = CmdHelper.GetCmdOutput(cmdParameter);
            bool gotApp = !string.IsNullOrEmpty(appV);

            var appVldt = new ConfigValidationModel();
            appVldt.ValidationItem = appName;
            appVldt.IsValid = gotApp;
            appVldt.ExtMessage = appV;

            return appVldt;
        }

    }
}
