using DbDocsGenerator.Models;
using System;
using System.IO;

namespace DbDocsGenerator.Helpers
{
    internal static class JsonConfigHelper
    {
        internal static ConfigModel GetConfigModel()
        {
            // get and read json config file
            string jsonConfig = GetJsonTextFromConfig();
            ConfigModel configModel;

            using (var s = new JsonSerializer())
            {
                configModel = s.FromJson<ConfigModel>(jsonConfig);
            }

            return configModel;
        }

        private static string GetJsonTextFromConfig()
        {
            const string configFileName = "config.json";
            string jsonConfigPath = Environment.CurrentDirectory + "\\" + configFileName;

            StreamReader reader = new StreamReader(jsonConfigPath);
            string jsonText = reader.ReadToEnd();
            return jsonText;
        }
    }
}
