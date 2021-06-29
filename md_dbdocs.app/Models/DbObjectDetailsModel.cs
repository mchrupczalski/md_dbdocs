using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class DbObjectDetailsModel
    {
        public string ObjectSchema { get; set; }
        public string ObjectName { get; set; }
        public bool HasTagDbDocs { get; set; }
        public bool HasTagDiagram { get; set; }
        public bool HasTagChangeLog { get; set; }

    }
}
