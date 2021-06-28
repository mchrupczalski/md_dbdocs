using DbDocsGenerator.Models;
using System;
using System.IO;

namespace DbDocsGenerator.Helpers
{
    internal class ConfigHelper
    {
        private ConfigModel Config { get; set; }

        private const string configFileName = "config.json";
        private string jsonConfigPath = Environment.CurrentDirectory + "\\" + configFileName;

        /// <summary>
        /// Gets the configuration model.
        /// </summary>
        /// <returns></returns>
        internal ConfigModel GetConfigModel()
        {
            ConfigConsole();
            return this.Config;
        }


        /// <summary>
        /// Shows options for config screen
        /// </summary>
        private void ConfigConsole()
        {
            string userInput = "";
            bool configChanged = false;

            // load config from JSON
            Config = LoadConfig();

            while (userInput != "0")
            {
                Console.Clear();

                // show current settings
                Console.WriteLine(ShowConfig(Config));

                string options = $"Enter parameter ID to modify or 0 to continue with current setup.";
                Console.WriteLine(options);
                Console.Write("Option: ");
                userInput = Console.ReadLine();
                // edit option and reload
                bool optionChanged = ChangeConfig(userInput);
                configChanged = !configChanged ? optionChanged : configChanged;
            }

            // save to JSON if config changed
            if (configChanged)
            {
                SaveConfig();
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        private void SaveConfig()
        {
            string json;
            using (var s = new JsonSerializer())
            {
                json = s.ToJson(Config);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserialize config file from JSON
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates table to show to user
        /// </summary>
        /// <param name="configModel">current config</param>
        /// <returns></returns>
        private string ShowConfig(ConfigModel configModel)
        {
            string msg = $"Current configuration:\n" +
                         $"\t| ID |   Parameter    | Option Id \n" +
                         $"\t|----|----------------|--------------------\n" +
                         $"\t|  1 | Server         | { configModel.Server }\n" +
                         $"\t|  2 | Database       | { configModel.DataBase }\n" +
                         $"\t|  3 | WinAuth        | { configModel.UseWindowsAuth }\n" +
                         $"\t|  4 | SqlUserName    | { configModel.SqlUserName }\n" +
                         $"\t|  5 | SqlPassword    | { configModel.SqlPassword }\n" +
                         $"\t|  6 | MdConfig       | { configModel.MarkdownIncludeConfigFilePath }\n" +
                         $"\t|  7 | SqlProjectRoot | { configModel.SqlProjectRootPath }\n" +
                         $"\t|----|----------------|--------------------\n" +
                         $"\t|  0 | CONTINUE       |\n";

            return msg;
        }

        /// <summary>
        /// Updates current config model on selected option
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private bool ChangeConfig(string option)
        {
            bool changed = false;

            switch (option)
            {
                case "0":
                    return false;
                case "1":
                    Config.Server = GetChangeConfigVal("Server");
                    changed = true;
                    break;
                case "2":
                    Config.DataBase = GetChangeConfigVal("Database");
                    changed = true;
                    break;
                case "3":
                    Console.WriteLine($"This parameter cannot be modified directly, either add or remove sql login details to change it");
                    Console.WriteLine($"Press any key to continue.");
                    Console.ReadLine();
                    break;
                case "4":
                    Config.SqlUserName = GetChangeConfigVal("SqlUserName");
                    Config.UseWindowsAuth = string.IsNullOrEmpty(Config.SqlUserName);
                    changed = true;
                    break;
                case "5":
                    Config.SqlPassword = GetChangeConfigVal("SqlPassword");
                    changed = true;
                    break;
                case "6":
                    Config.MarkdownIncludeConfigFilePath = GetChangeConfigVal("MdConfig");
                    changed = true;
                    break;
                case "7":
                    Config.SqlProjectRootPath = GetChangeConfigVal("SqlProjectRoot");
                    changed = true;
                    break;
                default:
                    Console.WriteLine($"#Error: { option } - was not recognizes as a valid option. Please try again.");
                    Console.WriteLine($"Press any key to continue.");
                    Console.ReadLine();
                    return false;
            }

            return changed;
        }

        /// <summary>
        /// Gets user input from console for selected config option
        /// </summary>
        /// <param name="paramName">Name of config parameter</param>
        /// <returns></returns>
        private string GetChangeConfigVal(string paramName)
        {
            Console.Write($"New value for '{ paramName }': ");
            return Console.ReadLine();
        }
    }
}
