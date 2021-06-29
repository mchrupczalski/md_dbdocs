using YamlDotNet.Serialization;

namespace md_dbdocs.app.Models.YamlModel
{
    public class DatesInfoModel
    {
        [YamlMember(Alias = "WeekDay")]
        public string WeekDay { get; set; }

        [YamlMember(Alias = "Date")]
        public string Date { get; set; }
    }
}
