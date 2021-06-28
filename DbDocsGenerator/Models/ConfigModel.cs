using Newtonsoft.Json;

namespace DbDocsGenerator.Models
{
    internal class ConfigModel
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("dataBase")]
        public string DataBase { get; set; }

        [JsonProperty("useWindowsAuth")]
        public bool UseWindowsAuth { get; set; }

        [JsonProperty("sqlUserName")]
        public string SqlUserName { get; set; }

        [JsonProperty("sqlPassword")]
        public string SqlPassword { get; set; }

        [JsonProperty("markdownIncludeConfigFilePath")]
        public string MarkdownIncludeConfigFilePath { get; set; }

        [JsonProperty("sqlProjectRootPath")]
        public string SqlProjectRootPath { get; set; }
    }
}