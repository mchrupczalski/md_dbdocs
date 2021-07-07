using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class ServerObjectChildParameterModel
    {
        public int ParameterId { get; set; }
        public string ParameterName { get; set; }
        public string DataType { get; set; }
        public int MaxLen { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsOutput { get; set; }
        public bool IsReadOnly { get; set; }
        public string DefaultVal { get; set; }
        public string ExtendedDesc { get; set; }
    }
}
