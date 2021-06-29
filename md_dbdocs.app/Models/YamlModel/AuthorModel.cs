using YamlDotNet.Serialization;

namespace md_dbdocs.app.Models.YamlModel
{
    public class AuthorModel
    {
        [YamlMember(Alias = "Name")]
        public string Name { get; set; }

        [YamlMember(Alias = "Initials")]
        public string Initials { get; set; }

        [YamlMember(Alias = "Email")]
        public string Email { get; set; }
    }
}
