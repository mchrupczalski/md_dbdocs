using Newtonsoft.Json;

namespace DbDocsGenerator.Models
{
    internal class AuthorModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Initials")]
        public string Initials { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }
    }
}
