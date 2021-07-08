using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class ObjectChildModel
    {
        public string Type { get; set; }    // column / parameter
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsServer { get; set; }
        public bool IsProject { get; set; }
        public string ServerDesc { get; set; }
        public string ProjectDesc { get; set; }
    }
}
