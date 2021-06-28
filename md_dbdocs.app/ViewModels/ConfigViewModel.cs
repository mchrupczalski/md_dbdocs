using md_dbdocs.app.Helpers;
using md_dbdocs.app.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        private const string configFileName = "config.json";
        private string jsonConfigPath = Environment.CurrentDirectory + "\\" + configFileName;

        private ConfigModel config;
        public ConfigModel Config { get => config; set { config = value; base.OnPropertyChanged(); } }



        public ConfigViewModel()
        {
            this.Config = LoadConfig();
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
    }
}
