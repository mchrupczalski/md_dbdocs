using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.lib.Config
{
    public static class AppConfigProvider
    {
        private static AppConfigModel _appConfig;

        public static AppConfigModel AppConfig { get => _appConfig; set => _appConfig = value; }

        public static void SaveToFile()
        {
            throw new NotImplementedException();
        }

        public static string ConfigFilePath { get; set; }

        public static void CreateFromInput(AppConfigModel appConfigModel) => AppConfig = appConfigModel;

        public static void LoadFromFile()
        {

        }

    }
}