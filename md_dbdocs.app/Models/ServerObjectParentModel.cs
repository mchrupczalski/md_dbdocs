using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class ServerObjectParentModel
    {
        public string SchemaId { get; set; }
        public string ObjectId { get; set; }
        public string UserTypeId { get; set; }
        public string SchemaName { get; set; }
        public string ObjectName { get; set; }
        public string ObjectTypeId { get; set; }
        public string ObjectTypeDesc { get; set; }
        public DateTime ObjectCreateDate { get; set; }
        public DateTime ObjectModDate { get; set; }
        public string ExtendedDesc { get; set; }
        public List<ServerObjectChildModel> ChildObjects { get; set; }
    }
}
