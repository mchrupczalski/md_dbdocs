using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace md_dbdocs.app.Models.YamlModel
{
    public class HeaderCommentModel
    {
        [YamlMember(Alias = "File_Name")]
        public string File_Name { get; set; }

        [YamlMember(Alias = "Created")]
        public DatesInfoModel Created { get; set; }

        [YamlMember(Alias = "Author")]
        public AuthorModel Author { get; set; }

        [YamlMember(Alias = "Last_Modified")]
        public DatesInfoModel LastModified { get; set; }

        [YamlMember(Alias = "Modified_By")]
        public AuthorModel ModifiedBy { get; set; }

        [YamlMember(Alias = "Module")]
        public string Module { get; set; }

        [YamlMember(Alias = "Schema_Name")]
        public string SchemaName { get; set; }

        [YamlMember(Alias = "Object_Name")]
        public string ObjectName { get; set; }

        [YamlMember(Alias = "Description")]
        public string Description { get; set; }

        [YamlMember(Alias = "Fields")]
        public Dictionary<string, string> Fields { get; set; }

        [YamlMember(Alias = "Parameters")]
        public Dictionary<string, string> Parameters { get; set; }

        [YamlMember(Alias = "Return_Value")]
        public string ReturnValue { get; set; }

        [YamlMember(Alias = "Programming_Notes")]
        public List<string> Notes { get; set; }


        public string Diagram { get; set; }
        public string ChangeLog { get; set; }

        public HeaderCommentModel()
        {
            // initialize empty dictionaries and lists
            this.Fields = new Dictionary<string, string>();
            this.Parameters = new Dictionary<string, string>();
            this.Notes = new List<string>();
        }
    }
}
