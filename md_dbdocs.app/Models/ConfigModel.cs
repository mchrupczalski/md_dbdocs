using md_dbdocs.app.Helpers;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace md_dbdocs.app.Models
{
    public class ConfigModel : BindableBase
    {
        private bool useWindowsAuth;
        private bool useSqlLogin;

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("dataBase")]
        public string DataBase { get; set; }

        [JsonProperty("useWindowsAuth")]
        public bool UseWindowsAuth { get => useWindowsAuth; set { useWindowsAuth = value; WinAuthChange(); } }

        [JsonProperty("sqlUserName")]
        public string SqlUserName { get; set; }

        [JsonProperty("sqlPassword")]
        public string SqlPassword { get; set; }

        [JsonProperty("markdownIncludeConfigFilePath")]
        public string MarkdownIncludeConfigFilePath { get; set; }

        [JsonProperty("sqlProjectRootPath")]
        public string SqlProjectRootPath { get; set; }

        public bool UseSqlLogin { get => useSqlLogin; set { useSqlLogin = value; base.OnPropertyChanged(); } }

        public void WinAuthChange([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            this.UseSqlLogin = !UseWindowsAuth;
        }
    }
}