using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class ServerObjectChildColumnModel
    {
        public int ColumnId { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public int MaxLen { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }
        public string DefaultVal { get; set; }
        public string ExtendedDesc { get; set; }
        public bool IsPK { get; set; }
        public bool IsFK { get; set; }
    }
}