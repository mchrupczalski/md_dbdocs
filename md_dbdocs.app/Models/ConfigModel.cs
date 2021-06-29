using md_dbdocs.app.Helpers;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace md_dbdocs.app.Models
{
    public class ConfigModel : BindableBase
    {
        private string server;
        private string dataBase;
        private bool useWindowsAuth;
        private bool useSqlLogin;
        private string sqlUserName;
        private string sqlPassword;
        private string markdownIncludeConfigFilePath;
        private string sqlProjectRootPath;

        [JsonProperty("server")]
        public string Server { get => server; set { server = value; base.OnPropertyChanged(); } }

        [JsonProperty("dataBase")]
        public string DataBase { get => dataBase; set { dataBase = value; base.OnPropertyChanged(); } }

        [JsonProperty("useWindowsAuth")]
        public bool UseWindowsAuth { get => useWindowsAuth; set { useWindowsAuth = value; WinAuthChange(); } }

        [JsonProperty("sqlUserName")]
        public string SqlUserName { get => sqlUserName; set { sqlUserName = value; base.OnPropertyChanged(); } }

        [JsonProperty("sqlPassword")]
        public string SqlPassword { get => sqlPassword; set { sqlPassword = value; base.OnPropertyChanged(); } }

        [JsonProperty("markdownIncludeConfigFilePath")]
        public string MarkdownIncludeConfigFilePath { get => markdownIncludeConfigFilePath; set { markdownIncludeConfigFilePath = value; base.OnPropertyChanged(); } }

        [JsonProperty("sqlProjectRootPath")]
        public string SqlProjectRootPath { get => sqlProjectRootPath; set { sqlProjectRootPath = value; base.OnPropertyChanged(); } }



        public bool UseSqlLogin { get => useSqlLogin; set { useSqlLogin = value; base.OnPropertyChanged(); } }



        public void WinAuthChange([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            this.UseSqlLogin = !UseWindowsAuth;
        }
    }
}