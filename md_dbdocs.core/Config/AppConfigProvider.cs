using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.core.Config
{
    public static class AppConfigProvider
    {
        private static AppConfigModel _appConfig;
        private static string _configFilePath;

        public static AppConfigModel AppConfig { get => _appConfig; set => _appConfig = value; }

        private static string GetConfigFilePath() => _configFilePath ?? (_configFilePath = Helpers.DialogHelper.GetFilePath());
        private static void SetConfigFilePath(string value) => _configFilePath = value;

        /// <summary>
        /// Creates config model from input.
        /// </summary>
        /// <param name="appConfigModel">The application configuration model.</param>
        public static void CreateFromInput(AppConfigModel appConfigModel) => AppConfig = appConfigModel;

        /// <summary>
        /// Loads from JSON file.
        /// </summary>
        public static void LoadFromFile()
        {
            System.Windows.MessageBox.Show(GetConfigFilePath());
        }

        /// <summary>
        /// Saves to JSON file.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void SaveToFile()
        {
            throw new NotImplementedException();
        }
    }
}
