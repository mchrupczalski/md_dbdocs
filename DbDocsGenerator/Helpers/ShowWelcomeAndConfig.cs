using DbDocsGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.Helpers
{
    internal static class ShowWelcomeAndConfig
    {
        internal static string WelcomeMessage(ConfigModel configModel)
        {
            string output = $"Welcome to dbdocs.\n" +
                            $"Current configuration:\n" +
                            $"\t|   Parameter    | Value \n" +
                            $"\t|----------------|--------------------\n" +
                            $"\t| Server         | { configModel.Server }\n" +
                            $"\t| Database       | { configModel.DataBase }\n" +
                            $"\t| WinAuth        | { configModel.UseWindowsAuth }\n" +
                            $"\t| SqlUserName    | { configModel.SqlUserName }\n" +
                            $"\t| SqlPassword    | { configModel.SqlPassword }\n" +
                            $"\t| MdConfig       | { configModel.MarkdownIncludeConfigFilePath }\n" +
                            $"\t| SqlProjectRoot | { configModel.SqlProjectRootPath }\n";

            return output;
        }


    }
}
