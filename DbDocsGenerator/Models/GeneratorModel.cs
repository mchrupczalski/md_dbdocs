using DbDocsGenerator.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDocsGenerator.Models
{
    internal class GeneratorModel
    {
        public ConfigModel ConfigModel { get; set; }
        public DataAccessService DataAccess { get; set; }
    }
}
