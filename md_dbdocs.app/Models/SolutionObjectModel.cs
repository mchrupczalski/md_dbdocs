using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class SolutionObjectModel
    {
        public ProjectObjectModel ProjectObjectModel { get; set; }
        public ServerObjectParentModel ServerObjectModel { get; set; }

        public bool IsProject { get => ProjectObjectModel != null ? true : false; }
        public bool IsServer { get => ServerObjectModel != null ? true : false; }
    }
}
