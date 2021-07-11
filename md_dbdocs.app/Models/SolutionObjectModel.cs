using md_dbdocs.app.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Models
{
    public class SolutionObjectModel : BindableBase
    {
        private bool _isReviewed;

        public ProjectObjectModel ProjectObjectModel { get; set; }
        public ServerObjectParentModel ServerObjectModel { get; set; }

        public bool IsProject { get => ProjectObjectModel != null ? true : false; }
        public bool IsServer { get => ServerObjectModel != null ? true : false; }
        public bool IsReviewed { get => _isReviewed; set { _isReviewed = value; base.OnPropertyChanged(); } }
    }
}
